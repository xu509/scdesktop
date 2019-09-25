using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace scdesktop
{

    /// <summary>
    /// 参照点
    /// </summary>
    public class RefPointAgent : MonoBehaviour
    {
        [SerializeField]
        RefPointAgent[] _nearlyRefPointAgents;

        [SerializeField]
        SpriteRenderer _spriteRenderer;

        [SerializeField]
        bool _needReversal;
        public bool needReversal { get { return _needReversal; } }



        public RefPointAgent[] nearlyRefPointAgents { get { return _nearlyRefPointAgents; } }


        RefPointStatusEnum _refPointStatus;
        public RefPointStatusEnum refPointStatus { set { _refPointStatus = value; } get { return _refPointStatus; } }


        BallAgent _ballAgent; // 关联的BallAgent
        public BallAgent ballAgent { set { _ballAgent = value; } get { return _ballAgent; } }



        public enum RefPointStatusEnum {
            vacant,busy
        }


        // Start is called before the first frame update
        void Start()
        {
            _refPointStatus = RefPointStatusEnum.vacant;
        }

        // Update is called once per frame
        void Update()
        {

        }


        public void Clear() {
            _ballAgent = null;
            _refPointStatus = RefPointStatusEnum.vacant;
            ClearMark();
        }



        public void Mark() {
            //_spriteRenderer.color = Color.red;
        }

        public void ClearMark()
        {
            //_spriteRenderer.color = Color.white;
        }

    }

}