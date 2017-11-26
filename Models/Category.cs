using System;
using System.Collections.Generic;

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
        public Category()
        {

        }
    }
}