namespace AP.ComponentModel
{
    public class Wrapper<T> : IWrapper<T>
    {
        public Wrapper(T value)
        {
            this.Value = value;
        }

        public static implicit operator T(Wrapper<T> wrapper)
        {
            return wrapper.Value;
        }
        public static implicit operator Wrapper<T>(T instance)
        {
            return new Wrapper<T>(instance);
        }

        public T Value { get; private set; }
    }

    public interface IWrapper<out T>
    {
        T Value { get; }
    }
}
