using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace scdesktop {
    public class MusicManager : MonoBehaviour
    {
        [SerializeField] AudioSource _audioSource;

        public void Init() {

        }


        public void Play() {

            _audioSource.Play();
        }




    }

}
