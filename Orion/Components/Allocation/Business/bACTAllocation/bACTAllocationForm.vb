Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Form_NET.Form")> _
Public NotInheritable Class Form
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 21/01/1998
    '
    ' Description: Creatable Form class which contains all the
    '              methods, Form rules required to manipulate
    '              a Allocation.
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

    ' Collection of Allocations (Private)
    Private m_oAllocations As bACTAllocation.Allocations

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

    Public ReadOnly Property Details() As Allocations
        Get
            Return m_oAllocations
        End Get
    End Property

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
                Case Is > m_oAllocations.Count()
                    m_lCurrentRecord = m_oAllocations.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Numner in Collection
            Return m_oAllocations.Count()

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

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
        End Get
    End Property

    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)
    'EK 230200
    ' ***************************************************************** '
    ' Name: GetCashItemSum
    '
    ' Description: Gets the unallocated amount left for the given cashlistitem
    '
    ' ***************************************************************** '
    Public Function GetCashItemSum(ByVal v_lCashListItemID As Integer, ByRef r_cCashItemSum As Decimal) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'sw 16/12/2003. What we actually want is the unallocated amount. sSQL changed accordingly

            sSQL = "select sum(amount) from "
            sSQL = sSQL & "(SELECT sum(amount) amount FROM cashlistitem WHERE cashlistitem_id = " & CStr(v_lCashListItemID)
            sSQL = sSQL & " Union "
            sSQL = sSQL & "select sum(alloc_base_amount) * -1 amount from allocationdetail where cashlistitem_id = " & CStr(v_lCashListItemID) & ") tmp"

            'end SW 16/12/2002

            ' Perform the query
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetCashItemSum", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCashItemSumFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCashItemSum", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse

            Else

                ' Return the sum

                r_cCashItemSum = CDec(vResultArray(0, 0))

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCashItemSumFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCashItemSum", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetCashListSum
    '
    ' Description: Gets the total amount for a cash list
    '
    ' ***************************************************************** '
    Public Function GetCashListSum(ByVal v_lCashListID As Integer, ByRef r_cCashListSum As Decimal) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Construct the SQL
            sSQL = "SELECT SUM(amount) FROM cashlistitem WHERE cashlist_id = " &
                   v_lCashListID

            ' Perform the query
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetCashListSum", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCashListSumFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCashListSum", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse

            Else

                ' Return the sum

                r_cCashListSum = CDec(vResultArray(0, 0))

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCashListSumFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCashListSum", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'EK 100100
    ' ***************************************************************** '
    ' Name: GetCashListCurrency
    '
    ' Description: Gets the total amount for a cash list
    '
    ' ***************************************************************** '
    Public Function GetCashListCurrency(ByVal v_lCashListID As Integer, ByRef r_lCashListCurrency As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Construct the SQL
            sSQL = "SELECT currency_id FROM cashlist WHERE cashlist_id = " &
                   v_lCashListID

            ' Perform the query
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetCashListCurrency", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCashListCurrencyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCashListCurrency", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse

            Else

                ' Return the sum

                r_lCashListCurrency = CInt(vResultArray(0, 0))

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCashListCurrencyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCashListCurrency", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAccountID
    '
    ' Description: Gets the account_id for an Account code
    '
    ' DD 05/08/2002: Added multi-branch to SQL
    ' Edit History  :
    ' RAM20030528   : Check for Product Option number 22
    '                 (Enable Multi-Branch Core Accounts)
    '
    ' ***************************************************************** '
    Public Function GetAccountID(ByVal v_sShortCode As String, ByRef r_lAccountID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim vValue As String = ""
        Dim bMultiBranchCoreAccountEnabled As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20030528 : Check whether Product Option 22 is set.
            '               (Enable Multi-Branch Core Accounts)
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            m_lReturn = CType(bPMFunc.getProductOptionValue(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiBranchCoreAccounts, v_vBranch:=m_iSourceID, r_vUnderwriting:=vValue), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bPMFunc.getProductOptionValue", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountID")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            bMultiBranchCoreAccountEnabled = ToSafeInteger(vValue) = 1

            If bMultiBranchCoreAccountEnabled Then
                ' Construct the SQL (filter by Branch/Source ID)
                sSQL = "SELECT account_id FROM account " &
                       "WHERE short_code = '" & v_sShortCode & "' AND " &
                       "company_id = " & CStr(m_iSourceID)
            Else
                ' Construct the SQL
                sSQL = "SELECT account_id FROM account " &
                       "WHERE short_code = '" & v_sShortCode & "'"
            End If

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20030528 : END
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If m_oDatabase Is Nothing Then
                m_lReturn = CType(gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error Opening Database", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseEngine", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                End If
            End If
            ' Peform the SQL Select statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAccountID", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Grab the return value...
            If Informations.IsArray(vResultArray) Then

                r_lAccountID = CInt(vResultArray(0, 0))
            Else
                ' ...or not
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ********************************************************************* '
    ' Name: MatchTransactions
    '
    ' Description: Sets Fully_Matched to 1 for the passed transdetail_ids
    '
    ' ********************************************************************* '
    Public Function MatchTransactions(ByVal v_vTransactions() As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' loop round the array
            For iLoop1 As Integer = v_vTransactions.GetLowerBound(0) To v_vTransactions.GetUpperBound(0)

                ' construct the sql to match the transaction

                sSQL = "UPDATE TransDetail SET fully_matched = 1 WHERE " &
                       "transdetail_id = " & CStr(v_vTransactions(iLoop1))

                ' perform the sql
                m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="MatchTransactions", bStoredProcedure:=False)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MatchTransactionsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="MatchTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer


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

            'Set m_oDatabase = GetOrionDatabase(m_lReturn, m_bCloseDatabase, vDatabase)



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

            ' Create Allocations Collection
            m_oAllocations = New bACTAllocation.Allocations()

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
                m_oAllocations = Nothing
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

    ''' <summary>
    ''' Check if transaction is linked to Instalment with a Third Party Scheme
    ''' </summary>
    ''' <param name="DocumentRef"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsLinkedToThirdPartyScheme(ByVal DocumentRef As String) As Boolean

        Dim result As Boolean = False

        Try
            result = False

            Dim SQL As String
            Dim vResultArray As Object = Nothing

            SQL = " SELECT distinct 1 FROM PFScheme PFS "
            SQL = SQL & " Join PFPremiumFinance PF ON PF.SchemeNo=PFS.SchemeNo and PF.SchemeVersion=PFS.SchemeVersion"
            SQL = SQL & " Join TransDetail TD ON (PF.PlanTransaction_id=TD.transdetail_id )"
            SQL = SQL & " Join Document DC On TD.document_id=DC.document_id"
            SQL = SQL & " WHERE  PFS.pfscheme_type_id=1 and DC.document_ref='" & DocumentRef & "'"

            m_lReturn = m_oDatabase.SQLSelect(
                sSQL:=SQL,
                sSQLName:="IsLinkedToThirdPartyScheme",
                bStoredProcedure:=False,
                vResultArray:=vResultArray)

            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                Return result
            End If

            If Informations.IsArray(vResultArray) Then
                If vResultArray(0, 0) = 1 Then
                    result = True
                End If
            End If

            Return result

        Catch

            ' Error Section.

            result = False

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsLinkedToThirdPartyScheme Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsLinkedToThirdPartyScheme", vErrNo:=Informations.Err.Number, vErrDesc:="")

            Return result
        End Try
    End Function

    'PM 019290
    Public Function ThirdPartyScheme(ByVal AccountId As Long) As Boolean

        Dim result As Integer = 0

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            Dim SQL As String
            Dim vResultArray As Object = Nothing

            SQL = "select pfscheme_type_id from Account A "
            SQL = SQL & "inner join party P on A.short_code = P.shortname "
            SQL = SQL & "inner join pfscheme PF on PF.party_cnt =  P.party_cnt "
            SQL = SQL & "where A.account_id = " & AccountId

            m_lReturn = m_oDatabase.SQLSelect(
                sSQL:=SQL$,
                sSQLName:="ThirdPartyScheme",
                bStoredProcedure:=False,
                vResultArray:=vResultArray)

            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                result = PMEReturnCode.PMFalse
                Return result
            End If

            If Informations.IsArray(vResultArray) Then
                If vResultArray(0, 0) = 1 Then
                    result = True
                Else
                    result = False
                End If
            End If

            Return result

        Catch ex As Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ThirdPartyScheme Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ThirdPartyScheme", vErrNo:=Informations.Err.Number, vErrDesc:=ex.Message, excep:=ex)

            Return result
        End Try
    End Function

    ''' <summary>
    '''  DirectAdd
    ''' </summary>
    ''' <param name="vAllocationID"></param>
    ''' <param name="vCompanyID"></param>
    ''' <param name="vAccountID"></param>
    ''' <param name="vUserID"></param>
    ''' <param name="vAllocationDate"></param>
    ''' <param name="vAllocationstatusID"></param>
    ''' <param name="r_nAllocationBatchID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DirectAdd(Optional ByRef vAllocationID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing,
                              Optional ByRef vAccountID As Object = Nothing, Optional ByRef vUserID As Object = Nothing,
                              Optional ByRef vAllocationDate As Object = Nothing, Optional ByRef vAllocationstatusID As Object = Nothing,
                              Optional ByRef r_nAllocationBatchID As Integer = 0) As Integer

        Dim nResult As Integer
        Dim oAllocation As bACTAllocation.Allocation

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Allocation
            oAllocation = New bACTAllocation.Allocation()

            ' Populate Allocation Attributes
            m_lReturn = SetProperties(oAllocation, gPMConstants.PMEComponentAction.PMAdd,
            vAllocationID:=vAllocationID,
            vCompanyID:=vCompanyID,
            vAccountID:=vAccountID,
            vUserID:=vUserID,
            vAllocationDate:=vAllocationDate,
            vAllocationstatusID:=vAllocationstatusID,
            r_nAllocationBatchID:=r_nAllocationBatchID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Allocation to the Database
            m_lReturn = CType(AddItem(oAllocation), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the ID of the Allocation Added

            If Not Informations.IsNothing(vAllocationID) Then
                vAllocationID = oAllocation.AllocationID
            End If

            oAllocation = Nothing

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a single Allocation directly from the database.
    '        Note: The Allocation will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByRef vAllocationID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vUserID As Object = Nothing, Optional ByRef vAllocationDate As Object = Nothing, Optional ByRef vAllocationstatusID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oAllocation As bACTAllocation.Allocation

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Allocation
            oAllocation = New bACTAllocation.Allocation()

            ' Populate Allocation Attributes

            m_lReturn = CType(SetProperties(oAllocation, gPMConstants.PMEComponentAction.PMDelete, vAllocationID:=vAllocationID, vCompanyID:=vCompanyID, vAccountID:=vAccountID, vUserID:=vUserID, vAllocationDate:=vAllocationDate, vAllocationstatusID:=vAllocationstatusID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Allocation to the Database
            m_lReturn = CType(DeleteItem(oAllocation), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oAllocation = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the Allocation.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vAllocationID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vUserID As Object = Nothing, Optional ByRef vAllocationDate As Object = Nothing, Optional ByRef vAllocationstatusID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults

            m_lReturn = CType(DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vAllocationID:=vAllocationID, vCompanyID:=vCompanyID, vAccountID:=vAccountID, vUserID:=vUserID, vAllocationDate:=vAllocationDate, vAllocationstatusID:=vAllocationstatusID), gPMConstants.PMEReturnCode)

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

            'Developer Guide No 98
            m_lReturn = m_oDatabase.Parameters.Add(sName:="id", vValue:=vID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)


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
        'developer guide no 112. 
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
            '    If (Trim$(vTable) <> PMTableAllocation) Then

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

                    'AK 230702 - check for null value

                    If Not (Convert.IsDBNull(oFields(vFieldArray(lSub))) Or Informations.IsNothing(oFields(vFieldArray(lSub)))) Then
                        ' Store the results in the Temporary results array

                        vResults(lSub) = oFields(vFieldArray(lSub))
                    Else
                        Select Case oFields(vFieldArray(lSub)).Type
                            ' to do list
                            'Case DbType.String, DbType.String, DbType.String,  adLongVarWChar, DbType.String, DbType.String, adWChar
                            Case DbType.String, ADODB.DataTypeEnum.adLongVarWChar, ADODB.DataTypeEnum.adWChar

                                vResults(lSub) = ""
                                'to do list
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCaptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required Allocations and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vAllocationID As Object = Nothing, Optional ByRef vLockMode As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oAllocation As bACTAllocation.Allocation

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oAllocations.Clear()

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

            If Not Informations.IsNothing(vAllocationID) Then

                ' Yes, Is the key valid

                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(CStr(vAllocationID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vAllocationID =" & CStr(vAllocationID), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                    Return result

                End If

                ' Add the AllocationID parameter (INPUT)

                'Developer Guide No 98
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Allocation_id", vValue:=vAllocationID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                'Tomo31012002 - Replaced PMInteger with PMLong

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
                'developer guide no.162
                For lSub As Integer = 0 To lRecordCount - 1

                    ' Create New Allocation
                    oAllocation = New bACTAllocation.Allocation()

                    m_lReturn = CType(SetPropertiesFromDB(oAllocation:=oAllocation, lRecordNumber:=lSub), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    ' Add Allocation to collection
                    If (m_oAllocations.Count = 0) Then
                        m_oAllocations.Add(Nothing)
                    End If
                    m_lReturn = CType(m_oAllocations.Add(oNewAllocation:=oAllocation), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oAllocation = Nothing

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
    ' Description: Gets the required Allocations and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vAllocationID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vUserID As Object = Nothing, Optional ByRef vAllocationDate As Object = Nothing, Optional ByRef vAllocationstatusID As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oAllocation As bACTAllocation.Allocation
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oAllocations.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oAllocation = m_oAllocations.Item(m_lCurrentRecord)

            ' Get the Allocation Property Values





            'developer guide no.98
            m_lReturn = GetProperties(oAllocation, iStatus, vAllocationID:=vAllocationID, vCompanyID:=vCompanyID, vAccountID:=vAccountID, vUserID:=vUserID, vAllocationDate:=vAllocationDate, vAllocationstatusID:=vAllocationstatusID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oAllocation = Nothing


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
    ' Description: Adds the supplied Allocation into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vAllocationID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vUserID As Object = Nothing, Optional ByRef vAllocationDate As Object = Nothing, Optional ByRef vAllocationstatusID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oAllocation As bACTAllocation.Allocation

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oAllocations.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new Allocation
            oAllocation = New bACTAllocation.Allocation()

            ' Populate Allocation Attributes





            'developer guide no.98
            m_lReturn = SetProperties(oAllocation, gPMConstants.PMEComponentAction.PMAdd, vAllocationID:=vAllocationID, vCompanyID:=vCompanyID, vAccountID:=vAccountID, vUserID:=vUserID, vAllocationDate:=vAllocationDate, vAllocationstatusID:=vAllocationstatusID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oAllocation = Nothing
                Return result
            End If

            ' Add Allocation to collection
            If (m_oAllocations.Count = 0) Then
                m_oAllocations.Add(Nothing)
            End If
            m_lReturn = CType(m_oAllocations.Add(oNewAllocation:=oAllocation), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oAllocation = Nothing
                Return result
            End If

            oAllocation = Nothing

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
    ' Description: Validates that this action is valid on the Allocation
    '              specified and updates the Allocation with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vAllocationID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vUserID As Object = Nothing, Optional ByRef vAllocationDate As Object = Nothing, Optional ByRef vAllocationstatusID As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oAllocation As bACTAllocation.Allocation
        Dim iStatus As gPMConstants.PMEComponentAction

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oAllocations.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            'oAllocation = m_oAllocations.Item(lRow)
            oAllocation = New bACTAllocation.Allocation
            oAllocation.AccountID = CInt(m_oAllocations.Item(lRow).AccountID)
            oAllocation.AllocationDate = CDate(m_oAllocations.Item(lRow).AllocationDate)
            oAllocation.AllocationID = CInt(m_oAllocations.Item(lRow).AllocationID)
            oAllocation.AllocationstatusID = CInt(m_oAllocations.Item(lRow).AllocationstatusID)
            oAllocation.CompanyID = CInt(m_oAllocations.Item(lRow).CompanyID)
            oAllocation.DatabaseStatus = CInt(m_oAllocations.Item(lRow).DatabaseStatus)
            oAllocation.UserID = CInt(m_oAllocations.Item(lRow).UserID)


            'm_oAllocations.Item(lRow)

            ' Check the Status of the Allocation

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oAllocation.DatabaseStatus
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

            ' Update Allocation Attributes





            m_lReturn = CType(SetProperties(oAllocation, iStatus, vAllocationID:=vAllocationID, vCompanyID:=vCompanyID, vAccountID:=vAccountID, vUserID:=vUserID, vAllocationDate:=vAllocationDate, vAllocationstatusID:=vAllocationstatusID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oAllocation = Nothing
                Return result
            End If

            ' Release reference to Allocation
            oAllocation = Nothing

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
    ' Description: Validate that the specified Allocation can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oAllocation As bACTAllocation.Allocation

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oAllocations.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oAllocation = m_oAllocations.Item(lRow)

            ' Check the Status of the Allocation

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oAllocation.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oAllocation.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oAllocation.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to Allocation
            oAllocation = Nothing

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
            For lSub As Integer = 1 To m_oAllocations.Count()
                Select Case m_oAllocations.Item(lSub).DatabaseStatus
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
        Dim oAllocation As bACTAllocation.Allocation
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oAllocations.Count()
                oAllocation = m_oAllocations.Item(lSub)


                Select Case oAllocation.DatabaseStatus
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
                        m_lReturn = CType(AddItem(oAllocation), gPMConstants.PMEReturnCode)
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
                        m_lReturn = CType(UpdateItem(oAllocation), gPMConstants.PMEReturnCode)
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
                        m_lReturn = CType(DeleteItem(oAllocation), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Release last reference
            oAllocation = Nothing

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
                    Do While lSub <= m_oAllocations.Count()

                        ' With the item
                        With m_oAllocations.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oAllocations.Delete(lSub)

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

    Public Function GetSubBranch(ByRef r_lSubBranchID As Integer, ByVal v_vAccountID As Object, ByVal v_vTransDetailID As Object, ByVal v_vPeriodID As Object, ByVal v_vBankAccountID As Object) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetSubBranch
        ' PURPOSE: Stub for GetSubBranchID so that call can be made from
        ' the Interface iACTAllocation
        ' AUTHOR: Danny Davis
        ' DATE: 05/08/2002, 15:35
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            result = bACTFunc.GetSubBranchID(v_oDatabase:=m_oDatabase, r_lSubBranchID:=r_lSubBranchID, v_vAccountID:=CStr(v_vAccountID), v_vTransDetailID:=CStr(v_vTransDetailID), v_vPeriodID:=CStr(v_vPeriodID), v_vBankAccountID:=CStr(v_vBankAccountID))


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSubBranch", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally

        End Try
        Return result


    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: AddItem (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Private Function AddItem(ByRef oAllocation As bACTAllocation.Allocation) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters
        m_lReturn = CType(AddInputParam(oAllocation:=oAllocation), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add AllocationID as an OUTPUT param for an insert
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Allocation_id", vValue:=oAllocation.AllocationID, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the ID of the record inserted
        oAllocation.AllocationID = m_oDatabase.Parameters.Item("Allocation_id").Value

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
    Private Function UpdateItem(ByRef oAllocation As bACTAllocation.Allocation) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the Input Parameters to the Parameteres collection
        m_lReturn = CType(AddInputParam(oAllocation:=oAllocation), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add AllocationID as an INPUT param for an update
        'dEVELOPER gUIDE NO 98
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Allocation_id", vValue:=oAllocation.AllocationID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' BB Not required by Orion
        ' Add Timestamp as an INPUT param for an update

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
    Private Function DeleteItem(ByRef oAllocation As bACTAllocation.Allocation) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the AllocationID INPUT parameter
        'dEVELOPER gUIDE No 98
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Allocation_id", vValue:=oAllocation.AllocationID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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

    ''' <summary>
    ''' SetPropertiesFromDB
    ''' </summary>
    ''' <param name="oAllocation"></param>
    ''' <param name="lRecordNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetPropertiesFromDB(ByRef oAllocation As bACTAllocation.Allocation, ByRef lRecordNumber As Integer) As Integer

        Dim nResult As Integer
        Dim oFields As DataRow



        nResult = gPMConstants.PMEReturnCode.PMTrue

        ' Set oFields to refer to Field Collection for Record 1
        oFields = m_oDatabase.Records.Item(lRecordNumber).Fields()

        ' Populate Base Details

        With oAllocation

            .AllocationID = oFields("allocation_id")

            If Convert.IsDBNull(oFields("company_id")) Or Informations.IsNothing(oFields("company_id")) Then
                .CompanyID = 0
            Else
                .CompanyID = oFields("company_id")
            End If

            If Convert.IsDBNull(oFields("account_id")) Or Informations.IsNothing(oFields("account_id")) Then
                .AccountID = 0
            Else
                .AccountID = oFields("account_id")
            End If

            If Convert.IsDBNull(oFields("user_id")) Or Informations.IsNothing(oFields("user_id")) Then
                .UserID = 0
            Else
                .UserID = oFields("user_id")
            End If

            If Convert.IsDBNull(oFields("allocation_date")) Or Informations.IsNothing(oFields("allocation_date")) Then
                .AllocationDate = #12/30/1899#
            Else
                .AllocationDate = oFields("allocation_date")
            End If

            If Convert.IsDBNull(oFields("allocationstatus_id")) Or Informations.IsNothing(oFields("allocationstatus_id")) Then
                .AllocationstatusID = 0
            Else
                .AllocationstatusID = oFields("allocationstatus_id")
            End If

            If Convert.IsDBNull(oFields("allocationbatch_id")) Or Informations.IsNothing(oFields("allocationbatch_id")) Then
                .AllocationBatchID = 0
            Else
                .AllocationBatchID = oFields("allocationbatch_id")
            End If

            .DatabaseStatus = gPMConstants.PMEComponentAction.PMView

        End With

        Return nResult

    End Function

    ''' <summary>
    ''' SetProperties
    ''' </summary>
    ''' <param name="oAllocation"></param>
    ''' <param name="iStatus"></param>
    ''' <param name="vAllocationID"></param>
    ''' <param name="vCompanyID"></param>
    ''' <param name="vAccountID"></param>
    ''' <param name="vUserID"></param>
    ''' <param name="vAllocationDate"></param>
    ''' <param name="vAllocationstatusID"></param>
    ''' <param name="r_nAllocationBatchID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetProperties(ByRef oAllocation As bACTAllocation.Allocation, ByRef iStatus As Integer, Optional ByRef vAllocationID As Object = Nothing,
                                   Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vUserID As Object = Nothing,
                                   Optional ByRef vAllocationDate As Object = Nothing, Optional ByRef vAllocationstatusID As Object = Nothing,
                                   Optional ByRef r_nAllocationBatchID As Integer = 0) As Integer


        Dim nResult As Integer
        Dim bDataChanged As Boolean = False



        nResult = gPMConstants.PMEReturnCode.PMTrue

        ' If Add Mode
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            ' Check that All Mandatory Parameters have been supplied
            nResult = CType(CheckMandatory(vAllocationID:=vAllocationID, vCompanyID:=vCompanyID, vAccountID:=vAccountID, vUserID:=vUserID, vAllocationDate:=vAllocationDate, vAllocationstatusID:=vAllocationstatusID), gPMConstants.PMEReturnCode)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            ' Default Any Missing Parameters
            m_lReturn = DefaultParameters(bDefaultAll:=False,
            vAllocationID:=vAllocationID,
            vCompanyID:=vCompanyID,
            vAccountID:=vAccountID,
            vUserID:=vUserID,
            vAllocationDate:=vAllocationDate,
            vAllocationstatusID:=vAllocationstatusID)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

        End If

        ' Validate Parameters
        nResult = CType(Validate(vAllocationID:=vAllocationID, vCompanyID:=vCompanyID, vAccountID:=vAccountID, vUserID:=vUserID, vAllocationDate:=vAllocationDate, vAllocationstatusID:=vAllocationstatusID), gPMConstants.PMEReturnCode)

        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            Return nResult
        End If

        ' Set Data Changed Flag to False
        bDataChanged = False

        ' Set Property values.
        With oAllocation

            If Not Informations.IsNothing(vAllocationID) Then
                If .AllocationID <> vAllocationID Then
                    .AllocationID = vAllocationID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vCompanyID) Then
                If .CompanyID <> ToSafeInteger(vCompanyID) Then
                    .CompanyID = vCompanyID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vAccountID) Then
                If .AccountID <> vAccountID Then
                    .AccountID = vAccountID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vUserID) Then
                If .UserID <> vUserID Then
                    .UserID = vUserID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vAllocationDate) Then
                If .AllocationDate <> CDate(vAllocationDate) Then
                    .AllocationDate = CDate(vAllocationDate)
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vAllocationstatusID) Then
                If .AllocationstatusID <> vAllocationstatusID Then
                    .AllocationstatusID = vAllocationstatusID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(r_nAllocationBatchID) Then
                If .AllocationBatchID <> r_nAllocationBatchID Then
                    .AllocationBatchID = r_nAllocationBatchID
                    bDataChanged = True
                End If
            End If

            ' If we have changed one of the properties, update the status
            If bDataChanged Then
                .DatabaseStatus = iStatus
            End If

        End With

        Return nResult

    End Function

    ' ***************************************************************** '
    ' Name: GetProperties (Private)
    '
    ' Description: Returns the supplied Allocation property values.
    '
    ' ***************************************************************** '
    'developer guide no.101
    Private Function GetProperties(ByRef oAllocation As bACTAllocation.Allocation, ByRef iStatus As Integer, Optional ByRef vAllocationID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vUserID As Object = Nothing, Optional ByRef vAllocationDate As Object = Nothing, Optional ByRef vAllocationstatusID As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set Property values.
        With oAllocation


            'developer guide no.118
            'start
            vAllocationID = .AllocationID

            vCompanyID = .CompanyID

            vAccountID = .AccountID

            vUserID = .UserID

            vAllocationDate = .AllocationDate

            vAllocationstatusID = .AllocationstatusID

            'end

            iStatus = .DatabaseStatus

        End With

        Return result

    End Function

    ''' <summary>
    ''' AddInputParam
    ''' </summary>
    ''' <param name="oAllocation"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AddInputParam(ByRef oAllocation As bACTAllocation.Allocation) As Integer

        Dim nResult As Integer


        nResult = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            If oAllocation.CompanyID < 1 Then
                m_lReturn = .Parameters.Add(sName:="company_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="company_id", vValue:=oAllocation.CompanyID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oAllocation.AccountID < 1 Then
                m_lReturn = .Parameters.Add(sName:="account_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="account_id", vValue:=oAllocation.AccountID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oAllocation.UserID < 1 Then
                m_lReturn = .Parameters.Add(sName:="user_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="user_id", vValue:=oAllocation.UserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="allocation_date", vValue:=oAllocation.AllocationDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oAllocation.AllocationstatusID < 1 Then
                m_lReturn = .Parameters.Add(sName:="allocationstatus_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="allocationstatus_id", vValue:=oAllocation.AllocationstatusID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If oAllocation.AllocationBatchID < 1 Then
                m_lReturn = .Parameters.Add(sName:="nAllocationbatch_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="nAllocationbatch_id", vValue:=oAllocation.AllocationBatchID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return nResult

    End Function

    ' ***************************************************************** '
    ' Name: DefaultParameters (Public)
    '
    ' Description: Sets the Default Values for a Allocation.
    '
    ' ***************************************************************** '
    'developer guide no.33
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vAllocationID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vUserID As Object = Nothing, Optional ByRef vAllocationDate As Object = Nothing, Optional ByRef vAllocationstatusID As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vAllocationID)) Or (vAllocationID.Equals(0)) Or (bDefaultAll) Then
            vAllocationID = 0
        End If



        If (Informations.IsNothing(vCompanyID)) Or (vCompanyID.Equals(0)) Or (bDefaultAll) Then
            vCompanyID = 0
        End If



        If (Informations.IsNothing(vAccountID)) Or (vAccountID.Equals(0)) Or (bDefaultAll) Then
            vAccountID = 0
        End If



        If (Informations.IsNothing(vUserID)) Or (vUserID.Equals(0)) Or (bDefaultAll) Then
            vUserID = 0
        End If



        If (Informations.IsNothing(vAllocationDate)) Or (vAllocationDate.Equals(DateTime.FromOADate(0))) Or (bDefaultAll) Then
            vAllocationDate = DateTime.Now
        End If



        If (Informations.IsNothing(vAllocationstatusID)) Or (vAllocationstatusID.Equals(0)) Or (bDefaultAll) Then
            vAllocationstatusID = 0
        End If


        ' {* USER DEFINED CODE (End) *}


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Public)
    '
    ' Description: Sets the Default Values for a Allocation.
    '
    ' ***************************************************************** '
    Private Function CheckMandatory(Optional ByRef vAllocationID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vUserID As Object = Nothing, Optional ByRef vAllocationDate As Object = Nothing, Optional ByRef vAllocationstatusID As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vAllocationID)) Or (Object.Equals(vAllocationID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vCompanyID)) Or (Object.Equals(vCompanyID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vAccountID)) Or (Object.Equals(vAccountID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vUserID)) Or (Object.Equals(vUserID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vAllocationDate)) Or (Object.Equals(vAllocationDate, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vAllocationstatusID)) Or (Object.Equals(vAllocationstatusID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If


        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the Allocation for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vAllocationID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vUserID As Object = Nothing, Optional ByRef vAllocationDate As Object = Nothing, Optional ByRef vAllocationstatusID As Object = Nothing) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        Dim dbNumericTemp As Double

        If (Not Informations.IsNothing(vAllocationID)) And (Not Double.TryParse(CStr(vAllocationID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp2 As Double

        If (Not Informations.IsNothing(vCompanyID)) And (Not Double.TryParse(CStr(vCompanyID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp3 As Double

        If (Not Informations.IsNothing(vAccountID)) And (Not Double.TryParse(CStr(vAccountID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp4 As Double

        If (Not Informations.IsNothing(vUserID)) And (Not Double.TryParse(CStr(vUserID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If (Not Informations.IsNothing(vAllocationDate)) And (Not Informations.IsDate(vAllocationDate)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp5 As Double

        If (Not Informations.IsNothing(vAllocationstatusID)) And (Not Double.TryParse(CStr(vAllocationstatusID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5)) Then
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


    ' ***************************************************************** '
    ' Name: GetToAllocateAmount
    '
    ' Description: Gets the amount left for allocation for a cashlist(item)
    '
    ' History :
    '
    ' DJM 06/12/2002 : Created
    '
    ' ***************************************************************** '
    Public Function GetToAllocateAmount(ByRef r_cToAllocateAmount As Decimal, ByVal v_lCashListItemID As Integer, ByVal v_lCashListID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim cAllocatedAmount, cCashListAmount As Decimal

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get already allocated amount
            sSQL = ""
            sSQL = sSQL & "SELECT ISNULL(SUM(ad.alloc_base_amount),0) "
            sSQL = sSQL & "FROM cashlistitem cli "
            sSQL = sSQL & "JOIN allocationdetail ad "
            sSQL = sSQL & "ON ad.transdetail_id = cli.transdetail_id "

            If Not False Then
                sSQL = sSQL & "WHERE cli.cashlistitem_id = " & CStr(v_lCashListItemID)
            ElseIf Not False Then
                sSQL = sSQL & " WHERE cli.cashlist_id = " & CStr(v_lCashListID)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetToAllocateAmount", bStoredProcedure:=False, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            cAllocatedAmount = 0
            If Informations.IsArray(vResultArray) Then

                cAllocatedAmount = CDec(vResultArray(0, 0))
            End If

            'Get original amount
            sSQL = ""
            sSQL = sSQL & "SELECT isnull(SUM(td.amount),0) "
            sSQL = sSQL & "FROM cashlistitem cli "
            sSQL = sSQL & "JOIN transdetail td "
            sSQL = sSQL & "ON td.transdetail_id = cli.transdetail_id "

            If Not False Then
                sSQL = sSQL & "WHERE cli.cashlistitem_id = " & CStr(v_lCashListItemID)
            ElseIf Not False Then
                sSQL = sSQL & " WHERE cli.cashlist_id = " & CStr(v_lCashListID)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetToAllocateAmount", bStoredProcedure:=False, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then

                cCashListAmount = CDec(vResultArray(0, 0))
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Return amount left for allocation
            r_cToAllocateAmount = (cCashListAmount - cAllocatedAmount) * -1
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetToAllocateAmountFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetToAllocateAmount", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class