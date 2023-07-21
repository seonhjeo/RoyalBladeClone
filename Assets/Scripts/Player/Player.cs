
using UnityEngine;

using StateMachine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Player
{
    public enum PlayerState
    {
        IdleState = 0,
        JumpState = 1,
        AttackState = 2,
        DefenseState = 3,
        DiedState = 4,
        Skill1State = 5,
    }
    
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        #region Internal Values

        internal Animator PlayerAnim;
        internal Rigidbody2D PlayerRigid;
        internal BoxCollider2D PlayerCol;
        internal AttackCollider AttackCol;
        
        internal PlayerInputAction PInput;

        [SerializeField] internal float jumpForce;
        [SerializeField] internal float pushForce;
        [SerializeField] internal int damage;

        internal PlayerState PState;

        #endregion

        
        #region Private Values

        private StateMachine<Player> _stateMachine;
        
        [SerializeField] private float groundOffset;

        [SerializeField] private UnityEvent onPlayerDead;

        private int _health;

        #endregion


        #region Public Methods

        /// <summary>
        /// 게임을 시작할 때 실행되는 함수, GameManager에서 Event 형식으로 실행된다
        /// </summary>
        public void OnPlayStart()
        {
            PInput.Player.Jump.started += OnJump;
            PInput.Player.Attack.started += OnAttack;
            PInput.Player.Defence.started += OnDefenceStart;
            PInput.Player.Skill.started += OnSkillStart;
            PInput.Player.Enable();

            PlayerAnim.enabled = true;
        }

        /// <summary>
        /// 게임이 종료되었을 때 실행되는 함수, GameManager에서 Event 형식으로 실행된다
        /// </summary>
        public void OnPlayEnd()
        {
            PInput.Player.Jump.started -= OnJump;
            PInput.Player.Attack.started -= OnAttack;
            PInput.Player.Defence.started -= OnDefenceStart;
            PInput.Player.Skill.started -= OnSkillStart;
            PInput.Player.Disable();
            
            PlayerAnim.enabled = false;
        }

        #endregion
        

        #region Mono Methods

        private void Awake()
        {
            // 컴포넌트 가져오기
            PlayerAnim = GetComponent<Animator>();
            PlayerRigid = GetComponent<Rigidbody2D>();
            PlayerCol = GetComponent<BoxCollider2D>();

            AttackCol = transform.GetChild(0).GetComponent<AttackCollider>();
            
            PInput = new PlayerInputAction();

            // 상태 머신 생성
            _stateMachine = new StateMachine<Player>(this, new PlayerIdle());
            _stateMachine.AddState(new PlayerJump());
            _stateMachine.AddState(new PlayerAttack());
            _stateMachine.AddState(new PlayerDefence());
            _stateMachine.AddState(new PlayerSkill1());

        }

        private void OnEnable()
        {
            OnPlayStart();
        }

        private void OnDisable()
        {
            OnPlayEnd();
        }

        private void Update()
        {
            _stateMachine.Update();
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
        }

        /// <summary>
        /// 플레이어가 땅에 있을 때 머리에 블록이 닿으면 게임을 종료시키는 함수
        /// </summary>
        private void OnCollisionStay2D(Collision2D col)
        {
            Debug.Log(col.gameObject.tag);
            if (col.gameObject.CompareTag("BuildingChunks"))
            {
                if (IsGrounded())
                {
                    onPlayerDead?.Invoke();
                }
            }
        }

        #endregion


        #region Input Methods

        /// <summary>
        /// 점프키를 눌렀을 때 실행되는 함수
        /// </summary>
        private void OnJump(InputAction.CallbackContext context)
        {
            if (context.action.phase == InputActionPhase.Started && PState != PlayerState.Skill1State && IsGrounded())
            {
                _stateMachine.ChangeState<PlayerJump>();
            }
        }

        /// <summary>
        /// 공격키를 눌렀을 때 실행되는 함수
        /// </summary>
        private void OnAttack(InputAction.CallbackContext context)
        {
            if (context.action.phase == InputActionPhase.Started && PState != PlayerState.Skill1State)
            {
                _stateMachine.ChangeState<PlayerAttack>();
            }
        }

        /// <summary>
        /// 방어키를 눌렀을 때 실행되는 함수
        /// </summary>
        private void OnDefenceStart(InputAction.CallbackContext context)
        {
            if (context.action.phase == InputActionPhase.Started && PState != PlayerState.Skill1State)
            {
                _stateMachine.ChangeState<PlayerDefence>();
            }
        }

        /// <summary>
        /// 스킬키를 눌렀을 때 실행되는 함수
        /// </summary>
        private void OnSkillStart(InputAction.CallbackContext context)
        {
            if (context.action.phase == InputActionPhase.Started)
            {
                _stateMachine.ChangeState<PlayerSkill1>();
            }
        }

        #endregion
        

        #region Private Methods

        /// <summary>
        /// 플레이어가 땅에 닿아있는지 확인하는 함수
        /// </summary>
        /// <returns>닿아있으면 true를 반환</returns>
        internal bool IsGrounded()
        {
            Bounds b = PlayerCol.bounds;
            
            Vector2 colliderCenter = b.center;
            Vector2 colliderSize = b.size;
            Vector2 raycastOrigin = colliderCenter + new Vector2(0f, -colliderSize.y / 2f);
            
            RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, groundOffset);
            
            if (hit && hit.collider.CompareTag("Ground"))
            {
                return true;
            }
            
            return false;
        }

        #endregion
    }

}

