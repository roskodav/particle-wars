using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Player
{
    [RequireComponent(typeof(PlayerControl))]
    public class LocalPlayerControl : MonoBehaviour
    {
        private PlayerControl _pc;

        private void Start()
        {
            _pc = GetComponent<PlayerControl>();
        }

        private void Update()
        {
            if (Input.GetMouseButton((int) MouseButton.LeftMouse))
            {
                var mousePos = Input.mousePosition;
                mousePos.z = Camera.main.nearClipPlane;
                var touchPosition = (Vector2) Camera.main.ScreenToWorldPoint(mousePos);
                _pc.ForceAttractPosition(touchPosition);
            }
        }
    }
}