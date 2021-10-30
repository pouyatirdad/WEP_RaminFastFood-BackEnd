using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StorePanel.Core.Models
{
    public interface IBaseEntity
    {
        int Id { get; set; }
        DateTime InsertDate { get; set; }
        DateTime? UpdateDate { get; set; }
        [MaxLength(100)]
        string InsertUser { get; set; }
        [MaxLength(100)]
        string UpdateUser { get; set; }
        bool IsDeleted { get; set; }
    }
    public class BaseEntity : IBaseEntity
    {
        public int Id { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string InsertUser { get; set; }
        public string UpdateUser { get; set; }
        public bool IsDeleted { get; set; }
    }
}
