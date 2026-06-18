Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Windows.Forms
'developer guide no. 129
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
    '              a ACTFindInvoice.
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

    ' Collection of ACTFindInvoices (Private)
    Private m_oACTFindInvoices As bACTFindInvoice.ACTFindInvoices

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Error Code (Private)
    Private m_lError As Integer

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
                Case Is > m_oACTFindInvoices.Count()
                    m_lCurrentRecord = m_oACTFindInvoices.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Numner in Collection
            Return m_oACTFindInvoices.Count()

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

            m_oDatabase = New dPMDAO.Database()

            ' Open the Database
            'SD 23/07/2002 extra paramaters
            m_lReturn = m_oDatabase.OpenDatabase(sSiriusUsername:=m_sUsername, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, sCallingAppName:=ACApp, vDSN:=gPMConstants.PMOrionDSN)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Close Database in Terminate() method
            m_bCloseDatabase = True

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

            ' Create ACTFindInvoices Collection
            m_oACTFindInvoices = New bACTFindInvoice.ACTFindInvoices()

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
            If disposing Then
                m_oACTFindInvoices = Nothing
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
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single ACTFindInvoice directly into the database.
    '        Note: The ACTFindInvoice will NOT be added to the collection.
    '
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByRef vInvoiceID As Integer = 0, Optional ByRef vInvoiceRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vInvoiceDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesInvoiceID As Object = Nothing, Optional ByRef vInvoiceStatusID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oACTFindInvoice As bACTFindInvoice.ACTFindInvoice

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new ACTFindInvoice
            oACTFindInvoice = New bACTFindInvoice.ACTFindInvoice()

            ' Populate ACTFindInvoice Attributes






            m_lReturn = SetProperties(oACTFindInvoice, gPMConstants.PMEComponentAction.PMAdd, vInvoiceID:=vInvoiceID, vInvoiceRef:=CStr(vInvoiceRef), vPeriodID:=CInt(vPeriodID), vInvoiceDescription:=CStr(vInvoiceDescription), vPeriodYearName:=CStr(vPeriodYearName), vRevisesInvoiceID:=CInt(vRevisesInvoiceID), vInvoiceStatusID:=CInt(vInvoiceStatusID))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the ACTFindInvoice to the Database
            m_lReturn = AddItem(oACTFindInvoice)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}

            ' Return the ID of the ACTFindInvoice Added

            If Not Information.IsNothing(vInvoiceID) Then
                vInvoiceID = oACTFindInvoice.InvoiceID
            End If

            ' {* USER DEFINED CODE (End) *}

            oACTFindInvoice = Nothing

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
    ' Description: Deletes a single ACTFindInvoice directly from the database.
    '        Note: The ACTFindInvoice will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByRef vInvoiceID As Object = Nothing, Optional ByRef vInvoiceRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vInvoiceDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesInvoiceID As Object = Nothing, Optional ByRef vInvoiceStatusID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oACTFindInvoice As bACTFindInvoice.ACTFindInvoice

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new ACTFindInvoice
            oACTFindInvoice = New bACTFindInvoice.ACTFindInvoice()

            ' Populate ACTFindInvoice Attributes







            m_lReturn = SetProperties(oACTFindInvoice, gPMConstants.PMEComponentAction.PMDelete, vInvoiceID:=CInt(vInvoiceID), vInvoiceRef:=CStr(vInvoiceRef), vPeriodID:=CInt(vPeriodID), vInvoiceDescription:=CStr(vInvoiceDescription), vPeriodYearName:=CStr(vPeriodYearName), vRevisesInvoiceID:=CInt(vRevisesInvoiceID), vInvoiceStatusID:=CInt(vInvoiceStatusID))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the ACTFindInvoice to the Database
            m_lReturn = DeleteItem(oACTFindInvoice)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oACTFindInvoice = Nothing

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
    ' Description: Returns the Default Values for the ACTFindInvoice.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vInvoiceID As Object = Nothing, Optional ByRef vInvoiceRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vInvoiceDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesInvoiceID As Object = Nothing, Optional ByRef vInvoiceStatusID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults
            m_lReturn = DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vInvoiceID:=CByte(vInvoiceID), vInvoiceRef:=CStr(vInvoiceRef), vPeriodID:=CByte(vPeriodID), vInvoiceDescription:=CStr(vInvoiceDescription), vPeriodYearName:=CStr(vPeriodYearName), vRevisesInvoiceID:=CByte(vRevisesInvoiceID), vInvoiceStatusID:=CByte(vInvoiceStatusID))

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
        Dim oFields As ADODB.Fields

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
            '    If (Trim$(vTable) <> PMTableACTFindInvoice) Then

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
            oFields = m_oDatabase.Records.Item(1).Fields()

            With oFields

                ' For Each Field requested
                For lSub As Integer = vFieldArray.GetLowerBound(0) To vFieldArray.GetUpperBound(0)

                    'PWF 230702 - scalability change - check for null value

                    If Not (Convert.IsDBNull(oFields(vFieldArray(lSub))) Or IsNothing(oFields(vFieldArray(lSub)))) Then
                        ' Store the results in the Temporary results array

                        vResults(lSub) = oFields(vFieldArray(lSub))
                    Else
                        Select Case oFields(vFieldArray(lSub)).Type
                            'developer guide no. 47 (No Solution)
                            'Case DbType.String, DbType.String, DbType.String, adLongVarWChar, DbType.String, DbType.String, adWChar
                            Case DbType.String, DbType.String, DbType.String, DbType.String, DbType.String

                                vResults(lSub) = ""
                                'developer guide no. 47 (No Solution)
                                ' Case DbType.Date, adDBDate
                            Case DbType.Date

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
    ' Description: Gets the required ACTFindInvoices and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vACTFindInvoiceID As Object = Nothing, Optional ByRef vLockMode As gPMConstants.PMELockMode = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oACTFindInvoice As bACTFindInvoice.ACTFindInvoice

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oACTFindInvoices.Clear()

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

            If Not Information.IsNothing(vACTFindInvoiceID) Then

                ' Yes, Is the key valid

                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(CStr(vACTFindInvoiceID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vACTFindInvoiceID =" & CStr(vACTFindInvoiceID), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                    Return result

                End If

                ' Add the ACTFindInvoiceID parameter (INPUT)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Invoice_id", vValue:=CStr(vACTFindInvoiceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

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

                    ' Create New ACTFindInvoice
                    oACTFindInvoice = New bACTFindInvoice.ACTFindInvoice()

                    m_lReturn = SetPropertiesFromDB(oACTFindInvoice:=oACTFindInvoice, lRecordNumber:=lSub)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    ' Add ACTFindInvoice to collection
                    m_lReturn = m_oACTFindInvoices.Add(oNewACTFindInvoice:=oACTFindInvoice)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oACTFindInvoice = Nothing

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
    ' Name: GetInvoiceRef
    '
    ' Description: Gets the reference for the passed Invoice_id
    '
    ' ***************************************************************** '
    Public Function GetInvoiceRef(ByVal v_lInvoiceID As Integer, ByRef r_sReference As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Construct the SQL
            sSQL = "SELECT Invoice_ref FROM Invoice WHERE Invoice_id = " & v_lInvoiceID

            ' Perform the operation
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetInvoiceRef", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the return value
            If Information.IsArray(vResultArray) Then

                r_sReference = CStr(vResultArray(0, 0))
            Else
                r_sReference = ""
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInvoiceRefFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInvoiceRef", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a DocumentType.
    '
    '
    ' ***************************************************************** '
    'Developer Guide No. 12
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim dtEffectiveDate As Date

        ' {* USER DEFINED CODE (Begin) *}
        Dim vTabArray(3, 0) As Object
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

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = gACTLibrary.ACTLookupPostingStatus


            ' {* USER DEFINED CODE (End) *}

            ' Do we have any records
            If m_lCurrentRecord < 1 Then
                ' No, we can only lookup all
                iLookupType = gPMConstants.PMELookupType.PMLookupAll
            Else

            End If

            Select Case iLookupType
                Case gPMConstants.PMELookupType.PMLookupAll

                    ' Do not supply a key

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""

                    ' Default Effective Date to current date/time
                    dtEffectiveDate = DateTime.Now

                Case gPMConstants.PMELookupType.PMLookupAllEffective
                    'complete when Document object is defined

                    ' Use keys and effective date from current record
                    ' Note: The keys are not used for the select, but are used by
                    '       the iterface program to set the list index.
                    'With oInsuranceFile

                    ' {* USER DEFINED CODE (Begin) *}
                    '    vTabArray(PMLookupKey, 0) = .DocumentTypeID
                    '    dtEffectiveDate = .EffectiveDate
                    ' {* USER DEFINED CODE (End) *}

                    'End With

                Case gPMConstants.PMELookupType.PMLookupSingle

                    ' Set keys from current record
                    'With oInsuranceFile

                    ' {* USER DEFINED CODE (Begin) *}
                    '    vTabArray(PMLookupKey, 0) = .DocumentTypeID
                    ' {* USER DEFINED CODE (End) *}

                    'End With
                    ' Default Effective Date to current date/time
                    'dtEffectiveDate = Now

            End Select

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
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required ACTFindInvoices and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vInvoiceID As Object = Nothing, Optional ByRef vInvoiceRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vInvoiceDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesInvoiceID As Object = Nothing, Optional ByRef vInvoiceStatusID As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oACTFindInvoice As bACTFindInvoice.ACTFindInvoice
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oACTFindInvoices.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oACTFindInvoice = m_oACTFindInvoices.Item(m_lCurrentRecord)

            ' Get the ACTFindInvoice Property Values

            m_lReturn = GetProperties(oACTFindInvoice, iStatus, vInvoiceID:=CInt(vInvoiceID), vInvoiceRef:=CStr(vInvoiceRef), vPeriodID:=CInt(vPeriodID), vInvoiceDescription:=CStr(vInvoiceDescription), vPeriodYearName:=CStr(vPeriodYearName), vRevisesInvoiceID:=CInt(vRevisesInvoiceID), vInvoiceStatusID:=CInt(vInvoiceStatusID))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oACTFindInvoice = Nothing


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
    ' Description: Adds the supplied ACTFindInvoice into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vInvoiceID As Object = Nothing, Optional ByRef vInvoiceRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vInvoiceDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesInvoiceID As Object = Nothing, Optional ByRef vInvoiceStatusID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oACTFindInvoice As bACTFindInvoice.ACTFindInvoice

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oACTFindInvoices.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new ACTFindInvoice
            oACTFindInvoice = New bACTFindInvoice.ACTFindInvoice()

            ' Populate ACTFindInvoice Attributes

            m_lReturn = SetProperties(oACTFindInvoice, gPMConstants.PMEComponentAction.PMAdd, vInvoiceID:=CInt(vInvoiceID), vInvoiceRef:=CStr(vInvoiceRef), vPeriodID:=CInt(vPeriodID), vInvoiceDescription:=CStr(vInvoiceDescription), vPeriodYearName:=CStr(vPeriodYearName), vRevisesInvoiceID:=CInt(vRevisesInvoiceID), vInvoiceStatusID:=CInt(vInvoiceStatusID))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oACTFindInvoice = Nothing
                Return result
            End If

            ' Add ACTFindInvoice to collection
            m_lReturn = m_oACTFindInvoices.Add(oNewACTFindInvoice:=oACTFindInvoice)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oACTFindInvoice = Nothing
                Return result
            End If

            oACTFindInvoice = Nothing

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
    ' Description: Validates that this action is valid on the ACTFindInvoice
    '              specified and updates the ACTFindInvoice with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vInvoiceID As Object = Nothing, Optional ByRef vInvoiceRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vInvoiceDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesInvoiceID As Object = Nothing, Optional ByRef vInvoiceStatusID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oACTFindInvoice As bACTFindInvoice.ACTFindInvoice
        Dim iStatus As gPMConstants.PMEComponentAction

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oACTFindInvoices.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oACTFindInvoice = m_oACTFindInvoices.Item(lRow)

            ' Check the Status of the ACTFindInvoice

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oACTFindInvoice.DatabaseStatus
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

            ' Update ACTFindInvoice Attributes

            m_lReturn = SetProperties(oACTFindInvoice, iStatus, vInvoiceID:=CInt(vInvoiceID), vInvoiceRef:=CStr(vInvoiceRef), vPeriodID:=CInt(vPeriodID), vInvoiceDescription:=CStr(vInvoiceDescription), vPeriodYearName:=CStr(vPeriodYearName), vRevisesInvoiceID:=CInt(vRevisesInvoiceID), vInvoiceStatusID:=CInt(vInvoiceStatusID))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oACTFindInvoice = Nothing
                Return result
            End If

            ' Release reference to ACTFindInvoice
            oACTFindInvoice = Nothing

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
    ' Description: Validate that the specified ACTFindInvoice can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oACTFindInvoice As bACTFindInvoice.ACTFindInvoice

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oACTFindInvoices.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oACTFindInvoice = m_oACTFindInvoices.Item(lRow)

            ' Check the Status of the ACTFindInvoice

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oACTFindInvoice.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oACTFindInvoice.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oACTFindInvoice.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to ACTFindInvoice
            oACTFindInvoice = Nothing

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
            For lSub As Integer = 1 To m_oACTFindInvoices.Count()
                Select Case m_oACTFindInvoices.Item(lSub).DatabaseStatus
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
        Dim oACTFindInvoice As bACTFindInvoice.ACTFindInvoice
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oACTFindInvoices.Count()
                oACTFindInvoice = m_oACTFindInvoices.Item(lSub)


                Select Case oACTFindInvoice.DatabaseStatus
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
                        m_lReturn = AddItem(oACTFindInvoice)
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
                        m_lReturn = UpdateItem(oACTFindInvoice)
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
                        m_lReturn = DeleteItem(oACTFindInvoice)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Release last reference
            oACTFindInvoice = Nothing

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
                    Do While lSub <= m_oACTFindInvoices.Count()

                        ' With the item
                        With m_oACTFindInvoices.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oACTFindInvoices.Delete(lSub)

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
    ' Name: SearchByQuery
    '
    ' Description: Search for Invoice
    '
    ' ***************************************************************** '
    Public Function SearchByQuery(ByRef lNumberOfRecords As Integer, ByRef vResultArray(,) As Object, Optional ByVal v_vReference As String = "", Optional ByVal v_vYear As Object = Nothing, Optional ByVal v_vStatus As Object = Nothing) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT  a.short_code, " & _
                   "i.invoice_date, " & _
                   "i.reference, " & _
                   "i.invoice_number, " & _
                   "i.description, " & _
                   "i.order_no, " & _
                   "i.code, " & _
                   "i.invoice_id " & _
                   "FROM Invoice i, Account a " & _
                   "WHERE (i.account_id = a.account_id) "

            ' Invoice reference

            If Not Information.IsNothing(v_vReference) Then
                If v_vReference <> "" Then
                    sSQL = sSQL & "And"

                    v_vReference = bPMFunc.ConvertWildCard(v_sSearchString:=v_vReference)
                    sSQL = sSQL & " a.short_code LIKE '" & v_vReference & "'" & Strings.Chr(13) & Strings.Chr(10)

                End If
            End If

            'add the order by clause
            sSQL = sSQL & "  Order by a.short_code " & Strings.Chr(13) & Strings.Chr(10)

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="SearchByQuery", bStoredProcedure:=False, lNumberRecords:=lNumberOfRecords, vResultArray:=vResultArray)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchByQuery")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'Check the returned result array

            Return CType(CheckResults(vResultArray), gPMConstants.PMEReturnCode)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchByQueryFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchByQuery", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Private Function AddItem(ByRef oACTFindInvoice As bACTFindInvoice.ACTFindInvoice) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters
        m_lReturn = AddInputParam(oACTFindInvoice:=oACTFindInvoice)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add ACTFindInvoiceID as an OUTPUT param for an insert
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Invoice_id", vValue:=CStr(oACTFindInvoice.InvoiceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the ID of the record inserted
        oACTFindInvoice.InvoiceID = gPMFunctions.NullToLong(m_oDatabase.Parameters.Item("Invoice_id").Value)

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
    Private Function UpdateItem(ByRef oACTFindInvoice As bACTFindInvoice.ACTFindInvoice) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the Input Parameters to the Parameteres collection
        m_lReturn = AddInputParam(oACTFindInvoice:=oACTFindInvoice)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add ACTFindInvoiceID as an INPUT param for an update
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Invoice_id", vValue:=CStr(oACTFindInvoice.InvoiceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' BB Not required by Orion
        ' Add Timestamp as an INPUT param for an update
        'm_lReturn& = m_oDatabase.Parameters.Add( _
        'sName:="timestamp", _
        'vValue:=oACTFindInvoice.Timestamp, _
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
    Private Function DeleteItem(ByRef oACTFindInvoice As bACTFindInvoice.ACTFindInvoice) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the ACTFindInvoiceID INPUT parameter
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Invoice_id", vValue:=CStr(oACTFindInvoice.InvoiceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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
    ' Description: Sets the supplied ACTFindInvoice properties from a database
    '              record.
    ' ***************************************************************** '
    Private Function SetPropertiesFromDB(ByRef oACTFindInvoice As bACTFindInvoice.ACTFindInvoice, ByRef lRecordNumber As Integer) As Integer

        Dim result As Integer = 0
        Dim oFields As ADODB.Fields



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set oFields to refer to Field Collection for Record 1
        oFields = m_oDatabase.Records.Item(lRecordNumber).Fields()

        ' Populate Base Details

        With oACTFindInvoice

            'PWF 230702 - scalability change - check for null value
            .InvoiceID = gPMFunctions.NullToLong(oFields("Invoice_id"))
            .InvoiceRef = gPMFunctions.NullToString(oFields("Invoice_ref"))
            .PeriodID = gPMFunctions.NullToLong(oFields("period_id"))
            .InvoiceDescription = gPMFunctions.NullToString(oFields("Invoice_description"))
            .PeriodYearName = gPMFunctions.NullToString(oFields("period_year_name"))
            .RevisesInvoiceID = gPMFunctions.NullToLong(oFields("revises_Invoice_id"))
            .InvoiceStatusID = gPMFunctions.NullToLong(oFields("Invoice_status_id"))

            .DatabaseStatus = gPMConstants.PMEComponentAction.PMView

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Private)
    '
    ' Description: Sets the supplied ACTFindInvoice property values.
    '
    ' ***************************************************************** '
    Private Function SetProperties(ByRef oACTFindInvoice As bACTFindInvoice.ACTFindInvoice, ByRef iStatus As Integer, Optional ByRef vInvoiceID As Integer = 0, Optional ByRef vInvoiceRef As String = "", Optional ByRef vPeriodID As Integer = 0, Optional ByRef vInvoiceDescription As String = "", Optional ByRef vPeriodYearName As String = "", Optional ByRef vRevisesInvoiceID As Integer = 0, Optional ByRef vInvoiceStatusID As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        ' If Add Mode
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            ' Check that All Mandatory Parameters have been supplied
            m_lReturn = CheckMandatory(vInvoiceID:=vInvoiceID, vInvoiceRef:=vInvoiceRef, vPeriodID:=vPeriodID, vInvoiceDescription:=vInvoiceDescription, vPeriodYearName:=vPeriodYearName, vRevisesInvoiceID:=vRevisesInvoiceID, vInvoiceStatusID:=vInvoiceStatusID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Default Any Missing Parameters
            m_lReturn = DefaultParameters(bDefaultAll:=False, vInvoiceID:=vInvoiceID, vInvoiceRef:=vInvoiceRef, vPeriodID:=vPeriodID, vInvoiceDescription:=vInvoiceDescription, vPeriodYearName:=vPeriodYearName, vRevisesInvoiceID:=vRevisesInvoiceID, vInvoiceStatusID:=vInvoiceStatusID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

        End If

        ' Validate Parameters
        m_lReturn = Validate(vInvoiceID:=vInvoiceID, vInvoiceRef:=vInvoiceRef, vPeriodID:=vPeriodID, vInvoiceDescription:=vInvoiceDescription, vPeriodYearName:=vPeriodYearName, vRevisesInvoiceID:=vRevisesInvoiceID, vInvoiceStatusID:=vInvoiceStatusID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        ' Set Data Changed Flag to False
        bDataChanged = False


        ' Set Property values.
        With oACTFindInvoice


            If Not Information.IsNothing(vInvoiceID) Then
                If .InvoiceID <> vInvoiceID Then
                    .InvoiceID = vInvoiceID
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vInvoiceRef) Then
                If .InvoiceRef.Trim() <> vInvoiceRef.Trim() Then
                    .InvoiceRef = vInvoiceRef
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vPeriodID) Then
                If .PeriodID <> vPeriodID Then
                    .PeriodID = vPeriodID
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vInvoiceDescription) Then
                If .InvoiceDescription.Trim() <> vInvoiceDescription.Trim() Then
                    .InvoiceDescription = vInvoiceDescription
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vPeriodYearName) Then
                If .PeriodYearName.Trim() <> vPeriodYearName.Trim() Then
                    .PeriodYearName = vPeriodYearName
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vRevisesInvoiceID) Then
                If .RevisesInvoiceID <> vRevisesInvoiceID Then
                    .RevisesInvoiceID = vRevisesInvoiceID
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vInvoiceStatusID) Then
                If .InvoiceStatusID <> vInvoiceStatusID Then
                    .InvoiceStatusID = vInvoiceStatusID
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
    ' Description: Returns the supplied ACTFindInvoice property values.
    '
    ' ***************************************************************** '
    Private Function GetProperties(ByRef oACTFindInvoice As bACTFindInvoice.ACTFindInvoice, ByRef iStatus As Integer, Optional ByRef vInvoiceID As Integer = 0, Optional ByRef vInvoiceRef As String = "", Optional ByRef vPeriodID As Integer = 0, Optional ByRef vInvoiceDescription As String = "", Optional ByRef vPeriodYearName As String = "", Optional ByRef vRevisesInvoiceID As Integer = 0, Optional ByRef vInvoiceStatusID As Integer = 0) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set Property values.
        With oACTFindInvoice


            If Not Information.IsNothing(vInvoiceID) Then
                vInvoiceID = .InvoiceID
            End If


            If Not Information.IsNothing(vInvoiceRef) Then
                vInvoiceRef = .InvoiceRef
            End If


            If Not Information.IsNothing(vPeriodID) Then
                vPeriodID = .PeriodID
            End If


            If Not Information.IsNothing(vInvoiceDescription) Then
                vInvoiceDescription = .InvoiceDescription
            End If


            If Not Information.IsNothing(vPeriodYearName) Then
                vPeriodYearName = .PeriodYearName
            End If


            If Not Information.IsNothing(vRevisesInvoiceID) Then
                vRevisesInvoiceID = .RevisesInvoiceID
            End If


            If Not Information.IsNothing(vInvoiceStatusID) Then
                vInvoiceStatusID = .InvoiceStatusID
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
    Private Function AddInputParam(ByRef oACTFindInvoice As bACTFindInvoice.ACTFindInvoice) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase


            m_lReturn = .Parameters.Add(sName:="Invoice_id", vValue:=CStr(oACTFindInvoice.InvoiceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="Invoice_ref", vValue:=oACTFindInvoice.InvoiceRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oACTFindInvoice.PeriodID < 1 Then

                'developer guide no. 85
                m_lReturn = .Parameters.Add(sName:="period_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="period_id", vValue:=CStr(oACTFindInvoice.PeriodID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="Invoice_description", vValue:=oACTFindInvoice.InvoiceDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="period_year_name", vValue:=oACTFindInvoice.PeriodYearName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oACTFindInvoice.RevisesInvoiceID < 1 Then

                'developer guide no. 85
                m_lReturn = .Parameters.Add(sName:="revises_Invoice_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="revises_Invoice_id", vValue:=CStr(oACTFindInvoice.RevisesInvoiceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="Invoice_status_id", vValue:=CStr(oACTFindInvoice.InvoiceStatusID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DefaultParameters (Public)
    '
    ' Description: Sets the Default Values for a ACTFindInvoice.
    '
    ' ***************************************************************** '
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vInvoiceID As Byte = 0, Optional ByRef vInvoiceRef As String = "", Optional ByRef vPeriodID As Byte = 0, Optional ByRef vInvoiceDescription As String = "", Optional ByRef vPeriodYearName As String = "", Optional ByRef vRevisesInvoiceID As Byte = 0, Optional ByRef vInvoiceStatusID As Byte = 0) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Information.IsNothing(vInvoiceID)) Or (vInvoiceID.Equals(0)) Or (bDefaultAll) Then
            vInvoiceID = 0
        End If



        If (Information.IsNothing(vInvoiceRef)) Or (String.IsNullOrEmpty(vInvoiceRef)) Or (bDefaultAll) Then
            vInvoiceRef = ""
        End If



        If (Information.IsNothing(vPeriodID)) Or (vPeriodID.Equals(0)) Or (bDefaultAll) Then
            vPeriodID = 0
        End If


        If (Information.IsNothing(vInvoiceDescription)) Or (String.IsNullOrEmpty(vInvoiceDescription)) Or (bDefaultAll) Then
            vInvoiceDescription = ""
        End If


        If (Information.IsNothing(vPeriodYearName)) Or (String.IsNullOrEmpty(vPeriodYearName)) Or (bDefaultAll) Then
            vPeriodYearName = ""
        End If


        If (Information.IsNothing(vRevisesInvoiceID)) Or (vRevisesInvoiceID.Equals(0)) Or (bDefaultAll) Then
            vRevisesInvoiceID = 0
        End If


        If (Information.IsNothing(vInvoiceStatusID)) Or (vInvoiceStatusID.Equals(0)) Or (bDefaultAll) Then
            vInvoiceStatusID = 0
        End If


        ' {* USER DEFINED CODE (End) *}


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Public)
    '
    ' Description: Sets the Default Values for a ACTFindInvoice.
    '
    ' ***************************************************************** '
    Private Function CheckMandatory(Optional ByRef vInvoiceID As Object = Nothing, Optional ByRef vInvoiceRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vInvoiceDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesInvoiceID As Object = Nothing, Optional ByRef vInvoiceStatusID As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Information.IsNothing(vInvoiceID)) Or (Object.Equals(vInvoiceID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If


        If (Information.IsNothing(vInvoiceRef)) Or (Object.Equals(vInvoiceRef, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If


        If (Information.IsNothing(vPeriodID)) Or (Object.Equals(vPeriodID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If


        If (Information.IsNothing(vInvoiceDescription)) Or (Object.Equals(vInvoiceDescription, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If


        If (Information.IsNothing(vPeriodYearName)) Or (Object.Equals(vPeriodYearName, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If


        If (Information.IsNothing(vRevisesInvoiceID)) Or (Object.Equals(vRevisesInvoiceID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If


        If (Information.IsNothing(vInvoiceStatusID)) Or (Object.Equals(vInvoiceStatusID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If


        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the ACTFindInvoice for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vInvoiceID As Object = Nothing, Optional ByRef vInvoiceRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vInvoiceDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesInvoiceID As Object = Nothing, Optional ByRef vInvoiceStatusID As Object = Nothing) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        Dim dbNumericTemp As Double
        If Not Double.TryParse(CStr(vInvoiceID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp2 As Double
        If Not Double.TryParse(CStr(vPeriodID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp3 As Double
        If Not Double.TryParse(CStr(vRevisesInvoiceID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp4 As Double
        If Not Double.TryParse(CStr(vInvoiceStatusID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
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

    ' ***************************************************************** '
    ' Name: CheckResults (Private)
    '
    ' Description: Checks the result array after a query
    '              If records found returns PMTrue
    '              If no records found returns PMNotFound
    '
    ' ***************************************************************** '
    Private Function CheckResults(ByRef vResultArray As Object) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' If NO records were found return PMNotFound
        If Not Information.IsArray(vResultArray) Then
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
