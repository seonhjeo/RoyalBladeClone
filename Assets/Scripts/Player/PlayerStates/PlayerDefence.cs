using StateMachine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerDefence : State<Player>
    {
        public override void OnEnter()
        {
            Context.PState = PlayerState.DefenseState;
            
            Context.PInput.Player.Defence.canceled += OnDefenseEnd;
            Context.AttackCol.SetCol(ColType.Defence, Context.pushForce, 0);
            Context.AttackCol.gameObject.SetActive(true);
            Context.AttackCol.gameObject.SetActive(false);
            
            Context.PlayerAnim.SetInteger("StateNum", (int)PlayerState.DefenseState);
        }

        public override void Update()
        {
            
        }

        internal void OnDefenseEnd(InputAction.CallbackContext context)
        {
            Context.AttackCol.SetCol(ColType.None, Context.pushForce, 0);
            Context.PInput.Player.Defence.canceled -= OnDefenseEnd;
            
            if (Context.IsGrounded() && Context.PlayerRigid.velocity.y < 0.1f)
            {
                StateMachine.ChangeState<PlayerIdle>();
            }
            else
            {
                StateMachine.ChangeState<PlayerJump>();
            }
            
        }
    }
}