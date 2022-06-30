public interface IState
{
    int ID { get; }
    void Enter();
    void Update();
    void Exit();
}