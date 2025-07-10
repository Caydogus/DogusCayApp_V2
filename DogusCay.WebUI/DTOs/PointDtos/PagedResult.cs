namespace DogusCay.WebUI.DTOs.PointDtos
{
    public class PagedResult
    {
        public List<ResultPointDto> Data { get; set; }
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
