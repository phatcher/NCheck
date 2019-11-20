namespace NCheck.Test.Checking
{
    public class SampleClass : IIdentifiable
    {
        public int Id { get; set; }

        object IIdentifiable.Id => Id;

        public string Name { get; set; }

        public string Ignore { get; set; }

        public double Value { get; set; }
    }
}