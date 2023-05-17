using Stpm.WebApi.Models.Topic;

namespace Stpm.WebApi.Models.TopicVideo;

public class TopicVideoDto
{
    public int Id { get; set; }

    public string VideoUrl { get; set; }

    public int TopicId { get; set; }

    //public virtual TopicDto Topic { get; set; }
}
