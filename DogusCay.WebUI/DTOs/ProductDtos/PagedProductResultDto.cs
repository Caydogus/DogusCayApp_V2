namespace DogusCay.WebUI.DTOs.ProductDtos
{
    public class PagedProductResultDto
    {
        public List<ResultProductDto> Data { get; set; }
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
