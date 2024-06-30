using AuthService.DataBase;
using AuthService.Models;
using AuthService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Repositories
{
    public class UserRepository : IUserRepository
    {
        public async Task<User> RegisterUserAsync(User user)
        {
            using (var db = new Db())
            {
                await db.Users.AddAsync(user);
                try
                {
                    await db.SaveChangesAsync();
                    return user;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task DeleteUserAsync(int userId)
        {
            using (var db = new Db())
            {
                var user = new User { Id = userId };

                db.Users.Remove(user);
                await db.SaveChangesAsync();
            }
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            using (var db = new Db())
            {
                return await db.Users.FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            using (var db = new Db())
            {
                db.Update(user);
                await db.SaveChangesAsync();
                return user;
            }
        }

        public async Task<bool> IsLoginExistAsync(string login)
        {
            using (var db = new Db())
            {
                return await db.Users.AnyAsync(x => x.Login.ToLower().Equals(login));
            }
        }

        public async Task<User> GetByLogin(string login)
        {
            using (var db = new Db())
            {
                return await db.Users.FirstOrDefaultAsync(x => x.Login.ToLower().Equals(login));
            }
        }

        public async Task<User> UpdateUserRefreshTokenAsync(User user)
        {
            using (Db db = new Db())
            {
                db.Update(user);
                await db.SaveChangesAsync();
            }

            return user;
        }
    }
}
