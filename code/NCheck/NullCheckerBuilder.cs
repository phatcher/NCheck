namespace NCheck
{
    /// <summary>
    /// Stub implementation of a checker builder that always returns null;
    /// </summary>
    public class NullCheckerBuilder : ICheckerBuilder
    {
        /// <copydocfrom cref="ICheckerBuilder.Build" />
        public IChecker Build(System.Type type)
        {
            return null;
        }
    }
}