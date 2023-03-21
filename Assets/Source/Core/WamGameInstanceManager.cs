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
    private WamMoleSlapGamemode mpGameModeManager;

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
    public WamMoleSlapGamemode GetGameModeManagerInstance( )
    {
        if ( this.mpGameModeManager == null )
        {
            Debug.Log( "[Error] <WamGameInstanceManager> GameModeManager is null." );
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
            Debug.Log( "[Error] <WamGameInstanceManager> UIManager is null." );
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
            Debug.Log( "[Error] <WamGameInstanceManager> TimeManager is null." );
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
            Debug.Log( "[Error] <WamGameInstanceManager> MoleSpawnManager is null." );
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

    }

    //------------------------------------------------------------------------------//
    //! @brief	更新処理
    //------------------------------------------------------------------------------//
    public void Update( )
    {

    }
}
