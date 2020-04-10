namespace DogCarePlatform.Web.ViewModels.Dogsitter
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class DogsitterApplicationInputModel
    {
        [Required(ErrorMessage = "Моля попълнете полето")]
        [StringLength(500, MinimumLength = 50, ErrorMessage = "Полето трябва да съдържа между 50 до 500 символа")]
        [RegularExpression("^[а-я А-Я 0-9_.,-]*$", ErrorMessage ="Моля пишете на кирилица")]
        [DisplayName("Въпрос 1")]
        public string Question1 { get; set; }

        [Required(ErrorMessage = "Моля попълнете полето")]
        [StringLength(500, MinimumLength = 50, ErrorMessage = "Полето трябва да съдържа между 50 до 500 символа")]
        [RegularExpression("^[а-я А-Я 0-9_.,-]*$", ErrorMessage = "Моля пишете на кирилица")]
        [DisplayName("Въпрос 2")]
        public string Question2 { get; set; }

        [Required(ErrorMessage = "Моля попълнете полето")]
        [StringLength(500, MinimumLength = 50, ErrorMessage = "Полето трябва да съдържа между 50 до 500 символа")]
        [RegularExpression("^[а-я А-Я 0-9_.,-]*$", ErrorMessage = "Моля пишете на кирилица")]
        [DisplayName("Въпрос 3")]
        public string Question3 { get; set; }

        [Required(ErrorMessage = "Моля попълнете полето")]
        [StringLength(500, MinimumLength = 50, ErrorMessage = "Полето трябва да съдържа между 50 до 500 символа")]
        [RegularExpression("^[а-я А-Я 0-9_.,-]*$", ErrorMessage = "Моля пишете на кирилица")]
        [DisplayName("Въпрос 4")]
        public string Question4 { get; set; }

        [Required(ErrorMessage = "Моля попълнете полето")]
        [StringLength(500, MinimumLength = 50, ErrorMessage = "Полето трябва да съдържа между 50 до 500 символа")]
        [RegularExpression("^[а-я А-Я 0-9_.,-]*$", ErrorMessage = "Моля пишете на кирилица")]
        [DisplayName("Въпрос 5")]
        public string Question5 { get; set; }

        [Required(ErrorMessage = "Моля попълнете полето")]
        [StringLength(500, MinimumLength = 50, ErrorMessage = "Полето трябва да съдържа между 50 до 500 символа")]
        [RegularExpression("^[а-я А-Я 0-9_.,-]*$", ErrorMessage = "Моля пишете на кирилица")]
        [DisplayName("Въпрос 6")]
        public string Question6 { get; set; }

        [Required(ErrorMessage = "Моля попълнете полето")]
        [StringLength(500, MinimumLength = 50, ErrorMessage = "Полето трябва да съдържа между 50 до 500 символа")]
        [RegularExpression("^[а-я А-Я 0-9_.,-]*$", ErrorMessage = "Моля пишете на кирилица")]
        [DisplayName("Въпрос 7")]
        public string Question7 { get; set; }

        [Required(ErrorMessage = "Моля попълнете полето")]
        [StringLength(500, MinimumLength = 50, ErrorMessage = "Полето трябва да съдържа между 50 до 500 символа")]
        [RegularExpression("^[а-я А-Я 0-9_.,-]*$", ErrorMessage = "Моля пишете на кирилица")]
        [DisplayName("Въпрос 8")]
        public string Question8 { get; set; }

        [Required(ErrorMessage = "Моля попълнете полето")]
        [StringLength(500, MinimumLength = 50, ErrorMessage = "Полето трябва да съдържа между 50 до 500 символа")]
        [RegularExpression("^[а-я А-Я 0-9_.,-]*$", ErrorMessage = "Моля пишете на кирилица")]
        [DisplayName("Въпрос 9")]
        public string Question9 { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
