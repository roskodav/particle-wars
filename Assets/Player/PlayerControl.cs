using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Player
{
    public class PlayerControl : MonoBehaviour
    {
        public float Radius = 4;
        public Player Player;
        public float PullForce = 1500;

        public void ForceAttractPosition(Vector2 attractedPosition)
        {
            foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, Radius))
            {
                var particle = collider.GetComponent<global::Particle>();
                if (particle != null && particle.Owner.Equals(Player))
                {
                    particle.SetControlled();
                    float actualDistance = Vector2.Distance(attractedPosition, collider.transform.position);
                    float minForceDistance = Radius;
                    float force = (1 - Mathf.Clamp01(actualDistance / minForceDistance)) * PullForce;
                    Vector3 forceDirection = transform.position - collider.transform.position;
                    collider.attachedRigidbody
                        .AddForce(forceDirection.normalized * 
                                  force * 
                                  Time.fixedDeltaTime);
                }
            }
        }

        private void OnDrawGizmos()
        {
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, Radius);
        }

        void Update()
        {
            ForceAttractPosition(transform.position);
        }
    }
}
