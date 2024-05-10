using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Lesson
    {
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

        [Range(1, 10)]
        public int Duration { get; set; }

    }
}