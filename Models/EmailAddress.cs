namespace Pneumail.Models
{
    public class EmailAddress
    {
        public System.Guid Id { get; set; }
        public string Username { get; set; }
        public string Host { get; set; }
        public string Domain { get; set; }

        public EmailAddress()
        {

        }

        public override string ToString() {
            return $"{Username}@{Host}.{Domain}";
        }
    }
}