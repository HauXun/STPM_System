namespace Stpm.Core.DTO.AppUser;

public class AppUserQuery
{
    public string Keyword { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string UrlSlug { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
    public string MSSV { get; set; }
    public string PostSlug { get; set; }
    public string TopicSlug { get; set; }

    public int? CommentId { get; set; }
    public int? PostId { get; set; }
    public int? TopicId { get; set; }
}
