using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

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

    /* �v���C�G���A */
    [field: SerializeField, Label( "�v���C�G���A" ), Tooltip( "�����炪�o������͈�" )]
    private GameObject mpObjPlayArea;

    /* ������A�^�b�`��e�I�u�W�F�N�g */
    [field: SerializeField, Label( "������A�^�b�`��e�I�u�W�F�N�g" ), Tooltip( "��������������v���n�u���A�^�b�`����e�I�u�W�F�N�g" )]
    private GameObject mpObjMoleAttachParent;

    /* �v���n�u�i������j */
    [field: SerializeField, Label( "�v���n�u�i������j" ), Tooltip( "���I�����_���������������̃v���n�u" )]
    private GameObject mpObjMole;

    /* ������̐����C���^�[�o���i�ŏ��j */
    [field: SerializeField, Label( "������̐����C���^�[�o���i�ŏ��j" ), Tooltip( "�����炪���������܂łɑҋ@���K�v�ȍŏ��C���^�[�o������" ), Range( 0.0f , 100.0f )]
    private float mMoleSpawnIntavalMin;

    /* ������̐����C���^�[�o���i�ő剄���j */
    [field: SerializeField, Label( "������̐����C���^�[�o���i�ő剄���j" ), Tooltip( "�����炪���������܂łɑҋ@���K�v�ȍő剄���C���^�[�o�����ԂŁA�ŏ����Ԃɉ��Z�����" ), Range( 0.0f , 100.0f )]
    private float mMoleSpawnIntavalMaxExt;

    /* ������Ԃ̍ŏ��������� */
    [field: SerializeField, Label( "������Ԃ̍ŏ���������" ), Tooltip( "�O�񐶐����ꂽ�����炩��󂯂�ŏ�����" ), Range( 0.0f , 10000.0f )]
    private float mMoleSpawnDistanceMin;

    /* ���w�b�_�[UI */
    [field: SerializeField, Label( "���w�b�_�[UI" ), Tooltip( "�Q�[���v���C���̏�񂪕\�������UI" )]
    private GameObject mpObjInfoUI;

    /* ���U���gUI */
    [field: SerializeField, Label( "���U���gUI" ), Tooltip( "�������ԏI����ɕ\������郊�U���gUI" )]
    private GameObject mpObjResultUI;

    /* �C���X�^���X */
    private static WamMoleSlapGamemode mpInstance;

    /* �������������ǂ��� */
    private bool mbInitialized;

    /* ���ݎ��� */
    private ushort mCurrentTime;

    /* �t���[������ */
    private float mFlameTime;

    /* �����琶���܂ł̌��݂̃C���^�[�o������ */
    private float mCurrentSpawnIntervalTime;

    /* �����琶���܂ł̃����_�������C���^�[�o������ */
    private float mRandomExtSpawnIntervalTime;

    /* ������̐������WX�� */
    private float mMoleSpawnLocationX;

    /* ������̐������WY�� */
    private float mMoleSpawnLocationY;

    /* �����_���I��p���l */
    private byte mRandomNumber;

    /* ��������������v���n�u */
    private GameObject mpCurrentMole;

    /* ������Ԃ̌��݂̐������� */
    private float mMoleSpawnDistance;

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
        if ( mpInstance == null )
        {
            mpInstance = this;
        }
    }

    // Start is called before the first frame update
    public void Start( )
    {
        /* ���ݎ��Ԃ��ő吧�����Ԃɐݒ� */
        this.mCurrentTime = this.mMaxTime;

        /* �t���[�����Ԃ������� */
        this.mFlameTime = 0.0f;

        /* �����琶���܂ł̌��݂̃C���^�[�o�����Ԃ������� */
        this.mCurrentSpawnIntervalTime = 0.0f;

        /* ������Ԃ̌��݂̐������� */
        this.mMoleSpawnDistance = 0.0f;

        /* ���݂̃X�R�A�������� */
        this.mCurrentScore = 0;

        /* ���݂̂������@�����񐔂������� */
        this.mCurrentSlapCount = 0;

        /* ���U���g�����ǂ����������� */
        this.mbResult = false;

        /* ��������������v���n�u�������� */
        this.mpCurrentMole = null;

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

        /* ���O�ɗ����̃V�[�h�l��ύX���āA���������̏������s�� */
        Random.InitState( System.DateTime.Now.Millisecond );

        /* �����_���Ŕw�i�Z�b�g��I�� */
        this.mRandomNumber = (byte)Random.Range( 0 , ( this.mpObjBackgroundArtSets.Length - 1 ) );

        /* �w�i�Z�b�g����ł͂Ȃ����� */
        if ( this.mpObjBackgroundArtSets[this.mRandomNumber] != null )
        {
            /* �w�i�Z�b�g��\������ */
            this.mpObjBackgroundArtSets[this.mRandomNumber].SetActive( true );
        }

        /* �����琶���܂ł̃����_�������C���^�[�o�����Ԃ��擾���� */
        this.mRandomExtSpawnIntervalTime = Random.Range( 0.0f , this.mMoleSpawnIntavalMaxExt );


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

        /* ���������Ă��Ȃ��ꍇ */
        if ( !this.mbInitialized )
        {
            /* ���ݎ��Ԃ��ő吧�����ԂɂȂ������Ƃ�ʒm */
            this.OnTimerCountdown( this.mMaxTime );

            /* ���݃X�R�A�����������ꂽ���Ƃ�ʒm */
            this.OnScoreUpdate( 0 );

            /* ���݂������@�����񐔂����������ꂽ���Ƃ�ʒm */
            this.OnMoleSlapCountUpdate( 0 );

            /* �������ς݂Ƃ��� */
            this.mbInitialized = true;
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

        /* �����琶���܂ł̌��݂̃C���^�[�o�����Ԃ��A�w�莞�Ԃɓ��B���Ă��Ȃ��ꍇ */
        if ( this.mCurrentSpawnIntervalTime < ( this.mMoleSpawnIntavalMin + this.mRandomExtSpawnIntervalTime ) )
        {
            /* �C���^�[�o�����Ԃ����Z���� */
            this.mCurrentSpawnIntervalTime += Time.deltaTime;
            return;
        }

        /* ������v���n�u����Ȃ� */
        if ( this.mpObjMole == null )
        {
            Debug.Log( "[Error] <WamMoleSlapGamemode> Mole prefab is null." );
            return;
        }

        /* �v���C�G���A����Ȃ� */
        if ( this.mpObjPlayArea == null )
        {
            Debug.Log( "[Error] <WamMoleSlapGamemode> PlayArea is null." );
            return;
        }

        /* ������A�^�b�`��e�I�u�W�F�N�g����Ȃ� */
        if ( this.mpObjMoleAttachParent == null )
        {
            Debug.Log( "[Error] <WamMoleSlapGamemode> Mole prefab attach parent object is null." );
            return;
        }

        /* ���O�ɗ����̃V�[�h�l��ύX���āA���������̏������s�� */
        Random.InitState( System.DateTime.Now.Millisecond );

        /* ������̐������W�������_���Ŏ擾���� */
        RectTransform rect = this.mpObjPlayArea.GetComponent<RectTransform>( );
        this.mMoleSpawnLocationX = Random.Range( ( this.mpObjPlayArea.transform.position.x - ( this.mpObjPlayArea.GetComponent<RectTransform>( ).rect.width / 2 ) ) ,
                                                ( this.mpObjPlayArea.transform.position.x + ( this.mpObjPlayArea.GetComponent<RectTransform>( ).rect.width / 2 ) ) );
        this.mMoleSpawnLocationY = Random.Range( ( this.mpObjPlayArea.transform.position.y - ( this.mpObjPlayArea.GetComponent<RectTransform>( ).rect.height / 2 ) ) ,
                                                ( this.mpObjPlayArea.transform.position.y + ( this.mpObjPlayArea.GetComponent<RectTransform>( ).rect.height / 2 ) ) );

        /* ���O�ɐ����ς݂̂�����v���n�u�����݂���ꍇ */
        if ( this.mpCurrentMole != null )
        {
            /* ������Ԃ̌��݂̐����������擾 */
            this.mMoleSpawnDistance = Vector2.Distance( this.mpCurrentMole.transform.position , new Vector2( this.mMoleSpawnLocationX , this.mMoleSpawnLocationY ) );
                                
            /* �O�񐶐�����������ƁA���񐶐�����\��̂�����̋������A�w�苗�������̏ꍇ */
            if ( this.mMoleSpawnDistance < this.mMoleSpawnDistanceMin )
            {
                /* ������x�����_���ō��W����点���� */
                return;
            }
        }

        /* �����_���Ŏ擾�������W�ɂ�����v���n�u�𐶐����� */
        this.mpCurrentMole = Instantiate( this.mpObjMole , new Vector2( this.mMoleSpawnLocationX , this.mMoleSpawnLocationY ) , Quaternion.identity , this.mpObjMoleAttachParent.transform );

        /* ������v���n�u�̐������������Ă��Ȃ��ꍇ */
        if ( this.mpCurrentMole == null )
        {
            Debug.Log( "[Failed] <WamMoleSlapGamemode> Mole prefab create failed." );
            return;
        }

        /* �����琶���܂ł̌��݂̃C���^�[�o�����Ԃ������� */
        this.mCurrentSpawnIntervalTime = 0.0f;

        /* �����琶���܂ł̃����_�������C���^�[�o�����Ԃ��擾���� */
        this.mRandomExtSpawnIntervalTime = Random.Range( 0.0f , this.mMoleSpawnIntavalMaxExt );
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
