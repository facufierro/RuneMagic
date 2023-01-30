using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace RuneMagic.Source
{


    public interface IMagicItem
    {
        public Spell Spell { get; set; }

        public void InitializeSpell();
        public void Use();
        public void Activate();
        public bool Fizzle();
        public void Update();
        public void DrawCastbar(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f);
    }
}
