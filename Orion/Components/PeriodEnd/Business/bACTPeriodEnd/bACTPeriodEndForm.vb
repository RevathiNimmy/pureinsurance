Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'developer guide no.129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Form_NET.Form")> _
Public NotInheritable Class Form
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 28/08/1998
    '
    ' Description: Class for performing Account Period and Year Ends.
    '
    ' Edit History:
    '   DD 01/08/2002: Major cleardown. Removed all obsolete code which
    '                  was inherited when this component was built from
    '                  bACTPeriod.
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

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

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

    ' Instance Component Services

    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business
    ' PRIVATE Data Members (End)

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

    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ********************************************************************** '
    ' Name: GetPeriodDates
    '
    ' Description: Gets THREE period_end_dates
    '              Support function for CheckPeriodsMatch in iACTPeriodEnd
    '
    ' ********************************************************************** '
    Public Function GetPeriodEndDates(ByVal v_vPeriodIDs As Object, ByRef r_vPeriodEndDates(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' clear the parameters
            m_oDatabase.Parameters.Clear()

            ' add the new ones
            ' input

            m_lReturn = m_oDatabase.Parameters.Add(sName:="period_id_1", vValue:=CStr(v_vPeriodIDs(0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' input
            m_lReturn = m_oDatabase.Parameters.Add(sName:="period_id_2", vValue:=CStr(v_vPeriodIDs(1)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' input

            m_lReturn = m_oDatabase.Parameters.Add(sName:="period_id_3", vValue:=CStr(v_vPeriodIDs(2)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Perform the SQL
            'KB 21032003 PN 2662 1.6.9 -> 1.8.6 catchup
            'return all records
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPeriodDatesSQL, sSQLName:=ACGetPeriodDatesName, bStoredProcedure:=True, vResultArray:=r_vPeriodEndDates, lNumberRecords:=gPMConstants.PMAllRecords)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPeriodEndDates Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPeriodEndDates", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPreviousPeriodEndComplete
    '
    ' Description: Gets the value of the previous period_end_complete
    '
    ' ***************************************************************** '
    Public Function GetPreviousPeriodEndComplete(ByVal v_lCurrentPeriodID As Integer, ByRef r_lPreviousPeriodID As Integer, ByRef r_iPreviousPeriodEndComplete As Integer) As Integer

        Dim result As Integer = 0
        Dim oACTPeriod As bACTPeriod.Form
        Dim lPreviousID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create instance of bACTPeriod
            'SD 23/07/2002 correct variable name

            oACTPeriod = New bACTPeriod.Form
            m_lReturn = oACTPeriod.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' get the previous period id

            m_lReturn = oACTPeriod.GetPreviousPeriodID(lPeriodID:=v_lCurrentPeriodID, lPreviousPeriodID:=lPreviousID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' read in the record the previous period

            m_lReturn = oACTPeriod.GetDetails(vPeriodID:=lPreviousID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the period_end_complete value

            m_lReturn = oACTPeriod.GetNext(vPeriodEndComplete:=r_iPreviousPeriodEndComplete)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '
            r_lPreviousPeriodID = lPreviousID

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPreviousPeriodEndCompleteFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPreviousPeriodEndComplete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessPeriodEnd
    '
    ' Description: Processes the period end
    ' eck 310701 Return the Previous Period End Remove Year End Flag
    ' ***************************************************************** '
    Public Function ProcessPeriodEnd(ByVal v_lCurrentPeriodID As Integer, ByRef v_lPreviousPeriodId As Integer) As Integer

        Dim result As Integer = 0
        Dim oACTPeriod As bACTPeriod.Form
        'Dim lPreviousPeriodID As Long
        Dim vCurrentYear, vPeriodEndYear, vPeriodEndDate As Object

        Dim vCompanyID, vSubBranchID, vYearName, vPeriodName As Object
        'developer guide no.101
        Dim vPeriodEndComplete As Object

        Dim vPreviousPeriodEndDate As Date

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create instance of bACTPeriod
            'SD 23/07/2002 correct variable name

            oACTPeriod = New bACTPeriod.Form
            m_lReturn = oACTPeriod.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get record where period_id = current_period_id

            m_lReturn = oACTPeriod.GetDetails(vPeriodID:=v_lCurrentPeriodID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oACTPeriod.GetNext(vYearName:=vCurrentYear, vPeriodEndDate:=vPeriodEndDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call GetPreviousPeriodID of bACTPeriod passing lCurrentPeriodID
            ' Get period_id_for_period_end

            m_lReturn = oACTPeriod.GetPreviousPeriodID(lPeriodID:=v_lCurrentPeriodID, lPreviousPeriodID:=v_lPreviousPeriodId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get record where period_id = period_id_for_period_end

            m_lReturn = oACTPeriod.GetDetails(vPeriodID:=v_lPreviousPeriodId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oACTPeriod.GetNext(vYearName:=vPeriodEndYear, vPeriodEndDate:=vPreviousPeriodEndDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' CF 260899 - Fixed to process period end before year end.

            ' Process the period end budgets

            m_lReturn = ProcessPeriodEndBudgets(v_lPeriodIDForPeriodEnd:=v_lPreviousPeriodId, v_sPeriodYear:=CStr(vPeriodEndYear))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get record where period_id = period_id_for_period_end

            m_lReturn = oACTPeriod.GetDetails(vPeriodID:=v_lPreviousPeriodId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DD 01/08/2002: Added SubBranchID

            m_lReturn = oACTPeriod.GetNext(vCompanyID:=vCompanyID, vSubBranchID:=vSubBranchID, vYearName:=vYearName, vPeriodName:=vPeriodName, vPeriodEndDate:=vPeriodEndDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' period ended
            vPeriodEndComplete = 1

            ' Update to show the period has ended
            'DD 01/08/2002: Added SubBranchID

            m_lReturn = oACTPeriod.EditUpdate(lRow:=1, vPeriodID:=v_lPreviousPeriodId, vCompanyID:=vCompanyID, vSubBranchID:=vSubBranchID, vYearName:=vYearName, vPeriodName:=vPeriodName, vPeriodEndDate:=vPeriodEndDate, vPeriodEndComplete:=vPeriodEndComplete)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' commit the changes

            m_lReturn = oACTPeriod.Update()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Remove instance of bACTPeriod
            oACTPeriod = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to process period end", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessPeriodEnd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessYearEnd
    '
    ' Description: Processes the end of year
    '
    ' ***************************************************************** '
    Public Function ProcessYearEnd(ByVal v_lPeriodIDPeriodEnd As Integer, ByVal v_sPeriodYear As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Call ProcessRetainedProfitJournal
            m_lReturn = ProcessRetainedProfitJournalV2(v_lPeriodID:=v_lPeriodIDPeriodEnd)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to process year end", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessYearEnd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPeriodEndDate
    '
    ' Description: Gets the period end date for the passed period
    '
    ' ***************************************************************** '
    Private Function GetPeriodEndDate(ByRef lPeriodID As Integer, ByRef vEndDate As Object) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object
        Dim sSQL As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Construct the SQL
        sSQL = "SELECT period_end_date FROM Period WHERE period_id = " & lPeriodID

        ' Hit the database
        'KB 21032003 PN 2662
        '1.6.9 -> 1.8.6 catchup

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetPeriodEndDate", bStoredProcedure:=False, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)

        ' Get the returned value
        If Information.IsArray(vResultArray) Then


            vEndDate = vResultArray(0, 0)
        Else


            vEndDate = vResultArray
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetPeriodTotal
    '
    ' Description: Calls the stored procedure to get the total of all
    '              transaction amounts for the given period.
    '
    '   eck310701 Return array of OS Transactions
    ' ***************************************************************** '
    Private Function GetPeriodTotal(ByVal v_lPeriodID As Integer, ByVal v_lAccountID As Integer, ByVal v_lCompanyID As Integer, ByRef r_cTotal As Decimal, ByRef r_vTransactionArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPeriodTotal"





        result = gPMConstants.PMEReturnCode.PMTrue

        'Clear the parameters
        m_oDatabase.Parameters.Clear()

        'Add period_id
        m_lReturn = m_oDatabase.Parameters.Add(sName:="period_id", vValue:=CStr(v_lPeriodID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "sName:=period_id, vValue:=" & v_lPeriodID, gPMConstants.PMELogLevel.PMLogError)
        End If

        'Add account_id
        m_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=CStr(v_lAccountID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "sName:=account_id, vValue:=" & v_lAccountID, gPMConstants.PMELogLevel.PMLogError)
        End If

        'Add company_id
        m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(v_lCompanyID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "sName:=account_id, vValue:=" & v_lCompanyID, gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Perform the SQL
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPeriodTotalSQL, sSQLName:=ACGetPeriodTotalName, bStoredProcedure:=ACGetPeriodTotalStored, vResultArray:=r_vTransactionArray, lNumberRecords:=gPMConstants.PMAllRecords)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "sSQL:=ACGetPeriodTotalSQL", gPMConstants.PMELogLevel.PMLogError)
        End If

        r_cTotal = 0

        If Information.IsArray(r_vTransactionArray) Then
            For lLoop As Integer = 0 To r_vTransactionArray.GetUpperBound(1)

                r_cTotal += CDec(r_vTransactionArray(1, lLoop))
            Next
        End If

        ' Do any tidy up, e.g. Set x = Nothing here
        Return result
        ' This is for debugging only
    End Function


    ' ***************************************************************** '
    ' Name: GetRetainedProfitAccountID
    '
    ' Description: Gets the account_id for the retained proft account
    '
    ' DD 01/08/2002: Added parameter for multi-branch accounting
    ' ***************************************************************** '
    'developer guide no.101
    Private Function GetRetainedProfitAccountID(ByRef r_vAccountID As Object, Optional ByVal v_vCompanyID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object



        ' Construct the SQL
        sSQL = ""
        sSQL = sSQL & "SELECT" & Strings.Chr(13) & Strings.Chr(10)
        sSQL = sSQL & "    account_id" & Strings.Chr(13) & Strings.Chr(10)
        sSQL = sSQL & "FROM account" & Strings.Chr(13) & Strings.Chr(10)
        sSQL = sSQL & "WHERE short_code like 'RET%PROF%'" & Strings.Chr(13) & Strings.Chr(10)


        If Not Information.IsNothing(v_vCompanyID) Then

            sSQL = sSQL & "AND company_id = " & CStr(v_vCompanyID)
        End If

        ' Perform the SQL
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetRetainedProfitAccountID", bStoredProcedure:=False, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Grab the return value
        If Information.IsArray(vResultArray) Then

            'developer guide no.101
            r_vAccountID = vResultArray(0, 0)
        Else
            r_vAccountID = 0
        End If


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    Public Function ProcessRetainedProfitJournal(ByVal v_lPeriodID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessRetainedProfitJournal"

        Dim oManualAllocation As bACTAllocationManual.Business
        Dim oDocumentPost As bACTDocumentPost.Form
        Dim oPMAutoNumber As bACTAutoNumber.Business

        Dim lCompanyID, lSubBranchID As Integer
        Dim vResultArray(,) As Object
        Dim vBranchIDs(,) As Object
        Dim cRunningTotal, cCurrentTotal As Decimal
        Dim lAccountID As Integer
        Dim iCurrencyID As Integer
        Dim vDocumentId As Object
        Dim lDocPostedStatus As Integer
        Dim dtDocAccountingDate As Date
        Dim vTransactionArray, vMatchTrans, vKeys As Object
        Dim lDocSequence, lNumberRangeID As Integer
        Dim sDocumentRef As String = ""
        Dim lNumber, lTransID As Integer
        Dim vMultiLedger As String = ""

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Get company currency for company_id
            iCurrencyID = m_iCurrencyID

            'Create an instance of DocumentPost

            oDocumentPost = New bACTDocumentPost.Form
            m_lReturn = oDocumentPost.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("gPMComponentServices.CreateBusinessObject", "v_sClassName:=bACTDocumentPost.Form", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Create an instance of Auto number

            oPMAutoNumber = New bACTAutoNumber.Business
            m_lReturn = oPMAutoNumber.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("gPMComponentServices.CreateBusinessObject", "v_sClassName:=bACTAutoNumber.Business", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Create a new instance of Manual Allocation

            oManualAllocation = New bACTAllocationManual.Business
            m_lReturn = oManualAllocation.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("gPMComponentServices.CreateBusinessObject", "v_sClassName:=bACTAllocationManual.Business", gPMConstants.PMELogLevel.PMLogError)
            End If


            'Is this a multi-ledger system
            bPMFunc.getProductOptionValue(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, m_iSourceID, vMultiLedger)


            If gPMFunctions.NullToString(vMultiLedger) = "1" Then
                'Just get the branch that we are logged in as.
                m_lReturn = GetBranchIDs(r_vBranches:=vBranchIDs, v_vCompanyID:=m_iSourceID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("GetBranchIDs", "Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            Else
                'Get all branches
                m_lReturn = GetBranchIDs(r_vBranches:=vBranchIDs)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("GetBranchIDs", "Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            If Not Information.IsArray(vBranchIDs) Then
                gPMFunctions.RaiseError("GetBranchIDs", "No branch details returned", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get all income and expense accounts regardless of branch
            'Clear the parameters
            m_oDatabase.Parameters.Clear()

            'Perform the select
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAccountsSQL, sSQLName:=ACGetAccountsName, bStoredProcedure:=ACGetAccountsStored, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "sSQLName:=ACGetAccountsName", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Information.IsArray(vResultArray) Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "No account details returned", gPMConstants.PMELogLevel.PMLogError)
            End If


            oDocumentPost.PostingPeriodNumber = v_lPeriodID

            ' Get the last day of the period
            m_lReturn = GetPeriodEndDate(lPeriodID:=v_lPeriodID, vEndDate:=dtDocAccountingDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetPeriodEndDate", "lPeriodID:=" & v_lPeriodID, gPMConstants.PMELogLevel.PMLogError)
            End If


            For lBranchLoop As Integer = 0 To vBranchIDs.GetUpperBound(1)


                lCompanyID = CInt(vBranchIDs(0, lBranchLoop))

                lSubBranchID = CInt(vBranchIDs(1, lBranchLoop))

                'Reset the total
                cRunningTotal = 0

                ' Current document sequence number
                lDocSequence = 1

                'Loop through all matching accounts

                For lAccountLoop As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                    'Get the current account_id

                    lAccountID = CInt(vResultArray(0, lAccountLoop))

                    'Return array of transactions to be matched and total amount

                    m_lReturn = GetPeriodTotal(v_lPeriodID:=v_lPeriodID, v_lAccountID:=lAccountID, v_lCompanyID:=lCompanyID, r_cTotal:=cCurrentTotal, r_vTransactionArray:=vTransactionArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("GetPeriodTotal", "v_vPeriodID:=" & v_lPeriodID, gPMConstants.PMELogLevel.PMLogError)
                    End If

                    'Increment the running total
                    cRunningTotal += cCurrentTotal
                    cCurrentTotal *= -1

                    'Post a transaction line if there are any outstanding transactions on the account.
                    'This allows the transactions to be allocated even if the overall outstanding amount is zero.
                    If Information.IsArray(vTransactionArray) Then

                        'First time through create a document
                        If lDocSequence = 1 Then
                            'Find out the range number for Orion document ref's

                            m_lReturn = oPMAutoNumber.GetNumberRange(v_sGroupCode:=gACTLibrary.ACTAutoNumberGroupCodeDocumentRef1, v_sRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeJn, r_lNumberRangeID:=lNumberRangeID)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("oPMAutoNumber.GetNumberRange", "v_sRangeCode:=" & gACTLibrary.ACTAutoNumberRangeCodeJn, gPMConstants.PMELogLevel.PMLogError)
                            End If

                            'Get the next number
                            'Start(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                            'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber

                            m_lReturn = oPMAutoNumber.GenerateDocumentReferenceNumber(v_lNumberRangeID:=lNumberRangeID, v_iUserID:=m_iUserID, v_iCompanyID:=lCompanyID, r_sDocumentRef:=sDocumentRef, v_sRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeJn)
                            'End(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                                'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber
                                gPMFunctions.RaiseError("oPMAutoNumber.GenerateDocumentReferenceNumber", "v_lNumberRangeID:=" & lNumberRangeID, gPMConstants.PMELogLevel.PMLogError)
                            End If

                            'Add a few zero's on the front
                            '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                            'sDocumentRef = Format(lNumber, "00000000")
                            sDocumentRef = gACTLibrary.ACTAutoNumberRangeCodeJn & sDocumentRef


                            m_lReturn = oDocumentPost.AddDocument(v_lDocumentTypeID:=gACTLibrary.ACTDocTypeJournal, v_sDocumentRef:=sDocumentRef, v_sComment:="Year End Retained Profit", v_dtDocumentDate:=dtDocAccountingDate, r_vDocumentId:=vDocumentId, r_vDocSourceID:=lCompanyID, r_vSubBranchID:=lSubBranchID)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("oDocumentPost.AddDocument", "v_sDocumentRef:=" & sDocumentRef, gPMConstants.PMELogLevel.PMLogError)
                            End If
                        End If

                        'Post the journal transaction

                        m_lReturn = oDocumentPost.AddTransaction(v_lAccountID:=lAccountID, v_vDocumentSequence:=lDocSequence, v_iCurrencyID:=iCurrencyID, v_cAmount:=cCurrentTotal, v_cCurrencyAmount:=cCurrentTotal, v_vdCurrencyBaseXRate:=1, v_vOperatorID:=m_iUserID, v_vAccountingDate:=dtDocAccountingDate, r_vTransDetailId:=lTransID)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        'Next document sequence
                        lDocSequence += 1

                        'Auto Allocate the journal against the outstanding transactions

                        ReDim vMatchTrans(vTransactionArray.GetUpperBound(1))

                        For lTransactionLoop As Integer = 0 To vMatchTrans.GetUpperBound(0)



                            vMatchTrans(lTransactionLoop) = CStr(CInt(vTransactionArray(0, lTransactionLoop))) & "|" & CStr(CDec(vTransactionArray(1, lTransactionLoop)))
                        Next


                        ReDim vKeys(1, 2)
                        'AccountID

                        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID

                        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = lAccountID

                        'JournalTransID

                        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameTransDetailID

                        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = CStr(lTransID) & "|" & CStr(cCurrentTotal)

                        'Outstanding Items

                        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameTransDetailIDs


                        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = vMatchTrans


                        m_lReturn = oManualAllocation.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("oManualAllocation.SetProcessModes", "Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        'Set the keys

                        m_lReturn = oManualAllocation.SetKeys(vKeyArray:=vKeys)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("oManualAllocation.SetKeys", "Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If


                        oManualAllocation.CompanyID = lCompanyID


                        m_lReturn = oManualAllocation.Start()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("oManualAllocation.Start", "Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                    End If
                Next

                'If we have
                If lDocSequence <> 1 Then

                    'Get retained profit account_id
                    lAccountID = 0

                    If gPMFunctions.NullToString(vMultiLedger) = "1" Then
                        m_lReturn = GetRetainedProfitAccountID(r_vAccountID:=lAccountID, v_vCompanyID:=lCompanyID)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("GetRetainedProfitAccountID", "Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    Else
                        m_lReturn = GetRetainedProfitAccountID(r_vAccountID:=lAccountID)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("GetRetainedProfitAccountID", "Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If

                    'Fail if no Retained Profit Account set up
                    If lAccountID = 0 Then
                        gPMFunctions.RaiseError("GetRetainedProfitAccountID", "Failed to find retained profit account", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    'Post the balancing journal to Retained Profit

                    m_lReturn = oDocumentPost.AddTransaction(v_lAccountID:=lAccountID, v_vDocumentSequence:=lDocSequence, v_iCurrencyID:=m_iCurrencyID, v_cAmount:=cRunningTotal, v_cCurrencyAmount:=cRunningTotal, v_vdCurrencyBaseXRate:=1, v_vOperatorID:=m_iUserID, v_vAccountingDate:=dtDocAccountingDate)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("oDocumentPost.AddTransaction", "v_lAccountID:=" & lAccountID, gPMConstants.PMELogLevel.PMLogError)
                    End If

                    'Set the posting status to Posted
                    lDocPostedStatus = gACTLibrary.ACTPostStatusPosted

                End If

            Next


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here
            If Not (oPMAutoNumber Is Nothing) Then

                oPMAutoNumber.Dispose()
                oPMAutoNumber = Nothing
            End If

            If Not (oDocumentPost Is Nothing) Then

                oDocumentPost.Dispose()
                oDocumentPost = Nothing
            End If

            If Not (oManualAllocation Is Nothing) Then

                oManualAllocation.Dispose()
                oManualAllocation = Nothing
            End If

            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    '
    ' Name: ProcessPeriodEndBudgets
    '
    ' Description: Calls bACTBudget to process end of period
    '
    ' ***************************************************************** '
    Public Function ProcessPeriodEndBudgets(ByVal v_lPeriodIDForPeriodEnd As Integer, ByVal v_sPeriodYear As String) As Integer

        Dim result As Integer = 0
        Dim oBudgetControl As bACTBudget.Form

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' create an instance of bACTBudget
            ' sd 23/07/2002 correct variable name
            ' DD 01/08/2002: Method now in bACTBudget Object

            oBudgetControl = New bACTBudget.Form
            m_lReturn = oBudgetControl.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Process the end of period

            m_lReturn = oBudgetControl.UpdateActualsAndVariances(v_lPeriodIDForPeriodEnd)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Remove the instance of budgetcontrol
            oBudgetControl = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to process period end budgets.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessPeriodEndBudgets", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            ' Set UserID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            'DD 01/08/2002: New method for database connection validation

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the ProcessMode to Generic
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric

            ' Set the Type Of Business to New Business
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric

            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now

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

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: GetSubBranchID
    '
    ' Description: Gets the value of the Sub Branch from the Period
    '
    ' DD 01/08/2002: Created
    ' ***************************************************************** '
    Private Function GetBranchIDs(ByRef r_vBranches(,) As Object, Optional ByVal v_vCompanyID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetBranchIDs"

        Dim sSQL As String = ""




        result = gPMConstants.PMEReturnCode.PMTrue

        sSQL = ""
        sSQL = sSQL & "SELECT" & Strings.Chr(13) & Strings.Chr(10)
        sSQL = sSQL & "    s.source_id," & Strings.Chr(13) & Strings.Chr(10)
        sSQL = sSQL & "    (" & Strings.Chr(13) & Strings.Chr(10)
        sSQL = sSQL & "        SELECT" & Strings.Chr(13) & Strings.Chr(10)
        sSQL = sSQL & "            ISNULL(MIN(sub_branch_id), 0)" & Strings.Chr(13) & Strings.Chr(10)
        sSQL = sSQL & "        FROM sub_branch" & Strings.Chr(13) & Strings.Chr(10)
        sSQL = sSQL & "        WHERE source_id = s.source_id" & Strings.Chr(13) & Strings.Chr(10)
        sSQL = sSQL & "        AND is_deleted = 0" & Strings.Chr(13) & Strings.Chr(10)
        sSQL = sSQL & "    )" & Strings.Chr(13) & Strings.Chr(10)
        sSQL = sSQL & "FROM source s" & Strings.Chr(13) & Strings.Chr(10)
        sSQL = sSQL & "WHERE s.is_deleted = 0" & Strings.Chr(13) & Strings.Chr(10)


        If Not Information.IsNothing(v_vCompanyID) Then

            sSQL = sSQL & "AND s.source_id = " & CStr(v_vCompanyID) & Strings.Chr(13) & Strings.Chr(10)
        End If

        'Clear the parameters
        m_oDatabase.Parameters.Clear()

        ' Perform the SQL
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetBranchIDs", bStoredProcedure:=False, vResultArray:=r_vBranches, lNumberRecords:=gPMConstants.PMAllRecords)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "sSQLName:=GetBranchIDs", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Do any tidy up, e.g. Set x = Nothing here
        Return result
        ' This is for debugging only
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetReports
    '
    ' Description: Gets ALL of the data off the report table.
    '
    ' History: 26/08/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetReports(ByRef r_vReportArray As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Construct the SQL
            sSQL = "SELECT periodendreport_id, " & _
                   "caption_id, " & _
                   "code, " & _
                   "description, " & _
                   "report_filename, " & _
                   "is_active_at_period_end, " & _
                   "is_active_at_year_end " & _
                   "FROM  PeriodEndReport " & _
                   "WHERE is_deleted = 0"

            ' Perform the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetPeriodEndReports", bStoredProcedure:=False, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check if we have any results
            If Not Information.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Assign the results


            r_vReportArray = vResultArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetReports Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReports", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (BeginTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function BeginTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLBeginTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CommitTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CommitTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLCommitTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (RollbackTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function RollbackTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLRollbackTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
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




    ' ***************************************************************** '
    ' Name: AllowYearEnd (Public)
    '
    ' Description: Returns true if a year end is required.
    '
    ' DJM 10/01/2003 : Created.
    ' DJM 07/04/2003 : Rewrote script to not rely on period id being in date order.
    '                  Also now allows year end at any point during last period.
    ' KB 07042003    : PN Issue 1919 1.6.9 -> 1.8.6 catchup
    '                  Incorporated changes above
    ' DD 22/07/2003  : Rewritten as a stored procedure as 1.6 code did not work.
    ' ***************************************************************** '
    Public Function AllowYearEnd(ByVal v_lPeriod_Id As Integer, ByRef r_bAllowYearEnd As Boolean) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_bAllowYearEnd = False

            'Perform the SQL
            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("period_id", CStr(v_lPeriod_Id), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                'developer guide no.39
                m_lReturn = .SQLSelect(sSQL:="spu_ACT_Check_AllowYearEnd", sSQLName:="AllowYearEnd", bStoredProcedure:=True, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            'Return the value
            r_bAllowYearEnd = (Information.IsArray(vResultArray))

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AllowYearEnd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AllowYearEnd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub


    Public Function ProcessRetainedProfitJournalV2(ByVal v_lPeriodID As Integer) As Integer

        Dim nResult As Integer = 0
        Const kMethodName As String = "ProcessRetainedProfitJournal"

        Dim oDocumentPost As bACTDocumentPost.Form
        Dim oPMAutoNumber As bACTAutoNumber.Business

        Dim lCompanyID, lSubBranchID As Integer
        Dim vResultArray(,) As Object
        Dim vBranchIDs(,) As Object
        Dim cRunningTotal As Decimal
        Dim nAccountID As Integer
        Dim nCurrencyID As Integer
        Dim nDocumentId As Integer = 0
        Dim nDocPostedStatus As Integer
        Dim dtDocAccountingDate As Date
        Dim nDocSequence, lNumberRangeID As Integer
        Dim sDocumentRef As String = ""
        Dim vMultiLedger As String = ""
        Dim lAllocationBatchId As Long
        Dim nAccountLoop As Integer
        Try



            nResult = gPMConstants.PMEReturnCode.PMTrue

            'Get company currency for company_id
            nCurrencyID = m_iCurrencyID

            'Create an instance of DocumentPost

            oDocumentPost = New bACTDocumentPost.Form
            m_lReturn = oDocumentPost.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("gPMComponentServices.CreateBusinessObject", "v_sClassName:=bACTDocumentPost.Form", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Create an instance of Auto number
            oPMAutoNumber = New bACTAutoNumber.Business
            m_lReturn = oPMAutoNumber.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("gPMComponentServices.CreateBusinessObject", "v_sClassName:=bACTAutoNumber.Business", gPMConstants.PMELogLevel.PMLogError)
            End If


            'Is this a multi-ledger system
            bPMFunc.getProductOptionValue(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, m_iSourceID, vMultiLedger)


            If gPMFunctions.NullToString(vMultiLedger) = "1" Then
                'Just get the branch that we are logged in as.
                m_lReturn = GetBranchIDs(r_vBranches:=vBranchIDs, v_vCompanyID:=m_iSourceID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("GetBranchIDs", "Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            Else
                'Get all branches
                m_lReturn = GetBranchIDsForYearEnd(nPeriodId:=v_lPeriodID, r_vBranches:=vBranchIDs)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("GetBranchIDs", "Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            If Not Information.IsArray(vBranchIDs) Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="No branch details returned", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRetainedProfitJournalV2()")
            End If

            'Get all income and expense accounts regardless of branch
            'Clear the parameters
            m_oDatabase.Parameters.Clear()

            'Perform the select
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAccountsSQL, sSQLName:=ACGetAccountsName, bStoredProcedure:=ACGetAccountsStored, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "sSQLName:=ACGetAccountsName", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Information.IsArray(vResultArray) Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "No account details returned", gPMConstants.PMELogLevel.PMLogError)
            End If


            oDocumentPost.PostingPeriodNumber = v_lPeriodID

            ' Get the last day of the period
            m_lReturn = GetPeriodEndDate(lPeriodID:=v_lPeriodID, vEndDate:=dtDocAccountingDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetPeriodEndDate", "lPeriodID:=" & v_lPeriodID, gPMConstants.PMELogLevel.PMLogError)
            End If

            If IsArray(vBranchIDs) Then
                For lBranchLoop As Integer = 0 To vBranchIDs.GetUpperBound(1)
                    lCompanyID = CInt(vBranchIDs(0, lBranchLoop))
                    lSubBranchID = CInt(vBranchIDs(1, lBranchLoop))

                    'Reset the total
                    cRunningTotal = 0

                    ' Current document sequence number
                    nDocSequence = 1
                    'Loop through all matching accounts
                    m_lReturn = m_oDatabase.SQLBeginTrans()


                    'Post a transaction line if there are any outstanding transactions on the account.
                    'This allows the transactions to be allocated even if the overall outstanding amount is zero.
                    'First time through create a document
                    If nDocSequence = 1 Then
                        'Find out the range number for Orion document ref's

                        m_lReturn = oPMAutoNumber.GetNumberRange(v_sGroupCode:=gACTLibrary.ACTAutoNumberGroupCodeDocumentRef1, v_sRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeJn, r_lNumberRangeID:=lNumberRangeID)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("oPMAutoNumber.GetNumberRange", "v_sRangeCode:=" & gACTLibrary.ACTAutoNumberRangeCodeJn, gPMConstants.PMELogLevel.PMLogError)
                        End If

                        'Get the next number
                        'Start(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                        'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber

                        m_lReturn = oPMAutoNumber.GenerateDocumentReferenceNumber(v_lNumberRangeID:=lNumberRangeID, v_iUserID:=m_iUserID, v_iCompanyID:=lCompanyID, r_sDocumentRef:=sDocumentRef, v_sRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeJn)
                        'End(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                            'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber
                            gPMFunctions.RaiseError("oPMAutoNumber.GenerateDocumentReferenceNumber", "v_lNumberRangeID:=" & lNumberRangeID, gPMConstants.PMELogLevel.PMLogError)
                        End If

                        'Add a few zero's on the front
                        '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                        'sDocumentRef = Format(lNumber, "00000000")
                        sDocumentRef = gACTLibrary.ACTAutoNumberRangeCodeJn & sDocumentRef


                        m_lReturn = oDocumentPost.AddDocument(v_lDocumentTypeId:=gACTLibrary.ACTDocTypeJournal, v_sDocumentRef:=sDocumentRef, _
                                                              v_sComment:="Year End Retained Profit", v_dtDocumentDate:=dtDocAccountingDate, _
                                                              r_vDocumentID:=nDocumentId, r_vDocSourceID:=lCompanyID, r_vSubBranchID:=lSubBranchID)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("oDocumentPost.AddDocument", "v_sDocumentRef:=" & sDocumentRef, gPMConstants.PMELogLevel.PMLogError)
                        End If

                        If lAllocationBatchId = 0 Then
                            m_lReturn = CreateAllocationBatch(lAllocationBatchId)
                        End If

                    End If

                    Dim crRunningTotal As Decimal
                    'Clear the parameters
                    For nAccountLoop = 0 To UBound(vResultArray, 2)
                        m_oDatabase.Parameters.Clear()
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="document_id", vValue:=nDocumentId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=lCompanyID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="Period_id", vValue:=v_lPeriodID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="currency_id", vValue:=nCurrencyID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="dtDocAccountingDate", vValue:=dtDocAccountingDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="AllocationBatchId", vValue:=lAllocationBatchId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=vResultArray(0, nAccountLoop), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="document_sequence", vValue:=nDocSequence, iDirection:=gPMConstants.PMEParameterDirection.PMParamInputOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="running_total", vValue:=crRunningTotal, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

                        ' Perform the SQL
                        m_lReturn = m_oDatabase.SQLAction( _
                            sSQL:="spu_ACT_DO_YearEND_Allocations", _
                            sSQLName:="spu_ACT_DO_YearEND_Allocations", _
                            bStoredProcedure:=True)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            RaiseError("m_oDatabase.SQLSelect", "sSQLName:=GetBranchIDsForYearEnd", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        'Get the return parameteres
                        nDocSequence = NullToInteger(m_oDatabase.Parameters.Item("document_sequence").Value)
                        cRunningTotal = cRunningTotal + ToSafeDecimal(m_oDatabase.Parameters.Item("running_total").Value)

                    Next
                    'If we have
                    If nDocSequence <> 1 Then

                        'Get retained profit account_id
                        nAccountID = 0

                        If gPMFunctions.NullToString(vMultiLedger) = "1" Then
                            m_lReturn = GetRetainedProfitAccountID(r_vAccountID:=nAccountID, v_vCompanyID:=lCompanyID)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("GetRetainedProfitAccountID", "Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                        Else
                            m_lReturn = GetRetainedProfitAccountID(r_vAccountID:=nAccountID)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("GetRetainedProfitAccountID", "Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                        End If

                        'Fail if no Retained Profit Account set up
                        If nAccountID = 0 Then
                            gPMFunctions.RaiseError("GetRetainedProfitAccountID", "Failed to find retained profit account", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        'Post the balancing journal to Retained Profit

                        m_lReturn = oDocumentPost.AddTransaction(v_lAccountID:=nAccountID, v_vDocumentSequence:=nDocSequence, v_iCurrencyID:=m_iCurrencyID, _
                                                                 v_cAmount:=cRunningTotal, v_cCurrencyAmount:=cRunningTotal, v_vdCurrencyBaseXRate:=1, _
                                                                 v_vOperatorID:=m_iUserID, v_vAccountingDate:=dtDocAccountingDate, v_vSpare:="")
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("oDocumentPost.AddTransaction", "v_lAccountID:=" & nAccountID, gPMConstants.PMELogLevel.PMLogError)
                        End If

                        'Set the posting status to Posted
                        nDocPostedStatus = gACTLibrary.ACTPostStatusPosted

                    End If

                    m_lReturn = m_oDatabase.SQLCommitTrans
                Next

            End If
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here
            If Not (oPMAutoNumber Is Nothing) Then

                oPMAutoNumber.Dispose()
                oPMAutoNumber = Nothing
            End If

            If Not (oDocumentPost Is Nothing) Then

                oDocumentPost.Dispose()
                oDocumentPost = Nothing
            End If
        End Try
        Return nResult
    End Function


    Private Function GetBranchIDsForYearEnd( _
    ByVal nPeriodId As Integer, _
    ByRef r_vBranches(,) As Object) As Long

        Const kMethodName As String = "GetBranchIDsForYearEnd"
        Dim nResult As Integer = 0
        Dim sSQL As String

        nResult = gPMConstants.PMEReturnCode.PMTrue
        sSQL = "spu_GetTransactions_summary_YearEnd"
        m_oDatabase.QueryTimeout = 0
        'Clear the parameters
        m_oDatabase.Parameters.Clear()
        m_lReturn = m_oDatabase.Parameters.Add(sName:="nPeriod_id", vValue:=nPeriodId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        ' Perform the SQL
        m_lReturn = m_oDatabase.SQLSelect( _
            sSQL:=sSQL, _
            sSQLName:=kMethodName, _
            bStoredProcedure:=True, _
            vResultArray:=r_vBranches, _
            lNumberRecords:=gPMConstants.PMAllRecords)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        Return nResult

    End Function


    Private Function CreateAllocationBatch(ByRef r_lAllocationBatchID As Long) As Long
        Dim nResult As Integer = 0
        nResult = gPMConstants.PMEReturnCode.PMTrue
        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("nAllocation_batch_id", r_lAllocationBatchID, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
            m_lReturn = .SQLAction("spu_ACT_Add_AllocationBatch", _
                                            "spu_ACT_Add_AllocationBatch", _
                                                True)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                r_lAllocationBatchID = m_oDatabase.Parameters.Item("nAllocation_batch_id").Value
                Return nResult
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End With
    End Function
End Class
