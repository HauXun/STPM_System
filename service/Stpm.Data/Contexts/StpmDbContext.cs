using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Stpm.Core.Entities;
using System.Reflection;

namespace Stpm.Data.Contexts;

public partial class StpmDbContext : IdentityDbContext<AppUser, AppUserRole, int>
{
    public StpmDbContext()
    {
    }

    public StpmDbContext(DbContextOptions<StpmDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<NotiLevel> NotiLevels { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<PostPhoto> PostPhotos { get; set; }

    public virtual DbSet<PostVideo> PostVideos { get; set; }

    public virtual DbSet<RankAward> RankAwards { get; set; }

    public virtual DbSet<SpecificAward> SpecificAwards { get; set; }

    public virtual DbSet<Topic> Topics { get; set; }

    public virtual DbSet<TopicPhoto> TopicPhotos { get; set; }

    public virtual DbSet<TopicVideo> TopicVideos { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<TopicHistoryAward> TopicHistoryAwards { get; set; }

    public virtual DbSet<TopicRank> TopicRanks { get; set; }

    public virtual DbSet<UserNotify> UserNotifies { get; set; }

    public virtual DbSet<NotifyAttachment> NotifyAttachments { get; set; }

    public virtual DbSet<Timeline> Timelines { get; set; }

    public virtual DbSet<ProjectTimeline> ProjectTimelines { get; set; }

    public virtual DbSet<UserTopicRating> UserTopicRatings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      => optionsBuilder.UseSqlServer("Server=(local)\\SQLEXPRESS;Database=Stpm_Admin;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //modelBuilder.ApplyConfiguration(new UserNotifyMap());
        //modelBuilder.ApplyConfiguration(new UserTopicRatingMap());
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Topic).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EntityConfigurationsAssembly).Assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (tableName.StartsWith("AspNet"))
            {
                entityType.SetTableName(tableName.Substring(6));
            }
        }
    }

    // Assembly containing entity configurations
    public class EntityConfigurationsAssembly : Assembly
    {
        // This constructor is used to load the assembly at runtime
        public EntityConfigurationsAssembly() { }

        // Override the GetTypes() method to return all types in the assembly that inherit from IEntityTypeConfiguration<T>
        public override Type[] GetTypes()
        {
            return GetExportedTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
                .ToArray();
        }
    }
}
