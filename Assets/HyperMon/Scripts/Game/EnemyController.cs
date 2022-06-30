using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyAnimatorParameters
{
    IsAttacking
}


public class EnemyController : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;
    public MonsterCardObject[] EnemyMonsterCards = new MonsterCardObject[3];

    public bool EnemyAttackAnimationFinish;

    #region Public Methods
    public void Controller()
    {

    }

    public void SetPlayerAnimation(EnemyAnimatorParameters selectedParameters, bool control)
    {
        m_Animator.SetBool(selectedParameters.ToString(), control);
    }

    public void SetBoolEnemyAttackAnimationFinish()
    {
        EnemyAttackAnimationFinish = true;
        SetPlayerAnimation(EnemyAnimatorParameters.IsAttacking, false);
    }

    #endregion
}
