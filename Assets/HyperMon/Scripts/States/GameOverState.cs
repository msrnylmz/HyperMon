public class GameOverState : State
{
    public GameOverState(GameManager gameManager) : base(StateType.GameOver, gameManager)
    {

    }

    public override void Enter()
    {
        base.Enter();
        m_GameManager.GameOverInitialize();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
