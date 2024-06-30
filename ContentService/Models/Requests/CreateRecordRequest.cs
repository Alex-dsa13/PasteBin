namespace ContentService.Models.Requests
{
    public class CreateRecordRequest
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public int UserId { get; private set; }

        public CreateRecordRequest(string title, string text)
        {
            Title = title;
            Text = text;
        }

        public void SetUserId(int userId)
        {
            UserId = userId;
        }

        public (bool valid, string errorMessage) IsRequestValid()
        {
            if (string.IsNullOrEmpty(Title))
                return (false, "title is null or empty");
            if (string.IsNullOrEmpty(Text))
                return (false, "text is null or empty");
            if (UserId <= 0)
                return (false, "userId can not be 0 or less");

            return (true, "");
        }
    }
}
