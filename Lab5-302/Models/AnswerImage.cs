using System.ComponentModel.DataAnnotations;

namespace Lab5_302.Models
{
    public enum Question
    {
        Earth, Computer
    }
    public class AnswerImage
    {

        public int AnswerImageId { get; set; }

        [Display(Name = "File Name")]
        [StringLength(50, MinimumLength = 1)]
        [Required]
        public string FileName { get; set; }

        [Display(Name = "Url")]
        [StringLength(500, MinimumLength = 1)]
        [Required]
        public string Url { get; set; }

        [Display(Name = "Question")]
        [Required]
        public Question Question { get; set; }
    }
}
