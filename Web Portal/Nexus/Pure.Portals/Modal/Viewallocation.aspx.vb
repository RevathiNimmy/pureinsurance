Imports System.Collections.Generic
Imports Nexus.Constants.Session
Imports Nexus.Utils

Partial Class Modal_Viewallocation
    Inherits System.Web.UI.Page

    Dim bTransactionsAlreadyReversed As Boolean
    Dim bTransactionsAlreadyReversal As Boolean
    Dim bAllowPartialReversal As Boolean = 1

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        CMS.Library.Frontend.Functions.SetTheme(Page, ConfigurationManager.AppSettings("ModalPageTemplate"))
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            'Initialze the allocation collections
            Dim oAllocationCollection As NexusProvider.AllocationDetailsCollections = Nothing
            Dim oAllocationCollectionforcredits As New NexusProvider.AllocationDetailsCollections
            Dim oAllocationCollectionfordebits As New NexusProvider.AllocationDetailsCollections
            'seperatly define the allocation details cache id for Debit and credit
            Dim AllocationDebitCacheID, AllocationCreditCacheID As Guid
            'seperatly define the reversal/reversed status cache id for Debit and credit
            Dim TransactionsAlreadyReversedCacheID, TransactionsAlreadyReversalCacheID As Guid
            'make an instance of UserAuthority class
            Dim oUserAuthority As New NexusProvider.UserAuthority
            Dim dAllocExpiryDate As Date
            Dim isValidTransactions As Boolean = True
            btnReverseAllocation.Enabled = False
            Try
                'Get the user name from session
                oUserAuthority.UserCode = Session(CNLoginName)
                'set the authority options for reverse allocation
                oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.AllowReverseAllocation
                oWebService = New NexusProvider.ProviderManager().Provider
                'initiate the GetUserAuthority method
                oWebService.GetUserAuthorityValue(oUserAuthority)

                'insert into hidden field
                'set the authority value in hidden field.
                hidChkAuthority.Value = oUserAuthority.UserAuthorityValue
                'set the no of days value in hidden field.
                hidNoOfDays.Value = oUserAuthority.UserAuthorityOptionalValue1
                'initialize the debit cache id
                AllocationDebitCacheID = Guid.NewGuid()
                ViewState.Add("AllocationDebitCacheID", AllocationDebitCacheID.ToString)
                'initialize the credit cache id
                AllocationCreditCacheID = Guid.NewGuid
                ViewState.Add("AllocationCreditCacheID", AllocationCreditCacheID.ToString)

                'initialize the TransactionsAlreadyReversedCacheID
                TransactionsAlreadyReversedCacheID = Guid.NewGuid
                ViewState.Add("TransactionsAlreadyReversedCacheID", TransactionsAlreadyReversedCacheID.ToString)

                'initialize the TransactionsAlreadyReversalCacheID
                TransactionsAlreadyReversalCacheID = Guid.NewGuid
                ViewState.Add("TransactionsAlreadyReversalCacheID", TransactionsAlreadyReversalCacheID.ToString)

                Dim oCurrencyColl As NexusProvider.CurrencyCollection
                oCurrencyColl = oWebService.GetCurrenciesByBranch(Session(CNBranchCode))
                ViewState("BaseCurerency") = oCurrencyColl(0).BaseCurrencyCode

                If Request("Transdetailkey") <> String.Empty Then 'Getting Transdetailkey from Query string
                    Dim oAllocation As New NexusProvider.AllocationDetails
                    oAllocation.TransdetailKey = CInt(Request("Transdetailkey"))
                    oAllocationCollection = oWebService.GetAllocationDetails(oAllocation.TransdetailKey)
                    If oAllocationCollection IsNot Nothing Then
                        Dim iCountVar As Integer = 0
                        For iCountVar = 0 To oAllocationCollection.Count - 1
                            If oAllocationCollection(iCountVar).AllocatedAmount < 0 Then
                                Dim bItemExists As Boolean = False
                                For Each oItem As NexusProvider.AllocationDetails In oAllocationCollectionforcredits
                                    If oItem.TransdetailKey = oAllocationCollection(iCountVar).TransdetailKey Then
                                        'Item Alreadt Exists
                                        oItem.AllocatedAmount += oAllocationCollection(iCountVar).AllocatedAmount
                                        bItemExists = True
                                    End If
                                Next
                                If bItemExists = False Then
                                    oAllocationCollectionforcredits.Add(oAllocationCollection(iCountVar))
                                End If
                            Else
                                Dim bItemExists As Boolean = False
                                For Each oItem As NexusProvider.AllocationDetails In oAllocationCollectionfordebits
                                    If oItem.TransdetailKey = oAllocationCollection(iCountVar).TransdetailKey Then
                                        'Item Alreadt Exists
                                        oItem.AllocatedAmount += oAllocationCollection(iCountVar).AllocatedAmount
                                        bItemExists = True
                                    End If
                                Next
                                If bItemExists = False Then
                                    oAllocationCollectionfordebits.Add(oAllocationCollection(iCountVar))
                                End If
                            End If

                            hidAllocationDate.Value = oAllocationCollection(iCountVar).AllocatedDate
                            hidAllocID.Value = oAllocationCollection(iCountVar).AllocationKey
                            hidTransID.Value = oAllocation.TransdetailKey
                            hidAccountKey.Value = oAllocation.AccountKey
                        Next

                        'Check if allocation is already reversed
                        For iCountVar = 0 To oAllocationCollection.Count - 1
                            If oAllocationCollection(iCountVar).Spare IsNot Nothing Then
                                If oAllocationCollection(iCountVar).Spare.Contains("Reversal") Then
                                    bTransactionsAlreadyReversal = True
                                    Exit For
                                ElseIf oAllocationCollection(iCountVar).Spare.Contains("Reversed") Then
                                    bTransactionsAlreadyReversed = True
                                    Exit For
                                End If
                            End If
                            'Check for Instalment Entry
                            If oAllocationCollection(iCountVar).DocRef <> "" Then
                                'Manual Reversals for Instalments is not allowed
                                If ChkDocTypeIsInstalments(oAllocationCollection(iCountVar).DocRef.ToString().Substring(0, 3)) Then
                                    isValidTransactions = False
                                    Exit For
                                End If
                            End If
                        Next

                        For iCountVar = 0 To oAllocationCollection.Count - 1
                            If oAllocationCollection(iCountVar).DocRef <> "" Then
                                Dim doctype As String = oAllocationCollection(iCountVar).DocRef.ToString().Substring(0, 3).ToUpper()
                                If (doctype = "SWD" OrElse doctype = "SCD") Then
                                    bAllowPartialReversal = 0
                                    lblPartialAllocNote.Visible = True
                                    Exit For
                                End If
                            End If
                        Next
                        'Calculating Allocation Expiry Date
                        'Check if allocation is already reversed
                        For iCountVar = 0 To oAllocationCollection.Count - 1
                            If Not String.IsNullOrEmpty(oAllocationCollection(iCountVar).AllocatedDate) Then
                                dAllocExpiryDate = DateAdd(DateInterval.Day, CInt(hidNoOfDays.Value), oAllocationCollection(iCountVar).AllocatedDate)
                            End If
                        Next
                        If CDate(dAllocExpiryDate) >= CDate(Now) Then
                            hidExpAlloc.Value = "0"
                        Else
                            hidExpAlloc.Value = "1" ' Allocation Expired
                        End If

                        Cache.Insert(ViewState("TransactionsAlreadyReversedCacheID"), bTransactionsAlreadyReversed, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
                        Cache.Insert(ViewState("TransactionsAlreadyReversalCacheID"), bTransactionsAlreadyReversal, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
                        gvCredits.DataSource = oAllocationCollectionforcredits 'To display Credit in one Grid 
                        Cache.Insert(ViewState("AllocationDebitCacheID"), oAllocationCollectionforcredits, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
                        gvDebit.DataSource = oAllocationCollectionfordebits 'To display debit in other Grid 
                        Cache.Insert(ViewState("AllocationCreditCacheID"), oAllocationCollectionfordebits, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
                        gvCredits.DataBind()
                        gvDebit.DataBind()
                    End If
                End If

                'make the Reverseallocation button visible on or off according to the authority value
                If Convert.ToString(hidChkAuthority.Value) = "1" AndAlso isValidTransactions = True Then
                    btnReverseAllocation.Visible = True
                    If bTransactionsAlreadyReversal = True Then
                        btnReverseAllocation.Attributes.Add("onclick", "javascript:ShowMsg('" & GetLocalResourceObject("AlreadyReversalMessage").ToString & "')")
                    ElseIf bTransactionsAlreadyReversed = True Then
                        btnReverseAllocation.Attributes.Add("onclick", "javascript:ShowMsg('" & GetLocalResourceObject("AlreadyReversedMessage").ToString & "')")
                    End If
                End If
            Finally
                oAllocationCollection = Nothing
                oWebService = Nothing
                oAllocationCollectionforcredits = Nothing
                oAllocationCollectionforcredits = Nothing
            End Try
        End If
    End Sub

    Protected Sub gvCredits_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvCredits.DataBinding
        If Not Page.IsPostBack Then
            Dim oCurrencies As Nexus.Library.Config.Currencies
            oCurrencies = CType(System.Configuration.ConfigurationManager.GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Currencies
            gvCredits.Columns(3).HeaderText = Replace(gvCredits.Columns(3).HeaderText, "[!BaseCurrency!]", oCurrencies.Currency(ViewState("BaseCurerency")).Symbol)
            gvCredits.Columns(4).HeaderText = Replace(gvCredits.Columns(4).HeaderText, "[!BaseCurrency!]", oCurrencies.Currency(ViewState("BaseCurerency")).Symbol)
            gvCredits.Columns(5).HeaderText = Replace(gvCredits.Columns(5).HeaderText, "[!BaseCurrency!]", oCurrencies.Currency(ViewState("BaseCurerency")).Symbol)
        End If
    End Sub
    ''' <summary>
    '''here "select" link button will made invisible if user does not have authority to reveerse the allocations
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub gvCredits_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvCredits.DataBound
        If hidChkAuthority.Value = "0" Or String.IsNullOrEmpty(hidChkAuthority.Value) Then
            gvCredits.Columns(gvCredits.Columns.Count - 1).Visible = False
        End If
    End Sub
    ''' <summary>
    '''here "select" command button will be handled to check the time limit and will show the message accordingly 
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub gvCredits_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCredits.RowCommand
        If e.CommandName IsNot Nothing Then
            Select Case e.CommandName
                Case "Select"
                    ProcessSelectCommand()
            End Select
        End If
    End Sub
    ''' <summary>
    '''this wiil be called when select button will be clicked will check the time limit and will show the message accordingly 
    ''' </summary>
    ''' <remarks></remarks>
    Sub ProcessSelectCommand()
        If ViewState("TransactionsAlreadyReversedCacheID") Is Nothing Or ViewState("TransactionsAlreadyReversalCacheID") Is Nothing Then
            If Request("Transdetailkey") <> String.Empty Then 'Getting Transdetailkey from Query string
                Dim oAllocation As New NexusProvider.AllocationDetails
                Dim oAllocationCollection As NexusProvider.AllocationDetailsCollections = Nothing
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                oAllocation.TransdetailKey = CInt(Request("Transdetailkey"))

                Try
                    oAllocationCollection = oWebService.GetAllocationDetails(oAllocation.TransdetailKey)
                Finally
                    oWebService = Nothing
                End Try

                If oAllocationCollection IsNot Nothing Then
                    Dim iCountVar As Integer = 0

                    'Check if allocation is already reversed
                    For iCountVar = 0 To oAllocationCollection.Count - 1
                        If oAllocationCollection(iCountVar).Spare IsNot Nothing Then
                            If oAllocationCollection(iCountVar).Spare.Contains("Reversal") Then
                                bTransactionsAlreadyReversal = True
                                Exit For
                            ElseIf oAllocationCollection(iCountVar).Spare.Contains("Reversed") Then
                                bTransactionsAlreadyReversed = True
                                Exit For
                            End If
                        End If
                    Next

                    Cache.Insert(ViewState("TransactionsAlreadyReversedCacheID"), bTransactionsAlreadyReversed, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
                    Cache.Insert(ViewState("TransactionsAlreadyReversalCacheID"), bTransactionsAlreadyReversal, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
                End If
            End If
        End If
        Dim sTotalDebit As String = String.Empty
        Dim sTotalCredit As String = String.Empty
        If gvDebit.Rows.Count > 0 Then
            Dim lblDebitAllocatedTotal As Label = gvDebit.FooterRow.FindControl("lblDebitAllocatedTotal")
            sTotalDebit = IIf(lblDebitAllocatedTotal.Text = "", 0.00, lblDebitAllocatedTotal.Text)
        End If
        If gvCredits.Rows.Count > 0 Then
            Dim lblCreditAllocatedTotal As Label = gvCredits.FooterRow.FindControl("lblCreditAllocatedTotal")
            sTotalCredit = IIf(lblCreditAllocatedTotal.Text = "", 0.00, lblCreditAllocatedTotal.Text)
        End If
        Dim dDebitAllocatedAmount As Decimal = Math.Abs(IIf(String.IsNullOrEmpty(sTotalDebit), Convert.ToDecimal("0.00"), Convert.ToDecimal(sTotalDebit)))
        Dim dCreditAllocatedAmount As Decimal = Math.Abs(IIf(String.IsNullOrEmpty(sTotalCredit), Convert.ToDecimal("0.00"), Convert.ToDecimal(sTotalCredit)))
        Dim sTotalWarning As String = GetLocalResourceObject("TotalWarning").ToString
        Dim bWarnning As String = "True"
        If (Not dDebitAllocatedAmount.Equals(dCreditAllocatedAmount)) Then
            bWarnning = "false"
        End If
        If hidExpAlloc.Value = "1" Then
            btnReverseAllocation.Attributes.Add("onclick", "javascript:ShowMsg('" & GetLocalResourceObject("ValidationMessage").ToString & "')")
        Else
            If Cache.Item(ViewState("TransactionsAlreadyReversalCacheID")) = True Then
                btnReverseAllocation.Attributes.Add("onclick", "javascript:ShowMsg('" & GetLocalResourceObject("AlreadyReversalMessage").ToString & "')")
            ElseIf Cache.Item(ViewState("TransactionsAlreadyReversedCacheID")) = True Then
                btnReverseAllocation.Attributes.Add("onclick", "javascript:ShowMsg('" & GetLocalResourceObject("AlreadyReversedMessage").ToString & "')")
            Else
                Dim sSplitMessage As String = GetLocalResourceObject("SplitAllocationMessage").ToString
                Dim sMessage As String = String.Empty
                sMessage = GetCustomMessage()
                'show the confirmation message with no. of allocation which will be reversed.
                btnReverseAllocation.Attributes.Add("onclick", "javascript:return ReversalConfirmation('" & sMessage & "','" & sSplitMessage & "','" & bWarnning & "','" & sTotalWarning & "')")
            End If
        End If
    End Sub
    ''' <summary>
    '''this method will generate the custom message containing doc ref and amount.
    ''' </summary>
    ''' <remarks></remarks>
    Function GetCustomMessage() As String
        Dim sCustomMessage As String = String.Empty
        'check whether view states are nothing or not.
        If ViewState("AllocationCreditCacheID") IsNot Nothing AndAlso ViewState("AllocationDebitCacheID") IsNot Nothing Then
            Dim oAllocationCollectionforcredits As New NexusProvider.AllocationDetailsCollections
            Dim oAllocationCollectionfordebits As New NexusProvider.AllocationDetailsCollections
            Dim sMessageBuilder As New StringBuilder

            sMessageBuilder.Append(GetLocalResourceObject("WarningMessage2").ToString)
            sMessageBuilder.Append("\n")
            sMessageBuilder.Append("\n")
            'get the allocation details
            oAllocationCollectionforcredits = CType(Cache.Item(ViewState("AllocationCreditCacheID")), NexusProvider.AllocationDetailsCollections)
            oAllocationCollectionfordebits = CType(Cache.Item(ViewState("AllocationDebitCacheID")), NexusProvider.AllocationDetailsCollections)

            'initalize the custome message.
            sCustomMessage = String.Empty
            'loop through the credit allocation details and match the allocation key with hidden allocation ID

            For Each row In gvCredits.Rows
                Dim chkCredit As CheckBox = TryCast(row.Cells(0).Controls(1), CheckBox)
                Dim txtAllocated As TextBox = DirectCast(row.FindControl("txtAllocated"), TextBox)
                Dim sReverseAmount As String = IIf(String.IsNullOrEmpty(txtAllocated.Text), "0.00", txtAllocated.Text)
                If chkCredit IsNot Nothing AndAlso chkCredit.Checked Then
                    sMessageBuilder.Append(row.Cells(1).Text.Trim)
                    sMessageBuilder.Append("=")
                    sMessageBuilder.Append(sReverseAmount.ToString.Trim)
                    sMessageBuilder.Append("\n")
                End If
            Next

            For Each row In gvDebit.Rows
                Dim chkDebit As CheckBox = TryCast(row.Cells(0).Controls(1), CheckBox)
                Dim txtAllocated As TextBox = DirectCast(row.FindControl("txtAllocatedDebit"), TextBox)
                Dim sReverseAmount As String = IIf(String.IsNullOrEmpty(txtAllocated.Text), "0.00", txtAllocated.Text)
                If chkDebit IsNot Nothing AndAlso chkDebit.Checked Then
                    sMessageBuilder.Append(row.Cells(1).Text.Trim)
                    sMessageBuilder.Append("=")
                    sMessageBuilder.Append(sReverseAmount.ToString.Trim)
                    sMessageBuilder.Append("\n")
                End If
            Next

            'check the lenght of message
            If sMessageBuilder.Length > 0 Then
                'insert the last question in the confirmation message 
                sMessageBuilder.Append("\n")
                sMessageBuilder.Append("Are You Sure?")
                sCustomMessage = sMessageBuilder.ToString
            End If
            sMessageBuilder = Nothing
        End If
        Return sCustomMessage
    End Function
    Protected Sub gvCredits_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCredits.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim oItem As NexusProvider.AllocationDetails = CType(e.Row.DataItem, NexusProvider.AllocationDetails)
            e.Row.Cells(2).Text = oItem.TransDate.ToShortDateString
            e.Row.Cells(3).Text = oItem.AllocatedDate.ToShortDateString
            If (bAllowPartialReversal = 0) Then
                e.Row.Cells(0).Enabled = False
                e.Row.Cells(10).Enabled = False
            End If
            'e.Row.Cells(3).Text = New Money(oItem.AllocatedAmount, oItem.CurrencyCode).Formatted
            'e.Row.Cells(4).Text = New Money(oItem.OriginalAmount, oItem.CurrencyCode).Formatted
            'e.Row.Cells(5).Text = New Money(oItem.WriteOffAmount, oItem.CurrencyCode).Formatted
        End If
    End Sub

    Protected Sub gvDebit_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvDebit.DataBinding
        If Not Page.IsPostBack Then
            Dim oCurrencies As Nexus.Library.Config.Currencies
            oCurrencies = CType(System.Configuration.ConfigurationManager.GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Currencies
            gvDebit.Columns(3).HeaderText = Replace(gvDebit.Columns(3).HeaderText, "[!BaseCurrency!]", oCurrencies.Currency(ViewState("BaseCurerency")).Symbol)
            gvDebit.Columns(4).HeaderText = Replace(gvDebit.Columns(4).HeaderText, "[!BaseCurrency!]", oCurrencies.Currency(ViewState("BaseCurerency")).Symbol)
            gvDebit.Columns(5).HeaderText = Replace(gvDebit.Columns(5).HeaderText, "[!BaseCurrency!]", oCurrencies.Currency(ViewState("BaseCurerency")).Symbol)

        End If
    End Sub
    ''' <summary>
    '''here "select" link button will made invisible if user does not have authority to reveerse the allocations
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub gvDebit_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvDebit.DataBound
        If hidChkAuthority.Value = "0" Or String.IsNullOrEmpty(hidChkAuthority.Value) Then
            gvDebit.Columns(gvDebit.Columns.Count - 1).Visible = False
        End If
       ' gvDebit.HeaderRow.Cells(9).Visible = False
       ' gvDebit.FooterRow.Cells(9).Visible = False
    End Sub
    ''' <summary>
    '''here "select" command button will be handled to check the time limit and will show the message accordingly 
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub gvDebit_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDebit.RowCommand
        If e.CommandName IsNot Nothing Then

            Select Case e.CommandName
                Case "Select"
                    'Process the select command and generate custom message
                    ProcessSelectCommand()
            End Select
        End If
    End Sub

    Protected Sub gvDebit_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDebit.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim oItem As NexusProvider.AllocationDetails = CType(e.Row.DataItem, NexusProvider.AllocationDetails)
            Dim txtAllocated As TextBox = DirectCast(e.Row.FindControl("txtAllocatedDebit"), TextBox)
            If (bAllowPartialReversal = 0) Then
                e.Row.Cells(0).Enabled = False
                e.Row.Cells(10).Enabled = False
            End If
            e.Row.Cells(2).Text = CType(e.Row.DataItem, NexusProvider.AllocationDetails).TransDate.ToShortDateString
            e.Row.Cells(3).Text = CType(e.Row.DataItem, NexusProvider.AllocationDetails).AllocatedDate.ToShortDateString


        End If
    End Sub
    ''' <summary>
    '''this will handle the reversal button click event
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnReverseAllocation_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReverseAllocation.Click
        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim lstReAllocationDetails As List(Of String) = New List(Of String)
        Dim lstCreditAllocation As List(Of String) = New List(Of String)
        Dim sAccountKey As Integer = 0
        If Not String.IsNullOrEmpty(Request("AccountKey")) Then
            sAccountKey = Convert.ToInt32(Request("AccountKey").ToString)
        End If

        Try
            Dim chkTranskey As New CheckBox
            For Each row In gvDebit.Rows
                Dim chkDebit As CheckBox = TryCast(row.Cells(0).Controls(1), CheckBox)
                If chkDebit IsNot Nothing AndAlso chkDebit.Checked Then
                    Dim txtAllocated As TextBox = DirectCast(row.FindControl("txtAllocatedDebit"), TextBox)

                    Dim dAllocatedAmount As Decimal = Convert.ToDecimal(row.Cells(4).Text)
                    Dim dReverseAmount As Decimal = Convert.ToDecimal(txtAllocated.Text)
                    If (dAllocatedAmount - dReverseAmount <> 0.00) Then
                        If Not lstReAllocationDetails.Contains(row.Cells(9).Text) Then
                            lstReAllocationDetails.Add(row.Cells(9).Text & "|" & (dAllocatedAmount - dReverseAmount) & "|" & row.Cells(1).Text)
                        End If
                    End If
                Else
                    If Not lstReAllocationDetails.Contains(row.Cells(9).Text) Then
                        lstReAllocationDetails.Add(row.Cells(9).Text & "|" & row.Cells(4).Text & "|" & row.Cells(1).Text)
                    End If
                End If
            Next
            For Each row In gvCredits.Rows
                Dim chkCredit As CheckBox = TryCast(row.Cells(0).Controls(1), CheckBox)
                If chkCredit IsNot Nothing AndAlso chkCredit.Checked Then
                    Dim txtAllocated As TextBox = DirectCast(row.FindControl("txtAllocated"), TextBox)
                    Dim dAllocatedAmount As Decimal = Math.Abs(Convert.ToDecimal(row.Cells(4).Text))
                    Dim dReverseAmount As Decimal = Math.Abs(Convert.ToDecimal(txtAllocated.Text))
                    Dim dFinalReverseAmount As Decimal = dAllocatedAmount - dReverseAmount
                    If (dAllocatedAmount - dReverseAmount <> 0.00) Then
                        If Not lstReAllocationDetails.Contains(row.Cells(9).Text) Then
                            lstReAllocationDetails.Add(row.Cells(9).Text & "|" & (-1 * dFinalReverseAmount) & "|" & row.Cells(1).Text)
                        End If
                    End If
                Else
                    If Not lstReAllocationDetails.Contains(row.Cells(9).Text) Then
                        lstReAllocationDetails.Add(row.Cells(9).Text & "|" & row.Cells(4).Text & "|" & row.Cells(1).Text)
                    End If
                    If Not lstCreditAllocation.Contains(row.Cells(9).Text) Then
                        lstCreditAllocation.Add(row.Cells(9).Text & "|" & row.Cells(4).Text & "|" & row.Cells(1).Text)
                    End If
                End If
            Next

            'check whether user has choosen to proceed or not
            If hidChkChoice.Value.Trim.ToUpper = "TRUE" Then
                'if user has choosen "yes" then call the SAM method to reverse the allocation.
                Dim sWarnings As String = String.Empty
                oWebService = New NexusProvider.ProviderManager().Provider
                'set information label empty
                lblInformation.Text = String.Empty
                sWarnings = oWebService.ReverseAllocation(Convert.ToInt32(hidTransID.Value), Convert.ToInt32(hidAllocID.Value))
                If Not (String.IsNullOrEmpty(sWarnings)) Then
                    'set information label visble
                    lblInformation.Visible = True
                    'set the description into label text
                    lblInformation.Text = sWarnings
                Else
                    'post back to Search transaction page.
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "closeThickBox", "self.parent.RefreshReverseAllocation();", True)
                End If
            End If
            If lstReAllocationDetails.Count > 0 Then
                Dim oUpdateAllocation As New NexusProvider.AllocationDetails
                For i As Integer = 1 To lstReAllocationDetails.Count

                    Dim oReAlloctionDetail As New NexusProvider.Allocation
                    Dim sAllocationdata As String = lstReAllocationDetails(i - 1)
                    Dim sAllocationDetails As String() = sAllocationdata.Split("|")
                    oReAlloctionDetail.AllocationAmount = Convert.ToDouble(sAllocationDetails(1).ToString().Trim())
                    oReAlloctionDetail.AllocationTransdetailKey = Convert.ToInt32(sAllocationDetails(0).ToString().Trim())

                    If (oUpdateAllocation.TransdetailKey = 0) Then
                        oUpdateAllocation.TransdetailKey = Convert.ToInt32(sAllocationDetails(0).ToString().Trim())
                        oUpdateAllocation.Amount = 0
                        oUpdateAllocation.AccountKey = sAccountKey
                    End If
                    Dim oAllocationDetailsCollections As New NexusProvider.AllocationDetailsCollections
                    Dim oAllocationDetails As New NexusProvider.AllocationDetails

                    oAllocationDetails.TransdetailKey = Convert.ToInt32(sAllocationDetails(0).ToString().Trim())
                    oAllocationDetailsCollections.Add(oAllocationDetails)

                    Dim oTrasactionDetails As New NexusProvider.AllocationDetailsCollections
                    oTrasactionDetails = oWebService.GetTransactionDetails(sAccountKey, oAllocationDetailsCollections)
                    Dim bTimespan As Byte()
                    For Each oTempAllocationDetails As NexusProvider.AllocationDetails In oTrasactionDetails
                        If (oTempAllocationDetails.TransdetailKey = oReAlloctionDetail.AllocationTransdetailKey) Then
                            bTimespan = oTempAllocationDetails.AllocationTimeStamp
                        End If
                    Next
                    oReAlloctionDetail.AllocationTimeStamp = bTimespan
                    oUpdateAllocation.Allocation.Add(oReAlloctionDetail)
                Next i

                Dim sUpdateWarnings As String = String.Empty
                If Page.IsValid Then
                    If Session(CNTransBranchCode) IsNot Nothing Then
                        sUpdateWarnings = oWebService.UpdateAllocation(oUpdateAllocation, Session(CNTransBranchCode).ToString())
                    Else
                        sUpdateWarnings = oWebService.UpdateAllocation(oUpdateAllocation)
                    End If
                End If
                If Not (String.IsNullOrEmpty(sUpdateWarnings)) Then
                    lblInformation.Visible = True
                    lblInformation.Text = sUpdateWarnings
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "closeThickBox", "self.parent.RefreshReverseAllocation();", True)
                End If
            End If
        Catch ex As NexusProvider.NexusException
            If ex.Errors(0).Code = "1000149" Then
                Dim sMessage As String = "alert('" + ex.Errors(0).Description + "')"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "showerrormsg", sMessage, True)
            End If
        End Try
        oWebService = Nothing
    End Sub
    Public Function ChkDocTypeIsInstalments(ByVal v_sDocType As String) As Boolean

        Dim result As Boolean = False
        Try

            Select Case v_sDocType

                Case "IDR", "ICR", "ICA", "IND", "INC", "IED", "IEC", "IRD", "IRC"
                    result = True
            End Select

            Return result

        Catch excep As System.Exception

            Return result
        End Try
    End Function
    Protected Sub chkCredit_OnCheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chkTempCheckBox As CheckBox = sender
        Dim gvCredit As GridViewRow = CType(chkTempCheckBox.NamingContainer, GridViewRow)
        Dim dAmount As Decimal = Convert.ToDecimal(gvCredit.Cells(4).Text.Trim())
        Dim txtAllocated As TextBox = TryCast(gvCredit.FindControl("txtAllocated"), TextBox)
        If chkTempCheckBox.Checked Then
            txtAllocated.Text = Math.Abs(Convert.ToDecimal(dAmount))
        Else
            txtAllocated.Text = Convert.ToDecimal(0.00)
        End If
        CalculateTotals()
        ProcessSelectCommand()
    End Sub
    Protected Sub chkDebit_OnCheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chkTempCheckBox As CheckBox = sender
        Dim gvTempGridView As GridViewRow = CType(chkTempCheckBox.NamingContainer, GridViewRow)
        Dim dAmount As Decimal = Convert.ToDecimal(gvTempGridView.Cells(4).Text.Trim())
        Dim txtAllocated As TextBox = TryCast(gvTempGridView.FindControl("txtAllocatedDebit"), TextBox)
        If chkTempCheckBox.Checked Then
            txtAllocated.Text = Convert.ToDecimal(dAmount)
        Else
            txtAllocated.Text = Convert.ToDecimal(0.00)
        End If
        CalculateTotals()
        ProcessSelectCommand()
    End Sub
    Protected Sub chkCreditSelectAll_OnCheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim chkAllCredit As CheckBox = CType(gvCredits.HeaderRow.FindControl("checkAllCredit"), CheckBox)
        For Each row As GridViewRow In gvCredits.Rows
            Dim txtAllocated As TextBox = DirectCast(row.FindControl("txtAllocated"), TextBox)
            Dim chkCredit As CheckBox = CType(row.FindControl("chkCredit"), CheckBox)
            If chkAllCredit.Checked = True Then
                chkCredit.Checked = True
            Else
                chkCredit.Checked = False
                txtAllocated.Text = Convert.ToDecimal("0.00")
            End If
        Next
        CalculateTotals()
        ProcessSelectCommand()
    End Sub

    Protected Sub chkDebitSelectAll_OnCheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim chkAllDebit As CheckBox = CType(gvDebit.HeaderRow.FindControl("checkAllDebit"), CheckBox)
        For Each row As GridViewRow In gvDebit.Rows
            Dim txtAllocated As TextBox = DirectCast(row.FindControl("txtAllocatedDebit"), TextBox)
            Dim chkDebit As CheckBox = CType(row.FindControl("chkDebit"), CheckBox)

            If chkAllDebit.Checked = True Then
                chkDebit.Checked = True
            Else
                chkDebit.Checked = False
                txtAllocated.Text = Convert.ToDecimal("0.00")
            End If
        Next
        CalculateTotals()
        ProcessSelectCommand()
    End Sub
    Protected Sub txtAllocated_TextChanged(sender As Object, e As EventArgs)

        Dim iRowIndex As Integer = (TryCast((TryCast(sender, TextBox)).NamingContainer, GridViewRow)).RowIndex
        Dim txtAllocated As TextBox
        txtAllocated = gvCredits.Rows(iRowIndex).FindControl("txtAllocated")
        If (Not IsNumeric(txtAllocated.Text) OrElse String.IsNullOrEmpty(txtAllocated.Text.Trim) OrElse Convert.ToDouble(txtAllocated.Text) > Decimal.MaxValue) Then
            txtAllocated.Text = Convert.ToDecimal(0.00)
        End If
        Dim dReverseAmount As Decimal = IIf(IsNumeric(txtAllocated.Text), Math.Abs(Convert.ToDecimal(txtAllocated.Text)), 0.00)
        Dim sReverseFormattedNumber As String = dReverseAmount.ToString("N2")
        If (Not txtAllocated.Text.Trim.Contains(".")) Then
            txtAllocated.Text = Math.Round(Convert.ToDouble(sReverseFormattedNumber), 2).ToString("N2")
        Else
            txtAllocated.Text = Math.Round(Convert.ToDouble(dReverseAmount), 2).ToString("N2")
        End If
        CalculateTotals()
        ProcessSelectCommand()
    End Sub
    Protected Sub txtAllocatedDebit_TextChanged(sender As Object, e As EventArgs)
        Dim iRowIndex As Integer = (TryCast((TryCast(sender, TextBox)).NamingContainer, GridViewRow)).RowIndex
        Dim txtAllocated As TextBox
        txtAllocated = gvDebit.Rows(iRowIndex).FindControl("txtAllocatedDebit")
        If (Not IsNumeric(txtAllocated.Text) OrElse String.IsNullOrEmpty(txtAllocated.Text.Trim) OrElse Convert.ToDouble(txtAllocated.Text) > Decimal.MaxValue) Then
            txtAllocated.Text = Convert.ToDecimal(0.00)
        End If
        Dim dReverseAmount As Decimal = IIf(IsNumeric(txtAllocated.Text), Math.Abs(Convert.ToDecimal(txtAllocated.Text)), 0.00)
        Dim sReverseFormattedNumber As String = dReverseAmount.ToString("N2")
        If (Not txtAllocated.Text.Trim.Contains(".")) Then
            txtAllocated.Text = Math.Round(Convert.ToDouble(sReverseFormattedNumber), 2).ToString("N2")
        Else
            txtAllocated.Text = Math.Round(Convert.ToDouble(dReverseAmount), 2).ToString("N2")
        End If

        CalculateTotals()
        ProcessSelectCommand()
    End Sub
    Sub CalculateTotals()
        Dim dAllocatedAmountCredit As Decimal = 0.00
        Dim dAllocatedAmountDebit As Decimal = 0.00
        Dim bReverseEnable As Boolean = False
        Dim chkAlldebit As CheckBox = CType(gvDebit.HeaderRow.FindControl("checkAllDebit"), CheckBox)
        Dim chkAllCredit As CheckBox = CType(gvCredits.HeaderRow.FindControl("checkAllCredit"), CheckBox)
        For Each row In gvCredits.Rows
            Dim oReverseAmount As Object = 0
            Dim dAllocationAmount As Decimal = Convert.ToDecimal(row.Cells(4).Text.ToString.Trim)
            Dim txtAllocated As TextBox = DirectCast(row.FindControl("txtAllocated"), TextBox)
            oReverseAmount = IIf(String.IsNullOrEmpty(txtAllocated.Text), "0.00", txtAllocated.Text)
            Dim chkCredit As CheckBox = TryCast(row.Cells(0).Controls(1), CheckBox)
            If (chkAllCredit.Checked = False And chkCredit.Checked = False And (Convert.ToDecimal(oReverseAmount) <> 0.00)) Then
                chkCredit.Checked = True
                bReverseEnable = True
            End If
            If chkCredit IsNot Nothing AndAlso chkCredit.Checked Then
                If (Math.Abs(Convert.ToDecimal(oReverseAmount)) > Math.Abs(Convert.ToDecimal(dAllocationAmount))) Then
                    Dim sReverseAmountWarning As String = GetLocalResourceObject("ReverseAmountWarning").ToString
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ReverseAmountWarning('" & sReverseAmountWarning & "');", True)
                    txtAllocated.Text = Math.Abs(Convert.ToDecimal(dAllocationAmount)) * -1
                ElseIf (Math.Abs(Convert.ToDecimal(oReverseAmount)) > 0) Then
                    txtAllocated.Text = Math.Abs(Convert.ToDecimal(oReverseAmount)) * -1
                Else
                    txtAllocated.Text = Math.Abs(Convert.ToDecimal(dAllocationAmount)) * -1
                End If

                If row.RowType = DataControlRowType.DataRow Then
                    dAllocatedAmountCredit = dAllocatedAmountCredit + Math.Abs(Convert.ToDecimal(IIf(IsNumeric(DirectCast(row.FindControl("txtAllocated"), TextBox).Text.Trim()),
                                                                          DirectCast(row.FindControl("txtAllocated"), TextBox).Text.Trim(), 0)))
                    btnReverseAllocation.Enabled = True
                    bReverseEnable = True
                End If
            Else
                Dim chkCreditAll As CheckBox = CType(gvCredits.HeaderRow.FindControl("checkAllCredit"), CheckBox)
                chkCreditAll.Checked = False
                txtAllocated.Text = Convert.ToDecimal("0.00")
                If (bReverseEnable = False) Then
                    btnReverseAllocation.Enabled = False
                End If
            End If
        Next
        For Each row In gvDebit.Rows
            Dim oReverseAmount As Object = 0
            Dim chkDebit As CheckBox = TryCast(row.Cells(0).Controls(1), CheckBox)
            Dim txtAllocated As TextBox = DirectCast(row.FindControl("txtAllocatedDebit"), TextBox)
            oReverseAmount = IIf(String.IsNullOrEmpty(txtAllocated.Text), "0.00", txtAllocated.Text)
            If (chkAlldebit.Checked = False And chkDebit.Checked = False And (Convert.ToDecimal(oReverseAmount) <> 0.00)) Then
                chkDebit.Checked = True
                bReverseEnable = True
            End If
            If chkDebit IsNot Nothing AndAlso chkDebit.Checked Then
                Dim allocationAmount As Decimal = Convert.ToDecimal(row.Cells(4).Text.ToString.Trim)
                If (Math.Abs(Convert.ToDecimal(oReverseAmount)) > Math.Abs(Convert.ToDecimal(allocationAmount))) Then
                    Dim sReverseAmountWarning As String = GetLocalResourceObject("ReverseAmountWarning").ToString
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ReverseAmountWarning('" & sReverseAmountWarning & "');", True)
                    txtAllocated.Text = Math.Abs(Convert.ToDecimal(allocationAmount))
                ElseIf (Math.Abs(Convert.ToDecimal(oReverseAmount)) > 0) Then
                    txtAllocated.Text = Math.Abs(Convert.ToDecimal(oReverseAmount))
                Else
                    txtAllocated.Text = Math.Abs(Convert.ToDecimal(allocationAmount))
                End If
                If row.RowType = DataControlRowType.DataRow Then
                    dAllocatedAmountDebit = dAllocatedAmountDebit + Math.Abs(Convert.ToDecimal(IIf(IsNumeric(DirectCast(row.FindControl("txtAllocatedDebit"), TextBox).Text.Trim()),
                                                                 DirectCast(row.FindControl("txtAllocatedDebit"), TextBox).Text.Trim(), 0.00)))
                    btnReverseAllocation.Enabled = True
                    bReverseEnable = True
                End If
            Else
                Dim chkDebitAll As CheckBox = CType(gvDebit.HeaderRow.FindControl("checkAllDebit"), CheckBox)
                chkDebitAll.Checked = False
                txtAllocated.Text = Convert.ToDecimal("0.00")
                If (bReverseEnable = False) Then
                    btnReverseAllocation.Enabled = False
                End If
            End If
        Next
        If gvDebit.Rows.Count > 0 Then
            Dim lblDebitAllocatedTotal As Label = gvDebit.FooterRow.FindControl("lblDebitAllocatedTotal")
            lblDebitAllocatedTotal.Text = dAllocatedAmountDebit.ToString("N2")
        End If
        If gvCredits.Rows.Count > 0 Then
            Dim lblCreditAllocatedTotal As Label = gvCredits.FooterRow.FindControl("lblCreditAllocatedTotal")
            lblCreditAllocatedTotal.Text = (dAllocatedAmountCredit * -1).ToString("N2")
        End If
    End Sub
End Class
