Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session
Namespace Nexus
    Partial Class Modal_SalvageRecoveryForEdit
        Inherits CMS.Library.Frontend.clsCMSPage

        Dim oClaim As NexusProvider.ClaimOpen = Nothing
        Dim m_sIsRecoveriesReadOnly As String
        Shared index As Integer
        Dim nClaimPerilId As Integer

        Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
            If Page.IsValid Then
                Dim screen As String = Nothing
                Dim iPeril As Integer = CInt(Request("PerilIndex"))
                Dim oClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                screen = Request("FromPage")

                Select Case CType(Session(CNMode), Mode)
                    Case Mode.NewClaim
                        If screen = "SA" Then
                            oClaim.ClaimPeril(iPeril).SalvageRecovery(index).InitialRecovery = Math.Round(CDec(InitialReserve), 2)
                            oClaim.ClaimPeril(iPeril).SalvageRecovery(index).RevisedRecovery = Math.Round(CDec(RevisedReserve), 2)
                            oClaim.ClaimPeril(iPeril).SalvageRecovery(index).CurrentRecovery = Math.Round(CDec((InitialReserve + ThisRevision)), 2)
                            oClaim.ClaimPeril(iPeril).SalvageRecovery(index).RevisionAmount = Math.Round(CDec(ThisRevision), 2)
                        ElseIf screen = "TP" Then
                            lblSalvageRecoveryReserve.Text = GetLocalResourceObject("lbl_ThirdPartyRecoveryReserve")
                            oClaim.ClaimPeril(iPeril).TPRecovery(index).InitialRecovery = Math.Round(CDec(InitialReserve), 2)
                            oClaim.ClaimPeril(iPeril).TPRecovery(index).RevisedRecovery = Math.Round(CDec(RevisedReserve), 2)
                            oClaim.ClaimPeril(iPeril).TPRecovery(index).CurrentRecovery = Math.Round(CDec((InitialReserve + ThisRevision)), 2)
                            oClaim.ClaimPeril(iPeril).TPRecovery(index).RevisionAmount = Math.Round(CDec(ThisRevision), 2)
                        End If
                    Case Mode.EditClaim
                        If screen = "SA" Then
                            oClaim.ClaimPeril(iPeril).SalvageRecovery(index).InitialRecovery = Math.Round(CDec(InitialReserve), 2)
                            oClaim.ClaimPeril(iPeril).SalvageRecovery(index).RevisedRecovery = Math.Round(CDec(RevisedReserve), 2)
                            oClaim.ClaimPeril(iPeril).SalvageRecovery(index).CurrentRecovery = Math.Round(CDec((InitialReserve + RevisedReserve + ThisRevision)), 2)
                            oClaim.ClaimPeril(iPeril).SalvageRecovery(index).RevisionAmount = Math.Round(CDec(ThisRevision), 2)
                        ElseIf screen = "TP" Then
                            lblSalvageRecoveryReserve.Text = GetLocalResourceObject("lbl_ThirdPartyRecoveryReserve")
                            oClaim.ClaimPeril(iPeril).TPRecovery(index).InitialRecovery = Math.Round(CDec(InitialReserve), 2)
                            oClaim.ClaimPeril(iPeril).TPRecovery(index).RevisedRecovery = Math.Round(CDec(RevisedReserve), 2)
                            oClaim.ClaimPeril(iPeril).TPRecovery(index).CurrentRecovery = Math.Round(CDec((InitialReserve + RevisedReserve + ThisRevision)), 2)
                            oClaim.ClaimPeril(iPeril).TPRecovery(index).RevisionAmount = Math.Round(CDec(ThisRevision), 2)
                        End If
                End Select

                Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_updated('" & Request.QueryString("PostbackTo") & "');", True)

            End If
        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
        End Sub

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Request.QueryString("PerilID") IsNot Nothing Then
                nClaimPerilId = CInt(Request.QueryString("PerilID"))
            End If
            If Not IsPostBack Then
                txtInitialReserve.Attributes.Add("readonly", "readonly")
                txtRevisedReserve.Attributes.Add("readonly", "readonly")
                txtReceipts.Attributes.Add("readonly", "readonly")
                txtreserveAfterReceipt.Attributes.Add("readonly", "readonly")
                txtReserveAfterRevision.Attributes.Add("readonly", "readonly")
                txtTotalReserve.Attributes.Add("readonly", "readonly")
                'To set the Focus
                Page.SetFocus(txtThisRevision)

                Dim screen As String = Request("FromPage")
                Dim iPeril As Integer = CInt(Request("PerilIndex"))
                Dim oClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                Dim oRecoveryCollection As NexusProvider.PerilRecoveryCollection = Nothing
                Dim oClaimRiskLinkCollection As NexusProvider.ClaimRiskLinkCollection = Nothing
                Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                m_sIsRecoveriesReadOnly = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsRecoveriesReadOnly, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)

                oClaimRiskLinkCollection = GetRecovery(oClaim.RiskKey)

                'Find the index of the requested recovery reserve
                Dim iCount As Integer
                Dim sTypeCode As String = Request("TypeCode")
                Dim nRecoveryPartyKey As Integer = 0
                If Request("RecoveryPartyKey") IsNot Nothing Then
                    nRecoveryPartyKey = CInt(Request("RecoveryPartyKey"))
                End If
                If screen = "SA" Then
                    lblSalvageRecoveryReserve.Text = GetLocalResourceObject("lbl_SalvageRecoveryReserve")
                    lblTitle.Text = GetLocalResourceObject("lbl_SalvageRecoveryForEdit_Title")
                    For iCount = 0 To oClaim.ClaimPeril(iPeril).SalvageRecovery.Count - 1
                        If sTypeCode.Trim.ToUpper = oClaim.ClaimPeril(iPeril).SalvageRecovery(iCount).TypeCode.Trim.ToUpper _
                            AndAlso oClaim.ClaimPeril(iPeril).SalvageRecovery(iCount).RecoveryPartyKey = nRecoveryPartyKey Then
                            index = iCount
                            Exit For
                        End If
                    Next
                ElseIf screen = "TP" Then
                    lblTitle.Text = GetLocalResourceObject("lbl_ThirdPartyRecoveryForEdit_Title")
                    lblSalvageRecoveryReserve.Text = GetLocalResourceObject("lbl_ThirdPartyRecoveryReserve")
                    For iCount = 0 To oClaim.ClaimPeril(iPeril).TPRecovery.Count - 1
                        If sTypeCode.Trim.ToUpper = oClaim.ClaimPeril(iPeril).TPRecovery(iCount).TypeCode.Trim.ToUpper _
                            AndAlso oClaim.ClaimPeril(iPeril).TPRecovery(iCount).RecoveryPartyKey = nRecoveryPartyKey _
                            AndAlso (oClaim.ClaimPeril(iPeril).ClaimPerilKey = oClaim.ClaimPeril(iPeril).TPRecovery(iCount).ClaimPerilId Or nClaimPerilId = oClaim.ClaimPeril(iPeril).TPRecovery(iCount).ClaimPerilId) Then
                            index = iCount
                            Exit For
                        End If
                    Next
                End If

                ' Display party info as read-only
                Dim oRecoveryForParty As NexusProvider.PerilRecovery = Nothing
                If screen = "SA" AndAlso index < oClaim.ClaimPeril(iPeril).SalvageRecovery.Count Then
                    oRecoveryForParty = oClaim.ClaimPeril(iPeril).SalvageRecovery(index)
                ElseIf screen = "TP" AndAlso index < oClaim.ClaimPeril(iPeril).TPRecovery.Count Then
                    oRecoveryForParty = oClaim.ClaimPeril(iPeril).TPRecovery(index)
                End If
                If oRecoveryForParty IsNot Nothing Then
                    lblPartyTypeValue.Text = If(oRecoveryForParty.RecoveryPartyTypeCode, String.Empty)
                    lblPartyValue.Text = If(oRecoveryForParty.PartyShortName, String.Empty)
                End If

                If screen = "SA" Then
                    oRecoveryCollection = oClaim.ClaimPeril(iPeril).SalvageRecovery
                    If oClaimRiskLinkCollection(0).RecoveryItemType IsNot Nothing Then
                        For Each oRecoveryItem As NexusProvider.RecoveryType In oClaimRiskLinkCollection(0).RecoveryItemType
                            If oRecoveryItem.IsSalvage = 1 Then
                                ddlRecoveryType.Items.Add(New ListItem(oRecoveryItem.Description.Trim, oRecoveryItem.Code.Trim))
                            End If
                        Next
                    End If
                ElseIf screen = "TP" Then
                    lblTitle.Text = GetLocalResourceObject("lbl_ThirdPartyRecoveryForEdit_Title")
                    lblSalvageRecoveryReserve.Text = GetLocalResourceObject("lbl_ThirdPartyRecoveryReserve")
                    oRecoveryCollection = oClaim.ClaimPeril(iPeril).TPRecovery
                    If oClaimRiskLinkCollection(0).RecoveryItemType IsNot Nothing Then
                        For Each oRecoveryItem As NexusProvider.RecoveryType In oClaimRiskLinkCollection(0).RecoveryItemType
                            If oRecoveryItem.IsSalvage = 0 Then
                                ddlRecoveryType.Items.Add(New ListItem(oRecoveryItem.Description.Trim, oRecoveryItem.Code.Trim))
                            End If
                        Next
                    End If
                End If
                Select Case CType(Session(CNMode), Mode)
                    Case Mode.NewClaim
                        txtInitialReserve.Text = FormatNumber(oRecoveryCollection(index).InitialRecovery, 2)
                        ddlRecoveryType.SelectedIndex = ddlRecoveryType.Items.IndexOf(ddlRecoveryType.Items.FindByValue((oRecoveryCollection(index).TypeCode.Trim())))
                        txtRevisedReserve.Text = FormatNumber(oRecoveryCollection(index).RevisedRecovery, 2)
                        txtThisRevision.Text = FormatNumber(oRecoveryCollection(index).RevisionAmount, 2)
                        txtTotalReserve.Text = FormatNumber(oRecoveryCollection(index).InitialRecovery + oRecoveryCollection(index).RevisedRecovery + oRecoveryCollection(index).RevisionAmount, 2)

                        txtThisRevision.Attributes.Add("onblur", "javascript:return CalculateRecovery();")
                        txtThisRevision.Attributes.Add("OnMouseOut", "javascript:return CalculateRecovery();")
                    Case Mode.EditClaim
                        If screen = "SA" Then
                            txtInitialReserve.Text = FormatNumber(oRecoveryCollection(index).InitialRecovery, 2)
                            ddlRecoveryType.SelectedIndex = ddlRecoveryType.Items.IndexOf(ddlRecoveryType.Items.FindByValue((oRecoveryCollection(index).TypeCode.Trim())))
                            txtReceipts.Text = FormatNumber(oRecoveryCollection(index).ReceiptedAmount, 2)
                            txtRevisedReserve.Text = FormatNumber(oRecoveryCollection(index).RevisedRecovery, 2)
                            txtThisRevision.Text = FormatNumber(oRecoveryCollection(index).RevisionAmount, 2)
                            txtTotalReserve.Text = FormatNumber(oRecoveryCollection(index).InitialRecovery + oRecoveryCollection(index).RevisedRecovery + oRecoveryCollection(index).RevisionAmount, 2)
                        ElseIf screen = "TP" Then
                            txtInitialReserve.Text = FormatNumber(oRecoveryCollection(index).InitialRecovery, 2)
                            txtReceipts.Text = FormatNumber(oRecoveryCollection(index).ReceiptedAmount, 2)
                            ddlRecoveryType.SelectedIndex = ddlRecoveryType.Items.IndexOf(ddlRecoveryType.Items.FindByValue((oRecoveryCollection(index).TypeCode.Trim())))
                            txtRevisedReserve.Text = FormatNumber(oRecoveryCollection(index).RevisedRecovery, 2)
                            txtThisRevision.Text = FormatNumber(oRecoveryCollection(index).RevisionAmount, 2)
                            txtTotalReserve.Text = FormatNumber(oRecoveryCollection(index).InitialRecovery + oRecoveryCollection(index).RevisedRecovery + oRecoveryCollection(index).RevisionAmount, 2)

                        End If
                        txtThisRevision.Attributes.Add("onblur", "javascript:return CalculateRecovery();")
                        txtThisRevision.Attributes.Add("OnMouseOut", "javascript:return CalculateRecovery();")
                End Select
            End If
            txtThisRevision.Attributes.Add("onkeypress", "javascript:return isInteger(event);")
            If m_sIsRecoveriesReadOnly = "1" Then
                txtThisRevision.ReadOnly = True
                btnOk.Visible = False
            End If
            Page.ClientScript.RegisterStartupScript(GetType(String), "CalculateOnLoad", "CalculateRecovery();", True)
        End Sub

        Public Property ThisRevision() As Double
            Get
                Dim dResult As Double
                If Double.TryParse(txtThisRevision.Text.Trim, dResult) = True Then
                    Return txtThisRevision.Text
                Else
                    Return "0.00"
                End If
            End Get
            Set(ByVal value As Double)
                Dim dResult As Double
                If Double.TryParse(txtThisRevision.Text.Trim, dResult) = True Then
                    txtThisRevision.Text = value
                Else
                    txtThisRevision.Text = "0.00"
                End If
            End Set
        End Property

        Public Property RevisedReserve() As Double
            Get
                Return txtRevisedReserve.Text
            End Get
            Set(ByVal value As Double)
                txtRevisedReserve.Text = value
            End Set
        End Property

        Public Property InitialReserve() As Double
            Get
                Return txtInitialReserve.Text
            End Get
            Set(ByVal value As Double)
                txtInitialReserve.Text = value
            End Set
        End Property

        Public Property Receipts() As Double
            Get
                Return txtReceipts.Text
            End Get
            Set(ByVal value As Double)
                txtReceipts.Text = value
            End Set
        End Property

        Public Property reserveAfterReceipt() As Double
            Get
                Return txtreserveAfterReceipt.Text
            End Get
            Set(ByVal value As Double)
                txtreserveAfterReceipt.Text = value
            End Set
        End Property

        Public Property ReserveAfterRevision() As Double
            Get
                Return txtReserveAfterRevision.Text
            End Get
            Set(ByVal value As Double)
                txtReserveAfterRevision.Text = value
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
                If txtThisRevision.Text.Trim.Length = 0 Or txtThisRevision.Text.Trim = "0.00" Then
                    args.IsValid = False
                    CustValidType.ErrorMessage = GetLocalResourceObject("Err_InvalidRecoveryReserve")
                End If
            End If
        End Sub
    End Class
End Namespace