using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Caching.Common;

namespace CachingTester
{
    [TestClass]
    public class CacheFixer
    {
        [TestMethod]
        public void Add()
        {
            var val = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            CachingService.Store.Add("key1", val);
            Assert.AreEqual(CachingService.Store.Get<string>("key1"), val);
        }

        [TestMethod]
        public void Get()
        {
            var val = new User { Id = 1, Name = "NH" };
            CachingService.Store.Add("key2", val);
            Assert.AreEqual(CachingService.Store.Get<User>("key2").Id, val.Id);
        }

        [TestMethod]
        public void Remove()
        {
            var val = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            CachingService.Store.Add("key1", val);
            Assert.AreEqual(CachingService.Store.Get<string>("key1"), val);
            CachingService.Store.Remove("key1");
            Assert.IsNull(CachingService.Store.Get<string>("key1"));
        }

        [TestMethod]
        public void ClearAll()
        {
            var val = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            CachingService.Store.Add("key1", val);
            CachingService.Store.Add("key2", val);
            Assert.AreEqual(CachingService.Store.Get<string>("key1"), val);
            CachingService.Store.ClearAll();
            Assert.IsNull(CachingService.Store.Get<string>("key1"), CachingService.Store.Get<string>("key2"));
        }

        [TestMethod]
        public void RemoveAll()
        {
            var val = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            CachingService.Store.Add("key1", val);
            CachingService.Store.Add("key2", val);
            Assert.AreEqual(CachingService.Store.Get<string>("key1"), val);
            CachingService.Store.RemoveAll(new[] { "key1" });
            Assert.IsNull(CachingService.Store.Get<string>("key1"));
            Assert.IsNotNull(CachingService.Store.Get<string>("key2"));
        }
    }

    [Serializable]
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}