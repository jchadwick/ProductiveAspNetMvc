namespace HPlusSports.Models
{
    public class Image
    {
        public long Id { get; set; }

        public string Url { get; set; }

        public byte[] Content { get; set; }

        public string ContentType { get; set; }
    }
}