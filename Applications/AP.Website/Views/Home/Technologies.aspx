<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" MasterPageFile="~/Views/Shared/Site.Master" %>

<asp:Content ContentPlaceHolderID="Headers" runat="server">
<h1><%: "Zur Projekterstellung werden u.A. folgende Technologien verwendet:" %></h1>
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">    
<fieldset><legend></legend>
    <ul>
        <li><%: "C# und .Net-Framework 4.0" %></li>
        <li><%: "Microsoft Visual Studio 2010" %></li>
        <li><%: "Microsoft Sql Server 2008" %></li>
        <li><%: "ASP.NET MVC 3.0 (WebForms-ViewEngine)" %></li>        
        <li><%: "Windows Presentation Foundation" %></li>
        <li><%: "Linq" %></li>
        <li><%: "EntityFramework" %></li>
        <li><%: "XAML" %></li>
        <li><%: "Html" %></li>    
        <li><%: "CSS" %></li>
        <li><%: "Silverlight" %></li>
        <li><%: "Javascript & jQuery" %></li>
        <li><%: "Bing Maps" %></li>
    </ul>                    
</fieldset>
</asp:Content>