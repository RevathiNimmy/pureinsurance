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
    Public Class TabIndex : Inherits UserControl

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
        Private bScrollable As Boolean = True

        Private sNextTab As String = Nothing
        Private sPreviousTab As String = Nothing
        Private sParentTab As String = Nothing
        Private sMainDetails As String = Nothing
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
        Public Event ValuesInitialized(ByVal ScreenCode As String,
                                        ByVal Depth As Integer,
                                        ByVal ParentTab As String,
                                        ByVal PrevPage As String,
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
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            AddHandler rptrTabs.ItemDataBound, AddressOf ItemBound
            Dim oRiskType As NexusProvider.RiskType = Session(CNRiskType)
            Dim oNexusConfig As NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), NexusFrameWork)
            Dim sProductPath() As String
            sProductPath = CStr(Request.ApplicationPath & "/" & oNexusConfig.ProductsFolder) _
                        .Split(Regex.Split("/", ""), StringSplitOptions.RemoveEmptyEntries)

            Dim oProductConfig As Product = oNexusConfig.Portals.Portal(GetPortalID()).Products.GetProductByName(Server.UrlDecode(Request.Url.Segments(
                        sProductPath.Length + 1).TrimEnd("/")))

            If String.IsNullOrEmpty(oProductConfig.Name) And oQuote IsNot Nothing Then
                oProductConfig = oNexusConfig.Portals.Portal(GetPortalID()).Products.Product(oQuote.ProductCode)
            End If

            sFolder = AppSettings("WebRoot") & oNexusConfig.ProductsFolder & "/" & oProductConfig.Name & "/" & oRiskType.Path

            iNestedPrefix = sFolder.Split("/").Length

            Dim sXMLPath As String = Server.MapPath(sFolder & "\")

            Select Case CType(Session.Item(CNQuoteMode), QuoteMode)
                Case QuoteMode.QuickQuote
                    sXMLPath = sXMLPath & oProductConfig.QuickQuoteConfig
                Case QuoteMode.FullQuote
                    sXMLPath = sXMLPath & oProductConfig.FullQuoteConfig
                Case QuoteMode.MTAQuote ''added by sbhatia on dated 05-march
                    sXMLPath = sXMLPath & oProductConfig.FullQuoteConfig
                Case QuoteMode.ReQuote ''added by sbhatia on dated 05-march
                    sXMLPath = sXMLPath & oProductConfig.FullQuoteConfig
            End Select

            If IO.File.Exists(sXMLPath) Then

                Dim xmlds As New XmlDataSource
                xmlds.DataFile = sXMLPath
                xmlds.EnableCaching = False

                Dim Navigator As XPathNavigator
                Dim Doc As XPathDocument = New XPathDocument(sXMLPath)
                Navigator = Doc.CreateNavigator()
                Dim i As XPathNodeIterator

                'Find the number of directories deep we are past the product root
                Dim iNestedDirectories As Integer
                'in page is place in portal and page needs the product pages link
                If Current.Request.Url.AbsoluteUri.ToUpper.Contains("/PORTAL/") = True Then
                    iNestedDirectories = -2
                Else
                    iNestedDirectories = (Current.Request.Url.Segments.Length - 1) - iNestedPrefix
                End If

                '//tab[@url = 'CHLDCLAIMS_ClaimsDetails.aspx']/preceding-sibling::tab | //tab[@url = 'CHLDCLAIMS_ClaimsDetails.aspx'] | //tab[@url = 'CHLDCLAIMS_ClaimsDetails.aspx']/following-sibling::tab

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
                    sMainDetails = i.Current.GetAttribute("maindetails", String.Empty)
                End While

                If String.IsNullOrEmpty(sParentTab) Then
                    'No parent, as we are at the top level, so when we get to
                    'the last screen we need to get out of the risk process
                    sParentTab = String.Empty
                ElseIf sMainDetails IsNot Nothing And sMainDetails.Trim.Length > 0 Then
                    sParentTab = AppSettings("WebRoot") & oNexusConfig.ProductsFolder & "/" & oProductConfig.Name & "/" & sParentTab
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
                    rptrTabs.DataSource = xmlds
                    rptrTabs.DataBind()
                Else
                    Dim oTabState As Hashtable = CType(Session.Item(CNTabState & "_" & ID), Hashtable)
                    'todo - the logic below will not work if we hide then show then hide a tab 
                    'client side as the id will be in both hidden fields, but inpVisibleTabs is read second
                    'can we modify the client script to remove entries?
                    Dim oTempTabState As Hashtable
                    Dim sHiddenTabs() As String
                    Dim sVisibleTabs() As String

                    oTempTabState = oTabState.Clone()
                    If Current.Request.Form(UniqueID & "$inpHiddenTabs") IsNot Nothing Then
                        sHiddenTabs = Current.Request.Form(UniqueID & "$inpHiddenTabs").Split(Regex.Split(", ", " "), StringSplitOptions.RemoveEmptyEntries)

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
                        sVisibleTabs = Current.Request.Form(UniqueID & "$inpVisibleTabs").Split(Regex.Split(", ", " "), StringSplitOptions.RemoveEmptyEntries)
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

                    i = Navigator.Select("//tab")

                    If Not (i Is Nothing) Then
                            While (i.MoveNext)
                                Dim sThisID As String = i.Current.GetAttribute("id", String.Empty)

                                If Not oTabState.ContainsKey(sThisID) AndAlso i.Current.GetAttribute("visible", String.Empty).Trim.ToLower = "false" Then
                                    oTabState.Add(sThisID, False)
                                End If

                                End While
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

            Dim sbJavaFunctions As New StringBuilder

            sbJavaFunctions.Append("sTabIndexID = '" & ClientID & "';" & vbCr)
            sbJavaFunctions.Append("ctrlInputHiddenTabs = '" & inpHiddenTabs.ClientID & "';" & vbCr)
            sbJavaFunctions.Append("ctrlInputVisibleTabs = '" & inpVisibleTabs.ClientID & "';" & vbCr)

            If bScrollable Then
                sbJavaFunctions.Append("document.getElementById('" & Me.ClientID & "_hypLeftArrow').style.display = '';" & vbCr)
                sbJavaFunctions.Append("document.getElementById('" & Me.ClientID & "_hypRightArrow').style.display = '';" & vbCr & vbCr)
            End If

            Page.ClientScript.RegisterClientScriptInclude("TabIndex", AppSettings("WebRoot") & "js/TabIndex.js")
            Page.ClientScript.RegisterStartupScript(Me.GetType, "TabIndexVariables", sbJavaFunctions.ToString, True)

        End Sub

        Private Sub ItemBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)

            Select Case e.Item.ItemType
                Case ListItemType.Item, ListItemType.AlternatingItem

                    Dim XPNav As XPathNavigator = CType(e.Item.DataItem, IXPathNavigable).CreateNavigator
                    Dim lnkTab As LinkButton = CType(e.Item.FindControl("lnkTab"), LinkButton)
                    Dim ltListItemStartTag As Literal = CType(e.Item.FindControl("ltListItemStartTag"), Literal)
                    Dim ltListItemEndTag As Literal = CType(e.Item.FindControl("ltListItemEndTag"), Literal)

                    lnkTab.Text = XPNav.GetAttribute("name", String.Empty)

                    'If MainDetails Page is not available then Link should also not available
                    If XPNav.GetAttribute("maindetails", String.Empty).ToUpper <> "TRUE" And lnkTab.Text.Trim.ToLower = "maindetails" Then
                        lnkTab.Enabled = False
                    End If
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
                        'This is the current tab, set risk progress to current ... if its the furthest we've gone
                        SetRiskProgress(sParentTabID, e.Item.ItemIndex)

                        'lnkTab.CssClass = sActiveTabClass

                        'store current page id so we can get back
                        Dim sCurrentID As String = sFolder & "/" & XPNav.GetAttribute("id", String.Empty)

                        'get the tabstate from session, this holds any hidden tab ids
                        Dim oTabState As Hashtable = CType(Session.Item(CNTabState & "_" & ID), Hashtable)

                        'loop through till we find a tab that is not hidden, this is the next tab
                        While (XPNav.MoveToNext)

                            Dim sThisID As String = XPNav.GetAttribute("id", String.Empty)
                            If oTabState.ContainsKey(sThisID) Then
                                If oTabState(sThisID) Then
                                    'current tab is stored, but it's visible so set this as next tab and exit loop
                                    sNextTab = sFolder & "/" & XPNav.GetAttribute("url", String.Empty)
                                    Exit While
                                End If
                            Else
                                'it's not even in the hashtable so can't be hidden
                                'set next tab and exit loop
                                ' sNextTab = ""
                                'If XPNav.GetAttribute("url", String.Empty).Trim <> "" AndAlso XPNav.GetAttribute("visible", String.Empty).Trim.ToLower <> "false" Then
                                sNextTab = sFolder & "/" & XPNav.GetAttribute("url", String.Empty)
                                ' End If
                                Exit While
                            End If
                        End While

                        If sFolder & "/" & XPNav.GetAttribute("id", String.Empty) = sCurrentID Then
                            'we didn't do anywhere, so there's no tab after this.
                            'set the next tab to the parent
                            sNextTab = sParentTab

                        Else
                            'go back till we get to the start point
                            Do While XPNav.MoveToPrevious
                                If sFolder & "/" & XPNav.GetAttribute("id", String.Empty) = sCurrentID Then
                                    'we've got back to the current tab
                                    Exit Do
                                End If
                            Loop
                        End If

                        If (sNextTab Is Nothing AndAlso sParentTab <> "") Then
                            sNextTab = sParentTab
                            If sFolder & "/" & XPNav.GetAttribute("id", String.Empty) <> sCurrentID Then
                                'go back till we get to the start point
                                Do While XPNav.MoveToPrevious
                                    If sFolder & "/" & XPNav.GetAttribute("id", String.Empty) = sCurrentID Then
                                        'we've got back to the current tab
                                        Exit Do
                                    End If
                                Loop
                            End If
                        End If

                        'loop back till we find a tab that is not hidden, this is the previous tab
                        Do While XPNav.MoveToPrevious
                            Dim sThisID As String = XPNav.GetAttribute("id", String.Empty)
                            If oTabState.ContainsKey(sThisID) Then
                                If oTabState(sThisID) Then
                                    'current tab is stored, but it's visible so set this as next tab and exit loop
                                    sPreviousTab = sFolder & "/" & XPNav.GetAttribute("url", String.Empty)
                                    Exit Do
                                End If
                            Else
                                'it's not even in the hashtable so can't be hidden
                                'set previous tab and exit loop
                                sPreviousTab = sFolder & "/" & XPNav.GetAttribute("url", String.Empty)
                                Exit Do
                            End If
                        Loop

                        If sFolder & "/" & XPNav.GetAttribute("id", String.Empty) = sCurrentID Then
                            'we didn't do anywhere, so there's no tab before this.
                            'set the next tab to the parent
                            sPreviousTab = sParentTab
                        End If

                        'If XPNav.MoveToPrevious Then
                        '    'Previous tab exists
                        '    sPreviousTab = sFolder & "/" & XPNav.GetAttribute("url", String.Empty)
                        'Else
                        '    sPreviousTab = sParentTab
                        'End If

                        'todo - check if next tab is hidden. add do while loop
                        'If XPNav.MoveToNext Then
                        '    'Next tab exists
                        '    sNextTab = sFolder & "/" & XPNav.GetAttribute("url", String.Empty)
                        '    XPNav.MoveToPrevious()


                        '    If XPNav.MoveToPrevious Then
                        '        'Previous tab exists
                        '        sPreviousTab = sFolder & "/" & XPNav.GetAttribute("url", String.Empty)
                        '    Else
                        '        sPreviousTab = sParentTab
                        '    End If
                        'Else
                        '    'End, so return to perils next
                        '    sNextTab = sParentTab

                        '    'Last tab is active, so no next
                        '    If XPNav.MoveToPrevious Then
                        '        'Previous tab exists
                        '        sPreviousTab = sFolder & "/" & XPNav.GetAttribute("url", String.Empty)
                        '    Else
                        '        sPreviousTab = sParentTab
                        '    End If
                        'End If

                    Else

                        If Session.Item(CNRiskProgress) Is Nothing Then
                            'No progress in session .. create entry for current level
                            SetRiskProgress(sParentTabID, 0)
                            ltListItemStartTag.Text &= ">"
                        Else
                            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                            Dim bNexusQuoteStatus As Boolean
                            If oQuote IsNot Nothing Then
                                bNexusQuoteStatus = NexusQuoteStatus(oQuote.Risks(Session(CNCurrentRiskKey)))
                            Else
                                bNexusQuoteStatus = False
                            End If

                            'if tab is further along than current progress, then disable tab
                            If (e.Item.ItemIndex > CType(Session.Item(CNRiskProgress), Hashtable)(sParentTabID)) And (bNexusQuoteStatus = False) Then
                                'If risk is already quoted then all tabs should be enabled
                                'lnkTab.CssClass = sDisabledClass
                                ltListItemStartTag.Text &= " class='" + sDisabledClass + "'>"

                            Else
                                ltListItemStartTag.Text &= ">"
                                If XPNav.GetAttribute("maindetails", String.Empty).ToUpper = "TRUE" Then
                                    Dim oNexusConfig As NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), NexusFrameWork)
                                    Dim sProductPath() As String
                                    Dim sFolder As String
                                    sProductPath = CStr(Request.ApplicationPath & "/" & oNexusConfig.ProductsFolder) _
                                                .Split(Regex.Split("/", ""), StringSplitOptions.RemoveEmptyEntries)

                                    Dim oProductConfig As Product = oNexusConfig.Portals.Portal(GetPortalID()).Products.GetProductByName(Server.UrlDecode(Request.Url.Segments(
                                                sProductPath.Length + 1).TrimEnd("/")))

                                    If String.IsNullOrEmpty(oProductConfig.Name) And oQuote IsNot Nothing Then
                                        oProductConfig = oNexusConfig.Portals.Portal(GetPortalID()).Products.Product(oQuote.ProductCode)
                                    End If

                                    sFolder = AppSettings("WebRoot") & oNexusConfig.ProductsFolder & "/" & oProductConfig.Name
                                    lnkTab.CommandArgument = sFolder & "/" & XPNav.GetAttribute("url", String.Empty)
                                    AddHandler lnkTab.Click, AddressOf TabClick
                                Else
                                    lnkTab.CommandArgument = sFolder & "/" & XPNav.GetAttribute("url", String.Empty)
                                    AddHandler lnkTab.Click, AddressOf TabClick
                                End If

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
