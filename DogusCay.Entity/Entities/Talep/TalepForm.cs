using DogusCay.Entity.Entities.Talep;
using DogusCay.Entity.Entities;

public class TalepForm
{
    public int TalepFormId { get; set; }

    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; }

    public TalepTip TalepTip { get; set; } = TalepTip.Insert;

    public int KanalId { get; set; }
    public Kanal Kanal { get; set; }

    public int? DistributorId { get; set; }
    public Distributor? Distributor { get; set; }

    public int? PointGroupTypeId { get; set; }
    public PointGroupType? PointGroupType { get; set; }

    public int PointId { get; set; }
    public Point Point { get; set; }

    public DateTime TalepBaslangicTarihi { get; set; } 
    public DateTime TalepBitisTarihi { get; set; } 

    public TalepDurumu TalepDurumu { get; set; } = TalepDurumu.Bekliyor;

    public int? OnaylayanAdminId { get; set; }
    public AppUser? OnaylayanAdmin { get; set; }

    public string? Note { get; set; }

    public ICollection<TalepFormItem> TalepFormItems { get; set; }
}
