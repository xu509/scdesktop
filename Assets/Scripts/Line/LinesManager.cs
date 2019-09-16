using UnityEngine;
using System;
using System.Collections.Generic;

namespace scdesktop
{
    /// <summary>
    ///     线条群
    /// </summary>
    public class LinesManager : MonoBehaviour
    {
        [SerializeField, Header("UI")]
        Transform _startPoint;

        [SerializeField]
        Transform _lineContainer;

        [SerializeField]
        Transform _endPoint;

        [SerializeField, Header("Config"), Range(0f, 4f)]
        float _startGenRange;

        [SerializeField, Range(0f, 4f)]
        float _endGenRange;


        // 密度区间，一个画面中显示的区间数量
        [SerializeField, Range(1f,3f)]
        float _densityRangeStart;

        [SerializeField, Range(1f, 3f)]
        float _densityRangeEnd;

        // 速度
        [SerializeField,Range(0f,1f)]
        float _speedRangeStart;

        [SerializeField,Range(0f,1f)]
        float _speedRangeEnd;

        // 初始位置偏移量
        [SerializeField, Range(0f, 365f)]
        int _xoffsetRangeStart;

        [SerializeField, Range(0f, 365f)]
        int _xoffsetRangeEnd;

        // 纵坐标（高度）的偏移量
        [SerializeField, Range(0.5f, 3f)]
        float _yoffsetRangeStart;

        [SerializeField, Range(0.5f, 3f)]
        float _yoffsetRangeEnd;


        [SerializeField,Header("number of lines"),Range(0,50)]
        int _number;

        [SerializeField,Header("Prefab")]
        LineAgent _lineAgentPrefab;

        // _lineAgents
        private List<LineAgent> _lineAgents;

        private bool _createLineLock = false;
        private bool _resetLinesLock = false;

        private bool _isStart = false;

        // Start is called before the first frame update
        void Start()
        {
            _lineAgents = new List<LineAgent>();

            _isStart = true;

            Debug.Log("start");

        }

        // Update is called once per frame
        void Update()
        {
            if ((_lineAgents.Count < _number) && (!_createLineLock)) {
                CreateLine();
            }
        }

        private void OnEnable()
        {
            Debug.Log("On Enable");
        }

        private void OnDisable()
        {
            _isStart = false;
        }


        /// <summary>
        /// inspector 中进行修改
        /// </summary>
        private void OnValidate()
        {
            if (_isStart) {
                if (!_resetLinesLock)
                {
                    _resetLinesLock = true;
                    ResetScene();
                    _resetLinesLock = false;
                }
            }
        }


        /// <summary>
        ///     重置
        /// </summary>
        private void ResetScene()
        {
            if (_lineAgents != null) {
                for (int i = 0; i < _lineAgents.Count; i++)
                {
                    Destroy(_lineAgents[i].gameObject);
                }
            }
            _lineAgents = new List<LineAgent>();
        }


        private void CreateLine() {
            _createLineLock = true;

            LineAgent lineAgent = Instantiate(_lineAgentPrefab, _lineContainer, false);

            var d = UnityEngine.Random.Range(0f, 1f);
            Vector3 startPoint = Vector3.Lerp(_startPoint.position + new Vector3(0, _startGenRange, 0), _startPoint.position - new Vector3(0, _startGenRange, 0), d);
            Vector3 endPoint = Vector3.Lerp(_endPoint.position + new Vector3(0, _endGenRange, 0), _endPoint.position - new Vector3(0, _endGenRange, 0), d);
            float lineWidth = Mathf.Lerp(0.2f, 0.4f, d);
            lineAgent.Init(startPoint, endPoint, lineWidth,
                GetDensityParam(), GetSpeedParam(), GetXOffsetParam(),
                GetYOffsetParam(),
                this);
            _lineAgents.Add(lineAgent);

            _createLineLock = false;
        }



        /// <summary>
        /// 获取密度参数
        /// </summary>
        /// <returns></returns>
        private float GetDensityParam() {
            var d = UnityEngine.Random.Range(0f, 1f);
            return Mathf.Lerp(_densityRangeStart, _densityRangeEnd, d);
        }

        private float GetSpeedParam() {
            var d = UnityEngine.Random.Range(0f, 1f);
            return Mathf.Lerp(_speedRangeStart, _speedRangeEnd, d);
        }

        private int GetXOffsetParam()
        {
            var d = UnityEngine.Random.Range(0f, 1f);

            var r = Mathf.Lerp((float)_xoffsetRangeStart, (float)_xoffsetRangeEnd, d);
            return Mathf.CeilToInt(r);
        }

        private float GetYOffsetParam()
        {
            var d = UnityEngine.Random.Range(0f, 1f);

            return Mathf.Lerp(_yoffsetRangeStart, _yoffsetRangeEnd, d);

        }


    }
}
