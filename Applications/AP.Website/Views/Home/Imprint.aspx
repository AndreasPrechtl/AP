<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content ContentPlaceHolderID="Headers" runat="server">
    <h1><%: "Impressum und Kontakt" %></h1>
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">    
<fieldset><legend></legend>
    <table>
        <tr>
            <td colspan="2">
                <h3>
                    <%: "Andreas Prechtl" %><br />
                    <%: ".NET & IT-Solutions" %>
                </h3>
            </td>
        </tr>
        <tr>
            <td style="width:200px">
                 <div id="_address">
                    <%: "Bahnhofstraße 7" %><br />
                    <%: "92237 Sulzbach-Rosenberg" %><br />                        
                    <%: "Deutschland" %>
                </div>
            </td>
            <td>
                <div>
                    <table>
                        <tr>
                            <td style="text-align:right"><label><%: "Tel:" %></label></td>
                            <td><%: "09661 / 30 48 730" %></td>
                        </tr>
                        <tr>
                            <td style="text-align:right"><label><%: "Fax:" %></label></td>
                            <td><%: "09661 / 30 48 731" %></td>
                        </tr>
                        <tr>
                            <td style="text-align:right"><label><%: "eMail:" %></label></td>
                            <td>
                                <%--<script type="text/javascript">var m='mailt'+'o:';var t='info'+'@and'+'reas-pr'+'echt'+'l.net';document.write('<a href=\"+m+t+\">'+t+'</a>');</script>--%>
                                <a onmouseover="this.onfocus();" onfocus="this.href='mailt'+'o:'+(this.innerText=this.textContent='info'+'@and'+'reas-pr'+'echt'+'l.net');"><%: "info [at] andreas-prechtl.net" %></a>
                            </td>
                        </tr>                                    
                    </table>
                </div>                
            </td>
        </tr>
    </table>
</fieldset>
<fieldset>
    <div>
        <table>
            <tr>
                <td><label><%: "Geschäftsform:" %></label></td>
                <td><%: "Einzelunternehmen" %></td>
            </tr>
            <tr>
                <td><label><%: "Umsatzsteuer-Id:"%></label></td>
                <td><%: "DE273712580"%></td>
            </tr>
            <tr>
                <td><label><%: "Registergericht:"%></label></td>
                <td><%: "Amberg"%></td>
            </tr>
        </table>
    </div>
</fieldset>
<fieldset>
    <legend>Website</legend>    
    <%= Html.Label("Verantwortlicher für den Inhalt im Sinne des § 55 RStV: ")%><p><%: "Andreas Prechtl" %><br /><%: "Bahnhofstraße 7" %><br /><%: "92237 Sulzbach-Rosenberg" %></p><br />

    <%= Html.Label("Design") %><%: "Andreas Prechtl" %>
    <%= Html.Label("Programmierung") %><%: "Andreas Prechtl" %><br />    

    <%= HttpUtility.HtmlDecode(ImprintValues.ExtraContent) %>
</fieldset>
<fieldset>
    <legend><%: "Urheberrecht" %></legend>
    <%: "Alle auf dieser Internetpräsenz verwendeten Inhalte (Skripte, Layouts, Fotos, etc.) sind urheberrechtlich geschützt. Sollten Sie Teile hiervon verwenden wollen, wenden Sie sich bitte an den jeweiligen Inhaber oder den Seitenbetreiber." %> 
</fieldset>
<fieldset>
    <legend><%: "Haftungsausschluss" %></legend>
    <%: "Trotz sorgfältiger inhaltlicher Kontrolle übernehmen wir keine Haftung für die Inhalte externer Links. Für den Inhalt der verlinkten Seiten sind ausschließlich deren Betreiber verantwortlich." %> 
</fieldset> 
</asp:Content>