using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Attack
{
    AttackAnimationStart,
    DrawCurve,
    FollowPokeball,
    SpawnMonster
}


public class AttackController : MonoBehaviour
{
    #region Public Field
    public Attack AttackOrder;
    public int PlayerSelectedMonsterIndex;
    public int EnemySelectedMonsterIndex;
    #endregion

    #region Private Field
    [Space]

    [SerializeField] private bool m_PlayerThrewPokeball;
    [SerializeField] private bool m_EnemyThrewPokeball;
    [Space]
    [SerializeField] private List<ButtonMonsterCard> m_ButtonMonsterCards;
    [Space]
    [SerializeField] private GameManager GameManager;
    [Space]
    [SerializeField] private Transform m_EnemyMonsterSpawnPoint;
    [SerializeField] private Transform m_PlayerMonsterSpawnPoint;
    [SerializeField] private Transform m_PlayerFollowStartPoint;
    [SerializeField] private Transform m_EnemyFollowStartPoint;
    [SerializeField] private Transform m_PlayerPoint1;
    [SerializeField] private Transform m_EnemyPoint1;
    [Space]
    [SerializeField] private TextMeshPro m_PlayerMonsterPower;
    [SerializeField] private TextMeshPro m_EnemyMonsterPower;
    [Space]
    [SerializeField] private bool m_PlayerTurn;
    [SerializeField] private bool m_PlayerTurnAttackStart;
    [Space]
    [SerializeField] private Vector3[] m_CurvePositions;
    [Space]
    [SerializeField] private GameObject m_PokeballPrefab;
    [Space]
    [SerializeField] private float PokeballFollowSpeed;
    [Space]
    [SerializeField] private int m_ComparisonCount;
    [Space]

    private int m_NumPoints;
    private int m_TargetIndex;
    private int m_SelectedPlayerMonsterPower;
    private int m_SelectedEnemyMonsterPower;
    private int m_PlayerScore;
    private int m_EnemyScore;
    private bool m_OneTimeIncrease;
    private GameObject m_PlayerSelectedMonsterGameObject;
    private GameObject m_EnemySelectedMonsterGameObject;
    #endregion

    #region Public Methods
    public void Initialize()
    {
        m_PlayerTurn = false;
        PokeballFollowSpeed = 10;
        m_NumPoints = 50;
        m_CurvePositions = new Vector3[m_NumPoints];
    }
    public void Controller()
    {
        if (GameManager.PlayerController.FinishWay)
        {
            if (!m_PlayerThrewPokeball || !m_EnemyThrewPokeball)
            {
                if (m_PlayerTurn)
                {
                    if (m_PlayerTurnAttackStart)
                        PlayerAttack();
                }
                else
                {
                    EnemyAttack();
                }
            }
            else
            {
                StartCoroutine(PowerComparison());
            }
        }
    }

    public void WayFinishedInitialize()
    {
        SetButtonMonsterCards();
        m_EnemyMonsterPower.gameObject.SetActive(true);
        m_PlayerMonsterPower.gameObject.SetActive(true);
    }
    #endregion

    #region Private Methods
    private void IncreaseComparisonCount()
    {
        m_ComparisonCount++;
        if (m_ComparisonCount == 3)
        {
            if (m_EnemyScore < m_PlayerScore)
            {
                GameManager.LevelFinished(true, false);
            }
            else
            {
                GameManager.LevelFinished(false, true);
            }
        }
    }

    IEnumerator PowerComparison()
    {
        yield return new WaitForSeconds(1);

        if (m_OneTimeIncrease)
        {
            if (m_SelectedEnemyMonsterPower < m_SelectedPlayerMonsterPower)
            {
                m_PlayerScore++;
            }
            else if (m_SelectedEnemyMonsterPower == m_SelectedPlayerMonsterPower)
            {
                m_PlayerScore++;
                m_EnemyScore++;
            }
            else
            {
                m_EnemyScore++;
            }
            m_OneTimeIncrease = false;
        }

        m_PlayerMonsterPower.gameObject.SetActive(false);
        m_EnemyMonsterPower.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.15f);

        m_EnemySelectedMonsterGameObject.SetActive(false);
        m_PlayerSelectedMonsterGameObject.SetActive(false);

        yield return new WaitForSeconds(1);

        GameManager.UIManager.SetEnemyScore(m_EnemyScore);
        GameManager.UIManager.SetPlayerScore(m_PlayerScore);

        m_EnemyThrewPokeball = false;
        m_PlayerThrewPokeball = false;

        IncreaseComparisonCount();
        StopAllCoroutines();
    }

    private void ResetAttack()
    {
        m_TargetIndex = 0;
        AttackOrder = Attack.AttackAnimationStart;
        m_PokeballPrefab.SetActive(true);
        m_PokeballPrefab.transform.position = Vector3.zero;
        m_PlayerTurnAttackStart = false;
        m_OneTimeIncrease = true;
    }
    private void PlayerAttack()
    {
        switch (AttackOrder)
        {
            case Attack.AttackAnimationStart:
                GameManager.PlayerController.SetPlayerAnimation(PlayerAnimatorParameters.IsAttacking, true);

                if (GameManager.PlayerController.PlayerAttackAnimationFinish)
                {
                    GameManager.PlayerController.PlayerAttackAnimationFinish = false;
                    IncreaseAttackOrder();
                }

                break;
            case Attack.DrawCurve:
                PlayerDrawCurve();
                GameManager.PlayerController.SetPlayerAnimation(PlayerAnimatorParameters.IsAttacking, false);
                IncreaseAttackOrder();
                break;
            case Attack.FollowPokeball:
                FollowPokeball();

                break;
            case Attack.SpawnMonster:
                MonsterCard m_SelectedMonsterCard = GameManager.MonsterController.PlayerMonsterCards[PlayerSelectedMonsterIndex];
                m_PlayerSelectedMonsterGameObject = m_SelectedMonsterCard.Monster;
                m_PlayerMonsterPower.gameObject.SetActive(true);

                m_SelectedPlayerMonsterPower = m_SelectedMonsterCard.MonsterCardObject.Power;
                m_PlayerMonsterPower.text = m_SelectedPlayerMonsterPower.ToString();

                m_PlayerSelectedMonsterGameObject.transform.parent.gameObject.SetActive(true);
                m_PlayerSelectedMonsterGameObject.transform.position = m_PlayerMonsterSpawnPoint.position;
                m_PlayerThrewPokeball = true;
                m_PlayerTurn = false;

                ResetAttack();

                break;
        }
    }
    private void EnemyAttack()
    {
        switch (AttackOrder)
        {
            case Attack.AttackAnimationStart:
                GameManager.EnemyController.SetPlayerAnimation(EnemyAnimatorParameters.IsAttacking, true);

                if (GameManager.EnemyController.EnemyAttackAnimationFinish)
                {
                    GameManager.EnemyController.EnemyAttackAnimationFinish = false;
                    IncreaseAttackOrder();
                }

                break;
            case Attack.DrawCurve:
                EnemyDrawCurve();
                GameManager.EnemyController.SetPlayerAnimation(EnemyAnimatorParameters.IsAttacking, false);
                IncreaseAttackOrder();
                break;
            case Attack.FollowPokeball:
                FollowPokeball();

                break;
            case Attack.SpawnMonster:

                MonsterCardObject monsterCardObject = GameManager.EnemyController.EnemyMonsterCards[EnemySelectedMonsterIndex];
                m_EnemySelectedMonsterGameObject = Instantiate(monsterCardObject.Prefab, m_EnemyMonsterSpawnPoint.position, m_EnemyMonsterSpawnPoint.rotation);

                m_EnemyMonsterPower.gameObject.SetActive(true);
                m_SelectedEnemyMonsterPower = monsterCardObject.Power;
                m_EnemyMonsterPower.text = m_SelectedEnemyMonsterPower.ToString();

                m_PlayerTurn = true;
                m_EnemyThrewPokeball = true;
                EnemySelectedMonsterIndex++;
                ResetAttack();
                break;
        }
    }

    private void IncreaseAttackOrder()
    {
        AttackOrder = (Attack)((int)AttackOrder + 1);
    }

    private void PlayerDrawCurve()
    {
        Vector3 p0 = m_PlayerFollowStartPoint.position;
        Vector3 p2 = m_PlayerMonsterSpawnPoint.position;

        m_PokeballPrefab.transform.position = p0;

        DrawQuadraticCurve(p0, m_PlayerPoint1.position, p2);
    }

    private void EnemyDrawCurve()
    {
        Vector3 p0 = m_EnemyFollowStartPoint.position;
        Vector3 p2 = m_EnemyMonsterSpawnPoint.position;

        m_PokeballPrefab.transform.position = p0;

        DrawQuadraticCurve(p0, m_EnemyPoint1.position, p2);
    }

    private void FollowPokeball()
    {
        if (Vector3.Distance(m_PokeballPrefab.transform.position, m_CurvePositions[m_TargetIndex]) < 0.01f)
        {
            if (m_CurvePositions.Length - 1 >= m_TargetIndex + 1)
                m_TargetIndex++;
            else
            {
                m_PokeballPrefab.SetActive(false);
                IncreaseAttackOrder();
            }
        }
        else
        {
            m_PokeballPrefab.transform.position = Vector3.MoveTowards(m_PokeballPrefab.transform.position, m_CurvePositions[m_TargetIndex], PokeballFollowSpeed * Time.deltaTime);
        }
    }

    private void DrawQuadraticCurve(Vector3 P0, Vector3 P1, Vector3 P2)
    {
        for (int i = 1; i < m_NumPoints + 1; i++)
        {
            float t = i / (float)m_NumPoints;
            m_CurvePositions[i - 1] = CalculateQuadraticBezierPoint(t, P0, P1, P2);
        }
    }
    private void SetButtonMonsterCards()
    {

        for (int i = 0; i < m_ButtonMonsterCards.Count; i++)
        {
            m_ButtonMonsterCards[i].ID = i;

            MonsterCardObject monsterCardObject = GameManager.MonsterController.PlayerMonsterCards[i].MonsterCardObject;
            m_ButtonMonsterCards[i].SetButtonCard(
            monsterCardObject.Sprite,
             monsterCardObject.Power,
            monsterCardObject.Color);

            int index = i;
            m_ButtonMonsterCards[index].ButtonMonster.onClick.AddListener(() =>
            {
                m_PlayerTurnAttackStart = true;
                PlayerSelectedMonsterIndex = index;
            });
        }
    }
    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        //return   (1-t)2 P0 + 2(1-t)  t   P1 + t2   P2
        //           u           u              tt
        //           uu * P0 + 2 * u * t * P1 + tt * P2
        float u = (1 - t);
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
        return p;
    }

    #endregion


}
