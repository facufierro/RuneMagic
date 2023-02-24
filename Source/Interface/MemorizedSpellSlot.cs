﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.Interface
{
    public class MemorizedSpellSlot : SpellSlot
    {
        public MemorizedSpellSlot(Spell spell, Rectangle rectangle) : base(spell, rectangle)
        {
            if (RuneMagic.PlayerStats.MemorizedSpells.Contains(Spell))
            {
                if (Selected)
                    ButtonTexture = RuneMagic.Textures["spellslot_active"];
                else
                    ButtonTexture = RuneMagic.Textures["spellslot"];
            }
        }

        public override void SetButtonTexture()
        {
            if (RuneMagic.PlayerStats.MemorizedSpells.Contains(Spell))
            {
                if (Selected)
                    ButtonTexture = RuneMagic.Textures["spellslot_active"];
                else
                    ButtonTexture = RuneMagic.Textures["spellslot"];
            }
        }
    }
}