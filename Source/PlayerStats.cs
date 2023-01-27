using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;
using Object = StardewValley.Object;
using System.Xml.Serialization;
using System.Reflection;
using RuneMagic.Framework;
using SpaceCore;
using RuneMagic.Skills;
using System.Threading;
using StardewModdingAPI;
using RuneMagic.Famework;
using RuneMagic.Source;
using StardewModdingAPI.Events;
using System.Runtime.InteropServices;
using RuneMagic.Items;

namespace RuneMagic.Source
{
    public class PlayerStats
    {

        public MagicSkill MagicSkill { get; set; }
        public MagicItem ItemHeld { get; set; } = null;
        public int SpellAttack { get; set; }
        public int CastingFailureChance { get; set; }
        public bool IsCasting { get; set; } = false;
        public float CastingTimer { get; set; } = 0;

        public PlayerStats()
        {
            CastingFailureChance = 10;
            SpellAttack = 0;
        }

        public void CheckCasting(object sender, UpdateTickedEventArgs e)
        {
            if (ItemHeld != null)
            {
                //ModEntry.Instance.Monitor.Log($"{ItemHeld.Name}");

                IsCasting = true;
                if (CastingTimer >= Math.Floor(ItemHeld.Spell.CastingTime * 60))
                {
                    //ModEntry.Instance.Monitor.Log($"{CastingTimer}");
                    ItemHeld.Activate();
                    ItemHeld = null;
                    IsCasting = false;
                    CastingTimer = 0;
                }
                else
                    CastingTimer += 1;
            }
        }
        public void LearnRecipes()
        {

        }
    }
}

