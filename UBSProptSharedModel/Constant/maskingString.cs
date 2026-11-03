using System;
namespace UBSProptSharedModel.Constant
{
    public static class StringMaskingHelper
    {
        public static string MaskAccountNumber(string input, int visibleStart = 4, int visibleEnd = 4, char maskChar = '*')
        {
            if (string.IsNullOrEmpty(input) || input.Length <= visibleStart + visibleEnd)
                return input;

            int maskLength = input.Length - visibleStart - visibleEnd;
            string maskedSection = new string(maskChar, maskLength);

            return input.Substring(0, visibleStart) + maskedSection + input.Substring(input.Length - visibleEnd);
        }
    }
}

