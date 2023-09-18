namespace AstroCreate.Game;

public class NoteInfo
{
    public enum NoteType
    {
        BREAK,
        EACH,
        SINGLE
    }

    public NoteType Type { get; set; } = NoteType.SINGLE;
}
