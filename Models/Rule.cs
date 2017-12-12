using System;
namespace Pneumail.Models
{
    public class Rule
    {
        public System.Guid Id { get; set; }
        public string SearchTerm { get; set; }
        public SearchLocation Location { get; set; }
        public Rule()
        {

        }
    }
    [Flags]
    public enum SearchLocation
    {
        Subject = 1,
        Body = 2,
        From = 4,
        To = 8,
    }
}