namespace ContentService.Models.Requests
{
    public class UpdateRecordRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public bool IsFavorite { get; set; }

        public (bool valid, string errorMessage) IsRequestValid()
        {
            if (Id <= 0)
                return (false, "id can not be 0 or less");
            if (string.IsNullOrEmpty(Title))
                return (false, "title is null or empty");
            if (string.IsNullOrEmpty(Text))
                return (false, "text is null or empty");

            return (true, "");
        }
    }
}
