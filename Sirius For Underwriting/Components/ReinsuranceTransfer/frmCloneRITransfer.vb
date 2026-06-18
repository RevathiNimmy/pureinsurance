Option Strict Off
Option Explicit On
Imports System.Collections.Concurrent
Imports System.Linq
Imports System.Threading
Imports System.Threading.Tasks
Imports SharedFiles
Imports SSP.PureInsuranceRestAPIHandler
Imports SSP.PureInsuranceRestAPIHandler.BaseClasses
Public Class frmCloneRITransfer
#Region "Private Variables"
    ' Object parameter members.
    Private m_nStatus As Integer

    ' ClonedRIPolicyField
    Private Enum CloneRIBatchProcess
        edInsuranceFileCnt = 0
        edInsuranceRef = 1
        edInsuranceFolderCnt = 2
        edInsuranceFileType = 3
        edClientCode = 4
        edClientName = 5
        edCoverStartDate = 6
        edInsuranceFileTypeCode = 7
    End Enum

    ' ClonedRIClaimsField
    Private Enum CloneClaimRIBatchProcess
        edCLMInsuranceRef = 0
        edCLMShortCode = 1
        edCLMClientName = 2
        edCLMClaimClonedRIUsageID = 3
        edCLMOldInsuranceFileCnt = 4
        edCLMNewInsuranceFileCnt = 5
        edCLMOldRiskCnt = 6
        edCLMNewRiskCnt = 7
        edCLMStatus = 8
        edCLMClaimId = 9
    End Enum

    Dim dtStartTime As Date
    Dim dtEndTime As Date
    Dim sTotalTime As String
    Dim iCount As Integer

    Private m_oClonedRIPoliciesData(,) As Object
    Private m_oClonedRIClaimsData(,) As Object

    Private m_nProductId As Integer
    Private m_nBranchId As Integer
    ' Stores the return value for the a
    ' function call.
    Private m_nReturn As Integer
    Private m_nItemsFound As Integer
    Private m_nClaimItemsFound As Integer

    Private m_oBusiness As Object

    Private m_sCurrentMode As String
    Private m_nTotalItems As Integer
    Private ReadOnly m_busy As ManualResetEvent()

    Private Delegate Sub SetClientNameCallback(text As String)

    Private Delegate Sub SetPolicyNumberCallback(text As String)

    Private Delegate Sub SetClientCodeCallback(text As String)

    Private Delegate Sub SetStatusBarCallback(text As String)

    Private Delegate Sub SetStartCallback(status As Boolean)

    Private Delegate Sub SetCancelCallback(status As Boolean)


    Private ReadOnly mPolicyQueue As ConcurrentQueue(Of Policies)
    Private ReadOnly mClaimQueue As ConcurrentQueue(Of Integer)
    Private m_nFailureCount As Integer
    Dim m_nPolicyLoopy As Integer
    Dim m_nClaimLoopy As Integer
    Dim m_nCrashedCount As Integer
    Dim m_nClaimFailureCount As Integer

#End Region
#Region "public Property"
    Public Property Status() As Integer
        Get
            Return m_nStatus
        End Get
        Set(ByVal nStatus As Integer)
            m_nStatus = nStatus
        End Set
    End Property
#End Region

#Region "Private Method"
    ''' <summary>
    ''' frmCloneRITransfer_Load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub frmCloneRITransfer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dtProductDetails As DataTable
        Dim dtBranchDetails As DataTable
        m_nReturn = SetInterfaceDefaults()
        If m_nReturn <> PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        m_oBusiness = New bSIRCloneRIBatchProcess.Business
        m_nReturn = m_oBusiness.Initialise(sUsername:="", sPassword:="", iUserID:=1, iSourceID:=1, iLanguageID:=1, iCurrencyID:=26, iLogLevel:=PMELogLevel.PMLogError, sCallingAppName:=ReinsuranceTransfer.MainModule.ACApp)

        m_nReturn = m_oBusiness.GetProductAndBranchDetails(r_dtProductDetails:=dtProductDetails, r_dtBranchDetails:=dtBranchDetails)

        PopulateCombos(dtProductDetails, dtBranchDetails)

        If m_nReturn <> PMEReturnCode.PMTrue Then
            Exit Sub
        End If
    End Sub

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
    End Function

    ''' <summary>
    ''' GetBusiness
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetBusiness()
        Dim nProductID As Integer
        Dim nBranchID As Integer
        Dim nResult As Integer

        nResult = PMEReturnCode.PMTrue
        m_oClonedRIPoliciesData = Nothing
        m_oClonedRIClaimsData = Nothing
        ' GetDeferredRIPolicies
        ' For Policy
        nProductID = m_nProductId
        nBranchID = m_nBranchId
        nResult = m_oBusiness.GetClonedRIPolicies(v_nProductId:=nProductID, v_nBranchID:=nBranchID, r_vClonedRIPoliciesArray:=m_oClonedRIPoliciesData)
        If (nResult <> PMEReturnCode.PMTrue) Then
            Return nResult
        End If

        ' update the module level variable that holds the number of risks we're dealing with
        If IsArray(m_oClonedRIPoliciesData) Then
            m_nItemsFound = UBound(m_oClonedRIPoliciesData, 2) + 1
        Else
            m_nItemsFound = 0
        End If

        ' Do for claim as well
        nResult = m_oBusiness.GetClonedRIClaims(v_nProductID:=nProductID, v_nBranchID:=nBranchID, r_vClonedRIClaimsArray:=m_oClonedRIClaimsData)
        If (nResult <> PMEReturnCode.PMTrue) Then
            Return nResult
        End If

        If IsArray(m_oClonedRIClaimsData) Then
            m_nClaimItemsFound = UBound(m_oClonedRIClaimsData, 2) + 1
        Else
            m_nClaimItemsFound = 0
        End If

        If chkProcessPolicies.Checked AndAlso chkProcessClaims.Checked Then
            m_nTotalItems = m_nItemsFound + m_nClaimItemsFound
        ElseIf chkProcessPolicies.Checked Then
            m_nTotalItems = m_nItemsFound
        ElseIf chkProcessClaims.Checked Then
            m_nTotalItems = m_nClaimItemsFound
        End If

        spbStatus.Value = 0
        spbStatus.Minimum = 0
        spbStatus.Maximum = m_nTotalItems
        Return nResult

    End Function
    ''' <summary>
    ''' cmdStart_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmdStart_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStart.Click

        If chkProcessPolicies.Checked = False And chkProcessClaims.Checked = False Then
            MsgBox("Please select Policies to transfer or Claim to transfer", vbInformation + vbOKOnly, "RI CloneTransfer")
            Exit Sub
        End If

        GetBusiness()

        If chkProcessPolicies.Checked AndAlso m_nTotalItems < 1 AndAlso Not chkProcessClaims.Checked Then
            MsgBox("No Policies Found", vbInformation + vbOKOnly, "RI clone Transfer")
        ElseIf chkProcessClaims.Checked AndAlso m_nClaimItemsFound < 1 AndAlso Not chkProcessPolicies.Checked Then
            MsgBox("No Claims Found", vbInformation + vbOKOnly, "RI clone Transfer")
        Else
            ' Policies found, ask user confirmation before processing
            Dim sConfirmaitonMessage As String
            If chkProcessPolicies.Checked AndAlso chkProcessClaims.Checked Then
                sConfirmaitonMessage = CStr(m_nItemsFound) & " " & "Policies Found and " & CStr(m_nClaimItemsFound) & " Claims found. " & "Do you wish to proceed?"
            ElseIf chkProcessPolicies.Checked Then
                sConfirmaitonMessage = CStr(m_nItemsFound) & " " & "Policies Found. " & "Do you wish to proceed?"
            ElseIf chkProcessClaims.Checked Then
                sConfirmaitonMessage = CStr(m_nClaimItemsFound) & " Claims found. " & "Do you wish to proceed?"
            End If

            If MsgBox(sConfirmaitonMessage, vbQuestion + vbYesNo, "RI Clone Transfer") = vbYes Then
                SetStart(False)
                SetCancel(True)
                dtStartTime = Now
                SbrStatus.Text = "Starting Transfer..."
                m_nPolicyLoopy = 0
                m_nClaimLoopy = 0
                Task.Factory.StartNew(Sub() StartTransfer())
            Else
                SetStart(True)
            End If
        End If
    End Sub

    ''' <summary>
    ''' StartTransfer
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub StartTransfer()
        Dim nInsuranceFolderCnt As Integer
        Dim oPolicy As Policies = Nothing
        Dim configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        Dim servers As SamServersConfigSection = CType(configuration.GetSection("SamServers"), SamServersConfigSection)

        m_nPolicyLoopy = 0
        m_nClaimLoopy = 0

        If chkProcessPolicies.Checked And m_nItemsFound > 0 Then
            m_sCurrentMode = "Policy"
            For iRow As Integer = 0 To m_nItemsFound - 1
                If nInsuranceFolderCnt = NullToLong(m_oClonedRIPoliciesData(CloneRIBatchProcess.edInsuranceFolderCnt, iRow)) Then
                    nInsuranceFolderCnt = NullToLong(m_oClonedRIPoliciesData(CloneRIBatchProcess.edInsuranceFolderCnt, iRow))
                    If iRow = m_nItemsFound - 1 Then
                        oPolicy.EndRow = iRow
                        mPolicyQueue.Enqueue(oPolicy)
                    End If
                Else
                    If nInsuranceFolderCnt <> 0 Then
                        oPolicy.EndRow = iRow - 1
                        mPolicyQueue.Enqueue(oPolicy)
                    End If
                    nInsuranceFolderCnt = NullToLong(m_oClonedRIPoliciesData(CloneRIBatchProcess.edInsuranceFolderCnt, iRow))
                    oPolicy = New Policies()
                    oPolicy.StartRow = iRow
                    If iRow = m_nItemsFound - 1 Then
                        oPolicy.EndRow = iRow
                        mPolicyQueue.Enqueue(oPolicy)
                    End If
                End If
            Next

            Dim taskArray() As Task
            ReDim taskArray(servers.InstanceItems.Count - 1)
            For nCnt As Integer = 0 To taskArray.Length - 1
                Dim instanceRef As InstanceElement = servers.InstanceItems(nCnt)
                taskArray(nCnt) = Task.Factory.StartNew(Sub() StartProcessClonedRIPolicies(instanceRef))
            Next

            Task.WaitAll(taskArray)
        End If

        If bgwTransferPolicies.CancellationPending Then
            Exit Sub
        End If

        If chkProcessClaims.Checked And m_nClaimItemsFound > 0 Then
            m_sCurrentMode = "Claim"
            For iRow As Integer = 0 To m_nClaimItemsFound - 1
                mClaimQueue.Enqueue(iRow)
            Next

            Dim taskArray() As Task
            ReDim taskArray(servers.InstanceItems.Count - 1)
            For nCnt As Integer = 0 To taskArray.Length - 1
                Dim instanceRef As InstanceElement = servers.InstanceItems(nCnt)
                taskArray(nCnt) = Task.Factory.StartNew(Sub() StartProcessClonedRIClaims(instanceRef))
            Next

            Task.WaitAll(taskArray)

        End If

        SetComplete()
    End Sub


    Private Sub bgwTransferPolicies_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bgwTransferPolicies.ProgressChanged

        Dim nLoopy As Integer = e.ProgressPercentage
        Try
            If m_sCurrentMode = "Policy" Then
                SetStatusBar("Processing Policy..." & nLoopy + 1 & " of " & m_nItemsFound)
                SetPolicyNumber(Trim$(NullToString(m_oClonedRIPoliciesData(CloneRIBatchProcess.edInsuranceRef, nLoopy))))
                SetClientCode(Trim$(NullToString(m_oClonedRIPoliciesData(CloneRIBatchProcess.edClientCode, nLoopy))))
                SetClientName(Trim$(NullToString(m_oClonedRIPoliciesData(CloneRIBatchProcess.edClientName, nLoopy))))
            ElseIf m_sCurrentMode = "Claim" Then
                SetStatusBar("Processing Claim..." & nLoopy + 1 & " of " & m_nClaimItemsFound)
                SetPolicyNumber(Trim$(NullToString(m_oClonedRIClaimsData(CloneClaimRIBatchProcess.edCLMInsuranceRef, nLoopy))))
                SetClientCode(Trim$(NullToString(m_oClonedRIClaimsData(CloneClaimRIBatchProcess.edCLMShortCode, nLoopy))))
                SetClientName(Trim$(NullToString(m_oClonedRIClaimsData(CloneClaimRIBatchProcess.edCLMClientName, nLoopy))))
            End If
        Finally

        End Try
    End Sub

    ''' <summary>
    ''' SetComplete
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetComplete()
        SetStatusBar("Processing complete.")
        dtEndTime = Now
        'sTotalTime = Int(DateDiff(DateInterval.Second, dtStartTime, dtEndTime) / 60) & " minutes & " & DateDiff(DateInterval.Second, dtStartTime, dtEndTime) Mod 60 & " Seconds.
        ' E007
        If chkProcessPolicies.Checked Then
            If m_nFailureCount = 0 AndAlso m_nCrashedCount = 0 Then
                MsgBox(Prompt:="Processing of policies is complete.", Buttons:=vbOKOnly + vbInformation, Title:="Processing Complete")
                ' E007
            Else
                MsgBox(Prompt:="Processing of policies is complete. " & m_nFailureCount & " Policies needed to be manually processed." & m_nCrashedCount & " Policies needs to be reprocessed.", Buttons:=vbOKOnly + vbInformation, Title:="Processing Complete")
            End If
        End If
        If chkProcessClaims.Checked Then
            If m_nClaimFailureCount = 0 Then
                MsgBox(Prompt:="Processing of claims is complete.", Buttons:=vbOKOnly + vbInformation, Title:="Processing Complete")
                ' E007
            Else
                MsgBox(Prompt:="Processing of Claims is complete. " & m_nClaimFailureCount & " Claims needs to be reprocessed.", Buttons:=vbOKOnly + vbInformation, Title:="Processing Complete")
            End If
        End If


        SetCancel(False)
        SetStart(True)
    End Sub
    ''' <summary>
    ''' PopulateCombos
    ''' </summary>
    ''' <param name="v_dtProductDetails"></param>
    ''' <param name="v_dtBranchDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PopulateCombos(ByVal v_dtProductDetails As DataTable, ByVal v_dtBranchDetails As DataTable)

        Dim nResult As Integer

        nResult = PMEReturnCode.PMTrue

        cboProducts.Items.Clear()

        If v_dtProductDetails IsNot Nothing And v_dtProductDetails.Rows.Count > 0 Then
            Dim objProductList As New List(Of LookupList)
            objProductList.Add(New LookupList(0, "--(All)--"))
            For nCnt As Integer = 0 To v_dtProductDetails.Rows.Count - 1
                objProductList.Add(New LookupList(v_dtProductDetails.Rows(nCnt).Item(0), v_dtProductDetails.Rows(nCnt).Item(1)))
            Next nCnt
            cboProducts.DataSource = objProductList
            cboProducts.ValueMember = "id"
            cboProducts.DisplayMember = "Description"
        End If

        cboBranch.Items.Clear()

        If v_dtBranchDetails IsNot Nothing AndAlso v_dtBranchDetails.Rows.Count > 0 Then
            Dim objBranchList As New List(Of LookupList)
            objBranchList.Add(New LookupList(0, "--(All)--"))
            For nCnt As Integer = 0 To v_dtBranchDetails.Rows.Count - 1
                objBranchList.Add(New LookupList(v_dtBranchDetails.Rows(nCnt).Item(0), v_dtBranchDetails.Rows(nCnt).Item(1)))
            Next nCnt
            cboBranch.DataSource = objBranchList
            cboBranch.ValueMember = "id"
            cboBranch.DisplayMember = "Description"
        End If

        If (m_nReturn <> PMEReturnCode.PMTrue) Then
            nResult = PMEReturnCode.PMFalse
        End If

        Return nResult

    End Function

    Private Sub cboProducts_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboProducts.SelectedIndexChanged
        m_nProductId = ToSafeInteger(cboProducts.SelectedValue)
    End Sub

    Private Sub cboBranch_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboBranch.SelectedIndexChanged
        m_nBranchId = ToSafeLong(cboBranch.SelectedValue)
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        m_nStatus = PMEReturnCode.PMCancel

        bgwTransferPolicies.CancelAsync()

        btnStart.Enabled = True
    End Sub

    ''' <summary>
    ''' SetClientName
    ''' </summary>
    ''' <param name="Text"></param>
    ''' <remarks></remarks>
    Private Sub SetClientName(Text As String)
        If txtClientName.InvokeRequired Then
            Dim d As SetClientNameCallback = New SetClientNameCallback(AddressOf SetClientName)
            Me.Invoke(d, Text)
        Else
            Me.txtClientName.Text = Text
        End If
    End Sub
    ''' <summary>
    ''' SetClientCode
    ''' </summary>
    ''' <param name="Text"></param>
    ''' <remarks></remarks>
    Private Sub SetClientCode(Text As String)
        If txtClientCode.InvokeRequired Then
            Dim d As SetClientCodeCallback = New SetClientCodeCallback(AddressOf SetClientCode)
            Me.Invoke(d, Text)
        Else
            Me.txtClientCode.Text = Text
        End If
    End Sub
    ''' <summary>
    ''' SetPolicyNumber
    ''' </summary>
    ''' <param name="Text"></param>
    ''' <remarks></remarks>
    Private Sub SetPolicyNumber(Text As String)
        If txtPolicyNumber.InvokeRequired Then
            Dim d As SetPolicyNumberCallback = New SetPolicyNumberCallback(AddressOf SetPolicyNumber)
            Me.Invoke(d, Text)
        Else
            Me.txtPolicyNumber.Text = Text
        End If
    End Sub
    ''' <summary>
    ''' SetStatusBar
    ''' </summary>
    ''' <param name="text"></param>
    ''' <remarks></remarks>
    Private Sub SetStatusBar(text As String)
        If sbrStatusStrip.InvokeRequired Then
            Dim d As SetStatusBarCallback = New SetStatusBarCallback(AddressOf SetStatusBar)
            Me.Invoke(d, text)
        Else
            SbrStatus.Text = text
            If spbStatus.Value < spbStatus.Maximum Then
                spbStatus.Value += 1
            End If
        End If
    End Sub
#End Region

#Region "Public method"
    ''' <summary>
    ''' StartTransfer
    ''' </summary>
    ''' <param name="instance"></param>
    ''' <remarks></remarks>
    Public Sub StartTransfer(ByVal instance As InstanceElement)
        Dim taskArray() As Task

        If instance.ConcurrentLimit < m_nItemsFound Then
            ReDim taskArray(instance.ConcurrentLimit - 1)
        Else
            ReDim taskArray(m_nItemsFound - 1)
        End If

        For nCnt As Integer = 0 To taskArray.Length - 1
            Dim ThreadID As Integer = nCnt
            taskArray(nCnt) = Task.Factory.StartNew(Function() ProcessClonedRIPolicies(instance, ThreadID))
        Next
        Task.WaitAll(taskArray)
    End Sub
    ''' <summary>
    ''' StartProcessClonedRIPolicies
    ''' </summary>
    ''' <param name="instance"></param>
    ''' <remarks></remarks>
    Public Sub StartProcessClonedRIPolicies(ByVal instance As InstanceElement)
        Dim taskArray() As Task

        If instance.ConcurrentLimit < m_nItemsFound Then
            ReDim taskArray(instance.ConcurrentLimit - 1)
        Else
            ReDim taskArray(m_nItemsFound - 1)
        End If

        For nCnt As Integer = 0 To taskArray.Length - 1
            Dim ThreadID As Integer = nCnt
            taskArray(nCnt) = Task.Factory.StartNew(Function() ProcessClonedRIPolicies(instance, ThreadID))
        Next
        Task.WaitAll(taskArray)
    End Sub
    ''' <summary>
    ''' StartProcessClonedRIClaims
    ''' </summary>
    ''' <param name="instance"></param>
    ''' <remarks></remarks>
    Public Sub StartProcessClonedRIClaims(ByVal instance As InstanceElement)
        Dim taskArray() As Task

        If instance.ConcurrentLimit < m_nClaimItemsFound Then
            ReDim taskArray(instance.ConcurrentLimit - 1)
        Else
            ReDim taskArray(m_nClaimItemsFound - 1)
        End If

        For nCnt As Integer = 0 To taskArray.Length - 1
            Dim ThreadID As Integer = nCnt
            taskArray(nCnt) = Task.Factory.StartNew(Function() ProcessClonedRIClaims(instance, ThreadID))
        Next
        Task.WaitAll(taskArray)
    End Sub
    ''' <summary>
    ''' ProcessClonedRIPolicies
    ''' </summary>
    ''' <param name="instance"></param>
    ''' <param name="ThreadID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessClonedRIPolicies(ByVal instance As InstanceElement, ByVal ThreadID As Integer) As Integer

        Dim nLoopy As Integer
        Dim oPolicy As Policies
        m_nFailureCount = 0
        m_nCrashedCount = 0
        Try

            While mPolicyQueue.TryDequeue(oPolicy)
                Dim bIsFailed As Boolean = False

                For nLoopy = oPolicy.StartRow To oPolicy.EndRow
                    Dim request As New RunCloneReworkCommand
                    Dim response As New RunCloneReworkCommandResponse

                    ' update the interface
                    If bgwTransferPolicies.CancellationPending Then
                        Exit Function
                    End If

                    bgwTransferPolicies.ReportProgress(m_nPolicyLoopy)

                    request.BranchCode = "HEADOFF"
                    request.LoginUserName = instance.UserName
                    request.InsuranceFileKey = NullToLong(m_oClonedRIPoliciesData(CloneRIBatchProcess.edInsuranceFileCnt, nLoopy))
                    request.InsuranceFileType = Trim$(NullToString(m_oClonedRIPoliciesData(CloneRIBatchProcess.edInsuranceFileTypeCode, nLoopy)))
                    request.IsFailed = bIsFailed
                    Try

                        ApiClient._tokenModel = GetApiTokendetails(instance)
                        response = ApiClient.DeserializeJson(Of RunCloneReworkCommandResponse)(CStr(ApiClient.Post($"/policies/runCloneRework", request)))
                        If response.Errors IsNot Nothing Then
                            m_nFailureCount += 1
                            bIsFailed = True
                        ElseIf response.IsFailed Then
                            m_nFailureCount += 1
                            bIsFailed = True
                        End If
                    Catch ex As Exception
                        m_nCrashedCount += 1
                        bIsFailed = True
                    Finally
                        m_nPolicyLoopy += 1
                    End Try

                Next nLoopy

            End While
        Finally

        End Try
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
    ''' ProcessClonedRIClaims
    ''' </summary>
    ''' <param name="instance"></param>
    ''' <param name="ThreadID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessClonedRIClaims(ByVal instance As InstanceElement, ThreadID As Integer) As Integer

        Dim nLoopy As Integer
        m_nClaimFailureCount = 0
        While mClaimQueue.TryDequeue(nLoopy)

            Dim request As New RunCloneReworkCommand
            Dim response As New RunCloneReworkCommandResponse

            If bgwTransferPolicies.CancellationPending Then
                Exit Function
            End If

            bgwTransferPolicies.ReportProgress(m_nClaimLoopy)
            request.BranchCode = "HeadOff"
            request.LoginUserName = instance.UserName
            request.InsuranceFileKey = NullToLong(m_oClonedRIClaimsData(CloneClaimRIBatchProcess.edCLMNewInsuranceFileCnt, nLoopy))
            request.RiskKey = NullToLong(m_oClonedRIClaimsData(CloneClaimRIBatchProcess.edCLMNewRiskCnt, nLoopy))
            request.ClaimKey = NullToLong(m_oClonedRIClaimsData(CloneClaimRIBatchProcess.edCLMClaimId, nLoopy))
            request.InsuranceFileType = ""
            Try
                ApiClient._tokenModel = GetApiTokendetails(instance)
                response = ApiClient.DeserializeJson(Of RunCloneReworkCommandResponse)(CStr(ApiClient.Post($"/policies/runCloneRework", request)))
                If response.Errors IsNot Nothing Then
                    m_nClaimFailureCount += 1
                ElseIf response.IsFailed Then
                    m_nClaimFailureCount += 1
                End If
            Catch ex As Exception
                m_nClaimFailureCount += 1
            Finally
                m_nClaimLoopy += 1
            End Try

        End While
    End Function
    ''' <summary>
    ''' SetCancel
    ''' </summary>
    ''' <param name="status"></param>
    ''' <remarks></remarks>
    Public Sub SetCancel(status As Boolean)
        If btnCancel.InvokeRequired Then
            Dim d As SetCancelCallback = New SetCancelCallback(AddressOf SetCancel)
            Me.Invoke(d, status)
        Else
            btnCancel.Enabled = status
        End If
    End Sub
    ''' <summary>
    ''' SetStart
    ''' </summary>
    ''' <param name="status"></param>
    ''' <remarks></remarks>
    Public Sub SetStart(status As Boolean)
        If btnStart.InvokeRequired Then
            Dim d As SetStartCallback = New SetStartCallback(AddressOf SetStart)
            Me.Invoke(d, status)
        Else
            btnStart.Enabled = status
        End If
    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        mPolicyQueue = New ConcurrentQueue(Of Policies)()
        mClaimQueue = New ConcurrentQueue(Of Integer)()
        ' Add any initialization after the InitializeComponent() call.
    End Sub
#End Region

End Class

Public Class Policies
    Public Property StartRow As Integer
    Public Property EndRow As Integer
End Class
