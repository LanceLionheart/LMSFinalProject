using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LMSFinalProject.DATA.EF.Metadata
{
    #region Course Metadata
    public class CourseMetadata
    {
        //public int CourseId { get; set; }

        [Display(Name = "Course Name")]
        [StringLength(200, ErrorMessage = "* Material must be 200 characters or less.")]
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public bool IsActive { get; set; }

    }
    #endregion

}
