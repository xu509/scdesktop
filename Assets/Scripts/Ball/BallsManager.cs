﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace scdesktop
{

    /// <summary>
    /// 球体管理器
    /// </summary>
    public class BallsManager : MonoBehaviour
    {
        [SerializeField,Range(0f,1.0f),Header("CONFIG")] float _birthPositionOffset;
        [SerializeField,Range(0f,1.0f)] float _birthIntervalTimeRangeMin;
        [SerializeField,Range(0f,1.0f)] float _birthIntervalTimeRangeMax;
        [SerializeField,Range(0f,1.0f)] float _speedRangeMin;
        [SerializeField, Range(0f, 3.0f)] float _speedRangeMax;
        [SerializeField, Range(0.1f, 2.0f)] float _scaleRangeMin;
        [SerializeField, Range(0.1f, 2.0f)] float _scaleRangeMax;


        [SerializeField, Header("UI")] Transform _birthPosition;
        [SerializeField] BallAgent _ballAgentPrefab;
        [SerializeField] Transform _ballsContainer;
        [SerializeField] RefPointAgent[] _refPointAgents;
        [SerializeField] CardAgent _cardAgentPrefab;
        [SerializeField] CardAgent _cardAgentRePrefab;
        [SerializeField] Transform _cardContainer;

        [SerializeField] SymbolAgent _symbolAgentPrefab;


        MainManager _manager;



        private float _lastGenerateTime = -10f;    // 上一个生成的时间
        private GeneratePosition _lastGeneratePosition; //  生成位置


        private float _lastSymbolTime = -10f;    // 上一个生成的时间



        private List<BallAgent> _ballAgents;
        private List<CardAgent> _cardAgents;


        private static float BIRTH_POSITION_CONST = 4f;
        private static float BIRTH_INTERVAL_TIME_CONST = 1f;


        public void Init(MainManager manager)
        {
            _manager = manager;
        }



        enum GeneratePosition {
            UP,DOWN
        }


        // Start is called before the first frame update
        void Start()
        {
            _ballAgents = new List<BallAgent>();
            _cardAgents = new List<CardAgent>();
            _lastGeneratePosition = GeneratePosition.DOWN;
        }

        // Update is called once per frame
        void Update()
        {

            // 生成数据球
            if (CanGenerateBalls()) {
                float offsetY = Mathf.Lerp(-(BIRTH_POSITION_CONST * _birthPositionOffset), BIRTH_POSITION_CONST * _birthPositionOffset,Random.Range(0f,1f));
                offsetY = getYOffset(offsetY);

                var agent = Instantiate(_ballAgentPrefab,_ballsContainer);

                float offsetZ = Mathf.Lerp(-2f, -0.1f, Random.Range(0, 1f));

                var data = _manager.daoService.GetItem();
                agent.Init(Random.Range(0f,1f), _birthPosition.position + new Vector3(0,offsetY, offsetZ), getScaleFactor(),this,data);

                // 随机正反                

                _ballAgents.Add(agent);
            }

            if (CanGenerateSymbols()) {
                float offsetY = Mathf.Lerp(-(BIRTH_POSITION_CONST * _birthPositionOffset), BIRTH_POSITION_CONST * _birthPositionOffset, Random.Range(0f, 1f));
                offsetY = getYOffset(offsetY);
                var symbol = Instantiate(_symbolAgentPrefab, _ballsContainer);

                float offsetZ = Mathf.Lerp(-2f, -0.1f, Random.Range(0, 1f));
                symbol.Init(Random.Range(0f, 1f), _birthPosition.position + new Vector3(0, offsetY, offsetZ),Random.Range(0.15f,0.3f));

            }


            // 清理需要删除的数据球
            List<BallAgent> ballsNeedDestory = new List<BallAgent>();
            for (int i = 0; i < _ballAgents.Count; i++) {
                if (_ballAgents[i].ballStatus == BallStatusEnum.destorying) {
                    ballsNeedDestory.Add(_ballAgents[i]);
                }
            }

            for (int i = 0; i < ballsNeedDestory.Count; i++) {
                var item = ballsNeedDestory[i];
                _ballAgents.Remove(item);
                Destroy(item.gameObject);
            }

            // 检测需要清理的card
            List<CardAgent> cardsNeedDestory = new List<CardAgent>();
            for (int i = 0; i < _cardAgents.Count; i++)
            {
                if (_cardAgents[i].Status == CardAgent.CardStatusEnum.Destoryed)
                {
                    cardsNeedDestory.Add(_cardAgents[i]);
                }
            }

            for (int i = 0; i < cardsNeedDestory.Count; i++)
            {
                var item = cardsNeedDestory[i];
                item.refBallAgent.ballStatus = BallStatusEnum.destorying;
                _cardAgents.Remove(item);

                // 销毁card
                Destroy(item.gameObject);

                // 销毁ball
                item.refBallAgent.ballStatus = BallStatusEnum.destorying;

                // 清理ref
                item.refPointAgent.Clear();
            }

        }



        bool CanGenerateBalls() {
            float birthTimeItv = Mathf.Lerp(_birthIntervalTimeRangeMin, _birthIntervalTimeRangeMax, Random.Range(0f, 1f)) * BIRTH_INTERVAL_TIME_CONST + BIRTH_INTERVAL_TIME_CONST;

            if ((Time.time - birthTimeItv) > _lastGenerateTime) {
                _lastGenerateTime = Time.time;
                return true;
            }

            return false;
        }

        bool CanGenerateSymbols()
        {
            float birthTimeItv = Mathf.Lerp(_birthIntervalTimeRangeMin, _birthIntervalTimeRangeMax, Random.Range(0f, 1f)) * BIRTH_INTERVAL_TIME_CONST + BIRTH_INTERVAL_TIME_CONST;

            if ((Time.time - birthTimeItv) > _lastSymbolTime)
            {
                _lastSymbolTime = Time.time;
                return true;
            }

            return false;
        }

        /// <summary>
        ///     将生成位置上下交错
        /// </summary>
        /// <param name="offsetY"></param>
        /// <returns></returns>
        float getYOffset(float offsetY) {
            //if (_lastGeneratePosition == GeneratePosition.UP)
            //{
            //    // 上一个内容是在上方，则下个内容在下方
            //    if (offsetY > 0)
            //    {
            //        offsetY = -offsetY;

            //    }
            //    _lastGeneratePosition = GeneratePosition.DOWN;
            //}
            //else {
            //    if (offsetY < 0)
            //    {
            //        offsetY = -offsetY;

            //    }
            //    _lastGeneratePosition = GeneratePosition.UP;

            //}
            return offsetY;
        }

        float getScaleFactor() {
            return Mathf.Lerp(_scaleRangeMin, _scaleRangeMax, Random.Range(0f, 1f));
        }



        /// <summary>
        /// 获取可用的 参考标记点
        /// 获取逻辑： https://www.yuque.com/u314548/lbo6va/phnmy7
        /// </summary>
        /// <param name="position">世界坐标</param>
        /// <returns></returns>
        public RefPointAgent GetAvailableRefPoint(Vector3 position) {
            RefPointAgent nearAvailableAgent = _refPointAgents[0];
            float shortestDistance = 100000;


            // 计算最近的参照点
            for (int i = 0; i < _refPointAgents.Length; i++) {
                var agentPosition = _refPointAgents[i].gameObject.transform.position;
                var distance = Vector3.Distance(agentPosition, position);
                //Debug.Log("R - " + _refPointAgents[i].gameObject.name + "  distances : " + distance);

                if (distance < shortestDistance) {
                    shortestDistance = distance;
                    nearAvailableAgent = _refPointAgents[i];
                }
            }

            //Debug.Log("Near Ref is : " + nearAvailableAgent.gameObject.name);

            return nearAvailableAgent;



            // 空闲时则直接返回
            if (nearAvailableAgent.refPointStatus == RefPointAgent.RefPointStatusEnum.vacant)
            {
                return nearAvailableAgent;
            }
            else {
                // 检查该参照点的最近点是否已经使用
                RefPointAgent nearAgent = null;

                var nearlyAgents = nearAvailableAgent.nearlyRefPointAgents;
                for (int i = 0; i < nearlyAgents.Length; i++) {
                    if (nearlyAgents[i].refPointStatus == RefPointAgent.RefPointStatusEnum.vacant) {
                        nearAgent = nearlyAgents[i];
                        break;
                    }
                }

                if (nearAgent != null)
                {
                    // 找到了调整的参照点
                    return nearAgent;
                }
                else {
                    
                    nearAvailableAgent.ballAgent.refCardAgent.DirectClose();
                    
                    return nearAvailableAgent;
                }
            }
        }



        /// <summary>
        /// 获取可用的 参考标记点
        /// 获取逻辑： https://www.yuque.com/u314548/lbo6va/phnmy7
        /// </summary>
        /// <param name="position">世界坐标</param>
        /// <returns></returns>
        public RefPointAgent GetAvailableRefPoint2(Vector3 position)
        {
            RefPointAgent nearAvailableAgent = _refPointAgents[0];
            float shortestDistance = 100000;


            // 计算最近的参照点
            for (int i = 0; i < _refPointAgents.Length; i++)
            {
                var agentPosition = _refPointAgents[i].gameObject.transform.position;
                var distance = Vector3.Distance(agentPosition, position);
                //Debug.Log("R - " + _refPointAgents[i].gameObject.name + "  distances : " + distance);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearAvailableAgent = _refPointAgents[i];
                }
            }


            return nearAvailableAgent;

        }

        /// <summary>
        ///     打开卡片
        /// </summary>
        /// <param name="refPointAgent"></param>
        /// <param name="ballAgent"></param>
        public void OpenCard(RefPointAgent refPointAgent, BallAgent ballAgent) {

            //if (refPointAgent.refPointStatus == RefPointAgent.RefPointStatusEnum.busy)
            //{
            //    var oriBallAgent = refPointAgent.ballAgent;

            //    //  关闭原有卡片
            //    if (oriBallAgent.refCardAgent == null)
            //    {
            //        // 关闭数据球
            //        oriBallAgent.DestoryIt();

            //        refPointAgent.Clear();
            //    }
            //    else {

            //        // 关闭数据球、关闭卡片
            //        oriBallAgent.refCardAgent.DirectClose();
            //        oriBallAgent.DestoryIt();

            //        refPointAgent.Clear();
            //    }               

            //}

            // 打开卡片

            // agent 移动到 refpoint 的位置
            refPointAgent.ballAgent = ballAgent;
            refPointAgent.refPointStatus = RefPointAgent.RefPointStatusEnum.busy;


            var to = refPointAgent.GetComponent<Transform>().position;

            var tweenerMove = ballAgent.GetComponent<Transform>().DOMove(to, 0.5f).OnComplete(() => {

                // 缩小到一定比例
                Vector3 toScale = new Vector3(0.2f, 0.2f, 1f);
                var tweenerSmall = ballAgent.GetComponent<Transform>().DOScale(toScale, 1f).OnComplete(() => {

                    // 创建卡片走向幕前
                    ballAgent.ballStatus = BallStatusEnum.opened;
                    ballAgent.gameObject.SetActive(false);

                    CardAgent cardAgent;


                    if (refPointAgent.needReversal)
                    {
                        cardAgent = Instantiate(_cardAgentRePrefab, _cardContainer);

                    }
                    else {
                        cardAgent = Instantiate(_cardAgentPrefab, _cardContainer);
                    }

                    //cardAgent
                    var genWorldPosition = ballAgent.GetComponent<Transform>().position;
                    cardAgent.Init(genWorldPosition, ballAgent, refPointAgent);



                    //if (cardAgent.refPointAgent.needReversal)
                    //{
                    //    cardAgent.GetComponent<RectTransform>().Rotate(new Vector3(180, 180, 0), Space.Self);
                        
                    //    //.DORotate(new Vector3(180, 180, 0), Time.deltaTime);
                    //}


                    // 放大
                    Vector3 cardToScale = new Vector3(1.6f, 1.6f, 1.6f);
                    cardAgent.OpenTweener = cardAgent.GetComponent<Transform>().DOScale(cardToScale, 1f);

                    ballAgent.refCardAgent = cardAgent;
                    _cardAgents.Add(cardAgent);
                });

                ballAgent.SmallTweener = tweenerSmall;
            });

            ballAgent.MoveTweener = tweenerMove;




            refPointAgent.Mark();
        }




        public void ClearRefsMark() {
            for (int i = 0; i < _refPointAgents.Length; i++)
            {
                if (_refPointAgents[i].refPointStatus == RefPointAgent.RefPointStatusEnum.vacant) {
                    _refPointAgents[i].ClearMark();
                }
            }

        }


    }

}

