
namespace Microsoft.Content.Build.Java2Yaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ExceptionUtility
    {
        private const string prefix = "##vso[task.logissue type=error;]java2yaml";

        public static void ThrowExceptionForAzureDevops(string fileName, string commandLineArgs, StringBuilder output, StringBuilder error)
        {
            string message = "";

            switch (fileName.Trim())
            {
                case "cmd.exe":
                    // cmd.exe will invoke mvn command, which writes everything into output, we need extract lines. 
                    message = GetMVNErrorFromOutput(output);
                    break;
                case "javadoc.exe":
                    message = $"{output.ToString()}. Error: {error.ToString()}.";
                    break;
            }

            // for Azure DevOps to pick up excpetion
            Console.WriteLine($"{prefix} '\"{fileName}\" {commandLineArgs}' failed. {ReplaceWithSpace(message)}");
        }

        public static void ThrowExceptionForAzureDevops(string message)
        {
            // for Azure DevOps to pick up excpetion
            Console.WriteLine($"{prefix}  {ReplaceWithSpace(message)}");
        }

        private static string GetMVNErrorFromOutput(StringBuilder output)
        {
            List<string> errors = output.ToString().Split('\n').ToList()
                .Where(s => s.StartsWith("[ERROR]"))
                .ToList();

            return string.Join(" ", errors);
        }

        private static string ReplaceWithSpace(string message)
        {
            return message.Replace('\n', ' ').Replace('\r', ' ');
        }
    }
}
