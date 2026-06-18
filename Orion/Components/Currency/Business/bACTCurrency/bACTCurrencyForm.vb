Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
Imports System.IO
Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports Microsoft.Practices.EnterpriseLibrary.Caching.Expirations
Imports SSP.Shared

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

    ' Collection of Currencys (Private)
    Private m_oCurrencys As bACTCurrency.Currencys

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
    ' Insurance File ID
    Private m_lInsuranceFileID As Integer
    ' Risk ID
    Private m_lRiskID As Integer
    Public Shared iCache As ICacheManager
    Private m_sCachePath As String


    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public Property CurrentRecord() As Integer
        Get

            Return m_lCurrentRecord

        End Get
        Set(ByVal Value As Integer)

            Select Case Value
                Case Is < 1
                    m_lCurrentRecord = 0
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
            If m_oCurrencys.Count > 0 AndAlso m_oCurrencys.Item(0) Is Nothing Then
                Return m_oCurrencys.Count - 1
            Else
                Return m_oCurrencys.Count
            End If

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

        Dim lReturn As gPMConstants.PMEReturnCode
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


            ' Get Reference to Database

            Try
                iCache = CacheFactory.GetCacheManager("PureCache")
            Catch ex As Exception
            End Try

            '-------------
            lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine,
                                                  v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture,
                                                  v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer,
                                                  v_sSettingName:=gPMConstants.PMRegKeyCachePath, r_sSettingValue:=m_sCachePath)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Right(m_sCachePath, 1) <> "\" Then
                m_sCachePath += "\"
            End If

            '--------------

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

            ' Create Currencys Collection
            m_oCurrencys = New bACTCurrency.Currencys()



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
    Public Function DirectAdd(Optional ByRef vCurrencyID As Integer = 0, Optional ByRef vCaptionID As Object = Nothing,
                              Optional ByRef vIsoCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing,
                              Optional ByRef vMinorPart As Object = Nothing, Optional ByRef vCode As Object = Nothing,
                              Optional ByRef vSymbol As Object = Nothing, Optional ByRef vAlignment As Object = Nothing,
                              Optional ByRef vDecimalPlaces As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing,
                              Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vFormatString As Object = Nothing,
                              Optional ByRef vRoundToPlaces As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oCurrency As bACTCurrency.Currency

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Currency
            oCurrency = New bACTCurrency.Currency()

            ' Populate Currency Attributes












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
        Dim oCurrency As bACTCurrency.Currency

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Currency
            oCurrency = New bACTCurrency.Currency()

            ' Populate Currency Attributes













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
            oFields = m_oDatabase.Records.Item(1).Fields()

            With oFields

                ' For Each Field requested
                For lSub As Integer = vFieldArray.GetLowerBound(0) To vFieldArray.GetUpperBound(0)

                    'PWF 230702 - scalability change - check for null value

                    If Not (Convert.IsDBNull(oFields(vFieldArray(lSub))) Or Informations.IsNothing(oFields(vFieldArray(lSub)))) Then
                        ' Store the results in the Temporary results array

                        vResults(lSub) = oFields(vFieldArray(lSub))
                    Else
                        Select Case oFields(vFieldArray(lSub)).Type
                            'developer guide no. 47(No Solutions)
                            'Case DbType.String, DbType.String, DbType.String, adLongVarWChar, DbType.String, DbType.String, adWChar
                            Case DbType.String, DbType.String, DbType.String, DbType.String, DbType.String

                                vResults(lSub) = ""
                                'developer guide no. 47(No Solutions)
                                'Case DbType.Date, adDBDate
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
    Public Function GetDetails(ByRef vCurrencyID As Object) As Integer
        Return GetDetails(vCurrencyID:=vCurrencyID, vLockMode:=0)
    End Function

    Public Function GetDetails() As Integer
        Return GetDetails(vCurrencyID:=Nothing, vLockMode:=0)
    End Function

    Public Function GetDetails(ByRef vCurrencyID As Object, ByRef vLockMode As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oCurrency As bACTCurrency.Currency = Nothing
        ''''''
        Dim sKey As String = String.Empty
        Dim sFilePath As String = ""
        Dim sCacheFilename As String = ""
        Dim sContent(1) As String
        '''''

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Informations.IsNothing(vCurrencyID) Then



                sKey = "KEY_LOOKUP_SelectCurrency_ID_" & vCurrencyID.ToString
                sCacheFilename = sKey


                If Not iCache Is Nothing AndAlso iCache.Contains(sKey) AndAlso Not String.IsNullOrEmpty(Convert.ToString(iCache.GetData(sKey))) Then
                    oCurrency = iCache.GetData(sKey)
                    ' Add Currency to collection
                    If (m_oCurrencys.Count = 0) Then
                        m_oCurrencys.Add(Nothing)
                    End If
                    m_lReturn = CType(m_oCurrencys.Add(oNewCurrency:=oCurrency), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'oCurrency = Nothing
                Else
                End If

                If oCurrency Is Nothing Then


                    ' Clear the Collection
                    m_oCurrencys.Clear()

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

                    'If Not Informations.IsNothing(vCurrencyID) Then

                    ' Yes, Is the key valid

                    Dim dbNumericTemp2 As Double
                    If Not Double.TryParse(CStr(vCurrencyID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                        result = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error Message

                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vCurrencyID =" & CStr(vCurrencyID), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                        Return result

                    End If

                    ' Add the CurrencyID parameter (INPUT)

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="Currency_id", vValue:=vCurrencyID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Execute SQL Statement
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDetailsSQL, sSQLName:=ACGetDetailsName, bStoredProcedure:=ACGetDetailsStored, lNumberRecords:=0)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
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
                            oCurrency = New bACTCurrency.Currency()
                            'developer guide no. 162(Guide)
                            m_lReturn = CType(SetPropertiesFromDB(oCurrency:=oCurrency, lRecordNumber:=lSub - 1), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If


                            ' Add Currency to collection
                            If (m_oCurrencys.Count = 0) Then
                                m_oCurrencys.Add(Nothing)
                            End If
                            m_lReturn = CType(m_oCurrencys.Add(oNewCurrency:=oCurrency), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            'oCurrency = Nothing

                        Next lSub

                    End If




                    ' Add them to the Cache
                    sFilePath = m_sCachePath + sCacheFilename + ".xml"

                    If Not FileExists(sFilePath) Then
                        Dim fileIO As FileStream
                        fileIO = File.Create(sFilePath)
                        fileIO.Close()
                        File.WriteAllLines(sFilePath, sContent)
                    End If
                    ' Add the key to the SIRIUS_CACHE_KEYS   Cache Array, to be used by
                    ' Sirius Cache Controller
                    If Not iCache Is Nothing Then
                        'If iCache Is Nothing Then
                        'iCache.Add(sKey, vResultArray, CacheItemPriority.NotRemovable, Nothing, New FileDependency(m_sCachePath + m_sCacheFilename))
                        iCache.Add(sKey, oCurrency, CacheItemPriority.NotRemovable, Nothing, New FileDependency(sFilePath))
                    End If

                    oCurrency = Nothing

                End If
                oCurrency = Nothing


            Else

                ' No Key, Get All Records
                m_oDatabase.Parameters.Clear()
                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'End If

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
                        oCurrency = New bACTCurrency.Currency()
                        'developer guide no. 162(Guide)
                        m_lReturn = CType(SetPropertiesFromDB(oCurrency:=oCurrency, lRecordNumber:=lSub - 1), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If


                        ' Add Currency to collection
                        If (m_oCurrencys.Count = 0) Then
                            m_oCurrencys.Add(Nothing)
                        End If
                        m_lReturn = CType(m_oCurrencys.Add(oNewCurrency:=oCurrency), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        oCurrency = Nothing

                    Next lSub

                End If

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
    Public Function GetNext(Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vIsoCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vMinorPart As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vSymbol As Object = Nothing, Optional ByRef vAlignment As Object = Nothing, Optional ByRef vDecimalPlaces As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vFormatString As Object = Nothing, Optional ByRef vRoundToPlaces As Object = Nothing) As Integer


        Dim result As Integer = 0

        Dim oCurrency As bACTCurrency.Currency
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oCurrencys.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oCurrency = m_oCurrencys.Item(m_lCurrentRecord)

            ' Get the Currency Property Values













            'developer guide no. 98(Guide)
            m_lReturn = CType(GetProperties(oCurrency, iStatus,
                        vCurrencyID:=vCurrencyID, vCaptionID:=vCaptionID,
                        vIsoCode:=vIsoCode, vDescription:=vDescription,
                        vMinorPart:=vMinorPart, vCode:=vCode,
                 vSymbol:=vSymbol, vAlignment:=vAlignment,
                 vDecimalPlaces:=vDecimalPlaces, vIsDeleted:=vIsDeleted,
                 vEffectiveDate:=vEffectiveDate, vFormatString:=vFormatString,
                 vRoundToPlaces:=vRoundToPlaces), gPMConstants.PMEReturnCode)

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
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vIsoCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vMinorPart As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vSymbol As Object = Nothing, Optional ByRef vAlignment As Object = Nothing, Optional ByRef vDecimalPlaces As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vFormatString As Object = Nothing, Optional ByRef vRoundToPlaces As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oCurrency As bACTCurrency.Currency

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oCurrencys.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new Currency
            oCurrency = New bACTCurrency.Currency()

            ' Populate Currency Attributes













            m_lReturn = CType(SetProperties(oCurrency, gPMConstants.PMEComponentAction.PMAdd, vCurrencyID:=vCurrencyID, vCaptionID:=vCaptionID, vIsoCode:=vIsoCode, vDescription:=vDescription, vMinorPart:=vMinorPart, vCode:=vCode, vSymbol:=vSymbol, vAlignment:=vAlignment, vDecimalPlaces:=vDecimalPlaces, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vFormatString:=vFormatString, vRoundToPlaces:=vRoundToPlaces), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oCurrency = Nothing
                Return result
            End If

            ' Add Currency to collection
            If (m_oCurrencys.Count = 0) Then
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
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vCurrencyID As Object = Nothing,
                               Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vIsoCode As Object = Nothing,
                               Optional ByRef vDescription As Object = Nothing,
                               Optional ByRef vMinorPart As Object = Nothing,
                               Optional ByRef vCode As Object = Nothing, Optional ByRef vSymbol As Object = Nothing,
                               Optional ByRef vAlignment As Object = Nothing,
                               Optional ByRef vDecimalPlaces As Object = Nothing,
                               Optional ByRef vIsDeleted As Object = Nothing,
                               Optional ByRef vEffectiveDate As Object = Nothing,
                               Optional ByRef vFormatString As Object = Nothing,
                               Optional ByRef vRoundToPlaces As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oCurrency As bACTCurrency.Currency
        Dim iStatus As gPMConstants.PMEComponentAction

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oCurrencys.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oCurrency = m_oCurrencys.Item(lRow)

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













            m_lReturn = CType(SetProperties(oCurrency, iStatus, vCurrencyID:=vCurrencyID, vCaptionID:=vCaptionID, vIsoCode:=vIsoCode, vDescription:=vDescription, vMinorPart:=vMinorPart, vCode:=vCode, vSymbol:=vSymbol, vAlignment:=vAlignment, vDecimalPlaces:=vDecimalPlaces, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vFormatString:=vFormatString, vRoundToPlaces:=vRoundToPlaces), gPMConstants.PMEReturnCode)

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
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oCurrency As bACTCurrency.Currency

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oCurrencys.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oCurrency = m_oCurrencys.Item(lRow)

            ' Check the Status of the Currency

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oCurrency.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oCurrency.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oCurrency.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

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
            For lSub As Integer = 1 To m_oCurrencys.Count()
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
        Dim oCurrency As bACTCurrency.Currency
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oCurrencys.Count()
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
                    lSub = 1

                    ' For each item in the collection
                    Do While lSub <= m_oCurrencys.Count()

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
    ' Name: GetISOCodeFromCurrencyID
    '
    ' Description: returns iso code from supplied currency id
    '
    ' ***************************************************************** '
    Public Function GetISOCodeFromCurrencyID(ByVal v_iCurrencyID As Integer, ByRef r_sISOCode As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' construct SQL statement
            sSQL = "SELECT iso_code FROM currency WHERE currency_id = {currency_id}"

            ' Clear parameters
            m_oDatabase.Parameters.Clear()

            ' Add the currency_id
            m_lReturn = m_oDatabase.Parameters.Add(sName:="currency_id", vValue:=CStr(v_iCurrencyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' perform SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetISOCodeFromCurrencyID", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Grab value if any returned
            If Informations.IsArray(vResultArray) Then

                r_sISOCode = CStr(vResultArray(0, 0))
            Else
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetISOCodeFromCurrencyID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetISOCodeFromCurrencyID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

                'Develoepr Guide No 111
                'r_iCurrencyId = gPMFunctions.NullToLong(.Records.Item(1).Fields()("currency_id"))
                r_iCurrencyId = gPMFunctions.NullToLong(.Records.Item(0).Fields()("currency_id"))
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
    Private Function AddItem(ByRef oCurrency As bACTCurrency.Currency) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters
        m_lReturn = CType(AddInputParam(oCurrency:=oCurrency), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add CurrencyID as an OUTPUT param for an insert
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Currency_id", vValue:=CStr(oCurrency.CurrencyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the ID of the record inserted
        oCurrency.CurrencyID = gPMFunctions.NullToLong(m_oDatabase.Parameters.Item("Currency_id").Value)

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
    Private Function UpdateItem(ByRef oCurrency As bACTCurrency.Currency) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the Input Parameters to the Parameteres collection
        m_lReturn = CType(AddInputParam(oCurrency:=oCurrency), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add CurrencyID as an INPUT param for an update
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Currency_id", vValue:=CStr(oCurrency.CurrencyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' BB Not required by Orion
        ' Add Timestamp as an INPUT param for an update
        'm_lReturn& = m_oDatabase.Parameters.Add( _
        'sName:="timestamp", _
        'vValue:=oCurrency.Timestamp, _
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
    Private Function DeleteItem(ByRef oCurrency As bACTCurrency.Currency) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the CurrencyID INPUT parameter
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Currency_id", vValue:=CStr(oCurrency.CurrencyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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
    ' Description: Sets the supplied Currency properties from a database
    '              record.
    ' ***************************************************************** '
    Private Function SetPropertiesFromDB(ByRef oCurrency As bACTCurrency.Currency, ByRef lRecordNumber As Integer) As Integer

        Dim result As Integer = 0
        'developer guide no. 112(Guide)
        Dim oFields As DataRow



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set oFields to refer to Field Collection for Record 1
        oFields = m_oDatabase.Records.Item(lRecordNumber).Fields()

        ' Populate Base Details

        With oCurrency

            .CurrencyID = gPMFunctions.NullToLong(oFields("currency_id"))
            .CaptionID = gPMFunctions.NullToLong(oFields("caption_id"))
            .IsoCode = gPMFunctions.NullToString(oFields("iso_code"))
            .Description = gPMFunctions.NullToString(oFields("description"))
            .MinorPart = gPMFunctions.NullToString(oFields("minor_part"))
            .Code = gPMFunctions.NullToString(oFields("code"))
            .Symbol = gPMFunctions.NullToString(oFields("symbol"))
            .Alignment = gPMFunctions.NullToString(oFields("alignment"))
            .DecimalPlaces = gPMFunctions.NullToLong(oFields("decimal_places"))
            .IsDeleted = gPMFunctions.NullToLong(oFields("is_deleted"))
            .EffectiveDate = gPMFunctions.NullToDate(oFields("effective_date"))
            .FormatString = gPMFunctions.NullToString(oFields("format_string"))
            .RoundToPlaces = gPMFunctions.NullToLong(oFields("round_to_places"))

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
    Private Function SetProperties(ByRef oCurrency As bACTCurrency.Currency, ByRef iStatus As Integer, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vIsoCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vMinorPart As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vSymbol As Object = Nothing, Optional ByRef vAlignment As Object = Nothing, Optional ByRef vDecimalPlaces As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vFormatString As Object = Nothing, Optional ByRef vRoundToPlaces As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        ' If Add Mode
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            ' Check that All Mandatory Parameters have been supplied
            m_lReturn = CType(CheckMandatory(vCurrencyID:=vCurrencyID, vCaptionID:=vCaptionID, vIsoCode:=vIsoCode, vDescription:=vDescription, vMinorPart:=vMinorPart, vCode:=vCode, vSymbol:=vSymbol, vAlignment:=vAlignment, vDecimalPlaces:=vDecimalPlaces, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vFormatString:=vFormatString, vRoundToPlaces:=vRoundToPlaces), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Default Any Missing Parameters
            m_lReturn = CType(DefaultParameters(bDefaultAll:=False, vCurrencyID:=vCurrencyID, vCaptionID:=vCaptionID, vIsoCode:=vIsoCode, vDescription:=vDescription, vMinorPart:=vMinorPart, vCode:=vCode, vSymbol:=vSymbol, vAlignment:=vAlignment, vDecimalPlaces:=vDecimalPlaces, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vFormatString:=vFormatString, vRoundToPlaces:=vRoundToPlaces), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

        End If

        ' Validate Parameters
        m_lReturn = CType(Validate(vCurrencyID:=vCurrencyID, vCaptionID:=vCaptionID, vIsoCode:=vIsoCode, vDescription:=vDescription, vMinorPart:=vMinorPart, vCode:=vCode, vSymbol:=vSymbol, vAlignment:=vAlignment, vDecimalPlaces:=vDecimalPlaces, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vFormatString:=vFormatString, vRoundToPlaces:=vRoundToPlaces), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
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


            If Not Informations.IsNothing(vCaptionID) Then
                If .CaptionID <> vCaptionID Then
                    .CaptionID = vCaptionID
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
                If .EffectiveDate <> vEffectiveDate Then
                    .EffectiveDate = vEffectiveDate
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
    Private Function GetProperties(ByRef oCurrency As bACTCurrency.Currency, ByRef iStatus As Integer, Optional ByRef vCurrencyID As Integer = 0, Optional ByRef vCaptionID As Integer = 0, Optional ByRef vIsoCode As String = "", Optional ByRef vDescription As String = "", Optional ByRef vMinorPart As String = "", Optional ByRef vCode As String = "", Optional ByRef vSymbol As String = "", Optional ByRef vAlignment As String = "", Optional ByRef vDecimalPlaces As Integer = 0, Optional ByRef vIsDeleted As Integer = 0, Optional ByRef vEffectiveDate As Date = #12/30/1899#, Optional ByRef vFormatString As String = "", Optional ByRef vRoundToPlaces As Integer = 0) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set Property values.
        With oCurrency

            'developer guide no. 143(Guide)
            vCurrencyID = .CurrencyID
            'developer guide no. 143(Guide)
            vCaptionID = .CaptionID
            'developer guide no. 143(Guide)
            vIsoCode = .IsoCode
            'developer guide no. 143(Guide)
            vDescription = .Description
            'developer guide no. 143(Guide)
            vMinorPart = .MinorPart
            'developer guide no. 143(Guide)
            vCode = .Code
            'developer guide no. 143(Guide)
            vSymbol = .Symbol
            'developer guide no. 143(Guide)
            vAlignment = .Alignment
            'developer guide no. 143(Guide)
            vDecimalPlaces = .DecimalPlaces
            'developer guide no. 143(Guide)
            vIsDeleted = .IsDeleted
            'developer guide no. 143(Guide)
            vEffectiveDate = .EffectiveDate
            'developer guide no. 143(Guide)
            vFormatString = .FormatString
            'developer guide no. 143(Guide)
            vRoundToPlaces = .RoundToPlaces

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
    Private Function AddInputParam(ByRef oCurrency As bACTCurrency.Currency) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase


            If oCurrency.CaptionID < 1 Then

                'developer guide no 85. 
                m_lReturn = .Parameters.Add(sName:="caption_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="caption_id", vValue:=CStr(oCurrency.CaptionID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

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

            m_lReturn = .Parameters.Add(sName:="minor_part", vValue:=oCurrency.MinorPart, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBinary)

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

            m_lReturn = .Parameters.Add(sName:="effective_date", vValue:=ToSafeString(oCurrency.EffectiveDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

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

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DefaultParameters (Public)
    '
    ' Description: Sets the Default Values for a Currency.
    '
    ' ***************************************************************** '
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vIsoCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vMinorPart As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vSymbol As Object = Nothing, Optional ByRef vAlignment As Object = Nothing, Optional ByRef vDecimalPlaces As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vFormatString As Object = Nothing, Optional ByRef vRoundToPlaces As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vCurrencyID)) Or (vCurrencyID.Equals(0)) Or (bDefaultAll) Then
            vCurrencyID = 0
        End If



        If (Informations.IsNothing(vCaptionID)) Or (vCaptionID.Equals(0)) Or (bDefaultAll) Then
            vCaptionID = 0
        End If



        If (Informations.IsNothing(vIsoCode)) Or (String.IsNullOrEmpty(vIsoCode)) Or (bDefaultAll) Then
            vIsoCode = ""
        End If



        If (Informations.IsNothing(vDescription)) Or (String.IsNullOrEmpty(vDescription)) Or (bDefaultAll) Then
            vDescription = ""
        End If



        If (Informations.IsNothing(vMinorPart)) Or (vMinorPart.Equals(0)) Or (bDefaultAll) Then
            vMinorPart = 0
        End If



        If (Informations.IsNothing(vCode)) Or (String.IsNullOrEmpty(vCode)) Or (bDefaultAll) Then
            vCode = ""
        End If



        If (Informations.IsNothing(vSymbol)) Or (String.IsNullOrEmpty(vSymbol)) Or (bDefaultAll) Then
            vSymbol = ""
        End If



        If (Informations.IsNothing(vAlignment)) Or (String.IsNullOrEmpty(vAlignment)) Or (bDefaultAll) Then
            vAlignment = ""
        End If



        If (Informations.IsNothing(vDecimalPlaces)) Or (vDecimalPlaces.Equals(0)) Or (bDefaultAll) Then
            vDecimalPlaces = 0
        End If



        If (Informations.IsNothing(vIsDeleted)) Or (vIsDeleted.Equals(0)) Or (bDefaultAll) Then
            vIsDeleted = 0
        End If



        If (Informations.IsNothing(vEffectiveDate)) Or (vEffectiveDate.Equals(DateTime.FromOADate(0))) Or (bDefaultAll) Then
            vEffectiveDate = DateTime.Now
        End If



        If (Informations.IsNothing(vFormatString)) Or (String.IsNullOrEmpty(vFormatString)) Or (bDefaultAll) Then
            vFormatString = ""
        End If



        If (Informations.IsNothing(vRoundToPlaces)) Or (vRoundToPlaces.Equals(0)) Or (bDefaultAll) Then
            vRoundToPlaces = 0
        End If


        ' {* USER DEFINED CODE (End) *}


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Public)
    '
    ' Description: Sets the Default Values for a Currency.
    '
    ' ***************************************************************** '
    Private Function CheckMandatory(Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vIsoCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vMinorPart As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vSymbol As Object = Nothing, Optional ByRef vAlignment As Object = Nothing, Optional ByRef vDecimalPlaces As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vFormatString As Object = Nothing, Optional ByRef vRoundToPlaces As Object = Nothing) As Integer

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
    ' Name: Validate (Private)
    '
    ' Description: Checks the Currency for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vIsoCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vMinorPart As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vSymbol As Object = Nothing, Optional ByRef vAlignment As Object = Nothing, Optional ByRef vDecimalPlaces As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vFormatString As Object = Nothing, Optional ByRef vRoundToPlaces As Object = Nothing) As Integer

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


    Public Function GetSystemCurrency(ByRef r_iCurrencyId As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()


            'developer guide no 85. 
            m_lReturn = m_oDatabase.Parameters.Add(sName:="currency_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetSystemCurrencySQL, sSQLName:=ACGetSystemCurrencyName, bStoredProcedure:=ACGetSystemCurrencyStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_iCurrencyId = m_oDatabase.Parameters.Item("currency_id").Value

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFail

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSystemCurrency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSystemCurrency", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class

