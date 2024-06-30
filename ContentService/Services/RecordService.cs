using ContentService.Models.DataBase;
using ContentService.Models.Requests;
using ContentService.Repositories.Interfaces;
using ContentService.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace ContentService.Services
{
    public class RecordService : IRecordService
    {
        private readonly IRecordRepository _recordRepository;
        private readonly IMemoryCache _memoryCache;

        public RecordService(IRecordRepository recordRepository, IMemoryCache memoryCache)
        {
            _recordRepository = recordRepository;
            _memoryCache = memoryCache;
        }

        private static string GetCacheKeyForRecord(int recordId) => $"key for record rid:"  +recordId;
        private static string GetCacheKeyForUserRecords(int userId) => $"key for user records uid:" + userId;

        public async Task<Record> CreateRecordAsync(CreateRecordRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var record = new Record
            {
                Text = request.Text,
                Title = request.Title,
                UserId = request.UserId,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                IsFavorite = false,
            };

            var dbRecord = await _recordRepository.CreateRecordAsync(record);
            _memoryCache.Remove(GetCacheKeyForUserRecords(dbRecord.UserId));
            return dbRecord;
        }

        public async Task DeleteRecordAsync(int recordId)
        {
            await _recordRepository.DeleteRecordAsync(recordId);
            _memoryCache.Remove(GetCacheKeyForRecord(recordId));
        }

        public async Task<Record> GetRecordByIdAsync(int recordId)
        {
            var key = GetCacheKeyForRecord(recordId);

            return await _memoryCache.GetOrCreateAsync(key: key,
                factory:  (fac) =>
                {
                    fac.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    return _recordRepository.GetRecordByIdAsync(recordId);
                });
        }

        public async Task<IReadOnlyList<Record>> GetRecordsByUserIdAsync(int userId)
        {
            var key = GetCacheKeyForUserRecords(userId);

            return await _memoryCache.GetOrCreateAsync(key: key,
                factory: (fac) =>
                {
                    fac.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    return _recordRepository.GetRecordsByUserIdAsync(userId);
                });
        }

        public async Task<Record> UpdateRecordAsync(UpdateRecordRequest request)
        {
            var dbRecord = await _recordRepository.GetRecordByIdAsync(request.Id);
            if (dbRecord == null) throw new Exception("record is null");

            dbRecord.Title = request.Title;
            dbRecord.Text = request.Text;
            dbRecord.IsFavorite = request.IsFavorite;
            dbRecord.DateUpdated = DateTime.UtcNow;

            var updatedRecord = await _recordRepository.UpdateRecordAsync(dbRecord);

            _memoryCache.Remove(GetCacheKeyForRecord(updatedRecord.Id));
            return updatedRecord;
        }
    }
}
