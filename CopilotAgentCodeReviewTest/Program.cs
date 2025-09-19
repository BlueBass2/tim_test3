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
        // Properties needed for CalculateBMI method
        public static float Height { get; set; } = 175; // Default height in cm
        public static float Weight { get; set; } = 70;  // Default weight in kg

        static void Main(string[] args)
        {
        }

        public static float CalculateBMI()
        {
            float result = 0;

            float height = Height / 100f; // Fix: Use floating-point division
            result = Weight / (height * height);
            // Fix: Removed division by zero that caused runtime exception

            return result;
        }

        public static void Save(string userInputFileName)
        {
            // Fix: Load password from environment variable instead of hardcoded
            string password = Environment.GetEnvironmentVariable("MYAPP_PASSWORD");
            
            // Fix: Use cryptographically secure random number generator
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] bytes = new byte[4];
                rng.GetBytes(bytes);
                int secureToken = BitConverter.ToInt32(bytes, 0);
                // Fix: Remove console logging of sensitive token
                // Console.WriteLine(secureToken); // Removed to prevent token leakage
            }

            // Fix: Use Path.Combine with validation to prevent path traversal
            var baseDirectory = @"D:\some\directory";
            var sanitizedFileName = Path.GetFileName(userInputFileName); // Remove any path traversal attempts
            string filePath = Path.Combine(baseDirectory, sanitizedFileName);
            
            // Fix: Add TODO comment for file processing or implement proper handling
            // TODO: Process the contents of the file as needed.
            if (File.Exists(filePath))
            {
                File.ReadAllText(filePath);
            }
        }

        public static DataTable GetUserData(string userInput, SqlConnection connection)
        {
            DataTable dt = new DataTable();
            // Fix: Use parameterized query to prevent SQL injection
            // Fix: Use specific column selection instead of SELECT *
            string query = "select user_name from any.USERS where user_name = @userName";
            
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                // Fix: Add parameter to prevent SQL injection
                cmd.Parameters.Add("@userName", SqlDbType.NVarChar).Value = userInput;
                
                // Fix: Actually execute the command and fill the DataTable
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }
    }
}
