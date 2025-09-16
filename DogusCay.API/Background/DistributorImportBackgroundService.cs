using DogusCay.Business.Importer;
using Microsoft.Extensions.Configuration;

namespace DogusCay.API.Background
{
    public class DistributorImportBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;

        public DistributorImportBackgroundService(IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // appsettings.json’dan ayarları oku
            string excelFile = _configuration["DistributorImport:ExcelFilePath"];
            string logFolder = _configuration["DistributorImport:LogFolder"];
            int intervalSeconds = int.TryParse(_configuration["DistributorImport:IntervalSeconds"], out var s) ? s : 86400;

            Directory.CreateDirectory(logFolder);

            string generalLog = Path.Combine(logFolder, "ImportGeneralLog.txt");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var importer = scope.ServiceProvider.GetRequiredService<IDistributorExcelImporter>();

                        if (File.Exists(excelFile))
                        {
                            File.AppendAllText(generalLog, $"{DateTime.Now}: Dosya bulundu, import başlatılıyor\n");

                            List<string> logs;
                            try
                            {
                                logs = importer.ImportDistributorsFromExcel(excelFile);
                            }
                            catch (IOException ioEx)
                            {
                                File.AppendAllText(generalLog, $"{DateTime.Now}: Dosya başka bir işlem tarafından kullanılıyor: {ioEx.Message}\n");
                                logs = new List<string> { $"Dosya kilitli: {ioEx.Message}" };
                            }

                            // Importer zaten log yazıyor, burada sadece özet yazabiliriz
                            File.AppendAllText(generalLog, $"{DateTime.Now}: DistImport işlemi bitti, {logs.Count} satır işlendi.\n");
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
                    string errorLogPath = Path.Combine(logFolder, "DistributorImportError.txt");
                    File.AppendAllText(errorLogPath, $"{DateTime.Now}: {ex}\n");
                    await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
                }
            }
        }
    }
}
