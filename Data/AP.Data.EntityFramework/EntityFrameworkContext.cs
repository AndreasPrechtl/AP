using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AP.Data.EntityFramework;

public class EntityFrameworkContext : EntityContextBase<DbContext>
{
    public EntityFrameworkContext(DbContext context, bool ownsContext = true, object? contextKey = null)
        : base(context, ownsContext, contextKey)
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

    public EntityFrameworkContext(string connectionString, DbCompiledModel? model = null, object? contextKey = null)
        : this(new DbContext(connectionString, model), true, contextKey)
    { }

    public EntityFrameworkContext(DbConnection connection, DbCompiledModel? model = null, bool ownsConnection = true, object? contextKey = null)
        : this(new DbContext(connection, model, ownsConnection), ownsConnection, contextKey)
    { }

    public EntityFrameworkContext(ObjectContext provider, bool ownsContext = true, object? contextKey = null)
        : this(new DbContext(provider, ownsContext), ownsContext, contextKey)
    { }

    protected override Task OnSave(CancellationToken cancellationToken) => this.Provider.SaveChangesAsync(cancellationToken);

    protected override void OnDiscard()
    {
        foreach (DbEntityEntry entry in this.Provider.ChangeTracker.Entries())
            entry.State = EntityState.Unchanged;
    }

    protected override void OnCreation<TEntity>(TEntity entity) => this.Provider.Entry<TEntity>(entity).State = EntityState.Added;

    protected override void OnRegisterForUpdate<TEntity>(TEntity entity) => this.Provider.Entry<TEntity>(entity).State = EntityState.Modified;

    protected override void OnRegisterForDeletion<TEntity>(TEntity entity) => this.Provider.Entry<TEntity>(entity).State = EntityState.Deleted;

    protected override IQueryable<TEntity> OnQuery<TEntity>() => this.Provider.Set<TEntity>().AsNoTracking().AsQueryable();

    protected override IEntityModel CreateEntityModel() => new AP.Data.EntityModel<EntityFrameworkContext>(this);

    protected override IEntitySet<TEntity> CreateEntitySet<TEntity>() => new AP.Data.EntitySet<EntityFrameworkContext, TEntity>(this);
}
