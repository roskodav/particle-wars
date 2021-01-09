using UnityEngine;

namespace Assets.Particle
{
    [RequireComponent(typeof(global::Particle))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class ParticleColor : MonoBehaviour
    {
        private global::Particle _particle;
        private SpriteRenderer _rend;

        private void Start()
        {
            _particle = GetComponent<global::Particle>();
            _rend = GetComponent<SpriteRenderer>();
        }

        private void UpdateColor()
        {
            if (_particle.Owner != null && _rend.color != _particle.Owner.Color)
                _rend.color = _particle.Owner.Color;
        }

        private void Update()
        {
            UpdateColor();
        }
    }
}