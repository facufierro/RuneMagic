namespace RuneMagic.Source.Spells
{
    public class Breaking : Spell
    {
        public Breaking() : base()
        {
            School = School.Alteration;
            Description = "Breaks debris on the target location.";
            Level = 3;
        }

        public override bool Cast()
        {
            //set Target to the debris under muse cursor
            //Target = Game1.currentLocation.getObjectAtTile((int)Game1.currentCursorTile.X, (int)Game1.currentCursorTile.Y);
            //if (Target is not null and Debris)
            //{
            //    return true;
            //}
            return false;
        }
    }
}