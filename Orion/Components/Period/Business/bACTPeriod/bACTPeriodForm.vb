Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
Imports System.Text
'developer guide no 129
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Form_NET.Form")>
Public NotInheritable Class Form
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 31/07/1997
    '
    ' Description: Creatable Form class which contains all the
    '              methods, Form rules required to manipulate
    '              a Period.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 06/02/2004
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

    ' Collection of Periods (Private)
    Private m_oPeriods As bACTPeriod.Periods

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

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

    ' Primary Keys to work with
    ' Source ID
    Private m_lSourceID As Integer
    ' Insurance File ID
    Private m_lInsuranceFileID As Integer
    ' Risk ID
    Private m_lRiskID As Integer

    'Private collections of PeriodId's and YearNames looked up

    Private m_Periods As Dictionary(Of String, Date)
    Private m_PeriodYears As Dictionary(Of String, Date)

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
                Case Is > m_oPeriods.Count()
                    m_lCurrentRecord = m_oPeriods.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Numner in Collection
            Return m_oPeriods.Count()

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

            Return m_lSourceID

        End Get
        Set(ByVal Value As Integer)

            m_lSourceID = Value

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


            ' Initialisation Code.

            ' Set Username and Password

            ' Set UserID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            ' Get Reference to Database
            '    Set m_oDatabase = GetOrionDatabase( _
            ''                            lOpenStatus:=m_lReturn, _
            ''                            bCloseDatabase:=m_bCloseDatabase, _
            ''                            vDatabase:=vDatabase)
            '
            '    If (m_lReturn& <> PMTrue) Then
            '        Initialise = PMFalse
            '        Exit Function
            '    End If



            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
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

            ' Create Periods Collection
            m_oPeriods = New bACTPeriod.Periods()

            ' Create Component Services

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
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
                m_oPeriods = Nothing
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



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single Period directly into the database.
    '        Note: The Period will NOT be added to the collection.
    '
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByVal vPeriodID As Integer = 0, Optional ByVal vCompanyID As Object = Nothing, Optional ByVal vSubBranchID As Object = Nothing, Optional ByVal vYearName As Object = Nothing, Optional ByVal vPeriodName As Object = Nothing, Optional ByVal vPeriodEndDate As Object = Nothing, Optional ByVal vPeriodEndComplete As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oPeriod As bACTPeriod.Period

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Period
            oPeriod = New bACTPeriod.Period()

            ' Populate Period Attributes





            'developer guide no.98
            m_lReturn = SetProperties(oPeriod, gPMConstants.PMEComponentAction.PMAdd,
                 vPeriodID:=vPeriodID,
                 vCompanyID:=vCompanyID,
                 vSubBranchID:=vSubBranchID,
                 vYearName:=vYearName,
                 vPeriodName:=vPeriodName,
                 vPeriodEndDate:=vPeriodEndDate,
                 vPeriodEndComplete:=vPeriodEndComplete)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Period to the Database
            m_lReturn = CType(AddItem(oPeriod), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}

            ' Return the ID of the Period Added

            'Developer Guide no 118
            'If Not Informations.IsNothing(vPeriodID) Then
            vPeriodID = oPeriod.PeriodID
            'End If

            ' {* USER DEFINED CODE (End) *}

            oPeriod = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a single Period directly from the database.
    '        Note: The Period will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vYearName As Object = Nothing, Optional ByRef vPeriodName As Object = Nothing, Optional ByRef vPeriodEndDate As Object = Nothing, Optional ByRef vPeriodEndComplete As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oPeriod As bACTPeriod.Period

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Period
            oPeriod = New bACTPeriod.Period()

            ' Populate Period Attributes

            'developer guide no.98
            m_lReturn = SetProperties(oPeriod, gPMConstants.PMEComponentAction.PMDelete,
                 vPeriodID:=vPeriodID,
                 vCompanyID:=vCompanyID,
                 vYearName:=vYearName,
                 vPeriodName:=vPeriodName,
                 vPeriodEndDate:=vPeriodEndDate,
                 vPeriodEndComplete:=vPeriodEndComplete)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Period to the Database
            m_lReturn = CType(DeleteItem(oPeriod), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oPeriod = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the Period.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vYearName As Object = Nothing, Optional ByRef vPeriodName As Object = Nothing, Optional ByRef vPeriodEndDate As Object = Nothing, Optional ByRef vPeriodEndComplete As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults

            'developer guide no.98
            m_lReturn = DefaultParameters(bDefaultAll:=True, vSubType:=vSubType,
                vPeriodID:=vPeriodID,
                vCompanyID:=vCompanyID,
                vYearName:=vYearName,
                vPeriodName:=vPeriodName,
                vPeriodEndDate:=vPeriodEndDate,
                vPeriodEndComplete:=vPeriodEndComplete)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetCaptions (Public)
    '
    ' Description: Get the requested caption fields for a record.
    '
    ' ***************************************************************** '
    Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object) As Integer
        Return GetCaptions(vID:=vID, vFieldArray:=vFieldArray, vResultArray:=vResultArray, vTable:=Nothing)
    End Function

    Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, ByRef vTable As Object) As Integer

        Dim result As Integer = 0
        Dim oFields As ADODB.Fields

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Informations.IsArray(vFieldArray) Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:="Parameter vFieldArray must be a Variant Array", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptions")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have a Table name
            'BB If (IsMissing(vTable) = False) Then

            ' Is this our table
            '    If (Trim$(vTable) <> PMTablePeriod) Then

            '        GetCaptions = PMInvalidRequest


            '       Exit Function

            '    End If

            'End If

            ' Get the Captions ourself

            ' Check that this record exists
            m_lReturn = CType(CheckID(vID:=vID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Resize the Temporary Results Array
            Dim vResults(vFieldArray.GetUpperBound(0)) As Object

            ' Get a reference to the Fields returned
            oFields = m_oDatabase.Records.Item(1).Fields()

            With oFields

                ' For Each Field requested
                For lSub As Integer = vFieldArray.GetLowerBound(0) To vFieldArray.GetUpperBound(0)

                    'AK 230702 - scalability change - check for null value

                    If Not (Convert.IsDBNull(oFields(vFieldArray(lSub))) Or Informations.IsNothing(oFields(vFieldArray(lSub)))) Then
                        ' Store the results in the Temporary results array

                        vResults(lSub) = oFields(vFieldArray(lSub))
                    Else
                        Select Case oFields(vFieldArray(lSub)).Type
                            Case DbType.String, DbType.String, DbType.String, ADODB.DataTypeEnum.adLongVarWChar, DbType.String, DbType.String, ADODB.DataTypeEnum.adWChar

                                vResults(lSub) = ""
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCaptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required Periods and populate the Collection
    '
    ' DD 31/07/2002: Altered for multi-branch accounting
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vYearName As Object = Nothing, Optional ByRef vLockMode As Integer = 0, Optional ByRef vSubBranchID As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oPeriod As bACTPeriod.Period

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oPeriods.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Informations.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vLockMode = gPMConstants.PMELockMode.PMNoLock
            End If

            ' Do we have a key

            If Not Informations.IsNothing(vPeriodID) Then

                ' Yes, Is the key valid

                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(CStr(vPeriodID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vPeriodID =" & CStr(vPeriodID), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                    Return result

                End If

                ' Add the PeriodID parameter (INPUT)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Period_id", vValue:=CStr(vPeriodID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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

                ' Add the Company ID parameter
                m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add the Sub Branch ID parameter
                m_lReturn = m_oDatabase.Parameters.Add(sName:="sub_branch_id", vValue:=CStr(vSubBranchID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add the Year Name parameter

                m_lReturn = m_oDatabase.Parameters.Add(sName:="year_name", vValue:=CStr(vYearName), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


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
                'Developer Guide no 162
                For lSub As Integer = 0 To lRecordCount - 1

                    ' Create New Period
                    oPeriod = New bACTPeriod.Period()

                    m_lReturn = CType(SetPropertiesFromDB(oPeriod:=oPeriod, lRecordNumber:=lSub), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    ' Add Period to collection
                    If (m_oPeriods.Count = 0) Then
                        m_oPeriods.Add(Nothing)
                    End If
                    m_lReturn = CType(m_oPeriods.Add(oNewPeriod:=oPeriod), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oPeriod = Nothing

                Next lSub

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required Periods and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vYearName As Object = Nothing, Optional ByRef vPeriodName As Object = Nothing, Optional ByRef vPeriodEndDate As Object = Nothing, Optional ByRef vPeriodEndComplete As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oPeriod As bACTPeriod.Period
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oPeriods.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oPeriod = m_oPeriods.Item(m_lCurrentRecord)

            ' Get the Period Property Values






            'Developer Guide no.98
            m_lReturn = GetProperties(oPeriod, iStatus, vPeriodID:=vPeriodID, vCompanyID:=vCompanyID, vSubBranchID:=vSubBranchID, vYearName:=vYearName, vPeriodName:=vPeriodName, vPeriodEndDate:=vPeriodEndDate, vPeriodEndComplete:=vPeriodEndComplete)



            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oPeriod = Nothing


            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetPeriodYears (Public)
    '
    ' Description: Gets the required Periods and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetPeriodYears(ByRef vResultArray(,) As Object) As Integer
        Return GetPeriodYears(vResultArray:=vResultArray, vLockMode:=0, v_vSubBranchID:=0)
    End Function

    Public Function GetPeriodYears(ByRef vResultArray(,) As Object, ByRef vLockMode As gPMConstants.PMELockMode) As Integer
        Return GetPeriodYears(vResultArray:=vResultArray, vLockMode:=vLockMode, v_vSubBranchID:=0)
    End Function

    Public Function GetPeriodYears(ByRef vResultArray(,) As Object, ByVal v_vSubBranchID As Integer) As Integer
        Return GetPeriodYears(vResultArray:=vResultArray, vLockMode:=0, v_vSubBranchID:=v_vSubBranchID)
    End Function

    Public Function GetPeriodYears(ByRef vResultArray(,) As Object, ByRef vLockMode As gPMConstants.PMELockMode, ByVal v_vSubBranchID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Informations.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vLockMode = gPMConstants.PMELockMode.PMNoLock
            End If

            ' Add the Company ID parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the sub_branch_id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="sub_branch_id", vValue:=CStr(v_vSubBranchID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPeriodYearsSQL, sSQLName:=ACGetPeriodYearsName, bStoredProcedure:=ACGetPeriodYearsStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Did we get any records ?
            If Not Informations.IsArray(vResultArray) Then
                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPeriodYears Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPeriodYears", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPeriodLastDate (Public)
    '
    ' Description: Gets the latest period end date
    '
    ' ***************************************************************** '
    Public Function GetPeriodLastDate(ByRef vResultArray(,) As Object) As Integer
        Return GetPeriodLastDate(vResultArray:=vResultArray, vLockMode:=0, v_vSubBranchID:=0)
    End Function

    Public Function GetPeriodLastDate(ByRef vResultArray(,) As Object, ByRef vLockMode As gPMConstants.PMELockMode) As Integer
        Return GetPeriodLastDate(vResultArray:=vResultArray, vLockMode:=vLockMode, v_vSubBranchID:=0)
    End Function

    Public Function GetPeriodLastDate(ByRef vResultArray(,) As Object, ByVal v_vSubBranchID As Integer) As Integer
        Return GetPeriodLastDate(vResultArray:=vResultArray, vLockMode:=0, v_vSubBranchID:=v_vSubBranchID)
    End Function

    Public Function GetPeriodLastDate(ByRef vResultArray(,) As Object, ByRef vLockMode As gPMConstants.PMELockMode, ByVal v_vSubBranchID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Informations.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vLockMode = gPMConstants.PMELockMode.PMNoLock
            End If

            ' Add the Company ID parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the sub_branch_id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="sub_branch_id", vValue:=CStr(v_vSubBranchID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPeriodLastDateSQL, sSQLName:=ACGetPeriodLastDateName, bStoredProcedure:=ACGetPeriodLastDateStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Did we get any records ?
            If Informations.IsArray(vResultArray) Then

                If CStr(vResultArray(0, 0)) = "" Then
                    ' No Records, return PMFalse
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If
            Else
                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPeriodLastDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPeriodLastDate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied Period into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByVal vPeriodID As Object = Nothing, Optional ByVal vCompanyID As Object = Nothing, Optional ByVal vSubBranchID As Object = Nothing, Optional ByVal vYearName As Object = Nothing, Optional ByVal vPeriodName As Object = Nothing, Optional ByVal vPeriodEndDate As Object = Nothing, Optional ByVal vPeriodEndComplete As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oPeriod As bACTPeriod.Period

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oPeriods.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new Period
            oPeriod = New bACTPeriod.Period()

            ' Populate Period Attributes

            'developer guide no.98
            m_lReturn = SetProperties(oPeriod, gPMConstants.PMEComponentAction.PMAdd,
                 vPeriodID:=vPeriodID,
                 vCompanyID:=vCompanyID,
                 vSubBranchID:=vSubBranchID,
                 vYearName:=vYearName,
                 vPeriodName:=vPeriodName,
                 vPeriodEndDate:=vPeriodEndDate,
                 vPeriodEndComplete:=vPeriodEndComplete)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oPeriod = Nothing
                Return result
            End If

            ' Add Period to collection
            If (m_oPeriods.Count = 0) Then
                m_oPeriods.Add(Nothing)
            End If
            m_lReturn = CType(m_oPeriods.Add(oNewPeriod:=oPeriod), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oPeriod = Nothing
                Return result
            End If

            oPeriod = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the Period
    '              specified and updates the Period with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByVal vPeriodID As Object = Nothing, Optional ByVal vCompanyID As Object = Nothing, Optional ByVal vSubBranchID As Object = Nothing, Optional ByVal vYearName As Object = Nothing, Optional ByVal vPeriodName As Object = Nothing, Optional ByVal vPeriodEndDate As Object = Nothing, Optional ByVal vPeriodEndComplete As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oPeriod As bACTPeriod.Period
        Dim iStatus As gPMConstants.PMEComponentAction

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oPeriods.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oPeriod = m_oPeriods.Item(lRow)

            ' Check the Status of the Period

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oPeriod.DatabaseStatus
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

            ' Update Period Attributes





            'developer guide no.98
            m_lReturn = SetProperties(oPeriod, iStatus,
                vPeriodID:=oPeriod.PeriodID,
                vCompanyID:=vCompanyID,
                vSubBranchID:=vSubBranchID,
                vYearName:=vYearName,
                vPeriodName:=vPeriodName,
                vPeriodEndDate:=vPeriodEndDate,
                vPeriodEndComplete:=vPeriodEndComplete)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oPeriod = Nothing
                Return result
            End If

            ' Release reference to Period
            oPeriod = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified Period can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oPeriod As bACTPeriod.Period

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oPeriods.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oPeriod = m_oPeriods.Item(lRow)

            ' Check the Status of the Period

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oPeriod.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oPeriod.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oPeriod.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to Period
            oPeriod = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            For lSub As Integer = 1 To m_oPeriods.Count()
                Select Case m_oPeriods.Item(lSub).DatabaseStatus
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Cancel", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim oPeriod As bACTPeriod.Period
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oPeriods.Count()
                oPeriod = m_oPeriods.Item(lSub)


                Select Case oPeriod.DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing

                    Case gPMConstants.PMEComponentAction.PMAdd

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Add Item
                        m_lReturn = CType(AddItem(oPeriod), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMEdit

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Update Item
                        m_lReturn = CType(UpdateItem(oPeriod), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMDelete

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Delete Item
                        m_lReturn = CType(DeleteItem(oPeriod), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Release last reference
            oPeriod = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Refresh the Collection Items D/B Status
                    lSub = 1

                    ' For each item in the collection
                    Do While lSub <= m_oPeriods.Count()

                        ' With the item
                        With m_oPeriods.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oPeriods.Delete(lSub)

                                    ' Anything Else
                                Case Else
                                    ' Set Status to view
                                    .DatabaseStatus = gPMConstants.PMEComponentAction.PMView
                                    lSub += 1

                            End Select

                        End With

                    Loop

                Else

                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '****************************************************************** '
    '* Name: GetPeriodForDate (Public)
    '*
    '* Description: Retrieves Period id for a specified
    '*              Company and date
    '*
    '****************************************************************** '
    Public Function GetPostingPeriodForDate(ByRef dtDateInPeriod As Date, ByRef lPeriodID As Integer, ByVal lLedgerID As Integer) As Integer
        Return GetPostingPeriodForDate(dtDateInPeriod:=dtDateInPeriod, lPeriodID:=lPeriodID, lLedgerID:=lLedgerID, vYearName:=Nothing)
    End Function

    Public Function GetPostingPeriodForDate(ByRef dtDateInPeriod As Date, ByRef lPeriodID As Integer, ByVal lLedgerID As Integer, ByRef vYearName As Object) As Integer

        Dim result As Integer = 0
        Dim dtCurrentEndDate As Date
        Dim oLedger As bACTLedger.Form
        Dim lCurrentPeriodID As Integer
        Dim vResultArray(,) As Object = Nothing
        Dim lSubBranchID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' get an instance of bACTLedger
            'SD 24/07/2002 rename variable

            oLedger = New bACTLedger.Form
            m_lReturn = oLedger.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            ' Retrieve ledger record where ledger_id = ledger_id

            m_lReturn = oLedger.GetDetails(vLedgerID:=lLedgerID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            ' get the current period for the ledger

            m_lReturn = oLedger.GetNext(vCurrentPeriodID:=lCurrentPeriodID, vSubBranchID:=lSubBranchID)

            ' Retrieve period_end_date for current_period_id
            m_lReturn = CType(GetDetails(vPeriodID:=lCurrentPeriodID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(GetNext(vPeriodEndDate:=dtCurrentEndDate), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Retrieve period_id in which DateInPeriod falls
            ' Uses stored procedure
            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the company_ID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the date_in_period parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="date_in_period", vValue:=CStr(dtDateInPeriod), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the sub_branch_id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="sub_branch_id", vValue:=CStr(lSubBranchID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPeriodForDateSQL, sSQLName:=ACGetPeriodForDateName, bStoredProcedure:=ACGetPeriodForDateStored, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' CF230299
            If Informations.IsArray(vResultArray) Then

                lPeriodID = CInt(vResultArray(0, 0))
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Compare the dates
            If dtDateInPeriod < dtCurrentEndDate Then

                ' Only if period before current period
                If lPeriodID < lCurrentPeriodID Then

                    lPeriodID = lCurrentPeriodID

                    ' this is the end of the current period, it should be the first day after
                    ' the closed period.
                    m_lReturn = CType(GetFirstDayOfPeriod(v_lPeriodID:=lCurrentPeriodID, r_dtDateInPeriod:=dtDateInPeriod), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If

            ' Remove the instance of ledger
            oLedger = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get posting period id for a date", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPostingPeriodForDate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetFirstDayOfPeriod
    '
    ' Description: Gets the first day of the period passed in.
    '
    ' ***************************************************************** '
    Public Function GetFirstDayOfPeriod(ByVal v_lPeriodID As Integer, ByRef r_dtDateInPeriod As Date) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the parameter collection
            m_oDatabase.Parameters.Clear()

            ' Add the period_id parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="period_id", vValue:=CStr(v_lPeriodID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the period start date
            'Developer Guide no.40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="period_start_date", vValue:=r_dtDateInPeriod, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Perform the Stored Procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetPeriodStartDateStoredSQL, sSQLName:=ACGetPeriodStartDateName, bStoredProcedure:=ACGetPeriodStartDateStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' get the start date from the parameter values
            r_dtDateInPeriod = m_oDatabase.Parameters.Item("period_start_date").Value

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFirstDayOfPeriodFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFirstDayOfPeriod", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    '****************************************************************** '
    '* Name: GetPeriodForDate (Public)
    '*
    '* Description: Retrieves Period id for a specified
    '*              Company and date
    '*
    '****************************************************************** '
    'Renuka - (WPR87 Paralleling) - New Optional parameter(v_bIsPeriodNotExist) has been added
    Public Function GetPeriodForDate(ByVal dtDateInPeriod As Date, ByRef lPeriodID As Integer) As Integer
        Return GetPeriodForDate(dtDateInPeriod:=dtDateInPeriod, lPeriodID:=lPeriodID, vYearName:="", vSubBranchID:=0, v_bIsPeriodNotExist:=False, v_bIncludeClosed:=False)
    End Function

    Public Function GetPeriodForDate(ByVal dtDateInPeriod As Date, ByRef lPeriodID As Integer, ByRef vYearName As String) As Integer
        Return GetPeriodForDate(dtDateInPeriod:=dtDateInPeriod, lPeriodID:=lPeriodID, vYearName:=vYearName, vSubBranchID:=0, v_bIsPeriodNotExist:=False, v_bIncludeClosed:=False)
    End Function

    Public Function GetPeriodForDate(ByVal dtDateInPeriod As Date, ByRef lPeriodID As Integer, ByRef vYearName As String, ByVal vSubBranchID As Integer) As Integer
        Return GetPeriodForDate(dtDateInPeriod:=dtDateInPeriod, lPeriodID:=lPeriodID, vYearName:=vYearName, vSubBranchID:=vSubBranchID, v_bIsPeriodNotExist:=False, v_bIncludeClosed:=False)
    End Function

    Public Function GetPeriodForDate(ByVal dtDateInPeriod As Date, ByRef lPeriodID As Integer, ByVal vSubBranchID As Integer) As Integer
        Return GetPeriodForDate(dtDateInPeriod:=dtDateInPeriod, lPeriodID:=lPeriodID, vYearName:="", vSubBranchID:=vSubBranchID, v_bIsPeriodNotExist:=False, v_bIncludeClosed:=False)
    End Function

    Public Function GetPeriodForDate(ByVal dtDateInPeriod As Date, ByRef lPeriodID As Integer, ByRef vYearName As String, ByVal v_bIncludeClosed As Boolean) As Integer
        Return GetPeriodForDate(dtDateInPeriod:=dtDateInPeriod, lPeriodID:=lPeriodID, vYearName:=vYearName, vSubBranchID:=0, v_bIsPeriodNotExist:=False, v_bIncludeClosed:=v_bIncludeClosed)
    End Function

    Public Function GetPeriodForDate(ByVal dtDateInPeriod As Date, ByRef lPeriodID As Integer, ByVal vSubBranchID As Integer, ByVal v_bIsPeriodNotExist As Boolean, ByVal v_bIncludeClosed As Boolean) As Integer
        Return GetPeriodForDate(dtDateInPeriod:=dtDateInPeriod, lPeriodID:=lPeriodID, vYearName:="", vSubBranchID:=vSubBranchID, v_bIsPeriodNotExist:=v_bIsPeriodNotExist, v_bIncludeClosed:=v_bIncludeClosed)
    End Function

    Public Function GetPeriodForDate(ByVal dtDateInPeriod As Date, ByRef lPeriodID As Integer, ByRef vYearName As String, ByVal vSubBranchID As Integer, ByVal v_bIsPeriodNotExist As Boolean, ByVal v_bIncludeClosed As Boolean) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim sYearName As String = ""

        Const ACFirstRow As Integer = 0
        Const ACFirstColumn As Integer = 0
        Const ACSecondColumn As Integer = 1


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'See if we've got this one before (already)
            m_lReturn = CType(GetAlreadyRetrievedPeriod(v_dtDateInPeriod:=dtDateInPeriod, r_lPeriodId:=lPeriodID, r_sYearName:=sYearName), gPMConstants.PMEReturnCode)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then 'we've got it

                'Developer Guide no. 118
                vYearName = CStr(sYearName)
                Return result
            ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then  'a bad thing
                Return m_lReturn
                '   it's not found so lets go to the database to look it up
            End If

            ' Uses stored procedure
            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()


            ' Add the company_ID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPeriodForDate")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' Add the date_in_period parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="date_in_period", vValue:=CDate(dtDateInPeriod), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPeriodForDate")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' Add the Sub Btanch ID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="sub_branch_id", vValue:=CStr(vSubBranchID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPeriodForDate")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' Add the Include Closed parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="include_closed", vValue:=CBool(v_bIncludeClosed), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPeriodForDate")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPeriodForDateSQL, sSQLName:=ACGetPeriodForDateName, bStoredProcedure:=ACGetPeriodForDateStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPeriodForDate")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            If Not Informations.IsArray(vResultArray) Then
                'Start - Renuka - (WPR87 Paralleling)
                If v_bIsPeriodNotExist Then
                    lPeriodID = -1
                Else

                    result = gPMConstants.PMEReturnCode.PMNotFound
                    'Developer Guide no.40
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No period found for " & dtDateInPeriod, vApp:=ACApp, vClass:=ACClass, vMethod:="GetPeriodForDate", vErrNo:=0, vErrDesc:="")
                End If
                'End - Renuka - (WPR87 Paralleling)
            Else
                'DD 15/08/2002. Initialise the period collection

                lPeriodID = CInt(vResultArray(ACFirstColumn, ACFirstRow))
                m_Periods = New Dictionary(Of String, Date) From {
                    {lPeriodID.ToString, DateTime.Parse(dtDateInPeriod).ToString("d")}
                }

                sYearName = CStr(vResultArray(ACSecondColumn, ACFirstRow))
                m_PeriodYears = New Dictionary(Of String, Date) From {
                    {sYearName, DateTime.Parse(dtDateInPeriod).ToString("d")}
                }

                'Developer Guide no. 118
                vYearName = CStr(sYearName)

            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            '''''MessageBox.Show(excep.Message, Application.ProductName)
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPeriodForDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPeriodForDate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetNextPeriodID
    '
    ' Description: Get the next period number ID
    '
    ' ***************************************************************** '
    Public Function GetNextPeriodID(ByRef lPeriodID As Integer, ByRef lNextPeriodID As Integer) As Integer

        Dim result As Integer = 0
        Dim dPeriodEndDate As Date

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the end date of the period
            m_lReturn = CType(GetDetails(vPeriodID:=lPeriodID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' as above
            m_lReturn = CType(GetNext(vPeriodEndDate:=dPeriodEndDate), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Clear the parameters and
            m_oDatabase.Parameters.Clear()

            ' m_oPeriods.Item(0).PeriodEndDate, _
            '' ...add new ones
            m_lReturn = m_oDatabase.Parameters.Add(sName:="period_id", vValue:=CStr(lPeriodID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' ...add another parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="next_period_id", vValue:=CStr(lNextPeriodID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' ...add another parameter
            'Developer Guide no.40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="period_end_date", vValue:=dPeriodEndDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Call the stored procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetNextPeriodIDPeriodStoredSQL, sSQLName:=ACGetNextPeriodIDName, bStoredProcedure:=ACGetNextPeriodIDStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the value gotten from the SP
            lNextPeriodID = m_oDatabase.Parameters.Item("next_period_id").Value

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNextPeriodID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNextPeriodID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPreviousPeriodID
    '
    ' Description: Get the Previous period number ID
    '
    ' ***************************************************************** '
    Public Function GetPreviousPeriodID(ByRef lPeriodID As Integer, ByRef lPreviousPeriodID As Integer) As Integer

        Dim result As Integer = 0
        Dim dPeriodEndDate As Date

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the end date of the period
            m_lReturn = CType(GetDetails(vPeriodID:=lPeriodID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' as above
            m_lReturn = CType(GetNext(vPeriodEndDate:=dPeriodEndDate), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Clear the parameters and
            m_oDatabase.Parameters.Clear()

            ' m_oPeriods.Item(0).PeriodEndDate, _
            '' ...add new ones
            'DD 31/07/2002: Added back in. Required for multi-branch accounting
            m_lReturn = m_oDatabase.Parameters.Add(sName:="period_id", vValue:=CStr(lPeriodID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' CTAF 191200 - Swapped parameters around

            ' ...add another parameter
            'Developer Guide no.40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="period_end_date", vValue:=dPeriodEndDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' ...add another parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="previous_period_id", vValue:=CStr(lPreviousPeriodID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Call the stored procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetPreviousPeriodIDPeriodStoredSQL, sSQLName:=ACGetPreviousPeriodIDName, bStoredProcedure:=ACGetPreviousPeriodIDStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the value gotten from the SP

            If Not (Convert.IsDBNull(m_oDatabase.Parameters.Item("previous_period_id").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("previous_period_id").Value)) Then
                lPreviousPeriodID = m_oDatabase.Parameters.Item("previous_period_id").Value
            Else
                lPreviousPeriodID = 0
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPreviousPeriodID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPreviousPeriodID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetCurrentPeriodDetails
    '
    ' Description:
    '
    ' History: 09/02/2000 CTAF - Created.
    ' Eck PN460 02/06/2003
    ' ***************************************************************** '
    Public Function GetCurrentPeriodDetails(ByRef r_vDetails As Object) As Integer
        Return GetCurrentPeriodDetails(r_vDetails:=r_vDetails, vSubBranchID:=0)
    End Function

    Public Function GetCurrentPeriodDetails(ByRef r_vDetails As Object, ByVal vSubBranchID As Integer) As Integer

        'Dim sSQL As String
        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        'Dim lPeriodID As Long

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If vSubBranchID = 0 Then
                m_oDatabase.Parameters.Clear()
                ' Add the company_ID parameter (INPUT)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrentPeriodDetails")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add the sub branch_ID parameter (OUTPUT)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="sub_branch_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrentPeriodDetails")

                    Return gPMConstants.PMEReturnCode.PMFalse

                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetSubBranchDefaultSQL, sSQLName:=ACGetSubBranchDefaultName, bStoredProcedure:=ACGetSubBranchDefaultStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLAction failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrentPeriodDetails")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                vSubBranchID = m_oDatabase.Parameters.Item("sub_branch_id").Value

            End If


            ' DD 31/07/2002: Rewritten with a stored procedure for
            ' Multi-Branch Accounting

            m_oDatabase.Parameters.Clear()

            ' Add the company_ID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrentPeriodDetails")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' Add the Sub Btanch ID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="sub_branch_id", vValue:=CStr(vSubBranchID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrentPeriodDetails")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCurrentPeriodSQL, sSQLName:=ACGetCurrentPeriodName, bStoredProcedure:=ACGetCurrentPeriodStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrentPeriodDetails")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            If Not Informations.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            Else
                ' Assign the return value


                r_vDetails = vResultArray
            End If

            '    ' Construct the SQL
            '    'MJ 21052002 so that it does not assume that the period_id is sequential
            '    sSQL$ = "SELECT MIN(period_id) FROM Period WHERE period_end_date = " & _
            ''            "(select min(period_end_date) from period WHERE period_end_complete = 0)"
            '
            '    m_lReturn& = m_oDatabase.SQLSelect( _
            ''        sSQL:=sSQL$, _
            ''        sSQLName:="GetCurrentPeriod", _
            ''        bStoredProcedure:=False, _
            ''        lNumberRecords:=PMAllRecords, _
            ''        vResultArray:=vResultArray)
            '    If (m_lReturn& <> PMTrue) Then
            '        If (m_lReturn& = PMNotFound) Then
            '            GetCurrentPeriodDetails = PMNotFound
            '        Else
            '            GetCurrentPeriodDetails = PMFalse
            '            ' Log Error Message
            '            LogMessage m_sUsername, _
            ''                iType:=PMLogOnError, _
            ''                sMsg:="SQL Failed : " & sSQL$, _
            ''                vApp:=ACApp, _
            ''                vClass:=ACClass, _
            ''                vMethod:="GetCurrentPeriodDetails", _
            ''                vErrNo:=Err.Number, _
            ''                vErrDesc:=Err.Description
            '        End If
            '        Exit Function
            '    End If
            '
            '    If (IsArray(vResultArray) = False) Then
            '        GetCurrentPeriodDetails = PMNotFound
            '        ' Log Error Message
            '        LogMessage m_sUsername, _
            ''            iType:=PMLogOnError, _
            ''            sMsg:="Unable to get current period.", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="GetCurrentPeriodDetails", _
            ''            vErrNo:=Err.Number, _
            ''            vErrDesc:=Err.Description
            '        Exit Function
            '    End If
            '
            '    ' Get the period_id
            '    lPeriodID& = vResultArray(0, 0)
            '
            '    ' Construct the SQL
            '    sSQL$ = "SELECT period_id, company_id, year_name, period_name, period_end_date, period_end_complete " & _
            ''            "FROM Period WHERE period_id = {period_id}"
            '
            '    ' Clear the parameters
            '    m_oDatabase.Parameters.Clear
            '
            '    m_lReturn& = m_oDatabase.Parameters.Add( _
            ''            sName:="period_id", _
            ''            vValue:=lPeriodID&, _
            ''            iDirection:=PMParamInput, _
            ''            iDataType:=PMLong)
            '    If (m_lReturn& <> PMTrue) Then
            '        GetCurrentPeriodDetails = PMFalse
            '        ' Log Error Message
            '        LogMessage m_sUsername, _
            ''            iType:=PMLogOnError, _
            ''            sMsg:="Failed to add parameter : period_id", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="GetCurrentPeriodDetails", _
            ''            vErrNo:=Err.Number, _
            ''            vErrDesc:=Err.Description
            '        Exit Function
            '    End If
            '
            '    m_lReturn& = m_oDatabase.SQLSelect( _
            ''            sSQL:=sSQL$, _
            ''            sSQLName:="GetPeriodDetails", _
            ''            bStoredProcedure:=False, _
            ''            lNumberRecords:=PMAllRecords, _
            ''            vResultArray:=vResultArray)
            '    If (m_lReturn& <> PMTrue) Then
            '        GetCurrentPeriodDetails = PMFalse
            '        ' Log Error Message
            '        LogMessage m_sUsername, _
            ''            iType:=PMLogOnError, _
            ''            sMsg:="SQL Failed : " & sSQL$, _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="GetCurrentPeriodDetails", _
            ''            vErrNo:=Err.Number, _
            ''            vErrDesc:=Err.Description
            '        Exit Function
            '    End If
            '
            '    If (IsArray(vResultArray) = False) Then
            '        GetCurrentPeriodDetails = PMFalse
            '        ' Log Error Message
            '        LogMessage m_sUsername, _
            ''            iType:=PMLogOnError, _
            ''            sMsg:="Failed to retrive details for period_id=" & CStr(lPeriodID&), _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="GetCurrentPeriodDetails", _
            ''            vErrNo:=Err.Number, _
            ''            vErrDesc:=Err.Description
            '        Exit Function
            '    End If
            '
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCurrentPeriodDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrentPeriodDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetUniqueYears
    '
    ' Description: Gets a list of unique period year names
    '
    ' DD 31/07/2002: Rewritten for multi-brach accounting
    '
    ' ***************************************************************** '
    Public Function GetUniqueYears(ByRef r_vYearArray(,) As Object) As Integer
        Return GetUniqueYears(r_vYearArray:=r_vYearArray, v_vSubBranchID:=0)
    End Function

    Public Function GetUniqueYears(ByRef r_vYearArray(,) As Object, ByVal v_vSubBranchID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Construct the SQL statement
            ' sSQL$ = "SELECT DISTINCT year_name FROM period"

            ' Add the company_ID parameter (INPUT)
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUniqueYears")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' Add the Sub Btanch ID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="sub_branch_id", vValue:=CStr(v_vSubBranchID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUniqueYears")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' Perform the select
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetUniqueYearsSQL, sSQLName:=ACGetUniqueYearsName, bStoredProcedure:=ACGetUniqueYearsStored, vResultArray:=r_vYearArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUniqueYearsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUniqueYears", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdatePeriodYearNames
    '
    ' Description: Changes the year names for the passed period array
    '
    ' History: 20/01/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function UpdatePeriodYearNames(ByVal v_vPeriodArray(,) As Object, ByVal v_sYearName As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As New StringBuilder
        Dim lRecordsAffected As Integer
        Dim vResultArray(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            ' Check that the year name isn't in use first
            sSQL = New StringBuilder("SELECT period_id FROM Period WHERE period_id NOT IN ( ")
            For iLoop1 As Integer = v_vPeriodArray.GetLowerBound(1) To v_vPeriodArray.GetUpperBound(1)

                sSQL.Append(CStr(v_vPeriodArray(0, iLoop1)) & ", ")
            Next iLoop1

            sSQL = New StringBuilder(sSQL.ToString().Substring(0, sSQL.ToString().Length - 2))

            sSQL.Append(") AND year_name = '" & v_sYearName & "'")

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL.ToString(), sSQLName:="CheckYearsInUse", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePeriodYearNames Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePeriodYearNames", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                End If

            End If

            ' if someone else is using the year name then
            If Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMRecordInUse
            End If

            For iLoop1 As Integer = v_vPeriodArray.GetLowerBound(1) To v_vPeriodArray.GetUpperBound(1)

                sSQL = New StringBuilder("UPDATE Period " &
                       "SET year_name = '" & v_sYearName & "' " &
                       "WHERE period_id = " & CStr(v_vPeriodArray(0, iLoop1)))
                m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="UpdatePeriodYearName", bStoredProcedure:=False, lRecordsAffected:=lRecordsAffected)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set year name to '" & v_sYearName & "' for period " & CStr(v_vPeriodArray(0, iLoop1)), vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePeriodYearNames", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePeriodYearNames Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePeriodYearNames", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: AddItem (Private)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Private Function AddItem(ByRef oPeriod As bACTPeriod.Period) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add PeriodID as an OUTPUT param for an insert
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Period_id", vValue:=CStr(oPeriod.PeriodID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the required INPUT parameters
        m_lReturn = CType(AddInputParam(oPeriod:=oPeriod), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the ID of the record inserted
        oPeriod.PeriodID = m_oDatabase.Parameters.Item("Period_id").Value

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
    Private Function UpdateItem(ByRef oPeriod As bACTPeriod.Period) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add PeriodID as an INPUT param for an update
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Period_id", vValue:=CStr(oPeriod.PeriodID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the Input Parameters to the Parameteres collection
        m_lReturn = CType(AddInputParam(oPeriod:=oPeriod), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' BB Not required by Orion
        ' Add Timestamp as an INPUT param for an update
        'm_lReturn& = m_oDatabase.Parameters.Add( _
        'sName:="timestamp", _
        'vValue:=oPeriod.Timestamp, _
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
    Private Function DeleteItem(ByRef oPeriod As bACTPeriod.Period) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the PeriodID INPUT parameter
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Period_id", vValue:=CStr(oPeriod.PeriodID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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
    ' Description: Sets the supplied Period properties from a database
    '              record.
    ' ***************************************************************** '
    Private Function SetPropertiesFromDB(ByRef oPeriod As bACTPeriod.Period, ByRef lRecordNumber As Integer) As Integer

        Dim result As Integer = 0
        'developer guide no 21
        Dim oFields As DataRow



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set oFields to refer to Field Collection for Record 1
        oFields = m_oDatabase.Records.Item(lRecordNumber).Fields()

        ' Populate Base Details

        With oPeriod

            .PeriodID = oFields("period_id")

            If Convert.IsDBNull(oFields("company_id")) Or Informations.IsNothing(oFields("company_id")) Then
                .CompanyID = 0
            Else
                .CompanyID = oFields("company_id")
            End If

            If Convert.IsDBNull(oFields("company_id")) Or Informations.IsNothing(oFields("company_id")) Then
                .SubBranchID = 0
            Else
                .SubBranchID = oFields("sub_branch_id")
            End If

            If Convert.IsDBNull(oFields("year_name")) Or Informations.IsNothing(oFields("year_name")) Then
                .YearName = ""
            Else
                .YearName = oFields("year_name")
            End If

            If Convert.IsDBNull(oFields("period_name")) Or Informations.IsNothing(oFields("period_name")) Then
                .PeriodName = ""
            Else
                .PeriodName = oFields("period_name")
            End If

            If Convert.IsDBNull(oFields("period_end_date")) Or Informations.IsNothing(oFields("period_end_date")) Then
                .PeriodEndDate = #12/30/1899#
            Else
                .PeriodEndDate = oFields("period_end_date")
            End If

            If Convert.IsDBNull(oFields("period_end_complete")) Or Informations.IsNothing(oFields("period_end_complete")) Then
                .PeriodEndComplete = 0
            Else
                .PeriodEndComplete = oFields("period_end_complete")
            End If

            .DatabaseStatus = gPMConstants.PMEComponentAction.PMView

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Private)
    '
    ' Description: Sets the supplied Period property values.
    '
    ' ***************************************************************** '
    'developer guide no.101
    Private Function SetProperties(ByRef oPeriod As bACTPeriod.Period, ByRef iStatus As Integer, Optional ByVal vPeriodID As Object = Nothing, Optional ByVal vCompanyID As Object = Nothing, Optional ByVal vSubBranchID As Object = Nothing, Optional ByVal vYearName As Object = Nothing, Optional ByVal vPeriodName As Object = Nothing, Optional ByVal vPeriodEndDate As Object = Nothing, Optional ByVal vPeriodEndComplete As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        ' If Add Mode
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            ' Check that All Mandatory Parameters have been supplied
            m_lReturn = CType(CheckMandatory(vPeriodID:=vPeriodID, vCompanyID:=vCompanyID, vSubBranchID:=vSubBranchID, vYearName:=vYearName, vPeriodName:=vPeriodName, vPeriodEndDate:=vPeriodEndDate, vPeriodEndComplete:=vPeriodEndComplete), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Default Any Missing Parameters

            'developer guide no.98
            m_lReturn = CType(DefaultParameters(bDefaultAll:=False, vPeriodID:=vPeriodID, vCompanyID:=vCompanyID, vSubBranchID:=vSubBranchID, vYearName:=vYearName, vPeriodName:=vPeriodName, vPeriodEndDate:=vPeriodEndDate, vPeriodEndComplete:=vPeriodEndComplete), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

        End If

        ' Validate Parameters
        m_lReturn = CType(Validate(vPeriodID:=vPeriodID, vCompanyID:=vCompanyID, vSubBranchID:=vSubBranchID, vYearName:=vYearName, vPeriodName:=vPeriodName, vPeriodEndDate:=vPeriodEndDate, vPeriodEndComplete:=vPeriodEndComplete), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        ' Set Data Changed Flag to False
        bDataChanged = False


        ' Set Property values.
        With oPeriod


            If Not Informations.IsNothing(vPeriodID) Then
                If .PeriodID <> vPeriodID Then
                    .PeriodID = vPeriodID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vCompanyID) Then
                If .CompanyID <> vCompanyID Then
                    .CompanyID = vCompanyID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vSubBranchID) Then
                If .SubBranchID <> vSubBranchID Then
                    .SubBranchID = vSubBranchID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vYearName) Then
                If .YearName.Trim() <> vYearName.Trim() Then
                    .YearName = vYearName
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vPeriodName) Then
                If .PeriodName.Trim() <> vPeriodName.Trim() Then
                    .PeriodName = vPeriodName
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vPeriodEndDate) Then

                If .PeriodEndDate <> CDate(vPeriodEndDate) Then

                    .PeriodEndDate = CDate(vPeriodEndDate)
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vPeriodEndComplete) Then
                If .PeriodEndComplete <> vPeriodEndComplete Then
                    .PeriodEndComplete = vPeriodEndComplete
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
    ' Description: Returns the supplied Period property values.
    '
    ' ***************************************************************** '
    'developer guide no.101
    Private Function GetProperties(ByRef oPeriod As bACTPeriod.Period, ByRef iStatus As Integer, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vYearName As Object = Nothing, Optional ByRef vPeriodName As Object = Nothing, Optional ByRef vPeriodEndDate As Object = Nothing, Optional ByRef vPeriodEndComplete As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set Property values.
        With oPeriod

            'Development Guide no 118
            vPeriodID = .PeriodID


            'Development Guide no 118
            vCompanyID = .CompanyID


            'Development Guide no 118
            vSubBranchID = .SubBranchID


            'Development Guide no 118
            vYearName = .YearName


            'Development Guide no 118
            vPeriodName = .PeriodName


            'Development Guide no 118
            vPeriodEndDate = .PeriodEndDate


            'Development Guide no 118
            vPeriodEndComplete = .PeriodEndComplete

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
    Private Function AddInputParam(ByRef oPeriod As bACTPeriod.Period) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            If oPeriod.CompanyID < 1 Then

                'developer guide no 85
                m_lReturn = .Parameters.Add(sName:="company_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="company_id", vValue:=CStr(oPeriod.CompanyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oPeriod.SubBranchID < 1 Then

                'developer guide no 85
                m_lReturn = .Parameters.Add(sName:="sub_branch_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="sub_branch_id", vValue:=CStr(oPeriod.SubBranchID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="year_name", vValue:=oPeriod.YearName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="period_name", vValue:=oPeriod.PeriodName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Developer Guide no.40
            m_lReturn = .Parameters.Add(sName:="period_end_date", vValue:=oPeriod.PeriodEndDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="period_end_complete", vValue:=CStr(oPeriod.PeriodEndComplete), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DefaultParameters (Public)
    '
    ' Description: Sets the Default Values for a Period.
    '
    ' ***************************************************************** '
    'developer guide no.101
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByVal vPeriodID As Object = Nothing, Optional ByVal vCompanyID As Object = Nothing, Optional ByVal vSubBranchID As Object = Nothing, Optional ByVal vYearName As Object = Nothing, Optional ByVal vPeriodName As Object = Nothing, Optional ByVal vPeriodEndDate As Object = Nothing, Optional ByVal vPeriodEndComplete As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vPeriodID)) Or (vPeriodID.Equals(0)) Or (bDefaultAll) Then
            vPeriodID = 0
        End If



        If (Informations.IsNothing(vCompanyID)) Or (vCompanyID.Equals(0)) Or (bDefaultAll) Then
            vCompanyID = 0
        End If



        If (Informations.IsNothing(vSubBranchID)) Or (vSubBranchID.Equals(0)) Or (bDefaultAll) Then
            vSubBranchID = 0
        End If



        If (Informations.IsNothing(vYearName)) Or (String.IsNullOrEmpty(vYearName)) Or (bDefaultAll) Then
            vYearName = ""
        End If



        If (Informations.IsNothing(vPeriodName)) Or (String.IsNullOrEmpty(vPeriodName)) Or (bDefaultAll) Then
            vPeriodName = ""
        End If



        If (Informations.IsNothing(vPeriodEndDate)) Or (vPeriodEndDate.Equals(DateTime.FromOADate(0))) Or (bDefaultAll) Then
            vPeriodEndDate = DateTime.Now
        End If


        'Developer Guide no. 44
        If (Informations.IsNothing(vPeriodEndComplete)) OrElse (vPeriodEndComplete.Equals(0)) Or (bDefaultAll) Then
            vPeriodEndComplete = 0
        End If

        ' {* USER DEFINED CODE (End) *}


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Public)
    '
    ' Description: Sets the Default Values for a Period.
    '
    ' ***************************************************************** '
    Private Function CheckMandatory(Optional ByVal vPeriodID As Object = Nothing, Optional ByVal vCompanyID As Object = Nothing, Optional ByVal vSubBranchID As Object = Nothing, Optional ByVal vYearName As Object = Nothing, Optional ByVal vPeriodName As Object = Nothing, Optional ByVal vPeriodEndDate As Object = Nothing, Optional ByVal vPeriodEndComplete As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vPeriodID)) Or (Object.Equals(vPeriodID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vCompanyID)) Or (Object.Equals(vCompanyID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vSubBranchID)) Or (Object.Equals(vSubBranchID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vYearName)) Or (Object.Equals(vYearName, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vPeriodName)) Or (Object.Equals(vPeriodName, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vPeriodEndDate)) Or (Object.Equals(vPeriodEndDate, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        '    If (IsMissing(vPeriodEndComplete) = True) _
        ''    Or (IsEmpty(vPeriodEndComplete) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If

        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the Period for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByVal vPeriodID As Object = Nothing, Optional ByVal vCompanyID As Object = Nothing, Optional ByVal vSubBranchID As Object = Nothing, Optional ByVal vYearName As Object = Nothing, Optional ByVal vPeriodName As Object = Nothing, Optional ByVal vPeriodEndDate As Object = Nothing, Optional ByVal vPeriodEndComplete As Object = Nothing) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        Dim dbNumericTemp As Double
        If Not Double.TryParse(CStr(vPeriodID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp2 As Double
        If Not Double.TryParse(CStr(vCompanyID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp3 As Double
        If Not Double.TryParse(CStr(vSubBranchID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsDate(vPeriodEndDate) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetSubBranches
    '
    ' Description: Return a list of Sub Branches
    '
    ' History: DD 02/08/2002: Take from Sirius Core
    '
    ' ***************************************************************** '
    Public Function GetSubBranches(ByRef r_vSubBranchArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lNumberOfRecords As Integer

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="source_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLSelect(sSQL:=ACGetSubBranchSQL, sSQLName:=ACGetSubBranchName, bStoredProcedure:=ACGetSubBranchStored, lNumberRecords:=lNumberOfRecords, vResultArray:=r_vSubBranchArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed to GetSubBranches", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSubBranches")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSubBranches Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSubBranches", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetAlreadyRetrievedPeriod (Private Method)
    '
    ' Description: See if we've already looked up this date and saved it
    '              in the collection
    '
    ' ***************************************************************** '
    Private Function GetAlreadyRetrievedPeriod(ByVal v_dtDateInPeriod As Date, ByRef r_lPeriodId As Integer, ByRef r_sYearName As String) As Integer

        Dim result As Integer = 0
        Dim bFound As Boolean
        Dim sKey As Date

        result = gPMConstants.PMEReturnCode.PMTrue

        bFound = True
        sKey = DateTime.Parse(v_dtDateInPeriod)

        'developer guide no 56

        If Informations.IsNothing(m_Periods) = False Then
            For Each kvp As KeyValuePair(Of String, Date) In m_Periods
                If kvp.Value = sKey Then
                    r_lPeriodId = CInt((kvp.Key).ToString)
                    Exit For
                End If
            Next
        Else
            bFound = False
        End If


        If Informations.IsNothing(m_PeriodYears) = False Then
            For Each kvp As KeyValuePair(Of String, Date) In m_PeriodYears
                If kvp.Value = sKey Then
                    r_sYearName = CStr(kvp.Key)
                    Exit For
                End If
            Next
        Else
            bFound = False
        End If

        If Not bFound Then
            result = gPMConstants.PMEReturnCode.PMNotFound
        End If

        Return result

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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    Public Function GetLatestUsedPeriod(ByVal v_lPeriodID As Integer, ByRef r_dtLatestUsedPeriod As Date) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetLatestUsedPeriod"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "period_id", v_lPeriodID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            bPMAddParameter.AddParameterLite(m_oDatabase, "period_end_date", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDate)

            ' Perform the Stored Procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetLatestUsedPeriodSQL, sSQLName:=ACGetLatestUsedPeriodName, bStoredProcedure:=ACGetLatestUsedPeriodStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "sSQL:=ACGetLatestUsedPeriodSQL", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get the start date from the parameter values
            r_dtLatestUsedPeriod = gPMFunctions.NullToDate(m_oDatabase.Parameters.Item("period_end_date").Value)


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here

            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
