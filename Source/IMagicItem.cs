using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RuneMagic.Famework;
using RuneMagic.Magic;
using RuneMagic.Source;
using SpaceCore;
using StardewModdingAPI;
using StardewModdingAPI.Enums;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using xTile;
using Object = StardewValley.Object;

namespace RuneMagic.Framework
{


    public interface IMagicItem
    {
        public Spell Spell { get; set; }

        public void InitializeSpell();
        public void Use();
        public void Activate();
        public bool Fizzle();
        public void DrawCastbar(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f);
    }
}
