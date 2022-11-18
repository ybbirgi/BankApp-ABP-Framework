namespace BankApp.Customers;

public class CustomerConstants
{
    public static int IdentityNumberLenght { get; set; } = 11;
    public static int NameLenght { get; set; } = 50;
    public static int LastNameLenght { get; set; } = 50;
    public static float DefaultRiskLimit { get; set; } = 10000;
    public static float DefaultRemainingRiskLimit { get; set; } = DefaultRiskLimit;
    public static bool CustomerStatus { get; set; } = true;
}