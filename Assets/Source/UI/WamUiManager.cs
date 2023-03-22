//------------------------------------------------------------------------------//
//!	@file   WamUiManager.cs
//!	@brief	UI管理ソース
//!	@author	立浪豪
//!	@date	2023/03/20
//------------------------------------------------------------------------------//


//======================================//
//				Include					//
//======================================//

using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;


//######################################################################################//
//!								UI管理クラス
//######################################################################################//

public class WamUiManager : MonoBehaviour
{
    //======================================//
    //		プライベートシリアライズ変数	//
    //======================================//

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
    [field: SerializeField, Label( "リトライボタンUI" ), Tooltip( "リザルトUI内にあるリトライボタンUI" )]
    private Button mpButtonRetryUI;


    //======================================//
    //		    プライベート変数        	//
    //======================================//

    /* リザルト中かどうか */
    private bool mbResult;

    /* ランダム選択用数値 */
    private byte mRandomNumber;


    //======================================//
    //		    パブリック関数             	//
    //======================================//

    //------------------------------------------------------------------------------//
    //! @brief	初期化処理
    //------------------------------------------------------------------------------//
    public void Initialize( )
    {
        /* 各種変数初期化 */
        this.mbResult = false;    /* リザルト中かどうか */


        /* 背景セットが１個以上存在している時に */
        if ( 0 < this.mpObjBackgroundArtSets.Length )
        {
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
        }


        /* 情報ヘッダーUIが空なら */
        if ( this.mpObjInfoUI == null )
        {
            WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamUiManager" , "Info UI is null" );
            return;
        }

        /* リザルトUIが空なら */
        if ( this.mpObjResultUI == null )
        {
            WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamUiManager" , "Result UI is null" );
            return;
        }

        /* リザルトUIがキャンバスグループコンポーネントを持っていない場合 */
        if ( this.mpObjResultUI.GetComponent<CanvasGroup>( ) == null )
        {
            WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamUiManager" , "Result UI is not add CanvasGroup component" );
            return;
        }


        /* 情報ヘッダーUIを表示 */
        this.mpObjInfoUI.SetActive( true );

        /* リザルトUIを表示 */
        this.mpObjResultUI.SetActive( true );

        /* リザルトUIの当たり判定を無くす */
        this.mpObjResultUI.GetComponent<CanvasGroup>( ).blocksRaycasts = false;

        /* リザルトUIを透明度0にする */
        this.mpObjResultUI.GetComponent<CanvasGroup>( ).alpha = 0.0f;
    }

    //------------------------------------------------------------------------------//
    //! @brief	活動開始時に呼ばれる 
    //------------------------------------------------------------------------------//
    public void Awake( )
    {
        /* リトライボタンUIが空なら、エラーログ出して処理しない */
        if ( this.mpButtonRetryUI == null )
        {
            WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamUiManager" , "Retry button UI is null" );
            return;
        }

        /* ボタンクリックイベントをバインド */
        this.mpButtonRetryUI.onClick.AsObservable( ).Subscribe( _ => WamGameInstanceManager.GetInstance( ).Initialize( ) );
    }

    //------------------------------------------------------------------------------//
    //! @brief	初回更新処理の直前に呼ばれる
    //------------------------------------------------------------------------------//
    public void Start( )
    {
        /* 実行に必要な設定条件を確認する（メッセージ付き） */
        this.CheckRequiredConditions( bMessage: true );
    }

    //------------------------------------------------------------------------------//
    //! @brief	更新処理
    //------------------------------------------------------------------------------//
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

                /* 実行に必要な条件が設定されていない場合、ログを表示して生成できなかった結果を返す */
                if ( !this.CheckRequiredConditions( ) )
                {
                    WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Failed , "WamUiManager" , "Result scene change conditions is not match" );
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
    }


    //======================================//
    //		    プライベート関数            //
    //======================================//

    //------------------------------------------------------------------------------//
    //! @brief	実行に必要な設定条件を確認する
    //!
    //! @param	bMessage	デバッグメッセージを表示するかどうか
    //!
    //! @return 実行に必要な条件が設定されていたかどうか
    //------------------------------------------------------------------------------//
    private bool CheckRequiredConditions( bool bMessage = false )
    {
        /* 情報ヘッダーUIが空なら */
        if ( this.mpObjInfoUI == null )
        {
            /* デバッグメッセージを表示する場合のみ表示する */
            if ( bMessage )
            {
                WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamUiManager" , "Info UI is null" );
            }

            /* 実行に必要な条件が設定されていない結果を返す */
            return false;
        }

        /* リザルトUIが空なら */
        if ( this.mpObjResultUI == null )
        {
            /* デバッグメッセージを表示する場合のみ表示する */
            if ( bMessage )
            {
                WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamUiManager" , "Result UI is null" );
            }

            /* 実行に必要な条件が設定されていない結果を返す */
            return false;
        }

        /* リザルトUIがキャンバスグループコンポーネントを持っていない場合 */
        if ( this.mpObjResultUI.GetComponent<CanvasGroup>( ) == null )
        {
            /* デバッグメッセージを表示する場合のみ表示する */
            if ( bMessage )
            {
                WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamUiManager" , "Result UI is not add CanvasGroup component" );
            }

            /* 実行に必要な条件が設定されていない結果を返す */
            return false;
        }

        /* 実行に必要な全ての条件が設定されている結果を返す */
        return true;
    }
}
