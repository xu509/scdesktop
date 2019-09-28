using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using DG.Tweening;

namespace scdesktop
{
    public class BallAgent : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
    {
        [SerializeField] SpriteRenderer spriteRenderer;

        private static float SPEED_CONST = 0.5f;
        private float _speed;
        private Vector3 _clickScreenOffset;
        private bool isReversal;
        private Vector3 _lastActivePosition;

        private Tweener _moveTweener;
        public Tweener MoveTweener { set { _moveTweener = value; } get { return _moveTweener; } }

        private Tweener _smallTweener;
        public Tweener SmallTweener { set { _smallTweener = value; } get { return _smallTweener; } }


        private float effectDistance = 300f;




        private BallData _ballData;
        public BallData ballData { get { return _ballData; } }




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
                Vector3 to;
                if (isReversal)
                {
                    to = new Vector3(Time.deltaTime * SPEED_CONST * (_speed + 1), 0, 0);
                }
                else
                {
                    to = new Vector3(-Time.deltaTime * SPEED_CONST * (_speed + 1), 0, 0);
                }

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
        public void Init(float speed, Vector3 birthPosition, float scaleFactor, BallsManager ballsManager, BallData ballData)
        {
            _ballsManager = ballsManager;
            _ballData = ballData;


            _speed = speed;
            transform.position = birthPosition;

            var scale = GetComponent<Transform>().localScale;
            scale.x *= scaleFactor;
            scale.y *= scaleFactor;
            GetComponent<Transform>().localScale = scale;

            _ballStatus = BallStatusEnum.flow;


            string coverAddress = "data/" + ballData.cover;

            //Debug.Log("coverAddress : " + coverAddress);

            var sprite = Resources.Load<Sprite>(coverAddress);
            spriteRenderer.sprite = sprite;


            // 设置图片
            //SpriteAtlas spriteAtlas = Resources.Load<SpriteAtlas>("SpriteAtlas");
            //string[] spriteAtlasAry = { "1", "2", "3", "4" };

            //spriteRenderer.sprite = spriteAtlas.GetSprite(spriteAtlasAry[Random.Range(0, 4)]);

            // 随机正反显示
            float rotRange = Random.Range(0, 1f);
            if (rotRange > 0.5f)
            {
                isReversal = true;
                GetComponent<RectTransform>().DORotate(new Vector3(180, 180, 0), Time.deltaTime);
            }
            else
            {
                isReversal = false;
            }

        }

        /// <summary>
        ///     ref ： https://www.yuque.com/u314548/lbo6va/phnmy7
        /// </summary>
        private void OpenCard()
        {

            //Debug.Log("Mock Open Card");
            _ballsManager.ClearRefsMark();
            var refPoint = _ballsManager.GetAvailableRefPoint(transform.position);
            _ballsManager.OpenCard(refPoint, this);
        }

        /// <summary>
        ///  检查是否已经越界
        /// </summary>
        private void CheckOverBords()
        {
            Vector3 v = Camera.main.WorldToScreenPoint(transform.position);

            if (v.x < (0 - 200))
            {
                ballStatus = BallStatusEnum.destorying;
            }
        }




        void OnMouseDown()
        {
            //OpenCard();
            //_ballStatus = BallStatusEnum.opening;
        }


        private bool CheckAvailablePostion(Vector3 mousePosition)
        {
            //var d = Vector3.Distance(_lastActivePosition, mousePosition);

            //return d < effectDistance;

            return true;

        }



        public void OnPointerUp(PointerEventData data)
        {
            print("OnPointerUp..");
        }



        public void OnDrag(PointerEventData eventData)
        {
            if (_ballStatus == BallStatusEnum.opening) {
                return; 
            }


            var selfScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
            //var selfScreenPosition = eventData.position;
            var mousePosition = new Vector3(eventData.position.x, eventData.position.y, selfScreenPosition.z);


            if (_ballStatus != BallStatusEnum.dragging)
            {
                // 获取点击处与物体中心的偏移量
                var startDragPosition = mousePosition;
                _clickScreenOffset = selfScreenPosition - startDragPosition;
                _lastActivePosition = startDragPosition;
            }

            _ballStatus = BallStatusEnum.dragging;
            //var toScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, selfScreenPosition.z) + _clickScreenOffset;
            var toScreenPosition = mousePosition + _clickScreenOffset;


            //Debug.Log("Distance : " + Vector3.Distance(_lastActivePosition, toScreenPosition));

            if (CheckAvailablePostion(toScreenPosition))
            {
                _lastActivePosition = toScreenPosition;

                var mouseWordPosition = Camera.main.ScreenToWorldPoint(toScreenPosition);
                transform.position = mouseWordPosition;
            }
            else
            {
                //超出影响范围
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //Debug.Log("click!");
            DoOpenCard();

            //throw new System.NotImplementedException();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            //throw new System.NotImplementedException();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_ballStatus == BallStatusEnum.dragging)
            {
                DoOpenCard();
            }
            else
            {
                //Debug.Log("点击");
            }
        }


        private void DoOpenCard() {
            if (_ballStatus != BallStatusEnum.opening) {
                _ballStatus = BallStatusEnum.opening;
                OpenCard();
            }


        }



        public void DestoryIt() {
            _smallTweener?.Kill();
            _moveTweener?.Kill();
            _ballStatus = BallStatusEnum.destorying;

        }


    }

}
