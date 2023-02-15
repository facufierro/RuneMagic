namespace RuneMagic.Source
{
    public class SpellEffect
    {
        public string Name { get; set; }
        public int Timer { get; set; }

        public SpellEffect(string name, int timer)
        {
            Name = $"Glyph of {name}";
            Timer = timer;
        }
    }
}