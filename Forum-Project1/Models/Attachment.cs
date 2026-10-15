namespace Forum_Project1.Models
{
    public class Attachment
    {
        public int Id { get; set; }
        public Post Post { get; set; }
        public int PostId { get; set; }
        public string FileUrl { get; set; }
        public string FileType { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
