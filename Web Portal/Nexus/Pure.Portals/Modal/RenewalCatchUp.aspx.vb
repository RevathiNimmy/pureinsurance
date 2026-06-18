Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants.Session
Imports System.Web.Configuration
Imports Nexus.Library
Imports CMS.Library
Imports Nexus.Utils
Imports Nexus.Constants
Imports Nexus.Web.UI.WebControls

Imports Nexus.Constants.Constant
Imports System.Web.Services
Imports SiriusFS.SAM.Client
Imports NexusProvider.Quote
Imports System.Linq
Imports System.Text
Imports System.Xml.Linq
Imports System.IO
Imports System.Data


Namespace Nexus

    Partial Class Modal_RenewalCatchUp
        Inherits CMS.Library.Frontend.clsCMSPage
        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
        Dim oInsuranceFolderKey As Integer
        Dim oInsuranceFileKey As Integer
        Dim productCode As String
        Dim ocoverStartDate As DateTime
        Dim ocoverEndDate As DateTime
        Dim oPreferredDate As DateTime
        Dim oRenewalDate As DateTime
        Dim oBillingMethod As String
        Dim oAmount As String
        Dim progress As Integer
        Dim netPrem As Decimal
        Dim oAnversarydate As DateTime
        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub
        Protected Sub Page_Load1(sender As Object, e As EventArgs) Handles Me.Load
            Dim insuranceFileRef As String
            insuranceFileRef = Convert.ToString(Request.QueryString("InsuranceFileRef"))
            ocoverStartDate = Convert.ToDateTime(Request.QueryString("CoverStartDate"))
            ocoverEndDate = Convert.ToDateTime(Request.QueryString("CoverEndDate"))
            oPreferredDate = Convert.ToDateTime(Request.QueryString("PreferredDate"))
            netPrem = CheckAndCalculateRoundOff()
            If Not Page.IsPostBack Then

                lblRenewed.Visible = False
                btnRenewedOk.Visible = False
                grdPolicyVersions.Visible = False
            End If
            'Dim script As String = "$(document).ready(function () { $('[id*=btnRenewYes]').click(); });"
            'ClientScript.RegisterStartupScript(Me.GetType, "load", script, True)
            If ScriptManager.GetCurrent(Me.Page) IsNot Nothing Then
                If Not (Page.ClientScript.IsStartupScriptRegistered("EndRequestHandlerForUpdatePanel")) Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "EndRequestHandlerForUpdatePanel", "Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandlerForUpdatePanel);", True)
                End If
            End If

        End Sub



        Protected Sub btnRenewYes_Click(sender As Object, e As EventArgs) Handles btnRenewYes.Click

            grdPolicyVersions.Visible = True
            btnRenewYes.Enabled = False
            btnRenewNo.Enabled = False
            Dim oInsuranceFileDetailsCollection As NexusProvider.InsuranceFileDetailsCollection
            oInsuranceFileDetailsCollection = Nothing

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

            'Get search results by calling FindPolicy

            Dim sInsuranceRef As String = IIf(String.IsNullOrEmpty(Request.QueryString("InsuranceFileRef")), Nothing, Request.QueryString("InsuranceFileRef"))
            'Dim sRiskIndex As String = IIf(String.IsNullOrEmpty(txtRiskIndex.Text), Nothing, txtRiskIndex.Text)
            'Dim sClientShortName As String = IIf(String.IsNullOrEmpty(txtClient.Text), Nothing, txtClient.Text)
            Dim sQuoteType As NexusProvider.InsuranceFileType
            Dim iMaxRowsToFetch As Integer = oPortal.MaxSearchResults

            sQuoteType = NexusProvider.InsuranceFileTypes.ALL

            oInsuranceFileDetailsCollection = oWebService.FindPolicy(sInsuranceRef, Nothing, Nothing, sQuoteType, False, iMaxRowsToFetch, Nothing)

            Dim startInsFileCount As Integer = oInsuranceFileDetailsCollection.Count
            Dim dtable As New DataTable
            dtable.Columns.Add(New DataColumn("CoverFrom"))
            dtable.Columns.Add(New DataColumn("RenewalDate"))
            dtable.Columns.Add(New DataColumn("BillingMethod"))
            dtable.Columns.Add(New DataColumn("Amount"))
            dtable.Columns.Add(New DataColumn("Status"))

            Dim message As String
            Dim bProcessFailed As Boolean = False

            Dim coverStartDate As DateTime
            Dim coverEndDate As DateTime
            Dim anniverssaryCheck As Integer = 0
            coverStartDate = ocoverStartDate
            coverEndDate = ocoverEndDate
            'Dim j As Integer = 0

            While coverStartDate <= DateTime.Today
                progress = 0
                If coverEndDate < DateTime.Today AndAlso anniverssaryCheck < 11 Then

                    Try
                        RenewalSelection()
                    Catch ex As Exception
                        'message = "alert('Auto renewal faced some issue please renew the subsequent policy versions manually!')"
                        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", message, True)
                        lblRenewed.Text = "The automatic renewal process has failed, Please renew policy manually"
                        bProcessFailed = True
                        'MsgBox("Auto renewal faced some issue please renew the subsequent policy versions manually!", MsgBoxStyle.Information, "Warning!")
                        Exit While
                    End Try



                    sQuoteType = NexusProvider.InsuranceFileTypes.ALL

                    oInsuranceFileDetailsCollection = oWebService.FindPolicy(sInsuranceRef, Nothing, Nothing, sQuoteType, False, iMaxRowsToFetch, Nothing)

                    If (oInsuranceFileDetailsCollection.Count < (anniverssaryCheck + 1) AndAlso startInsFileCount = 1) OrElse (startInsFileCount <> 1 AndAlso (oInsuranceFileDetailsCollection.Count - startInsFileCount + 1) < (anniverssaryCheck + 1)) Then
                        'message = "alert('Auto renewal faced some issue please renew the subsequent policy versions manually!')"
                        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", message, True)
                        'MsgBox("Auto renewal faced some issue please renew the subsequent policy versions manually!", MsgBoxStyle.Information, "Warning!")
                        If (dtable.Rows.Count = 2) Then
                            dtable.Rows(anniverssaryCheck - 2).Item(4) = "Current"
                            Dim MyDate1 As DateTime = ocoverStartDate.AddMonths(1)
                            ocoverStartDate = DateSerial(MyDate1.Year, MyDate1.Month, 1)
                            MyDate1 = ocoverStartDate.AddMonths(1)
                            oRenewalDate = DateSerial(MyDate1.Year, MyDate1.Month, 1)
                            dtable.Rows(anniverssaryCheck - 1).Item(0) = ocoverStartDate.ToShortDateString()
                            dtable.Rows(anniverssaryCheck - 1).Item(1) = oRenewalDate.ToShortDateString()
                            dtable.Rows(anniverssaryCheck - 1).Item(4) = "Under Renewal"
                        Else
                            dtable.Rows(anniverssaryCheck - 2).Item(4) = "Current"
                            Dim MyDate1 As DateTime = ocoverStartDate.AddMonths(1)
                            ocoverStartDate = DateSerial(MyDate1.Year, MyDate1.Month, 1)
                            MyDate1 = ocoverStartDate.AddMonths(1)
                            oRenewalDate = DateSerial(MyDate1.Year, MyDate1.Month, 1)
                            dtable.Rows(anniverssaryCheck - 1).Item(0) = ocoverStartDate.ToShortDateString()
                            dtable.Rows(anniverssaryCheck - 1).Item(1) = oRenewalDate.ToShortDateString()
                            dtable.Rows(anniverssaryCheck - 1).Item(3) = Math.Round(Convert.ToDecimal(Session(CNAmountToPay)), 2)
                            dtable.Rows(anniverssaryCheck - 1).Item(4) = "Under Renewal"
                        End If


                        dtable.AcceptChanges()

                        If Not oRenewalDate >= oAnversarydate Then
                            lblRenewed.Text = "The automatic renewal process has failed, Please renew policy manually"
                            bProcessFailed = True
                        End If

                        Exit While
                    Else

                        Dim RowValues As Object()
                        If anniverssaryCheck = 0 Then
                            RowValues = {ocoverStartDate.ToString("dd/MM/yyyy"), oRenewalDate.ToString("dd/MM/yyyy"), oBillingMethod, Math.Round(netPrem, 2), "Replaced"}
                        Else
                            RowValues = {ocoverStartDate.ToString("dd/MM/yyyy"), oRenewalDate.ToString("dd/MM/yyyy"), oBillingMethod, Math.Round(Convert.ToDecimal(oAmount), 2), "Replaced"}
                        End If


                        'create new data row
                        Dim dRow As DataRow
                        dRow = dtable.Rows.Add(RowValues)
                        dtable.AcceptChanges()

                        Try
                            RenewalManage()
                        Catch ex As Exception
                            'message = "alert('Auto renewal faced some issue please renew the subsequent policy versions manually!')"
                            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", message, True)
                            'lblRenewed.Text = "The automatic renewal process has failed, Please renew policy manually"
                            ''MsgBox("Auto renewal faced some issue please renew the subsequent policy versions manually!", MsgBoxStyle.Information, "Warning!")
                            'dtable.Rows(anniverssaryCheck).Item(4) = "Current"
                            'Dim MyDate1 As Date = ocoverStartDate.AddMonths(1)
                            'ocoverStartDate = DateSerial(MyDate1.Year, MyDate1.Month, 1)
                            'MyDate1 = ocoverStartDate.AddMonths(1)
                            'oRenewalDate = DateSerial(MyDate1.Year, MyDate1.Month, 1)
                            'dtable.Rows(anniverssaryCheck + 1).Item(0) = ocoverStartDate
                            'dtable.Rows(anniverssaryCheck + 1).Item(1) = oRenewalDate
                            'dtable.Rows(anniverssaryCheck + 1).Item(4) = "Under Renewal"

                            'dtable.AcceptChanges()
                            'Exit While
                        End Try



                        anniverssaryCheck = anniverssaryCheck + 1
                    End If


                Else

                    'Create object for RowValues

                    Dim RowValues As Object()
                    Dim IsProcessTerminate As Boolean = False
                    If anniverssaryCheck + 1 = 13 AndAlso anniverssaryCheck + 1 > 12 Then
                        Dim MyDate1 As DateTime = ocoverStartDate.AddMonths(1)
                        ocoverStartDate = DateSerial(MyDate1.Year, MyDate1.Month, 1)
                        MyDate1 = ocoverStartDate.AddMonths(1)
                        oRenewalDate = DateSerial(MyDate1.Year, MyDate1.Month, 1)
                        RowValues = {ocoverStartDate.ToString("dd/MM/yyyy"), oRenewalDate.ToString("dd/MM/yyyy"), oBillingMethod, Math.Round(Convert.ToDecimal(oAmount), 2), "Renewal Quote"}
                        IsProcessTerminate = True
                    Else
                        If anniverssaryCheck = 11 Then
                            Try
                                RenewalSelection()
                            Catch ex As Exception
                                'message = "alert('Auto renewal faced some issue please renew the subsequent policy versions manually!')"
                                'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", message, True)
                                lblRenewed.Text = "The automatic renewal process has failed, Please renew policy manually"
                                bProcessFailed = True
                                'MsgBox("Auto renewal faced some issue please renew the subsequent policy versions manually!", MsgBoxStyle.Information, "Warning!")
                                Exit While
                            End Try
                        Else
                            Dim MyDate1 As DateTime = ocoverStartDate.AddMonths(1)
                            ocoverStartDate = DateSerial(MyDate1.Year, MyDate1.Month, 1)
                            MyDate1 = ocoverStartDate.AddMonths(1)
                            oRenewalDate = DateSerial(MyDate1.Year, MyDate1.Month, 1)
                        End If


                        RowValues = {ocoverStartDate.ToString("dd/MM/yyyy"), oRenewalDate.ToString("dd/MM/yyyy"), oBillingMethod, Math.Round(Convert.ToDecimal(oAmount), 2), "Current"}
                    End If
                    'create new data row
                    Dim dRow As DataRow
                    dRow = dtable.Rows.Add(RowValues)
                    dtable.AcceptChanges()

                    anniverssaryCheck = anniverssaryCheck + 1
                    If IsProcessTerminate Then
                        Exit While
                    End If
                End If

                Dim MyDate As DateTime = ocoverStartDate.AddMonths(1)
                coverEndDate = New Date(MyDate.Year, MyDate.Month, Date.DaysInMonth(MyDate.Year, MyDate.Month))
                coverStartDate = DateSerial(MyDate.Year, MyDate.Month, 1)


            End While
            If dtable.Rows.Count > 0 AndAlso bProcessFailed Then
                Dim query = dtable.AsEnumerable().Where(Function(r) r.Field(Of String)("Status").ToLower() = "under renewal")
                If query.ToList().Count > 0 Then
                    Dim rowToRemove = dtable.AsEnumerable().Where(Function(r) r.Field(Of String)("Status").ToLower() = "replaced")
                    For Each row In rowToRemove.ToList()
                        row.Delete()
                    Next
                End If
                dtable.AcceptChanges()
            End If
            'now bind datatable to gridview... 
            grdPolicyVersions.DataSource = dtable
            grdPolicyVersions.DataBind()

            ' Below code will fetch the current live version of this policy
            oInsuranceFileDetailsCollection = oWebService.FindPolicy(sInsuranceRef, Nothing, Nothing, NexusProvider.InsuranceFileTypes.POLICY, False, iMaxRowsToFetch, Nothing)
            If Not IsNothing(oInsuranceFileDetailsCollection) andalso oInsuranceFileDetailsCollection.Count > 0 Then
                Session(CNQuote) = oWebService.GetHeaderAndSummariesByKey(oInsuranceFileDetailsCollection.Item(0).InsuranceFileKey, bExclusiveLock:=True)
            End If


            lblRenewed.Visible = True
            btnRenewedOk.Visible = True
            Session.Remove(CNPartyBankDetail)
            Session.Remove(CNSelectedSchemeNo)
        End Sub

        Protected Sub RenewalManage()
            '--Get Policices in renewal
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
            Dim oPolCol As New NexusProvider.PolicyCollection
            Dim oParty As NexusProvider.BaseParty = Session(CNParty)
            Dim branchCode As String
            Dim agentKey, partyKey As Integer
            Dim renewalDate As Date
            Dim productType As String

            productType = productCode
            'Set the Branch Code
            branchCode = 0

            'Set the Agent Key
            'Set the Party Key


            'Renewal Date

            progress = 40
            If oUserDetails IsNot Nothing Then
                If oUserDetails.Key = 0 AndAlso agentKey = 0 Then
                    ' -	Note that if the logged in user is an agent, then the agent code drop down should not be displayed.
                    'AgentPanel.Visible = False
                    'Get all Policies in Renewals for all the client against Agent key
                    oPolCol = oWebService.GetPoliciesInRenewal(partyKey, Nothing, productType, renewalDate, Nothing, Nothing, branchCode, v_sInsuranceRef:=Request.QueryString("InsuranceFileRef"))
                Else
                    oPolCol = oWebService.GetPoliciesInRenewal(partyKey, agentKey, productType, renewalDate, Nothing, Nothing, branchCode, v_sInsuranceRef:=Request.QueryString("InsuranceFileRef"), v_bShowUserOnly:=True)
                End If
            End If




            'Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote
            'Dim oParty As NexusProvider.BaseParty = Session(CNParty)
            Dim nInsuranceFolderKey As Integer
            Dim message As String
            Dim oExclusiveLocking As NexusProvider.OptionTypeSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ExclusiveLock)
            'If Not LCase(e.CommandName).Equals("page") And Not LCase(e.CommandName).Equals("sort") Then
            '    'Check for Exclusive lock
            '    Dim GridRow As GridViewRow = CType((e.CommandSource).NamingContainer, GridViewRow)
            '    Dim lblInsuranceFolderKey As Label = GridRow.FindControl("lblInsuranceFolderKey")
            nInsuranceFolderKey = oInsuranceFolderKey
            'End If
            If oExclusiveLocking.OptionValue = "1" Then
                Dim userName As String = UnlockPolicy(nInsuranceFolderKey)
                If userName.Trim.Length > 0 Then
                    message = "alert('" + Replace(GetLocalResourceObject("lbl_policylocked_error"), "{1}", userName + ".") + "')"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", message, True)
                    Exit Sub
                End If
            End If
            Try
                oQuote = oWebService.GetHeaderAndSummariesByKey(oInsuranceFileKey, bExclusiveLock:=True)

                If Session(CNParty) Is Nothing Then
                    oParty = oWebService.GetParty(oQuote.PartyKey)
                    Session(CNParty) = oParty
                End If

                'Locking message is required for details Mode
                Dim ignoreLocking As Boolean = False
                ' Put highest risk key into Session
                If Not oQuote.Risks Is Nothing AndAlso oQuote.Risks.Count > 0 Then
                    'Populate XML dataset atleast for first risk as it will help to get datamodal code and quick quote flag
                    For i As Integer = 0 To oQuote.Risks.Count - 1
                        oWebService.GetRisk(oQuote.Risks(i).Key, i, oQuote, oQuote.BranchCode, v_bIgnoreLocking:=ignoreLocking)
                    Next
                End If

                oWebService.GetHeaderAndRisksByKey(oQuote)
                Session(CNCurrenyCode) = oQuote.CurrencyCode
                Session(CNQuote) = oQuote

                'Use the GetDataSetDefinition to interogate the dataset to get the datamodelcode into session
                GetDataSetDefinition()

                progress = 55
            Catch ex As NexusProvider.NexusException
                'Policy locking error
                Select Case CType(ex.Errors(0), NexusProvider.NexusError).Code
                    Case "200", "1000158" 'Policy Locking
                        'Show policy locking error as alert
                        Dim sLockingMessage As String = "alert('" + ex.Errors(0).Description + ".\n" + ex.Errors(0).Detail + "')"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", sLockingMessage, True)
                        Server.ClearError()
                        ClearQuote()
                        Exit Sub
                    Case Else
                        Throw
                End Select
            Finally

            End Try

            Dim isPendingPortfolioTransfer, isPendingCloneTransfer As Boolean
            Try
                oWebService.IsPendingTransfer(oQuote.InsuranceFileKey, isPendingCloneTransfer, isPendingPortfolioTransfer, oQuote.InsuranceFileRef)
                message = ""
                If isPendingCloneTransfer OrElse isPendingPortfolioTransfer Then
                    If isPendingPortfolioTransfer Then
                        message = Convert.ToString(GetLocalResourceObject("msg_PendingPortfolioTransfer"))
                    ElseIf isPendingCloneTransfer Then
                        message = Convert.ToString(GetLocalResourceObject("msg_PendingClonedTransfer"))
                    End If
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "PendingPortfolioTransfer", "alert('" + message + "')", True)
                    Exit Sub
                End If
                Session.Remove(CNMTAType)
                Session.Remove(CNMTATypeDesc)
                Session.Remove(CNRenewal)
                Session.Remove(CNRenewalShowPremium)
                Session.Remove(CNRiskType)
                Session.Remove(CNMode)
                Session(CNMode) = Mode.Buy
                Session.Remove(CNOI)
                Session(CNQuoteInSync) = False
                Session.Remove(CNStatus)
                Session(CNRenewal) = True
                Session.Remove(CNQuoteMode)
                Session(CNQuoteMode) = QuoteMode.FullQuote



                'Process Buy
                '
                '
                '
                '
                progress = 70
                BuyPolicy()
                progress = 100
            Catch ex As System.Exception
                Throw

            Finally
                oWebService = Nothing
            End Try
            '
            '
            '
            '
            'Process accept

            'will update the premium with agent commision if agent type is BROKER
            'ReplicateStatements()

            '
            '
            '
            'QuoteBind
            '
            '
            '
            'ReplicateTransactions()
        End Sub

        Private Function UnlockPolicy(ByVal nInsuranceFileKey As Integer) As String
            Dim oLockCollection As NexusProvider.LockCollection
            Dim oWebService As NexusProvider.ProviderBase = Nothing
            Dim userName As String = String.Empty
            Dim maintainedSuccess As Boolean = False
            Dim logout As Boolean = False
            Dim bAllClear As Boolean = False
            Dim oLock As New NexusProvider.Locks
            oWebService = New NexusProvider.ProviderManager().Provider
            oLockCollection = oWebService.GetLockDetails(Session(CNBranchCode).ToString())

            For Each oLockItem As NexusProvider.Locks In oLockCollection
                If oLockItem.LockName.Trim() = "insurance_folder_cnt" AndAlso oLockItem.LockValue = nInsuranceFileKey Then
                    oLock.LockName = oLockItem.LockName
                    oLock.LockValue = oLockItem.LockValue
                    oLockCollection.Add(oLock)
                    If HttpContext.Current.User.Identity.Name.ToLower().Trim().ToUpper = oLockItem.LockUserName.ToLower().Trim().ToUpper Then
                        maintainedSuccess = oWebService.MaintainLock(oLockCollection, bAllClear, logout, Session(CNBranchCode).ToString())
                        userName = String.Empty
                    Else
                        userName = oLockItem.LockUserName.Trim
                    End If
                    Exit For
                End If
            Next
            Return userName
        End Function

        Protected Sub ReplicateTransactions()
            Dim paymentOptions As Config.PaymentTypes = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).PaymentTypes
            Dim oPayment As NexusProvider.Payment = Nothing
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oPolicySummary As NexusProvider.PolicySummary
            Dim sBackDatedMTA As Boolean = False 'To check that MTA is backdated or not
            'oQuote = oWebService.GetHeaderAndSummariesByKey(v_iInsuranceFileKey:=oQuote.InsuranceFileKey)
            'If (Session(CNPaid) = True OrElse Session(CNStatementsAgreed) Is Nothing) AndAlso Session(CNIsTransactionConfirmationVisited) Is Nothing AndAlso
            '    (Session(CNIsCancelMTA) Is Nothing OrElse CType(Session(CNIsCancelMTA), Boolean) = False) Then

            'If Not IsPostBack Then

            'SetPageProgress(7)
            'oQuote.PaymentMethod

            'this checks if in case of NB will display the schedule and Certificate
            Dim totalPremium As Decimal
            If oQuote.Risks.Count > 0 Then
                totalPremium = Session(CNAmountToPay)
            End If

            If (totalPremium <= 0.0) And (Session(CNMTAType) <> MTAType.CANCELLATION) And Session(CNMTAType) IsNot Nothing Then
                'In case of MTA Permanent or Temporary
                If Not Session(CNMTAType) Is Nothing Then
                    'If (totalPremium = 0.0) Then
                    '    lblMTAPremiumReturn.Text = GetLocalResourceObject("lbl_NoChange_Text").ToString()
                    'ElseIf (totalPremium < 0.0) Then
                    '    lblMTAPremiumReturn.Text = GetLocalResourceObject("lbl_ReturnPremium_Text").ToString()
                    'End If
                    If (totalPremium = 0.0) Then
                        oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.Cheque)
                    Else
                        oPayment = Session(CNPayment)
                    End If
                End If
            ElseIf (totalPremium = 0.0) And Session(CNMTAType) Is Nothing Then
                'New Business
                oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.Cheque)
            ElseIf (totalPremium <= 0.0) And (Session(CNMTAType) = MTAType.CANCELLATION) Then
                'In case of MTA Cancellation
                'lblMTAPremiumReturn.Text = GetLocalResourceObject("lbl_ReturnPremium_Text").ToString()
                If Session(CNPayment) IsNot Nothing Then
                    oPayment = Session(CNPayment)
                    'If oPayment.PayNowPaymentDetails IsNot Nothing Then
                    '    lblMTAPremiumReturn.Text = GetLocalResourceObject("lbl_CancelTransaction_text").ToString()
                    '    Select Case UCase(oPayment.PayNowPaymentDetails.MediaTypeCode)
                    '        Case "CA"
                    '            lblMTAPremiumReturn.Text = lblMTAPremiumReturn.Text.Replace("#MediaType", "Cash")
                    '        Case "CC"
                    '            lblMTAPremiumReturn.Text = lblMTAPremiumReturn.Text.Replace("#MediaType", "Credit Card")
                    '        Case "BD"
                    '            lblMTAPremiumReturn.Text = lblMTAPremiumReturn.Text.Replace("#MediaType", "Bankers Draft")
                    '        Case "DD"
                    '            lblMTAPremiumReturn.Text = lblMTAPremiumReturn.Text.Replace("#MediaType", "Direct Debit")
                    '        Case "CQ"
                    '            lblMTAPremiumReturn.Text = lblMTAPremiumReturn.Text.Replace("#MediaType", "Cheque")
                    '        Case "EFT"
                    '            lblMTAPremiumReturn.Text = lblMTAPremiumReturn.Text.Replace("#MediaType", "EFT")
                    '    End Select
                    'Else
                    '    lblMTAPremiumReturn.Text = GetLocalResourceObject("lbl_ReturnPremium_Text").ToString()
                    'End If
                Else
                    oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.Cheque)
                    'lblMTAPremiumReturn.Text = GetLocalResourceObject("lbl_CancelTransaction_text").ToString()
                    'lblMTAPremiumReturn.Text = lblMTAPremiumReturn.Text.Replace("#MediaType", "Cheque")
                End If
            Else

                Select Case paymentOptions.PaymentType(Session(CNSelectedPaymentIndex)).Type
                    Case "Invoice"
                        'oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.AgentCollection, CDec(Session(CNAmountToPay)))
                        If Session(CNPayment) Is Nothing Then 'AgentCollection without Pre Payment
                            oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.AgentCollection, CDec(Session(CNAmountToPay)))
                        Else 'AgentCollection with Pre Payment
                            oPayment = Session(CNPayment)
                        End If
                    Case "PayNow"
                        oPayment = Session(CNPayment) 'PayNow
                    Case "DebitCard"
                        oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.DebitCard, CDec(Session(CNAmountToPay)))
                    Case "CreditCard"
                        'oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.CreditCard, CDec(Session(CNAmountToPay)))
                        If Session(CNPayment) Is Nothing Then
                            oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.CreditCard, CDec(Session(CNAmountToPay)))
                        Else 'AgentCollection with Pre Payment
                            oPayment = Session(CNPayment)
                        End If
                    Case "BankersDraft"
                        oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.BankersDraft, CDec(Session(CNAmountToPay)))
                    Case "Cash"
                        oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.Cash, CDec(Session(CNAmountToPay)))
                    Case "Cheque"
                        oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.Cheque, CDec(Session(CNAmountToPay)))
                    Case "PremiumFinance"
                        SaveFinancePlan()
                        oPayment = Session(CNPayment)
                    Case Else
                        SaveFinancePlan()
                        oPayment = Session(CNPayment)
                End Select

            End If

            If Session(CNSelectedAccount) IsNot Nothing AndAlso oPayment IsNot Nothing Then
                If Convert.ToString(Session(CNSelectedAccount)) = "Client" Then
                    oPayment.DebitAgainstAccount = "CLIENT"
                    oPayment.DebitAgainst = NexusProvider.DebitAgainstType.DebitAgainstFloatBalance
                End If
                If Convert.ToString(Session(CNSelectedAccount)) = "Agent" Then
                    oPayment.DebitAgainstAccount = "AGENT"
                    oPayment.DebitAgainst = NexusProvider.DebitAgainstType.DebitAgainstFloatBalance
                End If
            End If

            '[start]Changes as per WPR 63 --- to change the status of live quote
            Dim oQuoteVersionSetting As NexusProvider.OptionTypeSetting
            Dim oQuoteStatus As NexusProvider.Quote.QuoteStatusType
            If Session(CNQuoteMode) = QuoteMode.FullQuote AndAlso Session(CNRenewal) Is Nothing AndAlso Session(CNMTAType) Is Nothing Then


                Dim sQuoteVersioning As String = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsQuoteVersioning, NexusProvider.RiskTypeOptions.Code, oQuote.ProductCode, "")
                If (Not String.IsNullOrEmpty(sQuoteVersioning) AndAlso sQuoteVersioning.Trim = "1") Then
                    oQuoteStatus = oQuote.QuoteStatusKey
                    oQuote.QuoteStatusKey = NexusProvider.Quote.QuoteStatusType.Live
                    oWebService.UpdateQuoteStatus(oQuote)
                End If

            End If
            '[end]Changes as per WPR 63 --- to change the status of live quote


            Try

                If Session(CNIsBackDatedMTA) = True Or Session(CNBackDatedReinstatement) = True Then
                    sBackDatedMTA = True
                End If

                If Session(CNMTAType) = MTAType.PERMANENT Or Session(CNMTAType) = MTAType.TEMPORARY Then
                    oPolicySummary = New NexusProvider.PolicySummary(oQuote.Reference)
                    '6389 - MTA Refund Process on Instalments
                    If totalPremium < 0 AndAlso (Session(CNMTAType) <> MTAType.CANCELLATION) Then
                        oPolicySummary = oWebService.BindQuote(oQuote.InsuranceFileKey, oPayment, oQuote.TimeStamp, Nothing, Nothing, "MTA", sBackDatedMTA, v_bPayNegativePremiumMTABalance:=True)
                    Else
                        oPolicySummary = oWebService.BindQuote(oQuote.InsuranceFileKey, oPayment, oQuote.TimeStamp, Nothing, Nothing, "MTA", sBackDatedMTA)
                    End If

                    'LblOrderID.Text = GetLocalResourceObject("lbl_Transaction_text").ToString() & oPolicySummary.Reference

                    'If payment method is Instalment then need to show instalment plan reference number
                    If paymentOptions.PaymentType(Session(CNSelectedPaymentIndex)).Type = "PremiumFinance" Then
                        Dim sConfirmationMessage As String = ""
                        If totalPremium >= 0 Then
                            If Not oPayment Is Nothing AndAlso oPayment.InstallmentType = NexusProvider.InstalmentType.AddToNewPlan Then
                                sConfirmationMessage = GetLocalResourceObject("lbl_Transaction_textForInstalment")
                                sConfirmationMessage = sConfirmationMessage.Replace("#InstalmentPlanRef", oPolicySummary.InstalmentPlanRef)
                            Else
                                sConfirmationMessage = GetLocalResourceObject("lbl_Transaction_textForInstalmentAmendment")
                            End If
                            sConfirmationMessage = sConfirmationMessage.Replace("#PolicyNumber", oPolicySummary.Reference)
                            'LblOrderID.Text = sConfirmationMessage
                            'lblMTAPremiumReturn.Text = ""
                        Else
                            sConfirmationMessage = GetLocalResourceObject("lbl_Transaction_textForInstalmentAmendment")
                            sConfirmationMessage = sConfirmationMessage.Replace("#PolicyNumber", oPolicySummary.Reference)
                            'LblOrderID.Text = sConfirmationMessage
                            'lblMTAPremiumReturn.Text = ""
                        End If
                    Else
                        If totalPremium < 0 AndAlso Session(CNMTAType) = MTAType.PERMANENT Then
                            'LblOrderID.Text = ""
                            Dim sConfirmationMessage As String = GetLocalResourceObject("lbl_ReturnPremium_Text")
                            sConfirmationMessage = sConfirmationMessage.Replace("#PolicyNumber", oPolicySummary.Reference)
                            '    lblMTAPremiumReturn.Text = sConfirmationMessage
                            'Else
                            '    'LblOrderID.Text = GetLocalResourceObject("lbl_Transaction_text").ToString() & oPolicySummary.Reference
                        End If
                    End If

                ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
                    oPolicySummary = New NexusProvider.PolicySummary(oQuote.Reference)
                    oPolicySummary = oWebService.BindQuote(oQuote.InsuranceFileKey, oPayment, oQuote.TimeStamp, Nothing, oQuote.BranchCode, "MTC", sBackDatedMTA)
                    'LblOrderID.Text = GetLocalResourceObject("lbl_CancelTransaction_text").ToString() & " " & oPolicySummary.Reference
                    'Dim sConfirmationMessage As String = lblMTAPremiumReturn.Text
                    'lblMTAPremiumReturn.Text = sConfirmationMessage.Replace("#PolicyNumber", oPolicySummary.Reference)
                    'LblOrderID.Text = ""
                ElseIf Session(CNMTAType) = MTAType.REINSTATEMENT Then
                    oPolicySummary = New NexusProvider.PolicySummary(oQuote.Reference)
                    oPolicySummary = oWebService.BindQuote(oQuote.InsuranceFileKey, oPayment, oQuote.TimeStamp, Nothing, oQuote.BranchCode, "MTR", sBackDatedMTA)
                    'LblOrderID.Text = GetLocalResourceObject("lbl_Transaction_text").ToString() & " " & oPolicySummary.Reference
                ElseIf Session(CNRenewal) IsNot Nothing Then
                    oPolicySummary = New NexusProvider.PolicySummary(oQuote.Reference)

                    oPolicySummary = oWebService.BindQuote(oQuote.InsuranceFileKey, oPayment, oQuote.TimeStamp, True, oQuote.BranchCode, "REN", sBackDatedMTA)

                    If paymentOptions.PaymentType(Session(CNSelectedPaymentIndex)).Type.ToString = "PremiumFinance" Then
                        'If payment method is Instalment then need to show instalment plan referance number
                        Dim sConfirmationMessage As String = GetLocalResourceObject("lbl_Transaction_textForInstalment").ToString
                        sConfirmationMessage = sConfirmationMessage.Replace("#PolicyNumber", oPolicySummary.Reference)
                        sConfirmationMessage = sConfirmationMessage.Replace("#InstalmentPlanRef", oPolicySummary.InstalmentPlanRef)
                        'LblOrderID.Text = sConfirmationMessage
                        'Else
                        '    LblOrderID.Text = GetLocalResourceObject("lbl_Transaction_text").ToString() & oPolicySummary.Reference 'oQuote.InsuranceFileRef 'oPolicySummary.Reference
                    End If
                Else
                    If oPayment IsNot Nothing Then
                        oPayment.StartDate = oQuote.CoverStartDate
                    End If
                    oPolicySummary = oWebService.BindQuote(oQuote.InsuranceFileKey, oPayment, oQuote.TimeStamp, Nothing, oQuote.BranchCode, "NB")
                    If paymentOptions.PaymentType(Session(CNSelectedPaymentIndex)).Type = "PremiumFinance" Then
                        'If payment method is Instalment then need to show instalment plan referance number
                        Dim sConfirmationMessage As String = GetLocalResourceObject("lbl_Transaction_textForInstalment")
                        sConfirmationMessage = sConfirmationMessage.Replace("#PolicyNumber", oPolicySummary.Reference)
                        sConfirmationMessage = sConfirmationMessage.Replace("#InstalmentPlanRef", oPolicySummary.InstalmentPlanRef)
                        'LblOrderID.Text = sConfirmationMessage
                        'Else
                        '    LblOrderID.Text = GetLocalResourceObject("lbl_Transaction_text").ToString() & oPolicySummary.Reference 'oQuote.InsuranceFileRef 'oPolicySummary.Reference
                    End If
                End If
                If oPayment IsNot Nothing Then
                    If oPayment.PayNowPaymentDetails IsNot Nothing AndAlso oPayment.PayNowPaymentDetails.MediaTypeCode IsNot Nothing Then
                        oPolicySummary.MediaTypeCode = oPayment.PayNowPaymentDetails.MediaTypeCode
                    Else
                        oPolicySummary.MediaTypeCode = ""
                    End If
                End If
                Session.Item(CNPolicy_Summary) = oPolicySummary

                'NIA WPR 09 - create cashlist for the credit card deposit amount 
                Dim paymentTypes As New Config.PaymentTypes

                paymentTypes = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID).PaymentTypes

                Dim PaymentCollectionUrl As String = paymentTypes.PaymentType(Session(CNSelectedPaymentIndex)).PaymentCollectionUrl

                If oQuote.InstDepositAmount > 0 AndAlso oPolicySummary IsNot Nothing AndAlso oQuote.DepositTransactasInstalment = False AndAlso PaymentCollectionUrl <> "" _
                            AndAlso Request.QueryString("Mode") = "INSDEPOSIT" Then
                    DoInstalmentDeposit()
                End If

                oQuote.InsuranceFileRef = oPolicySummary.Reference

                'MOSS 796
                If Not Session(CNWMTaskInstanceKey) Is Nothing Then
                    Dim oWorkManager As New NexusProvider.WorkManager

                    oWorkManager.TaskInstanceKey = Session(CNWMTaskInstanceKey)
                    oWebService.GetWmTask(oWorkManager)
                    oWorkManager.Client = oQuote.ClientCode
                    oWorkManager.WmActionType = NexusProvider.WMActionType.Complete
                    oWorkManager.TaskStatusKey = NexusProvider.TaskStatus.Complete
                    oWorkManager.TaskTimeStamp = oWorkManager.TimeStamp
                    oWebService.UpdateWmTask(oWorkManager)
                End If


                Session.Remove(CNAmountToPay)
                Session.Remove(CNPayment)
                Session.Remove(CNOI)
                Session.Remove(CNMode)
                Session.Remove(CNPaid)
                Session.Remove(CNRiskType)
                Session(CNIsTransactionConfirmationVisited) = True
            Catch ex As Exception

                Throw ex
                'End If

            Finally
                oWebService = Nothing
                oPolicySummary = Nothing
            End Try
            'End If

            progress = 95
            'Else
            '    oPolicySummary = Session.Item(CNPolicy_Summary)
            '    'If oPolicySummary IsNot Nothing Then
            '    '    If Session(CNMTAType) = MTAType.PERMANENT Or Session(CNMTAType) = MTAType.TEMPORARY Then
            '    '        LblOrderID.Text = GetLocalResourceObject("lbl_Transaction_text").ToString() & oPolicySummary.Reference
            '    '    ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
            '    '        LblOrderID.Text = GetLocalResourceObject("lbl_CancelTransaction_text").Replace("#PolicyNumber", oPolicySummary.Reference)
            '    '        ' If Session(oPolicySummary.MediaTypeCode) IsNot Nothing Then
            '    '        Select Case UCase(oPolicySummary.MediaTypeCode)
            '    '            Case "CA"
            '    '                LblOrderID.Text += " Cash"
            '    '            Case "CC"
            '    '                LblOrderID.Text += " Credit Card"
            '    '            Case "BD"
            '    '                LblOrderID.Text += " Bankers Draft"
            '    '            Case "DD"
            '    '                LblOrderID.Text += " Direct Debit"
            '    '            Case "CQ"
            '    '                LblOrderID.Text += " Cheque"
            '    '            Case Else
            '    '                LblOrderID.Text += " Cheque"
            '    '        End Select
            '    '        'Else
            '    '        '    LblOrderID.Text += " Cheque"
            '    '        'End If


            '    '    ElseIf Session(CNRenewal) IsNot Nothing Then
            '    '        If paymentOptions.PaymentType(Session(CNSelectedPaymentIndex)).Type.ToString = "PremiumFinance" Then
            '    '            'If payment method is Instalment then need to show instalment plan referance number
            '    '            Dim sConfirmationMessage As String = GetLocalResourceObject("lbl_Transaction_textForInstalment").ToString
            '    '            sConfirmationMessage = sConfirmationMessage.Replace("#PolicyNumber", oPolicySummary.Reference)
            '    '            sConfirmationMessage = sConfirmationMessage.Replace("#InstalmentPlanRef", oPolicySummary.InstalmentPlanRef)
            '    '            LblOrderID.Text = sConfirmationMessage
            '    '        Else
            '    '            LblOrderID.Text = GetLocalResourceObject("lbl_Transaction_text").ToString() & oPolicySummary.Reference
            '    '        End If
            '    '    Else
            '    '        LblOrderID.Text = GetLocalResourceObject("lbl_Transaction_text").ToString() & oPolicySummary.Reference 'oQuote.InsuranceFileRef 'oPolicySummary.Reference
            '    '    End If
            '    'ElseIf ((Session(CNMTAType) = MTAType.PERMANENT OrElse Session(CNMTAType) = MTAType.TEMPORARY OrElse
            '    '         Session(CNMTAType) = MTAType.REINSTATEMENT) And oPolicySummary Is Nothing) Then
            '    '    LblOrderID.Text = GetLocalResourceObject("lbl_CancelMTATransaction_text").ToString()

            '    'ElseIf Session(CNMTAType) = MTAType.CANCELLATION And oPolicySummary Is Nothing Then
            '    '    LblOrderID.Text = GetLocalResourceObject("lbl_CancelMTAQCANTransaction_text").ToString()

            '    'Else
            '    '    LblOrderID.Text = GetLocalResourceObject("lbl_errorBindQuote").ToString()
            '    'End If
            '    Session(CNQuote) = oQuote
            'End If

            ''[start]changes for WPR 73_74
            ''If an underwriter (non-agency user) is logged
            Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
            Dim desc As String = String.Empty
            Dim task As String = String.Empty
            Dim taskGroup As String = String.Empty

            If oUserDetails IsNot Nothing AndAlso oUserDetails.Key = 0 AndAlso oQuote.ContactUserName <> "" Then
                If (Session(CNQuoteMode) = QuoteMode.FullQuote AndAlso Session(CNRenewal) Is Nothing AndAlso Session(CNMTAType) <> MTAType.CANCELLATION AndAlso Session(CNMTAType) <> MTAType.REINSTATEMENT) Then 'If NB
                    task = "UNDERNB"
                    taskGroup = "UNDER"
                    'desc = IIf(GetLocalResourceObject("lblTaskNB") Is Nothing, "Your Quote with Ref No. XXXXX is Live", GetLocalResourceObject("lblTaskNB"))
                ElseIf (Session(CNQuoteMode) = QuoteMode.MTAQuote) Then 'If MTA
                    task = "UNDERMTA"
                    taskGroup = "UNDER"
                    'desc = IIf(GetLocalResourceObject("lblTaskMTA") Is Nothing, "Your Quote with Ref No. XXXXX is Live", GetLocalResourceObject("lblTaskMTA"))
                ElseIf (Session(CNMTAType) = MTAType.REINSTATEMENT) Then 'If MTR
                    task = "UNDERREINS"
                    taskGroup = "UNDER"
                    'desc = IIf(GetLocalResourceObject("lblTaskREIN") Is Nothing, "Your Quote with Ref No. XXXXX is reinstated", GetLocalResourceObject("lblTaskREIN"))
                    '[start]New code added against the issue 1411
                ElseIf Session(CNRenewal) IsNot Nothing Then
                    task = "RENACCWAM"
                    taskGroup = "UWRENEWAL"
                    'desc = IIf(GetLocalResourceObject("lblTaskRenewalAcceptance") Is Nothing, "Your renewal Quote with Ref No. XXXXX is Accepted", GetLocalResourceObject("lblTaskRenewalAcceptance"))
                    'desc = desc.Replace("XXXXX", oQuote.InsuranceFileRef)
                    '[end]New code added against the issue 1411
                End If
                'desc = desc.Replace("XXXXX", oQuote.InsuranceFileRef)
                ' GoTo gohere
                If (Not (String.IsNullOrEmpty(desc) Or String.IsNullOrEmpty(task) Or String.IsNullOrEmpty(taskGroup))) Then
                    FrameWorkFunctions.CreateTask(oQuote, desc, task, taskGroup)
                End If

                'SendMail()
            End If
            '[end]changes for WPR 73_74
            Session.Remove(CNIsCancelMTA)
        End Sub

        Protected Sub ReplicateStatements()
            progress = 85
            UpdatePremiumWithAgentCommision()
            CheckPremiumAndRedirect()

        End Sub

        'Process Buy
        '
        '
        Protected Sub BuyPolicy()
            Dim selectedPaymentOption As String = String.Empty
            Dim selectedPaymentIndex As Object
            Dim crSelectedPaymentValue As Decimal = Nothing
            Dim crOutstandingAmount As Decimal = Nothing
            Dim oWebService As NexusProvider.SAMForInsurance.ProviderSAMForInsuranceV2 = New NexusProvider.SAMForInsurance.ProviderSAMForInsuranceV2()

            Try


                ''First payment method will be defaulted when SkipPaymentSelect property is true
                If CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).SkipPaymentSelect = True Then
                    selectedPaymentIndex = Session(CNSelectedPaymentIndex)
                    'Else
                    '    'ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "NoPaymentMethodSelected", "alert('" + GetLocalResourceObject("msg_NoPaymentMethodAvailable") + "');", True)
                    '    Exit Sub
                End If

                'Session(CNSelectedPaymentIndex) = selectedPaymentIndex

                'CHECK FOR SELECTED VALUE AND REDIRECT THE PAGE ACCORDINGLY.
                Dim oPaymentOptions As Config.PaymentTypes = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).PaymentTypes
                Dim oPaymentType As Config.PaymentType = oPaymentOptions.PaymentType(Session(CNSelectedPaymentIndex))

                'If Page.IsValid AndAlso Session(CNMode) <> Mode.View AndAlso oPaymentOptions.PaymentType(Session(CNSelectedPaymentIndex)).Type = "PremiumFinance" Then
                '    Dim grdInstalments As GridView = Instalments.FindControl("grdInstallmentQuotes")
                '    If grdInstalments IsNot Nothing AndAlso grdInstalments.Rows.Count > 0 Then
                '        Instalments.SaveInstallmentPlan()
                '    Else
                '        vldInstalmentSchemes.IsValid = False
                '        Exit Sub
                '    End If
                'End If

                'If Session(CNCommissionGreaterThanPremium) IsNot Nothing AndAlso Session(CNCommissionGreaterThanPremium) = True Then
                '    vldCommissionGreaterThanPremium.Enabled = True
                '    vldCommissionGreaterThanPremium.IsValid = False
                'End If
                'If Page.IsValid Then
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oProductConfig As Config.Product = oNexusConfig.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)

                Dim totalPremium As Decimal
                If oQuote.Risks.Count > 0 Then
                    totalPremium = oQuote.GrossTotal
                    Session(CNAmountToPay) = totalPremium
                    'if Payment method is PAYNOW and return premium
                    If oQuote.PaymentMethod IsNot Nothing AndAlso oQuote.PaymentMethod.ToUpper = "PAYNOW" AndAlso totalPremium < 0 Then
                        oWebService.GetPolicyOutstandingAmount(crOutstandingAmount, oQuote.InsuranceFileKey, oQuote.BranchCode)
                        totalPremium = crOutstandingAmount + oQuote.GrossTotal
                        Session(CNAmountToPay) = totalPremium
                    End If
                End If
                oAmount = CheckAndCalculateRoundOff()
                progress = 80
                If CheckRefer() = True Then
                    Session(CNQuoteMode) = QuoteMode.FullQuote
                    Response.Redirect(AppSettings("WebRoot") & "referred.aspx")
                ElseIf CheckDecline() = True Then
                    Session(CNQuoteMode) = QuoteMode.FullQuote
                    Response.Redirect(AppSettings("WebRoot") & "declined.aspx")
                ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
                    If oPaymentType IsNot Nothing Then
                        If oPaymentType.Name.Trim.ToUpper = "PAYNOW" Then
                            Session.Remove(CNCashListItem) 'Loads Cash List screen when PayNow option selection
                            Session.Remove(CNQuoteMode)
                            Response.Redirect(oPaymentType.Url, False)
                        Else
                            Session(CNPaid) = True
                            ReplicateTransactions()
                        End If
                    Else
                        ReplicateTransactions()
                    End If
                Else

                    If Session(CNRenewal) Is Nothing AndAlso Session(CNMTAType) IsNot Nothing AndAlso totalPremium < 0 Then
                        ' Begin - WPR VB 64 - Media Type Status 
                        Dim CheckMediatypeStatusAtPolicyRefund As String = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance,
                                                            NexusProvider.ProductRiskOptions.CheckMediatypeStatusAtPolicyRefund, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing, oQuote.BranchCode).Trim()

                        If CheckMediatypeStatusAtPolicyRefund.Contains("1") Then
                            Dim oMediaTypeStatus As New NexusProvider.MediaTypeStatus
                            With oMediaTypeStatus
                                .InsuranceFileKey = oQuote.InsuranceFileKey
                                .LossDateSpecified = False
                            End With
                            oWebService.GetPolicyStatusForMediaTypeStatus(oMediaTypeStatus)
                            'SAM Return the False intead of True, if unclear fund exist then it retirn False or else true
                            'If Not oMediaTypeStatus.IsUnclearedCashListExists Then
                            '    vldMediaTypeStatus.IsValid = False
                            '    Exit Sub

                            'End If

                        End If
                        ' End - WPR VB 64 - Media Type Status 
                    End If

                    If CType(Session(CNIsAnonymous), Boolean) = True Then
                        Session(CNRedirectedFor) = "BuyQuote"
                        'redirecting the user to Find Client Page if Quote is anonymous
                        'If CType(Session.Item(CNLoginType), LoginType) = LoginType.Agent Then
                        '    Response.Redirect("~/secure/agent/FindClient.aspx", False)
                        'End If
                    End If

                    'If hSelectedAccount.Value <> "" Then
                    '    Session(CNSelectedAccount) = hSelectedAccount.Value
                    'End If
                    If (totalPremium < 0.0) And (Session(CNMTAType) <> MTAType.CANCELLATION) Then
                        Session(CNMode) = Mode.Edit
                        Session(CNQuoteInSync) = False
                        Session.Remove(CNOI)
                        'If hSelectedAccount.Value <> "" Then
                        '    Session(CNSelectedAccount) = hSelectedAccount.Value
                        'End If
                        Dim TotalSettlementBalance As Decimal = 0

                        Dim instalmentSettelmentAmount As Decimal
                        Dim transactionType As String = ""
                        If Session(CNRenewal) IsNot Nothing Then
                            transactionType = "REN"
                        ElseIf Session(CNMTAType) IsNot Nothing Then
                            transactionType = "MTA"
                        Else
                            transactionType = "NB"
                        End If

                        oWebService.GetInstalmentSettlementAmount(oQuote.InsuranceFileKey, instalmentSettelmentAmount, transactionType, oQuote.BranchCode)
                        TotalSettlementBalance = instalmentSettelmentAmount + totalPremium

                        'this will check in case of MTA Return Premium exists
                        ' which will check statements is set to true in web.config and then will redirect to staements page
                        If oQuote.PaymentMethod.Trim.ToUpper = "DIRECT DEBIT" AndAlso TotalSettlementBalance < 0 Then
                            oQuote.PaymentMethod = "Invoice"
                            Session.Remove(CNSelectedPaymentIndex)
                            Session(CNPaid) = True
                            ReplicateTransactions()
                        ElseIf CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).ShowStatements = True OrElse oQuote.PaymentMethod.Trim.ToUpper = "DIRECT DEBIT" Then
                            ReplicateStatements()
                            'else will redirect to transaction confirmation page directly
                        Else
                            Session(CNPaid) = True
                            ReplicateTransactions()
                            'End If
                        End If
                    End If

                    '  End If

                    'This will allow Zero Premium to be transacted in Case of NB/MTA/Renewals
                    If (totalPremium = 0.0) Then
                        Session(CNMode) = Mode.Edit
                        Session(CNQuoteInSync) = False
                        Session.Remove(CNOI)
                        If CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).ShowStatements = True Then
                            ReplicateStatements()
                        Else
                            Session(CNPaid) = True
                            ReplicateTransactions()
                        End If
                    End If

                    ReplicateStatements()
                End If
            Catch ex As System.Exception
                Throw
            End Try
            'End If
        End Sub


        'below method silent create cashlist for the credit card deposit amount
        Protected Sub DoInstalmentDeposit()
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oReceiptCashListItem As New NexusProvider.ReceiptCashListItemType
            Dim oCashListItem As New NexusProvider.PaymentItems
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim branchCode As String = oQuote.BranchCode
            Dim oPolicySummary As NexusProvider.PolicySummary = CType(Session.Item(CNPolicy_Summary), NexusProvider.PolicySummary)
            Dim mediaTypeId As Integer
            Dim mediaTypeCode As String
            Dim bankAccountId As Integer
            Dim bankAccountCode As String
            Dim oBaseParty As New NexusProvider.BaseParty
            oBaseParty = oWebservice.GetParty(oQuote.PartyKey)
            oWebservice = New NexusProvider.ProviderManager().Provider
            Dim oAccountSearchCr As New NexusProvider.AccountSearchCriteria
            Dim oAccountColl As NexusProvider.AccountSearchResultCollection
            'Dim oPayment As NexusProvider.Payment
            If oQuote.BusinessTypeCode = "DIRECT" Then 'Direct Business/Customer
                oAccountSearchCr.ShortCode = oQuote.ClientCode
            Else
                oAccountSearchCr.ShortCode = oQuote.AgentCode
            End If
            Try

                oAccountColl = oWebservice.FindAccounts(oAccountSearchCr)
                GetBankAccountDefault(mediaTypeId, bankAccountId, 2)

                If bankAccountId > 0 Then
                    mediaTypeCode = GetCodeForKey(NexusProvider.ListType.PMLookup, mediaTypeId, "MediaType", True)
                    bankAccountCode = GetCodeForKey(NexusProvider.ListType.PMLookup, bankAccountId, "BankAccount", True)
                Else
                    Exit Sub
                End If


                Dim oReceiptCashListItems As NexusProvider.ReceiptCashListItemType = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType)

                ''change value based on data received from session
                With oReceiptCashListItem.CoreCashList
                    .BankAccountCode = bankAccountCode
                    .CurrencyCode = oQuote.CurrencyCode
                    .ListDate = Today.Date
                    .TypeCode = "R"
                    .StatusCode = "E"
                End With

                oCashListItem.AccountShortCode = oReceiptCashListItems.ReceiptItems(0).AccountShortCode
                oCashListItem.Amount = oReceiptCashListItems.ReceiptItems(0).Amount
                oCashListItem.StatusCode = "ADD"
                oCashListItem.AllocationStatusCode = oReceiptCashListItems.ReceiptItems(0).AllocationStatusCode
                oCashListItem.Address = oReceiptCashListItems.ReceiptItems(0).Address
                oCashListItem.OurReference = ""
                oCashListItem.TransactionDate = oReceiptCashListItems.ReceiptItems(0).TransactionDate
                oCashListItem.MediaTypeCode = oReceiptCashListItems.ReceiptItems(0).MediaTypeCode
                oCashListItem.MediaReference = oPolicySummary.Reference + "INSDEPOSIT"
                oCashListItem.TypeCode = oReceiptCashListItems.ReceiptItems(0).TypeCode
                Dim oPayment As NexusProvider.Payment = CType(Session(CNPayment), NexusProvider.Payment)

                If mediaTypeCode.Trim() = "CC" AndAlso oPayment.CreditCard IsNot Nothing Then
                    Dim oCreditCard As New NexusProvider.CreditCard
                    oCreditCard.Number = oPayment.CreditCard.Number
                    oCreditCard.AuthCode = oPayment.CreditCard.AuthCode
                    oCashListItem.CreditCard = oCreditCard
                End If
                oReceiptCashListItem.TransactionDate = oReceiptCashListItems.ReceiptItems(0).TransactionDate
                oReceiptCashListItem.ReceiptItems.Add(oCashListItem)
                Dim oReceiptCashListCollection As New NexusProvider.ReceiptCashListCollection
                Try
                    oReceiptCashListCollection = oWebservice.CreateReceiptcashListWithItem(oReceiptCashListItem)

                    Dim oTransAllocationDetails As New NexusProvider.AllocationDetails
                    Dim oAllocation As NexusProvider.Allocation
                    Dim oAccountDetails As New NexusProvider.AccountDetails
                    Dim oAccountDetailsDefaults As New NexusProvider.AccountDetailsDefaults
                    Dim oAllocationDetailsCollections As New NexusProvider.AllocationDetailsCollections
                    Dim oAllocationDetails As New NexusProvider.AllocationDetails
                    Dim oTrasactionDetails As New NexusProvider.AllocationDetailsCollections
                    Dim sJN_Number, sSPR_Number As String

                    'Assignment of the Transdetails Key
                    oAllocationDetails = New NexusProvider.AllocationDetails
                    oAllocationDetails.TransdetailKey = oPolicySummary.InstdepositTransDetailsId
                    oAllocationDetailsCollections.Add(oAllocationDetails)
                    oAllocationDetails = Nothing
                    oAllocationDetails = New NexusProvider.AllocationDetails

                    oAllocationDetails.TransdetailKey = oReceiptCashListCollection(0).TransDetailKey
                    oAllocationDetailsCollections.Add(oAllocationDetails)
                    oTrasactionDetails = oWebservice.GetTransactionDetails(oAccountColl(0).AccountKey, oAllocationDetailsCollections)

                    For Each oTempAllocationDetails As NexusProvider.AllocationDetails In oTrasactionDetails
                        If oTempAllocationDetails.TransdetailKey = oPolicySummary.InstdepositTransDetailsId Then
                            oAllocation = New NexusProvider.Allocation
                            oAllocation.AllocationAmount = oTempAllocationDetails.Amount
                            oAllocation.AllocationTimeStamp = oTempAllocationDetails.AllocationTimeStamp

                            oAllocation.AllocationTransdetailKey = oPolicySummary.InstdepositTransDetailsId
                            oTransAllocationDetails.Allocation.Add(oAllocation)
                            oAllocation = Nothing
                            sJN_Number = oTempAllocationDetails.DocRef
                        ElseIf oTempAllocationDetails.TransdetailKey = oReceiptCashListCollection(0).TransDetailKey Then
                            sSPR_Number = oTempAllocationDetails.DocRef 'SRP
                        End If
                    Next

                    oTransAllocationDetails.AccountKey = oAccountColl(0).AccountKey
                    oTransAllocationDetails.CashListItemKey = oReceiptCashListCollection(0).CashListItemKey
                    oTransAllocationDetails.Amount = -oQuote.InstDepositAmount
                    oTransAllocationDetails.TransdetailKey = oReceiptCashListCollection(0).TransDetailKey
                    oTransAllocationDetails.Currency = oQuote.CurrencyCode
                    Try
                        Dim isUpdated As Boolean = oWebservice.UpdateAllocation(oTransAllocationDetails)
                    Catch
                        CreateTask(CType(Session(CNQuote), NexusProvider.Quote), Session("NABcrn").ToString() _
                                & "- Receipt has been generated but Allocation of Receipt " & sSPR_Number & " has failed against Journal " & sJN_Number & ", needs to be done manually", "FINDTXN", "SLACS", GetAccountHandlerTaskGroup())

                    End Try

                Catch
                    CreateTask(CType(Session(CNQuote), NexusProvider.Quote), oPolicySummary.Reference _
                            & " - Policy has been made live but CashList Receipt has failed, needs to be generated manually", "ACTRCTV2", "SLACS", GetAccountHandlerTaskGroup())
                    oReceiptCashListCollection = Nothing
                End Try
            Catch ex As System.Exception
                Throw
            Finally
                oWebservice = Nothing
                oPolicySummary = Nothing
                oQuote = Nothing

            End Try
        End Sub


        Private Function GetAccountHandlerTaskGroup() As String
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim sReturnCode As NexusProvider.OptionTypeSetting
            Dim taskGroup As String = ""
            Try
                sReturnCode = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5042)
                taskGroup = sReturnCode.OptionValue
            Catch ex As System.Exception
                Throw
            Finally
                sReturnCode = Nothing
                oWebService = Nothing
            End Try
            Return taskGroup.Trim()
        End Function

        Sub CheckPremiumAndRedirect()
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim tatalPremium As Decimal
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim isPrepaymentOptionEnabled As String
            isPrepaymentOptionEnabled = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsPrepaymentOptionEnabled, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
            Try
                If oQuote.Risks.Count > 0 Then
                    tatalPremium = Session(CNAmountToPay)
                End If
                If tatalPremium <= 0.0 AndAlso Session(CNMTAType) IsNot Nothing Then
                    'In case of MTA
                    If tatalPremium <= 0.0 AndAlso Session(CNMTAType) = MTAType.CANCELLATION Then
                        'if this is Refund Premium or Zero premium and PrePayment = 0 then go to directly TransactionConfirmation page
                        'If sPrePaymentOption Is Nothing Or sPrePaymentOption = "0" Then
                        'During MTA Cancellation now the client needs the payment method selection screen.
                        If isPrepaymentOptionEnabled Is Nothing Or isPrepaymentOptionEnabled = "0" Then
                            Session(CNStatementsAgreed) = True
                            Session(CNPaid) = True
                            ReplicateTransactions()
                            'Else
                            '    'this will simply redirect to the PrePayment page in case for Refund Premium.
                            '    'if this is Refund Premium and PrePayment = 1 then go to PrePayment page and select account
                            '    Response.Redirect("~/secure/payment/PrePayment.aspx", False)
                        End If
                    ElseIf Not Session(CNMTAType) Is Nothing And tatalPremium = 0.0 Then
                        'this will simply redirect to the Transaction Confirmation in case when there is Return Premium
                        'Or Premium equal to Zero in case of MTA Permanent
                        Session(CNPaid) = True
                        ReplicateTransactions()
                    ElseIf Not Session(CNMTAType) Is Nothing And tatalPremium < 0.0 AndAlso oQuote.PaymentMethod.Trim.ToUpper <> "DIRECT DEBIT" AndAlso oQuote.PaymentMethod.Trim.ToUpper <> "PAYNOW" Then
                        'if this is Refund Premium and PrePayment = 0 then go to directly TransactionConfirmation page
                        If String.IsNullOrEmpty(isPrepaymentOptionEnabled) Or isPrepaymentOptionEnabled = "0" Then
                            Session(CNPaid) = True
                            ReplicateTransactions()
                            'ElseIf isPrepaymentOptionEnabled = "1" Then
                            '    'this will simply redirect to the PrePayment page in case for Refund Premium.
                            '    'if this is Refund Premium and PrePayment = 1 then go to PrePayment page and select account

                            '    Response.Redirect("~/secure/payment/PrePayment.aspx", False)
                        End If
                    Else
                        'if premium is in positive
                        Session(CNStatementsAgreed) = True
                        ReplicateTransactions()
                    End If
                Else
                    'in case of NB/Renewal
                    Session(CNStatementsAgreed) = True
                    ReplicateTransactions()
                End If
                progress = 90
            Catch ex As System.Exception
                Throw
            End Try
        End Sub




        Protected Sub RenewalSelection()
            Dim oInsuranceFileDetailsCollection As NexusProvider.InsuranceFileDetailsCollection
            oInsuranceFileDetailsCollection = Nothing
            Dim nInsuranceFolderKey As Integer
            Dim nInsuranceFileKey As Integer
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            If oInsuranceFileDetailsCollection Is Nothing Then
                'Get search results by calling FindPolicy

                Dim sInsuranceRef As String = IIf(String.IsNullOrEmpty(Request.QueryString("InsuranceFileRef")), Nothing, Request.QueryString("InsuranceFileRef"))
                'Dim sRiskIndex As String = IIf(String.IsNullOrEmpty(txtRiskIndex.Text), Nothing, txtRiskIndex.Text)
                'Dim sClientShortName As String = IIf(String.IsNullOrEmpty(txtClient.Text), Nothing, txtClient.Text)
                Dim sQuoteType As NexusProvider.InsuranceFileType
                Dim iMaxRowsToFetch As Integer = oPortal.MaxSearchResults

                sQuoteType = NexusProvider.InsuranceFileTypes.POLICY

                oInsuranceFileDetailsCollection = oWebService.FindPolicy(sInsuranceRef, Nothing, Nothing, sQuoteType, False, iMaxRowsToFetch, Nothing)


                If oInsuranceFileDetailsCollection IsNot Nothing Then
                    'add the results to the cache so that we don't need to call FindPolicy again
                    'todo - cache length should be taken from config
                    'Cache.Insert(ViewState("pageCacheID"), oInsuranceFileDetailsCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
                    nInsuranceFileKey = oInsuranceFileDetailsCollection(0).InsuranceFileKey
                    nInsuranceFolderKey = oInsuranceFileDetailsCollection(0).InsuranceFolderKey
                    productCode = oInsuranceFileDetailsCollection(0).ProductCode

                    oInsuranceFileKey = nInsuranceFileKey
                    oInsuranceFolderKey = nInsuranceFolderKey
                    'validate size of dataset. if 500(configured at portal level) or more results are returned then add a validation message to the screen
                    'If oInsuranceFileDetailsCollection.Count >= oPortal.MaxSearchResults Then
                    '    'create a custom validator
                    '    Dim cstMaxResults As New CustomValidator
                    '    cstMaxResults.IsValid = False
                    '    'look for a validation message in the page resources, but if there is not one defined add a default message
                    '    cstMaxResults.ErrorMessage = IIf(GetLocalResourceObject("cstMaxResults") Is Nothing, "Maximum number of search results exceeded, please refine your search criteria", GetLocalResourceObject("cstMaxResults"))
                    '    cstMaxResults.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                    '    'add the validator to the page, this will have the effect of making the page invalid
                    '    Page.Validators.Add(cstMaxResults)
                    'End If

                End If
            End If

            progress = 5
            'Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As New NexusProvider.Quote
            Dim oPortalConfig As Config.Portal = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID())
            Dim oPolCol As New NexusProvider.PolicyCollection
            'PnlMessage.Visible = False
            Dim exclusiveLock As Boolean = True
            Try
                'Dim nInsuranceFolderKey As Integer
                'Dim nInsuranceFileKey As Integer
                Dim userName As String

                'Dim GridRow As GridViewRow
                'If Not LCase(e.CommandName).Equals("page") And Not LCase(e.CommandName).Equals("sort") Then
                '    GridRow = CType((e.CommandSource).NamingContainer, GridViewRow)
                '    Dim lblInsuranceFolderKey As Label = GridRow.FindControl("lblInsuranceFolderKey")
                '    nInsuranceFolderKey = CInt(lblInsuranceFolderKey.Text)
                '    Dim lblInsuranceFileKey As Label = GridRow.FindControl("lblInsuranceFileKey")
                '    nInsuranceFileKey = CInt(oInsuranceFileDetailsCollection)

                'End If

                'oQuote.InsuranceFileKey = e.CommandArgument
                oQuote = oWebService.GetHeaderAndSummariesByKey(v_iInsuranceFileKey:=nInsuranceFileKey, bExclusiveLock:=True)

                Dim oPreRenStatus As NexusProvider.RenewalStatus
                oPreRenStatus = oWebService.GetRenewalStatus(nInsuranceFileKey)
                If Not oPreRenStatus Is Nothing And Not oPreRenStatus.RenewalStatusTypeDescription Is Nothing Then
                    If oPreRenStatus.RenewalStatusTypeDescription.ToString <> "" Then
                        If oQuote Is Nothing Then
                            oQuote = oWebService.GetHeaderAndSummariesByKey(v_iInsuranceFileKey:=oQuote.InsuranceFileKey)
                        End If

                        oWebService.DeleteRenewal(oQuote, oQuote.BranchCode)
                        'Dim oInsuranceFileDetailsCollection As NexusProvider.InsuranceFileDetailsCollection
                        oInsuranceFileDetailsCollection = oWebService.FindPolicy(oQuote.InsuranceFileRef, "", "", NexusProvider.InsuranceFileTypes.POLICY, False)
                        For Each oInsuranceFileDetails As NexusProvider.InsuranceFileDetails In oInsuranceFileDetailsCollection
                            oQuote.InsuranceFileKey = oInsuranceFileDetails.InsuranceFileKey
                            Exit For
                        Next
                    End If
                Else
                    'Deletion of all previous version of the Renewal
                    oQuote.InsuranceFileKey = nInsuranceFileKey
                    If oQuote Is Nothing Then
                        oQuote = oWebService.GetHeaderAndSummariesByKey(v_iInsuranceFileKey:=oQuote.InsuranceFileKey)
                    End If
                    'Dim oInsuranceFileDetailsCollection As NexusProvider.InsuranceFileDetailsCollection
                    oInsuranceFileDetailsCollection = oWebService.FindPolicy(oQuote.InsuranceFileRef, "", "", NexusProvider.InsuranceFileTypes.RENEWAL, False)
                    If oInsuranceFileDetailsCollection IsNot Nothing Then
                        For Each oInsuranceFileDetails As NexusProvider.InsuranceFileDetails In oInsuranceFileDetailsCollection
                            Dim oTempQuote As New NexusProvider.Quote
                            oTempQuote = oWebService.GetHeaderAndSummariesByKey(oInsuranceFileDetails.InsuranceFileKey, , bExclusiveLock:=True)
                            If oTempQuote.CoverStartDate = oQuote.RenewalDate Then
                                oWebService.DeleteRenewal(oTempQuote, oTempQuote.BranchCode)
                            End If
                        Next
                    End If
                    oInsuranceFileDetailsCollection = oWebService.FindPolicy(oQuote.InsuranceFileRef, "", "", NexusProvider.InsuranceFileTypes.POLICY, False)
                    For Each oInsuranceFileDetails As NexusProvider.InsuranceFileDetails In oInsuranceFileDetailsCollection
                        oQuote.InsuranceFileKey = oInsuranceFileDetails.InsuranceFileKey
                        oQuote.Reference = oInsuranceFileDetails.InsuranceRef
                        Exit For
                    Next
                End If
                progress = 20
                oWebService.RunRenewalSelectionByPolicy(oQuote, oQuote.BranchCode)
                Dim oStatus As NexusProvider.RenewalStatus
                oStatus = oWebService.GetRenewalStatus(oQuote.InsuranceFileKey)
                'PnlMessage.Visible = True
                'lblMessage.Text = GetLocalResourceObject("lbl_Message").ToString()
                'lblMessage.Text = Replace(lblMessage.Text, "#PolicyRef", oQuote.Reference.Trim)
                'lblMessage.Text = Replace(lblMessage.Text, "#RenewalStatusDescription", oStatus.RenewalStatusTypeDescription.ToString)
                'BindGrid()
                'unlock the Policy ( if locked)
                Dim oOptionSettings As NexusProvider.OptionTypeSetting
                oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ExclusiveLock) 'Exclusive Lock
                If oOptionSettings.OptionValue = "1" Then
                    FrameWorkFunctions.UnlockPolicy(oQuote.InsuranceFolderKey, Session(CNBranchCode).ToString)
                End If

                ocoverStartDate = oQuote.CoverStartDate
                ocoverEndDate = oQuote.CoverEndDate
                oRenewalDate = oQuote.RenewalDate
                oBillingMethod = IIf((oQuote.PaymentMethod.ToString.ToUpper.Trim() = "DIRECT DEBIT" OrElse oQuote.PaymentMethod.ToString.ToUpper.Trim() = "PREMIUM FINANCE"), "Instalments", oQuote.PaymentMethod)
                oAmount = CheckAndCalculateRoundOff()
                oInsuranceFileKey = oQuote.InsuranceFileKey
                oInsuranceFolderKey = oQuote.InsuranceFolderKey
                oAnversarydate = oQuote.AnniversaryDate
            Catch ex As NexusProvider.NexusException
                'Policy locking error
                Select Case CType(ex.Errors(0), NexusProvider.NexusError).Code
                    Case "200", "1000158" 'Policy Locking
                        'Show policy locking error as alert
                        Dim message As String = "alert('" + Replace(GetLocalResourceObject("lbl_policylocked_error"), "{1}", (ex.Errors(0).Detail.Split(":"))(2) + ".") + "')"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", message, True)
                        Server.ClearError()
                        ClearQuote()
                        Exit Sub
                    Case Else
                        Throw
                End Select
            Finally
                oWebService = Nothing
                oQuote = Nothing
            End Try

        End Sub
        Protected Sub btnRenewedOk_Click(sender As Object, e As EventArgs) Handles btnRenewedOk.Click

        End Sub
        Public Sub SaveFinancePlan()
            Dim oPremiumFinancePlan As New NexusProvider.PremiumFinancePlan
            Dim oPayment As NexusProvider.Payment = Nothing
            Dim oPaymentOptions As Nexus.Library.Config.PaymentTypes = Nothing
            Dim oQuote As NexusProvider.Quote
            Dim oInstalmentQuote As NexusProvider.InstalmentQuote
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim nInsuranceFileKey As Integer = 0

            oQuote = Session(CNQuote)
            If Session(CNRenewal) IsNot Nothing AndAlso oQuote IsNot Nothing Then
                nInsuranceFileKey = oQuote.InsuranceFileKey
                Dim sPaymentMethod As String = Trim(oQuote.PaymentMethod.ToUpper())
                If sPaymentMethod = "INSTALMENT" OrElse sPaymentMethod = "PREMIUMFINANCE" OrElse sPaymentMethod = "INSTALMENTS" OrElse sPaymentMethod = "DIRECT DEBIT" Then
                    bindInstalments()
                    Dim oSelectedInstalmentQuote As NexusProvider.InstalmentQuote = Cache.Item(ViewState("SelectedInstalmentQuoteCacheId"))
                    If oSelectedInstalmentQuote Is Nothing Then
                        oPayment = Session(CNPayment)
                        Exit Sub
                    End If

                    SaveInstallmentPlan()
                    ''Allow Installment saving for Renewal only
                    'oPaymentOptions = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID()).PaymentTypes
                    ''If ViewState("InstalmentQuotesCacheID") IsNot Nothing OrElse Cache.Item(ViewState("InstalmentQuotesCacheID")) IsNot Nothing Then
                    'oInstalmentQuote = Cache.Item(ViewState("SelectedInstalmentQuoteCacheId"))
                    'oPremiumFinancePlan.PFPremFinanceKey = oInstalmentQuote.SchemeNo
                    'oPremiumFinancePlan.PFPremFinanceVersion = oInstalmentQuote.SchemeVersion
                    'End If
                    If Session(CNPayment) IsNot Nothing Then
                        oPayment = Session(CNPayment)
                    End If
                End If


                oWebService.SavePremiumFinanceDetails(oPayment, nInsuranceFileKey, "REN")
            End If
        End Sub

        Public Sub SaveInstallmentPlan()
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oSelectedPartyBankDetail As NexusProvider.Bank = Session(CNPartyBankDetail)
            Dim oSelectedInstalmentQuote As NexusProvider.InstalmentQuote = Cache.Item(ViewState("SelectedInstalmentQuoteCacheId"))

            Dim oPayment As NexusProvider.Payment = Session(CNPayment)
            oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.None, CDec(Session(CNAmountToPay)))
            'Set all required properties for payment object

            oPayment.AmountToFinance = oSelectedInstalmentQuote.TotalAmountInput

            oPayment.PaymentProtection = True
            oPayment.QuoteDate = DateTime.Now
            oPayment.SelectedSchemeNo = oSelectedInstalmentQuote.SchemeNo
            oPayment.SelectedSchemeVersion = oSelectedInstalmentQuote.SchemeVersion

            oPayment.WeekDay = 1 'Default to 1 as no input field available in nexus

            oPayment.Pref_ID = oSelectedInstalmentQuote.PFRF_ID
            If oSelectedInstalmentQuote.FirstInstalmentAlignWithDayInMonth = 1 Then
                oPayment.MonthDay = CType(Session(CNFinancePlan), NexusProvider.FinancePlan).DayOfWeekOrMonth
            Else
                If oSelectedInstalmentQuote.AlignTo = 1 Then
                    oPayment.MonthDay = CType(Session(CNFinancePlan), NexusProvider.FinancePlan).DayOfWeekOrMonth
                Else
                    oPayment.MonthDay = 1
                End If
            End If

            oPayment.PreferredDate = oPreferredDate
            oPayment.StartDate = oQuote.CoverStartDate
            oPayment.EndDate = oQuote.CoverEndDate
            oPayment.DaysDelay = oSelectedInstalmentQuote.DaysDelay
            oPayment.IsUseTransactionCurrency = False

            If Not oSelectedPartyBankDetail Is Nothing Then
                oPayment.PartyBankKey = oSelectedPartyBankDetail.PartyBankKey
                oPayment.BankAccountName = oSelectedPartyBankDetail.AccountHolderName
                oPayment.BankAccountNo = oSelectedPartyBankDetail.AccountNumber
                oPayment.BankAddress = oSelectedPartyBankDetail.PartyBankAddress
                oPayment.BankBranch = oSelectedPartyBankDetail.BankBranch
                oPayment.BankName = oSelectedPartyBankDetail.BankName
                oPayment.BankSortCode = oSelectedPartyBankDetail.BranchCode
                oPayment.BIC = oSelectedPartyBankDetail.BIC
                oPayment.IBAN = oSelectedPartyBankDetail.IBAN
            End If



            'We need to set Bank details for selected account type. 
            'If no account type is selected then bank details will be blank

            oQuote.DepositTransactasInstalment = oSelectedInstalmentQuote.DepositAsInstalment
            oQuote.InstDepositAmount = oSelectedInstalmentQuote.DepositAmount
            Session(CNQuote) = oQuote
            Session(CNPayment) = oPayment
        End Sub

        Public Sub bindInstalments()
            Dim dTotalRiskTaxExcludedFromInstalment As Decimal
            Dim dTotalFeeTaxExcludedFromInstalment As Decimal
            Dim dTotalRiskFeeExcludedFromInstalment As Double
            Dim dAmountToFinance As Double
            Dim dAgentCommission As Double
            Dim dTaxOnAgentCommission As Double
            Dim nPartyBankId As Integer = 0
            Dim dOverrideInterestRate As Double
            Dim dOverrideRate As Double
            Dim bPaymentProtection As Boolean
            Dim dOverrideDepositAmount As Double = Nothing
            Dim bOverrideCommission As Boolean
            Dim oFinancePlanInformation As New NexusProvider.FinancePlanInformation
            If Session(CNAmountToPay) IsNot Nothing AndAlso CType(Session(CNAmountToPay), Double) <> 0.0 Then
                Dim oInstalmentQuotes As NexusProvider.InstallmentQuoteCollection
                Dim oPartyBankDetails As NexusProvider.BankCollection
                Dim oPartyBankDetail As NexusProvider.Bank
                Dim oParty As NexusProvider.BaseParty = CType(Session(CNParty), NexusProvider.BaseParty)
                Dim oPartyBankDetailsForInstalment As New NexusProvider.BankCollection
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oQuote As NexusProvider.Quote
                Dim oQuoteForTax As NexusProvider.Quote
                Dim oQuoteForFees As NexusProvider.Quote
                Dim paymentOptions As New Config.PaymentTypes
                Dim oPayment As New NexusProvider.Payment(NexusProvider.PaymentTypes.None)

                Dim objBasePayment As Nexus.BasePayment
                Dim InstalmentQuotesCacheID As Guid
                Dim PartyBankdetailsCacheID As Guid
                Dim SelectedAccountTypeCacheId As Guid
                Dim SelectedInstalmentQuoteCacheId As Guid
                Dim QuoteForTaxCacheId As Guid
                Dim QuoteForFeesCacheId As Guid
                Dim TotalRiskTaxExcludedFromInstalmentCacheId As Guid
                Dim TotalFeeTaxExcludedFromInstalmentCacheId As Guid
                Dim TotalRiskFeeExcludedFromInstalmentCacheId As Guid
                Dim AgentCommissionCacheId As Guid
                Dim TaxOnAgentCommissionCacheId As Guid

                Dim Partybankidcache As Guid
                'Generata unique cache id for storing different values and collections in cache
                InstalmentQuotesCacheID = Guid.NewGuid()
                PartyBankdetailsCacheID = Guid.NewGuid()
                SelectedAccountTypeCacheId = Guid.NewGuid()
                SelectedInstalmentQuoteCacheId = Guid.NewGuid()
                QuoteForTaxCacheId = Guid.NewGuid()
                QuoteForFeesCacheId = Guid.NewGuid()
                TotalRiskTaxExcludedFromInstalmentCacheId = Guid.NewGuid()
                TotalFeeTaxExcludedFromInstalmentCacheId = Guid.NewGuid()
                TotalRiskFeeExcludedFromInstalmentCacheId = Guid.NewGuid()
                AgentCommissionCacheId = Guid.NewGuid()
                TaxOnAgentCommissionCacheId = Guid.NewGuid()
                Partybankidcache = New Guid()

                ViewState.Add("Partybankidcache", Partybankidcache.ToString)
                ViewState.Add("InstalmentQuotesCacheID", InstalmentQuotesCacheID.ToString)
                ViewState.Add("PartyBankdetailsCacheID", PartyBankdetailsCacheID.ToString)
                ViewState.Add("SelectedAccountTypeCacheId", SelectedAccountTypeCacheId.ToString)
                ViewState.Add("SelectedInstalmentQuoteCacheId", SelectedInstalmentQuoteCacheId.ToString)

                ViewState.Add("QuoteForTaxCacheId", QuoteForTaxCacheId.ToString)
                ViewState.Add("QuoteForFeesCacheId", QuoteForFeesCacheId.ToString)
                ViewState.Add("TotalRiskTaxExcludedFromInstalmentCacheId", TotalRiskTaxExcludedFromInstalmentCacheId.ToString())
                ViewState.Add("TotalFeeTaxExcludedFromInstalmentCacheId", TotalFeeTaxExcludedFromInstalmentCacheId.ToString())
                ViewState.Add("TotalRiskFeeExcludedFromInstalmentCacheId", TotalRiskFeeExcludedFromInstalmentCacheId.ToString())

                ViewState.Add("AgentCommissionCacheId", AgentCommissionCacheId.ToString())
                ViewState.Add("TaxOnAgentCommissionCacheId", TaxOnAgentCommissionCacheId.ToString())

                Try
                    'Create oQuote object from session
                    oQuote = Session(CNQuote)

                    'HERE WE PASS THE INCEPTION DATE AS CURRENT DATE
                    oQuote.InceptionDate = Date.Today

                    If Session(CNPFUseTransCurrency) = 0 Then
                        CheckUseTransCurrency(oQuote.BaseCurrencyID, oQuote.TransCurrencyID)
                        Session(CNPFUseTransCurrency) = 1
                    End If

                    'To get exact tax and fees, wee need to call below given SAM functions
                    oQuoteForTax = oWebService.GetHeaderAndPolicyTaxByKey(oQuote.InsuranceFileKey, oQuote.BranchCode)
                    Cache.Insert(ViewState("QuoteForTaxCacheId"), oQuoteForTax, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
                    oQuoteForFees = oWebService.GetHeaderAndPolicyFeesByKey(oQuote.InsuranceFileKey, oQuote.BranchCode)
                    Cache.Insert(ViewState("QuoteForFeesCacheId"), oQuoteForFees, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

                    'Get total tax excluded from instalment for all risks
                    For iCt As Integer = 0 To oQuote.Risks.Count - 1
                        Dim oHeaderandRisk As NexusProvider.HeaderAndRisk
                        oHeaderandRisk = oWebService.GetHeaderAndRiskFeesByKey(oQuote.InsuranceFileKey, oQuote.Risks(iCt).Key)
                        For Each oRiskFee As NexusProvider.Fee In oHeaderandRisk.RiskFees
                            If oRiskFee.IncludeInInstallment = 0 Then
                                dTotalFeeTaxExcludedFromInstalment = dTotalFeeTaxExcludedFromInstalment + oRiskFee.TaxAmount
                                dTotalRiskFeeExcludedFromInstalment = dTotalRiskFeeExcludedFromInstalment + oRiskFee.FeeAmount
                            End If
                        Next
                        oHeaderandRisk = Nothing
                        Dim oQuoteForRiskTax As NexusProvider.Quote
                        oQuoteForRiskTax = oWebService.GetHeaderAndRiskTaxByKey(oQuote.InsuranceFileKey, oQuote.Risks(iCt).Key)

                        For Each oRiskTax As NexusProvider.Tax In oQuoteForRiskTax.RiskTaxes
                            If oRiskTax.IncludeinInstallment = 0 Then
                                dTotalRiskTaxExcludedFromInstalment = dTotalRiskTaxExcludedFromInstalment + oRiskTax.TaxAmount
                            End If
                        Next
                        oQuoteForRiskTax = Nothing
                    Next

                    Cache.Insert(ViewState("TotalRiskTaxExcludedFromInstalmentCacheId"), dTotalRiskTaxExcludedFromInstalment, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
                    Cache.Insert(ViewState("TotalFeeTaxExcludedFromInstalmentCacheId"), dTotalFeeTaxExcludedFromInstalment, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
                    Cache.Insert(ViewState("TotalRiskFeeExcludedFromInstalmentCacheId"), dTotalRiskFeeExcludedFromInstalment, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

                    Dim oAgentCommission As NexusProvider.EditAgentCommission
                    'make SAM call to get the Agent Commission and save them in cache
                    oAgentCommission = oWebService.GetAgentCommission(oQuote.InsuranceFileKey)

                    If oAgentCommission IsNot Nothing Then
                        With oAgentCommission
                            For iCt As Integer = 0 To oAgentCommission.AgentCommission.Count - 1
                                Dim oSelectAgentCommission As NexusProvider.AgentCommission = .AgentCommission(iCt)
                                dAgentCommission = dAgentCommission + oSelectAgentCommission.CommissionValue
                                dTaxOnAgentCommission = dTaxOnAgentCommission + oSelectAgentCommission.TaxValue
                            Next
                        End With
                    End If

                    Cache.Insert(ViewState("AgentCommissionCacheId"), dAgentCommission, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
                    Cache.Insert(ViewState("TaxOnAgentCommissionCacheId"), dTaxOnAgentCommission, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

                    Dim sAgentType As String = String.Empty
                    If Session(CNAgentType) IsNot Nothing Then
                        sAgentType = Session(CNAgentType).ToString.Trim.ToUpper
                    End If

                    If sAgentType = "BROKER" Then
                        dAmountToFinance = CType(Session(CNAmountToPay), Double) - (oQuoteForTax.TotalPolicyTaxExcludedFromFinancing + oQuoteForFees.TotalPolicyFeesExcludedFromFinancing + dTotalRiskTaxExcludedFromInstalment + dTotalFeeTaxExcludedFromInstalment + dTotalRiskFeeExcludedFromInstalment)
                        dAmountToFinance = dAmountToFinance + dAgentCommission
                    Else
                        dAmountToFinance = CType(Session(CNAmountToPay), Double) - (oQuoteForTax.TotalPolicyTaxExcludedFromFinancing + oQuoteForFees.TotalPolicyFeesExcludedFromFinancing + dTotalRiskTaxExcludedFromInstalment + dTotalFeeTaxExcludedFromInstalment + dTotalRiskFeeExcludedFromInstalment)
                    End If

                    dOverrideInterestRate = ViewState("dOverrideInterestRate")
                    dOverrideRate = ViewState("dOverrideRate")
                    bPaymentProtection = ViewState("bPaymentProtection")
                    dOverrideDepositAmount = ViewState("dOverrideDepositAmount")
                    bOverrideCommission = ViewState("bOverrideCommission")

                    'If process is MTA then we need to display instalment type option 
                    If Not Session(CNMTAType) Is Nothing OrElse (Request.QueryString("Type") IsNot Nothing AndAlso Request.QueryString("Type") = "MTA") Then

                        'WPR005 - Do not Show Plans if user lands with this querystring request 
                        If Request.QueryString("ShowPlan") IsNot Nothing AndAlso Request.QueryString("ShowPlan") = "False" Then
                            'Selection of instalment type will be visible only for MTA
                            'pnlInstalmentType.Visible = False
                            'Call on Instalment Plan Maintenance MTA
                            Dim iPFPremFinanceKey As Integer = 0
                            Dim iPFPremFinanceVersion As Integer = 0
                            If Request.QueryString("FinancePlanKey") IsNot Nothing AndAlso Request.QueryString("FinancePlanKey") <> "" AndAlso Request.QueryString("FinancePlanVersion") IsNot Nothing AndAlso Request.QueryString("FinancePlanVersion") <> "" Then
                                iPFPremFinanceKey = Request.QueryString("FinancePlanKey")
                                iPFPremFinanceVersion = Request.QueryString("FinancePlanVersion")
                            End If

                            oInstalmentQuotes = oWebService.GetInstalmentQuotes(v_dAmountToFinance:=0,
                                             v_dtStartDate:=oQuote.CoverStartDate, v_dtEndDate:=oQuote.CoverEndDate, v_dtPreferredDate:=Date.Today,
                                              v_dtQuoteDate:=DateTime.Now, v_iWeekDay:=1, v_iMonthDay:=1, v_iInsuranceFileKey:=oQuote.InsuranceFileKey,
                                              v_dOverrideInterestRate:=dOverrideInterestRate, v_dOverrideRate:=dOverrideRate, v_bPaymentProtection:=bPaymentProtection,
                                              v_sBranchCode:=oQuote.BranchCode, v_iInstallmentType:=NexusProvider.InstalmentType.NoAmountChange, sProcessPFMode:="MTA",
                                              v_OverrideDepositAmount:=dOverrideDepositAmount, bOverrideCommission:=bOverrideCommission,
                                              iPremiumFinancekey:=iPFPremFinanceKey, iPremiumFinanceVersionKey:=iPFPremFinanceVersion)
                        Else
                            'Selection of instalment type will be visible only for MTA
                            'pnlInstalmentType.Visible = True

                            'check if existing plan is completed or cancelled
                            'if yes then disable first 2 options and 3rd should be default selected
                            If oQuote.OriginalInsuranceFileKey <> 0 Then
                                Dim oFinancePlanDetail As New NexusProvider.FinancePlan
                                oFinancePlanDetail = oWebService.GetFinancePlanDetails(oQuote.OriginalInsuranceFileKey, nPremiumFinanceCnt:=oQuote.DefaultInstalmentPlan, nPremiumFinanceVersion:=oQuote.DefaultInstalmentPlanVersion)
                                If oFinancePlanDetail IsNot Nothing Then
                                    Session(CNFinancePlan) = oFinancePlanDetail
                                    'If oFinancePlanDetail.StatusDescription = "Cancelled" Or oFinancePlanDetail.StatusDescription = "Completed" Then
                                    '    rbInstalmentType.Items(0).Enabled = False
                                    '    rbInstalmentType.Items(1).Enabled = False
                                    '    rbInstalmentType.SelectedValue = "2"
                                    'End If
                                End If
                            Else

                                'By default First option(AddAndSpread) will be selected 
                                'rbInstalmentType.SelectedValue = "0"

                                ''when policy changed to instalment policy from invoice during MTA
                                'If rblSelectedVal <> "0" AndAlso CInt(Request.QueryString("FinancePlanKey")) = 0 _
                                '    AndAlso (Session(CNMTAType) = MTAType.PERMANENT OrElse Session(CNMTAType) = MTAType.TEMPORARY) Then
                                '    ' rbInstalmentType.SelectedValue = rblSelectedVal
                                'End If
                            End If


                            If Request.QueryString("Type") IsNot Nothing AndAlso Request.QueryString("Type") = "MTA" Then
                                'Call on Instalment Plan Maintenance MTA
                                Dim iPFPremFinanceKey As Integer = 0
                                Dim iPFPremFinanceVersion As Integer = 0
                                If Request.QueryString("FinancePlanKey") IsNot Nothing AndAlso Request.QueryString("FinancePlanKey") <> "" AndAlso Request.QueryString("FinancePlanVersion") IsNot Nothing AndAlso Request.QueryString("FinancePlanVersion") <> "" Then
                                    iPFPremFinanceKey = Request.QueryString("FinancePlanKey")
                                    iPFPremFinanceVersion = Request.QueryString("FinancePlanVersion")

                                End If
                                If (Session(CNTransactionDetails)) IsNot Nothing Then
                                    Dim oFinancePlanTransactionCollection As New NexusProvider.FinancePlanTransactionsCollection
                                    oFinancePlanTransactionCollection = Session(CNTransactionDetails)

                                    oInstalmentQuotes = oWebService.GetInstalmentQuotes(v_dAmountToFinance:=0,
                                                                 v_dtStartDate:=oQuote.CoverStartDate, v_dtEndDate:=oQuote.CoverEndDate, v_dtPreferredDate:=Date.Today,
                                                                  v_dtQuoteDate:=DateTime.Now, v_iWeekDay:=1, v_iMonthDay:=1, v_iInsuranceFileKey:=oQuote.InsuranceFileKey,
                                                                  v_dOverrideInterestRate:=dOverrideInterestRate, v_dOverrideRate:=dOverrideRate, v_bPaymentProtection:=bPaymentProtection,
                                                                  v_sBranchCode:=oQuote.BranchCode, v_iInstallmentType:=1, sProcessPFMode:="MTA",
                                                                  v_OverrideDepositAmount:=dOverrideDepositAmount, bOverrideCommission:=bOverrideCommission,
                                                                  iPremiumFinancekey:=iPFPremFinanceKey, iPremiumFinanceVersionKey:=iPFPremFinanceVersion,
                                                                  oPFTransactionCollection:=oFinancePlanTransactionCollection)
                                End If
                            Else
                                'Call on Policy MTA
                                oInstalmentQuotes = oWebService.GetInstalmentQuotes(v_dAmountToFinance:=dAmountToFinance,
                                                 v_dtStartDate:=oQuote.CoverStartDate, v_dtEndDate:=oQuote.CoverEndDate, v_dtPreferredDate:=Date.Today,
                                                 v_dtQuoteDate:=DateTime.Now, v_iWeekDay:=1, v_iMonthDay:=1, v_iInsuranceFileKey:=oQuote.InsuranceFileKey,
                                                 v_dOverrideInterestRate:=-1, v_dOverrideRate:=0, v_bPaymentProtection:=True, v_sBranchCode:=oQuote.BranchCode,
                                                 v_iInstallmentType:=NexusProvider.InstalmentType.AddToNewPlan, iPremiumFinancekey:=oQuote.DefaultInstalmentPlan, iPremiumFinanceVersionKey:=oQuote.DefaultInstalmentPlanVersion)
                            End If
                        End If
                    Else

                        'Incase of SED and SRD plans selected
                        If Request.QueryString("Type") = "NewPlanSED" Then
                            Dim nInsuranceFileKey As Integer = oQuote.InsuranceFileKey
                            oFinancePlanInformation = oWebService.GetFinancePlanInformation(oQuote.InsuranceFileKey)
                            Session("PFProductCode") = oFinancePlanInformation.ProductCode
                            'In Nexus, we does not have input field for selecting weekday and month day
                            'By default first value is selected in BO.So passing value 1 for these parameters
                            If oFinancePlanInformation.ProductCode.ToUpper() = "MTA" OrElse oFinancePlanInformation.ProductCode.ToUpper() = "REN" Then
                                'Selection of instalment type will be visible only for MTA
                                'pnlInstalmentType.Visible = True
                                ''TFS Defect #9179
                                'divChangeDate.Attributes.Add("style", "display:none;")
                                ''By default First option(AddAndSpread) will be selected 
                                'rbInstalmentType.SelectedValue = "0"
                                If oFinancePlanInformation.ProductCode.ToUpper() = "MTA" Then
                                    nInsuranceFileKey = oFinancePlanInformation.OriginalInsuranceFileKey
                                End If
                                If oQuote.InsuranceFileKey <> 0 Then
                                    Dim oFinancePlan As New NexusProvider.FinancePlan
                                    If oFinancePlanInformation.ProductCode.ToUpper() = "MTA" Then
                                        oFinancePlan = oWebService.GetFinancePlanDetails(oFinancePlanInformation.OriginalInsuranceFileKey)
                                    Else
                                        oFinancePlan = oWebService.GetFinancePlanDetails(oQuote.InsuranceFileKey)
                                    End If
                                    If oFinancePlan IsNot Nothing Then
                                        Session(CNFinancePlan) = oFinancePlan
                                    End If
                                End If
                            End If
                            'On load of Plan NB
                            Dim oFinancePlanTransactionCollection As New NexusProvider.FinancePlanTransactionsCollection
                            If (Session(CNTransactionDetails)) IsNot Nothing Then
                                oFinancePlanTransactionCollection = Session(CNTransactionDetails)
                            End If

                            oInstalmentQuotes = oWebService.GetInstalmentQuotes(v_dAmountToFinance:=dAmountToFinance,
                                                        v_dtStartDate:=oQuote.CoverStartDate, v_dtEndDate:=oQuote.CoverEndDate, v_dtPreferredDate:=Date.Today,
                                                        v_dtQuoteDate:=DateTime.Now, v_iWeekDay:=1, v_iMonthDay:=1, v_iInsuranceFileKey:=nInsuranceFileKey,
                                                        v_dOverrideInterestRate:=dOverrideInterestRate, v_dOverrideRate:=dOverrideRate, v_bPaymentProtection:=bPaymentProtection,
                                                        v_iInstallmentType:="0", r_nPartyBankId:=nPartyBankId,
                                                        v_OverrideDepositAmount:=dOverrideDepositAmount,
                                                        bOverrideCommission:=bOverrideCommission, sProcessPFMode:=oFinancePlanInformation.ProductCode.ToUpper(),
                                                                                oPFTransactionCollection:=oFinancePlanTransactionCollection)
                        ElseIf Not Session(CNMode) Is Nothing AndAlso Session(CNMode) = Mode.View Then
                            Dim nInsuranceFileKey As Integer = oQuote.InsuranceFileKey
                            If oQuote.InsuranceFileKey <> 0 Then
                                Dim oFinancePlan As New NexusProvider.FinancePlan
                                oFinancePlan = oWebService.GetFinancePlanDetails(oQuote.InsuranceFileKey)
                                If oFinancePlan IsNot Nothing Then
                                    Session(CNFinancePlan) = oFinancePlan
                                End If
                            End If
                            oInstalmentQuotes = oWebService.GetInstalmentQuotes(v_dAmountToFinance:=0,
                                                        v_dtStartDate:=oQuote.CoverStartDate, v_dtEndDate:=oQuote.CoverEndDate, v_dtPreferredDate:=Date.Today,
                                                        v_dtQuoteDate:=DateTime.Now, v_iWeekDay:=1, v_iMonthDay:=1, v_iInsuranceFileKey:=nInsuranceFileKey,
                                                        v_dOverrideInterestRate:=dOverrideInterestRate, v_dOverrideRate:=dOverrideRate, v_bPaymentProtection:=bPaymentProtection,
                                                        v_iInstallmentType:=NexusProvider.InstalmentType.NoAmountChange, sProcessPFMode:="MTA", v_OverrideDepositAmount:=dOverrideDepositAmount,
                                                        bOverrideCommission:=bOverrideCommission)

                        Else
                            If Session(CNRenewal) IsNot Nothing AndAlso Convert.ToBoolean(Session(CNRenewal)) Then
                                Dim oFinancePlanDetail As New NexusProvider.FinancePlan
                                oFinancePlanDetail = oWebService.GetFinancePlanDetails(oQuote.InsuranceFileKey, nPremiumFinanceCnt:=oQuote.DefaultInstalmentPlan, nPremiumFinanceVersion:=oQuote.DefaultInstalmentPlanVersion)
                                If oFinancePlanDetail IsNot Nothing Then
                                    Session(CNFinancePlan) = oFinancePlanDetail
                                End If
                            End If
                            'In Nexus, we does not have input field for selecting weekday and month day
                            'By default first value is selected in BO.So passing value 1 for these parameters
                            'On load of Plan NB
                            If oQuote.InsuranceFileKey <> 0 Then
                                Dim oFinancePlan As New NexusProvider.FinancePlan
                                oFinancePlan = oWebService.GetFinancePlanDetails(oQuote.InsuranceFileKey)
                                If oFinancePlan IsNot Nothing Then
                                    Session(CNFinancePlan) = oFinancePlan
                                End If
                            End If
                            oInstalmentQuotes = oWebService.GetInstalmentQuotes(v_dAmountToFinance:=dAmountToFinance,
                                                     v_dtStartDate:=oQuote.CoverStartDate, v_dtEndDate:=oQuote.CoverEndDate, v_dtPreferredDate:=Date.Today,
                                                     v_dtQuoteDate:=DateTime.Now, v_iWeekDay:=1, v_iMonthDay:=1, v_iInsuranceFileKey:=oQuote.InsuranceFileKey,
                                                     v_dOverrideInterestRate:=dOverrideInterestRate, v_dOverrideRate:=dOverrideRate, v_bPaymentProtection:=bPaymentProtection,
                                                     v_sBranchCode:=oQuote.BranchCode, v_iInstallmentType:=NexusProvider.InstalmentType.AddToNewPlan, r_nPartyBankId:=nPartyBankId,
                                                     v_OverrideDepositAmount:=dOverrideDepositAmount, bOverrideCommission:=bOverrideCommission)

                            Dim isInstalmentSchemeValid As Boolean = False
                            For Each oInstalmentQuote As NexusProvider.InstalmentQuote In oInstalmentQuotes
                                If oInstalmentQuote.SchemeNo = Session(CNSelectedSchemeNo) AndAlso Session(CNSelectedSchemeNo) IsNot Nothing Then
                                    Cache.Insert(ViewState("SelectedInstalmentQuoteCacheId"), oInstalmentQuote, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
                                    isInstalmentSchemeValid = True
                                    Exit For
                                End If
                            Next
                            If Not isInstalmentSchemeValid Then
                                Session.Remove(CNPayment)
                                Exit Sub

                            End If


                            'to do


                            If Cache.Item(ViewState("Partybankidcache")) IsNot Nothing Then
                                nPartyBankId = Cache.Item(ViewState("Partybankidcache"))  ' CType(Cache.Item(ViewState("Partybankidcache")), Integer)
                            End If
                        End If

                    End If

                    'If (oInstalmentQuotes IsNot Nothing AndAlso oInstalmentQuotes.Count > 0) Then
                    '    If oInstalmentQuotes(0).UseTransCurrncy = 1 And oInstalmentQuotes(0).ProductCode = "MTA" AndAlso chkUseTransactionCurrency.Checked Then
                    '        chkUseTransactionCurrency.Checked = True
                    '        chkUseTransactionCurrency.Enabled = True
                    '    End If
                    '    If oInstalmentQuotes(0).ProductCode = "MTA" AndAlso rbInstalmentType.SelectedValue <> "2" Then
                    '        If oInstalmentQuotes(0).UseTransCurrncy = 1 Then
                    '            chkUseTransactionCurrency.Checked = True
                    '        Else
                    '            chkUseTransactionCurrency.Checked = False
                    '        End If

                    '    End If
                    'End If
                    'Add the retrived quotes in cache.So that they can be used throughout the page
                    Cache.Insert(ViewState("InstalmentQuotesCacheID"), oInstalmentQuotes, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
                    Cache.Insert(ViewState("Partybankidcache"), nPartyBankId, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

                    ' pnlPlanSummary.Visible = True
                Catch ex As NexusProvider.NexusException
                    If ex.Errors(0).Code = "0" Then
                        ' pnlPlanSummary.Visible = False
                    Else
                        Throw
                    End If
                End Try


                'Bind the grid with retrieved quotes
                'grdInstallmentQuotes.DataSource = oInstalmentQuotes
                'grdInstallmentQuotes.DataBind()

                oPartyBankDetails = oWebService.GetPartyBankDetails(oQuote.PartyKey)

                'Populate Party bank Details
                oParty.BankDetails = oPartyBankDetails
                Session(CNParty) = oParty

                '    If grdInstallmentQuotes.SelectedIndex <> -1 AndAlso grdInstallmentQuotes.Rows.Count > 0 Then
                '        grdInstallmentQuotes.Rows(grdInstallmentQuotes.SelectedIndex).Cells(0).Style.Add(HtmlTextWriterStyle.FontWeight, "Bold")
                '    End If

                '    'Details for selected instalment quote should be visible in pnlPlanSummary
                '    If grdInstallmentQuotes.Rows.Count > 0 And grdInstallmentQuotes.SelectedIndex = -1 Then
                '        If grdInstallmentQuotes.SelectedIndex = -1 Then
                '            Dim bUsePriorTermSchemeAtRenewal As Boolean = False
                '            Dim eGridViewSelectEvent As System.Web.UI.WebControls.GridViewSelectEventArgs = Nothing
                '            Dim eGridViewPageEvent As System.Web.UI.WebControls.GridViewPageEventArgs = Nothing
                '            If Session(CNInstalmentsPlan) IsNot Nothing AndAlso Session(CNMTAType) Is Nothing Then
                '                Dim sInstalmentsPlan() As String = Session(CNInstalmentsPlan).ToString().Split(",")
                '                Session(CNInstalmentsPlan) = Nothing
                '                Dim nPageIndex As Integer = -1
                '                Dim nSelectedItem As Integer = -1
                '                For iCount As Integer = 0 To oInstalmentQuotes.Count - 1
                '                    If CInt(sInstalmentsPlan(0)) = oInstalmentQuotes(iCount).SchemeNo AndAlso CInt(sInstalmentsPlan(1)) = oInstalmentQuotes(iCount).SchemeVersion Then
                '                        nPageIndex = CInt(Math.Ceiling((iCount + 1) / grdInstallmentQuotes.PageSize) - 1)
                '                        nSelectedItem = CInt(iCount - (grdInstallmentQuotes.PageSize * nPageIndex))
                '                        eGridViewSelectEvent = New System.Web.UI.WebControls.GridViewSelectEventArgs(nSelectedItem)
                '                        eGridViewPageEvent = New System.Web.UI.WebControls.GridViewPageEventArgs(nPageIndex)
                '                        grdInstallmentQuotes_PageIndexChanging(Nothing, eGridViewPageEvent)
                '                        grdInstallmentQuotes_SelectedIndexChanging(Nothing, eGridViewSelectEvent)
                '                        bUsePriorTermSchemeAtRenewal = True
                '                        Exit For
                '                    End If
                '                Next
                '                'If some quotes retrieved then first instalment quote should be selectes and btNext should be visible.
                '                If Not bUsePriorTermSchemeAtRenewal Then
                '                    'Select first row
                '                    eGridViewSelectEvent = New System.Web.UI.WebControls.GridViewSelectEventArgs(0)
                '                    grdInstallmentQuotes_SelectedIndexChanging(Nothing, eGridViewSelectEvent)
                '                    'Show instalment details for selecetd instalment quote
                '                    ShowDetailsForScheme(oInstalmentQuotes(0).SchemeNo, oInstalmentQuotes(0).SchemeVersion, oInstalmentQuotes(0).CompanyNo)
                '                End If

                '                ''Set the condition for View mode so that default scheme can be appear in selected mode.
                '            ElseIf (Session(CNMode) = Mode.View) _
                '            OrElse (Not Session(CNMTAType) Is Nothing OrElse (Request.QueryString("Type") IsNot Nothing AndAlso Request.QueryString("Type") = "MTA")) _
                '            AndAlso Not oQuote Is Nothing Then


                '                Dim nPageIndex As Integer = -1
                '                Dim nSelectedItem As Integer = -1

                '                For iCount As Integer = 0 To oInstalmentQuotes.Count - 1
                '                    If CInt(oQuote.DefaultSchemeNumber) = oInstalmentQuotes(iCount).SchemeNo AndAlso CInt(oQuote.DefaultSchemeVersion) = oInstalmentQuotes(iCount).SchemeVersion Then
                '                        nPageIndex = CInt(Math.Ceiling((iCount + 1) / grdInstallmentQuotes.PageSize) - 1)
                '                        nSelectedItem = CInt(iCount - (grdInstallmentQuotes.PageSize * nPageIndex))
                '                        eGridViewSelectEvent = New System.Web.UI.WebControls.GridViewSelectEventArgs(nSelectedItem)
                '                        eGridViewPageEvent = New System.Web.UI.WebControls.GridViewPageEventArgs(nPageIndex)
                '                        grdInstallmentQuotes_PageIndexChanging(Nothing, eGridViewPageEvent)
                '                        grdInstallmentQuotes_SelectedIndexChanging(Nothing, eGridViewSelectEvent)
                '                        bUsePriorTermSchemeAtRenewal = True
                '                        Exit For
                '                    End If
                '                Next

                '            Else
                '                'If some quotes retrieved then first instalment quote should be selectes and btNext should be visible.
                '                'Select first row
                '                eGridViewSelectEvent = New System.Web.UI.WebControls.GridViewSelectEventArgs(0)
                '                grdInstallmentQuotes_SelectedIndexChanging(Nothing, eGridViewSelectEvent)
                '                'Show instalment details for selecetd instalment quote
                '                ShowDetailsForScheme(oInstalmentQuotes(0).SchemeNo, oInstalmentQuotes(0).SchemeVersion, oInstalmentQuotes(0).CompanyNo)
                '            End If
                '        Else
                '            ShowDetailsForScheme(oInstalmentQuotes(grdInstallmentQuotes.SelectedIndex).SchemeNo, oInstalmentQuotes(grdInstallmentQuotes.SelectedIndex).SchemeVersion, oInstalmentQuotes(grdInstallmentQuotes.SelectedIndex).CompanyNo)
                '        End If
                '        'Make visible tyo plan summary and btnNext
                '        pnlPlanSummary.Visible = True
                '    Else
                '        If grdInstallmentQuotes.SelectedIndex <> -1 AndAlso grdInstallmentQuotes.Rows.Count > 0 Then
                '            Dim iSelectedSchemeNumber As Integer = grdInstallmentQuotes.DataKeys(grdInstallmentQuotes.SelectedIndex).Values("SchemeNo")
                '            Dim iSelectedSchemeVersion As Integer = grdInstallmentQuotes.DataKeys(grdInstallmentQuotes.SelectedIndex).Values("SchemeVersion")
                '            Dim iSelectedCompanyNumber As Integer = grdInstallmentQuotes.DataKeys(grdInstallmentQuotes.SelectedIndex).Values("CompanyNo")
                '            Dim iSelectedFrequencyId As Integer = grdInstallmentQuotes.DataKeys(grdInstallmentQuotes.SelectedIndex).Values("FrequencyID")

                '            'Show instalment details for selected instalment quote
                '            If hvActualIndex.Value <> "" Then
                '                PopulateInstalmentDates(hvActualIndex.Value)
                '            Else
                '                PopulateInstalmentDates(grdInstallmentQuotes.SelectedIndex)
                '            End If

                '            CallGetInstalmentQuotes()
                '            ShowDetailsForScheme(iSelectedSchemeNumber, iSelectedSchemeVersion, iSelectedCompanyNumber)
                '            Exit Sub
                '        End If
                '        If grdInstallmentQuotes.Rows.Count > 0 Then
                '            PopulateInstalmentDates()
                '        End If
                '    End If

                '    'If oQuote IsNot Nothing AndAlso grdInstallmentQuotes.SelectedIndex < 0 Then
                '    '    For Each row As GridViewRow In grdInstallmentQuotes.Rows
                '    '        If (grdInstallmentQuotes.DataKeys(row.RowIndex).Values("SchemeNo").ToString().Trim() = oQuote.DefaultSchemeNumber.ToString().Trim()) Then
                '    '            grdInstallmentQuotes.SelectedIndex = row.RowIndex
                '    '            Exit For
                '    '        End If
                '    '    Next
                '    'End If
                '    If Session(CNRenewal) IsNot Nothing AndAlso Convert.ToBoolean(Session(CNRenewal)) AndAlso
                '        Not IsPostBack AndAlso Session(CNFinancePlan) IsNot Nothing Then
                '        Dim oFinancePlan As New NexusProvider.FinancePlan
                '        oFinancePlan = CType(Session(CNFinancePlan), NexusProvider.FinancePlan)
                '        'ddlDayinMonth.SelectedValue = DateAndTime.Day(CDate(oFinancePlan.NextInstalmentDate))
                '        'ddlFirstPaymentDate.SelectedValue = oFinancePlan.FirstInstalmentDate.ToShortDateString
                '    End If


            End If
        End Sub

        Private Sub CheckUseTransCurrency(ByVal iBaseCurrencyID As Integer, ByVal iTransCurrencyID As Integer, Optional ByVal iTranCurrency As Integer = 0)

            'If iBaseCurrencyID <> iTransCurrencyID AndAlso Session(CNPFUserAuthorityValue) = 1 Then
            '    chkUseTransactionCurrency.Checked = True
            'Else
            '    chkUseTransactionCurrency.Checked = False
            'End If

            'If iBaseCurrencyID <> iTransCurrencyID AndAlso Session(CNPFUserAuthorityValue) = 1 Then
            '    chkUseTransactionCurrency.Enabled = True
            'Else
            '    chkUseTransactionCurrency.Enabled = False
            'End If

        End Sub

    End Class
End Namespace