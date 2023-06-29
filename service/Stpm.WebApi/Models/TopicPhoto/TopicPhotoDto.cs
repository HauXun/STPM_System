using Stpm.WebApi.Models.Topic;

namespace Stpm.WebApi.Models.TopicPhoto;

public class TopicPhotoDto
{
    public int Id { get; set; }

    public string ImageUrl { get; set; }

    public int TopicId { get; set; }

    //public virtual TopicDto Topic { get; set; }
}
