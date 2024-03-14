using TaskMate.Entities.Common;

namespace TaskMate.Entities;

public class Comment:BaseEntity
{
    /// <summary>
    /// Demeli Nedi: User ozu yazdigi commmenti Edit ve ya Delete ede biler.
    /// Admin Userin Yaratdigi Commenti sile biler.
    /// </summary>
    public string Message { get; set; }
    public DateTime CreateComment { get; set; } = DateTime.Now;
    public AppUser AppUser { get; set; }
    public string AppUserId { get; set; }
    public Card Card { get; set; }
    public Guid CardId { get; set; }
}
