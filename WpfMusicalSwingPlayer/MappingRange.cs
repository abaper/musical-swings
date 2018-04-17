namespace WpfMusicalSwingPlayer
{
    public class MappingRange
    {
        public float Low { get; set; }
        public float High { get; set; }

        public bool IsInRange(float theVal)
        {
            return theVal >= Low && theVal <= High;
        }
    }
}