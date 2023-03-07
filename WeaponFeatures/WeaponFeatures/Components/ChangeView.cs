using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WeaponFeatures.Components
{
    public class ChangeView : MonoBehaviour
    {
        public float LastRotation;
        public UnturnedPlayer Player;
        
        public delegate void RotationChanged(UnturnedPlayer player, float rotation);
        public event RotationChanged OnRotationChanged;

        void Update()
        {
            if (Player == null || LastRotation.Equals(Player.Rotation)) return;
            LastRotation = Player.Rotation;

            OnRotationChanged?.Invoke(Player, LastRotation);
        }
    }
}
