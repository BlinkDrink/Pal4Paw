// ReSharper disable VirtualMemberCallInConstructor
using DogCarePlatform.Data.Common.Models;

namespace DogCarePlatform.Data.Models
{
    public class QuestionAnswer : BaseModel<int>
    {
        public string Question { get; set; }

        public string Answer { get; set; }
    }
}