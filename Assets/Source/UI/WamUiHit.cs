using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WamUiHit : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        /* �Q�[�����[�h�N���X�̃C���X�^���X����Ȃ� */
        if ( WamMoleSlapGamemode.GetInstance( ) == null )
        {
            Debug.Log( "[Error] <WamUiHit> WamMoleSlapGamemode instance is null." );
            return;
        }

        /* �������@�����񐔕ϓ��̃f���Q�[�g��o�^ */
        WamMoleSlapGamemode.GetInstance( ).OnMoleSlapCountUpdate += OnMoleSlapCountUpdate;
    }

    // Update is called once per frame
    public void Update()
    {
        
    }

    /* �f���Q�[�g�o�C���h�֐��i�������@�����񐔕ϓ��j */
    private void OnMoleSlapCountUpdate( uint Count )
    {
        /* ���̃X�N���v�g���A�^�b�`����Ă���Q�[���I�u�W�F�N�g�ɁA�e�L�X�g���b�V���v��UGUI���ǉ�����Ă��Ȃ��ꍇ */
        if ( this.GetComponent<TextMeshProUGUI>( ) == null )
        {
            Debug.Log( "[Error] <WamUiHit> " + this.name + " is not add TextMeshProUGUI component." );
            return;
        }

        /* ���݂̂������@�����񐔂��X�V���� */
        this.GetComponent<TextMeshProUGUI>( ).SetText( Count.ToString( ) );
    }
}