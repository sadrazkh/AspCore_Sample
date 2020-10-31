using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLayer.Entity.Select_2_Test
{
    public class Lesson
    {
        [Key]
        public long Id { get; set; }
        public string Title { get; set; }


        [ForeignKey("ParentId")]
        public Lesson? LessonChild { get; set; }
        public long? ParentId { get; set; }

    }
}
