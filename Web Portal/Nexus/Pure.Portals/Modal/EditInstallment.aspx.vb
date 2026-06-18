Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Utils
Imports System.Globalization

Namespace Nexus
    Partial Class Modal_EditInstallment1 : Inherits System.Web.UI.Page

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oUserAuthorityEditInstalmentNoofDays As New NexusProvider.UserAuthority
            oUserAuthorityEditInstalmentNoofDays.UserCode = Session(CNLoginName)
            oUserAuthorityEditInstalmentNoofDays.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.EditInstalmentNoOfDays
            oWebservice.GetUserAuthorityValue(oUserAuthorityEditInstalmentNoofDays)
            Dim nPFInstalmentKey As Integer = 0
            If Request.QueryString("PfInstalmentKey") IsNot Nothing Then
                nPFInstalmentKey = Request.QueryString("PfInstalmentKey")
            End If
            Dim oInstalmentsCollection As New NexusProvider.InstalmentsCollection
            If Not Page.IsPostBack Then

                If Session(CNFinancePlanDetails) IsNot Nothing Then
                    oInstalmentsCollection = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).Instalments
                    If oInstalmentsCollection IsNot Nothing AndAlso oInstalmentsCollection.Count > 0 Then
                        For Each oInstalment As NexusProvider.Instalment In oInstalmentsCollection
                            If oInstalment.PFInstalmentsKey = nPFInstalmentKey Then
                                FillFormFields(oInstalment)
                                Exit For
                            End If
                        Next
                    End If
                End If
            End If

            DisableControls()
            If Request.QueryString("UserAuthorityInstalmentDueDate") IsNot Nothing AndAlso Request.QueryString("UserAuthorityInstalmentDueDate") = 1 Then
                clDueDate.Enabled = True
            Else
                clDueDate.Enabled = False
            End If
            If Request.QueryString("UserAuthorityChangeStatus") IsNot Nothing AndAlso Request.QueryString("UserAuthorityChangeStatus") = 1 Then
                ddlStatus.Enabled = True
            Else
                ddlStatus.Enabled = False
                ddlStatus.CssClass = "field-medium"
            End If
            If Session(CNInstalmentPlanMode) = InstalmentPlanType.View Then
                btnClose.Visible = True
                btnOk.Visible = False
                btnClose.Attributes.Add("onclick", "self.parent.tb_remove();return false;")
            Else
                btnClose.Visible = True
                btnClose.Text = GetLocalResourceObject("btnCancel").ToString()
                btnClose.Attributes.Add("onclick", "self.parent.tb_remove();return false;")
            End If

        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Private Sub FillFormFields(ByVal oInstalment As NexusProvider.Instalment)
            With oInstalment
                hfPreviousStatus.Value = .StatusCode.Trim()
                hfPreviousDueDate.Value = .DueDate
                ddlStatus.Value = .StatusCode.Trim()
                txtAmount.Text = .Amount
                txtBatchNo.Text = .BatchRef
                txtDueDate.Text = .DueDate
                txtFee.Text = .Fee
                hvPfInstalmentKey.Value = .PFInstalmentsKey
                txtInstalmentNo.Text = .InstalmentNumber
                If .PaymentDate <> Date.MinValue Then
                    txtPaidDate.Text = .PaymentDate
                Else
                    txtPaidDate.Text = ""
                End If
                If .PostedDate <> Date.MinValue Then
                    txtPostedDate.Text = .PostedDate
                Else
                    txtPostedDate.Text = ""
                End If

                txtTransactionCode.Text = .TransactionDescription

                If .InstalmentReasonCode Is Nothing Then
                    txtReason.Text = .StatusDescription
                Else
                    txtReason.Text = .InstalmentReasonCode
                End If

            End With
        End Sub

        Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click

            If Page.IsValid() Then
                Dim sMessage As String = String.Empty
                ValidateDueDates(sMessage, CInt(txtInstalmentNo.Text))
                If (sMessage = String.Empty) Then
                Dim objBaseInstalment As New BaseInstalment
                If ddlStatus.Value.Trim() <> hfPreviousStatus.Value Then
                    objBaseInstalment.UpdateInstalmentStatus(hvPfInstalmentKey.Value, ddlStatus.Value, Nothing)
                    If ddlStatus.Value = "H" Then
                        CreateWorkManagerTask()
                    End If
                End If
                'Re-populate instlament plan details with updated values
                Dim nFinancePlanKey As Integer
                Dim nFinancePlanVersion As Integer
                    Dim nInstalmentno As Integer
                    Dim dtDueDate As Date
                If Request.QueryString("FinancePlanKey") IsNot Nothing AndAlso Request.QueryString("FinancePlanKey") <> "" AndAlso Request.QueryString("FinancePlanVersion") IsNot Nothing AndAlso Request.QueryString("FinancePlanVersion") <> "" Then
                    nFinancePlanKey = Request.QueryString("FinancePlanKey")
                    nFinancePlanVersion = Request.QueryString("FinancePlanVersion")
                        If txtDueDate.Text <> hfPreviousDueDate.Value Then
                            nInstalmentno = txtInstalmentNo.Text
                            dtDueDate = Convert.ToDateTime(txtDueDate.Text).ToShortDateString()
                            objBaseInstalment.UpdateInstalmentDetails(nFinancePlanKey, nFinancePlanVersion, nInstalmentno, dtDueDate, Nothing)
                            Dim sEventMessage As String = ""
                            If nInstalmentno = 0 Then
                                sEventMessage = GetLocalResourceObject("EventMessage").ToString().Replace("&nFinancePlanKey", nFinancePlanKey.ToString()).Replace("Instalment Number", "").Replace("&nInstalmentno", "Deposit Instalment").Replace("&hfPreviousDueDate", hfPreviousDueDate.Value.ToString()).Replace("&txtDueDate", txtDueDate.Text)
                            Else
                                sEventMessage = GetLocalResourceObject("EventMessage").ToString().Replace("&nFinancePlanKey", nFinancePlanKey.ToString()).Replace("&nInstalmentno", nInstalmentno.ToString()).Replace("&hfPreviousDueDate", hfPreviousDueDate.Value.ToString()).Replace("&txtDueDate", txtDueDate.Text)
                            End If
                            CreateEvent(sEventMessage)
                        End If
                        ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "closeThickBox", "self.parent.tb_updatedEditMode('RedirectFinancePlanDetailsEdit','~/PremiumFinance/FinancePlanDetails.aspx?FinancePlanKey=" & nFinancePlanKey & "&FinancePlanVersion=" & nFinancePlanVersion & "&#tab-Instalments ');", True)
                    End If

                Else

                    Dim sScript As String = "alert('" + sMessage + "');"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "thickbox", sScript, True)
                End If
            End If

        End Sub

        ''' <summary>
        ''' Creates a workmanager task when instalment status is changed to hold
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub CreateWorkManagerTask()

            Dim oClaim As NexusProvider.ClaimOpen
            Dim oNexusConfig As Config.NexusFrameWork
            Dim oPortal As Nexus.Library.Config.Portal
            Dim oQuote As NexusProvider.Quote
            Dim oWorkManager As NexusProvider.WorkManager
            Dim oWebService As NexusProvider.ProviderBase

            Try
                oClaim = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                oNexusConfig = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                oPortal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
                oQuote = Session(CNQuote)
                oWorkManager = New NexusProvider.WorkManager
                oWebService = New NexusProvider.ProviderManager().Provider

                oWorkManager.DueDate = DateTime.Now()
                oWorkManager.Client = oQuote.InsuredName
                oWorkManager.Description = String.Format(GetLocalResourceObject("msg_WorkmanagerTaskForHold"), oQuote.InsuranceFileRef, Request.QueryString("FinancePlanKey"), _
                                                         txtInstalmentNo.Text, txtDueDate.Text, oQuote.CurrencyCode, txtAmount.Text)
                oWorkManager.Task = "MEMO"
                oWorkManager.AllocationUser = oQuote.ContactUserName
                oWorkManager.AllocationUserGroup = oPortal.AllocationUserGroup
                oWorkManager.IsUrgent = True
                oWorkManager.IsUrgentForUpdate = 1
                oWorkManager.IsComplete = False
                oWorkManager.IsTaskReview = True
                oWorkManager.ParentTaskId = 0
                'read fromm config
                oWorkManager.TaskGroup = oPortal.TaskGroup
                oWorkManager.ExternalTaskCategoryCode = oPortal.ExternalTaskCategoryCode
                oWorkManager.ExternalTaskStatus = 0

                oWorkManager.TaskGroup = "INSTAL"
                oWorkManager.Task = "PFPLNMAINT"
                oWorkManager.DueDate = DateTime.Now.AddDays(1)

                Dim oWmrk As New NexusProvider.KeyData
                oWmrk.KeyName = "PartyKey"
                oWmrk.KeyValue = CType(HttpContext.Current.Session(CNQuote), NexusProvider.Quote).PartyKey
                oWorkManager.KeyData.Add(oWmrk)

                oWmrk = New NexusProvider.KeyData
                oWmrk.KeyName = "InsuranceFileKey"
                oWmrk.KeyValue = CType(HttpContext.Current.Session(CNQuote), NexusProvider.Quote).InsuranceFileKey
                oWorkManager.KeyData.Add(oWmrk)

                oWmrk = New NexusProvider.KeyData
                oWmrk.KeyName = "InsuranceFolderKey"
                oWmrk.KeyValue = CType(HttpContext.Current.Session(CNQuote), NexusProvider.Quote).InsuranceFolderKey
                oWorkManager.KeyData.Add(oWmrk)


                If (oClaim Is Nothing) Then
                    oWorkManager.LockName = NexusProvider.WMLockType.InsuranceFileCnt
                    oWorkManager.LockValue = oQuote.InsuranceFileKey
                Else
                    oWorkManager.LockName = NexusProvider.WMLockType.ClaimId
                    oWorkManager.LockValue = oClaim.ClaimKey

                    oWmrk = New NexusProvider.KeyData
                    oWmrk.KeyName = "ClaimNo"
                    oWmrk.KeyValue = DirectCast(Session(CNClaim), NexusProvider.ClaimOpen).ClaimNumber
                    oWorkManager.KeyData.Add(oWmrk)

                    oWmrk = New NexusProvider.KeyData
                    oWmrk.KeyName = "PrimaryCause"
                    oWmrk.KeyValue = DirectCast(Session(CNClaim), NexusProvider.ClaimOpen).PrimaryCause
                    oWorkManager.KeyData.Add(oWmrk)

                    oWmrk = New NexusProvider.KeyData
                    oWmrk.KeyName = "ClaimKey"
                    oWmrk.KeyValue = DirectCast(Session(CNClaim), NexusProvider.ClaimOpen).ClaimKey
                    oWorkManager.KeyData.Add(oWmrk)

                    oWmrk = New NexusProvider.KeyData
                    oWmrk.KeyName = "BaseClaimKey"
                    oWmrk.KeyValue = DirectCast(Session(CNClaim), NexusProvider.ClaimOpen).BaseClaimKey
                    oWorkManager.KeyData.Add(oWmrk)
                End If
                oWorkManager.InsuranceFileKey = oQuote.InsuranceFileKey

                oWebService.CreateWmTask(oWorkManager)
            Finally
                oClaim = Nothing
                oNexusConfig = Nothing
                oPortal = Nothing
                oQuote = Nothing
                oWorkManager = Nothing
                oWebService = Nothing
            End Try

        End Sub

        ''' <summary>
        ''' To validate the configuration if status going to be set as HOLD
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="args"></param>
        ''' <remarks></remarks>
        Protected Sub cvInstalmentStatus_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvInstalmentStatus.ServerValidate
            Dim oNexusConfig As Config.NexusFrameWork
            Dim oPortal As Nexus.Library.Config.Portal

            If ddlStatus.Value.Trim() <> hfPreviousStatus.Value Then
                Try

                    oNexusConfig = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                    oPortal = oNexusConfig.Portals.Portal(Portal.GetPortalID())

                    If ddlStatus.Value = "H" And oPortal.AllocationUserGroup = String.Empty And oPortal.TaskGroup = String.Empty Then
                        args.IsValid = False
                    End If

                Finally
                    oNexusConfig = Nothing
                    oPortal = Nothing
                End Try
            End If
        End Sub

        ''' <summary>
        ''' Disable non-editable fields
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub DisableControls()
            txtInstalmentNo.Enabled = False
            txtBatchNo.Enabled = False
            txtReason.Enabled = False
            txtAmount.Enabled = False
            txtFee.Enabled = False
            clDueDate.Enabled = False
            clPostedDate.Enabled = False
            clPaidDate.Enabled = False
            txtPaidDate.Enabled = False
            txtTransactionCode.Enabled = False
        End Sub

        Private Sub ValidateDueDates(ByRef sMessage As String, ByVal iInstallmentNo As Integer)
            Dim formats() As String = {"d/MM/yyyy", "dd/MM/yyyy", "dd/M/yyyy", "d/M/yyyy"}

            Dim thisDt As DateTime
            Dim ntotalnoofInstalment As Integer

            ' this should work with all 3 strings above
            If DateTime.TryParseExact(txtDueDate.Text, formats, System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, thisDt) Then
            Else
                sMessage = GetLocalResourceObject("DueDateFormatValidator").ToString()
                Exit Sub
            End If
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oUserAuthorityEditInstalmentNoofDays As New NexusProvider.UserAuthority
            oUserAuthorityEditInstalmentNoofDays.UserCode = Session(CNLoginName)
            oUserAuthorityEditInstalmentNoofDays.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.EditInstalmentNoOfDays
            oWebservice.GetUserAuthorityValue(oUserAuthorityEditInstalmentNoofDays)
            Dim dtExpiryDate As DateTime
            Dim dtNextInsatlmentDate As DateTime
            dtExpiryDate = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).PremiumFinanceDetails.ExpiryDate
            If iInstallmentNo = 0 Then
                iInstallmentNo = 1
            End If
            If CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).Instalments.Item(0).TransactionDescription.ToUpper().Trim = "DEPOSIT INSTALMENT" Then
                ntotalnoofInstalment = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).Instalments.Count - 1
            Else
                ntotalnoofInstalment = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).Instalments.Count
            End If
            If iInstallmentNo < ntotalnoofInstalment Then
                dtNextInsatlmentDate = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).Instalments(iInstallmentNo).DueDate
            Else
                dtNextInsatlmentDate = dtExpiryDate
            End If

            Dim MaxDueDate As DateTime
            MaxDueDate = Convert.ToDateTime(hfPreviousDueDate.Value).AddDays(oUserAuthorityEditInstalmentNoofDays.UserAuthorityValue)
            If Convert.ToDateTime(txtDueDate.Text).ToString("yyyy/MM/dd") < DateTime.Now.ToString("yyyy/MM/dd") Then
                sMessage = GetLocalResourceObject("DueDateFutureDateValidator").ToString()
            End If
            If Convert.ToDateTime(txtDueDate.Text).ToString("yyyy/MM/dd") > MaxDueDate.ToString("yyyy/MM/dd") Then
                sMessage = GetLocalResourceObject("DueDateUpdatedDateValidator").ToString().Replace("&oUserAuthorityEditInstalmentNoofDays", oUserAuthorityEditInstalmentNoofDays.UserAuthorityValue)

            End If
            If Convert.ToDateTime(txtDueDate.Text).ToString("yyyy/MM/dd") > dtNextInsatlmentDate.ToString("yyyy/MM/dd") Then
                sMessage = GetLocalResourceObject("DueDateNextInsatlmentDateValidator").ToString()
            End If
            If Convert.ToDateTime(txtDueDate.Text).ToString("yyyy/MM/dd") >= dtExpiryDate.ToString("yyyy/MM/dd") Then
                sMessage = GetLocalResourceObject("DueDateRenewalDateValidator").ToString()
            End If
            
            If Convert.ToDateTime(txtDueDate.Text).ToString("yyyy/MM/dd") < Convert.ToDateTime(hfPreviousDueDate.Value).ToString("yyyy/MM/dd") Then
                sMessage = GetLocalResourceObject("OriginalDueDateValidator").ToString()
            End If

        End Sub
        Private Sub CreateEvent(ByVal sMessage As String)



            Dim oQuote As NexusProvider.Quote

            Dim oWebService As NexusProvider.ProviderBase
            Dim oEventDetails As New NexusProvider.EventDetails
            Try

                oQuote = Session(CNQuote)

                oWebService = New NexusProvider.ProviderManager().Provider
                If Page.IsValid Then
                    With oEventDetails
                        .EventDate = Now()
                        .PartyKey = CType(HttpContext.Current.Session(CNQuote), NexusProvider.Quote).PartyKey

                        .UserName = Session(CNLoginName)
                        .Description = sMessage
                        .RtfText = sMessage
                        .InsuranceFileKey = CType(HttpContext.Current.Session(CNQuote), NexusProvider.Quote).InsuranceFileKey
                        .InsuranceFolderKey = CType(HttpContext.Current.Session(CNQuote), NexusProvider.Quote).InsuranceFolderKey
                        
                        .EventTypeKey = 43

                    End With

                    oWebService.AddEvent(oEventDetails)
                End If


            Finally

                oQuote = Nothing
                oEventDetails = Nothing
                oWebService = Nothing
            End Try

        End Sub
        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        End Sub
    End Class
End Namespace
