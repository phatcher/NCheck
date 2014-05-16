namespace NCheck.Test
{
    using NCheck.Checking;

    public class CheckerFactory : NCheck.CheckerFactory
    {
        public CheckerFactory()
        {
            PropertyCheck.IdentityChecker = new IdentifiableChecker();

            Initialize();
        }

        private void Initialize()
        {
            Register(typeof(CheckerFactory).Assembly);
            Register(typeof(NCheck.CheckerFactory).Assembly);

            Builder = new CheckerBuilder(this);
        }
    }
}