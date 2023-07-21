using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cam
{
    public class CamController : MonoBehaviour
    {
        #region private Values

        private Transform _playerTrans;

        #endregion


        #region Mono Methods

        private void Awake()
        {
            _playerTrans = GameObject.FindWithTag("Player").GetComponent<Transform>();
        }

        private void LateUpdate()
        {
            Vector3 pos = new Vector3(0f, _playerTrans.transform.position.y, -10f);
            
            if (pos.y < 0f)
            {
                pos.y = 0f;
            }

            transform.position = pos;
        }

        #endregion
    }
}

