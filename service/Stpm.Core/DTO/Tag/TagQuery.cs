namespace Stpm.Core.DTO.Tag;

public class TagQuery
{
    public string Keyword { get; set; }
    public string Name { get; set; }
    public string UrlSlug { get; set; }
    public string PostSlug { get; set; }
    public int? PostId { get; set; }
}