
//using ClosedXML.Excel;
//using DogusCay.Business.Importer;
//using DogusCay.DataAccess.Context;
//using DogusCay.Entity.Entities;
//using Microsoft.EntityFrameworkCore; // DbUpdateException için eklendi


//public class PointExcelImporter : IPointExcelImporter
//{
//    private readonly DogusCayContext _context;

//    public PointExcelImporter(DogusCayContext context)
//    {
//        _context = context;
//    }

//    public List<string> ImportPointsFromExcel(string excelFilePath)
//    {
//        var log = new List<string>();
//        string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
//        string logFolder = Path.Combine(desktop, "DogusCayImportLogs");

//        // Log klasörünün var olduğundan emin olun
//        if (!Directory.Exists(logFolder))
//            Directory.CreateDirectory(logFolder);

//        // Excel dosyasının varlığını kontrol edin
//        if (!File.Exists(excelFilePath))
//        {
//            log.Add($"Hata: Dosya bulunamadı: {excelFilePath}");
//            File.AppendAllLines(Path.Combine(logFolder, "ImportGeneralLog.txt"), log);
//            return log;
//        }

//        try
//        {
//            using (var workbook = new XLWorkbook(excelFilePath))
//            {
//                // "pointmaster" adlı çalışma sayfasını alın
//                var ws = workbook.Worksheet("pointmaster");
//                if (ws == null)
//                {
//                    log.Add($"Hata: 'pointmaster' adında çalışma sayfası bulunamadı. Lütfen Excel dosyasındaki çalışma sayfası adını kontrol edin.");
//                    File.AppendAllLines(Path.Combine(logFolder, "ImportGeneralLog.txt"), log);
//                    return log;
//                }

//                var headerRow = ws.Row(1);

//                // Başlıkları normalize etmek için yardımcı fonksiyon
//                string Normalize(string s) => s?.Trim().Replace(" ", "").ToLowerInvariant();

//                // SQL başlıklarına göre normalize edilmiş gerekli anahtarlar
//                var requiredHeaders = new[] { "PointName", "PointGroupTypeId", "KanalId", "AppUserId", "DistributorId", "PointErc" }
//                    .Select(Normalize).ToList();

//                // Sütun adlarını indeksleriyle eşleştirin
//                var columnIndexes = new Dictionary<string, int>();
//                foreach (var cell in headerRow.Cells())
//                {
//                    var colName = Normalize(cell.GetString());
//                    if (!string.IsNullOrWhiteSpace(colName) && !columnIndexes.ContainsKey(colName))
//                        columnIndexes[colName] = cell.Address.ColumnNumber;
//                }

//                // Gerekli başlıkların eksik olup olmadığını kontrol edin
//                var missingHeaders = requiredHeaders.Where(h => !columnIndexes.ContainsKey(h)).ToList();
//                if (missingHeaders.Any())
//                {
//                    log.Add($"Hata: Eksik başlık(lar): {string.Join(", ", missingHeaders)}. İşlem iptal edildi.");
//                    File.AppendAllLines(Path.Combine(logFolder, "ImportGeneralLog.txt"), log);
//                    return log;
//                }

//                // Başlık satırını atlayarak her veri satırını işleyin
//                foreach (var row in ws.RowsUsed().Skip(1))
//                {
//                    int rowNumber = row.RowNumber(); // Loglama için mevcut satır numarasını alın
//                    string pointErc = "Bilinmiyor"; // Hata durumunda PointErc'yi loglamak için başlangıç değeri

//                    try
//                    {
//                        // Verileri hücrelerden al ve temizle
//                        string pointName = row.Cell(columnIndexes[Normalize("PointName")]).GetString()?.Trim();
//                        string pointGroupTypeIdStr = row.Cell(columnIndexes[Normalize("PointGroupTypeId")]).GetString()?.Trim();
//                        string kanalIdStr = row.Cell(columnIndexes[Normalize("KanalId")]).GetString()?.Trim();
//                        string appUserIdStr = row.Cell(columnIndexes[Normalize("AppUserId")]).GetString()?.Trim();
//                        string distributorIdStr = row.Cell(columnIndexes[Normalize("DistributorId")]).GetString()?.Trim();
//                        pointErc = row.Cell(columnIndexes[Normalize("PointErc")]).GetString()?.Trim(); // Burayı dışarıda tanımladığımız değişkene atayın

//                        // Kritik alanlar için temel doğrulama
//                        if (string.IsNullOrWhiteSpace(pointErc))
//                        {
//                            log.Add($"Satır {rowNumber}: 'PointErc' boş. Bu satır atlandı.");
//                            continue; // Bir sonraki satıra geç
//                        }
//                        if (string.IsNullOrWhiteSpace(pointName))
//                        {
//                            log.Add($"Satır {rowNumber}: 'PointName' boş. Bu satır atlandı. (PointErc: {pointErc})");
//                            continue; // Bir sonraki satıra geç
//                        }

//                        // Tamsayı değerlerini güvenli bir şekilde ayrıştırın; başarısız olursa null veya 0 varsayın
//                        int? pointGroupTypeId = int.TryParse(pointGroupTypeIdStr, out var tempPGT) ? tempPGT : (int?)null;
//                        int? distributorId = int.TryParse(distributorIdStr, out var tempDist) ? tempDist : (int?)null;
//                        int kanalId = int.TryParse(kanalIdStr, out var tempKanal) ? tempKanal : 0;
//                        int appUserId = int.TryParse(appUserIdStr, out var tempAppUser) ? tempAppUser : 0;

//                        // Veritabanında belirli bir PointErc'ye sahip bir noktanın zaten var olup olmadığını kontrol edin
//                        var existing = _context.Points.AsNoTracking().FirstOrDefault(x => x.PointErc == pointErc);

//                        if (existing != null)
//                        {
//                            // Eğer varsa, güncellenecek gerçek varlığı alın
//                            var pointToUpdate = _context.Points.Find(existing.PointId); // 'Id'nin birincil anahtar olduğunu varsayalım
//                            if (pointToUpdate != null)
//                            {
//                                pointToUpdate.PointName = pointName;
//                                pointToUpdate.PointGroupTypeId = pointGroupTypeId;
//                                pointToUpdate.KanalId = kanalId;
//                                pointToUpdate.AppUserId = appUserId;
//                                pointToUpdate.DistributorId = distributorId;
//                                _context.Points.Update(pointToUpdate); // Değiştirildi olarak işaretle
//                                log.Add($"Satır {rowNumber}: '{pointErc}' (ID: {existing.PointId}) güncellendi.");
//                            }
//                            else
//                            {
//                                log.Add($"Satır {rowNumber}: Veritabanında '{pointErc}' bulundu ancak güncellenecek nesne alınamadı.");
//                            }
//                        }
//                        else
//                        {
//                            // Yoksa, yeni bir Point varlığı oluşturun
//                            var newPoint = new Point
//                            {
//                                PointName = pointName,
//                                PointGroupTypeId = pointGroupTypeId,
//                                KanalId = kanalId,
//                                AppUserId = appUserId,
//                                DistributorId = distributorId,
//                                PointErc = pointErc
//                            };
//                            _context.Points.Add(newPoint); // Eklendi olarak işaretle
//                            log.Add($"Satır {rowNumber}: '{pointErc}' yeni kayıt olarak eklendi.");
//                        }

//                        // --- ÖNEMLİ DEĞİŞİKLİK: Her satır/varlık için değişiklikleri hemen kaydedin ---
//                        // Bu, diğerleri başarısız olsa bile geçerli satırların kaydedilmesini sağlar.
//                        _context.SaveChanges();
//                    }
//                    catch (DbUpdateException dbEx)
//                    {
//                        // Daha detaylı loglama için spesifik veritabanı güncelleme hatalarını yakalayın
//                        log.Add($"Hata: Satır {rowNumber} için veritabanı hatası (PointErc: {pointErc}): {dbEx.InnerException?.Message ?? dbEx.Message}");
//                    }
//                    catch (Exception ex)
//                    {
//                        // Satır işleme sırasında oluşan diğer tüm istisnaları yakalayın
//                        log.Add($"Hata: Satır {rowNumber} işlenirken genel hata (PointErc: {pointErc}): {ex.Message}");
//                    }
//                }
//            }
//        }
//        catch (IOException ioEx)
//        {
//            // Dosya ile ilgili özel hataları ele alın (örn: dosya kullanımda, bozuk)
//            log.Add($"Hata: Dosya erişim problemi '{excelFilePath}'. Dosya başka bir işlem tarafından kullanılıyor veya bozuk olabilir: {ioEx.Message}");
//        }
//        catch (Exception ex)
//        {
//            // En üst düzeydeki diğer tüm istisnaları yakalayın (örn: çalışma kitabı yükleme sorunları)
//            log.Add($"Genel bir hata oluştu: {ex.Message}");
//        }

//        // Bu özel içe aktarma işlemi için zaman damgalı bir log dosyası oluşturun
//        string timeStr = DateTime.Now.ToString("yyyyMMdd_HHmmss");
//        string logFileName = $"ImportLog_{timeStr}.txt";
//        File.WriteAllLines(Path.Combine(logFolder, logFileName), log);

//        // Genel log dosyasına da tüm mesajları ekleyin
//        File.AppendAllLines(Path.Combine(logFolder, "ImportGeneralLog.txt"), log);

//        return log;
//    }
//}