Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Utils
Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session

Namespace Nexus
    Partial Class Secure_ClaimPaymentDoc
        Inherits Frontend.clsCMSPage

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim sProductCode As String
                If Request.QueryString("ProductCode") IsNot Nothing And Not String.IsNullOrEmpty(Request.QueryString("ProductCode")) Then
                    sProductCode = Request.QueryString("ProductCode").Trim
                End If
                Dim iFunctionalArea As Integer
                If Request.QueryString("FunctionalArea") IsNot Nothing And Not String.IsNullOrEmpty(Request.QueryString("FunctionalArea")) Then
                    iFunctionalArea = CType(Request.QueryString("FunctionalArea").Trim, Integer)
                End If
                Dim oProductDocumentsCollection As NexusProvider.ProductDocumentsCollection
                'get document codes confirued at product level for claim payment
                oProductDocumentsCollection = oWebservice.GetProductDocuments(sProductCode, iFunctionalArea)

                'Bind the grid with collection
                grdDocumentLinks.Visible = True
                grdDocumentLinks.AllowPaging = True
                grdDocumentLinks.DataSource = oProductDocumentsCollection
                grdDocumentLinks.DataBind()


               
                If Session(CNMode) = Mode.DeclinePayment Then
                    Dim sConfirmMessage As String = IIf(GetLocalResourceObject("msgConfirmDeclinePayment") Is Nothing, "Are you sure you want to decline this payment?", GetLocalResourceObject("msgConfirmDeclinePayment"))

                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "DetailsConfirmation", _
                    "<script language=""javascript"" type=""text/javascript"">function showConfirmMessage() {return confirm('" & sConfirmMessage & "');}</script>")

                    btnOk.Attributes.Add("onclick", "return showConfirmMessage();")

                End If
            End If
        End Sub

        Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click

            Dim dPaymentDate As Date
            Dim nClaimPaymentKey As Integer
            Dim dPaymentAmount As Double
            Dim sComments As String = ""
            Dim sFailureReason As String
            Dim sProductCode As String
            Dim sClaimNumber As String
            Dim bAuthorise As Boolean
            Dim cstClaimPayment As New CustomValidator
            Dim sMessage As String = ""
            Dim bDecline As Boolean
            Dim sDeclineFailureReason As String

            If Request.QueryString("PaymentDate") IsNot Nothing And Not String.IsNullOrEmpty(Request.QueryString("PaymentDate")) Then
                dPaymentDate = CType(Request.QueryString("PaymentDate").Trim, Date)
            End If
            If Request.QueryString("ClaimPaymentKey") IsNot Nothing And Not String.IsNullOrEmpty(Request.QueryString("ClaimPaymentKey")) Then
                nClaimPaymentKey = CType(Request.QueryString("ClaimPaymentKey").Trim, Integer)
            End If
            If Request.QueryString("PaymentAmount") IsNot Nothing And Not String.IsNullOrEmpty(Request.QueryString("PaymentAmount")) Then
                dPaymentAmount = CType(Request.QueryString("PaymentAmount").Trim, Double)
            End If
            If Request.QueryString("ProductCode") IsNot Nothing And Not String.IsNullOrEmpty(Request.QueryString("ProductCode")) Then
                sProductCode = Request.QueryString("ProductCode").Trim
            End If
            If Request.QueryString("ClaimNumber") IsNot Nothing And Not String.IsNullOrEmpty(Request.QueryString("ClaimNumber")) Then
                sClaimNumber = Request.QueryString("ClaimNumber").Trim
            End If
            If Not String.IsNullOrEmpty(txtComents.Text.Trim) Then
                sComments = txtComents.Text.Trim
            Else
                If Session(CNMode) IsNot Nothing Then
                    If Session(CNMode) = Mode.DeclinePayment Then
                        sComments = CType(GetLocalResourceObject("lblDeclinedComments"), String)
                    Else
                        sComments = ""
                    End If
                End If
            End If
            If Session(CNMode) = Mode.Authorise AndAlso Session(CNDuplicateClaimPayment) = True Then
                Dim oEventDetails As New NexusProvider.EventDetails
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oOpenClaim As NexusProvider.ClaimOpen = Session(CNClaim)
                With oEventDetails
                    If Session(CNClaimQuote) IsNot Nothing Then
                        Dim oQuote As NexusProvider.Quote = CType(Session(CNClaimQuote), NexusProvider.Quote)
                        .PartyKey = oQuote.PartyKey
                    End If
                    .EventDate = Now()
                    '.PartyKey = oOpenClaim.ClaimPayment.PartyKey
                    If Session(CNDuplicateClaimPayment) = True And Session(CNDuplicateClaimPaymentReason) <> "" Then
                        .RtfText = "Duplicate Authorised Claim payment warning message triggered and overridden due to " & Session(CNDuplicateClaimPaymentReason)
                    Else
                        .RtfText = "Duplicate Authorised Claim payment warning message triggered"
                    End If
                    .UserName = Session(CNLoginName)
                    .EventLogSubjectKey = 1
                    .EventTypeKey = 22
                    If Session(CNClaim) IsNot Nothing Then
                        .InsuranceFileKey = oOpenClaim.InsuranceFileKey
                        .ClaimKey = oOpenClaim.ClaimKey
                    End If
                End With

                oWebService.AddEvent(oEventDetails)
                Session(CNDuplicateClaimPayment) = Nothing
                Session(CNDuplicateClaimPaymentReason) = Nothing
            End If
            'Check mode in session
            If Session(CNMode) Is Nothing Then
                sFailureReason = "206"
            ElseIf Session(CNMode) = Mode.Authorise Then
                'call AuthoriseClaimPayment
                bAuthorise = AuthoriseClaimPayment(nClaimPaymentKey:=nClaimPaymentKey,
                                                   sClaimNumber:=sClaimNumber,
                                                   dPaymentDate:=dPaymentDate,
                                                   sAhthoriseReason:=sComments,
                                                   sProductCode:=sProductCode,
                                                   sFailureReason:=sFailureReason, bExclusiveLock:=True)

            ElseIf Session(CNMode) = Mode.DeclinePayment Then
                'call DeclineClaimPayment
                Dim dAuthorityAmount As Double
                Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Try
                    Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
                    Dim oUserAuthority As New NexusProvider.UserAuthority
                    oUserAuthority.UserCode = CType(Session(CNLoginName), String)
                    oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.HasClaimPaymentsAuthority
                    oWebservice.GetUserAuthorityValue(oUserAuthority)
                    dAuthorityAmount = oUserAuthority.UserAuthorityOptionalValue2
                    If dPaymentAmount > dAuthorityAmount Then
                        Dim sAuthorityMessage As String = IIf(GetLocalResourceObject("msgUserAuthority") Is Nothing, "You are not within your Authorization limit for this payment type. Please contact your system adminsitrator.", GetLocalResourceObject("msgUserAuthority"))
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UserNotInAuthorirty", "alert('" + sAuthorityMessage + "');window.location='../secure/AuthoriseClaimPayments.aspx';", True)
                    End If

                Catch ex As System.Exception
                Finally
                    oWebservice = Nothing
                End Try

                bDecline = DeclineClaimPayment(nClaimPaymentKey:=nClaimPaymentKey,
                                               sDeclineReason:=sComments,
                                               sDeclineFailureReason:=sDeclineFailureReason, bExclusiveLock:=True)
                If Not String.IsNullOrEmpty(sDeclineFailureReason) Then
                    sFailureReason = sDeclineFailureReason
                End If

            End If

            If Not String.IsNullOrEmpty(sFailureReason) Then

                If sFailureReason = "331" Then
                    cstClaimPayment.IsValid = False
                    'look for a validation message in the page resources, but if there is not one defined add a default message
                    cstClaimPayment.ErrorMessage = CType(IIf(GetLocalResourceObject("cstDebtorUserGroups") Is Nothing, "Debtor User Groups are not setup. Please contact your system administrator", GetLocalResourceObject("cstDebtorUserGroups")), String)
                    cstClaimPayment.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                    'add the validator to the page, this will have the effect of making the page invalid
                    Page.Validators.Add(cstClaimPayment)
                    Exit Sub
                ElseIf sFailureReason = "200" OrElse sFailureReason = "1000158" Then ' Although this code is handled at the implementation level, however this code is ket here for additional precaution.
                    sMessage = GetLocalResourceObject("msg_ClaimLocked").ToString()
                ElseIf sFailureReason = "206" Then
                    sMessage = GetLocalResourceObject("msg_RecordModified").ToString()
                ElseIf sFailureReason = "1000128" Then
                    sMessage = GetLocalResourceObject("msg_IsReferred").ToString()
                ElseIf sFailureReason <> "Bank" And sFailureReason <> "Mandatory" Then
                    sMessage = sFailureReason.Replace(Environment.NewLine, String.Empty)
                Else
                    If Session(CNMode) <> Mode.DeclinePayment Then
                        'Claim Payment Declined 
                        DeclineClaimPayment(nClaimPaymentKey, sComments)
                        sMessage = "An error occured, Claim Payment Declined."
                    End If
                End If

                If sFailureReason = "Bank" Then
                    sMessage = GetLocalResourceObject("msg_BankDefault").ToString()
                ElseIf sFailureReason = "Mandatory" Then
                    sMessage = GetLocalResourceObject("msg_MandatoryFields").ToString()
                End If
                'Give Error Messages
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "DeclinePayment", _
                                                           "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){DeclinePayment('" & sMessage & "');});</script>")

            Else
                Response.Redirect("~/secure/AuthoriseClaimPayments.aspx")
            End If
        End Sub
    End Class
End Namespace
