namespace Stpm.Core.DTO.SpecificAward;

public partial class SpecificAwardItem
{
    public int Id { get; set; }
    public int BonusPrize { get; set; }
    public short Year { get; set; }
    public bool Passed { get; set; }
    public string RankAwardName { get; set; }
    public int TopicCount { get; set; }
}
