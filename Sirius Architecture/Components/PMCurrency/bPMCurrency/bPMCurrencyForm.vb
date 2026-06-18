Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
Imports SSP.Shared
'developer guide no.129
<System.Runtime.InteropServices.ProgId("Form_NET.Form")>
Public NotInheritable Class Form
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 11/07/1997
    '
    ' Description: Creatable Form class which contains all the
    '              methods, Form rules required to manipulate
    '              a Currency.
    '
    ' Edit History:
    ' DAK221299 - PMProductLookup.GetDetails - PMProductID changed to
    '             variant.
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 18/09/2003
    ' Username.
    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Form"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Collection of Currencys (Private)
    Private m_oCurrencys As bPMCurrency.Currencys

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As gPMConstants.PMEReturnCode

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
    ' Insurance File ID
    Private m_lInsuranceFileID As Integer
    ' Risk ID
    Private m_lRiskID As Integer

    ' Caption lookup
    Private m_oCaption As bPMCaption.Business
    'Private m_oCaption As bPMCaption.Business



    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusArchitecture

        End Get
    End Property

    Public Property CurrentRecord() As Integer
        Get

            Return m_lCurrentRecord

        End Get
        Set(ByVal Value As Integer)

            Select Case Value
                Case Is < 1
                    m_lCurrentRecord = gPMConstants.PMEReturnCode.PMFalse
                Case Is > m_oCurrencys.Count()
                    m_lCurrentRecord = m_oCurrencys.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Numner in Collection
            Return m_oCurrencys.Count()

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
    Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        ' RDC 13062002 CompServ replaced with BAS module
        'Dim oComponentServices As PMServerBusinessCS

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
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

            '    Set oComponentServices = New PMServerBusinessCS


            If Informations.IsNothing(vDatabase) Then
                '        lReturn = oComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, _
                'r_bNewInstanceCreated:=m_bCloseDatabase, _
                'r_oCheckedDatabase:=m_oDatabase)
                lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            Else
                '        lReturn = oComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, _
                'r_bNewInstanceCreated:=m_bCloseDatabase, _
                'r_oCheckedDatabase:=m_oDatabase, _
                'v_vDatabase:=vDatabase)

                lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            End If

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '        Set oComponentServices = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Don't pass a database in, otherwise we'd corrupt the existing one...
            '    lReturn = oComponentServices.CreateBusinessObject(r_oObject:=m_oCaption, _
            'v_sClassName:="bPMCaption.Business", _
            'v_sCallingAppName:=ACApp, _
            'v_sUserName:=m_sUsername, _
            'v_sPassword:=m_sPassword, _
            'v_iUserID:=m_iUserID, _
            'v_iSourceID:=m_iSourceID, _
            'v_iLanguageId:=m_iLanguageID, _
            'v_iCurrencyID:=m_iCurrencyID, _
            'v_iLogLevel:=m_iLogLevel)

            m_oCaption = New bPMCaption.Business
            lReturn = m_oCaption.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '        Set oComponentServices = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    Set oComponentServices = Nothing

            ' Set Current Record to zero
            m_lCurrentRecord = gPMConstants.PMEReturnCode.PMFalse

            ' Set the ProcessMode to Generic
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric

            ' Set the Type Of Business to New Business
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric

            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now

            ' Default RiskID to 0 (So we will work at Ins File Level by default)
            m_lRiskID = 0

            ' Create Currencys Collection
            m_oCurrencys = New bPMCurrency.Currencys()
            'developer guide no.9
            lReturn = CType(m_oCurrencys.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

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
            Me.disposedValue = True
            If disposing Then
                If m_oCaption IsNot Nothing Then
                    m_oCaption.Dispose()
                    m_oCaption = Nothing
                End If
                m_oCurrencys = Nothing
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
    ' Description: Adds a single Currency directly into the database.
    '        Note: The Currency will NOT be added to the collection.
    '
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByRef vCurrencyID As Integer = 0, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vIsoCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vMinorPart As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vSymbol As Object = Nothing, Optional ByRef vAlignment As Object = Nothing, Optional ByRef vDecimalPlaces As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vFormatString As Object = Nothing, Optional ByRef vRoundToPlaces As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oCurrency As bPMCurrency.Currency

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Currency
            oCurrency = New bPMCurrency.Currency()
            'developer guide no.9
            m_lReturn = CType(oCurrency.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName), gPMConstants.PMEReturnCode)

            ' Populate Currency Attributes
            'developer guide no.98
            m_lReturn = CType(SetProperties(oCurrency, gPMConstants.PMEComponentAction.PMAdd, vCurrencyID:=vCurrencyID, vCaptionID:=vCaptionID, vIsoCode:=vIsoCode, vDescription:=vDescription, vMinorPart:=vMinorPart, vCode:=vCode, vSymbol:=vSymbol, vAlignment:=vAlignment, vDecimalPlaces:=vDecimalPlaces, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vFormatString:=vFormatString, vRoundToPlaces:=vRoundToPlaces), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Currency to the Database
            m_lReturn = CType(AddItem(oCurrency), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}

            ' Return the ID of the Currency Added

            If Not Informations.IsNothing(vCurrencyID) Then
                vCurrencyID = oCurrency.CurrencyID
            End If

            ' {* USER DEFINED CODE (End) *}

            oCurrency = Nothing

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
    ' Description: Deletes a single Currency directly from the database.
    '        Note: The Currency will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vIsoCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vMinorPart As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vSymbol As Object = Nothing, Optional ByRef vAlignment As Object = Nothing, Optional ByRef vDecimalPlaces As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vFormatString As Object = Nothing, Optional ByRef vRoundToPlaces As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oCurrency As bPMCurrency.Currency

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Currency
            oCurrency = New bPMCurrency.Currency()
            'developer guide no.9
            m_lReturn = CType(oCurrency.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName), gPMConstants.PMEReturnCode)

            ' Populate Currency Attributes

            'developer guide no.98
            m_lReturn = CType(SetProperties(oCurrency, gPMConstants.PMEComponentAction.PMDelete, vCurrencyID:=vCurrencyID, vCaptionID:=vCaptionID, vIsoCode:=vIsoCode, vDescription:=vDescription, vMinorPart:=vMinorPart, vCode:=vCode, vSymbol:=vSymbol, vAlignment:=vAlignment, vDecimalPlaces:=vDecimalPlaces, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vFormatString:=vFormatString, vRoundToPlaces:=vRoundToPlaces), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Currency to the Database
            m_lReturn = CType(DeleteItem(oCurrency), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oCurrency = Nothing

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
    ' Description: Returns the Default Values for the Currency.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vIsoCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vMinorPart As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vSymbol As Object = Nothing, Optional ByRef vAlignment As Object = Nothing, Optional ByRef vDecimalPlaces As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vFormatString As Object = Nothing, Optional ByRef vRoundToPlaces As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults

            'developer guide no.98
            m_lReturn = CType(DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vCurrencyID:=vCurrencyID, vCaptionID:=vCaptionID, vIsoCode:=vIsoCode, vDescription:=vDescription, vMinorPart:=vMinorPart, vCode:=vCode, vSymbol:=vSymbol, vAlignment:=vAlignment, vDecimalPlaces:=vDecimalPlaces, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vFormatString:=vFormatString, vRoundToPlaces:=vRoundToPlaces), gPMConstants.PMEReturnCode)

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
    Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, Optional ByRef vTable As Object = Nothing) As Integer

        Dim result As Integer = 0
        'developer guide no. 112
        Dim oFields As DataRow

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
            '    If (Trim$(vTable) <> PMTableCurrency) Then

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
            'developer guide no.111
            oFields = m_oDatabase.Records.Item(0).Fields()

            With oFields

                ' For Each Field requested
                For lSub As Integer = vFieldArray.GetLowerBound(0) To vFieldArray.GetUpperBound(0)

                    ' Store the results in the Temporary results array

                    vResults(lSub) = oFields(vFieldArray(lSub))

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
    ' Description: Gets the required Currencys and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vLockMode As gPMConstants.PMELockMode = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oCurrency As bPMCurrency.Currency

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oCurrencys.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = gPMConstants.PMEReturnCode.PMFalse

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Informations.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vLockMode = gPMConstants.PMELockMode.PMNoLock
            End If

            ' Do we have a key

            If Not Informations.IsNothing(vCurrencyID) Then

                ' Yes, Is the key valid

                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(CStr(vCurrencyID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vCurrencyID =" & CStr(vCurrencyID), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                    Return result

                End If

                ' Add the CurrencyID parameter (INPUT)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Currency_id", vValue:=CStr(vCurrencyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

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

                    ' Create New Currency
                    oCurrency = New bPMCurrency.Currency()
                    'developer guide no.9
                    m_lReturn = CType(oCurrency.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName), gPMConstants.PMEReturnCode)
                    'developer guide no.111
                    m_lReturn = CType(SetPropertiesFromDB(oCurrency:=oCurrency, lRecordNumber:=lSub - 1), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    ' Add Currency to collection
                    m_lReturn = CType(m_oCurrencys.Add(oNewCurrency:=oCurrency), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oCurrency = Nothing

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
    ' Description: Gets the required Currencys and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vIsoCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vIsBase As Object = Nothing, Optional ByRef vMinorPart As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vSymbol As Object = Nothing, Optional ByRef vAlignment As Object = Nothing, Optional ByRef vDecimalPlaces As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vFormatString As Object = Nothing, Optional ByRef vRoundToPlaces As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oCurrency As bPMCurrency.Currency
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oCurrencys.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord = CType(m_lCurrentRecord + 1, gPMConstants.PMEReturnCode)
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oCurrency = m_oCurrencys.Item(m_lCurrentRecord - 1)

            ' Get the Currency Property Values

            'developer guide no.98
            m_lReturn = CType(GetProperties(oCurrency, iStatus, vCurrencyID:=vCurrencyID, vCaptionID:=vCaptionID, vIsoCode:=vIsoCode, vDescription:=vDescription, vIsBase:=vIsBase, vMinorPart:=vMinorPart, vCode:=vCode, vSymbol:=vSymbol, vAlignment:=vAlignment, vDecimalPlaces:=vDecimalPlaces, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vFormatString:=vFormatString, vRoundToPlaces:=vRoundToPlaces), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oCurrency = Nothing


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
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied Currency into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vIsoCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vIsBase As Object = Nothing, Optional ByRef vMinorPart As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vSymbol As Object = Nothing, Optional ByRef vAlignment As Object = Nothing, Optional ByRef vDecimalPlaces As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vFormatString As Object = Nothing, Optional ByRef vRoundToPlaces As Object = Nothing, Optional vUniqueId As String = "", Optional vScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim oCurrency As bPMCurrency.Currency

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            'If m_oCurrencys.Count() <> (lRow) Then
            '    Return gPMConstants.PMEReturnCode.PMFalse
            'End If

            ' Create a new Currency
            oCurrency = New bPMCurrency.Currency()
            'developer guide no.9
            m_lReturn = CType(oCurrency.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName), gPMConstants.PMEReturnCode)

            ' Populate Currency Attributes

            'developer guide no.98
            m_lReturn = CType(SetProperties(oCurrency, gPMConstants.PMEComponentAction.PMAdd, vCurrencyID:=vCurrencyID, vCaptionID:=vCaptionID, vIsoCode:=vIsoCode, vDescription:=vDescription, vIsBase:=vIsBase, vMinorPart:=vMinorPart, vCode:=vCode, vSymbol:=vSymbol, vAlignment:=vAlignment, vDecimalPlaces:=vDecimalPlaces, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vFormatString:=vFormatString, vRoundToPlaces:=vRoundToPlaces, vUniqueId:=vUniqueId, vScreenHierarchy:=vScreenHierarchy), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oCurrency = Nothing
                Return result
            End If

            ' Add Currency to collection
            If m_oCurrencys.Count = 0 Then
                m_oCurrencys.Add(Nothing)
            End If
            m_lReturn = CType(m_oCurrencys.Add(oNewCurrency:=oCurrency), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oCurrency = Nothing
                Return result
            End If

            oCurrency = Nothing

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
    ' Description: Validates that this action is valid on the Currency
    '              specified and updates the Currency with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vIsoCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vIsBase As Object = Nothing, Optional ByRef vMinorPart As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vSymbol As Object = Nothing, Optional ByRef vAlignment As Object = Nothing, Optional ByRef vDecimalPlaces As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vFormatString As Object = Nothing, Optional ByRef vRoundToPlaces As Object = Nothing, Optional vUniqueId As String = "", Optional vScreenHierarchy As String = "") As Integer


        Dim result As Integer = 0
        Dim oCurrency As bPMCurrency.Currency
        Dim iStatus As gPMConstants.PMEComponentAction

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oCurrencys.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oCurrency = m_oCurrencys.Item(lRow - 1)

            ' Check the Status of the Currency

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oCurrency.DatabaseStatus
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

            ' Update Currency Attributes
            'developer guide no.98
            m_lReturn = CType(SetProperties(oCurrency, iStatus, vCurrencyID:=vCurrencyID, vCaptionID:=vCaptionID, vIsoCode:=vIsoCode, vDescription:=vDescription, vIsBase:=vIsBase, vMinorPart:=vMinorPart, vCode:=vCode, vSymbol:=vSymbol, vAlignment:=vAlignment, vDecimalPlaces:=vDecimalPlaces, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vFormatString:=vFormatString, vRoundToPlaces:=vRoundToPlaces, vUniqueId:=vUniqueId, vScreenHierarchy:=vScreenHierarchy), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oCurrency = Nothing
                Return result
            End If

            ' Release reference to Currency
            oCurrency = Nothing

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
    ' Description: Validate that the specified Currency can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer, Optional vUniqueId As String = "", Optional vScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim oCurrency As bPMCurrency.Currency

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oCurrencys.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oCurrency = m_oCurrencys.Item(lRow - 1)

            ' Check the Status of the Currency

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oCurrency.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oCurrency.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oCurrency.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            oCurrency.UniqueId = vUniqueId
            oCurrency.ScreenHierarchy = vScreenHierarchy


            ' Release reference to Currency
            oCurrency = Nothing

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
            For lSub As Integer = 0 To m_oCurrencys.Count() - 1
                Select Case m_oCurrencys.Item(lSub).DatabaseStatus
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
        Dim oCurrency As bPMCurrency.Currency
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 0 To m_oCurrencys.Count() - 1
                oCurrency = m_oCurrencys.Item(lSub)


                Select Case oCurrency.DatabaseStatus
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
                        m_lReturn = CType(AddItem(oCurrency), gPMConstants.PMEReturnCode)
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
                        m_lReturn = CType(UpdateItem(oCurrency), gPMConstants.PMEReturnCode)
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
                        m_lReturn = CType(DeleteItem(oCurrency), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Release last reference
            oCurrency = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Refresh the Collection Items D/B Status
                    lSub = 0

                    ' For each item in the collection
                    Do While lSub <= m_oCurrencys.Count() - 1

                        ' With the item
                        With m_oCurrencys.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oCurrencys.Delete(lSub)

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

    ' ***************************************************************** '
    ' Name: GetCurrencyIdFromISO (Public)
    '
    ' Description: returns currency id from supplied iso code
    '
    ' ***************************************************************** '

    Public Function GetCurrencyIdFromISO(ByVal v_sISOCode As String, ByRef r_iCurrencyId As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="iso_code", vValue:=v_sISOCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                m_lReturn = .SQLSelect(sSQL:=ACGetDetailsByCodeSQL, sSQLName:=ACGetDetailsByCodeName, bStoredProcedure:=ACGetDetailsByCodeStored, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If .Records.Count() <> 1 Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'developer guide no.111
                r_iCurrencyId = .Records.Item(0).Fields()("currency_id")
            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFail

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCurrencyIdFromISO Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrencyIdFromISO", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Private Function AddItem(ByRef oCurrency As bPMCurrency.Currency) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer
        Dim sCode As String = ""
        Dim iId As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        'First things first.  This may be a deleted record, in which case we need to get the key
        'and update it.

        sCode = oCurrency.IsoCode

        m_lReturn = CType(GetCurrencyIdFromISO(v_sISOCode:=sCode, r_iCurrencyId:=iId), gPMConstants.PMEReturnCode)

        'It's there
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            oCurrency.CurrencyID = iId
            oCurrency.IsDeleted = 0
            'the effective date is already set...


            Return UpdateItem(oCurrency:=oCurrency)
        End If

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add CurrencyID as an OUTPUT param for an insert
        m_lReturn = m_oDatabase.Parameters.Add(sName:="currency_id", vValue:=CStr(oCurrency.CurrencyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the required INPUT parameters
        m_lReturn = CType(AddInputParam(oCurrency:=oCurrency), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the ID of the record inserted
        oCurrency.CurrencyID = m_oDatabase.Parameters.Item("Currency_id").Value

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
    Private Function UpdateItem(ByRef oCurrency As bPMCurrency.Currency) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add CurrencyID as an INPUT param for an update
        m_lReturn = m_oDatabase.Parameters.Add(sName:="currency_id", vValue:=CStr(oCurrency.CurrencyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the Input Parameters to the Parameteres collection
        m_lReturn = CType(AddInputParam(oCurrency:=oCurrency), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

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
    Private Function DeleteItem(ByRef oCurrency As bPMCurrency.Currency) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        oCurrency.IsDeleted = 1


        Return UpdateItem(oCurrency:=oCurrency)

    End Function

    ' ***************************************************************** '
    ' Name: SetPropertiesFromDB (Private)
    '
    ' Description: Sets the supplied Currency properties from a database
    '              record.
    ' ***************************************************************** '
    Private Function SetPropertiesFromDB(ByRef oCurrency As bPMCurrency.Currency, ByRef lRecordNumber As Integer) As Integer

        Dim result As Integer = 0
        Dim sDescription As String = ""
        'developer guide no.112
        Dim oFields As DataRow



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set oFields to refer to Field Collection for Record 1
        oFields = m_oDatabase.Records.Item(lRecordNumber).Fields()

        ' Populate Base Details

        With oCurrency

            .CurrencyID = oFields("currency_id")

            If Convert.IsDBNull(oFields("caption_id")) Or Informations.IsNothing(oFields("caption_id")) Then
                .CaptionID = 0
            Else
                .CaptionID = oFields("caption_id")
            End If

            If Convert.IsDBNull(oFields("iso_code")) Or Informations.IsNothing(oFields("iso_code")) Then
                .IsoCode = ""
            Else
                .IsoCode = oFields("iso_code")
            End If

            m_lReturn = m_oCaption.GetCaptionDesc(v_lCaptionId:= .CaptionID, r_sCaption:=sDescription)
            .Description = sDescription
            '        If (IsNull(oFields.Item("description").Value) = True) Then
            '            .Description = ""
            '        Else
            '            .Description = oFields.Item("description").Value
            '        End If

            If Convert.IsDBNull(oFields("minor_part")) Or Informations.IsNothing(oFields("minor_part")) Then
                .MinorPart = CStr(0)
            Else
                .MinorPart = oFields("minor_part")
            End If

            If Convert.IsDBNull(oFields("code")) Or Informations.IsNothing(oFields("code")) Then
                .Code = ""
            Else
                .Code = oFields("code")
            End If

            If Convert.IsDBNull(oFields("symbol")) Or Informations.IsNothing(oFields("symbol")) Then
                .Symbol = ""
            Else
                .Symbol = oFields("symbol")
            End If

            If Convert.IsDBNull(oFields("alignment")) Or Informations.IsNothing(oFields("alignment")) Then
                .Alignment = ""
            Else
                .Alignment = oFields("alignment")
            End If

            If Convert.IsDBNull(oFields("decimal_places")) Or Informations.IsNothing(oFields("decimal_places")) Then
                .DecimalPlaces = 0
            Else
                .DecimalPlaces = oFields("decimal_places")
            End If

            If Convert.IsDBNull(oFields("is_deleted")) Or Informations.IsNothing(oFields("is_deleted")) Then
                .IsDeleted = 0
            Else
                .IsDeleted = oFields("is_deleted")
            End If

            If Convert.IsDBNull(oFields("effective_date")) Or Informations.IsNothing(oFields("effective_date")) Then
                .EffectiveDate = #12/30/1899#
            Else
                .EffectiveDate = oFields("effective_date")
            End If

            If Convert.IsDBNull(oFields("format_string")) Or Informations.IsNothing(oFields("format_string")) Then
                .FormatString = ""
            Else
                .FormatString = oFields("format_string")
            End If

            If Convert.IsDBNull(oFields("round_to_places")) Or Informations.IsNothing(oFields("round_to_places")) Then
                .RoundToPlaces = 0
            Else
                .RoundToPlaces = oFields("round_to_places")
            End If

            If Convert.IsDBNull(oFields("is_base")) Or Informations.IsNothing(oFields("is_base")) Then
                .IsBase = False
            Else
                .IsBase = oFields("is_base")
            End If

            .DatabaseStatus = gPMConstants.PMEComponentAction.PMView

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Private)
    '
    ' Description: Sets the supplied Currency property values.
    '
    ' ***************************************************************** '
    Private Function SetProperties(ByRef oCurrency As bPMCurrency.Currency, ByRef iStatus As Integer, Optional ByRef vCurrencyID As Integer = 0, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vIsoCode As String = "", Optional ByRef vDescription As String = "", Optional ByRef vIsBase As Boolean = False, Optional ByRef vMinorPart As String = "", Optional ByRef vCode As String = "", Optional ByRef vSymbol As String = "", Optional ByRef vAlignment As String = "", Optional ByRef vDecimalPlaces As Integer = 0, Optional ByRef vIsDeleted As Integer = 0, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vFormatString As String = "", Optional ByRef vRoundToPlaces As Integer = 0, Optional vUniqueId As String = "", Optional vScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim lCaptionID As Integer
        Dim sDescription As String = ""
        Dim bDataChanged As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        ' If Add Mode
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            ' Check that All Mandatory Parameters have been supplied
            m_lReturn = CType(CheckMandatory(vCurrencyID:=vCurrencyID, vCaptionID:=vCaptionID, vIsoCode:=vIsoCode, vDescription:=vDescription, vIsBase:=vIsBase, vMinorPart:=vMinorPart, vCode:=vCode, vSymbol:=vSymbol, vAlignment:=vAlignment, vDecimalPlaces:=vDecimalPlaces, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vFormatString:=vFormatString, vRoundToPlaces:=vRoundToPlaces), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Default Any Missing Parameters


            m_lReturn = CType(DefaultParameters(bDefaultAll:=False, vCurrencyID:=vCurrencyID, vCaptionID:=vCaptionID, vIsoCode:=vIsoCode, vDescription:=vDescription, vIsBase:=vIsBase, vMinorPart:=vMinorPart, vCode:=vCode, vSymbol:=vSymbol, vAlignment:=vAlignment, vDecimalPlaces:=vDecimalPlaces, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vFormatString:=vFormatString, vRoundToPlaces:=vRoundToPlaces), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

        End If

        ' Validate Parameters
        m_lReturn = CType(Validate(vCurrencyID:=vCurrencyID, vCaptionID:=vCaptionID, vIsoCode:=vIsoCode, vDescription:=vDescription, vIsBase:=vIsBase, vMinorPart:=vMinorPart, vCode:=vCode, vSymbol:=vSymbol, vAlignment:=vAlignment, vDecimalPlaces:=vDecimalPlaces, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vFormatString:=vFormatString, vRoundToPlaces:=vRoundToPlaces), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        'Quick, let's go get the caption id

        If Not Informations.IsNothing(vDescription) Then
            sDescription = vDescription


            m_lReturn = m_oCaption.GetCaptionID(v_sCaption:=sDescription, r_lCaptionId:=lCaptionID)
        End If

        ' Set Data Changed Flag to False
        bDataChanged = False


        ' Set Property values.
        With oCurrency


            If Not Informations.IsNothing(vCurrencyID) Then
                If .CurrencyID <> vCurrencyID Then
                    .CurrencyID = vCurrencyID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vIsoCode) Then
                If .IsoCode.Trim() <> vIsoCode.Trim() Then
                    .IsoCode = vIsoCode
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vDescription) Then
                If .Description.Trim() <> vDescription.Trim() Then
                    .Description = vDescription
                    .CaptionID = lCaptionID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vIsBase) Then
                If .IsBase <> vIsBase Then
                    .IsBase = vIsBase
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vMinorPart) Then
                If .MinorPart <> vMinorPart Then
                    .MinorPart = vMinorPart
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vCode) Then
                If .Code.Trim() <> vCode.Trim() Then
                    .Code = vCode
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vSymbol) Then
                If .Symbol.Trim() <> vSymbol.Trim() Then
                    .Symbol = vSymbol
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vAlignment) Then
                If .Alignment.Trim() <> vAlignment.Trim() Then
                    .Alignment = vAlignment
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vDecimalPlaces) Then
                If .DecimalPlaces <> vDecimalPlaces Then
                    .DecimalPlaces = vDecimalPlaces
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vIsDeleted) Then
                If .IsDeleted <> vIsDeleted Then
                    .IsDeleted = vIsDeleted
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vEffectiveDate) Then

                If .EffectiveDate <> CDate(vEffectiveDate) Then

                    .EffectiveDate = CDate(vEffectiveDate)
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vFormatString) Then
                If .FormatString.Trim() <> vFormatString.Trim() Then
                    .FormatString = vFormatString
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vRoundToPlaces) Then
                If .RoundToPlaces <> vRoundToPlaces Then
                    .RoundToPlaces = vRoundToPlaces
                    bDataChanged = True
                End If
            End If

            If Not String.IsNullOrEmpty(vUniqueId) Then
                .UniqueId = vUniqueId
                .ScreenHierarchy = vScreenHierarchy
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
    ' Description: Returns the supplied Currency property values.
    '
    ' ***************************************************************** '
    'developer guide no.101
    Private Function GetProperties(ByRef oCurrency As bPMCurrency.Currency, ByRef iStatus As Integer, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vIsoCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vIsBase As Object = Nothing, Optional ByRef vMinorPart As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vSymbol As Object = Nothing, Optional ByRef vAlignment As Object = Nothing, Optional ByRef vDecimalPlaces As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vFormatString As Object = Nothing, Optional ByRef vRoundToPlaces As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set Property values.
        With oCurrency
            'developer guide no.118
            'start

            vCurrencyID = .CurrencyID


            vCaptionID = .CaptionID


            vIsoCode = .IsoCode


            vDescription = .Description


            vMinorPart = .MinorPart


            vCode = .Code


            vSymbol = .Symbol


            vAlignment = .Alignment


            vDecimalPlaces = .DecimalPlaces


            vIsDeleted = .IsDeleted


            vEffectiveDate = .EffectiveDate


            vFormatString = .FormatString


            vRoundToPlaces = .RoundToPlaces


            vIsBase = .IsBase

            'end
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
    Private Function AddInputParam(ByRef oCurrency As bPMCurrency.Currency) As Integer

        Dim result As Integer = 0
        Dim sCaption As String = ""
        Dim lCaptionID As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            sCaption = oCurrency.Description


            m_lReturn = m_oCaption.GetCaptionID(v_sCaption:=sCaption, r_lCaptionId:=lCaptionID)

            m_lReturn = .Parameters.Add(sName:="caption_id", vValue:=CStr(lCaptionID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="iso_code", vValue:=oCurrency.IsoCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="description", vValue:=oCurrency.Description, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="minor_part", vValue:=oCurrency.MinorPart, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="code", vValue:=oCurrency.Code, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="symbol", vValue:=oCurrency.Symbol, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="alignment", vValue:=oCurrency.Alignment, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="decimal_places", vValue:=CStr(oCurrency.DecimalPlaces), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="is_deleted", vValue:=CStr(oCurrency.IsDeleted), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="effective_date", vValue:=oCurrency.EffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="format_string", vValue:=oCurrency.FormatString, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="round_to_places", vValue:=CStr(oCurrency.RoundToPlaces), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="is_base", vValue:=CStr(oCurrency.IsBase), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="unique_id", vValue:=oCurrency.UniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="screen_hierarchy", vValue:=oCurrency.ScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DefaultParameters (Public)
    '
    ' Description: Sets the Default Values for a Currency.
    '
    ' ***************************************************************** '
    'developer guide no.101
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vIsoCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vIsBase As Object = Nothing, Optional ByRef vMinorPart As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vSymbol As Object = Nothing, Optional ByRef vAlignment As Object = Nothing, Optional ByRef vDecimalPlaces As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vFormatString As Object = Nothing, Optional ByRef vRoundToPlaces As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        'developer guide no.44
        'start
        If (Informations.IsNothing(vCurrencyID)) OrElse (vCurrencyID.Equals(0)) Or (bDefaultAll) Then
            vCurrencyID = 0
        End If



        If (Informations.IsNothing(vCaptionID)) OrElse (vCaptionID.Equals(0)) Or (bDefaultAll) Then
            vCaptionID = 0
        End If



        If (Informations.IsNothing(vIsoCode)) OrElse (String.IsNullOrEmpty(vIsoCode)) Or (bDefaultAll) Then
            vIsoCode = ""
        End If



        If (Informations.IsNothing(vDescription)) OrElse (String.IsNullOrEmpty(vDescription)) Or (bDefaultAll) Then
            vDescription = ""
        End If



        If (Informations.IsNothing(vIsBase)) OrElse (vIsBase.Equals(False)) Or (bDefaultAll) Then
            vIsBase = False
        End If



        If (Informations.IsNothing(vMinorPart)) OrElse (vMinorPart.Equals(0)) Or (bDefaultAll) Then
            vMinorPart = 0
        End If



        If (Informations.IsNothing(vCode)) OrElse (String.IsNullOrEmpty(vCode)) Or (bDefaultAll) Then
            vCode = ""
        End If



        If (Informations.IsNothing(vSymbol)) OrElse (String.IsNullOrEmpty(vSymbol)) Or (bDefaultAll) Then
            vSymbol = ""
        End If



        If (Informations.IsNothing(vAlignment)) OrElse (String.IsNullOrEmpty(vAlignment)) Or (bDefaultAll) Then
            vAlignment = ""
        End If



        If (Informations.IsNothing(vDecimalPlaces)) OrElse (vDecimalPlaces.Equals(0)) Or (bDefaultAll) Then
            vDecimalPlaces = 0
        End If



        If (Informations.IsNothing(vIsDeleted)) OrElse (vIsDeleted.Equals(0)) Or (bDefaultAll) Then
            vIsDeleted = 0
        End If



        If (Informations.IsNothing(vEffectiveDate)) OrElse (vEffectiveDate.Equals(DateTime.FromOADate(0))) Or (bDefaultAll) Then
            vEffectiveDate = DateTime.Now
        End If



        If (Informations.IsNothing(vFormatString)) OrElse (String.IsNullOrEmpty(vFormatString)) Or (bDefaultAll) Then
            vFormatString = ""
        End If



        If (Informations.IsNothing(vRoundToPlaces)) OrElse (vRoundToPlaces.Equals(0)) Or (bDefaultAll) Then
            vRoundToPlaces = 0
        End If
        'end

        ' {* USER DEFINED CODE (End) *}


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Public)
    '
    ' Description: Sets the Default Values for a Currency.
    '
    ' ***************************************************************** '
    Private Function CheckMandatory(Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vIsoCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vIsBase As Object = Nothing, Optional ByRef vMinorPart As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vSymbol As Object = Nothing, Optional ByRef vAlignment As Object = Nothing, Optional ByRef vDecimalPlaces As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vFormatString As Object = Nothing, Optional ByRef vRoundToPlaces As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vCurrencyID)) Or (Object.Equals(vCurrencyID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vCaptionID)) Or (Object.Equals(vCaptionID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vIsoCode)) Or (Object.Equals(vIsoCode, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vDescription)) Or (Object.Equals(vDescription, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vIsBase)) Or (Object.Equals(vIsBase, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vMinorPart)) Or (Object.Equals(vMinorPart, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vCode)) Or (Object.Equals(vCode, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vSymbol)) Or (Object.Equals(vSymbol, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vAlignment)) Or (Object.Equals(vAlignment, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vDecimalPlaces)) Or (Object.Equals(vDecimalPlaces, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vIsDeleted)) Or (Object.Equals(vIsDeleted, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vEffectiveDate)) Or (Object.Equals(vEffectiveDate, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vFormatString)) Or (Object.Equals(vFormatString, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vRoundToPlaces)) Or (Object.Equals(vRoundToPlaces, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If


        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetPrivilegeLevel
    '
    ' Description:
    '
    ' History: 10/11/1999 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function GetPrivilegeLevel(ByRef r_iPrivilegeLevel As Integer) As Integer
        Dim result As Integer = 0
        Dim lPMProductID As Integer
        ' RDC 13062002 CompServ replaced with BAS module
        'Dim oCS As sPMServerCS.PMServerBusinessCS
        Dim oPMProductLookup As bPMProductLookup.Business


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(GetProductID(lPMProductID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create the User Group Component to do this work.
            '    Set oCS = New sPMServerCS.PMServerBusinessCS
            '    m_lReturn = oCS.CreateBusinessObject( _
            'r_oObject:=oPMProductLookup, _
            'v_sClassName:="bPMProductLookup.Business", _
            'v_sCallingAppName:=m_sCallingAppName, _
            'v_sUserName:=m_sUsername, _
            'v_sPassword:=m_sPassword, _
            'v_iUserID:=m_iUserID, _
            'v_iSourceID:=m_iSourceID, _
            'v_iLanguageId:=m_iLanguageID, _
            'v_iCurrencyID:=m_iCurrencyID, _
            'v_iLogLevel:=m_iLogLevel, _
            'v_oDatabase:=m_oDatabase)

            oPMProductLookup = New bPMProductLookup.Business
            m_lReturn = oPMProductLookup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            '    Set oCS = Nothing
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DAK221299

            m_lReturn = oPMProductLookup.GetDetails(vPMProductId:=lPMProductID, vTableName:="Currency")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oPMProductLookup.Dispose()
                oPMProductLookup = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oPMProductLookup.GetNext(vPrivilegeLevel:=r_iPrivilegeLevel)

            oPMProductLookup.Dispose()
            oPMProductLookup = Nothing
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPrivilegeLevel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPrivilegeLevel", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetProductID
    '
    ' Description:
    '
    ' History: 11/11/1999 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function GetProductID(ByRef r_lPMProductID As Integer) As Integer
        ' RDC 13062002 CompServ replaced with BAS module
        'Dim oCS As sPMServerCS.PMServerBusinessCS
        Dim result As Integer = 0
        Dim oPMProduct As bPMProduct.Business


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create the User Group Component to do this work.
            '    Set oCS = New sPMServerCS.PMServerBusinessCS
            '    m_lReturn = oCS.CreateBusinessObject( _
            'r_oObject:=oPMProduct, _
            'v_sClassName:="bPMProduct.Business", _
            'v_sCallingAppName:=m_sCallingAppName, _
            'v_sUserName:=m_sUsername, _
            'v_sPassword:=m_sPassword, _
            'v_iUserID:=m_iUserID, _
            'v_iSourceID:=m_iSourceID, _
            'v_iLanguageId:=m_iLanguageID, _
            'v_iCurrencyID:=m_iCurrencyID, _
            'v_iLogLevel:=m_iLogLevel, _
            'v_oDatabase:=m_oDatabase)

            oPMProduct = New bPMProduct.Business
            m_lReturn = oPMProduct.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            '    Set oCS = Nothing
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oPMProduct.GetProductID(v_sProductCode:="Sirius", r_lPMProductID:=r_lPMProductID)

            oPMProduct.Dispose()
            oPMProduct = Nothing
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProductID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProductID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetUserAuthority
    '
    ' Description: Gets the User Groups that the current user is a
    '              Supervisor of.
    '
    ' ***************************************************************** '
    Public Function GetUserAuthority(ByRef r_bIsAdministrator As Boolean, ByRef r_vSupervisedGroups As Object) As Integer

        Dim result As Integer = 0
        Dim oUserGroup As bPMUserGroup.Utilities
        ' RDC 13062002 CompServ repalced with BAS module
        'Dim oCS As sPMServerCS.PMServerBusinessCS

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create the User Group Component to do this work.
            '    Set oCS = New sPMServerCS.PMServerBusinessCS
            '    m_lReturn = oCS.CreateBusinessObject( _
            'r_oObject:=oUserGroup, _
            'v_sClassName:="bPMUserGroup.Utilities", _
            'v_sCallingAppName:=m_sCallingAppName, _
            'v_sUserName:=m_sUsername, _
            'v_sPassword:=m_sPassword, _
            'v_iUserID:=m_iUserID, _
            'v_iSourceID:=m_iSourceID, _
            'v_iLanguageId:=m_iLanguageID, _
            'v_iCurrencyID:=m_iCurrencyID, _
            'v_iLogLevel:=m_iLogLevel, _
            'v_oDatabase:=m_oDatabase)

            oUserGroup = New bPMUserGroup.Utilities
            m_lReturn = oUserGroup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            '    Set oCS = Nothing
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Is the User an Administrator

            m_lReturn = oUserGroup.IsUserAdministrator(v_iUserID:=m_iUserID, r_bIsAdministrator:=r_bIsAdministrator)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                oUserGroup.Dispose()
                oUserGroup = Nothing
                Return result
            End If

            ' Get the Groups they Supervise

            m_lReturn = oUserGroup.GetGroupsSupervisedByUser(v_iUserID:=m_iUserID, r_vSupervisedGroups:=r_vSupervisedGroups)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                oUserGroup.Dispose()
                oUserGroup = Nothing
                Return result
            End If

            ' Terminate

            oUserGroup.Dispose()
            oUserGroup = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserAuthorityFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserAuthority", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the Currency for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vIsoCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vIsBase As Object = Nothing, Optional ByRef vMinorPart As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vSymbol As Object = Nothing, Optional ByRef vAlignment As Object = Nothing, Optional ByRef vDecimalPlaces As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vFormatString As Object = Nothing, Optional ByRef vRoundToPlaces As Object = Nothing) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        Dim dbNumericTemp As Double
        If Not Double.TryParse(CStr(vCurrencyID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp2 As Double
        If Not Double.TryParse(CStr(vCaptionID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp3 As Double
        If Not Double.TryParse(CStr(vDecimalPlaces), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp4 As Double
        If Not Double.TryParse(CStr(vIsDeleted), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsDate(vEffectiveDate) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp5 As Double
        If Not Double.TryParse(CStr(vRoundToPlaces), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
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
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
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



    ' ***************************************************************** '
    ' Name: GetSysAdminStatus
    '
    ' Description: check if user is member of a SysAdmin user group.
    '
    ' History: RDC 17102002 created
    ' ***************************************************************** '
    Public Function GetSysAdminStatus(ByRef lStatus As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_lReturn = CType(gPMComponentServices.GetSysAdminAccessStatus(m_sUsername, m_iUserID, m_iSourceID, m_iLanguageID, lStatus, m_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSysAdminStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSysAdminStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckCodesDescriptions
    '
    ' Description: checks that codes and descriptions are populated.
    '
    ' History: RDC 11092003 created
    ' ***************************************************************** '
    Public Function CheckCodesDescriptions() As Integer

        Dim result As Integer = 0
        Dim oCurrency As bPMCurrency.Currency

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            For lLoop As Integer = 0 To m_oCurrencys.Count() - 1
                oCurrency = m_oCurrencys.Item(lLoop)
                If oCurrency.Code.Trim() = "" Or oCurrency.Description.Trim() = "" Then
                    Return gPMConstants.PMEReturnCode.PMMandatoryMissing
                End If
            Next


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckCodesDescriptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckCodesDescriptions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
End Class
