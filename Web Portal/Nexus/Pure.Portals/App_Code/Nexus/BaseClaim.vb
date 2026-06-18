Imports Nexus.Utils
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports CMS.Library
Imports System.Xml.XmlReader
Imports System.Xml.XPath
Imports System.Xml
Imports System.Web.HttpContext
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports System.Resources

Namespace Nexus

    Public MustInherit Class BaseClaim : Inherits CMS.Library.Frontend.clsCMSPage

#Region " Private Fields "
        Private oMaster As ContentPlaceHolder

        Private oOI As Collections.Stack

        Private oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)

        Private sTabIndexControlID As String = "TabIndex"
        Private sScreenCode As String
        Private iDepth As Integer

        Private sNextPage As String
        Private sPrevPage As String
        Private sParentTab As String
        Dim oResource As ResXResourceReader
        Dim en As IDictionaryEnumerator
#End Region

#Region " Property Variables "
        ''' <summary>
        ''' Property to identify the TabIndexControl, as the TabIndex determines the location within
        ''' the risk dataset. This only needs be set if the TabIndex is not labelled 'ctrlTabIndex'
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TabIndexControlID() As String
            Get
                Return sTabIndexControlID
            End Get
            Set(ByVal value As String)
                sTabIndexControlID = value
            End Set
        End Property

        ''' <summary>
        ''' Allows the BranchCode to be returned without having an knowledge of the Nexus config
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property BranchCode() As String
            Get
                Return oNexusConfig.BranchCode
            End Get
        End Property

#End Region

#Region " Page Events "

        Private Shadows Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

            If Not IsPostBack Then
                Dim bConfigureQuote As Boolean = True
                'clear return url if not empty
                Session.Remove(CNPerilReturnURL)
                If Session(CNQuoteInSync) Is Nothing Or Not TypeOf Session(CNQuoteInSync) Is Boolean Then
                    'Reset Quote
                    Session.Remove(CNDataModelCode)
                    Session.Remove(CNQuote)
                    Session.Remove(CNRiskProgress)
                    Session.Remove(CNTabState)
                    If Session(CNReturnURL) Is Nothing Then
                        Session.Remove(CNTabState & "_" & sTabIndexControlID)
                    End If
                Else
                    If CType(Session(CNQuoteInSync), Boolean) Then
                        'Everything is fine with the quote so continue
                        bConfigureQuote = False
                    Else
                        'Quote maybe invalid so check
                    End If
                End If

                If bConfigureQuote Then
                    'Read DataModelCode from DataSet if it's not already in session
                    Dim sDataModelCode As String = String.Empty
                    Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())

                    If Current.Session.Item(CNDataModelCode) Is Nothing AndAlso Current.Session(CNDataSet) IsNot Nothing Then

                        Dim Doc As XPathDocument = New XPathDocument(New IO.StringReader(Current.Session(CNDataSet)))
                        Dim Navigator As XPathNavigator
                        Navigator = Doc.CreateNavigator()

                        Dim i As XPathNodeIterator = Navigator.Select("DATA_SET")

                        While (i.MoveNext)
                            sDataModelCode = i.Current.GetAttribute("DataModelCode", String.Empty)
                        End While

                        Current.Session.Item(CNDataModelCode) = sDataModelCode
                    Else

                        sDataModelCode = Current.Session.Item(CNDataModelCode)
                    End If


                    If Session(CNClaim) IsNot Nothing Then

                        If Not String.Equals(sDataModelCode, Session(CNDataModelCode)) Then
                            Dim oClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                            Dim sFolder As String = "~/Claims/ClientPages/" & oPortal.Claims.ScreenLocation & "/Claims/" _
                                & oPortal.Claims.RiskTypes.RiskType(Trim(oClaim.RiskType)).Folder
                            Dim sXMLPath As String = Server.MapPath(sFolder & "/claimscreens.config")

                            If IO.File.Exists(sXMLPath) Then
                                Dim xmlds As New XmlDataSource
                                xmlds.DataFile = sXMLPath
                                xmlds.EnableCaching = False

                                Dim Navigator As XPathNavigator
                                Dim Doc As XPathDocument = New XPathDocument(sXMLPath)
                                Navigator = Doc.CreateNavigator()
                                Dim i As XPathNodeIterator

                                i = Navigator.Select("/screens/screen/tab[1]")

                                Dim sFirstPage As String = String.Empty

                                While (i.MoveNext)
                                    sFirstPage = i.Current.GetAttribute("url", String.Empty)
                                End While

                                'We have check whether this condition is required or not
                                If Not IsCurrentPage(sFirstPage) Then

                                    'We're not on the first risk screen and we really should be

                                    'stops all the processing being down again on then correct
                                    'risks screen, as we've already collected all the info we need
                                    Session(CNQuoteInSync) = True
                                    Response.Redirect(sFolder & "/" & sFirstPage, False)
                                End If
                            End If
                        End If
                    End If
                End If
            End If

            Session(CNQuoteInSync) = False

            oMaster = GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName)

            Dim oTabIndex As ImprovedTabIndex = CType(oMaster.FindControl(sTabIndexControlID), ImprovedTabIndex)

            If oTabIndex IsNot Nothing Then

                AddHandler oTabIndex.ValuesInitialized, AddressOf TabIndexInitialized
                AddHandler oTabIndex.TabClicked, AddressOf TabClick

                If CType(Session.Item(CNMode), Mode) = Mode.ViewClaim _
                        OrElse CType(Session.Item(CNMode), Mode) = Mode.Review _
                        OrElse CType(Session.Item(CNMode), Mode) = Mode.ViewClaimPayment _
                        OrElse CType(Session.Item(CNMode), Mode) = Mode.Authorise _
                        OrElse CType(Session.Item(CNMode), Mode) = Mode.Recommend _
                        OrElse CType(Session.Item(CNMode), Mode) = Mode.DeclinePayment Then
                    DisableControls(oMaster)
                End If
            End If

            Dim oFrameWorkNav As FrameWorkNav = oMaster.FindControl("ClaimNav")

            If oFrameWorkNav IsNot Nothing Then
                If CType(Session.Item(CNMode), Mode) <> Mode.NewClaim Then
                    If HttpContext.Current.Session.IsCookieless Then
                        oFrameWorkNav.AddLink("Search Results", "../Claims/FindClaim.aspx") 'Resource File!
                        oFrameWorkNav.AddLink("Claim Overview", "../Claims/overview.aspx") 'Resource File!
                    Else
                        oFrameWorkNav.AddLink("Search Results", AppSettings("WebRoot") & "Claims/FindClaim.aspx") 'Resource File!
                        oFrameWorkNav.AddLink("Claim Overview", AppSettings("WebRoot") & "Claims/overview.aspx") 'Resource File!
                    End If

                End If

                If CType(Session.Item(CNMode), Mode) <> Mode.NewClaim Then

                    If CType(Session.Item(CNMode), Mode) = Mode.ViewClaim _
                        OrElse CType(Session.Item(CNMode), Mode) = Mode.Review _
                        OrElse CType(Session.Item(CNMode), Mode) = Mode.ViewClaimPayment _
                        OrElse CType(Session.Item(CNMode), Mode) = Mode.Authorise _
                        OrElse CType(Session.Item(CNMode), Mode) = Mode.Recommend _
                        OrElse CType(Session.Item(CNMode), Mode) = Mode.DeclinePayment Then
                        DisableControls(oMaster)
                    End If

                    oFrameWorkNav.AddLink("Summary", AppSettings("WebRoot") & "Claims/summary.aspx") 'Resource File!

                End If
            End If


            Dim Portal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim sDisplayReinsurance As String
            Dim hidChkCBuilderClaimClose As New HiddenField
            Dim hidChkCBuilderChoice As New HiddenField
            Dim hidChkCBuilderPaymentMsg As New HiddenField

            'Initialized all claim builder hidden control 
            hidChkCBuilderChoice.ID = "hidChkCBuilderChoice"
            hidChkCBuilderClaimClose.ID = "hidChkCBuilderClaimClose"
            hidChkCBuilderPaymentMsg.ID = "hidChkCBuilderPaymentMsg"

            oMaster = GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName)
            For Each oControl In oMaster.Controls
                'check whether controls "claimsprogressbar.ascx" exist on this page
                If oControl.GetType.Name.Contains("controls_claimsprogressbar_ascx") Then
                    Dim ophTmp As PlaceHolder = CType(oControl.FindControl("phTmp"), PlaceHolder)
                    If ophTmp IsNot Nothing Then
                        ophTmp.Controls.Add(hidChkCBuilderChoice)
                        ophTmp.Controls.Add(hidChkCBuilderClaimClose)
                        ophTmp.Controls.Add(hidChkCBuilderPaymentMsg)
                        Exit For
                    End If
                End If
            Next

            'if ShowSummary & claimReinsurance are OFF then we need to ensure that on the last claim builder should appear below messages
            oResource = New ResXResourceReader(HttpContext.Current.Server.MapPath(AppSettings("WebRoot") & "Claims/App_LocalResources/Summary.aspx.resx"))
            en = oResource.GetEnumerator()
            oMaster = GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName)

            'Get "Display Re-Insurance” option from Risk type Maintenance” 
            sDisplayReinsurance = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.RiskTypeMaintenance, NexusProvider.ProductRiskOptions.Description, NexusProvider.RiskTypeOptions.DisplayClaimReinsurance, CType(Session(CNClaimQuote), NexusProvider.Quote).ProductCode, CType(Session(CNClaim), NexusProvider.ClaimOpen).RiskType)

            If Portal.Claims.ShowSummary = False Then
                'Get "Display Re-Insurance” option from User Authority”  
                Dim oUserAuthority As New NexusProvider.UserAuthority
                oUserAuthority.UserCode = Session(CNLoginName)
                oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.DisplayClaimReinsurance
                oWebservice = New NexusProvider.ProviderManager().Provider
                oWebservice.GetUserAuthorityValue(oUserAuthority)

                'Check Display Re-Insurance” option is switched off from User Authority” 
                If oUserAuthority.UserAuthorityValue = "0" Or sDisplayReinsurance = "0" Or oUserAuthority.UserAuthorityValue Is Nothing Then
                    While (en.MoveNext)
                        If en.Key.ToString.Trim = "msg_AnotherPayment" Then
                            'User should be prompted with these message in case of PayClaim if user present on last claim builder screen for further payment
                            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "FurtherPaymentsConfirmation",
                           "<script language=""JavaScript"" type=""text/javascript"">function FurtherPaymentsConfirmation(){var r= confirm('" & en.Value & "'); document.getElementById('" & hidChkCBuilderChoice.ClientID & "').value=r; }</script>")
                        End If
                        If en.Key.ToString.Trim = "msg_CloseClaim" Then
                            ''User should be prompted with these message in case of PayClaim if user present on last claim builder screen for close claim
                            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "OnCliamBuilderClaimCloseConfirmation",
                                                                        "<script language=""JavaScript"" type=""text/javascript"">function OnCliamBuilderClaimCloseConfirmation(){var r= confirm('" & en.Value & "'); document.getElementById('" & hidChkCBuilderClaimClose.ClientID & "').value=r; if (document.getElementById('" & hidChkCBuilderPaymentMsg.ClientID & "').value ==  '0' && r == false) {FurtherPaymentsConfirmation();}}</script>")
                        End If
                    End While
                End If
            End If

            Response.Cache.SetCacheability(HttpCacheability.NoCache)
        End Sub
        ''' <summary>
        ''' Page Load
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'For authorise,decline and view claim payment, user need to redirected on payment detail page directly
            ' Start Fix for PURE-4457
            If (Session("FromRiskPage") = True) Then
                Session("FromRiskPage") = Nothing
                Session(CNOI) = Session(CNCurrentOI)
            End If
            ' End

            If (CType(Session(CNMode), Mode) = Mode.Authorise OrElse CType(Session(CNMode), Mode) = Mode.DeclinePayment OrElse
                CType(Session(CNMode), Mode) = Mode.Recommend OrElse CType(Session(CNMode), Mode) = Mode.ViewClaimPayment) AndAlso oMaster.FindControl("Work_Claim_Peril") IsNot Nothing Then

                Dim PerilsIndex As New System.Collections.Generic.List(Of Integer)

                Dim sPerilTypeCode As String = Nothing
                Dim nPerilKey As Integer
                Dim nPerilIndex As Integer
                Dim bFoundPayment As Boolean
                Dim oClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                For lCount As Integer = 0 To oClaim.ClaimPeril.Count - 1
                    For lInnerCount As Integer = 0 To oClaim.ClaimPeril(lCount).ClaimPayment.Count - 1
                        If Session(CNClaimPaymentKey) = oClaim.ClaimPeril(lCount).ClaimPayment(lInnerCount).BaseClaimPaymentKey Then
                            sPerilTypeCode = oClaim.ClaimPeril(lCount).TypeCode
                            nPerilKey = oClaim.ClaimPeril(lCount).ClaimPerilKey
                            If (Session(CNClaimPerilIndex) Is Nothing AndAlso Session(CNClaimMultiPerilIndex) IsNot Nothing) Then
                                PerilsIndex.Add(lCount)
                            Else
                                bFoundPayment = True
                                Exit For
                            End If
                        End If
                    Next
                    If bFoundPayment Then
                        nPerilIndex = lCount
                        Session(CNClaimPerilIndex) = nPerilIndex
                        Exit For
                    End If
                Next

                If PerilsIndex.Count > 0 Then
                    Session(CNClaimMultiPerilIndex) = PerilsIndex
                End If

                If Session(CNClaimBuilder) = True Then
                    Dim WebRoot As String = AppSettings("WebRoot")
                    Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
                    'Check Peril Builder
                    Dim sConfigFile As String
                    Dim sFolder As String
                    sFolder = String.Empty

                    If oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.PerilTypes.PerilType(Trim(sPerilTypeCode)) IsNot Nothing Then
                        sFolder = "~/Claims/ClientPages/" & oPortal.Claims.ScreenLocation & "/Perils/" _
                                            & oPortal.Claims.PerilTypes.PerilType(sPerilTypeCode).Folder
                    End If

                    sConfigFile = sFolder & "/perilscreens.config"

                    If System.IO.File.Exists(Server.MapPath(sConfigFile)) = True Then
                        Dim sXMLPath As String = Server.MapPath(sFolder & "/perilscreens.config")
                        If IO.File.Exists(sXMLPath) = True Then

                            Dim xmlds As New XmlDataSource
                            xmlds.DataFile = sXMLPath
                            xmlds.EnableCaching = False

                            Dim Navigator As XPathNavigator
                            Dim Doc As XPathDocument = New XPathDocument(sXMLPath)
                            Navigator = Doc.CreateNavigator()
                            Dim iSrc As XPathNodeIterator

                            iSrc = Navigator.Select("/screens/screen/tab[1]")
                            Dim sFirstPage As String = String.Empty
                            While (iSrc.MoveNext)
                                sFirstPage = iSrc.Current.GetAttribute("url", String.Empty)
                            End While

                            Dim iTab As XPathNodeIterator
                            Dim sPaymentScreen As String = String.Empty
                            Dim sURL As String = String.Empty

                            iSrc = Navigator.Select("/screens/screen")
                            While (iSrc.MoveNext)
                                iTab = Navigator.Select("/screens/screen/tab")
                                While (iTab.MoveNext)
                                    sPaymentScreen = iTab.Current.GetAttribute("paymentscreen", String.Empty)
                                    sURL = iTab.Current.GetAttribute("url", String.Empty)
                                    If sPaymentScreen.ToLower = "true" Then
                                        Exit While
                                    End If
                                End While
                                If sPaymentScreen.ToLower = "true" Then
                                    Exit While
                                End If
                            End While

                            If sPaymentScreen.ToLower = "true" Then
                                'We have check whether this condition is required or not
                                If Not IsCurrentPage(sURL) Then

                                    'We're not on the first risk screen and we really should be

                                    'stops all the processing being down again on then correct
                                    'risks screen, as we've already collected all the info we need
                                    Session(CNQuoteInSync) = True

                                    Response.Redirect(sFolder & "/" & sURL & "?FromPage=Overview&PerilID=" & nPerilKey & "&PerilIndex=" & nPerilIndex & "&ReturnUrl=" & Request.Path.Replace(WebRoot, "~/"))

                                End If
                            Else
                                'We have check whether this condition is required or not
                                If Not IsCurrentPage(sFirstPage) Then

                                    'We're not on the first risk screen and we really should be

                                    'stops all the processing being down again on then correct
                                    'risks screen, as we've already collected all the info we need
                                    Session(CNQuoteInSync) = True

                                    Response.Redirect(sFolder & "/" & sFirstPage & "?FromPage=Overview&PerilID=" & nPerilKey & "&PerilIndex=" & nPerilIndex & "&ReturnUrl=" & Request.Path.Replace(WebRoot, "~/"))

                                End If
                            End If
                        End If
                    End If
                Else
                    Response.Redirect("~/Claims/PerilDetails.aspx?FromPage=Overview&PerilID=" & nPerilKey & "&PerilIndex=" & nPerilIndex & "&ReturnUrl=" & Request.Path.Replace(WebRoot, "~/"))
                End If
            End If

            oOI = Session.Item(CNOI)

            'Depth should be equal to number of OI's in the stack, so remove any additional ones from the end
            'gonna have problems if stack length is shorter then depth, can deal with length zero
            If oOI Is Nothing Then
                oOI = New Collections.Stack()
            Else
                While oOI.Count > iDepth + 1

                    'we've moved back up the dataset tree from child to parent, so we need to check

                    Dim srDataset As New System.IO.StringReader(Session(CNDataSet))
                    Dim xmlTR As New XmlTextReader(srDataset)
                    Dim Doc As New XmlDocument
                    Doc.Load(xmlTR)
                    xmlTR.Close()

                    Dim oNode As XmlNode = Doc.SelectSingleNode("//*[@OI='" & oOI.Peek.ToString() & "' and @US='3']")
                    srDataset.Dispose()

                    If oNode Is Nothing Then
                        oNode = Doc.SelectSingleNode("//*[@OI='" & oOI.Peek.ToString() & "' and @NODEISVALID='0']")
                    End If

                    If oNode IsNot Nothing Then
                        Dim oSAMClient As New SiriusFS.SAM.Client.DataSetControl.Application
                        oSAMClient.LoadFromXML(ClaimGetDataSetDefinition(), Session(CNDataSet))
                        oSAMClient.DelObjectInstance(oNode.Name, oOI.Peek.ToString())
                        oSAMClient.ReturnAsXML(Session(CNDataSet))
                        oSAMClient.Terminate()
                    End If
                    oOI.Pop()
                End While
            End If

            Session.Item(CNOI) = oOI

            If IsPostBack Then
                If oOI.Count > 0 Then
                    DataSetFunctions.ReadContainerFromXML(oMaster, oOI.Peek, Me, True)
                Else
                    'To Delete the child added into Dataset Abnormally
                    DataSetFunctions.DeleteElementFromXML(sScreenCode, Nothing, Nothing)
                    DataSetFunctions.ReadContainerFromXML(oMaster, String.Empty, Me, True)
                End If
            Else
                'load controls from XML
                If oOI.Count > 0 Then
                    DataSetFunctions.ReadContainerFromXML(oMaster, oOI.Peek, Me)
                Else
                    'To Delete the child added into Dataset Abnormally
                    DataSetFunctions.DeleteElementFromXML(sScreenCode, Nothing, Nothing)
                    DataSetFunctions.ReadContainerFromXML(oMaster, String.Empty, Me)
                End If

                If Session(CNClaimOI) Is Nothing And Session.Item(CNOI) IsNot Nothing Then
                    Session(CNClaimOI) = Session.Item(CNOI)
                End If

                'Check ShowSummary is set to “False” at Portal->Claims level 
                Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
                If oPortal.Claims.ShowSummary = False Then
                    Dim sDisplayReinsurance As String
                    Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    sDisplayReinsurance = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.RiskTypeMaintenance, NexusProvider.ProductRiskOptions.Description, NexusProvider.RiskTypeOptions.DisplayClaimReinsurance, CType(Session(CNClaimQuote), NexusProvider.Quote).ProductCode, CType(Session(CNClaim), NexusProvider.ClaimOpen).RiskType)

                    'Get "Display Re-Insurance” option from User Authority”  
                    Dim oUserAuthority As New NexusProvider.UserAuthority
                    oUserAuthority.UserCode = Session(CNLoginName)
                    oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.DisplayClaimReinsurance
                    oWebservice = New NexusProvider.ProviderManager().Provider
                    oWebservice.GetUserAuthorityValue(oUserAuthority)


                    'Check “Display Re-Insurance” option is switched off from Risk type Maintenance and User Authority 
                    If sDisplayReinsurance = "0" Or oUserAuthority.UserAuthorityValue = "0" Or oUserAuthority.UserAuthorityValue Is Nothing Then
                        'Call the SkipSummaryPage
                        SkipSummaryPage()

                        'View Mode last screen = Cliam Builder last screen 
                        If CType(Session.Item(CNMode), Mode) = Mode.ViewClaim Then
                            oMaster = GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName)
                            Dim btnNextClmBuilder As Button = oMaster.FindControl("btnNext")
                            Dim btnFinishClmBuilder As Button = oMaster.FindControl("btnFinish")
                            Dim btnNext As LinkButton = oMaster.FindControl("btn_Next")
                            Dim btnFinish As LinkButton = oMaster.FindControl("btn_Finish")
                            If String.IsNullOrEmpty(sNextPage) Or sNextPage = sParentTab Then
                                If btnNextClmBuilder IsNot Nothing Then
                                    btnNextClmBuilder.Visible = False
                                End If
                            End If
                            If btnFinishClmBuilder IsNot Nothing Then
                                btnFinishClmBuilder.Visible = False
                            End If
                            If btnFinish IsNot Nothing Then
                                btnFinish.Visible = False
                            End If
                        End If

                    End If
                End If
            End If
        End Sub

#End Region

#Region " Private Methods "
        ''' <summary>
        ''' Handles the edit child item event from the ItemGrid when the ItemGrids is not in 'inline' mode
        ''' </summary>
        ''' <param name="v_sPath">Location of the first child page to redirect to</param>
        ''' <param name="v_sOI">Dataset child item identifier</param>
        ''' <remarks>The current form data will be written to the dataset at this point.</remarks>
        Public Sub EditItem(ByVal v_sPath As String, ByVal v_sOI As String, ByVal v_sScreenCode As String)
            Session(CNQuoteInSync) = True
            WriteClaim(Me)

            If Session(CNMode) <> Mode.View AndAlso Session(CNMode) <> Mode.Review AndAlso Session(CNMode) <> Mode.ViewClaim Then
                StorePreviousNode(v_sOI)
            End If
            oOI.Push(v_sOI)
            Session.Item(CNOI) = oOI
            Response.Redirect(v_sPath, False)
        End Sub

        ''' <summary>
        ''' Handles the edit child item event from the ItemGrid when in the ItemGrid is in 'inline' mode
        ''' </summary>
        ''' <param name="v_sRiskContainer">Control Id of the RiskContainer containing the child screen controls.</param>
        ''' <param name="v_sOI">Dataset child item identifier</param>
        ''' <remarks></remarks>
        Public Sub EditItemInRiskContainer(ByVal v_sRiskContainer As String, ByVal v_sOI As String)

            Session(CNQuoteInSync) = True

            Dim oRiskContainer As RiskContainer = oMaster.FindControl(v_sRiskContainer)

            If oRiskContainer IsNot Nothing Then
                oRiskContainer.Mode = RiskContainer.ChildMode.Edit
                oRiskContainer.OI = v_sOI
                DataSetFunctions.ReadContainerFromXML(oRiskContainer, v_sOI, Me)
            End If

        End Sub

        ''' <summary>
        ''' Handles the child item deletion event
        ''' </summary>
        ''' <param name="v_sOI">Dataset identifier of the selected child item</param>
        ''' <param name="v_sChildElement">Element name within the dataset of the child item</param>
        ''' <remarks></remarks>
        Public Sub DeleteItem(ByVal v_sOI As String, ByVal v_sChildElement As String)
            WriteClaim(Me)
            DataSetFunctions.DeleteElementFromXML(sScreenCode, v_sOI, v_sChildElement)
            DataSetFunctions.ReadContainerFromXML(oMaster, oOI.Peek.ToString(), Me)

        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="v_sScreenCode"></param>
        ''' <param name="v_sParentElement"></param>
        ''' <param name="v_sChildElement"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function LoadChildDefaultItem(ByVal v_sScreenCode As String, ByVal v_sParentElement As String, ByVal v_sChildElement As String) As String
            Dim oRiskContainer As New RiskContainer
            Dim oOI As Collections.Stack = Session(CNOI)
            oRiskContainer.OI = DataSetFunctions.CreateElementFromXML(v_sScreenCode,
                oOI.Peek, v_sParentElement, v_sChildElement)
            Return oRiskContainer.OI
        End Function
        Public Sub BackButton(ByVal sender As Object, ByVal e As System.EventArgs)

            If sPrevPage = sParentTab Then
                If Session(CNMode) <> Mode.ViewClaim And Session(CNMode) <> Mode.Review Then
                    Dim sXMLDataset As String = DirectCast(Session(CNDataSet), System.Object)
                    Dim srDataset As New System.IO.StringReader(sXMLDataset)
                    Dim xmlTRNew As New XmlTextReader(srDataset)
                    Dim Doc As New XmlDocument

                    Doc.Load(xmlTRNew)
                    xmlTRNew.Close()

                    Dim oNodes As XmlNodeList = Doc.SelectNodes("//" & Session.Item(CNDataModelCode).ToString() & "_OUTPUT[@REFER_REASON|@DECLINE_REASON]")
                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim oNode As XmlNode
                    Dim oClaimQuote As NexusProvider.Quote = Session(CNClaimQuote)

                    If oNodes IsNot Nothing Then
                        For Each oNode In oNodes
                            oOI = Session(CNOI)
                            Dim oNodefound As XmlNode = Doc.SelectSingleNode("//*[@OI='" & oOI.Peek.ToString() & "']")
                            If oNodefound IsNot Nothing Then
                                oNodefound.Attributes("US").Value = "3"
                                oNode.Attributes("US").Value = "3"
                                oNode.ParentNode.RemoveChild(oNode)
                                Dim swContent As New System.IO.StringWriter
                                Using xmlwContent As New XmlTextWriter(swContent)
                                    Doc.WriteTo(xmlwContent)
                                    sXMLDataset = swContent.ToString()
                                End Using
                            End If
                            Session(CNDataSet) = oWebService.RunDefaultRulesEdit(sScreenCode, sXMLDataset, Nothing, oClaimQuote.BranchCode)
                        Next
                    End If

                    'To Delete the latest child if user presses BACK button
                    oOI = Session(CNOI)
                    If oOI IsNot Nothing And oOI.Count > 0 Then
                        DataSetFunctions.DeleteElementFromXML(sScreenCode, oOI.Peek.ToString(), Nothing)
                    End If
                End If
            End If

            Session(CNQuoteInSync) = True

            If sPrevPage <> String.Empty Then
                'Call and override  this function if we want to Redirect to some other page
                BackButtonRedirect()
                Response.Redirect(sPrevPage, False)
            Else
                'Call and override  this function if we want to Redirect to some other page
                BackButtonRedirect()
                Response.Redirect("~/Claims/Perils.aspx", False)
            End If
        End Sub
        Private Function ValidateDataset() As String
            Dim strValidationMsg As String = String.Empty
            If Session(CNDataSet) IsNot Nothing Then
                'Code for Running XSLT Validation
                'Declaration of the Vairables used
                ' for taking the xml from session
                Dim sbOutput As New StringBuilder
                Dim xmlTR As New XmlTextReader(New System.IO.StringReader(Session(CNDataSet))) ' xml from session
                Dim xInput As New XmlDocument
                Dim oClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
                Dim sFolder As String
                Dim oClaimQuote As NexusProvider.Quote = Session(CNClaimQuote)

                If oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.RiskTypes.RiskType(Trim(oClaim.RiskType)) Is Nothing Then
                    'use the default folder If risk type is not configured
                    sFolder = "Claims/ClientPages/" & oNexusConfig.Portals.Portal(Portal.GetPortalID()).Claims.ScreenLocation & "/Claims/" _
                              & oNexusConfig.Portals.Portal(Portal.GetPortalID()).Claims.RiskTypes.DefaultFolder
                ElseIf String.IsNullOrEmpty(oNexusConfig.Portals.Portal(Portal.GetPortalID()).Claims.RiskTypes.RiskType(Trim(oClaim.RiskType)).Folder) = True Then
                    'use the default folder if folder is empty
                    sFolder = "Claims/ClientPages/" & oNexusConfig.Portals.Portal(Portal.GetPortalID()).Claims.ScreenLocation & "/Claims/" _
                              & oNexusConfig.Portals.Portal(Portal.GetPortalID()).Claims.RiskTypes.DefaultFolder
                Else
                    'we have the risk type specified so use that folder
                    sFolder = "Claims/ClientPages/" & oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.ScreenLocation & "/Claims/" _
                              & oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.RiskTypes.RiskType(Trim(oClaim.RiskType)).Folder
                End If

                If HttpContext.Current.Session.IsCookieless Then
                    sFolder = AppSettings("WebRoot") & sFolder
                Else
                    sFolder = "../" & sFolder
                End If

                Dim SClaimValidatorPath As String = Server.MapPath(sFolder) & "\" & sScreenCode & ".xslt"
                Dim iClaimKey As Integer
                'safe code to find the claim key
                If Session(CNClaimKey) IsNot Nothing Then
                    iClaimKey = CInt(Session(CNClaimKey))
                ElseIf Session(CNClaim) IsNot Nothing Then
                    iClaimKey = oClaim.ClaimKey
                End If

                'A check for validation file exist
                If (System.IO.File.Exists(SClaimValidatorPath)) Then
                    xInput.Load(xmlTR)
                    Dim xslDoc As New Xsl.XslCompiledTransform 'This should load the relevant validator file from the current product folder
                    xslDoc.Load(SClaimValidatorPath)
                    Using stream As New System.IO.MemoryStream()
                        Using xwOutput As New XmlTextWriter(stream, System.Text.Encoding.UTF8)
                            'transform xInput with xslDoc to create xOutput
                            xslDoc.Transform(xInput, xwOutput)

                            Dim xOutput As New XmlDocument
                            'reset stream to begining
                            stream.Position = 0
                            xOutput.Load(stream)

                            Dim nodes As XmlNodeList = xOutput.GetElementsByTagName("ValidationFailure")

                            If nodes.Count <> 0 Then 'no failures so return an empty string

                                'create the error message
                                Dim node As XmlNode

                                For Each node In nodes
                                    sbOutput.Append(node.InnerText)
                                    If node.NextSibling IsNot Nothing Then
                                        ' add html tags 
                                        ' the error message will already be placed in <li></li> tags 
                                        'so we just need to close and reopen the <li> tag if there is another error message to follow
                                        sbOutput.Append("</li><li>")
                                    End If

                                Next
                                'return the error message as as string
                                Return sbOutput.ToString

                            End If
                        End Using
                    End Using
                End If
                xmlTR.Close()
                'This will only run if validations  are fine on all the pages.
                'Code for Running SAM Validation Rules

                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                Dim v_sXMLDataSet As String



                v_sXMLDataSet = Session(CNDataSet)

                Try
                    oWebService.RunValidationRules(sScreenCode, v_sXMLDataSet, iClaimKey, Nothing, oClaimQuote.BranchCode)

                    Session(CNDataSet) = v_sXMLDataSet

                Catch ex As Exception
                    Throw
                End Try

                Dim srDataset As New System.IO.StringReader(v_sXMLDataSet)
                Dim xmlTRNew As New XmlTextReader(srDataset)
                Dim Doc As New XmlDocument

                Doc.Load(xmlTRNew)
                xmlTRNew.Close()


                Dim oNodes As XmlNodeList = Doc.SelectNodes("//" & Session.Item(CNDataModelCode).ToString() & "_OUTPUT[@REFER_REASON]")

                Dim oNode As XmlNode

                For Each oNode In oNodes
                    If oNode.Attributes("REFER_REASON").Value.Trim() <> "" Then
                        sbOutput.Append(oNode.Attributes("REFER_REASON").Value)
                        If oNode.NextSibling IsNot Nothing Then
                            sbOutput.Append("</li><li>")
                        End If
                    End If
                Next

                oNodes = Doc.SelectNodes("//" & Session.Item(CNDataModelCode).ToString() & "_OUTPUT[@DECLINE_REASON]")
                For Each oNode In oNodes
                    If oNode.Attributes("DECLINE_REASON").Value.Trim() <> "" Then
                        sbOutput.Append(oNode.Attributes("DECLINE_REASON").Value)
                        If oNode.NextSibling IsNot Nothing Then
                            sbOutput.Append("</li><li>")
                        End If
                    End If
                Next

                strValidationMsg = sbOutput.ToString()

                srDataset.Dispose()
            End If
            Return strValidationMsg

        End Function

        ''' <summary>
        ''' Allows the Finish button to be hidden from the user when they are entering the
        ''' details of a new risk, as at this point to must complete all pages and can not
        ''' skip to the end of the process. This event needs to manually hooked up to
        ''' the PreRender event of the Finish button.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>Using PreRender to overide any visibility set with the risk screen,
        ''' this should ensure the button can not be displayed when it shouldn't be.
        ''' we've conig option product level called "SubmitFromAnyPage".
        ''' If set to true then the finish button should not be hidden on the risk screens at any time
        ''' if the finish button is added to the page then it should always be visible</remarks>
        Public Sub PreRenderFinish(ByVal sender As Object, ByVal e As System.EventArgs)
            If CType(Session(CNMode), Mode) = Mode.Authorise Or CType(Session(CNMode), Mode) = Mode.DeclinePayment Or CType(Session(CNMode), Mode) = Mode.Recommend Or CType(Session(CNMode), Mode) = Mode.ViewClaimPayment Then
                'Finish should not be visible as user need to visit every page
                sender.visible = False
            Else

                Dim sProductCode As String = Session(CNProductCode)
                Dim bReturn As Boolean = False
                'Check for Valid product existance
                Dim oProducts As Config.Products = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Products
                For Each oProduct As Config.Product In oProducts
                    If sProductCode.Trim.ToUpper = oProduct.ProductCode.Trim.ToUpper Then
                        bReturn = True
                    End If
                Next
                If bReturn = True Then
                    'If product exist
                    Dim bSubmitFromAnyPage As Boolean = oNexusConfig.Portals.Portal(Portal.GetPortalID()).Products.Product(sProductCode).SubmitFromAnyPage
                    If bSubmitFromAnyPage Then
                        sender.visible = True
                    Else
                        sender.visible = False
                    End If
                Else
                    'if does not exist then By Default it is set True
                    sender.visible = True
                End If
            End If
        End Sub
        Public Sub NextButton(ByVal sender As Object, ByVal e As System.EventArgs)
            If Page.IsValid Then
                Dim bFindPerilControl As Boolean = False

                For Each oControl In oMaster.Controls
                    'check whether controls "perils.ascx" exist on this page
                    If oControl.GetType.Name.Equals("controls_perils_ascx") Then
                        bFindPerilControl = True
                        Exit For
                    End If
                Next
                ' Dim RequestedPageURL As String = Request.Url.Segments(Request.Url.Segments.Length - 1).ToString
                ' Dim RestrictedPageURLs As String = "perils.aspx"
                If bFindPerilControl AndAlso (CType(Session.Item(CNMode), Mode) = Mode.SalvageClaim OrElse CType(Session.Item(CNMode), Mode) = Mode.TPRecovery) Then
                    'For Salvage and TP Recover Mode
                    If HttpContext.Current.Session.IsCookieless Then
                        Response.Redirect("~/claims/changeclaim.aspx", False)
                    Else
                        Response.Redirect(AppSettings("WebRoot") & "claims/changeclaim.aspx", False)
                    End If
                Else
                    Dim sDatasetErrorMessages As String = String.Empty
                    Session(CNQuoteInSync) = True

                    If CType(Session.Item(CNMode), Mode) = Mode.NewClaim Or
                         CType(Session.Item(CNMode), Mode) = Mode.EditClaim Or
                         CType(Session.Item(CNMode), Mode) = Mode.PayClaim OrElse
                         (CType(Session.Item(CNMode), Mode) = Mode.SalvageClaim OrElse
                          CType(Session.Item(CNMode), Mode) = Mode.TPRecovery) Then
                        WriteClaim(Me)
                    End If

                    If CBool(Session(CNIsClaimLocked)) <> True Then
                        If String.IsNullOrEmpty(sNextPage) Or sNextPage = sParentTab Then
                            'Need to validate in these mode only as per BO
                            If CType(Session.Item(CNMode), Mode) = Mode.NewClaim Or CType(Session.Item(CNMode), Mode) = Mode.EditClaim Or CType(Session.Item(CNMode), Mode) = Mode.PayClaim Then
                                sDatasetErrorMessages = ValidateDataset()
                            End If

                            If sDatasetErrorMessages <> String.Empty Then
                                If Session(CNTempClaimDataSet) IsNot Nothing Then

                                    Dim srDataset As New System.IO.StringReader(Session(CNTempClaimDataSet).ToString())
                                    Dim xmlTR As New XmlTextReader(srDataset)
                                    Dim oDoc As New XmlDocument

                                    oDoc.Load(xmlTR)
                                    xmlTR.Close()
                                    Dim oOI1 As Collections.Stack
                                    oOI1 = Session(CNOI)
                                    Dim oNode As XmlNode = oDoc.SelectSingleNode("//*[@OI = '" & oOI1.Peek.ToString() & "']")

                                    'Add Mode
                                    If oNode IsNot Nothing AndAlso oNode.Attributes("US").Value = "0" Then
                                        'Edit Mode
                                        'If v_sOI IsNot Nothing AndAlso v_sParentTab IsNot Nothing AndAlso String.IsNullOrEmpty(v_sParentTab) Then
                                        '    'Parent element
                                        '    oNode.Attributes("US").Value = "2"
                                        'ElseIf v_sOI IsNot Nothing AndAlso String.IsNullOrEmpty(v_sParentTab) = False Then
                                        '    'Child element
                                        oNode.Attributes("US").Value = "2"

                                        'End If
                                    End If


                                    Dim swContent As New System.IO.StringWriter
                                    Dim xmlwContent As New XmlTextWriter(swContent)

                                    oDoc.WriteTo(xmlwContent)
                                    Session(CNDataSet) = swContent.ToString()

                                    xmlwContent.Close()
                                    swContent.Close()



                                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider


                                    Dim oPerilSummary As New NexusProvider.PerilSummary
                                    oPerilSummary.ClaimKey = Current.Session.Item(CNClaimKey)
                                    Dim oClaimQuote As NexusProvider.Quote = Current.Session(CNClaimQuote)
                                    Dim sBranchCode As String = oClaimQuote.BranchCode
                                    Current.Session.Item(CNDataSet) = oWebService.RunDefaultRulesEdit(sScreenCode, Current.Session.Item(CNDataSet), Nothing, oClaimQuote.BranchCode, nClaimPerilKey:=oPerilSummary.ClaimKey)
                                    oWebService = Nothing
                                    oPerilSummary = Nothing
                                End If
                                'create a new custom validator
                                'set it as invalid and set the error message property to the output from ValidateDataset
                                Dim cstInvalidDataset As New CustomValidator
                                cstInvalidDataset.IsValid = False
                                cstInvalidDataset.ErrorMessage = sDatasetErrorMessages
                                cstInvalidDataset.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                                Page.Validators.Add(cstInvalidDataset)
                            End If


                            If sDatasetErrorMessages = String.Empty Then
                                'no error messages returned so call UpdateQuote
                                If iDepth > 1 Then
                                    'Removal of USed OI from Hash Table
                                    If Session(CNMode) <> Mode.ViewClaim And Session(CNMode) <> Mode.Review Then
                                        Dim oUsedOI As Collections.Stack = Session(CNOI)
                                        If Session(CNNode) IsNot Nothing Then
                                            Dim hCurrentNodeColl As New Hashtable()
                                            hCurrentNodeColl = CType(Session(CNNode), Hashtable)

                                            If hCurrentNodeColl.ContainsKey(oUsedOI.Peek().ToString) Then
                                                hCurrentNodeColl.Remove(oUsedOI.Peek().ToString)
                                                Session(CNNode) = hCurrentNodeColl
                                            End If
                                        End If

                                        'Delete the usedOI from DeletedOI collection
                                        If Session(CNDeletedNode) IsNot Nothing Then
                                            Dim hDeletedNodeColl As New Hashtable()
                                            hDeletedNodeColl = CType(Session(CNDeletedNode), Hashtable)

                                            If hDeletedNodeColl.ContainsKey(oUsedOI.Peek().ToString) Then
                                                hDeletedNodeColl.Remove(oUsedOI.Peek().ToString)
                                                Session(CNDeletedNode) = hDeletedNodeColl
                                            End If
                                        End If

                                        'Remove the UC attrinbute to identify the valid/invalid node
                                        ResetUCElement(oUsedOI.Peek().ToString)
                                    End If

                                    'redirect the page if its required
                                    PrePageRedirect()
                                    Response.Redirect(sNextPage, False)
                                Else
                                    If CType(Session(CNMode), Mode) = Mode.Authorise Or CType(Session(CNMode), Mode) = Mode.DeclinePayment Or CType(Session(CNMode), Mode) = Mode.Recommend Or CType(Session(CNMode), Mode) = Mode.ViewClaimPayment Then
                                        'User should be redirected to peril builder screen, if more than one peril is available then user
                                        'should be redirected to the peril against whom payment had made.
                                        RedirectToPeril()
                                    Else
                                        If oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.ShowSummary = True Then
                                            'Call PreSummaryPageRedirect
                                            PreSummaryPageRedirect()
                                        Else
                                            'Call SkipSummaryPage
                                            SkipSummaryPage()
                                        End If
                                    End If
                                End If
                            End If
                        Else
                            'redirect the page if its required
                            PrePageRedirect()
                            Response.Redirect(sNextPage, False)
                        End If
                    End If
                End If
            End If
        End Sub
        Sub RedirectToPeril()
            If CType(Session(CNMode), Mode) = Mode.Authorise Or CType(Session(CNMode), Mode) = Mode.DeclinePayment Or CType(Session(CNMode), Mode) = Mode.Recommend Or CType(Session(CNMode), Mode) = Mode.ViewClaimPayment Then
                Dim sPerilTypeCode As String = Nothing
                Dim iPerilKey As Integer
                Dim iPerilIndex As Integer
                Dim bFoundPayment As Boolean
                Dim oClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                Dim bClaimBuilder As Boolean = False
                Boolean.TryParse(Session(CNClaimBuilder), bClaimBuilder)
                Dim PerilsIndex As New System.Collections.Generic.List(Of Integer)
                If oClaim.ClaimPeril IsNot Nothing AndAlso oClaim.ClaimPeril.Count = 1 Then
                    'select the only peril available
                    sPerilTypeCode = oClaim.ClaimPeril(0).TypeCode
                    iPerilKey = oClaim.ClaimPeril(0).ClaimPerilKey
                    iPerilIndex = 0
                    Session(CNClaimPerilIndex) = iPerilIndex

                ElseIf oClaim.ClaimPeril IsNot Nothing AndAlso oClaim.ClaimPeril.Count > 1 Then
                    'check the peril against whom payment had made
                    For lCount As Integer = 0 To oClaim.ClaimPeril.Count - 1
                        For lInnerCount As Integer = 0 To oClaim.ClaimPeril(lCount).ClaimPayment.Count - 1
                            If Session(CNClaimPaymentKey) = oClaim.ClaimPeril(lCount).ClaimPayment(lInnerCount).BaseClaimPaymentKey Then
                                sPerilTypeCode = oClaim.ClaimPeril(lCount).TypeCode
                                iPerilKey = oClaim.ClaimPeril(lCount).ClaimPerilKey
                                If (Session(CNClaimPerilIndex) Is Nothing AndAlso Session(CNClaimMultiPerilIndex) IsNot Nothing) Then
                                    PerilsIndex.Add(lCount)
                                Else
                                    bFoundPayment = True
                                    Exit For
                                End If
                            End If
                        Next
                        If bFoundPayment Then
                            iPerilIndex = lCount
                            Session(CNClaimPerilIndex) = iPerilIndex
                            Exit For
                        End If
                    Next
                End If

                If PerilsIndex.Count > 0 Then
                    Session(CNClaimMultiPerilIndex) = PerilsIndex
                End If

                If bClaimBuilder = True Then
                    Dim WebRoot As String = AppSettings("WebRoot")
                    Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
                    'Check Peril Builder
                    Dim sConfigFile As String
                    Dim sFolder As String
                    sFolder = String.Empty

                    If oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.PerilTypes.PerilType(Trim(sPerilTypeCode)) IsNot Nothing Then
                        sFolder = "~/Claims/ClientPages/" & oPortal.Claims.ScreenLocation & "/Perils/" _
                                            & oPortal.Claims.PerilTypes.PerilType(sPerilTypeCode).Folder
                    End If

                    sConfigFile = sFolder & "/perilscreens.config"

                    If System.IO.File.Exists(Server.MapPath(sConfigFile)) = True Then
                        Dim sXMLPath As String = Server.MapPath(sFolder & "/perilscreens.config")

                        Dim xmlds As New XmlDataSource
                        xmlds.DataFile = sXMLPath
                        xmlds.EnableCaching = False

                        Dim Navigator As XPathNavigator
                        Dim Doc As XPathDocument = New XPathDocument(sXMLPath)
                        Navigator = Doc.CreateNavigator()
                        Dim iSrc As XPathNodeIterator

                        iSrc = Navigator.Select("/screens/screen/tab[1]")
                        Dim sFirstPage As String = String.Empty
                        While (iSrc.MoveNext)
                            sFirstPage = iSrc.Current.GetAttribute("url", String.Empty)
                        End While

                        Dim iTab As XPathNodeIterator
                        Dim sURL As String = String.Empty

                        iSrc = Navigator.Select("/screens/screen")
                        While (iSrc.MoveNext)
                            iTab = Navigator.Select("/screens/screen/tab")
                            While (iTab.MoveNext)
                                sURL = iTab.Current.GetAttribute("url", String.Empty)
                                Exit While
                            End While
                        End While


                        'We have check whether this condition is required or not
                        If Not IsCurrentPage(sFirstPage) Then

                            'We're not on the first risk screen and we really should be

                            'stops all the processing being down again on then correct
                            'risks screen, as we've already collected all the info we need
                            Session(CNQuoteInSync) = True
                            Response.Redirect(sFolder & "/" & sFirstPage & "?FromPage=Overview&PerilID=" & iPerilKey & "&PerilIndex=" & iPerilIndex, False)
                        End If
                    Else
                        Response.Redirect("~/Claims/PerilDetails.aspx?FromPage=Overview&PerilID=" & iPerilKey & "&PerilIndex=" & iPerilIndex & "&ReturnUrl=" & Request.Path.Replace(WebRoot, "~/"))
                    End If
                Else
                    Response.Redirect("~/Claims/PerilDetails.aspx?FromPage=Overview&PerilID=" & iPerilKey & "&PerilIndex=" & iPerilIndex, False)
                End If
            End If

        End Sub
        Public Sub FinishButton(ByVal sender As Object, ByVal e As System.EventArgs)
            If Page.IsValid Then
                Dim bFindPerilControl As Boolean = False

                For Each oControl In oMaster.Controls
                    'check whether controls "perils.ascx" exist on this page
                    If oControl.GetType.Name.Equals("controls_perils_ascx") Then
                        bFindPerilControl = True
                        Exit For
                    End If
                Next
                Dim RequestedPageURL As String = Request.Url.Segments(Request.Url.Segments.Length - 1).ToString
                Dim RestrictedPageURLs As String = "perils.aspx"
                If RestrictedPageURLs.ToUpper.Contains(RequestedPageURL.ToUpper) AndAlso (CType(Session.Item(CNMode), Mode) = Mode.SalvageClaim Or CType(Session.Item(CNMode), Mode) = Mode.TPRecovery) Then
                    'For Salvage and TP Recover Mode
                    Response.Redirect("~/claims/changeclaim.aspx", False)
                Else
                    If CType(Session(CNMode), Mode) = Mode.Review Then
                        Response.Redirect(Session(CNReturnURL), False)
                    Else
                        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                        Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                        Dim sDatasetErrorMessages As String = String.Empty

                        Session(CNQuoteInSync) = True

                        If CType(Session.Item(CNMode), Mode) = Mode.NewClaim Or (CType(Session.Item(CNMode), Mode) = Mode.SalvageClaim _
                                                                                  OrElse CType(Session.Item(CNMode), Mode) = Mode.TPRecovery) OrElse
                                                                         CType(Session.Item(CNMode), Mode) = Mode.EditClaim Or CType(Session.Item(CNMode), Mode) = Mode.PayClaim Then

                            WriteClaim(Me)

                            sDatasetErrorMessages = ValidateDataset() 'need to validate the 
                        End If

                        If sDatasetErrorMessages <> String.Empty Then
                            'create a new custom validator
                            'set it as invalid and set the error message property to the output from ValidateDataset
                            Dim cstInvalidDataset As New CustomValidator
                            cstInvalidDataset.IsValid = False
                            cstInvalidDataset.ErrorMessage = sDatasetErrorMessages
                            cstInvalidDataset.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                            Page.Validators.Add(cstInvalidDataset)
                        End If

                        If (sDatasetErrorMessages = String.Empty And CBool(Session(CNIsClaimLocked)) <> True) Then
                            If Not String.IsNullOrEmpty(sNextPage) And iDepth > 1 Then
                                Session.Item(CNTempOI) = Nothing
                                'Removal of USed OI from Hash Table
                                If Session(CNMode) <> Mode.ViewClaim And Session(CNMode) <> Mode.Review Then
                                    Dim oUsedOI As Collections.Stack = Session(CNOI)
                                    If Session(CNNode) IsNot Nothing Then
                                        Dim hCurrentNodeColl As New Hashtable()
                                        hCurrentNodeColl = CType(Session(CNNode), Hashtable)

                                        If hCurrentNodeColl.ContainsKey(oUsedOI.Peek().ToString) Then
                                            hCurrentNodeColl.Remove(oUsedOI.Peek().ToString)
                                            Session(CNNode) = hCurrentNodeColl
                                        End If
                                    End If

                                    'Delete the usedOI from DeletedOI collection
                                    If Session(CNDeletedNode) IsNot Nothing Then
                                        Dim hDeletedNodeColl As New Hashtable()
                                        hDeletedNodeColl = CType(Session(CNDeletedNode), Hashtable)

                                        If hDeletedNodeColl.ContainsKey(oUsedOI.Peek().ToString) Then
                                            hDeletedNodeColl.Remove(oUsedOI.Peek().ToString)
                                            Session(CNDeletedNode) = hDeletedNodeColl
                                        End If
                                    End If

                                    'Remove the UC attrinbute to identify the valid/invalid node
                                    ResetUCElement(oUsedOI.Peek().ToString)
                                End If
                                'redirect the page if its required
                                PrePageRedirect()
                                Response.Redirect(sParentTab, False)
                            Else
                                If oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.ShowSummary = True Then
                                    'Call PreSummaryPageRedirect
                                    PreSummaryPageRedirect()
                                Else
                                    'Call SkipSummaryPage
                                    SkipSummaryPage()
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End Sub

        Public Sub TabClick(ByVal v_sPath As String)

            If Page.IsValid Then

                Session(CNQuoteInSync) = True

                WriteClaim(Me)
                Response.Redirect(v_sPath, False)

            End If
        End Sub
        Public Sub AddItem(ByVal v_sScreenCode As String, ByVal v_sPath As String,
                                  ByVal v_sParentElement As String, ByVal v_sChildElement As String)

            Session(CNQuoteInSync) = True

            WriteClaim(Me)

            'create new element in XML
            Dim sOI As String = DataSetFunctions.CreateElementFromXML(v_sScreenCode,
                oOI.Peek.ToString(), v_sParentElement, v_sChildElement)

            oOI.Push(sOI)
            Session.Item(CNOI) = oOI

            Response.Redirect(v_sPath, False)

        End Sub
        Public Overridable Sub PreDataSetWrite()

        End Sub
        Public Overridable Sub PostDataSetWrite()

        End Sub
        Public Overridable Sub PrePageRedirect()

        End Sub

        ''' <summary>
        ''' This will acutally allow the user to move to the same page back from where he has come.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub BackButtonRedirect()

        End Sub
        Private Sub WriteClaim(Optional ByVal sender As Object = Nothing)

            If CType(Session.Item(CNMode), Mode) = Mode.NewClaim OrElse
                 (CType(Session.Item(CNMode), Mode) = Mode.SalvageClaim OrElse
                  CType(Session.Item(CNMode), Mode) = Mode.TPRecovery) OrElse
                  CType(Session.Item(CNMode), Mode) = Mode.EditClaim OrElse
                  CType(Session.Item(CNMode), Mode) = Mode.PayClaim Then
                PreDataSetWrite()
                If oOI.Count > 0 Then
                    WriteContainerToXML(oMaster, sScreenCode, oOI.Peek, Nothing, Nothing, sender)
                Else
                    WriteContainerToXML(oMaster, sScreenCode, Nothing, Nothing, Nothing, sender)
                End If
                PostDataSetWrite()

            End If

        End Sub

        ''' <summary>
        ''' Handles the initialization event from the TabIndex, this is
        ''' intended to return the values set within the TabIndex
        ''' </summary>
        ''' <param name="v_sScreenCode">Current screen code, need for running the Add and Edit rules via SAM</param>
        ''' <param name="v_iDepth">Current screen depth of the tabs</param>
        ''' <param name="v_sPreviousPage">Path to the previous page</param>
        ''' <param name="v_sNextPage">Path to the next page</param>
        ''' <remarks></remarks>
        Public Sub TabIndexInitialized(ByVal v_sScreenCode As String,
                                ByVal v_iDepth As Integer,
                                 ByVal v_sParentTab As String,
                                ByVal v_sPreviousPage As String,
                                ByVal v_sNextPage As String)

            sScreenCode = v_sScreenCode
            iDepth = v_iDepth
            sParentTab = v_sParentTab
            sPrevPage = v_sPreviousPage
            sNextPage = v_sNextPage

        End Sub

        ''' <summary>
        ''' Handles the PreRender FinishAndPaybutton Event 
        ''' </summary>
        Protected Sub PreRender_FinishAndPay(ByVal sender As Object, ByVal e As System.EventArgs)
            If CType(Session(CNMode), Mode) = Mode.Authorise Or CType(Session(CNMode), Mode) = Mode.DeclinePayment Or CType(Session(CNMode), Mode) = Mode.Recommend Or CType(Session(CNMode), Mode) = Mode.ViewClaimPayment Then
                'Finish should not be visible as user need to visit every page
                sender.visible = False
            Else
                Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim btnFinishAndPay As LinkButton = oMaster.FindControl("btn_FinishAndPay")
                Dim ChkPerilsCnt As Boolean = False

                'Default Set FinishAndPay button Visiblity is OFF
                If btnFinishAndPay IsNot Nothing Then
                    btnFinishAndPay.Visible = False
                End If

                For Each oControl In oMaster.Controls
                    'check whether controls "perils.ascx" exist on this page
                    If oControl.GetType.Name.Equals("controls_perils_ascx") Then
                        ChkPerilsCnt = True
                        Exit For
                    End If
                Next

                If ChkPerilsCnt = True Then
                    'If “Perils.ascx” control exist then check
                    If (CType(Session.Item(CNMode), Mode) = Mode.NewClaim AndAlso CType(Session(CNClaimBuilder), Boolean) = True) Then
                        'check whether claim mode is NewClaim  and ClaimBuilder is ON 
                        Dim oFastTrackClaimPayment As NexusProvider.ProductClaimsWorkflowOptionsValue
                        Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                        oFastTrackClaimPayment = oWebservice.GetProductClaimsWorkflowOptions(NexusProvider.ClaimProcessType.OpenClaim, oQuote.ProductCode)
                        'Checking of the Fast Track Claim Payments
                        If (oFastTrackClaimPayment.FastTrackClaims = True) Then
                            'Set FinishAndPay button Visiblity is TRUE
                            If btnFinishAndPay IsNot Nothing Then
                                btnFinishAndPay.Visible = True
                            End If
                        End If
                    ElseIf (CType(Session.Item(CNMode), Mode) = Mode.EditClaim AndAlso CType(Session(CNClaimBuilder), Boolean) = True) Then
                        'When Claim mode is EditClaim and ClaimBuilder is ON 
                        Dim oFastTrackClaimPayment As NexusProvider.ProductClaimsWorkflowOptionsValue
                        Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                        oFastTrackClaimPayment = oWebservice.GetProductClaimsWorkflowOptions(NexusProvider.ClaimProcessType.MaintainClaim, oQuote.ProductCode)
                        'Checking of the Fast Track Claim Payments
                        If (oFastTrackClaimPayment.FastTrackClaims = True) Then
                            'Set FinishAndPay button Visiblity is TRUE
                            If btnFinishAndPay IsNot Nothing Then
                                btnFinishAndPay.Visible = True
                            End If
                        End If
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' Handles the  FinishAndPaybutton OnClick Event from the FinishAndPay Button
        ''' </summary>
        Public Sub FinishAndPayButton(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim oUserAuthority As New NexusProvider.UserAuthority
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            oUserAuthority.UserCode = Session(CNLoginName)
            oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.HasClaimPaymentsAuthority
            oWebservice = New NexusProvider.ProviderManager().Provider
            oWebservice.GetUserAuthorityValue(oUserAuthority)
            'Check the user's "Claim Payment Authority"
            If oUserAuthority.UserAuthorityValue = 0 Or oUserAuthority.UserAuthorityValue Is Nothing Then
                'if user have not Authority , Show the Error message using Resource file
                oResource = New ResXResourceReader(HttpContext.Current.Server.MapPath(AppSettings("WebRoot") & "App_LocalResources/Error.aspx.resx"))
                en = oResource.GetEnumerator()
                Dim cstClaimPaymentAuthority As New CustomValidator


                While (en.MoveNext)
                    If en.Key.ToString.Trim = "Err_NoPaymentAuthority" Then
                        'look for a Error message in the root App_LocalResources/Error.aspx.resx page
                        cstClaimPaymentAuthority.ErrorMessage = en.Value
                    End If
                End While
                cstClaimPaymentAuthority.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                'add the validator to the page, this will have the effect of making the page invalid
                cstClaimPaymentAuthority.IsValid = False
                Page.Validators.Add(cstClaimPaymentAuthority)
            Else
                'if user has 
                Dim bClaimTimeStamp() As Byte = Session.Item(CNClaimTimeStamp)
                Dim oClaimDetails As NexusProvider.ClaimOpen = Session.Item(CNClaim)
                Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                Dim sBranchCode As String = oQuote.BranchCode
                Dim oclaimRisk As New NexusProvider.ClaimRisk
                'Claim Risk has wrong argument called ClaimKey, actually it is BaseClaimKey
                oclaimRisk.ClaimKey = oClaimDetails.BaseClaimKey
                oclaimRisk.TimeStamp = Session.Item(CNClaimRiskTimeStamp)
                oclaimRisk.XMLDataSet = Session.Item(CNDataSet)
                If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Then
                    Dim bClaimBuilder As Boolean
                    Boolean.TryParse(Session(CNClaimBuilder), bClaimBuilder)
                    If bClaimBuilder = True Then
                        'Update the claim builder risk screen
                        'Arch issue 268
                        If Not UpdateClaimRiskCall(oclaimRisk, sBranchCode) Then
                            Exit Sub
                        End If
                    End If

                    If Session(CNMode) = Mode.NewClaim Then
                        'Fire the Bind Claim
                        'arch issue 268
                        'oWebservice.BindClaim(oClaimDetails, bClaimTimeStamp, 1, Nothing, sBranchCode)
                        If BindClaimCall(oClaimDetails, bClaimTimeStamp, 1, Nothing, sBranchCode) Is Nothing Then
                            Exit Sub
                        End If
                    ElseIf Session(CNMode) = Mode.EditClaim Then
                        'Fire the Bind Claim
                        'arch issue 268
                        'oWebservice.BindClaim(oClaimDetails, bClaimTimeStamp, 2, Nothing, sBranchCode)
                        If BindClaimCall(oClaimDetails, bClaimTimeStamp, 2, Nothing, sBranchCode) Is Nothing Then
                            Exit Sub
                        End If
                    End If

                    Session(CNMode) = Mode.PayClaim
                    ' Session(CNCurrentOI) = Session(CNOI)
                    GetLatestDetails() 'Update the session with latest values
                    'Session(CNOI) = Session(CNCurrentOI)
                    Session.Remove(CNEnablePayClaim) 'Remove the ReadOnly mode of the Pay Claim
                    Session(CNPayClaim) = Nothing ' Reset the pay claim to accept the another payment
                    'Dummy Cal of PayClaim, to retreive back the latetst claim key
                    PayClaimWithZeroAmount()
                    CType(sender, LinkButton).Visible = False
                    For Each oControl In oMaster.Controls
                        'check whether controls "perils.ascx" exist on this page
                        If oControl.GetType.Name.Equals("controls_perils_ascx") Then
                            oControl.ReBindGrid = True
                        End If
                    Next

                    'Move to same page again after new dataset
                    Dim WebRoot As String = AppSettings("WebRoot")
                    Dim sUrl As String = Request.Path.Replace(WebRoot, "~/")
                    Current.Session.Remove(CNOI)
                    Session(CNQuoteInSync) = False
                    If CBool(Session(CNIsClaimLocked)) <> True Then
                        Response.Redirect(sUrl, False)
                    End If

                End If
            End If
        End Sub
        Public Sub SaveChildButton(ByVal sender As Object, ByVal e As System.EventArgs)
            If Page.IsValid Then
                If CType(Session.Item(CNMode), Mode) <> Mode.View _
                AndAlso CType(Session.Item(CNMode), Mode) <> Mode.ViewClaim _
                AndAlso CType(Session.Item(CNMode), Mode) <> Mode.Review _
                AndAlso CType(Session.Item(CNMode), Mode) <> Mode.Authorise _
                AndAlso CType(Session.Item(CNMode), Mode) <> Mode.Recommend _
                AndAlso CType(Session.Item(CNMode), Mode) <> Mode.ViewClaimPayment _
                AndAlso CType(Session.Item(CNMode), Mode) <> Mode.DeclinePayment Then
                    Dim oRiskContainer As RiskContainer = sender.Parent
                    sScreenCode = oRiskContainer.ScreenCode
                    If oRiskContainer IsNot Nothing Then
                        Dim htOI As Collections.Stack = Session(CNOI)

                        If oRiskContainer.Mode = RiskContainer.ChildMode.Add Then

                            oRiskContainer.OI = DataSetFunctions.CreateElementFromXML(oRiskContainer.ScreenCode,
                                oOI.Peek, oRiskContainer.ParentElement, oRiskContainer.ChildElement)

                        End If

                        'ADD CHILD CONTROL VALUES TO XML DATASET
                        WriteContainerToXML(oRiskContainer, oRiskContainer.ScreenCode, oRiskContainer.OI)

                    End If

                    Dim sDatasetErrorMessages As String = ValidateDataset() 'need to validate the 
                    If sDatasetErrorMessages <> String.Empty Then
                        'create a new custom validator
                        'set it as invalid and set the error message property to the output from ValidateDataset
                        Dim cstInvalidDataset As New CustomValidator
                        cstInvalidDataset.IsValid = False
                        cstInvalidDataset.ErrorMessage = sDatasetErrorMessages
                        cstInvalidDataset.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                        Page.Validators.Add(cstInvalidDataset)

                        DataSetFunctions.DeleteElementFromXML(oRiskContainer.ScreenCode, oRiskContainer.OI, oRiskContainer.ChildElement)
                        oRiskContainer.Mode = RiskContainer.ChildMode.Add
                    Else
                        ResetUCElement(oRiskContainer.OI)
                        'RESET CHILD CONTROL
                        FrameWorkFunctions.ResetControls(oRiskContainer)
                        oRiskContainer.Mode = RiskContainer.ChildMode.Add
                        'RELOAD EDITED CHILD SCREEN VALUES IN ITEMGRID
                        DataSetFunctions.ReadContainerFromXML(oMaster, oOI.Peek, Me)
                    End If
                End If
            End If
        End Sub
        Public Sub CancelChildButton(ByVal sender As Object, ByVal e As System.EventArgs)

            Dim oRiskContainer As RiskContainer = sender.Parent

            If oRiskContainer IsNot Nothing Then

                'RESET CHILD CONTROL
                FrameWorkFunctions.ResetControls(oRiskContainer)

                oRiskContainer.Mode = RiskContainer.ChildMode.Add

            End If

        End Sub
#End Region
#Region "Peril Event"
        Sub SavePerilData()
            If Page.IsValid Then
                Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                Dim sBranchCode As String = oQuote.BranchCode
                Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim bTimeStamp() As Byte = Session.Item(CNClaimRiskTimeStamp)
                Dim bClaimTimeStamp() As Byte = Session.Item(CNClaimTimeStamp)
                Dim oClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                Dim sFolder As String = Nothing

                If Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                    'For Salvage and TP Recover Mode
                    Response.Redirect("~/claims/changeclaim.aspx", False)
                ElseIf Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Then
                    If Session(CNClaimBuilder) Is Nothing Or
                    (Session(CNClaimBuilder) IsNot Nothing AndAlso Session(CNClaimBuilder) = False) Then
                        'Claim Builder Not Configured
                        'if ShowSummary is false then skip the summary page
                        If oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.ShowSummary = True Then
                            'show the summary page
                            Response.Redirect("~/claims/summary.aspx?ReturnUrl=" & Request.Path.Replace(WebRoot, "~/"))
                        Else
                            'skip the summary page
                            SkipSummaryPage()
                        End If
                    End If
                ElseIf Session(CNMode) = Mode.PayClaim Then
                    If Session(CNClaimBuilder) Is Nothing Or
                   (Session(CNClaimBuilder) IsNot Nothing AndAlso Session(CNClaimBuilder) = False) Then
                        'Claim Builder Not Configured

                        Dim sRunAuthorizePayment As String
                        Dim oRunClaimWorkFlow As NexusProvider.ProductClaimsWorkflowOptionsValue

                        'get the Product Risk option setting named Run Authorize Payment
                        sRunAuthorizePayment = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.RunAuthorisationScriptsClaimPayments, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
                        'get the Claim Workflow Setting
                        oRunClaimWorkFlow = oWebservice.GetProductClaimsWorkflowOptions(NexusProvider.ClaimProcessType.ClaimPayment, oQuote.ProductCode)
                        ViewState("RunClaimWorkFlow") = oRunClaimWorkFlow

                        Session.Remove(CNAuthorizeStatus)
                        If sRunAuthorizePayment = "1" Then
                            'if ShowSummary is false then skip the summary page
                            Session(CNAuthorizeStatus) = "Authorize Payment"
                            If oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.ShowSummary = True Then
                                'show the summary page
                                Response.Redirect("~/claims/summary.aspx?ReturnUrl=" & Request.Path.Replace(WebRoot, "~/"))
                            Else
                                'skip the summary page
                                SkipSummaryPage()
                            End If
                        ElseIf oRunClaimWorkFlow.CashPaymentProcess = True Then
                            Dim dAmount As Decimal = 0.0

                            Dim PerilsIndex As New System.Collections.Generic.List(Of Integer)
                            If Session(CNClaimMultiPerilIndex) IsNot Nothing Then
                                PerilsIndex = Session(CNClaimMultiPerilIndex)
                            Else
                                PerilsIndex.Add(CInt(Session(CNClaimPerilIndex)))
                            End If

                            For Each PerilItemIndex As Integer In PerilsIndex
                                If CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(PerilItemIndex).ClaimReserve IsNot Nothing Then
                                    For Each oPaymentItem As NexusProvider.ClaimPerilReservePaymentType In CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(PerilItemIndex).ClaimReserve
                                        dAmount += oPaymentItem.ThisPaymentINCLTax
                                    Next
                                End If
                            Next

                            If dAmount > 0 Then
                                'Amount is positive so need to receive it
                                Response.Redirect("~/secure/payment/CashListNew.aspx", False)
                            Else
                                'Amount is negative so no need to receive it , so process it directly
                                'if ShowSummary is false then skip the summary page
                                If oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.ShowSummary = True Then
                                    'show the summary page
                                    Response.Redirect("~/claims/summary.aspx?ReturnUrl=" & Request.Path.Replace(WebRoot, "~/"))
                                Else
                                    'skip the summary page
                                    SkipSummaryPage()
                                End If
                            End If
                        Else
                            'If RunAuthorize is OFF and Cash Payment Process is OFF
                            'if ShowSummary is false then skip the summary page
                            If oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.ShowSummary = True Then
                                'show the summary page
                                Response.Redirect("~/claims/summary.aspx?ReturnUrl=" & Request.Path.Replace(WebRoot, "~/"))
                            Else
                                'skip the summary page
                                SkipSummaryPage()
                            End If
                        End If
                    End If
                End If
            End If
        End Sub
        Sub SkipSummaryPage()
            oMaster = GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName)
            Dim btnNextClmBuilder As Button = oMaster.FindControl("btnNext") 'Next Button of Claim Builder Page, when Claim Builder is ON
            Dim btnFinishClmBuilder As Button = oMaster.FindControl("btnFinish") 'Finish Button of Claim Builder Page, when Claim Builder is ON
            Dim btnFinishTopClmBuilder As Button = oMaster.FindControl("btnFinishTop") 'Finish Button of Claim Builder Page, when Claim Builder is ON
            Dim hidChkCBuilderChoice As New HiddenField 'Retrieve Javascript  FurtherPaymentsConfirmation value, when Claim Builder is ON
            Dim hidChkCBuilderClaimClose As New HiddenField 'Retrieve Javascript OnCliamBuilderClaimCloseConfirmation value, when Claim Builder is ON
            Dim hidChkCBuilderPaymentMsg As New HiddenField

            Dim btnNext As LinkButton = oMaster.FindControl("btn_Next") 'Next Button of Peril Page, when Claim Builder is OFF
            Dim btnFinish As LinkButton = oMaster.FindControl("btn_Finish") 'Finish Button of Peril Page, when Claim Builder is OFF
            Dim hidChlClaimClose As HiddenField 'Retrieve Javascript ClaimCloseConfirmation value, when Claim Builder is OFF
            Dim hidChkChoice As HiddenField 'Retrieve Javascript PaymentsConfirmation value, when Claim Builder is OFF
            Dim hidChkPaymentMsg As HiddenField

            Dim oRunClaimWorkFlow As NexusProvider.ProductClaimsWorkflowOptionsValue = ViewState("RunClaimWorkFlow")
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
            Dim sRunAuthorizePayment As String
            Dim bCheckStatus As Boolean = False
            Dim sDisplayReinsurance As String
            Dim bDisplayReinsuranceWeb As Boolean = True

            If Session(CNClaimBuilder) Is Nothing Or (Session(CNClaimBuilder) IsNot Nothing AndAlso Session(CNClaimBuilder) = False) Then
                For Each oControl In oMaster.Controls
                    'check whether perils user control have below hidden controls
                    If oControl.GetType.Name.Equals("controls_perils_ascx") Then
                        If oControl.FindControl("hidChkPaymentMsg") IsNot Nothing Then
                            hidChkPaymentMsg = CType(oControl.FindControl("hidChkPaymentMsg"), HiddenField)
                        End If
                        If oControl.FindControl("hidChlClaimClose") IsNot Nothing Then
                            hidChlClaimClose = CType(oControl.FindControl("hidChlClaimClose"), HiddenField)
                        End If
                        If oControl.FindControl("hidChkChoice") IsNot Nothing Then
                            hidChkChoice = CType(oControl.FindControl("hidChkChoice"), HiddenField)
                        End If
                    End If
                Next
            Else
                For Each oControl In oMaster.Controls
                    'check whether claimsprogressbar user control have below hiddeen controls
                    If oControl.FindControl("hidChkCBuilderChoice") IsNot Nothing Then
                        hidChkCBuilderChoice = CType(oControl.FindControl("hidChkCBuilderChoice"), HiddenField)
                    End If
                    If oControl.FindControl("hidChkCBuilderClaimClose") IsNot Nothing Then
                        hidChkCBuilderClaimClose = CType(oControl.FindControl("hidChkCBuilderClaimClose"), HiddenField)
                    End If
                    If oControl.FindControl("hidChkCBuilderPaymentMsg") IsNot Nothing Then
                        hidChkCBuilderPaymentMsg = CType(oControl.FindControl("hidChkCBuilderPaymentMsg"), HiddenField)
                    End If
                Next
            End If

            'Check the risk option to display of Reinsurance
            sDisplayReinsurance = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.RiskTypeMaintenance, NexusProvider.ProductRiskOptions.Description, NexusProvider.RiskTypeOptions.DisplayClaimReinsurance, CType(Session(CNClaimQuote), NexusProvider.Quote).ProductCode, CType(Session(CNClaim), NexusProvider.ClaimOpen).RiskType)

            'Check the web config option to display of Reinsurance -- oCliam
            If (oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.RiskTypes.RiskType(Trim(CType(Session(CNClaim), NexusProvider.ClaimOpen).RiskType)) IsNot Nothing) Then
                bDisplayReinsuranceWeb = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.RiskTypes.RiskType(Trim(CType(Session(CNClaim), NexusProvider.ClaimOpen).RiskType)).DisplayClaimReinsurance
            End If

            'Check the User Authority to display of Reinsurance
            Dim oUserAuthority As New NexusProvider.UserAuthority
            oUserAuthority.UserCode = Session(CNLoginName)
            oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.DisplayClaimReinsurance
            oWebservice = New NexusProvider.ProviderManager().Provider
            oWebservice.GetUserAuthorityValue(oUserAuthority)

            If Not IsPostBack Then
                If Session(CNMode) = Mode.PayClaim AndAlso CType(Session(CNEnablePayClaim), Boolean) _
                AndAlso (sDisplayReinsurance = "0" Or oUserAuthority.UserAuthorityValue = "0") Then
                    'If Session(CNMode) = Mode.PayClaim AndAlso CType(Session(CNEnablePayClaim), Boolean)  Then

                    If oRunClaimWorkFlow Is Nothing Then
                        'get the Claim Workflow Setting
                        oRunClaimWorkFlow = oWebservice.GetProductClaimsWorkflowOptions(NexusProvider.ClaimProcessType.ClaimPayment, oQuote.ProductCode)
                        ViewState("RunClaimWorkFlow") = oRunClaimWorkFlow
                    End If
                    'get the Product Risk option setting named Run Authorize Payment
                    sRunAuthorizePayment = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.RunAuthorisationScriptsClaimPayments, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)

                    Session.Remove(CNAuthorizeStatus)
                    If sRunAuthorizePayment = "1" Then

                        'Set "Authorize Payment" Session ON
                        Session(CNAuthorizeStatus) = "Authorize Payment"

                        bCheckStatus = True
                    ElseIf oRunClaimWorkFlow IsNot Nothing AndAlso oRunClaimWorkFlow.CashPaymentProcess = True Then
                        Dim dAmount As Decimal = 0.0

                        Dim PerilsIndex As New System.Collections.Generic.List(Of Integer)
                        If Session(CNClaimMultiPerilIndex) IsNot Nothing Then
                            PerilsIndex = Session(CNClaimMultiPerilIndex)
                        Else
                            PerilsIndex.Add(CInt(Session(CNClaimPerilIndex)))
                        End If
                        For Each PerilItemIndex As Integer In PerilsIndex
                            If CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(PerilItemIndex).ClaimReserve IsNot Nothing Then
                                For Each oPaymentItem As NexusProvider.ClaimPerilReservePaymentType In CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(PerilItemIndex).ClaimReserve
                                    dAmount += oPaymentItem.ThisPaymentINCLTax
                                Next
                            End If
                        Next


                        If dAmount < 0 Then
                            'Amount is negative so need to receive it
                            bCheckStatus = True
                        End If

                    End If
                    If btnFinishClmBuilder IsNot Nothing Then
                        btnFinishClmBuilder.Attributes.Add("onclick", "javascript:return FurtherPaymentsConfirmation();")
                    End If

                    If btnFinishTopClmBuilder IsNot Nothing Then
                        btnFinishTopClmBuilder.Attributes.Add("onclick", "javascript:return FurtherPaymentsConfirmation();")
                    End If


                    If bCheckStatus = True And oRunClaimWorkFlow IsNot Nothing AndAlso oRunClaimWorkFlow.MakeFurtherPayments = True Then
                        Dim oPerilColl As NexusProvider.PerilCollection = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril

                        If oRunClaimWorkFlow.MakeFurtherPayments = True Then
                            'Check the reserve amount vs Payment amount
                            Dim bStatus As Boolean = False
                            For j As Integer = 0 To oPerilColl.Count - 1
                                For i As Integer = 0 To oPerilColl(j).ClaimReserve.Count - 1
                                    Dim dTotalReserve As Double
                                    For iReserveCount As Integer = 0 To oPerilColl(j).Reserve.Count - 1
                                        If oPerilColl(j).Reserve(iReserveCount).BaseReserveKey <> 0 _
                                        AndAlso oPerilColl(j).Reserve(iReserveCount).BaseReserveKey = oPerilColl(j).ClaimReserve(i).BaseReserveKey Then
                                            dTotalReserve = oPerilColl(j).Reserve(iReserveCount).InitialReserve + oPerilColl(j).Reserve(iReserveCount).RevisedReserve
                                            Exit For
                                        End If
                                    Next
                                    Dim dTotalPaid As Double = (oPerilColl(j).ClaimReserve(i).ThisPaymentINCLTax + oPerilColl(j).ClaimReserve(i).PaidToDate) - (oPerilColl(j).ClaimReserve(i).ThisPaymentTax - oPerilColl(j).ClaimReserve(i).PaidToDateTax)
                                    If (dTotalReserve - dTotalPaid) <= 0 Then
                                        bStatus = True
                                    Else
                                        bStatus = False
                                        Exit For
                                    End If
                                Next
                                If bStatus = False Then
                                    Exit For
                                End If
                            Next
                            If bStatus = False Then
                                If btnFinish IsNot Nothing Then
                                    btnFinish.Attributes.Add("onclick", "javascript:return PaymentConfirmation();")
                                End If

                                If btnNext IsNot Nothing Then
                                    btnNext.Attributes.Add("onclick", "javascript:return PaymentConfirmation();")
                                End If



                                ''in case of PayClaim -Show PaymentConfirmation Message at the last claim builder page in case of “Next Button”
                                If String.IsNullOrEmpty(sNextPage) Or sNextPage = sParentTab Then
                                    If (Not iDepth > 1) Then
                                        If btnNextClmBuilder IsNot Nothing Then
                                            btnNextClmBuilder.Attributes.Add("onclick", "javascript:return FurtherPaymentsConfirmation();")
                                        End If
                                    End If
                                End If
                            Else
                                If hidChkPaymentMsg IsNot Nothing Then
                                    hidChkPaymentMsg.Value = "0"
                                End If

                                If hidChkCBuilderPaymentMsg IsNot Nothing Then
                                    hidChkCBuilderPaymentMsg.Value = "0"
                                End If
                                'If this value is set then paymentconfirmatio will be called from
                                'ClamCloseConfirmation javascript function
                                If btnNext IsNot Nothing Then
                                    btnNext.Attributes.Add("onclick", "javascript:return ClaimCloseConfirmation();")
                                End If

                                If btnFinish IsNot Nothing Then
                                    btnFinish.Attributes.Add("onclick", "javascript:return ClaimCloseConfirmation();")
                                End If

                                'in case of PayClaim - Show CloseClaimConfirmation Message at any stage of the claim builder page wherever user presses “Finish” button
                                If btnFinishClmBuilder IsNot Nothing Then
                                    btnFinishClmBuilder.Attributes.Add("onclick", "javascript:return OnCliamBuilderClaimCloseConfirmation();")
                                End If

                                ''in case of PayClaim -Show CloseClaimConfirmation Message at the last claim builder page in case of “Next Button”
                                If String.IsNullOrEmpty(sNextPage) Or sNextPage = sParentTab Then
                                    If (Not iDepth > 1) Then
                                        If btnNextClmBuilder IsNot Nothing Then
                                            btnNextClmBuilder.Attributes.Add("onclick", "javascript:return OnCliamBuilderClaimCloseConfirmation();")
                                        End If
                                    End If
                                End If
                            End If
                        ElseIf bCheckStatus = True And oRunClaimWorkFlow IsNot Nothing AndAlso oRunClaimWorkFlow.MakeFurtherPayments = False Then
                            'Check the reserve amount vs Payment amount
                            Dim bFound As Boolean = False
                            For j As Integer = 0 To oPerilColl.Count - 1
                                For i As Integer = 0 To oPerilColl(j).ClaimReserve.Count - 1
                                    If oPerilColl(j).ClaimReserve(i).CurrentReserve <= 0 Then
                                        bFound = True
                                    Else
                                        bFound = False
                                        Exit For
                                    End If
                                Next
                                If bFound = False Then
                                    Exit For
                                End If
                            Next
                            If bFound = True Then
                                If hidChkPaymentMsg IsNot Nothing Then
                                    hidChkPaymentMsg.Value = String.Empty
                                End If
                                If hidChkCBuilderPaymentMsg IsNot Nothing Then
                                    hidChkCBuilderPaymentMsg.Value = String.Empty
                                End If

                                If btnNext IsNot Nothing Then
                                    btnNext.Attributes.Add("onclick", "javascript:return ClaimCloseConfirmation();")
                                End If

                                If btnFinish IsNot Nothing Then
                                    btnFinish.Attributes.Add("onclick", "javascript:return ClaimCloseConfirmation();")
                                End If
                                'in case of PayClaim - Show CloseClaimConfirmation Message at any stage of the claim builder page wherever user presses “Finish” button
                                If btnFinishClmBuilder IsNot Nothing Then
                                    btnFinishClmBuilder.Attributes.Add("onclick", "javascript:return OnCliamBuilderClaimCloseConfirmation();")
                                End If

                                ''in case of PayClaim -Show CloseClaimConfirmation Message at the last claim builder page in case of “Next Button”
                                If String.IsNullOrEmpty(sNextPage) Or sNextPage = sParentTab Then
                                    If (Not iDepth > 1) Then
                                        If btnNextClmBuilder IsNot Nothing Then
                                            btnNextClmBuilder.Attributes.Add("onclick", "javascript:return OnCliamBuilderClaimCloseConfirmation();")
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                ElseIf CType(Session.Item(CNMode), Mode) = Mode.ViewClaim Then
                    If btnFinish IsNot Nothing Then
                        btnFinish.Visible = False
                    End If
                End If
            Else
                'if Finish or Next button has hit
                Dim sBranchCode As String = oQuote.BranchCode
                Dim oClaimDetails As NexusProvider.ClaimOpen = Session.Item(CNClaim)
                Dim bClaimTimeStamp() As Byte = Session.Item(CNClaimTimeStamp)
                Dim oclaimRisk As New NexusProvider.ClaimRisk
                Dim oOriginalClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)

                Dim oreturncode As Boolean
                ' get latest TimeStamp
                Dim bClaimBuilder As Boolean
                Boolean.TryParse(Session(CNClaimBuilder), bClaimBuilder)
                If bClaimBuilder = True Then
                    oclaimRisk = GetClaimRiskCall(oClaimDetails.BaseClaimKey, oClaimDetails.ClaimKey, sBranchCode)
                    Session.Item(CNClaimRiskTimeStamp) = oclaimRisk.TimeStamp

                    'Claim Risk has wrong argument called ClaimKey, actually it is BaseClaimKey
                    oclaimRisk.ClaimKey = oClaimDetails.BaseClaimKey
                    oclaimRisk.TimeStamp = Session.Item(CNClaimRiskTimeStamp)
                    oclaimRisk.XMLDataSet = Session.Item(CNDataSet)
                End If

                If Session(CNMode) = Mode.NewClaim Then
                    Boolean.TryParse(Session(CNClaimBuilder), bClaimBuilder)
                    If bClaimBuilder = True Then
                        'Update the claim builder risk screen
                        If Not UpdateClaimRiskCall(oclaimRisk, sBranchCode) Then
                            Exit Sub
                        End If
                    End If

                    If sDisplayReinsurance = "1" AndAlso oUserAuthority.UserAuthorityValue = "1" AndAlso bDisplayReinsuranceWeb AndAlso oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.RiskTypes.RiskType(Trim(oClaimDetails.RiskType)).DisplayClaimReinsurance = True Then
                        Response.Redirect("~/claims/ClaimReinsurance.aspx")
                    Else
                        'Fire the Bind Claim
                        If BindClaimCall(oOriginalClaim, bClaimTimeStamp, 1, Nothing, sBranchCode) Is Nothing Then
                            Exit Sub
                        End If
                    End If

                ElseIf Session(CNMode) = Mode.EditClaim Then

                    Boolean.TryParse(Session(CNClaimBuilder), bClaimBuilder)
                    If bClaimBuilder = True Then
                        'Update the claim builder risk screen
                        'arch issue 268
                        If Not UpdateClaimRiskCall(oclaimRisk, sBranchCode) Then
                            Exit Sub
                        End If
                    End If

                    If sDisplayReinsurance = "1" AndAlso oUserAuthority.UserAuthorityValue = "1" _
                    AndAlso oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.RiskTypes.RiskType(Trim(oClaimDetails.RiskType)).DisplayClaimReinsurance = True Then
                        Response.Redirect("~/claims/ClaimReinsurance.aspx")
                    Else
                        'Fire the Bind Claim
                        If BindClaimCall(oOriginalClaim, bClaimTimeStamp, 2, Nothing, sBranchCode) Is Nothing Then
                            Exit Sub
                        End If
                    End If
                ElseIf (CType(Session.Item(CNMode), Mode) = Mode.SalvageClaim Or CType(Session.Item(CNMode), Mode) = Mode.TPRecovery) Then
                    oreturncode = UpdateClaimRiskCall(oclaimRisk, sBranchCode)
                    BindClaimCall(oOriginalClaim, bClaimTimeStamp, 2, Nothing, sBranchCode)

                    'error parul
                ElseIf Session(CNMode) = Mode.PayClaim Then
                    For Each oControl In oMaster.Controls
                        'check whether controls "claimsprogressbar.ascx" exist on this page
                        If oControl.FindControl("hidChkCBuilderChoice") IsNot Nothing Then
                            hidChkCBuilderChoice = CType(oControl.FindControl("hidChkCBuilderChoice"), HiddenField)
                        End If
                    Next

                    'Fire the Bind Claim
                    Dim PerilsIndex As New System.Collections.Generic.List(Of Integer)
                    Dim oPayment As New NexusProvider.ClaimPayment
                    Dim oPaymentCollection As New NexusProvider.ClaimPaymentCollection
                    If Session(CNClaimMultiPerilIndex) IsNot Nothing Then
                        PerilsIndex = Session(CNClaimMultiPerilIndex)
                    Else
                        PerilsIndex.Add(CInt(Session(CNClaimPerilIndex)))
                        oPayment = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(Session(CNClaimPerilIndex)).Payment
                        If (hidChlClaimClose IsNot Nothing) Then
                            If (hidChlClaimClose.Value.Trim.ToUpper = "TRUE") Then
                                oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = True
                                oPayment.CloseClaimOnZeroReserveRecoveryBalance = True
                            ElseIf (hidChlClaimClose.Value.Trim.ToUpper = "FALSE") Then
                                oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = False
                                oPayment.CloseClaimOnZeroReserveRecoveryBalance = False
                            End If

                        ElseIf hidChkCBuilderClaimClose IsNot Nothing Then
                            If (hidChkCBuilderClaimClose.Value.Trim.ToUpper = "TRUE") Then
                                oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = True
                                oPayment.CloseClaimOnZeroReserveRecoveryBalance = True
                            ElseIf (hidChkCBuilderClaimClose.Value.Trim.ToUpper = "FALSE") Then
                                oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = False
                                oPayment.CloseClaimOnZeroReserveRecoveryBalance = False
                            End If
                        Else
                            oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = False
                            oPayment.CloseClaimOnZeroReserveRecoveryBalance = False
                        End If
                    End If

                    For Each perilIndex As Integer In PerilsIndex
                        oPayment = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(perilIndex).Payment
                        'Need to close the claim if Payment Amount is exceeding or equal to reserve amount for any peril
                        If (hidChlClaimClose IsNot Nothing) Then
                            If (hidChlClaimClose.Value.Trim.ToUpper = "TRUE") Then
                                oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = True
                                oPayment.CloseClaimOnZeroReserveRecoveryBalance = True
                            ElseIf (hidChlClaimClose.Value.Trim.ToUpper = "FALSE") Then
                                oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = False
                                oPayment.CloseClaimOnZeroReserveRecoveryBalance = False
                            End If

                        ElseIf hidChkCBuilderClaimClose IsNot Nothing Then
                            If (hidChkCBuilderClaimClose.Value.Trim.ToUpper = "TRUE") Then
                                oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = True
                                oPayment.CloseClaimOnZeroReserveRecoveryBalance = True
                            ElseIf (hidChkCBuilderClaimClose.Value.Trim.ToUpper = "FALSE") Then
                                oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = False
                                oPayment.CloseClaimOnZeroReserveRecoveryBalance = False
                            End If
                        Else
                            oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = False
                            oPayment.CloseClaimOnZeroReserveRecoveryBalance = False
                        End If
                        oPaymentCollection.Add(oPayment)
                    Next





                    If CType(Session(CNEnablePayClaim), Boolean) = False Then
                        'When user Press Finish without  filling pay information (Amount=0)
                        Dim cstInvalidDataset As New CustomValidator
                        cstInvalidDataset.IsValid = False
                        oResource = New ResXResourceReader(HttpContext.Current.Server.MapPath(AppSettings("WebRoot") & "Controls/App_LocalResources/Perils.ascx.resx"))
                        en = oResource.GetEnumerator()
                        While (en.MoveNext)
                            If en.Key.ToString.Trim = "lbl_PaymentReceived_Error" Then
                                cstInvalidDataset.ErrorMessage = en.Value
                            End If
                        End While
                        cstInvalidDataset.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                        Page.Validators.Add(cstInvalidDataset)
                        Exit Sub
                    End If

                    'Check to move to the  CashPaymentProcess 
                    If oRunClaimWorkFlow Is Nothing Then
                        'get the Claim Workflow Setting
                        oRunClaimWorkFlow = oWebservice.GetProductClaimsWorkflowOptions(NexusProvider.ClaimProcessType.ClaimPayment, oQuote.ProductCode)
                        ViewState("RunClaimWorkFlow") = oRunClaimWorkFlow
                    End If
                    oRunClaimWorkFlow = ViewState("RunClaimWorkFlow")

                    'Before Redirect , Check CashPaymentProcess 
                    'get the Product Risk option setting named Run Authorize Payment
                    sRunAuthorizePayment = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.RunAuthorisationScriptsClaimPayments, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
                    If oRunClaimWorkFlow.CashPaymentProcess = True AndAlso (sRunAuthorizePayment = "0" Or sRunAuthorizePayment Is Nothing) Then
                        Dim dAmount As Decimal = 0.0
                        If Session(CNClaimMultiPerilIndex) IsNot Nothing Then
                            PerilsIndex = Session(CNClaimMultiPerilIndex)
                        Else
                            PerilsIndex.Add(CInt(Session(CNClaimPerilIndex)))
                        End If
                        For Each PerilItemIndex As Integer In PerilsIndex
                            If CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(PerilItemIndex).ClaimReserve IsNot Nothing Then
                                For Each oPaymentItem As NexusProvider.ClaimPerilReservePaymentType In CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(PerilItemIndex).ClaimReserve
                                    dAmount += oPaymentItem.ThisPaymentINCLTax
                                Next
                            End If
                        Next


                        If dAmount > 0 Then
                            'Amount is positive so need to receive it
                            Response.Redirect("~/secure/payment/CashListNew.aspx", False)
                        End If

                    End If

                    Dim bPaymentAuthorized As Boolean = False
                    Try

                        Boolean.TryParse(Session(CNClaimBuilder), bClaimBuilder)
                        If bClaimBuilder = True Then
                            ''Update the claim builder risk screen
                            If Not UpdateClaimRiskCall(oclaimRisk, sBranchCode) Then
                                Exit Sub
                            End If
                        End If

                        If sDisplayReinsurance = "1" AndAlso oUserAuthority.UserAuthorityValue = "1" AndAlso bDisplayReinsuranceWeb AndAlso oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.RiskTypes.RiskType(Trim(oClaimDetails.RiskType)).DisplayClaimReinsurance = True Then
                            Response.Redirect("~/claims/ClaimReinsurance.aspx")
                        Else
                            If Session(CNMode) = Mode.PayClaim AndAlso (Session(CNLockPaymentGrid) Is Nothing OrElse (Session(CNLockPaymentGrid) IsNot Nothing AndAlso Session(CNLockPaymentGrid) = True)) Then
                                'Fire the Bind Claim
                                If (Session(CNClaimMultiPerilIndex) IsNot Nothing) Then
                                    BindClaimCall(oOriginalClaim, bClaimTimeStamp, 4, Nothing, sBranchCode, bPaymentAuthorized, oPaymentCollection:=oPaymentCollection)
                                Else
                                    BindClaimCall(oOriginalClaim, bClaimTimeStamp, 4, oPayment, sBranchCode, bPaymentAuthorized)
                                End If
                            Else
                                'Fire the Bind Claim
                                If (Session(CNClaimMultiPerilIndex) IsNot Nothing) Then
                                    BindClaimCall(oOriginalClaim, bClaimTimeStamp, 3, Nothing, sBranchCode, bPaymentAuthorized, oPaymentCollection:=oPaymentCollection)
                                Else
                                    BindClaimCall(oOriginalClaim, bClaimTimeStamp, 3, oPayment, sBranchCode, bPaymentAuthorized)
                                End If
                            End If
                        End If

                        If oRunClaimWorkFlow.MakeFurtherPayments = True Then
                            If (hidChkChoice IsNot Nothing) Then
                                If (hidChkChoice.Value.Trim.ToUpper = "TRUE") Then
                                    GetLatestDetails() 'Update the session with latest values
                                    Session.Remove(CNEnablePayClaim) 'Remove the ReadOnly mode of the Pay Claim
                                    Session(CNPayClaim) = Nothing ' Reset the pay claim to accept the another payment
                                    Dim sUrl As String = CheckClaimBuilder()
                                    Response.Redirect(sUrl, False)
                                ElseIf (hidChkChoice.Value.Trim.ToUpper = "FALSE") Then
                                    Response.Redirect("~/Claims/Complete.aspx", False)
                                End If
                            ElseIf hidChkCBuilderChoice IsNot Nothing Then
                                If hidChkCBuilderChoice.Value.Trim.ToUpper = "TRUE" Then
                                    GetLatestDetails() 'Update the session with latest values
                                    Session.Remove(CNEnablePayClaim) 'Remove the ReadOnly mode of the Pay Claim
                                    Session(CNPayClaim) = Nothing ' Reset the pay claim to accept the another payment
                                    'Dummy Cal of PayClaim, to retreive back the latetst claim key
                                    PayClaimWithZeroAmount()
                                    If CBool(Session(CNIsClaimLocked)) <> True Then
                                        Dim sUrl As String = CheckClaimBuilder()
                                        Response.Redirect(sUrl, False)
                                        Exit Sub
                                    End If

                                ElseIf hidChkCBuilderChoice.Value.Trim.ToUpper = "FALSE" Then
                                    Response.Redirect("~/Claims/Complete.aspx", False)
                                End If
                            End If
                        Else
                            If bPaymentAuthorized Then
                                Session(CNAuthorizeStatus) = ""
                            End If
                        End If

                    Catch ex As NexusProvider.NexusException
                        If ex.Errors(0).Code = "331" Then   'Code : 331 :: Description: DebtorUserGroupsAreNotSetup

                            Dim cstDebtorUserGroups As New CustomValidator
                            cstDebtorUserGroups.IsValid = False
                            'look for a validation message in the page resources, but if there is not one defined add a default message
                            cstDebtorUserGroups.ErrorMessage = IIf(GetLocalResourceObject("cstDebtorUserGroups") Is Nothing, "Debtor User Groups are not setup. Please contact your system administrator", GetLocalResourceObject("cstDebtorUserGroups"))
                            cstDebtorUserGroups.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                            'add the validator to the page, this will have the effect of making the page invalid
                            Page.Validators.Add(cstDebtorUserGroups)
                            Exit Sub
                        Else
                            Throw
                        End If
                    End Try
                ElseIf Session(CNMode) = Mode.ViewClaim Then
                    If sDisplayReinsurance = "1" AndAlso oUserAuthority.UserAuthorityValue = "1" _
                    AndAlso oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.RiskTypes.RiskType(Trim(oClaimDetails.RiskType)).DisplayClaimReinsurance = True Then
                        Response.Redirect("~/claims/ClaimReinsurance.aspx")
                    End If
                End If

                ' Update the Claims session variable
                GetClaimDetails(oClaimDetails.ClaimKey, oclaimRisk)
                Response.Redirect("~/claims/complete.aspx", False)

            End If
        End Sub
        ''' <summary>
        ''' When ShowSummary” is set to “True” at portal level and user Press Next of last claim builder page or Finish
        ''' </summary>
        ''' <remarks></remarks>
        Sub PreSummaryPageRedirect()

            If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Then
                'show the summary page
                Response.Redirect("~/claims/summary.aspx?ReturnUrl=" & Request.Path.Replace(WebRoot, "~/"))
            ElseIf Session(CNMode) = Mode.PayClaim Then
                If CType(Session(CNEnablePayClaim), Boolean) = False Then
                    'When user Press Finish without  filling pay information (Amount=0)
                    Dim cstInvalidDataset As New CustomValidator
                    cstInvalidDataset.IsValid = False
                    oResource = New ResXResourceReader(HttpContext.Current.Server.MapPath(AppSettings("WebRoot") & "Controls/App_LocalResources/Perils.ascx.resx"))
                    en = oResource.GetEnumerator()
                    While (en.MoveNext)
                        If en.Key.ToString.Trim = "lbl_PaymentReceived_Error" Then
                            cstInvalidDataset.ErrorMessage = en.Value
                        End If
                    End While
                    cstInvalidDataset.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                    Page.Validators.Add(cstInvalidDataset)
                    Exit Sub
                Else

                    'Before Redirect , Check CashPaymentProcess 
                    Dim sRunAuthorizePayment As String
                    Dim sIsPaymentsReadOnly As String
                    Dim oRunClaimWorkFlow As NexusProvider.ProductClaimsWorkflowOptionsValue
                    Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                    'get the Product Risk option setting named Run Authorize Payment
                    sRunAuthorizePayment = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.RunAuthorisationScriptsClaimPayments, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
                    sIsPaymentsReadOnly = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsPaymentsReadOnly, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
                    'get the Claim Workflow Setting
                    oRunClaimWorkFlow = oWebservice.GetProductClaimsWorkflowOptions(NexusProvider.ClaimProcessType.ClaimPayment, oQuote.ProductCode)

                    Session.Remove(CNAuthorizeStatus)
                    If sRunAuthorizePayment = "1" Then
                        'Set "Authorize Payment" Session ON
                        Session(CNAuthorizeStatus) = "Authorize Payment"
                        'show the summary page
                        Response.Redirect("~/claims/summary.aspx?ReturnUrl=" & Request.Path.Replace(WebRoot, "~/"))
                    ElseIf oRunClaimWorkFlow.CashPaymentProcess = True Then
                        Dim dAmount As Decimal = 0.0
                        Dim PerilsIndex As New System.Collections.Generic.List(Of Integer)
                        If Session(CNClaimMultiPerilIndex) IsNot Nothing Then
                            PerilsIndex = Session(CNClaimMultiPerilIndex)
                        Else
                            PerilsIndex.Add(CInt(Session(CNClaimPerilIndex)))
                        End If
                        For Each PerilItemIndex In PerilsIndex
                            If CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(PerilItemIndex).ClaimReserve IsNot Nothing Then
                                For Each oPaymentItem As NexusProvider.ClaimPerilReservePaymentType In CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(PerilItemIndex).ClaimReserve
                                    dAmount += oPaymentItem.ThisPaymentINCLTax
                                Next
                            End If
                        Next

                        If dAmount > 0 Then
                            'Amount is positive so need to receive it
                            Response.Redirect("~/secure/payment/CashListNew.aspx", False)
                        Else
                            'Amount is negative so no need to receive it , so process it directly
                            Response.Redirect("~/claims/summary.aspx?ReturnUrl=" & Request.Path.Replace(WebRoot, "~/"))
                        End If
                    Else
                        'If RunAuthorize is OFF and Cash Payment Process is OFF
                        Response.Redirect("~/claims/summary.aspx?ReturnUrl=" & Request.Path.Replace(WebRoot, "~/"))
                    End If

                End If
            ElseIf Session(CNMode) = Mode.Authorise OrElse Session(CNMode) = Mode.Recommend Or Session(CNMode) = Mode.DeclinePayment Then
                Response.Redirect("~/claims/summary.aspx", False)
            ElseIf Session(CNMode) = Mode.ViewClaim Then
                'show the summary page
                Response.Redirect("~/claims/summary.aspx?ReturnUrl=" & Request.Path.Replace(WebRoot, "~/"))
            End If
        End Sub

        ''' <summary>
        ''' Show specified tab ID in tab index control
        ''' </summary>
        ''' <param name="TabID">ID of tab, as found in quick / fullquote.config</param>
        ''' <remarks></remarks>
        Public Sub ShowTab(ByVal TabID As String)
            'get the hashtable of tabs from session
            Dim oTabState As Hashtable = CType(Session.Item(CNTabState & "_" & sTabIndexControlID), Hashtable)
            If oTabState Is Nothing Then
                'need to create a new hashtable
                oTabState = New Hashtable
            End If
            If oTabState.Contains(TabID) Then
                'set the value for this to true to make it visible regardless of the setting in config
                oTabState(TabID) = True
            Else
                'add an entry to the hash table to ensure this tab is visible
                oTabState.Add(TabID, True)
            End If
            'put the hashtable into session
            Session(CNTabState & "_" & sTabIndexControlID) = oTabState
        End Sub

        ''' <summary>
        ''' Show specified tab ID in tab index control
        ''' </summary>
        ''' <param name="TabID">ID of tab, as found in quick / fullquote.config</param>
        ''' <remarks></remarks>
        Public Sub HideTab(ByVal TabID As String)
            'get the hashtable of tabs from session
            Dim oTabState As Hashtable = CType(Session.Item(CNTabState & "_" & sTabIndexControlID), Hashtable)
            If oTabState Is Nothing Then
                'need to create a new hashtable
                oTabState = New Hashtable
            End If
            If oTabState.Contains(TabID) Then
                'set the value for this to false to hide tab
                oTabState(TabID) = False
            Else
                'add an entry to the hash table to ensure this tab is hidden
                oTabState.Add(TabID, False)
            End If
            'put the hashtable into session
            Session(CNTabState & "_" & sTabIndexControlID) = oTabState
        End Sub

#End Region
        ''' <summary>
        ''' Removes the UC attrinbute to identify the valid/invalid node
        ''' </summary>
        ''' <param name="sOI"></param>
        ''' <remarks></remarks>
        Sub ResetUCElement(ByVal sOI As String)
            'Remove the UC attrinbute to identify the valid/invalid node
            If Current.Session(CNDataSet) IsNot Nothing Then
                Dim srDataset As New System.IO.StringReader(Current.Session(CNDataSet))
                Dim xmlTR As New XmlTextReader(srDataset)
                Dim Doc As New XmlDocument

                Doc.Load(xmlTR)
                xmlTR.Close()

                'Dim oDefaultNodes As New Hashtable()
                Dim oNode As XmlNode = Doc.SelectSingleNode("//*[@OI = '" & sOI & "']")

                Dim xUCAttr As XmlAttribute
                xUCAttr = oNode.Attributes("UC")
                If xUCAttr IsNot Nothing Then
                    oNode.Attributes.Remove(xUCAttr)
                End If

                Dim swContent As New System.IO.StringWriter
                Dim xmlwContent As New XmlTextWriter(swContent)

                Doc.WriteTo(xmlwContent)
                Current.Session(CNDataSet) = swContent.ToString()

                xmlwContent.Close()
                swContent.Close()
            End If
        End Sub

        ''' <summary>
        ''' Add the UC attrinbute to identify the valid/invalid node
        ''' </summary>
        ''' <param name="sOI"></param>
        ''' <remarks></remarks>
        Sub AddUCElement(ByVal sOI As String)
            'Add the UC attrinbute to identify the valid/invalid node
            If Current.Session(CNDataSet) IsNot Nothing Then
                Dim srDataset As New System.IO.StringReader(Current.Session(CNDataSet))
                Dim xmlTR As New XmlTextReader(srDataset)
                Dim Doc As New XmlDocument

                Doc.Load(xmlTR)
                xmlTR.Close()

                'Dim oDefaultNodes As New Hashtable()
                Dim oNode As XmlNode = Doc.SelectSingleNode("//*[@OI = '" & sOI & "']")

                'Add the UC attrinbute to identify the valid/invalid node
                Dim xUCAttr As XmlAttribute
                xUCAttr = oNode.Attributes("UC")
                If xUCAttr IsNot Nothing Then
                    xUCAttr.Value = "1"
                Else
                    xUCAttr = oNode.OwnerDocument.CreateAttribute("UC")
                    oNode.Attributes.Append(xUCAttr)
                    oNode.Attributes("UC").Value = "1"
                End If

                Dim swContent As New System.IO.StringWriter
                Dim xmlwContent As New XmlTextWriter(swContent)

                Doc.WriteTo(xmlwContent)
                Current.Session(CNDataSet) = swContent.ToString()

                xmlwContent.Close()
                swContent.Close()

            End If
        End Sub
        ''' <summary>
        ''' Stores Previous Node if user is editing the record so that we can use it in case user 
        ''' cancels before clicking finish
        ''' </summary>
        ''' <param name="sOI"></param>
        ''' <remarks></remarks>
        Sub StorePreviousNode(ByVal sOI As String)
            'Need to store the prevoius copy of XMLDataset
            'If user is editing the records
            If Current.Session(CNDataSet) IsNot Nothing Then
                Dim hCurrentNodeColl As New Hashtable()
                If Session(CNNode) IsNot Nothing Then
                    hCurrentNodeColl = CType(Session(CNNode), Hashtable)
                End If

                Dim Doc As New XmlDocument
                Doc.LoadXml(Current.Session(CNDataSet))

                Dim oNode As XmlNode = Doc.SelectSingleNode("//*[@OI='" & sOI & "']")

                'check if key already exists
                If hCurrentNodeColl.Item(sOI) Is Nothing Then
                    'Added into collection
                    hCurrentNodeColl.Add(sOI, oNode.OuterXml)
                Else
                    hCurrentNodeColl.Item(sOI) = oNode.OuterXml
                End If
                Session(CNNode) = hCurrentNodeColl
            End If
        End Sub
    End Class

End Namespace
