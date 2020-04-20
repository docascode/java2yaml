namespace Microsoft.Content.Build.Java2Yaml
{
    using System;

    using System.Collections.Generic;

    [Serializable]
    public class TocYaml
        : List<TocItemYaml>
    {
        public TocYaml(IEnumerable<TocItemYaml> items) : base(items) { }
        public TocYaml() : base() { }
    }
}
