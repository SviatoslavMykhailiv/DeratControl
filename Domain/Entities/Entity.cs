using System;

namespace Domain.Entities {
  /// <summary>
  /// Represents the base entity for all types of entities in Notes Domain.
  /// Every entity should be derived from <see cref="Entity"/>
  /// </summary>
  public abstract class Entity : IEquatable<Entity> {
    /// <summary>
    /// Unique identifier of an entity.
    /// </summary>
    public virtual Guid Id { get; init; }

    /// <summary>
    /// Identifies if an entity is transient.
    /// An entity is considered to be transient if it hasn't been persisted in a data source yet.
    /// </summary>
    public virtual bool IsTransient() => Id.Equals(default);

    #region Equality comparison

    /// <summary>
    /// Entity equality comparer.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj) {
      if (ReferenceEquals(this, obj))
        return true;

      if (obj is null)
        return false;

      if (obj is not Entity entity)
        return false;

      if (IsTransient() || entity.IsTransient())
        return false;

      var thisEntityType = GetType();
      var otherEntityType = entity.GetType();

      if (thisEntityType != otherEntityType)
        return false;

      return Id.Equals(entity.Id);
    }

    /// <summary>
    /// A strongly-typed equality comparer.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public virtual bool Equals(Entity other) {
      return Equals((object)other);
    }

    /// <summary>
    /// Gets a hashcode for the entity.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() {
      return Id.GetHashCode() ^ 31;
    }

    /// <summary>
    /// Compares two entities for equality.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(Entity left, Entity right) {
      if (ReferenceEquals(left, right))
        return true;

      if (left is null || right is null)
        return false;

      return left.Equals(right);
    }

    /// <summary>
    /// Compares two entities for inequality.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(Entity left, Entity right) {
      return !(left == right);
    }

    #endregion
  }
}
