namespace ContentService.Models.Dto
{
    public class RecordDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsFavorite { get; set; }
        public int UserId { get; set; }
    }
}
