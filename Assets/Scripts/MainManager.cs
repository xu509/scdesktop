using UnityEngine;


namespace scdesktop
{

    /// <summary>
    ///     主控制器
    /// </summary>
    public class MainManager : MonoBehaviour
    {
        [SerializeField] WaterWallManager _waterWallManager;
        [SerializeField] OverLookWallManager _overLookWallManager;

        [SerializeField] ShicunDaoService shicunDaoService;



        private IDaoService _daoService;
        public IDaoService daoService { get { return _daoService; } }


        private SceneEnum _currentScene = SceneEnum.WaterWall;

        // Start is called before the first frame update
        void Start()
        {
            TurnOnWaterWallScene();

            _waterWallManager.Init(this);


            _daoService = shicunDaoService;
            daoService.prepareData();

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space)){
                if (_currentScene == SceneEnum.OverlookWall)
                {
                    TurnOnWaterWallScene();
                }
                else {
                    TurnOnOverlookScene();
                }
            }
        }       


        /// <summary>
        /// 开启流水墙
        /// </summary>
        private void TurnOnWaterWallScene() {
            if (!_waterWallManager.gameObject.activeSelf) {
                _waterWallManager.gameObject.SetActive(true);
            }
            _overLookWallManager.gameObject.SetActive(false);
            _currentScene = SceneEnum.WaterWall;
        }

        /// <summary>
        /// 开启俯视墙
        /// </summary>
        private void TurnOnOverlookScene()
        {
            if (!_overLookWallManager.gameObject.activeSelf)
            {
                _overLookWallManager.gameObject.SetActive(true);
            }
            _waterWallManager.gameObject.SetActive(false);
            _currentScene = SceneEnum.OverlookWall;
        }


    }

}

