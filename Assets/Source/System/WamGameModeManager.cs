using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WamMoleSlapGamemode : MonoBehaviour
{
    /* �f���Q�[�g�錾�i�^�C�}�[�J�E���g�_�E���j */
    public delegate void OnTimerCountdownDelegate( ushort Time );
    /* �f���Q�[�g��`�i�^�C�}�[�J�E���g�_�E���j */
    public OnTimerCountdownDelegate OnTimerCountdown;

    /* �f���Q�[�g�錾�i�X�R�A�ϓ��j */
    public delegate void OnScoreUpdateDelegate( uint Score );
    /* �f���Q�[�g��`�i�X�R�A�ϓ��j */
    public OnScoreUpdateDelegate OnScoreUpdate;

    /* �f���Q�[�g�錾�i�������@�����񐔕ϓ��j */
    public delegate void OnMoleSlapCountUpdateDelegate( uint Count );
    /* �f���Q�[�g��`�i�������@�����񐔕ϓ��j */
    public OnMoleSlapCountUpdateDelegate OnMoleSlapCountUpdate;

    /* �ő吧������ */
    [field: SerializeField, Label( "�ő吧������" ), Tooltip( "�Q�[���J�n���̍ő吧������" ), Range( 0 , 65535 )]
    private ushort mMaxTime;

    /* �w�i�Z�b�g */
    [field: SerializeField, Label( "�w�i�Z�b�g" ), Tooltip( "�Q�[���v���C���̔w�i�G" )]
    private GameObject[] mpObjBackgroundArtSets;

    /* ���w�b�_�[UI */
    [field: SerializeField, Label( "���w�b�_�[UI" ), Tooltip( "�Q�[���v���C���̏�񂪕\�������UI" )]
    private GameObject mpObjInfoUI;

    /* ���U���gUI */
    [field: SerializeField, Label( "���U���gUI" ), Tooltip( "�������ԏI����ɕ\������郊�U���gUI" )]
    private GameObject mpObjResultUI;

    /* ���g���C�{�^��UI */
    [field: SerializeField, Label( "���g���C�{�^��UI" ), Tooltip( "���U���gUI���ɂ��郊�g���C�{�^��UI")]
    private Button mpButtonRetryUI;

    /* �C���X�^���X */
    private static WamMoleSlapGamemode mpInstance;

    /* ����݂̂̏��������s�������ǂ��� */
    private bool mbExecFirstProcess;

    /* ���ݎ��� */
    private ushort mCurrentTime;

    /* �t���[������ */
    private float mFlameTime;

    /* �����_���I��p���l */
    private byte mRandomNumber;

    /* ���݂̃X�R�A */
    private uint mCurrentScore;

    /* ���݂̂������@������ */
    private uint mCurrentSlapCount;

    /* ���U���g�����ǂ��� */
    private bool mbResult;

    /* �C���X�^���X���擾���� */
    public static WamMoleSlapGamemode GetInstance( )
    {
        return mpInstance;
    }

    public void Awake( )
    {
        /* �C���X�^���X��ۑ� */
        if ( WamMoleSlapGamemode.mpInstance == null )
        {
            WamMoleSlapGamemode.mpInstance = this;
        }

        /* ���g���C�{�^��UI����Ȃ� */
        if ( this.mpButtonRetryUI == null )
        {
            Debug.Log( "[Error] <WamMoleSlapGamemode> Retry button UI is null." );
            return;
        }

        this.mpButtonRetryUI.onClick.AsObservable( ).Subscribe( _ => this.Initialize( ) );
    }

    // Start is called before the first frame update
    public void Start( )
    {
        /* �Q�[�����[�h�̏����������� */
        this.Initialize( );
    }

    /* �Q�[�����[�h�̏����������� */
    private void Initialize( )
    {
        /* �����琶���Ǘ������������� */
        WamGameInstanceManager.GetInstance( ).GetMoleSpawnManagerInstance( ).Initialize( );

        /* �e��ϐ������� */
        this.mFlameTime                 = 0.0f;     /* �t���[������ */

        this.mCurrentScore              = 0;        /* ���݂̃X�R�A */
        this.mCurrentSlapCount          = 0;        /* ���݂̂������@������ */

        this.mbResult                   = false;    /* ���U���g�����ǂ��� */
        this.mbExecFirstProcess         = false;    /* ����݂̂̏��������s�������ǂ��� */


        /* ���ݎ��Ԃ��ő吧�����Ԃɐݒ� */
        this.mCurrentTime = this.mMaxTime;


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


        /* ���w�b�_�[UI����Ȃ� */
        if ( this.mpObjInfoUI == null )
        {
            Debug.Log( "[Error] <WamMoleSlapGamemode> Info UI is null." );
            return;
        }

        /* ���w�b�_�[UI��\�� */
        this.mpObjInfoUI.SetActive( true );

        /* ���U���gUI����Ȃ� */
        if ( this.mpObjResultUI == null )
        {
            Debug.Log( "[Error] <WamMoleSlapGamemode> Result UI is null." );
            return;
        }

        if ( this.mpObjResultUI.GetComponent<CanvasGroup>( ) == null )
        {
            Debug.Log( "[Error] <WamMoleSlapGamemode> Result UI is not add CanvasGroup component." );
            return;
        }

        /* ���U���gUI��\�� */
        this.mpObjResultUI.SetActive( true );

        /* ���U���gUI�̓����蔻��𖳂��� */
        this.mpObjResultUI.GetComponent<CanvasGroup>( ).blocksRaycasts = false;

        /* ���U���gUI�𓧖��x0�ɂ��� */
        this.mpObjResultUI.GetComponent<CanvasGroup>( ).alpha = 0.0f;
    }

    // Update is called once per frame
    public void Update( )
    {
        /* �������ݎ��Ԃ�0�b�ȉ��Ȃ� */
        if ( this.mCurrentTime <= 0 )
        {
            /* ���O�܂Ń��U���g���łȂ������ꍇ */
            if ( !this.mbResult )
            {
                /* ���U���g���Ƃ��� */
                this.mbResult = true;

                /* ���w�b�_�[UI����Ȃ� */
                if ( this.mpObjInfoUI == null )
                {
                    Debug.Log( "[Error] <WamMoleSlapGamemode> Info UI is null." );
                    return;
                }

                /* ���U���gUI����Ȃ� */
                if ( this.mpObjResultUI == null )
                {
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

        /* ����݂̂̏��������s���Ă��Ȃ��ꍇ */
        if ( !this.mbExecFirstProcess )
        {
            /* ���ݎ��Ԃ��ő吧�����ԂɂȂ������Ƃ�ʒm */
            this.OnTimerCountdown( this.mMaxTime );

            /* ���݃X�R�A�����������ꂽ���Ƃ�ʒm */
            this.OnScoreUpdate( 0 );

            /* ���݂������@�����񐔂����������ꂽ���Ƃ�ʒm */
            this.OnMoleSlapCountUpdate( 0 );

            /* ����݂̂̏��������s�����Ƃ��� */
            this.mbExecFirstProcess = true;
        }

        /* �t���[�����Ԃ����Z */
        this.mFlameTime += Time.deltaTime;

        /* ���v�t���[�����Ԃ�1�b�𒴂����� */
        if ( 1.0f <= this.mFlameTime )
        {
            /* ���v�t���[�����Ԃ���1�b���Z */
            this.mFlameTime -= 1.0f;

            /* ���ݎ��Ԃ�1�b�����Z */
            this.mCurrentTime--;

            /* ���ݎ��Ԃ��X�V���ꂽ���Ƃ�ʒm���� */
            this.OnTimerCountdown( this.mCurrentTime );
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
