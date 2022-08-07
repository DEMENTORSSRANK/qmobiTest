using System.Threading.Tasks;
using UnityEngine;

namespace Sources.Sounds
{
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _source;

        [SerializeField] private AudioClip _explosion;

        [SerializeField] private AudioClip _shoot;

        [SerializeField] private AudioClip _thrust;

        private const int MinDelayExplosion = 100;

        private bool _canExplosion = true;
        
        public void Pause()
        {
            _source.Pause();
        }

        public void UnPause()
        {
            _source.UnPause();
        }
        
        public void PlayExplosion()
        {
            if (!_canExplosion)
                return;
            
            _source.PlayOneShot(_explosion);
            
            WaitExplosionAsync();
        }

        public void PlayShoot()
        {
            _source.PlayOneShot(_shoot);
        }

        public void StartPlayThrust()
        {
            _source.clip = _thrust;
            
            _source.Play();
        }

        public void EndPlayThrust()
        {
            _source.clip = null;
        }

        private async void WaitExplosionAsync()
        {
            _canExplosion = false;

            await Task.Delay(MinDelayExplosion);
            
            _canExplosion = true;
        }
    }
}