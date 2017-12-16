using System;
using System.Collections.Generic;
using System.Linq;
namespace Pneumail.Models
{
    /// <summary>
    /// A category represents a collection of messages for a user.
    /// A common category would be for new messages or messages
    /// about travel
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Unique ID for a Category
        /// </summary>
        /// <returns></returns>
        public Guid Id { get; set; }
        /// <summary>
        /// The user defined name of this category
        /// </summary>
        /// <returns></returns>
        public string Name { get; set; }
        /// <summary>
        /// The collection of messages in this category
        /// </summary>
        /// <returns></returns>
        public List<Message> Messages { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
        public Category()
        {

        }

        public string ToJson()
        {
            var jb = new System.Text.StringBuilder();
            jb.Append("{");
            jb.Append("\"id:\"");
            jb.Append(Id.ToString());
            jb.Append("\",\"name\":\"");
            jb.Append(Name);
            jb.Append("\",\"messages\":[");
            for (var i = 0; i < Messages.Count();i++)
            {
                var msg = Messages[i];
                jb.Append(msg.ToJson());
                if (i < Messages.Count() -1)
                {
                    jb.Append(",");
                }
            }
            jb.Append("]}");
            return jb.ToString();
        }
    }
}