Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
Imports System.Text
'developer guide no. 129
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
    '              a SIRPartyPC.
    '
    ' Edit History:
    ' SP191198 - Call to UpdateGemini should be in bSIRPartyPC not
    ' bSIRParty
    ' RAW 18/11/2002 : PS005 : Added loyalty scheme
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 11/02/2004
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

    ' Collection of SIRPartyPCs (Private)

    Private m_oSIRPartyPCs As bSIRPartyPC.SIRPartyPCs

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As gPMConstants.PMEReturnCode

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

    Private m_bEvent As Boolean
    'Addresses - needed for events.
    Private m_vOldAddress(,) As Object
    Private m_vNewAddress(,) As Object
    Private m_sOldClientCode As String = ""
    Private m_sNewClientCode As String = ""
    Private m_sOldClientName As String = ""
    Private m_sNewClientName As String = ""

    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business

    ' PM Event Business Component (Private)
    Private m_oEvent As bSIREvent.Business
    'Private m_oEvent As bSIREvent.Business

    'MSS200901 - Added for merge
    Private m_sUnderwritingOrBroking As String = ""
    'MSS200901 - Merge End

    Private m_bIsOrionInstalled As Boolean

    Private m_oSIRParty As bSIRParty.Business
    Private m_bIsAmended As Boolean
    '****************************************************************************
    ' Name: GetNextClientCode
    '
    ' Description: Get the next available client code from the unique
    '              numbers table
    '
    ' History: PW180303 - created (PS186)
    '****************************************************************************
    Public Function GetNextClientCode(ByRef r_lClientCode As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the parameter collection
            m_oDatabase.Parameters.Clear()

            ' Add the table name parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="table_name", vValue:="Client_Code", idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the next number OUTPUT parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="next_number", vValue:=CStr(r_lClientCode), idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetNextClientCodeSQL, sSQLName:=ACGetNextClientCodeName, bStoredProcedure:=ACGetNextClientCodeStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Retrieve the next number

            If Convert.IsDBNull(m_oDatabase.Parameters.Item("next_number").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("next_number").Value) Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetNextClientCode returned 0", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNextClientCode")
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                r_lClientCode = m_oDatabase.Parameters.Item("next_number").Value
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetNextClientCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNextClientCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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
                Case Is > m_oSIRPartyPCs.Count()
                    m_lCurrentRecord = m_oSIRPartyPCs.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Number in Collection
            Return m_oSIRPartyPCs.Count()

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

    Public WriteOnly Property IsAmended() As Boolean
        Set(ByVal Value As Boolean)

            m_bIsAmended = Value

        End Set
    End Property
    'MSS200901 - Added for merge
    Public ReadOnly Property UnderwritingOrBroking() As String
        Get

            If m_sUnderwritingOrBroking = "" Then
                m_lReturn = GetUnderwritingOrBroking()
            End If

            Return m_sUnderwritingOrBroking

        End Get
    End Property
    'MSS200901 - Merge End

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


            ' Set Username and Password

            m_lCurrentRecord = gPMConstants.PMEReturnCode.PMFalse
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create SIRPartyPCs Collection
            m_oSIRPartyPCs = New bSIRPartyPC.SIRPartyPCs()

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
                m_oSIRPartyPCs = Nothing
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
    '
    ' Description: Gets the Lookup values for a SIRPartyPC.
    '
    ' ***************************************************************** '
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyPC As bSIRPartyPC.SIRPartyPC = Nothing
        Dim dtEffectiveDate As Date

        Dim vTabArray(4, 11) As Object
        Dim vAreaId As Object=Nothing
        Dim vCurrencyId As Object = Nothing
        Dim vReminderTypeId As Object = Nothing
        Dim vServiceLevelId As Object = Nothing
        'Dim vEmployerBusiness, vSecondaryEmployerBusiness As Object
        Dim vNationalityId As Object = Nothing
        'Dim vPartyOccupationID As Object
        Dim vSeasonalGiftID As Object = Nothing
        Dim vStrengthCodeId As Object = Nothing
        'Dim vCorrespondenceTypeId As Object
        Dim vRenewalStopCodeId As Object = Nothing
        Dim vBlackListReasonId As Object = Nothing
        Dim vTermsOfPaymentId As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Setup Lookup Table Names
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = gSIRLibrary.SIRLookupArea
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 1) = gSIRLibrary.SIRLookupCurrency
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 2) = gSIRLibrary.SIRLookupReminderType
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 3) = gSIRLibrary.SIRLookupServiceLevel
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 4) = gSIRLibrary.SIRLookupNationality
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 5) = gSIRLibrary.SIRLookupProspectStatus
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 6) = gSIRLibrary.SIRLookupRiskGroup
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 7) = gSIRLibrary.SIRLookupSeasonalGift
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 8) = gSIRLibrary.SIRLookupStrengthCode
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
                oSIRPartyPC = m_oSIRPartyPCs.Item(m_lCurrentRecord)
            End If

            Select Case iLookupType
                Case gPMConstants.PMELookupType.PMLookupAll

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = 0
                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = 0
                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 2) = 0
                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 3) = 0
                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 4) = 0
                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 5) = 0
                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 6) = 0
                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 7) = 0
                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 8) = 0
                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 9) = 0
                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 10) = 0
                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 11) = 0

                Case gPMConstants.PMELookupType.PMLookupAllEffective

                    ' Use keys and effective date from current record
                    ' Note: The keys are not used for the select, but are used by
                    '       the interface program to set the list index.
                    With oSIRPartyPC

                        m_lReturn = .bSIRParty.GetLookupProperties(vCurrentRecord:=ToSafeInteger(m_lCurrentRecord), vAreaId:=vAreaId, vCurrencyId:=vCurrencyId, vReminderTypeId:=vReminderTypeId,
                                                                   vServiceLevelId:=vServiceLevelId, vSeasonalGiftID:=vSeasonalGiftID, vRenewalStopCodeId:=vRenewalStopCodeId,
                                                                    vCorrespondenceTypeId:=Nothing,
                                                                   vPaymentTermCode:=vTermsOfPaymentId)

                        If Convert.IsDBNull(vSeasonalGiftID) Or Informations.IsNothing(vSeasonalGiftID) Then
                            vSeasonalGiftID = 0
                        End If

                        vStrengthCodeId = 0

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = vAreaId
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = vCurrencyId
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 2) = vReminderTypeId
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 3) = vServiceLevelId

                        m_lReturn = .GetProperties(iStatus:=gPMConstants.PMEComponentAction.PMView, vNationalityId:=vNationalityId)

                        If Convert.IsDBNull(vNationalityId) Or Informations.IsNothing(vNationalityId) Then
                            vNationalityId = 0
                        End If

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 4) = vNationalityId
                        'Get the full prospect status list anyway
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 5) = 0
                        'And the full policy type list
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 6) = 0
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 7) = vSeasonalGiftID
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 8) = 0
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 9) = vRenewalStopCodeId
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 10) = gPMFunctions.ToSafeLong(vBlackListReasonId, 0)
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 11) = gPMFunctions.ToSafeLong(vTermsOfPaymentId, 0)
                    End With

                Case gPMConstants.PMELookupType.PMLookupSingle

                    ' Set keys from current record
                    With oSIRPartyPC

                        m_lReturn = .bSIRParty.GetLookupProperties(vCurrentRecord:=ToSafeInteger(m_lCurrentRecord), vAreaId:=vAreaId, vCurrencyId:=vCurrencyId, vReminderTypeId:=vReminderTypeId,
                                                                   vServiceLevelId:=vServiceLevelId, vSeasonalGiftID:=vSeasonalGiftID, vRenewalStopCodeId:=vRenewalStopCodeId,
                                                                   vCorrespondenceTypeId:=Nothing,
                                                                   vPaymentTermCode:=vTermsOfPaymentId)

                        If vAreaId = 0 Then
                            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = 1
                        Else
                            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = vAreaId
                        End If

                        If vCurrencyId = 0 Then
                            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = 1
                        Else
                            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = vCurrencyId
                        End If

                        If vReminderTypeId = 0 Then
                            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 2) = 1
                        Else
                            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 2) = vReminderTypeId
                        End If

                        If vServiceLevelId = 0 Then
                            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 3) = 1
                        Else
                            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 3) = vServiceLevelId
                        End If

                        m_lReturn = .GetProperties(iStatus:=gPMConstants.PMEComponentAction.PMView, vNationalityId:=vNationalityId)

                        If vNationalityId = 0 Then
                            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 4) = 1
                        Else
                            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 4) = vNationalityId
                        End If

                        'Get the full prospect status list anyway
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 5) = "1"
                        'And the full policy type list
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 6) = "1"
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 7) = vSeasonalGiftID
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 8) = "1"
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 9) = vRenewalStopCodeId
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 10) = gPMFunctions.ToSafeLong(vBlackListReasonId, 0)
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 11) = gPMFunctions.ToSafeLong(vTermsOfPaymentId, 0)
                    End With

            End Select

            ' Default Effective Date to current date/time
            dtEffectiveDate = DateTime.Now

            ' Release SIRPartyPC reference
            oSIRPartyPC = Nothing

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

            ' Do any tidy up, e.g. Set x = Nothing here

            '		Return result

            ' This is for debugging only
            '		Resume 

            '		Return result
        End Try
        Return result


    End Function

    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single SIRPartyPC directly into the database.
    '        Note: The SIRPartyPC will NOT be added to the collection.
    '
    ' sj 8/5/99 - added invariant key paramter
    ' ***************************************************************** '
    'DC 28/06/00 Added Correspondence Type Id
    Public Function DirectAdd(ByRef v_vFieldArray() As Object) As Integer

        Dim result As Integer = 0
        Dim vPartyCnt As Object
        Dim vPartyTitleCode As Object
        Dim vForename As Object
        Dim vInitials As Object
        Dim vEmploymentStatusCode As Object
        Dim vEmployerCnt As Object
        Dim vEmployerBusiness As Object
        Dim vSecondaryEmploymentStatusC As Object
        Dim vSecondaryEmployerBusiness As Object
        Dim vMaritalStatusCode As Object
        Dim vNumberOfChildren As Object
        Dim vNationalityId As Object
        Dim vCountryOfOriginCode As Object
        Dim vMailshot As Object
        Dim vIsPetOwner As Object
        Dim vAccommodationTypeCode As Object
        Dim vShortname As Object
        Dim vName As Object
        Dim vResolved As Object
        Dim vIsAlsoAgent As Object
        Dim vIsProspect As Object
        Dim vAgentCnt As Object
        Dim vConsultantCnt As Object
        Dim vFileCode As Object
        Dim vCurrencyId As Object
        Dim vPaymentMethodCode As Object
        Dim vReminderTypeId As Object
        Dim vAreaId As Object
        Dim vServiceLevelId As Object
        Dim vCreditCardCode As Object
        Dim vPaymentTermCode As Object
        Dim vCCJs As Object
        Dim vPartyLifestyleId As Object
        Dim vPartyLifestyleName As Object
        Dim vCategory As Object
        Dim vDateOfBirth As Object
        Dim vGender As Object
        Dim vOccupationCode As Object
        Dim vSecondaryOccupationCode As Object
        Dim vIsSmoker As Object
        Dim vStatus As Object
        Dim vABCCount As Object
        Dim vStatements As Object
        Dim vRenewals As Object
        Dim vLastModified As Object
        Dim vLAstActionType As Object
        Dim vDateCreated As Object
        Dim vInvariantKey As Object
        Dim vSeasonalGiftID As Object
        Dim vCorrespondenceTypeId As Object
        Dim vRenewalStopCodeId As Object
        Dim vSwiftPartyID As Object
        Dim vSalutation As Object
        Dim vSource As Object
        Dim vTPSind As Object
        Dim vEMPSind As Object
        Dim vTPPassword As Object
        Dim vLoyaltyNumber As Object
        Dim vAlternativeIdentifier As Object
        Dim vMarketingSegmentId As Object
        Dim vTradingName As Object
        Dim vSubBranchId As Object
        Dim vTobLetter As Object
        Dim vIsFeeClient As Object

        Dim oSIRPartyPC As bSIRPartyPC.SIRPartyPC
        Dim lPartyTypeId As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            result = gPMConstants.PMEReturnCode.PMTrue


            vPartyCnt = v_vFieldArray(AC_PARTYPC_PARTYCNT)

            vPartyTitleCode = v_vFieldArray(AC_PARTYPC_PARTYTITLECODE)

            vForename = v_vFieldArray(AC_PARTYPC_FORENAME)

            vInitials = v_vFieldArray(AC_PARTYPC_INITIALS)

            vEmploymentStatusCode = v_vFieldArray(AC_PARTYPC_EMPLOYMENTSTATUSCODE)

            vEmployerCnt = v_vFieldArray(AC_PARTYPC_EMPLOYERCNT)

            vEmployerBusiness = v_vFieldArray(AC_PARTYPC_EMPLOYERBUSINESS)

            vSecondaryEmploymentStatusC = v_vFieldArray(AC_PARTYPC_SECONDEMPLOYERBUSINESSSTATUS)

            vSecondaryEmployerBusiness = v_vFieldArray(AC_PARTYPC_SECONDEMPLOYERBUSINESS)

            vMaritalStatusCode = v_vFieldArray(AC_PARTYPC_MARITALSTATUSCODE)

            vNumberOfChildren = v_vFieldArray(AC_PARTYPC_NUMBERCHILDREN)

            vNationalityId = v_vFieldArray(AC_PARTYPC_NATIONALITYID)

            vCountryOfOriginCode = v_vFieldArray(AC_PARTYPC_COUNTRYOFORIGIN)

            vMailshot = v_vFieldArray(AC_PARTYPC_MAILSHOT)

            vIsPetOwner = v_vFieldArray(AC_PARTYPC_PETOWNER)

            vAccommodationTypeCode = v_vFieldArray(AC_PARTYPC_ACCOMMODATIONTYPECODE)

            vShortname = v_vFieldArray(AC_PARTYPC_SHORTNAME)

            vName = v_vFieldArray(AC_PARTYPC_NAME)

            vResolved = v_vFieldArray(AC_PARTYPC_RESOLVED)

            vIsAlsoAgent = v_vFieldArray(AC_PARTYPC_ISALSOAGENT)

            vIsProspect = v_vFieldArray(AC_PARTYPC_ISPROSPECT)

            vAgentCnt = v_vFieldArray(AC_PARTYPC_AGENTCNT)

            vConsultantCnt = v_vFieldArray(AC_PARTYPC_CONSULTANTCNT)

            vFileCode = v_vFieldArray(AC_PARTYPC_FILECODE)

            vCurrencyId = v_vFieldArray(AC_PARTYPC_CURRENCYID)

            vPaymentMethodCode = v_vFieldArray(AC_PARTYPC_PAYMENTMETHODCODE)

            vReminderTypeId = v_vFieldArray(AC_PARTYPC_REMINDERTYPEID)

            vAreaId = v_vFieldArray(AC_PARTYPC_AREAID)

            vServiceLevelId = v_vFieldArray(AC_PARTYPC_SERVICELEVELID)

            vCreditCardCode = v_vFieldArray(AC_PARTYPC_CREDITCARDCODE)

            vPaymentTermCode = v_vFieldArray(AC_PARTYPC_PAYMENTTERMCODE)

            vCCJs = v_vFieldArray(AC_PARTYPC_CCJS)

            vPartyLifestyleId = v_vFieldArray(AC_PARTYPC_LIFESTYLEID)

            vPartyLifestyleName = v_vFieldArray(AC_PARTYPC_LIFESTYLENAME)

            vCategory = v_vFieldArray(AC_PARTYPC_CATEGORY)

            vDateOfBirth = v_vFieldArray(AC_PARTYPC_DATEOFBIRTH)

            vGender = v_vFieldArray(AC_PARTYPC_GENDER)

            vOccupationCode = v_vFieldArray(AC_PARTYPC_OCCUPATIONCODE)

            vSecondaryOccupationCode = v_vFieldArray(AC_PARTYPC_SECONDARYOCCUPATIONCODE)

            vIsSmoker = v_vFieldArray(AC_PARTYPC_ISSMOKER)

            vStatus = v_vFieldArray(AC_PARTYPC_STATUS)

            vABCCount = v_vFieldArray(AC_PARTYPC_ABCCOUNT)

            vStatements = v_vFieldArray(AC_PARTYPC_STATEMENTS)

            vRenewals = v_vFieldArray(AC_PARTYPC_RENEWALS)

            vLastModified = v_vFieldArray(AC_PARTYPC_LASTMODIFIED)

            vLAstActionType = v_vFieldArray(AC_PARTYPC_LASTACTIONTYPE)

            vDateCreated = v_vFieldArray(AC_PARTYPC_DATECREATED)

            vInvariantKey = v_vFieldArray(AC_PARTYPC_INVARIANTKEY)

            vSeasonalGiftID = v_vFieldArray(AC_PARTYPC_SEASONALGIFTID)

            vCorrespondenceTypeId = v_vFieldArray(AC_PARTYPC_CORRESPONDENCETYPEID)

            vRenewalStopCodeId = v_vFieldArray(AC_PARTYPC_RENEWALSTOPCODEID)

            vSwiftPartyID = v_vFieldArray(AC_PARTYPC_SWIFTPARTYID)

            vSalutation = v_vFieldArray(AC_PARTYPC_SALUTATION)

            vSource = v_vFieldArray(AC_PARTYPC_SOURCE)

            vTPSind = v_vFieldArray(AC_PARTYPC_TPSIND)

            vEMPSind = v_vFieldArray(AC_PARTYPC_EMPSIND)

            vTPPassword = v_vFieldArray(AC_PARTYPC_TPPASSWORD)

            vLoyaltyNumber = v_vFieldArray(AC_PARTYPC_LOYALTYNUMBER)

            vAlternativeIdentifier = v_vFieldArray(AC_PARTYPC_ALTERNATIVEIDENTIFIER)

            vMarketingSegmentId = v_vFieldArray(AC_PARTYPC_MARKETINGSEGMENTIND)

            vTradingName = v_vFieldArray(AC_PARTYPC_TRADINGNAME)

            vSubBranchId = v_vFieldArray(AC_PARTYPC_SUBBRANCHID)

            vTobLetter = v_vFieldArray(AC_PARTYPC_TOBLETTER)

            vIsFeeClient = v_vFieldArray(AC_PARTYPC_ISFEECLIENT)

            'EK 22/9/99
            'Get party type id for a private individual
            m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupPartyType, v_sCode:=gSIRLibrary.SIRPartyTypePersonalClient, v_dtEffectiveDate:=DateTime.Now, r_lID:=lPartyTypeId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Create a new SIRPartyPC
            oSIRPartyPC = New bSIRPartyPC.SIRPartyPC()
            m_lReturn = oSIRPartyPC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
            'EK 22/9/99 Added partytypeId
            'DC 08/10/99 Added AreaId
            'sj 8/11/99 Added invariant key
            'DC 28/06/00 Added Correspondence Type Id
            ' CTAF 250701 - Added salutation
            ' CJB  240901 - Removed salutation from being passed to bSIRParty since it should not be and causes failure for Gnet!
            ' Added currencyID as it was not being passed through. PW080206.

            m_lReturn = oSIRPartyPC.bSIRParty.DirectAdd(vPartyTypeID:=ToSafeInteger(lPartyTypeId), vShortname:=vShortname, vName:=vName, vResolvedName:=vResolved, vIsAlsoAgent:=vIsAlsoAgent, vIsProspect:=vIsProspect, vAgentCnt:=vAgentCnt, vConsultantCnt:=vConsultantCnt, vFileCode:=vFileCode, vABCCount:=vABCCount, vStatements:=vStatements, vRenewals:=vRenewals, vStatus:=vStatus, vLastModified:=vLastModified, vLAstActionType:=vLAstActionType, vDateCreated:=vDateCreated, vPaymentMethodCode:=vPaymentMethodCode, vReminderTypeId:=vReminderTypeId, vServiceLevelId:=vServiceLevelId, vCreditCardCode:=vCreditCardCode, vPaymentTermCode:=vPaymentTermCode, vAreaId:=vAreaId, vCCJs:=vCCJs, vInvariantKey:=vInvariantKey, vSeasonalGiftID:=vSeasonalGiftID, vCorrespondenceTypeId:=vCorrespondenceTypeId, vRenewalStopCodeId:=vRenewalStopCodeId, vSwiftPartyID:=vSwiftPartyID, vTobLetter:=vTobLetter, vAlternativeIdentifier:=vAlternativeIdentifier, vCurrencyId:=vCurrencyId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyPC = Nothing
                Return result
            End If

            ' Retrieve Primary Key of new Core record

            vPartyCnt = oSIRPartyPC.bSIRParty.PartyCnt
            'EK 22/9/99
            'DC 28/10/99 added Gender, Occupation Code and Date Of Birth
            ' CTAF 250701 Added Salutation
            oSIRPartyPC.PartyCnt = vPartyCnt
            ' Populate SIRPartyPC Attributes
            m_lReturn = oSIRPartyPC.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vPartyCnt:=vPartyCnt, vPartyTitleCode:=CStr(vPartyTitleCode), vForename:=CStr(vForename), vInitials:=vInitials, vEmploymentStatusCode:=vEmploymentStatusCode, vEmployerCnt:=vEmployerCnt, vEmployerBusiness:=CStr(vEmployerBusiness), vSecondaryEmploymentStatusC:=CStr(vSecondaryEmploymentStatusC), vSecondaryEmployerBusiness:=CStr(vSecondaryEmployerBusiness), vMaritalStatusCode:=vMaritalStatusCode, vNumberOfChildren:=vNumberOfChildren, vNationalityId:=vNationalityId, vCountryOfOriginCode:=vCountryOfOriginCode, vMailshot:=vMailshot, vGender:=CStr(vGender), vDateOfBirth:=vDateOfBirth, vIsPetOwner:=vIsPetOwner, vOccupationCode:=CStr(vOccupationCode), vAccommodationTypeCode:=vAccommodationTypeCode, vPartyLifestyleName:=CStr(vPartyLifestyleName), vSalutation:=CStr(vSalutation), vSource:=vSource, vTPSind:=vTPSind, vEMPSind:=vEMPSind, vTPPassword:=vTPPassword, vIsFeeClient:=vIsFeeClient)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyPC = Nothing
                Return result
            End If

            ' Add the SIRPartyPC to the Database
            m_lReturn = oSIRPartyPC.AddItem()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyPC = Nothing
                Return result
            End If

            ' Retain the Primary Key of the SIRPartyPC Added
            With oSIRPartyPC
                PartyCnt = .PartyCnt
            End With

            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}


            v_vFieldArray(AC_PARTYPC_PARTYCNT) = vPartyCnt

            oSIRPartyPC = Nothing

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
    ' Description: Deletes a single SIRPartyPC directly from the database.
    '        Note: The SIRPartyPC will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByRef vPartyCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyPC As bSIRPartyPC.SIRPartyPC

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new SIRPartyPC
            oSIRPartyPC = New bSIRPartyPC.SIRPartyPC()
            m_lReturn = oSIRPartyPC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            ' Set SIRPartyPC Primary Key

            m_lReturn = oSIRPartyPC.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMDelete, vPartyCnt:=CInt(vPartyCnt))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyPC = Nothing
                Return result
            End If

            ' Delete the SIRPartyPC from the Database
            m_lReturn = oSIRPartyPC.DeleteItem()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyPC = Nothing
                Return result
            End If


            m_lReturn = oSIRPartyPC.bSIRParty.DirectDelete(vPartyCnt:=vPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyPC = Nothing
                Return result
            End If

            oSIRPartyPC = Nothing

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
    ' Description: Gets the required SIRPartyPCs and populate the Collection
    '
    ' ***************************************************************** '
    'developer guide no. 101 (Guide)
    Public Function GetDetails(Optional ByRef vLockMode As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        'developer guide no 21. 
        Dim oFields As DataRow
        Dim oSIRPartyPC As bSIRPartyPC.SIRPartyPC

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oSIRPartyPCs.Clear()

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

                ' Create New SIRPartyPC
                oSIRPartyPC = New bSIRPartyPC.SIRPartyPC()
                m_lReturn = oSIRPartyPC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

                ' Set component primary keys
                With oSIRPartyPC
                    .PartyCnt = vPartyCnt

                    'And if we're coming from events
                    .FromEvent = FromEvent

                    m_lReturn = .SelectItem()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return m_lReturn
                    End If
                End With

                'Set the database status to current task
                oSIRPartyPC.DatabaseStatus = Task

                ' Add SIRPartyPC to collection
                If m_oSIRPartyPCs.Count = 0 Then
                    m_oSIRPartyPCs.Add(Nothing)
                End If
                m_lReturn = m_oSIRPartyPCs.Add(oNewSIRPartyPC:=oSIRPartyPC)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Call the core Business method

                m_lReturn = oSIRPartyPC.bSIRParty.GetDetails(vLockMode:=vLockMode, vPartyCnt:=vPartyCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                oSIRPartyPC = Nothing

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
                    oSIRPartyPC = New bSIRPartyPC.SIRPartyPC()
                    m_lReturn = oSIRPartyPC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

                    ' Set oFields to refer to one Record
                    'developer guide no. 111
                    oFields = m_oDatabase.Records.Item(lSub - 1).Fields()

                    ' Set component primary keys from current record
                    With oSIRPartyPC
                        .PartyCnt = gPMFunctions.NullToLong(oFields("party_cnt"))

                        m_lReturn = .SelectItem()

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If


                        m_lReturn = .bSIRParty.GetDetails(vLockMode:=vLockMode, vPartyCnt:=ToSafeInteger(.PartyCnt))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    End With

                    ' Add SIRPartyPC to collection
                    If m_oSIRPartyPCs.Count = 0 Then
                        m_oSIRPartyPCs.Add(Nothing)
                    End If
                    m_lReturn = m_oSIRPartyPCs.Add(oNewSIRPartyPC:=oSIRPartyPC)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oSIRPartyPC = Nothing
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

    'eck150500 Added source ID as parameter
    'DC 28/06/00 Added correspondence type id
    ' CTAF 250701 Added salutation
    'sj 12/06/2002 - Added LoyaltyNumber, AlternativeIdentifier,
    '                MarketingSegmentInd, TradingName and SubBranchId
    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required SIRPartyPCs and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyID As Object = Nothing, Optional ByRef vPartyTitleCode As Object = Nothing, Optional ByRef vForename As Object = Nothing, Optional ByRef vInitials As Object = Nothing, Optional ByRef vEmploymentStatusCode As Object = Nothing, Optional ByRef vEmployerCnt As Object = Nothing, Optional ByRef vEmployerBusiness As Object = Nothing, Optional ByRef vSecondaryEmploymentStatusC As Object = Nothing, Optional ByRef vSecondaryEmployerBusiness As Object = Nothing, Optional ByRef vMaritalStatusCode As Object = Nothing, Optional ByRef vNumberOfChildren As Object = Nothing, Optional ByRef vNationalityId As Object = Nothing, Optional ByRef vCountryOfOriginCode As Object = Nothing, Optional ByRef vMailshot As Object = Nothing, Optional ByRef vIsPetOwner As Object = Nothing, Optional ByRef vGender As Object = Nothing, Optional ByRef vDateOfBirth As Object = Nothing, Optional ByRef vOccupationCode As Object = Nothing, Optional ByRef vAccommodationTypeCode As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vShortname As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vIsAlsoAgent As Object = Nothing, Optional ByRef vIsProspect As Object = Nothing, Optional ByRef vAgentCnt As Object = Nothing, Optional ByRef vConsultantCnt As Object = Nothing, Optional ByRef vFileCode As Object = Nothing, Optional ByRef vCurrencyId As Object = Nothing, Optional ByRef vPaymentMethodCode As Object = Nothing, Optional ByRef vReminderTypeId As Object = Nothing, Optional ByRef vAreaId As Object = Nothing, Optional ByRef vServiceLevelId As Object = Nothing, Optional ByRef vCreditCardCode As Object = Nothing, Optional ByRef vPaymentTermCode As Object = Nothing, Optional ByRef vCCJs As Object = Nothing, Optional ByRef vSeasonalGiftID As Object = Nothing, Optional ByRef vCorrespondenceTypeId As Object = Nothing, Optional ByRef vRenewalStopCodeId As Object = Nothing, Optional ByRef vResolvedName As Object = Nothing, Optional ByRef vSwiftPartyID As Object = Nothing, Optional ByRef vSalutation As Object = Nothing, Optional ByRef vLoyaltyNumber As Object = Nothing, Optional ByRef vAlternativeIdentifier As Object = Nothing, Optional ByRef vMarketingSegementInd As Object = Nothing, Optional ByRef vTradingName As Object = Nothing, Optional ByRef vSubBranchId As Object = Nothing, Optional ByRef vTobLetter As Object = Nothing, Optional ByRef vSource As Object = Nothing, Optional ByRef vTPSind As Object = Nothing, Optional ByRef vEMPSind As Object = Nothing, Optional ByRef vTPPassword As Object = Nothing, Optional ByRef vIsFeeClient As Object = Nothing) As Integer
        'MSS200901 - Added ResolvedName for UW

        Dim result As Integer = 0

        Dim oSIRPartyPC As bSIRPartyPC.SIRPartyPC
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oSIRPartyPCs.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord = CType(m_lCurrentRecord + 1, gPMConstants.PMEReturnCode)
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oSIRPartyPC = m_oSIRPartyPCs.Item(m_lCurrentRecord)
            'DC 28/10/99 Added Gender, Occupation and Date Of Birth
            ' Get the SIRPartyPC Property Values

            'developer guide no. 98 (Guide)
            m_lReturn = oSIRPartyPC.GetProperties(iStatus, vPartyCnt:=vPartyCnt, vPartyTitleCode:=vPartyTitleCode, vForename:=vForename, vInitials:=vInitials, vEmploymentStatusCode:=vEmploymentStatusCode, vEmployerCnt:=vEmployerCnt, vEmployerBusiness:=vEmployerBusiness, vSecondaryEmploymentStatusC:=vSecondaryEmploymentStatusC, vSecondaryEmployerBusiness:=vSecondaryEmployerBusiness, vMaritalStatusCode:=vMaritalStatusCode, vNumberOfChildren:=vNumberOfChildren, vNationalityId:=vNationalityId, vCountryOfOriginCode:=vCountryOfOriginCode, vMailshot:=vMailshot, vGender:=vGender, vDateOfBirth:=vDateOfBirth, vIsPetOwner:=vIsPetOwner, vOccupationCode:=vOccupationCode, vAccommodationTypeCode:=vAccommodationTypeCode, vSalutation:=vSalutation, vSource:=vSource, vTPSind:=vTPSind, vEMPSind:=vEMPSind, vTPPassword:=vTPPassword, vIsFeeClient:=vIsFeeClient)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get Core details

            m_lReturn = oSIRPartyPC.bSIRParty.GetDetails(vPartyCnt:=vPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'SP231198
            'ECK changes for Orion
            'DC 28/06/00 Added Correspondence Type Id
            'sj 12/06/2002 - Added LoyaltyNumber, AlternativeIdentifier,
            '                MarketingSegmentInd, TradingName and SubBranchId

            m_lReturn = oSIRPartyPC.bSIRParty.GetNext(vSourceID:=vSourceID, vPartyID:=vPartyID, vShortname:=vShortname, vName:=vName, vIsAlsoAgent:=vIsAlsoAgent, vIsProspect:=vIsProspect, vAgentCnt:=vAgentCnt, vConsultantCnt:=vConsultantCnt, vFileCode:=vFileCode, vCurrencyId:=vCurrencyId, vPaymentMethodCode:=vPaymentMethodCode, vReminderTypeId:=vReminderTypeId, vAreaId:=vAreaId, vServiceLevelId:=vServiceLevelId, vCreditCardCode:=vCreditCardCode, vPaymentTermCode:=vPaymentTermCode, vCCJs:=vCCJs, vSeasonalGiftID:=vSeasonalGiftID, vCorrespondenceTypeId:=vCorrespondenceTypeId, vRenewalStopCodeId:=vRenewalStopCodeId, vResolvedName:=vResolvedName, vSwiftPartyID:=vSwiftPartyID, vLoyaltyNumber:=vLoyaltyNumber, vAlternativeIdentifier:=vAlternativeIdentifier, vMarketingSegementInd:=vMarketingSegementInd, vTradingName:=vTradingName, vSubBranchId:=vSubBranchId, vTobLetter:=vTobLetter)
            'MSS200901 - Added ResolvedName for UW
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyPC = Nothing
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
    ' Description: Adds the supplied SIRPartyPC into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    'DC 28/06/00 Added Correspondence Type Id
    ' CTAF 250701 Added salutation
    'sj 12/06/2002 - Added LoyaltyNumber, AlternativeIdentifier,
    '                MarketingSegmentInd, TradingName and SubBranchId
    Public Function EditAdd(ByRef lRow As Object, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyTitleCode As Object = Nothing, Optional ByRef vForename As Object = Nothing, Optional ByRef vInitials As Object = Nothing, Optional ByRef vEmploymentStatusCode As Object = Nothing, Optional ByRef vEmployerCnt As Object = Nothing, Optional ByRef vEmployerBusiness As Object = Nothing, Optional ByRef vSecondaryEmploymentStatusC As Object = Nothing, Optional ByRef vSecondaryEmployerBusiness As Object = Nothing, Optional ByRef vMaritalStatusCode As Object = Nothing, Optional ByRef vNumberOfChildren As Object = Nothing, Optional ByRef vNationalityId As Object = Nothing, Optional ByRef vCountryOfOriginCode As Object = Nothing, Optional ByRef vMailshot As Object = Nothing, Optional ByRef vIsPetOwner As Object = Nothing, Optional ByRef vAccommodationTypeCode As Object = Nothing, Optional ByRef vShortname As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vResolved As Object = Nothing, Optional ByRef vIsAlsoAgent As Object = Nothing, Optional ByRef vIsProspect As Object = Nothing, Optional ByRef vAgentCnt As Object = Nothing, Optional ByRef vConsultantCnt As Object = Nothing, Optional ByRef vFileCode As Object = Nothing, Optional ByRef vCurrencyId As Object = Nothing, Optional ByRef vPaymentMethodCode As Object = Nothing, Optional ByRef vReminderTypeId As Object = Nothing, Optional ByRef vAreaId As Object = Nothing, Optional ByRef vServiceLevelId As Object = Nothing, Optional ByRef vCreditCardCode As Object = Nothing, Optional ByRef vPaymentTermCode As Object = Nothing, Optional ByRef vCCJs As Object = Nothing, Optional ByRef vPartyLifestyleId As Object = Nothing, Optional ByRef vPartyLifestyleName As Object = Nothing, Optional ByRef vCategory As Object = Nothing, Optional ByRef vDateOfBirth As Object = Nothing, Optional ByRef vGender As Object = Nothing, Optional ByRef vOccupationCode As Object = Nothing, Optional ByRef vSecondaryOccupationCode As Object = Nothing, Optional ByRef vIsSmoker As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vSeasonalGiftID As Object = Nothing, Optional ByRef vCorrespondenceTypeId As Object = Nothing, Optional ByRef vRenewalStopCodeId As Object = Nothing, Optional ByRef vSwiftPartyID As Object = Nothing, Optional ByRef vSalutation As Object = Nothing, Optional ByRef vLoyaltyNumber As Object = Nothing, Optional ByRef vAlternativeIdentifier As Object = Nothing, Optional ByRef vMarketingSegementInd As Object = Nothing, Optional ByRef vTradingName As Object = Nothing, Optional ByRef vSubBranchId As Object = Nothing, Optional ByRef vTobLetter As Object = Nothing, Optional ByRef vSource As Object = Nothing, Optional ByRef vTPSind As Object = Nothing, Optional ByRef vEMPSind As Object = Nothing, Optional ByRef vTPPassword As Object = Nothing, Optional ByRef vIsFeeClient As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyPC As bSIRPartyPC.SIRPartyPC
        Dim lPartyTypeId As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get party type id for a private individual
            m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupPartyType, v_sCode:=gSIRLibrary.SIRPartyTypePersonalClient, v_dtEffectiveDate:=DateTime.Now, r_lID:=lPartyTypeId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed on m_oLookup.GetEffectiveIDFromCode( _" & Environment.NewLine &
                                   "v_sTableName:=" & gSIRLibrary.SIRLookupPartyType & ", " & Environment.NewLine &
                                   "v_sCode:=" & gSIRLibrary.SIRPartyTypePersonalClient & ", " & Environment.NewLine &
                                   "v_dtEffectiveDate:=" & DateTime.Now.ToString & ", " & Environment.NewLine &
                                   "r_lID:=" & CStr(lPartyTypeId) & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oSIRPartyPCs.Count() <> (lRow - 1) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed on If (m_oSIRPartyPCs.Count <> (lRow& - 1))", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Create a new SIRPartyPC
            oSIRPartyPC = New bSIRPartyPC.SIRPartyPC()
            m_lReturn = oSIRPartyPC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            ' Populate SIRPartyPC Attributes

            'developer guide no. 98
            m_lReturn = oSIRPartyPC.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vPartyCnt:=vPartyCnt, vPartyTitleCode:=vPartyTitleCode, vForename:=vForename, vInitials:=vInitials, vEmploymentStatusCode:=vEmploymentStatusCode, vEmployerCnt:=(vEmployerCnt), vEmployerBusiness:=CStr(vEmployerBusiness), vSecondaryEmploymentStatusC:=CStr(vSecondaryEmploymentStatusC), vSecondaryEmployerBusiness:=CStr(vSecondaryEmployerBusiness), vMaritalStatusCode:=vMaritalStatusCode, vNumberOfChildren:=vNumberOfChildren, vNationalityId:=vNationalityId, vCountryOfOriginCode:=vCountryOfOriginCode, vMailshot:=vMailshot, vIsPetOwner:=vIsPetOwner, vAccommodationTypeCode:=vAccommodationTypeCode, vPartyLifestyleId:=CInt(vPartyLifestyleId), vPartyLifestyleName:=CStr(vPartyLifestyleName), vCategory:=CInt(vCategory), vDateOfBirth:=CDate(vDateOfBirth), vGender:=CStr(vGender), vOccupationCode:=CStr(vOccupationCode), vSecondaryOccupationCode:=CStr(vSecondaryOccupationCode), vIsSmoker:=CInt(vIsSmoker), vSalutation:=CStr(vSalutation), vSource:=vSource, vTPSind:=vTPSind, vEMPSind:=vEMPSind, vTPPassword:=vTPPassword, vIsFeeClient:=vIsFeeClient)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed on oSIRPartyPC.SetProperties", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                oSIRPartyPC = Nothing
                Return result
            End If

            ' Add SIRPartyPC to collection
            If m_oSIRPartyPCs.Count = 0 Then
                m_oSIRPartyPCs.Add(Nothing)
            End If
            m_lReturn = m_oSIRPartyPCs.Add(oNewSIRPartyPC:=oSIRPartyPC)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed on m_oSIRPartyPCs.Add", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                oSIRPartyPC = Nothing
                Return result
            End If
            'FSA Phase III

            m_lReturn = oSIRPartyPC.bSIRParty.EditAdd(lRow:=lRow, vPartyTypeID:=ToSafeInteger(lPartyTypeId), vSourceID:=vSourceID, vShortname:=vShortname, vName:=vName, vResolvedName:=vResolved, vIsAlsoAgent:=vIsAlsoAgent, vIsProspect:=vIsProspect, vAgentCnt:=vAgentCnt, vConsultantCnt:=vConsultantCnt, vFileCode:=vFileCode, vCurrencyId:=vCurrencyId, vPaymentMethodCode:=vPaymentMethodCode, vReminderTypeId:=vReminderTypeId, vAreaId:=vAreaId, vServiceLevelId:=vServiceLevelId, vCreditCardCode:=vCreditCardCode, vPFFrequencyID:=vPaymentTermCode, vCCJs:=vCCJs, vSeasonalGiftID:=vSeasonalGiftID, vCorrespondenceTypeId:=vCorrespondenceTypeId, vRenewalStopCodeId:=vRenewalStopCodeId, vSwiftPartyID:=vSwiftPartyID, vLoyaltyNumber:=vLoyaltyNumber, vAlternativeIdentifier:=vAlternativeIdentifier, vMarketingSegementInd:=vMarketingSegementInd, vTradingName:=vTradingName, vSubBranchId:=vSubBranchId, vTobLetter:=vTobLetter)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed on oSIRPartyPC.bSIRParty.EditAdd", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                oSIRPartyPC = Nothing
                Return result
            End If

            oSIRPartyPC = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the SIRPartyPC
    '              specified and updates the SIRPartyPC with the new values.
    '
    ' ***************************************************************** '
    'FSA Phase III TobLetter
    Public Function EditUpdate(ByRef lRow As Object, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyTitleCode As Object = Nothing, Optional ByRef vForename As Object = Nothing, Optional ByRef vInitials As Object = Nothing, Optional ByRef vEmploymentStatusCode As Object = Nothing, Optional ByRef vEmployerCnt As Object = Nothing, Optional ByRef vEmployerBusiness As Object = Nothing, Optional ByRef vSecondaryEmploymentStatusC As Object = Nothing, Optional ByRef vSecondaryEmployerBusiness As Object = Nothing, Optional ByRef vMaritalStatusCode As Object = Nothing, Optional ByRef vNumberOfChildren As Object = Nothing, Optional ByRef vNationalityId As Object = Nothing, Optional ByRef vCountryOfOriginCode As Object = Nothing, Optional ByRef vMailshot As Object = Nothing, Optional ByRef vIsPetOwner As Object = Nothing, Optional ByRef vAccommodationTypeCode As Object = Nothing, Optional ByRef vShortname As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vResolved As Object = Nothing, Optional ByRef vIsAlsoAgent As Object = Nothing, Optional ByRef vIsProspect As Object = Nothing, Optional ByRef vAgentCnt As Object = Nothing, Optional ByRef vConsultantCnt As Object = Nothing, Optional ByRef vFileCode As Object = Nothing, Optional ByRef vCurrencyId As Object = Nothing, Optional ByRef vPaymentMethodCode As Object = Nothing, Optional ByRef vReminderTypeId As Object = Nothing, Optional ByRef vAreaId As Object = Nothing, Optional ByRef vServiceLevelId As Object = Nothing, Optional ByRef vCreditCardCode As Object = Nothing, Optional ByRef vPaymentTermCode As Object = Nothing, Optional ByRef vCCJs As Object = Nothing, Optional ByRef vPartyLifestyleId As Object = Nothing, Optional ByRef vPartyLifestyleName As Object = Nothing, Optional ByRef vCategory As Object = Nothing, Optional ByRef vDateOfBirth As Object = Nothing, Optional ByRef vGender As Object = Nothing, Optional ByRef vOccupationCode As Object = Nothing, Optional ByRef vSecondaryOccupationCode As Object = Nothing, Optional ByRef vIsSmoker As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vPartyID As Object = Nothing, Optional ByRef vSeasonalGiftID As Object = Nothing, Optional ByRef vCorrespondenceTypeId As Object = Nothing, Optional ByRef vRenewalStopCodeId As Object = Nothing, Optional ByRef vSwiftPartyID As Object = Nothing, Optional ByRef vSalutation As Object = Nothing, Optional ByRef vLoyaltyNumber As Object = Nothing, Optional ByRef vAlternativeIdentifier As Object = Nothing, Optional ByRef vMarketingSegementInd As Object = Nothing, Optional ByRef vTradingName As Object = Nothing, Optional ByRef vSubBranchId As Object = Nothing, Optional ByRef vTobLetter As Object = Nothing, Optional ByRef vSource As Object = Nothing, Optional ByRef vTPSind As Object = Nothing, Optional ByRef vEMPSind As Object = Nothing, Optional ByRef vTPPassword As Object = Nothing, Optional ByRef vIsFeeClient As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyPC As bSIRPartyPC.SIRPartyPC
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oSIRPartyPCs.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oSIRPartyPC = m_oSIRPartyPCs.Item(lRow)

            ' Check the Status of the SIRPartyPC

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oSIRPartyPC.DatabaseStatus
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

            ' Update SIRPartyPC Attributes

            'developer guide no. 101 (Guide)
            m_lReturn = oSIRPartyPC.SetProperties(iStatus:=iStatus, vPartyCnt:=vPartyCnt, vPartyTitleCode:=vPartyTitleCode, vForename:=vForename, vInitials:=vInitials, vEmploymentStatusCode:=vEmploymentStatusCode, vEmployerCnt:=vEmployerCnt, vEmployerBusiness:=vEmployerBusiness, vSecondaryEmploymentStatusC:=vSecondaryEmploymentStatusC, vSecondaryEmployerBusiness:=vSecondaryEmployerBusiness, vMaritalStatusCode:=vMaritalStatusCode, vNumberOfChildren:=vNumberOfChildren, vNationalityId:=vNationalityId, vCountryOfOriginCode:=vCountryOfOriginCode, vMailshot:=vMailshot, vIsPetOwner:=vIsPetOwner, vAccommodationTypeCode:=vAccommodationTypeCode, vPartyLifestyleId:=vPartyLifestyleId, vPartyLifestyleName:=vPartyLifestyleName, vCategory:=vCategory, vDateOfBirth:=vDateOfBirth, vGender:=vGender, vOccupationCode:=vOccupationCode, vSecondaryOccupationCode:=vSecondaryOccupationCode, vIsSmoker:=vIsSmoker, vSalutation:=vSalutation, vSource:=vSource, vTPSind:=vTPSind, vEMPSind:=vEMPSind, vTPPassword:=vTPPassword, vIsFeeClient:=vIsFeeClient)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oSIRPartyPC = Nothing
                Return result
            End If

            'FSA Phase III TobLetter

            m_lReturn = oSIRPartyPC.bSIRParty.EditUpdate(lRow:=lRow, vSourceID:=vSourceID, vPartyID:=vPartyID, vShortname:=vShortname, vName:=vName, vResolvedName:=vResolved, vIsAlsoAgent:=vIsAlsoAgent, vIsProspect:=vIsProspect, vAgentCnt:=vAgentCnt, vConsultantCnt:=vConsultantCnt, vFileCode:=vFileCode, vCurrencyId:=vCurrencyId, vPaymentMethodCode:=vPaymentMethodCode, vReminderTypeId:=vReminderTypeId, vAreaId:=vAreaId, vServiceLevelId:=vServiceLevelId, vCreditCardCode:=vCreditCardCode, vPaymentTermCode:=vPaymentTermCode, vCCJs:=vCCJs, vSeasonalGiftID:=vSeasonalGiftID, vCorrespondenceTypeId:=vCorrespondenceTypeId, vRenewalStopCodeId:=vRenewalStopCodeId, vSwiftPartyID:=vSwiftPartyID, vLoyaltyNumber:=vLoyaltyNumber, vAlternativeIdentifier:=vAlternativeIdentifier, vMarketingSegementInd:=vMarketingSegementInd, vTradingName:=vTradingName, vSubBranchId:=vSubBranchId, vTobLetter:=vTobLetter)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyPC = Nothing
                Return result
            End If

            ' Release reference to SIRPartyPC
            oSIRPartyPC = Nothing

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
    ' Description: Validate that the specified SIRPartyPC can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyPC As bSIRPartyPC.SIRPartyPC

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oSIRPartyPCs.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oSIRPartyPC = m_oSIRPartyPCs.Item(lRow)

            ' Check the Status of the SIRPartyPC

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oSIRPartyPC.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oSIRPartyPC.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oSIRPartyPC.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If


            m_lReturn = oSIRPartyPC.bSIRParty.EditDelete(lRow:=ToSafeInteger(lRow))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyPC = Nothing
                Return result
            End If

            ' Release reference to SIRPartyPC
            oSIRPartyPC = Nothing

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
            For lSub As Integer = 1 To m_oSIRPartyPCs.Count()
                Select Case m_oSIRPartyPCs.Item(lSub).DatabaseStatus
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
        Dim oSIRPartyPC As bSIRPartyPC.SIRPartyPC = Nothing
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
            For lSub = 1 To m_oSIRPartyPCs.Count()
                oSIRPartyPC = m_oSIRPartyPCs.Item(lSub)


                Select Case oSIRPartyPC.DatabaseStatus
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

                        m_lReturn = oSIRPartyPC.bSIRParty.Update()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("oSIRPartyPC.bSIRParty.Update", "Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        ' Retrieve Primary Key of Core Item added

                        m_lPartyCnt = oSIRPartyPC.bSIRParty.PartyCnt
                        oSIRPartyPC.PartyCnt = m_lPartyCnt

                        m_lReturn = oSIRPartyPC.AddItem()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("oSIRPartyPC.AddItem", "Failed", gPMConstants.PMELogLevel.PMLogError)
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
                        m_lReturn = oSIRPartyPC.UpdateItem()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("oSIRPartyPC.UpdateItem", "Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If


                        m_lReturn = oSIRPartyPC.bSIRParty.Update()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("oSIRPartyPC.bSIRParty.Update", "Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        If m_sOldClientCode <> m_sNewClientCode Then

                            sDescription = New StringBuilder("Client code has been changed from '" & m_sOldClientCode & "' to '" & m_sNewClientCode & "'.")


                            m_lReturn = CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=oSIRPartyPC.bSIRParty.PartyCnt, v_lEventTypeId:=PMBConst.PMBEventClientNotes, v_dtEventDate:=DateTime.Today, v_vDescription:=sDescription.ToString())
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("CreateEvent", "Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                        End If

                        If m_sOldClientName <> m_sNewClientName Then

                            sDescription = New StringBuilder("Client name has been changed from '" & m_sOldClientName & "' to '" & m_sNewClientName & "'.")


                            m_lReturn = CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=oSIRPartyPC.bSIRParty.PartyCnt, v_lEventTypeId:=PMBConst.PMBEventClientNotes, v_dtEventDate:=DateTime.Today, v_vDescription:=sDescription.ToString())
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


                                        m_lReturn = CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=oSIRPartyPC.bSIRParty.PartyCnt, v_lEventTypeId:=PMBConst.PMBEventClientNotes, v_dtEventDate:=DateTime.Today, v_vDescription:=sDescription.ToString())
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


                                    m_lReturn = CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=oSIRPartyPC.bSIRParty.PartyCnt, v_lEventTypeId:=PMBConst.PMBEventClientNotes, v_dtEventDate:=DateTime.Today, v_vDescription:=sDescription.ToString())
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


                                    m_lReturn = CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=oSIRPartyPC.bSIRParty.PartyCnt, v_lEventTypeId:=PMBConst.PMBEventClientNotes, v_dtEventDate:=DateTime.Today, v_vDescription:=sDescription.ToString())
                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        gPMFunctions.RaiseError("CreateEvent", "Failed", gPMConstants.PMELogLevel.PMLogError)
                                    End If
                                End If

                            Next

                        End If

                        If m_bIsAmended Then
                            sDescription = New StringBuilder("Client Detail Amended")


                            m_lReturn = CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=oSIRPartyPC.bSIRParty.PartyCnt, v_vInsuranceFolderCnt:=DBNull.Value, v_vInsuranceFileCnt:=DBNull.Value, v_vClaimCnt:=DBNull.Value, v_vDocumentCnt:=DBNull.Value, v_vOldAddressCnt:=DBNull.Value, v_vNewAddressCnt:=DBNull.Value, v_vCampaignId:=DBNull.Value, v_vDocumentTypeId:=DBNull.Value, v_vReportTypeId:=DBNull.Value, v_lEventTypeId:=PMBConst.PMBEventNewClient, v_dtEventDate:=DateTime.Today, v_vDescription:=sDescription.ToString())
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("CreateEvent", "Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
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

                        m_lReturn = CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=oSIRPartyPC.bSIRParty.PartyCnt, v_lEventTypeId:=PMBConst.PMBEventDelClient, v_dtEventDate:=DateTime.Today)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("CreateEvent", "Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        'Now keep a copy of the deleted item
                        m_lReturn = oSIRPartyPC.CopyToEvent(v_lEventCnt:=lEventCnt)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("oSIRPartyPC.CopyToEvent", "v_lEventCnt:=" & lEventCnt, gPMConstants.PMELogLevel.PMLogError)
                        End If

                        ' Delete Item
                        m_lReturn = oSIRPartyPC.DeleteItem()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("oSIRPartyPC.DeleteItem", "Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If


                        m_lReturn = oSIRPartyPC.bSIRParty.Update()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("oSIRPartyPC.bSIRParty.Update", "Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                End Select

            Next

            ' Retain the Primary Key of the SIRPartyPC
            With oSIRPartyPC
                PartyCnt = .PartyCnt
            End With

            ' Release last reference
            oSIRPartyPC = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                m_lReturn = CommitTrans()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("CommitTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                'Refresh the Collection Items D/B Status
                lSub = gPMConstants.PMEReturnCode.PMTrue

                ' For each item in the collection
                Do While lSub <= m_oSIRPartyPCs.Count()

                    ' With the item
                    With m_oSIRPartyPCs.Item(lSub)


                        Select Case .DatabaseStatus
                            ' Delete or Dummy Delete
                            Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                ' Remove from Collection
                                m_oSIRPartyPCs.Delete(lSub)

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

        Dim oSIRPartyPC As bSIRPartyPC.SIRPartyPC

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyPC - need to for to core for shortname
            oSIRPartyPC = New bSIRPartyPC.SIRPartyPC()

            m_lReturn = oSIRPartyPC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            m_lReturn = oSIRPartyPC.bSIRParty.GetPartyCnt(vPartyRef:=vPartyRef, vPartyCnt:=vPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyPC = Nothing

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
    ' ***************************************************************** '
    'developer guide no. 263
    Public Function GetOtherDetails(Optional ByRef vAgentCnt As Object = -1, Optional ByRef vAgentRef As Object = -1, Optional ByRef vAgentName As Object = -1, Optional ByRef vEmployerCnt As Object = -1, Optional ByRef vEmployerRef As Object = -1, Optional ByRef vConsultantCnt As Object = -1, Optional ByRef vConsultantRef As Object = -1, Optional ByRef vConsultantname As Object = -1, Optional ByRef vInsurerCnt As Object = -1, Optional ByRef vInsurerRef As Object = -1, Optional ByRef vInsurername As Object = -1, Optional ByRef vBrokerCnt As Object = -1, Optional ByRef vBrokerRef As Object = -1, Optional ByRef vBrokername As Object = -1, Optional ByRef vPartyCnt As Object = -1, Optional ByRef vAssociates As Object = -1) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim oSIRPartyPC As bSIRPartyPC.SIRPartyPC


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyPC - ned to go to core for agent
            oSIRPartyPC = New bSIRPartyPC.SIRPartyPC()
            m_lReturn = oSIRPartyPC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            m_lReturn = oSIRPartyPC.bSIRParty.GetOtherDetails(vAgentCnt:=vAgentCnt, vAgentRef:=vAgentRef, vAgentName:=vAgentName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If




            If (vConsultantCnt <> -1) Then
                m_lReturn = oSIRPartyPC.bSIRParty.GetOtherDetails(vAgentCnt:=vConsultantCnt, vAgentRef:=vConsultantRef, vAgentName:=vConsultantname)
            End If

            If (vInsurerCnt <> -1) Then
                m_lReturn = oSIRPartyPC.bSIRParty.GetOtherDetails(vAgentCnt:=vInsurerCnt, vAgentRef:=vInsurerRef, vAgentName:=vInsurername)
            End If

            If (vBrokerCnt <> -1) Then
                m_lReturn = oSIRPartyPC.bSIRParty.GetOtherDetails(vAgentCnt:=vBrokerCnt, vAgentRef:=vBrokerRef, vAgentName:=vBrokername)
            End If



            'If vAssociates <> -1 Then
            '    'DC 29/03/00
            '    'Cater for more than one Associate
            '    'm_lReturn = oSIRPartyPC.bSIRParty.GetAssociateDetails(vPartyCnt:=vPartyCnt, _
            '    ''                                           vIsAssociate:=PMTrue, _
            '    ''                                           vAssociates:=vAssociates)

            '    m_lReturn = oSIRPartyPC.bSIRParty.GetAssociates(vPartyCnt:=vPartyCnt, vAssociates:=vAssociates)
            'End If

            ' Fix for Munich Re Issue 32
            If Not (vAssociates Is Nothing) Then
                If Not (vAssociates.Equals(-1)) Then
                    'DC 29/03/00
                    'Cater for more than one Associate
                    'm_lReturn = oSIRPartyPC.bSIRParty.GetAssociateDetails(vPartyCnt:=vPartyCnt, _
                    ''                                           vIsAssociate:=PMTrue, _
                    ''                                           vAssociates:=vAssociates)

                    m_lReturn = oSIRPartyPC.bSIRParty.GetAssociates(vPartyCnt:=vPartyCnt, vAssociates:=vAssociates)
                End If
            Else
                m_lReturn = oSIRPartyPC.bSIRParty.GetAssociates(vPartyCnt:=vPartyCnt, vAssociates:=vAssociates)
            End If

            oSIRPartyPC = Nothing





            If vEmployerCnt <> -1 Then

                'Get form DB

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetEmployerDetailsSQL & CStr(vEmployerCnt), sSQLName:=ACGetEmployerDetailsName, bStoredProcedure:=ACGetEmployerDetailsStored, lNumberRecords:=1, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'return the data
                If Not Informations.IsArray(vResultArray) Then
                    vEmployerRef = ""
                Else

                    vEmployerRef = CStr(vResultArray(0, 0))
                End If

            End If

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
        Dim oSIRPartyPC As bSIRPartyPC.SIRPartyPC


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyPC - need to hit core for address stuff
            oSIRPartyPC = New bSIRPartyPC.SIRPartyPC()

            m_lReturn = oSIRPartyPC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            m_lReturn = oSIRPartyPC.bSIRParty.GetAddressDetails(vPartyCnt:=ToSafeInteger(m_lPartyCnt), vAddresses:=vAddresses)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyPC = Nothing


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
        Dim oSIRPartyPC As bSIRPartyPC.SIRPartyPC


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyPC - need to hit core for address stuff
            oSIRPartyPC = New bSIRPartyPC.SIRPartyPC()

            m_lReturn = oSIRPartyPC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            m_lReturn = oSIRPartyPC.bSIRParty.GetAddressTypeLookups(vAddressTypes:=vAddressTypes)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyPC = Nothing


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
        Dim oSIRPartyPC As bSIRPartyPC.SIRPartyPC

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyPC - need to hit core for address stuff
            oSIRPartyPC = New bSIRPartyPC.SIRPartyPC()

            m_lReturn = oSIRPartyPC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            m_lReturn = oSIRPartyPC.bSIRParty.GetRelationshipTypeLookups(vRelationships:=vRelationships)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyPC = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetRelationshipTypeLookups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRelationshipTypeLookups", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim oSIRPartyPC As bSIRPartyPC.SIRPartyPC


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyPC - need to hit core for address stuff
            oSIRPartyPC = New bSIRPartyPC.SIRPartyPC()

            m_lReturn = oSIRPartyPC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            m_lReturn = oSIRPartyPC.bSIRParty.GetContactDetails(vPartyCnt:=ToSafeInteger(m_lPartyCnt), vContacts:=vContacts)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyPC = Nothing


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetContactDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetContactDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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


                m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(vPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Get all the convictions for the party
                m_lReturn = .SQLSelect(sSQL:=ACGetAllConvictionSQL, sSQLName:=ACGetAllConvictionName, bStoredProcedure:=ACGetAllConvictionStored, lNumberRecords:=0, vResultArray:=vConviction)

            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' CJB 130903 PN13903 Why log that there are no convictions!!!???
            'log if no relationships, but dont fail
            '    If (IsArray(vConviction) = False) Then
            '        LogMessage m_sUsername, _
            ''            iType:=PMLogError, _
            ''            sMsg:="There are no Conviction details for this party, party_cnt=" & CLng(vPartyCnt), _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="GetConvictionDetails", _
            ''            vErrNo:=Err.Number, _
            ''            vErrDesc:=Err.Description
            '        Exit Function
            '    End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetConvictionDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetConvictionDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetRelationshipDetails
    '
    ' Description: Get Relationship details for party.
    '
    ' ***************************************************************** '
    Public Function GetLifestyleDetails(ByRef vPartyCnt As Object, ByRef vLifestyle(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(PartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Get all the Lifestyles for the party
                m_lReturn = .SQLSelect(sSQL:=ACGetAllLifeStyleSQL, sSQLName:=ACGetAllLifestyleName, bStoredProcedure:=ACGetAllLifestyleStored, lNumberRecords:=0, vResultArray:=vLifestyle)


            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'log if no relationships, but dont fail
            If Not Informations.IsArray(vLifestyle) Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="There are no Lifestyle details for this party, party_cnt=" & CInt(vPartyCnt), vApp:=ACApp, vClass:=ACClass, vMethod:="GetLifestyleDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetLifestyleDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLifestyleDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLoyaltySchemeDetails
    '
    ' Description: Get LoyaltyScheme details for party.
    '
    ' ***************************************************************** '
    Public Function GetLoyaltySchemeDetails(ByRef vPartyCnt As Object, ByRef vLoyaltySchemes(,) As Object) As Integer 'RAW 18/11/2002 : PS005 : Added

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' This DB procedure accepts 3 OPTIONAL search parameters
                ' We only need to set PartyCnt

                .Parameters.Clear()


                'developer guide no. 85(Guide)
                m_lReturn = .Parameters.Add(sName:="party_loyalty_scheme_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(vPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                'developer guide no. 85(Guide)
                m_lReturn = .Parameters.Add(sName:="loyalty_scheme_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Language_ID", vValue:=CStr(m_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Language_ID", vValue:=CStr(m_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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
        Dim oSIRPartyPC As bSIRPartyPC.SIRPartyPC


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyPC - need to hit core for address updates
            oSIRPartyPC = New bSIRPartyPC.SIRPartyPC()

            m_lReturn = oSIRPartyPC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            'DC 29/03/00
            'Cater for more than one Associate
            'm_lReturn& = oSIRPartyPC.bSIRParty.UpdateAssociates( _
            ''                            vPartyCnt:=m_lPartyCnt&, _
            ''                            vIsAssociate:=vIsAssociate, _
            ''                            vAssociatedCnt:=vAssociatedCnt, _
            ''                            vAssociateDescription:=vAssociateDescription)


            m_lReturn = oSIRPartyPC.bSIRParty.UpdateAssociates(vPartyCnt:=ToSafeInteger(m_lPartyCnt), vAssociates:=vAssociates)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyPC = Nothing

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
    ' Name: UpdateAddresses
    '
    ' Description: Update the party_address usage table with old
    ' and new addresses for the party.
    '
    ' ***************************************************************** '
    Public Function UpdateAddresses(ByRef vPartyCnt As Object, Optional ByRef vAddAddresses As Object = Nothing, Optional ByRef vDeleteAddresses As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyPC As bSIRPartyPC.SIRPartyPC


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyPC - need to hit core for address updates
            oSIRPartyPC = New bSIRPartyPC.SIRPartyPC()

            m_lReturn = oSIRPartyPC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            m_lReturn = oSIRPartyPC.bSIRParty.UpdateAddresses(vPartyCnt:=ToSafeInteger(m_lPartyCnt), vAddAddresses:=vAddAddresses, vDeleteAddresses:=vDeleteAddresses)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyPC = Nothing

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
    ' Name: UpdateInsuredLifestyle
    '
    ' Description: Update the party_contact usage table with old
    ' and new InsuredLifestyle for the party.
    '
    ' ***************************************************************** '
    'developer guide no. 101
    Public Function UpdateInsuredLifestyle(ByRef vPartyCnt As Object, ByRef vPartyLifestyleId As Object, Optional ByRef vName As Object = Nothing, Optional ByRef vCategory As Object = Nothing, Optional ByRef vDOB As Object = Nothing, Optional ByRef vGender As Object = Nothing, Optional ByRef vOccupationCode As Object = Nothing, Optional ByRef vSecondaryOccupationCode As Object = Nothing, Optional ByRef vSmoker As Object = Nothing) As Integer

        Dim result As Integer = 0

        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = BeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Delete old InsuredLifestyle for party if supplied
            If vPartyLifestyleId = 0 Then


                m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="AddPartyLifestyle", bStoredProcedure:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollbackTrans()

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else

                m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="EditPartyLifeStyle", bStoredProcedure:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollbackTrans()

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()

                Return gPMConstants.PMEReturnCode.PMFalse
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateInsuredLifestyle Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateInsuredLifestyle", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: UpdateGemini
    '
    ' Description: Update the party_address usage table with old
    ' and new addresses for the party.
    '
    ' ***************************************************************** '
    Public Function UpdateGemini(ByRef vPartyCnt As Object, ByRef vTask As Object) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyPC As bSIRPartyPC.SIRPartyPC


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyPC - need to hit core to update gemini
            oSIRPartyPC = New bSIRPartyPC.SIRPartyPC()

            m_lReturn = oSIRPartyPC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oSIRPartyPC.bSIRParty.UpdateGemini(vPartyCnt:=vPartyCnt, vTask:=vTask)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyPC = Nothing

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
        Dim oSIRPartyPC As bSIRPartyPC.SIRPartyPC


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyPC - need to hit core for address updates
            oSIRPartyPC = New bSIRPartyPC.SIRPartyPC()

            m_lReturn = oSIRPartyPC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            m_lReturn = oSIRPartyPC.bSIRParty.UpdateContacts(vPartyCnt:=ToSafeInteger(m_lPartyCnt), vAddContacts:=vAddContacts, vDeleteContacts:=vDeleteContacts)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyPC = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = RollbackTrans()


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateContacts Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateContacts", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Private)
    '
    ' Description: Check Mandatory parameters have been passed.
    '
    ' ***************************************************************** '
    'DC 28/06/00 Added Correspondence Type Id
    'UPGRADE_NOTE: (7001) The following declaration (CheckMandatory) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CheckMandatory(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyTitleCode As Object = Nothing, Optional ByRef vForename As Object = Nothing, Optional ByRef vInitials As Object = Nothing, Optional ByRef vEmploymentStatusCode As Object = Nothing, Optional ByRef vEmployerCnt As Object = Nothing, Optional ByRef vEmployerBusiness As Object = Nothing, Optional ByRef vSecondaryEmploymentStatusC As Object = Nothing, Optional ByRef vSecondaryEmployerBusiness As Object = Nothing, Optional ByRef vMaritalStatusCode As Object = Nothing, Optional ByRef vNumberOfChildren As Object = Nothing, Optional ByRef vNationalityId As Object = Nothing, Optional ByRef vCountryOfOriginCode As Object = Nothing, Optional ByRef vMailshot As Object = Nothing, Optional ByRef vIsPetOwner As Object = Nothing, Optional ByRef vAccommodationTypeCode As Object = Nothing, Optional ByRef vSeasonalGiftID As Object = Nothing, Optional ByRef vSICCodeId As Object = Nothing, Optional ByRef vPreviousInsurerCnt As Object = Nothing, Optional ByRef vPreviousBrokerCnt As Object = Nothing, Optional ByRef vCorrespondenceTypeId As Object = Nothing, Optional ByRef vRenewalStopCodeId As Object = Nothing, Optional ByRef vSalutation As Integer = 0) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' {* USER DEFINED CODE (Begin) *}
    '


    'If (informations.IsNothing(vPartyTitleCode)) Or (Object.Equals(vPartyTitleCode, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '


    'If (informations.IsNothing(vForename)) Or (Object.Equals(vForename, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '


    'If (informations.IsNothing(vInitials)) Or (Object.Equals(vInitials, Nothing)) Then
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
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckMandatory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=informations.Err().Number, vErrDesc:=excep.Message)
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










    'developer guide no. 101(Guide)
    Private Function CreateEvent(ByRef r_lEventCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_dtEventDate As Date, ByVal v_lEventTypeId As Integer, Optional ByVal v_vInsuranceFolderCnt As Object = Nothing, Optional ByVal v_vInsuranceFileCnt As Object = Nothing, Optional ByVal v_vClaimCnt As Object = Nothing, Optional ByVal v_vDocumentCnt As Object = Nothing, Optional ByVal v_vOldAddressCnt As Object = Nothing, Optional ByVal v_vNewAddressCnt As Object = Nothing, Optional ByVal v_vCampaignId As Object = Nothing, Optional ByVal v_vDocumentTypeId As Object = Nothing, Optional ByVal v_vReportTypeId As Object = Nothing, Optional ByVal v_vDescription As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        If m_oEvent Is Nothing Then
            m_oEvent = New bSIREvent.Business()
            m_lReturn = m_oEvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(informations.Err().Number), vErrDesc:=excep.Message)
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
            'developer guide no. 131(Guide)
            If Informations.IsArray(vResultArray) And vResultArray.Length > 0 Then
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

    'MSS200901 - Added from UW for merge

    ' ***************************************************************** '
    '
    ' Name: GetUnderwritingOrBroking
    '
    ' Description:
    '
    ' History: 19/02/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function GetUnderwritingOrBroking() As Integer

        Dim result As Integer = 0




        result = gPMConstants.PMEReturnCode.PMTrue

        'sj 19/06/2002 - start
        m_lReturn = bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, r_vUnderwriting:=m_sUnderwritingOrBroking)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getUnderwritingOrAgency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUnderwritingOrBroking")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'sj 19/06/2002 - end

        Return result

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
    '
    ' Name: GetCorePartyDetails
    '
    ' Description:
    '
    ' History: 17/06/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function GetCorePartyDetails(ByRef vPartyCnt As Object, Optional ByRef vPartyTypeID As Object = Nothing, Optional ByRef vPartyStructureID As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vIsAlsoAgent As Object = Nothing, Optional ByRef vPartyID As Object = Nothing, Optional ByRef vShortname As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vResolvedName As Object = Nothing, Optional ByRef vCurrencyId As Object = Nothing, Optional ByRef vLanguageID As Object = Nothing, Optional ByRef vCollectTypeID As Object = Nothing, Optional ByRef vAccumTreatmentTypeID As Object = Nothing, Optional ByRef vStatsTreatmentTypeID As Object = Nothing, Optional ByRef vPartyCategoryID As Object = Nothing, Optional ByRef vAgentCnt As Object = Nothing, Optional ByRef vConsultantCnt As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vPaymentMethodCode As Object = Nothing, Optional ByRef vPaymentTermCode As Object = Nothing, Optional ByRef vCreditCardCode As Object = Nothing, Optional ByRef vFileCode As Object = Nothing, Optional ByRef vABCCount As Object = Nothing, Optional ByRef vStatements As Object = Nothing, Optional ByRef vReminderTypeId As Object = Nothing, Optional ByRef vRenewals As Object = Nothing, Optional ByRef vStatus As Object = Nothing, Optional ByRef vLAstActionType As Object = Nothing, Optional ByRef vIsTravelAgent As Object = Nothing, Optional ByRef vIsProspect As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vABICodeOn406 As Object = Nothing, Optional ByRef vABICodeOn81 As Object = Nothing, Optional ByRef vABICodeList As Object = Nothing, Optional ByRef vAreaId As Object = Nothing, Optional ByRef vServiceLevelId As Object = Nothing, Optional ByRef vInvariantKey As Object = Nothing, Optional ByRef vRecordStatus As Object = Nothing, Optional ByRef vCCJs As Object = Nothing, Optional ByRef vUserDefinedDataId As Object = Nothing, Optional ByRef vSeasonalGiftID As Object = Nothing, Optional ByRef vCorrespondenceTypeId As Object = Nothing, Optional ByRef vRenewalStopCodeId As Object = Nothing, Optional ByRef vSwiftPartyID As Object = Nothing, Optional ByRef vLoyaltyNumber As Object = Nothing, Optional ByRef vAlternativeIdentifier As Object = Nothing, Optional ByRef vMarketingSegementInd As Object = Nothing, Optional ByRef vTradingName As Object = Nothing, Optional ByRef vSubBranchId As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim oSIRPartyPC As bSIRPartyPC.SIRPartyPC

            oSIRPartyPC = New bSIRPartyPC.SIRPartyPC()
            m_lReturn = oSIRPartyPC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oSIRPartyPC.Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCorePartyDetails")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oSIRPartyPC.bSIRParty.GetDetails(vPartyCnt:=vPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oSIRPartyPC.bSIRParty.GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCorePartyDetails")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oSIRPartyPC.bSIRParty.GetNext(vSourceID:=vSourceID, vPartyID:=vPartyID, vShortname:=vShortname, vName:=vName, vIsAlsoAgent:=vIsAlsoAgent, vIsProspect:=vIsProspect, vAgentCnt:=vAgentCnt, vConsultantCnt:=vConsultantCnt, vFileCode:=vFileCode, vCurrencyId:=vCurrencyId, vPaymentMethodCode:=vPaymentMethodCode, vReminderTypeId:=vReminderTypeId, vAreaId:=vAreaId, vServiceLevelId:=vServiceLevelId, vCreditCardCode:=vCreditCardCode, vPaymentTermCode:=vPaymentTermCode, vCCJs:=vCCJs, vSeasonalGiftID:=vSeasonalGiftID, vCorrespondenceTypeId:=vCorrespondenceTypeId, vRenewalStopCodeId:=vRenewalStopCodeId, vResolvedName:=vResolvedName, vSwiftPartyID:=vSwiftPartyID, vLoyaltyNumber:=vLoyaltyNumber, vAlternativeIdentifier:=vAlternativeIdentifier, vMarketingSegementInd:=vMarketingSegementInd, vTradingName:=vTradingName, vSubBranchId:=vSubBranchId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oSIRPartyPC.bSIRParty.GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCorePartyDetails")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyPC = Nothing

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

            Dim oSIRPartyPC As bSIRPartyPC.SIRPartyPC

            oSIRPartyPC = New bSIRPartyPC.SIRPartyPC()

            m_lReturn = oSIRPartyPC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oSIRPartyPC.Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFutureDatedAddresses")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oSIRPartyPC.bSIRParty.GetFutureDatedAddresses(r_vFutureDatedAddresses:=r_vFutureDatedAddresses, v_vPartyCnt:=v_vPartyCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetFutureDatedAddresses Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFutureDatedAddresses")
                Return result
            End If

            oSIRPartyPC = Nothing

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

            Dim oSIRPartyPC As bSIRPartyPC.SIRPartyPC

            oSIRPartyPC = New bSIRPartyPC.SIRPartyPC()

            m_lReturn = oSIRPartyPC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oSIRPartyPC.Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateFutureDatedAddresses")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oSIRPartyPC.bSIRParty.CreateFutureDatedAddresses(v_lPartyCnt:=ToSafeInteger(v_lPartyCnt), v_vFutureDatedAddresses:=v_vFutureDatedAddresses)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="CreateFutureDatedAddresses Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateFutureDatedAddresses")
                Return result
            End If

            oSIRPartyPC = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateFutureDatedAddresses Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateFutureDatedAddresses", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

        Dim oSIRPartyPC As bSIRPartyPC.SIRPartyPC

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyCC - need to for to core for shortname
            oSIRPartyPC = New bSIRPartyPC.SIRPartyPC()

            m_lReturn = oSIRPartyPC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            m_lReturn = oSIRPartyPC.bSIRParty.GetAccountID(vPartyRef:=vPartyRef, vAccountID:=vAccountID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyPC = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetAccountID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim oSIRPartyPC As bSIRPartyPC.SIRPartyPC

        Try

            oSIRPartyPC = New bSIRPartyPC.SIRPartyPC()
            m_lReturn = oSIRPartyPC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oSIRPartyPC.Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckDuplicateShortname")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            result = oSIRPartyPC.bSIRParty.CheckDuplicateShortname(ToSafeString(v_sShortname), v_vMatchArray)


            oSIRPartyPC.Dispose()
            oSIRPartyPC = Nothing

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

