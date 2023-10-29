using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SuperFight
{
    public class BaseFxGame : MonoBehaviour
    {
        private ParticleSystem fxGame;

        private void Awake()
        {
            fxGame = gameObject.GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if(gameObject.activeSelf && !fxGame.isPlaying)
            {
                Free();
            }    
        }

        public void Free()
        {
            if (!gameObject.activeSelf) return;


            PoolingObject.FreeObject(gameObject);
        }
    }
}