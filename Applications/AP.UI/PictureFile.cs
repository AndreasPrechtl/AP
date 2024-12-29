namespace AP.UI
{
    public class PictureFile : AP.IO.File
    {
        public PictureFile(string fullName)
            : base(fullName)
        { }

        public string Thumbnail { get; set; }
    }
}
