//------------------------------------------------------------------------------//
//!	@file   WamGameModeManager.cs
//!	@brief	ゲームモード管理ソース
//!	@author	立浪豪
//!	@date	2023/03/17
//------------------------------------------------------------------------------//


//======================================//
//				Include					//
//======================================//

using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


//######################################################################################//
//!								ゲームモード管理クラス
//######################################################################################//

public class WamGameModeManager : MonoBehaviour
{
    //======================================//
    //				 デリゲート				//
    //======================================//

    /* デリゲート宣言（スコア変動） */
    public delegate void OnScoreUpdateDelegate( uint Score );
    /* デリゲート定義（スコア変動） */
    public OnScoreUpdateDelegate OnScoreUpdate;

    /* デリゲート宣言（もぐらを叩いた回数変動） */
    public delegate void OnMoleSlapCountUpdateDelegate( uint Count );
    /* デリゲート定義（もぐらを叩いた回数変動） */
    public OnMoleSlapCountUpdateDelegate OnMoleSlapCountUpdate;


    //======================================//
    //		    プライベート変数        	//
    //======================================//

    /* 現在のスコア */
    private uint mCurrentScore;

    /* 現在のもぐらを叩いた回数 */
    private uint mCurrentSlapCount;


    //======================================//
    //		    パブリック関数             	//
    //======================================//

    //------------------------------------------------------------------------------//
    //! @brief	初期化処理
    //------------------------------------------------------------------------------//
    public void Initialize( )
    {
        /* 各種変数初期化 */
        this.mCurrentScore      = 0;    /* 現在のスコア */
        this.mCurrentSlapCount  = 0;    /* 現在のもぐらを叩いた回数 */
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
        /* もし現在時間が0秒以下なら */
        if ( WamGameInstanceManager.GetInstance( ).GetTimeManagerInstance( ).IsTimeOver )
        {
            return;
        }

        /* ランダムでもぐらを生成する */
        WamGameInstanceManager.GetInstance( ).GetMoleSpawnManagerInstance( ).SpawnRandomMole( );
    }

    //------------------------------------------------------------------------------//
    //! @brief	スコアを加算する
    //------------------------------------------------------------------------------//
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
