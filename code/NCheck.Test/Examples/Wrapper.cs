namespace NCheck.Test.Examples
{
    public class Wrapper<T> : IWrapper<T>
    {
        public T Content { get; set; }
    }
}