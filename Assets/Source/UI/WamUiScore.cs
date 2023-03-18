using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WamUiScore : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        /* ゲームモードクラスのインスタンスが空なら */
        if ( WamMoleSlapGamemode.GetInstance( ) == null )
        {
            Debug.Log( "[Error] <WamUiTimer> WamMoleSlapGamemode instance is null." );
            return;
        }

        /* タイマーカウントダウンのデリゲートを登録 */
        WamMoleSlapGamemode.GetInstance( ).OnScoreUpdate += OnScoreUpdate;
    }

    // Update is called once per frame
    public void Update()
    {
        
    }

    /* デリゲートバインド関数（スコア変動） */
    private void OnScoreUpdate( uint Score )
    {
        /* このスクリプトがアタッチされているゲームオブジェクトに、テキストメッシュプロUGUIが追加されていない場合 */
        if ( this.GetComponent<TextMeshProUGUI>( ) == null )
        {
            Debug.Log( "[Error] <WamUiScore> " + this.name + " is not add TextMeshProUGUI component." );
            return;
        }

        /* 現在スコアを更新する */
        this.GetComponent<TextMeshProUGUI>( ).SetText( Score.ToString( ) );
    }
}
