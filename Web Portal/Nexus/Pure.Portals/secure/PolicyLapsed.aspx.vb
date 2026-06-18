Imports CMS.library
Imports Nexus.Library
Imports System.Data
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Xml
Imports Nexus
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session

Namespace Nexus
    Partial Class secure_PolicyLapsed : Inherits Frontend.clsCMSPage
        Dim oWebService As NexusProvider.ProviderBase
        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not IsPostBack Then
                Try
                    '      btnLapse.Attributes.Add("OnClick", "javascript:return CancelConfirmation();")

                    '' Lapsed Reasons 
                    oWebService = New NexusProvider.ProviderManager().Provider
                    Dim olist As NexusProvider.LookupListCollection
                    olist = oWebService.GetList(NexusProvider.ListType.PMLookup, "lapsed_reason", True, False)

                    Dim CountVar As Integer
                    Dim dt As New DataTable
                    Dim dr As DataRow
                    dt.Columns.Add(New DataColumn("Column1"))
                    dt.Columns.Add(New DataColumn("Column2"))
                    dt.Columns.Add(New DataColumn("Column3"))
                    dt.DefaultView.Sort = "Column1 ASC"

                    Dim MTAReasonCodeForCancellation As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).MTAReasonForCancellation
                    For CountVar = 0 To olist.Count - 1
                        If olist.Item(CountVar).IsDeleted = False Then 'to filter out the deleted records
                            dr = dt.NewRow
                            dr(0) = olist.Item(CountVar).Description
                            dr(1) = olist.Item(CountVar).Code
                            dr(2) = ""
                            dt.Rows.Add(dr)
                        End If
                    Next

                    GridLapsedReasons.DataSource = dt
                    GridLapsedReasons.DataBind()

                Finally
                    oWebService = Nothing
                End Try
            End If

        End Sub

        Protected Sub btnLapse_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLapse.Click

            If Page.IsValid Then
                btnLapse.Enabled = False
                Dim sMessage As String
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                Dim oWebService As NexusProvider.ProviderBase
                Dim oWebServiceProvider = New NexusProvider.ProviderManager().Provider
                Dim SelectedLapsedReason As String = Request("rdoLapsedReasonList")
                oWebService = New NexusProvider.ProviderManager().Provider
                oQuote = Session(CNQuote)
                Dim oExclusiveLocking As NexusProvider.OptionTypeSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ExclusiveLock)
                If oExclusiveLocking.OptionValue = "1" Then
                    'On Lapse policy unlock the Policy
                    UnlockPolicy(oQuote.InsuranceFolderKey)
                End If
                Try
                    oWebService.LapseRenewal(oQuote, SelectedLapsedReason, oQuote.BranchCode)
                Finally
                    oWebService = Nothing
                End Try
                'To Display Lapsed Confirmation Message
                pnlLapsedMsg.Visible = True
                pnlLapsedReason.Visible = False

                If oExclusiveLocking.OptionValue = "1" Then
                    Dim sUserName As String = UnlockPolicy(oQuote.InsuranceFolderKey)
                    If sUserName.Trim.Length > 0 Then
                        sMessage = "alert('" + Replace(GetLocalResourceObject("lbl_policylocked_error"), "{1}", sUserName + ".") + "')"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", sMessage, True)
                        Exit Sub
                    End If
                End If
                'SendMail()
                Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oPortalConfig As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
                'check the web config at portal to show email model page
                If oPortalConfig.PolicyLapseEmail = True Then
                    SendMail()
                    Exit Sub
                End If
            End If
        End Sub

        Private Function UnlockPolicy(ByVal nInsuranceFileKey As Integer) As String
            Dim oLockCollection As NexusProvider.LockCollection
            Dim oWebService As NexusProvider.ProviderBase = Nothing
            Dim sUserName As String = String.Empty
            Dim bMaintainedSuccess As Boolean = False
            Dim bLogout As Boolean = False
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
                        bMaintainedSuccess = oWebService.MaintainLock(oLockCollection, bAllClear, bLogout, Session(CNBranchCode).ToString())
                        sUserName = String.Empty
                    Else
                        sUserName = oLockItem.LockUserName.Trim
                    End If
                    Exit For
                End If
            Next
            Return sUserName
        End Function
        Protected Sub CustVldMTAReasonRequired_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles CustVldMTAReasonRequired.ServerValidate

            If Not Request("rdoLapsedReasonList") Is Nothing Then
                args.IsValid = True
            Else
                args.IsValid = False
            End If

        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "CancelConfirmation", _
            "<script language=""JavaScript"" type=""text/javascript"">function CancelConfirmation(){ var ret= confirm('" & GetLocalResourceObject("msg_CancelLapsePolicy").ToString() & "'); if (ret == true) { document.getElementById('<%=btnLapse.ClientID%>').disabled=ret;} return ret;}</script>")

        End Sub

        Public Sub SendMail()
            Dim sURL As String
            Dim oParty As NexusProvider.BaseParty = HttpContext.Current.Session(CNParty)
            Dim oQuote As NexusProvider.Quote
            Dim oClaim As NexusProvider.ClaimOpen = CType(HttpContext.Current.Session.Item(CNClaim), NexusProvider.ClaimOpen)
            If HttpContext.Current.Session(CNMode) = Mode.NewClaim Or HttpContext.Current.Session(CNMode) = Mode.EditClaim Or HttpContext.Current.Session(CNMode) = Mode.PayClaim Or HttpContext.Current.Session(CNMode) = Mode.SalvageClaim Or HttpContext.Current.Session(CNMode) = Mode.TPRecovery Then
                oQuote = HttpContext.Current.Session(CNClaimQuote)
            Else
                oQuote = HttpContext.Current.Session(CNQuote)
            End If

            If HttpContext.Current.Session.IsCookieless Then
                sURL = "../Modal/SendEmail.aspx?PartyKey=" & oParty.Key & "&key=Issued&InsuranceFileKey=" & oQuote.InsuranceFileKey & "&modal=true&loc=docm&n=p&Riskcheck=true&KeepThis=true&TB_iframe=true&height=300&width=750"
            Else
                sURL = ConfigurationManager.AppSettings("WebRoot") & "Modal/SendEmail.aspx?PartyKey=" & oParty.Key & "&key=Issued&InsuranceFileKey=" & oQuote.InsuranceFileKey & "&modal=true&loc=docm&n=p&Riskcheck=true&KeepThis=true&TB_iframe=true&height=300&width=750"
            End If


            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show", _
            "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sURL & "' , null);});</script>")
            Exit Sub
        End Sub
    End Class
End Namespace
