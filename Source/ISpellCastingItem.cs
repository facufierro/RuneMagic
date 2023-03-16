using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System.Xml.Serialization;

namespace RuneMagic.Source
{
    public interface ISpellCastingItem
    {
        [XmlIgnore]
        public Spell Spell { get; set; }

        public void InitializeSpell();

        public virtual void Activate()
        {
            Player.MagicStats.ActiveSchool.Experience += 5;
        }

        public void Update();
    }
}