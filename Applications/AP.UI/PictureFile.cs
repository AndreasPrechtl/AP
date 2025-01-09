namespace AP.UI
{
    public class PictureFile(string fullName) : AP.IO.File(fullName)
    {
        public string? Thumbnail { get; init; } = null;
    }
}
