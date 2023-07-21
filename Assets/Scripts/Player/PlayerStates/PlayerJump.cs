using StateMachine;
using UnityEngine;

namespace Player
{
    public class PlayerJump : State<Player>
    {
        public override void OnEnter()
        {
            Context.PState = PlayerState.JumpState;
            
            if (Context.IsGrounded())
            {
                Context.PlayerRigid.AddForce(Vector2.up * Context.jumpForce, ForceMode2D.Impulse);
            }
            Context.PlayerAnim.SetInteger("StateNum", (int)PlayerState.JumpState);
        }
        
        public override void Update()
        {
            Context.PlayerAnim.SetFloat("YVel", Context.PlayerRigid.velocity.y);
        }

        public override void FixedUpdate()
        {
            if (Context.IsGrounded() && Context.PlayerRigid.velocity.y < 0.1f)
            {
                StateMachine.ChangeState<PlayerIdle>();
            }
            
        }
    }
}