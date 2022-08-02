namespace AP.Observable
{
    public delegate void MemberAccessingEventHandler(object sender, MemberAccessingEventArgs e);

    public interface INotifyMemberAccessing
    {
        event MemberAccessingEventHandler MemberAccessing;
    }
}