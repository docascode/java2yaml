using System.IO;
using System.Linq;

namespace Microsoft.Content.Build.Java2Yaml
{
    public class GradleUtility
    {
        private string Tempalte;
        private const string _gradleFileName = "build.gradle";
        private string _templatePath= Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Templates/TemplateGradle.txt");

        public GradleUtility()
        {
            Tempalte = File.ReadAllText(_templatePath);
        }

        public bool GenerateBuilFile(string package, string output)
        {
            Guard.ArgumentNotNullOrEmpty(package, nameof(package));
            Guard.ArgumentNotNullOrEmpty(output, nameof(output));

            string conent = Tempalte.Replace("{0}", package);

            System.IO.File.WriteAllText(Path.Combine(output, _gradleFileName), conent);

            return FileUtility.GetFilesByNameExtension(output, _gradleFileName).Any();
        }
    }
}
