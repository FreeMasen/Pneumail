using System;
using System.Collections.Generic;

namespace Pneumail.Models
{
    public class UpdateMessage
    {
        public UpdateType UpdateType
        {
            get
            {
                UpdateType ret = UpdateType.None;
                if (this.Initial.Count > 0)
                {
                    ret |= UpdateType.Initial;
                }
                if (this.Deletes.Count > 0)
                {
                    ret |= UpdateType.Delete;
                }
                if (this.Modifies.Count > 0)
                {
                    ret |= UpdateType.Modify;
                }
                if (this.Inserts.Count > 0)
                {
                    ret |= UpdateType.Insert;
                }
                return ret;
            }
        }
        public List<Update> Initial = new List<Update>();
        public List<Update> Deletes = new List<Update>();
        public List<Update> Inserts = new List<Update>();
        public List<Update> Modifies = new List<Update>();
    }

    public class Update
    {
        public List<string> KeysModified { get; set; }
        public List<Category> Categories { get; set; }
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