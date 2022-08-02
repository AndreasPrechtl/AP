using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;

namespace AP.Data.EntityFramework
{
    public class EntityFrameworkContext : EntityContextBase<DbContext>
    {
        public EntityFrameworkContext(DbContext context, bool ownsContext = true, SaveMode saveMode = SaveMode.Default, object contextKey = null)
            : base(context, ownsContext, saveMode, contextKey)
        {
            context.Configuration.AutoDetectChangesEnabled = false;

            // disable the validation - funny enough - as soon as u use database first - it's hard coded into the entities... 
            // Why would I need a validation model when I can simply throw exceptions as soon as a property gets a wrong value
            // -> solution would be to use semivalidated classes -> if they have a validator 
            // -> they can throw exceptions otherwise they should just set the values without any complaints
            context.Configuration.ValidateOnSaveEnabled = false;

            if (!context.Database.Exists())
                throw new InvalidOperationException("Database doesn't exist or is unavailable");
        }

        public EntityFrameworkContext(string connectionString, DbCompiledModel model = null, SaveMode saveMode = SaveMode.Default, object contextKey = null)
            : this(new DbContext(connectionString, model), true, saveMode, contextKey)
        { }

        public EntityFrameworkContext(DbConnection connection, DbCompiledModel model = null, bool ownsConnection = true, SaveMode saveMode = SaveMode.Default, object contextKey = null)
            : this(new DbContext(connection, model, ownsConnection), ownsConnection, saveMode, contextKey)
        { }

        public EntityFrameworkContext(ObjectContext provider, bool ownsContext = true, SaveMode saveMode = SaveMode.Default, object contextKey = null)
            : this(new DbContext(provider, ownsContext), ownsContext, saveMode, contextKey)
        { }

        protected override void OnSave()
        {
            this.Provider.SaveChanges();
        }

        protected override void OnDiscard()
        {
            foreach (DbEntityEntry entry in this.Provider.ChangeTracker.Entries())
                entry.State = EntityState.Unchanged;
        }

        protected override void OnCreation<TEntity>(TEntity entity)
        {
            this.Provider.Entry<TEntity>(entity).State = EntityState.Added;
        }

        protected override void OnRegisterForUpdate<TEntity>(TEntity entity)
        {
            this.Provider.Entry<TEntity>(entity).State = EntityState.Modified;
        }

        protected override void OnRegisterForDeletion<TEntity>(TEntity entity)
        {
            this.Provider.Entry<TEntity>(entity).State = EntityState.Deleted;
        }

        protected override IQueryable<TEntity> OnQuery<TEntity>()
        {
            return this.Provider.Set<TEntity>().AsNoTracking().AsQueryable();
        }

        protected override IEntityModel CreateEntityModel()
        {
            return new AP.Data.EntityModel<EntityFrameworkContext>(this);
        }

        protected override IEntitySet<TEntity> CreateEntitySet<TEntity>()
        {
            return new AP.Data.EntitySet<EntityFrameworkContext, TEntity>(this);
        }
    }
}
