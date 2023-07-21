using System;
using System.Collections.Generic;

namespace StateMachine
{
    public class StateMachine<T>
    {
        private T _context;

        private State<T> _currentState;
        public State<T> CurrentState => _currentState;

        private State<T> _previousState;
        public State<T> PreviousState => _previousState;

        private Dictionary<Type, State<T>> _states = new Dictionary<Type, State<T>>();

        public StateMachine(T context, State<T> initialState)
        {
            this._context = context;

            AddState(initialState);
            _currentState = initialState;
            _currentState.OnEnter();
        }

        /// <summary>
        /// 상태 머신에 상태를 추가하는 함수
        /// </summary>
        /// <param name="state">제네릭 형을 정한 상태의 자식 클래스</param>
        public void AddState(State<T> state)
        {
            state.SetContext(this, _context);
            _states[state.GetType()] = state;
        }

        /// <summary>
        /// Update 함수, Context의 Update구문에서 호출
        /// </summary>
        public void Update()
        {
            _currentState.Update();
        }
        
        /// <summary>
        /// FixedUpdate 함수, Context의 FixedUpdate구문에서 호출
        /// </summary>
        public void FixedUpdate()
        {
            _currentState.FixedUpdate();
        }

        /// <summary>
        /// 상태를 변경할 때 사용하는 함수
        /// </summary>
        /// <typeparam name="T2">변경할 상태</typeparam>
        public void ChangeState<T2>() where T2 : State<T>
        {
            if (typeof(T2) == _currentState.GetType())
                return;

            if (_currentState != null)
                _currentState.OnExit();
            _previousState = _currentState;
            _currentState = _states[typeof(T2)];
            _currentState.OnEnter();
        }
    }
}
