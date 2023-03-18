using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WamCharacterMole : MonoBehaviour
{
    /* �A�j���[�^�[�X�e�[�g���i���u�j */
    [field: SerializeField, Label( "�A�j���[�^�[�X�e�[�g���i���u�j" ), Tooltip( "�����炪���u����Ă��鎞�̃A�j���[�^�[�X�e�[�g��" )]
    private string mStateNameIdle;

    /* �A�j���[�^�[�X�e�[�g���i�I���j */
    [field: SerializeField, Label( "�A�j���[�^�[�X�e�[�g���i�I���j" ), Tooltip( "�����炪�ޏꂩ���ł�����̃A�j���[�^�[�X�e�[�g��" )]
    private string mStateNameEnd;

    /* �A�j���[�^�[�p�����[�^�[���i�X�e�[�g�؂�ւ��j */
    [field: SerializeField, Label( "�A�j���[�^�[�p�����[�^�[���i�X�e�[�g�؂�ւ��j" ), Tooltip( "������̃A�j���[�^�[�X�e�[�g��؂�ւ��邽�߂̃p�����[�^�[��" )]
    private string mParameterNameStateChange;

    /* ���u�X�e�[�g����J�ڊJ�n����܂ł̍ŏ����� */
    [field: SerializeField, Label( "���u�X�e�[�g����J�ڊJ�n����܂ł̍ŏ�����" ), Tooltip( "���u�X�e�[�g����J�ڊJ�n����܂ł̍ŏ�����" ), Range( 0.0f , 100.0f )]
    private float mStateIdleWaitTimeMin;

    /* ���u�X�e�[�g����J�ڊJ�n����܂ł̍ő剄������ */
    [field: SerializeField, Label( "���u�X�e�[�g����J�ڊJ�n����܂ł̍ő剄������" ), Tooltip( "���u�X�e�[�g����J�ڊJ�n����܂ł̍ő剄�����ԂŁA�ŏ����ԂɃ����_���ŉ��Z�����" ), Range( 0.0f , 100.0f )]
    private float mStateIdleWaitTimeMaxExt;

    /* �A�j���[�^�[ */
    private Animator mpAnimator;

    /* ���u�X�e�[�g�ɓ��������ǂ��� */
    private bool mbStateIdle;

    /* ���u�X�e�[�g����J�ڊJ�n����܂ł̌��݂̑ҋ@���� */
    private float mStateIdleWaitTime;

    /* ���u�X�e�[�g����J�ڊJ�n����܂ł̃����_���������� */
    private float mStateIdleWaitTimeRandomExt;

    // Start is called before the first frame update
    public void Start()
    {
        /* ���g�ɐݒ肳��Ă���A�j���[�^�[�R���|�[�l���g���擾 */
        this.mpAnimator = this.GetComponent<Animator>( );

        /* �A�j���[�V��������̏ꍇ */
        if ( this.mpAnimator == null )
        {
            Debug.Log( "[Error] <WamCharacterMole> Animator component is null." );
            return;
        }

        /* �A�j���[�^�[�X�e�[�g���i���u�j����̏ꍇ */
        if ( string.IsNullOrEmpty( this.mStateNameIdle ) )
        {
            Debug.Log( "[Error] <WamCharacterMole> Animator state idle name is null or empty." );
            return;
        }

        /* �A�j���[�^�[�X�e�[�g���i�I���j����̏ꍇ */
        if ( string.IsNullOrEmpty( this.mStateNameEnd ) )
        {
            Debug.Log( "[Error] <WamCharacterMole> Animator state end name is null or empty." );
            return;
        }

        /* �A�j���[�^�[�p�����[�^�[���i�X�e�[�g�؂�ւ��j����̏ꍇ */
        if ( string.IsNullOrEmpty( this.mParameterNameStateChange ) )
        {
            Debug.Log( "[Error] <WamCharacterMole> Animator parameter state change name is null or empty." );
            return;
        }

        /* ���u�X�e�[�g����J�ڊJ�n����܂ł̌��݂̑ҋ@���Ԃ������� */
        this.mStateIdleWaitTime = 0.0f;

        /* ���O�ɗ����̃V�[�h�l��ύX���āA���������̏������s�� */
        Random.InitState( System.DateTime.Now.Millisecond );

        /* ���u�X�e�[�g����J�ڊJ�n����܂ł̃����_���������Ԃ��擾���� */
        this.mStateIdleWaitTimeRandomExt = Random.Range( 0.0f , this.mStateIdleWaitTimeMaxExt );
    }

    // Update is called once per frame
    public void Update()
    {
        /* �A�j���[�V��������̏ꍇ */
        if ( this.mpAnimator == null )
        {
            return;
        }

        /* ���u�X�e�[�g�ɂ���ꍇ */
        if ( this.mpAnimator.GetCurrentAnimatorStateInfo( 0 ).IsName( this.mStateNameIdle ) )
        {
            /* ���O�܂ŕ��u�X�e�[�g�ɓ����Ă��Ȃ������ꍇ�A�������Ƃ��� */
            if ( !this.mbStateIdle )
            {
                this.mbStateIdle = true;
            }
            /* ���u�X�e�[�g�ɓ����Ă��鎞 */
            else
            {
                /* �o�ߎ��Ԃ����Z���� */
                this.mStateIdleWaitTime += Time.deltaTime;

                /* �ҋ@���Ԃ��w�莞�Ԃɓ��B���Ă��Ȃ���΁A�������Ȃ� */
                if ( this.mStateIdleWaitTime < ( this.mStateIdleWaitTimeMin + this.mStateIdleWaitTimeRandomExt ) )
                {
                    return;
                }

                /* �ޏ�X�e�[�g�ɐ؂�ւ��� */
                this.mpAnimator.SetInteger( this.mParameterNameStateChange , 1 );
            }
        }
        /* �I���X�e�[�g�ɂ���ꍇ */
        else if ( this.mpAnimator.GetCurrentAnimatorStateInfo( 0 ).IsName( this.mStateNameEnd ) )
        {
            /* �������g���폜���� */
            Destroy( this.gameObject );
        }
    }
}
