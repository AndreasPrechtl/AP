#if fx45

using SCI = System.Collections.Immutable;

namespace AP.Collections.Immutable
{
    public class ImmutableQueue<T>
    {
        private ImmutableQueue(SCI.ImmutableQueue<T> inner)
        {
            ExceptionHelper.ThrowOnArgumentNullException(() => inner);
        }
    }
}

#endif