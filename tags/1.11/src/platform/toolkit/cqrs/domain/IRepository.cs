using System;

namespace Nohros.CQRS.Domain
{
  public interface IRepository<T> where T : AggregateRoot
  {
    /// <summary>
    /// Saves an aggregate into its repository.
    /// </summary>
    /// <param name="aggregate">
    /// The aggregate to be saved.
    /// </param>
    /// <param name="expected_version">
    /// The version that is expected for the <paramref name="aggregate"/>.
    /// </param>
    /// <exception cref=""></exception>
    void Save(T aggregate, int expected_version);

    /// <summary>
    /// Gets the aggregate that is associated with the given ID.
    /// </summary>
    /// <param name="id">
    /// The ID of the aggregate to get.
    /// </param>
    /// <returns>
    /// The aggregate that is associated with the given ID.
    /// </returns>
    /// <exception cref="AggregateNotFoundException">
    /// There is no aggregate associated with the given ID.
    /// </exception>
    T GetByID(Guid id);
  }
}
