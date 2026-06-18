Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports NexusProvider.SAMForInsurance
Imports System.Collections.Generic
Imports Nexus.Utils
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports CMS.Library
Imports System.Xml.XmlReader
Imports System.Xml.XPath
Imports System.Xml
Namespace Nexus
    Partial Class Claims_PayClaim
        Inherits CMS.Library.Frontend.clsCMSPage

#Region " Page Events "


        ''' <summary>
        ''' Page_Load method
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'if user is trying to access this page directly
            Dim oExGratiaOptionSettings As NexusProvider.OptionTypeSetting
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            If Session(CNMode) Is Nothing Then
                Response.Redirect(AppSettings("WebRoot") & "Login.aspx")
            End If
            'make Party ReadOnly
            txtParty.Attributes.Add("readonly", "readonly")

            If Not IsPostBack Then

                Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
                Dim oClaimOpen As NexusProvider.ClaimOpen = Nothing

                'Retreiving the claim quote information from session
                Dim oQuote As NexusProvider.Quote = CType(Session(CNClaimQuote), NexusProvider.Quote)
                If Session.Item(CNClaim) IsNot Nothing Then
                    'Retreiving the claim  information from session
                    oClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                    lblLossCurrency.Text = GetCurrencyForCode(oClaimOpen.CurrencyISOCode)
                    lblDateOfLoss.Text = oClaimOpen.LossToDate
                    lblPerilInfo.Text = oClaimOpen.ClaimPeril(iPeril).Description
                    If oQuote.BusinessTypeCode = "DIRECT" Then
                        rblPayee.Items(rblPayee.Items.IndexOf(rblPayee.Items.FindByText("Agent"))).Enabled = False
                    End If
                    lblRiskType.Text = oQuote.Risks(0).Description
                    rblPayee.Visible = True

                    If Session(CNMode) = Mode.PayClaim Then
                        
                        oExGratiaOptionSettings = oWebservice.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5114)

                        If (oExGratiaOptionSettings IsNot Nothing AndAlso String.IsNullOrEmpty(oExGratiaOptionSettings.OptionValue) = False) Then
                            chkExGratia.Visible = True
                        End If

                        'Clean the Value if exist in "Claim reserve"
                        For iPerilCount As Integer = 0 To oClaimOpen.ClaimPeril.Count - 1
                            If oClaimOpen.ClaimPeril(iPerilCount).ClaimReserve IsNot Nothing AndAlso oClaimOpen.ClaimPeril(iPerilCount).ClaimReserve.Count > 0 Then
                                For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPerilCount).ClaimReserve.Count - 1
                                    oClaimOpen.ClaimPeril(iPerilCount).ClaimReserve.Remove(0)
                                Next
                            End If
                        Next

                        'Populating the reserve item for each peril
                        PopulateReserveItem()

                        rblPayee.Items(0).Text = GetLocalResourceObject("li_ClaimPayable")
                        ltPageHeading.Text = GetLocalResourceObject("lbl_PaymentDetails")
                        ltThisPayment.Text = GetLocalResourceObject("lt_ThisPayment")
                        'Populating the Payment
                        PopulatePayClaim(iPeril, oClaimOpen)
                        'Hide "Insurer" option in case of Payclaim
                        rblPayee.Items(4).Enabled = False

                    ElseIf Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                        ' ADO#39480 (T2): IsRecoveryInstalmentEnabled check will replace this stub
                        btnInstalments.Visible = True
                        ltThisPayment.Text = GetLocalResourceObject("gvPaymentDetails_Header")
                        rblPayee.Items(0).Text = GetLocalResourceObject("li_ClaimReceivable")
                        ltThisPayment.Text = GetLocalResourceObject("ltThisReceipt")
                        'Retreival of Description based on code (TypeCode)
                        For iCount As Integer = 0 To oClaimOpen.ClaimPeril.Count - 1
                            For jCount As Integer = 0 To oClaimOpen.ClaimPeril(iCount).SalvageRecovery.Count - 1
                                oClaimOpen.ClaimPeril(iCount).SalvageRecovery(jCount).Description = GetDescriptionForCode(NexusProvider.ListType.PMLookup, oClaimOpen.ClaimPeril(iCount).SalvageRecovery(jCount).TypeCode, "recovery_type")
                            Next
                            For jCount As Integer = 0 To oClaimOpen.ClaimPeril(iCount).TPRecovery.Count - 1
                                oClaimOpen.ClaimPeril(iCount).TPRecovery(jCount).Description = GetDescriptionForCode(NexusProvider.ListType.PMLookup, oClaimOpen.ClaimPeril(iCount).TPRecovery(jCount).TypeCode, "recovery_type")
                            Next
                        Next

                        'Populating the reserve item for each peril
                        If Session(CNMode) = Mode.SalvageClaim Then

                            For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).SalvageRecovery.Count - 1
                                Dim oClaimReceiptItemType As New NexusProvider.BaseClaimRecoveryReceiptType
                                oClaimOpen.ClaimPeril(iPeril).Receipt.ReceiptItem.Add(oClaimReceiptItemType)
                            Next
                        ElseIf Session(CNMode) = Mode.TPRecovery Then

                            For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).TPRecovery.Count - 1
                                Dim oClaimReceiptItemType As New NexusProvider.BaseClaimRecoveryReceiptType
                                oClaimOpen.ClaimPeril(iPeril).Receipt.ReceiptItem.Add(oClaimReceiptItemType)
                            Next
                        End If

                        'Populating the Salvage/TPRecovery
                        PopulateSalvageClaim(iPeril, oClaimOpen)

                        rblPayee.Items(4).Enabled = True
                    End If

                    'Fill Client Details
                    If oClaimOpen.Client IsNot Nothing Then
                        txtPayeeName.Text = oClaimOpen.Client.ShortName

                        Address.Address1 = oClaimOpen.Client.Address.Address1
                        Address.Address2 = oClaimOpen.Client.Address.Address2
                        Address.Address3 = oClaimOpen.Client.Address.Address3
                        Address.Address4 = oClaimOpen.Client.Address.Address4
                        Address.CountryCode = oClaimOpen.Client.Address.CountryCode
                        Address.Postcode = oClaimOpen.Client.Address.PostCode
                    End If
                    'Set the Payee if Already Selected
                    If Session(CNMode) = Mode.SalvageClaim Then
                        For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).SalvageRecovery.Count - 1
                            If oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iCount).PartyReceiptCode = "CLMRECEIVABLE" Then
                                rblPayee.SelectedValue = "0"
                                txtParty.Text = "CLMRECEIVABLE"

                            ElseIf oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iCount).ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.PARTY Then
                                rblPayee.SelectedValue = "1"
                                txtParty.Text = oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iCount).PartyReceiptCode
                                btnParty.Enabled = True


                            ElseIf oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iCount).ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.AGENT Then
                                rblPayee.SelectedValue = "2"
                                txtParty.Text = oQuote.AgentCode


                            ElseIf oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iCount).ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.CLIENT Then
                                rblPayee.SelectedValue = "3"
                                txtParty.Text = oQuote.InsuredName


                            End If
                        Next
                    ElseIf Session(CNMode) = Mode.TPRecovery Then
                        For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).TPRecovery.Count - 1
                            If oClaimOpen.ClaimPeril(iPeril).TPRecovery(iCount).PartyReceiptCode = "CLMRECEIVABLE" Then
                                rblPayee.SelectedValue = "0"
                                txtParty.Text = "CLMRECEIVABLE"


                            ElseIf oClaimOpen.ClaimPeril(iPeril).TPRecovery(iCount).ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.PARTY Then
                                rblPayee.SelectedValue = "1"
                                txtParty.Text = oClaimOpen.ClaimPeril(iPeril).TPRecovery(iCount).PartyReceiptCode
                                btnParty.Enabled = True

                            ElseIf oClaimOpen.ClaimPeril(iPeril).TPRecovery(iCount).ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.AGENT Then
                                rblPayee.SelectedValue = "2"
                                txtParty.Text = oQuote.AgentCode

                            ElseIf oClaimOpen.ClaimPeril(iPeril).TPRecovery(iCount).ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.CLIENT Then
                                rblPayee.SelectedValue = "3"
                                txtParty.Text = oQuote.InsuredName

                            End If
                        Next
                    Else
                        If oClaimOpen.ClaimPeril(iPeril).Payment.PartyPaidCode IsNot Nothing Then
                            If oClaimOpen.ClaimPeril(iPeril).Payment.PartyPaidCode = NexusProvider.ClaimPaymentPartyTypeType.CLMPAYABLE Then
                                rblPayee.SelectedValue = "0"
                                txtParty.Text = "CLMPAYABLE"
                            End If
                        ElseIf oClaimOpen.ClaimPeril(iPeril).Payment.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.PARTY Then
                            rblPayee.SelectedValue = "1"
                            txtParty.Text = oClaimOpen.ClaimPeril(iPeril).Payment.PartyPaidCode
                            btnParty.Enabled = True

                        ElseIf oClaimOpen.ClaimPeril(iPeril).Payment.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.AGENT Then
                            rblPayee.SelectedValue = "2"
                            txtParty.Text = oQuote.AgentCode

                        ElseIf oClaimOpen.ClaimPeril(iPeril).Payment.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.CLIENT Then
                            rblPayee.SelectedValue = "3"
                            txtParty.Text = oQuote.InsuredName

                        End If

                        If oClaimOpen.ClaimPeril(iPeril).Payment.IsExGratia = True Then
                            chkExGratia.Checked = True
                        End If

                    End If

                Else
                    Response.Redirect("FindClaim.aspx")
                End If
                gvPaymentDetails.Enabled = False
                gvSalvageDetails.Enabled = False
            ElseIf Request("__EVENTARGUMENT") = "PaymentUpdation" Then
                'check if the postback has been triggered by the modal dialog "PaymentDetails"
                Dim PerilIndex As Integer = CInt(Session(CNClaimPerilIndex))
                Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                If Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                    PopulateSalvageClaim(PerilIndex, oClaimOpen)

                ElseIf Session(CNMode) = Mode.PayClaim Then
                    PopulatePayClaim(PerilIndex, oClaimOpen)
                End If
                PopulateThisPayment(PerilIndex, oClaimOpen)
            End If

            'Check MediaTypeFieldMandatory on Claim Payment       
            CheckMediaTypeFieldMandatory()
            If HidMediaTypeFieldMandatory.Value = "1" Then
                'Mandatory Field active 
                'emMediatype.Visible = True
                GISLookup_MediaType.CssClass = GISLookup_MediaType.CssClass & " field-mandatory"
            End If
        End Sub

#End Region

#Region " GridView Events "

        Protected Sub gvPaymentDetails_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPaymentDetails.Load
            If gvPaymentDetails.PageCount = 1 Then
                gvPaymentDetails.AllowPaging = False
            End If
        End Sub
        ''' <summary>
        ''' On change of page index
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvPaymentDetails_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPaymentDetails.PageIndexChanging
            Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
            Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            gvPaymentDetails.PageIndex = e.NewPageIndex
            PopulatePayClaim(iPeril, oClaimOpen)
        End Sub
        ''' <summary>
        ''' On selection of different row command
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvPaymentDetails_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPaymentDetails.RowCommand
            If e.CommandName = "Lock" Then
                Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
                Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                ViewState("IsLockPayment") = 1
                oClaimOpen.ClaimPeril(iPeril).ClaimReserve(e.CommandArgument).IsLocked = True
                PopulatePayClaim(iPeril, oClaimOpen)
            End If
        End Sub
        ''' <summary>
        ''' On Payment grid RowDataBound
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvPaymentDetails_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPaymentDetails.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim sUrl As String = Nothing
                If HttpContext.Current.Session.IsCookieless Then
                    sUrl = "' " & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/PaymentDetails.aspx?PostbackTo=" & updThisPayment.ClientID.ToString & "&modal=true&KeepThis=true&PaymentIndex=" & e.Row.DataItemIndex & "&TB_iframe=true&height=500&width=700'"
                Else
                    sUrl = "' " & AppSettings("WebRoot") & "/Modal/PaymentDetails.aspx?PostbackTo=" & updThisPayment.ClientID.ToString & "&modal=true&KeepThis=true&PaymentIndex=" & e.Row.DataItemIndex & "&TB_iframe=true&height=500&width=700'"
                End If

                'Check whether Excess Reserve is accepted first or not
                'if it is not accepted first then it has to ask the Reset Confirmation
                Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
                Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                Dim bFirstElement As Boolean = True
                If CType(e.Row.DataItem, NexusProvider.ClaimPerilReservePaymentType).IsExcess = True Then
                    For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).ClaimReserve.Count - 1
                        If oClaimOpen.ClaimPeril(iPeril).ClaimReserve(iCount).IsExcess = False _
                        AndAlso oClaimOpen.ClaimPeril(iPeril).ClaimReserve(iCount).PayQueue > 0 Then
                            bFirstElement = False
                        End If
                    Next
                End If
                If bFirstElement = False Then
                    'Reset confirmation
                    CType(e.Row.FindControl("hypEditPayment"), LinkButton).OnClientClick = "javascript:setConfirmation('" & GetLocalResourceObject("msg_ResetConfirm").ToString() & "',1," & sUrl & ");return false;"
                Else
                    CType(e.Row.FindControl("hypEditPayment"), LinkButton).OnClientClick = "javascript:setConfirmation('" & GetLocalResourceObject("msg_ResetConfirm").ToString() & "',0," & sUrl & ");return false;"
                End If
                ''''''''
            End If
        End Sub
        ''' <summary>
        ''' Radio button Selected Index Change
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub rblPayee_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblPayee.SelectedIndexChanged

            Dim oQuote As NexusProvider.Quote = CType(Session(CNClaimQuote), NexusProvider.Quote)
            Select Case rblPayee.SelectedValue
                Case "0"
                    If Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                        txtParty.Text = "CLMRECEIVABLE"
                    ElseIf Session(CNMode) = Mode.PayClaim Then
                        txtParty.Text = "CLMPAYABLE"
                    End If
                    btnParty.Enabled = False
                    '  ReqPartyKey.IsValid = True
                    '  ReqPartyKey.Enabled = False
                Case "1"
                    txtParty.Text = String.Empty
                    btnParty.Enabled = True
                    If HttpContext.Current.Session.IsCookieless Then
                        btnParty.OnClientClick = "tb_show(null , '" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/FindOtherParty.aspx?modal=true&Type=All&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                    Else
                        btnParty.OnClientClick = "tb_show(null , '../Modal/FindOtherParty.aspx?modal=true&Type=All&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                    End If
                    ' ReqPartyKey.Enabled = True
                Case "2"
                    txtParty.Text = oQuote.AgentCode
                    btnParty.Enabled = False
                    'ReqPartyKey.IsValid = True
                    'ReqPartyKey.Enabled = False
                Case "3"
                    txtParty.Text = oQuote.InsuredName
                    btnParty.Enabled = False
                    'ReqPartyKey.IsValid = True
                    'ReqPartyKey.Enabled = False
                Case "4"
                    txtParty.Text = String.Empty
                    btnParty.Enabled = True
                    If HttpContext.Current.Session.IsCookieless Then
                        btnParty.OnClientClick = "tb_show(null , '" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/FindReinsurer.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=700' , null);return false;"
                    Else
                        btnParty.OnClientClick = "tb_show(null , '../Modal/FindReinsurer.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=700' , null);return false;"
                    End If
                    'ReqPartyKey.Enabled = True
            End Select
            gvPaymentDetails.Enabled = True
            gvSalvageDetails.Enabled = True
        End Sub

#End Region

#Region " Button Events "
        ''' <summary>
        ''' On OK button
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
            If Page.IsValid Then
                Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
                Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
                'Retreval of the quote information from session
                Dim oQuote As NexusProvider.Quote = CType(Session(CNClaimQuote), NexusProvider.Quote)
                Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)

                If Session(CNMode) = Mode.PayClaim Then
                    oClaimOpen.ClaimPeril(iPeril).Payment.BaseClaimKey = oClaimOpen.BaseClaimKey
                    oClaimOpen.ClaimPeril(iPeril).Payment.BaseClaimPerilKey = oClaimOpen.ClaimPeril(iPeril).BaseClaimPerilKey
                    oClaimOpen.ClaimPeril(iPeril).Payment.PartyPaidName = Trim(txtParty.Text)
                    'Updation of the Payment object with values
                    With oClaimOpen.ClaimPeril(iPeril).Payment.Payee
                        .MediaReference = Trim(txtMediaRef.Text)
                        .MediaTypeCode = GISLookup_MediaType.Value
                        .Name = Trim(txtPayeeName.Text)
                        .TheirReference = Trim(txtThisReference.Text)
                        .Comments = Trim(txtComments.Text)
                        .BankCode = Trim(txtBankCode.Text)
                        .BankName = Trim(txtBankName.Text)
                        .BankNumber = Trim(txtBankAccNumber.Text)
                        .Address.Address1 = Address.Address1
                        .Address.Address2 = Address.Address2
                        .Address.Address3 = Address.Address3
                        .Address.Address4 = Address.Address4
                        .Address.AddressType = NexusProvider.AddressType.CorrespondenceAddress
                        .Address.CountryCode = Address.CountryCode
                        .Address.CountryDescription = Address.Country
                        .Address.PostCode = Address.Postcode
                        If chkExGratia.Checked = True Then
                            oClaimOpen.ClaimPeril(iPeril).Payment.IsExGratia = True
                        End If

                    End With

                Else
                    Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim bTimeStamp As Byte() = CType(Session(CNClaimTimeStamp), Byte())
                    Dim oClaimReceipt As New NexusProvider.ClaimReceipt
                    'Updation of the Receipt object with values
                    With oClaimOpen.ClaimPeril(iPeril).Receipt
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
                        .Payee.BankCode = Trim(txtBankCode.Text)
                        .Payee.BankName = Trim(txtBankName.Text)
                        .Payee.BankNumber = Trim(txtBankAccNumber.Text)
                        .Payee.Comments = Trim(txtComments.Text)
                        .Payee.MediaReference = Trim(txtMediaRef.Text)
                        .Payee.MediaTypeCode = GISLookup_MediaType.Value
                        .Payee.Name = Trim(txtPayeeName.Text)
                        .Payee.TheirReference = Trim(txtThisReference.Text)

                    End With
                End If
                'Storing the radion button values in Receipt/Payment Object i.e Payee
                Select Case rblPayee.SelectedValue
                    Case "0"
                        If Session(CNMode) = Mode.SalvageClaim Then
                            For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).SalvageRecovery.Count - 1
                                oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iCount).ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.CLMRECEIVABLE
                                oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iCount).PartyReceiptCode = "CLMRECEIVABLE"
                                oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iCount).IsSalvage = 1
                            Next
                            With oClaimOpen.ClaimPeril(iPeril).Receipt
                                .ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.CLMRECEIVABLE
                                .PartyReceiptCode = "CLMRECEIVABLE"
                            End With
                        ElseIf Session(CNMode) = Mode.TPRecovery Then
                            For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).TPRecovery.Count - 1
                                oClaimOpen.ClaimPeril(iPeril).TPRecovery(iCount).ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.CLMRECEIVABLE
                                oClaimOpen.ClaimPeril(iPeril).TPRecovery(iCount).PartyReceiptCode = "CLMRECEIVABLE"
                                oClaimOpen.ClaimPeril(iPeril).TPRecovery(iCount).IsSalvage = 0
                            Next
                            With oClaimOpen.ClaimPeril(iPeril).Receipt
                                .ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.CLMRECEIVABLE
                                .PartyReceiptCode = "CLMRECEIVABLE"
                            End With
                        Else
                            oClaimOpen.ClaimPeril(iPeril).Payment.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.CLMPAYABLE
                            oClaimOpen.ClaimPeril(iPeril).Payment.PartyPaidCode = "CLMPAYABLE"
                        End If

                    Case "1"
                        'Party
                        If Session(CNMode) = Mode.SalvageClaim Then
                            For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).SalvageRecovery.Count - 1
                                oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iCount).ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.PARTY
                                oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iCount).PartyReceiptCode = txtParty.Text.Trim
                                oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iCount).IsSalvage = 1
                            Next
                            With oClaimOpen.ClaimPeril(iPeril).Receipt
                                .ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.PARTY
                                .PartyReceiptCode = txtParty.Text.Trim
                                .PartyKey = CInt(hPartyKey.Value.Trim)
                            End With
                        ElseIf Session(CNMode) = Mode.TPRecovery Then
                            For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).TPRecovery.Count - 1
                                oClaimOpen.ClaimPeril(iPeril).TPRecovery(iCount).ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.PARTY
                                oClaimOpen.ClaimPeril(iPeril).TPRecovery(iCount).PartyReceiptCode = txtParty.Text.Trim
                                oClaimOpen.ClaimPeril(iPeril).TPRecovery(iCount).IsSalvage = 0
                            Next
                            With oClaimOpen.ClaimPeril(iPeril).Receipt
                                .ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.PARTY
                                .PartyReceiptCode = txtParty.Text.Trim
                                .PartyKey = CInt(hPartyKey.Value.Trim)
                            End With
                        Else
                            oClaimOpen.ClaimPeril(iPeril).Payment.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.PARTY
                            oClaimOpen.ClaimPeril(iPeril).Payment.PartyKey = CInt(hPartyKey.Value.Trim)
                            oClaimOpen.ClaimPeril(iPeril).Payment.PartyPaidCode = txtParty.Text.Trim
                        End If
                    Case "2"
                        'Agent
                        If Session(CNMode) = Mode.SalvageClaim Then
                            For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).SalvageRecovery.Count - 1
                                oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iCount).ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.AGENT
                                oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iCount).PartyReceiptCode = oQuote.AgentCode
                                oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iCount).IsSalvage = 1
                            Next
                            With oClaimOpen.ClaimPeril(iPeril).Receipt
                                .ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.AGENT
                                .PartyReceiptCode = oQuote.AgentCode
                            End With
                        ElseIf Session(CNMode) = Mode.TPRecovery Then
                            For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).TPRecovery.Count - 1
                                oClaimOpen.ClaimPeril(iPeril).TPRecovery(iCount).ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.AGENT
                                oClaimOpen.ClaimPeril(iPeril).TPRecovery(iCount).PartyReceiptCode = oQuote.AgentCode
                                oClaimOpen.ClaimPeril(iPeril).TPRecovery(iCount).IsSalvage = 0
                            Next
                            With oClaimOpen.ClaimPeril(iPeril).Receipt
                                .ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.AGENT
                                .PartyReceiptCode = oQuote.AgentCode
                            End With
                        Else
                            oClaimOpen.ClaimPeril(iPeril).Payment.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.AGENT
                            oClaimOpen.ClaimPeril(iPeril).Payment.PartyPaidCode = oQuote.AgentCode
                        End If
                    Case "3"
                        'Client
                        If Session(CNMode) = Mode.SalvageClaim Then
                            For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).SalvageRecovery.Count - 1
                                oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iCount).ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.CLIENT
                                oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iCount).PartyReceiptCode = oClaimOpen.ClientShortName
                                oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iCount).IsSalvage = 1
                            Next
                            With oClaimOpen.ClaimPeril(iPeril).Receipt
                                .ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.CLIENT
                                .PartyReceiptCode = oClaimOpen.ClientShortName
                            End With
                        ElseIf Session(CNMode) = Mode.TPRecovery Then
                            For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).TPRecovery.Count - 1
                                oClaimOpen.ClaimPeril(iPeril).TPRecovery(iCount).ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.CLIENT
                                oClaimOpen.ClaimPeril(iPeril).TPRecovery(iCount).PartyReceiptCode = oClaimOpen.ClientShortName
                                oClaimOpen.ClaimPeril(iPeril).TPRecovery(iCount).IsSalvage = 1
                            Next
                            With oClaimOpen.ClaimPeril(iPeril).Receipt
                                .ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.CLIENT
                                .PartyReceiptCode = oClaimOpen.ClientShortName
                            End With
                        Else
                            oClaimOpen.ClaimPeril(iPeril).Payment.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.CLIENT
                            oClaimOpen.ClaimPeril(iPeril).Payment.PartyPaidCode = oClaimOpen.ClientShortName
                        End If

                    Case "4"
                        'Insurer
                        If Session(CNMode) = Mode.SalvageClaim Then
                            For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).SalvageRecovery.Count - 1
                                oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iCount).ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.PARTY
                                oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iCount).PartyReceiptCode = txtParty.Text.Trim
                                oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iCount).IsSalvage = 1
                            Next
                            With oClaimOpen.ClaimPeril(iPeril).Receipt
                                .ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.PARTY
                                .PartyReceiptCode = txtParty.Text.Trim
                                .PartyKey = CInt(hPartyKey.Value.Trim)
                            End With
                        ElseIf Session(CNMode) = Mode.TPRecovery Then
                            For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).TPRecovery.Count - 1
                                oClaimOpen.ClaimPeril(iPeril).TPRecovery(iCount).ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.PARTY
                                oClaimOpen.ClaimPeril(iPeril).TPRecovery(iCount).PartyReceiptCode = txtParty.Text.Trim
                                oClaimOpen.ClaimPeril(iPeril).TPRecovery(iCount).IsSalvage = 0
                            Next
                            With oClaimOpen.ClaimPeril(iPeril).Receipt
                                .ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.PARTY
                                .PartyReceiptCode = txtParty.Text.Trim
                                .PartyKey = CInt(hPartyKey.Value.Trim)
                            End With
                        End If
                End Select
                'Update the data into session
                Session(CNClaim) = oClaimOpen
                If Session(CNMode) = Mode.PayClaim Then
                    If Session(CNPayClaim) Is Nothing Then
                        'Update the payments through UpdateClaimReserveOrPayments
                        Dim oPayClaimResponse As NexusProvider.PayClaimResponse = Nothing
                        Dim oPayment As NexusProvider.ClaimPayment = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(Session(CNClaimPerilIndex)).Payment
                        Dim oInitialClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                        Dim iPerilIndex As Integer = Request.QueryString("PerilIndex")
                        Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                        Dim bTimeStamp As Byte() = CType(Session(CNClaimTimeStamp), Byte())

                        oPayment.ClaimVersionDescription = Session(CNChangeReason)
                        'To Skip the posting
                        oPayment.PaymentOnly = True
                        'Arch issue 268
                        'oPayClaimResponse = oWebservice.PayClaim(oPayment, bTimeStamp)
                        oPayClaimResponse = PayClaimCall(oPayment, bTimeStamp)
                        If oPayClaimResponse Is Nothing Then
                            Exit Sub
                        End If
                        Session(CNPayClaim) = oPayClaimResponse
                    Else
                        'Update the Reserve
                        UpdatePaymentData()
                    End If

                    'To check whether Peril builder screen is configured or not, if configured then user will move to the 
                    'peril builder screen otherwise to the Perils.aspx page
                    Dim sPerilConfig As String
                    Dim sResult As String
                    Dim sScreenCode As String
                    Dim sFolder As String = String.Empty

                    If oNexusConfig.Portals.Portal(Portal.GetPortalID()).Claims.PerilTypes.PerilType(Trim(oClaimOpen.ClaimPeril(iPeril).TypeCode)) IsNot Nothing Then
                        sFolder = AppSettings("WebRoot") & "Claims/ClientPages/" & oNexusConfig.Portals.Portal(Portal.GetPortalID()).Claims.ScreenLocation & "/Perils/" _
                                          & oNexusConfig.Portals.Portal(Portal.GetPortalID()).Claims.PerilTypes.PerilType(oClaimOpen.ClaimPeril(iPeril).TypeCode).Folder

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
                        Exit Sub
                    End If
                End If

                If Session(CNMode) = Mode.PayClaim Then
                    'Update the Paid Amount here
                    'calculate total amount paid
                    Dim dPaidAmount As Decimal
                    For i As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).ClaimReserve.Count - 1
                        dPaidAmount = dPaidAmount + oClaimOpen.ClaimPeril(iPeril).ClaimReserve(i).ThisPaymentINCLTax
                    Next
                    oClaimOpen.ClaimPeril(iPeril).PaidAmount += dPaidAmount
                End If

                'Update the data into session
                Session(CNClaim) = oClaimOpen
                Session(CNEnablePayClaim) = True
                Response.Redirect("~/claims/perils.aspx")
            End If
        End Sub

#End Region

#Region " Private Methods "
        ''' <summary>
        ''' It populates the payment grid
        ''' </summary>
        ''' <param name="iPerilIndex"></param>
        ''' <param name="oClaimOpen"></param>
        ''' <remarks></remarks>
        Protected Sub PopulatePayClaim(ByVal iPerilIndex As Integer, ByVal oClaimOpen As NexusProvider.ClaimOpen)
            Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
            gvPaymentDetails.Visible = True

            gvPaymentDetails.DataSource = oClaimOpen.ClaimPeril(iPeril).ClaimReserve
            gvPaymentDetails.DataBind()
        End Sub
        ''' <summary>
        ''' It populates the Salvage/TPRecovery grid
        ''' </summary>
        ''' <param name="iPerilIndex"></param>
        ''' <param name="oClaimOpen"></param>
        ''' <remarks></remarks>
        Protected Sub PopulateSalvageClaim(ByVal iPerilIndex As Integer, ByVal oClaimOpen As NexusProvider.ClaimOpen)
            Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
            gvSalvageDetails.Visible = True

            If Session(CNMode) = Mode.SalvageClaim Then
                gvSalvageDetails.DataSource = oClaimOpen.ClaimPeril(iPeril).SalvageRecovery
                gvSalvageDetails.DataBind()
            ElseIf Session(CNMode) = Mode.TPRecovery Then
                gvSalvageDetails.DataSource = oClaimOpen.ClaimPeril(iPeril).TPRecovery
                gvSalvageDetails.DataBind()
            End If

        End Sub
        ''' <summary>
        ''' It populates the "This Payment" section
        ''' </summary>
        ''' <param name="iPerilIndex"></param>
        ''' <param name="oClaimOpen"></param>
        ''' <remarks></remarks>
        Protected Sub PopulateThisPayment(ByVal iPerilIndex As Integer, ByVal oClaimOpen As NexusProvider.ClaimOpen)

            If Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                Dim amount As Decimal
                Dim tax As Decimal

                If oClaimOpen.ClaimPeril(iPerilIndex).Receipt IsNot Nothing Then
                    'Calculation of the amount and tax
                    If oClaimOpen.ClaimPeril(iPerilIndex).Receipt.ReceiptItem IsNot Nothing Then
                        For Each oReceiptItem As NexusProvider.BaseClaimRecoveryReceiptType In oClaimOpen.ClaimPeril(iPerilIndex).Receipt.ReceiptItem
                            amount += oReceiptItem.ThisReceiptNetAmount
                            tax += oReceiptItem.ThisReceiptTaxAmount
                        Next
                    End If
                    'if tax is grater than 0
                    If tax > 0 Then

                        Dim oClaimReceipt As NexusProvider.ClaimReceipt = Nothing
                        Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                        'Clear the Tax if exist
                        If oClaimOpen.ClaimPeril(iPerilIndex).Receipt.TaxItem IsNot Nothing _
                        AndAlso oClaimOpen.ClaimPeril(iPerilIndex).Receipt.TaxItem.Count > 0 Then
                            oClaimOpen.ClaimPeril(iPerilIndex).Receipt.TaxItem.Clear()
                        End If

                        Try
                            'Sam call to retreive the tax details
                            oWebservice.GetClaimReceiptTaxes(oClaimOpen.ClaimPeril(iPerilIndex).Receipt)
                        Finally
                            oWebservice = Nothing
                        End Try
                        'populating the grid based on results returned from sam
                        gvTaxesonThisReceipt.Visible = True
                        gvTaxesonThisReceipt.DataSource = oClaimOpen.ClaimPeril(iPerilIndex).Receipt.TaxItem
                        gvTaxesonThisReceipt.DataBind()
                    Else
                        gvTaxesonThisReceipt.Visible = False
                    End If

                    If amount = 0 Then
                        'need to show the static value in decimal
                        txtGrossPayment.Text = "0.00"
                        txtTotalTax.Text = "0.00"
                        txtTotalWHTax.Text = "0.00"
                        txtNetPayment.Text = "0.00"
                        gvSalvageDetails.Columns(7).Visible = False
                    Else
                        'if amount is not 0
                        pnlThisPaymentTab.Visible = True
                        txtGrossPayment.Text = Math.Round((amount + tax), 2)
                        txtTotalTax.Text = Math.Round(tax, 2)
                        If tax > 0 Then
                            txtTotalWHTax.Text = Math.Round((amount + tax), 2)
                        Else
                            txtTotalWHTax.Text = "0.00"
                        End If
                        txtNetPayment.Text = Math.Round((CDbl(txtGrossPayment.Text.Trim) - tax), 2)
                    End If

                    If oClaimOpen.Client IsNot Nothing Then
                        txtPayeeName.Text = oClaimOpen.Client.ShortName

                        Address.Address1 = oClaimOpen.Client.Address.Address1
                        Address.Address2 = oClaimOpen.Client.Address.Address2
                        Address.Address3 = oClaimOpen.Client.Address.Address3
                        Address.Address4 = oClaimOpen.Client.Address.Address4
                        Address.CountryCode = oClaimOpen.Client.Address.CountryCode
                        Address.Postcode = oClaimOpen.Client.Address.PostCode
                    End If

                    'Set The label for Receipt
                    lblThisPayment.Text = GetLocalResourceObject("ltThisReceipt")
                    ThisPaymentSummary.Text = GetLocalResourceObject("ThisReceiptSummary")
                    ltGrossPayment.Text = GetLocalResourceObject("ltGrossReceipt")
                    ltNetPayment.Text = GetLocalResourceObject("ltNetReceipt")
                    lblTaxesOnThisPayment.Text = GetLocalResourceObject("ltTaxesOnThisReceipt")
                    lblPaymentDetails.Text = GetLocalResourceObject("ltReceiptDetails")
                    liChequeDate.Visible = False
                    liThisReference.Visible = False
                    divAddress.Visible = False

                End If
            Else
                With oClaimOpen.ClaimPeril(iPerilIndex)

                    Dim amount As Decimal = 0.0
                    Dim tax As Decimal = 0.0

                    If .ClaimReserve IsNot Nothing Then
                        'Calculation of the amount and tax
                        For Each oPaymentItem As NexusProvider.ClaimPerilReservePaymentType In .ClaimReserve
                            amount += oPaymentItem.ThisPaymentINCLTax
                            tax += oPaymentItem.ThisPaymentTax
                        Next
                        'if tax is grater than 0
                        If tax > 0 Then
                            Dim oClaimPayment As NexusProvider.ClaimPayment = Nothing
                            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                            Try
                                'Sam call to retreive the tax details
                                oClaimPayment = oWebservice.GetClaimPaymentTaxes(oClaimOpen.ClaimPeril(iPerilIndex).Payment)
                            Finally
                                oWebservice = Nothing
                            End Try
                            'populating the grid based on results returned from sam
                            gvTaxesonThisPayment.Visible = True
                            gvTaxesonThisPayment.DataSource = oClaimPayment.ClaimPaymentTaxItems
                            gvTaxesonThisPayment.DataBind()
                        Else
                            gvTaxesonThisPayment.Visible = False
                        End If
                        If amount = 0 Then
                            'need to show the static value in decimal
                            txtGrossPayment.Text = "0.00"
                            txtTotalTax.Text = "0.00"
                            txtTotalWHTax.Text = "0.00"
                            txtNetPayment.Text = "0.00"
                            gvPaymentDetails.Columns(10).Visible = False
                        Else
                            'if amount is not 0

                            pnlThisPaymentTab.Visible = True
                            txtGrossPayment.Text = amount
                            txtTotalTax.Text = tax
                            If tax > 0 Then
                                txtTotalWHTax.Text = amount
                            Else
                                txtTotalWHTax.Text = "0.00"
                            End If
                            txtNetPayment.Text = CDbl(txtGrossPayment.Text.Trim) - tax
                        End If

                        If oClaimOpen.Client IsNot Nothing Then
                            txtPayeeName.Text = oClaimOpen.Client.ShortName

                            Address.Address1 = oClaimOpen.Client.Address.Address1
                            Address.Address2 = oClaimOpen.Client.Address.Address2
                            Address.Address3 = oClaimOpen.Client.Address.Address3
                            Address.Address4 = oClaimOpen.Client.Address.Address4
                            Address.CountryCode = oClaimOpen.Client.Address.CountryCode
                            Address.Postcode = oClaimOpen.Client.Address.PostCode
                        End If
                    End If
                End With
            End If
            'Make read only these fields
            txtGrossPayment.Attributes.Add("readonly", "readonly")
            txtTotalTax.Attributes.Add("readonly", "readonly")
            txtTotalWHTax.Attributes.Add("readonly", "readonly")
            txtNetPayment.Attributes.Add("readonly", "readonly")

        End Sub

#End Region

        Protected Sub gvSalvageDetails_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSalvageDetails.Load
            If gvSalvageDetails.PageCount = 1 Then
                gvSalvageDetails.AllowPaging = False
            End If
        End Sub
        ''' <summary>
        ''' Salvage grid page index change
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvSalvageDetails_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSalvageDetails.PageIndexChanging
            Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
            Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            gvSalvageDetails.PageIndex = e.NewPageIndex
            PopulateSalvageClaim(iPeril, oClaimOpen)
        End Sub
        ''' <summary>
        ''' Salvage grid Row command
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvSalvageDetails_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSalvageDetails.RowCommand
            If e.CommandName = "Lock" Then
                Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
                Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                ViewState("IsLockPayment") = 1
                If Session(CNMode) = Mode.SalvageClaim Then
                    oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(e.CommandArgument).IsLocked = True
                ElseIf Session(CNMode) = Mode.TPRecovery Then
                    oClaimOpen.ClaimPeril(iPeril).TPRecovery(e.CommandArgument).IsLocked = True
                End If
                PopulateSalvageClaim(iPeril, oClaimOpen)
            End If
        End Sub
        ''' <summary>
        ''' Salvage grid row data bound
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvSalvageDetails_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSalvageDetails.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                If HttpContext.Current.Session.IsCookieless Then
                    CType(e.Row.FindControl("hypEditPayment"), LinkButton).OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/PaymentDetails.aspx?PostbackTo=" & updThisPayment.ClientID.ToString & "&modal=true&KeepThis=true&PaymentIndex=" & e.Row.DataItemIndex & "&TB_iframe=true&height=500&width=700' , null);return false;"
                Else
                    CType(e.Row.FindControl("hypEditPayment"), LinkButton).OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "/Modal/PaymentDetails.aspx?PostbackTo=" & updThisPayment.ClientID.ToString & "&modal=true&KeepThis=true&PaymentIndex=" & e.Row.DataItemIndex & "&TB_iframe=true&height=500&width=700' , null);return false;"
                End If
            End If
        End Sub
        ''' <summary>
        ''' Validate the payment/Receipt
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="args"></param>
        ''' <remarks></remarks>
        Protected Sub IsPaymentReceived_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles IsPaymentReceived.ServerValidate
            'Valid Party Key Validation
            Select Case rblPayee.SelectedValue
                Case "1", "4"
                    If String.IsNullOrEmpty(txtParty.Text) = True Then
                        IsPaymentReceived.ErrorMessage = GetLocalResourceObject("Err_VldPartyKey")
                        args.IsValid = False
                        Exit Sub
                    End If
            End Select

            Dim oClaimOpen As NexusProvider.ClaimOpen = Session.Item(CNClaim)
            Dim oClaimReserve As NexusProvider.ClaimPerilReservePaymentTypeCollection = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(Session(CNClaimPerilIndex)).ClaimReserve
            Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
            If Session(CNMode) = Mode.PayClaim Then
                'For Claim Payments
                Dim dAmount As Decimal = 0.0
                If oClaimReserve IsNot Nothing Then
                    For Each oPaymentItem As NexusProvider.ClaimPerilReservePaymentType In oClaimReserve
                        dAmount += oPaymentItem.CostToClaim
                    Next
                End If
                If dAmount = 0 Then
                    args.IsValid = False
                    'Check link ClaimReserve
                    If oClaimReserve IsNot Nothing Then
                        If oClaimReserve.Count = 0 Then
                            'No Reserve error message display
                            IsPaymentReceived.ErrorMessage = GetLocalResourceObject("lbl_ClaimReserveNothing_Error")
                        Else
                            'No amount error message display
                            IsPaymentReceived.ErrorMessage = GetLocalResourceObject("lbl_PaymentReceived_Error")
                        End If
                    End If
                Else
                    'Check Media Type Field Mandatory on Claim Payment       
                    If HidMediaTypeFieldMandatory.Value = "1" And GISLookup_MediaType.Value = "" Then
                        IsPaymentReceived.ErrorMessage = GetLocalResourceObject("lbl_MediatypeError")
                        args.IsValid = False
                    End If
                End If
            ElseIf Session(CNMode) = Mode.SalvageClaim Then
                'For Claim Salvage Receipt
                Dim dAmount As Decimal
                For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).SalvageRecovery.Count - 1
                    dAmount = dAmount + oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iCount).ThisReceiptINCLTax
                Next
                If dAmount <= 0 Then
                    args.IsValid = False
                End If
            ElseIf Session(CNMode) = Mode.TPRecovery Then
                'For Claim TPRecovery Receipt
                Dim dAmount As Decimal
                For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).TPRecovery.Count - 1
                    dAmount = dAmount + oClaimOpen.ClaimPeril(iPeril).TPRecovery(iCount).ThisReceiptINCLTax
                Next
                If dAmount <= 0 Then
                    args.IsValid = False
                End If
            End If
        End Sub
        ''' <summary>
        ''' Check MediaType Field Mandatory option after postback has been triggered by the modal dialog "PaymentDetails"
        ''' </summary>
        ''' <remarks></remarks>
        Sub CheckMediaTypeFieldMandatory()
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
            Dim oMediaTypeFieldMandatory As String
            oMediaTypeFieldMandatory = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.MediaTypeMandatory, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
            HidMediaTypeFieldMandatory.Value = oMediaTypeFieldMandatory
        End Sub
        Sub UpdatePaymentData()
            'Save the data back to the session object
            Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
            Dim oClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            Dim oModeClaim As Mode = CType(Session.Item(CNMode), Mode)
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
            'Flag to check which peril has been updated it need to be updated in DB
            oClaim.ClaimPeril(iPeril).PerilEdited = True
            Session.Item(CNClaim) = oClaim
            'if peril screen is not configured then need to validate the reserve with coreperilscreen code
            Dim oPayment As NexusProvider.ClaimPayment = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(Session(CNClaimPerilIndex)).Payment
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oClaimResponse As NexusProvider.ClaimResponse = Nothing
            Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
            Dim sBranchCode As String = oQuote.BranchCode
            oPayment.ClaimKey = oClaim.ClaimKey
            'arch issue 268
            'oClaimResponse = oWebservice.UpdateClaimReservesOrPayments(Nothing, oPayment, Session.Item(CNClaimTimeStamp), 3, sBranchCode)
            oClaimResponse = UpdateClaimReservesOrPaymentsCall(Nothing, oPayment, Session.Item(CNClaimTimeStamp), 3, sBranchCode)
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
            Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
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
            Session(CNDataSet) = sXmlDataSet
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
        Sub GetLatestDetails(Optional ByVal iClaimKey As Integer = 0)
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oClaimVersions As NexusProvider.VersionsCollections
            Dim iHighest As Integer = 0
            Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing
            Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
            Dim oOpenClaim As New NexusProvider.ClaimOpen
            Dim oClaimRisk As NexusProvider.ClaimRisk = Nothing

            If iClaimKey > 0 Then
                'arch issue 268
                oClaimDetails = GetClaimDetailsCall(iClaimKey)
            Else
                oClaimVersions = oWebservice.GetVersionsForClaim(Session(CNClaimNumber))

                If oClaimVersions IsNot Nothing Then
                    'Find Highest Version
                    For iCount As Integer = 0 To oClaimVersions.Count - 1
                        If oClaimVersions(iCount).Version > iHighest Then
                            iHighest = oClaimVersions(iCount).Version
                        End If
                    Next
                End If
                For iCount As Integer = 0 To oClaimVersions.Count - 1
                    If oClaimVersions(iCount).Version = iHighest Then
                        'arch issue 268
                        oClaimDetails = GetClaimDetailsCall(oClaimVersions(iCount).ClaimKey)
                        Exit For
                    End If
                Next
            End If

            'Updating claim session variables
            With oClaimDetails
                oOpenClaim.CatastropheCode = .CatastropheCode
                oOpenClaim.BaseClaimKey = .BaseClaimKey
                oOpenClaim.Claim = .Claim
                oOpenClaim.ClaimCoInsurer = .ClaimCoInsurer
                oOpenClaim.ClaimDescription = .ClaimDescription
                oOpenClaim.ClaimHandlerDescription = .ClaimHandlerDescription
                oOpenClaim.ClaimKey = .ClaimKey
                oOpenClaim.ClaimNumber = .ClaimNumber
                oOpenClaim.ClaimPeril = .ClaimPeril
                oOpenClaim.ClaimStatus = .ClaimStatus
                oOpenClaim.ClaimStatusDate = .ClaimStatusDate
                oOpenClaim.ClaimStatusID = .ClaimStatusID
                oOpenClaim.ClaimVersion = .ClaimVersion
                oOpenClaim.ClaimVersionDescription = .ClaimVersionDescription
                oOpenClaim.ClientClaimNumber = .ClientClaimNumber
                oOpenClaim.ClientEmail = .ClientEmail
                oOpenClaim.ClientFaxNo = .ClientFaxNo
                oOpenClaim.ClientMobileNo = .ClientMobileNo
                oOpenClaim.ClientName = .ClientName
                oOpenClaim.ClientShortName = oClaimVersions(0).ClientShortName 'IIf(.ClientShortName <> String.Empty, .ClientShortName, Trim(lblClientCode.Text))
                oOpenClaim.ClientTelNo = .ClientTelNo
                oOpenClaim.ClientTelNoOff = .ClientTelNoOff
                oOpenClaim.CloseClaimOnZeroReserveRecoveryBalance = .CloseClaimOnZeroReserveRecoveryBalance
                oOpenClaim.Comments = .Comments
                oOpenClaim.Contact = .Contact
                oOpenClaim.CurrencyISOCode = .CurrencyCode
                oOpenClaim.Description = .Description
                oOpenClaim.ExternalHandler = .ExternalHandler
                oOpenClaim.HandlerCode = .HandlerCode
                oOpenClaim.IgnoreClaimMaintain = .IgnoreClaimMaintain
                oOpenClaim.InfoOnly = .InfoOnly
                oOpenClaim.InsuranceFileKey = .InsuranceFileKey
                oOpenClaim.InsuranceRef = .InsuranceRef
                oOpenClaim.InsurerClaimNumber = .InsurerClaimNumber
                oOpenClaim.IsAllowedClosedClaims = .IsAllowedClosedClaims
                oOpenClaim.IsDeleted = .IsDeleted
                oOpenClaim.LastModifiedDate = .LastModifiedDate
                oOpenClaim.LikelyClaim = .LikelyClaim
                oOpenClaim.Location = .Location
                oOpenClaim.LossDate = .LossDate
                oOpenClaim.LossDateFrom = .LossDateFrom
                oOpenClaim.LossFromDate = .LossToDate
                oOpenClaim.LossToDate = .LossToDate
                oOpenClaim.LossToDateSpecified = .LossToDateSpecified
                oOpenClaim.Payments = .Payments
                oOpenClaim.PolicyNumber = .PolicyNumber
                oOpenClaim.PolicyType = .PolicyType
                oOpenClaim.PrimaryCause = .PrimaryCause
                oOpenClaim.PrimaryCauseCode = .PrimaryCauseCode
                oOpenClaim.PrimaryCauseDescription = .PrimaryCauseDescription
                oOpenClaim.ProductDescription = .ProductDescription
                oOpenClaim.ProgressStatusCode = .ProgressStatusCode
                oOpenClaim.ProgressStatusDescription = .ProgressStatusDescription
                oOpenClaim.ReportedDate = .ReportedDate
                oOpenClaim.Reserve = .Reserve
                oOpenClaim.RiskKey = .RiskKey
                oOpenClaim.RiskType = CType(Session(CNClaimQuote), NexusProvider.Quote).Risks.FindItemByRiskKey(.RiskKey).RiskTypeCode
                oOpenClaim.RiskTypeDescription = CType(Session(CNClaimQuote), NexusProvider.Quote).Risks.FindItemByRiskKey(.RiskKey).Description
                oOpenClaim.SecondaryCause = .SecondaryCause
                oOpenClaim.SecondaryCauseCode = .SecondaryCauseCode
                oOpenClaim.SecondaryCauseDescription = .SecondaryCauseDescription
                oOpenClaim.TotalCurrentShareValue = .TotalCurrentShareValue
                oOpenClaim.TotalShare = .TotalShare
                oOpenClaim.Town = .Town
                oOpenClaim.TownCode = .TownCode
                oOpenClaim.UnderwritingYearCode = .UnderwritingYearCode
                oOpenClaim.UserDefFldACode = .UserDefFldACode
                oOpenClaim.UserDefFldBCode = .UserDefFldBCode
                oOpenClaim.UserDefFldCCode = .UserDefFldCCode
                oOpenClaim.UserDefFldDCode = .UserDefFldECode
                oOpenClaim.UserDefFldECode = .UserDefFldECode
                'Added for Insurer
                oOpenClaim.Insurer = .Insurer
                Session.Item(CNClaimTimeStamp) = .TimeStamp
                oOpenClaim.CurrencyISOCode = .CurrencyCode
                Session.Item(CNCurrenyCode) = Trim(.CurrencyCode) 'Changed
                oOpenClaim.Client = .Client
                'Session(CNInsurer_Header) = .ClientName
                Session(CNClaimNumber) = .ClaimNumber
                Session(CNStatus) = .ClaimStatus
            End With
            Session(CNClaim) = oOpenClaim
        End Sub

        ''' <summary>
        ''' Shows the Instalments panel and hides the This Receipt content.
        ''' Duplicate plan check (T6) and full UI population (T3) will be implemented in subsequent tasks.
        ''' </summary>
        Protected Sub btnInstalments_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInstalments.Click
            pnlThisPaymentTab.Visible = False
            pnlInstalments.Visible = True
        End Sub

        Protected Sub btn_Back_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Back.Click
            Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
            If Session(CNMode) = Mode.PayClaim Then
                CType(Session.Item(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iPeril).Payment = Nothing
            ElseIf Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                CType(Session.Item(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iPeril).Receipt.ReceiptItem = Nothing
            End If
            GetLatestDetails()
            Response.Redirect("~/claims/Perils.aspx")
        End Sub

        Protected Sub gvTaxesonThisPayment_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvTaxesonThisPayment.Load
            If gvTaxesonThisPayment.PageCount = 1 Then
                gvTaxesonThisPayment.AllowPaging = False
            End If
        End Sub

        Sub PopulateReserveItem()
            Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
            Dim oClaimOpen As NexusProvider.ClaimOpen = Nothing
            Dim oClaimPayment As New NexusProvider.ClaimPayment
            'Retreiving the claim quote information from session
            Dim oQuote As NexusProvider.Quote = CType(Session(CNClaimQuote), NexusProvider.Quote)
            'Retreiving the claim  information from session
            oClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)

            For Each oCPeril As NexusProvider.PerilSummary In oClaimOpen.ClaimPeril
                For Each oReserveItem As NexusProvider.Reserve In oCPeril.Reserve
                    Dim oClaimPaymentItem As New NexusProvider.ClaimPaymentItemType
                    Dim oClaimReserve As New NexusProvider.ClaimPerilReservePaymentType
                    oClaimPayment.BaseReserveKey = oReserveItem.BaseReserveKey
                    oClaimPaymentItem.BaseReserveKey = oReserveItem.BaseReserveKey

                    With oClaimReserve
                        .TypeCode = oReserveItem.TypeCode
                        .BaseReserveKey = oReserveItem.BaseReserveKey
                        '.PaidToDate = oReserveItem.PaidAmount

                        'Total Tax Paid and Amount Paid
                        If oClaimOpen.ClaimPeril(iPeril).ClaimPayment IsNot Nothing Then
                            Dim dPaymentBaseReserveKey As Integer = 0
                            For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).ClaimPayment.Count - 1
                                If oClaimOpen.ClaimPeril(iPeril).ClaimPayment(iCount).PaymentItems IsNot Nothing _
                                AndAlso oClaimOpen.ClaimPeril(iPeril).ClaimPayment(iCount).PaymentItems.Count > 0 Then
                                    dPaymentBaseReserveKey = oClaimOpen.ClaimPeril(iPeril).ClaimPayment(iCount).PaymentItems(0).BaseReserveKey
                                End If
                                If dPaymentBaseReserveKey = oReserveItem.BaseReserveKey Then
                                    .PaidToDateTax += oClaimOpen.ClaimPeril(iPeril).ClaimPayment(iCount).TaxAmount
                                    .PaidToDate += oClaimOpen.ClaimPeril(iPeril).ClaimPayment(iCount).LossAmount
                                End If
                            Next
                        End If

                        .TotalReserve = oReserveItem.InitialReserve + oReserveItem.RevisedReserve
                        .IsExcess = oReserveItem.IsExcess
                        .IsIndemnity = oReserveItem.IsIndemnity
                        .IsExpense = oReserveItem.IsExpense

                        'Calculation of Current Reserve and other values
                        If oReserveItem.IsExcess Then
                            .CurrentReserve = (oReserveItem.InitialReserve + oReserveItem.RevisedReserve) + (-.PaidToDate) 'oReserveItem.PaidAmount)
                        Else
                            .CurrentReserve = (oReserveItem.InitialReserve + oReserveItem.RevisedReserve) - .PaidToDate
                        End If
                        If .OldReserve <= 0.0 Then
                            .OldReserve = .CurrentReserve
                        End If

                        If Session.Item(CNReserveDescriptions) IsNot Nothing Then
                            Dim oReserveDescriptions As Hashtable = Session.Item(CNReserveDescriptions)
                            Dim HData As DictionaryEntry
                            For Each HData In oReserveDescriptions
                                If HData.Key.ToString.Trim.ToUpper = oReserveItem.TypeCode.Trim.ToUpper Then
                                    .Description = HData.Value
                                End If
                            Next
                        End If
                    End With
                    'All claim reserve will be added
                    If oCPeril.ClaimReserve IsNot Nothing Then
                        oCPeril.ClaimReserve.Add(oClaimReserve)
                    End If
                    'Only selected peril will have payment item
                    If oClaimOpen.ClaimPeril(iPeril).ClaimPerilKey = oCPeril.ClaimPerilKey Then
                        oClaimPayment.ClaimPaymentItem.Add(oClaimPaymentItem)
                    End If
                Next
            Next

            'Updating values into oClaimPayment object
            oClaimPayment.LossCurrencyCode = oQuote.CurrencyCode
            oClaimPayment.RiskType = oQuote.Risks(0).Description
            oClaimPayment.BaseClaimKey = oClaimOpen.BaseClaimKey
            oClaimPayment.BaseClaimPerilKey = oClaimOpen.ClaimPeril(iPeril).BaseClaimPerilKey
            oClaimPayment.ClientKey = oClaimOpen.Client.PartyKey
            oClaimOpen.ClaimPeril(iPeril).Payment = oClaimPayment
            Session(CNClaim) = oClaimOpen
        End Sub

        Protected Sub chkExGratia_CheckedChanged(sender As Object, e As EventArgs) Handles chkExGratia.CheckedChanged
            Dim oExGratiaOptionSettings As NexusProvider.OptionTypeSetting
            Dim oAccountSearchCriteria As New NexusProvider.AccountSearchCriteria
            Dim oAccountSearchResultCollection As NexusProvider.AccountSearchResultCollection
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

            oExGratiaOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5114)

            If chkExGratia.Checked Then
                oExGratiaOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5114)

                If (oExGratiaOptionSettings IsNot Nothing AndAlso String.IsNullOrEmpty(oExGratiaOptionSettings.OptionValue) = False) Then
                    oAccountSearchCriteria.ShortCode = oExGratiaOptionSettings.OptionValue
                    oAccountSearchResultCollection = oWebService.FindAccounts(oAccountSearchCriteria)
                    If oAccountSearchResultCollection IsNot Nothing AndAlso oAccountSearchResultCollection.Count > 0 Then
                    Else
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid.ToString, "alert('Ex-gratia Expense Account does not exist. Please contact your System Administrator.');", True)
                        chkExGratia.Checked = False
                        Return
                    End If
                End If
            End If
        End Sub
    End Class
End Namespace
