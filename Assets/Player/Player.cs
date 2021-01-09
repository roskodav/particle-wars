using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Player
{
    public class Player : MonoBehaviour
    {
        public string Name;
        public Color Color;
        public int ID;

        public override bool Equals(System.Object obj)
        {
            //Check for null and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            else
            {
                return (ID == ((Player)obj).ID);
            }
        }
    }
}
