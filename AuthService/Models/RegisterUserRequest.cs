namespace AuthService.Models
{
    public class RegisterUserRequest
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public (bool valid, string errorMessage) IsRequestValid()
        {
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
