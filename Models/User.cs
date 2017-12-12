using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Pneumail.Models
{
    /// <summary>
    /// A user of this application
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// The email service providers he/she has routed
        /// through the application
        /// </summary>
        public List<EmailService> Services { get; set; }
        /// <summary>
        /// The list of categories that messages have 
        /// been sorted into
        /// </summary>
        public List<Category> Categories { get; set; }
        public List<Rule> Rules { get; set; }
    }
}