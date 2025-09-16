using ClosedXML.Excel;
using DogusCay.Business.Importer;
using DogusCay.DataAccess.Context;
using DogusCay.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class PointExcelImporter : IPointExcelImporter
{
    private readonly DogusCayContext _context;
    private readonly IConfiguration _configuration;

    public PointExcelImporter(DogusCayContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public List<string> ImportPointsFromExcel(string excelFilePath)
    {
        var log = new List<string>();

        // appsettings.json’dan log klasörünü oku
        string logFolder = _configuration["PointImport:LogFolder"];

        if (string.IsNullOrWhiteSpace(logFolder))
        {
            // fallback: uygulamanın çalıştığı klasöre yaz
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
                var ws = workbook.Worksheet("pointmaster");
                if (ws == null)
                {
                    log.Add($"Hata: 'pointmaster' adında çalışma sayfası bulunamadı. Lütfen Excel dosyasındaki çalışma sayfası adını kontrol edin.");
                    File.AppendAllLines(Path.Combine(logFolder, "ImportGeneralLog.txt"), log);
                    return log;
                }

                var headerRow = ws.Row(1);
                string Normalize(string s) => s?.Trim().Replace(" ", "").ToLowerInvariant();

                var requiredHeaders = new[] { "PointName", "PointGroupTypeId", "KanalId", "AppUserId", "DistributorId", "PointErc" }
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
                    string pointErc = "Bilinmiyor";

                    try
                    {
                        string pointName = row.Cell(columnIndexes[Normalize("PointName")]).GetString()?.Trim();
                        string pointGroupTypeIdStr = row.Cell(columnIndexes[Normalize("PointGroupTypeId")]).GetString()?.Trim();
                        string kanalIdStr = row.Cell(columnIndexes[Normalize("KanalId")]).GetString()?.Trim();
                        string appUserIdStr = row.Cell(columnIndexes[Normalize("AppUserId")]).GetString()?.Trim();
                        string distributorIdStr = row.Cell(columnIndexes[Normalize("DistributorId")]).GetString()?.Trim();
                        pointErc = row.Cell(columnIndexes[Normalize("PointErc")]).GetString()?.Trim();

                        if (string.IsNullOrWhiteSpace(pointErc))
                        {
                            log.Add($"Satır {rowNumber}: 'PointErc' boş. Bu satır atlandı.");
                            continue;
                        }
                        if (string.IsNullOrWhiteSpace(pointName))
                        {
                            log.Add($"Satır {rowNumber}: 'PointName' boş. Bu satır atlandı. (PointErc: {pointErc})");
                            continue;
                        }

                        int? pointGroupTypeId = int.TryParse(pointGroupTypeIdStr, out var tempPGT) ? tempPGT : (int?)null;
                        int? distributorId = int.TryParse(distributorIdStr, out var tempDist) ? tempDist : (int?)null;
                        int kanalId = int.TryParse(kanalIdStr, out var tempKanal) ? tempKanal : 0;
                        int appUserId = int.TryParse(appUserIdStr, out var tempAppUser) ? tempAppUser : 0;

                        var existing = _context.Points.AsNoTracking().FirstOrDefault(x => x.PointErc == pointErc);

                        if (existing != null)
                        {
                            var pointToUpdate = _context.Points.Find(existing.PointId);
                            if (pointToUpdate != null)
                            {
                                pointToUpdate.PointName = pointName;
                                pointToUpdate.PointGroupTypeId = pointGroupTypeId;
                                pointToUpdate.KanalId = kanalId;
                                pointToUpdate.AppUserId = appUserId;
                                pointToUpdate.DistributorId = distributorId;
                                _context.Points.Update(pointToUpdate);
                                log.Add($"Satır {rowNumber}: '{pointErc}' (ID: {existing.PointId}) güncellendi.");
                            }
                            else
                            {
                                log.Add($"Satır {rowNumber}: Veritabanında '{pointErc}' bulundu ancak güncellenecek nesne alınamadı.");
                            }
                        }
                        else
                        {
                            var newPoint = new Point
                            {
                                PointName = pointName,
                                PointGroupTypeId = pointGroupTypeId,
                                KanalId = kanalId,
                                AppUserId = appUserId,
                                DistributorId = distributorId,
                                PointErc = pointErc
                            };
                            _context.Points.Add(newPoint);
                            log.Add($"Satır {rowNumber}: '{pointErc}' yeni kayıt olarak eklendi.");
                        }

                        _context.SaveChanges();
                    }
                    catch (DbUpdateException dbEx)
                    {
                        log.Add($"Hata: Satır {rowNumber} için veritabanı hatası (PointErc: {pointErc}): {dbEx.InnerException?.Message ?? dbEx.Message}");
                    }
                    catch (Exception ex)
                    {
                        log.Add($"Hata: Satır {rowNumber} işlenirken genel hata (PointErc: {pointErc}): {ex.Message}");
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
