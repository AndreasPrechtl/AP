namespace AP.ComponentModel.ObjectManagement;

public partial class ObjectManager
{
    private class Item
    {
        public readonly bool DisposeOnRelease = true;
        public readonly IObjectLifetimeInternal Lifetime;

        public Item(IObjectLifetimeInternal lifetime, bool disposeOnRelease = true)
        {
            this.Lifetime = lifetime;
            this.DisposeOnRelease = disposeOnRelease;
        }

        public void OnReleased()
        {
            if (this.DisposeOnRelease)
                this.Lifetime.Dispose();                
        }
    }
}
