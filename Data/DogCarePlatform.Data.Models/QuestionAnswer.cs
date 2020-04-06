// ReSharper disable VirtualMemberCallInConstructor
using DogCarePlatform.Data.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace DogCarePlatform.Data.Models
{
    public class QuestionAnswer : BaseDeletableModel<int>
    {
        public string Question { get; set; }

        public string Answer { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}