using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

// [RequireComponent(typeof(Rigidbody2D))]
//public class Lagcompensation : Photon.MonoBehaviour
//{
//    private Vector2 networkPosition;
//    private Rigidbody2D rigidbody;
//    void Start()
//    {
//        rigidbody = GetComponent<Rigidbody2D>();
//    }

//    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
//    {
//        if (stream.IsWriting)
//        {
//            stream.SendNext(this.transform.position);
//            stream.SendNext(this.transform.rotation);
//            stream.SendNext(rigidbody.velocity);
//        }
//        else
//        {
//            networkPosition = (Vector3)stream.ReceiveNext();
//            rigidbody.velocity = (Vector3)stream.ReceiveNext();

//            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));
//            networkPosition += (rigidbody.velocity * lag);
//        }
//    }

//    public void FixedUpdate()
//    {
//        if (!photonView.IsMine)
//        {
//            rigidbody.position = Vector3.MoveTowards(rigidbody.position, networkPosition, Time.fixedDeltaTime);
//        }
//    }
//}
