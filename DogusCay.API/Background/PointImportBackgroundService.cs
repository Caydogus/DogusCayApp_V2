using DogusCay.Business.Importer;
using Microsoft.Extensions.Configuration;

namespace DogusCay.API.Background
{
    public class PointImportBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;

        public PointImportBackgroundService(IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // appsettings.json’dan oku
            string excelFile = _configuration["PointImport:ExcelFilePath"];
            string logFolder = _configuration["PointImport:LogFolder"];
            int intervalSeconds = int.TryParse(_configuration["PointImport:IntervalSeconds"], out var s) ? s : 86400;

            Directory.CreateDirectory(logFolder);

            string generalLog = Path.Combine(logFolder, "PointImportGeneralLog.txt");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var importer = scope.ServiceProvider.GetRequiredService<IPointExcelImporter>();

                        if (File.Exists(excelFile))
                        {
                            File.AppendAllText(generalLog, $"{DateTime.Now}: Dosya bulundu, import başlatılıyor\n");

                            List<string> logs;
                            try
                            {
                                logs = importer.ImportPointsFromExcel(excelFile);
                            }
                            catch (IOException ioEx)
                            {
                                File.AppendAllText(generalLog, $"{DateTime.Now}: Dosya başka bir işlem tarafından kullanılıyor: {ioEx.Message}\n");
                                logs = new List<string> { $"Dosya kilitli: {ioEx.Message}" };
                            }

                            // Importer detaylı logları zaten yazıyor, burada sadece özet bırak
                            File.AppendAllText(generalLog, $"{DateTime.Now}: Point Import işlemi bitti, {logs.Count} satır işlendi.\n");
                        }
                        else
                        {
                            File.AppendAllText(generalLog, $"{DateTime.Now}: Dosya bulunamadı: {excelFile}\n");
                        }
                    }

                    // Bekleme süresi
                    await Task.Delay(TimeSpan.FromSeconds(intervalSeconds), stoppingToken);
                }
                catch (Exception ex)
                {
                    string errorLogPath = Path.Combine(logFolder, "PointImportError.txt");
                    File.AppendAllText(errorLogPath, $"{DateTime.Now}: {ex}\n");
                    await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
                }
            }
        }
    }
}
