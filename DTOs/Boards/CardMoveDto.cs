public class CardMoveDto
{
    public Guid CardId { get; set; }
    public Guid SourceColumnId { get; set; }
    public Guid DestinationColumnId { get; set; }
    public int NewIndex { get; set; }
}
