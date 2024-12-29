<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content ContentPlaceHolderID="Headers" runat="server">
<h2><%: "Willkommen auf der Homepage von" %></h2>
    <h1><%: "Andreas Prechtl .NET & IT-Solutions" %></h1>
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">    
<fieldset><legend></legend>
    <%: "Unsere Leistungen:" %>
    <ul>
        <li><%: "Web- und Software Entwicklung" %></li>        
        <li><%: "Web-Hosting und Wartung" %></li>
        <li><%: "Vertrieb von Hard- und Software" %></li>        
    </ul>
    <p>
        <%: "Eine Übersicht der Technologien, die wir verwenden, finden Sie " %><a href="/Home/Technologies"><%: "hier" %></a>.
        <br />
        <%: "Mehr zu den Themen mit denen wir uns außerdem befassen im "%><a href="/Blog/Index"><%: "Entwickler-Blog" %></a>.
    </p>
</fieldset>
</asp:Content>