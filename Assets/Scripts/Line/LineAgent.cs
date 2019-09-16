using UnityEngine;


namespace scdesktop {
    
    /// <summary>
    /// Line 代理
    /// </summary>
    public class LineAgent : MonoBehaviour
    {
        [Header("UI"), SerializeField] LineRenderer _lineRenderer;

        [SerializeField, Range(0, 400)] int _points;   // 连接点的个数，4个点组成一个完整的循环        
        [SerializeField, Range(1f, 3f)] float _density;   // 密度，单个画面的区间数量。
        [SerializeField, Range(0, 360)] int _xoffset; // x的偏移量


        [SerializeField, Range(0.5f, 2f)] float _speed; // x的移动速度
        [SerializeField, Range(0.5f, 2f)] float _yoffset; // y的偏移高度


        LinesManager _linesManager;     // lines 管理器
        Vector3 _startPoint;    // 开始点
        Vector3 _endPoint;  // 结束点
        float _lineWidth;   // 线条宽度

        bool _initComplete = false;


        // Start is called before the first frame update
        void Start()
        {
            if (_initComplete) {
                Print();
            }
            
        }

        // Update is called once per frame
        void Update()
        {
            if (_initComplete)
            {
                Print();
            }
        }


        
        /// <summary>
        ///     初始化
        /// </summary>
        public void Init(Vector3 startPoint,Vector3 endPoint,float lineWidth,
            float density,float speed,int xoffset,float yoffset,LinesManager linesManager)
        {
            _linesManager = linesManager;
            _startPoint = startPoint;
            _endPoint = endPoint;
            _lineWidth = lineWidth;
            _density = density;
            _speed = speed;
            _xoffset = xoffset;
            _yoffset = yoffset;

            _lineRenderer.startWidth = _lineWidth;
            _lineRenderer.endWidth = _lineWidth;

            _initComplete = true;
        }








        void Print()
        {
            float dnumber = _points / _density; // 获取每个区间的使用点数量

            _lineRenderer.positionCount = _points + 1;

            for (int i = 0; i <= _points; i++)
            {
                // 获取横坐标

                //Time.time

                float t = (float)i / (float)_points;
                //Debug.Log(t);
                var point = Vector3.LerpUnclamped(_startPoint, _endPoint, t);

                float x = point.x;


                float r = i % dnumber;
                float t2 = r / dnumber;

                float a = Mathf.Lerp(-Mathf.PI, Mathf.PI, t2);

                a += Time.time * _speed + _xoffset / 365;

                float y = Mathf.Sin(a);

                y = y * _yoffset;

                // 获取纵坐标
                Vector3 position = new Vector3(x, point.y + y, point.z);

                _lineRenderer.SetPosition(i, position);
            }
        }





        void OnDrawGizmos()
        {
            Gizmos.color = Color.gray;
            //Gizmos.DrawLine(_startPoint, _endPosition.position);

        }
    }

}
