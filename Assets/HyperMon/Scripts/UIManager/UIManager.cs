using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum Panels
{
    Main,
    GamePlay,
    Attack,
    LevelComplete,
    Fail
}

public class UIManager : MonoBehaviour
{
    #region PublicFields
    public GameManager GameManager;
    #endregion

    #region PrivateFields
    [SerializeField] private List<Panel> GamePanels = new List<Panel>();
    [Space]
    [SerializeField] private Button m_TapToPlay;
    [SerializeField] private Button m_Restart;
    [SerializeField] private Button m_NextLevel;
    [Space]
    [SerializeField] private TextMeshProUGUI m_EnemyScoreText;
    [SerializeField] private TextMeshProUGUI m_PlayerScoreText;

    private Panel m_CurrentPanel;
    private StateManager m_StateManager;
    #endregion

    #region UnityMethods

    void Start()
    {
        Initialize();
    }
    #endregion UnityMethods

    #region Public Methods
    public void SetEnemyScore(int score)
    {
        m_EnemyScoreText.text = score.ToString();
    }

    public void SetPlayerScore(int score)
    {
        m_PlayerScoreText.text = score.ToString();
    }
    public void ChangePanel(Panels currentStateEnum)
    {
        if (m_CurrentPanel != null)
        {
            m_CurrentPanel.gameObject.SetActive(false);
        }
        for (int i = 0; i < GamePanels.Count; i++)
        {
            if (GamePanels[i].CurrentPanel == currentStateEnum)
            {
                m_CurrentPanel = GamePanels[i];
                m_CurrentPanel.gameObject.SetActive(true);
            }
        }
    }
    #endregion

    #region Methods
    private void Initialize()
    {
        m_StateManager = GameManager.GameStateManager;
        m_TapToPlay.onClick.AddListener(() => m_StateManager.ChangeState(State.StateType.GamePlay));

        m_Restart.onClick.AddListener(() => LevelRestart());
        m_NextLevel.onClick.AddListener(() => LevelRestart());
    }

    private void LevelRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion Methods
}
