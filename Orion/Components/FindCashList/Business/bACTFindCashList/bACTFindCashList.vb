Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Form_NET.Form")> _
Public NotInheritable Class Form
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness

    ' ************************************************
    ' Added to replace global variables 10/12/2003
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************


    ' ***************************************************************** '
    ' Class Name: FindCashList
    '
    ' Date: 3rd September 1997
    '
    ' Description: Creatable class used by the FindCashList interface.
    '
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' Constants for use in SQL
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lError As gPMConstants.PMEReturnCode

    ' Task
    Private m_iTask As Integer

    ' Navigate
    Private m_lNavigate As Integer

    ' Process Mode
    Private m_lProcessMode As Integer

    ' Type of Business
    Private m_sTypeOfBusiness As New FixedLengthString(10)

    ' Effective
    Private m_dtEffectiveDate As Date

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Component Sub Type
    Private m_sSubType As New FixedLengthString(20)
    ' Variable Data Business Component (Private)

    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
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

    Public ReadOnly Property TypeOfBusiness() As String
        Get

            Return m_sTypeOfBusiness.Value

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
        End Get
    End Property

    ' PUBLIC Property Procedures (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTypeOfBusiness As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

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


            If Not Information.IsNothing(vTypeOfBusiness) Then

                m_sTypeOfBusiness.Value = CStr(vTypeOfBusiness)
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
    ' Description: Gets the Lookup values as defined by vTableArray.
    '
    '
    ' ***************************************************************** '
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            vResultArray = Nothing

            ' Get the Lookup items from the Business Component

            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTableArray, iLanguageID:=iLanguageID, dtEffectiveDate:=m_dtEffectiveDate, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

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

            ' Set User ID
            m_iUserID = iUserID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level


            ' Get Reference to Database
            'Set m_oDatabase = GetOrionDatabase( _
            'lOpenStatus:=m_lReturn, _
            'bCloseDatabase:=m_bCloseDatabase, _
            'vDatabase:=vDatabase)

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Create PM Lookup Business Object
            m_oLookup = New bPMLookup.Business()

            ' Initialise PM Lookup Business Object passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFOrion

            Return result

        Catch excep As System.Exception



            ' Error Section.
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
                m_oLookup = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    '****************************************************************** '
    ' Name: SearchDetails (Public)
    '
    ' Description: Selects CashLists according to the query by given
    '              parameters
    '
    '****************************************************************** '
    Public Function SearchDetails(ByRef lNumberOfRecords As Integer, ByRef vResultArray(,) As Object, ByVal iCompanyID As Integer, Optional ByVal vCashListStatusID As Integer = 0, Optional ByVal vCashListTypeID As Integer = 0, Optional ByVal vCashListRef As String = "", Optional ByVal vBankAccountID As Integer = 0, Optional ByVal vCurrencyID As Integer = 0, Optional ByVal vStartDate As Byte = 0, Optional ByVal vEndDate As Byte = 0, Optional ByVal vControlTotal As Integer = 0, Optional ByVal vItemCount As Integer = 0) As Integer

        Dim result As Integer = 0
        On Error GoTo Err_SearchDetails

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the CompanyID parameter (INPUT)
        If m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(iCompanyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then GoTo Err_Parameter_Add

        ' CashListStatusID

        If Information.IsNothing(vCashListStatusID) Then

            vCashListStatusID = Nothing
        Else
            If vCashListStatusID = -1 Then

                vCashListStatusID = Nothing
            End If
        End If

        ' Add the CashListStatusID parameter (INPUT)
        If m_oDatabase.Parameters.Add(sName:="cashliststatus_id", vValue:=CStr(vCashListStatusID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then GoTo Err_Parameter_Add

        ' CashListTypeID

        If Information.IsNothing(vCashListTypeID) Then

            vCashListTypeID = Nothing
        Else
            If vCashListTypeID = -1 Then

                vCashListTypeID = Nothing
            End If
        End If

        ' Add the CashListTypeID parameter (INPUT)
        If m_oDatabase.Parameters.Add(sName:="cashlisttype_id", vValue:=CStr(vCashListTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then GoTo Err_Parameter_Add

        ' CashListRef

        If Information.IsNothing(vCashListRef) Then

            vCashListRef = Nothing
        Else
            If vCashListRef = "" Then

                vCashListRef = Nothing
            Else
                vCashListRef = bPMFunc.ConvertWildCard(v_sSearchString:=vCashListRef)
            End If
        End If

        ' Add the CashListRef parameter (INPUT)
        If m_oDatabase.Parameters.Add(sName:="cashlist_ref", vValue:=vCashListRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then GoTo Err_Parameter_Add

        ' BankAccountID

        If Information.IsNothing(vBankAccountID) Then

            vBankAccountID = Nothing
        Else
            If vBankAccountID = -1 Then

                vBankAccountID = Nothing
            End If
        End If
        ' Add the BankAccountID parameter (INPUT)
        If m_oDatabase.Parameters.Add(sName:="bankaccount_id", vValue:=CStr(vBankAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then GoTo Err_Parameter_Add

        ' CurrencyID

        If Information.IsNothing(vCurrencyID) Then

            vCurrencyID = Nothing
        Else
            If vCurrencyID = -1 Then

                vCurrencyID = Nothing
            End If
        End If

        ' Add the CurrencyID parameter (INPUT)
        If m_oDatabase.Parameters.Add(sName:="currency_id", vValue:=CStr(vCurrencyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then GoTo Err_Parameter_Add

        ' StartDate

        If Information.IsNothing(vStartDate) Then

            vStartDate = Nothing
        Else
            If vStartDate = 0 Then

                vStartDate = Nothing
            End If
        End If

        ' Add the StartDate parameter (INPUT)
        If m_oDatabase.Parameters.Add(sName:="start_date", vValue:=CStr(vStartDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then GoTo Err_Parameter_Add

        ' EndDate

        If Information.IsNothing(vEndDate) Then

            vEndDate = Nothing
        Else
            If vEndDate = 0 Then

                vEndDate = Nothing
            End If
        End If

        ' Add the EndDate parameter (INPUT)
        If m_oDatabase.Parameters.Add(sName:="end_date", vValue:=CStr(vEndDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then GoTo Err_Parameter_Add

        ' ControlTotal

        If Information.IsNothing(vControlTotal) Then

            vControlTotal = Nothing
        Else
            If vControlTotal = -1 Then

                vControlTotal = Nothing
            End If
        End If

        ' Add the ControlTotal parameter (INPUT)
        If m_oDatabase.Parameters.Add(sName:="control_total", vValue:=CStr(vControlTotal), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then GoTo Err_Parameter_Add

        ' ItemCount

        If Information.IsNothing(vItemCount) Then

            vItemCount = Nothing
        Else
            If vItemCount = -1 Then

                vItemCount = Nothing
            End If
        End If

        ' Add the ItemCount parameter (INPUT)
        If m_oDatabase.Parameters.Add(sName:="item_count", vValue:=CStr(vItemCount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then GoTo Err_Parameter_Add

        'Execute SQL Statement
        m_lError = m_oDatabase.SQLSelect(sSQL:=ACCashListFromQuerySQL, sSQLName:=ACCashListFromQueryName, bStoredProcedure:=ACCashListFromQueryStored, lNumberRecords:=lNumberOfRecords, vResultArray:=vResultArray)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchDetails")

            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Return CheckResults(vResultArray)

Err_SearchDetails:

        ' Error Section.

        result = gPMConstants.PMEReturnCode.PMError

        MessageBox.Show(Information.Err().Description, Application.ProductName)
        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

Err_Parameter_Add:

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchDetails")

        Return gPMConstants.PMEReturnCode.PMError

    End Function

    '' ***************************************************************** '
    '' Name: GetID (Public)
    ''
    '' Description: Gets the (last) CashListID matching
    ''              the given parameters.
    ''
    '' ***************************************************************** '
    'Public Function GetID( _
    ''             ByRef lCashListID As Long, _
    ''             ByVal iCompanyID As Integer, _
    ''    Optional ByVal vFullKey As Variant, _
    ''    Optional ByVal vShortCode As Variant) As Long
    '
    'Dim lRowsAffected As Long
    '
    '    On Error GoTo Err_GetID
    '
    '    GetID = PMTrue
    '
    '    ' At least one of vFullKey or vShortCode must be defined
    '    If (IsMissing(vFullKey) And IsMissing(vShortCode)) = True Then
    '        GetID = PMFalse
    '        lCashListID = -1
    '        Exit Function
    '    End If
    '
    '    ' Supply defaults for missing parameters
    '    If (IsMissing(vFullKey) = True) Then vFullKey = ""
    '    If (IsMissing(vShortCode) = True) Then vShortCode = ""
    '
    '    ' Clear the Database Parameters Collection
    '    m_oDatabase.Parameters.Clear
    '
    '    ' Add the CompanyID parameter (INPUT)
    '    If m_oDatabase.Parameters.Add( _
    ''        sName:="CompanyID", _
    ''        vValue:=CVar(iCompanyID), _
    ''        iDirection:=PMParamInput, _
    ''        iDataType:=PMInteger) <> PMTrue Then GoTo Err_Parameter_Add
    '
    '    ' Add the ShortCode parameter (INPUT)
    '    If m_oDatabase.Parameters.Add( _
    ''        sName:="ShortCode", _
    ''        vValue:=vShortCode, _
    ''        iDirection:=PMParamInput, _
    ''        iDataType:=PMString) <> PMTrue Then GoTo Err_Parameter_Add
    '
    '    ' Add the FullKey parameter (INPUT)
    '    If m_oDatabase.Parameters.Add( _
    ''        sName:="FullKey", _
    ''        vValue:=vFullKey, _
    ''        iDirection:=PMParamInput, _
    ''        iDataType:=PMString) <> PMTrue Then GoTo Err_Parameter_Add
    '
    '    ' Add the CashListID parameter (OUTPUT)
    '    If m_oDatabase.Parameters.Add( _
    ''            sName:="CashListID", _
    ''            vValue:=CVar(lCashListID), _
    ''            iDirection:=PMParamOutput, _
    ''            iDataType:=PMLong) <> PMTrue Then GoTo Err_Parameter_Add
    '
    '    ' Execute SQL Statement
    '    m_lError& = m_oDatabase.SQLAction( _
    ''        sSQL:=ACGetCashListIDSQL, _
    ''        sSQLName:=ACGetCashListIDName, _
    ''        bStoredProcedure:=ACGetCashListIDStored, _
    ''        lRecordsAffected:=lRowsAffected)
    '
    '    If (m_lError& <> PMTrue) Then
    '
    '        ' Log Error Message
    '        LogMessage m_sUsername, _
    ''            iType:=PMLogError, _
    ''            sMsg:="m_oDatabase.SQLAction failed", _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="GetID"
    '
    '        GetID = PMFalse
    '        Exit Function
    '
    '    End If
    '
    '    ' Get the CashListID of the record selected
    '     lCashListID = NullToLong(m_oDatabase.Parameters.Item("CashListID").Value)
    '
    '    If (lCashListID = -1) Then
    '        GetID = PMNotFound
    '    End If
    '
    '    Exit Function
    '
    'Err_GetID:
    '
    '    ' Error Section.
    '
    '    GetID = PMError
    '
    '    ' Log Error Message
    '    LogMessage m_sUsername, _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="GetID Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="GetID", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'Err_Parameter_Add:
    '
    '    ' Log Error Message
    '    LogMessage m_sUsername, _
    ''        iType:=PMLogError, _
    ''        sMsg:="oParameters.Add failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="GetID"
    '
    '    GetID = PMError
    '    Exit Function
    '
    'End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

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
        ' Error Section.
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
