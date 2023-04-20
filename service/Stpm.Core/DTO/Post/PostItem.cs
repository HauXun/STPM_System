namespace Stpm.Core.DTO.Post;

public class PostItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public string Meta { get; set; }
    public string UrlSlug { get; set; }
    public int ViewCount { get; set; }
    public bool Published { get; set; }
    public DateTime PostedDate { get; set; }
    public string TopicName { get; set; }
    public string AuthorName { get; set; }
    public IEnumerable<string> Tags { get; set; }
    public int TagCount { get; set; }
    public int CommentCount { get; set; }
    public int PostPhotoCount { get; set; }
    public int PostVideoCount { get; set; }
}
