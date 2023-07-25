using System.Collections.Generic;
using System.Xml.Serialization;
using Rocket.API;

namespace WeaponFeatures
{
    public class Configuration : IRocketPluginConfiguration
    {
        public List<BlockedADSClothing> BlockedADSClothings { get; set; }
        public List<WeaponLength> WeaponLengths { get; set; }

        public void LoadDefaults()
        {
            BlockedADSClothings = new List<BlockedADSClothing>()
            {
                new(309)
            };

            WeaponLengths = new List<WeaponLength>
            {
                new(363, 1.35f)
            };
        }
    }

    public class BlockedADSClothing
    {
        public ushort ClothingID { get; set; }

        public BlockedADSClothing(ushort clothing_id)
        {
            ClothingID = clothing_id;
        }

        public BlockedADSClothing()
        {
        }
    }

    public class WeaponLength
    {
        [XmlAttribute("WeaponID")]
        public ushort WeaponID { get; set; }
        [XmlAttribute("LengthOfWeapon")]
        public float LengthOfWeapon { get; set; }

        public WeaponLength(ushort weapon_id, float length_of_weapon)
        {
            WeaponID = weapon_id;
            LengthOfWeapon = length_of_weapon;
        }

        public WeaponLength()
        {
        }
    }
}
