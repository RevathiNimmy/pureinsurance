Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
'developer guide no.129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Form_NET.Form")> _
Public NotInheritable Class Form
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 20/10/1998
    '
    ' Description: Creatable Form class which contains all the
    '              methods, Form rules required to manipulate
    '              a ACTBudget.
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

    ' Collection of ACTBudgets (Private)
    Private m_oACTBudgets As bACTBudget.ACTBudgets

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
                Case Is > m_oACTBudgets.Count()
                    m_lCurrentRecord = m_oACTBudgets.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Numner in Collection
            Return m_oACTBudgets.Count()

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

            '    ' Have we a valid Database Object Reference?
            '    If (IsMissing(vDatabase) = False) _
            ''    And (IsObject(vDatabase) = True) Then
            '        ' Yes, so use it.
            '        Set m_oDatabase = vDatabase
            '
            '        ' Do NOT Close Database in Terminate() method
            '        m_bCloseDatabase = False
            '    Else
            ' NO, Create new instance of the database object
            m_oDatabase = New dPMDAO.Database()

            ' Open the Database
            'SD 23/07/2002 Extra parameters
            m_lReturn = m_oDatabase.OpenDatabase(sSiriusUsername:=m_sUsername, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, sCallingAppName:=ACApp, vDSN:=gPMConstants.PMOrionDSN)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Close Database in Terminate() method
            m_bCloseDatabase = True
            '    End If

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

            ' Create ACTBudgets Collection
            m_oACTBudgets = New bACTBudget.ACTBudgets()

            ' Create an instance of component services

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
            If disposing Then
                m_oACTBudgets = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
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
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single ACTBudget directly into the database.
    '        Note: The ACTBudget will NOT be added to the collection.
    '
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByRef vBudgetID As Integer = 0, Optional ByRef vBudgetRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vBudgetDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesBudgetID As Object = Nothing, Optional ByRef vBudgetStatusID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oACTBudget As bACTBudget.ACTBudget

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new ACTBudget
            oACTBudget = New bACTBudget.ACTBudget()

            ' Populate ACTBudget Attributes






            'developer guide no.98
            m_lReturn = SetProperties(oACTBudget, gPMConstants.PMEComponentAction.PMAdd, _
                 vBudgetID:=vBudgetID, _
                 vBudgetRef:=vBudgetRef, _
                 vPeriodID:=vPeriodID, _
                 vBudgetDescription:=vBudgetDescription, _
                 vPeriodYearName:=vPeriodYearName, _
                 vRevisesBudgetID:=vRevisesBudgetID, _
                 vBudgetStatusID:=vBudgetStatusID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the ACTBudget to the Database
            m_lReturn = AddItem(oACTBudget)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}

            ' Return the ID of the ACTBudget Added

            If Not Information.IsNothing(vBudgetID) Then
                vBudgetID = oACTBudget.BudgetID
            End If

            ' {* USER DEFINED CODE (End) *}

            oACTBudget = Nothing

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
    ' Description: Deletes a single ACTBudget directly from the database.
    '        Note: The ACTBudget will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vBudgetDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesBudgetID As Object = Nothing, Optional ByRef vBudgetStatusID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oACTBudget As bACTBudget.ACTBudget

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new ACTBudget
            oACTBudget = New bACTBudget.ACTBudget()

            ' Populate ACTBudget Attributes







            'developer guide no.98
            m_lReturn = SetProperties(oACTBudget, gPMConstants.PMEComponentAction.PMDelete, _
                vBudgetID:=vBudgetID, _
                vBudgetRef:=vBudgetRef, _
                vPeriodID:=vPeriodID, _
                vBudgetDescription:=vBudgetDescription, _
                vPeriodYearName:=vPeriodYearName, _
                vRevisesBudgetID:=vRevisesBudgetID, _
                vBudgetStatusID:=vBudgetStatusID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the ACTBudget to the Database
            m_lReturn = DeleteItem(oACTBudget)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oACTBudget = Nothing

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
    ' Description: Returns the Default Values for the ACTBudget.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vBudgetDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesBudgetID As Object = Nothing, Optional ByRef vBudgetStatusID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults







            m_lReturn = DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vBudgetID:=CByte(vBudgetID), vBudgetRef:=CStr(vBudgetRef), vPeriodID:=CByte(vPeriodID), vBudgetDescription:=CStr(vBudgetDescription), vPeriodYearName:=CStr(vPeriodYearName), vRevisesBudgetID:=CByte(vRevisesBudgetID), vBudgetStatusID:=CByte(vBudgetStatusID))

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

            m_lReturn = m_oDatabase.Parameters.Add(sName:="id", vValue:=CStr(vID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

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
        'developer guide no .111
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
            '    If (Trim$(vTable) <> PMTableACTBudget) Then

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
                            'Full path to be provided for ADODB constants
                            'Case DbType.String, DbType.String, DbType.String, adLongVarWChar, DbType.String, DbType.String, adWChar
                            Case DbType.String, DbType.String, DbType.String, ADODB.DataTypeEnum.adLongVarWChar, DbType.String, DbType.String, ADODB.DataTypeEnum.adWChar

                                vResults(lSub) = ""
                                'Full path to be provided for ADODB constants
                                'Case DbType.Date, adDBDate
                            Case DbType.Date, ADODB.DataTypeEnum.adDBDate

                                vResults(lSub) = -1
                            Case Else

                                vResults(lSub) = 0
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
    ' Description: Gets the required ACTBudgets and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vLockMode As gPMConstants.PMELockMode = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oBudget As bACTBudget.ACTBudget

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oACTBudgets.Clear()

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

            If Not Information.IsNothing(vBudgetID) Then

                ' Yes, Is the key valid

                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(CStr(vBudgetID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vBudgetID =" & CStr(vBudgetID), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                    Return result

                End If

                ' Add the ACTBudgetID parameter (INPUT)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Budget_id", vValue:=CStr(vBudgetID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

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

                    ' Create New ACTBudget
                    oBudget = New bACTBudget.ACTBudget()
                    'developer guide no.162
                    m_lReturn = SetPropertiesFromDB(oACTBudget:=oBudget, lRecordNumber:=lSub - 1)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    ' Add ACTBudget to collection
                    m_lReturn = m_oACTBudgets.Add(oNewACTBudget:=oBudget)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oBudget = Nothing

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
    ' Description: Gets the required ACTBudgets and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vBudgetDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesBudgetID As Object = Nothing, Optional ByRef vBudgetStatusID As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oACTBudget As bACTBudget.ACTBudget
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oACTBudgets.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oACTBudget = m_oACTBudgets.Item(m_lCurrentRecord)

            ' Get the ACTBudget Property Values







            'developer guide no.98
            m_lReturn = GetProperties(oACTBudget, iStatus, _
                 vBudgetID:=vBudgetID, _
                 vBudgetRef:=vBudgetRef, _
                 vPeriodID:=vPeriodID, _
                 vBudgetDescription:=vBudgetDescription, _
                 vPeriodYearName:=vPeriodYearName, _
                 vRevisesBudgetID:=vRevisesBudgetID, _
                 vBudgetStatusID:=vBudgetStatusID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oACTBudget = Nothing


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
    ' Description: Adds the supplied ACTBudget into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vBudgetDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesBudgetID As Object = Nothing, Optional ByRef vBudgetStatusID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oACTBudget As bACTBudget.ACTBudget

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oACTBudgets.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new ACTBudget
            oACTBudget = New bACTBudget.ACTBudget()

            ' Populate ACTBudget Attributes







            'developer guide no.98
            m_lReturn = SetProperties(oACTBudget, gPMConstants.PMEComponentAction.PMAdd, _
                 vBudgetID:=vBudgetID, _
                 vBudgetRef:=vBudgetRef, _
                 vPeriodID:=vPeriodID, _
                 vBudgetDescription:=vBudgetDescription, _
                 vPeriodYearName:=vPeriodYearName, _
                 vRevisesBudgetID:=vRevisesBudgetID, _
                 vBudgetStatusID:=vBudgetStatusID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oACTBudget = Nothing
                Return result
            End If

            ' Add ACTBudget to collection
            m_lReturn = m_oACTBudgets.Add(oNewACTBudget:=oACTBudget)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oACTBudget = Nothing
                Return result
            End If

            oACTBudget = Nothing

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
    ' Description: Validates that this action is valid on the ACTBudget
    '              specified and updates the ACTBudget with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vBudgetDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesBudgetID As Object = Nothing, Optional ByRef vBudgetStatusID As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oACTBudget As bACTBudget.ACTBudget
        Dim iStatus As gPMConstants.PMEComponentAction

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oACTBudgets.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oACTBudget = m_oACTBudgets.Item(lRow)

            ' Check the Status of the ACTBudget

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oACTBudget.DatabaseStatus
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

            ' Update ACTBudget Attributes







            'developer guide no.98
            m_lReturn = SetProperties(oACTBudget, iStatus, _
                 vBudgetID:=vBudgetID, _
                 vBudgetRef:=vBudgetRef, _
                 vPeriodID:=vPeriodID, _
                 vBudgetDescription:=vBudgetDescription, _
                 vPeriodYearName:=vPeriodYearName, _
                 vRevisesBudgetID:=vRevisesBudgetID, _
                 vBudgetStatusID:=vBudgetStatusID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oACTBudget = Nothing
                Return result
            End If

            ' Release reference to ACTBudget
            oACTBudget = Nothing

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
    ' Description: Validate that the specified ACTBudget can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oACTBudget As bACTBudget.ACTBudget

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oACTBudgets.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oACTBudget = m_oACTBudgets.Item(lRow)

            ' Check the Status of the ACTBudget

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oACTBudget.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oACTBudget.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oACTBudget.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to ACTBudget
            oACTBudget = Nothing

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
            For lSub As Integer = 1 To m_oACTBudgets.Count()
                Select Case m_oACTBudgets.Item(lSub).DatabaseStatus
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
        Dim oACTBudget As bACTBudget.ACTBudget
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oACTBudgets.Count()
                oACTBudget = m_oACTBudgets.Item(lSub)


                Select Case oACTBudget.DatabaseStatus
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
                        m_lReturn = AddItem(oACTBudget)
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
                        m_lReturn = UpdateItem(oACTBudget)
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
                        m_lReturn = DeleteItem(oACTBudget)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Release last reference
            oACTBudget = Nothing

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
                    Do While lSub <= m_oACTBudgets.Count()

                        ' With the item
                        With m_oACTBudgets.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oACTBudgets.Delete(lSub)

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

    ' *********************************************************************** '
    ' Name: GetUniquePeriodYears
    '
    ' Description: Gets a unique list of period year names from the period
    '              table.
    '
    ' *********************************************************************** '
    Public Function GetUniquePeriodYears(ByRef r_vYearArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim oPeriod As bACTPeriod.Form

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the period business using component services
            'SD 23/07/2002 name change

            oPeriod = New bACTPeriod.Form
            m_lReturn = oPeriod.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a list of unique period year names

            m_lReturn = oPeriod.GetUniqueYears(r_vYearArray:=r_vYearArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If


            oPeriod.Dispose()


            oPeriod = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUniquePeriodYearsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUniquePeriodYears", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetailsForBudgetID
    '
    ' Description: Gets the details for the budget_id passed.
    '
    ' DD 01/08/2002: Moved here from defunct bACTBudgetControl
    ' ***************************************************************** '
    Public Function GetDetailsForBudgetID(ByVal v_lBudgetID As Integer, ByRef r_vBudgetDetails(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the database parameters
            m_oDatabase.Parameters.Clear()

            ' Add the input parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="budget_id", vValue:=CStr(v_lBudgetID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Call the stored procedure
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDetailForBudgetIDSQL, sSQLName:=ACGetDetailForBudgetIDName, bStoredProcedure:=ACGetDetailForBudgetIDStored, vResultArray:=r_vBudgetDetails)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetailsForBudgetID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetailsForBudgetID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetBudgetStatus
    '
    ' Description: Gets the string for the budget status from the
    '              postingstatus table
    '              NB: There is no postingstatus business object.
    '
    ' ***************************************************************** '
    Public Function GetBudgetStatus(ByVal v_lBudgetStatus As Integer, ByRef r_sDescription As String, ByRef r_vStatusList(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT description FROM PostingStatus WHERE postingstatus_id = " & v_lBudgetStatus

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetBudgetStatus", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vResultArray) Then

                r_sDescription = CStr(vResultArray(0, 0))
            End If

            sSQL = "SELECT * FROM PostingStatus"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetPostingStatus", bStoredProcedure:=False, vResultArray:=r_vStatusList)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBudgetStatusFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBudgetStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateActualsAndVariances
    '
    ' Description: updates the Actuals and Variances for the Period
    '
    ' DD 01/08/2002: Moved here from defunct bACTBudgetControl
    ' ***************************************************************** '
    Public Function UpdateActualsAndVariances(ByVal v_lPeriodID As Integer) As Integer

        Dim result As Integer = 0
        Dim vBudgetArray, vBudgetDetailArray(,) As Object
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            ' Add the input parameter
            '    m_lReturn& = m_oDatabase.Parameters.Add( _
            ''        sName:="period_id", _
            ''        vValue:=v_lPeriodID, _
            ''        iDirection:=PMParamInput, _
            ''        iDataType:=PMLong)
            '    If (m_lReturn& <> PMTrue) Then
            '        UpdateActualsAndVariances = PMFalse
            '        Exit Function
            '    End If

            sSQL = "SELECT DISTINCT budget_id FROM budget_detail WHERE period_id = " & v_lPeriodID

            ' ACGetBudgetsForYearSQL

            ' Perform the SQL statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetBudgetDetailsForPeriod", bStoredProcedure:=False, vResultArray:=vBudgetArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vBudgetArray) Then
                ' for each budget

                For iLoop1 As Integer = vBudgetArray.GetLowerBound(1) To vBudgetArray.GetUpperBound(1)

                    ' select all budgetdetails where budget_id = current_budget_id
                    ' CF220199 -

                    sSQL = "SELECT budget_detail_id FROM Budget_Detail WHERE Budget_id = " & CStr(vBudgetArray(0, iLoop1)) & " AND period_id = " & CStr(v_lPeriodID)

                    ' Perform the sql
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetBudgetDetail", bStoredProcedure:=False, vResultArray:=vBudgetDetailArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If Information.IsArray(vBudgetDetailArray) Then

                        ' for each budgetdetail

                        For iLoop2 As Integer = vBudgetDetailArray.GetLowerBound(1) To vBudgetDetailArray.GetUpperBound(1)

                            ' Clear the parameters
                            m_oDatabase.Parameters.Clear()

                            ' Add the new ones
                            m_lReturn = m_oDatabase.Parameters.Add(sName:="period_id", vValue:=CStr(v_lPeriodID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If


                            m_lReturn = m_oDatabase.Parameters.Add(sName:="budget_detail_id", vValue:=CStr(vBudgetDetailArray(0, iLoop2)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            ' Update the actuals and variances
                            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateActAndVarSQL, sSQLName:=ACUpdateActAndVarName, bStoredProcedure:=ACUpdateActAndVarStored)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                        Next iLoop2 ' next budgetdetail

                    End If

                Next iLoop1 ' next budget
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateActualsAndVariances Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateActualsAndVariances", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Private Function AddItem(ByRef oACTBudget As bACTBudget.ACTBudget) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters
        m_lReturn = AddInputParam(oACTBudget:=oACTBudget)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add ACTBudgetID as an OUTPUT param for an insert
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Budget_id", vValue:=CStr(oACTBudget.BudgetID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the ID of the record inserted
        oACTBudget.BudgetID = m_oDatabase.Parameters.Item("Budget_id").Value

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
    Private Function UpdateItem(ByRef oACTBudget As bACTBudget.ACTBudget) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the Input Parameters to the Parameteres collection
        m_lReturn = AddInputParam(oACTBudget:=oACTBudget)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add ACTBudgetID as an INPUT param for an update
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Budget_id", vValue:=CStr(oACTBudget.BudgetID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' BB Not required by Orion
        ' Add Timestamp as an INPUT param for an update
        'm_lReturn& = m_oDatabase.Parameters.Add( _
        'sName:="timestamp", _
        'vValue:=oACTBudget.Timestamp, _
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
    Private Function DeleteItem(ByRef oACTBudget As bACTBudget.ACTBudget) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the ACTBudgetID INPUT parameter
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Budget_id", vValue:=CStr(oACTBudget.BudgetID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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
    ' Name: SetPropertiesFromDB (Private)
    '
    ' Description: Sets the supplied ACTBudget properties from a database
    '              record.
    ' ***************************************************************** '
    Private Function SetPropertiesFromDB(ByRef oACTBudget As bACTBudget.ACTBudget, ByRef lRecordNumber As Integer) As Integer

        Dim result As Integer = 0
        'developer guide no.112
        Dim oFields As DataRow



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set oFields to refer to Field Collection for Record 1
        oFields = m_oDatabase.Records.Item(lRecordNumber).Fields()

        ' Populate Base Details

        With oACTBudget

            .BudgetID = oFields("budget_id")
            .BudgetRef = oFields("budget_ref")

            If Convert.IsDBNull(oFields("period_id")) Or IsNothing(oFields("period_id")) Then
                .PeriodID = 0
            Else
                .PeriodID = oFields("period_id")
            End If

            If Convert.IsDBNull(oFields("budget_description")) Or IsNothing(oFields("budget_description")) Then
                .BudgetDescription = ""
            Else
                .BudgetDescription = oFields("budget_description")
            End If

            If Convert.IsDBNull(oFields("period_year_name")) Or IsNothing(oFields("period_year_name")) Then
                .PeriodYearName = ""
            Else
                .PeriodYearName = oFields("period_year_name")
            End If

            If Convert.IsDBNull(oFields("revises_budget_id")) Or IsNothing(oFields("revises_budget_id")) Then
                .RevisesBudgetID = 0
            Else
                .RevisesBudgetID = oFields("revises_budget_id")
            End If
            .BudgetStatusID = oFields("budget_status_id")

            .DatabaseStatus = gPMConstants.PMEComponentAction.PMView

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Private)
    '
    ' Description: Sets the supplied ACTBudget property values.
    '
    ' ***************************************************************** '
    Private Function SetProperties(ByRef oACTBudget As bACTBudget.ACTBudget, ByRef iStatus As Integer, Optional ByRef vBudgetID As Integer = 0, Optional ByRef vBudgetRef As String = "", Optional ByRef vPeriodID As Integer = 0, Optional ByRef vBudgetDescription As String = "", Optional ByRef vPeriodYearName As String = "", Optional ByRef vRevisesBudgetID As Integer = 0, Optional ByRef vBudgetStatusID As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        ' If Add Mode
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            ' Check that All Mandatory Parameters have been supplied
            m_lReturn = CheckMandatory(vBudgetID:=vBudgetID, vBudgetRef:=vBudgetRef, vPeriodID:=vPeriodID, vBudgetDescription:=vBudgetDescription, vPeriodYearName:=vPeriodYearName, vRevisesBudgetID:=vRevisesBudgetID, vBudgetStatusID:=vBudgetStatusID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Default Any Missing Parameters
            m_lReturn = DefaultParameters(bDefaultAll:=False, vBudgetID:=vBudgetID, vBudgetRef:=vBudgetRef, vPeriodID:=vPeriodID, vBudgetDescription:=vBudgetDescription, vPeriodYearName:=vPeriodYearName, vRevisesBudgetID:=vRevisesBudgetID, vBudgetStatusID:=vBudgetStatusID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

        End If

        ' Validate Parameters
        m_lReturn = Validate(vBudgetID:=vBudgetID, vBudgetRef:=vBudgetRef, vPeriodID:=vPeriodID, vBudgetDescription:=vBudgetDescription, vPeriodYearName:=vPeriodYearName, vRevisesBudgetID:=vRevisesBudgetID, vBudgetStatusID:=vBudgetStatusID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        ' Set Data Changed Flag to False
        bDataChanged = False


        ' Set Property values.
        With oACTBudget


            If Not Information.IsNothing(vBudgetID) Then
                If .BudgetID <> vBudgetID Then
                    .BudgetID = vBudgetID
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vBudgetRef) Then
                If .BudgetRef.Trim() <> vBudgetRef.Trim() Then
                    .BudgetRef = vBudgetRef
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vPeriodID) Then
                If .PeriodID <> vPeriodID Then
                    .PeriodID = vPeriodID
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vBudgetDescription) Then
                If .BudgetDescription.Trim() <> vBudgetDescription.Trim() Then
                    .BudgetDescription = vBudgetDescription
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vPeriodYearName) Then
                If .PeriodYearName.Trim() <> vPeriodYearName.Trim() Then
                    .PeriodYearName = vPeriodYearName
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vRevisesBudgetID) Then
                If .RevisesBudgetID <> vRevisesBudgetID Then
                    .RevisesBudgetID = vRevisesBudgetID
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vBudgetStatusID) Then
                If .BudgetStatusID <> vBudgetStatusID Then
                    .BudgetStatusID = vBudgetStatusID
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
    ' Description: Returns the supplied ACTBudget property values.
    '
    ' ***************************************************************** '
    'developer guide no.101
    Private Function GetProperties(ByRef oACTBudget As bACTBudget.ACTBudget, ByRef iStatus As Integer, Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vBudgetDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesBudgetID As Object = Nothing, Optional ByRef vBudgetStatusID As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set Property values.
        With oACTBudget


            If Not Information.IsNothing(vBudgetID) Then
                vBudgetID = .BudgetID
            End If


            If Not Information.IsNothing(vBudgetRef) Then
                vBudgetRef = .BudgetRef
            End If


            If Not Information.IsNothing(vPeriodID) Then
                vPeriodID = .PeriodID
            End If


            If Not Information.IsNothing(vBudgetDescription) Then
                vBudgetDescription = .BudgetDescription
            End If


            If Not Information.IsNothing(vPeriodYearName) Then
                vPeriodYearName = .PeriodYearName
            End If


            If Not Information.IsNothing(vRevisesBudgetID) Then
                vRevisesBudgetID = .RevisesBudgetID
            End If


            If Not Information.IsNothing(vBudgetStatusID) Then
                vBudgetStatusID = .BudgetStatusID
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
    Private Function AddInputParam(ByRef oACTBudget As bACTBudget.ACTBudget) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase


            '        m_lReturn& = .Parameters.Add( _
            ''              sName:="budget_id", _
            ''              vValue:=oACTBudget.BudgetID, _
            ''              iDirection:=PMParamInput, _
            ''              iDataType:=PMLong)
            '
            '        If (m_lReturn& <> PMTrue) Then
            '            AddInputParam = PMFalse
            '            Exit Function
            '        End If

            m_lReturn = .Parameters.Add(sName:="budget_ref", vValue:=oACTBudget.BudgetRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oACTBudget.PeriodID < 1 Then

                'developer guide no.85
                m_lReturn = .Parameters.Add(sName:="period_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="period_id", vValue:=CStr(oACTBudget.PeriodID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="budget_description", vValue:=oACTBudget.BudgetDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="period_year_name", vValue:=oACTBudget.PeriodYearName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oACTBudget.RevisesBudgetID < 1 Then

                'developer guide no.85
                m_lReturn = .Parameters.Add(sName:="revises_budget_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="revises_budget_id", vValue:=CStr(oACTBudget.RevisesBudgetID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="budget_status_id", vValue:=CStr(oACTBudget.BudgetStatusID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DefaultParameters (Public)
    '
    ' Description: Sets the Default Values for a ACTBudget.
    '
    ' ***************************************************************** '
    'developer guide no.101
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vBudgetDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesBudgetID As Object = Nothing, Optional ByRef vBudgetStatusID As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        'developer guide no.44
        'start
        If (Information.IsNothing(vBudgetID)) OrElse (vBudgetID.Equals(0)) Or (bDefaultAll) Then
            vBudgetID = 0
        End If



        If (Information.IsNothing(vBudgetRef)) OrElse (String.IsNullOrEmpty(vBudgetRef)) Or (bDefaultAll) Then
            vBudgetRef = ""
        End If



        If (Information.IsNothing(vPeriodID)) OrElse (vPeriodID.Equals(0)) Or (bDefaultAll) Then
            vPeriodID = 0
        End If



        If (Information.IsNothing(vBudgetDescription)) OrElse (String.IsNullOrEmpty(vBudgetDescription)) Or (bDefaultAll) Then
            vBudgetDescription = ""
        End If



        If (Information.IsNothing(vPeriodYearName)) OrElse (String.IsNullOrEmpty(vPeriodYearName)) Or (bDefaultAll) Then
            vPeriodYearName = ""
        End If



        If (Information.IsNothing(vRevisesBudgetID)) OrElse (vRevisesBudgetID.Equals(0)) Or (bDefaultAll) Then
            vRevisesBudgetID = 0
        End If



        If (Information.IsNothing(vBudgetStatusID)) OrElse (vBudgetStatusID.Equals(0)) Or (bDefaultAll) Then
            vBudgetStatusID = 1 ' Registered
        End If
        'end

        ' {* USER DEFINED CODE (End) *}


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Public)
    '
    ' Description: Sets the Default Values for a ACTBudget.
    '
    ' ***************************************************************** '
    Private Function CheckMandatory(Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vBudgetDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesBudgetID As Object = Nothing, Optional ByRef vBudgetStatusID As Object = Nothing) As Integer

        Dim result As Integer = 0



        ' {* USER DEFINED CODE (Begin) *}

        '    If (IsMissing(vBudgetID) = True) _
        ''    Or (IsEmpty(vBudgetID) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If
        '
        '    If (IsMissing(vBudgetRef) = True) _
        ''    Or (IsEmpty(vBudgetRef) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If
        '
        '    If (IsMissing(vPeriodID) = True) _
        ''    Or (IsEmpty(vPeriodID) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If
        '
        '    If (IsMissing(vBudgetDescription) = True) _
        ''    Or (IsEmpty(vBudgetDescription) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If
        '
        '    If (IsMissing(vPeriodYearName) = True) _
        ''    Or (IsEmpty(vPeriodYearName) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If
        '
        '    If (IsMissing(vRevisesBudgetID) = True) _
        ''    Or (IsEmpty(vRevisesBudgetID) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If
        '
        '    If (IsMissing(vBudgetStatusID) = True) _
        ''    Or (IsEmpty(vBudgetStatusID) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If


        ' {* USER DEFINED CODE (End) *}

        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the ACTBudget for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vBudgetDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesBudgetID As Object = Nothing, Optional ByRef vBudgetStatusID As Object = Nothing) As Integer

        Dim result As Integer = 0




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        Dim dbNumericTemp As Double
        If Not Double.TryParse(CStr(vBudgetID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp2 As Double
        If Not Double.TryParse(CStr(vPeriodID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp3 As Double
        If Not Double.TryParse(CStr(vRevisesBudgetID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp4 As Double
        If Not Double.TryParse(CStr(vBudgetStatusID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

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
