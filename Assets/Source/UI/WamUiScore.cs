using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WamUiScore : MonoBehaviour
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
        WamMoleSlapGamemode.GetInstance( ).OnScoreUpdate += OnScoreUpdate;
    }

    // Update is called once per frame
    public void Update()
    {
        
    }

    /* �f���Q�[�g�o�C���h�֐��i�X�R�A�ϓ��j */
    private void OnScoreUpdate( uint Score )
    {
        /* ���̃X�N���v�g���A�^�b�`����Ă���Q�[���I�u�W�F�N�g�ɁA�e�L�X�g���b�V���v��UGUI���ǉ�����Ă��Ȃ��ꍇ */
        if ( this.GetComponent<TextMeshProUGUI>( ) == null )
        {
            Debug.Log( "[Error] <WamUiScore> " + this.name + " is not add TextMeshProUGUI component." );
            return;
        }

        /* ���݃X�R�A���X�V���� */
        this.GetComponent<TextMeshProUGUI>( ).SetText( Score.ToString( ) );
    }
}
