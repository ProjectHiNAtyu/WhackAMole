using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WamMoleSlapGamemode : MonoBehaviour
{
    /* 最大制限時間 */
    [field: SerializeField, Label( "最大制限時間" ), Tooltip( "ゲーム開始時の最大制限時間" ), Range( 0 , 65535 )]
    private ushort mMaxTime;

    /* テキストメッシュプロ（現在時間） */
    [field: SerializeField, Label( "テキストUI（現在時間）" ), Tooltip( "現在時間を表示するテキストUI" )]
    private TextMeshProUGUI mTmpCurrentTime;

    /* テキストメッシュプロ（叩いた回数） */
    [field: SerializeField, Label( "テキストUI（叩いた回数）" ), Tooltip( "もぐらを叩いた回数を表示するテキストUI" )]
    private TextMeshProUGUI mTmpCurrentHitCount;

    /* テキストメッシュプロ（スコア） */
    [field: SerializeField, Label( "テキストUI（スコア）" ), Tooltip( "プレイ中の合計スコアを表示するテキストUI" )]
    private TextMeshProUGUI mTmpCurrentScore;

    /* 背景セット */
    [field: SerializeField, Label( "背景セット" ), Tooltip( "ゲームプレイ中の背景絵" )]
    private GameObject[] mObjBackgroundArtSet;

    /* 現在時間 */
    private ushort mCurrentTime;

    /* フレーム時間 */
    private float mFlameTime;

    /* ランダム選択用数値 */
    private byte mRandomNumber;

    // Start is called before the first frame update
    void Start( )
    {
        /* 叩いた回数テキストを初期化する */
        this.mTmpCurrentHitCount.SetText( "0" );

        /* スコアテキストを初期化する */
        this.mTmpCurrentScore.SetText( "0" );

        /* 現在時間テキストに最大制限時間を設定する */
        this.mTmpCurrentTime.SetText( this.mMaxTime.ToString( ) );

        /* 現在時間を最大制限時間に設定 */
        this.mCurrentTime = this.mMaxTime;

        /* フレーム時間を初期化 */
        this.mFlameTime = 0.0f;

        /* 背景セット個数分ループ */
        for ( byte i = 0; i < this.mObjBackgroundArtSet.Length; i++ )
        {
            /* 背景セットが空ではない時に */
            if ( this.mObjBackgroundArtSet[i] != null )
            {
                /* 一旦全ての背景セットを非表示にする */
                this.mObjBackgroundArtSet[i].SetActive( false );
            }
        }

        /* 事前に乱数のシード値を変更して、乱数生成の準備を行う */
        Random.InitState( System.DateTime.Now.Millisecond );

        /* ランダムで背景セットを選択 */
        this.mRandomNumber = (byte)Random.Range( 0 , ( this.mObjBackgroundArtSet.Length - 1 ) );

        /* 背景セットが空ではない時に */
        if ( this.mObjBackgroundArtSet[this.mRandomNumber] != null )
        {
            /* 背景セットを表示する */
            this.mObjBackgroundArtSet[this.mRandomNumber].SetActive( true );
        }
    }

    // Update is called once per frame
    void Update( )
    {
        /* もし現在時間が0秒以上なら */
        if ( 0 < this.mCurrentTime )
        {
            /* フレーム時間を加算 */
            this.mFlameTime += Time.deltaTime;

            /* 合計フレーム時間が1秒を超えたら */
            if ( 1.0f <= this.mFlameTime )
            {
                /* 合計フレーム時間から1秒減算 */
                this.mFlameTime -= 1.0f;

                /* 現在時間を1秒分減算 */
                this.mCurrentTime--;

                /* テキストに現在時間を設定する */
                this.mTmpCurrentTime.SetText( this.mCurrentTime.ToString( ) );
            }
        }
    }
}
