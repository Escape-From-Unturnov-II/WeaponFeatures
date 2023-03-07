using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.API.Extensions;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace WeaponFeatures.Components
{
    internal class ChangeFireMode : MonoBehaviour
    {
        public Player Player;
        private byte _lastFireMode;
        public bool PluginChangeFireMode;

        public delegate void FireModeChanged(UnturnedPlayer player, byte fire_mode);
        public event FireModeChanged OnFireModeChanged;

        void Start()
        {
            Logger.Log("Line 25");
            _lastFireMode = 0;
            PluginChangeFireMode = false;
            Logger.Log("Line 28");
            if (Player == null)
            {
                Player.gameObject.TryRemoveComponent<ChangeFireMode>();
                return;
            }
            Logger.Log("Line 34");
            if (Player.equipment.itemID != 0)
            {
                _lastFireMode = Player.equipment.state[11];
            }
            Logger.Log("Line 39");
        }

        void Update()
        {
            try
            {
                if (Player.equipment.itemID == 0) return;
                
                if (PluginChangeFireMode)
                {
                    PluginChangeFireMode = false;
                    return;
                }
                
                if (Player == null || _lastFireMode.Equals(Player.equipment.state[11])) return;

                _lastFireMode = Player.equipment.state[11];
                OnFireModeChanged?.Invoke(UnturnedPlayer.FromPlayer(Player), _lastFireMode);
            }
            catch (Exception ex)
            {
                Player.gameObject.TryRemoveComponent<ChangeFireMode>();
                Logger.LogException(ex);
            }
        }
    }
}
