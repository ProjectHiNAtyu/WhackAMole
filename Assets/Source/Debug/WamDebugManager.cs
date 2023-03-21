//------------------------------------------------------------------------------//
//!	@file   WamDebugManager.cs
//!	@brief	デバッグ管理ソース
//!	@author	立浪豪
//!	@date	2023/03/20
//------------------------------------------------------------------------------//


//======================================//
//				Define					//
//======================================//

#define DEBUG


#if DEBUG


//======================================//
//				Include					//
//======================================//

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


//######################################################################################//
//!								デバッグ管理クラス
//######################################################################################//

public class WamDebugManager : MonoBehaviour
{
    //======================================//
    //			パブリック列挙型			//
    //======================================//

    /* ログ種類 */
    public enum EWamLogType
    {
        Notice  = 0, /* ログ種類 通知 */
        Warning = 1, /* ログ種類 警告 */
        Error   = 2, /* ログ種類 エラー */
        Failed  = 3, /* ログ種類 失敗 */
        Success = 4, /* ログ種類 成功 */
    }


    //======================================//
    //			プライベート変数			//
    //======================================//

    /* デバッグログで表示するメッセージ内容 */
    private string mLogMessage;


    //======================================//
    //		    パブリック関数             	//
    //======================================//

    //------------------------------------------------------------------------------//
    //! @brief	活動開始時に呼ばれる 
    //------------------------------------------------------------------------------//
    public void Awake( )
    {

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

    //------------------------------------------------------------------------------//
    //! @brief	デバッグログを表示する
    //!
    //! @param	Message	表示するメッセージ内容
    //!
    //! @return 無し
    //------------------------------------------------------------------------------//
    [Conditional( "DEBUG" )]
    public void ShowDebugLog( string Message )
    {
        UnityEngine.Debug.Log( Message );
    }

    //------------------------------------------------------------------------------//
    //! @brief	デバッグログをテンプレートに沿って表示する
    //!
    //! @param	eType	    このログの種類
    //! @param	ClassName	ログの発生元クラス名
    //! @param	Message	    メッセージ本文
    //!
    //! @return 無し
    //------------------------------------------------------------------------------//
    [Conditional( "DEBUG" )]
    public void ShowDebugLogTemplate( EWamLogType eType , string ClassName , string Message )
    {
        /* 表示するメッセージ内容を初期化 */
        this.mLogMessage = string.Empty;

        /* ログ種類の構築 */
        this.mLogMessage += "[";
        switch ( eType )
        {
            case EWamLogType.Notice:    this.mLogMessage += "Notice";    break;
            case EWamLogType.Warning:   this.mLogMessage += "Warning";   break;
            case EWamLogType.Error:     this.mLogMessage += "Error";     break;
            case EWamLogType.Failed:    this.mLogMessage += "Failed";    break;
            case EWamLogType.Success:   this.mLogMessage += "Success";   break;
        }
        this.mLogMessage += "] ";

        /* 呼び出し元クラス名の構築 */
        this.mLogMessage += "<";
        this.mLogMessage += ClassName;
        this.mLogMessage += "> ";

        /* 本文の構築 */
        this.mLogMessage += Message;
        this.mLogMessage += ".";

        /* デバッグログを表示 */
        UnityEngine.Debug.Log( this.mLogMessage );
    }
}

#endif