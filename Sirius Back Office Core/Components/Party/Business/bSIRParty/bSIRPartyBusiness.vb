Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
Imports System.Xml
Imports Microsoft.VisualBasic.CompilerServices

'Developer Guide No. 129
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
    '              a SIRParty.
    '
    ' Edit History:
    ' DJM 22/04/2002 : MainContactCnt changed from a int to a long.
    ' SP191198 - Gemini update call must be in bSIRPartyPC, not bSIRParty
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
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Collection of SIRPartys (Private)
    Private m_oSIRPartys As bSIRParty.SIRPartys

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
    'eck300101
    Private m_vParties() As Object

    Private m_bEvent As Boolean

    'MSS200901 - Added for merge
    Private m_sUnderwritingOrAgency As String = ""
    'MSS200901 - Merge End

    ' PM Lookup Business Component (Private)
    Private m_oLookup As BPMLOOKUP.Business
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Const knPartyHistoryLoggingEnabled As Integer = 5180

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
                Case Is > m_oSIRPartys.Count()
                    m_lCurrentRecord = m_oSIRPartys.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Number in Collection
            Return m_oSIRPartys.Count()

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
    'eck3100101
    Public Property Parties() As Object
        Get
            If m_vParties IsNot Nothing Then
                Return CType(m_vParties, Object()).Clone
            Else
                Return Nothing
            End If

        End Get
        Set(ByVal Value As Object)

            m_vParties = Value

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

    'MSS200901 - Added for Merge
    Public ReadOnly Property UnderwritingOrAgency() As String
        Get

            If m_sUnderwritingOrAgency = "" Then
                m_lReturn = getUnderwritingOrAgency()
            End If

            Return m_sUnderwritingOrAgency

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

            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create SIRPartys Collection
            m_oSIRPartys = New bSIRParty.SIRPartys()

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
                    m_oLookup = Nothing
                End If
                m_oSIRPartys = Nothing
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

    Public Function GetGISPolicyLinkForParty(ByVal lPartyCnt As Integer, ByRef r_lGISPolicyLinkID As Integer) As Integer
        ' ***************************************************************************
        ' NAME:     GetGISPolicyLinkForParty
        ' PURPOSE:  Gets the GIS Policy Link Id for the Party passed in.
        ' REF:      PN24332
        ' AUTHOR:   Andrew Robinson
        ' DATE:     23-Aug-2005
        ' RETURNS:  PMTrue for link id defined
        '           PMFalse for link not defined
        '           PMError if an error occurred
        ' ***************************************************************************

        Dim result As Integer = 0
        Try

            With m_oDatabase

                .Parameters.Clear()
                .Parameters.Add("party_cnt", CStr(lPartyCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)


                'Developer Guide No. 86
                .Parameters.Add("gis_policy_link_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLAction(ACPartyGetGisPolicyLinkSQL, ACPartyGetGisPolicyLinkName, ACPartyGetGisPolicyLinkSP)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(CStr(m_lReturn), "Stored procedure " & ACPartyGetGisPolicyLinkName & " failed.")
                End If

                r_lGISPolicyLinkID = gPMFunctions.ToSafeLong(.Parameters.Item("gis_policy_link_id").Value, -1)
                If r_lGISPolicyLinkID = -1 Then
                    'Not valid
                    result = gPMConstants.PMEReturnCode.PMFalse
                Else
                    result = gPMConstants.PMEReturnCode.PMTrue
                End If

            End With

            Return result

        Catch excep As System.Exception


            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGISPolicyLinkForParty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function

    Public Function GetGISScreenForParty(ByVal lPartyCnt As Integer, ByRef r_lGISScreenID As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetGISScreenForParty
        ' PURPOSE:  Gets the GIS Screen defined for the Party Type of the Party passed
        '           in.
        ' AUTHOR:   Danny Davis
        ' DATE:     18 February 2005, 11:45:29
        ' RETURNS:  PMTrue for screen available along with id
        '           PMCancel for screen not defined
        '           PMError for incorrect screen assigned (wrong data model type)
        '           PMFalse if something went wrong
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try


            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("party_cnt", CStr(lPartyCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                'Developer Guide No. 85
                .Parameters.Add("gis_screen_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
                'Developer Guide No. 39
                m_lReturn = .SQLAction("spu_Party_Get_GIS_Screen", "Get GIS Screen for Party", True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(CStr(m_lReturn), "Failed to run spu_Party_Get_GIS_Screen")
                End If


                If Convert.IsDBNull(.Parameters.Item("gis_screen_id").Value) Or Informations.IsNothing(.Parameters.Item("gis_screen_id").Value) Then
                    'Not defined
                    result = gPMConstants.PMEReturnCode.PMCancel
                Else
                    r_lGISScreenID = .Parameters.Item("gis_screen_id").Value
                    If r_lGISScreenID = -1 Then
                        'Not correct
                        result = gPMConstants.PMEReturnCode.PMError
                    Else
                        'OK
                        result = gPMConstants.PMEReturnCode.PMTrue
                    End If
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
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGISScreenForParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
            End Select

        Finally


        End Try
        Return result
    End Function

    Public Function GetGISCustomDataForParty(ByVal lPartyCnt As Integer, ByVal lGISScreenID As Integer, ByVal lGISPolicyLinkID As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetGISCustomDataForParty
        ' PURPOSE:  Gets the GIS Custom Data for the Party, ScreenID and Policy link ID
        '           passed in
        ' RETURNS:  PMTrue for custom data available
        '           PMFalse for custom data not available
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            With m_oDatabase

                .Parameters.Clear()

                .Parameters.Add("party_cnt", CStr(lPartyCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add("gis_screen_id", CStr(lGISScreenID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add("gis_policy_link_id", CStr(lGISPolicyLinkID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add("ifound", CStr(0), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger)

                'Developer Guide No. 39
                m_lReturn = .SQLAction("spu_Party_Get_GIS_CustomData", "Get GIS Custom Data for Party", True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(CStr(m_lReturn), "Failed to run spu_Party_Get_GIS_CustomData")
                End If


                If Convert.IsDBNull(.Parameters.Item("ifound").Value) Or Informations.IsNothing(.Parameters.Item("ifound").Value) Then
                    'Not Found
                    result = gPMConstants.PMEReturnCode.PMFalse
                Else
                    If .Parameters.Item("ifound").Value = 1 Then
                        'Found
                        result = gPMConstants.PMEReturnCode.PMTrue
                    Else
                        'Not Found
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If
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
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGISCustomDataForParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
            End Select

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a SIRParty.
    '
    '
    ' ***************************************************************** '
    ''Developer Guide No. 18
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim oSIRParty As bSIRParty.SIRParty = Nothing
        Dim dtEffectiveDate As Date

        ' {* USER DEFINED CODE (Begin) *}
        Dim vTabArray As Object
        Dim vPartyTypeID As String = ""
        'eck120500
        Dim vSourceID As String = ""
        ' {* USER DEFINED CODE (End) *}

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            vResultArray = Nothing
            ' Reset Table Array

            'Developer Guide No. 17
            vTableArray = Nothing

            ' {* USER DEFINED CODE (Begin) *}

            If iLookupType = gPMConstants.PMELookupType.PMLookupSingle Then
                ReDim vTabArray(3, 0)
            Else
                'eck120500
                ReDim vTabArray(3, 2)

                vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 1) = gSIRLibrary.SIRLookupRiskCode
                'SD 02/08/2002

                vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 2) = gSIRLibrary.SIRLookupSource
            End If

            ' Setup Lookup Table Names

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = gSIRLibrary.SIRLookupPartyType
            ' {* USER DEFINED CODE (End) *}

            ' Do we have any records
            If m_lCurrentRecord < 1 Then
                ' No, we can only lookup all
                iLookupType = gPMConstants.PMELookupType.PMLookupAll
            Else
                ' Yes get current record
                oSIRParty = m_oSIRPartys.Item(m_lCurrentRecord)
            End If

            Select Case iLookupType
                Case gPMConstants.PMELookupType.PMLookupAll

                    ' Do not supply a key

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = ""
                    'eck120500

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 2) = ""
                Case gPMConstants.PMELookupType.PMLookupAllEffective

                    ' Use keys and effective date from current record
                    ' Note: The keys are not used for the select, but are used by
                    '       the iterface program to set the list index.
                    With oSIRParty

                        ' {* USER DEFINED CODE (Begin) *}
                        'Developer Guide No.98
                        m_lReturn = .GetProperties(iStatus:=gPMConstants.PMEComponentAction.PMView, vPartyTypeID:=vPartyTypeID, vSourceID:=vSourceID)


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = vPartyTypeID

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = ""
                        'eck120500

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 2) = vSourceID
                        ' {* USER DEFINED CODE (End) *}

                    End With

                Case gPMConstants.PMELookupType.PMLookupSingle

                    ' Set keys from current record
                    With oSIRParty

                        ' {* USER DEFINED CODE (Begin) *}
                        'Developer Guide No.98
                        m_lReturn = .GetProperties(iStatus:=gPMConstants.PMEComponentAction.PMView, vPartyTypeID:=vPartyTypeID, vSourceID:=vSourceID)


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = vPartyTypeID
                        'eck120500

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = ""

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 2) = vSourceID
                        ' {* USER DEFINED CODE (End) *}
                    End With

            End Select

            ' Default Effective Date to current date/time
            dtEffectiveDate = DateTime.Now

            ' Release SIRParty reference
            oSIRParty = Nothing

            ' Get the Lookup items

            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTabArray, iLanguageID:=iLanguageID, dtEffectiveDate:=dtEffectiveDate, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the Table Array


            vTableArray = vTabArray

            vTabArray = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a SIRParty.
    '
    '
    ' ***************************************************************** '
    'DC 28/06/00 Added Contact Type Id

    Public Function GetLookupProperties(ByRef vCurrentRecord As Integer) As Integer
        Return GetLookupProperties(vCurrentRecord:=vCurrentRecord, vAreaId:=Nothing, vCurrencyId:=Nothing, vReminderTypeId:=Nothing,
                                   vServiceLevelId:=Nothing, vSeasonalGiftID:=Nothing, vCorrespondenceTypeId:=Nothing,
                                   vRenewalStopCodeId:=Nothing, vPaymentTermCode:=Nothing)
    End Function
    Public Function GetLookupProperties(ByRef vCurrentRecord As Integer, ByRef vAreaId As Object, ByRef vCurrencyId As Object,
                                         ByRef vReminderTypeId As Object, ByRef vServiceLevelId As Object,
                                         ByRef vSeasonalGiftID As Object, ByRef vCorrespondenceTypeId As Object,
                                         ByRef vRenewalStopCodeId As Object, ByRef vPaymentTermCode As Object) As Integer

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        result = gPMConstants.PMEReturnCode.PMTrue

        Dim oSIRParty As bSIRParty.SIRParty = m_oSIRPartys.Item(vCurrentRecord)

        With oSIRParty
            .GetProperties(iStatus:=gPMConstants.PMEComponentAction.PMView, vAreaId:=vAreaId, vCurrencyId:=vCurrencyId, vReminderTypeId:=vReminderTypeId,
                           vServiceLevelId:=vServiceLevelId, vSeasonalGiftID:=vSeasonalGiftID, vCorrespondenceTypeId:=vCorrespondenceTypeId,
                           vRenewalStopCodeId:=vRenewalStopCodeId, vPaymentTermCode:=vPaymentTermCode)
        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single SIRParty directly into the database.
    '        Note: The SIRParty will NOT be added to the collection.
    '
    ' ***************************************************************** '
    'eck230500
    'DC 28/06/00 Added Correspondence Type Id
    'sj 12/06/2002 - Added LoyaltyNumber, AlternativeIdentifier,
    '                MarketingSegmentInd, TradingName and SubBranchId
    'JAS(CMG) 03/09/02 - Added vRecordStatus As Variant
    'FSA Phase III TobLetter
    Public Function DirectAdd(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyTypeID As Object = Nothing, Optional ByRef vPartyStructureID As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vIsAlsoAgent As Object = Nothing, Optional ByRef vPartyID As Object = Nothing, Optional ByRef vShortname As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vResolvedName As Object = Nothing, Optional ByRef vCurrencyId As Object = Nothing, Optional ByRef vLanguageID As Object = Nothing, Optional ByRef vCollectTypeID As Object = Nothing, Optional ByRef vAccumTreatmentTypeID As Object = Nothing, Optional ByRef vStatsTreatmentTypeID As Object = Nothing, Optional ByRef vPartyCategoryID As Object = Nothing, Optional ByRef vAgentCnt As Object = Nothing, Optional ByRef vConsultantCnt As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vPaymentMethodCode As Object = Nothing, Optional ByRef vPaymentTermCode As Object = Nothing, Optional ByRef vCreditCardCode As Object = Nothing, Optional ByRef vFileCode As Object = Nothing, Optional ByRef vABCCount As Object = Nothing, Optional ByRef vStatements As Object = Nothing, Optional ByRef vReminderTypeId As Object = Nothing, Optional ByRef vRenewals As Object = Nothing, Optional ByRef vStatus As Object = Nothing, Optional ByRef vLAstActionType As Object = Nothing, Optional ByRef vIsTravelAgent As Object = Nothing, Optional ByRef vIsProspect As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vABICodeOn406 As Object = Nothing, Optional ByRef vABICodeOn81 As Object = Nothing, Optional ByRef vABICodeList As Object = Nothing, Optional ByRef vAreaId As Object = Nothing, Optional ByRef vServiceLevelId As Object = Nothing, Optional ByRef vInvariantKey As Object = Nothing, Optional ByRef vRecordStatus As Object = Nothing, Optional ByRef vCCJs As Object = Nothing, Optional ByRef vUserDefinedDataId As Object = Nothing, Optional ByRef vSeasonalGiftID As Object = Nothing, Optional ByRef vCorrespondenceTypeId As Object = Nothing, Optional ByRef vRenewalStopCodeId As Object = Nothing, Optional ByRef vSwiftPartyId As Object = Nothing, Optional ByRef vLoyaltyNumber As Object = Nothing, Optional ByRef vAlternativeIdentifier As Object = Nothing, Optional ByRef vMarketingSegementInd As Object = Nothing, Optional ByRef vTradingName As Object = Nothing, Optional ByRef vSubBranchId As Object = Nothing, Optional ByRef vTobLetter As Object = Nothing, Optional ByRef vOverrideCommission As Object = Nothing, Optional ByRef vOverrideCommissionRenewal As Object = Nothing, Optional ByVal vUniqueId As Object = Nothing, Optional ByVal vScreenHeirarchy As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oSIRParty As bSIRParty.SIRParty

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new SIRParty
            oSIRParty = New bSIRParty.SIRParty()
            m_lReturn = oSIRParty.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            ' Populate SIRParty Attributes
            'DC 28/06/00 Added Correspondence Type Id
            'DC 21/08/00 Payment Term incorrectly set to Payment Method
            'sj 12/06/2002 - Added LoyaltyNumber, AlternativeIdentifier,
            '                MarketingSegmentInd, TradingName and SubBranchId

            m_lReturn = oSIRParty.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vPartyCnt:=CInt(vPartyCnt), vPartyTypeID:=CInt(vPartyTypeID), vIsAlsoAgent:=vIsAlsoAgent, vPartyStructureID:=CInt(vPartyStructureID), vSourceID:=CInt(vSourceID), vPartyID:=CInt(vPartyID), vShortname:=CStr(vShortname), vName:=CStr(vName), vResolvedName:=vResolvedName, vCurrencyId:=CInt(vCurrencyId), vLanguageID:=CInt(vLanguageID), vCollectTypeID:=vCollectTypeID, vAccumTreatmentTypeID:=vAccumTreatmentTypeID, vStatsTreatmentTypeID:=vStatsTreatmentTypeID, vPartyCategoryID:=vPartyCategoryID, vAgentCnt:=vAgentCnt, vConsultantCnt:=vConsultantCnt, vCreatedByID:=CInt(vCreatedByID), vDateCreated:=CDate(vDateCreated), vLastModified:=vLastModified, vModifiedByID:=vModifiedByID, vPaymentMethodCode:=vPaymentMethodCode, vPaymentTermCode:=vPaymentTermCode, vCreditCardCode:=vCreditCardCode, vFileCode:=vFileCode, vABCCount:=vABCCount, vStatements:=CInt(vStatements), vReminderTypeId:=vReminderTypeId, vRenewals:=CInt(vRenewals), vStatus:=vStatus, vLAstActionType:=vLAstActionType, vIsTravelAgent:=CInt(vIsTravelAgent), vIsProspect:=CInt(vIsProspect), vIsDeleted:=CInt(vIsDeleted), vABICodeOn406:=vABICodeOn406, vABICodeOn81:=vABICodeOn81, vABICodeList:=vABICodeList, vAreaId:=vAreaId, vServiceLevelId:=CInt(vServiceLevelId), vInvariantKey:=CInt(vInvariantKey), vRecordStatus:=vRecordStatus, vCCJs:=CInt(vCCJs), vUserDefinedDataId:=vUserDefinedDataId, vSeasonalGiftID:=vSeasonalGiftID, vCorrespondenceTypeId:=vCorrespondenceTypeId, vRenewalStopCodeId:=vRenewalStopCodeId, vSwiftPartyId:=vSwiftPartyId, vLoyaltyNumber:=vLoyaltyNumber, vAlternativeIdentifier:=vAlternativeIdentifier, vMarketingSegmentInd:=vMarketingSegementInd, vTradingName:=vTradingName, vSubBranchId:=vSubBranchId, vTobLetter:=vTobLetter, vOverrideCommission:=CInt(vOverrideCommission), vOverrideCommissionRenewal:=CInt(vOverrideCommissionRenewal), vUniqueId:=vUniqueId, vScreenHeirarchy:=vScreenHeirarchy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRParty = Nothing
                Return result
            End If

            ' Add the SIRParty to the Database
            m_lReturn = oSIRParty.AddItem()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRParty = Nothing
                Return result
            End If

            ' Retain the Primary Key of the SIRParty Added
            With oSIRParty
                PartyCnt = .PartyCnt
            End With

            oSIRParty = Nothing

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
    ' Description: Deletes a single SIRParty directly from the database.
    '        Note: The SIRParty will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete() As Integer
        Return DirectDelete(vPartyCnt:=Nothing)
    End Function
    Public Function DirectDelete(ByRef vPartyCnt As Object) As Integer

        Dim result As Integer = 0
        Dim oSIRParty As bSIRParty.SIRParty

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new SIRParty
            oSIRParty = New bSIRParty.SIRParty()
            m_lReturn = oSIRParty.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            ' Set SIRParty Primary Key

            m_lReturn = oSIRParty.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMDelete, vPartyCnt:=CInt(vPartyCnt))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRParty = Nothing
                Return result
            End If

            ' Delete the SIRParty from the Database
            m_lReturn = oSIRParty.DeleteItem()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRParty = Nothing
                Return result
            End If

            oSIRParty = Nothing

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
    Public Function CheckID(ByRef vid As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the ID parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="id", vValue:=CStr(vid), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

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
    ' Name: GetPartyCnt
    '
    ' Description: Get party count for a given reference (ie shortname)
    '
    ' ***************************************************************** '
    Public Function GetPartyCnt() As Integer
        Return GetPartyCnt(vPartyRef:=Nothing, vPartyCnt:=0)
    End Function

    Public Function GetPartyCnt(ByRef vPartyRef As Object) As Integer
        Return GetPartyCnt(vPartyRef:=vPartyRef, vPartyCnt:=0)
    End Function

    Public Function GetPartyCnt(ByRef vPartyCnt As Integer) As Integer
        Return GetPartyCnt(vPartyRef:=Nothing, vPartyCnt:=vPartyCnt)
    End Function

    Public Function GetPartyCnt(ByRef vPartyRef As Object, ByRef vPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            If (Not Informations.IsNothing(vPartyRef)) And (Not Object.Equals(vPartyRef, Nothing)) Then

                'Thinh Nguyen 21/06/2002 (start) - change to stored procedure because old sql won't work with apostrophe in shortname
                m_oDatabase.Parameters.Clear()

                ' Add the ID parameter (INPUT)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="ShortName", vValue:=CStr(vPartyRef), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Get form DB
                '        m_lReturn& = m_oDatabase.SQLSelect(sSQL:=ACGetPartyCntSQL & "'" & vPartyRef & "'", _
                ''                                            sSQLName:=ACGetPartyCntName, _
                ''                                            bStoredProcedure:=ACGetPartyCntStored, _
                ''                                            lNumberRecords:=1, _
                ''                                            vResultArray:=vResultArray)

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPartyCntSQL, sSQLName:=ACGetPartyCntName, bStoredProcedure:=ACGetPartyCntStored, lNumberRecords:=1, vResultArray:=vResultArray)

                'Thinh Nguyen 21/06/2002 (end) - change to stored procedure because old sql won't work with apostrophe in shortname
                'Get form DB

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'return the data
                If Not Informations.IsArray(vResultArray) Then
                    vPartyCnt = 0
                Else

                    vPartyCnt = CInt(vResultArray(0, 0))
                End If

            End If


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
    'Developer Guide No. 101
    Public Function GetAccountID() As Integer
        Return GetAccountID(vPartyRef:=Nothing, vAccountID:=Nothing)
    End Function
    Public Function GetAccountID(ByRef vPartyRef As Object, ByRef vAccountID As Object) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            If (Not Informations.IsNothing(vPartyRef)) And (Not Object.Equals(vPartyRef, Nothing)) Then

                'Thinh Nguyen 21/06/2002 (start) - change to stored procedure because old sql won't work with apostrophe in shortname
                m_oDatabase.Parameters.Clear()

                ' Add the ID parameter (INPUT)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                'Developer Guide No. 86
                m_lReturn = m_oDatabase.Parameters.Add(sName:="sub_branch_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = m_oDatabase.Parameters.Add(sName:="ShortCode", vValue:=CStr(vPartyRef).Trim(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                'Developer Guide No. 86
                m_lReturn = m_oDatabase.Parameters.Add(sName:="AccountID", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Developer Guide No. 39
                m_lReturn = m_oDatabase.SQLSelect(sSQL:="SPU_ACT_GET_ACCOUNTID_FROM_SHORTCODE", sSQLName:="SPU_ACT_GET_ACCOUNTID_FROM_SHORTCODE", bStoredProcedure:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                If Convert.IsDBNull(m_oDatabase.Parameters.Item("AccountID").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("AccountID").Value) Then
                    vAccountID = 0
                Else
                    vAccountID = m_oDatabase.Parameters.Item("AccountID").Value
                End If

            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetAccountID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function




    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required SIRPartys and populate the Collection
    '
    ' ***************************************************************** '
    'Developer Guide No. 101
    Public Function GetDetails() As Integer
        Return GetDetails(vLockMode:=0, vPartyCnt:=0)
    End Function
    Public Function GetDetails(ByRef vPartyCnt As Integer) As Integer
        Return GetDetails(vLockMode:=0, vPartyCnt:=vPartyCnt)
    End Function
    Public Function GetDetails(ByRef vLockMode As Integer, ByRef vPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        'Developer Guide No. 112
        Dim oFields As DataRow
        Dim oSIRParty As bSIRParty.SIRParty

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oSIRPartys.Clear()

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

                ' Create New SIRParty
                oSIRParty = New bSIRParty.SIRParty()
                m_lReturn = oSIRParty.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

                ' Set component primary keys
                With oSIRParty
                    .PartyCnt = vPartyCnt

                    'And if we're from events
                    .FromEvent = FromEvent

                    m_lReturn = .SelectItem()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End With

                ' Add SIRParty to collection
                If m_oSIRPartys.Count = 0 Then
                    m_oSIRPartys.Add(Nothing)
                End If
                m_lReturn = m_oSIRPartys.Add(oNewSIRParty:=oSIRParty)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                oSIRParty = Nothing

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
                    oSIRParty = New bSIRParty.SIRParty()
                    m_lReturn = oSIRParty.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

                    ' Set oFields to refer to one Record
                    'Developer Guide No. 111
                    oFields = m_oDatabase.Records.Item(lSub - 1).Fields()

                    ' Set component primary keys from current record
                    With oSIRParty
                        .PartyCnt = gPMFunctions.NullToLong(oFields("party_cnt"))

                        m_lReturn = .SelectItem()

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End With

                    ' Add SIRParty to collection
                    If m_oSIRPartys.Count = 0 Then
                        m_oSIRPartys.Add(Nothing)
                    End If
                    m_lReturn = m_oSIRPartys.Add(oNewSIRParty:=oSIRParty)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oSIRParty = Nothing

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
    ' Description: Gets the required SIRPartys and populate the Collection
    '
    ' ***************************************************************** '
    'DC 28/06/00 Added Correspondence Type Id
    'sj 12/06/2002 - Added LoyaltyNumber, AlternativeIdentifier,
    '                MarketingSegmentInd, TradingName and SubBranchId
    'SD 17/07/2002 - Added SubBranchName
    'JAS(CMG) 05/09/02 - Added vRecordStatus As Variant
    'FSA Phase III TobLetter
    Public Function GetNext(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyTypeID As Object = Nothing, Optional ByRef vPartyStructureID As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vIsAlsoAgent As Object = Nothing, Optional ByRef vPartyID As Object = Nothing, Optional ByRef vShortname As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vResolvedName As Object = Nothing, Optional ByRef vCurrencyId As Object = Nothing, Optional ByRef vLanguageID As Object = Nothing, Optional ByRef vCollectTypeID As Object = Nothing, Optional ByRef vAccumTreatmentTypeID As Object = Nothing, Optional ByRef vStatsTreatmentTypeID As Object = Nothing, Optional ByRef vPartyCategoryID As Object = Nothing, Optional ByRef vAgentCnt As Object = Nothing, Optional ByRef vConsultantCnt As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vPaymentMethodCode As Object = Nothing, Optional ByRef vPaymentTermCode As Object = Nothing, Optional ByRef vPFFrequencyID As Object = Nothing, Optional ByRef vCreditCardCode As Object = Nothing, Optional ByRef vFileCode As Object = Nothing, Optional ByRef vABCCount As Object = Nothing, Optional ByRef vStatements As Object = Nothing, Optional ByRef vReminderTypeId As Object = Nothing, Optional ByRef vRenewals As Object = Nothing, Optional ByRef vStatus As Object = Nothing, Optional ByRef vLAstActionType As Object = Nothing, Optional ByRef vIsTravelAgent As Object = Nothing, Optional ByRef vIsProspect As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vABICodeOn406 As Object = Nothing, Optional ByRef vABICodeOn81 As Object = Nothing, Optional ByRef vABICodeList As Object = Nothing, Optional ByRef vAreaId As Object = Nothing, Optional ByRef vServiceLevelId As Object = Nothing, Optional ByRef vInvariantKey As Object = Nothing, Optional ByRef vRecordStatus As Object = Nothing, Optional ByRef vCCJs As Object = Nothing, Optional ByRef vUserDefinedDataId As Object = Nothing, Optional ByRef vSeasonalGiftID As Object = Nothing, Optional ByRef vCorrespondenceTypeId As Object = Nothing, Optional ByRef vRenewalStopCodeId As Object = Nothing, Optional ByRef vSwiftPartyId As Object = Nothing, Optional ByRef vLoyaltyNumber As Object = Nothing, Optional ByRef vAlternativeIdentifier As Object = Nothing, Optional ByRef vMarketingSegementInd As Object = Nothing, Optional ByRef vTradingName As Object = Nothing, Optional ByRef vSubBranchId As Object = Nothing, Optional ByRef vSubBranchName As Object = Nothing, Optional ByRef vTobLetter As Object = Nothing, Optional ByRef vOverrideCommission As Object = Nothing, Optional ByRef vOverrideCommissionRenewal As Object = Nothing, Optional ByRef vParamArray() As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRParty As bSIRParty.SIRParty
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oSIRPartys.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oSIRParty = m_oSIRPartys.Item(m_lCurrentRecord)

            'Developer Guide No. 98
            m_lReturn = oSIRParty.GetProperties(iStatus,
            vPartyCnt:=vPartyCnt, vPartyTypeID:=vPartyTypeID,
            vIsAlsoAgent:=vIsAlsoAgent,
            vPartyStructureID:=vPartyStructureID, vSourceID:=vSourceID,
            vPartyID:=vPartyID, vShortname:=vShortname,
            vName:=vName, vResolvedName:=vResolvedName,
            vCurrencyId:=vCurrencyId, vLanguageID:=vLanguageID,
            vCollectTypeID:=vCollectTypeID, vAccumTreatmentTypeID:=vAccumTreatmentTypeID,
            vStatsTreatmentTypeID:=vStatsTreatmentTypeID, vPartyCategoryID:=vPartyCategoryID,
            vAgentCnt:=vAgentCnt, vConsultantCnt:=vConsultantCnt,
            vCreatedByID:=vCreatedByID, vDateCreated:=vDateCreated,
            vLastModified:=vLastModified, vModifiedByID:=vModifiedByID,
            vPaymentMethodCode:=vPaymentMethodCode, vPaymentTermCode:=vPaymentTermCode,
            vCreditCardCode:=vCreditCardCode, vFileCode:=vFileCode,
            vABCCount:=vABCCount, vStatements:=vStatements,
            vReminderTypeId:=vReminderTypeId, vRenewals:=vRenewals,
            vStatus:=vStatus, vLAstActionType:=vLAstActionType,
            vIsTravelAgent:=vIsTravelAgent, vIsProspect:=vIsProspect,
            vIsDeleted:=vIsDeleted, vABICodeOn406:=vABICodeOn406, vABICodeOn81:=vABICodeOn81, vABICodeList:=vABICodeList,
            vAreaId:=vAreaId, vServiceLevelId:=vServiceLevelId, vInvariantKey:=vInvariantKey, vRecordStatus:=vRecordStatus,
            vCCJs:=vCCJs, vUserDefinedDataId:=vUserDefinedDataId, vSeasonalGiftID:=vSeasonalGiftID,
            vCorrespondenceTypeId:=vCorrespondenceTypeId, vRenewalStopCodeId:=vRenewalStopCodeId,
            vSwiftPartyId:=vSwiftPartyId, vLoyaltyNumber:=vLoyaltyNumber, vAlternativeIdentifier:=vAlternativeIdentifier, vMarketingSegmentInd:=vMarketingSegementInd,
            vTradingName:=vTradingName, vSubBranchId:=vSubBranchId, vSubBranchName:=vSubBranchName, vTobLetter:=vTobLetter, vOverrideCommission:=vOverrideCommission, vOverrideCommissionRenewal:=vOverrideCommissionRenewal, vParamArray:=vParamArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRParty = Nothing
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
    ' Description: Adds the supplied SIRParty into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    'DC 28/06/00 Added Correspondence Type Id
    'sj 12/06/2002 - Added LoyaltyNumber, AlternativeIdentifier,
    '                MarketingSegmentInd, TradingName and SubBranchId
    'FSA Phase III TobLetter
    'Developer Guide No. 113
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyTypeID As Object = Nothing, Optional ByRef vPartyStructureID As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vIsAlsoAgent As Object = Nothing, Optional ByRef vPartyID As Object = Nothing, Optional ByRef vShortname As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vResolvedName As Object = Nothing, Optional ByRef vCurrencyId As Object = Nothing, Optional ByRef vLanguageID As Object = Nothing, Optional ByRef vCollectTypeID As Object = Nothing, Optional ByRef vAccumTreatmentTypeID As Object = Nothing, Optional ByRef vStatsTreatmentTypeID As Object = Nothing, Optional ByRef vPartyCategoryID As Object = Nothing, Optional ByRef vAgentCnt As Object = Nothing, Optional ByRef vConsultantCnt As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vPaymentMethodCode As Object = Nothing, Optional ByRef vPaymentTermCode As Object = Nothing, Optional ByRef vPFFrequencyID As Object = Nothing, Optional ByRef vCreditCardCode As Object = Nothing, Optional ByRef vFileCode As Object = Nothing, Optional ByRef vABCCount As Object = Nothing, Optional ByRef vStatements As Object = Nothing, Optional ByRef vReminderTypeId As Object = Nothing, Optional ByRef vRenewals As Object = Nothing, Optional ByRef vStatus As Object = Nothing, Optional ByRef vLAstActionType As Object = Nothing, Optional ByRef vIsTravelAgent As Object = Nothing, Optional ByRef vIsProspect As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vABICodeOn406 As Object = Nothing, Optional ByRef vABICodeOn81 As Object = Nothing, Optional ByRef vABICodeList As Object = Nothing, Optional ByRef vAreaId As Object = Nothing, Optional ByRef vServiceLevelId As Object = Nothing, Optional ByRef vInvariantKey As Object = Nothing, Optional ByRef vRecordStatus As Object = Nothing, Optional ByRef vCCJs As Object = Nothing, Optional ByRef vUserDefinedDataId As Object = Nothing, Optional ByRef vSeasonalGiftID As Object = Nothing, Optional ByRef vCorrespondenceTypeId As Object = Nothing, Optional ByRef vRenewalStopCodeId As Object = Nothing, Optional ByRef vSwiftPartyId As Object = Nothing, Optional ByRef vLoyaltyNumber As Object = Nothing, Optional ByRef vAlternativeIdentifier As Object = Nothing, Optional ByRef vMarketingSegementInd As Object = Nothing, Optional ByRef vTradingName As Object = Nothing, Optional ByRef vSubBranchId As Object = Nothing, Optional ByRef vTobLetter As Object = Nothing, Optional ByRef vOverrideCommission As Object = Nothing, Optional ByRef vOverrideCommissionRenewal As Object = Nothing, Optional ByRef vParamArray As Object = Nothing, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As Integer
        Dim result As Integer = 0
        Dim oSIRParty As bSIRParty.SIRParty

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oSIRPartys.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new SIRParty
            oSIRParty = New bSIRParty.SIRParty()
            m_lReturn = oSIRParty.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            ' Populate SIRParty Attributes
            'DC 28/06/00 Added Correspondence Type Id
            'sj 12/06/2002 - Added LoyaltyNumber, AlternativeIdentifier,
            '                MarketingSegmentInd, TradingName and SubBranchId
            'FSA Phase III


            'Developer Guide No. 98
            m_lReturn = oSIRParty.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vPartyCnt:=vPartyCnt, vPartyTypeID:=vPartyTypeID, vIsAlsoAgent:=vIsAlsoAgent, vPartyStructureID:=vPartyStructureID, vSourceID:=vSourceID, vPartyID:=vPartyID, vShortname:=vShortname, vName:=vName, vResolvedName:=vResolvedName, vCurrencyId:=vCurrencyId, vLanguageID:=vLanguageID, vCollectTypeID:=vCollectTypeID, vAccumTreatmentTypeID:=vAccumTreatmentTypeID, vStatsTreatmentTypeID:=vStatsTreatmentTypeID, vPartyCategoryID:=vPartyCategoryID, vAgentCnt:=vAgentCnt, vConsultantCnt:=vConsultantCnt, vCreatedByID:=vCreatedByID, vDateCreated:=vDateCreated, vLastModified:=vLastModified, vModifiedByID:=vModifiedByID, vPaymentMethodCode:=vPaymentMethodCode, vPaymentTermCode:=vPFFrequencyID, vCreditCardCode:=vCreditCardCode, vFileCode:=vFileCode, vABCCount:=vABCCount, vStatements:=vStatements, vReminderTypeId:=vReminderTypeId, vRenewals:=vRenewals, vStatus:=vStatus, vLAstActionType:=vLAstActionType, vIsTravelAgent:=vIsTravelAgent, vIsProspect:=vIsProspect, vIsDeleted:=vIsDeleted, vABICodeOn406:=vABICodeOn406, vABICodeOn81:=vABICodeOn81, vABICodeList:=vABICodeList, vAreaId:=vAreaId, vServiceLevelId:=vServiceLevelId, vInvariantKey:=vInvariantKey, vRecordStatus:=vRecordStatus, vCCJs:=vCCJs, vUserDefinedDataId:=vUserDefinedDataId, vSeasonalGiftID:=vSeasonalGiftID, vCorrespondenceTypeId:=vCorrespondenceTypeId, vRenewalStopCodeId:=vRenewalStopCodeId, vSwiftPartyId:=vSwiftPartyId, vLoyaltyNumber:=vLoyaltyNumber, vAlternativeIdentifier:=vAlternativeIdentifier, vMarketingSegmentInd:=vMarketingSegementInd, vTradingName:=vTradingName, vSubBranchId:=vSubBranchId, vTobLetter:=vTobLetter, vOverrideCommission:=vOverrideCommission, vOverrideCommissionRenewal:=vOverrideCommissionRenewal, vUniqueId:=ToSafeString(sUniqueId), vScreenHeirarchy:=ToSafeString(sScreenHierarchy))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oSIRParty = Nothing
                Return result
            End If

            ' Add SIRParty to collection
            If m_oSIRPartys.Count = 0 Then
                m_oSIRPartys.Add(Nothing)
            End If
            m_lReturn = m_oSIRPartys.Add(oNewSIRParty:=oSIRParty)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRParty = Nothing
                Return result
            End If

            oSIRParty = Nothing

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
    ' Description: Validates that this action is valid on the SIRParty
    '              specified and updates the SIRParty with the new values.
    '
    ' ***************************************************************** '
    'DC 28/06/00 Added Correspondence Type Id
    'sj 12/06/2002 - Added LoyaltyNumber, AlternativeIdentifier,
    '                MarketingSegmentInd, TradingName and SubBranchId
    'FSA Phase III TobLetter
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyTypeID As Object = Nothing,
                               Optional ByRef vPartyStructureID As Object = Nothing,
                               Optional ByRef vSourceID As Object = Nothing, Optional ByRef vIsAlsoAgent As Object = Nothing,
                               Optional ByRef vPartyID As Object = Nothing, Optional ByRef vShortname As Object = Nothing,
                               Optional ByRef vName As Object = Nothing, Optional ByRef vResolvedName As Object = Nothing,
                               Optional ByRef vCurrencyId As Object = Nothing, Optional ByRef vLanguageID As Object = Nothing,
                               Optional ByRef vCollectTypeID As Object = Nothing, Optional ByRef vAccumTreatmentTypeID As Object = Nothing,
                               Optional ByRef vStatsTreatmentTypeID As Object = Nothing, Optional ByRef vPartyCategoryID As Object = Nothing,
                               Optional ByRef vAgentCnt As Object = Nothing, Optional ByRef vConsultantCnt As Object = Nothing,
                               Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing,
                               Optional ByRef vLastModified As Object = Nothing, Optional ByRef vModifiedByID As Object = Nothing,
                               Optional ByRef vPaymentMethodCode As Object = Nothing,
                               Optional ByRef vPaymentTermCode As Object = Nothing,
                               Optional ByRef vPFFrequencyID As Object = Nothing, Optional ByRef vCreditCardCode As Object = Nothing, Optional ByRef vFileCode As Object = Nothing, Optional ByRef vABCCount As Object = Nothing, Optional ByRef vStatements As Object = Nothing, Optional ByRef vReminderTypeId As Object = Nothing, Optional ByRef vRenewals As Object = Nothing, Optional ByRef vStatus As Object = Nothing, Optional ByRef vLAstActionType As Object = Nothing, Optional ByRef vIsTravelAgent As Object = Nothing, Optional ByRef vIsProspect As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vABICodeOn406 As Object = Nothing, Optional ByRef vABICodeOn81 As Object = Nothing, Optional ByRef vABICodeList As Object = Nothing, Optional ByRef vAreaId As Object = Nothing, Optional ByRef vServiceLevelId As Object = Nothing, Optional ByRef vInvariantKey As Object = Nothing, Optional ByRef vRecordStatus As Object = Nothing, Optional ByRef vCCJs As Object = Nothing, Optional ByRef vUserDefinedDataId As Object = Nothing, Optional ByRef vSeasonalGiftID As Object = Nothing, Optional ByRef vCorrespondenceTypeId As Object = Nothing, Optional ByRef vRenewalStopCodeId As Object = Nothing, Optional ByRef vSwiftPartyId As Object = Nothing, Optional ByRef vLoyaltyNumber As Object = Nothing, Optional ByRef vAlternativeIdentifier As Object = Nothing, Optional ByRef vMarketingSegementInd As Object = Nothing, Optional ByRef vTradingName As Object = Nothing, Optional ByRef vSubBranchId As Object = Nothing, Optional ByRef vTobLetter As Object = Nothing, Optional ByRef vOverrideCommission As Object = Nothing, Optional ByRef vOverrideCommissionRenewal As Object = Nothing, Optional ByRef vParamArray() As Object = Nothing, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As Integer


        Dim result As Integer = 0
        Dim oSIRParty As bSIRParty.SIRParty
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oSIRPartys.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oSIRParty = m_oSIRPartys.Item(lRow)

            ' Check the Status of the SIRParty

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oSIRParty.DatabaseStatus
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

            ' Update SIRParty Attributes
            'DC 28/06/00 Added Correspondence Type Id
            'sj 12/06/2002 - Added LoyaltyNumber, AlternativeIdentifier,
            '                MarketingSegmentInd, TradingName and SubBranchId
            'FSA Phase III

            'Developer Guide No. 101
            If vPFFrequencyID <> 0 Then
                vPaymentTermCode = vPFFrequencyID
            End If

            m_lReturn = oSIRParty.SetProperties(iStatus:=iStatus, vPartyCnt:=vPartyCnt, vPartyTypeID:=vPartyTypeID, vIsAlsoAgent:=vIsAlsoAgent, vPartyStructureID:=vPartyStructureID, vSourceID:=vSourceID, vPartyID:=vPartyID, vShortname:=vShortname, vName:=vName, vResolvedName:=vResolvedName, vCurrencyId:=vCurrencyId, vLanguageID:=vLanguageID, vCollectTypeID:=vCollectTypeID, vAccumTreatmentTypeID:=vAccumTreatmentTypeID, vStatsTreatmentTypeID:=vStatsTreatmentTypeID, vPartyCategoryID:=vPartyCategoryID, vAgentCnt:=vAgentCnt, vConsultantCnt:=vConsultantCnt, vCreatedByID:=vCreatedByID, vDateCreated:=CDate(vDateCreated), vLastModified:=vLastModified, vModifiedByID:=vModifiedByID, vPaymentMethodCode:=vPaymentMethodCode, vPaymentTermCode:=vPaymentTermCode, vCreditCardCode:=vCreditCardCode, vFileCode:=vFileCode, vABCCount:=vABCCount, vStatements:=vStatements, vReminderTypeId:=vReminderTypeId, vRenewals:=vRenewals, vStatus:=vStatus, vLAstActionType:=vLAstActionType, vIsTravelAgent:=vIsTravelAgent, vIsProspect:=vIsProspect, vIsDeleted:=vIsDeleted, vABICodeOn406:=vABICodeOn406, vABICodeOn81:=vABICodeOn81, vABICodeList:=vABICodeList, vAreaId:=vAreaId, vServiceLevelId:=vServiceLevelId, vInvariantKey:=vInvariantKey, vRecordStatus:=vRecordStatus, vCCJs:=vCCJs, vUserDefinedDataId:=vUserDefinedDataId, vSeasonalGiftID:=vSeasonalGiftID, vCorrespondenceTypeId:=vCorrespondenceTypeId, vRenewalStopCodeId:=vRenewalStopCodeId, vSwiftPartyId:=vSwiftPartyId, vLoyaltyNumber:=vLoyaltyNumber, vAlternativeIdentifier:=vAlternativeIdentifier, vMarketingSegmentInd:=vMarketingSegementInd, vTradingName:=vTradingName, vSubBranchId:=vSubBranchId, vTobLetter:=vTobLetter, vOverrideCommission:=vOverrideCommission, vOverrideCommissionRenewal:=vOverrideCommissionRenewal, vParamArray:=vParamArray, vUniqueId:=ToSafeString(sUniqueId), vScreenHeirarchy:=ToSafeString(sScreenHierarchy))


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oSIRParty = Nothing
                Return result
            End If

            ' Release reference to SIRParty
            oSIRParty = Nothing

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
    ' Description: Validate that the specified SIRParty can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oSIRParty As bSIRParty.SIRParty

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oSIRPartys.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oSIRParty = m_oSIRPartys.Item(lRow)

            ' Check the Status of the SIRParty

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oSIRParty.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oSIRParty.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oSIRParty.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to SIRParty
            oSIRParty = Nothing

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
            For lSub As Integer = 1 To m_oSIRPartys.Count()
                Select Case m_oSIRPartys.Item(lSub).DatabaseStatus
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
        Dim oSIRParty As bSIRParty.SIRParty = Nothing
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' CTAF 010200 - Exit if nothing in the collection
            If m_oSIRPartys.Count() = 0 Then
                Return result
            End If

            ' Loop round Collection

            For lSub = 1 To m_oSIRPartys.Count()
                oSIRParty = m_oSIRPartys.Item(lSub)


                Select Case oSIRParty.DatabaseStatus
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
                        m_lReturn = oSIRParty.AddItem()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If
                        'eck30101
                        If Informations.IsArray(m_vParties) Then
                            ReDim Preserve m_vParties(m_vParties.GetUpperBound(0) + 1)
                            m_vParties(m_vParties.GetUpperBound(0)) = oSIRParty.PartyCnt
                        Else
                            ReDim m_vParties(0)
                            m_vParties(0) = oSIRParty.PartyCnt
                        End If

                        Parties = m_vParties.Clone

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
                        m_lReturn = oSIRParty.UpdateItem()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If
                        'For PN-41025
                        If Informations.IsArray(m_vParties) Then
                            ReDim Preserve m_vParties(m_vParties.GetUpperBound(0) + 1)
                            m_vParties(m_vParties.GetUpperBound(0)) = oSIRParty.PartyCnt
                        Else
                            ReDim m_vParties(0)
                            m_vParties(0) = oSIRParty.PartyCnt
                        End If

                        Parties = m_vParties.Clone
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
                        m_lReturn = oSIRParty.DeleteItem()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Retain the Primary Key of the SIRParty
            With oSIRParty
                PartyCnt = .PartyCnt
            End With

            ' Release last reference
            oSIRParty = Nothing

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
                    Do While lSub <= m_oSIRPartys.Count()

                        ' With the item
                        With m_oSIRPartys.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oSIRPartys.Delete(lSub)

                                    ' Anything Else
                                Case Else
                                    ' Set Status to view
                                    .DatabaseStatus = gPMConstants.PMEComponentAction.PMView
                                    lSub += 1

                            End Select

                        End With

                    Loop

                    Dim PartyType As Integer
                    m_oSIRPartys.Item(lSub - 1).GetProperties(1, vPartyTypeID:=PartyType)

                    'PC, CC, GC only
                    If PartyType = 1 Or PartyType = 2 Or PartyType = 4 Then
                        'Generate a default Sharepoint folder (if Sharepoint is enabled)
                        Dim Sharepoint As bSIRSharepoint.Business
                        Sharepoint = New bSIRSharepoint.Business
                        Sharepoint.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                        Sharepoint.GenerateDefaultPath(PartyCnt, 0, 0, 0)
                    End If
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
    ' Name: GetOtherDetails
    '
    ' Description: Get other details from DB, for related parties.
    '
    ' ***************************************************************** '
    Public Function GetOtherDetails(ByRef vAgentCnt As Object) As Integer
        Return GetOtherDetails(vAgentCnt:=vAgentCnt, vAgentRef:="", vAgentName:="")
    End Function
    Public Function GetOtherDetails(ByRef vAgentCnt As Object, ByRef vAgentRef As String) As Integer
        Return GetOtherDetails(vAgentCnt:=vAgentCnt, vAgentRef:=vAgentRef, vAgentName:="")
    End Function
    Public Function GetOtherDetails(ByRef vAgentCnt As Object, ByRef vAgentRef As String, ByRef vAgentName As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue




            If (Not Informations.IsNothing(vAgentCnt)) And (Not Object.Equals(vAgentCnt, Nothing)) And (Not (Convert.IsDBNull(vAgentCnt) Or Informations.IsNothing(vAgentCnt))) Then

                'Get form DB

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAgentDetailsSQL & CStr(vAgentCnt), sSQLName:=ACGetAgentDetailsName, bStoredProcedure:=ACGetAgentDetailsStored, lNumberRecords:=1, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'return the data
                If Not Informations.IsArray(vResultArray) Then
                    vAgentRef = ""
                    vAgentName = ""
                Else

                    vAgentRef = CStr(vResultArray(0, 0))

                    vAgentName = CStr(vResultArray(1, 0))
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
    ' Name: GetOtherSummaryDetails
    '
    ' Description: Get other details from DB, for related parties.
    '
    ' ***************************************************************** '
    Public Function GetOtherSummaryDetails(ByRef vAgentCnt As Object, ByRef vAgentRef As Object, ByRef vAgentName As Object, ByRef vConsultantCnt As Object, ByRef vConsultantRef As Object, ByRef vConsultantname As Object, ByRef vPartyCnt As Object, ByRef vAssociates As Object) As Integer
        Return GetOtherSummaryDetails(vAgentCnt:=vAgentCnt, vAgentRef:=vAgentRef, vAgentName:=vAgentName, vEmployerCnt:=Nothing, vEmployerRef:="", vConsultantCnt:=vConsultantCnt, vConsultantRef:=vConsultantRef, vConsultantname:=vConsultantname, vPartyCnt:=vPartyCnt, vAssociates:=vAssociates)
    End Function
    Public Function GetOtherSummaryDetails(ByRef vAgentCnt As Object, ByRef vAgentRef As Object, ByRef vAgentName As Object, ByRef vEmployerCnt As Object, ByRef vEmployerRef As String, ByRef vConsultantCnt As Object, ByRef vConsultantRef As Object, ByRef vConsultantname As Object, ByRef vPartyCnt As Object, ByRef vAssociates As Object) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            'Developer Guide No. 98
            m_lReturn = GetOtherDetails(vAgentCnt:=vAgentCnt, vAgentRef:=vAgentRef, vAgentName:=vAgentName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If




            If (Not Informations.IsNothing(vConsultantCnt)) And (Not Object.Equals(vConsultantCnt, Nothing)) And (Not (Convert.IsDBNull(vConsultantCnt) Or Informations.IsNothing(vConsultantCnt))) Then



                'Developer Guide No. 98
                m_lReturn = GetOtherDetails(vAgentCnt:=vConsultantCnt, vAgentRef:=vConsultantRef, vAgentName:=vConsultantname)
            End If

            ' DC 29/03/00
            ' Cater for more than one Associate
            '    m_lReturn = GetAssociateDetails(vPartyCnt:=vPartyCnt, _
            ''                                   vIsAssociate:=PMTrue, _
            ''                                   vAssociates:=vAssociates)

            m_lReturn = GetAssociates(vPartyCnt:=vPartyCnt, vAssociates:=vAssociates)




            If (Not Informations.IsNothing(vEmployerCnt)) And (Not Object.Equals(vEmployerCnt, Nothing)) And (Not (Convert.IsDBNull(vEmployerCnt) Or Informations.IsNothing(vEmployerCnt))) Then

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetOtherSummaryDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOtherSummaryDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' DC 23/03/00
    ' Cater for more than one associate
    ' CMG 16/7/2002 Only get the client relationship types if optional parameter not supplied
    'Developer Guide No. 101
    Public Function GetAssociates(ByRef vPartyCnt As Object, ByRef vAssociates As Object) As Integer
        Return GetAssociates(vPartyCnt:=vPartyCnt, vAssociates:=vAssociates, lPartyRelationshipGroupId:=1)
    End Function
    Public Function GetAssociates(ByRef vPartyCnt As Object, ByRef vAssociates As Object, ByRef lPartyRelationshipGroupId As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'DC 29/03/00
            'Cater for more than one Associate


            sSQL = "SELECT pr.relation_cnt, p.shortname, " &
                   "p.resolved_name, pr.relationship_type_id, " &
                   "pr.description, pr.commission_transaction, c.Currency_id " &
                   "FROM party_relationship pr, party p, relationship_type rt, " &
                   "party_relationship_group prg, currency c " &
                   "WHERE pr.party_cnt = " & CStr(CInt(vPartyCnt)) & " AND " &
                   "pr.relation_cnt = p.party_cnt" &
                   " AND pr.relationship_type_id = rt.relationship_type_id" &
                   " AND prg.party_relationship_group_id = rt.party_relationship_group_id" &
                   " AND p.currency_id = c.currency_id" &
                   " AND rt.party_relationship_group_id = " & CStr(lPartyRelationshipGroupId)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="PUTASSOCIATES", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    Get from DB
            '    If vIsAssociate = PMTrue Then
            '
            '        ' CF 171199 - Added short code to select
            '
            '        sSQL = "SELECT party_relationship.party_cnt, description, party.resolved_name, shortname " & _
            ''                "FROM party_relationship, party WHERE " & _
            ''                "relation_cnt = " & CLng(vPartyCnt) & " AND " & _
            ''                "relationship_type_id = 1" & " AND " & _
            ''                "party_relationship.party_cnt = party.party_cnt"
            '
            '        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL$, _
            ''                                            sSQLname:="PUTASSOCIATES", _
            ''                                            bStoredprocedure:=False, _
            ''                                            lNumberRecords:=1, _
            ''                                            vResultArray:=vResultArray)
            '
            '            If (m_lReturn& <> PMTrue) Then
            '                GetAssociateDetails = PMFalse
            '                Exit Function
            '            End If
            '
            '
            '    Else
            '        sSQL = "SELECT party_relationship.relation_cnt, description shortname " & _
            ''                "FROM party_relationship WHERE " & _
            ''                "party_cnt = " & CLng(vPartyCnt) & " AND " & _
            ''                "relationship_type_id = 1" & " AND " & _
            ''                "party_relationship.relation_cnt = party.party_cnt"
            '
            '            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL$, _
            ''                                            sSQLname:="GETASSOCIATEDETAILS", _
            ''                                            bStoredprocedure:=False, _
            ''                                            lNumberRecords:=500, _
            ''                                            vResultArray:=vResultArray)
            '
            '            If (m_lReturn& <> PMTrue) Then
            '                GetAssociateDetails = PMFalse
            '                Exit Function
            '            End If
            '
            '    End If

            'return the data



            vAssociates = vResultArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetAssociatesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAssociates", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAddressDetails
    '
    ' Description: Get address details for party.
    '
    ' ***************************************************************** '
    Public Function GetAddressDetails(ByRef vPartyCnt As Object, ByRef vAddresses(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        'eck100402
        Dim lAddressCnt As Integer
        Dim vResultArray(,) As Object = Nothing
        Dim vRiskGroups() As Object



        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'MSS200901 - Added switch

            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(vPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAddressesSQL, sSQLName:=ACGetAddressesName, bStoredProcedure:=ACGetAddressesStored, vResultArray:=vAddresses, lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'log if no adresses, but dont fail
            If Not Informations.IsArray(vAddresses) Then
                Return result
            End If

            For lRow As Integer = 0 To vAddresses.GetUpperBound(1)
                With m_oDatabase

                    .Parameters.Clear()


                    m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(CInt(vPartyCnt)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                    lAddressCnt = CInt(vAddresses(6, lRow))

                    m_lReturn = .Parameters.Add(sName:="address_cnt", vValue:=CStr(lAddressCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                    sSQL = "SELECT risk_group_id from Party_Address_Risk_Link " &
                           "where party_cnt = {party_cnt} and address_cnt = {address_cnt}"


                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAddressLink", bStoredProcedure:=False, vResultArray:=vResultArray)

                    If Informations.IsArray(vResultArray) Then

                        ReDim vRiskGroups(vResultArray.GetUpperBound(1))

                        For lrow2 As Integer = 0 To vRiskGroups.GetUpperBound(0)


                            vRiskGroups(lrow2) = vResultArray(0, lrow2)
                        Next lrow2


                        vAddresses(7, lRow) = vRiskGroups
                    End If
                End With
            Next lRow

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
    'Developer Guide No. 101
    Public Function GetAddressTypeLookups(ByRef vAddressTypes As Object) As Integer

        Dim result As Integer = 0
        'Developer Guide No. 71
        Dim vResultArray(,) As Object
        Dim vTabArray(,) As Object



        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'vResultArray = ""
            vResultArray = Nothing
            ReDim vTabArray(3, 0)


            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = "address_usage_type"

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = 0

            ' Get the Lookup items

            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=vTabArray, iLanguageID:=m_iLanguageID, dtEffectiveDate:=DateTime.Now, vResultArray:=vResultArray)

            'Return the address type lookups
            vAddressTypes = vResultArray

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
    '
    ' CMG 16/7/2002 Only get the client relationship types if optional parameter not supplied
    ' ***************************************************************** '
    'Developer Guide No. 101
    Public Function GetRelationshipTypeLookups(ByRef vRelationships As Object) As Integer
        Return GetRelationshipTypeLookups(vRelationships:=vRelationships, lPartyRelationshipGroup:=1)
    End Function
    Public Function GetRelationshipTypeLookups(ByRef vRelationships As Object, ByRef lPartyRelationshipGroup As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Construct the SQL
            'MKW270503 PN4227 - Updated 1.8.5 with 1.8.6 change.  Exclude Deleted records
            sSQL = "SELECT rt.relationship_type_id, " &
                   "rt.description " &
                   "FROM Relationship_Type rt, " &
                   "Party_Relationship_Group prg " &
                   "WHERE rt.party_relationship_group_id = prg.party_relationship_group_id " &
                   "AND prg.party_relationship_group_id = " & CStr(lPartyRelationshipGroup) &
                   "AND rt.is_deleted = " & CStr(gPMConstants.PMEReturnCode.PMFalse) &
                   " ORDER BY rt.code"

            ' Execute the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetDetails", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Return the relationship type lookups


            vRelationships = vResultArray

            'PB-CMG 11/07/2002 Only want client relationships from the lookup
            'Dim vResultArray As Variant
            'Dim vTabArray As Variant
            'Dim i As Integer
            '
            '    On Error GoTo Err_GetRelationshipTypeLookups
            '
            '    GetRelationshipTypeLookups = PMTrue
            '
            '    vResultArray = ""
            '    ReDim vTabArray(3, 0)
            '
            '    vTabArray(PMLookupTableName, 0) = "relationship_type"
            '    vTabArray(PMLookupKey, 0) = 0
            '
            '    ' Get the Lookup items
            '    m_lReturn& = m_oLookup.GetLookupValues( _
            ''        iLookupType:=PMLookupAll, _
            ''        vTableArray:=vTabArray, _
            ''        iLanguageID:=m_iLanguageID, _
            ''        dtEffectiveDate:=Now, _
            ''        vResultArray:=vResultArray)
            '
            '    'Return the relationship type lookups
            '    vRelationships = vResultArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetRelationshipTypeLookups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRealtionshipTypeLookups", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'EK 22/10/99
    ' ***************************************************************** '
    ' Name: GetContactTypeLookups
    '
    ' Description: Get Contact type lookups.
    '
    ' ***************************************************************** '
    'Developer Guide No. 18
    Public Function GetContactTypeLookups(ByRef vContactTypes As Object) As Integer

        Dim result As Integer = 0
        'Developer Guide No. 18
        Dim vResultArray(,) As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            vResultArray = Nothing
            Dim vTabArray(,) As Object
            ReDim vTabArray(3, 0)


            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = "Contact_type"

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = 0

            ' Get the Lookup items

            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=vTabArray, iLanguageID:=m_iLanguageID, dtEffectiveDate:=DateTime.Now, vResultArray:=vResultArray)

            'Return the Contact type lookups
            vContactTypes = vResultArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetContactTypeLookups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetContactTypeLookups", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetPreferredContact
    '
    ' Description: Gets the preferred contact method, and details for that method.
    '
    ' History: 24/07/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetPreferredContact(ByVal v_lPartyCnt As Integer, ByRef r_sContactType As String, ByRef r_sErrorMessage As String) As Integer
        Return GetPreferredContact(v_lPartyCnt:=v_lPartyCnt, r_sContactType:=r_sContactType, r_sErrorMessage:=r_sErrorMessage, r_sEmailAddress:="", r_sFaxCode:="", r_sFaxNumber:="", r_sFaxExt:="")
    End Function
    Public Function GetPreferredContact(ByVal v_lPartyCnt As Integer, ByRef r_sContactType As String, ByRef r_sErrorMessage As String, ByRef r_sEmailAddress As String, ByRef r_sFaxCode As String, ByRef r_sFaxNumber As String, ByRef r_sFaxExt As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim lContactTypeID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' No error yet
            r_sErrorMessage = ""

            ' Construct the SQL
            sSQL = "SELECT ct.code, ct.contact_type_id " &
                   "FROM Party p " &
                   "LEFT OUTER JOIN contact_type ct " &
                   "ON p.correspondence_type_id = ct.contact_type_id " &
                   "WHERE p.party_cnt = {party_cnt}"

            ' Clear the database parameters
            m_oDatabase.Parameters.Clear()

            ' Add the new one
            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Get_Correspondence", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any results?
            If Not Informations.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Party_cnt does not exist. " & Environment.NewLine & sSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="GetPreferredContact", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Return the contact type

            r_sContactType = CStr(vResultArray(0, 0)).Trim()
            ' CTAF 030800 - Changed from Clng to Val

            lContactTypeID = CInt(Val(CStr(vResultArray(1, 0))))

            ' Which type of correspondence do they prefer?

            Select Case r_sContactType
                Case "FAX" ' Fax

                    sSQL = "SELECT area_code, number, extension FROM Contact " &
                           "WHERE contact_cnt IN (SELECT contact_cnt FROM Party_Contact_Usage WHERE party_cnt = {party_cnt}) " &
                           "AND contact_type_id = {contact_type_id}"

                    m_oDatabase.Parameters.Clear()

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="contact_type_id", vValue:=CStr(lContactTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetFax", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vResultArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If Informations.IsArray(vResultArray) Then

                        ' Return the properties
                        If Not False Then

                            r_sFaxCode = CStr(vResultArray(0, 0))
                        End If

                        If Not False Then

                            r_sFaxNumber = CStr(vResultArray(1, 0))
                        End If

                        If Not False Then

                            r_sFaxExt = CStr(vResultArray(2, 0))
                        End If

                    Else

                        r_sErrorMessage = "The preferred method of communication is via FAX, but the client does not have a FAX number in contacts."
                        Return gPMConstants.PMEReturnCode.PMFalse

                    End If

                Case "E-MAIL"
                    ' Email

                    ' Get the email address
                    sSQL = "SELECT number FROM Contact " &
                           "WHERE contact_cnt IN (SELECT contact_cnt FROM Party_Contact_Usage WHERE party_cnt = {party_cnt}) " &
                           "AND contact_type_id = {contact_type_id}"

                    m_oDatabase.Parameters.Clear()

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="contact_type_id", vValue:=CStr(lContactTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetEmail", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vResultArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If Informations.IsArray(vResultArray) Then
                        ' Return the email address
                        If Not False Then

                            r_sEmailAddress = CStr(vResultArray(0, 0))
                        End If
                    Else
                        r_sErrorMessage = "The preferred method of communication is via E-Mail, but the client does not have an E-Mail address in contacts."
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case Else
                    ' Default to printing

            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPreferredContact Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPreferredContact", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: AddOtherPartyInfo
    '
    ' Parameters: r_oOtherPartyInfo and party_cnt
    '
    ' Description:
    '
    ' History:
    '           Created : VGupta : 05-10-2006 : Other party update SAM
    ' ***************************************************************** '
    Public Function AddOtherPartyInfo(ByVal r_oOtherPartyInfo As Object, ByVal r_lPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddOtherPartyInfo"



        Try


            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            AddInputParameter(v_sName:="party_cnt", v_vValue:=r_lPartyCnt, v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="license_type_id", v_vValue:=r_oOtherPartyInfo(1), v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="license_number", v_vValue:=r_oOtherPartyInfo(2), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="date_of_birth", v_vValue:=r_oOtherPartyInfo(3), v_iType:=gPMConstants.PMEDataType.PMDate)
            AddInputParameter(v_sName:="gender", v_vValue:=r_oOtherPartyInfo(4), v_iType:=gPMConstants.PMEDataType.PMString)

            AddInputParameter(v_sName:="party_status", v_vValue:=r_oOtherPartyInfo(5), v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="reference_number", v_vValue:=r_oOtherPartyInfo(6), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="external_id", v_vValue:=r_oOtherPartyInfo(7), v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="reg_number", v_vValue:=r_oOtherPartyInfo(8), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="date_passed_test", v_vValue:=r_oOtherPartyInfo(9), v_iType:=gPMConstants.PMEDataType.PMDate)
            AddInputParameter(v_sName:="contact_name", v_vValue:=r_oOtherPartyInfo(10), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="contact_telephone_number", v_vValue:=r_oOtherPartyInfo(11), v_iType:=gPMConstants.PMEDataType.PMString)

            AddInputParameter(v_sName:="insurer_name", v_vValue:=r_oOtherPartyInfo(12), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="insurer_address1", v_vValue:=r_oOtherPartyInfo(13), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="insurer_address2", v_vValue:=r_oOtherPartyInfo(14), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="insurer_address3", v_vValue:=r_oOtherPartyInfo(15), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="insurer_address4", v_vValue:=r_oOtherPartyInfo(16), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="insurer_postcode", v_vValue:=r_oOtherPartyInfo(17), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="insurer_telephone_number", v_vValue:=r_oOtherPartyInfo(18), v_iType:=gPMConstants.PMEDataType.PMString)


            AddInputParameter(v_sName:="insurer_fax_number", v_vValue:=r_oOtherPartyInfo(19), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="insurer_contact_name", v_vValue:=r_oOtherPartyInfo(20), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="insurer_email", v_vValue:=r_oOtherPartyInfo(21), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="insurer_notes", v_vValue:=r_oOtherPartyInfo(22), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="company_notes", v_vValue:=r_oOtherPartyInfo(23), v_iType:=gPMConstants.PMEDataType.PMString)

            AddInputParameter(v_sName:="active_indicator", v_vValue:=r_oOtherPartyInfo(24), v_iType:=gPMConstants.PMEDataType.PMInteger)
            AddInputParameter(v_sName:="after_hours_indicator", v_vValue:=r_oOtherPartyInfo(25), v_iType:=gPMConstants.PMEDataType.PMInteger)
            AddInputParameter(v_sName:="priority_indicator", v_vValue:=r_oOtherPartyInfo(26), v_iType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = m_oDatabase.SQLAction(sSQL:=kAddOtherPartyDetailsSQL, sSQLName:=kAddOtherPartyDetailsName, bStoredProcedure:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kAddOtherPartyDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If

            Return m_lReturn

        Catch ex As Exception


            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here



            Return result



            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: AddPartySupplier
    '
    ' Parameters: r_oPartySupplier and party_cnt
    '
    ' Description:
    '
    ' History:
    '           Created : VGupta : 06-10-2006 : Other party update SAM
    ' ***************************************************************** '
    Public Function AddPartySupplier(ByVal r_oPartySupplier As Object, ByVal r_lPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddPartySupplier"


        Try





            For cnt As Integer = 0 To r_oPartySupplier.GetUpperBound(0)

                m_oDatabase.Parameters.Clear()

                AddInputParameter(v_sName:="party_cnt", v_vValue:=r_lPartyCnt, v_iType:=gPMConstants.PMEDataType.PMLong)
                AddInputParameter(v_sName:="supplier_speciality_id", v_vValue:=r_oPartySupplier(cnt, 1), v_iType:=gPMConstants.PMEDataType.PMLong)
                AddInputParameter(v_sName:="supplier_business_id", v_vValue:=r_oPartySupplier(cnt, 2), v_iType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddPartySupplierDetailsSQL, sSQLName:=ACAddPartySupplierDetailsName, bStoredProcedure:=True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    gPMFunctions.RaiseError(kMethodName, ACAddPartySupplierDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

                End If


            Next

            Return m_lReturn

        Catch ex As Exception


            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here



            Return result



            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: AddAccidents
    '
    ' Parameters: r_oAccidents and party_cnt
    '
    ' Description:
    '
    ' History:
    '           Created : VGupta : 05-10-2006 : Other party update SAM
    ' ***************************************************************** '
    Public Function AddAccidents(ByVal r_oAccidents As Object, ByVal r_lPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddAccidents"


        Try





            For cnt As Integer = 0 To r_oAccidents.GetUpperBound(0)

                m_oDatabase.Parameters.Clear()

                AddInputParameter(v_sName:="party_cnt", v_vValue:=r_lPartyCnt, v_iType:=gPMConstants.PMEDataType.PMLong)
                AddInputParameter(v_sName:="previous_accidents_id", v_vValue:=0, v_iType:=gPMConstants.PMEDataType.PMLong)
                AddInputParameter(v_sName:="Date", v_vValue:=r_oAccidents(cnt, 1), v_iType:=gPMConstants.PMEDataType.PMDate)
                AddInputParameter(v_sName:="Description", v_vValue:=r_oAccidents(cnt, 2), v_iType:=gPMConstants.PMEDataType.PMString)
                AddInputParameter(v_sName:="is_at_fault", v_vValue:=r_oAccidents(cnt, 3), v_iType:=gPMConstants.PMEDataType.PMBoolean)

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddAccidentDetailsSQL, sSQLName:=ACAddAccidentDetailsName, bStoredProcedure:=True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    gPMFunctions.RaiseError(kMethodName, ACAddAccidentDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

                End If


            Next

            Return m_lReturn

        Catch ex As Exception


            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here



            Return result



            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: AddConvictions
    '
    ' Parameters: r_oConvictions and party_cnt
    '
    ' Description:
    '
    ' History:
    '           Created : VGupta : 05-10-2006 : Other party update SAM
    ' ***************************************************************** '
    Public Function AddConvictions(ByVal r_oConvictions As Object, ByVal r_lPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddConvictions"


        Try






            For cnt As Integer = 0 To r_oConvictions.GetUpperBound(0)
                m_oDatabase.Parameters.Clear()
                AddInputParameter(v_sName:="party_cnt", v_vValue:=r_lPartyCnt, v_iType:=gPMConstants.PMEDataType.PMLong)
                AddInputParameter(v_sName:="party_conviction_id", v_vValue:=0, v_iType:=gPMConstants.PMEDataType.PMLong)
                AddInputParameter(v_sName:="code", v_vValue:=r_oConvictions(cnt, 1), v_iType:=gPMConstants.PMEDataType.PMString)
                AddInputParameter(v_sName:="conviction_date", v_vValue:=r_oConvictions(cnt, 5), v_iType:=gPMConstants.PMEDataType.PMString)
                AddInputParameter(v_sName:="description", v_vValue:=r_oConvictions(cnt, 3), v_iType:=gPMConstants.PMEDataType.PMString)
                AddInputParameter(v_sName:="fine_amt", v_vValue:=r_oConvictions(cnt, 4), v_iType:=gPMConstants.PMEDataType.PMCurrency)
                AddInputParameter(v_sName:="sentence_code", v_vValue:=r_oConvictions(cnt, 6), v_iType:=gPMConstants.PMEDataType.PMString)
                AddInputParameter(v_sName:="sentence_description", v_vValue:=r_oConvictions(cnt, 7), v_iType:=gPMConstants.PMEDataType.PMString)
                AddInputParameter(v_sName:="sentence_duration", v_vValue:=r_oConvictions(cnt, 8), v_iType:=gPMConstants.PMEDataType.PMCurrency)
                AddInputParameter(v_sName:="sentence_duration_qualifier", v_vValue:=r_oConvictions(cnt, 9), v_iType:=gPMConstants.PMEDataType.PMString)
                AddInputParameter(v_sName:="sentence_effective_date", v_vValue:=r_oConvictions(cnt, 10), v_iType:=gPMConstants.PMEDataType.PMString)
                AddInputParameter(v_sName:="status_code", v_vValue:=r_oConvictions(cnt, 2), v_iType:=gPMConstants.PMEDataType.PMString)
                AddInputParameter(v_sName:="alcohol_level", v_vValue:=r_oConvictions(cnt, 11), v_iType:=gPMConstants.PMEDataType.PMCurrency)
                AddInputParameter(v_sName:="alcohol_measurement_method", v_vValue:=r_oConvictions(cnt, 12), v_iType:=gPMConstants.PMEDataType.PMString)
                AddInputParameter(v_sName:="driving_licence_penalty_pts", v_vValue:=r_oConvictions(cnt, 13), v_iType:=gPMConstants.PMEDataType.PMCurrency)

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddConvictionDetailsSQL, sSQLName:=ACAddConvictionDetailsName, bStoredProcedure:=True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    gPMFunctions.RaiseError(kMethodName, ACAddConvictionDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            Next

            Return m_lReturn

        Catch ex As Exception


            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here



            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetContactDetails
    '
    ' Description: Get contact details for party.
    '
    ' ***************************************************************** '
    Public Function GetContactDetails(ByRef vPartyCnt As Object, ByRef vContacts(,) As Object) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object
        Dim sSQL As String = ""
        Dim vTabArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'DC 04/08/00 Added extra check so that MAIN contact types are not included
            'Get all the contacts for the party


            sSQL = "SELECT contact.contact_cnt, area_code, number, " &
                   "extension, contact.contact_type_id, contact.description " &
                   "FROM contact, party_contact_usage, contact_type WHERE " &
                   "party_cnt = " & CStr(CInt(vPartyCnt)) & " AND " &
                   "party_contact_usage.contact_cnt = contact.contact_cnt AND " &
                   "contact.contact_type_id = contact_type.contact_type_id AND " &
                   "contact_type.code <> '" & gSIRLibrary.SIRMainContactCode & "'"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GETCONTACTS", bStoredProcedure:=False, vResultArray:=vContacts)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Now convert the contact types to descriptions, if we have contacts
            If Informations.IsArray(vContacts) Then

                ' Get the descriptions and codes for contact type

                'Developer Guide No. 71
                vResultArray = Nothing
                ReDim vTabArray(3, 0)
                For i As Integer = vContacts.GetLowerBound(1) To vContacts.GetUpperBound(1)

                    ' Setup Lookup Table Names
                    If i > 0 Then
                        ReDim Preserve vTabArray(3, i)
                    End If


                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, i) = "contact_type"


                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, i) = vContacts(4, i)

                Next i

                ' Get the Lookup items

                m_lReturn = m_oLookup.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=vTabArray, iLanguageID:=m_iLanguageID, dtEffectiveDate:=DateTime.Now, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'In the contactarray, replace the contact type id with the
                'looked up description
                For i As Integer = vContacts.GetLowerBound(1) To vContacts.GetUpperBound(1)



                    vContacts(4, i) = CStr(vResultArray(1, i)).Trim()

                Next i

            End If

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
    ' Description: Get main contact for party.
    '
    ' ***************************************************************** '
    Public Function GetMainContact(ByRef vPartyCnt As Object, ByRef lMainContactCnt As Integer, ByRef sMainContactDesc As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get all the contacts for the party

            sSQL = "SELECT pcu.contact_cnt, c.description " &
                   "FROM contact c, party_contact_usage pcu, contact_type ct " &
                   "WHERE pcu.party_cnt = " & CStr(CInt(vPartyCnt)) & " AND " &
                   "c.contact_cnt = pcu.contact_cnt AND " &
                   "c.contact_type_id = ct.contact_type_id AND " &
                   "ct.code = '" & gSIRLibrary.SIRMainContactCode & "'"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GETMAINCONTACT", bStoredProcedure:=False, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lMainContactCnt = 0
            sMainContactDesc = ""

            If (True) And (Informations.IsArray(vResultArray)) Then


                lMainContactCnt = CInt(vResultArray(0, 0))

                sMainContactDesc = CStr(vResultArray(1, 0))

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetMainContact Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNainContact", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAddressContactDetails
    '
    ' Description: Get contact details for party via address.
    '
    ' ***************************************************************** '
    Public Function GetAddressContactDetails(ByRef vPartyCnt As Object, ByRef vContacts(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get all the contacts for the party

            sSQL = "SELECT c.contact_cnt, c.area_code, c.number, " &
                   "c.extension, ct.code, aut.code " &
                   "FROM contact c, contact_type ct, contact_address_usage cau, " &
                   "party_address_usage pau, address_usage_type aut " &
                   "WHERE pau.party_cnt = " & CStr(CInt(vPartyCnt)) & " " &
                   "AND pau.address_cnt = cau.address_cnt " &
                   "AND cau.contact_cnt = c.contact_cnt " &
                   "AND pau.address_usage_type_id = aut.address_usage_type_id " &
                   "AND c.contact_type_id = ct.contact_type_id"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GETADDRESSCONTACTS", bStoredProcedure:=False, vResultArray:=vContacts)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetAddressContactDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAddressContactDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetCommissionAccounts
    '
    ' Description:
    '
    ' History: 31/01/2000 CTAF - Created.
    '          01/02/2000 CTAF - Added Party Type ID paramter
    '
    ' ***************************************************************** '
    Public Function GetCommissionAccounts(ByRef r_vResultArray(,) As Object, ByRef r_lPartyTypeID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Construct the SQL
            sSQL = "SELECT party_type_id FROM Party_Type WHERE code = 'CM'"

            ' Perform the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetPartyType", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then

                r_lPartyTypeID = CInt(vResultArray(0, 0))
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Construct the SQL
            sSQL = "SELECT P.party_cnt, P.shortname, P.name, P.party_type_id, P.is_deleted, 0, P.source_id, S.description "
            sSQL = sSQL & "FROM Party P, Source S WHERE " &
                   "S.Source_id = P.Source_id AND " &
                   "P.party_type_id = {party_type_id} ORDER BY P.shortname"

            ' Add the paramter
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_type_id", vValue:=CStr(r_lPartyTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Perform the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetCommAcc", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If no records returned then return not found
            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Return the result set

            r_vResultArray = vResultArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCommissionAccounts Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCommissionAccounts", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateAssociates
    '
    ' Description: Update the party_Associate usage table with old
    ' and new Associates for the party.
    '
    ' CMG 16/7/2002 update as client relationship types if optional parameter not supplied
    ' ***************************************************************** '

    ' DC 03/05/00
    ' Cater for more than one Associate
    'Developer Guide No. 101
    Public Function UpdateAssociates(ByRef vPartyCnt As Object, ByRef vAssociates(,) As Object, Optional ByVal sUniqueId As String = "") As Integer
        Return UpdateAssociates(vPartyCnt:=vPartyCnt, vAssociates:=vAssociates, lPartyRelationshipGroupId:=1, sUniqueId:=sUniqueId)
    End Function
    Public Function UpdateAssociates(ByRef vPartyCnt As Object, ByRef vAssociates(,) As Object, ByRef lPartyRelationshipGroupId As Object, Optional ByVal sUniqueId As String = "") As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        ' DC 03/05/00
        ' Cater for more than one Asscociate
        ' and setting up a corresponding relationship record
        Dim lOppRelTypeId As Integer
        Dim sOppRelTypeDesc As String = ""
        Dim bOppRelTypeId, bOppRelTypeDesc As Boolean
        Dim vResultArray(,) As Object = Nothing
        Dim vArray(,) As Object = Nothing
        Dim sAssociateScreenHierarchy As String = ""

        Dim sRelTypeDesc As String = "" 'DJM 08/04/2002

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = BeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Delete old Associates for party if supplied

            ' DC 03/05/00
            ' Cater for more than one Associate

            '    sSQL = "DELETE from party_relationship WHERE " & _
            ''        "relation_cnt = " & vPartyCnt & " AND " & _
            ''        "relationship_type_id = " & 1
            '
            If Informations.IsArray(vAssociates) Then
                For i As Integer = vAssociates.GetLowerBound(1) To vAssociates.GetUpperBound(1)
                    If sUniqueId <> "" Then
                        If CStr(vAssociates(0, i)) <> "" Then
                            sSQL = "SELECT shortname FROM Party WHERE party_cnt = " & CStr((vAssociates(0, i)))

                            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GETSHORTNAME", bStoredProcedure:=False, vResultArray:=vResultArray)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                m_lReturn = RollbackTrans()
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                        End If
                        sSQL = "SELECT shortname FROM Party WHERE party_cnt = " & CStr(CInt(vPartyCnt))

                        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GETSHORTNAME", bStoredProcedure:=False, vResultArray:=vArray)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = RollbackTrans()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If Informations.IsArray(vArray) AndAlso CStr(vAssociates(1, i)) <> "" Then
                            sAssociateScreenHierarchy = $"Agent({vArray(0, 0).ToString().Trim()})/AssociateCode({vAssociates(1, i).ToString().Trim()})"
                        ElseIf CStr(vResultArray(0, 0)) <> "" Then
                            sAssociateScreenHierarchy = $"Agent({vArray(0, 0).ToString().Trim()})/AssociateCode({vResultArray(0, 0).ToString().Trim()})"
                        End If

                        sSQL = "UPDATE party_relationship SET " &
                        "UserId = " & CStr(CInt(m_iUserID)) & ", " &
                        "UniqueId = '" & sUniqueId & "', " &
                        "ScreenHierarchy = '" & sAssociateScreenHierarchy & "' " &
                        "WHERE party_cnt = " & CStr(CInt(vPartyCnt)) & " AND " &
                        "relation_cnt = " & CStr((vAssociates(0, i)))

                        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="UPDATEPARTYASS", bStoredProcedure:=False)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = RollbackTrans()

                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If Informations.IsArray(vArray) AndAlso CStr(vAssociates(1, i)) <> "" Then
                            sAssociateScreenHierarchy = $"Agent({vAssociates(1, i).ToString().Trim()})/AssociateCode({vArray(0, 0).ToString().Trim()})"
                        ElseIf CStr(vResultArray(0, 0)) <> "" Then
                            sAssociateScreenHierarchy = $"Agent({vResultArray(0, 0).ToString().Trim()})/AssociateCode({vArray(0, 0).ToString().Trim()})"
                        End If

                        sSQL = "UPDATE party_relationship SET " &
                        "UserId = " & CStr(CInt(m_iUserID)) & ", " &
                        "UniqueId = '" & sUniqueId & "', " &
                        "ScreenHierarchy = '" & sAssociateScreenHierarchy & "' " &
                        "WHERE party_cnt = " & CStr((vAssociates(0, i))) & " AND " &
                        "relation_cnt = " & CStr(CInt(vPartyCnt))

                        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="UPDATEPARTYASS", bStoredProcedure:=False)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = RollbackTrans()

                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                Next
            End If

            vResultArray = Nothing

            sSQL = "DELETE from party_relationship WHERE " &
                   "party_cnt = " & CStr(CInt(vPartyCnt))

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="DELPARTYASS", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            sSQL = "DELETE from party_relationship WHERE " &
                   "relation_cnt = " & CStr(CInt(vPartyCnt))

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="DELRELATIONASS", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' DC 03/05/00
            ' Cater for more than one Associate

            '    If (vAssociatedCnt <> 0) Then
            '        sSQL = "INSERT INTO party_relationship " & _
            ''            "(party_cnt, " & _
            ''            "relation_cnt, " & _
            ''            "relationship_type_id, " & _
            ''            "description) " & _
            ''            "VALUES (" & _
            ''            CStr(vAssociatedCnt) & ", " & _
            ''            CStr(vPartyCnt) & ", " & _
            ''            CStr(1) & ", '" & _
            ''            CStr(vAssociateDescription) & "')"
            '
            '
            '        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, _
            ''                                        sSQLname:="INSASSOCIATE", _
            ''                                        bStoredprocedure:=False)
            '
            '        If (m_lReturn& <> PMTrue) Then
            '            m_lReturn& = RollbackTrans()
            '
            '            UpdateAssociates = PMFalse
            '            Exit Function
            '        End If
            '    End If

            'Add Associate Details for the Party passed
            If (True) And (Informations.IsArray(vAssociates)) Then
                ' Use SqlBulkCopy for performance with large datasets
                Dim dtPartyRel As New DataTable()
                dtPartyRel.Columns.Add("party_cnt", GetType(Integer))
                dtPartyRel.Columns.Add("relation_cnt", GetType(Integer))
                dtPartyRel.Columns.Add("relationship_type_id", GetType(Integer))
                dtPartyRel.Columns.Add("description", GetType(String))
                dtPartyRel.Columns.Add("commission_transaction", GetType(Integer))
                dtPartyRel.Columns.Add("UserId", GetType(Integer))
                dtPartyRel.Columns.Add("UniqueId", GetType(String))
                dtPartyRel.Columns.Add("ScreenHierarchy", GetType(String))

                Dim commTrans As Integer

                For i As Integer = vAssociates.GetLowerBound(1) To vAssociates.GetUpperBound(1)
                    If CStr(vAssociates(1, i)) <> "" Then
                        'DJM 08/04/2002 : Double all apostrophes so that they work when
                        '                 in an SQL Statement.
                        sRelTypeDesc = CStr(vAssociates(4, i))

                        commTrans = If(gPMFunctions.ToSafeString(vAssociates(5, i)).ToUpper() = "TRUE", 1, 0)
                        ' CMG 16/7/2002 update as client relationship types
                        ' if optional parameter not supplied

                        If Informations.IsArray(vArray) Then
                            sAssociateScreenHierarchy = $"Agent({vArray(0, 0).ToString().Trim()})/AssociateCode({vAssociates(1, i).ToString().Trim()})"
                        End If

                        dtPartyRel.Rows.Add(CInt(vPartyCnt), CInt(vAssociates(0, i)), CInt(vAssociates(3, i)),
                                           sRelTypeDesc, commTrans, m_iUserID, sUniqueId, sAssociateScreenHierarchy)

                        ' Get opposite relationship for complementary record
                        bOppRelTypeId = gPMConstants.PMEReturnCode.PMFalse
                        bOppRelTypeDesc = gPMConstants.PMEReturnCode.PMFalse
                        ' get opposite relationship type id

                        sSQL = "SELECT complementary_type_id " &
                               "FROM relationship_type " &
                               "WHERE relationship_type_id = " &
                               CStr(vAssociates(3, i)) &
                               " AND is_deleted = 0" &
                               " AND effective_date <= {effective_date}"

                        m_oDatabase.Parameters.Clear()
                        'Developer Guide No. 40
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTime.Today, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GETOPPASSID", bStoredProcedure:=False, vResultArray:=vResultArray)

                        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue AndAlso Informations.IsArray(vResultArray) Then
                            Dim auxVar As Object = vResultArray(0, 0)
                            If Not (Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar)) AndAlso Val(CStr(vResultArray(0, 0))) > 0 Then
                                lOppRelTypeId = CInt(vResultArray(0, 0))
                                bOppRelTypeId = gPMConstants.PMEReturnCode.PMTrue
                            End If
                        End If

                        If bOppRelTypeId Then
                            sSQL = "SELECT description FROM relationship_type WHERE relationship_type_id = " & lOppRelTypeId
                            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GETOPPASSDESC", bStoredProcedure:=False, vResultArray:=vResultArray)
                            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue AndAlso Informations.IsArray(vResultArray) Then
                                sOppRelTypeDesc = CStr(vResultArray(0, 0))
                                bOppRelTypeDesc = gPMConstants.PMEReturnCode.PMTrue
                            End If
                        End If

                        If Not bOppRelTypeId Then
                            lOppRelTypeId = CInt(vAssociates(3, i))
                        End If
                        If Not bOppRelTypeDesc Then
                            sOppRelTypeDesc = sRelTypeDesc
                        End If

                        If Informations.IsArray(vArray) Then
                            sAssociateScreenHierarchy = $"Agent({vAssociates(1, i).ToString().Trim()})/AssociateCode({vArray(0, 0).ToString().Trim()})"
                        End If

                        dtPartyRel.Rows.Add(CInt(vAssociates(0, i)), CInt(vPartyCnt), lOppRelTypeId,
                                           sOppRelTypeDesc, commTrans, m_iUserID, sUniqueId, sAssociateScreenHierarchy)

                    End If
                Next i

                ' Bulk insert using batched INSERT statements with duplicate check
                If dtPartyRel.Rows.Count > 0 Then
                    Const BATCH_SIZE As Integer = 500
                    Dim batchCount As Integer = 0
                    Dim sqlBatch As New System.Text.StringBuilder()

                    For Each row As DataRow In dtPartyRel.Rows
                        If batchCount = 0 Then
                            sqlBatch.Clear()
                            sqlBatch.Append("INSERT INTO party_relationship (party_cnt, relation_cnt, relationship_type_id, description, commission_transaction, UserId, UniqueId, ScreenHierarchy) ")
                            sqlBatch.Append("SELECT party_cnt, relation_cnt, relationship_type_id, description, commission_transaction, UserId, UniqueId, ScreenHierarchy FROM (VALUES ")
                        End If

                        If batchCount > 0 Then sqlBatch.Append(", ")

                        sqlBatch.AppendFormat("({0}, {1}, {2}, '{3}', {4}, {5}, '{6}', '{7}')",
                            row("party_cnt"), row("relation_cnt"), row("relationship_type_id"),
                            CStr(row("description")).Replace("'", "''"), row("commission_transaction"),
                            row("UserId"), CStr(row("UniqueId")).Replace("'", "''"),
                            CStr(row("ScreenHierarchy")).Replace("'", "''"))

                        batchCount += 1

                        If batchCount >= BATCH_SIZE Then
                            sqlBatch.Append(") AS v(party_cnt, relation_cnt, relationship_type_id, description, commission_transaction, UserId, UniqueId, ScreenHierarchy) ")
                            sqlBatch.Append("WHERE NOT EXISTS (SELECT 1 FROM party_relationship WHERE party_relationship.party_cnt = v.party_cnt AND party_relationship.relation_cnt = v.relation_cnt)")
                            m_lReturn = m_oDatabase.SQLAction(sSQL:=sqlBatch.ToString(), sSQLName:="INSASSOCIATE_BATCH", bStoredProcedure:=False)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                m_lReturn = RollbackTrans()
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            batchCount = 0
                        End If
                    Next

                    ' Execute remaining batch
                    If batchCount > 0 Then
                        sqlBatch.Append(") AS v(party_cnt, relation_cnt, relationship_type_id, description, commission_transaction, UserId, UniqueId, ScreenHierarchy) ")
                        sqlBatch.Append("WHERE NOT EXISTS (SELECT 1 FROM party_relationship WHERE party_relationship.party_cnt = v.party_cnt AND party_relationship.relation_cnt = v.relation_cnt)")
                        m_lReturn = m_oDatabase.SQLAction(sSQL:=sqlBatch.ToString(), sSQLName:="INSASSOCIATE_BATCH", bStoredProcedure:=False)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = RollbackTrans()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                End If
            End If

            '    'Delete old Associates for party if supplied
            '    If vIsAssociate = 1 Then
            '        sSQL = "DELETE from party_relationship WHERE " & _
            ''            "relation_cnt = " & vPartyCnt & " AND " & _
            ''            "relationship_type_id = " & 1
            '
            '        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, _
            ''                                        sSQLname:="DELPARTYCONS", _
            ''                                        bStoredprocedure:=False)
            '
            '        If (m_lReturn& <> PMTrue) Then
            '            m_lReturn& = RollbackTrans()
            '
            '            UpdateAssociates = PMFalse
            '            Exit Function
            '        End If
            '        If (vAssociatedCnt <> 0) Then
            '            sSQL = "INSERT INTO party_relationship " & _
            ''                "(party_cnt, " & _
            ''                "relation_cnt, " & _
            ''                "relationship_type_id, " & _
            ''                "description) " & _
            ''                "VALUES (" & _
            ''                CStr(vAssociatedCnt) & ", " & _
            ''                CStr(vPartyCnt) & ", " & _
            ''                CStr(1) & ", '" & _
            ''                CStr(vAssociateDescription) & "')"
            '
            '
            '            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, _
            ''                                            sSQLname:="INSASSOCIATE", _
            ''                                            bStoredprocedure:=False)
            '
            '            If (m_lReturn& <> PMTrue) Then
            '                m_lReturn& = RollbackTrans()
            '
            '                UpdateAssociates = PMFalse
            '                Exit Function
            '            End If
            '        End If
            '     Else
            '        sSQL = "DELETE from party_relationship WHERE " & _
            ''            "relation_cnt = " & vPartyCnt & " AND " & _
            ''            "relationship_type_id = " & 1
            '
            '        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, _
            ''                                        sSQLname:="DELPARTYCONS", _
            ''                                        bStoredprocedure:=False)
            '
            '        If (m_lReturn& <> PMTrue) Then
            '            m_lReturn& = RollbackTrans()
            '
            '            UpdateAssociates = PMFalse
            '            Exit Function
            '        End If
            '        If (vPartyCnt <> 0) Then
            '            sSQL = "INSERT INTO party_relationship WHERE " & _
            ''                "(party_cnt, " & _
            ''                "relation_cnt,  " & _
            ''                "relationship_type_id, " & _
            ''                "description) " & _
            ''                "VALUES (" & _
            ''                vPartyCnt & "," & _
            ''                vAssociatedCnt & "," & _
            ''                1 & ", '" & _
            ''                vAssociateDescription & "')"
            '
            '
            '            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, _
            ''                                            sSQLname:="INSASSOCIATE", _
            ''                                            bStoredprocedure:=False)
            '
            '            If (m_lReturn& <> PMTrue) Then
            '                m_lReturn& = RollbackTrans()
            '
            '                UpdateAssociates = PMFalse
            '                Exit Function
            '            End If
            '        End If
            '     End If

            m_lReturn = CommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = RollbackTrans()

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateAssociatesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAssociates", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateAddresses
    '
    ' Description: Update the party_address usage table with old
    ' and new addresses for the party.
    '
    ' Edit History  :
    ' RAM20020610   : Changed the Checking of vAddAddresses(0, i)) <> 1 to
    '                   vAddAddresses(0, i)) <> 0
    '                 Desc : If we have a blank database, then the existing code
    '                        will not update the PartyAddressUsage table for the
    '                        Address_Cnt = 1. (Ref. SBO 1.8 Bug No. 21)
    ' ***************************************************************** '
    'Developer Guide No. 71

    Public Function UpdateAddresses(ByRef vPartyCnt As Object, ByRef vAddAddresses(,) As Object) As Integer
        Return UpdateAddresses(vPartyCnt:=vPartyCnt, vAddAddresses:=vAddAddresses, vDeleteAddresses:=Nothing)
    End Function
    Public Function UpdateAddresses(ByRef vPartyCnt As Object, ByRef vAddAddresses(,) As Object, ByRef vDeleteAddresses(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        'eck100402


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = BeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Delete old addresses for party if supplied

            If Not Informations.IsNothing(vDeleteAddresses) Then

                For i As Integer = vDeleteAddresses.GetLowerBound(1) To vDeleteAddresses.GetUpperBound(1)


                    If CInt(vDeleteAddresses(0, i)) <> 0 Then




                        sSQL = "DELETE from party_address_usage WHERE " &
                               "address_cnt = " & CStr(vDeleteAddresses(0, i)) & " AND " &
                               "party_cnt = " & CStr(vPartyCnt) & " AND " &
                               "address_usage_type_id = " & CStr(vDeleteAddresses(1, i))

                        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="DELADDPARTY", bStoredProcedure:=False)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = RollbackTrans()

                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        'eck100402


                        sSQL = "DELETE from party_address_risk_link WHERE " &
                               "address_cnt = " & CStr(vDeleteAddresses(0, i)) & " AND " &
                               "party_cnt = " & CStr(vPartyCnt)

                        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="DELADDPARTYADDRESSRISK", bStoredProcedure:=False)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = RollbackTrans()

                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        'eck100402end
                    End If
                Next i
            End If

            'Add new addresses for party if supplied

            If Not Informations.IsNothing(vAddAddresses) Then

                For i As Integer = vAddAddresses.GetLowerBound(1) To vAddAddresses.GetUpperBound(1)

                    ' RAM20020610 : Make sure we are adding values to PartyAddressUsage table for
                    '               the Address_cnt = 1 too (Ref. SBO 1.8 Bug No : 21)

                    'If (CLng(vAddAddresses(0, i)) <> 1) Then     ' RAM20020610 : Changed <> 1 to <> 0


                    If CInt(vAddAddresses(0, i)) <> 0 Then
                        'eck081101
                        'eck100402 Don't bother with riskId as it is now an array
                        '                If UBound(vAddAddresses, 1) = 2 Then

                        '                    If vAddAddresses(2, i) = "" Then
                        '                        vAddAddresses(2, i) = 0
                        '                    End If
                        '                     sSQL = "INSERT INTO party_address_usage " & _
                        ''                        "(address_cnt, " & _
                        ''                        "party_cnt, " & _
                        ''                        "address_usage_type_id, " & _
                        ''                        "risk_group_id) " & _
                        ''                        "VALUES (" & _
                        ''                        CStr(vAddAddresses(0, i)) & ", " & _
                        ''                        CStr(vPartyCnt) & ", " & _
                        ''                        CStr(vAddAddresses(1, i)) & ", " & _
                        ''                        CStr(vAddAddresses(2, i)) & ")"
                        '                Else



                        sSQL = "INSERT INTO party_address_usage " &
                               "(address_cnt, " &
                               "party_cnt, " &
                               "address_usage_type_id, " &
                               "risk_group_id) " &
                               "VALUES (" &
                               CStr(vAddAddresses(0, i)) & ", " &
                               CStr(vPartyCnt) & ", " &
                               CStr(vAddAddresses(1, i)) & ", " &
                               0 & ")"
                        '               End If


                        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="ADDADDPARTY", bStoredProcedure:=False)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = RollbackTrans()

                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        'eck100402 Do  risk group array
                        If vAddAddresses.GetUpperBound(0) = 2 Then
                            If Informations.IsArray(vAddAddresses(2, i)) Then

                                For i2 As Integer = 0 To vAddAddresses(2, i).GetUpperBound(0)




                                    sSQL = "INSERT INTO party_address_risk_link " &
                                           "(party_cnt, " &
                                           "address_cnt, " &
                                           "risk_group_id) " &
                                           "VALUES (" &
                                           CStr(vPartyCnt) & ", " &
                                           CStr(vAddAddresses(0, i)) & ", " &
                                           CStr(vAddAddresses(2, i)(i2)) & ") "
                                    m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="ADDADDPARTYRISKLINK", bStoredProcedure:=False)

                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        m_lReturn = RollbackTrans()

                                        Return gPMConstants.PMEReturnCode.PMFalse
                                    End If
                                Next i2
                            End If
                        End If
                        'eck100402End
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
    'Developer Guide No. 101
    Public Function UpdateContacts(ByRef vPartyCnt As Object, ByRef vAddContacts As Object, ByRef vDeleteContacts As Object) As Integer
        Return UpdateContacts(vPartyCnt:=vPartyCnt, vAddContacts:=vAddContacts, vDeleteContacts:=vDeleteContacts, bAddContacts:=True)
    End Function
    Public Function UpdateContacts(ByRef vPartyCnt As Object, ByRef vAddContacts As Object) As Integer
        Return UpdateContacts(vPartyCnt:=vPartyCnt, vAddContacts:=vAddContacts, vDeleteContacts:=Nothing, bAddContacts:=True)
    End Function
    Public Function UpdateContacts(ByRef vPartyCnt As Object, ByRef vAddContacts As Object, ByRef vDeleteContacts As Object, ByVal bAddContacts As Boolean) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = BeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Delete old contacts for party if supplied

            If Not Informations.IsNothing(vDeleteContacts) Then

                For i As Integer = vDeleteContacts.GetLowerBound(0) To vDeleteContacts.GetUpperBound(0)


                    If CInt(vDeleteContacts(i)) <> 0 Then

                        sSQL = "UPDATE contact SET screenHierarchy = " &
                               "STUFF(screenHierarchy, " &
                               "       CHARINDEX('(', screenHierarchy) + 1, " &
                               "       CHARINDEX(')', screenHierarchy) - CHARINDEX('(', screenHierarchy) - 1, " &
                               "       (SELECT LTRIM(RTRIM(shortname)) FROM party WHERE party_cnt = " & CStr(vPartyCnt) & ")) " &
                               "WHERE contact_cnt = " & CStr(vDeleteContacts(i))

                        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="UPDATEPARTYCONS", bStoredProcedure:=False)

                        sSQL = "DELETE from party_contact_usage WHERE " &
                               "contact_cnt = " & CStr(vDeleteContacts(i)) & " AND " &
                               "party_cnt = " & CStr(vPartyCnt)

                        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="DELPARTYCONS", bStoredProcedure:=False)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = RollbackTrans()

                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                Next i
            End If

            'Add new contacts for address if supplied

            If (Not Informations.IsNothing(vAddContacts)) And bAddContacts Then

                For i As Integer = vAddContacts.GetLowerBound(0) To vAddContacts.GetUpperBound(0)


                    If CInt(vAddContacts(i)) <> 0 Then



                        sSQL = "INSERT INTO party_contact_usage " &
                               "(contact_cnt, party_cnt) VALUES " &
                               "(" & CStr(vAddContacts(i)) & ", " &
                               CStr(vPartyCnt) & ")"

                        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="ADDPARTYCONS", bStoredProcedure:=False)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateContactsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateContacts", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DC 04/08/00
    ' ***************************************************************** '
    ' Name: UpdateMainContact
    '
    ' Description: Update the party_contact usage table and contact table
    '               with new main contact for party
    '
    ' DJM 24/05/2002 : Changed entire function to update correctly, especially
    '                  in the case of two companies with the same named Main Contact.
    '
    ' ***************************************************************** '
    Public Function UpdateMainContact(ByRef vPartyCnt As Object, ByRef lMainContactCnt As Integer, ByRef sMainContactDesc As String) As Integer

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sSQL As String = ""
        Dim iMainContactType As Integer
        Dim vResultArray(,) As Object = Nothing
        Dim bAddRecord As Boolean
        Dim lSourceID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = BeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'If main contact parameters are blank, delete existing main contact record.
            If lMainContactCnt <> 0 And sMainContactDesc = "" Then

                'Delete record from Party Contact Usage table
                m_oDatabase.Parameters.Clear()


                m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(CInt(vPartyCnt)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="contact_cnt", vValue:=CStr(lMainContactCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDelPartyUsageSQL, sSQLName:=ACDelPartyUsageName, bStoredProcedure:=ACDelPartyUsageStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Delete record from Contact table
                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="contact_cnt", vValue:=CStr(lMainContactCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDelContactSQL, sSQLName:=ACDelContactName, bStoredProcedure:=ACDelContactStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            If sMainContactDesc <> "" Then

                'Are we adding or updating a main contact
                bAddRecord = lMainContactCnt = 0

                'Get the main contact type
                sSQL = "SELECT contact_type_id " &
                       "FROM contact_type " &
                       "WHERE code = '" & gSIRLibrary.SIRMainContactCode & "' AND " &
                       "is_deleted = 0 AND " &
                       "effective_date <= {effective_date}"

                m_oDatabase.Parameters.Clear()
                'Developer Guide No. 40
                m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTime.Today, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GETMAINCONTACTTYPE", bStoredProcedure:=False, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                iMainContactType = 0

                If (True) And (Informations.IsArray(vResultArray)) Then

                    iMainContactType = CInt(vResultArray(0, 0))
                End If


                'Add/Update record within contact table
                m_oDatabase.Parameters.Clear()

                'MKW300603 PN4993 Start Retrieve SourceID for previously saved record
                lSourceID = 0
                If Not bAddRecord Then

                    sSQL = "select source_id from contact " &
                           "where contact_cnt={contact_cnt}"

                    m_oDatabase.Parameters.Clear()

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="contact_cnt", vValue:=CStr(lMainContactCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GETINITIALSOURCEID", bStoredProcedure:=False, vResultArray:=vResultArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If (True) And (Informations.IsArray(vResultArray)) Then

                        lSourceID = CInt(vResultArray(0, 0))
                    End If
                End If

                m_oDatabase.Parameters.Clear()
                'MKW300603 PN4993 End


                'CMG/PB Bug 582, this parameter isnt in the stored proc
                'm_lReturn = m_oDatabase.Parameters.Add(sName:="tablename", _
                'vValue:="contact", _
                'idirection:=PMParamInput, _
                'iDataType:=PMString)
                'If (m_lReturn& <> PMTrue) Then
                '   m_lReturn& = RollbackTrans()
                '  UpdateMainContact = PMFalse
                ' Exit Function
                'End If
                'End CMG

                If bAddRecord Then
                    'We want this returned
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="contact_cnt", vValue:=CStr(lMainContactCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Else
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="contact_cnt", vValue:=CStr(lMainContactCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                End If
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="contact_type_id", vValue:=CStr(iMainContactType), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'MKW300603 PN4993 START - Use initial SourceID
                If bAddRecord Or lSourceID = 0 Then
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Else
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(lSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
                'MKW300603 PN4993 END

                'CMG/PB 04092002 Bug 185 dont set the contact id back to 0
                If bAddRecord Then
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="contact_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Else
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="contact_id", vValue:=CStr(lMainContactCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                End If
                'End CMG
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="country_id", vValue:=CStr(m_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=sMainContactDesc, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="area_code", vValue:="", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="number", vValue:="", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="extension", vValue:="", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="created_by_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'Developer Guide No. 40
                m_lReturn = m_oDatabase.Parameters.Add(sName:="date_created", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="modified_by_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'Developer Guide No. 40
                m_lReturn = m_oDatabase.Parameters.Add(sName:="last_modified", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If bAddRecord Then
                    'Add and return contact_cnt
                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddContactSQL, sSQLName:=ACAddContactName, bStoredProcedure:=ACAddContactStored)
                Else
                    'Update
                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdContactSQL, sSQLName:=ACUpdContactName, bStoredProcedure:=ACUpdContactStored)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'If we are adding a record then add an entry into the Party_Contact_Usage table
                If bAddRecord Then

                    'Get contact_cnt just created
                    lMainContactCnt = m_oDatabase.Parameters.Item("contact_cnt").Value

                    'Add record within party_contact_usage table
                    m_oDatabase.Parameters.Clear()


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(CInt(vPartyCnt)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="contact_cnt", vValue:=CStr(lMainContactCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    'Developer Guide No.86
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddPartyUsageSQL, sSQLName:=ACAddPartyUsageName, bStoredProcedure:=ACAddPartyUsageStored)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If

            'All done. Commit the changes.
            m_lReturn = CommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = RollbackTrans()

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateMainContactFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateMainContact", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateGemini
    '
    ' Description: This function first checks if Gemini installed.
    ' If so it calls the appropriate link to update with the party
    ' details.
    '
    ' ***************************************************************** '
    Public Function UpdateGemini(ByRef vPartyCnt As Object, ByRef vTask As gPMConstants.PMEComponentAction) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim bGeminiInstalled As Boolean
        Dim oLink As Object = Nothing
        Dim lReturnValue As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Check if Gemini on system

            m_lReturn = gPMComponentServices.CheckPMProductInstalled(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFGemini, r_bInstalled:=bGeminiInstalled)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not bGeminiInstalled Then
                'Fine, nothing to do
                Return result
            End If

            'We need to update Gemini, so get the source_id and party_id

            sSQL = "SELECT source_id, party_id FROM party WHERE party_cnt = " & CStr(vPartyCnt)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GETIDS", bStoredProcedure:=False, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                'should have a party
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get party details", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateGemini")
                Return result
            End If

            'Create the link object

            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oLink, v_sClassName:="bSIRToGEMParty.Business", v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_oDatabase:=m_oDatabase)



            'Set the properties to identify the party


            oLink.SourceID = vResultArray(0, 0)


            oLink.PartyID = vResultArray(1, 0)


            Select Case vTask
                Case gPMConstants.PMEComponentAction.PMEdit
                    'create the party in Gemini

                    m_lReturn = oLink.UpdateParty(lReturnValue:=ToSafeInteger(lReturnValue))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        result = gPMConstants.PMEReturnCode.PMFalse

                        oLink.Dispose()
                        oLink = Nothing
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Party in Gemini. Link Error Code = " & lReturnValue, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateGemini")
                        Return result
                    End If

                Case gPMConstants.PMEComponentAction.PMAdd


                    m_lReturn = oLink.CreateParty(lReturnValue:=ToSafeInteger(lReturnValue))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        result = gPMConstants.PMEReturnCode.PMFalse

                        oLink.Dispose()
                        oLink = Nothing
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Create Party in Gemini. Link Error Code = " & lReturnValue, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateGemini")
                        Return result

                    End If

                Case Else

                    result = gPMConstants.PMEReturnCode.PMFalse

                    oLink.Dispose()
                    oLink = Nothing
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unknown task value = " & vTask, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateGemini")
                    Return result

            End Select


            oLink.Dispose()

            oLink = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateGeminiFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateGemini", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateSwiftPartyID
    '
    ' Description: Easily updates the Swift_Party_ID without having
    '               to go throught the EditUpdate of the business object
    '
    ' History: 04/01/2002 MSS - Created.
    '
    ' ***************************************************************** '

    Public Function UpdateSwiftPartyID(ByVal lPartyCnt As Integer, ByVal lSwiftPartyID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "Update Party Set Swift_Party_ID = " & lSwiftPartyID & " "
            sSQL = sSQL & "Where Party_Cnt = " & CStr(lPartyCnt)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="UpdateSwiftPartyID", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="UpdateSwiftPartyID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateSwiftPartyID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'FSA Phase III
    ' ***************************************************************** '
    '
    ' Name: UpdateTOBLetter
    '
    ' Description: Easily updates the FSA Terms of Business Letter without having
    '               to go throught the EditUpdate of the business object
    '
    ' History: 03/11/2004 ECK - Created.
    '
    ' ***************************************************************** '

    Public Function UpdateTobLetter(ByVal lPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()
                'Developer Guide No. 40
                .Parameters.Add("Today", DateTime.Today, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
                .Parameters.Add("Party_cnt", CStr(lPartyCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)


                m_lReturn = .SQLAction(sSQL:=ACUpdateTobLetterSQL, sSQLName:=ACUpdateTobLetterName, bStoredProcedure:=ACUpdateTobLetterStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


            End With



            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="UpdateTOBLetter Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateTOBLetter", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    'DC 28/06/00 Added Correspondence Type Id
    'UPGRADE_NOTE: (7001) The following declaration (CheckMandatory) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CheckMandatory(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyTypeID As Object = Nothing, Optional ByRef vPartyStructureID As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vIsAlsoAgent As Object = Nothing, Optional ByRef vPartyID As Object = Nothing, Optional ByRef vShortname As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vResolvedName As Object = Nothing, Optional ByRef vCurrencyId As Object = Nothing, Optional ByRef vLanguageID As Object = Nothing, Optional ByRef vCollectTypeID As Object = Nothing, Optional ByRef vAccumTreatmentTypeID As Object = Nothing, Optional ByRef vStatsTreatmentTypeID As Object = Nothing, Optional ByRef vPartyCategoryID As Object = Nothing, Optional ByRef vAgentCnt As Object = Nothing, Optional ByRef vConsultantCnt As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vPaymentMethodCode As Object = Nothing, Optional ByRef vPaymentTermCode As Object = Nothing, Optional ByRef vCreditCardCode As Object = Nothing, Optional ByRef vFileCode As Object = Nothing, Optional ByRef vABCCount As Object = Nothing, Optional ByRef vStatements As Object = Nothing, Optional ByRef vReminderTypeId As Object = Nothing, Optional ByRef vRenewals As Object = Nothing, Optional ByRef vStatus As Object = Nothing, Optional ByRef vLAstActionType As Object = Nothing, Optional ByRef vIsTravelAgent As Object = Nothing, Optional ByRef vIsProspect As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vABICodeOn406 As Object = Nothing, Optional ByRef vABICodeOn81 As Object = Nothing, Optional ByRef vABICodeList As Object = Nothing, Optional ByRef vAreaId As Object = Nothing, Optional ByRef vServiceLevelId As Object = Nothing, Optional ByRef vInvariantKey As Object = Nothing, Optional ByRef vRecordStatus As Object = Nothing, Optional ByRef vCCJs As Object = Nothing, Optional ByRef vUserDefinedDataId As Object = Nothing, Optional ByRef vSeasonalGiftID As Object = Nothing, Optional ByRef vCorrespondenceTypeId As Object = Nothing, Optional ByRef vRenewalStopCodeId As Object = Nothing, Optional ByRef vSwiftPartyId As Object = Nothing, Optional ByRef OverrideCommission As Object = Nothing) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' {* USER DEFINED CODE (Begin) *}
    '


    'If (Informations.IsNothing(vPartyTypeID)) Or (Object.Equals(vPartyTypeID, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '


    'If (Informations.IsNothing(vPartyStructureID)) Or (Object.Equals(vPartyStructureID, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '


    'If (Informations.IsNothing(vSourceID)) Or (Object.Equals(vSourceID, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '


    'If (Informations.IsNothing(vPartyID)) Or (Object.Equals(vPartyID, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '


    'If (Informations.IsNothing(vShortname)) Or (Object.Equals(vShortname, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '


    'If (Informations.IsNothing(vName)) Or (Object.Equals(vName, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '


    'If (Informations.IsNothing(vCurrencyId)) Or (Object.Equals(vCurrencyId, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '


    'If (Informations.IsNothing(vLanguageID)) Or (Object.Equals(vLanguageID, Nothing)) Then
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


    'Added 990930 MK
    ' ***************************************************************** '
    ' Name: GetPartyType
    '
    ' Description: Get party type for a given party_type_id(ie shortname)
    '
    ' ***************************************************************** '
    Public Function GetPartyType(ByRef vPartyTypeID As Object, ByRef vPartyTypeCode As String) As Integer
        Return GetPartyType(vPartyTypeID:=vPartyTypeID, vPartyTypeCode:=vPartyTypeCode, vPartyType:="")
    End Function

    Public Function GetPartyType(ByRef vPartyTypeID As Object, ByRef vPartyType As String, ByRef vPartyTypeCode As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            If (Not Informations.IsNothing(vPartyTypeID)) And (Not Object.Equals(vPartyTypeID, Nothing)) Then

                'Get form DB

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPartyTypeSQL & CStr(vPartyTypeID), sSQLName:=ACGetPartyTypeName, bStoredProcedure:=ACGetPartyTypeStored, lNumberRecords:=1, vResultArray:=vResultArray)
                ' "'" & & "'"
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'return the data
                If Not Informations.IsArray(vResultArray) Then
                    vPartyType = CStr(0)
                    vPartyTypeCode = CStr(0)
                Else

                    vPartyType = CStr(vResultArray(0, 0)).Trim()

                    vPartyTypeCode = CStr(vResultArray(1, 0)).Trim()
                End If

            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetPartyType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'Added 991005 MK
    ' ***************************************************************** '
    ' Name: GetFeeDetails
    '
    ' Description: Get party fee details for a given party_cnt
    '
    ' ***************************************************************** '
    'Developer Guide No. 15
    Public Function GetFeeDetails(ByRef vPartyCnt As Object, ByRef vFeeDetails As Object) As Integer

        Dim result As Integer = 0


        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            If (Not Informations.IsNothing(vPartyCnt)) And (Not Object.Equals(vPartyCnt, Nothing)) Then

                m_oDatabase.Parameters.Clear()
                m_lReturn = m_oDatabase.Parameters.Add(sName:="language_id", vValue:=CStr(m_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(vPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Get form DB

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetFeeDetailsSQL, sSQLName:=ACGetFeeDetailsName, bStoredProcedure:=ACGetFeeDetailsStored, lNumberRecords:=0, vResultArray:=vFeeDetails)

                ' "'" & & "'"
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    vFeeDetails = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If
                'return the data
                If Not Informations.IsArray(vFeeDetails) Then
                    result = gPMConstants.PMEReturnCode.PMNotFound
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetFeeDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFeeDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ConvertParty
    '
    ' Description: Converts a party from one type to another
    '
    ' History: 22/08/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function ConvertParty(ByVal v_lPartyCnt As Integer, ByVal v_lPartyTypeOldID As Integer, ByVal v_lPartyTypeNewID As Integer, ByVal v_sPartyTypeOld As String, ByVal v_sPartyTypeNew As String) As Integer

        Dim result As Integer = 0
        Dim oEvent As bSIREvent.Business
        Dim oLock As bPMLock.User
        Dim sDescription As String = ""
        Dim lGplId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            oLock = New bPMLock.User
            m_lReturn = oLock.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bPMLock.User", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Remove Component Services

            ' Lock the party
            ' This is unlocked by user control later on

            m_lReturn = oLock.LockKey(sKeyName:="party_cnt", vKeyValue:=v_lPartyCnt, iUserID:=m_iUserID, sCurrentlyLockedBy:=m_sUsername)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lock party : " & v_lPartyCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                ' Remove the instance of lock

                oLock.Dispose()
                oLock = Nothing
                Return result
            End If

            ' Remove the instance of lock

            oLock.Dispose()
            oLock = Nothing

            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            ' Add the new ones
            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_type_id", vValue:=CStr(v_lPartyTypeOldID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="new_party_type_id", vValue:=CStr(v_lPartyTypeNewID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the stored procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACConvertSQL, sSQLName:=ACConvertSQLName, bStoredProcedure:=ACConvertSQLStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed on SQL : " & ACConvertSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' get the gis_policy_link_id for this client
            m_lReturn = GetGISPolicyLinkForParty(lPartyCnt:=v_lPartyCnt, r_lGISPolicyLinkID:=lGplId)

            ' delete the data from the datamodel
            With m_oDatabase
                ' Clear the parameters
                .Parameters.Clear()

                ' Add the new ones
                m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_policy_link_id", vValue:=CStr(lGplId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Call the stored procedure
                m_lReturn = .SQLAction(sSQL:=ACDelGisObjectSQL, sSQLName:=ACDelGisObjectName, bStoredProcedure:=ACDelGisObjectSP)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed on SQL : " & ACDelGisObjectSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End With

            ' Now log the event

            oEvent = New bSIREvent.Business
            m_lReturn = oEvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bPMLock.User", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Remove Component Services

            ' Add the event
            sDescription = "Changed party type from " & v_sPartyTypeOld & " to " & v_sPartyTypeNew

            ' EventType 14 ... can't find any constants!

            m_lReturn = oEvent.DirectAdd(vPartyCnt:=v_lPartyCnt, vUserId:=m_iUserID, vDescription:=sDescription, vEventTypeCode:="CLICHANGE", vOldPartyTypeID:=v_lPartyTypeOldID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add event.", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Terminate the object and clear it up

            oEvent.Dispose()

            oEvent = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ConvertParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertParty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'MSS200901 - Added For Merge

    ' ***************************************************************** '
    ' Name: UnderwritingOrAgency
    '
    ' Description:  Finds out if Underwriting or Agency business
    '
    ' ***************************************************************** '
    Private Function getUnderwritingOrAgency() As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        'sj 19/06/2002 - start
        m_lReturn = bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, r_vUnderwriting:=m_sUnderwritingOrAgency)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getUnderwritingOrAgency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUnderwritingOrAgency")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    'SD 17/07/2002 Redundant function has been disabled
    '' SD 12/07/2002
    '' Get the sub branch name (description field) given the id
    '' ***************************************************************** '
    '' Name: GetSubBranchName
    ''
    '' Description:  Gets the Sub Branch Name for a given party
    ''
    '' ***************************************************************** '
    'Public Function GetSubBranchName(vPartyCnt As Variant, _
    ''                                    vSubBranchName As Variant) As Long
    '
    'Dim sSQL As String
    'Dim vResultArray As Variant
    '
    '    On Error GoTo Err_GetSubBranchName
    '
    '    GetSubBranchName = PMTrue
    '
    '    sSQL = "SELECT sb.description " & _
    ''            "FROM party p, sub_branch sb WHERE " & _
    ''            "p.party_cnt = " & CLng(vPartyCnt) & " AND " & _
    ''            "p.sub_branch_id =  sb.sub_branch_id"
    '
    '    m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL$, _
    ''                                        sSQLName:="GETSUBBRANCHNAME", _
    ''                                    bStoredProcedure:=False, _
    ''                                    vResultArray:=vResultArray)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        GetSubBranchName = PMFalse
    '        Exit Function
    '    End If
    '
    '    'Return the value if there is one
    '    If (IsArray(vResultArray) = True) Then
    '        vSubBranchName = CStr(vResultArray(0, 0))
    '    Else
    '        vSubBranchName = ""
    '    End If
    '
    '    Exit Function
    '
    'Err_GetSubBranchName:
    '
    '    GetSubBranchName = PMError
    '
    '    ' Log Error Message
    '    LogMessage m_sUsername, _
    ''        iType:=PMLogError, _
    ''        sMsg:="GetSubBranchNameFailed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="GetSubBranchName", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function

    ' ***************************************************************** '
    '
    ' Name: GetFutureDatedAddresses
    '
    ' Description:
    '
    ' History: 18/07/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function GetFutureDatedAddresses(ByRef r_vFutureDatedAddresses(,) As Object) As Integer
        Return GetFutureDatedAddresses(r_vFutureDatedAddresses:=r_vFutureDatedAddresses, v_vPartyCnt:=Nothing)
    End Function
    Public Function GetFutureDatedAddresses(ByRef r_vFutureDatedAddresses(,) As Object, ByVal v_vPartyCnt As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()


            If Not Informations.IsNothing(v_vPartyCnt) Then
                'Get all the uncommitted records for a given party

                m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(Val(CStr(v_vPartyCnt))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add party_cnt parameter", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFutureDatedAddresses")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACPartyVariantAddressSelectSQL, sSQLName:=ACPartyVariantAddressSelectName, bStoredProcedure:=ACPartyVariantAddressSelectStored, vResultArray:=r_vFutureDatedAddresses)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="spu_party_variant_address_sel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFutureDatedAddresses")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                'Get all the uncommitted records
                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACPartyVariantAddressSelectAllSQL, sSQLName:=ACPartyVariantAddressSelectAllName, bStoredProcedure:=ACPartyVariantAddressSelectAllStored, vResultArray:=r_vFutureDatedAddresses)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="spu_party_variant_address_selall Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFutureDatedAddresses")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

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
    '          08/05/2003 SJ - Add user id
    ' ***************************************************************** '
    Public Function CreateFutureDatedAddresses(ByVal v_lPartyCnt As Integer, ByVal v_vFutureDatedAddresses(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sSQL As String = ""
            Dim lReturn As gPMConstants.PMEReturnCode

            'Const ACPartyVariantAddressCnt As Integer = 0     ''Unused Variable by Rachana
            'Const ACPartyCnt As Integer = 1     ''Unused Variable by Rachana
            Const ACAddressCnt As Integer = 2
            Const ACOriginalAddressCnt As Integer = 3
            Const ACEffectiveDate As Integer = 4
            Const ACDateCreated As Integer = 5
            'Const ACCommitInd As Integer = 6    ''Unused Variable by Rachana

            'Delete
            Const DELETE_PARTY_VARIANT_ADDRESS_1 As String = "DELETE FROM party_variant_address WHERE commit_ind = 0 AND party_cnt = "
            Const DELETE_PARTY_VARIANT_ADDRESS_2 As String = ";"

            'Insert
            Const INSERT_PARTY_VARIANT_ADDRESS_1 As String = "INSERT INTO party_variant_address " &
                                                             "(party_cnt,address_cnt,original_address_cnt,effective_date,date_created,commit_ind,user_id) VALUES ("
            Const INSERT_PARTY_VARIANT_ADDRESS_2 As String = ", "
            Const INSERT_PARTY_VARIANT_ADDRESS_3 As String = ");" & Strings.ChrW(13) & Strings.ChrW(10)

            ' Start transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start transaction", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateFutureDatedAddresses")
                Return result
            End If

            ' Save every selected scheme.
            sSQL = "BEGIN" & Strings.ChrW(13) & Strings.ChrW(10) & "SET NOCOUNT ON " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & DELETE_PARTY_VARIANT_ADDRESS_1 & CStr(v_lPartyCnt) & DELETE_PARTY_VARIANT_ADDRESS_2 & Strings.ChrW(13) & Strings.ChrW(10)

            If Informations.IsArray(v_vFutureDatedAddresses) Then
                For iCnt As Integer = v_vFutureDatedAddresses.GetLowerBound(1) To v_vFutureDatedAddresses.GetUpperBound(1)





                    sSQL = sSQL & INSERT_PARTY_VARIANT_ADDRESS_1 & CStr(v_lPartyCnt) & INSERT_PARTY_VARIANT_ADDRESS_2 & CStr(v_vFutureDatedAddresses(ACAddressCnt, iCnt)) & INSERT_PARTY_VARIANT_ADDRESS_2 & CStr(v_vFutureDatedAddresses(ACOriginalAddressCnt, iCnt)) & INSERT_PARTY_VARIANT_ADDRESS_2 & "'" & CDate(v_vFutureDatedAddresses(ACEffectiveDate, iCnt)).ToString("dd MMM yyyy") & "'" & INSERT_PARTY_VARIANT_ADDRESS_2 & "'" & CDate(v_vFutureDatedAddresses(ACDateCreated, iCnt)).ToString("dd MMM yyyy") & "'" & INSERT_PARTY_VARIANT_ADDRESS_2 & CStr(0) & INSERT_PARTY_VARIANT_ADDRESS_2 & CStr(m_iUserID) & INSERT_PARTY_VARIANT_ADDRESS_3

                Next
            End If

            sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10) & " SET NOCOUNT OFF " & Strings.ChrW(13) & Strings.ChrW(10) & "END"

            ' Process the SQL Statements
            m_lReturn = m_oDatabase.SQLAction(sSQL, "CreateFutureDatedAddresses", False)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.SQLAction Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateFutureDatedAddresses")

                ' Rollback the Transaction
                lReturn = m_oDatabase.SQLRollbackTrans()
                ' More to do here. If Failed to Rollback log an error
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' Commit changes to database.
            m_lReturn = m_oDatabase.SQLCommitTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to commit transaction", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateFutureDatedAddresses")
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateFutureDatedAddresses Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateFutureDatedAddresses", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitFutureDatedAddress
    '
    ' Description:
    '
    ' History: 18/07/2002 sj - Created.
    '
    ' ***************************************************************** '
    Public Function CommitFutureDatedAddress(ByVal v_dtEffectiveDate As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=CStr(v_dtEffectiveDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add effective_date parameter", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitFutureDatedAddress")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACPartyVariantAddressCommitSQL, sSQLName:=ACPartyVariantAddressCommitName, bStoredProcedure:=ACPartyVariantAddressCommitStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="spu_party_variant_address_upd Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitFutureDatedAddress")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitFutureDatedAddress Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitFutureDatedAddress", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
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


        '        On Error GoTo Catch_Renamed
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Add parameters
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=v_lParty_cnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPartyPoliciesSQL, sSQLName:=ACGetPartyPoliciesName, bStoredProcedure:=ACGetPartyPolicies, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "sSQL:= " & ACGetPartyPoliciesSQL, gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Informations.IsArray(r_vResultArray) Then
                r_vResultArray = Nothing

                Return result
            End If

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

            Return result

        End Try
        '        GoTo Finally_Renamed

        'Catch_Renamed:

        ' DO Not Call any functions before here or the error will be lost


        ' If you want to rollback a transaction or something, do it here

        'Finally_Renamed:

        ' Do any tidy up, e.g. Set x = Nothing here

        '        Return result

        ' This is for debugging only
        '        Resume

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetBaseCurrencyID
    '
    ' Description:
    '
    ' History: 20042004 RDC created
    '
    ' ***************************************************************** '
    Public Function GetBaseCurrencyID(ByVal lSourceID As Integer, ByRef iCurrencyID As Integer) As Integer


        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResult(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="SourceID", vValue:=CStr(lSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBaseCurrencyID failed to add parameter SourceID", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBaseCurrencyID")

                Return result
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetBaseCurrencySQL, sSQLName:=ACGetBaseCurrencyName, bStoredProcedure:=ACGetBaseCurrencyStored, vResultArray:=vResult)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBaseCurrencyID failed to get SourceID", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBaseCurrencyID")

                Return result
            End If

            If Not Informations.IsArray(vResult) Then
                Return result
            End If


            iCurrencyID = CInt(vResult(0, 0))


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBaseCurrencyID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBaseCurrencyID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function CheckDuplicateShortname(ByVal v_sShortname As String, ByRef v_vMatchArray(,) As Object) As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="shortname", vValue:=v_sShortname, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckDuplicateShortname failed to add parameter shortname", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckDuplicateShortname")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDuplicateClientSQL, sSQLName:=ACGetDuplicateClient, bStoredProcedure:=ACGetDuplicateClientStored, vResultArray:=v_vMatchArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckDuplicateShortname failed to get details", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckDuplicateShortname")


                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckDuplicateShortname Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckDuplicateShortname", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckReference (Public)
    '
    ' Description: Checks if the passed account code already exists
    '
    ' ***************************************************************** '
    Public Function CheckIfAccountCodeExists(ByVal v_sAccountCode As String, ByRef r_bExists As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CheckIfAccountCodeExists"



        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Default to assuming the account code is not duplicated
            r_bExists = False

            'Add parameters
            'Developer Guide No. 98
            bPMAddParameter.AddParameterLite(m_oDatabase, "AccountCode", v_sAccountCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True)

            bPMAddParameter.AddParameterLite(m_oDatabase, "Exists", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMBoolean)

            'Check for dupicates in the party table
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCheckDupRefSQL, sSQLName:=ACCheckDupRefName, bStoredProcedure:=ACCheckDupRefStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "sSQL:= " & ACCheckDupRefSQL, gPMConstants.PMELogLevel.PMLogError)
            End If

            'Set the return parameter to the same as what was returned from the stored procedure
            r_bExists = m_oDatabase.Parameters.Item("Exists").Value
            Return result

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        End Try
        Return result
    End Function
    Public Function GetPartyContacts(ByVal v_lParty_cnt As Long, ByRef r_ResultArray As Object(,)) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetPartyContacts"

        'On Error GoTo Catch_Renamed
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            'Add parameters
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=v_lParty_cnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPartyContactsSQL, sSQLName:=ACGetPartyContactsName, bStoredProcedure:=ACGetPartyContacts, vResultArray:=r_ResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "sSQL:= " & ACGetPartyContactsSQL, gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(r_ResultArray) Then
                Return result
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

        ' DO Not Call any functions before here or the error will be lost
        '        bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

        ' If you want to rollback a transaction or something, do it here

        'Finally_Renamed:

        ' Do any tidy up, e.g. Set x = Nothing here

        '            Return result

        ' This is for debugging only
        '            Resume

        '            Return result
    End Function


    ' ***************************************************************** '
    ' Name: CheckIfPartyAccountExists (Public)
    '
    ' Description: Checks if the account for a Party exists
    '
    ' ***************************************************************** '
    Public Function CheckIfPartyAccountExists(ByVal v_lPartyCnt As Integer, ByRef r_bExists As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CheckIfPartyAccountExists"

        Dim vResultArray(,) As Object = Nothing

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Default to assuming the account code is not dupli cated
            r_bExists = 0

            'Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "PartyCnt", v_lPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            bPMAddParameter.AddParameterLite(m_oDatabase, "Exists", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMBoolean)

            'Check for dupicates in the party table
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCheckClientAccountSQL, sSQLName:=ACCheckClientAccountName, bStoredProcedure:=ACCheckClientAccount)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "sSQL:= " & ACCheckClientAccountSQL, gPMConstants.PMELogLevel.PMLogError)
            End If

            'Set the return parameter to the same as what was returned from the stored procedure
            r_bExists = m_oDatabase.Parameters.Item("Exists").Value
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

    ' ***************************************************************** '
    ' Name: GetPartyDetails
    '
    ' Parameters: n/a
    '
    ' Description: REt
    '
    ' History:
    '           Created : MEvans : 18-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function GetPartyDetails(ByVal v_lPartyCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPartyDetails"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = AddInputParameter(v_sName:="party_cnt", v_vValue:=v_lPartyCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=kGetPartyDetailsSQL, sSQLName:=kGetPartyDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetPartyDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

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
    '           Created : MEvans : 18-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function UpdatePartyDetails(ByVal v_lPartyCnt As Integer, ByVal v_vPartyDetails As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdatePartyDetails"


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            AddInputParameter(v_sName:="party_cnt", v_vValue:=v_lPartyCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Add Required Stored Procedure Parameters

            AddPartyDetailParameters(v_vPartyDetails:=v_vPartyDetails)

            ' Execute Action Query
            If m_oDatabase.SQLAction(sSQL:=kUpdatePartyDetailsSQL, sSQLName:=kUpdatePartyDetailsName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kUpdatePartyDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

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
    ' Name: AddTaxDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : VGupta : 05-10-2006 : Other party update SAM
    ' ***************************************************************** '
    Public Function AddTaxDetails(ByVal r_lPartyCnt As Integer, ByVal r_sTaxNumber As String, ByVal r_bDomiciledForTax As Integer, ByVal r_bTaxExempt As Integer, ByVal r_lPercentage As Decimal) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddTaxDetails"



        Try



            m_oDatabase.Parameters.Clear()
            AddInputParameter(v_sName:="party_cnt", v_vValue:=r_lPartyCnt, v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="tax_number", v_vValue:=r_sTaxNumber, v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="domiciled_for_tax", v_vValue:=r_bDomiciledForTax, v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="tax_exempt", v_vValue:=r_bTaxExempt, v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="tax_percentage", v_vValue:=r_lPercentage, v_iType:=gPMConstants.PMEDataType.PMCurrency)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=kUpdatePartyTaxDetailsSQL, sSQLName:=kUpdatePartyTaxDetailsName, bStoredProcedure:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kUpdatePartyTaxDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If
            Return m_lReturn

        Catch ex As Exception


            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here



            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateOtherpartyInfo
    '
    ' Parameters:Update Details of Other Party
    '
    ' Description:Update details of Other Party in Party_Other table
    '
    ' History:
    '           Created : Gaurav Arora : 12-OCT-2006
    ' ***************************************************************** '

    Public Function UpdateOtherpartyInfo(ByVal lPartyCnt As Integer, ByVal sLicenseTypeCode As String, ByVal sLicenseNumber As String, ByVal DateOfBirth As Date, ByVal sGender As String, ByVal sPartyStatus As String, ByVal sReferenceNo As String, ByVal sExternalId As String, ByVal sRegNumber As String, ByVal DatePassedTest As Date, ByVal sContactName As String, ByVal sContacttelNo As String, ByVal sInsurerName As String, ByVal sInsurerAdd1 As String, ByVal sInsurerAdd2 As String, ByVal sInsurerAdd3 As String, ByVal sInsurerAdd4 As String, ByVal sInsurerPostCode As String, ByVal sInsurerTelNo As String, ByVal sInsurerFaxNo As String, ByVal sInsurerContactName As String, ByVal sInsurerEmail As String, ByVal sInsurerNotes As String, ByVal sCompanyNotes As String, ByVal lActiveIndicator As Integer, ByVal lAfterHoursIndicator As Integer, ByVal lPriorityIndicator As Integer) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "UpdateOtherpartyInfo"

        Try




            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            AddInputParameter(v_sName:="party_cnt", v_vValue:=lPartyCnt, v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="license_type_id", v_vValue:=sLicenseTypeCode, v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="license_number", v_vValue:=sLicenseNumber, v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="date_of_birth", v_vValue:=DateOfBirth, v_iType:=gPMConstants.PMEDataType.PMDate)
            AddInputParameter(v_sName:="gender", v_vValue:=sGender, v_iType:=gPMConstants.PMEDataType.PMString)

            AddInputParameter(v_sName:="party_status", v_vValue:=sPartyStatus, v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="reference_number", v_vValue:=sReferenceNo, v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="external_id", v_vValue:=sExternalId, v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="reg_number", v_vValue:=sRegNumber, v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="date_passed_test", v_vValue:=DatePassedTest, v_iType:=gPMConstants.PMEDataType.PMDate)
            AddInputParameter(v_sName:="contact_name", v_vValue:=sContactName, v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="contact_telephone_number", v_vValue:=sContacttelNo, v_iType:=gPMConstants.PMEDataType.PMString)

            AddInputParameter(v_sName:="insurer_name", v_vValue:=sInsurerName, v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="insurer_address1", v_vValue:=sInsurerAdd1, v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="insurer_address2", v_vValue:=sInsurerAdd2, v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="insurer_address3", v_vValue:=sInsurerAdd3, v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="insurer_address4", v_vValue:=sInsurerAdd4, v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="insurer_postcode", v_vValue:=sInsurerPostCode, v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="insurer_telephone_number", v_vValue:=sInsurerTelNo, v_iType:=gPMConstants.PMEDataType.PMString)


            AddInputParameter(v_sName:="insurer_fax_number", v_vValue:=sInsurerFaxNo, v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="insurer_contact_name", v_vValue:=sInsurerContactName, v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="insurer_email", v_vValue:=sInsurerEmail, v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="insurer_notes", v_vValue:=sInsurerNotes, v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="company_notes", v_vValue:=sCompanyNotes, v_iType:=gPMConstants.PMEDataType.PMString)

            AddInputParameter(v_sName:="active_indicator", v_vValue:=sInsurerEmail, v_iType:=gPMConstants.PMEDataType.PMInteger)
            AddInputParameter(v_sName:="after_hours_indicator", v_vValue:=sInsurerNotes, v_iType:=gPMConstants.PMEDataType.PMInteger)
            AddInputParameter(v_sName:="priority_indicator", v_vValue:=sCompanyNotes, v_iType:=gPMConstants.PMEDataType.PMInteger)

            ' Execute Action Query
            If m_oDatabase.SQLAction(sSQL:=kUpdateOtherPartySQL, sSQLName:=kUpdateOtherPartyName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kUpdatePartyDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

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
    ' Name: UpdatePartyConviction
    '
    ' Parameters:
    '
    ' Description:Update Conviction Details related to Other Party
    '
    ' History:
    '           Created : Gaurav Arora : 12-OCT-2006
    ' ***************************************************************** '
    Public Function UpdatePartyConviction(ByVal lParty_cnt As Integer, ByVal lParty_conviction_id As Integer, ByVal sCode As String, ByVal sconviction_date As String, ByVal sDescription As String, ByVal dfine_amt As Double, ByVal sSentence_code As String, ByVal sSentence_description As String, ByVal dSentence_duration As Double, ByVal sSentence_duration_qualifier As String, ByVal sSentence_effective_date As String, ByVal sStatus_code As String, ByVal dAlcohol_level As Double, ByVal sAlcohol_measurement_method As String, ByVal dDriving_licence_penalty_pts As Double) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "UpdatePartyConviction"

        Try




            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            AddInputParameter(v_sName:="party_cnt", v_vValue:=lParty_cnt, v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="party_conviction_id", v_vValue:=lParty_conviction_id, v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="code", v_vValue:=sCode, v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="conviction_date", v_vValue:=sconviction_date, v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="description", v_vValue:=sDescription, v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="fine_amt", v_vValue:=dfine_amt, v_iType:=gPMConstants.PMEDataType.PMCurrency)
            AddInputParameter(v_sName:="sentence_code", v_vValue:=sSentence_code, v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="sentence_description", v_vValue:=sSentence_description, v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="sentence_duration", v_vValue:=dSentence_duration, v_iType:=gPMConstants.PMEDataType.PMCurrency)
            AddInputParameter(v_sName:="sentence_duration_qualifier", v_vValue:=sSentence_duration_qualifier, v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="sentence_effective_date", v_vValue:=sSentence_effective_date, v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="status_code", v_vValue:=sStatus_code, v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="alcohol_level", v_vValue:=dAlcohol_level, v_iType:=gPMConstants.PMEDataType.PMCurrency)
            AddInputParameter(v_sName:="alcohol_measurement_method", v_vValue:=sAlcohol_measurement_method, v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="driving_licence_penalty_pts", v_vValue:=dDriving_licence_penalty_pts, v_iType:=gPMConstants.PMEDataType.PMCurrency)

            ' Execute Action Query
            If m_oDatabase.SQLAction(sSQL:=kUpdateOPConvictionSQL, sSQLName:=kUpdateOPConvictionName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kUpdateOPConvictionSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

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
    ' Name: UpdatePartyAccident
    '
    ' Parameters:
    '
    ' Description:Update Accident Details related to Other Party
    '
    ' History:
    '           Created : Gaurav Arora : 12-OCT-2006
    ' ***************************************************************** '
    Public Function UpdatePartyAccident(ByVal lParty_cnt As Integer, ByVal lPrevious_accidents_id As Integer, ByVal dDate As Date, ByVal sDescription As String, ByVal lIs_at_fault As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdatePartyAccident"

        Try




            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            AddInputParameter(v_sName:="party_cnt", v_vValue:=lParty_cnt, v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="previous_accidents_id", v_vValue:=lPrevious_accidents_id, v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="Date", v_vValue:=dDate, v_iType:=gPMConstants.PMEDataType.PMDate)
            AddInputParameter(v_sName:="Description", v_vValue:=sDescription, v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="is_at_fault", v_vValue:=lIs_at_fault, v_iType:=gPMConstants.PMEDataType.PMBoolean)

            ' Execute Action Query
            If m_oDatabase.SQLAction(sSQL:=kUpdateOPAccidentSQL, sSQLName:=kUpdateOPAccidentName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kUpdateOPAccidentSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

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
    ' Name: AddAddressContacts
    '
    ' Description: Add the details to Contact_Address_Usage table
    '
    ' History:
    '           Created : VGupta : 13-10-2006
    ' ***************************************************************** '
    'Developer Guide No. 71

    Public Function AddAddressContacts(ByRef vAddContacts(,) As Object) As Integer
        Return AddAddressContacts(vAddContacts:=vAddContacts, vDeleteContacts:=Nothing)
    End Function

    Public Function AddAddressContacts(ByRef vAddContacts(,) As Object, ByRef vDeleteContacts As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Add Address Contacts"

        Dim sSQL As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = BeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Add new contacts for address if supplied
            If Informations.IsArray(vAddContacts) Then

                For i As Integer = vAddContacts.GetLowerBound(0) To vAddContacts.GetUpperBound(0)


                    If CInt(vAddContacts(i, 0)) <> 0 Then



                        sSQL = "INSERT INTO contact_address_usage " &
                               "(contact_cnt, address_cnt) VALUES " &
                               "(" & CStr(vAddContacts(i, 0)) & ", " &
                               CStr(vAddContacts(i, 1)) & ")"

                        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="ADDPARTYCONS", bStoredProcedure:=False)

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

        Catch ex As Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = RollbackTrans()


            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

            Return result
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: UpdateSupplierBusiness
    '
    ' Parameters:
    '
    ' Description:Update Supplier Business related to Other Party
    '
    ' History:
    '           Created : Gaurav Arora : 12-OCT-2006
    ' ***************************************************************** '
    Public Function UpdateSupplierBusiness(ByVal vSupplierBusiness As Object, ByVal lPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SupplierBusiness"



        Try


            ' Delete existing supplier business data related to party
            Dim sSQL As String = ""
            sSQL = "Delete from Party_Supplier_Business where party_cnt=" & lPartyCnt
            If m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="SupplierBusiness", bStoredProcedure:=False) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kUpdatePartyTaxDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If

            ' Add the Details Supplied in

            For cnt As Integer = 0 To vSupplierBusiness.GetUpperBound(0)
                m_oDatabase.Parameters.Clear()

                AddInputParameter(v_sName:="party_cnt", v_vValue:=lPartyCnt, v_iType:=gPMConstants.PMEDataType.PMLong)
                AddInputParameter(v_sName:="supplier_speciality_id", v_vValue:=1, v_iType:=gPMConstants.PMEDataType.PMLong)
                AddInputParameter(v_sName:="supplier_business_id", v_vValue:=1, v_iType:=gPMConstants.PMEDataType.PMLong)

                If m_oDatabase.SQLAction(sSQL:=ACAddPartySupplierDetailsSQL, sSQLName:=ACAddPartySupplierDetailsName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                    gPMFunctions.RaiseError(kMethodName, ACAddPartySupplierDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

                End If

            Next

        Catch
        End Try



        ' DO Not Call any functions before here or the error will be lost
        bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

        ' If you want to rollback a transaction or something, do it here



        Return result



        Return result
    End Function

    ' ***************************************************************** '
    ' Name: UpdateAddressContacts
    '
    ' Description: Update the Contact_Address_Usage table with old
    ' and new contacts attached with the address.
    '
    ' History:
    '           Created : Gaurav Arora : 12-OCT-2006
    ' ***************************************************************** '
    'Developer Guide No. 71 
    Public Function UpdateAddressContacts(ByRef vAddContacts(,) As Object) As Integer
        Return UpdateAddressContacts(vAddContacts:=vAddContacts, vDeleteContacts:=Nothing)
    End Function
    Public Function UpdateAddressContacts(ByRef vAddContacts(,) As Object, ByRef vDeleteContacts As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Update Address Contacts"

        Dim sSQL As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = BeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Add new contacts for address if supplied
            If Informations.IsArray(vAddContacts) Then

                For i As Integer = vAddContacts.GetLowerBound(0) To vAddContacts.GetUpperBound(0)


                    If CInt(vAddContacts(i, 0)) <> 0 Then



                        sSQL = "INSERT INTO contact_address_usage " &
                               "(contact_cnt, address_cnt) VALUES " &
                               "(" & CStr(vAddContacts(i, 0)) & ", " &
                               CStr(vAddContacts(i, 1)) & ")"

                        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="ADDPARTYCONS", bStoredProcedure:=False)

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

        Catch ex As Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = RollbackTrans()


            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

            Return result
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: AddPartyDetailParameters
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 18-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function AddPartyDetailParameters(ByVal v_vPartyDetails(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddPartyDetailParameters"


        Dim vTaxNumber As Object = Nothing
        Dim vDomiciledForTax As Object = Nothing
        Dim vTaxExempt As Object = Nothing
        Dim vTaxPercentage As Object = Nothing
        Dim vBlackListReasonId As Object = Nothing
        Dim lUBound As Integer

        ' Party Detail Array Position Constants
        Const kPartyDetailTaxNumber As Integer = 0
        Const kPartyDetailDomiciledForTax As Integer = 1
        Const kPartyDetailTaxExempt As Integer = 2
        Const kPartyDetailTaxPercentage As Integer = 3
        Const kPartyDetailBlackListReasonId As Integer = 4

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lUBound = v_vPartyDetails.GetUpperBound(0)

            If Informations.IsArray(v_vPartyDetails) Then


                vTaxNumber = v_vPartyDetails(kPartyDetailTaxNumber, 0)


                vDomiciledForTax = v_vPartyDetails(kPartyDetailDomiciledForTax, 0)


                vTaxExempt = v_vPartyDetails(kPartyDetailTaxExempt, 0)


                vTaxPercentage = v_vPartyDetails(kPartyDetailTaxPercentage, 0)

                If lUBound >= kPartyDetailBlackListReasonId Then


                    vBlackListReasonId = v_vPartyDetails(kPartyDetailBlackListReasonId, 0)
                Else


                    vBlackListReasonId = DBNull.Value
                End If
            End If

            AddInputParameter(v_sName:="tax_number", v_vValue:=vTaxNumber, v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="domiciled_for_tax", v_vValue:=vDomiciledForTax, v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="tax_exempt", v_vValue:=vTaxExempt, v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="tax_percentage", v_vValue:=vTaxPercentage, v_iType:=gPMConstants.PMEDataType.PMCurrency)
            AddInputParameter(v_sName:="blacklist_reason_id", v_vValue:=vBlackListReasonId, v_iType:=gPMConstants.PMEDataType.PMLong)

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
    ' Name: AddInputParameter
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddInputParameter"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add Parameter to database object

            If v_vValue Is DBNull.Value Then
                lReturn = m_oDatabase.Parameters.Add(v_sName, v_vValue, gPMConstants.PMEParameterDirection.PMParamInput, v_iType)
            Else
                lReturn = m_oDatabase.Parameters.Add(v_sName, CStr(v_vValue), gPMConstants.PMEParameterDirection.PMParamInput, v_iType)
            End If

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, " Failed to add parameter name:" & v_sName &
                                        ", values :" & CStr(v_vValue) & ", type:" & CStr(v_iType), gPMConstants.PMELogLevel.PMLogError)
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

    ' **************************************************************************** '
    ' Name: GetPreviousDataModel
    ' Description: Returns previous data model Id if screen data model has changed
    '              Returns GIS Policy Link Id if there is any
    ' **************************************************************************** '
    Public Function GetPreviousDataModel(ByVal lPartyCnt As Integer, ByRef r_lPreviousDataModelId As Integer, ByRef r_lGISPolicyLinkID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPreviousDataModel"



        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()
                .Parameters.Add("party_cnt", CStr(lPartyCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                'Developer Guide No. 86
                .Parameters.Add("previous_data_model_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

                'Developer Guide No. 86
                .Parameters.Add("gis_policy_link_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLAction(sSQL:=ACIsScreenDataModelChangedSQL, sSQLName:=ACIsScreenDataModelChangedName, bStoredProcedure:=ACIsScreenDataModelChangedStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(CStr(m_lReturn), "Stored procedure " & ACIsScreenDataModelChangedName & " failed.")
                End If

                r_lPreviousDataModelId = gPMFunctions.ToSafeLong(.Parameters.Item("previous_data_model_id").Value)
                r_lGISPolicyLinkID = gPMFunctions.ToSafeLong(.Parameters.Item("gis_policy_link_id").Value)

            End With

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
        Return result

    End Function

    ' **************************************************************************** '
    ' Name: DeleteCustomData
    ' Description: Deletes all corresponding GIS data for a GIS Policy Link Id
    ' **************************************************************************** '
    Public Function DeleteCustomData(ByVal lGISPolicyLinkID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DeleteCustomData"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = BeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(CStr(m_lReturn), "Begin Trans Failed.")
            End If

            With m_oDatabase

                .Parameters.Clear()
                .Parameters.Add("gis_policy_link_id", CStr(lGISPolicyLinkID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLAction(sSQL:=ACDeleteCustomDataSQL, sSQLName:=ACDeleteCustomDataName, bStoredProcedure:=ACDeleteCustomDataStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(CStr(m_lReturn), "Stored procedure " & ACDeleteCustomDataName & " failed.")
                End If

            End With

            lReturn = CommitTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(CStr(m_lReturn), "Commit Trans Failed.")
            End If

            Return result
        Catch ex As Exception

            'DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            lReturn = RollbackTrans()

        Finally


        End Try
        Return result

    End Function
    ''' <summary>
    ''' Creates party history
    ''' </summary>
    ''' <param name="r_nPartyCnt"></param>
    ''' <param name="v_sXMLDataset"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddPartyHistory(ByVal r_nPartyCnt As Integer, ByVal v_sXMLDataset As String) As Integer
        Const kMethodName As String = "AddPartyHistory"
        Dim bPartyHistoryLoggingEnabled As Boolean
        Dim nReturn As Integer = 1
        Dim sValue As String = "0"
        Dim sPartybuilder As String = String.Empty
        Try

            nReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID,
                                                v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, r_sOptionValue:=sValue, v_iOptionNumber:=knPartyHistoryLoggingEnabled)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nReturn)
                Return nReturn
            End If
            bPartyHistoryLoggingEnabled = (sValue = "1")

            If bPartyHistoryLoggingEnabled Then
                If String.IsNullOrEmpty(v_sXMLDataset) Then
                    nReturn = GetXBuilderDetails(r_nPartyCnt, sPartybuilder)
                    If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nReturn)
                    End If
                Else
                    sPartybuilder = v_sXMLDataset
                End If

                m_oDatabase.Parameters.Clear()
                AddInputParameter(v_sName:="Party_cnt", v_vValue:=r_nPartyCnt, v_iType:=gPMConstants.PMEDataType.PMInteger)
                AddInputParameter(v_sName:="Party_Builder_Data", v_vValue:=sPartybuilder, v_iType:=gPMConstants.PMEDataType.PMString)
                AddInputParameter(v_sName:="Date_Of_Change", v_vValue:=DateTime.Now, v_iType:=gPMConstants.PMEDataType.PMDate)
                AddInputParameter(v_sName:="User_Changed", v_vValue:=m_sUsername, v_iType:=gPMConstants.PMEDataType.PMString)

                nReturn = m_oDatabase.SQLAction(sSQL:=kAddPartyHistorySQL, sSQLName:=kAddPartyHistoryName, bStoredProcedure:=True)
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nReturn)
                End If
            End If
            Return nReturn
        Catch ex As Exception
            nReturn = PMEReturnCode.PMError
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nReturn)
            Return nReturn
        End Try
    End Function
    ''' <summary>
    ''' Get XBuilder Details
    ''' </summary>
    ''' <param name="v_partyKey"></param>
    ''' <param name="r_sXmlDataset"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetXBuilderDetails(ByVal v_partyKey As Integer, ByRef r_sXmlDataset As String) As Integer

        Dim nGisScreenId As Integer
        Dim sDataModelCode As String
        Dim nDataModelTypeId As Integer
        Dim nPartyTypeId As Integer
        Dim sPartyTypeCode As String
        Dim sXmlDataset As String = String.Empty
        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue

        Const kMethodName As String = "GetXBuilderDetails"

        Try
            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("party_cnt", CStr(v_partyKey), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                .Parameters.Add("gis_screen_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger)
                .Parameters.Add("party_type_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger)
                .Parameters.Add("data_model_code", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMString)
                .Parameters.Add("party_type_code", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMString)
                .Parameters.Add("data_model_type_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger)

                nResult = .SQLAction(sSQL:=kGetPartyGISDetailSQL, sSQLName:=kGetPartyGISDetailName, bStoredProcedure:=True)

                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult)
                    r_sXmlDataset = String.Empty
                    Return nResult
                End If

                nGisScreenId = gPMFunctions.ToSafeLong(.Parameters.Item("gis_screen_id").Value, 0)
                nPartyTypeId = gPMFunctions.ToSafeLong(.Parameters.Item("party_type_id").Value, 0)
                sDataModelCode = gPMFunctions.ToSafeString(.Parameters.Item("data_model_code").Value, String.Empty)
                sPartyTypeCode = gPMFunctions.ToSafeString(.Parameters.Item("party_type_code").Value, String.Empty)
                nDataModelTypeId = gPMFunctions.ToSafeLong(.Parameters.Item("data_model_type_id").Value, 0)
            End With

            If Not String.IsNullOrEmpty(sDataModelCode) AndAlso sDataModelCode.Length > 0 Then
                nResult = LoadPartyBuilderData(v_partyKey, nGisScreenId, sDataModelCode, nDataModelTypeId, sXmlDataset)
            End If
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult)
            End If

            r_sXmlDataset = sXmlDataset
            Return nResult
        Catch ex As Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult)
            Return nResult
        End Try

    End Function
    ''' <summary>
    ''' Load party builder data in XML format
    ''' </summary>
    ''' <param name="lPartyKey"></param>
    ''' <param name="lScreenId"></param>
    ''' <param name="sDataModelCode"></param>
    ''' <param name="lDataModelTypeId"></param>
    ''' <param name="r_sXMLDataset"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function LoadPartyBuilderData(
            ByVal lPartyKey As Integer,
            ByVal lScreenId As Integer,
            ByVal sDataModelCode As String,
            ByVal lDataModelTypeId As Integer,
            ByRef r_sXMLDataset As String) As Integer

        Const kMethodName As String = "LoadPartyBuilderData"
        Dim oPartyBuilderScreen As Object = Nothing
        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        nResult = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=oPartyBuilderScreen, v_sClassName:="bSIRRiskScreen.Stateless", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)



        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Dim oPartyBuilderScreen As bSIRRiskScreen.Stateless = New bSIRRiskScreen.Stateless

        Dim XMLDOC As New XmlDocument
        Dim XDType As XmlDocumentType
        Try

            'oPartyBuilderScreen = CType(oPartyBuilderScreen, bSIRRiskScreen.Stateless)

            'nResult = oPartyBuilderScreen.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            'If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            '    Return gPMConstants.PMEReturnCode.PMFalse
            'End If

            Dim partyBuilderScreenLoad As New XMLTransPartyBuilderScreenLoad
            Dim TheInput As New XMLTransPartyBuilderScreenLoad.PartyBuilderScreenLoadIn
            Dim TheOutput As XMLTransPartyBuilderScreenLoad.PartyBuilderScreenLoadOut

            With TheInput
                .nTask = CShort(Task) ' PMAdd or PMEdit or PMView 
                .nSourceID = 0 ' Not needed
                .nNavigate = 0 ' Not needed
                .nProcessMode = CShort(0) ' Not needed
                .dtEffectiveDate = DateTime.Now
                .bSubScreen = False ' Not a sub screen
                .nScreenId = lScreenId ' NEED THE SCREEN ID PASSED IN
                .nRiskId = 0 ' Pretty sure not needed
                .nRiskTypeId = 0
                .sGisDataModelCode = sDataModelCode
                .nGISDataModelType = lDataModelTypeId
                .nObjectType = GISOTRisk
                .sGISXMLDataset = String.Empty  ' not needed unless processing a subscreen
                .sMyOIKey = String.Empty  ' Not needed
                .sMyObjectName = String.Empty ' Not needed
                .sParentOIKey = String.Empty  ' Not needed
                .sParentObjectName = String.Empty  ' Not needed
                .nPolicyLinkId = 0 ' Loaded by the GIS
                .nInsuranceFolderCnt = 0 ' Only used by Risks DMs
                .nInsuranceFileCnt = 0 ' Only used by Risks DMs
                .oScreenDetailsArray = Nothing ' Get's loaded by bSIRRiskScreen
                .oScreenValuesArray = Nothing ' Doesn't get used
                .oRiskDetailsArray = Nothing ' Only used by Risks DMs
                .oRiskTypeDetailsArray = Nothing ' Only used by Risks DMs
                .sTransactionType = String.Empty  ' The transaction type code of the claim, i.e. Open, Maintain etc.
                .nTransactionType = 0 ' The transaction type of the claim, i.e. Open, Maintain etc. (I don't know why we need both)
                .nProductId = 0 ' Only used by Risks DMs
                .nPartyCnt = lPartyKey ' Only used by Party DMs 
                .nClaimID = 0 ' Proper claim id, not the base claim id
                .bCopyRisk = False
            End With

            Dim sInput As String = partyBuilderScreenLoad.SerializePartyBuilderScreenLoadIn(TheInput)

            Dim sOutput As String = oPartyBuilderScreen.RiskScreenLoadRisk(v_sInput:=ToSafeString(sInput))

            TheOutput = partyBuilderScreenLoad.DeserializePartyBuilderScreenLoadOut(sOutput)
            With TheOutput
                XMLDOC.LoadXml(TheOutput.sGISXMLDataset)
                XDType = XMLDOC.DocumentType
                XMLDOC.RemoveChild(XDType)
                r_sXMLDataset = XMLDOC.InnerXml
            End With

        Catch ex As Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult)
        Finally
            oPartyBuilderScreen.Dispose()
            oPartyBuilderScreen = Nothing
            XMLDOC = Nothing
            XDType = Nothing
        End Try
        Return nResult
    End Function
    ''' <summary>
    ''' Get All Parties Cnt data
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAllParties(ByRef r_oPartyCnt As Object) As Integer

        Const kMethodName As String = "GetAllParties"

        Dim nReturn As Integer
        Try
            m_oDatabase.Parameters.Clear()

            nReturn = m_oDatabase.SQLSelect(sSQL:=kGetAllPartiesSQl, sSQLName:=kGetAllPartiesName, bStoredProcedure:=True, lNumberRecords:=-1, vResultArray:=r_oPartyCnt)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nReturn)
                Return nReturn
            End If
            If Not Informations.IsArray(r_oPartyCnt) Then
                nReturn = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return nReturn
        Catch ex As Exception
            nReturn = PMEReturnCode.PMError
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nReturn)
            Return nReturn
        End Try
    End Function

    Public Function CreateAndSavePartyHistorySchema() As Integer
        Const kMethodName As String = "CreateAndSavePartyHistorySchema"
        Dim nReturn As Integer = gPMConstants.PMEReturnCode.PMTrue
        Try
            m_oDatabase.Parameters.Clear()
            nReturn = PartyFunc.CreateAndSavePartyHistorySchema(m_oDatabase)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nReturn)
                Return nReturn
            End If
            Return nReturn
        Catch ex As Exception
            nReturn = PMEReturnCode.PMError
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nReturn)
            Return nReturn
        End Try
    End Function


    Public Function LogClientViewedEvent(ByVal partyCnt As Integer) As Integer
        Dim result As Integer = 0
        Try
            Dim oEvent As bSIREvent.Business
            Dim eventCnt As Object = Nothing
            Dim eventTypeId As Integer = 0
            oEvent = New bSIREvent.Business
            m_lReturn = oEvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bPMLock.User", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Get event type ID for CLVIEW
            m_lReturn = GetEventTypeId("CLVIEW", eventTypeId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Some error has occured.
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            ' Create event
            'm_lReturn = oEvent.CreateEvent(r_lEventCnt:=eventCnt, v_lPartyCnt:=partyCnt, v_vInsuranceFolderCnt:=DBNull.Value, v_vInsuranceFileCnt:=DBNull.Value, v_vClaimCnt:=DBNull.Value, v_vDocumentCnt:=DBNull.Value, v_vOldAddressCnt:=DBNull.Value, v_vNewAddressCnt:=DBNull.Value, v_vCampaignId:=DBNull.Value, v_vDocumentTypeId:=DBNull.Value, v_vReportTypeId:=DBNull.Value, v_lEventTypeId:=eventTypeId, v_dtEventDate:=DateTime.Now, v_vDescription:="Client was viewed")
            m_lReturn = oEvent.DirectAdd(vEventCnt:=eventCnt, vPartyCnt:=partyCnt, vEventType:=eventTypeId, vUserId:=m_iUserID, vEventDate:=DateTime.Now, vDescription:="Client was viewed")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Some error has occured.
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If
            oEvent.Dispose()

            oEvent = Nothing

            result = gPMConstants.PMEReturnCode.PMTrue
            Return result
        Catch excep As Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LogClientViewedEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LogClientViewedEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Private Function GetEventTypeId(ByVal eventTypeCode As String, ByRef eventTypeId As Integer) As Integer
        Dim result As Integer = 0
        Try
            Dim vTabArray(3, 0) As Object
            Dim vResultArray(,) As Object = Nothing

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = "event_type"
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""

            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=vTabArray, iLanguageID:=m_iLanguageID, dtEffectiveDate:=DateTime.Now, vResultArray:=vResultArray)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue AndAlso vResultArray IsNot Nothing Then
                ' Search for the matching code in the result array
                For i As Integer = 0 To vResultArray.GetUpperBound(1)
                    If CStr(vResultArray(2, i)).Trim().ToUpper() = eventTypeCode.Trim().ToUpper() Then
                        eventTypeId = CInt(vResultArray(0, i))
                        result = gPMConstants.PMEReturnCode.PMTrue
                        Exit For
                    End If
                Next
            Else
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Event type lookup failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetEventTypeId")
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            result = gPMConstants.PMEReturnCode.PMTrue
            Return result
        Catch excep As Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetEventTypeId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetEventTypeId", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function
End Class

