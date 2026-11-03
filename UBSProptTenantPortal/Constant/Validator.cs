using System.Globalization;
using System.Text.RegularExpressions;

public static class Validator
{
    private static readonly Regex EmailRegex = new Regex(
        @"^[^\s@]+@[^\s@]+\.[^\s@]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public static bool IsValidEmail(string email)
    {
        return EmailRegex.IsMatch(email);
    }

    public static string FormatRentAmount(string rentString)
    {
        // Try to parse the string to a decimal
        if (decimal.TryParse(rentString, out decimal rentAmount))
        {
            // Format and return the amount
            return $"AED {rentAmount.ToString("N2", CultureInfo.InvariantCulture)}";
        }
        else
        {
            return "Invalid Amount"; // Handle invalid input
        }
    }
}