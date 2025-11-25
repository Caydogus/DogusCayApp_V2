namespace DogusCay.WebUI.DTOs.PivotDtos
{
    public class PivotResponse
    {
        // API → WebUI’ya dönen ham pivot datası
        public List<Dictionary<string, object>> Rows { get; set; }
    }
}
