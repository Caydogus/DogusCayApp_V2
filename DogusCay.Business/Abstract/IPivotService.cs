using DogusCay.DTO.DTOs.PivotDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.Business.Abstract
{
    public interface IPivotService
    {
        Task<List<Dictionary<string, object>>> TGetPivotDataAsync(
      PivotRequest request,
      string userRole,
      string userId
        );
    }
}
