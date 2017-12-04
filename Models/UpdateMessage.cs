using System;
using System.Collections.Generic;

namespace Pneumail.Models
{
    public class UpdateMessage
    {
        public UpdateType UpdateType { get; set; }
        public List<Category> Categories = new List<Category>();
    }

    [Flags]
    public enum UpdateType
    {
        None = 0,
        Initial = 1,
        Insert = 2,
        Delete = 4,
        Modify = 8
    }
}