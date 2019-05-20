namespace Microsoft.Content.Build.Java2Yaml
{
    using System.Text;

    public class JavadocOptionModel
    {
        public string ClassPath { get; set; }
        public string Encoding => " UTF-8";
        public string DocletPath { get; set; }
        public string Doclet => Constants.DocletClass;
        public string SourcePath { get; set; }
        public string OutputPath { get; set; }

        public JavadocOptionModel() {}

        public static JavadocOptionModel Create(
            JavadocOptionModel option,
            string dependencies,
            string docletPath,
            string sourcePath,
            string outputPath)
        {
            option.ClassPath = dependencies;
            option.DocletPath = docletPath;
            option.SourcePath = sourcePath;
            option.OutputPath = sourcePath;

            return option;
        }

        public static string ToCmdArguments(JavadocOptionModel option)
        {
            var optionBuilder = new StringBuilder();

            foreach (var prop in typeof(JavadocOptionModel).GetProperties())
            {
                var optionName = prop.Name;
                var optionValue = option.GetType().GetProperty(optionName).GetValue(option, null);

                optionBuilder.Append("-");
                optionBuilder.Append(optionName.ToLower());
                optionBuilder.Append(" ");
                optionBuilder.Append(optionValue);
                optionBuilder.Append(" ");
            };

            return optionBuilder.ToString();
        }
    }
}
