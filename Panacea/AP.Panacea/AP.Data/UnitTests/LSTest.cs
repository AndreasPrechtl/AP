using System;
using System.Data.Entity;
using System.Linq;
using AP.Data;
using AP.Data.EntityFramework;
using AP.Data.LinqToSql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class LSTest
    {
        private static readonly object ctxKey = new object();
        
        private IEntityContext _lsctx = null;
        private IEntitySet<LSPig> LSSet { get { return _lsctx.GetEntitySet<LSPig>(); } }

        [TestInitialize]
        public void Setup()
        {
            _lsctx = new LinqToSqlContext(new DataClasses1DataContext(), contextKey: ctxKey);
        }

        [TestCleanup]
        public void Dispose()
        {
            _lsctx.Dispose(ctxKey);
            _lsctx = null;
        }

        [TestMethod]
        public void TestLSCreation()
        {
            this.LSSet.Create(new LSPig { Name = "ficklette" });
            _lsctx.Save();

            var fick = this.LSSet.Query().First(p => p.Name == "ficklette");

            Assert.AreNotEqual(0, fick.Id);
        }

        [TestMethod]
        public void TestLSUpdate()
        {
            this.LSSet.Update(p => p.Name == "ficklette", (x) => x.Name = "fucklette");
            _lsctx.Save();

            var fick = this.LSSet.Query().FirstOrDefault(p => p.Name == "fucklette");

            Assert.IsNotNull(fick);
        }

        [TestMethod]
        public void TestLSDeletion()
        {
            this.LSSet.Delete(p => p.Name == "fucklette");
            _lsctx.Save();

            var fick = this.LSSet.Query().FirstOrDefault(p => p.Name == "fucklette");

            Assert.IsNull(fick);
        }
    }
}
