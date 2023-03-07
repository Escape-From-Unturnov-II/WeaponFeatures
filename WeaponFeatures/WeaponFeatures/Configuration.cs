using System.Xml.Serialization;
using Rocket.API;

namespace WeaponFeatures
{
    public class Configuration : IRocketPluginConfiguration
    {
        public void LoadDefaults()
        {
        }
    }

    public class FiringRange
    {
        [XmlAttribute("WeaponID")]
        public ushort WeaponID { get; set; }
        [XmlAttribute("LengthOfWeapon")]
        public float LengthOfWeapon { get; set; }

        public FiringRange(ushort weapon_id, float length_of_weapon)
        {
            WeaponID = weapon_id;
            LengthOfWeapon = length_of_weapon;
        }

        public FiringRange()
        {
        }
    }
}
