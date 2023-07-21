
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

namespace Building
{
    public class BuildingChunk : MonoBehaviour
    {
        #region Private Values

        // Values for Object pool
        private int _buildingCount;

        private IObjectPool<Building> _pool;
        
        private Vector3 _initPos;
        [SerializeField] private GameObject buildingPrefab;
        [SerializeField] private List<Sprite> sprites;

        [SerializeField]private UnityEvent onBuildingDestroyed;
        
        // Value for Buildings
        private Rigidbody2D _chunkRigid;
        private int _buildingHealth;

        #endregion


        #region Mono Methods

        private void Awake()
        {
            _pool = new ObjectPool<Building>(CreateBuilding, ActivateBuilding, DeActivateBuilding, OnDestroyBuilding, maxSize:20);
            _chunkRigid = GetComponent<Rigidbody2D>();
            
            _initPos = transform.position;
        }
        
        #endregion
        
        
        #region Public Methods
        
        /// <summary>
        /// 게임을 시작할 때 실행하는 함수
        /// </summary>
        public void OnPlayStart()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            _InitRandom();
        }

        /// <summary>
        /// 게임이 종료되었을 때 실행하는 함수
        /// </summary>
        public void OnPlayEnd()
        {
            _chunkRigid.simulated = false;
        }

        #endregion



        #region Private Methods
        
        /// <summary>
        /// 블록 청크 초기화 함수
        /// </summary>
        /// <param name="health">생성되는 블록들의 체력</param>
        /// <param name="buildingCount">생성되는 블록들의 개수</param>
        /// <param name="sprite">생성되는 블록의 스프라이트</param>
        /// <param name="pos">생성되는 청크의 초기 위치</param>
        /// <param name="drag">생성되는 청크의 공기 저항력, 값이 높을수록 떨어지는 속도가 줄어든다</param>
        private void Init(int health, int buildingCount, Sprite sprite, Vector2 pos, float drag)
        {
            _chunkRigid.simulated = false;
            
            transform.position = _initPos;
            _buildingCount = buildingCount;

            for (int i = 0; i < buildingCount; i++)
            {
                Building b = _pool.Get();
                b.Init(health, _chunkRigid, _CheckAllDestroyed, sprite);
                
                b.transform.position = pos;
                b.transform.SetParent(transform);
                
                pos.y += b.ReturnHeight();
            }

            _chunkRigid.drag = drag;
            _chunkRigid.simulated = true;
        }
        
        /// <summary>
        /// 모든 변수를 무작위로 설정해 빌딩들을 초기화해주는 함수
        /// </summary>
        private void _InitRandom()
        {
            _pool.Clear();
            
            Sprite randomSprite = sprites[Random.Range(0, sprites.Count - 1)];
            int h = Random.Range(1, 10);
            int c = Random.Range(10, 20);
            float v = Random.Range(-3f, 3f);
            float d = Random.Range(0f, 2f);

            Init(h, c, randomSprite, _initPos + new Vector3(0, v, 0), d);
        }
        
        /// <summary>
        /// 청크 내 모든 빌딩이 사라졌나 확인하는 함수. 다 사라지면 새 청크를 생성
        /// </summary>
        private void _CheckAllDestroyed()
        {
            _buildingCount--;
            onBuildingDestroyed?.Invoke();

            if (_buildingCount == 0)
            {
                transform.position = _initPos;
                _InitRandom();
            }
        }
        
        // 풀이 비어있을 경우 오브젝트 인스턴스를 생성하여 반환하는 함수
        private Building CreateBuilding()
        {
            Building b = Instantiate(buildingPrefab).GetComponent<Building>();
            b.SetManagedPool(_pool);
            return b;
        }

        // 오브젝트 인스턴스를 풀에서 빼낼 때 실행하는 함수
        private void ActivateBuilding(Building enemy)
        {
            enemy.gameObject.SetActive(true);
        }
        
        // 오브젝트 인스턴스를 풀에 다시 넣을 때 실행하는 함수
        private void DeActivateBuilding(Building enemy)
        {
            enemy.gameObject.SetActive(false);
        }

        // 오브젝트 인스턴스를 삭제하는 함수
        private void OnDestroyBuilding(Building enemy)
        {
            Destroy(enemy.gameObject);
        }

        #endregion
    }
}


