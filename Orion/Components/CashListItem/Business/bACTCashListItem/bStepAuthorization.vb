Option Strict Off
Option Explicit On
'developer guide no.129
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("StepAuthorization_NET.StepAuthorization")>
Public NotInheritable Class StepAuthorization
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: StepAuthorization
    '
    ' Date: Nov 24, 2003
    '
    ' Description: Creatable class which contains all the methods,
    '              business rules required to manipulate AuthorisePayments.
    '
    ' ***************************************************************** '

    ' ************************************************
    ' Added to replace global variables 03/04/2007
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "StepAuthorization"

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private lPMAuthorityLevel As Integer
    Private m_sUnderwritingOrAgency As String = ""
    Private m_bAuthorisePayments As Boolean

    ' PM Lookup Business Component (Private)
    Private m_oLookup As BPMLOOKUP.Business

    Private m_lPaymentType As Integer
    Private m_lPaymentID As Integer
    Private m_cPaymentAmount As Decimal
    Private m_lPaymentCreatorUserID As Integer
    Private m_sApprovalGroup As String = ""
    Private m_lApprovalGroup As Integer
    Private m_sProcessErrorMessage As String = ""
    'RKS PN14253
    Private m_sErrorMessage As String = ""
    Private m_bLastStep As Boolean

    Private Const ACClaimPaymentsType As Integer = 1
    Private Const ACPaymentsType As Integer = 2

    'Limit Array Structure
    Private Const ACHasPaymentAuthority As Integer = 0
    Private Const ACPaymentLimit As Integer = 1
    Private Const ACHasClaimPaymentAuthority As Integer = 2
    Private Const ACClaimPaymentLimit As Integer = 3
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property
    Public ReadOnly Property UnderwritingOrAgency() As String
        Get
            If m_sUnderwritingOrAgency = "" Then
                m_lReturn = getUnderwritingOrAgency()
            End If
            Return m_sUnderwritingOrAgency
        End Get
    End Property
    Public Property PaymentType() As Integer
        Get
            Return m_lPaymentType
        End Get
        Set(ByVal Value As Integer)
            m_lPaymentType = Value
        End Set
    End Property
    Public Property PaymentAmount() As Decimal
        Get
            Return m_cPaymentAmount
        End Get
        Set(ByVal Value As Decimal)
            m_cPaymentAmount = Value
        End Set
    End Property
    Public Property PaymentID() As Integer
        Get
            Return m_lPaymentID
        End Get
        Set(ByVal Value As Integer)
            m_lPaymentID = Value
        End Set
    End Property
    Public Property PaymentCreatorUserID() As Integer
        Get
            Return m_lPaymentCreatorUserID
        End Get
        Set(ByVal Value As Integer)
            m_lPaymentCreatorUserID = Value
        End Set
    End Property
    Public Property ProcessErrorMessage() As String
        Get
            Return m_sProcessErrorMessage
        End Get
        Set(ByVal Value As String)
            m_sProcessErrorMessage = Value
        End Set
    End Property
    'RKS PN14253
    'RKS PN14253
    Public Property ErrorMessage() As String
        Get
            Return m_sErrorMessage
        End Get
        Set(ByVal Value As String)
            m_sErrorMessage = Value
        End Set
    End Property
    Public Property LastStep() As Boolean
        Get
            Return m_bLastStep
        End Get
        Set(ByVal Value As Boolean)
            m_bLastStep = Value
        End Set
    End Property

    Public Property CashListSourceID() As Integer

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUserName
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            ' Initialisation Code.

            ' Set Username and Password

            ' Set UserID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level


            m_lReturn = CType(gPMComponentServices.CheckDatabase(v_sUsername:=sUserName, v_iSourceID:=iSourceID, v_iLanguageID:=iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set Username and Password


            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_dtEffectiveDate = DateTime.Now


            ' Create PM Lookup Business Object
            m_oLookup = New BPMLOOKUP.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUserName, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If m_oLookup IsNot Nothing Then
                    m_oLookup.Dispose()
                    m_oLookup = Nothing
                End If
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Informations.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Informations.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Informations.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Informations.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Informations.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Public Function BeginTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Public Function CommitTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Public Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function
    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        'Exit Sub
        'End Try

    End Sub
    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Public Function getUnderwritingOrAgency() As Integer
        ' ***************************************************************** '
        ' Name: UnderwritingOrAgency
        '
        ' Description:  Finds out if Underwriting or Agency business

        ' ***************************************************************** '

        Dim result As Integer = 0
        Try


            Return bPMFunc.getUnderwritingOrAgency(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, r_vUnderwriting:=m_sUnderwritingOrAgency)

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnderwritingOrAgencyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnderwritingOrAgency", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function
    Public Function CheckUserGroup(ByRef r_bUserInGroup As Boolean) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            With m_oDatabase

                .Parameters.Clear()

                ' add username parameter
                m_lReturn = m_oDatabase.Parameters.Add(sName:="username", vValue:=m_sUsername.Trim(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckUserGroup Failed to add Parameter", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckUserGroup", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                ' add groupcode parameter
                m_lReturn = m_oDatabase.Parameters.Add(sName:="group_code", vValue:=m_sApprovalGroup, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckUserGroup Failed to add Parameter", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckUserGroup", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                ' run SP
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckUserGroupSQL, sSQLName:=ACCheckUserGroupName, bStoredProcedure:=ACCheckUserGroupStored, vResultArray:=vArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckUserGroup Failed to Run The stored proc", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckUserGroup", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                r_bUserInGroup = Informations.IsArray(vArray)

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckUserGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckUserGroup", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function
    Public Function ProcessApproval() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(ProcessStep(v_lApproved:=1), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessApproval Failed to Process", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessApproval", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessApproval Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessApproval", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function
    Private Function ProcessStep(ByVal v_lApproved As Integer) As Integer

        Dim result As Integer = 0
        Dim bGroupsExists As Boolean
        Dim lApprovalStep As Integer
        Dim vStepDetails As Object = Nothing
        Dim bUserIsUnique, bUserWithinLimits As Boolean
        Dim lNumberOfSteps As Integer
        Dim bUserInGroup As Boolean

        result = gPMConstants.PMEReturnCode.PMTrue

        'Check if steps are setup in Debtor User Group for Claim Payments Group Type
        m_lReturn = CType(CheckDebtorGroups(r_bGroupsExists:=bGroupsExists, r_lNumberOfSteps:=lNumberOfSteps), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessStep Failed CheckDebtorGroups", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessStep", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        If bGroupsExists Then
            'Now we have groups for the perticular type
            'Get the Group for the step we are on. = Number of records in New approval table + 1
            m_lReturn = CType(GetApprovalRecords(r_lApprovalStep:=lApprovalStep), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessStep Failed GetApprovalRecords", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessStep", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If
            'Are we on the last step
            If v_lApproved = 0 Then
                lApprovalStep = lNumberOfSteps
            End If
            m_bLastStep = (lApprovalStep = lNumberOfSteps)

            'Get the details for the step from Debtor Groups
            m_lReturn = CType(GetStepDetails(v_lApprovalStep:=lApprovalStep, r_vStepDetails:=vStepDetails), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessStep Failed GetStepDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessStep", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            If Informations.IsArray(vStepDetails) Then
                'Check the user belongs to the group
                m_sApprovalGroup = gPMFunctions.NullToString(vStepDetails(1, 0))
                m_lApprovalGroup = gPMFunctions.NullToLong(vStepDetails(0, 0))
                m_lReturn = CType(CheckUserGroup(r_bUserInGroup:=bUserInGroup), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    ' Log Error Message
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessStep Failed GetStepDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessStep", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                If Not bUserInGroup Then
                    m_sProcessErrorMessage = "You do not belong to the user group authorised for this step." & Strings.ChrW(13) & Strings.ChrW(10) & "Please contact your system administrator."
                    Return result
                End If

                'Check if the user is unique in the chain of approvals.
                m_lReturn = CType(IsUserUnique(r_bUserIsUnique:=bUserIsUnique, v_lApprovalStep:=lApprovalStep), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    ' Log Error Message
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessStep Failed GetStepDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessStep", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End If

            If bUserIsUnique Then
                'Check if the user has authority to approve and meet the limits.
                m_lReturn = CType(CheckUserAuthorityLimit(r_bUserWithinLimits:=bUserWithinLimits), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    ' Log Error Message
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessStep Failed CheckUserAuthorityLimit", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessStep", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                If bUserWithinLimits Then
                    'Create a record in the new approval table.
                    m_lReturn = CType(CreateApprovalRecord(v_lApproved:=v_lApproved), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn
                        ' Log Error Message
                        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessStep Failed CheckUserAuthorityLimit", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessStep", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If
                Else
                    m_sProcessErrorMessage = "You are not within your authorization limit for this payment type." & Strings.ChrW(13) & Strings.ChrW(10) & "Please contact your system administrator."
                End If
            Else
                m_sProcessErrorMessage = "Cannot Proceed- You are either the person who entered this payment or you have authorised a previous step." & Strings.ChrW(13) & Strings.ChrW(10) & "You cannot authorise two steps on the same payment."
            End If
        Else
            m_sProcessErrorMessage = "Debtor User Groups are not setup." & Strings.ChrW(13) & Strings.ChrW(10) & " Please contact your system administrator"
        End If

        Return result


        result = gPMConstants.PMEReturnCode.PMError
        ' Log Error Message
        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessStep Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessStep", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
        Return result
    End Function


    Public Function ProcessDecline() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(ProcessStep(v_lApproved:=0), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessDecline Failed to Process", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessDecline", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessDecline Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessDecline", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Private Function CheckDebtorGroups(ByRef r_bGroupsExists As Boolean, ByRef r_lNumberOfSteps As Integer) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Dim sGroupType As String = ""

        If m_lPaymentType = ACClaimPaymentsType Then
            sGroupType = "Claim Payments"
        ElseIf m_lPaymentType = ACPaymentsType Then
            sGroupType = "Payments"
        End If

        With m_oDatabase
            'Clear Parameters
            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="GroupType", vValue:=sGroupType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckDebtorGroups Failed to add Parameter", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckDebtorGroups", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Add SourceID parameter only once
            m_lReturn = .Parameters.Add(sName:="SourceID", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckDebtorGroups Failed to add Parameter", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckDebtorGroups", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            m_lReturn = .SQLSelect(sSQL:=ACGetDebtorGroupsSQL, sSQLName:=ACGetDebtorGroupsName, bStoredProcedure:=ACGetDebtorGroupsStored, lNumberRecords:=r_lNumberOfSteps)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckDebtorGroups Failed to run the stored proc", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckDebtorGroups", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            r_bGroupsExists = (r_lNumberOfSteps > 0)
        End With

        Return result
    End Function



    Public Function GetStepGroupCode(ByRef r_sGroupCode As String) As Integer
        Return GetStepGroupCode(r_sGroupCode:=r_sGroupCode, r_sErrorMessage:="")
    End Function
    Public Function GetStepGroupCode(ByRef r_sGroupCode As String, ByRef r_sErrorMessage As String, Optional ByVal IsViaBulkClaimPayment As Boolean = False) As Integer

        Dim result As Integer = 0
        Try

            Dim bGroupsExists As Boolean
            Dim lNumberOfSteps, lApprovalStep As Integer
            Dim vStepDetails As Object = Nothing

            result = gPMConstants.PMEReturnCode.PMTrue

            'RKS PN14253
            m_sErrorMessage = ""

            'Check if steps are setup in Debtor User Group for Claim Payments Group Type
            m_lReturn = CType(CheckDebtorGroups(r_bGroupsExists:=bGroupsExists, r_lNumberOfSteps:=lNumberOfSteps), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetStepGroupCode Failed CheckDebtorGroups", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStepGroupCode", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            If bGroupsExists Then
                'Now we have groups for the perticular type
                'Get the Group for the step we are on. = Number of records in New approval table + 1
                m_lReturn = CType(GetApprovalRecords(r_lApprovalStep:=lApprovalStep), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    ' Log Error Message
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetStepGroupCode Failed GetApprovalRecords", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStepGroupCode", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If


                'Get the details for the step from Debtor Groups
                m_lReturn = CType(GetStepDetails(v_lApprovalStep:=lApprovalStep, r_vStepDetails:=vStepDetails, IsViaBulkClaimPayment:=IsViaBulkClaimPayment), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    ' Log Error Message
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetStepGroupCode Failed GetStepDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStepGroupCode", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                If Informations.IsArray(vStepDetails) Then
                    'Check the user belongs to the group
                    r_sGroupCode = gPMFunctions.NullToString(vStepDetails(1, 0))
                Else
                    r_sGroupCode = ""
                    m_sErrorMessage = "Debtor User Groups are not setup." & Strings.ChrW(13) & Strings.ChrW(10) & "Please contact your system administrator"
                    r_sErrorMessage = m_sErrorMessage
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                r_sGroupCode = ""
                'RKS PN14253
                'if User Groups are not exist
                m_sErrorMessage = "Debtor User Groups are not setup." & Strings.ChrW(13) & Strings.ChrW(10) & "Please contact your system administrator"
                r_sErrorMessage = m_sErrorMessage
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetStepGroupCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStepGroupCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function



    Private Function GetApprovalRecords(ByRef r_lApprovalStep As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase
            'Clear Parameters
            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="PaymentType", vValue:=CStr(m_lPaymentType), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetApprovalRecords Failed to add Parameter", vApp:=ACApp, vClass:=ACClass, vMethod:="GetApprovalRecords", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            m_lReturn = .Parameters.Add(sName:="PaymentID", vValue:=CStr(m_lPaymentID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetApprovalRecords Failed to add Parameter", vApp:=ACApp, vClass:=ACClass, vMethod:="GetApprovalRecords", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            m_lReturn = .SQLSelect(sSQL:=ACGetApprovalRecordsSQL, sSQLName:=ACGetApprovalRecordsName, bStoredProcedure:=ACGetApprovalRecordsStored, lNumberRecords:=r_lApprovalStep)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetApprovalRecords Failed to run the stored proc", vApp:=ACApp, vClass:=ACClass, vMethod:="GetApprovalRecords", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'Add one to the # of records
            r_lApprovalStep += 1

        End With

        Return result

    End Function
    Public Function GetStepDetails(ByVal v_lApprovalStep As Integer, ByRef r_vStepDetails(,) As Object, Optional ByVal IsViaBulkClaimPayment As Boolean = False) As Integer

        Dim result As Integer = 0
        Try

            Dim sGroupType As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_lPaymentType = ACClaimPaymentsType Then
                sGroupType = "Claim Payments"
            ElseIf m_lPaymentType = ACPaymentsType Then
                sGroupType = "Payments"
            End If

            With m_oDatabase
                'Clear Parameters
                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="GroupType", vValue:=sGroupType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetStepDetails Failed to add Parameter", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStepDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                m_lReturn = .Parameters.Add(sName:="SourceID", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetStepDetails Failed to add Parameter", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStepDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                m_lReturn = .Parameters.Add(sName:="StepNumber", vValue:=CStr(v_lApprovalStep), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetStepDetails Failed to add Parameter", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStepDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                If IsViaBulkClaimPayment Then
                    m_lReturn = .Parameters.Add(sName:="IsViaBulkClaimPayment", vValue:=CStr(IsViaBulkClaimPayment), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn
                        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetStepDetails Failed to add Parameter", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStepDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    End If
                End If
                
                m_lReturn = .SQLSelect(sSQL:=ACApprovalStepDetailsSQL, sSQLName:=ACApprovalStepDetailsName, bStoredProcedure:=ACApprovalStepDetailsStored, vResultArray:=r_vStepDetails)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetStepDetails Failed to run the stored proc", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStepDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End With

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetStepDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStepDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function
    Private Function IsUserUnique(ByRef r_bUserIsUnique As Boolean, ByVal v_lApprovalStep As Integer) As Integer

        Dim result As Integer = 0


        Dim lNumberReocrds As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        If v_lApprovalStep = 1 Then
            If m_lPaymentCreatorUserID = m_iUserID Then
                r_bUserIsUnique = False
                Return result
            End If
        End If

        With m_oDatabase
            'Clear Parameters
            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="UserID", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsUserUnique Failed to add Parameter", vApp:=ACApp, vClass:=ACClass, vMethod:="IsUserUnique", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            m_lReturn = .Parameters.Add(sName:="PaymentID", vValue:=CStr(m_lPaymentID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsUserUnique Failed to add Parameter", vApp:=ACApp, vClass:=ACClass, vMethod:="IsUserUnique", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            m_lReturn = .SQLSelect(sSQL:=ACIsUserUniqueSQL, sSQLName:=ACIsUserUniqueName, bStoredProcedure:=ACIsUserUniqueStored, lNumberRecords:=lNumberReocrds)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsUserUnique Failed to run the stored proc", vApp:=ACApp, vClass:=ACClass, vMethod:="IsUserUnique", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            r_bUserIsUnique = (lNumberReocrds = 0)

        End With

        Return result

    End Function

    Private Function CheckUserAuthorityLimit(ByRef r_bUserWithinLimits As Boolean) As Integer

        Dim result As Integer = 0


        Dim vAuthorityLimit(,) As Object = Nothing
        Dim sGroupType As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase
            'Clear Parameters
            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="UserID", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckUserAuthorityLimit Failed to add Parameter", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckUserAuthorityLimit", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            m_lReturn = .SQLSelect(sSQL:=ACUserLimitsSQL, sSQLName:=ACUserLimitsName, bStoredProcedure:=ACUserLimitsStored, vResultArray:=vAuthorityLimit, bKeepNulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckUserAuthorityLimit Failed to run the stored proc", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckUserAuthorityLimit", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

        End With
        If Informations.IsArray(vAuthorityLimit) Then

            Select Case m_lPaymentType
                Case ACClaimPaymentsType
                    If gPMFunctions.NullToBoolean(vAuthorityLimit(ACHasClaimPaymentAuthority, 0)) Then
                        r_bUserWithinLimits = gPMFunctions.NullToCurrency(vAuthorityLimit(ACClaimPaymentLimit, 0)) >= m_cPaymentAmount
                    Else
                        r_bUserWithinLimits = False
                    End If

                Case ACPaymentsType
                    If gPMFunctions.NullToBoolean(vAuthorityLimit(ACHasPaymentAuthority, 0)) Then
                        r_bUserWithinLimits = gPMFunctions.NullToCurrency(vAuthorityLimit(ACPaymentLimit, 0)) >= m_cPaymentAmount
                    Else
                        r_bUserWithinLimits = False
                    End If

            End Select
        Else
            r_bUserWithinLimits = False
        End If

        Return result

    End Function
    Private Function CreateApprovalRecord(ByVal v_lApproved As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase
            'Clear Parameters
            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="PaymentType", vValue:=CStr(m_lPaymentType), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateApprovalRecord Failed to add Parameter", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateApprovalRecord", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            m_lReturn = .Parameters.Add(sName:="PaymentCnt", vValue:=CStr(m_lPaymentID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateApprovalRecord Failed to add Parameter", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateApprovalRecord", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            m_lReturn = .Parameters.Add(sName:="Payment_Amount", vValue:=CStr(m_cPaymentAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateApprovalRecord Failed to add Parameter", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateApprovalRecord", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            m_lReturn = .Parameters.Add(sName:="ApprovalGroup", vValue:=CStr(m_lApprovalGroup), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateApprovalRecord Failed to add Parameter", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateApprovalRecord", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            m_lReturn = .Parameters.Add(sName:="UserID", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateApprovalRecord Failed to add Parameter", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateApprovalRecord", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            m_lReturn = .Parameters.Add(sName:="Approved", vValue:=CStr(v_lApproved), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateApprovalRecord Failed to add Parameter", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateApprovalRecord", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If
            m_lReturn = .SQLSelect(sSQL:=ACAddPaymentApprovalRecordSQL, sSQLName:=ACAddPaymentApprovalRecordName, bStoredProcedure:=ACAddPaymentApprovalRecordStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateApprovalRecord Failed to run the stored proc", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateApprovalRecord", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If
        End With

        Return result

    End Function
    Public Function CheckPaymentStepStatus(ByRef nApproved As Integer) As Integer
        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim bGroupsExists As Boolean
        Dim nApprovalStep As Integer
        Dim nNumberOfSteps As Integer

        Try
            m_lReturn = CType(CheckDebtorGroups(r_bGroupsExists:=bGroupsExists, r_lNumberOfSteps:=nNumberOfSteps), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckPaymentStepStatus Failed CheckDebtorGroups", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckPaymentStepStatus", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return m_lReturn
            End If

            If bGroupsExists Then
                m_lReturn = CType(GetApprovalRecords(r_lApprovalStep:=nApprovalStep), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckPaymentStepStatus Failed GetApprovalRecords", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckPaymentStepStatus", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return m_lReturn
                End If
                m_bLastStep = (nApprovalStep > nNumberOfSteps)

                nApproved = m_bLastStep
            Else
                m_sProcessErrorMessage = "Debtor User Groups are not setup." & Strings.ChrW(13) & Strings.ChrW(10) & " Please contact your system administrator"
            End If

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckPaymentStepStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckPaymentStepStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep)
            Return nResult
        End Try
    End Function

    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
