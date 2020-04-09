namespace DogCarePlatform.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public enum AppointmentStatus
    {
        [Display(Name = "Предстояща")]
        Unprocessed = 1,

        [Display(Name = "В процес")]
        Happening = 2,

        [Display(Name = "Обработена")]
        Processed = 3,
    }
}
