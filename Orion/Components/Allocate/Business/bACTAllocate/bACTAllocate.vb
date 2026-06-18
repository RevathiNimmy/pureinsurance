Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
Imports System.Runtime.InteropServices
Imports SSP.Shared
'Developer Guide 29
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: bACTAllocate
    '
    ' Date: 28 August 1997
    '
    ' Description: Orion component for allocation transactions
    '              against each other.
    '
    ' Edit History:
    '   DD 02/08/2002: Altered for Multi-Branch Accounting.
    ' RAW 13/01/2003 : PS187 : replaced hard-coded sql that deleted from
    '                          TransMatch with stored procedure
    ' RAW 01/04/2003 : ISS2854 : corrected allocationdetail unrounded, orig and os amounts
    ' CJB 04/02/2005 : PN18528 : Change Allocate to use property procedure variable if optional
    '                            CompanyID not passed in.
    '****************************************************************** '

    ' ************************************************
    ' Added to replace global variables 02/12/2003
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
    Private Const ACClass As String = "Business"


    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database
    Private m_oS4BDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lError As Integer

    ' Task
    Private m_iTask As Integer

    ' Navigate
    Private m_lNavigate As Integer

    ' Process Mode
    Private m_lProcessMode As Integer

    ' Type of Business
    Private m_sTypeOfBusiness As New StringsHelper.FixedLengthString(10)

    ' Effective
    Private m_dtEffectiveDate As Date

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Component Sub Type
    Private m_sSubType As New StringsHelper.FixedLengthString(20)
    ' Variable Data Business Component (Private)

    ' PM Lookup Business Component (Private)
    Private m_oLookup As BPMLOOKUP.Business

    ' NavigatorV3 variables
    Private m_lPMAuthorityLevel As Integer
    Private m_lStatus As Integer

    Private m_oAllocation As bACTAllocation.Form
    Private m_oAllocationDetail As bACTAllocationdetail.Form
    Private m_oMatchGroup As bACTMatchgroup.Form
    Private m_oTransmatch As bACTTransmatch.Form

    Private m_oCurrencyConvert As bACTCurrencyConvert.Form
    Private m_oPeriod As bACTPeriod.Form
    Private m_oAccount As bACTAccount.Form

    Private m_iCompany_Id As Integer
    Private m_iCurrency_Id As Integer
    Private m_lAccount_Id As Integer
    Private m_lWriteOffTransdetail_Id As Integer
    Private m_lWriteOffReason_Id As Integer
    Private m_cWriteOffAmount As Decimal

    'eck100502 Write Off
    Private m_cWriteOffBaseAmount As Decimal
    Private m_cWriteOfAmount As Decimal
    Private m_lWriteOfAllocationDetail_id As Integer
    'eck100502End

    'Commission movement data
    Private m_sCommissionOption As String = ""
    Private m_oSystemOption As bSIROptions.Business

    Private m_oTransDetail As bACTTransdetail.Form

    ' Moved with global variable changes
    Private m_oMatchPost As bACTMatchPost.Form
    'Private m_bWithDID As Boolean Deleted as part of FSA Phase 3.2

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

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
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
    Public Property CompanyId() As Integer
        Get

            Return m_iCompany_Id

        End Get
        Set(ByVal Value As Integer)
            m_iCompany_Id = Value
        End Set
    End Property
    Public WriteOnly Property CurrencyId() As Integer
        Set(ByVal Value As Integer)
            m_iCurrency_Id = Value
        End Set
    End Property
    Public ReadOnly Property CurrrencyId() As Integer
        Get

            Return m_iCurrency_Id

        End Get
    End Property
    Public Property AccountId() As Integer
        Get

            Return m_lAccount_Id

        End Get
        Set(ByVal Value As Integer)
            m_lAccount_Id = Value
        End Set
    End Property
    Public Property WriteOffTransdetailId() As Integer
        Get

            Return m_lWriteOffTransdetail_Id

        End Get
        Set(ByVal Value As Integer)

            m_lWriteOffTransdetail_Id = Value

        End Set
    End Property
    Public Property WriteOffReasonId() As Integer
        Get

            Return m_lWriteOffReason_Id

        End Get
        Set(ByVal Value As Integer)

            m_lWriteOffReason_Id = Value

        End Set
    End Property
    Public Property WriteOffAmount() As Decimal
        Get

            Return m_cWriteOffAmount

        End Get
        Set(ByVal Value As Decimal)

            m_cWriteOffAmount = Value

        End Set
    End Property
    ' PUBLIC Property Procedures (End)

    ' PUBLIC Methods (Begin)


    ' ***************************************************************** '
    ' Name: SetKeys
    '
    ' Description: Navigator SetKeys function.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeysFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys
    '
    ' Description: Navigator GetKeys function.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            vKeyArray = ""

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeysFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            vKeyArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
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

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummaryFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Start
    '
    ' Description: Navigator Start function. Entry point into Navigator.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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


            If Not Informations.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Informations.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Informations.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Informations.IsNothing(vTypeOfBusiness) Then

                m_sTypeOfBusiness.Value = CStr(vTypeOfBusiness)
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


    '****************************************************************** '
    '* Name: GetUserAuthorities (Public)
    '*
    '* Description: Get user authorities for the user
    '*
    '*
    '****************************************************************** '
    Public Function GetUserAuthorities() As Integer

        Dim result As Integer = 0
        Dim lNumberRecords As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        Dim sSQL As String = "SELECT has_unrestricted_enquiry,has_unrestricted_update FROM user_authorities WHERE user_id = {user_id}"
        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        ' Perform the query
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetEnquiryRestriction", bStoredProcedure:=False, lNumberRecords:=lNumberRecords)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
        End If


        Return result



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetUserAuthorities", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserAuthorities")

        Return result

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            ' Get Reference to Database

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Create PM Lookup Business Object
            m_oLookup = New BPMLOOKUP.Business()

            ' Initialise PM Lookup Business Object passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFOrion

            ' Currency Convert

            m_oCurrencyConvert = New bACTCurrencyConvert.Form
            m_lReturn = m_oCurrencyConvert.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Remove instance of Component Services
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bACTCurrencyConvert.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Period

            m_oPeriod = New bACTPeriod.Form
            m_lReturn = m_oPeriod.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Remove instance of Component Services
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bACTPeriod.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Account

            m_oAccount = New bACTAccount.Form
            m_lReturn = m_oAccount.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Remove instance of Component Services
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bACTAccount.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Get the Commission Transfer settings
            m_lReturn = GetOption(v_iOptionNumber:=16, r_sOptionValue:=m_sCommissionOption)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to read system option for Commission Option assuming Insurer Settled.", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                m_sCommissionOption = "2"
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.
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
                m_oLookup = Nothing
                If m_oCurrencyConvert IsNot Nothing Then
                    m_oCurrencyConvert.Dispose()
                    m_oCurrencyConvert = Nothing
                End If
                If m_oPeriod IsNot Nothing Then
                    m_oPeriod.Dispose()
                    m_oPeriod = Nothing
                End If
                If m_oAccount IsNot Nothing Then
                    m_oAccount.Dispose()
                    m_oAccount = Nothing
                End If
                If m_oMatchPost IsNot Nothing Then
                    m_oMatchPost.Dispose()
                    m_oMatchPost = Nothing
                End If
                If m_oAllocation IsNot Nothing Then
                    m_oAllocation.Dispose()
                    m_oAllocation = Nothing
                End If
                If m_oAllocationDetail IsNot Nothing Then
                    m_oAllocationDetail.Dispose()
                    m_oAllocationDetail = Nothing
                End If
                If m_oMatchGroup IsNot Nothing Then
                    m_oMatchGroup.Dispose()
                    m_oMatchGroup = Nothing
                End If
                If m_oTransmatch IsNot Nothing Then
                    m_oTransmatch.Dispose()
                    m_oTransmatch = Nothing
                End If

                If m_oTransDetail IsNot Nothing Then
                    m_oTransDetail.Dispose()
                    m_oTransDetail = Nothing
                End If

                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing


            End If
        End If
        Me.disposedValue = True
    End Sub



    ' *********************************************************************************************
    '
    ' Function: SelectTransQuery
    ' Purpose:  Performs the query using VB instead of above function which uses SQL.
    ' *********************************************************************************************
    Public Function SelectTransQuery(ByRef r_lNumberOfRecords As Integer, ByRef r_vResultArray(,) As Object, ByVal v_iCompanyID As Integer, Optional ByVal v_vAccountID As String = "", Optional ByVal v_vDocumentRef As Integer = 0, Optional ByVal v_vCurrencyID As Byte = 0, Optional ByVal v_vCurrencyAmount As Double = 0, Optional ByVal v_vTolerance As Byte = 0, Optional ByVal v_vDocTypeGroupId As String = "", Optional ByVal v_vDocumentTypeID As String = "", Optional ByVal v_vPeriodID As Object = Nothing, Optional ByVal v_vDateFrom As Byte = 0, Optional ByVal v_vDateTo As Byte = 0, Optional ByVal v_vInsuranceRef As String = "", Optional ByVal v_vUsername As Integer = 0, Optional ByVal v_vPurchaseInvoiceNo As Integer = 0, Optional ByVal v_vPurchaseOrderNo As Integer = 0, Optional ByVal v_vDepartment As Integer = 0, Optional ByVal v_vSpare As Integer = 0, Optional ByVal v_bMultiTreeAccounting As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim sSQL, sSQL1, sSQL2, sSQL3 As String
        Dim vFindTrans(,) As Object = Nothing
        Dim vMatchAmounts As Array
        Dim vMarkedAmounts As Array
        Dim vResultArray(,) As Object = Nothing

        Dim sPrefix As String = ""

        Dim lTdFindTrans As Integer
        Dim dtMatchDate As Date
        Dim cMatchCurrencyAmount, cMarkedCurrencyAmount As Decimal
        Dim iIndex As Integer

        Dim SSQLIndex As String = ""

        Dim bAddedDateFromParam, bAddedDateToParam As Boolean
        Dim sWhere As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            bAddedDateFromParam = False
            bAddedDateToParam = False

            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            sSQL1 = "SELECT DISTINCT 0 as marked_status,d.document_ref, d.document_id, " &
                    "t.document_sequence, tef.cover_start_date, p.period_name, " &
                    "t.currency_amount, d.documenttype_id, dt.doctypegroup_id, t.insurance_ref, " &
                    "pmu.username, " &
                    "t.purchase_order_no, t.purchase_invoice_no, t.department, t.spare, " &
                    "a.short_code, a.account_id, " &
                    "t.currency_id, t.transdetail_id, t.amount, td2.fully_matched, d.document_date, " &
                    "t.company_id,0 as match , 0 as match_date , 0 as marked_amount " &
                    "FROM TransDetail t " &
                    "INNER JOIN TransDetail td2 ON t.document_id = td2.document_id AND " &
                    "td2.document_sequence = 1 " &
                    "INNER JOIN Account a ON t.account_id = a.account_id " &
                    "INNER JOIN Document d ON t.document_id = d.document_id " &
                    "INNER JOIN Period p ON t.period_id = p.period_id " &
                    "INNER JOIN PMUser pmu ON t.operator_id = pmu.user_id " &
                    "INNER JOIN Transaction_Export_Folder tef ON tef.source_id " &
                    "= d.company_id " &
                    "AND tef.document_ref = d.document_ref "

            'DD 02/08/2002: Added filter for Multi-branch accounting
            sSQL2 = "INNER JOIN DocumentType dt ON d.DocumentType_id = dt.documenttype_id " &
                    "WHERE tef.accounts_export_status = 'c' "

            If v_bMultiTreeAccounting Then
                sSQL2 = sSQL2 & "AND t.company_id=" & CStr(m_iSourceID) & " "
            End If
            'eck PN 6155 180803 Put cint(v_vCurrencyID) outside the quotation marks

            If Not Informations.IsNothing(v_vCurrencyID) Then
                If v_vCurrencyID <> 0 Then
                    sSQL2 = sSQL2 & "AND t.currency_id = " & CStr(v_vCurrencyID)
                End If
            End If


            If Not Informations.IsNothing(v_vDocumentRef) Then

                If Marshal.SizeOf(v_vDocumentRef) > 0 Then
                    If (CStr(v_vDocumentRef).IndexOf("%"c) + 1) = 0 Then
                        sSQL2 = sSQL2 & "AND (d.document_ref = '" & CStr(v_vDocumentRef) & "') "
                    Else
                        sSQL2 = sSQL2 & "AND (d.document_ref LIKE '" & CStr(v_vDocumentRef) & "') "
                    End If
                End If
            End If


            If Not Informations.IsNothing(v_vDocumentTypeID) Then
                If CInt(v_vDocumentTypeID) > -1 Then
                    sSQL2 = sSQL2 & "AND (d.documenttype_id = '" & v_vDocumentTypeID & "') "
                End If
            End If


            If Not Informations.IsNothing(v_vDocTypeGroupId) Then
                If CInt(v_vDocTypeGroupId) > -1 Then
                    sSQL2 = sSQL2 & "AND (dt.doctypegroup_id = '" & v_vDocTypeGroupId & "') "
                End If
            End If


            If Not Informations.IsNothing(v_vUsername) Then

                If Marshal.SizeOf(v_vUsername) > 0 Then
                    If (CStr(v_vUsername).IndexOf("%"c) + 1) = 0 Then
                        sSQL2 = sSQL2 & "AND (pmu.username = '" & CStr(v_vUsername) & "') "
                    Else
                        sSQL2 = sSQL2 & "AND (pmu.username LIKE '" & CStr(v_vUsername) & "') "
                    End If
                End If
            End If


            If Not Informations.IsNothing(v_vPurchaseOrderNo) Then

                If Marshal.SizeOf(v_vPurchaseOrderNo) > 0 Then
                    If (CStr(v_vPurchaseOrderNo).IndexOf("%"c) + 1) = 0 Then
                        sSQL2 = sSQL2 & "AND (t.purchase_order_no = '" & CStr(v_vPurchaseOrderNo) & "') "
                    Else
                        sSQL2 = sSQL2 & "AND (t.purchase_order_no LIKE '" & CStr(v_vPurchaseOrderNo) & "') "
                    End If
                End If
            End If


            If Not Informations.IsNothing(v_vPurchaseInvoiceNo) Then

                If Marshal.SizeOf(v_vPurchaseInvoiceNo) > 0 Then
                    If (CStr(v_vPurchaseInvoiceNo).IndexOf("%"c) + 1) = 0 Then
                        sSQL2 = sSQL2 & "AND (t.purchase_invoice_no = '" & CStr(v_vPurchaseInvoiceNo) & "') "
                    Else
                        sSQL2 = sSQL2 & "AND (t.purchase_invoice_no LIKE '" & CStr(v_vPurchaseInvoiceNo) &
                                "') "
                    End If
                End If
            End If


            If Not Informations.IsNothing(v_vDepartment) Then

                If Marshal.SizeOf(v_vDepartment) > 0 Then
                    If (CStr(v_vDepartment).IndexOf("%"c) + 1) = 0 Then
                        sSQL2 = sSQL2 & "AND (t.department = '" & CStr(v_vDepartment) & "')"
                    Else
                        sSQL2 = sSQL2 & "AND (t.department LIKE '" & CStr(v_vDepartment) & "')"
                    End If
                End If
            End If


            If Not Informations.IsNothing(v_vSpare) Then

                If Marshal.SizeOf(v_vSpare) > 0 Then
                    If (CStr(v_vSpare).IndexOf("%"c) + 1) = 0 Then
                        sSQL2 = sSQL2 & "AND (t.spare = '" & CStr(v_vSpare) & "') "
                    Else
                        sSQL2 = sSQL2 & "AND (t.spare LIKE '" & CStr(v_vSpare) & "') "
                    End If
                End If
            End If


            If Not Informations.IsNothing(v_vPeriodID) Then

                If CInt(v_vPeriodID) > -1 Then

                    sSQL2 = sSQL2 & "AND (t.period_id = " & CStr(CInt(v_vPeriodID)) & ") "
                End If
            End If


            If Not Informations.IsNothing(v_vDateFrom) Then
                If v_vDateFrom > 0 Then
                    sSQL2 = sSQL2 & "AND (t.accounting_date >= {date_from}) "
                    ' Add the parameter
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="date_from", vValue:=CStr(v_vDateFrom), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add 'date_from' parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectTransQuery", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If
                    ' CTAF 110200
                    bAddedDateFromParam = True
                End If
            End If


            If Not Informations.IsNothing(v_vDateTo) Then
                If v_vDateTo > 0 Then
                    sSQL2 = sSQL2 & "AND (t.accounting_Date <= {date_to}) "
                    ' Add the paramter
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="date_to", vValue:=CStr(v_vDateTo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add 'date_to' parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectTransQuery", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If
                    ' CTAF 110200
                    bAddedDateToParam = True
                End If
            End If

            SSQLIndex = ""


            If Not Informations.IsNothing(v_vInsuranceRef) Then
                If v_vInsuranceRef.Length > 0 Then
                    Dim dbNumericTemp As Double
                    If Double.TryParse(v_vInsuranceRef, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                        SSQLIndex = SSQLIndex & "AND (t.insurance_ref_index = " & v_vInsuranceRef &
                                    ") "
                    Else
                        If (v_vInsuranceRef.IndexOf("%"c) + 1) = 0 Then
                            SSQLIndex = SSQLIndex & "AND (t.insurance_ref = '" & v_vInsuranceRef & "') "
                        Else
                            SSQLIndex = SSQLIndex & "AND (t.insurance_ref LIKE '" & v_vInsuranceRef & "') " &
                                        ""
                        End If
                    End If
                End If
            End If

            sSQL3 = ""


            If Not Informations.IsNothing(v_vAccountID) Then
                If Val(v_vAccountID) > 0 Then
                    sSQL3 = sSQL3 & "AND (t.account_id = " & v_vAccountID & ") "
                End If
            End If

            If Not Informations.IsNothing(v_vCurrencyID) Then
                If v_vCurrencyID <> 0 Then
                    sSQL3 = sSQL3 & "AND (t.currency_id = " & CStr(v_vCurrencyID) & ") "
                End If
            End If

            If (Not Informations.IsNothing(v_vCurrencyAmount)) And (Not Informations.IsNothing(v_vTolerance)) Then
                Select Case v_vCurrencyAmount
                    Case 0.01
                        sSQL3 = sSQL3 & "AND ((t.currency_amount >= " & CStr(v_vCurrencyAmount) & "))"
                    Case -0.01
                        sSQL3 = sSQL3 & "AND ((t.currency_amount <= " & CStr(v_vCurrencyAmount) & "))"
                    Case Is <> 0
                        If v_vTolerance > 0 Then
                            If v_vCurrencyAmount > 0 Then
                                sSQL3 = sSQL3 & "AND ((t.currency_amount >= " & CStr(v_vCurrencyAmount) & " - ((" &
                                        v_vCurrencyAmount & ") * " & CStr(v_vTolerance) & " / 100 )) " &
                                        "AND (t.currency_amount <= " & CStr(v_vCurrencyAmount) & " + ((" & CStr(v_vCurrencyAmount) & ") * " & CStr(v_vTolerance) & " / 100 ))) "
                            Else
                                sSQL3 = sSQL3 & "AND ((t.currency_amount <= " & CStr(v_vCurrencyAmount) & " - ((" &
                                        v_vCurrencyAmount & ") * " & CStr(v_vTolerance) & " / 100 )) " &
                                        "AND (t.currency_amount >= " & CStr(v_vCurrencyAmount) & " + ((" & CStr(v_vCurrencyAmount) & ") * " & CStr(v_vTolerance) & " / 100 ))) "
                            End If
                        Else
                            sSQL3 = sSQL3 & "AND (t.currency_amount  = " & CStr(v_vCurrencyAmount) & ")"
                        End If
                End Select
            End If


            sSQL3 = sSQL3 & " AND dt.from_sirius = 1 "

            ' Construct the SQL
            sSQL = sSQL1 & sSQL2 & SSQLIndex & sSQL3


            sSQL1 = "SELECT DISTINCT " &
                    "0 as marked_status, " &
                    "d.document_ref, " &
                    "d.document_id, t.document_sequence, " &
                    "d.document_date, p.period_name, " &
                    "t.currency_amount, d.documenttype_id, " &
                    "dt.doctypegroup_id, t.insurance_ref, " &
                    "pmu.username, t.purchase_order_no, " &
                    "t.purchase_invoice_no, t.department,t.spare, " &
                    "a.short_code, " &
                    "a.account_id, " &
                    "t.currency_id, " &
                    "t.transdetail_id, " &
                    "t.amount, " &
                    "td2.fully_matched, " &
                    "d.document_date, " &
                    "t.company_id," &
                    "0  as match, " &
                    "0  as match_date, " &
                    "0  as marked_amount " &
                    "FROM  TransDetail t INNER JOIN TransDetail td2 " &
                    "ON t.document_id = td2.document_id AND td2.document_sequence = 1 " &
                    "INNER JOIN Document d ON t.document_id = d.document_id "

            'KB 24062003 PN 4957 Sirius_for_Broking no longer exists!
            'sSQL1 = sSQL1 & "LEFT JOIN Sirius_For_Broking..Transaction_export_folder tef " & _
            '
            sSQL1 = sSQL1 & "LEFT JOIN Transaction_export_folder tef " &
                    "ON "
            sSQL1 = sSQL1 & "(tef.document_ref = d.document_ref AND tef.source_id = " &
                    "d.company_id) "
            sSQL1 = sSQL1 & "INNER JOIN Account a ON t.account_id = a.account_id "
            sSQL1 = sSQL1 & "INNER JOIN Period p ON t.period_id = p.period_id "
            sSQL1 = sSQL1 & "INNER JOIN DocumentType dt ON dt.documenttype_id = " &
                    "d.documenttype_id "
            sSQL1 = sSQL1 & "INNER JOIN PMUser pmu ON t.operator_id = pmu.user_id "

            'DD 02/08/2002: Added filter for Multi-branch accounting
            'eck PN4594 check multi company flag
            sSQL2 = "WHERE t.transdetail_id <> 0 "
            '            "AND t.company_id=" & m_iSourceID & " "
            If v_bMultiTreeAccounting Then
                sSQL2 = sSQL2 & "AND t.company_id=" & CStr(m_iSourceID) & " "
            End If


            If Not Informations.IsNothing(v_vDocumentRef) Then

                If Marshal.SizeOf(v_vDocumentRef) > 0 Then
                    If (CStr(v_vDocumentRef).IndexOf("%"c) + 1) = 0 Then
                        sSQL2 = sSQL2 & "AND (d.document_ref = '" & CStr(v_vDocumentRef) & "') "
                    Else
                        sSQL2 = sSQL2 & "AND (d.document_ref LIKE '" & CStr(v_vDocumentRef) & "') "
                    End If

                End If
            End If


            If Not Informations.IsNothing(v_vDocumentTypeID) Then
                If CInt(v_vDocumentTypeID) > -1 Then
                    sSQL2 = sSQL2 & "AND (d.documenttype_id = '" & v_vDocumentTypeID & "') "
                End If
            End If


            If Not Informations.IsNothing(v_vDocTypeGroupId) Then
                If CInt(v_vDocTypeGroupId) > -1 Then
                    sSQL2 = sSQL2 & "AND (dt.doctypegroup_id = '" & v_vDocTypeGroupId & "') "
                End If
            End If


            If Not Informations.IsNothing(v_vUsername) Then

                If Marshal.SizeOf(v_vUsername) > 0 Then
                    If (CStr(v_vUsername).IndexOf("%"c) + 1) = 0 Then
                        sSQL2 = sSQL2 & "AND (pmu.username = '" & CStr(v_vUsername) & "') "
                    Else
                        sSQL2 = sSQL2 & "AND (pmu.username LIKE '" & CStr(v_vUsername) & "') "
                    End If
                End If
            End If


            If Not Informations.IsNothing(v_vPurchaseOrderNo) Then

                If Marshal.SizeOf(v_vPurchaseOrderNo) > 0 Then
                    If (CStr(v_vPurchaseOrderNo).IndexOf("%"c) + 1) = 0 Then
                        sSQL2 = sSQL2 & "AND (t.purchase_order_no = '" & CStr(v_vPurchaseOrderNo) & "') "
                    Else
                        sSQL2 = sSQL2 & "AND (t.purchase_order_no LIKE '" & CStr(v_vPurchaseOrderNo) & "') "
                    End If
                End If
            End If


            If Not Informations.IsNothing(v_vPurchaseInvoiceNo) Then

                If Marshal.SizeOf(v_vPurchaseInvoiceNo) > 0 Then
                    If (CStr(v_vPurchaseInvoiceNo).IndexOf("%"c) + 1) = 0 Then
                        sSQL2 = sSQL2 & "AND (t.purchase_invoice_no = '" & CStr(v_vPurchaseInvoiceNo) & "') "
                    Else
                        sSQL2 = sSQL2 & "AND (t.purchase_invoice_no LIKE '" & CStr(v_vPurchaseInvoiceNo) &
                                "') "
                    End If
                End If
            End If


            If Not Informations.IsNothing(v_vDepartment) Then

                If Marshal.SizeOf(v_vDepartment) > 0 Then
                    If (CStr(v_vDepartment).IndexOf("%"c) + 1) = 0 Then
                        sSQL2 = sSQL2 & "AND (t.department = '" & CStr(v_vDepartment) & "')"
                    Else
                        sSQL2 = sSQL2 & "AND (t.department LIKE '" & CStr(v_vDepartment) & "')"
                    End If
                End If
            End If


            If Not Informations.IsNothing(v_vSpare) Then

                If Marshal.SizeOf(v_vSpare) > 0 Then
                    If (CStr(v_vSpare).IndexOf("%"c) + 1) = 0 Then
                        sSQL2 = sSQL2 & "AND (t.spare = '" & CStr(v_vSpare) & "') "
                    Else
                        sSQL2 = sSQL2 & "AND (t.spare LIKE '" & CStr(v_vSpare) & "') "
                    End If
                End If
            End If


            If Not Informations.IsNothing(v_vPeriodID) Then

                If CInt(v_vPeriodID) > -1 Then

                    sSQL2 = sSQL2 & "AND (t.period_id = " & CStr(CInt(v_vPeriodID)) & ") "
                End If
            End If


            If Not Informations.IsNothing(v_vDateFrom) Then
                If v_vDateFrom > 0 Then
                    sSQL2 = sSQL2 & "AND (t.accounting_date >= {date_from}) "

                    ' CTAF 021100
                    If Not bAddedDateFromParam Then

                        ' Add the parameter
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="date_from", vValue:=CStr(v_vDateFrom), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' Log Error Message
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add 'date_from' parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectTransQuery", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                            Return result
                        End If

                        bAddedDateFromParam = True

                    End If

                End If
            End If


            If Not Informations.IsNothing(v_vDateTo) Then
                If v_vDateTo > 0 Then
                    sSQL2 = sSQL2 & "AND (t.accounting_Date <= {date_to}) "

                    ' CTAF 021100
                    If Not bAddedDateToParam Then

                        ' Add the paramter
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="date_to", vValue:=CStr(v_vDateTo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' Log Error Message
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add 'date_to' parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectTransQuery", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                            Return result
                        End If

                        bAddedDateToParam = True

                    End If

                End If
            End If

            SSQLIndex = ""

            If Not Informations.IsNothing(v_vInsuranceRef) Then
                If v_vInsuranceRef.Length > 0 Then
                    Dim dbNumericTemp2 As Double
                    If Double.TryParse(v_vInsuranceRef, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                        SSQLIndex = SSQLIndex & "AND (t.insurance_ref_index = " & v_vInsuranceRef &
                                    ") "
                    Else
                        If (v_vInsuranceRef.IndexOf("%"c) + 1) = 0 Then
                            SSQLIndex = SSQLIndex & "AND (t.insurance_ref = '" & v_vInsuranceRef & "') "
                        Else
                            SSQLIndex = SSQLIndex & "AND (t.insurance_ref LIKE '" & v_vInsuranceRef & "') " &
                                        ""
                        End If
                    End If

                End If
            End If

            sSQL3 = ""


            If Not Informations.IsNothing(v_vAccountID) Then
                If Val(v_vAccountID) > 0 Then
                    sSQL3 = sSQL3 & "AND (t.account_id = " & v_vAccountID & ") "
                End If
            End If

            If Not Informations.IsNothing(v_vCurrencyID) Then
                If v_vCurrencyID <> 0 Then
                    sSQL3 = sSQL3 & "AND (t.currency_id = " & CStr(v_vCurrencyID) & ") "
                End If
            End If


            If (Not Informations.IsNothing(v_vCurrencyAmount)) And (Not Informations.IsNothing(v_vTolerance)) Then
                Select Case v_vCurrencyAmount
                    Case 0.01
                        sSQL3 = sSQL3 & "AND ((t.currency_amount >= " & CStr(v_vCurrencyAmount) & "))"
                    Case -0.01
                        sSQL3 = sSQL3 & "AND ((t.currency_amount <= " & CStr(v_vCurrencyAmount) & "))"
                    Case Is <> 0
                        If v_vTolerance > 0 Then
                            If v_vCurrencyAmount > 0 Then
                                sSQL3 = sSQL3 & "AND ((t.currency_amount >= " & CStr(v_vCurrencyAmount) & " - ((" &
                                        v_vCurrencyAmount & ") * " & CStr(v_vTolerance) & " / 100 )) " &
                                        "AND (t.currency_amount <= " & CStr(v_vCurrencyAmount) & " + ((" & CStr(v_vCurrencyAmount) & ") * " & CStr(v_vTolerance) & " / 100 ))) "
                            Else
                                sSQL3 = sSQL3 & "AND ((t.currency_amount <= " & CStr(v_vCurrencyAmount) & " - ((" &
                                        v_vCurrencyAmount & ") * " & CStr(v_vTolerance) & " / 100 )) " &
                                        "AND (t.currency_amount >= " & CStr(v_vCurrencyAmount) & " + ((" & CStr(v_vCurrencyAmount) & ") * " & CStr(v_vTolerance) & " / 100 ))) "
                            End If
                        Else
                            sSQL3 = sSQL3 & "AND (t.currency_amount  = " & CStr(v_vCurrencyAmount) & ")"
                        End If
                End Select
            End If

            sSQL3 = sSQL3 & " AND (dt.from_sirius = 0 "

            sSQL3 = sSQL3 & " OR t.spare = 'Reversal') "

            ' Construct the SQL
            sSQL = sSQL & " UNION " & sSQL1 & sSQL2 & SSQLIndex & sSQL3
            sSQL = sSQL & " ORDER BY t.company_id"



            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="FindTransaction", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vFindTrans)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed on m_oDatabase.SQLSelect", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectTransQuery", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            If Not Informations.IsArray(vFindTrans) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If
            'Get Matched Amounts

            vMatchAmounts = Array.CreateInstance(GetType(Object), New Integer() {4, vFindTrans.GetUpperBound(1) - vFindTrans.GetLowerBound(1) + 1}, New Integer() {0, vFindTrans.GetLowerBound(1)})


            For iLoop1 As Integer = vFindTrans.GetLowerBound(1) To vFindTrans.GetUpperBound(1)


                lTdFindTrans = CInt(vFindTrans(18, iLoop1))

                sSQL = "SELECT t.transdetail_id, sum(t.base_match_amount), " &
                       "sum(t.currency_match_amount) , " &
                       " (SELECT max(m.match_date) " &
                       "FROM Transmatch t, MatchGroup m " &
                       "WHERE  t.transdetail_id = " & CStr(lTdFindTrans) & " " &
                       "AND t.allocationdetail_id IS NOT NULL " &
                       "AND t.match_id = m.match_id) " &
                       "FROM TransMatch t " &
                       "WHERE t.transdetail_id  = " & CStr(lTdFindTrans) & " " &
                       "AND t.allocationdetail_id IS NOT NULL " &
                       "GROUP BY t.transdetail_id"

                ' #MatchAmounts
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="SelectMatchAmounts", bStoredProcedure:=False, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed on m_oDatabase.SQLSelect ' #MatchAmounts", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectTransQuery", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If
                End If

                If Not Informations.IsArray(vResultArray) Then
                    ' Not yet matched
                    vMatchAmounts(0, iLoop1) = lTdFindTrans
                    vMatchAmounts(1, iLoop1) = 0.0#
                    vMatchAmounts(2, iLoop1) = 0.0#
                    vMatchAmounts(3, iLoop1) = 0
                Else
                    ' Matched so store amounts

                    vMatchAmounts(0, iLoop1) = vResultArray(0, 0)

                    vMatchAmounts(1, iLoop1) = vResultArray(1, 0)

                    vMatchAmounts(2, iLoop1) = vResultArray(2, 0)

                    vMatchAmounts(3, iLoop1) = vResultArray(3, 0)
                End If

            Next iLoop1
            'Get Marked Amounts

            vMarkedAmounts = Array.CreateInstance(GetType(Object), New Integer() {3, vFindTrans.GetUpperBound(1) - vFindTrans.GetLowerBound(1) + 1}, New Integer() {0, vFindTrans.GetLowerBound(1)})


            For iLoop1 As Integer = vFindTrans.GetLowerBound(1) To vFindTrans.GetUpperBound(1)


                lTdFindTrans = CInt(vFindTrans(18, iLoop1))

                sSQL = "SELECT t.transdetail_id, sum(t.base_match_amount), " &
                       "sum(t.currency_match_amount) " &
                       "FROM TransMatch t " &
                       "WHERE t.transdetail_id  = " & CStr(lTdFindTrans) & " " &
                       "AND t.allocationdetail_id IS NULL " &
                       "GROUP BY t.transdetail_id"

                ' #MarkedAmounts
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="SelectMarkedAmounts", bStoredProcedure:=False, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed on m_oDatabase.SQLSelect ' #MarkedAmounts", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectTransQuery", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If
                End If

                If Not Informations.IsArray(vResultArray) Then
                    ' Not yet Marked
                    vMarkedAmounts(0, iLoop1) = lTdFindTrans
                    vMarkedAmounts(1, iLoop1) = 0
                    vMarkedAmounts(2, iLoop1) = 0
                Else
                    ' Markeded so store amounts

                    vMarkedAmounts(0, iLoop1) = vResultArray(0, 0)

                    vMarkedAmounts(1, iLoop1) = vResultArray(1, 0)

                    vMarkedAmounts(2, iLoop1) = vResultArray(2, 0)
                End If

            Next iLoop1

            'Combine Transactions with matched and marked amounts to give return array


            For iLoop1 As Integer = vFindTrans.GetLowerBound(1) To vFindTrans.GetUpperBound(1)


                lTdFindTrans = CInt(vFindTrans(18, iLoop1))

                'Get matched amount
                For iLoop2 As Integer = vMatchAmounts.GetLowerBound(1) To vMatchAmounts.GetUpperBound(1)
                    If CDbl(vMatchAmounts(0, iLoop1)) = lTdFindTrans Then
                        dtMatchDate = CDate(vMatchAmounts(3, iLoop1))
                        cMatchCurrencyAmount = CDec(vMatchAmounts(2, iLoop1))
                    End If
                Next iLoop2

                'Get marked amount
                For iLoop2 As Integer = vMarkedAmounts.GetLowerBound(1) To vMarkedAmounts.GetUpperBound(1)
                    If CDbl(vMarkedAmounts(0, iLoop1)) = lTdFindTrans Then
                        cMarkedCurrencyAmount = CDec(vMarkedAmounts(2, iLoop1))
                    End If
                Next iLoop2

                ' If matched amount is not equal to original amount - add to the list

                If cMatchCurrencyAmount <> CDec(vFindTrans(6, iLoop1)) Then

                    If Informations.IsArray(r_vResultArray) Then
                        iIndex = r_vResultArray.GetUpperBound(1) + 1
                        ReDim Preserve r_vResultArray(25, iIndex)
                    Else
                        iIndex = 0
                        ReDim r_vResultArray(25, iIndex)
                    End If

                    ' Marked Status

                    r_vResultArray(0, iIndex) = 0
                    ' document_ref


                    r_vResultArray(1, iIndex) = vFindTrans(1, iLoop1)
                    ' document-id


                    r_vResultArray(2, iIndex) = vFindTrans(2, iLoop1)
                    ' document seq


                    r_vResultArray(3, iIndex) = vFindTrans(3, iLoop1)
                    ' cover start date


                    r_vResultArray(4, iIndex) = CDate(vFindTrans(4, iLoop1))
                    ' period name


                    r_vResultArray(5, iIndex) = vFindTrans(5, iLoop1)
                    ' currency amount


                    r_vResultArray(6, iIndex) = CDec(vFindTrans(6, iLoop1))
                    ' document type id


                    r_vResultArray(7, iIndex) = vFindTrans(7, iLoop1)
                    ' documenttypegroup_id


                    r_vResultArray(8, iIndex) = vFindTrans(8, iLoop1)
                    ' insurance_ref


                    r_vResultArray(9, iIndex) = vFindTrans(9, iLoop1)
                    ' username


                    r_vResultArray(10, iIndex) = vFindTrans(10, iLoop1)
                    ' purchase_order_no


                    r_vResultArray(11, iIndex) = vFindTrans(11, iLoop1)
                    ' purchase_invoice_no


                    r_vResultArray(12, iIndex) = vFindTrans(12, iLoop1)
                    ' department


                    r_vResultArray(13, iIndex) = vFindTrans(13, iLoop1)
                    ' spare


                    r_vResultArray(14, iIndex) = vFindTrans(14, iLoop1)
                    ' short_code


                    r_vResultArray(15, iIndex) = vFindTrans(15, iLoop1)
                    ' account_id


                    r_vResultArray(16, iIndex) = CInt(vFindTrans(16, iLoop1))
                    ' currency_id


                    r_vResultArray(17, iIndex) = CInt(vFindTrans(17, iLoop1))
                    ' transdetail_id

                    r_vResultArray(18, iIndex) = lTdFindTrans
                    ' amount


                    r_vResultArray(19, iIndex) = CDec(vFindTrans(19, iLoop1))
                    ' Fully matched


                    r_vResultArray(20, iIndex) = vFindTrans(20, iLoop1)
                    ' document_date
                    'eck300502 use correct index
                    '           r_vResultArray(21, iIndex) = CDate(vFindTrans(22, iLoop1))


                    r_vResultArray(21, iIndex) = CDate(vFindTrans(21, iLoop1))
                    ' Company_id


                    r_vResultArray(22, iIndex) = vFindTrans(22, iLoop1)
                    ' Matched Amount

                    r_vResultArray(23, iIndex) = cMatchCurrencyAmount
                    ' Matched Date

                    r_vResultArray(24, iIndex) = dtMatchDate
                    ' Marked Amount

                    r_vResultArray(25, iIndex) = cMarkedCurrencyAmount
                    ' Update marked status
                    If cMarkedCurrencyAmount <> 0 Then

                        r_vResultArray(0, iIndex) = 1
                    End If
                End If

            Next iLoop1



            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed in SelectTransQuery", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectTransQuery", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: MarkTransaction
    '
    ' Description: Creates a fake TransMatch entry to say that it's
    '              ready to be paid. This'll get over-written when it
    '              is actually paid.
    '
    '
    ' See Also: UnMarkTransaction
    '
    ' ***************************************************************** '
    Public Function MarkTransaction(ByVal v_lTransactionId As Integer, ByVal v_iCurrencyID As Integer, ByVal v_lCompanyID As Integer, ByVal v_cPayment As Decimal) As Integer

        Dim result As Integer = 0
        Dim dtAccountingDate As Date
        Dim lMatchID, lAllocationID, lSubBranchID As Integer
        Dim cBaseAmount, cCurrencyAmount As Decimal
        Dim vdCurrencyBaseXRate As Byte

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oMatchPost Is Nothing Then

                m_oMatchPost = New bACTMatchPost.Form
                m_lReturn = m_oMatchPost.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Set the month to now for marking purposes
            dtAccountingDate = DateTime.Now

            ' Determine the correct SubBranch
            m_lReturn = bACTFunc.GetSubBranchID(v_oDatabase:=m_oDatabase, r_lSubBranchID:=lSubBranchID, v_vTransDetailID:=CStr(v_lTransactionId))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the match group

            m_lReturn = m_oMatchPost.AddMatchGroup(v_dtMatchDate:=dtAccountingDate, v_lSubBranchID:=lSubBranchID, r_vMatchId:=lMatchID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set up the data
            lAllocationID = 0
            vdCurrencyBaseXRate = 0 'This will get updated with the correct rate in the currency conversion
            cCurrencyAmount = v_cPayment


            m_lReturn = m_oCurrencyConvert.ConvertCurrencytoBase(lCurrencyID:=v_iCurrencyID, lCompanyID:=v_lCompanyID, cBaseAmount:=cBaseAmount, cCurrencyAmount:=cCurrencyAmount, vConversionDate:=DateTime.Today, vConversionRate:=vdCurrencyBaseXRate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFail
            End If

            ' Add the blank match

            m_lReturn = m_oMatchPost.AddMatchTrans(v_lAllocationdetailID:=lAllocationID, v_lTransDetailID:=v_lTransactionId, v_iCurrencyID:=v_iCurrencyID, v_cBaseMatchAmount:=cBaseAmount, v_cCurrencyMatchAmount:=cCurrencyAmount, v_vdCurrencyMatchXRate:=vdCurrencyBaseXRate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Write the match posts

            m_lReturn = m_oMatchPost.Commit()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Terminate

            m_oMatchPost.Dispose()
            m_oMatchPost = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MarkTransaction Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MarkTransaction", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UnMarkTransaction
    '
    ' Description: UnMarks a transaction.
    '
    '
    ' ***************************************************************** '
    Public Function UnMarkTransaction(ByVal v_lTransDetailId As Integer) As Integer

        'Dim sSQL As String                  ' RAW 13/01/2003 : PS187 : replaced by stored procedure
        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Construct the SQL
            ' RAW 13/01/2003 : PS187 : replaced with stored procedure
            '    ' Construct the SQL
            '    sSQL = "DELETE FROM TransMatch " & _
            ''            "WHERE transdetail_id = {transdetail_id} " & _
            ''            "AND allocationdetail_id IS null"
            ' RAW 13/01/2003 : PS187 : end

            ' Clear paramters
            m_oDatabase.Parameters.Clear()

            ' Add transdetail_id
            m_lReturn = m_oDatabase.Parameters.Add(sName:="transdetail_id", vValue:=CStr(v_lTransDetailId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lRecordsAffected = gPMConstants.PMAllRecords

            ' Perform Query
            ' RAW 13/01/2003 : PS187 : replaced hard-coded sql with stored procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUnMarkTransMatchSQL, sSQLName:=ACUnMarkTransMatchName, bStoredProcedure:=ACUnMarkTransMatchStored, lRecordsAffected:=lRecordsAffected)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return PMNotFound if no records were deleted
            If lRecordsAffected = 0 Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnMarkTransaction Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnMarkTransaction", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function AutoAllocate(ByVal v_lTransDetailId As Integer, ByRef r_sStatusCode As String, Optional ByVal v_lCashListItemID As Integer = 0, Optional ByVal v_lTransAccountID As Object = Nothing) As Integer
        Dim Catch_Renamed As Boolean = False
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: AutoAllocate
        ' PURPOSE: Perform an automatic allocation of transactions
        ' AUTHOR: Paul Cunnigham
        ' DATE: 22 November 2002, 11:06:56
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' Edit History :
        ' RAM20030311  : Added the v_lTransAccountID Optional Parameter. If passed in
        '                   then no need to fetch it, if not, then fetch it first
        '                 Ref. Issue 2911
        ' RAM20030314  : Set the r_sStatusCode to return the status of Allocation
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim oAccount As bACTAccount.Form

        Dim blWriteDetailFound As Boolean
        Dim lTransAccountId, lWriteOffAccountId As Integer
        Dim crNegativeAmount, crPositiveAmount, crBalance As Decimal

        Dim vTransactions(,) As Object = Nothing

        Const klColAmount As Integer = 1


        Try
            Catch_Renamed = True

            result = gPMConstants.PMEReturnCode.PMFalse
            r_sStatusCode = CStr(gACTLibrary.ACTAllocationStatusUnallocated) ' RAM20030314   : Return Status

            'Get the small amount write off
            m_lReturn = GetSmallAmountWriteOffDetails(v_lSourceID:=m_iSourceID, r_lAccountId:=lWriteOffAccountId, r_crNegativeAmount:=crNegativeAmount, r_crPositiveAmount:=crPositiveAmount)

            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMTrue
                    blWriteDetailFound = True
                Case gPMConstants.PMEReturnCode.PMNotFound
                    blWriteDetailFound = False
                Case Else
                    Return result
            End Select

            'Attempt to auto allocate based on the Policy Reference...

            'Get the transactions
            If GetTransFilterByPolicyRef(v_lTransDetailId:=v_lTransDetailId, r_vResultArray:=vTransactions) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return result
            End If

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'RAM20030311 :  Code Changes related to the Optional v_lTransAccountID
            '               Parameter.
            '               Ref. Issue 2911 - START
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            If Informations.IsNothing(v_lTransAccountID) Then

                'Get a reference to the tarans detail obejct
                If m_oTransDetail Is Nothing Then
                    If CreateTransDetailObject() <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return result
                    End If
                End If

                With m_oTransDetail

                    If .GetDetails(vTransdetailID:=v_lTransDetailId) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return result
                    End If


                    If .GetNext(vAccountID:=lTransAccountId) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return result
                    End If
                End With
            Else

                lTransAccountId = CInt(v_lTransAccountID)
            End If
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'RAM20030311 :  END
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            'Any transactions? - there should always be at least 1...
            If Informations.IsArray(vTransactions) Then
                '...but that will be for the transdetail_id and we need more to actually allocate on

                If vTransactions.GetUpperBound(1) > 0 Then
                    'Calculate the balance for these transactions

                    If CalculateTransactionArrayBalance(r_lAmountColumn:=klColAmount, r_vResultArray:=vTransactions, r_crBalance:=crBalance) <> gPMConstants.PMEReturnCode.PMTrue Then

                        Return result
                    End If

                    'Check to see if the balance is within the bounds of the write off amount
                    Select Case crBalance
                        Case crNegativeAmount To crPositiveAmount
                            'If there is a balance we need to write it off before we allocate
                            If blWriteDetailFound And crBalance <> 0 Then

                                If PostWriteOff(r_crAmount:=crBalance, r_lWriteOffAccountId:=lWriteOffAccountId, r_lClientAccountId:=lTransAccountId, r_vTransactions:=vTransactions) <> gPMConstants.PMEReturnCode.PMTrue Then

                                    Return result
                                End If
                            End If

                            'Pereform the auto allocation on the outstanding transactions

                            If PerformAutoAllocation(r_lAccountId:=lTransAccountId, r_lTransDetailId:=v_lTransDetailId, v_vOSTransactions:=vTransactions, v_lCashListItemID:=v_lCashListItemID) <> gPMConstants.PMEReturnCode.PMTrue Then

                                Return result
                            End If

                            'Allocation OK so exit with success
                            '18/03/2003 - Issue (ref:2752) - PWC - Also need to set the status here RAM
                            r_sStatusCode = CStr(gACTLibrary.ACTAllocationStatusAllocated)
                            Return gPMConstants.PMEReturnCode.PMTrue

                    End Select
                End If
            End If

            'Attempt to auto allocate based on the amount outstanding fo the account

            'Get the Transaction account balance
            oAccount = New bACTAccount.Form
            If oAccount.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return result
            End If


            If oAccount.GetAccountBalance(r_vdAccountBalance:=crBalance, v_vAccountID:=lTransAccountId) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return result
            End If

            'Check to see if the balance is within the bounds of the write off amount
            Select Case crBalance
                Case crNegativeAmount To crPositiveAmount
                    'If there is a balance we need to write it off before we allocate
                    If crBalance <> 0 And blWriteDetailFound Then

                        If PostWriteOff(r_crAmount:=crBalance, r_lWriteOffAccountId:=lWriteOffAccountId, r_lClientAccountId:=lTransAccountId, r_vTransactions:=vTransactions) <> gPMConstants.PMEReturnCode.PMTrue Then

                            Return result
                        End If
                    End If

                    'Get the outstanding transactions for the account

                    If oAccount.GetAccountOSTransactions(vAccount_id:=lTransAccountId, vOSTransactions:=vTransactions) <> gPMConstants.PMEReturnCode.PMTrue Then

                        Return result
                    End If


                    oAccount.Dispose()
                    oAccount = Nothing

                    'Pereform the auto allocation on the outstanding transactions

                    If PerformAutoAllocation(r_lAccountId:=lTransAccountId, r_lTransDetailId:=v_lTransDetailId, v_vOSTransactions:=vTransactions, v_lCashListItemID:=v_lCashListItemID) <> gPMConstants.PMEReturnCode.PMTrue Then

                        Return result
                    End If

                    'Allocation OK so exit with success
                    r_sStatusCode = CStr(gACTLibrary.ACTAllocationStatusAllocated) ' RAM20030314   : Return Status

                Case Else
                    ' Any error handling required ???

            End Select

            ' Everything OK
            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If

            GoTo Finally_Renamed

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------



            If Catch_Renamed Then

                Select Case Informations.Err().Number
                    Case Else
                        ' Log Error.
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoAllocate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                        result = gPMConstants.PMEReturnCode.PMError

                        GoTo Finally_Renamed
                End Select

            End If
Finally_Renamed:
        End Try
    End Function

    Private Function CalculateTransactionArrayBalance(ByRef r_lAmountColumn As Integer, ByRef r_vResultArray(,) As Object, ByRef r_crBalance As Decimal) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: CalculateTransactionArrayBalance
        ' PURPOSE: calcualtes a balance from an array of transactions
        ' AUTHOR: Paul Cunnigham
        ' DATE: 22 November 2002, 17:09:06
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim lLower, lUpper As Integer
        Const klRowDimension As Integer = 2



        result = gPMConstants.PMEReturnCode.PMFalse

        lLower = r_vResultArray.GetLowerBound(klRowDimension - 1)
        lUpper = r_vResultArray.GetUpperBound(klRowDimension - 1)

        For lRow As Integer = lLower To lUpper

            r_crBalance += CDbl(r_vResultArray(r_lAmountColumn, lRow))
        Next lRow

        result = gPMConstants.PMEReturnCode.PMTrue



        '----------------------------------------------------------------------------------------
        'Only for Debugging, the code will never execute this line
        '----------------------------------------------------------------------------------------



        Return result


    End Function

    Private Function PostWriteOff(ByRef r_crAmount As Decimal, ByRef r_lWriteOffAccountId As Integer, ByRef r_lClientAccountId As Integer, ByRef r_vTransactions As Array) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: PostWriteOff
        ' PURPOSE: Post a write off and balancing entry to the passed accounts
        '          and add the write off to the array of transactions to autoallocate
        ' AUTHOR: Paul Cunnigham
        ' DATE: 22 November 2002, 13:59:47
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim oDocumentPost As bACTDocumentPost.Form
        Dim oPMAutoNumber As bACTAutoNumber.Business

        Dim sGroupCode, sRangeCode, sDocumentRef As String
        Dim lDocumentId, lNumberRangeId, lNumber, lDocumentSequence As Integer
        Dim dtAccountingDate As Date
        Dim lTransDetailId As Integer

        Dim cBaseAmount, cCurrencyAmount As Decimal
        Dim lEuroCurrencyID As Integer
        Dim cEuroAmount As Decimal
        Dim vdEuroBaseXrate, vdEuroCcyXrate, vdCurrencyBaseXRate,  vdCurrencyAmountUnrounded As Object
        Dim vdBaseAmountUnrounded As Object = Nothing
        '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        Dim sReference As String = ""
        Dim lLowerRowId, lNewUpperRowId As Integer

        Const klRowDimension As Integer = 2 'Indicates the dimension in the array that relates to rows

        'Column indexes in the result array
        Const klACTransDetailId As Integer = 0
        Const klACAmount As Integer = 1
        Const klACCurrencyAmount As Integer = 2

        Try
            result = gPMConstants.PMEReturnCode.PMFalse

            sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef14
            sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSwd
            oPMAutoNumber = New bACTAutoNumber.Business
            If oPMAutoNumber.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return result
            End If

            With oPMAutoNumber

                If .GetNumberRange(v_sGroupCode:=sGroupCode, v_sRangeCode:=sRangeCode, r_lNumberRangeID:=lNumberRangeId) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Return result
                End If

                'Start-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                'Note:- GenerateNumber is related with  GenerateDocumentReferenceNumber

                If .GenerateDocumentReferenceNumber(v_lNumberRangeID:=lNumberRangeId, v_iUserID:=m_iUserID, v_iCompanyID:=m_iSourceID, r_sDocumentRef:=sReference, v_sRangeCode:=sRangeCode) <> gPMConstants.PMEReturnCode.PMTrue Then
                    'End-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                    Return result
                End If


                '.Terminate()
                .Dispose()
            End With
            oPMAutoNumber = Nothing
            oDocumentPost = New bACTDocumentPost.Form
            If oDocumentPost.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return result
            End If

            'Start-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            If sReference.Trim() <> "" Then
                sDocumentRef = sRangeCode & sReference
            End If
            'End-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            dtAccountingDate = DateTime.Now


            If oDocumentPost.AddDocument(v_lDocumentTypeId:=gACTLibrary.ACTDocTypeJournal, v_sDocumentRef:=sDocumentRef, v_dtDocumentDate:=dtAccountingDate, v_sComment:="Write Off", r_vDocumentID:=lDocumentId, r_vDocSourceID:=m_iSourceID) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return result
            End If

            'Get the base amount to post
            '(we pass in r_crAmount as currency amount and routine returns amount to post in cBaseAmount)
            cCurrencyAmount = r_crAmount


            If GetBaseAmountFromCurrency(v_iCurrencyID:=m_iCurrencyID, v_iCompanyID:=m_iSourceID, r_cBaseAmount:=cBaseAmount, v_cCurrencyAmount:=cCurrencyAmount, r_vdCurrencyBaseXRate:=vdCurrencyBaseXRate, v_dtAccountingDate:=dtAccountingDate, r_lEuro:=lEuroCurrencyID, r_cEuroAmount:=cEuroAmount, r_vEuroCCyXrate:=vdEuroCcyXrate, r_vEuroBaseXRate:=vdEuroBaseXrate, r_vCCyAmountUnrounded:=vdCurrencyAmountUnrounded, r_vBaseAmountUnrounded:=vdBaseAmountUnrounded) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return result
            End If

            'Post the inverse of the write off amount credit to the client account
            lDocumentSequence = 1

            If oDocumentPost.AddTransaction(v_lAccountID:=r_lClientAccountId, v_vDocumentSequence:=lDocumentSequence, v_iCurrencyID:=m_iCurrencyID, v_cAmount:=cBaseAmount * -1, v_cCurrencyAmount:=cCurrencyAmount * -1, v_vdCurrencyBaseXRate:=vdCurrencyBaseXRate, v_vComment:="", r_vTransDetailId:=lTransDetailId, v_vAccountingDate:=dtAccountingDate, v_vBaseAmountUnrounded:=vdBaseAmountUnrounded, v_vCurrencyAmountUnrounded:=vdCurrencyAmountUnrounded, v_vEuroCurrencyId:=lEuroCurrencyID, v_vEuroAmount:=cEuroAmount, v_vEuroBaseXRate:=vdEuroBaseXrate, v_vEuroCcyXrate:=vdEuroCcyXrate, v_vSpare:="") <> gPMConstants.PMEReturnCode.PMTrue Then

                Return result
            End If

            'Now add the Write off to the array so that it is allocated allong with the other transactions
            If Informations.IsArray(r_vTransactions) Then
                lLowerRowId = r_vTransactions.GetLowerBound(klRowDimension - 1)
                lNewUpperRowId = r_vTransactions.GetUpperBound(1) + 1
                r_vTransactions = ArraysHelper.RedimPreserve(Of Object(,))(r_vTransactions, New Integer() {klACCurrencyAmount - klACTransDetailId + 1, lNewUpperRowId - lLowerRowId + 1}, New Integer() {klACTransDetailId, lLowerRowId})


                r_vTransactions(klACTransDetailId, lNewUpperRowId) = lTransDetailId

                r_vTransactions(klACAmount, lNewUpperRowId) = cBaseAmount * -1

                r_vTransactions(klACCurrencyAmount, lNewUpperRowId) = cCurrencyAmount * -1
            End If

            'Post the matching writeoff to the write off account
            lDocumentSequence += 1

            If oDocumentPost.AddTransaction(v_lAccountID:=r_lWriteOffAccountId, v_vDocumentSequence:=lDocumentSequence, v_iCurrencyID:=m_iCurrencyID, v_cAmount:=cBaseAmount, v_cCurrencyAmount:=cCurrencyAmount, v_vdCurrencyBaseXRate:=vdCurrencyBaseXRate, v_vComment:="", r_vTransDetailId:=lTransDetailId, v_vAccountingDate:=dtAccountingDate, v_vBaseAmountUnrounded:=vdBaseAmountUnrounded, v_vCurrencyAmountUnrounded:=vdCurrencyAmountUnrounded, v_vEuroCurrencyId:=lEuroCurrencyID, v_vEuroAmount:=cEuroAmount, v_vEuroBaseXRate:=vdEuroBaseXrate, v_vEuroCcyXrate:=vdEuroCcyXrate, v_vSpare:="") <> gPMConstants.PMEReturnCode.PMTrue Then

                Return result
            End If


            If oDocumentPost.Commit() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = gPMConstants.PMEReturnCode.PMTrue


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="PostWriteOff", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError


            End Select
        Finally
            If Not (oDocumentPost Is Nothing) Then

                oDocumentPost.Dispose()
                oDocumentPost = Nothing
            End If
        End Try



        Return result

    End Function

    'sw 17/01/2003 Added in CashListItemID as optional param
    Public Function PerformAutoAllocation(ByRef r_lAccountId As Integer, ByRef r_lTransDetailId As Integer, ByVal v_vOSTransactions(,) As Object, Optional ByVal v_lCashListItemID As Integer = 0, Optional ByVal lWriteOffReasonID As Integer = 0, Optional ByVal cCurrencyWriteOff As Decimal = 0, Optional ByVal v_lCurExchangeRateGainLossReasonID As Integer = 0, Optional ByVal v_cCurGainLossAutoAllocationLimitAmount As Decimal = 0) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: PerformAutoAllocation
        ' PURPOSE: perform auto allocation on the passed transactions
        '          (copy / paste from iACTCashListItem.AutoAllocate)
        ' AUTHOR: Paul Cunnigham
        ' DATE: 21 November 2002, 15:50:55
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------
        Dim result As Integer = 0
        Dim oAllocationManual As bACTAllocationManual.Business = Nothing
        Dim vAllocationTrans As Object
        Dim cPayment As Decimal
        Dim lUpperRowBound As Integer

        Const lOldUpperOSColBound As Integer = 2

        'Declaring vKeyArray
        Dim vKeyArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'Get a reference to the tarans detail obejct
            If m_oTransDetail Is Nothing Then
                If CreateTransDetailObject() <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If
            End If

            'Get Transaction Amount
            With m_oTransDetail

                If .GetDetails(vTransdetailID:=r_lTransDetailId) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Return result
                End If


                If .GetNext(vTransdetailID:=r_lTransDetailId, vOSBaseAmount:=cPayment) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Return result
                End If

            End With

            'Use the bACTAllocationManual component to do the allocation
            oAllocationManual = New bACTAllocationManual.Business
            If oAllocationManual.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return result
            End If

            'Format the Outstanding transactions for use with Manual Allocation
            lUpperRowBound = 0
            For iCount As Integer = 0 To v_vOSTransactions.GetUpperBound(1)

                If Val(CStr(v_vOSTransactions(0, iCount))) <> r_lTransDetailId Then
                    If Not Informations.IsArray(vAllocationTrans) Then
                        ReDim vAllocationTrans(0)
                    Else

                        lUpperRowBound = vAllocationTrans.GetUpperBound(0) + 1
                        ReDim Preserve vAllocationTrans(lUpperRowBound)
                    End If


                    vAllocationTrans(lUpperRowBound) = v_vOSTransactions(0, iCount)


                    vAllocationTrans(lUpperRowBound) = CStr(vAllocationTrans(lUpperRowBound)) & "|"



                    vAllocationTrans(lUpperRowBound) = CStr(vAllocationTrans(lUpperRowBound)) & CStr(v_vOSTransactions(1, iCount))
                End If
            Next iCount

            'Set keys for the AllocationManual component
            If v_lCashListItemID <> 0 Then
                If (cCurrencyWriteOff <> 0) Or (v_cCurGainLossAutoAllocationLimitAmount <> 0) Then
                    ReDim vKeyArray(1, 6)
                Else
                    ReDim vKeyArray(1, 3)
                End If
            Else
                If (cCurrencyWriteOff <> 0) Or (v_cCurGainLossAutoAllocationLimitAmount <> 0) Then
                    ReDim vKeyArray(1, 5)
                Else
                    ReDim vKeyArray(1, 2)
                End If
            End If


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = r_lAccountId


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameTransDetailID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = CStr(r_lTransDetailId) & "|" & (CStr(cPayment - (cCurrencyWriteOff + v_cCurGainLossAutoAllocationLimitAmount)))


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameTransDetailIDs


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = vAllocationTrans

            If (cCurrencyWriteOff <> 0) Or (v_cCurGainLossAutoAllocationLimitAmount <> 0) Then
                ' Write Off Reason

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.ACTKeyNameWriteOffReasonId

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = lWriteOffReasonID

                'WriteOff difference

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.ACTKeyNameWriteOffAmount

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = cCurrencyWriteOff

                'Currency difference

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.ACTKeyNameCurrencyDifference

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = v_cCurGainLossAutoAllocationLimitAmount
            End If

            If v_lCashListItemID <> 0 Then

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, vKeyArray.GetUpperBound(1)) = PMNavKeyConst.ACTKeyNameCashListItemId

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, vKeyArray.GetUpperBound(1)) = v_lCashListItemID
            End If

            'Perform the allocation
            With oAllocationManual

                If .SetKeys(vKeyArray:=vKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Return result
                End If


                If .Start() <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If


                .Dispose()
            End With
            oAllocationManual = Nothing

            result = gPMConstants.PMEReturnCode.PMTrue


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------
            '        Resume

        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="PerformAutoAllocation", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

            End Select

        Finally

        End Try
        Return result

    End Function

    'EK 100100 Added extra passed parameters
    Private Function GetBaseAmountFromCurrency(ByVal v_iCurrencyID As Integer, ByVal v_iCompanyID As Integer, ByRef r_cBaseAmount As Decimal, ByVal v_cCurrencyAmount As Decimal, ByRef r_vdCurrencyBaseXRate As Object, ByVal v_dtAccountingDate As Date, ByRef r_lEuro As Integer, ByRef r_cEuroAmount As Decimal, ByRef r_vEuroCCyXrate As Object, ByRef r_vEuroBaseXRate As Object, ByRef r_vCCyAmountUnrounded As Object, ByRef r_vBaseAmountUnrounded As Object) As Integer

        Dim result As Integer = 0
        Dim oCurrencyConvert As bACTCurrencyConvert.Form
        oCurrencyConvert = New bACTCurrencyConvert.Form()

        result = gPMConstants.PMEReturnCode.PMFalse

        r_vdCurrencyBaseXRate = 0

        result = oCurrencyConvert.ConvertCurrencytoBase(lCurrencyID:=ToSafeInteger(v_iCurrencyID), lCompanyID:=ToSafeInteger(v_iCompanyID), cBaseAmount:=r_cBaseAmount, cCurrencyAmount:=ToSafeDecimal(v_cCurrencyAmount),
                                                  vConversionDate:=ToSafeDate(v_dtAccountingDate), vConversionRate:=r_vdCurrencyBaseXRate, vRounded:=True, lEuro:=r_lEuro,
                                                  cEuroAmount:=r_cEuroAmount, vEuroCCyXrate:=r_vEuroCCyXrate, vEuroBaseXRate:=r_vEuroBaseXRate,
                                                  vCCyAmountUnRounded:=r_vCCyAmountUnrounded, vBaseAmountUnRounded:=r_vBaseAmountUnrounded)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then

            Return result
        End If


        oCurrencyConvert.Dispose()
        oCurrencyConvert = Nothing


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    ' ***************************************************************** '
    '
    ' Name: Allocate
    '
    ' Description: Allocates transactions.
    '
    ' eck210302 Added Insurer Binder

    ' ***************************************************************** '
    Public Function Allocate(Optional ByRef v_bInsurerBinder As Object = Nothing, Optional ByRef v_lCompanyID As Integer = 0, Optional ByRef v_lAccountID As Object = Nothing, Optional ByRef v_CurrencyID As Object = Nothing, Optional ByRef v_iDocTypeGroup As Object = Nothing, Optional ByRef v_iDocumentTypeID As Object = Nothing, Optional ByRef v_lPeriodID As Object = Nothing, Optional ByRef v_dtDateFrom As Object = Nothing, Optional ByRef v_dtDateTo As Object = Nothing, Optional ByRef v_sOperatorName As Object = Nothing, Optional ByRef v_sDepartment As Object = Nothing, Optional ByRef v_bMultiTreeAccounting As Object = Nothing, Optional ByRef v_vTransIDs() As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lAllocation_id, lAllocationDetail_id As Integer

        Dim lPeriod_Id, lMatchGroup_id As Integer

        Dim vMarkedTransactions, vTransIDs(,) As Object

        Dim lDocumentType_Id As Integer
        Dim sDocument_Ref As String = ""
        Dim dtDocumentDate As Date
        Dim lDocumentSequence, lTransdetail_id As Integer
        Dim iCurrency_Id As Integer
        Dim cTransBaseAmount, cTransCcyAmount, cTransBaseAmountUnrounded As Decimal ' RAW 01/04/2003 : ISS2854 : added
        Dim cTransCcyAmountUnrounded As Decimal ' RAW 01/04/2003 : ISS2854 : added
        Dim dTransXrate As Double
        Dim cAllocBaseAmount, cAllocCcyAmount, cMarkedBaseAmount, cMarkedCcyAmount As Decimal
        Dim dMarkedXrate As Double
        Dim iFullyMatched As Integer
        Dim lMarkedTransmatch_id As Integer

        Dim lTransmatch_id As Integer
        Dim sAccountType As String = ""
        'eck210302
        Dim bInsurerBinder As Boolean

        'DD 05/08/2002: Multi-Branch
        Dim lSubBranchID As Integer

        Dim bFound As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If optional CompanyID not passed in then use property procedure variable if it has been set PN18528

            If Informations.IsNothing(v_lCompanyID) And m_iCompany_Id <> 0 Then
                v_lCompanyID = m_iCompany_Id
            End If

            'Create instances of extra business objects
            ' Allocation
            If m_oAllocation Is Nothing Then

                m_oAllocation = New bACTAllocation.Form
                m_lReturn = m_oAllocation.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Remove instance of Component Services
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bACTAllocation.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="Allocate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End If
            ' AllocationDetail
            If m_oAllocationDetail Is Nothing Then

                m_oAllocationDetail = New bACTAllocationdetail.Form
                m_lReturn = m_oAllocationDetail.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Remove instance of Component Services
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bACTAllocationDetail.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="Allocate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End If

            ' MatchGroup
            If m_oMatchGroup Is Nothing Then

                m_oMatchGroup = New bACTMatchgroup.Form
                m_lReturn = m_oMatchGroup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Remove instance of Component Services
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bACTMatchGroup.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="Allocate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End If

            ' TransMatch
            If m_oTransmatch Is Nothing Then

                m_oTransmatch = New bACTTransmatch.Form
                m_lReturn = m_oTransmatch.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Remove instance of Component Services
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bACTTransmatch.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="Allocate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End If
            'PN15896
            ' TransDetail
            If m_oTransDetail Is Nothing Then

                m_oTransDetail = New bACTTransdetail.Form
                m_lReturn = m_oTransDetail.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Remove instance of Component Services
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bACTTransdetail.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="Allocate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End If



            If m_sCommissionOption <> AsDebited And m_sCommissionOption <> WhenEffective Then
                'eck100203
                'FSA Phase 3.2 Remove withDID option
                '        If m_sCommissionOption = ClientPaymentincDID Then
                '            m_sCommissionOption = ClientPayment
                '            m_bWithDID = True
                '        End If
                m_lReturn = GetAccountType(v_lAccountID:=m_lAccount_Id, v_sAccountType:=sAccountType)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Commit Commission Posting.", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                End If
            End If
            'eck210302


            bInsurerBinder = Not ((Informations.IsNothing(v_bInsurerBinder)) Or (Object.Equals(v_bInsurerBinder, Nothing)))
            '
            'Do complete allocation

            m_lReturn = BeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception()
            End If

            'Get array of marked transactions

            m_lReturn = GetMarkedTransactions(v_vMarkedTransactions:=vMarkedTransactions, v_CompanyID:=v_lCompanyID, v_CurrencyID:=v_CurrencyID, v_DocTypeGroup:=v_iDocTypeGroup, v_DocumentTypeID:=v_iDocumentTypeID, v_PeriodID:=v_lPeriodID, v_DateFrom:=v_dtDateFrom, v_DateTo:=v_dtDateTo, v_OperatorName:=v_sOperatorName, v_Department:=v_sDepartment, v_MultiTreeAccounting:=CStr(v_bMultiTreeAccounting))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception()
            End If

            'DD 05/08/2002: Get SubBranch
            m_lReturn = bACTFunc.GetSubBranchID(v_oDatabase:=m_oDatabase, r_lSubBranchID:=lSubBranchID, v_vAccountID:=CStr(m_lAccount_Id))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception()
            End If

            'DD 05/08/2002: Call Period object

            m_lReturn = m_oPeriod.GetPeriodForDate(dtDateInPeriod:=DateTime.Now, lPeriodID:=lPeriod_Id, vSubBranchID:=lSubBranchID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception()
            End If

            'create allocation record

            m_lReturn = m_oAllocation.DirectAdd(vAllocationID:=lAllocation_id, vCompanyID:=m_iCompany_Id, vAccountID:=m_lAccount_Id, vUserID:=m_iUserID, vAllocationDate:=DateTime.Now, vAllocationstatusID:=3)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception()
            End If
            'create matchgroup record

            m_lReturn = m_oMatchGroup.DirectAdd(vMatchID:=lMatchGroup_id, vPeriodID:=lPeriod_Id, vCompanyID:=m_iCompany_Id, vMatchDate:=DateTime.Now)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception()
            End If


            For lRow As Integer = 0 To vMarkedTransactions.GetUpperBound(1)
                ' Peter Finney 14/11/2003 - Add transid filtering (if an array was passed)
                If Informations.IsArray(v_vTransIDs) Then
                    ' We have an array check our transaction is in it!

                    bFound = gPMFunctions.BinarySearch(CInt(vMarkedTransactions(4, lRow)), v_vTransIDs)
                Else
                    ' No array so assume this is okay!
                    bFound = True
                End If

                If bFound Then

                    lDocumentType_Id = CInt(vMarkedTransactions(0, lRow))

                    sDocument_Ref = CStr(vMarkedTransactions(1, lRow))

                    dtDocumentDate = CDate(vMarkedTransactions(2, lRow))

                    lDocumentSequence = CInt(vMarkedTransactions(3, lRow))

                    lTransdetail_id = CInt(vMarkedTransactions(4, lRow))

                    iCurrency_Id = CInt(vMarkedTransactions(5, lRow))

                    cTransBaseAmount = CDec(vMarkedTransactions(6, lRow))

                    cTransBaseAmountUnrounded = CDec(vMarkedTransactions(16, lRow)) ' RAW 01/04/2003 : ISS2854 : added

                    cTransCcyAmount = CDec(vMarkedTransactions(7, lRow))

                    cTransCcyAmountUnrounded = CDec(vMarkedTransactions(17, lRow)) ' RAW 01/04/2003 : ISS2854 : added

                    dTransXrate = CDec(vMarkedTransactions(8, lRow))

                    If (CStr(vMarkedTransactions(9, lRow))) = "" Then
                        cAllocBaseAmount = 0
                    Else

                        cAllocBaseAmount = CDec(vMarkedTransactions(9, lRow))
                    End If

                    If (CStr(vMarkedTransactions(10, lRow))) = "" Then
                        cAllocCcyAmount = 0
                    Else

                        cAllocCcyAmount = CDec(vMarkedTransactions(10, lRow))
                    End If

                    cMarkedBaseAmount = CDec(vMarkedTransactions(11, lRow))

                    cMarkedCcyAmount = CDec(vMarkedTransactions(12, lRow))

                    dMarkedXrate = CDec(vMarkedTransactions(13, lRow))

                    lMarkedTransmatch_id = CInt(vMarkedTransactions(14, lRow))
                    'eck210302 If we are running from Insurer Payments
                    'nothing allocated really means fully allocated
                    If bInsurerBinder Then
                        If cMarkedBaseAmount = 0 Then
                            cMarkedBaseAmount = cTransBaseAmount - cAllocBaseAmount
                            cMarkedCcyAmount = cTransCcyAmount - cAllocCcyAmount
                        End If
                    End If
                    'eck210302

                    'Write Allocationdetail
                    ' RAW 01/04/2003 : ISS2854 : added unrounded arguments
                    m_lReturn = WriteAllocationDetail(v_lAllocation_Id:=lAllocation_id, v_lDocumentType_Id:=lDocumentType_Id, v_sDocument_Ref:=sDocument_Ref, v_dtDocumentDate:=dtDocumentDate, v_lDocumentSequence:=lDocumentSequence, v_lTransdetail_id:=lTransdetail_id, v_iCurrency_Id:=iCurrency_Id, v_cTransBaseAmount:=cTransBaseAmount, v_cTransBaseAmountUnrounded:=cTransBaseAmountUnrounded, v_cTransCcyAmount:=cTransCcyAmount, v_cTransCcyAmountUnrounded:=cTransCcyAmountUnrounded, v_dTransXrate:=dTransXrate, v_cAllocBaseAmount:=cAllocBaseAmount, v_cAllocCcyAmount:=cAllocCcyAmount, v_iFullyMatched:=iFullyMatched, v_cMarkedBaseAmount:=cMarkedBaseAmount, v_cMarkedCcyAmount:=cMarkedCcyAmount, v_dMarkedXrate:=dMarkedXrate, v_lAllocationDetail_id:=lAllocationDetail_id, v_lCompanyID:=v_lCompanyID)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Throw New Exception()
                    End If

                    'Write Transmatch

                    m_lReturn = m_oTransmatch.DirectAdd(vTransmatchID:=lTransmatch_id, vAllocationdetailID:=lAllocationDetail_id, vTransdetailID:=lTransdetail_id, vMatchID:=lMatchGroup_id, vCurrencyID:=iCurrency_Id, vBaseMatchAmount:=cMarkedBaseAmount, vCurrencyMatchAmount:=cMarkedCcyAmount, vCurrencyMatchXrate:=dMarkedXrate)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Throw New Exception()
                    End If

                    'Delete Marker Transacmatch
                    m_lReturn = DeleteMarkedTransMatch(v_lMarkedTransmatch_Id:=lMarkedTransmatch_id)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Throw New Exception()
                    End If

                    'PN15896
                    ' get details for just the transdetail row concerned


                    m_lReturn = m_oTransDetail.GetDetails(vTransdetailID:=CInt(vMarkedTransactions(4, lRow)))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get transdetail details", vApp:=ACApp, vClass:=ACClass, vMethod:="Allocate")
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Throw New Exception()
                    End If


                    ' update the transdetail properties

                    m_lReturn = m_oTransDetail.EditUpdate(lRow:=1, vFullyMatched:=iFullyMatched)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update transdetail details", vApp:=ACApp, vClass:=ACClass, vMethod:="Allocate")
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Throw New Exception()
                    End If


                    ' save details to database

                    m_lReturn = m_oTransDetail.Update()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to save transdetail to database", vApp:=ACApp, vClass:=ACClass, vMethod:="Allocate")
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Throw New Exception()
                    End If
                    'PN15896End

                    If sAccountType = m_sCommissionOption Then
                        If Informations.IsArray(vTransIDs) Then

                            ReDim Preserve vTransIDs(0, vTransIDs.GetUpperBound(1) + 1)
                        Else
                            ReDim vTransIDs(0, 0)
                        End If


                        vTransIDs(0, vTransIDs.GetUpperBound(1)) = lTransdetail_id
                    End If
                End If 'bFound
            Next lRow

            'eck100502 Do we need to do a write off
            If m_lWriteOffReason_Id > 0 Then
                m_lReturn = WriteOff(v_lAllocationDetailId:=m_lWriteOfAllocationDetail_id, v_iCurrencyID:=m_iCurrency_Id, v_cBaseAmount:=m_cWriteOffBaseAmount * -1, v_cAmount:=m_cWriteOfAmount * -1, v_lWriteOffReasonID:=m_lWriteOffReason_Id)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Throw New Exception()
                End If

            End If
            'eck100502End


            If Informations.IsArray(vTransIDs) Then

                For lRow As Integer = vTransIDs.GetLowerBound(1) To vTransIDs.GetUpperBound(1)

                    m_lReturn = PostCommission(v_sCommissionOption:=sAccountType, v_iCompanyID:=m_iCompany_Id, v_lTransactionId:=CInt(vTransIDs(0, lRow)))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Throw New Exception()
                    End If
                Next lRow
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception()
            End If


            Return CommitTrans()

        Catch excep As System.Exception



            m_lReturn = RollbackTrans()

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Allocate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Allocate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'eck100502
    ' ******************************************************************** '
    ' Name: WriteOff
    '
    ' Description: Writes off the current allocation difference
    '
    '
    '
    ' ******************************************************************** '
    Private Function WriteOff(ByVal v_lAllocationDetailId As Integer, ByVal v_iCurrencyID As Integer, ByVal v_cBaseAmount As Decimal, ByVal v_cAmount As Decimal, ByVal v_lWriteOffReasonID As Integer) As Integer

        ' Variables

        Dim result As Integer = 0
        Dim lEuroCurrencyID As Integer

        ' Parameters
        Dim lWOAccountID, lDocumentId, lTransDetailId As Integer

        ' Transdetail Parameters
        Dim vCurrencyID As Integer
        Dim vPeriodID, lPeriodID, lLedgerID As Integer
        Dim cAmount As Decimal
        Dim vFullyMatched As Byte
        Dim vCurrencyAmount As Decimal
        Dim vCurrencyBaseXRate As Byte
        Dim vComment As String = ""
        Dim vInsuranceRef, vPurchaseOrderNo, vPurchaseInvoiceNo, vDepartment, vSpare As Object
        Dim vRefAmount As Byte
        Dim vRefQuantity As Byte
        Dim vRefUnits As Byte
        Dim vAccountingDate As Date
        Dim lWriteOffReasonID As Integer
        Dim vBaseAmountUnrounded As Decimal
        Dim vCurrencyAmountUnrounded As Decimal
        Dim lMatchID As Integer

        ' Document Parameters
        Dim vAuditSetID As Byte
        Dim vBatchID As Byte
        Dim vDocumentRef As String = ""
        Dim vAuthorisedDate As Date
        Dim vDocumentDate As Date
        Dim vCreatedDate As Date

        ' Auto numbering
        Dim lNumberRangeId As Integer
        '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        Dim sReference As String = ""

        ' Objects
        Dim oMatchPost As bACTMatchPost.Form
        Dim oDocument As bACTDocument.Form
        Dim oTransDetail As bACTTransdetail.Form
        Dim oAutoNumber As bACTAutoNumber.Business
        Dim oPeriod As bACTPeriod.Form

        Dim lSubBranchID As Integer
        Dim sLedgerTypeCode As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue


        If oDocument Is Nothing Then


            oDocument = New bACTDocument.Form
            m_lReturn = oDocument.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        ' Create TransDetail object
        If oTransDetail Is Nothing Then


            oTransDetail = New bACTTransdetail.Form
            m_lReturn = oTransDetail.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        ' Create AutoNumber object
        If oAutoNumber Is Nothing Then


            oAutoNumber = New bACTAutoNumber.Business
            m_lReturn = oAutoNumber.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        ' Create Period object
        If oPeriod Is Nothing Then


            oPeriod = New bACTPeriod.Form
            m_lReturn = oPeriod.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        ' Create Trans Match object
        If oMatchPost Is Nothing Then


            oMatchPost = New bACTMatchPost.Form
            m_lReturn = oMatchPost.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        ' Get the ledger for the account
        m_lReturn = GetLedgerForAccount(v_lAccountID:=m_lAccount_Id, r_lLedgerID:=lLedgerID, r_sLedgerTypeCode:=sLedgerTypeCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the write off account for this ledger
        m_lReturn = GetWriteOffAccount(v_sLedgerTypeCode:=sLedgerTypeCode, r_lWOAccountID:=lWOAccountID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the currency_id for the Euro
        m_lReturn = GetEuroCurrencyID(r_lCurrencyID:=lEuroCurrencyID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Get number for write off range
        ' Get the number range for documentref

        m_lReturn = oAutoNumber.GetNumberRange(v_sGroupCode:=gACTLibrary.ACTAutoNumberGroupCodeDocumentRef14, v_sRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeSwd, r_lNumberRangeID:=lNumberRangeId)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the next number
        'Start-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        'Note:- GenerateNumber is related with  GenerateDocumentReferenceNumber

        m_lReturn = oAutoNumber.GenerateDocumentReferenceNumber(v_lNumberRangeID:=lNumberRangeId, v_iUserID:=m_iUserID, v_iCompanyID:=m_iSourceID, v_sRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeSwd, r_sDocumentRef:=sReference)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'End-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        ' Generate a document
        lDocumentId = 0
        vAuditSetID = 0
        vAuthorisedDate = DateTime.Now
        vBatchID = 0
        vComment = "Write Off Document (Generated)"
        'Start(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        'vDocumentRef = Format$(lNumber&, "00000000")
        If sReference.Trim() <> "" Then
            vDocumentRef = gACTLibrary.ACTAutoNumberRangeCodeSwd & sReference
        End If
        'End(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        vDocumentDate = DateTime.Now
        vCreatedDate = DateTime.Now
        lWriteOffReasonID = v_lWriteOffReasonID


        m_lReturn = oPeriod.GetPostingPeriodForDate(dtDateInPeriod:=vDocumentDate, lPeriodID:=lPeriodID, lLedgerID:=lLedgerID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Re-assign the period_id
        vPeriodID = lPeriodID

        ' DD 05/08/2002: Get the Sub Branch
        m_lReturn = bACTFunc.GetSubBranchID(v_oDatabase:=m_oDatabase, r_lSubBranchID:=lSubBranchID, v_vPeriodID:=CStr(lPeriodID))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add a match group

        m_lReturn = oMatchPost.AddMatchGroup(v_dtMatchDate:=vDocumentDate, r_vMatchId:=lMatchID, v_lSubBranchID:=lSubBranchID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add it...

        m_lReturn = oDocument.DirectAdd(vDocumentID:=lDocumentId, vCompanyID:=m_iCompany_Id, vPostingstatusID:=gACTLibrary.ACTPostStatusPosted, vDocumenttypeID:=gACTLibrary.ACTDocTypeWriteOff, vAuditsetID:=vAuditSetID, vBatchID:=vBatchID, vDocumentRef:=vDocumentRef, vDocumentDate:=vDocumentDate, vCreatedDate:=vCreatedDate, vAuthorisedDate:=vAuthorisedDate, vComment:=vComment, vWriteOffReasonID:=lWriteOffReasonID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        vCurrencyID = v_iCurrencyID
        vAccountingDate = vDocumentDate
        cAmount = v_cBaseAmount
        vBaseAmountUnrounded = v_cBaseAmount
        vCurrencyAmountUnrounded = v_cAmount
        vFullyMatched = 1
        vCurrencyAmount = v_cAmount
        vCurrencyBaseXRate = 1
        vComment = "Write Off Transaction"
        vRefAmount = 0
        vRefQuantity = 0
        vRefUnits = 0

        ' Generate a transaction for the sales/purchase ledger

        m_lReturn = oTransDetail.DirectAdd(vTransdetailID:=lTransDetailId, vAccountID:=m_lAccount_Id, vPostingstatusID:=gACTLibrary.ACTPostStatusPosted, vCompanyID:=m_iCompany_Id, vCurrencyID:=vCurrencyID, vPeriodID:=vPeriodID, vDocumentID:=lDocumentId, vDocumentSequence:=1, vAccountingDate:=vAccountingDate, vAmount:=cAmount, vBaseAmountUnrounded:=vBaseAmountUnrounded, vFullyMatched:=vFullyMatched, vCurrencyAmount:=vCurrencyAmount, vCurrencyAmountUnrounded:=vCurrencyAmountUnrounded, vComment:=vComment, vInsuranceRef:=vInsuranceRef, vOperatorID:=m_iUserID, vPurchaseOrderNo:=vPurchaseOrderNo, vPurchaseInvoiceNo:=vPurchaseInvoiceNo, vDepartment:=vDepartment, vSpare:=vSpare, vRefQuantity:=vRefQuantity, vCurrencyBaseXrate:=vCurrencyBaseXRate, vAccountBaseDate:=vAccountingDate, vSystemBaseDate:=vAccountingDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add transaction.", vApp:=ACApp, vClass:=ACClass, vMethod:="WriteOff", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If


        m_lReturn = oMatchPost.AddMatchTrans(v_lAllocationdetailID:=v_lAllocationDetailId, v_lTransDetailID:=lTransDetailId, v_iCurrencyID:=vCurrencyID, v_cBaseMatchAmount:=cAmount, v_cCurrencyMatchAmount:=vCurrencyAmount, v_vdCurrencyMatchXRate:=vCurrencyBaseXRate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        ' Generate a transaction for the nominal ledger write off account
        cAmount = v_cBaseAmount * -1
        vCurrencyAmount = v_cAmount * -1
        vBaseAmountUnrounded = v_cBaseAmount * -1
        vCurrencyAmountUnrounded = v_cAmount * -1
        vComment = "Matching Write Off Transaction"
        'This side is not fully matched

        m_lReturn = oTransDetail.DirectAdd(vTransdetailID:=lTransDetailId, vAccountID:=lWOAccountID, vPostingstatusID:=gACTLibrary.ACTPostStatusPosted, vCompanyID:=m_iCompany_Id, vCurrencyID:=vCurrencyID, vPeriodID:=vPeriodID, vDocumentID:=lDocumentId, vDocumentSequence:=2, vAccountingDate:=vAccountingDate, vAmount:=cAmount, vBaseAmountUnrounded:=vBaseAmountUnrounded, vFullyMatched:=0, vCurrencyAmount:=vCurrencyAmount, vCurrencyAmountUnrounded:=vCurrencyAmountUnrounded, vComment:=vComment, vInsuranceRef:=vInsuranceRef, vOperatorID:=m_iUserID, vPurchaseOrderNo:=vPurchaseOrderNo, vPurchaseInvoiceNo:=vPurchaseInvoiceNo, vDepartment:=vDepartment, vSpare:=vSpare, vRefQuantity:=vRefQuantity, vCurrencyBaseXrate:=vCurrencyBaseXRate, vAccountBaseDate:=vAccountingDate, vSystemBaseDate:=vAccountingDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message

            'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add transaction.", vApp:=ACApp, vClass:=ACClass, vMethod:="WriteOff", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().mDescription)
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add transaction.", vApp:=ACApp, vClass:=ACClass, vMethod:="WriteOff", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' match the transaction

        ' Update the allocation detail table to show the write off

        m_lReturn = m_oAllocationDetail.SetWriteOff(v_lAllocationDetailID:=v_lAllocationDetailId, v_cWriteOffAmount:=cAmount, v_lWriteOffReasonID:=lWriteOffReasonID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Terminate the objects

        m_lReturn = oMatchPost.Commit()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        oMatchPost.Dispose()
        oMatchPost = Nothing


        oPeriod.Dispose()
        oPeriod = Nothing


        oAutoNumber.Dispose()
        oAutoNumber = Nothing


        oDocument.Dispose()
        oDocument = Nothing


        oTransDetail.Dispose()
        oTransDetail = Nothing


        Return result

    End Function


    ' ***************************************************************** '
    ' Name: GetLedgerForAccount
    '
    ' Description: Gets the ledger_id and ledger type code
    ' for the passed account.
    '
    ' DD 03/07/2003: Rewritten as it will not work in a
    ' multi-branch set-up where Ledgers are duplicated per sub-branch
    ' ***************************************************************** '
    Public Function GetLedgerForAccount(ByVal v_lAccountID As Integer, ByRef r_lLedgerID As Integer, ByRef r_sLedgerTypeCode As String) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("account_id", CStr(v_lAccountID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                'Developer Guide No 85
                '.Parameters.Add("ledger_id", CStr(DBNull.Value), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add("ledger_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

                'Developer Guide No 85
                '.Parameters.Add("ledgertype_code", CStr(DBNull.Value), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMString)
                .Parameters.Add("ledgertype_code", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMString)
                'Developer Guide No. 85
                'm_lReturn = .SQLSelect("{call spu_ACT_Get_LedgerType_Code (?,?,?)}", "Get Ledger Type Code for Account", True)
                m_lReturn = .SQLSelect("spu_ACT_Get_LedgerType_Code", "Get Ledger Type Code for Account", True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to call spu_ACT_Get_Ledger_Code", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLedgerForAccount", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                End If

                r_lLedgerID = CInt(.Parameters.Item("ledger_id").Value.Trim())
                r_sLedgerTypeCode = .Parameters.Item("ledgertype_code").Value.Trim()
            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLedgerForAccountFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLedgerForAccount", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: GetWriteOffAccount
    '
    ' Description: Gets the write off account_id depending on sales
    '              or purchase ledger.
    '
    ' DD 03/07/2003: Uses LedgerTypeCode instead of ID
    '
    ' ***************************************************************** '
    Private Function GetWriteOffAccount(ByVal v_sLedgerTypeCode As String, ByRef r_lWOAccountID As Integer) As Integer

        Dim result As Integer = 0
        Dim sShortCode As String = ""
        Dim oAllocation As bACTAllocation.Form = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue


        Select Case v_sLedgerTypeCode
            ' Sales ledger
            Case "D"

                m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=ACWriteOffDebtorAccount, r_sOptionValue:=sShortCode)

                ' Purchase ledger
            Case "C"

                m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=ACWriteOffCrebitorAccount, r_sOptionValue:=sShortCode)

                ' Another ledger
            Case Else

                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cannot write off on this ledger : " & v_sLedgerTypeCode, vApp:=ACApp, vClass:=ACClass, vMethod:="GetWriteOffAccount", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result

        End Select

        If oAllocation Is Nothing Then
            oAllocation = New bACTAllocation.Form
            m_lReturn = oAllocation.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        ' Get the account_id from the business

        m_lReturn = oAllocation.GetAccountID(v_sShortCode:=sShortCode, r_lAccountID:=r_lWOAccountID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        oAllocation.Dispose()
        oAllocation = Nothing

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: GetEuroCurrencyID
    '
    ' Description: Gets the currency id for the euro
    '
    ' ***************************************************************** '
    Private Function GetEuroCurrencyID(ByRef r_lCurrencyID As Integer) As Integer

        Dim result As Integer = 0
        Dim oCurrency As bACTCurrency.Form = Nothing

        Dim iCurrencyID As Integer

        Const EURO_ISO As String = "EUR"



        result = gPMConstants.PMEReturnCode.PMTrue


        If oCurrency Is Nothing Then

            oCurrency = New bACTCurrency.Form
            m_lReturn = oCurrency.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        ' Get the code for Euro's

        m_lReturn = oCurrency.GetCurrencyIdFromISO(v_sISOCode:=EURO_ISO, r_iCurrencyId:=iCurrencyID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Return it...
        r_lCurrencyID = iCurrencyID

        ' Terminate the object

        oCurrency.Dispose()
        oCurrency = Nothing

        Return result

    End Function

    'eck100502End


    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)
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
    ' Name: WriteAllocationDetail (Private)
    '
    ' Description: Write allocation detail
    '
    '  'PN15896 Pass back fully matched indicator
    ' ***************************************************************** '
    ' RAW 01/04/2003 : ISS2854 : added unrounded parameters
    Private Function WriteAllocationDetail(ByVal v_lAllocation_Id As Integer, ByVal v_lDocumentType_Id As Integer, ByVal v_sDocument_Ref As String, ByVal v_dtDocumentDate As Date, ByVal v_lDocumentSequence As Integer, ByVal v_lTransdetail_id As Integer, ByVal v_iCurrency_Id As Integer, ByVal v_cTransBaseAmount As Decimal, ByVal v_cTransBaseAmountUnrounded As Decimal, ByVal v_cTransCcyAmount As Decimal, ByVal v_cTransCcyAmountUnrounded As Decimal, ByVal v_dTransXrate As Double, ByVal v_cAllocBaseAmount As Decimal, ByVal v_cAllocCcyAmount As Decimal, ByRef v_iFullyMatched As Integer, ByVal v_cMarkedBaseAmount As Decimal, ByVal v_cMarkedCcyAmount As Decimal, ByVal v_dMarkedXrate As Double, ByRef v_lAllocationDetail_id As Integer, ByVal v_lCompanyID As Integer) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim cWriteOffBaseAmount As Decimal
        Dim iIsPrimary As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        If (v_cTransBaseAmount - (v_cAllocBaseAmount + v_cMarkedBaseAmount) = 0) Or (v_cTransCcyAmount - (v_cAllocCcyAmount + v_cMarkedCcyAmount) = 0) Then
            v_iFullyMatched = 1
        Else
            v_iFullyMatched = 0
        End If

        Dim cWriteOffAmount As Decimal = 0
        Dim lWriteOffReason_id As Integer = 0
        Dim cMarkedCcyAmount As Decimal = v_cMarkedCcyAmount
        Dim cMarkedBaseAmount As Decimal = v_cMarkedBaseAmount

        If v_lTransdetail_id = m_lWriteOffTransdetail_Id Then
            cWriteOffAmount = m_cWriteOffAmount
            lWriteOffReason_id = m_lWriteOffReason_Id


            m_lReturn = m_oCurrencyConvert.ConvertCurrencytoBase(lCurrencyID:=v_iCurrency_Id, lCompanyID:=v_lCompanyID, cBaseAmount:=cWriteOffBaseAmount, cCurrencyAmount:=cWriteOffAmount, vConversionDate:=DateTime.Today)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFail
            End If

            cMarkedCcyAmount -= cWriteOffAmount
            cMarkedBaseAmount -= cWriteOffBaseAmount

        End If

        If v_lDocumentSequence = 1 Then
            iIsPrimary = 1
        Else
            iIsPrimary = 0
        End If

        ' RAW 01/04/2003 : ISS2854 : added unrounded arguments and corrected orig and os amounts

        m_lReturn = m_oAllocationDetail.DirectAdd(vAllocationId:=v_lAllocation_Id, vAllocationDetailID:=v_lAllocationDetail_id, vOriginalCurrency:=v_iCurrency_Id, vTransdetailID:=v_lTransdetail_id, vDocumenttypeID:=v_lDocumentType_Id, vAccountingDate:=DateTime.Now, vDocumentRef:=v_sDocument_Ref, vOriginalDate:=v_dtDocumentDate, vAllocateToBase:=gPMConstants.PMEReturnCode.PMFalse, vOrigBaseAmount:=v_cTransBaseAmount, vOrigBaseAmountUnrounded:=v_cTransBaseAmountUnrounded, vOrigCcyAmount:=v_cTransCcyAmount, vOrigCcyAmountUnrounded:=v_cTransCcyAmountUnrounded, vOrigXrate:=v_dTransXrate, vEffectiveXrate:=v_dMarkedXrate, vOsBaseAmount:=v_cTransBaseAmount - v_cAllocBaseAmount, vOsCcyAmount:=v_cTransCcyAmount - v_cAllocBaseAmount, vAllocBaseAmount:=cMarkedBaseAmount, vAllocCcyAmount:=cMarkedCcyAmount, vFullyMatched:=v_iFullyMatched, vWriteOffReasonID:=lWriteOffReason_id, vWriteOffAmount:=cWriteOffBaseAmount, vNewOsCcyAmount:=v_cTransCcyAmount - (v_cAllocCcyAmount + (cMarkedCcyAmount + cWriteOffAmount)), vNewOsBaseAmount:=v_cTransBaseAmount - (v_cAllocBaseAmount + (cMarkedBaseAmount + cWriteOffBaseAmount)), vLossGainAmount:=0, vIsPrimary:=iIsPrimary)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFail
        Else


            'eck100502
            If v_lTransdetail_id = m_lWriteOffTransdetail_Id Then
                m_cWriteOffBaseAmount = cWriteOffBaseAmount
                m_cWriteOfAmount = cWriteOffAmount
                m_lWriteOfAllocationDetail_id = v_lAllocationDetail_id
            End If
            'eck100502End


            Return result

        End If


        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="WriteAllocationDetail Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="WriteAllocationDetail", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: DeleteMarkedTransMatch (Private)
    '
    ' Description: Write allocation detail
    '
    '
    ' ***************************************************************** '
    Private Function DeleteMarkedTransMatch(ByRef v_lMarkedTransmatch_Id As Integer) As Integer
        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()
            m_lReturn = .Parameters.Add(sName:="transmatch_id", vValue:=CStr(v_lMarkedTransmatch_Id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Perform the delete
            m_lReturn = .SQLAction(sSQL:=ACDeleteMarkedDetailsSQL, sSQLName:=ACDeleteMarkedDetailsName, bStoredProcedure:=ACDeleteMarkedDetailsStored, lRecordsAffected:=lRecordsAffected)


        End With

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        If lRecordsAffected = 0 Then
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result
    End Function
    ' ***************************************************************** '
    ' Name: GetOption (Private)
    '
    ' Description: Get an option.
    '
    ' ***************************************************************** '
    Private Function GetOption(ByVal v_iOptionNumber As Integer, ByRef r_sOptionValue As String, Optional ByRef vDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim bCloseDatabase As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        If m_oSystemOption Is Nothing Then

            ' Get Reference to Database

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_bNewInstanceCreated:=bCloseDatabase, r_oCheckedDatabase:=m_oS4BDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Get Option Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Get Instance of System Option Business

            m_oSystemOption = New bSIROptions.Business
            m_lReturn = m_oSystemOption.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oS4BDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


        End If



        m_lReturn = m_oSystemOption.GetOption(iOptionNumber:=v_iOptionNumber, sValue:=r_sOptionValue)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the option", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If

        m_oSystemOption.Dispose()



        m_oSystemOption = Nothing
        m_lReturn = m_oS4BDatabase.CloseDatabase()

        m_oS4BDatabase = Nothing

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: Get ledger for Transaction(Private)
    '
    ' Description: Gets Account Type Debit Transaction
    '
    ' ***************************************************************** '
    Private Function GetAccountType(ByVal v_lAccountID As Integer, ByRef v_sAccountType As String) As Integer

        Dim result As Integer = 0
        'Developer Guide No 112
        'Dim oFields As ADODB.Fields
        Dim oFields As DataRow
        Dim sSQL As String = ""
        Dim lRecordCount As Integer

        result = gPMConstants.PMEReturnCode.PMTrue


        'eck010801
        m_oDatabase.Parameters.Clear()
        '
        m_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=CStr(v_lAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        sSQL = ""
        sSQL = "SELECT L.ledger_name  " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "FROM ledger L, " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "account A " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "WHERE L.ledger_id = A.ledger_id " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "AND A.account_id = {account_id}"

        With m_oDatabase
            m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="SelectAccountLedgerName", bStoredProcedure:=False)


            ' Database error encountered

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            ' Set return value
            lRecordCount = .Records.Count()
            ' Record Count includes Doc and Transactions so we need at least 1 of each
            If lRecordCount <> 1 Then
                ' No enough rows retreived
                result = gPMConstants.PMEReturnCode.PMFalse
            Else
                ' Rows retrieved successfully
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            oFields = m_oDatabase.Records.Item(1).Fields()
            With oFields
                v_sAccountType = oFields("ledger_name").Trim()
            End With
            Select Case v_sAccountType
                Case "Client"
                    v_sAccountType = ClientPayment
                Case "Insurer"
                    v_sAccountType = InsurerSetted
                Case Else
                    v_sAccountType = ""
            End Select
        End With
        Return result

    End Function
    ' ***************************************************************** '
    ' Name: PostCommission (Private)
    '
    ' Description: Transfer Commission to Earned Account.
    '
    ' ***************************************************************** '
    Private Function PostCommission(ByVal v_sCommissionOption As String, ByVal v_iCompanyID As Integer, ByVal v_lTransactionId As Integer) As Integer

        Dim result As Integer = 0
        Dim oCommissionPost As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue
        If oCommissionPost Is Nothing Then
            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oCommissionPost, v_sClassName:="bACTCommissionPost.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase) 'eck 131103 PN8306

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        m_lReturn = oCommissionPost.PostCommission(v_sCommissionOption:=ToSafeString(v_sCommissionOption), v_iCompanyID:=ToSafeInteger(v_iCompanyID), v_lTransactionId:=ToSafeInteger(v_lTransactionId))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to post the commission", vApp:=ACApp, vClass:=ACClass, vMethod:="PostCommission", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If

        oCommissionPost.Dispose()
        oCommissionPost = Nothing

        Return result

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
    'Dim iCount2 As Integer
    'Dim iCount1 As Integer
    '
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
    ' Error Section.
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
        ' Error Section.
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
    '
    ' Name: GetSymbolForCurrency
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function GetSymbolForCurrency(ByVal v_iCurrencyID As Integer, ByRef r_sSymbol As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Construct the SQL
            sSQL = "SELECT symbol FROM Currency WHERE currency_id = " & v_iCurrencyID

            ' Perform the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetSymbolForCurrency", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the result
            If Informations.IsArray(vResultArray) Then

                r_sSymbol = CStr(vResultArray(0, 0)).Trim()
            Else
                ' Default to GBP
                r_sSymbol = "?"
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSymbolForCurrency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSymbolForCurrency", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetMarkedDetails
    '
    ' Description: Gets marked transactions from Transmatch table.
    '
    '
    ' ***************************************************************** '
    Public Function GetMarkedTransactions(ByRef v_vMarkedTransactions(,) As Object, Optional ByRef v_CompanyID As Object = Nothing, Optional ByRef v_CurrencyID As Object = Nothing, Optional ByRef v_DocTypeGroup As Object = Nothing, Optional ByRef v_DocumentTypeID As Object = Nothing, Optional ByRef v_PeriodID As Object = Nothing, Optional ByRef v_DateFrom As Object = Nothing, Optional ByRef v_DateTo As Object = Nothing, Optional ByRef v_OperatorName As Object = Nothing, Optional ByRef v_Department As Object = Nothing, Optional ByRef v_MultiTreeAccounting As String = "") As Integer

        Dim result As Integer = 0
        Dim vCompanyID, vCurrencyID, vDocTypeGroup, vDocumentTypeID, vPeriodID, vDateFrom, vDateTo, vOperatorName, vDepartment As Object
        Dim vMultiTreeAccounting As gPMConstants.PMEReturnCode
        'eck 131103 PN4594 Added IsMissing logic

        If Informations.IsNothing(v_CompanyID) Then


            vCompanyID = DBNull.Value
        Else

            If CInt(v_CompanyID) > 0 Then


                vCompanyID = v_CompanyID
            Else


                vCompanyID = DBNull.Value
            End If
        End If

        If Informations.IsNothing(v_CurrencyID) Then


            vCurrencyID = DBNull.Value
        Else

            If CInt(v_CurrencyID) > 0 Then


                vCurrencyID = v_CurrencyID
            Else


                vCurrencyID = DBNull.Value
            End If
        End If


        If Informations.IsNothing(v_DocTypeGroup) Then


            vDocTypeGroup = DBNull.Value
        Else

            If CInt(v_DocTypeGroup) > 0 Then


                vDocTypeGroup = v_DocTypeGroup
            Else


                vDocTypeGroup = DBNull.Value
            End If
        End If


        If Informations.IsNothing(v_DocumentTypeID) Then


            vDocumentTypeID = DBNull.Value
        Else

            If CInt(v_DocumentTypeID) > 0 Then


                vDocumentTypeID = v_DocumentTypeID
            Else


                vDocumentTypeID = DBNull.Value
            End If
        End If


        If Informations.IsNothing(v_PeriodID) Then


            vPeriodID = DBNull.Value
        Else

            If CInt(v_PeriodID) > 0 Then


                vPeriodID = v_PeriodID
            Else


                vPeriodID = DBNull.Value
            End If
        End If


        If Informations.IsNothing(v_DateFrom) Then


            vDateFrom = DBNull.Value
        Else

            If CDate(v_DateFrom) > #12/30/1899# Then


                vDateFrom = v_DateFrom
            Else


                vDateFrom = DBNull.Value
            End If
        End If


        If Informations.IsNothing(v_DateTo) Then


            vDateTo = DBNull.Value
        Else

            If CDate(v_DateTo) > #12/30/1899# Then


                vDateTo = v_DateTo
            Else


                vDateTo = DBNull.Value
            End If
        End If

        If Informations.IsNothing(v_OperatorName) Then


            vOperatorName = DBNull.Value
        Else

            If CStr(v_OperatorName).Length > 0 Then


                vOperatorName = v_OperatorName
            Else


                vOperatorName = DBNull.Value
            End If
        End If

        If Informations.IsNothing(v_Department) Then


            vDepartment = DBNull.Value
        Else

            If CStr(v_Department).Length > 0 Then


                vDepartment = v_Department
            Else


                vDepartment = DBNull.Value
            End If
        End If

        If Informations.IsNothing(v_MultiTreeAccounting) Then
            vMultiTreeAccounting = gPMConstants.PMEReturnCode.PMFalse
        ElseIf v_MultiTreeAccounting = "" Then
            vMultiTreeAccounting = gPMConstants.PMEReturnCode.PMFalse
        ElseIf CBool(v_MultiTreeAccounting) Then
            vMultiTreeAccounting = gPMConstants.PMEReturnCode.PMTrue
        Else
            vMultiTreeAccounting = gPMConstants.PMEReturnCode.PMFalse
        End If


        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            With m_oDatabase
                .Parameters.Clear()
                m_lReturn = .Parameters.Add(sName:="account_id", vValue:=CStr(m_lAccount_Id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = .Parameters.Add(sName:="company_id", vValue:=CStr(vCompanyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = .Parameters.Add(sName:="currency_id", vValue:=CStr(vCurrencyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = .Parameters.Add(sName:="DocTypeGroup", vValue:=CStr(vDocTypeGroup), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = .Parameters.Add(sName:="DocumentTypeId", vValue:=CStr(vDocumentTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = .Parameters.Add(sName:="Period_id", vValue:=CStr(vPeriodID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = .Parameters.Add(sName:="DateFrom", vValue:=CStr(vDateFrom), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)


                m_lReturn = .Parameters.Add(sName:="DateTo", vValue:=CStr(vDateTo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)


                m_lReturn = .Parameters.Add(sName:="OperatorName", vValue:=CStr(vOperatorName), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)


                m_lReturn = .Parameters.Add(sName:="Department", vValue:=CStr(vDepartment), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                m_lReturn = .Parameters.Add(sName:="Multitreeaccounting", vValue:=CStr(vMultiTreeAccounting), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                ' Perform the query
                m_lReturn = .SQLSelect(sSQL:=ACGetMarkedDetailsSQL, sSQLName:=ACGetMarkedDetailsName, bStoredProcedure:=ACGetMarkedDetailsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=v_vMarkedTransactions)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception()
                End If

                If Not Informations.IsArray(v_vMarkedTransactions) Then
                    result = gPMConstants.PMEReturnCode.PMNotFound
                End If


            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMarkedTransactions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMarkedTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetAccountDetails
    '
    ' Description: Pass the call through to the account object.
    '
    ' History: 29/10/1999 CTAF - Created.
    '
    ' eck180901 add extra return parameter for Account Currency
    '
    ' ***************************************************************** '
    Public Function GetAccountDetails(ByRef r_lAccountId As Integer, ByRef sAccountName As String, ByRef sContactName As String, ByRef sPhoneAreaCode As String, ByRef sPhoneNumber As String, ByRef sPhoneExtension As String, ByRef r_vdAccountBalance As Object, ByRef r_sAccountCode As String, ByRef r_vlAccountCurrencyId As Integer, Optional ByVal v_dtAccountingDate As Date = #12/30/1899#, Optional ByRef r_lCompanyID As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim lAccountStatusID As Integer
        Dim vAccountingDate As Date

        Try

            ' Get the details of the account
            sSQL = "SELECT account_name, contact_name, phone_area_code, phone_number, phone_extension, accountstatus_id, company_id " &
                   "FROM Account WHERE account_id = " & CStr(r_lAccountId)
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAccountDetails", bStoredProcedure:=False, vResultArray:=vResultArray)
            If (Not Informations.IsArray(vResultArray)) Or (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SQL Failed : " & sSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If


            sAccountName = CStr(vResultArray(0, 0))

            sContactName = CStr(vResultArray(1, 0))

            sPhoneAreaCode = CStr(vResultArray(2, 0))

            sPhoneNumber = CStr(vResultArray(3, 0))

            sPhoneExtension = CStr(vResultArray(4, 0))

            lAccountStatusID = CInt(vResultArray(5, 0))

            r_lCompanyID = CInt(vResultArray(6, 0))

            ' Get the status of the account
            sSQL = "SELECT code FROM AccountStatus where accountstatus_id = " & lAccountStatusID

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAccountStatus", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SQL Failed : " & sSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If


            r_sAccountCode = CStr(vResultArray(0, 0))

            If Not True Then
                vAccountingDate = DateTime.Today
            Else
                vAccountingDate = v_dtAccountingDate
            End If
            'eck180901 Return Currency Id

            m_lReturn = m_oAccount.GetAccountBalance(r_vdAccountBalance:=r_vdAccountBalance, v_vAccountID:=r_lAccountId, v_vAccountingDate:=vAccountingDate, r_vlAccountCurrencyId:=r_vlAccountCurrencyId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetAccountID
    '
    ' Description:
    '
    ' History: 05/01/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetAccountID(ByVal v_sShortCode As String, ByRef r_lAccountId As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT account_id FROM Account WHERE short_code = '" & v_sShortCode & "'"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAccountID", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountID", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then

            Else

                r_lAccountId = CInt(vResultArray(0, 0))
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: FormatCurrency
    '
    ' Description:  Pass the call through to the currency convert object.
    '
    ' ***************************************************************** '
    Public Function FormatCurrency(ByRef vCurrencyID As Object, ByRef vCurrencyAmount As Object, ByRef vFormattedCurrency As Object, ByRef vConversionDate As Object) As Integer

        Dim result As Integer = 0
        Try



            Return m_oCurrencyConvert.FormatCurrency(vCurrencyID:=vCurrencyID, vCurrencyAmount:=vCurrencyAmount, vFormattedCurrency:=vFormattedCurrency, vConversionDate:=vConversionDate)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormatCurrency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatCurrency", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetRegSettings
    '
    ' Description: Pass the call through to the solution config object.
    '
    '
    ' ***************************************************************** '
    Public Function GetRegSettings(ByRef sResult As String, ByRef sAppName As String, ByRef sSection As String, ByRef sKey As String, ByRef vDefault As Object) As Integer

        Dim result As Integer = 0
        Try

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRegSettings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRegSettings", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetSmallAmountWriteOffDetails(ByVal v_lSourceID As Integer, Optional ByRef r_lAccountId As Integer = 0, Optional ByRef r_crNegativeAmount As Decimal = 0, Optional ByRef r_crPositiveAmount As Decimal = 0) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetSmallAmountWriteOffDetails
        ' PURPOSE: Gets details for small write offs
        '          (this is currently only used for autoallocation and therefore
        '          gets a subset of the available columns - extend if required)
        ' AUTHOR: Paul Cunnigham
        ' DATE: 21 November 2002, 11:54:08
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' RAM20030314   : Type conversion changes from cCur ---> cLng
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        'Columns for result array
        Const kiColAccountId As Integer = 0
        Const kiColNegativeAmount As Integer = 1
        Const kiColPositiveAmount As Integer = 2

        Const klFirstRow As Integer = 0


        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the lSourceId INPUT parameter
            If m_oDatabase.Parameters.Add(sName:="lSourceId", vValue:=CStr(v_lSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return result
            End If

            'Execute SQL Statement
            If m_oDatabase.SQLSelect(sSQL:=ACGetSmallAmountWriteOffSQL, sSQLName:=ACGetSmallAmountWriteOffName, bStoredProcedure:=ACGetSmallAmountWriteOffStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return result
            End If

            'Did the call bring back data?
            If Not Informations.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
                Return result
            Else
                'Populate the params if the called requested them
                If Not False Then
                    ' RAM20030314 : Type Conversion from cCur ---> cLng

                    r_lAccountId = CInt(vResultArray(kiColAccountId, klFirstRow))
                End If
                If Not False Then

                    r_crNegativeAmount = -Math.Abs(CDec(vResultArray(kiColNegativeAmount, klFirstRow)))
                End If
                If Not False Then

                    r_crPositiveAmount = CDec(vResultArray(kiColPositiveAmount, klFirstRow))
                End If
            End If

            result = gPMConstants.PMEReturnCode.PMTrue


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------
            ' Resume

        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSmallAmountWriteOffDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

            End Select

        Finally



        End Try
        Return result
    End Function

    Private Function GetTransFilterByPolicyRef(ByVal v_lTransDetailId As Integer, ByRef r_vResultArray(,) As Object) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetTransFilterByPolicyRef
        ' PURPOSE: Gets transactions that relate to an insurance file reference
        ' AUTHOR: Paul Cunnigham
        ' DATE: 22 November 2002, 11:54:08
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMFalse

        'Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        'Add the lSourceId INPUT parameter
        If m_oDatabase.Parameters.Add(sName:="lTransDetailId", vValue:=CStr(v_lTransDetailId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

            Return result
        End If

        'Execute SQL Statement
        If m_oDatabase.SQLSelect(sSQL:=ACGetTransFilterByPolicyRefSQL, sSQLName:=ACGetTransFilterByPolicyRefName, bStoredProcedure:=ACGetTransFilterByPolicyRefStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then

            Return result
        End If

        result = gPMConstants.PMEReturnCode.PMTrue



        '----------------------------------------------------------------------------------------
        'Only for Debugging, the code will never execute this line
        '----------------------------------------------------------------------------------------



        Return result


    End Function

    Private Function CreateTransDetailObject() As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: CreateTransDetailObject
        ' PURPOSE: Creates a bACTTransDetail.Form object and sets the module level
        '          reference variable m_oTransDetail
        ' AUTHOR: Paul Cunningham
        ' DATE: 04 December 2002, 09:45:41
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMFalse

        'Use the bACTTransDetail component to get the transaction details
        m_oTransDetail = New bACTTransdetail.Form
        If m_oTransDetail.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

            Return result
        End If

        result = gPMConstants.PMEReturnCode.PMTrue



        '----------------------------------------------------------------------------------------
        'Only for Debugging, the code will never execute this line
        '----------------------------------------------------------------------------------------



        Return result


    End Function
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
