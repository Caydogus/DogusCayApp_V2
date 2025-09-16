using DogusCay.Business.Importer;
using DogusCay.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

string connectionString = config.GetConnectionString("SqlConnection");
string excelPath = config["Excel:Path"];

var optionsBuilder = new DbContextOptionsBuilder<DogusCayContext>();
optionsBuilder.UseSqlServer(connectionString);

var context = new DogusCayContext(optionsBuilder.Options);
var importer = new DistributorExcelImporter(context);

string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
string logFolder = Path.Combine(desktop, "DogusCayImportLogs");
Directory.CreateDirectory(logFolder);
string logFile = Path.Combine(logFolder, $"ConsoleImportLog_{DateTime.Now:yyyyMMdd_HHmmss}.txt");

try
{
    var logs = importer.ImportDistributorsFromExcel(excelPath);

    if (logs.Any())
    {
        File.WriteAllLines(logFile, logs);
        Console.WriteLine($"Log dosyası: {logFile}");
    }
    else
    {
        Console.WriteLine("Hiçbir işlem yapılmadı veya log oluşmadı.");
    }
}
catch (Exception ex)
{
    File.AppendAllText(logFile, $"Genel hata: {ex}\n");
    Console.WriteLine($"Bir hata oluştu: {ex.Message}");
}

Console.WriteLine("Import tamamlandı!");
