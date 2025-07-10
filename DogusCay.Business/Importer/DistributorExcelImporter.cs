//using ClosedXML.Excel;
//using DogusCay.DataAccess.Context;
//using DogusCay.Entity.Entities;
//using Microsoft.EntityFrameworkCore; // DbUpdateException için eklendi

//namespace DogusCay.Business.Importer
//{
//    public class DistributorExcelImporter : IDistributorExcelImporter
//    {
//        private readonly DogusCayContext _context;

//        public DistributorExcelImporter(DogusCayContext context)
//        {
//            _context = context;
//        }

//        public List<string> ImportDistributorsFromExcel(string excelFilePath)
//        {
//            var log = new List<string>();
//            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
//            string logFolder = Path.Combine(desktop, "DogusCayImportLogs");

//            // Log klasörünün var olduğundan emin olun
//            if (!Directory.Exists(logFolder))
//                Directory.CreateDirectory(logFolder);

//            // Excel dosyasının varlığını kontrol edin
//            if (!File.Exists(excelFilePath))
//            {
//                log.Add($"Hata: Dosya bulunamadı: {excelFilePath}");
//                // Genel log dosyasına da hatayı yazın
//                File.AppendAllLines(Path.Combine(logFolder, "ImportGeneralLog.txt"), log);
//                return log;
//            }

//            try
//            {
//                using (var workbook = new XLWorkbook(excelFilePath))
//                {
//                    // "distmaster" adlı çalışma sayfasını alın
//                    var ws = workbook.Worksheet("distmaster");
//                    if (ws == null)
//                    {
//                        log.Add($"Hata: 'distmaster' adında çalışma sayfası bulunamadı. Lütfen Excel dosyasındaki çalışma sayfası adını kontrol edin.");
//                        File.AppendAllLines(Path.Combine(logFolder, "ImportGeneralLog.txt"), log);
//                        return log;
//                    }

//                    var headerRow = ws.Row(1);

//                    // Başlıkları normalize etmek için yardımcı fonksiyon
//                    string Normalize(string s) => s?.Trim().Replace(" ", "").ToLowerInvariant();

//                    // SQL başlıklarına göre normalize edilmiş gerekli anahtarlar
//                    var requiredHeaders = new[] { "DistributorName", "AppUserId", "KanalId", "DistributorErcKod" }
//                        .Select(Normalize).ToList();

//                    // Sütun adlarını indeksleriyle eşleştirin
//                    var columnIndexes = new Dictionary<string, int>();
//                    foreach (var cell in headerRow.Cells())
//                    {
//                        var colName = Normalize(cell.GetString());
//                        if (!string.IsNullOrWhiteSpace(colName) && !columnIndexes.ContainsKey(colName))
//                            columnIndexes[colName] = cell.Address.ColumnNumber;
//                    }

//                    // Gerekli başlıkların eksik olup olmadığını kontrol edin
//                    var missingHeaders = requiredHeaders.Where(h => !columnIndexes.ContainsKey(h)).ToList();
//                    if (missingHeaders.Any())
//                    {
//                        log.Add($"Hata: Eksik başlık(lar): {string.Join(", ", missingHeaders)}. İşlem iptal edildi.");
//                        File.AppendAllLines(Path.Combine(logFolder, "ImportGeneralLog.txt"), log);
//                        return log;
//                    }

//                    // Başlık satırını atlayarak her veri satırını işleyin
//                    foreach (var row in ws.RowsUsed().Skip(1))
//                    {
//                        int rowNumber = row.RowNumber(); // Loglama için mevcut satır numarasını alın
//                        string distributorErcKod = "Bilinmiyor"; // Hata durumunda ErcKod'u loglamak için başlangıç değeri

//                        try
//                        {
//                            // Verileri hücrelerden al ve temizle
//                            string distributorName = row.Cell(columnIndexes[Normalize("DistributorName")]).GetString()?.Trim();
//                            string appUserIdStr = row.Cell(columnIndexes[Normalize("AppUserId")]).GetString()?.Trim();
//                            string kanalIdStr = row.Cell(columnIndexes[Normalize("KanalId")]).GetString()?.Trim();
//                            distributorErcKod = row.Cell(columnIndexes[Normalize("DistributorErcKod")]).GetString()?.Trim(); // Burayı dışarıda tanımladığımız değişkene atayın

//                            // Kritik alanlar için temel doğrulama
//                            if (string.IsNullOrWhiteSpace(distributorErcKod))
//                            {
//                                log.Add($"Satır {rowNumber}: 'DistributorErcKod' boş. Bu satır atlandı.");
//                                continue; // Bir sonraki satıra geç
//                            }
//                            if (string.IsNullOrWhiteSpace(distributorName))
//                            {
//                                log.Add($"Satır {rowNumber}: 'DistributorName' boş. Bu satır atlandı. (ErcKod: {distributorErcKod})");
//                                continue; // Bir sonraki satıra geç
//                            }

//                            // Tamsayı değerlerini güvenli bir şekilde ayrıştırın; başarısız olursa 0 varsayın
//                            int appUserId = int.TryParse(appUserIdStr, out var tempAppUserId) ? tempAppUserId : 0;
//                            int kanalId = int.TryParse(kanalIdStr, out var tempKanalId) ? tempKanalId : 0;

//                            // Veritabanında belirli bir ErcKod'a sahip bir distribütörün zaten var olup olmadığını kontrol edin
//                            // Performans için AsNoTracking() kullanın, sadece varlığını kontrol ediyorsanız
//                            var existing = _context.Distributors.AsNoTracking().FirstOrDefault(x => x.DistributorErcKod == distributorErcKod);

//                            if (existing != null)
//                            {
//                                // Eğer varsa, güncellenecek gerçek varlığı alın
//                                var distributorToUpdate = _context.Distributors.Find(existing.DistributorId); // 'Id'nin birincil anahtar olduğunu varsayalım
//                                if (distributorToUpdate != null)
//                                {
//                                    distributorToUpdate.DistributorName = distributorName;
//                                    distributorToUpdate.AppUserId = appUserId;
//                                    distributorToUpdate.KanalId = kanalId;
//                                    _context.Distributors.Update(distributorToUpdate); // Değiştirildi olarak işaretle
//                                    log.Add($"Satır {rowNumber}: '{distributorErcKod}' (ID: {existing.DistributorId}) güncellendi.");
//                                }
//                                else
//                                {
//                                    log.Add($"Satır {rowNumber}: Veritabanında '{distributorErcKod}' bulundu ancak güncellenecek nesne alınamadı.");
//                                }
//                            }
//                            else
//                            {
//                                // Yoksa, yeni bir Distribütör varlığı oluşturun
//                                var newDist = new Distributor
//                                {
//                                    DistributorName = distributorName,
//                                    AppUserId = appUserId,
//                                    KanalId = kanalId,
//                                    DistributorErcKod = distributorErcKod
//                                };
//                                _context.Distributors.Add(newDist); // Eklendi olarak işaretle
//                                log.Add($"Satır {rowNumber}: '{distributorErcKod}' yeni kayıt olarak eklendi.");
//                            }

//                            // --- ÖNEMLİ DEĞİŞİKLİK: Her satır/varlık için değişiklikleri hemen kaydedin ---
//                            // Bu, diğerleri başarısız olsa bile geçerli satırların kaydedilmesini sağlar.
//                            _context.SaveChanges();
//                            // Eğer toplu kaydetmeyi (örneğin her 50 satırda bir) tercih ediyorsanız,
//                            // varlıkları bir listeye ekleyip SaveChanges()'i periyodik olarak çağırmanız gerekir.
//                            // Ancak satır bazında hata işleme için, her satırda kaydetmek idealdir.
//                        }
//                        catch (DbUpdateException dbEx)
//                        {
//                            // Daha detaylı loglama için spesifik veritabanı güncelleme hatalarını yakalayın
//                            log.Add($"Hata: Satır {rowNumber} için veritabanı hatası (ErcKod: {distributorErcKod}): {dbEx.InnerException?.Message ?? dbEx.Message}");
//                            // Not: Eğer bir satırda DB hatası olursa (örn: benzersiz kısıtlama ihlali),
//                            // mevcut context state'i bozulabilir. Bu durumda, daha fazla kayıt yapmadan
//                            // önce context'i yeniden başlatmak veya entry state'ini temizlemek gerekebilir.
//                            // Ancak bu senaryoda, her save sonrasında yeni bir context başlatılmadığı için
//                            // sonraki işlemler de başarısız olabilir. Eğer bu bir sorunsa, bu metodu
//                            // Transaction Scope veya her bir Add/Update işlemi için yeni bir context oluşturarak
//                            // daha da güçlendirmeniz gerekebilir (ancak performans maliyeti olur).
//                            // Basit çözüm olarak, hata veren satırı atlamak en iyisidir.
//                        }
//                        catch (Exception ex)
//                        {
//                            // Satır işleme sırasında oluşan diğer tüm istisnaları yakalayın
//                            log.Add($"Hata: Satır {rowNumber} işlenirken genel hata (ErcKod: {distributorErcKod}): {ex.Message}");
//                        }
//                    }
//                }
//            }
//            catch (IOException ioEx)
//            {
//                // Dosya ile ilgili özel hataları ele alın (örn: dosya kullanımda, bozuk)
//                log.Add($"Hata: Dosya erişim problemi '{excelFilePath}'. Dosya başka bir işlem tarafından kullanılıyor veya bozuk olabilir: {ioEx.Message}");
//            }
//            catch (Exception ex)
//            {
//                // En üst düzeydeki diğer tüm istisnaları yakalayın (örn: çalışma kitabı yükleme sorunları)
//                log.Add($"Genel bir hata oluştu: {ex.Message}");
//            }

//            // Bu özel içe aktarma işlemi için zaman damgalı bir log dosyası oluşturun
//            string timeStr = DateTime.Now.ToString("yyyyMMdd_HHmmss");
//            string logFileName = $"ImportLog_{timeStr}.txt";
//            File.WriteAllLines(Path.Combine(logFolder, logFileName), log);

//            // Genel log dosyasına da tüm mesajları ekleyin
//            // Mevcut AppendAllLines kullanımı zaten her seferinde tüm log'u yeniden yazıyor gibi görünüyor,
//            // bunu düzeltmek için File.AppendAllLines'ı kullanmalıyız.
//            // Önceki çağrılarda zaten yapıldığı için burada tekrar eklemeye gerek olmayabilir
//            // ancak tutarlılık için tekrar ekledim.
//            File.AppendAllLines(Path.Combine(logFolder, "ImportGeneralLog.txt"), log);

//            return log;
//        }
//    }
//}