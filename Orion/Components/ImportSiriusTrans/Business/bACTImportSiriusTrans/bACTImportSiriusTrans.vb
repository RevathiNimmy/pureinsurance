Option Strict Off
Option Explicit On
Imports System.IO
Imports System.Text
Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports Microsoft.Practices.EnterpriseLibrary.Caching.Expirations
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 05/10/1997
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a Statement.
    '
    ' Edit History: TF261198 - PostDocument() changed test on DocType
    '               DeriveAccountID() removed return test on call to Sub
    ' RAW 25/02/2003 : ISS2419 : improved error message when PostDocument fails
    ' CJB 25/04/2005 : PN20443 : PostDocument changed (for broking) to use transaction
    '                  date, not effective date when getting the currency rates.
    ' CJB 06/07/2005 : PN22188 : Changed GetDestinationAccountId (and its call) - general
    '                  tidy up and cater for no Map To Income a/c for the comm a/c
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' ************************************************
    ' Added to replace global variables 29/01/2004
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    Private m_oS4BDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

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
    'Underwriting or broking?
    Private m_sUnderwritingOrAgency As String = ""

    Private m_oAccount As bACTAccount.Form
    Private m_oDocumentPost As bACTDocumentPost.Form
    Private m_oCurrencyConvert As bACTCurrencyConvert.Form
    Private m_oExplorer As bACTExplorer.Form
    Private m_oPeriod As bACTPeriod.Form
    Private m_oCurrency As bACTCurrency.Form
    Private m_oTransdetail As bACTTransdetail.Form

    ' NavigatorV3 variables
    Private m_lPMAuthorityLevel As Integer
    Private m_lStatus As Integer
    Private m_sCommissionOption As String = ""
    Private m_lTransactionId As Integer
    Private m_oSystemOption As bSIROptions.Business
    Private m_oCommissionPost As bACTCommissionPost.Business
    Private m_lPostingPeriodNumber As Integer
    Private m_lTransactionExportFolderCnt As Integer
    Private m_lJournalExportFolderCnt As Integer
    Private m_sAccountingBasisOption As String = ""
    Private m_oCommissionPostTax As bACTCommissionPost.Business

    'PN37017 When Premium is zero and earned commission is set when client pays then set this property to TRUE
    Private m_bIsPostCommission As Boolean

    Public Shared iCache As ICacheManager
    Private m_sCachePath As String
    Dim oFeeType As Object

    Public WriteOnly Property TransactionExportFolderCnt() As Integer
        Set(ByVal Value As Integer)
            m_lTransactionExportFolderCnt = Value
        End Set
    End Property

    Public WriteOnly Property JournalExportFolderCnt() As Integer
        Set(ByVal Value As Integer)

            m_lJournalExportFolderCnt = Value

        End Set
    End Property

    Public Property PostingPeriodNumber() As Integer
        Get
            Return m_lPostingPeriodNumber
        End Get
        Set(ByVal Value As Integer)
            m_lPostingPeriodNumber = Value
        End Set
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
        End Get
    End Property

    Public Property CallingAppName() As String
        Get
            Return m_sCallingAppName
        End Get
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property

    Public Property PMAuthorityLevel() As Integer
        Get
            Return m_lPMAuthorityLevel
        End Get
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
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

    Public WriteOnly Property IsPostCommission() As Boolean
        Set(ByVal Value As Boolean)

            m_bIsPostCommission = Value

        End Set
    End Property
    ' ***************************************************************** '
    ' Name: SetKeys
    '
    ' Description: Navigator SetKeys function.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray As Object) As Integer

        Dim result As Integer = 0
        Try

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeysFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys
    '
    ' Description: Navigator GetKeys function.
    '
    ' ***************************************************************** '

    Public Function GetKeys(ByRef vKeyArray As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            vKeyArray = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeysFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSummary
    '
    ' Description: GetSummary Navigator function.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer

        Dim result As Integer = 0
        Try

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummaryFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetOption (Private)
    '
    ' Description: Get an option.
    '
    ' ***************************************************************** '
    Public Function GetOption(ByVal v_iOptionNumber As Integer, ByRef r_sOptionValue As String, Optional ByRef vDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim bCloseDatabase As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oSystemOption Is Nothing Then
                ' Get Reference to Database
                'SD 24/07/2002 rename variable
                ' PWF 16/10/2002 - Must not use module level CloseDatabase flag!!!

                m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_bNewInstanceCreated:=bCloseDatabase, r_oCheckedDatabase:=m_oS4BDatabase, v_vDatabase:=vDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="gPMComponentServices.CheckDatabase Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'Get Instance of System Option Business
                'SD 24/07/2002 rename variable

                m_oSystemOption = New bSIROptions.Business
                m_lReturn = m_oSystemOption.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oS4BDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="gPMComponentServices.CreateBusinessObject Failed for bSIROptions.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            m_lReturn = m_oSystemOption.GetOption(iOptionNumber:=v_iOptionNumber, sValue:=r_sOptionValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the option number " & v_iOptionNumber, vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption")

                Return result
            End If

            m_oSystemOption.Dispose()
            m_oSystemOption = Nothing
            m_lReturn = m_oS4BDatabase.CloseDatabase()

            m_oS4BDatabase = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Start
    '
    ' Description: Navigator Start function. Entry point into Navigator.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            ' Get Reference to Database
            'SD 24/07/2002 rename variable

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="gPMComponentServices.CheckDatabase Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Current Record to zero etc.
            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now
            'Decide if we are underwriting or agency - for use later
            m_lReturn = bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_sUnderwritingOrAgency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Get Underwriting or Agency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'SD 24/07/2002 rename variable

            m_oAccount = New bACTAccount.Form
            m_lReturn = m_oAccount.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="gPMComponentServices.CreateBusinessObject Failed for bACTAccount.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'SD 24/07/2002 rename variable

            m_oExplorer = New bACTExplorer.Form
            m_lReturn = m_oExplorer.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="gPMComponentServices.CreateBusinessObject Failed for bACTExplorer.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'SD 24/07/2002 rename variable

            m_oCurrencyConvert = New bACTCurrencyConvert.Form
            m_lReturn = m_oCurrencyConvert.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="gPMComponentServices.CreateBusinessObject Failed for bACTCurrencyConvert.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oCurrency = New bACTCurrency.Form
            m_lReturn = m_oCurrency.Initialise(sUserName:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="gPMComponentServices.CreateBusinessObject Failed for bACTCurrency.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'RKC 23/08/2002
            ' Create Period Business Object
            'Servicing Period Specific Functionality

            m_oPeriod = New bACTPeriod.Form
            m_lReturn = m_oPeriod.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="gPMComponentServices.CreateBusinessObject Failed for bACTPeriod.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set m_oDocumentPost = GetOrionBusiness(v_sClassName:="bACTDocumentPost.Form", v_vDatabase:=m_oDatabase)
            'SD 24/07/2002 rename variable

            m_oDocumentPost = New bACTDocumentPost.Form
            m_lReturn = m_oDocumentPost.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="gPMComponentServices.CreateBusinessObject Failed for bACTDocumentPost.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            m_oTransdetail = New bACTTransdetail.Form
            m_lReturn = m_oTransdetail.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="gPMComponentServices.CreateBusinessObject Failed for bACTTransDetail.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Get the Commission Transfer settings
            m_lReturn = GetOption(v_iOptionNumber:=16, r_sOptionValue:=m_sCommissionOption)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then


                m_sCommissionOption = ""
            End If

            Try
                iCache = CacheFactory.GetCacheManager("PureCache")
            Catch ex As Exception

            End Try

            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine,
                                                   v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture,
                                                   v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer,
                                                   v_sSettingName:=gPMConstants.PMRegKeyCachePath, r_sSettingValue:=m_sCachePath)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Right(m_sCachePath, 1) <> "\" Then
                m_sCachePath += "\"
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
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing

                If m_oAccount IsNot Nothing Then
                    m_oAccount.Dispose()
                    m_oAccount = Nothing
                End If
                If m_oDocumentPost IsNot Nothing Then
                    m_oDocumentPost.Dispose()
                    m_oDocumentPost = Nothing
                End If
                If m_oExplorer IsNot Nothing Then
                    m_oExplorer.Dispose()
                    m_oExplorer = Nothing
                End If
                If m_oCurrencyConvert IsNot Nothing Then
                    m_oCurrencyConvert.Dispose()
                    m_oCurrencyConvert = Nothing
                End If
                If m_oCurrency IsNot Nothing Then
                    m_oCurrency.Dispose()
                    m_oCurrency = Nothing
                End If
                If m_oPeriod IsNot Nothing Then
                    m_oPeriod.Dispose()
                    m_oPeriod = Nothing
                End If
                If m_oTransdetail IsNot Nothing Then
                    m_oTransdetail.Dispose()
                    m_oTransdetail = Nothing
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

            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function GetSuspendedCommissionTransactions(ByVal sAgentType As String, ByVal lInsuranceFileCnt As Integer, ByRef r_vSuspenseArray(,) As Object) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetSuspendedTransactions
        ' PURPOSE:
        ' AUTHOR: Danny Davis
        ' DATE: 05 October 2006, 16:32:29
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()

            .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            .Parameters.Add(sName:="agent_type", vValue:=sAgentType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = .SQLSelect(sSQL:="spu_ACT_Get_Suspended_Commission", sSQLName:="Get Suspended Commission", bStoredProcedure:=True, vResultArray:=r_vSuspenseArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", spu_ACT_Get_Suspended_Commission failed.")
            End If
        End With


        Return result

    End Function
    ''' <summary>
    ''' PostDocument
    ''' </summary>
    ''' <param name="v_sDocRef"></param>
    ''' <param name="v_sDocDebitCredit"></param>
    ''' <param name="v_sDocTransactionTypeCode"></param>
    ''' <param name="v_dtDocDate"></param>
    ''' <param name="v_dtDocAccountingDate"></param>
    ''' <param name="v_sDocComments"></param>
    ''' <param name="v_sDocCurrencyCode"></param>
    ''' <param name="v_sDocBusinessTypeCode"></param>
    ''' <param name="v_sDocInsuranceRef"></param>
    ''' <param name="v_sDocProductCode"></param>
    ''' <param name="v_sDocBranchCode"></param>
    ''' <param name="v_sDocLeadAgentShortName"></param>
    ''' <param name="v_sDocInsuranceHolderShortName"></param>
    ''' <param name="v_dtDocInsuranceEffectiveDate"></param>
    ''' <param name="v_iDocOperatorID"></param>
    ''' <param name="v_vTransactionsArray"></param>
    ''' <param name="r_lDocPostedStatus"></param>
    ''' <param name="v_iDocSourceID"></param>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <param name="v_sReason"></param>
    ''' <param name="r_vNewDocumentId"></param>
    ''' <param name="v_vTermsOfPaymentId"></param>
    ''' <param name="v_vPaymentDueDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PostDocument(ByVal v_sDocRef As String, ByVal v_sDocDebitCredit As String, ByVal v_sDocTransactionTypeCode As String,
                                 ByVal v_dtDocDate As Date, ByVal v_dtDocAccountingDate As Date, ByVal v_sDocComments As String,
                                 ByVal v_sDocCurrencyCode As String, ByVal v_sDocBusinessTypeCode As String, ByVal v_sDocInsuranceRef As String,
                                 ByVal v_sDocProductCode As String, ByVal v_sDocBranchCode As String, ByVal v_sDocLeadAgentShortName As String,
                                 ByVal v_sDocInsuranceHolderShortName As String, ByVal v_dtDocInsuranceEffectiveDate As Date,
                                 ByVal v_iDocOperatorID As Object, ByVal v_vTransactionsArray(,) As Object, ByRef r_lDocPostedStatus As Integer,
                                 ByVal v_iDocSourceID As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_sReason As String,
                                 Optional ByRef r_vNewDocumentId As Object = Nothing, Optional ByRef v_vTermsOfPaymentId As Object = Nothing,
                                 Optional ByRef v_vPaymentDueDate As Object = Nothing, Optional ByRef r_sfailureReason As String = "") As Integer

        Const kINVALID_PERIOD As Integer = 0

        Dim nResult As Integer
        Dim bTransactionOpen As Boolean = False

        Dim aResults As Array = Nothing
        Dim nDocumentId As Integer = 0
        Dim sDocumentRef As New StringsHelper.FixedLengthString(25)
        Dim sDocumentType As New StringsHelper.FixedLengthString(3)
        Dim nDocumentTYpeID As Integer = 0
        Dim dtDocumentDate As Date
        Dim dtAccountingDate As Date
        Dim sDocComment As String
        Dim sCurrencyCode As New StringsHelper.FixedLengthString(10)
        Dim nAccountId As Integer = 0
        Dim nCurrencyID As Integer = 0

        Dim dBaseAmountUnrounded As Double = 0

        Dim sdCurrencyAmountUnrounded As String = ""

        Dim nEuroCurrencyID As Integer = 0
        Dim crEuroAmount As Decimal = 0
        Dim odEuroBaseXrate As Object = Nothing
        Dim odEuroCcyXrate As Object = Nothing
        Dim sTransComment As String = ""

        Dim vInsuranceRef As String = ""
        Dim oOperatorID As Object = Nothing
        Dim oPurchaseOrderNo As Object = Nothing
        Dim oPurchaseInvoiceNo As Object = Nothing
        Dim oDepartment As Object = Nothing
        Dim oSpare As Object

        Dim dtRefDate As Date
        Dim crRefAmount As Decimal
        Dim crRefQuantity As Decimal

        Dim sRefUnits As New StringsHelper.FixedLengthString(30)
        Dim nParentNodeId As Integer
        Dim sRelativeCode As String

        Dim sLedgerCode As New StringsHelper.FixedLengthString(2)
        Dim sAccountTypeCode As New StringsHelper.FixedLengthString(10)
        Dim nDeleteAtPurge As Integer = 0

        Dim nTransdetailId As Integer = 0
        Dim nDocSequence As Integer
        Dim oTransIds() As Object = Nothing
        Dim sSQL As String

        Dim nPrimarys As Integer
        Dim nSecondarys As Integer

        Dim nLedgerID As Integer = 0
        Dim nPeriodID As Integer = 0
        Dim crRoundingError As Decimal
        Dim nSubBranchId As Integer = 0

        Dim oPremiumFinanceCnt As Object = Nothing
        Dim oPremiumFinanceVer As Object = Nothing
        Dim oFinancePlanArray As Object = Nothing

        Dim oSuspenseArray(,) As Object = Nothing
        Dim nSuspended As Integer
        Dim nReleaseToIncome As Integer
        Dim sReleaseAccountCode As String
        Dim nDestinationAccountId As Integer
        Dim sTransdetailTypeCode As String
        Dim nTransdetailTypeID As Integer

        Dim bTrueMonthlyPolicy As Boolean
        Dim nLeadMonthCycle As Integer
        Dim nSubMonthCycle As Integer
        Dim bLeadConsolidate As Boolean
        Dim bSubConsolidate As Boolean
        Dim sTranasactionType As String
        Dim nRenewalCount As Integer
        Dim bTransactionExportComplete As Boolean
        Dim nAgentAccountID As Integer
        Dim dCurrencyBaseXrate As Double
        Dim dtCurrencyBaseDate As Date
        Dim dAccountBaseXrate As Double
        Dim dtAccountBaseDate As Date
        Dim dSystemBaseXrate As Double
        Dim dtSystemBaseDate As Date
        Dim oPostedAmounts As Object = Nothing
        Dim sPaymentMethod As String = ""
        Dim sBalancetype As String = ""
        Dim oUnderwritingYearID As Object = Nothing
        Dim oTaxGroupID As Object = Nothing
        Dim oTaxBandID As Object = Nothing
        Dim oValue As Object = Nothing
        Dim nRiskTransferAgreement As Integer
        Dim oRiskTransfer As Object = Nothing
        Dim sAltReference As String = ""
        Dim dtDueDate As Date

        Dim bManuallyReleased As Boolean
        Dim bReleasedOnFullSettlement As Boolean
        Dim bReleasedForWholePosting As Boolean
        Dim bReleasedOnPolicyEffective As Boolean
        Dim bValid As Boolean
        Dim nCnt As Integer
        Dim sContent(1) As String
        Dim sCacheFilename As String = ""
        Dim sFilePath As String = ""
        Dim nStatsCurrencyXrate As Double

        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            sDocumentRef.Value = v_sDocRef.Trim()

            If sDocumentRef.Value.Substring(0, 2) = gACTLibrary.ACTAutoNumberRangeCodeJn Then
                m_lJournalExportFolderCnt = m_lTransactionExportFolderCnt
                m_lTransactionExportFolderCnt = 0
            End If

            If m_lTransactionExportFolderCnt > 0 Then

                'Check to see if these transactions have already been exported
                nResult = CheckTransactionExportComplete(v_lTransactionExportFolderCnt:=m_lTransactionExportFolderCnt, r_bTransactionExportComplete:=bTransactionExportComplete)

                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CheckTransactionExportComplete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                    r_lDocPostedStatus = gPMConstants.PMEReturnCode.PMFail
                    Return nResult
                End If

                If bTransactionExportComplete Then
                    r_lDocPostedStatus = gPMConstants.PMEReturnCode.PMSucceed
                    Return nResult
                End If
            End If

            ' Set the default return status to fail
            ' Set to PMSucceed once complete
            r_lDocPostedStatus = gPMConstants.PMEReturnCode.PMFail

            'Setup Document parameters

            sDocumentRef.Value = v_sDocRef.Trim()
            If m_lJournalExportFolderCnt <> 0 Then
                If sDocumentRef.Value.Substring(0, 2) = gACTLibrary.ACTAutoNumberRangeCodeJn Then
                    sDocumentType.Value = gACTLibrary.ACTAutoNumberRangeCodeJn
                Else
                    sDocumentType.Value = sDocumentRef.Value.Substring(0, 3)
                End If
            Else
                sDocumentType.Value = sDocumentRef.Value.Substring(0, 3)
            End If
            Dim sKey As String = "Cache_documenttype_" & sDocumentType.Value.Trim
            If Not iCache Is Nothing AndAlso iCache.Contains(sKey) AndAlso Not String.IsNullOrEmpty(Convert.ToString(iCache.GetData(sKey))) Then
                nDocumentTYpeID = iCache.GetData(sKey)
            End If

            If nDocumentTYpeID = 0 Then
                With m_oDatabase

                    .Parameters.Clear()
                    nResult = .Parameters.Add(sName:="code", vValue:=sDocumentType.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.Parameters.Add failed for code value = " & sDocumentType.Value, vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    sSQL = "SELECT documenttype_id from documenttype WHERE code = {code}"
                    nResult = .SQLSelect(sSQL:=sSQL, sSQLName:="GetDocumentType", bStoredProcedure:=False, vResultArray:=aResults)

                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="SELECT documenttype_id from documenttype failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If Not Informations.IsArray(aResults) Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to get documenttype_id for code " & sDocumentType.Value, vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    nDocumentTYpeID = CInt(aResults(0, 0))
                End With
            End If

            ' Add them to the Cache
            sFilePath = m_sCachePath + sCacheFilename + ".xml"

            If Not FileExists(sFilePath) Then
                Dim fileIO As FileStream
                fileIO = File.Create(sFilePath)
                fileIO.Close()
                File.WriteAllLines(sFilePath, sContent)
            End If
            ' Add the key to the SIRIUS_CACHE_KEYS   Cache Array, to be used by
            ' Sirius Cache Controller
            If Not iCache Is Nothing Then
                Dim atTimePeriod As AbsoluteTime = New AbsoluteTime(TimeSpan.FromMinutes(10))
                iCache.Add(sKey, nDocumentTYpeID, CacheItemPriority.Normal, Nothing, New FileDependency(sFilePath), atTimePeriod)
            End If

            With m_oDatabase
                If sDocumentType.Value.ToUpper() <> "FEE" Then 'eck300904 - Client Fees don't have insurance_file_cnt

                    'Get the Insurance File Payment Method
                    .Parameters.Clear()

                    nResult = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="spe_Insurance_File_sel failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    nResult = .SQLSelect(sSQL:="spe_Insurance_File_sel", sSQLName:="Get Insurance File Payment Method", bStoredProcedure:=True, vResultArray:=aResults)

                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="spe_Insurance_File_sel failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If Not Informations.IsArray(aResults) Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to get payment method for insurance file " & v_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Get the payment method
                    sPaymentMethod = gPMFunctions.NullToString(aResults(71, 0)).Trim()
                    sBalancetype = gPMFunctions.NullToString(aResults(103, 0)).Trim()
                    sAltReference = gPMFunctions.NullToString(aResults(76, 0)).Trim()
                End If

                If (sDocumentType.Value.ToString.ToUpper) = "CLC" Then

                    .Parameters.Clear()

                    nResult = .Parameters.Add(sName:="DocumentRef", vValue:=CStr(sDocumentRef.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="spu_GetStatsCurrencyXRate failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    nResult = .SQLSelect(sSQL:=ACGetStatsCurrencyXRateSQL, sSQLName:=ACGetStatsCurrencyXRateName, bStoredProcedure:=ACGetStatsCurrencyXRateStored, vResultArray:=aResults)

                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=ACGetStatsCurrencyXRateSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If Not Informations.IsArray(aResults) Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to get currency rate " & v_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    nStatsCurrencyXrate = ToSafeDouble(aResults(0, 0), 1)

                End If

            End With


            dtDocumentDate = DateTime.Now
            dtAccountingDate = DateTime.Now
            sDocComment = v_sDocComments.Trim()

            'Set up the Amounts Array
            'Index 0 - holds account_id, 1 holds transdetail_id, 2 holds base amount
            'ReDim oPostedAmounts(2, v_vTransactionsArray.GetUpperBound(1))

            ' Setup the Transaction Array to match incoming, but needs to start at 1
            'BB Not ideal starting at 1 should fix bACTDocument method
            nPrimarys = 0
            For iRow As Integer = v_vTransactionsArray.GetLowerBound(1) To v_vTransactionsArray.GetUpperBound(1)

                If CStr(v_vTransactionsArray(ACTBatchConst.ACTTransImportLedgerCode, iRow)).Trim() = SIRSalesTransactionLedgerCode Then
                    nPrimarys += 1

                    'RWH(21/06/01) For UW we want to use the PeriodID we already have on
                    'transaction_export_folder. Property only set by UW.
                    If m_lPostingPeriodNumber > 0 Then
                        nPeriodID = m_lPostingPeriodNumber
                    Else

                        'as S4B does not post in sequence
                        nResult = m_oDocumentPost.GetLedgerIdForAccountId(v_vAccountID:=v_vTransactionsArray(ACTBatchConst.ACTTransImportAccountID, iRow), r_vLedgerID:=nLedgerID)
                        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDocumentPost.GetLedgerIdForAccountId Failed for account " & CStr(v_vTransactionsArray(ACTBatchConst.ACTTransImportAccountID, iRow)), vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                            Return nResult
                        End If

                        nResult = m_oDocumentPost.GetPeriodIdForDate(r_lPeriodId:=nPeriodID, v_dtAccountingDate:=dtAccountingDate, lLedgerID:=nLedgerID)

                        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDocumentPost.GetPeriodIdForDate Failed for date " & dtAccountingDate, vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                            Return nResult
                        End If
                    End If

                    m_oDocumentPost.PeriodID = nPeriodID
                    '
                Else
                    'Attempt to fill in the Account ID - this reduces the size of the transaction

                    If CDbl(v_vTransactionsArray(ACTBatchConst.ACTTransImportAccountKey, iRow)) > 0 Then
                        nAccountId = CInt(v_vTransactionsArray(ACTBatchConst.ACTTransImportAccountID, iRow))
                    Else
                        nResult = m_oAccount.GetAccountID(v_vTransactionsArray(ACTBatchConst.ACTTransImportRelativeCode, iRow), nAccountId, nDeleteAtPurge)

                        If nResult = gPMConstants.PMEReturnCode.PMTrue Then
                            If Math.Abs(nDeleteAtPurge) = gPMConstants.PMEReturnCode.PMTrue Then 'account is set for deletion

                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=CStr(v_vTransactionsArray(ACTBatchConst.ACTTransImportRelativeCode, iRow)) &
                                                   " - Account set for deletion", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                                nResult = nResult
                                Return nResult
                            End If
                        Else
                            'Need to pass in SubBranchId
                            'Get SubBranchId from Period Id
                            'Get PeriodId from Accounting Date

                            If Convert.ToInt32(m_oDocumentPost.PeriodID) = kINVALID_PERIOD Then

                                m_oPeriod.GetPeriodForDate(dtDateInPeriod:=dtAccountingDate, lPeriodID:=nPeriodID)

                                If nPeriodID = kINVALID_PERIOD Then
                                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetPeriodForDate for PostDocument.", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                                    nResult = gPMConstants.PMEReturnCode.PMFalse
                                    Return nResult
                                Else
                                    'Set Period Id
                                    m_oDocumentPost.PeriodID = nPeriodID
                                End If
                            End If

                            'Need to pass in Ledger Id and Not Ledger Code
                            'Get Ledger Id from SubBranchId

                            nResult = bACTFunc.GetSubBranchID(m_oDatabase, r_lSubBranchID:=nSubBranchId, v_vPeriodID:=m_oDocumentPost.PeriodID)

                            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetSubBranchID for PostDocument.", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                                nResult = gPMConstants.PMEReturnCode.PMFalse
                                Return nResult
                            End If

                            nParentNodeId = CInt(v_vTransactionsArray(ACTBatchConst.ACTTransImportParentNode, iRow))
                            sRelativeCode = CStr(v_vTransactionsArray(ACTBatchConst.ACTTransImportRelativeCode, iRow))
                            sAccountTypeCode.Value = CStr(v_vTransactionsArray(ACTBatchConst.ACTTransImportAccountTypeCode, iRow)).Trim()
                            sLedgerCode.Value = CStr(v_vTransactionsArray(ACTBatchConst.ACTTransImportLedgerCode, iRow)).Trim()

                            'Get Ledger Id from SubBranchId
                            nResult = bACTFunc.GetLedgerIDFromShortName(m_oDatabase, nLedgerID, sLedgerCode.Value, nSubBranchId)

                            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetLedgerID for PostDocument.", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                                nResult = gPMConstants.PMEReturnCode.PMFalse
                                Return nResult
                            End If

                            nResult = DeriveAccountID(r_lAccountId:=nAccountId, v_lParentNodeId:=nParentNodeId, v_sRelativeCode:=sRelativeCode,
                                                        v_iAccountType:=gPMFunctions.ToSafeInteger(sAccountTypeCode.Value), v_lLedgerId:=nLedgerID)

                            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oAccount.GetAccountID Failed for Short Code " & CStr(v_vTransactionsArray(ACTBatchConst.ACTTransImportRelativeCode, iRow)), vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                                nResult = gPMConstants.PMEReturnCode.PMFalse
                                Return nResult
                            End If
                        End If
                    End If

                    v_vTransactionsArray(ACTBatchConst.ACTTransImportAccountID, iRow) = nAccountId
                End If
            Next iRow
            nSecondarys = nPrimarys
            nPrimarys = 0

            ' Start Transaction to enable Rollback if any part of posting fails
            nResult = BeginTrans()
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Start Transaction Posting.", vApp:=ACApp, vClass:=ACClass, vMethod:="SendToOrion", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return nResult
            End If

            ' indicate that we have started the transaction and it is open
            bTransactionOpen = True

            m_oDocumentPost.PostingPeriodNumber = m_lPostingPeriodNumber

            nResult = m_oDocumentPost.AddDocument(v_lDocumentTypeId:=nDocumentTYpeID, v_sDocumentRef:=sDocumentRef.Value,
                                                    v_dtDocumentDate:=dtDocumentDate, v_sComment:=sDocComment, v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                                                    v_sReason:=v_sReason, r_vDocumentID:=nDocumentId, r_vDocSourceID:=v_iDocSourceID,
                                                    v_vTermsOfPaymentId:=v_vTermsOfPaymentId, v_vPaymentDueDate:=v_vPaymentDueDate)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDocumentPost.AddDocument Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                nResult = nResult
                Return nResult
            End If

            'Return the id of the new document if required
            If Not Informations.IsNothing(r_vNewDocumentId) Then
                r_vNewDocumentId = nDocumentId
            End If

            nAccountId = 0
            'crAmount = 0
            'crCurrencyAmount = 0
            nAgentAccountID = 0
            dCurrencyBaseXrate = 0
            dtCurrencyBaseDate = #12/30/1899#
            dAccountBaseXrate = 0
            dtAccountBaseDate = #12/30/1899#
            dSystemBaseXrate = 0
            dtSystemBaseDate = #12/30/1899#

            If (sDocumentType.Value.ToUpper() = "CLC" Or sDocumentType.Value.ToUpper() = "CLD") And v_sDocTransactionTypeCode = "C_CO" Then
                sDocumentType.Value = "CLO"
            ElseIf (sDocumentType.Value.ToUpper() = "CLC" Or sDocumentType.Value.ToUpper() = "CLD") And v_sDocTransactionTypeCode = "C_CP" Then
                sDocumentType.Value = "CLP"
            ElseIf (sDocumentType.Value.ToUpper() = "CLC" Or sDocumentType.Value.ToUpper() = "CLD") And (v_sDocTransactionTypeCode = "C_SA" Or v_sDocTransactionTypeCode = "C_RV") Then
                sDocumentType.Value = "CLR"
            ElseIf (sDocumentType.Value.ToUpper() = "CLC" Or sDocumentType.Value.ToUpper() = "CLD") And v_sDocTransactionTypeCode = "C_CR" Then
                sDocumentType.Value = "CLA"
            End If

            'Determine the type of posting we are doing and get the appropriate currency conversion rates
            Select Case sDocumentType.Value
                Case "CLO"
                    dtCurrencyBaseDate = dtAccountingDate
                Case "CLA"
                    m_lReturn = m_oCurrencyConvert.GetClaimInformation(
                  sDocumentRef:=sDocumentRef.Value.Trim(),
                  r_lCompanyID:=v_iDocSourceID,
                  r_lAccountID:=nAgentAccountID,
                  r_iCurrencyID:=nCurrencyID,
                  r_dCurrencyBaseXrate:=dCurrencyBaseXrate,
                  r_dtCurrencyBaseDate:=dtCurrencyBaseDate,
                  r_dAccountBaseXrate:=dAccountBaseXrate,
                  r_dtAccountBaseDate:=dtAccountBaseDate,
                  r_dSystemBaseXrate:=dSystemBaseXrate,
                  r_dtSystemBaseDate:=dtSystemBaseDate,
                  r_lRateOverrideReasonID:=0)
                Case "CLP"
                    nResult = m_oCurrencyConvert.GetClaimPaymentInformation(sDocumentRef:=sDocumentRef.Value.Trim(), r_lAccountID:=nAgentAccountID, r_dCurrencyBaseXrate:=dCurrencyBaseXrate, r_dtCurrencyBaseDate:=dtCurrencyBaseDate, r_dAccountBaseXrate:=dAccountBaseXrate, r_dtAccountBaseDate:=dtAccountBaseDate, r_dSystemBaseXrate:=dSystemBaseXrate, r_dtSystemBaseDate:=dtSystemBaseDate)
                Case "CLR"
                    nResult = m_oCurrencyConvert.GetClaimReceiptInformation(sDocumentRef:=sDocumentRef.Value.Trim(), r_lAccountID:=nAgentAccountID, r_dCurrencyBaseXrate:=dCurrencyBaseXrate, r_dtCurrencyBaseDate:=dtCurrencyBaseDate, r_dAccountBaseXrate:=dAccountBaseXrate, r_dtAccountBaseDate:=dtAccountBaseDate, r_dSystemBaseXrate:=dSystemBaseXrate, r_dtSystemBaseDate:=dtSystemBaseDate)
                Case Else
                    nResult = m_oCurrencyConvert.GetInsuranceFileInformation(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_lAccountID:=nAgentAccountID, r_dCurrencyBaseXrate:=dCurrencyBaseXrate, r_dtCurrencyBaseDate:=dtCurrencyBaseDate, r_dAccountBaseXrate:=dAccountBaseXrate, r_dtAccountBaseDate:=dtAccountBaseDate, r_dSystemBaseXrate:=dSystemBaseXrate, r_dtSystemBaseDate:=dtSystemBaseDate)
            End Select

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oCurrencyConvert Get Currency Informations Failed for Document Type " & sDocumentType.Value, vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If
            sKey = "Cache_CurrencyIdFromISO_" & v_sDocCurrencyCode.Trim()
            ' Get the Currency ID for the given ISO code
            sCurrencyCode.Value = v_sDocCurrencyCode.Trim()
            If Not iCache Is Nothing AndAlso iCache.Contains(sKey) AndAlso Not String.IsNullOrEmpty(Convert.ToString(iCache.GetData(sKey))) Then
                nCurrencyID = ToSafeInteger(iCache.GetData(sKey))
            End If

            If nCurrencyID = 0 Then
                nResult = m_oCurrency.GetCurrencyIdFromISO(v_sISOCode:=sCurrencyCode.Value, r_iCurrencyId:=nCurrencyID)

                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oCurrencyConvert.GetCurrencyIdFromISO Failed for code " & sCurrencyCode.Value, vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    Return nResult
                End If
                If Not iCache Is Nothing Then
                    Dim atTimePeriod As AbsoluteTime = New AbsoluteTime(TimeSpan.FromMinutes(10))
                    iCache.Add(sKey, nCurrencyID, CacheItemPriority.Low, Nothing, New FileDependency(sFilePath), atTimePeriod)
                End If
            End If

            dtRefDate = DateTime.Now
            crRefAmount = 0
            crRefQuantity = 0
            sRefUnits.Value = ""

            ' Loop round incoming trans array setting up outgoing array as we go
            For iRow As Integer = v_vTransactionsArray.GetLowerBound(1) To v_vTransactionsArray.GetUpperBound(1)
                Dim crAmount As Decimal = 0
                Dim crCurrencyAmount As Double = 0

                sRelativeCode = CStr(v_vTransactionsArray(ACTBatchConst.ACTTransImportRelativeCode, iRow))
                sAccountTypeCode.Value = CStr(v_vTransactionsArray(ACTBatchConst.ACTTransImportAccountTypeCode, iRow)).Trim()
                sLedgerCode.Value = CStr(v_vTransactionsArray(ACTBatchConst.ACTTransImportLedgerCode, iRow)).Trim()
                oUnderwritingYearID = gPMFunctions.ToSafeLong(v_vTransactionsArray(ACTBatchConst.ACTTransImportUWYearID, iRow), 0)

                If gPMFunctions.ToSafeLong(oUnderwritingYearID, 0) = 0 Then
                    oUnderwritingYearID = DBNull.Value
                End If

                nAccountId = CInt(v_vTransactionsArray(ACTBatchConst.ACTTransImportAccountID, iRow))

                nSuspended = gPMFunctions.NullToInteger(v_vTransactionsArray(ACTBatchConst.ACTTransImportSuspended, iRow))
                nReleaseToIncome = gPMFunctions.NullToInteger(v_vTransactionsArray(ACTBatchConst.ACTTransImportReleaseToIncome, iRow))
                sReleaseAccountCode = gPMFunctions.NullToString(v_vTransactionsArray(ACTBatchConst.ACTTransImportReleaseAccountCode, iRow))
                sTransdetailTypeCode = gPMFunctions.NullToString(v_vTransactionsArray(ACTBatchConst.ACTTransImportTransdetailTypeCode, iRow))

                If sLedgerCode.Value = "IN" Or sLedgerCode.Value = "EX" Then
                    If v_sDocRef.ToUpper().StartsWith("SND") Or v_sDocRef.ToUpper().StartsWith("SRD") Or v_sDocRef.ToUpper().StartsWith("SED") Or v_sDocRef.ToUpper().StartsWith("SHD") Or v_sDocRef.ToUpper().StartsWith("TRD") Or v_sDocRef.ToUpper().StartsWith("CLR") Or (v_sDocRef.ToUpper().StartsWith("JN ") And crAmount > 0) Then

                        nRiskTransferAgreement = GetInsurerRiskTransferAgreement(lDocumentId:=nDocumentId, lAccountId:=nAccountId)

                        If nRiskTransferAgreement = 0 Then ' No Agreement
                            oRiskTransfer = 1 ' RT status Raised
                        Else
                            oRiskTransfer = 0 ' RT - 0 for Insurer with RT Agreement
                        End If
                    Else
                        oRiskTransfer = 0 ' RT - 0 for all other transaction type
                    End If
                Else
                    oRiskTransfer = Nothing ' RT - null for all non Insurer Transactions
                End If

                sLedgerCode.Value = CStr(v_vTransactionsArray(ACTBatchConst.ACTTransImportLedgerCode, iRow)).Trim()
                crCurrencyAmount = CDec(v_vTransactionsArray(ACTBatchConst.ACTTransImportCurrencyAmount, iRow))
                crRefAmount = CDec(v_vTransactionsArray(ACTBatchConst.ACTTransImportIPTTotal, iRow))
                crRefQuantity = CDec(v_vTransactionsArray(ACTBatchConst.ACTTransImportVATTotal, iRow))
                sTransComment = sDocComment
                oFeeType = v_vTransactionsArray(ACTBatchConst.kTTransImportFeeType, iRow)
                ' Derive the base amount from currency amount and ID
                'We need to use the effective date rather than the
                'accounting date for the currency conversion rate
                'but underwriting need the transaction date


                If (sDocumentRef.Value.ToString.Substring(0, 3) = "CLC") Then
                    dCurrencyBaseXrate = nStatsCurrencyXrate
                    crAmount = (crCurrencyAmount * dCurrencyBaseXrate)
                    dBaseAmountUnrounded = crAmount
                    sdCurrencyAmountUnrounded = CStr(crCurrencyAmount)
                Else


                    nResult = m_oCurrencyConvert.ConvertCurrencytoBase(lCurrencyID:=nCurrencyID, lCompanyID:=v_iDocSourceID, cBaseAmount:=crAmount,
                                                                             cCurrencyAmount:=crCurrencyAmount, vConversionDate:=dtCurrencyBaseDate,
                                                                             vConversionRate:=dCurrencyBaseXrate, vRounded:=True, lEuro:=nEuroCurrencyID,
                                                                             cEuroAmount:=crEuroAmount, vEuroCCyXrate:=odEuroCcyXrate, vEuroBaseXRate:=odEuroBaseXrate,
                                                                             vCCyAmountUnRounded:=sdCurrencyAmountUnrounded, vBaseAmountUnRounded:=dBaseAmountUnrounded)

                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        dCurrencyBaseXrate = 1
                        crAmount = (crCurrencyAmount * dCurrencyBaseXrate)
                        dBaseAmountUnrounded = crAmount
                        sdCurrencyAmountUnrounded = CStr(crCurrencyAmount)
                    End If
                End If
                If Convert.ToDecimal(dCurrencyBaseXrate) = 1 Then
                    ' set transaction amount = base amount, to avoid any round to decimal error
                    sdCurrencyAmountUnrounded = CStr(crCurrencyAmount)
                    crCurrencyAmount = crAmount
                End If

                oSpare = v_vTransactionsArray(ACTBatchConst.ACTTransImportSpare, iRow)
                vInsuranceRef = v_sDocInsuranceRef

                oOperatorID = v_iDocOperatorID

                If sLedgerCode.Value = SIRSalesTransactionLedgerCode Then
                    nDocSequence = nPrimarys + 1
                    nPrimarys += 1
                Else
                    nDocSequence = nSecondarys + 1
                    nSecondarys += 1
                End If

                oPurchaseOrderNo = v_vTransactionsArray(ACTBatchConst.ACTTransImportPurchaseOrderNo, iRow)
                oPurchaseInvoiceNo = v_vTransactionsArray(ACTBatchConst.ACTTransImportPurchaseInvoiceNo, iRow)

                If (CStr(v_vTransactionsArray(ACTBatchConst.ACTTransImportSpare, iRow)).ToUpper().StartsWith("TAX") Or
                    CStr(v_vTransactionsArray(ACTBatchConst.ACTTransImportSpare, iRow)).ToUpper().StartsWith("COMM")) _
                    And (v_sDocRef.ToUpper().StartsWith("IND") Or v_sDocRef.ToUpper().StartsWith("IRD") Or v_sDocRef.ToUpper().StartsWith("IED") Or v_sDocRef.ToUpper().StartsWith("ICD")) Then

                    'Get the finance plan array, we need the PremiumFinanceCnt and PremiumFinanceVer to
                    'post a suspended transaction
                    nResult = GetPremiumFinanceRecord(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_lFinancePlanArray:=oFinancePlanArray)

                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get finance plan array", vApp:=ACApp, vClass:=ACClass, vMethod:=nResult)
                        Return nResult
                    Else
                        oPremiumFinanceCnt = CInt(oFinancePlanArray(0, 0))
                        oPremiumFinanceVer = CInt(oFinancePlanArray(1, 0))
                    End If
                End If
                nTransdetailTypeID = 0
                sKey = "Cache_GetTransDetailTypeID_" & sTransdetailTypeCode
                ' Get the Currency ID for the given ISO code
                sCurrencyCode.Value = v_sDocCurrencyCode.Trim()
                If Not iCache Is Nothing AndAlso iCache.Contains(sKey) AndAlso Not String.IsNullOrEmpty(Convert.ToString(iCache.GetData(sKey))) Then
                    nTransdetailTypeID = ToSafeInteger(iCache.GetData(sKey))
                End If

                If nTransdetailTypeID = 0 Then
                    nResult = GetTransDetailTypeID(sTransdetailTypeCode, nTransdetailTypeID)
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                                            sMsg:="GetTransDetailTypeID failed for insurance file cnt " & v_lInsuranceFileCnt,
                                            vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                        Return nResult
                    End If
                    If Not iCache Is Nothing Then
                        Dim atTimePeriod As AbsoluteTime = New AbsoluteTime(TimeSpan.FromMinutes(10))
                        iCache.Add(sKey, nTransdetailTypeID, CacheItemPriority.Low, Nothing, New FileDependency(sFilePath), atTimePeriod)
                    End If
                End If

                If (sLedgerCode.Value = "IN" OrElse sLedgerCode.Value = "SL" OrElse sLedgerCode.Value = "AG" OrElse sLedgerCode.Value = "UB") Then
                    'get the due date based on insurance file cnt
                    nResult = GetDueDateForTransactions(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lAccountID:=nAccountId, v_dtDueDate:=dtDueDate)

                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                                           sMsg:="GetDueDateForTransactions failed for insurance file cnt " & v_lInsuranceFileCnt,
                                           vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                        Return nResult
                    End If
                Else
                    dtDueDate = Nothing
                End If

                oTaxGroupID = v_vTransactionsArray(ACTBatchConst.ACTTransImportTaxGroupID, iRow)
                oTaxBandID = v_vTransactionsArray(ACTBatchConst.ACTTransImportTaxBandID, iRow)
                bManuallyReleased = CBool(v_vTransactionsArray(ACTBatchConst.ACTTransImportManuallyReleased, iRow))
                bReleasedOnFullSettlement = CBool(v_vTransactionsArray(ACTBatchConst.ACTTransImportReleasedOnFullSettlement, iRow))
                bReleasedForWholePosting = CBool(v_vTransactionsArray(ACTBatchConst.ACTTransImportReleasedForWholePosting, iRow))
                bReleasedOnPolicyEffective = CBool(v_vTransactionsArray(ACTBatchConst.ACTTransImportReleasedOnPolicyEffective, iRow))

                If sDocumentType.Value.ToUpper() = "CLP" Then

                    nResult = m_oCurrencyConvert.FindCurrencyBaseRateByAccount(v_lAccountID:=nAccountId, v_lCompanyId:=v_iDocSourceID,
                                                           r_dAccountBaseXrate:=dAccountBaseXrate,
                                                           r_dtAccountBaseDate:=dtAccountBaseDate)

                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get FindCurrencyBaseRateByAccount", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                        nResult = gPMConstants.PMEReturnCode.PMFail
                        Return nResult
                    End If

                End If
                If (sDocumentRef.ToString.Substring(0, 3) = "SDD") Then
                    dCurrencyBaseXrate = 0
                End If

                m_lReturn = m_oDocumentPost.AddTransaction(r_vTransDetailId:=nTransdetailId, v_vDocumentSequence:=nDocSequence, v_lAccountID:=nAccountId,
                                                           v_iCurrencyID:=nCurrencyID, v_cAmount:=crAmount, v_vBaseAmountUnrounded:=dBaseAmountUnrounded,
                                                           v_cCurrencyAmount:=crCurrencyAmount, v_vCurrencyAmountUnrounded:=sdCurrencyAmountUnrounded,
                                                           v_vdCurrencyBaseXRate:=dCurrencyBaseXrate, v_vComment:=sTransComment, v_vEuroCurrencyId:=nEuroCurrencyID,
                                                           v_vEuroAmount:=crEuroAmount, v_vEuroBaseXRate:=odEuroBaseXrate, v_vEuroCcyXrate:=odEuroCcyXrate,
                                                           v_vInsuranceRef:=vInsuranceRef, v_vOperatorID:=oOperatorID, v_vPurchaseOrderNo:=oPurchaseOrderNo,
                                                           v_vPurchaseInvoiceNo:=oPurchaseInvoiceNo, v_vSpare:=oSpare,
                                                           v_vRefDate:=dtRefDate, v_vRefAmount:=crRefAmount, v_vRefQuantity:=crRefQuantity,
                                                           v_vRefUnits:=sRefUnits.Value, v_vAccountingDate:=dtAccountingDate, v_vCurrencyBaseDate:=dtCurrencyBaseDate,
                                                           v_vAccountBaseXrate:=dAccountBaseXrate, v_vAccountBaseDate:=dtAccountBaseDate,
                                                           v_vSystemBaseXrate:=dSystemBaseXrate, v_vSystemBaseDate:=dtSystemBaseDate,
                                                           v_vUnderwritingYearID:=oUnderwritingYearID, v_vTransdetailTypeID:=nTransdetailTypeID,
                                                           v_vTaxGroupID:=oTaxGroupID, v_vTaxBandID:=oTaxBandID, v_vRiskTransfer:=oRiskTransfer,
                                                           v_vReference:=sAltReference, v_vDueDate:=dtDueDate, oFeeType:=oFeeType)

                'Else
                'm_lReturn = m_oDocumentPost.AddTransaction(r_vTransDetailId:=lTransdetailId, v_vDocumentSequence:=lDocSequence, v_lAccountID:=lAccountId, v_iCurrencyID:=iCurrencyID, v_cAmount:=cAmount, v_vBaseAmountUnrounded:=vdBaseAmountUnrounded, v_cCurrencyAmount:=cCurrencyAmount, v_vCurrencyAmountUnrounded:=vdCurrencyAmountUnrounded, v_vdCurrencyBaseXRate:=dCurrencyBaseXrate, v_vComment:=sTransComment, v_vEuroCurrencyID:=lEuroCurrencyID, v_vEuroAmount:=cEuroAmount, v_vEuroBaseXrate:=vdEuroBaseXrate, v_vEuroCcyXrate:=vdEuroCcyXrate, v_vInsuranceRef:=vInsuranceRef, v_vOperatorID:=vOperatorID, v_vPurchaseOrderNo:=vPurchaseOrderNo, v_vPurchaseInvoiceNo:=vPurchaseInvoiceNo, v_vDepartment:=vDepartment, v_vSpare:=vSpare, v_vRefDate:=dtRefDate, v_vRefAmount:=cRefAmount, v_vRefQuantity:=cRefQuantity, v_vRefUnits:=sRefUnits.Value, v_vAccountingDate:=dtAccountingDate, v_vCurrencyBaseDate:=dtCurrencyBaseDate, v_vAccountBaseXrate:=DBNull.Value, v_vAccountBaseDate:=dtAccountBaseDate, v_vSystemBaseXrate:=dSystemBaseXrate, v_vSystemBaseDate:=dtSystemBaseDate, v_vUnderwritingYearID:=vUnderwritingYearID, v_vTransdetailTypeID:=lTransdetailTypeID, v_vTaxGroupID:=vTaxGroupID, v_vTaxBandID:=vTaxBandID, v_vRisktransfer:=lRiskTransfer, v_vBalancetype:=sBalancetype, v_vReference:=sAltReference)
                'End If

                If nSuspended = 1 Then
                    'If we have no TransDetailType Id we will be unable to post suspended transactions

                    If Convert.IsDBNull(nTransdetailTypeID) Or Informations.IsNothing(nTransdetailTypeID) Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Invalid TransDetailType - Unable to write to " &
                                           "SuspendedAccountsTransaction Table", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                        nResult = gPMConstants.PMEReturnCode.PMFail
                        Return nResult
                    End If

                    'Get Released account  PN22188
                    nResult = GetDestinationAccountId(v_iReleaseToIncome:=nReleaseToIncome, v_lAccountID:=nAccountId, v_sAccountName:=sRelativeCode, v_sReleaseAccountCode:=sReleaseAccountCode, r_lDestinationAccountId:=nDestinationAccountId)

                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get Destination Account", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                        nResult = gPMConstants.PMEReturnCode.PMFail
                        Return nResult
                    End If

                    If Not Informations.IsArray(oSuspenseArray) Then
                        ReDim oSuspenseArray(12, 0)
                    Else

                        ReDim Preserve oSuspenseArray(12, oSuspenseArray.GetUpperBound(1) + 1)
                    End If

                    oSuspenseArray(0, oSuspenseArray.GetUpperBound(1)) = nTransdetailId
                    oSuspenseArray(1, oSuspenseArray.GetUpperBound(1)) = oPremiumFinanceCnt
                    oSuspenseArray(2, oSuspenseArray.GetUpperBound(1)) = oPremiumFinanceVer
                    oSuspenseArray(3, oSuspenseArray.GetUpperBound(1)) = nDestinationAccountId
                    oSuspenseArray(4, oSuspenseArray.GetUpperBound(1)) = v_lInsuranceFileCnt
                    oSuspenseArray(5, oSuspenseArray.GetUpperBound(1)) = nDocumentTYpeID
                    oSuspenseArray(6, oSuspenseArray.GetUpperBound(1)) = nTransdetailTypeID
                    oSuspenseArray(7, oSuspenseArray.GetUpperBound(1)) = oSpare
                    oSuspenseArray(8, oSuspenseArray.GetUpperBound(1)) = sTransdetailTypeCode
                    oSuspenseArray(9, oSuspenseArray.GetUpperBound(1)) = bManuallyReleased
                    oSuspenseArray(10, oSuspenseArray.GetUpperBound(1)) = bReleasedOnFullSettlement
                    oSuspenseArray(11, oSuspenseArray.GetUpperBound(1)) = bReleasedForWholePosting
                    oSuspenseArray(12, oSuspenseArray.GetUpperBound(1)) = bReleasedOnPolicyEffective
                End If

                If nDocSequence = 1 Then
                    m_lTransactionId = nTransdetailId
                End If
                ReDim Preserve oTransIds(nDocSequence - 1)

                oTransIds(nDocSequence - 1) = nTransdetailId

                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDocumentPost.AddTransaction Failed for AccountId " & nAccountId, vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                    nResult = gPMConstants.PMEReturnCode.PMFail
                    Return nResult
                End If

                'Add the summary to the array

                'Index 0 - holds account_id, 1 holds transdetail_id, 2 holds base amount
                If nTransdetailId <> 0 Then
                    If oPostedAmounts Is Nothing Then
                        ReDim oPostedAmounts(2, 0)
                    Else
                        ReDim Preserve oPostedAmounts(2, oPostedAmounts.GetUpperBound(1) + 1)
                    End If
                    oPostedAmounts(0, oPostedAmounts.GetUpperBound(1)) = nAccountId
                    oPostedAmounts(1, oPostedAmounts.GetUpperBound(1)) = nTransdetailId
                    oPostedAmounts(2, oPostedAmounts.GetUpperBound(1)) = crAmount
                End If
            Next

            crRoundingError = m_oDocumentPost.TotalBase
            If crRoundingError <> 0 Then

                Dim crAmount As Decimal = 0
                Dim crCurrencyAmount As Double = 0

                crAmount = -crRoundingError

                'We need to use the effective date rather than the
                'accounting date for the currency conversion rate
                'but underwriting need the transaction date  PN20443

                nResult = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=nCurrencyID, lCompanyID:=v_iDocSourceID, cBaseAmount:=crAmount, cCurrencyAmount:=crCurrencyAmount, vConversionDate:=dtAccountingDate, vConversionRate:=dCurrencyBaseXrate, vIsMultiplier:=Nothing, vRounded:=True, vBaseRoundingDifference:=Nothing, vCurrencyRoundingDifference:=Nothing, vFormattedBase:=Nothing, vFormattedCurrency:=Nothing, lEuro:=nEuroCurrencyID, cEuroAmount:=crEuroAmount, vEuroCCyXrate:=odEuroCcyXrate, vEuroBaseXRate:=odEuroBaseXrate, vCCyAmountUnRounded:=sdCurrencyAmountUnrounded, vBaseAmountUnRounded:=dBaseAmountUnrounded)

                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    crAmount = -crRoundingError
                    dBaseAmountUnrounded = crAmount
                    crCurrencyAmount = ((-crRoundingError) / (If(gPMFunctions.ToSafeDecimal(dCurrencyBaseXrate, 0) = 0, 1, dCurrencyBaseXrate)))
                    sdCurrencyAmountUnrounded = CStr(crCurrencyAmount)
                End If

                If Convert.ToDecimal(dCurrencyBaseXrate) = 1 AndAlso ValidateAndFixRounding(m_lTransactionExportFolderCnt, nDocumentId, crAmount) Then
                    'Attempt to fix rounding error if not proceed as usual
                Else

                    'Commented for ando--need to fix in transaction posting
                    'If v_sDocTransactionTypeCode.Trim = "REN" AndAlso Math.Abs(crRoundingError) > 1 AndAlso Math.Abs(m_oDocumentPost.TotalCurrency) > 1 Then

                    '    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Rounding amount is greater than 1 ", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                    '    r_sfailureReason = " :- Rounding amount is greater than 1"

                    '    Return gPMConstants.PMEReturnCode.PMFalse
                    'End If
                    'So we need to call AddTransaction, but what do we put in?
                    nDocSequence = nSecondarys + 1

                    nResult = GetGLAccount(v_cAmount:=crRoundingError, r_lGLAccountID:=nAccountId)
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetGLAccount failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    'vSpare is flagged that this is a rounding error

                    oSpare = "Rounding"

                    nResult = GetTransDetailTypeID("ROUNDING", nTransdetailTypeID)
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetTransDetailTypeID failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    If (sDocumentRef.ToString.Substring(0, 3) = "SDD") Then
                        dCurrencyBaseXrate = 0
                    End If
                    nResult = m_oDocumentPost.AddTransaction(r_vTransDetailId:=nTransdetailId, v_vDocumentSequence:=nDocSequence, v_lAccountID:=nAccountId,
                                                           v_iCurrencyID:=nCurrencyID, v_cAmount:=crAmount, v_vBaseAmountUnrounded:=dBaseAmountUnrounded,
                                                           v_cCurrencyAmount:=crCurrencyAmount, v_vCurrencyAmountUnrounded:=sdCurrencyAmountUnrounded,
                                                           v_vdCurrencyBaseXRate:=dCurrencyBaseXrate, v_vComment:=sTransComment, v_vEuroCurrencyId:=nEuroCurrencyID,
                                                           v_vEuroAmount:=crEuroAmount, v_vEuroBaseXRate:=odEuroBaseXrate, v_vEuroCcyXrate:=odEuroCcyXrate,
                                                           v_vInsuranceRef:=vInsuranceRef, v_vOperatorID:=oOperatorID, v_vPurchaseOrderNo:=oPurchaseOrderNo,
                                                           v_vPurchaseInvoiceNo:=oPurchaseInvoiceNo, v_vDepartment:=oDepartment, v_vSpare:=oSpare, v_vRefDate:=dtRefDate,
                                                           v_vRefAmount:=crRefAmount, v_vRefQuantity:=crRefQuantity, v_vRefUnits:=sRefUnits.Value, v_vAccountingDate:=dtAccountingDate,
                                                           v_vCurrencyBaseDate:=dtCurrencyBaseDate, v_vAccountBaseXrate:=dAccountBaseXrate, v_vAccountBaseDate:=dtAccountBaseDate,
                                                           v_vSystemBaseXrate:=dSystemBaseXrate, v_vSystemBaseDate:=dtSystemBaseDate, v_vUnderwritingYearID:=oUnderwritingYearID,
                                                           v_vRiskTransfer:=oRiskTransfer, v_vBalanceType:=sBalancetype, v_vReference:=sAltReference, v_vTransdetailTypeID:=nTransdetailTypeID)
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDocumentPost.AddTransaction Failed for AccountId " & nAccountId, vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                        nResult = gPMConstants.PMEReturnCode.PMFail
                        Return nResult
                    End If
                    If nDocSequence = 1 Then
                        m_lTransactionId = nTransdetailId
                    End If

                    ReDim Preserve oTransIds(nDocSequence - 1)

                    oTransIds(nDocSequence - 1) = nTransdetailId

                End If
            End If

            If v_lInsuranceFileCnt > 0 Then

                ' now auto-allocate transactions for same account that cancel each other out
                'this is only done for standard policies
                Dim bIsClaimTransactionType As Boolean = False
                If v_sDocTransactionTypeCode = "C_CO" OrElse v_sDocTransactionTypeCode = "C_CR" OrElse v_sDocTransactionTypeCode = "C_CP" OrElse v_sDocTransactionTypeCode = "C_RV" OrElse v_sDocTransactionTypeCode = "C_SA" Then
                    bIsClaimTransactionType = True
                End If

                If sPaymentMethod <> "Instalments" And sPaymentMethod <> "Premium Finance" And Informations.IsArray(oPostedAmounts) = True And sPaymentMethod <> "Direct Debit" AndAlso Not bIsClaimTransactionType Then
                    bValid = False
                    For nCnt = 0 To oPostedAmounts.GetUpperBound(1)
                        If ToSafeDouble(oPostedAmounts(2, nCnt)) <> 0 Then
                            bValid = True
                            Exit For
                        End If
                    Next nCnt

                    If bValid = True Then
                        nResult = AutoAllocateCancellingTransactions(vTransactions:=oPostedAmounts, sDocBusinessTypeCode:=v_sDocBusinessTypeCode)
                        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="AutoAllocateCancellingTransactions Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                            nResult = gPMConstants.PMEReturnCode.PMFail
                            Return nResult
                        End If
                    End If
                End If
            End If
            If Informations.IsArray(oSuspenseArray) Then

                nResult = WriteSuspendedAccountsTransactions(oSuspenseArray, nDocumentId, m_lTransactionId)

                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Write Suspended Transactions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                    Return nResult
                End If
            End If

            Const ACUpdateAgentDetailsforSuspendedAccountStored As Boolean = True
            Const ACUpdateAgentDetailsforSuspendedAccountName As String = "UpdateAgentDetailsforSuspendedAccount"
            Const ACUpdateAgentDetailsforSuspendedAccountSQL As String = "Spu_Update_Agent_Details_for_Suspended_account"

            If v_sDocLeadAgentShortName <> "" AndAlso sDocumentType.Value <> "SND" Then

                With m_oDatabase
                    .Parameters.Clear()
                    nResult = .Parameters.Add(sName:="Agent_Code", vValue:=v_sDocLeadAgentShortName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    nResult = .Parameters.Add(sName:="Insurance_ref", vValue:=v_sDocInsuranceRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    nResult = .SQLAction(sSQL:=ACUpdateAgentDetailsforSuspendedAccountSQL, sSQLName:=ACUpdateAgentDetailsforSuspendedAccountName, bStoredProcedure:=ACUpdateAgentDetailsforSuspendedAccountStored)
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End With
            End If

            With m_oDatabase

                .Parameters.Clear()
                nResult = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="spu_Get_TMP ", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    Return nResult
                End If

                nResult = .Parameters.Add(sName:="is_true_monthly_policy", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="spu_Get_TMP ", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    Return nResult
                End If

                nResult = .Parameters.Add(sName:="lead_month_in_cycle", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="spu_Get_TMP ", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    Return nResult
                End If
                nResult = .Parameters.Add(sName:="sub_month_in_cycle", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="spu_Get_TMP ", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    Return nResult
                End If
                nResult = .Parameters.Add(sName:="lead_allow_consolidated_commission", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="spu_Get_TMP ", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    Return nResult
                End If
                nResult = .Parameters.Add(sName:="sub_allow_consolidated_commission", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="spu_Get_TMP ", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    Return nResult
                End If
                nResult = .Parameters.Add(sName:="transaction_type_code", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="spu_Get_TMP ", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    Return nResult
                End If
                nResult = .Parameters.Add(sName:="renewal_count", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="spu_Get_TMP ", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    Return nResult
                End If

                nResult = .SQLSelect(sSQL:="spu_get_true_monthly_policy_details", sSQLName:="Get True Monthly Policy Details", bStoredProcedure:=True)

                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="spu_Get_TMP ", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    Return nResult
                End If

                If Not Informations.IsArray(aResults) Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to get payment method for insurance file " &
                                       v_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    Return nResult
                Else
                    bTrueMonthlyPolicy = gPMFunctions.ToSafeBoolean(.Parameters.Item("is_true_monthly_policy").Value)
                    If bTrueMonthlyPolicy Then
                        nLeadMonthCycle = gPMFunctions.ToSafeLong(.Parameters.Item("lead_month_in_cycle").Value)
                        nSubMonthCycle = gPMFunctions.ToSafeLong(.Parameters.Item("sub_month_in_cycle").Value)
                        bLeadConsolidate = gPMFunctions.ToSafeBoolean(.Parameters.Item("lead_allow_consolidated_commission").Value)
                        bSubConsolidate = gPMFunctions.ToSafeBoolean(.Parameters.Item("sub_allow_consolidated_commission").Value)
                        sTranasactionType = gPMFunctions.ToSafeString(.Parameters.Item("transaction_type_code").Value).Trim()
                        nRenewalCount = gPMFunctions.ToSafeLong(.Parameters.Item("renewal_count").Value)

                        If bTrueMonthlyPolicy And sTranasactionType = "REN" Then
                            If nRenewalCount = nLeadMonthCycle And bLeadConsolidate Then
                                nResult = GetSuspendedCommissionTransactions(sAgentType:="LEAD", lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vSuspenseArray:=oSuspenseArray)

                                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", GetSuspendedCommissionTransactions LEAD failed")
                                End If

                                If Informations.IsArray(oSuspenseArray) Then

                                    For iCount As Integer = 0 To oSuspenseArray.GetUpperBound(1)

                                        nResult = m_oTransdetail.ReleaseSuspendedTransactions(lAllocationId:=0, vSuspendedTransdetailID:=oSuspenseArray(0, iCount), vInsuranceFileCnt:=v_lInsuranceFileCnt, vPercentage:=1)
                                        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", ReleaseSuspendedTransaction SUB failed")
                                        End If
                                    Next
                                End If
                            End If

                            If nRenewalCount = nSubMonthCycle And bSubConsolidate Then
                                nResult = GetSuspendedCommissionTransactions(sAgentType:="SUB", lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vSuspenseArray:=oSuspenseArray)

                                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", GetSuspendedCommissionTransactions SUB failed")
                                End If

                                If Informations.IsArray(oSuspenseArray) Then

                                    For iCount As Integer = 0 To oSuspenseArray.GetUpperBound(1)

                                        nResult = m_oTransdetail.ReleaseSuspendedTransactions(lAllocationId:=0, vSuspendedTransdetailID:=oSuspenseArray(0, iCount), vInsuranceFileCnt:=v_lInsuranceFileCnt, vPercentage:=1)
                                        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", ReleaseSuspendedTransaction SUB failed")
                                        End If
                                    Next
                                End If
                            End If
                        End If
                    End If
                End If
            End With

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDocumentPost.Commit Failed : " &
                                   "AccountID = " & CStr(nAccountId) & " " &
                                   "TransactionExportFolderCnt = " & CStr(m_lTransactionExportFolderCnt), vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                nResult = gPMConstants.PMEReturnCode.PMFail
                Return nResult
            End If

            If m_lTransactionExportFolderCnt > 0 Then
                nResult = UpdateTransactionExportComplete(v_lTransactionExportFolderCnt:=m_lTransactionExportFolderCnt)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="UpdateTransactionExportComplete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument")
                    Return nResult
                End If
            End If

            r_lDocPostedStatus = gPMConstants.PMEReturnCode.PMSucceed

            ' Commit Transaction
            nResult = CommitTrans()
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Commit Transaction Posting.", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            ' indicate that we have closed the transaction opened in this procedure
            bTransactionOpen = False

            ' Pass Company
            ' If set earned commission is AsDebited or set earned commission is Client Pays and premium is zero
            If m_sCommissionOption = AsDebited Or (m_bIsPostCommission And CBool(ClientPayment)) Then

                ' Commission option AsDebited for auto posting of commission in case of zero premium with
                'earned commission is when client pays
                If m_bIsPostCommission And CBool(ClientPayment) Then
                    m_sCommissionOption = AsDebited
                End If

                nResult = PostCommission(v_sCommissionOption:=m_sCommissionOption, v_iCompanyID:=v_iDocSourceID, v_lTransactionId:=m_lTransactionId)

                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Commit Commission Posting.", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return nResult
                End If
            End If

            nResult = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnhancedAccountingBasis, v_vBranch:=1, r_vUnderwriting:=oValue)

            Return nResult

        Catch ex As Exception

            nResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Post Document Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
            Return nResult
        Finally

            If bTransactionOpen Then
                m_oDatabase.SQLRollbackTrans()
            End If
        End Try

    End Function

    Private Function ValidateAndFixRounding(ByVal nTransactionExportFolderCnt As Integer, ByVal nDocumentId As Integer, ByVal crAmount As Double) As Integer
        Dim vResults(,) As Object = Nothing
        Try
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="nTransactionExportFolderCnt", vValue:=nTransactionExportFolderCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="nDocumentId", vValue:=nDocumentId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="crAmount", vValue:=crAmount, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kRoundingSQL, sSQLName:=kRoundingName, bStoredProcedure:=kRoundingStored, vResultArray:=vResults)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                If Informations.IsArray(vResults) Then
                    If (CStr(vResults(0, 0)).Trim() = "1") Then
                        Return gPMConstants.PMEReturnCode.PMTrue
                    End If
                End If
            End If


        Catch ex As Exception
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

        Return gPMConstants.PMEReturnCode.PMFalse
    End Function

    ' ***************************************************************** '
    ' Name: PostCommission (Private)
    '
    ' Description: Transfer Commission to Earned Account.
    '

    'eck180500 New Parameter for Company
    ' ***************************************************************** '
    Public Function PostCommission(ByVal v_sCommissionOption As String, ByVal v_iCompanyID As Integer, ByVal v_lTransactionId As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oCommissionPost Is Nothing Then
                'SD 24/07/2002 rename variable

                m_oCommissionPost = New bACTCommissionPost.Business
                m_lReturn = m_oCommissionPost.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=" gPMComponentServices.CreateBusinessObject Failed for bACTCommissionPost.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="PostCommission")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'eck180500 New Parameter for Company

            m_lReturn = m_oCommissionPost.PostCommission(v_sCommissionOption:=v_sCommissionOption, v_iCompanyID:=v_iCompanyID, v_lTransactionId:=v_lTransactionId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to post the commission", vApp:=ACApp, vClass:=ACClass, vMethod:="PostCommission")

                Return result
            End If

            m_oCommissionPost.Dispose()
            m_oCommissionPost = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostCommission Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostCommission", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PostCommissionTax (Private)
    '
    ' Description: Transfer Commission Tax to Due Account.
    '
    ' ***************************************************************** '
    Public Function PostCommissionTax(ByVal v_iCompanyID As Integer, ByVal v_lTransactionId As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oCommissionPostTax Is Nothing Then
                'SD 24/07/2002 rename variable

                m_oCommissionPostTax = New bACTCommissionPost.Business
                m_lReturn = m_oCommissionPostTax.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=" gPMComponentServices.CreateBusinessObject Failed for bACTCommissionPost.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="PostCommissionTax")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            m_lReturn = m_oCommissionPostTax.PostCommissionTax(v_iCompanyID:=v_iCompanyID, v_lTransactionId:=v_lTransactionId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to post the commission tax", vApp:=ACApp, vClass:=ACClass, vMethod:="PostCommission tax")

                Return result
            End If

            m_oCommissionPostTax.Dispose()
            m_oCommissionPostTax = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostCommissionTax Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostCommissionTax", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeriveAccountID (Private)
    '
    ' Description: Determine the Account in the Structure or auto-
    ' create one.
    '
    ' DD 06/08/2002: Remove obsolete code
    ' RKC 20/08/2002 Create Path requires to pass LedgerId to
    '                bACTExplorer.InsertAccount and not LedgerCode
    ' ***************************************************************** '

    Public Function DeriveAccountID(ByRef r_lAccountId As Integer, ByVal v_lParentNodeId As String, ByVal v_sRelativeCode As String, ByVal v_iAccountType As Integer, ByVal v_lLedgerId As Integer) As Integer

        Dim result As Integer = 0
        Dim sFullCode As String = ""
        Dim sFullCodeOfMapping As String = ""
        Dim lCompanyID As Integer
        Dim vValue As Object = Nothing
        Dim sReturnString As String = ""
        'DC260706 added different accountid

        ' Const ACDefaultMappingID As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get the Full Path from the Structure Tree

            m_oExplorer.GetFullPath(lNodeId:=v_lParentNodeId, vFullPath:=sFullCodeOfMapping, vCompanyId:=lCompanyID)

            If sFullCodeOfMapping = "" Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sFullCode = (sFullCodeOfMapping & "\" & v_sRelativeCode).Trim()

            'Get the AccountID from the tree

            m_lReturn = m_oExplorer.FullKeyExists(v_sFullKey:=sFullCode, r_vAccountId:=r_lAccountId, v_vCompanyId:=lCompanyID)

            'If the AccountID cannot be found then locate/create it
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Use a Transaction

                m_oExplorer.BeginTrans()

                m_lReturn = gPMConstants.PMEReturnCode.PMTrue

                'Create a new account and get the new ID
                m_lReturn = CreatePath(r_lAccountId:=r_lAccountId, v_sFullCodeOfMapping:=sFullCodeOfMapping, v_sRelativeCode:=v_sRelativeCode, v_iAccountType:=v_iAccountType, v_lLedgerId:=v_lLedgerId, v_lCompanyId:=lCompanyID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Create Path Failed for " & sFullCodeOfMapping & "," &
                                       v_sRelativeCode & "," & CStr(v_iAccountType) & "," & CStr(v_lLedgerId), vApp:=ACApp, vClass:=ACClass, vMethod:="DeriveAccountId")
                    result = gPMConstants.PMEReturnCode.PMFalse

                    m_oExplorer.RollbackTrans()
                    Return result
                Else
                    'S4BDAT003 Data Sure

                    m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnhancedAccountingBasis, v_vBranch:=1, r_vUnderwriting:=vValue)

                    m_oExplorer.CommitTrans()
                End If
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeriveAccountId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeriveAccountId", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'RKC 20/08/2002
    'Insert Account Requires Ledger Id as opposed to Ledger Code
    Private Function CreatePath(ByRef r_lAccountId As Integer, ByVal v_sFullCodeOfMapping As String, ByVal v_sRelativeCode As String, ByVal v_iAccountType As Integer, ByVal v_lLedgerId As Integer, ByVal v_lCompanyId As Integer) As Integer

        Dim result As Integer = 0
        Dim sWorkCode As String = ""
        Dim sParentCode As New StringBuilder
        Dim iStart, iEnd As Integer
        Dim bCreateAccount As Boolean
        Dim lNodeId As Integer
        Dim sElementName As String = ""
        Dim lElementId, lParentNodeId As Integer
        Dim vNodeData(,) As Object = Nothing
        Dim bFound As Boolean
        Dim lNumberRecords As Integer
        Dim bGetAccountIdFromMapping As Boolean

        ' PWF 20/09/2002 - Local company id. This will allow us to try and locate
        ' the account if it doesn't belong to the current company but still try
        ' to create a new account using the current company
        Dim lCompanyID As Object

        Const NO_ACCOUNT_FOR_MAPPING As Integer = 0
        Const NO_NODE_FOR_MAPPING As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue
        sParentCode = New StringBuilder(v_sFullCodeOfMapping)
        bCreateAccount = False

        lCompanyID = DBNull.Value

        'RKC 22/08/2002
        'Initialise to False so that AccountId is Not Required if Default Functions work as expected
        bGetAccountIdFromMapping = False

        If (v_sRelativeCode.IndexOf("\"c) + 1) = 1 Then
            iStart = 2
        Else
            iStart = 1
        End If

        iEnd = v_sRelativeCode.IndexOf("\"c, iStart + 1) + 1
        r_lAccountId = 0

        Do
            If iEnd = 0 Then
                iEnd = v_sRelativeCode.Length + 1
                bCreateAccount = True 'end of code so create account
            End If

            sElementName = v_sRelativeCode.Substring(iStart - 1, Math.Min(v_sRelativeCode.Length, iEnd - iStart))

            m_oExplorer.GetNodeIdFromFullPath(v_sFullKey:=sParentCode.ToString(), r_lNodeId:=lParentNodeId, v_vCompanyId:=v_lCompanyId)
            sParentCode.Append("\" & v_sRelativeCode.Substring(iStart - 1, Math.Min(v_sRelativeCode.Length, iEnd - iStart)))

            If lParentNodeId = 0 Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ParentNodId = 0", vApp:=ACApp, vClass:=ACClass, vMethod:="CreatePath")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oExplorer.GetChildrenOfNode(lParentNodeId:=lParentNodeId, lNumberRecords:=0, vResultArray:=vNodeData, lCompanyID:=v_lCompanyId)

            If Not Informations.IsArray(vNodeData) Then
                lNumberRecords = 0
            Else

                lNumberRecords = vNodeData.GetUpperBound(1) + 1
            End If

            bFound = False

            For lRecord As Integer = 0 To lNumberRecords - 1

                If CStr(vNodeData(ACTExplorerConst.ACGetNodeElementName, lRecord)) = sElementName Then
                    bFound = True
                    Exit For
                End If
            Next lRecord

            If Not bFound Then

                If bCreateAccount Then
                    'Tomo071100
                    'Minor change - SBO never call this, but SFU will.  Probably.
                    '                r_lAccountId = m_oExplorer.InsertAccount( _
                    'r_vAccountName:=sElementName, _
                    'r_vShortCode:=sElementName, _
                    'r_vAccountType:=v_iAccountType)

                    m_lReturn = m_oExplorer.InsertAccount(r_lAccountID:=r_lAccountId, r_vAccountName:=sElementName, r_vShortCode:=sElementName, r_vAccountType:=v_iAccountType, r_vLedgerId:=v_lLedgerId, r_vCompanyID:=v_lCompanyId)

                    'RKC 22/08/2002
                    'We Need to Get Node Id from Alternative Method As Cannot Get Node Id
                    'from Create Account as This Account Is Already There
                    If m_lReturn = gPMConstants.PMEReturnCode.PMRecordInUse Then
                        bGetAccountIdFromMapping = True
                    End If

                    'Always insert the element after inserting the account
                End If

                'be it folder or account, an element is required for the tree

                lElementId = m_oExplorer.LookupElementId(sElementName:=sElementName, lCompanyID:=lCompanyID)

                If lElementId = 0 Then

                    lElementId = m_oExplorer.InsertElement(sElementName:=sElementName)

                    If lElementId = 0 Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

                'RKC 22/08/2002
                If Not bGetAccountIdFromMapping Then
                    'Leave account creation functions alone
                    If bCreateAccount Then

                        lNodeId = m_oExplorer.InsertNode(lParentNodeId:=lParentNodeId, lElementId:=lElementId, vAccountID:=r_lAccountId)
                    Else

                        lNodeId = m_oExplorer.InsertNode(lParentNodeId:=lParentNodeId, lElementId:=lElementId)
                    End If
                Else
                    'Get AccountId from FullPath

                    m_oExplorer.GetAccountIdFromFullPath(v_sFullKey:=sParentCode.ToString(), r_lAccountID:=r_lAccountId, v_vCompanyId:=lCompanyID)

                    If r_lAccountId = NO_ACCOUNT_FOR_MAPPING Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to GetAccountIdFromFullPath for CreatePath.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreatePath", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Get NodeId from FullPath

                    m_oExplorer.GetNodeIdFromFullPath(v_sFullKey:=sParentCode.ToString(), r_lNodeId:=lNodeId, v_vCompanyId:=lCompanyID)

                    If lNodeId = NO_NODE_FOR_MAPPING Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to GetNodeIdFromFullPath for CreatePath.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreatePath", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                If lNodeId = 0 Then
                    'err
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            If bCreateAccount Then Exit Do 'account created so all done

            iStart = iEnd + 1
            iEnd = v_sRelativeCode.IndexOf("\"c, iStart + 1)

        Loop

        Return result

    End Function

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

            ' Error.
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

            ' Error.
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
    'UPGRADE_NOTE: (7001) The following declaration (RollbackTrans) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
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
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '

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
        ' Error.
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        ''

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    ' ***************************************************************** '
    '
    ' Name: CheckTransactionExportComplete
    '
    ' Description:
    '
    ' History: 12/07/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function CheckTransactionExportComplete(ByVal v_lTransactionExportFolderCnt As Integer, ByRef r_bTransactionExportComplete As Boolean) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Const ACSelectTransactionExportCompleteStored As Boolean = True
        Const ACSelectTransactionExportCompleteName As String = "SelectTransactionExportComplete"

        Const ACSelectTransactionExportCompleteSQL As String = "spu_transaction_export_complete_sel"
        Dim vResultArray(,) As Object = Nothing

        With m_oDatabase

            .Parameters.Clear()

            'Transaction_export_folder_cnt
            m_lReturn = .Parameters.Add(sName:="transaction_export_folder_cnt", vValue:=CStr(v_lTransactionExportFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .SQLSelect(sSQL:=ACSelectTransactionExportCompleteSQL, sSQLName:=ACSelectTransactionExportCompleteName, bStoredProcedure:=ACSelectTransactionExportCompleteStored, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get TransactionExportComplete from database", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckTransactionExportComplete")
                Return result
            End If
        End With

        r_bTransactionExportComplete = Informations.IsArray(vResultArray)

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateTransactionExportComplete
    '
    ' Description:
    '
    ' History: 12/07/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateTransactionExportComplete(ByVal v_lTransactionExportFolderCnt As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Const ACAddTransactionExportCompleteStored As Boolean = True
        Const ACAddTransactionExportCompleteName As String = "UpdateTransactionExportComplete"

        Const ACAddTransactionExportCompleteSQL As String = "spu_transaction_export_complete_add"

        With m_oDatabase

            .Parameters.Clear()

            'Transaction_export_folder_cnt
            m_lReturn = .Parameters.Add(sName:="transaction_export_folder_cnt", vValue:=CStr(v_lTransactionExportFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .SQLAction(sSQL:=ACAddTransactionExportCompleteSQL, sSQLName:=ACAddTransactionExportCompleteName, bStoredProcedure:=ACAddTransactionExportCompleteStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetPremiumFinanceRecord
    '
    ' Description: Get's the Premium finance record from the insurance file count.
    '
    ' Written By: Simon.Baynes
    '
    ' Returns: Finance Plan array
    '
    ' Date: 22/10/2003
    '
    ' ***************************************************************** '
    Private Function GetPremiumFinanceRecord(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lFinancePlanArray As Object) As Integer

        Dim result As Integer = 0

        Const sSQL As String = "spu_PFPremiumFinance_Sel_SingleFromInsuranceFileCount"
        Const sSQLName As String = "GetPremiumFinanceRecord"
        Const sSQLStored As Boolean = True

        Dim vResults(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:=sSQLName, bStoredProcedure:=sSQLStored, vResultArray:=vResults)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetPremiumFinanceRecord failed", vApp:=ACApp, vClass:=ACClass, vMethod:=result)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End With

        If Not Informations.IsArray(vResults) Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get finance plan array", vApp:=ACApp, vClass:=ACClass, vMethod:=result)
            Return gPMConstants.PMEReturnCode.PMFalse
        Else

            r_lFinancePlanArray = vResults
        End If

        Return result

    End Function

    Private Function GetGLAccount(ByVal v_cAmount As Decimal, ByRef r_lGLAccountID As Integer) As Integer

        Dim result As Integer = 0
        Dim sShortCode As String = ""
        Dim oAllocation As New bACTAllocation.Form



        result = gPMConstants.PMEReturnCode.PMTrue

        If v_cAmount > 0 Then
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=ACCurrencyDifferenceDebitAccount, r_sOptionValue:=sShortCode)
        Else
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=ACCurrencyDifferenceCrebitAccount, r_sOptionValue:=sShortCode)
        End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or sShortCode = "" Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If oAllocation Is Nothing Then

            oAllocation = New bACTAllocation.Form
            m_lReturn = oAllocation.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        ' Get the account_id from the business

        m_lReturn = oAllocation.GetAccountID(v_sShortCode:=sShortCode, r_lAccountID:=r_lGLAccountID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        oAllocation.Dispose()
        oAllocation = Nothing

        Return result

    End Function

    Public Function AutoAllocateCancellingTransactions(ByVal vTransactions(,) As Object, Optional ByVal bAllowPartialAllocation As Boolean = False, Optional ByVal cWriteOffAmount As Decimal = 0, Optional ByVal lWriteOffReasonID As Integer = 0, Optional ByVal bCurrencyWriteOff As Boolean = False, Optional ByVal lCashListItemId As Integer = 0, Optional sDocBusinessTypeCode As String = "", Optional ByVal v_lPaymentAccount As Integer = 0) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: AutoAllocateCancellingTransactions
        ' PURPOSE: Automatically allocate transactions on the same account that
        '          cancel each other out
        ' AUTHOR: Danny Davis
        ' DATE: 14 September 2004, 09:59:35
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim nResult As Integer = 0
        Dim lAccountId As Integer
        Dim cSum As Decimal
        Dim lFirstRow, lLastRow As Integer
        Dim vAllocationArray() As Object
        Dim oAllocation As  bACTAllocationManual.Business
        Dim lLower, lUpper As Integer
        Dim bAccountCheck As Boolean = False
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            'Sort into Account order
            m_lReturn = gPMFunctions.ShellSort2DArray(vTransactions, 0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If


            lLower = vTransactions.GetLowerBound(1)
            lUpper = vTransactions.GetUpperBound(1)

            'Move through the array
            lAccountId = 0


            Dim bIsCoinsurance As Boolean = False
            If sDocBusinessTypeCode.ToUpper() = "COIN LEAD" Or sDocBusinessTypeCode.ToUpper() = "COIN FOLL" Then
                bIsCoinsurance = True
            End If


            For lRow As Integer = lLower To lUpper
                ' Has the account changed?
                If lAccountId <> gPMFunctions.NullToLong(vTransactions(0, lRow)) OrElse bAccountCheck = True Then
                    ' If we have found more than one line and the sum is zero then process
                    If (v_lPaymentAccount = 0 OrElse lAccountId = v_lPaymentAccount) AndAlso ((lRow - lFirstRow) > 1 And (cSum = 0 Or bAllowPartialAllocation) And bIsCoinsurance = False) Then
                        ' Last row to match is now the last row we looked at!
                        If bAccountCheck = True Then
                            lLastRow = lRow
                        Else
                            lLastRow = lRow - 1
                        End If

                        If oAllocation Is Nothing Then
                            'Get Instance of Allocation Manual

                            oAllocation = New bACTAllocationManual.Business
                            m_lReturn = oAllocation.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", gPMComponentServices.CreateBusinessObject Failed for bACTAllocationManual.Business")
                            End If
                        End If

                        'Build the allocation array
                        ReDim vAllocationArray(lLastRow - lFirstRow - 1)
                        For lAllocationRow As Integer = 0 To (lLastRow - lFirstRow - 1)
                            vAllocationArray(lAllocationRow) = CStr(vTransactions(1, lAllocationRow + lFirstRow + 1)) &
                                                      "|" & CStr(vTransactions(2, lAllocationRow + lFirstRow + 1))
                        Next lAllocationRow

                        'Set keys for the AllocationManual component
                        Dim vKeyArray As Array = Array.CreateInstance(GetType(Object), New Integer() _
                                                                         {gPMConstants.PMENavLetGetKeyColPosition.
                                                                              PMKeyValue _
                                                                          -
                                                                          gPMConstants.PMENavLetGetKeyColPosition.
                                                                              PMKeyName + 1, 7},
                                                                      New Integer() _
                                                                         {gPMConstants.PMENavLetGetKeyColPosition.
                                                                         PMKeyName, 0})

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameTransDetailID

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameTransDetailIDs

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = lAccountId

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = CStr(vTransactions(1, lFirstRow)) & "|" & CStr(vTransactions(2, lFirstRow))

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = vAllocationArray

                        ' Pass cash list item Id

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.ACTKeyNameCashListItemId

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = lCashListItemId
                        'vKeys(PMKeyName, 3) = ACTKeyNameTransDetailId
                        'vKeys(PMKeyValue, 3) = CashTransId

                        If cWriteOffAmount <> 0 Then
                            ' Write Off Reason

                            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.ACTKeyNameWriteOffReasonId

                            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = lWriteOffReasonID

                            ' if its a currency write off
                            If bCurrencyWriteOff Then
                                ' pass through the amount in the currency difference field

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.ACTKeyNameCurrencyDifference

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = cWriteOffAmount
                            Else
                                ' else set the currency difference = 0

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.ACTKeyNameCurrencyDifference

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = 0

                            End If

                            ' set the write off amount

                            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.ACTKeyNameWriteOffAmount

                            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = cWriteOffAmount

                        End If

                        'Perform the allocation
                        With oAllocation

                            If .SetKeys(vKeyArray:=vKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", Set Keys Failed")
                            End If

                            If .Start() <> gPMConstants.PMEReturnCode.PMTrue Then
                                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", Allocation Start Failed")
                            End If
                        End With

                    End If

                    'Reset for next account
                    lFirstRow = lRow
                    lAccountId = gPMFunctions.NullToLong(vTransactions(0, lRow))
                    cSum = 0
                End If
                If lRow + 1 = lUpper Then
                    bAccountCheck = True
                    cSum += gPMFunctions.NullToCurrency(vTransactions(2, lRow))
                    cSum += gPMFunctions.NullToCurrency(vTransactions(2, lRow + 1))
                Else
                    cSum += gPMFunctions.NullToCurrency(vTransactions(2, lRow))
                End If
            Next lRow

            If Not (oAllocation Is Nothing) Then

                oAllocation.Dispose()
                oAllocation = Nothing
            End If


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoAllocateCancellingTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    nResult = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally

        End Try
        Return nResult
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetTransDetailTypeID
    '
    ' Description: Get TransDetail_Type ID from code
    '
    ' History: 12/07/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function GetTransDetailTypeID(ByVal sTransdetailTypeCode As String, ByRef lTransdetailTypeID As Integer) As Integer
        Dim Catch_Renamed As Boolean = False

        Dim result As Integer = 0
        Dim vTransDetailTypeId(,) As Object = Nothing

        Const ACGetTransDetailTypeIDStored As Boolean = False
        Const ACGetTransDetailTypeIDName As String = "GetTransDetailTypeID"
        Const ACGetTransDetailTypeIDSQL As String = "SELECT transdetail_type_id from transdetail_type where code = {code}"

        result = gPMConstants.PMEReturnCode.PMTrue

        Try
            Catch_Renamed = True
            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="code", vValue:=sTransdetailTypeCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                m_lReturn = .SQLSelect(sSQL:=ACGetTransDetailTypeIDSQL, sSQLName:=ACGetTransDetailTypeIDName, bStoredProcedure:=ACGetTransDetailTypeIDStored, vResultArray:=vTransDetailTypeId)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", GetTransDetailTypeID Failed to get TransdetailTypeID - bACTImportSiriusTrans.Form")
                End If
            End With

            If Informations.IsArray(vTransDetailTypeId) Then
                lTransdetailTypeID = gPMFunctions.NullToLong(vTransDetailTypeId(0, 0))
            Else
                lTransdetailTypeID = 1 'Default to Journal
            End If

            Return result

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If

            GoTo Finally_Renamed

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

            If Catch_Renamed Then

                Select Case Informations.Err().Number
                    Case Else

                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTransDetailTypeID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                        result = gPMConstants.PMEReturnCode.PMFalse

                        GoTo Finally_Renamed
                End Select
            End If
Finally_Renamed:
        End Try
    End Function
    Private Function GetDestinationAccountId(ByVal v_iReleaseToIncome As Integer, ByVal v_lAccountID As Integer, ByVal v_sAccountName As String, ByVal v_sReleaseAccountCode As String, ByRef r_lDestinationAccountId As Integer) As Integer
        Dim Catch_Renamed As Boolean = False
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: Function GetDestinationAccountId
        ' PURPOSE: Get Destination Account Id for suspense transaction
        ' AUTHOR: Elaine Knott
        ' DATE: DEC 2004
        ' REMARKS: FSA Phase 3.2 + tidy up (PN22188)
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const ACGetDestinationAccountIdStored As Boolean = False
        Const ACGetDestinationAccountIdName As String = "GetDestinationAccountId"
        Dim sGetDestinationAccountIdSQL As String = ""
        Dim vResults(,) As Object = Nothing
        Dim bError As Boolean

        Try
            Catch_Renamed = True
            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()
                If v_sReleaseAccountCode <> "" Then

                    sGetDestinationAccountIdSQL = "SELECT account_id from account where short_code = {code}"

                    m_lReturn = .Parameters.Add(sName:="Code", vValue:=v_sReleaseAccountCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    m_lReturn = .SQLSelect(sSQL:=sGetDestinationAccountIdSQL, sSQLName:=ACGetDestinationAccountIdName, bStoredProcedure:=ACGetDestinationAccountIdStored, vResultArray:=vResults)

                Else
                    sGetDestinationAccountIdSQL = "SELECT ea.account_map_id " &
                                                  "FROM elementextras ea " &
                                                  "JOIN structuretree s on s.element_id = ea.element_id " &
                                                  "WHERE s.account_id  = {v_lAccountID}"

                    m_lReturn = .Parameters.Add(sName:="v_lAccountId", vValue:=CStr(v_lAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    m_lReturn = .SQLSelect(sSQL:=sGetDestinationAccountIdSQL, sSQLName:=ACGetDestinationAccountIdName, bStoredProcedure:=ACGetDestinationAccountIdStored, vResultArray:=vResults)

                End If

            End With

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                If Informations.IsArray(vResults) Then

                    If Not (CStr(vResults(0, 0)).Trim() = "") And Not (CStr(vResults(0, 0)).Trim() = "0") Then

                        r_lDestinationAccountId = CInt(vResults(0, 0))
                    Else
                        bError = True
                    End If
                Else
                    bError = True
                End If
            Else
                bError = True
            End If

            If bError Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", " + "bACTImportSiriusTrans.Form GetDestinationAccountId Failed to get destinationAccountID for " & v_sAccountName & ". Please ensure that the account is mapped to an income account in Account " & "Explorer/Balance Sheet/Liabilities/Current Liabilities/Commission. Right click and select Extras.")
            End If

            Return result

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If

            GoTo Finally_Renamed

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

            If Catch_Renamed Then

                Select Case Informations.Err().Number
                    Case Else

                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDestinationAccountId", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                        result = gPMConstants.PMEReturnCode.PMFalse

                        GoTo Finally_Renamed
                End Select
            End If
Finally_Renamed:
        End Try
    End Function

    Public Function WriteSuspendedAccountsTransactions(ByVal vSuspenseArray(,) As Object, ByVal lDocumentId As Integer, Optional ByVal lLinkedTransactionID As Integer = 0) As Integer
        Dim Catch_Renamed As Boolean = False
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: Function WriteSuspendedAccountsTransactions
        ' PURPOSE: Write transactions which move based on commission movement system option
        '          to the suspense file
        ' AUTHOR: Elaine Knott
        ' DATE: DEC 2004
        ' REMARKS: FSA Phase 3.2
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim vResults(,) As Object = Nothing

        ' Select Document SQL
        'Const ACGetTriggersStored As Boolean = True
        'Const ACGetTriggersName As String = "SelectTriggers"

        'Const ACGetTriggersSQL As String = "spu_ACT_Get_SuspendedAccountsTransactions_Triggers"

        ' Const ACGetFeeTriggersStored As Boolean = True
        'Const ACGetFeeTriggersName As String = "SelectFeeTriggers"

        ' Const ACGetFeeTriggersSQL As String = "spu_ACT_Get_SuspendedFeeTransactions_Triggers"

        'Const ACGetTaxTriggersStored As Boolean = True
        'Const ACGetTaxTriggersName As String = "SelectTaxTriggers"

        'Const ACGetTaxTriggersSQL As String = "spu_ACT_Get_SuspendedTaxTransactions_Triggers"

        'Const ACGetAgentTaxTriggersStored As Boolean = True
        'Const ACGetAgentTaxTriggersName As String = "SelectAgentTaxTriggers"

        'Const ACGetAgentTaxTriggersSQL As String = "spu_ACT_Get_SuspendedAgentTaxTransactions_Triggers"

        'Values for Trigger Array
        Dim lTriggerTransdetailId As Integer
        Dim dPercentage As Double

        'Values for Suspense Array
        Dim lTransdetailId As Integer
        Dim vPremiumFinanceCnt, vPremiumFinanceVersion As Object
        Dim lDestinationAccountId, lInsuranceFileCnt As Integer
        Dim iDocumentTYpeID As Integer
        Dim lTransdetailTypeID As Integer

        Dim sTransdetailTypeCode As String = ""


        '(RC) PLICO 9-10
        Dim bManuallyReleased, bReleasedOnFullSettlement, bReleasedForWholePosting, bReleasedOnPolicyEffective As Boolean

        Try
            Catch_Renamed = True

            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim vResults(1, 0)

            vResults(0, 0) = gPMFunctions.ToSafeLong(lLinkedTransactionID, 0)

            vResults(1, 0) = 100

            'suspend Commission using Commission Movement Trigger

            For lTriggerCount As Integer = 0 To vResults.GetUpperBound(1)
                lTriggerTransdetailId = gPMFunctions.ToSafeLong(vResults(0, lTriggerCount))

                dPercentage = CDbl(vResults(1, lTriggerCount))

                For lSuspenseCount As Integer = 0 To vSuspenseArray.GetUpperBound(1)

                    lTransdetailId = CInt(vSuspenseArray(0, lSuspenseCount))

                    vPremiumFinanceCnt = vSuspenseArray(1, lSuspenseCount)

                    vPremiumFinanceVersion = vSuspenseArray(2, lSuspenseCount)

                    lDestinationAccountId = CInt(vSuspenseArray(3, lSuspenseCount))

                    lInsuranceFileCnt = CInt(vSuspenseArray(4, lSuspenseCount))

                    iDocumentTYpeID = CInt(vSuspenseArray(5, lSuspenseCount))

                    lTransdetailTypeID = CInt(vSuspenseArray(6, lSuspenseCount))

                    sTransdetailTypeCode = CStr(vSuspenseArray(8, lSuspenseCount))

                    '(RC) PLICO 9-10
                    bManuallyReleased = gPMFunctions.ToSafeBoolean(vSuspenseArray(9, lSuspenseCount))
                    bReleasedOnFullSettlement = gPMFunctions.ToSafeBoolean(vSuspenseArray(10, lSuspenseCount))
                    bReleasedForWholePosting = gPMFunctions.ToSafeBoolean(vSuspenseArray(11, lSuspenseCount))
                    bReleasedOnPolicyEffective = gPMFunctions.ToSafeBoolean(vSuspenseArray(12, lSuspenseCount))

                    'Don't suspend fees until we have checked the new option
                    If sTransdetailTypeCode <> "FEE" And sTransdetailTypeCode <> "CFEE" And sTransdetailTypeCode <> "TAX" And sTransdetailTypeCode <> "AGENTTAX" Then

                        m_lReturn = m_oTransdetail.CreateSuspendedTransaction(lSuspendedTransdetailId:=lTransdetailId, lLinkedTransdetailID:=lTriggerTransdetailId, dLinkedPercentage:=dPercentage, vPFPremFinanceCnt:=vPremiumFinanceCnt, vPFPremFinanceVersion:=vPremiumFinanceVersion, lInsuranceFileCnt:=lInsuranceFileCnt, lDestinationAccountID:=lDestinationAccountId, lDocumentTypeId:=iDocumentTYpeID, lTransdetailTypeID:=lTransdetailTypeID, bManuallyReleased:=bManuallyReleased, bReleasedOnFullSettlement:=bReleasedOnFullSettlement, bReleasedForWholePosting:=bReleasedForWholePosting, bReleasedOnPolicyEffective:=bReleasedOnPolicyEffective, sSpare:="")
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", WriteSuspendedAccountsTransactions - Failed to Write SuspendedAccountsTransaction - bACTImportSiriusTrans.Form")
                        End If
                    End If
                Next lSuspenseCount
            Next lTriggerCount

            'DC260606 Datasure -end

            Return result

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If

            GoTo Finally_Renamed

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

            If Catch_Renamed Then

                Select Case Informations.Err().Number
                    Case Else

                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="WriteSuspendedAccountsTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                        result = gPMConstants.PMEReturnCode.PMFalse

                        GoTo Finally_Renamed
                End Select

            End If
Finally_Renamed:
        End Try
    End Function

    Public Function GetInsurerRiskTransferAgreement(ByVal lDocumentId As Integer, ByVal lAccountId As Integer) As Integer

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim vRiskTransferAgreement(,) As Object = Nothing

        Const AC_SQL_RiskTransferStatus_SP As Boolean = True
        Const AC_SQL_RiskTransferStatus_Name As String = "GetRiskTransferStatus"
        Const AC_SQL_RiskTransferStatus_SQL As String = "spu_TRN_risk_transfer_status_select"

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="DocumentId", vValue:=CStr(lDocumentId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = .Parameters.Add(sName:="AccountId", vValue:=CStr(lAccountId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLSelect(sSQL:=AC_SQL_RiskTransferStatus_SQL, sSQLName:=AC_SQL_RiskTransferStatus_Name, bStoredProcedure:=AC_SQL_RiskTransferStatus_SP, vResultArray:=vRiskTransferAgreement)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", GetInsurerRiskTransferAgreement Failed to get Risk_Transfer_Agreement - bACTImportSiriusTrans.Form")
                End If

            End With

            If Informations.IsArray(vRiskTransferAgreement) Then

                If gPMFunctions.ToSafeBoolean(vRiskTransferAgreement(0, 0)) = True Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsurerRiskTransferAgreement", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select
        Finally

        End Try
        Return result

    End Function

    Private Function GetDueDateForTransactions(ByVal v_lInsuranceFileCnt As Integer,
                                               ByVal v_lAccountID As Integer,
                                               ByRef v_dtDueDate As Date) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetDueDateForTransactions"

        Const ACGetDueDateForTransactionsSQL As String = "spu_Get_Due_Date_For_Transactions"
        Const ACGetDueDateForTransactionsName As String = "GetDueDateForTransactionsName"
        Const ACGetDueDateForTransactionsStored As Boolean = True

        Const kDueDateCol As Integer = 0

        Dim vResults(,) As Object = Nothing
        result = gPMConstants.PMEReturnCode.PMTrue



        With m_oDatabase

            .Parameters.Clear()
            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", v_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "account_id", v_lAccountID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = .SQLSelect(sSQL:=ACGetDueDateForTransactionsSQL, sSQLName:=ACGetDueDateForTransactionsName,
                                   bStoredProcedure:=ACGetDueDateForTransactionsStored,
                                   vResultArray:=vResults)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACGetDueDateForTransactionsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Informations.IsArray(vResults) Then
                gPMFunctions.RaiseError(kMethodName, "No record found for the insurance file cnt: " & v_lInsuranceFileCnt, gPMConstants.PMELogLevel.PMLogError)
            Else
                v_dtDueDate = gPMFunctions.ToSafeDate(vResults(kDueDateCol, 0))
            End If

        End With

        Return result


    End Function

    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub

    ''' <summary>
    ''' Check if an account exists
    ''' </summary>
    ''' <param name="sShortCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DoAccountExists(ByVal sShortCode As String) As Integer
        Dim oResultArray(,) As Object = Nothing
        Try

            With m_oDatabase
                .Parameters.Clear()
                m_lReturn = .Parameters.Add(
                            sName:="short_code",
                            vValue:=sShortCode,
                            iDirection:=PMEParameterDirection.PMParamInput,
                            iDataType:=PMEDataType.PMString)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Throw New Exception("Failed to add paramter short_code")
                End If

                m_lReturn = .SQLSelect(sSQL:=kDoAccountExistsSQL,
                                       sSQLName:=kDoAccountExistsName,
                                       bStoredProcedure:=kDoAccountExistsStored,
                                       lNumberRecords:=gPMConstants.PMAllRecords,
                                       vResultArray:=oResultArray)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Throw New Exception("Failed to add paramter short_code")
                End If
            End With

            If Informations.IsArray(oResultArray) Then
                Return PMEReturnCode.PMTrue
            Else
                Return PMEReturnCode.PMNotFound
            End If

        Catch ex As Exception
            Return PMEReturnCode.PMFalse
        End Try

    End Function



End Class
