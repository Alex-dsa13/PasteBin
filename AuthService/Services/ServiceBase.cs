using AuthService.DataBase;
using AuthService.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace AuthService.Services
{
    public class ServiceBase
    {
        protected string GetHashedPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] hashBytes = sha.ComputeHash(Encoding.ASCII.GetBytes(password));
                return Convert.ToHexString(hashBytes);
            }
        }
    }
}
