using ClosedXML.Excel;
using DogusCay.DataAccess.Context;
using DogusCay.Entity.Entities;
using Microsoft.EntityFrameworkCore; // DbUpdateException için eklendi
using Microsoft.Extensions.Configuration;

namespace DogusCay.Business.Importer
{
    public class DistributorExcelImporter : IDistributorExcelImporter
    {
        private readonly DogusCayContext _context;
        private readonly IConfiguration _configuration;

        public DistributorExcelImporter(DogusCayContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public List<string> ImportDistributorsFromExcel(string excelFilePath)
        {
            var log = new List<string>();

            // appsettings.json’dan log klasörünü oku
            string logFolder = _configuration["DistributorImport:LogFolder"];

            if (string.IsNullOrWhiteSpace(logFolder))
            {
                logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DogusCayImportLogs");
            }

            if (!Directory.Exists(logFolder))
                Directory.CreateDirectory(logFolder);

            if (!File.Exists(excelFilePath))
            {
                log.Add($"Hata: Dosya bulunamadı: {excelFilePath}");
                File.AppendAllLines(Path.Combine(logFolder, "ImportGeneralLog.txt"), log);
                return log;
            }

            try
            {
                using (var workbook = new XLWorkbook(excelFilePath))
                {
                    var ws = workbook.Worksheet("distmaster");
                    if (ws == null)
                    {
                        log.Add($"Hata: 'distmaster' adında çalışma sayfası bulunamadı. Lütfen Excel dosyasındaki çalışma sayfası adını kontrol edin.");
                        File.AppendAllLines(Path.Combine(logFolder, "ImportGeneralLog.txt"), log);
                        return log;
                    }

                    var headerRow = ws.Row(1);

                    string Normalize(string s) => s?.Trim().Replace(" ", "").ToLowerInvariant();

                    var requiredHeaders = new[] { "DistributorName", "AppUserId", "KanalId", "DistributorErcKod" }
                        .Select(Normalize).ToList();

                    var columnIndexes = new Dictionary<string, int>();
                    foreach (var cell in headerRow.Cells())
                    {
                        var colName = Normalize(cell.GetString());
                        if (!string.IsNullOrWhiteSpace(colName) && !columnIndexes.ContainsKey(colName))
                            columnIndexes[colName] = cell.Address.ColumnNumber;
                    }

                    var missingHeaders = requiredHeaders.Where(h => !columnIndexes.ContainsKey(h)).ToList();
                    if (missingHeaders.Any())
                    {
                        log.Add($"Hata: Eksik başlık(lar): {string.Join(", ", missingHeaders)}. İşlem iptal edildi.");
                        File.AppendAllLines(Path.Combine(logFolder, "ImportGeneralLog.txt"), log);
                        return log;
                    }

                    foreach (var row in ws.RowsUsed().Skip(1))
                    {
                        int rowNumber = row.RowNumber();
                        string distributorErcKod = "Bilinmiyor";

                        try
                        {
                            string distributorName = row.Cell(columnIndexes[Normalize("DistributorName")]).GetString()?.Trim();
                            string appUserIdStr = row.Cell(columnIndexes[Normalize("AppUserId")]).GetString()?.Trim();
                            string kanalIdStr = row.Cell(columnIndexes[Normalize("KanalId")]).GetString()?.Trim();
                            distributorErcKod = row.Cell(columnIndexes[Normalize("DistributorErcKod")]).GetString()?.Trim();

                            if (string.IsNullOrWhiteSpace(distributorErcKod))
                            {
                                log.Add($"Satır {rowNumber}: 'DistributorErcKod' boş. Bu satır atlandı.");
                                continue;
                            }
                            if (string.IsNullOrWhiteSpace(distributorName))
                            {
                                log.Add($"Satır {rowNumber}: 'DistributorName' boş. Bu satır atlandı. (ErcKod: {distributorErcKod})");
                                continue;
                            }

                            int appUserId = int.TryParse(appUserIdStr, out var tempAppUserId) ? tempAppUserId : 0;
                            int kanalId = int.TryParse(kanalIdStr, out var tempKanalId) ? tempKanalId : 0;

                            var existing = _context.Distributors.AsNoTracking().FirstOrDefault(x => x.DistributorErcKod == distributorErcKod);

                            if (existing != null)
                            {
                                var distributorToUpdate = _context.Distributors.Find(existing.DistributorId);
                                if (distributorToUpdate != null)
                                {
                                    distributorToUpdate.DistributorName = distributorName;
                                    distributorToUpdate.AppUserId = appUserId;
                                    distributorToUpdate.KanalId = kanalId;
                                    _context.Distributors.Update(distributorToUpdate);
                                    log.Add($"Satır {rowNumber}: '{distributorErcKod}' (ID: {existing.DistributorId}) güncellendi.");
                                }
                                else
                                {
                                    log.Add($"Satır {rowNumber}: Veritabanında '{distributorErcKod}' bulundu ancak güncellenecek nesne alınamadı.");
                                }
                            }
                            else
                            {
                                var newDist = new Distributor
                                {
                                    DistributorName = distributorName,
                                    AppUserId = appUserId,
                                    KanalId = kanalId,
                                    DistributorErcKod = distributorErcKod
                                };
                                _context.Distributors.Add(newDist);
                                log.Add($"Satır {rowNumber}: '{distributorErcKod}' yeni kayıt olarak eklendi.");
                            }

                            _context.SaveChanges();
                        }
                        catch (DbUpdateException dbEx)
                        {
                            log.Add($"Hata: Satır {rowNumber} için veritabanı hatası (ErcKod: {distributorErcKod}): {dbEx.InnerException?.Message ?? dbEx.Message}");
                        }
                        catch (Exception ex)
                        {
                            log.Add($"Hata: Satır {rowNumber} işlenirken genel hata (ErcKod: {distributorErcKod}): {ex.Message}");
                        }
                    }
                }
            }
            catch (IOException ioEx)
            {
                log.Add($"Hata: Dosya erişim problemi '{excelFilePath}'. Dosya başka bir işlem tarafından kullanılıyor veya bozuk olabilir: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                log.Add($"Genel bir hata oluştu: {ex.Message}");
            }

            string timeStr = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string logFileName = $"ImportLog_{timeStr}.txt";
            File.WriteAllLines(Path.Combine(logFolder, logFileName), log);
            File.AppendAllLines(Path.Combine(logFolder, "ImportGeneralLog.txt"), log);

            return log;
        }
    }
}
