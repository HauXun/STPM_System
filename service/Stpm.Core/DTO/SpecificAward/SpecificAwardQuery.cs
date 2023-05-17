namespace Stpm.Core.DTO.SpecificAward;

public partial class SpecificAwardQuery
{
    public string Keyword { get; set; }
    public int? BonusPrize { get; set; }
    public short? Year { get; set; }
    public bool? Passed { get; set; }
    public string RankAwardSlug { get; set; }
    public string TopicSlug { get; set; }
    public int? TopicId { get; set; }
    public int? RankAwardId { get; set; }
}
