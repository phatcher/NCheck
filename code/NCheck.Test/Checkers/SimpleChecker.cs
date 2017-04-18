namespace NCheck.Test
{
    using System;

    public class SimpleChecker : Checker<Simple>
    {
        public SimpleChecker()
        {
            Compare(x => x.Id);
            Compare(x => x.Name);
            Compare(x => x.Value).Value<double>((x, y) => Math.Abs(x - y) < 0.001);
        }
    }
}