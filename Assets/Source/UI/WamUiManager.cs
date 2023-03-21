//------------------------------------------------------------------------------//
//!	@file   WamUiManager.cs
//!	@brief	UI�Ǘ��\�[�X
//!	@author	���Q��
//!	@date	2023/03/20
//------------------------------------------------------------------------------//


//======================================//
//				Include					//
//======================================//

using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;


//######################################################################################//
//!								UI�Ǘ��N���X
//######################################################################################//

public class WamUiManager : MonoBehaviour
{
    //======================================//
    //		�v���C�x�[�g�V���A���C�Y�ϐ�	//
    //======================================//

    /* ���w�b�_�[UI */
    [field: SerializeField, Label( "���w�b�_�[UI" ), Tooltip( "�Q�[���v���C���̏�񂪕\�������UI" )]
    private GameObject mpObjInfoUI;

    /* ���U���gUI */
    [field: SerializeField, Label( "���U���gUI" ), Tooltip( "�������ԏI����ɕ\������郊�U���gUI" )]
    private GameObject mpObjResultUI;

    /* ���g���C�{�^��UI */
    [field: SerializeField, Label( "���g���C�{�^��UI" ), Tooltip( "���U���gUI���ɂ��郊�g���C�{�^��UI" )]
    private Button mpButtonRetryUI;


    //======================================//
    //		    �v���C�x�[�g�ϐ�        	//
    //======================================//

    /* ���U���g�����ǂ��� */
    private bool mbResult;


    //======================================//
    //		    �p�u���b�N�֐�             	//
    //======================================//

    //------------------------------------------------------------------------------//
    //! @brief	����������
    //------------------------------------------------------------------------------//
    public void Initialize( )
    {
        /* �e��ϐ������� */
        this.mbResult = false;    /* ���U���g�����ǂ��� */


        /* ���w�b�_�[UI����Ȃ� */
        if ( this.mpObjInfoUI == null )
        {
            WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamUiManager" , "Info UI is null" );
            return;
        }

        /* ���U���gUI����Ȃ� */
        if ( this.mpObjResultUI == null )
        {
            WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamUiManager" , "Result UI is null" );
            return;
        }

        /* ���U���gUI���L�����o�X�O���[�v�R���|�[�l���g�������Ă��Ȃ��ꍇ */
        if ( this.mpObjResultUI.GetComponent<CanvasGroup>( ) == null )
        {
            WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamUiManager" , "Result UI is not add CanvasGroup component" );
            return;
        }


        /* ���w�b�_�[UI��\�� */
        this.mpObjInfoUI.SetActive( true );

        /* ���U���gUI��\�� */
        this.mpObjResultUI.SetActive( true );

        /* ���U���gUI�̓����蔻��𖳂��� */
        this.mpObjResultUI.GetComponent<CanvasGroup>( ).blocksRaycasts = false;

        /* ���U���gUI�𓧖��x0�ɂ��� */
        this.mpObjResultUI.GetComponent<CanvasGroup>( ).alpha = 0.0f;
    }

    //------------------------------------------------------------------------------//
    //! @brief	�����J�n���ɌĂ΂�� 
    //------------------------------------------------------------------------------//
    public void Awake( )
    {
        /* ���g���C�{�^��UI����Ȃ�A�G���[���O�o���ď������Ȃ� */
        if ( this.mpButtonRetryUI == null )
        {
            WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamUiManager" , "Retry button UI is null" );
            return;
        }

        /* �{�^���N���b�N�C�x���g���o�C���h */
        this.mpButtonRetryUI.onClick.AsObservable( ).Subscribe( _ => WamGameInstanceManager.GetInstance( ).Initialize( ) );
    }

    //------------------------------------------------------------------------------//
    //! @brief	����X�V�����̒��O�ɌĂ΂��
    //------------------------------------------------------------------------------//
    public void Start( )
    {
        /* ���s�ɕK�v�Ȑݒ�������m�F����i���b�Z�[�W�t���j */
        this.CheckRequiredConditions( bMessage: true );
    }

    //------------------------------------------------------------------------------//
    //! @brief	�X�V����
    //------------------------------------------------------------------------------//
    public void Update( )
    {
        /* �������ݎ��Ԃ�0�b�ȉ��Ȃ� */
        if ( WamGameInstanceManager.GetInstance( ).GetTimeManagerInstance( ).IsTimeOver )
        {
            /* ���O�܂Ń��U���g���łȂ������ꍇ */
            if ( !this.mbResult )
            {
                /* ���U���g���Ƃ��� */
                this.mbResult = true;

                /* ���s�ɕK�v�ȏ������ݒ肳��Ă��Ȃ��ꍇ�A���O��\�����Đ����ł��Ȃ��������ʂ�Ԃ� */
                if ( !this.CheckRequiredConditions( ) )
                {
                    WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Failed , "WamUiManager" , "Result scene change conditions is not match" );
                    return;
                }

                /* ���w�b�_�[UI���\�� */
                this.mpObjInfoUI.SetActive( false );

                /* ���U���gUI�̓����蔻��𕜊������� */
                this.mpObjResultUI.GetComponent<CanvasGroup>( ).blocksRaycasts = true;

                /* ���U���gUI�𓧖��x1�ɂ��� */
                this.mpObjResultUI.GetComponent<CanvasGroup>( ).alpha = 1.0f;
            }
            return;
        }
    }


    //======================================//
    //		    �v���C�x�[�g�֐�            //
    //======================================//

    //------------------------------------------------------------------------------//
    //! @brief	���s�ɕK�v�Ȑݒ�������m�F����
    //!
    //! @param	bMessage	�f�o�b�O���b�Z�[�W��\�����邩�ǂ���
    //!
    //! @return ���s�ɕK�v�ȏ������ݒ肳��Ă������ǂ���
    //------------------------------------------------------------------------------//
    private bool CheckRequiredConditions( bool bMessage = false )
    {
        /* ���w�b�_�[UI����Ȃ� */
        if ( this.mpObjInfoUI == null )
        {
            /* �f�o�b�O���b�Z�[�W��\������ꍇ�̂ݕ\������ */
            if ( bMessage )
            {
                WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamUiManager" , "Info UI is null" );
            }

            /* ���s�ɕK�v�ȏ������ݒ肳��Ă��Ȃ����ʂ�Ԃ� */
            return false;
        }

        /* ���U���gUI����Ȃ� */
        if ( this.mpObjResultUI == null )
        {
            /* �f�o�b�O���b�Z�[�W��\������ꍇ�̂ݕ\������ */
            if ( bMessage )
            {
                WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamUiManager" , "Result UI is null" );
            }

            /* ���s�ɕK�v�ȏ������ݒ肳��Ă��Ȃ����ʂ�Ԃ� */
            return false;
        }

        /* ���U���gUI���L�����o�X�O���[�v�R���|�[�l���g�������Ă��Ȃ��ꍇ */
        if ( this.mpObjResultUI.GetComponent<CanvasGroup>( ) == null )
        {
            /* �f�o�b�O���b�Z�[�W��\������ꍇ�̂ݕ\������ */
            if ( bMessage )
            {
                WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamUiManager" , "Result UI is not add CanvasGroup component" );
            }

            /* ���s�ɕK�v�ȏ������ݒ肳��Ă��Ȃ����ʂ�Ԃ� */
            return false;
        }

        /* ���s�ɕK�v�ȑS�Ă̏������ݒ肳��Ă��錋�ʂ�Ԃ� */
        return true;
    }
}
