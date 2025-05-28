using DogusCay.Entity.Entities.Talep;

namespace DogusCay.WebUI.DTOs.TalepDtos
{
    public class ResultTalepFormDto
    {
        public int TalepFormId { get; set; }
        public string UserFullName { get; set; }
        public string KanalName { get; set; }
        public string? DistributorName { get; set; }
        public string? PointGroupTypeName { get; set; }
        public string PointName { get; set; }
        public string? Note { get; set; }
        public TalepTip TalepTip { get; set; }
        public TalepDurumu TalepDurumu { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public List<ResultTalepFormItemDto> Items { get; set; } = new();
    }
}
