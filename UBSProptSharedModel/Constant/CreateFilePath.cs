using System;
using System.IO;

namespace UBSProptSharedModel.Constant
{
   
    public class CreateFileHelper
    {
       static public string CreateTempFile(byte[] data, string fileName)
        {
            // Ensure we got some data
            if (data == null || data.Length == 0)
                throw new ArgumentException("File data cannot be empty.");

            // Get OS temp folder path
            string tempPath = Path.GetTempPath();

            // Combine with filename
            string filePath = Path.Combine(tempPath, fileName);

            // Write the file
            File.WriteAllBytes(filePath, data);

            // Return full path
            return filePath;
        }
        static public byte[] DataUrlToBytes(string dataUrl)
        {
            // Example: "data:image/png;base64,...."
            var base64Data = dataUrl.Substring(dataUrl.IndexOf(",") + 1);
            return Convert.FromBase64String(base64Data);
        }
    }

}

