namespace NCheck.Test.Examples
{
    public class ShinyBusinessService
    {
        public Simple Run(Simple source)
        {
            return new Simple { Id = 2, Name = "B", Value = 1.2 };
        }
    }
}