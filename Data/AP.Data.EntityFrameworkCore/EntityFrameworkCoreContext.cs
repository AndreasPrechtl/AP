﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AP.Data.EntityFrameworkCore;

public class EntityFrameworkCoreContext : EntityContextBase<DbContext>
{
    public EntityFrameworkCoreContext(DbContext context, bool ownsContext = true, object? contextKey = null)
        : base(context, ownsContext, contextKey)
    {
        context.ChangeTracker.AutoDetectChangesEnabled = false;

        // disable the validation - funny enough - as soon as u use database first - it's hard coded into the entities... 
        // Why would I need a validation model when I can simply throw exceptions as soon as a property gets a wrong value
        // -> solution would be to use semivalidated classes -> if they have a validator 
        // -> they can throw exceptions otherwise they should just set the values without any complaints
        //context.ChangeTracker.ValidateOnSaveEnabled = false;
        
        if (!context.Database.CanConnect())
            throw new InvalidOperationException("Database doesn't exist or is unavailable");
    }

    // todo: check those ctors and see what can be done about it
    //public EntityFrameworkCoreContext(string connectionString, DbCompiledModel model = null, SaveMode saveMode = SaveMode.Default, object contextKey = null)
    //    : this(new DbContext(connectionString, model), true, saveMode, contextKey)
    //{ }

    //public EntityFrameworkCoreContext(DbConnection connection, DbCompiledModel model = null, bool ownsConnection = true, SaveMode saveMode = SaveMode.Default, object contextKey = null)
    //    : this(new DbContext(connection, model, ownsConnection), ownsConnection, saveMode, contextKey)
    //{ }

    //public EntityFrameworkCoreContext(ObjectContext provider, bool ownsContext = true, SaveMode saveMode = SaveMode.Default, object contextKey = null)
    //    : this(new DbContext(provider, ownsContext), ownsContext, saveMode, contextKey)
    //{ }

    protected override Task OnSave(CancellationToken cancellationToken) => this.Provider.SaveChangesAsync(cancellationToken);

    protected override void OnDiscard()
    {
        foreach (EntityEntry entry in this.Provider.ChangeTracker.Entries())
            entry.State = EntityState.Unchanged;
    }

    protected override void OnCreation<TEntity>(TEntity entity) => this.Provider.Entry<TEntity>(entity).State = EntityState.Added;

    protected override void OnRegisterForUpdate<TEntity>(TEntity entity) => this.Provider.Entry<TEntity>(entity).State = EntityState.Modified;

    protected override void OnRegisterForDeletion<TEntity>(TEntity entity) => this.Provider.Entry<TEntity>(entity).State = EntityState.Deleted;

    protected override IQueryable<TEntity> OnQuery<TEntity>() => this.Provider.Set<TEntity>().AsNoTracking().AsQueryable();

    protected override IEntityModel CreateEntityModel() => new AP.Data.EntityModel<EntityFrameworkCoreContext>(this);

    protected override IEntitySet<TEntity> CreateEntitySet<TEntity>() => new AP.Data.EntitySet<EntityFrameworkCoreContext, TEntity>(this);
}
