// todo: move class out to separate assembly and basically use json for configs

//using Uno.Xaml;

//namespace AP.Configuration;

///// <summary>
///// Specialized provider for working with Xaml files.
///// </summary>
///// <typeparam name="TContent">The content type.</typeparam>
//public class XamlContentProvider<TContent> : FileContentProviderBase<TContent>
//{
//    protected XamlContentProvider(string? name = null)
//        : base(name: name)
//    { }

//    public XamlContentProvider(string fileName, string? name = null)
//        : base(fileName, name)
//    { }

//    protected override TContent ReadContent()
//    {
//        return (TContent)XamlServices.Load(this.FileName);         
//    }
//}
