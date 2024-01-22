namespace CaseMix
{
    public class CaseMixConsts
    {
        public const string LocalizationSourceName = "CaseMix";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = false;

        public const int PasswordMinLength = 8;

        public const string PasswordSpecialChars = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";

        public const string PasswordRegexValidator = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[" + PasswordSpecialChars + @"])[A-Za-z\d" + PasswordSpecialChars + "]{8,32}$";

        //@TODO: localize this value, currently not supported on Models or DTOs
        public const string InvalidPasswordValidationMessage = "Password must contain at 1 uppercase letter, 1 lowercase letter, 1 number and 1 special character: " + PasswordSpecialChars;
    }
}
