using static System.Net.Mime.MediaTypeNames;

namespace AuthService.Models
{
    public class UpdateUserRequest
    {
        public int Id { get; set; }
        public string Login {  get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public (bool valid, string errorMessage) IsRequestValid()
        {
            if (Id <= 0)
                return (false, "id can not be 0 or less");
            if (string.IsNullOrEmpty(Login))
                return (false, "login is null or empty");
            if (string.IsNullOrEmpty(Email))
                return (false, "email is null or empty");
            if (string.IsNullOrEmpty(Password))
                return (false, "password is null or empty");

            return (true, "");
        }
    }
}
