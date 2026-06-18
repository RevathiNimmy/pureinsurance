Option Strict Off
Option Explicit On
Imports SSP.Shared
'Developer Guide No 129

<System.Runtime.InteropServices.ProgId("Form_NET.Form")>
Public NotInheritable Class Form
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 13/05/1997
    '
    ' Description: Creatable Form class which contains all the
    '              methods, business rules required for the
    '              DocumentPost Form.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' ************************************************
    ' Added to replace global variables 09/12/2003
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
    Private Const ACClass As String = "Form"

    'TR - Enumerate various states of transactions
    Private Enum TransState
        Trans_Unset = 0
        Trans_Commit = 1
        Trans_Rollback = 2
    End Enum

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    '27/07/03 Tracy Richards - PN5205 - Removed non-requested rollback
    Private m_enTransStatusOnTerminate As TransState
    'Private m_bAbortTrans As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

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

    Private m_cTotalBase As Decimal
    Private m_vdTotalBaseUnrounded As Single
    Private m_cTotalCurrency As Decimal

    Private m_oDocument As bACTDocument.Form
    Private m_oTransDetail As bACTTransdetail.Form
    Private m_lPeriodId As Integer

    Private m_lDocumentID As Integer

    Private m_lReversingDocumentID As Integer
    Private m_vRecurringDocumentIDs As Object

    ' NavigatorV3 variables
    Private m_lPMAuthorityLevel As Integer
    Private m_lStatus As Integer

    Private m_lPostingPeriodNumber As Integer

    Public Property PostingPeriodNumber() As Integer
        Get
            Return m_lPostingPeriodNumber
        End Get
        Set(ByVal Value As Integer)
            m_lPostingPeriodNumber = Value
        End Set
    End Property

    ' PRIVATE Data Members (End)
    ' PUBLIC Property Procedures (Begin)

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

    Public Property TotalBase() As Decimal
        Get
            Return m_cTotalBase
        End Get
        Set(ByVal Value As Decimal)
            m_cTotalBase = Value
        End Set
    End Property

    Public Property TotalBaseUnrounded() As Single
        Get
            Return m_vdTotalBaseUnrounded
        End Get
        Set(ByVal Value As Single)

            m_vdTotalBaseUnrounded = CSng(Value)
        End Set
    End Property

    Public Property TotalCurrency() As Decimal
        Get
            Return m_cTotalCurrency
        End Get
        Set(ByVal Value As Decimal)
            m_cTotalCurrency = Value
        End Set
    End Property

    Public Property AbortTrans() As Boolean
        Get
            Return m_enTransStatusOnTerminate = Form.TransState.Trans_Rollback
            '27/07/03 Tracy Richards - PN5205 - Removed non-requested rollback
            'AbortTrans = m_bAbortTrans
        End Get
        Set(ByVal Value As Boolean)
            '27/07/03 Tracy Richards - PN5205 - Removed non-requested rollback
            'm_bAbortTrans = bAbortTrans
            If Value Then
                m_enTransStatusOnTerminate = TransState.Trans_Rollback
            Else
                If m_enTransStatusOnTerminate = Form.TransState.Trans_Rollback Then
                    m_enTransStatusOnTerminate = TransState.Trans_Unset
                End If
            End If
        End Set
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

    Public ReadOnly Property StepStatus() As String
        Get

            ' Standard Property.

            ' Return the Steps Status
            Return m_sStepStatus.Value

        End Get
    End Property
    'RKC 21/08/2002
    'Access Required for this property by External Objects

    Public Property PeriodID() As Integer
        Get
            Return m_lPeriodId
        End Get
        Set(ByVal Value As Integer)

            m_lPeriodId = Value

        End Set
    End Property

    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: GetKeys
    '
    ' Description: Navigator GetKeys function.
    '
    ' ***************************************************************** '
    'Developer Guide No. 101
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            vKeyArray = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeysFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSummary
    '
    ' Description: GetSummary Navigator function.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer

        Dim result As Integer = 0
        Try

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummaryFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

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

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oDocument = New bACTDocument.Form
            m_lReturn = m_oDocument.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oTransDetail = New bACTTransdetail.Form
            m_lReturn = m_oTransDetail.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_enTransStatusOnTerminate = TransState.Trans_Unset

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Set the ProcessMode to Generic
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric

            ' Set the Type Of Business to New Business
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric

            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now

            m_cTotalBase = 0
            m_cTotalCurrency = 0

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
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
            If disposing Then

                If m_bCloseDatabase AndAlso m_oDatabase IsNot Nothing Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If

                If m_oDocument IsNot Nothing Then
                    m_oDocument.Dispose()
                    m_oDocument = Nothing
                End If

                If m_oTransDetail IsNot Nothing Then
                    m_oTransDetail.Dispose()
                    m_oTransDetail = Nothing
                End If
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

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLedgerIdFromDocumentId
    '
    ' Description: Given a document_id, will trace into the transdetails
    '              and get the account_id. Then into account and get
    '              the matching ledger_id.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetLedgerIdFromDocumentId) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetLedgerIdFromDocumentId(ByVal v_vDocumentID As Object, ByRef r_vLedgerID As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' clear the parameters
    'm_oDatabase.Parameters.Clear()
    '
    ' add new ones

    'm_lReturn = m_oDatabase.Parameters.Add(sName:="DocumentID", vValue:=CStr(v_vDocumentID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="LedgerID", vValue:=CStr(r_vLedgerID), idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    ' Execute the SQL
    'm_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetLedgerFromDocumentSQL, sSQLName:=ACGetLedgerFromDocumentName, bStoredProcedure:=ACGetLedgerFromDocumentStored)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    ' Grab the returned value
    'r_vLedgerID = gPMFunctions.NullToLong(m_oDatabase.Parameters.Item("LedgerID").Value)
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '

    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLedgerIdFromDocumentId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLedgerIdFromDocumentId", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: Start
    '
    ' Description: Entry point from navigator
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Do we have recurring documents?
            If Informations.IsArray(m_vRecurringDocumentIDs) Then

                ' Loop through each recurring document id and post it

                For iLoop1 As Integer = m_vRecurringDocumentIDs.GetLowerBound(0) To m_vRecurringDocumentIDs.GetUpperBound(0)

                    ' Post the document

                    m_lReturn = CType(PostDocument(v_lDocumentID:=CInt(m_vRecurringDocumentIDs(iLoop1))), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Next iLoop1

            Else

                ' Post the main document
                m_lReturn = CType(PostDocument(v_lDocumentID:=m_lDocumentID), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Post the reversing document too
                If m_lReversingDocumentID <> 0 Then
                    m_lReturn = CType(PostDocument(v_lDocumentID:=m_lReversingDocumentID), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetKeys
    '
    ' Description: Navigator SetKeys function
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sTmp As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a vaild array.
            If Not Informations.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the
                ' correct key array item.

                ' {* USER DEFINED CODE (Begin) *}

                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMNavKeyConst.ACTKeyNameDocumentID

                        m_lDocumentID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.ACTKeyNameRecurringDocumentIDs

                        sTmp = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        m_lReturn = CType(gACTLibrary.ParseArray(vArray:=m_vRecurringDocumentIDs, sString:=sTmp, bArrayToString:=False), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ParseArray Failed for Recurring Document IDs", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        End If

                    Case PMNavKeyConst.ACTKeyNameReversingDocumentID

                        m_lReversingDocumentID = CInt(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))

                End Select

                ' {* USER DEFINED CODE (End) *}
            Next lRow

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeysFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetTransactionsForDoc
    '
    ' Description: Gets the transdetails that have the document_id pased
    '
    ' ***************************************************************** '
    Public Function GetTransactionsForDoc(ByVal v_lDocumentID As Integer, ByRef r_vTransDetailIDs(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' construct the sql
            sSQL = "SELECT transdetail_id FROM TransDetail WHERE document_id = " &
                   v_lDocumentID

            ' perform the query
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetTransactionsForDoc", bStoredProcedure:=False, vResultArray:=r_vTransDetailIDs)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check that there are some transactions for the document
            If Not Informations.IsArray(r_vTransDetailIDs) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTransactionsForDocFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTransactionsForDoc", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ************************************************************************* '
    ' Name: UpdateAccountingDates
    '
    ' Description: Updates the accounting dates on the transactions depending
    '              on any closed periods.
    '
    ' ************************************************************************* '
    Private Function UpdateAccountingDates(ByVal v_lDocumentID As Integer, ByVal v_vTransDetailIds(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim oPeriod As bACTPeriod.Form
        Dim lLedgerPeriod As Integer
        Dim dtLedgerDate As Date



        result = gPMConstants.PMEReturnCode.PMTrue

        ' construct the SQL
        sSQL = "SELECT transdetail_id FROM transdetail WHERE " &
               "document_id = " & CStr(v_lDocumentID) & " AND " &
               "document_sequence = 1"

        ' Perform the query
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetPrimaryLedger", bStoredProcedure:=False, vResultArray:=vResultArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsArray(vResultArray) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the ledger for the primary transaction

        sSQL = "SELECT current_period_id FROM ledger WHERE ledger_id = (" &
               "SELECT ledger_id FROM account WHERE account_id = (" &
           "SELECT account_id FROM transdetail WHERE transdetail_id = " & CStr(vResultArray(0, 0)) & "))"

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetPeriodPrimTransLedger", bStoredProcedure:=False, vResultArray:=vResultArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsArray(vResultArray) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the ledger's period

        lLedgerPeriod = CInt(vResultArray(0, 0))

        ' Get an instance of bACTPeriod

        oPeriod = New bACTPeriod.Form
        m_lReturn = oPeriod.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = oPeriod.GetFirstDayOfPeriod(v_lPeriodID:=lLedgerPeriod, r_dtDateInPeriod:=dtLedgerDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        For iLoop1 As Integer = v_vTransDetailIds.GetLowerBound(1) To v_vTransDetailIds.GetUpperBound(1)

            ' Clear the parameters.
            m_oDatabase.Parameters.Clear()

            ' Add transdetail

            m_lReturn = m_oDatabase.Parameters.Add(sName:="transdetail_id", vValue:=CStr(v_vTransDetailIds(0, iLoop1)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add current period
            m_lReturn = m_oDatabase.Parameters.Add(sName:="current_period_id", vValue:=CStr(lLedgerPeriod), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the date it should set it to if its in a closed period
            m_lReturn = m_oDatabase.Parameters.Add(sName:="first_date", vValue:=dtLedgerDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Perform the query
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateTransActDateSQL, sSQLName:=ACUpdateTransActDateName, bStoredProcedure:=ACUpdateTransActDateStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Next iLoop1

        ' Remove the object

        oPeriod.Dispose()
        oPeriod = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: PostDocument
    '
    ' Description: Posts the document and any associated transaction
    '
    ' ***************************************************************** '
    Public Function PostDocument() As Integer
        Return PostDocument(v_lDocumentID:=0)
    End Function

    Public Function PostDocument(ByVal v_lDocumentID As Integer) As Integer

        Dim result As Integer
        Dim sSQL As String = ""
        Dim vTransDetailIDs(,) As Object = Nothing
        Dim lTransDetailID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not True Then
                v_lDocumentID = m_lDocumentID
            End If

            ' Get the list of transactions that will need posting too
            m_lReturn = CType(GetTransactionsForDoc(v_lDocumentID:=v_lDocumentID, r_vTransDetailIDs:=vTransDetailIDs), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Begin the transaction
            m_oDatabase.SQLBeginTrans()

            ' Update the accounting dates and correct the periods
            m_lReturn = CType(UpdateAccountingDates(v_lDocumentID:=v_lDocumentID, v_vTransDetailIds:=vTransDetailIDs), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Rollback the transaction
                m_oDatabase.SQLRollbackTrans()

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update accounting dates", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Construct the SQL
            sSQL = "UPDATE Document SET postingstatus_id = 3 WHERE document_id = " &
                   v_lDocumentID

            ' Update the database
            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="PostDocument", bStoredProcedure:=False)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Rollback the transaction
                m_oDatabase.SQLRollbackTrans()

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set posted status on document. Document_id = " & v_lDocumentID, vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Loop through each associated transaction and post it

            For iLoop1 As Integer = vTransDetailIDs.GetLowerBound(1) To vTransDetailIDs.GetUpperBound(1)

                ' Get the transdetail_id

                lTransDetailID = CInt(vTransDetailIDs(0, iLoop1))

                ' Construct the sql
                sSQL = "UPDATE TransDetail SET postingstatus_id = 3 where transdetail_id = " &
                       lTransDetailID

                m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="PostTransaction", bStoredProcedure:=False)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Rollback the transaction
                    m_oDatabase.SQLRollbackTrans()

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set posted status on document. Document_id = " & v_lDocumentID & " Transdetail_ID = " & CStr(lTransDetailID), vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' fully_matched - settle the primary transaction
                '

            Next iLoop1

            ' End the transaction
            m_oDatabase.SQLCommitTrans()

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostDocumentFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ''' <summary>
    ''' AddDocument
    ''' </summary>
    ''' <param name="v_lDocumentTypeId"></param>
    ''' <param name="v_sDocumentRef"></param>
    ''' <param name="v_dtDocumentDate"></param>
    ''' <param name="v_sComment"></param>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <param name="v_sReason"></param>
    ''' <param name="r_vDocumentID"></param>
    ''' <param name="r_vDocSourceID"></param>
    ''' <param name="r_vSubBranchID"></param>
    ''' <param name="v_vBatchID"></param>
    ''' <param name="v_vClaimID"></param>
    ''' <param name="v_vTermsOfPaymentId"></param>
    ''' <param name="v_vPaymentDueDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddDocument(ByVal v_lDocumentTypeId As Integer, ByVal v_sDocumentRef As String, ByVal v_dtDocumentDate As Date, ByVal v_sComment As String,
                                Optional ByVal v_lInsuranceFileCnt As Integer = 0, Optional ByVal v_sReason As String = "", Optional ByRef r_vDocumentID As Object = Nothing,
                                Optional ByVal r_vDocSourceID As Object = Nothing, Optional ByVal r_vSubBranchID As Object = Nothing,
                                Optional ByVal v_vBatchID As Object = Nothing, Optional ByVal v_vClaimID As Object = Nothing,
                                Optional ByVal v_vTermsOfPaymentId As Object = Nothing, Optional ByVal v_vPaymentDueDate As Object = Nothing) As Integer

        ' Variables for defaulting
        Dim nResult As Integer
        Dim nCompanyID As Integer
        Dim oSubBranchID As Object
        Dim nUserID As Integer
        Dim dtPostedDate As Date
        Dim dtCreatedDate As Date
        Dim dtAuthorisedDate As Date
        Dim dtAccountingDate As Date
        Dim nPostingstatusID As Integer
        Dim nDocumentId As Integer

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Defaults
            nCompanyID = m_iSourceID

            If Not Informations.IsNothing(r_vDocSourceID) Then
                If Not (Convert.IsDBNull(r_vDocSourceID) Or Informations.IsNothing(r_vDocSourceID)) Then
                    nCompanyID = CInt(r_vDocSourceID)
                End If
            End If

            If Not Informations.IsNothing(r_vSubBranchID) Then
                oSubBranchID = r_vSubBranchID
            Else
                oSubBranchID = DBNull.Value
            End If

            nUserID = m_iUserID
            dtPostedDate = DateTime.Now
            dtCreatedDate = DateTime.Now
            dtAuthorisedDate = DateTime.Now
            dtAccountingDate = DateTime.Now
            nPostingstatusID = gACTLibrary.ACTPostStatusPosted

            ' Do a Direct add so we can get the ID
            m_lReturn = m_oDocument.DirectAdd(vDocumentID:=nDocumentId, vCompanyID:=nCompanyID, vPostingstatusID:=nPostingstatusID,
                                              vDocumenttypeID:=v_lDocumentTypeId, vDocumentRef:=v_sDocumentRef, vDocumentDate:=v_dtDocumentDate,
                                              vCreatedDate:=dtCreatedDate, vAuthorisedDate:=dtAuthorisedDate, vComment:=v_sComment,
                                              vInsuranceFileCnt:=v_lInsuranceFileCnt, vReason:=v_sReason, vSubBranchID:=oSubBranchID,
                                              vBatchID:=v_vBatchID, vClaimID:=v_vClaimID, v_vTermsOfPaymentId:=v_vTermsOfPaymentId,
                                              v_vPaymentDueDate:=v_vPaymentDueDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Read back the added record into the collection

            m_lReturn = m_oDocument.GetDetails(nDocumentId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsNothing(r_vDocumentID) Then
                r_vDocumentID = nDocumentId
            End If

            ' the current period cannot be determined here now
            ' it must be set when adding a transaction
            If m_lPostingPeriodNumber > 0 Then
                m_lPeriodId = m_lPostingPeriodNumber
            End If

            Return nResult

        Catch excep As Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddDocument", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    Public Function GetDocument(ByVal v_lDocumentID As Integer) As Integer

        ' Variables for defaulting
        Dim result As Integer = 0
        'Dim lLedgerID As Long

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Read back the added record into the collection

            m_lReturn = m_oDocument.GetDetails(v_lDocumentID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '  m_lReturn& = GetPeriodIdForDate(r_lPeriodId:=m_lPeriodId, _
            ''                                 v_dtAccountingDate:=v_dtDocumentDate)
            '  If (m_lReturn& <> PMTrue) Then
            '    GetDocument = PMFalse
            '    Exit Function
            '  End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocument", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLedgerIdForAccountId
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    'EK 150200 Made Public
    'Developer guide no 101
    Public Function GetLedgerIdForAccountId(ByVal v_vAccountID As Object, ByRef r_vLedgerID As Object) As Integer

        Dim result As Integer = 0
        Try

            Dim sSQL As String = ""
            Dim vResultArray(,) As Object = Nothing

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT ledger_id FROM Account WHERE account_id = " & v_vAccountID

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetLedgerIdForAccountID", bStoredProcedure:=False, vResultArray:=vResultArray)

            If Informations.IsArray(vResultArray) Then

                r_vLedgerID = CInt(vResultArray(0, 0))
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLedgerIdForAccountIdFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLedgerIdForAccountId", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ''' <summary>
    ''' AddTransaction
    ''' </summary>
    ''' <param name="v_lAccountID"></param>
    ''' <param name="v_iCurrencyID"></param>
    ''' <param name="v_cAmount"></param>
    ''' <param name="v_cCurrencyAmount"></param>
    ''' <param name="v_vdCurrencyBaseXRate"></param>
    ''' <param name="r_vTransDetailId"></param>
    ''' <param name="v_vDocumentSequence"></param>
    ''' <param name="v_vComment"></param>
    ''' <param name="v_vEuroCurrencyId"></param>
    ''' <param name="v_vEuroAmount"></param>
    ''' <param name="v_vEuroBaseXRate"></param>
    ''' <param name="v_vEuroCcyXrate"></param>
    ''' <param name="v_vInsuranceRef"></param>
    ''' <param name="v_vOperatorID"></param>
    ''' <param name="v_vPurchaseOrderNo"></param>
    ''' <param name="v_vPurchaseInvoiceNo"></param>
    ''' <param name="v_vDepartment"></param>
    ''' <param name="v_vSpare"></param>
    ''' <param name="v_vRefDate"></param>
    ''' <param name="v_vRefAmount"></param>
    ''' <param name="v_vRefQuantity"></param>
    ''' <param name="v_vRefUnits"></param>
    ''' <param name="v_vBaseAmountUnrounded"></param>
    ''' <param name="v_vCurrencyAmountUnrounded"></param>
    ''' <param name="v_vAccountingDate"></param>
    ''' <param name="v_vDocSourceID"></param>
    ''' <param name="v_vSubBranchId"></param>
    ''' <param name="v_vUnderwritingYearID"></param>
    ''' <param name="v_vCurrencyBaseDate"></param>
    ''' <param name="v_vAccountBaseXrate"></param>
    ''' <param name="v_vAccountBaseDate"></param>
    ''' <param name="v_vSystemBaseXrate"></param>
    ''' <param name="v_vSystemBaseDate"></param>
    ''' <param name="v_vTransdetailTypeID"></param>
    ''' <param name="v_vReference"></param>
    ''' <param name="v_vTypeCode"></param>
    ''' <param name="v_vTaxGroupID"></param>
    ''' <param name="v_vTaxBandID"></param>
    ''' <param name="v_vClaimReference"></param>
    ''' <param name="v_vRiskTransfer"></param>
    ''' <param name="v_vBalanceType"></param>
    ''' <param name="v_periodID"></param>
    ''' <param name="v_vDueDate"></param>
    ''' <param name="oFeeType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddTransaction(ByVal v_lAccountID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_cAmount As Decimal, ByVal v_cCurrencyAmount As Decimal,
                                   ByVal v_vdCurrencyBaseXRate As Object, Optional ByRef r_vTransDetailId As Object = Nothing,
                                   Optional ByVal v_vDocumentSequence As Object = Nothing, Optional ByVal v_vComment As Object = Nothing,
                                   Optional ByVal v_vEuroCurrencyId As Object = Nothing, Optional ByVal v_vEuroAmount As Object = Nothing,
                                   Optional ByVal v_vEuroBaseXRate As Object = Nothing, Optional ByVal v_vEuroCcyXrate As Object = Nothing,
                                   Optional ByVal v_vInsuranceRef As Object = Nothing, Optional ByVal v_vOperatorID As Object = Nothing,
                                   Optional ByVal v_vPurchaseOrderNo As Object = Nothing, Optional ByVal v_vPurchaseInvoiceNo As Object = Nothing,
                                   Optional ByVal v_vDepartment As Object = Nothing, Optional ByVal v_vSpare As Object = Nothing,
                                   Optional ByVal v_vRefDate As Object = Nothing, Optional ByVal v_vRefAmount As Object = Nothing,
                                   Optional ByVal v_vRefQuantity As Object = Nothing, Optional ByVal v_vRefUnits As Object = Nothing,
                                   Optional ByVal v_vBaseAmountUnrounded As Object = Nothing, Optional ByVal v_vCurrencyAmountUnrounded As Object = Nothing,
                                   Optional ByVal v_vAccountingDate As Object = Nothing, Optional ByVal v_vDocSourceID As Object = Nothing,
                                   Optional ByVal v_vSubBranchId As Object = Nothing, Optional ByVal v_vUnderwritingYearID As Object = Nothing,
                                   Optional ByVal v_vCurrencyBaseDate As Object = Nothing, Optional ByVal v_vAccountBaseXrate As Object = Nothing,
                                   Optional ByVal v_vAccountBaseDate As Object = Nothing, Optional ByVal v_vSystemBaseXrate As Object = Nothing,
                                   Optional ByVal v_vSystemBaseDate As Object = Nothing, Optional ByVal v_vTransdetailTypeID As Object = Nothing,
                                   Optional ByVal v_vReference As Object = Nothing, Optional ByVal v_vTypeCode As Object = Nothing,
                                   Optional ByVal v_vTaxGroupID As Object = Nothing, Optional ByVal v_vTaxBandID As Object = Nothing,
                                   Optional ByVal v_vClaimReference As Object = Nothing, Optional ByRef v_vRiskTransfer As Object = Nothing,
                                   Optional ByVal v_vBalanceType As Object = Nothing, Optional ByVal v_periodID As Object = Nothing,
                                   Optional ByVal v_vDueDate As Object = Nothing, Optional ByVal oFeeType As Object = Nothing) As Integer

        ' Values to be defaulted
        Dim nResult As Integer
        Dim nTransDetailID As Integer
        Dim sComment As String
        Dim crBaseAmountUnRounded As Decimal
        Dim crCurrencyAmountUnRounded As Decimal
        Dim nLedgerID As Integer
        Dim oAccountingDate As Object

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            Dim sSysOpt As String = ""
            With m_oDocument.Details.Item(1)

                'If no comment then use documents comment
                If Informations.IsNothing(v_vComment) Then
                    sComment = .Comment
                Else
                    sComment = v_vComment.ToString()
                End If

                'If no unrounded base amount then set it to rounded base amount
                If Informations.IsNothing(v_vBaseAmountUnrounded) Then
                    crBaseAmountUnRounded = v_cAmount
                Else
                    crBaseAmountUnRounded = v_vBaseAmountUnrounded
                End If

                'If no unrounded currency amount then set it to rounded currency amount
                If Informations.IsNothing(v_vCurrencyAmountUnrounded) Then
                    crCurrencyAmountUnRounded = v_cCurrencyAmount
                Else
                    crCurrencyAmountUnRounded = v_vCurrencyAmountUnrounded
                End If

                'If no user then use logged in user
                If Informations.IsNothing(v_vOperatorID) Then
                    v_vOperatorID = m_iUserID
                End If

                'If no document sequence then fail
                If Informations.IsNothing(v_vDocumentSequence) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'If no date then use the documents date
                'If Informations.IsNothing(v_vAccountingDate) Then
                '    vAccountingDate = .DocumentDate
                'Else
                oAccountingDate = DateTime.Now
                'End If

                'get system option "Posting by Pol Eff Date & Trans date"
                m_lReturn = CType(bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=5038, r_sOptionValue:=sSysOpt), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'If system option "Posting by Pol Eff Date & Trans date" is true
                If gPMFunctions.ToSafeLong(sSysOpt, 0) = 1 Then

                    'Posting period should be greater period of the "policy effective date"
                    'and "transaction date"
                    'If gPMFunctions.ToSafeDate(v_vRefDate) > gPMFunctions.ToSafeDate(v_vAccountingDate) Then
                    oAccountingDate = DateTime.Now
                    'Else
                    ' vAccountingDate = gPMFunctions.ToSafeDate(v_vAccountingDate)
                    ' End If

                End If

                'Get ledger id
                m_lReturn = CType(GetLedgerIdForAccountId(v_vAccountID:=v_lAccountID, r_vLedgerID:=nLedgerID), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'If no Ref_Date then default it to the Accounting_Date
                'If Informations.IsNothing(v_vRefDate) Then
                v_vRefDate = DateTime.Now
                'End If

                If Not Informations.IsNothing(v_vSpare) Then
                    If v_vSpare = "WRITEOFF" Then
                        m_lPostingPeriodNumber = gPMFunctions.ToSafeInteger(v_periodID)
                    End If
                End If
                'If no period id then get it
                If m_lPostingPeriodNumber > 0 AndAlso v_vSpare <> "WRITEOFF" Then
                    m_lPeriodId = m_lPostingPeriodNumber
                Else
                    m_lReturn = CType(GetPeriodIdForDate(r_lPeriodId:=m_lPeriodId, v_dtAccountingDate:=oAccountingDate, lLedgerID:=nLedgerID), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTransaction Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTransaction", vErrNo:="", vErrDesc:="There are no valid Accounting Periods for the date " & ToSafeDate(oAccountingDate).ToString("dd-MMM-yyyy") &
                                           "." & Strings.ChrW(13) & Strings.ChrW(10) & "Please go to Accounting Period Maintenance and add additional financial years.")

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                m_lReturn = m_oTransDetail.DirectAdd(vTransdetailID:=nTransDetailID, vAccountID:=v_lAccountID, vPostingstatusID:= .PostingstatusID,
                                                     vCompanyID:= .CompanyID, vCurrencyID:=v_iCurrencyID, vPeriodID:=m_lPeriodId, vDocumentID:= .DocumentID,
                                                     vDocumentSequence:=v_vDocumentSequence, vAccountingDate:=oAccountingDate, vAmount:=v_cAmount,
                                                     vBaseAmountUnrounded:=crBaseAmountUnRounded, vFullyMatched:=gPMConstants.PMEReturnCode.PMFalse,
                                                     vCurrencyAmount:=v_cCurrencyAmount, vCurrencyAmountUnrounded:=crCurrencyAmountUnRounded,
                                                     vEuroCurrencyId:=v_vEuroCurrencyId, vEuroAmount:=v_vEuroAmount, vEuroBaseXRate:=v_vEuroBaseXRate,
                                                     vEuroCcyXrate:=v_vEuroCcyXrate, vComment:=sComment, vInsuranceRef:=v_vInsuranceRef, vOperatorID:=v_vOperatorID,
                                                     vPurchaseOrderNo:=v_vPurchaseOrderNo, vPurchaseInvoiceNo:=v_vPurchaseInvoiceNo, vDepartment:=v_vDepartment,
                                                     vSpare:=v_vSpare, vRefDate:=v_vRefDate, vRefAmount:=v_vRefAmount, vRefQuantity:=v_vRefQuantity,
                                                     vRefUnits:=v_vRefUnits, vUnderwritingYearID:=v_vUnderwritingYearID, vCurrencyBaseXrate:=v_vdCurrencyBaseXRate,
                                                     vCurrencyBaseDate:=v_vCurrencyBaseDate, vAccountBaseXrate:=v_vAccountBaseXrate,
                                                     vAccountBaseDate:=v_vAccountBaseDate, vSystemBaseXrate:=v_vSystemBaseXrate, vSystemBaseDate:=v_vSystemBaseDate,
                                                     vTransdetailTypeID:=v_vTransdetailTypeID, vReference:=v_vReference, vTypeCode:=v_vTypeCode,
                                                     vTaxGroupID:=v_vTaxGroupID, vTaxBandID:=v_vTaxBandID, vClaimReference:=v_vClaimReference,
                                                     vRiskTransfer:=v_vRiskTransfer, vBalanceType:=v_vBalanceType, vDueDate:=v_vDueDate, oFeeType:=oFeeType)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                r_vTransDetailId = nTransDetailID
            End With

            'Calculate totals
            m_cTotalBase += v_cAmount
            m_vdTotalBaseUnrounded += crBaseAmountUnRounded
            m_cTotalCurrency += v_cCurrencyAmount

            If Not Informations.IsNothing(r_vTransDetailId) Then
                r_vTransDetailId = nTransDetailID
            End If

            Return nResult

        Catch excep As System.Exception

            ' Error Section.

            nResult = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTransaction Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTransaction", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ''' <summary>
    ''' AddAdjustmentTransaction    
    ''' </summary>
    ''' <param name="v_lAccountID"></param>
    ''' <param name="v_iCurrencyID"></param>
    ''' <param name="v_cAmount"></param>
    ''' <param name="v_cCurrencyAmount"></param>
    ''' <param name="v_vdCurrencyBaseXRate"></param>
    ''' <param name="r_vTransDetailId"></param>
    ''' <param name="v_vDocumentSequence"></param>
    ''' <param name="v_vComment"></param>
    ''' <param name="v_vEuroCurrencyId"></param>
    ''' <param name="v_vEuroAmount"></param>
    ''' <param name="v_vEuroBaseXRate"></param>
    ''' <param name="v_vEuroCcyXrate"></param>
    ''' <param name="v_vInsuranceRef"></param>
    ''' <param name="v_vOperatorID"></param>
    ''' <param name="v_vPurchaseOrderNo"></param>
    ''' <param name="v_vPurchaseInvoiceNo"></param>
    ''' <param name="v_vDepartment"></param>
    ''' <param name="v_vSpare"></param>
    ''' <param name="v_vRefDate"></param>
    ''' <param name="v_vRefAmount"></param>
    ''' <param name="v_vRefQuantity"></param>
    ''' <param name="v_vRefUnits"></param>
    ''' <param name="v_vBaseAmountUnrounded"></param>
    ''' <param name="v_vCurrencyAmountUnrounded"></param>
    ''' <param name="v_vAccountingDate"></param>
    ''' <param name="v_vDocSourceID"></param>
    ''' <param name="v_vSubBranchId"></param>
    ''' <param name="v_vUnderwritingYearID"></param>
    ''' <param name="v_vCurrencyBaseDate"></param>
    ''' <param name="v_vAccountBaseXrate"></param>
    ''' <param name="v_vAccountBaseDate"></param>
    ''' <param name="v_vSystemBaseXrate"></param>
    ''' <param name="v_vSystemBaseDate"></param>
    ''' <param name="v_vTransdetailTypeID"></param>
    ''' <param name="v_vReference"></param>
    ''' <param name="v_vTypeCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddAdjustmentTransaction(ByVal v_lAccountID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_cAmount As Decimal, ByVal v_cCurrencyAmount As Decimal, ByVal v_vdCurrencyBaseXRate As Object, Optional ByRef r_vTransDetailId As Object = Nothing, Optional ByVal v_vDocumentSequence As Object = Nothing, Optional ByVal v_vComment As Object = Nothing, Optional ByVal v_vEuroCurrencyId As Object = Nothing, Optional ByVal v_vEuroAmount As Object = Nothing, Optional ByVal v_vEuroBaseXRate As Object = Nothing, Optional ByVal v_vEuroCcyXrate As Object = Nothing, Optional ByVal v_vInsuranceRef As Object = Nothing, Optional ByVal v_vOperatorID As Object = Nothing, Optional ByVal v_vPurchaseOrderNo As Object = Nothing, Optional ByVal v_vPurchaseInvoiceNo As Object = Nothing, Optional ByVal v_vDepartment As Object = Nothing, Optional ByVal v_vSpare As Object = Nothing, Optional ByVal v_vRefDate As Object = Nothing, Optional ByVal v_vRefAmount As Object = Nothing, Optional ByVal v_vRefQuantity As Object = Nothing, Optional ByVal v_vRefUnits As Object = Nothing, Optional ByVal v_vBaseAmountUnrounded As Object = Nothing, Optional ByVal v_vCurrencyAmountUnrounded As Object = Nothing, Optional ByVal v_vAccountingDate As Object = Nothing, Optional ByVal v_vDocSourceID As Object = Nothing, Optional ByVal v_vSubBranchId As Object = Nothing, Optional ByVal v_vUnderwritingYearID As Object = Nothing, Optional ByVal v_vCurrencyBaseDate As Object = Nothing, Optional ByVal v_vAccountBaseXrate As Object = Nothing, Optional ByVal v_vAccountBaseDate As Object = Nothing, Optional ByVal v_vSystemBaseXrate As Object = Nothing, Optional ByVal v_vSystemBaseDate As Object = Nothing, Optional ByVal v_vTransdetailTypeID As Object = Nothing, Optional ByVal v_vReference As Object = Nothing, Optional ByVal v_vTypeCode As Object = Nothing) As Integer

        ' Values to be defaulted
        Dim result As Integer = 0
        Dim lTransDetailID As Integer
        Dim sComment As String = ""
        Dim cBaseAmountUnRounded, cCurrencyAmountUnRounded As Decimal
        Dim lLedgerID As Integer
        Dim vAccountingDate As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDocument.Details.Item(1)

                'If no comment then use documents comment

                If Informations.IsNothing(v_vComment) Then

                    sComment = .Comment
                Else
                    sComment = v_vComment
                End If

                'If no unrounded base amount then set it to rounded base amount

                If Informations.IsNothing(v_vBaseAmountUnrounded) Then
                    cBaseAmountUnRounded = v_cAmount
                Else
                    cBaseAmountUnRounded = v_vBaseAmountUnrounded
                End If

                'If no unrounded currency amount then set it to rounded currency amount

                If Informations.IsNothing(v_vCurrencyAmountUnrounded) Then
                    cCurrencyAmountUnRounded = v_cCurrencyAmount
                Else
                    cCurrencyAmountUnRounded = v_vCurrencyAmountUnrounded
                End If

                'If no user then use logged in user

                If Informations.IsNothing(v_vOperatorID) Then
                    v_vOperatorID = m_iUserID
                End If

                'If no document sequence then fail

                If Informations.IsNothing(v_vDocumentSequence) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'If no date then use the documents date

                If Informations.IsNothing(v_vAccountingDate) Then

                    vAccountingDate = .DocumentDate
                Else

                    vAccountingDate = v_vAccountingDate
                End If

                'Get ledger id
                m_lReturn = CType(GetLedgerIdForAccountId(v_vAccountID:=CStr(v_lAccountID), r_vLedgerID:=lLedgerID), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'If no Ref_Date then default it to the Accounting_Date

                If Informations.IsNothing(v_vRefDate) Then

                    v_vRefDate = vAccountingDate
                End If

                'If no period id then get it
                If m_lPostingPeriodNumber > 0 AndAlso v_vSpare <> "WRITEOFF" Then
                    m_lPeriodId = m_lPostingPeriodNumber
                Else

                    m_lReturn = CType(GetPeriodIdForDate(r_lPeriodId:=m_lPeriodId, v_dtAccountingDate:=CDate(vAccountingDate), lLedgerID:=lLedgerID), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                m_lReturn = m_oTransDetail.DirectAdd(vTransdetailID:=lTransDetailID, vAccountID:=v_lAccountID, vPostingstatusID:= .PostingstatusID, vCompanyID:= .CompanyID, vCurrencyID:=v_iCurrencyID, vPeriodID:=m_lPeriodId, vDocumentID:= .DocumentID, vDocumentSequence:=v_vDocumentSequence, vAccountingDate:=vAccountingDate, vAmount:=v_cAmount, vBaseAmountUnrounded:=cBaseAmountUnRounded, vFullyMatched:=gPMConstants.PMEReturnCode.PMFalse, vCurrencyAmount:=v_cCurrencyAmount, vCurrencyAmountUnrounded:=cCurrencyAmountUnRounded, vEuroCurrencyId:=v_vEuroCurrencyId, vEuroAmount:=v_vEuroAmount, vEuroBaseXRate:=v_vEuroBaseXRate, vEuroCcyXrate:=v_vEuroCcyXrate, vComment:=sComment, vInsuranceRef:=v_vInsuranceRef, vOperatorID:=v_vOperatorID, vPurchaseOrderNo:=v_vPurchaseOrderNo, vPurchaseInvoiceNo:=v_vPurchaseInvoiceNo, vDepartment:=v_vDepartment, vSpare:=v_vSpare, vRefDate:=v_vRefDate, vRefAmount:=v_vRefAmount, vRefQuantity:=v_vRefQuantity, vRefUnits:=v_vRefUnits, vUnderwritingYearID:=v_vUnderwritingYearID, vCurrencyBaseXrate:=v_vdCurrencyBaseXRate, vCurrencyBaseDate:=v_vCurrencyBaseDate, vAccountBaseXrate:=v_vAccountBaseXrate, vAccountBaseDate:=v_vAccountBaseDate, vSystemBaseXrate:=v_vSystemBaseXrate, vSystemBaseDate:=v_vSystemBaseDate, vTransdetailTypeID:=v_vTransdetailTypeID, vReference:=v_vReference, vTypeCode:=v_vTypeCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            'Calculate totals
            m_cTotalBase += v_cAmount
            m_vdTotalBaseUnrounded += cBaseAmountUnRounded
            m_cTotalCurrency += v_cCurrencyAmount

            If Not Informations.IsNothing(r_vTransDetailId) Then
                r_vTransDetailId = lTransDetailID
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAdjustmentTransaction Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddAdjustmentTransaction", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' --------------------------------------------------------------
    ' Validate the Document and its transactions
    ' If its ok then set the Abort flag to false so that the
    ' Database transactions are not rolled back
    ' --------------------------------------------------------------
    Public Function Commit() As Integer

        Dim result As Integer = 0
        Try

            ' Assume the worst
            result = gPMConstants.PMEReturnCode.PMFalse

            ' Check that the document adds up
            If m_cTotalBase <> 0 Then
                Return result
            End If

            ' Says that it passed validation
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Will commit trans in terminate
            'm_bAbortTrans = False
            '27/07/03 Tracy Richards - PN5205 - Removed non-requested rollback
            m_enTransStatusOnTerminate = TransState.Trans_Commit

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Commit Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Commit", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)
    'EK 150200 Made Public
    ' DD 13/08/2002: Made LedgerID mandatory so that period is always determined
    Public Function GetPeriodIdForDate(ByRef r_lPeriodId As Integer, ByVal v_dtAccountingDate As Date, ByRef lLedgerID As Integer) As Integer

        Dim result As Integer = 0
        Dim lPeriodID As Integer
        Dim oPeriod As bACTPeriod.Form

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' CF280898

            oPeriod = New bACTPeriod.Form
            m_lReturn = oPeriod.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oPeriod.GetPostingPeriodForDate(dtDateInPeriod:=v_dtAccountingDate, lPeriodID:=lPeriodID, lLedgerID:=lLedgerID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    Return ACTransErrPeriodNotDef
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            r_lPeriodId = lPeriodID

            oPeriod = Nothing

            ' CTAF 270601 - Check if the period has been set up
            If lPeriodID = 0 Then
                result = ACTransErrPeriodNotDef
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPeriodIdForDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPeriodIdForDate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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


    ' ***************************************************************** '
    ' Name: GetDocumentReference
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 30-10-2006 :
    ' ***************************************************************** '
    Private Function GetDocumentReference(ByVal v_lDocumentTypeId As Integer, ByVal v_lDocumentSourceId As Integer, ByRef r_sDocumentRef As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetDocumentReference"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sGroupCode As String = ""
        Dim sRangeCode As String = ""
        Dim sReference As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        ' get the group and range code for the document type
        lReturn = m_oDocument.GetAutoNumValues(v_iDocumentTypeID:=v_lDocumentTypeId, r_sGroupCode:=sGroupCode, r_sRangeCode:=sRangeCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("bACTDocument.Form.GetAutoNumValues", "Function failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' get the document ref number
        'Start(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber
        lReturn = m_oDocument.GenerateDocumentReferenceNumber(v_sGroupCode:=sGroupCode, v_sRangeCode:=sRangeCode, v_iUserID:=m_iUserID, v_iCompanyID:=v_lDocumentSourceId, r_sDocumentRef:=sReference)

        If lReturn = gPMConstants.PMEReturnCode.PMTrue And sReference.Trim() <> "" Then

            ' Prefix a few zeros

            sReference = sRangeCode & sReference
            'End(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            r_sDocumentRef = sReference

        Else
            '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber
            gPMFunctions.RaiseError(kMethodName, "bACTDocument.Form.GenerateDocumentReferenceNumber Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        Return result
    End Function

    Public Function AddDocumentTransactions(ByRef r_vDocumentID As Object, ByVal v_lDocumentTypeId As Integer, ByVal v_sBranchID As String, ByVal v_sComment As String, ByVal v_dtDocumentDate As Date, ByVal v_vDocSourceID As Integer, ByRef v_sDocumentRef As String, ByVal v_vOperatorID As Object, ByVal v_vTransArray(,) As Object) As Integer
        Return AddDocumentTransactions(r_vDocumentID:=r_vDocumentID, v_lDocumentTypeId:=v_lDocumentTypeId, v_sBranchID:=v_sBranchID, v_sComment:=v_sComment, v_dtDocumentDate:=v_dtDocumentDate, v_vDocSourceID:=v_vDocSourceID, v_sDocumentRef:=v_sDocumentRef, v_vOperatorID:=v_vOperatorID, v_vTransArray:=v_vTransArray, v_lInsuranceFileCnt:=0, vTransdetailTypeID:=Nothing)
    End Function

    Public Function AddDocumentTransactions(ByRef r_vDocumentID As Object, ByVal v_lDocumentTypeId As Integer, ByVal v_sBranchID As String, ByVal v_sComment As String, ByVal v_dtDocumentDate As Date, ByVal v_vDocSourceID As Integer, ByRef v_sDocumentRef As String, ByVal v_vOperatorID As Object, ByVal v_vTransArray(,) As Object, ByRef v_lInsuranceFileCnt As Integer) As Integer
        Return AddDocumentTransactions(r_vDocumentID:=r_vDocumentID, v_lDocumentTypeId:=v_lDocumentTypeId, v_sBranchID:=v_sBranchID, v_sComment:=v_sComment, v_dtDocumentDate:=v_dtDocumentDate, v_vDocSourceID:=v_vDocSourceID, v_sDocumentRef:=v_sDocumentRef, v_vOperatorID:=v_vOperatorID, v_vTransArray:=v_vTransArray, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, vTransdetailTypeID:=Nothing)
    End Function

    ''' <summary>
    ''' AddDocumentTransactions
    ''' </summary>
    ''' <param name="r_vDocumentID"></param>
    ''' <param name="v_lDocumentTypeId"></param>
    ''' <param name="v_sBranchID"></param>
    ''' <param name="v_sComment"></param>
    ''' <param name="v_dtDocumentDate"></param>
    ''' <param name="v_vDocSourceID"></param>
    ''' <param name="v_sDocumentRef"></param>
    ''' <param name="v_vOperatorID"></param>
    ''' <param name="v_vTransArray"></param>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <param name="vTransdetailTypeID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddDocumentTransactions(ByRef r_vDocumentID As Object,
                                            ByVal v_lDocumentTypeId As Integer,
                                            ByVal v_sBranchID As String,
                                            ByVal v_sComment As String,
                                            ByVal v_dtDocumentDate As Date,
                                            ByVal v_vDocSourceID As Integer,
                                            ByRef v_sDocumentRef As String,
                                            ByVal v_vOperatorID As Object,
                                            ByVal v_vTransArray(,) As Object,
                                            Optional ByRef v_lInsuranceFileCnt As Integer = 0,
                                            Optional ByVal vTransdetailTypeID As Object = Nothing,
                                            Optional ByVal v_lSubBranchID As Object = Nothing) As Integer

        Dim nResult As Integer = 0
        Const kMethodName As String = "AddDocumentTransactions"
        Dim nTransDetailID As Integer = 0
        Dim nLedgerID As Integer = 0
        Dim nPeriodID As Integer = 0
        Dim bTransactionStarted As Boolean = False
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue
            ' if no document reference has been supplied
            If v_sDocumentRef = "" Then
                ' get document reference number
                m_lReturn = CType(GetDocumentReference(v_lDocumentTypeId, v_vDocSourceID, v_sDocumentRef),
                    gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetDocumentReference Failed",
                                            gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            ' Begin the transaction
            m_oDatabase.SQLBeginTrans()
            bTransactionStarted = True

            If Not True Then
                m_lReturn = m_oDocument.DirectAdd(vDocumentID:=r_vDocumentID,
                                                  vCompanyID:=v_vDocSourceID,
                                                  vPostingstatusID:=gACTLibrary.ACTPostStatusPosted,
                                                  vDocumenttypeID:=v_lDocumentTypeId,
                                                  vDocumentRef:=v_sDocumentRef,
                                                  vDocumentDate:=v_dtDocumentDate,
                                                  vComment:=v_sComment,
                                                  vCreatedDate:=DateTime.Now,
                                                  vAuthorisedDate:=DateTime.Now)
            Else
                m_lReturn = m_oDocument.DirectAdd(vDocumentID:=r_vDocumentID,
                                                  vCompanyID:=v_vDocSourceID,
                                                  vPostingstatusID:=gACTLibrary.ACTPostStatusPosted,
                                                  vDocumenttypeID:=v_lDocumentTypeId,
                                                  vDocumentRef:=v_sDocumentRef, vDocumentDate:=v_dtDocumentDate,
                                                  vComment:=v_sComment, vCreatedDate:=DateTime.Now,
                                                  vAuthorisedDate:=DateTime.Now,
                                                  vInsuranceFileCnt:=v_lInsuranceFileCnt,
                                                  vSubBranchID:=v_lSubBranchID)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bACTDocument.Form.DirectAdd Failed",
                                        gPMConstants.PMELogLevel.PMLogError)
            End If

            For nRow As Integer = 0 To v_vTransArray.GetUpperBound(0)

                'get Ledger Id for the Account

                m_lReturn = CType(GetLedgerIdForAccountId(CStr(v_vTransArray(nRow, 0)), nLedgerID),
                    gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetLedgerIdForAccountId Failed",
                                            gPMConstants.PMELogLevel.PMLogError)
                End If

                'get Period Id
                'DC080806 change from today to document date
                m_lReturn = CType(GetPeriodIdForDate(nPeriodID, v_dtDocumentDate, nLedgerID), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetPeriodIdForDate Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                'DC080806 change date from today to document date

                m_lReturn = m_oTransDetail.DirectAdd(vCompanyID:=v_vDocSourceID,
                                                     vDocumentID:=r_vDocumentID, vTransdetailID:=nTransDetailID,
                                                     vDocumentSequence:=nRow + 1,
                                                     vPostingstatusID:=gACTLibrary.ACTPostStatusPosted,
                                                     vAccountingDate:=v_dtDocumentDate,
                                                     vOperatorID:=v_vOperatorID, vFullyMatched:=0,
                                                     vAccountID:=v_vTransArray(nRow, 0),
                                                     vCurrencyID:=v_vTransArray(nRow, 2),
                                                     vPeriodID:=nPeriodID,
                                                     vCurrencyAmount:=gPMFunctions.ToSafeCurrency(v_vTransArray(nRow, 4)),
                                                     vCurrencyBaseXrate:=gPMFunctions.ToSafeDecimal(v_vTransArray(nRow, 5)),
                                                     vAmount:=ToSafeCurrency(v_vTransArray(nRow, 6)),
                                                     vReference:=v_vTransArray(nRow, 7),
                                                     vComment:=v_vTransArray(nRow, 8), vRefDate:=v_dtDocumentDate,
                                                     vUnderwritingYearID:=v_vTransArray(nRow, 9),
                                                     vDepartmentID:=v_vTransArray(nRow, 11),
                                                     vInsuranceRef:=v_vTransArray(nRow, 13),
                                                     vPurchaseOrderNo:=v_vTransArray(nRow, 14),
                                                     vPurchaseInvoiceNo:=v_vTransArray(nRow, 15),
                                                     vTransdetailTypeID:=vTransdetailTypeID)

                If (v_vTransArray.GetUpperBound(1) > 15 AndAlso v_vTransArray(nRow, 16) > 0 AndAlso nTransDetailID > 0) Then
                    m_lReturn = UpdateManualJournalDetail(v_vTransArray(nRow, 16), nTransDetailID)
                End If


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "m_oTransDetail.DirectAdd Failed",
                                            gPMConstants.PMELogLevel.PMLogError)
                End If
            Next
            ' End the transaction
            m_oDatabase.SQLCommitTrans()
            bTransactionStarted = False
            Return nResult
        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult)
            If bTransactionStarted Then
                ' roll the transaction back
                m_oDatabase.SQLRollbackTrans()
            End If
            ' return false
            nResult = gPMConstants.PMEReturnCode.PMFalse
            Return nResult
        End Try
    End Function
    Private Function UpdateManualJournalDetail(ByVal ManualJournalDetail_Id As Integer, ByVal TransDetail_id As Integer) As Integer
        Dim lReturn As Integer = 0

        lReturn = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the ID parameter (INPUT)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="ManualJournalDetail_Id", vValue:=CStr(ManualJournalDetail_Id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="TransDetail_id", vValue:=CStr(TransDetail_id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_update_ManualJournalDetail", sSQLName:="UpdateManualJournalwithTransdetail", bStoredProcedure:=True, lNumberRecords:=0)

        Return lReturn

    End Function
End Class

