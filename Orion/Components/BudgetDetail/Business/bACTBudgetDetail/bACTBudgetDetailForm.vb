Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Form_NET.Form")> _
Public NotInheritable Class Form
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 27/10/1998
    '
    ' Description: Creatable Form class which contains all the
    '              methods, Form rules required to manipulate
    '              a BudgetDetail.
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

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Collection of BudgetDetails (Private)
    Private m_oBudgetDetails As bACTBudgetDetail.BudgetDetails

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

    ' Primary Keys to work with
    ' Source ID
    ' Insurance File ID
    Private m_lInsuranceFileID As Integer
    ' Risk ID
    Private m_lRiskID As Integer

    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business

    ' Component Services

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
        End Get
    End Property


    Public Property CurrentRecord() As Integer
        Get

            Return m_lCurrentRecord

        End Get
        Set(ByVal Value As Integer)

            Select Case Value
                Case Is < 1
                    m_lCurrentRecord = 0
                Case Is > m_oBudgetDetails.Count()
                    m_lCurrentRecord = m_oBudgetDetails.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Numner in Collection
            Return m_oBudgetDetails.Count()

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

    Public ReadOnly Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
    End Property

    Public Property SourceID() As Integer
        Get

            Return m_iSourceID

        End Get
        Set(ByVal Value As Integer)

            m_iSourceID = Value

        End Set
    End Property

    Public Property InsuranceFileID() As Integer
        Get

            Return m_lInsuranceFileID

        End Get
        Set(ByVal Value As Integer)

            m_lInsuranceFileID = Value

        End Set
    End Property

    Public Property RiskID() As Integer
        Get

            Return m_lRiskID

        End Get
        Set(ByVal Value As Integer)

            m_lRiskID = Value

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


            ' Initialisation Code.

            ' Set Username and Password

            ' Set UserID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            ' Have we a valid Database Object Reference?

            If (Not Information.IsNothing(vDatabase)) And (Information.IsReference(vDatabase)) Then
                ' Yes, so use it.
                m_oDatabase = vDatabase

                ' Do NOT Close Database in Terminate() method
                m_bCloseDatabase = False
            Else
                ' NO, Create new instance of the database object
                m_oDatabase = New dPMDAO.Database()

                ' Open the Database
                'SD 23/07/2002 extra paramaters
                m_lReturn = m_oDatabase.OpenDatabase(sSiriusUsername:=m_sUsername, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, sCallingAppName:=ACApp, vDSN:=gPMConstants.PMOrionDSN)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Close Database in Terminate() method
                m_bCloseDatabase = True
            End If

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Set the ProcessMode to Generic
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric

            ' Set the Type Of Business to New Business
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric

            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now

            ' Default RiskID to 0 (So we will work at Ins File Level by default)
            m_lRiskID = 0

            ' Create BudgetDetails Collection
            m_oBudgetDetails = New bACTBudgetDetail.BudgetDetails()


            ' Create PM Lookup Business Object
            m_oLookup = New bPMLookup.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFOrion

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
                m_oBudgetDetails = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetBudgetRef
    '
    ' Description: Gets the budget reference from the budget_id
    '
    ' ***************************************************************** '
    Public Function GetBudgetRef(ByVal v_vBudgetID As Object, ByRef r_sBudgetRef As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' construct sql

            sSQL = "SELECT budget_ref FROM budget WHERE budget_id = " & CStr(v_vBudgetID)

            ' perform the select statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetBudgetRef", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' grab the return value
            If Information.IsArray(vResultArray) Then

                r_sBudgetRef = CStr(vResultArray(0, 0))
            Else
                r_sBudgetRef = ""
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBudgetRefFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBudgetRef", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a BudgetDetail.
    '
    '
    ' ***************************************************************** '
    'Developer guide no. 101
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim oBudgetDetail As bACTBudgetDetail.BudgetDetail
        Dim dtEffectiveDate As Date

        ' {* USER DEFINED CODE (Begin) *}
        Dim vTabArray(3, 1) As Object
        ' {* USER DEFINED CODE (End) *}

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            'developer guide no. 12
            vResultArray = Nothing
            ' Reset Table Array

            'developer guide no. 12
            vTableArray = Nothing

            ' {* USER DEFINED CODE (Begin) *}

            ' Setup Lookup Table Names
            '    vTabArray(PMLookupTableName, 0) = PMLookupEventRepeatType
            '    vTabArray(PMLookupTableName, 1) = PMLookupEventType

            ' {* USER DEFINED CODE (End) *}

            ' Do we have any records
            If m_lCurrentRecord < 1 Then
                ' No, we can only lookup all
                iLookupType = gPMConstants.PMELookupType.PMLookupAll
            Else
                ' Yes get current record
                oBudgetDetail = m_oBudgetDetails.Item(m_lCurrentRecord)
            End If

            Select Case iLookupType
                Case gPMConstants.PMELookupType.PMLookupAll

                    ' Do not supply a key

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = ""
                    ' Default Effective Date to current date/time
                    dtEffectiveDate = DateTime.Now

                Case gPMConstants.PMELookupType.PMLookupAllEffective

                    ' Use keys and effective date from current record
                    ' Note: The keys are not used for the select, but are used by
                    '       the iterface program to set the list index.
                    With oBudgetDetail

                        ' {* USER DEFINED CODE (Begin) *}
                        '            vTabArray(PMLookupKey, 0) = .RepeatTypeID
                        '            vTabArray(PMLookupKey, 1) = .EventTypeID
                        '            dtEffectiveDate = .EffectiveDate
                        ' {* USER DEFINED CODE (End) *}

                    End With

                Case gPMConstants.PMELookupType.PMLookupSingle

                    ' Set keys from current record
                    With oBudgetDetail

                        ' {* USER DEFINED CODE (Begin) *}
                        '            vTabArray(PMLookupKey, 0) = .RepeatTypeID
                        '            vTabArray(PMLookupKey, 1) = .EventTypeID
                        ' {* USER DEFINED CODE (End) *}

                    End With
                    ' Default Effective Date to current date/time
                    dtEffectiveDate = DateTime.Now

            End Select

            ' Release BudgetDetail reference
            oBudgetDetail = Nothing

            ' Get the Lookup items
            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTabArray, iLanguageID:=iLanguageID, dtEffectiveDate:=dtEffectiveDate, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the Table Array

            vTableArray = vTabArray

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single BudgetDetail directly into the database.
    '        Note: The BudgetDetail will NOT be added to the collection.
    '
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByRef vBudgetDetailID As Integer = 0, Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetSequence As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vBudgetAmount As Object = Nothing, Optional ByRef vActualAmount As Object = Nothing, Optional ByRef vVarianceAmount As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oBudgetDetail As bACTBudgetDetail.BudgetDetail

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new BudgetDetail
            oBudgetDetail = New bACTBudgetDetail.BudgetDetail()

            ' Populate BudgetDetail Attributes







            m_lReturn = SetProperties(oBudgetDetail, gPMConstants.PMEComponentAction.PMAdd, vBudgetDetailID:=vBudgetDetailID, vBudgetID:=CInt(vBudgetID), vBudgetSequence:=CInt(vBudgetSequence), vPeriodID:=CInt(vPeriodID), vAccountID:=CInt(vAccountID), vBudgetAmount:=CDec(vBudgetAmount), vActualAmount:=CDec(vActualAmount), vVarianceAmount:=CDec(vVarianceAmount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the BudgetDetail to the Database
            m_lReturn = AddItem(oBudgetDetail)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}

            ' Return the ID of the BudgetDetail Added

            If Not Information.IsNothing(vBudgetDetailID) Then
                vBudgetDetailID = oBudgetDetail.BudgetDetailID
            End If

            ' {* USER DEFINED CODE (End) *}

            oBudgetDetail = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a single BudgetDetail directly from the database.
    '        Note: The BudgetDetail will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByRef vBudgetDetailID As Object = Nothing, Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetSequence As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vBudgetAmount As Object = Nothing, Optional ByRef vActualAmount As Object = Nothing, Optional ByRef vVarianceAmount As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oBudgetDetail As bACTBudgetDetail.BudgetDetail

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new BudgetDetail
            oBudgetDetail = New bACTBudgetDetail.BudgetDetail()

            ' Populate BudgetDetail Attributes








            m_lReturn = SetProperties(oBudgetDetail, gPMConstants.PMEComponentAction.PMDelete, vBudgetDetailID:=CInt(vBudgetDetailID), vBudgetID:=CInt(vBudgetID), vBudgetSequence:=CInt(vBudgetSequence), vPeriodID:=CInt(vPeriodID), vAccountID:=CInt(vAccountID), vBudgetAmount:=CDec(vBudgetAmount), vActualAmount:=CDec(vActualAmount), vVarianceAmount:=CDec(vVarianceAmount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the BudgetDetail to the Database
            m_lReturn = DeleteItem(oBudgetDetail)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oBudgetDetail = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the BudgetDetail.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vBudgetDetailID As Object = Nothing, Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetSequence As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vBudgetAmount As Object = Nothing, Optional ByRef vActualAmount As Object = Nothing, Optional ByRef vVarianceAmount As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults








            m_lReturn = DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vBudgetDetailID:=CByte(vBudgetDetailID), vBudgetID:=CByte(vBudgetID), vBudgetSequence:=CByte(vBudgetSequence), vPeriodID:=CByte(vPeriodID), vAccountID:=CByte(vAccountID), vBudgetAmount:=CByte(vBudgetAmount), vActualAmount:=CByte(vActualAmount), vVarianceAmount:=CByte(vVarianceAmount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckPosted
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function CheckPosted(ByRef r_bPosted As Boolean, ByVal v_lBudgetID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Construct the SQL
            sSQL = "SELECT budget_status_id FROM budget WHERE budget_id = " & v_lBudgetID

            ' Perform the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="CheckPosted", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check if the posting status id is 3, if it is then its posted
            If Information.IsArray(vResultArray) Then

                r_bPosted = (CStr(vResultArray(0, 0)).Trim() = "3")
            Else
                ' default to not posted if something weird happened
                r_bPosted = False
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckPostedFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckPosted", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckID (Public)
    '
    ' Description: Checks to see if the supplied ID is a valid record.
    '
    ' ***************************************************************** '
    Public Function CheckID(ByRef vID As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the ID parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="id", vValue:=CStr(vID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckIDSQL, sSQLName:=ACCheckIDName, bStoredProcedure:=ACCheckIDStored, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?
            If lRecordCount < 1 Then
                ' No record found
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetCaptions (Public)
    '
    ' Description: Get the requested caption fields for a record.
    '
    ' ***************************************************************** '
    Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, Optional ByRef vTable As Object = Nothing) As Integer

        Dim result As Integer = 0
        'developer guide no. 111
        Dim oFields As DataRow

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(vFieldArray) Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:="Parameter vFieldArray must be a Variant Array", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptions")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have a Table name
            'BB If (IsMissing(vTable) = False) Then

            ' Is this our table
            '    If (Trim$(vTable) <> PMTableBudgetDetail) Then

            '        GetCaptions = PMInvalidRequest


            '       Exit Function

            '    End If

            'End If

            ' Get the Captions ourself

            ' Check that this record exists
            m_lReturn = CheckID(vID:=vID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Resize the Temporary Results Array
            Dim vResults(vFieldArray.GetUpperBound(0)) As Object

            ' Get a reference to the Fields returned
            oFields = m_oDatabase.Records.Item(0).Fields()

            With oFields

                ' For Each Field requested
                For lSub As Integer = vFieldArray.GetLowerBound(0) To vFieldArray.GetUpperBound(0)

                    'AK 230702 - check for null value

                    If Not (Convert.IsDBNull(oFields(vFieldArray(lSub))) Or IsNothing(oFields(vFieldArray(lSub)))) Then
                        ' Store the results in the Temporary results array

                        vResults(lSub) = oFields(vFieldArray(lSub))
                    Else
                        Select Case oFields(vFieldArray(lSub)).Type
                            'Developer guide No. 47(no solutions)
                            'Case DbType.String, DbType.String, DbType.String, adLongVarWChar, DbType.String, DbType.String, adWChar

                            '	vResults(lSub) = ""
                            'Case DbType.Date, adDBDate

                            '	vResults(lSub) = -1
                            'Case Else

                            '	vResults(lSub) = 0
                        End Select
                    End If

                Next lSub

            End With

            ' Assign the results
            vResultArray = vResults

            ' Release the reference to the Fields
            oFields = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCaptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required BudgetDetails and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vBudgetDetailID As Object = Nothing, Optional ByRef vLockMode As gPMConstants.PMELockMode = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oBudgetDetail As bACTBudgetDetail.BudgetDetail

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oBudgetDetails.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Information.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vLockMode = gPMConstants.PMELockMode.PMNoLock
            End If

            ' Do we have a key

            If Not Information.IsNothing(vBudgetDetailID) Then

                ' Yes, Is the key valid

                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(CStr(vBudgetDetailID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vBudgetDetailID =" & CStr(vBudgetDetailID), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                    Return result

                End If

                ' Add the BudgetDetailID parameter (INPUT)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Budget_Detail_id", vValue:=CStr(vBudgetDetailID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDetailsSQL, sSQLName:=ACGetDetailsName, bStoredProcedure:=ACGetDetailsStored, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else

                ' No Key, Get All Records

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?

            If lRecordCount < 1 Then

                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound

            Else

                ' Yes, load them into the collection

                For lSub As Integer = 1 To lRecordCount

                    ' Create New BudgetDetail
                    oBudgetDetail = New bACTBudgetDetail.BudgetDetail()

                    m_lReturn = SetPropertiesFromDB(oBudgetDetail:=oBudgetDetail, lRecordNumber:=lSub)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    ' Add BudgetDetail to collection
                    m_lReturn = m_oBudgetDetails.Add(oNewBudgetDetail:=oBudgetDetail)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oBudgetDetail = Nothing

                Next lSub

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required BudgetDetails and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vBudgetDetailID As Object = Nothing, Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetSequence As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vBudgetAmount As Object = Nothing, Optional ByRef vActualAmount As Object = Nothing, Optional ByRef vVarianceAmount As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oBudgetDetail As bACTBudgetDetail.BudgetDetail
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oBudgetDetails.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oBudgetDetail = m_oBudgetDetails.Item(m_lCurrentRecord)

            ' Get the BudgetDetail Property Values








            m_lReturn = GetProperties(oBudgetDetail, iStatus, vBudgetDetailID:=CInt(vBudgetDetailID), vBudgetID:=CInt(vBudgetID), vBudgetSequence:=CInt(vBudgetSequence), vPeriodID:=CInt(vPeriodID), vAccountID:=CInt(vAccountID), vBudgetAmount:=CDec(vBudgetAmount), vActualAmount:=CDec(vActualAmount), vVarianceAmount:=CDec(vVarianceAmount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oBudgetDetail = Nothing


            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied BudgetDetail into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vBudgetDetailID As Object = Nothing, Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetSequence As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vBudgetAmount As Object = Nothing, Optional ByRef vActualAmount As Object = Nothing, Optional ByRef vVarianceAmount As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oBudgetDetail As bACTBudgetDetail.BudgetDetail

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If vBudgetDetailID <> 0 Then
                If m_oBudgetDetails.Count() <> (lRow - 1) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Create a new BudgetDetail
            oBudgetDetail = New bACTBudgetDetail.BudgetDetail()

            ' Populate BudgetDetail Attributes







            m_lReturn = SetProperties(oBudgetDetail, gPMConstants.PMEComponentAction.PMAdd, vBudgetDetailID:=vBudgetDetailID, vBudgetID:=CInt(vBudgetID), vBudgetSequence:=CInt(vBudgetSequence), vPeriodID:=CInt(vPeriodID), vAccountID:=CInt(vAccountID), vBudgetAmount:=CDec(vBudgetAmount), vActualAmount:=CDec(vActualAmount), vVarianceAmount:=CDec(vVarianceAmount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oBudgetDetail = Nothing
                Return result
            End If

            ' Add BudgetDetail to collection
            m_lReturn = m_oBudgetDetails.Add(oNewBudgetDetail:=oBudgetDetail)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oBudgetDetail = Nothing
                Return result
            End If

            oBudgetDetail = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the BudgetDetail
    '              specified and updates the BudgetDetail with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vBudgetDetailID As Object = Nothing, Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetSequence As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vBudgetAmount As Object = Nothing, Optional ByRef vActualAmount As Object = Nothing, Optional ByRef vVarianceAmount As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oBudgetDetail As bACTBudgetDetail.BudgetDetail
        Dim iStatus As gPMConstants.PMEComponentAction

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oBudgetDetails.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oBudgetDetail = m_oBudgetDetails.Item(lRow)

            ' Check the Status of the BudgetDetail

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oBudgetDetail.DatabaseStatus
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Leave Status as Add
                    iStatus = gPMConstants.PMEComponentAction.PMAdd
                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                    ' Error
                    result = gPMConstants.PMEReturnCode.PMFalse
                Case Else
                    ' Set Edit (Update) Status
                    iStatus = gPMConstants.PMEComponentAction.PMEdit
            End Select

            ' Update BudgetDetail Attributes








            m_lReturn = SetProperties(oBudgetDetail, iStatus, vBudgetDetailID:=CInt(vBudgetDetailID), vBudgetID:=CInt(vBudgetID), vBudgetSequence:=CInt(vBudgetSequence), vPeriodID:=CInt(vPeriodID), vAccountID:=CInt(vAccountID), vBudgetAmount:=CDec(vBudgetAmount), vActualAmount:=CDec(vActualAmount), vVarianceAmount:=CDec(vVarianceAmount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oBudgetDetail = Nothing
                Return result
            End If

            ' Release reference to BudgetDetail
            oBudgetDetail = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified BudgetDetail can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oBudgetDetail As bACTBudgetDetail.BudgetDetail

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oBudgetDetails.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oBudgetDetail = m_oBudgetDetails.Item(lRow)

            ' Check the Status of the BudgetDetail

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oBudgetDetail.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oBudgetDetail.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oBudgetDetail.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to BudgetDetail
            oBudgetDetail = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDelete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Loop round Collection
            For lSub As Integer = 1 To m_oBudgetDetails.Count()
                Select Case m_oBudgetDetails.Item(lSub).DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing
                    Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMDelete
                        result = gPMConstants.PMEReturnCode.PMDataChanged
                        Exit For
                End Select
            Next lSub

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Cancel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Update (Public)
    '
    ' Description: Loops round the collection, doing any required
    '              Adds, Deletes or Updates.
    '
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Dim lSub As Integer
        Dim oBudgetDetail As bACTBudgetDetail.BudgetDetail
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oBudgetDetails.Count()
                oBudgetDetail = m_oBudgetDetails.Item(lSub)


                Select Case oBudgetDetail.DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing

                    Case gPMConstants.PMEComponentAction.PMAdd

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = BeginTrans()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Add Item
                        m_lReturn = AddItem(oBudgetDetail)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMEdit

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = BeginTrans()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Update Item
                        m_lReturn = UpdateItem(oBudgetDetail)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMDelete

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = BeginTrans()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Delete Item
                        m_lReturn = DeleteItem(oBudgetDetail)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Release last reference
            oBudgetDetail = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CommitTrans()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Refresh the Collection Items D/B Status
                    lSub = 1

                    ' For each item in the collection
                    Do While lSub <= m_oBudgetDetails.Count()

                        ' With the item
                        With m_oBudgetDetails.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oBudgetDetails.Delete(lSub)

                                    ' Anything Else
                                Case Else
                                    ' Set Status to view
                                    .DatabaseStatus = gPMConstants.PMEComponentAction.PMView
                                    lSub += 1

                            End Select

                        End With

                    Loop

                Else

                    m_lReturn = RollbackTrans()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetailsForBudgetID
    '
    ' Description: Calls GetDetailsForBudgetID in bACTBudget
    '
    ' DD 01/08/2002: bACTBudgetControl is now defunct. Method now in
    '                bACTBudget.
    ' ***************************************************************** '
    Public Function GetDetailsForBudgetID(ByVal v_lBudgetID As Integer, ByRef r_vBudgetDetails As Object, Optional ByRef r_sAccountName As String = "") As Integer

        Dim result As Integer = 0
        Dim oBudgetControl As bACTBudget.Form
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'SD rename variable

            oBudgetControl = New bACTBudget.Form
            m_lReturn = oBudgetControl.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oBudgetControl.GetDetailsForBudgetID(v_lBudgetID:=v_lBudgetID, r_vBudgetDetails:=r_vBudgetDetails)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            oBudgetControl.Dispose()


            oBudgetControl = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetailsForBudgetIDFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetailsForBudgetID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetBudgetAccountName
    '
    ' Description: Gets the account_name for the given account_id
    '
    ' ***************************************************************** '
    Public Function GetBudgetAccountName(ByVal v_lAccountID As Object, ByRef r_sAccountName As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Construct the SQL

            sSQL = "SELECT account_name " & _
                   "FROM Account " & _
                   "WHERE account_id = " & CStr(v_lAccountID)

            ' Peform the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAccountName", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the return value
            If Information.IsArray(vResultArray) Then

                r_sAccountName = CStr(vResultArray(0, 0))
            Else
                r_sAccountName = ""
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBudgetAccountNameFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBudgetAccountName", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetPeriodsForYear
    '
    ' Description: Gets the names of the periods for the given year
    '
    ' ***************************************************************** '
    Public Function GetPeriodsForYear(ByVal v_sPeriodYearName As String, ByRef r_vPeriods(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Construct the SQL statement
            ' AMJ 23 07 2001    Added order by parameter to ensure periods shown in the correct order on screen
            sSQL = "SELECT period_name, period_id FROM period WHERE year_name = '" & v_sPeriodYearName & "' order by period_id"

            ' Perform the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetPeriodsForYear", bStoredProcedure:=False, vResultArray:=r_vPeriods)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPeriodsForYearFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPeriodsForYear", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: AddItem (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Private Function AddItem(ByRef oBudgetDetail As bACTBudgetDetail.BudgetDetail) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add BudgetDetailID as an OUTPUT param for an insert
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Budget_Detail_id", vValue:=CStr(oBudgetDetail.BudgetDetailID), idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the required INPUT parameters
        m_lReturn = AddInputParam(oBudgetDetail:=oBudgetDetail)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the ID of the record inserted
        oBudgetDetail.BudgetDetailID = m_oDatabase.Parameters.Item("Budget_Detail_id").Value

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdateItem (Public)
    '
    ' Description: UpdateItems a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Private Function UpdateItem(ByRef oBudgetDetail As bACTBudgetDetail.BudgetDetail) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add BudgetDetailID as an INPUT param for an update
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Budget_Detail_id", vValue:=CStr(oBudgetDetail.BudgetDetailID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the Input Parameters to the Parameteres collection
        m_lReturn = AddInputParam(oBudgetDetail:=oBudgetDetail)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' BB Not required by Orion
        ' Add Timestamp as an INPUT param for an update
        'm_lReturn& = m_oDatabase.Parameters.Add( _
        'sName:="timestamp", _
        'vValue:=oBudgetDetail.Timestamp, _
        'iDirection:=PMParamInput, _
        'iDataType:=PMBinary)

        'If (m_lReturn& <> PMTrue) Then
        '    UpdateItem = PMFalse
        '    Exit Function
        'End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Check to see that the record was updated OK

        If lRecordsAffected > 0 Then
            ' Updated No action required
        Else
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DeleteItem (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    '
    ' ***************************************************************** '
    Private Function DeleteItem(ByRef oBudgetDetail As bACTBudgetDetail.BudgetDetail) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the BudgetDetailID INPUT parameter
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Budget_Detail_id", vValue:=CStr(oBudgetDetail.BudgetDetailID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' If record wasn't deleted, error
        If lRecordsAffected > 0 Then
            ' Deleted, No action required
        Else
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DeleteBudgetDetails (Public)
    '
    ' Description: Deletes existing budget details from the database.
    '
    ' ***************************************************************** '
    Public Function DeleteBudgetDetails(ByRef vBudgetID As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the BudgetDetailID INPUT parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Budget_id", vValue:=CStr(vBudgetID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteBudgetDetailsSQL, sSQLName:=ACDeleteBudgetDetailsName, bStoredProcedure:=ACDeleteBudgetDetailsStored, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If record wasn't deleted, error
            If lRecordsAffected > 0 Then
                ' Deleted, No action required
            ElseIf (lRecordsAffected = 0) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            Else
                Return gPMConstants.PMEReturnCode.PMError
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteBudgetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetPropertiesFromDB (Private)
    '
    ' Description: Sets the supplied BudgetDetail properties from a database
    '              record.
    ' ***************************************************************** '
    Private Function SetPropertiesFromDB(ByRef oBudgetDetail As bACTBudgetDetail.BudgetDetail, ByRef lRecordNumber As Integer) As Integer

        Dim result As Integer = 0
        'Developer Guide No. 112
        Dim oFields As DataRow



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set oFields to refer to Field Collection for Record 1
        oFields = m_oDatabase.Records.Item(lRecordNumber - 1).Fields()

        ' Populate Base Details

        With oBudgetDetail

            .BudgetDetailID = oFields("budget_detail_id")
            .BudgetID = oFields("budget_id")
            .BudgetSequence = oFields("budget_sequence")
            .PeriodID = oFields("period_id")
            .AccountID = oFields("account_id")

            If Convert.IsDBNull(oFields("budget_amount")) Or IsNothing(oFields("budget_amount")) Then
                .BudgetAmount = 0
            Else
                .BudgetAmount = oFields("budget_amount")
            End If

            If Convert.IsDBNull(oFields("actual_amount")) Or IsNothing(oFields("actual_amount")) Then
                .ActualAmount = 0
            Else
                .ActualAmount = oFields("actual_amount")
            End If

            If Convert.IsDBNull(oFields("variance_amount")) Or IsNothing(oFields("variance_amount")) Then
                .VarianceAmount = 0
            Else
                .VarianceAmount = oFields("variance_amount")
            End If

            .DatabaseStatus = gPMConstants.PMEComponentAction.PMView

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Private)
    '
    ' Description: Sets the supplied BudgetDetail property values.
    '
    ' ***************************************************************** '
    Private Function SetProperties(ByRef oBudgetDetail As bACTBudgetDetail.BudgetDetail, ByRef iStatus As Integer, Optional ByRef vBudgetDetailID As Integer = 0, Optional ByRef vBudgetID As Integer = 0, Optional ByRef vBudgetSequence As Integer = 0, Optional ByRef vPeriodID As Integer = 0, Optional ByRef vAccountID As Integer = 0, Optional ByRef vBudgetAmount As Decimal = 0, Optional ByRef vActualAmount As Decimal = 0, Optional ByRef vVarianceAmount As Decimal = 0) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        ' If Add Mode
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            ' Check that All Mandatory Parameters have been supplied
            m_lReturn = CheckMandatory(vBudgetDetailID:=vBudgetDetailID, vBudgetID:=vBudgetID, vBudgetSequence:=vBudgetSequence, vPeriodID:=vPeriodID, vAccountID:=vAccountID, vBudgetAmount:=vBudgetAmount, vActualAmount:=vActualAmount, vVarianceAmount:=vVarianceAmount)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Default Any Missing Parameters
            m_lReturn = DefaultParameters(bDefaultAll:=False, vBudgetDetailID:=vBudgetDetailID, vBudgetID:=vBudgetID, vBudgetSequence:=vBudgetSequence, vPeriodID:=vPeriodID, vAccountID:=vAccountID, vBudgetAmount:=vBudgetAmount, vActualAmount:=vActualAmount, vVarianceAmount:=vVarianceAmount)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

        End If

        ' Validate Parameters
        m_lReturn = Validate(vBudgetDetailID:=vBudgetDetailID, vBudgetID:=vBudgetID, vBudgetSequence:=vBudgetSequence, vPeriodID:=vPeriodID, vAccountID:=vAccountID, vBudgetAmount:=vBudgetAmount, vActualAmount:=vActualAmount, vVarianceAmount:=vVarianceAmount)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        ' Set Data Changed Flag to False
        bDataChanged = False


        ' Set Property values.
        With oBudgetDetail


            If Not Information.IsNothing(vBudgetDetailID) Then
                If .BudgetDetailID <> vBudgetDetailID Then
                    .BudgetDetailID = vBudgetDetailID
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vBudgetID) Then
                If .BudgetID <> vBudgetID Then
                    .BudgetID = vBudgetID
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vBudgetSequence) Then
                If .BudgetSequence <> vBudgetSequence Then
                    .BudgetSequence = vBudgetSequence
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vPeriodID) Then
                If .PeriodID <> vPeriodID Then
                    .PeriodID = vPeriodID
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vAccountID) Then
                If .AccountID <> vAccountID Then
                    .AccountID = vAccountID
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vBudgetAmount) Then
                If .BudgetAmount <> vBudgetAmount Then
                    .BudgetAmount = vBudgetAmount
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vActualAmount) Then
                If .ActualAmount <> vActualAmount Then
                    .ActualAmount = vActualAmount
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vVarianceAmount) Then
                If .VarianceAmount <> vVarianceAmount Then
                    .VarianceAmount = vVarianceAmount
                    bDataChanged = True
                End If
            End If


            ' If we have changed one of the properties, update the status
            If bDataChanged Then
                .DatabaseStatus = iStatus
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetProperties (Private)
    '
    ' Description: Returns the supplied BudgetDetail property values.
    '
    ' ***************************************************************** '
    Private Function GetProperties(ByRef oBudgetDetail As bACTBudgetDetail.BudgetDetail, ByRef iStatus As Integer, Optional ByRef vBudgetDetailID As Integer = 0, Optional ByRef vBudgetID As Integer = 0, Optional ByRef vBudgetSequence As Integer = 0, Optional ByRef vPeriodID As Integer = 0, Optional ByRef vAccountID As Integer = 0, Optional ByRef vBudgetAmount As Decimal = 0, Optional ByRef vActualAmount As Decimal = 0, Optional ByRef vVarianceAmount As Decimal = 0) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set Property values.
        With oBudgetDetail


            If Not Information.IsNothing(vBudgetDetailID) Then
                vBudgetDetailID = .BudgetDetailID
            End If


            If Not Information.IsNothing(vBudgetID) Then
                vBudgetID = .BudgetID
            End If


            If Not Information.IsNothing(vBudgetSequence) Then
                vBudgetSequence = .BudgetSequence
            End If


            If Not Information.IsNothing(vPeriodID) Then
                vPeriodID = .PeriodID
            End If


            If Not Information.IsNothing(vAccountID) Then
                vAccountID = .AccountID
            End If


            If Not Information.IsNothing(vBudgetAmount) Then
                vBudgetAmount = .BudgetAmount
            End If


            If Not Information.IsNothing(vActualAmount) Then
                vActualAmount = .ActualAmount
            End If


            If Not Information.IsNothing(vVarianceAmount) Then
                vVarianceAmount = .VarianceAmount
            End If


            iStatus = .DatabaseStatus

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddInputParam (Private)
    '
    ' Description: Adds all of the INPUT parameters
    '              required for an Insert or Update.
    '
    ' ***************************************************************** '
    Private Function AddInputParam(ByRef oBudgetDetail As bACTBudgetDetail.BudgetDetail) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            '        m_lReturn& = .Parameters.Add( _
            ''              sName:="budget_detail_id", _
            ''              vValue:=oBudgetDetail.BudgetDetailID, _
            ''              iDirection:=PMParamInput, _
            ''              iDataType:=PMLong)
            '
            '        If (m_lReturn& <> PMTrue) Then
            '            AddInputParam = PMFalse
            '            Exit Function
            '        End If

            m_lReturn = .Parameters.Add(sName:="budget_id", vValue:=CStr(oBudgetDetail.BudgetID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="budget_sequence", vValue:=CStr(oBudgetDetail.BudgetSequence), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="period_id", vValue:=CStr(oBudgetDetail.PeriodID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="account_id", vValue:=CStr(oBudgetDetail.AccountID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oBudgetDetail.BudgetAmount = 0 Then

                'Developer Guide No. 85
                m_lReturn = .Parameters.Add(sName:="budget_amount", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="budget_amount", vValue:=CStr(oBudgetDetail.BudgetAmount), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oBudgetDetail.ActualAmount = 0 Then

                'Developer guide  No. 85
                m_lReturn = .Parameters.Add(sName:="actual_amount", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="actual_amount", vValue:=CStr(oBudgetDetail.ActualAmount), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oBudgetDetail.VarianceAmount = 0 Then

                'Developer Guide No. 85
                m_lReturn = .Parameters.Add(sName:="variance_amount", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="variance_amount", vValue:=CStr(oBudgetDetail.VarianceAmount), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DefaultParameters (Public)
    '
    ' Description: Sets the Default Values for a BudgetDetail.
    '
    ' ***************************************************************** '
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vBudgetDetailID As Object = Nothing, Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetSequence As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vBudgetAmount As Object = Nothing, Optional ByRef vActualAmount As Object = Nothing, Optional ByRef vVarianceAmount As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Information.IsNothing(vBudgetDetailID)) Or (vBudgetDetailID.Equals(0)) Or (bDefaultAll) Then
            vBudgetDetailID = 0
        End If



        If (Information.IsNothing(vBudgetID)) Or (vBudgetID.Equals(0)) Or (bDefaultAll) Then
            vBudgetID = 0
        End If



        If (Information.IsNothing(vBudgetSequence)) Or (vBudgetSequence.Equals(0)) Or (bDefaultAll) Then
            vBudgetSequence = 0
        End If



        If (Information.IsNothing(vPeriodID)) Or (vPeriodID.Equals(0)) Or (bDefaultAll) Then
            vPeriodID = 0
        End If



        If (Information.IsNothing(vAccountID)) Or (vAccountID.Equals(0)) Or (bDefaultAll) Then
            vAccountID = 0
        End If



        If (Information.IsNothing(vBudgetAmount)) Or (vBudgetAmount.Equals(0)) Or (bDefaultAll) Then
            vBudgetAmount = 0
        End If



        If (Information.IsNothing(vActualAmount)) Or (vActualAmount.Equals(0)) Or (bDefaultAll) Then
            vActualAmount = 0
        End If



        If (Information.IsNothing(vVarianceAmount)) Or (vVarianceAmount.Equals(0)) Or (bDefaultAll) Then
            vVarianceAmount = 0
        End If


        ' {* USER DEFINED CODE (End) *}


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Public)
    '
    ' Description: Sets the Default Values for a BudgetDetail.
    '
    ' ***************************************************************** '
    Private Function CheckMandatory(Optional ByRef vBudgetDetailID As Object = Nothing, Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetSequence As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vBudgetAmount As Object = Nothing, Optional ByRef vActualAmount As Object = Nothing, Optional ByRef vVarianceAmount As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Information.IsNothing(vBudgetDetailID)) Or (Object.Equals(vBudgetDetailID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Information.IsNothing(vBudgetID)) Or (Object.Equals(vBudgetID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Information.IsNothing(vBudgetSequence)) Or (Object.Equals(vBudgetSequence, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Information.IsNothing(vPeriodID)) Or (Object.Equals(vPeriodID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Information.IsNothing(vAccountID)) Or (Object.Equals(vAccountID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Information.IsNothing(vBudgetAmount)) Or (Object.Equals(vBudgetAmount, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        '    If (IsMissing(vActualAmount) = True) _
        ''    Or (IsEmpty(vActualAmount) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If
        '
        '    If (IsMissing(vVarianceAmount) = True) _
        ''    Or (IsEmpty(vVarianceAmount) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If


        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the BudgetDetail for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vBudgetDetailID As Object = Nothing, Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetSequence As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vBudgetAmount As Object = Nothing, Optional ByRef vActualAmount As Object = Nothing, Optional ByRef vVarianceAmount As Object = Nothing) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        Dim dbNumericTemp As Double
        If Not Double.TryParse(CStr(vBudgetDetailID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp2 As Double
        If Not Double.TryParse(CStr(vBudgetID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp3 As Double
        If Not Double.TryParse(CStr(vBudgetSequence), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp4 As Double
        If Not Double.TryParse(CStr(vPeriodID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp5 As Double
        If Not Double.TryParse(CStr(vAccountID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp6 As Double
        If Not Double.TryParse(CStr(vBudgetAmount), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '    If (IsNumeric(vActualAmount) = False) Then
        '        Validate = PMFalse
        '        Exit Function
        '    End If
        '
        '    If (IsNumeric(vVarianceAmount) = False) Then
        '        Validate = PMFalse
        '        Exit Function
        '    End If


        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Private Function BeginTrans() As Integer

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



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Private Function CommitTrans() As Integer

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



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Private Function RollbackTrans() As Integer

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



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PRIVATE Methods (End)


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
        ' Error.
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

    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
