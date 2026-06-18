Option Strict On

Imports System.Collections.ObjectModel
Imports System.Math
Imports System.Xml

Friend Class Instalment_Import : Inherits ImportBase

#Region "Constructors"
    Public Sub New(ByVal oXML As XmlDocument)
        MyBase.New(oXML)

        ' get values for all system options that may apply to this process
        GetApplicableSystemOptions()

        ' get default task details
        GetDefaultTaskDetails(Me.DefaultTaskId, Me.DefaultTaskGroupId, Me.DefaultUserGroupId, Me.DefaultUserId)

    End Sub
#End Region

#Region "Constants"

    Private NotInheritable Class CreditControlBusinessType

#Region "Constructors"
        Private Sub New()
            ' This class cannot be instantiated.
        End Sub
#End Region

        Public Const Live As String = "INS"
        Public Const Cancelled As String = "INSC"
        Public Const OnHold As String = "INSH"

    End Class

    Private NotInheritable Class InstalmentPlanStatus

#Region "Constructors"
        Private Sub New()
            ' This class cannot be instantiated.
        End Sub
#End Region

        Public Const Saved As String = "010"
        Public Const Updated As String = "011"
        Public Const QuotePrinted As String = "012"
        Public Const Live As String = "040"
        Public Const OnHold As String = "140"
        Public Const Completed As String = "900"
        Public Const Superseded As String = "990"
        Public Const Cancelled As String = "999"

    End Class

    Private NotInheritable Class ActionType
#Region "Constructors"
        Private Sub New()
            ' This class cannot be instantiated.
        End Sub
#End Region

        Public Const Amendment As String = "A"
        Public Const Cancellation As String = "C"
        Public Const Reinstatement As String = "REINSTATEMENT"
        Public Const Rejection As String = "R"

    End Class

    Public Enum ProcessMode
        Cancellation = 1
        Rejection = 2
    End Enum

    Public Enum InstalmentProcess
        Amendment
        Cancellation
        Rejection
    End Enum

    Public Enum InsuranceFileStatus
        Live = 0
        Cancelled = 1
        Lapsed = 2
        UnderRenewal = 3
        Replaced = 4
    End Enum

    Public Enum SystemOptions
        CreditControlEnabled = 5001
        ADDACSUserGroup = 5041
        ARUDDSUserGroup = 5042
        AutoArchiveEnabled = 5008
    End Enum

#End Region

#Region "Fields"

    Private m_nNoOfTotalRecords As Integer
    Private m_nNoOfRejections As Integer
    Private _insuredShortName As String
    Public Property InsuredShortName() As String
        Get
            Return _insuredShortName
        End Get
        Set(ByVal value As String)
            _insuredShortName = value
        End Set
    End Property

    Private _searchString As String
    Public Property SearchString() As String
        Get
            Return _searchString
        End Get
        Set(ByVal value As String)
            _searchString = value
        End Set
    End Property

    Private _listOfPFInstalmentTransactionCodes As List(Of String)
    Public Property ListOfPFInstalmentTransactionCodes() As List(Of String)
        Get
            Return _listOfPFInstalmentTransactionCodes
        End Get
        Set(ByVal value As List(Of String))
            _listOfPFInstalmentTransactionCodes = value
        End Set
    End Property

    Private _addacsUserGroup As Integer
    Public Property AddacsUserGroup() As Integer
        Get
            Return _addacsUserGroup
        End Get
        Set(ByVal value As Integer)
            _addacsUserGroup = value
        End Set
    End Property

    Private _aruddsUserGroup As Integer
    Public Property AruddsUserGroup() As Integer
        Get
            Return _aruddsUserGroup
        End Get
        Set(ByVal value As Integer)
            _aruddsUserGroup = value
        End Set
    End Property

    Private _actionTypeCode As String
    Public Property ActionTypeCode() As String
        Get
            Return _actionTypeCode
        End Get
        Set(ByVal value As String)
            _actionTypeCode = value
        End Set
    End Property

    Private _taskCustomer As String
    Public Property TaskCustomer() As String
        Get
            Return _taskCustomer
        End Get
        Set(ByVal value As String)
            _taskCustomer = value
        End Set
    End Property

    Private _taskDescriptionPrefix As String
    Public Property TaskDescriptionPrefix() As String
        Get
            Return _taskDescriptionPrefix
        End Get
        Set(ByVal value As String)
            _taskDescriptionPrefix = value
        End Set
    End Property

    Private _insuranceFileCnt As Integer
    Public Property InsuranceFileCnt() As Integer
        Get
            Return _insuranceFileCnt
        End Get
        Set(ByVal value As Integer)
            _insuranceFileCnt = value
        End Set
    End Property

    Private _rejectionCodeForCancellationOnLivePaidPolicy As String
    Public Property RejectionCodeForCancellationOnLivePaidPolicy() As String
        Get
            Return _rejectionCodeForCancellationOnLivePaidPolicy
        End Get
        Set(ByVal value As String)
            _rejectionCodeForCancellationOnLivePaidPolicy = value
        End Set
    End Property

    Private _rejectionCodeForCancellationOnLivePaidUnPaidPolicy As String
    Public Property RejectionCodeForCancellationOnLivePaidUnPaidPolicy() As String
        Get
            Return _rejectionCodeForCancellationOnLivePaidUnPaidPolicy
        End Get
        Set(ByVal value As String)
            _rejectionCodeForCancellationOnLivePaidUnPaidPolicy = value
        End Set
    End Property

    Private _rejectionCodeForCancellationOnCancellationLapsedReplacedPolicy As String
    Public Property RejectionCodeForCancellationOnCancellationLapsedReplacedPolicy() As String
        Get
            Return _rejectionCodeForCancellationOnCancellationLapsedReplacedPolicy
        End Get
        Set(ByVal value As String)
            _rejectionCodeForCancellationOnCancellationLapsedReplacedPolicy = value
        End Set
    End Property

    Private _creditControlEnabled As Boolean
    Public Property CreditControlEnabled() As Boolean
        Get
            Return _creditControlEnabled
        End Get
        Set(ByVal value As Boolean)
            _creditControlEnabled = value
        End Set
    End Property

    Private _planReference As String = String.Empty
    Public Property PlanReference() As String
        Get
            Return _planReference
        End Get
        Set(ByVal value As String)
            _planReference = value
        End Set
    End Property

    Private _groupId As Integer
    Public Property GroupId() As Integer
        Get
            Return _groupId
        End Get
        Set(ByVal value As Integer)
            _groupId = value
        End Set
    End Property

    Private _taskId As Integer
    Public Property DefaultTaskId() As Integer
        Get
            Return _taskId
        End Get
        Set(ByVal value As Integer)
            _taskId = value
        End Set
    End Property

    Private _taskGroupId As Integer
    Public Property DefaultTaskGroupId() As Integer
        Get
            Return _taskGroupId
        End Get
        Set(ByVal value As Integer)
            _taskGroupId = value
        End Set
    End Property

    Private _userGroupId As Integer
    Public Property DefaultUserGroupId() As Integer
        Get
            Return _userGroupId
        End Get
        Set(ByVal value As Integer)
            _userGroupId = value
        End Set
    End Property

    Private _userId As Integer
    Public Property DefaultUserId() As Integer
        Get
            Return _userId
        End Get
        Set(ByVal value As Integer)
            _userId = value
        End Set
    End Property

    Private _planId As Integer
    Public Property PlanId() As Integer
        Get
            Return _planId
        End Get
        Set(ByVal value As Integer)
            _planId = value
        End Set
    End Property

    Private _planVersion As Integer
    Public Property PlanVersion() As Integer
        Get
            Return _planVersion
        End Get
        Set(ByVal value As Integer)
            _planVersion = value
        End Set
    End Property

    Private _planStatus As String
    Public Property PlanStatus() As String
        Get
            Return _planStatus
        End Get
        Set(ByVal value As String)
            _planStatus = value
        End Set
    End Property

    Private _ddCancelled As Byte
    Public Property DdCancelled() As Byte
        Get
            Return _ddCancelled
        End Get
        Set(ByVal value As Byte)
            _ddCancelled = value
        End Set
    End Property

    Private _ccCancelled As Byte
    Public Property CcCancelled() As Byte
        Get
            Return _ccCancelled
        End Get
        Set(ByVal value As Byte)
            _ccCancelled = value
        End Set
    End Property

    Private _planBankAccountName As String
    Public Property PlanBankAccountName() As String
        Get
            Return _planBankAccountName
        End Get
        Set(ByVal value As String)
            _planBankAccountName = value
        End Set
    End Property

    Private _coverStartDate As Date
    Public Property CoverStartDate() As Date
        Get
            Return _coverStartDate
        End Get
        Set(ByVal value As Date)
            _coverStartDate = value
        End Set
    End Property

    Private _coverExpiryDate As Date
    Public Property CoverExpiryDate() As Date
        Get
            Return _coverExpiryDate
        End Get
        Set(ByVal value As Date)
            _coverExpiryDate = value
        End Set
    End Property

    Private _insuranceFileStatusId As Integer
    Public Property InsuranceFileStatusId() As Integer
        Get
            Return _insuranceFileStatusId
        End Get
        Set(ByVal value As Integer)
            _insuranceFileStatusId = value
        End Set
    End Property

    Public Overrides ReadOnly Property BatchCode() As String
        Get
            Return "INSI"
        End Get
    End Property

    Public Overrides ReadOnly Property InterfaceName() As String
        Get
            Return "Instalment_Import"
        End Get
    End Property

    ''' <summary>
    ''' Specifies the number of records in batch for this Class
    ''' </summary>
    ''' <returns></returns>
    Public Overrides Property NoOfTotalRecords() As Integer
        Get
            Return m_nNoOfTotalRecords
        End Get
        Set(ByVal value As Integer)
            m_nNoOfTotalRecords = value
        End Set
    End Property

    ''' <summary>
    ''' Specifies the no of rejected records in the batch for this Class
    ''' </summary>
    ''' <returns></returns>
    Public Overrides Property NoOfRejections() As Integer
        Get
            Return m_nNoOfRejections
        End Get
        Set(ByVal value As Integer)
            m_nNoOfRejections = value
        End Set
    End Property



#End Region

#Region "Methods"

    Protected Overrides Sub PostImportProcessing()

        Dim runCreditControl As Boolean

        runCreditControl = Util.ToSafeBoolean(GetHeaderAttribute("run_credit_control"), False)

        If runCreditControl Then

            RunCreditControlForAllBranches()

        End If

    End Sub

    Protected Overrides Sub ProcessElement()

        Me.ActionTypeCode = CStr(GetAttribute("action_type_code"))
        Me.TaskDescriptionPrefix = CStr(GetAttribute("generated_task_description_prefix"))
        Me.TaskCustomer = CStr(GetAttribute("generated_task_customer"))
        NoOfTotalRecords += 1
        ' validate that the plan reference is valid
        Dim returnCode As PMEReturnCode
        returnCode = LocateLivePlan()

        If returnCode = PMEReturnCode.PMTrue Then

            Select Case Me.ActionTypeCode
                Case ActionType.Amendment
                    ProcessPlanAmendment()
                Case ActionType.Reinstatement
                    ProcessReinstatement()
                Case ActionType.Cancellation
                    ProcessPlanCancellation()
                Case ActionType.Rejection
                    ProcessInstalmentRejection()
                Case Else
                    Throw New Exception("Unable to import instalment record - action_type_code is not recognised.")
            End Select

        End If

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub RunCreditControlForAllBranches()

        Const ACSpokeInterfaceCode As String = "CREDITCONTROL"
        Const ACSpokeStatusCode As String = "A"
        Const ACSpokeMessage As String = "A"
        Const ACSpokeHeaderXML As String = "<XML>"
        Const ACSpokeDetailData As String = ""
        Const ACSpokeBatch As String = ""

        Const kbHDBranch As Byte = 9
        Const kbHDAsOfDate As Byte = 10
        Const kbHDSpoolDoc As Byte = 11
        Const kbHDArchiveDoc As Byte = 12

        Dim vHeaderData(1) As Object
        Dim vHeaderDetail(12) As Object

        Dim creditControlManager As bACTFinanceSpoke.Business

        creditControlManager = New bACTFinanceSpoke.Business
        creditControlManager.Initialise(
            sUsername:="",
            sPassword:="",
            iUserID:=1,
            iSourceID:=1,
            iLanguageID:=1,
            iCurrencyID:=26,
            iLogLevel:=PMELogLevel.PMLogError,
            sCallingAppName:=ACApp,
            vDatabase:=m_oDatabase)

        Try
            ' Set the vHeaderData Elements
            vHeaderData(0) = "SIRIUS"

            Dim sourceDetails As Object(,) = Nothing

            ' get applicable sources
            GetSourceDetails(sourceDetails)
            Dim sSystemOptionValue As String = GetSystemOption(SystemOptions.AutoArchiveEnabled)

            If creditControlManager IsNot Nothing AndAlso sourceDetails IsNot Nothing Then

                Dim lBound As Integer = sourceDetails.GetLowerBound(1)
                Dim UBound As Integer = sourceDetails.GetUpperBound(1)

                For source As Integer = lBound To UBound

                    vHeaderDetail(kbHDAsOfDate) = Date.Today
                    vHeaderDetail(kbHDSpoolDoc) = True
                    If sSystemOptionValue = "1" Then
                        vHeaderDetail(kbHDArchiveDoc) = True
                    End If
                    vHeaderDetail(kbHDBranch) = Trim(sourceDetails(1, source).ToString)
                    vHeaderData(1) = vHeaderDetail

                    Dim returnCode As Integer

                    returnCode = creditControlManager.Export(v_sInterfaceCode:=ACSpokeInterfaceCode,
                            r_sBatchRef:=ACSpokeBatch,
                            r_sStatusCode:=ACSpokeStatusCode,
                            r_sMessage:=ACSpokeMessage,
                            r_sHeaderXML:=ACSpokeHeaderXML,
                            r_vHeaderData:=CObj(vHeaderData),
                            r_vDetailData:=ACSpokeDetailData)

                    If returnCode <> PMEReturnCode.PMTrue AndAlso returnCode <> PMEReturnCode.PMNotFound Then
                        Dim msg As String = "Credit Control Run for Source:" + sourceDetails(1, source).ToString() + " failed"
                        Me.InsuredShortName = String.Empty
                        CreateTaskInstance(msg)
                    End If

                Next
            End If

        Finally
            creditControlManager.Dispose()
        End Try

    End Sub

    Private Sub GetSourceDetails(ByRef sourceDetails As Object(,))

        Dim vResults As Object = Nothing

        m_oDatabase.Parameters.Clear()
        ' Execute sql 
        Dim dbreturnCode As Integer
        dbreturnCode = m_oDatabase.SQLSelect("spu_PM_SelAll_Source", "spu_PM_SelAll_Source", True, vResultArray:=vResults)
        If dbreturnCode <> PMEReturnCode.PMTrue Then
            Throw New Exception("spu_PM_SelAll_Source failed")
        End If

        sourceDetails = DirectCast(vResults, Object(,))

    End Sub

    Private Sub ProcessPlanAmendment()

        If UpdateInstalmentPlanForAmendment() <> -1 Then
            ProcessBankNotification()
            ProcessPartyBankAmendment()
        End If


    End Sub
    Private Sub ProcessPartyBankAmendment()
        Dim msg As String
        Dim iNoofInstances As Integer
        Dim vResults(,) As Object = Nothing

        m_oDatabase.Parameters.Clear()
        ' Execute sql 

        ' update premium finance plan
        AddParameterLite(m_oDatabase, "planid", Me.PlanId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
        AddParameterLite(m_oDatabase, "NoofInstances", 0, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)


        Dim dbreturnCode As Integer
        dbreturnCode = m_oDatabase.SQLSelect("spu_Get_PartyBank_instances", "spu_Get_PartyBank_instances", True)
        If dbreturnCode <> PMEReturnCode.PMTrue Then
            Throw New Exception("spu_Get_PartyBank_instances")
        End If
        iNoofInstances = Util.ToSafeInt(m_oDatabase.Parameters.Item("NoofInstances").Value, 0)

        If iNoofInstances > 1 Then
            msg = "An change of payment details has been received for a party with multiple instalment plans and/or account types.The Party record has not been updated"
            CreateTaskInstance(msg)
        End If


    End Sub
    Private Sub ProcessReinstatement()

        ' the reinstatement process is validation only
        ' by the time this point has been reached the reinstatement process has already validated that  
        ' autogenerated plan reference / sort code / account number match with an existing plan
        Dim bankAccountName As String = CStr(GetAttribute("plan_bank_account_name"))

        If Me.PlanBankAccountName.ToUpper <> bankAccountName.ToUpper Then
            CreateTaskInstance("Re-instatement of DD rejected. Bank Details do not match.")
        End If

    End Sub

    Private Sub ProcessPlanCancellation()

        UpdateInstalmentPlanForCancellation()

        ProcessManualRenewal()

        If Me.CreditControlEnabled Then

            ' add first unpaid instalment on plan to credit control
            AddPlanToCreditControlCancellation()

        End If

    End Sub

    Private Sub UpdateInstalmentPlanForCancellation()

        Dim returnCode As Integer

        ' default action code to amendment
        Dim actionCode As String = CStr(GetAttribute("rejection_code"))

        ' set the plans status to on hold 
        Dim statusInd As String = InstalmentPlanStatus.OnHold

        ' use the current values from the stored plan
        Dim ccCancelled As Byte = Me.CcCancelled
        Dim ddCancelled As Byte = Me.DdCancelled

        ' determine whether its a cc cancellation or a dd cancellation
        Dim existingCCNumber As String = CStr(GetAttribute("plan_cc_number"))

        If Not String.IsNullOrEmpty(existingCCNumber) Then
            ccCancelled = CByte(1)
        Else
            ddCancelled = CByte(1)
        End If

        If String.IsNullOrEmpty(actionCode) Then
            actionCode = "Cancellation"
        End If

        ' update premium finance plan
        AddParameterLite(m_oDatabase, "pfprem_finance_cnt", Me.PlanId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
        AddParameterLite(m_oDatabase, "pfprem_finance_version", Me.PlanVersion, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
        AddParameterLite(m_oDatabase, "action_code", actionCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        AddParameterLite(m_oDatabase, "statusInd", statusInd, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        AddParameterLite(m_oDatabase, "dd_cancelled", ddCancelled, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
        AddParameterLite(m_oDatabase, "cc_cancelled", ccCancelled, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)

        ' Execute sql
        returnCode = m_oDatabase.SQLAction("spu_PFPremiumFinance_Import_Cancellation", "spu_PFPremiumFinance_Import_Cancellation", True)
        If returnCode <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'spu_PFPremiumFinance_Import_Cancellation'")
        End If

    End Sub

    Private Sub GetInstalmentCreditControlDetails(ByVal instalmentId As Integer, ByVal processMode As ProcessMode, ByRef creditControlDetails As Object(,))

        Dim vResults As Object = Nothing

        ' update premium finance plan
        AddParameterLite(m_oDatabase, "pfinstalments_id", instalmentId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
        AddParameterLite(m_oDatabase, "processMode", processMode, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)

        ' Execute sql 
        Dim dbreturnCode As Integer
        dbreturnCode = m_oDatabase.SQLSelect("spu_ACT_Get_Credit_Control_Details_For_Instalment", "spu_ACT_Get_Credit_Control_Details_For_Instalment", True, vResultArray:=vResults)
        If dbreturnCode <> PMEReturnCode.PMTrue Then
            Throw New Exception("spu_ACT_Get_Credit_Control_Details_For_Instalment failed")
        End If

        creditControlDetails = DirectCast(vResults, Object(,))

    End Sub

    Private Sub AddPlanToCreditControlCancellation()

        Dim instalmentId As Integer
        Dim amount As Decimal

        ' need to get instalment to put into credit control - this is basically the next one to be processed
        GetNextInstalmentToBeProcessedOnCancelledPlan(instalmentId, amount)

        ' if there is another instalment to be processed
        If instalmentId <> 0 Then

            Dim instalmentResultCode As String = CStr(GetAttribute("rejection_code"))
            Dim instalmentResultId As Integer = 0
            Dim returnCode As Integer = PMEReturnCode.PMTrue

            If Not String.IsNullOrEmpty(instalmentResultCode) Then
                ' get pfinstalments_result for specified result code
                returnCode = GetInstalmentsResultFromCode(instalmentResultCode, instalmentResultId)
            End If

            ' if the result code is valid
            If returnCode = PMEReturnCode.PMTrue Then

                ' if there is a valid result id allocate it to the instalment 
                ' before we get the search for appropriate credit control rule 
                ' to use as this determines which field is used
                If instalmentResultId <> 0 Then
                    UpdateInstalmentsResult(instalmentId, instalmentResultId)
                End If

                If ValidateCreditControlSetup(instalmentId, ProcessMode.Cancellation) Then

                    Dim business As bACTCreditControlItem.Business

                    ' just call generic add instalment method on bACTCreditControl

                    business = New bACTCreditControlItem.Business
                    business.Initialise(
                        sUsername:="",
                        sPassword:="",
                        iUserID:=1,
                        iSourceID:=1,
                        iLanguageID:=1,
                        iCurrencyID:=26,
                        iLogLevel:=PMELogLevel.PMLogError,
                        sCallingAppName:=ACApp,
                        vDatabase:=DirectCast(m_oDatabase, dPMDAO.Database))

                    Try

                        Dim reason As String = CStr(GetAttribute("rejection_code"))

                        If String.IsNullOrEmpty(reason) Then
                            reason = "Instalment Import - Plan Cancelled"
                        End If

                        Dim lRetVal As Integer

                        lRetVal = business.SetupCreditControlItemForInstalment(instalmentId, reason, ProcessMode.Cancellation)
                        If lRetVal <> 1 Then
                            Dim msg As String = Me.PlanReference + " SetupCreditControlItemForInstalments Failed "
                            CreateTaskInstance(msg)
                        End If

                    Catch ex As Exception
                        Throw
                    Finally
                        business.Dispose()
                    End Try

                End If

            End If

        End If

    End Sub

    Private Function ValidateCreditControlSetup(ByVal instalmentId As Integer, ByVal processMode As ProcessMode) As Boolean

        Dim creditControlDetails As Object(,) = Nothing

        ' get credit control details
        GetInstalmentCreditControlDetails(instalmentId, processMode, creditControlDetails)

        'Dim accountId As Integer = Util.ToSafeInt(creditControlDetails(0, 0), 0)
        'Dim planTransactionId As Integer = Util.ToSafeInt(creditControlDetails(1, 0), 0)
        'Dim insuranceFileCnt As Integer = Util.ToSafeInt(creditControlDetails(2, 0), 0)
        'Dim canAutoCancel As Boolean = Util.ToSafeBoolean(creditControlDetails(3, 0), False)
        Dim creditControlStepId As Integer = Util.ToSafeInt(creditControlDetails(4, 0), 0)
        'Dim dueDays As Integer = Util.ToSafeInt(creditControlDetails(5, 0), 0)
        'Dim planSourceId As Integer = Util.ToSafeInt(creditControlDetails(6, 0), 0)
        'Dim planId As Integer = Util.ToSafeInt(creditControlDetails(7, 0), 0)
        'Dim planVersion As Integer = Util.ToSafeInt(creditControlDetails(8, 0), 0)
        'Dim rateFrequencyId As Integer = Util.ToSafeInt(creditControlDetails(9, 0), 0)
        'Dim instalmentResultId As Integer = Util.ToSafeInt(creditControlDetails(10, 0), 0)
        'Dim instalmentDueDate As Date = Util.ToSafeDate(creditControlDetails(11, 0), Date.Now)
        Dim instalmentFailureCount As Integer = Util.ToSafeInt(creditControlDetails(12, 0), 0)
        'Dim rateRecollectOnNext As Boolean = Util.ToSafeBoolean(creditControlDetails(13, 0), False)
        'Dim rateRecollectDays As Integer = Util.ToSafeInt(creditControlDetails(14, 0), 0)
        'Dim rateRetryLimit As Integer = Util.ToSafeInt(creditControlDetails(15, 0), 0)
        'Dim creditControlItemId As Integer = Util.ToSafeInt(creditControlDetails(16, 0), 0)
        Dim frequencyDescription As String = CStr(creditControlDetails(17, 0))
        Dim sourceDescription As String = CStr(creditControlDetails(18, 0))
        Dim instalmentResultDescription As String = CStr(creditControlDetails(20, 0))
        Dim businessType As String = CStr(creditControlDetails(21, 0))
        Dim policyIsPaid As Integer = Util.ToSafeInt(creditControlDetails(22, 0), 0)
        Dim insuranceFileStatus As String = CStr(creditControlDetails(23, 0))

        If creditControlStepId <> 0 Then
            Return True
        Else

            If processMode = ProcessMode.Cancellation Then

                If String.IsNullOrEmpty(insuranceFileStatus) Then
                    insuranceFileStatus = "Live"
                End If

                'Dim msg As String = PlanReference + " failed to find credit control step on a credit control rule" & _
                '" which has the following configuration -  business_type:=" + businessType & _
                Dim msg As String = PlanReference + " missing CCStep on CCRule" &
                            " with config - business_type:=" + businessType &
                            ", source:= " & sourceDescription &
                            ", frequency:= " & frequencyDescription &
                            ", policy is paid:= " & policyIsPaid.ToString &
                            ", pfinstalment result:= " & instalmentResultDescription &
                            ", insurance file status:= " & insuranceFileStatus

                CreateTaskInstance(msg)

            ElseIf processMode = ProcessMode.Rejection Then

                'Dim msg As String = PlanReference + " failed to find credit control step on a credit control rule" & _
                '" which has the following configuration -  business_type:=" + businessType & _
                Dim msg As String = PlanReference + " missing CCStep on CCRule" &
                " with cofig - business_type:=" + businessType &
                            ", source:= " & sourceDescription &
                            ", frequency:= " & frequencyDescription &
                            ", pfinstalment result:= " & instalmentResultDescription &
                            ", failure count:= " & instalmentFailureCount.ToString

                CreateTaskInstance(msg)

            End If

            Return False

        End If

    End Function

    Private Function UpdateInstalmentPlanForAmendment() As Integer

        Dim returnCode As Integer
        Dim sStrippedString As String
        Dim bValid As Boolean
        Dim oValidationMessage As Object
        Dim bOverridable As Boolean

        Dim bRaiseAmendmentError As Boolean =
            Util.ToSafeBoolean(GetHeaderAttribute("raise_error_on_amendment_if_policy_is_not_live_or_current"), False)

        If bRaiseAmendmentError Then
            If Me.CoverStartDate > Date.Now Or
                Me.CoverExpiryDate < Date.Now Or
                    Util.ToSafeInt(Me.InsuranceFileStatusId, 0) <> 0 Then

                Dim msg As String = Me.PlanReference + " - Import header attribute Raise_Error_On_Amendment_If_Policy_Is_Not_Live_Or_Current caused amendment to fail."
                CreateTaskInstance(msg)
                Return -1
                Exit Function
            End If
        End If

        ' get data to update
        Dim bankSortCode As String = CStr(GetAttribute("new_plan_bank_sort_code"))
        Dim bankAccountNumber As String = CStr(GetAttribute("new_plan_bank_account_number"))
        Dim bankAccountName As String = CStr(GetAttribute("new_plan_bank_account_name"))
        Dim bankName As String = CStr(GetAttribute("new_plan_bank_name"))
        Dim bankAddress1 As String = CStr(GetAttribute("new_plan_bank_address1"))
        Dim bankAddress2 As String = CStr(GetAttribute("new_plan_bank_address2"))
        Dim bankAddress3 As String = CStr(GetAttribute("new_plan_bank_address3"))
        Dim bankAddress4 As String = CStr(GetAttribute("new_plan_bank_address4"))
        Dim bankPostalCode As String = CStr(GetAttribute("new_plan_bank_postal_code"))
        Dim bankCountryCode As String = CStr(GetAttribute("new_plan_bank_country_code"))
        Dim ccNumber As String = CStr(GetAttribute("new_plan_cc_number"))
        Dim ccExpiryDate As String = CStr(GetAttribute("new_plan_cc_expiry_date"))
        Dim ccStartDate As String = CStr(GetAttribute("new_plan_cc_start_date"))
        Dim ccIssue As String = CStr(GetAttribute("new_plan_cc_issue"))
        Dim ccPin As String = CStr(GetAttribute("new_plan_cc_pin"))
        Dim paperlessdd As Boolean = Util.ToSafeBoolean(GetAttribute("paperlessdd"), False)
        Dim sBIC As String = CStr(GetAttribute("new_sepa_plan_bic"))
        Dim sIBAN As String = CStr(GetAttribute("new_sepa_plan_iban"))

        ' if not passed use the original detail from the plan for 
        ' bank account name only
        ' all the other details can be being cleared down when an empty entry is provided
        If String.IsNullOrEmpty(bankAccountName) Then
            bankAccountName = Me.PlanBankAccountName
        End If

        ' set the plans status 
        Dim statusInd As String = Me.PlanStatus
        Dim planOnHold As Boolean = Util.ToSafeBoolean(GetAttribute("plan_on_hold"), False)
        If planOnHold Then
            statusInd = InstalmentPlanStatus.OnHold
        End If

        ' default action code to amendment
        'Dim actionCode As String = "Amendment"
        Dim actionCode As String = "ADDACS 03"
        ' use the current values from the stored plan
        Dim ccCancelled As Byte = Me.CcCancelled
        Dim ddCancelled As Byte = Me.DdCancelled

        ' verify that the bank account details have been provided
        If Not String.IsNullOrEmpty(bankSortCode) And String.IsNullOrEmpty(bankAccountNumber) Then
            Dim msg As String = Me.PlanReference + " Amendment Failed. Bank Sort Code and Account Number do not match."
            CreateTaskInstance(msg)
            Return -1
            Exit Function
        End If


        Dim oSirMediaTypeValidation As bSIRMediaTypeValidation.Business
        oSirMediaTypeValidation = New bSIRMediaTypeValidation.Business
        oSirMediaTypeValidation.Initialise(
            sUsername:="",
            sPassword:="",
            iUserID:=1,
            iSourceID:=1,
            iLanguageID:=1,
            iCurrencyID:=26,
            iLogLevel:=PMELogLevel.PMLogError,
            sCallingAppName:=ACApp,
            vDatabase:=m_oDatabase)

        sStrippedString = Replace(bankSortCode, " ", "") & "|" & Replace(bankAccountNumber, " ", "")

        'Modified as change the parameter as per Requirement
        'oSirMediaTypeValidation.ValidateNumber(0, 1, sStrippedString, bValid, "", "", "", "", "", "", oValidationMessage, bOverridable, "Bank")
        oSirMediaTypeValidation.ValidateNumber(0, 1, sStrippedString, bValid, "", "", "", "", "", "", CStr(oValidationMessage), bOverridable, "Bank", sBIC:=sBIC, sIBAN:=sIBAN)
        If DirectCast(bValid, Boolean) = True Or bOverridable Then
            'do nothing
        Else
            CreateTaskInstance("ADDACS update rejected. Bank Codes invalid")
            Return -1
            Exit Function
        End If
        If CStr(GetAttribute("plan_cc_number")) = String.Empty Then

            ' if not passed use the original detail for sort code and bank account number
            If String.IsNullOrEmpty(bankSortCode) Then
                ' replace with original
                bankSortCode = CStr(GetAttribute("plan_bank_sort_code"))
            End If

            If String.IsNullOrEmpty(bankAccountNumber) Then
                ' replace with original
                bankAccountNumber = CStr(GetAttribute("plan_bank_account_number"))
            End If

            If String.IsNullOrEmpty(sBIC) Then
                sBIC = CStr(GetAttribute("sepa_plan_bic"))
            End If

            If String.IsNullOrEmpty(sIBAN) Then
                sIBAN = CStr(GetAttribute("sepa_plan_iban"))
            End If

        Else

            ' if not passed use the original detail for sort code and bank account number
            If String.IsNullOrEmpty(bankSortCode) Then
                ccNumber = CStr(GetAttribute("plan_cc_number"))
            End If

        End If

        ' update premium finance plan
        AddParameterLite(m_oDatabase, "pfprem_finance_cnt", Me.PlanId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
        AddParameterLite(m_oDatabase, "pfprem_finance_version", Me.PlanVersion, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
        AddParameterLite(m_oDatabase, "action_code", actionCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        AddParameterLite(m_oDatabase, "PlanBankSortCode", bankSortCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        AddParameterLite(m_oDatabase, "PlanBankAccountNumber", bankAccountNumber, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        AddParameterLite(m_oDatabase, "PlanBankAccountName", bankAccountName, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        AddParameterLite(m_oDatabase, "PlanBankName", bankName, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        AddParameterLite(m_oDatabase, "PlanBankAddress1", bankAddress1, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        AddParameterLite(m_oDatabase, "PlanBankAddress2", bankAddress2, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        AddParameterLite(m_oDatabase, "PlanBankAddress3", bankAddress3, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        AddParameterLite(m_oDatabase, "PlanBankAddress4", bankAddress4, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        AddParameterLite(m_oDatabase, "PlanBankPostalCode", bankPostalCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        AddParameterLite(m_oDatabase, "PlanBankCountryCode", bankCountryCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        AddParameterLite(m_oDatabase, "PlanCCNumber", ccNumber, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        AddParameterLite(m_oDatabase, "PlanCCExpiryDate", ccExpiryDate, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        AddParameterLite(m_oDatabase, "PlanCCStartDate", ccStartDate, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        AddParameterLite(m_oDatabase, "PlanCCIssue", ccIssue, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        AddParameterLite(m_oDatabase, "PlanCCPin", ccPin, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        AddParameterLite(m_oDatabase, "statusInd", statusInd, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        AddParameterLite(m_oDatabase, "dd_cancelled", ddCancelled, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
        AddParameterLite(m_oDatabase, "cc_cancelled", ccCancelled, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
        AddParameterLite(m_oDatabase, "userid", Me.DefaultUserId, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
        AddParameterLite(m_oDatabase, "sBusinessIdentifierCode", sBIC, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        AddParameterLite(m_oDatabase, "sInternationalBankAccountNumber", sIBAN, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        'PN 48823 (WRADDACS 180)
        ' only add the parameters where paperless is true
        'If paperlessdd Then
        ' reset the paper dd indicator to zero
        AddParameterLite(m_oDatabase, "paperdd", 0, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
        'End If

        ' Execute sql
        returnCode = m_oDatabase.SQLAction("spu_PFPremiumFinance_Import_Amendment", "spu_PFPremiumFinance_Import_Amendment", True)
        If returnCode <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'spu_ACT_Import_Payment_CreateCashListItem'")
        End If

    End Function

    Private Sub ProcessBankNotification()

        Dim returnCode As PMEReturnCode
        Dim iPrevMediaHistoryId As Integer
        Dim iCurrMediaHistoryId As Integer

        Dim notificationCode As String = CStr(GetAttribute("generate_instalment_notification"))
        Dim nextInstalmentTransactionCode As String = CStr(GetAttribute("set_next_instalment_transaction_code"))

        Dim notificationCodeIsValid As Boolean = IsPfInstalmentTransactionCodeValid(notificationCode)
        Dim nextInstalmentTransactionCodeIsValid As Boolean = IsPfInstalmentTransactionCodeValid(nextInstalmentTransactionCode)

        If notificationCodeIsValid = False Then
            CreateTaskInstance("generate_instalment_notification = " + notificationCode + " is not valid")
        End If

        If nextInstalmentTransactionCodeIsValid = False Then
            CreateTaskInstance("generate_instalment_notification = " + nextInstalmentTransactionCode + " is not valid")
        End If

        returnCode = GetMediatypeHistoryId(iPrevMediaHistoryId, iCurrMediaHistoryId)
        If returnCode = PMEReturnCode.PMFalse Then
            Throw New Exception("Unable to fetch Media Type History Id.")
        End If


        If Not String.IsNullOrEmpty(notificationCode) AndAlso notificationCodeIsValid Then
            CreateInstalmentNotification(notificationCode, iCurrMediaHistoryId)
            If Not String.IsNullOrEmpty(nextInstalmentTransactionCode) AndAlso nextInstalmentTransactionCodeIsValid Then
                UpdateNextInstalmentTransactionCode(nextInstalmentTransactionCode)
            End If
            'PN4643-Start
        Else
            CreateInstalmentNotification("0C", iPrevMediaHistoryId)
            CreateInstalmentNotification("0N", iCurrMediaHistoryId)
            'PN4643-End
        End If

    End Sub

    Private Function IsPfInstalmentTransactionCodeValid(ByVal pfinstalments_transaction_code As String) As Boolean

        If Not String.IsNullOrEmpty(pfinstalments_transaction_code) Then

            ' ensure we have the list of valid transaction codes
            If Me.ListOfPFInstalmentTransactionCodes Is Nothing Then
                GetPFInstalmentTransactionCodes()
            End If

            Me.SearchString = pfinstalments_transaction_code

            If ListOfPFInstalmentTransactionCodes.Exists(AddressOf FindStringExists) Then
                Return True
            Else
                Return False
            End If

        Else

            Return True

        End If

    End Function

    Private Function FindStringExists(ByVal listString As String) As Boolean

        If listString.ToUpper.Trim = Me.SearchString.ToUpper.Trim Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Sub GetPFInstalmentTransactionCodes()

        ListOfPFInstalmentTransactionCodes = New List(Of String)

        Dim arrayOfTransactionCodes As Object(,) = Nothing
        Dim vResults As Object = Nothing
        Dim dbreturnCode As Integer

        m_oDatabase.Parameters.Clear()

        dbreturnCode = m_oDatabase.SQLSelect(
        sSQL:="spu_ACT_Import_Get_PFInstalments_Transaction",
        sSQLName:="spu_ACT_Import_Get_PFInstalments_Transaction",
        bStoredProcedure:=True,
        vResultArray:=vResults)

        If dbreturnCode <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'spu_ACT_Import_Get_PFInstalments_Transaction'")
        End If

        arrayOfTransactionCodes = DirectCast(vResults, Object(,))

        Dim lBound As Integer = arrayOfTransactionCodes.GetLowerBound(1)
        Dim UBound As Integer = arrayOfTransactionCodes.GetUpperBound(1)
        Dim transactionCodeIndex As Integer
        Dim transactionCode As String

        For transactionCodeIndex = lBound To UBound

            transactionCode = CStr(arrayOfTransactionCodes(0, transactionCodeIndex))

            If Not String.IsNullOrEmpty(transactionCode) Then
                ListOfPFInstalmentTransactionCodes.Add(transactionCode)
            End If

        Next

    End Sub

    Private Sub UpdateNextInstalmentTransactionCode(ByVal transactionCode As String)

        AddParameterLite(m_oDatabase, "pfprem_finance_cnt", Me.PlanId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
        AddParameterLite(m_oDatabase, "pfprem_finance_version", Me.PlanVersion, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
        AddParameterLite(m_oDatabase, "pfinstalments_transaction_code", transactionCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)

        Dim dbreturnCode As Integer
        dbreturnCode = m_oDatabase.SQLAction(sSQL:="spu_ACT_Import_Update_Instalment_Transaction_Code", sSQLName:="spu_ACT_Import_Create_Instalment_Notification", bStoredProcedure:=True)
        If dbreturnCode <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'spu_ACT_Import_Update_Instalment_Transaction_Code'")
        End If

    End Sub

    Private Sub ProcessInstalmentRejection()

        Dim planInstalmentGroupId As Integer = Util.ToSafeInt(GetAttribute("instalment_group_id"), 0)
        Dim planReference As String = CStr(GetAttribute("plan_reference"))
        Dim groupId As Integer = Util.ToSafeInt(GetAttribute("instalment_group_id"), 0)
        Dim instalmentAmount As Decimal = Util.ToSafeDecimal(GetAttribute("amount"), 0)
        Dim returnCode As PMEReturnCode

        If planInstalmentGroupId <> 0 Then
            returnCode = ValidateInstalmentsGroup(groupId)
        Else
            returnCode = GetInstalmentGroup(groupId)
        End If

        ' if the group is valid
        If returnCode = PMEReturnCode.PMTrue Then

            ' create pre update history items
            CreateGroupInstalmentHistoryItems(groupId)

            Dim instalmentResultCode As String = CStr(GetAttribute("rejection_code"))
            Dim instalmentResultId As Integer

            ' get pfinstalments_result for specified result code
            returnCode = GetInstalmentsResultFromCode(instalmentResultCode, instalmentResultId)

            ' if the result code is valid
            If returnCode = PMEReturnCode.PMTrue Then

                ProcessManualRenewal()

                ' update instalments with the result code
                UpdateGroupsInstalmentsResult(groupId, instalmentResultId)

                ' update the live plans status 
                UpdateLivePlanStatus()

                Dim recallInstalment As Boolean = Util.ToSafeBoolean(GetAttribute("recall_instalment"), False)
                If recallInstalment Then

                    ' Recall Instalment
                    RecallInstalments(groupId, instalmentAmount)

                End If

                ' create post update history items
                CreateGroupInstalmentHistoryItems(groupId)

                'Add Media type history
                AddMediaTypeHistory()

            Else

                ' invalid pfintalment reason 
                CreateTaskInstance(planReference + " failed to find pfInstalment_reason.code = " + instalmentResultCode)

            End If

        End If

    End Sub

    Private Sub CreateGroupInstalmentHistoryItems(
    ByVal groupId As Integer)

        ' update premium finance plan
        AddParameterLite(m_oDatabase, "group_Id", groupId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)

        ' Execute sql 
        Dim dbreturnCode As Integer
        dbreturnCode = m_oDatabase.SQLAction("spu_ACT_Add_PFInstalments_History_For_Group", "spu_ACT_PFInstalments_For_Group_Select", True)
        If dbreturnCode <> PMEReturnCode.PMTrue Then
            Throw New Exception("spu_ACT_Get_Credit_Control_Details_For_Instalment failed")
        End If

    End Sub



    Private Sub ProcessManualRenewal()

        ' determine if the process needs to force manual renewal
        Dim forceManualRenewal As Boolean = Util.ToSafeBoolean(GetAttribute("force_manual_renewal"), False)
        If forceManualRenewal Then
            UpdateInsuranceFileToForceManualRenewal(Me.InsuranceFileCnt)
        End If

    End Sub

    Private Sub GetInstalmentsForGroup(
    ByVal groupId As Integer,
    ByRef instalmentDetails As Object(,), Optional ByVal instalmentAmount As Decimal = Nothing)

        Dim vResults As Object = Nothing

        ' update premium finance plan
        AddParameterLite(m_oDatabase, "group_Id", groupId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)

        AddParameterLite(m_oDatabase, "instalmentAmount", instalmentAmount, PMEParameterDirection.PMParamInput, PMEDataType.PMDecimal, False)
        ' Execute sql 
        Dim dbreturnCode As Integer
        dbreturnCode = m_oDatabase.SQLSelect("spu_ACT_Select_PFInstalments_For_Group_Select", "spu_ACT_PFInstalments_For_Group_Select", True, vResultArray:=vResults)
        If dbreturnCode <> PMEReturnCode.PMTrue Then
            Throw New Exception("spu_ACT_Get_Credit_Control_Details_For_Instalment failed")
        End If

        instalmentDetails = DirectCast(vResults, Object(,))

    End Sub

    Private Sub RecallInstalments(ByVal groupId As Integer, Optional ByVal instalmentAmount As Decimal = Nothing)

        Dim instalmentDetails As Object(,) = Nothing

        ' get instalments for group
        GetInstalmentsForGroup(groupId, instalmentDetails, instalmentAmount)

        Dim instalmentId As Integer
        Dim instalmentsTransactionCode As String
        Dim planTransactionID As Integer
        Dim instalmentTransactionId As Integer
        Dim instalmentsStatusId As Integer
        Dim defaultCreditControlItemReason As String
        Dim retryLimitReached As Boolean

        Dim returnCode As Integer

        Dim recallManager As bSIRPFInstalments.Business

        recallManager = New bSIRPFInstalments.Business
        recallManager.Initialise(
            sUsername:="",
            sPassword:="",
            iUserID:=1,
            iSourceID:=1,
            iLanguageID:=1,
            iCurrencyID:=26,
            iLogLevel:=PMELogLevel.PMLogError,
            sCallingAppName:=ACApp,
            vDatabase:=m_oDatabase)


        Try

            If recallManager IsNot Nothing AndAlso instalmentDetails IsNot Nothing Then

                Dim lBound As Integer = instalmentDetails.GetLowerBound(1)
                Dim UBound As Integer = instalmentDetails.GetUpperBound(1)
                Dim instalment As Integer

                For instalment = lBound To UBound

                    instalmentId = Util.ToSafeInt(instalmentDetails(0, instalment), 0)
                    instalmentsTransactionCode = Trim(CStr(instalmentDetails(1, instalment)))
                    planTransactionID = Util.ToSafeInt(instalmentDetails(2, instalment), 0)
                    instalmentTransactionId = Util.ToSafeInt(instalmentDetails(3, instalment), 0)
                    instalmentsStatusId = Util.ToSafeInt(instalmentDetails(4, instalment), 0)
                    defaultCreditControlItemReason = "SiriusImport - Instalment Import"
                    retryLimitReached = False
                    If Not (Me.PlanStatus = InstalmentPlanStatus.Superseded OrElse Me.PlanStatus = InstalmentPlanStatus.Cancelled) Then

                        ValidateCreditControlSetup(instalmentId, ProcessMode.Rejection)
                    End If
                    recallManager.PlanStatus = Me.PlanStatus
                    recallManager.PFFinancePlanCnt = Me.PlanId
                    recallManager.PFFinancePlanVersion = Me.PlanVersion
                    returnCode = recallManager.RecallInstalment(
                    instalmentsTransactionCode,
                    planTransactionID,
                    instalmentTransactionId,
                    instalmentId,
                    instalmentsStatusId,
                    defaultCreditControlItemReason,
                    Me.CreditControlEnabled,
                    retryLimitReached,
                    ProcessMode.Rejection)

                    If returnCode = PMEReturnCode.PMTrue Then

                        If retryLimitReached Then

                            ' create work manager task
                            Dim msg As String = "Instalment PFinstalmentsId :-" + instalmentId.ToString() + " has reached its retry limit."
                            CreateTaskInstance(msg)

                        End If
                        If Me.PlanStatus = InstalmentPlanStatus.Superseded OrElse Me.PlanStatus = InstalmentPlanStatus.Cancelled Then
                            ' Create credit control work item entry for SED
                            returnCode = SetupCreditControlDetailsForCancelledSupersededPlan(Me.InsuranceFileCnt)
                            If returnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                                Throw New Exception("Method SetupCreditControlDetailsForCancelledSupersededPlan failed")
                            End If

                            Dim msg As String = "SED has been added to client account because plan is either superseded or cancelled."

                            ' Assigned value according to requirement.
                            Me.TaskCustomer = "INSTALMENT_IMPORT"

                            CreateTaskInstance(msg)

                            ' Assigned origional value from import XML.
                            Me.TaskCustomer = CStr(GetAttribute("generated_task_customer"))
                        End If
                    Else

                        Dim msg As String = "Failed to recall instalment with PFinstalmentsId :-" + instalmentId.ToString()
                        CreateTaskInstance(msg)

                    End If

                Next

            End If

        Finally
            recallManager.Dispose()
        End Try

    End Sub

    Private Sub UpdateLivePlanStatus()

        Dim planOnHold As Boolean = Util.ToSafeBoolean(GetAttribute("plan_on_hold"), False)
        If planOnHold AndAlso Me.PlanStatus <> InstalmentPlanStatus.Superseded AndAlso
            Me.PlanStatus <> InstalmentPlanStatus.Cancelled Then
            UpdateInstalmentPlanStatus(Me.PlanId, Me.PlanVersion, InstalmentPlanStatus.OnHold)
        ElseIf Me.PlanStatus = InstalmentPlanStatus.Completed Then
            UpdateInstalmentPlanStatus(Me.PlanId, Me.PlanVersion, InstalmentPlanStatus.Live)
        End If

    End Sub

    Private Sub UpdateInsuranceFileToForceManualRenewal(ByVal insuranceFileCnt As Integer)

        AddParameterLite(m_oDatabase, "insurance_file_cnt", insuranceFileCnt, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)

        ' Execute sql
        Dim dbreturnCode As Integer
        dbreturnCode = m_oDatabase.SQLAction("spu_SIR_Insurance_File_To_Force_Manual_Renewal", "spu_SIR_Insurance_File_To_Force_Manual_Renewal", True)
        If dbreturnCode <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'spu_SIR_Insurance_File_To_Force_Manual_Renewal'")
        End If

    End Sub

    Private Sub UpdateInstalmentsResult(ByVal instalmentId As Integer, ByVal resultId As Integer)

        AddParameterLite(m_oDatabase, "pfinstalments_id", instalmentId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
        AddParameterLite(m_oDatabase, "pfinstalments_result_id", resultId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)

        ' Execute sql
        Dim dbreturnCode As Integer
        dbreturnCode = m_oDatabase.SQLAction("spu_PfInstalments_Result_Update", "spu_PfInstalments_Result_Update", True)
        If dbreturnCode <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'spu_PfInstalment_Result_Update'")
        End If

    End Sub

    Private Sub UpdateGroupsInstalmentsResult(ByVal groupId As Integer, ByVal resultId As Integer)

        AddParameterLite(m_oDatabase, "group_id", groupId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
        AddParameterLite(m_oDatabase, "pfinstalments_result_id", resultId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)

        ' Execute sql
        Dim dbreturnCode As Integer
        dbreturnCode = m_oDatabase.SQLAction("spu_PfInstalments_Group_Result_Update", "spu_PfInstalments_Group_Result_Update", True)
        If dbreturnCode <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'spu_PfInstalments_Group_Result_Update'")
        End If

    End Sub

    Private Sub UpdateInstalmentPlanStatus(ByVal id As Integer, ByVal version As Integer, ByVal status As String)

        AddParameterLite(m_oDatabase, "pfprem_finance_cnt", id, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
        AddParameterLite(m_oDatabase, "pfprem_finance_version", version, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
        AddParameterLite(m_oDatabase, "statusind", status, PMEParameterDirection.PMParamInput, PMEDataType.PMString)

        ' Execute sql
        Dim dbreturnCode As Integer
        dbreturnCode = m_oDatabase.SQLAction("spu_PFPremiumFinance_Status_Update", "spu_PFPremiumFinance_Status_Update", True)
        If dbreturnCode <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'spu_PFPremiumFinance_Status_Update'")
        End If

    End Sub

    Private Sub GetInstalmentPlan(
                ByVal planReference As String,
                ByVal planCCNumber As String,
                ByVal planBankSortCode As String,
                ByVal planBankAccountNumber As String)

        Dim iReturn As Integer
        Dim nGroupId As Integer = Util.ToSafeInt(GetAttribute("instalment_group_id"), 0)
        Dim nReturnCode As PMEReturnCode

        If nGroupId <> 0 Then
            nReturnCode = ValidateInstalmentsGroup(nGroupId)
        Else
            nReturnCode = GetInstalmentGroup(nGroupId)
        End If
        If nReturnCode = PMEReturnCode.PMTrue Then

            AddParameterLite(m_oDatabase, "plan_Ref", planReference, PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)
            AddParameterLite(m_oDatabase, "pfpremfinancecnt", 0, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "pfpremfinanceversion", 0, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "statusind", CStr(0), PMEParameterDirection.PMParamOutput, PMEDataType.PMString)

            If Not String.IsNullOrEmpty(planCCNumber) Then
                AddParameterLite(m_oDatabase, "cc_number", planCCNumber, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "banksortcode", DBNull.Value, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "bankaccountno", DBNull.Value, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            Else
                AddParameterLite(m_oDatabase, "cc_number", DBNull.Value, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "banksortcode", planBankSortCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "bankaccountno", planBankAccountNumber, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            End If

            AddParameterLite(m_oDatabase, "ddcancelled", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMBoolean)
            AddParameterLite(m_oDatabase, "ccCancelled", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMBoolean)
            AddParameterLite(m_oDatabase, "planBankAccountName", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMString)

            AddParameterLite(m_oDatabase, "cover_start_date", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMDate)
            AddParameterLite(m_oDatabase, "expiry_date", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMDate)
            AddParameterLite(m_oDatabase, "insurance_file_status_id", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "insurance_file_cnt", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "insured_shortname", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "instalment_group_id", nGroupId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            ' Execute sql
            iReturn = m_oDatabase.SQLSelect("spu_ACT_Import_Get_Instalment_Plan", "spu_ACT_Import_Get_Instalment_Plan", True)
            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spu_ACT_Import_Get_Instalment_Plan'")
            End If

            Me.PlanId = Util.ToSafeInt(m_oDatabase.Parameters.Item("pfpremfinancecnt").Value, 0)
            Me.PlanVersion = Util.ToSafeInt(m_oDatabase.Parameters.Item("pfpremfinanceversion").Value, 0)
            Me.PlanStatus = m_oDatabase.Parameters.Item("statusind").Value.ToString
            Me.DdCancelled = Util.ToSafeByte(m_oDatabase.Parameters.Item("ddcancelled").Value.ToString, 0)
            Me.CcCancelled = Util.ToSafeByte(m_oDatabase.Parameters.Item("cccancelled").Value.ToString, 0)
            Me.PlanBankAccountName = m_oDatabase.Parameters.Item("planBankAccountName").Value.ToString

            If Not String.IsNullOrEmpty(Me.PlanBankAccountName) Then
                Me.PlanBankAccountName = Me.PlanBankAccountName.Trim
            End If

            Me.CoverStartDate = Util.ToSafeDate(m_oDatabase.Parameters.Item("cover_start_date").Value.ToString, Date.MinValue)
            Me.CoverExpiryDate = Util.ToSafeDate(m_oDatabase.Parameters.Item("expiry_date").Value.ToString, Date.MinValue)
            Me.InsuranceFileStatusId = Util.ToSafeInt(m_oDatabase.Parameters.Item("insurance_file_status_id").Value.ToString, 0)
            Me.InsuranceFileCnt = Util.ToSafeInt(m_oDatabase.Parameters.Item("insurance_file_cnt").Value, 0)
            Me.InsuredShortName = m_oDatabase.Parameters.Item("insured_shortname").Value.ToString

            If Not String.IsNullOrEmpty(Me.InsuredShortName) Then
                Me.InsuredShortName = Me.InsuredShortName.Trim
            End If
        End If
    End Sub

    Private Function LocateLivePlan() As PMEReturnCode

        Const PlanNotFound As Integer = 0

        Me.PlanReference = CStr(GetAttribute("plan_reference"))

        Dim planCCNumber As String = CStr(GetAttribute("plan_cc_number"))
        Dim planBankSortCode As String = CStr(GetAttribute("plan_bank_sort_code"))
        Dim planBankAccountNumber As String = CStr(GetAttribute("plan_bank_account_number"))

        Dim returnCode As PMEReturnCode = PMEReturnCode.PMTrue

        GetInstalmentPlan(PlanReference, planCCNumber, planBankSortCode, planBankAccountNumber)

        Dim taskDescription As String = String.Empty

        If Me.PlanId = PlanNotFound Then
            NoOfRejections += 1
            If Me.ActionTypeCode = ActionType.Reinstatement Then
                taskDescription = "Re-instatement of DD rejected. Bank Details do not match. Reference: " & Me.PlanReference
                returnCode = PMEReturnCode.PMFalse
            Else
                taskDescription = "Update rejected. Client Instalment Plan details not recognised. Reference: " & Me.PlanReference
                returnCode = PMEReturnCode.PMFalse
            End If
        ElseIf Me.PlanStatus <> InstalmentPlanStatus.Live AndAlso
                Me.PlanStatus <> InstalmentPlanStatus.Completed AndAlso Me.PlanStatus <> InstalmentPlanStatus.Superseded AndAlso Me.PlanStatus <> InstalmentPlanStatus.Cancelled Then
            NoOfRejections += 1
            taskDescription = "Update rejected. Client Instalment Plan is not valid. Reference: " & Me.PlanReference
            returnCode = PMEReturnCode.PMFalse
        ElseIf Me.PlanStatus = InstalmentPlanStatus.OnHold Then

            If Me.ActionTypeCode = ActionType.Amendment Then
                NoOfRejections += 1
                Dim bRaiseAmendmentErrorForInstalmentPlanOnHold As Boolean =
                Util.ToSafeBoolean(GetHeaderAttribute("raise_error_on_amendment_if_instalment_plan_is_on_hold"), False)
                If bRaiseAmendmentErrorForInstalmentPlanOnHold Then
                    taskDescription = "Bank details amendment required to instalment plan on hold. Reference: " & Me.PlanReference
                    returnCode = PMEReturnCode.PMFalse
                End If
            End If

        End If

        If Not String.IsNullOrEmpty(taskDescription) Then
            CreateTaskInstance(taskDescription)
        End If

        Return returnCode

    End Function

    Private Sub GetDefaultTaskDetails(
    ByRef taskId As Integer,
    ByRef taskGroupId As Integer,
    ByRef userGroupId As Integer,
    ByRef userId As Integer)

        Dim returnCode As Integer

        AddParameterLite(m_oDatabase, "pmwrk_task_id", 0, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong, True)
        AddParameterLite(m_oDatabase, "pmwrk_task_group_id", 0, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)
        AddParameterLite(m_oDatabase, "pmuser_group_id", 0, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)
        AddParameterLite(m_oDatabase, "pmuser_id", 0, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)

        ' Execute sql
        returnCode = m_oDatabase.SQLSelect("spu_ACT_Import_Get_Default_Task_Details", "spu_ACT_Import_Get_Default_Task_Details", True)
        If returnCode <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'spu_ACT_Import_Payment_CreateCashListItem'")
        End If

        taskId = Util.ToSafeInt(m_oDatabase.Parameters.Item("pmwrk_task_id").Value, 0)
        taskGroupId = Util.ToSafeInt(m_oDatabase.Parameters.Item("pmwrk_task_group_id").Value, 0)
        userGroupId = Util.ToSafeInt(m_oDatabase.Parameters.Item("pmuser_group_id").Value, 0)
        userId = Util.ToSafeInt(m_oDatabase.Parameters.Item("pmuser_id").Value, 0)

    End Sub

    Private Sub CreateTaskInstance(ByVal description As String)
        CreateTaskInstance(Me.DefaultTaskGroupId, Me.DefaultTaskId, Me.DefaultUserGroupId, Me.DefaultUserId, description)
    End Sub

    Private Sub CreateTaskInstance(
        ByVal taskGroupId As Integer,
        ByVal taskId As Integer,
        ByVal userGroupId As Integer,
        ByVal userId As Integer,
        ByVal description As String)

        Dim taskInstanceCnt As Integer
        Dim replacementUserGroupId As Integer
        Dim descriptionToUse As String = String.Empty

        If Me.ActionTypeCode = ActionType.Rejection Then
            replacementUserGroupId = Me.AruddsUserGroup
        Else
            replacementUserGroupId = Me.AddacsUserGroup
        End If

        ' replace the default user group if system appropriate options have been set up
        If replacementUserGroupId <> 0 Then
            userGroupId = replacementUserGroupId
        End If

        If String.IsNullOrEmpty(Me.TaskCustomer) Then
            Me.TaskCustomer = "INSTALMENT_IMPORT"
        End If

        If String.IsNullOrEmpty(Me.TaskDescriptionPrefix) Then
            descriptionToUse = description
        Else
            descriptionToUse = Me.TaskDescriptionPrefix + " - " + description
        End If

        If Not String.IsNullOrEmpty(Me.InsuredShortName) Then
            descriptionToUse = Me.InsuredShortName + " - " + descriptionToUse
        End If

        CreateTaskInstance(taskInstanceCnt, taskGroupId, taskId, Me.TaskCustomer, userGroupId, userId, descriptionToUse, "")

    End Sub

    Private Sub CreateTaskInstance(
    ByRef taskInstanceCnt As Integer,
    ByVal taskGroupId As Integer,
    ByVal taskId As Integer,
    ByVal customer As String,
    ByVal userGroupId As Integer,
    ByVal userId As Integer,
    ByVal description As String,
    ByVal workflowInformation As String)

        Dim returnCode As Integer


        AddParameterLite(m_oDatabase, "pmwrk_task_instance_cnt", 0, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
        AddParameterLite(m_oDatabase, "pmwrk_task_group_id", taskGroupId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
        AddParameterLite(m_oDatabase, "pmwrk_task_id", taskId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
        AddParameterLite(m_oDatabase, "customer", customer, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        AddParameterLite(m_oDatabase, "task_due_date", Date.Now, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
        AddParameterLite(m_oDatabase, "pmuser_group_id", userGroupId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
        AddParameterLite(m_oDatabase, "user_id", userId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
        AddParameterLite(m_oDatabase, "description", description, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        AddParameterLite(m_oDatabase, "task_status", CStr(0), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        AddParameterLite(m_oDatabase, "is_urgent", 0, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
        AddParameterLite(m_oDatabase, "date_created", Date.Now, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
        AddParameterLite(m_oDatabase, "created_by_id", 1, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
        AddParameterLite(m_oDatabase, "last_modified", Date.Now, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
        AddParameterLite(m_oDatabase, "modified_by_id", 1, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
        AddParameterLite(m_oDatabase, "is_visible", 1, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
        AddParameterLite(m_oDatabase, "workflow_information", workflowInformation, PMEParameterDirection.PMParamInput, PMEDataType.PMString)

        ' Execute sql
        returnCode = m_oDatabase.SQLAction("spe_PMWrk_Task_Instance_add", "spe_PMWrk_Task_Instance_add", True)
        If returnCode <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'spe_PMWrk_Task_Instance_add'")
        End If

    End Sub

    Private Sub GetInstalmentPlanDetailsForGroupId(
    ByRef groupId As Integer,
    ByRef countOfvalidInstalmentStatusesWithinGroup As Integer)

        AddParameterLite(m_oDatabase, "group_id", groupId, PMEParameterDirection.PMParamInputOutput, PMEDataType.PMLong, True)
        AddParameterLite(m_oDatabase, "count", 0, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)

        ' Execute sql
        Dim dbreturnCode As Integer
        dbreturnCode = m_oDatabase.SQLAction("spu_ACT_Import_Get_InstalmentGroupDetails", "spu_ACT_Import_Get_InstalmentGroupDetails", True)
        If dbreturnCode <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'spu_ACT_Import_Get_InstalmentGroupDetails'")
        End If

        groupId = Util.ToSafeInt(m_oDatabase.Parameters.Item("group_id").Value, 0)
        countOfvalidInstalmentStatusesWithinGroup = Util.ToSafeInt(m_oDatabase.Parameters.Item("count").Value, 0)

    End Sub

    Private Sub GetApplicableSystemOptions()

        ' credit control enabled 
        Dim SystemOptionValue As String = GetSystemOption(SystemOptions.CreditControlEnabled)
        If SystemOptionValue = "1" Then
            Me.CreditControlEnabled = True
        End If

        Me.AddacsUserGroup = Util.ToSafeInt(GetSystemOption(SystemOptions.ADDACSUserGroup), 0)
        Me.AruddsUserGroup = Util.ToSafeInt(GetSystemOption(SystemOptions.ARUDDSUserGroup), 0)

    End Sub

    Private Function GetNextInstalmentToBeProcessedOnCancelledPlan(
    ByRef instalmentId As Integer,
    ByRef amount As Decimal) As Integer

        ' update premium finance plan
        AddParameterLite(m_oDatabase, "pfprem_finance_cnt", Me.PlanId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
        AddParameterLite(m_oDatabase, "pfprem_finance_version", Me.PlanVersion, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
        AddParameterLite(m_oDatabase, "instalment_id", 0, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)
        AddParameterLite(m_oDatabase, "amount", 0, PMEParameterDirection.PMParamOutput, PMEDataType.PMCurrency)

        ' Execute sql 
        Dim dbreturnCode As Integer
        dbreturnCode = m_oDatabase.SQLSelect("spu_ACT_Import_Get_Next_Instalment_For_Cancelled_Plan", "spu_ACT_Import_Get_Next_Instalment_For_Cancelled_Plan", True)
        If dbreturnCode <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'spu_ACT_Import_Get_Next_Instalment_For_Cancelled_Plan'")
        End If

        instalmentId = Util.ToSafeInt(m_oDatabase.Parameters.Item("instalment_id").Value, 0)
        amount = Util.ToSafeDecimal(m_oDatabase.Parameters.Item("amount").Value, 0)

    End Function

    Private Function GetInstalmentsResultFromCode(
    ByVal code As String,
    ByRef id As Integer) As PMEReturnCode

        Dim returnCode As Integer
        Dim retVal As PMEReturnCode = PMEReturnCode.PMFalse

        AddParameterLite(m_oDatabase, "code", code, PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)
        AddParameterLite(m_oDatabase, "pfinstalments_result_id", code, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)

        returnCode = m_oDatabase.SQLSelect("spu_PFInstalments_Result_Sel_By_Code", "spu_PFInstalments_Result_Sel_By_Code", True)
        If returnCode <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'spu_PFInstalments_Result_Sel_By_Code'")
        End If

        id = Util.ToSafeInt(m_oDatabase.Parameters.Item("pfinstalments_result_id").Value, 0)
        If id <> 0 Then
            retVal = PMEReturnCode.PMTrue
        End If

        Return retVal

    End Function

    Private Function GetInstalmentGroup(ByRef groupId As Integer) As PMEReturnCode

        'Dim totalAmount As Decimal = Util.ToSafeDecimal(GetAttribute("amount"), 0)

        Dim returnCode As PMEReturnCode
        returnCode = GetInstalmentGroupForPlanRefAndAmount(groupId)

        If returnCode = PMEReturnCode.PMFalse Then

            Dim planReference As String = CStr(GetAttribute("plan_reference"))

            CreateTaskInstance(planReference + " Instalment failed. Instalments not recognised.")

        End If

        Return returnCode

    End Function

    Private Function GetInstalmentGroupForPlanRefAndAmount(ByRef groupId As Integer) As PMEReturnCode

        Dim returnCode As PMEReturnCode = PMEReturnCode.PMFalse

        Dim amount As Decimal = Util.ToSafeDecimal(GetAttribute("amount"), 0)

        AddParameterLite(m_oDatabase, "autogeneratedplanref", Me.PlanReference, PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)
        AddParameterLite(m_oDatabase, "amount", amount, PMEParameterDirection.PMParamInput, PMEDataType.PMCurrency)
        AddParameterLite(m_oDatabase, "group_id", 0, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)

        Dim dbreturnCode As Integer

        ' Execute sql
        dbreturnCode = m_oDatabase.SQLAction("spu_ACT_Import_Get_Instalment_Group", "spu_ACT_Import_Get_Instalment_Group", True)
        If dbreturnCode <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'spu_ACT_Import_Get_Instalment_Group'")
        End If

        groupId = Util.ToSafeInt(m_oDatabase.Parameters.Item("group_id").Value, 0)

        If groupId <> 0 Then
            returnCode = PMEReturnCode.PMTrue
        End If

        Return returnCode

    End Function

    Private Function ValidateInstalmentsGroup(ByVal groupId As Integer) As PMEReturnCode

        Dim returnCode As PMEReturnCode = PMEReturnCode.PMFalse

        If groupId = 0 Then

            ' if there is no valid group id to start with
            CreateTaskInstance(Me.PlanReference + " - No instalment_group_id specified.")

        Else

            Dim countOfvalidInstalmentStatusesWithinGroup As Integer = 0

            ' confirm group id exists
            GetInstalmentPlanDetailsForGroupId(groupId, countOfvalidInstalmentStatusesWithinGroup)

            If groupId = 0 Then
                ' if the group id hasnt been found
                CreateTaskInstance(Me.PlanReference + " - Instalment failed. Instalment Group not recognised.")
            ElseIf countOfvalidInstalmentStatusesWithinGroup = 0 Then
                ' if the number of invalid instalment statuses within group 
                CreateTaskInstance(Me.PlanReference + " - Instalment failed. Client Instalment is not posted or pending.")
            Else
                ' return success
                returnCode = PMEReturnCode.PMTrue
            End If

        End If

        Return returnCode

    End Function

    Private Function IsPolicyPaid(ByVal insuranceFileCnt As Integer) As Boolean

        Const OutstandingAmount As Integer = 1

        Dim returnValue As Boolean = False
        Dim results(,) As Object = Nothing
        Dim resultArray As Object = Nothing

        AddParameterLite(m_oDatabase, "insurance_file_cnt", 0, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)

        Dim dbreturnCode As Integer
        dbreturnCode = m_oDatabase.SQLSelect(sSQL:="spu_ACT_Is_Policy_Paid", sSQLName:="spu_ACT_Is_Policy_Paid", bStoredProcedure:=True, vResultArray:=resultArray, bKeepNulls:=True)
        If dbreturnCode <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'spu_ACT_Is_Policy_Paid'")
        End If

        ' cast the result
        If resultArray IsNot Nothing Then
            results = DirectCast(resultArray, Object(,))
        End If

        ' Check outstanding amount
        If Util.ToSafeInt(results(OutstandingAmount, 0), 0) = 0 Then
            returnValue = True
        End If

        Return returnValue

    End Function

    Private Sub CreateInstalmentNotification(ByVal notificationCode As String, ByVal MediaTypeId As Integer)

        AddParameterLite(m_oDatabase, "pfprem_finance_cnt", Me.PlanId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
        AddParameterLite(m_oDatabase, "pfprem_finance_version", Me.PlanVersion, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
        AddParameterLite(m_oDatabase, "pfinstalments_transaction_code", notificationCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        AddParameterLite(m_oDatabase, "pfmedia_history_id", MediaTypeId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)

        Dim dbreturnCode As Integer
        dbreturnCode = m_oDatabase.SQLAction(sSQL:="spu_ACT_Import_Create_Instalment_Notification", sSQLName:="spu_ACT_Import_Create_Instalment_Notification", bStoredProcedure:=True)
        If dbreturnCode <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'spu_ACT_Import_Create_Instalment_Notification'")
        End If

    End Sub

    Private Function GetMediatypeHistoryId(ByRef iMediaHistoryPrev As Integer,
                                    ByRef iMediaHistoryCurrent As Integer) As PMEReturnCode


        Dim returnCode As PMEReturnCode = PMEReturnCode.PMFalse

        AddParameterLite(m_oDatabase, "pfprem_finance_cnt", Me.PlanId, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, True)
        AddParameterLite(m_oDatabase, "pfprem_finance_version", Me.PlanVersion, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
        AddParameterLite(m_oDatabase, "media_history_prev_id", 0, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)
        AddParameterLite(m_oDatabase, "media_history_curr_id", 0, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)

        Dim dbreturnCode As Integer

        ' Execute sql
        dbreturnCode = m_oDatabase.SQLAction("spu_get_mediahistory_id", "spu_get_mediahistory_id", True)
        If dbreturnCode <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'spu_get_mediahistory_id'")
        End If

        iMediaHistoryPrev = Util.ToSafeInt(m_oDatabase.Parameters.Item("media_history_prev_id").Value, 0)
        iMediaHistoryCurrent = Util.ToSafeInt(m_oDatabase.Parameters.Item("media_history_curr_id").Value, 0)

        If iMediaHistoryCurrent <> 0 Then
            returnCode = PMEReturnCode.PMTrue
        End If

        Return returnCode

    End Function
    Private Sub AddMediaTypeHistory()

        Dim nReturnCode As Integer

        ' default action code to amendment
        Dim sActionCode As String = CStr(GetAttribute("rejection_code"))

        ' update premium finance plan
        AddParameterLite(m_oDatabase, "pfprem_finance_cnt", Me.PlanId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
        AddParameterLite(m_oDatabase, "pfprem_finance_version", Me.PlanVersion, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
        AddParameterLite(m_oDatabase, "action_code", sActionCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)

        ' Execute sql
        nReturnCode = m_oDatabase.SQLAction("spu_PFMediaTypeHistory_add", "spu_PFMediaTypeHistory_add", True)
        If nReturnCode <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'spu_PFMediaTypeHistory_add'")
        End If

    End Sub
    ''' <summary>
    ''' Update batch Status
    ''' </summary>
    Protected Overrides Sub UpdateBatchStatus()
        UpdateImportBatchStatus(kBatchStatusComplete, NoOfTotalRecords, NoOfRejections)
    End Sub
#End Region
    Public Function SetupCreditControlDetailsForCancelledSupersededPlan(ByVal v_nInsurance_file_cnt As Integer) As Integer

        Dim nResult As Integer = 0
        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue


            ' update premium finance plan
            AddParameterLite(m_oDatabase, "insurance_file_cnt", v_nInsurance_file_cnt, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
            AddParameterLite(m_oDatabase, "business_type", "IMPORT", PMEParameterDirection.PMParamInput, PMEDataType.PMString)

            ' Execute sql 
            nResult = m_oDatabase.SQLAction(sSQL:="spu_ACT_Add_Credit_Control_Item_InsFile", sSQLName:="spu_ACT_Add_Credit_Control_Item_InsFile", bStoredProcedure:=True)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception("spu_ACT_Add_Credit_Control_Item_InsFile failed")
            End If

        Catch ex As Exception
            nResult = 0
            Throw New Exception("spu_ACT_Add_Credit_Control_Item_InsFile failed")
        End Try

        Return nResult

    End Function
End Class
