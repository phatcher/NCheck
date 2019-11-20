using System.Collections.Generic;

namespace NCheck.Test.Checking
{
    public class SampleDictionary : SampleClass
    {
        public SampleDictionary()
        {
            Integers = new Dictionary<string, int>();
            Strings = new Dictionary<string, string>();
            Children = new Dictionary<string, SampleClass>();
            Properties = new Dictionary<string, object>();
            Measures = new Dictionary<string, Measure<decimal>>();
        }

        public IDictionary<string, int> Integers { get; set; }

        public IDictionary<string, string> Strings { get; set; }

        public IDictionary<string, SampleClass> Children { get; set; }

        public IDictionary<string, Measure<decimal>> Measures { get; set; }

        public IDictionary<string, object> Properties { get; set; }
    }
}
