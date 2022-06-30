using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    #region Public Field
    public List<MonsterCard> PlayerMonsterCards;
    public Transform MonstersParent;
    #endregion

    #region Private Field,

    [Space]
    [Range(-50, 50)]
    [SerializeField] private float m_ZOffset;
    [Range(-50, 50)]
    [SerializeField] private float m_XOffset;

    [Range(0, 10)]
    [SerializeField] private float m_MonsterParentMovementSmoothness;
    [SerializeField] private Vector3[] m_PlayerMonstersPositions;
    [SerializeField] private List<MonsterCard> MonsterCards;

    private GameManager m_GameManager;
    private Transform m_PlayerTransform;
    private Vector3 m_PlayerMonsterCenterPoint;
    private Vector3 m_RefPosition;
    #endregion

    #region Private Methods

    private void SetMonsterParentPosition()
    {
        Vector3 smoothedPosition;
        m_PlayerMonsterCenterPoint = m_PlayerTransform.position + new Vector3(0, 0, m_ZOffset);
        smoothedPosition = Vector3.SmoothDamp(MonstersParent.position, m_PlayerMonsterCenterPoint, ref m_RefPosition, m_MonsterParentMovementSmoothness);
        MonstersParent.position = new Vector3(smoothedPosition.x, smoothedPosition.y, m_PlayerTransform.position.z + m_ZOffset);
    }

    private void SetMonstersPositions()
    {
        m_PlayerMonstersPositions[0] = m_PlayerMonsterCenterPoint;
        m_PlayerMonstersPositions[1] = m_PlayerMonsterCenterPoint + new Vector3(m_XOffset, 0, 0);
        m_PlayerMonstersPositions[2] = m_PlayerMonsterCenterPoint - new Vector3(m_XOffset, 0, 0);
    }
    #endregion

    #region Public Methods
    public void Initialize(GameManager gameManager)
    {
        m_PlayerMonstersPositions = new Vector3[3];
        m_GameManager = gameManager;
        m_PlayerTransform = gameManager.PlayerController.transform;
        PlayerMonsterCards = new List<MonsterCard>();

        foreach (var item in MonsterCards)
        {
            item.Initialize(gameManager);
        }
    }
    public void Controller()
    {
        SetMonsterParentPosition();
    }

    public void AddPlayerMonsterCard(MonsterCard monsterCard)
    {
        SetMonstersPositions();
        PlayerMonsterCards.Add(monsterCard);
        monsterCard.CreateMonsterPrefab(m_PlayerMonstersPositions[PlayerMonsterCards.Count - 1], MonstersParent);
        m_GameManager.CameraMovement.SetZoom();
    }

    #endregion
}
