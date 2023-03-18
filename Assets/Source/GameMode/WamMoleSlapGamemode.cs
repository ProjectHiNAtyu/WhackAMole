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

    /* デリゲート宣言（スコア変動） */
    public delegate void OnScoreUpdateDelegate( uint Score );
    /* デリゲート定義（スコア変動） */
    public OnScoreUpdateDelegate OnScoreUpdate;

    /* デリゲート宣言（もぐらを叩いた回数変動） */
    public delegate void OnMoleSlapCountUpdateDelegate( uint Count );
    /* デリゲート定義（もぐらを叩いた回数変動） */
    public OnMoleSlapCountUpdateDelegate OnMoleSlapCountUpdate;

    /* 最大制限時間 */
    [field: SerializeField, Label( "最大制限時間" ), Tooltip( "ゲーム開始時の最大制限時間" ), Range( 0 , 65535 )]
    private ushort mMaxTime;

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

    /* 情報ヘッダーUI */
    [field: SerializeField, Label( "情報ヘッダーUI" ), Tooltip( "ゲームプレイ中の情報が表示されるUI" )]
    private GameObject mpObjInfoUI;

    /* リザルトUI */
    [field: SerializeField, Label( "リザルトUI" ), Tooltip( "制限時間終了後に表示されるリザルトUI" )]
    private GameObject mpObjResultUI;

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

    /* 現在のスコア */
    private uint mCurrentScore;

    /* 現在のもぐらを叩いた回数 */
    private uint mCurrentSlapCount;

    /* リザルト中かどうか */
    private bool mbResult;

    /* インスタンスを取得する */
    public static WamMoleSlapGamemode GetInstance( )
    {
        return mpInstance;
    }

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
        /* 現在時間を最大制限時間に設定 */
        this.mCurrentTime = this.mMaxTime;

        /* フレーム時間を初期化 */
        this.mFlameTime = 0.0f;

        /* もぐら生成までの現在のインターバル時間を初期化 */
        this.mCurrentSpawnIntervalTime = 0.0f;

        /* もぐら間の現在の生成距離 */
        this.mMoleSpawnDistance = 0.0f;

        /* 現在のスコアを初期化 */
        this.mCurrentScore = 0;

        /* 現在のもぐらを叩いた回数を初期化 */
        this.mCurrentSlapCount = 0;

        /* リザルト中かどうかを初期化 */
        this.mbResult = false;

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


        /* 情報ヘッダーUIが空なら */
        if ( this.mpObjInfoUI == null )
        {
            Debug.Log( "[Error] <WamMoleSlapGamemode> Info UI is null." );
            return;
        }

        /* 情報ヘッダーUIを表示 */
        this.mpObjInfoUI.SetActive( true );

        /* リザルトUIが空なら */
        if ( this.mpObjResultUI == null )
        {
            Debug.Log( "[Error] <WamMoleSlapGamemode> Result UI is null." );
            return;
        }

        if ( this.mpObjResultUI.GetComponent<CanvasGroup>( ) == null )
        {
            Debug.Log( "[Error] <WamMoleSlapGamemode> Result UI is not add CanvasGroup component." );
            return;
        }

        /* リザルトUIを表示 */
        this.mpObjResultUI.SetActive( true );

        /* リザルトUIの当たり判定を無くす */
        this.mpObjResultUI.GetComponent<CanvasGroup>( ).blocksRaycasts = false;

        /* リザルトUIを透明度0にする */
        this.mpObjResultUI.GetComponent<CanvasGroup>( ).alpha = 0.0f;
    }

    // Update is called once per frame
    public void Update( )
    {
        /* もし現在時間が0秒以下なら */
        if ( this.mCurrentTime <= 0 )
        {
            /* 直前までリザルト中でなかった場合 */
            if ( !this.mbResult )
            {
                /* リザルト中とする */
                this.mbResult = true;

                /* 情報ヘッダーUIが空なら */
                if ( this.mpObjInfoUI == null )
                {
                    Debug.Log( "[Error] <WamMoleSlapGamemode> Info UI is null." );
                    return;
                }

                /* リザルトUIが空なら */
                if ( this.mpObjResultUI == null )
                {
                    return;
                }

                /* 情報ヘッダーUIを非表示 */
                this.mpObjInfoUI.SetActive( false );

                /* リザルトUIの当たり判定を復活させる */
                this.mpObjResultUI.GetComponent<CanvasGroup>( ).blocksRaycasts = true;

                /* リザルトUIを透明度1にする */
                this.mpObjResultUI.GetComponent<CanvasGroup>( ).alpha = 1.0f;
            }
            return;
        }

        /* 初期化していない場合 */
        if ( !this.mbInitialized )
        {
            /* 現在時間が最大制限時間になったことを通知 */
            this.OnTimerCountdown( this.mMaxTime );

            /* 現在スコアが初期化されたことを通知 */
            this.OnScoreUpdate( 0 );

            /* 現在もぐらを叩いた回数が初期化されたことを通知 */
            this.OnMoleSlapCountUpdate( 0 );

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

            /* 現在時間が更新されたことを通知する */
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

    /* スコアを加算する */
    public void AddScore( ushort Score )
    {
        /* 現在のスコアに倒されたもぐらのスコアを加算 */
        this.mCurrentScore += Score;

        /* スコアが変動したことを通知 */
        this.OnScoreUpdate( this.mCurrentScore );

        /* 現在のもぐらを叩いた回数を加算 */
        this.mCurrentSlapCount++;

        /* もぐらを叩いた回数が変動したことを通知 */
        this.OnMoleSlapCountUpdate( this.mCurrentSlapCount );
    }
}
