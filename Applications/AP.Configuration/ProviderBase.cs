namespace AP.Configuration;

public abstract class ProviderBase : DisposableObject, IProvider
{
    public ProviderBase(string? name = null)
    {
        _name = name;
        this.Initialize();
    }

    private string _name;
    public string Name 
    {
        get { return _name; }
        private set
        {
            //if (IsRegistered)
            //    throw new InvalidOperationException("Cannot set the name for registered providers");

            _name = value;
        }
    }

    //public bool IsRegistered
    //{
    //    get { return this.Manager != null; }
    //}

    //private IProviderManager _mapper;
    //public IProviderManager Manager
    //{
    //    get { return _mapper; }            
    //}

    //void IProvider.SetManager(IProviderManager mapper)
    //{
    //    _mapper = mapper;
    //}

    protected virtual void Initialize()
    {
        _name = _name ?? this.GetType().Name;
    }

    //protected override void CleanUpResources()
    //{
    //    base.CleanUpResources();

    //    if (this.IsRegistered)
    //        _mapper.Release(this, false);            
    //}
}