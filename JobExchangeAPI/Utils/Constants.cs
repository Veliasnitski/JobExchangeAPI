namespace JobExchangeAPI.Utils
{
    public class Constants
    {
        public const string TokenKey = "efgt453rdfg45sdflhsjkldgjgh847t5huwerhlsdhlghssdfg"; 
    }

    public class LengthFields
    {
        public static int minPasswordLen = 8;   
    }

    public class ErrorMessages
    {
        public static readonly string MinPasswordLen = $"Minimum password length should be {LengthFields.minPasswordLen}";
        public static readonly string PasswordAlphanumeric = "Password should be Alphanumeric";
        public static readonly string PasswordSpecChars = "Password should contain chars";
    }

    public class UserRole
    {
        public const string User = "User";
        public const string Admin = "Admin";
    }

    public class ExpireTime
    {
        public const int Token15 = 15;
    }
}
