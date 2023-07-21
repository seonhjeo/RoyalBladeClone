using System.Collections;
using StateMachine;
using UnityEngine;

namespace Player
{
    public class PlayerAttack : State<Player>
    {
        private float _attackSpeed = 3f;
        private float _attackAnimTime = 1.1f;
        private float _attackYieldTime;

        public override void OnInitialized()
        {
            _attackYieldTime = _attackAnimTime / _attackSpeed;
        }

        public override void OnEnter()
        {
            Context.PState = PlayerState.AttackState;
            
            // 공격 콜라이더 설정
            Context.AttackCol.SetCol(ColType.Attack, 0f, Context.damage);
            
            // 애니메이션 설정
            Context.PlayerAnim.SetInteger("StateNum", (int)PlayerState.AttackState);
            Context.PlayerAnim.speed = _attackSpeed;
            Context.StartCoroutine(_FinishAttack());
        }

        public override void Update()
        {
            
        }

        private IEnumerator _FinishAttack()
        {
            yield return new WaitForSeconds(_attackYieldTime);

            Context.AttackCol.SetCol(ColType.None, 0f, 0);
            Context.PlayerAnim.speed = 1f;
            
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