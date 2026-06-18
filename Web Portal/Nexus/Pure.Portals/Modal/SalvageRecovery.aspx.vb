Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus
    Partial Class Modal_SalvageRecovery
        Inherits CMS.Library.Frontend.clsCMSPage
        Dim oRecoveryType As New NexusProvider.RecoveryType()
        Dim nClaimPerilId As Integer

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Request.QueryString("PerilID") IsNot Nothing Then
                nClaimPerilId = CInt(Request.QueryString("PerilID"))
            End If

            Dim screen As String = Request("FromPage")

            If Not IsPostBack Then
                Page.SetFocus(ddlRecoveryType)

                Dim oClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                Dim oClaimRiskLinkCollection As NexusProvider.ClaimRiskLinkCollection = Nothing
                oClaimRiskLinkCollection = GetRecovery(oClaim.RiskKey)
                If screen = "SA" Then
                    If oClaimRiskLinkCollection(0).RecoveryItemType IsNot Nothing Then
                        For Each oRecoveryItem As NexusProvider.RecoveryType In oClaimRiskLinkCollection(0).RecoveryItemType
                            If oRecoveryItem.IsSalvage = 1 Then
                                ddlRecoveryType.Items.Add(New ListItem(oRecoveryItem.Description, oRecoveryItem.Code))
                            End If
                        Next
                    End If
                ElseIf screen = "TP" Then
                    lblTitle.Text = GetLocalResourceObject("lbl_ThirdPartyRecovery_Title")
                    lblSalvageRecoveryReserve.Text = GetLocalResourceObject("lbl_ThirdPartyRecoveryReserve")
                    If oClaimRiskLinkCollection(0).RecoveryItemType IsNot Nothing Then
                        For Each oRecoveryItem As NexusProvider.RecoveryType In oClaimRiskLinkCollection(0).RecoveryItemType
                            If oRecoveryItem.IsSalvage = 0 Then
                                ddlRecoveryType.Items.Add(New ListItem(oRecoveryItem.Description, oRecoveryItem.Code))
                            End If
                        Next
                    End If
                End If
                txtInitialReserve.Text = "0.00"
                hPartyKey.Value = "0"
            End If



            txtParty.Attributes.Add("readonly", "readonly")

            ' Restore btnParty enabled state based on current party type selection
            Dim sCurrentPartyType As String = GISLookup_RecoveryPartyType.Text.Trim.ToUpper
            If sCurrentPartyType = "OTHER PARTY" Then
                btnParty.Enabled = True
                If HttpContext.Current.Session.IsCookieless Then
                    btnParty.OnClientClick = "tb_show(null , '" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() & "))" & "/Modal/FindOtherParty.aspx?modal=true&Type=All&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                Else
                    btnParty.OnClientClick = "tb_show(null , '" & AppSettings("WebRoot") & "Modal/FindOtherParty.aspx?modal=true&Type=All&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                End If
            ElseIf sCurrentPartyType = "INSURER" Then
                btnParty.Enabled = True
                If HttpContext.Current.Session.IsCookieless Then
                    btnParty.OnClientClick = "tb_show(null , '" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() & "))" & "/Modal/FindReinsurer.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=700' , null);return false;"
                Else
                    btnParty.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "Modal/FindReinsurer.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=700' , null);return false;"
                End If
            Else
                btnParty.Enabled = False
            End If
            txtInitialReserve.Attributes.Add("onkeypress", "javascript:return isInteger(event);")

            BindRecoveryPartyGrid()
        End Sub

        Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
            If Page.IsValid Then
                Dim dResult As Double
                If InitialReserve.Trim.Length <> 0 And Double.TryParse(InitialReserve.Trim, dResult) = True Then
                    Dim oRecovery As New NexusProvider.PerilRecovery
                    Dim oClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                    Dim iPeril As Integer = CInt(Request("PerilIndex"))
                    Dim screen As String = Request("FromPage")
                    With oRecovery
                        .TypeCode = ddlRecoveryType.SelectedValue
                        .InitialRecovery = Math.Round(CDec(InitialReserve), 2)
                        .IsSalvage = If(screen = "SA", 1, 0)
                        .IsNew = True
                        .CanDelete = True
                        .ClaimPerilId = nClaimPerilId
                        .RecoveryPartyTypeId = CInt(GISLookup_RecoveryPartyType.Value)
                        .RecoveryPartyKey = CInt(hPartyKey.Value)
                        .PartyShortName = txtParty.Text
                        .RecoveryPartyTypeCode = GISLookup_RecoveryPartyType.Text
                    End With

                    If screen = "SA" Then
                        If Session(CNRecovery) IsNot Nothing Then
                            Dim OldRecovery As NexusProvider.Claim = Session(CNRecovery)
                            If OldRecovery.ClaimPeril(iPeril).SalvageRecovery.Count > 0 Then
                                oRecovery.CurrentRecovery = InitialReserve
                                oRecovery.CurrentRecovery = Math.Round(oRecovery.CurrentRecovery, 2)
                            Else
                                oRecovery.InitialRecovery = InitialReserve
                            End If
                        End If
                        oClaim.ClaimPeril(iPeril).SalvageRecovery.Add(oRecovery)
                    ElseIf screen = "TP" Then
                        lblSalvageRecoveryReserve.Text = GetLocalResourceObject("lbl_ThirdPartyRecoveryReserve")
                        lblTitle.Text = GetLocalResourceObject("lbl_ThirdPartyRecovery_Title")
                        If Session(CNRecovery) IsNot Nothing Then
                            Dim OldRecovery As NexusProvider.Claim = Session(CNRecovery)
                            If OldRecovery.ClaimPeril(iPeril).TPRecovery.Count > 0 Then
                                oRecovery.CurrentRecovery = Math.Round(CDec(InitialReserve), 2)
                            Else
                                oRecovery.InitialRecovery = InitialReserve
                            End If
                        End If
                        oClaim.ClaimPeril(iPeril).TPRecovery.Add(oRecovery)
                    End If

                    ' Clear inputs for next entry
                    txtInitialReserve.Text = "0.00"
                    txtParty.Text = String.Empty
                    hPartyKey.Value = "0"
                    GISLookup_RecoveryPartyType.Value = ""

                    BindRecoveryPartyGrid()
                End If
            End If
        End Sub

        Protected Sub btnNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNext.Click
            Dim sPostbackTo As String = HttpUtility.JavaScriptStringEncode(Request.QueryString("PostbackTo"))
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_updated('" & sPostbackTo & "');", True)
        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
        End Sub

        Protected Sub gvRecoveryParty_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvRecoveryParty.RowDeleting
            Dim oClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            Dim iPeril As Integer = CInt(Request("PerilIndex"))
            Dim screen As String = Request("FromPage")
            Dim iIndex As Integer = e.RowIndex

            Dim oCollection As NexusProvider.PerilRecoveryCollection = Nothing
            If screen = "SA" Then
                oCollection = oClaim.ClaimPeril(iPeril).SalvageRecovery
            ElseIf screen = "TP" Then
                oCollection = oClaim.ClaimPeril(iPeril).TPRecovery
            End If

            If oCollection IsNot Nothing AndAlso iIndex < oCollection.Count Then
                Dim oRecovery As NexusProvider.PerilRecovery = oCollection(iIndex)
                If oRecovery.ReceiptedAmount > 0 Then
                    CustValidType.IsValid = False
                    CustValidType.ErrorMessage = "Cannot remove party with existing recovery transactions"
                    Return
                End If
                If oRecovery.IsNew Then
                    oCollection.RemoveAt(iIndex)
                Else
                    oRecovery.IsDeleted = True
                End If
            End If

            BindRecoveryPartyGrid()
        End Sub

        Private Sub BindRecoveryPartyGrid()
            Dim oClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            Dim iPeril As Integer = CInt(Request("PerilIndex"))
            Dim screen As String = Request("FromPage")

            Dim oActiveRecoveries As New System.Collections.Generic.List(Of NexusProvider.PerilRecovery)
            Dim oCollection As NexusProvider.PerilRecoveryCollection = Nothing
            If screen = "SA" Then
                oCollection = oClaim.ClaimPeril(iPeril).SalvageRecovery
            ElseIf screen = "TP" Then
                oCollection = oClaim.ClaimPeril(iPeril).TPRecovery
            End If

            If oCollection IsNot Nothing Then
                For Each oRec As NexusProvider.PerilRecovery In oCollection
                    If Not oRec.IsDeleted Then
                        oActiveRecoveries.Add(oRec)
                    End If
                Next
            End If

            gvRecoveryParty.DataSource = oActiveRecoveries
            gvRecoveryParty.DataBind()

            ' Calculate total
            Dim dTotal As Decimal = 0
            For Each oRec As NexusProvider.PerilRecovery In oActiveRecoveries
                dTotal += oRec.InitialRecovery
            Next
            lblRecoveryReserveTotal.Text = dTotal.ToString("N2")
        End Sub

        Public Property InitialReserve() As String
            Get
                Return txtInitialReserve.Text
            End Get
            Set(ByVal value As String)
                txtInitialReserve.Text = value
            End Set
        End Property

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub CustValidType_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles CustValidType.ServerValidate
            If ddlRecoveryType.Items.Count = 0 Then
                args.IsValid = False
                CustValidType.ErrorMessage = GetLocalResourceObject("Err_InvalidRecovery")
            ElseIf ddlRecoveryType.Items.Count > 0 Then
                If txtInitialReserve.Text.Trim.Length = 0 Or txtInitialReserve.Text.Trim = "0.00" Then
                    args.IsValid = False
                    CustValidType.ErrorMessage = GetLocalResourceObject("Err_InvalidRecoveryReserve")
                Else
                    Dim oClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                    Dim screen As String = Request("FromPage")
                    Dim iPeril As Integer = CInt(Request("PerilIndex"))
                    Dim nPartyKey As Integer = CInt(hPartyKey.Value)
                    If screen = "SA" Then
                        For iCount As Integer = 0 To oClaim.ClaimPeril(iPeril).SalvageRecovery.Count - 1
                            If oClaim.ClaimPeril(iPeril).SalvageRecovery(iCount).IsDeleted = False _
                                AndAlso oClaim.ClaimPeril(iPeril).SalvageRecovery(iCount).TypeCode.Trim.ToUpper = ddlRecoveryType.SelectedValue.Trim.ToUpper _
                                AndAlso oClaim.ClaimPeril(iPeril).SalvageRecovery(iCount).RecoveryPartyKey = nPartyKey _
                                AndAlso oClaim.ClaimPeril(iPeril).SalvageRecovery(iCount).ClaimPerilId = nClaimPerilId Then
                                args.IsValid = False
                                CustValidType.ErrorMessage = GetLocalResourceObject("Err_DuplicateParty")
                            End If
                        Next
                    ElseIf screen = "TP" Then
                        For iCount As Integer = 0 To oClaim.ClaimPeril(iPeril).TPRecovery.Count - 1
                            If oClaim.ClaimPeril(iPeril).TPRecovery(iCount).IsDeleted = False _
                                AndAlso oClaim.ClaimPeril(iPeril).TPRecovery(iCount).TypeCode.Trim.ToUpper = ddlRecoveryType.SelectedValue.Trim.ToUpper _
                                AndAlso oClaim.ClaimPeril(iPeril).TPRecovery(iCount).RecoveryPartyKey = nPartyKey _
                                AndAlso oClaim.ClaimPeril(iPeril).TPRecovery(iCount).ClaimPerilId = nClaimPerilId Then
                                args.IsValid = False
                                CustValidType.ErrorMessage = GetLocalResourceObject("Err_DuplicateParty")
                            End If
                        Next
                    End If
                End If
            End If
        End Sub

        Protected Sub CustValidPartyType_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles CustValidPartyType.ServerValidate
            If String.IsNullOrEmpty(GISLookup_RecoveryPartyType.Value) OrElse GISLookup_RecoveryPartyType.Value = "0" Then
                args.IsValid = False
                CustValidPartyType.ErrorMessage = GetLocalResourceObject("Err_PartyTypeRequired")
            End If
        End Sub

        Protected Sub CustValidParty_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles CustValidParty.ServerValidate
            Dim sPartyTypeCode As String = GISLookup_RecoveryPartyType.Text.Trim.ToUpper
            ' PARTY and INSURER require a party key from modal search; AGENT/CLIENT are auto-filled so no key needed
            If sPartyTypeCode = "OTHER PARTY" OrElse sPartyTypeCode = "INSURER" Then
                If String.IsNullOrEmpty(hPartyKey.Value) OrElse hPartyKey.Value = "0" Then
                    args.IsValid = False
                    CustValidParty.ErrorMessage = GetLocalResourceObject("Err_PartyRequired")
                End If
            ElseIf String.IsNullOrEmpty(txtParty.Text.Trim) Then
                args.IsValid = False
                CustValidParty.ErrorMessage = GetLocalResourceObject("Err_PartyRequired")
            End If
        End Sub

        Protected Sub GISLookup_RecoveryPartyType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GISLookup_RecoveryPartyType.SelectedIndexChange
            Dim oQuote As NexusProvider.Quote = CType(Session(CNClaimQuote), NexusProvider.Quote)
            Dim sPartyTypeCode As String = GISLookup_RecoveryPartyType.Text.Trim.ToUpper

            txtParty.Text = String.Empty
            hPartyKey.Value = "0"

            Select Case sPartyTypeCode
                Case "OTHER PARTY"
                    btnParty.Enabled = True
                    If HttpContext.Current.Session.IsCookieless Then
                        btnParty.OnClientClick = "tb_show(null , '" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() & "))" & "/Modal/FindOtherParty.aspx?modal=true&Type=All&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                    Else
                        btnParty.OnClientClick = "tb_show(null , '" & AppSettings("WebRoot") & "Modal/FindOtherParty.aspx?modal=true&Type=All&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                    End If
                Case "INSURER"
                    btnParty.Enabled = True
                    If HttpContext.Current.Session.IsCookieless Then
                        btnParty.OnClientClick = "tb_show(null , '" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() & "))" & "/Modal/FindReinsurer.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=700' , null);return false;"
                    Else
                        btnParty.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "Modal/FindReinsurer.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=700' , null);return false;"
                    End If
                Case "AGENT"
                    btnParty.Enabled = False
                    txtParty.Text = oQuote.AgentCode
                    hPartyKey.Value = oQuote.AgentKey
                Case "CLIENT"
                    btnParty.Enabled = False
                    txtParty.Text = oQuote.InsuredName
                    hPartyKey.Value = oQuote.PartyKey
                Case Else
                    btnParty.Enabled = False
                    txtParty.Text = String.Empty
            End Select
        End Sub
    End Class

End Namespace
