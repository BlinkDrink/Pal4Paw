namespace DogCarePlatform.Web.ViewModels.Administration
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using DogCarePlatform.Services.Mapping;
    using DogCarePlatform.Web.ViewModels.Dogsitter;

    public class DogsitterApplicationViewModel : IMapFrom<DogsitterApplicationInputModel>
    {
        public string Question1 { get; set; }

        public string Question2 { get; set; }

        public string Question3 { get; set; }

        public string Question4 { get; set; }

        public string Question5 { get; set; }

        public string Question6 { get; set; }

        public string Question7 { get; set; }

        public string Question8 { get; set; }

        public string Question9 { get; set; }
    }
}
