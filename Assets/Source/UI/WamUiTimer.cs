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
        if ( WamMoleSlapGamemode.GetInstance( ) == null )
        {
            Debug.Log( "[Error] <WamUiTimer> WamMoleSlapGamemode instance is null." );
            return;
        }

        /* タイマーカウントダウンのデリゲートを登録 */
        WamMoleSlapGamemode.GetInstance( ).OnTimerCountdown += OnTimerCountdown;
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
            Debug.Log( "[Error] <WamUiTimer> " + this.name + " is not add TextMeshProUGUI component." );
            return;
        }

        /* 現在時間を更新する */
        this.GetComponent<TextMeshProUGUI>( ).SetText( Time.ToString( ) );
    }
}
