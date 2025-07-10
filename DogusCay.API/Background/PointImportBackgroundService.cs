//using DogusCay.Business.Importer;

//namespace DogusCay.API.Background
//{
//    public class PointImportBackgroundService : BackgroundService
//    {
//        private readonly IServiceScopeFactory _scopeFactory;

//        public PointImportBackgroundService(IServiceScopeFactory scopeFactory)
//        {
//            _scopeFactory = scopeFactory;
//        }

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
//            string logFolder = Path.Combine(desktop, "DogusCayImportLogs");
//            Directory.CreateDirectory(logFolder);

//            string excelFile = Path.Combine(desktop, "DistPointSql.xlsx");
//            string generalLog = Path.Combine(logFolder, "PointImportGeneralLog.txt");

//            while (!stoppingToken.IsCancellationRequested)
//            {
//                try
//                {
//                    using (var scope = _scopeFactory.CreateScope())
//                    {
//                        var importer = scope.ServiceProvider.GetRequiredService<IPointExcelImporter>();

//                        if (File.Exists(excelFile))
//                        {
//                            File.AppendAllText(
//                                generalLog,
//                                $"{DateTime.Now}: Dosya bulundu, import başlatılıyor\n"
//                            );

//                            List<string> logs;
//                            try
//                            {
//                                logs = importer.ImportPointsFromExcel(excelFile);
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
//                                $"PointImportLog_{DateTime.Now:yyyyMMddHHmmss}.txt"
//                            );
//                            File.WriteAllLines(importLogPath, logs);

//                            File.AppendAllText(
//                                generalLog,
//                                $"{DateTime.Now}: Point Import işlemi bitti, {logs.Count} log var.\n"
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

//                    await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
//                }
//                catch (Exception ex)
//                {
//                    string errorLogPath = Path.Combine(logFolder, "PointImportError.txt");
//                    File.AppendAllText(errorLogPath, $"{DateTime.Now}: {ex}\n");
//                    await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
//                }
//            }
//        }
//    }
//}
