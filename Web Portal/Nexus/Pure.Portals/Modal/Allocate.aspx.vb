Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports System.Web.Configuration
Imports Nexus.Library
Imports Nexus.Utils
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus
    Partial Class Modal_Allocate
        Inherits CMS.Library.Frontend.clsCMSPage

        Public Const CNTransctionforUpdate As String = "TRANSACTION_FOR_UPDATE"
        Public Const CNOTempKEY As String = "TEMP_Transkey"
        Dim oTrasactionDetails As New NexusProvider.AllocationDetailsCollections
        Dim k_ACBaseOutstanding As Integer = 0
        Dim k_ACBaseAllocated As Integer = 1
        Dim k_ACTAllocRule As Integer = 2
        Dim k_ACTAllocSequence As Integer = 3
        Dim k_ACTypeCode As Integer = 4
        Dim arrAllocationArray As Object
        Dim oustandingamount As Double
        Dim traskeyCount As Integer
        Dim iAllocationAccountKey As Integer
        Dim iMainTransKey As Integer
        Dim oTempTranskey As New ArrayList
        Dim bRemove As Boolean
        Shared DiffCurrency As Boolean = False
        Dim bIsSingleCashListItemAllocation As Boolean = False

        ''' <summary>
        ''' Initializing system option.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oOptionSettings As NexusProvider.OptionTypeSetting

            'If System Option for "Single Cash List receipt/payment per allocation" is ON then we will not allow with multiple SPR/SPY
            oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.SingleCashListItemPerAllocation)

            'Save this system option in cache. So that it can be used furter
            If oOptionSettings.OptionValue.ToString() Is Nothing OrElse oOptionSettings.OptionValue.ToString() = "" Then
                bIsSingleCashListItemAllocation = 0
            Else
                bIsSingleCashListItemAllocation = oOptionSettings.OptionValue
            End If
            If bIsSingleCashListItemAllocation = True Then
                cvSingleSRPnSPY.Enabled = True
            End If
        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            If Not String.IsNullOrEmpty(Request("Accountkey")) Then
                CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
            End If
        End Sub

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'To set the Focus
            Page.SetFocus(writeOffReason)

            If Not IsPostBack Then
                'changes for cashlist allocation
                Dim oAllationDetails As NexusProvider.AllocationDetails
                Dim oAllocationDetailsCollections As NexusProvider.AllocationDetailsCollections
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oAllaction As New NexusProvider.AllocationDetailsCollections

                oAllocationDetailsCollections = New NexusProvider.AllocationDetailsCollections


                Dim oAccount As New NexusProvider.AccountDetails
                Dim k_ACBaseOutstanding As Integer = 0
                Dim k_ACBaseAllocated As Integer = 1
                Dim k_ACTAllocRule As Integer = 2
                Dim k_ACTAllocSequence As Integer = 3
                Dim k_ACTypeCode As Integer = 4
                Dim arrAllocationArray(,) As Object
                Dim oustandingamount As Double
                Dim traskeyCount As Integer

                If Request.QueryString("Mode") = "Allocation" Then
                    Session(CNTransDeatilsKeys) = Nothing
                    btnCancel.Visible = True
                ElseIf Not String.IsNullOrEmpty(Request("Accountkey")) Then
                    oAccount.AccountKey = CInt(Request("Accountkey"))
                End If

                If Session(CNTransdetailKeyfromCashList) IsNot Nothing Then
                    traskeyCount = Convert.ToInt32(DirectCast(Session(CNTransdetailKeyfromCashList), ArrayList).Count)
                    If traskeyCount > 0 Then
                        For i As Integer = 0 To traskeyCount - 1
                            oAllationDetails = New NexusProvider.AllocationDetails
                            oAllationDetails.TransdetailKey = Convert.ToInt32(DirectCast(Session(CNTransdetailKeyfromCashList), ArrayList)(i))
                            iMainTransKey = Convert.ToInt32(DirectCast(Session(CNTransdetailKeyfromCashList), ArrayList)(i))
                            oAllocationDetailsCollections.Add(oAllationDetails)
                        Next
                    End If
                End If

                If Session(CNTransDeatilsKeys) IsNot Nothing Then

                    traskeyCount = Convert.ToInt32(DirectCast(Session(CNTransDeatilsKeys), ArrayList).Count)
                    If traskeyCount > 0 Then
                        For i As Integer = 0 To traskeyCount - 1
                            oAllationDetails = New NexusProvider.AllocationDetails
                            oAllationDetails.TransdetailKey = Convert.ToInt32(DirectCast(Session(CNTransDeatilsKeys), ArrayList)(i))
                            oTempTranskey.Add(Convert.ToInt32(DirectCast(Session(CNTransDeatilsKeys), ArrayList)(i)))

                            oAllocationDetailsCollections.Add(oAllationDetails)
                        Next
                    End If
                End If

                oTrasactionDetails = oWebService.GetTransactionDetails(oAccount.AccountKey, oAllocationDetailsCollections)
                ' Calculate number of selected Cash entry (SRP/SYP) Entry and other entry
                If bIsSingleCashListItemAllocation = True Then
                    hfNumberOfSelectedCashEntry.Value = 0
                    hfNumberOfSelectedOtherEntry.Value = 0
                    For iCount As Integer = 0 To oTrasactionDetails.Count - 1
                        If Request.QueryString("Mode") = "Allocation" Then
                            If Left(oTrasactionDetails(iCount).DocRef.ToUpper, 3) = "SRP" Or Left(oTrasactionDetails(iCount).DocRef.ToUpper, 3) = "SPY" Then
                                hfNumberOfSelectedCashEntry.Value = CInt(hfNumberOfSelectedCashEntry.Value) + 1
                            Else
                                hfNumberOfSelectedOtherEntry.Value = CInt(hfNumberOfSelectedOtherEntry.Value) + 1
                            End If
                        End If
                    Next
                End If

                'If Base Currency and Transaction Currency is Different
                Dim sPrevCurrency As String = Nothing
                For iCount As Integer = 0 To oTrasactionDetails.Count - 1
                    If Request.QueryString("Mode") = "Allocation" Then
                        Dim sCurrencyCode As String = GetCurrencyForDescription(oTrasactionDetails(iCount).Currency.Trim.ToUpper, oTrasactionDetails(iCount).BranchCode)
                        Session(CNCurrency) = sCurrencyCode
                        If oTrasactionDetails(iCount).TransactionCurrencyCode.Trim.ToUpper <> sCurrencyCode.Trim.ToUpper Then
                            DiffCurrency = True
                            Exit For
                        End If
                    Else
                        If String.IsNullOrEmpty(sPrevCurrency) Then
                            sPrevCurrency = oTrasactionDetails(0).CurrencyCode
                            Session(CNCurrency) = sPrevCurrency.Trim
                        Else
                            If oTrasactionDetails(iCount).TransactionCurrencyCode.Trim.ToUpper <> sPrevCurrency.Trim.ToUpper Then
                                DiffCurrency = True
                                Exit For
                            End If
                        End If
                    End If
                Next

                ReDim Preserve arrAllocationArray(oTrasactionDetails.Count - 1, k_ACTypeCode)
                'Creatingg an array that need to be Passed for the AutoCalculateAllocation 
                For icntVarAllocationArray As Integer = 0 To oTrasactionDetails.Count - 1
                    oustandingamount = oustandingamount + oTrasactionDetails.Item(icntVarAllocationArray).OutStandingamount
                    arrAllocationArray(icntVarAllocationArray, k_ACBaseOutstanding) = oTrasactionDetails.Item(icntVarAllocationArray).OutStandingamount
                    arrAllocationArray(icntVarAllocationArray, k_ACBaseAllocated) = 0
                    arrAllocationArray(icntVarAllocationArray, k_ACTAllocRule) = ""
                    arrAllocationArray(icntVarAllocationArray, k_ACTAllocSequence) = ""
                    arrAllocationArray(icntVarAllocationArray, k_ACTypeCode) = oTrasactionDetails.Item(icntVarAllocationArray).MediaType
                    iAllocationAccountKey = oTrasactionDetails.Item(icntVarAllocationArray).AccountKey
                Next
                'txtTotals.Text = oustandingamount ' To  dispaly the OutSatnding amount
                AutoCalculateAllocation(arrAllocationArray) 'Passing the Array to For auto allocation

                For icntVarAllocationArray As Integer = 0 To oTrasactionDetails.Count - 1
                    'Passing the alloacted amount into the Collection 
                    If oTrasactionDetails(icntVarAllocationArray).OutStandingamount = Convert.ToDouble(arrAllocationArray(icntVarAllocationArray, k_ACBaseOutstanding)) Then
                        oTrasactionDetails(icntVarAllocationArray).AllocatedAmount = Convert.ToDouble(arrAllocationArray(icntVarAllocationArray, k_ACBaseAllocated))
                    End If
                Next

                gvAllocate.DataSource = oTrasactionDetails 'Binding the new Collection to the datasource
                gvAllocate.DataBind()

                CalculateTotals()

                ViewState.Add(CNTransctionforUpdate, oTrasactionDetails)

                'Checking the Writeoff limit For validations
                Dim oUserAuthority As New NexusProvider.UserAuthority
                oUserAuthority.UserCode = Session(CNLoginName)
                oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.HasWriteOffAuthority
                oWebService.GetUserAuthorityValue(oUserAuthority)
                hidden_limitwriteOffamount.Value = oUserAuthority.UserAuthorityOptionalValue2 ' keeping the write off limit in hidden field for Validations
                hidden_WriteOffUserAuthority.Value = oUserAuthority.UserAuthorityValue
                If Not IsNothing(oUserAuthority.UserAuthorityOptionalValue3) Then
                    hidden_WriteOffCurrency.Value = Convert.ToString(oUserAuthority.UserAuthorityOptionalValue3).Trim()
                Else
                    hidden_WriteOffCurrency.Value = String.Empty
                End If

                If Request.QueryString("Mode") = "Allocation" Then
                    btnAdd.Visible = True
                    If HttpContext.Current.Session.IsCookieless Then
                        btnAdd.OnClientClick = "tb_show(null , '" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/secure/SearchTransactions.aspx?AllocationAccountkey=" & iAllocationAccountKey & "&Mode=CashListAllocation&modal=true&KeepThis=true&TB_iframe=true&height=500&width=800' , null);return false;"
                    Else
                        btnAdd.OnClientClick = "tb_show(null , '../secure/SearchTransactions.aspx?AllocationAccountkey=" & iAllocationAccountKey & "&Mode=CashListAllocation&modal=true&KeepThis=true&TB_iframe=true&height=500&width=800' , null);return false;"
                    End If
                End If
            End If

            If Request("__EVENTARGUMENT") = "RefreshAllocate" Then
                UpdateTransactions()
            End If
        End Sub

        Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oUpdateAllocationCollection As New NexusProvider.AllocationDetailsCollections
            Dim dFirstCredit As Double
            Dim iMainRow As Integer
            Dim WriteoffAmount As Double
            Dim currencyDiff As Double
            Dim mainTrasDetailskey As Integer
            Dim oUpdateAllocation As New NexusProvider.AllocationDetails

            'If all condition are satisfied
            If Page.IsValid Then
                oTrasactionDetails = ViewState(CNTransctionforUpdate)

                'Make the SPY entries in negative

                For iCount As Integer = 0 To oTrasactionDetails.Count - 1
                    If oTrasactionDetails(iCount).DocRef.Contains("SPY") = True Then
                        If Session(CNTransdetailKeyfromCashList) IsNot Nothing Then
                            traskeyCount = Convert.ToInt32(DirectCast(Session(CNTransdetailKeyfromCashList), ArrayList).Count)
                            If traskeyCount > 0 Then
                                For i As Integer = 0 To traskeyCount - 1
                                    If oTrasactionDetails(iCount).TransdetailKey = Convert.ToInt32(DirectCast(Session(CNTransdetailKeyfromCashList), ArrayList)(i)) Then

                                        oTrasactionDetails(iCount).AllocatedAmount = oTrasactionDetails(iCount).OutStandingamount

                                        'To Put the allocation amount in non SPY entries
                                        For jCount As Integer = 0 To oTrasactionDetails.Count - 1
                                            Dim bFound As Boolean = False
                                            If traskeyCount > 0 Then
                                                For j As Integer = 0 To traskeyCount - 1
                                                    If oTrasactionDetails(jCount).TransdetailKey = Convert.ToInt32(DirectCast(Session(CNTransdetailKeyfromCashList), ArrayList)(j)) Then
                                                        bFound = True
                                                        Exit For
                                                    End If
                                                Next
                                            End If

                                            If bFound = False Then
                                                Dim dOutstaningAmount As Double
                                                dOutstaningAmount = oTrasactionDetails(jCount).OutStandingamount + oTrasactionDetails(jCount).AllocatedAmount
                                                If oTrasactionDetails(iCount).OutStandingamount <= dOutstaningAmount Then
                                                    oTrasactionDetails(jCount).AllocatedAmount += oTrasactionDetails(iCount).OutStandingamount
                                                End If
                                            End If
                                        Next
                                        Exit For
                                    End If
                                Next
                            End If
                        End If
                    End If
                Next

                'Finding out whether WriteOff Amount is specified or not 
                For Each row In gvAllocate.Rows
                    If row.RowType = DataControlRowType.DataRow Then
                        Dim txtWriteOff As TextBox = DirectCast(row.FindControl("txtWriteOff"), TextBox)

                        If Not String.IsNullOrEmpty(txtWriteOff.Text.Trim()) AndAlso Convert.ToDecimal(txtWriteOff.Text) <> 0 Then
                            WriteoffAmount = Convert.ToDecimal(txtWriteOff.Text)
                            mainTrasDetailskey = Convert.ToInt32(row.Cells(0).Text)
                            Exit For
                        End If

                    End If
                Next
                'If the System Option is Off or If System  Option is On and WriteOffAmount Not Specified Then
                'This logic will get Executed. if the 

                Dim oUserAuthority As New NexusProvider.UserAuthority
                oUserAuthority.UserCode = Session(CNLoginName)
                oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.HasWriteOffAuthority
                oWebService.GetUserAuthorityValue(oUserAuthority)

                If oUserAuthority.UserAuthorityValue = "0" Or (oUserAuthority.UserAuthorityValue = "1" And WriteoffAmount = 0) Then

                    ' When the Write off Option is not enabled or WriteOff Amount Not Specified than this will be executed.
                    ' Need to Find the First -Ve value(First Credit and it will be Main Transaction)
                    Dim bMatch As Boolean = False
                    For iCnt As Integer = 0 To oTrasactionDetails.Count - 1

                        dFirstCredit = oTrasactionDetails(iCnt).AllocatedAmount
                        If dFirstCredit < 0 Or oTrasactionDetails(iCnt).DocRef.Contains("CLP") Then
                            bMatch = True
                            iMainRow = iCnt

                            oUpdateAllocation.AccountKey = oTrasactionDetails(iCnt).AccountKey  'CInt(Request("Accountkey"))
                            '   oUpdateAllocation.CashListItemKey = 100 'To Do Cash list key is not Mandatory. SAM bug Raised 
                            'oUpdateAllocation.Amount = oTrasactionDetails(iCnt).AllocatedAmount
                            For Each row In gvAllocate.Rows
                                If oTrasactionDetails(iCnt).TransdetailKey = Convert.ToInt32(row.Cells(0).Text) Then
                                    oUpdateAllocation.Amount = DirectCast(row.FindControl("txtAllocated"), TextBox).Text.Trim()
                                End If
                            Next
                            oUpdateAllocation.TransdetailKey = oTrasactionDetails(iCnt).TransdetailKey

                            Exit For ' Passing main Values and exit the Loop
                        End If
                    Next

                    If bMatch = False Then
                        cstvalidAccount.Enabled = True
                        cstvalidAccount.IsValid = False
                    End If

                    If Page.IsValid Then

                        Dim oAllationDetails As NexusProvider.Allocation
                        For iTrasRow As Integer = 0 To oTrasactionDetails.Count - 1

                            'Remaining Trasaction apart From the Main Trasactions need to be passed as allocation Collection
                            If iTrasRow <> iMainRow Then
                                oAllationDetails = New NexusProvider.Allocation
                                oAllationDetails.AllocationTransdetailKey = oTrasactionDetails(iTrasRow).TransdetailKey
                                'oAllationDetails.AllocationAmount = oTrasactionDetails(iTrasRow).AllocatedAmount
                                For Each row In gvAllocate.Rows
                                    If oTrasactionDetails(iTrasRow).TransdetailKey = Convert.ToInt32(row.Cells(0).Text) Then
                                        Dim txtAllocated As TextBox = DirectCast(row.FindControl("txtAllocated"), TextBox)
                                        oAllationDetails.AllocationAmount = Convert.ToDecimal(txtAllocated.Text.ToString())
                                    End If
                                Next
                                oAllationDetails.AllocationTimeStamp = oTrasactionDetails(iTrasRow).AllocationTimeStamp
                                oUpdateAllocation.Allocation.Add(oAllationDetails)

                            End If
                        Next
                        Dim lblCurrencyDiffTotal As Label = DirectCast(gvAllocate.FooterRow.FindControl("lblCurrencyDiffTotal"), Label)
                        oUpdateAllocation.CurrencyDiff = Convert.ToDecimal(lblCurrencyDiffTotal.Text.ToString())
                        If Session(CNTransBranchCode) IsNot Nothing Then
                            oWebService.UpdateAllocation(oUpdateAllocation, Session(CNTransBranchCode).ToString())
                        Else
                            oWebService.UpdateAllocation(oUpdateAllocation)
                        End If

                    End If
                Else
                    ' System Option is On and writeOff Amount is Specified then this will get Executed.
                    If WriteoffAmount <> 0 Then

                        For iCnt As Integer = 0 To oTrasactionDetails.Count - 1
                            If mainTrasDetailskey = oTrasactionDetails(iCnt).TransdetailKey Then
                                oUpdateAllocation.AccountKey = oTrasactionDetails(iCnt).AccountKey 'CInt(Request("Accountkey"))
                                'oUpdateAllocation.CashListItemKey = 100 'TODO:'To Do Cash list key is not Mandatory. SAM bug Raised 
                                'oUpdateAllocation.Amount = oTrasactionDetails(iCnt).AllocatedAmount
                                For Each row In gvAllocate.Rows
                                    If oTrasactionDetails(iCnt).TransdetailKey = Convert.ToInt32(row.Cells(0).Text) Then
                                        oUpdateAllocation.Amount = DirectCast(row.FindControl("txtAllocated"), TextBox).Text.Trim()
                                    End If
                                Next
                                oUpdateAllocation.TransdetailKey = oTrasactionDetails(iCnt).TransdetailKey
                                oUpdateAllocation.WriteOffAmount = WriteoffAmount
                                oUpdateAllocation.WriteOffReason = writeOffReason.Value
                                Exit For ' Passing main Values and exit the Loop
                            End If
                        Next

                        Dim oAllationDetails As NexusProvider.Allocation
                        For iTrasRow As Integer = 0 To oTrasactionDetails.Count - 1
                            'Remaining Trasaction apart From the Main Trasactions nedd to be passed as allocation Collection
                            If mainTrasDetailskey <> oTrasactionDetails(iTrasRow).TransdetailKey Then
                                oAllationDetails = New NexusProvider.Allocation
                                oAllationDetails.AllocationTransdetailKey = oTrasactionDetails(iTrasRow).TransdetailKey
                                'oAllationDetails.AllocationAmount = oTrasactionDetails(iTrasRow).AllocatedAmount
                                For Each row In gvAllocate.Rows
                                    If oTrasactionDetails(iTrasRow).TransdetailKey = Convert.ToInt32(row.Cells(0).Text) Then
                                        Dim txtAllocated As TextBox = DirectCast(row.FindControl("txtAllocated"), TextBox)
                                        oAllationDetails.AllocationAmount = Convert.ToDecimal(txtAllocated.Text.ToString())
                                    End If
                                Next
                                oAllationDetails.AllocationTimeStamp = oTrasactionDetails(iTrasRow).AllocationTimeStamp
                                oUpdateAllocation.Allocation.Add(oAllationDetails)
                            End If
                        Next

                        If oUpdateAllocation.Allocation.Count = 0 Then
                            cstvalidAccount.Enabled = True
                            cstvalidAccount.IsValid = False
                        End If

                        If Page.IsValid Then
                            If Session(CNTransBranchCode) IsNot Nothing Then
                                oWebService.UpdateAllocation(oUpdateAllocation, Session(CNTransBranchCode).ToString())
                            Else
                                oWebService.UpdateAllocation(oUpdateAllocation)
                            End If
                        End If
                    End If
                End If

                If Page.IsValid Then
                    Session.Remove(CNTransDeatilsKeys)
                    'Session.Remove(CNTransdetailKeyfromCashList)
                    Session.Remove(CNTransBranchCode)
                    If Request.QueryString("Mode") = "Allocation" Then
                        'Response.Redirect("~/SelectBranch.aspx")
                        Session(CNCashListItemAllocationStatus) = "completed"
                        Dim sAgentStartPage As String = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).AgentStartPage
                        Response.Redirect(sAgentStartPage)
                    ElseIf Request.QueryString("TransMode") = "SearchTrans" Then
                        Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.RefreshAllocation();", True)
                    Else
                        'add javascript to call script in parent page which will close modal dialog
                        Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "RefreshAllocation") & ";"
                        Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
                    End If
                End If
            End If
        End Sub
        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            Response.Redirect("~/secure/payment/CashListItems.aspx?frompage=Allocate", False)
        End Sub

        Private Function AutoCalculateAllocation(ByVal AllocationArray(,) As Object) As Integer

            Dim k_ACBaseAllocated As Object = 1
            Dim k_ACBaseOutstanding As Object = 0
            Dim lRow As Integer
            Dim lRows As Integer
            Dim cCROrigTotal As Decimal
            Dim cDROrigTotal As Decimal
            Dim cCRTotal As Decimal
            Dim cDRTotal As Decimal
            Dim vArray As Object
            Dim lNewCol1 As Integer
            Dim lNewCol2 As Integer
            Dim lLastCR As Integer
            Dim lLastDR As Integer

            vArray = AllocationArray
            lNewCol1 = UBound(vArray, 2) + 1
            lNewCol2 = UBound(vArray, 2) + 2
            ReDim Preserve vArray(UBound(vArray, 1), lNewCol2)

            lRows = UBound(vArray, 1)

            'Retain the original row number
            For lRow = 0 To lRows

                vArray(lRow, lNewCol1) = lRow

                vArray(lRow, lNewCol2) = System.Math.Abs(vArray(lRow, k_ACBaseOutstanding))
            Next lRow

            'Sort into numerical order
            ShellSort2DArray(vArray, lNewCol2)

            'Calculate the total credit
            cCROrigTotal = 0
            cDROrigTotal = 0
            For lRow = 0 To lRows

                If vArray(lRow, k_ACBaseOutstanding) < 0 Then
                    'For Credits
                    cCROrigTotal = cCROrigTotal + vArray(lRow, k_ACBaseOutstanding)
                    lLastCR = lRow
                Else
                    'For Debits
                    cDROrigTotal = cDROrigTotal + vArray(lRow, k_ACBaseOutstanding)
                    lLastDR = lRow
                End If
            Next lRow


            If System.Math.Abs(cCROrigTotal) < System.Math.Abs(cDROrigTotal) Then
                ' Pass in what we've already done
                AutoCalculatePartAllocation(vArray, cCROrigTotal, cDROrigTotal)
            Else
                'Work out the lowest of the two
                If System.Math.Abs(cCROrigTotal) < System.Math.Abs(cDROrigTotal) Then
                    cCRTotal = cCROrigTotal
                    cDRTotal = cCROrigTotal * -1
                Else
                    cCRTotal = cDROrigTotal * -1
                    cDRTotal = cDROrigTotal
                End If

                'Now assign the credits and debits
                For lRow = 0 To lRows
                    'Deal with credits
                    If vArray(lRow, k_ACBaseOutstanding) < 0 Then
                        'Is entry less than the total left
                        If vArray(lRow, k_ACBaseOutstanding) >= cCRTotal Then
                            'Allocate all
                            vArray(lRow, k_ACBaseAllocated) = vArray(lRow, k_ACBaseOutstanding)
                            cCRTotal = cCRTotal - vArray(lRow, k_ACBaseAllocated)
                            'Are we on the last row
                        ElseIf lRow = lLastCR Then
                            vArray(lRow, k_ACBaseAllocated) = cCRTotal
                            cCRTotal = 0
                        ElseIf cCRTotal <> 0 Then
                            vArray(lRow, k_ACBaseAllocated) = System.Math.Round(cCRTotal / 2, 2)
                            cCRTotal = cCRTotal - vArray(lRow, k_ACBaseAllocated)
                        End If
                        'Deal with debits
                    Else
                        'Is entry less than the total left

                        If vArray(lRow, k_ACBaseOutstanding) <= cDRTotal Then
                            'Allocate all
                            vArray(lRow, k_ACBaseAllocated) = vArray(lRow, k_ACBaseOutstanding)
                            cDRTotal = cDRTotal - vArray(lRow, k_ACBaseAllocated)
                            'Are we on the last row
                        ElseIf lRow = lLastDR Then
                            vArray(lRow, k_ACBaseAllocated) = cDRTotal
                            cDRTotal = 0
                        ElseIf cDRTotal <> 0 Then
                            vArray(lRow, k_ACBaseAllocated) = System.Math.Round(cDRTotal / 2, 2)
                            cDRTotal = cDRTotal - vArray(lRow, k_ACBaseAllocated)
                        End If
                    End If
                Next lRow
            End If
            'Copy the array back
            For lRow = 0 To lRows
                AllocationArray(vArray(lRow, lNewCol1), k_ACBaseAllocated) = vArray(lRow, k_ACBaseAllocated)
            Next lRow
            Exit Function

        End Function

        Private Function AutoCalculatePartAllocation(ByRef vArray(,) As Object, ByVal cCRTotal As Decimal, ByVal cDRTotal As Decimal) As Integer

            Dim k_ACTAllocRule As Object = 2
            Dim k_ACTAllocSequence As Object = 3
            Dim k_ACBaseAllocated As Object = 1
            Dim k_ACBaseOutstanding As Object = 0

            Dim lRow As Integer
            Dim lRows As Integer
            Dim cPartDebit As Decimal ' The total of all lines to part allocate
            Dim cPartCredit As Decimal ' The credit available to part allocated lines
            Dim lPartCredit As Integer ' The current part credit sequence
            Dim lPartCreditRow As Integer
            Dim cBalance As Decimal ' The balance available to allocate

            lRows = UBound(vArray, 1)
            lPartCredit = -1

            'Sort into numerical order
            ShellSortAllocationArray(vArray)

            'Now assign the credits and debits
            For lRow = 0 To lRows
                ' Check if line is credit or debit

                If vArray(lRow, k_ACBaseOutstanding) < 0 Then
                    ' This is where our credit comes from and it'll always be sorted first
                    ' so allocate it all and build up our credit total

                    vArray(lRow, k_ACBaseAllocated) = vArray(lRow, k_ACBaseOutstanding)

                    cBalance = cBalance - vArray(lRow, k_ACBaseOutstanding)

                    ' Store credit available to part lines (in case there are no full ones)
                    cPartCredit = cBalance
                Else
                    ' Note: We always have more debits than credits
                    ' Follow allocation rules

                    If Convert.ToInt64(vArray(lRow, k_ACTAllocRule)) = 0 Then ' Tosafelong
                        'Is entry less than the balance

                        If vArray(lRow, k_ACBaseOutstanding) < cBalance Then
                            'Allocate all

                            vArray(lRow, k_ACBaseAllocated) = vArray(lRow, k_ACBaseOutstanding)

                            cBalance = cBalance - vArray(lRow, k_ACBaseAllocated)
                        Else
                            'Allocate remainder

                            vArray(lRow, k_ACBaseAllocated) = cBalance
                            cBalance = 0
                        End If

                        ' Set the amount left for part allocation to what's left
                        cPartCredit = cBalance
                    Else

                        If lPartCredit <> vArray(lRow, k_ACTAllocSequence) Then
                            ' Reset balance for new part credit

                            lPartCredit = vArray(lRow, k_ACTAllocSequence)
                            cPartDebit = 0

                            'Calculate the total debit on lines to partial allocate at this priority
                            For lPartCreditRow = 0 To lRows

                                If vArray(lPartCreditRow, k_ACBaseOutstanding) > 0 And vArray(lPartCreditRow, k_ACTAllocRule) = 1 And vArray(lPartCreditRow, k_ACTAllocSequence) = lPartCredit Then

                                    cPartDebit = cPartDebit + vArray(lPartCreditRow, k_ACBaseOutstanding)
                                End If
                            Next lPartCreditRow
                        End If

                        ' Is this the last line?
                        If lRow = lRows Then
                            'Is entry less than the balance .. it should be

                            If vArray(lRow, k_ACBaseOutstanding) < cBalance Then
                                'Allocate all

                                vArray(lRow, k_ACBaseAllocated) = vArray(lRow, k_ACBaseOutstanding)

                                cBalance = cBalance - vArray(lRow, k_ACBaseAllocated)
                            Else
                                'Allocate remainder

                                vArray(lRow, k_ACBaseAllocated) = cBalance
                                cBalance = 0
                            End If
                        Else
                            ' Calculate the partial allocation
                            If cPartDebit = 0 Then

                                vArray(lRow, k_ACBaseAllocated) = 0
                            Else
                                ' Allocate all part lines proportionally

                                vArray(lRow, k_ACBaseAllocated) = System.Math.Round((vArray(lRow, k_ACBaseOutstanding) / cPartDebit) * cPartCredit, 2)

                                cBalance = cBalance - vArray(lRow, k_ACBaseAllocated)
                            End If
                        End If
                    End If
                End If
            Next lRow

            Exit Function

        End Function

        Public Function ShellSort2DArray(ByRef r_vArray(,) As Object, ByVal v_iSortColumn As Short, Optional ByVal v_sSortDirection As String = "ASCENDING") As Integer

            Dim iNoOfColumns As Integer 'Total number of columns in the array
            Dim iNoOfRows As Integer 'Total number of rows in the array
            Dim iFirstRowNo As Integer 'Index of 1st row number
            Dim iLastRowNo As Integer 'Index of last row number
            Dim iCurrentColumn As Integer 'Holds current column currently processing
            Dim iCurrentRow As Integer 'Holds current row currently processing
            Dim iDistance As Integer 'Value used in sorting
            Dim iNextRow As Integer 'Holds next row to process
            Dim vTempStorage As Object 'Holds array element while swapping around
            Dim cDataValue1 As Decimal 'Holds value of string to compare with cDataValue2
            Dim cDataValue2 As Decimal 'Holds value of string to compare with cDataValue1

            'Find number of columns in the array
            iNoOfColumns = UBound(r_vArray, 2)

            'Save the first row number
            iFirstRowNo = LBound(r_vArray, 1)

            'Save the last row number
            iLastRowNo = UBound(r_vArray, 1)

            'Find no. of rows to traverse
            iNoOfRows = iLastRowNo - iFirstRowNo + 1
            iDistance = 1

            While (iDistance <= iNoOfRows)
                iDistance = 2 * iDistance
            End While

            iDistance = (iDistance / 2) - 1

            Do While (iDistance > 0)
                iNextRow = iFirstRowNo + iDistance

                'While there are rows to process
                Do While (iNextRow <= iLastRowNo)
                    iCurrentRow = iNextRow
                    Do
                        If iCurrentRow >= (iFirstRowNo + iDistance) Then

                            'Prepare for actual compare of data value
                            cDataValue1 = r_vArray(iCurrentRow, v_iSortColumn)

                            cDataValue2 = r_vArray(iCurrentRow - iDistance, v_iSortColumn)

                            'Ascending sort
                            If v_sSortDirection = "ASCENDING" Then

                                'Do the comparison of data values - if unsorted then swap the two rows around
                                If cDataValue1 < cDataValue2 Then

                                    For iCurrentColumn = 0 To iNoOfColumns

                                        vTempStorage = r_vArray(iCurrentRow, iCurrentColumn)

                                        r_vArray(iCurrentRow, iCurrentColumn) = r_vArray(iCurrentRow - iDistance, iCurrentColumn)

                                        r_vArray(iCurrentRow - iDistance, iCurrentColumn) = vTempStorage
                                    Next
                                    iCurrentRow = iCurrentRow - iDistance
                                Else
                                    Exit Do
                                End If
                            Else

                                'Descending sort
                                If v_sSortDirection = "DESCENDING" Then

                                    'Actual compare of data value - if unsorted then swap the two rows around
                                    If cDataValue1 >= cDataValue2 Then
                                        For iCurrentColumn = 0 To iNoOfColumns

                                            vTempStorage = r_vArray(iCurrentRow, iCurrentColumn)

                                            r_vArray(iCurrentRow, iCurrentColumn) = r_vArray(iCurrentRow - iDistance, iCurrentColumn)

                                            r_vArray(iCurrentRow - iDistance, iCurrentColumn) = vTempStorage
                                        Next
                                        iCurrentRow = iCurrentRow - iDistance
                                    Else
                                        Exit Do
                                    End If
                                End If
                            End If
                        Else
                            Exit Do
                        End If
                    Loop
                    iNextRow = iNextRow + 1
                Loop
                iDistance = (iDistance - 1) / 2
            Loop

            Exit Function

        End Function

        Public Function ShellSortAllocationArray(ByRef r_vArray(,) As Object) As Integer

            Dim k_ACBaseOutstanding As Object = 0
            Dim k_ACTAllocSequence As Object = 3
            Dim k_ACTAllocRule As Object = 2
            Dim k_ACTypeCode As Object = 4
            Dim iNoOfColumns As Integer 'Total number of columns in the array
            Dim iNoOfRows As Integer 'Total number of rows in the array
            Dim iFirstRowNo As Integer 'Index of 1st row number
            Dim iLastRowNo As Integer 'Index of last row number
            Dim iCurrentColumn As Integer 'Holds current column currently processing
            Dim iCurrentRow As Integer 'Holds current row currently processing
            Dim iDistance As Integer 'Value used in sorting
            Dim iNextRow As Integer 'Holds next row to process
            Dim vTempStorage As Object 'Holds array element while swapping around
            Dim lDataValue1 As Integer 'Holds value to compare with lDataValue2
            Dim lDataValue2 As Integer 'Holds value to compare with lDataValue1

            'Find number of columns in the array
            iNoOfColumns = UBound(r_vArray, 2)

            'Save the first row number
            iFirstRowNo = LBound(r_vArray, 1)

            'Save the last row number
            iLastRowNo = UBound(r_vArray, 1)
            'Ensure rule and sequence set appropriately
            For iCurrentRow = iFirstRowNo To iLastRowNo
                ' Unsupported or non-configured types are always treated as with premium
                Select Case r_vArray(iCurrentRow, k_ACTypeCode)
                    Case "TAX"
                        ' A configured type, just ensure we have a valid id

                        r_vArray(iCurrentRow, k_ACTAllocRule) = Convert.ToInt32(r_vArray(iCurrentRow, k_ACTAllocRule), 1)

                        ' Rule 1 is always at same sequence...

                        If r_vArray(iCurrentRow, k_ACTAllocRule) = 1 Then

                            r_vArray(iCurrentRow, k_ACTAllocSequence) = 1
                        Else

                            r_vArray(iCurrentRow, k_ACTAllocSequence) = Convert.ToInt32(r_vArray(iCurrentRow, k_ACTAllocSequence), 1)
                        End If
                    Case Else
                        r_vArray(iCurrentRow, k_ACTAllocRule) = 1

                        r_vArray(iCurrentRow, k_ACTAllocSequence) = 1
                End Select
            Next iCurrentRow

            'Find no. of rows to traverse
            iNoOfRows = iLastRowNo - iFirstRowNo + 1
            iDistance = 1

            Do While (iDistance <= iNoOfRows)
                iDistance = 2 * iDistance
            Loop

            iDistance = (iDistance / 2) - 1

            Do While (iDistance > 0)
                iNextRow = iFirstRowNo + iDistance

                'While there are rows to process
                Do While (iNextRow <= iLastRowNo)
                    iCurrentRow = iNextRow
                    Do
                        If iCurrentRow >= (iFirstRowNo + iDistance) Then
                            'Initial compare is on the credit/debit of the value, not the value

                            lDataValue1 = System.Math.Sign(r_vArray(iCurrentRow, k_ACBaseOutstanding))

                            lDataValue2 = System.Math.Sign(r_vArray(iCurrentRow - iDistance, k_ACBaseOutstanding))
                            ' If amount is same goto rule
                            If lDataValue1 = lDataValue2 Then

                                lDataValue1 = Convert.ToInt64(r_vArray(iCurrentRow, k_ACTAllocRule))

                                lDataValue2 = Convert.ToInt64(r_vArray(iCurrentRow - iDistance, k_ACTAllocRule))
                                ' If rule is same goto sequence
                                If lDataValue1 = lDataValue2 Then

                                    lDataValue1 = Convert.ToInt64(r_vArray(iCurrentRow, k_ACTAllocSequence))

                                    lDataValue2 = Convert.ToInt64(r_vArray(iCurrentRow - iDistance, k_ACTAllocSequence))
                                End If
                            End If
                            'Do the comparison of values - if unsorted then swap the two rows around
                            If lDataValue1 < lDataValue2 Then
                                For iCurrentColumn = 0 To iNoOfColumns

                                    vTempStorage = r_vArray(iCurrentRow, iCurrentColumn)

                                    r_vArray(iCurrentRow, iCurrentColumn) = r_vArray(iCurrentRow - iDistance, iCurrentColumn)

                                    r_vArray(iCurrentRow - iDistance, iCurrentColumn) = vTempStorage
                                Next
                                iCurrentRow = iCurrentRow - iDistance
                            Else
                                Exit Do
                            End If
                        Else
                            Exit Do
                        End If
                    Loop
                    iNextRow = iNextRow + 1
                Loop
                iDistance = (iDistance - 1) / 2
            Loop

            Exit Function

        End Function

        Protected Sub UpdateTransactions()
            Dim oAllationDetails As NexusProvider.AllocationDetails
            Dim oAllocationDetailsCollections As New NexusProvider.AllocationDetailsCollections
            Dim iVarTranscount As Integer
            iVarTranscount = Convert.ToInt32(DirectCast(Session(CNTransdetailKeyfromCashList), ArrayList).Count)

            If iVarTranscount > 0 Then

                For i As Integer = 0 To iVarTranscount - 1
                    oAllationDetails = New NexusProvider.AllocationDetails
                    oAllationDetails.TransdetailKey = Convert.ToInt32(DirectCast(Session(CNTransdetailKeyfromCashList), ArrayList)(i))
                    oAllocationDetailsCollections.Add(oAllationDetails)
                    iMainTransKey = Convert.ToInt32(DirectCast(Session(CNTransdetailKeyfromCashList), ArrayList)(i))
                Next

            End If

            If Session(CNTransDeatilsKeys) IsNot Nothing Then
                iVarTranscount = Convert.ToInt32(DirectCast(Session(CNTransDeatilsKeys), ArrayList).Count)
                If ViewState(CNOTempKEY) Is Nothing Then
                    If iVarTranscount > 0 Then
                        For i As Integer = 0 To iVarTranscount - 1
                            oAllationDetails = New NexusProvider.AllocationDetails
                            oAllationDetails.TransdetailKey = Convert.ToInt32(DirectCast(Session(CNTransDeatilsKeys), ArrayList)(i))
                            oTempTranskey.Add(Convert.ToInt32(DirectCast(Session(CNTransDeatilsKeys), ArrayList)(i)))

                            oAllocationDetailsCollections.Add(oAllationDetails)
                        Next
                        ViewState.Add(CNOTempKEY, oTempTranskey)

                    End If

                Else
                    If bRemove = True Then
                        oTempTranskey = ViewState(CNOTempKEY)
                        Dim iTempTrans As Integer = oTempTranskey.Count

                        If iTempTrans <> 0 Then
                            For i As Integer = 0 To iTempTrans - 1
                                oAllationDetails = New NexusProvider.AllocationDetails
                                oAllationDetails.TransdetailKey = Convert.ToInt32(DirectCast(oTempTranskey, ArrayList)(i))
                                oAllocationDetailsCollections.Add(oAllationDetails)
                            Next
                        Else

                            If hvKeyToRemove.Value <> "" AndAlso bRemove Then
                                Dim arrlistTransidForCashList As New ArrayList
                                If gvAllocate.Rows.Count > 0 Then
                                    Session(CNTransDeatilsKeys) = Nothing
                                    If gvAllocate.Rows.Count <> oAllocationDetailsCollections.Count Then
                                        oAllocationDetailsCollections = DirectCast(ViewState(CNTransctionforUpdate), NexusProvider.AllocationDetailsCollections)
                                    End If
                                    For iCount As Integer = 0 To gvAllocate.Rows.Count - 1
                                        If (oAllocationDetailsCollections(iCount).TransdetailKey = Convert.ToInt16(hvKeyToRemove.Value)) Then
                                            Session(CNTransDeatilsKeys) = oAllocationDetailsCollections(iCount).TransdetailKey
                                            arrlistTransidForCashList.Add(oAllocationDetailsCollections(iCount).TransdetailKey)
                                            oAllocationDetailsCollections.Remove(iCount)
                                            If gvAllocate.Rows.Count = 2 Then
                                                Exit For
                                            End If

                                        End If
                                    Next
                                End If
                                Session(CNTransdetailKeyfromCashList) = arrlistTransidForCashList
                            End If

                        End If

                    Else
                        iVarTranscount = Convert.ToInt32(DirectCast(Session(CNTransDeatilsKeys), ArrayList).Count)
                        oTempTranskey = ViewState(CNOTempKEY)
                        If iVarTranscount > 0 Then
                            For i As Integer = 0 To iVarTranscount - 1
                                oTempTranskey.Add(Convert.ToInt32(DirectCast(Session(CNTransDeatilsKeys), ArrayList)(i)))
                            Next
                            ViewState.Remove(CNOTempKEY)
                            ViewState.Add(CNOTempKEY, oTempTranskey)

                            Dim iTempTrans As Integer = oTempTranskey.Count

                            For i As Integer = 0 To iTempTrans - 1
                                oAllationDetails = New NexusProvider.AllocationDetails
                                oAllationDetails.TransdetailKey = Convert.ToInt32(DirectCast(oTempTranskey, ArrayList)(i))
                                oAllocationDetailsCollections.Add(oAllationDetails)
                            Next

                        End If

                    End If

                End If
            Else
                If hvKeyToRemove.Value <> "" AndAlso bRemove Then
                    Dim arrlistTransidForCashList As New ArrayList
                    If gvAllocate.Rows.Count > 0 Then
                        Session(CNTransDeatilsKeys) = Nothing
                        For iCount As Integer = 0 To gvAllocate.Rows.Count - 1
                            If (oAllocationDetailsCollections(iCount).TransdetailKey = Convert.ToInt16(hvKeyToRemove.Value)) Then
                                oAllocationDetailsCollections.Remove(iCount)
                                Session(CNTransDeatilsKeys) = oAllocationDetailsCollections(iCount).TransdetailKey
                                arrlistTransidForCashList.Add(oAllocationDetailsCollections(iCount).TransdetailKey)
                                If gvAllocate.Rows.Count = 2 Then
                                    Exit For
                                End If
                            End If
                        Next
                    End If
                    Session(CNTransdetailKeyfromCashList) = arrlistTransidForCashList
                End If

            End If


            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oAllaction As New NexusProvider.AllocationDetailsCollections
            Dim oAccount As New NexusProvider.AccountDetails
            oTrasactionDetails = oWebService.GetTransactionDetails(oAccount.AccountKey, oAllocationDetailsCollections)

            'If Base Currency and Transaction Currency is Different
            Dim sPrevCurrency As String = Nothing
            For iCount As Integer = 0 To oTrasactionDetails.Count - 1
                If String.IsNullOrEmpty(sPrevCurrency) Then
                    sPrevCurrency = oTrasactionDetails(0).Currency
                Else
                    If oTrasactionDetails(iCount).Currency.Trim.ToUpper <> sPrevCurrency.Trim.ToUpper Then
                        DiffCurrency = True
                        Exit For
                    End If
                End If
            Next
            ReDim Preserve arrAllocationArray(oTrasactionDetails.Count - 1, k_ACTypeCode)

            'Creatingg an array that need to be Passed for the AutoCalculateAllocation 
            For icntVarAllocationArray As Integer = 0 To oTrasactionDetails.Count - 1
                oustandingamount = oustandingamount + oTrasactionDetails.Item(icntVarAllocationArray).OutStandingamount
                arrAllocationArray(icntVarAllocationArray, k_ACBaseOutstanding) = oTrasactionDetails.Item(icntVarAllocationArray).OutStandingamount
                arrAllocationArray(icntVarAllocationArray, k_ACBaseAllocated) = 0
                arrAllocationArray(icntVarAllocationArray, k_ACTAllocRule) = ""
                arrAllocationArray(icntVarAllocationArray, k_ACTAllocSequence) = ""
                arrAllocationArray(icntVarAllocationArray, k_ACTypeCode) = oTrasactionDetails.Item(icntVarAllocationArray).MediaType
            Next

            'Make the Outstanding
            'txtTotals.Text = oustandingamount ' To  dispaly the OutSatnding amount
            AutoCalculateAllocation(arrAllocationArray) 'Passing the Array to For auto allocation

            For icntVarAllocationArray As Integer = 0 To oTrasactionDetails.Count - 1
                'Passing the alloacted amount into the Collection 
                If oTrasactionDetails(icntVarAllocationArray).OutStandingamount = Convert.ToDouble(arrAllocationArray(icntVarAllocationArray, k_ACBaseOutstanding)) Then
                    oTrasactionDetails(icntVarAllocationArray).AllocatedAmount = Convert.ToDouble(arrAllocationArray(icntVarAllocationArray, k_ACBaseAllocated))
                End If
            Next

            ' Calculate number of selected Cash entry (SRP/SYP) Entry and other entry
            If bIsSingleCashListItemAllocation = True Then
                hfNumberOfSelectedCashEntry.Value = 0
                hfNumberOfSelectedOtherEntry.Value = 0
                For iCount As Integer = 0 To oTrasactionDetails.Count - 1
                    If Request.QueryString("Mode") = "Allocation" Then
                        If Left(oTrasactionDetails(iCount).DocRef.ToUpper, 3) = "SRP" Or Left(oTrasactionDetails(iCount).DocRef.ToUpper, 3) = "SPY" Then
                            hfNumberOfSelectedCashEntry.Value = CInt(hfNumberOfSelectedCashEntry.Value) + 1
                        Else
                            hfNumberOfSelectedOtherEntry.Value = CInt(hfNumberOfSelectedOtherEntry.Value) + 1
                        End If
                    End If
                Next
            End If

            gvAllocate.DataSource = oTrasactionDetails 'Binding the new Collection to the datasource
            gvAllocate.DataBind()

            CalculateTotals()

            ViewState.Add(CNTransctionforUpdate, oTrasactionDetails)

            'Checking the Writeoff limit For validations
            Dim oUserAuthority As New NexusProvider.UserAuthority
            oUserAuthority.UserCode = Session(CNLoginName)
            oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.HasWriteOffAuthority
            oWebService.GetUserAuthorityValue(oUserAuthority)
            hidden_limitwriteOffamount.Value = oUserAuthority.UserAuthorityOptionalValue2 ' keeping the write off limit in hidden field for Validations
            hidden_WriteOffUserAuthority.Value = oUserAuthority.UserAuthorityValue
            If Not IsNothing(oUserAuthority.UserAuthorityOptionalValue3) Then
                hidden_WriteOffCurrency.Value = Convert.ToString(oUserAuthority.UserAuthorityOptionalValue3).Trim()
            Else
                hidden_WriteOffCurrency.Value = String.Empty
            End If
        End Sub

        Protected Sub gvAllocate_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAllocate.RowDataBound
            If e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(11).Visible = If(Request.QueryString("Mode") = "Allocation", True, False)
            End If

            If e.Row.RowType = DataControlRowType.DataRow Then
                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("TransdetailKey") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.AllocationDetails).TransdetailKey)
                Dim oItem As NexusProvider.AllocationDetails = CType(e.Row.DataItem, NexusProvider.AllocationDetails)

                e.Row.Cells(5).Text = New Money(oItem.TransactionCurrencyAmount, oItem.TransactionCurrencyCode).Formatted 'TransactionCurrencyAmount
                '  e.Row.Cells(10).Text = New Money(oItem.CurrencyDiff, oItem.CurrencyCode).Formatted 'CurrencyDiff


                Dim olinkbutton As LinkButton = e.Row.Cells(11).FindControl("lnkbtnSelect")
                Dim txtCurrencyDiff As TextBox
                Dim lblOSAmount As Label
                Dim txtAllocatedAmount As TextBox
                If Request.QueryString("Mode") = "Allocation" Then
                    'To Show extra columns
                    If DiffCurrency = True Then
                        e.Row.Cells(3).Visible = True
                        e.Row.Cells(4).Visible = True
                        e.Row.Cells(5).Visible = True
                        e.Row.Cells(10).Visible = True

                    Else
                        e.Row.Cells(3).Visible = False
                        e.Row.Cells(4).Visible = False
                        e.Row.Cells(5).Visible = False
                        e.Row.Cells(10).Visible = False
                    End If
                    e.Row.Cells(0).Visible = False
                    If iMainTransKey = CType(e.Row.DataItem, NexusProvider.AllocationDetails).TransdetailKey Then
                        olinkbutton.Visible = False
                    Else
                        olinkbutton.Text = "<i class='fa fa-trash' aria-hidden='true'></i> Remove"
                        olinkbutton.Visible = True
                    End If
                    'Cash/Cheque Payment Allocate
                    txtTotals.Text = (CDbl(txtTotals.Text) + CType(e.Row.DataItem, NexusProvider.AllocationDetails).OutStandingamount).ToString("N2")

                    Dim txtAllocated As TextBox = DirectCast(e.Row.FindControl("txtAllocated"), TextBox)
                    txtAllocated.Text = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AllocatedAmount")).ToString("N2")
                    txtBalanceAmount.Text = Convert.ToDecimal(txtBalanceAmount.Text) + Convert.ToDecimal(txtAllocated.Text.Trim())
                    If DiffCurrency = True Then

                        lblOSAmount = DirectCast(e.Row.FindControl("lblOSAmount"), Label)
                        lblOSAmount.Text = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "OutstandingAmount")).ToString("N2")
                        If Convert.ToDouble(lblOSAmount.Text) = Convert.ToDouble(txtAllocated.Text) Then
                            e.Row.Cells(10).Enabled = False
                        Else
                            e.Row.Cells(10).Enabled = True
                        End If

                    End If
                Else
                    e.Row.Cells(0).Visible = False
                    If DiffCurrency = True Then
                        e.Row.Cells(3).Visible = True
                        e.Row.Cells(4).Visible = True
                        e.Row.Cells(5).Visible = True
                        e.Row.Cells(10).Visible = True

                    Else
                        e.Row.Cells(3).Visible = False
                        e.Row.Cells(4).Visible = False
                        e.Row.Cells(5).Visible = False
                        e.Row.Cells(10).Visible = False
                    End If

                    Dim txtAllocated As TextBox = DirectCast(e.Row.FindControl("txtAllocated"), TextBox)
                    txtAllocated.Text = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AllocatedAmount")).ToString("N2")

                    If iMainTransKey = CType(e.Row.DataItem, NexusProvider.AllocationDetails).TransdetailKey Then
                        txtBalanceAmount.Text = Convert.ToDecimal(txtBalanceAmount.Text) + Convert.ToDecimal(txtAllocated.Text.Trim())
                    End If
                    If DiffCurrency = True Then

                        lblOSAmount = DirectCast(e.Row.FindControl("lblOSAmount"), Label)
                        lblOSAmount.Text = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "OutstandingAmount")).ToString("N2")
                        If Convert.ToDouble(lblOSAmount.Text) = Convert.ToDouble(txtAllocated.Text) Then
                            e.Row.Cells(10).Enabled = False
                        Else
                            e.Row.Cells(10).Enabled = True
                        End If

                    End If
                    txtTotals.Text = (CDbl(txtTotals.Text) + CType(e.Row.DataItem, NexusProvider.AllocationDetails).OutStandingamount).ToString("N2")
                End If
            ElseIf e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(0).Visible = False
                txtTotals.Text = "0.00"
                txtBalanceAmount.Text = "0.00"
                If DiffCurrency = True Then
                    e.Row.Cells(3).Visible = True
                    e.Row.Cells(4).Visible = True
                    e.Row.Cells(5).Visible = True
                    e.Row.Cells(10).Visible = True
                Else
                    e.Row.Cells(3).Visible = False
                    e.Row.Cells(4).Visible = False
                    e.Row.Cells(5).Visible = False
                    e.Row.Cells(10).Visible = False
                End If
            ElseIf e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(0).Visible = False
                If DiffCurrency = True Then
                    e.Row.Cells(3).Visible = True
                    e.Row.Cells(4).Visible = True
                    e.Row.Cells(5).Visible = True
                    e.Row.Cells(10).Visible = True
                Else
                    e.Row.Cells(3).Visible = False
                    e.Row.Cells(4).Visible = False
                    e.Row.Cells(5).Visible = False
                    e.Row.Cells(10).Visible = False
                End If
            End If
        End Sub

        Protected Sub gvAllocate_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvAllocate.RowCommand

            Dim olinkbutton As LinkButton = e.CommandSource
            Dim oGridRow As GridViewRow = olinkbutton.NamingContainer
            Dim OTemp As New ArrayList
            If olinkbutton IsNot Nothing AndAlso olinkbutton.Text = "Remove" Then
                hvKeyToRemove.Value = e.CommandArgument
            End If

            'If user is removing any cashlist from main entries, as currently multiple cashlist allcation is allowed
            If Session(CNTransdetailKeyfromCashList) IsNot Nothing Then
                Dim aTransDetailForCashList As ArrayList = DirectCast(Session(CNTransdetailKeyfromCashList), ArrayList)
                If aTransDetailForCashList.Count > 0 Then
                    For i As Integer = 0 To aTransDetailForCashList.Count - 1
                        If CInt(oGridRow.Cells(0).Text.Trim()) = Convert.ToInt32(DirectCast(Session(CNTransdetailKeyfromCashList), ArrayList)(i)) Then
                            aTransDetailForCashList.Remove(Convert.ToInt32(DirectCast(Session(CNTransdetailKeyfromCashList), ArrayList)(i)))
                            Exit For
                        End If
                    Next
                End If
                Session(CNTransdetailKeyfromCashList) = aTransDetailForCashList
            End If

            If ViewState(CNOTempKEY) IsNot Nothing Then
                oTempTranskey = ViewState(CNOTempKEY)

                OTemp = ViewState(CNOTempKEY)
                traskeyCount = oTempTranskey.Count
                For i As Integer = 0 To traskeyCount - 1
                    If CInt(oGridRow.Cells(0).Text.Trim()) = Convert.ToInt32(DirectCast(oTempTranskey, ArrayList)(i)) Then
                        OTemp.Remove(Convert.ToInt32(DirectCast(oTempTranskey, ArrayList)(i)))
                        Exit For
                    End If
                Next
                ViewState(CNOTempKEY) = OTemp
                oTempTranskey = ViewState(CNOTempKEY)
            End If

            bRemove = True
            UpdateTransactions()
            bRemove = False
        End Sub
        ''' <summary>
        ''' Set error message when more then SPR/SPY is selected.
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="args"></param>
        ''' <remarks></remarks>
        Protected Sub cvSingleSRPnSPY_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvSingleSRPnSPY.ServerValidate
            If CType(bIsSingleCashListItemAllocation, Boolean) = True Then
                If (hfNumberOfSelectedCashEntry.Value > 1 And hfNumberOfSelectedOtherEntry.Value >= 1) Then
                    args.IsValid = False
                Else
                    args.IsValid = True
                End If
            End If
        End Sub

#Region "Validations"

        Protected Sub custvldWriteoffAuthority_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles custvldWriteoffAuthority.ServerValidate
            Dim writeOffAuthority As String = String.Empty
            Dim writeOffAmount As Decimal = 0
            Dim lblWriteOffTotal As Label
            If gvAllocate.Rows.Count > 0 Then
                lblWriteOffTotal = DirectCast(gvAllocate.FooterRow.FindControl("lblWriteOffTotal"), Label)
                writeOffAmount = Convert.ToDecimal(lblWriteOffTotal.Text.Trim())
            End If
            Dim oCurrencies As Nexus.Library.Config.Currencies
            oCurrencies = CType(GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Currencies
            writeOffAuthority = hidden_WriteOffUserAuthority.Value.Trim()


            If (writeOffAuthority <> "1" AndAlso Math.Abs(writeOffAmount) > 0) Then
                args.IsValid = False
                Dim errorMsg As String = Convert.ToString(GetLocalResourceObject("lbl_WriteOff_Limit"))
                errorMsg = Replace(errorMsg, "<Difference>", writeOffAmount)
                errorMsg = Replace(errorMsg, "<Limit>", hidden_limitwriteOffamount.Value.Trim())
                errorMsg = Replace(errorMsg, "<Currency>", oCurrencies.Currency(hidden_WriteOffCurrency.Value.Trim()).Display)
                custvldWriteoffAuthority.ErrorMessage = errorMsg
            Else
                args.IsValid = True
            End If
        End Sub

        Protected Sub custvldWriteOffOnlyOne_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles custvldWriteOffOnlyOne.ServerValidate
            If Page.IsValid Then
                Dim writeOffCount As Integer = 0
                For Each row In gvAllocate.Rows
                    If row.RowType = DataControlRowType.DataRow Then
                        Dim txtWriteOff As TextBox = DirectCast(row.FindControl("txtWriteOff"), TextBox)
                        If Not String.IsNullOrEmpty(txtWriteOff.Text) AndAlso Convert.ToDecimal(txtWriteOff.Text) <> 0 Then
                            writeOffCount = writeOffCount + 1
                        End If
                    End If
                Next
                If writeOffCount > 1 Then
                    args.IsValid = False
                Else
                    args.IsValid = True
                End If
            End If
        End Sub

        Protected Sub custvldWriteoffReason_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles custvldWriteoffReason.ServerValidate
            If Page.IsValid Then
                Dim writeOffAmount As Decimal = 0
                Dim lblWriteOff As Label
                If gvAllocate.Rows.Count > 0 Then
                    lblWriteOff = DirectCast(gvAllocate.FooterRow.FindControl("lblWriteOffTotal"), Label)
                    If Not IsNumeric(lblWriteOff.Text.Trim()) Then
                        lblWriteOff.Text = "0.00"
                    End If
                    If Math.Abs(Convert.ToDecimal(lblWriteOff.Text.Trim())) > 0 AndAlso (String.IsNullOrEmpty(writeOffReason.Value) OrElse writeOffReason.Value = 0) Then
                        args.IsValid = False
                    Else
                        args.IsValid = True
                    End If
                Else
                    args.IsValid = True
                End If
            End If
        End Sub

        Protected Sub custvldWriteoffLimit_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles custvldWriteoffLimit.ServerValidate
            If Page.IsValid Then

                Dim writeOffAmount As Decimal = 0
                Dim writeOffLimit As Decimal
                If gvAllocate.Rows.Count > 0 Then
                    Dim lblWriteOff As Label = DirectCast(gvAllocate.FooterRow.FindControl("lblWriteOffTotal"), Label)
                    If Not IsNumeric(lblWriteOff.Text.Trim) Then
                        lblWriteOff.Text = "0.00"
                    End If
                    writeOffAmount = Convert.ToDecimal(lblWriteOff.Text.Trim())
                End If
                writeOffLimit = Convert.ToDecimal(hidden_limitwriteOffamount.Value.Trim())
                Dim oCurrencies As Nexus.Library.Config.Currencies

                oCurrencies = CType(GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Currencies

                If Math.Abs(writeOffAmount) > Math.Abs(writeOffLimit) Then
                    args.IsValid = False
                    Dim errorMsg As String = Convert.ToString(GetLocalResourceObject("lbl_WriteOff_Limit"))
                    errorMsg = Replace(errorMsg, "<Difference>", writeOffAmount.ToString())
                    errorMsg = Replace(errorMsg, "<Limit>", hidden_limitwriteOffamount.Value.Trim())
                    errorMsg = Replace(errorMsg, "<Currency>", oCurrencies.Currency(hidden_WriteOffCurrency.Value.Trim()).Display)
                    custvldWriteoffLimit.ErrorMessage = errorMsg
                Else
                    args.IsValid = True
                End If
            End If
        End Sub

        Protected Sub custvldValidateBalance_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles custvldValidateBalance.ServerValidate
            If Not IsNumeric(txtBalanceAmount.Text.Trim()) Then
                txtBalanceAmount.Text = "0.00"
            End If
            If Math.Abs(Convert.ToDecimal(txtBalanceAmount.Text.Trim())) > 0 Then
                args.IsValid = False
            Else
                args.IsValid = True
            End If
        End Sub

#End Region

        Protected Sub txtAllocated_TextChanged(sender As Object, e As EventArgs)
            Dim rowIndex As Integer = (TryCast((TryCast(sender, TextBox)).NamingContainer, GridViewRow)).RowIndex
            Dim txtAllocated As TextBox

            txtAllocated = gvAllocate.Rows(rowIndex).FindControl("txtAllocated")
            If String.IsNullOrEmpty(txtAllocated.Text.Trim()) OrElse Not IsNumeric(txtAllocated.Text.Trim()) Then
                txtAllocated.Text = "0.00"
            End If
            txtAllocated.Text = Convert.ToDecimal(txtAllocated.Text.Trim()).ToString("N2")

            CalculateTotals()
        End Sub

        Protected Sub txtWriteOff_TextChanged(sender As Object, e As EventArgs)
            Dim rowIndex As Integer = (TryCast((TryCast(sender, TextBox)).NamingContainer, GridViewRow)).RowIndex
            Dim txtWriteOff As TextBox

            txtWriteOff = gvAllocate.Rows(rowIndex).FindControl("txtWriteOff")
            If Not String.IsNullOrEmpty(txtWriteOff.Text.Trim()) AndAlso Not IsNumeric(txtWriteOff.Text.Trim()) Then
                txtWriteOff.Text = String.Empty
            ElseIf Not String.IsNullOrEmpty(txtWriteOff.Text.Trim()) AndAlso IsNumeric(txtWriteOff.Text.Trim()) Then
                txtWriteOff.Text = Convert.ToDecimal(txtWriteOff.Text.Trim()).ToString("N2")
            End If

            CalculateTotals()
        End Sub
        Protected Sub txtCurrencyDiff_TextChanged(sender As Object, e As EventArgs)
            Dim rowIndex As Integer = (TryCast((TryCast(sender, TextBox)).NamingContainer, GridViewRow)).RowIndex
            Dim txtCurrencyDiff As TextBox

            Dim lblOSAmount As Label
            Dim txtAllocated As TextBox
            txtCurrencyDiff = gvAllocate.Rows(rowIndex).FindControl("txtCurrencyDiff")
            lblOSAmount = gvAllocate.Rows(rowIndex).FindControl("lblOSAmount")
            txtAllocated = gvAllocate.Rows(rowIndex).FindControl("txtAllocated")
            If Not String.IsNullOrEmpty(txtCurrencyDiff.Text.Trim()) AndAlso IsNumeric(txtCurrencyDiff.Text.Trim()) Then
                txtCurrencyDiff.Text = Convert.ToDecimal(lblOSAmount.Text) - Convert.ToDecimal(txtAllocated.Text)
                txtCurrencyDiff.Text = Convert.ToDecimal(txtCurrencyDiff.Text.Trim()).ToString("N2")
            End If

            CalculateTotals()
        End Sub

        Sub CalculateTotals()
            Dim outStandingAmount As Decimal = 0
            Dim allocatedAmount As Decimal = 0
            Dim writeOffAmount As Decimal = 0
            Dim currencyDiffAmount As Decimal = 0
            'Calculate allocation and Write Off total
            For Each row In gvAllocate.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    outStandingAmount = outStandingAmount + Convert.ToDecimal(DirectCast(row.FindControl("lblOSAmount"), Label).Text.Trim())
                    allocatedAmount = allocatedAmount + Convert.ToDecimal(IIf(IsNumeric(DirectCast(row.FindControl("txtAllocated"), TextBox).Text.Trim()),
                                                                              DirectCast(row.FindControl("txtAllocated"), TextBox).Text.Trim(), 0))
                    writeOffAmount = writeOffAmount + Convert.ToDecimal(IIf(IsNumeric(DirectCast(row.FindControl("txtWriteOff"), TextBox).Text.Trim()),
                                                                            DirectCast(row.FindControl("txtWriteOff"), TextBox).Text.Trim(), 0))
                    If row.FindControl("txtCurrencyDiff") IsNot Nothing Then
                        currencyDiffAmount = currencyDiffAmount + Convert.ToDecimal(IIf(IsNumeric(DirectCast(row.FindControl("txtCurrencyDiff"), TextBox).Text.Trim()),
                                                                            DirectCast(row.FindControl("txtCurrencyDiff"), TextBox).Text.Trim(), 0))
                    End If
                End If
            Next
            If gvAllocate.Rows.Count > 0 Then
                Dim lblOSAmountTotal As Label = gvAllocate.FooterRow.FindControl("lblOSAmountTotal")
                Dim lblAllocatedTotal As Label = gvAllocate.FooterRow.FindControl("lblAllocatedTotal")
                Dim lblWriteOffTotal As Label = gvAllocate.FooterRow.FindControl("lblWriteOffTotal")
                Dim lblCurrencyDiffTotal As Label = gvAllocate.FooterRow.FindControl("lblCurrencyDiffTotal")
                lblOSAmountTotal.Text = outStandingAmount.ToString("N2")
                lblAllocatedTotal.Text = allocatedAmount.ToString("N2")
                lblWriteOffTotal.Text = writeOffAmount.ToString("N2")
                txtBalanceAmount.Text = allocatedAmount.ToString("N2")
                lblCurrencyDiffTotal.Text = currencyDiffAmount.ToString("N2")
                hidden_CurrencyDiff.Value = lblCurrencyDiffTotal.Text
            End If
            If writeOffAmount <> 0 Then
                writeOffReason.Enabled = True
            Else
                writeOffReason.Enabled = False
                writeOffReason.Value = 0
            End If

            updTotals.Update()
        End Sub

    End Class
End Namespace
