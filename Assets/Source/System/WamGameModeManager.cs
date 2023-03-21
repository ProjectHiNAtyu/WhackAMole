using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WamGameModeManager : MonoBehaviour
{
    /* デリゲート宣言（スコア変動） */
    public delegate void OnScoreUpdateDelegate( uint Score );
    /* デリゲート定義（スコア変動） */
    public OnScoreUpdateDelegate OnScoreUpdate;

    /* デリゲート宣言（もぐらを叩いた回数変動） */
    public delegate void OnMoleSlapCountUpdateDelegate( uint Count );
    /* デリゲート定義（もぐらを叩いた回数変動） */
    public OnMoleSlapCountUpdateDelegate OnMoleSlapCountUpdate;

    /* 背景セット */
    [field: SerializeField, Label( "背景セット" ), Tooltip( "ゲームプレイ中の背景絵" )]
    private GameObject[] mpObjBackgroundArtSets;

    /* 情報ヘッダーUI */
    [field: SerializeField, Label( "情報ヘッダーUI" ), Tooltip( "ゲームプレイ中の情報が表示されるUI" )]
    private GameObject mpObjInfoUI;

    /* リザルトUI */
    [field: SerializeField, Label( "リザルトUI" ), Tooltip( "制限時間終了後に表示されるリザルトUI" )]
    private GameObject mpObjResultUI;

    /* リトライボタンUI */
    [field: SerializeField, Label( "リトライボタンUI" ), Tooltip( "リザルトUI内にあるリトライボタンUI")]
    private Button mpButtonRetryUI;

    /* ランダム選択用数値 */
    private byte mRandomNumber;

    /* 現在のスコア */
    private uint mCurrentScore;

    /* 現在のもぐらを叩いた回数 */
    private uint mCurrentSlapCount;

    /* リザルト中かどうか */
    private bool mbResult;

    public void Awake( )
    {
        /* リトライボタンUIが空なら */
        if ( this.mpButtonRetryUI == null )
        {
            Debug.Log( "[Error] <WamMoleSlapGamemode> Retry button UI is null." );
            return;
        }

        this.mpButtonRetryUI.onClick.AsObservable( ).Subscribe( _ => this.Initialize( ) );
    }

    // Start is called before the first frame update
    public void Start( )
    {
    }

    /* ゲームモードの初期化をする */
    public void Initialize( )
    {
        /* 各種変数初期化 */
        this.mCurrentScore              = 0;        /* 現在のスコア */
        this.mCurrentSlapCount          = 0;        /* 現在のもぐらを叩いた回数 */

        this.mbResult                   = false;    /* リザルト中かどうか */


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

        /* ランダムで背景セットを選択 */
        this.mRandomNumber = (byte)Random.Range( 0 , ( this.mpObjBackgroundArtSets.Length - 1 ) );

        /* 背景セットが空ではない時に */
        if ( this.mpObjBackgroundArtSets[this.mRandomNumber] != null )
        {
            /* 背景セットを表示する */
            this.mpObjBackgroundArtSets[this.mRandomNumber].SetActive( true );
        }


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

    //------------------------------------------------------------------------------//
    //! @brief	初回のみの処理を実行する
    //------------------------------------------------------------------------------//
    public void ExecFirstProcess( )
    {
        /* 現在スコアが初期化されたことを通知 */
        this.OnScoreUpdate( 0 );

        /* 現在もぐらを叩いた回数が初期化されたことを通知 */
        this.OnMoleSlapCountUpdate( 0 );
    }

    // Update is called once per frame
    public void Update( )
    {
        /* もし現在時間が0秒以下なら */
        if ( WamGameInstanceManager.GetInstance( ).GetTimeManagerInstance( ).IsTimeOver )
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

        /* ランダムでもぐらを生成する */
        WamGameInstanceManager.GetInstance( ).GetMoleSpawnManagerInstance( ).SpawnRandomMole( );
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
