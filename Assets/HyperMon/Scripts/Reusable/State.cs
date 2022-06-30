public class State : IState
{
    public enum StateType
    {
        MainMenu,
        GamePlay,
        GameOver
    }

    protected StateType m_StateType;
    protected GameManager m_GameManager;
    protected StateManager m_StateManager;

    private int m_ID;
    public int ID { get { return m_ID; } }

    public State(StateType stateType, GameManager gameManager)
    {
        m_GameManager = gameManager;
        m_StateManager = gameManager.GameStateManager;
        m_StateType = stateType;
        m_ID = (int)stateType;
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }

    public virtual void Update()
    {

    }

}