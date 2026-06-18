Imports System.Configuration.ConfigurationManager
Imports NexusProvider.SAMForInsurance
Imports Nexus
Imports Nexus.Utils
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Library
Imports CMS.Library
Imports System.Xml.XmlReader
Imports System.Xml.XPath
Imports System.Xml
Namespace Nexus

    Partial Class Framework_EditReserveItems
        Inherits CMS.Library.Frontend.clsCMSPage

        Private sJScriptDisableCalculate As String
        ''' <summary>
        ''' Tab Click Event
        ''' </summary>
        ''' <param name="Path"></param>
        ''' <remarks></remarks>
        Protected Sub TabIndex_TabClicked(ByVal Path As String) Handles TabIndex.TabClicked
            'Save the data back to the session object

            Dim iPeril As Integer = CInt(Request.QueryString("PerilIndex"))
            Dim oClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
            'Update the Reserve
            UpdateReserveData()
            'To check whether Peril builder screen is configured or not, if configured then user will move to the 
            'peril builder screen otherwise to the Perils.aspx page
            Dim sPerilConfig As String
            Dim sResult As String
            Dim sScreenCode As String
            Dim sFolder As String = String.Empty

            If oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.PerilTypes.PerilType(Trim(oClaim.ClaimPeril(iPeril).TypeCode)) IsNot Nothing Then
                sFolder = "~/Claims/ClientPages/" & oPortal.Claims.ScreenLocation & "/Perils/" _
                                & oPortal.Claims.PerilTypes.PerilType(oClaim.ClaimPeril(iPeril).TypeCode).Folder
            End If

            sPerilConfig = sFolder & "/perilscreens.config"

            If System.IO.File.Exists(Server.MapPath(sPerilConfig)) Then
                'If peril screen is configured then
                sScreenCode = GetScreenCode(sPerilConfig)
            Else
                'If peril screen is not configured then
                sScreenCode = oPortal.Claims.CorePerilScreenCode
            End If

            sResult = ValidateData(Trim(UCase(sScreenCode)))

            If String.IsNullOrEmpty(sResult) = True Then
                'if error is not thrown
                IsValidReserve.IsValid = True
            Else
                'if error is thrown
                IsValidReserve.ErrorMessage = sResult
                IsValidReserve.IsValid = False
            End If

            'To check whether any error exist on page or not
            If Page.IsValid Then
                Response.Redirect(Path, False)
            End If
        End Sub
#Region " Page Events "
        ''' <summary>
        ''' 'This event is fired on Page Load
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'if user is trying to access this page directly
            If Session(CNMode) Is Nothing Then
                Response.Redirect(AppSettings("WebRoot") & "Login.aspx", False)
            End If
            If Not IsPostBack Then
                'if page is loaded first time then setting of the status of progres bar
                ucProgressBar.OverviewStyle = "complete"
                ucProgressBar.PerilsStyle = "in-progress"
                ucProgressBar.SummaryStyle = "incomplete"
                ucProgressBar.ReinsuranceStyle = "incomplete"
                ucProgressBar.CompleteStyle = "incomplete"
            End If

            Dim oMode As Mode = CType(Session.Item(CNMode), Mode)

            If oMode <> Mode.ViewClaim Then

                If IsNumeric(Request.QueryString("PerilIndex")) Then
                    'Registration of the dynamic javascript
                    Page.ClientScript.RegisterStartupScript(GetType(String), "EnableCalculation", _
                    "<script type=""text/javascript"" language=""javascript""> document.getElementById('" _
                    & hdnCalculate.ClientID & "').value = '1';" & "</script>" & vbCr)

                    If Not IsPostBack Then
                        Dim iPeril As Integer = CInt(Request.QueryString("PerilIndex"))
                        'Retreiving the claim details from the session
                        Dim oClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)

                        If oClaim IsNot Nothing Then
                            'Populating the grid with reserve item
                            grdvReserveItem.DataKeyNames = Split("RevisedReserve")
                            grdvReserveItem.DataSource = oClaim.ClaimPeril(iPeril).Reserve
                            grdvReserveItem.DataBind()
                            lblPageTitle.Text = lblPageTitle.Text & oClaim.ClaimPeril(iPeril).Description
                        End If
                    End If
                Else
                    'no peril index, so we dont know which peril to edit, so go back
                    Response.Redirect("~\claims\perils.aspx", False)
                End If
         
            End If

        End Sub
        ''' <summary>
        ''' This event is fired on PagePreInit and sets the master page.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            'CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub
#End Region

#Region " GridView Events "
        ''' <summary>
        ''' This event is fired on the rowdatabound of the gridview.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvReserveItem_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvReserveItem.RowDataBound
            Dim oMode As Mode = CType(Session.Item(CNMode), Mode)
            If e.Row.RowType = DataControlRowType.DataRow Then

                'Find the reserve description from session and calculation of the Current Reserve
                Dim lblDescription As Label = CType(e.Row.FindControl("lblDescription"), Label)

                If lblDescription IsNot Nothing And Session.Item(CNReserveDescriptions) IsNot Nothing Then
                    lblDescription.Text = CType(Session.Item(CNReserveDescriptions), Hashtable).Item(CType(e.Row.DataItem, NexusProvider.Reserve).TypeCode)
                End If

                'Add js to update revised amount (clientside)
                Dim txtAmount As TextBox = CType(e.Row.FindControl("txtAmount"), TextBox)
                Dim lblRevisedReserve As Label = CType(e.Row.FindControl("lblRevisedReserve"), Label)
                Dim lblCurrentReserve As TextBox = CType(e.Row.FindControl("lblCurrentReserve"), TextBox)
                Dim lblInitialReserve As Label = CType(e.Row.FindControl("lblInitialReserve"), Label)
                Dim HiddenCurrReserve As HiddenField = CType(e.Row.FindControl("HiddenCurrReserve"), HiddenField)

                If txtAmount IsNot Nothing And lblRevisedReserve IsNot Nothing And lblCurrentReserve IsNot Nothing Then
                    If Session(CNMode) = Mode.NewClaim Then
                        txtAmount.Attributes.Add("onblur", "javascript:ReviseAmount(" _
                                                                    & CType(e.Row.DataItem, NexusProvider.Reserve).InitialReserve & ", " _
                                                                    & CType(e.Row.DataItem, NexusProvider.Reserve).RevisedReserve & ", " _
                                                                    & txtAmount.ClientID & ".value, " & lblCurrentReserve.ClientID & ",1)")
                    ElseIf Session(CNMode) = Mode.EditClaim Then
                        txtAmount.Attributes.Add("onblur", "javascript:ReviseAmount(" _
                                                                                           & CType(e.Row.DataItem, NexusProvider.Reserve).InitialReserve & ", " _
                                                                                           & CType(e.Row.DataItem, NexusProvider.Reserve).RevisedReserve & ", " _
                                                                                           & txtAmount.ClientID & ".value, " & lblCurrentReserve.ClientID & ",2)")
                    End If

                    lblInitialReserve.Text = Math.Round(CType(e.Row.DataItem, NexusProvider.Reserve).InitialReserve, 2)
                    lblRevisedReserve.Text = Math.Round(CType(e.Row.DataItem, NexusProvider.Reserve).RevisedReserve, 2) 'Format(CType(e.Row.DataItem, NexusProvider.Reserve).RevisedReserve, "##.##")
                    Dim sRevisedReserve As Double = CType(e.Row.DataItem, NexusProvider.Reserve).RevisedReserve
                    Dim sInitialReserve As Double = CType(e.Row.DataItem, NexusProvider.Reserve).InitialReserve
                    Dim sCurrentReserve As Double = Math.Round(sRevisedReserve + sInitialReserve, 2)
                    If sCurrentReserve = 0 Then
                        lblCurrentReserve.Text = Format(0.0, "##.##") 'sCurrentReserve
                    Else
                        lblCurrentReserve.Text = Format(sCurrentReserve, "##.##") 'sCurrentReserve
                    End If

                    HiddenCurrReserve.Value = sCurrentReserve.ToString

                End If
            End If

        End Sub
#End Region

#Region " Button Events "
        ''' <summary>
        ''' This event is fired on the Next Button click.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNext.Click

            If Page.IsValid Then
                'Save the data back to the session object

                Dim iPeril As Integer = CInt(Request.QueryString("PerilIndex"))
                Dim oClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())

                'Read the data from grid and update in the session object of the claim
                UpdateReserveData()
                'Pick up the latest claim details
                GetClaimDetails(oClaim.ClaimKey)
                'Set the Dirty Flag till it is not passed in validation
                Session(CNDirtyPeril) = True
                'To check whether Peril builder screen is configured or not, if configured then user will move to the 
                'peril builder screen otherwise to the Perils.aspx page
                Dim sFirstPage, sConfigFile, sFolder As String
                sFolder = String.Empty

                If oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.PerilTypes.PerilType(Trim(oClaim.ClaimPeril(iPeril).TypeCode)) IsNot Nothing Then
                    sFolder = "~/Claims/ClientPages/" & oPortal.Claims.ScreenLocation & "/Perils/" _
                                        & oPortal.Claims.PerilTypes.PerilType(oClaim.ClaimPeril(iPeril).TypeCode).Folder
                End If

                sConfigFile = sFolder & "/perilscreens.config"

                If System.IO.File.Exists(Server.MapPath(sConfigFile)) = True Then
                    sFirstPage = FrameWorkFunctions.GetFirstRiskScreen("~/Claims/ClientPages/" & oPortal.Claims.ScreenLocation & "/Perils/" _
                                           & oPortal.Claims.PerilTypes.PerilType(oClaim.ClaimPeril(iPeril).TypeCode).Folder & "/perilscreens.config")

                    Dim sUrl As String = "~/Claims/ClientPages/" & oPortal.Claims.ScreenLocation & "/Perils/" _
                        & oPortal.Claims.PerilTypes.PerilType(oClaim.ClaimPeril(iPeril).TypeCode).Folder _
                        & "/" & sFirstPage & "?OI=CP" & oClaim.ClaimPeril(iPeril).ClaimPerilKey & "&PerilID=" & oClaim.ClaimPeril(iPeril).ClaimPerilKey & "&PerilIndex=" & iPeril
                    Response.Redirect(sUrl, False)
                Else
                    ''To check the reserve validation
                    Dim sResult As String
                    sResult = ValidateData(Trim(UCase(oPortal.Claims.CorePerilScreenCode)))

                    If String.IsNullOrEmpty(sResult) = True Then
                        'if error is not thrown
                        'Clear the selected peril key 
                        Session.Remove(CNClaimPerilKey)
                        Session(CNDirtyPeril) = Nothing
                        Response.Redirect("~/Claims/Perils.aspx", False)
                    Else
                        'if error is thrown
                        IsValidReserve.ErrorMessage = sResult
                        IsValidReserve.IsValid = False
                    End If
                End If
            End If
        End Sub
        ''' <summary>
        ''' This Function Validates the Revise Amount
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub ValidateRevisedAmount()

            For Each oRowObject As GridViewRow In grdvReserveItem.Rows
                Dim dCurrentReserve As Decimal = CDec(IIf(CType(oRowObject.Cells(0).FindControl("HiddenCurrReserve"), HiddenField).Value <> String.Empty, CType(oRowObject.Cells(0).FindControl("HiddenCurrReserve"), HiddenField).Value, 0.0))

                Dim dAmount As Decimal = CDec(IIf(CType(oRowObject.Cells(0).FindControl("txtAmount"), TextBox).Text <> String.Empty, CType(oRowObject.Cells(0).FindControl("txtAmount"), TextBox).Text, 0.0))
                Dim dTotal As Decimal
                dTotal = dCurrentReserve + dAmount
                If dTotal < 0.0 And dAmount <> 0.0 Then
                    CType(oRowObject.Cells(0).FindControl("rqdvldRevised"), CustomValidator).IsValid = False
                End If
            Next

        End Sub
        Sub GetClaimDetails(ByVal v_iClaimKey As Integer)
            Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oOriginalClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
            Dim sBranchCode As String = oQuote.BranchCode
            'Retreiving the latest details
            'arch issue 268
            oClaimDetails = GetClaimDetailsCall(v_iClaimKey, sBranchCode)
            'updation of latest session values 
            Session.Item(CNClaimTimeStamp) = oClaimDetails.TimeStamp
            Session.Item(CNBaseClaimKey) = oClaimDetails.BaseClaimKey
            Session.Item(CNClaimKey) = oClaimDetails.ClaimKey
            Session.Item(CNClaimNumber) = oClaimDetails.ClaimNumber

            With oClaimDetails
                oOriginalClaim.CatastropheCode = .CatastropheCode
                oOriginalClaim.BaseClaimKey = .BaseClaimKey
                oOriginalClaim.Claim = .Claim
                oOriginalClaim.ClaimCoInsurer = .ClaimCoInsurer
                oOriginalClaim.ClaimDescription = .ClaimDescription
                oOriginalClaim.ClaimHandlerDescription = .ClaimHandlerDescription
                oOriginalClaim.ClaimKey = .ClaimKey
                oOriginalClaim.ClaimNumber = .ClaimNumber
                oOriginalClaim.ClaimPeril = .ClaimPeril
                oOriginalClaim.ClaimStatus = .ClaimStatus
                oOriginalClaim.ClaimStatusDate = .ClaimStatusDate
                oOriginalClaim.ClaimStatusID = .ClaimStatusID
                oOriginalClaim.ClaimVersion = .ClaimVersion
                oOriginalClaim.ClaimVersionDescription = .ClaimVersionDescription
                oOriginalClaim.ClientClaimNumber = .ClientClaimNumber
                oOriginalClaim.ClientEmail = .ClientEmail
                oOriginalClaim.ClientFaxNo = .ClientFaxNo
                oOriginalClaim.ClientMobileNo = .ClientMobileNo
                oOriginalClaim.ClientName = .ClientName
                oOriginalClaim.ClientShortName = .ClientShortName
                oOriginalClaim.ClientTelNo = .ClientTelNo
                oOriginalClaim.ClientTelNoOff = .ClientTelNoOff
                oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = .CloseClaimOnZeroReserveRecoveryBalance
                oOriginalClaim.Comments = .Comments
                oOriginalClaim.Contact = .Contact
                oOriginalClaim.CurrencyISOCode = .CurrencyISOCode
                oOriginalClaim.Description = .Description
                oOriginalClaim.ExternalHandler = .ExternalHandler
                oOriginalClaim.HandlerCode = .HandlerCode
                oOriginalClaim.IgnoreClaimMaintain = .IgnoreClaimMaintain
                oOriginalClaim.InfoOnly = .InfoOnly
                oOriginalClaim.InsuranceFileKey = .InsuranceFileKey
                oOriginalClaim.InsuranceRef = .InsuranceRef
                oOriginalClaim.InsurerClaimNumber = .InsurerClaimNumber
                oOriginalClaim.IsAllowedClosedClaims = .IsAllowedClosedClaims
                oOriginalClaim.IsDeleted = .IsDeleted
                oOriginalClaim.LastModifiedDate = .LastModifiedDate
                oOriginalClaim.LikelyClaim = .LikelyClaim
                oOriginalClaim.Location = .Location
                oOriginalClaim.LossDate = .LossDate
                oOriginalClaim.LossDateFrom = .LossDateFrom
                oOriginalClaim.LossFromDate = .LossToDate
                oOriginalClaim.LossToDate = .LossToDate
                oOriginalClaim.LossToDateSpecified = .LossToDateSpecified
                oOriginalClaim.Payments = .Payments
                oOriginalClaim.PolicyNumber = .PolicyNumber
                oOriginalClaim.PolicyType = .PolicyType
                oOriginalClaim.PrimaryCause = .PrimaryCause
                oOriginalClaim.PrimaryCauseCode = .PrimaryCauseCode
                oOriginalClaim.PrimaryCauseDescription = .PrimaryCauseDescription
                oOriginalClaim.ProductDescription = .ProductDescription
                oOriginalClaim.ProgressStatusCode = .ProgressStatusCode
                oOriginalClaim.ProgressStatusDescription = .ProgressStatusDescription
                oOriginalClaim.ReportedDate = .ReportedDate
                oOriginalClaim.RiskKey = .RiskKey
                oOriginalClaim.SecondaryCause = .SecondaryCause
                oOriginalClaim.SecondaryCauseCode = .SecondaryCauseCode
                oOriginalClaim.SecondaryCauseDescription = .SecondaryCauseDescription
                oOriginalClaim.TotalCurrentShareValue = .TotalCurrentShareValue
                oOriginalClaim.TotalShare = .TotalShare
                oOriginalClaim.Town = .Town
                oOriginalClaim.TownCode = .TownCode
                oOriginalClaim.UnderwritingYearCode = .UnderwritingYearCode
                oOriginalClaim.UserDefFldACode = .UserDefFldACode
                oOriginalClaim.UserDefFldBCode = .UserDefFldBCode
                oOriginalClaim.UserDefFldCCode = .UserDefFldCCode
                oOriginalClaim.UserDefFldDCode = .UserDefFldECode
                oOriginalClaim.UserDefFldECode = .UserDefFldECode
            End With
            Session.Item(CNClaim) = oOriginalClaim
        End Sub

        Function ValidateData(ByVal v_sScreenCode As String) As String
            Dim oOriginalClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim sbOutput As New StringBuilder
            Dim strValidationMsg As String = String.Empty
            'Use the GetDataSetDefinition to interogate the dataset to get the datamodelcode into session
            'Read DataModelCode from DataSet if it's not already in session
            Dim sDataModelCode As String = String.Empty
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
            Dim sXmlDataSet As String = Session.Item(CNDataSet)
            Dim iPeril As Integer = CInt(Request.QueryString("PerilIndex"))
            If Session.Item(CNDataModelCode) Is Nothing Then

                Dim XDoc As XPathDocument = New XPathDocument(New IO.StringReader(Session(CNDataSet)))
                Dim Navigator As XPathNavigator
                Navigator = XDoc.CreateNavigator()

                Dim i As XPathNodeIterator = Navigator.Select("DATA_SET")

                While (i.MoveNext)
                    sDataModelCode = i.Current.GetAttribute("DataModelCode", String.Empty)
                End While

                Session.Item(CNDataModelCode) = sDataModelCode
            Else

                sDataModelCode = Session.Item(CNDataModelCode)
            End If
            'To check the reserve validation
            oWebservice.RunValidationRules(Trim(UCase(v_sScreenCode)), sXmlDataSet, oOriginalClaim.ClaimKey, oOriginalClaim.ClaimPeril(iPeril).ClaimPerilKey)
            Session.Item(CNDataSet) = sXmlDataSet
            Dim srDataset As New System.IO.StringReader(sXmlDataSet)
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
            Return strValidationMsg

        End Function

        Sub UpdateReserveData()
            'Save the data back to the session object
            Dim iPeril As Integer = CInt(Request.QueryString("PerilIndex"))
            Dim oClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            Dim oModeClaim As Mode = CType(Session.Item(CNMode), Mode)
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
            'Flag to check which peril has been updated it need to be updated in DB
            oClaim.ClaimPeril(iPeril).PerilEdited = True

            For Each oRow As GridViewRow In grdvReserveItem.Rows
                'If oModeClaim <> Mode.NewClaim Then
                If hdnCalculate.Value = "1" Then
                    If Session(CNMode) = Mode.NewClaim Then
                        'JS is enabled so do the calucation again for save
                        Dim sAmount As String = Request.Form(grdvReserveItem.Rows(oRow.DataItemIndex).FindControl("txtAmount").UniqueID)
                        If String.IsNullOrEmpty(sAmount) = False Then
                            oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).RevisedReserve = sAmount
                            'Flag to check which reserve has been updated it need to be updated in DB
                            oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).ReserveEdited = True
                        Else
                            oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).RevisedReserve = oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).InitialReserve
                            'Flag to check which reserve has been updated it need to be updated in DB
                            oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).ReserveEdited = True
                        End If
                    ElseIf Session(CNMode) = Mode.EditClaim Then
                        'JS is enabled so do the calucation again for save
                        Dim sAmount As String = Request.Form(grdvReserveItem.Rows(oRow.DataItemIndex).FindControl("txtAmount").UniqueID)

                        If String.IsNullOrEmpty(sAmount) = False Then
                            oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).RevisedReserve = CDbl(sAmount)
                            'Flag to check which reserve has been updated it need to be updated in DB
                            oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).ReserveEdited = True
                        Else
                            'Flag to check which reserve has been updated it need to be updated in DB
                            oClaim.ClaimPeril(iPeril).Reserve(oRow.DataItemIndex).ReserveEdited = False
                        End If
                    End If
                End If
            Next
            Session.Item(CNClaim) = oClaim

            'if peril screen is not configured then need to validate the reserve with coreperilscreen code
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oClaimResponse As NexusProvider.ClaimResponse = Nothing
            Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
            Dim sBranchCode As String = oQuote.BranchCode

            If Session(CNMode) = Mode.NewClaim Then
                'arch issue 268
                'oClaimResponse = oWebservice.UpdateClaimReservesOrPayments(oClaim, Nothing, Session.Item(CNClaimTimeStamp), 1, sBranchCode)
                oClaimResponse = UpdateClaimReservesOrPaymentsCall(oClaim, Nothing, Session.Item(CNClaimTimeStamp), 1, sBranchCode)
            ElseIf Session(CNMode) = Mode.EditClaim Then
                'arch issue 268
                'oClaimResponse = oWebservice.UpdateClaimReservesOrPayments(oClaim, Nothing, Session.Item(CNClaimTimeStamp), 2, sBranchCode)
                oClaimResponse = UpdateClaimReservesOrPaymentsCall(oClaim, Nothing, Session.Item(CNClaimTimeStamp), 2, sBranchCode)
            End If
        End Sub
        Protected Sub btnFinish_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFinish.Click
            'Save the data back to the session object

            Dim iPeril As Integer = CInt(Request.QueryString("PerilIndex"))
            Dim oClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
            'Update the Reserve
            UpdateReserveData()

            'Set the Dirty Flag till it is not passed in validation
            Session(CNDirtyPeril) = True

            'To check whether Peril builder screen is configured or not, if configured then user will move to the 
            'peril builder screen otherwise to the Perils.aspx page
            Dim sPerilConfig As String
            Dim sResult As String
            Dim sScreenCode As String
            Dim sFolder As String = String.Empty

            If oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.PerilTypes.PerilType(Trim(oClaim.ClaimPeril(iPeril).TypeCode)) IsNot Nothing Then
                sFolder = "~/Claims/ClientPages/" & oPortal.Claims.ScreenLocation & "/Perils/" _
                                    & oPortal.Claims.PerilTypes.PerilType(oClaim.ClaimPeril(iPeril).TypeCode).Folder
            End If

            sPerilConfig = sFolder & "/perilscreens.config"

            If System.IO.File.Exists(Server.MapPath(sPerilConfig)) Then
                'If peril screen is configured then
                sScreenCode = GetScreenCode(sPerilConfig)
            Else
                'If peril screen is not configured then
                sScreenCode = oPortal.Claims.CorePerilScreenCode
            End If

            sResult = ValidateData(Trim(UCase(sScreenCode)))

            If String.IsNullOrEmpty(sResult) = True Then
                'if error is not thrown
                IsValidReserve.IsValid = True
            Else
                'if error is thrown
                IsValidReserve.ErrorMessage = sResult
                IsValidReserve.IsValid = False
            End If

            'To check whether any error exist on page or not
            If Page.IsValid Then
                'Clear the selected peril key 
                Session.Remove(CNClaimPerilKey)
                Session(CNDirtyPeril) = Nothing
                Response.Redirect("~/claims/perils.aspx", False)
            End If
        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            Response.Redirect("~/claims/perils.aspx", False)
        End Sub
#End Region

    End Class

End Namespace
