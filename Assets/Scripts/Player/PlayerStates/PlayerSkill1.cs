using System.Collections;

using StateMachine;
using UnityEngine;

namespace Player
{
    public class PlayerSkill1 : State<Player>
    {
        private float _dmgOffset;
        private float _speed;

        public override void OnInitialized()
        {
            _dmgOffset = 2.5f;
            _speed = 15f;
        }

        public override void OnEnter()
        {
            Context.PState = PlayerState.Skill1State;
            
            Context.AttackCol.SetCol(ColType.Attack, 0f, (int)(Context.damage * _dmgOffset));
            Context.PlayerAnim.SetInteger("StateNum", (int)PlayerState.Skill1State);
            Context.StartCoroutine(MoveUpwardsCoroutine());
        }

        public override void Update()
        {
        
        }

        public override void FixedUpdate()
        {
        }
        
        private IEnumerator MoveUpwardsCoroutine()
        {
            Vector3 movement = _speed * Time.fixedDeltaTime * Vector3.up; // 위쪽 방향으로 이동하는 벡터

            float timer = 0f;

            while (timer < 1f)
            {
                Context.PlayerRigid.MovePosition(Context.transform.position + movement); // MovePosition 함수를 사용하여 이동

                timer += Time.deltaTime;
                yield return null;
            }
            
            Context.AttackCol.SetCol(ColType.None, 0f, 0);
            
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


