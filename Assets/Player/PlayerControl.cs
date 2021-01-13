using UnityEditor;
using UnityEngine;

namespace Assets.Player
{
    public class PlayerControl : MonoBehaviour
    {
        private Vector3 _lastAttractPosition = Vector3.zero;
        public Player Player;
        public float PullForce = 1500;
        public float Radius = 4;

        public void ForceAttractPosition(Vector2 attractedPosition)
        {
            foreach (var collider in Physics2D.OverlapCircleAll(attractedPosition, Radius))
            {
                var particle = collider.GetComponent<global::Particle>();
                if (particle != null && particle.Owner != null && particle.photonView.IsMine)
                {
                    particle.SetControlled(); //Add influence bonus to every controlled cell
                    var actualDistance =
                        Vector2.Distance(attractedPosition,
                            collider.transform.position); //Check distance between cell and mouse input
                    var force = (1 - Mathf.Clamp01(actualDistance / Radius)) * PullForce;
                    Vector3 forceDirection = attractedPosition - (Vector2) collider.transform.position;

                    //Add force in mouse direction
                    collider.attachedRigidbody
                        .AddForce(forceDirection.normalized *
                                  force *
                                  Time.fixedDeltaTime);
                }
            }

            //Only for Gizmo purposes
            _lastAttractPosition = attractedPosition;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.DrawWireDisc(_lastAttractPosition, Vector3.back, Radius);
        }
#endif
    }
}