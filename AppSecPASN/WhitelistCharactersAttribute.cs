using System.ComponentModel.DataAnnotations;

namespace AppSecPASN
{
    public class WhitelistCharactersAttribute : ValidationAttribute
    {
        private readonly string characters;

        public WhitelistCharactersAttribute(string characters)
        {
            this.characters = characters;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var strValue = value as string;

            if (strValue != null)
            {
                foreach (var ch in strValue)
                {
                    if (!characters.Contains(ch))
                    {
                        return new ValidationResult($"Invalid character '{ch}' in {validationContext.DisplayName}.");
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}
