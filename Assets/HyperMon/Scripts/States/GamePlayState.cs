public class GamePlayState : State
{
    public GamePlayState(GameManager gameManager) : base(StateType.GamePlay, gameManager)
    {

    }

    public override void Enter()
    {
        base.Enter();
        m_GameManager.HasTheGameStarted = true;
        m_GameManager.PlayerController.SetPlayerAnimation(PlayerAnimatorParameters.IsRunning, true);
        m_GameManager.AttackController.Initialize();
    }

    public override void Update()
    {
        base.Update();
        m_GameManager.PlayerController.Controller();
        m_GameManager.MonsterController.Controller();
        m_GameManager.AttackController.Controller();
    }


    public override void Exit()
    {
        base.Exit();
        m_GameManager.UIManager.ChangePanel(Panels.Attack);
    }
}
