using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Particle
{
    [RequireComponent(typeof(global::Particle))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class ParticleColor : MonoBehaviour
    {
        private global::Particle particle;
        private SpriteRenderer rend;
        void Start()
        {
            particle = GetComponent<global::Particle>();
            rend = GetComponent<SpriteRenderer>();
        }

        void UpdateColor()
        {
            if(particle.Owner != null && rend.color != particle.Owner.Color)
                rend.color = particle.Owner.Color;
        }

        void Update()
        {
            UpdateColor();
        }
    }
}
