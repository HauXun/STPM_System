namespace Stpm.Core.DTO.Comment;

public partial class CommentQuery
{
    public string Keyword { get; set; }
    public string Content { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? Day { get; set; }
    public string TopicSlug { get; set; }
    public string UserSlug { get; set; }
    public int? TopicId { get; set; }
    public int? PostId { get; set; }
    public string UserId { get; set; }
}
