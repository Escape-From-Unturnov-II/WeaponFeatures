using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Rocket.Unturned.Player;
using SDG.Unturned;

namespace WeaponFeatures.Patches
{
    [HarmonyPatch(typeof(UseableGun))]
    [HarmonyPatch("startAim")]
    public static class AimPatch
    {
        public static bool Prefix(UseableGun __instance)
        {
            var unturned_player = UnturnedPlayer.FromPlayer(__instance.player);

            return !Main.Instance.Configuration.Instance.BlockedADSClothings.Exists(x =>
                x.ClothingID == unturned_player.Player.clothing.pants ||
                x.ClothingID == unturned_player.Player.clothing.shirt ||
                x.ClothingID == unturned_player.Player.clothing.backpack ||
                x.ClothingID == unturned_player.Player.clothing.vest ||
                x.ClothingID == unturned_player.Player.clothing.mask ||
                x.ClothingID == unturned_player.Player.clothing.glasses ||
                x.ClothingID == unturned_player.Player.clothing.hat
                );
        }
    }
}
