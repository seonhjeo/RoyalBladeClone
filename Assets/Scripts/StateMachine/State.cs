
namespace StateMachine
{
    public abstract class State<T>
    {
        protected StateMachine<T> StateMachine;
        protected T Context;

        internal void SetContext(StateMachine<T> stateMachine, T context)
        {
            StateMachine = stateMachine;
            Context = context;
            
            OnInitialized();
        }
        
        /// <summary>
        /// 상태 머신에 상태가 추가될 때 한 번 실행되는 함수
        /// </summary>
        public virtual void OnInitialized()
        {}
        
        /// <summary>
        /// 상태로 진입할 때 실행되는 함수
        /// </summary>
        public virtual void OnEnter()
        {}

        /// <summary>
        /// 실시간으로 갱신되는 함수
        /// </summary>
        public abstract void Update();
        
        /// <summary>
        /// 물리적으로 실시간으로 갱신되는 함수
        /// </summary>
        public virtual void FixedUpdate()
        {}

        /// <summary>
        /// 상태를 빠져나갈 때 실행되는 함수
        /// </summary>
        public virtual void OnExit()
        {}
    }
}
