using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Public Field
    public StateManager GameStateManager;
    public PlayerController PlayerController;
    public EnemyController EnemyController;
    public CameraMovement CameraMovement;
    public MonsterController MonsterController;
    public AttackController AttackController;
    public UIManager UIManager;

    [Space]
    public bool HasTheGameStarted;
    public bool IsLevelCompleted;
    public bool IsFailed;

    #endregion PublicFields

    #region Private Field
    private int m_InsufficientPokeballCount;
    #endregion

    #region UnityMethods
    void Awake()
    {
        Initialize();
    }
    void Update()
    {
        Controller();
    }
    #endregion UnityMethods

    #region Private Methods
    private void Initialize()
    {
        InitializeGameStates();
        GameStateManager.StartState(State.StateType.MainMenu);
    }
    private void InitializeGameStates()
    {
        GameStateManager = new StateManager();
        GameStateManager.AddState(new MainMenuState(this));
        GameStateManager.AddState(new GamePlayState(this));
        GameStateManager.AddState(new GameOverState(this));
    }

    private void Controller()
    {
        GameStateManager.UpdateController();
    }


    #endregion

    #region Public Fields

    public void GameOverInitialize()
    {
        if (IsFailed)
        {
            Failed();
        }
        else if (IsLevelCompleted)
        {
            LevelCompleted();
        }
    }
    public void IncraseInsufficientCount()
    {
        m_InsufficientPokeballCount++;
    }
    public void InsufficientCountReset()
    {
        m_InsufficientPokeballCount = 0;
    }
    public void InsufficientLevelFailedControl()
    {
        if (m_InsufficientPokeballCount == 3 && PlayerController.IsMovement)
        {
            IsFailed = true;
            GameStateManager.ChangeState(State.StateType.GameOver);
        }
    }

    public void LevelFinished(bool isLevelCompleted, bool isFailed)
    {
        IsLevelCompleted = isLevelCompleted;
        IsFailed = isFailed;
        GameStateManager.ChangeState(State.StateType.GameOver);
    }

    public void Failed()
    {
        UIManager.ChangePanel(Panels.Fail);
        PlayerController.SetPlayerAnimation(PlayerAnimatorParameters.IsRunning, false);
        PlayerController.SetPlayerAnimation(PlayerAnimatorParameters.IsAttacking, false);

    }

    public void LevelCompleted()
    {
        UIManager.ChangePanel(Panels.LevelComplete);
        PlayerController.SetPlayerAnimation(PlayerAnimatorParameters.IsRunning, false);
        PlayerController.SetPlayerAnimation(PlayerAnimatorParameters.IsAttacking, false);
    }

    #endregion

}
