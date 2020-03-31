using System.ComponentModel.DataAnnotations;

namespace DogCarePlatform.Data.Models
{
    public enum ApplicationStatus
    {
        [Display(Name ="Подадена заявка")]
        Submitted = 1,

        [Display(Name = "Удобрен")]
        Approved = 2,

        [Display(Name = "Отхвърлен")]
        Rejected = 3,
    }
}