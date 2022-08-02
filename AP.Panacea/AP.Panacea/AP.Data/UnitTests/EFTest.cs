using System;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using AP.Data;
using AP.Data.EntityFramework;
using AP.Data.LinqToSql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class EFTest
    {
        private static readonly object ctxKey = new object();
        private IEntityContext _efctx = null;
        private TestDBEntities _dbTestEntities;

        private IEntitySet<EFPig> EFSet { get { return _efctx.GetEntitySet<EFPig>(); } }

        [TestInitialize]
        public void Setup()
        {
            _dbTestEntities = new UnitTestProject1.TestDBEntities();
            _efctx = new EntityFrameworkContext(_dbTestEntities, contextKey: ctxKey);
        }

        [TestCleanup]
        public void Dispose()
        {
            _efctx.Dispose(ctxKey);
            _efctx = null;
            _dbTestEntities = null;
        }

        [TestMethod]
        public void TestEFCreation()
        {
            var pig = new EFPig { Name = "ficklette" };

            this.EFSet.Create(pig);
            _efctx.Save();
            
            var fick = this.EFSet.Query().First(p => p.Name == "ficklette");
            
            Assert.AreNotEqual(0, fick.Id);
        }

        public void ClearShit()
        {
            this.EFSet.Delete(p => p != null);
        }

        [TestMethod]
        public void TestEFUpdate()
        {
            this.EFSet.Update(p => p.Name == "ficklette", x => x.Name = "fucklette");
            _efctx.Save();

            var fick = this.EFSet.Query().FirstOrDefault(p => p.Name == "fucklette");
            
            Assert.IsNotNull(fick);
        }

        [TestMethod]
        public void TestEFDeletion()
        {
            this.EFSet.Delete(p => p.Name == "fucklette");
            _efctx.Save();

            var fick = this.EFSet.Query().FirstOrDefault(p => p.Name == "fucklette");
            
            Assert.IsNull(fick);
        }
    }
}
