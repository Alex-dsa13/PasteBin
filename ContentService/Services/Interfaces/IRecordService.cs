using ContentService.Models.DataBase;
using ContentService.Models.Requests;

namespace ContentService.Services.Interfaces
{
    public interface IRecordService
    {
        Task<Record> CreateRecordAsync(CreateRecordRequest request);
        Task DeleteRecordAsync(int recordId);
        Task<Record> GetRecordByIdAsync(int recordId);
        Task<IReadOnlyList<Record>> GetRecordsByUserIdAsync(int userId);
        Task<Record> UpdateRecordAsync(UpdateRecordRequest record);

    }
}
