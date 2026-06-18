Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
Imports System.Text
'Developer Guide No. 129
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 12/10/1998
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a SIRPartyAG.
    '
    ' Edit History:
    ' SP191198 - Call to UpdateGemini should be in bSIRPartyAG not
    ' bSIRParty
    ' SJP14062002 - getUnderWritingOrAgency uses new product options scheme
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 27/11/2003
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

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Collection of SIRPartyAGs (Private)
    Private m_oSIRPartyAGs As New bSIRPartyAG.SIRPartyAGs

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

    ' Primary Keys to work with
    Private m_lPartyCnt As Integer

    'TN20001711
    Private m_sUnderwritingOrAgency As String = ""

    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business
    Private m_vParamArrayForParty As Object
    ' PW160702
    'Developer Guide No. 101
    Private Const ACPartyRelationshipGroupAgent As Object = 2

    Private m_oSIRParty As Object ' bSIRParty.Business


    Private Enum AgentDetails
        ADCode = 0
        ADBranchID = 1
        ADBranchName = 2
        ADSchemeName = 3
        ADAgentName = 4
        ADAddress1 = 5
        ADAddress2 = 6
        ADAddress3 = 7
        ADAddress4 = 8
        ADPostcode = 9
        ADTelephone = 10
        ADUserName = 11
        ADPassword = 12
    End Enum

    Dim m_sAgentCode As String = ""
    Dim m_iAgentBranchID As Integer
    Dim m_sBranchName As String = ""
    Dim m_sSchemeName As String = ""
    Dim m_sAgentName As String = ""
    Dim m_sAddress1 As String = ""
    Dim m_sAddress2 As String = ""
    Dim m_sAddress3 As String = ""
    Dim m_sAddress4 As String = ""
    Dim m_sPostCode As String = ""
    Dim m_sTelephone As String = ""
    Dim m_sAgentUserName As String = ""
    Dim m_sAgentPassword As String = ""

    Private m_iPartyOriginId As Integer
    Private m_iPartyTypeID As Integer
    Private m_iAddressUsageTypeID As Integer
    Private m_iContactTypeId As Integer

    Private m_iAgentCurrencyID As Integer
    Private m_iAgentCountryId As Integer

    Private m_oPartyAGAddress As bSIRAddress.Business
    Private m_oPartyContact As bSIRContact.Business
    Private m_oPMUser As bPMUser.Business
    Private m_bImportData As Boolean


    Public Function GetCorrespondenceTypes(ByRef vCorrespondenceTypes As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add the parameters
            m_oDatabase.Parameters.Clear()
            'developer guide no. 40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Effective_Date", vValue:=DateTime.Today, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Language_ID", vValue:=CStr(m_iLanguageID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            sSQL = "SELECT ct.contact_type_id, cap.caption, ct.code " & _
                   "FROM contact_type ct, pmcaption cap " & _
                   "WHERE ct.is_deleted = 0 " & _
                   "AND ct.is_correspondence_type = 1 " & _
                   "AND ct.effective_date <= {Effective_Date} " & _
                   "AND ct.caption_id = cap.caption_id " & _
                   "AND cap.language_id = {Language_ID}"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GETCORRESPONDTYPES", bStoredProcedure:=False, lNumberRecords:=500, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'return the data



            vCorrespondenceTypes = vResultArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetCorrespondenceTypesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCorrespondenceTypes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' PRIVATE Data Members (End)

    ' ***************************************************************** '
    ' Name: CheckReference (Public)
    '
    ' Description: Checks if the passed shortname (reference) already exists
    '
    ' ***************************************************************** '
    Public Function CheckReference(ByRef sReference As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CheckReference"

        Dim oParty As Object ' bSIRParty.Business
        Dim bExists As Object = False

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Create an instance of the main party business object
            'oParty = New bSIRParty.Business()
            oParty = Nothing
            If oParty Is Nothing Then
                result = gPMComponentServices.CreateBusinessObject(r_oObject:=oParty, v_sClassName:="bSIRParty.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    Dim r_sMessage As String = "Failed to create an instance of bSIRParty.Business"
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ReprocessClaim", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End If
            m_lReturn = oParty.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ACApp, vDatabase:=CType(m_oDatabase, dPMDAO.Database))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oParty.Initialise", "oParty = bSIRParty.Business", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Pass in the account code to see if it already exists
            m_lReturn = oParty.CheckIfAccountCodeExists(v_sAccountCode:=sReference, r_bExists:=bExists)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oParty.CheckIfAccountCodeExists", "v_sAccountCode:=" & sReference, gPMConstants.PMELogLevel.PMLogError)
            End If

            'If the Reference was found then return empty string
            If bExists Then
                sReference = ""
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here

            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public Property CurrentRecord() As Integer
        Get

            Return m_lCurrentRecord

        End Get
        Set(ByVal Value As Integer)

            Select Case Value
                Case Is < 1
                    m_lCurrentRecord = 0
                Case Is > m_oSIRPartyAGs.Count()
                    m_lCurrentRecord = m_oSIRPartyAGs.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Number in Collection
            Return m_oSIRPartyAGs.Count()

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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

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


            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '***************************************

            ' m_oSIRParty = New bSIRParty.Business
            m_oSIRParty = Nothing
            If m_oSIRParty Is Nothing Then
                result = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oSIRParty, v_sClassName:="bSIRParty.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    Dim r_sMessage As String = "Failed to create an instance of bSIRParty.Business"
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ReprocessClaim", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End If

            m_lReturn = m_oSIRParty.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ACApp, vDatabase:=CType(m_oDatabase, dPMDAO.Database))


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '***************************************


            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create SIRPartyAGs Collection
            m_oSIRPartyAGs = New bSIRPartyAG.SIRPartyAGs()

            ' Create PM Lookup Business Object
            m_oLookup = New BPMLOOKUP.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

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
            Me.disposedValue = True
            If disposing Then
                If m_oLookup IsNot Nothing Then
                    m_oLookup.Dispose()
                End If
                m_oLookup = Nothing
                m_oSIRParty = Nothing
                m_oSIRPartyAGs = Nothing
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

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a SIRPartyAG.
    '
    '
    ' ***************************************************************** '
    'Developer Guide No. 17
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim oSIRpartyAG As bSIRPartyAG.SIRPartyAG = Nothing
        Dim dtEffectiveDate As Date

        Dim vTabArray(4, 7) As Object
        Dim vPartyAgentOriginID As Object = Nothing
        Dim vPaymentMethod As Object = Nothing
        Dim vPaymentFrequency As Object = Nothing
        Dim vAddressOnNotice As Object = Nothing
        Dim vRenewalStopCode As Object = Nothing
        Dim vPaymentTermCode As Object = Nothing
        Dim vAgentStatus As Integer = 0
        'SAGICOR WPR14
        Dim vCommissionlevel As Object = Nothing
        ' {* USER DEFINED CODE (End) *}

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = gSIRLibrary.SIRLookupPartyAgentOrigin
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 1) = "MediaType"
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 2) = gSIRLibrary.SIRLookupPaymentFrequency
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 3) = gSIRLibrary.SIRLookupAddressOnNotice
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 4) = gSIRLibrary.SIRLookupRenewalStopCode
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 5) = gSIRLibrary.SIRLookupFSAAgentStatus
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 6) = gSIRLibrary.SIRLookupPFFrequency
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupWhereClause, 6) = "is_available_on_client_screen = 1"
            'SAGICOR WPR14
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 7) = gSIRLibrary.SIRLookupCommissionLevel
            ' Do we have any records
            If m_lCurrentRecord < 1 Then
                ' No, we can only lookup all
                iLookupType = gPMConstants.PMELookupType.PMLookupAll
            Else
                ' Yes get current record
                oSIRpartyAG = m_oSIRPartyAGs.Item(m_lCurrentRecord)
            End If

            Select Case iLookupType
                Case gPMConstants.PMELookupType.PMLookupAll

                    ' Do not supply a key
                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""
                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = ""
                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 2) = ""
                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 3) = ""
                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 4) = ""
                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 5) = ""
                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 6) = ""
                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 7) = ""

                Case gPMConstants.PMELookupType.PMLookupAllEffective

                    ' Use keys and effective date from current record
                    ' Note: The keys are not used for the select, but are used by
                    '       the interface program to set the list index.
                    With oSIRpartyAG

                        m_lReturn = .GetProperties(iStatus:=gPMConstants.PMEComponentAction.PMView, vPartyAgentOriginID:=vPartyAgentOriginID,
                                                   vPaymentMethod:=vPaymentMethod, vPaymentFrequency:=vPaymentFrequency,
                                                   vAddressOnNotice:=vAddressOnNotice, vAgentStatus:=vAgentStatus)

                        m_lReturn = .bSIRParty.GetLookupProperties(vCurrentRecord:=ToSafeInteger(m_lCurrentRecord), vAreaId:=Nothing, vCurrencyId:=Nothing, vReminderTypeId:=Nothing, vServiceLevelId:=Nothing, vSeasonalGiftID:=Nothing, vCorrespondenceTypeId:=Nothing, vRenewalStopCodeId:=vRenewalStopCode, vPaymentTermCode:=vPaymentTermCode)

                        'Developer Guide No.98
                        m_lReturn = .GetProperties(iStatus:=gPMConstants.PMEComponentAction.PMView, vPartyAgentOriginID:=vPartyAgentOriginID, vPaymentMethod:=vPaymentMethod, vPaymentFrequency:=vPaymentFrequency, vAddressOnNotice:=vAddressOnNotice, vAgentStatus:=vAgentStatus, vCommissionlevel:=vCommissionlevel)


                        m_lReturn = .bSIRParty.GetLookupProperties(vCurrentRecord:=ToSafeInteger(m_lCurrentRecord), vAreaId:=Nothing, vCurrencyId:=Nothing, vReminderTypeId:=Nothing, vServiceLevelId:=Nothing, vSeasonalGiftID:=Nothing, vCorrespondenceTypeId:=Nothing, vRenewalStopCodeId:=vRenewalStopCode, vPaymentTermCode:=Nothing)



                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = vPartyAgentOriginID
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = vPaymentMethod
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 2) = vPaymentFrequency
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 3) = vAddressOnNotice
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 4) = vRenewalStopCode
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 5) = vAgentStatus

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 6) = vPaymentTermCode

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 7) = vCommissionlevel

                    End With

                Case gPMConstants.PMELookupType.PMLookupSingle

                    ' Set keys from current record
                    With oSIRpartyAG

                        m_lReturn = .GetProperties(iStatus:=gPMConstants.PMEComponentAction.PMView, vPartyAgentOriginID:=vPartyAgentOriginID,
                                                   vPaymentMethod:=vPaymentMethod, vPaymentFrequency:=vPaymentFrequency,
                                                   vAddressOnNotice:=vAddressOnNotice, vAgentStatus:=vAgentStatus, vCommissionlevel:=vCommissionlevel)

                        m_lReturn = .bSIRParty.GetLookupProperties(vCurrentRecord:=ToSafeInteger(m_lCurrentRecord), vAreaId:=Nothing, vCurrencyId:=Nothing, vReminderTypeId:=Nothing, vServiceLevelId:=Nothing, vSeasonalGiftID:=Nothing, vCorrespondenceTypeId:=Nothing, vRenewalStopCodeId:=vRenewalStopCode, vPaymentTermCode:=vPaymentTermCode)

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = vPartyAgentOriginID
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = vPaymentMethod
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 2) = vPaymentFrequency
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 3) = vAddressOnNotice
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 4) = vRenewalStopCode
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 5) = vAgentStatus
                        ' {* USER DEFINED CODE (End) *}


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 7) = vCommissionlevel
                        ' {* USER DEFINED CODE (End) *}
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 6) = vPaymentTermCode

                    End With
            End Select

            ' Default Effective Date to current date/time
            dtEffectiveDate = DateTime.Now

            ' Release SIRPartyAG reference
            oSIRpartyAG = Nothing

            ' Get the Lookup items
            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTabArray, iLanguageID:=iLanguageID, dtEffectiveDate:=dtEffectiveDate, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the Table Array
            vTableArray = vTabArray

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single SIRPartyAG directly into the database.
    '        Note: The SIRPartyAG will NOT be added to the collection.
    '
    ' ***************************************************************** '
    'DC 15/08/00 Added Invariant Key
    'EK 210199 Bug 253 Add Resolved date
    'DC141204 -added expense account id
    Public Function DirectAdd(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyAgentTypeID As Object = Nothing, Optional ByRef vPartyAgentOriginID As Object = Nothing, Optional ByRef vIsBranch As Object = Nothing, Optional ByRef vIsHeadOffice As Object = Nothing, Optional ByRef vAgencyAgreementDate As Object = Nothing, Optional ByRef vAgencyNextReviewDate As Object = Nothing, Optional ByRef vAgencyAccountNumber As Object = Nothing, Optional ByRef vShortName As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vResolvedName As Object = Nothing, Optional ByRef vPaymentTermCode As Object = Nothing, Optional ByRef vFileCode As Object = Nothing, Optional ByRef vABCCount As Object = Nothing, Optional ByRef vCurrencyId As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vLastActionType As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vDefaultCommissionPercent As Object = Nothing, Optional ByRef vTradingName As Object = Nothing, Optional ByRef vBinderIndicator As Object = Nothing, Optional ByRef vReportIndicator As Object = Nothing, Optional ByRef vInvariantKey As Object = Nothing, Optional ByVal vBrokerAbiId As Object = Nothing, Optional ByRef vExpenseAccountId As Object = Nothing, Optional ByRef vIsInTransferMode As Object = Nothing, Optional ByRef vTransferToBusinessTypeID As Object = Nothing, Optional ByRef vTransferToPartyCnt As Object = Nothing, Optional ByRef vOverrideCommission As Object = Nothing, Optional ByRef vOverrideCommissionRenewal As Object = Nothing, Optional ByRef vDomiciledForTax As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRpartyAG As bSIRPartyAG.SIRPartyAG
        'EK 27/9/9/99
        Dim lPartyTypeId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupPartyType, v_sCode:=gSIRLibrary.SIRPartyTypeAgent, v_dtEffectiveDate:=DateTime.Now, r_lID:=lPartyTypeId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Create a new SIRPartyAG
            oSIRpartyAG = New bSIRPartyAG.SIRPartyAG()
            m_lReturn = oSIRpartyAG.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            m_lReturn = oSIRpartyAG.bSIRParty.DirectAdd(vPartyTypeId:=ToSafeInteger(lPartyTypeId), vShortName:=vShortName, vName:=vName, vResolvedName:=vResolvedName, vABCCount:=vABCCount, vLastModified:=vLastModified, vCurrencyId:=vCurrencyId, vLastActionType:=vLastActionType, vDateCreated:=vDateCreated, vInvariantKey:=vInvariantKey)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRpartyAG = Nothing
                Return result
            End If

            ' Retrieve Primary Key of new Core record
            vPartyCnt = oSIRpartyAG.bSIRParty.PartyCnt
            oSIRpartyAG.PartyCnt = vPartyCnt

            m_lReturn = oSIRpartyAG.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vPartyCnt:=vPartyCnt, vPartyAgentTypeID:=vPartyAgentTypeID, vPartyAgentOriginID:=vPartyAgentOriginID, vIsBranch:=vIsBranch, vIsHeadOffice:=vIsHeadOffice, vAgencyAgreementDate:=vAgencyAgreementDate, vAgencyNextReviewDate:=vAgencyNextReviewDate, vAgencyAccountNumber:=vAgencyAccountNumber, vDefaultCommissionPercent:=vDefaultCommissionPercent, vTradingName:=vTradingName, vBinderIndicator:=vBinderIndicator, vReportIndicator:=vReportIndicator, vBrokerAbiId:=vBrokerAbiId, vExpenseAccountId:=vExpenseAccountId, vIsInTransferMode:=vIsInTransferMode, vTransferToBusinessTypeID:=vTransferToBusinessTypeID, vTransferToPartyCnt:=vTransferToPartyCnt, vOverrideCommission:=vOverrideCommission, vOverrideCommissionRenewal:=vOverrideCommissionRenewal, vDomiciledForTax:=vDomiciledForTax)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRpartyAG = Nothing
                Return result
            End If

            ' Add the SIRPartyAG to the Database
            m_lReturn = oSIRpartyAG.AddItem()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRpartyAG = Nothing
                Return result
            End If

            ' Retain the Primary Key of the SIRPartyAG Added
            With oSIRpartyAG
                PartyCnt = .PartyCnt
            End With


            oSIRpartyAG = Nothing

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a single SIRPartyAG directly from the database.
    '        Note: The SIRPartyAG will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    'Developer Guide No 101
    Public Function DirectDelete(Optional ByRef vPartyCnt As Object = 0) As Integer

        Dim result As Integer = 0
        Dim oSIRpartyAG As bSIRPartyAG.SIRPartyAG

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new SIRPartyAG
            oSIRpartyAG = New bSIRPartyAG.SIRPartyAG()
            m_lReturn = oSIRpartyAG.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            ' Set SIRPartyAG Primary Key
            'developer guide no.98
            m_lReturn = oSIRpartyAG.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMDelete, vPartyCnt:=vPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRpartyAG = Nothing
                Return result
            End If

            ' Delete the SIRPartyAG from the Database
            m_lReturn = oSIRpartyAG.DeleteItem()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRpartyAG = Nothing
                Return result
            End If


            m_lReturn = oSIRpartyAG.bSIRParty.DirectDelete(vPartyCnt:=vPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRpartyAG = Nothing
                Return result
            End If

            oSIRpartyAG = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: StoreSuppressedDocsList
    '
    ' Description: Stores the list of suppressed document types
    '
    ' ***************************************************************** '
    Public Function StoreSuppressedDocsList(Optional ByRef lPartyCnt As Integer = 0, Optional ByRef vSuppressedDocs() As Object = Nothing, Optional v_sUniqueId As String = "", Optional v_sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Try


            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' Delete the current records
            '
            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the party count parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="unique_id", vValue:=v_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="screen_hierarchy", vValue:=v_sScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDelSuppressedDocsSQL, sSQLName:=ACDelSuppressedDocsName, bStoredProcedure:=ACDelSuppressedDocsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '
            ' Store the current selections
            '
            'CMG/PB 19072002 If the suppressed docs array is empty
            ' this was failing so add this check
            'SD 14/08/2002 START modification
            If vSuppressedDocs.GetUpperBound(0) - vSuppressedDocs.GetLowerBound(0) >= 0 Then
                For i As Integer = 0 To vSuppressedDocs.GetUpperBound(0)


                    If Not (Object.Equals(vSuppressedDocs(i), Nothing)) Then
                        ' Clear the Database Parameters Collection
                        m_oDatabase.Parameters.Clear()

                        ' Add the party count parameter
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                        ' Add the process type

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="process_type", vValue:=CStr(vSuppressedDocs(i)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="unique_id", vValue:=v_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="screen_hierarchy", vValue:=v_sScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                        ' Execute SQL Statement
                        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSuppressedDocsSQL, sSQLName:=ACAddSuppressedDocsName, bStoredProcedure:=ACAddSuppressedDocsStored)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                    ''SD 14/08/2002 END modification

                Next i
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StoreSuppressedDocsList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="StoreSuppressedDocsList", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required SIRPartyAGs and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vLockMode As Object = 0, Optional ByRef vPartyCnt As Object = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        'Developer Guide No 21. 
        Dim oFields As DataRow
        Dim oSIRpartyAG As bSIRPartyAG.SIRPartyAG

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oSIRPartyAGs.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Informations.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vLockMode = gPMConstants.PMELockMode.PMNoLock
            End If

            ' Check for Valid Primary Key
            Dim dbNumericTemp2 As Double

            If (Not Informations.IsNothing(vPartyCnt)) And (Not Double.TryParse(CStr(vPartyCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vPartyCnt=" & vPartyCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                Return result
            End If


            If Not Informations.IsNothing(vPartyCnt) Then

                ' Create New SIRPartyAG
                oSIRpartyAG = New bSIRPartyAG.SIRPartyAG()
                m_lReturn = oSIRpartyAG.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

                ' Set component primary keys
                With oSIRpartyAG
                    .PartyCnt = vPartyCnt


                    m_lReturn = .SelectItem()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return m_lReturn
                    End If
                End With

                ' Add SIRPartyAG to collection
                If m_oSIRPartyAGs.Count = 0 Then
                    m_oSIRPartyAGs.Add(Nothing)
                End If
                m_lReturn = m_oSIRPartyAGs.Add(oNewSIRPartyAG:=oSIRpartyAG)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Call the core Business method

                m_lReturn = oSIRpartyAG.bSIRParty.GetDetails(vLockMode:=vLockMode, vPartyCnt:=vPartyCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                oSIRpartyAG = Nothing

            Else

                ' No Key, Get All Records

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' How many records were selected
                lRecordCount = m_oDatabase.Records.Count()

                ' Do we have any records ?
                If lRecordCount < 1 Then
                    ' No Records, return PMFalse
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If

                ' Yes, load them into the collection
                For lSub As Integer = 1 To lRecordCount

                    ' Create New
                    oSIRpartyAG = New bSIRPartyAG.SIRPartyAG()
                    'Developer Guide No. 9
                    m_lReturn = oSIRpartyAG.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

                    ' Set oFields to refer to one Record
                    'Developer Guide No. 111
                    oFields = m_oDatabase.Records.Item(lSub - 1).Fields()

                    ' Set component primary keys from current record
                    With oSIRpartyAG
                        .PartyCnt = gPMFunctions.NullToLong(oFields("party_cnt"))

                        m_lReturn = .SelectItem()

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If


                        m_lReturn = .bSIRParty.GetDetails(vLockMode:=ToSafeInteger(vLockMode), vPartyCnt:=ToSafeInteger(.PartyCnt))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    End With

                    ' Add SIRPartyAG to collection
                    If m_oSIRPartyAGs.Count = 0 Then
                        m_oSIRPartyAGs.Add(Nothing)
                    End If
                    m_lReturn = m_oSIRPartyAGs.Add(oNewSIRPartyAG:=oSIRpartyAG)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oSIRpartyAG = Nothing
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
    ' Description: Gets the required SIRPartyAGs and populate the Collection
    '
    ' eck270901 Added resolved Name
    'DC190803 added agent status id
    'DC021203 -PN8727 -fsa compliance -registration number
    ' ***************************************************************** '
    'DC141204 -added expense account id
    Public Function GetNext(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyAgentTypeID As Object = Nothing, Optional ByRef vPartyAgentOriginID As Object = Nothing, Optional ByRef vIsBranch As Object = Nothing, Optional ByRef vIsHeadOffice As Object = Nothing, Optional ByRef vAgencyAgreementDate As Object = Nothing, Optional ByRef vAgencyNextReviewDate As Object = Nothing, Optional ByRef vAgencyAccountNumber As Object = Nothing, Optional ByRef vSourceId As Object = Nothing, Optional ByRef vPartyID As Object = Nothing, Optional ByRef vCurrencyId As Object = Nothing, Optional ByRef vShortName As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vResolvedName As Object = Nothing, Optional ByRef vAgentCnt As Object = Nothing, Optional ByRef vDefaultCommissionPercent As Object = Nothing, Optional ByRef vTradingName As Object = Nothing, Optional ByRef vBinderIndicator As Object = Nothing, Optional ByRef vReportIndicator As Object = Nothing, Optional ByRef vStatements As Object = Nothing, Optional ByRef vFileCode As Object = Nothing, Optional ByRef vPFFrequencyID As Object = Nothing, Optional ByRef vPartyCategoryID As Object = Nothing, Optional ByRef vConsultantCnt As Object = Nothing, Optional ByRef vAgentGroupCnt As Object = Nothing, Optional ByRef vPaymentMethod As Object = Nothing, Optional ByRef vPaymentFrequency As Object = Nothing, Optional ByRef vAddressOnNotice As Object = Nothing, Optional ByRef vTypeOfStatement As Object = Nothing, Optional ByRef vSource As Object = Nothing, Optional ByRef vTitle As Object = Nothing, Optional ByRef vBranch As Object = Nothing, Optional ByRef vSubBranch As Object = Nothing, Optional ByRef vMultipac As Object = Nothing, Optional ByRef vRenewalStopCode As Object = Nothing, Optional ByRef vContactPerson As Object = Nothing, Optional ByRef vFirstName As Object = Nothing, Optional ByRef vDateCancelled As Object = Nothing, Optional ByRef vAgentStatus As Object = Nothing, Optional ByRef vRegistrationNumber As Object = Nothing, Optional ByRef vBrokerAbiId As Object = Nothing, Optional ByRef vExpenseAccountId As Object = Nothing, Optional ByRef vIsInTransferMode As Object = Nothing, Optional ByRef vTransferToBusinessTypeID As Object = Nothing, Optional ByRef vTransferToPartyCnt As Object = Nothing, Optional ByRef vOverrideCommission As Object = Nothing, Optional ByRef vOverrideCommissionRenewal As Object = Nothing, Optional ByRef vDomiciledForTax As Object = Nothing, Optional ByRef vAllowConsolidate As Object = Nothing, Optional ByRef vParamArray As Object = Nothing, Optional ByRef vIsViewableOnly As Boolean = False, Optional ByRef vBankAccount As Object = Nothing, Optional ByRef vCommissionLevel As Object = Nothing,
                            Optional ByRef vCorrespondenceTypeId As Object = Nothing, Optional ByRef vReceivesClientCorrespondence As Object = Nothing) As Object
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse

        Dim oSIRpartyAG As bSIRPartyAG.SIRPartyAG
        Dim iStatus As Integer
        'sj 30/07/2002 - start
        Dim vWorkSourceId As Object = Nothing
        'sj 30/07/2002 - end

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oSIRPartyAGs.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oSIRpartyAG = m_oSIRPartyAGs.Item(m_lCurrentRecord)

            If (Informations.IsDBNull(vAgentStatus) OrElse Informations.IsNothing(vAgentStatus)) Then
                vAgentStatus = 0
            End If
            If (Informations.IsDBNull(vAgencyAgreementDate) OrElse Informations.IsNothing(vAgencyAgreementDate)) Then
                vAgencyAgreementDate = "#12/30/1899#"
            End If
            If (Informations.IsDBNull(vAgencyNextReviewDate) OrElse Informations.IsNothing(vAgencyNextReviewDate)) Then
                vAgencyNextReviewDate = "#12/30/1899#"
            End If
            If (Informations.IsDBNull(vDateCancelled) OrElse Informations.IsNothing(vDateCancelled)) Then
                vDateCancelled = "#12/30/1899#"
            End If

            'Developer Guide No.98
            m_lReturn = oSIRpartyAG.GetProperties(iStatus, vPartyCnt:=vPartyCnt, vPartyAgentTypeID:=vPartyAgentTypeID, vPartyAgentOriginID:=vPartyAgentOriginID, vIsBranch:=vIsBranch, vIsHeadOffice:=vIsHeadOffice, vAgencyAgreementDate:=vAgencyAgreementDate, vAgencyNextReviewDate:=vAgencyNextReviewDate, vAgencyAccountNumber:=vAgencyAccountNumber, vDefaultCommissionPercent:=vDefaultCommissionPercent, vTradingName:=vTradingName, vBinderIndicator:=vBinderIndicator, vReportIndicator:=vReportIndicator, vLinkedAccountExecutiveID:=vConsultantCnt, vLinkedAccountGroup:=vAgentGroupCnt, vPaymentMethod:=vPaymentMethod, vPaymentFrequency:=vPaymentFrequency, vAddressOnNotice:=vAddressOnNotice, vTypeOfStatement:=vTypeOfStatement, vSource:=vSource, vTitle:=vTitle, vMultipac:=vMultipac, vContactPerson:=vContactPerson, vFirstName:=vFirstName, vDateCancelled:=vDateCancelled, vAgentStatus:=vAgentStatus, vRegistrationNumber:=vRegistrationNumber, vBrokerAbiId:=vBrokerAbiId, vExpenseAccountId:=vExpenseAccountId, vIsInTransferMode:=vIsInTransferMode, vTransferToBusinessTypeID:=vTransferToBusinessTypeID, vTransferToPartyCnt:=vTransferToPartyCnt, vOverrideCommission:=vOverrideCommission, vOverrideCommissionRenewal:=vOverrideCommissionRenewal, vDomiciledForTax:=vDomiciledForTax, vAllowConsolidate:=vAllowConsolidate, vParamArray:=vParamArray, vBankAccount:=vBankAccount, vCommissionlevel:=vCommissionLevel, vReceivesClientCorrespondence:=vReceivesClientCorrespondence)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get Core details
            'UPGRADE_WARNING: Couldn't resolve default property of object oSIRpartyAG.bSIRParty.GetDetails. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            m_lReturn = oSIRpartyAG.bSIRParty.GetDetails(vPartyCnt:=vPartyCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oSIRpartyAG.bSIRParty.GetNext(vSourceId:=vWorkSourceId, vPartyID:=vPartyID, vCurrencyId:=vCurrencyId, vShortName:=vShortName, vName:=vName, vResolvedName:=vResolvedName, vAgentCnt:=vAgentCnt, vStatements:=vStatements, vFileCode:=vFileCode, vPFFrequencyID:=vPFFrequencyID, vPartyCategoryID:=vPartyCategoryID, vRenewalStopCodeId:=vRenewalStopCode, vSubBranchID:=vSubBranch, vOverrideCommission:=vOverrideCommission, vOverrideCommissionRenewal:=vOverrideCommissionRenewal, vParamArray:=m_vParamArrayForParty, vCorrespondenceTypeId:=vCorrespondenceTypeId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            vBranch = vWorkSourceId
            vSourceId = vWorkSourceId

            oSIRpartyAG = Nothing
            'm_lpartycnt
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied SIRPartyAG into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    'EK 210199 Bug 253 Add Resolved Name
    ' PW100702 - add consultant/agent group
    'DC180803 added agent status id
    'DC021203 -PN8727 -fsa compliance -registration number
    'DC141204 -added expense account id
    'Developer Guide No. 101
    'Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyAgentTypeID As Object = Nothing, Optional ByRef vPartyAgentOriginID As Object = Nothing, Optional ByRef vIsBranch As Object = Nothing, Optional ByRef vIsHeadOffice As Object = Nothing, Optional ByRef vAgencyAgreementDate As Object = Nothing, Optional ByRef vAgencyNextReviewDate As Object = Nothing, Optional ByRef vAgencyAccountNumber As Object = Nothing, Optional ByRef vShortName As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vResolvedName As Object = Nothing, Optional ByRef vAgentCnt As Object = Nothing, Optional ByRef vDefaultCommissionPercent As Object = Nothing, Optional ByRef vTradingName As Object = Nothing, Optional ByRef vBinderIndicator As Object = Nothing, Optional ByRef vReportIndicator As Object = Nothing, Optional ByRef vStatements As Object = Nothing, Optional ByRef vCurrencyId As Object = Nothing, Optional ByRef vFileCode As Object = Nothing, Optional ByRef vTermsOfPayment As Object = Nothing, Optional ByRef vPartyCategoryID As Object = Nothing, Optional ByRef vConsultantCnt As Object = Nothing, Optional ByRef vAgentGroupCnt As Object = Nothing, Optional ByRef vPaymentMethod As Object = Nothing, Optional ByRef vPaymentFrequency As Object = Nothing, Optional ByRef vAddressOnNotice As Object = Nothing, Optional ByRef vTypeOfStatement As Object = Nothing, Optional ByRef vSource As Object = Nothing, Optional ByRef vTitle As Object = Nothing, Optional ByRef vBranch As Object = Nothing, Optional ByRef vSubBranch As Object = Nothing, Optional ByRef vMultipac As Object = Nothing, Optional ByRef vRenewalStopCode As Object = Nothing, Optional ByRef vContactPerson As Object = Nothing, Optional ByRef vFirstName As Object = Nothing, Optional ByRef vBankAccount As Object = Nothing, Optional ByRef vDateCancelled As Object = Nothing, Optional ByRef vAgentStatus As Object = Nothing, Optional ByRef vRegistrationNumber As Object = Nothing, Optional ByRef vBrokerAbiId As Object = Nothing, Optional ByRef vExpenseAccountId As Object = Nothing, Optional ByRef vIsInTransferMode As Object = Nothing, Optional ByRef vTransferToBusinessTypeID As Object = Nothing, Optional ByRef vTransferToPartyCnt As Object = Nothing, Optional ByRef vOverrideCommission As Object = Nothing, Optional ByRef vDomiciledForTax As Object = Nothing, Optional ByRef vAllowConsolidate As Object = Nothing, Optional ByRef vMakeLiveArray() As Object = Nothing) As Integer
    'EK 210199 Bug 253 Add Resolved Name
    ' PW100702 - add consultant/agent group
    'DC180803 added agent status id
    'DC021203 -PN8727 -fsa compliance -registration number
    'DC141204 -added expense account id
    'Developer Guide No. 101
    Public Function EditAdd(ByRef lRow As Object, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyAgentTypeID As Object = Nothing, Optional ByRef vPartyAgentOriginID As Object = Nothing, Optional ByRef vIsBranch As Object = Nothing, Optional ByRef vIsHeadOffice As Object = Nothing, Optional ByRef vAgencyAgreementDate As Object = Nothing, Optional ByRef vAgencyNextReviewDate As Object = Nothing, Optional ByRef vAgencyAccountNumber As Object = Nothing, Optional ByRef vShortName As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vResolvedName As Object = Nothing, Optional ByRef vAgentCnt As Object = Nothing, Optional ByRef vDefaultCommissionPercent As Object = Nothing, Optional ByRef vTradingName As Object = Nothing, Optional ByRef vBinderIndicator As Object = Nothing, Optional ByRef vReportIndicator As Object = Nothing, Optional ByRef vStatements As Object = Nothing, Optional ByRef vCurrencyId As Object = Nothing, Optional ByRef vFileCode As Object = Nothing, Optional ByRef vPFFrequencyID As Object = Nothing, Optional ByRef vPartyCategoryID As Object = Nothing, Optional ByRef vConsultantCnt As Object = Nothing, Optional ByRef vAgentGroupCnt As Object = Nothing, Optional ByRef vPaymentMethod As Object = Nothing, Optional ByRef vPaymentFrequency As Object = Nothing, Optional ByRef vAddressOnNotice As Object = Nothing, Optional ByRef vTypeOfStatement As Object = Nothing, Optional ByRef vSource As Object = Nothing, Optional ByRef vTitle As Object = Nothing, Optional ByRef vBranch As Object = Nothing, Optional ByRef vSubBranch As Object = Nothing, Optional ByRef vMultipac As Object = Nothing, Optional ByRef vRenewalStopCode As Object = Nothing, Optional ByRef vContactPerson As Object = Nothing, Optional ByRef vFirstName As Object = Nothing, Optional ByRef vDateCancelled As Object = Nothing, Optional ByRef vAgentStatus As Object = Nothing, Optional ByRef vRegistrationNumber As Object = Nothing, Optional ByRef vBrokerAbiId As Object = Nothing, Optional ByRef vExpenseAccountId As Object = Nothing, Optional ByRef vIsInTransferMode As Object = Nothing, Optional ByRef vTransferToBusinessTypeID As Object = Nothing, Optional ByRef vTransferToPartyCnt As Object = Nothing, Optional ByRef vOverrideCommission As Object = Nothing, Optional ByRef vOverrideCommissionRenewal As Object = Nothing, Optional ByRef vDomiciledForTax As Object = Nothing, Optional ByRef vAllowConsolidate As Object = Nothing, Optional ByRef vParamArray As Object = Nothing, Optional ByRef v_lCommissionLevel As Object = Nothing, Optional ByRef vBankAccount As Object = Nothing, Optional ByRef vCorrespondenceTypeId As Object = Nothing,
                            Optional ByRef vReceivesClientCorr As Object = Nothing, Optional vUniqueId As String = "", Optional vScreenHierarchy As String = "") As Integer
        Dim result As Integer = 0
        Dim oSIRpartyAG As bSIRPartyAG.SIRPartyAG
        Dim lPartyTypeId As Integer




        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'Get party type id for an agent
            m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupPartyType, v_sCode:=gSIRLibrary.SIRPartyTypeAgent, v_dtEffectiveDate:=DateTime.Now, r_lID:=lPartyTypeId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If Not m_bImportData Then
                If m_oSIRPartyAGs.Count() <> (lRow - 1) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Create a new SIRPartyAG
            oSIRpartyAG = New bSIRPartyAG.SIRPartyAG()
            m_lReturn = oSIRpartyAG.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            ' Populate SIRPartyAG Attributes
            ' PW100702 - add consultant/agent group
            'DC180803 -added agent status id
            'DC141204 -added expense account id
            m_lReturn = oSIRpartyAG.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vPartyCnt:=vPartyCnt, vPartyAgentTypeID:=vPartyAgentTypeID, vPartyAgentOriginID:=vPartyAgentOriginID, vIsBranch:=vIsBranch, vIsHeadOffice:=vIsHeadOffice, vAgencyAgreementDate:=vAgencyAgreementDate, vAgencyNextReviewDate:=vAgencyNextReviewDate, vAgencyAccountNumber:=vAgencyAccountNumber, vDefaultCommissionPercent:=vDefaultCommissionPercent, vTradingName:=vTradingName, vBinderIndicator:=vBinderIndicator, vReportIndicator:=vReportIndicator, vConsultantCnt:=vConsultantCnt, vAgentGroupCnt:=vAgentGroupCnt, vPaymentMethod:=vPaymentMethod, vPaymentFrequency:=vPaymentFrequency, vAddressOnNotice:=vAddressOnNotice, vTypeOfStatement:=vTypeOfStatement, vSource:=vSource, vTitle:=vTitle, vMultipac:=vMultipac, vContactPerson:=vContactPerson, vFirstName:=vFirstName, vDateCancelled:=vDateCancelled, vAgentStatus:=vAgentStatus, vRegistrationNumber:=vRegistrationNumber, vBrokerAbiId:=vBrokerAbiId, vExpenseAccountId:=vExpenseAccountId, vIsInTransferMode:=vIsInTransferMode, vTransferToBusinessTypeID:=vTransferToBusinessTypeID, vTransferToPartyCnt:=vTransferToPartyCnt, vOverrideCommission:=vOverrideCommission, vOverrideCommissionRenewal:=vOverrideCommissionRenewal, vDomiciledForTax:=vDomiciledForTax, vAllowConsolidate:=vAllowConsolidate, vParamArray:=vParamArray, vBankAccount:=vBankAccount, v_lCommissionlevel:=v_lCommissionLevel, vReceivesClientCorrespondence:=vReceivesClientCorr, vUniqueId:=vUniqueId, vScreenHierarchy:=vScreenHierarchy) '(RC) QBENZ014   '(RC) PLICO 9-10
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oSIRpartyAG = Nothing
                Return result
            End If

            ' Add SIRPartyAG to collection
            If m_oSIRPartyAGs.Count = 0 Then
                m_oSIRPartyAGs.Add(Nothing)
            End If
            m_lReturn = m_oSIRPartyAGs.Add(oNewSIRPartyAG:=oSIRpartyAG)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRpartyAG = Nothing
                Return result
            End If

            ReDim m_vParamArrayForParty(2)

            m_lReturn = oSIRpartyAG.bSIRParty.EditAdd(lRow:=lRow, vPartyTypeId:=ToSafeInteger(lPartyTypeId), vShortName:=vShortName, vName:=vName, vResolvedName:=vResolvedName, vAgentCnt:=vAgentCnt, vPFFrequencyID:=vPFFrequencyID, vPartyCategoryID:=vPartyCategoryID, vFileCode:=vFileCode, vCurrencyId:=vCurrencyId, vStatements:=vStatements, vSourceId:=vBranch, vRenewalStopCodeId:=vRenewalStopCode, vSubBranchID:=vSubBranch, vOverrideCommission:=vOverrideCommission, vParamArray:=m_vParamArrayForParty, vCorrespondenceTypeId:=vCorrespondenceTypeId, sUniqueId:=ToSafeString(vUniqueId), sScreenHierarchy:=ToSafeString(vScreenHierarchy), vOverrideCommissionRenewal:=vOverrideCommissionRenewal)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRpartyAG = Nothing
                Return result
            End If

            'UPGRADE_NOTE: Object oSIRpartyAG may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oSIRpartyAG = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try

    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the SIRPartyAG
    '              specified and updates the SIRPartyAG with the new values.
    '
    ' ***************************************************************** '
    'EK 210199 Bug 253 Add resolved name
    ' PW100702 - add consultant/agent group
    'DC021203 -PN8727 -fsa compliance -registration number
    'DC141204 -added expense account id
    'Added vAllowReallocationInstalmentDebt : C & G Reallocate Debt :Bhagwan Singh :12.05.2009
    Public Function EditUpdate(ByRef lRow As Object, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyAgentTypeID As Object = Nothing, Optional ByRef vPartyAgentOriginID As Object = Nothing, Optional ByRef vIsBranch As Object = Nothing, Optional ByRef vIsHeadOffice As Object = Nothing, Optional ByRef vAgencyAgreementDate As Object = Nothing, Optional ByRef vAgencyNextReviewDate As Object = Nothing, Optional ByRef vAgencyAccountNumber As Object = Nothing, Optional ByRef vShortName As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vResolvedName As Object = Nothing, Optional ByRef vAgentCnt As Object = Nothing, Optional ByRef vDefaultCommissionPercent As Object = Nothing, Optional ByRef vTradingName As Object = Nothing, Optional ByRef vBinderIndicator As Object = Nothing, Optional ByRef vReportIndicator As Object = Nothing, Optional ByRef vStatements As Object = Nothing, Optional ByRef vCurrencyId As Object = Nothing, Optional ByRef vFileCode As Object = Nothing, Optional ByRef vPFFrequencyID As Object = Nothing, Optional ByRef vPartyCategoryID As Object = Nothing, Optional ByRef vConsultantCnt As Object = Nothing, Optional ByRef vAgentGroupCnt As Object = Nothing, Optional ByRef vPaymentMethod As Object = Nothing, Optional ByRef vPaymentFrequency As Object = Nothing, Optional ByRef vAddressOnNotice As Object = Nothing, Optional ByRef vTypeOfStatement As Object = Nothing, Optional ByRef vSource As Object = Nothing, Optional ByRef vTitle As Object = Nothing, Optional ByRef vBranch As Object = Nothing, Optional ByRef vSubBranch As Object = Nothing, Optional ByRef vMultipac As Object = Nothing, Optional ByRef vRenewalStopCode As Object = Nothing, Optional ByRef vContactPerson As Object = Nothing, Optional ByRef vFirstName As Object = Nothing, Optional ByRef vDateCancelled As Object = Nothing, Optional ByRef vAgentStatus As Object = Nothing, Optional ByRef vRegistrationNumber As Object = Nothing, Optional ByRef vBrokerAbiId As Object = Nothing, Optional ByRef vExpenseAccountId As Object = Nothing, Optional ByRef vIsInTransferMode As Object = Nothing, Optional ByRef vTransferToBusinessTypeID As Object = Nothing, Optional ByRef vTransferToPartyCnt As Object = Nothing, Optional ByRef vOverrideCommission As Object = Nothing, Optional ByRef vOverrideCommissionRenewal As Object = Nothing, Optional ByRef vDomiciledForTax As Object = Nothing, Optional ByRef vAllowConsolidate As Object = Nothing, Optional ByRef vParamArray As Object = Nothing, Optional ByRef v_lCommissionLevel As Object = Nothing, Optional ByRef vBankAccount As Object = Nothing, Optional ByRef vCorrespondenceTypeId As Object = Nothing,
                               Optional ByRef vReceivesClientCorr As Object = Nothing, Optional vUniqueId As String = "", Optional vScreenHierarchy As String = "") As Integer

        Dim oSIRpartyAG As bSIRPartyAG.SIRPartyAG
        Dim iStatus As Integer
        Dim result As Integer = 0
        '    '(RC) PLICO 9-10
        '    Dim vCommissionPostingType As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oSIRPartyAGs.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oSIRpartyAG = m_oSIRPartyAGs.Item(lRow)

            ' Check the Status of the SIRPartyAG

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oSIRpartyAG.DatabaseStatus
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

            m_lReturn = oSIRpartyAG.SetProperties(iStatus:=iStatus, vPartyCnt:=vPartyCnt, vPartyAgentTypeID:=vPartyAgentTypeID, vPartyAgentOriginID:=vPartyAgentOriginID, vIsBranch:=vIsBranch, vIsHeadOffice:=vIsHeadOffice, vAgencyAgreementDate:=vAgencyAgreementDate, vAgencyNextReviewDate:=vAgencyNextReviewDate, vAgencyAccountNumber:=vAgencyAccountNumber, vDefaultCommissionPercent:=vDefaultCommissionPercent, vTradingName:=vTradingName, vBinderIndicator:=vBinderIndicator, vReportIndicator:=vReportIndicator, vConsultantCnt:=vConsultantCnt, vAgentGroupCnt:=vAgentGroupCnt, vPaymentMethod:=vPaymentMethod, vPaymentFrequency:=vPaymentFrequency, vAddressOnNotice:=vAddressOnNotice, vTypeOfStatement:=vTypeOfStatement, vSource:=vSource, vTitle:=vTitle, vMultipac:=vMultipac, vContactPerson:=vContactPerson, vFirstName:=vFirstName, vDateCancelled:=vDateCancelled, vAgentStatus:=vAgentStatus, vRegistrationNumber:=vRegistrationNumber, vBrokerAbiId:=vBrokerAbiId, vExpenseAccountId:=vExpenseAccountId, vIsInTransferMode:=vIsInTransferMode, vTransferToBusinessTypeID:=vTransferToBusinessTypeID, vTransferToPartyCnt:=vTransferToPartyCnt, vOverrideCommission:=vOverrideCommission, vOverrideCommissionRenewal:=vOverrideCommissionRenewal, vDomiciledForTax:=vDomiciledForTax, vAllowConsolidate:=vAllowConsolidate, vParamArray:=vParamArray, vBankAccount:=vBankAccount, v_lCommissionlevel:=v_lCommissionLevel, vReceivesClientCorrespondence:=vReceivesClientCorr, vUniqueId:=vUniqueId, vScreenHierarchy:=vScreenHierarchy) '(RC) QBENZ014  '(RC) PLICO 9-10
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oSIRpartyAG = Nothing
                Return result
            End If

            ReDim m_vParamArrayForParty(2)
            m_lReturn = oSIRpartyAG.bSIRParty.EditUpdate(lRow:=lRow, vShortName:=vShortName, vName:=vName, vResolvedName:=vResolvedName, vAgentCnt:=vAgentCnt, vStatements:=vStatements, vCurrencyId:=vCurrencyId, vFileCode:=vFileCode, vPFFrequencyID:=vPFFrequencyID, vPartyCategoryID:=vPartyCategoryID, vSourceId:=vBranch, vRenewalStopCodeId:=vRenewalStopCode, vSubBranchID:=vSubBranch, vOverrideCommission:=vOverrideCommission, vParamArray:=m_vParamArrayForParty, vCorrespondenceTypeId:=vCorrespondenceTypeId, sUniqueId:=ToSafeString(vUniqueId), sScreenHierarchy:=ToSafeString(vScreenHierarchy), vOverrideCommissionRenewal:=vOverrideCommissionRenewal)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                EditUpdate = gPMConstants.PMEReturnCode.PMFalse
                oSIRpartyAG = Nothing
                Exit Function
            End If

            ' Release reference to SIRPartyAG
            oSIRpartyAG = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetSubBranches
    '
    ' Description:
    '
    ' History: 11/06/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function GetSubBranches(ByVal v_lSourceID As Integer, ByRef r_vSubBranchArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Return SiriusCoreFunc.GetSubBranches(v_oDatabase:=m_oDatabase, v_lSourceID:=v_lSourceID, r_vSubBranchArray:=r_vSubBranchArray)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSubBranches Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSubBranches", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified SIRPartyAG can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oSIRpartyAG As bSIRPartyAG.SIRPartyAG

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oSIRPartyAGs.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oSIRpartyAG = m_oSIRPartyAGs.Item(lRow)

            ' Check the Status of the SIRPartyAG

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oSIRpartyAG.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oSIRpartyAG.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oSIRpartyAG.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If


            m_lReturn = oSIRpartyAG.bSIRParty.EditDelete(lRow:=ToSafeInteger(lRow))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRpartyAG = Nothing
                Return result
            End If

            ' Release reference to SIRPartyAG
            oSIRpartyAG = Nothing

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
            For lSub As Integer = 1 To m_oSIRPartyAGs.Count()
                Select Case m_oSIRPartyAGs.Item(lSub).DatabaseStatus
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
        Dim oSIRpartyAG As bSIRPartyAG.SIRPartyAG = Nothing
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oSIRPartyAGs.Count()
                oSIRpartyAG = m_oSIRPartyAGs.Item(lSub)


                Select Case oSIRpartyAG.DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing

                    Case gPMConstants.PMEComponentAction.PMAdd

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = BeginTrans()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Add Item

                        m_lReturn = oSIRpartyAG.bSIRParty.Update()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                        ' Retrieve Primary Key of Core Item added

                        PartyCnt = oSIRpartyAG.bSIRParty.PartyCnt
                        oSIRpartyAG.PartyCnt = PartyCnt

                        'm_lReturn = CommitTrans()
                        m_lReturn = oSIRpartyAG.AddItem()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMEdit

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = BeginTrans()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Update Item
                        m_lReturn = oSIRpartyAG.UpdateItem()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If


                        m_lReturn = oSIRpartyAG.bSIRParty.Update()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMDelete

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = BeginTrans()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Delete Item
                        m_lReturn = oSIRpartyAG.DeleteItem()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If


                        m_lReturn = oSIRpartyAG.bSIRParty.Update()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Retain the Primary Key of the SIRPartyAG
            With oSIRpartyAG
                PartyCnt = .PartyCnt
            End With

            '    'SP191198 - Now update Gemini (if installed)
            '    m_lReturn = oSIRPartyAG.bSIRParty.UpdateGemini(vPartyCnt:=PartyCnt, _
            ''                                                vTask:=oSIRPartyAG.DatabaseStatus)
            '
            '    If (m_lReturn& <> PMTrue) Then
            '        Update = PMFalse
            '    End If

            ' Release last reference
            oSIRpartyAG = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CommitTrans()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Refresh the Collection Items D/B Status
                    lSub = 1

                    ' For each item in the collection
                    Do While lSub <= m_oSIRPartyAGs.Count()

                        ' With the item
                        With m_oSIRPartyAGs.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oSIRPartyAGs.Delete(lSub)

                                    ' Anything Else
                                Case Else
                                    ' Set Status to view
                                    .DatabaseStatus = gPMConstants.PMEComponentAction.PMView
                                    lSub += 1

                            End Select

                        End With

                    Loop

                Else

                    m_lReturn = RollbackTrans()
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

    ' ***************************************************************** '
    ' Name: Getnextshortname
    '
    ' Description: Function used to get the next available
    '              Agent shortname
    '
    ' JB 050199
    '
    ' ***************************************************************** '
    Public Function GetNextShortname(ByVal sCode As String, ByRef iShortname As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add Party Code as an INPUT param for an update
            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=sCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add Shortname as an OUTPUT param for an insert
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Shortname", vValue:=CStr(iShortname), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetNextRefSQL, sSQLName:=ACGetNextRefName, bStoredProcedure:=ACGetNextRefStored, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the ID of the record inserted
            iShortname = (m_oDatabase.Parameters.Item("shortname").Value) + 1

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetnextshortnameFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="Getnextshortname", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    ' Name: GetPartyCnt
    '
    ' Description: Get party count for a given reference (ie shortname)
    '
    ' ***************************************************************** '
    Public Function GetPartyCnt(Optional ByRef vPartyRef As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRpartyAG As bSIRPartyAG.SIRPartyAG

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyAG - need to for to core for shortname
            oSIRpartyAG = New bSIRPartyAG.SIRPartyAG()

            m_lReturn = oSIRpartyAG.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            m_lReturn = oSIRpartyAG.bSIRParty.GetPartyCnt(vPartyRef:=vPartyRef, vPartyCnt:=vPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRpartyAG = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetPartyCnt Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyCnt", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetOtherDetails
    '
    ' Description: Get other details from DB, for related parties.
    '
    ' PW090702 - add parameters for additional tab fields
    '
    ' ***************************************************************** '
    Public Function GetOtherDetails(Optional ByRef vAgentCnt As Object = Nothing, Optional ByRef vAgentRef As Object = Nothing, Optional ByRef vAgentName As Object = Nothing, Optional ByRef vConsultantCnt As Object = Nothing, Optional ByRef vConsultantRef As Object = Nothing, Optional ByRef vConsultantName As Object = Nothing, Optional ByRef vAgentGroupCnt As Object = Nothing, Optional ByRef vAgentGroupRef As Object = Nothing, Optional ByRef vAgentGroupName As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vAssociates As Object = Nothing, Optional ByRef vDocs As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRpartyAG As bSIRPartyAG.SIRPartyAG

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyAG - ned to go to core for agent
            oSIRpartyAG = New bSIRPartyAG.SIRPartyAG()
            m_lReturn = oSIRpartyAG.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            m_lReturn = oSIRpartyAG.bSIRParty.GetOtherDetails(vAgentCnt:=vAgentCnt, vAgentRef:=vAgentRef, vAgentName:=vAgentName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '
            ' PW160702 - get consultant and agent group
            '

            m_lReturn = oSIRpartyAG.bSIRParty.GetOtherDetails(vAgentCnt:=vConsultantCnt, vAgentRef:=vConsultantRef, vAgentName:=vConsultantName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oSIRpartyAG.bSIRParty.GetOtherDetails(vAgentCnt:=vAgentGroupCnt, vAgentRef:=vAgentGroupRef, vAgentName:=vAgentGroupName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' PW150702 - get suppressed documents
            'developer guide no.118
            m_lReturn = GetSuppressedDocs(vPartyCnt:=vPartyCnt, vDocs:=vDocs)

            ' PW090702 - get associates

            'developer guide no.118
            m_lReturn = oSIRpartyAG.bSIRParty.GetAssociates(vPartyCnt:=vPartyCnt, vAssociates:=vAssociates, lPartyRelationshipGroupId:=ACPartyRelationshipGroupAgent)

            oSIRpartyAG = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetOtherDetailsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOtherDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DC220803 -PS253 -fsa compliance
    ' ***************************************************************** '
    ' Name: GetRiskGroups
    '
    ' Description: Get risk groups for agent
    '
    ' ***************************************************************** '
    Public Function GetRiskGroups(ByRef vRiskGroups(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the party count parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(m_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAgentRiskGroupsSQL, sSQLName:=ACGetAgentRiskGroupsName, bStoredProcedure:=ACGetAgentRiskGroupsStored, vResultArray:=vRiskGroups)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetRiskGroupsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRIskGroups", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAvailableDocs
    '
    ' Description: Get available docs for suppression
    '
    ' PW150702 - created
    '
    ' ***************************************************************** '
    Public Function GetAvailableDocs(Optional ByRef vDocs(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAvailableDocsSQL, sSQLName:=ACGetAvailableDocsName, bStoredProcedure:=ACGetAvailableDocsStored, vResultArray:=vDocs)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetAvailableDocsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAvailableDocs", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetSuppressedDocs
    '
    ' Description: Get available docs for suppression
    '
    ' PW150702 - created
    '
    ' ***************************************************************** '
    Public Function GetSuppressedDocs(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vDocs(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the party count parameter

            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(vPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetSuppressedDocsSQL, sSQLName:=ACGetSuppressedDocsName, bStoredProcedure:=ACGetSuppressedDocsStored, vResultArray:=vDocs)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetSuppressedDocsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSuppressedDocs", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetRelationshipTypeLookups
    '
    ' Description: Get relationship type lookups.
    '
    ' ***************************************************************** '
    Public Function GetRelationshipTypeLookups(ByRef vRelationships As Object) As Integer

        Dim result As Integer = 0
        Dim oSIRpartyAG As bSIRPartyAG.SIRPartyAG

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRpartyAG - need to hit core for address stuff
            oSIRpartyAG = New bSIRPartyAG.SIRPartyAG()

            m_lReturn = oSIRpartyAG.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            m_lReturn = oSIRpartyAG.bSIRParty.GetRelationshipTypeLookups(vRelationships:=vRelationships, lPartyRelationshipGroup:=ACPartyRelationshipGroupAgent)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRpartyAG = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetRelationshipTypeLookups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRelationshipTypeLookups", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: UpdateAssociates
    '
    ' Description: Update the party_Associate usage table with old
    ' and new Associates for the party.
    '
    ' ***************************************************************** '
    Public Function UpdateAssociates(ByRef vPartyCnt As Object, ByRef vAssociates As Object, Optional ByVal sUniqueId As String = "") As Integer

        Dim result As Integer = 0
        Dim oSIRpartyAG As bSIRPartyAG.SIRPartyAG

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRpartyAG - need to hit core for address updates
            oSIRpartyAG = New bSIRPartyAG.SIRPartyAG()

            m_lReturn = oSIRpartyAG.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            m_lReturn = oSIRpartyAG.bSIRParty.UpdateAssociates(vPartyCnt:=ToSafeInteger(m_lPartyCnt), vAssociates:=vAssociates, lPartyRelationshipGroupId:=ToSafeInteger(ACPartyRelationshipGroupAgent), sUniqueId:=ToSafeString(sUniqueId))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRpartyAG = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = RollbackTrans()

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateAssociates Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAssociates", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: GetAddressDetails
    '
    ' Description: Get address details for party.
    '
    ' ***************************************************************** '
    Public Function GetAddressDetails(ByRef vAddresses As Object) As Integer

        Dim result As Integer = 0
        Dim oSIRpartyAG As bSIRPartyAG.SIRPartyAG


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyAG - need to hit core for address stuff
            oSIRpartyAG = New bSIRPartyAG.SIRPartyAG()

            m_lReturn = oSIRpartyAG.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            m_lReturn = oSIRpartyAG.bSIRParty.GetAddressDetails(vPartyCnt:=ToSafeInteger(m_lPartyCnt), vAddresses:=vAddresses)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRpartyAG = Nothing


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetAddressDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAddressDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetAddressTypeLookups
    '
    ' Description: Get address type lookups.
    '
    ' ***************************************************************** '
    Public Function GetAddressTypeLookups(ByRef vAddressTypes As Object) As Integer

        Dim result As Integer = 0
        Dim oSIRpartyAG As bSIRPartyAG.SIRPartyAG


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyAG - need to hit core for address stuff
            oSIRpartyAG = New bSIRPartyAG.SIRPartyAG()

            m_lReturn = oSIRpartyAG.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            m_lReturn = oSIRpartyAG.bSIRParty.GetAddressTypeLookups(vAddressTypes:=vAddressTypes)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRpartyAG = Nothing


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetAddressTypeLookups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAddressTypeLookups", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetContactDetails
    '
    ' Description: Get contact details for party.
    '
    ' ***************************************************************** '
    Public Function GetContactDetails(ByRef vContacts As Object) As Integer

        Dim result As Integer = 0
        Dim oSIRpartyAG As bSIRPartyAG.SIRPartyAG


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyAG - need to hit core for address stuff
            oSIRpartyAG = New bSIRPartyAG.SIRPartyAG()

            m_lReturn = oSIRpartyAG.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            m_lReturn = oSIRpartyAG.bSIRParty.GetContactDetails(vPartyCnt:=ToSafeInteger(m_lPartyCnt), vContacts:=vContacts)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRpartyAG = Nothing


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetContactDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetContactDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: UpdateAddresses
    '
    ' Description: Update the party_address usage table with old
    ' and new addresses for the party.
    '
    ' ***************************************************************** '
    Public Function UpdateAddresses(ByRef vPartyCnt As Object, Optional ByRef vAddAddresses As Object = Nothing, Optional ByRef vDeleteAddresses As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRpartyAG As bSIRPartyAG.SIRPartyAG


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyAG - need to hit core for address updates
            oSIRpartyAG = New bSIRPartyAG.SIRPartyAG()

            m_lReturn = oSIRpartyAG.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            m_lReturn = oSIRpartyAG.bSIRParty.UpdateAddresses(vPartyCnt:=ToSafeInteger(m_lPartyCnt), vAddAddresses:=vAddAddresses, vDeleteAddresses:=vDeleteAddresses)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRpartyAG = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = RollbackTrans()


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateAddresses Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAddresses", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: UpdateContacts
    '
    ' Description: Update the party_contact usage table with old
    ' and new contacts for the party.
    '
    ' ***************************************************************** '
    Public Function UpdateContacts(ByRef vPartyCnt As Object, Optional ByRef vAddContacts As Object = Nothing, Optional ByRef vDeleteContacts As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRpartyAG As bSIRPartyAG.SIRPartyAG

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyAG - need to hit core for address updates
            oSIRpartyAG = New bSIRPartyAG.SIRPartyAG()

            m_lReturn = oSIRpartyAG.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            m_lReturn = oSIRpartyAG.bSIRParty.UpdateContacts(vPartyCnt:=ToSafeInteger(m_lPartyCnt), vAddContacts:=vAddContacts, vDeleteContacts:=vDeleteContacts)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRpartyAG = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = RollbackTrans()

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateContacts Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateContacts", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DC220803 -PS253 -fsa compliance
    ' ***************************************************************** '
    ' Name: UpdateRiskGroups
    '
    ' Description: Update the party_agent_risk_group table with old
    ' and new agent risk_groups
    '
    ' ***************************************************************** '
    Public Function UpdateRiskGroups(ByRef vPartyCnt As Object, Optional ByRef vNewRiskGroups As Object = Nothing, Optional ByRef vDelRiskGroups As Object = Nothing) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Informations.IsArray(vNewRiskGroups) Then


                For i As Integer = vNewRiskGroups.GetLowerBound(0) To vNewRiskGroups.GetUpperBound(0)

                    ' Clear the Database Parameters Collection
                    m_oDatabase.Parameters.Clear()

                    ' Add the party count parameter
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(m_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    ' Add the party count parameter

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_group_id", vValue:=CStr(CInt(vNewRiskGroups(i))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    ' Execute SQL Statement
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACAddAgentRiskGroupSQL, sSQLName:=ACAddAgentRiskGroupName, bStoredProcedure:=ACAddAgentRiskGroupStored)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Next i

            End If

            If Informations.IsArray(vDelRiskGroups) Then


                For i As Integer = vDelRiskGroups.GetLowerBound(0) To vDelRiskGroups.GetUpperBound(0)

                    ' Clear the Database Parameters Collection
                    m_oDatabase.Parameters.Clear()

                    ' Add the party count parameter
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(m_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    ' Add the party count parameter

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_group_id", vValue:=CStr(CInt(vDelRiskGroups(i))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    ' Execute SQL Statement
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACDeleteAgentRiskGroupSQL, sSQLName:=ACDeleteAgentRiskGroupName, bStoredProcedure:=ACDeleteAgentRiskGroupStored)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Next i

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = RollbackTrans()

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRisk Groups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRisk Groups", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    'Private Function CheckMandatory(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyAgentTypeID As Object = Nothing, Optional ByRef vPartyAgentOriginID As Object = Nothing, Optional ByRef vIsBranch As Object = Nothing, Optional ByRef vIsHeadOffice As Object = Nothing, Optional ByRef vAgencyAgreementDate As Object = Nothing, Optional ByRef vAgencyNextReviewDate As Object = Nothing, Optional ByRef vAgencyAccountNumber As Object = Nothing, Optional ByRef vDefaultCommissionPercent As Object = Nothing, Optional ByRef vTradingName As Object = Nothing, Optional ByRef vBinderIndicator As Object = Nothing, Optional ByRef vReportIndicator As Object = Nothing) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' {* USER DEFINED CODE (Begin) *}
    '


    'If (Informations.IsNothing(vPartyAgentOriginID)) Or (Object.Equals(vPartyAgentOriginID, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing

    '


    'If (Informations.IsNothing(vIsBranch)) Or (Object.Equals(vIsBranch, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing

    '


    'If (Informations.IsNothing(vIsHeadOffice)) Or (Object.Equals(vIsHeadOffice, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing

    '
    ' {* USER DEFINED CODE (End) *}
    '
    'Return result
    '
    'Catch excep As System.Exception
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
    ' Name: GetDefaultCountryCode
    '
    ' Description: Get Country Code (eg - 'GBR') from system home_country_id.
    '
    ' Author: RWH
    '
    ' CTAF 141200 - Changed to use vResultArray to get the result.
    '               This was stopping reports from working for some reason.
    '
    ' ***************************************************************** '

    Public Function GetDefaultCountryCode(ByVal v_iCountryID As Integer, ByRef r_sCountryCode As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Construct the SQL
            sSQL = "SELECT code FROM Country WHERE country_id = " & v_iCountryID

            ' Perform the query
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetDefaultCountryCode", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the result
            If Informations.IsArray(vResultArray) Then

                r_sCountryCode = CStr(vResultArray(0, 0)).Trim()
            Else
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            'Dim sSQL As String
            'Dim lRecordCount As Long
            'Dim oFields As ADODB.Fields
            '
            '    On Error GoTo Err_GetDefaultCountryCode
            '
            '    GetDefaultCountryCode = PMTrue
            '
            '    ' Clear the Database Parameters Collection
            '    m_oDatabase.Parameters.Clear
            '
            '    sSQL = "SELECT code " & vbCrLf & _
            ''            "FROM country " & vbCrLf & _
            ''            "WHERE country_id = " & v_iCountryID
            '
            '     ' Execute SQL Statement
            '    m_lReturn& = m_oDatabase.SQLSelect( _
            ''        sSQL:=sSQL, _
            ''        sSQLName:="GetDefaultCountryCode", _
            ''        bStoredProcedure:=False, _
            ''        lNumberRecords:=0)
            '
            '    If (m_lReturn& <> PMTrue) Then
            '        GetDefaultCountryCode = PMFalse
            '        Exit Function
            '    End If
            '
            '    ' How many records were selected
            '    lRecordCount& = m_oDatabase.Records.Count
            '
            '    ' Do we have any records ?
            '    If (lRecordCount& < 1) Then
            '        ' No Records, return PMFalse
            '        GetDefaultCountryCode = PMNotFound
            '        Exit Function
            '    End If
            '    Set oFields = m_oDatabase.Records.Item(1).Fields
            '    r_sCountryCode = Trim$(oFields.Item("code").Value)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaultCountryCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultCountryCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UnderwritingOrAgency
    '
    ' Description:  Finds out if Underwriting or Agency business
    '
    ' History:
    ' 06/06/2002 SP - moved to uniform Product Options scheme
    ' ***************************************************************** '
    Private Function getUnderwritingOrAgency() As Integer

        Dim result As Integer = 0



        Return bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_sUnderwritingOrAgency)

    End Function

    'SD 14/08/2002
    ' ***************************************************************** '
    ' Name: GetPartyNameFromShortName (Public)
    '
    ' Description: Returns a party's name given shortname (code).
    '
    ' ***************************************************************** '
    Public Function GetPartyNameFromShortName(ByVal v_sPartyShortname As String, ByRef r_sPartyName As String) As Integer

        Dim result As Integer = 0
        Dim sName As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the ID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="shortname", vValue:=v_sPartyShortname, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPartyNameFromShortnameSQL, sSQLName:=ACGetPartyNameFromShortnameName, bStoredProcedure:=ACGetPartyNameFromShortnameStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then

                r_sPartyName = CStr(vResultArray(0, 0)).Trim()
            Else
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyNameFromShortName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPartyCntFromBrokerAbiId
    '
    ' Description:
    '
    ' History: 02/03/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function GetPartyCntFromBrokerAbiId(ByVal v_sBrokerAbiId As String, Optional ByRef r_lPartyCnt As Integer = 0, Optional ByRef r_sTradingName As String = "") As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = bUnderwritingBranchFunc.GetPartyCntFromBrokerAbiId(v_sBrokerAbiId:=v_sBrokerAbiId, v_oDatabase:=m_oDatabase, v_sUsername:=m_sUsername, r_lPartyCnt:=r_lPartyCnt, r_sTradingName:=r_sTradingName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bUnderwritingBranchFunc.GetPartyCntFromBrokerAbiId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyCntFromBrokerAbiId")
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyCntFromBrokerAbiId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyCntFromBrokerAbiId", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '****************************************************************************
    ' Get values from specified columns (v_vReturnColumn)
    ' if v_sKeyColumn and v_sKeyValue is passed in then use it as criteria otherwise
    ' whole table is selected
    ' if v_vReturnColumn is not an array then single value is returned
    ' Note: v_lColumnType = PMLong, PMString etc
    '
    '****************************************************************************
    'Developer Guide No 101
    Public Function GetValueFromTable(ByVal v_sTableName As String, ByVal v_vReturnColumn As Object, ByVal v_sKeyColumn As String, ByVal v_sKeyValue As String, ByVal v_iDataType As Integer, ByRef r_vResult As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As New StringBuilder
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = New StringBuilder("SELECT DISTINCT ")

            If Informations.IsArray(v_vReturnColumn) Then

                For Each v_vReturnColumn_item As Object In v_vReturnColumn

                    sSQL.Append(CStr(v_vReturnColumn_item) & ",")
                Next v_vReturnColumn_item

                'get rid of last comma
                sSQL = New StringBuilder(sSQL.ToString().Substring(0, sSQL.ToString().Length - 1))

            Else
                sSQL.Append(v_vReturnColumn)
            End If

            sSQL.Append(Strings.ChrW(13) & Strings.ChrW(10) & " FROM " & v_sTableName & Strings.ChrW(13) & Strings.ChrW(10))

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
                        'Developer Guide No. 40
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CDate(v_sKeyValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)

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

            Return result
        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get value from " & v_sTableName & "." & v_sKeyColumn & " = " & v_sKeyValue, vApp:=ACApp, vClass:=ACClass, vMethod:="GetValueFromTable", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

            Return result

        Finally

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetAgencyUsers (Public)
    '
    ' Description: Gets the Users Associated with the Agent.
    '
    ' Module : SAGICOR WPR 14
    ' ***************************************************************** '

    Public Function GetAgencyUsers(ByVal v_lPartyCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Try

            GetAgencyUsers = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the party count parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=m_lPartyCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAgentUserSQL, sSQLName:=ACGetAgentUserName, bStoredProcedure:=ACGetAgentUserStored, vResultArray:=r_vResults)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                GetAgencyUsers = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            Exit Function

        Catch ex As Exception

            GetAgencyUsers = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetAgencyUsersFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAgencyUsers", vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description, excep:=ex)

            Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckAgentRegNumber (Public) - PN 21971
    '
    ' Description: Checks to see if the registration number is unique or not.
    '
    ' ***************************************************************** '
    Public Function CheckAgentRegNumber(ByRef vRegNumber As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the ID parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="RegNumber", vValue:=CStr(vRegNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckAgentRegNumberSQL, sSQLName:=ACCheckAgentRegNumberName, bStoredProcedure:=ACCheckAgentRegNumberStored, lNumberRecords:=0)

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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckAgentRegNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckAgentRegNumber", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPartyDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 19-08-2005 : 360 = Taxes on Claims
    ' ***************************************************************** '
    Public Function GetPartyDetails(ByVal v_lPartyCnt As Integer, ByRef r_vResults As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPartyDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue



            lReturn = m_oSIRParty.GetPartyDetails(v_lPartyCnt:=ToSafeInteger(v_lPartyCnt), r_vResults:=r_vResults)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetPartyDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result
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
    ' Name: UpdatePartyDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function UpdatePartyDetails(ByVal v_lPartyCnt As Integer, ByVal v_vPartyDetails As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdatePartyDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = m_oSIRParty.UpdatePartyDetails(v_lPartyCnt:=ToSafeInteger(v_lPartyCnt), v_vPartyDetails:=v_vPartyDetails)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "UpdatePartyDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result
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
    '
    ' Name: PickListLoad
    '
    ' Description: Standard method for the PickList control to call
    '
    ' History: 26/09/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function PickListLoad(ByVal sPickListType As String, ByVal vFKArray(,) As Object, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            With m_oDatabase
                .Parameters.Clear()

                'Load the parameters
                For iParam As Integer = vFKArray.GetLowerBound(1) To vFKArray.GetUpperBound(1)
                    m_lReturn = .Parameters.Add(sName:=CStr(vFKArray(0, iParam)), vValue:=CStr(vFKArray(1, iParam)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=CType(CInt(vFKArray(2, iParam)), gPMConstants.PMEDataType))
                Next iParam

                'Call the SP
                Select Case sPickListType.Trim.ToUpper
                    Case "SOURCE"
                        m_lReturn = .SQLSelect(sSQL:=ACGetAgencyUserSourceSQL, sSQLName:=ACGetAgencyUserSourcerName, bStoredProcedure:=ACGetAgencyUserSourceStored, vResultArray:=vResultArray)

                    Case "PRODUCT"
                        m_lReturn = .SQLSelect(sSQL:=ACGetAgencyUserProductSQL, sSQLName:=ACGetAgencyUserProductName, bStoredProcedure:=ACGetAgencyUserProductStored, vResultArray:=vResultArray)

                End Select
                'developers guide no. 39
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PickListLoad Select Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PickListLoad", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return m_lReturn
                End If
            End With


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PickListLoad Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PickListLoad", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: PickListParams
    '
    ' Description: Returns a string of question marks for the SP definition
    '
    ' History: 26/09/2001 DD - Created.
    '
    ' ***************************************************************** '
    Private Function PickListParams(ByRef vParams(,) As Object) As String

        Dim result As String = String.Empty


        Dim sComma As String = ""
        Dim sParam As New StringBuilder

        sComma = ""
        sParam = New StringBuilder("")
        For iParam As Integer = vParams.GetLowerBound(1) To vParams.GetUpperBound(1)
            sParam.Append(sComma & "?")
            sComma = ","
        Next iParam


        Return sParam.ToString()

    End Function

    Public Function PickListSave(ByRef sPickListType As String, ByRef vFKArray(,) As Object, ByRef vKeys As Object) As Integer

        Dim result As Integer = 0

        Try

            BeginTrans()

            If vFKArray.GetUpperBound(1) > 2 And sPickListType.Trim().ToUpper() = "SOURCE" Then
                'ReDim Preserve vFKArray(vFKArray.GetUpperBound(0), vFKArray.GetUpperBound(1) - 1)
            End If

            With m_oDatabase

                'clear the old data
                .Parameters.Clear()

                'Load the FK parameters
                For iParam As Integer = vFKArray.GetLowerBound(1) To vFKArray.GetUpperBound(1)


                    .Parameters.Add(sName:=CStr(vFKArray(0, iParam)), vValue:=CStr(vFKArray(1, iParam)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=vFKArray(2, iParam))
                Next iParam
                'Developer Guide No. 39
                'm_lReturn = .SQLAction("spe_Agent_PLDSource", sPickListType & " PickList Delete", True)
                Select Case sPickListType.Trim.ToUpper
                    Case "SOURCE"

                        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteSourceIDforAgentSQL, sSQLName:=ACDeleteSourceIDforAgentName, bStoredProcedure:=True)

                    Case "PRODUCT"
                        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteProductIDforAgentSQL, sSQLName:=ACDeleteProductIDforAgentName, bStoredProcedure:=True)

                End Select
                'See if there is anything to save
                If Informations.IsArray(vKeys) Then

                    For iKey As Integer = vKeys.GetLowerBound(0) To vKeys.GetUpperBound(0)
                        .Parameters.Clear()
                        .Parameters.Add(sName:=CStr(vFKArray(0, 0)), vValue:=CStr(vFKArray(1, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        'Call the SP
                        'Developer Guide No. 39
                        'm_lReturn = .SQLAction("spe_Agent_PLSSource", sPickListType & " PickList Load", True)
                        Select Case sPickListType.Trim.ToUpper
                            Case "SOURCE"
                                .Parameters.Add("Branchid", vKeys(iKey), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                                .Parameters.Add(CStr(vFKArray(0, 2)), CStr(vFKArray(1, 2)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                                .Parameters.Add(CStr(vFKArray(0, 3)), CStr(vFKArray(1, 3)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                                .Parameters.Add(CStr(vFKArray(0, 4)), CStr(vFKArray(1, 4)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACSaveSourceIDforAgentSQL, sSQLName:=ACSaveSourceIDforAgentName, bStoredProcedure:=True)

                            Case "PRODUCT"
                                .Parameters.Add("Product_id", vKeys(iKey), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                                .Parameters.Add(CStr(vFKArray(0, 2)), CStr(vFKArray(1, 2)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                                .Parameters.Add(CStr(vFKArray(0, 3)), CStr(vFKArray(1, 3)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                                .Parameters.Add(CStr(vFKArray(0, 4)), CStr(vFKArray(1, 4)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACSaveProductIDforAgentSQL, sSQLName:=ACSaveProductIDforAgentName, bStoredProcedure:=True)

                        End Select
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Log Error Message
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PickListSave Write Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PickListSave", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                            RollbackTrans()
                            Return m_lReturn
                        End If
                    Next iKey
                End If
            End With

            With m_oDatabase

                'clear the old data
                .Parameters.Clear()

                .Parameters.Add(sName:=CStr(vFKArray(0, 0)), vValue:=CStr(vFKArray(1, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLAction("spu_Update_Agent_User_Source", sPickListType & " PickList Save", True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PickListSave Write Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PickListSave", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    RollbackTrans()
                    Return m_lReturn
                End If


            End With


            CommitTrans()

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PickListSave Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PickListSave", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            RollbackTrans()
            Return result

        End Try
    End Function


    Public Function ImportAgents(ByRef vAgentArray(,) As Object, Optional ByRef r_iRecordsEffectred As Integer = 0, Optional ByRef r_sErrorMsg As String = "") As Integer
        Dim result As Integer = 0
        Dim iBranchID_old As Integer
        'Dim r_sErrorMsg As String

        Dim vNewContacts(0) As Object
        Dim sEncrypt As String = ""
        Dim m_iuser As Integer
        Dim m_lContactCnt As Integer
        Dim vResultArray As Object = Nothing
        Dim m_lAddressCnt As Integer
        Dim sSQL As String = ""
        Dim vNewAddresses(1, 0) As Object
        Dim oSIROrionUpdate As Object = Nothing

        Dim lPartyCnt As Integer

        Try
            m_bImportData = True

            iBranchID_old = 0

            result = gPMConstants.PMEReturnCode.PMTrue
            If Informations.IsArray(vAgentArray) Then
                For lvar As Integer = 0 To vAgentArray.GetUpperBound(1)

                    m_sAgentCode = CStr(vAgentArray(AgentDetails.ADCode, lvar))

                    m_iAgentBranchID = CInt(vAgentArray(AgentDetails.ADBranchID, lvar))

                    m_sBranchName = CStr(vAgentArray(AgentDetails.ADBranchName, lvar))

                    m_sSchemeName = CStr(vAgentArray(AgentDetails.ADSchemeName, lvar))

                    m_sAgentName = CStr(vAgentArray(AgentDetails.ADAgentName, lvar))

                    m_sPostCode = CStr(vAgentArray(AgentDetails.ADPostcode, lvar))

                    m_sAddress1 = CStr(vAgentArray(AgentDetails.ADAddress1, lvar))

                    m_sAddress2 = CStr(vAgentArray(AgentDetails.ADAddress2, lvar))

                    m_sAddress3 = CStr(vAgentArray(AgentDetails.ADAddress3, lvar))

                    m_sAddress4 = CStr(vAgentArray(AgentDetails.ADAddress4, lvar))

                    m_sTelephone = CStr(vAgentArray(AgentDetails.ADTelephone, lvar))

                    m_sAgentUserName = CStr(vAgentArray(AgentDetails.ADUserName, lvar))

                    m_sAgentPassword = CStr(vAgentArray(AgentDetails.ADPassword, lvar))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        r_sErrorMsg = "Failed to Init"
                        Throw New Exception(r_sErrorMsg)
                    End If

                    If iBranchID_old = 0 Then
                        m_lReturn = GetDefaultInformation()
                        iBranchID_old = m_iAgentBranchID ' BranchID / SourceID
                    End If
                    If iBranchID_old <> m_iAgentBranchID Then
                        m_lReturn = GetDefaultInformation()
                        iBranchID_old = m_iAgentBranchID
                    End If
                    m_lReturn = Init()

                    m_lReturn = IsAgentExists(m_sAgentCode, vResultArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        r_sErrorMsg = "IsAgentExists Failed"
                        Throw New Exception(r_sErrorMsg)
                    End If
                    If Informations.IsArray(vResultArray) Then

                    Else
                        lPartyCnt = 0
                        'Add Details to Party Table
                        r_iRecordsEffectred += 1
                        m_lReturn = EditAdd(lRow:=1, vPartyCnt:=lPartyCnt, vPartyAgentTypeID:=m_iPartyTypeID, vPartyAgentOriginID:=m_iPartyOriginId, vShortName:=m_sAgentCode, vName:=m_sAgentName, vResolvedName:=m_sAgentName, vTradingName:=m_sAgentName, vAddressOnNotice:=m_iAddressUsageTypeID, vCurrencyId:=m_iCurrencyID)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            r_sErrorMsg = "Agent EditAdd Failed"
                            Throw New Exception(r_sErrorMsg)
                        End If

                        m_lReturn = Update()
                        lPartyCnt = m_lPartyCnt

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            r_sErrorMsg = "Agent Update Failed"
                            Throw New Exception(r_sErrorMsg)
                        End If


                        m_lReturn = m_oPartyAGAddress.EditAdd(lRow:=1, vAddressCnt:=0, vAddress1:=m_sAddress1, vAddress2:=m_sAddress2, vAddress3:=m_sAddress3, vAddress4:=m_sAddress4, vPostalCode:=m_sPostCode, vCountryID:=m_iAgentCountryId)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            r_sErrorMsg = "Address EditAdd Failed"
                            Throw New Exception(r_sErrorMsg)
                        End If


                        m_lReturn = m_oPartyAGAddress.Update()

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            r_sErrorMsg = "Address Update Failed"
                            Throw New Exception(r_sErrorMsg)
                        End If


                        m_lAddressCnt = m_oPartyAGAddress.AddressCnt


                        vNewAddresses(0, 0) = m_lAddressCnt

                        vNewAddresses(1, 0) = 4

                        m_lReturn = UpdateAddresses(vPartyCnt:=m_lPartyCnt, vAddAddresses:=vNewAddresses)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            r_sErrorMsg = "UpdateAddresses Failed"
                            Throw New Exception(r_sErrorMsg)
                        End If


                        m_lReturn = m_oPartyContact.EditAdd(lRow:=1, vContactCnt:=m_lContactCnt, vContactTypeID:=m_iContactTypeId, vNumber:=m_sTelephone)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            r_sErrorMsg = "m_oPartyContact.EditAdd Failed"
                            Throw New Exception(r_sErrorMsg)
                        End If



                        m_lReturn = m_oPartyContact.Update

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            r_sErrorMsg = "m_oPartyContact.Update Failed"
                            Throw New Exception(r_sErrorMsg)
                        End If


                        m_lContactCnt = m_oPartyContact.ContactCnt

                        '            'Update Source ID
                        If m_iAgentBranchID <> m_iSourceID Then
                            'Update the Agent Source id
                            m_oDatabase.Parameters.Clear()
                            If m_oDatabase.Parameters.Add("Source_ID", CStr(m_iAgentBranchID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then
                                r_sErrorMsg = "Could Not Add BranchID Parameter"
                                Throw New Exception(r_sErrorMsg)
                            End If

                            If m_oDatabase.Parameters.Add("Party_cnt", CStr(m_lPartyCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                                r_sErrorMsg = "Could Not Add BranchID Parameter"
                                Throw New Exception(r_sErrorMsg)
                            End If

                            If m_oDatabase.Parameters.Add("Address_cnt", CStr(m_lAddressCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                                r_sErrorMsg = "Could Not Add BranchID Parameter"
                                Throw New Exception(r_sErrorMsg)
                            End If

                            If m_oDatabase.Parameters.Add("Contact_cnt", CStr(m_lContactCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                                r_sErrorMsg = "Could Not Add BranchID Parameter"
                                Throw New Exception(r_sErrorMsg)
                            End If
                            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateSourceIDforAgentImportSQL, sSQLName:=ACUpdateSourceIDforAgentImportName, bStoredProcedure:=True)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                r_sErrorMsg = "Update SourceID Failed"
                                Throw New Exception(r_sErrorMsg)
                            End If
                        End If



                        vNewContacts(0) = m_lContactCnt
                        m_lReturn = UpdateContacts(vPartyCnt:=m_lPartyCnt, vAddContacts:=vNewContacts)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            r_sErrorMsg = "UpdateAddresses Failed"
                            Throw New Exception(r_sErrorMsg)
                        End If


                        m_oDatabase.Parameters.Clear()
                        If m_oDatabase.Parameters.Add("Party_cnt", CStr(m_lPartyCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                            r_sErrorMsg = "Could Not Add PartyCnt Parameter"
                            Throw New Exception(r_sErrorMsg)
                        End If

                        If m_oDatabase.Parameters.Add("BranchID", CStr(m_iAgentBranchID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then
                            r_sErrorMsg = "Could Not Add BranchID Parameter"
                            Throw New Exception(r_sErrorMsg)

                        End If
                        m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_Party_agent_Branch_Add", sSQLName:="spu_Party_agent_Branch_Add", bStoredProcedure:=True)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            r_sErrorMsg = "spu_Party_agent_Branch_Add Failed"
                            Throw New Exception(r_sErrorMsg)
                        End If

                        If m_iAgentBranchID <> 1 Then
                            m_oDatabase.Parameters.Clear()
                            If m_oDatabase.Parameters.Add("Party_cnt", CStr(m_lPartyCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                                r_sErrorMsg = "Could Not Add PartyCnt Parameter"
                                Throw New Exception(r_sErrorMsg)
                            End If

                            If m_oDatabase.Parameters.Add("BranchID", CStr(1), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then
                                r_sErrorMsg = "Could Not Add BranchID Parameter"
                                Throw New Exception(r_sErrorMsg)

                            End If
                            m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_Party_agent_Branch_Add", sSQLName:="spu_Party_agent_Branch_Add", bStoredProcedure:=True)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                r_sErrorMsg = "spu_Party_agent_Branch_Add Failed"
                                Throw New Exception(r_sErrorMsg)
                            End If
                        End If



                        m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oSIROrionUpdate, v_sClassName:="bSIROrionUpdate.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iAgentBranchID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iAgentCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        m_lReturn = oSIROrionUpdate.SiriusToOrion(v_lPartyCnt:=ToSafeInteger(m_lPartyCnt))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            r_sErrorMsg = "m_oSIROrionUpdate.SiriusToOrion Failed"
                            Throw New Exception(r_sErrorMsg)
                        End If


                        m_lReturn = bPMFunc.Encrypt(m_sAgentPassword, sEncrypt)


                        m_lReturn = m_oPMUser.Add(iUserID:=m_iuser, iLanguageID:=m_iLanguageID, sUsername:=m_sAgentUserName, sPassword:=sEncrypt, dtPasswordChangeDate:=DateTime.Now, lPartyCnt:=m_lPartyCnt, dtDateCreated:=DateTime.Now, dtLastlogin:=DateTime.Now, iIsDeleted:=0, dtEffectiveDate:=DateTime.Now, vFullName:=m_sAgentUserName, vTelephoneNumber:=m_sTelephone)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            r_sErrorMsg = "m_oPMUser.Add Failed"
                            Throw New Exception(r_sErrorMsg)
                        End If


                        m_lReturn = m_oPMUser.Update

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            r_sErrorMsg = "m_oPMUser.Update Failed"
                            Throw New Exception(r_sErrorMsg)
                        End If

                    End If
                    m_oPartyContact = Nothing
                    m_oPMUser = Nothing
                    oSIROrionUpdate = Nothing
                    m_oPartyAGAddress = Nothing


                Next
            End If

            Return result
        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ImportAgent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportAgent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description & "." & r_sErrorMsg, excep:=ex)


        Finally

        End Try
        Return result
    End Function

    Private Function GetDefaultInformation() As Integer
        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue
        Dim sErrMsg As String = ""
        Dim vResult(,) As Object = Nothing


        m_lReturn = GetSourceInfo(m_iAgentBranchID, vResult)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            sErrMsg = "GetSourceInfo Failed."
            Throw New Exception(sErrMsg)
        End If

        If Informations.IsArray(vResult) Then

            m_iAgentCurrencyID = CInt(vResult(0, 0))

            m_iAgentCountryId = CInt(vResult(1, 0))
            m_iCurrencyID = m_iAgentCurrencyID
        End If


        vResult = Nothing

        m_lReturn = GetPartyOriginID(vResult)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            sErrMsg = "GetPartyOriginId Failed."
            Throw New Exception(sErrMsg)
        End If

        If Informations.IsArray(vResult) Then

            m_iPartyOriginId = CInt(vResult(0, 0))
        End If

        vResult = Nothing

        'Get Party Type ID
        m_lReturn = GetPartyTypeIDForAgent(vResult)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            sErrMsg = "GetPartyTypeIdForAgent Failed."
            Throw New Exception(sErrMsg)
        End If

        If Informations.IsArray(vResult) Then

            m_iPartyTypeID = CInt(vResult(0, 0))
        End If

        vResult = Nothing

        'Get Address Usage Type ID
        m_lReturn = GetCorrespondenceAddressUsageTypeID(vResult)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            sErrMsg = "GetCorrespondanceAddressUsageTypeID Failed."
            Throw New Exception(sErrMsg)
        End If

        If Informations.IsArray(vResult) Then

            m_iAddressUsageTypeID = CInt(vResult(0, 0))
        End If

        vResult = Nothing

        'Get Contact Type ID
        m_lReturn = GetContactTypeID(vResult)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            sErrMsg = "GetContactTypeID Failed."
            Throw New Exception(sErrMsg)
        End If
        If Informations.IsArray(vResult) Then

            m_iContactTypeId = CInt(vResult(0, 0))
        End If

        vResult = Nothing

        Return result
    End Function
    Public Function GetSourceInfo(ByVal iBranchID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oDatabase.Parameters.Add("BranchId", CStr(iBranchID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetSourceInfoSQL, sSQLName:=ACGetSourceInfoName, bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSourceInfo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSourceInfo", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function GetPartyOriginID(ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPartyAgentOriginSQL, sSQLName:=ACGetPartyAgentOriginName, bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyOriginID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyOriginID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function GetPartyTypeIDForAgent(ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPartyTypeIDForAgentSQL, sSQLName:=ACGetPartyTypeIDForAgentName, bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyTypeIDForAgent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyTypeIDForAgent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function GetContactTypeID(ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetContactTypeIDSQL, sSQLName:=ACGetContactTypeIDName, bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetContactTypeID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetContactTypeID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function GetCorrespondenceAddressUsageTypeID(ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCorrespondenceAddressUsageTypeIDSQL, sSQLName:=ACGetCorrespondenceAddressUsageTypeIDName, bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCorrespondenceAddressUsageTypeID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCorrespondenceAddressUsageTypeID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function IsAgentExists(ByVal Code As String, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oDatabase.Parameters.Add("Code", Code, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetIsAgentExistsSQL, sSQLName:=ACGetIsAgentExistsName, bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsAgentExists Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsAgentExists", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    Private Function Init() As Integer
        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue


        m_oPartyAGAddress = New bSIRAddress.Business
        m_lReturn = m_oPartyAGAddress.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iAgentBranchID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iAgentCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_oPartyContact = New bsircontact.Business
        m_lReturn = m_oPartyContact.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iAgentBranchID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iAgentCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_oPMUser = New Bpmuser.Business
        m_lReturn = m_oPMUser.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iAgentBranchID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iAgentCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    Public Function GetCertificateYearDetails(ByRef vCertYear(,) As Object, Optional ByVal vPartyCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the party count parameter

            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(vPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCertYearSQL, sSQLName:=ACGetCertYearName, bStoredProcedure:=ACGetCertYearStored, vResultArray:=vCertYear)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetGetCertYearFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSuppressedDocs", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function UpdateCertYear(ByRef vPartyCnt As Object, Optional ByRef vUpdateCertYear As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = BeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Delete old CertYear for party if supplied

            If Not Informations.IsNothing(vUpdateCertYear) Then

                For i As Integer = vUpdateCertYear.GetLowerBound(1) To vUpdateCertYear.GetUpperBound(1)

                    If vUpdateCertYear(0, i).ToString.Trim <> "" Then
                        m_oDatabase.Parameters.Clear()

                        ' Add the party count parameter
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(vPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="Code", vValue:=CStr(vUpdateCertYear(0, i).ToString.Trim), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="Description", vValue:=CStr(vUpdateCertYear(1, i).ToString.Trim), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="StartDate", vValue:=CStr(vUpdateCertYear(2, i).ToString.Trim), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="EndDate", vValue:=CStr(vUpdateCertYear(3, i).ToString.Trim), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="UpdateStatus", vValue:=CStr(vUpdateCertYear(4, i).ToString.Trim), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                        ' Execute SQL Statement
                        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateCertYearSQL, sSQLName:=ACUpdateCertYearName, bStoredProcedure:=ACUpdateCertYearStored)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = RollbackTrans()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                Next i
            End If


            m_lReturn = CommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = RollbackTrans()


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateCertYearFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateCertYear", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Function GetAndValidateSubAgentDetails(ByVal v_sPartyCode As String, ByVal v_dtCoverStartDate As Date, ByVal v_dtCoverEndDate As Date, ByRef r_bIsValid As Boolean) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_code", vValue:=CStr(v_sPartyCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="CoverStartDate", vValue:=CStr(v_dtCoverStartDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="CoverEndDate", vValue:=CStr(v_dtCoverEndDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetSubAgentDetailSQL, sSQLName:=ACGetSubAgentDetailName, bStoredProcedure:=ACGetSubAgentDetailStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If vResultArray IsNot Nothing AndAlso Informations.IsArray(vResultArray) Then
                If CStr(vResultArray(0, 0)).Trim() = "VALID" Then
                    r_bIsValid = True
                Else
                    r_bIsValid = False
                End If
            Else
                r_bIsValid = True
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSubAgents Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSubAgents", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Function GetBankAccountForCurrency(
                      ByVal v_lCurrencyId As Integer,
                      ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the PartyCnt parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="currency_id", vValue:=v_lCurrencyId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetBankAccountForCurrencySQL, sSQLName:=ACGetBankAccountForCurrencyName, bStoredProcedure:=ACGetBankAccountForCurrencyStored, vResultArray:=r_vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result


        Catch excp As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetBankAccountForCurrency", r_lFunctionReturn:=result, excep:=excp)
            Return result
        End Try
    End Function

    Public Function GetCommissionLevel(ByRef vPartyCnt As Object, ByRef vCommissionLevel(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'DC 29/03/00
            'Cater for more than one Associate

            sSQL = "Select acl.commission_level_id, cl.description,acl.effective_date, acl.is_deleted, acl.Agent_Commission_Level_Id " &
                    "From agent_Commission_level acl " &
                    "Inner Join Commission_level cl on cl.commission_level_id = acl.commission_level_id " &
                    "Where acl.party_agent_cnt = " & CStr(CInt(vPartyCnt))


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetCommissionLevel", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            vCommissionLevel = vResultArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetCommissionLevelFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCommissionLevel", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function SetCommissionLevel(ByVal v_iAutoID As Integer, ByVal v_iPartyAgentCnt As Integer,
                                       ByVal v_iCommissionLevelId As Integer, ByVal v_dEffectiveDate As Date, ByVal v_bIsDeleted As Boolean, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add Party Code as an INPUT param for an update
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Agent_Commission_Level_Id", vValue:=v_iAutoID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add Party Code as an INPUT param for an update
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Party_Agent_Cnt", vValue:=v_iPartyAgentCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Commission_Level_Id", vValue:=v_iCommissionLevelId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Efffective_Date", vValue:=v_dEffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Is_deleted", vValue:=v_bIsDeleted, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="unique_Id", vValue:=v_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="screen_hierarchy", vValue:=v_sScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateCommissionLevelSQL, sSQLName:=ACUpdateCommissionLevelName, bStoredProcedure:=ACUpdateCommissionLevelStored, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetCommissionLevelFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetCommissionLevel", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try


    End Function

    Public Function DeleteAddress(ByVal party_cnt As Integer, ByVal address_cnt As Integer) As Integer

        Dim m_lReturn As Integer = gPMConstants.PMEReturnCode.PMTrue

        Try
            With m_oDatabase

                .Parameters.Clear()
                'Developer Guide No. 40
                .Parameters.Add("is_deleted", 1, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                .Parameters.Add("party_cnt", party_cnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                .Parameters.Add("address_cnt", address_cnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLAction(sSQL:=ACDelAddressSQL, sSQLName:=ACDelAddressName, bStoredProcedure:=ACDelAddressStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                Return m_lReturn
            End With
        Catch excep As System.Exception

            m_lReturn = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete Addresses Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAddress", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return m_lReturn
        End Try
    End Function

End Class
