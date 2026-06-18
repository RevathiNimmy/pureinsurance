Option Strict Off
Option Explicit On
Imports System.Data
Imports System.IO
Imports System.Text
Imports SSP.Shared


<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 27/09/00
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a SIRRenewal.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 26/09/2003
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
    ' Chase Cycle
    Dim sOptionValue As String = ""
    Dim bChaseCycleEnabled As Boolean
    Dim bPaymentHubEnabled As Boolean = False
    Dim bProcessCashList As Boolean = False
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_oInsuranceFile As bSIRInsuranceFile.Services
    Private m_sUnderwritingOrAgency As String = ""

    'Error return for renewal that has already been accepted
    Const PM_FAILED_RENEWAL_STATUS As gPMConstants.PMEReturnCode = 60132

    Private m_oReport As Object

    Private m_sReportOutputLocation As String = ""
    Private m_lSelNewPolicyCnt As Integer
    Private m_lSelPartyCnt As Integer

    Private m_sRenSchedulePrinting As String = "" 'option number 1036
    Private m_sRenCertificatePrinting As String = "" 'option number 1037
    Private m_sRenDebitNotePrinting As String = "" 'option number 1038
    Private m_vAllowPayNowOption As Object
    Private m_vRenewalsData(,) As Object
    Private m_lPaymentAccountID As Integer
    Private m_iDebitAgainst As Integer
    Private m_vCreditTransactions As Object
    Private m_lCashListID As Integer
    Private m_lCashListItemID As Integer
    Private m_lTransactionID As Integer
    Private m_bIsMigratedPolicy As Boolean
    Private Const ACUserChoice As Integer = 1
    Private m_oDataSet As cGISDataSetControl.Application
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public Property SetManualReview As Boolean = False
    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

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

    'Deepak

    Public Property UnderWritingorAgency() As String
        Get
            Return m_sUnderwritingOrAgency
        End Get
        Set(ByVal Value As String)
            m_sUnderwritingOrAgency = Value
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
            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel




            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            'sj 21/01/2003 - start
            'PS104
            m_oInsuranceFile = New bSIRInsuranceFile.Services
            m_lReturn = m_oInsuranceFile.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))


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
                m_oInsuranceFile = Nothing
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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetRenewals
    '
    ' Description:
    '
    ' Changes: RWH(04/10/2000) Added Renewal Date & Product params.
    '          Thinh Nguyen 20/03/2002 - add filter by branch and select
    '                                    branch, account hander and lead agent
    '          VB 06/05/2005 PN20715: New parameter added for filtering the renewals
    '                                 on accessible branches by logged in user.
    ' ***************************************************************** '
    Public Function GetRenewals(ByRef r_vResultArray(,) As Object, ByRef v_lRunMode As Integer, ByRef v_lRenewalInsFileCnt As Integer, Optional ByVal v_sRenewalDate As String = "", Optional ByVal v_lProductId As Integer = 0, Optional ByVal v_lSourceID As Integer = 0, Optional ByVal v_iCompare As Integer = 0, Optional ByVal v_lLeadAgentCnt As Integer = 0, Optional ByVal v_lPartyCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim bAnd As Boolean
        Dim sTempStr As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Now get the Renewal details for the appropriate renewal statuses

            m_oDatabase.Parameters.Clear()

            sSQL = ""
            sSQL = sSQL & "SELECT rs.renewal_status_Cnt, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "p.description, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "rs.insurance_holder_cnt, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "pa.shortname, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "pt.code, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "i.insurance_ref, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "rs.renewal_insurance_file_cnt, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "(SELECT insurance_ref FROM insurance_file WHERE insurance_file_cnt = rs.renewal_insurance_file_cnt) ," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "i.insurance_folder_cnt, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "i.insurance_file_structure_id, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "rs.renewal_status_type_id, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "rst.description, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "rs.critical_date, " & Strings.ChrW(13) & Strings.ChrW(10)

            'RWH(04/10/2000) Extras added for Accept process.
            sSQL = sSQL & "rs.insurance_file_cnt, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "i.cover_start_date, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "i.expiry_date, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "Isnull(i.lead_agent_cnt,0) as 'lead_agent_cnt', " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "p.product_id, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "i.renewal_date, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "CASE IsNull(la.ShortName,'') WHEN '' THEN 'Direct'" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ELSE la.ShortName END LeadAgent, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "CASE IsNull(ah.ShortName,'') WHEN '' THEN 'None'" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ELSE ah.ShortName END AccHandler, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "s.code BranchCode " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & ",CASE WHEN EXISTS(SELECT * FROM Claim c WHERE i.insurance_ref = c.policy_number) THEN 'YES' Else 'NO' END, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "s.is_deleted Closed, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "pta.is_in_transfer_mode , pta.transfer_to_party_cnt, xferpa.shortname, p.is_true_monthly_policy, i.anniversary_copy, " & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & "i.branch_id, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "rs.renewal_exception_reason_id,rs.renewal_exception_notes,rer.description, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "i.renewal_product_id , i.Payment_method, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "Business_type.Code " & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & "FROM insurance_file i " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "JOIN renewal_status rs       ON rs.renewal_insurance_file_cnt = i.insurance_file_cnt " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "JOIN product p               ON p.product_id = rs.product_id " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "JOIN renewal_status_type rst ON rst.renewal_status_type_id = rs.renewal_status_type_id " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "JOIN party pa                ON pa.party_cnt = rs.insurance_holder_cnt " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "JOIN party_type pt           ON pa.party_type_id = pt.party_type_id " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "LEFT JOIN party la           ON la.party_cnt = rs.lead_agent_cnt " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "LEFT JOIN party ah           ON ah.party_cnt = i.account_handler_cnt " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "LEFT JOIN source s           ON s.source_id = i.source_id " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "LEFT JOIN Party_Agent pta    ON pta.party_cnt = rs.lead_agent_cnt " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "LEFT JOIN Party xferpa       ON xferpa.party_cnt = pta.transfer_to_party_cnt " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "LEFT JOIN renewal_exception_reason rer  ON rs.renewal_exception_reason_id = rer.renewal_exception_reason_id " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " LEFT JOIN business_type ON  i.business_type_id = business_type.business_type_id " & Strings.ChrW(13) & Strings.ChrW(10)

            Select Case v_lRunMode
                Case gPMConstants.ACRenewalModeAmend
                    sSQL = sSQL & WAnd(bAnd) & " (rs.renewal_status_type_id IN (" & "SELECT renewal_status_type_id FROM renewal_status_type " & "WHERE UPPER(code) NOT IN ('POLICYCHAN','UPDATE')))" & Strings.ChrW(13) & Strings.ChrW(10)

                Case gPMConstants.ACRenewalModeAccept
                    sSQL = sSQL & WAnd(bAnd) & " (rs.renewal_status_type_id IN(" & "SELECT renewal_status_type_id FROM renewal_status_type " & "WHERE UPPER(code) IN ('UPDATE')))" & Strings.ChrW(13) & Strings.ChrW(10)
            End Select

            If v_lRenewalInsFileCnt <> 0 Then
                sSQL = sSQL & WAnd(bAnd) & " rs.renewal_insurance_file_cnt = " & v_lRenewalInsFileCnt & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            'RWH(04/10/2000) Added Product & Renewal Date params.
            If v_lProductId <> 0 Then
                sSQL = sSQL & WAnd(bAnd) & " p.product_id = " & v_lProductId & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            'Thinh Nguyen 20/03/2002 (start) - add filter by branch
            If v_lSourceID <> 0 Then
                sSQL = sSQL & WAnd(bAnd) & " i.source_id = " & v_lSourceID & Strings.ChrW(13) & Strings.ChrW(10)
            Else
                sSQL = sSQL & WAnd(bAnd) & " s.source_id NOT IN (SELECT source_id FROM PMUser_Source WHERE user_id = " & m_iUserID & ")" & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            If v_lPartyCnt <> 0 Then
                sSQL = sSQL & WAnd(bAnd) & " la.Party_cnt = " & v_lPartyCnt & Strings.ChrW(13) & Strings.ChrW(10)
            End If
            'Thinh Nguyen 20/03/2002 (end) - add filter by branch

            'RWH(24/05/2001) Now that we are getting policy details from the renewal version
            'of the policy rather than the live one, we need to compare against cover_start_date
            'rather than renewal_date.
            If v_sRenewalDate <> "" Then
                'sj 10/10/2002 - start
                If v_iCompare = 1 Then
                    sSQL = sSQL & WAnd(bAnd) & " i.cover_start_date = {renewal_date}" & Strings.ChrW(13) & Strings.ChrW(10)
                Else
                    'sj 10/10/2002 - end
                    sSQL = sSQL & WAnd(bAnd) & " i.cover_start_date <= {renewal_date}" & Strings.ChrW(13) & Strings.ChrW(10)
                End If
                m_oDatabase.Parameters.Clear()

                'Done this way to ensure correct formatting of date.
                m_lReturn = m_oDatabase.Parameters.Add(sName:="renewal_date", vValue:=v_sRenewalDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            End If

            If v_lLeadAgentCnt <> 0 Then
                sSQL = sSQL & WAnd(bAnd) & " rs.lead_agent_cnt = " & v_lLeadAgentCnt & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & WAnd(bAnd) & " rs.renewal_status_type_id = " & gPMConstants.PMBRenewalStatusTypeAwaitBrokerTransfer & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            'RWH(06/02/2001) Limit to current policy.
            '     sSQL = sSQL & "AND i.insurance_file_status_id = " _
            ''                    & "(select insurance_file_status_id from insurance_file_status where code = 'REN')" & vbCrLf
            '     sSQL = sSQL & "AND i.insurance_file_type_id in" _
            ''                & "(select insurance_file_type_id from insurance_file_type where code in ('POLICY','MTA PERM'))"

            sSQL = sSQL & "ORDER BY i.renewal_date"

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GETRENEWALS", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRenewals Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRenewals", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Function WAnd(ByRef bAnd As Boolean) As String
        If bAnd Then
            Return "AND"
        Else
            bAnd = True
            Return "WHERE"
        End If
    End Function


    ' ***************************************************************** '
    ' Name: GetLapseReasons
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function GetLapseReasons(ByRef r_vLapseReasons(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Now get the Renwal details for the appropriate renewal statuses

            sSQL = ""
            sSQL = sSQL & "SELECT DISTINCT lr.lapsed_reason_id, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "lr.description " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM lapsed_reason lr " & Strings.ChrW(13) & Strings.ChrW(10)

            'AG 05/10/2004 - PN13722 - Added the condition to filter out deleted lapse reasons.
            sSQL = sSQL & "WHERE is_deleted = " & CStr(gSIRLibrary.knLapsedReasonsNotDeleted)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetLapsedReasons", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vLapseReasons)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLapseReasons Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLapseReasons", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Rerate
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function Rerate(ByRef lRenewalInsFileCnt As Integer, ByRef sFailureReason As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Insert code to run the rating script when it is written.

            'It is not yet known what the outputs are, only that a table will be  popoulated with
            'values including a rerate failure reason and rerate referral reason.
            'Current interface code assumes that this function will accept an insurance file cnt
            'of the renewal for input and output "" if the rerate went through ok and a string
            'with the reason as output if it didn't. I am currently assuming that a rerate failure
            'will return true from this function but with string populated.
            'A technical database or code error will return false from this function



            'This is an unexpected technical non business logic failure so this function fails
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Rerate", vApp:=ACApp, vClass:=ACClass, vMethod:="Rerate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ReRatePolicy (Private)
    '
    ' Description: ReRate Policy
    '
    ' History: 04/09/2000 Created (TN)
    '          16/11/2000 RWH - split getting risks into it's own method
    '
    ' ***************************************************************** '

    'Public Function ReRatePolicy(ByVal v_lInsuranceFileCnt As Long) As Long
    Public Function ReRatePolicy(ByVal v_lInsuranceFileCnt As Integer, ByVal v_vRiskIDArray(,) As Object) As Integer
        Dim result As Integer = 0

        'Dim vRiskIDArray As Variant
        Dim oTestHarness As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    m_oDatabase.Parameters.Clear
            '
            '    m_lReturn& = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", _
            ''                                                                    vValue:=v_lInsuranceFileCnt, _
            ''                                                                    iDirection:=PMParamInput, _
            ''                                                                    iDataType:=PMLong)
            '    If m_lReturn& <> PMTrue Then
            '        ReRatePolicy = PMFalse
            '        Exit Function
            '    End If
            '
            '    m_lReturn& = m_oDatabase.SQLSelect(sSQL:=ACSelRiskIDSQL, _
            ''                                                            ssqlname:=ACSelRiskIDName, _
            ''                                                            bstoredprocedure:=ACSelRiskIDStored, _
            ''                                                            vResultArray:=vRiskIDArray)
            '
            '    If m_lReturn& <> PMTrue Then
            '        ReRatePolicy = PMFalse
            '        Exit Function
            '    End If

            'Policy must have risk(s) attached to it to re-rate
            If Not Informations.IsArray(v_vRiskIDArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'set up the Test object and rerate each risk
            'TODO need to be checked
            'oTestHarness = New bSIRTestHarness.Business()
            oTestHarness = New Object()

            If oTestHarness Is Nothing Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSIRTestHarness object", vApp:=ACApp, vClass:=ACClass, vMethod:="ReRatePolicy", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If oTestHarness.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database)) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'loop thro and rerate each risk

            For lCount As Integer = 0 To v_vRiskIDArray.GetUpperBound(1)
                'Rerate policy for this risk

                If oTestHarness.Rerate(v_sDataModel:="RSA", v_lInsuranceFileCnt:=ToSafeInteger(v_lInsuranceFileCnt), v_lRiskID:=v_vRiskIDArray(0, lCount)) <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Log Error Message

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to re-rate Policy InsuranceFileCnt: " & v_lInsuranceFileCnt & " RiskID: " & CStr(v_vRiskIDArray(0, lCount)), vApp:=ACApp, vClass:=ACClass, vMethod:="ReRatePolicy", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    result = gPMConstants.PMEReturnCode.PMFalse

                    Exit For

                End If
            Next


            oTestHarness.Dispose()
            oTestHarness = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReRatePolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReRatePolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetRenewalStatus
    '
    ' Description:
    '
    ' History    : Kevin Renshaw (CMG) 02/04/2003 PN3078 Invite Print error
    ' ***************************************************************** '
    'developer guide no. 101
    Public Function SetRenewalStatus(ByRef v_lRenewalCnt As Object, ByRef v_iRenewalStatus As Object) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Now set the Renewal status to that supplied

            sSQL = ""
            sSQL = sSQL & "UPDATE renewal_status " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "SET renewal_status_type_id = " & v_iRenewalStatus & " " & Strings.ChrW(13) & Strings.ChrW(10)
            If v_iRenewalStatus = ACAwaitRenewalPrint Then
                sSQL = sSQL & ", is_invite_printed = 0 " & Strings.ChrW(13) & Strings.ChrW(10)
            End If
            sSQL = sSQL & "WHERE renewal_insurance_file_cnt = " & v_lRenewalCnt & Strings.ChrW(13) & Strings.ChrW(10)

            m_lReturn = BeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="SetRenewalStatus", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                m_lReturn = CommitTrans()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception

            'RWH(17/10/2000)
            m_lReturn = RollbackTrans()

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetRenewalStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetRenewalStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: LapseRenewal
    ' ***************************************************************** '

    Public Function LapseRenewal(ByRef v_lRenewalCnt As Integer, ByRef v_lLivePolicyCnt As Integer, ByRef v_lStatusId As Integer, ByRef v_lReasonID As Integer, ByRef v_sReasonDesc As String, Optional ByVal v_lPartyCnt As Integer = 0, Optional ByVal v_lInsFolderCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim bTransactionOpen As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMFalse
            bTransactionOpen = False

            m_lReturn = RunLapseRule(v_lLivePolicyCnt, v_lInsFolderCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If
            If m_oDatabase.SQLBeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            bTransactionOpen = True

            m_lReturn = DeleteRenewal(v_lRenewalCnt:=v_lRenewalCnt, v_lLivePolicyCnt:=v_lLivePolicyCnt, v_lStatusId:=v_lStatusId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            'set policy status to lapsed
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFolderCnt", vValue:=CStr(v_lInsFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdLapsedPolicySQL, sSQLName:=ACUpdLapsedPolicyName, bStoredProcedure:=ACUpdLapsedPolicyStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            '    'EventType -PolicyChange

            m_lReturn = CreateEvent(v_vEventCnt:=0, v_vPartyCnt:=v_lPartyCnt, v_vInsuranceFolderCnt:=v_lInsFolderCnt, v_vInsuranceFileCnt:=v_lLivePolicyCnt, v_vClaimCnt:=DBNull.Value, v_vDocumentCnt:=DBNull.Value, v_vOldAddressCnt:=DBNull.Value, v_vNewAddressCnt:=DBNull.Value, v_vCampaignId:=DBNull.Value, v_vDocumentType:=DBNull.Value, v_vReportType:=DBNull.Value, v_vEventType:=5, v_vUserId:=m_iUserID, v_vEventDate:=DateTime.Today, v_vDescription:="Lapsed renewal (" & v_sReasonDesc & ")")

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add event.", vApp:=ACApp, vClass:=ACClass, vMethod:="LapseRenewal", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'record that the live policy is now lapsed
            m_lReturn = LapsePolicy(v_lLivePolicyCnt, v_lReasonID, v_sReasonDesc, v_lLivePolicy:=gPMConstants.PMEReturnCode.PMTrue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'PN4538 - Start
            If CheckAndUpdateCommonRenewalDate(v_lInsuranceFileCnt:=v_lLivePolicyCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' if Chase Cycle enabled
            m_lReturn = CType(GetSystemOptionLite(v_iOptionNumber:=kSystemOptionChaseCycleEnabled, r_sOptionValue:=sOptionValue, v_iSourceID:=1), gPMConstants.PMEReturnCode)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                bChaseCycleEnabled = (sOptionValue = "1")
            End If

            If bChaseCycleEnabled Then
                ' Add a Chase Cycle item
                ' NB: if this policy is later moved onto instalments the Chase Cycle item
                ' will be deleted by the instalment process.
                m_sTransactionType = "RENLAP"
                m_lReturn = AddChaseCycleItem(v_lInsuranceFileCnt:=v_lLivePolicyCnt, v_sBusinessType:=m_sTransactionType)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add Chase Cycle Item", vApp:=ACApp, vClass:=ACClass, vMethod:="LapseRenewal", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
                m_sTransactionType = Nothing
            End If
            'PN4538 - Start
            If CheckAndUpdateCommonRenewalDate(v_lInsuranceFileCnt:=v_lLivePolicyCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If
            'PN4538 - End
            result = gPMConstants.PMEReturnCode.PMTrue

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LapseRenewal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LapseRenewal", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally
            If bTransactionOpen Then
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CommitTrans()
                Else
                    m_lReturn = RollbackTrans()
                End If
            End If
        End Try

        Return result
    End Function
    ''' <summary>
    ''' DeleteRenewal
    ''' </summary>
    ''' <param name="v_lRenewalCnt"></param>
    ''' <param name="v_lLivePolicyCnt"></param>
    ''' <param name="v_lStatusId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeleteRenewal(ByRef v_lRenewalCnt As Object, ByRef v_lLivePolicyCnt As Object, ByRef v_lStatusId As Object) As Integer

        Dim nResult As Integer
        Dim sSQL As String = ""
        Dim oRenSelection As bSIRRenSelection.Business
        Dim dtRenPolDetails As New DataTable
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            oRenSelection = New bSIRRenSelection.Business
            m_lReturn = oRenSelection.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'delete work tasks

            m_lReturn = oRenSelection.DeleteWorkTask(v_sKeyName:="insurance_file_cnt", v_sKeyValue:=v_lRenewalCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = GetRenewalPolicyDetails(v_lInsuranceFileCnt:=v_lRenewalCnt, dtResult:=dtRenPolDetails)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                DeleteRenewal = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
            'delete renewal version of the policy
            If Not (dtRenPolDetails IsNot Nothing AndAlso dtRenPolDetails.Rows.Count > 0) Then
                m_lReturn = oRenSelection.DeletePolicy(v_lRenewalCnt)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            oRenSelection.Dispose()
            oRenSelection = Nothing

            m_lReturn = BeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Remove any records in 'Last_Print_Run' linked to our Renewal_Status record.
            m_lReturn = DeleteLastPrintRun(v_lStatusId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = RollbackTrans()
                Return nResult
            End If

            m_lReturn = DeleteRenewalStatus(v_lStatusId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = UpdStatusOfLivePolicy(v_lLivePolicyCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'got this far - commit
            m_lReturn = CommitTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch excep As System.Exception

            'RWH(17/10/2000)
            m_lReturn = RollbackTrans()

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteRenewal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRenewal", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteRenewalStatus
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    'developer guide no. 101
    Public Function DeleteRenewalStatus(ByRef v_lRenewalStatusID As Object) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Now set the Renewal status to that supplied

            sSQL = "DELETE Renewal_status " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE renewal_status_cnt = " & v_lRenewalStatusID & Strings.ChrW(13) & Strings.ChrW(10)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="DeleteRenewalStatus", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteRenewalStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRenewalStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ''' <summary>
    ''' LapsePolicy
    ''' </summary>
    ''' <param name="v_lPolicyCnt"></param>
    ''' <param name="v_lLapseId"></param>
    ''' <param name="v_sLapseDesc"></param>
    ''' <param name="v_lLivePolicy"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function LapsePolicy(ByRef v_lPolicyCnt As Integer, ByRef v_lLapseId As Integer, ByRef v_sLapseDesc As String, ByRef v_lLivePolicy As Integer) As Integer
        Const kMethodName As String = "LapsePolicy"
        Dim nResult As Integer
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            'Update the policy to be lapsed.

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="PolicyCnt", vValue:=v_lPolicyCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(ACClass, kMethodName & " Fails to Add parameter PolicyCnt", gPMConstants.PMELogLevel.PMLogError)
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="LapseId", vValue:=v_lLapseId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(ACClass, kMethodName & " Fails to Add parameter LapseId", gPMConstants.PMELogLevel.PMLogError)
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="LivePolicy", vValue:=v_lLivePolicy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(ACClass, kMethodName & " Fails to Add parameter LivePolicy", gPMConstants.PMELogLevel.PMLogError)
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="LapseDesc", vValue:=v_sLapseDesc, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(ACClass, kMethodName & " Fails to Add parameter LapseDesc", gPMConstants.PMELogLevel.PMLogError)
            End If

            If m_bIsMigratedPolicy = True Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="IsMigratedPolicy", vValue:=1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Check for any error
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(ACClass, kMethodName & " Fails to Add parameter IsMigratedPolicy", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACUpdRenewalPolicyDetailsSQL,
                                                     sSQLName:=ACUpdRenewalPolicyDetailsName,
                                                     bStoredProcedure:=ACUpdRenewalPolicyDetailsStored)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(ACClass, kMethodName & " Fails to Get Renewal Policy Details", gPMConstants.PMELogLevel.PMLogError)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch excep As System.Exception



            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LapsePolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LapsePolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CreateEvent
    '
    ' Description: add record to event_log
    '
    ' History: 04/07/2001 TN - Created.
    '
    ' ***************************************************************** '
    'developer guide no. 101
    Public Function CreateEvent(Optional ByVal v_vEventCnt As Object = Nothing, Optional ByVal v_vPartyCnt As Object = Nothing, Optional ByVal v_vInsuranceFolderCnt As Object = Nothing, Optional ByVal v_vInsuranceFileCnt As Object = Nothing, Optional ByVal v_vClaimCnt As Object = Nothing, Optional ByVal v_vDocumentCnt As Object = Nothing, Optional ByVal v_vOldAddressCnt As Object = Nothing, Optional ByVal v_vNewAddressCnt As Object = Nothing, Optional ByVal v_vCampaignId As Object = Nothing, Optional ByVal v_vDocumentType As Object = Nothing, Optional ByVal v_vReportType As Object = Nothing, Optional ByVal v_vEventType As Object = Nothing, Optional ByVal v_vUserId As Object = Nothing, Optional ByVal v_vEventDate As Date = #12/30/1899#, Optional ByVal v_vDescription As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oEvent As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Informations.IsNothing(v_vEventCnt) Then
                v_vEventCnt = 0
            End If

            If Informations.IsNothing(v_vPartyCnt) Then

                v_vPartyCnt = DBNull.Value
            End If

            If Informations.IsNothing(v_vInsuranceFolderCnt) Then

                v_vInsuranceFolderCnt = DBNull.Value
            End If

            If Informations.IsNothing(v_vInsuranceFileCnt) Then

                v_vInsuranceFileCnt = DBNull.Value
            End If

            If Informations.IsNothing(v_vClaimCnt) Then

                v_vClaimCnt = DBNull.Value
            End If

            If Informations.IsNothing(v_vDocumentCnt) Then

                v_vDocumentCnt = DBNull.Value
            End If

            If Informations.IsNothing(v_vOldAddressCnt) Then

                v_vOldAddressCnt = DBNull.Value
            End If

            If Informations.IsNothing(v_vNewAddressCnt) Then

                v_vNewAddressCnt = DBNull.Value
            End If

            If Informations.IsNothing(v_vCampaignId) Then

                v_vCampaignId = DBNull.Value
            End If

            If Informations.IsNothing(v_vDocumentType) Then

                v_vDocumentType = DBNull.Value
            End If

            If Informations.IsNothing(v_vReportType) Then

                v_vReportType = DBNull.Value
            End If

            If Informations.IsNothing(v_vEventType) Then

                v_vEventType = Nothing
            End If

            If Informations.IsNothing(v_vUserId) Then
                v_vEventType = m_iUserID
            End If

            If Informations.IsNothing(v_vEventDate) Then
                v_vEventDate = DateTime.Today
            End If

            If Informations.IsNothing(v_vDescription) Then

                v_vDescription = DBNull.Value
            End If

            'sj 21/01/2003 - start
            'PS104
            oEvent = New bSIREvent.Business
            result = oEvent.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))


            'CreateEvent = gPMComponentServices.CreateBusinessObject(r_oObject:=oEvent, _
            'v_sClassName:="bSIREvent.Business", _
            'v_sCallingAppName:=ACApp, _
            'v_sUserName:=m_sUsername$, _
            'v_sPassword:=m_sPassword$, _
            'v_iUserID:=m_iUserID%, _
            'v_iSourceID:=m_iSourceID%, _
            'v_iLanguageId:=m_iLanguageID%, _
            'v_iCurrencyID:=m_iCurrencyID%, _
            'v_iLogLevel:=m_iLogLevel%)
            'sj 21/01/2003 - end

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                'Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSIREvent.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'Remove Component Services

            'EventType -PolicyChange

            result = oEvent.DirectAdd(vEventCnt:=v_vEventCnt, vPartyCnt:=v_vPartyCnt, vInsuranceFolderCnt:=v_vInsuranceFolderCnt, vInsuranceFileCnt:=v_vInsuranceFileCnt, vClaimCnt:=v_vClaimCnt, vDocumentCnt:=v_vDocumentCnt, vOldAddressCnt:=v_vOldAddressCnt, vNewAddressCnt:=v_vNewAddressCnt, vCampaignId:=v_vCampaignId, vDocumentType:=v_vDocumentType, vReportType:=v_vReportType, vEventType:=v_vEventType, vUserId:=v_vUserId, vEventDate:=CType(v_vEventDate, Date), vDescription:=v_vDescription)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                'Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add event.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'Terminate the object and clear it up

            oEvent.Dispose()

            oEvent = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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
    ' Name: CheckMandatory (Private)
    '
    ' Description: Check Mandatory parameters have been passed.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CheckMandatory) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CheckMandatory(Optional ByRef vAddressCnt As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vAddressID As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vLastModified As Object = Nothing) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' {* USER DEFINED CODE (Begin) *}
    '

    'If (Informations.IsNothing(vSourceID)) Or (Object.Equals(vSourceID, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '

    'If (Informations.IsNothing(vAddressID)) Or (Object.Equals(vAddressID, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '

    'If (Informations.IsNothing(vAddress1)) Or (Object.Equals(vAddress1, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '

    'If (Informations.IsNothing(vPostalCode)) Or (Object.Equals(vPostalCode, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '

    'If (Informations.IsNothing(vCountryID)) Or (Object.Equals(vCountryID, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '

    'If (Informations.IsNothing(vCreatedByID)) Or (Object.Equals(vCreatedByID, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '

    'If (Informations.IsNothing(vDateCreated)) Or (Object.Equals(vDateCreated, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '
    '
    ' {* USER DEFINED CODE (End) *}
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
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckMandatory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
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



    ' ***************************************************************** '
    '
    ' Name: DeleteOldPolicy
    '
    ' Description: Used by Accept Renewals process to remove previous
    '               policy.
    '
    ' History: 04/10/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function DeleteOldPolicy(ByRef v_lInsuranceFileCnt As Integer, ByRef v_lRenewalStatusCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Start transaction.
            m_lReturn = BeginTrans()

            sSQL = "UPDATE insurance_file " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "SET policy_ignore = 1" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE insurance_file_cnt = " & CStr(v_lInsuranceFileCnt)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="SetInsuranceFileToBeIgnored", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = RollbackTrans()
                Return result
            End If

            'Remove any records in 'Last_Print_Run' linked to our Renewal_Status record.
            m_lReturn = DeleteLastPrintRun(v_lRenewalStatusCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = RollbackTrans()
                Return result
            End If

            'Remove record in 'Renewal_Status'.
            m_lReturn = DeleteRenewalStatus(v_lRenewalStatusCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = RollbackTrans()
                Return result

            End If

            'If everything has succeeded then Commit changes.
            m_lReturn = CommitTrans()

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteOldPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteOldPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdatePolicyDetails
    '
    ' Description:
    '
    ' History: 04/10/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function UpdatePolicyDetails(ByRef v_lInsuranceFileCnt As Integer, Optional ByRef v_sNewPolicyRef As String = "", Optional ByRef v_sNewStartDate As String = "", Optional ByRef v_sNewExpiryDate As String = "", Optional ByRef v_lInsuranceFileType As Integer = 0, Optional ByRef v_lInsuranceFileStatus As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oInsuranceFile.InsuranceFileCnt = v_lInsuranceFileCnt

            m_oInsuranceFile.InsuranceRef = v_sNewPolicyRef

            m_oInsuranceFile.CoverStartDate = v_sNewStartDate

            m_oInsuranceFile.ExpiryDate = v_sNewExpiryDate

            m_lReturn = m_oInsuranceFile.UpdatePolicy
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePolicyDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePolicyDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteLastPrintRun
    '
    ' Description: Removes any records from the Last_Print_Run table
    '               which link to the Renewal_Status record we wish
    '               to remove.
    '
    ' ***************************************************************** '
    Public Function DeleteLastPrintRun(ByRef v_lRenewalStatusID As Integer) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "DELETE Last_Print_Run " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE renewal_status_cnt = " & CStr(v_lRenewalStatusID) & Strings.ChrW(13) & Strings.ChrW(10)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="DeleteLastPrintRun", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteLastPrintRun Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteLastPrintRun", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'PK
    Public Function DeleteCreditControlItem(ByRef v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteCreditControlItemSQL, sSQLName:=ACDeleteCreditControlItemName, bStoredProcedure:=ACDeleteCreditControlItemStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteCreditControlItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteCreditControlItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetRisks
    '
    ' Description: Gets Risks associated with a policy.
    '
    ' History: 16/11/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetRisks(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vRiskIDArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelRiskCntSQL, sSQLName:=ACSelRiskCntName, bStoredProcedure:=ACSelRiskCntStored, vResultArray:=r_vRiskIDArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    'Policy must have risk(s) attached to it
            '    If Not Informations.IsArray(vRiskIDArray) Then
            ''        GetRisks = PMFalse
            '        Exit Function
            '    End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRisks", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AcceptRenewal
    '
    ' Description:
    '
    ' History: 21/11/2000 RWH - Created.
    '
    ' ***************************************************************** '
    'developer guide no 101
    Public Function AcceptRenewal(ByRef v_lOldInsuranceFileCnt As Object,
                                  ByRef v_lNewInsuranceFileCnt As Object,
                                  ByRef v_lRenewalStatusID As Object,
                                  Optional ByRef v_sNewPolicyRef As Object = Nothing,
                                  Optional ByRef v_sNewStartDate As Object = Nothing,
                                  Optional ByRef v_sNewExpiryDate As Object = Nothing,
                                  Optional ByRef r_sFailureMessage As String = "") As Integer

        Dim result As Integer = 0
        Dim iReturn As Integer
        Dim iInsFileStatusId, iInsFileTypeId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add("old_insurance_file_cnt", v_lOldInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add("old_insurance_file_status_id", iInsFileStatusId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add("new_insurance_file_cnt", v_lNewInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add("new_insurance_file_type_id", iInsFileTypeId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)


            If v_sNewPolicyRef <> "" Then
                m_lReturn = m_oDatabase.Parameters.Add("new_insurance_ref", v_sNewPolicyRef, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            End If
            If v_sNewStartDate <> "" Then
                m_lReturn = m_oDatabase.Parameters.Add("new_cover_start_date", ToSafeDate(v_sNewStartDate), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            End If
            If v_sNewExpiryDate <> "" Then
                m_lReturn = m_oDatabase.Parameters.Add("new_expiry_date", ToSafeDate(v_sNewStartDate), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="FailureMessage", vValue:="", iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ReturnValue", vValue:=0, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction("spu_SIR_AcceptRenewal", "spu_SIR_AcceptRenewal", True)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailureMessage = NullToString(m_oDatabase.Parameters.Item("FailureMessage").Value)
                iReturn = NullToLong(m_oDatabase.Parameters.Item("ReturnValue").Value)
                m_oInsuranceFile.InsuranceFileCnt = v_lOldInsuranceFileCnt

                If r_sFailureMessage <> "" Then
                    Exit Function
                End If
                If iReturn = PM_FAILED_RENEWAL_STATUS Then
                    AcceptRenewal = PM_FAILED_RENEWAL_STATUS
                    Exit Function
                End If
            ElseIf (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                AcceptRenewal = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_oInsuranceFile.InsuranceFileCnt = v_lOldInsuranceFileCnt
            m_oInsuranceFile.GetDetails()

            'Changes done by Krishna Nand PN: 70509 Dated: 31/03/2010
            m_lReturn = UpdateCurrencyToInsuranceFile()
            m_oInsuranceFile.InsuranceFileStatusID = iInsFileStatusId

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'End of Changes

            ' m_oInsuranceFile = Nothing

            m_lReturn = UpdateInsuranceFileSystem(v_lNewInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'RWH(24/05/2001) Need to remove Last Print Run before deleting Renewal_Status table.
            If DeleteLastPrintRun(v_lRenewalStatusID:=v_lRenewalStatusID) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'delete record out of renewal status table
            If DeleteRenewalStatus(v_lRenewalStatusID:=v_lRenewalStatusID) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If ProcessUpdatePolicy(v_lInsuranceFileCnt:=v_lNewInsuranceFileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If DeleteCreditControlItem(v_lInsuranceFileCnt:=v_lNewInsuranceFileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If UpdateRenewalCount(v_lInsuranceFileCnt:=v_lNewInsuranceFileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'PN4538 - start
            If CheckAndUpdateCommonRenewalDate(v_lInsuranceFileCnt:=v_lNewInsuranceFileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then

                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to update common renewal date"
                End If
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
            'PN4538 - end

            Dim oaPFPlan(,) As Object = Nothing
            m_lReturn = GetPFPlanForInsFile(v_lNewInsuranceFileCnt, oaPFPlan)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailureMessage = "Failed to check If PF paln is attached to Insurance file"
                Return m_lReturn
            End If
            'If PF plan exists for insurance file
            If oaPFPlan IsNot Nothing AndAlso Informations.IsArray(oaPFPlan) AndAlso oaPFPlan.Length > 0 Then
                'Get the payment method type
                Dim sPaymentMethod As String = String.Empty
                m_lReturn = GetPaymentMethod(v_lNewInsuranceFileCnt, sPaymentMethod)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureMessage = "Failed to get payment method for insurance file"
                    Return m_lReturn
                End If
                sPaymentMethod = ToSafeString(sPaymentMethod).Trim().ToUpper()
                If sPaymentMethod <> "INSTALMENTS" AndAlso sPaymentMethod <> "INSTALMENT" AndAlso sPaymentMethod <> "PREMIUMFINANCE" AndAlso
                    sPaymentMethod <> "DIRECT DEBIT" AndAlso sPaymentMethod <> "CREDIT CARD" Then
                    Dim obSIRPremiumFinance As bSIRPremiumFinance.Business = New bSIRPremiumFinance.Business
                    m_lReturn = obSIRPremiumFinance.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureMessage = "Failed to initilize bSIRPremiumFinance.Business"
                        Return m_lReturn
                    End If
                    m_lReturn = obSIRPremiumFinance.DeletePlanForOneInsFile(v_lNewInsuranceFileCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureMessage = "Failed to Delete PF plan for insurance file"
                        Return m_lReturn
                    End If
                    m_lReturn = CreateEvent(v_vEventCnt:=0, v_vPartyCnt:=m_oInsuranceFile.InsuredCnt, v_vInsuranceFolderCnt:=m_oInsuranceFile.InsuranceFolderCnt, v_vInsuranceFileCnt:=v_lNewInsuranceFileCnt,
                                            v_vClaimCnt:=DBNull.Value, v_vDocumentCnt:=DBNull.Value, v_vOldAddressCnt:=DBNull.Value, v_vNewAddressCnt:=DBNull.Value,
                                            v_vCampaignId:=DBNull.Value, v_vDocumentType:=DBNull.Value, v_vReportType:=DBNull.Value, v_vEventType:=PMBConst.PMBEventPolChange,
                                            v_vUserId:=m_iUserID, v_vEventDate:=DateTime.Today, v_vDescription:="Instalment plan deleted")
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureMessage <> "GGGGGRRRRRR" Then
                            r_sFailureMessage = "Failed to create event"
                        End If
                        Return m_lReturn
                    End If
                    obSIRPremiumFinance.Dispose()
                    obSIRPremiumFinance = Nothing
                End If
            End If
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AcceptRenewal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AcceptRenewal", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetInsuranceFileStatusId
    '
    ' Description:
    '
    ' History: 21/11/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function GetInsuranceFileStatusId(ByRef v_sCode As String, ByRef r_lInsFileStatusId As Integer) As Integer

        Dim result As Integer = 0
        Dim vResult(,) As Object = Nothing

        Const sSQL As String = "SELECT insurance_file_status_id FROM Insurance_File_Status " & "WHERE code = {code}"



        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=v_sCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="", bStoredProcedure:=False, vResultArray:=vResult)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Informations.IsArray(vResult) Then

            r_lInsFileStatusId = CInt(vResult(0, 0))
        Else
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetInsuranceFileTypeId
    '
    ' Description:
    '
    ' History: 21/11/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function GetInsuranceFileTypeId(ByRef v_sCode As String, ByRef r_lInsFileTypeId As Integer) As Integer
        Dim result As Integer = 0
        Dim vResult(,) As Object = Nothing

        Const sSQL As String = "SELECT insurance_file_type_id FROM Insurance_File_Type " & "WHERE code = {code}"



        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=v_sCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="", bStoredProcedure:=False, vResultArray:=vResult)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Informations.IsArray(vResult) Then

            r_lInsFileTypeId = CInt(vResult(0, 0))
        Else
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: CreateInsuranceFileObject
    '
    ' Description:
    '
    ' History: 22/11/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function CreateInsuranceFileObject() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'sj 21/01/2003 - start
        'PS104
        m_oInsuranceFile = New bSIRInsuranceFile.Services
        m_lReturn = m_oInsuranceFile.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))
        'm_lReturn& = gPMComponentServices.CreateBusinessObject(m_oInsuranceFile, _
        '"bSIRInsuranceFile.Services", _
        'm_sCallingAppName$, _
        'm_sUsername$, _
        'm_sPassword$, _
        'm_iUserID%, _
        'm_iSourceID%, _
        'm_iLanguageID%, _
        'm_iCurrencyID%, _
        'm_iLogLevel%) ', _
        'g_vDatabase)
        'sj 21/01/2003 - end

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: CreateBusinessObject
    '
    ' Description:
    '
    ' History: 16/02/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function CreateBusinessObject(ByRef r_oObject As Object, ByVal v_sClassName As String) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue




        Return gPMComponentServices.CreateBusinessObject(r_oObject:=r_oObject, v_sClassName:=v_sClassName, v_sCallingAppName:=m_sCallingAppName, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdStatusOfLivePolicy
    '
    ' Description:
    '
    ' History: 26/02/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function UpdStatusOfLivePolicy(ByRef v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        sSQL = "UPDATE Insurance_File" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "SET insurance_file_status_id = Null" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "WHERE insurance_file_cnt = " & CStr(v_lInsuranceFileCnt) & " AND insurance_file_status_id <> 1"


        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="UpdateInsFileStatus", bStoredProcedure:=False)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateRenewalCount
    '
    ' Description:
    '
    ' History: 09/07/2001 TN - Created.
    '
    ' ***************************************************************** '
    Public Function UpdateRenewalCount(Optional ByVal v_lInsuranceFileCnt As Integer = 0, Optional ByVal v_lInsuranceFolderCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_lInsuranceFileCnt = 0 And v_lInsuranceFolderCnt = 0 Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Insurance File Count And Insurance Folder Count Are Zero", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRenewalCount", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            result = m_oDatabase.SQLBeginTrans()

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            result = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            result = m_oDatabase.SQLAction(sSQL:=ACUpdRenewalCountSQL, sSQLName:=ACUpdRenewalCountName, bStoredProcedure:=ACUpdRenewalCountStored)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            result = m_oDatabase.SQLCommitTrans()

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRenewalCount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRenewalCount", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: IsQuoted
    '
    ' Description: are all risks quoted
    '
    ' History: 09/07/2001 TN - Created.
    '
    ' ***************************************************************** '
    Public Function IsQuoted(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lResult As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACIsQuotedSQL, sSQLName:=ACIsQuotedName, bStoredProcedure:=ACIsQuotedStored, vResultArray:=vResultArray, bKeepNulls:=True)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            If CDbl(vResultArray(0, 0)) = 0 Then

                r_lResult = gPMConstants.PMEReturnCode.PMTrue
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsQuoted Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsQuoted", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateInsuranceFileSystem
    '
    ' Description:
    '
    ' History: 31/08/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function UpdateInsuranceFileSystem(ByRef v_lInsuranceFileCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim vResultArray1(,) As Object
        Dim lTransTypeId As Integer
        Dim sTransTypeDescription As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get the required transaction details first.
            sSQL = "SELECT transaction_type_id, description" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM Transaction_Type" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE LTRIM(RTRIM(code)) = 'REN'"

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetTransactionTypeDetails", bStoredProcedure:=False, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            lTransTypeId = CInt(vResultArray(0, 0))

            sSQL = "Select last_trans_description from Insurance_File_System Where insurance_file_cnt = " & CStr(v_lInsuranceFileCnt)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="SelectLastTransDescription", bStoredProcedure:=False, vResultArray:=vResultArray1)

            If Informations.IsArray(vResultArray1) Then
                If (CStr(vResultArray1(0, 0).contains("'"))) Then
                    sTransTypeDescription = CStr(vResultArray1(0, 0)).ToString.Replace("'", "''")
                Else
                    sTransTypeDescription = vResultArray1(0, 0)
                End If
            End If

            If ((sTransTypeDescription Is Nothing) OrElse (sTransTypeDescription.Trim = "")) Then
                If (CStr(vResultArray(1, 0).contains("'"))) Then
                    sTransTypeDescription = CStr(vResultArray(1, 0)).ToString.Replace("'", "''")
                Else
                    sTransTypeDescription = vResultArray(1, 0)
                End If
            End If


            sSQL = "UPDATE Insurance_File_System" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "SET modified_by_id = " & CStr(m_iUserID) & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & ", last_modified = {last_modified}" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & ",last_trans_date = {last_trans_date}" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & ",last_trans_type_id = " & CStr(lTransTypeId) & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & ",last_trans_description = '" & sTransTypeDescription & "'" & Strings.ChrW(13) & Strings.ChrW(10)
            'last_trans_debit_credit
            sSQL = sSQL & "FROM insurance_file_system ifs" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE ifs.insurance_file_cnt = " & CStr(v_lInsuranceFileCnt)

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="last_modified", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="last_trans_date", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="UpdateInsuranceFileSystem", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateInsuranceFileSystem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateInsuranceFileSystem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    '*****************************************************************
    ' Name: GetDepositAmount
    '
    ' Description: Get the deposit amount for the renewal policy
    '
    ' History: PW060203 - created (PS209)
    '*****************************************************************
    'developer guide no. 101
    Public Function GetDepositAmount(ByVal v_lInsuranceFileCnt As Object, ByRef r_cDepositAmount As Object) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the database parameters
            m_oDatabase.Parameters.Clear()

            ' Add the insurance file count parameter

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Run the Stored Procedure
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDepositAmountSQL, sSQLName:=ACGetDepositAmountName, bStoredProcedure:=ACGetDepositAmountStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the deposit amount, if it exists
            If Informations.IsArray(vResultArray) Then

                'developer guide no. 101
                r_cDepositAmount = vResultArray(0, 0)
            Else
                r_cDepositAmount = 0
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDepositAmount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDepositAmount", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '************************************************************************
    ' Name : IsInstalment
    '
    ' Desc : check to see if policy has a record in pfPremiumFiance
    '
    ' Hist : Thinh Nguyen 26/04/2002 (created)
    '************************************************************************
    Public Function IsInstalment(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelIsInstalmentSQL, sSQLName:=ACSelIsInstalmentName, bStoredProcedure:=ACSelIsInstalmentStored, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsInstalment Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsInstalment", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '************************************************************************
    ' Name : GetPaymentMethod
    '
    ' Desc : get payment method for this policy
    '
    ' Hist : Thinh Nguyen 26/04/2002 (created)
    '************************************************************************
    Public Function GetPaymentMethod(ByVal v_lInsuranceFileCnt As Integer, ByRef r_sPaymentMethod As String) As Integer

        Dim result As Integer = 0
        Dim vResult(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelPaymentMethodSQL, sSQLName:=ACSelPaymentMethodName, bStoredProcedure:=ACSelPaymentMethodStored, vResultArray:=vResult)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResult) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            r_sPaymentMethod = CStr(vResult(0, 0))

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPaymentMethod Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPaymentMethod", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name : GetDocTypeID
    '
    ' Desc : get document type id using document type code
    '
    ' Thinh Nguyen 01/10/2002 - Created
    '
    ' ***************************************************************** '
    Public Function GetDocTypeID(ByVal v_sDocCode As String, ByRef r_lDocTypeId As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add(sName:="code", vValue:=v_sDocCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'execute SQL statement
            result = m_oDatabase.SQLSelect(sSQL:=ACSelDocTypeIDSQL, sSQLName:=ACSelDocTypeIDName, bStoredProcedure:=ACSelDocTypeIDStored, vResultArray:=vResultArray)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            r_lDocTypeId = CInt(vResultArray(0, 0))

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocTypeID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocTypeID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function GetDocTemplateTypeID(ByVal v_sDocCode As String, ByRef r_lDocTemplateTypeId As Integer, ByRef r_lDocTypeID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add(sName:="code", vValue:=v_sDocCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'execute SQL statement
            result = m_oDatabase.SQLSelect(sSQL:=ACSelDoctTemplateTypeIDSQL, sSQLName:=ACSelDoctTemplateTypeIDName, bStoredProcedure:=ACSelDoctTemplateTypeIDStored, vResultArray:=vResultArray)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            r_lDocTemplateTypeId = CInt(vResultArray(0, 0))
            r_lDocTypeID = CInt(vResultArray(1, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocTemplateTypeID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocTemplateTypeID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function


    '*****************************************************************************
    ' 1. get policy version which is in renewal and renewal version of policy and renewal status count
    ' using v_lInsuranceFileCnt (any version of insurance file count will do)
    ' 2. delete renewal version of policy change status of policy in renewal back to live and delete all related details
    '*****************************************************************************
    Public Function DeletePolicyFromRenewal(ByVal v_lInsuranceFileCnt As Integer, Optional ByVal v_bRetainAnniversaryCopy As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim lInsuranceFileCnt, lRenewalInsuranceFileCnt, lRenewalStatusCnt As Integer
        Dim nIsAnniversaryCopy As Integer
        Dim llBound, lUBound As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        Try


            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetUnderRenewalPolicyVersionsSQL, sSQLName:=ACGetUnderRenewalPolicyVersionsName, bStoredProcedure:=ACGetUnderRenewalPolicyVersionsStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
                Return result
            End If

            '*************************************************
            ' NB: True monthly policies may have two renewals
            ' that will need to be deleted,
            ' 1. the next renewal in the current TMP cycle
            ' 2. the anniversary copy of the renewal for the next TMP cycle
            ' so ensure that all Renewals are deleted
            '*************************************************
            If (v_bRetainAnniversaryCopy = True) Then

                llBound = vResultArray.GetLowerBound(1)

                lUBound = vResultArray.GetUpperBound(1)

                For lRenewal As Integer = llBound To lUBound

                    lInsuranceFileCnt = CLng(vResultArray(0, lRenewal))
                    lRenewalStatusCnt = CLng(vResultArray(1, lRenewal))
                    lRenewalInsuranceFileCnt = CLng(vResultArray(2, lRenewal))
                    nIsAnniversaryCopy = CInt(vResultArray(3, lRenewal))
                    If (v_bRetainAnniversaryCopy = True And nIsAnniversaryCopy = 0) Then
                        DeletePolicyFromRenewal = DeleteRenewal(v_lRenewalCnt:=lRenewalInsuranceFileCnt,
                                                                v_lLivePolicyCnt:=lInsuranceFileCnt,
                                                                v_lStatusId:=lRenewalStatusCnt)
                    End If
                Next
            Else

                llBound = vResultArray.GetLowerBound(1)

                lUBound = vResultArray.GetUpperBound(1)

                For lRenewal As Integer = llBound To lUBound


                    lInsuranceFileCnt = CInt(vResultArray.GetValue(0, lRenewal))

                    lRenewalStatusCnt = CInt(vResultArray(1, lRenewal))

                    lRenewalInsuranceFileCnt = CInt(vResultArray(2, lRenewal))

                    result = DeleteRenewal(v_lRenewalCnt:=lRenewalInsuranceFileCnt, v_lLivePolicyCnt:=lInsuranceFileCnt, v_lStatusId:=lRenewalStatusCnt)
                Next
            End If

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete policy from renewal - insurance_file_cnt : " & v_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="DeletePolicyFromRenewal", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)


        Finally

        End Try
        Return result
    End Function

    '*****************************************************************************
    'is the flag midnight renewal set for this policy
    '*****************************************************************************
    Private Function IsMidnightRenewal(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lIsMidNightRenewal As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing


        result = gPMConstants.PMEReturnCode.PMTrue
        r_lIsMidNightRenewal = gPMConstants.PMEReturnCode.PMFalse

        m_oDatabase.Parameters.Clear()

        If m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            Return result
        End If

        If m_oDatabase.SQLSelect(sSQL:=ACSelIsMidNightRenewalSQL, sSQLName:=ACSelIsMidNightRenewalName, bStoredProcedure:=ACSelIsMidNightRenewalStored, vResultArray:=vResultArray, bKeepNulls:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMFalse
            Return result

        End If

        If Informations.IsArray(vResultArray) Then
            r_lIsMidNightRenewal = gPMFunctions.NullToLong(vResultArray(0, 0))
        Else
            result = gPMConstants.PMEReturnCode.PMNotFound
        End If


        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetAllUserBranches
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 17-03-2005 : PN19562
    ' ***************************************************************** '
    Public Function GetAllUserBranches(ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetAllUserBranches"


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = AddInputParameter(v_sName:="user_id", v_vValue:=m_iUserID, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=kGetAllUserBranchesSQL, sSQLName:=kGetAllUserBranchesName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetAllUserBranchesSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: AddInputParameter
    '
    ' Parameters: v_sName   : Parameter Name
    '             v_vValue  : Parameter Value
    '             v_iType   : Parameter DataType
    '
    ' Description: Adds an input parameter to the database parameters
    '
    ' History:
    '           Created : MEvans : 18-12-2002 : 202
    ' ***************************************************************** '
    Private Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "AddInputParameter"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Add Parameter to database object

        If m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=CStr(v_vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType) <> gPMConstants.PMEReturnCode.PMTrue Then

            ' Log Error.
            result = gPMConstants.PMEReturnCode.PMFalse


            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to add parameter name:" & v_sName &
                                      ", values :" & CStr(v_vValue) & ", type:" & CStr(v_iType), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)

        End If

        Return result

    End Function


    '***********************************************************************************************************************
    ' during renewal amendment user can change policy details so we need to update renewal status table with these info
    '***********************************************************************************************************************
    'developer guide no. 101
    Public Function UpdateRenewalStatus(ByVal v_lRenewalStatusCnt As Object, ByRef r_sMessage As String) As Integer

        Dim result As Integer = 0
        Try

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="RenewalStatusCnt", vValue:=v_lRenewalStatusCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to add param RenewalStatusCnt - UpdateRenewalStatus()"
                Return result
            End If

            result = m_oDatabase.SQLAction(sSQL:=ACUpdateRenewalStatusSQL, sSQLName:=ACUpdateRenewalStatusName, bStoredProcedure:=ACUpdateRenewalStatusStored)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to execute proc to update Renewal Status table"
            End If


        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            r_sMessage = Informations.Err().Description

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRenewalStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRenewalStatus()", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally


        End Try
        Return result
    End Function


    Public Function UpdateRenewalStatus(ByVal v_lRenewalStatusCnt As Object, ByRef r_sMessage As String, ByVal v_lRenewalStatusTypeId As Integer) As Integer

        Dim result As Integer = 0
        Try

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="RenewalStatusCnt", vValue:=v_lRenewalStatusCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to add param RenewalStatusCnt - UpdateRenewalStatus()"
                Return result
            End If
            result = m_oDatabase.Parameters.Add(sName:="RenewalStatusTypeId", vValue:=v_lRenewalStatusTypeId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to add param v_lRenewalStatusTypeId - UpdateRenewalStatus()"
                Return result
            End If


            result = m_oDatabase.SQLAction(sSQL:=ACUpdateRenewalStatusSQL, sSQLName:=ACUpdateRenewalStatusName, bStoredProcedure:=ACUpdateRenewalStatusStored)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to execute proc to update Renewal Status table"
            End If


        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            r_sMessage = Informations.Err().Description

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRenewalStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRenewalStatus()", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally


        End Try
        Return result
    End Function
    '****************************************************************************
    ' Get values from specified columns (v_vReturnColumn)
    ' if v_sKeyColumn and v_sKeyValue is passed in then use it as criteria otherwise
    ' whole table is selected
    ' if v_vReturnColumn is not an array then single value is returned
    ' Note: v_lColumnType = PMLong, PMString etc
    '
    '****************************************************************************
    Public Function GetValueFromTable(ByVal v_sTableName As String, ByVal v_vReturnColumn As Object, ByVal v_sKeyColumn As String, ByVal v_sKeyValue As String, ByVal v_iDataType As Integer, ByRef r_vResult As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As New StringBuilder
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = New StringBuilder("SELECT DISTINCT ")

            If Informations.IsArray(v_vReturnColumn) Then


                For lCount As Integer = 0 To v_vReturnColumn.GetUpperBound(0)

                    sSQL.Append(CStr(v_vReturnColumn(lCount)) & ",")
                Next

                'get rid of last comma
                sSQL = New StringBuilder(sSQL.ToString().Substring(0, sSQL.ToString().Length - 1))

            Else

                sSQL.Append(CStr(v_vReturnColumn))
            End If

            sSQL.Append(Strings.ChrW(13) & Strings.ChrW(10) & "FROM " & v_sTableName & Strings.ChrW(13) & Strings.ChrW(10))

            If v_sKeyColumn <> "" Then
                sSQL.Append("WHERE " & v_sKeyColumn & " = {KeyValue}")

                m_oDatabase.Parameters.Clear()

                Select Case v_iDataType
                    Case gPMConstants.PMEDataType.PMString
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=v_sKeyValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)
                    Case gPMConstants.PMEDataType.PMLong
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CStr(CInt(v_sKeyValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)
                    Case gPMConstants.PMEDataType.PMInteger
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CStr(CInt(v_sKeyValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)

                    Case gPMConstants.PMEDataType.PMDouble
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CStr(CDbl(v_sKeyValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)

                    Case gPMConstants.PMEDataType.PMDate
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=Informations.FormatDateTime(CDate(v_sKeyValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)

                    Case gPMConstants.PMEDataType.PMBoolean
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CStr(CBool(v_sKeyValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)

                    Case gPMConstants.PMEDataType.PMCurrency
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CStr(CDec(v_sKeyValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)

                End Select

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL.ToString(), sSQLName:="Get single value from table", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray, bKeepNulls:=False)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            'are we returning an array or a single value?
            If Informations.IsArray(v_vReturnColumn) Then


                r_vResult = vResultArray
            Else
                If Informations.IsArray(vResultArray) Then


                    r_vResult = vResultArray(0, 0)
                Else
                    result = gPMConstants.PMEReturnCode.PMNotFound
                End If
            End If


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get value from " & v_sTableName & "." & v_sKeyColumn & " = " & v_sKeyValue, vApp:=ACApp, vClass:=ACClass, vMethod:="GetValueFromTable", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)


        Finally



        End Try
        Return result
    End Function

    '****************************************************************************
    'transfer renewal version of policy to new broker and reset Renewal_Status.renewal_status_type_id to original value
    ' this will be stored in Renewal_Status.broker_xfer_status_type_id by renewal selection
    '****************************************************************************
    Public Function TransferBroker(ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lTransferToPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim sFailMsg As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="RenewalInsuranceFileCnt", vValue:=CStr(v_lRenewalInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to add RenewalInsuranceFileCnt param"
                Throw New Exception(sFailMsg)
            End If


            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="TransferToPartyCnt", vValue:=If(v_lTransferToPartyCnt = 0, DBNull.Value, CStr(v_lTransferToPartyCnt)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to add TransferToPartyCnt param"
                Throw New Exception(sFailMsg)
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateTransferBrokerSQL, sSQLName:=ACUpdateTransferBrokerName, bStoredProcedure:=ACUpdateTransferBrokerStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to update database with new broker"
                Throw New Exception(sFailMsg)
            End If


        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            If sFailMsg = "" Then
                sFailMsg = "Failed to transfer broker"
            End If

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFailMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="GetValueFromTable", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ValidateAcceptTMPIsValidAction
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 14-10-2005 : True Monthly Policies
    ' ***************************************************************** '
    Public Function ValidateAcceptTMPIsValidAction(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sInsuranceRef As String, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ValidateAcceptTMPIsValidAction"


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters

            ' insurance file cnt
            m_lReturn = AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' insurance ref
            m_lReturn = AddInputParameter(v_sName:="insurance_ref", v_vValue:=v_sInsuranceRef, v_iType:=gPMConstants.PMEDataType.PMString)

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=kValidateAcceptTMPIsValidActionSQL, sSQLName:=kValidateAcceptTMPIsValidActionName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kValidateAcceptTMPIsValidActionSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    '***************************************************************************************
    'delete the agent_commission link for this policy version if its now a direct business
    '****************************************************************************************
    Public Function DeleteAgentcommission(ByVal v_lInsuranceFileCnt As Integer, Optional ByRef r_sFailMessage As String = "") As Integer

        Dim result As Integer = 0
        Dim sFailMsg As String = ""

        Try

            m_oDatabase.Parameters.Clear()
            result = gPMConstants.PMEReturnCode.PMTrue
            r_sFailMessage = ""

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to add InsuranceFileCnt param"
                Throw New Exception(sFailMsg)
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDelAgentCommissionSQL, sSQLName:=ACDelAgentCommissionName, bStoredProcedure:=ACDelAgentCommissionStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to remove agent commission link for policy_id " & v_lInsuranceFileCnt
                Throw New Exception(sFailMsg)
            End If


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            If sFailMsg = "" Then
                sFailMsg = "Failed to delete record from Agent_Commission for policy_id " & v_lInsuranceFileCnt
            End If

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFailMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAgentcommission", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally
            If Not False Then
                r_sFailMessage = sFailMsg
            End If


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetProdPrintOptions
    '
    ' Parameters:
    '
    ' Description: Used to retrieve product risk options to determine
    '              whether to produce documents or not. Following are the
    '              documents it used to look for
    '                  1)produce_schedule
    '                  2)produce_certificate
    '                  3)produce_debit_note
    '
    ' History:
    '
    ' ***************************************************************** '
    Public Function GetProdPrintOptions(ByVal lproduct_id As Integer, ByRef vPrintOptions(,) As Object) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="product_id", vValue:=CStr(lproduct_id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetProductPrintOptionsSQL, sSQLName:=ACGetProductPrintOptionsName, bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vPrintOptions)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetAgents
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function GetAgents(ByRef r_vAgentArray(,) As Object, Optional ByRef v_lSourceID As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_lSourceID = 0 Then 'get all Agents
                sSQL = " SELECT PA.party_cnt, P.ShortName"
                sSQL = sSQL & " FROM Party_Agent PA"
                sSQL = sSQL & " Join Party P ON PA.Party_Cnt=P.Party_cnt"
                sSQL = sSQL & " Where Party_Agent_Type_id Not In (2,4)"
                sSQL = sSQL & " ORDER BY P.ShortName"

                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAllAgents", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vAgentArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                'get Agents specific to selected Branch

                m_oDatabase.Parameters.Clear()

                ' Add the branch parameter
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Branchid", vValue:=CStr(v_lSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Call the stored procedure
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetBranchAgentsSQL, sSQLName:=ACGetBranchAgentsName, bStoredProcedure:=ACGetBranchAgentsStored, vResultArray:=r_vAgentArray, bKeepNulls:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAgents Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAgents", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UnderwritingOrAgency
    '
    ' Description:  Finds out if Underwriting or Agency business
    '
    ' ***************************************************************** '
    Public Function getUnderwritingOrAgency() As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim oDatabase As dPMDAO.Database

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_oDatabase:=oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_sUnderwritingOrAgency = "A"

            m_oDatabase.Parameters.Clear()

            sSQL = "SELECT value FROM hidden_options WHERE option_number = 1"

            m_lReturn = oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetHiddenOption", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
                ' Carry on without default set
            End If

            If oDatabase.Records.Count() = 1 Then
                ' select first letter of the return field
                '				m_sUnderwritingOrAgency = gPMFunctions.NullToString(oDatabase.Records.Item(1).Fields()("value")).Substring(0, 1)
                m_sUnderwritingOrAgency = gPMFunctions.NullToString(oDatabase.Records.Item(0).Fields()("value")).Substring(0, 1)
            End If

            If (m_sUnderwritingOrAgency <> "A") And (m_sUnderwritingOrAgency <> "U") Then
                m_sUnderwritingOrAgency = "A"
            End If

            m_lReturn = oDatabase.CloseDatabase()

            oDatabase = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnderwritingOrAgencyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnderwritingOrAgency", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'Get the AccountID for Agent/Client
    Public Function GetAccountID(ByVal v_lPartyCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Try
            Dim lReturn As gPMConstants.PMEReturnCode

            Const kMethodName As String = "GetAccountID"

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add("Party_Cnt", CStr(v_lPartyCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception
            End If
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectAccountIDSQL, sSQLName:=ACSelectAccountIDName, bStoredProcedure:=False, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)


            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(kMethodName, ACSelectAccountIDSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetAccountID", r_lFunctionReturn:=result, excep:=ex)
        Finally



        End Try
        Return result
    End Function

    Public Function GetCurrencyAndAgentType(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lCurrency As Integer, ByRef r_sAgentType As String) As Integer

        Dim result As Integer = 0
        Try
            Dim lReturn As gPMConstants.PMEReturnCode
            Dim vResults(,) As Object
            Const kMethodName As String = "GetCurrencyAndAgentType"

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add("insurance_file_cnt", CStr(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception
            End If
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelCurrencyandAgentTypeSQL, sSQLName:=ACSelCurrencyandAgentTypeName, bStoredProcedure:=True, vResultArray:=vResults, lNumberRecords:=gPMConstants.PMAllRecords)


            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(kMethodName, ACSelCurrencyandAgentTypeSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            If Informations.IsArray(vResults) Then

                r_lCurrency = CInt(vResults(0, 0))

                r_sAgentType = CStr(vResults(1, 0))
            End If

        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetCurrencyAndAgentType", r_lFunctionReturn:=result, excep:=ex)
        Finally



        End Try
        Return result
    End Function

    'Get the AccountID for Agent/Client
    Public Function GetPolicyGrossTotal(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Try
            Dim lReturn As gPMConstants.PMEReturnCode

            Const kMethodName As String = "GetPolicyGrossTotal"

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add("insurance_file_cnt", CStr(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception
            End If
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectPolicyGrossTotalSQL, sSQLName:=ACSelectAccountIDName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)


            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(kMethodName, ACSelectPolicyGrossTotalSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetPolicyGrossTotal", r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    Public Function GetAgentCommission(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Try
            Dim lReturn As gPMConstants.PMEReturnCode

            Const kMethodName As String = "GetAgentCommission"

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add("Insurance_File_Cnt", CStr(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception
            End If

            lReturn = m_oDatabase.SQLSelect(sSQL:="spu_sir_agent_commission_sel", sSQLName:="spu_sir_agent_commission_sel", bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)


            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "spu_sir_agent_commission_sel" & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetAgentCommission", r_lFunctionReturn:=result, excep:=ex)
        Finally



        End Try
        Return result
    End Function

    'Pankaj
    ' ***************************************************************** '
    ' Name: UpdateRenewalException
    '
    ' Description: update the renewal status set renewal_exception_reason_id = 1,2,3 or 5
    '                   and renewal_status_type = "Renewal Accept (Type=8)"
    '
    ' ***************************************************************** '
    '
    'developer guide no. 101
    Public Function UpdateRenewalExceptions(ByVal v_lInsuranceFileCnt As Object, ByVal v_lRenewalExceptionReasonID As Object, Optional ByVal v_sRenewalExceptionNote As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sSQLUpdateRenewalStatus As String = ""

            'start transaction
            If BeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add(sName:="renewal_insurance_file_cnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            If m_oDatabase.Parameters.Add(sName:="exception_reason_id", vValue:=CStr(v_lRenewalExceptionReasonID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            If m_oDatabase.Parameters.Add(sName:="exception_note", vValue:=v_sRenewalExceptionNote, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateRenewalExceptionSQL, sSQLName:=ACUpdateRenewalExceptionName, bStoredProcedure:=ACUpdateRenewalExceptionStored)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                'undo changes
                lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'commit to database

            Return CommitTrans()

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="UpdateRenewalExceptions", r_lFunctionReturn:=result, excep:=excep)

            lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ValidateRenewalAcceptance
    '
    ' Parameters: InsuranceFileCnt
    '
    ' Description:
    '
    ' History:
    '           Created : Pankaj Kaushik : 20-12-2007 : Unattended Renewals
    ' ***************************************************************** '
    Public Function ValidateRenewalAcceptance(ByVal v_lInsuranceFileCnt As Integer, ByRef r_bPolicyNumberToChange As Boolean, ByRef r_bNoRenewalInstalmentPlan As Boolean, ByRef r_bPrepaymentRequired As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ValidateRenewalAcceptance"

        Dim vResultArray(,) As Object = Nothing
        Dim sFailMsg As String = ""

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            r_bPolicyNumberToChange = False
            r_bNoRenewalInstalmentPlan = False
            r_bPrepaymentRequired = False


            m_oDatabase.Parameters.Clear()
            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to add InsuranceFileCnt param"
                Throw New Exception(sFailMsg)
            End If


            '<Check for Policy Number change on renewal >
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyNumberChangeOnRenewalSQL, sSQLName:=ACGetPolicyNumberChangeOnRenewalName, bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray, bKeepNulls:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to get Change policy number at renewal for policy_id " & v_lInsuranceFileCnt
                Throw New Exception(sFailMsg)
            End If

            If Informations.IsArray(vResultArray) Then

                If CStr(vResultArray(0, 0)) = "1" Then ' Policy Number change on Renewal
                    r_bPolicyNumberToChange = True
                    Return result
                End If
            End If



            '<Check for Renewal Installment Quote exists>

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to add InsuranceFileCnt param"
                Throw New Exception(sFailMsg)
            End If
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRenewalInstallmentPlanSQL, sSQLName:=ACGetRenewalInstallmentPlanName, bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray, bKeepNulls:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to get renewal installment plan for policy_id " & v_lInsuranceFileCnt
                Throw New Exception(sFailMsg)
            End If

            If Informations.IsArray(vResultArray) Then
                'Is Bank Name Mandatory

                If CStr(vResultArray(0, 0)) = "1" Then ' Bank Name Mandatory

                    If CStr(vResultArray(4, 0)).Trim().Length = 0 Then
                        r_bNoRenewalInstalmentPlan = True

                        Return result
                    End If
                End If

                'Is Bank Address(Address Line 1 and Post Code) Mandatory

                If CStr(vResultArray(1, 0)) = "1" Then
                    'Address Line 1

                    If CStr(vResultArray(5, 0)).Trim().Length = 0 Then
                        r_bNoRenewalInstalmentPlan = True

                        Return result
                    End If
                    'PostCode

                    If CStr(vResultArray(6, 0)).Trim().Length = 0 Then
                        r_bNoRenewalInstalmentPlan = True

                        Return result
                    End If
                End If

                'Is Branch Name Mandatory

                If CStr(vResultArray(2, 0)) = "1" Then

                    If CStr(vResultArray(7, 0)).Trim().Length = 0 Then
                        r_bNoRenewalInstalmentPlan = True

                        Return result
                    End If
                End If

                'IS Branch Code Mandatory

                If CStr(vResultArray(3, 0)) = "1" Then ' Bank Name Mandatory

                    If CStr(vResultArray(8, 0)).Trim().Length = 0 Then
                        r_bNoRenewalInstalmentPlan = True

                        Return result
                    End If
                End If

            End If


            '<Check for Prepayment required on renewal >

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to add InsuranceFileCnt param"
                Throw New Exception(sFailMsg)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyPaymentMethodSQL, sSQLName:=ACGetPolicyPaymentMethodName, bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray, bKeepNulls:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to get Change policy number at renewal for policy_id " & v_lInsuranceFileCnt
                Throw New Exception(sFailMsg)
            End If

            If Informations.IsArray(vResultArray) Then

                If CStr(vResultArray(0, 0)).ToUpper() = ("PayNow").ToUpper() Then ' Policy Number change on Renewal
                    m_lReturn = CType(GetSystemOptionLite(v_iOptionNumber:=kSystemOptionPaymentHubEnabled, r_sOptionValue:=sOptionValue, v_iSourceID:=1), gPMConstants.PMEReturnCode)

                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        bPaymentHubEnabled = (sOptionValue = "1")
                    End If
                    If Not bPaymentHubEnabled Then
                        r_bPrepaymentRequired = True
                    End If
                End If
            End If

        Catch ex As Exception

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFailMsg, vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
        Finally




        End Try
        Return result
    End Function

    Public Function GenerateCustomerRenewalEmail(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_sType As String) As Integer
        Dim result As Integer = 0
        Dim vDocArray(,) As Object
        Dim vContactArrray(,) As Object
        Dim sQuerySQL, sQueryName As String
        Dim bQueryProcedure As Boolean
        Dim lDocTemplateIDMsg, lDocTemplateIDAtt, lRenewalStatus As Integer
        Dim sMergedFilePathMsg, sMergedFilePathAtt, sDocDesc, sMEmailID, sFailMsg As String
        Dim vResultArray(,) As Object = Nothing
        Dim lDocumentTypeId As Integer
        Dim sDocDescripton As String = ""
        Dim sFile() As String
        Dim sResolveFileName As String = ""

        Const kMethodName As String = "GenerateCustomerRenewalEmail"
        Const kAnniversaryCommunication As String = "2"
        Const kAsRenewalProcessRunCommunication As String = "1"
        Const kAnniversaryCopy As String = "1"
        Const kTradeRNLOnLine As String = "1"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'First Check Is TradeRNLOnline Option is on on the product
            'Only in that case we will send the mail to the customer
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetProductTradeRNLOnlinOptionSQL, sSQLName:=ACGetProductTradeRNLOnlinOptionName, bStoredProcedure:=ACGetProductTradeRNLOnlinOptionStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray, bKeepNulls:=False)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to get Main Email Address for the Party_cnt " & v_lPartyCnt
                'Return PMFail in case of failure
                result = gPMConstants.PMEReturnCode.PMFail
                Throw New Exception(sFailMsg)
            End If

            If Not Informations.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            'Check for TradeRNLOnLine Option


            If gPMFunctions.ToSafeInteger(CStr(vResultArray(1, 0))) <> ToSafeDouble(kTradeRNLOnLine) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            'Check for TMP Renewal Communication If it is 1 or 2 then This is a TMP

            If gPMFunctions.ToSafeInteger(CStr(vResultArray(2, 0))) > 0 Then
                'Now check the if Email communication is on Anniversary Date
                'then this copy must be Anniversary Copy


                If gPMFunctions.ToSafeInteger(CStr(vResultArray(2, 0))) = ToSafeDouble(kAnniversaryCommunication) Then

                    If gPMFunctions.ToSafeInteger(CStr(vResultArray(3, 0))) <> ToSafeDouble(kAnniversaryCopy) Then
                        Return gPMConstants.PMEReturnCode.PMNotFound
                    End If
                End If


                If gPMFunctions.ToSafeInteger(CStr(vResultArray(2, 0))) = ToSafeDouble(kAsRenewalProcessRunCommunication) Then

                    If gPMFunctions.ToSafeInteger(CStr(vResultArray(3, 0))) = ToSafeDouble(kAnniversaryCopy) Then
                        Return gPMConstants.PMEReturnCode.PMNotFound
                    End If
                End If
            End If

            'Get the Main Email contact address for lPartyCnt
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="contact_type", vValue:="MEmail", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelEmailContactSQL, sSQLName:=ACSelEmailContactName, bStoredProcedure:=ACSelEmailContactStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vContactArrray, bKeepNulls:=False)

            'If null Then exit with PMNotFound
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to get Main Email Address for the Party_cnt " & v_lPartyCnt
                'Return PMFail in case of failure
                result = gPMConstants.PMEReturnCode.PMFail
                Throw New Exception(sFailMsg)
            End If

            If Not Informations.IsArray(vContactArrray) Then
                sFailMsg = "Failed to get Main Email Address for the Party_cnt " & v_lPartyCnt
                result = gPMConstants.PMEReturnCode.PMNotFound
                Throw New Exception(sFailMsg)
            Else

                If CStr(vContactArrray(ACContactFieldNumber, 0)).Trim().Length <> 0 Then

                    sMEmailID = CStr(vContactArrray(ACContactFieldNumber, 0)).Trim()
                Else
                    result = gPMConstants.PMEReturnCode.PMNotFound
                    Throw New Exception
                End If
            End If

            'Get the Email Document Template for Product for lInsuranceFileCnt and
            'Status from Renewal_Status

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to add insurance_file_cnt param"
                Throw New Exception(sFailMsg)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="renewal_type", vValue:=v_sType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to add renewal_type param"
                Throw New Exception(sFailMsg)
            End If

            sQuerySQL = ACSelRenewalCustomerEmailDocumentTemplateSQL
            sQueryName = ACSelRenewalCustomerEmailDocumentTemplateName
            bQueryProcedure = ACSelRenewalCustomerEmailDocumentTemplateStored

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sQuerySQL, sSQLName:=sQueryName, bStoredProcedure:=bQueryProcedure, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vDocArray, bKeepNulls:=False)

            'If null Then exit with PMNotFound

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to get Renewal Document Template "
                'Return PMFail in case of failure
                result = gPMConstants.PMEReturnCode.PMFail
                Throw New Exception(sFailMsg)
            End If

            If Not Informations.IsArray(vDocArray) Then
                sFailMsg = "Failed to get Renewal Document Template "
                result = gPMConstants.PMEReturnCode.PMNotFound
                Throw New Exception(sFailMsg)
            End If

            '***** End

            If Informations.IsArray(vDocArray) Then
                'Is the option enabled for selection, invitation, acceptance

                lRenewalStatus = gPMFunctions.ToSafeLong(CStr(vDocArray(ACDocArrRenewalStatusTypeID, 0)))

                Select Case lRenewalStatus
                    Case ACAwaitManReview

                        lDocTemplateIDMsg = gPMFunctions.ToSafeLong(CStr(vDocArray(ACDocArrAwaitManReviewDocTemplateID_Msg, 0)))

                        lDocTemplateIDAtt = gPMFunctions.ToSafeLong(CStr(vDocArray(ACDocArrAwaitManReviewDocTemplateID_Att, 0)))

                        lDocumentTypeId = gPMFunctions.ToSafeLong(CStr(vDocArray(ACDocArrAwaitManReviewDocTypeID_Att, 0)))

                        sDocDescripton = gPMFunctions.ToSafeString(CStr(vDocArray(14, 0)))
                    Case ACAwaitRenewalPrint

                        lDocTemplateIDMsg = gPMFunctions.ToSafeLong(CStr(vDocArray(ACDocArrAwaitInviteDocTemplateID_Msg, 0)))

                        lDocTemplateIDAtt = gPMFunctions.ToSafeLong(CStr(vDocArray(ACDocArrAwaitInviteDocTemplateID_Att, 0)))

                        lDocumentTypeId = gPMFunctions.ToSafeLong(CStr(vDocArray(ACDocArrAwaitInviteDocTypeID_Att, 0)))

                        sDocDescripton = gPMFunctions.ToSafeString(CStr(vDocArray(15, 0)))
                    Case ACAwaitUpdate

                        lDocTemplateIDMsg = gPMFunctions.ToSafeLong(CStr(vDocArray(ACDocArrAwaitUpdateDocTemplateID_Msg, 0)))

                        lDocTemplateIDAtt = gPMFunctions.ToSafeLong(CStr(vDocArray(ACDocArrAwaitUpdateDocTemplateID_Att, 0)))

                        lDocumentTypeId = gPMFunctions.ToSafeLong(CStr(vDocArray(ACDocArrAwaitUpdateDocTypeID_Att, 0)))

                        sDocDescripton = gPMFunctions.ToSafeString(CStr(vDocArray(16, 0)))
                End Select

            End If

            If lDocTemplateIDMsg > 0 Then
                'This is the Message (Template)
                m_lReturn = PrintDocument(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lDocTemplateID:=lDocTemplateIDMsg, v_sType:="Message", r_sMergedFilePath:=sMergedFilePathMsg, r_sDocDesc:=sDocDesc, v_lDocumentTypeId:=lDocumentTypeId, v_iMode:=gSIRLibrary.ACEmailMode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sFailMsg = "Failed to Generate Document to HTML file"
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Throw New Exception(sFailMsg)
                End If

            End If

            If lDocTemplateIDAtt > 0 Then
                'This is the Attachment
                m_lReturn = PrintDocument(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lDocTemplateID:=lDocTemplateIDAtt, v_sType:="Attachment", r_sMergedFilePath:=sMergedFilePathAtt, v_lDocumentTypeId:=lDocumentTypeId, v_iMode:=gSIRLibrary.ACEmailMode)

                sFile = sMergedFilePathAtt.Split("\"c)

                sResolveFileName = sMergedFilePathAtt.Substring(0, sMergedFilePathAtt.Length - sFile(sFile.GetUpperBound(0)).Length)
                sResolveFileName = sResolveFileName & sDocDescripton & ".htm"

                File.Copy(sMergedFilePathAtt, sResolveFileName)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sFailMsg = "Failed to Generate Document to HTML file"
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Throw New Exception(sFailMsg)
                End If
            End If

            If lDocTemplateIDAtt = 0 And lDocTemplateIDMsg = 0 Then
                sFailMsg = "Failed to get Renewal Document Template"
                result = gPMConstants.PMEReturnCode.PMNotFound
                Throw New Exception(sFailMsg)
            End If

            ' Send Email with SendEmail function. The document description is the subject.
            Dim oDocTemplate As New bSIRDocTemplate.Business

            m_lReturn = oDocTemplate.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))

            m_lReturn = m_lReturn = oDocTemplate.SendEMail(v_sTo:=sMEmailID,
                                                                      v_sSubject:=sDocDesc,
                                                                     v_sMessagePath:=sMergedFilePathMsg,
                                                                      v_sAttachment:=sResolveFileName)
            oDocTemplate = Nothing

            If lDocTemplateIDAtt > 0 Then
                File.Delete(sResolveFileName)
            End If

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                'Update the Email Status
                m_oDatabase.Parameters.Clear()
                m_lReturn = m_oDatabase.Parameters.Add(sName:="renewal_insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACUpdEmailSentStatusSQL, sSQLName:=ACUpdEmailSentStatusName, bStoredProcedure:=ACUpdEmailSentStatusStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sFailMsg = "Failed to set the Email_Status for " & v_lInsuranceFileCnt
                    result = gPMConstants.PMEReturnCode.PMNotFound
                    Throw New Exception(sFailMsg)
                End If
            End If

            'Generate Event Log with lPartyCnt, lInsuranceFileCnt with description "E-mail renewal "+sType+" notification generated to "+Party.ResolvedName

            m_lReturn = CreateEvent(v_vEventCnt:=0, v_vPartyCnt:=v_lPartyCnt, v_vInsuranceFileCnt:=v_lInsuranceFileCnt, v_vEventType:=5, v_vUserId:=m_iUserID, v_vEventDate:=DateTime.Today, v_vDescription:="E-mail renewal " & v_sType & " notification generated to " & CStr(vContactArrray(ACContactFieldPartyResolvedName, 0)))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Log Error Message
                sFailMsg = "Failed to add event."
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception(sFailMsg)

            End If

            result = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As Exception

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFailMsg, vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    'Name:  GenerateReportAsDocument
    '
    ' Description: Generate Report Document
    '
    ' *****************************************************************
    Private Function GenerateReportAsDocument(ByVal v_lReportID As Integer, ByVal v_sReportTemplate As String, ByVal v_lPartyCnt As Integer, ByRef r_sExportReportFile As String) As Integer

        Dim result As Integer = 0
        Dim sExportFile As Object = ""
        Dim sReportOutput, sUserReportName As String
        Dim vParameters, vDefaultValues As Object

        Dim ErrMsg As String = ""




        Dim Str() As String
        Dim sReportName As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        r_sExportReportFile = ""

 'If m_oReport Is Nothing Then
 '    m_oReport = New bSIRReportPrint.Business
 '    m_lReturn = m_oReport.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))

 '    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
 '        ' Log Error Message
 '        ErrMsg = "Failed to create bSIRReportPrint object"
 '        result = gPMConstants.PMEReturnCode.PMFalse
 '        Throw New Exception()
 '    End If
 'End If

        result = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oReport, v_sClassName:="bSIRReportPrint.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUserName, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
             Dim r_sMessage As String = "Failed to create an instance of bSIRReportPrint.Business"
             bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="bSIRReportPrint.Business", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
             Return result
        End If

        'Remove Ext

        Str = v_sReportTemplate.Split("."c)

        sReportName = Str(0)

        'assign report name and get output path

        m_oReport.reportName = sReportName

        sReportOutput = m_oReport.ReportOutputLocation

        'get user report name - this is unique per user per session

        sUserReportName = m_oReport.UserReportName

        If sReportOutput.Length > 1 Then
            If Not sReportOutput.EndsWith("\") Then
                sReportOutput = sReportOutput & "\"
            End If
        End If

        'delete old version of output file
        If Directory.GetFiles(sReportOutput & sUserReportName & ".*", FileAttribute.Normal) IsNot "" Then
            File.Delete(sReportOutput & sUserReportName & ".*")
        End If



        m_lReturn = m_oReport.GetParameters(r_vParameters:=vParameters, r_vDefaultValues:=vDefaultValues)

        'Only one parameter, so just add the user_id into it.
        If Not Informations.IsArray(vParameters) Then
            Return result
        End If

        'vParameters(1, 0) = m_iUserID
        'vParameters(1, 4) = v_lReportID

        vParameters(1, 1) = v_lPartyCnt

        'export to XLS format

        m_lReturn = m_oReport.ExportToDisk(r_ExportFile:=sExportFile, v_iFormatType:=0, v_vParameters:=vParameters, v_sExt:=".xls")

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Log Error.

            ErrMsg = "Failed To Export Agent List To Excel Format"
            result = gPMConstants.PMEReturnCode.PMFalse
            Throw New Exception()

        End If

        r_sExportReportFile = sExportFile

        Return result

    End Function

    Public Function SendEMail(ByVal v_sTo As String, ByVal v_sSubject As String, ByVal v_sMessagePath As String, ByVal v_sAttachment As String) As Integer

        Dim result As Integer = 0
        Dim sch As String = ""

        'Developer guide no 205
        Dim cdoMail As New Object
        Dim cdoConf As New Object

        Dim sSMTPEmailServer, sSMTPEmailPort, sSMTPEmailFrom As String

        Dim sFailMsg As String = ""

        Const SMTP_Email_Server As Integer = 5045
        Const SMTP_Email_Port As Integer = 5046
        Const SMTP_Email_From As Integer = 5047

        Const kMethodName As String = "SendEMail"

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            'Developer guide no 205
            cdoMail = New Object
            cdoConf = New Object

            cdoMail.Subject = v_sSubject

            ' Get System Option (SMTP Email From)
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=SMTP_Email_From, r_sOptionValue:=sSMTPEmailFrom)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to get the System option SMTP_Email_From"
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception(sFailMsg)
            End If

            If sSMTPEmailFrom = "" Then
                ' option not populated, no action
                sFailMsg = "Invalid SMTP"
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception(sFailMsg)

            End If

            ' Get System Option (SMTP Email Server)
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=SMTP_Email_Server, r_sOptionValue:=sSMTPEmailServer)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to get the System option SMTP_Email_Server"
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception(sFailMsg)
            End If

            If sSMTPEmailServer = "" Then
                ' option not populated, no action
                sFailMsg = "Invalid SMTP"
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception(sFailMsg)

            End If

            ' Get System Option (SMTP Email Port)
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=SMTP_Email_Port, r_sOptionValue:=sSMTPEmailPort)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to get the System option SMTP_Email_Port"
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception(sFailMsg)
            End If

            If sSMTPEmailPort = "" Then
                ' option not populated, no action
                sFailMsg = "Invalid SMTP"
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception(sFailMsg)

            End If

            cdoMail.From = sSMTPEmailFrom
            cdoMail.To = v_sTo

            Dim fileInfo As FileInfo = New FileInfo(v_sMessagePath)
            fileInfo.Attributes = FileAttribute.Normal
            cdoMail.CreateMHTMLBody("file://" & v_sMessagePath)

            If v_sAttachment <> "" Then
                cdoMail.AddAttachment(ToSafeString(v_sAttachment))
            End If

            sch = "http://schemas.microsoft.com/cdo/configuration/"
            'todo list
            'cdoConf.Fields(sch & "sendusing") = 2
            'cdoConf.Fields(sch & "smtpserver") = sSMTPEmailServer
            'cdoConf.Fields(sch & "smtpserverport") = sSMTPEmailPort
            'cdoConf.Fields(sch & "smtpauthenticate") = 0
            cdoConf.Fields.Update()

            cdoMail.Configuration = cdoConf

            cdoMail.Send()


            cdoMail = Nothing
            cdoConf = Nothing




        Catch ex As Exception

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFailMsg, vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: PrintDocument (Private)
    '
    ' Description: Print out document
    '
    ' ***************************************************************** '
    Private Function PrintDocument(ByVal v_lPartyCnt As Integer, ByVal v_lDocTemplateID As Integer, ByVal v_sType As String, ByRef r_sMergedFilePath As String,
                                   ByVal v_iMode As Integer, Optional ByVal v_lInsuranceFileCnt As Integer = 0, Optional ByRef r_sDocDesc As String = "",
                                   Optional ByVal v_lDocumentTypeId As Integer = 0, Optional ByVal v_iIsClient As Integer = 0, Optional ByVal v_iIsAgent As Integer = 0,
                                   Optional ByVal v_iIsOffice As Integer = 0, Optional ByVal v_iProductionOrder As Integer = 1, Optional ByVal v_sSpoolDesc As String = "",
                                   Optional ByVal v_bCalledFromSAM As Boolean = False, Optional ByVal v_lInsuranceFolderCnt As Integer = 0, Optional ByVal v_sInsuranceFileRef As String = "",
                                   Optional ByVal v_bOutputAsHTM As Boolean = True,
                                   Optional ByVal v_bOutPutAsTXT As Boolean = False,
                                   Optional ByRef r_bRetainTempFiles As Boolean = False,
                                   Optional ByVal v_bOutputAsPDF As Boolean = False, Optional ByVal v_sDocTemplateCode As String = "") As Integer

        Dim result As Integer = 0
        Dim sTempFilePath As String
        Dim sTempFileName As String
        Dim sMergedFilePath As String = ""
        Dim oDocManagerWrapper As bSIRDocManagerWrapper.Interface_Renamed

        result = gPMConstants.PMEReturnCode.PMTrue
        'developer guide no. 108
        oDocManagerWrapper = New bSIRDocManagerWrapper.Interface_Renamed()
        m_lReturn = oDocManagerWrapper.InitialiseBusiness(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New Exception()
        End If

        With oDocManagerWrapper

            .InsuranceFolderCnt = v_lInsuranceFolderCnt
            .InsuranceFileRef = v_sInsuranceFileRef
            .PartyCnt = v_lPartyCnt
            .InsuranceFileCnt = v_lInsuranceFileCnt
            .OutputAsHTML = v_bOutputAsHTM
            .OutputAsTXT = v_bOutPutAsTXT
            .Mode = v_iMode
            .DocumentTemplateId = v_lDocTemplateID
            .DocumentTypeId = v_lDocumentTypeId
            .DocumentTemplateCode = v_sDocTemplateCode
            .SpoolDesc = v_sSpoolDesc
            .IsClient = v_iIsClient
            .IsAgent = v_iIsAgent
            .IsOffice = v_iIsOffice
            .ProductionOrder = v_iProductionOrder
            .CalledFromSAM = v_bCalledFromSAM
            .RetainTempFiles = r_bRetainTempFiles

            m_lReturn = .Start()
            r_sMergedFilePath = .MergedFilePath
            r_sDocDesc = .DocumentTemplateDescription

        End With
        If v_bOutputAsPDF Then
            sTempFileName = String.Empty
            sTempFilePath = String.Empty

            If r_sMergedFilePath.ToUpper.EndsWith("XML") Then
                sTempFileName = Path.GetFileNameWithoutExtension(r_sMergedFilePath)
                sTempFilePath = r_sMergedFilePath.Remove(r_sMergedFilePath.Length - 3) & "PDF"
                sTempFilePath = sTempFilePath.Replace(sTempFileName, r_sDocDesc)
                ConvertDocumentUsingSiriusDocumentUtility(r_sMergedFilePath, sTempFilePath)
            ElseIf r_sMergedFilePath.ToUpper.EndsWith("DOCX") Then
                sTempFileName = Path.GetFileNameWithoutExtension(r_sMergedFilePath)
                sTempFilePath = r_sMergedFilePath.Remove(r_sMergedFilePath.Length - 4) & "PDF"
                sTempFilePath = sTempFilePath.Replace(sTempFileName, r_sDocDesc)
                ConvertDocumentUsingSiriusDocumentUtility(r_sMergedFilePath, sTempFilePath)
            ElseIf System.IO.File.Exists(r_sMergedFilePath.Remove(r_sMergedFilePath.Length - 3) & "XML") Then
                sTempFileName = Path.GetFileNameWithoutExtension(r_sMergedFilePath)
                sTempFilePath = r_sMergedFilePath.Remove(r_sMergedFilePath.Length - 3) & "PDF"
                sTempFilePath = sTempFilePath.Replace(sTempFileName, r_sDocDesc)
                ConvertDocumentUsingSiriusDocumentUtility(r_sMergedFilePath.Remove(r_sMergedFilePath.Length - 3) & "XML", sTempFilePath)
            End If

            r_sMergedFilePath = sTempFilePath
        End If
        oDocManagerWrapper.Dispose()

        oDocManagerWrapper = Nothing


        Return result

    End Function

    Private Function ArchiveEmail(ByVal partyCnt As Integer, ByVal docTemplateID As Integer, ByVal sType As String, ByRef mergedFilePath As String,
                                    ByVal v_iMode As Integer, Optional ByVal insuranceFileCnt As Integer = 0, Optional ByRef docDesc As String = "",
                                    Optional ByVal documentTypeId As Integer = 0, Optional ByVal v_iIsClient As Integer = 0, Optional ByVal isAgent As Integer = 0,
                                    Optional ByVal isOffice As Integer = 0, Optional ByVal productionOrder As Integer = 1, Optional ByVal spoolDesc As String = "",
                                    Optional ByVal calledFromSAM As Boolean = False, Optional ByVal insuranceFolderCnt As Integer = 0,
                                    Optional ByVal insuranceFileRef As String = "",
                                    Optional ByVal outputAsHTM As Boolean = False,
                                    Optional ByVal outPutAsTXT As Boolean = False,
                                    Optional ByRef retainTempFiles As Boolean = False,
                                    Optional ByVal outputAsPDF As Boolean = False,
                                    Optional ByVal docTemplateCode As String = "",
                                    Optional ByVal archiveWithNoMerge As Boolean = False,
                                    Optional ByVal documentDestinationFilename As String = "",
                                    Optional ByVal isGeneratedEmail As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim sMergedFilePath As String = ""
        Dim oDocManagerWrapper As bSIRDocManagerWrapper.Interface_Renamed

        result = gPMConstants.PMEReturnCode.PMTrue
        oDocManagerWrapper = New bSIRDocManagerWrapper.Interface_Renamed()

        m_lReturn = oDocManagerWrapper.InitialiseBusiness(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New Exception()
        End If

        With oDocManagerWrapper

            .InsuranceFolderCnt = insuranceFolderCnt
            .InsuranceFileRef = insuranceFileRef
            .PartyCnt = partyCnt
            .InsuranceFileCnt = insuranceFileCnt
            .OutputAsHTML = outputAsHTM
            .OutputAsTXT = outPutAsTXT
            .Mode = v_iMode
            .DocumentTemplateId = docTemplateID
            .DocumentTypeId = documentTypeId
            .DocumentTemplateCode = docTemplateCode
            .SpoolDesc = spoolDesc
            .IsClient = v_iIsClient
            .IsAgent = isAgent
            .IsOffice = isOffice
            .ProductionOrder = productionOrder
            .CalledFromSAM = calledFromSAM
            .RetainTempFiles = retainTempFiles
            .IsGeneratedMail = isGeneratedEmail
            .MergedFilePath = mergedFilePath

            If String.IsNullOrEmpty(mergedFilePath) Then
                .ArchiveDoc = archiveWithNoMerge
            Else
                .ArchiveWithNoMerge = archiveWithNoMerge
            End If

            m_lReturn = .Start()

            mergedFilePath = .MergedFilePath
        End With

        oDocManagerWrapper.Dispose()
        oDocManagerWrapper = Nothing

        Return result

    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' ***************************************************************** '
    ' Name: GetReportPath
    '
    ' Description: Gets the Report Templates location from the registry.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetReportPath) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetReportPath() As Integer
    '
    'Dim result As Integer = 0
    'Dim sRegPath As String = ""
    '
    'Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
    'Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
    'Dim eProductFamily As gPMConstants.PMEProductFamily
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Set to LocalMachine/Sirius/Client
    'eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
    'eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
    'eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient
    '
    ' Location for Exported Reports
    'sRegPath = ""
    'm_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="PrntFileDir", r_sSettingValue:=sRegPath)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Report Destination directory from Registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReportPath", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
    'Return gPMConstants.PMEReturnCode.PMFalse
    'Else
    'm_sReportOutputLocation = sRegPath
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
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetReportPathFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReportPath", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    Public Function GenerateAgentRenewalEmail(ByVal v_sType As String) As Integer

        Dim result As Integer = 0
        Dim sFailMsg As String = ""

        Dim vResultArray(,) As Object = Nothing



        Dim vContactArrray(,) As Object

        Dim sMEmailID As String = ""

        Dim lDocTemplateIDMsg, lReportID As Integer
        Dim sReportTemplate As String = ""

        Dim sDocDesc, sMergedFilePathMsg, sMergedFilePathAtt As String
        Dim lPartyCnt As Integer
        Dim bMailAddressFound, bSendMailToAgent As Boolean

        Const kMethodName As String = "GenerateAgentRenewalEmail"
        Const kAnniversaryCommunication As String = "2"
        Const kAsRenewalProcessRunCommunication As String = "1"
        Const kAnniversaryCopy As String = "1"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="renewal_type", vValue:=v_sType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to add lead_agent_cnt param"
                result = gPMConstants.PMEReturnCode.PMFail
                Throw New Exception(sFailMsg)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelRenewalAgentEmailDocumentTemplateSQL, sSQLName:=ACSelRenewalAgentEmailDocumentTemplateName, bStoredProcedure:=ACSelRenewalAgentEmailDocumentTemplateStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray, bKeepNulls:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to get Renewal Document Template ID "
                result = gPMConstants.PMEReturnCode.PMFail
                Throw New Exception(sFailMsg)
            End If


            If Not Informations.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
                Throw New Exception(sFailMsg)
            End If



            For lLoop As Integer = 0 To vResultArray.GetUpperBound(1)


                lPartyCnt = gPMFunctions.ToSafeLong(CStr(vResultArray(0, lLoop)))

                bSendMailToAgent = True

                '        'Get the Main Email contact address for lPartyCnt
                '        'If null Then exit with PMTrue

                'Check for TMP Renewal Communication If it is 1 or 2 then This is a TMP

                If gPMFunctions.ToSafeInteger(CStr(vResultArray(4, lLoop))) > 0 Then
                    'Now check the if Email communication is on Anniversary Date
                    'then this copy must be Anniversary Copy


                    If gPMFunctions.ToSafeInteger(CStr(vResultArray.GetValue(4, lLoop))) = ToSafeDouble(kAnniversaryCommunication) Then

                        If gPMFunctions.ToSafeInteger(CStr(vResultArray(5, lLoop))) <> ToSafeDouble(kAnniversaryCopy) Then
                            bSendMailToAgent = False
                        End If
                    End If


                    If gPMFunctions.ToSafeInteger(CStr(vResultArray(4, lLoop))) = ToSafeDouble(kAsRenewalProcessRunCommunication) Then

                        If gPMFunctions.ToSafeInteger(CStr(vResultArray(5, lLoop))) = ToSafeDouble(kAnniversaryCopy) Then
                            bSendMailToAgent = False
                        End If
                    End If
                End If

                If bSendMailToAgent Then
                    m_oDatabase.Parameters.Clear()
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="contact_type", vValue:="MEmail", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelEmailContactSQL, sSQLName:=ACSelEmailContactName, bStoredProcedure:=ACSelEmailContactStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vContactArrray, bKeepNulls:=False)

                    'If null Then exit with PMNotFound
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        sFailMsg = "Failed to get Main Email Address for the Party_cnt " & lPartyCnt
                        result = gPMConstants.PMEReturnCode.PMFail
                        Throw New Exception(sFailMsg)
                    End If

                    bMailAddressFound = True

                    If Not Informations.IsArray(vContactArrray) Then
                        sFailMsg = "Failed to get Main Email Address for the Party_cnt " & lPartyCnt
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFailMsg, vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        bMailAddressFound = False
                    End If

                    If Informations.IsArray(vContactArrray) Then

                        If CStr(vContactArrray(ACContactFieldNumber, 0)).Trim().Length = 0 Then
                            sFailMsg = "Failed to get Main Email Address for the Party_cnt " & lPartyCnt
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFailMsg, vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                            bMailAddressFound = False
                        End If
                    End If

                    If bMailAddressFound Then


                        sMEmailID = CStr(vContactArrray(ACContactFieldNumber, 0)).Trim()

                        lDocTemplateIDMsg = gPMFunctions.ToSafeLong(CStr(vResultArray(1, lLoop)))

                        lReportID = gPMFunctions.ToSafeLong(CStr(vResultArray(2, lLoop)))

                        sReportTemplate = CStr(vResultArray(3, lLoop))

                        'This is the Message Body
                        If lDocTemplateIDMsg <> 0 Then
                            m_lReturn = PrintDocument(v_lPartyCnt:=lPartyCnt, v_lDocTemplateID:=lDocTemplateIDMsg, v_sType:="Message", r_sMergedFilePath:=sMergedFilePathMsg, r_sDocDesc:=sDocDesc, v_lDocumentTypeId:=gSIRLibrary.ACEmailMode, v_iMode:=gSIRLibrary.ACEmailMode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                sFailMsg = "Failed "
                                result = gPMConstants.PMEReturnCode.PMFail
                                Throw New Exception(sFailMsg)
                            End If
                        End If

                        'This is the Attachment
                        If sReportTemplate.Trim().Length > 0 Then
                            m_lReturn = GenerateReportAsDocument(v_lReportID:=lReportID, v_sReportTemplate:=sReportTemplate, v_lPartyCnt:=lPartyCnt, r_sExportReportFile:=sMergedFilePathAtt)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                sFailMsg = "Failed to Generate Report"
                                result = gPMConstants.PMEReturnCode.PMFail
                                Throw New Exception(sFailMsg)
                            End If
                        End If

                        'Send Email with SendEmail function. The document description is the subject.
                        m_lReturn = SendEMail(sMEmailID, sDocDesc, sMergedFilePathMsg, sMergedFilePathAtt)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            sFailMsg = "Failed to Send the Email to party_cnt" & lPartyCnt
                            result = gPMConstants.PMELogLevel.PMLogError
                            Throw New Exception(sFailMsg)
                        End If

                        'Generate Event Log with lPartyCnt, lInsuranceFileCnt with description "E-mail renewal "+sType+" notification generated to "+Party.ResolvedName

                        m_lReturn = CreateEvent(v_vEventCnt:=0, v_vPartyCnt:=lPartyCnt, v_vEventType:=5, v_vUserId:=m_iUserID, v_vEventDate:=DateTime.Today, v_vDescription:="E-mail renewal " & v_sType & " notification generated to " + CDbl(vContactArrray(ACContactFieldPartyResolvedName, 0)))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            'Log Error Message
                            sFailMsg = "Failed to add event."
                            result = gPMConstants.PMELogLevel.PMLogError
                            Throw New Exception(sFailMsg)
                        End If

                    End If
                End If
            Next


        Catch ex As Exception

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFailMsg, vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

            Return result
        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetRenewalDocumentTemplateDetails
    '
    ' Parameters: n/a
    '
    ' Description: Copied ValidateRenewalInvite from bSIRRenInvitePrint
    '
    ' History:
    '           Created : MEvans : 03-03-2008 :
    ' ***************************************************************** '
    Public Function GetRenewalDocumentTemplateDetails(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lProcessType As Integer, ByRef r_lDocumentTemplateId As Integer, ByRef r_lDocumentTypeId As Integer) As Integer
        Dim Catch_Renamed As Boolean = False

        Dim result As Integer = 0
        Const kMethodName As String = "GetRenewalDocumentTemplateDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Dim sTemplateCode As String = ""
        Dim lReportPointer As Integer
        Dim sBusinessType As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim vTabArray(,) As Object
        Dim lDocumentTemplateId, lDocumentTypeId As Integer
        Dim sDocumentTemplateDescription As String = ""
        Dim oSirFindDocumentTemplate As bSIRFindDocTemplate.Form

        Try
            Catch_Renamed = True

            result = gPMConstants.PMEReturnCode.PMTrue


            vResultArray = Nothing
            ReDim vTabArray(3, 0)


            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = "process_types_docs"

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = v_lProcessType

            oSirFindDocumentTemplate = New bSIRFindDocTemplate.Form
            m_lReturn = oSirFindDocumentTemplate.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))

            lReturn = oSirFindDocumentTemplate.GetProcessTypesLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=vTabArray, iLanguageID:=m_iLanguageID, vResultArray:=vResultArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bSIRFindDocumentTemplate.Form.GetProcessTypesLookupValues Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Informations.IsArray(vResultArray) Then
                gPMFunctions.RaiseError(kMethodName, "bSIRFindDocumentTemplate.Form.GetProcessTypesLookupValues failed to return data", gPMConstants.PMELogLevel.PMLogError)
            End If


            sTemplateCode = CStr(vResultArray(2, 0)).Trim()

            If sTemplateCode = gSIRLibrary.SIRDocTypeCodeCancel Then

                lReturn = oSirFindDocumentTemplate.GetBusinessType(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_sBusinessType:=sBusinessType)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "bSIRFindDocumentTemplate.Form.GetBusinessType Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                Select Case sBusinessType
                    Case "DIRECT"
                        sTemplateCode = gSIRLibrary.SIRDocTypeCodeCancelClient
                    Case Else
                        sTemplateCode = gSIRLibrary.SIRDocTypeCodeCancelAgent
                End Select

            End If

            sTemplateCode = sTemplateCode & m_sTransactionType.Trim()

            lReturn = oSirFindDocumentTemplate.GetReportPointer(v_lInsuranceFileCnt, lReportPointer)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bSIRFindDocumentTemplate.Form.GetReportPointer Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If lReportPointer <> 0 Then
                sTemplateCode = sTemplateCode & CStr(lReportPointer)
            End If

            'Ensure template exists. If not, apply rules until  suitable template is found.

            lReturn = oSirFindDocumentTemplate.GetAvailableTemplate(sTemplateCode, lDocumentTemplateId, lDocumentTypeId, sDocumentTemplateDescription)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bSIRFindDocumentTemplate.Form.GetAvailableTemplate Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If lDocumentTemplateId <> 0 And lDocumentTypeId <> 0 Then
                r_lDocumentTemplateId = lDocumentTemplateId
                r_lDocumentTypeId = lDocumentTypeId
            Else
                gPMFunctions.RaiseError(kMethodName, "bSIRFindDocumentTemplate.Form.GetAvailableTemplate failed to return data", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If

            If Catch_Renamed Then

                ' DO Not Call any functions before here or the error will be lost
                bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=excep)

                ' If you want to rollback a transaction or something, do it here

            End If

        Finally
            If Not oSirFindDocumentTemplate Is Nothing Then
                oSirFindDocumentTemplate.Dispose()
                oSirFindDocumentTemplate = Nothing
            End If

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: PrcessRenewalAcceptance
    '
    ' History: 12.06.08 Pankaj - Created.
    '
    ' Task: WR9 Batch Renewals - Multi Threaded Crontroller
    ' ***************************************************************** '
    Public Function ProcessRenewalAcceptance(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lBatchRenewalJobID As Integer, ByVal v_lRecordsCount As Integer, ByVal v_sGUID As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessRenewalAcceptance"

        Dim sReportTitle, sReportText, sFailedText, sAcceptanceReportFileName, sRegPath As String
        Dim sSpoolRenewalReport As String = ""
        Dim bInvalidTMPFound As Boolean
        Dim lInvalidTMPCount As Integer
        Dim bValid As Boolean

        Dim bPolicyNumberToChange, bNoRenewalInstalmentPlan, bNoRenewalInstalmentPlanFound, bPrepaymentRequired As Boolean

        Dim lIsQuoted As gPMConstants.PMEReturnCode
        Dim oPMLock As bpmlock.User
        Dim sLockedBy As String = ""
        Dim lRecordsCount, lBatchRenewalJobRunsID As Integer
        Dim bOtherException As Boolean
        Dim bTransactionOpen As Boolean = False
        Dim vPlanArray As Object
        Dim obSIRRenewalProcess As Object
        Dim r_sFailureMessage As Object

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            m_lReturn = m_oDatabase.SQLBeginTrans()

            bTransactionOpen = True

            'Get bPMLock
            oPMLock = New bpmlock.User
            m_lReturn = oPMLock.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))


            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACClass, kMethodName & " Fails to get Instance of bPMLock.User", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Lock the Key

            m_lReturn = oPMLock.LockKey(sKeyName:="RENACC", vKeyValue:=v_lInsuranceFileCnt, iUserID:=m_iUserID, sCurrentlyLockedBy:=sLockedBy)

            'Check Return Type. If <> PMTrue then Skip (to ensure single user mode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailedText = "This record was being locked by " & sLockedBy
                Throw New Exception(sFailedText)
            End If

            m_lReturn = GetRenewalAcceptanceDetails(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vResultArray:=m_vRenewalsData)

            If Not Informations.IsArray(m_vRenewalsData) Then
                Return result
            End If

            m_lSelPartyCnt = CInt(m_vRenewalsData(ACIRenewalInsuranceHolder, 0))

            m_lSelNewPolicyCnt = CInt(m_vRenewalsData(ACIRenewalPolicyCnt, 0))

            If sSpoolRenewalReport = "" Then
                If bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=1012, r_sOptionValue:=sSpoolRenewalReport) <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If
            End If

            If m_sRenSchedulePrinting = "" Then
                If bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=1036, r_sOptionValue:=m_sRenSchedulePrinting) <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If
            End If

            If m_sRenCertificatePrinting = "" Then
                If bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=1037, r_sOptionValue:=m_sRenCertificatePrinting) <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If
            End If

            If m_sRenDebitNotePrinting = "" Then
                If bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=1038, r_sOptionValue:=m_sRenDebitNotePrinting) <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If
            End If

            'get path from registry
            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="PrntFileDir", r_sSettingValue:=sRegPath)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            If Not sRegPath.EndsWith("\") Then
                sRegPath = sRegPath & "\"
            End If
            m_lReturn = CreateBusinessObject(obSIRRenewalProcess, "bSIRRenewalProcess.Business")

            Dim sPaymentMethod As String
            sPaymentMethod = ToSafeString(m_vRenewalsData(kRenewalPolicyPaymentMethod, 0))

            If (sPaymentMethod).ToUpper <> "INVOICE" Then
                If ((sPaymentMethod).ToUpper = "INSTALMENTS" OrElse (sPaymentMethod).ToUpper = "DIRECT DEBIT" OrElse (sPaymentMethod).ToUpper = "PREMIUMFINANCE") AndAlso
                     obSIRRenewalProcess.IsInstalment(v_lInsuranceFileCnt:=m_vRenewalsData(ACIRenewalLivePolicyCnt, 0)) = gPMConstants.PMEReturnCode.PMTrue Then
                    'create quote plan for renewal version ( if its not already there) now that we have plan on current version
                    result = obSIRRenewalProcess.CreateInstalmentQuote(v_lOriginalInsuranceFileCnt:=m_vRenewalsData(ACIRenewalLivePolicyCnt, 0),
                                                               v_lRenewalInsuranceFileCnt:=CInt(m_vRenewalsData(ACIRenewalPolicyCnt, 0)),
                                                               v_lPartyCnt:=CInt(m_vRenewalsData(ACIRenewalInsuranceHolder, 0)),
                                                               r_vPlanArray:=vPlanArray, r_sFailureMessage:=r_sFailureMessage)
                    If result <> PMEReturnCode.PMTrue Then
                        bNoRenewalInstalmentPlanFound = True
                    End If
                End If
            End If

            'developer guide no. 98
            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnablePayNowOptions, v_vBranch:=m_iSourceID, r_vUnderwriting:=m_vAllowPayNowOption)

            'get report file name including path
            sAcceptanceReportFileName = sRegPath & DateTime.Now.ToString("yyyyMMddHHMMss") &
                                        "Renewal_" & m_sUsername & ".log"

            'report title
            sReportTitle = "Renewal Acceptance Status Report - " & StringsHelper.Format(DateTime.Now, "dd mmmm " &
                           "yyyy hh:mm:ss AMPM") & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)

            'accept just one policy

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                sFailedText = "AllPolicyPayNow Failed"
                Throw New Exception(sFailedText)
            End If

            ' Determine if this is a true monthly policy anniversary copy
            ' if it is only allow acceptance if the previous cycle has been completed
            m_lReturn = ValidateAcceptDuplicateVersion(CInt(m_vRenewalsData(ACIRenewalPolicyCnt, 0)), bValid)
            If bValid = False Then
                sFailedText = "The Renewal cannot be accepted because it is a duplicate version. Please go to Renewal Manager and Delete the Version."
                Throw New Exception(sFailedText)
            End If

            ' Determine if this is a true monthly policy anniversary copy
            ' if it is only allow acceptance if the previous cycle has been completed
            m_lReturn = ValidateTMPPolicy(v_lSelectedIndex:=0, r_bIsValid:=bValid)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bInvalidTMPFound = True
                lInvalidTMPCount += 1
                bValid = False
                sFailedText = "The Anniversary Renewal cannot be accepted until the last monthly cycle has been accepted"
                Throw New Exception(sFailedText)
            End If

            If bValid Then
                m_lReturn = ValidateRenewalAndUpdateExceptions(v_lInsuranceFileCnt:=CInt(m_vRenewalsData(ACIRenewalPolicyCnt, 0)), r_bPolicyNumberToChange:=bPolicyNumberToChange, r_bNoRenewalInstalmentPlan:=bNoRenewalInstalmentPlan, r_bPrepaymentRequired:=bPrepaymentRequired)
                'Check for any error
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception
                End If

                m_lReturn = CType(GetSystemOptionLite(v_iOptionNumber:=kSystemOptionPaymentHubEnabled, r_sOptionValue:=sOptionValue, v_iSourceID:=1), gPMConstants.PMEReturnCode)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    bPaymentHubEnabled = (sOptionValue = "1")
                End If

                If Not bPolicyNumberToChange And Not bNoRenewalInstalmentPlan And (Not bPrepaymentRequired OrElse bPaymentHubEnabled) Then

                    If CStr(m_vRenewalsData(ACIRenewalClosedBranch, 0)).Trim() = "1" Then
                        sFailedText = sFailedText &
                                      CStr(m_vRenewalsData(ACIRenewalLivePolicy, 0)).Trim() & " - " &
                                      "Branch Closed" & Strings.ChrW(13) & Strings.ChrW(10)

                        m_lReturn = UpdateRenewalExceptions(v_lInsuranceFileCnt:=CInt(m_vRenewalsData(ACIRenewalPolicyCnt, 0)), v_lRenewalExceptionReasonID:=5, v_sRenewalExceptionNote:=sFailedText)
                        'Check for any error
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(ACClass, kMethodName & " Fails to UpdateRenewalExceptions", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        Return result
                    ElseIf gPMFunctions.ToSafeLong(CStr(m_vRenewalsData(ACIRenewalStatusTypeId, 0)), 0) = gPMConstants.PMBRenewalStatusTypeAwaitBrokerTransfer Then
                        sReportText = sReportText & CStr(m_vRenewalsData(ACIRenewalLivePolicy, 0)).Trim() & " - " &
                                      "Agent/Broker for this policy is in transfer mode. Please contact the System Administrator"
                    Else

                        m_lReturn = IsQuoted(v_lInsuranceFileCnt:=gPMFunctions.ToSafeLong(CStr(m_vRenewalsData(ACIRenewalPolicyCnt, 0)), 0), r_lResult:=lIsQuoted)

                        If lIsQuoted = gPMConstants.PMEReturnCode.PMTrue Then
                            sFailedText = ""
                            bOtherException = False

                            m_lReturn = CheckAcceptRenewal(v_iSelectedIndex:=0, v_lBatchRenewalJobID:=v_lBatchRenewalJobID, r_sFailedText:=sFailedText)

                            If sFailedText <> "" Then
                                sReportText = sReportText & CStr(m_vRenewalsData(ACIRenewalLivePolicy, 0)).Trim() & " - " &
                                              "" & sFailedText & Strings.ChrW(13) & Strings.ChrW(10)
                                bOtherException = True
                            Else
                                sReportText = sReportText & CStr(m_vRenewalsData(ACIRenewalLivePolicy, 0)).Trim() & " - " &
                                              "Successful" & Strings.ChrW(13) & Strings.ChrW(10)

                                m_lReturn = CancelMTAQuotes(ToSafeLong(m_vRenewalsData(ACIRenewalPolicyCnt, 0), 0),
                                                           ToSafeLong(m_vRenewalsData(ACIRenewalInsuranceFolder, 0), 0),
                                                               ToSafeLong(m_vRenewalsData(ACIRenewalInsuranceHolder, 0), 0))
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    'continue with the batch
                                End If
                            End If

                        ElseIf m_lReturn <> PMEReturnCode.PMTrue Then
                            'SP Fails or times out, then dont set status as Awaiting Manual Reivew
                            'As we dont really know if all the risk are quoted or not
                            sFailedText = "Failed to find if all risk are quoted."
                            Throw New Exception(sFailedText)
                        Else ' m_lReturn = PMTrue, but lIsQuoted <> PMTrue
                            sReportText = sReportText & CStr(m_vRenewalsData(ACIRenewalLivePolicy, 0)).Trim() & " - " &
                                          "Unquoted" & Strings.ChrW(13) & Strings.ChrW(10)

                            m_lReturn = SetRenewalStatus(v_lRenewalCnt:=m_vRenewalsData(ACIRenewalPolicyCnt, 0), v_iRenewalStatus:=1)

                        End If
                    End If
                End If
            Else
                bInvalidTMPFound = True
                lInvalidTMPCount += 1
            End If

            If bInvalidTMPFound Then
                sFailedText = "Anniversary renewal/s could not be processed. "
                sFailedText = sFailedText & "Anniversary Renewals cannot be accepted until "
                sFailedText = sFailedText & "the last monthly cycle has been accepted "
            End If

            If sFailedText <> "" Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()
                bTransactionOpen = False
            End If

            If bPolicyNumberToChange Then
                'Add Record to Batch_Renewal_Job_Runs
                m_lReturn = AddBatchRenewalJobRuns(v_lBatchRenewalJobID:=v_lBatchRenewalJobID, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_dRunDate:=ToSafeDate(DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss")), v_sFailureReason:="Product Flagged for policy number change - manual acceptance required", v_sDocumentPrinted:="", v_iIsFailed:=1, v_sGUID:=v_sGUID, r_lRecordsCount:=lRecordsCount, r_lBatchRenewalJobRunsID:=lBatchRenewalJobRunsID)
            ElseIf bNoRenewalInstalmentPlan Then
                'Add Record to Batch_Renewal_Job_Runs
                m_lReturn = AddBatchRenewalJobRuns(v_lBatchRenewalJobID:=v_lBatchRenewalJobID, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_dRunDate:=ToSafeDate(DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss")), v_sFailureReason:="Insufficient Instalment Details", v_sDocumentPrinted:="", v_iIsFailed:=1, v_sGUID:=v_sGUID, r_lRecordsCount:=lRecordsCount, r_lBatchRenewalJobRunsID:=lBatchRenewalJobRunsID)

            ElseIf bNoRenewalInstalmentPlanFound Then
                'Add Record to Batch_Renewal_Job_Runs
                m_lReturn = AddBatchRenewalJobRuns(v_lBatchRenewalJobID:=v_lBatchRenewalJobID, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_dRunDate:=ToSafeDate(DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss")), v_sFailureReason:="Payment Type is instalment but no valid plan found for renewal version - Manual reselection required", v_sDocumentPrinted:="", v_iIsFailed:=1, v_sGUID:=v_sGUID, r_lRecordsCount:=lRecordsCount, r_lBatchRenewalJobRunsID:=lBatchRenewalJobRunsID)

            ElseIf bPrepaymentRequired Then
                'Add Record to Batch_Renewal_Job_Runs
                m_lReturn = AddBatchRenewalJobRuns(v_lBatchRenewalJobID:=v_lBatchRenewalJobID, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_dRunDate:=ToSafeDate(DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss")), v_sFailureReason:="Prepayment Required", v_sDocumentPrinted:="", v_iIsFailed:=1, v_sGUID:=v_sGUID, r_lRecordsCount:=lRecordsCount, r_lBatchRenewalJobRunsID:=lBatchRenewalJobRunsID)
            ElseIf bInvalidTMPFound Then
                'Add Record to Batch_Renewal_Job_Runs
                m_lReturn = AddBatchRenewalJobRuns(v_lBatchRenewalJobID:=v_lBatchRenewalJobID, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_dRunDate:=ToSafeDate(DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss")), v_sFailureReason:="Anniversary Renewals cannot be accepted until the last monthly cycle has been accepted", v_sDocumentPrinted:="", v_iIsFailed:=1, v_sGUID:=v_sGUID, r_lRecordsCount:=lRecordsCount, r_lBatchRenewalJobRunsID:=lBatchRenewalJobRunsID)
            ElseIf bOtherException Then
                'Add Record to Batch_Renewal_Job_Runs
                m_lReturn = AddBatchRenewalJobRuns(v_lBatchRenewalJobID:=v_lBatchRenewalJobID, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_dRunDate:=ToSafeDate(DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss")), v_sFailureReason:=sFailedText, v_sDocumentPrinted:="", v_iIsFailed:=1, v_sGUID:=v_sGUID, r_lRecordsCount:=lRecordsCount, r_lBatchRenewalJobRunsID:=lBatchRenewalJobRunsID)
            ElseIf lIsQuoted <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = AddBatchRenewalJobRuns(v_lBatchRenewalJobID:=v_lBatchRenewalJobID, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_dRunDate:=ToSafeDate(DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss")), v_sFailureReason:="All risk are not quoted.", v_sDocumentPrinted:="", v_iIsFailed:=1, v_sGUID:=v_sGUID, r_lRecordsCount:=lRecordsCount, r_lBatchRenewalJobRunsID:=lBatchRenewalJobRunsID)
            Else
                'Add Record to Batch_Renewal_Job_Runs
                m_lReturn = AddBatchRenewalJobRuns(v_lBatchRenewalJobID:=v_lBatchRenewalJobID, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_dRunDate:=ToSafeDate(DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss")), v_sFailureReason:="", v_sDocumentPrinted:="", v_iIsFailed:=0, v_sGUID:=v_sGUID, r_lRecordsCount:=lRecordsCount, r_lBatchRenewalJobRunsID:=lBatchRenewalJobRunsID)

            End If

            If bTransactionOpen Then
                m_lReturn = m_oDatabase.SQLCommitTrans()
                bTransactionOpen = False
            End If

            Dim lReturn As Integer
            lReturn = ProcessRenewalAcceptanceEmail(m_lSelPartyCnt, ToSafeLong(m_vRenewalsData(ACIRenewalInsuranceFolder, 0), 0), v_lInsuranceFileCnt, v_lBatchRenewalJobID, False, m_vRenewalsData(ACIRenewalLivePolicy, 0))

            'UnLock the Key
        Catch ex As Exception
            If bTransactionOpen Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()
                bTransactionOpen = False
            End If

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

            If sFailedText.Length = 0 Then
                sFailedText = Informations.Err().Description
            End If

            If sFailedText.Length = 0 Then
                sFailedText = "Fails to Process Renewal Accptance"
            End If

            'Add Failure Reason to Batch_Renewal_Job_Runs
            m_lReturn = AddBatchRenewalJobRuns(v_lBatchRenewalJobID:=v_lBatchRenewalJobID, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_dRunDate:=DateTime.Now, v_sFailureReason:=sFailedText, v_sDocumentPrinted:="", v_iIsFailed:=1, v_sGUID:=v_sGUID, r_lRecordsCount:=lRecordsCount, r_lBatchRenewalJobRunsID:=lBatchRenewalJobRunsID)

            m_lReturn = oPMLock.UnLockKey(sKeyName:="RENACC", vKeyValue:=v_lInsuranceFileCnt, iUserID:=m_iUserID)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACClass, kMethodName & " Fails to UnLockKey", gPMConstants.PMELogLevel.PMLogError)
            End If

            oPMLock = Nothing
        Finally
            'UnLock the Key
            m_lReturn = oPMLock.UnLockKey(sKeyName:="RENACC", vKeyValue:=v_lInsuranceFileCnt, iUserID:=m_iUserID)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACClass, kMethodName & " Fails to UnLockKey", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not oPMLock Is Nothing Then
                oPMLock.Dispose()
                oPMLock = Nothing
            End If
        End Try
        Return m_lReturn
    End Function

    ' ***************************************************************** '
    ' Name: GetRenewalInvitationDetails
    ' Description: Get policy details that needs renewal
    ' ***************************************************************** '
    Public Function GetRenewalAcceptanceDetails(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetRenewalAcceptanceDetails"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameter
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACClass, kMethodName & " Fails to Add parameter Insurance_File_Cnt", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRenewalAcceptanceDetailsSQL, sSQLName:=ACGetRenewalAcceptanceDetailsName, bStoredProcedure:=ACGetRenewalAcceptanceDetailsStored, vResultArray:=r_vResultArray, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACClass, kMethodName & " Fails to Get Renewal Acceptance Details", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally
            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    'UPGRADE_NOTE: (7001) The following declaration (AllPolicyPayNow) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function AllPolicyPayNow(ByVal v_lInsuranceFileCnt As Integer, ByRef value As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Const kMethodName As String = "AllPolicyPayNow"
    '
    'If return value=0 : Payment Method of Selected Policies are either Invoice/Instalment
    'If return value=1 : Payment Method of Selected Policies is PayNow
    'If return value=2 : Payment Method of Selected Policies are either Paynow/Instalment/Invoice
    'Dim lPolicycnt As Integer
    'Dim iListCount As Integer
    'Dim sPaymentMethod As String = ""
    'Dim bPayNow, bInvoiceorInstalment As Boolean
    'Dim iPosInArray As Integer
    'Dim iVar As Integer
    '
    'On Error GoTo Catch_Renamed
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'm_lReturn = GetPaymentMethod(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_sPaymentMethod:=sPaymentMethod)
    'If sPaymentMethod = "PayNow" Then 'bPayNow = True
    'If sPaymentMethod = "Invoice" Or sPaymentMethod = "" Or sPaymentMethod = "Instalment" Then 'bInvoiceorInstalment = True
    '
    'If Not bPayNow And Not bInvoiceorInstalment Then 'value = 0
    'If Not bPayNow And bInvoiceorInstalment Then 'value = 0
    'If bPayNow And Not bInvoiceorInstalment Then 'value = 1
    'If bPayNow And bInvoiceorInstalment Then 'value = 2
    '
    'GoTo Finally_Renamed
    'Catch_Renamed: '
    ' DO Not Call any functions before here or the error will be lost
    'bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
    ' If you want to rollback a transaction or something, do it here
    '
    'Finally_Renamed: '
    'Return result
    'End Function

    'UPGRADE_NOTE: (7001) The following declaration (AllPolicyInvoice) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function AllPolicyInvoice(ByVal v_lInsuranceFileCnt As Object, ByRef value As Integer) As Integer
    '
    'If return value=1 : Payment Method of Selected Policies is Invoice
    'If return value=0: Payment Method of Selected Policies are either Paynow/Instalment/Invoice
    '
    'Dim result As Integer = 0
    'Const kMethodName As String = "AllPolicyInvoice"
    '
    'Dim sPaymentMethod As String = ""
    'Dim bInvoice, bPayNoworInstalment As Boolean
    '
    'On Error GoTo Catch_Renamed
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '

    'm_lReturn = GetPaymentMethod(v_lInsuranceFileCnt:=CInt(v_lInsuranceFileCnt), r_sPaymentMethod:=sPaymentMethod)
    '
    'If sPaymentMethod = "Invoice" Then 'bInvoice = True
    '
    'If sPaymentMethod = "PayNow" Or sPaymentMethod = "" Or sPaymentMethod = "Instalment" Or sPaymentMethod = "Direct Debit" Then 'bPayNoworInstalment = True
    '
    'If bInvoice And Not bPayNoworInstalment Then
    'value = 1
    'Else
    'value = 0
    'End If
    '
    'GoTo Finally_Renamed
    'Catch_Renamed: '
    ' DO Not Call any functions before here or the error will be lost
    'bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
    ' If you want to rollback a transaction or something, do it here
    '
    'Finally_Renamed: '
    'Return result
    'End Function
    ''' <summary>
    ''' ValidateTMPPolicy
    ''' </summary>
    ''' <param name="v_lSelectedIndex"></param>
    ''' <param name="r_bIsValid"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ValidateTMPPolicy(ByVal v_lSelectedIndex As Integer, ByRef r_bIsValid As Boolean) As Integer

        Dim nResult As Integer = 0
        Const kMethodName As String = "ValidateTMPPolicy"

        Dim nReturn As gPMConstants.PMEReturnCode
        Dim nNewPolicyCnt, nAnniversaryCopy As Integer
        Dim sInsuranceRef As String = ""
        Dim oValidationResults(,) As Object
        Dim bIsTrueMonthlyPolicy As Boolean
        Dim bAcceptIsValid As Boolean
        Dim dtRenewalDate As Date

        Try



            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' get renewal details
            nNewPolicyCnt = CInt(m_vRenewalsData(ACIRenewalPolicyCnt, v_lSelectedIndex))
            sInsuranceRef = CStr(m_vRenewalsData(ACIRenewalPolicy, v_lSelectedIndex))
            bIsTrueMonthlyPolicy = gPMFunctions.ToSafeLong(CStr(m_vRenewalsData(ACIRenewalProductIsTrueMonthlyPolicy, v_lSelectedIndex)), 0) = 1
            nAnniversaryCopy = gPMFunctions.ToSafeLong(CStr(m_vRenewalsData(ACIRenewalAnniversaryCopy, v_lSelectedIndex)), 0)
            dtRenewalDate = CDate(m_vRenewalsData(ACIRenewalCoverStartDate, v_lSelectedIndex))

            ' default to accept being a valid action
            bAcceptIsValid = True

            ' if this is an "anniversary copy" version of a policy
            ' based on a "true monthly policy" based product
            If bIsTrueMonthlyPolicy Then

                If nAnniversaryCopy Then

                    ' then validate that the last item of the previous cycle
                    ' has already been accepted if not then this item cannot be accepted
                    nReturn = ValidateAcceptTMPAnniversaryIsValidAction(v_lInsuranceFileCnt:=nNewPolicyCnt, v_dRenewalDate:=dtRenewalDate, r_vResults:=oValidationResults)


                    If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "IsAcceptTMPValid Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    If Informations.IsArray(oValidationResults) Then

                        If gPMFunctions.ToSafeLong(CStr(oValidationResults(0, 0)), 0) = 0 Then
                            bAcceptIsValid = False
                        Else
                            bAcceptIsValid = True
                        End If
                    Else
                        bAcceptIsValid = False
                    End If
                End If
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            r_bIsValid = bAcceptIsValid

        End Try
        Return nResult
    End Function

    ' ***************************************************************** '
    ' Name: ValidateRenewalAndUpdateExceptions || to ValidateRenewalAcceptance of iPMURenewal
    '
    ' Description: This function is || to ValidateRenewalAcceptance of iPMURenewal
    '
    ' History:
    '           Created : Pankaj : 13-06-2008 : Unattended Renewals
    ' ***************************************************************** '
    Private Function ValidateRenewalAndUpdateExceptions(ByVal v_lInsuranceFileCnt As Integer, ByRef r_bPolicyNumberToChange As Boolean, ByRef r_bNoRenewalInstalmentPlan As Boolean, ByRef r_bPrepaymentRequired As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ValidateRenewalAndUpdateExceptions"


        Try
            Dim sExceptionNote, sMsg As String

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = ValidateRenewalAcceptance(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_bPolicyNumberToChange:=r_bPolicyNumberToChange, r_bNoRenewalInstalmentPlan:=r_bNoRenewalInstalmentPlan, r_bPrepaymentRequired:=r_bPrepaymentRequired)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMsg = "Failed to validate renewal acceptance"
                Throw New Exception(sMsg)
            End If

            sExceptionNote = ""

            '1. Set Exception Reason when policy flagged for policy number change
            If r_bPolicyNumberToChange Then

                m_lReturn = UpdateRenewalExceptions(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRenewalExceptionReasonID:=1, v_sRenewalExceptionNote:=sExceptionNote)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMsg = "Failed to Update Renewal Exceptions"
                    Throw New Exception(sMsg)
                End If

                Return result
            End If

            '2. Set Exception Reason when bank details are missing
            If r_bNoRenewalInstalmentPlan Then

                m_lReturn = UpdateRenewalExceptions(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRenewalExceptionReasonID:=2, v_sRenewalExceptionNote:=sExceptionNote)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMsg = "Failed to Update Renewal Exceptions"
                    Throw New Exception(sMsg)
                End If

                Return result
            End If

            '3. Set Exception Reason when Prepayment Required
            If r_bPrepaymentRequired Then

                m_lReturn = UpdateRenewalExceptions(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRenewalExceptionReasonID:=3, v_sRenewalExceptionNote:=sExceptionNote)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMsg = "Failed to Update Renewal Exceptions"
                    Throw New Exception(sMsg)
                End If
                Return result
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    Private Function CheckAcceptRenewal(ByVal v_iSelectedIndex As Integer, ByVal v_lBatchRenewalJobID As Integer, ByRef r_sFailedText As Object) As Integer
        Dim nResult As Integer = 0
        Dim lOldPolicyCnt, lRenewalStatusCnt, lNewPolicyCnt, lInsuranceFolder, lPartyCnt As Integer
        Dim sInsuranceRef As String = ""
        Dim bProduceSchedule, bProduceDebitNote, bProduceCertificate As Boolean
        Dim vPrintOptions As Object
        Dim lProductId As Integer

        Dim bIsTrueMonthlyPolicy As Boolean
        Dim lAnniversaryCopy As Integer
        Dim bGenerateDocuments As Boolean

        Dim iRenewalDocDestination, iReportSortOrder As Integer

        'Const ACProcessing As String = "Processing Acceptance ... "
        Const kMethodName As String = "CheckAcceptRenewal"

        Try

            nResult = PMEReturnCode.PMTrue

            lOldPolicyCnt = CInt(m_vRenewalsData(ACIRenewalLivePolicyCnt, v_iSelectedIndex))
            lRenewalStatusCnt = CInt(m_vRenewalsData(ACIRenewalStatusId, v_iSelectedIndex))
            lNewPolicyCnt = CInt(m_vRenewalsData(ACIRenewalPolicyCnt, v_iSelectedIndex))
            lInsuranceFolder = CInt(m_vRenewalsData(ACIRenewalInsuranceFolder, v_iSelectedIndex))
            lPartyCnt = CInt(m_vRenewalsData(ACIRenewalInsuranceHolder, v_iSelectedIndex))
            sInsuranceRef = ToSafeString(CStr(m_vRenewalsData(ACIRenewalPolicy, v_iSelectedIndex)))
            bIsTrueMonthlyPolicy = ToSafeLong(CStr(m_vRenewalsData(ACIRenewalProductIsTrueMonthlyPolicy, v_iSelectedIndex)), 0) = 1
            lAnniversaryCopy = ToSafeLong(CStr(m_vRenewalsData(ACIRenewalAnniversaryCopy, v_iSelectedIndex)), 0)
            lProductId = ToSafeLong(CStr(m_vRenewalsData(ACIRenewalProductId, v_iSelectedIndex)), 0)

            Dim sTransactionId As String = ""
            Dim sIntegerationId As String = ""
            Dim sTokenId As String = ""
            Dim sPaymentMethod As String = ""
            Dim sMediaType As String = ""
            Dim dPremiumAmount As Decimal = 0
            Dim sCurrencyCode As String = ""
            bProcessCashList = False
            Dim obPaymenthub As bSIRPaymentHubWrapper.Business = Nothing
            If bPaymentHubEnabled Then

                nResult = GetCreditCardDetails(v_nOldInsuranceFileCnt:=lOldPolicyCnt, v_nNewInsuranceFileCnt:=lNewPolicyCnt, r_sTransactionId:=sTransactionId, r_sIntegerationToken:=sIntegerationId,
                                                         r_sTokenId:=sTokenId, r_sPaymentMethod:=sPaymentMethod, r_sMediaType:=sMediaType, r_dPremiumAmount:=dPremiumAmount, r_sCurrencyCode:=sCurrencyCode)

                If nResult <> PMEReturnCode.PMTrue Then
                    Return PMEReturnCode.PMFalse
                End If

                If sPaymentMethod.ToUpper = "PAYNOW" AndAlso sMediaType IsNot Nothing AndAlso sMediaType.ToUpper = "OCP" Then
                    Dim sSetStatusToManualReview As String
                    If bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=5198, r_sOptionValue:=sSetStatusToManualReview) <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        Return nResult
                    End If
                    If Not String.IsNullOrEmpty(sSetStatusToManualReview) AndAlso sSetStatusToManualReview = "1" Then
                        nResult = SetRenewalStatus(v_lRenewalCnt:=m_vRenewalsData(ACIRenewalPolicyCnt, 0), v_iRenewalStatus:=1)
                        Return nResult
                    End If
                    obPaymenthub = New bSIRPaymentHubWrapper.Business()
                    nResult = obPaymenthub.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))

                    If nResult <> PMEReturnCode.PMTrue Then
                        Return PMEReturnCode.PMFalse
                    End If
                    Dim sStatus As String
                    Dim oPaymentHubResponseParameters As New bSIRPaymentHubWrapper.PaymentHubResponseParameters()
                    sStatus = obPaymenthub.ProcessPurchase(strTransactionID:=sTransactionId, IntegrationToken:=sIntegerationId,
                                                               TokenID:=sTokenId, oPaymentHubResponseParameters:=oPaymentHubResponseParameters,
                                                               v_dTransactionValue:=dPremiumAmount, v_sTransactionCurrencyCode:=sCurrencyCode, v_nPartyCnt:=lPartyCnt)

                    If sStatus Is Nothing OrElse sStatus.ToUpper <> "0" Then
                        nResult = SetRenewalStatus(v_lRenewalCnt:=m_vRenewalsData(ACIRenewalPolicyCnt, 0), v_iRenewalStatus:=1)

                        'EventType -Payment Hub Failure
                        m_lReturn = CreateEvent(v_vEventCnt:=0, v_vPartyCnt:=lPartyCnt, v_vInsuranceFolderCnt:=lInsuranceFolder, v_vInsuranceFileCnt:=lNewPolicyCnt, v_vClaimCnt:=DBNull.Value, v_vDocumentCnt:=DBNull.Value, v_vOldAddressCnt:=DBNull.Value, v_vNewAddressCnt:=DBNull.Value, v_vCampaignId:=DBNull.Value, v_vDocumentType:=DBNull.Value, v_vReportType:=DBNull.Value, v_vEventType:=14, v_vUserId:=m_iUserID, v_vEventDate:=DateTime.Today, v_vDescription:="Payment Hub Failure")

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            'Log Error Message
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add event.", vApp:=ACApp, vClass:=ACClass, vMethod:="LapseRenewal", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                            Return nResult
                        End If

                        Return nResult
                    End If
                    bProcessCashList = True
                End If
            End If

            nResult = AcceptRenewal(lOldPolicyCnt, lNewPolicyCnt, lRenewalStatusCnt, r_sFailureMessage:=r_sFailedText) '(RC) QBENZ014

            If r_sFailedText <> "" Then
                Return nResult
            End If

            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            'Create Stats (Needs insurance_file_cnt)
            nResult = GetStats(lNewPolicyCnt)
            If nResult = PMEReturnCode.PMCancel Then
                Return PMEReturnCode.PMCancel
            End If
            If nResult <> PMEReturnCode.PMTrue Then
                r_sFailedText = "Failed to generate statistics"

                Return PMEReturnCode.PMFalse
            End If
            If bProcessCashList Then
                If obPaymenthub Is Nothing Then
                    obPaymenthub = New bSIRPaymentHubWrapper.Business()
                    nResult = obPaymenthub.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))

                    If nResult <> PMEReturnCode.PMTrue Then
                        r_sFailedText = "Failed to get Payment HUB Wrapper object"
                        Return PMEReturnCode.PMFalse
                    End If

                End If
                If obPaymenthub IsNot Nothing AndAlso dPremiumAmount <> 0 Then
                    nResult = obPaymenthub.AddAndUpdateCashListDetails(lOldPolicyCnt.ToString(), lNewPolicyCnt.ToString(), sTokenId, dPremiumAmount)
                    If nResult <> PMEReturnCode.PMTrue Then
                        r_sFailedText = "Failed to create and allocate cashlist"
                        Return PMEReturnCode.PMFalse
                    End If
                End If
            End If
            nResult = CreateEvent(v_vEventCnt:=0, v_vPartyCnt:=lPartyCnt, v_vInsuranceFolderCnt:=lInsuranceFolder, v_vInsuranceFileCnt:=lNewPolicyCnt, v_vEventType:=5, v_vUserId:=m_iUserID, v_vEventDate:=DateTime.Today, v_vDescription:="Accept Renewal - " & CStr(m_vRenewalsData(ACIRenewalPolicy, v_iSelectedIndex)))

            If nResult <> PMEReturnCode.PMTrue Then
                r_sFailedText = "Failed to create event"
                Return PMEReturnCode.PMFalse
            End If

            If ToSafeLong(CStr(m_vRenewalsData(ACIRenewalIsInTransferMode, v_iSelectedIndex)), 0) <> 0 Then
                nResult = CreateEvent(v_vEventCnt:=0, v_vPartyCnt:=lPartyCnt, v_vInsuranceFolderCnt:=lInsuranceFolder, v_vInsuranceFileCnt:=lNewPolicyCnt, v_vEventType:=5, v_vUserId:=m_iUserID, v_vEventDate:=DateTime.Today, v_vDescription:="Renewal Accepted - Broker Transfer From " & CStr(m_vRenewalsData(ACIRenewalLeadAgentCode, v_iSelectedIndex)) &
                            " to " & CStr(m_vRenewalsData(ACIRenewalTransferToShortname, v_iSelectedIndex)))

                If nResult <> PMEReturnCode.PMTrue Then
                    r_sFailedText = "Failed to create event"
                    Return PMEReturnCode.PMFalse
                End If
            End If

            bGenerateDocuments = True

            If bIsTrueMonthlyPolicy Then
                If lAnniversaryCopy <> 1 Then
                    bGenerateDocuments = False
                End If
            End If

            If bGenerateDocuments Then

                nResult = GetBatchJobPrintingOptions(v_lBatchRenewalJobID:=v_lBatchRenewalJobID, r_iRenewalDocDestination:=iRenewalDocDestination, r_iReportSortOrder:=iReportSortOrder)

                If nResult <> PMEReturnCode.PMTrue Then
                    Throw New Exception
                End If

                'If no document printing require than exit from docs printing process
                If iRenewalDocDestination = 0 Then
                    Return nResult
                End If

                If iRenewalDocDestination = PMRenewalDocDestination_Print Then
                    iRenewalDocDestination = gSIRLibrary.ACPrintSilentMode
                Else
                    iRenewalDocDestination = gSIRLibrary.ACSpoolDocMode
                End If

                'm_lReturn = GenerateCustomerRenewalEmail(v_lPartyCnt:=lPartyCnt, v_lInsuranceFileCnt:=lNewPolicyCnt, v_sType:="acceptance")
                'If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then

                m_lReturn = GetProdPrintOptions(lProductId, vPrintOptions)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Informations.IsArray(vPrintOptions) Then
                    bProduceSchedule = ToSafeBoolean(vPrintOptions(0, 0))
                    bProduceCertificate = ToSafeBoolean(vPrintOptions(1, 0))
                    bProduceDebitNote = ToSafeBoolean(vPrintOptions(2, 0))
                End If

                If (m_sRenSchedulePrinting = "1" Or m_sRenSchedulePrinting = "0") And bProduceSchedule Then
                    'Generate schedule document.
                    nResult = GenerateDocument(ACDocTypeSchedule, iRenewalDocDestination, lNewPolicyCnt, lInsuranceFolder, lPartyCnt, "Accept Renewal - Schedule Document", v_sTransactionType:="RN", v_bCalledFromSAM:=True)


                    If nResult <> PMEReturnCode.PMTrue Then
                        r_sFailedText = "Failed to generate schedule document"
                        Return PMEReturnCode.PMFalse
                    End If
                End If

                If (m_sRenCertificatePrinting = "1" Or m_sRenCertificatePrinting = "0") And bProduceCertificate Then
                    'Generate certificate document.
                    nResult = GenerateDocument(ACDocTypeCertificate, iRenewalDocDestination, lNewPolicyCnt, lInsuranceFolder, lPartyCnt, "Accept Renewal -  Certificate Document", v_sTransactionType:="RN", v_bCalledFromSAM:=False)

                    If nResult <> PMEReturnCode.PMTrue Then
                        r_sFailedText = "Failed to generate certificate document"

                        Return PMEReturnCode.PMFalse
                    End If
                End If

                If (m_sRenDebitNotePrinting = "1" Or m_sRenDebitNotePrinting = "0") And bProduceDebitNote Then
                    'DN 21/02/03 - Generate debit note.
                    'PSL 10/09/2003 Iss 6535 needs a seperate debit note to new business
                    nResult = GenerateDocument(ACDOCTypeDebitNote, iRenewalDocDestination, lNewPolicyCnt, lInsuranceFolder, lPartyCnt, "Accept Renewal -  Debit Note Document", v_sTransactionType:="RN")

                    If nResult <> PMEReturnCode.PMTrue Then
                        r_sFailedText = "Failed to generate debit note"
                        Return PMEReturnCode.PMFalse
                    End If
                End If
            End If

        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
            r_sFailedText = ex.Message
        End Try
        Return nResult
    End Function

    'Gives the grossTotal of all the Selected Policies
    'UPGRADE_NOTE: (7001) The following declaration (GetGrossTotal) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetGrossTotal(ByRef value As Decimal) As Integer
    '
    'Dim result As Integer = 0
    'Const kMethodName As String = "GetGrossTotal"
    '
    'Dim iListCount As Integer
    'Dim lReturn As gPMConstants.PMEReturnCode
    'Dim vResult As Object
    'Dim vGrosstotal As Decimal
    'Dim sErrMsg As String = ""
    'Dim iPosInArray As Integer
    'Dim iVar As Integer
    'Dim sAgentType As String = ""
    'Dim cAgentcommission As Decimal
    '
    'On Error GoTo Catch_Renamed
    '
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'lReturn = CType(GetPolicyGrossTotal(v_lInsuranceFileCnt:=CInt(m_vRenewalsData(ACIRenewalPolicyCnt, 0)), r_vResults:=vResult), gPMConstants.PMEReturnCode)
    '
    'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'sErrMsg = "Failed to Get GetPolicyGrossTotal"
    'result = gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'If Informations.IsArray(vResult) Then

    'vGrosstotal += CDec(vResult(4, 0))
    'End If
    '

    'vResult = ""
    '
    'lReturn = CType(GetAgentCommission(v_lInsuranceFileCnt:=CInt(m_vRenewalsData(ACIRenewalPolicyCnt, 0)), r_vResults:=vResult), gPMConstants.PMEReturnCode)
    '
    'If Informations.IsArray(vResult) Then

    'sAgentType = CStr(vResult(1, 0)).Trim()
    'If sAgentType = "Broker" Then

    'cAgentcommission = gPMFunctions.ToSafeCurrency(CStr(vResult(6, 0))) + gPMFunctions.ToSafeCurrency(CStr(vResult(16, 0)))
    'vGrosstotal -= cAgentcommission
    'End If
    'End If
    '
    '
    'value = vGrosstotal
    '
    'GoTo Finally_Renamed
    '
    'Catch_Renamed: '
    '
    ' DO Not Call any functions before here or the error will be lost
    'bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
    '
    ' If you want to rollback a transaction or something, do it here
    '
    'Finally_Renamed: '
    '
    'Return result
    'Resume 
    'Return result
    'End Function

    Private Function AddBatchRenewalJobRuns(ByVal v_lBatchRenewalJobID As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_dRunDate As Date, ByVal v_sFailureReason As String, ByVal v_sDocumentPrinted As Object, ByVal v_iIsFailed As Integer, ByVal v_sGUID As String, ByRef r_lRecordsCount As Integer, ByRef r_lBatchRenewalJobRunsID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddBatchRenewalJobRuns"


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oDatabase.SQLBeginTrans()

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="batch_renewal_job_id", vValue:=CStr(v_lBatchRenewalJobID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="run_date", vValue:=v_dRunDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="failure_reason", vValue:=v_sFailureReason, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="document_printed", vValue:=CStr(v_sDocumentPrinted), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_failed", vValue:=CStr(v_iIsFailed), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="GUID", vValue:=v_sGUID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Batch_Renewal_Job_Runs_ID", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Record_Count", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddBatchRenewalJobRunsSQL, sSQLName:=ACAddBatchRenewalJobRunsName, bStoredProcedure:=ACAddBatchRenewalJobRunsStored)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.SQLCommitTrans()

            r_lBatchRenewalJobRunsID = m_oDatabase.Parameters.Item("Batch_Renewal_Job_Runs_ID").Value
            r_lRecordsCount = m_oDatabase.Parameters.Item("Record_Count").Value


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If



        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            m_lReturn = m_oDatabase.SQLRollbackTrans()
        Finally
        End Try
        Return result
    End Function

    Private Function GetBatchJobPrintingOptions(ByVal v_lBatchRenewalJobID As Integer, ByRef r_iRenewalDocDestination As Integer, ByVal r_iReportSortOrder As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetBatchJobPrintingOptions"

        Dim vResultArray(,) As Object = Nothing

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            r_iRenewalDocDestination = 0
            r_iReportSortOrder = 0

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Batch_Renewal_Job_Id", vValue:=CStr(v_lBatchRenewalJobID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetBatchJobPrintingOptionsSQL, sSQLName:=ACGetBatchJobPrintingOptionsName, bStoredProcedure:=ACGetBatchJobPrintingOptionsStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            If Informations.IsArray(vResultArray) Then

                r_iRenewalDocDestination = gPMFunctions.ToSafeLong(CStr(vResultArray(0, 0)))

                r_iReportSortOrder = gPMFunctions.ToSafeLong(CStr(vResultArray(1, 0)))
            End If




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here


        Finally
        End Try

        Return result
    End Function

    '*************************************************************************
    ' Name: ProcessInterface (Standard Method)
    '
    ' Description: Calls the appropriate methods to process the
    '              interface.
    '
    ' History    :  Kevin Renshaw (CMG) set m_bIsMTAWithNoPremium flag
    '               Kevin Renshaw (CMG) 11/04/2003 Display a message with the number of
    '               instalment payments remaining
    '               Tracy Richards - 25/06/03 - Commented out
    '               m_bIsMTAWithNoPremium as New Business can also have 0
    '               premiums (requirement for TPU)
    '*************************************************************************
    Private Function GetStats(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim sFailureReason As String = ""

        ' DD 7-2-2002 : Added for PF functions
        Dim oPFBusiness As bSIRPremiumFinance.Business
        Dim oBusiness As bControlTrans.Automated
        Dim lCashListID, lTransactionID As Integer
        Dim sOldPolicyNumber As String = ""
        Dim iDebitAgainst As Integer
        Dim lPaymentAccountID As Integer
        Dim vCreditTransactions As Object = Nothing
        Dim lCashListItemID As Integer
        Dim cTransactionAmount As Decimal
        Dim vTransactionArray(,) As Object = Nothing
        Dim vPlanArray(,) As Object = Nothing
        Dim lPlanTransDetailID As Integer
        Dim lPfPremFinanceCnt, lPfPremFinanceVersion As Object

        'PN4644-Start
        Dim lOldPlanCnt As Long
        Dim lVersion As Long
        Dim vMediaHistoryPrevId As Object
        Dim vMediaHistoryCurrId As Object
        Dim lPlanRefChanged As Long
        Dim sActionCode As String
        Dim sMediaValidation As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        'Create the Transaction object
        Dim obSIRPaymentHubWrapper As bSIRPaymentHubWrapper.Business()


        oBusiness = New bControlTrans.Automated
        m_lReturn = oBusiness.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceId:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Set the Insurance file count

        oBusiness.InsuranceFileCnt = v_lInsuranceFileCnt


        m_lReturn = oBusiness.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            oBusiness.Dispose()
            oBusiness = Nothing
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = oBusiness.Start(lPaymentAccountID, iDebitAgainst, vCreditTransactions, lCashListID, lCashListItemID, lTransactionID, cTransactionAmount, sOldPolicyNumber)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Dim failureReason As String
            failureReason = oBusiness.Message
            Throw New Exception(failureReason)
        End If

        ' DD 5-2-2002 : Get PF Transactions for going through the Navigator

        m_lReturn = oBusiness.GetPFTransactions(v_lInsuranceFileCnt, vTransactionArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New Exception()
        End If

        oPFBusiness = New bSIRPremiumFinance.Business
        m_lReturn = oPFBusiness.Initialise(sUserName:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))

        m_lReturn = oPFBusiness.GetSingleFinancePlanFromInsFileCnt(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vPFPremiumFinance:=vPlanArray)

        'Convert the Plan to Live
        If Informations.IsArray(vPlanArray) Then
            m_lReturn = oPFBusiness.TranslateQuoteToPlan(vPlanArray, vTransactionArray)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue And Informations.IsArray(vPlanArray) Then

                lPfPremFinanceCnt = vPlanArray(0, 0)

                lPfPremFinanceVersion = vPlanArray(1, 0)
                sActionCode = "Setup"

                m_lReturn = oPFBusiness.SaveInstalmentsPlanMediaTypeDetails(lPfPremFinanceCnt, lPfPremFinanceVersion, sActionCode)
                m_lReturn = oPFBusiness.TransactPlanInHouse(vPremiumFinance:=vPlanArray, vPremiumFinanceTransactions:=vTransactionArray, lPlanTransDetailID:=lPlanTransDetailID)
            End If
        End If


        'PN4644-Start
        If Informations.IsArray(vPlanArray) Then
            lPfPremFinanceCnt = ToSafeLong(vPlanArray(0, 0))
            lPfPremFinanceVersion = ToSafeLong(vPlanArray(1, 0))
            sMediaValidation = gPMFunctions.ToSafeString(vPlanArray(bSIRPremFinConst.k_PFPlanMediaTypeValidation, 0)).Trim()
        End If
        sActionCode = "Setup"
        If ToSafeString(Trim(sActionCode)) <> "" Then
            m_lReturn = oPFBusiness.SaveInstalmentsPlanMediaTypeDetails(lPfPremFinanceCnt, lPfPremFinanceVersion, sActionCode)
        End If

        m_lReturn = oPFBusiness.GetOldPremimFinanceCnt(lPfPremFinanceCnt, lOldPlanCnt, lVersion, lPlanRefChanged)
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = oPFBusiness.GetMediaHistoryId(lPremFinanceCnt:=lPfPremFinanceCnt,
                                                lPremFinanceVersion:=lPfPremFinanceVersion,
                                                vMediaHistoryPrev:=vMediaHistoryPrevId,
                                                vMediaHistoryCurrent:=vMediaHistoryCurrId)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                If lPlanRefChanged = 1 AndAlso sMediaValidation = "BANK" Then
                    'fetch old plan media type id
                    m_lReturn = oPFBusiness.GetMediaHistoryId(lPremFinanceCnt:=lOldPlanCnt,
                                                lPremFinanceVersion:=lVersion,
                                                vMediaHistoryPrev:=vMediaHistoryPrevId,
                                                vMediaHistoryCurrent:=vMediaHistoryCurrId)

                    m_lReturn = oPFBusiness.CreateInstalmentNotification _
                                                (lOldPlanCnt,
                                                 lVersion,
                                                 "0C",
                                                 vMediaHistoryCurrId)

                    'fetch new plan media type id
                    m_lReturn = oPFBusiness.GetMediaHistoryId(lPremFinanceCnt:=lPfPremFinanceCnt,
                                                lPremFinanceVersion:=lPfPremFinanceVersion,
                                                vMediaHistoryPrev:=vMediaHistoryPrevId,
                                                vMediaHistoryCurrent:=vMediaHistoryCurrId)

                    m_lReturn = oPFBusiness.CreateInstalmentNotification _
                                                (lPfPremFinanceCnt,
                                                 lPfPremFinanceVersion,
                                                 "0N",
                                                 vMediaHistoryCurrId)
                End If
            End If
        End If
        'PN4644-End

        oBusiness.Dispose()
        oPFBusiness.Dispose()
        oPFBusiness = Nothing
        oBusiness = Nothing

        Return result

    End Function

    'Document Production Business call
    Public Function GenerateDocument(ByVal v_iDocType As Integer, ByVal v_iMode As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_sSpoolDesc As String, ByVal v_sTransactionType As Object, Optional ByVal v_bCalledFromSAM As Boolean = False, Optional ByVal v_sInsuranceFileRef As String = "") As Integer

        Dim result As Integer = 0
        Dim lDocId, lDocTypeId As Integer
        Dim vDocumentArray(,) As Object = Nothing
        Dim bDocSpooled As Boolean
        Dim sDocDesc, sMergeFilePath, v_sDocTemplateCode As String
        Dim lCount As Integer
        Dim m_oBusiness As New bSIRDocTemplate.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oBusiness.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSIRDocTemplate.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseBusiness")
                Return result
            End If

            m_lReturn = GetTheTemplate(r_lDocumentTemplateId:=lDocId, r_lDocumentTypeId:=lDocTypeId,
                                       v_lProcessType:=v_iDocType, v_sTransactionType:=v_sTransactionType,
                                       v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vDocumentArray:=vDocumentArray)

            If Not Informations.IsArray(vDocumentArray) Then
                Return result
            End If

            If vDocumentArray(8, lCount) = 2 Then
                v_iMode = ACUserChoice
            Else
                If vDocumentArray(8, lCount) = 0 Then
                    v_iMode = ACPrintSilentMode
                ElseIf vDocumentArray(8, lCount) = 1 Then
                    v_iMode = ACSpoolDocMode
                End If
            End If


            If Informations.IsArray(vDocumentArray) Then
                For lCount = vDocumentArray.GetLowerBound(1) To vDocumentArray.GetUpperBound(1)


                    Dim bSendEMailToClient As Boolean = False
                    Dim bSendEMailToAgent As Boolean = False

                    ' set default LETTER - maintain backward compatibility
                    Dim sCorrespondenceType As String = "LETTER"
                    Dim sDefaultPreferredCorrespondenceType As String = "LETTER"
                    Dim bIsAgentCorrespondence As Boolean = False

                    If ToSafeInteger(vDocumentArray(1, lCount)) = ACEmailDocType Then

                        sCorrespondenceType = ToSafeString(vDocumentArray(ACCorrespondenceType, lCount)).Trim
                        sDefaultPreferredCorrespondenceType = ToSafeString(vDocumentArray(ACDefaultPreferredCorrespondenceType, lCount)).Trim
                        bIsAgentCorrespondence = ToSafeBoolean(vDocumentArray(ACIsAgentReceiveCorrespondence, lCount))

                        If v_sTransactionType = "RNI" OrElse v_sTransactionType = "RN" Then
                            bSendEMailToClient = True
                            bSendEMailToAgent = True
                        End If

                        'No Agent business
                        If ToSafeInteger(vDocumentArray(ACLeadAgentCnt, lCount)) = 0 Then
                            bSendEMailToAgent = False
                            bIsAgentCorrespondence = False 'no correspondence to agent
                        End If

                        If bSendEMailToAgent Then
                            If Not bIsAgentCorrespondence OrElse (sCorrespondenceType = "DEFAULT" AndAlso sDefaultPreferredCorrespondenceType <> "MEMAIL") _
                                OrElse sCorrespondenceType = "LETTER" Then
                                bSendEMailToAgent = False
                            End If
                        End If

                        If bSendEMailToClient Then
                            If bIsAgentCorrespondence OrElse (sCorrespondenceType = "DEFAULT" AndAlso sDefaultPreferredCorrespondenceType <> "MEMAIL") _
                                OrElse sCorrespondenceType = "LETTER" Then
                                bSendEMailToClient = False
                            End If
                        End If

                        Dim vEmailDocsPathArray As Object

                        If bSendEMailToClient Or bSendEMailToAgent Then
                            Dim sDocMergedPath As String = ""

                            Dim sDocCode As String = ""
                            Dim iDocTemplateID As Integer = 0
                            Dim iDocTypeId As Integer = 0
                            Dim vEMailDocs As New List(Of String)
                            Dim vEMailDocPath As New List(Of String)
                            Dim bOutputAsPDF As Boolean = False
                            Dim sEmailAddress As String = String.Empty
                            Dim bOutPutHTM As Boolean = False
                            Dim bOutPutTXT As Boolean = False

                            Dim sAttachmentSTR() As String = ToSafeString(vDocumentArray(ACEMailAttachementTemplateCodes, lCount)).Split(",")

                            ReDim vEmailDocsPathArray(sAttachmentSTR.GetUpperBound(0) + 2)

                            vEMailDocs.Add(ToSafeString(vDocumentArray(0, lCount)))
                            vEMailDocs.Add(ToSafeString(vDocumentArray(ACEMailSubTemplateCode, lCount)))

                            For iAttachmentCnt As Integer = 0 To sAttachmentSTR.GetUpperBound(0)
                                vEMailDocs.Add(ToSafeString(sAttachmentSTR(iAttachmentCnt).Trim))
                            Next

                            Dim sSubjectContent As String = ""

                            For iDocCnt As Integer = 0 To vEMailDocs.Count - 1
                                sDocCode = vEMailDocs.Item(iDocCnt)
                                iDocTemplateID = 0
                                iDocTypeId = 0
                                bOutputAsPDF = False
                                bOutPutHTM = False
                                bOutPutTXT = False
                                sDocMergedPath = String.Empty

                                If iDocCnt = 0 Then
                                    iDocTemplateID = ToSafeInteger(sDocCode)
                                    iDocTypeId = ToSafeInteger(vDocumentArray(1, lCount))
                                    bOutPutHTM = True
                                Else
                                    m_lReturn = GetDocTemplateTypeID(v_sDocCode:=sDocCode,
                                                              r_lDocTemplateTypeId:=iDocTemplateID,
                                                              r_lDocTypeID:=iDocTypeId)

                                    If iDocCnt = 1 Then
                                        bOutPutTXT = True
                                    Else
                                        bOutputAsPDF = True
                                    End If
                                End If
                                If iDocTemplateID <> 0 And iDocTypeId <> 0 Then
                                    m_lReturn = PrintDocument(v_lPartyCnt:=v_lPartyCnt,
                                                             v_lDocTemplateID:=iDocTemplateID,
                                                             v_sType:="",
                                                             r_sMergedFilePath:=sDocMergedPath,
                                                             v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                                                             r_sDocDesc:="",
                                                             v_lDocumentTypeId:=iDocTypeId,
                                                             v_iProductionOrder:=gPMFunctions.ToSafeInteger(CStr(vDocumentArray(5, lCount)), 0),
                                                             v_iMode:=gSIRLibrary.ACEmailMode, v_bCalledFromSAM:=v_bCalledFromSAM,
                                                             v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt,
                                                             v_sInsuranceFileRef:=v_sInsuranceFileRef, v_bOutputAsHTM:=bOutPutHTM,
                                                             v_bOutPutAsTXT:=bOutPutTXT, r_bRetainTempFiles:=True, v_bOutputAsPDF:=bOutputAsPDF)

                                    If iDocCnt = 1 And Not String.IsNullOrEmpty(sDocMergedPath) Then
                                        Dim srSubjectTmp As New StreamReader(File.OpenRead(sDocMergedPath))
                                        Dim sbsbTemplate As New StringBuilder(srSubjectTmp.ReadToEnd())
                                        sbsbTemplate.Replace(vbCrLf, " ")
                                        sSubjectContent = sbsbTemplate.ToString
                                    End If


                                    If (String.IsNullOrEmpty(sSubjectContent)) Then
                                        If v_sTransactionType = "RNI" Then
                                            sSubjectContent = "Renewal Invite Notification"
                                        ElseIf v_sTransactionType = "RN" Then
                                            sSubjectContent = "Renewal Acceptance Notification"
                                        End If
                                    End If
                                    vEMailDocPath.Add(sDocMergedPath)
                                End If
                            Next

                            Dim v_Attachment As New List(Of String)

                            For iDocCnt As Integer = 2 To vEMailDocPath.Count - 1
                                v_Attachment.Add(vEMailDocPath(iDocCnt))
                            Next

                            m_lReturn = m_oBusiness.GetPolicyLevelEmailAddress(v_lInsurance_File_Cnt:=v_lInsuranceFileCnt, v_sRecipientEmailAddress:=sEmailAddress)
                            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                                Return result
                            End If

                            Dim sEmailFilePath As String = String.Empty
                            If bSendEMailToClient Then
                                sEmailAddress = String.Empty
                                If (sCorrespondenceType = "DEFAULT" AndAlso sDefaultPreferredCorrespondenceType = "MEMAIL") OrElse sCorrespondenceType = "EMAIL" Then
                                    If String.IsNullOrEmpty(sEmailAddress) Then
                                        m_lReturn = GetEmailAddress(v_lPartyCnt, "MEMAIL", sEmailAddress)
                                    End If
                                Else
                                    m_lReturn = GetBranchEmailAddress(m_iSourceID, sEmailAddress)
                                End If
                                If Not String.IsNullOrEmpty(sEmailAddress) Then
                                    m_lReturn = m_oBusiness.SendEMail(v_sTo:=sEmailAddress,
                                                                     v_sSubject:=sSubjectContent,
                                                                     bSaveEMLFile:=True,
                                                                     v_sMessagePath:=vEMailDocPath(0),
                                                                     v_sAttachment:=v_Attachment.ToArray,
                                                                     sEMLFile:=sEmailFilePath)

                                    'Archive the eml file 
                                    m_lReturn = ArchiveEmail(partyCnt:=v_lPartyCnt,
                                                                     docTemplateID:=iDocTemplateID,
                                                                     sType:="", mergedFilePath:=sEmailFilePath, insuranceFileCnt:=v_lInsuranceFileCnt,
                                                                     documentTypeId:=iDocTypeId, v_iMode:=ACSpoolDocMode,
                                                                     isOffice:=gPMFunctions.ToSafeInteger(CStr(vDocumentArray(4, lCount)), 0),
                                                                     productionOrder:=gPMFunctions.ToSafeInteger(CStr(vDocumentArray(5, lCount)), 0),
                                                                     calledFromSAM:=v_bCalledFromSAM,
                                                                     insuranceFolderCnt:=v_lInsuranceFolderCnt,
                                                                     insuranceFileRef:=v_sInsuranceFileRef,
                                                                     retainTempFiles:=True,
                                                                     archiveWithNoMerge:=True,
                                                                     isGeneratedEmail:=True)
                                End If

                            End If

                            If bSendEMailToAgent Then
                                If (sCorrespondenceType = "DEFAULT" AndAlso sDefaultPreferredCorrespondenceType = "MEMAIL") OrElse sCorrespondenceType = "EMAIL" Then
                                    If String.IsNullOrEmpty(sEmailAddress) Then
                                        m_lReturn = GetEmailAddress(ToSafeInteger(vDocumentArray(ACLeadAgentCnt, lCount)), "MEMAIL", sEmailAddress)
                                    End If
                                Else
                                    m_lReturn = GetBranchEmailAddress(m_iSourceID, sEmailAddress)
                                End If

                                If Not String.IsNullOrEmpty(sEmailAddress) Then
                                    m_lReturn = m_oBusiness.SendEMail(v_sTo:=sEmailAddress,
                                                                      v_sSubject:=sSubjectContent,
                                                                      bSaveEMLFile:=True,
                                                                      v_sMessagePath:=vEMailDocPath(0),
                                                                      v_sAttachment:=v_Attachment.ToArray,
                                                                      sEMLFile:=sEmailFilePath)

                                    'Archive the eml file 
                                    m_lReturn = ArchiveEmail(partyCnt:=v_lPartyCnt,
                                                                     docTemplateID:=iDocTemplateID,
                                                                     sType:="", mergedFilePath:=sEmailFilePath, insuranceFileCnt:=v_lInsuranceFileCnt,
                                                                     documentTypeId:=iDocTypeId, v_iMode:=ACSpoolDocMode,
                                                                     isOffice:=gPMFunctions.ToSafeInteger(CStr(vDocumentArray(4, lCount)), 0),
                                                                     productionOrder:=gPMFunctions.ToSafeInteger(CStr(vDocumentArray(5, lCount)), 0),
                                                                     calledFromSAM:=v_bCalledFromSAM,
                                                                     insuranceFolderCnt:=v_lInsuranceFolderCnt,
                                                                     insuranceFileRef:=v_sInsuranceFileRef,
                                                                     retainTempFiles:=True,
                                                                     archiveWithNoMerge:=True,
                                                                     isGeneratedEmail:=True)
                                End If

                            End If

                        End If

                    End If
                    If sCorrespondenceType.ToUpper = "LETTER" Or sDefaultPreferredCorrespondenceType.ToUpper = "LETTER" Then
                        Dim iUbound As Integer = 0
                        Dim bAttachmentFound As Boolean = False

                        Dim sAttachmentSTR() As String

                        If ToSafeInteger(vDocumentArray(1, lCount)) = ACEmailDocType Then
                            sAttachmentSTR = ToSafeString(vDocumentArray(ACEMailAttachementTemplateCodes, lCount)).Split(",")
                            iUbound = sAttachmentSTR.GetUpperBound(0)
                            If ToSafeString(vDocumentArray(ACEMailAttachementTemplateCodes, lCount)).Trim <> "" Then
                                bAttachmentFound = True
                            End If
                        End If

                        For iTmpCnt As Integer = 0 To iUbound
                            bDocSpooled = False
                            Dim iDocTemplateID As Integer = 0
                            Dim iDocTypeId As Integer = 0

                            If bAttachmentFound Then
                                Dim sDocCode As String = sAttachmentSTR(iTmpCnt).Trim


                                m_lReturn = GetDocTemplateTypeID(v_sDocCode:=sDocCode,
                                                             r_lDocTemplateTypeId:=iDocTemplateID,
                                                             r_lDocTypeID:=iDocTypeId)
                            Else
                                iDocTemplateID = ToSafeInteger(vDocumentArray(0, lCount))
                                iDocTypeId = ToSafeInteger(vDocumentArray(1, lCount))
                            End If
                            'Client Copy
                            If Not bIsAgentCorrespondence Then
                                If gPMFunctions.ToSafeInteger(CStr(vDocumentArray(2, lCount)), 0) = 1 Then

                                    sDocDesc = gPMFunctions.ToSafeString(CStr(vDocumentArray(7, lCount)))

                                    m_lReturn = PrintDocument(v_lPartyCnt:=v_lPartyCnt, v_lDocTemplateID:=iDocTemplateID,
                                                              v_sType:="", r_sMergedFilePath:=sMergeFilePath,
                                                              v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_sDocDesc:=sDocDesc,
                                                              v_lDocumentTypeId:=iDocTypeId,
                                                              v_iIsClient:=gPMFunctions.ToSafeInteger(CStr(vDocumentArray(2, lCount)), 0),
                                                              v_iProductionOrder:=gPMFunctions.ToSafeInteger(CStr(vDocumentArray(5, lCount)), 0),
                                                              v_iMode:=v_iMode, v_bCalledFromSAM:=v_bCalledFromSAM,
                                                              v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt,
                                                              v_sInsuranceFileRef:=v_sInsuranceFileRef)
                                    bDocSpooled = True

                                End If
                            End If
                            'If there is not Lead Agent means its a direct business
                            'So agent copy should not be spooled
                            If bIsAgentCorrespondence Then
                                If gPMFunctions.ToSafeInteger(CStr(vDocumentArray(3, lCount)), 0) = 1 And gPMFunctions.ToSafeLong(CStr(vDocumentArray(6, lCount)), 0) > 0 Then

                                    sDocDesc = gPMFunctions.ToSafeString(CStr(vDocumentArray(7, lCount)))

                                    m_lReturn = PrintDocument(v_lPartyCnt:=v_lPartyCnt,
                                                              v_lDocTemplateID:=iDocTemplateID,
                                                              v_sType:="", r_sMergedFilePath:=sMergeFilePath, v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                                                              r_sDocDesc:=sDocDesc, v_lDocumentTypeId:=iDocTypeId,
                                                              v_iIsAgent:=gPMFunctions.ToSafeInteger(CStr(vDocumentArray(3, lCount))),
                                                              v_iProductionOrder:=gPMFunctions.ToSafeInteger(CStr(vDocumentArray(5, lCount)), 0),
                                                              v_iMode:=v_iMode, v_bCalledFromSAM:=v_bCalledFromSAM,
                                                              v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt,
                                                              v_sInsuranceFileRef:=v_sInsuranceFileRef)

                                    bDocSpooled = True
                                End If
                            End If

                            'Office Copy
                            If Not bIsAgentCorrespondence Then
                                If gPMFunctions.ToSafeInteger(CStr(vDocumentArray(4, lCount)), 0) = 1 Then

                                    sDocDesc = gPMFunctions.ToSafeString(CStr(vDocumentArray(7, lCount)))

                                    m_lReturn = PrintDocument(v_lPartyCnt:=v_lPartyCnt,
                                                              v_lDocTemplateID:=iDocTemplateID,
                                                              v_sType:="", r_sMergedFilePath:=sMergeFilePath, v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                                                              r_sDocDesc:=sDocDesc,
                                                              v_lDocumentTypeId:=iDocTypeId,
                                                              v_iIsOffice:=gPMFunctions.ToSafeInteger(CStr(vDocumentArray(4, lCount)), 0),
                                                              v_iProductionOrder:=gPMFunctions.ToSafeInteger(CStr(vDocumentArray(5, lCount)), 0),
                                                              v_iMode:=v_iMode, v_bCalledFromSAM:=v_bCalledFromSAM, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt,
                                                              v_sInsuranceFileRef:=v_sInsuranceFileRef)
                                    bDocSpooled = True
                                End If
                            End If

                            If Not bDocSpooled Then


                                v_sDocTemplateCode = gPMFunctions.ToSafeString(CStr(vDocumentArray.GetValue(12, lCount)))
                                sDocDesc = gPMFunctions.ToSafeString(CStr(vDocumentArray(7, lCount)))

                                m_lReturn = PrintDocument(v_lPartyCnt:=v_lPartyCnt,
                                                          v_lDocTemplateID:=iDocTemplateID,
                                                          v_sType:="", r_sMergedFilePath:=sMergeFilePath, v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                                                          r_sDocDesc:=sDocDesc, v_lDocumentTypeId:=iDocTypeId,
                                                          v_iProductionOrder:=gPMFunctions.ToSafeInteger(CStr(vDocumentArray(5, lCount)), 0),
                                                          v_iMode:=v_iMode, v_bCalledFromSAM:=v_bCalledFromSAM,
                                                          v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt,
                                                          v_sInsuranceFileRef:=v_sInsuranceFileRef, v_sDocTemplateCode:=v_sDocTemplateCode)

                            End If
                        Next
                    End If

                Next lCount
            End If

            Return result

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GenerateDocument", r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

            Return result
        End Try
    End Function

    Private Function GetEmailAddress(ByRef v_Partycnt As Integer, ByVal v_ContactType As String, ByRef v_EmailAddress As String) As Integer
        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(v_Partycnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="contact_type", vValue:=v_ContactType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelEmailContactSQL, sSQLName:=ACSelEmailContactName, bStoredProcedure:=ACSelEmailContactStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray, bKeepNulls:=False)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Dim nlength As Integer = vResultArray.GetUpperBound(1)
            For nTemp As Integer = 0 To nlength
                If (nTemp = nlength) Then
                    v_EmailAddress += ToSafeString(vResultArray(2, nTemp))
                Else
                    v_EmailAddress += ToSafeString(vResultArray(2, nTemp)) + ";"
                End If
            Next

            Return result


        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocTemplateTypeID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocTemplateTypeID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Get the branch Email Address
    ''' </summary>
    ''' <param name="v_Source_Id"></param>
    ''' <param name="v_EmailAddress"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetBranchEmailAddress(ByRef v_Source_Id As Integer, ByRef v_EmailAddress As String) As Integer
        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(v_Source_Id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelBranchEmailContactSQL, sSQLName:=ACSelBranchEmailContactName, bStoredProcedure:=ACSelBranchEmailContactStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray, bKeepNulls:=False)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                v_EmailAddress = String.Empty
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            v_EmailAddress = ToSafeString(vResultArray(0, 0))

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocTemplateTypeID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocTemplateTypeID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetTheTemplate
    '
    ' Description:
    '
    ' History: 26/09/2000 RWH - Created.
    '
    ' ***************************************************************** '
    'developer guide no. 33
    Private Function GetTheTemplate(ByRef r_lDocumentTemplateId As Integer, ByRef r_lDocumentTypeId As Integer, ByVal v_lProcessType As Integer, ByVal v_sTransactionType As String, ByVal v_lInsuranceFileCnt As Integer, Optional ByRef r_vDocumentArray(,) As Object = Nothing, Optional ByRef r_lUserChoice As Long = 0) As Integer

        Dim result As Integer = 0


        Dim obPMBDocLink As bPMBDocLink.Business
        Dim iFunctionalArea As Integer

        'Initialize object
        result = gPMConstants.PMEReturnCode.PMTrue

        obPMBDocLink = New bPMBDocLink.Business
        m_lReturn = obPMBDocLink.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Log Error Message
            Throw New Exception()
        End If

        'Get Values from
        'For time being funtional area is set to 1 i.e. document linking for policy
        iFunctionalArea = 1

        m_lReturn = obPMBDocLink.GetSFIDocumentTemplatesForProcessType(v_iFunctionalArea:=iFunctionalArea, v_lInsurance_File_Cnt:=v_lInsuranceFileCnt, v_lProcessType_Docs_ID:=v_lProcessType, v_lProcess_Type_Code:=v_sTransactionType, v_dtEffectiveDate:=DateTime.Now.ToString("dd MMMM yyyy"), r_vResultarray:=r_vDocumentArray)


        '    'Returning the values
        If Informations.IsArray(r_vDocumentArray) Then

            r_lDocumentTemplateId = CInt(r_vDocumentArray(0, 0))

            r_lDocumentTypeId = CInt(r_vDocumentArray(1, 0))

            r_lUserChoice = r_vDocumentArray(8, 0)
        End If

        '
        'Terminate the object
        If Not (obPMBDocLink Is Nothing) Then
            ' Terminate the business object

            obPMBDocLink.Dispose()
            ' Destroy the instance of the business object
            ' from memory.
            obPMBDocLink = Nothing

        End If


        Return result

    End Function

    'Start-(Arul Stephen)-(PN Fixing-PN 59278)
    ' ***************************************************************** '
    ' Name: GetRenewalPaymentMethod
    ' Description: Get the Payment Method
    ' ***************************************************************** '
    Public Function GetRenewalPaymentMethod(ByVal v_lInsuranceFileCnt As Integer, ByRef r_sPaymentMethod As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetRenewalPaymentMethod"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameter
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACClass, kMethodName & " Fails to Add parameter Insurance_File_Cnt", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="Payment_Method", vValue:="", iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACClass, kMethodName & " Fails to Add parameter Payment_Method", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRenewalPaymentMethodSQL, sSQLName:=ACGetRenewalPaymentMethodName, bStoredProcedure:=ACGetRenewalPaymentMethodStored)


            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACClass, kMethodName & " Fails to Get Payment Method Details", gPMConstants.PMELogLevel.PMLogError)
            End If

            r_sPaymentMethod = m_oDatabase.Parameters.Item("Payment_Method").Value


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally
            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    'End-(Arul Stephen)-(PN Fixing-PN 59278)

    'Changes done by Krishna Nand PN: 70509 Dated: 31/03/2010
    Public Function UpdateCurrencyToInsuranceFile() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateCurrencyToInsuranceFile"

        Dim sFailMsg As String = ""

        Dim oCurrencyConvert As bACTCurrencyConvert.Form
        Dim lAccountID, lSourceID As Integer
        Dim iTransactionCurrencyID As Integer
        Dim cTransactionAmount As Decimal
        Dim iBaseCurrencyID As Integer
        Dim cBaseCurrentAmount As Decimal
        Dim iAccountCurrencyID As Integer
        Dim cAccountCurrentAmount As Decimal
        Dim iSystemCurrencyID As Integer
        Dim cSystemCurrentAmount As Decimal
        Dim dTransToBaseExchangeRate As Double
        Dim dtEffectiveDateOfExchange As Date
        Dim dAccountToBaseExchangeRate, dSystemToBaseExchangeRate As Double
        Dim iRateOverrideReasonID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oCurrencyConvert = New bACTCurrencyConvert.Form
            If oCurrencyConvert.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database)) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            iTransactionCurrencyID = m_oInsuranceFile.CurrencyID

            lSourceID = m_oInsuranceFile.SourceID

            If Convert.IsDBNull(dtEffectiveDateOfExchange) Or Informations.IsNothing(dtEffectiveDateOfExchange) Or dtEffectiveDateOfExchange = CDate("00:00:00") Then
                dtEffectiveDateOfExchange = DateTime.Today
            End If


            m_lReturn = oCurrencyConvert.DoCurrencyConversion(v_lAccountID:=lAccountID, v_lCompanyId:=lSourceID, v_iCurrencyID:=iTransactionCurrencyID, v_cCurrencyAmountUnrounded:=cTransactionAmount, r_iBaseCurrencyID:=iBaseCurrencyID, r_cBaseAmount:=cBaseCurrentAmount, r_iAccountCurrencyID:=iAccountCurrencyID, r_cAccountAmount:=cAccountCurrentAmount, r_iSystemCurrencyID:=iSystemCurrencyID, r_cSystemAmount:=cSystemCurrentAmount, r_dCurrencyBaseXrate:=dTransToBaseExchangeRate, r_dtCurrencyBaseDate:=dtEffectiveDateOfExchange, r_dAccountBaseXrate:=dAccountToBaseExchangeRate, r_dtAccountBaseDate:=dtEffectiveDateOfExchange, r_dSystemBaseXrate:=dSystemToBaseExchangeRate, r_dtSystemBaseDate:=dtEffectiveDateOfExchange)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                oCurrencyConvert.Dispose()
                oCurrencyConvert = Nothing
                Throw New Exception
            End If

            'A zero here means that the exchange rates have not been set up
            If dTransToBaseExchangeRate = 0 Or dSystemToBaseExchangeRate = 0 Then
                result = gPMConstants.PMEReturnCode.PMFalse

                oCurrencyConvert.Dispose()
                oCurrencyConvert = Nothing
                Throw New Exception
            End If



            m_lReturn = oCurrencyConvert.UpdateInsuranceFile(v_lInsuranceFileCnt:=m_oInsuranceFile.InsuranceFileCnt, v_dCurrencyBaseXrate:=dTransToBaseExchangeRate, v_dtCurrencyBaseDate:=dtEffectiveDateOfExchange, v_dAccountBaseXrate:=0, v_dtAccountBaseDate:=#12/30/1899#, v_dSystemBaseXrate:=dSystemToBaseExchangeRate, v_dtSystemBaseDate:=dtEffectiveDateOfExchange, v_lRateOverrideReasonID:=iRateOverrideReasonID, v_iBaseCurrencyID:=iBaseCurrencyID, v_iAccountCurrencyID:=0)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to update Insurance File Data"

                oCurrencyConvert.Dispose()
                oCurrencyConvert = Nothing
                Throw New Exception(sFailMsg)
            End If

            oCurrencyConvert = Nothing

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            If sFailMsg = "" Then
                sFailMsg = "Failed to Update Currency "
            End If

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFailMsg, vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally

        End Try
        Return result
    End Function
    Public Function ValidateCertificateYear(ByRef bIsValid As Boolean, ByVal lNewInsuranceFileCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim vValue As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTSubAgentCertificateYears, v_vBranch:=1, r_vUnderwriting:=vValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get product option " & gPMConstants.SIRHiddenOptions.SIROPTHoldCoverExpiryDate, vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            End If

            If vValue = "1" Then
                m_lReturn = GetAndValidateSubAgentDetailsViaInsFile(bIsValid, lNewInsuranceFileCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
                If bIsValid = False Then
                    'System.MessageBox.Show("You Cannot Make This Transaction Live- Please check the Certificate Year Configuration of Sub Agent", ACApp, MessageBoxButtons.OK)
                    bPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="You Cannot Make This Transaction Live- Please check the Certificate Year Configuration of Sub Agent" & gPMConstants.SIRHiddenOptions.SIROPTHoldCoverExpiryDate, vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

            End If
            Return result
        Catch ex As Exception
            bPMFunc.LogError(
                 v_sUsername:=m_sUsername,
                 v_sClass:=ACClass,
                 v_sMethod:="ValidateCertificateYear",
                 r_lFunctionReturn:=result, excep:=ex)
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function
    Public Function GetAndValidateSubAgentDetailsViaInsFile(ByRef r_bIsValid As Boolean, ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            If v_lInsuranceFileCnt <> 0 Then

                m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetSubAgentDetailsSQL, sSQLName:=ACGetSubAgentDetailsName, bStoredProcedure:=ACGetSubAgentDetailsStored, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                If vResultArray IsNot Nothing AndAlso Informations.IsArray(vResultArray) Then

                    Dim iUBound As Integer = 0
                    Dim iLBound As Integer = 0
                    Dim sPartyCode As String = String.Empty

                    iLBound = vResultArray.GetLowerBound(1)
                    iUBound = vResultArray.GetUpperBound(1)

                    For iCNT As Integer = iLBound To iUBound
                        sPartyCode = CStr(vResultArray(1, iCNT))
                        m_lReturn = GetAndValidateSubAgentDetails(r_bIsValid:=r_bIsValid, v_sPartyCode:=sPartyCode, v_lInsuranceFileCnt:=v_lInsuranceFileCnt)
                        If r_bIsValid = False Then
                            Exit For
                        End If
                    Next
                Else
                    r_bIsValid = True
                    Return result
                End If
            End If
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAndValidateSubAgentDetailsViaInsFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSubAgents", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Function GetAndValidateSubAgentDetails(ByRef r_bIsValid As Boolean, Optional ByVal v_sPartyCode As String = "", Optional ByVal v_dtCoverStartDate As Date = Nothing, Optional ByVal v_dtCoverEndDate As Date = Nothing, Optional ByVal v_lInsuranceFileCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_code", vValue:=CStr(v_sPartyCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="CoverStartDate", vValue:=CStr(v_dtCoverStartDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="CoverEndDate", vValue:=CStr(v_dtCoverEndDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetandValidateSubAgentDetailSQL, sSQLName:=ACGetandValidateSubAgentDetailName, bStoredProcedure:=ACGetandValidateSubAgentDetailStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If vResultArray IsNot Nothing AndAlso Informations.IsArray(vResultArray) Then
                If CStr(vResultArray(0, 0)).Trim() = "VALID" Then
                    r_bIsValid = True
                Else
                    r_bIsValid = False
                End If
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAndValidateSubAgentDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSubAgents", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Function CancelMTAQuotes(ByVal v_lInsuranceFileCnt As Long,
                                                ByVal v_lInsuranceFolderCnt As Long,
                                                            ByVal v_lPartyCnt As Long) As Long

        Const kMethodName As String = "CancelMTAQuotes"
        Dim obSIRInsuranceFile As bSIRInsuranceFile.Business
        Dim vReturnArray(,) As Object
        Dim iLoop As Long
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            obSIRInsuranceFile = New bSIRInsuranceFile.Business
            If obSIRInsuranceFile.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database)) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to get instance of bSIRInsuranceFile.Business", gPMConstants.PMELogLevel.PMLogError)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetMTAQuotePolicyVersions(v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                                                              v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt,
                                                              r_vResults:=vReturnArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetInsuranceFileDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vReturnArray) Then
                For iLoop = vReturnArray.GetLowerBound(1) To vReturnArray.GetUpperBound(1)
                    v_lInsuranceFileCnt = ToSafeLong(vReturnArray(0, iLoop))
                    v_lInsuranceFolderCnt = ToSafeLong(vReturnArray(1, iLoop))
                    If v_lInsuranceFileCnt > 0 And v_lInsuranceFolderCnt > 0 Then
                        m_lReturn =
                        obSIRInsuranceFile.MTACancellation(v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                                                           v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt,
                                                           v_lPartyCnt:=v_lPartyCnt)
                        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If
                Next
            End If

            Return result

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=CancelMTAQuotes, v_sUsername:="", excep:=ex)

            Return result
        Finally
            If Not obSIRInsuranceFile Is Nothing Then
                obSIRInsuranceFile.Dispose()
                obSIRInsuranceFile = Nothing
            End If

            ' This is for debugging only
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetMTAQuotePolicyVersions
    '
    ' Parameters: InsuranceFileCnt,InsuranceFolderCnt
    '
    ' Description:
    ' ***************************************************************** '
    Private Function GetMTAQuotePolicyVersions(
                            ByVal v_lInsuranceFileCnt As Long,
                            ByVal v_lInsuranceFolderCnt As Long,
                            ByRef r_vResults(,) As Object) As Long

        Const kMethodName As String = "GetMTAQuotePolicyVersions"
        Dim result As Integer = 0
        Dim lReturn As Long



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

        AddInputParameter(v_sName:="insurance_folder_cnt", v_vValue:=v_lInsuranceFolderCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

        ' Execute selection Query
        lReturn = m_oDatabase.SQLSelect(
                                sSQL:=kGetMTAQuotePolicyVersionsSQL,
                                sSQLName:=kGetMTAQuotePolicyVersionsName,
                                bStoredProcedure:=True,
                                vResultArray:=r_vResults,
                                lNumberRecords:=gPMConstants.PMAllRecords)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, kGetMTAQuotePolicyVersionsSQL & " Failed", result = gPMConstants.PMELogLevel.PMLogError)
            result = gPMConstants.PMEReturnCode.PMError
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: RunLapseRule
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function RunLapseRule(ByVal iInsuranceFileCnt As Object, ByVal iInsuranceFolderCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim oRiskData As bSIRRiskData.Business
        Dim iReturn As Integer
        Dim oRiskArray(,) As Object
        Dim oGisPolicyLinkArray(,) As Object

        Try

            Const ACRiskPosCnt As Integer = 0
            Dim iTransactionType, iQuoteType As Integer

            Dim sXMLDataSetDef, sXMLDataSet As Object
            Dim oGIS As Object  'bGIS.Application
            result = gPMConstants.PMEReturnCode.PMTrue

            oRiskData = New bSIRRiskData.Business
            m_lReturn = CType(oRiskData.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oRiskData.GetRisk(v_lInsuranceFileCnt:=iInsuranceFileCnt, r_vResultArray:=oRiskArray) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not (oRiskArray Is Nothing) Then
                For iCount As Integer = 0 To oRiskArray.GetUpperBound(1)

                    m_lReturn = oRiskData.GetGISPolicyLink(v_lInsuranceFolderCnt:=iInsuranceFolderCnt, v_lRiskID:=oRiskArray(ACRiskPosCnt, iCount), r_vResultArray:=oGisPolicyLinkArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    'do we have any data
                    Dim obj As Object = oGisPolicyLinkArray(0, 0)

                    If Not (Convert.IsDBNull(obj) Or Informations.IsNothing(obj)) Then
                        'Make sure GIS object present.
                        m_oDataSet = New cGISDataSetControl.Application()
                        'oGIS = New bGIS.Application

                        oGIS = Nothing
                        result = gPMComponentServices.CreateBusinessObject(r_oObject:=oGIS, v_sClassName:="bGIS.Application", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
                        If result <> gPMConstants.PMEReturnCode.PMTrue Then
                            Dim r_sMessage As String = "Failed to create an instance of bGIS.Application"
                            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="bGIS.Application", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                            Return result
                        End If

                        iReturn = oGIS.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))


                        Dim sDataModelCode As Object = CStr(oGisPolicyLinkArray(4, 0)).Trim()
                        Dim iRiskID As Integer = oRiskArray(0, iCount)
                        Dim iPolicyLinkId As Object = oGisPolicyLinkArray(0, 0)

                        iReturn = oGIS.LoadFromDB(r_sXMLDataSetDef:=sXMLDataSetDef,
                                                  r_sXMLDataSet:=sXMLDataSet,
                                                  v_sGisDataModelCode:=sDataModelCode,
                                                  r_vInsuranceFileCnt:=iInsuranceFileCnt,
                                                  r_vPolicyLinkID:=iPolicyLinkId)
                        If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        ' Load Data as XML
                        iReturn = m_oDataSet.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet)

                        If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        iReturn = GetLapsedRenewalTranType()
                        If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        iTransactionType = m_lTransactionID ' for Renewal Lapse

                        'run renewal script
                        EncodeTransactionScreenAndType(r_lEncoded:=iQuoteType, r_lTransactionType:=iTransactionType, r_lGISScreenId:=0, r_lQuoteType:=PBCQemQuoteTypeRenewalLapse)
                        'PBCQemQuoteTypeRenewalLapse Declared in SharedFiles
                        iReturn = GIS_NBQuote(v_sGisDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                  v_lQuoteType:=iQuoteType,
                                                  r_sXMLDataSet:=sXMLDataSet,
                                                  r_sXMLDataSetDef:=sXMLDataSetDef)
                        If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        iReturn = GIS_SaveToDB(v_sGisDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim())
                        If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If

                Next
            End If
            Return result

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GIS_NBQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GIS_NBQuote", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)

            Return result
        Finally
            oRiskData = Nothing
            oRiskArray = Nothing
            oGisPolicyLinkArray = Nothing
        End Try
    End Function
    Public Function GIS_NBQuote(ByRef v_sGisDataModelCode As String, ByRef v_lQuoteType As Integer, ByRef r_sXMLDataSet As Object, ByRef r_sXMLDataSetDef As String) As Integer


        Dim result As Integer = 0
        Dim iReturn As Integer
        Dim sDataModelCode As String = ""
        Dim oGIS As Object  'bGIS.Application

        Try
            'Make sure GIS object present.
            result = gPMConstants.PMEReturnCode.PMTrue
            'oGIS = New bGIS.Application
            oGIS = Nothing
            result = gPMComponentServices.CreateBusinessObject(r_oObject:=oGIS, v_sClassName:="bGIS.Application", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Dim r_sMessage As String = "Failed to create an instance of bGIS.Application"
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="bGIS.Application", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                Return result
            End If

            iReturn = oGIS.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))

            If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' RFC300300 - Clear all Quote Output that may already exist
            ' as there is no need to Pass it back across the network.
            iReturn = m_oDataSet.ClearAllQuoteOutput()
            If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Data as an XML String
            iReturn = m_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataSet:=r_sXMLDataSet)
            If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oGIS.NBQuote(v_sGisDataModelCode:=ToSafeString(v_sGisDataModelCode), v_lQuoteType:=ToSafeInteger(v_lQuoteType),
                                     v_sGisBusinessTypeCode:="NB", v_dtEffectiveDate:=DateTime.Today,
                                     r_sXMLDataset:=r_sXMLDataSet)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Load the NBQuote Results
            iReturn = m_oDataSet.LoadFromXML(v_sXMLDataSetDef:=r_sXMLDataSetDef, v_sXMLDataSet:=r_sXMLDataSet)
            If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'destroy gis object
            If Not (oGIS Is Nothing) Then

                oGIS.Dispose()
                oGIS = Nothing
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GIS_NBQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GIS_NBQuote", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        Finally
            oGIS = Nothing
        End Try
    End Function


    Public Function GIS_SaveToDB(ByVal v_sGisDataModelCode As String) As Integer

        Dim result As Integer = 0
        Dim sXMLDataSetDef, sXMLDataSet As Object
        Dim iReturn As Integer
        Dim oGIS As Object  'bGIS.Application

        Try
            'Make sure GIS object present.
            result = gPMConstants.PMEReturnCode.PMTrue

            'oGIS = New bGIS.Application
            oGIS = Nothing
            result = gPMComponentServices.CreateBusinessObject(r_oObject:=oGIS, v_sClassName:="bGIS.Application", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Dim r_sMessage As String = "Failed to create an instance of bGIS.Application"
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="bGIS.Application", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                Return result
            End If

            iReturn = oGIS.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))

            If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Data as an XML String
            iReturn = m_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataSet:=sXMLDataSet)
            If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Save it to the DataBase

            iReturn = oGIS.SaveToDB(v_sGisDataModelCode:=ToSafeString(v_sGisDataModelCode), r_sXMLDataSet:=sXMLDataSet)
            If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Load the Saved to DB Results
            iReturn = m_oDataSet.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet)
            If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'destroy gis object
            If Not (oGIS Is Nothing) Then

                oGIS.Dispose()
                oGIS = Nothing
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GIS_SaveToDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GIS_SaveToDB", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        Finally
            oGIS = Nothing
        End Try
    End Function

    ' Name: 
    '
    ' Description: Get TransactionType for lapsed renewal
    '*****************************************************************
    Public Function GetLapsedRenewalTranType() As Integer
        Dim iresult As Integer = 0
        Dim sSQL As String = ""
        Dim oResultArray(,) As Object
        Dim sTransTypeDescription As String = ""

        Try
            iresult = gPMConstants.PMEReturnCode.PMTrue
            sSQL = "SELECT transaction_type_id, description FROM Transaction_Type WHERE LTRIM(RTRIM(code)) = 'RENLAP' "

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetTransactionTypeDetails", bStoredProcedure:=False, vResultArray:=oResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(oResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            m_lTransactionID = gPMFunctions.ToSafeInteger(oResultArray(0, 0))

            sTransTypeDescription = gPMFunctions.ToSafeString(oResultArray(1, 0))

        Catch excep As System.Exception
            iresult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLapsedRenewalTranType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLapsedRenewalTranType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return iresult
        End Try
        Return iresult
    End Function

    Private Function AddChaseCycleItem(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sBusinessType As String) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()

            result = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = .Parameters.Add(sName:="business_type", vValue:=v_sBusinessType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = .SQLAction(sSQL:=ACAddChaseCycleItemInsuranceFileSQL, sSQLName:=ACAddChaseCycleItemInsuranceFileName, bStoredProcedure:=ACAddChaseCycleItemInsuranceFileStored)

        End With

        Return result

    End Function
    Public Function GetSystemOptionLite(ByVal v_iOptionNumber As Integer, ByRef r_sOptionValue As String, ByVal v_iSourceID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetSystemOptionLite"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CType(bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=v_iOptionNumber, r_sOptionValue:=r_sOptionValue, v_iSourceID:=v_iSourceID), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSystemOption Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function GetPrePaymentOptionValue(ByVal v_lproductid As Integer, ByRef r_Prepayment(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPrePaymentOptionValue"

        Dim lReturn As gPMConstants.PMEReturnCode


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            lReturn = CType(AddInputParameter(v_sName:="product_id", v_vValue:=v_lproductid, v_iType:=gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetPrepaymentOPtionValSQL, sSQLName:=kGetPrepaymentOPtionVal, bStoredProcedure:=True, vResultArray:=r_Prepayment, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PrePaymentOptionValue Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function
    ''' <summary>
    ''' GetRenewalPolicyDetails
    ''' </summary>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <param name="dtResult"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRenewalPolicyDetails(ByVal v_lInsuranceFileCnt As Long, Optional ByRef dtResult As DataTable = Nothing) As Long

        Const kMethodName As String = "GetRenewalPolicyDetails"

        Dim nResult As Integer

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue
            ' Add parameter
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(ACClass, kMethodName & " Fails to Add parameter Insurance_File_Cnt", gPMConstants.PMELogLevel.PMLogError)
            End If
            m_lReturn = m_oDatabase.ExecuteDataTable(sSQL:=ACGetRenewalPolicyDetailsSQL, bStoredProcedure:=ACGetRenewalPolicyDetailsStored, sSQLName:=ACGetRenewalPolicyDetailsName, oRecordset:=dtResult)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(ACClass, kMethodName & " Fails to Get Renewal Policy Details", gPMConstants.PMELogLevel.PMLogError)
            End If

            If dtResult IsNot Nothing AndAlso dtResult.Rows.Count > 0 Then
                m_bIsMigratedPolicy = True
            End If
        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GetRenewalPolicyDetails, excep:=ex)
            nResult = gPMConstants.PMEReturnCode.PMError

        Finally

        End Try
        Return nResult
    End Function

    Public Function GetRenewalDetails(ByVal v_lInsuranceFileCnt As Long,
                                            ByRef r_dtResult As DataTable) As Long

        Const kMethodName As String = "GetRenewalDetails"

        Dim nResult As Integer
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameter
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="nInsurance_file_cnt",
                                          vValue:=v_lInsuranceFileCnt,
                                          iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                          iDataType:=gPMConstants.PMEDataType.PMLong)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(ACClass, kMethodName & " Fails to Add parameter Insurance_File_Cnt", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.ExecuteDataTable(sSQL:=ACGetRenewalDetailsSQL,
                                                     sSQLName:=ACGetRenewalDetailsName,
                                                     bStoredProcedure:=ACGetRenewalDetailsStored,
                                                     oRecordset:=r_dtResult)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(ACClass, kMethodName & " Fails to Get Renewal Details", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GetRenewalDetails, excep:=ex)
            ' If you want to rollback a transaction or something, do it here
            nResult = gPMConstants.PMEReturnCode.PMError
        Finally

        End Try
        Return nResult
    End Function


    Private Function ProcessUpdatePolicy(v_lInsuranceFileCnt As Long) As Long
        Dim lReturn As Long
        Dim vRisks As Object
        Dim m_oChangePolicyStatus As bSIRChangePolicyStatus.Business
        Const kMethodName As String = "ProcessUpdatePolicy"
        Dim nResult As Integer


        nResult = gPMConstants.PMEReturnCode.PMTrue
        ProcessUpdatePolicy = gPMConstants.PMEReturnCode.PMTrue
        m_oChangePolicyStatus = New bSIRChangePolicyStatus.Business

        If m_oChangePolicyStatus.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database)) <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "Failed to get instance of bSIRChangePolicyStatus.Business", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Get risks associated with this insurance file
        lReturn = m_oChangePolicyStatus.GetRisksByStatus(
                  v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                  r_vRisks:=vRisks)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "GetRisksByStatus Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = gPMConstants.PMEReturnCode.PMFalse
        End If

        If Informations.IsArray(vRisks) Then
            lReturn = m_oChangePolicyStatus.DeleteRisks(v_vrisks:=vRisks)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "DeleteRisks Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Re-jig the risk and variation numbers of the remaining
            '            risks on this policy
            lReturn = m_oChangePolicyStatus.RenumberRisks(
                      v_lInsuranceFileCnt:=v_lInsuranceFileCnt)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "RenumberRisks Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        End If

        Return nResult
    End Function

    ''' <summary>
    ''' Get Latest Version For Renewal
    ''' </summary>
    ''' <param name="nInsuranceFolderCnt"></param>
    ''' <param name="oResultArray"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function GetLatestVersionForRenewal(ByVal nInsuranceFolderCnt As Integer,
                                            ByRef oResultArray(,) As Object) As Integer


        Dim nResult As Integer
        Const kMethodName As String = "GetLatestVersionForRenewal"
        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameter
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="nInsurance_folder_cnt",
                                          vValue:=nInsuranceFolderCnt,
                                          iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                          iDataType:=gPMConstants.PMEDataType.PMLong)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_get_latest_policy_version_for_Renewal",
                                                     sSQLName:="spu_get_latest_policy_version_for_Renewal",
                                                     bStoredProcedure:=True,
                                                     vResultArray:=oResultArray,
                                                     lNumberRecords:=gPMConstants.PMAllRecords)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If
            Return m_lReturn
        Catch ex As Exception


            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(
                 v_sUsername:=m_sUsername,
                 v_sClass:=ACClass,
                 v_sMethod:=kMethodName,
                 r_lFunctionReturn:=GetLatestVersionForRenewal, excep:=ex)
            nResult = gPMConstants.PMEReturnCode.PMError
            ' If you want to rollback a transaction or something, do it here
        End Try
        Return nResult
    End Function

    ''' <summary>
    ''' Validate Accept Duplicate Version
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="bIsValid"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Private Function ValidateAcceptDuplicateVersion(
                            ByVal nInsuranceFileCnt As Integer,
                            ByRef bIsValid As Boolean) As Integer

        Dim oResults(,) As Object
        Dim nResult As Integer

        nResult = gPMConstants.PMEReturnCode.PMTrue
        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters

        ' insurance file cnt
        m_lReturn = AddInputParameter(v_sName:="nInsurance_file_cnt", v_vValue:=nInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)


        ' Execute selection Query
        m_lReturn = m_oDatabase.SQLSelect(
                                sSQL:="spu_SIR_Select_Duplicate_Renewal",
                                sSQLName:="spu_SIR_Select_Duplicate_Renewal",
                                bStoredProcedure:=True,
                                vResultArray:=oResults,
                                lNumberRecords:=gPMConstants.PMAllRecords)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
            nResult = gPMConstants.PMEReturnCode.PMFalse
            Return nResult
        End If

        If Informations.IsArray(oResults) Then
            bIsValid = False
        Else
            bIsValid = True
        End If

        Return nResult
    End Function

    ''' <summary>
    ''' Validate Premium
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="bIsValid"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Private Function ValidatePremium(
                            ByVal nInsuranceFileCnt As Integer,
                            ByRef bIsValid As Boolean) As Integer

        Dim oResults(,) As Object
        Dim nResult As Integer

        nResult = gPMConstants.PMEReturnCode.PMTrue
        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters

        ' insurance file cnt
        m_lReturn = AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=nInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)


        ' Execute selection Query
        m_lReturn = m_oDatabase.SQLSelect(
                                sSQL:="spu_Get_GrossTotal",
                                sSQLName:="spu_Get_GrossTotal",
                                bStoredProcedure:=True,
                                vResultArray:=oResults,
                                lNumberRecords:=gPMConstants.PMAllRecords)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
            nResult = gPMConstants.PMEReturnCode.PMFalse
            Return nResult
        End If

        If Informations.IsArray(oResults) Then
            If (gPMFunctions.ToSafeDecimal(oResults(1, 0), 0) < 0) Then
                bIsValid = False
            Else
                bIsValid = True
            End If
        Else
            bIsValid = False
        End If

        Return nResult
    End Function

    ''' <summary>
    ''' ValidateAcceptTMPAnniversaryIsValidAction
    ''' </summary>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <param name="v_dRenewalDate"></param>
    ''' <param name="r_vResults"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ValidateAcceptTMPAnniversaryIsValidAction(
                        ByVal v_lInsuranceFileCnt As Integer,
                        ByVal v_dRenewalDate As Date,
                        ByRef r_vResults(,) As Object) As Integer

        Const kMethodName As String = "ValidateAcceptTMPAnniversaryIsValidAction"



        ValidateAcceptTMPAnniversaryIsValidAction = gPMConstants.PMEReturnCode.PMTrue
        Try


            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters

            ' insurance file cnt
            m_lReturn = AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' insurance ref
            m_lReturn = AddInputParameter(v_sName:="renewal_date", v_vValue:=v_dRenewalDate, v_iType:=gPMConstants.PMEDataType.PMDate)

            ' Execute selection Query
            If m_oDatabase.SQLSelect(
                                    sSQL:="spu_SIR_Get_Is_Accept_TMP_Anniversary_Valid_Action",
                                    sSQLName:="spu_SIR_Get_Is_Accept_TMP_Anniversary_Valid_Action",
                                    bStoredProcedure:=True,
                                    vResultArray:=r_vResults,
                                    lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                Throw New ApplicationException("spu_SIR_Get_Is_Accept_TMP_Anniversary_Valid_Action" & " Failed")


            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(
                 v_sUsername:=m_sUsername,
                 v_sClass:=ACClass,
                 v_sMethod:=kMethodName,
                 r_lFunctionReturn:=ValidateAcceptTMPAnniversaryIsValidAction, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        End Try
    End Function

    ''' <summary>
    ''' Get Premium Finance attached to the insurance file
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="aoResults"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPFPlanForInsFile(ByVal nInsuranceFileCnt As Integer, ByRef aoResults(,) As Object) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "GetPFPlanForInsFile"
        Try
            m_oDatabase.Parameters.Clear()
            m_oDatabase.Parameters.Add("financeplancnt", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            m_oDatabase.Parameters.Add("financeplanversion", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            m_oDatabase.Parameters.Add("insurancefilecnt", nInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            nResult = m_oDatabase.SQLSelect(sSQL:=KSelectSinglePFForInsSQL, sSQLName:=KSelectSinglePFForInsName, bStoredProcedure:=KSelectSinglePFForInsStored, vResultArray:=aoResults, lNumberRecords:=gPMConstants.PMAllRecords)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, KSelectSinglePFForInsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
            nResult = PMEReturnCode.PMFalse
        End Try
        Return nResult
    End Function
    Private Function GetCreditCardDetails(ByVal v_nOldInsuranceFileCnt As Integer, ByVal v_nNewInsuranceFileCnt As Integer, ByRef r_sTransactionId As String,
                                          ByRef r_sIntegerationToken As String, ByRef r_sTokenId As String, ByRef r_sPaymentMethod As String,
                                          ByRef r_sMediaType As String, ByRef r_dPremiumAmount As Decimal, ByRef r_sCurrencyCode As String) As Integer

        Dim nResult As Integer = 0
        Dim oResult(,) As Object

        Try

            nResult = PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            nResult = m_oDatabase.Parameters.Add(sName:="OldInsuranceFileCnt", vValue:=CStr(v_nOldInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If nResult <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If
            nResult = m_oDatabase.Parameters.Add(sName:="NewInsuranceFileCnt", vValue:=CStr(v_nNewInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If nResult <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            nResult = m_oDatabase.SQLSelect(sSQL:=kSelCreditCardDetailsSQL, sSQLName:=kSelCreditCardDetailsName, bStoredProcedure:=kSelCreditCardDetailsStored, vResultArray:=oResult)
            If nResult <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(oResult) Then
                Return PMEReturnCode.PMNotFound
            End If

            r_sPaymentMethod = ToSafeString(oResult(kIndexPaymentMethod, 0), "")
            r_sMediaType = ToSafeString(oResult(kIndexMediaType, 0), "")
            r_sTransactionId = ToSafeString(oResult(kIndexTransactionId, 0), "")
            r_sIntegerationToken = ToSafeString(oResult(kIndexIntegerationToken, 0), "")
            r_sTokenId = ToSafeString(oResult(kIndexTokenId, 0), "")
            r_dPremiumAmount = ToSafeDecimal(oResult(kIndexPremiumAmount, 0), 0)
            r_sCurrencyCode = ToSafeString(oResult(kIndexCurrencyCode, 0), "")
            Return nResult

        Catch excep As System.Exception

            nResult = PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="GetCreditCardDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCreditCardDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function
    'End of Changes
    'PN4538 - Start
    Private Function CheckAndUpdateCommonRenewalDate(ByVal v_lInsuranceFileCnt As Long) As Long

        Dim result As Integer = 0
        Const kMethodName As String = "CheckAndUpdateCommonRenewalDate"

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateCommonRenewalDateSQL,
                                        sSQLName:=ACUpdateCommonRenewalDateName,
                                        bStoredProcedure:=ACUpdateCommonRenewalDateStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch ex As Exception
            bPMFunc.LogError(
                 v_sUsername:=m_sUsername,
                 v_sClass:=ACClass,
                 v_sMethod:=kMethodName,
                 r_lFunctionReturn:=CheckAndUpdateCommonRenewalDate)
            result = gPMConstants.PMEReturnCode.PMError
        Finally

        End Try
        Return result

    End Function
    Public Function CheckJobBatchRenewalInProcess(ByVal v_sKey As String,
                                               ByRef r_bIsJobBatchRenewalInProcess As Boolean) As Integer

        Const kMethodName As String = "CheckJobBatchRenewalInProcess"
        Dim vResults As Object

        Try

            CheckJobBatchRenewalInProcess = gPMConstants.PMEReturnCode.PMTrue

            r_bIsJobBatchRenewalInProcess = False

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            Call AddInputParameter(v_sName:="ProcessKey", v_vValue:=v_sKey, v_iType:=gPMConstants.PMEDataType.PMString)

            ' Execute selection Query
            m_lReturn = m_oDatabase.SQLSelect(
                                    sSQL:=ACCheckJobBatchRenewalInProcessSQL,
                                    sSQLName:=ACCheckJobBatchRenewalInProcessName,
                                    bStoredProcedure:=ACCheckJobBatchRenewalInProcessStored,
                                    vResultArray:=vResults)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(vResults) Then
                r_bIsJobBatchRenewalInProcess = True
            End If

        Catch

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(
                 v_sUsername:=m_sUsername,
                 v_sClass:=ACClass,
                 v_sMethod:=kMethodName,
                 r_lFunctionReturn:=CheckJobBatchRenewalInProcess)

            ' If you want to rollback a transaction or something, do it here

        End Try
    End Function
    Private Function ProcessRenewalAcceptanceEmail(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lBatchRenewalJobID As Integer, ByVal bCalledFromSAM As Boolean, ByVal v_sInsuranceFileRef As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessRenewalAcceptanceEmail"
        Const kRenewalAcceptance As Integer = 14

        Dim iRenewalDocDestination, iReportSortOrder As Integer


        result = gPMConstants.PMEReturnCode.PMTrue

        result = CType(GetBatchJobPrintingOptions(v_lBatchRenewalJobID:=v_lBatchRenewalJobID, r_iRenewalDocDestination:=iRenewalDocDestination, r_iReportSortOrder:=iReportSortOrder), gPMConstants.PMEReturnCode)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        'If no document printing require than exit from docs printing process
        If iRenewalDocDestination = 0 Then
            Return result
        End If

        If iRenewalDocDestination = gPMConstants.PMRenewalDocDestination_Print Then
            iRenewalDocDestination = gSIRLibrary.ACPrintSilentMode
        Else
            iRenewalDocDestination = gSIRLibrary.ACSpoolDocMode
        End If

        result = GenerateDocument(v_iDocType:=kRenewalAcceptance, v_iMode:=iRenewalDocDestination, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_sSpoolDesc:="Renewal Acceptance - Renewal Documents", v_sTransactionType:="RN", v_bCalledFromSAM:=bCalledFromSAM, v_sInsuranceFileRef:=v_sInsuranceFileRef)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(ACClass, kMethodName & " Fails to Send Renewal Acceptance Email", gPMConstants.PMELogLevel.PMLogError)
        End If
        Return result
    End Function

    Public Function ConvertDocumentUsingSiriusDocumentUtility(ByVal v_sSourceDocument As String, ByVal v_sDestDocument As String) As Integer
        Dim m_bIsCalledFromBatchProcess As Boolean
        Dim result As Integer = 0
        ' Dim SiriusDocumentUtility As Object
        Const kMethodName As String = "ConvertDocumentUsingSiriusDocumentUtility"
        Dim oConvert As SiriusDocumentUtility.Document


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            oConvert = New SiriusDocumentUtility.Document()

            oConvert.Convert(v_sSourceDocument, v_sDestDocument)


        Catch ex As Exception

            If m_bIsCalledFromBatchProcess = True Then
                result = gPMConstants.PMEReturnCode.PMError
                bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:="", excep:=ex)
            Else
                bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:="")
            End If

        Finally
            oConvert = Nothing

            '        Return result

            ' This is for debugging only
            '        
            '        Return result
        End Try
        Return result
    End Function

    ''' <summary>
    ''' LockKey
    ''' </summary>
    ''' <param name="v_sKeyName"></param>
    ''' <param name="v_nKeyValue"></param>
    ''' <param name="v_nUserID"></param>
    ''' <param name="r_sLockedBy"></param>
    ''' <returns></returns>
    ''' <remarks>create a lock for specified key And value</remarks>
    Public Function LockKey(ByVal v_sKeyName As String, ByVal v_nKeyValue As Integer, ByVal v_nUserID As Integer, ByRef r_sLockedBy As String) As Integer
        Dim oLock As bPMLock.User
        Dim nResult As Integer = 0
        Try

            oLock = New bPMLock.User
            If oLock.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If

            nResult = oLock.LockKey(sKeyName:=v_sKeyName, vKeyValue:=v_nKeyValue, iUserID:=v_nUserID, sCurrentlyLockedBy:=r_sLockedBy, v_bOtherUserOnly:=False)


        Catch ex As Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lock KeyName: " & v_sKeyName & " KeyValue: " & CStr(v_nKeyValue), vApp:=ACApp, vClass:=ACClass, vMethod:="LockKey", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)


        Finally
            If Not (oLock Is Nothing) Then
                m_lReturn = oLock.Terminate()
                oLock = Nothing
            End If



        End Try
        Return nResult
    End Function

    ''' <summary>
    ''' UnLockKey
    ''' </summary>
    ''' <param name="v_sKeyName"></param>
    ''' <param name="v_nKeyValue"></param>
    ''' <param name="v_nUserID"></param>
    ''' <returns></returns>
    ''' <remarks>unlock specified key</remarks>
    Public Function UnLockKey(ByVal v_sKeyName As String, ByVal v_nKeyValue As Integer, ByVal v_nUserID As Integer) As Integer
        Dim oLock As bPMLock.User
        Dim nResult As Integer = 0
        Try

            oLock = New bPMLock.User
            If oLock.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If

            nResult = oLock.UnLockKey(sKeyName:=v_sKeyName, vKeyValue:=v_nKeyValue, iUserID:=v_nUserID)


        Catch ex As Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lock KeyName: " & v_sKeyName & " KeyValue: " & CStr(v_nKeyValue), vApp:=ACApp, vClass:=ACClass, vMethod:="LockKey", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)


        Finally
            If Not (oLock Is Nothing) Then
                m_lReturn = oLock.Terminate()
                oLock = Nothing
            End If


        End Try
        Return nResult
    End Function

End Class
