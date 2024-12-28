namespace AP.ComponentModel;

public interface IFilter<in TContext>
{
    void Filter(TContext context);
}