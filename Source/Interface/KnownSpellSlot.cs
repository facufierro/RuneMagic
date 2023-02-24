using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.Interface
{
    public class KnownSpellSlot : SpellSlot
    {
        public KnownSpellSlot(Spell spell, Rectangle rectangle) : base(spell, rectangle)
        {
            if (RuneMagic.PlayerStats.KnownSpells.Contains(Spell))
            {
                if (RuneMagic.PlayerStats.MemorizedSpells.Contains(Spell))
                    ButtonTexture = RuneMagic.Textures["spellslot_active"];
                else
                    ButtonTexture = RuneMagic.Textures["spellslot"];
            }
        }

        public override void SetButtonTexture()
        {
            if (RuneMagic.PlayerStats.KnownSpells.Contains(Spell))
            {
                if (RuneMagic.PlayerStats.MemorizedSpells.Contains(Spell))
                    ButtonTexture = RuneMagic.Textures["spellslot_active"];
                else
                    ButtonTexture = RuneMagic.Textures["spellslot"];
            }
        }
    }
}