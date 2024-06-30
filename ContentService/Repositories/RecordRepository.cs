using ContentService.DataBase;
using ContentService.Models.DataBase;
using ContentService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ContentService.Repositories
{
    public class RecordRepository : IRecordRepository
    {
        public async Task<Record> CreateRecordAsync(Record record)
        {
            using (Db db = new Db())
            {
                await db.Records.AddAsync(record);

                try
                {
                    await db.SaveChangesAsync();
                    return record;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task DeleteRecordAsync(int recordId)
        {
            using (Db db = new Db())
            {
                var record = new Record { Id = recordId };

                db.Records.Remove(record);
                await db.SaveChangesAsync();
            }
        }

        public async Task<Record> GetRecordByIdAsync(int recordId)
        {
            using (Db db = new Db())
            {
                return await db.Records.FirstOrDefaultAsync(r => r.Id == recordId);
            }
        }

        public async Task<IReadOnlyList<Record>> GetRecordsByUserIdAsync(int userId)
        {
            using (Db db = new Db())
            {
                return await db.Records.Where(r => r.UserId == userId).ToListAsync();
            }
        }

        public async Task<Record> UpdateRecordAsync(Record record)
        {
            using (Db db = new Db())
            {
                db.Update(record);
                await db.SaveChangesAsync();
            }
            return record;
        }
    }
}
