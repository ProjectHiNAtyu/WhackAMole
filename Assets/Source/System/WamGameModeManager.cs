using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WamGameModeManager : MonoBehaviour
{
    /* �f���Q�[�g�錾�i�X�R�A�ϓ��j */
    public delegate void OnScoreUpdateDelegate( uint Score );
    /* �f���Q�[�g��`�i�X�R�A�ϓ��j */
    public OnScoreUpdateDelegate OnScoreUpdate;

    /* �f���Q�[�g�錾�i�������@�����񐔕ϓ��j */
    public delegate void OnMoleSlapCountUpdateDelegate( uint Count );
    /* �f���Q�[�g��`�i�������@�����񐔕ϓ��j */
    public OnMoleSlapCountUpdateDelegate OnMoleSlapCountUpdate;

    /* �w�i�Z�b�g */
    [field: SerializeField, Label( "�w�i�Z�b�g" ), Tooltip( "�Q�[���v���C���̔w�i�G" )]
    private GameObject[] mpObjBackgroundArtSets;

    /* �����_���I��p���l */
    private byte mRandomNumber;

    /* ���݂̃X�R�A */
    private uint mCurrentScore;

    /* ���݂̂������@������ */
    private uint mCurrentSlapCount;

    public void Awake( )
    {

    }

    // Start is called before the first frame update
    public void Start( )
    {
    }

    /* �Q�[�����[�h�̏����������� */
    public void Initialize( )
    {
        /* �e��ϐ������� */
        this.mCurrentScore              = 0;        /* ���݂̃X�R�A */
        this.mCurrentSlapCount          = 0;        /* ���݂̂������@������ */


        /* �w�i�Z�b�g�������[�v */
        for ( byte i = 0; i < this.mpObjBackgroundArtSets.Length; i++ )
        {
            /* �w�i�Z�b�g����ł͂Ȃ����� */
            if ( this.mpObjBackgroundArtSets[i] != null )
            {
                /* ��U�S�Ă̔w�i�Z�b�g���\���ɂ��� */
                this.mpObjBackgroundArtSets[i].SetActive( false );
            }
        }

        /* �����_���Ŕw�i�Z�b�g��I�� */
        this.mRandomNumber = (byte)Random.Range( 0 , ( this.mpObjBackgroundArtSets.Length - 1 ) );

        /* �w�i�Z�b�g����ł͂Ȃ����� */
        if ( this.mpObjBackgroundArtSets[this.mRandomNumber] != null )
        {
            /* �w�i�Z�b�g��\������ */
            this.mpObjBackgroundArtSets[this.mRandomNumber].SetActive( true );
        }
    }

    //------------------------------------------------------------------------------//
    //! @brief	����݂̂̏��������s����
    //------------------------------------------------------------------------------//
    public void ExecFirstProcess( )
    {
        /* ���݃X�R�A�����������ꂽ���Ƃ�ʒm */
        this.OnScoreUpdate( 0 );

        /* ���݂������@�����񐔂����������ꂽ���Ƃ�ʒm */
        this.OnMoleSlapCountUpdate( 0 );
    }

    // Update is called once per frame
    public void Update( )
    {
        /* �������ݎ��Ԃ�0�b�ȉ��Ȃ� */
        if ( WamGameInstanceManager.GetInstance( ).GetTimeManagerInstance( ).IsTimeOver )
        {
            return;
        }

        /* �����_���ł�����𐶐����� */
        WamGameInstanceManager.GetInstance( ).GetMoleSpawnManagerInstance( ).SpawnRandomMole( );
    }

    /* �X�R�A�����Z���� */
    public void AddScore( ushort Score )
    {
        /* ���݂̃X�R�A�ɓ|���ꂽ������̃X�R�A�����Z */
        this.mCurrentScore += Score;

        /* �X�R�A���ϓ��������Ƃ�ʒm */
        this.OnScoreUpdate( this.mCurrentScore );

        /* ���݂̂������@�����񐔂����Z */
        this.mCurrentSlapCount++;

        /* �������@�����񐔂��ϓ��������Ƃ�ʒm */
        this.OnMoleSlapCountUpdate( this.mCurrentSlapCount );
    }
}
