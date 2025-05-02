namespace Data.Entities
{
    public class Alarm : IEntity
    {
        public long Id { get; set; }
        public int DaysOfWeek { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public bool Enabled { get; set; }
        public string Title { get; set; }
    }
}
