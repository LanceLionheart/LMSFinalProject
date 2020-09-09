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
        [Required(ErrorMessage = "* Course Name is Required")]
        public string CourseName { get; set; }

        [Display(Name = "Course Description")]
        [StringLength(500, ErrorMessage = "* Major must be 500 characters or less.")]
        [DisplayFormat(NullDisplayText = "[-N/A-]")]
        public string CourseDescription { get; set; }

        [Display(Name = "Active")]
        [Required(ErrorMessage = "* Required")]
        public bool IsActive { get; set; }
    }
    #endregion

    #region Course Completion Metadata
    public class CourseCompletionMetadata
    {
        //public int CourseCompletionId { get; set; }
        //public string UserId { get; set; }
        //public int CourseId { get; set; }

        [Display(Name = "Completion Date")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "* Completion Date is required")]
        public System.DateTime DateCompleted { get; set; }
    }
    #endregion

    #region Lesson Metadata
    public class LessonMetadata
    {
        //public int LessonId { get; set; }

        [Display(Name = "Lesson Title")]
        [StringLength(200, ErrorMessage = "* Lesson Title must be 200 characters or less.")]
        [Required(ErrorMessage = "* Lesson Title is Required")]
        public string LessonTitle { get; set; }

        //public int CourseId { get; set; }

        [Display(Name = "Introduction")]
        [StringLength(300, ErrorMessage = "* Introduction must be 300 characters or less.")]
        [DisplayFormat(NullDisplayText = "[-N/A-]")]
        public string Introduction { get; set; }

        [Display(Name = "Video URL")]
        [StringLength(250, ErrorMessage = "* Introduction must be 250 characters or less.")]
        [DisplayFormat(NullDisplayText = "[-N/A-]")]
        public string VideoURL { get; set; }

        [Display(Name = "PDF File Name")]
        [StringLength(100, ErrorMessage = "* Introduction must be 100 characters or less.")]
        [DisplayFormat(NullDisplayText = "[-N/A-]")]
        public string PdfFilename { get; set; }

        [Display(Name = "Active")]
        [Required(ErrorMessage = "* Required")]
        public bool IsActive { get; set; }
    }
    #endregion

    #region Lesson View Metadata
    public class LessonViewMetadata
    {
        //public int LessonViewId { get; set; }
        //public string UserId { get; set; }
        //public int LessonId { get; set; }

        [Display(Name = "Date Viewed")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "* Date Viewed is required")]
        public System.DateTime DateViewed { get; set; }
    }
    #endregion

    #region User Detail Metadata
    public class UserDetailMetadata
    {
        //public string UserId { get; set; }

        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "* First Name must be 50 characters or less.")]
        [Required(ErrorMessage = "* First Name is required")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "* Last Name must be 50 characters or less.")]
        [Required(ErrorMessage = "* Last Name is required")]
        public string LastName { get; set; }
    }
    #endregion

}
