//------------------------------------------------------------------------------//
//!	@file   WamCharacterMole.cs
//!	@brief	もぐらキャラクターソース
//!	@author	立浪豪
//!	@date	2023/03/18
//------------------------------------------------------------------------------//


//======================================//
//				Include					//
//======================================//

using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using Unity.VisualScripting;
using UnityEngine;


//######################################################################################//
//!								もぐらキャラクタークラス
//######################################################################################//

public class WamCharacterMole : MonoBehaviour
{
    //======================================//
    //		プライベートシリアライズ変数	//
    //======================================//

    /* タップターゲット（もぐら） */
    [field: SerializeField, Label( "タップターゲット（もぐら）" ), Tooltip( "タップで倒す事が可能なもぐらオブジェクト" )]
    private GameObject mpObjMoleTapTarget;

    /* もぐらを倒した時に貰えるスコア */
    [field: SerializeField, Label( "もぐらを倒した時に貰えるスコア" ), Tooltip( "もぐらを倒した時に貰えるスコア" ), Range( 0 , 65535 )]
    private ushort mDefeatedScore;

    /* アニメーターステート名（放置） */
    [field: SerializeField, Label( "アニメーターステート名（放置）" ), Tooltip( "もぐらが放置されている時のアニメーターステート名" )]
    private string mStateNameIdle;

    /* アニメーターステート名（終了） */
    [field: SerializeField, Label( "アニメーターステート名（終了）" ), Tooltip( "もぐらが退場か消滅した後のアニメーターステート名" )]
    private string mStateNameEnd;

    /* アニメーターパラメーター名（ステート切り替え） */
    [field: SerializeField, Label( "アニメーターパラメーター名（ステート切り替え）" ), Tooltip( "もぐらのアニメーターステートを切り替えるためのパラメーター名" )]
    private string mParameterNameStateChange;

    /* 放置ステートから遷移開始するまでの最小時間 */
    [field: SerializeField, Label( "放置ステートから遷移開始するまでの最小時間" ), Tooltip( "放置ステートから遷移開始するまでの最小時間" ), Range( 0.0f , 100.0f )]
    private float mStateIdleWaitTimeMin;

    /* 放置ステートから遷移開始するまでの最大延長時間 */
    [field: SerializeField, Label( "放置ステートから遷移開始するまでの最大延長時間" ), Tooltip( "放置ステートから遷移開始するまでの最大延長時間で、最小時間にランダムで加算される" ), Range( 0.0f , 100.0f )]
    private float mStateIdleWaitTimeMaxExt;


    //======================================//
    //		    プライベート変数        	//
    //======================================//

    /* アニメーター */
    private Animator mpAnimator;

    /* 放置ステートに入ったかどうか */
    private bool mbStateIdle;

    /* 放置ステートから遷移開始するまでの現在の待機時間 */
    private float mStateIdleWaitTime;

    /* 放置ステートから遷移開始するまでのランダム延長時間 */
    private float mStateIdleWaitTimeRandomExt;

    /* UniRxイベントハンドリング */
    private ObservableEventTrigger mpUniRxEventTrigger;

    /* もぐらが倒されたかどうか */
    private bool mbDefeat;


    //======================================//
    //		    パブリック関数             	//
    //======================================//

    //------------------------------------------------------------------------------//
    //! @brief	初回更新処理の直前に呼ばれる
    //------------------------------------------------------------------------------//
    public void Start()
    {
        /* 自身に設定されているアニメーターコンポーネントを取得 */
        this.mpAnimator = this.GetComponent<Animator>( );

        /* アニメーションが空の場合 */
        if ( this.mpAnimator == null )
        {
            Debug.Log( "[Error] <WamCharacterMole> Animator component is null." );
            return;
        }

        /* タップターゲットが空の場合 */
        if ( this.mpObjMoleTapTarget == null )
        {
            Debug.Log( "[Error] <WamCharacterMole> Tap target is null." );
            return;
        }

        /* タップターゲットからUniRxイベントハンドリングを取得 */
        this.mpUniRxEventTrigger = this.mpObjMoleTapTarget.GetComponent<ObservableEventTrigger>( );

        /* タップターゲットが空の場合 */
        if ( this.mpUniRxEventTrigger == null )
        {
            Debug.Log( "[Error] <WamCharacterMole> Tap target is not added ObservableEventTrigger component." );
            return;
        }

        /* タップターゲットがタップされた時のイベントを登録 */
        this.mpUniRxEventTrigger.OnPointerDownAsObservable( ).Subscribe( _ => this.TappedMoleEvent( ) );

        /* アニメーターステート名（放置）が空の場合 */
        if ( string.IsNullOrEmpty( this.mStateNameIdle ) )
        {
            Debug.Log( "[Error] <WamCharacterMole> Animator state idle name is null or empty." );
            return;
        }

        /* アニメーターステート名（終了）が空の場合 */
        if ( string.IsNullOrEmpty( this.mStateNameEnd ) )
        {
            Debug.Log( "[Error] <WamCharacterMole> Animator state end name is null or empty." );
            return;
        }

        /* アニメーターパラメーター名（ステート切り替え）が空の場合 */
        if ( string.IsNullOrEmpty( this.mParameterNameStateChange ) )
        {
            Debug.Log( "[Error] <WamCharacterMole> Animator parameter state change name is null or empty." );
            return;
        }

        /* 放置ステートから遷移開始するまでの現在の待機時間を初期化 */
        this.mStateIdleWaitTime = 0.0f;

        /* 事前に乱数のシード値を変更して、乱数生成の準備を行う */
        Random.InitState( System.DateTime.Now.Millisecond );

        /* 放置ステートから遷移開始するまでのランダム延長時間を取得する */
        this.mStateIdleWaitTimeRandomExt = Random.Range( 0.0f , this.mStateIdleWaitTimeMaxExt );
    }

    //------------------------------------------------------------------------------//
    //! @brief	更新処理
    //------------------------------------------------------------------------------//
    public void Update()
    {
        /* アニメーションが空の場合 */
        if ( this.mpAnimator == null )
        {
            return;
        }

        /* 放置ステートにいる場合 */
        if ( this.mpAnimator.GetCurrentAnimatorStateInfo( 0 ).IsName( this.mStateNameIdle ) )
        {
            /* 直前まで放置ステートに入っていなかった場合、入ったとする */
            if ( !this.mbStateIdle )
            {
                this.mbStateIdle = true;
            }
            /* 放置ステートに入っている時 */
            else
            {
                /* 経過時間を加算する */
                this.mStateIdleWaitTime += Time.deltaTime;

                /* 待機時間が指定時間に到達していなければ、処理しない */
                if ( this.mStateIdleWaitTime < ( this.mStateIdleWaitTimeMin + this.mStateIdleWaitTimeRandomExt ) )
                {
                    return;
                }

                /* 退場ステートに切り替える */
                this.mpAnimator.SetInteger( this.mParameterNameStateChange , 1 );
            }
        }
        /* 終了ステートにいる場合 */
        else if ( this.mpAnimator.GetCurrentAnimatorStateInfo( 0 ).IsName( this.mStateNameEnd ) )
        {
            /* 自分自身を削除する */
            Destroy( this.gameObject );
        }
    }

    //------------------------------------------------------------------------------//
    //! @brief	タップターゲットがタップされた時のイベント
    //------------------------------------------------------------------------------//
    public void TappedMoleEvent( )
    {
        /* 既にもぐらがタップされて倒されている場合、処理しない */
        if ( this.mbDefeat )
        {
            return;
        }

        /* もぐらが倒されたとする */
        this.mbDefeat = true;

        /* 消失ステートに切り替える */
        this.mpAnimator.SetInteger( this.mParameterNameStateChange , 2 );

        /* ゲームモードクラスのインスタンスが空なら */
        if ( WamGameInstanceManager.GetInstance( ).GetGameModeManagerInstance( ) == null )
        {
            WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamCharacterMole" , "GameModeManger instance is null" );
            return;
        }

        /* スコアを加算する */
        WamGameInstanceManager.GetInstance( ).GetGameModeManagerInstance( ).AddScore( this.mDefeatedScore );
    }
}
