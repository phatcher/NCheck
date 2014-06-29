namespace NCheck.Test
{
    public class ParentChecker : Checker<Parent>
    {
        public ParentChecker()
        {
            Initialize();
            Compare(x => x.Another).Ignore();
        }
    }
}