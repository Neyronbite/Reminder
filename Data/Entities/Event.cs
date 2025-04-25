namespace Data.Entities
{
    public class Event : IEntity
    {
        public long Id { get; set; }
        public int DayId { get; set; }
        public int DayNum { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public bool Enabled { get; set; }
        public bool Triggered { get; set; }
        public string Title { get; set; }
    }
}
