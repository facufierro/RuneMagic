﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace RuneMagic.Source
{
    public interface IMagicItem
    {
        public string Name { get; set; }
        public Spell Spell { get; set; }
        public float Charges { get; set; }
        public void InitializeSpell();
        public void Activate();
        public bool Fizzle();
        public void Update();
        public void DrawCastbar(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f);
    }
}
