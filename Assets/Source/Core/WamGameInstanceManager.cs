//------------------------------------------------------------------------------//
//!	@file   WamGameInstanceManager.cs
//!	@brief	ゲームインスタンス管理ソース
//!	@author	立浪豪
//!	@date	2023/03/20
//------------------------------------------------------------------------------//


//======================================//
//				Include					//
//======================================//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//######################################################################################//
//!								ゲームインスタンス管理クラス
//######################################################################################//

public class WamGameInstanceManager : MonoBehaviour
{
    //======================================//
    //		プライベートシリアライズ変数	//
    //======================================//

    /* ゲームモード管理 */
    [field: SerializeField, Label( "ゲームモード管理" ), Tooltip( "ゲームモード管理スクリプトが追加されているゲームオブジェクトを指定" )]
    private WamGameModeManager mpGameModeManager;

    /* UI管理 */
    [field: SerializeField, Label( "UI管理" ), Tooltip( "UI管理スクリプトが追加されているゲームオブジェクトを指定" )]
    private WamUiManager mpUIManager;

    /* 時間管理 */
    [field: SerializeField, Label( "時間管理" ), Tooltip( "時間管理スクリプトが追加されているゲームオブジェクトを指定" )]
    private WamTimeManager mpTimeManager;

    /* もぐら生成管理 */
    [field: SerializeField, Label( "もぐら生成管理" ), Tooltip( "もぐら生成管理スクリプトが追加されているゲームオブジェクトを指定" )]
    private WamMoleSpawnManager mpMoleSpawnManager;

    /* デバッグ管理 */
    [field: SerializeField, Label( "デバッグ管理" ), Tooltip( "デバッグ管理スクリプトが追加されているゲームオブジェクトを指定" )]
    private WamDebugManager mpDebugManager;


    //======================================//
    //		    プライベート変数        	//
    //======================================//

    /* インスタンス */
    private static WamGameInstanceManager mpInstance;

    /* 初回のみの処理を実行したかどうか */
    private bool mbExecFirstProcess;


    //======================================//
    //		    パブリック関数             	//
    //======================================//

    //------------------------------------------------------------------------------//
    //! @brief	ゲームインスタンス管理インスタンスを取得する
    //------------------------------------------------------------------------------//
    public static WamGameInstanceManager GetInstance( )
    {
        if ( WamGameInstanceManager.mpInstance == null )
        {
            Debug.Log( "[Error] <WamGameInstanceManager> GameInstanceManager is null." );
        }
        return WamGameInstanceManager.mpInstance;
    }
    
    //------------------------------------------------------------------------------//
    //! @brief	ゲームモード管理インスタンスを取得する
    //------------------------------------------------------------------------------//
    public WamGameModeManager GetGameModeManagerInstance( )
    {
        if ( this.mpGameModeManager == null )
        {
            this.GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamGameInstanceManager" , "GameModeManager is null" );
        }
        return this.mpGameModeManager;
    }

    //------------------------------------------------------------------------------//
    //! @brief	UI管理インスタンスを取得する
    //------------------------------------------------------------------------------//
    public WamUiManager GetUIManagerInstance( )
    {
        if ( this.mpUIManager == null )
        {
            this.GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamGameInstanceManager" , "UIManager is null" );
        }
        return this.mpUIManager;
    }

    //------------------------------------------------------------------------------//
    //! @brief	時間管理インスタンスを取得する
    //------------------------------------------------------------------------------//
    public WamTimeManager GetTimeManagerInstance( )
    {
        if ( this.mpTimeManager == null )
        {
            this.GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamGameInstanceManager" , "TimeManager is null" );
        }
        return this.mpTimeManager;
    }

    //------------------------------------------------------------------------------//
    //! @brief	もぐら生成管理インスタンスを取得する
    //------------------------------------------------------------------------------//
    public WamMoleSpawnManager GetMoleSpawnManagerInstance( )
    {
        if ( this.mpMoleSpawnManager == null )
        {
            this.GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamGameInstanceManager" , "MoleSpawnManager is null" );
        }
        return this.mpMoleSpawnManager;
    }

    //------------------------------------------------------------------------------//
    //! @brief	デバッグ管理インスタンスを取得する
    //------------------------------------------------------------------------------//
    public WamDebugManager GetDebugManagerInstance( )
    {
        if ( this.mpDebugManager == null )
        {
            Debug.Log( "[Error] <WamGameInstanceManager> DebugManager is null." );
        }
        return this.mpDebugManager;
    }

    //------------------------------------------------------------------------------//
    //! @brief	活動開始時に呼ばれる 
    //------------------------------------------------------------------------------//
    public void Awake( )
    {
        /* インスタンスを保存 */
        if ( WamGameInstanceManager.mpInstance == null )
        {
            WamGameInstanceManager.mpInstance = this;
        }
    }

    //------------------------------------------------------------------------------//
    //! @brief	初回更新処理の直前に呼ばれる
    //------------------------------------------------------------------------------//
    public void Start( )
    {
        /* 初期化処理 */
        this.Initialize( );
    }

    //------------------------------------------------------------------------------//
    //! @brief	更新処理
    //------------------------------------------------------------------------------//
    public void Update( )
    {
        /* 初回のみの処理を実行していない場合 */
        if ( !this.mbExecFirstProcess )
        {
            this.GetTimeManagerInstance( ).ExecFirstProcess( );
            this.GetGameModeManagerInstance( ).ExecFirstProcess( );

            /* 初回のみの処理を実行したとする */
            this.mbExecFirstProcess = true;
        }
    }


    //======================================//
    //		    プライベート関数            //
    //======================================//

    //------------------------------------------------------------------------------//
    //! @brief	初期化処理
    //------------------------------------------------------------------------------//
    private void Initialize( )
    {
        /* 各種変数初期化 */
        this.mbExecFirstProcess = false;    /* 初回のみの処理を実行したかどうか */

        this.GetGameModeManagerInstance( ).Initialize( );
        this.GetMoleSpawnManagerInstance( ).Initialize( );
        this.GetTimeManagerInstance( ).Initialize( );
    }
}
