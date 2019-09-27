using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace scdesktop
{
    
    public class LightAgent : MonoBehaviour
    {

        [SerializeField] Light _light;
        [SerializeField] Color _startColor;
        [SerializeField] Color _endColor;


        [SerializeField] float _durTime;


        private void Start()
        {
            var colorTweener = _light.DOColor(_endColor, _durTime)
                .SetLoops(-1, LoopType.Yoyo);

            

        }


        private void Update()
        {




        }


    }



}
