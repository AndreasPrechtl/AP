<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Web.Mvc.HandleErrorInfo>" %>

<asp:Content ID="errorTitle" ContentPlaceHolderID="Headers" runat="server">
    <h1><%: "Es ist ein Fehler aufgetreten:" %></h1>
</asp:Content>

<asp:Content ID="errorContent" ContentPlaceHolderID="Content" runat="server">
    <fieldset><legend></legend>
    <% 
        if (this.Model != null)
            Response.Write(this.Model.Exception.Message);
        else
            Response.Write(this.Context.Error.Message);    
    %>
    </fieldset>
</asp:Content>