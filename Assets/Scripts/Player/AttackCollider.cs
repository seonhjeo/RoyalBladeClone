
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;


namespace Player
{
    public enum ColType
    {
        None,
        Attack,
        Defence
    }
    
    public class AttackCollider : MonoBehaviour
    {
        #region Private Values

        [SerializeField] private DamageText damageText;
        
        private ColType _currentType;
        private float _pushForce;
        private int _damage;

        #endregion


        #region Public Methods

        public void SetCol(ColType curType, float pushForce, int damage)
        {
            _currentType = curType;
            _pushForce = pushForce;
            _damage = damage;
        }

        #endregion
        
        
        #region Mono Methods

        private void OnEnable()
        {
            _AttackOrPush();
        }

        #endregion


        #region Private Methods

        private void _AttackOrPush()
        {
            Collider2D colliders = Physics2D.OverlapBox(transform.position, transform.localScale, 0f, LayerMask.NameToLayer("Building"));

            if (!colliders.IsUnityNull() && colliders.CompareTag("Building"))
            {
                Building.Building b = colliders.gameObject.GetComponent<Building.Building>();
                
                if (_currentType == ColType.Attack)
                {
                    b.LoseHealth(_damage);
                    
                    DamageText d = Instantiate(damageText, transform.position, quaternion.identity);
                    d.Init(_damage);
                }
                else if (_currentType == ColType.Defence)
                {
                    b.GetPushed(_pushForce);
                }
            }
        }

        #endregion
    }
}


