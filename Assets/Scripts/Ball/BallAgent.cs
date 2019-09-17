using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;

namespace scdesktop
{
    public class BallAgent : MonoBehaviour
    {
        [SerializeField] SpriteRenderer spriteRenderer;

        private static float SPEED_CONST = 0.5f;        
        private float _speed;
        private Vector3 _clickScreenOffset;


        private CardAgent _refCardAgent;
        public CardAgent refCardAgent { set { _refCardAgent = value; } get { return _refCardAgent; } }


        private BallStatusEnum _ballStatus;
        public BallStatusEnum ballStatus { set { _ballStatus = value; } get { return _ballStatus; } }

        private BallsManager _ballsManager;


        // Start is called before the first frame update
        void Start()
        {
            //_speed = 1f;
        }

        // Update is called once per frame
        void Update()
        {
            if (_ballStatus == BallStatusEnum.flow)
            {
                Vector3 to = new Vector3(-Time.deltaTime * SPEED_CONST * (_speed + 1), 0, 0);
                transform.Translate(to);

                CheckOverBords();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="speed">0 -> 1</param>
        /// <param name="birthPosition"></param>
        /// <param name="scaleFactor">0.5 -> 2.0</param>
        public void Init(float speed,Vector3 birthPosition,float scaleFactor,BallsManager ballsManager) {
            _ballsManager = ballsManager;

            _speed = speed;
            transform.position = birthPosition;

            var scale = GetComponent<Transform>().localScale;
            scale.x *= scaleFactor;
            scale.y *= scaleFactor;
            GetComponent<Transform>().localScale = scale;

            _ballStatus = BallStatusEnum.flow;


            // 设置图片
            SpriteAtlas spriteAtlas = Resources.Load<SpriteAtlas>("SpriteAtlas");
            string[] spriteAtlasAry = { "1", "2", "3", "4" };
                     
            spriteRenderer.sprite = spriteAtlas.GetSprite(spriteAtlasAry[Random.Range(0, 4)]);


        }

        /// <summary>
        ///     ref ： https://www.yuque.com/u314548/lbo6va/phnmy7
        /// </summary>
        private void OpenCard() {

            Debug.Log("Mock Open Card");
            _ballsManager.ClearRefsMark();
            var refPoint = _ballsManager.GetAvailableRefPoint(transform.position);
            _ballsManager.OpenCard(refPoint, this);
        }

        /// <summary>
        ///  检查是否已经越界
        /// </summary>
        private void CheckOverBords() {
            Vector3 v = Camera.main.WorldToScreenPoint(transform.position);

            if (v.x < (0 - 200)) {
                ballStatus = BallStatusEnum.destorying;
            }
        }




        void OnMouseDown()
        {
            //OpenCard();
            //_ballStatus = BallStatusEnum.opening;
        }

        void OnMouseDrag() {
            // 拖拽逻辑
            var selfScreenPosition = Camera.main.WorldToScreenPoint(transform.position);

            if (_ballStatus != BallStatusEnum.dragging) {
                // 获取点击处与物体中心的偏移量
                var startDragPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, selfScreenPosition.z);
                _clickScreenOffset = selfScreenPosition - startDragPosition;
            }

            _ballStatus = BallStatusEnum.dragging;
            var toScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, selfScreenPosition.z) + _clickScreenOffset;

            // 屏幕坐标转换为相对坐标
            var mousePosition = Camera.main.ScreenToWorldPoint(toScreenPosition);
            transform.position = mousePosition;
        }

        private void OnMouseUp()
        {
            if (_ballStatus == BallStatusEnum.dragging)
            {
                OpenCard();
                _ballStatus = BallStatusEnum.opening;
            }
            else {
                //Debug.Log("点击");
            }

        }



    }

}
