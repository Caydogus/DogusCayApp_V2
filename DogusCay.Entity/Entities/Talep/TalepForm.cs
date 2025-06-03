using DogusCay.Entity.Entities.Talep;
using DogusCay.Entity.Entities;

public class TalepForm
{
    public int TalepFormId { get; set; }

    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; }

    public TalepTip TalepTip { get; set; }

    public int KanalId { get; set; }
    public Kanal Kanal { get; set; }
    public int? DistributorId { get; set; }
    public Distributor? Distributor { get; set; }
    public int? PointGroupTypeId { get; set; }
    public PointGroupType? PointGroupType { get; set; }
    public int PointId { get; set; }
    public Point Point { get; set; }
    public int CategoryId { get; set; }
    public int? SubCategoryId { get; set; }
    public int? SubSubCategoryId { get; set; }
    public Category Category { get; set; }
    public Category SubCategory { get; set; }
    public Category SubSubCategory { get; set; }

    public int ProductId { get; set; } // Insert tipi için zorunlu
    public Product Product { get; set; }
    public string ProductName { get; set; }
    public string ErpCode { get; set; }
    public int Quantity { get; set; }
    public decimal? Price { get; set; }
    public int? KoliIciAdet { get; set; }
    public decimal Total { get; set; }
    public decimal? ApproximateWeightKg { get; set; }
    public decimal? SabitBedelTL { get; set; }
    public decimal? Iskonto1 { get; set; }
    public decimal? Iskonto2 { get; set; }
    public decimal? Iskonto3 { get; set; }
    public decimal? Iskonto4 { get; set; }
    public string? Note { get; set; }
    public decimal KoliToplamAgirligiKg { get; set; }
    public int KoliIciToplamAdet { get; set; }
    public decimal ListeFiyat { get; set; }
    public decimal SonAdetFiyati { get; set; }
    public decimal AdetFarkDonusuTL { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public TalepDurumu TalepDurumu { get; set; } = TalepDurumu.Bekliyor;
    public int? OnaylayanAdminId { get; set; }
    public AppUser? OnaylayanAdmin { get; set; }

}
