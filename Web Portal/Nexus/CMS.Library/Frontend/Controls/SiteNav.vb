Imports Nexus.Utils.Nexus
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Web.UI

'DH  - 05-10-06 - Removes the need to copy the sitenav control into each new project

Namespace Frontend

    Public Class SiteNav
        Inherits System.Web.UI.UserControl

        Private iTopID As Integer = -1
        Private iDepth As Integer = 1
        Private bShowRoot As Boolean = False
        Private sCssClass As String
        Private iSitemapID As Integer
        Private bDropdownMenu As Boolean = False

        Public WriteOnly Property TopID() As Integer
            Set(ByVal Value As Integer)
                iTopID = Value
            End Set
        End Property

        'Public WriteOnly Property SitemapID() As Integer
        '    Set(ByVal Value As Integer)
        '        iSitemapID = Value
        '    End Set
        'End Property

        Public Property SitemapID() As Integer
            Get
                Return iSitemapID
            End Get
            Set(ByVal value As Integer)
                iSitemapID = value
            End Set
        End Property


        Public WriteOnly Property Depth() As Integer
            Set(ByVal Value As Integer)
                iDepth = Value
            End Set
        End Property

        Public WriteOnly Property ShowRoot() As Boolean
            Set(ByVal Value As Boolean)
                bShowRoot = Value
            End Set
        End Property

        Public Property CssClass() As String
            Get
                Return sCssClass
            End Get
            Set(ByVal value As String)
                sCssClass = value
            End Set
        End Property

        Public Property DropdownMenu() As Boolean
            Get
                Return bDropdownMenu
            End Get
            Set(ByVal value As Boolean)
                bDropdownMenu = value
            End Set
        End Property
        
        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
            MyBase.Render(writer)

            Dim phTmp As New WebControls.PlaceHolder
            Dim COunter As Integer = 1
            phTmp.Controls.Add(New LiteralControl("<ul" & IIf(ID Is Nothing, "", " id=""" & ID & """" & IIf(sCssClass Is Nothing, "", " class=""" & sCssClass & """") & ">" & vbCr)))

            If IsNumeric(iTopID) Then

                Dim arNavigation As ArrayList = Functions.GetNavigation(iTopID, iDepth, bShowRoot)

                If arNavigation.Count > 0 Then
                    For Each oNavLink As NavLink In arNavigation

                        If Counter = arNavigation.Count Then
                            If SitemapID = oNavLink.MenuID Then
                                phTmp.Controls.Add(New LiteralControl("<li class=""both"">"))
                            Else
                                phTmp.Controls.Add(New LiteralControl("<li class=""last"">"))
                            End If
                        Else
                            If SitemapID = oNavLink.MenuID Then
                                phTmp.Controls.Add(New LiteralControl("<li class=""current"">"))
                            Else
                                phTmp.Controls.Add(New LiteralControl("<li>"))
                            End If
                        End If

                        Dim hypTmp As New WebControls.HyperLink
                        hypTmp.Text = oNavLink.Text
                        'hypTmp.NavigateUrl = oNavLink.Url

                        'If Not oNavLink.Url.ToLower().StartsWith("http") Then
                        '    'If AllowReWrite Then
                        '    '    hypTmp.NavigateUrl = System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & oNavLink.Title
                        '    'Else
                        '    hypTmp.NavigateUrl = oNavLink.Url
                        '    'End If
                        'Else
                        hypTmp.NavigateUrl = oNavLink.Url
                        'End If

                        hypTmp.Target = oNavLink.Target
                        phTmp.Controls.Add(hypTmp)

                        If bDropdownMenu Then
                            ' Write out the list for drop down menu.
                            Dim arSubNav As ArrayList = Functions.GetNavigation(oNavLink.MenuID, 1, False)

                            If arSubNav.Count > 0 Then
                                phTmp.Controls.Add(New LiteralControl("<ul" & IIf(ID Is Nothing, "SubMenu_" & oNavLink.MenuID, " id=""" & ID & "_SubMenu_" & oNavLink.MenuID & """") & ">"))
                                For Each SubNavLink As NavLink In arSubNav
                                    phTmp.Controls.Add(New LiteralControl("<li>"))
                                    Dim SubhypTmp As New WebControls.HyperLink
                                    SubhypTmp.Text = SubNavLink.Text

                                    SubhypTmp.NavigateUrl = SubNavLink.Url

                                    SubhypTmp.Target = SubNavLink.Target
                                    phTmp.Controls.Add(SubhypTmp)

                                    phTmp.Controls.Add(New LiteralControl("</li>"))
                                Next
                                phTmp.Controls.Add(New LiteralControl("</ul>"))
                            End If
                        End If


                        phTmp.Controls.Add(New LiteralControl("</li>"))
                        COunter = COunter + 1
                    Next

                End If

            End If

            phTmp.Controls.Add(New LiteralControl("</ul>"))
            phTmp.RenderControl(writer)

        End Sub

    End Class

End Namespace