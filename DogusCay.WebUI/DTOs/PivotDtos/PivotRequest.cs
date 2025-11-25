namespace DogusCay.WebUI.DTOs.PivotDtos
{
    public class PivotRequest
    {
        public string TableName { get; set; }

        public string FilterColumn { get; set; } = "AppUserId";
        public string? FilterValue { get; set; }

        public Dictionary<string, string>? AdditionalFilters { get; set; }
    }
}
