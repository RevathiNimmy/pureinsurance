Option Strict Off
Option Explicit On
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Application_NET.Application")>
Public NotInheritable Class Application
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Application
    '
    ' Date: CL020600
    '
    ' Description: Back Office Mapper for the GeminiNet (motor) solution.
    '
    ' Edit History:
    '
    '   PW181105 - Changed code that was assuming InsuranceFileCnt is stored
    '              on gis_policy_link. InsuranceFolderCnt is still stored in
    '              there at present.
    ' ***************************************************************** '

#Const BypassAutoPosting = False

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Application"

    Private Const ACProductID As String = "PRODUCT_ID"
    Private Const ACConsolidatedLeadAgentCommission As String = "CLAC"
    Private Const ACConsolidatedSubAgentCommission As String = "CSAC"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Data Set Definition
    Private m_oDataSet As cGISDataSetControl.Application

    Private m_lReturn As Integer = 0

    Private m_sGISDataModel As String = ""

    ' SQL String used for Adding/Updating Objects in DB
    Private m_sAddSQL As String = ""
    Private m_sUpdateSQL As String = ""
    Private m_sDeleteSQL As String = ""
    Private m_sAddUpdateSQL As String = "" 'sj 18/08/99

    Public m_vIsRI2007 As String = ""

    ' ************************************************
    ' Added to replace global variables 18/09/2003
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
    ' ************************************************
    'SJ 11/03/2004 - start
    Private m_bUnderwritingBranchEnabled As Boolean
    Private m_bIsUnderwritingBranch As Boolean
    'SJ 11/03/2004 - end


    Public Property GISDataModel() As String
        Get
            Return m_sGISDataModel
        End Get
        Set(ByVal Value As String)
            m_sGISDataModel = Value
        End Set
    End Property

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    ' ***************************************************************** '
    ' Name: CreatePostTransactionsWMTask
    '
    ' Description: Creates a WM Task instance for the posting of
    ' transactions.
    ' ***************************************************************** '
    Public Function CreatePostTransactionsWMTask(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_lInsFileCnt As Integer, ByRef r_sClientName As String, ByRef r_sPolicyNo As String) As Integer

        Dim result As Integer = 0
        Dim bPMWrkManInst As bPMWrkTaskInstance.TaskControl
        Dim vKeys As Object
        Dim lCnt As Integer
        Dim sTaskGroupID, sUserGroupID, sUserID As String
        Dim lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    Set bPMWrkManInst = CreateObject("bPMWrkTaskInstance.TaskControl")
            '
            '    lReturn& = bPMWrkManInst.Initialise(sUsername:=tosafestring(m_sUsername), _
            ''                                    sPassword:=tosafestring(m_sPassword), _
            ''                                    iUserID:=tosafeinteger(m_iUserID), _
            ''                                    iSourceID:=tosafeinteger(m_iSourceID), _
            ''                                    iLanguageID:=tosafestring(m_iLanguageID), _
            ''                                    iCurrencyID:=tosafeinteger(m_iCurrencyID), _
            ''                                    iLogLevel:=tosafeinteger(m_iLogLevel), _
            ''                                    sCallingAppName:=tosafestring(ACApp))

            ' Create bSiriusLink object
            bPMWrkManInst = New bPMWrkTaskInstance.TaskControl
            lReturn = bPMWrkManInst.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = lReturn

                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise bPMWrkTaskInstance.TaskControl", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="CreatePostTransactionsWMTask", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'Get the reg settings
            lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode,
                                                                       v_sBusinessTypeCode:=v_sGisBusinessTypeCode,
                                                                       v_sSettingName:="PostTransTaskGroupID",
                                                                       r_sSettingValue:=sTaskGroupID)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn

                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to GetRegSettingFromDataBusModel PostTransTaskGroupID", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="CreatePostTransactionsWMTask", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'Get the reg settings
            lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode,
                                                                       v_sBusinessTypeCode:=v_sGisBusinessTypeCode,
                                                                       v_sSettingName:="PostTransUserGroupID",
                                                                       r_sSettingValue:=sUserGroupID)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = lReturn

                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to GetRegSettingFromDataBusModel PostTransUserGroupID", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="CreatePostTransactionsWMTask", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'Get the reg settings
            lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode,
                                                                       v_sBusinessTypeCode:=v_sGisBusinessTypeCode,
                                                                       v_sSettingName:="PostTransUserID",
                                                                       r_sSettingValue:=sUserID)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = lReturn

                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to GetRegSettingFromDataBusModel PostTransUserID", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="CreatePostTransactionsWMTask", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'Create the Key Array
            ReDim vKeys(1, 2)

            vKeys(0, 0) = PMNavKeyConst.PMKeyNameClientName

            vKeys(1, 0) = r_sClientName.Trim()

            vKeys(0, 1) = PMNavKeyConst.PMKeyNameInsFileCnt

            vKeys(1, 1) = r_lInsFileCnt

            vKeys(0, 2) = PMNavKeyConst.PMKeyNamePolicyNo

            vKeys(1, 2) = r_sPolicyNo.Trim()

            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Before CreateNewByCode values : v_lPMWrkTaskGroupID=" & sTaskGroupID &
                              " v_sCustomer=" & r_sClientName &
                              " v_dtTaskDueDate=" & String.Format("{0:d}", Informations.DateAdd("ww", 1, DateTime.Today)) &
                              " v_lPMUserGroupID=" & sUserGroupID &
                              " r_lPMWrkTaskInstanceCnt=" & ToSafeString(lCnt) &
                                              " v_iUserId=" & sUserID, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="CreatePostTransactionsWMTask")



            lReturn = bPMWrkManInst.CreateNewByCode(v_lPMWrkTaskGroupID:=CInt(sTaskGroupID), v_sPMWrkTaskCode:="DMCPOST", v_sCustomer:=r_sClientName, v_dtTaskDueDate:=Informations.DateAdd("ww", 1, DateTime.Today), v_lPMUserGroupID:=CInt(sUserGroupID), v_sDescription:="Post Transactions for Policy '" & r_sPolicyNo & "'", v_iTaskStatus:=0, v_iIsUrgent:=0, r_lPMWrkTaskInstanceCnt:=lCnt, v_iUserID:=CInt(sUserID), v_vKeyArray:=vKeys, v_iIsVisible:=1)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = lReturn

                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to CreateNewByCode in bPMWrkTaskInstance.TaskControl", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="CreatePostTransactionsWMTask", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            ' Destroy the object

            bPMWrkManInst.Dispose()
            bPMWrkManInst = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreatePostTransactionsWMTask Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="CreatePostTransactionsWMTask", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result

        End Try
    End Function
    Public Function CreateChangeAddressWMTask(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_lInsFileCnt As Integer, ByRef r_sClientName As String, ByRef r_sPolicyNo As String) As Integer

        Dim result As Integer = 0
        Dim bPMWrkManInst As bPMWrkTaskInstance.TaskControl
        Dim vKeys As Object
        Dim lCnt As Integer
        Dim sTaskGroupID, sUserGroupID As String
        Dim lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    Set bPMWrkManInst = CreateObject("bPMWrkTaskInstance.TaskControl")
            '
            '    m_lReturn = bPMWrkManInst.Initialise(sUsername:=tosafestring(m_sUsername), _
            ''                                    sPassword:=tosafestring(m_sPassword), _
            ''                                    iUserID:=tosafeinteger(m_iUserID), _
            ''                                    iSourceID:=tosafeinteger(m_iSourceID), _
            ''                                    iLanguageID:=tosafestring(m_iLanguageID), _
            ''                                    iCurrencyID:=tosafeinteger(m_iCurrencyID), _
            ''                                    iLogLevel:=tosafeinteger(m_iLogLevel), _
            ''                                    sCallingAppName:=tosafestring(ACApp))

            ' Create bSiriusLink object
            bPMWrkManInst = New bPMWrkTaskInstance.TaskControl
            lReturn = bPMWrkManInst.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = lReturn

                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError,
                                                  sMsg:="Failed to initialise bPMWrkTaskInstance.TaskControl",
                                                  vApp:=ToSafeString(ACApp), vClass:=ACClass,
                                                  vMethod:="CreateChangeAddressWMTask",
                                                  vErrNo:=Informations.Err().Number,
                                                  vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'Get the reg settings
            lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode,
                                                                       v_sBusinessTypeCode:=v_sGisBusinessTypeCode,
                                                                       v_sSettingName:="ChangeAddressTaskGroupID",
                                                                       r_sSettingValue:=sTaskGroupID)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = lReturn

                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError,
                                                  sMsg:=
                                                     "Failed to GetRegSettingFromDataBusModel ChangeAddressTaskGroupID",
                                                  vApp:=ToSafeString(ACApp), vClass:=ACClass,
                                                  vMethod:="CreateChangeAddressWMTask",
                                                  vErrNo:=Informations.Err().Number,
                                                  vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'Get the reg settings
            lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode,
                                                                       v_sBusinessTypeCode:=v_sGisBusinessTypeCode,
                                                                       v_sSettingName:="ChangeAddressUserGroupID",
                                                                       r_sSettingValue:=sUserGroupID)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = lReturn

                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError,
                                                  sMsg:=
                                                     "Failed to GetRegSettingFromDataBusModel ChangeAddressUserGroupID",
                                                  vApp:=ToSafeString(ACApp), vClass:=ACClass,
                                                  vMethod:="CreateChangeAddressWMTask",
                                                  vErrNo:=Informations.Err().Number,
                                                  vErrDesc:=Informations.Err().Description)
                Return result
            End If

            Dim dbNumericTemp2 As Double
            Dim dbNumericTemp As Double
            If _
                (Not _
                 Double.TryParse(sUserGroupID, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat,
                                 dbNumericTemp)) Or
                (Not _
                 Double.TryParse(sTaskGroupID, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat,
                                 dbNumericTemp2)) Then
                GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError,
                                                       sMsg:="Invalid Reg Settings", vApp:=ToSafeString(ACApp), vClass:=ACClass,
                                                       vMethod:="CreateChangeAddressWMTask")
                Return result
            End If

            'Create the Key Array
            ReDim vKeys(1, 2)

            vKeys(0, 0) = PMNavKeyConst.PMKeyNameClientName

            vKeys(1, 0) = r_sClientName.Trim()

            vKeys(0, 1) = PMNavKeyConst.PMKeyNameInsFileCnt

            vKeys(1, 1) = r_lInsFileCnt

            vKeys(0, 2) = PMNavKeyConst.PMKeyNamePolicyNumber

            vKeys(1, 2) = r_sPolicyNo.Trim()

            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError,
                                                   sMsg:=
                                                      "Before CreateNewByCode values : v_lPMWrkTaskGroupID=" &
                                                      sTaskGroupID &
                                                      " v_sCustomer=" & r_sClientName &
                                                      " v_dtTaskDueDate=" &
                                                      String.Format("{0:d}",
                                                                    Informations.DateAdd("ww", 1, DateTime.Today)) &
                                                      " v_lPMUserGroupID=" & sUserGroupID &
                                                      " r_lPMWrkTaskInstanceCnt=" & ToSafeString(lCnt), vApp:=ToSafeString(ACApp),
                                                   vClass:=ACClass, vMethod:="CreateChangeAddressWMTask")


            lReturn = bPMWrkManInst.CreateNewByCode(v_lPMWrkTaskGroupID:=CInt(sTaskGroupID),
                                                    v_sPMWrkTaskCode:="MEMO", v_sCustomer:=r_sClientName,
                                                    v_dtTaskDueDate:=DateTime.Today,
                                                    v_lPMUserGroupID:=CInt(sUserGroupID),
                                                    v_sDescription:=
                                                       "MTA performed on policy '" & r_sPolicyNo &
                                                       "'. Check other policies for change of address.",
                                                    v_iTaskStatus:=0, v_iIsUrgent:=0,
                                                    r_lPMWrkTaskInstanceCnt:=lCnt, v_iUserID:=0,
                                                    v_vKeyArray:=vKeys, v_iIsVisible:=1)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = lReturn

                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError,
                                                  sMsg:="Failed to CreateNewByCode in bPMWrkTaskInstance.TaskControl",
                                                  vApp:=ToSafeString(ACApp), vClass:=ACClass,
                                                  vMethod:="CreateChangeAddressWMTask",
                                                  vErrNo:=Informations.Err().Number,
                                                  vErrDesc:=Informations.Err().Description)

                Return result
            End If

            'Destroy the object

            bPMWrkManInst.Dispose()
            bPMWrkManInst = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                              sMsg:="CreateChangeAddressWMTask Failed", vApp:=ToSafeString(ACApp),
                                              vClass:=ACClass, vMethod:="CreateChangeAddressWMTask",
                                              vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    Private Function GetAge(ByRef dtDOB As Date) As Integer


        Dim idd As Integer = dtDOB.Day
        Dim imm As Integer = dtDOB.Month
        Dim iyyyy As Integer = dtDOB.Year

        Dim iAge As Integer = DateTime.Today.Year - iyyyy

        If imm > DateTime.Today.Month Then
            iAge -= 1
        End If

        If imm = DateTime.Today.Month And idd > DateTime.Today.Day Then
            iAge -= 1
        End If

        Return iAge
    End Function

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
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lReturn As Integer

            m_oDataSet = New cGISDataSetControl.Application()

            ' Initialisation Code.

            ' Set Username and Password
            m_sUsername = sUserName
            m_sPassword = sPassword

            ' Set User ID
            m_iUserID = iUserID

            ' Set Calling Application
            m_sCallingAppName = sCallingAppName

            ' Set Language ID
            m_iLanguageID = iLanguageID

            ' Set Source ID
            m_iSourceID = iSourceID

            ' Set Currency ID
            m_iCurrencyID = iCurrencyID

            ' Set Log Level
            m_iLogLevel = iLogLevel


            If Informations.IsNothing(vDatabase) Then
                lReturn = gPMComponentServices.CheckDatabase(v_sUsername:=m_sUsername, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase)
            Else

                lReturn = gPMComponentServices.CheckDatabase(v_sUsername:=m_sUsername, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)
            End If

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed - Failed to create connection to Database", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:="Failed to create connection to Database")
                Return result
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

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
                m_oDataSet = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub



    ' ***************************************************************** '
    ' Name: NBStartBefore
    '
    ' Description: This method is called by the GIS when a New Business
    '              Quote is started.
    '
    '              Called BEFORE GIS has done its stuff.
    ' RFC110800 - Use a GUID for the Insurance Folder Code as it has to be unique.
    ' ***************************************************************** '
    Public Function NBStartBefore(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lPartyCnt As Integer, ByRef r_lInsuranceFileCnt As Integer, ByRef r_sQuoteRef As String, ByRef r_sQuoteRefPassword As String, ByRef r_vAdditionalDataArray As Array) As Integer
        Dim result As Integer = 0
        Dim sReturn As String = ""
        Dim lSourceID As Integer
        Dim oSBOLink As bSIRIUSLink.SIRIUSLink
        Dim sInsFolderCode As String = ""
        Dim dtCoverStartDate, dtExpiryDate As Date
        Dim sInsuredName As String = ""
        Dim lInsuranceFolderCnt, lReturn As Integer
        Dim lRiskCnt As Object
        Dim lRiskFolderCnt As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_sQuoteRef = ""
            r_sQuoteRefPassword = ""

            '    Set oSBOLink = CreateObject("bSIRIUSLink.SIRIUSLink")
            '
            '    m_lReturn = oSBOLink.Initialise(sUsername:=tosafestring(m_sUsername), _
            ''                                    sPassword:=tosafestring(m_sPassword), _
            ''                                    iUserID:=tosafeinteger(m_iUserID), _
            ''                                    iSourceID:=tosafeinteger(m_iSourceID), _
            ''                                    iLanguageID:=tosafestring(m_iLanguageID), _
            ''                                    iCurrencyID:=tosafeinteger(m_iCurrencyID), _
            ''                                    iLogLevel:=tosafeinteger(m_iLogLevel), _
            ''                                    sCallingAppName:=tosafestring(ACApp), _
            ''                                    vDatabase:=directcast(m_oDatabase,dpmdao.database))
            ' Create bSiriusLink object
            oSBOLink = New bSIRIUSLink.SIRIUSLink
            lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)


            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NBStartBefore Failed - Failed to create bSiriusLink Object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBStartBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Set GIS codes in SBOLink

            oSBOLink.GISDataModelCode = v_sGisDataModelCode

            oSBOLink.GISBusinessTypeCode = v_sGisBusinessTypeCode

            dtCoverStartDate = DateTime.Now

            ' PWF 01/05/2002 - The expiry date should be one year yesterday so take the day off!
            dtExpiryDate = dtCoverStartDate.AddYears(1).AddDays(-1)

            'sInsFolderCode = Format$(dtCoverStartDate, "yyyymmddhhnnss")
            ' RFC110800 - Use a GUID for the Insurance Folder Code as it has to be unique.
            sInsFolderCode = bPMFunc.GetGUID()

            lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode,
                                                                       v_sSettingName:=
                                                                          GISSharedConstants.GISRegPolicySourceID,
                                                                       r_sSettingValue:=sReturn,
                                                                       v_sBusinessTypeCode:=v_sGisBusinessTypeCode)

            lSourceID = 1 'Empty
            Dim dbNumericTemp As Double
            If (lReturn = gPMConstants.PMEReturnCode.PMTrue) And Double.TryParse(sReturn, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                lSourceID = CInt(sReturn)
            End If

            'PWF 30/10/2001 - Check the Additional Data Array for an insured name
            lReturn = ParseArray(m_sUsername, r_vAdditionalDataArray, "INSURED_NAME", sInsuredName)

            ' Add the quote

            lReturn = oSBOLink.AddQuote(v_lPartyCnt:=v_lPartyCnt, v_dtStartDate:=dtCoverStartDate, v_dtEndDate:=dtExpiryDate, v_sInsuranceFolderCode:=sInsFolderCode, v_sVehicleMakeModel:="", v_sInsuranceRef:="NEW QUOTE", r_lInsuranceFileCnt:=r_lInsuranceFileCnt, v_cAnnualPremium:=0, v_lPolicySourceID:=lSourceID, v_sInsuredName:=sInsuredName, r_lInsuranceFolderCnt:=lInsuranceFolderCnt)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                  sMsg:="NBStartBefore Failed - bSiriusLink.AddQuote method failed.",
                                                  vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBStartBefore",
                                                  vErrNo:=Informations.Err().Number,
                                                  vErrDesc:=Informations.Err().Description)

                Return result
            End If

            ' PWF 06/11/2001 - Add risk details (SFU/MIA only)
            If v_sGisBusinessTypeCode = "MIA" Then

                lReturn = oSBOLink.SFUAddRisk(v_lInsuranceFileCnt:=r_lInsuranceFileCnt,
                                              r_lRiskFolderCnt:=lRiskFolderCnt, r_lRiskCnt:=lRiskCnt,
                                              v_lInsuranceFolderCnt:=lInsuranceFolderCnt)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                      sMsg:=
                                                         "NBStartBefore Failed - bSiriusLink.SFUAddRisk method failed.",
                                                      vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBStartBefore",
                                                      vErrNo:=Informations.Err().Number,
                                                      vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                ' Store the return values to the additional data array
                lReturn = AddToArray(m_sUsername, r_vAdditionalDataArray, "NP_RISK_FOLDER_CNT", lRiskFolderCnt)
                lReturn = AddToArray(m_sUsername, r_vAdditionalDataArray, "NP_RISK_CNT", lRiskCnt)
            End If


            oSBOLink.Dispose()
            oSBOLink = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                              sMsg:="NBStartBefore Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass,
                                              vMethod:="NBStartBefore", vErrNo:=Informations.Err().Number,
                                              vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ************************************************************************************************************
    '
    ' Function: CheckAlternateReference
    '
    ' Purpose: Checks to see if the passed alternate_reference is in use on the insurance_file table for uniqueness
    '
    ' Return: r_bUnique = True if unique (good), False otherwise (bad)
    '
    '
    ' Pass in the insurance_file_ref as the stored procedure checks that (against the ref) and also
    ' prefixes it with E- and checks it against the alternate_reference
    '
    ' ************************************************************************************************************
    Private Function CheckAlternateReference(ByVal v_vAlternateReference As String, ByRef r_bUnique As Boolean) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the parameters
        m_oDatabase.Parameters.Clear()

        ' Check for valid SQL
        v_vAlternateReference = v_vAlternateReference.Replace("'", "''")

        ' Add the new one
        m_lReturn = m_oDatabase.Parameters.Add(sName:="alternate_reference", vValue:=v_vAlternateReference, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter alternate_reference = " & v_vAlternateReference, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBStartBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Call the SQL
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckAltRefSQL, sSQLName:=ACCheckAltRefName, bStoredProcedure:=ACCheckAltRefStored, vResultArray:=vResultArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to call SQL : " & ACCheckAltRefSQL, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBStartBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Check our results
        If Informations.IsArray(vResultArray) Then
            ' Returns the number of records with that alternate_reference
            ' so... > 0 in use, 0 = not in use (unique)

            r_bUnique = Not (CStr(vResultArray(0, 0)) = "1")
        Else
            ' Didn't return anything, lets error
            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACCheckAltRefSQL & " didn't return any data.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBStartBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Clear up
        vResultArray = Nothing

        Return result

    End Function

    'UPGRADE_NOTE: (7001) The following declaration (GetAgentCntFromBrokerID) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx	
    'Private Function GetAgentCntFromBrokerID(ByVal v_vBrokerABIID As Object, ByRef r_lAgentCnt As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Dim oInsuranceFile As Object
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Create an instance of bSIRInsuranceFile.Services
    'm_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oInsuranceFile, v_sClassName:="bSIRInsuranceFile.Services", v_sCallingAppName:=tosafestring(ACApp), v_sUsername:=tosafestring(m_sUsername), v_sPassword:=tosafestring(m_sPassword), v_iUserID:=tosafeinteger(m_iUserID), v_iSourceID:=tosafeinteger(m_iSourceID), v_iLanguageID:=tosafestring(m_iLanguageID), v_iCurrencyID:=tosafeinteger(m_iCurrencyID), v_iLogLevel:=tosafeinteger(m_iLogLevel), v_oDatabase:=directcast(m_oDatabase,dpmdao.database))
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    ' Set the property to load all the details
    '
    '
    'oInsuranceFile.BrokerAbiId = v_vBrokerABIID
    '
    ' Get the party_cnt
    '
    'r_lAgentCnt = CInt(oInsuranceFile.LeadAgentCnt)
    '
    ' Clear up
    '
    'm_lReturn = oInsuranceFile.Terminate()
    'oInsuranceFile = Nothing
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
    'GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAgentCntFromBrokerID Failed", vApp:=tosafestring(ACApp), vClass:=ACClass, vMethod:="GetAgentCntFromBrokerID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    Public Function AddQuote(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_dtEffectiveDate As Date, ByVal v_dtExpirationDate As Date, ByVal v_sInsuredName As String, ByVal v_lPartyCnt As Integer, ByRef r_lAgentCnt As Integer, ByRef r_lInsuranceFolderCnt As Integer, ByRef r_lInsuranceFileCnt As Integer, Optional ByVal v_sInsuranceFolderDescription As String = "", Optional ByRef r_sInsuranceFileRef As String = "", Optional ByRef r_lRiskCodeId As Integer = 0, Optional ByVal v_lSourceId As Integer = 0, Optional ByRef r_lInsurerCnt As Object = Nothing, Optional ByVal v_lscreenid As Integer = 0, Optional ByVal v_vAlternateReference As Object = Nothing, Optional ByRef r_lRiskCnt As Integer = 0, Optional ByRef r_lRiskGroupId As Integer = 0, Optional ByRef r_lGisSchemeId As Integer = 0, Optional ByRef r_vAdditionalDataArray(,) As Object = Nothing, Optional ByVal v_lCurrencyID As Integer = 0, Optional ByVal v_lAnalysisCodeId As Integer = 0, Optional ByVal v_sPolicyStatusCode As String = "", Optional ByVal v_lPolicyVersion As Integer = 0, Optional ByVal v_sRenewalFrequency As String = "", Optional ByVal v_sInsuranceFileStructure As String = "", Optional ByVal v_sBusinessType As String = "", Optional ByVal v_sPaymentMethod As String = "", Optional ByVal v_blConsLeadAgntComm As Boolean = False, Optional ByVal v_blConsSubAgntComm As Boolean = False, Optional ByVal v_lLapsedReasonId As Integer = 0, Optional ByVal v_dtLapsedDate As Date = GISSharedConstants.GISLowDate, Optional ByVal v_sLapsedReasonDescription As String = "", Optional ByVal v_dtInceptionDate As Date = GISSharedConstants.GISLowDate, Optional ByVal v_dtInceptionDateTPI As Date = GISSharedConstants.GISLowDate, Optional ByVal v_dtRenewalDate As Date = GISSharedConstants.GISLowDate, Optional ByVal v_sAlternateReference As String = "", Optional ByVal v_sOldPolicyNumber As String = "", Optional ByVal v_sAccountExecutiveShortname As String = "", Optional ByVal v_sAccountHandlerShortname As String = "", Optional ByVal v_sInsuranceFileTypeCode As String = "") As Integer
        Dim result As Integer = 0
        Dim sReturn As String = ""
        Dim lSourceID, lReturn As Integer
        Dim oSBOLink As bSIRIUSLink.SIRIUSLink
        Dim sInsFolderCode As String = ""
        Dim lProductID As Integer
        Dim oPolicyNumber As bSIRPolicyNumMaint.Business

        'TJB 30012003

        Dim blCalledFromSTS As Boolean

        ' CTAF 20040302
        Dim bUnique As Boolean
        Dim cAnnualPremium, cNetPremium As Decimal

        Dim cTaxAmount As Decimal
        Dim sExternalSchemeNo As String = ""

        Dim blConsolidatedLeadAgentCommission, blConsolidatedSubAgentCommission As Boolean
        Dim oSource As bPMSource.Business
        Dim iCountryId As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            blCalledFromSTS = False

            lProductID = -1
            lSourceID = -1

            cAnnualPremium = 0
            cNetPremium = 0
            cTaxAmount = 0
            sExternalSchemeNo = ""

            For i As Integer = 0 To r_vAdditionalDataArray.GetUpperBound(1)
                Select Case r_vAdditionalDataArray(0, i)
                    Case CNCalledFromSTS

                        blCalledFromSTS = CBool(r_vAdditionalDataArray(1, i))
                    Case CNEdiSolution

                        m_bUnderwritingBranchEnabled = CBool(r_vAdditionalDataArray(1, i))
                    Case ACProductID

                        lProductID = CInt(Val(CStr(r_vAdditionalDataArray(1, i))))
                    Case PMNavKeyConst.PMKeyNameThisPremium

                        cAnnualPremium = Val(CStr(r_vAdditionalDataArray(1, i)))
                    Case PMNavKeyConst.PMKeyNameNetPremium

                        cNetPremium = Val(CStr(r_vAdditionalDataArray(1, i)))
                    Case PMNavKeyConst.PMKeyNameTaxAmount

                        cTaxAmount = Val(CStr(r_vAdditionalDataArray(1, i)))
                    Case ACConsolidatedLeadAgentCommission

                        blConsolidatedLeadAgentCommission = CBool(r_vAdditionalDataArray(1, i))
                    Case ACConsolidatedSubAgentCommission

                        blConsolidatedSubAgentCommission = CBool(r_vAdditionalDataArray(1, i))
                End Select
            Next i

            ' If this call hasn't arisen from the STS layer then skip out.
            If Not blCalledFromSTS Then
                Return result
            End If


            ' If supplied use the one passed in
            If Not False Then
                lSourceID = v_lSourceId
            Else

                lSourceID = m_iSourceID 'Default to user's source_id

                lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode,
                                                                           v_sSettingName:=
                                                                              GISSharedConstants.GISRegPolicySourceID,
                                                                           r_sSettingValue:=sReturn,
                                                                           v_sBusinessTypeCode:=v_sGisBusinessTypeCode)

                Dim dbNumericTemp As Double
                If (lReturn = gPMConstants.PMEReturnCode.PMTrue) And Double.TryParse(sReturn, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    lSourceID = CInt(sReturn)
                End If

            End If

            ' CTAF 20040301 - Start
            If r_sInsuranceFileRef = "" Then

                If (lSourceID > -1) And (lProductID > -1) Then

                    ' Create bSIRPolicyNumMaint object
                    oPolicyNumber = New bSIRPolicyNumMaint.Business
                    lReturn = oPolicyNumber.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = lReturn
                        ' Log Error Message
                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddQuote Failed - Failed to create bSIRPolicyNumMaint object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If


                    lReturn = oPolicyNumber.GeneratePolicyNumber(v_lBusinessType:=1, v_iBranch:=lSourceID, v_lProductId:=lProductID, v_lAgent:=r_lAgentCnt, r_sGeneratedPolicyNumber:=r_sInsuranceFileRef, v_lPartyCnt:=v_lPartyCnt)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        ' Log the message, but it isn't fatal

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="AddQuote Failed - Failed to Generate Policy Number for Product Id " & lProductID, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                        ' Make it Unique
                        r_sInsuranceFileRef = bPMFunc.GetGUID()

                    End If


                    oPolicyNumber.Dispose()

                    oPolicyNumber = Nothing

                Else

                    ' Make it Unique
                    r_sInsuranceFileRef = bPMFunc.GetGUID()

                End If

            Else

                ' Check if it already exists via the alternate_reference

                m_lReturn = CheckAlternateReference(v_vAlternateReference:=CStr(v_vAlternateReference), r_bUnique:=bUnique)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Check the return value here, we may want to return different errors...
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not bUnique Then
                    ' Return the error that it's not unique
                    result = gPMConstants.PMEReturnCode.PMRecordInUse
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Attempted to add quote with existing alternate_reference of " & r_sInsuranceFileRef, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

            End If
            ' CTAF 20040301 - End

            ' Create bPmSource object
            oSource = New bPMSource.Business
            lReturn = oSource.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddQuote Failed - Failed to create bPmSource object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Else

                lReturn = oSource.GetBranchBaseCountry(v_lSourceID:=lSourceID, r_iCountryID:=iCountryId)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "oSource.GetBranchBaseCountry Failed", ACApp, ACClass, "AddQuote")
                End If


                oSource.Dispose()
            End If
            oSource = Nothing

            sInsFolderCode = r_sInsuranceFileRef

            ' Create bSiriusLink object
            oSBOLink = New bSIRIUSLink.SIRIUSLink
            lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddQuote Failed - Failed to create bSiriusLink object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Set GIS codes in SBOLink

            oSBOLink.GISDataModelCode = v_sGisDataModelCode

            oSBOLink.GISBusinessTypeCode = v_sGisBusinessTypeCode

            ' Add the quote

            lReturn = oSBOLink.AddQuote(v_lPartyCnt:=v_lPartyCnt, v_dtStartDate:=v_dtEffectiveDate, v_dtEndDate:=v_dtExpirationDate, v_sInsuranceFolderCode:=sInsFolderCode, v_sVehicleMakeModel:="", v_sInsuranceRef:=r_sInsuranceFileRef, v_lLeadAgentCnt:=r_lAgentCnt, v_cAnnualPremium:=cAnnualPremium, v_lPolicySourceID:=lSourceID, v_sInsuredName:=v_sInsuredName, r_lInsuranceFileCnt:=r_lInsuranceFileCnt, r_lInsuranceFolderCnt:=r_lInsuranceFolderCnt, v_sInsuranceFolderDescription:=v_sInsuranceFolderDescription, v_sInsuranceFileStructure:=v_sInsuranceFileStructure, v_cNetPremium:=cNetPremium, v_sRenewalFrequency:=v_sRenewalFrequency, v_sBusinessType:=v_sBusinessType, v_sPaymentMethod:=v_sPaymentMethod, v_lProductID:=lProductID, v_lCurrencyID:=v_lCurrencyID, v_lAnalysisCodeId:=v_lAnalysisCodeId, v_sPolicyStatusCode:=v_sPolicyStatusCode, v_lPolicyVersion:=v_lPolicyVersion, v_blConsLeadAgntComm:=blConsolidatedLeadAgentCommission, v_blConsSubAgntComm:=blConsolidatedSubAgentCommission, v_lLapsedReasonId:=v_lLapsedReasonId, v_dtLapsedDate:=v_dtLapsedDate, v_sLapsedReasonDescription:=v_sLapsedReasonDescription, v_dtInceptionDate:=v_dtInceptionDate, v_dtInceptionDateTPI:=v_dtInceptionDateTPI, v_dtRenewalDate:=v_dtRenewalDate, v_sAlternateReference:=v_sAlternateReference, v_sOldPolicyNumber:=v_sOldPolicyNumber, v_sAccountExecutiveShortname:=v_sAccountExecutiveShortname, v_sAccountHandlerShortname:=v_sAccountHandlerShortname, v_sInsuranceFileTypeCode:=v_sInsuranceFileTypeCode, v_bCCTermsAgreed:=True, v_lTypeOfSaleId:=1)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddQuote Failed - bSiriusLink.AddQuote method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If


            oSBOLink.Dispose()
            oSBOLink = Nothing


            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddQuote Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddQuote", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetPolicyLinkID
    '
    ' Description: Gets the gis_policy_link_id for an insurance_file_cnt
    '              Works for SBO 1.8.6 where there is a 1-1 relationship
    '
    ' History: 28/07/2003 CTAF - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetPolicyLinkID) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx	
    'Private Function GetPolicyLinkID(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lGISPolicyLinkID As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Dim vResultArray(,) As Object
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Clear any parameters
    'm_oDatabase.Parameters.Clear()
    '
    ' Add the parameters
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="gis_policy_link_id", vValue:=CStr(DBNull.Value), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    ' Log Error Message
    'bPMFunc.LogMessage(sUserName:=tosafestring(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter : gis_policy_link_id", vApp:=tosafestring(ACApp), vClass:=ACClass, vMethod:="GetPolicyLinkID", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
    'Return result
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    ' Log Error Message
    'bPMFunc.LogMessage(sUserName:=tosafestring(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter : insurance_file_cnt . value = " & v_lInsuranceFileCnt, vApp:=tosafestring(ACApp), vClass:=ACClass, vMethod:="GetPolicyLinkID", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
    'Return result
    'End If
    '
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="quote_ref", vValue:=CStr(DBNull.Value), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    ' Log Error Message
    'bPMFunc.LogMessage(sUserName:=tosafestring(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter : quote_ref", vApp:=tosafestring(ACApp), vClass:=ACClass, vMethod:="GetPolicyLinkID", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
    'Return result
    'End If
    '
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="risk_id", vValue:=CStr(DBNull.Value), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    ' Log Error Message
    'bPMFunc.LogMessage(sUserName:=tosafestring(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter : risk_id", vApp:=tosafestring(ACApp), vClass:=ACClass, vMethod:="GetPolicyLinkID", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
    'Return result
    'End If
    '
    ' Call the SQL
    'm_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyLinkIDSQL, sSQLName:=ACGetPolicyLinkIDName, bStoredProcedure:=ACGetPolicyLinkIDStored, vResultArray:=vResultArray)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    ' Log Error Message
    'bPMFunc.LogMessage(sUserName:=tosafestring(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter : risk_id", vApp:=tosafestring(ACApp), vClass:=ACClass, vMethod:="GetPolicyLinkID", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
    'Return result
    'End If
    '
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
    'bPMFunc.LogMessage(sUserName:=tosafestring(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyLinkID Failed", vApp:=tosafestring(ACApp), vClass:=ACClass, vMethod:="GetPolicyLinkID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    '

    '
    'Return result
    'End Try
    'End Function


    ' ***************************************************************** '
    ' Name: NBStartAfter
    '
    ' Description: This method is called by the GIS when a New Business
    '              Quote is started.
    '              Called AFTER GIS has done its stuff.
    ' ***************************************************************** '
    Public Function NBStartAfter(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lPartyCnt As Integer, ByRef r_lInsuranceFileCnt As Integer, ByRef r_sQuoteRef As String, ByRef r_sQuoteRefPassword As String, ByRef r_oDataset As cGISDataSetControl.Application, ByRef r_vAdditionalDataArray As Array) As Integer
        Dim result As Integer = 0
        Dim vKeyArray As Object
        Dim sBinderKey, sKey, sIncidentKey, sDriverKey, sVehicleKey, sCoverKey, s As String
        Dim oSBOLink As Object
        Dim lRiskCnt, lRiskFolderCnt As Integer
        Dim sQuery As String = ""
        Dim lInsurance_FileCnt As Object
        Dim sQuoteRef As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Inside NBStartAfter...", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBStartAfter")

            ' Set Our Class Ref to the Dataset
            m_oDataSet = r_oDataset


            m_lReturn = m_oDataSet.GetAllOIKey(v_sObjectName:=ACDataModelCodePrefix & "POLICY_BINDER", r_vOIKeyArray:=vKeyArray)


            sBinderKey = CStr(vKeyArray(0))

            '
            ' NP_Premium_Finance
            '
            m_lReturn = m_oDataSet.NewObjectInstance(v_sObjectName:=ACDataModelCodePrefix & "NP_PREMIUM_FINANCE", r_sOIKey:=sKey, v_sParentOIKey:=sBinderKey)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '
            ' PROPOSER_POLICYHOLDER
            '
            m_lReturn = m_oDataSet.NewObjectInstance(v_sObjectName:=ACDataModelCodePrefix & "PROPOSER_POLICYHOLDER", r_sOIKey:=sKey, v_sParentOIKey:=sBinderKey)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '
            ' POLICY
            '
            m_lReturn = m_oDataSet.NewObjectInstance(v_sObjectName:=ACDataModelCodePrefix & "POLICY", r_sOIKey:=sKey, v_sParentOIKey:=sBinderKey)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '
            ' PAYMENT_BANK
            '
            m_lReturn = m_oDataSet.NewObjectInstance(v_sObjectName:=ACDataModelCodePrefix & "PAYMENT_BANK", r_sOIKey:=sKey, v_sParentOIKey:=sBinderKey)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            '=======================================================================================
            ' Driver and all children
            '=======================================================================================
            For i As Integer = 1 To 3
                m_lReturn = m_oDataSet.NewObjectInstance(v_sObjectName:=ACDataModelCodePrefix & "DRIVER", r_sOIKey:=sDriverKey, v_sParentOIKey:=sBinderKey)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                '
                ' create 2 x occupation
                '
                For j As Integer = 1 To 2
                    m_lReturn = m_oDataSet.NewObjectInstance(v_sObjectName:=ACDataModelCodePrefix & "OCCUPATION", r_sOIKey:=sKey, v_sParentOIKey:=sDriverKey)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Next j

                '
                ' create 6 x INCIDENTs
                '
                For j As Integer = 1 To 6
                    m_lReturn = m_oDataSet.NewObjectInstance(v_sObjectName:=ACDataModelCodePrefix & "INCIDENT", r_sOIKey:=sIncidentKey, v_sParentOIKey:=sDriverKey)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' we want a claim for the first 3 incidents and conviction for the last 3
                    If j >= 1 And j <= 3 Then
                        s = ACDataModelCodePrefix & "CLAIM"
                    End If

                    If j >= 4 And j <= 6 Then
                        s = ACDataModelCodePrefix & "CONVICTION"
                    End If

                    m_lReturn = m_oDataSet.NewObjectInstance(v_sObjectName:=s, r_sOIKey:=sKey, v_sParentOIKey:=sIncidentKey)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Next j ' incident

                'Create a Legacy_Driver QMM object instance - CJB 110501
                m_lReturn = m_oDataSet.NewObjectInstance(v_sObjectName:="Legacy_Driver", r_sOIKey:=sKey, v_sParentOIKey:=sDriverKey)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Next i ' driver


            '=======================================================================================
            ' VEHICLE and all children
            '=======================================================================================
            m_lReturn = m_oDataSet.NewObjectInstance(v_sObjectName:=ACDataModelCodePrefix & "VEHICLE", r_sOIKey:=sVehicleKey, v_sParentOIKey:=sBinderKey)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '
            ' NCD
            '
            m_lReturn = m_oDataSet.NewObjectInstance(v_sObjectName:=ACDataModelCodePrefix & "NCD", r_sOIKey:=sKey, v_sParentOIKey:=sVehicleKey)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '
            ' SECURITY
            '
            m_lReturn = m_oDataSet.NewObjectInstance(v_sObjectName:=ACDataModelCodePrefix & "SECURITY", r_sOIKey:=sKey, v_sParentOIKey:=sVehicleKey)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            '=======================================================================================
            ' COVER and all children
            '=======================================================================================
            m_lReturn = m_oDataSet.NewObjectInstance(v_sObjectName:=ACDataModelCodePrefix & "COVER", r_sOIKey:=sCoverKey, v_sParentOIKey:=sBinderKey)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '
            ' Create 6 x SELECTED_COVER_VAR
            '
            For i As Integer = 1 To 6
                m_lReturn = m_oDataSet.NewObjectInstance(v_sObjectName:=ACDataModelCodePrefix & "SELECTED_COVER_VAR", r_sOIKey:=sKey, v_sParentOIKey:=sCoverKey)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Next i

            '=======================================================================================
            'CJB 260201 Start
            'Create QMM related objects here (as well as USES + LEGACY_DRIVER creation earlier)
            '=======================================================================================
            m_lReturn = m_oDataSet.NewObjectInstance(v_sObjectName:="Broker_Details", r_sOIKey:=sKey, v_sParentOIKey:=sBinderKey)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDataSet.NewObjectInstance(v_sObjectName:="GemQuoteConfiguration", r_sOIKey:=sKey, v_sParentOIKey:=sBinderKey)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDataSet.NewObjectInstance(v_sObjectName:="Control_Block", r_sOIKey:=sKey, v_sParentOIKey:=sBinderKey)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDataSet.NewObjectInstance(v_sObjectName:="Legacy_Policy", r_sOIKey:=sKey, v_sParentOIKey:=sBinderKey)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '=======================================================================================
            'CJB 260201 End
            '=======================================================================================

            '=======================================================================================
            'RJG 260701 Begin new object to enable working August Rates for Zurich
            '=======================================================================================
            m_lReturn = m_oDataSet.NewObjectInstance(v_sObjectName:="GenInsurer1", r_sOIKey:=sKey, v_sParentOIKey:=sBinderKey)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '=======================================================================================
            'RJG 260701 End
            '=======================================================================================

            '=======================================================================================
            'RJG 120901 The Saved_Quote object stores MTA information for the COBOL rather than
            ' Calculated_Result which is what is currently used.
            '=======================================================================================
            m_lReturn = m_oDataSet.NewObjectInstance(v_sObjectName:="Saved_Quote", r_sOIKey:=sKey, v_sParentOIKey:=sBinderKey)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '=======================================================================================
            ' RAG 02/07/2001
            ' Need to call SiriusLink to update the quote ref in SBO, otherwise
            ' it gets left as "NEW QUOTE".
            '=======================================================================================
            ' Create a SBO Link object
            oSBOLink = New bSIRIUSLink.SIRIUSLink
            Dim oDatabase As Object = m_oDatabase
            m_lReturn = oSBOLink.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeString(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "oSBOLink.Initialise Failed", ACApp, ACClass, "NBStartAfter")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_oDatabase = oDatabase
            ' Set GIS codes in SBOLink

            oSBOLink.GISDataModelCode = v_sGisDataModelCode

            oSBOLink.GISBusinessTypeCode = v_sGisBusinessTypeCode

            ' Update cover details

            m_lReturn = oSBOLink.UpdateCoverDetails(v_lInsuranceFileCnt:=lInsurance_FileCnt, v_sInsuranceRef:=sQuoteRef)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "oSBOLink.UpdateCoverDetails Failed", ACApp, ACClass, "NBStartAfter")
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                r_lInsuranceFileCnt = lInsurance_FileCnt
                r_sQuoteRef = sQuoteRef
            End If
            '=======================================================================================
            ' RAG End
            '=======================================================================================


            ' PWF 06/11/2001 - Add risk details (SFU/MIA only)
            If v_sGisBusinessTypeCode = "MIA" Then
                ' Get values from the additional data array
                m_lReturn = ParseArray(m_sUsername, r_vAdditionalDataArray, "NP_RISK_FOLDER_CNT", lRiskFolderCnt)
                m_lReturn = ParseArray(m_sUsername, r_vAdditionalDataArray, "NP_RISK_CNT", lRiskCnt)

                ' Store the return values to the dataset
                m_oDataSet.Risk.Item(ACDataModelCodePrefix & "POLICY").Item("NP_RISK_FOLDER_CNT").Value = CStr(lRiskFolderCnt)
                m_oDataSet.Risk.Item(ACDataModelCodePrefix & "POLICY").Item("NP_RISK_CNT").Value = CStr(lRiskCnt)

                ' Update the GIS_Policy_Link record

                ' Use dynamic sql for development speed (yes, it is called risk_id in the table, jeez)
                sQuery = "UPDATE GIS_Policy_Link " &
                         "SET risk_id = " & CStr(lRiskCnt) & " " &
                         "WHERE quote_ref = '" & r_sQuoteRef & "'"


                ' Send the query to the log file
                GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogDebug1, sMsg:="Updating GIS_Policy_Link risk_id" & Strings.ChrW(13) & Strings.ChrW(10) & "   " & sQuery, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBStartAfter")

                m_lReturn = m_oDatabase.SQLAction(sSQL:=sQuery, sSQLName:="UpdateGISPolicyLink", bStoredProcedure:=False)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "GIS_Policy_Link Update Failed", ACApp, ACClass, "NBStartBefore")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Release the SBO link object

            oSBOLink.Dispose()
            oSBOLink = Nothing

            ' Release our Ref to the Dataset
            m_oDataSet = Nothing

            Return result

        Catch excep As System.Exception


            ' PWF 06/11/2002 - Moved cleanup code AFTER log to preserve Error details.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NBStartAfter Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBStartAfter", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            ' Release the SBO link object

            ' Release our Ref to the Dataset
            m_oDataSet = Nothing

            Return result

        End Try
    End Function


    '' ***************************************************************** '
    '' Name: NBRestartQuote
    ''
    '' Description: This method is called by the GIS when a New Business
    ''              Quote is re-started.
    ''              i.e. When the Load From DB method is called
    ''
    '' ***************************************************************** '
    'Public Function NBRestartQuote() As Long
    '
    '    On Error GoTo Err_NBRestartQuote
    '
    '    NBRestartQuote = PMTrue
    '
    '    ' Not sure what you would want to do here
    '
    '    Exit Function
    '
    'Err_NBRestartQuote:
    '
    '    NBRestartQuote = PMError
    '
    '    ' Log Error Message
    '    LogMessageFile _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="NBRestartQuote Failed", _
    ''        vApp:=tosafestring(ACApp), _
    ''        vClass:=ACClass, _
    ''        vMethod:="NBRestartQuote", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function
    '
    '
    '' ***************************************************************** '
    '' Name: NBParkQuote
    ''
    '' Description: This method is called by the GIS when a New Business
    ''              Quote is Parked, or saved for later.
    ''
    '' ***************************************************************** '
    'Public Function NBParkQuote() As Long
    '
    '    On Error GoTo Err_NBParkQuote
    '
    '    NBParkQuote = PMTrue
    '
    '    ' Not Sure what would need to be done here yet
    '
    '    Exit Function
    '
    'Err_NBParkQuote:
    '
    '    NBParkQuote = PMError
    '
    '    ' Log Error Message
    '    LogMessageFile _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="NBParkQuote Failed", _
    ''        vApp:=tosafestring(ACApp), _
    ''        vClass:=ACClass, _
    ''        vMethod:="NBParkQuote", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function
    Public Function AddRisk(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_sQuoteRef As String, ByVal v_sQuoteRefPassword As String, ByVal v_lRiskTypeId As Integer, ByVal v_lRiskScreenId As Integer, ByVal v_sRiskDescription As String, ByVal v_lProductId As Integer, ByRef r_lRiskFolderCnt As Integer, ByRef r_lRiskCnt As Integer, ByRef r_oDataset As cGISDataSetControl.Application, ByRef r_vAdditionalDataArray(,) As Object) As Integer
        Dim result As Integer = 0
        Dim lReturn, lRiskCnt As Integer
        'Dim lRiskFolderCnt  As Long
        Dim oSBOLink As bSIRIUSLink.SIRIUSLink
        Dim sQuery As String = ""
        Dim blCalledFromSTS As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            blCalledFromSTS = False

            For i As Integer = 0 To r_vAdditionalDataArray.GetUpperBound(1)
                Select Case r_vAdditionalDataArray(0, i)
                    Case CNCalledFromSTS

                        blCalledFromSTS = CBool(r_vAdditionalDataArray(1, i))
                        Exit For
                End Select
            Next i

            ' If this call hasn't arisen from the STS layer then skip out.
            If Not blCalledFromSTS Then
                Return result
            End If

            ' Set Our Class Ref to the Dataset
            m_oDataSet = r_oDataset

            ' Create bSiriusLink object
            oSBOLink = New bSIRIUSLink.SIRIUSLink
            lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddRisk Failed - Failed to create bSiriusLink object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddRisk", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Set GIS codes in SBOLink

            oSBOLink.GISDataModelCode = v_sGisDataModelCode

            oSBOLink.GISBusinessTypeCode = v_sGisBusinessTypeCode

            'Add Risk

            lReturn = oSBOLink.AddRisk(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lRiskTypeId:=v_lRiskTypeId, v_lRiskScreenId:=v_lRiskScreenId, v_sRiskDescription:=v_sRiskDescription, v_lProductID:=v_lProductId, r_lRiskFolderCnt:=r_lRiskFolderCnt, r_lRiskCnt:=lRiskCnt, r_oDataset:=r_oDataset)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddRisk Failed - bSiriusLink.AddRisk method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddRisk", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'r_lRiskFolderCnt = lRiskFolderCnt
            r_lRiskCnt = lRiskCnt

            ' RDT START *******************************************************************
            'RDT 10/02/2003 - Temporary - this needs to be removed when we start managing the Select risks behaviour properly.

            Dim vSelectedRisk(2, 0) As Object

            vSelectedRisk(0, 0) = lRiskCnt

            vSelectedRisk(1, 0) = True

            lReturn = UpdateRiskSelectionStatus(v_vSelectionArray:=vSelectedRisk)

            ' RDT END *********************************************************************


            ' Release the SBO link object

            oSBOLink.Dispose()
            oSBOLink = Nothing

            ' Release our Ref to the Dataset
            m_oDataSet = Nothing

            Return result

        Catch excep As System.Exception


            ' PWF 06/11/2002 - Moved cleanup code AFTER log to preserve Error details.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddRisk Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddRisk", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            ' Release the SBO link object

            ' Release our Ref to the Dataset
            m_oDataSet = Nothing

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateRiskSelectionStatus
    '
    ' Description: Accepts an array of risk cnts/selection status'
    '              and updates the corresponding risk records with the status'.
    '
    ' Created: RDT301002
    '
    ' ***************************************************************** '
    Public Function UpdateRiskSelectionStatus(ByVal v_vSelectionArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim oSIRListRisks As bSIRListRisks.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create bSiriusLink object
            oSIRListRisks = New bSIRListRisks.Business
            lReturn = oSIRListRisks.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRiskSelectionStatus Failed - Failed to create bSIRListRisks.Business object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="UpdateRiskSelectionStatus", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If


            lReturn = oSIRListRisks.UpdateRiskSelectionStatus(v_vSelectionArray:=v_vSelectionArray)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRiskSelectionStatus Failed - bSIRListRisks.UpdateRiskSelectionStatus method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="UpdateRiskSelectionStatus", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Release the SBO link object

            oSIRListRisks.Dispose()
            oSIRListRisks = Nothing

            Return result

        Catch excep As System.Exception


            ' Moved cleanup code AFTER log to preserve Error details.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRiskSelectionStatus Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="UpdateRiskSelectionStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            ' Release the oSIRListRisks object

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDataModelDef
    '
    ' Description: Either Load the Stored Data Set Definition from file
    '              OR get the details from the database.
    '
    ' ***************************************************************** '
    Private Function GetDataModelDef(ByVal v_sGisDataModelCode As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sDataSetDefFile, sDataSetFile As String

        ' RDC 30072001 Dataset Def creation timestamp
        Dim sTimestamp As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the Path/FileNames for stored EMPTY Data Set Files
        ' of this Data Model Type
        lReturn = CType(GISSharedConstants.GetDataSetFileNames(v_sDataModelCode:=v_sGisDataModelCode, r_sDataSetDefFile:=sDataSetDefFile, r_sDataSetFile:=sDataSetFile), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Create a New Data Set
        m_oDataSet = New cGISDataSetControl.Application()

        ' Try to load from the Empty XML files
        lReturn = m_oDataSet.LoadFromXMLFile(v_sDataSetDefFile:=sDataSetDefFile, v_sDataSetFile:=sDataSetFile)


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: NBQuoteBefore
    '
    ' Description: This method is called by the GIS when a New Business
    '              is Quoted.
    '              BEFORE the GIS has done its stuff.
    ' ***************************************************************** '
    Public Function NBQuoteBefore(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lQuoteType As Integer, ByRef r_oDataset As cGISDataSetControl.Application, ByRef r_vAdditionalDataArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sIsCustEmailRequired As String = ""
        Dim lInsuranceFileCnt, lInsuranceFolderCnt, lRiskCnt As Integer
        Dim sTransactionType As String = ""
        Dim oPerilAllocation As bSirPerilAllocation.Business
        Dim lReturn, lTransactionType, lGISScreenId, lQuoteType As Integer
        Dim blCalledFromSTS As Boolean
        Dim sOIKey As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Our Class Ref to the Dataset
            m_oDataSet = r_oDataset

            blCalledFromSTS = False

            If r_vAdditionalDataArray IsNot Nothing Then
                For i As Integer = 0 To r_vAdditionalDataArray.GetUpperBound(1)
                    Select Case r_vAdditionalDataArray(0, i)
                        Case CNCalledFromSTS

                            blCalledFromSTS = CBool(r_vAdditionalDataArray(1, i))
                            Exit For
                    End Select
                Next i
            Else
                'Assume that the call did not come from the STS
                blCalledFromSTS = False
            End If

            ' If this call hasn't arisen from the STS layer then skip out.
            If Not blCalledFromSTS Then
                Return result
            End If

            PBQuoteTypeEncode.decodeTransactionScreenAndType(v_lQuoteType, lTransactionType, lGISScreenId, lQuoteType)

            If lQuoteType = PBQuoteTypeEncode.PBCQemQuoteTypeQuote Then

                'We are going to assume that we are always passing values in the AdditionalDataArray.
                'If not we are going to exit the function with and error
                If Informations.IsArray(r_vAdditionalDataArray) Then

                    For i As Integer = 0 To r_vAdditionalDataArray.GetUpperBound(1)
                        Select Case r_vAdditionalDataArray(0, i)
                            Case CNInsuranceFolderCnt

                                lInsuranceFolderCnt = CInt(r_vAdditionalDataArray(1, i))
                            Case CNInsuranceFileCnt

                                lInsuranceFileCnt = CInt(r_vAdditionalDataArray(1, i))
                            Case CNCurrentRiskCnt

                                lRiskCnt = CInt(r_vAdditionalDataArray(1, i))
                            Case CNTransactionType

                                sTransactionType = CStr(r_vAdditionalDataArray(1, i))
                        End Select
                    Next i
                Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="There is no information is the Additional Data Array. Must be populated.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If


                ' Create bSIRPerilAllocation object
                oPerilAllocation = New bSirPerilAllocation.Business
                lReturn = oPerilAllocation.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NBQuoteAfter Failed to Initialise bSirPerilAllocation.Business object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                'Set the Business Object Properties
                ' Changed to pass InsuranceFolderCnt instead of InsuranceFileCnt to
                ' the oPerilAllocation.InsuranceFolderCnt. PW181105

                oPerilAllocation.InsuranceFolderCnt = lInsuranceFolderCnt


                oPerilAllocation.InsuranceFileCnt = lInsuranceFileCnt ' CTAF 20030129 - Changed from folder_cnt


                oPerilAllocation.RiskID = lRiskCnt

                If sTransactionType = "" Then
                    sTransactionType = v_sGisBusinessTypeCode
                End If


                oPerilAllocation.TransactionType = sTransactionType

                'This also clears the output table

                lReturn = oPerilAllocation.GetDataModel(sGISDataModel:=v_sGisDataModelCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSirPerilAllocation.Business.GetDataModel method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

            End If

            ' Release our Ref to the Dataset
            m_oDataSet = Nothing

            Return result

        Catch excep As System.Exception
            m_oDataSet = Nothing
            result = gPMConstants.PMEReturnCode.PMError
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_sGisDataModelCode", v_sGisDataModelCode)
            oDict.Add("v_sGisBusinessTypeCode", v_sGisBusinessTypeCode)
            gPMFunctions.LogMessageToFile(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "NBQuoteBefore Failed", ACApp, ACClass, "NBQuoteBefore", excep, oDicParms:=oDict)
            Return result
        End Try
    End Function



    ''' <summary>
    ''' This method is called by the GIS when a New Business is Quoted.
    ''' AFTER the GIS has done its stuff.
    ''' </summary>
    ''' <param name="v_sGisDataModelCode"></param>
    ''' <param name="v_sGisBusinessTypeCode"></param>
    ''' <param name="v_lQuoteType"></param>
    ''' <param name="r_oDataset"></param>
    ''' <param name="r_vAdditionalDataArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function NBQuoteAfter(ByVal v_sGisDataModelCode As Object, ByVal v_sGisBusinessTypeCode As String, ByVal v_lQuoteType As Integer, ByRef r_oDataset As cGISDataSetControl.Application, ByRef r_vAdditionalDataArray(,) As Object) As Integer
        Dim nResult As Integer
        Dim oSIRRITax As bSIRRITax.Business
        Dim oSIRPartyFee As bSIRPartyFee.UBusiness
        Dim oSIRListRisks As bSIRListRisks.Business
        Dim nInsuranceFileCnt As Integer = 0
        Dim nInsuranceFolderCnt As Integer = 0
        Dim nRiskCnt As Integer = 0
        Dim sTransactionType As String = ""
        Dim oBusiness As Object
        Dim nReturn As Integer = 0
        Dim nQuoteType As Integer = 0
        Dim nGISScreenId As Integer = 0
        Dim nTransactionType As Integer = 0
        Dim sDataModelCode As String = ""
        Dim bApplyReinsurance As Object = False
        Dim bDisplayScreen As Object
        Dim oCommissionArray As Object
        Dim sXMLDataSet, sXMLDataSetDef As Object
        Dim blCalledFromSTS As Boolean
        Dim bIsRIValid As Boolean
        Dim nRIValid As Object = 0
        Dim nRIBand As Object = 0
        Dim sbRIComponentname As String = ""
        Dim bSkipSaveToDB As Boolean
        Dim bQuestionUnanswered As Boolean = False


        Try

            nResult = PMEReturnCode.PMTrue

            bIsRIValid = True

            blCalledFromSTS = False

            If r_vAdditionalDataArray IsNot Nothing AndAlso r_vAdditionalDataArray.Length > 0 Then
                For i As Integer = 0 To r_vAdditionalDataArray.GetUpperBound(1)
                    Select Case r_vAdditionalDataArray(0, i)
                        Case CNCalledFromSTS

                            blCalledFromSTS = CBool(r_vAdditionalDataArray(1, i))
                        Case CNSkipSaveToDB

                            bSkipSaveToDB = CBool(r_vAdditionalDataArray(1, i))
                    End Select
                Next i
            Else
                'Assume that the call did not come from the STS
                blCalledFromSTS = False
                bSkipSaveToDB = False
            End If

            ' If this call hasn't arisen from the STS layer then skip out.
            If Not blCalledFromSTS Then
                Return nResult
            End If

            ' Set the Class Dataset reference
            m_oDataSet = r_oDataset

            'First thing we need to do is save the Dataset to the Database...
            'We are going to create bGIS.Application use SaveToDB method.
            'We need to do this in order that bSIRPerilAllocation etc. can access the Risk details in the DB.
            nReturn = m_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataSet:=sXMLDataSet)
            If nReturn <> PMEReturnCode.PMTrue Then
                nResult = nReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:="DataSet.ReturnAsXML method Failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            ' Create bGIS.Application object
            nReturn = CreateBusinessObject(r_oObject:=oBusiness, v_sClassName:="bGIS.Application", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            If nReturn <> PMEReturnCode.PMTrue Then
                nResult = nReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:="NBQuoteAfter Failed to Initialise bGIS.Application object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return nResult
            End If
            If Not bSkipSaveToDB Then

                nReturn = oBusiness.SaveToDB(v_sGisDataModelCode:=v_sGisDataModelCode, r_sXMLDataset:=sXMLDataSet)

                If nReturn <> PMEReturnCode.PMTrue Then
                    nResult = nReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:="bGIS.Application.SaveToDB method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If
            End If
            nReturn = m_oDataSet.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet)
            If nReturn <> PMEReturnCode.PMTrue Then
                nResult = nReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:="DataSet.LoadFromXML method Failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return nResult
            End If


            oBusiness.Dispose()
            oBusiness = Nothing

            decodeTransactionScreenAndType(v_lQuoteType, nTransactionType, nGISScreenId, nQuoteType)

            If nQuoteType = PBCQemQuoteTypeQuote Then
                'ONLY DO THIS IF WE ARE QUOTING

                'We are going to assume that we are always passing values in the AdditionalDataArray.
                'If not we are going to exit the function with and error
                If Informations.IsArray(r_vAdditionalDataArray) Then
                    For i As Integer = 0 To r_vAdditionalDataArray.GetUpperBound(1)
                        Select Case r_vAdditionalDataArray(0, i)
                            Case CNInsuranceFolderCnt

                                nInsuranceFolderCnt = CInt(r_vAdditionalDataArray(1, i))
                            Case CNInsuranceFileCnt

                                nInsuranceFileCnt = CInt(r_vAdditionalDataArray(1, i))
                            Case CNCurrentRiskCnt

                                nRiskCnt = CInt(r_vAdditionalDataArray(1, i))
                            Case CNTransactionType

                                sTransactionType = CStr(r_vAdditionalDataArray(1, i))
                        End Select
                    Next i
                Else
                    nResult = PMEReturnCode.PMFalse
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:="There is no information is the Additional Data Array. Must be populated.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If

                ' Create bSIRPerilAllocation object
                nReturn = CreateBusinessObject(r_oObject:=oBusiness, v_sClassName:="bSirPerilAllocation.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

                If nReturn <> PMEReturnCode.PMTrue Then
                    nResult = nReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:="NBQuoteAfter Failed to Initialise bSirPerilAllocation.Business object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If

                'Set the Business Object Properties
                ' Changed to pass InsuranceFolderCnt instead of InsuranceFileCnt to
                ' the oPerilAllocation.InsuranceFolderCnt. PW181105

                oBusiness.InsuranceFolderCnt = nInsuranceFolderCnt

                oBusiness.InsuranceFileCnt = nInsuranceFileCnt

                oBusiness.RiskID = nRiskCnt

                If sTransactionType = "" Then
                    sTransactionType = v_sGisBusinessTypeCode
                End If

                oBusiness.TransactionType = sTransactionType

                nReturn = oBusiness.PopulateRatingSections()
                If nReturn <> PMEReturnCode.PMTrue Then
                    nResult = nReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:="bSirPerilAllocation.Business.PopulateRatingSections method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If
                bQuestionUnanswered = oBusiness.QuestionUnanswered
                'Populate the output table.

                nReturn = oBusiness.UpdateRisk()
                If nReturn <> PMEReturnCode.PMTrue Then
                    nResult = nReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:="bSirPerilAllocation.Business.UpdateRisk method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If


                If oBusiness.DeclineReasons <> "" Then
                    Return oBusiness.UpdateRiskStatus(v_lRiskCnt:=ToSafeInteger(nRiskCnt), v_lRiskStatusID:=2)
                ElseIf oBusiness.ReferReasons <> "" Then
                    Return oBusiness.UpdateRiskStatus(v_lRiskCnt:=ToSafeInteger(nRiskCnt), v_lRiskStatusID:=1)
                ElseIf oBusiness.Messages <> "" Then
                    'The Risk
                End If

                oBusiness.Dispose()
                oBusiness = Nothing

                m_lReturn = bPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, v_vBranch:=m_iSourceID, v_sCallingAppName:=ACApp, r_vUnderwriting:=m_vIsRI2007, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel)
                If m_vIsRI2007 <> "1" Then
                    'Create bSIRReinsurance.Form object
                    sbRIComponentname = "bSIRReinsurance"
                    nReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oBusiness, v_sClassName:="bSIRReinsurance.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

                    If nReturn <> PMEReturnCode.PMTrue Then
                        nResult = nReturn
                        ' Log Error Message
                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NBQuoteAfter Failed to Initialise bSIRReinsurance.Form object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return nResult
                    End If
                Else
                    sbRIComponentname = "bSIRReinsuranceRI2007"
                    nReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oBusiness, v_sClassName:="bSIRReinsuranceRI2007.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

                    If nReturn <> PMEReturnCode.PMTrue Then
                        nResult = nReturn
                        ' Log Error Message
                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NBQuoteAfter Failed to Initialise bSIRReinsuranceRI2007.Form object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return nResult
                    End If

                End If

                m_lReturn = oBusiness.SetProcessModes(vTask:=PMEComponentAction.PMEdit, vNavigate:=0, vProcessMode:=110, vTransactionType:=ToSafeString(sTransactionType), vEffectiveDate:=DateTime.Now)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    nResult = nReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:="NBQuoteAfter Failed to SetProcessModes for " & sbRIComponentname & ".Form object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If

                ' Set the business keys.

                oBusiness.InsuranceFileCnt = nInsuranceFileCnt

                oBusiness.RiskID = nRiskCnt

                nReturn = oBusiness.ApplyReinsurance(bApplyReinsurance)
                If nReturn <> PMEReturnCode.PMTrue Then
                    nResult = nReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:=sbRIComponentname & ".Form.ApplyReinsurance method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If

                If Not bApplyReinsurance Then
                    nReturn = oBusiness.ChangeRiskStatus
                    If nReturn <> PMEReturnCode.PMTrue Then
                        nResult = nReturn
                        ' Log Error Message
                        GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:=sbRIComponentname & ".Form.ChangeRiskStatus method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return nResult
                    End If

                Else
                    'If m_vIsRI2007 <> "1" Then
                    '    ' Delete previous Reinsurance
                    '    lReturn = DeleteExistingReinsurance(lRiskCnt)
                    '    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    '        result = lReturn
                    '        ' Log Error Message
                    '        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteExistingReinsurance method failed.", vApp:=tosafestring(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    '        Return result
                    '    End If
                    'End If

                    nReturn = oBusiness.CalculateRI
                    If nReturn <> PMEReturnCode.PMTrue Then
                        nResult = nReturn
                        ' Log Error Message
                        GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:=sbRIComponentname & ".Form.CalculateRI method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return nResult
                    End If

                    nReturn = oBusiness.GetDetails
                    If nReturn <> PMEReturnCode.PMTrue And nReturn <> PMEReturnCode.PMNotFound Then
                        nResult = nReturn
                        ' Log Error Message
                        GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:=sbRIComponentname & ".Form.GetDetails method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return nResult
                    End If

                    nReturn = oBusiness.Update
                    If nReturn <> PMEReturnCode.PMTrue Then
                        nResult = nReturn
                        ' Log Error Message
                        GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:=sbRIComponentname & ".Form.Update method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return nResult
                    End If

                    nReturn = oBusiness.ValidateBands(r_lValid:=nRIValid, r_lBand:=nRIBand)
                    If nReturn <> PMEReturnCode.PMTrue Then
                        nResult = nReturn
                        ' Log Error Message
                        GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:=sbRIComponentname & ".Form.ValidateBands method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return nResult
                    End If

                    If nRIValid <> 0 Then
                        bIsRIValid = False
                    End If

                End If


                oBusiness.Dispose()
                oBusiness = Nothing

                ' Create the bSIRRITax.Business object
                nReturn = CreateBusinessObject(r_oObject:=oBusiness, v_sClassName:="bSIRRiskData.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

                If nReturn <> PMEReturnCode.PMTrue Then
                    nResult = nReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:="NBQuoteAfter Failed to Initialise bSirRiskData.Business object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If


                m_lReturn = oBusiness.SetProcessModes(vTask:=PMEComponentAction.PMEdit, vNavigate:=0, vProcessMode:=110, vTransactionType:=ToSafeString(sTransactionType), vEffectiveDate:=DateTime.Now)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    nResult = nReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:="NBQuoteAfter Failed to SetProcessModes for bSirRiskData.Business object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If

                'PM018420 - JP 01/03/2012
                If bIsRIValid <> True Then
                    m_lReturn = oBusiness.UpdateRiskStatus(v_lRiskCnt:=ToSafeInteger(nRiskCnt), v_lRiskStatusID:=8)
                    If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = nReturn
                        ' Log Error Message
                        GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:="bSIRReinsurance.Form.ChangeRiskStatus method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return nResult
                    End If
                Else
                    If Not bQuestionUnanswered Then
                        m_lReturn = oBusiness.UpdateRiskStatus(v_lRiskCnt:=ToSafeInteger(nRiskCnt), v_lRiskStatusID:=4)
                    End If
                End If


                oBusiness.Dispose()
                oBusiness = Nothing

                'We need three business object here

                ' Create the oSIRPartyFee.Business object
                oSIRPartyFee = New bSIRPartyFee.UBusiness

                nReturn = oSIRPartyFee.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                If nReturn <> PMEReturnCode.PMTrue Then
                    nResult = nReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:="NBQuoteAfter Failed to Initialise bSIRPartyFee.UBusiness object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If

                m_lReturn = oSIRPartyFee.SetProcessModes(vTask:=PMEComponentAction.PMEdit, vNavigate:=0, vProcessMode:=110, vTransactionType:=sTransactionType, vEffectiveDate:=DateTime.Now)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    nResult = nReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:="NBQuoteAfter Failed to SetProcessModes for bSIRPartyFee.UBusiness object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If

                ' Create the bSIRRITax.Business object
                oSIRRITax = New bSIRRITax.Business
                nReturn = oSIRRITax.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                If nReturn <> PMEReturnCode.PMTrue Then
                    nResult = nReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:="NBQuoteAfter Failed to Initialise bSIRRITax.Business object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If

                m_lReturn = oSIRRITax.SetProcessModes(vTask:=PMEComponentAction.PMEdit, vNavigate:=0, vProcessMode:=110, vTransactionType:=sTransactionType, vEffectiveDate:=DateTime.Now)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    nResult = nReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:="NBQuoteAfter Failed to SetProcessModes for bSIRRITax.Business object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If

                ' Create the bSIRListRisks.Business object
                oSIRListRisks = New bSIRListRisks.Business
                nReturn = oSIRListRisks.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                If nReturn <> PMEReturnCode.PMTrue Then
                    nResult = nReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:="NBQuoteAfter Failed to Initialise bSIRListRisks.Business object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If

                m_lReturn = oSIRListRisks.SetProcessModes(vTask:=PMEComponentAction.PMEdit, vNavigate:=0, vProcessMode:=110, vTransactionType:=sTransactionType, vEffectiveDate:=DateTime.Now)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    nResult = nReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:="NBQuoteAfter Failed to SetProcessModes for bSIRListRisks.Business object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If

                ' Recalculate the fees and taxes

                nReturn = oSIRPartyFee.RecalculateRiskFees(v_lInsuranceFileCnt:=nInsuranceFileCnt, v_lRiskCnt:=nRiskCnt, v_lTransactionTypeId:=nTransactionType, v_bUseExistingFeeDetail:=False)
                If nReturn <> PMEReturnCode.PMTrue Then
                    nResult = nReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:="bSIRPartyFee.UBusiness.RecalculateRiskFees method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If

                oSIRRITax.InsuranceFileCnt = nInsuranceFileCnt
                ' Recalculate the fees and taxes

                nReturn = oSIRRITax.RecalculatePolicyRiskTaxes(v_lRiskCnt:=nRiskCnt, v_lTask:=PMEComponentAction.PMEdit, v_sTransactionType:=sTransactionType)
                If nReturn <> PMEReturnCode.PMTrue Then
                    nResult = nReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:="bSIRRITax.Business.RecalculateRisk method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If

                'update the policy premium and tax

                nReturn = oSIRListRisks.UpdatePolicyPremium(v_lInsuranceFileCnt:=nInsuranceFileCnt)
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = nReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRListRisks.Business.UpdatePolicyPremium method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If

                ' Recalculate the Policy taxes

                nReturn = oSIRRITax.RecalculatePolicyTaxes(v_lInsuranceFileCnt:=nInsuranceFileCnt, v_lTask:=gPMConstants.PMEComponentAction.PMEdit, v_sTransactionType:=v_sGisBusinessTypeCode)
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = nReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRListRisks.Business.RecalculatePolicyTaxes method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If
                nReturn = oSIRListRisks.UpdatePolicyPremium(v_lInsuranceFileCnt:=nInsuranceFileCnt)
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = nReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRListRisks.Business.UpdatePolicyPremium method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return nReturn
                End If
                ' Recalculate the Policy Fees

                nReturn = oSIRPartyFee.RecalculatePolicyFees(v_lInsuranceFileCnt:=nInsuranceFileCnt, v_lProductId:=-1, v_lTransactionTypeId:=nTransactionType, v_bUseExistingFeeDetail:=False, nViaSam:=1)
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = nReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRPartyFee.UBusiness.RecalculatePolicyFees method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If


                oSIRPartyFee.Dispose()
                oSIRPartyFee = Nothing


                oSIRListRisks.Dispose()
                oSIRListRisks = Nothing


                oSIRRITax.Dispose()
                oSIRRITax = Nothing

                ' Create the bSirAgentCommission.Business object
                oBusiness = gPMFunctions.CreateLateBoundObject("bSirAgentCommission.Business")
                Dim oDatabase As Object = m_oDatabase
                nReturn = oBusiness.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeString(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=oDatabase)
                If nReturn <> PMEReturnCode.PMTrue Then
                    nResult = nReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:="NBQuoteAfter Failed to Initialise bSirAgentCommission.Business object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If
                m_oDatabase = oDatabase
                m_lReturn = oBusiness.SetProcessModes(vTask:=PMEComponentAction.PMEdit, vNavigate:=0, vProcessMode:=110, vTransactionType:=ToSafeString(sTransactionType), vEffectiveDate:=DateTime.Now)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    nResult = nReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:="NBQuoteAfter Failed to SetProcessModes for bSirAgentCommission.Business object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If

                'Set the Business Object Properties

                oBusiness.InsuranceFileCnt = nInsuranceFileCnt


                nReturn = oBusiness.CheckDisplayCommission(r_bDisplayScreen:=bDisplayScreen)
                If nReturn <> PMEReturnCode.PMTrue Then
                    nResult = nReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:="bSirAgentCommission.Business.CheckDisplayCommission method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If

                If Not bDisplayScreen Then
                    'No commission do nothing
                Else

                    nReturn = oBusiness.CalculateAgentCommission(v_lInsuranceFileCnt:=ToSafeInteger(nInsuranceFileCnt), v_sTransactionType:=ToSafeString(sTransactionType), r_vntResult:=oCommissionArray)                    '            End If
                    If nReturn <> PMEReturnCode.PMTrue Then
                        nResult = nReturn
                        ' Log Error Message
                        GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:="bSirAgentCommission.Business.CalculateAgentCommission method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return nResult
                    End If

                    If Informations.IsArray(oCommissionArray) Then
                        'Update the lead commission records for the insurancefile

                        nReturn = oBusiness.UpdateLeadCommission(ToSafeInteger(nInsuranceFileCnt))
                        If nReturn <> PMEReturnCode.PMTrue Then
                            nResult = nReturn
                            ' Log Error Message
                            GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:="bSirAgentCommission.Business.UpdateLeadCommission method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                            Return nResult
                        End If
                    End If

                End If


                oBusiness.Dispose()
                oBusiness = Nothing

            End If

            ' Set the return reference of the DatasetControl
            r_oDataset = m_oDataSet

            'Clear the dataset reference.
            m_oDataSet = Nothing

            Return nResult


        Catch excep As System.Exception


            m_oDataSet = Nothing

            nResult = PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:="NBQuoteAfter Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return nResult



        End Try
    End Function

    Public Function NBTransactAfter(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_oDataset As cGISDataSetControl.Application, ByVal v_vSchemeArray(,) As Object, ByRef r_vAdditionalDataArray As Array) As Integer
        '******************************************************************************
        '        Function Name:  NBTransactAfter
        '******************************************************************************
        '           Created By:  Ahmed "Jay" Bishtawi
        '           Created On:  19-Nov-2002
        '******************************************************************************
        '       Parameters Are:
        '         (In)     - v_sGisDataModelCode    - String                          -
        '         (In)     - v_sGisBusinessTypeCode - String                          -
        '         (In/Out) - r_oDataset             - cGISDataSetControl.Application  -
        '         (In)     - v_vSchemeArray         - Variant                         -
        '         (In/Out) - r_vAdditionalDataArray - Variant                         -
        '
        ' Return Value Type Is:  Long -
        '******************************************************************************
        ' Function Description:  This function replicate the
        '                        iPMUChangePolicyStatus.Interface.ProcessInterface
        '******************************************************************************

        Dim result As Integer = 0
        Try

            Dim lReturn As Integer
            Dim blCalledFromSTS As Boolean


            blCalledFromSTS = False

            For i As Integer = 0 To r_vAdditionalDataArray.GetUpperBound(1)
                Select Case r_vAdditionalDataArray(0, i)
                    Case CNCalledFromSTS

                        blCalledFromSTS = CBool(r_vAdditionalDataArray(1, i))
                        Exit For
                End Select
            Next i

            ' If this call hasn't arisen from the STS layer then skip out.
            If Not blCalledFromSTS Then
                Return result
            End If


            lReturn = NBTransactAfter_SFU(v_sGisDataModelCode, v_sGisBusinessTypeCode, r_oDataset, v_vSchemeArray, r_vAdditionalDataArray)

            result = lReturn

            ' Release the Ref to Local DataSet
            m_oDataSet = Nothing

            Return result

        Catch excep As System.Exception


            ' Error
            result = gPMConstants.PMEReturnCode.PMError

            ' Release the Ref to Local DataSet
            m_oDataSet = Nothing

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NBTransactAfter Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBTransactAfter", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function
    Public Function NBTransactAfter_SFU(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_oDataset As cGISDataSetControl.Application, ByVal v_vSchemeArray(,) As Object, ByRef r_vAdditionalDataArray As Array) As Integer

        Dim result As Integer = 0
        Dim oBusiness As bSIRChangePolicyStatus.Business
        Dim iMode As Integer
        Dim lInsuranceFileCnt As Integer
        Dim vRisks(,) As Object
        Dim sMessage As String = ""
        Dim iRisksUBound, iRisksLBound As Integer
        Dim lLevel As Integer
        Dim bSelectedRisks As Boolean
        Dim sNewPolicyNumber As String = ""
        Dim lReturn As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Our Class Ref to the Dataset
            m_oDataSet = r_oDataset

            ' CTAF - 20040303 commented out, why is this being done?
            '    ' Ensure the Business Type = "AOL"
            '    If v_sGisBusinessTypeCode <> ACDataModelCode Then
            '        Exit Function
            '    End If

            ' We must at least have the mode in there otherwise log an error and exit
            If Informations.IsArray(r_vAdditionalDataArray) Then
                ' Get the Claim Mode from additional Data Array
                lReturn = ParseArray(m_sUsername, r_vAdditionalDataArray, CNClaimMode, iMode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMError
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NBTransactAfter_SFU Failed - Failed to get Claim Mode value from AdditionalDataArray.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBTransactAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                ' Get the FileInsuranceCnt from AdditionalData Array
                lReturn = ParseArray(m_sUsername, r_vAdditionalDataArray, CNInsuranceFileCnt, lInsuranceFileCnt)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMError
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NBTransactAfter_SFU Failed - Failed to get InsuranceFileCnt value from AdditionalDataArray.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBTransactAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            Else
                result = gPMConstants.PMEReturnCode.PMError
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NBTransactAfter_SFU Failed - AdditionalDataArray is empty.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBTransactAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If


            ' Create the bSIRChangePolicyStatus object.
            oBusiness = New bSIRChangePolicyStatus.Business
            lReturn = oBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NBTransactAfter_SFU Failed - Failed to create bSIRChangePolicyStatus.Business object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBTransactAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            If iMode <> 1 Then

                lReturn = oBusiness.GetRisksByStatus(v_lInsuranceFileCnt:=lInsuranceFileCnt, r_vRisks:=vRisks)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NBTransactAfter_SFU Failed - bSIRChangePolicyStatus.Business.GetRisksByStatus method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBTransactAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                If Not Informations.IsArray(vRisks) Then

                    sMessage = "Cannot proceed -" & Strings.ChrW(13) & Strings.ChrW(10) &
                               "There are no risks on this policy."
                    ' Add the message to AdditionalDataArray
                    lReturn = AddToArray(m_sUsername, r_vAdditionalDataArray, CNNBTransactMessage, sMessage)

                    result = lReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NBTransactAfter_SFU Failed - bSIRChangePolicyStatus.Business.GetRisksByStatus method did not return an array of risk.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBTransactAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                Else
                    'Process vRisks Array
                    '1 = Referred
                    '2 = Declined
                    '3 = Quoted
                    '4 = Unquoted
                    '5 = Purchase question to be answered
                    '6 = Post quote questions to be answered
                    '7 = Pre quote questions to be answered
                    '8 = Pending Reinsurance


                    iRisksLBound = vRisks.GetLowerBound(1)

                    iRisksUBound = vRisks.GetUpperBound(1)

                    ' Reset Level and Message
                    sMessage = ""
                    lLevel = 0

                    ' Loop through the Risks Array and Check status.
                    For iCounter As Integer = iRisksLBound To iRisksUBound
                        'AAB-20112002 - I am disabling the selection critiria for now
                        'If vRisks(1, iCounter) = 1 Then
                        bSelectedRisks = True
                        Select Case vRisks(0, iCounter)
                            Case 1, 2, 4
                                If lLevel < 3 Then
                                    lLevel = 3
                                    sMessage = "Cannot proceed -" & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "At least one risk on this policy is unquoted"
                                End If
                            Case 8
                                If lLevel < 2 Then
                                    lLevel = 2
                                    sMessage = "Cannot proceed -" & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "At least one risk on this policy has no reinsurance"
                                End If
                            Case 5, 6, 7
                                If lLevel < 1 Then
                                    lLevel = 1
                                    sMessage = "Cannot proceed -" & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "At least one risk on this policy has questions to be answered"
                                End If
                        End Select
                        'End If
                    Next iCounter

                    ' Check if any of the risks are flagged as "selected"
                    If Not bSelectedRisks Then
                        sMessage = "Cannot proceed -" & Strings.ChrW(13) & Strings.ChrW(10) &
                                   "At least one risk on this policy must be selected to make it live"
                    End If

                    ' Only Proceed beyond this point if we did not have any error, sMessage is blank
                    If sMessage <> "" Then
                        ' Add the message to AdditionalDataArray
                        lReturn = AddToArray(m_sUsername, r_vAdditionalDataArray, CNNBTransactMessage, sMessage)
                        Return gPMConstants.PMEReturnCode.PMError
                    End If

                    ' PW151102 - If going live, delete any unselected risks' link records
                    'lReturn& = oBusiness.DeleteRisks(v_vRisks:=vRisks)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = lReturn
                        ' Log Error Message
                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NBTransactAfter_SFU Failed - bSIRChangePolicyStatus.Business.DeleteRisks method did not return an array of risk.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBTransactAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                    ' PW311002 - Re-jig the risk and variation numbers of the remaining
                    '            risks on this policy


                    lReturn = oBusiness.RenumberRisks(v_lInsuranceFileCnt:=CInt(vRisks(2, 0)))
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = lReturn
                        ' Log Error Message
                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NBTransactAfter_SFU Failed - bSIRChangePolicyStatus.Business.RenumberRisks method did not return an array of risk.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBTransactAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                    'Clear vRisks

                    vRisks = Nothing


                    oBusiness.Mode = iMode


                    lReturn = oBusiness.ChangePolicyStatus(v_lInsuranceFileCnt:=lInsuranceFileCnt)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = lReturn
                        ' Log Error Message
                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NBTransactAfter_SFU Failed - bSIRChangePolicyStatus.Business.ChangePolicyStatus method did not return an array of risk.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBTransactAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If


                    lReturn = oBusiness.UpdatePolicyPremium(v_lInsuranceFileCnt:=lInsuranceFileCnt)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = lReturn
                        ' Log Error Message
                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NBTransactAfter_SFU Failed - bSIRChangePolicyStatus.Business.UpdatePolicyPremium method did not return an array of risk.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBTransactAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                    'get back new policy number

                    sNewPolicyNumber = oBusiness.NewPolicyNumber
                    ' Add it to the AdditionalDataArray
                    lReturn = AddToArray(m_sUsername, r_vAdditionalDataArray, CNNewPolicyNumber, sNewPolicyNumber)

                End If 'Not IsArray(vRisks)

            End If 'iMode <> 1

            ' Realese the object

            oBusiness.Dispose()
            oBusiness = Nothing

            Return result

            ' error
            m_oDataSet = Nothing
        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NBTransactAfter_SFU Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBTransactAfter_SFU", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End Try

    End Function


    ' ***************************************************************** '
    ' Name: MTAStartBefore
    '
    ' Description: This method is called by the GIS when an MTA
    '              is started.
    '
    '              Called BEFORE GIS has done its stuff.
    ' ***************************************************************** '
    Public Function MTAStartBefore(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_iType As Integer, ByVal v_lOldInsuranceFileCnt As Integer, ByVal v_dtCoverStartDate As Date, ByVal v_dtExpiryDate As Date, ByVal v_lPolicyVersion As Integer, ByRef r_lNewInsuranceFileCnt As Integer, ByRef r_vAdditionalDataArray(,) As Object) As Integer

        Dim result As Integer = 0
        'Modified by Vijay Pal on 5/25/2010 1:30:57 PM todolist,iteration3,and declare  Dim oSBOLink As Object
        Dim oSBOLink As bSIRIUSLink.SIRIUSLink
        'Dim oSBOLink As Object
        Dim vArray(,) As Object
        Dim iFileType As Integer
        Dim sDebug As String = ""
        Dim lInsuranceFolderCnt As Integer

        Dim blCalledFromSTS As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            blCalledFromSTS = False

            For i As Integer = 0 To r_vAdditionalDataArray.GetUpperBound(1)
                Select Case r_vAdditionalDataArray(0, i)
                    Case CNCalledFromSTS

                        blCalledFromSTS = CBool(r_vAdditionalDataArray(1, i))
                        Exit For
                End Select
            Next i

            ' If this call hasn't arisen from the STS layer then skip out.
            If Not blCalledFromSTS Then
                Return result
            End If

            ' RG 081200 - debugging date reversal problem.
            sDebug = "CoverStartDate = " & v_dtCoverStartDate.ToString("yyyy-MM-dd") & "ExpiryDate = " & v_dtExpiryDate.ToString("yyyy-MM-dd")
            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sDebug, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTAStartBefore")
            ' RG End


            ' Create a SBO Link object
            'Set oSBOLink = New bSIRIUSLink.SIRIUSLink
            'Modified by Vijay Pal on 5/25/2010 1:32:06 PM todolist,iteration3,declare oSBOLink = New Object
            oSBOLink = New bSIRIUSLink.SIRIUSLink()
            ' oSBOLink = New Object

            m_lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "oSBOLink.Initialise Failed", ACApp, ACClass, "RegisterUser")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set GIS codes in SBOLink
            oSBOLink.GISDataModelCode = v_sGisDataModelCode
            oSBOLink.GISBusinessTypeCode = v_sGisBusinessTypeCode


            ' Compare end date of MTA with expiry date of original policy - CL080800
            m_lReturn = oSBOLink.GetPolicyVersions(r_lInsuranceFileCnt:=v_lOldInsuranceFileCnt, r_vResults:=vArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "oSBOLink.GetPolicyVersions Failed", ACApp, ACClass, "MTAStartBefore")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' CL100800
            Select Case v_iType
                Case GISSharedConstants.GISMTAStartTypePerm : iFileType = 4 ' SBO IFT numbers
                Case GISSharedConstants.GISMTAStartTypeTemp : iFileType = 7 ' SBO IFT numbers
                Case GISSharedConstants.GISMTAStartTypeCancel : iFileType = 8 ' SBO IFT numbers - to de decided
                Case GISSharedConstants.GISMTAStartTypeReinstate : iFileType = 9 ' SBO IFT numbers - to de decided
            End Select


            m_lReturn = oSBOLink.NewPolicyVersion(v_lCurrentInsuranceFileCnt:=v_lOldInsuranceFileCnt, v_dCoverStartDate:=v_dtCoverStartDate, v_dExpiryDate:=v_dtExpiryDate, v_enmInsuranceFileType:=iFileType, v_lPolicyVersion:=v_lPolicyVersion, r_lNewInsuranceFileCnt:=r_lNewInsuranceFileCnt, v_cAnnualPremium:=0, r_lInsuranceFolderCnt:=lInsuranceFolderCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "oSBOLink.NewPolicyVersion Failed", ACApp, ACClass, "MTAStartBefore")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' RAG 04/12/2001 - Add risk details, as for NB (SFU/MIA only)
            ' RAG 06/12/2001 - No, we should use the same risk_folder, as this is what SFU does.
            '    If (v_sGisBusinessTypeCode = "MIA") Then
            '        m_lReturn = oSBOLink.SFUAddRisk(v_lInsuranceFileCnt:=tosafeinteger(r_lNewInsuranceFileCnt), _
            ''                                        r_lRiskFolderCnt:=tosafeinteger(lRiskFolderCnt), _
            ''                                        r_lRiskCnt:=tosafeinteger(lRiskCnt), _
            ''                                        v_lInsuranceFolderCnt:=lInsuranceFolderCnt)
            '        If (m_lReturn <> PMTrue) Then
            '            LogMessageFile PMLogOnError, "oSBOLink.SFUAddRisk Failed", ACApp, ACClass, "NBStartBefore"
            '            MTAStartBefore = PMFalse
            '            Exit Function
            '        End If
            '
            '        ' Store the return values to the additional data array
            '        m_lReturn = AddToArray(m_susername$, r_vAdditionalDataArray, "NP_RISK_FOLDER_CNT", lRiskFolderCnt)
            '        m_lReturn = AddToArray(m_susername$, r_vAdditionalDataArray, "NP_RISK_CNT", lRiskCnt)
            '    End If


            oSBOLink.Dispose()
            oSBOLink = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MTAStartBefore Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTAStartBefore", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: MTAStartAfter
    '
    ' Description: This method is called by the GIS when an MTA
    '              is started.
    '              Called AFTER GIS has done its stuff.
    ' ***************************************************************** '
    Public Function MTAStartAfter(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_lNewInsuranceFileCnt As Integer, ByRef r_oDataset As cGISDataSetControl.Application, ByRef r_vAdditionalDataArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim oSBOLink As Object
        Dim lRiskCnt As Object
        Dim lRiskFolderCnt As Object
        Dim sQuery As String = ""
        Dim blCalledFromSTS As Boolean
        Dim lInsurance_FileCnt As Object
        Dim oDatabase As Object = m_oDatabase

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            blCalledFromSTS = False

            For i As Integer = 0 To r_vAdditionalDataArray.GetUpperBound(1)
                Select Case r_vAdditionalDataArray(0, i)
                    Case CNCalledFromSTS

                        blCalledFromSTS = CBool(r_vAdditionalDataArray(1, i))
                        Exit For
                End Select
            Next i

            ' If this call hasn't arisen from the STS layer then skip out.
            If Not blCalledFromSTS Then
                Return result
            End If

            m_oDataSet = r_oDataset

            ' Update the InsuranceFileCnt in the Dataset
            m_oDataSet.Risk.Item(ACDataModelCodePrefix & "POLICY").Item("NP_INSURANCE_FILE_CNT").Value = CStr(r_lNewInsuranceFileCnt)

            ' PWF 06/11/2001 - Add risk details (SFU/MIA only)

            If v_sGisBusinessTypeCode = "MIA" Then

                ' RAG 04/12/2001 - I think this should work the same as NB, i.e. a new risk folder
                ' RAG 06/12/2001 - No, we should use the same risk_folder as NB, as this is what SFU does.

                ' Get values from the additional data array
                'm_lReturn = ParseArray(m_susername$, r_vAdditionalDataArray, "NP_RISK_FOLDER_CNT", lRiskFolderCnt)
                'm_lReturn = ParseArray(m_susername$, r_vAdditionalDataArray, "NP_RISK_CNT", lRiskCnt)

                ' Store the return values to the dataset
                'm_oDataSet.Risk.Item(ACDataModelCodePrefix & "POLICY").Item("NP_RISK_FOLDER_CNT").Value = lRiskFolderCnt
                'm_oDataSet.Risk.Item(ACDataModelCodePrefix & "POLICY").Item("NP_RISK_CNT").Value = lRiskCnt


                ' Get the existing risk folder
                lRiskFolderCnt = CInt(m_oDataSet.Risk.Item(ACDataModelCodePrefix & "POLICY").Item("NP_RISK_FOLDER_CNT").Value)



                ' Create a SBO Link object
                oSBOLink = New bSIRIUSLink.SIRIUSLink
                m_lReturn = oSBOLink.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeString(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=oDatabase)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "oSBOLink.Initialise Failed", ACApp, ACClass, "MTAStartAfter")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_oDatabase = oDatabase

                ' Set GIS codes in SBOLink

                oSBOLink.GISDataModelCode = v_sGisDataModelCode

                oSBOLink.GISBusinessTypeCode = v_sGisBusinessTypeCode

                ' Add risk

                m_lReturn = oSBOLink.SFUAddRisk(v_lInsuranceFileCnt:=lInsurance_FileCnt, r_lRiskFolderCnt:=lRiskFolderCnt, r_lRiskCnt:=lRiskCnt, v_lOldRiskFolderCnt:=ToSafeInteger(lRiskFolderCnt))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "oSBOLink.SFUAddRisk Failed", ACApp, ACClass, "MTAStartBefore")
                    Return gPMConstants.PMEReturnCode.PMFalse
                Else
                    r_lNewInsuranceFileCnt = lInsurance_FileCnt
                End If

                ' Store the return values to the dataset
                m_oDataSet.Risk.Item(ACDataModelCodePrefix & "POLICY").Item("NP_RISK_FOLDER_CNT").Value = CStr(lRiskFolderCnt)
                m_oDataSet.Risk.Item(ACDataModelCodePrefix & "POLICY").Item("NP_RISK_CNT").Value = CStr(lRiskCnt)

                ' Update the GIS_Policy_Link record

                ' Use dynamic sql for development speed (yes, it is called risk_id in the table, jeez)
                sQuery = "UPDATE GIS_Policy_Link " &
                         "SET risk_id = " & CStr(lRiskCnt) & " " &
                         "WHERE insurance_file_cnt = " & CStr(r_lNewInsuranceFileCnt)

                ' Send the query to the log file
                GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogDebug1, sMsg:="Updating GIS_Policy_Link risk_id" & Strings.ChrW(13) & Strings.ChrW(10) & "   " & sQuery, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTAStartAfter")

                m_lReturn = m_oDatabase.SQLAction(sSQL:=sQuery, sSQLName:="UpdateGISPolicyLink", bStoredProcedure:=False)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "GIS_Policy_Link Update Failed", ACApp, ACClass, "MTAStartBefore")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Release the SBO object

                oSBOLink.Dispose()
                oSBOLink = Nothing
            End If

            ' Release our reference to the dataset
            m_oDataSet = Nothing

            Return result

        Catch excep As System.Exception


            m_oDataSet = Nothing

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MTAStartAfter Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTAStartAfter", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: MTATransactAfter
    '
    ' Description: This method is called by the GIS after an MTA
    '              Quote is Transacted
    ' ***************************************************************** '
    Public Function MTATransactAfter(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_oDataset As cGISDataSetControl.Application, ByVal v_lQuoteType As Integer, ByVal v_vSchemeArray(,) As Object, ByRef r_vAdditionalDataArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim blCalledFromSTS As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            blCalledFromSTS = False

            For i As Integer = 0 To r_vAdditionalDataArray.GetUpperBound(1)
                Select Case r_vAdditionalDataArray(0, i)
                    Case CNCalledFromSTS

                        blCalledFromSTS = CBool(r_vAdditionalDataArray(1, i))
                        Exit For
                End Select
            Next i

            ' If this call hasn't arisen from the STS layer then skip out.
            If Not blCalledFromSTS Then
                Return result
            End If

            ' Decide whether to run the SFB or SFU version

            If v_sGisBusinessTypeCode = "MIA" Then
                ' Run SFU
                m_lReturn = MTATransactAfter_SFU(v_sGisDataModelCode, v_sGisBusinessTypeCode, r_oDataset, v_lQuoteType, v_vSchemeArray, r_vAdditionalDataArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MTATransactAfter_SFU Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter")
                End If

                Return m_lReturn
            Else
                ' Run SFB
                m_lReturn = MTATransactAfter_SFB(v_sGisDataModelCode, v_sGisBusinessTypeCode, r_oDataset, v_lQuoteType, v_vSchemeArray, r_vAdditionalDataArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MTATransactAfter_SFB Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter")
                End If

                Return m_lReturn
            End If

        Catch
        End Try


        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MTATransactAfter Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result


        'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

        Return result
    End Function


    ' ***************************************************************** '
    ' Name: MTATransactAfter_SFU
    '
    ' Description: This method is called by the GIS after an MTA
    '              Quote is Transacted
    '
    ' ***************************************************************** '
    Public Function MTATransactAfter_SFU(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_oDataset As cGISDataSetControl.Application, ByVal v_lQuoteType As Integer, ByVal v_vSchemeArray(,) As Object, ByRef r_vAdditionalDataArray(,) As Object) As Integer
        Dim result As Integer = 0
        Dim sIsCustEmailRequired As String = ""
        Dim lInsurerNo As Integer

        'Dim lPartyCnt As Long
        Dim lInsuranceFileCnt As Integer
        Dim sMake, sModel, sMakeDescription, sModelDescription, sPolNo As String
        'Modified by Vijay Pal on 5/25/2010 1:33:11 PM todolist,iteration3,declare Dim oSBOLink As object
        'Dim oSBOLink As bSIRIUSLink.SIRIUSLink
        Dim oSBOLink As bSIRIUSLink.SIRIUSLink
        Dim sSubject, sFrom As String

        Dim lGISPolicyLinkID As gPMConstants.PMEReturnCode
        Dim bSOFStepRequired, bBorderauxStepReq As Boolean
        Dim sInsurerNo, sInsurerDesc, sSchDesc, sBrandCode As String

        ' PF041001
        Dim lBrokerFeePartyCnt As Integer
        Dim crBrokerFeeAmount As Decimal
        Dim sGetSetting As String = ""

        'RJG 121001 - MTA premiums to be passed to SBO
        Dim cThisPremium, cNetPremium As Decimal
        Dim sCommPerc As String = ""
        Dim dCommissionPerc As Double
        Dim cCommissionAmount As Decimal
        Dim cIPTPercent As Double
        Dim cAnnualGross, cAnnualNet, cAnnualIPT As Decimal


        ' PWF 06/11/2001 - Reusable object for SFU Posting
        Dim oBusiness As Object
        Dim sQuery As String = ""
        Dim vArray(,) As Object
        Dim lRiskCnt, lRSTypeID As Integer
        Dim bApplyTaxes As Object
        Dim sDescription As Object = ""
        Dim vRiskTax As Object
        Dim lSchemeID As Integer

        ' RAG 2002-02-15 - change of address
        Dim sAddressLine(4) As String
        Dim lPartyCnt As Integer
        Dim sPartyCnt As String = ""
        Dim sClient As String

        Dim sMessage As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Inside MTATransactAfter_SFU...", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFU")

            m_oDataSet = r_oDataset

            ' Get the Policy LinkID
            lGISPolicyLinkID = m_oDataSet.PolicyLinkID()

            'CJB 050601 QMM Integration Change

            Dim dbNumericTemp As Double
            If Double.TryParse(CStr(v_vSchemeArray(GISSharedConstants.GISQEMSchAbi81Insurer, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                sInsurerNo = CStr(v_vSchemeArray(GISSharedConstants.GISQEMSchAbi81Insurer, 0))
            Else
                sInsurerNo = CStr(0)
            End If

            '***************************************************************************
            ' MTA Statement of Fact Email
            '****************************************************************************

            ' Update the Trans Type to say we are about to do a SOF
            m_lReturn = bGISTemp.UpdatePolicyLinkTransact(v_lPolicyLinkID:=lGISPolicyLinkID, v_dTransactDate:=DateTime.Now, v_sTransactType:=bGISTemp.GISMTATransTypeSOF, r_oDatabase:=m_oDatabase, r_bTransStepRequired:=bSOFStepRequired)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "UpdatePolicyLinkTransact (GISMTATransTypeSOF) Failed", ACApp, ACClass, "MTATransactAfter_SFU")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If we need to do the SOF (i.e. We have not done it before.)
            If bSOFStepRequired Then

                ' Check registry first to see if the SOF email is needed
                m_lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sSettingName:=GISSharedConstants.GISRegMTASOFEmailRequired, r_sSettingValue:=sIsCustEmailRequired, v_sBusinessTypeCode:=v_sGisBusinessTypeCode, v_sSubKey:=GISSharedConstants.GISRegSubKeyEmails)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Get Customer Email Reg Setting OK", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFU")

                ' RFC050800 - Do NOT generate SOF email for Cancellations
                If sIsCustEmailRequired = "1" And v_lQuoteType <> GISSharedConstants.GISMTAQuoteTypeAddCancellation Then

                    ' Check registry to get Subject
                    m_lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sSettingName:=GISSharedConstants.GISRegMTASOFEmailSubject, r_sSettingValue:=sSubject, v_sBusinessTypeCode:=v_sGisBusinessTypeCode, v_sSubKey:=GISSharedConstants.GISRegSubKeyEmails)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "GetRegSettingFromDataBusModel Failed : " & GISSharedConstants.GISRegMTASOFEmailSubject, ACApp, ACClass, "RegisterUser")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Check registry to get From
                    m_lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sSettingName:=GISSharedConstants.GISRegMTASOFEmailFrom, r_sSettingValue:=sFrom, v_sBusinessTypeCode:=v_sGisBusinessTypeCode, v_sSubKey:=GISSharedConstants.GISRegSubKeyEmails)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "GetRegSettingFromDataBusModel Failed : " & GISSharedConstants.GISRegMTASOFEmailFrom, ACApp, ACClass, "RegisterUser")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    sInsurerDesc = CStr(v_vSchemeArray(GISSharedConstants.GISQEMSchInsurerDesc, 0))

                    sSchDesc = CStr(v_vSchemeArray(GISSharedConstants.GISQEMSchDesc, 0))

                    sBrandCode = m_oDataSet.Risk.Item(ACDataModelCodePrefix & "POLICY").Item("NP_BRAND").Value


                    m_lReturn = GenerateCustomerEmail(v_sGisDataModelCode:=v_sGisDataModelCode, v_sSubject:=sSubject, v_sFrom:=sFrom, v_sInsurerNo:=sInsurerNo, v_sInsurerDesc:=sInsurerDesc, v_sSchemeName:=sSchDesc, v_sBrandCode:=sBrandCode, v_bMTA:=True)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateCustomerEmail Failed inside MTATransactAfter_SFU", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFU")

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

                GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Generate Customer Email OK", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFU")

            End If

            '***************************************************************************
            ' Generate the Borderaux
            '****************************************************************************

            ' Update the Trans Type to say we are about to do the Borderaux
            m_lReturn = bGISTemp.UpdatePolicyLinkTransact(v_lPolicyLinkID:=lGISPolicyLinkID, v_dTransactDate:=DateTime.Now, v_sTransactType:=bGISTemp.GISMTATransTypeBorderaux, r_oDatabase:=m_oDatabase, r_bTransStepRequired:=bBorderauxStepReq)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "UpdatePolicyLinkTransact (GISMTATransTypeBorderaux) Failed", ACApp, ACClass, "MTATransactAfter_SFU")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If we need to do the SOF (i.e. We have not done it before.)
            If bBorderauxStepReq Then

                m_lReturn = GenerateBordereaux(v_lInsurerNo:=lInsurerNo, v_lMTAQuoteType:=v_lQuoteType)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "GenerateBordereaux Failed", ACApp, ACClass, "MTATransactAfter_SFU")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            '***************************************************************************
            ' Make the MTA Live
            '****************************************************************************

            ' Update the Trans Type to say we are about to make the MTA Live
            m_lReturn = bGISTemp.UpdatePolicyLinkTransact(v_lPolicyLinkID:=lGISPolicyLinkID, v_dTransactDate:=DateTime.Now, v_sTransactType:=bGISTemp.GISMTATransTypeMakeLive, r_oDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "UpdatePolicyLinkTransact (GISMTATransTypeMakeLive) Failed", ACApp, ACClass, "MTATransactAfter_SFU")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lInsuranceFileCnt = CInt(m_oDataSet.Risk.Item(ACDataModelCodePrefix & "POLICY").Item("NP_INSURANCE_FILE_CNT").Value)

            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Got Ins File Cnt: " & lInsuranceFileCnt, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFU")

            sPolNo = m_oDataSet.Risk.Item(ACDataModelCodePrefix & "POLICY").Item("POLICY_NO").Value

            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Got Pol No: " & sPolNo, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFU")

            sMake = m_oDataSet.Risk.Item(ACDataModelCodePrefix & "VEHICLE").Item("MANUFACTURER").Value

            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Got Make: " & sMake, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFU")

            sModel = m_oDataSet.Risk.Item(ACDataModelCodePrefix & "VEHICLE").Item("MODEL").Value

            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Got Model: " & sModel, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFU")

            ' Get vehicle make and model descriptions
            m_lReturn = GetVehicleDescriptionFromABI(v_sGisDataModelCode, v_sGisBusinessTypeCode, sMake, sModel, sMakeDescription, sModelDescription)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "GetVehicleDescriptionFromABI Failed", ACApp, ACClass, "MTATransactAfter_SFU")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Got Make Description: " & sMakeDescription, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFU")

            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Got Model Description: " & sModelDescription, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFU")

            'Modified by Vijay Pal on 5/25/2010 1:34:28 PM todolist,iteration3,declare oSBOLink = New Object
            'oSBOLink = New bSIRIUSLink.SIRIUSLink()
            oSBOLink = New Object

            If oSBOLink Is Nothing Then

                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MTATransactAfter_SFU Failed: bSIRIUSLink.SIRIUSLink not created", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFU")

            End If

            m_lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "oSBOLink.Initialise Failed", ACApp, ACClass, "MTATransactAfter_SFU")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SBO Link Initialised", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFU")


            ' RAG 11/07/2002 - Moved up from below.

            cAnnualGross = Val(m_oDataSet.Risk.Item("CALCULATED_RESULT", 1).Item("new_risk_premium").Value)

            'CJB 290702 Use correct field !
            cAnnualIPT = Val(m_oDataSet.Risk.Item("POLICY", 1).Item("policy_ipt").Value)
            'cAnnualIPT = Val(m_oDataSet.Risk.Item("CALCULATED_RESULT", 1).Item("ipt_amount").Value)

            cAnnualNet = cAnnualGross - cAnnualIPT

            'CJB 290702 This has not been calculating at exactly 5% so results in incorrect postings
            'Since half of the system has a hardcoded 5% then I shall do the same here!
            'cIPTPercent = cAnnualIPT / (cAnnualNet - cAnnualIPT)
            cIPTPercent = 0.05

            ' RAG 12/07/2002 - I've had to bodge this for now because I can't find where
            ' to get the IPT from for cancellations. IPT_AMOUNT is zero.
            If v_lQuoteType = GISSharedConstants.GISMTAQuoteTypeAddCancellation Then

                ' THIS NEEDS SORTING OUT PROPERLY !!!
                cIPTPercent = 0.05
            End If


            'RJG 121001 - Get the premiums
            cThisPremium = Val(m_oDataSet.Risk.Item("CALCULATED_RESULT", 1).Item("OVERRIDE_PREMIUM_INCL_IPT").Value)
            cNetPremium = Math.Round(cThisPremium / (1 + cIPTPercent), 2)

            ' ----------------------------------------------
            ' CJB290702 Start - O/P Extra info for debugging
            ' ----------------------------------------------
            sMessage = Strings.ChrW(13) & Strings.ChrW(10) & "----------------------------------------------------------------------" & Strings.ChrW(13) & Strings.ChrW(10)
            sMessage = sMessage & "MTATransactAfter_SFU premium related information:" & Strings.ChrW(13) & Strings.ChrW(10)
            sMessage = sMessage & "cAnnualGross:" & CStr(cAnnualGross) & Strings.ChrW(13) & Strings.ChrW(10)
            sMessage = sMessage & "cAnnualIPT:" & CStr(cAnnualIPT) & Strings.ChrW(13) & Strings.ChrW(10)
            sMessage = sMessage & "cAnnualNet:" & CStr(cAnnualNet) & Strings.ChrW(13) & Strings.ChrW(10)
            sMessage = sMessage & "cIPTPercent:" & CStr(cIPTPercent) & Strings.ChrW(13) & Strings.ChrW(10)
            sMessage = sMessage & "v_cNetPremium:" & CStr(cNetPremium) & Strings.ChrW(13) & Strings.ChrW(10)
            sMessage = sMessage & "cThisPremium:" & CStr(cThisPremium) & Strings.ChrW(13) & Strings.ChrW(10)
            sMessage = sMessage & "cNetPremium:" & CStr(cNetPremium) & Strings.ChrW(13) & Strings.ChrW(10)
            sMessage = sMessage & "----------------------------------------------------------------------" & Strings.ChrW(13) & Strings.ChrW(10)

            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFU")
            ' ----------------------------------------------
            ' CJB290702 End
            ' ----------------------------------------------

            'CJB 250401 QMM Integration change (rqd as no comm perc set)

            If Not (Convert.IsDBNull(v_vSchemeArray(GISSharedConstants.GISQEMSchCommPerc, 0)) Or Informations.IsNothing(v_vSchemeArray(GISSharedConstants.GISQEMSchCommPerc, 0))) Then

                sCommPerc = CStr(v_vSchemeArray(GISSharedConstants.GISQEMSchCommPerc, 0))
            Else
                sCommPerc = CStr(0)
            End If

            dCommissionPerc = Val(sCommPerc)
            dCommissionPerc = (dCommissionPerc * 100) / (100 + dCommissionPerc)
            cCommissionAmount = cNetPremium * (dCommissionPerc / 100)

            ' RAG 11/07/2002 - End


            If v_lQuoteType = GISSharedConstants.GISMTAQuoteTypeAddCancellation Then

                ' Set policy to cancel  - CL140700
                ' Pass into optional parameter the new return premium - MSB 04.07.2002

                m_lReturn = oSBOLink.CancelPolicy(v_lInsuranceFileCnt:=lInsuranceFileCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "oSBOLink.CancelPolicy Failed", ACApp, ACClass, "MTATransactAfter_SFU")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SBO CancelPolicy OK", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFU")

            Else

                ' RAG 11/07/2002 - Moved further up.
                '
                '        cAnnualGross = m_oDataSet.Risk.Item("CALCULATED_RESULT", 1).Item("new_risk_premium").Value
                '        cAnnualIPT = m_oDataSet.Risk.Item("CALCULATED_RESULT", 1).Item("ipt_amount").Value
                '        cAnnualNet = cAnnualGross - cAnnualIPT
                '        cIPTPercent = cAnnualIPT / cAnnualNet
                '
                '        'RJG 121001 - Get the premiums
                '        cThisPremium = m_oDataSet.Risk.Item("CALCULATED_RESULT", 1).Item("OVERRIDE_PREMIUM_INCL_IPT").Value
                '        cNetPremium = Round(cThisPremium / (1 + cIPTPercent), 2)
                '
                '        'CJB 250401 QMM Integration change (rqd as no comm perc set)
                '        If Not IsNull(v_vSchemeArray(GISQEMSchCommPerc, 0)) Then
                '            sCommPerc = v_vSchemeArray(GISQEMSchCommPerc, 0)
                '        Else
                '            sCommPerc = 0
                '        End If
                '
                '        dCommissionPerc = Val(sCommPerc)
                '        dCommissionPerc = (dCommissionPerc * 100) / (100 + dCommissionPerc)
                '        cCommissionAmount = cNetPremium * (dCommissionPerc / 100)
                '
                ' RAG 11/07/2002 - End

                ' ----------------------------------------------
                ' CJB290702 Start - O/P Extra info for debugging
                ' ----------------------------------------------
                sMessage = Strings.ChrW(13) & Strings.ChrW(10) & "----------------------------------------------------------------------" & Strings.ChrW(13) & Strings.ChrW(10)
                sMessage = sMessage & "About to call oSBOLink.Quote2Policy in MTATransactAfter_SFU with:" & Strings.ChrW(13) & Strings.ChrW(10)
                sMessage = sMessage & "v_lInsuranceFileCnt:" & CStr(lInsuranceFileCnt) & Strings.ChrW(13) & Strings.ChrW(10)
                sMessage = sMessage & "v_sInsuranceRef:" & sPolNo & Strings.ChrW(13) & Strings.ChrW(10)
                sMessage = sMessage & "v_sVehicleMakeModel:" & sMakeDescription & " " & sModelDescription & Strings.ChrW(13) & Strings.ChrW(10)
                sMessage = sMessage & "v_cThisPremium:" & CStr(cThisPremium) & Strings.ChrW(13) & Strings.ChrW(10)
                sMessage = sMessage & "v_cNetPremium:" & CStr(cNetPremium) & Strings.ChrW(13) & Strings.ChrW(10)
                sMessage = sMessage & "v_cCommissionAmount:" & CStr(cCommissionAmount) & Strings.ChrW(13) & Strings.ChrW(10)
                sMessage = sMessage & "v_dCommissionPerc:" & CStr(dCommissionPerc) & Strings.ChrW(13) & Strings.ChrW(10)
                sMessage = sMessage & "----------------------------------------------------------------------" & Strings.ChrW(13) & Strings.ChrW(10)

                GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFU")
                ' ----------------------------------------------
                ' CJB290702 End
                ' ----------------------------------------------

                ' Convert the Quote To a Policy
                ' Set to MTA
                m_lReturn = oSBOLink.MTAQuote2Policy(v_lInsuranceFileCnt:=lInsuranceFileCnt, v_sInsuranceRef:=sPolNo, v_sVehicleMakeModel:=(sMakeDescription & " " & sModelDescription), v_cThisPremium:=cThisPremium, v_cNetPremium:=cNetPremium, v_cCommissionAmount:=cCommissionAmount, v_dCommissionPerc:=dCommissionPerc)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "oSBOLink.Quote2Policy Failed", ACApp, ACClass, "MTATransactAfter_SFU")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SBO Quote2Policy OK", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFU")


                ' RAG 2002-02-15
                ' We need to update the Party address with the Risk address
                ' as it might be a change of address MTA
                ' Then, we will create a WorkManager task to remind the operator
                ' to check any other policies for this party
                ' (as these might need MTA's too)

                ' This bit has been taken from mta_20.asp,
                ' where Mo had inserted it (incorrectly).

                lPartyCnt = 0
                sPartyCnt = m_oDataSet.Risk.Item("POLICY", 1).Item("NP_Party_Cnt").Value   'State_GetValue("POLICY__1__NP_Party_Cnt")

                Dim dbNumericTemp2 As Double
                If Double.TryParse(sPartyCnt, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                    lPartyCnt = CInt(sPartyCnt)

                    sAddressLine(0) = m_oDataSet.Risk.Item("PROPOSER_POLICYHOLDER", 1).Item("Address_Line_1").Value
                    sAddressLine(1) = m_oDataSet.Risk.Item("PROPOSER_POLICYHOLDER", 1).Item("Address_Line_2").Value
                    sAddressLine(2) = m_oDataSet.Risk.Item("PROPOSER_POLICYHOLDER", 1).Item("Address_Line_3").Value
                    sAddressLine(3) = m_oDataSet.Risk.Item("PROPOSER_POLICYHOLDER", 1).Item("Address_Line_4").Value
                    sAddressLine(4) = m_oDataSet.Risk.Item("PROPOSER_POLICYHOLDER", 1).Item("Address_Post_Code").Value

                    sClient = m_oDataSet.Risk.Item("PROPOSER_POLICYHOLDER", 1).Item("forename_initial_1").Value
                    sClient = sClient & " " & m_oDataSet.Risk.Item("PROPOSER_POLICYHOLDER", 1).Item("surname").Value

                    GISSharedConstants.DebugLogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "About to update Party Address: " &
                                                           " v_lPartyCnt:=" & CStr(lPartyCnt) &
                                                           " v_sAddress1:=" & sAddressLine(0) &
                                                           " v_sAddress2:=" & sAddressLine(1) &
                                                           " v_sAddress3:=" & sAddressLine(2) &
                                                           " v_sAddress4:=" & sAddressLine(3) &
                                                           " v_sPostcode:=" & sAddressLine(4), ACApp, ACClass, "MTATransactAfter_SFU")


                    m_lReturn = oSBOLink.UpdateParty(v_lPartyCnt:=lPartyCnt, v_sAddress1:=sAddressLine(0), v_sAddress2:=sAddressLine(1),
                                                     v_sAddress3:=sAddressLine(2), v_sAddress4:=sAddressLine(3), v_sPostCode:=sAddressLine(4))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "oSBOLink.UpdateParty Failed", ACApp, ACClass, "MTATransactAfter_SFU")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = CreateChangeAddressWMTask(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_lInsFileCnt:=lInsuranceFileCnt, r_sClientName:=sClient, r_sPolicyNo:=sPolNo)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "CreateChangeAddressWMTask Failed", ACApp, ACClass, "MTATransactAfter_SFU")
                        'MTATransactAfter_SFU = PMFalse
                        'Exit Function
                    End If


                Else

                    GISSharedConstants.DebugLogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "About to update Party Address: Invalid party_cnt: " & sPartyCnt, ACApp, ACClass, "MTATransactAfter_SFU")

                End If


            End If

            ' PWF 08/11/2001 - Not need in SFU?
            ' RAG - Yes it is, because they want to behave like a broker and have admin fees!

            ' PF031001 - Add party fees functionality - START
            ' Set default (and invalid) states
            lBrokerFeePartyCnt = 0
            crBrokerFeeAmount = 0

            ' Fees: Broker Fee / Admin Fee.
            m_lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sSettingName:=GISRegDMCBrokerFeePartyCnt, r_sSettingValue:=sGetSetting)
            ' Check for valid party_cnt
            Dim dbNumericTemp3 As Double
            If Double.TryParse(sGetSetting, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                lBrokerFeePartyCnt = CInt(sGetSetting)
            End If

            GISSharedConstants.DebugLogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "lBrokerFeePartyCnt:=" & lBrokerFeePartyCnt &
                                                   ACApp, ACClass, "MTATransactAfter_SFU")

            ' Only proceed if we have a party count
            If lBrokerFeePartyCnt > 0 Then
                sGetSetting = ""
                sGetSetting = m_oDataSet.Risk.Item("PAYMENT_BANK").Item("BROKER_FEE").Value

                ' Check for valid amount
                Dim dbNumericTemp4 As Double
                If Double.TryParse(sGetSetting, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                    crBrokerFeeAmount = CDec(sGetSetting)

                    ' A word of explanation: Trim to 2 decimal places.
                    crBrokerFeeAmount = CDec(StringsHelper.Format(crBrokerFeeAmount, "####0.00"))
                End If
            End If

            GISSharedConstants.DebugLogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "cBrokerFeeAmount:=" & crBrokerFeeAmount & ACApp, ACClass, "MTATransactAfter_SFU")

            ' Decide if we have anything to post
            If crBrokerFeeAmount > 0 Then
                ' Debug.
                GISSharedConstants.DebugLogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Before Add Party Fees" &
                                                       "  v_lInsuranceFileCnt:=" & CStr(lInsuranceFileCnt) &
                                                       ", v_lLegalPartyCnt:=0" &
                                                       ", v_cLegalAmount:=0" &
                                                       ", v_cLegalCommissionAmount:=0" &
                                                       ", v_lBreakdownPartyCnt:=0" &
                                                       ", v_cBreakdownAmount:=0" &
                                                       ", v_cBreakdownCommissionAmount:=0" &
                                                       ", v_lPromptPartyCnt:=0" &
                                                       ", v_cPromptAmount:=0" &
                                                       ", v_cPromptCommission:=0" &
                                                       ", v_lCreditCardChargePartyCnt:=0" &
                                                       ", v_cCreditCardChargeAmount:=0" &
                                                       ", v_lBrokerFeePartyCnt:=" & CStr(lBrokerFeePartyCnt) &
                                                       ", v_cBrokerFeeAmount:=" & CStr(crBrokerFeeAmount) &
                                                       ACApp, ACClass, "MTATransactAfter_SFU")


                m_lReturn = oSBOLink.AddPartyFees(v_lInsuranceFileCnt:=lInsuranceFileCnt, v_lLegalPartyCnt:=0, v_cLegalAmount:=0, v_cLegalCommissionAmount:=0, v_lBreakdownPartyCnt:=0, v_cBreakdownAmount:=0, v_cBreakdownCommissionAmount:=0, v_lPromptPartyCnt:=0, v_cPromptAmount:=0, v_cPromptCommission:=0, v_lCreditCardChargePartyCnt:=0, v_cCreditCardChargeAmount:=0, v_lBrokerFeePartyCnt:=lBrokerFeePartyCnt, v_cBrokerFeeAmount:=crBrokerFeeAmount)

                ' Debug.
                GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="After AddPartyFees Returned " & m_lReturn, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFU")

                ' Check result
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                ' Debug.
                GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Skipped AddPartyFees. No Broker Fee to post.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFU")
            End If
            ' PF031001 - Add party fees functionality - END


            oSBOLink.Dispose()
            oSBOLink = Nothing


            ' PWF 08/11/2001 - Not need in SFU?
            '    ' -----------------------------------------------------------------------------------
            '    ' PF091001 - Autoposting MTA amount
            '    '          - On hold, MTA posts very strange amount to accounts, behind scenes needs
            '    '            looking at to determine cause!
            '    ' -----------------------------------------------------------------------------------
            '    #If Not BypassAutoPosting Then
            '        m_lReturn = PostAccounts(v_lInsuranceFileCnt:=tosafeinteger(lInsuranceFileCnt), v_blnIsMTA:=True)
            '        If (m_lReturn <> PMTrue) Then
            '            Set oSBOLink = Nothing
            '            Set m_oDataSet = Nothing
            '
            '            MTATransactAfter_SFU = PMFalse
            '            Exit Function
            '        End If
            '    #End If


            ' ******************************************************************
            ' PWF 06/11/2001 - SFU Posting
            ' ******************************************************************
            ' Store useful dataset values to variables
            lRiskCnt = CInt(m_oDataSet.Risk.Item(ACDataModelCodePrefix & "POLICY").Item("NP_RISK_CNT").Value)

            lSchemeID = CInt(v_vSchemeArray(GISSharedConstants.GISQEMSchID, 0))

            ' Get rating section type id
            sQuery = "SELECT rating_section_type_id " &
                     "FROM DMC_Scheme_Rating_Section_Type " &
                     "WHERE gis_scheme_id = " & CStr(lSchemeID)

            ' Send the query to the log file
            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogDebug1, sMsg:="Retrieving rating_section_type_id" & Strings.ChrW(13) & Strings.ChrW(10) & "   " & sQuery, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFU")

            ' Execute the query
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sQuery, sSQLName:="SelectDMCSchemeRatingSectionType", bStoredProcedure:=False, vResultArray:=vArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "DMC_Scheme_Rating_Section_Type Select Failed", ACApp, ACClass, "MTATransactAfter_SFU")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the results array
            If Not Informations.IsArray(vArray) Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Unable to retrieve rating_section_type_id from DMC_Scheme_Rating_Section_Type", ACApp, ACClass, "MTATransactAfter_SFU")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Dim dbNumericTemp5 As Double
            If Not Double.TryParse(CStr(vArray(0, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Invalid datatype retrieving rating_section_type_id from DMC_Scheme_Rating_Section_Type", ACApp, ACClass, "MTATransactAfter_SFU")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Store the rating section type id

            lRSTypeID = CInt(vArray(0, 0))


            ' Add perils

            ' Get the business object
            m_lReturn = CreateNewBusinessObject(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, r_oObject:=oBusiness, v_sClassName:="bSirPerilAllocation.Business", v_oDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Unable to initialise bSirPerilAllocation.Business", ACApp, ACClass, "MTATransactAfter_SFU")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            oBusiness.InsuranceFileCnt = lInsuranceFileCnt

            oBusiness.RiskID = lRiskCnt

            ' Call the function
            ' RAG 2001-11-26 - pass through net premium instead of gross

            'm_lReturn = oBusiness.AddSectionAndPerils(v_lRatingSectionTypeId:=tosafeinteger(lRSTypeID), _
            'v_lPolicySectionTypeId:=-1, _
            'v_cAnnualPremium:=tosafedecimal(cThisPremium), _
            'v_cThisPremium:=tosafedecimal(cThisPremium), _
            'v_cAnnualRate:=tosafedecimal(cThisPremium), _
            'v_cSumInsured:=1, _
            'v_lRatetypeId:=2, _
            'v_lOriginalFlag:=0)


            m_lReturn = oBusiness.AddSectionAndPerils(v_lRatingSectionTypeId:=ToSafeInteger(lRSTypeID), v_lPolicySectionTypeId:=-1, v_cAnnualPremium:=ToSafeDecimal(cAnnualNet),
                                                      v_cThisPremium:=ToSafeDecimal(cNetPremium), v_cAnnualRate:=ToSafeDecimal(cAnnualNet), v_cSumInsured:=1, v_lRatetypeId:=2, v_lOriginalFlag:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Error In oBusiness.AddSectionAndPerils", ACApp, ACClass, "MTATransactAfter_SFU")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Release the object

            oBusiness.Dispose()
            oBusiness = Nothing


            m_lReturn = bPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, v_vBranch:=m_iSourceID, v_sCallingAppName:=ACApp, r_vUnderwriting:=m_vIsRI2007, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Unable to get Product Options", ACApp, ACClass, "MTATransactAfter_SFU")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Calculate reinsurance

            If m_vIsRI2007 = "1" Then
                ' Get the business object
                m_lReturn = CreateNewBusinessObject(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, r_oObject:=oBusiness, v_sClassName:="bSIRReinsuranceRI2007.Form", v_oDatabase:=m_oDatabase)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Unable to initialise bSIRReinsuranceRI2007.Form", ACApp, ACClass, "MTATransactAfter_SFU")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else
                ' Get the business object
                m_lReturn = CreateNewBusinessObject(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, r_oObject:=oBusiness, v_sClassName:="bSIRReinsurance.Form", v_oDatabase:=m_oDatabase)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Unable to initialise bSIRReinsurance.Form", ACApp, ACClass, "MTATransactAfter_SFU")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Set parameters and call the function

            oBusiness.InsuranceFileCnt = lInsuranceFileCnt

            oBusiness.RiskID = lRiskCnt


            m_lReturn = oBusiness.CalculateRI()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Error In oBusiness.CalculateRI", ACApp, ACClass, "MTATransactAfter_SFU")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Release the object

            oBusiness.Dispose()
            oBusiness = Nothing

            ' Calculate Reinsurance tax

            ' Get the business object
            m_lReturn = CreateNewBusinessObject(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, r_oObject:=oBusiness, v_sClassName:="bSIRRITax.Business", v_oDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Unable to initialise bSIRRITax.Business", ACApp, ACClass, "MTATransactAfter_SFU")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the function

            m_lReturn = oBusiness.ApplyTaxes(v_lInsFileCnt:=ToSafeInteger(lInsuranceFileCnt), v_lRiskCnt:=ToSafeInteger(lRiskCnt), r_bApplyTaxes:=bApplyTaxes)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Error In oBusiness.ApplyTaxes", ACApp, ACClass, "MTATransactAfter_SFU")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If bApplyTaxes Then
                If lRiskCnt <> 0 Then

                    oBusiness.RiskCnt = lRiskCnt


                    m_lReturn = oBusiness.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)


                    m_lReturn = oBusiness.GetRiskTax(r_vRiskTax:=vRiskTax, r_sDescription:=sDescription, iTask:=gPMConstants.PMEComponentAction.PMAdd)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Error In oBusiness.GetRiskTax", ACApp, ACClass, "MTATransactAfter_SFU")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Else

                    oBusiness.InsuranceFileCnt = lInsuranceFileCnt

                    m_lReturn = oBusiness.GetInsuranceFileTax(r_vInsuranceFileTax:=vRiskTax, r_sDescription:=sDescription, iTask:=gPMConstants.PMEComponentAction.PMAdd)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Error In oBusiness.GetInsuranceFileTax", ACApp, ACClass, "MTATransactAfter_SFU")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            ' Release the object

            oBusiness.Dispose()
            oBusiness = Nothing

            ' Change policy status

            ' Get the business object
            m_lReturn = CreateNewBusinessObject(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, r_oObject:=oBusiness, v_sClassName:="bSIRChangePolicyStatus.Business", v_oDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Unable to initialise bSIRChangePolicyStatus.Business", ACApp, ACClass, "MTATransactAfter_SFU")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the function

            m_lReturn = oBusiness.ChangePolicyStatus(v_lInsuranceFileCnt:=ToSafeInteger(lInsuranceFileCnt))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Error In oBusiness.ChangePolicyStatus", ACApp, ACClass, "MTATransactAfter_SFU")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Release the object

            oBusiness.Dispose()
            oBusiness = Nothing

            ' ControlTrans Automated !?!

            ' Get the business object
            m_lReturn = CreateNewBusinessObject(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, r_oObject:=oBusiness, v_sClassName:="bControlTrans.Automated", v_oDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Unable to initialise bControlTrans.Automated", ACApp, ACClass, "MTATransactAfter_SFU")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the function

            oBusiness.InsuranceFileCnt = lInsuranceFileCnt

            ' RAG 06/12/2001 - Set the transaction type

            m_lReturn = oBusiness.SetProcessModes(vTransactionType:="MTA")


            m_lReturn = oBusiness.Start

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Error In oBusiness.Start", ACApp, ACClass, "MTATransactAfter_SFU")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Release the object

            oBusiness.Dispose()
            oBusiness = Nothing
            ' ******************************************************************
            ' PWF 06/11/2001 - End of SFU Posting
            ' ******************************************************************

            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Finished", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFU")

            m_oDataSet = Nothing

            Return result

        Catch excep As System.Exception


            m_oDataSet = Nothing

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MTATransactAfter_SFU Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFU", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: MTATransactAfter_SFB
    '
    ' Description: This method is called by the GIS after an MTA
    '              Quote is Transacted
    '
    ' ***************************************************************** '
    Public Function MTATransactAfter_SFB(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_oDataset As cGISDataSetControl.Application, ByVal v_lQuoteType As Integer, ByVal v_vSchemeArray(,) As Object, ByRef r_vAdditionalDataArray(,) As Object) As Integer
        Dim result As Integer = 0
        Dim sIsCustEmailRequired As String = ""
        Dim lInsurerNo As Integer

        'Dim lPartyCnt As Long
        Dim lInsuranceFileCnt As Integer
        Dim sMake, sModel, sMakeDescription, sModelDescription, sPolNo As String
        'Modified by Vijay Pal on 5/25/2010 1:35:16 PM todolist,iteration3,declare Dim oSBOLink As Object 
        'Dim oSBOLink As bSIRIUSLink.SIRIUSLink
        Dim oSBOLink As bSIRIUSLink.SIRIUSLink
        Dim sSubject, sFrom As String

        Dim lGISPolicyLinkID As gPMConstants.PMEReturnCode
        Dim bSOFStepRequired, bBorderauxStepReq As Boolean
        Dim sInsurerNo, sInsurerDesc, sSchDesc, sBrandCode As String

        ' PF041001
        Dim lBrokerFeePartyCnt As Integer
        Dim crBrokerFeeAmount As Decimal
        Dim sGetSetting As String = ""

        'RJG 121001 - MTA premiums to be passed to SBO
        Dim cThisPremium, cNetPremium As Decimal
        Dim sCommPerc As String = ""
        Dim dCommissionPerc As Double
        Dim cCommissionAmount As Decimal

        Dim sMessage As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Inside MTATransactAfter_SFB...", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFB")

            m_oDataSet = r_oDataset

            ' Get the Policy LinkID
            lGISPolicyLinkID = m_oDataSet.PolicyLinkID()

            'CJB 050601 QMM Integration Change

            Dim dbNumericTemp As Double
            If Double.TryParse(CStr(v_vSchemeArray(GISSharedConstants.GISQEMSchAbi81Insurer, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                sInsurerNo = CStr(v_vSchemeArray(GISSharedConstants.GISQEMSchAbi81Insurer, 0))
            Else
                sInsurerNo = CStr(0)
            End If

            '***************************************************************************
            ' MTA Statement of Fact Email
            '****************************************************************************

            ' Update the Trans Type to say we are about to do a SOF
            m_lReturn = bGISTemp.UpdatePolicyLinkTransact(v_lPolicyLinkID:=lGISPolicyLinkID, v_dTransactDate:=DateTime.Now, v_sTransactType:=bGISTemp.GISMTATransTypeSOF, r_oDatabase:=m_oDatabase, r_bTransStepRequired:=bSOFStepRequired)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "UpdatePolicyLinkTransact (GISMTATransTypeSOF) Failed", ACApp, ACClass, "MTATransactAfter_SFB")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If we need to do the SOF (i.e. We have not done it before.)
            If bSOFStepRequired Then

                ' Check registry first to see if the SOF email is needed
                m_lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sSettingName:=GISSharedConstants.GISRegMTASOFEmailRequired, r_sSettingValue:=sIsCustEmailRequired, v_sBusinessTypeCode:=v_sGisBusinessTypeCode, v_sSubKey:=GISSharedConstants.GISRegSubKeyEmails)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Get Customer Email Reg Setting OK", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFB")
                ' RFC050800 - Do NOT generate SOF email for Cancellations
                If sIsCustEmailRequired = "1" And v_lQuoteType <> GISSharedConstants.GISMTAQuoteTypeAddCancellation Then

                    ' Check registry to get Subject
                    m_lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sSettingName:=GISSharedConstants.GISRegMTASOFEmailSubject, r_sSettingValue:=sSubject, v_sBusinessTypeCode:=v_sGisBusinessTypeCode, v_sSubKey:=GISSharedConstants.GISRegSubKeyEmails)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "GetRegSettingFromDataBusModel Failed : " & GISSharedConstants.GISRegMTASOFEmailSubject, ACApp, ACClass, "RegisterUser")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Check registry to get From
                    m_lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sSettingName:=GISSharedConstants.GISRegMTASOFEmailFrom, r_sSettingValue:=sFrom, v_sBusinessTypeCode:=v_sGisBusinessTypeCode, v_sSubKey:=GISSharedConstants.GISRegSubKeyEmails)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "GetRegSettingFromDataBusModel Failed : " & GISSharedConstants.GISRegMTASOFEmailFrom, ACApp, ACClass, "RegisterUser")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    sInsurerDesc = CStr(v_vSchemeArray(GISSharedConstants.GISQEMSchInsurerDesc, 0))

                    sSchDesc = CStr(v_vSchemeArray(GISSharedConstants.GISQEMSchDesc, 0))

                    sBrandCode = m_oDataSet.Risk.Item(ACDataModelCodePrefix & "POLICY").Item("NP_BRAND").Value


                    m_lReturn = GenerateCustomerEmail(v_sGisDataModelCode:=v_sGisDataModelCode, v_sSubject:=sSubject, v_sFrom:=sFrom, v_sInsurerNo:=sInsurerNo, v_sInsurerDesc:=sInsurerDesc, v_sSchemeName:=sSchDesc, v_sBrandCode:=sBrandCode, v_bMTA:=True)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateCustomerEmail Failed inside MTATransactAfter_SFB", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFB")

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

                GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Generate Customer Email OK", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFB")

            End If

            '***************************************************************************
            ' Generate the Borderaux
            '****************************************************************************

            ' Update the Trans Type to say we are about to do the Borderaux
            m_lReturn = bGISTemp.UpdatePolicyLinkTransact(v_lPolicyLinkID:=lGISPolicyLinkID, v_dTransactDate:=DateTime.Now, v_sTransactType:=bGISTemp.GISMTATransTypeBorderaux, r_oDatabase:=m_oDatabase, r_bTransStepRequired:=bBorderauxStepReq)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "UpdatePolicyLinkTransact (GISMTATransTypeBorderaux) Failed", ACApp, ACClass, "MTATransactAfter_SFB")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If we need to do the SOF (i.e. We have not done it before.)
            If bBorderauxStepReq Then

                m_lReturn = GenerateBordereaux(v_lInsurerNo:=lInsurerNo, v_lMTAQuoteType:=v_lQuoteType)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "GenerateBordereaux Failed", ACApp, ACClass, "MTATransactAfter_SFB")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            '***************************************************************************
            ' Make the MTA Live
            '****************************************************************************

            ' Update the Trans Type to say we are about to make the MTA Live
            m_lReturn = bGISTemp.UpdatePolicyLinkTransact(v_lPolicyLinkID:=lGISPolicyLinkID, v_dTransactDate:=DateTime.Now, v_sTransactType:=bGISTemp.GISMTATransTypeMakeLive, r_oDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "UpdatePolicyLinkTransact (GISMTATransTypeMakeLive) Failed", ACApp, ACClass, "MTATransactAfter_SFB")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lInsuranceFileCnt = CInt(m_oDataSet.Risk.Item(ACDataModelCodePrefix & "POLICY").Item("NP_INSURANCE_FILE_CNT").Value)

            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Got Ins File Cnt: " & lInsuranceFileCnt, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFB")

            sPolNo = m_oDataSet.Risk.Item(ACDataModelCodePrefix & "POLICY").Item("POLICY_NO").Value

            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Got Pol No: " & sPolNo, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFB")

            sMake = m_oDataSet.Risk.Item(ACDataModelCodePrefix & "VEHICLE").Item("MANUFACTURER").Value

            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Got Make: " & sMake, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFB")

            sModel = m_oDataSet.Risk.Item(ACDataModelCodePrefix & "VEHICLE").Item("MODEL").Value

            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Got Model: " & sModel, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFB")

            ' Get vehicle make and model descriptions
            m_lReturn = GetVehicleDescriptionFromABI(v_sGisDataModelCode, v_sGisBusinessTypeCode, sMake, sModel, sMakeDescription, sModelDescription)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "GetVehicleDescriptionFromABI Failed", ACApp, ACClass, "MTATransactAfter_SFB")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Got Make Description: " & sMakeDescription, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFB")

            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Got Model Description: " & sModelDescription, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFB")

            'Modified by Vijay Pal on 5/25/2010 1:37:59 PM todolist,iteration3,and declare oSBOLink = New Object 
            'oSBOLink = New bSIRIUSLink.SIRIUSLink()
            oSBOLink = New Object
            If oSBOLink Is Nothing Then

                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MTATransactAfter_SFB Failed: bSIRIUSLink.SIRIUSLink not created", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFB")

            End If

            m_lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "oSBOLink.Initialise Failed", ACApp, ACClass, "MTATransactAfter_SFB")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SBO Link Initialised", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFB")


            If v_lQuoteType = GISSharedConstants.GISMTAQuoteTypeAddCancellation Then

                ' Set policy to cancel  - CL140700

                m_lReturn = oSBOLink.CancelPolicy(v_lInsuranceFileCnt:=lInsuranceFileCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "oSBOLink.CancelPolicy Failed", ACApp, ACClass, "MTATransactAfter_SFB")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SBO CancelPolicy OK", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFB")

            Else
                ' Convert the Quote To a Policy
                ' Set to MTA

                'RJG 121001 - Get the premiums
                cThisPremium = CDec(m_oDataSet.Risk.Item("CALCULATED_RESULT", 1).Item("OVERRIDE_PREMIUM_INCL_IPT").Value)
                cNetPremium = cThisPremium - CDbl(m_oDataSet.Risk.Item("CALCULATED_RESULT", 1).Item("IPT_AMOUNT").Value)

                'CJB 250401 QMM Integration change (rqd as no comm perc set)

                If Not (Convert.IsDBNull(v_vSchemeArray(GISSharedConstants.GISQEMSchCommPerc, 0)) Or Informations.IsNothing(v_vSchemeArray(GISSharedConstants.GISQEMSchCommPerc, 0))) Then

                    sCommPerc = CStr(v_vSchemeArray(GISSharedConstants.GISQEMSchCommPerc, 0))
                Else
                    sCommPerc = CStr(0)
                End If

                dCommissionPerc = Val(sCommPerc)
                dCommissionPerc = (dCommissionPerc * 100) / (100 + dCommissionPerc)
                cCommissionAmount = cNetPremium * (dCommissionPerc / 100)

                ' ----------------------------------------------
                ' CJB290702 Start - O/P Extra info for debugging
                ' ----------------------------------------------
                sMessage = Strings.ChrW(13) & Strings.ChrW(10) & "----------------------------------------------------------------------" & Strings.ChrW(13) & Strings.ChrW(10)
                sMessage = sMessage & "About to call oSBOLink.Quote2Policy in MTATransactAfter_S4B with:" & Strings.ChrW(13) & Strings.ChrW(10)
                sMessage = sMessage & "v_lInsuranceFileCnt:" & CStr(lInsuranceFileCnt) & Strings.ChrW(13) & Strings.ChrW(10)
                sMessage = sMessage & "v_sInsuranceRef:" & sPolNo & Strings.ChrW(13) & Strings.ChrW(10)
                sMessage = sMessage & "v_sVehicleMakeModel:" & sMakeDescription & " " & sModelDescription & Strings.ChrW(13) & Strings.ChrW(10)
                sMessage = sMessage & "v_cThisPremium:" & CStr(cThisPremium) & Strings.ChrW(13) & Strings.ChrW(10)
                sMessage = sMessage & "v_cNetPremium:" & CStr(cNetPremium) & Strings.ChrW(13) & Strings.ChrW(10)
                sMessage = sMessage & "v_cCommissionAmount:" & CStr(cCommissionAmount) & Strings.ChrW(13) & Strings.ChrW(10)
                sMessage = sMessage & "v_dCommissionPerc:" & CStr(dCommissionPerc) & Strings.ChrW(13) & Strings.ChrW(10)
                sMessage = sMessage & "----------------------------------------------------------------------" & Strings.ChrW(13) & Strings.ChrW(10)

                GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_S4B")
                ' ----------------------------------------------
                ' CJB290702 End
                ' ----------------------------------------------

                m_lReturn = oSBOLink.MTAQuote2Policy(v_lInsuranceFileCnt:=lInsuranceFileCnt, v_sInsuranceRef:=sPolNo, v_sVehicleMakeModel:=sMakeDescription & " " & sModelDescription, v_cThisPremium:=cThisPremium, v_cNetPremium:=cNetPremium, v_cCommissionAmount:=cCommissionAmount, v_dCommissionPerc:=dCommissionPerc)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "oSBOLink.Quote2Policy Failed", ACApp, ACClass, "MTATransactAfter_SFB")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SBO Quote2Policy OK", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFB")

            End If


            ' PF031001 - Add party fees functionality - START
            ' Set default (and invalid) states
            lBrokerFeePartyCnt = 0
            crBrokerFeeAmount = 0

            ' Fees: Broker Fee / Admin Fee.
            m_lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sSettingName:=GISRegDMCBrokerFeePartyCnt, r_sSettingValue:=sGetSetting)
            ' Check for valid party_cnt
            Dim dbNumericTemp2 As Double
            If Double.TryParse(sGetSetting, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                lBrokerFeePartyCnt = CInt(sGetSetting)
            End If

            ' Only proceed if we have a party count
            If lBrokerFeePartyCnt > 0 Then
                sGetSetting = ""
                sGetSetting = m_oDataSet.Risk.Item("PAYMENT_BANK").Item("BROKER_FEE").Value

                ' Check for valid amount
                Dim dbNumericTemp3 As Double
                If Double.TryParse(sGetSetting, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                    crBrokerFeeAmount = CDec(sGetSetting)

                    ' A word of explanation: Trim to 2 decimal places.
                    crBrokerFeeAmount = CDec(StringsHelper.Format(crBrokerFeeAmount, "####0.00"))
                End If
            End If

            ' Decide if we have anything to post
            If crBrokerFeeAmount > 0 Then
                ' Debug.
                GISSharedConstants.DebugLogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Before Add Party Fees" &
                                                                                              "  v_lInsuranceFileCnt:=" &
                                                                                              CStr(lInsuranceFileCnt) &
                                                                                              ", v_lLegalPartyCnt:=0" &
                                                                                              ", v_cLegalAmount:=0" &
                                                                                              ", v_cLegalCommissionAmount:=0" &
                                                                                              ", v_lBreakdownPartyCnt:=0" &
                                                                                              ", v_cBreakdownAmount:=0" &
                                                                                              ", v_cBreakdownCommissionAmount:=0" &
                                                                                              ", v_lPromptPartyCnt:=0" &
                                                                                              ", v_cPromptAmount:=0" &
                                                                                              ", v_cPromptCommission:=0" &
                                                                                              ", v_lCreditCardChargePartyCnt:=0" &
                                                                                              ", v_cCreditCardChargeAmount:=0" &
                                                                                              ", v_lBrokerFeePartyCnt:=" &
                                                                                              CStr(lBrokerFeePartyCnt) &
                                                                                              ", v_cBrokerFeeAmount:=" &
                                                                                              CStr(crBrokerFeeAmount) &
                                                                                              ACApp, ACClass,
                                                       "MTATransactAfter_SFB")

                m_lReturn = oSBOLink.AddPartyFees(v_lInsuranceFileCnt:=lInsuranceFileCnt, v_lLegalPartyCnt:=0, v_cLegalAmount:=0, v_cLegalCommissionAmount:=0, v_lBreakdownPartyCnt:=0, v_cBreakdownAmount:=0, v_cBreakdownCommissionAmount:=0, v_lPromptPartyCnt:=0, v_cPromptAmount:=0, v_cPromptCommission:=0, v_lCreditCardChargePartyCnt:=0, v_cCreditCardChargeAmount:=0, v_lBrokerFeePartyCnt:=lBrokerFeePartyCnt, v_cBrokerFeeAmount:=crBrokerFeeAmount)
                ' Debug.
                GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="After AddPartyFees Returned " & m_lReturn, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFB")

                ' Check result
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                ' Debug.
                GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Skipped AddPartyFees. No Broker Fee to post.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFB")
            End If
            ' PF031001 - Add party fees functionality - END

            ' -----------------------------------------------------------------------------------
            ' PF091001 - Autoposting MTA amount
            '          - On hold, MTA posts very strange amount to accounts, behind scenes needs
            '            looking at to determine cause!
            ' -----------------------------------------------------------------------------------
#If Not BypassAutoPosting Then
            m_lReturn = PostAccounts(v_lInsuranceFileCnt:=lInsuranceFileCnt, v_blnIsMTA:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oSBOLink = Nothing
                m_oDataSet = Nothing

                Return gPMConstants.PMEReturnCode.PMFalse
            End If
#End If

            oSBOLink.Dispose()

            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SBO Terminate OK", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFB")

            oSBOLink = Nothing

            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Finished", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFB")

            m_oDataSet = Nothing

            Return result

        Catch excep As System.Exception


            m_oDataSet = Nothing

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MTATransactAfter_SFB Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactAfter_SFB", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: MTATransactBefore
    '
    ' Description: This method is called by the GIS when an MTA
    '              is transacted.
    '              Called AFTER GIS has done its stuff.
    ' ***************************************************************** '
    Public Function MTATransactBefore(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_oDataset As cGISDataSetControl.Application, ByVal v_lQuoteType As Integer, ByVal v_vSchemeArray(,) As Object, ByRef r_vAdditionalDataArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sPolNo As String = ""
        Dim bIsAssumedInfo As Boolean
        Dim sPolicyKey, sProposerKey, sNPQuoteKey, sPaymentAndBankKey, sNPPremiumFinanceKey As String
        Dim vNPPremiumFinanceKeyArray As Object
        Dim vPolicyKeyArray As Object
        Dim vProposerKeyArray As Object
        Dim vPaymentAndBankKeyArray As Object
        Dim vNPQuoteKeyArray As Object
        Dim dtCoverStartDate As Date
        Dim vCoverEndDate As Date
        Dim sEmail, sTelNoHome, sAddress_Line_1, sAddress_Line_2, sAddress_Line_3, sAddress_Line_4, sAddress_Post_Code, sCustomerFullName, sCustomerFirstName, sCustomerSurname, sCustomerTitle As String
        Dim iSchemeCount As Integer
        Dim lSchemeID As Integer
        Dim sSchemeID, sPolarisPremium As String
        Dim cPolarisPremium As Decimal
        Dim sPFDeposit As String = ""
        Dim cPFDeposit, cOutstandingAmt As Decimal
        Dim sPFTotal_Premium As String = ""
        Dim cPFTotal_Premium As Decimal
        Dim sPFArrangementFee As String = ""
        Dim cPFArrangementFee As Decimal
        Dim sPFNoOfInstalments As String = ""
        Dim iPFNoOfInstalments As Integer
        Dim sPFInterestRate As String = ""
        Dim cPFInterestRate As Decimal
        Dim sPremiumFinanceRef As String = ""
        Dim sStatusCode As String = ""
        Dim sTransNumber As String = ""
        Dim sLoanRenewalID As String = ""
        Dim sUserID As String = ""
        Dim sBrokerCode As String = ""
        Dim sTermID As String = ""
        Dim dtActualPaymentDate As Date
        Dim sNPPromptBankAccountName As String = ""
        Dim sNPPromptBankAccountNo As String = ""
        Dim sNPPromptBankSortCode As String = ""
        Dim sPaymentMethod As String = ""
        Dim dtCoverEndDate As Date
        Dim sInsurerABICode, sURL As String
        Dim boPromptEnableStatus As Boolean
        Dim sNPPremiumFinanceObj As String = ""
        Dim sPFAmountToFinance As String = ""
        Dim sXMLDataSet As String = ""
        Dim sXMLDataSetDef As String = ""
        Dim sPolicyObj As String = ""
        Dim sProposerObj As String = ""
        Dim sPaymentAndBankObj As String = ""
        Dim sNPQuoteObj As String = ""
        Dim bPFTransactStepRequired As Boolean
        Dim lGISPolicyLinkID As gPMConstants.PMEReturnCode
        Dim lInsurerNo, lSchemeNo As Integer
        Dim oGISPromptInterface As bGISPromptInterface.Application
        Dim oList As bGISListManager.InterfaceNoLogin
        Dim sBusinessStatus As String = ""
        Dim vChannel As Object
        Dim sNPPromptPremiumFinanceRef As String = ""

        Dim blCalledFromSTS As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            blCalledFromSTS = False

            For i As Integer = 0 To r_vAdditionalDataArray.GetUpperBound(1)
                Select Case r_vAdditionalDataArray(0, i)
                    Case CNCalledFromSTS

                        blCalledFromSTS = CBool(r_vAdditionalDataArray(1, i))
                        Exit For
                End Select
            Next i

            ' If this call hasn't arisen from the STS layer then skip out.
            If Not blCalledFromSTS Then
                Return result
            End If

            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Inside MTATransactBefore...", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore")

            ' Set Our Class Ref to the Dataset
            m_oDataSet = r_oDataset

            ' Get the Policy Link ID
            lGISPolicyLinkID = m_oDataSet.PolicyLinkID()

            ' Get the Insurer ABI Code, Insurer & Scheme No

            sInsurerABICode = CStr(v_vSchemeArray(GISSharedConstants.GISQEMSchAbi81Insurer, 0))

            lInsurerNo = CInt(v_vSchemeArray(GISSharedConstants.GISQEMSchPolarisInsurerNo, 0))

            lSchemeNo = CInt(v_vSchemeArray(GISSharedConstants.GISQEMSchSchemeNo, 0))

            ' Set the Policy Object
            sPolicyObj = ACDataModelCodePrefix & "Policy"

            ' Get the Policy Object Key

            m_lReturn = m_oDataSet.GetAllOIKey(v_sObjectName:=sPolicyObj, r_vOIKeyArray:=vPolicyKeyArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetAllOIKey Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            ' There should only be ONE Policy Key


            sPolicyKey = CStr(vPolicyKeyArray(vPolicyKeyArray.GetLowerBound(0)))

            ' Get the Cover Start Date
            dtCoverStartDate = CDate(m_oDataSet.Risk.Item(ACDataModelCodePrefix & "POLICY").Item("Effective_Start_Date").Value)

            '- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            'CJB 050101 Start
            'If Method of payment is DD (Direct Debit)...
            '   Call the bGISPromptInterface component to send an xml message to
            '   Prompt with the information required to allow them to setup the
            '   finance account against the given (and prevalidated) bank account details.
            '- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            'Update the Trans Type to say we are about to do a Premium Finance Transact (for MTA),
            'check the status that is returned to determine whether to do this or not (as it may
            'have been done already)
            m_lReturn = bGISTemp.UpdatePolicyLinkTransact(v_lPolicyLinkID:=lGISPolicyLinkID, v_dTransactDate:=DateTime.Now, v_sTransactType:=bGISTemp.GISMTATransTypePremiumFinanceTransact, r_oDatabase:=m_oDatabase, r_bTransStepRequired:=bPFTransactStepRequired)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the Policy Link Transact Status to GISMTATransTypePremiumFinanceTransact", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            ' If we need to do the Premium Finance Transact (i.e. We have not done it before.)
            If Not bPFTransactStepRequired Then
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Prompt Premium Finance Transact (for MTA) has been skipped as the Policy Link Transact Status indicates it has taken place already.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Else

                'Check if the Prompt URL held in the registry is blank in which case you'd get an error if you
                'proceeded and also as at 7/11/00 RC told me to treat as meaning Prompt was disabled and
                'to continue without doing any Prompt processing...

                m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=GISSharedConstants.GISRegPromptInterfaceURL, r_sSettingValue:=sURL, v_sSubKey:=GISSharedConstants.GISRegSubKey)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

                If sURL.Trim() = "" Then

                    boPromptEnableStatus = False

                    GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="In MTATransactBefore, after GetPMRegSetting -- PromptInterfaceURL is" & sURL, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Else
                    boPromptEnableStatus = True
                End If


                ' Get the Payment And Bank Object Key
                sPaymentAndBankObj = ACDataModelCodePrefix & "Payment_Bank"

                m_lReturn = m_oDataSet.GetAllOIKey(v_sObjectName:=sPaymentAndBankObj, r_vOIKeyArray:=vPaymentAndBankKeyArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn

                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetAllOIKey Failed for " & ACDataModelCodePrefix & "Payment_Bank", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                End If

                ' There should only be ONE Payment And Bank Key


                sPaymentAndBankKey = CStr(vPaymentAndBankKeyArray(vPaymentAndBankKeyArray.GetLowerBound(0)))

                ' Get the Payment And Bank / Insr Payment Method Type (Method of Payment)
                m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=sPaymentAndBankObj, v_sPropertyName:="Insr_Pmt_Method_Type", v_sOIKey:=sPaymentAndBankKey, r_vPropertyValue:=sPaymentMethod, r_bIsAssumedInfo:=bIsAssumedInfo)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn

                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetPropertyValue Payment And Bank / Insr_Payment_Method_Type", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                'Only carry out the Prompt processing if DD selected
                If sPaymentMethod.Trim() = GISPromptConstants.PromptPaymentMethodDirectDebit And boPromptEnableStatus Then

                    'First get some of the data required to pass to Prompt


                    'Need to check Channel to set broker code accordingly

                    m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=sPolicyObj, v_sPropertyName:="NP_Brand", v_sOIKey:=sPolicyKey, r_vPropertyValue:=CStr(vChannel), r_bIsAssumedInfo:=bIsAssumedInfo)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetPropertyValue Policy / NP_Brand Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If


                    Select Case CStr(vChannel).Trim()
                        Case GISSharedConstants.GISXelBrandCodeAutoBytel
                            sBrokerCode = GISSharedConstants.GISXelBrandNameAutoBytel

                        Case GISSharedConstants.GISXelBrandCodeFirste
                            sBrokerCode = GISSharedConstants.GISXelBrandNameFirste

                    End Select

                    'Set the Term ID depending on who the insurer is:
                    ':It corresponds at Prompt to the following payment terms:
                    '    30 days - All of Xelector's insurers at present (3/11/00) --> set to "X"
                    '    60 days - None of Xelector's insurers at present (3/11/00)--> set to "Y"

                    sTermID = "X"

                    'Select Case lInsurerNo
                    '
                    '    Case 6, 13, 51      '6=NU 13=Cornhill 51=LINK
                    '        sTermID = "X"   'this means 30 days
                    '
                    '    Case 21, 20, 15        '21=MMA 20=NIG 15=GroupAMA(GAN)
                    '        sTermID = "Y"   'this means 60 days
                    '
                    'End Select

                    ' Get the Policy / Effective End Date (note we retrieve into a variant as it may be null !)
                    m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=sPolicyObj, v_sPropertyName:="Effective_End_Date", v_sOIKey:=sPolicyKey, r_vPropertyValue:=vCoverEndDate, r_bIsAssumedInfo:=bIsAssumedInfo)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetPropertyValue Policy / Effective End Date Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                    'Process NULL (or invalid) end dates by not sending them to Prompt

                    If Not (Convert.IsDBNull(vCoverEndDate) Or Informations.IsNothing(vCoverEndDate)) Then
                        If Informations.IsDate(vCoverEndDate) Then
                            dtCoverEndDate = vCoverEndDate
                        End If
                    End If

                    'If No End Date found then to get one into Prompt we will add 1 year and take off 1 day from the Start Date
                    If dtCoverEndDate = CDate("00:00:00") Then
                        dtCoverEndDate = dtCoverStartDate.AddYears(1)
                        dtCoverEndDate = dtCoverEndDate.AddDays(-1)
                    End If

                    ' Get the Policy / NP_User_id (aka Gnet Client Code / Back Office Client Code)
                    m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=sPolicyObj, v_sPropertyName:="NP_user_id", v_sOIKey:=sPolicyKey, r_vPropertyValue:=sUserID, r_bIsAssumedInfo:=bIsAssumedInfo)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetPropertyValue Policy / NP_user_id", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                    ' Get the Policy / NP_Prompt_Bank_Account_Name
                    m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=sPolicyObj, v_sPropertyName:="NP_prompt_bank_account_name", v_sOIKey:=sPolicyKey, r_vPropertyValue:=sNPPromptBankAccountName, r_bIsAssumedInfo:=bIsAssumedInfo)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetPropertyValue Policy / NP_prompt_bank_account_name", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                    ' Get the Policy / NP_Prompt_Bank_Account_No
                    m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=sPolicyObj, v_sPropertyName:="NP_prompt_bank_account_number", v_sOIKey:=sPolicyKey, r_vPropertyValue:=sNPPromptBankAccountNo, r_bIsAssumedInfo:=bIsAssumedInfo)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetPropertyValue Policy / NP_prompt_bank_account_no", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                    ' Get the Policy / NP_Prompt_Bank_Sort_Code
                    m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=sPolicyObj, v_sPropertyName:="NP_prompt_bank_sort_code", v_sOIKey:=sPolicyKey, r_vPropertyValue:=sNPPromptBankSortCode, r_bIsAssumedInfo:=bIsAssumedInfo)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetPropertyValue Policy / NP_prompt_bank_sort_code", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                    ' Get the Policy / NP_Prompt_Premium_Finance_Ref
                    m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=sPolicyObj, v_sPropertyName:="NP_Prompt_Premium_Finance_Ref", v_sOIKey:=sPolicyKey, r_vPropertyValue:=sNPPromptPremiumFinanceRef, r_bIsAssumedInfo:=bIsAssumedInfo)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetPropertyValue Policy / NP_Prompt_Premium_Finance_Ref", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                    'Pass the premium finance ref in the gnet client code element for Prompt premium finance transact
                    'messages that are NOT of type NB.
                    sPolNo = sNPPromptPremiumFinanceRef

                    ' Get the Proposer Object Key
                    sProposerObj = ACDataModelCodePrefix & "Proposer_Policyholder"

                    m_lReturn = m_oDataSet.GetAllOIKey(v_sObjectName:=sProposerObj, r_vOIKeyArray:=vProposerKeyArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetAllOIKey Failed for " & ACDataModelCodePrefix & "Proposer_Policyholder", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                        Return result
                    End If

                    ' There should only be ONE Proposer Key


                    sProposerKey = CStr(vProposerKeyArray(vProposerKeyArray.GetLowerBound(0)))

                    ' Get the Proposer / Email
                    m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=sProposerObj, v_sPropertyName:="Email", v_sOIKey:=sProposerKey, r_vPropertyValue:=sEmail, r_bIsAssumedInfo:=bIsAssumedInfo)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetPropertyValue Proposer / Email", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                    ' Get the Proposer / Tel No Home
                    m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=sProposerObj, v_sPropertyName:="Tel_No_Home", v_sOIKey:=sProposerKey, r_vPropertyValue:=sTelNoHome, r_bIsAssumedInfo:=bIsAssumedInfo)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetPropertyValue Proposer / Tel_No_Home", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                    ' Get the Proposer / Address Line 1
                    m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=sProposerObj, v_sPropertyName:="Address_Line_1", v_sOIKey:=sProposerKey, r_vPropertyValue:=sAddress_Line_1, r_bIsAssumedInfo:=bIsAssumedInfo)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetPropertyValue Proposer / Address_Line_1", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                    ' Get the Proposer / Address Line 2
                    m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=sProposerObj, v_sPropertyName:="Address_Line_2", v_sOIKey:=sProposerKey, r_vPropertyValue:=sAddress_Line_2, r_bIsAssumedInfo:=bIsAssumedInfo)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetPropertyValue Proposer / Address_Line_2", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                    ' Get the Proposer / Address Line 3
                    m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=sProposerObj, v_sPropertyName:="Address_Line_3", v_sOIKey:=sProposerKey, r_vPropertyValue:=sAddress_Line_3, r_bIsAssumedInfo:=bIsAssumedInfo)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetPropertyValue Proposer / Address_Line_3", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                    ' Get the Proposer / Address Line 4
                    m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=sProposerObj, v_sPropertyName:="Address_Line_4", v_sOIKey:=sProposerKey, r_vPropertyValue:=sAddress_Line_4, r_bIsAssumedInfo:=bIsAssumedInfo)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetPropertyValue Proposer / Address_Line_4", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                    ' Get the Proposer / Address Post Code
                    m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=sProposerObj, v_sPropertyName:="Address_Post_Code", v_sOIKey:=sProposerKey, r_vPropertyValue:=sAddress_Post_Code, r_bIsAssumedInfo:=bIsAssumedInfo)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetPropertyValue Proposer / Address_Post_Code", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                    ' Get the Proposer / Forename_Initial_1
                    m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=sProposerObj, v_sPropertyName:="Forename_Initial_1", v_sOIKey:=sProposerKey, r_vPropertyValue:=sCustomerFirstName, r_bIsAssumedInfo:=bIsAssumedInfo)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetPropertyValue Proposer / Forename_Initial_1", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                    ' Get the Proposer / Surname
                    m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=sProposerObj, v_sPropertyName:="Surname", v_sOIKey:=sProposerKey, r_vPropertyValue:=sCustomerSurname, r_bIsAssumedInfo:=bIsAssumedInfo)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetPropertyValue Proposer / Surname", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                    sCustomerFullName = sCustomerFirstName & " " & sCustomerSurname

                    ' Get the Proposer / Title Code
                    m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=sProposerObj, v_sPropertyName:="Title_Code", v_sOIKey:=sProposerKey, r_vPropertyValue:=sCustomerTitle, r_bIsAssumedInfo:=bIsAssumedInfo)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetPropertyValue Proposer / Title", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                    'Convert the Customer Title ABI code just returned into its proper text
                    oList = New bGISListManager.InterfaceNoLogin()
                    If oList Is Nothing Then
                        result = gPMConstants.PMEReturnCode.PMFalse

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Set oList = New iGISListManager.InterfaceNoLogin - oList is Nothing", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                    m_lReturn = oList.Initialise()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_lReturn = oList.Initialise() - m_lReturn is:" & m_lReturn, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result

                    End If

                    m_lReturn = oList.CheckListVersions(ACDataModelCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_lReturn = oList.CheckListVersions('" & ACDataModelCode & "', '" & ACDataModelCode & "') - m_lReturn is:" & CStr(m_lReturn), vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                    m_lReturn = oList.GetDescription("131085", sCustomerTitle, sCustomerTitle)

                    ' Get the NP_Quote Object Key into an array
                    sNPQuoteObj = ACDataModelCodePrefix & "NP_Quote"

                    m_lReturn = m_oDataSet.GetAllOIKey(v_sObjectName:=sNPQuoteObj, r_vOIKeyArray:=vNPQuoteKeyArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetAllOIKey Failed for " & ACDataModelCodePrefix & "NP_Quote", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                        Return result
                    End If

                    'Loop through the Quote Output instances to find the selected insurer instance so we
                    'can extract info. from it to send to Prompt

                    'We know the selected scheme id - it'll be used in the loop to compare so we can find an instance match

                    lSchemeID = CInt(v_vSchemeArray(GISSharedConstants.GISQEMSchID, 0))

                    'Log a debug message
                    GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Inside MTATransactBefore: lSchemeID is:" & lSchemeID, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore")
                    If Informations.IsArray(vNPQuoteKeyArray) Then

                        iSchemeCount = vNPQuoteKeyArray.GetUpperBound(0)

                        GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Inside MTATransactBefore: iSchemeCount is:" & iSchemeCount, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore")
                        iSchemeCount += 1 'Add 1 as array is zero bound

                        For iLoopCounter As Integer = 1 To iSchemeCount

                            'Note key array is zero based

                            sNPQuoteKey = CStr(vNPQuoteKeyArray(iLoopCounter - 1))

                            'Log a debug message
                            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Inside MTATransactBefore: sNPQuoteKey is:" & sNPQuoteKey & " for #" & CStr(iLoopCounter) & " instance of quote o/p", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore")


                            ' Get the NP_Quote / Scheme ID for the current instance
                            m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=sNPQuoteObj, v_sPropertyName:="Scheme_id", v_sOIKey:=sNPQuoteKey, r_vPropertyValue:=sSchemeID, r_bIsAssumedInfo:=bIsAssumedInfo)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = m_lReturn

                                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetPropertyValue NP_Quote / Scheme_ID", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                                Return result
                            End If

                            'Check if we've found the instance of the quote output that was selected
                            If Val(sSchemeID) = lSchemeID Then

                                'CJB 151100 Xel gets the Premium Finance Rating Engine O/P values from a new object - NP_Premium_Finance
                                'unlike i4m which gets them from the O/P object - this was not possible in Xel as the Rating Engine is
                                'not able to be called in the NBQuoteAfter event and dataset populated there (as of dynamic quote page) so
                                'the rating engine is called later on explicity and these new object properties set in the web pages...
                                'Note we couldn't set them in the GIS as the Financial class had no dataset in it !

                                ' Set the Premium Finance Object
                                sNPPremiumFinanceObj = ACDataModelCodePrefix & "NP_Premium_Finance"

                                ' Get the Premium Finance Object Key

                                m_lReturn = m_oDataSet.GetAllOIKey(v_sObjectName:=sNPPremiumFinanceObj, r_vOIKeyArray:=vNPPremiumFinanceKeyArray)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = m_lReturn

                                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetAllOIKey Failed for " & ACDataModelCodePrefix & "NP_Premium_Finance", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                                    Return result
                                End If

                                ' There should only be ONE Premium Finance Key


                                sNPPremiumFinanceKey = CStr(vNPPremiumFinanceKeyArray(vNPPremiumFinanceKeyArray.GetLowerBound(0)))

                                'Get the Premium Finance Arrangement Fee from the selected instance of quote output
                                m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=sNPPremiumFinanceObj, v_sPropertyName:="PF_Arrangement_Fee", v_sOIKey:=sNPPremiumFinanceKey, r_vPropertyValue:=sPFArrangementFee, r_bIsAssumedInfo:=bIsAssumedInfo)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = m_lReturn

                                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetPropertyValue NP_Premium_Finance / PF_Arrangement_Fee", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                                    Return result
                                End If

                                'Store the value returned in a currency field
                                Dim dbNumericTemp As Double
                                If Double.TryParse(sPFArrangementFee, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                                    cPFArrangementFee = CDec(sPFArrangementFee)
                                Else
                                    cPFArrangementFee = 0
                                End If

                                'Get the Premium Finance Interest Rate from the selected instance of quote output
                                m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=sNPPremiumFinanceObj, v_sPropertyName:="PF_Interest_Rate", v_sOIKey:=sNPPremiumFinanceKey, r_vPropertyValue:=sPFInterestRate, r_bIsAssumedInfo:=bIsAssumedInfo)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = m_lReturn

                                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetPropertyValue NP_Premium_Finance / PF_Interest_Rate", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                                    Return result
                                End If

                                'Store the value returned in a currency field
                                Dim dbNumericTemp2 As Double
                                If Double.TryParse(sPFInterestRate, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                                    cPFInterestRate = CDec(sPFInterestRate)
                                Else
                                    cPFInterestRate = 0
                                End If

                                'Get the Premium Finance No. of Instalments from the selected instance of quote output
                                m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=sNPPremiumFinanceObj, v_sPropertyName:="PF_No_of_instalments", v_sOIKey:=sNPPremiumFinanceKey, r_vPropertyValue:=sPFNoOfInstalments, r_bIsAssumedInfo:=bIsAssumedInfo)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = m_lReturn

                                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetPropertyValue NP_Premium_Finance / PF_No_of_instalments", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                                    Return result
                                End If

                                'Store the value returned in a currency field
                                Dim dbNumericTemp3 As Double
                                If Double.TryParse(sPFNoOfInstalments, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                                    iPFNoOfInstalments = CInt(sPFNoOfInstalments)
                                Else
                                    iPFNoOfInstalments = 0
                                End If

                                'Get the Premium Finance deposit from the selected instance of quote output
                                m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=sNPPremiumFinanceObj, v_sPropertyName:="PF_Deposit", v_sOIKey:=sNPPremiumFinanceKey, r_vPropertyValue:=sPFDeposit, r_bIsAssumedInfo:=bIsAssumedInfo)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = m_lReturn

                                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetPropertyValue NP_Premium_Finance / PF_Deposit", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                                    Return result
                                End If

                                'Store the value returned in a currency field
                                Dim dbNumericTemp4 As Double
                                If Double.TryParse(sPFDeposit, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                                    cPFDeposit = CDec(sPFDeposit)
                                Else
                                    cPFDeposit = 0
                                End If

                                'Get the Premium Finance total premium amount from the selected instance of quote output
                                m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=sNPPremiumFinanceObj, v_sPropertyName:="PF_Total_Premium", v_sOIKey:=sNPPremiumFinanceKey, r_vPropertyValue:=sPFTotal_Premium, r_bIsAssumedInfo:=bIsAssumedInfo)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = m_lReturn

                                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetPropertyValue NP_Premium_Finance / PF_Total_Premium", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                                    Return result
                                End If

                                'Store the value returned in a currency field
                                Dim dbNumericTemp5 As Double
                                If Double.TryParse(sPFTotal_Premium, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                                    cPFTotal_Premium = CDec(sPFTotal_Premium)
                                Else
                                    cPFTotal_Premium = 0
                                End If

                                'Calculate the Outstanding Amt
                                cOutstandingAmt = cPFTotal_Premium - cPFDeposit


                                'Note that Prompt require an 'Insurer Premium' value which they consider to
                                'be Polaris Premium + Add-Ons (although the insurer should obviosuly not get
                                'paid for these) but not incl. Admin Fees or interest on the loan.

                                'Get the Premium Finance Amount to Finance
                                m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=sNPPremiumFinanceObj, v_sPropertyName:="PF_Amount_To_Finance", v_sOIKey:=sNPPremiumFinanceKey, r_vPropertyValue:=sPFAmountToFinance, r_bIsAssumedInfo:=bIsAssumedInfo)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = m_lReturn

                                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetPropertyValue NP_Premium_Finance / PF_Amount_To_Finance", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                                    Return result
                                End If

                                'Store the value returned in a currency field
                                Dim dbNumericTemp6 As Double
                                If Double.TryParse(sPFAmountToFinance, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6) Then
                                    cPolarisPremium = CDec(sPFAmountToFinance)
                                Else
                                    cPolarisPremium = 0
                                End If

                                Exit For
                            End If

                        Next

                    Else

                        'Not an array !
                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Inside MTATransactBefore: vNPQuoteKeyArray is not an array!", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore")


                    End If

                    'Check type - whether cancellation (mtc) or mta etc
                    'Note that this doesn't currently cater for renewals
                    'as no code has been created for them yet
                    If v_lQuoteType = GISSharedConstants.GISMTAQuoteTypeAddCancellation Then
                        sBusinessStatus = GISPromptConstants.PromptBusStatMidTermCancellation
                    Else
                        sBusinessStatus = GISPromptConstants.PromptBusStatMidTermAdjustment
                    End If

                    'Log a debug message that oGISPromptInterface.PremiumFinanceTransact is about to be called
                    GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="About to call:oGISPromptInterface.PremiumFinanceTransact with v_sSenderID=" & GISPromptConstants.PromptSenderIDXelector &
                                                           ", v_sCoverType=" & GISPromptConstants.PromptCoverTypeMotor & ", v_sGnetClientCode=" & sUserID & ", v_sBusinessStatus=" & sBusinessStatus &
                                                           ", v_cTotalPremiumAmount=" & CStr(cPFTotal_Premium) & ", v_cOutstandingAmount=" & CStr(cOutstandingAmt) & ", v_sPaymentMethod=" & GISPromptConstants.PromptPaymentMethodDirectDebit &
                                                           ", v_cInsurerPremium=" & CStr(cPolarisPremium) & ", v_cSenderCharge=" & CStr(0) & ", v_cPMCharge=" & CStr(0) &
                                                           ", v_cPromptAdminCharge=" & CStr(cPFArrangementFee) & ", v_sTitle=" & sCustomerTitle & ", v_sCustomerName=" & sCustomerFullName & ", v_sAddressLine1=" & sAddress_Line_1 & ", v_sAddressLine2=" & sAddress_Line_2 &
                                                           ", v_sAddressLine3=" & sAddress_Line_3 & ", v_sAddressLine4=" & sAddress_Line_4 & ", v_sPostcode=" & sAddress_Post_Code &
                                                           ", v_sTelephoneNo=" & sTelNoHome & ", v_sEmailAddress=" & sEmail & ", v_sGnetPolicyCode=" & sPolNo &
                                                           ", v_sInsurerCode=" & sInsurerABICode & ", v_sSchemeCode=" & CStr(lSchemeNo) & ", v_sBrokerCode=" & sBrokerCode &
                                                           ", v_sTermID=" & sTermID & ", v_dtInceptionDate=" & dtCoverStartDate.ToString() & ", v_dtExpiryDate=" & vCoverEndDate.ToString() &
                                                           ", v_vDepositAmount=" & CStr(cPFDeposit) & ", v_lInstalmentNo=" & CStr(iPFNoOfInstalments) & ", v_vInterestRate=" & CStr(cPFInterestRate) &
                                                           ", v_sBankAccountName=" & sNPPromptBankAccountName & ", v_sBankAccountNo=" & sNPPromptBankAccountNo & ", v_sBankSortCode=" & sNPPromptBankSortCode & ", v_sDataModelCode:=" & v_sGisDataModelCode & ", v_sBusinessTypeCode:=" & v_sGisBusinessTypeCode, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)


                    'Create reference to the GIS Prompt Interface component
                    oGISPromptInterface = New bGISPromptInterface.Application()

                    'Call the GIS Prompt Interface Premium Finance Transact Method
                    m_lReturn = oGISPromptInterface.PremiumFinanceTransact(v_sSenderID:=GISPromptConstants.PromptSenderIDXelector, v_sCoverType:=GISPromptConstants.PromptCoverTypeMotor, v_sGnetClientCode:=sUserID, v_sBusinessStatus:=sBusinessStatus, v_cTotalPremiumAmount:=cPFTotal_Premium, v_cOutstandingAmount:=cOutstandingAmt, v_sPaymentMethod:=GISPromptConstants.PromptPaymentMethodDirectDebit, v_cInsurerPremium:=cPolarisPremium, v_cSenderCharge:=0, v_cPMCharge:=0, v_cPromptAdminCharge:=cPFArrangementFee, v_sTitle:=sCustomerTitle, v_sCustomerName:=sCustomerFullName, v_sAddressLine1:=sAddress_Line_1, v_sAddressLine2:=sAddress_Line_2, v_sAddressLine3:=sAddress_Line_3, v_sAddressLine4:=sAddress_Line_4, v_sPostcode:=sAddress_Post_Code, v_sTelephoneNo:=sTelNoHome, v_sEmailAddress:=sEmail, v_sGnetPolicyCode:=ToSafeString(sPolNo), v_sInsurerCode:=sInsurerABICode, v_sSchemeCode:=CStr(lSchemeNo), v_sBrokerCode:=sBrokerCode, v_sTermID:=sTermID, v_dtInceptionDate:=dtCoverStartDate, v_dtExpiryDate:=dtCoverEndDate, v_sDataModelCode:=v_sGisDataModelCode, v_sBusinessTypeCode:=v_sGisBusinessTypeCode, r_sStatusCode:=sStatusCode, r_sPremiumFinanceRef:=sPremiumFinanceRef, r_sTransNumber:=sTransNumber, r_sLoanRenewalID:=sLoanRenewalID, r_dtActualPaymentDate:=dtActualPaymentDate, v_vDepositAmount:=cPFDeposit, v_lInstalmentNo:=iPFNoOfInstalments, v_vInterestRate:=cPFInterestRate, v_sBankAccountName:=sNPPromptBankAccountName, v_sBankAccountNo:=sNPPromptBankAccountNo, v_sBankSortCode:=sNPPromptBankSortCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn


                        If Not (Convert.IsDBNull(sStatusCode) Or Informations.IsNothing(sStatusCode)) Then
                            If sStatusCode.Trim() <> "" Then

                                'Store the status (if any) returned from the msg received back from Prompt
                                m_lReturn = m_oDataSet.SetPropertyValue(v_sObjectName:=sPolicyObj, v_sPropertyName:="NP_prompt_status_code", v_sOIKey:=sPolicyKey, v_vPropertyValue:=sStatusCode, v_bIsAssumedInfo:=bIsAssumedInfo)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = m_lReturn

                                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.SetPropertyValue " & ACDataModelCodePrefix & "Policy / NP_prompt_status_code failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                                    Return result
                                End If
                            End If
                        End If

                        'Check the type of error from Prompt - if a bank a/c error or a handled error
                        'returned in an ERROR element of the XML response then show the status etc,
                        'else, just show error code...
                        'NOTE that the error codes, 10500 and 10501 are handled in the web pages and result in a
                        'specific bank a/c validation error being returned or a General Prompt error shown.
                        If m_lReturn = ToSafeDouble(GISPromptConstants.PromptBankAccountError) Or m_lReturn = ToSafeDouble(GISPromptConstants.PromptOtherError) Then
                            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oGISPromptInterface.PremiumFinanceTransact Returned with StatusCode=" & sStatusCode &
                                                              ", PremiumFinanceRef=" & sPremiumFinanceRef &
                                                              ", TransNumber=" & sTransNumber &
                                                              ", LoanRenewalID=" & sLoanRenewalID &
                                                              ", ActualPaymentDate=" & dtActualPaymentDate.ToString(), vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                        Else
                            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oGISPromptInterface.PremiumFinanceTransact Failed with lReturn=" & m_lReturn, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        End If
                        Return result

                    End If

                    'Log a debug message that oGISPromptInterface.PremiumFinanceTransact has returned
                    GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oGISPromptInterface.PremiumFinanceTransact Returned with StatusCode=" & sStatusCode &
                                                              ", PremiumFinanceRef=" & sPremiumFinanceRef &
                                                              ", TransNumber=" & sTransNumber &
                                                              ", LoanRenewalID=" & sLoanRenewalID &
                                                           ", ActualPaymentDate=" & dtActualPaymentDate.ToString(), vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    'Store the data items returned from the msg received back from Prompt
                    m_lReturn = m_oDataSet.SetPropertyValue(v_sObjectName:=sPolicyObj, v_sPropertyName:="NP_prompt_status_code", v_sOIKey:=sPolicyKey, v_vPropertyValue:=sStatusCode, v_bIsAssumedInfo:=bIsAssumedInfo)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.SetPropertyValue " & ACDataModelCodePrefix & "Policy / NP_prompt_status_code failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                    m_lReturn = m_oDataSet.SetPropertyValue(v_sObjectName:=sPolicyObj, v_sPropertyName:="NP_prompt_premium_finance_ref", v_sOIKey:=sPolicyKey, v_vPropertyValue:=sPremiumFinanceRef, v_bIsAssumedInfo:=bIsAssumedInfo)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.SetPropertyValue " & ACDataModelCodePrefix & "Policy / NP_prompt_premium_finance_ref failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                        Return result
                    End If

                    m_lReturn = m_oDataSet.SetPropertyValue(v_sObjectName:=sPolicyObj, v_sPropertyName:="NP_prompt_trans_number", v_sOIKey:=sPolicyKey, v_vPropertyValue:=sTransNumber, v_bIsAssumedInfo:=bIsAssumedInfo)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.SetPropertyValue " & ACDataModelCodePrefix & "Policy / NP_prompt_trans_number failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                        Return result
                    End If

                    m_lReturn = m_oDataSet.SetPropertyValue(v_sObjectName:=sPolicyObj, v_sPropertyName:="NP_prompt_loan_renewal_id", v_sOIKey:=sPolicyKey, v_vPropertyValue:=sLoanRenewalID, v_bIsAssumedInfo:=bIsAssumedInfo)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.SetPropertyValue " & ACDataModelCodePrefix & "Policy / NP_prompt_loan_renewal_id failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                        Return result
                    End If

                    m_lReturn = m_oDataSet.SetPropertyValue(v_sObjectName:=sPolicyObj, v_sPropertyName:="NP_prompt_actual_payment_date", v_sOIKey:=sPolicyKey, v_vPropertyValue:=dtActualPaymentDate.ToString(), v_bIsAssumedInfo:=bIsAssumedInfo)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.SetPropertyValue " & ACDataModelCodePrefix & "Policy / NP_prompt_actual_payment_date failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                        Return result
                    End If

                    'Log a debug message that just called bGISPromptInterface
                    GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Inside MTATransactBefore: After bGISPromptInterface.PremiumFinanceTransact", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore")

                    '- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                    'CJB 011100 End
                    '- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                End If
            End If

            ' Release our Ref to the Dataset
            m_oDataSet = Nothing
            oList = Nothing

            Return result

        Catch excep As System.Exception


            m_oDataSet = Nothing

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MTATransactBefore Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTATransactBefore", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    'Name:  MTAQuoteBefore
    '
    ' Description: This method is called by the GIS at the start
    '               of the MTAQuote Method
    '
    ' ***************************************************************** '
    Public Function MTAQuoteBefore(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lQuoteType As Integer, ByRef r_oDataset As cGISDataSetControl.Application, ByRef r_vAdditionalDataArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sIsTPAEmailRequired As String = ""

        Dim blCalledFromSTS As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            blCalledFromSTS = False

            For i As Integer = 0 To r_vAdditionalDataArray.GetUpperBound(1)
                Select Case r_vAdditionalDataArray(0, i)
                    Case CNCalledFromSTS

                        blCalledFromSTS = CBool(r_vAdditionalDataArray(1, i))
                        Exit For
                End Select
            Next i

            ' If this call hasn't arisen from the STS layer then skip out.
            If Not blCalledFromSTS Then
                Return result
            End If

            m_oDataSet = Nothing

            Return result

        Catch excep As System.Exception


            m_oDataSet = Nothing

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MTAQuoteBefore Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTAQuoteBefore", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: MTAQuoteAfter
    '
    ' Description: This method is called by the GIS when an MTA
    '              is Quoted.
    '              AFTER the GIS has done its stuff.
    '
    '
    ' ***************************************************************** '
    Public Function MTAQuoteAfter(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lQuoteType As Integer, ByRef r_oDataset As cGISDataSetControl.Application, ByRef r_vAdditionalDataArray(,) As Object) As Integer

        'Dim lPartyCnt As Long
        Dim result As Integer = 0
        Dim lInsuranceFileCnt As Integer
        Dim dtCoverStartDate, dtExpiryDate As Date
        Dim sMake, sModel, sQuoteRef As String

        Dim oSBOLink As Object
        Dim v As Object

        Dim blCalledFromSTS As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            blCalledFromSTS = False

            For i As Integer = 0 To r_vAdditionalDataArray.GetUpperBound(1)
                Select Case r_vAdditionalDataArray(0, i)
                    Case CNCalledFromSTS

                        blCalledFromSTS = CBool(r_vAdditionalDataArray(1, i))
                        Exit For
                End Select
            Next i

            ' If this call hasn't arisen from the STS layer then skip out.
            If Not blCalledFromSTS Then
                Return result
            End If

            Return result

        Catch excep As System.Exception


            m_oDataSet = Nothing

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MTAQuoteAfter Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="MTAQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    Private Function PostAccounts(ByVal v_lInsuranceFileCnt As Integer, Optional ByVal v_blnIsMTA As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim oSBOTransactions As Object
        Dim vTransKeyArray As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Log startup message
            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Beginning Autopost Cycle...", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBTransactAfter")

            ' Create and initialise the object
            oSBOTransactions = gPMFunctions.CreateLateBoundObject("iPMBTransactions.GNetNoLogon")
            Dim oDatabase As Object = m_oDatabase
            m_lReturn = oSBOTransactions.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeString(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(m_sCallingAppName), vDatabase:=oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", Internal, Failed to Initialise iPMBTransactions.InterfaceNoLogon.")
            End If
            m_oDatabase = oDatabase
            ' Generate and set keys array
            ReDim vTransKeyArray(1, 3)

            vTransKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameInsFileCnt

            vTransKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = v_lInsuranceFileCnt

            vTransKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameDebitCredit

            vTransKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = "D"

            vTransKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameFromGNet

            vTransKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = True

            vTransKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameFromIts4Me

            vTransKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = False


            m_lReturn = oSBOTransactions.SetKeys(vKeyArray:=vTransKeyArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", Internal, Failed to SetKeys for iPMBTransactions.NavigatorV3.")
            End If


            If Not Informations.IsNothing(v_blnIsMTA) Then

                If v_blnIsMTA Then
                    ' Set process modes
                    m_lReturn = oSBOTransactions.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vTransactionType:="Mid Term Adjustment")
                Else
                    ' Set process modes
                    m_lReturn = oSBOTransactions.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vTransactionType:="New Business")
                End If
            Else
                ' Set process modes
                m_lReturn = oSBOTransactions.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vTransactionType:="New Business")
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", Internal, Failed to SetProcessModes for iPMBTransactions.NavigatorV3.")
            End If

            ' Process the transaction
            m_lReturn = oSBOTransactions.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", Internal, Failed to Process SBO Post Transactions.")
            End If

            ' Terminate the object
            oSBOTransactions.Dispose()
            ' Log completion message
            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Completed Autopost Cycle", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBTransactAfter")

            Return result

        Catch excep As System.Exception


            ' Determine if this is error or internal exit
            If Informations.Err().Number = Constants.vbObjectError Then
                ' Return false and log warning
                result = gPMConstants.PMEReturnCode.PMFalse

                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=excep.Message, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="PostAccounts")
            Else
                ' Return error and log
                result = gPMConstants.PMEReturnCode.PMError

                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostAccounts Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="PostAccounts", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            End If

            ' Release transactions object, just in case
            ' Note: Terminate called by class event if active!

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: NBGenerateCustomerEmail
    '
    ' Description: Build and send a summary of policy sold
    '
    ' Date: CL020600
    '       RG200600    - missing data for Claims, Convictions, Drivers etc added
    '       CB120101    - added DD payment information when applicable
    '
    ' ***************************************************************** '
    Private Function GenerateCustomerEmail(ByVal v_sGisDataModelCode As String, ByVal v_sSubject As String, ByVal v_sFrom As String, ByVal v_sInsurerNo As String, ByVal v_sInsurerDesc As String, ByVal v_sSchemeName As String, ByVal v_sBrandCode As String, ByVal v_bMTA As Boolean) As Integer
        Dim result As Integer = 0
        Dim CDONTS As Object

        Dim vKeyArray As Object
        Dim vDriverKeyArray As Object
        Dim vPaymentAndBankKeyArray As Object
        Dim vNPPremiumFinanceKeyArray As Object
        Dim sKey, sDriverKey, sPaymentAndBankKey, sNPPremiumFinanceKey As String
        Dim vData As Object
        Dim bIsAssumedInfo As Boolean

        Dim sProposer_TitleCode, sProposer_Forename, sProposer_Surname, sProposer_AddressLine1, sProposer_AddressLine2, sProposer_AddressLine3, sProposer_AddressLine4, sProposer_AddressPostcode, sProposer_Email, sProposer_TelNoHome, sProposer_Sex, sProposer_DateOfbirth, sProposer_Age, sProposer_MaritalStatus As String
        Dim sDriver_Surname, sDriver_Initial, sDriver_MainDriverInd, sDriver_MainDriver, sDriver_Relationship, sDriver_Sex, sDriver_MaritalStatus, sDriver_DateOfBirth, sDriver_LicenceType, sDriver_LicenceDate, sDriver_Age As String

        Dim iOccupationKey As Integer
        Dim sOcc_Code, sOcc_EmployersBusiness, sOcc_FullTimeEmploymentInd, sOcc_MainCode, sOcc_MainEmployersBusiness, sOcc_PartTimeCode, sOcc_PartTimeEmployersBusiness As String
        Dim sVehicle_Manufacturer, sVehicle_Model, sVehicle_AnnualMileage, sVehicle_Value, sVehicle_YearManufactured, sVehicle_RegNo, sVehicle_LocationKeptOvernight, sVehicle_Postcode, sVehicle_Ownership, sVehicle_Keeper, sVehicle_ClassOfUse, sVehicle_TrackerFitted, sSecurity_MakeAndModel As String

        Dim sPolicy_PolicyNo, sPolicy_EffectiveStartDate, sPolicy_EffectiveStartTime, sPolicy_PolicyExcess, sPolicy_VoluntaryExcess, sPolicy_PolicyPremium, sPolicy_PolicyIPT As String
        'ND 061000
        Dim sPolicy_MTAPremium, sPolicy_MTAIPT As String

        Dim sCover_Code, sCover_RequiredDrivers, sCover_BreakdownRequired, sCover_ULRRequired As String

        'RJG 09/03/2001 - Vars for Breakdown and ULR values
        Dim sCover_BreakdownPremium, sCover_BreakdownIPT, sCover_ULRPremium, sCover_ULRIPT As String

        Dim sNCD_ClaimedYearsEarned, sNCD_ClaimedProtectionReqdInd As String

        Dim sCalculatedResult_OVERRIDE_PREMIUM_INCL_IPT, sCalculatedResult_IPT_Pct, sCalculatedResult_ADJUSTMENT_PREMIUM_INCL_IPT As String

        Dim sPaymentMethod, sDeposit, sFirstInstalAmt, sSubseqInstalAmt, sNoOfInstal As String

        Dim s, sMsg As String
        Dim oCDONTS As Object

        Dim oList As bGISListManager.InterfaceNoLogin

        Dim iKey, iDriverKey, iIncidentKey, iMonths As Integer

        Dim iChan As Integer
        Dim sPath, sFooterPath, sDummy As String

        Dim sBrandName As String = ""




        result = gPMConstants.PMEReturnCode.PMTrue

        sMsg = ""

        oList = New bGISListManager.InterfaceNoLogin()
        If oList Is Nothing Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = oList.Initialise()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = oList.CheckListVersions(ACDataModelCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        'm_lReturn = m_oDataSet.ReturnAsXML(sDummy, s)

        'LogMessageFile _
        'iType:=PMLogOnError, _
        'sMsg:="GenerateCustomerEmail XML = " & s, _
        'vApp:=tosafestring(ACApp), _
        'vClass:=ACClass, _
        'vMethod:="GenerateCustomerEmail"

        If v_bMTA Then

            '------------------------------------------------------
            'CALCULATED_RESULT
            '------------------------------------------------------

            m_lReturn = m_oDataSet.GetAllOIKey(ACDataModelCodePrefix & "Calculated_result", vKeyArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vKeyArray) Then
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OI Key array is empty for " & ACDataModelCodePrefix & "Calculated_result", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GenerateCustomerEmail")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            sKey = CStr(vKeyArray(0))

            ' Get IPT % from the correct Quote object

            s = ""
            sCalculatedResult_IPT_Pct = ""


            m_lReturn = m_oDataSet.GetAllOIKey(ACDataModelCodePrefix & "NP_QUOTE", vKeyArray)
            If Informations.IsArray(vKeyArray) Then
                If m_oDataSet.Quote(1).Count(ACDataModelCodePrefix & "NP_QUOTE") = 1 Then s = ACDataModelCodePrefix & "NP_QUOTE"
            End If


            m_lReturn = m_oDataSet.GetAllOIKey(ACDataModelCodePrefix & "NP_REFER", vKeyArray)
            If Informations.IsArray(vKeyArray) Then
                If m_oDataSet.Quote(1).Count(ACDataModelCodePrefix & "NP_REFER") = 1 Then s = ACDataModelCodePrefix & "NP_REFER"
            End If


            m_lReturn = m_oDataSet.GetAllOIKey(ACDataModelCodePrefix & "NP_DECLINE", vKeyArray)
            If Informations.IsArray(vKeyArray) Then
                If m_oDataSet.Quote(1).Count(ACDataModelCodePrefix & "NP_DECLINE") = 1 Then s = ACDataModelCodePrefix & "NP_DECLINE"
            End If


            If s <> "" Then
                ' IPT %
                m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=s, v_sPropertyName:="IPT_Pct", v_sOIKey:=sKey, r_vPropertyValue:=sCalculatedResult_IPT_Pct, r_bIsAssumedInfo:=bIsAssumedInfo)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateCustEmail - sCalculatedResult_IPT_Pct = " & sCalculatedResult_IPT_Pct, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GenerateCustEmail")
            End If

            If sCalculatedResult_IPT_Pct = "" Then sCalculatedResult_IPT_Pct = "5"

            ' Get new risk prem (annual premium excl IPT)
            '        m_lReturn = m_oDataSet.GetPropertyValue( _
            ''            v_sObjectName:=ACDataModelCodePrefix & "CALCULATED_RESULT", _
            ''            v_spropertyname:="NEW_RISK_PREMIUM", _
            ''            v_sOIKey:=sKey, _
            ''            r_vpropertyvalue:=sPolicy_PolicyPremium, _
            ''            r_bisassumedinfo:=bIsAssumedInfo)
            '        If m_lReturn <> PMTrue Then
            '            GenerateCustomerEmail = PMFalse
            '            Exit Function
            '        End If


            m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "CALCULATED_RESULT", v_sPropertyName:="ORIGINAL_PREMIUM", v_sOIKey:=sKey, r_vPropertyValue:=sPolicy_PolicyPremium, r_bIsAssumedInfo:=bIsAssumedInfo)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ''''sPolicy_PolicyPremium = m_oDataSet.Quote(1).Item(ACDataModelCodePrefix & "NP_QUOTE", 1).Item("NEW_RISK_PREMIUM").Value

            ' Get MTA Premium from overridden amount
            m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "CALCULATED_RESULT", v_sPropertyName:="OVERRIDE_PREMIUM_INCL_IPT", v_sOIKey:=sKey, r_vPropertyValue:=sPolicy_MTAPremium, r_bIsAssumedInfo:=bIsAssumedInfo)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' if premium was not overridden get from adjustment premium
            If sPolicy_MTAPremium = "" Then
                sPolicy_MTAPremium = m_oDataSet.Quote(1).Item(ACDataModelCodePrefix & "NP_QUOTE", 1).Item("Adjustment_premium_incl_ipt").Value
            End If

            ' Calculate MTA IPT

            s = CStr(1 + (CDbl(sCalculatedResult_IPT_Pct) / 100))
            sPolicy_MTAIPT = CStr(CDbl(sPolicy_MTAPremium) - (CDbl(sPolicy_MTAPremium) / CDbl(s)))

            ' Add IPT to annual premium
            sPolicy_PolicyPremium = CStr(CDbl(sPolicy_PolicyPremium) * CDbl(s))

            ' Calculate Revised Premium IPT
            sPolicy_PolicyIPT = CStr(CDbl(sPolicy_PolicyPremium) - (CDbl(sPolicy_PolicyPremium) / CDbl(s)))

        End If

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Proposer_Policyholder
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


        m_lReturn = m_oDataSet.GetAllOIKey(ACDataModelCodePrefix & "Proposer_Policyholder", vKeyArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        sKey = CStr(vKeyArray(0))

        ' Get all reqd properties from Proposer_Policyholder

        ' Proposer_Policyholder, title_code
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "PROPOSER_POLICYHOLDER", v_sPropertyName:="title_code", v_sOIKey:=sKey, r_vPropertyValue:=sProposer_TitleCode, r_bIsAssumedInfo:=bIsAssumedInfo)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get text for this ABI code
        m_lReturn = oList.GetDescription("131085", sProposer_TitleCode, sProposer_TitleCode)
        '    If m_lReturn <> PMTrue Then
        '        GenerateCustomerEmail = PMFalse
        '        Exit Function
        '    End If


        ' Proposer_Policyholder, Forename/Initial 1
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "PROPOSER_Policyholder", v_sPropertyName:="Forename_Initial_1", v_sOIKey:=sKey, r_vPropertyValue:=sProposer_Forename, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        ' Proposer_Policyholder, Surname
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "PROPOSER_POLICYHOLDER", v_sPropertyName:="Surname", v_sOIKey:=sKey, r_vPropertyValue:=sProposer_Surname, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Proposer_Policyholder, Address_Line_1
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "PROPOSER_POLICYHOLDER", v_sPropertyName:="Address_Line_1", v_sOIKey:=sKey, r_vPropertyValue:=sProposer_AddressLine1, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Proposer_Policyholder, Address_Line_2
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "PROPOSER_POLICYHOLDER", v_sPropertyName:="Address_Line_2", v_sOIKey:=sKey, r_vPropertyValue:=sProposer_AddressLine2, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Proposer_Policyholder, Address_Line_3
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "PROPOSER_POLICYHOLDER", v_sPropertyName:="Address_Line_3", v_sOIKey:=sKey, r_vPropertyValue:=sProposer_AddressLine3, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Proposer_Policyholder, Address_Line_4
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "PROPOSER_POLICYHOLDER", v_sPropertyName:="Address_Line_4", v_sOIKey:=sKey, r_vPropertyValue:=sProposer_AddressLine4, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Proposer_Policyholder, Address_Postcode
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "PROPOSER_POLICYHOLDER", v_sPropertyName:="Address_Post_code", v_sOIKey:=sKey, r_vPropertyValue:=sProposer_AddressPostcode, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Proposer_Policyholder, Email
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "PROPOSER_POLICYHOLDER", v_sPropertyName:="email", v_sOIKey:=sKey, r_vPropertyValue:=sProposer_Email, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Proposer_Policyholder, Tel_No_Home
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "PROPOSER_POLICYHOLDER", v_sPropertyName:="Tel_No_Home", v_sOIKey:=sKey, r_vPropertyValue:=sProposer_TelNoHome, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Proposer_Policyholder, sex
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "PROPOSER_POLICYHOLDER", v_sPropertyName:="sex", v_sOIKey:=sKey, r_vPropertyValue:=sProposer_Sex, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        ' Get text for this ABI code
        m_lReturn = oList.GetDescription("131091", sProposer_Sex, sProposer_Sex)
        '    If m_lReturn <> PMTrue Then
        '        GenerateCustomerEmail = PMFalse
        '        Exit Function
        '    End If


        ' Proposer_Policyholder, Date_of_birth
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "PROPOSER_POLICYHOLDER", v_sPropertyName:="Date_of_birth", v_sOIKey:=sKey, r_vPropertyValue:=sProposer_DateOfbirth, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        sProposer_Age = CStr(GetAge(CDate(sProposer_DateOfbirth)))

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Driver
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


        m_lReturn = m_oDataSet.GetAllOIKey(ACDataModelCodePrefix & "Driver", vKeyArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        sKey = CStr(vKeyArray(0))
        sDriverKey = sKey

        ' Get all reqd properties from Driver 1 (assume Proposer)

        ' Driver, Licence_Type
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "DRIVER", v_sPropertyName:="Licence_Type", v_sOIKey:=sKey, r_vPropertyValue:=sDriver_LicenceType, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get text for this ABI code
        m_lReturn = oList.GetDescription("983055", sDriver_LicenceType, sDriver_LicenceType)
        '    If m_lReturn <> PMTrue Then
        '        GenerateCustomerEmail = PMFalse
        '        Exit Function
        '    End If


        ' Driver, Licence_Date
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "DRIVER", v_sPropertyName:="Licence_Date", v_sOIKey:=sKey, r_vPropertyValue:=sDriver_LicenceDate, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Driver, Main Driver ind
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "DRIVER", v_sPropertyName:="Drives_vehicle_1_ind", v_sOIKey:=sKey, r_vPropertyValue:=sDriver_MainDriverInd, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If sDriver_MainDriverInd = "M" Then
            sDriver_MainDriver = sProposer_Forename & " " & sProposer_Surname
        End If

        ' Proposer, Marital Status
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "DRIVER", v_sPropertyName:="Marital_Status", v_sOIKey:=sKey, r_vPropertyValue:=sProposer_MaritalStatus, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get text for this ABI code
        m_lReturn = oList.GetDescription("131107", sProposer_MaritalStatus, sProposer_MaritalStatus)


        'CL190700
        'Change "licence date" to "number of years licence held"
        If Informations.IsDate(sDriver_LicenceDate) Then
            iMonths = CInt(CStr(Informations.DateDiff("m", CDate(sDriver_LicenceDate), DateTime.Now, FirstDayOfWeek.Sunday, FirstWeekOfYear.FirstJan1)))
            '''sDriver_LicenceDate = CStr(iMonths \ 12) & " Years and " & CStr(iMonths Mod 12) & " Months"
            If iMonths \ 12 >= 10 Then
                sDriver_LicenceDate = CStr(iMonths \ 12) & " or more Years"
            Else
                sDriver_LicenceDate = CStr(iMonths \ 12) & " Years"
            End If
        End If

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' OCCUPATION for driver instance 1 above
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        m_lReturn = m_oDataSet.GetChildOIKey(ACDataModelCodePrefix & "DRIVER", sDriverKey, ACDataModelCodePrefix & "OCCUPATION", vKeyArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        sOcc_PartTimeCode = ""
        sOcc_PartTimeEmployersBusiness = ""


        If vKeyArray.GetUpperBound(0) > 0 Then

            iOccupationKey = 0


            Do While vKeyArray.GetUpperBound(0) >= iOccupationKey


                sKey = CStr(vKeyArray(iOccupationKey))

                ' OCCUPATION, Full Time Employment Ind
                m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "OCCUPATION", v_sPropertyName:="Full_time_employment_ind", v_sOIKey:=sKey, r_vPropertyValue:=sOcc_FullTimeEmploymentInd, r_bIsAssumedInfo:=bIsAssumedInfo)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' OCCUPATION, code
                m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "OCCUPATION", v_sPropertyName:="code", v_sOIKey:=sKey, r_vPropertyValue:=sOcc_Code, r_bIsAssumedInfo:=bIsAssumedInfo)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Get text for this ABI code
                m_lReturn = oList.GetDescription("2228226", sOcc_Code, sOcc_Code)
                '    If m_lReturn <> PMTrue Then
                '        GenerateCustomerEmail = PMFalse
                '        Exit Function
                '    End If

                ' OCCUPATION, Employers Business
                m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "OCCUPATION", v_sPropertyName:="Employers_Business", v_sOIKey:=sKey, r_vPropertyValue:=sOcc_EmployersBusiness, r_bIsAssumedInfo:=bIsAssumedInfo)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If sOcc_EmployersBusiness = "186" Then
                    sOcc_FullTimeEmploymentInd = "Y"
                End If

                ' Get text for this ABI code
                m_lReturn = oList.GetDescription("2228228", sOcc_EmployersBusiness, sOcc_EmployersBusiness)

                If sOcc_FullTimeEmploymentInd = "Y" Then
                    sOcc_MainCode = sOcc_Code
                    sOcc_MainEmployersBusiness = sOcc_EmployersBusiness
                Else
                    If sOcc_Code.Trim() <> "" Then
                        sOcc_PartTimeCode = sOcc_Code
                        sOcc_PartTimeEmployersBusiness = sOcc_EmployersBusiness
                    End If
                End If

                iOccupationKey += 1

            Loop
        Else


            sKey = CStr(vKeyArray(0))

            ' OCCUPATION, code
            m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "OCCUPATION", v_sPropertyName:="code", v_sOIKey:=sKey, r_vPropertyValue:=sOcc_MainCode, r_bIsAssumedInfo:=bIsAssumedInfo)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'sMsg = sMsg & " DEBUG THIS IS THE OCC MAIN CODE " & sOcc_MainCode & vbCrLf

            ' Get text for this ABI code
            m_lReturn = oList.GetDescription("2228226", sOcc_MainCode, sOcc_MainCode)
            '    If m_lReturn <> PMTrue Then
            '        GenerateCustomerEmail = PMFalse
            '        Exit Function
            '    End If

            ' OCCUPATION, Employers Business
            m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "OCCUPATION", v_sPropertyName:="Employers_Business", v_sOIKey:=sKey, r_vPropertyValue:=sOcc_MainEmployersBusiness, r_bIsAssumedInfo:=bIsAssumedInfo)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get text for this ABI code
            m_lReturn = oList.GetDescription("2228228", sOcc_MainEmployersBusiness, sOcc_MainEmployersBusiness)
        End If


        m_lReturn = GISSharedConstants.GetBrandName(v_sBrandCode:=v_sBrandCode, r_sBrandName:=sBrandName)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Default opening text
        sMsg = ""
        sMsg = sMsg & "Thank you for buying your Motor Insurance from " & sBrandName & "." & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Please find below the details of your cover. If any of these details are incorrect, or change at anytime, please contact help@insurance-enquiries.com" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10) 'give a 2 line space

        ' AUTOBYTEL - opening text
        If v_sBrandCode = GISSharedConstants.GISXelBrandCodeAutoBytel Then
            sMsg = ""
            sMsg = sMsg & "Thank you for buying your Motor Insurance from " & sBrandName & "." & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
            sMsg = sMsg & "Please find below the details of your cover. If any of these details are incorrect, or change at anytime, please contact help@insurance-enquiries.com" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10) 'give a 2 line space
        End If

        ' FIRST E - opening text
        If v_sBrandCode = GISSharedConstants.GISXelBrandCodeFirste Then
            sMsg = ""
            sMsg = sMsg & "Thank you for buying your motor insurance from us. This service was brought to you in conjunction with first-e, the internet bank." & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
            sMsg = sMsg & "Please find below the details of your cover. If any of these details are incorrect, or change at anytime, please contact help@insurance-enquiries.com" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10) 'give a 2 line space
        End If

        ' ETRADE - opening text
        If v_sBrandCode = GISSharedConstants.GISXelBrandCodeETrade Then
            sMsg = ""
            sMsg = sMsg & "Thank you for buying your motor insurance from us. This service was brought to you in conjunction with " & sBrandName & "." & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
            sMsg = sMsg & "Please find below the details of your cover. If any of these details are incorrect, or change at anytime, please contact help@insurance-enquiries.com" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10) 'give a 2 line space
        End If


        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Policy
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


        m_lReturn = m_oDataSet.GetAllOIKey(ACDataModelCodePrefix & "Policy", vKeyArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        sKey = CStr(vKeyArray(0))

        ' Get all reqd properties from vehicle

        ' Policy, Policy No
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "POLICY", v_sPropertyName:="Policy_No", v_sOIKey:=sKey, r_vPropertyValue:=sPolicy_PolicyNo, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Policy, Effective Start Date
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "POLICY", v_sPropertyName:="Effective_Start_Date", v_sOIKey:=sKey, r_vPropertyValue:=sPolicy_EffectiveStartDate, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Policy, Effective Start Time
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "POLICY", v_sPropertyName:="Effective_Start_Time", v_sOIKey:=sKey, r_vPropertyValue:=sPolicy_EffectiveStartTime, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Convert this long to hh:mm

        If sPolicy_EffectiveStartTime.Length < 6 Then
            sPolicy_EffectiveStartTime = New String(" "c, 6 - sPolicy_EffectiveStartTime.Length) & sPolicy_EffectiveStartTime
        End If

        sPolicy_EffectiveStartTime = sPolicy_EffectiveStartTime.Substring(0, 2) & ":" & Mid(sPolicy_EffectiveStartTime, 3, 2)

        ' Policy, Policy Excess
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "POLICY", v_sPropertyName:="Policy_Excess", v_sOIKey:=sKey, r_vPropertyValue:=sPolicy_PolicyExcess, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If Not v_bMTA Then

            ' Policy, Policy Premium
            m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "POLICY", v_sPropertyName:="Policy_Premium", v_sOIKey:=sKey, r_vPropertyValue:=sPolicy_PolicyPremium, r_bIsAssumedInfo:=bIsAssumedInfo)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Policy, Policy IPT
            m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "POLICY", v_sPropertyName:="Policy_IPT", v_sOIKey:=sKey, r_vPropertyValue:=sPolicy_PolicyIPT, r_bIsAssumedInfo:=bIsAssumedInfo)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Cover
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


        m_lReturn = m_oDataSet.GetAllOIKey(ACDataModelCodePrefix & "Cover", vKeyArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        sKey = CStr(vKeyArray(0))

        ' Get all reqd properties from vehicle

        ' Cover, Code
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "COVER", v_sPropertyName:="Code", v_sOIKey:=sKey, r_vPropertyValue:=sCover_Code, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get text for this ABI code
        m_lReturn = oList.GetDescription("851969", sCover_Code, sCover_Code)
        '    If m_lReturn <> PMTrue Then
        '        GenerateCustomerEmail = PMFalse
        '        Exit Function
        '    End If

        ' Cover, Voluntary Excess Allowed
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "COVER", v_sPropertyName:="Vol_xs_allowed", v_sOIKey:=sKey, r_vPropertyValue:=sPolicy_VoluntaryExcess, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        ' Cover, Required Drivers
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "COVER", v_sPropertyName:="Required_drivers", v_sOIKey:=sKey, r_vPropertyValue:=sCover_RequiredDrivers, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get text for this ABI code
        m_lReturn = oList.GetDescription("852055", sCover_RequiredDrivers, sCover_RequiredDrivers)

        ' Cover, Breakdown Required
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "COVER", v_sPropertyName:="NP_Breakdown_Required", v_sOIKey:=sKey, r_vPropertyValue:=sCover_BreakdownRequired, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If sCover_BreakdownRequired = "1" Then
            sCover_BreakdownRequired = "You do require Breakdown Cover Upgrade."
        Else
            sCover_BreakdownRequired = "You do not require Breakdown Cover Upgrade."
        End If

        ' Cover, ULR Required
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "COVER", v_sPropertyName:="NP_ULR_Required", v_sOIKey:=sKey, r_vPropertyValue:=sCover_ULRRequired, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If sCover_ULRRequired = "1" Then
            sCover_ULRRequired = "You do require Uninsured Loss Recovery Upgrade."
        Else
            sCover_ULRRequired = "You do not require Uninsured Loss Recovery Upgrade."
        End If

        ' Cover, Breakdown Premium
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "COVER", v_sPropertyName:="NP_BREAKDOWN_PREMIUM", v_sOIKey:=sKey, r_vPropertyValue:=sCover_BreakdownPremium, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Cover, Breakdown IPT
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "COVER", v_sPropertyName:="NP_BREAKDOWN_IPT", v_sOIKey:=sKey, r_vPropertyValue:=sCover_BreakdownIPT, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Cover, ULR Premium
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "COVER", v_sPropertyName:="NP_ULR_PREMIUM", v_sOIKey:=sKey, r_vPropertyValue:=sCover_ULRPremium, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Cover, ULR IPT
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "COVER", v_sPropertyName:="NP_ULR_IPT", v_sOIKey:=sKey, r_vPropertyValue:=sCover_ULRIPT, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not v_bMTA And (sCover_BreakdownRequired = "1" Or sCover_ULRRequired = "1") Then
            sPolicy_PolicyPremium = ToSafeString(CDbl(sPolicy_PolicyPremium) + CDbl(sCover_BreakdownPremium) + CDbl(sCover_ULRPremium))
            sPolicy_PolicyIPT = ToSafeString(CDbl(sPolicy_PolicyIPT) + CDbl(sCover_BreakdownIPT) + CDbl(sCover_ULRIPT))
        End If

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' NCD
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


        m_lReturn = m_oDataSet.GetAllOIKey(ACDataModelCodePrefix & "NCD", vKeyArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        sKey = CStr(vKeyArray(0))

        ' Get all reqd properties from vehicle

        ' NCD, Claimed years earned
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "NCD", v_sPropertyName:="Claimed_years_earned", v_sOIKey:=sKey, r_vPropertyValue:=sNCD_ClaimedYearsEarned, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If sNCD_ClaimedYearsEarned.Trim() = "6" Then
            sNCD_ClaimedYearsEarned = sNCD_ClaimedYearsEarned.Trim() & " or Above"
        End If

        ' NCD, Claimed Protection Required Ind
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "NCD", v_sPropertyName:="Claimed_protection_reqd_ind", v_sOIKey:=sKey, r_vPropertyValue:=sNCD_ClaimedProtectionReqdInd, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get text for this ABI code
        'm_lReturn = oList.GetDescription("6291463", sNCD_ClaimedProtectionReqdInd, sNCD_ClaimedProtectionReqdInd)

        'BOTCH the ABI lookup doesn't seem to work for this so just select from Y or N

        If sNCD_ClaimedProtectionReqdInd = "Y" Then
            sNCD_ClaimedProtectionReqdInd = "Yes"
        Else
            sNCD_ClaimedProtectionReqdInd = "No"
        End If

        '--------------------------------------------------------------------------------------
        'CJB 120101 Start
        'If Method of payment is DD (Direct Debit) then get payment details to show on email
        '--------------------------------------------------------------------------------------

        ' Get the Payment And Bank Object Key

        m_lReturn = m_oDataSet.GetAllOIKey(ACDataModelCodePrefix & "Payment_Bank", vPaymentAndBankKeyArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        sPaymentAndBankKey = CStr(vPaymentAndBankKeyArray(vPaymentAndBankKeyArray.GetLowerBound(0)))

        sPaymentMethod = m_oDataSet.Risk.Item(ACDataModelCodePrefix & "Payment_Bank").Item("Insr_Pmt_Method_Type").Value

        If sPaymentMethod = GISPromptConstants.PromptPaymentMethodDirectDebit Then

            ' Get the NP_Premium_Finance Object Key

            m_lReturn = m_oDataSet.GetAllOIKey(ACDataModelCodePrefix & "NP_Premium_Finance", vNPPremiumFinanceKeyArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            sNPPremiumFinanceKey = CStr(vNPPremiumFinanceKeyArray(vNPPremiumFinanceKeyArray.GetLowerBound(0)))

            sDeposit = m_oDataSet.Risk.Item(ACDataModelCodePrefix & "NP_Premium_Finance").Item("PF_Deposit").Value
            sFirstInstalAmt = m_oDataSet.Risk.Item(ACDataModelCodePrefix & "NP_Premium_Finance").Item("PF_Amount_of_First_Instalment").Value
            sSubseqInstalAmt = m_oDataSet.Risk.Item(ACDataModelCodePrefix & "NP_Premium_Finance").Item("PF_Amount_of_Subsequent_Instalments").Value
            sNoOfInstal = m_oDataSet.Risk.Item(ACDataModelCodePrefix & "NP_Premium_Finance").Item("PF_No_of_instalments").Value

            'Subtract 1 off no. of instal to get no. of subsequent instalments
            Dim dbNumericTemp As Double
            If Double.TryParse(sNoOfInstal, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                sNoOfInstal = CStr(CInt(sNoOfInstal) - 1)
            Else
                sNoOfInstal = ""
            End If

        End If

        '--------------------------------------------------------------------------------------
        'CJB 120101 End
        '--------------------------------------------------------------------------------------

        ' Your Cover
        sMsg = sMsg & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Your Cover" & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "----------" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Policy No.: " & sPolicy_PolicyNo & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Insurer: " & v_sInsurerDesc & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Scheme: " & v_sSchemeName & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Start Date and Time " & sPolicy_EffectiveStartDate & " at " & sPolicy_EffectiveStartTime & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Type Of Cover: " & sCover_Code & Strings.ChrW(13) & Strings.ChrW(10)

        'RJG 25/07/2000 - New fields added so that E-Mail SOF is same as the screen SOF
        'BEGIN
        sMsg = sMsg & "No of years No Claims Discount: " & sNCD_ClaimedYearsEarned & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Who will drive the car?: " & sCover_RequiredDrivers & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "No claims discount protected?: " & sNCD_ClaimedProtectionReqdInd & Strings.ChrW(13) & Strings.ChrW(10)
        'RJG Dont know where the values for these come from yet but put the text in anyway!
        sMsg = sMsg & "Policy Excess: " & Strings.ChrW(163).ToString() & StringsHelper.Format(sPolicy_PolicyExcess, "####0.00") & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Voluntary Excess: " & Strings.ChrW(163).ToString() & StringsHelper.Format(sPolicy_VoluntaryExcess, "####0.00") & Strings.ChrW(13) & Strings.ChrW(10)

        'RJG 25/07/2000 - END


        If v_bMTA Then

            sMsg = sMsg & "Revised Annual Premium: " & Strings.ChrW(163).ToString() & StringsHelper.Format(sPolicy_PolicyPremium, "####0.00") & Strings.ChrW(13) & Strings.ChrW(10)
            sMsg = sMsg & "Revised Annual Premium IPT: " & Strings.ChrW(163).ToString() & StringsHelper.Format(sPolicy_PolicyIPT, "####0.00") & Strings.ChrW(13) & Strings.ChrW(10)
            sMsg = sMsg & "MTA Premium: " & Strings.ChrW(163).ToString() & StringsHelper.Format(sPolicy_MTAPremium, "####0.00") & Strings.ChrW(13) & Strings.ChrW(10)
            sMsg = sMsg & "MTA IPT: " & Strings.ChrW(163).ToString() & StringsHelper.Format(sPolicy_MTAIPT, "####0.00") & Strings.ChrW(13) & Strings.ChrW(10)

        Else

            sMsg = sMsg & "Policy Premium: " & Strings.ChrW(163).ToString() & StringsHelper.Format(sPolicy_PolicyPremium, "####0.00") & Strings.ChrW(13) & Strings.ChrW(10)
            sMsg = sMsg & "Policy IPT: " & Strings.ChrW(163).ToString() & StringsHelper.Format(sPolicy_PolicyIPT, "####0.00") & Strings.ChrW(13) & Strings.ChrW(10)

            'CJB 120101 Show payment info if DD
            If sPaymentMethod = GISPromptConstants.PromptPaymentMethodDirectDebit Then
                sMsg = sMsg & "Initial Payment: " & Strings.ChrW(163).ToString() & StringsHelper.Format(sDeposit, "####0.00") & Strings.ChrW(13) & Strings.ChrW(10)
                sMsg = sMsg & "First Instalment: " & Strings.ChrW(163).ToString() & StringsHelper.Format(sFirstInstalAmt, "####0.00") & Strings.ChrW(13) & Strings.ChrW(10)
                sMsg = sMsg & "Followed by " & sNoOfInstal & " successive payments of " & Strings.ChrW(163).ToString() & StringsHelper.Format(sSubseqInstalAmt, "####0.00") & Strings.ChrW(13) & Strings.ChrW(10)
            End If
        End If


        m_lReturn = m_oDataSet.GetAllOIKey(ACDataModelCodePrefix & "Vehicle", vKeyArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        sKey = CStr(vKeyArray(0))

        ' SPW 20/10/2000 get class of use as this is the only vehicle property required at this
        ' point in the email SOF
        ' Vehicle, Class_of_Use
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "VEHICLE", v_sPropertyName:="Class_of_Use", v_sOIKey:=sKey, r_vPropertyValue:=sVehicle_ClassOfUse, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        ' Get text for this ABI code
        m_lReturn = oList.GetDescription("393239", sVehicle_ClassOfUse, sVehicle_ClassOfUse)


        sMsg = sMsg & "Use Of Car: " & sVehicle_ClassOfUse & Strings.ChrW(13) & Strings.ChrW(10)


        ' Your Insurance Options
        sMsg = sMsg & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Your Insurance Options" & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "----------------------" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & sCover_BreakdownRequired & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & sCover_ULRRequired & Strings.ChrW(13) & Strings.ChrW(10)

        sMsg = sMsg & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)


        ' About You
        sMsg = sMsg & "About You" & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "---------" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
        s = sProposer_TitleCode & " " & sProposer_Forename & " " & sProposer_Surname
        sMsg = sMsg & "Name: " & s & Strings.ChrW(13) & Strings.ChrW(10)
        s = sProposer_AddressLine1 & " " & sProposer_AddressLine2 & " " & sProposer_AddressLine3 & " " & sProposer_AddressLine4
        sMsg = sMsg & "Address: " & s & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Postcode: " & sProposer_AddressPostcode & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Email: " & sProposer_Email & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Tel No: " & sProposer_TelNoHome & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Sex: " & sProposer_Sex & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Marital Status: " & sProposer_MaritalStatus & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Date Of Birth (Age): " & sProposer_DateOfbirth & " (" & sProposer_Age & " years)" & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Occupation: " & sOcc_MainCode & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Employer's Business: " & sOcc_MainEmployersBusiness & Strings.ChrW(13) & Strings.ChrW(10)

        If sOcc_PartTimeCode <> "" Then
            sMsg = sMsg & "Part Time Occupation: " & sOcc_PartTimeCode & Strings.ChrW(13) & Strings.ChrW(10)
            sMsg = sMsg & "Part Time Employer's Business: " & sOcc_PartTimeEmployersBusiness & Strings.ChrW(13) & Strings.ChrW(10)
        End If

        sMsg = sMsg & "Licence Type: " & sDriver_LicenceType & Strings.ChrW(13) & Strings.ChrW(10)
        '    sMsg = sMsg & "Number of years licence held: " & CStr((DateDiff("d", sDriver_LicenceDate, Now) - (DateDiff("d", sDriver_LicenceDate, Now))) / 365) & vbCrLf
        sMsg = sMsg & "Number of years licence held: " & sDriver_LicenceDate & Strings.ChrW(13) & Strings.ChrW(10)


        ' Get Claims and convictions for this driver
        m_lReturn = GetDriverIncidents(sDriverKey, oList, sMsg)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDriverIncidents Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GenerateCustomerEmail")
            Return result
        End If

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Driver - Other Drivers RAG210600
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


        m_lReturn = m_oDataSet.GetAllOIKey(ACDataModelCodePrefix & "Driver", vDriverKeyArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        iDriverKey = 1

        ' Loop for all additional drivers

        Do While (vDriverKeyArray.GetUpperBound(0) >= iDriverKey)


            sKey = CStr(vDriverKeyArray(iDriverKey))
            sDriverKey = sKey

            ' Driver, Forename/Initial 1
            m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "DRIVER", v_sPropertyName:="Forename_Initial_1", v_sOIKey:=sKey, r_vPropertyValue:=sDriver_Initial, r_bIsAssumedInfo:=bIsAssumedInfo)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Driver, Surname
            m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "DRIVER", v_sPropertyName:="Surname", v_sOIKey:=sKey, r_vPropertyValue:=sDriver_Surname, r_bIsAssumedInfo:=bIsAssumedInfo)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            s = sDriver_Initial & " " & sDriver_Surname

            If s.Trim() <> "" Then

                ' Driver, Relationship_to_Proposer
                m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "DRIVER", v_sPropertyName:="Relationship_To_Proposer", v_sOIKey:=sKey, r_vPropertyValue:=sDriver_Relationship, r_bIsAssumedInfo:=bIsAssumedInfo)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Get text for this ABI code
                m_lReturn = oList.GetDescription("983044", sDriver_Relationship, sDriver_Relationship)

                ' Driver, Sex
                m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "DRIVER", v_sPropertyName:="Sex", v_sOIKey:=sKey, r_vPropertyValue:=sDriver_Sex, r_bIsAssumedInfo:=bIsAssumedInfo)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Proposer, Marital Status
                m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "DRIVER", v_sPropertyName:="Marital_Status", v_sOIKey:=sKey, r_vPropertyValue:=sDriver_MaritalStatus, r_bIsAssumedInfo:=bIsAssumedInfo)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Get text for this ABI code
                m_lReturn = oList.GetDescription("131107", sDriver_MaritalStatus, sDriver_MaritalStatus)


                ' Get text for this ABI code
                'RJG 21/07/2000 - This list no didn't seem to work m_lReturn = oList.GetDescription("10022", sDriver_Sex, sDriver_Sex)
                m_lReturn = oList.GetDescription("131091", sDriver_Sex, sDriver_Sex)

                ' Driver, Date_Of_Birth
                m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "DRIVER", v_sPropertyName:="Date_Of_Birth", v_sOIKey:=sKey, r_vPropertyValue:=sDriver_DateOfBirth, r_bIsAssumedInfo:=bIsAssumedInfo)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                sDriver_Age = CStr(GetAge(CDate(sDriver_DateOfBirth)))

                ' Driver, Licence_Type
                m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "DRIVER", v_sPropertyName:="Licence_Type", v_sOIKey:=sKey, r_vPropertyValue:=sDriver_LicenceType, r_bIsAssumedInfo:=bIsAssumedInfo)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = oList.GetDescription("983055", sDriver_LicenceType, sDriver_LicenceType)

                ' Driver, Licence_Date
                m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "DRIVER", v_sPropertyName:="Licence_Date", v_sOIKey:=sKey, r_vPropertyValue:=sDriver_LicenceDate, r_bIsAssumedInfo:=bIsAssumedInfo)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'CL190700
                'Change "licence date" to "number of years licence held"
                If Informations.IsDate(sDriver_LicenceDate) Then
                    iMonths = CInt(CStr(Informations.DateDiff("m", CDate(sDriver_LicenceDate), DateTime.Now, FirstDayOfWeek.Sunday, FirstWeekOfYear.FirstJan1)))
                    '''sDriver_LicenceDate = CStr(iMonths \ 12) & " Years and " & CStr(iMonths Mod 12) & " Months"
                    If iMonths \ 12 >= 10 Then
                        sDriver_LicenceDate = CStr(iMonths \ 12) & " or more Years"
                    Else
                        sDriver_LicenceDate = CStr(iMonths \ 12) & " Years"
                    End If
                End If

                ' Driver, Main Driver ind
                m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "DRIVER", v_sPropertyName:="Drives_vehicle_1_ind", v_sOIKey:=sKey, r_vPropertyValue:=sDriver_MainDriverInd, r_bIsAssumedInfo:=bIsAssumedInfo)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If sDriver_MainDriverInd = "M" Then
                    sDriver_MainDriver = s
                End If

                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ' OCCUPATION for driver instance x above
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                m_lReturn = m_oDataSet.GetChildOIKey(ACDataModelCodePrefix & "DRIVER", sDriverKey, ACDataModelCodePrefix & "OCCUPATION", vKeyArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                sOcc_MainCode = ""
                sOcc_MainEmployersBusiness = ""
                sOcc_PartTimeCode = ""
                sOcc_PartTimeEmployersBusiness = ""


                If vKeyArray.GetUpperBound(0) > 0 Then

                    iOccupationKey = 0


                    Do While vKeyArray.GetUpperBound(0) >= iOccupationKey


                        sKey = CStr(vKeyArray(iOccupationKey))

                        ' OCCUPATION, Full Time Employment Ind
                        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "OCCUPATION", v_sPropertyName:="Full_time_employment_ind", v_sOIKey:=sKey, r_vPropertyValue:=sOcc_FullTimeEmploymentInd, r_bIsAssumedInfo:=bIsAssumedInfo)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        ' OCCUPATION, code
                        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "OCCUPATION", v_sPropertyName:="code", v_sOIKey:=sKey, r_vPropertyValue:=sOcc_Code, r_bIsAssumedInfo:=bIsAssumedInfo)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        ' Get text for this ABI code
                        m_lReturn = oList.GetDescription("2228226", sOcc_Code, sOcc_Code)
                        '    If m_lReturn <> PMTrue Then
                        '        GenerateCustomerEmail = PMFalse
                        '        Exit Function
                        '    End If

                        ' OCCUPATION, Employers Business
                        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "OCCUPATION", v_sPropertyName:="Employers_Business", v_sOIKey:=sKey, r_vPropertyValue:=sOcc_EmployersBusiness, r_bIsAssumedInfo:=bIsAssumedInfo)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If sOcc_EmployersBusiness = "186" Then
                            sOcc_FullTimeEmploymentInd = "Y"
                        End If

                        ' Get text for this ABI code
                        m_lReturn = oList.GetDescription("2228228", sOcc_EmployersBusiness, sOcc_EmployersBusiness)


                        If sOcc_FullTimeEmploymentInd = "Y" Then
                            sOcc_MainCode = sOcc_Code
                            sOcc_MainEmployersBusiness = sOcc_EmployersBusiness
                        Else
                            sOcc_PartTimeCode = sOcc_Code
                            sOcc_PartTimeEmployersBusiness = sOcc_EmployersBusiness
                        End If

                        iOccupationKey += 1

                    Loop
                Else

                    sKey = CStr(vKeyArray(0))

                    ' OCCUPATION, code
                    m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "OCCUPATION", v_sPropertyName:="code", v_sOIKey:=sKey, r_vPropertyValue:=sOcc_MainCode, r_bIsAssumedInfo:=bIsAssumedInfo)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Get text for this ABI code
                    m_lReturn = oList.GetDescription("2228226", sOcc_MainCode, sOcc_MainCode)
                    '    If m_lReturn <> PMTrue Then
                    '        GenerateCustomerEmail = PMFalse
                    '        Exit Function
                    '    End If

                    ' OCCUPATION, Employers Business
                    m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "OCCUPATION", v_sPropertyName:="Employers_Business", v_sOIKey:=sKey, r_vPropertyValue:=sOcc_MainEmployersBusiness, r_bIsAssumedInfo:=bIsAssumedInfo)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Get text for this ABI code
                    m_lReturn = oList.GetDescription("2228228", sOcc_MainEmployersBusiness, sOcc_MainEmployersBusiness)
                End If

                ' Named Drivers
                sMsg = sMsg & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
                If iDriverKey = 1 Then
                    sMsg = sMsg & "Named Drivers" & Strings.ChrW(13) & Strings.ChrW(10)
                    sMsg = sMsg & "-------------" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
                End If
                sMsg = sMsg & "Name: " & s & Strings.ChrW(13) & Strings.ChrW(10)
                sMsg = sMsg & "Relationship to proposer: " & sDriver_Relationship & Strings.ChrW(13) & Strings.ChrW(10)
                sMsg = sMsg & "Sex: " & sDriver_Sex & Strings.ChrW(13) & Strings.ChrW(10)
                sMsg = sMsg & "Marital Status: " & sDriver_MaritalStatus & Strings.ChrW(13) & Strings.ChrW(10)
                sMsg = sMsg & "Date Of Birth (Age): " & sDriver_DateOfBirth & " (" & sDriver_Age & " years)" & Strings.ChrW(13) & Strings.ChrW(10)
                sMsg = sMsg & "Occupation: " & sOcc_MainCode & Strings.ChrW(13) & Strings.ChrW(10)
                sMsg = sMsg & "Employer's Business: " & sOcc_MainEmployersBusiness & Strings.ChrW(13) & Strings.ChrW(10)

                If sOcc_PartTimeCode <> "" Then
                    sMsg = sMsg & "Part Time Occupation: " & sOcc_PartTimeCode & Strings.ChrW(13) & Strings.ChrW(10)
                    sMsg = sMsg & "Part Time Employer's Business: " & sOcc_PartTimeEmployersBusiness & Strings.ChrW(13) & Strings.ChrW(10)
                End If

                sMsg = sMsg & "Licence Type: " & sDriver_LicenceType & Strings.ChrW(13) & Strings.ChrW(10)
                '            sMsg = sMsg & "Number of years licence held: " & CStr((DateDiff("d", sDriver_LicenceDate, Now) - (DateDiff("d", sDriver_LicenceDate, Now))) / 365) & vbCrLf
                sMsg = sMsg & "Number of years licence held: " & sDriver_LicenceDate & Strings.ChrW(13) & Strings.ChrW(10)


                ' Get Claims and convictions for this driver
                m_lReturn = GetDriverIncidents(sDriverKey, oList, sMsg)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDriverIncidents Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GenerateCustomerEmail")
                    Return result
                End If

            End If

            iDriverKey += 1

        Loop

        ' Main Driver
        sMsg = sMsg & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Main Driver" & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "-----------" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Name: " & sDriver_MainDriver & Strings.ChrW(13) & Strings.ChrW(10)

        sMsg = sMsg & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)

        sMsg = sMsg & "You or any person covered by this policy have never had insurance" & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "declined or refused, or had special terms imposed." & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "You or any person covered by this policy are not suffering from any " & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "medical condition of which the DVLA needs to be informed." & Strings.ChrW(13) & Strings.ChrW(10)

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Vehicle
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


        m_lReturn = m_oDataSet.GetAllOIKey(ACDataModelCodePrefix & "Vehicle", vKeyArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        sKey = CStr(vKeyArray(0))

        ' Get all reqd properties from vehicle

        ' Vehicle, Manufacturer
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "VEHICLE", v_sPropertyName:="Manufacturer", v_sOIKey:=sKey, r_vPropertyValue:=sVehicle_Manufacturer, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get text for this ABI code
        m_lReturn = oList.GetDescription("393219", sVehicle_Manufacturer, sVehicle_Manufacturer)
        '    If m_lReturn <> PMTrue Then
        '        GenerateCustomerEmail = PMFalse
        '        Exit Function
        '    End If


        ' Vehicle, Model
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "VEHICLE", v_sPropertyName:="Model", v_sOIKey:=sKey, r_vPropertyValue:=sVehicle_Model, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get text for this ABI code
        m_lReturn = oList.GetDescription("393220", sVehicle_Model, sVehicle_Model)
        '    If m_lReturn <> PMTrue Then
        '        GenerateCustomerEmail = PMFalse
        '        Exit Function
        '    End If

        ' We just want the model name, not the other details!
        sVehicle_Model = sVehicle_Model.Substring(sVehicle_Model.Length - (sVehicle_Model.Length - 28))

        ' Vehicle, Annual Mileage
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "VEHICLE", v_sPropertyName:="Annual_Mileage", v_sOIKey:=sKey, r_vPropertyValue:=sVehicle_AnnualMileage, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Vehicle, Value
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "VEHICLE", v_sPropertyName:="Value", v_sOIKey:=sKey, r_vPropertyValue:=sVehicle_Value, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        ' Vehicle, Year_Manufactured
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "VEHICLE", v_sPropertyName:="Year_Manufactured", v_sOIKey:=sKey, r_vPropertyValue:=sVehicle_YearManufactured, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Vehicle, Reg
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "VEHICLE", v_sPropertyName:="Reg_No", v_sOIKey:=sKey, r_vPropertyValue:=sVehicle_RegNo, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Vehicle, Location Kept Overnight
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "VEHICLE", v_sPropertyName:="Location_Kept_Overnight", v_sOIKey:=sKey, r_vPropertyValue:=sVehicle_LocationKeptOvernight, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get text for this ABI code
        m_lReturn = oList.GetDescription("393234", sVehicle_LocationKeptOvernight, sVehicle_LocationKeptOvernight)
        '    If m_lReturn <> PMTrue Then
        '        GenerateCustomerEmail = PMFalse
        '        Exit Function
        '    End If


        ' Vehicle, Postcode
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "VEHICLE", v_sPropertyName:="Post_code", v_sOIKey:=sKey, r_vPropertyValue:=sVehicle_Postcode, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Vehicle, Ownership
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "VEHICLE", v_sPropertyName:="Ownership", v_sOIKey:=sKey, r_vPropertyValue:=sVehicle_Ownership, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get text for this ABI code
        m_lReturn = oList.GetDescription("393241", sVehicle_Ownership, sVehicle_Ownership)
        '    If m_lReturn <> PMTrue Then
        '        GenerateCustomerEmail = PMFalse
        '        Exit Function
        '    End If


        ' Vehicle, Keeper
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "VEHICLE", v_sPropertyName:="Keeper", v_sOIKey:=sKey, r_vPropertyValue:=sVehicle_Keeper, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get text for this ABI code
        m_lReturn = oList.GetDescription("393303", sVehicle_Keeper, sVehicle_Keeper)
        '    If m_lReturn <> PMTrue Then
        '        GenerateCustomerEmail = PMFalse
        '        Exit Function
        '    End If


        '    If m_lReturn <> PMTrue Then
        '        GenerateCustomerEmail = PMFalse
        '        Exit Function
        '    End If

        ' Vehicle, Tracker Device Fitted
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "VEHICLE", v_sPropertyName:="Tracker_device_fitted_ind", v_sOIKey:=sKey, r_vPropertyValue:=sVehicle_TrackerFitted, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Security
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If sVehicle_TrackerFitted = "Y" Then
            sSecurity_MakeAndModel = "Tracker"
        Else

            m_lReturn = m_oDataSet.GetAllOIKey(ACDataModelCodePrefix & "Security", vKeyArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            sKey = CStr(vKeyArray(0))

            ' Get all reqd properties from vehicle

            ' Security, Make and Model
            m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "SECURITY", v_sPropertyName:="System_make_and_model", v_sOIKey:=sKey, r_vPropertyValue:=sSecurity_MakeAndModel, r_bIsAssumedInfo:=bIsAssumedInfo)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get text for this ABI code
            m_lReturn = oList.GetDescription("589832", sSecurity_MakeAndModel, sSecurity_MakeAndModel)
        End If

        ' Your Car
        sMsg = sMsg & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Your Car" & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "--------" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Manufacturer: " & sVehicle_Manufacturer & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Model: " & sVehicle_Model & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Annual Mileage: " & sVehicle_AnnualMileage & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Estimated Value: " & Strings.ChrW(163).ToString() & StringsHelper.Format(sVehicle_Value, "#####0.00") & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Year of Manufacture: " & sVehicle_YearManufactured & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Registration: " & sVehicle_RegNo & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Security: " & sSecurity_MakeAndModel & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Overnight Location: " & sVehicle_LocationKeptOvernight & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Risk Postcode: " & sVehicle_Postcode & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Car Owner: " & sVehicle_Ownership & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Registered Keeper: " & sVehicle_Keeper & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "The vehicle has not been modified in any way." & Strings.ChrW(13) & Strings.ChrW(10)


        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Add insurer specific e-mail footer - CL090800
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        sMsg = sMsg & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)


        ' Fetch path from reg
        m_lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sSettingName:=GISSharedConstants.GISRegNBSOFEmailFooterPath, r_sSettingValue:=sFooterPath, v_sSubKey:=GISSharedConstants.GISRegSubKeyEmails)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        ' Add insurer specific file onto end of email
        sPath = sFooterPath & "\" & v_sInsurerNo & "_footer.txt"

        ' Does file exist?
        If Directory.GetFiles(sPath, FileAttribute.Normal).Equals("") = False Then

            File.Open(sPath, OpenMode.Input)
            File.AppendAllText(sPath, sMsg & s & Strings.ChrW(13) & Strings.ChrW(10))
            FileClose()

        Else
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SOF EMail Footer file does not exist : " & sPath, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GenerateCustomerEmail")
        End If

        sMsg = sMsg & Strings.ChrW(13) & Strings.ChrW(10)

        oList = Nothing


        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Now send the email
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Developer guide no. 205

        oCDONTS = New Object

        ' If we are running on PWS then we cannot sent the Mail
        ' as CDONTS and the SMTP service is NOT available.

        If oCDONTS Is Nothing Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Set up the sender, recipient, subject & text message


        oCDONTS.To = sProposer_Email ' from the XML

        oCDONTS.From = v_sFrom

        oCDONTS.Subject = v_sSubject

        oCDONTS.Body = sMsg


        'Send the message


        oCDONTS.send()

        oCDONTS = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdateLeadInsurer
    '
    ' Description: Updates the lead insurer on an insurance file
    '
    ' History: TF020908 - Created in bGISBOMSFB
    '          CTAF 20030218 - Copied into bGISBOMAOL
    '
    ' ***************************************************************** '
    Public Function UpdateLeadInsurer(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lLeadInsurerCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim oInsuranceFile As bSIRInsuranceFile.Services

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get insurancefile services
            oInsuranceFile = New bSIRInsuranceFile.Services
            m_lReturn = oInsuranceFile.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the insurance file

            oInsuranceFile.InsuranceFileCnt = v_lInsuranceFileCnt


            oInsuranceFile.LeadInsurerCnt = v_lLeadInsurerCnt

            ' Update the details

            m_lReturn = oInsuranceFile.UpdatePolicy()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oInsuranceFile.Dispose()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Terminate and clear up

            oInsuranceFile.Dispose()
            oInsuranceFile = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateLeadInsurer Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="UpdateLeadInsurer", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result


            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: UpdatePartyCnt
    ' Date: 03/07/2000
    '
    ' Description: Update the ins holder cnt of the ins.folder for a
    ' given ins.file and party cnt
    '
    ' Author: CL
    ' ***************************************************************** '

    Public Function UpdatePartyCnt(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim m_lReturn As gPMConstants.PMEReturnCode
        'Modified by Vijay Pal on 5/25/2010 1:43:29 PM todolist,iteration3,and declare Dim oSBOLink As Object 
        'Dim oSBOLink As bSIRIUSLink.SIRIUSLink
        Dim oSBOLink As bSIRIUSLink.SIRIUSLink
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'Modified by Vijay Pal on 5/25/2010 1:44:42 PM todolist,iteration3,and declare  oSBOLink = New Object
            oSBOLink = New bSIRIUSLink.SIRIUSLink()
            'oSBOLink = New Object
            m_lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            m_lReturn = oSBOLink.UpdatePartyCnt(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFileCnt:=v_lInsuranceFileCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            oSBOLink = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message

            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePartyCnt Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="UpdatePartyCnt", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result


            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
            Return result
        End Try
    End Function


    ''''''''''''''''''''''''''''''''''''''''
    ' Get Claims and convictions for given driver
    ''''''''''''''''''''''''''''''''''''''''
    Private Function GetDriverIncidents(ByRef sDriverKey As String, ByRef oList As bGISListManager.InterfaceNoLogin, ByRef sMsg As String) As Integer

        Dim result As Integer = 0
        Const sTab As String = "    "

        Dim vKeyArray As Object
        Dim vIncidentKeyArray As Object
        Dim iKey, iIncidentKey As Integer
        Dim sIncidentKey, sKey As String
        Dim bIsAssumedInfo As Boolean

        Dim sClaim_Type, sClaim_Date, sClaim_Cost, sClaim_NCDLost, sClaim_AtFault As String

        Dim sConviction_Date, sConviction_Code, sConviction_PenaltyPoints, sConviction_PenaltyAmt, sConviction_SuspensionPeriod As String

        Dim sConvictionsMsg As New StringBuilder
        Dim sAccidentsMsg As New StringBuilder



        result = gPMConstants.PMEReturnCode.PMTrue

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Incidents - RAG210600
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        m_lReturn = m_oDataSet.GetChildOIKey(ACDataModelCodePrefix & "DRIVER", sDriverKey, ACDataModelCodePrefix & "INCIDENT", vIncidentKeyArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        iIncidentKey = 0
        ' Loop for all incidents
        If Informations.IsArray(vIncidentKeyArray) Then

            Do While (vIncidentKeyArray.GetUpperBound(0) >= iIncidentKey)


                sIncidentKey = CStr(vIncidentKeyArray(iIncidentKey))

                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ' Claims - RAG210600
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                m_lReturn = m_oDataSet.GetChildOIKey(ACDataModelCodePrefix & "INCIDENT", sIncidentKey, ACDataModelCodePrefix & "Claim", vKeyArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                iKey = 0

                ' Loop for all claims
                If Informations.IsArray(vKeyArray) Then

                    Do While (vKeyArray.GetUpperBound(0) >= iKey)


                        sKey = CStr(vKeyArray(iKey))

                        ' Claim, Type
                        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "CLAIM", v_sPropertyName:="Claim_type", v_sOIKey:=sKey, r_vPropertyValue:=sClaim_Type, r_bIsAssumedInfo:=bIsAssumedInfo)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If sClaim_Type.Trim() <> "" Then

                            m_lReturn = oList.GetDescription("1179649", sClaim_Type, sClaim_Type)

                            ' Claim, Date
                            m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "CLAIM", v_sPropertyName:="Claim_date", v_sOIKey:=sKey, r_vPropertyValue:=sClaim_Date, r_bIsAssumedInfo:=bIsAssumedInfo)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            Dim TempDate As Date
                            sClaim_Date = If(DateTime.TryParse(sClaim_Date, TempDate), TempDate.ToString("MM/yyyy"), sClaim_Date)

                            ' Claim, cost
                            m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "CLAIM", v_sPropertyName:="Costs_total", v_sOIKey:=sKey, r_vPropertyValue:=sClaim_Cost, r_bIsAssumedInfo:=bIsAssumedInfo)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            ' Claim, NCD_Lost
                            m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "CLAIM", v_sPropertyName:="NCD_lost_ind", v_sOIKey:=sKey, r_vPropertyValue:=sClaim_NCDLost, r_bIsAssumedInfo:=bIsAssumedInfo)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            ' Claim, At_Fault
                            m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "CLAIM", v_sPropertyName:="Driver_at_fault_ind", v_sOIKey:=sKey, r_vPropertyValue:=sClaim_AtFault, r_bIsAssumedInfo:=bIsAssumedInfo)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            ' Claims
                            sAccidentsMsg.Append(Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10))
                            If iKey = 0 Then
                                sAccidentsMsg.Append(sTab & "Accidents, Incidents or Losses" & Strings.ChrW(13) & Strings.ChrW(10))
                                sAccidentsMsg.Append(sTab & "------------------------------" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10))
                            End If
                            sAccidentsMsg.Append(sTab & "Type of Accident, Incident or Loss: " & sClaim_Type & Strings.ChrW(13) & Strings.ChrW(10))
                            sAccidentsMsg.Append(sTab & "Date of Accident, Incident or Loss: " & sClaim_Date & Strings.ChrW(13) & Strings.ChrW(10))
                            sAccidentsMsg.Append(sTab & "Cost of Resulting Claim Made: " & Strings.ChrW(163).ToString() & StringsHelper.Format(sClaim_Cost, "####0.00") & Strings.ChrW(13) & Strings.ChrW(10))
                            sAccidentsMsg.Append(sTab & "No Claims Discount Lost?: " & sClaim_NCDLost & Strings.ChrW(13) & Strings.ChrW(10))
                            sAccidentsMsg.Append(sTab & "Driver At Fault?: " & sClaim_AtFault & Strings.ChrW(13) & Strings.ChrW(10))

                        End If

                        iKey += 1
                    Loop

                End If

                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ' Convictions - RAG210600
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                m_lReturn = m_oDataSet.GetChildOIKey(ACDataModelCodePrefix & "INCIDENT", sIncidentKey, ACDataModelCodePrefix & "Conviction", vKeyArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                iKey = 0

                ' Loop for all claims
                If Informations.IsArray(vKeyArray) Then

                    Do While (vKeyArray.GetUpperBound(0) >= iKey)


                        sKey = CStr(vKeyArray(iKey))

                        ' Conviction, Type
                        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "CONVICTION", v_sPropertyName:="Code", v_sOIKey:=sKey, r_vPropertyValue:=sConviction_Code, r_bIsAssumedInfo:=bIsAssumedInfo)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If sConviction_Code.Trim() <> "" Then

                            m_lReturn = oList.GetDescription("1245186", sConviction_Code, sConviction_Code)

                            ' Conviction, Date
                            m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "CONVICTION", v_sPropertyName:="Date", v_sOIKey:=sKey, r_vPropertyValue:=sConviction_Date, r_bIsAssumedInfo:=bIsAssumedInfo)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            Dim TempDate2 As Date
                            sConviction_Date = If(DateTime.TryParse(sConviction_Date, TempDate2), TempDate2.ToString("MM/yyyy"), sConviction_Date)

                            ' Conviction, Penalty_points
                            m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "CONVICTION", v_sPropertyName:="Penalty_pts", v_sOIKey:=sKey, r_vPropertyValue:=sConviction_PenaltyPoints, r_bIsAssumedInfo:=bIsAssumedInfo)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            ' Conviction, Penalty_amt
                            m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "CONVICTION", v_sPropertyName:="Penalty_amt", v_sOIKey:=sKey, r_vPropertyValue:=sConviction_PenaltyAmt, r_bIsAssumedInfo:=bIsAssumedInfo)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            ' Conviction, Suspension_period
                            m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "CONVICTION", v_sPropertyName:="Suspension_period", v_sOIKey:=sKey, r_vPropertyValue:=sConviction_SuspensionPeriod, r_bIsAssumedInfo:=bIsAssumedInfo)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If


                            ' Convictions
                            sConvictionsMsg.Append(Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10))
                            If iKey = 0 Then
                                sConvictionsMsg.Append(sTab & "Convictions (including pending convictions)" & Strings.ChrW(13) & Strings.ChrW(10))
                                sConvictionsMsg.Append(sTab & "-------------------------------------------" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10))
                            End If
                            sConvictionsMsg.Append(sTab & "Date of Conviction: " & sConviction_Date & Strings.ChrW(13) & Strings.ChrW(10))
                            sConvictionsMsg.Append(sTab & "Conviction Code: " & sConviction_Code & Strings.ChrW(13) & Strings.ChrW(10))
                            sConvictionsMsg.Append(sTab & "Number of Penalty Points (if any): " & sConviction_PenaltyPoints & Strings.ChrW(13) & Strings.ChrW(10))
                            sConvictionsMsg.Append(sTab & "Amount of Fine (if any): " & Strings.ChrW(163).ToString() & StringsHelper.Format(sConviction_PenaltyAmt, "####0.00") & Strings.ChrW(13) & Strings.ChrW(10))
                            sConvictionsMsg.Append(sTab & "Period of Supsension (if any): " & sConviction_SuspensionPeriod & Strings.ChrW(13) & Strings.ChrW(10))
                        End If

                        iKey += 1
                    Loop

                End If

                iIncidentKey += 1

            Loop

        End If


        ' check accidents and convictions messages to see if user has any
        ' if not add none to the message

        If sAccidentsMsg.ToString() <> "" Then

            sMsg = sMsg & sAccidentsMsg.ToString()

        Else

            ' add header with none if user has no accidents
            sMsg = sMsg & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
            sMsg = sMsg & sTab & "Accidents, Incidents or Losses" & Strings.ChrW(13) & Strings.ChrW(10)
            sMsg = sMsg & sTab & "------------------------------" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
            sMsg = sMsg & sTab & "None" & Strings.ChrW(13) & Strings.ChrW(10)

        End If

        If sConvictionsMsg.ToString() <> "" Then

            sMsg = sMsg & sConvictionsMsg.ToString()

        Else

            ' add header with none if user has no claims
            sMsg = sMsg & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
            sMsg = sMsg & sTab & "Convictions (including pending convictions)" & Strings.ChrW(13) & Strings.ChrW(10)
            sMsg = sMsg & sTab & "-------------------------------------------" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
            sMsg = sMsg & sTab & "None" & Strings.ChrW(13) & Strings.ChrW(10)

        End If

        Return result

    End Function

    ''''''''''''''''''''''''''''''''''''''''
    ' Get Claims and convictions for given driver
    ''''''''''''''''''''''''''''''''''''''''
    Private Function GetNBBordereauxDriverIncidents(ByVal v_sDriverKey As String, ByRef r_vIncidentsArray(,) As Object, ByRef r_iIncidentsCount As Integer, ByRef r_vAccidentsArray(,) As Object, ByRef r_iAccidentsCount As Integer, ByVal v_sDriverName As String) As Integer

        Dim result As Integer = 0
        Dim vKeyArray As Object
        Dim iKey, iIncidentKey As Integer
        Dim sIncidentKey, sKey As String
        Dim bIsAssumedInfo As Boolean

        Dim sConvictionCode, sClaimType, sConvictionPenaltyAmt, sConvictionSuspensionPeriod As String

        Dim vIncidentKeyArray As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Incidents
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        m_lReturn = m_oDataSet.GetChildOIKey(ACDataModelCodePrefix & "DRIVER", v_sDriverKey, ACDataModelCodePrefix & "INCIDENT", vIncidentKeyArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        iIncidentKey = 0
        ' Loop for all incidents
        If Informations.IsArray(vIncidentKeyArray) Then

            Do While (vIncidentKeyArray.GetUpperBound(0) >= iIncidentKey)


                sIncidentKey = CStr(vIncidentKeyArray(iIncidentKey))

                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ' Claims
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                m_lReturn = m_oDataSet.GetChildOIKey(ACDataModelCodePrefix & "INCIDENT", sIncidentKey, ACDataModelCodePrefix & "Claim", vKeyArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                iKey = 0

                ' Loop for all claims
                If Informations.IsArray(vKeyArray) Then

                    Do While (vKeyArray.GetUpperBound(0) >= iKey And r_iAccidentsCount < 4)


                        sKey = CStr(vKeyArray(iKey))

                        ' Claim, Type
                        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "CLAIM", v_sPropertyName:="Claim_type", v_sOIKey:=sKey, r_vPropertyValue:=sClaimType, r_bIsAssumedInfo:=bIsAssumedInfo)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If sClaimType.Trim() = "A" Then


                            r_vAccidentsArray(0, r_iAccidentsCount) = v_sDriverName

                            ' Claim, Date

                            m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "CLAIM", v_sPropertyName:="Claim_date", v_sOIKey:=sKey, r_vPropertyValue:=r_vAccidentsArray(1, r_iAccidentsCount), r_bIsAssumedInfo:=bIsAssumedInfo)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If


                            r_vAccidentsArray(1, r_iAccidentsCount) = CDate(r_vAccidentsArray(1, r_iAccidentsCount)).ToString("MM/yyyy")

                            ' Claim, Costs Total

                            m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "CLAIM", v_sPropertyName:="Costs_total", v_sOIKey:=sKey, r_vPropertyValue:=r_vAccidentsArray(2, r_iAccidentsCount), r_bIsAssumedInfo:=bIsAssumedInfo)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            ' Claim, Driver at fault ind

                            m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "CLAIM", v_sPropertyName:="Driver_at_fault_ind", v_sOIKey:=sKey, r_vPropertyValue:=r_vAccidentsArray(3, r_iAccidentsCount), r_bIsAssumedInfo:=bIsAssumedInfo)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            r_iAccidentsCount += 1
                        End If

                        iKey += 1
                    Loop
                End If

                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ' Convictions
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                m_lReturn = m_oDataSet.GetChildOIKey(ACDataModelCodePrefix & "INCIDENT", sIncidentKey, ACDataModelCodePrefix & "Conviction", vKeyArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                iKey = 0

                ' Loop for all claims
                If Informations.IsArray(vKeyArray) Then

                    Do While (vKeyArray.GetUpperBound(0) >= iKey And r_iIncidentsCount < 4)


                        sKey = CStr(vKeyArray(iKey))

                        ' Conviction, Type
                        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "CONVICTION", v_sPropertyName:="Code", v_sOIKey:=sKey, r_vPropertyValue:=sConvictionCode, r_bIsAssumedInfo:=bIsAssumedInfo)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If sConvictionCode.Trim() <> "" Then


                            r_vIncidentsArray(0, r_iIncidentsCount) = v_sDriverName

                            ' Conviction, Date

                            m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "CONVICTION", v_sPropertyName:="Date", v_sOIKey:=sKey, r_vPropertyValue:=r_vIncidentsArray(1, r_iIncidentsCount), r_bIsAssumedInfo:=bIsAssumedInfo)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If


                            r_vIncidentsArray(1, r_iIncidentsCount) = CDate(r_vIncidentsArray(1, r_iIncidentsCount)).ToString("MM/yyyy")


                            r_vIncidentsArray(2, r_iIncidentsCount) = sConvictionCode

                            ' Conviction, Penalty_amt
                            m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "CONVICTION", v_sPropertyName:="Penalty_amt", v_sOIKey:=sKey, r_vPropertyValue:=sConvictionPenaltyAmt, r_bIsAssumedInfo:=bIsAssumedInfo)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            ' Conviction, Suspension_period
                            m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "CONVICTION", v_sPropertyName:="Suspension_period", v_sOIKey:=sKey, r_vPropertyValue:=sConvictionSuspensionPeriod, r_bIsAssumedInfo:=bIsAssumedInfo)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If


                            r_vIncidentsArray(3, r_iIncidentsCount) = (sConvictionPenaltyAmt & " " & sConvictionSuspensionPeriod).Trim()

                            r_iIncidentsCount += 1

                        End If

                        iKey += 1
                    Loop
                End If

                iIncidentKey += 1
            Loop
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GenerateBordereaux
    '
    ' Description: Build a summary of policy sold
    '
    ' Date: RJG 28/06/2000
    '
    ' ***************************************************************** '
    Private Function GenerateBordereaux(ByVal v_lInsurerNo As Integer, ByVal v_lMTAQuoteType As Integer) As Integer

        Dim result As Integer = 0


        Dim vKeyArray As Object
        Dim sKey As String = ""
        Dim bIsAssumedInfo As Boolean

        Dim sFilename As String = ""
        Dim hFile As Integer

        Dim sPremiumIncIPT, sCommissionAmount As String

        Dim sPolicyNumber, sClientForename, sClientSurname, sClientName, sInceptionDate, sEffectiveDate As String
        Dim dblGrossPremium As Double
        Dim sIPT, sTransactionCode, sPostcode, sCoverType, sClassOfUse, sDriverRestriction, sPolicyHolderDOB, sPolicyHolderAge, sVoluntaryXS, sCompulsoryXS, sNCDLevel, sProtectedNCD, sVehicleReg, sVehicleValue, sEngineSize, sVehicleManufacturer, sVehicleModel, sVehicleModelName, sVehicleDetails As String
        Dim sFieldDelimiter As String = ""

        'SPW 11/10/00
        Dim oList As bGISListManager.InterfaceNoLogin


        'SPW 11/10/2000 Added to get descriptions from listmanager
        oList = New bGISListManager.InterfaceNoLogin()
        If oList Is Nothing Then
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error creating list manager", vApp:=ToSafeString(ACApp), vClass:="", vMethod:="GenerateBordereaux")
            Return result
        End If

        'SPW 11/10/2000
        m_lReturn = oList.Initialise()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error initialising List Manager", vApp:=ToSafeString(ACApp), vClass:="", vMethod:="GenerateBordereaux")
            Return result
        End If

        m_lReturn = oList.CheckListVersions(ACDataModelCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error in CheckListVersions", vApp:=ToSafeString(ACApp), vClass:="", vMethod:="GenerateBordereaux")
            Return result
        End If

        result = gPMConstants.PMEReturnCode.PMTrue

        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Inside GenerateBordereaux...", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GenerateBordereaux")

        sFieldDelimiter = ","

        ' RFC050800 - Set the Trans Type Code to CAN for cancellations
        If v_lMTAQuoteType = GISSharedConstants.GISMTAQuoteTypeAddCancellation Then
            sTransactionCode = "CAN"
        Else
            sTransactionCode = "MTA"
        End If

        'Get the Bordereaux information from the dataset
        '------------------------------------------------------
        'POLICY
        '------------------------------------------------------

        m_lReturn = m_oDataSet.GetAllOIKey(ACDataModelCodePrefix & "Policy", vKeyArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsArray(vKeyArray) Then
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OI Key array is empty for " & ACDataModelCodePrefix & "Policy", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GenerateBordereaux")
        End If


        sKey = CStr(vKeyArray(0))

        ' Policy Number
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "POLICY", v_sPropertyName:="Policy_No", v_sOIKey:=sKey, r_vPropertyValue:=sPolicyNumber, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Inception Date
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "POLICY", v_sPropertyName:="Inception_Date", v_sOIKey:=sKey, r_vPropertyValue:=sInceptionDate, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Effective Date
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "POLICY", v_sPropertyName:="Effective_start_date", v_sOIKey:=sKey, r_vPropertyValue:=sEffectiveDate, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Commission Amount
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "POLICY", v_sPropertyName:="Commission_pct", v_sOIKey:=sKey, r_vPropertyValue:=sCommissionAmount, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        ' Compulsory XS
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "POLICY", v_sPropertyName:="Policy_Excess", v_sOIKey:=sKey, r_vPropertyValue:=sCompulsoryXS, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Voluntary XS
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "POLICY", v_sPropertyName:="Vol_Excess", v_sOIKey:=sKey, r_vPropertyValue:=sVoluntaryXS, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' RFC030800 - Changed from the NP_QUOTE Object,
        ' however I think that this should be coming from the
        ' Calculated Result Object
        ' IPT
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "POLICY", v_sPropertyName:="Policy_ipt", v_sOIKey:=sKey, r_vPropertyValue:=sIPT, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '------------------------------------------------------
        'CALCULATED_RESULT
        '------------------------------------------------------

        m_lReturn = m_oDataSet.GetAllOIKey(ACDataModelCodePrefix & "Calculated_result", vKeyArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsArray(vKeyArray) Then
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OI Key array is empty for " & ACDataModelCodePrefix & "Calculated_result", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GenerateBordereaux")
        End If


        sKey = CStr(vKeyArray(0))

        ' Premium Including IPT
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "CALCULATED_RESULT", v_sPropertyName:="Premium_incl_ipt", v_sOIKey:=sKey, r_vPropertyValue:=sPremiumIncIPT, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '------------------------------------------------------
        'PROPOSER_POLICYHOLDER
        '------------------------------------------------------

        m_lReturn = m_oDataSet.GetAllOIKey(ACDataModelCodePrefix & "Proposer_Policyholder", vKeyArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsArray(vKeyArray) Then
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OI Key array is empty for " & ACDataModelCodePrefix & "Proposer_Policyholder", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GenerateBordereaux")
        End If


        sKey = CStr(vKeyArray(0))

        ' Client Forename
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "PROPOSER_POLICYHOLDER", v_sPropertyName:="Forename_initial_1", v_sOIKey:=sKey, r_vPropertyValue:=sClientForename, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Client Surname
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "PROPOSER_POLICYHOLDER", v_sPropertyName:="Surname", v_sOIKey:=sKey, r_vPropertyValue:=sClientSurname, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' PolicyHolder DOB
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "PROPOSER_POLICYHOLDER", v_sPropertyName:="Date_of_birth", v_sOIKey:=sKey, r_vPropertyValue:=sPolicyHolderDOB, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '------------------------------------------------------
        'VEHICLE
        '------------------------------------------------------


        m_lReturn = m_oDataSet.GetAllOIKey(ACDataModelCodePrefix & "Vehicle", vKeyArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsArray(vKeyArray) Then
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OI Key array is empty for " & ACDataModelCodePrefix & "Vehicle", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GenerateBordereaux")
        End If


        sKey = CStr(vKeyArray(0))

        ' Postcode where vehicle is being kept
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "VEHICLE", v_sPropertyName:="Post_code", v_sOIKey:=sKey, r_vPropertyValue:=sPostcode, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Class of use
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "VEHICLE", v_sPropertyName:="Class_of_use", v_sOIKey:=sKey, r_vPropertyValue:=sClassOfUse, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'SPW 11/10/2000 get description from listmanager
        m_lReturn = oList.GetDescription("393239", sClassOfUse, sClassOfUse)

        ' Reg No
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "VEHICLE", v_sPropertyName:="Reg_no", v_sOIKey:=sKey, r_vPropertyValue:=sVehicleReg, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Vehicle Value
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "VEHICLE", v_sPropertyName:="Value", v_sOIKey:=sKey, r_vPropertyValue:=sVehicleValue, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Engine Size
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "VEHICLE", v_sPropertyName:="Cubic_capacity", v_sOIKey:=sKey, r_vPropertyValue:=sEngineSize, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Vehicle Manufacturer
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "VEHICLE", v_sPropertyName:="Manufacturer", v_sOIKey:=sKey, r_vPropertyValue:=sVehicleManufacturer, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Vehicle Model
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "VEHICLE", v_sPropertyName:="Model", v_sOIKey:=sKey, r_vPropertyValue:=sVehicleModel, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Vehicle Model name
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "VEHICLE", v_sPropertyName:="Model_name", v_sOIKey:=sKey, r_vPropertyValue:=sVehicleModelName, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '------------------------------------------------------
        'COVER
        '------------------------------------------------------


        m_lReturn = m_oDataSet.GetAllOIKey(ACDataModelCodePrefix & "Cover", vKeyArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsArray(vKeyArray) Then
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OI Key array is empty for " & ACDataModelCodePrefix & "Cover", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GenerateBordereaux")
        End If


        sKey = CStr(vKeyArray(0))

        ' Cover code
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "COVER", v_sPropertyName:="Code", v_sOIKey:=sKey, r_vPropertyValue:=sCoverType, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'SPW 11/10/2000 get description from listmanager
        m_lReturn = oList.GetDescription("851969", sCoverType, sCoverType)

        ' Driver Restriction
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "COVER", v_sPropertyName:="Required_drivers", v_sOIKey:=sKey, r_vPropertyValue:=sDriverRestriction, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'SPW 11/10/2000 get description from listmanager
        m_lReturn = oList.GetDescription("852055", sDriverRestriction, sDriverRestriction)

        '------------------------------------------------------
        'NCD
        '------------------------------------------------------


        m_lReturn = m_oDataSet.GetAllOIKey(ACDataModelCodePrefix & "NCD", vKeyArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsArray(vKeyArray) Then
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OI Key array is empty for " & ACDataModelCodePrefix & "NCD", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GenerateBordereaux")
        End If


        sKey = CStr(vKeyArray(0))

        ' NCD Level
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "NCD", v_sPropertyName:="Claimed_years_earned", v_sOIKey:=sKey, r_vPropertyValue:=sNCDLevel, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Protected NCD
        m_lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=ACDataModelCodePrefix & "NCD", v_sPropertyName:="Claimed_protection_reqd_ind", v_sOIKey:=sKey, r_vPropertyValue:=sProtectedNCD, r_bIsAssumedInfo:=bIsAssumedInfo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If sPremiumIncIPT = "" Then
            sPremiumIncIPT = "0"
        End If

        If sCommissionAmount = "" Then
            sCommissionAmount = "0"
        End If

        sCommissionAmount = CStr(CDbl(sPremiumIncIPT) * CDbl(sCommissionAmount) / 100)

        dblGrossPremium = CDbl(sPremiumIncIPT) + CDbl(sCommissionAmount)

        sClientName = (sClientForename & " " & sClientSurname).Trim()

        'sVehicleDetails = Trim$(sVehicleManufacturer & " " & sVehicleModel & " " & sVehicleModelName)
        ' SPW 11/10/2000 Model Description only required
        sVehicleDetails = sVehicleModelName.Trim()

        '------------------------------------------------------
        'NP_QUOTE - for filename of report
        '------------------------------------------------------

        ' Use the InsurerNo for the Filename
        sFilename = CStr(v_lInsurerNo)

        If sFilename = "" Then
            sFilename = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) & "\Bordereaux.inf"
        Else
            sFilename = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) & "\" & sFilename & ".inf"
        End If

        Dim F As FileStream
        ' FileMode.Truncate also works, but the file needs to exist from before
        F = File.Open(sFilename, FileMode.OpenOrCreate, FileAccess.Write)
        F.SetLength(0) ' truncate to zero size
        Dim line As String = (sPolicyNumber & sFieldDelimiter & sClientName & sFieldDelimiter & sInceptionDate & sFieldDelimiter & sEffectiveDate & sFieldDelimiter &
                                    StringsHelper.Format(dblGrossPremium, "####0.00") & sFieldDelimiter &
                                    StringsHelper.Format(sIPT, "####0.00") & sFieldDelimiter &
                                    StringsHelper.Format(sCommissionAmount, "####0.00") & sFieldDelimiter &
                                    StringsHelper.Format(sPremiumIncIPT, "####0.00") & sFieldDelimiter &
                                    sTransactionCode & sFieldDelimiter & sPostcode & sFieldDelimiter &
                                    sCoverType & sFieldDelimiter & sClassOfUse & sFieldDelimiter &
                                    sDriverRestriction & sFieldDelimiter & sPolicyHolderDOB & sFieldDelimiter &
                                    sVoluntaryXS & sFieldDelimiter & sCompulsoryXS & sFieldDelimiter &
                                    sNCDLevel & sFieldDelimiter & sProtectedNCD & sFieldDelimiter &
                                    sVehicleReg & sFieldDelimiter & sVehicleValue & sFieldDelimiter &
                             sEngineSize & sFieldDelimiter & sVehicleDetails)
        Dim w = New StreamWriter(F)
        w.WriteLine(line)
        w.Flush()
        w.Close()
        F.Close()

        'hFile = FreeFile()

        'File.Open(hFile, sFilename, OpenMode.Append)

        'PrintLine(hFile, sPolicyNumber & sFieldDelimiter & sClientName & sFieldDelimiter & sInceptionDate & sFieldDelimiter & sEffectiveDate & sFieldDelimiter &
        '                            StringsHelper.Format(dblGrossPremium, "####0.00") & sFieldDelimiter &
        '                            StringsHelper.Format(sIPT, "####0.00") & sFieldDelimiter &
        '                            StringsHelper.Format(sCommissionAmount, "####0.00") & sFieldDelimiter &
        '                            StringsHelper.Format(sPremiumIncIPT, "####0.00") & sFieldDelimiter &
        '                            sTransactionCode & sFieldDelimiter & sPostcode & sFieldDelimiter &
        '                            sCoverType & sFieldDelimiter & sClassOfUse & sFieldDelimiter &
        '                            sDriverRestriction & sFieldDelimiter & sPolicyHolderDOB & sFieldDelimiter &
        '                            sVoluntaryXS & sFieldDelimiter & sCompulsoryXS & sFieldDelimiter &
        '                            sNCDLevel & sFieldDelimiter & sProtectedNCD & sFieldDelimiter &
        '                            sVehicleReg & sFieldDelimiter & sVehicleValue & sFieldDelimiter &
        '                     sEngineSize & sFieldDelimiter & sVehicleDetails)

        'FileClose(hFile)

        GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Finished GenerateBordereaux...", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GenerateBordereaux")

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetVehicleDescriptionFromABI
    '
    ' Description: Accepts a make and model ABI code and returns the
    '              descriptions
    ' PWF 17/07/2001
    ' ***************************************************************** '
    Private Function GetVehicleDescriptionFromABI(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_sMakeABI As String, ByVal v_sModelABI As String, ByRef r_sMakeDescription As String, ByRef r_sModelDescription As String) As Integer
        Dim result As Integer = 0
        Dim oList As bGISListManager.InterfaceNoLogin
        Dim nIndex As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="GetVehicleDescriptionFromABI: Make=" & v_sMakeABI & " - Model=" & v_sModelABI, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetVehicleDescriptionFromABI", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        ' Create a new ListManager object
        oList = New bGISListManager.InterfaceNoLogin()
        If oList Is Nothing Then
            result = gPMConstants.PMEReturnCode.PMFalse

            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Set oList = New iGISListManager.InterfaceNoLogin - oList is Nothing", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetVehicleDescriptionFromABI", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Initialise the ListManager object
        m_lReturn = oList.Initialise()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_lReturn = oList.Initialise() - m_lReturn is:" & m_lReturn, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetVehicleDescriptionFromABI", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Check versions
        m_lReturn = oList.CheckListVersions(v_sGisDataModelCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_lReturn = oList.CheckListVersions('" & ACDataModelCode & "', '" & v_sGisDataModelCode & "') - m_lReturn is:" & CStr(m_lReturn), vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetVehicleDescriptionFromABI", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Lookup vehicle make
        m_lReturn = oList.GetDescription("393219", v_sMakeABI, r_sMakeDescription)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_lReturn = oList.GetDescription('393219',... - Failed: " & m_lReturn, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetVehicleDescriptionFromABI", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Lookup vehicle model
        m_lReturn = oList.GetDescription("393220", v_sModelABI, r_sModelDescription)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_lReturn = oList.GetDescription('393220',... - Failed: " & m_lReturn, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetVehicleDescriptionFromABI", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' We need to mess with the model a little to trim off excess info
        nIndex = (r_sModelDescription.IndexOf(")"c) + 1)
        If nIndex > 0 Then
            r_sModelDescription = r_sModelDescription.Substring(nIndex).Trim()
        End If

        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="GetVehicleDescriptionFromABI: Make=" & r_sMakeDescription & " - Model=" & r_sModelDescription, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetVehicleDescriptionFromABI", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)


        Return result

    End Function

    ' ***************************************************************** '
    ' Name:         SendEmailBefore
    '
    ' Description:  Adust Parameters before bGis sends an Email (depending on Type).
    '               To/CC parameters can be semi-colon delimited list of recipients.
    '
    ' Author:       RAG161100
    '
    ' ***************************************************************** '
    Public Function SendEmailBefore(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String,
                                    ByRef r_oDataset As cGISDataSetControl.Application, ByVal v_lEMailType As Integer,
                                    ByRef r_sEMailFrom As String, ByRef r_sEMailTo As String, ByRef r_sEMailCC As String,
                                    ByRef r_sEMailSubject As String, ByRef r_sEMailText As String,
                                    Optional ByRef r_vAdditionalDataArray As Object = Nothing) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Select Case v_lEMailType
                Case ACEmailTellAFriend ' =1

                    ' All parameters are as passed from the Web Pages.


                Case ACEmailConsumerMTA ' =2

                    ' Need to get "To" and "From" parameters from Registry

                    ' Get "To" field from the registry.
                    m_lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode,
                                                                                 v_sSettingName:=
                                                                                    GISSharedConstants.
                                                                                    GISRegConsumerMTAEmailTo,
                                                                                 r_sSettingValue:=r_sEMailTo,
                                                                                 v_sBusinessTypeCode:=
                                                                                    v_sGisBusinessTypeCode,
                                                                                 v_sSubKey:=
                                                                                    GISSharedConstants.
                                                                                    GISRegSubKeyEmails)

                    ' Get "From" field from the registry.
                    m_lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode,
                                                                                 v_sSettingName:=
                                                                                    GISSharedConstants.
                                                                                    GISRegConsumerMTAEmailFrom,
                                                                                 r_sSettingValue:=r_sEMailFrom,
                                                                                 v_sBusinessTypeCode:=
                                                                                    v_sGisBusinessTypeCode,
                                                                                 v_sSubKey:=
                                                                                    GISSharedConstants.
                                                                                    GISRegSubKeyEmails)


                Case Else
                    Return result

            End Select

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                              sMsg:="SendEmailBefore Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass,
                                              vMethod:="SendEmailBefore", vErrNo:=Informations.Err().Number,
                                              vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         SendEmailAfter
    '
    ' Description:  Adust Parameters after bGis sends an Email.
    '
    ' Author:       RAG161100
    '
    ' ***************************************************************** '
    Public Function SendEmailAfter(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String,
                                   ByRef r_oDataset As cGISDataSetControl.Application, ByVal v_lEMailType As Integer,
                                   ByRef r_sEMailFrom As String, ByRef r_sEMailTo As String, ByRef r_sEMailCC As String,
                                   ByRef r_sEMailSubject As String, ByRef r_sEMailText As String,
                                   Optional ByRef r_vAdditionalDataArray As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                              sMsg:="SendEmailAfter Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass,
                                              vMethod:="SendEmailAfter", vErrNo:=Informations.Err().Number,
                                              vErrDesc:=excep.Message)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name:         VehicleLookup
    '
    ' Description:  Look up vehicle details from the Registration Mark.
    '               Vehicle details should be stored directly
    '               into the dataset if match is successful.
    '
    ' Author:       RAG171100
    '
    ' ***************************************************************** '
    Public Function VehicleLookup(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String,
                                  ByRef r_oDataset As cGISDataSetControl.Application,
                                  ByVal v_sRegistrationNumber As String,
                                  Optional ByRef r_vAdditionalDataArray As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oBOM As Object

        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                              sMsg:="VehicleLookup Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass,
                                              vMethod:="VehicleLookup", vErrNo:=Informations.Err().Number,
                                              vErrDesc:=excep.Message)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         Refer
    '
    ' Description:  Do a refer
    '
    ' Author:       RAG161100
    '
    ' ***************************************************************** '
    Public Function Refer(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String,
                          ByRef r_oDataset As cGISDataSetControl.Application, ByVal v_sInsurerCode As String,
                          Optional ByRef r_vAdditionalDataArray As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Refer Failed",
                                              vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="Refer",
                                              vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    Public Sub New()
        MyBase.New()
    End Sub


    Public Function NBTransactBefore(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String,
                                     ByRef r_oDataset As cGISDataSetControl.Application, ByVal v_vSchemeArray(,) As Object,
                                     ByRef r_vAdditionalDataArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim blCalledFromSTS As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            blCalledFromSTS = False

            For i As Integer = 0 To r_vAdditionalDataArray.GetUpperBound(1)
                Select Case r_vAdditionalDataArray(0, i)
                    Case CNCalledFromSTS

                        blCalledFromSTS = CBool(r_vAdditionalDataArray(1, i))
                        Exit For
                End Select
            Next i

            ' If this call hasn't arisen from the STS layer then skip out.
            If Not blCalledFromSTS Then
                Return result
            End If

            ' *************************************************************************
            ' For now, we are doing nothing in NBTransactBefore.
            ' Check with Richard Taylor or Jay Bishtawi, before changing this PLEASE
            ' *************************************************************************


            Return result

        Catch excep As System.Exception


            ' Release our Ref to the Dataset
            m_oDataSet = Nothing

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                              sMsg:="NBTransactBefore Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass,
                                              vMethod:="NBTransactBefore", vErrNo:=Informations.Err().Number,
                                              vErrDesc:=excep.Message)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: UpdatePremium
    '
    ' Description: Updates this_premium on an insurance file ready to post
    '
    ' History: 04/07/2003 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function UpdatePremium(ByVal v_lInsuranceFileCnt As Integer, ByVal v_vThisPremium As Object) As Integer

        Dim result As Integer = 0
        Dim oBusiness As bSIRInsuranceFile.Business
        Dim vArray() As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get an instance of bSIRInsuranceFile.Business
        oBusiness = New bSIRInsuranceFile.Business
        m_lReturn = oBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogInfo,
                               sMsg:="Failed to get instance of bSIRInsuranceFile.Business", vApp:=ToSafeString(ACApp),
                               vClass:=ACClass, vMethod:="UpdatePremium", vErrNo:=Informations.Err().Number,
                               vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Get the insurance file record

        m_lReturn = oBusiness.GetDetails(vInsuranceFileCnt:=v_lInsuranceFileCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Load the values into the

        m_lReturn = oBusiness.GetNext(r_vFieldArray:=vArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Update the premium


        vArray(InsuranceFileConst.ACThisPremium) = v_vThisPremium

        ' Pass the value back to the business object

        m_lReturn = oBusiness.EditUpdate(lRow:=1, r_vFieldArray:=vArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get it to update the value in the database

        m_lReturn = oBusiness.Update()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Terminate and clear up

        oBusiness.Dispose()
        oBusiness = Nothing

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: ProcessTransactions
    '
    ' Description: Posts the transactions to Orion
    '
    ' History: 02/07/2003 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessTransactions(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sDebitCredit As String,
                                         ByVal v_vLastTransType As String, ByVal v_vLastTransDate As Object,
                                         ByVal v_dtPolicyStartDate As Date, ByVal v_dPostingAmount As Double,
                                         ByVal v_sReason As String, ByVal v_lRealInsuranceFileCnt As Integer,
                                         ByVal r_bUnderwritingBranchEnabled As Boolean) As Integer

        Dim result As Integer = 0
        Dim oTransaction As Object
        Dim lStatus As gPMConstants.PMEReturnCode



        result = gPMConstants.PMEReturnCode.PMTrue

        If Not r_bUnderwritingBranchEnabled Then
            m_lReturn = UpdatePremium(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_vThisPremium:=v_dPostingAmount)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        ' Create the same object that ITS4ME use
        oTransaction = gPMFunctions.CreateLateBoundObject("iPMBTransactions.GNetNoLogon")
        Dim oDatabase As Object = m_oDatabase
        m_lReturn = oTransaction.Initialise(sUserName:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeString(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(m_sCallingAppName), vDatabase:=oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_oDatabase = oDatabase
        'SJ 19/03/2004 - start
        If v_vLastTransType.Trim() = "" Then
            v_vLastTransType = "New Business"
        End If
        m_lReturn = oTransaction.SetProcessModes(vTransactionType:=ToSafeString(v_vLastTransType))
        '    m_lReturn& = oTransaction.SetProcessModes( _
        ''                   vTransactionType:="New Business")
        'SJ 19/03/2004 - end
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        With oTransaction
            .FromEvent = False
            .InsuranceFileCnt = v_lInsuranceFileCnt
            .DebitCredit = v_sDebitCredit
            .PolicyStartDate = v_dtPolicyStartDate
            .Reason = v_sReason
        End With

        m_lReturn = oTransaction.Start()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'SJ 04/06/2004 - start
        lStatus = oTransaction.Status
        'SJ 04/06/2004 - end

        oTransaction.Dispose()

        oTransaction = Nothing

        'SJ 04/06/2004 - start
        If lStatus <> gPMConstants.PMEReturnCode.PMOK Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'SJ 04/06/2004 - end

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetValuesFromAdditionalData
    '
    ' Description:
    '
    ' History: 05/03/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function GetValuesFromAdditionalData(ByRef r_lNewSchemeId As Integer, ByRef r_lSchemeId As Integer, ByRef r_sCoverType As String, ByRef r_sMessage As String, ByRef r_dtCoverStartDate As Date, ByRef r_lRiskCodeId As Integer, ByRef r_lInsurerCnt As Integer, ByRef r_lInsuranceFileCnt As Integer, ByRef r_lNBInsuranceFileCnt As Integer, ByRef r_sNBDebitCredit As String, ByRef r_vNBLastTransType As Object, ByRef r_vNBLastTransDate As Object, ByRef r_vNBPolicyStartDate As Object, ByRef r_dNBPostingAmount As Double, ByRef r_sNBReason As String, ByRef r_lNBRealInsuranceFileCnt As Integer, ByRef r_bUnderwritingBranchEnabled As Boolean, ByRef r_lLastEdiMessageCountSent As Integer, ByRef r_sMTAType As String, ByRef r_lFinancePlanSchemeNo As Integer, ByRef r_bAddMtaToExistingFinancePlan As Boolean, ByRef r_lOldInsuranceFileCnt As Integer, ByVal r_vAdditionalDataArray As Array) As Integer
        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the lNewSchemeId from AdditionalData Array
        m_lReturn = ParseArray(m_sUsername, r_vAdditionalDataArray, CNNewSchemeId, r_lNewSchemeId)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMError
            r_sMessage = "lNewSchemeId"
            Return result
        End If
        ' Get the lSchemeId from AdditionalData Array
        m_lReturn = ParseArray(m_sUsername, r_vAdditionalDataArray, CNSchemeId, r_lSchemeId)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMError
            r_sMessage = "lSchemeId"
            Return result
        End If
        ' Get the sCoverType from AdditionalData Array
        m_lReturn = ParseArray(m_sUsername, r_vAdditionalDataArray, CNCoverType, r_sCoverType)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "sCoverType"
            Return gPMConstants.PMEReturnCode.PMError
        End If

        ' Get the dtCoverStartDate from AdditionalData Array
        m_lReturn = ParseArray(m_sUsername, r_vAdditionalDataArray, CNCoverFromDate, r_dtCoverStartDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "dtCoverStartDate"
            Return gPMConstants.PMEReturnCode.PMError
        End If

        ' Get the lRiskCodeId from AdditionalData Array
        m_lReturn = ParseArray(m_sUsername, r_vAdditionalDataArray, CNRiskCodeId, r_lRiskCodeId)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "lRiskCodeId"
            Return gPMConstants.PMEReturnCode.PMError
        End If

        ' Get the lInsurerCnt from AdditionalData Array
        m_lReturn = ParseArray(m_sUsername, r_vAdditionalDataArray, CNInsurerCnt, r_lInsurerCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "lInsurerCnt"
            Return gPMConstants.PMEReturnCode.PMError
        End If

        ' Get the FileInsuranceCnt from AdditionalData Array
        m_lReturn = ParseArray(m_sUsername, r_vAdditionalDataArray, CNInsuranceFileCnt, r_lInsuranceFileCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "lInsuranceFileCnt"
            Return gPMConstants.PMEReturnCode.PMError
        End If

        ' NB_InsuranceFileCnt
        m_lReturn = ParseArray(m_sUsername, r_vAdditionalDataArray, "NB_InsuranceFileCnt", r_lNBInsuranceFileCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMError
        End If

        ' NB_DebitCredit
        m_lReturn = ParseArray(m_sUsername, r_vAdditionalDataArray, "NB_DebitCredit", r_sNBDebitCredit)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "sNBDebitCredit"
            Return gPMConstants.PMEReturnCode.PMError
        End If

        ' NB_LastTransType
        m_lReturn = ParseArray(m_sUsername, r_vAdditionalDataArray, "NB_LastTransType", r_vNBLastTransType)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "vNBLastTransType"
            Return gPMConstants.PMEReturnCode.PMError
        End If

        ' NB_LastTransDate
        m_lReturn = ParseArray(m_sUsername, r_vAdditionalDataArray, "NB_LastTransDate", r_vNBLastTransDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "vNBLastTransDate"
            Return gPMConstants.PMEReturnCode.PMError
        End If

        ' NB_PolicyStartDate
        m_lReturn = ParseArray(m_sUsername, r_vAdditionalDataArray, "NB_PolicyStartDate", r_vNBPolicyStartDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "vNBPolicyStartDate"
            Return gPMConstants.PMEReturnCode.PMError
        End If

        ' NB_PostingAmount
        m_lReturn = ParseArray(m_sUsername, r_vAdditionalDataArray, "NB_PostingAmount", r_dNBPostingAmount)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "dNBPostingAmount"
            Return gPMConstants.PMEReturnCode.PMError
        End If

        ' NB_Reason
        m_lReturn = ParseArray(m_sUsername, r_vAdditionalDataArray, "NB_Reason", r_sNBReason)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "sNBReason"
            Return gPMConstants.PMEReturnCode.PMError
        End If

        ' NB_RealInsuranceFileCnt
        m_lReturn = ParseArray(m_sUsername, r_vAdditionalDataArray, "NB_RealInsuranceFileCnt",
                               r_lNBRealInsuranceFileCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "lNBRealInsuranceFileCnt"
            Return gPMConstants.PMEReturnCode.PMError
        End If

        ' Underwriting Branch Ind
        m_lReturn = ParseArray(m_sUsername, r_vAdditionalDataArray, CNEdiSolution, r_bUnderwritingBranchEnabled)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "bUnderwritingBranchEnabled"
            result = gPMConstants.PMEReturnCode.PMError
        End If

        ' Underwriting Branch Ind
        m_lReturn = ParseArray(m_sUsername, r_vAdditionalDataArray, CNLastEdiMessageCountSent,
                               r_lLastEdiMessageCountSent)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "lLastEdiMessageCountSent"
            result = gPMConstants.PMEReturnCode.PMError
        End If

        ' Get the MTA Type from AdditionalData Array
        m_lReturn = ParseArray(m_sUsername, r_vAdditionalDataArray, PMNavKeyConst.ACTKeyNameMTAType, r_sMTAType)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMError
            r_sMessage = "lNewSchemeId"
            Return result
        End If

        m_lReturn = ParseArray(m_sUsername, r_vAdditionalDataArray, "FinancePlanSchemeNo", r_lFinancePlanSchemeNo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMError
            r_sMessage = "FinancePlanSchemeNo"
            Return result
        End If

        m_lReturn = ParseArray(m_sUsername, r_vAdditionalDataArray, "AddMtaToExistingFinancePlan",
                               r_bAddMtaToExistingFinancePlan)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMError
            r_sMessage = "AddMtaToExistingFinancePlan"
            Return result
        End If

        m_lReturn = ParseArray(m_sUsername, r_vAdditionalDataArray, "old_insurance_file_cnt", r_lOldInsuranceFileCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMError
            r_sMessage = "old_insurance_file_cnt"
            Return result
        End If

        Return result

    End Function

    Public Function GenerateDocument(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String,
                                     ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer,
                                     ByVal v_lInsuranceFileCnt As Integer,
                                     Optional ByVal v_lProcessTypesDocId As Integer = 0,
                                     Optional ByVal v_lDocumentTemplateId As Integer = 0,
                                     Optional ByVal v_lDocumentTypeId As Integer = 0,
                                     Optional ByRef r_sMergedFilePath As String = "",
                                     Optional ByRef r_sSpooledFilePath As String = "") As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        'Developer guide no. 108

        Dim oDocumentWrapper As Object
        Dim oDocTemplate As Object


        'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Err_GenerateDocument)")

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Create bSIRDocManagerWrapper object
        'Developer guide no. 108

        oDocumentWrapper = gPMFunctions.CreateLateBoundObject("bSIRDocManagerWrapper.Interface_Renamed")

        'Invoke Initialise method
        Dim oDatabase As Object = m_oDatabase
        lReturn = CType(oDocumentWrapper.InitialiseBusiness(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeString(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(m_sCallingAppName), vDatabase:=oDatabase), gPMConstants.PMEReturnCode)

        'Test for errors
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateDocument Failed - Failed to create bSIRDocManagerWrapper object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GenerateDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If
        m_oDatabase = oDatabase
        'Set the object property values
        oDocumentWrapper.TransactionType = v_sGisBusinessTypeCode
        oDocumentWrapper.PartyCnt = v_lPartyCnt
        oDocumentWrapper.InsuranceFolderCnt = v_lInsuranceFolderCnt
        oDocumentWrapper.InsuranceFileCnt = v_lInsuranceFileCnt
        oDocumentWrapper.ProcessTypesDocsId = v_lProcessTypesDocId
        oDocumentWrapper.DocumentTemplateId = v_lDocumentTemplateId
        oDocumentWrapper.DocumentTypeId = v_lDocumentTypeId
        oDocumentWrapper.OutputAsHTML = True
        oDocumentWrapper.Mode = gSIRLibrary.ACSpoolDocMode
        oDocumentWrapper.CalledFromSAM = True
        'oDocumentWrapper.uniqueID = v_lInsuranceFileCnt + v_lDocumentTemplateId

        ' This poperty may not exist in a 1.8.6 environment
        Try
            oDocumentWrapper.EffectiveDate = DateTime.Now

        Catch
        End Try

        'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Err_GenerateDocument)")

        'Invoke object Start method
        lReturn = CType(oDocumentWrapper.Start(), gPMConstants.PMEReturnCode)

        'Test for errors
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError,
                                              sMsg:=
                                                 "GenerateDocument Failed - Failed to execute bSIRDocManagerWrapper.Interface Start ",
                                              vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GenerateDocument",
                                              vErrNo:=Informations.Err().Number,
                                              vErrDesc:=Informations.Err().Description)
            Return result
        End If

        'Set return values
        r_sMergedFilePath = oDocumentWrapper.MergedFilePath
        r_sSpooledFilePath = oDocumentWrapper.SpooledFilePath

        'Clean up objects
        oDocumentWrapper.Dispose()
        oDocumentWrapper = Nothing

        Return result

Err_GenerateDocument:

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                          sMsg:="GenerateDocument Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass,
                                          vMethod:="GenerateDocument", vErrNo:=Informations.Err().Number,
                                          vErrDesc:=Informations.Err().Description)

        Return result

        'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

        Return result
    End Function

    Public Function GeneratePolicyDocument(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String,
                                           ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer,
                                           ByVal v_lInsuranceFileCnt As Integer,
                                           Optional ByVal v_lProcessTypesDocId As Integer = 0,
                                           Optional ByVal v_lDocumentTemplateId As Integer = 0,
                                           Optional ByVal v_lDocumentTypeId As Integer = 0,
                                           Optional ByRef r_sMergedFilePath As String = "",
                                           Optional ByRef r_sSpooledFilePath As String = "",
                                           Optional ByRef r_vAdditionalDataArray As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        'Developer guide no. 108

        Dim oDocumentWrapper As Object
        Dim oDocTemplate As Object = Nothing


        'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Err_GeneratePolicyDocument)")

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Create bSIRDocManagerWrapper object
        'Developer guide no. 108

        oDocumentWrapper = gPMFunctions.CreateLateBoundObject("bSIRDocManagerWrapper.Interface_Renamed")

        'Invoke Initialise method
        Dim oDatabase As Object = m_oDatabase
        lReturn = CType(oDocumentWrapper.InitialiseBusiness(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID),
                                                            iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID),
                                                            iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(m_sCallingAppName), vDatabase:=oDatabase), gPMConstants.PMEReturnCode)

        'Test for errors
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GeneratePolicyDocument Failed - Failed to create bSIRDocManagerWrapper object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GeneratePolicyDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If
        m_oDatabase = oDatabase
        'Set the object property values
        oDocumentWrapper.TransactionType = v_sGisBusinessTypeCode
        oDocumentWrapper.PartyCnt = v_lPartyCnt
        oDocumentWrapper.InsuranceFolderCnt = v_lInsuranceFolderCnt
        oDocumentWrapper.InsuranceFileCnt = v_lInsuranceFileCnt
        oDocumentWrapper.ProcessTypesDocsId = v_lProcessTypesDocId
        oDocumentWrapper.DocumentTemplateId = v_lDocumentTemplateId
        oDocumentWrapper.DocumentTypeId = v_lDocumentTypeId
        oDocumentWrapper.OutputAsHTML = True
        oDocumentWrapper.Mode = 4 'ACSpoolDocMode
        'oDocumentWrapper.uniqueID = v_lInsuranceFileCnt + v_lDocumentTemplateId

        ' This poperty may not exist in a 1.8.6 environment
        Try
            oDocumentWrapper.EffectiveDate = DateTime.Now

        Catch
        End Try

        'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Err_GeneratePolicyDocument)")

        'Invoke object Start method
        lReturn = CType(oDocumentWrapper.Start(), gPMConstants.PMEReturnCode)

        'Test for errors
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GeneratePolicyDocument Failed - Failed to execute bSIRDocManagerWrapper.Interface Start ", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GeneratePolicyDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        'Set return values
        r_sMergedFilePath = oDocumentWrapper.MergedFilePath
        r_sSpooledFilePath = oDocumentWrapper.SpooledFilePath

        'Clean up objects
        oDocumentWrapper.Dispose()
        oDocumentWrapper = Nothing

        Return result

Err_GeneratePolicyDocument:

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GeneratePolicyDocument Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GeneratePolicyDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

        'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

        Return result
    End Function

    Public Function GetDocTemplate(ByVal v_lSchemeID As Integer, ByVal v_lAgentCnt As Integer,
                                   ByVal v_sProcessTypeCode As String, ByRef v_vDocumentArray(,) As Object) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim obPMBDocLink As bPMBDocLink.Business
            Dim lReturn As Integer

            ' Create bPMBDocLink object
            obPMBDocLink = New bPMBDocLink.Business
            lReturn = obPMBDocLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="GetDocTemplate Failed - Failed to create obPMBDocLink object.",
                                   vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetRiskCodeList",
                                   vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If


            lReturn = obPMBDocLink.GetDocTemplate(v_lSchemeID:=v_lSchemeID, v_lAgentCnt:=ToSafeInteger(v_lAgentCnt),
                                                  v_sProcessTypeCode:=v_sProcessTypeCode,
                                                  v_vDocumentArray:=v_vDocumentArray)

            'Check return value - if no documents were found for the agent call method
            'again with AgentCnt of -1. This should return all docs that are linked to
            'the scheme and process type only and not to any specific agent.
            If lReturn = gPMConstants.PMEReturnCode.PMNotFound Then


                lReturn = obPMBDocLink.GetDocTemplate(v_lSchemeID:=v_lSchemeID, v_lAgentCnt:=-1,
                                                      v_sProcessTypeCode:=v_sProcessTypeCode,
                                                      v_vDocumentArray:=v_vDocumentArray)
            End If

            'Check return value for error
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="GetDocTemplate Failed - obPMBDocLink.GetDocTemplate method failed.",
                                   vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetDocTemplate",
                                   vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If


            obPMBDocLink.Dispose()
            obPMBDocLink = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocTemplate Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetDocTemplate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddMTAQuote
    '
    ' Description:
    '
    ' History: 05/03/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function AddMTAQuote(ByVal v_sMTAType As String, ByVal v_lNewMessageVersion As Integer,
                                ByVal v_dtEffectiveDate As Date, ByRef r_lInsuranceFileCnt As Integer,
                                ByRef r_lInsuranceFolderCnt As Integer, ByVal v_cThisPremium As Decimal,
                                ByVal v_cNetPremium As Decimal, ByVal v_cTaxAmount As Decimal,
                                ByVal v_cTaxRate As Decimal, Optional ByRef r_lRiskFolderCnt As Integer = 0,
                                Optional ByVal v_dtCoverEndDate As Date = #12/30/1899#,
                                Optional ByVal v_vInsuranceRef As Object = Nothing,
                                Optional ByVal v_vAlternateReference As Object = Nothing,
                                Optional ByRef r_vAdditionalDataArray(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim blCalledFromSTS As Boolean
            Dim vKeyArray As Object
            Dim lPartyCnt As Integer
            Dim obSIRQuotePolicy As Object
            Dim sErrorMessage As String = ""
            Dim lNewPolicyVersion, lLastEdiMessageCountReceived As Integer

            blCalledFromSTS = False

            For i As Integer = 0 To r_vAdditionalDataArray.GetUpperBound(1)
                Select Case r_vAdditionalDataArray(0, i)
                    Case CNCalledFromSTS

                        blCalledFromSTS = CBool(r_vAdditionalDataArray(1, i))
                        Exit For
                End Select
            Next i

            ' If this call hasn't arisen from the STS layer then skip out.
            If Not blCalledFromSTS Then
                Return result
            End If


            If Informations.IsNothing(v_vAlternateReference) Then
                'Must have an alternate reference
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the latest live version of the policy
            m_lReturn = GetPolicyDetailsForMTA(v_dtEffectiveDate:=v_dtEffectiveDate, v_vAlternateReference:=v_vAlternateReference, r_lPartyCnt:=lPartyCnt, r_lInsuranceFileCnt:=r_lInsuranceFileCnt, r_lInsuranceFolderCnt:=r_lInsuranceFolderCnt, r_lLastEdiMessageCountReceived:=lLastEdiMessageCountReceived, r_lRiskFolderCnt:=r_lRiskFolderCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyDetailsForMTA Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddMTAQuote")
                Return result
            End If

            'sp now retuns -1 for NULL entries indicating old value not know so fix it up
            If (lLastEdiMessageCountReceived <> -1) And (v_lNewMessageVersion <> (lLastEdiMessageCountReceived + 1)) Then
                sErrorMessage = "Message version error: current version = " & lLastEdiMessageCountReceived & ", new version = " & CStr(v_lNewMessageVersion)
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sErrorMessage, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddMTAQuote")
                Return result
            End If

            'Get the next policy verison
            m_lReturn = GetNextPolicyVersion(v_lInsuranceFolderCnt:=r_lInsuranceFolderCnt, r_lNewPolicyVersion:=lNewPolicyVersion)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNextPolicyVersion Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddMTAQuote")
                Return result
            End If

            'Call bSIRQuotePolicy to create the new policy version
            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=obSIRQuotePolicy, v_sClassName:="bSIRQuotePolicy.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSIRQuotePolicy object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddQuote")
                Return result
            End If

            'Call SetProcessModes

            m_lReturn = obSIRQuotePolicy.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeMTALive, vTransactionType:=gPMConstants.PMTransactionTypeMTA)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRQuotePolicy.SetProcessModes failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddMTAQuote")
                Return result
            End If

            'Party Cnt

            m_lReturn = AddToKeyArray(vKeyArray, PMNavKeyConst.PMKeyNamePartyCnt, lPartyCnt)
            'Cover Start Date

            m_lReturn = AddToKeyArray(vKeyArray, PMNavKeyConst.PMKeyNamePolicyStartDate, v_dtEffectiveDate)
            ' Cover End Date
            If v_dtCoverEndDate <> #12/30/1899# Then

                m_lReturn = AddToKeyArray(vKeyArray, "policy_end_date", v_dtCoverEndDate)
            End If
            'Insurance Folder Cnt

            m_lReturn = AddToKeyArray(vKeyArray, PMNavKeyConst.PMKeyNameInsFolderCnt, r_lInsuranceFolderCnt)
            'Insurance File Cnt

            m_lReturn = AddToKeyArray(vKeyArray, PMNavKeyConst.PMKeyNameInsuranceFileCnt, r_lInsuranceFileCnt)
            ' Alternate Reference

            m_lReturn = AddToKeyArray(vKeyArray, PMNavKeyConst.PMKeyNameAlternateReference, v_vAlternateReference)
            ' this_premium

            m_lReturn = AddToKeyArray(vKeyArray, PMNavKeyConst.PMKeyNameThisPremium, v_cThisPremium)
            ' net premium

            m_lReturn = AddToKeyArray(vKeyArray, PMNavKeyConst.PMKeyNameNetPremium, v_cNetPremium)
            ' ipt amount

            m_lReturn = AddToKeyArray(vKeyArray, PMNavKeyConst.PMKeyNameTaxAmount, v_cTaxAmount)
            ' ipt rate

            m_lReturn = AddToKeyArray(vKeyArray, PMNavKeyConst.PMKeyNameTaxRate, v_cTaxRate)
            'MTA Type

            m_lReturn = AddToKeyArray(vKeyArray, PMNavKeyConst.ACTKeyNameMTAType, v_sMTAType)
            'Policy Version

            m_lReturn = AddToKeyArray(vKeyArray, PMNavKeyConst.PMKeyNamePolicyVersion, lNewPolicyVersion)

            'Call SetKeys

            m_lReturn = obSIRQuotePolicy.SetKeys(vKeyArray:=vKeyArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRQuotePolicy.SetKeys failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddMTAQuote")
                Return result
            End If

            'Call ProcessStsMTA to create the new insurance file record

            m_lReturn = obSIRQuotePolicy.ProcessStsMTA()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRQuotePolicy.ProcessStsMTA failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddQuote")
                Return result
            End If

            'Get keys stuff

            m_lReturn = obSIRQuotePolicy.Getkeys(vKeyArray:=vKeyArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRQuotePolicy.GetKeys failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddQuote")
                Return result
            End If

            'Loop through returned key array and pass relevant values back

            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)

                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMNavKeyConst.PMKeyNameInsuranceFileCnt

                        r_lInsuranceFileCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameInsuranceFolderCnt

                        r_lInsuranceFolderCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                End Select
            Next lRow


            'Clean up objects

            obSIRQuotePolicy.Dispose()
            obSIRQuotePolicy = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddMTAQuote Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddMTAQuote", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            ''Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPolicyDetailsForMTA
    '
    ' Description:
    '
    ' History: 05/03/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function GetPolicyDetailsForMTA(ByVal v_dtEffectiveDate As Date, ByVal v_vAlternateReference As Object, ByRef r_lPartyCnt As Integer, ByRef r_lInsuranceFileCnt As Integer, ByRef r_lInsuranceFolderCnt As Integer, ByRef r_lLastEdiMessageCountReceived As Integer, ByRef r_lRiskFolderCnt As Integer) As Integer

        Dim result As Integer = 0


        Const ACPartyCnt As Integer = 0
        Const ACInsuranceFolderCnt As Integer = 1
        Const ACInsuranceFileCnt As Integer = 6
        ' Const ACRiskCodeId As Integer = 9
        Const ACLastEdiMessageCountReceived As Integer = 10
        Const ACRiskFolderCnt As Integer = 11

        result = gPMConstants.PMEReturnCode.PMTrue

        Dim vResultArray(,) As Object = Nothing

        ' Clear the parameters
        m_oDatabase.Parameters.Clear()

        ' Add the new one

        m_lReturn = m_oDatabase.Parameters.Add(sName:="alternate_reference", vValue:=CStr(v_vAlternateReference), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message

            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter alternate_reference = " & CStr(v_vAlternateReference), vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetPolicyDetailsForMTA")
            Return result
        End If

        ' Call the SQL
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyVersionForMtaAltReferenceSQL, sSQLName:=ACGetPolicyVersionForMtaAltReferenceName, bStoredProcedure:=ACGetPolicyVersionForMtaAltReferenceStored, vResultArray:=vResultArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to call SQL : " & ACGetPolicyVersionForMtaAltReferenceSQL, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetPolicyDetailsForMTA")
            Return result
        End If

        ' Check our results
        If Informations.IsArray(vResultArray) Then

            r_lPartyCnt = CInt(vResultArray(ACPartyCnt, 0))

            r_lInsuranceFolderCnt = CInt(vResultArray(ACInsuranceFolderCnt, 0))

            r_lInsuranceFileCnt = CInt(vResultArray(ACInsuranceFileCnt, 0))

            r_lLastEdiMessageCountReceived = CInt(vResultArray(ACLastEdiMessageCountReceived, 0))

            r_lRiskFolderCnt = CInt(vResultArray(ACRiskFolderCnt, 0))
        Else
            result = gPMConstants.PMEReturnCode.PMNotFound
            ' Didn't return anything, lets error
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACGetPolicyVersionForMtaAltReferenceName & " didn't return any data.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetPolicyDetailsForMTA")
            Return result
        End If

        ' Clear up
        vResultArray = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetNextPolicyVersion
    '
    ' Description:
    '
    ' History: 05/03/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function GetNextPolicyVersion(ByVal v_lInsuranceFolderCnt As Integer, ByRef r_lNewPolicyVersion As Integer) _
        As Integer

        Dim result As Integer = 0


        Const ACNewPolicyVersion As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        Dim vResultArray(,) As Object = Nothing

        ' Clear the parameters
        m_oDatabase.Parameters.Clear()

        ' Add the new one
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_Folder_Cnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter insurance_folder_cnt = " & v_lInsuranceFolderCnt, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetNextPolicyVersion")
            Return result
        End If

        ' Call the SQL
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetMaxPolVersionSQL, sSQLName:=ACGetMaxPolVersionName, bStoredProcedure:=ACGetMaxPolVersionStored, vResultArray:=vResultArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to call SQL : " & ACGetMaxPolVersionSQL, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetNextPolicyVersion")
            Return result
        End If

        ' Check our results
        If Informations.IsArray(vResultArray) Then

            r_lNewPolicyVersion = CInt(CDbl(vResultArray(ACNewPolicyVersion, 0)) + 1)
        Else
            result = gPMConstants.PMEReturnCode.PMNotFound
            ' Didn't return anything, lets error
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACGetMaxPolVersionName & " didn't return any data.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetNextPolicyVersion")
            Return result
        End If

        ' Clear up
        vResultArray = Nothing

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: UpdateLastEdiMessageCountReceived
    '
    ' Description:
    '
    ' History: 08/03/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function UpdateLastEdiMessageCountReceived(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lLastEdiMessageCountReceived As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            'Insurance_file_cnt
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'last_edi_message_count_received
            m_lReturn = m_oDatabase.Parameters.Add(sName:="last_edi_message_count_received", vValue:=CStr(v_lLastEdiMessageCountReceived), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the SQL
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateLastEdiMessageCountReceivedSQL, sSQLName:=ACUpdateLastEdiMessageCountReceivedName, bStoredProcedure:=ACUpdateLastEdiMessageCountReceivedStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update last message count received", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="UpdateLastEdiMessageCountReceived")
                Return result
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateLastEdiMessageCountReceived Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="UpdateLastEdiMessageCountReceived", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteExistingReinsurance
    '
    ' Description:
    '
    ' History: 05/02/2008 RDT - Created.
    '
    ' ***************************************************************** '
    Public Function DeleteExistingReinsurance(ByVal v_lRiskCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", CStr(v_lRiskCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Call the SQL
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteExistingReinsuranceSQL, sSQLName:=ACDeleteExistingReinsuranceName, bStoredProcedure:=ACDeleteExistingReinsuranceStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update last message count received", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="DeleteExistingReinsurance")
                Return result
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteExistingReinsurance Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="DeleteExistingReinsurance", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteOriginalReinsurance
    '
    ' Description:
    '
    ' History: 05/02/2008 RDT - Created.
    '
    ' ***************************************************************** '
    Public Function DeleteOriginalReinsurance(
        ByVal v_lRiskCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", CStr(v_lRiskCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Call the SQL
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteOriginalReinsuranceSQL, sSQLName:=ACDeleteOriginalReinsuranceName, bStoredProcedure:=ACDeleteOriginalReinsuranceStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update last message count received", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="DeleteExistingReinsurance")
                Return result
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteExistingReinsurance Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="DeleteExistingReinsurance", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function
End Class
