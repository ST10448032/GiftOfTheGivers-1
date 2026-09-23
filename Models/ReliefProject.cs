using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Models
{
    public class ReliefProject
    {
        public int ReliefProjectID { get; set; }

        [Required]
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Project Update")]
        public string Update { get; set; } = string.Empty;

        [Display(Name = "Update Date")]
        public DateTime UpdateDate { get; set; } = DateTime.Now;
    }
}
