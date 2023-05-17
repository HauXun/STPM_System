using Mapster;
using Microsoft.AspNetCore.Identity;
using Stpm.Core.DTO.AppUser;
using Stpm.Core.DTO.Comment;
using Stpm.Core.DTO.Notification;
using Stpm.Core.DTO.NotiLevel;
using Stpm.Core.DTO.Post;
using Stpm.Core.DTO.ProjectTimeline;
using Stpm.Core.DTO.RankAward;
using Stpm.Core.DTO.SpecificAward;
using Stpm.Core.DTO.Tag;
using Stpm.Core.DTO.Timeline;
using Stpm.Core.DTO.Topic;
using Stpm.Core.DTO.TopicRank;
using Stpm.Core.DTO.UserNotify;
using Stpm.Core.DTO.UserTopicRating;
using Stpm.Core.Entities;
using Stpm.WebApi.Models.AppUser;
using Stpm.WebApi.Models.Comment;
using Stpm.WebApi.Models.Notification;
using Stpm.WebApi.Models.NotiLevel;
using Stpm.WebApi.Models.Post;
using Stpm.WebApi.Models.ProjectTimeline;
using Stpm.WebApi.Models.RankAward;
using Stpm.WebApi.Models.SpecificAward;
using Stpm.WebApi.Models.Tag;
using Stpm.WebApi.Models.Timeline;
using Stpm.WebApi.Models.Topic;
using Stpm.WebApi.Models.TopicRank;
using Stpm.WebApi.Models.UserNotify;
using Stpm.WebApi.Models.UserTopicRating;

namespace Stpm.WebApi.Mapsters;

public class MapsterConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AppUser, AppUserDto>();
		config.NewConfig<AppUser, AppUserItem>()
			  .Map(dest => dest.PostCount, src => src.Posts == null ? 0 : src.Posts.Count)
			  .Map(dest => dest.TopicCount, src => src.Topics == null ? 0 : src.Topics.Count)
			  .Map(dest => dest.NotifyCount, src => src.UserNotifies == null ? 0 : src.UserNotifies.Count)
			  .Map(dest => dest.CommentCount, src => src.Comments == null ? 0 : src.Comments.Count)
			  .Map(dest => dest.TopicRatingCount, src => src.UserTopicRatings == null ? 0 : src.UserTopicRatings.Count);

        config.NewConfig<Comment, CommentDto>();
		config.NewConfig<Comment, CommentItem>()
			  .Map(dest => dest.FullName, src => src.User.FullName)
			  .Map(dest => dest.UserMSSV, src => src.User.MSSV)
			  .Map(dest => dest.UserGradeName, src => src.User.GradeName)
              .Map(dest => dest.Mark, src => src.User.UserTopicRatings.FirstOrDefault(u => u.UserId == src.UserId).Mark);

        config.NewConfig<Notification, NotificationDto>();
        config.NewConfig<Notification, NotificationItem>()
              .Map(dest => dest.LevelName, src => src.Level.LevelName)
              .Map(dest => dest.UserCount, src => src.UserNotifies.Count)
              .Map(dest => dest.TimelineCount, src => src.Timelines.Count)
              .Map(dest => dest.AttachmentCount, src => src.NotifyAttachments.Count);

        config.NewConfig<NotiLevel, NotiLevelDto>();
        config.NewConfig<NotiLevel, NotiLevelItem>()
              .Map(dest => dest.NotificationCount, src => src.Notifications.Count);

        config.NewConfig<Post, PostDto>();
		config.NewConfig<Post, PostItem>()
			  .Map(dest => dest.AuthorName, src => src.User.FullName)
              .Map(dest => dest.Tags, src => src.Tags.Select(t => t.Name))
              .Map(dest => dest.TagCount, src => src.Tags.Count)
              .Map(dest => dest.CommentCount, src => src.Comments.Count)
              .Map(dest => dest.PostPhotoCount, src => src.PostPhotos.Count)
              .Map(dest => dest.PostVideoCount, src => src.PostVideos.Count);

        config.NewConfig<RankAward, RankAwardDto>();
        config.NewConfig<RankAwardItem, RankAward>();
        config.NewConfig<RankAward, RankAwardItem>()
              .Map(dest => dest.TopicRankName, src => src.TopicRank.RankName)
              .Map(dest => dest.SpecificAwardCount, src => src.SpecificAwards.Count);

        config.NewConfig<SpecificAward, SpecificAwardDto>();
        config.NewConfig<SpecificAward, SpecificAwardItem>()
              .Map(dest => dest.RankAwardName, src => src.RankAward.AwardName)
              .Map(dest => dest.TopicCount, src => src.Topics.Count);

        config.NewConfig<Tag, TagDto>();
        config.NewConfig<Tag, TagItem>()
              .Map(dest => dest.PostCount, src => src.Posts.Count);

        config.NewConfig<Tag, TagDto>();
        config.NewConfig<Tag, TagItem>()
              .Map(dest => dest.PostCount, src => src.Posts.Count);

        config.NewConfig<Topic, TopicDto>();
        config.NewConfig<Topic, TopicItem>()
              .Map(dest => dest.AwardName, src => src.SpecificAward.RankAward.AwardName)
              .Map(dest => dest.TopicRankName, src => src.TopicRank.RankName)
              .Map(dest => dest.LeaderName, src => src.Leader.FullName)
              .Map(dest => dest.TopicPhotoCount, src => src.TopicPhotos.Count)
              .Map(dest => dest.TopicVideoCount, src => src.TopicVideos.Count)
              .Map(dest => dest.UserCount, src => src.Users.Count)
              .Map(dest => dest.CommentCount, src => src.Comments.Count)
              .Map(dest => dest.TopicRatingCount, src => src.UserTopicRatings.Count)
              .Map(dest => dest.Mark, src => src.UserTopicRatings == null ? null : src.UserTopicRatings.Sum(x => x.Mark));

        config.NewConfig<ProjectTimeline, ProjectTimelineDto>();
        config.NewConfig<ProjectTimeline, ProjectTimelineItem>()
              .Map(dest => dest.TimelineCount, src => src.Timelines.Count);

        config.NewConfig<Timeline, TimelineDto>();
        config.NewConfig<Timeline, TimelineItem>()
              .Map(dest => dest.ProjectTimelineName, src => src.Project.Title)
              .Map(dest => dest.NotifyCount, src => src.Notifies.Count);

        config.NewConfig<TopicRank, TopicRankDto>();
        config.NewConfig<TopicRank, TopicRankItem>()
              .Map(dest => dest.RankAwardCount, src => src.RankAwards.Count)
              .Map(dest => dest.TopicCount, src => src.Topics.Count);

        config.NewConfig<UserNotify, UserNotifyDto>();
        config.NewConfig<UserNotify, UserNotifyItem>()
              .Map(dest => dest.UserName, src => src.User.FullName)
              .Map(dest => dest.Title, src => src.Notify.Title)
              .Map(dest => dest.Content, src => src.Notify.Content)
              .Map(dest => dest.DueDate, src => src.Notify.DueDate)
              .Map(dest => dest.LevelName, src => src.Notify.Level.LevelName);

        config.NewConfig<UserTopicRating, UserTopicRatingDto>();
        config.NewConfig<UserTopicRating, UserTopicRatingItem>()
              .Map(dest => dest.UserName, src => src.User.FullName)
              .Map(dest => dest.TopicName, src => src.Topic.TopicName);
    }
}
