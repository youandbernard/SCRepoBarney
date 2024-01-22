using System.ComponentModel.DataAnnotations;

namespace CaseMix.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}