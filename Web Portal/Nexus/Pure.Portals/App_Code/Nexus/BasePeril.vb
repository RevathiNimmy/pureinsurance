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
Imports CMS.Library.Portal
Imports System.Resources
Imports Nexus.Constants.Constant

Namespace Nexus

    Public MustInherit Class BasePeril : Inherits CMS.Library.Frontend.clsCMSPage

#Region " Private Fields "

        Private oMaster As ContentPlaceHolder

        Private sOI As String = String.Empty

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

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            sOI = Request("OI")

            'we've moved back up the dataset tree from child to parent, so we need to check
            If Session(CNDataSet) IsNot Nothing Then
                Dim srDataset As New System.IO.StringReader(Session(CNDataSet))
                Dim xmlTR As New XmlTextReader(srDataset)
                Dim Doc As New XmlDocument
                Doc.Load(xmlTR)
                xmlTR.Close()

                Dim oNode As XmlNode = Doc.SelectSingleNode("//*[@OI='" & sOI & "' and @US='3']")
                srDataset.Dispose()

                If oNode IsNot Nothing Then
                    Dim oSAMClient As New SiriusFS.SAM.Client.DataSetControl.Application
                    oSAMClient.LoadFromXML(ClaimGetDataSetDefinition(), Session(CNDataSet))
                    oSAMClient.DelObjectInstance(oNode.Name, sOI)
                    oSAMClient.ReturnAsXML(Session(CNDataSet))
                    oSAMClient.Terminate()
                End If
            End If


            'When PayClaim SAM Method return an Warnings Message , Based on OK & Cancel operation  PayClaimWarningConfirmation do The Rest Of Operation.
            PayClaimWarningConfirmation()

            If IsPostBack Then
                DataSetFunctions.ReadContainerFromXML(oMaster, sOI, Me, True)
            Else
                DataSetFunctions.ReadContainerFromXML(oMaster, sOI, Me)
            End If

            ViewState.Add("OI", sOI)
            If Request("__EVENTARGUMENT") = "ProcessClaimPaymentNextButton" Then
                ProcessClaimNextButton()
            ElseIf Request("__EVENTARGUMENT") = "ProcessClaimPaymentFinishButton" Then
                ProcessClaimFinishButton()
            End If
        End Sub

        Private Shadows Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

            If Not IsPostBack Then
                Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(GetPortalID())

                If Current.Session.Item(CNDataModelCode) Is Nothing AndAlso Current.Session(CNDataSet) IsNot Nothing Then

                    'Read DataModelCode from DataSet if it's not already in session
                    Dim sDataModelCode As String = String.Empty
                    Dim Doc As XPathDocument = New XPathDocument(New IO.StringReader(Current.Session(CNDataSet)))
                    Dim Navigator As XPathNavigator
                    Navigator = Doc.CreateNavigator()

                    Dim i As XPathNodeIterator = Navigator.Select("DATA_SET")

                    While (i.MoveNext)
                        sDataModelCode = i.Current.GetAttribute("DataModelCode", String.Empty)
                    End While

                    Current.Session.Item(CNDataModelCode) = sDataModelCode

                End If

                If Session(CNClaim) IsNot Nothing Then
                    Dim oClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                    Dim PerilCode As String = String.Empty
                    Dim iPerilID As Integer
                    'Check whether return url is passsed or not, it will be passed only when first peril
                    'build screen is loaded, if used move from another peril builder page then it will not be
                    'passed thus it should be checked from session and need to preserve it 
                    If Request.QueryString("ReturnUrl") IsNot Nothing Then
                        Session(CNPerilReturnURL) = Request.QueryString("ReturnUrl")
                    End If

                    If Request("PerilIndex") IsNot Nothing Then
                        Session(CNClaimPerilIndex) = Request("PerilIndex")
                    End If

                    If Request("PerilID") IsNot Nothing Then
                        iPerilID = Request("PerilID")
                        For Each oClaimPeril As NexusProvider.PerilSummary In oClaim.ClaimPeril
                            If oClaimPeril.ClaimPerilKey = iPerilID Then
                                PerilCode = Trim(oClaimPeril.TypeCode)
                                Session(CNClaimPerilKey) = iPerilID
                            End If
                        Next
                    Else
                        For Each oClaimPeril As NexusProvider.PerilSummary In oClaim.ClaimPeril
                            If oClaimPeril.ClaimPerilKey = Session(CNClaimPerilKey) Then
                                PerilCode = Trim(oClaimPeril.TypeCode)
                            End If
                        Next
                    End If

                    'Dim sFolder As String
                    'If oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.PerilTypes.PerilType(Trim(PerilCode)) IsNot Nothing Then
                    '    sFolder = AppSettings("WebRoot") & "Claims/ClientPages/" & oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.ScreenLocation & "/Perils/" _
                    '                      & oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.PerilTypes.PerilType(PerilCode).Folder
                    'Else
                    '    sFolder = String.Empty
                    'End If

                    'Dim sXMLPath As String = Server.MapPath(sFolder & "/perilscreens.config")

                    'Dim bClaimBuilder As Boolean
                    'Boolean.TryParse(Session(CNClaimBuilder), bClaimBuilder)

                    ' If IO.File.Exists(sXMLPath) = True AndAlso bClaimBuilder = True Then

                    'Dim xmlds As New XmlDataSource
                    'xmlds.DataFile = sXMLPath
                    'xmlds.EnableCaching = False

                    'Dim Navigator As XPathNavigator
                    'Dim Doc As XPathDocument = New XPathDocument(sXMLPath)
                    'Navigator = Doc.CreateNavigator()
                    'Dim i As XPathNodeIterator

                    'i = Navigator.Select("/screens/screen/tab[1]")

                    'Dim sFirstPage As String = String.Empty

                    'While (i.MoveNext)
                    '    sFirstPage = i.Current.GetAttribute("url", String.Empty)
                    'End While

                    ''We have check whether this condition is required or not
                    'If Not IsCurrentPage(sFirstPage) Then

                    '    'We're not on the first risk screen and we really should be

                    '    'stops all the processing being down again on then correct
                    '    'risks screen, as we've already collected all the info we need
                    '    Session(CNQuoteInSync) = True

                    '    Response.Redirect(sFolder & "/" & sFirstPage)

                    'End If
                    'End If
                End If
            End If

            oMaster = GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName)

            Dim oTabIndex As ImprovedTabIndex = CType(oMaster.FindControl(sTabIndexControlID), ImprovedTabIndex)

            If oTabIndex IsNot Nothing Then
                AddHandler oTabIndex.ValuesInitialized, AddressOf TabIndexInitialized
                AddHandler oTabIndex.TabClicked, AddressOf TabClick

                If CType(Session.Item(CNMode), Mode) = Mode.ViewClaim _
                Or CType(Session.Item(CNMode), Mode) = Mode.Authorise Or CType(Session.Item(CNMode), Mode) = Mode.Recommend _
                Or CType(Session.Item(CNMode), Mode) = Mode.DeclinePayment Or CType(Session.Item(CNMode), Mode) = Mode.ViewClaimPayment Then
                    DisableControls(oMaster)
                End If
            End If

            'oMaster.Controls.AddAt(0, New LiteralControl("<h1>Claim Information</h1>"))

            Dim oFrameWorkNav As FrameWorkNav = oMaster.FindControl("ClaimNav")

            If oFrameWorkNav IsNot Nothing Then
                If CType(Session.Item(CNMode), Mode) <> Mode.NewClaim And CType(Session.Item(CNMode), Mode) <> Mode.PayClaim Then
                    If HttpContext.Current.Session.IsCookieless Then
                        oFrameWorkNav.AddLink("Search Results", "../Claims/FindClaim.aspx") 'Resource File!
                        oFrameWorkNav.AddLink("Claim Overview", "../Claims/overview.aspx") 'Resource File!
                    Else
                        oFrameWorkNav.AddLink("Search Results", AppSettings("webroot") & "Claims/FindClaim.aspx") 'Resource File!
                        oFrameWorkNav.AddLink("Claim Overview", AppSettings("webroot") & "Claims/overview.aspx") 'Resource File!
                    End If
                End If
                If HttpContext.Current.Session.IsCookieless Then
                    oFrameWorkNav.AddLink("Claim Reserves", "../Claims/perils.aspx") 'Resource File!
                Else
                    oFrameWorkNav.AddLink("Claim Reserves", AppSettings("webroot") & "Claims/perils.aspx") 'Resource File!
                End If
                '  If CType(Session.Item(CNMode), Mode) <> Mode.NewClaim Then
                If CType(Session.Item(CNMode), Mode) = Mode.ViewClaim _
                    Or CType(Session.Item(CNMode), Mode) = Mode.Authorise Or CType(Session.Item(CNMode), Mode) = Mode.Recommend _
                    Or CType(Session.Item(CNMode), Mode) = Mode.DeclinePayment Or CType(Session.Item(CNMode), Mode) = Mode.ViewClaimPayment Then
                    DisableControls(oMaster)
                End If

                If HttpContext.Current.Session.IsCookieless Then
                    oFrameWorkNav.AddLink("Summary", "../Claims/summary.aspx") 'Resource File!
                Else
                    oFrameWorkNav.AddLink("Summary", AppSettings("webroot") & "Claims/summary.aspx") 'Resource File!
                End If

                ' End If
            End If

            Response.Cache.SetCacheability(HttpCacheability.NoCache)

        End Sub

#End Region

#Region " Private Methods "

        Public Sub BackButton(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))

            If sPrevPage <> String.Empty Then
                BackButtonPageRedirect()
                Response.Redirect(sPrevPage, False)
            Else
                If Session(CNClaimBuilder) IsNot Nothing AndAlso Session(CNClaimBuilder) = True Then
                    'If Claim Builder is ON
                    If CType(Session.Item(CNMode), Mode) = Mode.Authorise Or CType(Session.Item(CNMode), Mode) = Mode.Recommend _
                                Or CType(Session.Item(CNMode), Mode) = Mode.DeclinePayment Or CType(Session.Item(CNMode), Mode) = Mode.ViewClaimPayment Then
                        If InStr(Session(CNPerilReturnURL), "Perils.aspx") > 0 Then
                            Response.Redirect("~/claims/overview.aspx", False)
                        Else
                            Session.Remove(CNClaimPerilKey)
                            If Session(CNPerilReturnURL) IsNot Nothing Then
                                Response.Redirect(Session(CNPerilReturnURL), False)
                            End If
                        End If
                    Else
                        If Session(CNEnablePayClaim) Is Nothing AndAlso Session(CNMode) = Mode.PayClaim Then
                            CType(Session.Item(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iPeril).Payment = Nothing
                            Session.Remove(CNLockPaymentGrid)
                        End If
                        ' Session(CNOI) = Session(CNCurrentOI)
                        Session.Remove(CNClaimPerilKey)
                        BackButtonPageRedirect()
                        If Session(CNPerilReturnURL) IsNot Nothing Then
                            Response.Redirect(Session(CNPerilReturnURL), False)
                        Else
                            Response.Redirect("~/claims/perils.aspx", False)
                        End If
                    End If
                Else
                    'If Claim Builder is OFF
                    If CType(Session.Item(CNMode), Mode) = Mode.Authorise Or CType(Session.Item(CNMode), Mode) = Mode.Recommend _
                                Or CType(Session.Item(CNMode), Mode) = Mode.DeclinePayment Or CType(Session.Item(CNMode), Mode) = Mode.ViewClaimPayment Then
                        If InStr(Session(CNPerilReturnURL), "Perils.aspx") > 0 Then
                            Response.Redirect("~/claims/overview.aspx", False)
                        End If
                    Else
                        If Session(CNEnablePayClaim) Is Nothing AndAlso Session(CNMode) = Mode.PayClaim Then
                            CType(Session.Item(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iPeril).Payment = Nothing
                        ElseIf Session(CNEnablePayClaim) Is Nothing AndAlso Session(CNMode) = Mode.SalvageClaim Then
                            'Keep ThisReceiptINCLTax amount blank so that peril page server validator IsPaymentReceived_ServerValidate can restrict to go next
                            'keep  LossThisNet amount blank to reduse new added Amount in incurred
                            With CType(Session.Item(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iPeril)
                                For iCount As Integer = 0 To .SalvageRecovery.Count - 1
                                    CType(Session.Item(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iPeril).SalvageRecovery(iCount).ThisReceiptINCLTax = 0
                                    CType(Session.Item(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iPeril).SalvageRecovery(iCount).LossThisNet = 0
                                Next
                            End With
                        ElseIf Session(CNEnablePayClaim) Is Nothing AndAlso Session(CNMode) = Mode.TPRecovery Then
                            'Keep ThisReceiptINCLTax amount blank so that peril page server validator IsPaymentReceived_ServerValidate can restrict to go next 
                            'keep  LossThisNet amount blank to reduse new added Amount in incurred
                            With CType(Session.Item(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iPeril)
                                For iCount As Integer = 0 To .TPRecovery.Count - 1
                                    CType(Session.Item(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iPeril).TPRecovery(iCount).ThisReceiptINCLTax = 0
                                    CType(Session.Item(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iPeril).TPRecovery(iCount).LossThisNet = 0
                                Next
                            End With
                        End If
                        Response.Redirect("~/claims/perils.aspx", False)
                    End If
                End If
            End If
        End Sub
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
                Dim bSubmitFromAnyPage As Boolean = oNexusConfig.Portals.Portal(GetPortalID()).Products.Product(sProductCode).SubmitFromAnyPage
                If bSubmitFromAnyPage Then
                    sender.visible = True
                Else
                    sender.visible = False
                End If
            End If
        End Sub
        Private Function ValidateDataset() As String
            Dim strValidationMsg As String = String.Empty
            If Session(CNDataSet) IsNot Nothing Then
                'Code for Running XSLT Validation
                'Declaration of the Vairables used
                Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(GetPortalID())
                Dim sbOutput As New StringBuilder
                Dim xmlTR As New XmlTextReader(New System.IO.StringReader(Session(CNDataSet))) ' xml from session
                Dim xInput As New XmlDocument
                Dim oClaimQuote As NexusProvider.Quote = Session(CNClaimQuote)

                'check the PerilCode
                Dim oClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                Dim PerilCode As String = String.Empty
                For Each oClaimPeril As NexusProvider.PerilSummary In oClaim.ClaimPeril
                    If oClaimPeril.ClaimPerilKey = Session(CNClaimPerilKey) Then
                        PerilCode = Trim(oClaimPeril.TypeCode)
                    End If
                Next
                'Search for the Peril Folder Path
                Dim sFolder As String
                If oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.PerilTypes.PerilType(Trim(PerilCode)) IsNot Nothing Then
                    If HttpContext.Current.Session.IsCookieless Then
                        sFolder = "../Claims/ClientPages/" & oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.ScreenLocation & "/Perils/" _
                                          & oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.PerilTypes.PerilType(PerilCode).Folder
                    Else
                        sFolder = AppSettings("WebRoot") & "Claims/ClientPages/" & oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.ScreenLocation & "/Perils/" _
                                          & oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.PerilTypes.PerilType(PerilCode).Folder
                    End If
                Else
                    sFolder = String.Empty
                End If

                Dim SPerilValidatorPath As String = Server.MapPath(sFolder) & "\" & sScreenCode & ".xslt"

                'A check for validation file exist
                If (System.IO.File.Exists(SPerilValidatorPath)) Then

                    xInput.Load(xmlTR)
                    Dim xslDoc As New Xsl.XslCompiledTransform 'This should load the relevant validator file from the current product folder
                    xslDoc.Load(SPerilValidatorPath)
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

                'This will only run if validations  are fine on all the pages.
                'Code for Running SAM Validation Rules

                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim v_sXMLDataSet As String


                v_sXMLDataSet = Session(CNDataSet)
                If String.IsNullOrEmpty(sScreenCode) Then
                    sScreenCode = oPortal.Claims.CorePerilScreenCode
                End If
                Try
                    oWebService.RunValidationRules(sScreenCode, v_sXMLDataSet, CInt(Session(CNClaimKey)), CInt(Session(CNClaimPerilKey)), oClaimQuote.BranchCode)

                    Session(CNDataSet) = v_sXMLDataSet

                Catch ex As Exception
                    Throw
                End Try

                Dim srDataset As New System.IO.StringReader(v_sXMLDataSet)
                Dim Doc As New XmlDocument
                Dim xmlTRNew As New XmlTextReader(srDataset)
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

        Public Sub FinishButton(ByVal sender As Object, ByVal e As System.EventArgs)
            If Page.IsValid Then
                Dim hdnDuplicateClaimPayment As HiddenField
                For Each oControl In oMaster.Controls
                    'check whether controls "payclaim" exist on this page
                    If oControl.GetType.Name.Contains("controls_payclaim_ascx") Then

                        If CType(oControl.FindControl("hdnDuplicateClaimPayment"), HiddenField) IsNot Nothing Then
                            hdnDuplicateClaimPayment = CType(oControl.FindControl("hdnDuplicateClaimPayment"), HiddenField)
                            If hdnDuplicateClaimPayment.Value IsNot "" Then
                                If hdnDuplicateClaimPayment.Value = True Then
                                    Dim sURL As String
                                    If HttpContext.Current.Session.IsCookieless Then
                                        sURL = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/DuplicateClaimPaymentWarningMessage.aspx?modal=true&RequestBy=NextButton&Riskcheck=true&KeepThis=true&TB_iframe=true&height=200&width=200"
                                    Else
                                        sURL = AppSettings("WebRoot") & "/Modal/DuplicateClaimPaymentWarningMessage.aspx?modal=true&RequestBy=NextButton&Riskcheck=true&KeepThis=true&TB_iframe=true&height=200&width=200"
                                    End If
                                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show", "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sURL & "' , null);});</script>")
                                    Exit Sub
                                End If
                            End If
                        End If
                    End If
                Next

                ProcessClaimFinishButton()
            End If
        End Sub
        Private Sub ProcessClaimFinishButton()


            Dim sDatasetErrorMessages As String = String.Empty
            Dim oPayErrorFlag As Boolean = False
            Dim HidPayClaimWarningConfirmation As HiddenField
            Dim msg_AllowMultipleClaimPayment_error As String
            Dim AutoReinsuranceWarning As Boolean = False
            Session(CNQuoteInSync) = True

            If CType(Session(CNMode), Mode) = Mode.NewClaim Or CType(Session.Item(CNMode), Mode) = Mode.EditClaim _
            Or CType(Session(CNMode), Mode) = Mode.PayClaim Or CType(Session.Item(CNMode), Mode) = Mode.SalvageClaim _
            Or CType(Session.Item(CNMode), Mode) = Mode.TPRecovery Then
                WritePeril(Me)
            End If

            'Check Updated Session of PayClaimResponse  
            Dim oPayClaimResponse As NexusProvider.PayClaimResponse = Nothing
            If Session(CNPayClaimError) IsNot Nothing Then
                oPayClaimResponse = Session(CNPayClaimError)
                'if Warning collection of PayClaimResponse return by SAM
                If oPayClaimResponse.Warnings IsNot Nothing AndAlso oPayClaimResponse.Warnings.Count > 0 Then
                    oPayErrorFlag = True
                    oResource = New ResXResourceReader(HttpContext.Current.Server.MapPath(AppSettings("WebRoot") & "Controls/App_LocalResources/PayClaim.ascx.resx"))
                    en = oResource.GetEnumerator()
                    While (en.MoveNext)
                        If en.Key.ToString.Trim = "msg_ClaimPaymentWarning" Then
                            msg_AllowMultipleClaimPayment_error = en.Value
                        End If
                    End While

                    For Each oControl In oMaster.Controls
                        'check whether controls "payclaim" exist on this page
                        If oControl.GetType.Name.Contains("controls_payclaim_ascx") Then
                            If CType(oControl.FindControl("HidPayClaimWarningConfirmation"), HiddenField) IsNot Nothing Then
                                HidPayClaimWarningConfirmation = CType(oControl.FindControl("HidPayClaimWarningConfirmation"), HiddenField)
                                For Each warning As NexusProvider.Warnings In oPayClaimResponse.Warnings
                                        If warning.Code = "1000013" Then
                                            AutoReinsuranceWarning = True
                                            Me.ClientScript.RegisterStartupScript(Me.GetType(), "errMessage", "document.getElementById('" & HidPayClaimWarningConfirmation.ClientID & "').value=alert('" & warning.Description & "');", True)
                                            Exit For
                                        End If
                                    Next
                                If Not AutoReinsuranceWarning Then
                                    If Not String.IsNullOrEmpty(msg_AllowMultipleClaimPayment_error) Then
                                    'call the javascript message with OK and Cancle button
                                    Me.ClientScript.RegisterStartupScript(Me.GetType(), "clientScript", "document.getElementById('" & HidPayClaimWarningConfirmation.ClientID & "').value=confirm('" & msg_AllowMultipleClaimPayment_error & "');", True)
                                    End If

                                End If
                            End If
                        End If
                    Next
                End If
            End If


            If (oPayErrorFlag = False And CBool(Session(CNIsClaimLocked)) <> True) Then
                If CType(Session(CNMode), Mode) = Mode.NewClaim Or CType(Session.Item(CNMode), Mode) = Mode.EditClaim _
               Or CType(Session(CNMode), Mode) = Mode.PayClaim Then
                    sDatasetErrorMessages = ValidateDataset() 'need to validate the 
                End If

                If sDatasetErrorMessages = String.Empty Then
                    If Not String.IsNullOrEmpty(sNextPage) And iDepth > 1 Then
                        Response.Redirect(sParentTab, False)
                    Else
                        'Clear the selected peril key 
                        Session.Remove(CNClaimPerilKey)
                        Session(CNDirtyPeril) = Nothing

                        If CType(Session.Item(CNMode), Mode) = Mode.Authorise Or CType(Session.Item(CNMode), Mode) = Mode.Recommend _
                            Or CType(Session.Item(CNMode), Mode) = Mode.DeclinePayment Or CType(Session.Item(CNMode), Mode) = Mode.ViewClaimPayment Then
                            Response.Redirect("~/claims/summary.aspx", False)
                        Else
                            FinishButtonPageRedirect()

                            If Session(CNPerilReturnURL) IsNot Nothing Then
                                'If Claim Builder is ON
                                Response.Redirect(Session(CNPerilReturnURL), False)
                            Else
                                'If Claim Builder is OFF
                                'Response.Redirect("~/claims/perils.aspx")
                                Dim sUrl As String = CheckClaimBuilder()
                                Response.Redirect(sUrl, False)
                            End If
                        End If

                    End If
                Else
                    'create a new custom validator
                    'set it as invalid and set the error message property to the output from ValidateDataset
                    Dim cstInvalidDataset As New CustomValidator
                    cstInvalidDataset.IsValid = False
                    cstInvalidDataset.ErrorMessage = sDatasetErrorMessages
                    cstInvalidDataset.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                    Page.Validators.Add(cstInvalidDataset)
                End If
            Else
                If Not String.IsNullOrEmpty(msg_AllowMultipleClaimPayment_error) Or AutoReinsuranceWarning Then
                    'do the Postback if PayClaim SAM method showing error message
                    Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "OnClickHiddenOperation") & ";"
                    ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "ParentPostBack", PostBackStr, True)
                End If
            End If

        End Sub
        Public Sub NextButton(ByVal sender As Object, ByVal e As System.EventArgs)
            If CType(Session(CNMode), Mode) = Mode.ViewClaim Or CType(Session(CNMode), Mode) = Mode.ViewClaimPayment Or CType(Session(CNMode), Mode) = Mode.Authorise Or CType(Session(CNMode), Mode) = Mode.DeclinePayment Or CType(Session(CNMode), Mode) = Mode.Recommend Then
                For Each vValidator As BaseValidator In Page.Validators
                    vValidator.Enabled = False
                Next
            End If

            If Page.IsValid Then
                Dim hdnDuplicateClaimPayment As HiddenField
                For Each oControl In oMaster.Controls
                    'check whether controls "payclaim" exist on this page
                    If oControl.GetType.Name.Contains("controls_payclaim_ascx") Then
                        If CType(oControl.FindControl("hdnDuplicateClaimPayment"), HiddenField) IsNot Nothing Then
                            hdnDuplicateClaimPayment = CType(oControl.FindControl("hdnDuplicateClaimPayment"), HiddenField)
                            If hdnDuplicateClaimPayment.Value IsNot "" Then

                                If hdnDuplicateClaimPayment.Value = True Then
                                    Dim sURL As String
                                    If HttpContext.Current.Session.IsCookieless Then
                                        sURL = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/DuplicateClaimPaymentWarningMessage.aspx?modal=true&RequestBy=NextButton&Riskcheck=true&KeepThis=true&TB_iframe=true&height=200&width=200"
                                    Else
                                        sURL = AppSettings("WebRoot") & "/Modal/DuplicateClaimPaymentWarningMessage.aspx?modal=true&RequestBy=NextButton&Riskcheck=true&KeepThis=true&TB_iframe=true&height=200&width=200"
                                    End If
                                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show", "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sURL & "' , null);});</script>")
                                    Exit Sub
                                End If
                            End If
                        End If
                    End If
                Next
                ProcessClaimNextButton()
            End If


        End Sub
        Private Sub ProcessClaimNextButton()

            Dim oPayErrorFlag As Boolean = False
            Session(CNQuoteInSync) = True

            If CType(Session(CNMode), Mode) = Mode.NewClaim Or CType(Session.Item(CNMode), Mode) = Mode.EditClaim _
            Or CType(Session.Item(CNMode), Mode) = Mode.PayClaim Or CType(Session.Item(CNMode), Mode) = Mode.SalvageClaim _
            Or CType(Session.Item(CNMode), Mode) = Mode.TPRecovery Then
                WritePeril(Me)
            End If

            Dim oPayClaimResponse As NexusProvider.PayClaimResponse = Nothing
            Dim HidPayClaimWarningConfirmation As HiddenField
            Dim msg_AllowMultipleClaimPayment_error As String
            Dim AutoReinsuranceWarning As Boolean = False

            If Session(CNPayClaimError) IsNot Nothing Then
                oPayClaimResponse = Session(CNPayClaimError)
                'Check Updated Session of PayClaimResponse  
                If oPayClaimResponse.Warnings IsNot Nothing AndAlso oPayClaimResponse.Warnings.Count > 0 Then
                    oPayErrorFlag = True

                    oResource = New ResXResourceReader(HttpContext.Current.Server.MapPath(AppSettings("WebRoot") & "Controls/App_LocalResources/PayClaim.ascx.resx"))
                    en = oResource.GetEnumerator()
                    While (en.MoveNext)
                        If en.Key.ToString.Trim = "msg_ClaimPaymentWarning" Then
                            msg_AllowMultipleClaimPayment_error = en.Value
                        End If
                    End While

                    For Each oControl In oMaster.Controls
                        'check whether controls "payclaim" exist on this page
                        If oControl.GetType.Name.Contains("controls_payclaim_ascx") Then
                            If CType(oControl.FindControl("HidPayClaimWarningConfirmation"), HiddenField) IsNot Nothing Then
                                HidPayClaimWarningConfirmation = CType(oControl.FindControl("HidPayClaimWarningConfirmation"), HiddenField)
                                For Each warning As NexusProvider.Warnings In oPayClaimResponse.Warnings
                                        If warning.Code = "1000013" Then
                                            AutoReinsuranceWarning = True
                                            Me.ClientScript.RegisterStartupScript(Me.GetType(), "errMessage", "document.getElementById('" & HidPayClaimWarningConfirmation.ClientID & "').value=alert('" & warning.Description & "');", True)
                                            Exit For
                                        End If
                                    Next
                                If Not AutoReinsuranceWarning Then
                                    If Not String.IsNullOrEmpty(msg_AllowMultipleClaimPayment_error) Then
                                    'call the javascript message with OK and Cancle button
                                    Me.ClientScript.RegisterStartupScript(Me.GetType(), "clientScript", "document.getElementById('" & HidPayClaimWarningConfirmation.ClientID & "').value=confirm('" & msg_AllowMultipleClaimPayment_error & "');", True)
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If
            End If

            If (oPayErrorFlag = False And CBool(Session(CNIsClaimLocked)) <> True) Then
                If sNextPage <> String.Empty Then
                    Response.Redirect(sNextPage, False)

                ElseIf CType(Session(CNMode), Mode) = Mode.NewClaim Or CType(Session.Item(CNMode), Mode) = Mode.EditClaim _
                Or CType(Session(CNMode), Mode) = Mode.PayClaim Then

                    Dim sDatasetErrorMessages As String = ValidateDataset()
                    If sDatasetErrorMessages = String.Empty Then
                        'Clear the selected peril key 
                        Session.Remove(CNClaimPerilKey)
                        Session(CNDirtyPeril) = Nothing

                        If Session(CNPerilReturnURL) IsNot Nothing Then
                            'If Claim Builder is ON
                            Response.Redirect(Session(CNPerilReturnURL), False)
                        Else
                            'If Claim Builder is OFF
                            Response.Redirect("~/claims/perils.aspx", False)
                        End If
                    Else
                        Dim cstInvalidDataset As New CustomValidator
                        cstInvalidDataset.IsValid = False
                        cstInvalidDataset.ErrorMessage = sDatasetErrorMessages
                        cstInvalidDataset.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                        Page.Validators.Add(cstInvalidDataset)
                    End If
                ElseIf CType(Session.Item(CNMode), Mode) = Mode.Authorise Or CType(Session.Item(CNMode), Mode) = Mode.Recommend _
                            Or CType(Session.Item(CNMode), Mode) = Mode.DeclinePayment Or CType(Session.Item(CNMode), Mode) = Mode.ViewClaimPayment Then
                    'Clear the selected peril key 
                    Session.Remove(CNClaimPerilKey)
                    Session(CNDirtyPeril) = Nothing
                    If (Session(CNPerilReturnURL) IsNot Nothing AndAlso CType(Session.Item(CNMode), Mode) = Mode.ViewClaimPayment) Then
                        'If user has selected "View" link
                        Response.Redirect(Session(CNPerilReturnURL), False)
                    Else
                        'forceful view of peril builder pages
                        Response.Redirect("~/claims/summary.aspx", False)
                    End If
                Else
                    'Clear the selected peril key 
                    Session.Remove(CNClaimPerilKey)
                    Session(CNDirtyPeril) = Nothing

                    If Session(CNPerilReturnURL) IsNot Nothing Then
                        'If Claim Builder is ON
                        Response.Redirect(Session(CNPerilReturnURL), False)
                    Else
                        'If Claim Builder is OFF
                        Response.Redirect("~/claims/perils.aspx", False)
                    End If
                End If
            Else
                If Not String.IsNullOrEmpty(msg_AllowMultipleClaimPayment_error) Or AutoReinsuranceWarning Then
                    Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "OnClickHiddenOperation") & ";"
                    ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "ParentPostBack", PostBackStr, True)
                End If
            End If

        End Sub
        Public Sub TabClick(ByVal v_sPath As String)
            If Page.IsValid Then
                WritePeril(Me)
                Response.Redirect(v_sPath, False)
            End If
        End Sub

        Public Overridable Sub PreDataSetWrite()

        End Sub
        Public Overridable Sub PostDataSetWrite()

        End Sub
        Public Overridable Sub PrePageRedirect()

        End Sub
        Public Overridable Sub FinishButtonPageRedirect()

        End Sub
        Public Overridable Sub BackButtonPageRedirect()

        End Sub
        Public Overridable Sub NextButtonPageRedirect()

        End Sub

        Private Sub WritePeril(Optional ByVal sender As Object = Nothing)

            If CType(Session.Item(CNMode), Mode) <> Mode.ViewClaim Then

                PreDataSetWrite()
                WriteContainerToXML(oMaster, sScreenCode, sOI, Nothing, Nothing, sender)
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

#End Region
#Region "Reserve And Recovery Data"
        Sub SaveReserveAndRecoveryData(ByVal v_oContainer As Control)
            'Read the data from grid and update in the session object of the claim
            Dim grdvReserveItem As GridView = CType(v_oContainer.FindControl("grdvReserveItem"), GridView)
            Dim hdnCalculate As HtmlInputHidden = CType(v_oContainer.FindControl("hdnCalculate"), HtmlInputHidden)
            'Save the data back to the session object
            Dim iPeril As Integer
            If Not Integer.TryParse(Convert.ToString(Request.QueryString("PerilIndex")), iPeril) Then
                If Not Integer.TryParse(Convert.ToString(Session(CNClaimPerilIndex)), iPeril) Then
                    Throw New InvalidOperationException("Peril index is missing or invalid.")
                End If
            End If
            Dim oClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            Dim oModeClaim As Mode = CType(Session.Item(CNMode), Mode)
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
            'Flag to check which peril has been updated it need to be updated in DB
            oClaim.ClaimPeril(iPeril).PerilEdited = True

            Dim claimsReserveForGross As NexusProvider.OptionTypeSetting
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            claimsReserveForGross = oWebservice.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ClaimsReserveForGross)

            For Each oRow As GridViewRow In grdvReserveItem.Rows

                If hdnCalculate.Value = "1" Then
                    If Session(CNMode) = Mode.NewClaim Then
                        'JS is enabled so do the calucation again for save
                        Dim sAmount As String = Request.Form(grdvReserveItem.Rows(oRow.DataItemIndex).FindControl("txtAmount").UniqueID)
                        Dim lblNewReserveNet As String = CType(grdvReserveItem.Rows(oRow.DataItemIndex).FindControl("lblNewReserveNet"), Label).Text
                        Dim grossReserve As String = Request.Form(grdvReserveItem.Rows(oRow.DataItemIndex).FindControl("txtGrossReserve").UniqueID)
                        Dim tax As String = CType(grdvReserveItem.Rows(oRow.DataItemIndex).FindControl("lblTax"), Label).Text
                        Dim taxCurrentReserve As String = CType(grdvReserveItem.Rows(oRow.DataItemIndex).FindControl("lblTaxCurrentReserve"), Label).Text
                        Dim grossCurrentReserve As String = CType(grdvReserveItem.Rows(oRow.DataItemIndex).FindControl("lblGrossCurrentReserve"), Label).Text

                        If oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).BaseReserveKey <> 0 Then
                            If String.IsNullOrEmpty(sAmount) = False Then
                                If claimsReserveForGross.OptionValue <> "1" Then
                                    oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).CurrentReserve = sAmount
                                    oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).InitialReserve = sAmount
                                Else
                                    oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).CurrentReserve = Convert.ToDecimal(lblNewReserveNet)
                                    oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).InitialReserve = Convert.ToDecimal(lblNewReserveNet)
                                End If

                                oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).GrossReserve = grossReserve
                                oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).Tax = tax
                                oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).RevisedGrossReserve = grossCurrentReserve
                                oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).RevisedTaxReserve = taxCurrentReserve
                                'Flag to check which reserve has been updated it need to be updated in DB
                                oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).ReserveEdited = True
                            Else
                                'Flag to check which reserve has been updated it need to be updated in DB
                                oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).ReserveEdited = False
                            End If
                        Else

                            If String.IsNullOrEmpty(sAmount) = False Then
                                'Check Whether Records is present or not if present then it will be updated 
                                'or else it will be added
                                Dim bPresent As Boolean = False
                                If oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).IsSalvage = False Then
                                    If oClaim.ClaimPeril(iPeril).TPRecovery IsNot Nothing AndAlso oClaim.ClaimPeril(iPeril).TPRecovery.Count > 0 Then
                                        For iCount As Integer = 0 To oClaim.ClaimPeril(iPeril).TPRecovery.Count - 1
                                            If oClaim.ClaimPeril(iPeril).TPRecovery(iCount).TypeCode.Trim.ToUpper = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).TypeCode.Trim.ToUpper Then
                                                bPresent = True
                                                With oClaim.ClaimPeril(iPeril).TPRecovery(iCount)
                                                    .TypeCode = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).TypeCode
                                                    oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).InitialReserve = sAmount
                                                    .CurrentRecovery = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).InitialReserve
                                                    .CurrentRecovery = Math.Round(.CurrentRecovery, 2)
                                                    .InitialRecovery = .CurrentRecovery
                                                End With
                                            End If
                                        Next
                                    End If
                                ElseIf oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).IsSalvage = True Then
                                    If oClaim.ClaimPeril(iPeril).SalvageRecovery IsNot Nothing AndAlso oClaim.ClaimPeril(iPeril).SalvageRecovery.Count > 0 Then
                                        For iCount As Integer = 0 To oClaim.ClaimPeril(iPeril).SalvageRecovery.Count - 1
                                            If oClaim.ClaimPeril(iPeril).SalvageRecovery(iCount).TypeCode.Trim.ToUpper = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).TypeCode.Trim.ToUpper Then
                                                bPresent = True
                                                With oClaim.ClaimPeril(iPeril).SalvageRecovery(iCount)
                                                    .TypeCode = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).TypeCode
                                                    oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).InitialReserve = sAmount
                                                    .CurrentRecovery = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).InitialReserve
                                                    .CurrentRecovery = Math.Round(.CurrentRecovery, 2)
                                                    .InitialRecovery = .CurrentRecovery
                                                End With
                                            End If
                                        Next
                                    End If
                                End If

                                If bPresent = False Then
                                    'Recovery will be added here
                                    Dim oRecovery As New NexusProvider.PerilRecovery
                                    With oRecovery
                                        .TypeCode = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).TypeCode
                                        oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).InitialReserve = sAmount
                                        .CurrentRecovery = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).InitialReserve
                                        .CurrentRecovery = Math.Round(.CurrentRecovery, 2)
                                        .InitialRecovery = .CurrentRecovery
                                        .IsNew = False
                                        .CanDelete = True
                                    End With
                                    If oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).IsSalvage = False Then
                                        oClaim.ClaimPeril(iPeril).TPRecovery.Add(oRecovery)
                                    ElseIf oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).IsSalvage = True Then
                                        oClaim.ClaimPeril(iPeril).SalvageRecovery.Add(oRecovery)
                                    End If
                                End If
                            End If
                        End If
                    ElseIf Session(CNMode) = Mode.EditClaim Or (Session(CNMode) = Mode.PayClaim AndAlso (Session(CNLockPaymentGrid) IsNot Nothing AndAlso Session(CNLockPaymentGrid) = True)) Then
                        'JS is enabled so do the calucation again for save
                        Dim sAmount As String = Request.Form(grdvReserveItem.Rows(oRow.DataItemIndex).FindControl("txtAmount").UniqueID)
                        Dim lblNewReserveNet As String = CType(grdvReserveItem.Rows(oRow.DataItemIndex).FindControl("lblNewReserveNet"), Label).Text
                        Dim grossReserve As String = Request.Form(grdvReserveItem.Rows(oRow.DataItemIndex).FindControl("txtGrossReserve").UniqueID)
                        Dim tax As String = CType(grdvReserveItem.Rows(oRow.DataItemIndex).FindControl("lblTax"), Label).Text
                        Dim taxCurrentReserve As String = CType(grdvReserveItem.Rows(oRow.DataItemIndex).FindControl("lblTaxCurrentReserve"), Label).Text
                        Dim grossCurrentReserve As String = CType(grdvReserveItem.Rows(oRow.DataItemIndex).FindControl("lblGrossCurrentReserve"), Label).Text

                        If oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).BaseReserveKey <> 0 Then
                            If String.IsNullOrEmpty(sAmount) = False Then
                                Dim dAmountToBePaid As Decimal
                                If oClaim.ClaimPeril(iPeril).ClaimReserve IsNot Nothing AndAlso oClaim.ClaimPeril(iPeril).ClaimReserve.Count > 0 Then
                                    Decimal.TryParse(oClaim.ClaimPeril(iPeril).ClaimReserve(oRow.DataItemIndex).ThisPaymentINCLTax, dAmountToBePaid)
                                End If

                                oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).CurrentReserve += CDbl(sAmount) + dAmountToBePaid
                                oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).RevisedReserve += CDbl(sAmount) + dAmountToBePaid

                                oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).GrossReserve = grossReserve
                                oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).Tax = tax

                                oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).RevisedGrossReserve = Convert.ToDecimal(grossReserve)
                                oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).RevisedTaxReserve = Convert.ToDecimal(tax)

                                'Flag to check which reserve has been updated it need to be updated in DB
                                oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).ReserveEdited = True
                            Else
                                'Flag to check which reserve has been updated it need to be updated in DB
                                oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).ReserveEdited = False
                            End If
                        Else
                            If String.IsNullOrEmpty(sAmount) = True And oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).InitialReserve.ToString() = "0" And oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).RevisedReserve.ToString() = "0" Then
                                sAmount = 0
                            End If
                            Dim bPresent As Boolean = False
                            If String.IsNullOrEmpty(sAmount) = False AndAlso sAmount > 0 Then
                                'Recovery will be updated here
                                If oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).IsSalvage = True Then
                                    If oClaim.ClaimPeril(iPeril).SalvageRecovery IsNot Nothing AndAlso oClaim.ClaimPeril(iPeril).SalvageRecovery.Count > 0 Then
                                        For Each oSalvage As NexusProvider.PerilRecovery In oClaim.ClaimPeril(iPeril).SalvageRecovery
                                            If oSalvage.TypeCode.Trim.ToUpper = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).TypeCode.Trim.ToUpper Then
                                                bPresent = True
                                                With oSalvage
                                                    .TypeCode = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).TypeCode
                                                    .InitialRecovery = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).InitialReserve
                                                    .CurrentRecovery = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).RevisedReserve + Math.Round(CDec(sAmount), 2)
                                                    .CurrentRecovery = Math.Round(.CurrentRecovery, 2)
                                                    oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).RevisedReserve += Math.Round(CDec(sAmount), 2)
                                                    .RevisedRecovery = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).RevisedReserve
                                                    .IsNew = False
                                                End With
                                                Exit For
                                            End If
                                        Next
                                    Else
                                        'if added during edit/pay claim claim
                                        bPresent = True
                                        Dim oRecovery As New NexusProvider.PerilRecovery
                                        With oRecovery
                                            '.TypeCode = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).TypeCode
                                            'oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).RevisedReserve = sAmount
                                            '.RevisedRecovery = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).RevisedReserve
                                            '.CurrentRecovery = .RevisedRecovery
                                            '.CurrentRecovery = Math.Round(.CurrentRecovery, 2)
                                            '.RevisedRecovery = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).RevisedReserve + .CurrentRecovery
                                            '.IsNew = False
                                            '.CanDelete = True

                                            .TypeCode = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).TypeCode
                                            oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).InitialReserve = sAmount
                                            .CurrentRecovery = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).InitialReserve
                                            .RevisedRecovery = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).RevisedReserve + .CurrentRecovery
                                            .CurrentRecovery = Math.Round(.CurrentRecovery, 2)
                                            .InitialRecovery = .CurrentRecovery
                                            .IsNew = True
                                            .CanDelete = True
                                        End With

                                        oClaim.ClaimPeril(iPeril).SalvageRecovery.Add(oRecovery)
                                    End If

                                ElseIf oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).IsSalvage = False Then
                                    If oClaim.ClaimPeril(iPeril).TPRecovery IsNot Nothing AndAlso oClaim.ClaimPeril(iPeril).TPRecovery.Count > 0 Then
                                        For Each oTPRecovery As NexusProvider.PerilRecovery In oClaim.ClaimPeril(iPeril).TPRecovery
                                            If oTPRecovery.TypeCode.Trim.ToUpper = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).TypeCode.Trim.ToUpper Then
                                                bPresent = True
                                                With oTPRecovery
                                                    .TypeCode = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).TypeCode
                                                    .InitialRecovery = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).InitialReserve
                                                    .CurrentRecovery = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).RevisedReserve + Math.Round(CDec(sAmount), 2)
                                                    .CurrentRecovery = Math.Round(.CurrentRecovery, 2)
                                                    oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).RevisedReserve += Math.Round(CDec(sAmount), 2)
                                                    .RevisedRecovery = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).RevisedReserve
                                                    .IsNew = False
                                                End With
                                                Exit For
                                            End If
                                        Next
                                    Else
                                        'if added during edit/pay claim claim
                                        bPresent = True
                                        Dim oRecovery As New NexusProvider.PerilRecovery
                                        With oRecovery
                                            '.TypeCode = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).TypeCode
                                            'oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).RevisedReserve = sAmount
                                            '.RevisedRecovery = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).RevisedReserve
                                            '.CurrentRecovery = .RevisedRecovery
                                            '.CurrentRecovery = Math.Round(.CurrentRecovery, 2)
                                            '.RevisedRecovery = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).RevisedReserve + .CurrentRecovery
                                            '.IsNew = False
                                            '.CanDelete = True

                                            .TypeCode = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).TypeCode
                                            oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).InitialReserve = sAmount
                                            .CurrentRecovery = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).InitialReserve
                                            .RevisedRecovery = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).RevisedReserve + .CurrentRecovery
                                            .CurrentRecovery = Math.Round(.CurrentRecovery, 2)
                                            .InitialRecovery = .CurrentRecovery
                                            .IsNew = True
                                            .CanDelete = True
                                        End With

                                        oClaim.ClaimPeril(iPeril).TPRecovery.Add(oRecovery)

                                    End If

                                End If
                                If bPresent = False Then
                                    'Recovery will be added here
                                    Dim oRecovery As New NexusProvider.PerilRecovery
                                    With oRecovery
                                        '.TypeCode = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).TypeCode
                                        'oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).RevisedReserve = sAmount
                                        '.RevisedRecovery = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).RevisedReserve
                                        '.RevisedRecovery = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).RevisedReserve + .CurrentRecovery
                                        '.CurrentRecovery = .RevisedRecovery
                                        '.CurrentRecovery = Math.Round(.CurrentRecovery, 2)
                                        '.IsNew = False
                                        '.CanDelete = True

                                        .TypeCode = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).TypeCode
                                        oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).InitialReserve = sAmount
                                        .CurrentRecovery = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).InitialReserve
                                        .CurrentRecovery = Math.Round(.CurrentRecovery, 2)
                                        .InitialRecovery = .CurrentRecovery
                                        .IsNew = True
                                        .CanDelete = True


                                    End With
                                    If oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).IsSalvage = False Then
                                        oClaim.ClaimPeril(iPeril).TPRecovery.Add(oRecovery)
                                    ElseIf oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).IsSalvage = True Then
                                        oClaim.ClaimPeril(iPeril).SalvageRecovery.Add(oRecovery)
                                    End If
                                End If
                                If bPresent = False Then
                                    'Recovery will be added here
                                    Dim oRecovery As New NexusProvider.PerilRecovery
                                    With oRecovery
                                        '.TypeCode = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).TypeCode
                                        'oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).RevisedReserve = sAmount
                                        '.RevisedRecovery = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).RevisedReserve
                                        '.RevisedRecovery = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).RevisedReserve + .CurrentRecovery
                                        '.CurrentRecovery = .RevisedRecovery
                                        '.CurrentRecovery = Math.Round(.CurrentRecovery, 2)
                                        '.IsNew = False
                                        '.CanDelete = True

                                        .TypeCode = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).TypeCode
                                        oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).InitialReserve = sAmount
                                        .CurrentRecovery = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).InitialReserve
                                        .CurrentRecovery = Math.Round(.CurrentRecovery, 2)
                                        .InitialRecovery = .CurrentRecovery
                                        .IsNew = True
                                        .CanDelete = True


                                    End With
                                    If oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).IsSalvage = False Then
                                        oClaim.ClaimPeril(iPeril).TPRecovery.Add(oRecovery)
                                    ElseIf oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).IsSalvage = True Then
                                        oClaim.ClaimPeril(iPeril).SalvageRecovery.Add(oRecovery)
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            Next
            Session.Item(CNClaim) = oClaim

            'if peril screen is not configured then need to validate the reserve with coreperilscreen code
            Dim oClaimResponse As NexusProvider.ClaimResponse = Nothing
            Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
            Dim sBranchCode As String = oQuote.BranchCode

            If Session(CNMode) = Mode.NewClaim Then
                'arch issue 268
                'oClaimResponse = oWebservice.UpdateClaimReservesOrPayments(oClaim, Nothing, Session.Item(CNClaimTimeStamp), 1, sBranchCode)
                oClaimResponse = UpdateClaimReservesOrPaymentsCall(oClaim, Nothing, Session.Item(CNClaimTimeStamp), 1, sBranchCode)
            ElseIf Session(CNMode) = Mode.EditClaim _
            Or Session(CNMode) = Mode.PayClaim AndAlso (Session(CNLockPaymentGrid) IsNot Nothing AndAlso Session(CNLockPaymentGrid) = True) Then
                'arch issue 268
                'oClaimResponse = oWebservice.UpdateClaimReservesOrPayments(oClaim, Nothing, Session.Item(CNClaimTimeStamp), 2, sBranchCode)
                oClaimResponse = UpdateClaimReservesOrPaymentsCall(oClaim, Nothing, Session.Item(CNClaimTimeStamp), 2, sBranchCode)
            End If
        End Sub

#End Region
#Region "Payment Data"
        Sub SavePaymentData(ByVal v_oContainer As Control)
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(GetPortalID())
            Dim PerilsIndex As New System.Collections.Generic.List(Of Integer)

            If Session(CNClaimMultiPerilIndex) IsNot Nothing Then
                PerilsIndex = Session(CNClaimMultiPerilIndex)
            Else
                PerilsIndex.Add(CInt(Session(CNClaimPerilIndex)))
            End If

            Dim txtParty As TextBox = CType(v_oContainer.FindControl("txtParty"), TextBox)
            Dim txtMediaRef As TextBox = CType(v_oContainer.FindControl("txtMediaRef"), TextBox)
            Dim ddlAccountType As DropDownList = CType(v_oContainer.FindControl("ddlAccountType"), DropDownList)
            Dim GISLookup_MediaType As NexusProvider.LookupList = CType(v_oContainer.FindControl("GISLookup_MediaType"), NexusProvider.LookupList)
            Dim txtPayeeName As TextBox = CType(v_oContainer.FindControl("txtPayeeName"), TextBox)
            Dim txtChequeDate As TextBox = CType(v_oContainer.FindControl("txtChequeDate"), TextBox)
            Dim txtThisReference As TextBox = CType(v_oContainer.FindControl("txtThisReference"), TextBox)
            Dim txtComments As TextBox = CType(v_oContainer.FindControl("txtComments"), TextBox)
            Dim txtOurRef As TextBox = CType(v_oContainer.FindControl("txtOurRef"), TextBox)
            Dim txtBIC As TextBox = CType(v_oContainer.FindControl("txtBIC"), TextBox)
            Dim txtIBAN As TextBox = CType(v_oContainer.FindControl("txtIBAN"), TextBox)
            Dim txtAccountType As TextBox = CType(v_oContainer.FindControl("txtAccountType"), TextBox)

            Dim txtBankCode As TextBox = CType(v_oContainer.FindControl("txtBankCode"), TextBox)
            Dim txtBankName As TextBox = CType(v_oContainer.FindControl("txtBankName"), TextBox)
            Dim txtBankAccNumber As TextBox = CType(v_oContainer.FindControl("txtBankAccNumber"), TextBox)
            Dim Address As UserControl = CType(v_oContainer.FindControl("Address"), UserControl)
            Dim rblPayee As RadioButtonList = CType(v_oContainer.FindControl("rblPayee"), RadioButtonList)
            Dim hPartyKey As HiddenField = CType(v_oContainer.FindControl("hPartyKey"), HiddenField)

            'Retreval of the quote information from session
            Dim oQuote As NexusProvider.Quote = CType(Session(CNClaimQuote), NexusProvider.Quote)
            Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
            Dim oAddress As NexusProvider.Address = Nothing

            Dim txtUltimatePayee As TextBox = CType(v_oContainer.FindControl("txtUltimatePayee"), TextBox)
            For Each PerilItemIndex As Integer In PerilsIndex
                If Session(CNMode) = Mode.PayClaim AndAlso Session(CNEnablePayClaim) Is Nothing Then

                    oClaimOpen.ClaimPeril(PerilItemIndex).Payment.BaseClaimKey = oClaimOpen.BaseClaimKey
                    oClaimOpen.ClaimPeril(PerilItemIndex).Payment.BaseClaimPerilKey = oClaimOpen.ClaimPeril(PerilItemIndex).BaseClaimPerilKey
                    oClaimOpen.ClaimPeril(PerilItemIndex).Payment.PartyPaidName = Trim(txtParty.Text)
                    oClaimOpen.ClaimPeril(PerilItemIndex).Payment.UltimatePayee = Trim(txtUltimatePayee.Text)
                    If txtOurRef IsNot Nothing Then
                        oClaimOpen.ClaimPeril(PerilItemIndex).Payment.OurRef = Trim(txtOurRef.Text)
                    End If

                    If txtChequeDate IsNot Nothing Then
                        If IsDate(txtChequeDate.Text) Then
                            oClaimOpen.ClaimPeril(PerilItemIndex).Payment.PaymentDate = Trim(txtChequeDate.Text)
                        End If
                    End If

                    'Updation of the Payment object with values
                    With oClaimOpen.ClaimPeril(PerilItemIndex).Payment.Payee
                        Dim iPartyBankKey As Integer
                        Integer.TryParse(ddlAccountType.SelectedValue, iPartyBankKey)
                        '.PartyBankKey = iPartyBankKey
                        .MediaReference = Trim(txtMediaRef.Text)
                        .MediaTypeCode = GISLookup_MediaType.Value
                        .Name = Trim(txtPayeeName.Text)
                        .TheirReference = Trim(txtThisReference.Text)
                        .Comments = Trim(txtComments.Text)
                        .BankCode = Trim(txtBankCode.Text)
                        .BankName = Trim(txtBankName.Text)
                        .BankNumber = Trim(txtBankAccNumber.Text)
                        .BIC = Trim(txtBIC.Text)
                        .IBAN = Trim(txtIBAN.Text)
                        If txtAccountType Is Nothing Then
                            .AccountType = ""
                        Else
                            .AccountType = Trim(txtAccountType.Text)
                        End If
                        'Reading the address control from page
                        oAddress = GetAddressFromControl(v_oContainer)
                        .Address.Address1 = oAddress.Address1
                        .Address.Address2 = oAddress.Address2
                        .Address.Address3 = oAddress.Address3
                        .Address.Address4 = oAddress.Address4
                        .Address.AddressType = NexusProvider.AddressType.CorrespondenceAddress
                        .Address.CountryCode = oAddress.CountryCode
                        .Address.CountryDescription = oAddress.CountryDescription
                        .Address.PostCode = oAddress.PostCode
                    End With

                ElseIf Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                    Dim bTimeStamp As Byte() = CType(Session(CNClaimTimeStamp), Byte())
                    Dim oClaimReceipt As New NexusProvider.ClaimReceipt
                    'Updation of the Receipt object with values
                    With oClaimOpen.ClaimPeril(PerilItemIndex).Receipt
                        If oClaimOpen.ClaimPeril(PerilItemIndex).Receipt.Payee.Address IsNot Nothing Then
                            If oClaimOpen.Client IsNot Nothing Then
                                .Payee.Address.Address1 = oClaimOpen.Client.Address.Address1
                                .Payee.Address.Address2 = oClaimOpen.Client.Address.Address2
                                .Payee.Address.Address3 = oClaimOpen.Client.Address.Address3
                                .Payee.Address.Address4 = oClaimOpen.Client.Address.Address4
                                .Payee.Address.AddressType = NexusProvider.AddressType.CorrespondenceAddress
                                .Payee.Address.CountryCode = oClaimOpen.Client.Address.CountryCode
                                .Payee.Address.CountryDescription = oClaimOpen.Client.Address.CountryDescription
                                .Payee.Address.Key = oClaimOpen.Client.Address.Key
                                .Payee.Address.CountryKey = oClaimOpen.Client.Address.CountryKey
                                .Payee.Address.PostCode = oClaimOpen.Client.Address.PostCode
                                .Payee.Address.StateCode = oClaimOpen.Client.Address.StateCode
                            End If
                            '.PartyKey = oQuote.PartyKey

                            If .Payee.Address.CountryCode IsNot Nothing Then
                                If .Payee.Address.CountryCode.Trim.Length = 0 Then
                                    .Payee.Address.CountryCode = oPortal.Countries.DefaultCountryCode
                                    .Payee.Address.CountryDescription = GetDescriptionForCode(NexusProvider.ListType.PMLookup, .Payee.Address.CountryCode, "Country")
                                End If
                            Else
                                .Payee.Address.CountryCode = oPortal.Countries.DefaultCountryCode
                                .Payee.Address.CountryDescription = GetDescriptionForCode(NexusProvider.ListType.PMLookup, .Payee.Address.CountryCode, "Country")
                            End If
                        End If
                        .Payee.BankCode = Trim(txtBankCode.Text)
                        .Payee.BankName = Trim(txtBankName.Text)
                        .Payee.BankNumber = Trim(txtBankAccNumber.Text)
                        .Payee.Comments = Trim(txtComments.Text)
                        .Payee.MediaReference = Trim(txtMediaRef.Text)
                        .Payee.MediaTypeCode = GISLookup_MediaType.Value
                        .Payee.Name = Trim(txtPayeeName.Text)
                        .Payee.TheirReference = Trim(txtOurRef.Text)
                        .Payee.BIC = Trim(txtBIC.Text)
                        .Payee.IBAN = Trim(txtIBAN.Text)
                        If Session(CNMode) = Mode.SalvageClaim Then
                            .ClaimVersionDescription = "Salvage Recovery "
                        ElseIf Session(CNMode) = Mode.TPRecovery Then
                            .ClaimVersionDescription = "Third Party Recovery "
                        End If

                    End With
                End If
                'Storing the radion button values in Receipt/Payment Object i.e Payee
                Select Case rblPayee.SelectedValue
                    Case "0"
                        If Session(CNMode) = Mode.SalvageClaim Then
                            For iCount As Integer = 0 To oClaimOpen.ClaimPeril(PerilItemIndex).SalvageRecovery.Count - 1
                                oClaimOpen.ClaimPeril(PerilItemIndex).SalvageRecovery(iCount).ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.CLMRECEIVABLE
                                oClaimOpen.ClaimPeril(PerilItemIndex).SalvageRecovery(iCount).PartyReceiptCode = "CLMRECEIVABLE"
                                oClaimOpen.ClaimPeril(PerilItemIndex).SalvageRecovery(iCount).IsSalvage = 1
                            Next
                            With oClaimOpen.ClaimPeril(PerilItemIndex).Receipt
                                .ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.CLMRECEIVABLE
                                .PartyReceiptCode = "CLMRECEIVABLE"
                            End With
                        ElseIf Session(CNMode) = Mode.TPRecovery Then
                            For iCount As Integer = 0 To oClaimOpen.ClaimPeril(PerilItemIndex).TPRecovery.Count - 1
                                oClaimOpen.ClaimPeril(PerilItemIndex).TPRecovery(iCount).ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.CLMRECEIVABLE
                                oClaimOpen.ClaimPeril(PerilItemIndex).TPRecovery(iCount).PartyReceiptCode = "CLMRECEIVABLE"
                                oClaimOpen.ClaimPeril(PerilItemIndex).TPRecovery(iCount).IsSalvage = 0
                            Next
                            With oClaimOpen.ClaimPeril(PerilItemIndex).Receipt
                                .ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.CLMRECEIVABLE
                                .PartyReceiptCode = "CLMRECEIVABLE"
                            End With
                        Else
                            oClaimOpen.ClaimPeril(PerilItemIndex).Payment.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.CLMPAYABLE
                            oClaimOpen.ClaimPeril(PerilItemIndex).Payment.PartyPaidCode = "CLMPAYABLE"
                        End If

                    Case "1"
                        'Party
                        If Session(CNMode) = Mode.SalvageClaim Then
                            For iCount As Integer = 0 To oClaimOpen.ClaimPeril(PerilItemIndex).SalvageRecovery.Count - 1
                                oClaimOpen.ClaimPeril(PerilItemIndex).SalvageRecovery(iCount).ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.PARTY
                                oClaimOpen.ClaimPeril(PerilItemIndex).SalvageRecovery(iCount).PartyReceiptCode = txtParty.Text.Trim
                                oClaimOpen.ClaimPeril(PerilItemIndex).SalvageRecovery(iCount).IsSalvage = 1
                            Next
                            With oClaimOpen.ClaimPeril(PerilItemIndex).Receipt
                                .ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.PARTY
                                .PartyReceiptCode = txtParty.Text.Trim
                                .PartyKey = CInt(hPartyKey.Value.Trim)
                            End With
                        ElseIf Session(CNMode) = Mode.TPRecovery Then
                            For iCount As Integer = 0 To oClaimOpen.ClaimPeril(PerilItemIndex).TPRecovery.Count - 1
                                oClaimOpen.ClaimPeril(PerilItemIndex).TPRecovery(iCount).ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.PARTY
                                oClaimOpen.ClaimPeril(PerilItemIndex).TPRecovery(iCount).PartyReceiptCode = txtParty.Text.Trim
                                oClaimOpen.ClaimPeril(PerilItemIndex).TPRecovery(iCount).IsSalvage = 0
                            Next
                            With oClaimOpen.ClaimPeril(PerilItemIndex).Receipt
                                .ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.PARTY
                                .PartyReceiptCode = txtParty.Text.Trim
                                .PartyKey = CInt(hPartyKey.Value.Trim)
                            End With
                        Else
                            oClaimOpen.ClaimPeril(PerilItemIndex).Payment.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.PARTY
                            oClaimOpen.ClaimPeril(PerilItemIndex).Payment.PartyKey = CInt(hPartyKey.Value.Trim)
                            oClaimOpen.ClaimPeril(PerilItemIndex).Payment.PartyPaidCode = txtParty.Text.Trim
                        End If
                    Case "2"
                        'Agent
                        If Session(CNMode) = Mode.SalvageClaim Then
                            For iCount As Integer = 0 To oClaimOpen.ClaimPeril(PerilItemIndex).SalvageRecovery.Count - 1
                                oClaimOpen.ClaimPeril(PerilItemIndex).SalvageRecovery(iCount).ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.AGENT
                                oClaimOpen.ClaimPeril(PerilItemIndex).SalvageRecovery(iCount).PartyReceiptCode = oQuote.AgentCode
                                oClaimOpen.ClaimPeril(PerilItemIndex).SalvageRecovery(iCount).IsSalvage = 1
                            Next
                            With oClaimOpen.ClaimPeril(PerilItemIndex).Receipt
                                .ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.AGENT
                                .PartyReceiptCode = oQuote.AgentCode
                            End With
                        ElseIf Session(CNMode) = Mode.TPRecovery Then
                            For iCount As Integer = 0 To oClaimOpen.ClaimPeril(PerilItemIndex).TPRecovery.Count - 1
                                oClaimOpen.ClaimPeril(PerilItemIndex).TPRecovery(iCount).ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.AGENT
                                oClaimOpen.ClaimPeril(PerilItemIndex).TPRecovery(iCount).PartyReceiptCode = oQuote.AgentCode
                                oClaimOpen.ClaimPeril(PerilItemIndex).TPRecovery(iCount).IsSalvage = 0
                            Next
                            With oClaimOpen.ClaimPeril(PerilItemIndex).Receipt
                                .ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.AGENT
                                .PartyReceiptCode = oQuote.AgentCode
                            End With
                        Else
                            oClaimOpen.ClaimPeril(PerilItemIndex).Payment.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.AGENT
                            oClaimOpen.ClaimPeril(PerilItemIndex).Payment.PartyPaidCode = oQuote.AgentCode
                        End If
                    Case "3"
                        'Client
                        If Session(CNMode) = Mode.SalvageClaim Then
                            For iCount As Integer = 0 To oClaimOpen.ClaimPeril(PerilItemIndex).SalvageRecovery.Count - 1
                                oClaimOpen.ClaimPeril(PerilItemIndex).SalvageRecovery(iCount).ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.CLIENT
                                oClaimOpen.ClaimPeril(PerilItemIndex).SalvageRecovery(iCount).PartyReceiptCode = oClaimOpen.ClientShortName
                                oClaimOpen.ClaimPeril(PerilItemIndex).SalvageRecovery(iCount).IsSalvage = 1
                            Next
                            With oClaimOpen.ClaimPeril(PerilItemIndex).Receipt
                                .ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.CLIENT
                                .PartyReceiptCode = oClaimOpen.ClientShortName
                            End With
                        ElseIf Session(CNMode) = Mode.TPRecovery Then
                            For iCount As Integer = 0 To oClaimOpen.ClaimPeril(PerilItemIndex).TPRecovery.Count - 1
                                oClaimOpen.ClaimPeril(PerilItemIndex).TPRecovery(iCount).ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.CLIENT
                                oClaimOpen.ClaimPeril(PerilItemIndex).TPRecovery(iCount).PartyReceiptCode = oClaimOpen.ClientShortName
                                oClaimOpen.ClaimPeril(PerilItemIndex).TPRecovery(iCount).IsSalvage = 1
                            Next
                            With oClaimOpen.ClaimPeril(PerilItemIndex).Receipt
                                .ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.CLIENT
                                .PartyReceiptCode = oClaimOpen.ClientShortName
                            End With
                        Else
                            oClaimOpen.ClaimPeril(PerilItemIndex).Payment.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.CLIENT
                            oClaimOpen.ClaimPeril(PerilItemIndex).Payment.PartyPaidCode = oClaimOpen.ClientShortName
                        End If

                    Case "4"
                        'Insurer
                        If Session(CNMode) = Mode.SalvageClaim Then
                            For iCount As Integer = 0 To oClaimOpen.ClaimPeril(PerilItemIndex).SalvageRecovery.Count - 1
                                oClaimOpen.ClaimPeril(PerilItemIndex).SalvageRecovery(iCount).ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.PARTY
                                oClaimOpen.ClaimPeril(PerilItemIndex).SalvageRecovery(iCount).PartyReceiptCode = txtParty.Text.Trim
                                oClaimOpen.ClaimPeril(PerilItemIndex).SalvageRecovery(iCount).IsSalvage = 1
                            Next
                            With oClaimOpen.ClaimPeril(PerilItemIndex).Receipt
                                .ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.PARTY
                                .PartyReceiptCode = txtParty.Text.Trim
                                .PartyKey = CInt(hPartyKey.Value.Trim)
                            End With
                        ElseIf Session(CNMode) = Mode.TPRecovery Then
                            For iCount As Integer = 0 To oClaimOpen.ClaimPeril(PerilItemIndex).TPRecovery.Count - 1
                                oClaimOpen.ClaimPeril(PerilItemIndex).TPRecovery(iCount).ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.PARTY
                                oClaimOpen.ClaimPeril(PerilItemIndex).TPRecovery(iCount).PartyReceiptCode = txtParty.Text.Trim
                                oClaimOpen.ClaimPeril(PerilItemIndex).TPRecovery(iCount).IsSalvage = 0
                            Next
                            With oClaimOpen.ClaimPeril(PerilItemIndex).Receipt
                                .ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.PARTY
                                .PartyReceiptCode = txtParty.Text.Trim
                                .PartyKey = CInt(hPartyKey.Value.Trim)

                            End With
                        End If

                        If (Session(CNMode) = Mode.SalvageClaim OrElse Session(CNMode) = Mode.TPRecovery) Then
                            With oClaimOpen.ClaimPeril(PerilItemIndex).Receipt
                                If .Payee.Address Is Nothing AndAlso Session(CNSearchData) IsNot Nothing Then
                                    Dim oPartyCollection As New NexusProvider.PartyCollection
                                    oPartyCollection = DirectCast(Session(CNSearchData), NexusProvider.PartyCollection)
                                    If oPartyCollection.Count > 0 Then
                                        Dim nInsurerPartyID As Integer = CInt(hPartyKey.Value.Trim)
                                        Dim bIsAddressFound As Boolean = False
                                        For iIndex As Integer = 0 To oPartyCollection.Count - 1
                                            If oPartyCollection(iIndex).Key = nInsurerPartyID Then
                                                For iAddressCount As Integer = 0 To oPartyCollection(iIndex).Addresses.Count - 1
                                                    If (oPartyCollection(iIndex).Addresses(iAddressCount).AddressType = NexusProvider.AddressType.CorrespondenceAddress) Then
                                                        Dim oInsurerAddress = New NexusProvider.Address
                                                        oInsurerAddress.CountryCode = oPartyCollection(iIndex).Addresses(iAddressCount).CountryCode
                                                        oInsurerAddress.Address1 = oPartyCollection(iIndex).Addresses(iAddressCount).Address1
                                                        oInsurerAddress.Address2 = oPartyCollection(iIndex).Addresses(iAddressCount).Address2
                                                        oInsurerAddress.PostCode = oPartyCollection(iIndex).Addresses(iAddressCount).PostCode
                                                        .Payee.Address = oInsurerAddress
                                                        bIsAddressFound = True
                                                        Exit For
                                                    End If
                                                Next
                                            End If
                                            If bIsAddressFound Then
                                                Exit For
                                            End If
                                        Next
                                    End If
                                End If
                            End With
                        End If
                End Select
            Next
            'Update the data into session
            Session(CNClaim) = oClaimOpen
            If Session(CNMode) = Mode.PayClaim AndAlso Session(CNEnablePayClaim) Is Nothing Then

                'Update the payments through UpdateClaimReserveOrPayments
                Dim oPayClaimResponse As NexusProvider.PayClaimResponse = Nothing
                Dim oInitialClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                Dim iPerilIndex As Integer = Request.QueryString("PerilIndex")
                Dim bTimeStamp As Byte() = CType(Session(CNClaimTimeStamp), Byte())

                Try
                    If Session(CNPayClaimError) Is Nothing Then

                        'Update the Reserve
                        UpdatePaymentData()
                    End If

                Catch ex As NexusProvider.NexusException
                    If ex.Errors(0).Code = "1000151" Or ex.Errors(0).Code = "1000013" Then
                        oPayClaimResponse = New NexusProvider.PayClaimResponse
                        Dim oWarning As New NexusProvider.Warnings
                        oPayClaimResponse.Warnings = New NexusProvider.WarningCollection
                        oWarning.Code = ex.Errors(0).Code
                        oWarning.Description = ex.Errors(0).Description
                        oPayClaimResponse.Warnings.Add(oWarning)
                        Session(CNPayClaimError) = oPayClaimResponse
                        Session(CNEnablePayClaim) = True
                    End If
                    If ex.Errors(0).Code = "1000152" Then
                        oPayClaimResponse = New NexusProvider.PayClaimResponse
                        Dim oWarning As New NexusProvider.Warnings
                        oPayClaimResponse.Warnings = New NexusProvider.WarningCollection
                        oWarning.Code = ex.Errors(0).Code
                        oWarning.Description = ex.Errors(0).Description
                        oPayClaimResponse.Warnings.Add(oWarning)
                        Session(CNPayClaimError) = oPayClaimResponse
                        Session(CNEnablePayClaim) = True
                    End If
                End Try

                'Update the Paid Amount here
                'calculate total amount paid
                Dim sOption As String
                sOption = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsGrossClaimPaymentAmount, NexusProvider.RiskTypeOptions.None, Session(CNProductCode), Nothing)
                If String.IsNullOrEmpty(sOption) Then
                    sOption = "0"
                End If
                Dim dPaidAmount As Decimal

                If Session(CNClaimMultiPerilIndex) IsNot Nothing Then
                    PerilsIndex = Session(CNClaimMultiPerilIndex)
                Else
                    PerilsIndex.Add(CInt(Session(CNClaimPerilIndex)))
                End If
                For Each PerilItemIndex As Integer In PerilsIndex
                    For i As Integer = 0 To oClaimOpen.ClaimPeril(PerilItemIndex).ClaimReserve.Count - 1
                        If sOption = "1" Then
                            dPaidAmount = dPaidAmount + oClaimOpen.ClaimPeril(PerilItemIndex).ClaimReserve(i).ThisPaymentINCLTax - oClaimOpen.ClaimPeril(PerilItemIndex).ClaimReserve(i).ThisPaymentTax
                        Else
                            dPaidAmount = dPaidAmount + oClaimOpen.ClaimPeril(PerilItemIndex).ClaimReserve(i).ThisPaymentINCLTax
                        End If
                    Next
                    oClaimOpen.ClaimPeril(PerilItemIndex).TotalPaidAmount = oClaimOpen.ClaimPeril(PerilItemIndex).PaidAmount + dPaidAmount
                Next

            ElseIf Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                Session(CNEnablePayClaim) = True
            End If

            'Update the data into session
            Session(CNClaim) = oClaimOpen
        End Sub
        Function GetAddressFromControl(ByVal v_oContainer As Control) As NexusProvider.Address
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oAddress As NexusProvider.Address = Nothing
            For Each oControl In v_oContainer.Controls
                Select Case oControl.GetType.Name
                    Case "Panel"
                        oAddress = GetAddressFromControl(oControl)
                        If oAddress IsNot Nothing Then
                            Exit For
                        End If
                    Case "HtmlGenericControl"
                        oAddress = GetAddressFromControl(oControl)
                        If oAddress IsNot Nothing Then
                            Exit For
                        End If
                    Case "UpdatePanel"
                        Dim oCtrl As Control
                        For Each oCtrl In oControl.Controls
                            oAddress = GetAddressFromControl(oCtrl)
                            If oAddress IsNot Nothing Then
                                Exit For
                            End If
                        Next
                        If oAddress IsNot Nothing Then
                            Exit For
                        End If
                    Case Else
                        Select Case True
                            Case oControl.GetType.Name.Contains("controls_addresscntrl_ascx")
                                oAddress = oControl.Address()
                                Exit For
                        End Select
                End Select
            Next
            Return oAddress
        End Function
        Sub UpdatePaymentData()
            'Save the data back to the session object
            Dim PerilsIndex As New System.Collections.Generic.List(Of Integer)

            If Session(CNClaimMultiPerilIndex) IsNot Nothing Then
                PerilsIndex = Session(CNClaimMultiPerilIndex)
            Else
                PerilsIndex.Add(CInt(Session(CNClaimPerilIndex)))
            End If
            Dim oClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            Dim oModeClaim As Mode = CType(Session.Item(CNMode), Mode)
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
            'Flag to check which peril has been updated it need to be updated in DB

            Dim chkExGratia As CheckBox
            Dim oMaster As ContentPlaceHolder
            oMaster = GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName)

            For Each oControl In oMaster.Controls
                'check whether controls "payclaim" exist on this page
                If oControl.GetType.Name.Contains("controls_payclaim_ascx") Then
                    chkExGratia = CType(oControl.FindControl("chkExGratia"), CheckBox)
                    Exit For
                End If
            Next

            For Each PerilItemIndex As Integer In PerilsIndex
                oClaim.ClaimPeril(PerilItemIndex).Payment.IsExGratia = chkExGratia.Checked
                oClaim.ClaimPeril(PerilItemIndex).PerilEdited = True
                Session.Item(CNClaim) = oClaim
                'if peril screen is not configured then need to validate the reserve with coreperilscreen code
                Dim oPayment As NexusProvider.ClaimPayment = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(PerilItemIndex).Payment
                Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oClaimResponse As NexusProvider.ClaimResponse = Nothing
                Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                Dim sBranchCode As String = oQuote.BranchCode
                Dim nProcessType As Integer = 4
                oPayment.ClaimKey = oClaim.ClaimKey

                If Session(CNMode) = Mode.PayClaim Then
                    Dim m_sIsPaymentsReadOnly As String = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsPaymentsReadOnly, NexusProvider.RiskTypeOptions.None, Current.Session(CNProductCode), Nothing)
                    If m_sIsPaymentsReadOnly = "1" Then
                        GetClaimDetails(CType(Session.Item(CNClaim), NexusProvider.ClaimOpen).ClaimKey, Nothing)
                        SetScriptPayment()
                        Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                        oClaimOpen.ClaimPeril(PerilItemIndex).Payment = oPayment
                        Session.Item(CNClaim) = oClaimOpen
                        nProcessType = 5
                    End If
                    'Arch issue 268
                    'oClaimResponse = oWebservice.UpdateClaimReservesOrPayments(oClaim, oPayment, Session.Item(CNClaimTimeStamp), 4, sBranchCode)
                    oClaimResponse = UpdateClaimReservesOrPaymentsCall(oClaim, oPayment, Session.Item(CNClaimTimeStamp), nProcessType, sBranchCode)
                    If oClaimResponse Is Nothing Then
                        Exit Sub
                    End If
                End If

                Session(CNPayClaim) = oClaimResponse
                Session(CNEnablePayClaim) = True
            Next
        End Sub

        Sub PayClaimWarningConfirmation()
            If Request("__EVENTARGUMENT") = "OnClickHiddenOperation" Then
                Dim Hid As HiddenField
                oMaster = GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName)
                For Each oControl In oMaster.Controls
                    'check whether controls "payclaim" exist on this page
                    If oControl.GetType.Name.Contains("controls_payclaim_ascx") Then
                        If CType(oControl.FindControl("HidPayClaimWarningConfirmation"), HiddenField) IsNot Nothing Then
                            Hid = CType(oControl.FindControl("HidPayClaimWarningConfirmation"), HiddenField)
                            If (Hid IsNot Nothing AndAlso Hid.Value.Trim.ToUpper = "TRUE") Then
                                Session(CNMode) = Mode.EditClaim
                                Dim oClaimQuote As NexusProvider.Quote = Session(CNClaimQuote)
                                Dim oOriginalClaim As NexusProvider.ClaimOpen = Nothing
                                Dim oClaimDetails As NexusProvider.ClaimOpen = Nothing
                                Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                                Dim bClaimTimeStamp() As Byte = Nothing
                                GetLatestDetails() 'Update the session with latest values
                                oOriginalClaim = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen) 'Update oOriginalClaim with latest values
                                bClaimTimeStamp = Session.Item(CNClaimTimeStamp)

                                ''Before Fire Maintain Claim - Set Zero amounts in respect of 'This Revision' and 'This Payment' fields/columns
                                oOriginalClaim.ReserveOnly = False
                                'Arch issue 268 
                                'oWebservice.MaintainClaim(oOriginalClaim, bClaimTimeStamp, oClaimQuote.BranchCode)
                                MaintainClaimCall(oOriginalClaim, bClaimTimeStamp, oClaimQuote.BranchCode)

                                Response.Redirect("~/claims/complete.aspx", False)
                            End If
                        End If
                    End If
                Next
            End If
        End Sub
#End Region
#Region "Default Logic"
        ''' <summary>
        ''' WPR 17 - Fast Track Default Logic of Selected Peril Screen for Payment.
        ''' The Default claim values that have been entered into claim builder screens into the appropriate fields on the payment grid
        ''' </summary>
        ''' <param name="iPaymentIndex"></param>
        ''' <param name="dPaymentAmount"></param>
        ''' <param name="rblPaymentPartyType"></param>
        ''' <param name="iPartykey"></param>
        ''' <param name="bPaymentGridEditable"></param>
        '''  <param name="sBankPaymenttype"></param>
        ''' <param name="sTaxGroupCode"></param>
        ''' <remarks></remarks>
        Sub AddDefaultPayment(ByVal iPaymentIndex As Integer, ByVal dPaymentAmount As Double, ByVal rblPaymentPartyType As String, ByVal iPartykey As Integer, ByVal sPartyname As String, ByVal sBankPaymenttype As String, ByVal bPaymentGridEditable As Boolean, Optional ByVal sTaxGroupCode As String = Nothing)
            If Session(CNMode) = Mode.PayClaim AndAlso Not IsPostBack Then
                Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oFastTrackClaimPayment As NexusProvider.ProductClaimsWorkflowOptionsValue
                Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                oFastTrackClaimPayment = oWebservice.GetProductClaimsWorkflowOptions(NexusProvider.ClaimProcessType.OpenClaim, oQuote.ProductCode)
                'to do rakesh 
                oFastTrackClaimPayment.FastTrackClaims = True
                'Checking of the Fast Track Claim Payments
                If (oFastTrackClaimPayment.FastTrackClaims = True) Then
                    Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                    Dim rblPayee As RadioButtonList
                    Dim hPartyKey As HiddenField
                    Dim txtParty As TextBox
                    Dim gvPaymentDetails As GridView
                    Dim oMaster As ContentPlaceHolder
                    oMaster = GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName)

                    For Each oControl In oMaster.Controls
                        'check whether controls "payclaim" exist on this page
                        If oControl.GetType.Name.Contains("controls_payclaim_ascx") Then
                            rblPayee = CType(oControl.FindControl("rblPayee"), RadioButtonList)
                            hPartyKey = CType(oControl.FindControl("hPartyKey"), HiddenField)
                            txtParty = CType(oControl.FindControl("txtParty"), TextBox)
                            gvPaymentDetails = CType(oControl.FindControl("gvPaymentDetails"), GridView)
                            Exit For
                        End If
                    Next

                    Dim oTaxForClaim As New NexusProvider.TaxForClaims
                    Dim oParty As NexusProvider.BaseParty = Nothing
                    Dim HiddenLossTaxAmountDisabled As Double = 0
                    If Session(CNMode) = Mode.PayClaim Then
                        Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                        Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
                        If Not IsPostBack Then
                            If oClaimOpen.ClaimPeril(iPeril).ClaimReserve(iPaymentIndex).ThisPaymentINCLTax = 0 Then
                                'if the passed parameter sTaxGroupCode is NOT EMPTY OR NULL
                                If Not String.IsNullOrEmpty(sTaxGroupCode) Then
                                    'Populating Tax information
                                    With oTaxForClaim
                                        .Amount = CType(Trim(dPaymentAmount), Double)
                                        .CurrencyCode = Session(CNCurrenyCode)
                                        .LossCurrencyCode = oQuote.CurrencyCode
                                        .TaxGroupCode = sTaxGroupCode
                                    End With
                                    oWebservice.CalculateTaxForClaims(oTaxForClaim)
                                End If
                                With oClaimOpen.ClaimPeril(iPeril)
                                    .ClaimReserve(iPaymentIndex).TotalReserve = .ClaimReserve(iPaymentIndex).TotalReserve
                                    If oTaxForClaim IsNot Nothing Then
                                        HiddenLossTaxAmountDisabled = oTaxForClaim.TaxCurrencyAmount
                                        .ClaimReserve(iPaymentIndex).ThisPaymentINCLTax = CDec(dPaymentAmount)
                                        .ClaimReserve(iPaymentIndex).ThisPaymentTax = CDec(HiddenLossTaxAmountDisabled)
                                    End If
                                    .ClaimReserve(iPaymentIndex).CostToClaim = CDec(dPaymentAmount) - CDec(HiddenLossTaxAmountDisabled)
                                    .ClaimReserve(iPaymentIndex).CurrencyCode = Session(CNCurrenyCode)
                                    .ClaimReserve(iPaymentIndex).CurrencyRate = 1 ' Default logic Always have currencyrate=1' txtCurrencyRate.Text

                                    If .ClaimReserve(iPaymentIndex).IsExcess Then
                                        'we are not handling the excess payment as default logic
                                    Else
                                        oClaimOpen.ClaimPeril(iPeril).Payment.ClaimPaymentItem(iPaymentIndex).ReverseExcess = False
                                        .ClaimReserve(iPaymentIndex).CurrentReserve = .ClaimReserve(iPaymentIndex).OldReserve - (CDec(dPaymentAmount) - CDec(HiddenLossTaxAmountDisabled))
                                    End If
                                    oClaimOpen.ClaimPeril(iPeril).Payment.CurrencyCode = Session(CNCurrenyCode)
                                    oClaimOpen.ClaimPeril(iPeril).Payment.CurrencyRate = 1 ' Default logic Always have currencyrate=1
                                    oClaimOpen.ClaimPeril(iPeril).Payment.PaymentAmount = CDec(dPaymentAmount)
                                    oClaimOpen.ClaimPeril(iPeril).Payment.RiskType = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iPeril).Payment.RiskType
                                    oClaimOpen.ClaimPeril(iPeril).Payment.ReserveType = .ClaimReserve(iPaymentIndex).TypeCode

                                    Select Case rblPaymentPartyType.Trim.ToUpper
                                        Case "CLMPAYABLE"
                                            rblPayee.SelectedValue = 0
                                            txtParty.Text = "CLMPAYABLE"
                                        Case "PARTY"
                                            rblPayee.SelectedValue = 1
                                            hPartyKey.Value = iPartykey
                                            txtParty.Text = sPartyname
                                        Case "AGENT"
                                            rblPayee.SelectedValue = 2
                                            txtParty.Text = oQuote.AgentCode
                                        Case "CLIENT"
                                            rblPayee.SelectedValue = 3
                                            txtParty.Text = oQuote.InsuredName
                                    End Select

                                    oClaimOpen.ClaimPeril(iPeril).Payment.ClaimPaymentItem(iPaymentIndex).LossPaymentAmount = CDec(dPaymentAmount)
                                    oClaimOpen.ClaimPeril(iPeril).Payment.ClaimPaymentItem(iPaymentIndex).PaymentAmount = CDec(dPaymentAmount)

                                    If oTaxForClaim IsNot Nothing Then
                                        oClaimOpen.ClaimPeril(iPeril).Payment.TaxAmount = oTaxForClaim.TaxCurrencyAmount
                                        oClaimOpen.ClaimPeril(iPeril).Payment.ClaimPaymentItem(iPaymentIndex).TaxGroupCode = sTaxGroupCode
                                        oClaimOpen.ClaimPeril(iPeril).Payment.ClaimPaymentItem(iPaymentIndex).TaxAmount = oTaxForClaim.TaxCurrencyAmount
                                    End If

                                    oClaimOpen.ClaimPeril(iPeril).Payment.ClaimPaymentItem(iPaymentIndex).CurrencyCode = Session(CNCurrenyCode)
                                    oClaimOpen.ClaimPeril(iPeril).Payment.ClaimPaymentItem(iPaymentIndex).CurrencyRate = 1 ' Default logic Always have currencyrate=1

                                    'Asigning of the PayQueue
                                    Dim bFirstElement As Boolean = True
                                    For iCount As Integer = 0 To .ClaimReserve.Count - 1
                                        If .ClaimReserve(iCount).PayQueue > 0 Then
                                            bFirstElement = False
                                            Exit For
                                        End If
                                    Next
                                    If bFirstElement = True Then
                                        .ClaimReserve(iPaymentIndex).PayQueue = 1
                                        oClaimOpen.ClaimPeril(iPeril).Payment.ClaimPaymentItem(iPaymentIndex).PayQueue = 1
                                    End If
                                End With

                                If bPaymentGridEditable = True Then
                                    oMaster = GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName)
                                    DisableControls(oMaster)
                                Else
                                    gvPaymentDetails.Enabled = True
                                    gvPaymentDetails.Columns(9).Visible = True
                                End If
                                Session(CNClaim) = oClaimOpen
                                Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_updated('','PaymentUpdation');", True)
                            End If
                        End If
                    End If
                End If
            End If
        End Sub

        Public Sub ClaimFinishButton(ByVal sender As Object, ByVal e As System.EventArgs)
            If Page.IsValid Then
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

                        If CType(Session.Item(CNMode), Mode) = Mode.NewClaim Or CType(Session.Item(CNMode), Mode) = Mode.EditClaim Or CType(Session.Item(CNMode), Mode) = Mode.PayClaim Then
                            WritePeril(Me)

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
                            'redirect the page if its required
                            PrePageRedirect()
                            If Not String.IsNullOrEmpty(sNextPage) And iDepth > 1 Then
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
                    Dim oRunClaimWorkFlow As NexusProvider.ProductClaimsWorkflowOptionsValue
                    Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                    'get the Product Risk option setting named Run Authorize Payment
                    sRunAuthorizePayment = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.RunAuthorisationScriptsClaimPayments, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
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
                        For Each PerilItemIndex As Integer In PerilsIndex
                            If CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(PerilItemIndex).ClaimReserve IsNot Nothing Then
                                For Each oPaymentItem As NexusProvider.ClaimPerilReservePaymentType In CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(PerilItemIndex).ClaimReserve
                                    dAmount += oPaymentItem.ThisPaymentINCLTax
                                Next
                            End If
                        Next
                        If dAmount > 0 Then
                            'Amount is positive so need to receive it
                            Response.Redirect("~/secure/payment/CashList.aspx", False)
                        Else
                            'Amount is negative so no need to receive it , so process it directly
                            Response.Redirect("~/claims/summary.aspx?ReturnUrl=" & Request.Path.Replace(WebRoot, "~/"))
                        End If
                    Else
                        'If RunAuthorize is OFF and Cash Payment Process is OFF
                        Response.Redirect("~/claims/summary.aspx?ReturnUrl=" & Request.Path.Replace(WebRoot, "~/"))
                    End If

                End If
            ElseIf Session(CNMode) = Mode.ViewClaim Then
                'show the summary page
                Response.Redirect("~/claims/summary.aspx?ReturnUrl=" & Request.Path.Replace(WebRoot, "~/"))
            End If
        End Sub

        Sub SkipSummaryPage()
            oMaster = GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName)
            Dim btnNextClmBuilder As Button = oMaster.FindControl("btnNext") 'Next Button of Claim Builder Page, when Claim Builder is ON
            Dim btnFinishClmBuilder As Button = oMaster.FindControl("btnFinish") 'Finish Button of Claim Builder Page, when Claim Builder is ON
            Dim hidChkCBuilderChoice As New HiddenField 'Retrieve Javascript  FurtherPaymentsConfirmation value, when Claim Builder is ON
            Dim hidChkCBuilderClaimClose As New HiddenField 'Retrieve Javascript OnCliamBuilderClaimCloseConfirmation value, when Claim Builder is ON
            Dim hidChkCBuilderPaymentMsg As New HiddenField

            Dim btnNext As Button = oMaster.FindControl("btn_Next") 'Next Button of Peril Page, when Claim Builder is OFF
            Dim btnFinish As Button = oMaster.FindControl("btn_Finish") 'Finish Button of Peril Page, when Claim Builder is OFF
            Dim hidChlClaimClose As HiddenField 'Retrieve Javascript ClaimCloseConfirmation value, when Claim Builder is OFF
            Dim hidChkChoice As HiddenField 'Retrieve Javascript PaymentsConfirmation value, when Claim Builder is OFF
            Dim hidChkPaymentMsg As HiddenField

            Dim oRunClaimWorkFlow As NexusProvider.ProductClaimsWorkflowOptionsValue = ViewState("RunClaimWorkFlow")
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
            Dim sRunAuthorizePayment As String
            Dim bCheckStatus As Boolean = False
            Dim sDisplayReinsurance As String

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
            sDisplayReinsurance = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.RiskTypeMaintenance, NexusProvider.ProductRiskOptions.Description, NexusProvider.RiskTypeOptions.DisplayClaimReinsurance, CType(Session(CNClaimQuote), NexusProvider.Quote).ProductCode, CType(Session(CNClaimQuote), NexusProvider.Quote).Risks(0).RiskTypeCode)

            'Check the User Authority to display of Reinsurance
            Dim oUserAuthority As New NexusProvider.UserAuthority
            oUserAuthority.UserCode = Session(CNLoginName)
            oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.DisplayClaimReinsurance
            oUserAuthority.UserAuthorityValue = "0"
            oWebservice = New NexusProvider.ProviderManager().Provider
            oWebservice.GetUserAuthorityValue(oUserAuthority)

            If Not IsPostBack Then
                If Session(CNMode) = Mode.PayClaim AndAlso CType(Session(CNEnablePayClaim), Boolean) _
                AndAlso (sDisplayReinsurance = "0" Or oUserAuthority.UserAuthorityValue = "0") Then

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

                                'in case of PayClaim - Show PaymentConfirmation Message at any stage of the claim builder page wherever user presses “Finish” button
                                If btnFinishClmBuilder IsNot Nothing Then
                                    btnFinishClmBuilder.Attributes.Add("onclick", "javascript:return FurtherPaymentsConfirmation();")
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

                ' get latest TimeStamp
                oclaimRisk = GetClaimRiskCall(oClaimDetails.BaseClaimKey, oClaimDetails.ClaimKey, sBranchCode)
                Session.Item(CNClaimRiskTimeStamp) = oclaimRisk.TimeStamp

                'Claim Risk has wrong argument called ClaimKey, actually it is BaseClaimKey
                oclaimRisk.ClaimKey = oClaimDetails.BaseClaimKey
                oclaimRisk.TimeStamp = Session.Item(CNClaimRiskTimeStamp)
                oclaimRisk.XMLDataSet = Session.Item(CNDataSet)


                If Session(CNMode) = Mode.NewClaim Then
                    Dim bClaimBuilder As Boolean
                    Boolean.TryParse(Session(CNClaimBuilder), bClaimBuilder)
                    If bClaimBuilder = True Then
                        'Update the claim builder risk screen
                        If Not UpdateClaimRiskCall(oclaimRisk, sBranchCode) Then
                            Exit Sub
                        End If

                    End If

                    If sDisplayReinsurance = "1" AndAlso oUserAuthority.UserAuthorityValue = "1" _
                    AndAlso oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.RiskTypes.RiskType(Trim(oClaimDetails.RiskType)).DisplayClaimReinsurance = True Then
                        'Overrides display ClaimReinsurance setting if DisplayClaimReinsurance set "False"  in web.config
                        'Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
                        'If oPortal.Claims.RiskTypes.RiskType(Trim(oClaimDetails.RiskType)).DisplayClaimReinsurance = True Then
                        Response.Redirect("~/claims/ClaimReinsurance.aspx", False)
                        'End If
                    Else
                        'Fire the Bind Claim
                        'arch issue 268
                        'oWebservice.BindClaim(oOriginalClaim, bClaimTimeStamp, 1, Nothing, sBranchCode)
                        If BindClaimCall(oOriginalClaim, bClaimTimeStamp, 1, Nothing, sBranchCode) Is Nothing Then
                            Exit Sub
                        End If
                    End If

                ElseIf Session(CNMode) = Mode.EditClaim Then
                    Dim bClaimBuilder As Boolean
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
                        'Overrides display ClaimReinsurance setting if DisplayClaimReinsurance set "False"  in web.config
                        'Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
                        'If oPortal.Claims.RiskTypes.RiskType(Trim(oClaimDetails.RiskType)).DisplayClaimReinsurance = True Then
                        Response.Redirect("~/claims/ClaimReinsurance.aspx", False)
                        'End If
                    Else
                        'Fire the Bind Claim
                        'arch issue 268
                        'oWebservice.BindClaim(oOriginalClaim, bClaimTimeStamp, 2, Nothing, sBranchCode)
                        If BindClaimCall(oOriginalClaim, bClaimTimeStamp, 2, Nothing, sBranchCode) Is Nothing Then
                            Exit Sub
                        End If
                    End If

                ElseIf Session(CNMode) = Mode.PayClaim Then
                    For Each oControl In oMaster.Controls
                        'check whether controls "claimsprogressbar.ascx" exist on this page
                        If oControl.FindControl("hidChkCBuilderChoice") IsNot Nothing Then
                            hidChkCBuilderChoice = CType(oControl.FindControl("hidChkCBuilderChoice"), HiddenField)
                        End If
                    Next

                    Dim oPayment As New NexusProvider.ClaimPayment
                    Dim PerilsIndex As New System.Collections.Generic.List(Of Integer)
                    Dim oClaimPerilPayment As New NexusProvider.ClaimPaymentCollection
                    If Session(CNClaimMultiPerilIndex) IsNot Nothing Then
                        PerilsIndex = Session(CNClaimMultiPerilIndex)
                    Else
                        PerilsIndex.Add(CInt(Session(CNClaimPerilIndex)))
                        'Fire the Bind Claim
                        oPayment = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(Session(CNClaimPerilIndex)).Payment
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
                    End If

                    For Each PerilItemIndex As Integer In PerilsIndex
                        oPayment = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(PerilItemIndex).Payment
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
                        oClaimPerilPayment.Add(oPayment)
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
                        'Dim PerilsIndex As New System.Collections.Generic.List(Of Integer)
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
                            Response.Redirect("~/secure/payment/CashList.aspx", False)
                        End If

                    End If


                    Try
                        Dim bClaimBuilder As Boolean
                        Boolean.TryParse(Session(CNClaimBuilder), bClaimBuilder)
                        If bClaimBuilder = True Then
                            ''Update the claim builder risk screen
                            If Not UpdateClaimRiskCall(oclaimRisk, sBranchCode) Then
                                Exit Sub
                            End If
                        End If

                        If sDisplayReinsurance = "1" AndAlso oUserAuthority.UserAuthorityValue = "1" _
                    AndAlso oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.RiskTypes.RiskType(Trim(oClaimDetails.RiskType)).DisplayClaimReinsurance = True Then
                            'Overrides display ClaimReinsurance setting if DisplayClaimReinsurance set "False"  in web.config
                            'Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
                            'If oPortal.Claims.RiskTypes.RiskType(Trim(oClaimDetails.RiskType)).DisplayClaimReinsurance = True Then
                            Response.Redirect("~/claims/ClaimReinsurance.aspx", False)
                            'End If
                        Else
                            If Session(CNMode) = Mode.PayClaim Then
                                'Fire the Bind Claim
                                'arch issue 268
                                'oWebservice.BindClaim(oOriginalClaim, bClaimTimeStamp, 4, oPayment, sBranchCode)
                                If oClaimPerilPayment IsNot Nothing Then
                                    If BindClaimCall(oOriginalClaim, bClaimTimeStamp, 4, Nothing, sBranchCode, oPaymentCollection:=oClaimPerilPayment) Is Nothing Then
                                        Exit Sub
                                    End If
                                Else
                                    If BindClaimCall(oOriginalClaim, bClaimTimeStamp, 4, oPayment, sBranchCode) Is Nothing Then
                                        Exit Sub
                                    End If
                                End If

                            Else
                                'Fire the Bind Claim
                                'arch issue 268
                                'oWebservice.BindClaim(oOriginalClaim, bClaimTimeStamp, 3, oPayment, sBranchCode)
                                If BindClaimCall(oOriginalClaim, bClaimTimeStamp, 3, oPayment, sBranchCode) Is Nothing Then
                                    Exit Sub
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
                                    End If

                                ElseIf hidChkCBuilderChoice.Value.Trim.ToUpper = "FALSE" Then
                                    Response.Redirect("~/Claims/Complete.aspx", False)
                                End If
                            End If
                        Else

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
                        'Overrides display ClaimReinsurance setting if DisplayClaimReinsurance set "False"  in web.config
                        'Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
                        'If oPortal.Claims.RiskTypes.RiskType(Trim(oClaimDetails.RiskType)).DisplayClaimReinsurance = True Then
                        Response.Redirect("~/claims/ClaimReinsurance.aspx", False)
                    End If
                End If

                ' Update the Claims session variable
                GetClaimDetails(oClaimDetails.ClaimKey, oclaimRisk)
                Response.Redirect("~/claims/complete.aspx", False)

            End If
        End Sub

#End Region
    End Class

End Namespace
