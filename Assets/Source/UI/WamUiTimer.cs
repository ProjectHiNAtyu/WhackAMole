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
        /* �Q�[�����[�h�N���X�̃C���X�^���X����Ȃ� */
        if ( WamMoleSlapGamemode.GetInstance( ) == null )
        {
            Debug.Log( "[Error] <WamUiTimer> WamMoleSlapGamemode instance is null." );
            return;
        }

        /* �^�C�}�[�J�E���g�_�E���̃f���Q�[�g��o�^ */
        WamMoleSlapGamemode.GetInstance( ).OnTimerCountdown += OnTimerCountdown;
    }

    // Update is called once per frame
    public void Update()
    {
        
    }

    /* �f���Q�[�g�o�C���h�֐��i�^�C�}�[�J�E���g�_�E���j */
    private void OnTimerCountdown( ushort Time )
    {
        /* ���̃X�N���v�g���A�^�b�`����Ă���Q�[���I�u�W�F�N�g�ɁA�e�L�X�g���b�V���v��UGUI���ǉ�����Ă��Ȃ��ꍇ */
        if ( this.GetComponent<TextMeshProUGUI>( ) == null )
        {
            Debug.Log( "[Error] <WamUiTimer> " + this.name + " is not add TextMeshProUGUI component." );
            return;
        }

        /* ���ݎ��Ԃ��X�V���� */
        this.GetComponent<TextMeshProUGUI>( ).SetText( Time.ToString( ) );
    }
}