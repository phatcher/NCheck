using System.Collections.Generic;

namespace NCheck.Test.Checking
{
    public class SampleDictionary : SampleClass
    {
        public SampleDictionary()
        {
            Properties = new Dictionary<string, object>();
        }

        public IDictionary<string, object> Properties { get; set; }
    }
}
