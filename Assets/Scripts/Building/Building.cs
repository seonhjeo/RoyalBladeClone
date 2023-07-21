
using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Building
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Building : MonoBehaviour
    {
        #region Private Values
        
        private int _health;

        private SpriteRenderer _sprite;
        
        private IObjectPool<Building> _managedPool;

        private Rigidbody2D _chunkRigid;

        private Action _actionOnDestroyed;

        #endregion


        #region Public Methods

        /// <summary>
        /// 빌딩 초기화 함수
        /// </summary>
        /// <param name="health">빌딩의 체력</param>
        /// <param name="rigid">청크의 리지드바디 참조</param>
        /// <param name="destroyAction">파괴될 때 청크에서 수행할 액션</param>
        public void Init(int health, Rigidbody2D rigid, Action destroyAction, Sprite sprite)
        {
            _health = health;
            _chunkRigid = rigid;
            _actionOnDestroyed += destroyAction;
            _sprite.sprite = sprite;
        }

        /// <summary>
        /// 체력을 없애는 함수
        /// </summary>
        /// <param name="amount">닳는 체력의 양</param>
        public void LoseHealth(int amount)
        {
            _health -= amount;
            
            if (_health <= 0)
            {
                _BuildingDestroy();
            }
        }

        /// <summary>
        /// 위로 밀리는 함수
        /// </summary>
        /// <param name="force">위로 미는 힘의 크기</param>
        public void GetPushed(float force)
        {
            _chunkRigid.velocity = Vector2.zero;
            _chunkRigid.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        }

        public void SetManagedPool(IObjectPool<Building> pool)
        {
            _managedPool = pool;
        }

        public float ReturnHeight()
        {
            return _sprite.bounds.size.y;
        }

        #endregion
        
        
        #region Mono Methods

        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
        }

        #endregion

        
        #region Private Methods

        /// <summary>
        /// 빌딩이 부서질 때 호출되는 함수.
        /// </summary>
        private void _BuildingDestroy()
        {
            _actionOnDestroyed.Invoke();
            _actionOnDestroyed = null;
            _managedPool.Release(this);
        }

        #endregion
        

    } 
}


