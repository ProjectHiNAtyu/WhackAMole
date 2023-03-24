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
        if ( WamGameInstanceManager.GetInstance( ).GetGameModeManagerInstance( ) == null )
        {
            WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamUiScore" , "GameModeManger instance is null" );
            return;
        }

        /* スコア変動のデリゲートを登録 */
        WamGameInstanceManager.GetInstance( ).GetGameModeManagerInstance( ).OnScoreUpdate += OnScoreUpdate;
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
            WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamUiScore" , this.name + " is not add TextMeshProUGUI component" );
            return;
        }

        /* 現在スコアを更新する */
        this.GetComponent<TextMeshProUGUI>( ).SetText( Score.ToString( ) );
    }
}
