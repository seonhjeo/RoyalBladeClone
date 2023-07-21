using StateMachine;


namespace Player
{
    public class PlayerIdle : State<Player>
    {
        public override void OnEnter()
        {
            Context.PState = PlayerState.IdleState;
            
            Context.PlayerAnim.SetInteger("StateNum", (int)PlayerState.IdleState);
        }

        public override void Update()
        {
        }
    }
}