using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CopilotAgentCodeReviewTest
{
    internal class Program
    {
        // Properties for BMI calculation - should be provided by user input or parameters
        public static float Height { get; set; }
        public static float Weight { get; set; }

        static void Main(string[] args)
        {
        }

        public static float CalculateBMI(float weight, float height)
        {
            if (height <= 0 || weight <= 0)
            {
                throw new ArgumentException("Height and weight must be positive values");
            }

            float heightInMeters = height / 100;
            float result = weight / (heightInMeters * heightInMeters);
            return result;
        }

        public static void Save(string userInputFileName)
        {
            // Password should come from secure configuration, not hardcoded
            string password = System.Configuration.ConfigurationManager.AppSettings["SecurePassword"] ?? "";
            
            // Use cryptographically secure random number generator
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] tokenBytes = new byte[4];
                rng.GetBytes(tokenBytes);
                int secureToken = BitConverter.ToInt32(tokenBytes, 0);
                Console.WriteLine(Math.Abs(secureToken));
            }

            // Sanitize filename to prevent path traversal attacks
            string sanitizedFileName = Path.GetFileName(userInputFileName);
            if (string.IsNullOrEmpty(sanitizedFileName) || 
                sanitizedFileName.Contains("..") || 
                Path.GetInvalidFileNameChars().Any(c => sanitizedFileName.Contains(c)))
            {
                throw new ArgumentException("Invalid file name provided");
            }

            string baseDirectory = @"D:\some\directory\";
            string filePath = Path.Combine(baseDirectory, sanitizedFileName);
            
            // Ensure the file path is within the expected directory
            string fullPath = Path.GetFullPath(filePath);
            string fullBaseDirectory = Path.GetFullPath(baseDirectory);
            if (!fullPath.StartsWith(fullBaseDirectory))
            {
                throw new ArgumentException("Access to path outside base directory is not allowed");
            }

            if (File.Exists(filePath))
            {
                File.ReadAllText(filePath);
            }
        }

        public static DataTable GetUserData(string userInput, SqlConnection connection)
        {
            DataTable dt = new DataTable();
            
            // Use parameterized query to prevent SQL injection
            string query = "SELECT * FROM any.USERS WHERE user_name = @username";
            
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@username", userInput);
                
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            
            return dt;
        }
    }
}
