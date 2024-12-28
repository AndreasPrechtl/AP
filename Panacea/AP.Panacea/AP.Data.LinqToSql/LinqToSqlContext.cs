using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data;
using System.Data.Linq.Mapping;

namespace AP.Data.LinqToSql
{
    /// <summary>
    /// Provides an entity context using Linq2Sql as data layer.
    /// </summary>
    public class LinqToSqlContext : EntityContextBase<DataContext>
    {
        public LinqToSqlContext(DataContext model, bool ownsContext = true, SaveMode saveMode = Data.SaveMode.Default, object contextKey = null)
            : base(model, ownsContext, saveMode, contextKey)
        {
            model.ObjectTrackingEnabled = true;

            if (!model.DatabaseExists())
                throw new InvalidOperationException("Database doesn't exist or is unavailable");
        }

        public LinqToSqlContext(string connectionString, MappingSource mappingSource = null, SaveMode saveMode = Data.SaveMode.Default, object contextKey = null)
            : this(mappingSource != null ? new DataContext(connectionString, mappingSource) : new DataContext(connectionString), true, saveMode, contextKey)
        { }

        public LinqToSqlContext(IDbConnection connection, MappingSource mappingSource = null, bool ownsConnection = true, SaveMode saveMode = Data.SaveMode.Default, object contextKey = null)
            : this(mappingSource != null ? new DataContext(connection, mappingSource) : new DataContext(connection), ownsConnection, saveMode, contextKey)
        { }
        
        protected override void OnSave()
        {
            this.Provider.SubmitChanges();
        }

        protected override void OnDiscard()
        {
            this.Provider.Transaction.Rollback();
        }

        protected override IQueryable<TEntity> OnQuery<TEntity>()
        {
            return this.Provider.GetTable<TEntity>().AsQueryable();
        }

        protected override void OnCreation<TEntity>(TEntity entity)
        {
            this.Provider.GetTable<TEntity>().InsertOnSubmit(entity);
        }

        protected override void OnRegisterForUpdate<TEntity>(TEntity entity)
        {
            Table<TEntity> table = this.Provider.GetTable<TEntity>();

            TEntity original = table.GetOriginalEntityState(entity);

            if (original == null)
                table.Attach(entity, true);
        }

        protected override void OnRegisterForDeletion<TEntity>(TEntity entity)
        {
            this.Provider.GetTable<TEntity>().DeleteOnSubmit(entity);
        }

        protected override IEntityModel CreateEntityModel()
        {
            return new AP.Data.EntityModel<LinqToSqlContext>(this);
        }

        protected override IEntitySet<TEntity> CreateEntitySet<TEntity>()
        {
            return new AP.Data.EntitySet<LinqToSqlContext, TEntity>(this);
        }
    }
}
