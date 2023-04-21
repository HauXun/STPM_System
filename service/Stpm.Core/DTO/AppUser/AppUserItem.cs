namespace Stpm.Core.DTO.AppUser;

public class AppUserItem
{
    public string Id { get; set; } = default!;
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string ImageUrl { get; set; }
    public string UrlSlug { get; set; }
    public DateTime JoinedDate { get; set; }
    public string MSSV { get; set; }

    public int PostCount { get; set; }
    public int TopicCount { get; set; }
    public int NotifyCount { get; set; }
    public int CommentCount { get; set; }
    public int TopicRatingCount { get; set; }
}
