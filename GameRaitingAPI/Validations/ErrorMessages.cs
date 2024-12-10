namespace GameRaitingAPI.Validations
{
    public static class ErrorMessages
    {
        public static string FieldIsRequired = "The {PropertyName} field is required";
        public static string MaximumLength = "The {PropertyName} field must be less than {MaxLength} characters";
        public static string FirstCapitalLetter = "The first letter must be capitalized";
        public static string GenreAlreadyExist = "Genre already exist";
        public static string IncorrectEmailFormat = "{PropertyName} must be valid";


    }
}
