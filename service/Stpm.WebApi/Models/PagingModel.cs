using Stpm.Core.Contracts;

namespace Stpm.WebApi.Models;

public class PagingModel : IPagingParams
{
    private int? _pageSize;
    private int? _pageNumber;

    int IPagingParams.PageSize
    {
        get => _pageSize ?? 10;
        set => _pageSize = value;
    }

    int IPagingParams.PageNumber
    {
        get => _pageNumber ?? 1;
        set => _pageNumber = value;
    }

    public int? PageSize
    {
        get => _pageSize;
        set => _pageSize = value;
    }
    public int? PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value;
    }
    public string SortColumn { get; set; } = "Id";
	public string SortOrder { get; set; } = "DESC";
}
