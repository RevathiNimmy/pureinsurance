Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
Imports System.Text
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 12/10/1998
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a SIRPartyCC.
    '
    ' Edit History:
    ' DJM 22/04/2002 : MainContactCnt changed from a int to a long.
    ' RAW 18/11/2002 : PS005 : Added loyalty scheme
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

    ' Collection of SIRPartyCCs (Private)
    Private m_oSIRPartyCCs As bSIRPartyCC.SIRPartyCCs

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Object

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Primary Keys to work with
    Private m_lPartyCnt As Object

    Private m_bEvent As Boolean
    'eck150500
    Public g_oGIS As Object

    'Addresses - needed for events.
    Private m_vOldAddress(,) As Object
    Private m_vNewAddress(,) As Object
    Private m_sOldClientCode As String = ""
    Private m_sNewClientCode As String = ""
    Private m_sOldClientName As String = ""
    Private m_sNewClientName As String = ""

    ' PM Lookup Business Component (Private)
    Private m_oLookup As BPMLOOKUP.Business

    ' PM Event Business Component (Private)
    Private m_oEvent As bSIREvent.Business
    'Private m_oEvent As bSIREvent.Business

    Private m_bIsOrionInstalled As Boolean
    Private m_sUnderwritingOrAgency As String = ""

    Private m_oSIRParty As bSIRParty.Business


    ' PRIVATE Data Members (End)
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
                    m_lCurrentRecord = gPMConstants.PMEReturnCode.PMFalse
                Case Is > m_oSIRPartyCCs.Count()
                    m_lCurrentRecord = m_oSIRPartyCCs.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Number in Collection
            Return m_oSIRPartyCCs.Count()

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

    Public Property FromEvent() As Boolean
        Get

            Return m_bEvent

        End Get
        Set(ByVal Value As Boolean)

            m_bEvent = Value

        End Set
    End Property

    Public Property OldAddress() As Object
        Get
            Return m_vOldAddress.Clone
        End Get
        Set(ByVal Value As Object)
            m_vOldAddress = Value
        End Set
    End Property

    Public Property NewAddress() As Object
        Get
            Return m_vNewAddress.Clone
        End Get
        Set(ByVal Value As Object)
            m_vNewAddress = Value
        End Set
    End Property

    Public Property OldClientCode() As String
        Get
            Return m_sOldClientCode
        End Get
        Set(ByVal Value As String)
            m_sOldClientCode = Value
        End Set
    End Property

    Public Property NewClientCode() As String
        Get
            Return m_sNewClientCode
        End Get
        Set(ByVal Value As String)
            m_sNewClientCode = Value
        End Set
    End Property

    Public Property OldClientName() As String
        Get
            Return m_sOldClientName
        End Get
        Set(ByVal Value As String)
            m_sOldClientName = Value
        End Set
    End Property

    Public Property NewClientName() As String
        Get
            Return m_sNewClientName
        End Get
        Set(ByVal Value As String)
            m_sNewClientName = Value
        End Set
    End Property

    Public Property IsOrionInstalled() As Boolean
        Get

            Return m_bIsOrionInstalled

        End Get
        Set(ByVal Value As Boolean)

            m_bIsOrionInstalled = Value

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




            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '***************************************

            m_oSIRParty = New bSIRParty.Business
            m_lReturn = m_oSIRParty.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '***************************************


            m_lReturn = gPMComponentServices.CheckPMProductInstalled(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, r_bInstalled:=m_bIsOrionInstalled)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            m_lCurrentRecord = gPMConstants.PMEReturnCode.PMFalse
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create SIRPartyCCs Collection
            m_oSIRPartyCCs = New bSIRPartyCC.SIRPartyCCs()

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
                If m_oEvent IsNot Nothing Then
                    m_oEvent.Dispose()
                    m_oEvent = Nothing
                End If
                m_oSIRParty = Nothing
                m_oSIRPartyCCs = Nothing
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

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    ' Description: Gets the Lookup values for a SIRPartyCC.
    ' ***************************************************************** '
    'developer guide no. 
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyCC As bSIRPartyCC.SIRPartyCC = Nothing
        Dim dtEffectiveDate As Date
        Dim vTabArray(4, 11) As Object
        Dim vAreaId As Object = Nothing
        Dim vCurrencyId As Object = Nothing
        Dim vReminderTypeId As Object = Nothing
        Dim vServiceLevelId As Object = Nothing
        Dim vProspectStatusID As Object = Nothing
        Dim vPolicyTypeID As Object = Nothing
        Dim vSeasonalGiftID As Object = ""
        Dim vStrengthCodeId As String = ""
        Dim vSICCodeId As String = ""
        Dim vRenewalStopCodeId As Object = Nothing
        Dim vTermsOfPaymentId As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            vResultArray = Nothing
            ' Reset Table Array
            vTableArray = Nothing

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = gSIRLibrary.SIRLookupArea
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 1) = gSIRLibrary.SIRLookupCurrency
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 2) = gSIRLibrary.SIRLookupReminderType
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 3) = gSIRLibrary.SIRLookupServiceLevel
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 4) = gSIRLibrary.SIRLookupProspectStatus
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 5) = gSIRLibrary.SIRLookupRiskGroup
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 6) = gSIRLibrary.SIRLookupSeasonalGift
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 7) = gSIRLibrary.SIRLookupStrengthCode
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 8) = gSIRLibrary.SIRLookupSICCode
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 9) = gSIRLibrary.SIRLookupRenewalStopCode
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 10) = gSIRLibrary.SIRLookupBlackListReason
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 11) = gSIRLibrary.SIRLookupPFFrequency
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupWhereClause, 11) = "is_available_on_client_screen = 1"

            ' Do we have any records
            If m_lCurrentRecord < 1 Then
                ' No, we can only lookup all
                iLookupType = gPMConstants.PMELookupType.PMLookupAll
            Else
                ' Yes get current record
                oSIRPartyCC = m_oSIRPartyCCs.Item(m_lCurrentRecord)
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
                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 8) = ""
                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 9) = ""
                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 10) = ""
                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 11) = ""

                Case gPMConstants.PMELookupType.PMLookupAllEffective
                    ' Use keys and effective date from current record
                    ' Note: The keys are not used for the select, but are used by
                    '       the interface program to set the list index.
                    With oSIRPartyCC

                        m_lReturn = .bSIRParty.GetLookupProperties(vCurrentRecord:=m_lCurrentRecord, vAreaId:=vAreaId, vCurrencyId:=vCurrencyId, vReminderTypeId:=vReminderTypeId, vServiceLevelId:=vServiceLevelId, vSeasonalGiftID:=vSeasonalGiftID, vCorrespondenceTypeId:=Nothing, vRenewalStopCodeId:=vRenewalStopCodeId, vPaymentTermCode:=vTermsOfPaymentId)
                        m_lReturn = .GetProperties(iStatus:=gPMConstants.PMEComponentAction.PMView, vSICCodeId:=vSICCodeId)

                        If Convert.IsDBNull(vSeasonalGiftID) Or Informations.IsNothing(vSeasonalGiftID) Then
                            vSeasonalGiftID = ""
                        End If

                        vStrengthCodeId = ""

                        If Convert.IsDBNull(vSICCodeId) Or Informations.IsNothing(vSICCodeId) Then
                            vSICCodeId = ""
                        End If

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = vAreaId
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = vCurrencyId
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 2) = vReminderTypeId
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 3) = vServiceLevelId
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 4) = vProspectStatusID
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 5) = vPolicyTypeID
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 6) = vSeasonalGiftID
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 7) = vStrengthCodeId
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 8) = vSICCodeId
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 9) = vRenewalStopCodeId
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 10) = ""
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 11) = gPMFunctions.ToSafeLong(vTermsOfPaymentId, 0)


                    End With

                Case gPMConstants.PMELookupType.PMLookupSingle

                    ' Set keys from current record
                    With oSIRPartyCC

                        m_lReturn = .bSIRParty.GetLookupProperties(vCurrentRecord:=m_lCurrentRecord, vAreaId:=vAreaId, vCurrencyId:=vCurrencyId, vReminderTypeId:=vReminderTypeId, vServiceLevelId:=vServiceLevelId, vSeasonalGiftID:=vSeasonalGiftID, vCorrespondenceTypeId:=Nothing, vRenewalStopCodeId:=vRenewalStopCodeId, vPaymentTermCode:=vTermsOfPaymentId)

                        m_lReturn = .GetProperties(iStatus:=gPMConstants.PMEComponentAction.PMView, vSICCodeId:=vSICCodeId)

                        If Convert.IsDBNull(vSeasonalGiftID) Or Informations.IsNothing(vSeasonalGiftID) Then
                            'Tracy Richards 22/08/03 - Changed from "" to 0
                            vSeasonalGiftID = CStr(0)
                        End If
                        'Tracy Richards 22/08/03 - Changed from "" to 0
                        vStrengthCodeId = CStr(0)

                        If Convert.IsDBNull(vSICCodeId) Or Informations.IsNothing(vSICCodeId) Then
                            'Tracy Richards 22/08/03 - Changed from "" to 0
                            vSICCodeId = CStr(0)
                        End If

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = vAreaId
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = vCurrencyId
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 2) = vReminderTypeId
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 3) = vServiceLevelId
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 4) = 2 'vProspectStatusID
                        'Tracy Richards 22/08/03 - vSICCodeId is never populated so just pass in 0
                        'get's rid of "Key Must be supplied at position 5" error
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 5) = 0 'vPolicyTypeID
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 6) = vSeasonalGiftID
                        'DC 26/06/00 Added StrengthCodeId as it was missing
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 7) = vStrengthCodeId
                        'DC 26/06/00 Was incorrectly set to 7 now 8
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 8) = vSICCodeId
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 9) = vRenewalStopCodeId
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 10) = ""
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 11) = gPMFunctions.ToSafeLong(vTermsOfPaymentId, 0)
                    End With

            End Select

            ' Default Effective Date to current date/time
            dtEffectiveDate = DateTime.Now

            ' Release SIRPartyCC reference
            oSIRPartyCC = Nothing

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
    ' Name: CheckReference (Public)
    '
    ' Description: Checks if the passed shortname (reference) already exists
    '
    ' ***************************************************************** '
    Public Function CheckReference(ByRef sReference As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CheckReference"

        Dim oParty As bSIRParty.Business
        Dim bExists As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Create an instance of the main party business object
            oParty = New bSIRParty.Business()
            m_lReturn = oParty.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

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

            '		Return result

            ' This is for debugging only
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function

    ''' <summary>
    ''' Get latest policy version details   
    ''' </summary>
    ''' <param name="v_lParty_cnt"></param>
    ''' <param name="r_vResultArray"></param>
    ''' <returns></returns>
    ''' <remarks>WPR 3</remarks>
    Public Function GetPartyPolicies(ByVal v_lParty_cnt As Long, ByRef r_vResultArray As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPartyPolicies"

        Dim oParty As bSIRParty.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Create an instance of the main party business object
            oParty = New bSIRParty.Business()
            m_lReturn = oParty.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oParty.Initialise", "oParty = bSIRParty.Business", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = oParty.GetPartyPolicies(v_lParty_cnt:=v_lParty_cnt, r_vResultArray:=r_vResultArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oParty.GetPartyPolicies", "r_vResultArray:=" & r_vResultArray, gPMConstants.PMELogLevel.PMLogError)
            End If
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

            ' If you want to rollback a transaction or something, do it here

        Finally
        End Try
        Return result
        '        GoTo Finally_Renamed

        'Catch_Renamed:

        '        ' DO Not Call any functions before here or the error will be lost
        '        bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

        '        ' If you want to rollback a transaction or something, do it here

        'Finally_Renamed:

        '        ' Do any tidy up, e.g. Set x = Nothing here

        '        Return result

        '        ' This is for debugging only
        '        Resume

        '        Return result
    End Function

    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single SIRPartyCC directly into the database.
    '        Note: The SIRPartyCC will NOT be added to the collection.
    '
    ' ***************************************************************** '
    'EK 24/9/99 Added extra parameters
    'DC 28/06/00 Added Correspondence Type Id
    'DC 15/08/00 Added Invariant Key
    ' CTAF 270900 - Added Swift Party ID
    'JAS(CMG)03/09/02 - Added RecordStatus
    'JAS(CMG)28/10/02 - Added SubBranchID
    ' AMB 21-Oct-03: 1.8.6 Folgate EL0037 development - added trade_id
    Public Function DirectAdd(Optional ByRef vPartyCnt As Integer = 0, Optional ByRef vCompanyReg As Object = Nothing, Optional ByRef vTradingSinceDate As Object = Nothing, Optional ByRef vPartyBusinessId As Object = Nothing, Optional ByRef vLocation As Object = Nothing, Optional ByRef vNoOfOffices As Object = Nothing, Optional ByRef vNoOfEmployees As Object = Nothing, Optional ByRef vFinancialYear As Object = Nothing, Optional ByRef vTradeCode As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vShortname As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vResolvedName As Object = Nothing, Optional ByRef vIsAlsoAgent As Object = Nothing, Optional ByRef vIsProspect As Object = Nothing, Optional ByRef vAgentCnt As Object = Nothing, Optional ByRef vConsultantCnt As Object = Nothing, Optional ByRef vFileCode As Object = Nothing, Optional ByRef vCurrencyId As Object = Nothing, Optional ByRef vPaymentMethodCode As Object = Nothing, Optional ByRef vReminderTypeId As Object = Nothing, Optional ByRef vAreaId As Object = Nothing, Optional ByRef vServiceLevelId As Object = Nothing, Optional ByRef vRecordStatus As Object = Nothing, Optional ByRef vCreditCardCode As Object = Nothing, Optional ByRef vPaymentTermCode As Object = Nothing, Optional ByRef vCCJs As Object = Nothing, Optional ByRef vWageRoll As Object = Nothing, Optional ByRef vTurnover As Object = Nothing, Optional ByRef vStatus As Object = Nothing, Optional ByRef vABCCount As Object = Nothing, Optional ByRef vStatements As Object = Nothing, Optional ByRef vRenewals As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vLAstActionType As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vSICCodeId As Object = Nothing, Optional ByRef vSeasonalGiftID As Object = Nothing, Optional ByRef vCorrespondenceTypeId As Object = Nothing, Optional ByRef vRenewalStopCodeId As Object = Nothing, Optional ByRef vInvariantKey As Object = Nothing, Optional ByRef vSwiftPartyID As Object = Nothing, Optional ByRef vSalutation As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vTradeID As Object = Nothing, Optional ByRef vSource As Object = Nothing, Optional ByRef vTPSind As Object = Nothing, Optional ByRef vMailshot As Object = Nothing, Optional ByRef vEMPSind As Object = Nothing, Optional ByRef vTPPassword As Object = Nothing, Optional ByRef vTobLetter As Object = Nothing, Optional ByVal vLoyaltyNumber As Object = Nothing, Optional ByVal vAlternativeIdentifier As Object = Nothing, Optional ByVal vMarketingSegmentInd As Object = Nothing, Optional ByVal vTradingName As Object = Nothing, Optional ByRef vIsFeeClient As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyCC As bSIRPartyCC.SIRPartyCC
        Dim lPartyTypeId As Object
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'EK 22/9/99
            'Get party type id for a corporate client
            m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupPartyType, v_sCode:=gSIRLibrary.SIRPartyTypeCorporateClient, v_dtEffectiveDate:=DateTime.Now, r_lID:=lPartyTypeId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Create a new SIRPartyCC
            oSIRPartyCC = New bSIRPartyCC.SIRPartyCC()
            m_lReturn = oSIRPartyCC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            'DC 28/06/00 Added Correspondence Type Id
            'DC 15/08/00 Added Invariant Key
            'DC 23/08/00 Added Is Prospect flag
            'DC071200 added payment term code, payment method code,
            '           file code & reminder type id
            'DC181200 added consultant

            m_lReturn = oSIRPartyCC.bSIRParty.DirectAdd(vPartyTypeID:=lPartyTypeId, vShortname:=vShortname, vName:=vName, vResolvedName:=vResolvedName,
                                                        vAgentCnt:=vAgentCnt, vABCCount:=vABCCount, vStatements:=vStatements, vRenewals:=vRenewals,
                                                        vStatus:=vStatus, vLastModified:=vLastModified,
                                                        vLAstActionType:=vLAstActionType, vDateCreated:=vDateCreated,
                                                        vIsProspect:=vIsProspect, vSeasonalGiftID:=vSeasonalGiftID,
                                                        vCorrespondenceTypeId:=vCorrespondenceTypeId,
                                                        vRenewalStopCodeId:=vRenewalStopCodeId, vInvariantKey:=vInvariantKey,
                                                        vRecordStatus:=vRecordStatus,
                                                        vSwiftPartyID:=vSwiftPartyID, vPaymentTermCode:=vPaymentTermCode,
                                                        vPaymentMethodCode:=vPaymentMethodCode, vFileCode:=vFileCode,
                                                        vReminderTypeId:=vReminderTypeId, vConsultantCnt:=vConsultantCnt,
                                                        vCurrencyId:=vCurrencyId, vSubBranchID:=vSubBranchID,
                                                        vTobLetter:=vTobLetter, vLoyaltyNumber:=vLoyaltyNumber,
                                                        vAlternativeIdentifier:=vAlternativeIdentifier,
                                                        vMarketingSegementInd:=vMarketingSegmentInd,
                                                        vTradingName:=vTradingName)
            'MSS200901 - Added CurrencyID from UW
            'JAS(CMG)03/09/02 - Added RecordStatus
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyCC = Nothing
                Return result
            End If

            ' Retrieve Primary Key of new Core record

            vPartyCnt = oSIRPartyCC.bSIRParty.PartyCnt
            'EK 22/9/99
            oSIRPartyCC.PartyCnt = vPartyCnt
            ' Populate SIRPartyCC Attributes



            m_lReturn = oSIRPartyCC.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vPartyCnt:=vPartyCnt, vCompanyReg:=vCompanyReg, vTradingSinceDate:=vTradingSinceDate, vPartyBusinessId:=vPartyBusinessId, vLocation:=vLocation, vNoOfOffices:=vNoOfOffices, vNoOfEmployees:=vNoOfEmployees, vFinancialYear:=vFinancialYear, vTradeCode:=vTradeCode, vWageRoll:=vWageRoll, vTurnover:=vTurnover, vSICCodeId:=vSICCodeId, vSalutation:=vSalutation, vTradeID:=vTradeID, vTPSind:=vTPSind, vSource:=vSource, vMailshot:=vMailshot, vEMPSind:=vEMPSind, vTPPassword:=vTPPassword, vIsFeeClient:=vIsFeeClient)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyCC = Nothing
                Return result
            End If

            ' Add the SIRPartyCC to the Database
            m_lReturn = oSIRPartyCC.AddItem()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyCC = Nothing
                Return result
            End If

            ' Retain the Primary Key of the SIRPartyCC Added
            With oSIRPartyCC
                PartyCnt = .PartyCnt
            End With

            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

            oSIRPartyCC = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a single SIRPartyCC directly from the database.
    '        Note: The SIRPartyCC will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByRef vPartyCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyCC As bSIRPartyCC.SIRPartyCC

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new SIRPartyCC
            oSIRPartyCC = New bSIRPartyCC.SIRPartyCC()
            m_lReturn = oSIRPartyCC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            ' Set SIRPartyCC Primary Key

            m_lReturn = oSIRPartyCC.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMDelete, vPartyCnt:=vPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyCC = Nothing
                Return result
            End If

            ' Delete the SIRPartyCC from the Database
            m_lReturn = oSIRPartyCC.DeleteItem()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyCC = Nothing
                Return result
            End If


            m_lReturn = oSIRPartyCC.bSIRParty.DirectDelete(vPartyCnt:=vPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyCC = Nothing
                Return result
            End If

            oSIRPartyCC = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required SIRPartyCCs and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vLockMode As Object = 0, Optional ByRef vPartyCnt As Object = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        'developer guide no. 21
        Dim oFields As DataRow
        Dim oSIRPartyCC As bSIRPartyCC.SIRPartyCC

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oSIRPartyCCs.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = gPMConstants.PMEReturnCode.PMFalse

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

                ' Create New SIRPartyCC
                oSIRPartyCC = New bSIRPartyCC.SIRPartyCC()
                m_lReturn = oSIRPartyCC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

                ' Set component primary keys
                With oSIRPartyCC
                    .PartyCnt = vPartyCnt

                    'And if we're coming from events
                    .FromEvent = FromEvent

                    m_lReturn = .SelectItem()
                    'Start Girija --- PN 54576
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        'End Girija --- PN 54576
                        Return m_lReturn
                    End If
                End With

                ' Add SIRPartyCC to collection
                If m_oSIRPartyCCs.Count = 0 Then
                    m_oSIRPartyCCs.Add(Nothing)
                End If
                m_lReturn = m_oSIRPartyCCs.Add(oNewSIRPartyCC:=oSIRPartyCC)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Call the core Business method

                m_lReturn = oSIRPartyCC.bSIRParty.GetDetails(vLockMode:=vLockMode, vPartyCnt:=vPartyCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                oSIRPartyCC = Nothing

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
                    oSIRPartyCC = New bSIRPartyCC.SIRPartyCC()
                    'developer guide no. 9
                    m_lReturn = oSIRPartyCC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)
                    ' Set oFields to refer to one Record
                    oFields = m_oDatabase.Records.Item(lSub).Fields()

                    ' Set component primary keys from current record
                    With oSIRPartyCC
                        'SD 02/08/2002
                        .PartyCnt = gPMFunctions.NullToLong(oFields("party_cnt"))

                        m_lReturn = .SelectItem()

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return m_lReturn
                        End If


                        m_lReturn = .bSIRParty.GetDetails(vLockMode:=vLockMode, vPartyCnt:=ToSafeInteger(.PartyCnt))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    End With

                    ' Add SIRPartyCC to collection
                    If m_oSIRPartyCCs.Count = 0 Then
                        m_oSIRPartyCCs.Add(Nothing)
                    End If
                    m_lReturn = m_oSIRPartyCCs.Add(oNewSIRPartyCC:=oSIRPartyCC)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oSIRPartyCC = Nothing
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
    ' Description: Gets the required SIRPartyCCs and populate the Collection
    '
    ' ***************************************************************** '
    'DC 28/06/00 Added Correspondence Type Id
    ' CTAF 270900 - Added Swift Party ID
    'sj 12/06/2002 - Added LoyaltyNumber, AlternativeIdentifier,
    '                MarketingSegmentInd, TradingName and SubBranchId
    'JAS(CMG) 05/09/02 - Added vRecordStatus As Variant
    ' AMB 21-Oct-03: 1.8.6 Folgate EL0037 development - added trade_id
    Public Function GetNext(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vCompanyReg As Object = Nothing, Optional ByRef vTradingSinceDate As Object = Nothing, Optional ByRef vPartyBusinessId As Object = Nothing, Optional ByRef vLocation As Object = Nothing, Optional ByRef vNoOfOffices As Object = Nothing, Optional ByRef vNoOfEmployees As Object = Nothing, Optional ByRef vFinancialYear As Object = Nothing, Optional ByRef vTradeCode As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vPartyID As Object = Nothing, Optional ByRef vShortname As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vIsAlsoAgent As Object = Nothing, Optional ByRef vIsProspect As Object = Nothing, Optional ByRef vAgentCnt As Object = Nothing, Optional ByRef vConsultantCnt As Object = Nothing, Optional ByRef vRecordStatus As Object = Nothing, Optional ByRef vFileCode As Object = Nothing, Optional ByRef vCurrencyId As Object = Nothing, Optional ByRef vPaymentMethodCode As Object = Nothing, Optional ByRef vReminderTypeId As Object = Nothing, Optional ByRef vAreaId As Object = Nothing, Optional ByRef vServiceLevelId As Object = Nothing, Optional ByRef vCreditCardCode As Object = Nothing, Optional ByRef vPaymentTermCode As Object = Nothing, Optional ByRef vCCJs As Object = Nothing, Optional ByRef vWageRoll As Object = Nothing, Optional ByRef vTurnover As Object = Nothing, Optional ByRef vSeasonalGiftID As Object = Nothing, Optional ByRef vSICCodeId As Object = Nothing, Optional ByRef vCorrespondenceTypeId As Object = Nothing, Optional ByRef vRenewalStopCodeId As Object = Nothing, Optional ByRef vSwiftPartyID As Object = Nothing, Optional ByRef vLoyaltyNumber As Object = Nothing, Optional ByRef vAlternativeIdentifier As Object = Nothing, Optional ByRef vMarketingSegementInd As Object = Nothing, Optional ByRef vTradingName As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vTobLetter As Object = Nothing, Optional ByRef vSalutation As Object = Nothing, Optional ByRef vTradeID As Object = Nothing, Optional ByRef vSource As Object = Nothing, Optional ByRef vTPSind As Object = Nothing, Optional ByRef vMailshot As Object = Nothing, Optional ByRef vEMPSind As Object = Nothing, Optional ByRef vTPPassword As Object = Nothing, Optional ByRef vIsFeeClient As Object = Nothing) As Integer

        Dim result As Integer = 0

        Dim oSIRPartyCC As bSIRPartyCC.SIRPartyCC
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oSIRPartyCCs.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord = CType(m_lCurrentRecord + 1, gPMConstants.PMEReturnCode)
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If
            'developer guide no. 
            oSIRPartyCC = m_oSIRPartyCCs.Item(m_lCurrentRecord)

            ' Get the SIRPartyCC Property Values









            m_lReturn = oSIRPartyCC.GetProperties(iStatus, vPartyCnt:=vPartyCnt, vCompanyReg:=vCompanyReg, vTradingSinceDate:=vTradingSinceDate, vPartyBusinessId:=vPartyBusinessId, vLocation:=vLocation, vNoOfOffices:=vNoOfOffices, vNoOfEmployees:=vNoOfEmployees, vFinancialYear:=vFinancialYear, vTradeCode:=vTradeCode, vWageRoll:=vWageRoll, vTurnover:=vTurnover, vSICCodeId:=vSICCodeId, vSalutation:=vSalutation, vTradeID:=vTradeID, vTPSind:=vTPSind, vSource:=vSource, vMailshot:=vMailshot, vEMPSind:=vEMPSind, vTPPassword:=vTPPassword, vIsFeeClient:=vIsFeeClient)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get Core details

            m_lReturn = oSIRPartyCC.bSIRParty.GetDetails(vPartyCnt:=vPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'SP231198
            'DC 28/06/00 Added Correspondence Type Id
            ' CTAF 270900 - Added Swift Party ID
            'sj 12/06/2002 - Added LoyaltyNumber, AlternativeIdentifier,
            '                MarketingSegmentInd, TradingName and SubBranchId
            'JAS(CMG) 05/09/02 - Added vRecordStatus

            m_lReturn = oSIRPartyCC.bSIRParty.GetNext(vSourceID:=vSourceID, vPartyID:=vPartyID, vShortname:=vShortname, vName:=vName, vIsAlsoAgent:=vIsAlsoAgent, vIsProspect:=vIsProspect, vAgentCnt:=vAgentCnt, vConsultantCnt:=vConsultantCnt, vFileCode:=vFileCode, vRecordStatus:=vRecordStatus, vCurrencyId:=vCurrencyId, vPaymentMethodCode:=vPaymentMethodCode, vReminderTypeId:=vReminderTypeId, vAreaId:=vAreaId, vServiceLevelId:=vServiceLevelId, vCreditCardCode:=vCreditCardCode, vPaymentTermCode:=vPaymentTermCode, vCCJs:=vCCJs, vSeasonalGiftID:=vSeasonalGiftID, vCorrespondenceTypeId:=vCorrespondenceTypeId, vRenewalStopCodeId:=vRenewalStopCodeId, vSwiftPartyID:=vSwiftPartyID, vLoyaltyNumber:=vLoyaltyNumber, vAlternativeIdentifier:=vAlternativeIdentifier, vMarketingSegementInd:=vMarketingSegementInd, vTradingName:=vTradingName, vSubBranchID:=vSubBranchID, vTobLetter:=vTobLetter)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyCC = Nothing

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
    ' Description: Adds the supplied SIRPartyCC into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' AMB 21-Oct-03: 1.8.6 Folgate EL0037 development - added trade_id parameter
    ' ***************************************************************** '
    'DC 28/06/00 Added Correspondence Type Id
    Public Function EditAdd(ByRef lRow As Object, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vCompanyReg As Object = Nothing, Optional ByRef vTradingSinceDate As Object = Nothing, Optional ByRef vPartyBusinessId As Object = Nothing, Optional ByRef vLocation As Object = Nothing, Optional ByRef vNoOfOffices As Object = Nothing, Optional ByRef vNoOfEmployees As Object = Nothing, Optional ByRef vFinancialYear As Object = Nothing, Optional ByRef vTradeCode As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vShortname As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vResolvedName As Object = Nothing, Optional ByRef vIsAlsoAgent As Object = Nothing, Optional ByRef vIsProspect As Object = Nothing, Optional ByRef vAgentCnt As Object = Nothing, Optional ByRef vConsultantCnt As Object = Nothing, Optional ByRef vFileCode As Object = Nothing, Optional ByRef vCurrencyId As Object = Nothing, Optional ByRef vPaymentMethodCode As Object = Nothing, Optional ByRef vReminderTypeId As Object = Nothing, Optional ByRef vAreaId As Object = Nothing, Optional ByRef vServiceLevelId As Object = Nothing, Optional ByRef vCreditCardCode As Object = Nothing, Optional ByRef vPaymentTermCode As Object = Nothing, Optional ByRef vCCJs As Object = Nothing, Optional ByRef vWageRoll As Object = Nothing, Optional ByRef vTurnover As Object = Nothing, Optional ByRef vSeasonalGiftID As Object = Nothing, Optional ByRef vSICCodeId As Object = Nothing, Optional ByRef vCorrespondenceTypeId As Object = Nothing, Optional ByRef vRenewalStopCodeId As Object = Nothing, Optional ByRef vSwiftPartyID As Object = Nothing, Optional ByRef vSalutation As Object = Nothing, Optional ByRef vLoyaltyNumber As Object = Nothing, Optional ByRef vAlternativeIdentifier As Object = Nothing, Optional ByRef vMarketingSegementInd As Object = Nothing, Optional ByRef vTradingName As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vTobLetter As Object = Nothing, Optional ByRef vTradeID As Object = Nothing, Optional ByRef vSource As Object = Nothing, Optional ByRef vTPSind As Object = Nothing, Optional ByRef vMailshot As Object = Nothing, Optional ByRef vEMPSind As Object = Nothing, Optional ByRef vTPPassword As Object = Nothing, Optional ByRef vIsFeeClient As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyCC As bSIRPartyCC.SIRPartyCC
        Dim lPartyTypeId As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get party type id for a Corporate client
            m_lReturn = m_oLookup.GetEffectiveIDFromCode(gSIRLibrary.SIRLookupPartyType, gSIRLibrary.SIRPartyTypeCorporateClient, DateTime.Now, lPartyTypeId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oSIRPartyCCs.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new SIRPartyCC
            oSIRPartyCC = New bSIRPartyCC.SIRPartyCC()
            m_lReturn = oSIRPartyCC.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_oDatabase)

            ' Populate SIRPartyCC Attributes

            ' developer guide no. 98
            m_lReturn = oSIRPartyCC.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vPartyCnt:=vPartyCnt, vCompanyReg:=vCompanyReg, vTradingSinceDate:=vTradingSinceDate, vPartyBusinessId:=vPartyBusinessId, vLocation:=vLocation, vNoOfOffices:=vNoOfOffices, vNoOfEmployees:=vNoOfEmployees, vFinancialYear:=vFinancialYear, vTradeCode:=vTradeCode, vWageRoll:=vWageRoll, vTurnover:=vTurnover, vSICCodeId:=vSICCodeId, vSalutation:=vSalutation, vTradeID:=vTradeID, vTPSind:=vTPSind, vSource:=vSource, vMailshot:=vMailshot, vEMPSind:=vEMPSind, vTPPassword:=vTPPassword, vIsFeeClient:=vIsFeeClient)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oSIRPartyCC = Nothing
                Return result
            End If

            ' Add SIRPartyCC to collection
            If m_oSIRPartyCCs.Count = 0 Then
                m_oSIRPartyCCs.Add(Nothing)
            End If
            m_lReturn = m_oSIRPartyCCs.Add(oSIRPartyCC)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyCC = Nothing
                Return result
            End If
            'eck150500 pass source_id
            'DC 28/06/00 Added Correspondence Type Id
            ' CTAF 270900 - Added Swift Party ID
            'sj 12/06/2002 - Added LoyaltyNumber, AlternativeIdentifier,
            '                MarketingSegmentInd, TradingName and SubBranchId

            m_lReturn = oSIRPartyCC.bSIRParty.EditAdd(lRow:=lRow, vPartyTypeID:=lPartyTypeId, vSourceID:=vSourceID,
                                                      vShortname:=vShortname,
                                                      vName:=vName, vResolvedName:=vResolvedName, vIsAlsoAgent:=vIsAlsoAgent,
                                                      vIsProspect:=vIsProspect, vAgentCnt:=vAgentCnt,
                                                      vConsultantCnt:=vConsultantCnt, vFileCode:=vFileCode,
                                                      vCurrencyId:=vCurrencyId, vPaymentMethodCode:=vPaymentMethodCode,
                                                      vAreaId:=vAreaId, vReminderTypeId:=vReminderTypeId,
                                                      vServiceLevelId:=vServiceLevelId, vCreditCardCode:=vCreditCardCode,
                                                      vPaymentTermCode:=vPaymentTermCode, vCCJs:=vCCJs,
                                                      vSeasonalGiftID:=vSeasonalGiftID, vCorrespondenceTypeId:=vCorrespondenceTypeId,
                                                      vRenewalStopCodeId:=vRenewalStopCodeId, vSwiftPartyID:=vSwiftPartyID,
                                                      vLoyaltyNumber:=vLoyaltyNumber, vAlternativeIdentifier:=vAlternativeIdentifier,
                                                      vMarketingSegementInd:=vMarketingSegementInd, vTradingName:=vTradingName,
                                                      vSubBranchID:=vSubBranchID, vTobLetter:=vTobLetter)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyCC = Nothing
                Return result
            End If

            oSIRPartyCC = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "EditAdd Failed", MainModule.ACApp, ACClass, "EditAdd", Informations.Err().Number, excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the SIRPartyCC
    '              specified and updates the SIRPartyCC with the new values.
    '
    ' ***************************************************************** '
    'DC 28/06/00 Added Correspondence Type Id
    'eck010900 Receive Party Id
    ' CTAF 021100 - Added SwiftPartyID (forgot this one before!!!)
    'sj 12/06/2002 - Added LoyaltyNumber, AlternativeIdentifier,
    '                MarketingSegmentInd, TradingName and SubBranchId
    ' AMB 21-Oct-03: 1.8.6 Folgate EL0037 development - added trade_id
    Public Function EditUpdate(ByRef lRow As Object, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vCompanyReg As Object = Nothing, Optional ByRef vTradingSinceDate As Object = Nothing, Optional ByRef vPartyBusinessId As Object = Nothing, Optional ByRef vLocation As Object = Nothing, Optional ByRef vNoOfOffices As Object = Nothing, Optional ByRef vNoOfEmployees As Object = Nothing, Optional ByRef vFinancialYear As Object = Nothing, Optional ByRef vTradeCode As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vShortname As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vResolvedName As Object = Nothing, Optional ByRef vIsAlsoAgent As Object = Nothing, Optional ByRef vIsProspect As Object = Nothing, Optional ByRef vAgentCnt As Object = Nothing, Optional ByRef vConsultantCnt As Object = Nothing, Optional ByRef vFileCode As Object = Nothing, Optional ByRef vCurrencyId As Object = Nothing, Optional ByRef vPaymentMethodCode As Object = Nothing, Optional ByRef vReminderTypeId As Object = Nothing, Optional ByRef vAreaId As Object = Nothing, Optional ByRef vServiceLevelId As Object = Nothing, Optional ByRef vCreditCardCode As Object = Nothing, Optional ByRef vPaymentTermCode As Object = Nothing, Optional ByRef vCCJs As Object = Nothing, Optional ByRef vWageRoll As Object = Nothing, Optional ByRef vTurnover As Object = Nothing, Optional ByRef vSeasonalGiftID As Object = Nothing, Optional ByRef vSICCodeId As Object = Nothing, Optional ByRef vPartyID As Object = Nothing, Optional ByRef vCorrespondenceTypeId As Object = Nothing, Optional ByRef vRenewalStopCodeId As Object = Nothing, Optional ByRef vSwiftPartyID As Object = Nothing, Optional ByRef vSalutation As Object = Nothing, Optional ByRef vLoyaltyNumber As Object = Nothing, Optional ByRef vAlternativeIdentifier As Object = Nothing, Optional ByRef vMarketingSegementInd As Object = Nothing, Optional ByRef vTradingName As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vTobLetter As Object = Nothing, Optional ByRef vTradeID As Object = Nothing, Optional ByRef vSource As Object = Nothing, Optional ByRef vTPSind As Object = Nothing, Optional ByRef vMailshot As Object = Nothing, Optional ByRef vEMPSind As Object = Nothing, Optional ByRef vTPPassword As Object = Nothing, Optional ByRef vIsFeeClient As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyCC As bSIRPartyCC.SIRPartyCC
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oSIRPartyCCs.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oSIRPartyCC = m_oSIRPartyCCs.Item(lRow)

            ' Check the Status of the SIRPartyCC

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oSIRPartyCC.DatabaseStatus
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

            ' Update SIRPartyCC Attributes









            m_lReturn = oSIRPartyCC.SetProperties(iStatus:=iStatus, vPartyCnt:=vPartyCnt, vCompanyReg:=vCompanyReg, vTradingSinceDate:=vTradingSinceDate, vPartyBusinessId:=vPartyBusinessId, vLocation:=vLocation, vNoOfOffices:=vNoOfOffices, vNoOfEmployees:=vNoOfEmployees, vFinancialYear:=vFinancialYear, vTradeCode:=vTradeCode, vWageRoll:=vWageRoll, vTurnover:=vTurnover, vSICCodeId:=vSICCodeId, vSalutation:=vSalutation, vTradeID:=vTradeID, vTPSind:=vTPSind, vSource:=vSource, vMailshot:=vMailshot, vEMPSind:=vEMPSind, vTPPassword:=vTPPassword, vIsFeeClient:=vIsFeeClient)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oSIRPartyCC = Nothing
                Return result
            End If

            'DC 28/06/00 Added Correspondence Type Id
            'eck0109000 pass source& party Id
            'sj 12/06/2002 - Added LoyaltyNumber, AlternativeIdentifier,
            '                MarketingSegmentInd, TradingName and SubBranchId

            m_lReturn = oSIRPartyCC.bSIRParty.EditUpdate(lRow:=lRow, vPartyCnt:=vPartyCnt, vPartyID:=vPartyID,
                                                         vSourceID:=vSourceID, vShortname:=vShortname,
                                                         vName:=vName, vResolvedName:=vResolvedName, vAgentCnt:=vAgentCnt,
                                                         vIsAlsoAgent:=vIsAlsoAgent, vIsProspect:=vIsProspect,
                                                         vConsultantCnt:=vConsultantCnt, vFileCode:=vFileCode,
                                                         vCurrencyId:=vCurrencyId, vPaymentMethodCode:=vPaymentMethodCode,
                                                         vReminderTypeId:=vReminderTypeId, vAreaId:=vAreaId,
                                                         vServiceLevelId:=vServiceLevelId, vCreditCardCode:=vCreditCardCode,
                                                         vPaymentTermCode:=vPaymentTermCode, vCCJs:=vCCJs,
                                                         vSeasonalGiftID:=vSeasonalGiftID, vCorrespondenceTypeId:=vCorrespondenceTypeId,
                                                         vRenewalStopCodeId:=vRenewalStopCodeId, vSwiftPartyID:=vSwiftPartyID,
                                                         vLoyaltyNumber:=vLoyaltyNumber, vAlternativeIdentifier:=vAlternativeIdentifier,
                                                         vMarketingSegementInd:=vMarketingSegementInd, vTradingName:=vTradingName,
                                                         vSubBranchID:=vSubBranchID, vTobLetter:=vTobLetter)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyCC = Nothing
                Return result
            End If

            ' Release reference to SIRPartyCC
            oSIRPartyCC = Nothing

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
    ' Description: Validate that the specified SIRPartyCC can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyCC As bSIRPartyCC.SIRPartyCC

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oSIRPartyCCs.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oSIRPartyCC = m_oSIRPartyCCs.Item(lRow)

            ' Check the Status of the SIRPartyCC

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oSIRPartyCC.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oSIRPartyCC.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oSIRPartyCC.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If


            m_lReturn = oSIRPartyCC.bSIRParty.EditDelete(lRow:=ToSafeInteger(lRow))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyCC = Nothing
                Return result
            End If

            ' Release reference to SIRPartyCC
            oSIRPartyCC = Nothing

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
            For lSub As Integer = 1 To m_oSIRPartyCCs.Count()
                Select Case m_oSIRPartyCCs.Item(lSub).DatabaseStatus
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
        Const kMethodName As String = "Update"


        Dim lSub As gPMConstants.PMEReturnCode
        Dim oSIRPartyCC As bSIRPartyCC.SIRPartyCC = Nothing
        Dim bTransStarted As Boolean
        Dim lEventCnt As Integer
        Dim sDescription As New StringBuilder
        Dim lLoopNewAddress As Integer
        Dim bAddressMatched As Boolean

        Try





            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection
            For lSub = 1 To m_oSIRPartyCCs.Count()
                oSIRPartyCC = m_oSIRPartyCCs.Item(lSub)


                Select Case oSIRPartyCC.DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing

                    Case gPMConstants.PMEComponentAction.PMAdd

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = BeginTrans()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("BeginTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                            bTransStarted = True
                        End If

                        ' Add Item

                        m_lReturn = oSIRPartyCC.bSIRParty.Update()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("oSIRPartyCC.bSIRParty.Update", "Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        ' Retrieve Primary Key of Core Item added

                        m_lPartyCnt = oSIRPartyCC.bSIRParty.PartyCnt
                        oSIRPartyCC.PartyCnt = m_lPartyCnt

                        m_lReturn = oSIRPartyCC.AddItem()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("oSIRPartyCC.AddItem", "Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        'Add the created event
                        m_lReturn = CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=PartyCnt, v_lEventTypeId:=PMBConst.PMBEventNewClient, v_dtEventDate:=DateTime.Today)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("CreateEvent", "Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                    Case gPMConstants.PMEComponentAction.PMEdit

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = BeginTrans()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("BeginTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                            bTransStarted = True
                        End If

                        ' Update Item
                        m_lReturn = oSIRPartyCC.UpdateItem()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("oSIRPartyCC.UpdateItem", "Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If


                        m_lReturn = oSIRPartyCC.bSIRParty.Update()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("oSIRPartyCC.bSIRParty.Update", "Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        If m_sOldClientCode <> m_sNewClientCode Then

                            sDescription = New StringBuilder("Client code has been changed from '" & m_sOldClientCode & "' to '" & m_sNewClientCode & "'.")


                            m_lReturn = CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=oSIRPartyCC.bSIRParty.PartyCnt, v_lEventTypeId:=PMBConst.PMBEventClientNotes, v_dtEventDate:=DateTime.Today, v_vDescription:=sDescription.ToString())
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("CreateEvent", "Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                        End If

                        If m_sOldClientName <> m_sNewClientName Then

                            sDescription = New StringBuilder("Client name has been changed from '" & m_sOldClientName & "' to '" & m_sNewClientName & "'.")


                            m_lReturn = CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=oSIRPartyCC.bSIRParty.PartyCnt, v_lEventTypeId:=PMBConst.PMBEventClientNotes, v_dtEventDate:=DateTime.Today, v_vDescription:=sDescription.ToString())
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("CreateEvent", "Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                        End If

                        If Informations.IsArray(m_vOldAddress) Then

                            For lLoopOldAddress As Integer = 0 To m_vOldAddress.GetUpperBound(1)

                                bAddressMatched = False

                                For lLoopNewAddress = 0 To m_vNewAddress.GetUpperBound(1)
                                    If m_vOldAddress(0, lLoopOldAddress).Equals(m_vNewAddress(0, lLoopNewAddress)) Then
                                        bAddressMatched = True
                                        Exit For
                                    End If
                                Next

                                If bAddressMatched Then
                                    If Not m_vOldAddress(2, lLoopOldAddress).Equals(m_vNewAddress(2, lLoopNewAddress)) Or Not m_vOldAddress(3, lLoopOldAddress).Equals(m_vNewAddress(3, lLoopNewAddress)) Or Not m_vOldAddress(4, lLoopOldAddress).Equals(m_vNewAddress(4, lLoopNewAddress)) Or Not m_vOldAddress(5, lLoopOldAddress).Equals(m_vNewAddress(5, lLoopNewAddress)) Or Not m_vOldAddress(6, lLoopOldAddress).Equals(m_vNewAddress(6, lLoopNewAddress)) Then

                                        sDescription = New StringBuilder(CStr(m_vOldAddress(1, lLoopOldAddress)) & " has been changed from:" & Strings.ChrW(13) & Strings.ChrW(10))
                                        sDescription.Append("    " & CStr(m_vOldAddress(2, lLoopOldAddress)) & Strings.ChrW(13) & Strings.ChrW(10))
                                        sDescription.Append("    " & CStr(m_vOldAddress(3, lLoopOldAddress)) & Strings.ChrW(13) & Strings.ChrW(10))
                                        sDescription.Append("    " & CStr(m_vOldAddress(4, lLoopOldAddress)) & Strings.ChrW(13) & Strings.ChrW(10))
                                        sDescription.Append("    " & CStr(m_vOldAddress(5, lLoopOldAddress)) & Strings.ChrW(13) & Strings.ChrW(10))
                                        sDescription.Append("    " & CStr(m_vOldAddress(6, lLoopOldAddress)) & Strings.ChrW(13) & Strings.ChrW(10))
                                        sDescription.Append("To:" & Strings.ChrW(13) & Strings.ChrW(10))
                                        sDescription.Append("    " & CStr(m_vNewAddress(2, lLoopNewAddress)) & Strings.ChrW(13) & Strings.ChrW(10))
                                        sDescription.Append("    " & CStr(m_vNewAddress(3, lLoopNewAddress)) & Strings.ChrW(13) & Strings.ChrW(10))
                                        sDescription.Append("    " & CStr(m_vNewAddress(4, lLoopNewAddress)) & Strings.ChrW(13) & Strings.ChrW(10))
                                        sDescription.Append("    " & CStr(m_vNewAddress(5, lLoopNewAddress)) & Strings.ChrW(13) & Strings.ChrW(10))
                                        sDescription.Append("    " & CStr(m_vNewAddress(6, lLoopNewAddress)))



                                        m_lReturn = CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=oSIRPartyCC.bSIRParty.PartyCnt, v_lEventTypeId:=PMBConst.PMBEventClientNotes, v_dtEventDate:=DateTime.Today, v_vDescription:=sDescription.ToString())
                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            gPMFunctions.RaiseError("CreateEvent", "Failed", gPMConstants.PMELogLevel.PMLogError)
                                        End If
                                    End If
                                Else
                                    sDescription = New StringBuilder(CStr(m_vOldAddress(1, lLoopOldAddress)) & ":" & Strings.ChrW(13) & Strings.ChrW(10))
                                    sDescription.Append("    " & CStr(m_vOldAddress(2, lLoopOldAddress)) & Strings.ChrW(13) & Strings.ChrW(10))
                                    sDescription.Append("    " & CStr(m_vOldAddress(3, lLoopOldAddress)) & Strings.ChrW(13) & Strings.ChrW(10))
                                    sDescription.Append("    " & CStr(m_vOldAddress(4, lLoopOldAddress)) & Strings.ChrW(13) & Strings.ChrW(10))
                                    sDescription.Append("    " & CStr(m_vOldAddress(5, lLoopOldAddress)) & Strings.ChrW(13) & Strings.ChrW(10))
                                    sDescription.Append("    " & CStr(m_vOldAddress(6, lLoopOldAddress)) & Strings.ChrW(13) & Strings.ChrW(10))
                                    sDescription.Append("has been deleted.")


                                    m_lReturn = CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=oSIRPartyCC.bSIRParty.PartyCnt, v_lEventTypeId:=PMBConst.PMBEventClientNotes, v_dtEventDate:=DateTime.Today, v_vDescription:=sDescription.ToString())
                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        gPMFunctions.RaiseError("CreateEvent", "Failed", gPMConstants.PMELogLevel.PMLogError)
                                    End If
                                End If

                            Next

                        End If

                        If Informations.IsArray(m_vNewAddress) Then

                            For lLoopNewAddress = 0 To m_vNewAddress.GetUpperBound(1)

                                bAddressMatched = False

                                For lLoopOldAddress As Integer = 0 To m_vOldAddress.GetUpperBound(1)
                                    If m_vNewAddress(0, lLoopNewAddress).Equals(m_vOldAddress(0, lLoopOldAddress)) Then
                                        bAddressMatched = True
                                        Exit For
                                    End If
                                Next

                                If Not bAddressMatched Then
                                    sDescription = New StringBuilder(CStr(m_vNewAddress(1, lLoopNewAddress)) & ":" & Strings.ChrW(13) & Strings.ChrW(10))
                                    sDescription.Append("    " & CStr(m_vNewAddress(2, lLoopNewAddress)) & Strings.ChrW(13) & Strings.ChrW(10))
                                    sDescription.Append("    " & CStr(m_vNewAddress(3, lLoopNewAddress)) & Strings.ChrW(13) & Strings.ChrW(10))
                                    sDescription.Append("    " & CStr(m_vNewAddress(4, lLoopNewAddress)) & Strings.ChrW(13) & Strings.ChrW(10))
                                    sDescription.Append("    " & CStr(m_vNewAddress(5, lLoopNewAddress)) & Strings.ChrW(13) & Strings.ChrW(10))
                                    sDescription.Append("    " & CStr(m_vNewAddress(6, lLoopNewAddress)) & Strings.ChrW(13) & Strings.ChrW(10))
                                    sDescription.Append("has been added.")


                                    m_lReturn = CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=oSIRPartyCC.bSIRParty.PartyCnt, v_lEventTypeId:=PMBConst.PMBEventClientNotes, v_dtEventDate:=DateTime.Today, v_vDescription:=sDescription.ToString())
                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        gPMFunctions.RaiseError("CreateEvent", "Failed", gPMConstants.PMELogLevel.PMLogError)
                                    End If
                                End If

                            Next

                        End If

                    Case gPMConstants.PMEComponentAction.PMDelete

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = BeginTrans()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("BeginTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                            bTransStarted = True
                        End If

                        'Add the created event first

                        m_lReturn = CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=oSIRPartyCC.bSIRParty.PartyCnt, v_lEventTypeId:=PMBConst.PMBEventDelClient, v_dtEventDate:=DateTime.Today)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("CreateEvent", "Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        'Now keep a copy of the deleted item
                        m_lReturn = oSIRPartyCC.CopyToEvent(v_lEventCnt:=lEventCnt)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("oSIRPartyCC.CopyToEvent", "v_lEventCnt:=" & lEventCnt, gPMConstants.PMELogLevel.PMLogError)
                        End If

                        ' Delete Item
                        m_lReturn = oSIRPartyCC.DeleteItem()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("oSIRPartyCC.DeleteItem", "Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If


                        m_lReturn = oSIRPartyCC.bSIRParty.Update()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("oSIRPartyCC.bSIRParty.Update", "Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                End Select

            Next

            ' Retain the Primary Key of the SIRPartyCC
            With oSIRPartyCC
                PartyCnt = .PartyCnt
            End With

            ' Release last reference
            oSIRPartyCC = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                m_lReturn = CommitTrans()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("CommitTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                'Refresh the Collection Items D/B Status
                lSub = gPMConstants.PMEReturnCode.PMTrue

                ' For each item in the collection
                Do While lSub <= m_oSIRPartyCCs.Count()

                    ' With the item
                    With m_oSIRPartyCCs.Item(lSub)


                        Select Case .DatabaseStatus
                            ' Delete or Dummy Delete
                            Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                ' Remove from Collection
                                m_oSIRPartyCCs.Delete(lSub)

                                ' Anything Else
                            Case Else
                                ' Set Status to view
                                .DatabaseStatus = gPMConstants.PMEComponentAction.PMView
                                lSub = CType(lSub + 1, gPMConstants.PMEReturnCode)

                        End Select

                    End With

                Loop

            End If

        Catch ex As Exception


            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            m_lReturn = RollbackTrans()

        Finally

        End Try

        Return result
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

        Dim oSIRPartyCC As bSIRPartyCC.SIRPartyCC

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyCC - need to for to core for shortname
            oSIRPartyCC = New bSIRPartyCC.SIRPartyCC()

            m_lReturn = oSIRPartyCC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            m_lReturn = oSIRPartyCC.bSIRParty.GetPartyCnt(vPartyRef:=vPartyRef, vPartyCnt:=vPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyCC = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetPartyCnt Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyCnt", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAccountID
    '
    ' Description: Get Account ID for a given reference (ie shortname)
    '
    ' Created MKW 15/10/2003 PN7523
    ' ***************************************************************** '
    Public Function GetAccountID(Optional ByRef vPartyRef As Object = Nothing, Optional ByRef vAccountID As Object = Nothing) As Integer

        Dim result As Integer = 0

        Dim oSIRPartyCC As bSIRPartyCC.SIRPartyCC

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyCC - need to for to core for shortname
            oSIRPartyCC = New bSIRPartyCC.SIRPartyCC()

            m_lReturn = oSIRPartyCC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            m_lReturn = oSIRPartyCC.bSIRParty.GetAccountID(vPartyRef:=vPartyRef, vAccountID:=vAccountID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyCC = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetAccountID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetOtherDetails
    '
    ' Description: Get other details from DB, for related parties.
    '
    ' ***************************************************************** '
    Public Function GetOtherDetails(Optional ByRef vAgentCnt As Object = Nothing, Optional ByRef vAgentRef As Object = Nothing, Optional ByRef vAgentName As Object = Nothing, Optional ByRef vConsultantCnt As Object = Nothing, Optional ByRef vConsultantRef As Object = Nothing, Optional ByRef vConsultantname As Object = Nothing, Optional ByRef vInsurerCnt As Object = Nothing, Optional ByRef vInsurerRef As Object = Nothing, Optional ByRef vInsurername As Object = Nothing, Optional ByRef vBrokerCnt As Object = Nothing, Optional ByRef vBrokerRef As Object = Nothing, Optional ByRef vBrokername As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vAssociates As Object = Nothing) As Integer

        Dim result As Integer = 0

        Dim oSIRPartyCC As bSIRPartyCC.SIRPartyCC


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyCC - ned to go to core for agent
            oSIRPartyCC = New bSIRPartyCC.SIRPartyCC()
            m_lReturn = oSIRPartyCC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            m_lReturn = oSIRPartyCC.bSIRParty.GetOtherDetails(vAgentCnt:=vAgentCnt, vAgentRef:=vAgentRef, vAgentName:=vAgentName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If




            If (Not Informations.IsNothing(vConsultantCnt)) And (Not Object.Equals(vConsultantCnt, Nothing)) And (Not (Convert.IsDBNull(vConsultantCnt) Or Informations.IsNothing(vConsultantCnt))) Then



                m_lReturn = oSIRPartyCC.bSIRParty.GetOtherDetails(vAgentCnt:=vConsultantCnt, vAgentRef:=vConsultantRef, vAgentName:=vConsultantname)
            End If




            If (Not Informations.IsNothing(vInsurerCnt)) And (Not Object.Equals(vInsurerCnt, Nothing)) And (Not (Convert.IsDBNull(vInsurerCnt) Or Informations.IsNothing(vInsurerCnt))) Then



                m_lReturn = oSIRPartyCC.bSIRParty.GetOtherDetails(vAgentCnt:=vInsurerCnt, vAgentRef:=vInsurerRef, vAgentName:=vInsurername)
            End If




            If (Not Informations.IsNothing(vBrokerCnt)) And (Not Object.Equals(vBrokerCnt, Nothing)) And (Not (Convert.IsDBNull(vBrokerCnt) Or Informations.IsNothing(vBrokerCnt))) Then



                m_lReturn = oSIRPartyCC.bSIRParty.GetOtherDetails(vAgentCnt:=vBrokerCnt, vAgentRef:=vBrokerRef, vAgentName:=vBrokername)
            End If

            'DC 03/05/00
            'Cater for more than one Associate
            '    m_lReturn = oSIRPartyCC.bSIRParty.GetAssociateDetails(vPartyCnt:=vPartyCnt, _
            ''                                                    vIsAssociate:=PMTrue, _
            ''                                                    vAssociates:=vAssociates)

            m_lReturn = oSIRPartyCC.bSIRParty.GetAssociates(vPartyCnt:=vPartyCnt, vAssociates:=vAssociates)


            oSIRPartyCC = Nothing




            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetOtherDetailsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOtherDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim oSIRPartyCC As bSIRPartyCC.SIRPartyCC


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyCC - need to hit core for address stuff
            oSIRPartyCC = New bSIRPartyCC.SIRPartyCC()

            m_lReturn = oSIRPartyCC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            m_lReturn = oSIRPartyCC.bSIRParty.GetAddressDetails(vPartyCnt:=m_lPartyCnt, vAddresses:=vAddresses)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyCC = Nothing


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
        Dim oSIRPartyCC As bSIRPartyCC.SIRPartyCC


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyCC - need to hit core for address stuff
            oSIRPartyCC = New bSIRPartyCC.SIRPartyCC()

            m_lReturn = oSIRPartyCC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            m_lReturn = oSIRPartyCC.bSIRParty.GetAddressTypeLookups(vAddressTypes:=vAddressTypes)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyCC = Nothing


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetAddressTypeLookups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAddressTypeLookups", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DC 03/05/00
    ' ***************************************************************** '
    ' Name: GetRelationshipTypeLookups
    '
    ' Description: Get relationship type lookups.
    '
    ' ***************************************************************** '
    Public Function GetRelationshipTypeLookups(ByRef vRelationships As Object) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyCC As bSIRPartyCC.SIRPartyCC

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyCC - need to hit core for address stuff
            oSIRPartyCC = New bSIRPartyCC.SIRPartyCC()

            m_lReturn = oSIRPartyCC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            m_lReturn = oSIRPartyCC.bSIRParty.GetRelationshipTypeLookups(vRelationships:=vRelationships)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyCC = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetRelationshipTypeLookups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRelationshipTypeLookups", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLoyaltySchemeDetails
    '
    ' Description: Get LoyaltyScheme details for party.
    '
    ' ***************************************************************** '
    Public Function GetLoyaltySchemeDetails(ByRef vPartyCnt As Object, ByRef vLoyaltySchemes(,) As Object) As Integer  'RAW 18/11/2002 : PS005 : Added

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' This DB procedure accepts 3 OPTIONAL search parameters
                ' We only need to set PartyCnt

                .Parameters.Clear()




                'developer guide no. 85
                m_lReturn = .Parameters.Add("party_loyalty_scheme_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)


                'developer guide no. 98
                m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=vPartyCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                'developer guide no. 85 (Guide)
                m_lReturn = .Parameters.Add("loyalty_scheme_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                'Get all the LoyaltySchemes for the party
                m_lReturn = .SQLSelect(sSQL:=ACGetAllLoyaltySchemeSQL, sSQLName:=ACGetAllLoyaltySchemeName, bStoredProcedure:=ACGetAllLoyaltySchemeStored, lNumberRecords:=0, vResultArray:=vLoyaltySchemes)

            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetLoyaltySchemeDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLoyaltySchemeDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DC 28/06/00
    Public Function GetContactTypes(ByRef vContactTypes As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add the parameters
            m_oDatabase.Parameters.Clear()
            'developer guide no. 40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Effective_Date", vValue:=DateTime.Today, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Language_ID", vValue:=m_iLanguageID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            sSQL = "SELECT ct.contact_type_id, cap.caption, ct.code " &
                   "FROM contact_type ct, pmcaption cap " &
                   "WHERE ct.is_deleted = 0 " &
                   "AND ct.is_contact_type = 1 " &
                   "AND ct.effective_date <= {Effective_Date} " &
                   "AND ct.caption_id = cap.caption_id " &
                   "AND cap.language_id = {Language_ID}"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GETCONTACTTYPES", bStoredProcedure:=False, lNumberRecords:=500, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'return the data



            vContactTypes = vResultArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetContactTypesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetContactTypes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DC 28/06/00
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
            'developer guide no. 98
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Language_ID", vValue:=m_iLanguageID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            sSQL = "SELECT ct.contact_type_id, cap.caption, ct.code " &
                   "FROM contact_type ct, pmcaption cap " &
                   "WHERE ct.is_deleted = 0 " &
                   "AND ct.is_correspondence_type = 1 " &
                   "AND ct.effective_date <= {Effective_Date} " &
                   "AND ct.caption_id = cap.caption_id " &
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetCorrespondenceTypesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCorrespondenceTypes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim oSIRPartyCC As bSIRPartyCC.SIRPartyCC


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyCC - need to hit core for address stuff
            oSIRPartyCC = New bSIRPartyCC.SIRPartyCC()

            m_lReturn = oSIRPartyCC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            m_lReturn = oSIRPartyCC.bSIRParty.GetContactDetails(vPartyCnt:=m_lPartyCnt, vContacts:=vContacts)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyCC = Nothing


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetContactDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetContactDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'DC 04/08/00
    ' ***************************************************************** '
    ' Name: GetMainContact
    '
    ' Description: Get main contact details for party.
    '
    ' ***************************************************************** '
    Public Function GetMainContact(ByRef lMainContactCnt As Object, ByRef sMainContactDesc As Object) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyCC As bSIRPartyCC.SIRPartyCC

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyCC - need to hit core for address stuff
            oSIRPartyCC = New bSIRPartyCC.SIRPartyCC()

            m_lReturn = oSIRPartyCC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            m_lReturn = oSIRPartyCC.bSIRParty.GetMainContact(vPartyCnt:=m_lPartyCnt, lMainContactCnt:=lMainContactCnt, sMainContactDesc:=sMainContactDesc)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyCC = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetMainContact Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMainContact", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim oSIRPartyCC As bSIRPartyCC.SIRPartyCC


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyCC - need to hit core for address updates
            oSIRPartyCC = New bSIRPartyCC.SIRPartyCC()

            m_lReturn = oSIRPartyCC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            m_lReturn = oSIRPartyCC.bSIRParty.UpdateAddresses(vPartyCnt:=m_lPartyCnt, vAddAddresses:=vAddAddresses, vDeleteAddresses:=vDeleteAddresses)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyCC = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = RollbackTrans()


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateAddresses Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAddresses", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DC 03/05/00

    ' ***************************************************************** '
    ' Name: UpdateAssociates
    '
    ' Description: Update the party_Associate usage table with old
    ' and new Associates for the party.
    '
    ' ***************************************************************** '
    Public Function UpdateAssociates(ByRef vPartyCnt As Object, ByRef vAssociates As Object) As Integer
        'DC 29/03/00
        'Cater for more than one Associate
        'parameters were as follows :-
        '                               (vPartyCnt As Variant, _
        ''                                vIsAssociate As Variant, _
        ''                                vAssociatedCnt As Variant, _
        ''                                vAssociateDescription As Variant) As Long

        Dim result As Integer = 0
        Dim oSIRPartyCC As bSIRPartyCC.SIRPartyCC


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyCC - need to hit core for address updates
            oSIRPartyCC = New bSIRPartyCC.SIRPartyCC()

            m_lReturn = oSIRPartyCC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            'DC 29/03/00
            'Cater for more than one Associate
            'm_lReturn& = oSIRPartyCC.bSIRParty.UpdateAssociates( _
            ''                            vPartyCnt:=m_lPartyCnt&, _
            ''                            vIsAssociate:=vIsAssociate, _
            ''                            vAssociatedCnt:=vAssociatedCnt, _
            ''                            vAssociateDescription:=vAssociateDescription)


            m_lReturn = oSIRPartyCC.bSIRParty.UpdateAssociates(vPartyCnt:=m_lPartyCnt, vAssociates:=vAssociates)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyCC = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = RollbackTrans()

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateAssociates Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAssociates", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DC 04/08/00
    ' ***************************************************************** '
    ' Name: UpdateMainContact
    '
    ' Description: Update the contact table with main contact
    '
    ' ***************************************************************** '
    Public Function UpdateMainContact(ByRef vPartyCnt As Object, ByRef lMainContactCnt As Object, ByRef sMainContactDesc As Object) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyCC As bSIRPartyCC.SIRPartyCC


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyCC - need to hit core for main contact updates
            oSIRPartyCC = New bSIRPartyCC.SIRPartyCC()

            m_lReturn = oSIRPartyCC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            m_lReturn = oSIRPartyCC.bSIRParty.UpdateMainContact(vPartyCnt:=m_lPartyCnt, lMainContactCnt:=lMainContactCnt, sMainContactDesc:=sMainContactDesc)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyCC = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = RollbackTrans()

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateMainContact Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateMainContact", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateGemini
    '
    ' Description: Update Gemini
    '
    ' ***************************************************************** '
    Public Function UpdateGemini(ByRef vPartyCnt As Object, ByRef vTask As Object) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyCC As bSIRPartyCC.SIRPartyCC


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyCC - need to hit core to update gemini
            oSIRPartyCC = New bSIRPartyCC.SIRPartyCC()

            m_lReturn = oSIRPartyCC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oSIRPartyCC.bSIRParty.UpdateGemini(vPartyCnt:=vPartyCnt, vTask:=vTask)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyCC = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateGemini Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateGemini", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim oSIRPartyCC As bSIRPartyCC.SIRPartyCC


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyCC - need to hit core for address updates
            oSIRPartyCC = New bSIRPartyCC.SIRPartyCC()

            m_lReturn = oSIRPartyCC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            m_lReturn = oSIRPartyCC.bSIRParty.UpdateContacts(vPartyCnt:=m_lPartyCnt, vAddContacts:=vAddContacts, vDeleteContacts:=vDeleteContacts)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyCC = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = RollbackTrans()


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateContacts Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateContacts", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'EK 9/12/99
    ' ***************************************************************** '
    ' Name: GetConvictionDetails
    '
    ' Description: Get Conviction details for party.
    '
    ' ***************************************************************** '
    Public Function GetConvictionDetails(ByRef vPartyCnt As Object, ByRef vConviction(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()


                m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=vPartyCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Get all the Lifestyles for the party
                m_lReturn = .SQLSelect(sSQL:=ACGetAllConvictionSQL, sSQLName:=ACGetAllConvictionName, bStoredProcedure:=ACGetAllConvictionStored, lNumberRecords:=0, vResultArray:=vConviction)

            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'log if no relationships, but dont fail
            If Not Informations.IsArray(vConviction) Then
                'Start Girija --- PN 54576
                'LogMessage m_sUsername, _
                'iType:=PMLogInfo, _
                'sMsg:="There are no Conviction details for this party, party_cnt=" & CLng(vPartyCnt), _
                'vApp:=ACApp, _
                'vClass:=ACClass, _
                'vMethod:="GetConvictionDetails", _
                'vErrNo:=Err.Number, _
                'vErrDesc:=Err.Description
                'End Girija --- PN 54576
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetConvictionDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetConvictionDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function




    ' ***************************************************************** '
    ' Name: CheckMandatory (Private)
    '
    ' Description: Check Mandatory parameters have been passed.
    '
    ' AMB 21-Oct-03: 1.8.6 Folgate EL0037 development - added trade_id
    ' ***************************************************************** '
    'DC 28/06/00 Added Correspondence Type Id
    'UPGRADE_NOTE: (7001) The following declaration (CheckMandatory) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CheckMandatory(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vCompanyReg As Object = Nothing, Optional ByRef vTradingSinceDate As Object = Nothing, Optional ByRef vPartyBusinessId As Object = Nothing, Optional ByRef vLocation As Object = Nothing, Optional ByRef vNoOfOffices As Object = Nothing, Optional ByRef vNoOfEmployees As Object = Nothing, Optional ByRef vFinancialYear As Object = Nothing, Optional ByRef vTradeCode As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vShortname As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vResolvedName As Object = Nothing, Optional ByRef vIsAlsoAgent As Object = Nothing, Optional ByRef vIsProspect As Object = Nothing, Optional ByRef vAgentCnt As Object = Nothing, Optional ByRef vConsultantCnt As Object = Nothing, Optional ByRef vFileCode As Object = Nothing, Optional ByRef vCurrencyId As Object = Nothing, Optional ByRef vPaymentMethodCode As Object = Nothing, Optional ByRef vReminderTypeId As Object = Nothing, Optional ByRef vAreaId As Object = Nothing, Optional ByRef vServiceLevelId As Object = Nothing, Optional ByRef vCreditCardCode As Object = Nothing, Optional ByRef vPaymentTermCode As Object = Nothing, Optional ByRef vCCJs As Object = Nothing, Optional ByRef vWageRoll As Object = Nothing, Optional ByRef vTurnover As Object = Nothing, Optional ByRef vSeasonalGiftID As Object = Nothing, Optional ByRef vStrengthCodeId As Object = Nothing, Optional ByRef vSICCodeId As Object = Nothing, Optional ByRef vPreviousInsurerCnt As Object = Nothing, Optional ByRef vPreviousBrokerCnt As Object = Nothing, Optional ByRef vCorrespondenceTypeId As Object = Nothing, Optional ByRef vRenewalStopCodeId As Object = Nothing, Optional ByRef vSalutation As Object = Nothing, Optional ByRef vTradeID As Object = Nothing) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' {* USER DEFINED CODE (Begin) *}
    '


    'If (Informations.IsNothing(vTradeCode)) Or (Object.Equals(vTradeCode, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '


    'If (Informations.IsNothing(vCompanyReg)) Or (Object.Equals(vCompanyReg, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '
    ' DC 16/03/00
    ' Trading Since Date is no longer mandatory
    '    If (IsMissing(vTradingSinceDate) = True) _
    ''    Or (IsEmpty(vTradingSinceDate) = True) Then
    '        CheckMandatory = PMMandatoryMissing
    '        Exit Function
    '    End If
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

    ' ***************************************************************** '
    ' Name: CreateEvent (Private)
    '
    ' Description: Create an event record.
    '
    ' ***************************************************************** '










    'developer guide no. 119 (Guide)
    Private Function CreateEvent(ByRef r_lEventCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_dtEventDate As Date, ByVal v_lEventTypeId As Integer, Optional ByVal v_vInsuranceFolderCnt As Object = Nothing, Optional ByVal v_vInsuranceFileCnt As Object = Nothing, Optional ByVal v_vClaimCnt As Object = Nothing, Optional ByVal v_vDocumentCnt As Object = Nothing, Optional ByVal v_vOldAddressCnt As Object = Nothing, Optional ByVal v_vNewAddressCnt As Object = Nothing, Optional ByVal v_vCampaignId As Object = Nothing, Optional ByVal v_vDocumentTypeId As Object = Nothing, Optional ByVal v_vReportTypeId As Object = Nothing, Optional ByVal v_vDescription As Object = Nothing) As Integer

        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        If m_oEvent Is Nothing Then
            m_oEvent = New bSIREvent.Business()
            'developer guide no.67(guide) 
            m_lReturn = m_oEvent.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, MainModule.ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oEvent.Initialise", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

        m_lReturn = m_oEvent.DirectAdd(vEventCnt:=r_lEventCnt, vPartyCnt:=v_lPartyCnt, vInsuranceFolderCnt:=v_vInsuranceFolderCnt, vInsuranceFileCnt:=v_vInsuranceFileCnt, vClaimCnt:=v_vClaimCnt, vDocumentCnt:=v_vDocumentCnt, vOldAddressCnt:=v_vOldAddressCnt, vNewAddressCnt:=v_vNewAddressCnt, vCampaignId:=v_vCampaignId, vDocumentType:=v_vDocumentTypeId, vReportType:=v_vReportTypeId, vEventType:=v_lEventTypeId, vUserId:=m_iUserID, vEventDate:=v_dtEventDate, vDescription:=v_vDescription)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oEvent.DirectAdd", "vPartyCnt:=" & v_lPartyCnt, gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Do any tidy up, e.g. Set x = Nothing here
        Return result
        ' This is for debugging only
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
    ' ***************************************************************** '
    Public Function getUnderwritingOrAgency() As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'sj 19/06/2002 - start
            m_lReturn = bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, r_vUnderwriting:=m_sUnderwritingOrAgency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bPMFunc.getUnderwritingOrAgency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUnderwritingOrAgency")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    m_sUnderwritingOrAgency = "A"
            '
            '    m_oDatabase.Parameters.Clear
            '
            '    sSQL = "SELECT value FROM hidden_options"
            '
            '    m_lReturn& = m_oDatabase.SQLSelect( _
            ''        sSQL:=sSQL, _
            ''        sSQLName:="GetHiddenOption", _
            ''        bStoredProcedure:=False)
            '
            '    If (m_lReturn& <> PMTrue) Then
            '        Exit Function
            '        ' Carry on without default set
            '    End If
            '
            '    If (m_oDatabase.Records.Count = 1) Then
            '        ' select first letter of the return field
            '        m_sUnderwritingOrAgency = Left$(CStr(m_oDatabase.Records.Item(1).Fields.Item("value").Value), 1)
            '    End If
            '
            '    If ((m_sUnderwritingOrAgency <> "A") And (m_sUnderwritingOrAgency <> "U")) Then
            '        m_sUnderwritingOrAgency = "A"
            '    End If
            'sj 19/06/2002 - end

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnderwritingOrAgencyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnderwritingOrAgency", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

    Public Function GetCorePartyDetails(ByRef vPartyCnt As Object, Optional ByRef vPartyTypeID As Object = Nothing, Optional ByRef vPartyStructureID As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vIsAlsoAgent As Object = Nothing, Optional ByRef vPartyID As Object = Nothing, Optional ByRef vShortname As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vResolvedName As Object = Nothing, Optional ByRef vCurrencyId As Object = Nothing, Optional ByRef vLanguageID As Object = Nothing, Optional ByRef vCollectTypeID As Object = Nothing, Optional ByRef vAccumTreatmentTypeID As Object = Nothing, Optional ByRef vStatsTreatmentTypeID As Object = Nothing, Optional ByRef vPartyCategoryID As Object = Nothing, Optional ByRef vAgentCnt As Object = Nothing, Optional ByRef vConsultantCnt As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vPaymentMethodCode As Object = Nothing, Optional ByRef vPaymentTermCode As Object = Nothing, Optional ByRef vCreditCardCode As Object = Nothing, Optional ByRef vFileCode As Object = Nothing, Optional ByRef vABCCount As Object = Nothing, Optional ByRef vStatements As Object = Nothing, Optional ByRef vReminderTypeId As Object = Nothing, Optional ByRef vRenewals As Object = Nothing, Optional ByRef vStatus As Object = Nothing, Optional ByRef vLAstActionType As Object = Nothing, Optional ByRef vIsTravelAgent As Object = Nothing, Optional ByRef vIsProspect As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vABICodeOn406 As Object = Nothing, Optional ByRef vABICodeOn81 As Object = Nothing, Optional ByRef vABICodeList As Object = Nothing, Optional ByRef vAreaId As Object = Nothing, Optional ByRef vServiceLevelId As Object = Nothing, Optional ByRef vInvariantKey As Object = Nothing, Optional ByRef vRecordStatus As Object = Nothing, Optional ByRef vCCJs As Object = Nothing, Optional ByRef vUserDefinedDataId As Object = Nothing, Optional ByRef vSeasonalGiftID As Object = Nothing, Optional ByRef vCorrespondenceTypeId As Object = Nothing, Optional ByRef vRenewalStopCodeId As Object = Nothing, Optional ByRef vSwiftPartyID As Object = Nothing, Optional ByRef vLoyaltyNumber As Object = Nothing, Optional ByRef vAlternativeIdentifier As Object = Nothing, Optional ByRef vMarketingSegementInd As Object = Nothing, Optional ByRef vTradingName As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim oSIRPartyCC As bSIRPartyCC.SIRPartyCC

            oSIRPartyCC = New bSIRPartyCC.SIRPartyCC()
            m_lReturn = oSIRPartyCC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oSIRPartyCC.Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCorePartyDetails")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oSIRPartyCC.bSIRParty.GetDetails(vPartyCnt:=vPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oSIRPartyCC.bSIRParty.GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCorePartyDetails")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oSIRPartyCC.bSIRParty.GetNext(vSourceID:=vSourceID, vPartyID:=vPartyID, vShortname:=vShortname, vName:=vName, vIsAlsoAgent:=vIsAlsoAgent, vIsProspect:=vIsProspect, vAgentCnt:=vAgentCnt, vConsultantCnt:=vConsultantCnt, vFileCode:=vFileCode, vCurrencyId:=vCurrencyId, vPaymentMethodCode:=vPaymentMethodCode, vReminderTypeId:=vReminderTypeId, vAreaId:=vAreaId, vServiceLevelId:=vServiceLevelId, vCreditCardCode:=vCreditCardCode, vPaymentTermCode:=vPaymentTermCode, vCCJs:=vCCJs, vSeasonalGiftID:=vSeasonalGiftID, vCorrespondenceTypeId:=vCorrespondenceTypeId, vRenewalStopCodeId:=vRenewalStopCodeId, vResolvedName:=vResolvedName, vSwiftPartyID:=vSwiftPartyID, vLoyaltyNumber:=vLoyaltyNumber, vAlternativeIdentifier:=vAlternativeIdentifier, vMarketingSegementInd:=vMarketingSegementInd, vTradingName:=vTradingName, vSubBranchID:=vSubBranchID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oSIRPartyCC.bSIRParty.GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCorePartyDetails")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyCC = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCorePartyDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCorePartyDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetNumberValidationScripts
    '
    ' Description:
    '
    ' History: 12/06/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function GetNumberValidationScripts(ByVal v_sBranchPrefix As String, ByRef r_sLoyaltyNumberScript As String, ByRef r_sAlternativeIdentifierScript As String) As Integer

        Dim result As Integer = 0
        Try



            Return SiriusCoreFunc.GetNumberValidationScripts(v_sBranchPrefix:=v_sBranchPrefix, r_sLoyaltyNumberScript:=r_sLoyaltyNumberScript, r_sAlternativeIdentifierScript:=r_sAlternativeIdentifierScript)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNumberValidationScripts Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNumberValidationScripts", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: GetFutureDatedAddresses
    '
    ' Description:
    '
    ' History: 18/07/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function GetFutureDatedAddresses(ByRef r_vFutureDatedAddresses As Object, Optional ByVal v_vPartyCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim oSIRPartyCC As bSIRPartyCC.SIRPartyCC

            oSIRPartyCC = New bSIRPartyCC.SIRPartyCC()

            m_lReturn = oSIRPartyCC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oSIRPartyCC.Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFutureDatedAddresses")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oSIRPartyCC.bSIRParty.GetFutureDatedAddresses(r_vFutureDatedAddresses:=r_vFutureDatedAddresses, v_vPartyCnt:=v_vPartyCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetFutureDatedAddresses Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFutureDatedAddresses")
                Return result
            End If

            oSIRPartyCC = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFutureDatedAddresses Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFutureDatedAddresses", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CreateFutureDatedAddresses
    '
    ' Description:
    '
    ' History: 17/07/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function CreateFutureDatedAddresses(ByVal v_lPartyCnt As Integer, ByVal v_vFutureDatedAddresses As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim oSIRPartyCC As bSIRPartyCC.SIRPartyCC

            oSIRPartyCC = New bSIRPartyCC.SIRPartyCC()

            m_lReturn = oSIRPartyCC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oSIRPartyCC.Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateFutureDatedAddresses")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oSIRPartyCC.bSIRParty.CreateFutureDatedAddresses(v_lPartyCnt:=ToSafeInteger(v_lPartyCnt), v_vFutureDatedAddresses:=v_vFutureDatedAddresses)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="CreateFutureDatedAddresses Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateFutureDatedAddresses")
                Return result
            End If

            oSIRPartyCC = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateFutureDatedAddresses Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateFutureDatedAddresses", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: GetReadTradeSysOptions
    '
    ' Description: Get the system options for Accident Management
    '
    ' AMB 10-Oct-03: 1.8.6 Accident Management development - created
    ' ***************************************************************** '
    Public Function GetReadTradeSysOptions(ByRef r_bReadTradeABIEnabled As Boolean) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sOptionValue1 As String = ""

        ' system option numbers
        Const klReadTradeABIListEnabled As Integer = 90

        Try

            ' is read trade list from ABI enabled?
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=klReadTradeABIListEnabled, r_sOptionValue:=sOptionValue1)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_bReadTradeABIEnabled = gPMFunctions.NullToBoolean(sOptionValue1)


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetReadTradeSysOptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReadTradeSysOptions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckDuplicateShortname
    '
    ' Description: Check for Duplicate Clients on the basis of shortname
    '
    ' History: 19/01/2005 JT - Created.
    '
    ' ***************************************************************** '
    Public Function CheckDuplicateShortname(ByVal v_sShortname As String, ByRef v_vMatchArray As Object) As Integer
        Dim result As Integer = 0
        Dim oSIRPartyCC As bSIRPartyCC.SIRPartyCC

        Try

            oSIRPartyCC = New bSIRPartyCC.SIRPartyCC()
            m_lReturn = oSIRPartyCC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oSIRPartyCC.Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckDuplicateShortname")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            result = oSIRPartyCC.bSIRParty.CheckDuplicateShortname(ToSafeString(v_sShortname), v_vMatchArray)


            oSIRPartyCC.Dispose()
            oSIRPartyCC = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckDuplicateShortname Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckDuplicateShortname", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
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



            lReturn = m_oSIRParty.GetPartyDetails(v_lPartyCnt:=v_lPartyCnt, r_vResults:=r_vResults)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetPartyDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result
            '		Resume 
            '		Return result
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


            lReturn = m_oSIRParty.UpdatePartyDetails(v_lPartyCnt:=v_lPartyCnt, v_vPartyDetails:=v_vPartyDetails)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "UpdatePartyDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



            Return result

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function
End Class
