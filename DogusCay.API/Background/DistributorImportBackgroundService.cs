//using DogusCay.Business.Importer;

//namespace DogusCay.API.Background
//{
//    public class DistributorImportBackgroundService : BackgroundService
//    {
//        private readonly IServiceScopeFactory _scopeFactory;

//        public DistributorImportBackgroundService(IServiceScopeFactory scopeFactory)
//        {
//            _scopeFactory = scopeFactory;
//        }

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
//            string logFolder = Path.Combine(desktop, "DogusCayImportLogs");
//            Directory.CreateDirectory(logFolder);

//            string excelFile = Path.Combine(desktop, "DistPointSql.xlsx");
//            string generalLog = Path.Combine(logFolder, "ImportGeneralLog.txt");

//            while (!stoppingToken.IsCancellationRequested)
//            {
//                try
//                {
//                    using (var scope = _scopeFactory.CreateScope())
//                    {
//                        var importer = scope.ServiceProvider.GetRequiredService<IDistributorExcelImporter>();

//                        if (File.Exists(excelFile))
//                        {
//                            File.AppendAllText(
//                                generalLog,
//                                $"{DateTime.Now}: Dosya bulundu, import başlatılıyor\n"
//                            );

//                            List<string> logs;
//                            try
//                            {
//                                logs = importer.ImportDistributorsFromExcel(excelFile);
//                            }
//                            catch (IOException ioEx)
//                            {
//                                File.AppendAllText(
//                                    generalLog,
//                                    $"{DateTime.Now}: Dosya başka bir işlem tarafından kullanılıyor: {ioEx.Message}\n"
//                                );
//                                logs = new List<string> { $"Dosya kilitli: {ioEx.Message}" };
//                            }

//                            string importLogPath = Path.Combine(
//                                logFolder,
//                                $"DistImportLog_{DateTime.Now:yyyyMMddHHmmss}.txt"
//                            );
//                            File.WriteAllLines(importLogPath, logs);

//                            File.AppendAllText(
//                                generalLog,
//                                $"{DateTime.Now}: DistImport işlemi bitti, {logs.Count} log var.\n"
//                            );
//                        }
//                        else
//                        {
//                            File.AppendAllText(
//                                generalLog,
//                                $"{DateTime.Now}: Dosya bulunamadı: {excelFile}\n"
//                            );
//                        }
//                    }

//                    // Test için 10 saniye, canlıda örn. 1 saat: TimeSpan.FromHours(1)
//                    await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
//                }
//                catch (Exception ex)
//                {
//                    string errorLogPath = Path.Combine(logFolder, "DistributorImportError.txt");
//                    File.AppendAllText(errorLogPath, $"{DateTime.Now}: {ex}\n");
//                    await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
//                }
//            }
//        }
//    }
//}
