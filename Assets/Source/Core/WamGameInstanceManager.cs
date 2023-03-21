//------------------------------------------------------------------------------//
//!	@file   WamGameInstanceManager.cs
//!	@brief	�Q�[���C���X�^���X�Ǘ��\�[�X
//!	@author	���Q��
//!	@date	2023/03/20
//------------------------------------------------------------------------------//


//======================================//
//				Include					//
//======================================//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//######################################################################################//
//!								�Q�[���C���X�^���X�Ǘ��N���X
//######################################################################################//

public class WamGameInstanceManager : MonoBehaviour
{
    //======================================//
    //		�v���C�x�[�g�V���A���C�Y�ϐ�	//
    //======================================//

    /* �Q�[�����[�h�Ǘ� */
    [field: SerializeField, Label( "�Q�[�����[�h�Ǘ�" ), Tooltip( "�Q�[�����[�h�Ǘ��X�N���v�g���ǉ�����Ă���Q�[���I�u�W�F�N�g���w��" )]
    private WamMoleSlapGamemode mpGameModeManager;

    /* UI�Ǘ� */
    [field: SerializeField, Label( "UI�Ǘ�" ), Tooltip( "UI�Ǘ��X�N���v�g���ǉ�����Ă���Q�[���I�u�W�F�N�g���w��" )]
    private WamUiManager mpUIManager;

    /* ���ԊǗ� */
    [field: SerializeField, Label( "���ԊǗ�" ), Tooltip( "���ԊǗ��X�N���v�g���ǉ�����Ă���Q�[���I�u�W�F�N�g���w��" )]
    private WamTimeManager mpTimeManager;

    /* �����琶���Ǘ� */
    [field: SerializeField, Label( "�����琶���Ǘ�" ), Tooltip( "�����琶���Ǘ��X�N���v�g���ǉ�����Ă���Q�[���I�u�W�F�N�g���w��" )]
    private WamMoleSpawnManager mpMoleSpawnManager;

    /* �f�o�b�O�Ǘ� */
    [field: SerializeField, Label( "�f�o�b�O�Ǘ�" ), Tooltip( "�f�o�b�O�Ǘ��X�N���v�g���ǉ�����Ă���Q�[���I�u�W�F�N�g���w��" )]
    private WamDebugManager mpDebugManager;


    //======================================//
    //		    �v���C�x�[�g�ϐ�        	//
    //======================================//

    /* �C���X�^���X */
    private static WamGameInstanceManager mpInstance;


    //======================================//
    //		    �p�u���b�N�֐�             	//
    //======================================//

    //------------------------------------------------------------------------------//
    //! @brief	�Q�[���C���X�^���X�Ǘ��C���X�^���X���擾����
    //------------------------------------------------------------------------------//
    public static WamGameInstanceManager GetInstance( )
    {
        if ( WamGameInstanceManager.mpInstance == null )
        {
            Debug.Log( "[Error] <WamGameInstanceManager> GameInstanceManager is null." );
        }
        return WamGameInstanceManager.mpInstance;
    }
    
    //------------------------------------------------------------------------------//
    //! @brief	�Q�[�����[�h�Ǘ��C���X�^���X���擾����
    //------------------------------------------------------------------------------//
    public WamMoleSlapGamemode GetGameModeManagerInstance( )
    {
        if ( this.mpGameModeManager == null )
        {
            Debug.Log( "[Error] <WamGameInstanceManager> GameModeManager is null." );
        }
        return this.mpGameModeManager;
    }

    //------------------------------------------------------------------------------//
    //! @brief	UI�Ǘ��C���X�^���X���擾����
    //------------------------------------------------------------------------------//
    public WamUiManager GetUIManagerInstance( )
    {
        if ( this.mpUIManager == null )
        {
            Debug.Log( "[Error] <WamGameInstanceManager> UIManager is null." );
        }
        return this.mpUIManager;
    }

    //------------------------------------------------------------------------------//
    //! @brief	���ԊǗ��C���X�^���X���擾����
    //------------------------------------------------------------------------------//
    public WamTimeManager GetTimeManagerInstance( )
    {
        if ( this.mpTimeManager == null )
        {
            Debug.Log( "[Error] <WamGameInstanceManager> TimeManager is null." );
        }
        return this.mpTimeManager;
    }

    //------------------------------------------------------------------------------//
    //! @brief	�����琶���Ǘ��C���X�^���X���擾����
    //------------------------------------------------------------------------------//
    public WamMoleSpawnManager GetMoleSpawnManagerInstance( )
    {
        if ( this.mpMoleSpawnManager == null )
        {
            Debug.Log( "[Error] <WamGameInstanceManager> MoleSpawnManager is null." );
        }
        return this.mpMoleSpawnManager;
    }

    //------------------------------------------------------------------------------//
    //! @brief	�f�o�b�O�Ǘ��C���X�^���X���擾����
    //------------------------------------------------------------------------------//
    public WamDebugManager GetDebugManagerInstance( )
    {
        if ( this.mpDebugManager == null )
        {
            Debug.Log( "[Error] <WamGameInstanceManager> DebugManager is null." );
        }
        return this.mpDebugManager;
    }

    //------------------------------------------------------------------------------//
    //! @brief	�����J�n���ɌĂ΂�� 
    //------------------------------------------------------------------------------//
    public void Awake( )
    {
        /* �C���X�^���X��ۑ� */
        if ( WamGameInstanceManager.mpInstance == null )
        {
            WamGameInstanceManager.mpInstance = this;
        }
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
}
