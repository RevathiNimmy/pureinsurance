Imports Nexus.Utils.Nexus

Imports System.Web.UI

'AM + DH - 13-10-06 - Creates unlimited size dynamic tree for front end CMS navigation
'AM - 17-10-06 Updated to allow any number of trees generated on the page, and for them to inherit from each other
'              values assigned to them, to allow each tree to know how far to expand and know where it is in
'              comparrison with each other.

Namespace Frontend

    Public Class NavigationTree
        Inherits System.Web.UI.UserControl

        Private iTopID As Integer = -1
        Private iDepth As Integer
        Private iLowestDepth As Integer = 1
        Private iMaxDepth As Integer = 0
        Private bShowRoot As Boolean
        Private sCssID As String
        Private sCssSelectedID As String = ""
        Private iNavSelected As Integer = 0
        Dim phTmp As New WebControls.PlaceHolder
        Dim hasStarted As Boolean = False
        Dim sParentControl As String = ""

        'Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        '    'If sParentControl <> "" Then
        '    '    generateNav()
        '    '    '       Dim testControl As NavigationTree = CType(Page.FindControl(value), NavigationTree)
        '    '    Dim testControl As New NavigationTree
        '    '    Dim ControlList As ControlCollection = Nothing
        '    '    '            testControl = CType(Page.FindControl(value), NavigationTree)
        '    '    '            testControl = CType(Me.Parent.FindControl(value), NavigationTree)
        '    '    '            testControl = CType(Me.Parent.Page.FindControl(value), NavigationTree)
        '    '    '            ControlList = Page.Controls
        '    '    '            ControlList = Page.Master.Controls
        '    '    '            ControlList = Me.Parent.Controls
        '    '    ControlList = Me.Page.Master.Master.Controls
        '    '    If testControl Is Nothing Then
        '    '    Else
        '    '        testControl.NavSelected = iNavSelected
        '    '    End If

        '    'End If
        'End Sub


        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            If sParentControl <> "" And iNavSelected > 0 Then
                Dim testControl As NavigationTree = CType(Page.Master.FindControl(sParentControl), NavigationTree)
                testControl.NavSelected = iNavSelected
            End If
            If hasStarted = False Then
                generateNav()
            End If
        End Sub


        Public Sub generateNav()
            If iNavSelected = 0 Then
                iNavSelected = CInt(Request("sitemap_id"))
            End If
            If iLowestDepth > 1 Then
                iTopID = SiteMap.Functions.GetRoot(CInt(Request("sitemap_id")), (iLowestDepth))
                iNavSelected = iTopID
            End If
            Dim nodes As ArrayList = Functions.GetNavigation(iTopID, iDepth, True)
            phTmp.ID = "pl_" & Me.ID
            Dim OldDepth As Int16 = -1
            Dim MyPlaceHolder As WebControls.PlaceHolder
            MyPlaceHolder = phTmp
            Dim MyTmpPlaceHolder2 As WebControls.PlaceHolder = Nothing
            MyTmpPlaceHolder2 = MyPlaceHolder
            Dim i As Int16 = 0
            Dim output As String = String.Empty
            Dim bCheckLevel As Boolean = False
            Dim startDepth As Int16 = 0
            If checkNodeCorrect(nodes) Then
                For Each node As NavLink In nodes
                    If hasStarted = False Then
                        startDepth = node.Depth
                    End If
                    If hasStarted = False And iNavSelected = 0 And bShowRoot = False Then
                        bCheckLevel = True
                        MyTmpPlaceHolder2 = MyPlaceHolder
                    End If

                    If OldDepth < node.Depth Then
                        Dim MyTmpPlaceHolder As WebControls.PlaceHolder = New WebControls.PlaceHolder
                        MyTmpPlaceHolder.Visible = False
                        MyTmpPlaceHolder.ID = node.SiteID & "_" & Me.ID
                        MyPlaceHolder.Controls.Add(MyTmpPlaceHolder)
                        MyPlaceHolder = MyTmpPlaceHolder
                        If bCheckLevel = True Then
                            MyTmpPlaceHolder2 = MyPlaceHolder
                            '         aNavSelected.Add(node.SiteID & "_" & Me.ID)
                        End If
                        If hasStarted = True Or bShowRoot = True Then
                            MyPlaceHolder.Controls.Add(New LiteralControl("<ul>"))
                            MyPlaceHolder.Controls.Add(New LiteralControl("<li>"))
                        End If
                        iMaxDepth = iMaxDepth + 1
                    End If
                    If OldDepth = node.Depth Then
                        MyPlaceHolder.Controls.Add(New LiteralControl("</li><li>"))
                    End If

                    If OldDepth > node.Depth Then
                        For i = node.Depth To OldDepth - 1
                            MyPlaceHolder.Controls.Add(New LiteralControl("</li>"))
                            MyPlaceHolder.Controls.Add(New LiteralControl("</ul>"))
                            MyPlaceHolder = MyPlaceHolder.Parent
                        Next
                        MyPlaceHolder.Controls.Add(New LiteralControl("</li><li>"))
                    End If
                    bCheckLevel = False
                    If node.SiteID = iNavSelected Or (hasStarted = False And iNavSelected = 0) Then
                        bCheckLevel = True
                        If hasStarted = True Or bShowRoot = True Then
                            MyPlaceHolder.Controls.Add(CreatNavLink(node, True))
                        End If
                        MyTmpPlaceHolder2 = MyPlaceHolder
                    Else
                        If hasStarted = True Or bShowRoot = True Then
                            MyPlaceHolder.Controls.Add(CreatNavLink(node, False))
                        End If
                    End If
                    '    MyPlaceHolder.Controls.Add(New LiteralControl("<span>ID:" & node.SiteID & ":depth:" & node.Depth & ":checklevel:" & bCheckLevel & "</span>"))

                    OldDepth = node.Depth
                    hasStarted = True
                Next
                For i = startDepth - 1 To OldDepth - 1
                    If bShowRoot = False And i + 1 = OldDepth Then
                    Else
                        MyPlaceHolder.Controls.Add(New LiteralControl("</li>"))
                        MyPlaceHolder.Controls.Add(New LiteralControl("</ul>"))
                    End If
                    MyPlaceHolder = MyPlaceHolder.Parent
                Next

                ShowNav(MyTmpPlaceHolder2, startDepth)

            End If
            hasStarted = True

        End Sub

        Private Function checkNodeCorrect(ByVal arrayToCheck As ArrayList) As Boolean
            If arrayToCheck.Count > 0 Then
                Dim aChckNavLink As NavLink = arrayToCheck(0)
                If aChckNavLink.Depth < iLowestDepth Then
                    Return False
                Else
                    Return True
                End If
            Else
                Return False
            End If
        End Function

        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
            MyBase.Render(writer)
            phTmp.RenderControl(writer)
        End Sub


        Public Overridable Function CreatNavLink(ByVal Link As NavLink, ByVal selected As Boolean) As Control
            Dim hypTmp As New WebControls.HyperLink
            hypTmp.Text = Link.Text
            hypTmp.NavigateUrl = Link.Url
            hypTmp.Target = Link.Target
            If selected = True Then
                hypTmp.CssClass = sCssSelectedID
            End If
            Return hypTmp
        End Function

        Public Overridable Function ShowNav(ByVal pPlaceHolder As WebControls.PlaceHolder, ByVal iStartDepth As Integer)
            Dim i As Integer = iMaxDepth
            Do While pPlaceHolder.ID <> ("pl_" & Me.ID)
                If Not InStr("pl_", pPlaceHolder.ID) Then
                    iNavSelected = CInt(Left(pPlaceHolder.ID, (InStr(pPlaceHolder.ID, "_") - 1)))
                End If
                pPlaceHolder.Visible = True
                pPlaceHolder = pPlaceHolder.Parent
            Loop
            Return 0
        End Function

        Public WriteOnly Property TopID() As Integer
            Set(ByVal Value As Integer)
                iTopID = Value
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

        Public WriteOnly Property cssClass() As String
            Set(ByVal Value As String)
                sCssID = Value
            End Set
        End Property

        Public WriteOnly Property cssSelectedClass() As String
            Set(ByVal Value As String)
                sCssSelectedID = Value
            End Set
        End Property

        Public WriteOnly Property LowestDepth() As Integer
            Set(ByVal value As Integer)
                iLowestDepth = value
            End Set
        End Property

        Public Property NavSelected() As Integer
            Set(ByVal Value As Integer)
                iNavSelected = Value
            End Set
            Get
                generateNav()
                Return iNavSelected
            End Get
        End Property

        Public WriteOnly Property ParentControl() As String
            Set(ByVal value As String)
                sParentControl = value
            End Set
        End Property

    End Class

End Namespace
