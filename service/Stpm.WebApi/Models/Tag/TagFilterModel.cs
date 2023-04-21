using System.ComponentModel;

namespace Stpm.WebApi.Models.Tag;

public class TagFilterModel
{
    [DisplayName("Từ khoá")]
    public string Keyword { get; set; }
    [DisplayName("Tên thẻ")]
    public string Name { get; set; }
    [DisplayName("Post-Slug")]
    public string PostSlug { get; set; }
    [DisplayName("Bài viết")]
    public int? PostId { get; set; }
}
