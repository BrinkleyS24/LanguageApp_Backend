using System;

namespace backend.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; } 
        public int LessonId { get; set; } 
        public Lesson? Lesson { get; set; } 
    }

}