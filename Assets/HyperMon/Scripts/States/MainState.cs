public class MainMenuState : State
{
    public MainMenuState(GameManager gameManager) : base(StateType.MainMenu, gameManager)
    {

    }

    public override void Enter()
    {
        base.Enter();
        m_GameManager.MonsterController.Initialize(m_GameManager);
        m_GameManager.UIManager.ChangePanel(Panels.Main);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
        m_GameManager.UIManager.ChangePanel(Panels.GamePlay);
    }
}
