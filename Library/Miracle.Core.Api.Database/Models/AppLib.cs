using System;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Core.Api.Database.Models
{
    public class AppLib
    {
        [Key]
        public int Id { get; set; }
        public string LibName { get; set; }
        private bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; LastChangeDate = DateTime.UtcNow; }
        }
        public DateTime PublishDate { get; set; }
        public DateTime LastChangeDate { get; set; }
        public bool IsLoaded { get; set; }

        public AppLib()
        {
            var date = DateTime.UtcNow;
            PublishDate = date;
            LastChangeDate = date;
        }
    }
}
