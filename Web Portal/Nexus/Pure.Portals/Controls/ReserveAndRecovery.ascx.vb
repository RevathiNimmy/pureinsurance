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
Partial Class Controls_ReserveAndRecovery
    Inherits System.Web.UI.UserControl
    Dim m_sIsReservesReadOnly As String

    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        If Not IsPostBack Then
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oIsNegativeReserve As String
            oIsNegativeReserve = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance,
                                                                       NexusProvider.ProductRiskOptions.AllowNegativeReserve,
                                                                       NexusProvider.RiskTypeOptions.None, Session(CNProductCode), Nothing)
            ViewState.Add("AllowNegativeReserve", oIsNegativeReserve)

            Dim claimsReserveForGross As NexusProvider.OptionTypeSetting
            Dim taxGroupIDForClaimsReserve As NexusProvider.OptionTypeSetting
            Dim taxVatPercForClaimsReserve As Decimal = 0

            claimsReserveForGross = oWebservice.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ClaimsReserveForGross)
            hdnClaimsReserveForGross.Value = claimsReserveForGross.OptionValue

            If claimsReserveForGross.OptionValue = "1" Then
                taxGroupIDForClaimsReserve = oWebservice.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.TaxGroupForClaimsReserve)
				If Convert.ToInt32(taxGroupIDForClaimsReserve.OptionValue) > 0 Then

					Dim oTaxTypesAndBands As NexusProvider.TaxTypesAndBandsCollection
					oTaxTypesAndBands = oWebservice.GetTaxTypesAndBands(CType(taxGroupIDForClaimsReserve.OptionValue, Integer))
					For CountVar = 0 To oTaxTypesAndBands.Count - 1
						taxVatPercForClaimsReserve = taxVatPercForClaimsReserve + oTaxTypesAndBands.Item(CountVar).Rate
					Next

					hdnClaimsReserveTaxPerc.Value = taxVatPercForClaimsReserve
				End If
			End If
        End If

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not IsPostBack Then
            Dim oMode As Mode = CType(Session.Item(CNMode), Mode)
            Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            m_sIsReservesReadOnly = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsReservesReadOnly, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)

            Dim iPerilIndex As Integer
            Integer.TryParse(Request.QueryString("PerilIndex"), iPerilIndex)
            'if peril index is zero means either navigated from another peril builder screen
            If iPerilIndex = 0 Then
                Integer.TryParse(Session(CNClaimPerilIndex), iPerilIndex)
            End If

            If IsNumeric(iPerilIndex) Then
                'Registration of the dynamic javascript
                Page.ClientScript.RegisterStartupScript(GetType(String), "EnableCalculation",
                "<script type=""text/javascript"" language=""javascript""> document.getElementById('" _
                & hdnCalculate.ClientID & "').value = '1';" & "</script>" & vbCr)

                If Not IsPostBack Then
                    Dim iPeril As Integer = CInt(Request.QueryString("PerilIndex"))
                    If iPeril = 0 Then
                        Integer.TryParse(Session(CNClaimPerilIndex), iPeril)
                    End If
                    'Retreiving the claim details from the session
                    Dim oClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                    If oClaim IsNot Nothing Then

                        GetReserves(oClaim.RiskKey)
                        'update the reserve's description with type code if no description specified
                        For Each oReserveItem As NexusProvider.Reserve In oClaim.ClaimPeril(iPeril).Reserve
                            If oReserveItem.Description Is Nothing Then
                                oReserveItem.Description = CType(Session(CNReserveDescriptions), Hashtable).Item(oReserveItem.TypeCode)
                            End If
                        Next
                        'code commented
                        'Add The Salvage Collection
                        'Add The TPRecovery Collection
                        Dim oClaimRiskLinkCollection As NexusProvider.ClaimRiskLinkCollection = Nothing
                        oClaimRiskLinkCollection = GetRecovery(oClaim.RiskKey)
                        If oClaimRiskLinkCollection IsNot Nothing AndAlso oClaimRiskLinkCollection(0).RecoveryItemType IsNot Nothing Then
                            For Each oRecoveryItem As NexusProvider.RecoveryType In oClaimRiskLinkCollection(0).RecoveryItemType
                                Dim bFound As Boolean = False
                                'Check whether Recovery is already added or not (may be accedently)
                                'Then remove it
                                For jReserveCount As Integer = 0 To oClaim.ClaimPeril(iPeril).Reserve.Count - 1
                                    If oClaim.ClaimPeril(iPeril).Reserve(jReserveCount).TypeCode.Trim.ToUpper _
                                    = oRecoveryItem.Code.Trim.ToUpper And oClaim.ClaimPeril(iPeril).Reserve(jReserveCount).BaseReserveKey = 0 Then
                                        oClaim.ClaimPeril(iPeril).Reserve.RemoveAt(jReserveCount)
                                        Exit For
                                    End If
                                Next
                                'Salvage Recovery is not added then add it into the reserve collection
                                If oRecoveryItem.IsSalvage = 1 AndAlso oClaim.ClaimPeril(iPeril).SalvageRecovery.Count = 0 Then
                                    'New Claim
                                    Dim oReserve As New NexusProvider.Reserve
                                    oReserve.BaseReserveKey = 0
                                    oReserve.TypeCode = oRecoveryItem.Code
                                    oReserve.Description = oRecoveryItem.Description
                                    oReserve.InitialReserve = 0
                                    oReserve.RevisedReserve = 0
                                    oReserve.IsSalvage = 1
                                    oClaim.ClaimPeril(iPeril).Reserve.Add(oReserve)
                                ElseIf oRecoveryItem.IsSalvage = 1 AndAlso oClaim.ClaimPeril(iPeril).SalvageRecovery.Count > 0 Then
                                    'Edit Claim - sum across all parties for this recovery type
                                    Dim bPresent As Boolean = False
                                    Dim dSumInitial As Decimal = 0
                                    Dim dSumRevised As Decimal = 0
                                    Dim dSumReceipted As Decimal = 0
                                    For iCount As Integer = 0 To oClaim.ClaimPeril(iPeril).SalvageRecovery.Count - 1
                                        If oClaim.ClaimPeril(iPeril).SalvageRecovery(iCount).TypeCode.Trim.ToUpper _
                                     = oRecoveryItem.Code.Trim.ToUpper Then
                                            bPresent = True
                                            dSumInitial += oClaim.ClaimPeril(iPeril).SalvageRecovery(iCount).InitialRecovery
                                            dSumRevised += oClaim.ClaimPeril(iPeril).SalvageRecovery(iCount).RevisedRecovery
                                            dSumReceipted += oClaim.ClaimPeril(iPeril).SalvageRecovery(iCount).ReceiptedAmount
                                        End If
                                    Next
                                    If bPresent Then
                                        Dim oReserve As New NexusProvider.Reserve
                                        oReserve.BaseReserveKey = 0
                                        oReserve.TypeCode = oRecoveryItem.Code
                                        oReserve.Description = oRecoveryItem.Description
                                        oReserve.IsSalvage = 1
                                        oReserve.InitialReserve = dSumInitial
                                        oReserve.RevisedReserve = dSumRevised
                                        oReserve.ReceiptedAmount = dSumReceipted
                                        oClaim.ClaimPeril(iPeril).Reserve.Add(oReserve)
                                    End If
                                    If bPresent = False Then
                                        Dim oReserve As New NexusProvider.Reserve
                                        oReserve.BaseReserveKey = 0
                                        oReserve.TypeCode = oRecoveryItem.Code
                                        oReserve.Description = oRecoveryItem.Description
                                        oReserve.InitialReserve = 0
                                        oReserve.RevisedReserve = 0
                                        oReserve.IsSalvage = 1
                                        oClaim.ClaimPeril(iPeril).Reserve.Add(oReserve)
                                    End If
                                End If
                                'TP Recovery
                                If oRecoveryItem.IsSalvage = 0 AndAlso oClaim.ClaimPeril(iPeril).TPRecovery.Count = 0 Then
                                    'New Claim
                                    Dim oReserve As New NexusProvider.Reserve
                                    oReserve.BaseReserveKey = 0
                                    oReserve.TypeCode = oRecoveryItem.Code
                                    oReserve.Description = oRecoveryItem.Description
                                    oReserve.InitialReserve = 0
                                    oReserve.RevisedReserve = 0
                                    oReserve.IsSalvage = 0
                                    oClaim.ClaimPeril(iPeril).Reserve.Add(oReserve)
                                ElseIf oRecoveryItem.IsSalvage = 0 AndAlso oClaim.ClaimPeril(iPeril).TPRecovery.Count > 0 Then
                                    'Edit Claim - sum across all parties for this recovery type
                                    Dim bPresent As Boolean = False
                                    Dim dSumInitial As Decimal = 0
                                    Dim dSumRevised As Decimal = 0
                                    Dim dSumReceipted As Decimal = 0
                                    For iCount As Integer = 0 To oClaim.ClaimPeril(iPeril).TPRecovery.Count - 1
                                        If oClaim.ClaimPeril(iPeril).TPRecovery(iCount).TypeCode.Trim.ToUpper _
                                     = oRecoveryItem.Code.Trim.ToUpper Then
                                            bPresent = True
                                            dSumInitial += oClaim.ClaimPeril(iPeril).TPRecovery(iCount).InitialRecovery
                                            dSumRevised += oClaim.ClaimPeril(iPeril).TPRecovery(iCount).RevisedRecovery
                                            dSumReceipted += oClaim.ClaimPeril(iPeril).TPRecovery(iCount).ReceiptedAmount
                                        End If
                                    Next
                                    If bPresent Then
                                        Dim oReserve As New NexusProvider.Reserve
                                        oReserve.BaseReserveKey = 0
                                        oReserve.TypeCode = oRecoveryItem.Code
                                        oReserve.Description = oRecoveryItem.Description
                                        oReserve.IsSalvage = 0
                                        oReserve.InitialReserve = dSumInitial
                                        oReserve.RevisedReserve = dSumRevised
                                        oReserve.ReceiptedAmount = dSumReceipted
                                        oClaim.ClaimPeril(iPeril).Reserve.Add(oReserve)
                                    End If
                                    If bPresent = False Then
                                        Dim oReserve As New NexusProvider.Reserve
                                        oReserve.BaseReserveKey = 0
                                        oReserve.TypeCode = oRecoveryItem.Code
                                        oReserve.Description = oRecoveryItem.Description
                                        oReserve.InitialReserve = 0
                                        oReserve.RevisedReserve = 0
                                        oReserve.IsSalvage = 0
                                        oClaim.ClaimPeril(iPeril).Reserve.Add(oReserve)
                                    End If
                                End If
                            Next
                        End If
                        'end comment
                        'Update the session
                        Session(CNClaim) = oClaim
                        'Populate the reserve grid
                        grdvReserveItem.DataSource = oClaim.ClaimPeril(iPeril).Reserve
                        grdvReserveItem.DataBind()
                        lblPageTitle.Text = lblPageTitle.Text & oClaim.ClaimPeril(iPeril).Description

                        'Find the ddlReserve Control if found then populate with reserves
                        Dim TempDropDownReserve As DropDownList = Nothing
                        Dim bFoundDropDown As Boolean = False
                        Dim oMaster As ContentPlaceHolder
                        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                        oMaster = GetMasterPlaceHolder(Me.Page, oNexusConfig.MainContainerName)
                        For Each oControl In oMaster.Controls
                            'check whether controls "SelectedClaimPerilReserves.ascx" exist on this page
                            If oControl.GetType.Name.Equals("controls_selectedclaimperilreserves_ascx") Then
                                For Each oChildCtrl In oControl.Controls
                                    Select Case oChildCtrl.GetType.Name.ToUpper
                                        Case "UPDATEPANEL"
                                            Dim oUpdPanel As UpdatePanel = CType(oChildCtrl, UpdatePanel)
                                            If oUpdPanel.HasControls Then
                                                For Each oChildControl As Control In oUpdPanel.Controls(0).Controls
                                                    Select Case oChildControl.ID
                                                        Case "ddlReserves"
                                                            TempDropDownReserve = CType(oChildControl, DropDownList)
                                                            If (Session(CNLockPaymentGrid) IsNot Nothing AndAlso Session(CNLockPaymentGrid) = True AndAlso Session(CNMode) = Mode.PayClaim) Then
                                                                TempDropDownReserve.Enabled = True
                                                            ElseIf (Session(CNMode) = Mode.PayClaim AndAlso (Session(CNLockPaymentGrid) Is Nothing Or Session(CNLockPaymentGrid) = False)) Then
                                                                TempDropDownReserve.Enabled = False
                                                            End If
                                                            bFoundDropDown = True
                                                            Exit For
                                                    End Select
                                                Next
                                            End If
                                    End Select
                                    If bFoundDropDown = False Then
                                        Select Case oChildCtrl.ID
                                            Case "ddlReserves"
                                                bFoundDropDown = True
                                                TempDropDownReserve = CType(oChildCtrl, DropDownList)
                                                If (Session(CNLockPaymentGrid) IsNot Nothing AndAlso Session(CNLockPaymentGrid) = True AndAlso Session(CNMode) = Mode.PayClaim) Then
                                                    TempDropDownReserve.Enabled = True
                                                ElseIf (Session(CNMode) = Mode.PayClaim AndAlso (Session(CNLockPaymentGrid) Is Nothing Or Session(CNLockPaymentGrid) = False)) Then
                                                    TempDropDownReserve.Enabled = False
                                                End If
                                                Exit For
                                        End Select
                                    Else
                                        Exit For
                                    End If
                                Next
                            End If
                            If bFoundDropDown Then
                                Exit For
                            End If
                        Next
                        If TempDropDownReserve IsNot Nothing Then
                            TempDropDownReserve.DataSource = oClaim.ClaimPeril(iPeril).Reserve
                            TempDropDownReserve.DataTextField = "Description"
                            TempDropDownReserve.DataValueField = "TypeCode"
                            TempDropDownReserve.DataBind()
                        End If

                        If Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.ViewClaim Or Session(CNMode) = Mode.ViewClaimPayment Or Session(CNMode) = Mode.Recommend Or Session(CNMode) = Mode.Authorise Or Session(CNMode) = Mode.DeclinePayment Then
                            grdvReserveItem.Enabled = False
                            If TempDropDownReserve IsNot Nothing Then
                                TempDropDownReserve.Enabled = False
                            End If
                        ElseIf Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Then
                            grdvReserveItem.Enabled = True
                            If TempDropDownReserve IsNot Nothing Then
                                TempDropDownReserve.Enabled = True
                            End If
                        End If
                    End If
                End If
            End If
            If Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                grdvReserveItem.Enabled = False
            ElseIf Session(CNMode) = Mode.PayClaim Then
                Dim oUserAuthority As New NexusProvider.UserAuthority
                oUserAuthority.UserCode = Session(CNLoginName)
                oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.CanUserChangeReserves
                oWebservice = New NexusProvider.ProviderManager().Provider
                oWebservice.GetUserAuthorityValue(oUserAuthority)

                If oUserAuthority.UserAuthorityValue = 1 Then
                    btnEditReserve.Visible = True
                    'For Claim Payments, show the warning message
                    btnEditReserve.OnClientClick = "return ConfirmEditReserve('" & GetLocalResourceObject("lbl_ConfirmEditReserveMsg").ToString() & "');"
                    'if payment has not been made then user can not edit the reserve
                    'eventually it will lock the payment thus restriction is placed, so payment is complusary then user
                    'can edit the reserve and lock the payment grid
                    If CheckPayment() = True Then
                        btnEditReserve.Enabled = True
                    Else
                        btnEditReserve.Enabled = False
                    End If
                Else
                    grdvReserveItem.Enabled = False
                    btnEditReserve.Visible = False
                End If
            End If
        Else
            'When Used Lock/Edit Reserve and update reserve
            If (Session(CNMode) = Mode.PayClaim AndAlso Session(CNLockPaymentGrid) IsNot Nothing AndAlso Session(CNLockPaymentGrid) = True) Then
                Dim oMode As Mode = CType(Session.Item(CNMode), Mode)
                Dim oClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                If oClaim IsNot Nothing Then
                    Dim iPerilIndex As Integer
                    If iPerilIndex = 0 Then
                        Integer.TryParse(Session(CNClaimPerilIndex), iPerilIndex)
                    End If
                    'Populate the reserve grid
                    grdvReserveItem.DataSource = oClaim.ClaimPeril(iPerilIndex).Reserve
                    grdvReserveItem.DataBind()
                End If
            End If

            'Lock/Edit Reserve and update reserve on claim mode bases
            'If Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.ViewClaim Or Session(CNMode) = Mode.ViewClaimPayment Or Session(CNMode) = Mode.Recommend Or Session(CNMode) = Mode.Authorise Or Session(CNMode) = Mode.DeclinePayment Then
            '    grdvReserveItem.Enabled = False

            'ElseIf Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Then
            '    grdvReserveItem.Enabled = True

            'End If
        End If

        If Request("__EVENTARGUMENT") = "PaymentUpdation" Then
            'if button is visible it means that option is ON and button is available
            If btnEditReserve.Visible = True Then

                'if payment has not been made then user can not edit the reserve
                'eventually it will lock the payment thus restriction is placed, so payment is complusary then user
                'can edit the reserve and lock the payment grid
                If CheckPayment() = True Then
                    btnEditReserve.Enabled = True
                Else
                    btnEditReserve.Enabled = False
                End If
            End If
        End If
    End Sub
#Region " GridView Events "

    Protected Sub grdvReserveItem_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvReserveItem.DataBound
        grdvReserveItem.Columns(10).Visible = False
        If hdnClaimsReserveForGross.Value = "1" Then
            If Session(CNMode) = Mode.EditClaim Then
                grdvReserveItem.Columns(3).Visible = True
                grdvReserveItem.Columns(4).Visible = True
            Else
                grdvReserveItem.Columns(3).Visible = False
                grdvReserveItem.Columns(4).Visible = False
            End If
            grdvReserveItem.Columns(5).Visible = True
            grdvReserveItem.Columns(6).Visible = True
            grdvReserveItem.Columns(8).Visible = True
            grdvReserveItem.Columns(9).Visible = False
        Else
            grdvReserveItem.Columns(3).Visible = False
            grdvReserveItem.Columns(4).Visible = False
            grdvReserveItem.Columns(5).Visible = False
            grdvReserveItem.Columns(6).Visible = False
            grdvReserveItem.Columns(8).Visible = False
            grdvReserveItem.Columns(9).Visible = True
        End If
    End Sub
    ''' <summary>
    ''' This event is fired on the rowdatabound of the gridview.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grdvReserveItem_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvReserveItem.RowDataBound
        Dim oMode As Mode = CType(Session.Item(CNMode), Mode)
        If e.Row.RowType = DataControlRowType.Header Then
            If hdnClaimsReserveForGross.Value = "1" AndAlso Session(CNMode) = Mode.EditClaim Then
                e.Row.Cells(1).Text = GetLocalResourceObject("lbl_grdvReserveItem_InitialReserve_headingCR").ToString().Replace("[!Currency!]", TransactionCurrency.Symbol)
                e.Row.Cells(2).Text = GetLocalResourceObject("lbl_grdvRevisedItem_CurrentReserve_headingCR").ToString().Replace("[!Currency!]", TransactionCurrency.Symbol)
                e.Row.Cells(5).Text = GetLocalResourceObject("lbl_grdvRevisedItem_GrossReserve_headingCR").ToString().Replace("[!Currency!]", TransactionCurrency.Symbol)
            End If
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then

            'Find the reserve description from session and calculation of the Current Reserve
            Dim lblDescription As Label = CType(e.Row.FindControl("lblDescription"), Label)

            If CType(e.Row.DataItem, NexusProvider.Reserve).BaseReserveKey <> 0 Then
                If lblDescription IsNot Nothing And Session.Item(CNReserveDescriptions) IsNot Nothing Then
                    lblDescription.Text = CType(Session.Item(CNReserveDescriptions), Hashtable).Item(CType(e.Row.DataItem, NexusProvider.Reserve).TypeCode)
                End If
            Else
                lblDescription.Text = CType(e.Row.DataItem, NexusProvider.Reserve).Description
            End If

            'Add js to update revised amount (clientside)
            Dim txtAmount As TextBox = CType(e.Row.FindControl("txtAmount"), TextBox)
            Dim lblCurrentReserve As Label = CType(e.Row.FindControl("lblCurrentReserve"), Label)
            Dim lblNewReserve As TextBox = CType(e.Row.FindControl("lblNewReserve"), TextBox)
            Dim lblInitialReserve As Label = CType(e.Row.FindControl("lblInitialReserve"), Label)
            Dim lblNewReserveNet As Label = CType(e.Row.FindControl("lblNewReserveNet"), Label)
            Dim lblTax As Label = CType(e.Row.FindControl("lblTax"), Label)
            Dim txtGrossReserve As TextBox = CType(e.Row.FindControl("txtGrossReserve"), TextBox)
            Dim lblTaxCurrentReserve As Label = CType(e.Row.FindControl("lblTaxCurrentReserve"), Label)
            Dim lblGrossCurrentReserve As Label = CType(e.Row.FindControl("lblGrossCurrentReserve"), Label)
            Dim HiddenCurrReserve As HiddenField = CType(e.Row.FindControl("HiddenCurrReserve"), HiddenField)
            Dim HiddenInitReserve As HiddenField = CType(e.Row.FindControl("HiddenInitReserve"), HiddenField)
            Dim HiddenRevsReserve As HiddenField = CType(e.Row.FindControl("HiddenRevsReserve"), HiddenField)

            Dim RngNewReserve As RangeValidator = CType(e.Row.FindControl("RngNewReserve"), RangeValidator)
            Dim RngtxtGrossReserve As RangeValidator = CType(e.Row.FindControl("RngtxtGrossReserve"), RangeValidator)

            ' get amount to be paid
            Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
            Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
            Dim dAmountToBePaid As Decimal
            If CType(Session.Item(CNMode), Mode) <> Mode.ViewClaim AndAlso CType(Session.Item(CNMode), Mode) <> Mode.DeclinePayment AndAlso CType(Session.Item(CNMode), Mode) <> Mode.Authorise AndAlso CType(Session.Item(CNMode), Mode) <> Mode.ViewClaimPayment AndAlso CType(Session.Item(CNMode), Mode) <> Mode.Recommend Then
                If oClaimOpen.ClaimPeril.Count > 0 AndAlso oClaimOpen.ClaimPeril(iPeril).ClaimReserve.Count > 0 Then
                    Dim currentBaseReserveKey As Integer = CType(e.Row.DataItem, NexusProvider.Reserve).BaseReserveKey
                    If currentBaseReserveKey <> 0 Then
                        For pCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).ClaimReserve.Count - 1
                            For rCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).Reserve.Count - 1
                                If oClaimOpen.ClaimPeril(iPeril).Reserve(rCount).BaseReserveKey = currentBaseReserveKey Then
                                    Decimal.TryParse(oClaimOpen.ClaimPeril(iPeril).ClaimReserve(pCount).CostToClaim, dAmountToBePaid)
                                    Exit For
                                End If
                            Next
                        Next
                    End If
                End If
            Else
                dAmountToBePaid = 0
            End If
            If txtAmount IsNot Nothing And lblCurrentReserve IsNot Nothing And lblNewReserve IsNot Nothing Then
                If Session(CNMode) = Mode.NewClaim Then
                    lblNewReserve.Attributes.Add("onkeyup", "javascript:ReviseAmount(" _
                                                                & CType(e.Row.DataItem, NexusProvider.Reserve).InitialReserve & ", " _
                                                                & CType(e.Row.DataItem, NexusProvider.Reserve).RevisedReserve & ", " _
                                                                & lblNewReserve.ClientID & ".value, " & txtAmount.ClientID & ",1)")
                    txtGrossReserve.Attributes.Add("onkeyup", "javascript:ReviseAmountForGross(" _
                                                                & CType(e.Row.DataItem, NexusProvider.Reserve).InitialReserve & ", " _
                                                                & CType(e.Row.DataItem, NexusProvider.Reserve).RevisedReserve & ", " _
                                                                & txtGrossReserve.ClientID & ".value, " & txtAmount.ClientID & ",1 , " & lblTax.ClientID & ", " & lblNewReserveNet.ClientID & ")")
                ElseIf Session(CNMode) = Mode.EditClaim Or (Session(CNMode) = Mode.PayClaim AndAlso Session(CNLockPaymentGrid) IsNot Nothing AndAlso Session(CNLockPaymentGrid) = True) Then
                    lblNewReserve.Attributes.Add("onkeyup", "javascript:ReviseAmount(" _
                                                                                       & CType(e.Row.DataItem, NexusProvider.Reserve).InitialReserve & ", " _
                                                                                       & CType(e.Row.DataItem, NexusProvider.Reserve).RevisedReserve - CType(e.Row.DataItem, NexusProvider.Reserve).PaidAmount & ", " _
                                                                                       & lblNewReserve.ClientID & ".value, " & txtAmount.ClientID & ",2)")
                    txtGrossReserve.Attributes.Add("onkeyup", "javascript:ReviseAmountForGross(" _
                                                                                       & CType(e.Row.DataItem, NexusProvider.Reserve).InitialReserve & ", " _
                                                                                       & CType(e.Row.DataItem, NexusProvider.Reserve).RevisedReserve - CType(e.Row.DataItem, NexusProvider.Reserve).PaidAmount & ", " _
                                                                                       & txtGrossReserve.ClientID & ".value, " & txtAmount.ClientID & ",2 , " & lblTax.ClientID & ", " & lblNewReserveNet.ClientID & ")")
                End If

                If Session(CNMode) = Mode.NewClaim Then
                    HiddenInitReserve.Value = CType(e.Row.DataItem, NexusProvider.Reserve).InitialReserve
                    HiddenRevsReserve.Value = CType(e.Row.DataItem, NexusProvider.Reserve).RevisedReserve
                ElseIf Session(CNMode) = Mode.EditClaim Or (Session(CNMode) = Mode.PayClaim AndAlso Session(CNLockPaymentGrid) IsNot Nothing AndAlso Session(CNLockPaymentGrid) = True) Then
                    HiddenInitReserve.Value = CType(e.Row.DataItem, NexusProvider.Reserve).InitialReserve
                    HiddenRevsReserve.Value = Convert.ToDecimal(CType(e.Row.DataItem, NexusProvider.Reserve).RevisedReserve - CType(e.Row.DataItem, NexusProvider.Reserve).PaidAmount)
                End If
                'Format change Etana Nexus 3.1
                Dim oFormatString As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).FormatStrings.FormatString("Currency").DataFormatString
                lblInitialReserve.Text = String.Format(oFormatString, CType(e.Row.DataItem, NexusProvider.Reserve).InitialReserve)
                lblTax.Text = String.Format(oFormatString, CType(e.Row.DataItem, NexusProvider.Reserve).Tax)
                lblNewReserveNet.Text = String.Format(oFormatString, (CType(e.Row.DataItem, NexusProvider.Reserve).GrossReserve - CType(e.Row.DataItem, NexusProvider.Reserve).Tax))

                'lblInitialReserve.Text = Math.Round(CType(e.Row.DataItem, NexusProvider.Reserve).InitialReserve, 2)
                'lblCurrentReserve.Text = Math.Round(CType(e.Row.DataItem, NexusProvider.Reserve).InitialReserve + IIf(Session(CNMode) = Mode.NewClaim, 0, CType(e.Row.DataItem, NexusProvider.Reserve).RevisedReserve), 2) ' Format(CType(e.Row.DataItem, NexusProvider.Reserve).InitialReserve + CType(e.Row.DataItem, NexusProvider.Reserve).RevisedReserve, "##.##") 'IIf(Session(CNMode) = Mode.NewClaim, 0, Math.Round(CType(e.Row.DataItem, NexusProvider.Reserve).RevisedReserve, 2))
                Dim sRevisedReserve As Double = IIf(Session(CNMode) = Mode.NewClaim, 0, CType(e.Row.DataItem, NexusProvider.Reserve).RevisedReserve)
                Dim sInitialReserve As Double = CType(e.Row.DataItem, NexusProvider.Reserve).InitialReserve

                If ViewState("AllowNegativeReserve") IsNot Nothing AndAlso ViewState("AllowNegativeReserve").ToString() = "0" Then
                    IsValidReserve.Display = ValidatorDisplay.Static
                    IsValidReserve.Enabled = True
                    IsValidReserve.ErrorMessage = GetLocalResourceObject("lbl_Failed_Negative_Reserve").ToString()
                    If hdnClaimsReserveForGross.Value = "1" Then
                        RngtxtGrossReserve.MinimumValue = 0
                        RngtxtGrossReserve.ErrorMessage = GetLocalResourceObject("lbl_Failed_Negative_Reserve").ToString()
                    End If
                End If

                hdnAllowNegativeReserve.Value = Convert.ToString(ViewState("AllowNegativeReserve"))
                ' get amount to be paid
                If CType(Session.Item(CNMode), Mode) <> Mode.ViewClaim Then
                    If oClaimOpen.ClaimPeril.Count > 0 AndAlso oClaimOpen.ClaimPeril(iPeril).ClaimReserve.Count > 0 Then
                        Dim currentBaseReserveKey As Integer = CType(e.Row.DataItem, NexusProvider.Reserve).BaseReserveKey
                        If currentBaseReserveKey <> 0 Then
                            For pCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).ClaimReserve.Count - 1
                                For rCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).Reserve.Count - 1
                                    If oClaimOpen.ClaimPeril(iPeril).Reserve(rCount).BaseReserveKey = currentBaseReserveKey Then
                                        Decimal.TryParse(oClaimOpen.ClaimPeril(iPeril).ClaimReserve(pCount).CostToClaim, dAmountToBePaid)
                                        Exit For
                                    End If
                                Next
                            Next
                        End If
                    End If
                    dAmountToBePaid = 0
                End If

                'Disable edit reserve for Salvage and recovery   
                If CType(e.Row.DataItem, NexusProvider.Reserve).BaseReserveKey = 0 Then
                    lblNewReserve.Enabled = False
                    txtGrossReserve.Enabled = False
                End If

                'CurrentReserve=InitialReserve+RevisedReserve-PaidAmount - amount to be paid 
                ' NewReserve = CurrentReserve - amount to be paid 
                'Format change Etana Nexus 3.1
                ' lblCurrentReserve.Text = Math.Round(CType(e.Row.DataItem, NexusProvider.Reserve).InitialReserve + IIf(Session(CNMode) = Mode.NewClaim, 0, CType(e.Row.DataItem, NexusProvider.Reserve).RevisedReserve) - CType(e.Row.DataItem, NexusProvider.Reserve).PaidAmount - dAmountToBePaid, 2)
                Dim dReceiptedAmount As Decimal = CType(e.Row.DataItem, NexusProvider.Reserve).ReceiptedAmount

                lblCurrentReserve.Text = String.Format(oFormatString, (CType(e.Row.DataItem, NexusProvider.Reserve).InitialReserve + IIf(Session(CNMode) = Mode.NewClaim, 0, CType(e.Row.DataItem, NexusProvider.Reserve).RevisedReserve) - CType(e.Row.DataItem, NexusProvider.Reserve).PaidAmount - dAmountToBePaid - dReceiptedAmount))
                lblGrossCurrentReserve.Text = String.Format(oFormatString, CType(e.Row.DataItem, NexusProvider.Reserve).RevisedGrossReserve)
                lblTaxCurrentReserve.Text = String.Format(oFormatString, CType(e.Row.DataItem, NexusProvider.Reserve).RevisedTaxReserve)

                Dim sNewReserve As Double = Math.Round(sRevisedReserve + sInitialReserve - CType(e.Row.DataItem, NexusProvider.Reserve).PaidAmount - dAmountToBePaid - dReceiptedAmount, 2)
                Dim sNewReserveGross As Double = Math.Round(CType(e.Row.DataItem, NexusProvider.Reserve).GrossReserve, 2)
                '-CType(e.Row.DataItem, NexusProvider.Reserve).PaidAmount - dAmountToBePaid - dReceiptedAmount - CType(e.Row.DataItem, NexusProvider.Reserve).PaidToDateTax, 2)

                If sNewReserve = 0 Then
                    lblNewReserve.Text = Format(0.0, "##.##") 'sNewReserve
                    txtGrossReserve.Text = String.Format(oFormatString, 0)
                Else
                    lblNewReserve.Text = String.Format(oFormatString, sNewReserve) 'sNewReserve
                    txtGrossReserve.Text = String.Format(oFormatString, sNewReserveGross)
                End If

                HiddenCurrReserve.Value = sNewReserve.ToString

                If m_sIsReservesReadOnly = "1" Then
                    lblNewReserve.ReadOnly = True
                    txtGrossReserve.ReadOnly = True
                    txtGrossReserve.Enabled = False
                End If

                ''For case when the open claim was not done with ClaimsReserveForGross 
                '' and maintain claim Is being done with the system topion selected
                If hdnClaimsReserveForGross.Value = "1" AndAlso Session(CNMode) = Mode.EditClaim AndAlso
                    Convert.ToDecimal(lblCurrentReserve.Text) > 0 AndAlso Convert.ToDecimal(lblGrossCurrentReserve.Text) = 0 AndAlso
                    Convert.ToDecimal(txtGrossReserve.Text) = 0 Then
                    lblNewReserveNet.Text = String.Format(oFormatString, lblCurrentReserve.Text)
                    txtGrossReserve.Text = String.Format(oFormatString, Convert.ToDecimal(lblCurrentReserve.Text) * (1 + hdnClaimsReserveTaxPerc.Value / 100))
                    lblTax.Text = String.Format(oFormatString, Convert.ToDecimal(txtGrossReserve.Text) - Convert.ToDecimal(lblNewReserveNet.Text))
                    txtAmount.Text = Convert.ToDecimal(lblNewReserveNet.Text) - (Convert.ToDecimal(HiddenInitReserve.Value) + Convert.ToDecimal(HiddenRevsReserve.Value))
                End If
            End If
        End If
    End Sub
    ''' <summary>
    ''' This event is fired on the onclick EditReserve button.
    ''' </summary>
    Protected Sub btnEditReserve_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditReserve.Click
        'Set CNLockPaymentGrid Session True for Payment Grid non editable
        Session(CNLockPaymentGrid) = True
        grdvReserveItem.Enabled = True
        Dim TempDataGrid As GridView
        Dim bFoundGrid As Boolean = False
        Dim bFoundDropDown As Boolean = False
        'Find the ddlReserve Control if found then populate with reserves
        Dim TempDropDownReserve As DropDownList
        Dim oMaster As ContentPlaceHolder
        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        oMaster = GetMasterPlaceHolder(Me.Page, oNexusConfig.MainContainerName)
        For Each oControl In oMaster.Controls
            'check whether controls "payclaims.ascx" exist on this page
            If oControl.GetType.Name.Equals("controls_payclaim_ascx") Then
                Dim rblPayee As RadioButtonList = CType(oControl.FindControl("rblPayee"), RadioButtonList)
                rblPayee.Enabled = False
                For Each oChildCtrl In oControl.Controls
                    Select Case oChildCtrl.GetType.Name.ToUpper
                        Case "UPDATEPANEL"
                            Dim oUpdPanel As UpdatePanel = CType(oChildCtrl, UpdatePanel)
                            If oUpdPanel.HasControls Then
                                For Each oChildControl As Control In oUpdPanel.Controls(0).Controls
                                    Select Case oChildControl.ID
                                        Case "gvPaymentDetails"
                                            TempDataGrid = CType(oChildControl, GridView)
                                            TempDataGrid.Enabled = False
                                            TempDataGrid.Columns(9).Visible = False
                                            bFoundGrid = True
                                            'Exit For
                                    End Select
                                Next
                            End If
                    End Select

                    If bFoundGrid = False Then
                        Select Case oChildCtrl.ID
                            Case "gvPaymentDetails"
                                bFoundGrid = True
                                TempDataGrid = CType(oChildCtrl, GridView)
                                TempDataGrid.Enabled = False
                                TempDataGrid.Columns(9).Visible = False
                                'Exit For
                        End Select
                    Else
                        'Exit For
                    End If
                Next
            End If
            If bFoundGrid Then
                'Exit For
            End If

            ''check whether controls "SelectedClaimPerilReserves.ascx" exist on this page
            If oControl.GetType.Name.Equals("controls_selectedclaimperilreserves_ascx") Then
                For Each oChildCtrl In oControl.Controls
                    Select Case oChildCtrl.GetType.Name.ToUpper
                        Case "UPDATEPANEL"
                            Dim oUpdPanel As UpdatePanel = CType(oChildCtrl, UpdatePanel)
                            If oUpdPanel.HasControls Then
                                For Each oChildControl As Control In oUpdPanel.Controls(0).Controls
                                    Select Case oChildControl.ID
                                        Case "ddlReserves"
                                            TempDropDownReserve = CType(oChildControl, DropDownList)
                                            TempDropDownReserve.Enabled = True
                                            bFoundDropDown = True
                                            'Exit For
                                    End Select
                                Next
                            End If
                    End Select
                    If bFoundDropDown = False Then
                        Select Case oChildCtrl.ID
                            Case "ddlReserves"
                                bFoundDropDown = True
                                TempDropDownReserve = CType(oChildCtrl, DropDownList)
                                TempDropDownReserve.Enabled = True
                                ' Exit For
                        End Select
                    Else
                        ' Exit For
                    End If
                Next
            End If
            If bFoundDropDown Then
                'Exit For
            End If
            If TempDropDownReserve IsNot Nothing Then
                TempDropDownReserve.Enabled = True
            End If
        Next
    End Sub
#End Region
    ''' <summary>
    ''' This Method check whether payment has been made or not
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function CheckPayment() As Boolean
        Dim oReturn As Boolean = False
        'Check the payment amount, whether payment is made or not
        Dim oClaimOpen As NexusProvider.ClaimOpen = Session.Item(CNClaim)
        Dim oClaimReserve As NexusProvider.ClaimPerilReservePaymentTypeCollection = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(Session(CNClaimPerilIndex)).ClaimReserve
        Dim iPeril As Integer
        Integer.TryParse(Session(CNClaimPerilIndex), iPeril)
        'For Claim Payments
        Dim dAmount As Decimal = 0.0
        If oClaimReserve IsNot Nothing Then
            For Each oPaymentItem As NexusProvider.ClaimPerilReservePaymentType In oClaimReserve
                dAmount += oPaymentItem.ThisPaymentINCLTax
            Next
        End If
        If dAmount <> 0.0 Then
            oReturn = True
        End If

        Return oReturn
    End Function

    Protected Sub txtGrossRsrv_TextChanged(sender As Object, e As EventArgs)

        Dim rowIndex As Integer = (TryCast((TryCast(sender, TextBox)).NamingContainer, GridViewRow)).RowIndex
        Dim oFormatString As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).FormatStrings.FormatString("Currency").DataFormatString
        Dim txtGrossReserve As TextBox = DirectCast(grdvReserveItem.Rows(rowIndex).FindControl("txtGrossReserve"), TextBox)
        Dim lblTax As Label = DirectCast(grdvReserveItem.Rows(rowIndex).FindControl("lblTax"), Label)
        Dim lblNewReserveNet As Label = DirectCast(grdvReserveItem.Rows(rowIndex).FindControl("lblNewReserveNet"), Label)
        Dim txtAmount As TextBox = DirectCast(grdvReserveItem.Rows(rowIndex).FindControl("txtAmount"), TextBox)
        Dim lblInitialReserve As Label = DirectCast(grdvReserveItem.Rows(rowIndex).FindControl("lblInitialReserve"), Label)
        Dim lblCurrentReserve As Label = DirectCast(grdvReserveItem.Rows(rowIndex).FindControl("lblCurrentReserve"), Label)
        Dim HiddenCurrReserve As HiddenField = CType(grdvReserveItem.Rows(rowIndex).FindControl("HiddenCurrReserve"), HiddenField)
        Dim HiddenInitReserve As HiddenField = CType(grdvReserveItem.Rows(rowIndex).FindControl("HiddenInitReserve"), HiddenField)
        Dim HiddenRevsReserve As HiddenField = CType(grdvReserveItem.Rows(rowIndex).FindControl("HiddenRevsReserve"), HiddenField)

        If String.IsNullOrEmpty(txtGrossReserve.Text.Trim()) Then
            txtGrossReserve.Text = "0.00"
        Else
            txtGrossReserve.Text = String.Format(oFormatString, Convert.ToDecimal(txtGrossReserve.Text))
        End If

        '' Tax = Gross Reserve - (Gross Reserve/(100% + VAT%))
        lblTax.Text = String.Format(oFormatString, txtGrossReserve.Text - (txtGrossReserve.Text / (1 + hdnClaimsReserveTaxPerc.Value / 100)))

        '' New Reserve = Gross Reserve � Tax
        lblNewReserveNet.Text = String.Format(oFormatString, txtGrossReserve.Text - lblTax.Text)

        If Session(CNMode) = Mode.NewClaim Then
            txtAmount.Text = Convert.ToDecimal(lblNewReserveNet.Text)
        ElseIf Session(CNMode) = Mode.EditClaim Or (Session(CNMode) = Mode.PayClaim AndAlso Session(CNLockPaymentGrid) IsNot Nothing AndAlso Session(CNLockPaymentGrid) = True) Then
            '' ctrlTxtAmount.value = (parseFloat(dAmount) - (parseFloat(dInitialReserve) + parseFloat(dRevisedReserve))).toFixed(2);
            txtAmount.Text = Convert.ToDecimal(lblNewReserveNet.Text) - (Convert.ToDecimal(HiddenInitReserve.Value) + Convert.ToDecimal(HiddenRevsReserve.Value))
        End If

    End Sub

End Class
