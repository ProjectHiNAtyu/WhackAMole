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
        if ( WamMoleSlapGamemode.GetInstance( ) == null )
        {
            Debug.Log( "[Error] <WamUiHit> WamMoleSlapGamemode instance is null." );
            return;
        }

        /* もぐらを叩いた回数変動のデリゲートを登録 */
        WamMoleSlapGamemode.GetInstance( ).OnMoleSlapCountUpdate += OnMoleSlapCountUpdate;
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
            Debug.Log( "[Error] <WamUiHit> " + this.name + " is not add TextMeshProUGUI component." );
            return;
        }

        /* 現在のもぐらを叩いた回数を更新する */
        this.GetComponent<TextMeshProUGUI>( ).SetText( Count.ToString( ) );
    }
}
