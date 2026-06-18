Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
Imports System.Globalization
'Developer Guide No 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Form_NET.Form")> _
Public NotInheritable Class Form
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 11/07/1997
    '
    ' Description: Creatable Form class which contains all the
    '              methods, Form rules required to manipulate
    '              a Lookup.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Form"

    ' ************************************************
    ' Added to replace global variables 13/01/2004
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************
    Private m_oLookups As bGEMLookup.Lookups
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
    Private m_sLookupName As String = ""
    Private m_sGetDetailsName As String = ""
    Private m_sGetDetailsSQL As String = ""
    Private m_sGetAllDetailsName As String = ""
    Private m_sGetAllDetailsSQL As String = ""
    Private m_sCheckIDName As String = ""
    Private m_sCheckIDSQL As String = ""
    Private m_sAddName As String = ""
    Private m_sAddSQL As String = ""
    Private m_sDeleteName As String = ""
    Private m_sDeleteSQL As String = ""
    Private m_sDeleteAllName As String = ""
    Private m_sDeleteAllSQL As String = ""
    Private m_sUpdateName As String = ""
    Private m_sUpdateSQL As String = ""
    Private m_lInsuranceFileID As Integer
    Private m_lRiskID As Integer

    ' PRIVATE Data Members (End)
    Private Function get_SubstituteTableName(ByRef sSQLConstant As String, ByVal sNewTableName As String) As String
        Dim nPosition As Integer = (sSQLConstant.IndexOf("TypeTable") + 1)
        If nPosition = 0 Then
            Return sSQLConstant
        Else
            Return sSQLConstant.Substring(0, nPosition - 1) & sNewTableName & Mid(sSQLConstant, nPosition + ("TypeTable").Length)
        End If
    End Function

    ' PUBLIC Property Procedures (Begin)
    'sj 19/06/98
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFGemini
        End Get
    End Property
    Public Property LookupName() As String
        Get
            Return m_sLookupName
        End Get
        Set(ByVal Value As String)

            m_sLookupName = Value

            m_sGetDetailsName = get_SubstituteTableName(ACGetDetailsName, Value)
            m_sGetDetailsSQL = get_SubstituteTableName(ACGetDetailsSQL, Value)
            m_sGetAllDetailsName = get_SubstituteTableName(ACGetAllDetailsName, Value)
            m_sGetAllDetailsSQL = get_SubstituteTableName(ACGetAllDetailsSQL, Value)
            m_sCheckIDName = get_SubstituteTableName(ACCheckIDName, Value)
            m_sCheckIDSQL = get_SubstituteTableName(ACCheckIDSQL, Value)
            m_sAddName = get_SubstituteTableName(ACAddName, Value)
            m_sAddSQL = get_SubstituteTableName(ACAddSQL, Value)
            m_sDeleteName = get_SubstituteTableName(ACDeleteName, Value)
            m_sDeleteSQL = get_SubstituteTableName(ACDeleteSQL, Value)
            m_sDeleteAllName = get_SubstituteTableName(ACDeleteAllName, Value)
            m_sDeleteAllSQL = get_SubstituteTableName(ACDeleteAllSQL, Value)
            m_sUpdateName = get_SubstituteTableName(ACUpdateName, Value)
            m_sUpdateSQL = get_SubstituteTableName(ACUpdateSQL, Value)

        End Set
    End Property
    Public Property CurrentRecord() As Integer
        Get

            Return m_lCurrentRecord

        End Get
        Set(ByVal Value As Integer)

            Select Case Value
                Case Is < 1
                    m_lCurrentRecord = 0
                Case Is > m_oLookups.Count()
                    m_lCurrentRecord = m_oLookups.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Numner in Collection
            Return m_oLookups.Count()

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

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise




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
                'm_lReturn& = m_oDatabase.OpenDatabase(vDSN:=PMGeminiDSN)
                'AK 100802 - scalability
                m_lReturn = CType(m_oDatabase.OpenDatabase(vDSN:=gPMConstants.PMGeminiDSN, sSiriusUsername:=m_sUsername, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, sCallingAppName:=ACApp), gPMConstants.PMEReturnCode)

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

            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now

            ' Default RiskID to 0 (So we will work at Ins File Level by default)
            m_lRiskID = 0

            ' Create Lookups Collection
            m_oLookups = New bGEMLookup.Lookups()

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
                m_oLookups = Nothing
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
    ' Description: Gets the Lookup values for the current lookup table
    '
    '
    ' ***************************************************************** '
    'Developer Guide No 101
    'Public Function GetLookupValues(ByRef dtEffectiveDate As Date, ByRef vTableArray As Object, ByRef iLanguageID As Integer, ByRef vResultArray As String, Optional ByVal v_vInsurerNo As Object = Nothing, Optional ByVal v_vObjectID As Object = Nothing) As Integer
    Public Function GetLookupValues(ByRef dtEffectiveDate As Date, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object, Optional ByVal v_vInsurerNo As Object = Nothing, Optional ByVal v_vObjectID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim vTabArray(3, 0) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            vResultArray = Nothing
            ' Reset Table Array

            vTableArray = Nothing


            ' Setup Lookup Table Name

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = LookupName

            ' Do not supply a key

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""

            ' Get the Lookup items

            m_lReturn = CType(FillTable(vTableArray:=vTabArray, vResultArray:=vResultArray, v_vInsurerNo:=v_vInsurerNo, v_vObjectID:=v_vObjectID), gPMConstants.PMEReturnCode)

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
    ' Description: Adds a single Lookup directly into the database.
    '        Note: The Lookup  will NOT be added to the collection.
    '
    ' ***************************************************************** '
    'Developer Guide No 101
    'Public Function DirectAdd(Optional ByRef vLookupID As Integer = 0, Optional ByRef vdescription As Object = Nothing) As Integer
    Public Function DirectAdd(Optional ByRef vLookupID As Object = Nothing, Optional ByRef vdescription As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oLookup As bGEMLookup.Lookup

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Lookup
            oLookup = New bGEMLookup.Lookup()

            ' Populate Lookup Attributes

            m_lReturn = CType(SetProperties(oLookup, gPMConstants.PMEComponentAction.PMAdd, vLookupID:=vLookupID, vdescription:=CStr(vdescription)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Lookup to the Database
            m_lReturn = CType(AddItem(oLookup), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the ID of the Lookup Added

            If Not Information.IsNothing(vLookupID) Then
                vLookupID = oLookup.LookupID
            End If

            ' {* USER DEFINED CODE (End) *}

            oLookup = Nothing

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
    ' Description: Deletes a single Lookup directly from the database.
    '        Note: The Lookup will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByRef vLookupID As Object = Nothing, Optional ByRef vdescription As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oLookup As bGEMLookup.Lookup

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Lookup
            oLookup = New bGEMLookup.Lookup()

            ' Populate Lookup Attributes


            m_lReturn = CType(SetProperties(oLookup, gPMConstants.PMEComponentAction.PMDelete, vLookupID:=CInt(vLookupID), vdescription:=CStr(vdescription)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Lookup to the Database
            m_lReturn = CType(DeleteItem(oLookup), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oLookup = Nothing

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
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=m_sCheckIDSQL, sSQLName:=m_sCheckIDName, bStoredProcedure:=ACCheckIDStored, lNumberRecords:=0)

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
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required Lookups and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vLookupID As Object = Nothing, Optional ByRef vLockMode As gPMConstants.PMELockMode = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oLookup As bGEMLookup.Lookup

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oLookups.Clear()

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

            If Not Information.IsNothing(vLookupID) Then

                ' Yes, Is the key valid

                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(CStr(vLookupID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vLookupID =" & CStr(vLookupID), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                    Return result

                End If

                ' Add the LookupID parameter (INPUT)

                m_lReturn = m_oDatabase.Parameters.Add(sName:=m_sLookupName & "_id", vValue:=CStr(vLookupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=m_sGetDetailsSQL, sSQLName:=m_sGetDetailsName, bStoredProcedure:=ACGetDetailsStored, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else

                ' No Key, Get All Records

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=m_sGetAllDetailsSQL, sSQLName:=m_sGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0)

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

                    ' Create New Lookup
                    oLookup = New bGEMLookup.Lookup()

                    m_lReturn = CType(SetPropertiesFromDB(oLookup:=oLookup, lRecordNumber:=lSub), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    ' Add Lookup to collection
                    m_lReturn = CType(m_oLookups.Add(oNewLookup:=oLookup), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oLookup = Nothing

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
    ' Description: Gets the required Lookups and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vLookupID As Object = Nothing, Optional ByRef vdescription As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oLookup As bGEMLookup.Lookup
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oLookups.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oLookup = m_oLookups.Item(m_lCurrentRecord)

            ' Get the Lookup Property Values


            m_lReturn = CType(GetProperties(oLookup, iStatus, vLookupID:=CInt(vLookupID), vdescription:=CStr(vdescription)), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oLookup = Nothing


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
    ' Description: Adds the supplied Lookup into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vLookupID As Object = Nothing, Optional ByRef vdescription As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oLookup As bGEMLookup.Lookup

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oLookups.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new Lookup
            oLookup = New bGEMLookup.Lookup()

            ' Populate Lookup Attributes


            m_lReturn = CType(SetProperties(oLookup, gPMConstants.PMEComponentAction.PMAdd, vLookupID:=CInt(vLookupID), vdescription:=CStr(vdescription)), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oLookup = Nothing
                Return result
            End If

            ' Add Lookup to collection
            m_lReturn = CType(m_oLookups.Add(oNewLookup:=oLookup), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oLookup = Nothing
                Return result
            End If

            oLookup = Nothing

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
    ' Description: Validates that this action is valid on the Lookup
    '              specified and updates the Lookup with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vLookupID As Object = Nothing, Optional ByRef vdescription As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oLookup As bGEMLookup.Lookup
        Dim iStatus As gPMConstants.PMEComponentAction

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oLookups.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oLookup = m_oLookups.Item(lRow)

            ' Check the Status of the Lookup

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oLookup.DatabaseStatus
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

            ' Update Lookup Attributes


            m_lReturn = CType(SetProperties(oLookup, iStatus, vLookupID:=CInt(vLookupID), vdescription:=CStr(vdescription)), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oLookup = Nothing
                Return result
            End If

            ' Release reference to Lookup
            oLookup = Nothing

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
    ' Description: Validate that the specified Lookup can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oLookup As bGEMLookup.Lookup

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oLookups.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oLookup = m_oLookups.Item(lRow)

            ' Check the Status of the Lookup

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oLookup.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oLookup.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oLookup.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to Lookup
            oLookup = Nothing

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
            For lSub As Integer = 1 To m_oLookups.Count()
                Select Case m_oLookups.Item(lSub).DatabaseStatus
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
        Dim oLookup As bGEMLookup.Lookup
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oLookups.Count()
                oLookup = m_oLookups.Item(lSub)


                Select Case oLookup.DatabaseStatus
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
                        m_lReturn = CType(AddItem(oLookup), gPMConstants.PMEReturnCode)
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
                        m_lReturn = CType(UpdateItem(oLookup), gPMConstants.PMEReturnCode)
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
                        m_lReturn = CType(DeleteItem(oLookup), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Release last reference
            oLookup = Nothing

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
                    Do While lSub <= m_oLookups.Count()

                        ' With the item
                        With m_oLookups.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oLookups.Delete(lSub)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Private Function AddItem(ByRef oLookup As bGEMLookup.Lookup) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()


        ' Add LookupID as an INPUT param for an insert
        m_lReturn = m_oDatabase.Parameters.Add(sName:=m_sLookupName & "_id", vValue:=CStr(oLookup.LookupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the required INPUT parameters
        m_lReturn = CType(AddInputParam(oLookup:=oLookup), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=m_sAddSQL, sSQLName:=m_sAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the ID of the record inserted
        oLookup.LookupID = m_oDatabase.Parameters.Item(m_sLookupName & "_id").Value

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
    Private Function UpdateItem(ByRef oLookup As bGEMLookup.Lookup) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the Input Parameters to the Parameteres collection
        m_lReturn = CType(AddInputParam(oLookup:=oLookup), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add LookupID as an INPUT param for an update
        m_lReturn = m_oDatabase.Parameters.Add(sName:=m_sLookupName & "_id", vValue:=CStr(oLookup.LookupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=m_sUpdateSQL, sSQLName:=m_sUpdateName, bStoredProcedure:=ACUpdateStored, lRecordsAffected:=lRecordsAffected)

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
    ' Name: DeleteAllItems (Public)
    '
    ' Description: Deletes all records for this lookup from the db
    '
    ' ***************************************************************** '

    Public Function DeleteAllItems() As Integer
        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=m_sDeleteAllSQL, sSQLName:=m_sDeleteAllName, bStoredProcedure:=ACDeleteAllStored, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteAllItems Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAllItems", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: DeleteItem (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    '
    ' ***************************************************************** '
    Private Function DeleteItem(ByRef oLookup As bGEMLookup.Lookup) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the LookupID INPUT parameter
        m_lReturn = m_oDatabase.Parameters.Add(sName:=m_sLookupName & "_id", vValue:=CStr(oLookup.LookupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=m_sDeleteSQL, sSQLName:=m_sDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)

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
    ' Description: Sets the supplied Lookup properties from a database
    '              record.
    ' ***************************************************************** '
    Private Function SetPropertiesFromDB(ByRef oLookup As bGEMLookup.Lookup, ByRef lRecordNumber As Integer) As Integer

        Dim result As Integer = 0
        'Developer Guide No 21
        'Dim oFields As ADODB.Fields
        Dim oFields As DataRow



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set oFields to refer to Field Collection for Record 1
        oFields = m_oDatabase.Records.Item(lRecordNumber).Fields()

        ' Populate Base Details

        With oLookup

            .LookupID = oFields(m_sLookupName & "_id")

            If Convert.IsDBNull(oFields("description")) Or IsNothing(oFields("description")) Then
                .Description = ""
            Else
                .Description = oFields("description")
            End If
            .DatabaseStatus = gPMConstants.PMEComponentAction.PMView

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Private)
    '
    ' Description: Sets the supplied Lookup property values.
    '
    ' ***************************************************************** '
    Private Function SetProperties(ByRef oLookup As bGEMLookup.Lookup, ByRef iStatus As Integer, Optional ByRef vLookupID As Integer = 0, Optional ByRef vdescription As String = "") As Integer


        Dim result As Integer = 0
        Dim bDataChanged As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        ' If Add Mode
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            ' Check that All Mandatory Parameters have been supplied
            m_lReturn = CType(CheckMandatory(vLookupID:=vLookupID, vdescription:=vdescription), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Default Any Missing Parameters
            m_lReturn = CType(DefaultParameters(bDefaultAll:=False, vLookupID:=vLookupID, vdescription:=vdescription), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

        End If

        ' Validate Parameters
        m_lReturn = CType(Validate(vLookupID:=vLookupID, vdescription:=vdescription), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        ' Set Data Changed Flag to False
        bDataChanged = False


        ' Set Property values.
        With oLookup


            If Not Information.IsNothing(vLookupID) Then
                If .LookupID <> vLookupID Then
                    .LookupID = vLookupID
                    bDataChanged = True
                End If
            End If

            If Not Information.IsNothing(vdescription) Then
                If .Description.Trim() <> vdescription.Trim() Then
                    .Description = vdescription
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
    ' Description: Returns the supplied Lookup property values.
    '
    ' ***************************************************************** '
    'Developer Guide No 101
    'Private Function GetProperties(ByRef oLookup As bGEMLookup.Lookup, ByRef iStatus As Integer, Optional ByRef vLookupID As Integer = 0, Optional ByRef vdescription As String = "") As Integer
    Private Function GetProperties(ByRef oLookup As bGEMLookup.Lookup, ByRef iStatus As Integer, Optional ByRef vLookupID As Object = Nothing, Optional ByRef vdescription As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set Property values.
        With oLookup


            If Not Information.IsNothing(vLookupID) Then
                vLookupID = .LookupID
            End If

            If Not Information.IsNothing(vdescription) Then
                vdescription = .Description
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
    Private Function AddInputParam(ByRef oLookup As bGEMLookup.Lookup) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:=m_sLookupName & "_txt", vValue:=oLookup.Description, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DefaultParameters (Public)
    '
    ' Description: Sets the Default Values for a Lookup.
    '
    ' ***************************************************************** '
    'Developer Guide No 101
    'Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vLookupID As Byte = 0, Optional ByRef vdescription As String = "") As Integer
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vLookupID As Object = Nothing, Optional ByRef vdescription As Object = Nothing) As Integer


        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue



        If (Information.IsNothing(vLookupID)) Or (vLookupID.Equals(0)) Or (bDefaultAll) Then
            vLookupID = 0
        End If



        If (Information.IsNothing(vdescription)) Or (String.IsNullOrEmpty(vdescription)) Or (bDefaultAll) Then
            vdescription = ""
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Public)
    '
    ' Description: Sets the Default Values for a Lookup.
    '
    ' ***************************************************************** '
    Private Function CheckMandatory(Optional ByRef vLookupID As Object = Nothing, Optional ByRef vdescription As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue



        If (Information.IsNothing(vdescription)) Or (Object.Equals(vdescription, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the Lookup for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vLookupID As Object = Nothing, Optional ByRef vdescription As Object = Nothing) As Integer


        Dim result As Integer = 0




        result = gPMConstants.PMEReturnCode.PMTrue


        Dim dbNumericTemp As Double
        If Not Double.TryParse(CStr(vLookupID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
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



    'Developer Guide No 101
    'Private Function GetData(ByRef vResultArray As Object, Optional ByRef vID As Object = Nothing, Optional ByRef v_vObjectID As Object = Nothing, Optional ByRef vTablePrefix As String = "") As Integer
    Private Function GetData(ByRef vResultArray(,) As Object, Optional ByRef vID As Object = Nothing, Optional ByRef v_vObjectID As Object = Nothing, Optional ByRef vTablePrefix As Object = Nothing) As Integer


        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Is ID Supplied

        If Not Information.IsNothing(vID) Then

            ' Add the LookupID parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurer_no", vValue:=CStr(vID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_sGetDetailsSQL = "{call sp_GEM_select_SchByIns (?)}"
            ' Execute SQL Statement

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=m_sGetDetailsSQL, sSQLName:=m_sGetDetailsName, bStoredProcedure:=ACGetDetailsStored, lNumberRecords:=0, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        ElseIf (Not Information.IsNothing(v_vObjectID)) Then

            ' Add the LookupID parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="object_id", vValue:=CStr(v_vObjectID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'MN160799 - Need to account for HKJ
            '(IB)290799 - added ismissing trap!

            If Information.IsNothing(vTablePrefix) Then
                m_sGetDetailsSQL = "{call sp_GEM_select_NP_Properties (?)}"
            Else
                If vTablePrefix = HKJTablePrefix Then

                    m_sGetDetailsSQL = "{call sp_GEMHKJ_select_np_properties (?)}"

                Else
                    m_sGetDetailsSQL = "{call sp_GEM_select_NP_Properties (?)}"

                End If
            End If
            ' Execute SQL Statement

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=m_sGetDetailsSQL, sSQLName:=m_sGetDetailsName, bStoredProcedure:=ACGetDetailsStored, lNumberRecords:=0, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Else

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=m_sGetAllDetailsSQL, sSQLName:=m_sGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: FillTable (Private)
    '
    ' Description: Selects all lookups for the table given
    '
    '
    ' ***************************************************************** '
    Private Function FillTable(ByRef vTableArray(,) As Object, ByRef vResultArray(,) As Object, Optional ByVal v_vInsurerNo As Object = Nothing, Optional ByVal v_vObjectID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim vLookupItems(,) As Object
        Dim sTableName As String = ""
        Dim vLookupKey As String = ""
        Dim lNumOfItems, lStartPosition As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Reset Result Array
        vResultArray = Nothing

        ' Set Start Position to beginning
        lStartPosition = 0
        ' This is listed from PMLookup and retained just in case
        ' For each Lookup Table in the array
        For lTableRow As Integer = vTableArray.GetLowerBound(1) To vTableArray.GetUpperBound(1)

            ' Get the Lookup Table Name

            sTableName = CStr(vTableArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, lTableRow)).Trim()

            If sTableName = "" Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Lookup Table Name Must be supplied at position " & lTableRow, vApp:=ACApp, vClass:=ACClass, vMethod:="FillTable")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Lookup Key

            vLookupKey = CStr(vTableArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, lTableRow)).Trim()

            ' Reset Lookup Items
            vLookupItems = Nothing

            If sTableName = "schemes" Then

                m_lReturn = CType(GetData(vResultArray:=vLookupItems, vID:=v_vInsurerNo), gPMConstants.PMEReturnCode)

            ElseIf (sTableName = "NP_Properties") Then
                m_lReturn = CType(GetData(vResultArray:=vLookupItems, v_vObjectID:=v_vObjectID), gPMConstants.PMEReturnCode)
                'MN160799 - Must use the HKJ properties in some cases
            ElseIf (sTableName = "HKJNP_Properties") Then

                m_lReturn = CType(GetData(vResultArray:=vLookupItems, v_vObjectID:=v_vObjectID, vTablePrefix:=HKJTablePrefix), gPMConstants.PMEReturnCode)

            Else
                '  get all Lookups for this table
                m_lReturn = CType(GetData(vResultArray:=vLookupItems), gPMConstants.PMEReturnCode)
            End If


            ' Check Return Code from select
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Were any LookupItems returned
            If Information.IsArray(vLookupItems) Then

                lNumOfItems = vLookupItems.GetUpperBound(1) + 1
            Else
                lNumOfItems = 0
            End If

            ' Set Start Position and Number of Items in Table Array

            vTableArray(gPMConstants.PMELookupInArrayColPos.PMLookupStartPos, lTableRow) = lStartPosition

            vTableArray(gPMConstants.PMELookupInArrayColPos.PMLookupNumOfItems, lTableRow) = lNumOfItems

            ' If we have any Items
            If lNumOfItems > 0 Then

                ' Is this the first set of lookup items
                If Information.IsArray(vResultArray) Then

                    ' No, Add the lookup items to the end of the results array

                    ' Resize the Results array

                    ReDim Preserve vResultArray(vResultArray.GetUpperBound(0), vResultArray.GetUpperBound(1) + lNumOfItems)

                    ' Add the Lookup Items to the end of the results array

                    For lItemRow As Integer = vLookupItems.GetLowerBound(1) To vLookupItems.GetUpperBound(1)

                        vResultArray(gPMConstants.PMELookupOutArrayColPos.PMLookupID, lStartPosition) = vLookupItems(gPMConstants.PMELookupOutArrayColPos.PMLookupID, lItemRow)

                        vResultArray(gPMConstants.PMELookupOutArrayColPos.PMLookupCaption, lStartPosition) = vLookupItems(gPMConstants.PMELookupOutArrayColPos.PMLookupCaption, lItemRow)

                        vResultArray(gPMConstants.PMELookupOutArrayColPos.PMLookupCode, lStartPosition) = vLookupItems(gPMConstants.PMELookupOutArrayColPos.PMLookupCode, lItemRow)

                        lStartPosition += 1

                    Next lItemRow

                Else

                    ' Yes, we can simply assign the results


                    vResultArray = vLookupItems
                    lStartPosition += lNumOfItems

                End If

            End If

        Next lTableRow

        ' Reset Lookup Items
        vLookupItems = Nothing

        Return result

    End Function
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class

