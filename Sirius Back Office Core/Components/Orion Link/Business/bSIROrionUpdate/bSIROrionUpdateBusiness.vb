Option Strict Off
Option Explicit On
'Modified by Archana Tokas on 4/28/2010 10:23:28 AM refer developer guide no. 129
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 14/06/1998
    '
    ' SJP14062002 moved to uniform Product Options scheme and gSIRLibrary.bas
    ' RAW 21/02/2003 : ISS2379 : added Agent Group party type
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 08/12/2003
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************

    Private Const ACClass As String = "Business"
    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Database Class (Private)
    Private m_oACTAccount As bACTAccount.Form
    Private m_oSiriusDatabase As dPMDAO.Database
    'RDT 06/11/2006 - Must use a single DB connection for transactional database integrity
    'Private m_oOrionDatabase As dPMDAO.Database
    ' Close Database Flag (Private)
    Private m_bCloseSiriusDatabase As Boolean
    'Private m_bCloseOrionDatabase As Boolean

    ' Component Services object

    Private m_oAccount As Object
    ' Return value
    Private m_lReturn As Integer
    'Sirius Party Variables
    Private m_sPartyType As String = ""
    Private m_iSubBranchID As Integer
    Private m_lPartyId As Integer
    Private m_sShortName As String = ""
    Private m_sName As String = ""
    Private m_sContactName As String = ""
    'TR 11/06/03 - Added Logic for Commission Agents
    Private m_lPartyAgentType As Integer
    Private m_sAddress1 As String = ""
    Private m_sAddress2 As String = ""
    Private m_sAddress3 As String = ""
    Private m_sAddress4 As String = ""
    Private m_sPostalCode As String = ""
    Private m_iCountryId As Integer
    Private m_sPhoneAreaCode As String = ""
    Private m_sPhoneNumber As String = ""
    Private m_sPhoneExtension As String = ""
    Private m_sFaxAreaCode As String = ""
    Private m_sFaxNumber As String = ""
    Private m_sFaxExtension As String = ""
    Private m_lPaymentMethod As Integer

    'Orion Account Variables
    Private m_lAccountKey As Integer
    Private m_iAccountTypeID As Integer
    Private m_lLedgerID As Integer
    Private m_lNodeID As Integer
    Private m_lAccountId As Integer
    Private m_iAccountCurrencyId As Integer
    Private m_sAccountName As String = ""
    Private m_sAccountShortCode As String = ""
    Private m_sAccountAddress1 As String = ""
    Private m_sAccountAddress2 As String = ""
    Private m_sAccountAddress3 As String = ""
    Private m_sAccountAddress4 As String = ""
    Private m_sAccountPostalCode As String = ""
    Private m_iAccountCountryID As Integer
    Private m_sAccountPhoneAreaCode As String = ""
    Private m_sAccountPhoneNumber As String = ""
    Private m_sAccountPhoneExtension As String = ""
    Private m_sAccountFaxAreaCode As String = ""
    Private m_sAccountFaxNumber As String = ""
    Private m_sAccountFaxExtension As String = ""
    'RWH(02/05/01)
    Private m_sUnderwritingOrAgency As String = ""
    'sj 05/07/2002 - start
    Private m_bAsynchronousPosting As Boolean
    'sj 05/07/2002 - end

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
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

    'RWH(02/05/01)
    Public ReadOnly Property UnderwritingOrAgency() As String
        Get

            If m_sUnderwritingOrAgency = "" Then
                m_lReturn = getUnderwritingOrAgency()
            End If

            Return m_sUnderwritingOrAgency

        End Get
    End Property

    ' *********************** Public Functions ************************

    ' ***************************************************************** '
    ' Name: Initialise
    '
    ' Description: Standard initialise function.
    '
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


            Dim sValue As String = ""

            'SD 02/08/2002

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseSiriusDatabase, r_oCheckedDatabase:=m_oSiriusDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '     m_lReturn& = m_gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, _
            ''        v_lPMProductFamily:=pmePFOrion, _
            ''        r_bNewInstanceCreated:=m_bCloseOrionDatabase, _
            ''        r_oCheckedDatabase:=m_oSiriusDatabase)
            '

            'RDT 06/11/2006 - Must use a single DB connection for transactional database integrity
            ''SD 02/08/2002
            '    m_lReturn = gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, _
            ''        v_lPMProductFamily:=pmePFOrion, _
            ''        r_oDatabase:=m_oSiriusDatabase)
            '    If (m_lReturn <> PMTrue) Then
            '        Initialise = PMFalse
            '        Exit Function
            '    End If
            ' Set Username and Password

            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now
            'SD 02/08/2002

            m_oACTAccount = New bACTAccount.Form
            m_lReturn = m_oACTAccount.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oSiriusDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise bACTAccount.Form.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountIDs", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            'sj 05/07/2002 - start
            m_lReturn = bPMFunc.getProductOptionValue(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTAsynchronousPosting, v_vBranch:=m_iSourceID, r_vUnderwriting:=sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for Multi Branch Accounting", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sValue = "1" Then
                m_bAsynchronousPosting = True
            End If
            'sj 05/07/2002 - end

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate
    '
    ' Description: Standard terminate function.
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
                If m_oACTAccount IsNot Nothing Then
                    m_oACTAccount.Dispose()
                    m_oACTAccount = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    '
    ' Name: SiriusToOrion
    '
    ' Description: Updates Orion Account with Sirius Party Changes
    ' eck010900 Two new optional parameters required , if the source has been changed then
    '           party _id and cthe account key will need to be rebuild
    ' ***************************************************************** '
    Public Function SiriusToOrion(ByVal v_lPartyCnt As Integer, Optional ByVal v_iOldSourceId As Object = Nothing, Optional ByVal v_iOldPartyId As Object = Nothing) As Integer

        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        Try


            'sj 11/07/2002 - start
            'Multi Branch Accounting
            'If multi branch accounting is turned on then write out a record in the
            'accounts_party_queue table and exit
            If m_bAsynchronousPosting Then

                m_lReturn = AccountsPartyQueueAdd(v_lPartyCnt:=v_lPartyCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="AccountsPartyQueueAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SiriusToOrion")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else
                m_lReturn = SiriusToOrionBatch(v_lPartyCnt:=v_lPartyCnt, v_iOldSourceId:=v_iOldSourceId, v_iOldPartyId:=v_iOldPartyId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="SiriusToOrionBatch Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SiriusToOrion")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SiriusToOrion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SiriusToOrion", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: SiriusToOrionBatch
    '
    ' Description:
    '
    ' History: 05/07/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function SiriusToOrionBatch(ByVal v_lPartyCnt As Integer, Optional ByVal v_iOldSourceId As Object = Nothing, Optional ByVal v_iOldPartyId As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = GetSiriusData(v_lPartyCnt:=v_lPartyCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSiriusData Failed for party_cnt " & v_lPartyCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="SiriusToOrionBatch")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lAccountKey = v_lPartyCnt

            If Informations.IsNothing(v_iOldSourceId) Then
                m_lReturn = gPMComponentServices.calccombinedkey(v_lSourceID:=m_iSourceID, v_lKeyID:=m_lPartyId, r_lCombinedKeyID:=m_lAccountKey)
            Else


                m_lReturn = gPMComponentServices.calccombinedkey(v_lSourceID:=CInt(v_iOldSourceId), v_lKeyID:=CInt(v_iOldPartyId), r_lCombinedKeyID:=m_lAccountKey)
            End If

            'Account key is updated in procedure spe_party_upd.
            'This means that we need to use the current values rather than the old,
            'if the old account key can't be found.

            If Not Informations.IsNothing(v_iOldSourceId) Then
                m_lReturn = GetOrionAccount(m_lAccountKey)
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    m_lReturn = gPMComponentServices.calccombinedkey(v_lSourceID:=m_iSourceID, v_lKeyID:=m_lPartyId, r_lCombinedKeyID:=m_lAccountKey)

                    m_lReturn = GetOrionAccount(m_lAccountKey)
                End If
            Else
                m_lReturn = GetOrionAccount(m_lAccountKey)
            End If

            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMTrue
                    'Account exists
                    m_lReturn = UpdateOrionAccount(v_lPartyCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateOrionAccount Failed for party_cnt " & v_lPartyCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="SiriusToOrionBatch")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Case gPMConstants.PMEReturnCode.PMNotFound
                    'Create new account
                    m_lReturn = CreateOrionAccount(v_lPartyCnt:=v_lPartyCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateOrionAccount Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="SiriusToOrionBatch")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Case Else
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOrionAccount Failed for Account key " & m_lAccountKey, vApp:=ACApp, vClass:=ACClass, vMethod:="SiriusToOrionBatch")
                    Return gPMConstants.PMEReturnCode.PMFalse
            End Select


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=excep.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="SiriusToOrionBatch", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: GetSiriusData
    '
    ' Description: Gets data relative to the Party which is stored in
    '               the Orion Account
    '
    ' ***************************************************************** '
    Public Function GetSiriusData(ByRef v_lPartyCnt As Integer) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            Dim oParty As bSIRParty.Services
            'Use party services to get the main details
            'SD 02/08/2002

            oParty = New bSIRParty.Services
            m_lReturn = oParty.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oSiriusDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise bSIRParty.Services", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateOrionAccounts", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If


            ' RAW 21/02/2003 : ISS2379 : added
            If Not True Then
                result = gPMConstants.PMEReturnCode.PMError
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No Party ID supplied.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSiriusData")
                Return result
            End If


            With oParty


                .PartyCnt = v_lPartyCnt


                m_lReturn = .GetDetails()


                m_iSourceID = .SourceID

                m_iSubBranchID = .SubBranchId

                m_sShortName = .Shortname

                m_sPartyType = .SolutionPartyType

                m_lPartyId = .PartyID

                m_sName = .Name

                m_iCurrencyID = .CurrencyID

                If m_sPartyType = gSIRLibrary.SIRPartyTypeAgent Then
                    m_sContactName = .FirstName & " " & .ContactPerson
                Else
                    m_sContactName = .ResolvedName
                End If

                If Trim(m_sContactName) = "" Then
                    m_sContactName = .Name
                End If

                m_lPartyAgentType = .PartyAgentTypeID

                m_sAddress1 = .Address1

                m_sAddress2 = .Address2

                m_sAddress3 = .Address3

                m_sAddress4 = .Address4

                m_sPostalCode = .PostalCode
                'eck130601 Services now returns countryID
                '        m_iCountryId% = 0 '.AddressCountryID

                m_iCountryId = .CountryId


                m_sPhoneAreaCode = .AreaCode

                m_sPhoneNumber = .Number

                m_sPhoneExtension = .Extension

                m_sFaxAreaCode = ""
                m_sFaxNumber = ""
                m_sFaxExtension = ""


                m_lPaymentMethod = .PaymentMethod

            End With

            ' Destroy Party object

            oParty.Dispose()
            oParty = Nothing

            ' CTAF 091100 - If it's an agent, then we need to check if it's
            '               a sub-agent and if so, change the partytype (this sucks!)
            If m_sPartyType = gSIRLibrary.SIRPartyTypeAgent Then
                m_lReturn = CheckAgentType(v_lPartyCnt:=v_lPartyCnt, r_sPartyType:=m_sPartyType)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Party services gets phone details based on address/contact
            'usage - if this is not set get 1st telephone number available
            If m_sPhoneAreaCode = "" And m_sPhoneNumber = "" And m_sPhoneExtension = "" Then
                m_lReturn = GetPhoneData(v_lPartyCnt:=v_lPartyCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            'Party services doesn't get Fax details
            m_lReturn = GetFaxData(v_lPartyCnt:=v_lPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSiriusData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSiriusData", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetPartyData
    '
    ' Description: Gets the Party specific data  which is stored in
    '               Sirius
    '
    ' ***************************************************************** '
    Public Function GetPartyData(ByRef v_lPartyCnt As Integer) As gPMConstants.PMEReturnCode
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            With m_oSiriusDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                sSQL = ""
                sSQL = "SELECT p.source_id, p.party_id, t.code, p.currency_id, p.shortname, p.name "
                sSQL = sSQL & "FROM party P, party_type T "
                sSQL = sSQL & " WHERE p.party_type_id = t.party_type_id"
                sSQL = sSQL & " AND p.party_cnt = {party_cnt}"

                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetPartyDetails", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vResultArray)
            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_iSourceID = CInt(vResultArray(0, 0))

            m_lPartyId = CInt(vResultArray(1, 0))

            m_sPartyType = CStr(vResultArray(2, 0)).Trim()

            m_iCurrencyID = CInt(vResultArray(3, 0))

            m_sShortName = CStr(vResultArray(4, 0))

            m_sName = CStr(vResultArray(5, 0))


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyData", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetAddressData
    '
    ' Description: Gets the Address data  which is stored in
    '               Sirius
    '
    ' ***************************************************************** '
    Public Function GetAddressData(ByRef v_lPartyCnt As Integer) As gPMConstants.PMEReturnCode
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            With m_oSiriusDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                sSQL = ""
                sSQL = "SELECT a.address1, a.address2, a.address3, a.address4, a.postal_code, a.country_id "
                sSQL = sSQL & "FROM address a , party_address_usage u, address_usage_type t "
                sSQL = sSQL & "WHERE a.address_cnt = u.address_cnt "
                sSQL = sSQL & "AND u.address_usage_type_id = t.address_usage_type_id "
                sSQL = sSQL & "AND t.description = 'Correspondence Address' "
                sSQL = sSQL & "AND u.party_cnt = {party_cnt} "

                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetAddressDetails", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vResultArray)
            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then

                Return result
            End If

            m_sAddress1 = CStr(vResultArray(0, 0))

            m_sAddress2 = CStr(vResultArray(1, 0))

            m_sAddress3 = CStr(vResultArray(2, 0))

            m_sAddress4 = CStr(vResultArray(3, 0))

            m_sPostalCode = CStr(vResultArray(4, 0))

            m_iCountryId = CInt(vResultArray(5, 0))


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAddressData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAddressData", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetPhoneData
    '
    ' Description: Gets the Contact data  which is stored in
    '               Sirius
    '
    ' ***************************************************************** '
    Public Function GetPhoneData(ByRef v_lPartyCnt As Integer) As gPMConstants.PMEReturnCode
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            With m_oSiriusDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                sSQL = ""
                sSQL = "SELECT c.area_code, c.number, c.extension "
                sSQL = sSQL & "FROM contact c , party_contact_usage u, contact_type t "
                sSQL = sSQL & "WHERE c.contact_cnt = u.contact_cnt "
                sSQL = sSQL & "AND c.contact_type_id = t.contact_type_id "
                sSQL = sSQL & "AND t.code = 'TELEPHONE' "
                sSQL = sSQL & "AND u.party_cnt = {party_cnt} "

                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetContactDetails", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Informations.IsArray(vResultArray) Then

                    m_sPhoneAreaCode = CStr(vResultArray(0, 0))

                    m_sPhoneNumber = CStr(vResultArray(1, 0))

                    m_sPhoneExtension = CStr(vResultArray(2, 0))
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPhoneData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPhoneData", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetFaxData
    '
    ' Description: Gets the Contact data  which is stored in
    '               Sirius
    '
    ' ***************************************************************** '
    Public Function GetFaxData(ByRef v_lPartyCnt As Integer) As gPMConstants.PMEReturnCode
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            With m_oSiriusDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                sSQL = ""
                sSQL = "SELECT c.area_code, c.number, c.extension "
                sSQL = sSQL & "FROM contact c , party_contact_usage u, contact_type t "
                sSQL = sSQL & "WHERE c.contact_cnt = u.contact_cnt "
                sSQL = sSQL & "AND c.contact_type_id = t.contact_type_id "
                sSQL = sSQL & "AND t.code = 'FAX' "
                sSQL = sSQL & "AND u.party_cnt = {party_cnt} "

                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetFaxDetails", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Informations.IsArray(vResultArray) Then

                    m_sFaxAreaCode = CStr(vResultArray(0, 0))

                    m_sFaxNumber = CStr(vResultArray(1, 0))

                    m_sFaxExtension = CStr(vResultArray(2, 0))
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFaxData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFaxData", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetOrionAccount
    ' Description: Gets the Contact data  which is stored in
    '               Sirius
    '
    ' ***************************************************************** '
    Public Function GetOrionAccount(ByRef v_lAccountKey As Integer) As gPMConstants.PMEReturnCode


        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            With m_oSiriusDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="account_key", vValue:=CStr(v_lAccountKey), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                sSQL = ""
                sSQL = "SELECT a.account_id  "
                sSQL = sSQL & "FROM Account a "
                sSQL = sSQL & "WHERE a.account_key = {account_key}"

                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetAccountDetails", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vResultArray)
            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            m_lAccountId = CInt(vResultArray(0, 0))


            m_lReturn = m_oACTAccount.GetDetails(vAccountID:=m_lAccountId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOrionAccount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOrionAccount", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CreateAccount
    '
    ' Description: Creates an account on the orion database with the
    '              passed short code.
    '
    ' ***************************************************************** '
    Public Function CreateAccount() As Integer


        Dim result As Integer = 0
        Return gPMConstants.PMEReturnCode.PMTrue



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateAccount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAccount", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: UpdateOrionAccount
    '
    ' Description: Updates the account on the orion database with
    '
    ' ***************************************************************** '
    'sj 05/07/2002 - Changed to private
    Private Function UpdateOrionAccount(ByVal v_lPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        'eck010900 recalculate accountkey
        '   SJP 04072002 SP - Account Key is now Party Count
        m_lAccountKey = v_lPartyCnt
        'SD 02/08/2002
        m_lReturn = gPMComponentServices.calccombinedkey(v_lSourceID:=m_iSourceID, v_lKeyID:=m_lPartyId, r_lCombinedKeyID:=m_lAccountKey)

        'MKW170603 PN4457 START - Change Nodes, AccountTypeID and ledgerId for agent/subagent

        m_lReturn = m_oACTAccount.EditUpdate(lRow:=1, vAccountID:=m_lAccountId, vCurrencyID:=m_iCurrencyID, vAccountName:=m_sName, vShortCode:=m_sShortName, vAddress1:=m_sAddress1, vAddress2:=m_sAddress2, vAddress3:=m_sAddress3, vAddress4:=m_sAddress4, vPostalCode:=m_sPostalCode, vAddressCountry:=m_iCountryId, vPhoneAreaCode:=m_sPhoneAreaCode, vPhoneNumber:=m_sPhoneNumber, vPhoneExtension:=m_sPhoneExtension, vFaxAreaCode:=m_sFaxAreaCode, vFaxNumber:=m_sFaxNumber, vFaxExtension:=m_sFaxExtension, vPartySourceID:=m_iSourceID, vAccountKey:=m_lAccountKey, vContactName:=m_sContactName, vPaymenttypeID:=m_lPaymentMethod, vSubBranchID:=m_iSubBranchID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to update the details
            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the Orion Account", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateOrionACcount")
        End If



        m_lReturn = m_oACTAccount.Update()

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to update the details
            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the Orion Account details", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateOrionAccount")
        End If

        Return result



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateOrionAccount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateOrionAccount", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: CreateOrionAccount (Public)
    '
    ' Description:
    ' ***************************************************************** '
    Public Function CreateOrionAccount(Optional ByVal v_lPartyCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim oOrionExplorer As bACTExplorer.Form
        Dim lElementID As Integer

        Dim sPostingType As String = String.Empty

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Validate incoming parameters
            If v_lPartyCnt <= 0 Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="An Account cannot be created without a PartyCnt ", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateOrionAccount")
                Return result
            End If

            m_lAccountKey = v_lPartyCnt

            ' Create an instance of the Orion Account Explorer object

            oOrionExplorer = New bACTExplorer.Form
            m_lReturn = oOrionExplorer.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oSiriusDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise bACTExplorer.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateOrionAccount", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            ' Add new ledger accounts

            Select Case m_sPartyType.ToUpper()
                Case gSIRLibrary.SIRPartyTypeCommission
                    'Get Commission LedgerID
                    m_lReturn = GetLedgerDetails(r_lCommissionLedgerId:=m_lLedgerID, r_lCommissionLedgerNodeId:=m_lNodeID)
                    m_iAccountTypeID = ACTConst.ACTAccountTypeLiability

                Case gSIRLibrary.SIRPartyTypeInsurer
                    'Get Insurer LedgerID
                    m_lReturn = GetLedgerDetails(r_lInsurerLedgerId:=m_lLedgerID, r_lInsurerLedgerNodeId:=m_lNodeID)
                    m_iAccountTypeID = ACTConst.ACTAccountTypeLiability

                    'Agents and Commission Agents (Agent types 1 and 3) added here
                Case gSIRLibrary.SIRPartyTypeAgent, gSIRLibrary.SIRPartyTypeAgentGroup
                    'Broking Agents dealt with differently to Underwriting
                    'TR - Underwriting - Agents have asset accounts except
                    'for Commission Agents, which have liability ones
                    Select Case m_lPartyAgentType
                        Case 3 'Commission Agents
                            'Get Commission LedgerID for Commissions agents
                            m_lReturn = GetLedgerDetails(r_lCommissionLedgerId:=m_lLedgerID, r_lCommissionLedgerNodeId:=m_lNodeID)
                            m_iAccountTypeID = ACTConst.ACTAccountTypeLiability

                        Case Else 'Regular Agents
                            'Get Agent LedgerID
                            m_lReturn = GetLedgerDetails(r_lAgentLedgerId:=m_lLedgerID, r_lAgentLedgerNodeId:=m_lNodeID)
                            m_iAccountTypeID = ACTConst.ACTAccountTypeAsset
                    End Select


                    'Sub Agents (Agent type 2) added here
                Case "UB"
                    'Sub Agents have an Asset acc in SFB and a Liability Acc in SFU
                    'Stick Sub Agent Acc into Liability LedgerID
                    m_lReturn = GetLedgerDetails(r_lSubAgentLedgerId:=m_lLedgerID, r_lSubAgentLedgerNodeId:=m_lNodeID)
                    m_iAccountTypeID = ACTConst.ACTAccountTypeLiability


                    'DC141204 -added for introducer agent type
                Case "TR"
                    'Introducers have an Asset acc in SFB and a Liability Acc in SFU

                Case gSIRLibrary.SIRPartyTypeFee
                    ' Get Fee LedgerID
                    m_lReturn = GetLedgerDetails(r_lFeeLedgerId:=m_lLedgerID, r_lFeeLedgerNodeId:=m_lNodeID)
                    m_iAccountTypeID = ACTConst.ACTAccountTypeAsset

                Case gSIRLibrary.SIRPartyTypeExtra
                    'Get Insurer LedgerID
                    m_lReturn = GetLedgerDetails(r_lInsurerLedgerId:=m_lLedgerID, r_lInsurerLedgerNodeId:=m_lNodeID)
                    m_iAccountTypeID = ACTConst.ACTAccountTypeLiability

                Case gSIRLibrary.SIRPartyTypeDiscount
                    'Get Discount LedgerID
                    m_lReturn = GetLedgerDetails(r_lDiscountLedgerId:=m_lLedgerID, r_lDiscountLedgerNodeId:=m_lNodeID)
                    'DJM 02/09/2003 : Default the account type to Income.
                    m_iAccountTypeID = ACTConst.ACTAccountTypeIncome

                Case gSIRLibrary.SIRPartyTypeFinanceProvider
                    ' Get Premium Finance LedgerID
                    m_lReturn = GetLedgerDetails(r_lPremiumFinanceLedgerId:=m_lLedgerID, r_lPremiumFinanceLedgerNodeId:=m_lNodeID)
                    'DC310505 PN20780 changed from liability to asset
                    m_iAccountTypeID = ACTConst.ACTAccountTypeAsset

                Case Else
                    'Deal with 'Other' party types.

                    If m_sPartyType.ToUpper().Substring(0, 2) = gSIRLibrary.SIRPartyTypeOther Then
                        m_lReturn = GetOtherPartyPostingType(v_lPartyCnt:=v_lPartyCnt, r_sPostingType:=sPostingType)

                        If sPostingType = "PAYABLE" Then
                            ' Get Other Party Payable LedgerID
                            m_lReturn = GetLedgerDetails(r_lOtherPartyPayLedgerId:=m_lLedgerID, r_lOtherPartyPayLedgerNodeId:=m_lNodeID)
                            m_iAccountTypeID = ACTConst.ACTAccountTypeLiability
                        Else
                            ' Get Other Party Receivable LedgerID
                            m_lReturn = GetLedgerDetails(r_lOtherPartyRecLedgerId:=m_lLedgerID, r_lOtherPartyRecLedgerNodeId:=m_lNodeID)
                            m_iAccountTypeID = ACTConst.ACTAccountTypeAsset
                        End If
                    Else
                        'Default to Sales LedgerID
                        m_lReturn = GetLedgerDetails(r_lSalesLedgerID:=m_lLedgerID, r_lSalesLedgerNodeID:=m_lNodeID)
                        m_iAccountTypeID = ACTConst.ACTAccountTypeAsset
                    End If

            End Select

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get LedgerDetails for party type " & m_sPartyType, vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If


            m_lReturn = m_oACTAccount.DirectAdd(vAccountID:=m_lAccountId, vPurgefrequencyID:=ACTConst.ACTPurgeFreqNever, vCurrencyID:=m_iCurrencyID, vAccounttypeID:=m_iAccountTypeID, vLedgerId:=m_lLedgerID, vAccountName:=m_sName, vShortCode:=m_sShortName, vRestrictEnquiry:=False, vRestrictUpdate:=False, vDeleteAtPurge:=False, vContactName:=m_sContactName, vAddress1:=m_sAddress1, vAddress2:=m_sAddress2, vAddress3:=m_sAddress3, vAddress4:=m_sAddress4, vPostalCode:=m_sPostalCode, vAddressCountry:=m_iCountryId, vPhoneAreaCode:=m_sPhoneAreaCode, vPhoneNumber:=m_sPhoneNumber, vPhoneExtension:=m_sPhoneExtension, vFaxAreaCode:=m_sFaxAreaCode, vFaxNumber:=m_sFaxNumber, vFaxExtension:=m_sFaxExtension, vAccountKey:=m_lAccountKey, vAccountStatusID:=ACTConst.ACTAccountStatusActive, vPartySourceID:=m_iSourceID, vPaymenttypeID:=m_lPaymentMethod)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Destroy the Orion object

                m_oACTAccount.Dispose()

                m_oACTAccount = Nothing

                Return result
            End If


            m_oACTAccount.Dispose()
            m_oACTAccount = Nothing

            ' Insert new element

            lElementID = oOrionExplorer.InsertElement(m_sShortName)

            If lElementID > 0 And m_lNodeID > 0 Then

                m_lNodeID = oOrionExplorer.InsertNode(lParentNodeId:=m_lNodeID, lElementId:=lElementID, vAccountID:=m_lAccountId)
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            oOrionExplorer.Dispose()
            oOrionExplorer = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateOrionAccount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetLedgerIDs (Public)
    '
    ' Description: Gets LedgerID from Orion
    '               RWH(23/07/01) Added stuff for other party ledgers.
    '               DC 14/12/04 added introducer ledger
    ' ***************************************************************** '
    Public Function GetLedgerDetails(Optional ByRef r_lSalesLedgerID As Integer = 0, Optional ByRef r_lSalesLedgerNodeID As Integer = 0, Optional ByRef r_lPurchaseLedgerID As Integer = 0, Optional ByRef r_lPurchaseLedgerNodeID As Integer = 0, Optional ByRef r_lInsurerLedgerId As Integer = 0, Optional ByRef r_lInsurerLedgerNodeId As Integer = 0, Optional ByRef r_lAgentLedgerId As Integer = 0, Optional ByRef r_lAgentLedgerNodeId As Integer = 0, Optional ByRef r_lFeeLedgerId As Integer = 0, Optional ByRef r_lFeeLedgerNodeId As Integer = 0, Optional ByRef r_lCommissionLedgerId As Integer = 0, Optional ByRef r_lCommissionLedgerNodeId As Integer = 0, Optional ByRef r_lDiscountLedgerId As Integer = 0, Optional ByRef r_lDiscountLedgerNodeId As Integer = 0, Optional ByRef r_lPremiumFinanceLedgerId As Integer = 0, Optional ByRef r_lPremiumFinanceLedgerNodeId As Integer = 0, Optional ByRef r_lSubAgentLedgerId As Integer = 0, Optional ByRef r_lSubAgentLedgerNodeId As Integer = 0, Optional ByRef r_lOtherPartyRecLedgerId As Integer = 0, Optional ByRef r_lOtherPartyRecLedgerNodeId As Integer = 0, Optional ByRef r_lOtherPartyPayLedgerId As Integer = 0, Optional ByRef r_lOtherPartyPayLedgerNodeId As Integer = 0, Optional ByRef r_lIntroducerLedgerId As Integer = 0, Optional ByRef r_lIntroducerLedgerNodeId As Integer = 0) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim oACTLedger As bACTLedger.Form
        Dim lLedgerID, lNodeID As Integer
        Dim sLedgerShortName As String = String.Empty
        Dim sLedgerName As String = String.Empty
        Dim r_lPartyCnt, r_lSubBranchID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create instance of Orion Account Ledger class

            oACTLedger = New bACTLedger.Form
            m_lReturn = oACTLedger.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oSiriusDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise the Account Ledger object.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLedgerDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oACTLedger.GetDetails()


            For iCount As Integer = 1 To oACTLedger.RecordCount


                m_lReturn = oACTLedger.GetNext(vLedgerID:=lLedgerID, vLedgerShortName:=sLedgerShortName, vLedgerName:=sLedgerName)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Ledger details.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLedgerDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                End If
                Select Case sLedgerShortName.Substring(0, 1)
                    Case gSIRLibrary.SIRACTSalesLedgerShortName
                        If Not False Then
                            r_lSalesLedgerID = lLedgerID
                        End If
                    Case gSIRLibrary.SIRACTPurchaseLedgerShortName
                        If Not False Then
                            r_lPurchaseLedgerID = lLedgerID
                        End If
                    Case gSIRLibrary.SIRACTInsurerLedgerShortName
                        If Not False Then
                            r_lInsurerLedgerId = lLedgerID
                        End If
                    Case gSIRLibrary.SIRACTAgentLedgerShortName
                        If Not False Then
                            r_lAgentLedgerId = lLedgerID
                        End If
                    Case gSIRLibrary.SIRACTFeeLedgerShortName
                        If Not False Then
                            r_lFeeLedgerId = lLedgerID
                        End If
                    Case gSIRLibrary.SIRACTCommissionLedgerShortName
                        If Not False Then
                            r_lCommissionLedgerId = lLedgerID
                        End If
                    Case gSIRLibrary.SIRACTDiscountLedgerShortName
                        If Not False Then
                            r_lDiscountLedgerId = lLedgerID
                        End If
                    Case gSIRLibrary.SIRACTPremiumFinanceLedgerShortName
                        If Not False Then
                            r_lPremiumFinanceLedgerId = lLedgerID
                        End If
                    Case gSIRLibrary.SIRACTSubAgentLedgerShortName
                        If Not False Then
                            r_lSubAgentLedgerId = lLedgerID
                        End If
                        'DC141204 - added introducer ledger
                    Case gSIRLibrary.SIRACTIntroducerLedgerShortname
                        If Not False Then
                            r_lIntroducerLedgerId = lLedgerID
                        End If

                    Case Else
                        'RWH(23/07/01) Other Party stuff.

                        Select Case sLedgerShortName.Trim()
                            Case gSIRLibrary.SIRACTOtherPartyRecLedgerShortName
                                If Not False Then
                                    r_lOtherPartyRecLedgerId = lLedgerID
                                End If

                            Case gSIRLibrary.SIRACTOtherPartyPayLedgerShortName
                                If Not False Then
                                    r_lOtherPartyPayLedgerId = lLedgerID
                                End If

                        End Select


                End Select

                lNodeID = 0
                'eck310800 Get Node
                '        m_lReturn = oACTLedger.GetLedgerNodeId(sLedgerName, lNodeID)

                'RKC 21/08/2002
                'Need to Obtain SubBranch Id
                'Obtain PartyCnt First
                m_lReturn = GetPartyCntFromShortName(v_sShortname:=m_sShortName, r_lPartyCnt:=r_lPartyCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetPartyCnt for Get Ledger details.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLedgerDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Get SubBranch Id from PartyCnt
                m_lReturn = GetSubBranchID(m_oSiriusDatabase, r_lSubBranchID:=r_lSubBranchID, v_vPartyCnt:=CStr(r_lPartyCnt))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetSubBranchID for Get Ledger details.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLedgerDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = oACTLedger.GetNodeFromLedger(sLedgerName, r_lSubBranchID, lNodeID)

                Select Case sLedgerShortName.Substring(0, 1)
                    Case gSIRLibrary.SIRACTSalesLedgerShortName
                        If Not False Then
                            r_lSalesLedgerNodeID = lNodeID
                        End If
                    Case gSIRLibrary.SIRACTPurchaseLedgerShortName
                        If Not False Then
                            r_lPurchaseLedgerNodeID = lNodeID
                        End If
                    Case gSIRLibrary.SIRACTInsurerLedgerShortName
                        If Not False Then
                            r_lInsurerLedgerNodeId = lNodeID
                        End If
                    Case gSIRLibrary.SIRACTAgentLedgerShortName
                        If Not False Then
                            r_lAgentLedgerNodeId = lNodeID
                        End If
                    Case gSIRLibrary.SIRACTFeeLedgerShortName
                        If Not False Then
                            r_lFeeLedgerNodeId = lNodeID
                        End If
                    Case gSIRLibrary.SIRACTCommissionLedgerShortName
                        If Not False Then
                            r_lCommissionLedgerNodeId = lNodeID
                        End If
                    Case gSIRLibrary.SIRACTDiscountLedgerShortName
                        If Not False Then
                            r_lDiscountLedgerNodeId = lNodeID
                        End If
                    Case gSIRLibrary.SIRACTPremiumFinanceLedgerShortName
                        If Not False Then
                            r_lPremiumFinanceLedgerNodeId = lNodeID
                        End If
                    Case gSIRLibrary.SIRACTSubAgentLedgerShortName
                        If Not False Then
                            r_lSubAgentLedgerNodeId = lNodeID
                        End If
                        'DC141204 -added check for introducer ledger
                    Case gSIRLibrary.SIRACTIntroducerLedgerShortname
                        If Not False Then
                            r_lIntroducerLedgerNodeId = lNodeID
                        End If

                    Case Else
                        'RWH(23/07/01) Other Party stuff.

                        Select Case sLedgerShortName.Trim()
                            Case gSIRLibrary.SIRACTOtherPartyRecLedgerShortName
                                If Not False Then
                                    r_lOtherPartyRecLedgerNodeId = lNodeID
                                End If

                            Case gSIRLibrary.SIRACTOtherPartyPayLedgerShortName
                                If Not False Then
                                    r_lOtherPartyPayLedgerNodeId = lNodeID
                                End If

                        End Select

                End Select

            Next iCount


            oACTLedger.Dispose()
            oACTLedger = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetLedgerDetails.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLedgerDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckAgentType
    '
    ' Description:
    '
    ' History: 09/11/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function CheckAgentType(ByVal v_lPartyCnt As Integer, ByRef r_sPartyType As String) As Integer

        Dim result As Integer = 0
        Dim oPartyAG As bSIRPartyAG.Business
        Dim lTypeID As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Get an instance of PartyAG

        oPartyAG = New bSIRPartyAG.Business
        m_lReturn = oPartyAG.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        ' Get the record

        m_lReturn = oPartyAG.GetDetails(vPartyCnt:=v_lPartyCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckAgentType", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Read the details

        m_lReturn = oPartyAG.GetNext(vPartyCnt:=v_lPartyCnt, vPartyAgentTypeID:=lTypeID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetNext", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckAgentType", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Change the party type if its a sub-agent
        If lTypeID = 2 Then
            r_sPartyType = "UB" ' its a silent s in sub-agent apparently...
        End If


        ' Clear up

        oPartyAG.Dispose()

        oPartyAG = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetUnderwritingOrAgency
    '
    ' Description:  Finds out if Underwriting or Agency business
    '
    ' RWH(07/02/2001) (U/W hidden option)
    ' 06/06/2002 SP - Modified to use global library
    ' ***************************************************************** '
    Private Function getUnderwritingOrAgency() As Integer

        Dim result As Integer = 0


        'Retrieves the value from the class

        Return bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_sUnderwritingOrAgency)

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetOtherPartyPostingType
    '
    ' Description:
    '
    ' History: 23/07/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function GetOtherPartyPostingType(ByVal v_lPartyCnt As Integer, ByRef r_sPostingType As String) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        sSQL = "SELECT popt.code" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "FROM Party p, party_type pt, party_other_posting_type popt" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "WHERE p.party_cnt = " & CStr(v_lPartyCnt) & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "AND pt.party_type_id = p.party_type_id" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "AND popt.party_other_posting_type_id = pt.party_other_posting_type_id"

        m_oSiriusDatabase.Parameters.Clear()

        m_lReturn = m_oSiriusDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetPartyOtherPostingType", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsArray(vResultArray) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        r_sPostingType = CStr(vResultArray(0, 0)).Trim().ToUpper()

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: AccountsPartyQueueAdd
    '
    ' Description:
    '
    ' History: 05/07/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function AccountsPartyQueueAdd(ByVal v_lPartyCnt As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue
        'Modified by Deepak Sharma on 5/13/2010 12:36:59 PM refer developer guide no. 39(Guide)
        'Const kAddAccountsPartyQueueSQL As String = "{call spu_accounts_party_queue_add (?,?,?,?)}"
        Const kAddAccountsPartyQueueSQL As String = "spu_accounts_party_queue_add"
        Const kAddAccountsPartyQueueName As String = "AddAccountsPartyQueue"
        With m_oSiriusDatabase

            .Parameters.Clear()

            'Party_cnt
            m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Create_date
            m_lReturn = .Parameters.Add(sName:="create_date", vValue:=Informations.FormatDateTime(DateTime.Now), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'commit_ind
            m_lReturn = .Parameters.Add(sName:="commit_ind", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'commit_date

            'Modified by Archana Tokas on 4/28/2010 10:53:31 AM refer developer guide no. 85
            'm_lReturn = .Parameters.Add(sName:="commit_date", vValue:=CStr(DBNull.Value), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            m_lReturn = .Parameters.Add(sName:="commit_date", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .SQLAction(sSQL:=kAddAccountsPartyQueueSQL, sSQLName:=kAddAccountsPartyQueueName, bStoredProcedure:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetPartyCntFromShortName
    '
    ' Description:
    '
    ' History: 21/08/2002 RKC - Created.
    '
    ' ***************************************************************** '
    Private Function GetPartyCntFromShortName(ByVal v_sShortname As String, ByRef r_lPartyCnt As Integer) As Integer


        Dim result As Integer = 0
        Dim oParty As bSIRParty.Business





        result = gPMConstants.PMEReturnCode.PMTrue


        ' Get a new instance of component services

        ' Create the object

        oParty = New bSIRParty.Business
        m_lReturn = oParty.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oSiriusDatabase)

        ' Remove it


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = oParty.GetPartyCnt(vPartyRef:=v_sShortname, vPartyCnt:=r_lPartyCnt)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        oParty.Dispose()


        Return result

    End Function

    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: GetSubBranchID
    ' PURPOSE: Returns the SubBranchID for different areas of Orion. Used by
    ' multi-branch accounting to determine the correct period in which a
    ' record will fall.
    ' ORIGINAL AUTHOR: Danny Davis
    ' DATE: 05/08/2002, 11:27
    ' AUTHOR: Raj Chanian: Reused Locally
    ' DATE: 21/08/2002
    ' RETURNS: PMTrue for success
    ' CHANGES:
    ' ---------------------------------------------------------------------------
    'Public Function GetSubBranchID(ByVal v_oDatabase As dPMDAO.Database, ByRef r_lSubBranchID As Integer, Optional ByVal v_vAccountID As String = VariantType.Null, Optional ByVal v_vTransDetailID As String = VariantType.Null, Optional ByVal v_vPeriodID As String = VariantType.Null, Optional ByVal v_vBankAccountID As String = VariantType.Null, Optional ByVal v_vPartyCnt As String = VariantType.Null) As Integer
    Public Function GetSubBranchID(ByVal v_oDatabase As dPMDAO.Database, ByRef r_lSubBranchID As Integer, Optional ByVal v_vAccountID As Object = Nothing, Optional ByVal v_vTransDetailID As Object = Nothing, Optional ByVal v_vPeriodID As Object = Nothing, Optional ByVal v_vBankAccountID As Object = Nothing, Optional ByVal v_vPartyCnt As Object = Nothing) As Integer

        Dim result As Integer = 0

        Try

            With v_oDatabase
                .Parameters.Clear()

                'Modified by Archana Tokas on 4/28/2010 10:37:30 AM refer developer guide no. 85
                '.Parameters.Add("sub_branch_id", CStr(DBNull.Value), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add("sub_branch_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add("account_id", v_vAccountID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add("transdetail_id", v_vTransDetailID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add("period_id", v_vPeriodID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add("bankaccount_id", v_vBankAccountID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add("party_cnt", v_vPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                'Modified by Deepak Sharma on 5/13/2010 12:36:59 PM refer developer guide no. 39(Guide)
                'result = .SQLAction("{call spu_ACT_Get_Sub_Branch_id (?,?,?,?,?,?)}", "Get Sub Branch ID", True)
                result = .SQLAction("spu_ACT_Get_Sub_Branch_id", "Get Sub Branch ID", True)
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                Else
                    r_lSubBranchID = gPMFunctions.NullToLong(.Parameters.Item("sub_branch_id").Value)
                End If
            End With

            Return result

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSubBranchID", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
            End Select

        Finally

        End Try
        Return result
    End Function
End Class

