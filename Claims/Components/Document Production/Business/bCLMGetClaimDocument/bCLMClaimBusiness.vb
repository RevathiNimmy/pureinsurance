Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
'developer guide no.129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 06/10/1998
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a CLMInfoChklst.
    '
    ' Edit History:
    ' SJP14062002 - getUnderWritingOrAgency uses new product options scheme
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 11/12/2003
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
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Collection of CLMInfoChklsts (Private)
    'Private m_oCLMInfoChklsts As bCLMInfoChklst.CLMInfoChklsts

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Primary Keys to work with
    Private m_lClmExpServId As Integer

    'AM(11/12/2000) Claim Handler
    Private m_sUnderwritingOrAgency As String = ""
    Private m_oSystemOption As bSIROptions.Business

    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property



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

    'AM(11/12/2000) Claims Numbering.
    Public ReadOnly Property UnderwritingOrAgency() As String
        Get

            If m_sUnderwritingOrAgency = "" Then
                m_lReturn = getUnderwritingOrAgency()
            End If

            Return m_sUnderwritingOrAgency

        End Get
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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise





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




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now


            ' Create PM Lookup Business Object
            m_oLookup = New bPMLookup.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '' ***************************************************************** '
    '' Name: GetLookupValues (Public)
    ''
    '' Description: Gets the Lookup values for a CLMInfoChklst.
    ''
    ''
    '' ***************************************************************** '
    'Public Function GetLookupValues( _
    ''    iLookupType As Integer, _
    ''    vTableArray As Variant, _
    ''    iLanguageID As Integer, _
    ''    vResultArray As Variant) As Long
    '
    'Dim oCLMInfoChklst As bCLMInfoChklst.CLMInfoChklst
    'Dim dtEffectiveDate As Date
    '
    '' {* USER DEFINED CODE (Begin) *}
    'Dim vTabArray(3, 0) As Variant
    'Dim vReserveID As Variant
    ''Dim vEventTypeID As Variant
    '' {* USER DEFINED CODE (End) *}
    '
    '    On Error GoTo Err_GetLookupValues
    '
    '    GetLookupValues = PMTrue
    '
    '    ' Reset Result Array
    '    vResultArray = ""
    '    ' Reset Table Array
    '    vTableArray = ""
    '
    '    ' {* USER DEFINED CODE (Begin) *}
    '
    '    ' Setup Lookup Table Names
    '    vTabArray(PMLookupTableName, 0) = SIRLookupContactType
    ''    vTabArray(PMLookupTableName, 1) = PMLookupEventType
    '
    '    ' {* USER DEFINED CODE (End) *}
    '
    '    ' Do we have any records
    '    If (m_lCurrentRecord& < 1) Then
    '        ' No, we can only lookup all
    '        iLookupType = PMLookupAll
    '    Else
    '        ' Yes get current record
    '        Set oCLMInfoChklst = m_oCLMInfoChklsts.Item(m_lCurrentRecord&)
    '    End If
    '
    '    Select Case iLookupType%
    '      Case PMLookupAll
    '
    '        ' Do not supply a key
    '        vTabArray(PMLookupKey, 0) = ""
    ''        vTabArray(PMLookupKey, 1) = ""
    '
    '      Case PMLookupAllEffective
    '
    '        ' Use keys and effective date from current record
    '        ' Note: The keys are not used for the select, but are used by
    '        '       the iterface program to set the list index.
    '        With oCLMInfoChklst
    '
    '            ' {* USER DEFINED CODE (Begin) *}
    '            m_lReturn& = .GetProperties(iStatus:=PMView, _
    ''                                        vReserveID:=vReserveID)
    '
    '            vTabArray(PMLookupKey, 0) = vReserveID
    '            ' {* USER DEFINED CODE (End) *}
    '
    '        End With
    '
    '      Case PMLookupSingle
    '
    '        ' Set keys from current record
    '        With oCLMInfoChklst
    '
    '            ' {* USER DEFINED CODE (Begin) *}
    '            m_lReturn& = .GetProperties(iStatus:=PMView, _
    ''                                        vReserveID:=vReserveID)
    '
    '            vTabArray(PMLookupKey, 0) = vReserveID
    '            'vTabArray(PMLookupKey, 1) = vEventTypeID
    '            ' {* USER DEFINED CODE (End) *}
    '
    '        End With
    '
    '    End Select
    '
    '    ' Default Effective Date to current date/time
    '    dtEffectiveDate = Now
    '
    '    ' Release CLMInfoChklst reference
    '    Set oCLMInfoChklst = Nothing
    '
    '    ' Get the Lookup items
    '    m_lReturn& = m_oLookup.GetLookupValues( _
    ''        iLookupType:=iLookupType, _
    ''        vTableArray:=vTabArray, _
    ''        iLanguageID:=iLanguageID, _
    ''        dtEffectiveDate:=dtEffectiveDate, _
    ''        vResultArray:=vResultArray)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        GetLookupValues = PMFalse
    '        Exit Function
    '    End If
    '
    '    ' Return the Table Array
    '    vTableArray = vTabArray
    '
    '    Exit Function
    '
    '
    'Err_GetLookupValues:
    '
    '    GetLookupValues = PMError
    '
    '    ' Log Error Message
    '    LogMessage m_sUsername, _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="GetLookupValues Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="GetLookupValues", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function

    ' ***************************************************************** '
    ' Name: Cancel (Public)
    '
    ' Description: Checks the Collection to see if Cancel is OK.
    '              i.e. Do we need any Adding, Deleting or Updating.
    '
    '              Returns PMTrue if all items are clean
    '                      (PMView or PMDummyDelete)
    '              Otherwise returns PMDataChanged.
    ' ***************************************************************** '
    Public Function Cancel() As Integer
        Dim result As Integer = 0

        Try

            '    ' Loop round Collection
            '    For lSub& = 1 To m_oCLMInfoChklsts.Count
            '        Select Case m_oCLMInfoChklsts.Item(lSub&).DatabaseStatus
            '            Case PMView, PMDummyDelete
            '                ' Do nothing
            '            Case PMAdd, PMEdit, PMDelete
            '                Cancel = PMDataChanged
            '                Exit For
            '        End Select
            '    Next lSub&

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Cancel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '

    'Private Function BeginTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLBeginTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '

    'Private Function CommitTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLCommitTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '

    'Private Function RollbackTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLRollbackTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Private)
    '
    ' Description: Check Mandatory parameters have been passed.
    '
    ' ***************************************************************** '

    'Private Function CheckMandatory(Optional ByRef vClmExpServId As Object = Nothing, Optional ByRef vClaim_Id As Object = Nothing, Optional ByRef vExpServId As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vServTypeId As Object = Nothing, Optional ByRef vService As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vReference As Object = Nothing, Optional ByRef vContact As Object = Nothing, Optional ByRef vDateReq As Object = Nothing, Optional ByRef vDateCrit As Object = Nothing, Optional ByRef vDateRecv As Object = Nothing, Optional ByRef vPaymentId As Object = Nothing, Optional ByRef vClmId As Object = Nothing, Optional ByRef vPaymentAmount As Object = Nothing, Optional ByRef vDateofPayment As Object = Nothing, Optional ByRef vComments As Object = Nothing, Optional ByRef vTable As Object = Nothing) As Integer
    '
    ''    On Error GoTo Err_CheckMandatory
    ''
    ''    CheckMandatory = PMTrue
    ''
    ''    ' {* USER DEFINED CODE (Begin) *}
    ''
    ''    If (IsMissing(vReserveID) = True) _
    '''    Or (IsEmpty(vReserveID) = True) Then
    ''        CheckMandatory = PMMandatoryMissing
    ''        Exit Function
    ''    End If
    ''
    ''    If (IsMissing(vSourceID) = True) _
    '''    Or (IsEmpty(vSourceID) = True) Then
    ''        CheckMandatory = PMMandatoryMissing
    ''        Exit Function
    ''    End If
    ''
    ''    If (IsMissing(vExpServId) = True) _
    '''    Or (IsEmpty(vExpServId) = True) Then
    ''        CheckMandatory = PMMandatoryMissing
    ''        Exit Function
    ''    End If
    ''
    ''    If (IsMissing(vClaim_Id) = True) _
    '''    Or (IsEmpty(vClaim_Id) = True) Then
    ''        CheckMandatory = PMMandatoryMissing
    ''        Exit Function
    ''    End If
    ''
    ''    If (IsMissing(vNumber) = True) _
    '''    Or (IsEmpty(vNumber) = True) Then
    ''        CheckMandatory = PMMandatoryMissing
    ''        Exit Function
    ''    End If
    ''
    ''    If (IsMissing(vCreatedByID) = True) _
    '''    Or (IsEmpty(vCreatedByID) = True) Then
    ''        CheckMandatory = PMMandatoryMissing
    ''        Exit Function
    ''    End If
    ''
    ''    If (IsMissing(vDateCreated) = True) _
    '''    Or (IsEmpty(vDateCreated) = True) Then
    ''        CheckMandatory = PMMandatoryMissing
    ''        Exit Function
    ''    End If
    ''
    ''
    ''    ' {* USER DEFINED CODE (End) *}
    ''
    ''    Exit Function
    ''
    ''
    ''Err_CheckMandatory:
    ''
    ''    CheckMandatory = PMError
    ''
    ''    ' Log Error Message
    ''    LogMessage m_sUsername, _
    '''        iType:=PMLogOnError, _
    '''        sMsg:="CheckMandatory Failed", _
    '''        vApp:=ACApp, _
    '''        vClass:=ACClass, _
    '''        vMethod:="CheckMandatory", _
    '''        vErrNo:=Err.Number, _
    '''        vErrDesc:=Err.Description
    ''
    ''    Exit Function
    ''
    'End Function
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()


        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: UnderwritingOrAgency
    '
    ' Description:  Finds out if Underwriting or Agency business
    '
    ' Created By:  'RWH(09/11/2000) For Claims Numbering.
    '
    ' SJP14062002 - getUnderWritingOrAgency uses new product options scheme
    ' ***************************************************************** '
    Public Function getUnderwritingOrAgency() As Integer

        Dim result As Integer = 0
        Try


            Return bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_sUnderwritingOrAgency)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnderwritingOrAgencyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnderwritingOrAgency", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetCleintAndPolicyID
    '
    ' Description:
    '
    ' History: 16/02/2001 ajm - Created.
    '
    ' ***************************************************************** '
    Public Function GetClientAndPolicyID(ByVal v_lClaimID As Object, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Claim Number parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_id", vValue:=CStr(v_lClaimID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientAndPolicyID")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClientAndPolicyIDSQL, sSQLName:=ACGetClientAndPolicyID, bStoredProcedure:=ACGetClientAndPolicyIDStored, vResultArray:=r_vResultArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientAndPolicyID")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            If Not Information.IsArray(r_vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClientAndPolicyID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientAndPolicyID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetLargeLossAdviceLimit
    '
    ' Description: Get the maximum claim value required to force production
    '              of a large loss advice for claims reinsurance
    '
    ' History: 07/03/2001 ajm - Created.
    '
    ' ***************************************************************** '
    Public Function GetLargeLossAdviceLimit(ByRef r_vClaimLimit As String) As Integer

        Dim result As Integer = 0
        Dim iOptionNumber As Integer
        Dim sOptionValue As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iOptionNumber = 1014
            sOptionValue = ""

            m_lReturn = CType(GetOption(r_iOptionNumber:=iOptionNumber, v_sOptionValue:=sOptionValue), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_vClaimLimit = sOptionValue

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLargeLossAdviceLimit Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLargeLossAdviceLimit", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetOption (Private)
    '
    ' Description: Get an option.
    '
    ' ***************************************************************** '
    Private Function GetOption(ByRef r_iOptionNumber As Integer, ByRef v_sOptionValue As String) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        If m_oSystemOption Is Nothing Then
            m_oSystemOption = New bSIROptions.Business()

            m_lReturn = m_oSystemOption.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the system option object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If
        End If

        m_lReturn = m_oSystemOption.GetOption(iOptionNumber:=r_iOptionNumber, sValue:=v_sOptionValue)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get system option data", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return result
        End If

        m_oSystemOption.Dispose()

        

        m_oSystemOption = Nothing

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: GetEffectiveDate
    '
    ' Description:
    '
    ' History: 26/04/2007 VB - Created.
    '
    ' ***************************************************************** '
    Public Function GetEffectiveDate(ByVal v_lClaimID As Integer, ByRef r_dtEffectiveDate As Date) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetEffectiveDate"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Claim Number parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_id", vValue:=CStr(v_lClaimID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If
            'developer guide no.40
            'm_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTimeHelper.ToString(r_dtEffectiveDate), idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMDate)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=r_dtEffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetEffectiveDateSQL, sSQLName:=ACGetEffectiveDateName, bStoredProcedure:=ACGetEffectiveDateStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If


            If Not (Convert.IsDBNull(m_oDatabase.Parameters.Item("effective_date").Value) Or IsNothing(m_oDatabase.Parameters.Item("effective_date").Value)) Then
                r_dtEffectiveDate = gPMFunctions.ToSafeDate(m_oDatabase.Parameters.Item("effective_date").Value)
            End If


        Catch ex As Exception


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetEffectiveDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
        Finally



        End Try
        Return result
    End Function
    Public Function GetClaimSpooledDesc(ByVal v_lClaimID As Object, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Claim Number parameter (INPUT)
            'UPGRADE_WARNING: (1068) v_lClaimID of type Variant is being forced to String. More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_id", vValue:=CStr(v_lClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimSpooledDesc")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClaimSpooleDescdSql, sSQLName:=ACGetClaimSpooleDescdName, bStoredProcedure:=ACGetClaimSpooledDescStored, vResultArray:=r_vResultArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimSpooledDesc")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            '    If Not IsArray(r_vResultArray) Then
            '        GetClaimSpooledDesc = PMFalse
            '        Exit Function
            '    End If

            Return result

        Catch excep As System.Exception




            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimSpooledDesc Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimSpooledDesc", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
