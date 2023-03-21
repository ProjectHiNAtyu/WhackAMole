//------------------------------------------------------------------------------//
//!	@file   WamTimeManager.cs
//!	@brief	時間管理ソース
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
//!								時間管理クラス
//######################################################################################//

public class WamTimeManager : MonoBehaviour
{
    //======================================//
    //				 デリゲート				//
    //======================================//

    /* デリゲート宣言（タイマーカウントダウン） */
    public delegate void OnTimerCountdownDelegate( ushort Time );
    /* デリゲート定義（タイマーカウントダウン） */
    public OnTimerCountdownDelegate OnTimerCountdown;


    //======================================//
    //		プライベートシリアライズ変数	//
    //======================================//

    /* 最大制限時間 */
    [field: SerializeField, Label( "最大制限時間" ), Tooltip( "ゲーム開始時の最大制限時間" ), Range( 0 , 65535 )]
    private ushort mMaxTime;


    //======================================//
    //		    プライベート変数        	//
    //======================================//

    /* 現在時間 */
    private ushort mCurrentTime;

    /* フレーム時間 */
    private float mFlameTime;

    /* 制限時間をタイムオーバーしたかどうか */
    private bool mbTimeOver;


    //======================================//
    //		    パブリックアクセサ          //
    //======================================//

    /* 制限時間をタイムオーバーしたかどうか */
    public bool IsTimeOver { get { return this.mbTimeOver; } }


    //======================================//
    //		    パブリック関数             	//
    //======================================//

    //------------------------------------------------------------------------------//
    //! @brief	初期化処理
    //------------------------------------------------------------------------------//
    public void Initialize( )
    {
        /* 各種変数初期化 */
        this.mFlameTime = 0.0f;     /* フレーム時間 */
        this.mbTimeOver = false;    /* 制限時間をタイムオーバーしたかどうか */

        /* 現在時間を最大制限時間に設定 */
        this.mCurrentTime = this.mMaxTime;
    }

    //------------------------------------------------------------------------------//
    //! @brief	初回のみの処理を実行する
    //------------------------------------------------------------------------------//
    public void ExecFirstProcess( )
    {
        /* 現在時間が最大制限時間になったことを通知 */
        this.OnTimerCountdown( this.mMaxTime );
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
        if ( this.mCurrentTime <= 0 )
        {
            /* タイムオーバーしたとする */
            if ( !this.mbTimeOver )
            {
                this.mbTimeOver = true;
            }

            /* タイムオーバー中なのでこれ以上時間のカウントは行わない */
            return;
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
    }
}
