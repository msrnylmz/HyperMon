using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MonsterCard : MonoBehaviour
{
    #region Public Field
    public MonsterCardObject MonsterCardObject;
    public GameObject Monster;
    #endregion

    #region Private Field
    private GameManager m_GameManager;
    [SerializeField] private MonsterCard m_DuoMonsterCard;

    [SerializeField] private TextMeshPro m_Type;
    [SerializeField] private TextMeshPro m_Namet;
    [SerializeField] private TextMeshPro m_Power;
    [SerializeField] private TextMeshPro m_Price;
    [SerializeField] private SpriteRenderer m_Icon;
    [SerializeField] private SpriteRenderer m_BG;
    #endregion

    #region Public Methods
    public void Initialize(GameManager gameManager)
    {
        m_GameManager = gameManager;
        SetCard();
    }
    #endregion

    #region Private Methods

    private void SetCard()
    {
        m_Type.text = MonsterCardObject.Type;
        m_Namet.text = MonsterCardObject.Name;
        m_Power.text = MonsterCardObject.Power.ToString();
        m_Price.text = MonsterCardObject.Price.ToString();
        m_Icon.sprite = MonsterCardObject.Sprite;
        m_BG.color = MonsterCardObject.Color;
    }

    #endregion

    #region Public Methods
    public GameObject CreateMonsterPrefab(Vector3 position, Transform parent)
    {
        Monster = Instantiate(MonsterCardObject.Prefab, position, Quaternion.identity, parent);
        return Monster;
    }

    public void DuoMonsterCardDestroy()
    {
        m_DuoMonsterCard.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    #endregion
}
