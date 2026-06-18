Option Strict Off
Option Explicit On
'developer guide no.129
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Automated_NET.Automated")>
Public NotInheritable Class Automated
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 13/05/1997
    '
    ' Description: Creatable Form class which contains all the
    '              methods, business rules required for the
    '              AllocationPost Form.
    '
    ' Edit History:
    ' RAW 12/03/2003 : ISS2893 : coorected handling of Reverse Allocations
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
    Private Const ACClass As String = "Automated"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Process Mode Properties
    ' Task
    Private m_iTask As Integer
    ' Navigate
    Private m_lNavigate As Integer
    ' Process Mode
    Private m_lProcessMode As Integer
    ' Type of Business
    Private m_sTransactionType As String = ""
    ' Effective
    Private m_dtEffectiveDate As Date

    ' Status members
    Private m_sProcessStatus As New StringsHelper.FixedLengthString(2)
    Private m_sMapStatus As New StringsHelper.FixedLengthString(2)
    Private m_sStepStatus As New StringsHelper.FixedLengthString(2)

    Private m_vAllocationId As Object
    Private m_vAllocationIDs As Object

    Private m_bAbortTrans As Boolean

    Private m_oDocumentPost As bACTDocumentPost.Form
    Private m_oAllocationDetail As bACTAllocationDetail.Form
    Private m_oAllocation As bACTAllocation.Form
    Private m_oMatchPost As bACTMatchPost.Form
    Private m_oTransDetail As bACTTransDetail.Form

    ' CF200199
    Private m_lPMAuthorityLevel As Integer
    Private m_lStatus As gPMConstants.PMEReturnCode

    Public Property CallingAppName() As String
        Get
            Return m_sCallingAppName
        End Get
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property

    Public Property PMAuthorityLevel() As Integer
        Get
            Return m_lPMAuthorityLevel
        End Get
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
        End Get
    End Property

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property Task() As Integer
        Get

            Return m_iTask

        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get

            Return m_lNavigate

        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get

            Return m_lProcessMode

        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get

            Return m_sTransactionType

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
    End Property

    Public ReadOnly Property StepStatus() As String
        Get

            ' Standard Property.

            ' Return the Steps Status
            Return m_sStepStatus.Value

        End Get
    End Property

    Public Property AbortTrans() As Boolean
        Get

            Return m_bAbortTrans

        End Get
        Set(ByVal Value As Boolean)

            m_bAbortTrans = Value

        End Set
    End Property

    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long


        ' Server Component Services

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
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

            ' RAW 12/03/2003 : ISS2893 : added
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel


            m_lReturn = gPMComponentServices.CheckDatabase(v_sUsername:="", v_iSourceID:=0, v_iLanguageID:=0, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'm_oDatabase.SQLBeginTrans

            m_bAbortTrans = True

            'Set m_oAllocation = GetOrionBusiness(v_sClassName:="bACTAllocation.Form", v_vDatabase:=m_oDatabase)
            '
            ' If m_oAllocation Is Nothing Then
            '   Initialise = PMFalse
            '   Exit Function
            ' End If


            m_oAllocation = New bACTAllocation.Form
            m_lReturn = m_oAllocation.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set m_oAllocationDetail = GetOrionBusiness(v_sClassName:="bACTAllocationDetail.Form", v_vDatabase:=m_oDatabase)
            '
            'If m_oAllocationDetail Is Nothing Then
            '  Initialise = PMFalse
            '  Exit Function
            'End If



            m_oAllocationDetail = New bACTAllocationDetail.Form
            m_lReturn = m_oAllocationDetail.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set m_oDocumentPost = GetOrionBusiness(v_sClassName:="bACTDocumentPost.Form", v_vDatabase:=m_oDatabase)
            '
            'If m_oDocumentPost Is Nothing Then
            '  Initialise = PMFalse
            '  Exit Function
            'End If



            m_oDocumentPost = New bACTDocumentPost.Form
            m_lReturn = m_oDocumentPost.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set m_oMatchPost = GetOrionBusiness(v_sClassName:="bACTMatchPost.Form", v_vDatabase:=m_oDatabase)
            '
            'If m_oMatchPost Is Nothing Then
            '  Initialise = PMFalse
            '  Exit Function
            'End If



            m_oMatchPost = New bACTMatchPost.Form
            m_lReturn = m_oMatchPost.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set m_oTransDetail = GetOrionBusiness(v_sClassName:="bACTTransDetail.Form", v_vDatabase:=m_oDatabase)
            '
            'If m_oTransDetail Is Nothing Then
            '  Initialise = PMFalse
            '  Exit Function
            'End If



            m_oTransDetail = New bACTTransDetail.Form
            m_lReturn = m_oTransDetail.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Remove component services

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Set the ProcessMode to Generic
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric

            ' Set the Type Of Business to New Business
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric

            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
                If m_oTransDetail IsNot Nothing Then
                    m_oTransDetail.Dispose()
                    m_oTransDetail = Nothing
                End If
                If m_oAllocation IsNot Nothing Then
                    m_oAllocation.Dispose()
                    m_oAllocation = Nothing
                End If
                If m_oAllocationDetail IsNot Nothing Then
                    m_oAllocationDetail.Dispose()
                    m_oAllocationDetail = Nothing
                End If
                If m_oDocumentPost IsNot Nothing Then
                    m_oDocumentPost.Dispose()
                    m_oDocumentPost = Nothing
                End If
                If m_oMatchPost IsNot Nothing Then
                    m_oMatchPost.Dispose()
                    m_oMatchPost = Nothing
                End If

            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetSummary
    '
    ' Description: Returns the summary information to Navigator
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Dont return anything

            vSummaryArray = Nothing

            ' If theres no allocation_id then tell the user that no
            ' allocations were posted

            If Not Informations.IsArray(m_vAllocationIDs) Then


                If Convert.IsDBNull(m_vAllocationId) Or Informations.IsNothing(m_vAllocationId) Then

                    ReDim vSummaryArray(2, 0)


                    vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummLevel, 0) = gPMConstants.PMENavSummaryLevel.PMNavSummMapSummary

                    vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummHeading, 0) = "Post Allocation"

                    vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, 0) = "There were no allocations to post."

                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummaryFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PostAllocation (Public)
    '
    ' Description: Creates a document for allocation id supplied
    '              Creates matches and loss/gain & write offs
    '
    ' CF 210199 - Changed documentref to autonumber
    '
    ' ***************************************************************** '
    Public Function PostAllocation(ByVal v_vAllocationID As Object) As Integer

        Dim result As Integer = 0
        Dim dtAccountingDate As Date
        Dim lMatchId As Integer
        Dim cTotalWriteOff, cTotalLossGain As Decimal
        Dim lDocSequence As Integer
        Dim iCompanyID As Integer
        Dim lSubBranchID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Load up the collections with the Allocations

            m_lReturn = GetAllocationDetails(v_lAllocationID:=CInt(v_vAllocationID))
            'eck030701
            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFail
            End If


            dtAccountingDate = m_oAllocation.Details.Item(1).AllocationDate

            ' Initialise total write off accum
            cTotalWriteOff = 0
            lDocSequence = 0
            cTotalLossGain = 0

            ' Match group header
            'eck100500

            iCompanyID = m_oAllocation.Details.Item(1).CompanyID

            'DD 05/08/2002: Multi-branch

            m_lReturn = bACTFunc.GetSubBranchID(v_oDatabase:=m_oDatabase, r_lSubBranchID:=lSubBranchID, v_vAccountID:=m_oAllocation.Details.Item(1).AccountID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFail
            End If

            'DD 05/08/2002: Added parameter for sub-branch

            m_lReturn = m_oMatchPost.AddMatchGroup(v_dtMatchDate:=dtAccountingDate, r_vMatchId:=lMatchId, r_vMatchSourceId:=iCompanyID, v_lSubBranchID:=lSubBranchID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFail
            End If


            For lItem As Integer = 1 To m_oAllocationDetail.Details.Count


                With m_oAllocationDetail.Details.Item(lItem)

                    'RKS160904 PN14826 - Insurer Payment Transaction Lost (Part Payment)
                    'This issue occurs due to "outstanding_currency_amount" set to 0 in
                    'case of part payment (Insurer Payment). This is because of passing
                    'incorrect value to v_cCurrencyMatchAmount, no need of adding the
                    'oustanding amount (.OsCCyAmount) instead tehre must be a equivalent
                    'of .WriteOffAmount in Transaction Currency.

                    'Changed from v_cCurrencyMatchAmount:=.AllocCCyAmount + .OsCCyAmount
                    'to v_cCurrencyMatchAmount:=.AllocCCyAmount


                    m_lReturn = m_oMatchPost.AddMatchTrans(v_lAllocationDetailID:= .AllocationdetailID, v_lTransDetailID:= .TransdetailID, v_iCurrencyID:= .OriginalCurrency, v_cBaseMatchAmount:= .AllocBaseAmount + .WriteOffAmount, v_cCurrencyMatchAmount:= .AllocCCyAmount, v_vdCurrencyMatchXRate:= .EffectiveXrate)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFail
                    End If

                End With

            Next lItem


            ' End by writing the Allocation updates back

            m_lReturn = m_oAllocation.EditUpdate(1, vAllocationstatusID:=gACTLibrary.ACTAllocationStatusAllocated)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFail
            End If


            m_lReturn = m_oAllocation.Update

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFail
            End If


            m_lReturn = m_oAllocationDetail.Update

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFail
            End If

            ' and signal to DocumentPost that its ok to update


            m_lReturn = m_oDocumentPost.Commit

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFail
            End If

            ' signal to MatchPost that its ok to update


            m_lReturn = m_oMatchPost.Commit

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFail
            End If

            'Now if any of the transactions we've allocated have been
            'fully matched then update it


            For lItem As Integer = 1 To m_oAllocationDetail.Details.Count


                With m_oAllocationDetail.Details.Item(lItem)


                    m_lReturn = GetTransDetail(v_lTransDetailID:= .TransdetailID)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFail
                    End If

                End With

            Next lItem

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostAllocation", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: SetKeys (Standard Method)
    '
    ' Description: Stores all of the parameter members with the key
    '              array.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sTmp As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_vAllocationId = Nothing

            ' Check we have a vaild array.
            If Not Informations.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the
                ' correct key array item.


                If Not (vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow) Is Nothing) Then
                    Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                        Case PMNavKeyConst.ACTKeyNameAllocationId

                            m_vAllocationId = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)

                            ' CF200499
                        Case PMNavKeyConst.ACTKeyNameAllocationIDs

                            sTmp = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                            m_lReturn = gACTLibrary.ParseArray(vArray:=m_vAllocationIDs, sString:=sTmp, bArrayToString:=False)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                    End Select
                End If

            Next lRow

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys (Standard Method)
    '
    ' Description: Stores all of the key array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the key array with the number of
            ' keys needed to be returned.
            ' Note: Remember arrays are zero based.
            ReDim vKeyArray(1, 0)

            ' Assign the key array with the parameter members.
            '    vkeyarray(PMKeyName, 0) = ACTKeyNameCashListTypeId
            '    vkeyarray(PMKeyValue, 0) = m_lCashListTypeID&

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Start (Public)
    '
    ' Description: Performs the Automated Action dependant on the Task
    '              Process Mode etc.
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0

        Try

            m_oDatabase.SQLBeginTrans()

            ' If we have multiple id's then post them all
            If Informations.IsArray(m_vAllocationIDs) Then


                For iLoop1 As Integer = m_vAllocationIDs.GetLowerBound(0) To m_vAllocationIDs.GetUpperBound(0)
                    m_lReturn = PostAllocation(v_vAllocationID:=m_vAllocationIDs(iLoop1))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' RAW 12/03/2003 : ISS2893 : added
                        m_lStatus = gPMConstants.PMEReturnCode.PMCancel ' force navigator to stop
                        ' Log Error Message

                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to post allocation :" & CStr(m_vAllocationIDs(iLoop1)), vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        m_oDatabase.SQLRollbackTrans()
                        'eck30701
                        '                Start = PMFalse
                        result = m_lReturn
                    Else
                        ' RAW 12/03/2003 : ISS2893 : added
                        m_lStatus = gPMConstants.PMEReturnCode.PMOK

                        m_oDatabase.SQLCommitTrans()
                        'EK 090200
                        m_oDatabase.SQLCommitTrans()
                        result = gPMConstants.PMEReturnCode.PMTrue
                    End If

                Next iLoop1

            Else

                ' CF300499 - If you dont allocate any cash list items
                '            then you wont have an allocation_id
                ' SP290999 - Tweak logic so will not fall over if have null array

                If Not (Convert.IsDBNull(m_vAllocationId) Or Informations.IsNothing(m_vAllocationId)) Then

                    If m_vAllocationId <> 0 Then

                        ' Otherwise just post the one
                        m_lReturn = PostAllocation(v_vAllocationID:=m_vAllocationId)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                            ' RAW 12/03/2003 : ISS2893 : added
                            m_lStatus = gPMConstants.PMEReturnCode.PMCancel ' force navigator to stop
                            ' Log Error Message
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to post allocation :" & m_vAllocationId, vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                            m_oDatabase.SQLRollbackTrans()
                            result = gPMConstants.PMEReturnCode.PMFalse

                        Else
                            ' RAW 12/03/2003 : ISS2893 : added
                            m_lStatus = gPMConstants.PMEReturnCode.PMOK

                            m_oDatabase.SQLCommitTrans()
                            'EK 090200
                            m_oDatabase.SQLCommitTrans()
                            result = gPMConstants.PMEReturnCode.PMTrue
                        End If
                    Else
                        ' RAW 12/03/2003 : ISS2893 : added
                        m_lStatus = gPMConstants.PMEReturnCode.PMOK
                        result = gPMConstants.PMEReturnCode.PMTrue
                    End If
                Else
                    ' RAW 12/03/2003 : ISS2893 : added
                    m_lStatus = gPMConstants.PMEReturnCode.PMOK
                    result = gPMConstants.PMEReturnCode.PMTrue
                End If
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            'SD 15/01/2003
            m_oDatabase.SQLRollbackTrans()

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetStatus (Standard Method)
    '
    ' Description: Set the Process, Map and Step status.
    ' Note:        A Property Get is provided for the Step Status only
    '              as this is the only one which this component can
    '              alter directly.
    ' ***************************************************************** '
    Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the current Status settings.
            m_sProcessStatus.Value = sProcessStatus.Trim()
            m_sMapStatus.Value = sMapStatus.Trim()
            m_sStepStatus.Value = sStepStatus.Trim()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetDebitDetails (Public)
    '
    ' Description: Get the debit allocation IDs so we can process
    '              Credit Control items
    '
    ' Written By: SMJB 13/08/03 (CQ2189)
    ' ***************************************************************** '
    Function GetDebitDetails(ByVal v_lAllocationID As Integer, ByRef r_Results(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            With m_oDatabase
                With .Parameters
                    .Clear()
                    m_lReturn = .Add(sName:="allocation_id", vValue:=CStr(v_lAllocationID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                End With

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = .SQLSelect(sSQL:=ACGetDebitAllocationIDSQL, sSQLName:=ACGetDebitAllocationIDName, bStoredProcedure:=ACGetDebitAllocationIDStored, vResultArray:=r_Results)
                End If



                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed on running SP", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDebitDetails")
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

            End With



            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get debit details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDebitDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: ReverseCommissionMovement (Public)
    '
    ' Description: If allocation being reversed generated commission movement
    '              then the commission must move back
    '              FSA Phase 3.2
    ' Written By: ECK 25/01/2005
    ' ***************************************************************** '
    Function ReverseCommissionMovement(ByVal v_lAllocationID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResults(,) As Object = Nothing


        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            With m_oDatabase
                With .Parameters
                    m_lReturn = .Add(sName:="allocation_id", vValue:=CStr(v_lAllocationID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                End With

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = .SQLSelect(sSQL:=ACGetTransdetailIDsInAllocationSQL, sSQLName:=ACGetTransdetailIDsInAllocationName, bStoredProcedure:=ACGetTransdetailIDsInAllocationStored, vResultArray:=vResults)
                End If



                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed on running SP", vApp:=ACApp, vClass:=ACClass, vMethod:="ReverseCommissionMovement")
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not Informations.IsArray(vResults) Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed on running SP", vApp:=ACApp, vClass:=ACClass, vMethod:="ReverseCommissionMovement")
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If


                For lCount As Integer = 0 To vResults.GetUpperBound(1)



                    m_lReturn = m_oTransDetail.RecallReleasedTransaction(lTriggerTransdetailId:=CInt(vResults(0, lCount)), lAllocationID:=v_lAllocationID)


                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to recall released commission", vApp:=ACApp, vClass:=ACClass, vMethod:="ReverseCommissionMovement")
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If


                Next lCount
            End With



            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get debit details", vApp:=ACApp, vClass:=ACClass, vMethod:="ReverseCommissionMovement", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function
    ' PUBLIC Methods (End)

    Public Sub New()
        MyBase.New()

        Try

            ' Class Initialise

            ' Initialise the Status settings
            m_sProcessStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sMapStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sStepStatus.Value = gPMConstants.PMNavStatusUnknown

        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    ' Load the details into the Allocation & AllocationItem Collections
    Private Function GetAllocationDetails(ByVal v_lAllocationID As Integer, Optional ByRef v_vAllocationItemId As Object = Nothing) As Integer
        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue


        m_lReturn = m_oAllocation.GetDetails(vAllocationID:=v_lAllocationID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        ' Now get the details


        m_lReturn = m_oAllocationDetail.GetDetails(vAllocationID:=v_lAllocationID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        Return result

    End Function

    ' Get the details of a particular TransDetail into the class
    Private Function GetTransDetail(ByVal v_lTransDetailID As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue


        m_lReturn = m_oTransDetail.GetDetails(vTransdetailID:=v_lTransDetailID, vOSAmounts:=True)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        With m_oTransDetail


            If (.Details.Item(1).OSBaseAmount = 0) Or (.Details.Item(1).OSCurrencyAmount = 0) Then

                m_lReturn = .EditUpdate(1, vTransdetailID:=v_lTransDetailID, vFullyMatched:=gPMConstants.PMEReturnCode.PMTrue)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFail
                End If


                m_lReturn = .Update

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFail
                End If

            End If

        End With

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: DoAllocationDetailPairsExist
    '
    ' Description: This procedure will establish whether every transaction identified by the
    '              search criteria can be matched to a corresponding credit or debit in the same allocation.
    '              This is unlikely to be possible where there are many credits allocated to many debits.
    '
    ' History:
    ' RAW 15/05/2003 : CQ954 : created
    '
    ' ***************************************************************** '
    Public Function DoAllocationDetailPairsExist(ByRef r_bDoAllocationDetailPairsExist As Boolean, Optional ByVal v_lTransDetailID As Integer = 0, Optional ByVal v_lCashListItemID As Integer = 0, Optional ByVal v_lAllocationID As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sSQLSearch As String = ""
        Dim iDoAllocationDetailPairsExist As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        r_bDoAllocationDetailPairsExist = False

        If v_lTransDetailID = 0 And v_lCashListItemID = 0 And v_lAllocationID = 0 Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="DoAllocationDetailPairsExist failed. At least one parameter must be passed in.", vApp:=ACApp, vClass:=ACClass, vMethod:="DoAllocationDetailPairsExist")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        With m_oDatabase

            With .Parameters

                .Clear()

                ' add return value
                m_lReturn = m_oDatabase.Parameters.Add(sName:="return_value", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamReturnValue, iDataType:=gPMConstants.PMEDataType.PMLong)

                If v_lAllocationID <> 0 Then

                    m_lReturn = .Add(sName:="v_iAllocationId", vValue:=CStr(v_lAllocationID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Else

                    'Developer Guide No. 85
                    m_lReturn = .Add(sName:="v_iAllocationId", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                End If

                If v_lCashListItemID <> 0 Then

                    m_lReturn = .Add(sName:="v_iCashListItemId", vValue:=CStr(v_lCashListItemID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Else

                    'Developer Guide No.85
                    m_lReturn = .Add(sName:="v_iCashListItemId", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                End If

                If v_lTransDetailID <> 0 Then

                    m_lReturn = .Add(sName:="v_iTransDetailId", vValue:=CStr(v_lTransDetailID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Else

                    'Developer Guide No. 85
                    m_lReturn = .Add(sName:="v_iTransDetailId", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                End If


                ' add output params

                m_lReturn = .Add(sName:="r_bDoAllocationDetailPairsExist", vValue:=CStr(iDoAllocationDetailPairsExist), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            End With


            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = .SQLAction(sSQL:=ACDoAllocationDetailPairsExistSQL, sSQLName:=ACDoAllocationDetailPairsExistName, bStoredProcedure:=ACDoAllocationDetailPairsExistStored)
            End If

        End With

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'SD 15/01/2003 get rid of message box
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed on running.SP", vApp:=ACApp, vClass:=ACClass, vMethod:="DoAllocationDetailPairsExist")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Check return value from sp
        If m_oDatabase.Parameters.Item("return_value").Value <> 0 Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=ACDoAllocationDetailPairsExistSQL & " returned " & m_oDatabase.Parameters.Item("return_value").Value, vApp:=ACApp, vClass:=ACClass, vMethod:="DoAllocationDetailPairsExist")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        iDoAllocationDetailPairsExist = m_oDatabase.Parameters.Item("r_bDoAllocationDetailPairsExist").Value

        r_bDoAllocationDetailPairsExist = (iDoAllocationDetailPairsExist = 1)

        Return result



        result = gPMConstants.PMEReturnCode.PMError

        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="DoAllocationDetailPairsExist Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DoAllocationDetailPairsExist", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function

    ''' <summary>
    ''' Reverse the allocation of the original debt
    ''' </summary>
    ''' <param name="v_lTransDetailID"></param>
    ''' <param name="v_lCashListItemID"></param>
    ''' <param name="v_lAllocationID"></param>
    ''' <param name="r_lAccountID"></param>
    ''' <param name="r_sFailureReason"></param>
    ''' <param name="v_bDisableTransactions"></param>
    ''' <param name="bSelectSplit"></param>
    ''' <param name="bReverseWriteOff"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ReverseAllocation(Optional ByVal v_lTransDetailID As Integer = 0,
                                      Optional ByVal v_lCashListItemID As Integer = 0,
                                      Optional ByVal v_lAllocationID As Integer = 0,
                                      Optional ByRef r_lAccountID As Integer = 0,
                                      Optional ByRef r_sFailureReason As String = "",
                                      Optional ByVal v_bDisableTransactions As Boolean = False,
                                      Optional ByVal bSelectSplit As Boolean = False,
                                      Optional ByVal bReverseWriteOff As Boolean = False,
                                      Optional ByVal iVoidTransactionLogID As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sSQLSearch As String = ""

        Dim vResults(,) As Object = Nothing
        Dim vDebitResults(,) As Object = Nothing

        'SD 15/01/2003
        Dim bTransactionStarted As Boolean
        Dim lAllocationID As Integer ' RAW 12/03/2003 : ISS2893 : added
        Dim lAllocationDetailID_CR As Integer ' RAW 12/03/2003 : ISS2893 : renamed to represent Credit trans
        Dim lAllocationDetailID_DR As Integer ' RAW 12/03/2003 : ISS2893 : added
        Dim sCreditControlValue As String = ""
        'SMJB 13/08/03 CQ2189
        Dim vDocumentArray(,) As Object = Nothing
        Dim oDocumentReversal As Object = Nothing

        Try

            result = PMEReturnCode.PMTrue

            ' Alix - 03/03/2003 - Issue 2525
            ' RAW 12/03/2003 : ISS2893 : added allocationId
            If v_lTransDetailID = 0 And v_lCashListItemID = 0 And v_lAllocationID = 0 Then
                bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogError, sMsg:="ReverseAllocation failed. At least one parameter must be passed in.",
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="ReverseAllocation")
                Return PMEReturnCode.PMFalse
            End If

            'SMJB 15/10/03: Now check if there are any Small Amount Write Off Documents in this allocation
            'if there are, then they must be reversed separately

            m_lReturn = CheckForSmallAmountWriteOffs(r_vDocumentArray:=vDocumentArray, v_lTransDetailID:=v_lTransDetailID,
                                                     v_lCashListItemID:=v_lCashListItemID, r_lAllocationID:=v_lAllocationID)


            If bReverseWriteOff Then
                'If there were SWD documents, then they will be returned into vDocumentArray
                If Informations.IsArray(vDocumentArray) Then
                    m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oDocumentReversal,
                                                                          v_sClassName:="bACTDocumentReversal.Business",
                                                                          v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername,
                                                                          v_sPassword:=m_sPassword, v_iUserID:=m_iUserID,
                                                                          v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID,
                                                                          v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        Return PMEReturnCode.PMFalse
                    End If

                    'Reverse each SWD document
                    For lCount As Integer = 0 To vDocumentArray.GetUpperBound(1)
                        oDocumentReversal.DocumentId = vDocumentArray(2, lCount)
                        m_lReturn = oDocumentReversal.Start()
                        If m_lReturn <> PMEReturnCode.PMTrue Then
                            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogError,
                                               sMsg:="Failed to reverse document " & CStr(vDocumentArray(1, lCount)),
                                               vApp:=ACApp, vClass:=ACClass, vMethod:=result)
                            Return PMEReturnCode.PMFalse
                        End If
                    Next
                    oDocumentReversal.Dispose()
                    oDocumentReversal = Nothing
                End If
            End If



            With m_oDatabase

                ' RAW 12/03/2003 : ISS2893 : condensed to 1 sql statement

                With .Parameters

                    .Clear()

                    ' Alix - 03/03/2003 - Issue 2525
                    If v_lAllocationID <> 0 Then

                        m_lReturn = .Add(sName:="v_iAllocationId", vValue:=CStr(v_lAllocationID),
                                         iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMLong)
                    Else
                        m_lReturn = .Add(sName:="v_iAllocationId", vValue:=DBNull.Value,
                                         iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMLong)
                    End If
                    If v_lCashListItemID <> 0 Then

                        m_lReturn = .Add(sName:="v_iCashListItemId", vValue:=CStr(v_lCashListItemID),
                                         iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMLong)
                    Else
                        m_lReturn = .Add(sName:="v_iCashListItemId", vValue:=DBNull.Value,
                                         iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMLong)
                    End If

                    If v_lTransDetailID <> 0 Then
                        m_lReturn = .Add(sName:="v_iTransDetailId", vValue:=CStr(v_lTransDetailID),
                                         iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMLong)
                    Else
                        m_lReturn = .Add(sName:="v_iTransDetailId", vValue:=DBNull.Value,
                                         iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMLong)
                    End If
                    If bSelectSplit Then
                        m_lReturn = .Add(sName:="bSelectSplit", vValue:=1,
                                         iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMBoolean)
                    End If

                End With


                If m_lReturn = PMEReturnCode.PMTrue Then
                    m_lReturn = .SQLSelect(sSQL:=ACSelectReverseAllocationSQL, sSQLName:=ACSelectReverseAllocationName,
                                           bStoredProcedure:=True, vResultArray:=vResults)
                End If

            End With

            If m_lReturn <> PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(sUsername:="", iType:=PMELogLevel.PMLogError, sMsg:="Failed on running.SP",
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="ReverseAllocation")
                Return PMEReturnCode.PMFalse
            ElseIf (m_lReturn = PMEReturnCode.PMTrue) And Not (Informations.IsArray(vResults)) Then
                r_lAccountID = 0
                Return result
            End If


            ' RAW 12/03/2003 : ISS2893 : added loop to handle multiple allocations for a transaction

            For lRow As Integer = vResults.GetLowerBound(1) To vResults.GetUpperBound(1)

                ' Note - if it was not possible to pair up credits and debits then the complete allocation
                ' will have to be reversed in one go - the previous sql will only return 1 row with the allocation id.
                ' This means that this loop will have no knowledge of individual transactions, will only
                ' execute once, so credit control changes will not be activated


                ' RAW 12/03/2003 : ISS2893 : added new columns to result set

                lAllocationID = CInt(vResults(0, lRow))

                r_lAccountID = CInt(vResults(1, lRow)) ' from credit transaction

                lAllocationDetailID_CR = CInt(vResults(3, lRow)) ' from credit transaction

                lAllocationDetailID_DR = CInt(vResults(4, lRow)) ' from debit transaction
				
				 If iVoidTransactionLogID > 0 Then
                    m_lReturn = AddReversalDetailLog(iVoidTransactionLogID, lAllocationID, r_lAccountID)
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(sUsername:="", iType:=PMELogLevel.PMLogError, sMsg:="Failed on running AddReversalDetailLog ", vApp:=ACApp, vClass:=ACClass, vMethod:="ReverseAllocation")
                    End If
                End If
				

                With m_oDatabase

                    If Not v_bDisableTransactions Then
                        'SD 15/01/2003 START Add a Credit Control Item if req'd - do this within a transaction
                        bTransactionStarted = True
                        .SQLBeginTrans()
                    End If

                    With .Parameters
                        .Clear()

                        ' RAW 12/03/2003 : ISS2893 : Add a param for the return value from the stored proc
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="return_value", vValue:=CStr(0), iDirection:=PMEParameterDirection.PMParamReturnValue, iDataType:=PMEDataType.PMLong)

                        m_lReturn = .Add(sName:="v_iAllocationId", vValue:=CStr(lAllocationID), iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMLong)

                        ' RAW 12/03/2003 : ISS2893 : replaced allocationdetailid with separate ones for credit and debit

                        If lAllocationDetailID_CR <> 0 Then

                            m_lReturn = .Add(sName:="v_iAllocationDetailID_CR", vValue:=CStr(lAllocationDetailID_CR),
                                             iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMLong)
                        Else

                            m_lReturn = .Add(sName:="v_iAllocationDetailID_CR", vValue:=DBNull.Value,
                                             iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMLong)

                        End If

                        If lAllocationDetailID_DR <> 0 Then
                            m_lReturn = .Add(sName:="v_iAllocationDetailID_DR", vValue:=CStr(lAllocationDetailID_DR),
                                             iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMLong)
                        Else
                            m_lReturn = .Add(sName:="v_iAllocationDetailID_DR", vValue:=DBNull.Value,
                                             iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMLong)
                        End If

                        m_lReturn = .Add(sName:="v_iUserID", vValue:=m_iUserID,
                                         iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMLong)
                    End With

                    If m_lReturn = PMEReturnCode.PMTrue Then
                        m_lReturn = .SQLAction(sSQL:=ACReverseAllocationSQL, sSQLName:=ACReverseAllocationName, bStoredProcedure:=ACReverseAllocationStored)
                    End If

                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(sUsername:="", iType:=PMELogLevel.PMLogError, sMsg:="Failed on running SP " & ACReverseAllocationSQL,
                                           vApp:=ACApp, vClass:=ACClass, vMethod:="ReverseAllocation")
                        result = PMEReturnCode.PMFalse
                        If Not v_bDisableTransactions And bTransactionStarted Then
                            .SQLRollbackTrans()
                        End If
                        Return result
                    End If

                    If m_oDatabase.Parameters.Item("return_value").Value <> 0 Then
                        bPMFunc.LogMessage(sUsername:="", iType:=PMELogLevel.PMLogError,
                                           sMsg:=ACReverseAllocationSQL & " returned " & m_oDatabase.Parameters.Item("return_value").Value,
                                           vApp:=ACApp, vClass:=ACClass, vMethod:="ReverseAllocation")
                        result = PMEReturnCode.PMFalse
                        If Not v_bDisableTransactions And bTransactionStarted Then
                            .SQLRollbackTrans()
                        End If
                        Return result
                    End If

                    'FSA Phase 3.2 Reverse any commission movement
                    m_lReturn = ReverseCommissionMovement(v_lAllocationID:=lAllocationID)
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        r_sFailureReason = "Unable to reverse commission movement."
                        result = PMEReturnCode.PMFalse
                        If Not v_bDisableTransactions And bTransactionStarted Then
                            .SQLRollbackTrans()
                        End If
                        Return result
                    End If

                    'Check if the credit control system option is activated
                    m_lReturn = GetSystemOptionValue(CInt(ACCreditControlOptionNo), sCreditControlValue)
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        result = PMEReturnCode.PMFalse
                        If Not v_bDisableTransactions And bTransactionStarted Then
                            .SQLRollbackTrans()
                        End If
                        Return result
                    End If


                    'Only proceed with credit control insert if system option has been set
                    If String.Compare(sCreditControlValue, ACValueWhenCreditControlSet) = 0 Then
                        m_lReturn = GetDebitDetails(v_lAllocationID:=lAllocationID, r_Results:=vDebitResults)

                        If Not Informations.IsArray(vDebitResults) Then
                            r_sFailureReason = "Unable to retrieve debit details."
                            result = PMEReturnCode.PMFalse
                            If Not v_bDisableTransactions And bTransactionStarted Then
                                .SQLRollbackTrans()
                            End If
                            Return result
                        End If

                        ' SMJB 13/08/03 CQ2189 Must loop through all debits to ensure Credit Control steps are activated

                        For lCount As Integer = 0 To vDebitResults.GetUpperBound(1)
                            ' RAW 12/03/2003 : ISS2893 : transfered code to a new function
                            ' SW CQ 849 924 pass in debit transaction not the credit
                            'SMJB CQ2189 CashListItem_ID no longer required


                            ' Need to make sure that If Debit Transaction is Instalment Debit Transaction then activate Credit Control Item
                            ' only for instalment which is recalled
                            If vDebitResults(1, lCount).ToString.Contains("IND") OrElse
                                                vDebitResults(1, lCount).ToString.Contains("IED") OrElse
                                                vDebitResults(1, lCount).ToString.Contains("IRD") OrElse
                                                vDebitResults(1, lCount).ToString.Contains("IDR") Then

                                m_lReturn = AddToCreditControl(v_lAllocationDetailID:=CInt(vDebitResults.GetValue(0, lCount)),
                                                                   nTransDetailId:=v_lTransDetailID)
                            Else
                                m_lReturn = AddToCreditControl(v_lAllocationDetailID:=CInt(vDebitResults.GetValue(0, lCount)))
                            End If

                            If m_lReturn <> PMEReturnCode.PMTrue Then
                                'return failure description to the client
                                r_sFailureReason = "Unable to add the Credit Control Item(s) into the database, please check the credit control set up."
                                result = PMEReturnCode.PMFalse
                                If Not v_bDisableTransactions And bTransactionStarted Then
                                    .SQLRollbackTrans()
                                End If
                                Return result
                            End If
                        Next
                    End If

                    If Not v_bDisableTransactions And bTransactionStarted Then
                        .SQLCommitTrans()
                    End If

                End With
            Next lRow


            If Not bReverseWriteOff Then
                'If there were SWD documents, then they will be returned into vDocumentArray
                If Informations.IsArray(vDocumentArray) Then
                    m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oDocumentReversal,
                                                                          v_sClassName:="bACTDocumentReversal.Business",
                                                                          v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername,
                                                                          v_sPassword:=m_sPassword, v_iUserID:=m_iUserID,
                                                                          v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID,
                                                                          v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        Return PMEReturnCode.PMFalse
                    End If

                    'Reverse each SWD document
                    For lCount As Integer = 0 To vDocumentArray.GetUpperBound(1)
                        oDocumentReversal.DocumentId = vDocumentArray(2, lCount)
                        m_lReturn = oDocumentReversal.Start()
                        If m_lReturn <> PMEReturnCode.PMTrue Then
                            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogError,
                                               sMsg:="Failed to reverse document " & CStr(vDocumentArray(1, lCount)),
                                               vApp:=ACApp, vClass:=ACClass, vMethod:=result)
                            Return PMEReturnCode.PMFalse
                        End If
                    Next
                    oDocumentReversal.Dispose()
                    oDocumentReversal = Nothing
                End If
            End If

            Return result

        Catch excep As System.Exception
            result = PMEReturnCode.PMError
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".ReverseAllocation")
            bPMFunc.LogMessage(sUsername:="", iType:=PMELogLevel.PMLogError, sMsg:="ReverseAllocation Failed",
                               vApp:=ACApp, vClass:=ACClass, vMethod:="ReverseAllocation", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            If Not v_bDisableTransactions And bTransactionStarted Then
                m_oDatabase.SQLRollbackTrans()
            End If
            Return result
        End Try
    End Function
	
	 Public Function AddReversalDetailLog(ByVal v_lLogid As Integer, ByVal v_lallocation_id As Integer, ByVal v_laccount_id As Integer) As Integer
        m_lReturn = PMEReturnCode.PMTrue
        m_oDatabase.Parameters.Clear()
        m_lReturn = m_oDatabase.Parameters.Add(sName:="LogId", vValue:=ToSafeInteger(v_lLogid), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Allocation_id", vValue:=ToSafeInteger(v_lallocation_id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        m_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=ToSafeInteger(v_laccount_id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertVoidReversalDetailSQL, sSQLName:=ACInsertVoidReversalDetailName, bStoredProcedure:=ACInsertVoidReversalDetailStored)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return m_lReturn
    End Function
	


    ' RAW 12/03/2003 : ISS2893 : added - code extracted from ReverseAllocation
    ' ***************************************************************** '
    ' Name: AddToCreditControl
    '
    ' Description: Adds rows to credit control.
    '
    ' ***************************************************************** '
    Private Function AddToCreditControl(ByVal v_lAllocationDetailID As Integer,
                                        Optional ByVal nTransDetailId As Integer = 0) As Integer

        'SMJB CQ2189 13/08/03 v_lCashListItemID parameter no longer required for SP
        'so removed
        Dim result As Integer = 0
        Const ksMyProcedureName As String = "AddToCreditControl"
        Dim lMyReturn As gPMConstants.PMEReturnCode

        Dim vSQLResultArray As Object = Nothing
        Dim bError As Boolean = False

        Dim lCreditControlItemID As Integer = 0
        Dim sInfo As String = ""
        Dim iReturnValue As Integer = 0
        Dim bParamInputError As Boolean


        Try

            lMyReturn = gPMConstants.PMEReturnCode.PMTrue


            With m_oDatabase

                With .Parameters
                    .Clear()

                    lMyReturn = .Add(sName:="allocationdetail_id", vValue:=CStr(v_lAllocationDetailID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If nTransDetailId > 0 Then
                        lMyReturn = .Add(sName:="transdetail_id", vValue:=nTransDetailId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    End If

                    If lMyReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bParamInputError = True
                    End If



                End With

                If Not bParamInputError Then
                    lMyReturn = .SQLAction(sSQL:=ACReAddCredControlItemSQL, sSQLName:=ACReAddCredControlItemName, bStoredProcedure:=ACReAddCredControlItemStored)
                Else
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Error on adding parameters", vApp:=ACApp, vClass:=ACClass, vMethod:=ksMyProcedureName)
                    lMyReturn = gPMConstants.PMEReturnCode.PMFalse
                    .SQLRollbackTrans()
                    Return result
                End If

                If lMyReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Error on re-adding credit control item.SP " & ACReAddCredControlItemSQL, vApp:=ACApp, vClass:=ACClass, vMethod:=ksMyProcedureName)
                    lMyReturn = gPMConstants.PMEReturnCode.PMFalse
                    .SQLRollbackTrans()
                    Return result
                End If


            End With


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    m_oDatabase.SQLRollbackTrans()

                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ksMyProcedureName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ksMyProcedureName, vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    lMyReturn = gPMConstants.PMEReturnCode.PMError
            End Select
        Finally
            result = lMyReturn
            m_lReturn = lMyReturn
        End Try


        Return result

    End Function



    'SD 09/01/2003 Start - Use system option for Credit Control changes
    ' ***************************************************************** '
    ' Name: GetSystemOptionValue
    ' Description:
    ' History: 09/01/2003 sd - Created.
    '
    ' ***************************************************************** '
    Private Function GetSystemOptionValue(ByRef v_iOptionNumber As Integer, ByRef r_sValue As String) As Integer

        Dim result As Integer = 0
        Dim oOptions As bSIROptions.Business



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get an instance of component services

        oOptions = New bSIROptions.Business
        m_lReturn = oOptions.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the value for this option

        m_lReturn = oOptions.GetOption(iOptionNumber:=v_iOptionNumber, sValue:=r_sValue)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then

            oOptions = Nothing

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get system option", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSystemOptionValue")

            Return gPMConstants.PMEReturnCode.PMFalse

        End If

        'If system option not found, default to zero
        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
            r_sValue = "0"
        End If


        oOptions.Dispose()
        oOptions = Nothing

        Return result

    End Function
    'SD 09/01/2003 End



    ' ***************************************************************** '
    ' Name: CheckForSmallAmountWriteOffs
    '
    ' Description: Checks if there are any SWD documents in the allocation we are about to reverse.
    '
    ' Written By: Simon.Baynes
    '
    ' Returns: An array of the document refs and document IDs of the SWDs if there were any
    '           Also returns the allocation id if it was zero when passed in
    '
    ' Date: 15/10/2003
    '
    ' ***************************************************************** '
    Private Function CheckForSmallAmountWriteOffs(ByRef r_vDocumentArray(,) As Object, Optional ByVal v_lTransDetailID As Integer = 0, Optional ByVal v_lCashListItemID As Integer = 0, Optional ByRef r_lAllocationID As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim vResults(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="TransDetailID", vValue:=CStr(v_lTransDetailID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = .Parameters.Add(sName:="CashListItemID", vValue:=CStr(v_lCashListItemID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = .Parameters.Add(sName:="AllocationID", vValue:=CStr(r_lAllocationID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = .SQLSelect(sSQL:=ACGetSWDDocumentsInAllocationSQL, sSQLName:=ACGetSWDDocumentsInAllocationName, bStoredProcedure:=ACGetSWDDocumentsInAllocationStored, vResultArray:=vResults)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CheckForSmallAmountWriteOffs failed", vApp:=ACApp, vClass:=ACClass, vMethod:=result)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End With

        'Return the allocation ID if it was passed in as zero
        If Informations.IsArray(vResults) Then
            If r_lAllocationID = 0 Then

                r_lAllocationID = CInt(vResults(0, 0))
            End If

            r_vDocumentArray = vResults
        End If


        Return result

    End Function
End Class