using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Player
{
    public class Spawner : MonoBehaviour
    {
        public Player Player;
        public int SpawnCount = 100;


        void Start()
        {
          //  InvokeRepeating(nameof(HealingUpdate), 1f, 1f);
        }
    }
}
