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
        public Farmer Farmer { get; set; }
        public int CastingFailureChance { get; set; }
        public int SpellAttack { get; set; }
        public int MagicSkillLevel { get; set; }
        public MagicItem MagicItem { get; set; } = null;
        public bool IsCasting { get; set; } = false;
        public float CastingTimer { get; set; } = 0;

        public PlayerStats()
        {
            MagicSkillLevel = 1;
            CastingFailureChance = 10 - MagicSkillLevel;
            SpellAttack = 0;
        }
        public PlayerStats(Farmer farmer) : this()
        {
            Farmer = farmer;
        }

        public void CheckCasting(object sender, UpdateTickedEventArgs e)
        {
            if (MagicItem != null)
            {
                //ModEntry.Instance.Monitor.Log($"{MagicItem.Name}");

                IsCasting = true;
                if (CastingTimer >= Math.Floor(MagicItem.Spell.CastingTime * 60))
                {
                    //ModEntry.Instance.Monitor.Log($"{CastingTimer}");
                    MagicItem.Activate();
                    MagicItem = null;
                    IsCasting = false;
                    CastingTimer = 0;
                }
                else
                    CastingTimer += 1;
            }
        }
    }
}

