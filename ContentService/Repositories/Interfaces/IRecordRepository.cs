using ContentService.Models.DataBase;

namespace ContentService.Repositories.Interfaces
{
    public interface IRecordRepository
    {
        Task<Record> CreateRecordAsync(Record record);
        Task DeleteRecordAsync(int recordId);
        Task<Record> GetRecordByIdAsync(int recordId);
        Task<IReadOnlyList<Record>> GetRecordsByUserIdAsync(int userId);
        Task<Record> UpdateRecordAsync(Record record);
    }
}
