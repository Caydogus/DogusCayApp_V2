using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.Business.Importer
{
    public interface IDistributorExcelImporter
    {
        // Excel dosyasından Distributor verilerini içeri aktarır. Hataları log olarak döndürür.
        List<string> ImportDistributorsFromExcel(string excelFilePath);
    }
}
