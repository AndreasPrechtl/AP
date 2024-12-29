<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ContentPlaceHolderID="Headers" runat="server">
    <h1><%: "Hier sehen Sie einige Referenzen:" %></h1>
</asp:Content>
<asp:Content ID="aboutContent" ContentPlaceHolderID="Content" runat="server">
    <fieldset>
        <a href="http://www.bf-herbst.de"><img src="http://www.bf-herbst.de/Content/Images/favicon.ico" alt="" /><%: "Bäder- und Fliesenstudio Herbst - 92237 Sulzbach-Rosenberg" %></a>
    </fieldset>
    <fieldset>
        <a href="http://www.metallbau-kalkbrenner.de"><img src="http://www.metallbau-kalkbrenner.de/Content/Images/favicon.ico" alt="" /><%: "Metallbau Kalkbrenner - 92237 Sulzbach-Rosenberg" %></a>                    
    </fieldset>   
    <fieldset>
        <legend><%: "Haftungsausschluss" %></legend>
        <%: "Trotz sorgfältiger inhaltlicher Kontrolle übernehmen wir keine Haftung für die Inhalte externer Links. Für den Inhalt der verlinkten Seiten sind ausschließlich deren Betreiber verantwortlich." %> 
    </fieldset> 
</asp:Content>
