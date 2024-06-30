using ContentService.Repositories;
using ContentService.Repositories.Interfaces;
using ContentService.Services;
using ContentService.Services.Interfaces;

namespace ContentService.Base
{
    public static class InterfaceBindings
    {
        public static IReadOnlyDictionary<Type, Type> GetServiceBindings()
        {
            var result = new Dictionary<Type, Type>
            {
                { typeof(IRecordService), typeof(RecordService) },
            };
            return result;
        }

        public static IReadOnlyDictionary<Type, Type> GetRepositoryBindings()
        {
            var result = new Dictionary<Type, Type>
            {
                { typeof(IRecordRepository), typeof(RecordRepository) }
            };
            return result;
        }
    }
}
