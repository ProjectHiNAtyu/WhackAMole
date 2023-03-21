//------------------------------------------------------------------------------//
//!	@file   WamEnemySpawnManager.cs
//!	@brief	敵生成管理ソース
//!	@author	立浪豪
//!	@date	2023/03/20
//------------------------------------------------------------------------------//


//======================================//
//				Include					//
//======================================//

using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;


//######################################################################################//
//!								敵生成管理クラス
//######################################################################################//

public class WamMoleSpawnManager : MonoBehaviour
{
    //======================================//
    //		プライベートシリアライズ変数	//
    //======================================//

    /* プレイエリア */
    [field: SerializeField, Label( "プレイエリア" ), Tooltip( "もぐらが出現する範囲、及び生成したもぐらプレハブをアタッチする親オブジェクト" )]
    private GameObject mpObjPlayArea;

    /* プレハブ配列（もぐら） */
    [field: SerializeField, Label( "プレハブ（もぐら）" ), Tooltip( "動的ランダム生成するもぐらのプレハブ郡" )]
    private GameObject[] mpObjMoles;

    /* もぐらの生成インターバル（最小） */
    [field: SerializeField, Label( "生成インターバル（最小）" ), Tooltip( "もぐらが生成されるまでに待機が必要な最小インターバル時間" ), Range( 0.0f , 100.0f )]
    private float mSpawnIntavalMin;

    /* もぐらの生成インターバル（最大延長） */
    [field: SerializeField, Label( "生成インターバル（最大延長）" ), Tooltip( "もぐらが生成されるまでに待機が必要な最大延長インターバル時間で、最小時間に加算される" ), Range( 0.0f , 100.0f )]
    private float mSpawnIntavalMaxExt;

    /* もぐら間の最小生成距離間隔 */
    [field: SerializeField, Label( "最小生成距離間隔" ), Tooltip( "前回生成されたもぐらから空ける最小距離間隔" ), Range( 0.0f , 10000.0f )]
    private float mSpawnDistanceMin;


    //======================================//
    //		    プライベート変数        	//
    //======================================//

    /* もぐら生成までの現在のインターバル時間 */
    private float mCurrentSpawnIntervalTime;

    /* もぐら生成までのランダム延長インターバル時間 */
    private float mRandomExtSpawnIntervalTime;

    /* 直前に生成されたもぐら間との距離 */
    private float mSpawnDistance;

    /* 生成するもぐらプレハブのランダム選定数値 */
    private byte mRandomSelect;

    /* もぐらの生成座標 */
    private Vector2 mSpawnLocation;

    /* プレイエリアの短形変換情報 */
    private RectTransform mPlayAreaRect;

    /* 生成したもぐらプレハブ */
    private GameObject mpCurrentMole;


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
    public void Start()
    {
        /* プレイエリアの短形変換情報を取得 */
        if ( this.mpObjPlayArea != null )
        {
            this.mPlayAreaRect = this.mpObjPlayArea.GetComponent<RectTransform>( );
        }

        /* 実行に必要な設定条件を確認する（メッセージ付き） */
        this.CheckRequiredConditions( bMessage: true );
    }

    //------------------------------------------------------------------------------//
    //! @brief	更新処理
    //------------------------------------------------------------------------------//
    public void Update()
    {
        
    }

    //------------------------------------------------------------------------------//
    //! @brief	初期化処理
    //------------------------------------------------------------------------------//
    public void Initialize( )
    {
        /* 各種変数初期化 */
        this.mRandomSelect              = 0;            /* 生成するもぐらプレハブのランダム選定数値 */
        this.mCurrentSpawnIntervalTime  = 0.0f;         /* もぐら生成までの現在のインターバル時間 */
        this.mSpawnDistance             = 0.0f;         /* 直前に生成されたもぐら間との距離 */
        this.mpCurrentMole              = null;         /* 生成したもぐらプレハブ */
        this.mSpawnLocation             = Vector2.zero; /* もぐらの生成座標 */

        /* 事前に乱数のシード値を変更して、乱数生成の準備を行う */
        Random.InitState( System.DateTime.Now.Millisecond );

        /* もぐら生成までのランダム延長インターバル時間を取得する */
        this.mRandomExtSpawnIntervalTime = Random.Range( 0.0f , this.mSpawnIntavalMaxExt );
    }

    //------------------------------------------------------------------------------//
    //! @brief	もぐらをランダムな座標、種類で生成する
    //------------------------------------------------------------------------------//
    public bool SpawnRandomMole( )
    {
        /* 実行に必要な条件が設定されていない場合、ログを表示して生成できなかった結果を返す */
        if ( !this.CheckRequiredConditions( ) )
        {
            WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Failed , "WamMoleSpawnManager" , "Mole spawn conditions is not match" );
            return false;
        }

        /* もぐら生成までの現在のインターバル時間が、指定時間に到達していない場合 */
        if ( this.mCurrentSpawnIntervalTime < ( this.mSpawnIntavalMin + this.mRandomExtSpawnIntervalTime ) )
        {
            /* インターバル時間を加算し、まだ生成できない結果を返す */
            this.mCurrentSpawnIntervalTime += Time.deltaTime;
            return false;
        }

        /* 事前に乱数のシード値を変更して、乱数生成の準備を行う */
        Random.InitState( System.DateTime.Now.Millisecond );

        /* もぐらを生成する座標をプレイエリア内からランダムで取得する */
        this.GetRandomSpawnLocation( );

        /* 直前に生成されたもぐらがいる場合、そのもぐらと現在ランダムで取得した座標との距離を調べ、指定距離以内の場合は再度ランダムで座標を取らせる */
        if ( !this.CheckSpawnDistance( ) )
        {
            return false;
        }

        /* もぐらプレハブ配列からランダムで生成するもぐらを選定、プレハブが存在しない場合は再度ランダム選定する */
        if ( !this.GetRandomMole( ) )
        {
            return false;
        }

        /* もぐらプレハブを生成し、失敗している場合は再度生成ルーチンに戻る */
        if ( !this.CreateMole( ) )
        {
            return false;
        }

        /* もぐら生成までの現在のインターバル時間を初期化 */
        this.mCurrentSpawnIntervalTime = 0.0f;

        /* もぐら生成までのランダム延長インターバル時間を取得する */
        this.mRandomExtSpawnIntervalTime = Random.Range( 0.0f , this.mSpawnIntavalMaxExt );

        //  WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Success , "WamMoleSpawnManager" , "Mole spawned to (" + this.mSpawnLocation.x + " , " + this.mSpawnLocation.y + ")" );

        /* もぐらプレハブを生成出来た結果を返す */
        return true;
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
        /* もぐらプレハブ郡が空なら */
        if ( this.mpObjMoles.Length <= 0 )
        {
            /* デバッグメッセージを表示する場合のみ表示する */
            if ( bMessage )
            {
                WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamMoleSpawnManager" , "Mole prefab is null" );
            }

            /* 実行に必要な条件が設定されていない結果を返す */
            return false;
        }

        /* プレイエリアが空なら */
        if ( this.mpObjPlayArea == null )
        {
            /* デバッグメッセージを表示する場合のみ表示する */
            if ( bMessage )
            {
                WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamMoleSpawnManager" , "PlayArea is null" );
            }

            /* 実行に必要な条件が設定されていない結果を返す */
            return false;
        }

        /* プレイエリアの短形変換情報が空なら */
        if ( this.mPlayAreaRect == null )
        {
            /* デバッグメッセージを表示する場合のみ表示する */
            if ( bMessage )
            {
                WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamMoleSpawnManager" , "PlayArea RectTransform component is null" );
            }

            /* 実行に必要な条件が設定されていない結果を返す */
            return false;
        }

        /* 実行に必要な全ての条件が設定されている結果を返す */
        return true;
    }

    //------------------------------------------------------------------------------//
    //! @brief	もぐらを生成する座標をプレイエリア内からランダムで取得する
    //------------------------------------------------------------------------------//
    private void GetRandomSpawnLocation( )
    {
        /* もぐらの生成座標をランダムで取得する */
        this.mSpawnLocation.x = Random.Range( ( this.mpObjPlayArea.transform.position.x - ( this.mPlayAreaRect.rect.width / 2 ) ) ,
                                                ( this.mpObjPlayArea.transform.position.x + ( this.mPlayAreaRect.rect.width / 2 ) ) );
        this.mSpawnLocation.y = Random.Range( ( this.mpObjPlayArea.transform.position.y - ( this.mPlayAreaRect.rect.height / 2 ) ) ,
                                                ( this.mpObjPlayArea.transform.position.y + ( this.mPlayAreaRect.rect.height / 2 ) ) );
    }

    //------------------------------------------------------------------------------//
    //! @brief	直前に生成されたもぐらがいる場合、そのもぐらと現在ランダムで取得した座標との距離を調べる
    //------------------------------------------------------------------------------//
    private bool CheckSpawnDistance( )
    {
        /* 直前に生成済みのもぐらプレハブが存在しない場合、距離の比較の必要が無い結果を返す */
        if ( this.mpCurrentMole == null )
        {
            return true;
        }

        /* もぐら間の現在の生成距離を取得 */
        this.mSpawnDistance = Vector2.Distance( this.mpCurrentMole.transform.position , new Vector2( this.mSpawnLocation.x , this.mSpawnLocation.y ) );

        /* 前回生成したもぐらと、今回生成する予定のもぐらの距離が、指定距離未満の場合 */
        if ( this.mSpawnDistance < this.mSpawnDistanceMin )
        {
            /* もう一度ランダムで座標を取らせ直す */
            return false;
        }

        /* 前回生成もぐらと今回ランダム座標の距離が、指定以上の距離であった結果を返す */
        return true;
    }

    //------------------------------------------------------------------------------//
    //! @brief	もぐらプレハブ配列からランダムで生成するもぐらを選定する
    //------------------------------------------------------------------------------//
    private bool GetRandomMole( )
    {
        /* 生成するもぐらプレハブをランダムで選定 */
        this.mRandomSelect = (byte)Random.Range( 0 , this.mpObjMoles.Length );

        /* もぐらプレハブが空なら */
        if ( this.mpObjMoles[this.mRandomSelect] == null )
        {
            /* デバッグメッセージを表示する */
            WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamMoleSpawnManager" , "Moles prefab array num " + this.mRandomSelect + " is null" );

            /* もう一度ランダムでもぐらを取らせ直す */
            return false;
        }

        /* ランダムで選定したもぐらが存在した結果を返す */
        return true;
    }

    //------------------------------------------------------------------------------//
    //! @brief	もぐらプレハブを生成する
    //------------------------------------------------------------------------------//
    private bool CreateMole( )
    {
        /* ランダムで取得した座標にランダムで選定したもぐらプレハブを生成する */
        this.mpCurrentMole = Instantiate( this.mpObjMoles[this.mRandomSelect] , new Vector2( this.mSpawnLocation.x , this.mSpawnLocation.y ) , Quaternion.identity , this.mpObjPlayArea.transform );

        /* もぐらプレハブの生成が成功していない場合、エラーログを出して失敗した結果を返す */
        if ( this.mpCurrentMole == null )
        {
            WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Failed , "WamMoleSpawnManager" , "Mole prefab create failed" );
            return false;
        }

        /* もぐらプレハブの生成が成功した結果を返す */
        return true;
    }
}
