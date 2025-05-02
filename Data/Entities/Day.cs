namespace Data.Entities
{
    public class Day : IEntity
    {
        public long Id { get; set; }
        public int DayNum { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string Title { get; set; }
        public string Notes { get; set; }
        public string Color { get; set; }
    }
}
