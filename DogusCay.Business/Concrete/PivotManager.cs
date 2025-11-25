using DogusCay.Business.Abstract;
using DogusCay.DataAccess.Abstract;
using DogusCay.DTO.DTOs.PivotDtos;


namespace DogusCay.Business.Concrete
{
    public class PivotManager : IPivotService
    {
        private readonly IPivotRepository _pivotRepository;

        public PivotManager(IPivotRepository pivotRepository)
        {
            _pivotRepository = pivotRepository;
        }

        public async Task<List<Dictionary<string, object>>> TGetPivotDataAsync(
            PivotRequest request,
            string userRole,
            string userId)
        {
            // Admin → tüm veri
            if (userRole == "Admin")
            {
                return await _pivotRepository.GetTableDynamicAsync(
                    request.TableName,
                    null,
                    null
                );
            }

            // Bölge Müdürü → sadece kendi AppUserId datası
            if (userRole == "BolgeMuduru")
            {
                return await _pivotRepository.GetTableDynamicAsync(
                    request.TableName,
                    request.FilterColumn,   // AppUserId
                    userId                  // JWT içerisinden gelecek ID
                );
            }

            // Diğer roller için şimdilik boş liste döner
            return new List<Dictionary<string, object>>();
        }
    }
}
