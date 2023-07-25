using System.Linq;
using HarmonyLib;
using Rocket.API.Extensions;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;
using WeaponFeatures.Components;

namespace WeaponFeatures
{
    public class Main : RocketPlugin<Configuration>
    {
        public Harmony Harmony { get; private set; }
        public static Main Instance { get; set; }

        protected override void Load()
        {
            Instance = this;

            Harmony = new Harmony("unturnov.weaponfeatures");
            Harmony.PatchAll();

            U.Events.OnPlayerConnected += OnPlayerConnected;
            U.Events.OnPlayerDisconnected += OnPlayerLeft;
        }

        private static void OnPlayerLeft(UnturnedPlayer player)
        {
            player.Player.equipment.onEquipRequested -= OnEquipItem;
            player.Player.equipment.onDequipRequested -= OnDequipItem;

            if(player.Player.gameObject.TryGetComponent<ChangeView>(out var comp))
            {
                comp.OnRotationChanged -= OnRotationChange;
                player.Player.gameObject.TryRemoveComponent<ChangeView>();
            }

            if (player.Player.gameObject.TryGetComponent<ChangeFireMode>(out var comp2))
            {
                comp2.OnFireModeChanged -= OnFireModeChanged;
                player.Player.gameObject.TryRemoveComponent<ChangeFireMode>();
            }
        }
        
        private static void OnPlayerConnected(UnturnedPlayer player)
        {
            player.Player.equipment.onEquipRequested += OnEquipItem;
            player.Player.equipment.onDequipRequested += OnDequipItem;

            var player_rotation_comp = player.Player.gameObject.AddComponent<ChangeView>();
            player_rotation_comp.Player = player;
            player_rotation_comp.LastRotation = player.Rotation;

            player_rotation_comp.OnRotationChanged += OnRotationChange;

            var fire_mode_comp = player.Player.gameObject.AddComponent<ChangeFireMode>();
            fire_mode_comp.Player = player.Player;

            fire_mode_comp.OnFireModeChanged += OnFireModeChanged;
        }

        private static void OnDequipItem(PlayerEquipment equipment, ref bool should_allow)
        {
            if (!equipment.player.gameObject.TryGetComponent<FiremodeBlocker>(out var component)) return;

            var comp2 = equipment.player.gameObject.GetComponent<ChangeFireMode>();

            comp2.PluginChangeFireMode = true;
            equipment.player.equipment.state[11] = component.LastFireMode;
            equipment.player.equipment.sendUpdateState();

            equipment.player.equipment.gameObject.TryRemoveComponent<FiremodeBlocker>();
        }

        private static void OnFireModeChanged(UnturnedPlayer player, byte fire_mode) => CheckFireMode(player.Player);

        private static void OnRotationChange(UnturnedPlayer player, float rotation) => CheckFireMode(player.Player);

        private static void OnEquipItem(PlayerEquipment equipment, ItemJar jar, ItemAsset asset, ref bool should_allow) => CheckFireMode(equipment.player);

        private static void CheckFireMode(Player player)
        {
            if (player.equipment.asset is not ItemGunAsset) return;
            if(Instance.Configuration.Instance.WeaponLengths.All(x => x.WeaponID != player.equipment.asset.id)) return;

            var comp2 = player.gameObject.GetComponent<ChangeFireMode>();
            var weapon = Instance.Configuration.Instance.WeaponLengths.First(x => x.WeaponID == player.equipment.asset.id);

            var ray = new Ray(player.look.aim.position, player.look.aim.forward);
            
            if (!Physics.Raycast(ray, out _, weapon.LengthOfWeapon, RayMasks.BLOCK_COLLISION))
            {
                var exists = player.equipment.gameObject.GetComponent<FiremodeBlocker>();
                if (exists == null) return;
                
                comp2.PluginChangeFireMode = true;
                player.equipment.state[11] = exists.LastFireMode;
                player.equipment.sendUpdateState();
                player.equipment.gameObject.TryRemoveComponent<FiremodeBlocker>();
                return;
            }

            if (player.equipment.itemID == 0) return;
            {
                var current_fire_mode = player.equipment.state[11];
                var exists = player.equipment.gameObject.GetComponent<FiremodeBlocker>();
                if (exists == null)
                {
                    var comp = player.equipment.gameObject.AddComponent<FiremodeBlocker>();
                    comp.LastFireMode = current_fire_mode;
                }
            }
            comp2.PluginChangeFireMode = true;
            player.equipment.state[11] = 0;
            player.equipment.sendUpdateState();
        }
        
        protected override void Unload()
        {
            Instance = null;

            Harmony.UnpatchAll("unturnov.weaponfeatures");

            U.Events.OnPlayerConnected -= OnPlayerConnected;
            U.Events.OnPlayerDisconnected -= OnPlayerLeft;
        }
    }
}
