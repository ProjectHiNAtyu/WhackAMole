using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WamMoleSlapGamemode : MonoBehaviour
{
    /* �ő吧������ */
    [field: SerializeField, Label( "�ő吧������" ), Tooltip( "�Q�[���J�n���̍ő吧������" ), Range( 0 , 65535 )]
    private ushort mMaxTime;

    /* �e�L�X�g���b�V���v���i���ݎ��ԁj */
    [field: SerializeField, Label( "�e�L�X�gUI�i���ݎ��ԁj" ), Tooltip( "���ݎ��Ԃ�\������e�L�X�gUI" )]
    private TextMeshProUGUI mTmpCurrentTime;

    /* �e�L�X�g���b�V���v���i�@�����񐔁j */
    [field: SerializeField, Label( "�e�L�X�gUI�i�@�����񐔁j" ), Tooltip( "�������@�����񐔂�\������e�L�X�gUI" )]
    private TextMeshProUGUI mTmpCurrentHitCount;

    /* �e�L�X�g���b�V���v���i�X�R�A�j */
    [field: SerializeField, Label( "�e�L�X�gUI�i�X�R�A�j" ), Tooltip( "�v���C���̍��v�X�R�A��\������e�L�X�gUI" )]
    private TextMeshProUGUI mTmpCurrentScore;

    /* �w�i�Z�b�g */
    [field: SerializeField, Label( "�w�i�Z�b�g" ), Tooltip( "�Q�[���v���C���̔w�i�G" )]
    private GameObject[] mObjBackgroundArtSet;

    /* ���ݎ��� */
    private ushort mCurrentTime;

    /* �t���[������ */
    private float mFlameTime;

    /* �����_���I��p���l */
    private byte mRandomNumber;

    // Start is called before the first frame update
    void Start( )
    {
        /* �@�����񐔃e�L�X�g������������ */
        this.mTmpCurrentHitCount.SetText( "0" );

        /* �X�R�A�e�L�X�g������������ */
        this.mTmpCurrentScore.SetText( "0" );

        /* ���ݎ��ԃe�L�X�g�ɍő吧�����Ԃ�ݒ肷�� */
        this.mTmpCurrentTime.SetText( this.mMaxTime.ToString( ) );

        /* ���ݎ��Ԃ��ő吧�����Ԃɐݒ� */
        this.mCurrentTime = this.mMaxTime;

        /* �t���[�����Ԃ������� */
        this.mFlameTime = 0.0f;

        /* �w�i�Z�b�g�������[�v */
        for ( byte i = 0; i < this.mObjBackgroundArtSet.Length; i++ )
        {
            /* �w�i�Z�b�g����ł͂Ȃ����� */
            if ( this.mObjBackgroundArtSet[i] != null )
            {
                /* ��U�S�Ă̔w�i�Z�b�g���\���ɂ��� */
                this.mObjBackgroundArtSet[i].SetActive( false );
            }
        }

        /* ���O�ɗ����̃V�[�h�l��ύX���āA���������̏������s�� */
        Random.InitState( System.DateTime.Now.Millisecond );

        /* �����_���Ŕw�i�Z�b�g��I�� */
        this.mRandomNumber = (byte)Random.Range( 0 , ( this.mObjBackgroundArtSet.Length - 1 ) );

        /* �w�i�Z�b�g����ł͂Ȃ����� */
        if ( this.mObjBackgroundArtSet[this.mRandomNumber] != null )
        {
            /* �w�i�Z�b�g��\������ */
            this.mObjBackgroundArtSet[this.mRandomNumber].SetActive( true );
        }
    }

    // Update is called once per frame
    void Update( )
    {
        /* �������ݎ��Ԃ�0�b�ȏ�Ȃ� */
        if ( 0 < this.mCurrentTime )
        {
            /* �t���[�����Ԃ����Z */
            this.mFlameTime += Time.deltaTime;

            /* ���v�t���[�����Ԃ�1�b�𒴂����� */
            if ( 1.0f <= this.mFlameTime )
            {
                /* ���v�t���[�����Ԃ���1�b���Z */
                this.mFlameTime -= 1.0f;

                /* ���ݎ��Ԃ�1�b�����Z */
                this.mCurrentTime--;

                /* �e�L�X�g�Ɍ��ݎ��Ԃ�ݒ肷�� */
                this.mTmpCurrentTime.SetText( this.mCurrentTime.ToString( ) );
            }
        }
    }
}
