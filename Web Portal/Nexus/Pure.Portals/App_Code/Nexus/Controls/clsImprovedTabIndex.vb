Imports System.Xml.XPath
Imports System.Configuration
Imports System.Web.HttpContext
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports Nexus.Library.Config
Imports CMS.Library.Portal
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus

    ''' <summary>
    ''' The TabIndex displays all tabs (page links) for the current level of page
    ''' within the BackOffice screen structure. The TabIndex is key to the functionality
    ''' of the risk screen as it determines the current location within the SAM dataset
    ''' and the current element name and parent element.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ImprovedTabIndex : Inherits UserControl

        ''' <summary>
        ''' The TabIndex control implements a repeater, so we need to define a template
        ''' for the reapeater to use, the TabTemplate class is that template.
        ''' </summary>
        ''' <remarks></remarks>
        Private Class TabTemplate : Implements System.Web.UI.ITemplate

            Private sID As String
            Private sCssClass As String
            Private oTemplateType As ListItemType



            Sub New(ByVal v_oType As ListItemType, ByVal v_sID As String, Optional ByVal v_sCssClass As String = Nothing)
                oTemplateType = v_oType
                sID = v_sID
                sCssClass = v_sCssClass
            End Sub

            ''' <summary>
            ''' Add the tabs and the surrounding controls to the page control tree as they are created
            ''' </summary>
            ''' <param name="container"></param>
            ''' <remarks></remarks>
            Public Sub InstantiateIn(ByVal container As System.Web.UI.Control) _
                Implements System.Web.UI.ITemplate.InstantiateIn

                Dim phTmp As New PlaceHolder()

                Select Case (oTemplateType)
                    Case ListItemType.Header

                        'Peril Link
                        phTmp.Controls.Add(New LiteralControl("<ul id=""" & sID & "_tabs"" class='nav nav-lines nav-tabs b-danger'>" & vbCr))


                    Case ListItemType.Item, ListItemType.AlternatingItem

                        Dim lnkTab As New LinkButton()
                        lnkTab.ID = "lnkTab"

                        Dim ltListItemStartTag, ltListItemEndTag As New Literal()
                        ltListItemStartTag.ID = "ltListItemStartTag"
                        ltListItemEndTag.ID = "ltListItemEndTag"

                        phTmp.Controls.Add(ltListItemStartTag)
                        phTmp.Controls.Add(lnkTab)
                        phTmp.Controls.Add(ltListItemEndTag)

                    Case ListItemType.Footer

                        phTmp.Controls.Add(New LiteralControl("</ul>" & vbCr))

                End Select

                container.Controls.Add(phTmp)

            End Sub

        End Class

        Private rptrTabs As New Repeater
        Private inpHiddenTabs As New HtmlInputHidden
        Private inpVisibleTabs As New HtmlInputHidden

        Private sCssClass As String
        Private sTabContainerClass As String
        Private sActiveTabClass As String
        Private sDisabledClass As String

        Private sFolder As String
        Private iNestedPrefix As Integer

        Private sScreenCode As String
        Private bScrollable As Boolean = False

        Private sNextTab As String = Nothing
        Private sPreviousTab As String = Nothing
        Private sParentTab As String = Nothing
        Private sParentTabID As String = Nothing
        Private iDepth As Integer = 1

        ''' <summary>
        ''' The event that is fired when a tab is clicked, this is intended to be handled by the BaseRisk class
        ''' </summary>
        ''' <param name="Path">The location to redirect to</param>
        ''' <remarks></remarks>
        Public Event TabClicked(ByVal Path As String)

        ''' <summary>
        ''' After the OnInit event is completed this event is fired, to pass the configured values to the parent page
        ''' </summary>
        ''' <param name="ScreenCode">The current screencode, according to the current url and the tabindex config for the current product</param>
        ''' <param name="Depth">Current depth of the tabindex tree, need to match up the OI keys with the correct screens</param>
        ''' <param name="PrevPage">Path of the previous page, to enable the redirect for the back button on the risk screen</param>
        ''' <param name="NextPage">Path of the next page, to enable the redirect for the next button on the risk screen</param>
        ''' <remarks></remarks>
        Public Event ValuesInitialized(ByVal ScreenCode As String, _
                                        ByVal Depth As Integer, _
                                         ByVal ParentTab As String, _
                                        ByVal PrevPage As String, _
                                        ByVal NextPage As String)

        ''' <summary>
        ''' The css class to be implemented on the tabindex container.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CssClass() As String
            Get
                Return sCssClass
            End Get
            Set(ByVal value As String)
                sCssClass = value
            End Set
        End Property

        ''' <summary>
        ''' The css class used to style the internal container of the tabindex, this is generally
        ''' used to implement the scrolling tabs functionality as you need to limited the size
        ''' and visibility of the container to allow the scrolling to take place
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TabContainerClass() As String
            Get
                Return sTabContainerClass
            End Get
            Set(ByVal value As String)
                sTabContainerClass = value
            End Set
        End Property

        ''' <summary>
        ''' Allows the current tab to be styled differently to the rest
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ActiveTabClass() As String
            Get
                Return sActiveTabClass
            End Get
            Set(ByVal value As String)
                sActiveTabClass = value
            End Set
        End Property

        ''' <summary>
        ''' Allows any tabs which are disabledt to be styled differently
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DisabledClass() As String
            Get
                Return sDisabledClass
            End Get
            Set(ByVal value As String)
                sDisabledClass = value
            End Set
        End Property

        ''' <summary>
        ''' Should the scrolling functionality of the tabindex be enabled? only need when there
        ''' are more tabs than will fit on the screen horizontally, usually 5+. This needs to
        ''' be used on conjuction with the css styling, as simply enabling it will only display
        ''' the scroll buttons and add the js, the tabs won't scroll if the containing elements
        ''' aren't limited in size.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Scrollable() As Boolean
            Get
                Return bScrollable
            End Get
            Set(ByVal value As Boolean)
                bScrollable = value
            End Set
        End Property

        Public ReadOnly Property ScreenCode() As String
            Get
                Return sScreenCode
            End Get
        End Property

        Public ReadOnly Property Depth() As Integer
            Get
                Return iDepth
            End Get
        End Property

        Public ReadOnly Property NextTab() As String
            Get
                Return sNextTab
            End Get
        End Property

        Public ReadOnly Property PreviousTab() As String
            Get
                Return sPreviousTab
            End Get
        End Property

        Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
            MyBase.OnInit(e)

            rptrTabs.HeaderTemplate = New TabTemplate(ListItemType.Header, ClientID)
            rptrTabs.FooterTemplate = New TabTemplate(ListItemType.Footer, ClientID)
            rptrTabs.ItemTemplate = New TabTemplate(ListItemType.Item, ClientID)
            rptrTabs.AlternatingItemTemplate = New TabTemplate(ListItemType.AlternatingItem, ClientID)
            AddHandler rptrTabs.ItemDataBound, AddressOf ItemBound
            Dim oNexusConfig As NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), NexusFrameWork)
            Dim sXMLPath As String
            Dim oClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)

            If Request("PerilID") IsNot Nothing Or Session(CNClaimPerilKey) IsNot Nothing Then
                Dim PerilCode As String = String.Empty
                Dim iPerilID As Integer
                If Request("PerilID") IsNot Nothing Then
                    iPerilID = Request("PerilID")
                    Session(CNClaimPerilKey) = iPerilID
                ElseIf Session(CNClaimPerilKey) IsNot Nothing Then
                    iPerilID = Session(CNClaimPerilKey)
                End If
                If iPerilID = 0 Then
                    Dim iPerilIndex As Integer
                    If Request("PerilIndex") IsNot Nothing Then
                        iPerilIndex = Request("PerilIndex")
                        Session(CNClaimPerilIndex) = iPerilIndex
                        PerilCode = oClaim.ClaimPeril(iPerilIndex).TypeCode
                    ElseIf Session(CNClaimPerilIndex) IsNot Nothing Then
                        iPerilIndex = Session(CNClaimPerilIndex)
                        PerilCode = oClaim.ClaimPeril(iPerilIndex).TypeCode
                    End If
                Else
                    For Each oClaimPeril As NexusProvider.PerilSummary In oClaim.ClaimPeril
                        If oClaimPeril.ClaimPerilKey = iPerilID Then
                            PerilCode = Trim(oClaimPeril.TypeCode)
                            Session(CNClaimPerilKey) = oClaimPeril.ClaimPerilKey
                        End If
                    Next
                End If

                If oNexusConfig.Portals.Portal(GetPortalID()).Claims.PerilTypes.PerilType(Trim(PerilCode)) IsNot Nothing Then
                    sFolder = AppSettings("WebRoot") & "Claims/ClientPages/" & oNexusConfig.Portals.Portal(GetPortalID()).Claims.ScreenLocation & "/Perils/" _
                                      & oNexusConfig.Portals.Portal(GetPortalID()).Claims.PerilTypes.PerilType(PerilCode).Folder
                Else
                    sFolder = String.Empty
                End If

                sXMLPath = Server.MapPath(sFolder & "\perilscreens.config")

            Else
                Session.Remove(CNClaimPerilKey)
                If oNexusConfig.Portals.Portal(GetPortalID()).Claims.RiskTypes.RiskType(Trim(oClaim.RiskType)) Is Nothing Then
                    'use the default folder If risk type is not configured
                    sFolder = AppSettings("WebRoot") & "Claims/ClientPages/" & oNexusConfig.Portals.Portal(GetPortalID()).Claims.ScreenLocation & "/Claims/" _
                              & oNexusConfig.Portals.Portal(GetPortalID()).Claims.RiskTypes.DefaultFolder
                ElseIf String.IsNullOrEmpty(oNexusConfig.Portals.Portal(GetPortalID()).Claims.RiskTypes.RiskType(Trim(oClaim.RiskType)).Folder) = True Then
                    'use the default folder,if folder is empty
                    sFolder = AppSettings("WebRoot") & "Claims/ClientPages/" & oNexusConfig.Portals.Portal(GetPortalID()).Claims.ScreenLocation & "/Claims/" _
                              & oNexusConfig.Portals.Portal(GetPortalID()).Claims.RiskTypes.DefaultFolder
                Else
                    'we have the risk type specified so use that folder
                    sFolder = AppSettings("WebRoot") & "Claims/ClientPages/" & oNexusConfig.Portals.Portal(GetPortalID()).Claims.ScreenLocation & "/Claims/" _
                              & oNexusConfig.Portals.Portal(GetPortalID()).Claims.RiskTypes.RiskType(Trim(oClaim.RiskType)).Folder
                End If

                sXMLPath = Server.MapPath(sFolder & "\claimscreens.config")
            End If

            iNestedPrefix = sFolder.Split("/").Length

            If IO.File.Exists(sXMLPath) Then

                Dim xmlds As New XmlDataSource
                xmlds.DataFile = sXMLPath
                xmlds.EnableCaching = False

                Dim Navigator As XPathNavigator
                Dim Doc As XPathDocument = New XPathDocument(sXMLPath)
                Navigator = Doc.CreateNavigator()
                Dim i As XPathNodeIterator

                'Find the number of directories deep we are past the product root
                Dim iNestedDirectories As Integer = (Current.Request.Url.Segments.Length - 1) - iNestedPrefix

                Dim sUrl As String = String.Empty
                Dim sXPath As String = String.Empty

                If iNestedDirectories >= 0 Then

                    iDepth = iNestedDirectories + 1

                    'Construct the url from the product root
                    For x As Integer = iNestedDirectories To 0 Step -1
                        sUrl &= Request.Url.Segments(Request.Url.Segments.Length - (x + 1))
                    Next

                    sXPath = "//tab[translate(@url, 'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')='" & UCase(sUrl) & "']"

                Else
                    iDepth = 1

                    sXPath = "/screens/screen/tab[1]"

                End If

                i = Navigator.Select(sXPath & "/parent::node()")

                While (i.MoveNext)
                    sScreenCode = i.Current.GetAttribute("screen_code", String.Empty)
                    sParentTabID = i.Current.GetAttribute("id", String.Empty)
                End While

                i = Navigator.Select(sXPath & "/parent::node()" & "/parent::node()")

                While (i.MoveNext)
                    sParentTab = i.Current.GetAttribute("url", String.Empty)
                End While

                If String.IsNullOrEmpty(sParentTab) Then
                    'No parent, as we are at the top level, so when we get to
                    'the last screen we need to get out of the risk process
                    sParentTab = String.Empty
                Else
                    sParentTab = sFolder & "/" & sParentTab
                End If

                xmlds.XPath = sXPath & "/preceding-sibling::tab | " & sXPath & " | " & sXPath & "/following-sibling::tab"

                MyBase.Controls.Add(New LiteralControl("<div id=""" & ClientID & """" & IIf(sCssClass <> "", " class=""" & sCssClass & """", "") & ">" & vbCr))

                If bScrollable Then
                    Dim hypScrollLeft As New HyperLink
                    hypScrollLeft.ID = "hypLeftArrow"
                    hypScrollLeft.SkinID = "left_arrow"
                    'hypScrollLeft.NavigateUrl = "#"
                    hypScrollLeft.Attributes.Add("onmousedown", "ScrollLeft();")
                    hypScrollLeft.Attributes.Add("onmouseup", "CancelScroll();")
                    hypScrollLeft.Attributes.Add("onmouseout", "CancelScroll();")
                    hypScrollLeft.Attributes.Add("style", "display:none;")
                    MyBase.Controls.Add(hypScrollLeft)
                End If

                MyBase.Controls.Add(New LiteralControl("<div id=""" & ClientID & "_tabholder""" & IIf(sTabContainerClass <> "", " class=""" & sTabContainerClass & """", "") & ">" & vbCr))
                MyBase.Controls.Add(rptrTabs)

                If Session.Item(CNTabState & "_" & ID) Is Nothing Then
                    Session.Add(CNTabState & "_" & ID, New Hashtable())
                Else
                    Dim oTabState As Hashtable = CType(Session.Item(CNTabState & "_" & ID), Hashtable)

                    If Current.Request.Form(UniqueID & "$inpHiddenTabs") IsNot Nothing Then

                        Dim sHiddenTabs() As String = Current.Request.Form(UniqueID & "$inpHiddenTabs").Split(Regex.Split(", ", " "), StringSplitOptions.RemoveEmptyEntries)

                        For Each sTab As String In sHiddenTabs
                            If sTab.StartsWith("0") Then
                                sTab = CInt(sTab).ToString()
                            End If

                            If oTabState.ContainsKey(sTab) Then
                                oTabState(sTab) = False
                            Else
                                oTabState.Add(sTab, False)
                            End If
                        Next

                    End If

                    If Current.Request.Form(UniqueID & "$inpVisibleTabs") IsNot Nothing Then

                        Dim sVisibleTabs() As String = Current.Request.Form(UniqueID & "$inpVisibleTabs").Split(Regex.Split(", ", " "), StringSplitOptions.RemoveEmptyEntries)

                        For Each sTab As String In sVisibleTabs
                            If sTab.StartsWith("0") Then
                                sTab = CInt(sTab).ToString()
                            End If

                            If oTabState.ContainsKey(sTab) Then
                                oTabState(sTab) = True
                            Else
                                oTabState.Add(sTab, True)
                            End If
                        Next

                    End If

                End If

                MyBase.Controls.Add(New LiteralControl("</div>" & vbCr))

                If bScrollable Then
                    Dim hypScrollRight As New HyperLink
                    hypScrollRight.ID = "hypRightArrow"
                    hypScrollRight.SkinID = "right_arrow"
                    'hypScrollRight.NavigateUrl = "#"
                    hypScrollRight.Attributes.Add("onmousedown", "ScrollRight();")
                    hypScrollRight.Attributes.Add("onmouseup", "CancelScroll();")
                    hypScrollRight.Attributes.Add("onmouseout", "CancelScroll();")
                    hypScrollRight.Attributes.Add("style", "display:none;")
                    MyBase.Controls.Add(hypScrollRight)
                End If

                MyBase.Controls.Add(New LiteralControl("</div>" & vbCr))

                inpHiddenTabs.ID = "inpHiddenTabs"
                inpVisibleTabs.ID = "inpVisibleTabs"

                MyBase.Controls.Add(inpHiddenTabs)
                MyBase.Controls.Add(inpVisibleTabs)

                rptrTabs.DataSource = xmlds
                rptrTabs.DataBind()

                RaiseEvent ValuesInitialized(sScreenCode, iDepth, sParentTab, sPreviousTab, sNextTab)

            End If

        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

            'Dim sbJavaFunctions As New StringBuilder

            'sbJavaFunctions.Append("sTabIndexID = '" & ClientID & "'" & vbCr)
            'sbJavaFunctions.Append("ctrlInputHiddenTabs = '" & inpHiddenTabs.ClientID & "'" & vbCr)
            'sbJavaFunctions.Append("ctrlInputVisibleTabs = '" & inpVisibleTabs.ClientID & "'" & vbCr)

            'If bScrollable Then
            '    sbJavaFunctions.Append("document.getElementById('" & Me.ClientID & "_hypLeftArrow').style.display = '';" & vbCr)
            '    sbJavaFunctions.Append("document.getElementById('" & Me.ClientID & "_hypRightArrow').style.display = '';" & vbCr & vbCr)
            'End If

            Page.ClientScript.RegisterClientScriptInclude("TabIndex", AppSettings("WebRoot") & "js/TabIndex.js")
            'Page.ClientScript.RegisterStartupScript(Me.GetType, "TabIndexVariables", sbJavaFunctions.ToString, True)

        End Sub

        Private Sub ItemBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)

            Select Case e.Item.ItemType
                Case ListItemType.Header

                Case ListItemType.Item, ListItemType.AlternatingItem

                    Dim XPNav As XPathNavigator = CType(e.Item.DataItem, IXPathNavigable).CreateNavigator
                    Dim lnkTab As LinkButton = CType(e.Item.FindControl("lnkTab"), LinkButton)
                    Dim ltListItemStartTag As Literal = CType(e.Item.FindControl("ltListItemStartTag"), Literal)
                    Dim ltListItemEndTag As Literal = CType(e.Item.FindControl("ltListItemEndTag"), Literal)
                    Dim oTabState As Hashtable = CType(Session.Item(CNTabState & "_" & ID), Hashtable)
                    'To handle the navigation logic of dynamic hiding of tabs from session state.
                    Dim bIsHidden As Boolean = False

                    lnkTab.Text = XPNav.GetAttribute("name", String.Empty)

                    'Is the tab in the hidden list ?
                    Dim oTab As Object = CType(Session.Item(CNTabState & "_" & ID), Hashtable)(XPNav.GetAttribute("id", String.Empty))

                    ltListItemStartTag.Text = "<li id=""" & ClientID & "_" & XPNav.GetAttribute("id", String.Empty) & """"

                    If oTab IsNot Nothing Then
                        'Found, so use the state held in session
                        ltListItemStartTag.Text &= IIf(CType(oTab, Boolean), "", " style=""display:none""")
                    Else
                        'Not in session so use the default state in the xml
                        ltListItemStartTag.Text &= IIf(XPNav.GetAttribute("visible", String.Empty) = "false", " style=""display:none""", "")
                    End If

                    'ltListItemStartTag.Text &= ">"
                    ltListItemEndTag.Text = "</li>" & vbCr

                    If IsCurrentPage(XPNav.GetAttribute("url", String.Empty)) Then
                        ltListItemStartTag.Text &= " class='" + sActiveTabClass + "'>"
                        'Set risk progress to current ... if its the furthest we've gone
                        SetRiskProgress(sParentTabID, e.Item.ItemIndex)

                        'lnkTab.CssClass = sActiveTabClass

                        If XPNav.MoveToNext Then
                            Dim sThisID As String = XPNav.GetAttribute("id", String.Empty)
                            Dim sThisURL As String = XPNav.GetAttribute("url", String.Empty)
                            If oTabState.ContainsKey(sThisID) Then
                                If oTabState(sThisID) = False Then
                                    'if current page set HIDETAB of next page , move to NextToNext Page
                                    XPNav.MoveToNext()
                                End If
                            End If

                            sThisID = XPNav.GetAttribute("id", String.Empty)
                            bIsHidden = IIf((oTabState.ContainsKey(sThisID) AndAlso oTabState(sThisID) = False), True, False)

                            'will check next page visiblity
                            If (XPNav.GetAttribute("visible", String.Empty) = "false") OrElse
                                (XPNav.GetAttribute("visible", String.Empty) = "" AndAlso bIsHidden) Then
                                Dim bNextVisible As Boolean = False
                                bIsHidden = False
                                While bNextVisible = False
                                    ''loop through till we find a tab that is not hidden, this is the next tab
                                    If XPNav.MoveToNext() Then
                                        sThisID = XPNav.GetAttribute("id", String.Empty)
                                        bIsHidden = IIf((oTabState.ContainsKey(sThisID) AndAlso oTabState(sThisID) = False), True, False)

                                        If (XPNav.GetAttribute("visible", String.Empty) = "false") OrElse
                                        (XPNav.GetAttribute("visible", String.Empty) = "" AndAlso bIsHidden) Then
                                            bNextVisible = False
                                        Else
                                            bNextVisible = True
                                            sNextTab = sFolder & "/" & XPNav.GetAttribute("url", String.Empty)
                                        End If
                                    Else
                                        bNextVisible = True
                                        sNextTab = sParentTab
                                    End If
                                End While
                            Else
                                sNextTab = sFolder & "/" & XPNav.GetAttribute("url", String.Empty)
                            End If

                            bIsHidden = False
                            'Now XPNav have next Page information- Set MoveToPrevious so that 
                            If XPNav.MoveToPrevious Then
                                sThisID = XPNav.GetAttribute("id", String.Empty)
                                bIsHidden = IIf((oTabState.ContainsKey(sThisID) AndAlso oTabState(sThisID) = False), True, False)

                                'Previous tab exists
                                If (XPNav.GetAttribute("visible", String.Empty) = "false") OrElse
                                    (XPNav.GetAttribute("visible", String.Empty) = "" AndAlso bIsHidden) OrElse
                                    IsCurrentPage(XPNav.GetAttribute("url", String.Empty)) Then
                                    Dim bPreviousVisible As Boolean = False
                                    While bPreviousVisible = False
                                        ''loop through Pervious Tab of Current Page- we find a tab that is not hidden, this is the Pervious tab
                                        If XPNav.MoveToPrevious() Then
                                            sThisID = XPNav.GetAttribute("id", String.Empty)
                                            bIsHidden = IIf((oTabState.ContainsKey(sThisID) AndAlso oTabState(sThisID) = False), True, False)
                                            If (XPNav.GetAttribute("visible", String.Empty) = "false") OrElse
                                                (XPNav.GetAttribute("visible", String.Empty) = "" AndAlso bIsHidden) Then
                                                bPreviousVisible = False
                                            Else
                                                If IsCurrentPage(XPNav.GetAttribute("url", String.Empty)) Then
                                                    bPreviousVisible = False
                                                Else
                                                    bPreviousVisible = True
                                                    sPreviousTab = sFolder & "/" & XPNav.GetAttribute("url", String.Empty)
                                                End If
                                            End If
                                        Else
                                            bPreviousVisible = True
                                            sPreviousTab = sParentTab
                                        End If
                                    End While
                                Else
                                    If XPNav.MoveToPrevious Then
                                        sPreviousTab = sFolder & "/" & XPNav.GetAttribute("url", String.Empty)
                                    Else
                                        sPreviousTab = sParentTab
                                    End If
                                End If
                            Else
                                sPreviousTab = sParentTab
                            End If

                        Else
                            'End, so return to perils next
                            sNextTab = sParentTab
                            bIsHidden = False

                            ''Last tab is active, so no next
                            Dim bPreviousVisible As Boolean = False
                            Dim sThisID As String
                            While bPreviousVisible = False
                                ''loop through Pervious Tab of Current Page- we find a tab that is not hidden, this is the Pervious tab
                                If XPNav.MoveToPrevious() Then
                                    sThisID = XPNav.GetAttribute("id", String.Empty)
                                    bIsHidden = IIf((oTabState.ContainsKey(sThisID) AndAlso oTabState(sThisID) = False), True, False)

                                    'Previous tab exists
                                    If (XPNav.GetAttribute("visible", String.Empty) = "false") OrElse
                                        (XPNav.GetAttribute("visible", String.Empty) = "" AndAlso bIsHidden) Then
                                        bPreviousVisible = False
                                    Else
                                        If IsCurrentPage(XPNav.GetAttribute("url", String.Empty)) Then
                                            bPreviousVisible = False
                                        Else
                                            bPreviousVisible = True
                                            sPreviousTab = sFolder & "/" & XPNav.GetAttribute("url", String.Empty)
                                        End If
                                    End If
                                Else
                                    bPreviousVisible = True
                                    sPreviousTab = sParentTab
                                End If
                            End While
                        End If

                    Else
                        ltListItemStartTag.Text &= ">"
                        If Session.Item(CNRiskProgress) Is Nothing Then
                            If Session(CNMode) = Mode.NewClaim AndAlso Current.Session(CNHighestPageNumber) IsNot Nothing Then
                                SetRiskProgress(sParentTabID, CType(Current.Session(CNHighestPageNumber), Integer))
                            Else
                                SetRiskProgress(sParentTabID, 0)
                            End If
                        Else
                            'if tab is further along than current progress, then disable tab
                            If e.Item.ItemIndex > CType(Session.Item(CNRiskProgress), Hashtable)(sParentTabID) _
                           AndAlso Not (Session(CNMode) = Mode.EditClaim Or Session(CNMode) = Mode.ViewClaim Or Session(CNMode) = Mode.PayClaim) Then
                                lnkTab.CssClass = sDisabledClass
                                lnkTab.Enabled = False
                            Else

                                lnkTab.CommandArgument = sFolder & "/" & XPNav.GetAttribute("url", String.Empty)
                                AddHandler lnkTab.Click, AddressOf TabClick
                            End If
                        End If

                    End If
            End Select

        End Sub



        ''' <summary>
        ''' Handles the linkbutton click, which in turn raises the TabClicked event
        ''' to be handle further up on the stack
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub TabClick(ByVal sender As Object, ByVal e As System.EventArgs)

            RaiseEvent TabClicked(CType(sender, LinkButton).CommandArgument)

        End Sub

    End Class

End Namespace