using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace scdesktop
{
    public class CardAgent : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
    {
        // 关闭时间       
        [SerializeField] float _destoryIntervalTime;
        [SerializeField] Image _detailCover;

        [SerializeField] Sprite _btnCloseSprite;
        [SerializeField] RectTransform _closeBtn;


        private Tweener _openTweener;
        public Tweener OpenTweener { set { _openTweener = value; } get { return _openTweener; } }


        private float _lastActiveTime;
        private float _destoryStartTime;

        private BallAgent _refBallAgent;
        public BallAgent refBallAgent { get { return _refBallAgent; } }

        private RefPointAgent _refPointAgent;
        public RefPointAgent refPointAgent { get { return _refPointAgent; } }

        private CardStatusEnum _status;
        public CardStatusEnum Status { get { return _status; } } 


        private static float DESTORY_CONFIRM_TIME = 3f;        


        public enum CardStatusEnum {
            Open,Destorying,DestoryingCompleted,Destoryed,Recover
        }



        // Start is called before the first frame update
        void Start()
        {
            _lastActiveTime = Time.time;
            _status = CardStatusEnum.Open;

            _destoryStartTime = Mathf.Infinity;
        }

        // Update is called once per frame
        void Update()
        {
            if ((Time.time - _lastActiveTime) > _destoryIntervalTime  && (_status  == CardStatusEnum.Open)) {
                // 进行第一次缩小
                //Debug.Log("进行第一次缩小");
                _status = CardStatusEnum.Destorying;
                var nowScale = GetComponent<Transform>().localScale;
                var toScale = nowScale * 0.6f;
                GetComponent<Transform>().DOScale(toScale, 1.5f)
                    .OnComplete(() => {
                        _destoryStartTime = Time.time;
                        // ...
                    });
            }

            if ((Time.time - _destoryStartTime) > DESTORY_CONFIRM_TIME && (_status == CardStatusEnum.Destorying))
            {
                //Debug.Log("进行销毁");

                _status = CardStatusEnum.DestoryingCompleted;
                // 销毁cardagent;
                GetComponent<Transform>().DOScale(Vector3.zero, 0.5f)
                    .OnComplete(() => {
                        _status = CardStatusEnum.Destoryed;                                        
                    });
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="birthWorldPosition">世界坐标</param>
        public void Init(Vector3 birthWorldPosition,BallAgent ballAgent,RefPointAgent refPointAgent) {
            GetComponent<Transform>().position = birthWorldPosition;
            Vector3 oriScale = new Vector3(0.2f, 0.2f, 1);
            GetComponent<Transform>().localScale = oriScale;

            _refBallAgent = ballAgent;
            _refPointAgent = refPointAgent;




            // 设置图片
            string detailCoverStr = "data/" + ballAgent.ballData.detailCover;
            _detailCover.sprite = Resources.Load<Sprite>(detailCoverStr);

        }


        /// <summary>
        /// 直接关闭
        /// </summary>
        public void DirectClose() {

            if (_openTweener.active) {
                _openTweener.Kill();
            }


            _status = CardStatusEnum.DestoryingCompleted;
            // 销毁cardagent;
            GetComponent<Transform>().DOScale(Vector3.zero, 0.5f)
                .OnComplete(() => {
                    _status = CardStatusEnum.Destoryed;
            }).SetEase(Ease.InBack);
        }


        public void Recover() {
            _status = CardStatusEnum.Recover;

            Debug.Log("进行恢复");

            Vector3 to = new Vector3(1.6f, 1.6f, 1f);

            DoUpdated();

            GetComponent<Transform>().DOScale(to, 1f)
                .OnComplete(() => {
                    _status = CardStatusEnum.Open;
                    DoUpdated();
                });
        }


        public void DoClose() {
            _closeBtn.GetComponent<Image>().sprite = _btnCloseSprite;

            DirectClose();

        }

        private void DoUpdated() {
            if (_status == CardStatusEnum.Destorying) {
                Recover();
            }

            _lastActiveTime = Time.time;

        }






        public void OnEndDrag(PointerEventData eventData)
        {
            //throw new System.NotImplementedException();
        }

        public void OnDrag(PointerEventData eventData)
        {
            //throw new System.NotImplementedException();
            DoUpdated();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //Debug.Log("click!");
            DoUpdated();

            //throw new System.NotImplementedException();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            //throw new System.NotImplementedException();
        }
    }


}
