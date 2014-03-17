using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wazzap.Models
{
    public class ChatMessage
    {
        public int ID { get; set; }

        [Required]
        public User user { get; set; }

        [Required]
        public string message { get; set; }

        public DateTime timestamp { get; set; }
    }
}