Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("SIRParty_NET.SIRParty")> _
Public NotInheritable Class SIRParty
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: SIRParty
    '
    ' Date: 07/09/1998
    '
    ' Description: Describes the SIRParty attributes.
    '
    ' Edit History:
    ' ***************************************************************** '
    'test

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



    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "SIRParty"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag
    Private m_bCloseDatabase As Boolean

    ' DataBase Attributes
    Private m_lPartyCnt As Integer
    Private m_iPartyTypeID As Integer
    Private m_vIsAlsoAgent As Object
    Private m_lPartyStructureID As Integer
    Private m_lPartyID As Integer
    Private m_sShortname As New StringsHelper.FixedLengthString(20)
    'PN6556 - Removed 60 character limit
    Private m_sName As String = ""
    Private m_vResolvedName As Object
    Private m_vCollectTypeID As Object
    Private m_vAccumTreatmentTypeID As Object
    Private m_vStatsTreatmentTypeID As Object
    Private m_vPartyCategoryId As Object
    Private m_vAgentCnt As Object
    Private m_vConsultantCnt As Object
    Private m_iCreatedByID As Integer
    Private m_dtDateCreated As Date
    Private m_vLastModified As Object
    Private m_vModifiedByID As Object
    Private m_vPaymentMethodCode As Object
    Private m_vPaymentTermCode As Object
    Private m_vCreditCardCode As Object
    Private m_vFileCode As Object
    Private m_vABCCount As Object
    Private m_vStatements As Integer
    Private m_vOverride As Integer
    Private m_vOverrideRenewal As Integer
    Private m_vReminderTypeId As Object
    Private m_vRenewals As Integer
    Private m_vStatus As Object
    Private m_vLastACtionType As Object
    Private m_vIsTravelAgent As Integer
    Private m_vIsProspect As Integer
    Private m_vIsDeleted As Integer
    Private m_vABICodeOn406 As Object
    Private m_vABICodeOn81 As Object
    Private m_vABICodeList As Object
    Private m_vAreaId As Object
    Private m_vServiceLevelId As Integer
    Private m_vInvariantKey As Integer
    Private m_vRecordStatus As Object
    Private m_vCCJS As Integer
    Private m_vUserDefinedDataId As Object
    Private m_vSeasonalGiftID As Object
    Private m_bEvent As Boolean
    'DC 28/06/00
    Private m_vCorrespondenceTypeId As Object
    'Tomo060700
    Private m_vRenewalStopCodeId As Object

    ' CTAF 250900 SwiftPartyID
    Private m_vSwiftPartyID As Object

    'sj 12/06/2002 - start
    Private m_vTradingName As Object
    Private m_vLoyaltyNumber As Object
    Private m_vMarketingSegmentInd As Object
    Private m_vAlternativeIdentifier As Object
    Private m_vSubBranchId As Object

    Private m_vUniqueId As Object
    Private m_vScreenHeirarchy As Object
    Private m_vSubBranchName As Object
    Private m_vTobLetter As Object 'FSA Phase III
    Private m_oSystemOption As bSIROptions.Business
    ' Function Return Code
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public WriteOnly Property Database() As dPMDAO.Database
        Set(ByVal Value As dPMDAO.Database)
            m_oDatabase = Value
        End Set
    End Property


    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property

    Public Property PartyTypeID() As Integer
        Get
            Return m_iPartyTypeID
        End Get
        Set(ByVal Value As Integer)
            m_iPartyTypeID = Value
        End Set
    End Property

    Public Property IsAlsoAgent() As Object
        Get
            Return m_vIsAlsoAgent
        End Get
        Set(ByVal Value As Object)
            m_vIsAlsoAgent = Value
        End Set
    End Property

    Public Property PartyStructureID() As Integer
        Get
            Return m_lPartyStructureID
        End Get
        Set(ByVal Value As Integer)
            m_lPartyStructureID = Value
        End Set
    End Property

    Public Property SourceID() As Integer
        Get
            Return m_iSourceID
        End Get
        Set(ByVal Value As Integer)
            m_iSourceID = Value
        End Set
    End Property

    Public Property PartyID() As Integer
        Get
            Return m_lPartyID
        End Get
        Set(ByVal Value As Integer)
            m_lPartyID = Value
        End Set
    End Property

    Public Property Shortname() As String
        Get
            Return m_sShortname.Value
        End Get
        Set(ByVal Value As String)
            m_sShortname.Value = Value
        End Set
    End Property

    Public Property Name() As String
        Get
            Return m_sName
        End Get
        Set(ByVal Value As String)
            m_sName = Value
        End Set
    End Property

    Public Property ResolvedName() As Object
        Get
            Return m_vResolvedName
        End Get
        Set(ByVal Value As Object)
            m_vResolvedName = Value
        End Set
    End Property

    Public Property CurrencyID() As Integer
        Get
            Return m_iCurrencyID
        End Get
        Set(ByVal Value As Integer)
            m_iCurrencyID = Value
        End Set
    End Property

    Public Property LanguageID() As Integer
        Get
            Return m_iLanguageID
        End Get
        Set(ByVal Value As Integer)
            m_iLanguageID = Value
        End Set
    End Property

    Public Property CollectTypeID() As Object
        Get
            Return m_vCollectTypeID
        End Get
        Set(ByVal Value As Object)
            m_vCollectTypeID = Value
        End Set
    End Property

    Public Property AccumTreatmentTypeID() As Object
        Get
            Return m_vAccumTreatmentTypeID
        End Get
        Set(ByVal Value As Object)
            m_vAccumTreatmentTypeID = Value
        End Set
    End Property

    Public Property StatsTreatmentTypeID() As Object
        Get
            Return m_vStatsTreatmentTypeID
        End Get
        Set(ByVal Value As Object)
            m_vStatsTreatmentTypeID = Value
        End Set
    End Property

    Public Property PartyCategoryID() As Object
        Get
            Return m_vPartyCategoryId
        End Get
        Set(ByVal Value As Object)
            m_vPartyCategoryId = Value
        End Set
    End Property

    Public Property AgentCnt() As Object
        Get
            Return m_vAgentCnt
        End Get
        Set(ByVal Value As Object)
            m_vAgentCnt = Value
        End Set
    End Property

    Public Property ConsultantCnt() As Object
        Get
            Return m_vConsultantCnt
        End Get
        Set(ByVal Value As Object)
            m_vConsultantCnt = Value
        End Set
    End Property

    Public Property CreatedByID() As Integer
        Get
            Return m_iCreatedByID
        End Get
        Set(ByVal Value As Integer)
            m_iCreatedByID = Value
        End Set
    End Property

    Public Property DateCreated() As Date
        Get
            Return m_dtDateCreated
        End Get
        Set(ByVal Value As Date)
            m_dtDateCreated = Value
        End Set
    End Property

    Public Property LastModified() As Object
        Get
            Return m_vLastModified
        End Get
        Set(ByVal Value As Object)
            m_vLastModified = Value
        End Set
    End Property

    Public Property ModifiedByID() As Object
        Get
            Return m_vModifiedByID
        End Get
        Set(ByVal Value As Object)
            m_vModifiedByID = Value
        End Set
    End Property

    Public Property PaymentMethodCode() As Object
        Get
            Return m_vPaymentMethodCode
        End Get
        Set(ByVal Value As Object)
            m_vPaymentMethodCode = Value
        End Set
    End Property

    Public Property PaymentTermCode() As Object
        Get
            Return m_vPaymentTermCode
        End Get
        Set(ByVal Value As Object)
            m_vPaymentTermCode = Value
        End Set
    End Property

    Public Property CreditCardCode() As Object
        Get
            Return m_vCreditCardCode
        End Get
        Set(ByVal Value As Object)
            m_vCreditCardCode = Value
        End Set
    End Property

    Public Property FileCode() As Object
        Get
            Return m_vFileCode
        End Get
        Set(ByVal Value As Object)
            m_vFileCode = Value
        End Set
    End Property

    Public Property ABCCount() As Object
        Get
            Return m_vABCCount
        End Get
        Set(ByVal Value As Object)
            m_vABCCount = Value
        End Set
    End Property
    Public Property Override() As Integer
        Get
            Return m_vOverride
        End Get
        Set(ByVal Value As Integer)
            m_vOverride = CInt(Value)
        End Set
    End Property
    Public Property OverrideRenewal() As Integer
        Get
            Return m_vOverrideRenewal
        End Get
        Set(ByVal Value As Integer)
            m_vOverrideRenewal = CInt(Value)
        End Set
    End Property
    Public Property Statements() As Integer
        Get
            Return m_vStatements
        End Get
        Set(ByVal Value As Integer)
            m_vStatements = CInt(Value)
        End Set
    End Property

    Public Property ReminderTypeId() As Object
        Get
            Return m_vReminderTypeId
        End Get
        Set(ByVal Value As Object)
            m_vReminderTypeId = Value
        End Set
    End Property

    Public Property Renewals() As Integer
        Get
            Return m_vRenewals
        End Get
        Set(ByVal Value As Integer)
            m_vRenewals = CInt(Value)
        End Set
    End Property

    Public Property Status() As Object
        Get
            Return m_vStatus
        End Get
        Set(ByVal Value As Object)
            m_vStatus = Value
        End Set
    End Property

    Public Property LastActionType() As Object
        Get
            Return m_vLastACtionType
        End Get
        Set(ByVal Value As Object)
            m_vLastACtionType = Value
        End Set
    End Property

    Public Property IsTravelAgent() As Integer
        Get
            Return m_vIsTravelAgent
        End Get
        Set(ByVal Value As Integer)
            m_vIsTravelAgent = CInt(Value)
        End Set
    End Property

    Public Property IsProspect() As Integer
        Get
            Return m_vIsProspect
        End Get
        Set(ByVal Value As Integer)
            m_vIsProspect = CInt(Value)
        End Set
    End Property

    Public Property IsDeleted() As Integer
        Get
            Return m_vIsDeleted

        End Get
        Set(ByVal Value As Integer)
            m_vIsDeleted = CInt(Value)
        End Set
    End Property

    Public Property ABICodeOn406() As Object
        Get
            Return m_vABICodeOn406
        End Get
        Set(ByVal Value As Object)
            m_vABICodeOn406 = Value
        End Set
    End Property

    Public Property ABICodeOn81() As Object
        Get
            Return m_vABICodeOn81
        End Get
        Set(ByVal Value As Object)
            m_vABICodeOn81 = Value
        End Set
    End Property

    Public Property ABICodeList() As Object
        Get
            Return m_vABICodeList
        End Get
        Set(ByVal Value As Object)
            m_vABICodeList = Value
        End Set
    End Property

    Public Property AreaId() As Object
        Get
            Return m_vAreaId
        End Get
        Set(ByVal Value As Object)
            m_vAreaId = Value
        End Set
    End Property

    Public Property ServiceLevelId() As Integer
        Get
            Return m_vServiceLevelId
        End Get
        Set(ByVal Value As Integer)
            m_vServiceLevelId = CInt(Value)
        End Set
    End Property

    Public Property InvariantKey() As Integer
        Get
            Return m_vInvariantKey
        End Get
        Set(ByVal Value As Integer)
            m_vInvariantKey = CInt(Value)
        End Set
    End Property

    Public Property RecordStatus() As Object
        Get
            Return m_vRecordStatus
        End Get
        Set(ByVal Value As Object)
            m_vRecordStatus = Value
        End Set
    End Property

    Public Property CCJs() As Integer
        Get
            Return m_vCCJS
        End Get
        Set(ByVal Value As Integer)
            m_vCCJS = CInt(Value)
        End Set
    End Property

    Public Property UserDefinedDataId() As Object
        Get
            Return m_vUserDefinedDataId
        End Get
        Set(ByVal Value As Object)
            m_vUserDefinedDataId = Value
        End Set
    End Property
    Public Property SeasonalGiftID() As Object
        Get
            Return m_vSeasonalGiftID
        End Get
        Set(ByVal Value As Object)
            m_vSeasonalGiftID = Value
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
    'DC 28/06/00
    'DC 28/06/00
    Public Property CorrespondenceTypeId() As Object
        Get
            Return m_vCorrespondenceTypeId
        End Get
        Set(ByVal Value As Object)
            m_vCorrespondenceTypeId = Value
        End Set
    End Property

    'Tomo060700
    Public Property RenewalStopCodeId() As Object
        Get
            Return m_vRenewalStopCodeId
        End Get
        Set(ByVal Value As Object)
            m_vRenewalStopCodeId = Value
        End Set
    End Property

    ' CTAF 250900
    Public Property SwiftPartyID() As Object
        Get
            Return m_vSwiftPartyID
        End Get
        Set(ByVal Value As Object)
            m_vSwiftPartyID = Value
        End Set
    End Property
    'sj 12/06/2002 - start
    Public Property LoyaltyNumber() As Object
        Get
            Return m_vLoyaltyNumber
        End Get
        Set(ByVal Value As Object)
            m_vLoyaltyNumber = Value
        End Set
    End Property
    Public Property AlternativeIdentifier() As Object
        Get
            Return m_vAlternativeIdentifier
        End Get
        Set(ByVal Value As Object)
            m_vAlternativeIdentifier = Value
        End Set
    End Property
    Public Property MarketingSegmentInd() As Object
        Get
            Return m_vMarketingSegmentInd
        End Get
        Set(ByVal Value As Object)
            m_vMarketingSegmentInd = Value
        End Set
    End Property
    Public Property TradingName() As Object
        Get
            Return m_vTradingName
        End Get
        Set(ByVal Value As Object)
            m_vTradingName = Value
        End Set
    End Property
    Public Property SubBranchId() As Object
        Get
            Return m_vSubBranchId
        End Get
        Set(ByVal Value As Object)
            m_vSubBranchId = Value
        End Set
    End Property
    'sj 12/06/2002 - end
    Public Property SubBranchName() As Object
        Get
            Return m_vSubBranchName
        End Get
        Set(ByVal Value As Object)
            m_vSubBranchName = Value
        End Set
    End Property
    'FSA Phase III
    Public Property TobLetter() As Object
        Get
            Return m_vTobLetter
        End Get
        Set(ByVal Value As Object)
            m_vTobLetter = Value
        End Set
    End Property

    Public Property UniqueId() As Object
        Get
            Return m_vUniqueId
        End Get
        Set(ByVal Value As Object)
            m_vUniqueId = Value
        End Set
    End Property

    Public Property ScreenHeirarchy() As Object
        Get
            Return m_vScreenHeirarchy
        End Get
        Set(ByVal Value As Object)
            m_vScreenHeirarchy = Value
        End Set
    End Property

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
            m_lReturn = CType(gPMComponentServices.CheckDatabase(v_sUsername:=m_sUsername, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Set Username and Password
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
                If m_oSystemOption IsNot Nothing Then
                    m_oSystemOption.Dispose()
                    m_oSystemOption = Nothing
                End If

                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: Add (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Public Function Add() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' CTAF 191200 - Swapped the i/o parameters
                ' Add PrimaryKey as OUTPUT parameters
                m_lReturn = CType(AddKeyOutputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add the required INPUT parameters
                m_lReturn = CType(AddInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Get the Primary Key of the record inserted
                m_lReturn = CType(GetNewPrimaryKeyID(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            m_lReturn = CType(UpdateFileMaster(lMode:=1), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Update (Public)
    '
    ' Description: Updates a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            With m_oDatabase
                ' Clear the Database Parameters Collection
                .Parameters.Clear()
                m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                ' Add the required INPUT parameters
                m_lReturn = CType(AddInputParam(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored, lRecordsAffected:=lRecordsAffected)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                ' Check to see that the record was updated OK
                If lRecordsAffected > 0 Then
                    ' Updated No action required
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With
            m_lReturn = CType(UpdateFileMaster(lMode:=2), gPMConstants.PMEReturnCode)
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Delete (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    ' ***************************************************************** '
    Public Function Delete() As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Add PrimaryKey as INPUT parameters
                m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' If record wasn't deleted, error
                If lRecordsAffected > 0 Then
                    ' Deleted, No action required
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With
            m_lReturn = CType(UpdateFileMaster(lMode:=3), gPMConstants.PMEReturnCode)
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Delete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectSingle (Public)
    '
    ' Description: Selects the required SIRParty
    '
    ' ***************************************************************** '
    Public Function SelectSingle(Optional ByRef vLockMode As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            With m_oDatabase
                ' Clear the Database Parameters Collection
                .Parameters.Clear()
                ' Default to No Lock if not supplied or not numeric
                Dim dbNumericTemp As Double
                If (Informations.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                    vLockMode = gPMConstants.PMELockMode.PMNoLock
                End If
                ' Add PrimaryKey as INPUT parameters
                m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                If FromEvent Then
                    m_lReturn = .SQLSelect(sSQL:=ACSelectSingleEventSQL, sSQLName:=ACSelectSingleEventName, bStoredProcedure:=ACSelectSingleEventStored, bKeepNulls:=True)
                Else
                    m_lReturn = .SQLSelect(sSQL:=ACSelectSingleSQL, sSQLName:=ACSelectSingleName, bStoredProcedure:=ACSelectSingleStored, bKeepNulls:=True)
                End If
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                lRecordCount = .Records.Count()
                If lRecordCount = 1 Then
                Else
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If
                m_lReturn = CType(SetPropertiesFromDB(oFields:= .Records.Item(0).Fields()), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectSingle Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectSingle", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetPropertiesFromDB (Public)
    '
    ' Description: Sets the supplied SIRParty properties from a database
    '              record.
    ' ***************************************************************** '
    'developer guide no. 112 (Guide)
    Public Function SetPropertiesFromDB(ByRef oFields As DataRow) As Integer

        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            With oFields
                PartyCnt = oFields("party_cnt")
                PartyTypeID = oFields("party_type_id")
                If Convert.IsDBNull(oFields("is_Also_Agent")) Or Informations.IsNothing(oFields("is_Also_Agent")) Then
                    IsAlsoAgent = Nothing
                Else
                    IsAlsoAgent = oFields("is_also_agent")
                End If
                PartyStructureID = oFields("party_structure_id")
                SourceID = oFields("source_id")
                PartyID = oFields("party_id")
                Shortname = oFields("shortname")
                Name = oFields("name")

                If Convert.IsDBNull(oFields("resolved_name")) Or Informations.IsNothing(oFields("resolved_name")) Then
                    ResolvedName = Nothing
                Else
                    ResolvedName = oFields("resolved_name")
                End If
                CurrencyID = oFields("currency_id")
                LanguageID = oFields("language_id")

                If Convert.IsDBNull(oFields("collect_type_id")) Or Informations.IsNothing(oFields("collect_type_id")) Then
                    CollectTypeID = Nothing
                Else
                    CollectTypeID = oFields("collect_type_id")
                End If

                If Convert.IsDBNull(oFields("accum_treatment_type_id")) Or Informations.IsNothing(oFields("accum_treatment_type_id")) Then
                    AccumTreatmentTypeID = Nothing
                Else
                    AccumTreatmentTypeID = oFields("accum_treatment_type_id")
                End If

                If Convert.IsDBNull(oFields("stats_treatment_type_id")) Or Informations.IsNothing(oFields("stats_treatment_type_id")) Then
                    StatsTreatmentTypeID = Nothing
                Else
                    StatsTreatmentTypeID = oFields("stats_treatment_type_id")
                End If
                If Convert.IsDBNull(oFields("party_category_id")) Or Informations.IsNothing(oFields("party_category_id")) Then
                    PartyCategoryID = Nothing
                Else
                    PartyCategoryID = oFields("party_category_id")
                End If
                If Convert.IsDBNull(oFields("agent_cnt")) Or Informations.IsNothing(oFields("agent_cnt")) Then
                    AgentCnt = Nothing
                Else
                    AgentCnt = oFields("agent_cnt")
                End If
                If Convert.IsDBNull(oFields("consultant_cnt")) Or Informations.IsNothing(oFields("consultant_cnt")) Then
                    ConsultantCnt = Nothing
                Else
                    ConsultantCnt = oFields("consultant_cnt")
                End If
                CreatedByID = oFields("created_by_id")
                DateCreated = oFields("date_created")
                If Convert.IsDBNull(oFields("last_modified")) Or Informations.IsNothing(oFields("last_modified")) Then
                    LastModified = Nothing
                Else
                    LastModified = oFields("last_modified")
                End If
                If Convert.IsDBNull(oFields("modified_by_id")) Or Informations.IsNothing(oFields("modified_by_id")) Then
                    ModifiedByID = Nothing
                Else
                    ModifiedByID = oFields("modified_by_id")
                End If
                If Convert.IsDBNull(oFields("payment_method_code")) Or Informations.IsNothing(oFields("payment_method_code")) Then
                    PaymentMethodCode = Nothing
                Else
                    PaymentMethodCode = oFields("payment_method_code")
                End If
                If Convert.IsDBNull(oFields("payment_term_code")) Or Informations.IsNothing(oFields("payment_term_code")) Then
                    PaymentTermCode = Nothing
                Else
                    PaymentTermCode = oFields("payment_term_code")
                End If
                If Convert.IsDBNull(oFields("credit_card_code")) Or Informations.IsNothing(oFields("credit_card_code")) Then
                    CreditCardCode = Nothing
                Else
                    CreditCardCode = oFields("credit_card_code")
                End If
                If Convert.IsDBNull(oFields("file_code")) Or Informations.IsNothing(oFields("file_code")) Then
                    FileCode = Nothing
                Else
                    FileCode = oFields("file_code")
                End If
                If Convert.IsDBNull(oFields("abc_count")) Or Informations.IsNothing(oFields("abc_count")) Then
                    ABCCount = Nothing
                Else
                    ABCCount = oFields("abc_count")
                End If
                If Convert.IsDBNull(oFields("statements")) Or Informations.IsNothing(oFields("statements")) Then
                    Statements = Nothing
                Else
                    Statements = oFields("statements")
                End If
                If Convert.IsDBNull(oFields("reminder_type_id")) Or Informations.IsNothing(oFields("reminder_type_id")) Then
                    ReminderTypeId = Nothing
                Else
                    ReminderTypeId = oFields("reminder_type_id")
                End If
                If Convert.IsDBNull(oFields("renewals")) Or Informations.IsNothing(oFields("renewals")) Then
                    Renewals = Nothing
                Else
                    Renewals = oFields("renewals")
                End If
                If Convert.IsDBNull(oFields("status")) Or Informations.IsNothing(oFields("status")) Then
                    Status = Nothing
                Else
                    Status = oFields("status")
                End If
                If Convert.IsDBNull(oFields("last_action_type")) Or Informations.IsNothing(oFields("last_action_type")) Then
                    LastActionType = Nothing
                Else
                    LastActionType = oFields("last_action_type")
                End If
                If Convert.IsDBNull(oFields("is_travel_agent")) Or Informations.IsNothing(oFields("is_travel_agent")) Then
                    IsTravelAgent = Nothing
                Else
                    IsTravelAgent = oFields("is_travel_agent")
                End If
                If Convert.IsDBNull(oFields("is_prospect")) Or Informations.IsNothing(oFields("is_prospect")) Then
                    IsProspect = Nothing
                Else
                    IsProspect = oFields("is_prospect")
                End If
                If Convert.IsDBNull(oFields("is_deleted")) Or Informations.IsNothing(oFields("is_deleted")) Then
                    IsDeleted = Nothing
                Else
                    IsDeleted = oFields("is_deleted")
                End If
                If Convert.IsDBNull(oFields("abi_code_on_406")) Or Informations.IsNothing(oFields("abi_code_on_406")) Then
                    ABICodeOn406 = Nothing
                Else
                    ABICodeOn406 = oFields("abi_code_on_406")
                End If
                If Convert.IsDBNull(oFields("abi_code_on_81")) Or Informations.IsNothing(oFields("abi_code_on_81")) Then
                    ABICodeOn81 = Nothing
                Else
                    ABICodeOn81 = oFields("abi_code_on_81")
                End If

                If Convert.IsDBNull(oFields("abi_codelist")) Or Informations.IsNothing(oFields("abi_codelist")) Then
                    ABICodeList = Nothing
                Else
                    ABICodeList = oFields("abi_codelist")
                End If
                If Convert.IsDBNull(oFields("area_id")) Or Informations.IsNothing(oFields("area_id")) Then
                    AreaId = Nothing
                Else
                    AreaId = oFields("area_id")
                End If
                If Convert.IsDBNull(oFields("service_level_id")) Or Informations.IsNothing(oFields("service_level_id")) Then
                    ServiceLevelId = Nothing
                Else
                    ServiceLevelId = oFields("service_level_id")
                End If
                If Convert.IsDBNull(oFields("invariant_key")) Or Informations.IsNothing(oFields("invariant_key")) Then
                    InvariantKey = Nothing
                Else
                    InvariantKey = oFields("invariant_key")
                End If

                If Convert.IsDBNull(oFields("record_status")) Or Informations.IsNothing(oFields("record_status")) Then
                    RecordStatus = Nothing
                Else
                    RecordStatus = oFields("record_status")
                End If
                If Convert.IsDBNull(oFields("CCJs")) Or Informations.IsNothing(oFields("CCJs")) Then
                    CCJs = Nothing
                Else
                    CCJs = oFields("CCJs")
                End If

                If Convert.IsDBNull(oFields("user_defined_data_id")) Or Informations.IsNothing(oFields("user_defined_data_id")) Then
                    UserDefinedDataId = Nothing
                Else
                    UserDefinedDataId = oFields("user_defined_data_id")
                End If

                If Convert.IsDBNull(oFields("seasonal_gift_id")) Or Informations.IsNothing(oFields("seasonal_gift_id")) Then
                    SeasonalGiftID = Nothing
                Else
                    SeasonalGiftID = oFields("seasonal_gift_id")
                End If
                'DC 28/06/00

                If Convert.IsDBNull(oFields("correspondence_type_id")) Or Informations.IsNothing(oFields("correspondence_type_id")) Then
                    CorrespondenceTypeId = Nothing
                Else
                    CorrespondenceTypeId = oFields("correspondence_type_id")
                End If
                'Tomo060700
                If Convert.IsDBNull(oFields("renewal_stop_code_id")) Or Informations.IsNothing(oFields("renewal_stop_code_id")) Then
                    RenewalStopCodeId = Nothing
                Else
                    RenewalStopCodeId = oFields("renewal_stop_code_id")
                End If
                ' CTAF 250900
                If Convert.IsDBNull(oFields("swift_party_id")) Or Informations.IsNothing(oFields("swift_party_id")) Then
                    SwiftPartyID = Nothing
                Else
                    SwiftPartyID = oFields("swift_party_id")
                End If
                If Convert.IsDBNull(oFields("loyalty_number")) Or Informations.IsNothing(oFields("loyalty_number")) Then
                    LoyaltyNumber = Nothing
                Else
                    LoyaltyNumber = oFields("loyalty_number")
                End If
                If Convert.IsDBNull(oFields("alternative_identifier")) Or Informations.IsNothing(oFields("alternative_identifier")) Then
                    AlternativeIdentifier = Nothing
                Else
                    AlternativeIdentifier = oFields("alternative_identifier")
                End If

                If Convert.IsDBNull(oFields("marketing_segment_ind")) Or Informations.IsNothing(oFields("marketing_segment_ind")) Then
                    MarketingSegmentInd = Nothing
                Else
                    MarketingSegmentInd = oFields("marketing_segment_ind")
                End If
                If Convert.IsDBNull(oFields("trading_name")) Or Informations.IsNothing(oFields("trading_name")) Then
                    TradingName = Nothing
                Else
                    TradingName = oFields("trading_name")
                End If
                If Convert.IsDBNull(oFields("sub_branch_id")) Or Informations.IsNothing(oFields("sub_branch_id")) Then
                    SubBranchId = Nothing
                Else
                    SubBranchId = oFields("sub_branch_id")
                End If
                If Convert.IsDBNull(oFields("sub_branch_name")) Or Informations.IsNothing(oFields("sub_branch_name")) Then
                    SubBranchName = Nothing
                Else
                    SubBranchName = oFields("sub_branch_name")
                End If
                'FSA Phase III
                If Convert.IsDBNull(oFields("tob_letter")) Or Informations.IsNothing(oFields("tob_letter")) Then
                    TobLetter = CDate(#12/29/1899#)
                Else
                    TobLetter = oFields("tob_letter")
                End If
                'FSA Phase III End
                Override = gPMFunctions.ToSafeInteger(oFields("use_override_commission_rate"))
                OverrideRenewal = gPMFunctions.ToSafeInteger(oFields("use_override_commission_renewal"))
            End With
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "SetPropertiesFromDB Failed", ACApp, ACClass, "SetPropertiesFromDB", Informations.Err().Number, excep.Message, excep:=excep)
            Return result
        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' ***************************************************************** '
    ' Name: AddInputParam (Private)
    '
    ' Description: Adds all of the NON-KEY INPUT parameters
    '              required for an Insert or Update.
    '
    ' ***************************************************************** '
    Private Function AddInputParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        AddInputParameter("party_type_id", PartyTypeID, gPMConstants.PMEDataType.PMInteger)
        AddInputParameter("is_also_agent", IsAlsoAgent, gPMConstants.PMEDataType.PMInteger)

        AddInputParameter("party_structure_id", PartyStructureID, gPMConstants.PMEDataType.PMLong)
        AddInputParameter("source_id", SourceID, gPMConstants.PMEDataType.PMInteger)
        AddInputParameter("party_id", PartyID, gPMConstants.PMEDataType.PMLong)
        AddInputParameter("shortname", Shortname, gPMConstants.PMEDataType.PMString)
        AddInputParameter("name", Name, gPMConstants.PMEDataType.PMString)
        AddInputParameter("resolved_name", gPMFunctions.NullToString(ResolvedName), gPMConstants.PMEDataType.PMString)
        AddInputParameter("currency_id", CurrencyID, gPMConstants.PMEDataType.PMInteger)
        AddInputParameter("language_id", LanguageID, gPMConstants.PMEDataType.PMInteger)
        AddInputParameter("collect_type_id", CollectTypeID, gPMConstants.PMEDataType.PMInteger, True)
        AddInputParameter("accum_treatment_type_id", AccumTreatmentTypeID, gPMConstants.PMEDataType.PMLong, True)

        AddInputParameter("stats_treatment_type_id", StatsTreatmentTypeID, gPMConstants.PMEDataType.PMLong, True)
        AddInputParameter("party_category_id", PartyCategoryID, gPMConstants.PMEDataType.PMLong, True)
        AddInputParameter("agent_cnt", AgentCnt, gPMConstants.PMEDataType.PMLong, True)
        AddInputParameter("consultant_cnt", ConsultantCnt, gPMConstants.PMEDataType.PMLong, True)
        AddInputParameter("created_by_id", CreatedByID, gPMConstants.PMEDataType.PMInteger)
        AddInputParameter("date_created", DateCreated, gPMConstants.PMEDataType.PMDate)
        AddInputParameter("last_modified", gPMFunctions.NullToDate(DateTime.Now), gPMConstants.PMEDataType.PMDate) ' Use Now to have current date & time 
        AddInputParameter("modified_by_id", m_iUserID, gPMConstants.PMEDataType.PMInteger, True) 'Priya PN:77930
        AddInputParameter("payment_method_code", gPMFunctions.NullToString(PaymentMethodCode), gPMConstants.PMEDataType.PMString)

        If Not Informations.IsNumeric(PaymentTermCode) Then
            PaymentTermCode = DBNull.Value
        End If
        AddInputParameter("payment_term_code", PaymentTermCode, gPMConstants.PMEDataType.PMLong)

        AddInputParameter("credit_card_code", gPMFunctions.NullToString(CreditCardCode), gPMConstants.PMEDataType.PMString)
        AddInputParameter("file_code", gPMFunctions.NullToString(FileCode), gPMConstants.PMEDataType.PMString)
        AddInputParameter("abc_count", ABCCount, gPMConstants.PMEDataType.PMLong, True)
        AddInputParameter("statements", gPMFunctions.NullToLong(Statements), gPMConstants.PMEDataType.PMInteger)
        AddInputParameter("reminder_type_id", ReminderTypeId, gPMConstants.PMEDataType.PMLong, True)
        AddInputParameter("renewals", gPMFunctions.NullToLong(Renewals), gPMConstants.PMEDataType.PMInteger)
        AddInputParameter("status", gPMFunctions.NullToString(Status), gPMConstants.PMEDataType.PMString)
        AddInputParameter("last_action_type", gPMFunctions.NullToString(LastActionType), gPMConstants.PMEDataType.PMString)
        AddInputParameter("is_travel_agent", gPMFunctions.NullToLong(IsTravelAgent), gPMConstants.PMEDataType.PMInteger)
        AddInputParameter("is_prospect", gPMFunctions.NullToLong(IsProspect), gPMConstants.PMEDataType.PMInteger)

        AddInputParameter("is_deleted", gPMFunctions.NullToLong(IsDeleted), gPMConstants.PMEDataType.PMInteger)
        AddInputParameter("abi_code_on_406", gPMFunctions.NullToString(ABICodeOn406), gPMConstants.PMEDataType.PMString)
        AddInputParameter("abi_code_on_81", gPMFunctions.NullToString(ABICodeOn81), gPMConstants.PMEDataType.PMString)
        AddInputParameter("abi_codelist", gPMFunctions.NullToString(ABICodeList), gPMConstants.PMEDataType.PMString)

        'developer guide no. 131
        If Not AreaId Is DBNull.Value Then
            AddInputParameter("area_id", If(AreaId = 0, DBNull.Value, AreaId), gPMConstants.PMEDataType.PMLong)
        Else
            AddInputParameter("area_id", AreaId, gPMConstants.PMEDataType.PMLong)
        End If
        AddInputParameter("service_level_id", gPMFunctions.NullToLong(ServiceLevelId), gPMConstants.PMEDataType.PMLong)
        AddInputParameter("invariant_key", gPMFunctions.NullToLong(InvariantKey), gPMConstants.PMEDataType.PMLong)
        AddInputParameter("record_status", gPMFunctions.NullToString(RecordStatus), gPMConstants.PMEDataType.PMString)
        AddInputParameter("CCJs", gPMFunctions.NullToLong(CCJs), gPMConstants.PMEDataType.PMLong)
        AddInputParameter("user_defined_data_id", UserDefinedDataId, gPMConstants.PMEDataType.PMLong, False, True)

        If Not SeasonalGiftID Is DBNull.Value Then
            AddInputParameter("seasonal_gift_id", If(SeasonalGiftID = 0, DBNull.Value, SeasonalGiftID), gPMConstants.PMEDataType.PMLong)
        Else
            AddInputParameter("seasonal_gift_id", SeasonalGiftID, gPMConstants.PMEDataType.PMLong)
        End If

        AddInputParameter("correspondence_type_id", CorrespondenceTypeId, gPMConstants.PMEDataType.PMLong)
        AddInputParameter("renewal_stop_code_id", RenewalStopCodeId, gPMConstants.PMEDataType.PMLong, True)

        'developer guide no. 131
        If Not SwiftPartyID Is DBNull.Value Then
            AddInputParameter("swift_party_id", If(SwiftPartyID = 0, DBNull.Value, SwiftPartyID), gPMConstants.PMEDataType.PMLong)
        Else
            AddInputParameter("swift_party_id", SwiftPartyID, gPMConstants.PMEDataType.PMLong)
        End If
        AddInputParameter("loyalty_number", gPMFunctions.NullToString(LoyaltyNumber), gPMConstants.PMEDataType.PMString)
        AddInputParameter("alternative_identifier", gPMFunctions.NullToString(AlternativeIdentifier), gPMConstants.PMEDataType.PMString)
        AddInputParameter("marketing_segment_ind", gPMFunctions.NullToString(MarketingSegmentInd), gPMConstants.PMEDataType.PMString)
        AddInputParameter("trading_name", gPMFunctions.NullToString(TradingName), gPMConstants.PMEDataType.PMString)
        AddInputParameter("sub_branch_id", SubBranchId, gPMConstants.PMEDataType.PMLong)
        AddInputParameter("tob_letter", TobLetter, gPMConstants.PMEDataType.PMDate) 'FSA Phase III
        AddInputParameter("UserId", m_iUserID, gPMConstants.PMEDataType.PMLong)
        AddInputParameter("UniqueId", gPMFunctions.NullToString(m_vUniqueId), gPMConstants.PMEDataType.PMString)
        AddInputParameter("ScreenHierarchy", gPMFunctions.NullToString(m_vScreenHeirarchy), gPMConstants.PMEDataType.PMString)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddKeyInputParam (Private)
    '
    ' Description: Adds all of the PRIMARY KEY INPUT parameters
    '              required for a Select, Delete or Update.
    '
    ' ***************************************************************** '
    Private Function AddKeyInputParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(PartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddKeyOutputParam (Private)
    '
    ' Description: Adds all of the PRIMARY KEY OUTPUT parameters
    '              required for an Add.
    '
    ' ***************************************************************** '
    Private Function AddKeyOutputParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = .Parameters.Add(sName:="party_shortname", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetNewPrimaryKeyID (Private)
    '
    ' Description: Returns the new PRIMARY KEY values from an Add.
    '
    ' ***************************************************************** '
    Private Function GetNewPrimaryKeyID() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            PartyCnt = .Parameters.Item("party_cnt").Value

            If .Parameters.Item("party_shortname").Value <> "" Then
                Dim sShortname As String = .Parameters.Item("party_shortname").Value
                m_sShortname.Value = sShortname.Trim() & New String(" ", 20)

            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateFileMaster
    '
    ' Description:
    '
    ' History: 03/09/1999 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateFileMaster(ByRef lMode As Object) As Integer

        Dim oSIRDOCAPI As Object = Nothing
        Dim result As Integer = 0
        Dim iOptionNumber As Integer
        Dim sOptionValue As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        iOptionNumber = 10
        sOptionValue = ""

        m_lReturn = CType(GetOption(r_iOptionNumber:=iOptionNumber, v_sOptionValue:=sOptionValue), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        'Not set up - do nothing
        If sOptionValue.Trim() = "0" Then
            Return result
        End If

        m_lReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=oSIRDOCAPI, v_sClassName:="bSIRDOCAPI.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(UpdateFileMaster, "Failed to create business object bSIRDOCAPI.Form", gPMConstants.PMELogLevel.PMLogError)
        End If



        'eck010201 passed party_sourceid not global source_id
        m_lReturn = oSIRDOCAPI.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(SourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ACApp, vDatabase:=CType(m_oDatabase, dPMDAO.Database))

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the DOC API object", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFileMaster", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If

        'DN 02/04/01 - Changes made to SIRDOCAPI to use InsFolderCnt instead of InsFileCnt
        m_lReturn = oSIRDOCAPI.ProcessIndex(lMode:=lMode, iSourceID:=ToSafeInteger(SourceID), lPartyID:=ToSafeInteger(m_lPartyCnt), sPartyName:=ToSafeString(m_sShortname.Value), lInsuranceFolderId:=0, sInsuranceFileRef:="", lClaimId:=0, sClaimRef:="")
        If oSIRDOCAPI IsNot Nothing Then
            oSIRDOCAPI.Dispose()
            oSIRDOCAPI = Nothing
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetOption (Private)
    '
    ' Description: Get an option.
    '
    ' ***************************************************************** '
    Private Function GetOption(ByRef r_iOptionNumber As Integer, ByRef v_sOptionValue As String) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        If m_oSystemOption Is Nothing Then
            m_oSystemOption = New bSIROptions.Business()

            m_lReturn = m_oSystemOption.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the system option object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If
        End If

        m_lReturn = m_oSystemOption.GetOption(iOptionNumber:=r_iOptionNumber, sValue:=v_sOptionValue)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the event", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If

        Return result

    End Function
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        ' Class Initialise

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' **************************************************************** '
    ' Add parameters the easy way
    '
    ' PWF 10/08/2002 - If you don't believe me look at the difference
    '                  it makes to AddInputParam
    ' **************************************************************** '
    Private Sub AddInputParameter(ByVal sParameterName As String, ByVal vValue As Object, ByVal lDataType As gPMConstants.PMEDataType, Optional ByVal bCheckPositive As Boolean = False, Optional ByVal bCheckEmpty As Boolean = False)

        ' No error handling, let them filter up

        ' If the number is not positive convert to null
        If bCheckPositive Then

            If ToSafeDouble(vValue) < 1 Then


                vValue = DBNull.Value
            End If
        End If

        ' If the number is empty convert to null
        If bCheckEmpty Then

            If Object.Equals(vValue, Nothing) Then


                vValue = DBNull.Value
            End If
        End If
        'developer guide no. 105 (Guide)
        If vValue Is Nothing Then
            vValue = DBNull.Value
        End If


        'developer guide no. 98 (Guide)		
        m_lReturn = m_oDatabase.Parameters.Add(sName:=sParameterName, vValue:=vValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=CType(lDataType, gPMConstants.PMEDataType))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(Informations.Err().Number.ToString() + ", AddInputParameter, " + "Unable to add parameter '" & sParameterName & "'" & Strings.ChrW(13) & Strings.ChrW(10) & Informations.Err().Description)
        End If

    End Sub
End Class

