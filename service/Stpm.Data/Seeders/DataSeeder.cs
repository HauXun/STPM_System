using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stpm.Core.Constants.Role;
using Stpm.Core.Entities;
using Stpm.Data.Contexts;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Stpm.Data.Seeders;

public class DataSeeder : IDataSeeder
{
    private readonly StpmDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppUserRole> _roleManager;

    public DataSeeder(StpmDbContext dbContext, UserManager<AppUser> userManager, RoleManager<AppUserRole> roleManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public void Initialize()
    {
        _dbContext.Database.EnsureCreated();

        if (_dbContext.Users.Any()) return;

        Randomizer.Seed = new Random(8675309);
        RandomSeedData().GetAwaiter().GetResult();
    }

    private async Task RandomSeedData()
    {
        var roles = await AddRoles();
        await AddDefaultUsers();
        var users = /*_dbContext.Users.Any() ? _dbContext.Users.ToList() :*/ await AddUsers(roles);
        var notiLevels = _dbContext.NotiLevels.Any() ? _dbContext.NotiLevels.ToList() : AddNotiLevels();
        var notifications = _dbContext.Notifications.Any() ? _dbContext.Notifications.ToList() : AddNotifications(notiLevels);
        var notifyAttachments = _dbContext.NotifyAttachments.Any() ? _dbContext.NotifyAttachments.ToList() : AddNotifyAttachments(notifications);
        var projectTimelines = _dbContext.ProjectTimelines.Any() ? _dbContext.ProjectTimelines.ToList() : AddProjectTimelines();
        var timelines = _dbContext.Timelines.Any() ? _dbContext.Timelines.ToList() : AddTimelines(projectTimelines, notifications);
        var topicRanks = _dbContext.TopicRanks.Any() ? _dbContext.TopicRanks.ToList() : AddTopicRanks();
        var rankAwards = _dbContext.RankAwards.Any() ? _dbContext.RankAwards.ToList() : AddRankAwards(topicRanks);
        var specificAwards = _dbContext.SpecificAwards.Any() ? _dbContext.SpecificAwards.ToList() : AddSpecificAwards(rankAwards);
        var topics = _dbContext.Topics.Any() ? _dbContext.Topics.ToList() : AddTopics(topicRanks, specificAwards, users);
        var topicPhotos = _dbContext.TopicPhotos.Any() ? _dbContext.TopicPhotos.ToList() : AddTopicPhotos(topics);
        var topicVideos = _dbContext.TopicVideos.Any() ? _dbContext.TopicVideos.ToList() : AddTopicVideos(topics);
        var tags = _dbContext.Tags.Any() ? _dbContext.Tags.ToList() : AddTags();
        var posts = _dbContext.Posts.Any() ? _dbContext.Posts.ToList() : AddPosts(users, tags);
        var postPhotos = _dbContext.PostPhotos.Any() ? _dbContext.PostPhotos.ToList() : AddPostPhotos(posts);
        var postVideos = _dbContext.PostVideos.Any() ? _dbContext.PostVideos.ToList() : AddPostVideos(posts);
        var topicComments = _dbContext.Comments.Any() ? _dbContext.Comments.ToList() : AddTopicComments(users, topics);
        var postComments = /*_dbContext.Comments.Any() ? _dbContext.Comments.ToList() : */ AddPostComments(users, posts);
        var userNotify = _dbContext.UserNotifies.Any() ? _dbContext.UserNotifies.ToList() : AddUserNotify(users, notifications);
        var userTopicRating = _dbContext.UserTopicRatings.Any() ? _dbContext.UserTopicRatings.ToList() : AddUserTopicRating(users, topics);
        var topicHistoryAward = _dbContext.TopicHistoryAwards.Any() ? _dbContext.TopicHistoryAwards.ToList() : AddTopicHistoryAward();
    }

    private List<Comment> AddPostComments(List<AppUser> users, List<Post> posts)
    {
        var fakeComments = new Faker<Comment>("vi");
        fakeComments.RuleFor(u => u.Content, f => f.Lorem.Sentence(10, 5));
        fakeComments.RuleFor(u => u.Date, f => f.Date.Between(new DateTime(2021, 5, 10), new DateTime(2023, 5, 10)));
        fakeComments.RuleFor(u => u.User, f => f.PickRandom(users));
        fakeComments.RuleFor(u => u.Posts, f => f.PickRandom(posts, 2).ToList());

        var comments = fakeComments.Generate(50);

        _dbContext.Comments.AddRange(comments);
        _dbContext.SaveChanges();

        return comments;
    }

    private List<Timeline> AddTimelines(List<ProjectTimeline> projectTimelines, List<Notification> notifications)
    {
        var fakeTimelines = new Faker<Timeline>("vi");
        fakeTimelines.RuleFor(u => u.Title, f => f.Lorem.Sentence());
        fakeTimelines.RuleFor(u => u.ShortDescription, f => f.Lorem.Paragraph());
        fakeTimelines.RuleFor(u => u.DueDate, f => f.Date.Between(new DateTime(2021, 5, 10), new DateTime(2023, 5, 10)));
        fakeTimelines.RuleFor(u => u.Project, f => f.PickRandom(projectTimelines));
        fakeTimelines.RuleFor(u => u.Notifies, f => f.PickRandom(notifications, 3).ToList());

        var timelines = fakeTimelines.Generate(100);

        _dbContext.Timelines.AddRange(timelines);
        _dbContext.SaveChanges();

        return timelines;
    }

    private List<ProjectTimeline> AddProjectTimelines()
    {
        var fakeProjectTimelines = new Faker<ProjectTimeline>("vi");
        fakeProjectTimelines.RuleFor(u => u.Title, f => f.Lorem.Sentence());
        fakeProjectTimelines.RuleFor(u => u.ShortDescription, f => f.Lorem.Paragraph());
        fakeProjectTimelines.RuleFor(u => u.ShowOn, f => false);

        var projectTimelines = fakeProjectTimelines.Generate(100);

        _dbContext.ProjectTimelines.AddRange(projectTimelines);
        _dbContext.SaveChanges();

        return projectTimelines;
    }

    private List<NotifyAttachment> AddNotifyAttachments(List<Notification> notifications)
    {
        var fakeNotifyAttachments = new Faker<NotifyAttachment>("vi");
        fakeNotifyAttachments.RuleFor(p => p.AttachmentUrl, f => f.Image.PicsumUrl());
        fakeNotifyAttachments.RuleFor(p => p.Notify, f => f.PickRandom(notifications));

        var notifyAttachments = fakeNotifyAttachments.Generate(100);

        _dbContext.NotifyAttachments.AddRange(notifyAttachments);
        _dbContext.SaveChanges();

        return notifyAttachments;
    }

    private List<PostPhoto> AddPostPhotos(List<Post> posts)
    {
        var fakePostPhotos = new Faker<PostPhoto>("vi");
        fakePostPhotos.RuleFor(p => p.ImageUrl, f => f.Image.PicsumUrl());
        fakePostPhotos.RuleFor(p => p.Post, f => f.PickRandom(posts));

        var postPhotos = fakePostPhotos.Generate(100);

        _dbContext.PostPhotos.AddRange(postPhotos);
        _dbContext.SaveChanges();

        return postPhotos;
    }

    private List<PostVideo> AddPostVideos(List<Post> posts)
    {
        var fakePostVideos = new Faker<PostVideo>("vi");
        fakePostVideos.RuleFor(p => p.VideoUrl, f => "https://www.youtube.com/embed/mw9WcQo6aIY");
        fakePostVideos.RuleFor(p => p.Post, f => f.PickRandom(posts));

        var postVideos = fakePostVideos.Generate(100);

        _dbContext.PostVideos.AddRange(postVideos);
        _dbContext.SaveChanges();

        return postVideos;
    }

    private List<TopicVideo> AddTopicVideos(List<Topic> topics)
    {
        var fakeTopicVideos = new Faker<TopicVideo>("vi");
        fakeTopicVideos.RuleFor(p => p.VideoUrl, f => "https://www.youtube.com/embed/mw9WcQo6aIY");
        fakeTopicVideos.RuleFor(p => p.Topic, f => f.PickRandom(topics));

        var topicVideos = fakeTopicVideos.Generate(100);

        _dbContext.TopicVideos.AddRange(topicVideos);
        _dbContext.SaveChanges();

        return topicVideos;
    }

    private List<TopicPhoto> AddTopicPhotos(List<Topic> topics)
    {
        var fakeTopicPhotos = new Faker<TopicPhoto>("vi");
        fakeTopicPhotos.RuleFor(p => p.ImageUrl, f => f.Image.PicsumUrl());
        fakeTopicPhotos.RuleFor(p => p.Topic, f => f.PickRandom(topics));

        var topicPhotos = fakeTopicPhotos.Generate(100);

        _dbContext.TopicPhotos.AddRange(topicPhotos);
        _dbContext.SaveChanges();

        return topicPhotos;
    }

    private List<Tag> AddTags()
    {
        var fakeTags = new Faker<Tag>("vi");
        fakeTags.RuleFor(t => t.Name, f => f.Lorem.Word());
        fakeTags.Rules((f, u) =>
        {
            u.UrlSlug = u.Name.GenerateSlug();
        });
        fakeTags.RuleFor(t => t.Description, f => f.Lorem.Sentence());

        var tags = fakeTags.Generate(100);

        _dbContext.Tags.AddRange(tags);
        _dbContext.SaveChanges();

        return _dbContext.Tags.ToList();
    }

    private List<TopicHistoryAward> AddTopicHistoryAward()
    {
        var fakeTopicHistoryAward = new Faker<TopicHistoryAward>("vi");
        fakeTopicHistoryAward.RuleFor(t => t.TopicName, f => f.Commerce.ProductName());
        fakeTopicHistoryAward.Rules((f, u) =>
        {
            u.UrlSlug = u.TopicName.GenerateSlug();
        });
        fakeTopicHistoryAward.RuleFor(t => t.TopicAward, f => f.PickRandom("Giải nhất", "Giải Nhì", "Giải ba", "Giải khuyến khích"));
        fakeTopicHistoryAward.RuleFor(t => t.TopicRank, f => f.PickRandom("Hạng mục 1", "Hạng mục 2"));
        fakeTopicHistoryAward.RuleFor(t => t.Year, f => f.Random.Short(2021, 2023));
        fakeTopicHistoryAward.RuleFor(u => u.Fullscore, f => float.Round(f.Random.Float(1, 10), 2));

        var historyAwards = fakeTopicHistoryAward.Generate(50);

        _dbContext.TopicHistoryAwards.AddRange(historyAwards);
        _dbContext.SaveChanges();

        return historyAwards;
    }

    private List<UserTopicRating> AddUserTopicRating(List<AppUser> users, List<Topic> topics)
    {
        List<UserTopicRating> ratings = new List<UserTopicRating>();
        var fakeUserTopicRating = new Faker<UserTopicRating>("vi");
        fakeUserTopicRating.Rules((f, u) =>
        {
            var user = f.PickRandom(users);
            var topic = f.PickRandom(topics);

            while (
            _dbContext.UserTopicRatings
                      .Include(x => x.User)
                      .Include(x => x.Topic)
                      .Any(x => x.UserId == user.Id && x.TopicId == topic.Id))
            {
                user = f.PickRandom(users);
                topic = f.PickRandom(topics);
            }

            u.User = user;
            u.Topic = topic;
        });
        fakeUserTopicRating.RuleFor(u => u.Mark, f => float.Round(f.Random.Float(1, 10), 2));


        int i = 0;
        do
        {
            if (i >= 100)
                break;

            UserTopicRating rating = fakeUserTopicRating.Generate();
            _dbContext.UserTopicRatings.Add(rating);
            var result = _dbContext.SaveChangesAsync().GetAwaiter().GetResult();

            if (result > 0)
            {
                ratings.Add(rating);
                i++;
                continue;
            }
        } while (true);

        return ratings;
    }

    private List<Post> AddPosts(List<AppUser> users, List<Tag> tags)
    {
        var fakePosts = new Faker<Post>("vi");
        fakePosts.RuleFor(p => p.Title, f => f.Lorem.Sentence());
        fakePosts.RuleFor(p => p.ShortDescription, f => f.Lorem.Paragraph());
        fakePosts.RuleFor(p => p.Description, f => f.Lorem.Paragraphs(3));
        fakePosts.RuleFor(p => p.Meta, f => f.Lorem.Sentence());
        fakePosts.RuleFor(p => p.Tags, f => f.PickRandom(tags, 3).ToList());
        fakePosts.Rules((f, u) =>
        {
            u.UrlSlug = u.Title.GenerateSlug();
        });
        fakePosts.RuleFor(p => p.ViewCount, f => f.Random.Number(0, 1000));
        fakePosts.RuleFor(p => p.Published, f => f.Random.Bool());
        fakePosts.RuleFor(p => p.PostedDate, f => f.Date.Past());
        fakePosts.RuleFor(p => p.ModifiedDate, f => f.Date.Recent());
        fakePosts.RuleFor(p => p.User, f => f.PickRandom(users));

        var posts = fakePosts.Generate(100);

        _dbContext.Posts.AddRange(posts);
        _dbContext.SaveChanges();

        return _dbContext.Posts.ToList();
    }

    private List<UserNotify> AddUserNotify(List<AppUser> users, List<Notification> notifications)
    {
        List<UserNotify> notifies = new List<UserNotify>();
        var fakeUserNofify = new Faker<UserNotify>("vi");
        fakeUserNofify.Rules((f, u) =>
        {
            var user = f.PickRandom(users);
            var notify = f.PickRandom(notifications);

            while (
            _dbContext.UserNotifies
                      .Include(x => x.User)
                      .Include(x => x.Notify)
                      .Any(x => x.UserId == user.Id && x.NotifyId == notify.Id))
            {
                user = f.PickRandom(users);
                notify = f.PickRandom(notifications);
            }

            u.User = user;
            u.Notify = notify;
        });
        fakeUserNofify.RuleFor(u => u.Viewed, f => f.Random.Bool());

        int i = 0;
        do
        {
            if (i >= 100)
                break;

            UserNotify notify = fakeUserNofify.Generate();
            _dbContext.UserNotifies.Add(notify);
            var result = _dbContext.SaveChangesAsync().GetAwaiter().GetResult();

            if (result > 0)
            {
                notifies.Add(notify);
                i++;
                continue;
            }
        } while (true);

        return notifies;
    }

    private List<Notification> AddNotifications(List<NotiLevel> notiLevels)
    {
        var fakeNotify = new Faker<Notification>("vi");
        fakeNotify.RuleFor(u => u.Title, f => f.Lorem.Sentence());
        fakeNotify.RuleFor(u => u.Content, f => f.Lorem.Paragraph());
        fakeNotify.RuleFor(u => u.DueDate, f => f.Date.Between(new DateTime(2021, 5, 10), new DateTime(2023, 5, 10)));
        fakeNotify.RuleFor(u => u.Level, f => f.PickRandom(notiLevels));

        var notifying = fakeNotify.Generate(100);

        _dbContext.Notifications.AddRange(notifying);
        _dbContext.SaveChanges();

        return notifying;
    }

    private List<NotiLevel> AddNotiLevels()
    {
        var notiLevels = new List<NotiLevel>
        {
            new()
            {
                Id = "low",
                LevelName = "Low",
                Priority = (byte)Level.Low,
                Description = "Mức độ ưu tiên này được sử dụng cho các tin nhắn hoặc thông báo không khẩn cấp hoặc quan trọng, chẳng hạn như cập nhật thường kỳ, thông báo chung hoặc thông báo có mức độ ưu tiên thấp."
            },
            new()
            {
                Id = "low-urgency",
                LevelName = "Low Urgency",
                Priority = (byte)Level.Low_Urgency,
                Description = "Mức độ ưu tiên này được sử dụng cho các tin nhắn hoặc thông báo không nhạy cảm về thời gian và có thể được xử lý sau."
            },
            new()
            {
                Id = "minor",
                LevelName = "Minor",
                Priority = (byte)Level.Minor,
                Description = "Mức độ ưu tiên này được sử dụng cho các tin nhắn hoặc thông báo là các sự cố nhỏ hoặc cập nhật không cần chú ý ngay lập tức."
            },
            new()
            {
                Id = "normal",
                LevelName = "Normal Priority",
                Priority = (byte)Level.Normal,
                Description = "Mức độ ưu tiên này được sử dụng cho các tin nhắn hoặc thông báo không cần chú ý ngay lập tức và có thể được xử lý sau."
            },
            new()
            {
                Id = "major",
                LevelName = "Major Priority",
                Priority = (byte)Level.Major,
                Description = "Mức độ ưu tiên này được sử dụng cho các thư hoặc thông báo là các vấn đề hoặc cập nhật chính cần được chú ý nhưng không quan trọng."
            },
            new()
            {
                Id = "important",
                LevelName = "Important Priority",
                Priority = (byte)Level.Important,
                Description = "Mức độ ưu tiên này được sử dụng cho các thư hoặc thông báo quan trọng nhưng không quan trọng, chẳng hạn như thời hạn sắp tới hoặc lời nhắc sự kiện."
            },
            new()
            {
                Id = "public",
                LevelName = "Public Priority",
                Priority = (byte)Level.Public,
                Description = "Mức độ ưu tiên này được sử dụng cho các tin nhắn hoặc thông báo dành cho nhiều đối tượng và không chứa thông tin nhạy cảm hoặc bí mật."
            },
            new()
            {
                Id = "routine",
                LevelName = "Routine Priority",
                Priority = (byte)Level.Routine,
                Description = "Mức độ ưu tiên này được sử dụng cho các tin nhắn hoặc thông báo là một phần của lịch trình hoặc quy trình thông thường, chẳng hạn như báo cáo hàng ngày hoặc cập nhật trạng thái."
            },
            new()
            {
                Id = "high-urgency",
                LevelName = "High Urgency",
                Priority = (byte)Level.High_Urgency,
                Description = "Mức độ ưu tiên này được sử dụng cho các tin nhắn hoặc thông báo cần được chú ý ngay lập tức nhưng không quan trọng, chẳng hạn như yêu cầu cung cấp thông tin hoặc các vấn đề nhỏ."
            },
            new()
            {
                Id = "confidential",
                LevelName = "Confidential Priority",
                Priority = (byte)Level.Confidential,
                Description = "Mức độ ưu tiên này được sử dụng cho các tin nhắn hoặc thông báo chứa thông tin nhạy cảm hoặc bí mật phải được giữ kín."
            },
            new()
            {
                Id = "emergency",
                LevelName = "Emergency Priority",
                Priority = (byte)Level.Emergency,
                Description = "Mức độ ưu tiên này được sử dụng cho các tin nhắn hoặc thông báo cần được chú ý ngay lập tức do tình huống khẩn cấp, chẳng hạn như thiên tai hoặc cấp cứu y tế."
            },
            new()
            {
                Id = "immediate",
                LevelName = "Immediate Priority",
                Priority = (byte)Level.Immediate,
                Description = "Mức độ ưu tiên này được sử dụng cho các tin nhắn hoặc thông báo cần được chú ý ngay lập tức và không thể trì hoãn."
            },
            new()
            {
                Id = "medium",
                LevelName = "Medium Priority",
                Priority = (byte)Level.Medium,
                Description = "Mức độ ưu tiên này được sử dụng cho các thư hoặc thông báo quan trọng nhưng không quan trọng, chẳng hạn như cập nhật, lời nhắc hoặc thông báo thông tin."
            },
            new()
            {
                Id = "top",
                LevelName = "Top Priority",
                Priority = (byte)Level.Top,
                Description = "Mức độ ưu tiên này được sử dụng cho các tin nhắn hoặc thông báo quan trọng nhất cần được chú ý ngay lập tức trên tất cả các tác vụ khác."
            },
            new()
            {
                Id = "high",
                LevelName = "High Priority",
                Priority = (byte)Level.High,
                Description = "Mức độ ưu tiên này được sử dụng cho các thông báo hoặc thông báo quan trọng cần được chú ý ngay lập tức, chẳng hạn như cảnh báo hệ thống, thông báo khẩn cấp hoặc thông báo khẩn cấp."
            },
            new()
            {
                Id = "critical",
                LevelName = "Critical Priority",
                Priority = (byte)Level.Critical,
                Description = "Mức độ ưu tiên này được sử dụng cho các tin nhắn hoặc thông báo có tầm quan trọng tối đa và yêu cầu hành động ngay lập tức để ngăn chặn hậu quả nghiêm trọng."
            },
            new()
            {
                Id = "urgent",
                LevelName = "Urgent Priority",
                Priority = (byte)Level.Urgent,
                Description = "Mức độ ưu tiên này được sử dụng cho các tin nhắn hoặc thông báo nhạy cảm với thời gian và yêu cầu hành động ngay lập tức, chẳng hạn như cảnh báo khẩn cấp, thông báo bảo mật hoặc cập nhật hệ thống quan trọng."
            },
            new()
            {
                Id = "express",
                LevelName = "Express Priority",
                Priority = (byte)Level.Express,
                Description = "Mức độ ưu tiên này được sử dụng cho các yêu cầu khẩn cấp cần được xử lý nhanh nhất có thể, thường là trong vòng vài phút."
            },
            new()
            {
                Id = "critical-urgency",
                LevelName = "Critical Urgency",
                Priority = (byte)Level.Critical_Urgency,
                Description = "Mức độ ưu tiên này được sử dụng cho các vấn đề cần được quan tâm và giải quyết ngay lập tức do mức độ nghiêm trọng hoặc tác động của chúng."
            },
            new()
            {
                Id = "catastrophic",
                LevelName = "Catastrophic Priority",
                Priority = (byte)Level.Catastrophic,
                Description = "Mức độ ưu tiên này được sử dụng cho các tình huống có thể dẫn đến thảm họa lớn, thiệt hại đáng kể về người hoặc tài sản hoặc các hậu quả thảm khốc khác nếu không được giải quyết ngay lập tức."
            },
        };

        _dbContext.NotiLevels.AddRange(notiLevels);
        _dbContext.SaveChanges();

        return notiLevels;
    }

    private List<Comment> AddTopicComments(List<AppUser> users, List<Topic> topics)
    {
        var fakeComments = new Faker<Comment>("vi");
        fakeComments.RuleFor(u => u.Content, f => f.Lorem.Sentence(10, 5));
        fakeComments.RuleFor(u => u.Date, f => f.Date.Between(new DateTime(2021, 5, 10), new DateTime(2023, 5, 10)));
        fakeComments.RuleFor(u => u.User, f => f.PickRandom(users));
        fakeComments.RuleFor(u => u.Topics, f => f.PickRandom(topics, 2).ToList());

        var comments = fakeComments.Generate(50);

        _dbContext.Comments.AddRange(comments);
        _dbContext.SaveChanges();

        return comments;
    }

    private List<Topic> AddTopics(List<TopicRank> topicRanks, List<SpecificAward> specificAwards, List<AppUser> users)
    {
        var fakeTopics = new Faker<Topic>("vi");
        fakeTopics.RuleFor(u => u.TopicName, f => f.Commerce.ProductName());
        fakeTopics.RuleFor(p => p.ShortDescription, f => f.Lorem.Paragraph());
        fakeTopics.RuleFor(p => p.Description, f => f.Lorem.Paragraphs(3));
        fakeTopics.RuleFor(p => p.Leader, f => f.PickRandom(users));
        fakeTopics.RuleFor(p => p.Users, f => f.PickRandom(users, 3).ToList());
        fakeTopics.Rules((f, u) =>
        {
            u.UrlSlug = u.TopicName.GenerateSlug();
        });
        fakeTopics.RuleFor(u => u.RegisDate, f => f.Date.Between(new DateTime(2021, 5, 10), new DateTime(2023, 5, 10)));
        fakeTopics.RuleFor(u => u.TopicRank, f => f.PickRandom(topicRanks));
        fakeTopics.RuleFor(u => u.SpecificAward, f => f.PickRandom(specificAwards));

        var topics = fakeTopics.Generate(100);

        _dbContext.Topics.AddRange(topics);
        _dbContext.SaveChanges();

        return _dbContext.Topics.ToList();
    }

    private List<SpecificAward> AddSpecificAwards(List<RankAward> rankAwards)
    {
        var specificAwards = new List<SpecificAward>
        {
            new()
            {
                BonusPrize = 2000000,
                Year = 2021,
                RankAward = rankAwards[0]
            },
            new()
            {
                BonusPrize = 1500000,
                Year = 2021,
                RankAward = rankAwards[1]
            },
            new()
            {
                BonusPrize = 1000000,
                Year = 2021,
                RankAward = rankAwards[2]
            },
            new()
            {
                BonusPrize = 500000,
                Year = 2021,
                RankAward = rankAwards[3]
            },
            new()
            {
                BonusPrize = 4000000,
                Year = 2021,
                RankAward = rankAwards[4]
            },
            new()
            {
                BonusPrize = 3000000,
                Year = 2021,
                RankAward = rankAwards[5]
            },
            new()
            {
                BonusPrize = 2000000,
                Year = 2021,
                RankAward = rankAwards[6]
            },
            new()
            {
                BonusPrize = 1000000,
                Year = 2021,
                RankAward = rankAwards[7]
            },
            new()
            {
                BonusPrize = 2000000,
                Year = 2022,
                RankAward = rankAwards[0]
            },
            new()
            {
                BonusPrize = 1500000,
                Year = 2022,
                RankAward = rankAwards[1]
            },
            new()
            {
                BonusPrize = 1000000,
                Year = 2022,
                RankAward = rankAwards[2]
            },
            new()
            {
                BonusPrize = 500000,
                Year = 2022,
                RankAward = rankAwards[3]
            },
            new()
            {
                BonusPrize = 4000000,
                Year = 2022,
                RankAward = rankAwards[4]
            },
            new()
            {
                BonusPrize = 3000000,
                Year = 2022,
                RankAward = rankAwards[5]
            },
            new()
            {
                BonusPrize = 2000000,
                Year = 2022,
                RankAward = rankAwards[6]
            },
            new()
            {
                BonusPrize = 1000000,
                Year = 2022,
                RankAward = rankAwards[7]
            },
            new()
            {
                BonusPrize = 2000000,
                Year = 2023,
                RankAward = rankAwards[0]
            },
            new()
            {
                BonusPrize = 1500000,
                Year = 2023,
                RankAward = rankAwards[1]
            },
            new()
            {
                BonusPrize = 1000000,
                Year = 2023,
                RankAward = rankAwards[2]
            },
            new()
            {
                BonusPrize = 500000,
                Year = 2023,
                RankAward = rankAwards[3]
            },
            new()
            {
                BonusPrize = 4000000,
                Year = 2023,
                RankAward = rankAwards[4]
            },
            new()
            {
                BonusPrize = 3000000,
                Year = 2023,
                RankAward = rankAwards[5]
            },
            new()
            {
                BonusPrize = 2000000,
                Year = 2023,
                RankAward = rankAwards[6]
            },
            new()
            {
                BonusPrize = 1000000,
                Year = 2023,
                RankAward = rankAwards[7]
            },
        };

        _dbContext.SpecificAwards.AddRange(specificAwards);
        _dbContext.SaveChanges();

        return _dbContext.SpecificAwards.ToList();
    }

    private List<RankAward> AddRankAwards(List<TopicRank> topicRanks)
    {
        var rankAwards = new List<RankAward>
        {
            new()
            {
                AwardName = "Giải nhất",
                ShortDescription = "Giải thưởng cho đề tài cao điểm nhất hạng mục 1 của cuộc thi sau vòng chung kết",
                TopicRank = topicRanks[0],
                UrlSlug = "giai-nhat-hm1"
            },
            new()
            {
                AwardName = "Giải nhì",
                ShortDescription = "Giải thưởng cho đề tài cao nhì bảng hạng mục 1 của cuộc thi sau vòng chung kết",
                TopicRank = topicRanks[0],
                UrlSlug = "giai-nhi-hm1"
            },
            new()
            {
                AwardName = "Giải ba",
                ShortDescription = "Giải thưởng cho đề tài đứng hạng 3 hạng mục 1 của cuộc thi sau vòng chung kết",
                TopicRank = topicRanks[0],
                UrlSlug = "giai-ba-hm1"
            },
            new()
            {
                AwardName = "Giải khuyến khích",
                ShortDescription = "Giải thưởng cho đề tài có thành tích nổi bật của hạng mục 1",
                TopicRank = topicRanks[0],
                UrlSlug = "giai-kuyen-khich-hm1"
            },
            new()
            {
                AwardName = "Giải nhất",
                ShortDescription = "Giải thưởng cho đề tài cao điểm nhất hạng mục 2 của cuộc thi sau vòng chung kết",
                TopicRank = topicRanks[1],
                UrlSlug = "giai-nhat-hm2"
            },
            new()
            {
                AwardName = "Giải nhì",
                ShortDescription = "Giải thưởng cho đề tài cao nhì bảng hạng mục 2 của cuộc thi sau vòng chung kết",
                TopicRank = topicRanks[1],
                UrlSlug = "giai-nhi-hm2"
            },
            new()
            {
                AwardName = "Giải ba",
                ShortDescription = "Giải thưởng cho đề tài đứng hạng 3 hạng mục 2 của cuộc thi sau vòng chung kết",
                TopicRank = topicRanks[1],
                UrlSlug = "giai-ba-hm2"
            },
            new()
            {
                AwardName = "Giải khuyến khích",
                ShortDescription = "Giải thưởng cho đề tài có thành tích nổi bật của hạng mục 2",
                TopicRank = topicRanks[1],
                UrlSlug = "giai-khuyen-khich-hm2"
            },
        };

        _dbContext.RankAwards.AddRange(rankAwards);
        _dbContext.SaveChanges();

        return _dbContext.RankAwards.ToList();
    }

    private List<TopicRank> AddTopicRanks()
    {
        var topicRanks = new List<TopicRank>
        {
            new() {RankName = "Hạng mục 1", ShortDescription = "Giải thưởng cho sinh viên năm 1, năm 2", UrlSlug = "hang-muc-1" },
            new() {RankName = "Hạng mục 2", ShortDescription = "Giải thưởng cho sinh viên năm 3, năm 4. Đặc biệt sinh viên ở hạng mục 1 cảm thấy có khả năng hoặc kĩ năng tốt có thể tham gia vào hạng mục 2", UrlSlug = "hang-muc-2" },
        };

        _dbContext.TopicRanks.AddRange(topicRanks);
        _dbContext.SaveChanges();

        return _dbContext.TopicRanks.ToList();
    }

    private async Task<List<AppUser>> AddUsers(List<AppUserRole> roles)
    {
        var users = new List<AppUser>();
        var fakeUsers = new Faker<AppUser>("vi");
        fakeUsers.RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber());
        fakeUsers.RuleFor(u => u.PhoneNumberConfirmed, f => true);
        fakeUsers.RuleFor(u => u.EmailConfirmed, f => true);
        fakeUsers.RuleFor(p => p.ImageUrl, f => f.Internet.Avatar());
        fakeUsers.Rules((f, u) =>
        {
            string fullName = f.Name.FullName();
            string email = f.Internet.Email();
            string uniqueUserName = new Bogus.DataSets.Name().LastName().ToLower();

            while (_dbContext.Users.Any(x => x.UserName == uniqueUserName))
            {
                uniqueUserName = new Bogus.DataSets.Name().LastName().ToLower();
            }

            while (_dbContext.Users.Any(x => x.Email == email))
            {
                email = f.Internet.Email();
            }

            u.FullName = fullName;
            u.UserName = uniqueUserName;
            u.UrlSlug = fullName.GenerateSlug();
            u.Email = email;
        });
        fakeUsers.RuleFor(u => u.JoinedDate, f => f.Date.Between(new DateTime(2021, 5, 10), new DateTime(2023, 5, 10)));

        for (int i = 0; i < 10; i++)
        {
            AppUser user = fakeUsers.Generate();

            var resultAdd = await _userManager.CreateAsync(user, "123");  // password: 123
            if (resultAdd.Succeeded)
            {
                var resultAddRole = await _userManager.AddToRoleAsync(user, RoleName.Examiner);
                users.Add(user);
            }
        }

        fakeUsers.RuleFor(u => u.MSSV, f => f.Random.Int(1700000, 3000000).ToString());
        fakeUsers.RuleFor(u => u.GradeName, f => f.PickRandom("CTK44", "CTK45", "CTK46", "CTK47"));

        for (int i = 0; i < 50; i++)
        {
            AppUser user = fakeUsers.Generate();

            try
            {
                var resultAdd = await _userManager.CreateAsync(user, "123");  // password: 123
                //await Task.Delay(500);
                if (resultAdd.Succeeded)
                {
                    var resultAddRole = await _userManager.AddToRoleAsync(user, RoleName.Examinee);
                    users.Add(user);
                }
            }
            catch (Exception ex)
            {
                foreach (var item in _dbContext.ChangeTracker.Entries())
                {
                    Console.WriteLine(item.ToString());
                }

                await Console.Out.WriteLineAsync(ex.Message);
            }
        }

        return users;
    }

    private async Task AddDefaultUsers()
    {
        var userAdmin = await _userManager.FindByEmailAsync("admin@example.com");
        if (userAdmin == null)
        {
            userAdmin = new AppUser
            {
                UserName = "ngaptt",
                Email = "admin@example.com",
                EmailConfirmed = true,
                FullName = "Phan Thị Thanh Nga",
                UrlSlug = "ngaptt",
                JoinedDate = DateTime.Now,
            };

            await _userManager.CreateAsync(userAdmin, "123");  // password: 123
            await _userManager.AddToRoleAsync(userAdmin, RoleName.Administrator);
        }
    }

    private async Task<List<AppUserRole>> AddRoles()
    {
        var roleNames = typeof(RoleName).GetFields().ToList();
        var roles = new List<AppUserRole>();
        foreach (var role in roleNames)
        {
            var roleName = role.GetRawConstantValue()?.ToString();
            var roleFound = await _roleManager.FindByNameAsync(roleName);
            if (roleFound == null)
            {
                var newRole = new AppUserRole(roleName);
                await _roleManager.CreateAsync(newRole);
                roles.Add(newRole);
            }
            else
            {
                roles.Add(roleFound);
            }
        }

        return roles;
    }
}

public static class StringExtensions
{
    public static IEnumerable<string> SplitCamelCase(this string input)
    {
        return Regex.Split(input, @"([A-Z]?[a-z]+)").Where(str => !string.IsNullOrEmpty(str));
    }

    public static string FirstCharUppercase(this string input)
    {
        return $"{char.ToUpper(input[0])}{input.Substring(1)}";
    }

    public static string GenerateSlug(this string slug)
    {
        slug = RemoveAccents(slug);
        var splitToValidFormat = slug.Split(new[] { " ", ",", ";", ".", "-", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < splitToValidFormat.Length; i++)
        {
            splitToValidFormat[i] = splitToValidFormat[i].FirstCharUppercase();
        }
        var refixAlphabet = splitToValidFormat;
        var slugFormat = string.Join("", refixAlphabet);
        var reflectionSlug = String.Join("-", slugFormat.SplitCamelCase());

        return reflectionSlug.ToLower();
    }

    public static string RemoveAccents(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        text = text.Replace("Đ", "D").Replace("Ð", "D").Replace("đ", "d").Replace("đ", "d");
        text = text.Normalize(NormalizationForm.FormD);
        char[] chars = text
            .Where(c => CharUnicodeInfo.GetUnicodeCategory(c)
            != UnicodeCategory.NonSpacingMark).ToArray();

        return new string(chars).Normalize(NormalizationForm.FormC);
    }
}