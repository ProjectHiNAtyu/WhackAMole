using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WamUiHit : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        /* ゲームモードクラスのインスタンスが空なら */
        if ( WamGameInstanceManager.GetInstance( ).GetGameModeManagerInstance( ) == null )
        {
            WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamUiHit" , "GameModeManger instance is null" );
            return;
        }

        /* もぐらを叩いた回数変動のデリゲートを登録 */
        WamGameInstanceManager.GetInstance( ).GetGameModeManagerInstance( ).OnMoleSlapCountUpdate += OnMoleSlapCountUpdate;
    }

    // Update is called once per frame
    public void Update()
    {
        
    }

    /* デリゲートバインド関数（もぐらを叩いた回数変動） */
    private void OnMoleSlapCountUpdate( uint Count )
    {
        /* このスクリプトがアタッチされているゲームオブジェクトに、テキストメッシュプロUGUIが追加されていない場合 */
        if ( this.GetComponent<TextMeshProUGUI>( ) == null )
        {
            WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamUiHit" , this.name + " is not add TextMeshProUGUI component" );
            return;
        }

        /* 現在のもぐらを叩いた回数を更新する */
        this.GetComponent<TextMeshProUGUI>( ).SetText( Count.ToString( ) );
    }
}
