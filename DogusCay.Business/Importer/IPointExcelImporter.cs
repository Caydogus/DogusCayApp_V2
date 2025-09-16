using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.Business.Importer
{
    public interface IPointExcelImporter
    {
        public List<string> ImportPointsFromExcel(string excelFilePath);

    }
}
