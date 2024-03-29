﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.Database.Entity
{
    public class UserActivity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public virtual User? User { get; set; }
        public string? Action { get; set; }
        public string? AdditionalData { get; set; }
    }
}