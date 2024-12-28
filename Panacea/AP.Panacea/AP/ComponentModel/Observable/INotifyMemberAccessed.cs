namespace AP.ComponentModel.Observable
{
    public delegate void MemberAccessedEventHandler(object sender, MemberAccessedEventArgs e);

    public interface INotifyMemberAccessed
    {
        event MemberAccessedEventHandler MemberAccessed;
    }
}