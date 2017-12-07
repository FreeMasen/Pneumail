namespace Pneumail.Models
{
    public class EmailAddress
    {
        public System.Guid Id { get; set; }
        public string Username { get; set; }
        public string Host { get; set; }
        public string Domain { get; set; }
        public string Name { get; set; }

        public EmailAddress()
        {

        }

        public EmailAddress(string address, string name = null) {
            var split = address.Split('@');
            Username = split[0];
            var secondSplit = split[1].Split('.');
            Host = secondSplit[0];
            Domain = secondSplit[1];
            Name = name;
        }

        public override string ToString() {
            return $"{Username}@{Host}.{Domain}";
        }
    }
}