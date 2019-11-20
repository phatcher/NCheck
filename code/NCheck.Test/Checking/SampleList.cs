using System.Collections.Generic;

namespace NCheck.Test.Checking
{
    public class SampleList
    {
        public SampleList()
        {
            Children = new List<SampleClass>();
        }

        public int Id { get; set; }

        public IList<SampleClass> Children { get; set; }
    }
}