namespace RuneMagic.Source.Interfaces
{
    public interface ISpellEffect
    {
        public abstract string Name { get; set; }
        public abstract string Description { get; set; }
        public abstract int Duration { get; set; }

        public abstract void Update();
        public abstract void Start();
    }
}
