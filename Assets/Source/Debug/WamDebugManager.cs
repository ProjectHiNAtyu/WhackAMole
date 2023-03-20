//------------------------------------------------------------------------------//
//!	@file   WamDebugManager.cs
//!	@brief	�f�o�b�O�Ǘ��\�[�X
//!	@author	���Q��
//!	@date	2023/03/20
//------------------------------------------------------------------------------//


//======================================//
//				Define					//
//======================================//

#define DEBUG


//======================================//
//				Include					//
//======================================//

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


//######################################################################################//
//!								�f�o�b�O�Ǘ��N���X
//######################################################################################//

public class WamDebugManager : MonoBehaviour
{
    //======================================//
    //			�p�u���b�N�񋓌^			//
    //======================================//

    /* ���O��� */
    public enum EWamLogType
    {
        Notice  = 0, /* ���O��� �ʒm */
        Warning = 1, /* ���O��� �x�� */
        Error   = 2, /* ���O��� �G���[ */
        Failed  = 3, /* ���O��� ���s */
        Success = 4, /* ���O��� ���� */
    }


    //======================================//
    //			�v���C�x�[�g�ϐ�			//
    //======================================//

    /* �f�o�b�O���O�ŕ\�����郁�b�Z�[�W���e */
    private string mLogMessage;


    //======================================//
    //		    �p�u���b�N�֐�             	//
    //======================================//

    //------------------------------------------------------------------------------//
    //! @brief	�����J�n���ɌĂ΂�� 
    //------------------------------------------------------------------------------//
    public void Awake( )
    {

    }

    //------------------------------------------------------------------------------//
    //! @brief	����X�V�����̒��O�ɌĂ΂��
    //------------------------------------------------------------------------------//
    public void Start( )
    {

    }

    //------------------------------------------------------------------------------//
    //! @brief	�X�V����
    //------------------------------------------------------------------------------//
    public void Update( )
    {

    }

    //------------------------------------------------------------------------------//
    //! @brief	�f�o�b�O���O��\������
    //!
    //! @param	Message	�\�����郁�b�Z�[�W���e
    //!
    //! @return ����
    //------------------------------------------------------------------------------//
    [Conditional( "DEBUG" )]
    public void ShowDebugLog( string Message )
    {
        UnityEngine.Debug.Log( Message );
    }

    //------------------------------------------------------------------------------//
    //! @brief	�f�o�b�O���O���e���v���[�g�ɉ����ĕ\������
    //!
    //! @param	eType	    ���̃��O�̎��
    //! @param	ClassName	���O�̔������N���X��
    //! @param	Message	    ���b�Z�[�W�{��
    //!
    //! @return ����
    //------------------------------------------------------------------------------//
    [Conditional( "DEBUG" )]
    public void ShowDebugLogTemplate( EWamLogType eType , string ClassName , string Message )
    {
        /* �\�����郁�b�Z�[�W���e�������� */
        mLogMessage = string.Empty;

        /* ���O��ނ̍\�z */
        mLogMessage += "[";
        switch ( eType )
        {
            case EWamLogType.Notice:    mLogMessage += "Notice";    break;
            case EWamLogType.Warning:   mLogMessage += "Warning";   break;
            case EWamLogType.Error:     mLogMessage += "Error";     break;
            case EWamLogType.Failed:    mLogMessage += "Failed";    break;
            case EWamLogType.Success:   mLogMessage += "Success";   break;
        }
        mLogMessage += "] ";

        /* �Ăяo�����N���X���̍\�z */
        mLogMessage += "<";
        mLogMessage += ClassName;
        mLogMessage += "> ";

        /* �{���̍\�z */
        mLogMessage += Message;
        mLogMessage += ".";

        /* �f�o�b�O���O��\�� */
        UnityEngine.Debug.Log( mLogMessage );
    }
}