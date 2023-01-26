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
    public class PlayerStats : Farmer
    {
        public Farmer Farmer { get; set; }
        public int CastingFailureChance { get; set; }
        public int SpellAttack { get; set; }
        public int MagicSkillLevel { get; set; }
        public Rune RuneBeingUsed { get; set; }
        public bool IsCasting { get; set; } = false;


        int timeCasted = 0;

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

            if (RuneBeingUsed != null)
            {

                ModEntry.Instance.Monitor.Log($"Time Spent: {timeCasted}");
                ModEntry.Instance.Monitor.Log($"Finish: {RuneBeingUsed.Spell.CastingTime}");


                if (timeCasted == RuneBeingUsed.Spell.CastingTime * 60)
                {
                    RuneBeingUsed.Use();
                    RuneBeingUsed = null;
                    timeCasted = 0;
                }
                timeCasted++;
            }

        }



    }
}

