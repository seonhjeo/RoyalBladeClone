using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private TextMesh _text;


    #region Public Methods

    public void Init(int damage)
    {
        _text.text = damage.ToString();
    }

    #endregion
    
    
    #region Mono Methods

    private void Awake()
    {
        _text = GetComponent<TextMesh>();
    }
    
    private void Start()
    {
        StartCoroutine(_LifeCycle());
    }

    #endregion
    
    
    #region Private Methods

    /// <summary>
    /// 데미지 텍스트의 라이프 사이클, 1초 후에 본인을 Destroy한다
    /// </summary>
    private IEnumerator _LifeCycle()
    {
        float timer = 0f;

        while (timer < 1f)
        {
            transform.position += new Vector3(0, 3f * Time.deltaTime, 0);

            timer += Time.deltaTime;
            yield return null;
        }
        
        Destroy(gameObject);
    }

    #endregion

    
}
