using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Entity.Select_2_Test;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Asp_Sample.Models
{
    public class LessonViewModel
    {
        public Lesson Lesson { get; set; }
        public long[] Lessons { get; set; }

    }
}
