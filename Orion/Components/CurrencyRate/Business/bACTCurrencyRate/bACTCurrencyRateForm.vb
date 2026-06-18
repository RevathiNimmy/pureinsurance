Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
'Developer Guide
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Form_NET.Form")>
Public NotInheritable Class Form
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 24/07/1997
    '
    ' Description: Creatable Form class which contains all the
    '              methods, Form rules required to manipulate
    '              a CurrencyRate.
    '
    ' Edit History: TF181198 - rate_against_euro column deleted
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

    ' Collection of CurrencyRates (Private)
    Private m_oCurrencyRates As CurrencyRates

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
    Private m_iCompanyID As Integer
    Private m_sUniqueId As String
    Private m_sScreenHierarchy As String
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
        End Get
    End Property

    Public Property CompanyId() As Integer
        Get

            Return m_iCompanyID

        End Get
        Set(ByVal Value As Integer)

            m_iCompanyID = Value

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
                Case Is > m_oCurrencyRates.Count()
                    m_lCurrentRecord = m_oCurrencyRates.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Numner in Collection
            Return m_oCurrencyRates.Count()

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

    Public Property UniqueId() As String
        Get
            Return m_sUniqueId
        End Get
        Set(ByVal Value As String)

            m_sUniqueId = Value

        End Set
    End Property

    Public Property ScreenHierarchy() As String
        Get
            Return m_sScreenHierarchy
        End Get
        Set(ByVal Value As String)

            m_sScreenHierarchy = Value

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


            ' Set database


            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Set the ProcessMode etc.
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create CurrencyRates Collection
            m_oCurrencyRates = New CurrencyRates()

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
                m_oCurrencyRates = Nothing
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

                'm_dtEffectiveDate = CDate(vEffectiveDate)
                m_dtEffectiveDate = vEffectiveDate
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single CurrencyRate directly into the database.
    '        Note: The CurrencyRate will NOT be added to the collection.
    '
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByRef vEffectiveFrom As Object = Nothing, Optional ByRef vRateAgainstBase As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oCurrencyRate As bACTCurrencyRate.CurrencyRate

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new CurrencyRate
            oCurrencyRate = New bACTCurrencyRate.CurrencyRate()

            ' Populate CurrencyRate Attributes




            'm_lReturn = CType(SetProperties(oCurrencyRate, gPMConstants.PMEComponentAction.PMAdd, vEffectiveFrom:=CDate(vEffectiveFrom), vRateAgainstBase:=CDec(vRateAgainstBase), vCurrencyID:=CInt(vCurrencyID), vCompanyID:=CInt(vCompanyID)), gPMConstants.PMEReturnCode)
            m_lReturn = CType(SetProperties(oCurrencyRate, gPMConstants.PMEComponentAction.PMAdd, vEffectiveFrom:=vEffectiveFrom, vRateAgainstBase:=vRateAgainstBase, vCurrencyID:=vCurrencyID, vCompanyID:=vCompanyID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the CurrencyRate to the Database
            m_lReturn = CType(AddItem(oCurrencyRate), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

            oCurrencyRate = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the CurrencyRate.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vEffectiveFrom As Object = Nothing, Optional ByRef vRateAgainstBase As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults




            'm_lReturn = CType(DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vEffectiveFrom:=CDate(vEffectiveFrom), vRateAgainstBase:=CByte(vRateAgainstBase), vCurrencyID:=CByte(vCurrencyID), vCompanyID:=CByte(vCompanyID)), gPMConstants.PMEReturnCode)
            m_lReturn = CType(DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vEffectiveFrom:=vEffectiveFrom, vRateAgainstBase:=vRateAgainstBase, vCurrencyID:=vCurrencyID, vCompanyID:=vCompanyID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required CurrencyRates and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetails(ByVal v_lCompanyID As Integer, ByVal v_dtEffectiveFrom As Date) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oCurrencyRate As CurrencyRate

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oCurrencyRates.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the CompanyID parameter (INPUT)
            If v_lCompanyID = 0 Then

                'Developer Guide No 85
                m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(v_lCompanyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the EffectiveFrom parameter (INPUT)
            'developer guide no.40

            'v_dtEffectiveFrom = DateTime.Parse(v_dtEffectiveFrom.ToString("yyyy-dd-MM"))

            v_dtEffectiveFrom = DateTime.ParseExact(v_dtEffectiveFrom.ToString("yyyy-dd-MM"), "yyyy-dd-MM", System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"))
            m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=v_dtEffectiveFrom, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?

            If lRecordCount < 1 Then

                'No records, just means none are set up.
                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound

            Else

                ' Yes, load them into the collection
                For lSub As Integer = 1 To lRecordCount

                    ' Create New CurrencyRate
                    oCurrencyRate = New bACTCurrencyRate.CurrencyRate()
                    'developer guide no.162
                    m_lReturn = CType(SetPropertiesFromDB(oCurrencyRate:=oCurrencyRate, lRecordNumber:=lSub - 1), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    ' Add CurrencyRate to collection
                    If (m_oCurrencyRates.Count = 0) Then
                        m_oCurrencyRates.Add(Nothing)
                    End If
                    m_lReturn = CType(m_oCurrencyRates.Add(oNewCurrencyRate:=oCurrencyRate), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oCurrencyRate = Nothing

                Next lSub

            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required CurrencyRates and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vEffectiveFrom As Object = Nothing, Optional ByRef vRateAgainstBase As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing) As Integer

        Dim result As Integer = 0

        Dim oCurrencyRate As CurrencyRate
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oCurrencyRates.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                Return gPMConstants.PMEReturnCode.PMEOF
            End If

            oCurrencyRate = m_oCurrencyRates.Item(m_lCurrentRecord)

            ' Get the CurrencyRate Property Values




            'developer guide no.98
            m_lReturn = GetProperties(oCurrencyRate, iStatus,
            vEffectiveFrom:=vEffectiveFrom,
            vRateAgainstBase:=vRateAgainstBase,
            vCurrencyID:=vCurrencyID,
            vCompanyID:=vCompanyID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oCurrencyRate = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetCount (Public)
    '
    ' Description: get the number of rows in the collection
    '
    ' ***************************************************************** '
    Public Function GetCount(ByRef v_lCount As Integer) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        v_lCount = m_oCurrencyRates.Count()
        Return result



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCount", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied CurrencyRate into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vEffectiveFrom As Object = Nothing, Optional ByRef vRateAgainstBase As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oCurrencyRate As bACTCurrencyRate.CurrencyRate

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            'eck280600 -ignore check if lrow = 0
            If m_oCurrencyRates.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new CurrencyRate
            oCurrencyRate = New bACTCurrencyRate.CurrencyRate()

            ' Populate CurrencyRate Attributes




            'm_lReturn = CType(SetProperties(oCurrencyRate, gPMConstants.PMEComponentAction.PMAdd, vEffectiveFrom:=CDate(vEffectiveFrom), vRateAgainstBase:=CDec(vRateAgainstBase), vCurrencyID:=CInt(vCurrencyID), vCompanyID:=CInt(vCompanyID)), gPMConstants.PMEReturnCode)
            m_lReturn = CType(SetProperties(oCurrencyRate, gPMConstants.PMEComponentAction.PMAdd, vEffectiveFrom:=vEffectiveFrom, vRateAgainstBase:=vRateAgainstBase, vCurrencyID:=vCurrencyID, vCompanyID:=vCompanyID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oCurrencyRate = Nothing
                Return result
            End If

            ' Add CurrencyRate to collection
            If (m_oCurrencyRates.Count = 0) Then
                m_oCurrencyRates.Add(Nothing)
            End If
            m_lReturn = CType(m_oCurrencyRates.Add(oNewCurrencyRate:=oCurrencyRate), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oCurrencyRate = Nothing
                Return result
            End If

            oCurrencyRate = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the CurrencyRate
    '              specified and updates the CurrencyRate with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vEffectiveFrom As Object = Nothing, Optional ByRef vRateAgainstBase As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oCurrencyRate As bACTCurrencyRate.CurrencyRate
        Dim iStatus As gPMConstants.PMEComponentAction

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oCurrencyRates.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oCurrencyRate = m_oCurrencyRates.Item(lRow)

            ' Check the Status of the CurrencyRate

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oCurrencyRate.DatabaseStatus
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

            ' Update CurrencyRate Attributes




            'm_lReturn = CType(SetProperties(oCurrencyRate, iStatus, vEffectiveFrom:=CDate(vEffectiveFrom), vRateAgainstBase:=CDec(vRateAgainstBase), vCurrencyID:=CInt(vCurrencyID), vCompanyID:=CInt(vCompanyID)), gPMConstants.PMEReturnCode)
            m_lReturn = CType(SetProperties(oCurrencyRate, iStatus, vEffectiveFrom:=vEffectiveFrom, vRateAgainstBase:=vRateAgainstBase, vCurrencyID:=vCurrencyID, vCompanyID:=vCompanyID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oCurrencyRate = Nothing
                Return result
            End If

            ' Release reference to CurrencyRate
            oCurrencyRate = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified CurrencyRate can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oCurrencyRate As CurrencyRate

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oCurrencyRates.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oCurrencyRate = m_oCurrencyRates.Item(lRow)

            ' Check the Status of the CurrencyRate

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oCurrencyRate.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oCurrencyRate.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oCurrencyRate.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to CurrencyRate
            oCurrencyRate = Nothing

            Return result

        Catch excep As System.Exception




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
            For lSub As Integer = 1 To m_oCurrencyRates.Count()
                Select Case m_oCurrencyRates.Item(lSub).DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing
                    Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMDelete
                        result = gPMConstants.PMEReturnCode.PMDataChanged
                        Exit For
                End Select
            Next lSub

            Return result

        Catch excep As System.Exception




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
        Dim oCurrencyRate As bACTCurrencyRate.CurrencyRate
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oCurrencyRates.Count()
                oCurrencyRate = m_oCurrencyRates.Item(lSub)

                If m_sUniqueId <> "" Then
                    m_sScreenHierarchy = $"Currency Rates({oCurrencyRate.EffectiveFrom})"
                End If
                Select Case oCurrencyRate.DatabaseStatus
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
                        m_lReturn = CType(AddItem(oCurrencyRate), gPMConstants.PMEReturnCode)
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
                        m_lReturn = CType(UpdateItem(oCurrencyRate), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Release last reference
            oCurrencyRate = Nothing

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
                    Do While lSub <= m_oCurrencyRates.Count()

                        ' With the item
                        With m_oCurrencyRates.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oCurrencyRates.Delete(lSub)

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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '****************************************************************** '
    '* Name: GetRateForDate (Public)
    '*
    '* Description: Retrieves CurrencyRate record for a specified
    '*              CompanyCurrency_id and date
    '*
    '****************************************************************** '
    Public Function GetRateForDate(ByVal lCurrencyID As Integer, ByVal lCompanyID As Integer, ByVal dtRateDate As Date, ByRef dRate As Double) As Integer

        ' Uses stored procedure
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the currency_ID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add("currency_ID", CStr(lCurrencyID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the company_ID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add("company_ID", CStr(lCompanyID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the rate_date parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add("effective_date", CStr(dtRateDate), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the rate parameter (OUTPUT)
            m_lReturn = m_oDatabase.Parameters.Add("rate", CStr(dtRateDate), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDouble)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetRateForDateSQL, sSQLName:=ACGetRateForDateName, bStoredProcedure:=ACGetRateForDateStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            dRate = m_oDatabase.Parameters.Item("rate").Value

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRateForDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRateForDate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '****************************************************************** '
    '* Name: ApplyToAllBranches (Public)
    '*
    '* Description: Applies the currencyrates set up for head office
    '* for a particular currency to all non-deleted branches
    '*
    '****************************************************************** '
    Public Function ApplyToAllBranches(ByVal lCurrencyID As Integer) As Integer

        ' Uses stored procedure

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the currency_ID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="currency_ID", vValue:=CStr(lCurrencyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ApplyToAllBranches")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If



            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACApplyCurrencyRateToAllBranchesSQL, sSQLName:=ACApplyCurrencyRateToAllBranchesName, bStoredProcedure:=ACApplyCurrencyRateToAllBranchesStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ApplyToAllBranches")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If



            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            '''''MessageBox.Show(excep.Message, Application.ProductName)
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ApplyToAllBranches Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ApplyToAllBranches", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Private Function AddItem(ByRef oCurrencyRate As bACTCurrencyRate.CurrencyRate) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters
        m_lReturn = CType(AddInputParam(oCurrencyRate:=oCurrencyRate), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)

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
    Private Function UpdateItem(ByRef oCurrencyRate As bACTCurrencyRate.CurrencyRate) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the Input Parameters to the Parameteres collection
        m_lReturn = CType(AddInputParam(oCurrencyRate:=oCurrencyRate), gPMConstants.PMEReturnCode)

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
    ' Name: SetPropertiesFromDB (Private)
    '
    ' Description: Sets the supplied CurrencyRate properties from a database
    '              record.
    ' ***************************************************************** '
    Private Function SetPropertiesFromDB(ByRef oCurrencyRate As bACTCurrencyRate.CurrencyRate, ByRef lRecordNumber As Integer) As Integer

        Dim result As Integer = 0
        'developer guide no.112
        Dim oFields As DataRow



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set oFields to refer to Field Collection for Record 1
        oFields = m_oDatabase.Records.Item(lRecordNumber).Fields()

        ' Populate Base Details

        With oCurrencyRate

            .EffectiveFrom = gPMFunctions.NullToDate(oFields("effective_from"))
            .RateAgainstBase = gPMFunctions.NullToDecimal(oFields("rate_against_base"))
            .CurrencyID = gPMFunctions.NullToLong(oFields("currency_id"))
            .CompanyId = gPMFunctions.NullToLong(oFields("company_id"))

            .DatabaseStatus = gPMConstants.PMEComponentAction.PMView

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Private)
    '
    ' Description: Sets the supplied CurrencyRate property values.
    '
    ' ***************************************************************** '
    Private Function SetProperties(ByRef oCurrencyRate As bACTCurrencyRate.CurrencyRate, ByRef iStatus As Integer, Optional ByRef vEffectiveFrom As Object = Nothing, Optional ByRef vRateAgainstBase As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        ' If Add Mode
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            ' Check that All Mandatory Parameters have been supplied
            m_lReturn = CType(CheckMandatory(vEffectiveFrom:=vEffectiveFrom, vRateAgainstBase:=vRateAgainstBase, vCurrencyID:=vCurrencyID, vCompanyID:=vCompanyID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Default Any Missing Parameters
            m_lReturn = CType(DefaultParameters(bDefaultAll:=False, vEffectiveFrom:=vEffectiveFrom, vRateAgainstBase:=vRateAgainstBase, vCurrencyID:=vCurrencyID, vCompanyID:=vCompanyID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

        End If

        ' Validate Parameters
        m_lReturn = CType(Validate(vEffectiveFrom:=vEffectiveFrom, vRateAgainstBase:=vRateAgainstBase, vCurrencyID:=vCurrencyID, vCompanyID:=vCompanyID), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        ' Set Data Changed Flag to False
        bDataChanged = False

        ' Set Property values.
        With oCurrencyRate


            If Not Informations.IsNothing(vEffectiveFrom) Then
                If .EffectiveFrom <> vEffectiveFrom Then
                    .EffectiveFrom = vEffectiveFrom
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vRateAgainstBase) Then
                If .RateAgainstBase <> vRateAgainstBase Then
                    .RateAgainstBase = vRateAgainstBase
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vCurrencyID) Then
                If .CurrencyID <> vCurrencyID Then
                    .CurrencyID = vCurrencyID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vCompanyID) Then
                If .CompanyId <> vCompanyID Then
                    .CompanyId = vCompanyID
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
    ' Description: Returns the supplied CurrencyRate property values.
    '
    ' ***************************************************************** '
    'developer guide no.101
    Private Function GetProperties(ByRef oCurrencyRate As bACTCurrencyRate.CurrencyRate, ByRef iStatus As Integer, Optional ByRef vEffectiveFrom As Object = Nothing, Optional ByRef vRateAgainstBase As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set Property values.
        With oCurrencyRate


            vEffectiveFrom = .EffectiveFrom

            vRateAgainstBase = .RateAgainstBase

            vCurrencyID = .CurrencyID

            vCompanyID = .CompanyId

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
    Private Function AddInputParam(ByRef oCurrencyRate As bACTCurrencyRate.CurrencyRate) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase
            m_lReturn = .Parameters.Add(sName:="effective_from", vValue:=oCurrencyRate.EffectiveFrom, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="rate_against_base", vValue:=CStr(oCurrencyRate.RateAgainstBase), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oCurrencyRate.CurrencyID < 1 Then

                'Developer Guide No 85
                m_lReturn = .Parameters.Add(sName:="currency_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="currency_id", vValue:=CStr(oCurrencyRate.CurrencyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oCurrencyRate.CompanyId < 1 Then

                'Developer Guide No 85
                m_lReturn = .Parameters.Add(sName:="company_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="company_id", vValue:=CStr(oCurrencyRate.CompanyId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            m_lReturn = .Parameters.Add(sName:="user_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_sUniqueId = "" Then

                'Developer Guide No 85
                m_lReturn = .Parameters.Add(sName:="unique_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                m_lReturn = .Parameters.Add(sName:="screen_hierarchy", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lReturn = .Parameters.Add(sName:="unique_id", vValue:=CStr(m_sUniqueId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                m_lReturn = .Parameters.Add(sName:="screen_hierarchy", vValue:=CStr(m_sScreenHierarchy), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
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
    ' Description: Sets the Default Values for a CurrencyRate.
    '
    ' ***************************************************************** '
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vEffectiveFrom As Object = Nothing, Optional ByRef vRateAgainstBase As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vEffectiveFrom)) Or (vEffectiveFrom.Equals(DateTime.FromOADate(0))) Or (bDefaultAll) Then
            vEffectiveFrom = DateTime.Now
        End If



        If (Informations.IsNothing(vRateAgainstBase)) Or (vRateAgainstBase.Equals(0)) Or (bDefaultAll) Then
            vRateAgainstBase = 0
        End If



        If (Informations.IsNothing(vCurrencyID)) Or (vCurrencyID.Equals(0)) Or (bDefaultAll) Then
            vCurrencyID = 0
        End If



        If (Informations.IsNothing(vCompanyID)) Or (vCompanyID.Equals(0)) Or (bDefaultAll) Then
            vCompanyID = 0
        End If

        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Public)
    '
    ' Description: Sets the Default Values for a CurrencyRate.
    '
    ' ***************************************************************** '
    Private Function CheckMandatory(Optional ByRef vEffectiveFrom As Object = Nothing, Optional ByRef vRateAgainstBase As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vEffectiveFrom)) Or (Object.Equals(vEffectiveFrom, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vRateAgainstBase)) Or (Object.Equals(vRateAgainstBase, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vCurrencyID)) Or (Object.Equals(vCurrencyID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vCompanyID)) Or (Object.Equals(vCompanyID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the CurrencyRate for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vEffectiveFrom As Object = Nothing, Optional ByRef vRateAgainstBase As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing) As Integer

        Dim result As Integer = 0




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}

        If Not Informations.IsDate(vEffectiveFrom) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp As Double
        If Not Double.TryParse(CStr(vRateAgainstBase), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp2 As Double
        If Not Double.TryParse(CStr(vCurrencyID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp3 As Double
        If Not Double.TryParse(CStr(vCompanyID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    'UPGRADE_NOTE: (7001) The following declaration (CheckResults) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CheckResults(ByRef vResultArray As Object) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' If NO records were found return PMNotFound
    'If Not Informations.IsArray(vResultArray) Then
    'result = gPMConstants.PMEReturnCode.PMNotFound
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
    'MessageBox.Show(excep.Message, Application.ProductName)
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckResults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckResults", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
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


    Public Function GetTypeOfRates(ByRef r_iTypeOfRates As Integer) As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()


            'Developer Guide No 85
            m_lReturn = m_oDatabase.Parameters.Add("TypeOfRates", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetTypeOfRatesSQL, sSQLName:=ACGetTypeOfRatesName, bStoredProcedure:=ACGetTypeOfRatesStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_iTypeOfRates = m_oDatabase.Parameters.Item("TypeOfRates").Value

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            '''''MessageBox.Show(excep.Message, Application.ProductName)
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTypeOfRates Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTypeOfRates", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetNextEffectiveDate(ByVal v_bNext As Boolean, ByVal v_iCompanyID As Integer, ByRef r_dtEffectiveDate As Date) As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add("next", CStr(v_bNext), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMBoolean)
            If v_iCompanyID = 0 Then

                'Developer Guide No 85
                m_lReturn = m_oDatabase.Parameters.Add("company_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = m_oDatabase.Parameters.Add("company_id", CStr(v_iCompanyID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            End If
            m_lReturn = m_oDatabase.Parameters.Add("effective_date", r_dtEffectiveDate, gPMConstants.PMEParameterDirection.PMParamInputOutput, gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetNextEffectiveDateSQL, sSQLName:=ACGetNextEffectiveDateName, bStoredProcedure:=ACGetNextEffectiveDateStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_dtEffectiveDate = m_oDatabase.Parameters.Item("effective_date").Value

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            '''''MessageBox.Show(excep.Message, Application.ProductName)
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNextEffectiveDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNextEffectiveDate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class

