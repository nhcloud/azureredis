using System;

namespace Caching.Interfaces
{
    public interface ICachingService
    {
        /// <summary>
        /// Adds the key to caching store
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        void Add(string key, object value);

        /// <summary>
        /// Add a key with absolute expiration
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpiration"></param>
        void Add(string key, object value, TimeSpan? absoluteExpiration);

        /// <summary>
        /// Read the value and returns as a object
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object Get(string key);

        /// <summary>
        /// Read the value and returns a type of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// Revemove the given key
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);

        /// <summary>
        /// Removes the keys that contains any given string
        /// </summary>
        /// <param name="containsStrings"></param>
        void RemoveAll(string[] containsStrings);

        /// <summary>
        /// Removes all keys
        /// </summary>
        void ClearAll();
    }
}