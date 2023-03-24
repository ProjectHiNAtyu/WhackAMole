using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WamUiTimer : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        /* ゲームモードクラスのインスタンスが空なら */
        if ( WamGameInstanceManager.GetInstance( ).GetTimeManagerInstance( ) == null )
        {
            WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamUiTimer" , "TimeManager instance is null" );
            return;
        }

        /* タイマーカウントダウンのデリゲートを登録 */
        WamGameInstanceManager.GetInstance( ).GetTimeManagerInstance( ).OnTimerCountdown += OnTimerCountdown;
    }

    // Update is called once per frame
    public void Update()
    {
        
    }

    /* デリゲートバインド関数（タイマーカウントダウン） */
    private void OnTimerCountdown( ushort Time )
    {
        /* このスクリプトがアタッチされているゲームオブジェクトに、テキストメッシュプロUGUIが追加されていない場合 */
        if ( this.GetComponent<TextMeshProUGUI>( ) == null )
        {
            WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamUiTimer" , this.name + " is not add TextMeshProUGUI component" );
            return;
        }

        /* 現在時間を更新する */
        this.GetComponent<TextMeshProUGUI>( ).SetText( Time.ToString( ) );
    }
}
