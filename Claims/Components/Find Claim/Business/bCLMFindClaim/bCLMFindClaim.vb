Option Strict Off
Option Explicit On
Imports System.Text
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable


    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 14/07/2000
    '
    ' Description: Creatable Bussiness class which contains all the
    '              methods, business rules required for the
    '              SIRFindClaim .
    '
    ' Edit History:Pandu
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 11/12/2003
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
    Private Const ACClass As String = "Business"

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' SET 01082002 - Removed for scalability
    'Private oComponentServices As PMServerBusinessCS

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_lError As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
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


    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' Date :15/07/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long


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


            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now



            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
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
    ' Date :15/07/2000
    '
    ' Edit History :Pandu
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
    ' Date :15/07/2000
    '
    ' Edit History :Pandu
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
    ' Name:Get Claim Details
    '
    ' Description:  SQL Query to Select Claim details
    '
    ' Date :15/07/2000
    '
    ' Edit History :Pandu
    '              :DC201100 - get extra details for list claims
    '              :DC010200 - get extra details for claims summary srn
    '              :MKW190503- restrict search to sources available to user
    ' ***************************************************************** '
    'modified guide no 33
    'Public Function GetClaimDetails(ByRef r_vResultArray As Object, ByVal v_vSiriusProduct As String, Optional ByVal v_vClaimNumber As String = "", Optional ByVal v_vClientName As String = "", Optional ByVal v_vPolicyNumber As String = "", Optional ByVal v_vRegNumber As Object = Nothing, Optional ByVal v_vLossFromdate As String = "", Optional ByVal v_vLossToDate As Object = Nothing, Optional ByVal v_vClaimStatus As Boolean = False, Optional ByVal v_vValidSourceArray() As Object = Nothing, Optional ByVal v_vClientResolvedName As Object = Nothing, Optional ByVal v_vInsurer As Object = Nothing, Optional ByVal v_vAccountExecutive As Object = Nothing, Optional ByVal v_vVehicleRegistration As Object = Nothing) As Integer
    Public Function GetClaimDetails(ByRef r_vResultArray(,) As Object, ByVal v_vSiriusProduct As String, Optional ByVal v_vClaimNumber As Object = Nothing, Optional ByVal v_vClientName As Object = Nothing, Optional ByVal v_vPolicyNumber As Object = Nothing, Optional ByVal v_vRegNumber As Object = Nothing, Optional ByVal v_vLossFromdate As Object = Nothing, Optional ByVal v_vLossToDate As Object = Nothing, Optional ByVal v_vClaimStatus As Object = Nothing, Optional ByVal v_vValidSourceArray As Object = Nothing, Optional ByVal v_vClientResolvedName As Object = Nothing, Optional ByVal v_vInsurer As Object = Nothing, Optional ByVal v_vAccountExecutive As Object = Nothing, Optional ByVal v_vVehicleRegistration As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim iParamCount As Integer
        Dim sVbsFlag As String = ""
        'DC250501 for use with checking for apostrophes
        Dim sShortname As String = ""
        'S4B Claim Enhancements R&D 2005
        Dim sClientResolvedName, sAccountExec, sInsurer, sVehicleReg As String

        'MKW 190503 PN2032 START
        Dim sTemp As New StringBuilder
        Dim nLower, nUpper As Integer

        'DD 08/10/2003 - Added for filtering in a multi-company broking setup
        Dim bFilterOutOtherBranches As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If Not Informations.IsNothing(v_vLossFromdate) Then
                If v_vLossFromdate <> "" Then
                    If Not Informations.IsDate(v_vLossFromdate) Then

                        Return gPMConstants.PMEReturnCode.PMFalse

                    End If
                End If
            End If


            If Not Informations.IsNothing(v_vLossToDate) Then

                If CStr(v_vLossToDate) <> "" Then
                    If Not Informations.IsDate(v_vLossToDate) Then

                        Return gPMConstants.PMEReturnCode.PMFalse
                    Else
                        Dim lossToDate As DateTime = CDate(v_vLossToDate)
                        If lossToDate.Hour = 0 Then

                            lossToDate = lossToDate.AddHours(23)

                            lossToDate = lossToDate.AddMinutes(59)

                            lossToDate = lossToDate.AddSeconds(59)

                            v_vLossToDate = lossToDate
                        End If
                    End If
                End If
            End If

            ' Build the SQL select statement according to the parameters passed
            ' Select statement to select all details relating to values entered
            sSQL = ""
            sSQL = sSQL & "SELECT Claim.Policy_id,Claim.Claim_id,Claim.Description,Claim.Claim_Number," & Strings.ChrW(13) & Strings.ChrW(10)

            'RiskIndex and Product Code are required for UnderWriting


            sSQL = sSQL & "Claim.Policy_Number,Claim.client_short_name," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "(" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "SELECT  prd.description" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM    product prd," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "insurance_file ifi" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "Where ifi.insurance_file_cnt = claim.Policy_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND ifi.product_id = prd.product_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & ") product, " & Strings.ChrW(13) & Strings.ChrW(10)

            bFilterOutOtherBranches = False

            'DC010201 recoded following to make more sense + added selection of
            '           primary cause, secondary cause & progress status
            sSQL = sSQL & "Claim.loss_from_date,Claim.client_name," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "Claim.claim_status_id, " & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & "( " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " SELECT ISNULL(Handler.description,'') " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " FROM Handler " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " WHERE handler_id = Claim.handler_id " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & ") Handler, " & Strings.ChrW(13) & Strings.ChrW(10)


            sSQL = sSQL & "Claim.insurer_claim_number, Claim.client_claim_number, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "Claim.client_tel_no, Claim.client_tel_no_off, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "( " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " SELECT ISNULL(Primary_Cause.description,'') " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " FROM Primary_Cause " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " WHERE primary_cause_id = Claim.primary_cause_id " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & ") Primary_Cause, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "( " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " SELECT ISNULL(Secondary_Cause.description,'') " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " FROM Secondary_Cause " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " WHERE secondary_cause_id = Claim.secondary_cause_id " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & ") Secondary_Cause, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "( " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " SELECT ISNULL(Progress_Status.description,'') " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " FROM Progress_Status " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " WHERE progress_status_id = Claim.progress_status_id " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & ") Progress_Status, " & Strings.ChrW(13) & Strings.ChrW(10)
            'DC250501 -start -get client code also
            sSQL = sSQL & "claim.client_short_name " & Strings.ChrW(13) & Strings.ChrW(10)
            'DC250501 -end

            'MKW170903 PN6326 START

            sSQL = sSQL & "FROM Claim" & Strings.ChrW(13) & Strings.ChrW(10)

            'DC010201

            ' MKW 190503 PN2032 START - link party table
            sSQL = sSQL & "  JOIN party" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "   ON party.shortname = claim.client_short_name"

            'S4B Claim Enhancements R&D 2005
            sSQL = sSQL & " JOIN insurance_file I" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON I.insurance_file_cnt = claim.policy_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " JOIN party Insurer" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON I.lead_insurer_cnt = Insurer.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " LEFT OUTER JOIN party ACExec" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON party.consultant_cnt = ACExec.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)

            If bFilterOutOtherBranches Then
                sSQL = sSQL & "  JOIN Insurance_file ifl" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "   ON ifl.insurance_file_cnt = claim.policy_id "
            End If

            'append the parameters to the where clause
            iParamCount = 0

            'if the field value ClaimNumber supplied

            If Not Informations.IsNothing(v_vClaimNumber) Then
                If v_vClaimNumber <> "" Then
                    'increase the parameter count by 1
                    iParamCount += 1

                    If iParamCount > 1 Then
                        sSQL = sSQL & " AND"
                    Else
                        sSQL = sSQL & " WHERE"
                    End If
                    sSQL = sSQL & " Claim.Claim_Number Like '" & v_vClaimNumber.Trim() & "'" & Strings.ChrW(13) & Strings.ChrW(10)

                End If
            End If

            'if the field value ShortName supplied

            If Not Informations.IsNothing(v_vClientName) Then
                If v_vClientName <> "" Then

                    'DC250501 -start -check for apostrophes and double up for searching
                    sShortname = v_vClientName
                    m_lReturn = CType(Apostrophes(sShortname), gPMConstants.PMEReturnCode)
                    'DC250501 -end

                    'increase the parameter count by 1
                    iParamCount += 1

                    If iParamCount > 1 Then
                        sSQL = sSQL & " AND"
                    Else
                        sSQL = sSQL & " WHERE"
                    End If

                    'DC250501 -start -use sShortname thats been checked for apostrophes
                    'sSQL = sSQL & " Claim.Client_Short_Name Like '" & Trim$(CStr(v_vClientName)) & "'" & vbCrLf
                    sSQL = sSQL & " Claim.Client_Short_Name Like '" & sShortname.Trim() & "'" & Strings.ChrW(13) & Strings.ChrW(10)
                    'DC250501 -end

                End If
            End If

            'if the field value PolicyNumber supplied

            If Not Informations.IsNothing(v_vPolicyNumber) Then
                If v_vPolicyNumber <> "" Then


                    'increase the parameter count by 1
                    iParamCount += 1

                    If iParamCount > 1 Then
                        sSQL = sSQL & " AND"
                    Else
                        sSQL = sSQL & " WHERE"
                    End If
                    sSQL = sSQL & " Claim.Policy_Number Like '" & v_vPolicyNumber.Trim() & "'" & Strings.ChrW(13) & Strings.ChrW(10)

                End If
            End If



            'if the field value LossFromDate supplied

            If Not Informations.IsNothing(v_vLossFromdate) Then
                If v_vLossFromdate <> "" Then

                    'increase the parameter count by 1
                    iParamCount += 1

                    If iParamCount > 1 Then
                        sSQL = sSQL & " AND"
                    Else
                        sSQL = sSQL & " WHERE"
                    End If

                    sSQL = sSQL & " convert(datetime,claim.loss_from_date,103) >= '" & StringsHelper.Format(v_vLossFromdate.Trim(), ACDateReverse) & "'" & Strings.ChrW(13) & Strings.ChrW(10)

                End If
            End If

            'if the field value LossToDate supplied

            If Not Informations.IsNothing(v_vLossToDate) Then

                If CStr(v_vLossToDate) <> "" Then

                    'increase the parameter count by 1
                    iParamCount += 1

                    If iParamCount > 1 Then
                        sSQL = sSQL & " AND"
                    Else
                        sSQL = sSQL & " WHERE"
                    End If


                    sSQL = sSQL & " convert(datetime,claim.loss_from_date,103) <= '" & StringsHelper.Format(CStr(v_vLossToDate).Trim(), ACDateReverse) & "'" & Strings.ChrW(13) & Strings.ChrW(10)

                End If
            End If

            'if the field value IS supplied

            If Not Informations.IsNothing(v_vClaimStatus) Then
                If v_vClaimStatus Then

                    'therefore, Address Cnt is present
                    'increase the parameter count by 1
                    iParamCount += 1

                    If iParamCount > 1 Then
                        sSQL = sSQL & " AND"
                    Else
                        sSQL = sSQL & " WHERE"
                    End If
                    sSQL = sSQL & "(Claim.Claim_Status_id  = " & CStr(CLMProvisionalOpenClaim).Trim() & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & " OR Claim.Claim_Status_id = " & CStr(CLMLiveOpenClaim).Trim() & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & " OR Claim.Claim_Status_id = " & CStr(CLMReOpened).Trim() & ")" & Strings.ChrW(13) & Strings.ChrW(10)

                End If
            End If

            'S4B Claim Enhancements R&D 2005

            If Not Informations.IsNothing(v_vClientResolvedName) Then
                sClientResolvedName = gPMFunctions.ToSafeString(v_vClientResolvedName).Trim()
                If sClientResolvedName <> "" Then
                    Apostrophes(sClientResolvedName)
                    iParamCount += 1
                    sSQL = sSQL & (If(iParamCount > 1, " AND", " WHERE"))
                    sSQL = sSQL & " party.resolved_name LIKE '" & sClientResolvedName & "'" & Strings.ChrW(13) & Strings.ChrW(10)
                End If
            End If

            'S4B Claim Enhancements R&D 2005

            If Not Informations.IsNothing(v_vAccountExecutive) Then
                sAccountExec = gPMFunctions.ToSafeString(v_vAccountExecutive).Trim()
                If sAccountExec <> "" Then
                    iParamCount += 1
                    sSQL = sSQL & (If(iParamCount > 1, " AND", " WHERE"))
                    'DC270606 PN29151 changed = to LIKE
                    sSQL = sSQL & " ACExec.shortname LIKE '" & sAccountExec & "'" & Strings.ChrW(13) & Strings.ChrW(10)
                End If
            End If

            'S4B Claim Enhancements R&D 2005

            If Not Informations.IsNothing(v_vInsurer) Then
                sInsurer = gPMFunctions.ToSafeString(v_vInsurer).Trim()
                If sInsurer <> "" Then
                    iParamCount += 1
                    sSQL = sSQL & (If(iParamCount > 1, " AND", " WHERE"))
                    'DC270606 PN29151 changed = to LIKE
                    sSQL = sSQL & " Insurer.shortname LIKE '" & sInsurer & "'" & Strings.ChrW(13) & Strings.ChrW(10)
                End If
            End If

            'S4B Claim Enhancements R&D 2005

            If Not Informations.IsNothing(v_vVehicleRegistration) Then
                sVehicleReg = gPMFunctions.ToSafeString(v_vVehicleRegistration).Trim().Replace("%", "")
                'If Replace(sVehicleReg, "%", "") <> "" Then
                If sVehicleReg.Trim() <> "" Then
                    Apostrophes(sVehicleReg)
                    iParamCount += 1
                    sSQL = sSQL & (If(iParamCount > 1, " AND", " WHERE"))
                    sSQL = sSQL & " EXISTS(SELECT NULL FROM claim_party_link CPL JOIN party_other PO ON CPL.party_cnt=PO.party_cnt WHERE CPL.claim_id=Claim.claim_id AND PO.reg_number LIKE '" & sVehicleReg & "' AND PO.reg_number <> '') " & Strings.ChrW(13) & Strings.ChrW(10)
                End If
            End If

            'DD 08/10/2003: Filter out branches not belonging to the logged
            'in user if flag says so
            If bFilterOutOtherBranches Then
                iParamCount += 1

                If iParamCount > 1 Then
                    sSQL = sSQL & " AND"
                Else
                    sSQL = sSQL & " WHERE"
                End If
                sSQL = sSQL & " ifl.source_id  = " & CStr(m_iSourceID) & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            'DC010201 no longer required
            '    'DC201100 - Start - Get Handler Id
            '    If iParamCount > 0 Then
            '        sSQL = sSQL & " AND"
            '    Else
            '        sSQL = sSQL & " WHERE"
            '    End If
            '
            '    sSQL = sSQL & " Handler.handler_id = Claim.handler_id" & vbCrLf
            '    'DC201100 - End - Get Handler Id
            'DC010201

            If iParamCount = 0 Then
                'no parameters passed so query cannot be executed
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' MKW 190503 PN2032 START.  Put valid sources in sql
            If Informations.IsArray(v_vValidSourceArray) Then
                nLower = v_vValidSourceArray.GetLowerBound(1)
                nUpper = v_vValidSourceArray.GetUpperBound(1)

                sTemp = New StringBuilder("")

                For iLoop As Integer = nLower To nUpper
                    If iLoop = nLower Then
                        sTemp = New StringBuilder("(party.source_id IN (")
                    End If


                    sTemp.Append(CStr(Val(CStr(v_vValidSourceArray(1, iLoop)))))

                    If iLoop = nUpper Then
                        sTemp.Append("))")
                    Else
                        sTemp.Append(",")
                    End If
                Next

                iParamCount += 1

                If iParamCount > 1 Then
                    sSQL = sSQL & " AND "
                Else
                    sSQL = sSQL & " WHERE "
                End If

                sSQL = sSQL & sTemp.ToString()

            End If
            ' MKW 190503 PN2032 END.

            ' Execute SQL Statement - use array for speed
            With m_oDatabase
                .Parameters.Clear()

                'DC310701 gets all records not just first 500
                m_lError = .SQLSelect(sSQL:=sSQL, sSQLName:=ACClaimFileSearchName, bStoredProcedure:=ACClaimFileSearchStored, vResultArray:=r_vResultArray, lNumberRecords:=gPMConstants.PMAllRecords, bKeepNulls:=True)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimDetails")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' If NO records were found return PMFalse
                If Not Informations.IsArray(r_vResultArray) Then
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Search Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:Get Claim Details UW
    '
    ' Description:  SQL Query to Select Claim details for Underwriting Only
    '
    ' Date :18/05/2001
    '
    ' Edit History :
    ' ***************************************************************** '
    Public Function GetClaimDetailsUW(ByRef r_vResultArray(,) As Object, ByVal v_vSiriusProduct As String, Optional ByVal v_vClaimNumber As Object = Nothing, Optional ByVal v_vPolicyNumber As Object = Nothing, Optional ByVal v_vClientName As Object = Nothing, Optional ByVal v_vRegNumber As Object = Nothing, Optional ByVal v_vLossFromdate As Object = Nothing, Optional ByVal v_vLossToDate As Object = Nothing, Optional ByVal v_vClaimStatus As Object = Nothing, Optional ByVal v_lCaseID As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Build parameters


            bPMAddParameter.AddParameterLite(m_oDatabase, "ClaimNumber", If(CStr(v_vClaimNumber).Length, v_vClaimNumber, DBNull.Value), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True)


            bPMAddParameter.AddParameterLite(m_oDatabase, "PolicyNumber", If(CStr(v_vPolicyNumber).Length, v_vPolicyNumber, DBNull.Value), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)


            bPMAddParameter.AddParameterLite(m_oDatabase, "ClientShortName", If(CStr(v_vClientName).Length, v_vClientName, DBNull.Value), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)


            bPMAddParameter.AddParameterLite(m_oDatabase, "LossDateFrom", If(CStr(v_vLossFromdate).Length, v_vLossFromdate, DBNull.Value), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)


            bPMAddParameter.AddParameterLite(m_oDatabase, "LossDateTo", If(CStr(v_vLossToDate).Length, v_vLossToDate, DBNull.Value), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            bPMAddParameter.AddParameterLite(m_oDatabase, "IncludeClosed", If(v_vClaimStatus = "True", 1, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMBoolean)
            bPMAddParameter.AddParameterLite(m_oDatabase, "UserID", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "SourceID", m_iSourceID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "CaseID", v_lCaseID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            ' Execute SQL Statement - use array for speed
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACFindClaimDetailsUWSQL, sSQLName:=ACFindClaimDetailsUWName, bStoredProcedure:=ACFindClaimDetailsUWStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimDetailsUW")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If NO records were found return PMFalse
            If Not Informations.IsArray(r_vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Search Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimDetailsUW", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: DeleteClaim
    '
    ' Description:
    '
    ' History: 08/05/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function DeleteClaim(ByVal v_lClaimId As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_id", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteClaimSQL, sSQLName:=ACDeleteClaimName, bStoredProcedure:=ACDeleteClaimStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteClaim", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CopyClaimToWork
    '
    ' Description:
    '
    ' History: 08/05/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    'Public Function CopyClaimtoWork(ByVal v_lClaimId As Long, _
    ''                          ByRef r_lCopyClaimId As Long) As Long
    '
    'Dim lStatus As Long
    '
    '    On Error GoTo Err_CopyClaimToWork
    '
    '    CopyClaimtoWork = PMTrue
    '
    '    m_oDatabase.Parameters.Clear
    '
    '    m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_id", _
    ''                                           vValue:=v_lClaimId, _
    ''                                           iDirection:=PMParamInput, _
    ''                                           iDataType:=PMLong)
    '    If (m_lReturn <> PMTrue) Then
    '        CopyClaimtoWork = PMFalse
    '        Exit Function
    '    End If
    '
    '    m_lReturn = m_oDatabase.Parameters.Add(sName:="copy_claim_id", _
    ''                                           vValue:=0, _
    ''                                           iDirection:=PMParamOutput, _
    ''                                           iDataType:=PMLong)
    '    If (m_lReturn <> PMTrue) Then
    '        CopyClaimtoWork = PMFalse
    '        Exit Function
    '    End If
    '
    '    m_lReturn = m_oDatabase.Parameters.Add(sName:="status", _
    ''                                           vValue:=0, _
    ''                                           iDirection:=PMParamOutput, _
    ''                                           iDataType:=PMLong)
    '    If (m_lReturn <> PMTrue) Then
    '        CopyClaimtoWork = PMFalse
    '        Exit Function
    '    End If
    '
    '    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyClaimToWorkSQL, _
    ''                                      sSQLName:=ACCopyClaimToWorkName, _
    ''                                      bStoredProcedure:=ACCopyClaimToWorkStored)
    '    If (m_lReturn <> PMTrue) Then
    '        CopyClaimtoWork = PMFalse
    '        Exit Function
    '    End If
    '
    '    ' SET 01082002 - Scalability
    '    lStatus = NullToLong(m_oDatabase.Parameters.Item("status").value)
    '
    '    If (lStatus = -1) Then
    '        CopyClaimtoWork = PMFalse
    '        Exit Function
    '    End If
    '
    '    ' SET 01082002 - Scalability
    '    r_lWorkClaimId = NullToLong(m_oDatabase.Parameters.Item("work_claim_id").value)
    '    ' RVH - 26/2/2003   Check if claimsbuilder is enabled and if it IS
    '    '                   then we need to copy some GIS related data from
    '    '                   live to work
    '    If ClaimBuilderIsEnabled Then
    '        m_lReturn = CopyClaimToWorkGIS(v_lClaimId, r_lWorkClaimId)
    '
    '        If (m_lReturn <> PMTrue) Then
    '            CopyClaimtoWork = PMFalse
    '            ' Log Error Message
    '            LogMessage m_sUsername, _
    ''                iType:=PMLogError, _
    ''                sMsg:="Failed to copy GIS-related claim details from live to work (CopyClaimToWorkGIS)", _
    ''                vApp:=ACApp, _
    ''                vClass:=ACClass, _
    ''                vMethod:="CopyClaimToWork"
    '            Exit Function
    '        End If
    '    End If
    '    ' RVH - END
    '    Exit Function
    '
    'Err_CopyClaimToWork:
    '
    '    CopyClaimtoWork = PMError
    '
    '    ' Log Error Message
    '    LogMessage m_sUsername, _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="CopyClaimToWork Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="CopyClaimToWork", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function


    Public Sub New()
        MyBase.New()

        Try

            Dim vDatabase As Object = Nothing

            ' Class Initialise
            m_oDatabase = New dPMDAO.Database()




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try



    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' Author        : Richard Hill
    ' Date          : 18/04/2001
    ' Description   : Function to get all Claim Details that match on insurance_file_cnt found in GIS Index search
    '                   plus add GIS Index value to result array
    '
    ' Note          : JMK - amalgamation of GetAllPolicyByGISSearchIndex (Ram Chandrabose)
    '                 and GetUWPolicyList
    'developer guide no.33
    Public Function GetMultiPolicyClaims(ByRef vInputData As Object, ByRef vOutputData As Object, ByRef v_vSiriusProduct As Object, Optional ByVal v_vClaimNumber As Object = Nothing, Optional ByVal v_vClientName As Object = Nothing, Optional ByVal v_vPolicyNumber As Object = Nothing, Optional ByVal v_vRegNumber As Object = Nothing, Optional ByVal v_vLossFromdate As Object = Nothing, Optional ByVal v_vLossToDate As Object = Nothing, Optional ByVal v_vClaimStatus As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim NoofFields, iMaxRow, iFromRow As Integer
        Dim sSQL As New StringBuilder
        Dim vTempData As Object = Nothing
        Dim vResultData(,) As Object = Nothing
        Dim iParamCount As Integer
        'developer guide no.33
        Dim vInsuranceFileCnt As Object
        Dim vPropertyValue As Object
        Dim vGISPolicyLink As Object
        Dim vClaimCnt As Object

        'DC250501 used for searching for apostrophes
        Dim sShortname As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If Not Informations.IsNothing(v_vLossFromdate) Then
                If v_vLossFromdate <> "" Then
                    If Not Informations.IsDate(v_vLossFromdate) Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            Dim nParty_cnt(,) As Object = Nothing
            Dim sSql2 As String = String.Empty

            sSql2 = "select party_cnt from Pmuser where user_id =" & m_iUserID & Strings.ChrW(13) & Strings.ChrW(10)

            With m_oDatabase
                m_lError = .SQLSelect(sSQL:=sSql2.ToString(), sSQLName:="", bStoredProcedure:=ACClaimFileSearchStored, vResultArray:=nParty_cnt, bKeepNulls:=True)
                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartycnt")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            If Not Informations.IsNothing(v_vLossToDate) Then
                If v_vLossToDate <> "" Then
                    If Not Informations.IsDate(v_vLossToDate) Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            If Informations.IsArray(vInputData) Then

                ' Get all related data
                For iCounter As Integer = vInputData.GetLowerBound(1) To vInputData.GetUpperBound(1)

                    ' Initialise the search Criteria Variables

                    'developer guide no.33
                    'start
                    vInsuranceFileCnt = vInputData(0, iCounter)

                    vGISPolicyLink = vInputData(1, iCounter)

                    vPropertyValue = vInputData(4, iCounter)
                    'end
                    vClaimCnt = vInputData(vInputData.GetUpperBound(0), iCounter)

                    ' Build the SQL select statement according to the parameters passed
                    ' Select statement to select all details relating to values entered
                    sSQL = New StringBuilder("")
                    sSQL.Append("SELECT Claim.Policy_id,Claim.Claim_id,Claim.Description,Claim.Claim_Number," & Strings.ChrW(13) & Strings.ChrW(10))


                    'RiskIndex and Product Code are required for UnderWriting



                    'TN20010505 Start
                    'sSQL = sSQL & "Claim.Policy_Number,Claim.Risk_type_id,Claim.client_short_name," & vbCrLf   'Risk_Type.code,Risk_code.code," & vbCrLf

                    sSQL.Append("Claim.Policy_Number,Claim.client_short_name," & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("(" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("SELECT  prd.description" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("FROM    product prd," & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("insurance_file ifi" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("Where ifi.insurance_file_cnt = claim.Policy_id" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("AND ifi.product_id = prd.product_id" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(") product, " & Strings.ChrW(13) & Strings.ChrW(10))

                    'TN20010505 End

                    'DC010201 recoded following to make more sense + added selection of
                    '           primary cause, secondary cause & progress status
                    sSQL.Append("Claim.loss_from_date,Claim.client_name," & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("Claim.claim_status_id, " & Strings.ChrW(13) & Strings.ChrW(10))

                    sSQL.Append("( " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(" SELECT ISNULL(Handler.description,'') " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(" FROM Handler " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(" WHERE handler_id = Claim.handler_id " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(") Handler, " & Strings.ChrW(13) & Strings.ChrW(10))


                    sSQL.Append("Claim.insurer_claim_number, Claim.client_claim_number, " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("Claim.client_tel_no, Claim.client_tel_no_off, " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("( " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(" SELECT ISNULL(Primary_Cause.description,'') " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(" FROM Primary_Cause " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(" WHERE primary_cause_id = Claim.primary_cause_id " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(") Primary_Cause, " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("( " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(" SELECT ISNULL(Secondary_Cause.description,'') " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(" FROM Secondary_Cause " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(" WHERE secondary_cause_id = Claim.secondary_cause_id " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(") Secondary_Cause, " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("( " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(" SELECT ISNULL(Progress_Status.description,'') " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(" FROM Progress_Status " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(" WHERE progress_status_id = Claim.progress_status_id " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(") Progress_Status, " & Strings.ChrW(13) & Strings.ChrW(10))

                    ' RVH 04/01/2005 - Corrected potential bug. Changes made in May 2004 meant that if a search was performed by "risk index" the
                    '                  find claim step would crash when the matching claim was selected and the "OK" button clicked...5 additional
                    '                  columns are required to be returned...payments, reserve, iso_code (currency), is_deleted (source), closed_allow_claims (source)
                    sSQL.Append("isnull(" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("(" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("select sum(isnull(r.paid_to_date,0))" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("from reserve r join claim_peril cp on r.claim_peril_id = cp.claim_peril_id and claim_id=claim.claim_id" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(")" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(",0) Payments," & Strings.ChrW(13) & Strings.ChrW(10))

                    sSQL.Append("(" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("(select isnull(sum(isnull(r.revised_reserve,0)),0) + isnull(sum(isnull(r.initial_reserve,0)),0) from reserve r join claim_peril cp on r.claim_peril_id = cp.claim_peril_id and claim_id=claim.claim_id)" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(") Reserve," & Strings.ChrW(13) & Strings.ChrW(10))

                    sSQL.Append("(" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("select  cu.iso_code" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("from    currency cu" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("Where cu.currency_id = claim.currency_id" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(") iso_code," & Strings.ChrW(13) & Strings.ChrW(10))

                    sSQL.Append("(" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("select s.is_deleted" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("from source s" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("inner join insurance_file ifi ON ifi.source_id = s.source_id" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("Where ifi.insurance_file_cnt = claim.Policy_id" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(") is_deleted," & Strings.ChrW(13) & Strings.ChrW(10))

                    sSQL.Append("(" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("select s.closed_allow_claims" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("from source s" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("inner join insurance_file ifi ON ifi.source_id = s.source_id" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("Where ifi.insurance_file_cnt = claim.Policy_id" & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(") closed_allow_claims" & Strings.ChrW(13) & Strings.ChrW(10))

                    sSQL.Append("FROM Claim" & Strings.ChrW(13) & Strings.ChrW(10))

                    If Not Informations.IsNothing(nParty_cnt) AndAlso nParty_cnt(0, 0) <> 0 Then
                        sSQL.Append("INNER JOIN insurance_file ifi ON claim.Policy_id=ifi.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10))
                    End If

                    sSQL.Append(" INNER JOIN (SELECT MAX(Version_id) as version_id,MAX(Claim_Id) as claim_id, base_claim_id FROM claim " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(" WHERE is_dirty = 0 GROUP BY base_claim_id ) claim_version ON " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append(" Claim.claim_id = claim_version.claim_id " & Strings.ChrW(13) & Strings.ChrW(10))
                    'DC010201

                    'append the parameters to the where clause
                    iParamCount = 0

                    'if the field value ClaimNumber supplied

                    If Not Informations.IsNothing(v_vClaimNumber) Then
                        If v_vClaimNumber <> "" Then
                            'increase the parameter count by 1
                            iParamCount += 1

                            If iParamCount > 1 Then
                                sSQL.Append(" AND")
                            Else
                                sSQL.Append(" WHERE")
                            End If
                            sSQL.Append(" Claim.Claim_Number Like '" & v_vClaimNumber.Trim() & "'" & Strings.ChrW(13) & Strings.ChrW(10))

                        End If
                    End If

                    'if the field value ShortName supplied

                    If Not Informations.IsNothing(v_vClientName) Then
                        If v_vClientName <> "" Then

                            sShortname = v_vClientName
                            m_lReturn = CType(Apostrophes(sShortname), gPMConstants.PMEReturnCode)

                            'increase the parameter count by 1
                            iParamCount += 1

                            If iParamCount > 1 Then
                                sSQL.Append(" AND")
                            Else
                                sSQL.Append(" WHERE")
                            End If

                            'DC250501 -start -use sShortname as its been check for apostrophes
                            'sSQL = sSQL & " Claim.Client_Short_Name Like '" & Trim$(CStr(v_vClientName)) & "'" & vbCrLf
                            sSQL.Append(" Claim.Client_Short_Name Like '" & sShortname.Trim() & "'" & Strings.ChrW(13) & Strings.ChrW(10))
                            'DC250501 -end

                        End If
                    End If


                    'if the field value LossFromDate supplied

                    If Not Informations.IsNothing(v_vLossFromdate) Then
                        If v_vLossFromdate <> "" Then


                            'increase the parameter count by 1
                            iParamCount += 1

                            If iParamCount > 1 Then
                                sSQL.Append(" AND")
                            Else
                                sSQL.Append(" WHERE")
                            End If
                            'sSQL = sSQL & " claim.loss_from_date Between CONVERT(DATETIME, '" & Trim$(CStr(Format(DateAdd("d", -1, v_vLossFromdate), "d/m/yyyy"))) & "',103) " & _
                            '"And CONVERT(DATETIME, '" & Trim(CStr(Format(DateAdd("d", 1, v_vLossFromdate), "d/m/yyyy"))) & "',103)" & vbCrLf

                            sSQL.Append(" convert(char(12),claim.loss_from_date,103)=CONVERT(char(12), '" & StringsHelper.Format(v_vLossFromdate, ACDateConversion).Trim() & "',103)" & Strings.ChrW(13) & Strings.ChrW(10))
                        End If
                    End If

                    'if the field value IS supplied

                    If Not Informations.IsNothing(v_vLossToDate) Then
                        If v_vLossToDate <> "" Then

                            'therefore, Address Cnt is present
                            'increase the parameter count by 1
                            iParamCount += 1

                            If iParamCount > 1 Then
                                sSQL.Append(" AND")
                            Else
                                sSQL.Append(" WHERE")
                            End If
                            'sSQL = sSQL & " claim.loss_to_date Between CONVERT(DATETIME, '" & Trim$(CStr(Format(DateAdd("d", -1, v_vLossToDate), "d/m/yyyy"))) & "',103) " & _
                            '"And CONVERT(DATETIME, '" & Trim(CStr(Format(DateAdd("d", 1, v_vLossToDate), "d/m/yyyy"))) & "',103)" & vbCrLf

                            sSQL.Append(" convert(char(12),claim.loss_to_date,103)=CONVERT(char(12), '" & StringsHelper.Format(v_vLossToDate, ACDateConversion).Trim() & "',103)" & Strings.ChrW(13) & Strings.ChrW(10))

                        End If
                    End If

                    'if the field value IS supplied

                    If Not Informations.IsNothing(v_vClaimStatus) Then
                        If v_vClaimStatus Then

                            'therefore, Address Cnt is present
                            'increase the parameter count by 1
                            iParamCount += 1

                            If iParamCount > 1 Then
                                sSQL.Append(" AND")
                            Else
                                sSQL.Append(" WHERE")
                            End If
                            sSQL.Append("(Claim.Claim_Status_id  = " & CStr(CLMProvisionalOpenClaim).Trim() & Strings.ChrW(13) & Strings.ChrW(10))
                            sSQL.Append(" OR Claim.Claim_Status_id = " & CStr(CLMLiveOpenClaim).Trim() & Strings.ChrW(13) & Strings.ChrW(10))
                            sSQL.Append(" OR Claim.Claim_Status_id = " & CStr(CLMReOpened).Trim() & ")" & Strings.ChrW(13) & Strings.ChrW(10))

                        End If
                    End If



                    If vInsuranceFileCnt <> 0 Then
                        'increase the parameter count by 1
                        iParamCount += 1

                        If iParamCount > 1 Then
                            sSQL.Append(" AND")
                        Else
                            sSQL.Append(" WHERE")
                        End If
                        sSQL.Append(" Claim.Policy_id = " & vInsuranceFileCnt & Strings.ChrW(13) & Strings.ChrW(10))
                    End If


                    If vClaimCnt <> 0 Then
                        'increase the parameter count by 1
                        iParamCount += 1

                        If iParamCount > 1 Then
                            sSQL.Append(" AND")
                        Else
                            sSQL.Append(" WHERE")
                        End If
                        sSQL.Append(" Claim.claim_id = " & vClaimCnt & Strings.ChrW(13) & Strings.ChrW(10))
                    End If

                    If (Not Informations.IsNothing(nParty_cnt) AndAlso ((nParty_cnt(0, 0) <> 0) OrElse (nParty_cnt(0, 0) IsNot Nothing))) Then
                        sSQL.Append(" AND")
                        sSQL.Append(" ifi.lead_agent_cnt = " & nParty_cnt(0, 0) & Strings.ChrW(13) & Strings.ChrW(10))

                    End If
                    If iParamCount = 0 Then
                        'no parameters passed so query cannot be executed
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Execute SQL Statement - use array for speed
                    With m_oDatabase

                        .Parameters.Clear()

                        m_lError = .SQLSelect(sSQL:=sSQL.ToString(), sSQLName:=ACClaimFileSearchName, bStoredProcedure:=ACClaimFileSearchStored, vResultArray:=vTempData, bKeepNulls:=True)

                        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Log Error Message
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimDetails")

                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End With

                    '        ' If NO records were found return PMFalse
                    '        If (IsArray(vTempData) = False) Then
                    '            GetMultiPolicyClaims = PMNotFound
                    '            Exit Function
                    '        End If


                    If Informations.IsArray(vTempData) Then
                        ' We have some search results for this Insurance cnt.
                        ' So merge the result Array

                        ' Get the no of fields selected

                        NoofFields = vTempData.GetUpperBound(0)

                        If Not Informations.IsArray(vResultData) Then


                            vResultData = vTempData
                        Else
                            ' We alreay have some data and we have to merge it with new data

                            iFromRow = vResultData.GetUpperBound(1)


                            iMaxRow = vResultData.GetUpperBound(1) + vTempData.GetUpperBound(1) + 1
                            ReDim Preserve vResultData(NoofFields, iMaxRow)


                            For iCounter1 As Integer = vTempData.GetLowerBound(1) To vTempData.GetUpperBound(1)
                                iFromRow += 1
                                For iCounter2 As Integer = 0 To NoofFields


                                    vResultData(iCounter2, iFromRow) = vTempData(iCounter2, iCounter1)
                                Next iCounter2
                            Next iCounter1
                        End If
                    End If


                Next iCounter


            End If
            ' If NO records were found return PMFalse
            If Not Informations.IsArray(vResultData) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Set the return Value

            vOutputData = vResultData

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUWPolicyByGISSearchIndex Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUWPolicyByGISSearchIndex", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    'DC250501 routine to search for apostrophes and double for searching purposes
    '******************************************************************************
    ' Apostrophes
    '
    ' Take a string and replace ' with ''
    '
    '******************************************************************************
    Public Function Apostrophes(ByRef sString As String) As Integer

        Dim result As Integer = 0
        Dim i As Integer
        Dim sTemp As New StringBuilder

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If sString.Length = 0 Then
                Return result
            End If

            sTemp = New StringBuilder("")

            Do While True
                i = (sString.IndexOf("'"c) + 1)

                If i = 0 Then
                    sTemp.Append(sString)
                    Exit Do
                End If

                sTemp.Append(sString.Substring(0, i - 1) & "''")
                sString = sString.Substring(i)
            Loop

            sString = sTemp.ToString()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Run Time Error", vApp:=ACApp, vClass:="ExtraFunc", vMethod:="Apostrophes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '**********************************************************************
    ' Function Name:    ClaimBuilderIsEnabled
    ' Author:           Russell Hill
    ' Date:             26/2/2003
    ' Description:      Check if SIROPTClaimsBuilder product option is ON
    '**********************************************************************
    Private Function ClaimBuilderIsEnabled() As Boolean

        Dim result As Boolean = False
        'developer guide no.101
        Dim vResult As Object = Nothing

        Try

            result = True
            'developer guide no. 98
            bPMFunc.getProductOptionValue(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTClaimsBuilder, v_vBranch:=m_iSourceID, r_vUnderwriting:=vResult)


            Return vResult = 1

        Catch
        End Try



        Return False

    End Function

    '**********************************************************************
    ' Function Name:    CopyClaimToWorkGISNEWWITHVERSIONS
    ' Author:           Russell Hill
    ' Date:             26/2/2003
    ' Description:      Copy GIS related elements from live to work tables
    ' Updates:          MEvans : 07-01-2004 : CQ3414
    '                       Changed to support claim gis versioning
    '**********************************************************************
    Private Function CopyClaimToWorkGISNEWWITHVERSIONS(ByVal v_lClaimId As Integer, ByVal v_lWorkClaimId As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "CopyClaimToWorkGISNEWWITHVERSIONS"

        Dim sDataModelCode As String = String.Empty
        Dim vGISPolicyLink As Object = Nothing
        Dim lGisPolicyLinkId, lNewGisPolicyLinkId As Integer
        Dim bCommitTrans, bTransOpen As Boolean
        Dim sQuoteRefPassword As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '*****************
            ' Updated version should create an entry in the gis_policy_link table
            ' using spg_DATAMODEL_copydataset
            ' copy all claim / claim peril data to DATAMODEL work tables
            ' using spg_DATAMODEL_copy_claim_to_work
            ' and then update gis_policy_link with the work_claim_id
            '*****************

            bCommitTrans = False

            ' so first thing we need to know is the gis_policy_link_id for
            ' the current claim_id...
            If GetGisPolicyLinkDetails(v_lClaimId:=v_lClaimId, r_vResults:=vGISPolicyLink) = gPMConstants.PMEReturnCode.PMTrue Then

                ' if the details have been retrieved
                If Informations.IsArray(vGISPolicyLink) Then

                    ' get the gis policy link id

                    lGisPolicyLinkId = CInt(vGISPolicyLink(0, 0))

                    sQuoteRefPassword = CStr(vGISPolicyLink(2, 0))


                    ' attempt to get the relevant datamodel code for this claim
                    If GetClaimDataModelCode(v_lClaimId:=v_lWorkClaimId, r_sDataModelCode:=sDataModelCode) = gPMConstants.PMEReturnCode.PMTrue Then

                        ' start transaction
                        m_lReturn = m_oDatabase.SQLBeginTrans()
                        bTransOpen = True

                        ' Create the new gis policy link and copy the original datasets
                        ' data to it
                        If CopyGISDataSet(v_sDataModelCode:=sDataModelCode, v_lOriginalGisPolicyLinkId:=lGisPolicyLinkId, r_lNewGisPolicyLinkId:=lNewGisPolicyLinkId) = gPMConstants.PMEReturnCode.PMTrue Then

                            ' update the quote reference fields on the new gis policy link..
                            If UpdateGisPolicyLinkQuoteRef(v_sQuoteRefPassword:=sQuoteRefPassword, v_sDataModelCode:=sDataModelCode, v_lGisPolicyLinkId:=lNewGisPolicyLinkId) = gPMConstants.PMEReturnCode.PMTrue Then

                                ' Copy the GIS claim / claim peril details to the work tables
                                If CopyGISClaim(v_sDataModelCode:=sDataModelCode, v_lClaimId:=v_lWorkClaimId, v_lWorkClaimId:=v_lClaimId) = gPMConstants.PMEReturnCode.PMTrue Then

                                    ' Associated the work claim with the new gis policy link
                                    If UpdateGisPolicyLinkDetails(v_lCopyClaimId:=v_lWorkClaimId, v_lGisPolicyLinkId:=lNewGisPolicyLinkId) = gPMConstants.PMEReturnCode.PMTrue Then
                                        bCommitTrans = True
                                    Else
                                        ' Log Error
                                        result = gPMConstants.PMEReturnCode.PMFalse

                                        Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                                        oDict.Add("v_lClaimId", v_lClaimId)
                                        oDict.Add("v_lWorkClaimId", v_lWorkClaimId)
                                        gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to copy gis claim details for claim:" & CStr(v_lClaimId) & " datamodel:" & sDataModelCode, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

                                    End If

                                Else
                                    ' Log Error
                                    result = gPMConstants.PMEReturnCode.PMFalse

                                    Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                                    oDict.Add("v_lClaimId", v_lClaimId)
                                    oDict.Add("v_lWorkClaimId", v_lWorkClaimId)
                                    gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to copy gis claim details for claim:" & CStr(v_lClaimId) & " datamodel:" & sDataModelCode, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)
                                End If
                            Else
                                ' Log Error
                                result = gPMConstants.PMEReturnCode.PMFalse

                                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                                oDict.Add("v_lClaimId", v_lClaimId)
                                oDict.Add("v_lWorkClaimId", v_lWorkClaimId)
                                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to update quote reference fields for gis policy link:" & CStr(lNewGisPolicyLinkId), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)
                            End If

                        Else
                            ' Log Error
                            result = gPMConstants.PMEReturnCode.PMFalse

                            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                            oDict.Add("v_lClaimId", v_lClaimId)
                            oDict.Add("v_lWorkClaimId", v_lWorkClaimId)
                            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to copy gis dataset for gis policy link id:" & CStr(lGisPolicyLinkId), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)
                        End If

                        ' commit the transaction or roll it back as appropriate
                        If bCommitTrans Then
                            m_lReturn = m_oDatabase.SQLCommitTrans()
                            bTransOpen = False
                        Else
                            m_lReturn = m_oDatabase.SQLRollbackTrans()
                            bTransOpen = False
                        End If

                    Else
                        ' Log Error
                        result = gPMConstants.PMEReturnCode.PMFalse

                        Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                        oDict.Add("v_lClaimId", v_lClaimId)
                        oDict.Add("v_lWorkClaimId", v_lWorkClaimId)
                        gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to get the claim data model for claim:" & CStr(v_lWorkClaimId), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)
                    End If

                Else
                    ' NOT FINDING A GIS POLICY LINK FOR THE CLAIM  IS NOT AN ERROR
                    ' just means it is prior to claims builder being switched on
                End If

            Else
                ' Log Error
                result = gPMConstants.PMEReturnCode.PMFalse

                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lClaimId", v_lClaimId)
                oDict.Add("v_lWorkClaimId", v_lWorkClaimId)
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " GetGisPolicyLinkid Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)
            End If

            Return result

        Catch excep As System.Exception



            ' rollback transaction if one is still open
            If bTransOpen Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()
            End If

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyClaimToWorkGIS Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyClaimToWorkGIS", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '**********************************************************************
    ' Function Name:    CopyClaimGIS
    ' Author:           Russell Hill
    ' Date:             26/2/2003
    ' Description:      Copy GIS related elements from live to work tables
    '**********************************************************************

    'Private Function CopyClaimGIS(ByVal v_lClaimId As Integer, ByVal v_lCopyClaimId As Integer) As Integer
    'Dim result As Integer = 0
    'Dim bTransOpen As Boolean
    'Dim lErrNumber As Integer
    'Dim sErrDescription As String = ""
    'Try 
    '
    'Dim lStatus As Integer
    'Dim sSQL, sDataModelCode As String
    'Dim vGISPolicyLink As Object
    'Dim lGisPolicyLinkId As Integer
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' get the data model code
    'm_lReturn = CType(GetClaimDataModelCode(v_lClaimId:=v_lCopyClaimId, r_sDataModelCode:=sDataModelCode), gPMConstants.PMEReturnCode)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the data model code for the passed claim id:" & v_lClaimId, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyClaimGIS")
    'Return result
    'End If
    '
    ' get current policy link details for the passed "live" claim id
    'm_lReturn = CType(GetGisPolicyLinkDetails(v_lClaimId:=v_lClaimId, r_vResults:=vGISPolicyLink), gPMConstants.PMEReturnCode)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the gis policy link record for the passed claim id:" & v_lClaimId, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyClaimGIS")
    'Return result
    'End If
    '
    ' if the details have not been retrieved log informational message and exit...
    'If Not Informations.IsArray(vGISPolicyLink) Then
    ' Log informational message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="INFO ONLY: Failed to get the gis policy link record for the passed claim id:" & v_lClaimId & ". Likely cause: claim created prior to enabling ClaimsBuilder.", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyClaimGIS")
    'Return result
    'End If
    '
    ' get the gis policy link id

    'lGisPolicyLinkId = CInt(vGISPolicyLink(0, 0))
    '
    ' start transaction
    'm_lReturn = m_oDatabase.SQLBeginTrans()
    'bTransOpen = True
    '
    ' copy the live claim gis details to gis work tables
    'bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", v_lClaimId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
    'bPMAddParameter.AddParameterLite(m_oDatabase, "copy_claim_id", v_lCopyClaimId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
    '
    'sSQL = ACGISCopyClaimToWorkSQLStart & sDataModelCode & ACGISCopyClaimToWorkSQLEnd
    '
    'm_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:=ACGISCopyClaimToWorkName, bStoredProcedure:=ACGISCopyClaimToWorkStored)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    'm_lReturn = m_oDatabase.SQLRollbackTrans()
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to copy the GIS claim details to work tables for the passed claim id:" & v_lClaimId, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyClaimGIS")
    'Return result
    'End If
    '
    '    ' now toggle the live and work claim id's on the gis policy link record
    '    m_lReturn = UpdateGisPolicyLinkDetails(v_lCopyClaimId:=v_lCopyClaimId, v_lGisPolicyLinkId:=lGisPolicyLinkId)
    ''
    '    If m_lReturn <> PMTrue Then
    '        CopyClaimGIS = PMFalse
    '        m_lReturn = m_oDatabase.SQLRollbackTrans
    '        ' Log Error Message
    '        LogMessage m_sUsername, _
    ''            iType:=PMLogError, _
    ''            sMsg:="Failed to update the gis policy link record for gis_policy_link_id:" & lGisPolicyLinkId, _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="CopyClaimGIS"
    '        Exit Function
    '    End If
    '
    'm_lReturn = m_oDatabase.SQLCommitTrans()
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' store error details
    'lErrNumber = Informations.Err().Number
    'sErrDescription = excep.Message
    '
    ' rollback transaction if one is still open
    'If bTransOpen Then
    'm_lReturn = m_oDatabase.SQLRollbackTrans()
    'End If
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyClaimGIS Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyClaimGIS", vErrNo:=lErrNumber, vErrDesc:=sErrDescription)
    '
    'Return result
    '
    'End Try
    'End Function


    '**********************************************************************
    ' Function Name:    GetClaimDataModelCode
    ' Author:           Russell Hill
    ' Date:             26/2/2003
    ' Description:      Get the datamodel code for the current claim
    '**********************************************************************
    Private Function GetClaimDataModelCode(ByVal v_lClaimId As Integer, ByRef r_sDataModelCode As String) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_id", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Datamodel_Code", vValue:="", iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetDatamodeCodeforClaimSQL, sSQLName:=ACGetDatamodeCodeforClaimName, bStoredProcedure:=ACGetDatamodeCodeforClaimStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        r_sDataModelCode = gPMFunctions.NullToString(m_oDatabase.Parameters.Item("Datamodel_Code").Value).Trim()

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetGisPolicyLinkDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 07-01-2004 : CQ3414
    ' ***************************************************************** '
    Public Function GetGisPolicyLinkDetails(ByVal v_lClaimId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetGisPolicyLinkDetails"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", v_lClaimId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=ACGetGisPolicyLinkDetailsSQL, sSQLName:=ACGetGisPolicyLinkDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lClaimId", v_lClaimId)
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)
                '******************************

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lClaimId", v_lClaimId)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '******************************

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: CopyGISDataSet
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 07-01-2004 : CQ3414
    ' ***************************************************************** '
    Private Function CopyGISDataSet(ByVal v_sDataModelCode As String, ByVal v_lOriginalGisPolicyLinkId As Integer, ByRef r_lNewGisPolicyLinkId As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "CopyGISDataSet"

        Dim sCopyGISDataSetSQL As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Add Required Stored Procedure Parameters

        ' old gis policy link id
        bPMAddParameter.AddParameterLite(m_oDatabase, "old_gis_policy_link_id", v_lOriginalGisPolicyLinkId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

        ' copy quotes - its a required param but doesnt do anything so just default to zero...
        bPMAddParameter.AddParameterLite(m_oDatabase, "copy_quotes", 0, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

        ' new gis policy link id
        bPMAddParameter.AddParameterLite(m_oDatabase, "new_gis_policy_link_id", 0, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

        ' Generate Required Stored Procedure Name
        sCopyGISDataSetSQL = ACGISCopyDatasetStart & v_sDataModelCode & ACGISCopyDatasetEnd

        ' Execute Action Query
        If m_oDatabase.SQLAction(sSQL:=sCopyGISDataSetSQL, sSQLName:="Copy GIS DataSet", bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMFalse

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_sDataModelCode", v_sDataModelCode)
            oDict.Add("v_lOriginalGisPolicyLinkId", v_lOriginalGisPolicyLinkId)
            oDict.Add("r_lNewGisPolicyLinkId", r_lNewGisPolicyLinkId)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to copydataset " & sCopyGISDataSetSQL & " for gis_policy_link_id :" & CStr(v_lOriginalGisPolicyLinkId), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)
            '******************************

        Else
            r_lNewGisPolicyLinkId = m_oDatabase.Parameters.Item("new_gis_policy_link_id").Value
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CopyGISClaim
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 07-01-2004 : CQ3414
    ' ***************************************************************** '
    Private Function CopyGISClaim(ByVal v_sDataModelCode As String, ByVal v_lClaimId As Integer, ByVal v_lWorkClaimId As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "CopyGISClaim"

        Dim sCopyGisClaimSQL As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Add Required Stored Procedure Parameters

        ' Claim Id
        bPMAddParameter.AddParameterLite(m_oDatabase, "Copy_Claim_id", v_lClaimId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
        ' Work Claim Id
        bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", v_lWorkClaimId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        ' Generate Required Procedure Name
        sCopyGisClaimSQL = ACGISCopyClaimToWorkSQLStart & v_sDataModelCode & ACGISCopyClaimToWorkSQLEnd

        ' Execute Action Query
        If m_oDatabase.SQLAction(sSQL:=sCopyGisClaimSQL, sSQLName:="Copy GIS Claim - Claim Peril Details", bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMFalse

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_sDataModelCode", v_sDataModelCode)
            oDict.Add("v_lClaimId", v_lClaimId)
            oDict.Add("v_lWorkClaimId", v_lWorkClaimId)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to copy gis claim for claim_id:" & CStr(v_lClaimId) & " using procedure:" & sCopyGisClaimSQL, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)
            '******************************

        End If

        Return result

    End Function
    '' ***************************************************************** '
    '' Name: UpdateGisPolicyLinkDetails
    ''
    '' Parameters: n/a
    ''
    '' Description:
    ''
    '' History:
    ''           Created : MEvans : 07-01-2004 : CQ3414
    '' ***************************************************************** '
    Private Function UpdateGisPolicyLinkDetails(ByVal v_lCopyClaimId As Integer, ByVal v_lGisPolicyLinkId As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "UpdateGisPolicyLinkDetails"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Add Required Stored Procedure Parameters

        ' Gis Policy Link Id
        bPMAddParameter.AddParameterLite(m_oDatabase, "gis_policy_link_id", v_lGisPolicyLinkId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
        ' Work Claim Id
        bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", v_lCopyClaimId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
        ' Claim Id - set to "-1" as we want to write this away as "NULL" on the policy link
        'Call AddParameterLite(m_oDatabase, "claim_id", -1, PMParamInput, PMLong, False)

        ' Execute Action Query
        If m_oDatabase.SQLAction(sSQL:=ACUpdateGisPolicyLinkDetailsSQL, sSQLName:=ACUpdateGisPolicyLinkDetailsName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMFalse

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lCopyClaimId", v_lCopyClaimId)
            oDict.Add("v_lGisPolicyLinkId", v_lGisPolicyLinkId)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to update gis_policy_link:" & CStr(v_lGisPolicyLinkId) & " with work claim id " & CStr(v_lCopyClaimId), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)
            '******************************

        End If

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: UpdateGisPolicyLinkQuoteRef
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 07-01-2004 : CQ3414
    ' ***************************************************************** '
    Private Function UpdateGisPolicyLinkQuoteRef(ByVal v_sQuoteRefPassword As String, ByVal v_sDataModelCode As String, ByVal v_lGisPolicyLinkId As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "UpdateGisPolicyLinkQuoteRef"
        Dim sQuoteRef As Object 
        Dim oGis As Object  'bGIS.Application



        result = gPMConstants.PMEReturnCode.PMTrue

        ' create business object
        'oGis = New bGIS.Application
        oGis = Nothing
        result = gPMComponentServices.CreateBusinessObject(r_oObject:=oGis, v_sClassName:="bGIS.Application", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            Dim r_sMessage As String = "Failed to create an instance of bGIS.Application"
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="bGIS.Application", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
            Return result
        End If
        If oGis.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database)) = gPMConstants.PMEReturnCode.PMTrue Then

            ' generate quote reference from the gis policy link id

            If oGis.GenerateQuoteRef(v_lGisPolicyLinkId:=ToSafeInteger(v_lGisPolicyLinkId), r_sQuoteRef:=sQuoteRef, v_sGisDataModelCode:=ToSafeString(v_sDataModelCode)) = gPMConstants.PMEReturnCode.PMTrue Then

                ' Update the Quote Ref and Password

                If oGis.UpdateQuoteRef(v_lGisPolicyLinkId:=ToSafeInteger(v_lGisPolicyLinkId), v_sQuoteRef:=ToSafeString(sQuoteRef), v_sQuoteRefPassword:=ToSafeString(v_sQuoteRefPassword), v_sGisDataModelCode:=ToSafeString(v_sDataModelCode)) <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Log Error.
                    result = gPMConstants.PMEReturnCode.PMFalse

                    Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                    oDict.Add("v_sDataModelCode", v_sDataModelCode)
                    oDict.Add("v_lGisPolicyLinkId", v_lGisPolicyLinkId)
                    gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to Update quote ref for gis policy link" & CStr(v_lGisPolicyLinkId), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)
                End If

            Else
                ' Log Error.
                result = gPMConstants.PMEReturnCode.PMFalse

                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_sDataModelCode", v_sDataModelCode)
                oDict.Add("v_lGisPolicyLinkId", v_lGisPolicyLinkId)
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to Generate Quote Ref for gis policy link:" & CStr(v_lGisPolicyLinkId), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

            End If

        Else

            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_sDataModelCode", v_sDataModelCode)
            oDict.Add("v_lGisPolicyLinkId", v_lGisPolicyLinkId)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to create instance of bGIS.application", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

        End If



        ' destroy instance of gis object
        oGis = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ProcessCopyClaim
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-03-2006 : Claims Versioning Changes
    ' ***************************************************************** '
    Public Function ProcessCopyClaim(ByVal v_lClaimId As Integer, ByRef r_lCopyClaimId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessCopyClaim"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin transaction
            m_oDatabase.SQLBeginTrans()

            ' copy the claim related details
            lReturn = CType(CopyClaim(v_lClaimId:=v_lClaimId, r_lCopyClaimId:=r_lCopyClaimId), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "CopyClaim Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' if claims builder is enabled
            If ClaimBuilderIsEnabled() And r_lCopyClaimId <> 0 Then
                If m_sTransactionType = "C_CR" And IsInfoOnlyClaim(r_lCopyClaimId) Then
                    'Information Only version
                Else
                    ' copy the gis data as well
                    lReturn = CType(CopyClaimToWorkGISNEWWITHVERSIONS(v_lClaimId, r_lCopyClaimId), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "CopyClaimToWorkGIS Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If
            End If

            ' Commit transaction after all processing is complete
            m_oDatabase.SQLCommitTrans()


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

            ' Rollback Transaction
            m_oDatabase.SQLRollbackTrans()

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: CopyClaim
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-03-2006 : Claims Versioning Changes
    ' ***************************************************************** '
    Public Function CopyClaim(ByVal v_lClaimId As Integer, ByRef r_lCopyClaimId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CopyClaim"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lStatus As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", v_lClaimId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "transaction_type_code", m_sTransactionType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "created_by_id", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "copy_claim_id", 0, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "status", 0, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            ' Execute Action Query
            lReturn = m_oDatabase.SQLAction(sSQL:=kCopyClaimSQL, sSQLName:=kCopyClaimName, bStoredProcedure:=True)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kCopyClaimSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lStatus = gPMFunctions.ToSafeLong(m_oDatabase.Parameters.Item("status").Value, 0)
            If lStatus <> 0 Then
                gPMFunctions.RaiseError(kMethodName, "copy claim returned invalid status", gPMConstants.PMELogLevel.PMLogError)
            End If

            r_lCopyClaimId = gPMFunctions.ToSafeLong(m_oDatabase.Parameters.Item("copy_claim_id").Value, 0)

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetClaimVersions
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 10-03-2006 : Claims Versioning Changes
    ' ***************************************************************** '
    Public Function GetClaimVersions(ByVal v_lClaimId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimVersions"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="claim_id", v_vValue:=v_lClaimId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetClaimVersionsSQL, sSQLName:=kGetClaimVersionsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetClaimVersionsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

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
    ' Name: IsInfoOnlyClaim
    '
    ' Parameters: n/a
    '
    ' Description:
    '           Takes claim_id and returns its previous version's Info_Only bit
    ' History:
    '           Created : PM : 22-11-2006 : PN #30511
    ' ***************************************************************** '
    Public Function IsInfoOnlyClaim(ByVal v_lClaimId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "IsInfoOnlyClaim"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim r_vResults(,) As Object = Nothing
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="claim_id", v_vValue:=v_lClaimId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kIsInfoOnlyClaimSQL, sSQLName:=kIsInfoOnlyClaimName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kIsInfoOnlyClaimSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                result = gPMFunctions.ToSafeBoolean(r_vResults(0, 0))
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
    ' Name: FindClaim
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 10-03-2006 : Claims Versioning Changes
    ' ***************************************************************** '
    Public Function FindClaim(ByVal v_sShortname As String, ByVal v_sInsuranceRef As String, ByVal v_lClaimId As Integer, ByRef r_vResults(,) As Object, Optional ByVal v_bViaClaimVersionList As Boolean = False) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "FindClaim"
        Dim vResult(,) As Object = Nothing
        Dim lReturn As gPMConstants.PMEReturnCode
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_lReturn = GetUserOtherParty(iUserID:=m_iUserID, r_vResultArray:=vResult)

            'm_lReturn = GetUserOtherParty(iUserID:=m_iUserID, r_vResultArray:=vResult)

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' only add required stored procedure parameters
            If v_sShortname <> "" Then
                'Modified by Vijay Pal on 6/26/2010 12:40:07 PM refer as per vb code
                'm_lReturn = CType(AddInputParameter(v_sName:="shortname", v_vValue:=CInt(v_sShortname), v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
                m_lReturn = CType(AddInputParameter(v_sName:="shortname", v_vValue:=v_sShortname, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            End If

            If v_sInsuranceRef <> "" Then
                'Modified by Vijay Pal on 6/26/2010 12:40:38 PM refer as per vb code
                'm_lReturn = CType(AddInputParameter(v_sName:="insurance_ref", v_vValue:=CInt(v_sInsuranceRef), v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
                m_lReturn = CType(AddInputParameter(v_sName:="insurance_ref", v_vValue:=v_sInsuranceRef, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            End If

            If v_lClaimId <> 0 Then
                m_lReturn = CType(AddInputParameter(v_sName:="claim_id", v_vValue:=v_lClaimId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            End If

            If v_bViaClaimVersionList Then
                m_lReturn = CType(AddInputParameter(v_sName:="ViaClaimVersionList", v_vValue:=v_bViaClaimVersionList, v_iType:=gPMConstants.PMEDataType.PMBoolean), gPMConstants.PMEReturnCode)
            End If
            If Informations.IsArray(vResult) AndAlso vResult(0, 0).ToString() <> "" Then

                m_lReturn = CType(AddInputParameter(v_sName:="TPAID", v_vValue:=vResult(0, 0).ToString(), v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            End If
            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kFindClaimSQL, sSQLName:=kFindClaimName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kFindClaimSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

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
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    'Modified by Vijay Pal on 6/26/2010 12:50:20 PM refer guide no 33,as per vb code
    'Public Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Integer, ByVal v_iType As Integer) As Integer
    Public Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddInputParameter"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add Parameter to database object
            'Modified as per vb code
            'lReturn = m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=CStr(v_vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType)
            lReturn = m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=v_vValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, " Failed to add parameter name:" & v_sName &
                                        ", values :" & CStr(v_vValue) & ", type:" & CStr(v_iType), gPMConstants.PMELogLevel.PMLogError)
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
    ' Name: GetOriginalClaimId
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 13-03-2006 : Claims Versioning Changes
    ' ***************************************************************** '
    Public Function GetOriginalClaimId(ByVal v_lClaimId As Integer, ByRef r_lOriginalClaimId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetOriginalClaimId"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vResults(,) As Object = Nothing

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="claim_id", v_vValue:=v_lClaimId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetOriginalClaimIdSQL, sSQLName:=kGetOriginalClaimIdName, bStoredProcedure:=True, vResultArray:=vResults, lNumberRecords:=gPMConstants.PMAllRecords)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetOriginalClaimIdSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(vResults) Then

                r_lOriginalClaimId = CInt(vResults(0, 0))
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
    ' Name: CleanUpDirtyClaims
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 21-03-2006 : Claims Versioning Changes
    ' ***************************************************************** '
    Public Function CleanUpDirtyClaims(ByVal v_lClaimId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CleanUpDirtyClaims"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            AddInputParameter("claim_id", v_lClaimId, gPMConstants.PMEDataType.PMLong)

            ' Execute Action Query
            lReturn = m_oDatabase.SQLAction(sSQL:=kCleanUpDirtyClaimsSQL, sSQLName:=kCleanUpDirtyClaimsName, bStoredProcedure:=True)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kCleanUpDirtyClaimsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
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

    Public Function CheckReferredPayment(ByVal v_lClaimId As Integer, ByRef r_bStatus As Boolean, Optional ByRef r_iNoofReferredPayments As Integer = 0, Optional ByRef r_cSumofReferredPayments As Decimal = 0) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_bStatus = False
            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACGetReferredPaymentSQL, sSQLName:=ACGetReferredPaymentName, bStoredProcedure:=ACGetReferredPaymentStored, vResultArray:=vResultArray, bKeepNulls:=True)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            If gPMFunctions.ToSafeDouble(vResultArray(0, 0)) <> 0 Then
                r_bStatus = True

                r_iNoofReferredPayments = gPMFunctions.ToSafeInteger(vResultArray(0, 0))

                r_cSumofReferredPayments = gPMFunctions.ToSafeCurrency(vResultArray(1, 0))
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckReferredPayment Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckReferredPayment", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function FindOtherClaims(ByVal v_lPartyCnt As Integer, ByRef r_vOtherClaimDetails(,) As Object) As Integer

        ' Debug message
        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".FindOtherClaims")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            If Not False Then
                ' Add the party_cnt
                m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Call the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCLMGetOtherClaimsSQL, sSQLName:=ACCLMGetOtherClaimsName, bStoredProcedure:=ACCLMGetOtherClaimsStored, vResultArray:=r_vOtherClaimDetails, lNumberRecords:=gPMConstants.PMAllRecords, bKeepNulls:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".FindOtherClaims")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchAll Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindOtherClaims", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result





            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name:Get Claim Details UW
    '
    ' Description:  SQL Query to Select Claim details for Underwriting Only
    '
    ' Date :15/01/2009
    '
    ' Edit History :
    ' ***************************************************************** '
    Public Function GetClaimDetailsSFU(ByRef r_vResultArray(,) As Object, Optional ByVal v_vClaimNumber As Object = Nothing, Optional ByVal v_vClientName As Object = Nothing, Optional ByVal v_vPolicyNumber As Object = Nothing, Optional ByVal v_vRegNumber As Object = Nothing, Optional ByVal v_vLossFromdate As Object = Nothing, Optional ByVal v_vLossToDate As Object = Nothing, Optional ByVal v_vClaimStatus As Boolean = False, Optional ByVal v_lCaseID As Integer = 0, Optional ByVal v_sOtherParty As String = "") As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimDetailsSFU"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Build parameters

            If Informations.IsNothing(v_vClaimNumber) Then
                v_vClaimNumber = ""
            End If
            If Informations.IsNothing(v_vPolicyNumber) Then
                v_vPolicyNumber = ""
            End If
            If Informations.IsNothing(v_vClientName) Then
                v_vClientName = ""
            End If
            If Informations.IsNothing(v_vLossFromdate) Then
                v_vLossFromdate = ""
            End If
            If Informations.IsNothing(v_vLossToDate) Then
                v_vLossToDate = ""
            End If

            bPMAddParameter.AddParameterLite(m_oDatabase, "ClaimNumber", If(CStr(v_vClaimNumber).Length, v_vClaimNumber, DBNull.Value), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "PolicyNumber", If(CStr(v_vPolicyNumber).Length, v_vPolicyNumber, DBNull.Value), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "ClientShortName", If(CStr(v_vClientName).Length, v_vClientName, DBNull.Value), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "LossDateFrom", If(CStr(v_vLossFromdate).Length, v_vLossFromdate, DBNull.Value), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            bPMAddParameter.AddParameterLite(m_oDatabase, "LossDateTo", If(CStr(v_vLossToDate).Length, v_vLossToDate, DBNull.Value), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            bPMAddParameter.AddParameterLite(m_oDatabase, "IncludeClosed", If(v_vClaimStatus, 1, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMBoolean)
            bPMAddParameter.AddParameterLite(m_oDatabase, "UserID", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "SourceID", m_iSourceID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "CaseID", v_lCaseID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "TPA", v_sOtherParty, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            ' Execute SQL Statement - use array for speed
            lReturn = m_oDatabase.SQLSelect(sSQL:=kFindClaimDetailsUWSQL, sSQLName:=kFindClaimDetailsUWName, bStoredProcedure:=kFindClaimDetailsUWStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' If NO records were found return PMFalse
            If Not Informations.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    'Start - Sankar - (WPRvb64 Media Type Status) - Paralleling
    Public Function ProcessPolicyReceiptMediaTypeStatus(ByVal v_lInsuranceFileId As Integer, ByVal v_dtLossDate As Date, ByRef r_bProceed As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessPolicyReceiptMediaTypeStatus"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "IsValid", r_bProceed, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMBoolean)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Insurance_File_Cnt", v_lInsuranceFileId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Claim_Loss_Date", v_dtLossDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)

            ' Execute SQL Statement
            lReturn = m_oDatabase.SQLAction(sSQL:=kCheckPolicyReceiptMediaTypeStatusSQL, sSQLName:=kCheckPolicyReceiptMediaTypeStatusName, bStoredProcedure:=kCheckPolicyReceiptMediaTypeStatusStored)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to execute the stored procedure: " & kCheckPolicyReceiptMediaTypeStatusSQL, gPMConstants.PMELogLevel.PMLogError)
            End If

            r_bProceed = gPMFunctions.ToSafeBoolean(m_oDatabase.Parameters.Item("IsValid").Value)


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
    'End - Sankar - (WPRvb64 Media Type Status) - Paralleling
    Private Shared _DefaultInstance As Business = Nothing
    Public Shared ReadOnly Property DefaultInstance() As Business
        Get
            If _DefaultInstance Is Nothing Then
                _DefaultInstance = New Business
            End If
            Return _DefaultInstance
        End Get
    End Property

    Public Function GetUserOtherParty(ByVal iUserID As Integer, ByRef r_vResultArray(,) As Object) As Long
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "userid", iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)


            ' Execute the stored procedure.

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetUserotherpartySQL, sSQLName:=ACGetUserotherpartyName, bStoredProcedure:=ACGetUserotherpartyStored, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Get user other party Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Getuserotherparty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function GetLatestClaimId(ByVal v_sClaimRef As String, ByRef r_lLatestClaimId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetLatestClaimId"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vResults As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="claim_number", v_vValue:=v_sClaimRef, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetLatestClaimIdSQL, sSQLName:=kGetLatestClaimIdName, bStoredProcedure:=True, vResultArray:=vResults, lNumberRecords:=gPMConstants.PMAllRecords)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetLatestClaimIdSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Information.IsArray(vResults) Then
                r_lLatestClaimId = CInt(vResults(0, 0))
            End If

            Return lReturn

        Catch excep As System.Exception

            ' DO Not Call any functions before here or the error will be lost
            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Get user other party Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Getuserotherparty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

            ' If you want to rollback a transaction or something, do it here

        End Try
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

