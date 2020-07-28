namespace DexcomApp.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for basic data provider.
    /// </summary>
    /// <typeparam name="T">The type of data to provide.</typeparam>
    public interface IDataProvider<T>
    {
        /// <summary>
        /// Creates the supplied object in the data store.
        /// </summary>
        /// <param name="data">The data to store.</param>
        /// <returns>The id of the created data object.</returns>
        int Create(T data);

        /// <summary>
        /// Reads the object from the data store by id.
        /// </summary>
        /// <param name="id">The id of the object to find.</param>
        /// <returns>The requested object or default.</returns>
        T Read(int id);

        /// <summary>
        /// Updates the object in the data store.
        /// </summary>
        /// <param name="data">The data to update.</param>
        /// <returns>The id of the updated object or 0 if the operation was not successful.</returns>
        int Update(T data);

        /// <summary>
        /// Deletes the object from the data store by id.
        /// </summary>
        /// <param name="id">The id of the object to delete.</param>
        /// <returns>The id of the deleted object or 0 if the operation was not successful.</returns>
        int Delete(int id);
    }
}
