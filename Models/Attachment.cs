namespace Pneumail.Models
{
    public class Attachment
    {
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        public System.Guid MessageId { get; set; }
        public Message Message { get; set; }
        public Attachment()
        {

        }
    }
}