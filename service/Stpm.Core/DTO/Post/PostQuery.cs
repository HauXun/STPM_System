namespace Stpm.Core.DTO.Post;

public class PostQuery
{
    public string Keyword { get; set; }
    public bool? Published { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? Day { get; set; }
    public string UserId { get; set; }
    public int? TopicId { get; set; }
    public string UserSlug { get; set; }
    public string TopicUrl { get; set; }
    public string Tags { get; set; }
    public string PostSlug { get; set; }
    public string TagSlug { get; set; }
    public IList<string> SelectedTag { get; set; }

    public void GetTagListAsync()
    {
        SelectedTag = (Tags ?? "").Split(new[] { ",", ";", ".", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
    }
}