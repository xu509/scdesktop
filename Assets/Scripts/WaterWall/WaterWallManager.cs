using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace scdesktop
{
    /// <summary>
    ///     流水墙
    /// </summary>
    public class WaterWallManager : MonoBehaviour
    {
        [SerializeField] LinesManager _LinesManager;
        [SerializeField] BallsManager _ballsManager;

        MainManager _manager;


        public void Init(MainManager manager) {
            _manager = manager;
            _ballsManager.Init(_manager);
        }



        // Start is called before the first frame update
        void Start()
        {
            // 开始运行线条动画




        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
