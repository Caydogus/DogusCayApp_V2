namespace DogusCay.WebUI.DTOs.TalepDtos
{
    public class PagedTalepFormResponse
    {
        public List<ResultTalepFormDto> Data { get; set; }
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

}
