Option Strict Off
Option Explicit On
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Maintenance_NET.Maintenance")>
Public NotInheritable Class Maintenance
    Implements IDisposable

    ' ************************************************
    ' Added to replace global variables 02/04/2007
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
    ' Class Name: Maintenance
    '
    ' Date: 31/10/2000
    '
    ' Description: Creatable Maintenance class used by the Premium Finance
    '              Maintenance.
    '
    ' Edit History:
    ' TF311000 -    Created
    ' SJP14062002 - getUnderWritingOrAgency uses new product options scheme and bas file
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Maintenance"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_lError As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_sStructure As String = ""

    'sj 3/11/99 - start
    Private m_lDataSource As Integer
    Private m_cInvariantKeys As ArrayList
    Private m_oPMBusiness As Object
    Private m_vPMResultArray As Object
    'sj 3/11/99 - end

    Private m_bOKToDelete As Boolean

    Private m_sNoDeleteReasons As String = ""

    Private m_bCheckedOKToDelete As Boolean

    ' Primary Keys to work with
    Private m_lPartyCnt As Integer

    ' PM Event Business Component (Private)
    Private m_oEvent As bSIREvent.Business
    Private m_sUnderwritingOrAgency As String = ""

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

    Public Property PartyCnt() As Integer
        Get

            Return m_lPartyCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPartyCnt = Value

        End Set
    End Property
    'sj 3/11/99 - start
    Public WriteOnly Property DataSource() As Integer
        Set(ByVal Value As Integer)
            m_lDataSource = Value
        End Set
    End Property
    'sj 3/11/99 - end

    Public ReadOnly Property OKToDelete() As Boolean
        Get

            If Not m_bCheckedOKToDelete Then
                m_lReturn = CheckOKToDelete()
            End If

            Return m_bOKToDelete

        End Get
    End Property

    Public ReadOnly Property NoDeleteReasons() As String
        Get

            Return m_sNoDeleteReasons

        End Get
    End Property

    Public ReadOnly Property UnderwritingOrAgency() As String
        Get

            If m_sUnderwritingOrAgency = "" Then
                m_lReturn = getUnderwritingOrAgency()
            End If

            Return m_sUnderwritingOrAgency

        End Get
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

            m_lReturn = CType(gPMComponentServices.CheckDatabase(v_sUsername:=sUserName, v_iSourceID:=iSourceID, v_iLanguageID:=iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the ProcessMode etc.
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                If m_oEvent IsNot Nothing Then
                    m_oEvent.Dispose()
                    m_oEvent = Nothing
                End If
                m_cInvariantKeys = Nothing
                m_oPMBusiness = Nothing
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

                m_sTransactionType = gPMFunctions.ToSafeString(vTransactionType)
            End If

            If Not Informations.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '****************************************************************** '
    ' Name: SearchByQuery (Public)
    '
    ' Description: Selects Schemes according to the query by example
    '               parameters
    '****************************************************************** '

    Public Function SearchByQuery(ByRef r_vResultArray(,) As Object, ByRef r_lNumberOfRecords As Integer, Optional ByVal v_vCompanyNumber As Object = Nothing, Optional ByVal v_vSchemeNumber As Object = Nothing, Optional ByVal v_vSchemeName As Object = Nothing, Optional ByVal v_vPartyCode As Object = Nothing, Optional ByVal v_vPartyName As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim sSQL As String
        Dim iParamCount As Integer

        Dim sCompanyNumber As String = String.Empty
        Dim sSchemeNumber As String = String.Empty
        Dim sSchemeName As String = String.Empty
        Dim sPartyCode As String = String.Empty
        Dim sPartyName As String = String.Empty

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'CT 2/8/00 fix to allow clients with apostrophies in name to be found
            sPartyCode = v_vPartyCode
            m_lReturn = CType(bPMFunc.ValidateSQL(sPartyCode), gPMConstants.PMEReturnCode)
            v_vPartyCode = sPartyCode
            'CT end

            If (Not Informations.IsNothing(v_vCompanyNumber)) AndAlso (Not Object.Equals(v_vCompanyNumber, Nothing)) Then

                sCompanyNumber = gPMFunctions.ToSafeString(v_vCompanyNumber)
            End If

            If (Not Informations.IsNothing(v_vSchemeNumber)) AndAlso (Not v_vSchemeNumber.Equals(0)) Then
                sSchemeNumber = gPMFunctions.ToSafeString(v_vSchemeNumber)
            End If

            If (Not Informations.IsNothing(v_vSchemeName)) AndAlso (Not v_vSchemeName.Equals(0)) Then
                sSchemeName = gPMFunctions.ToSafeString(v_vSchemeName)
            End If

            If (Not Informations.IsNothing(v_vPartyCode)) AndAlso (Not String.IsNullOrEmpty(v_vPartyCode)) Then
                sPartyCode = v_vPartyCode
            End If

            If (Not Informations.IsNothing(v_vPartyName)) AndAlso (Not v_vPartyName.Equals(0)) Then
                sPartyName = gPMFunctions.ToSafeString(v_vPartyName)
            End If

            sSQL = ""
            sSQL = sSQL & "SELECT DISTINCT "
            sSQL = sSQL & " S.CompanyNo," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " S.SchemeName," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " S.SchemeNo," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " S.SchemeVersion," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " P.shortname," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " P.name," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " P.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & " FROM PFScheme S LEFT JOIN Party P on P.party_cnt = S.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & " WHERE (P.is_deleted = 0 OR P.is_deleted IS NULL)" & Strings.ChrW(13) & Strings.ChrW(10)

            ' Append the parameters to the Where clause
            iParamCount = 1

            ' Look for all matching parties

            'If (Not Informations.IsNothing(v_vCompanyNumber)) And (Not Object.Equals(v_vCompanyNumber, Nothing)) Then
            If (Not Informations.IsNothing(v_vCompanyNumber)) AndAlso (Not Object.Equals(v_vCompanyNumber, Nothing)) Then
                If sCompanyNumber <> "" Then
                    iParamCount += 1
                    If iParamCount > 1 Then
                        sSQL = sSQL & " AND"
                    End If
                    If gPMFunctions.ToSafeString(v_vSchemeNumber).IndexOf("%"c) >= 0 Then
                        sSQL = sSQL & " SchemeNo LIKE '" & gPMFunctions.ToSafeString(v_vSchemeNumber).Trim() & "'" & Strings.ChrW(13) & Strings.ChrW(10)
                    Else
                        sSQL = sSQL & " SchemeNo = " & gPMFunctions.ToSafeString(v_vSchemeNumber).Trim() & Strings.ChrW(13) & Strings.ChrW(10)
                    End If
                End If
            End If

            If (Not Informations.IsNothing(v_vSchemeNumber)) AndAlso (Not v_vSchemeNumber.Equals(0)) Then
                If sSchemeNumber <> "" Then
                    iParamCount += 1
                    If iParamCount > 1 Then
                        sSQL = sSQL & " AND"
                    End If
                    If gPMFunctions.ToSafeString(v_vSchemeNumber).IndexOf("%"c) >= 0 Then
                        sSQL = sSQL & " SchemeNo LIKE '" & gPMFunctions.ToSafeString(v_vSchemeNumber).Trim() & "'" & Strings.ChrW(13) & Strings.ChrW(10)
                    Else
                        sSQL = sSQL & " SchemeNo = " & gPMFunctions.ToSafeString(v_vSchemeNumber).Trim() & Strings.ChrW(13) & Strings.ChrW(10)
                    End If
                End If
            End If

            If (Not Informations.IsNothing(v_vSchemeName)) AndAlso (Not v_vSchemeName.Equals(0)) Then
                If sSchemeName <> "" Then
                    iParamCount += 1
                    If iParamCount > 1 Then
                        sSQL = sSQL & " AND"
                    End If
                    If gPMFunctions.ToSafeString(v_vSchemeName).IndexOf("%"c) >= 0 Then
                        sSQL = sSQL & " SchemeName LIKE '" & gPMFunctions.ToSafeString(v_vSchemeName).Trim() & "'" & Strings.ChrW(13) & Strings.ChrW(10)
                    Else
                        sSQL = sSQL & " SchemeName = '" & gPMFunctions.ToSafeString(v_vSchemeName).Trim() & "'" & Strings.ChrW(13) & Strings.ChrW(10)
                    End If
                End If
            End If

            If (Not Informations.IsNothing(v_vPartyCode)) AndAlso (Not String.IsNullOrEmpty(v_vPartyCode)) Then
                If sPartyCode <> "" Then
                    iParamCount += 1
                    If iParamCount > 1 Then
                        sSQL = sSQL & " AND"
                    End If
                    If v_vPartyCode.IndexOf("%"c) >= 0 Then
                        sSQL = sSQL & " Shortname LIKE '" & v_vPartyCode.Trim() & "'" & Strings.ChrW(13) & Strings.ChrW(10)
                    Else
                        sSQL = sSQL & " Shortname = '" & v_vPartyCode.Trim() & "'" & Strings.ChrW(13) & Strings.ChrW(10)
                    End If
                End If
            End If

            If (Not Informations.IsNothing(v_vPartyName)) AndAlso (Not v_vPartyName.Equals(0)) Then
                If sPartyName <> "" Then
                    iParamCount += 1
                    If iParamCount > 1 Then
                        sSQL = sSQL & " AND"
                    End If
                    If gPMFunctions.ToSafeString(v_vPartyName).IndexOf("%"c) >= 0 Then
                        sSQL = sSQL & " Name LIKE '" & gPMFunctions.ToSafeString(v_vPartyName).Trim() & "'" & Strings.ChrW(13) & Strings.ChrW(10)
                    Else
                        sSQL = sSQL & " Name = '" & gPMFunctions.ToSafeString(v_vPartyName).Trim() & "'" & Strings.ChrW(13) & Strings.ChrW(10)
                    End If
                End If
            End If

            If iParamCount = 1 Then
                'no parameters passed so query cannot be executed
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Order By clause
            sSQL = sSQL & " ORDER BY S.SchemeName" & Strings.ChrW(13) & Strings.ChrW(10)

            ' Execute SQL Statement - use array for speed
            '    With m_oDatabase

            m_oDatabase.Parameters.Clear()

            m_lError = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACSchemeFromQueryName, bStoredProcedure:=ACSchemeFromQueryStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchByQuery")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If NO records were found return PMFalse
            If Not Informations.IsArray(r_vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchByQuery Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchByQuery", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckOKToDelete
    '
    ' Description:
    '
    ' History: 14/01/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function CheckOKToDelete() As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim sString As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        m_bOKToDelete = True

        'There are 6 reasons in PMB why a client cannot be deleted
        '1. Outstanding account records

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=gPMFunctions.ToSafeString(m_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        '    m_lReturn& = m_oDatabase.SQLSelect(sSQL:=ACGetLiveTransactionDetailsSQL, _
        'sSqlName:=ACGetLiveTransactionDetailsName, _
        'bStoredProcedure:=ACGetLiveTransactionDetailsStored, _
        'lNumberRecords:=PMAllRecords, _
        'vResultArray:=vResultArray)

        If Informations.IsArray(vResultArray) Then
            m_bOKToDelete = False
            sString = "Live Transactions exist for this client"
            If m_sNoDeleteReasons = "" Then
                m_sNoDeleteReasons = sString
            Else
                m_sNoDeleteReasons = m_sNoDeleteReasons & Strings.ChrW(13) & Strings.ChrW(10) & sString
            End If
        End If

        vResultArray = Nothing

        '2. Outstanding claims
        'No claims system as yet - but will need to check a system option when there is

        '3. Travel schemes
        'Not relevant

        '4. Live Policies (including being a sharer of premium)

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=gPMFunctions.ToSafeString(m_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        '    m_lReturn& = m_oDatabase.SQLSelect(sSQL:=ACGetLivePolicyDetailsSQL, _
        'sSqlName:=ACGetLivePolicyDetailsName, _
        'bStoredProcedure:=ACGetLivePolicyDetailsStored, _
        'lNumberRecords:=PMAllRecords, _
        'vResultArray:=vResultArray)

        If Informations.IsArray(vResultArray) Then
            m_bOKToDelete = False
            sString = "Live Policies exist for this client"
            If m_sNoDeleteReasons = "" Then
                m_sNoDeleteReasons = sString
            Else
                m_sNoDeleteReasons = m_sNoDeleteReasons & Strings.ChrW(13) & Strings.ChrW(10) & sString
            End If
        End If

        vResultArray = Nothing

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=gPMFunctions.ToSafeString(m_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        '    m_lReturn& = m_oDatabase.SQLSelect(sSQL:=ACGetSharedPremiumDetailsSQL, _
        'sSqlName:=ACGetSharedPremiumDetailsName, _
        'bStoredProcedure:=ACGetSharedPremiumDetailsStored, _
        'lNumberRecords:=PMAllRecords, _
        'vResultArray:=vResultArray)

        If Informations.IsArray(vResultArray) Then
            m_bOKToDelete = False
            sString = "This client shares a premium on policies owned by other clients"
            If m_sNoDeleteReasons = "" Then
                m_sNoDeleteReasons = sString
            Else
                m_sNoDeleteReasons = m_sNoDeleteReasons & Strings.ChrW(13) & Strings.ChrW(10) & sString
            End If
        End If

        vResultArray = Nothing

        '5. Sub Agent
        'Not relevant - but may be when Associated Clients is finally sorted.

        '6. Life Policies
        'Not relevant

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: DeleteParty
    '
    ' Description:
    '
    ' History: 14/01/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function DeleteParty() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=gPMFunctions.ToSafeString(m_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            '    m_lReturn& = m_oDatabase.SQLAction(sSQL:=ACDeletePartySQL, _
            'sSqlName:=ACDeletePartyName, _
            'bStoredProcedure:=ACDeletePartyStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            '    m_lReturn = CreateEvent(r_lEventCnt:=lEventCnt, _
            'v_lPartyCnt:=PartyCnt, _
            'v_vInsuranceFolderCnt:=Null, _
            'v_vInsuranceFileCnt:=Null, _
            'v_vClaimCnt:=Null, _
            'v_vDocumentCnt:=Null, _
            'v_vOldAddressCnt:=Null, _
            'v_vNewAddressCnt:=Null, _
            'v_vCampaignId:=Null, _
            'v_vDocumentTypeId:=Null, _
            'v_vReportTypeId:=Null, _
            'v_lEventTypeId:=PMBEventDelClient, _
            'v_dtEventDate:=Now, _
            'v_vDescription:=Null)

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteParty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UndeleteParty
    '
    ' Description:
    '
    ' History: 14/01/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function UndeleteParty() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=gPMFunctions.ToSafeString(m_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            '    m_lReturn& = m_oDatabase.SQLAction(sSQL:=ACUndeletePartySQL, _
            'sSqlName:=ACUndeletePartyName, _
            'bStoredProcedure:=ACUndeletePartyStored)

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UndeleteParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UndeleteParty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPartyType
    '
    ' Description: Return the party type
    '
    '
    ' ***************************************************************** '
    Public Function GetPartyType(ByRef lPartyCnt As Integer, ByRef sPartyTypeText As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'What type of party/client is it
            '    sSQL$ = "SELECT party_cnt, '" & SIRPartyTypePersonalClient & _
            '"' FROM party_personal_client WHERE party_cnt = " & m_lPartyCnt & _
            '"UNION SELECT party_cnt, '" & SIRPartyTypeAgent & _
            '"' FROM party_agent WHERE party_cnt = " & m_lPartyCnt & _
            '"UNION SELECT party_cnt, '" & SIRPartyTypeCorporateClient & _
            '"' FROM party_corporate_client WHERE party_cnt = " & m_lPartyCnt & _
            '"UNION SELECT party_cnt, '" & SIRPartyTypeGroupClient & _
            '"' FROM party_group_client WHERE party_cnt = " & m_lPartyCnt

            '    sSQL$ = "SELECT '" & SIRPartyTypePersonalClientText & _
            '"' FROM party_personal_client WHERE party_cnt = " & lPartyCnt & _
            '"UNION SELECT '" & SIRPartyTypeAgentText & _
            '"' FROM party_agent WHERE party_cnt = " & lPartyCnt & _
            '"UNION SELECT '" & SIRPartyTypeCorporateClientText & _
            '"' FROM party_corporate_client WHERE party_cnt = " & lPartyCnt & _
            '"UNION SELECT '" & SIRPartyTypeGroupClientText & _
            '"' FROM party_group_client WHERE party_cnt = " & lPartyCnt

            sSQL = "SELECT t.code from party_type t, party p" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "where t.party_type_id = p.party_type_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "and p.party_cnt = " & gPMFunctions.ToSafeString(lPartyCnt) & Strings.ChrW(13) & Strings.ChrW(10)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GETPARTYTYPE", bStoredProcedure:=False, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then

                result = gPMConstants.PMEReturnCode.PMError
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Could not find party type for party_cnt " & lPartyCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyType", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result

            End If

            'Return the type

            sPartyTypeText = gPMFunctions.ToSafeString(vResultArray(0, 0)).Trim()

            vResultArray = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyTypeFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CalcCombinedKey
    '
    ' Description: This is a wrapper to the component services function
    ' to derive the Unique Invariant Key from source and entity ID.
    '
    ' ***************************************************************** '
    Public Function calccombinedkey(ByVal v_lSourceID As Integer, ByVal v_lKeyID As Integer, ByRef r_lCombinedKeyID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(gPMComponentServices.calccombinedkey(v_lSourceID:=v_lSourceID, v_lKeyID:=v_lKeyID, r_lCombinedKeyID:=r_lCombinedKeyID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CalcCombinedKeyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CalcCombinedKey", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetId (Public)
    '
    ' Description: Selects Policies by ID and populates the
    '              Base Details.
    '
    ' ***************************************************************** '
    Public Function GetID(ByRef lID As Integer) As Integer
        Return GetID(lID:=lID, vName:=Nothing, vShortName:=Nothing, vSourceId:=0)
    End Function

    Public Function GetID(ByRef lID As Integer, ByVal vName As Object) As Integer
        Return GetID(lID:=lID, vName:=vName, vShortName:=Nothing, vSourceId:=0)
    End Function

    Public Function GetID(ByRef lID As Integer, ByVal vName As Object, ByVal vShortName As Object) As Integer
        Return GetID(lID:=lID, vName:=vName, vShortName:=vShortName, vSourceId:=0)
    End Function

    Public Function GetID(ByRef lID As Integer, ByVal vName As Object, ByVal vShortName As Object, ByVal vSourceId As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Informations.IsNothing(vName) And Informations.IsNothing(vShortName) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                lID = -1
                Return result
            End If

            'set the source_id from the global property if parameter is missing

            If Informations.IsNothing(vSourceId) Then
                vSourceId = m_iSourceID
            End If

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            If Not Informations.IsNothing(vName) Then
                ' Add the name parameter (INPUT)
                m_lError = m_oDatabase.Parameters.Add(sName:="Party_Name", vValue:=gPMFunctions.ToSafeString(vName), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            ElseIf (Not Informations.IsNothing(vShortName)) Then
                ' Add the shortname parameter (INPUT)
                m_lError = m_oDatabase.Parameters.Add(sName:="Party_ShortName", vValue:=gPMFunctions.ToSafeString(vShortName), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                'else case
            End If

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetId")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' Add the source_id parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="Source_Id", vValue:=gPMFunctions.ToSafeString(vSourceId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetId")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' Add the party cnt parameter (OUTPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="Party_cnt", vValue:=gPMFunctions.ToSafeString(lID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetId")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            If Not Informations.IsNothing(vName) Then
                ' Execute SQL Statement
                '        m_lError& = m_oDatabase.SQLAction( _
                'sSQL:=ACPartyFromNameSQL, _
                'sSqlName:=ACPartyFromNameName, _
                'bStoredProcedure:=ACPartyFromNameStored, _
                'lrecordsAffected:=lRowsAffected)
            ElseIf (Not Informations.IsNothing(vShortName)) Then
                ' Execute SQL Statement
                '        m_lError& = m_oDatabase.SQLAction( _
                'sSQL:=ACPartyFromShortNameSQL, _
                'sSqlName:=ACPartyFromShortNameName, _
                'bStoredProcedure:=ACPartyFromShortNameStored, _
                'lrecordsAffected:=lRowsAffected)
            Else

            End If

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetId")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' Get the party_cnt of the record selected
            lID = m_oDatabase.Parameters.Item("party_cnt").Value

            If lID = -1 Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetId", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetName (Public)
    '
    ' Description: Selects the party name using the party ID.
    '
    ' ***************************************************************** '
    Public Function GetName(ByRef lPartyCnt As Integer, ByRef sPartyName As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=gPMFunctions.ToSafeString(lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetName")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the parameter (OUTPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="shortname", vValue:=gPMFunctions.ToSafeString(sPartyName), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetName")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            '    m_lError& = m_oDatabase.SQLAction( _
            'sSQL:=ACPartyFromCntSQL, _
            'sSqlName:=ACPartyFromCntName, _
            'bStoredProcedure:=ACPartyFromCntStored, _
            'lrecordsAffected:=lRowsAffected)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetName")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the party_cnt of the record selected
            sPartyName = m_oDatabase.Parameters.Item("shortname").Value.Trim()

            If sPartyName.Trim() = "" Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetName", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetResolvedName (Public)
    '
    ' Description: Selects the party ResolvedName using the party ID.
    '
    ' CT 17/08/00
    '
    ' ***************************************************************** '
    Public Function GetResolvedName(ByRef lPartyCnt As Integer, ByRef sPartyResolvedName As String) As Integer

        Dim result As Integer = 0
        Dim lRowsAffected As Integer
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            sSQL = "SELECT resolved_name "
            sSQL = sSQL & "FROM party "
            sSQL = sSQL & "WHERE party_cnt= {party_cnt}"

            ' Add the parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=gPMFunctions.ToSafeString(lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetResolvedName")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetResolvedName", bStoredProcedure:=gPMConstants.PMEReturnCode.PMFalse, lNumberRecords:=lRowsAffected, vResultArray:=vResultArray)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetResolvedName")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the resolved name of the record selected

            sPartyResolvedName = gPMFunctions.ToSafeString(vResultArray(0, 0))
            If sPartyResolvedName.Trim() = "" Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetResolvedName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetResolvedName", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetStructure
    '
    ' Description: Return the party structure
    '
    '
    ' ***************************************************************** '
    Public Function GetStructure(ByRef sStructure As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT code from party_structure" & Strings.ChrW(13) & Strings.ChrW(10)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetStructure", bStoredProcedure:=False, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then

                result = gPMConstants.PMEReturnCode.PMError
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Could not find structure", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStructure", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result

            End If

            'Return the structure

            sStructure = gPMFunctions.ToSafeString(vResultArray(0, 0)).Trim()

            m_sStructure = sStructure

            vResultArray = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetStructure Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStructure", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: CreateEvent (Private)
    '
    ' Description: Create an event record.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CreateEvent) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CreateEvent(ByRef r_lEventCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_vInsuranceFolderCnt As Object, ByVal v_vInsuranceFileCnt As Object, ByVal v_vClaimCnt As Object, ByVal v_vDocumentCnt As Object, ByVal v_vOldAddressCnt As Object, ByVal v_vNewAddressCnt As Object, ByVal v_vCampaignId As Object, ByVal v_vDocumentTypeId As Object, ByVal v_vReportTypeId As Object, ByVal v_lEventTypeId As Integer, ByVal v_dtEventDate As Date, ByVal v_vDescription As Object) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'If m_oEvent Is Nothing Then
    'm_oEvent = New bSIREvent.Business()
    '
    'm_lReturn = m_oEvent.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    'bPMFunc.LogMessage(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the event object", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
    '
    'Return result
    'End If
    'End If
    '
    'm_lReturn = m_oEvent.DirectAdd(vEventCnt:=r_lEventCnt, vPartyCnt:=v_lPartyCnt, vInsuranceFolderCnt:=v_vInsuranceFolderCnt, vInsuranceFileCnt:=v_vInsuranceFileCnt, vClaimCnt:=v_vClaimCnt, vDocumentCnt:=v_vDocumentCnt, vOldAddressCnt:=v_vOldAddressCnt, vNewAddressCnt:=v_vNewAddressCnt, vCampaignId:=v_vCampaignId, vDocumentType:=v_vDocumentTypeId, vReportType:=v_vReportTypeId, vEventType:=v_lEventTypeId, vUserid:=m_iUserID, vEventDate:=v_dtEventDate, vDescription:=v_vDescription)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    'bPMFunc.LogMessage(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the event", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
    '
    'Return result
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
    ' Log Error Message
    'bPMFunc.LogMessage(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: ClearParameters (Private)
    '
    ' Description: Clears the Database Parameters Collection if there
    '              are any.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (ClearParameters) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub ClearParameters()
    '
    'Try 
    '
    ' Clear the Databases Parameters Collection
    'If m_oDatabase.Parameters Is Nothing Then
    ' Do Nothing
    'Else
    'm_oDatabase.Parameters.Clear()
    'End If
    '
    '
    ' Added by Scalability Update Program - 30/07/2002
    'm_cInvariantKeys = New Collection()
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    ' Log Error Message
    'bPMFunc.LogMessage(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error Clear Parameters Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearParameters", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub

    ' ***************************************************************** '
    ' Name: UnderwritingOrAgency
    '
    ' Description:  Finds out if Underwriting or Agency business
    '
    ' History:
    ' 14/06/2002 SP - moved to uniform Product Options scheme
    ' ***************************************************************** '
    Private Function getUnderwritingOrAgency() As Integer

        Dim result As Integer = 0


        'getUnderwritingOrAgency = _
        ''gSIRLibrary.getUnderwritingOrAgency(m_sUnderwritingOrAgency)
        Dim vUnderwriting As Object = Nothing

        m_lReturn = CType(bPMFunc.getUnderwritingOrAgency(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, r_vUnderwriting:=vUnderwriting), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnderwritingOrAgencyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnderwritingOrAgency", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
        Else

        End If

        Return result

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

End Class

