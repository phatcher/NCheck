namespace NCheck.Test
{
    public class SimpleChecker : Checker<Simple>
    {
        public SimpleChecker()
        {
            Compare(x => x.Id);
            Compare(x => x.Name);
        }
    }
}