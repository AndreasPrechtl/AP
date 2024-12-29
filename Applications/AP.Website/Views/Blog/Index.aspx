<%@ Page Title="Entwickler-Blog" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ContentPlaceHolderID="Headers" runat="server"><h1><%: "Entwickler-Blog" %></h1></asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
<fieldset><legend></legend>
    <h3><%: "Demnächst mehr zu u.A. diesen Themen und Eigententwicklungen:" %></h3>
    <ul>
        <li><%: "Entity Framework Repository" %></li>
        <li><%: "Inversion of Control Container" %></li>
        <li><%: "Generic Type Converters" %></li>
        <li><%: "Linq-Extensions" %></li>
        <li><%: "Erweiterte und neue \"ReadOnlyCollections\"" %></li>
        <li><%: "Configuration mit XAML-Unterstützung" %></li>
        <li><%: "SiteMap speziell für ASP.NET MVC" %></li>
        <li><%: "Javascript- und Style-Combiners (WebForms)" %></li>
        <li><%: "Verbessertes Title- & MetaTags-System" %><br /><%: "für ASP.NET WebForms" %></li>
        <li><%: "HtmlHelper Erweiterungen" %></li>
    </ul>     
</fieldset>

</asp:Content>
