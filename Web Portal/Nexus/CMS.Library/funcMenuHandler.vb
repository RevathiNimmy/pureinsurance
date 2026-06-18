Imports System.Text.RegularExpressions
Imports System.Web.UI.WebControls
Imports System.Web.HttpContext
Imports System.Web.Security
Imports System.Web
Imports System.Web.Configuration.WebConfigurationManager

Imports Nexus.Utils.Nexus

Public Module funcMenuHandler

    'All the functions need to be set as handles for
    'the menu in the masterpage for the application

    Sub mnuMain_Load(ByVal sender As Object, ByVal e As System.EventArgs)

        If Not Current.Session.Item("PortalID") Is Nothing Then

            Dim mnu As Menu = CType(sender, Menu)
            With mnu.DataBindings
                .Clear()
                Dim bindings As New MenuItemBinding
                With bindings
                    .DataMember = "MenuItem"
                    .NavigateUrlField = "url"
                    .TextField = "title"
                End With
                .Add(bindings)
            End With

            Dim xmlds As New XmlDataSource
            Dim datafile As String = "~/Configuration/" & AppSettings("ClientConfigurationFolder") & "/web.sitemap"

            xmlds.DataFile = Current.Server.MapPath(datafile)
            xmlds.XPath = "Menu/MenuItem"
            mnu.DataSource = xmlds
            mnu.DataBind()

            Dim i As Integer = 0
            While i <= mnu.Items.Count - 1
                If mnu.Items(i).ChildItems.Count < 1 And String.IsNullOrEmpty(mnu.Items(i).NavigateUrl) Then
                    mnu.Items.RemoveAt(i)
                Else
                    i += 1
                End If
            End While

            mnu.Items.Add(New System.Web.UI.WebControls.MenuItem("Log Off", "LogOff"))
            mnu.Visible = True

        End If

    End Sub

    Sub mnuMain_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs)

        Select Case e.Item.Value
            Case "LogOff"
                FormsAuthentication.SignOut()
                Current.Session.RemoveAll()
                Current.Response.Redirect(AppSettings("webroot") & "default.aspx")
        End Select

    End Sub

    Sub mnuMain_MenuItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs)

        Dim oParentItem As UI.WebControls.MenuItem = Nothing

        If e.Item.NavigateUrl <> "" Then
            oParentItem = e.Item.Parent()
        Else
            e.Item.Selectable = False
        End If

        If CType(e.Item.DataItem, System.Xml.XmlElement).Attributes.GetNamedItem("actions") IsNot Nothing Then

            Dim bHasPermission As Boolean = False
            Dim i As IEnumerator = CType(Current.Session.Item("Actions"), Hashtable).Keys.GetEnumerator()

            While i.MoveNext()
                If Regex.IsMatch(i.Current, CType(e.Item.DataItem, System.Xml.XmlElement) _
                    .Attributes.GetNamedItem("actions").Value) Then

                    bHasPermission = True

                End If
            End While

            If Not bHasPermission Then
                If oParentItem Is Nothing Then
                    CType(sender, UI.WebControls.Menu).Items.Remove(e.Item)
                Else
                    oParentItem.ChildItems.Remove(e.Item)
                End If
            End If

        End If

    End Sub

End Module