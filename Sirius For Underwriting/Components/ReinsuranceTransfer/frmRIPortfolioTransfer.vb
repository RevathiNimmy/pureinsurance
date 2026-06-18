Option Strict Off
Option Explicit On
Imports System.Collections.Concurrent
Imports System.Linq
Imports System.Threading.Tasks
Imports SharedFiles
Imports SSP.PureInsuranceRestAPIHandler
Imports SSP.PureInsuranceRestAPIHandler.BaseClasses
Public Class frmRIPortfolioTransfer
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmRIPortfolioTransfer
    '
    ' Date: 06/07/04
    '
    ' Description: Interface for RI portfolio transfer.
    ' ***************************************************************** '
    ' Constant for the functions to identify which class this is.
    ' Object parameter members.
#Region "Private variables"
    Private nFailureCount As Integer ' E007
    Private m_nBranchId As Integer
    Private m_nProductId As Integer
    Private m_oClaims(,) As Object
    Private m_nLoopy As Integer
    Private m_nClaimLoopy As Integer
    Private m_sUserName As String

    Private Delegate Sub SetClientNameCallback(sText As String)

    Private Delegate Sub SetPolicyNumberCallback(sText As String)

    Private Delegate Sub SetClientCodeCallback(sText As String)

    Private Delegate Sub SetStatusBarCallback(sText As String)

    Private Delegate Sub SetStartCallback(sStatus As Boolean)

    Private Delegate Sub SetCancelCallback(sStatus As Boolean)

    Private ReadOnly mPolicyQueue As ConcurrentQueue(Of Integer)
    Private ReadOnly mClaimQueue As ConcurrentQueue(Of Integer)

    Private Enum DeferredRIField
        edInsuranceFileCnt = 0
        edInsuranceRef = 1
        edClientCode = 2
        edClientName = 3
        edTransferDate = 4
        edStartDate = 5
        edEndDate = 6
        edInceptionDate = 7
        edProductId = 8
        edInsuranceFileType = 9
    End Enum

    Private Enum ClaimRIField
        edBaseClaimId = 0
        edClaimId = 1
        edInduranceFileCnt = 2
        edInsuranceRef = 3
    End Enum

    Private m_oPolicies As Object

    ' Stores the return value for the a function call.
    Private m_nReturn As Integer
    Private m_nItemsFound As Integer
    Private m_oBusiness As bsirriportfoliotransfer.Business

    Private m_nClaimItemsFound As Integer
    Private m_bProcessingPolicy As Boolean
    Private m_nTotalItemsFound As Integer
    Private dtTransferDate As Date

    Private m_nItemsForPost As Integer
    Private m_bProcessingStats As Boolean
    Private m_oPoliciesDetails As Object
#End Region

#Region "Public Class"
    Public Class ArgumentType
        Public InsuranceFileCnt As Integer
        Public TransactionType As String
    End Class
#End Region

#Region "Public Method"
    ''' <summary>
    ''' TransferPolicies
    ''' </summary>
    ''' <param name="instance"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function TransferPolicies(ByVal instance As InstanceElement) As Integer
        Dim nLoopy As Integer

        m_sUserName = instance.UserName
        While mPolicyQueue.TryDequeue(nLoopy)

            bgwTransferPolicies.ReportProgress(m_nLoopy)

            TransferPolicy(nLoopy, instance)

            m_nLoopy += 1

            If bgwTransferPolicies.CancellationPending Then
                Exit Function
            End If

        End While

        TransferPolicies = PMEReturnCode.PMTrue
    End Function

    ''' <summary>
    ''' SetCancel
    ''' </summary>
    ''' <param name="sStatus"></param>
    ''' <remarks></remarks>
    Public Sub SetCancel(ByVal sStatus As Boolean)
        If btnCancel.InvokeRequired Then
            Dim d As SetCancelCallback = New SetCancelCallback(AddressOf SetCancel)
            Me.Invoke(d, sStatus)
        Else
            btnCancel.Enabled = sStatus
        End If
    End Sub

    ''' <summary>
    ''' SetStart
    ''' </summary>
    ''' <param name="sStatus"></param>
    ''' <remarks></remarks>
    Public Sub SetStart(sStatus As Boolean)
        If btnStart.InvokeRequired Then
            Dim d As SetStartCallback = New SetStartCallback(AddressOf SetStart)
            Me.Invoke(d, sStatus)
        Else
            btnStart.Enabled = sStatus
        End If
    End Sub

    ''' <summary>
    ''' TransferClaims
    ''' </summary>
    ''' <param name="instance"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function TransferClaims(ByVal instance As InstanceElement) As Integer
        Dim nClaimLoopy As Integer
        m_sUserName = instance.UserName

        While mClaimQueue.TryDequeue(nClaimLoopy)

            bgwTransferPolicies.ReportProgress(m_nClaimLoopy + m_nLoopy)

            TransferClaim(nClaimLoopy, instance)

            m_nClaimLoopy += 1

            If bgwTransferPolicies.CancellationPending Then
                Exit Function
            End If

        End While
        ' Finished!
        TransferClaims = PMEReturnCode.PMTrue
    End Function

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        mPolicyQueue = New ConcurrentQueue(Of Integer)()
        mClaimQueue = New ConcurrentQueue(Of Integer)()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

#End Region

#Region "Private methods"
    Private m_dtOriginalTransferDate As Date
    Private m_bPageLoaded As Boolean = False

    ''' <summary>
    ''' GetBusiness
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetBusiness() As Integer

        Dim nProductID As Integer
        Dim nBranchID As Integer
        Dim nReturn As Integer

        nReturn = PMEReturnCode.PMTrue
        m_oPolicies = Nothing
        m_oClaims = Nothing
        m_oPoliciesDetails = Nothing
        ' Display a searching message

        ' Get selection criteria from interface
        nProductID = m_nProductId
        nBranchID = m_nBranchId
        dtTransferDate = txtTransferDate.Value

        ' Get matching policies
        m_nReturn = m_oBusiness.GetPoliciesPortfolioTransfer(v_lProductID:=nProductID, v_nBranchID:=nBranchID, v_dtTransferDate:=dtTransferDate, r_vPolicyArray:=m_oPolicies)

        If (m_nReturn <> PMEReturnCode.PMTrue) Then
            nReturn = PMEReturnCode.PMFalse
            Return nReturn
        End If

        ' Update the module level variable that holds the number of policies we're dealing with
        If IsArray(m_oPolicies) Then
            m_nItemsFound = UBound(m_oPolicies, 2) + 1
        Else
            m_nItemsFound = 0
        End If

        ' Do for claim as well
        m_nReturn = m_oBusiness.GetClaimsPortfolioTransfer(nProductID:=nProductID, nBranchID:=nBranchID, v_dtTransferDate:=dtTransferDate, r_oClaimsArray:=m_oClaims)
        If (m_nReturn <> PMEReturnCode.PMTrue) Then
            nReturn = PMEReturnCode.PMFalse
            Return nReturn
        End If
        If chkRunPostings.Checked Then
            m_nReturn = m_oBusiness.GetPolicyListDetails(r_oPoliciesDetails:=m_oPoliciesDetails)
            If IsArray(m_oPoliciesDetails) Then
                m_nItemsForPost = UBound(m_oPoliciesDetails, 2) + 1
            Else
                m_nItemsForPost = 0
            End If
        End If

        If IsArray(m_oClaims) Then
            m_nClaimItemsFound = UBound(m_oClaims, 2) + 1
        Else
            m_nClaimItemsFound = 0
        End If

        m_nTotalItemsFound = m_nItemsFound + m_nClaimItemsFound + m_nItemsForPost

        Return nReturn
    End Function

    ''' <summary>
    ''' SetInterfaceDefaults
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetInterfaceDefaults() As Integer

        SetInterfaceDefaults = PMEReturnCode.PMTrue

        ' Default some fields
        txtPolicyNumber.Text = ""
        txtClientCode.Text = ""
        txtClientName.Text = ""

        txtTransferDate.Value = Now.Date
        dtTransferDate = txtTransferDate.Value
    End Function

    ''' <summary>
    ''' cmdCancel_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        bgwTransferPolicies.CancelAsync()

        btnStart.Enabled = True
    End Sub

    Private Sub cmdStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click

        ' Check if Transfer Date matches the accounting year (only if date has changed from original and page is loaded)
        If m_bPageLoaded AndAlso txtTransferDate.Value <> m_dtOriginalTransferDate Then
            If MsgBox("Portfolio Transfer date does not match the beginning of accounting year. Do you want to continue?", vbQuestion + vbOKCancel, "RI Portfolio Transfer") = vbCancel Then
                Exit Sub
            End If
        End If

        ' Get policies to be processed
        m_nReturn = GetBusiness()
        spbStatus.Value = 0
        spbStatus.Maximum = m_nTotalItemsFound

        ' Display message box according to number of policies returned
        If m_nTotalItemsFound < 1 Then
            ' No policy matches criteria, warn user and do nothing

            MsgBox("No Policies Found", vbInformation + vbOKOnly, "RI Portfolio Transfer")
        Else
            ' Policies found, ask user confirmation before processing
            If chkRunPostings.Checked = True And m_nItemsFound = 0 AndAlso m_nClaimItemsFound = 0 Then
                If MsgBox(CStr(m_nItemsForPost) & " " & "Policies found for account postings . " & "Do you wish to proceed?", vbQuestion + vbYesNo, "RI Portfolio Transfer") = vbYes Then
                    btnStart.Enabled = False
                    btnCancel.Enabled = True
                    bgwTransferPolicies.RunWorkerAsync()

                End If
            Else
                If MsgBox(CStr(m_nItemsFound) & " " & "Policies Found and " & CStr(m_nClaimItemsFound) & " Claims found. " & "Do you wish to proceed?", vbQuestion + vbYesNo, "RI Portfolio Transfer") = vbYes Then
                    btnStart.Enabled = False
                    btnCancel.Enabled = True


                    'Task.Factory.StartNew(Sub() bgwTransferPolicies.RunWorkerAsync())
                    SbrStatus.Text = "Starting Transfer"
                    Task.Factory.StartNew(Sub() StartSAMProcessing())

                Else
                    btnStart.Enabled = True
                End If
            End If
        End If

        ' If an error's occurred, it should have been handled already

        ' Set up the interface again

        btnCancel.Enabled = True
    End Sub

    Private Sub frmRIPortfolioTransferInterface_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        If Not (m_oBusiness Is Nothing) Then

            ' Terminate the business object
            m_oBusiness.Dispose()

            ' Destroy the instance of the business object from memory.
            m_oBusiness = Nothing

        End If
    End Sub

    Private Sub frmRIPortfolioTransferInterface_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim dtProductDetails As DataTable = Nothing
        Dim dtBranchDetails As DataTable = Nothing
        Dim selectedDate As Date = txtTransferDate.Value

        m_nReturn = SetInterfaceDefaults()
        If m_nReturn <> PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        m_oBusiness = New bSIRRIPortfolioTransfer.Business
        m_nReturn = m_oBusiness.Initialise(sUsername:="", sPassword:="", iUserID:=1, iSourceID:=1, iLanguageID:=1, iCurrencyID:=26, iLogLevel:=PMELogLevel.PMLogError, sCallingAppName:=ReinsuranceTransfer.MainModule.ACApp)

        m_nReturn = m_oBusiness.GetProductAndBranchDetails(dtProductDetails:=dtProductDetails, dtBranchDetails:=dtBranchDetails)

        If m_nReturn <> PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        PopulateCombos(dtProductDetails, dtBranchDetails)

        m_nReturn = m_oBusiness.GetPortfolioTransferDate(r_dtTransferDate:=dtTransferDate)
        If m_nReturn <> PMEReturnCode.PMTrue Then
            Exit Sub
        End If
        txtTransferDate.Value = dtTransferDate
        ' Store the original transfer date for validation
        m_dtOriginalTransferDate = txtTransferDate.Value

        txtTransferDate.Enabled = True
        m_bPageLoaded = True
        Exit Sub
    End Sub

    ''' <summary>
    ''' TransferPolicy
    ''' </summary>
    ''' <param name="nLoopy"></param>
    ''' <param name="oProxy"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function TransferPolicy(ByVal nLoopy As Object, ByVal instance As InstanceElement) As Integer

        Dim oRequest As New RunPortfolioTransferCommand
        Dim oResponse As New RunPortfolioTransferCommandResponse

        TransferPolicy = PMEReturnCode.PMTrue

        oRequest.BranchCode = "HEADOFF"
        oRequest.LoginUserName = m_sUserName
        oRequest.InsuranceFileKey = NullToLong(m_oPolicies(DeferredRIField.edInsuranceFileCnt, nLoopy))
        oRequest.StartDate = NullToDate(m_oPolicies(DeferredRIField.edStartDate, nLoopy))
        oRequest.EndDate = NullToDate(m_oPolicies(DeferredRIField.edEndDate, nLoopy))
        oRequest.InceptionDate = NullToDate(m_oPolicies(DeferredRIField.edInceptionDate, nLoopy))
        oRequest.ProductKey = NullToLong(m_oPolicies(DeferredRIField.edProductId, nLoopy))
        oRequest.InsuranceFileType = Trim$(NullToString(m_oPolicies(DeferredRIField.edInsuranceFileType, nLoopy)))
        oRequest.TransferDate = m_oPolicies(DeferredRIField.edTransferDate, nLoopy)
        oRequest.SkipPostings = chkDelayPostings.Checked
        ApiClient._tokenModel = GetApiTokendetails(instance)
        oResponse = ApiClient.DeserializeJson(Of BaseClasses.RunPortfolioTransferCommandResponse)(CStr(ApiClient.Post($"/policies/runPortfolioTransfer", oRequest)))

        If oResponse.Errors IsNot Nothing Then
            nFailureCount += 1
        ElseIf oResponse.IsFailed Then
            nFailureCount += 1
        End If
    End Function
    Private Function GetApiTokendetails(ByVal instance As InstanceElement) As TokenModel
        Dim apiTokenDetails As TokenModel = New TokenModel()
        apiTokenDetails = GenerateToken.GetJwtTokenForBatchProcess(instance.ClientID, instance.TokenUrl)
        Dim address As String = instance.Address
        If address.EndsWith("/") Then
            address = address.Substring(0, address.Length - 1)
        End If
        apiTokenDetails.ApiBaseUrl = address
        apiTokenDetails.TokenUrl = instance.TokenUrl
        Return apiTokenDetails
    End Function
    ''' <summary>
    ''' StartSAMProcessing
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub StartSAMProcessing()

        Dim configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)

        Dim servers As SamServersConfigSection = CType(configuration.GetSection("SamServers"), SamServersConfigSection)

        For iRow As Integer = 0 To m_nItemsFound - 1
            mPolicyQueue.Enqueue(iRow)
        Next

        For iRow As Integer = 0 To m_nClaimItemsFound - 1
            mClaimQueue.Enqueue(iRow)
        Next

        Dim taskArray() As Task
        ReDim taskArray(servers.InstanceItems.Count - 1)
        For nCnt As Integer = 0 To taskArray.Length - 1
            Dim instanceRef As InstanceElement = servers.InstanceItems(nCnt)
            taskArray(nCnt) = Task.Factory.StartNew(Sub() StartTransfer(instanceRef))
        Next

        Task.WaitAll(taskArray)
    End Sub

    ''' <summary>
    ''' StartTransfer
    ''' </summary>
    ''' <param name="instance"></param>
    ''' <remarks></remarks>
    Private Sub StartTransfer(ByVal instance As InstanceElement)
        Dim oPoliciesDetails As Object = Nothing
        Dim nInsuranceFileCnt As Integer
        Dim sMessage As String
        Dim nNumberOfThreads As Integer = instance.ConcurrentLimit

        m_nLoopy = 0
        m_nClaimLoopy = 0

        If m_nItemsFound > 0 Then

            Dim taskArray() As Task
            If m_nItemsFound < nNumberOfThreads Then
                ReDim taskArray(m_nItemsFound - 1)
            Else
                ReDim taskArray(nNumberOfThreads - 1)
            End If
            m_bProcessingPolicy = True
            For nCnt As Integer = 0 To taskArray.Length - 1
                taskArray(nCnt) = Task.Factory.StartNew(Function() TransferPolicies(instance))
            Next

            Task.WaitAll(taskArray)
            m_bProcessingPolicy = False
        End If

        If m_nClaimItemsFound > 0 Then

            Dim taskArray() As Task
            If m_nClaimItemsFound < nNumberOfThreads Then
                ReDim taskArray(m_nClaimItemsFound - 1)
            Else
                ReDim taskArray(nNumberOfThreads - 1)
            End If

            For nCnt As Integer = 0 To taskArray.Length - 1
                taskArray(nCnt) = Task.Factory.StartNew(Function() TransferClaims(instance))
            Next

            Task.WaitAll(taskArray)
        End If

        If chkRunPostings.Checked Then
            m_oBusiness.GetPolicyListDetails(r_oPoliciesDetails:=oPoliciesDetails)

            For iCount As Integer = 0 To oPoliciesDetails.GetUpperBound(1)
                m_bProcessingStats = True
                bgwTransferPolicies.ReportProgress(iCount + m_nItemsFound + m_nClaimItemsFound)

                nInsuranceFileCnt = oPoliciesDetails(0, iCount)
                m_oBusiness.CreateAndPostStats(nInsuranceFileCnt:=nInsuranceFileCnt, sTransactionType:="PT", nPTInsuranceFileCnt:=nInsuranceFileCnt, bReversePT:=False, dtTransferDate:=dtTransferDate, r_sMessage:=sMessage)
            Next

            m_bProcessingStats = False
        End If

        SetComplete()
    End Sub

    Private Sub bgwTransferPolicies_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bgwTransferPolicies.ProgressChanged
        Dim nLoopy As Integer = e.ProgressPercentage
        Dim iCount As Integer
        If m_bProcessingPolicy = True Then
            SetPolicyNumber(Trim$(NullToString(m_oPolicies(DeferredRIField.edInsuranceRef, nLoopy))))
            SetClientCode(Trim$(NullToString(m_oPolicies(DeferredRIField.edClientCode, nLoopy))))
            SetClientName(Trim$(NullToString(m_oPolicies(DeferredRIField.edClientName, nLoopy))))
        End If

        If m_bProcessingStats = True Then
            iCount = nLoopy - m_nItemsFound - m_nClaimItemsFound
            SetPolicyNumber(Trim$(NullToString(m_oPoliciesDetails(1, iCount))))
            SetClientCode(Trim$(NullToString(m_oPoliciesDetails(2, iCount))))
            SetClientName(Trim$(NullToString(m_oPoliciesDetails(3, iCount))))
        End If

        If m_bProcessingStats = True Then
            SetStatusBar("Processing Delayed Postings..." & iCount + 1 & " of " & m_nItemsForPost)
        ElseIf nLoopy > m_nItemsFound - 1 Then
            SetStatusBar("Processing Claim..." & nLoopy - m_nItemsFound + 1 & " of " & m_nClaimItemsFound)
        Else
            SetStatusBar("Processing Policy..." & nLoopy + 1 & " of " & m_nItemsFound)
        End If
    End Sub

    ''' <summary>
    ''' SetComplete
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetComplete()
        SetStatusBar("Processing complete.")

        If nFailureCount = 0 Then
            MsgBox(Prompt:="Processing of policies is complete.", Buttons:=vbOKOnly + vbInformation, Title:="Processing Complete")
        Else
            MsgBox(Prompt:="Processing of policies is complete. " & nFailureCount & " Policies needed to be manually processed.", Buttons:=vbOKOnly + vbInformation, Title:="Processing Complete")
        End If

        SetCancel(False)
        SetStart(True)
    End Sub

    ''' <summary>
    ''' PopulateCombos
    ''' </summary>
    ''' <param name="dtProductDetails"></param>
    ''' <param name="dtBranchDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PopulateCombos(ByVal dtProductDetails As DataTable, ByVal dtBranchDetails As DataTable)

        Dim nReturn As Integer
        nReturn = PMEReturnCode.PMTrue

        cboProducts.Items.Clear()

        If dtProductDetails IsNot Nothing AndAlso dtProductDetails.Rows.Count > 0 Then
            Dim objProductList As New List(Of LookupList)
            objProductList.Add(New LookupList(0, "--(All)--"))
            For nCnt As Integer = 0 To dtProductDetails.Rows.Count - 1
                objProductList.Add(New LookupList(dtProductDetails.Rows(nCnt).Item(0), dtProductDetails.Rows(nCnt).Item(1)))
            Next nCnt
            cboProducts.DataSource = objProductList
            cboProducts.ValueMember = "id"
            cboProducts.DisplayMember = "Description"
        End If

        cboBranch.Items.Clear()

        If dtBranchDetails IsNot Nothing AndAlso dtBranchDetails.Rows.Count > 0 Then
            Dim objBranchList As New List(Of LookupList)
            objBranchList.Add(New LookupList(0, "--(All)--"))
            For nCnt As Integer = 0 To dtBranchDetails.Rows.Count - 1
                objBranchList.Add(New LookupList(dtBranchDetails.Rows(nCnt).Item(0), dtBranchDetails.Rows(nCnt).Item(1)))
            Next nCnt
            cboBranch.DataSource = objBranchList
            cboBranch.ValueMember = "id"
            cboBranch.DisplayMember = "Description"
        End If

        If (m_nReturn <> PMEReturnCode.PMTrue) Then
            nReturn = PMEReturnCode.PMFalse
        End If

        Return nReturn
    End Function

    Private Sub cboProducts_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboProducts.SelectedIndexChanged
        m_nProductId = ToSafeInteger(cboProducts.SelectedValue)
    End Sub

    ''' <summary>
    ''' cboBranch_SelectedIndexChanged
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cboBranch_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboBranch.SelectedIndexChanged
        m_nBranchId = ToSafeLong(cboBranch.SelectedValue)
    End Sub

    ''' <summary>
    ''' TransferClaim
    ''' </summary>
    ''' <param name="nLoopy"></param>
    ''' <param name="oProxy"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function TransferClaim(ByVal nLoopy As Object, ByVal instance As InstanceElement) As Integer

        Dim oRequest As New RunPortfolioTransferCommand
        Dim oResponse As New RunPortfolioTransferCommandResponse
        TransferClaim = PMEReturnCode.PMTrue

        oRequest.BranchCode = "HEADOFF"
        oRequest.LoginUserName = m_sUserName
        oRequest.ClaimKey = NullToLong(m_oClaims(ClaimRIField.edClaimId, nLoopy))
        oRequest.InsuranceFileKey = NullToLong(m_oClaims(ClaimRIField.edInduranceFileCnt, nLoopy))
        oRequest.InsuranceFileType = String.Empty
        ApiClient._tokenModel = GetApiTokendetails(instance)
        oResponse = ApiClient.DeserializeJson(Of BaseClasses.RunPortfolioTransferCommandResponse)(CStr(ApiClient.Post($"/policies/runPortfolioTransfer", oRequest)))

    End Function

    ''' <summary>
    ''' SetClientName
    ''' </summary>
    ''' <param name="sText"></param>
    ''' <remarks></remarks>
    Private Sub SetClientName(sText As String)
        If txtClientName.InvokeRequired Then
            Dim d As SetClientNameCallback = New SetClientNameCallback(AddressOf SetClientName)
            Me.Invoke(d, sText)
        Else
            Me.txtClientName.Text = sText
        End If
    End Sub

    ''' <summary>
    ''' SetClientCode
    ''' </summary>
    ''' <param name="sText"></param>
    ''' <remarks></remarks>
    Private Sub SetClientCode(sText As String)
        If txtClientCode.InvokeRequired Then
            Dim d As SetClientCodeCallback = New SetClientCodeCallback(AddressOf SetClientCode)
            Me.Invoke(d, sText)
        Else
            Me.txtClientCode.Text = sText
        End If
    End Sub

    ''' <summary>
    ''' SetPolicyNumber
    ''' </summary>
    ''' <param name="sText"></param>
    ''' <remarks></remarks>
    Private Sub SetPolicyNumber(sText As String)
        If txtPolicyNumber.InvokeRequired Then
            Dim d As SetPolicyNumberCallback = New SetPolicyNumberCallback(AddressOf SetPolicyNumber)
            Me.Invoke(d, sText)
        Else
            Me.txtPolicyNumber.Text = sText
        End If
    End Sub

    ''' <summary>
    ''' SetStatusBar
    ''' </summary>
    ''' <param name="sText"></param>
    ''' <remarks></remarks>
    Private Sub SetStatusBar(sText As String)
        If sbrStatusStrip.InvokeRequired Then
            Dim d As SetStatusBarCallback = New SetStatusBarCallback(AddressOf SetStatusBar)
            Me.Invoke(d, sText)
        Else
            SbrStatus.Text = sText
            If spbStatus.Value < spbStatus.Maximum Then
                spbStatus.Value += 1
            End If
        End If
    End Sub

#End Region

End Class