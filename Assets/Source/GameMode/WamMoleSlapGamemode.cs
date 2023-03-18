using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class WamMoleSlapGamemode : MonoBehaviour
{

    /* デリゲート宣言（タイマーカウントダウン） */
    public delegate void OnTimerCountdownDelegate( ushort Time );
    /* デリゲート定義（タイマーカウントダウン） */
    public OnTimerCountdownDelegate OnTimerCountdown;

    /* インスタンスを取得する */
    public static WamMoleSlapGamemode GetInstance( ) { return mpInstance; }

    /* 最大制限時間 */
    [field: SerializeField, Label( "最大制限時間" ), Tooltip( "ゲーム開始時の最大制限時間" ), Range( 0 , 65535 )]
    private ushort mMaxTime;

    /* テキストメッシュプロ（叩いた回数） */
    [field: SerializeField, Label( "テキストUI（叩いた回数）" ), Tooltip( "もぐらを叩いた回数を表示するテキストUI" )]
    private TextMeshProUGUI mpTmpCurrentHitCount;

    /* テキストメッシュプロ（スコア） */
    [field: SerializeField, Label( "テキストUI（スコア）" ), Tooltip( "プレイ中の合計スコアを表示するテキストUI" )]
    private TextMeshProUGUI mpTmpCurrentScore;

    /* 背景セット */
    [field: SerializeField, Label( "背景セット" ), Tooltip( "ゲームプレイ中の背景絵" )]
    private GameObject[] mpObjBackgroundArtSets;

    /* プレイエリア */
    [field: SerializeField, Label( "プレイエリア" ), Tooltip( "もぐらが出現する範囲" )]
    private GameObject mpObjPlayArea;

    /* もぐらアタッチ先親オブジェクト */
    [field: SerializeField, Label( "もぐらアタッチ先親オブジェクト" ), Tooltip( "生成したもぐらプレハブをアタッチする親オブジェクト" )]
    private GameObject mpObjMoleAttachParent;

    /* プレハブ（もぐら） */
    [field: SerializeField, Label( "プレハブ（もぐら）" ), Tooltip( "動的ランダム生成するもぐらのプレハブ" )]
    private GameObject mpObjMole;

    /* もぐらの生成インターバル（最小） */
    [field: SerializeField, Label( "もぐらの生成インターバル（最小）" ), Tooltip( "もぐらが生成されるまでに待機が必要な最小インターバル時間" ), Range( 0.0f , 100.0f )]
    private float mMoleSpawnIntavalMin;

    /* もぐらの生成インターバル（最大延長） */
    [field: SerializeField, Label( "もぐらの生成インターバル（最大延長）" ), Tooltip( "もぐらが生成されるまでに待機が必要な最大延長インターバル時間で、最小時間に加算される" ), Range( 0.0f , 100.0f )]
    private float mMoleSpawnIntavalMaxExt;

    /* もぐら間の最小生成距離 */
    [field: SerializeField, Label( "もぐら間の最小生成距離" ), Tooltip( "前回生成されたもぐらから空ける最小距離" ), Range( 0.0f , 10000.0f )]
    private float mMoleSpawnDistanceMin;

    /* インスタンス */
    private static WamMoleSlapGamemode mpInstance;

    /* 初期化したかどうか */
    private bool mbInitialized;

    /* 現在時間 */
    private ushort mCurrentTime;

    /* フレーム時間 */
    private float mFlameTime;

    /* もぐら生成までの現在のインターバル時間 */
    private float mCurrentSpawnIntervalTime;

    /* もぐら生成までのランダム延長インターバル時間 */
    private float mRandomExtSpawnIntervalTime;

    /* もぐらの生成座標X軸 */
    private float mMoleSpawnLocationX;

    /* もぐらの生成座標Y軸 */
    private float mMoleSpawnLocationY;

    /* ランダム選択用数値 */
    private byte mRandomNumber;

    /* 生成したもぐらプレハブ */
    private GameObject mpCurrentMole;

    /* もぐら間の現在の生成距離 */
    private float mMoleSpawnDistance;

    public void Awake( )
    {
        /* インスタンスを保存 */
        if ( mpInstance == null )
        {
            mpInstance = this;
        }
    }

    // Start is called before the first frame update
    public void Start( )
    {
        /* 叩いた回数テキストを初期化する */
        this.mpTmpCurrentHitCount.SetText( "0" );

        /* スコアテキストを初期化する */
        this.mpTmpCurrentScore.SetText( "0" );

        /* 現在時間を最大制限時間に設定 */
        this.mCurrentTime = this.mMaxTime;

        /* フレーム時間を初期化 */
        this.mFlameTime = 0.0f;

        /* もぐら生成までの現在のインターバル時間を初期化 */
        this.mCurrentSpawnIntervalTime = 0.0f;

        /* もぐら間の現在の生成距離 */
        this.mMoleSpawnDistance = 0.0f;

        /* 生成したもぐらプレハブを初期化 */
        this.mpCurrentMole = null;

        /* 背景セット個数分ループ */
        for ( byte i = 0; i < this.mpObjBackgroundArtSets.Length; i++ )
        {
            /* 背景セットが空ではない時に */
            if ( this.mpObjBackgroundArtSets[i] != null )
            {
                /* 一旦全ての背景セットを非表示にする */
                this.mpObjBackgroundArtSets[i].SetActive( false );
            }
        }

        /* 事前に乱数のシード値を変更して、乱数生成の準備を行う */
        Random.InitState( System.DateTime.Now.Millisecond );

        /* ランダムで背景セットを選択 */
        this.mRandomNumber = (byte)Random.Range( 0 , ( this.mpObjBackgroundArtSets.Length - 1 ) );

        /* 背景セットが空ではない時に */
        if ( this.mpObjBackgroundArtSets[this.mRandomNumber] != null )
        {
            /* 背景セットを表示する */
            this.mpObjBackgroundArtSets[this.mRandomNumber].SetActive( true );
        }

        /* もぐら生成までのランダム延長インターバル時間を取得する */
        this.mRandomExtSpawnIntervalTime = Random.Range( 0.0f , this.mMoleSpawnIntavalMaxExt );
    }

    // Update is called once per frame
    public void Update( )
    {
        /* もし現在時間が0秒以下なら */
        if ( this.mCurrentTime <= 0 )
        {
            Debug.Log( "[Notice] <WamMoleSlapGamemode> Time up." );
            return;
        }

        /* 初期化していない場合 */
        if ( !this.mbInitialized )
        {
            /* 現在時間テキストに最大制限時間を設定する */
            this.OnTimerCountdown( this.mMaxTime );

            /* 初期化済みとする */
            this.mbInitialized = true;
        }

        /* フレーム時間を加算 */
        this.mFlameTime += Time.deltaTime;

        /* 合計フレーム時間が1秒を超えたら */
        if ( 1.0f <= this.mFlameTime )
        {
            /* 合計フレーム時間から1秒減算 */
            this.mFlameTime -= 1.0f;

            /* 現在時間を1秒分減算 */
            this.mCurrentTime--;

            /* テキストに現在時間を設定する */
            this.OnTimerCountdown( this.mCurrentTime );
        }

        /* もぐら生成までの現在のインターバル時間が、指定時間に到達していない場合 */
        if ( this.mCurrentSpawnIntervalTime < ( this.mMoleSpawnIntavalMin + this.mRandomExtSpawnIntervalTime ) )
        {
            /* インターバル時間を加算する */
            this.mCurrentSpawnIntervalTime += Time.deltaTime;
            return;
        }

        /* もぐらプレハブが空なら */
        if ( this.mpObjMole == null )
        {
            Debug.Log( "[Error] <WamMoleSlapGamemode> Mole prefab is null." );
            return;
        }

        /* プレイエリアが空なら */
        if ( this.mpObjPlayArea == null )
        {
            Debug.Log( "[Error] <WamMoleSlapGamemode> PlayArea is null." );
            return;
        }

        /* もぐらアタッチ先親オブジェクトが空なら */
        if ( this.mpObjMoleAttachParent == null )
        {
            Debug.Log( "[Error] <WamMoleSlapGamemode> Mole prefab attach parent object is null." );
            return;
        }

        /* 事前に乱数のシード値を変更して、乱数生成の準備を行う */
        Random.InitState( System.DateTime.Now.Millisecond );

        /* もぐらの生成座標をランダムで取得する */
        RectTransform rect = this.mpObjPlayArea.GetComponent<RectTransform>( );
        this.mMoleSpawnLocationX = Random.Range( ( this.mpObjPlayArea.transform.position.x - ( this.mpObjPlayArea.GetComponent<RectTransform>( ).rect.width / 2 ) ) ,
                                                ( this.mpObjPlayArea.transform.position.x + ( this.mpObjPlayArea.GetComponent<RectTransform>( ).rect.width / 2 ) ) );
        this.mMoleSpawnLocationY = Random.Range( ( this.mpObjPlayArea.transform.position.y - ( this.mpObjPlayArea.GetComponent<RectTransform>( ).rect.height / 2 ) ) ,
                                                ( this.mpObjPlayArea.transform.position.y + ( this.mpObjPlayArea.GetComponent<RectTransform>( ).rect.height / 2 ) ) );

        /* 直前に生成済みのもぐらプレハブが存在する場合 */
        if ( this.mpCurrentMole != null )
        {
            /* もぐら間の現在の生成距離を取得 */
            this.mMoleSpawnDistance = Vector2.Distance( this.mpCurrentMole.transform.position , new Vector2( this.mMoleSpawnLocationX , this.mMoleSpawnLocationY ) );
                                
            /* 前回生成したもぐらと、今回生成する予定のもぐらの距離が、指定距離未満の場合 */
            if ( this.mMoleSpawnDistance < this.mMoleSpawnDistanceMin )
            {
                /* もう一度ランダムで座標を取らせ直す */
                return;
            }
        }

        /* ランダムで取得した座標にもぐらプレハブを生成する */
        this.mpCurrentMole = Instantiate( this.mpObjMole , new Vector2( this.mMoleSpawnLocationX , this.mMoleSpawnLocationY ) , Quaternion.identity , this.mpObjMoleAttachParent.transform );

        /* もぐらプレハブの生成が成功していない場合 */
        if ( this.mpCurrentMole == null )
        {
            Debug.Log( "[Failed] <WamMoleSlapGamemode> Mole prefab create failed." );
            return;
        }

        /* もぐら生成までの現在のインターバル時間を初期化 */
        this.mCurrentSpawnIntervalTime = 0.0f;

        /* もぐら生成までのランダム延長インターバル時間を取得する */
        this.mRandomExtSpawnIntervalTime = Random.Range( 0.0f , this.mMoleSpawnIntavalMaxExt );
    }
}
