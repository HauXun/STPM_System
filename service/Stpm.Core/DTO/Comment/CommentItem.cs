namespace Stpm.Core.DTO.Comment;

public partial class CommentItem
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime Date { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string FullName { get; set; }
    public string UserMSSV { get; set; }
    public string UserGradeName { get; set; }
    public float? Mark { get; set; }
}
