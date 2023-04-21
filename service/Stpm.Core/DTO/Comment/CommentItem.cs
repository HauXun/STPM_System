namespace Stpm.Core.DTO.Comment;

public partial class CommentItem
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime Date { get; set; }
    public DateTime ModifiedDate { get; set; }
    public string UserName { get; set; }
    public string UserMSSV { get; set; }
}
