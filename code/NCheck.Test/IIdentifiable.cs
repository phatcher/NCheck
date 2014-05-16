namespace NCheck.Test
{
    /// <summary>
    /// Marker interface for entities with identity.
    /// </summary>
    public interface IIdentifiable
    {
        object Id { get; }
    }
}