﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
        [Required]
        public DateTime UpdateTime { get; set; }
        public bool IsDeleted { get; set; }

        public BaseEntity()
        {
            CreationTime = DateTime.Now;
            UpdateTime = DateTime.Now;
            IsDeleted = false;
        }
    }
}