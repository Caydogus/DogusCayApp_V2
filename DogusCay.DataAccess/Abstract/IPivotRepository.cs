using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.DataAccess.Abstract
{
    public interface IPivotRepository
    {
        Task<List<Dictionary<string, object>>> GetTableDynamicAsync(
           string tableName,
           string filterColumn = null,
           string filterValue = null
       );
    }
}
