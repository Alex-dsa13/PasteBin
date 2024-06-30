namespace ContentService.Auth
{
    public class CurrentUser : IIdentity
    {
        public int UserId { get; set; }

        public CurrentUser(int userId)
        {
            UserId = userId;
        }
    }
}
