Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Security.Cryptography
Imports System.Text
Imports System.Xml.Serialization
Imports System.Xml.XPath
Imports Microsoft.Reporting.WinForms
Imports SSP.Shared


<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 07/01/1999
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a SIRReportPrint.
    '
    ' Edit History:
    ' SJP14062002       moved to uniform Product Options scheme and gSIRLibrary.bas
    '                   not GSirLibraries.dll
    '
    ' RVH 11/11/2003    Switch off report option "morePrintEngineErrorMessages"
    ' CQ3208 IAG        for all reports as this can cause a dialog to be shown
    '                   on the server and will eventually cause degredation of
    '                   performance due to thread-blocking
    ' CJB04022005       PN18554 GetParameters changed to compare passed in param names
    '                   with report param names but in UCase format.
    ' ***************************************************************** '

    ' ************************************************
    ' Added to replace global variables 07/10/2003
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
    ' PN9975
    ' Multi Company
    Private m_bMultiCompany As Boolean
    ' Temp
    Private m_iSysLanguage As Integer
    'Start (Girija chokkalingam) - (Tech Spec - LOA009 - Hide Inactive Users.doc) - (5.2.1.1)
    Private m_bReportingOnDeletedData As Boolean
    Private Const ACReportingOnDeletedData As Integer = 5074
    'End (Girija chokkalingam) - (Tech Spec - LOA009 - Hide Inactive Users.doc) - (5.2.1.1)

    ' ************************************************

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Collection of SIRReportParameters (Private)
    Private m_oSIRReportParameters As Collection

    'DC270303 -ISS1911 -Start
    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database
    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean
    'DC270303 -ISS1911 -end

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private lPMAuthorityLevel As Integer

    'JK310399
    Private m_oSiriusDatabase As dPMDAO.Database
    'Private m_oOrionDatabase As dPMDAO.Database 'mkw220503 PN4333
    Private m_bCloseSiriusDatabase As Boolean
    'Private m_bCloseOrionDatabase As Boolean 'mkw220503 PN4333
    Private m_dPeriodEndDate As Date
    Private m_sYearName As String = ""

    Private m_sReportPath As String = ""
    ' JMK 12/07/2001 new variable for ReportName with UserID appended
    Private m_sUserReportName As String = ""
    Private m_sReportName As String = ""
    Private m_sDescription As String = String.Empty
    Private m_sReportTemplateLocation As String = ""
    Private m_sCustomer As String = ""
    Private m_sReportOutputLocation As String = ""
    Private m_iPrintJob As Integer
    Private m_iPrintReport As Integer

    ' To hold separate lists of reports

    Private m_colReportFolders As Collection
    Private m_colReportList() As Collection

    ' JMK31012001 Finds out if Underwriting or Agency business (U/W hidden option)
    Private m_sUnderwritingOrAgency As String = ""
    ' PRIVATE Data Members (End)

    'instead of padding UserID to report name we'll pad userID + time so ensure uniqueness
    Private m_sReportUniqueKey As String = ""

    '8.5
    Private m_bCalledFromCLI As Boolean
    Private m_sScheduleReportPath As String = ""
    Private m_sScheduleReportName As String = ""

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '
    ' AkashS    14-Oct-2010   Declare an object of CrystalDecisions.CrystalReports.Engine.ReportDocument
    '
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public m_oReportDocument As Report
    Private dFinancialEndDate, dThisFinancialYearEnd, dLastFinancialStartYear, dLastFinancialEndYear, dThisFinancialYearStart, dThisDateLastYear As Date
    Private sThisFinancialYearEnd, sLastFinancialStartYear, sLastFinancialEndYear, sThisFinancialYearStart, sThisDateLastYear As String


    ReadOnly Property MultiCompany() As Boolean
        Get
            Return m_bMultiCompany
        End Get
    End Property

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property
    ' JMK31012001 return "A" for Agency and "U" for Underwriting (U/W hidden option)
    Public ReadOnly Property UnderwritingOrAgency() As String
        Get

            If m_sUnderwritingOrAgency = "" Then
                m_lReturn = getUnderwritingOrAgency()
            End If

            Return m_sUnderwritingOrAgency

        End Get
    End Property
    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            Value = Value

        End Set
    End Property

    Public Property reportName() As String
        Get

            Return m_sReportName

        End Get
        Set(ByVal Value As String)

            m_sReportName = Value

        End Set
    End Property
    Public Property Description() As String
        Get
            Return m_sDescription
        End Get
        Set(value As String)
            m_sDescription = value
        End Set
    End Property

    Public WriteOnly Property PrintReport() As Integer
        Set(ByVal Value As Integer)

            m_iPrintReport = Value

        End Set
    End Property

    Public ReadOnly Property ReportTemplateLocation() As String
        Get
            ' Make sure paths are set
            m_lReturn = CType(GetReportPath(), gPMConstants.PMEReturnCode)
            Return m_sReportTemplateLocation

        End Get
    End Property

    Public ReadOnly Property CustomerName() As String
        Get

            ' Make sure paths are set
            m_lReturn = CType(GetCustomer(), gPMConstants.PMEReturnCode)

            Return m_sCustomer

        End Get
    End Property

    Public ReadOnly Property ReportOutputLocation() As String
        Get

            ' Make sure paths are set
            m_lReturn = CType(GetReportPath(), gPMConstants.PMEReturnCode)

            Return m_sReportOutputLocation

        End Get
    End Property

    Public ReadOnly Property ReportFolders() As Collection
        Get

            Return m_colReportFolders

        End Get
    End Property

    Public ReadOnly Property ReportList(ByVal iCount As Integer) As Collection
        Get

            Return m_colReportList(iCount)

        End Get
    End Property

    Public ReadOnly Property ReportParameters() As Collection
        Get

            Return m_oSIRReportParameters

        End Get
    End Property

    Public ReadOnly Property ReportUniqueKey() As String
        Get

            'Generating every time the unique key to avoid error... PN19342
            'If m_sReportUniqueKey = "" Then
            m_sReportUniqueKey = CStr(m_iUserID) & SSP.Shared.DateTimeHelper.Time.ToString("HHmmss")
            'End If

            Return m_sReportUniqueKey

        End Get
    End Property

    Public ReadOnly Property UserReportName() As String
        Get
            If m_sUserReportName = "" Then
                m_lReturn = CType(GetReportPath(), gPMConstants.PMEReturnCode)
            End If

            Return m_sUserReportName
        End Get
    End Property

    '8.5

    Public Property CalledFromCLI() As Boolean
        Get

            Return m_bCalledFromCLI

        End Get
        Set(ByVal Value As Boolean)

            m_bCalledFromCLI = Value

        End Set
    End Property

    Public Property ScheduleReportPath() As String
        Get

            Return m_sScheduleReportPath

        End Get
        Set(ByVal Value As String)

            m_sScheduleReportPath = Value

        End Set
    End Property

    Public Property ScheduleReportName() As String
        Get

            Return m_sScheduleReportName

        End Get
        Set(ByVal Value As String)

            m_sScheduleReportName = Value

        End Set
    End Property

    Private Function GetDbName(ByRef sDatabase As String) As Integer
        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = CType(GetDatabaseConnectItem(gPMConstants.PMSiriusSolutionsDSN, "Database", sDatabase), gPMConstants.PMEReturnCode)

        If sDatabase = "" Then
            result = gPMConstants.PMEReturnCode.PMNotFound
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetDatabaseConnectItem
    '
    ' Description: retrieves a database connection parameter
    '              from the registry
    '
    ' ***************************************************************** '
    Private Function GetDatabaseConnectItem(ByVal sDSN As String, ByVal sRegItem As String, ByRef sRegValue As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSubKey:="\Databases\" & sDSN, v_sSettingName:=sRegItem, r_sSettingValue:=sRegValue), gPMConstants.PMEReturnCode)

            sRegValue = sRegValue.Trim()

            Return gPMConstants.PMEReturnCode.PMTrue
        Catch

            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)
    ' ***************************************************************** '
    ' Name: GetUnderwritingOrAgency
    '
    ' Description:  Finds out if Underwriting or Agency business
    ' JMK31012001 (U/W hidden option)
    ' SJP14062002 moved to uniform Product Options scheme
    ' ***************************************************************** '
    Private Function getUnderwritingOrAgency() As Integer

        Dim result As Integer = 0


        Return bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_sUnderwritingOrAgency)

    End Function
    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        'PN9975
        Dim result As Integer = 0
        Dim vValue As String = ""
        'Start (Girija chokkalingam) - (Tech Spec - LOA009 - Hide Inactive Users.doc) - (5.2.1.2)
        Dim sOptionValue As String = ""
        'End (Girija chokkalingam) - (Tech Spec - LOA009 - Hide Inactive Users.doc) - (5.2.1.2)

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            ' Set Username and Password

            'JK310399

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseSiriusDatabase, r_oCheckedDatabase:=m_oSiriusDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_bCloseSiriusDatabase = False
            End If

            'mkw220503 PN4333 Remove orion database references
            '    m_lReturn& = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, _
            ''        v_lPMProductFamily:=pmePFOrion, _
            ''        r_bNewInstanceCreated:=m_bCloseOrionDatabase, _
            ''        r_oCheckedDatabase:=m_oOrionDatabase, _
            ''        v_vDatabase:=vDatabase)
            '
            '    If (m_lReturn <> PMTrue) Then
            '        m_bCloseOrionDatabase = False
            '    End If

            'DC270303 -ISS1911

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_bCloseDatabase = False
            End If

            ' PN9975
            m_lReturn = CType(bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiBranchCoreAccounts, v_vBranch:=m_iSourceID, r_vUnderwriting:=vValue), gPMConstants.PMEReturnCode)
            m_bMultiCompany = (vValue = "1")
            'Start (Girija chokkalingam) - (Tech Spec - LOA009 - Hide Inactive Users.doc) - (5.2.1.2)
            m_lReturn = CType(bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=ACReportingOnDeletedData, r_sOptionValue:=sOptionValue, v_iSourceID:=m_iSourceID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Initialise", "GetSystemOption failed to get the value for" & ACReportingOnDeletedData, gPMConstants.PMELogLevel.PMLogError)
            End If
            m_bReportingOnDeletedData = sOptionValue = "1"

            'End (Girija chokkalingam) - (Tech Spec - LOA009 - Hide Inactive Users.doc) - (5.2.1.2)

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

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
    Private ReportFolderName As String
    Private ConnectionString As String

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                If m_bCloseSiriusDatabase Then
                    ' Close the Database

                    m_oSiriusDatabase.CloseDatabase()

                End If
                m_oSiriusDatabase = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetReportPath
    '
    ' Description: Gets the Report Templates location from the registry.
    '
    ' ***************************************************************** '
    Public Function GetReportPath(Optional ByRef ReportTemplatePath As String = "") As Integer

        Dim result As Integer = 0
        Dim sRegPath As String = ""

        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set to LocalMachine/Sirius/Client
            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            ' Location for Report Templates
            sRegPath = ""
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Reports", r_sSettingValue:=sRegPath), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Report Template directory from Registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReportPath", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                'TR - Make sure that the path has a trailing backslash
                If Not sRegPath.EndsWith("\") Then
                    sRegPath = sRegPath & "\"
                End If
                m_sReportTemplateLocation = sRegPath
                ReportTemplatePath = sRegPath
            End If

            'KB 09042003 this function is called by the period end processing
            'but at that point we have no reportname - so this is failing
            'so period end reports aren't run
            'so change it to allow a pathe to be identified even if no
            'report name
            If m_sReportName <> "" Then
                ' JMK 12/07/2001 new variable for ReportName with UserID appended
                If m_sReportUniqueKey = "" Then
                    m_sUserReportName = m_sReportName & ReportUniqueKey
                Else
                    m_sUserReportName = m_sReportName & m_sReportUniqueKey
                End If
                'For Issue 44087
                If m_sReportName = "Remittance_Advice" Then
                    m_sReportName = "Navigator\Remittance_Advice"
                End If
                'For Issue PN-61851 (By Nitesh Dwivedi as on 20-Jan-2009)
                If m_sReportName = "Reconciliation_Report" Then
                    m_sReportName = "Navigator\Reconciliation_Report"
                End If
                'For Issue PN-1878 -  Ritu----------------
                If m_sReportName = "Cheque_Register" Then
                    m_sReportName = "Navigator\Cheque_Register"
                End If
                If m_sReportName = "Client_Statement_By_PartyCnt_U" Then
                    m_sReportName = "Navigator\Client_Statement_By_PartyCnt_U"
                End If
                If m_sReportName = "Insurer_Payment_Marked_Items" Then
                    m_sReportName = "Navigator\Insurer_Payment_Marked_Items"
                End If
                If m_sReportName = "Marked_For_Payment" Then
                    m_sReportName = "Navigator\Marked_For_Payment"
                End If
                If m_sReportName = "Marked_For_Reconciliation" Then
                    m_sReportName = "Navigator\Marked_For_Reconciliation"
                End If
                'For Issue PN-1749
                If m_sReportName = "Remittance_Advice_Agency" Then
                    m_sReportName = "Agency\Remittance_Advice_Agency"
                End If
                'For Issue PM027771
                If m_sReportName = "PLICO_certificate" Then
                    m_sReportName = "Underwriting\PLICO_certificate"
                End If
                'For Issue PM028018
                If m_sReportName = "PLICO_LossHistoryLetter" Then
                    m_sReportName = "Underwriting\PLICO_LossHistoryLetter"
                End If

                '--------------------------------------------
                'RWH(05/06/01) Make working copy of server report template.
                'TR - Fix bug where no
                Try
                    File.Copy(gPMFunctions.BuildFilePath(m_sReportTemplateLocation, m_sReportName & ".rdl"), gPMFunctions.BuildFilePath(m_sReportTemplateLocation, m_sUserReportName & ".rdl"))
                Catch ex As Exception

                End Try

            End If

            ' Location for Exported Reports
            sRegPath = ""
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="PrntFileDir", r_sSettingValue:=sRegPath), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Report Destination directory from Registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReportPath", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                m_sReportOutputLocation = sRegPath
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetReportPathFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReportPath", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetCustomer
    '
    ' Description:
    '
    ' History: 21/10/1999 JSB - Created.
    '
    ' ***************************************************************** '
    Public Function GetCustomer() As Integer

        Dim result As Integer = 0
        Dim sCustName As String = ""

        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'This _was_ just a copy of get report directory
            ' Exit if already set
            If m_sCustomer.Trim() > "" Then
                Return result
            End If

            ' Set to LocalMachine/Sirius/Client
            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            ' Get customer name from registry
            sCustName = ""
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Customer", r_sSettingValue:=sCustName), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Customer from Registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReportPath", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                m_sCustomer = sCustName
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCustomer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCustomer", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetReportsList
    '
    ' Description: Populate list of reports.
    '
    ' ***************************************************************** '
    Public Function GetReportsList() As Integer

        Dim result As Integer = 0
        Dim sEntry As String = ""
        Dim bTestAccess, bFoundReport As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Report Templates location from the Business object
            m_sReportPath = ReportTemplateLocation

            If CBool(CStr(m_sReportPath = "").Trim()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Customer Name from the Business object
            m_sCustomer = CustomerName

            ' TF220600 - Populate folders collection
            sEntry = FileSystem.Dir(m_sReportPath, FileAttribute.Directory)
            ' Add this and all other directories to collection
            m_colReportFolders = New Collection()
            Do Until sEntry = ""
                ' Make sure is directory, not normal file
                If FileSystem.GetAttr(m_sReportPath & sEntry) And FileAttribute.Directory Then
                    ' Don't include system directories
                    If (sEntry <> ".") And (sEntry <> "..") Then
                        '            And (sEntry$ <> "Navigator")) Then
                        m_colReportFolders.Add(sEntry)
                    End If
                End If
                sEntry = FileSystem.Dir()
            Loop

            ' Populate collections with report lists
            ReDim m_colReportList(m_colReportFolders.Count)
            For iCount As Integer = 1 To m_colReportList.GetUpperBound(0)
                m_colReportList(iCount) = New Collection()
                ' Special error trap for inaccessible folders
                bTestAccess = True

                sEntry = FileSystem.Dir(m_sReportPath & CStr(m_colReportFolders(iCount)) & "\*.rdl", FileAttribute.Normal)
                bTestAccess = False
                Do Until sEntry = ""
                    ' Save filename without .rpt
                    m_colReportList(iCount).Add(sEntry.Substring(0, sEntry.Length - 4))
                    sEntry = FileSystem.Dir()
                Loop
            Next iCount

            ' Check report is in collections if passed in Nav keys
            bFoundReport = False
            If m_sReportName <> "" Then
                For iCount As Integer = 1 To m_colReportList.GetUpperBound(0)
                    For Each vElement As String In m_colReportList(iCount)
                        If vElement.ToLower() = m_sReportName.ToLower() Then
                            bFoundReport = True

                            m_sReportName = CStr(m_colReportFolders(iCount)) & "\" & m_sReportName
                            Exit For
                        End If
                    Next vElement
                    If bFoundReport Then
                        Exit For
                    End If
                Next iCount
            End If

            Return result

        Catch excep As System.Exception

            If bTestAccess Then
                ' Access denied returns error 52
                ' Bad file name or number
                If Informations.Err().Number = 52 Then

                End If
            End If

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetReportsList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReportsList", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetLimitedReportList
    '
    ' Description:  JMK30012001 (U/W hidden option)
    '               Populate limited list of reports based on
    '               user's access rights
    '
    ' ***************************************************************** '
    Public Function GetLimitedReportList(ByRef r_vLimitedReportList(,) As Object) As Integer

        Dim result As Integer = 0
        Try
            Dim sSPSQL As String = ""
            sSPSQL = "spu_get_reports_limit_by_user"
            result = gPMConstants.PMEReturnCode.PMTrue


            m_oSiriusDatabase.Parameters.Clear()

            ' RVH 11/11/03 - CQ3208 : Sproc parameter name was incorrect and didn't match
            '                         the sproc (was "g_iUserID", now "UserID").
            m_lReturn = m_oSiriusDatabase.Parameters.Add(sName:="UserID", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oSiriusDatabase.SQLSelect(sSQL:=sSPSQL, sSQLName:="SelectLimitedReportList", bStoredProcedure:=True, vResultArray:=r_vLimitedReportList)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLimitedReportList")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLimitedReportList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLimitedReportList", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'Public Function GetReportDatSet(ByVal spName As String, ByRef parameters(,) As Object, ByRef dataset As Object) As Integer

    '    Dim result As Integer = 0
    '    Try
    '        Dim sSPSQL As String = ""
    '        sSPSQL = spName
    '        result = gPMConstants.PMEReturnCode.PMTrue

    '        If Information.IsArray(parameters) Then
    '            For iLimitCount As Integer = parameters.GetLowerBound(1) To parameters.GetUpperBound(1)
    '                sLtdReport = CStr(m_vLimitedReportList(0, iLimitCount))

    '            Next
    '        End If
    '        Dim re As New Dictionary(Of String, Object)  = parameters.to

    '        m_oSiriusDatabase.Parameters.Clear()


    '        m_lReturn = m_oSiriusDatabase.Parameters.Add(sName:="UserID", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

    '        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '            Return gPMConstants.PMEReturnCode.PMFalse
    '        End If

    '        m_lReturn = m_oSiriusDatabase.SQLSelect(sSQL:=sSPSQL, sSQLName:="SelectLimitedReportList", bStoredProcedure:=True, vResultArray:=r_vLimitedReportList)

    '        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '            ' Log Error Message
    '            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLimitedReportList")

    '            Return gPMConstants.PMEReturnCode.PMFalse
    '        End If

    '        Return result

    '    Catch excep As System.Exception

    '        result = gPMConstants.PMEReturnCode.PMError

    '        ' Log Error Message
    '        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLimitedReportList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLimitedReportList", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

    '        Return result

    '    End Try
    'End Function

    Public Function GetReportsFilterByReportGroupCode(ByVal v_sReportGroupCode As String, ByRef r_vLimitedReportList(,) As Object) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetReportsFilterByReportGroupCode
        ' PURPOSE: Gets a list of reports that belong to a report_group
        ' AUTHOR: Paul Cunnigham
        ' DATE: 31 October 2002, 15:34:54
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim sSPSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSPSQL = "spu_get_reports_limit_by_report_group"

            m_oSiriusDatabase.Parameters.Clear()

            m_lReturn = m_oSiriusDatabase.Parameters.Add(sName:="report_group_code", vValue:=v_sReportGroupCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            'Check for errors
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result

            m_lReturn = m_oSiriusDatabase.SQLSelect(sSQL:=sSPSQL, sSQLName:="GetReportsFilterByReportGroupCode", bStoredProcedure:=True, vResultArray:=r_vLimitedReportList)

            'Check for errors
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result

            result = gPMConstants.PMEReturnCode.PMTrue

            Return result
            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReportsFilterByReportGroupCode", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

                    Return result
            End Select

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetOtherDetails (Public)
    '
    ' Description: Gets the extra information
    '
    ' ***************************************************************** '
    Public Function GetOtherDetails(ByRef r_vRisks(,) As Object, ByRef r_vAccountExecutives(,) As Object, ByRef r_vAccountHandlers(,) As Object, ByRef r_vInsurers(,) As Object, ByRef r_vThirdParties(,) As Object, ByRef r_vGroups(,) As Object, ByRef r_vBranches(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            sSQL = "SELECT rc.risk_code_id, rc.code, c.caption" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM risk_code rc," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "pmcaption c" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE rc.caption_id = c.caption_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND c.language_id = " & CStr(m_iLanguageID) & Strings.ChrW(13) & Strings.ChrW(10)
            'DC220802 added check to ignore deleted risk codes
            'Start (Girija chokkalingam) - (Tech Spec - LOA009 - Hide Inactive Users.doc) - (5.2.1.3)
            If Not m_bReportingOnDeletedData Then
                sSQL = sSQL & "AND rc.is_deleted = 0" & Strings.ChrW(13) & Strings.ChrW(10)
            End If
            'End (Girija chokkalingam) - (Tech Spec - LOA009 - Hide Inactive Users.doc) - (5.2.1.3)
            sSQL = sSQL & "ORDER BY rc.code" & Strings.ChrW(13) & Strings.ChrW(10)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetOtherDetails4", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=r_vRisks)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            sSQL = "SELECT rg.risk_group_id, rg.code, c.caption" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM risk_group rg," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "pmcaption c" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE rg.caption_id = c.caption_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND c.language_id = " & CStr(m_iLanguageID) & Strings.ChrW(13) & Strings.ChrW(10)
            'DC220802 adde check to ignore deleted items
            'Start (Girija chokkalingam) - (Tech Spec - LOA009 - Hide Inactive Users.doc) - (5.2.1.3)
            If Not m_bReportingOnDeletedData Then
                sSQL = sSQL & "AND rg.is_deleted = 0" & Strings.ChrW(13) & Strings.ChrW(10)
            End If
            'End (Girija chokkalingam) - (Tech Spec - LOA009 - Hide Inactive Users.doc) - (5.2.1.3)
            sSQL = sSQL & "ORDER BY rg.code" & Strings.ChrW(13) & Strings.ChrW(10)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetOtherDetails5", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=r_vGroups)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Account Executives
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_code", vValue:="CO", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Start (Girija chokkalingam) - (Tech Spec - LOA009 - Hide Inactive Users.doc) - (5.2.1.3)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Branchcode", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="option_number", vValue:=CStr(ACReportingOnDeletedData), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'End (Girija chokkalingam) - (Tech Spec - LOA009 - Hide Inactive Users.doc) - (5.2.1.3)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectPartyNameSQL, sSQLName:=ACSelectPartyName, bStoredProcedure:=ACSelectPartyStored, vResultArray:=r_vAccountExecutives)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Account Handler
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_code", vValue:="AH", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectPartyNameSQL, sSQLName:=ACSelectPartyName, bStoredProcedure:=ACSelectPartyStored, vResultArray:=r_vAccountHandlers)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Insurer
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_code", vValue:="IN", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectPartyNameSQL, sSQLName:=ACSelectPartyName, bStoredProcedure:=ACSelectPartyStored, vResultArray:=r_vInsurers)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Third Party
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_code", vValue:="AG", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectPartyNameSQL, sSQLName:=ACSelectPartyName, bStoredProcedure:=ACSelectPartyStored, vResultArray:=r_vThirdParties)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectBranchSQL, sSQLName:=ACSelectBranchName, bStoredProcedure:=ACSelectBranchStored, vResultArray:=r_vBranches)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOtherDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOtherDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: InsertExcludedType
    '
    ' Description: Insert Excluded Types Into Table
    '
    ' ***************************************************************** '
    Public Function InsertExcludedType(ByVal sUniqueReportName As String, ByVal sType As String, ByVal lTypeId As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="unique_report_name", vValue:=sUniqueReportName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ExcludedType", vValue:=sType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ExcludedType_Id", vValue:=CStr(lTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACInsertExcludedTypeSQL, sSQLName:=ACInsertExcludedTypeName, bStoredProcedure:=ACInsertExcludedTypeStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Insert Excluded Types Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InsertExcludedType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DC270303 -ISS1911 -Start
    ' ***************************************************************** '
    ' Name: InsertGroupBy
    '
    ' Description: Insert Groups TO Be Used Within Reports
    '
    ' ***************************************************************** '
    Public Function InsertGroupBy(ByVal sGroupBy As String, ByVal lSessionID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="session_id", vValue:=CStr(lSessionID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Group", vValue:=sGroupBy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACInsertGroupBySQL, sSQLName:=ACInsertGroupByName, bStoredProcedure:=ACInsertGroupByStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Insert Group By Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InsertGroupBy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DC270303 -ISS1911 -Start
    ' ***************************************************************** '
    ' Name: InsertUser
    '
    ' Description: Insert Users TO Be Used Within Reports
    '
    ' ***************************************************************** '
    Public Function InsertUser(ByVal sUser As String, ByVal lSessionID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="session_id", vValue:=CStr(lSessionID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="User", vValue:=sUser, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACInsertReportUserSQL, sSQLName:=ACInsertReportUserName, bStoredProcedure:=ACInsertReportUserStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Insert Group By Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InsertUser", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: OpenReport
    '
    ' Description:  Open required report.'
    '
    ' ***************************************************************** '
    Private Function OpenReport() As Integer

        Dim result As Integer = 0
        Dim sDbName As String = ""
        ''Dim strLogOnInfo As Module1.PELogOnInfo = Module1.PELogOnInfo.CreateInstance()
        Dim sSectionCode As String = ""
        Dim sSubreportName As String = ""
        Dim sLoginId As String = ""
        Dim sPassword As String = ""
        Dim sTrusted As String = String.Empty
        result = gPMConstants.PMEReturnCode.PMTrue

        If m_sReportName.Contains("\") Then
            'm_sReportName = m_sReportName.Replace("\", "").Trim()
            m_sReportName = m_sReportName.TrimEnd(Path.DirectorySeparatorChar)
        End If

        m_sUserReportName = m_sReportName & m_sReportUniqueKey

        Return result
    End Function
    Public Function GetConnectionString(ByRef r_sConnectionString As String) As Integer

        Dim dPMDAO As New dPMDAO.Database()
        Dim result As Integer = 0

        Try
            dPMDAO.OpenDatabase(sSiriusUsername:=m_sUsername, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, sCallingAppName:=ACApp)
            r_sConnectionString = dPMDAO.ConnectionString
            result = gPMConstants.PMEReturnCode.PMTrue

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMFalse
        Finally
            If dPMDAO IsNot Nothing Then
                dPMDAO = Nothing
            End If

        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: CloseReport
    '
    ' Description:  Close current report.
    '
    ' ***************************************************************** '
    Public Function CloseReport() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'PEClosePrintJob(CShort(m_iPrintJob))
            'PECloseEngine()

            m_oReportDocument = Nothing

            'RWH(05/06/01) Remove template copy.
            If m_sReportTemplateLocation <> "" Then
                ' SET 29082002 - does the file exist...
                If FileSystem.Dir(m_sReportTemplateLocation & m_sUserReportName & ".rpt", FileAttribute.Normal) <> "" Then
                    File.Delete(m_sReportTemplateLocation & m_sUserReportName & ".rpt")
                End If
                ' SET 29082002 - End
            End If

            m_sReportTemplateLocation = ""
            m_sReportOutputLocation = ""
            m_sReportName = ""
            m_iPrintJob = 0

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CloseReport Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CloseReport", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name :ExportToDisk
    '
    ' Desc : export report to disk with user defined format
    '
    ' Hist : 29/01/2001 Tinny - Created
    ' ***************************************************************** '
    Public Function ExportToDisk(ByRef r_ExportFile As String, ByVal v_iFormatType As Integer, Optional ByVal v_vParameters As Object = "", Optional ByVal v_sExt As String = ".Doc",
                                               Optional ByVal v_bIsAccessedFromSAM As Boolean = False, Optional ByVal v_sSeparatedByName As String = "") As Integer


        Dim result As Integer = 0
        Dim oParameter As bSIRReportPrint.SIRReportParameter
        Dim lIDValue As Integer
        Dim sExt As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_sExt <> "" Then
                sExt = v_sExt
            Else
                sExt = ".Exp"
            End If

            m_lReturn = CType(OpenReport(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Deal with specials - if branch_id is used, assign the value of branch to it.
            If Informations.IsArray(v_vParameters) Then

                For iParamCount As Integer = 0 To v_vParameters.GetUpperBound(0)

                    Select Case v_vParameters(iParamCount, 0)
                        Case "branch_id"

                            For iCount As Integer = 0 To v_vParameters.GetUpperBound(0)

                                If CStr(v_vParameters(iCount, 0)) = "Branch" Then
                                    lIDValue = gPMFunctions.ToSafeLong(v_vParameters(iCount, 4))
                                    oParameter = m_oSIRReportParameters(iCount + 1)

                                    v_vParameters(iParamCount, 1) = oParameter.IDValues(v_vParameters(iCount, 4))
                                    Exit For
                                End If
                            Next iCount

                            '12/03/2003 - PWC - Issue (ref:2896)
                        Case "period_id", "perioddate"
                            Select Case m_sReportName.Trim().ToUpper()
                                Case ACRptName_AccountsEarnedPremium, ACRptName_AccountsUnearnedPremium, ACRptName_ClaimsOSClaims, ACRptName_ClaimsOSClaimsGrossToNet

                                    For iCount As Integer = 0 To v_vParameters.GetUpperBound(0)

                                        If CStr(v_vParameters(iCount, 0)) = "Period" Then
                                            lIDValue = gPMFunctions.ToSafeLong(v_vParameters(iCount, 4))
                                            oParameter = m_oSIRReportParameters(iCount + 1)

                                            v_vParameters(iParamCount, 1) = oParameter.IDValues(v_vParameters(iCount, 4))
                                            Exit For
                                        End If
                                    Next iCount
                            End Select


                        Case "batch_ref"
                            For iCount As Integer = 0 To v_vParameters.GetUpperBound(0)

                                If CStr(v_vParameters(iCount, 0)) = "batch_ref" Then
                                    lIDValue = gPMFunctions.ToSafeLong(v_vParameters(iCount, 4))
                                    oParameter = m_oSIRReportParameters(iCount + 1)

                                    v_vParameters(iParamCount, 1) = oParameter.IDValues(v_vParameters(iCount, 4))
                                    Exit For
                                End If
                            Next iCount


                    End Select
                Next iParamCount
            End If

            ' Pass parameters required for report
            If Informations.IsArray(v_vParameters) Then

                For iParamCount As Integer = 0 To v_vParameters.GetUpperBound(0)
                    oParameter = m_oSIRReportParameters(iParamCount + 1)
                    With oParameter

                        'developer guide no.24
                        .currentValue = v_vParameters(iParamCount, 1)
                    End With
                    ' m_lReturn = CType(oParameter.SendPropertiesToReport(), gPMConstants.PMEReturnCode)
                Next iParamCount
            End If

            ' Set the path of the compiled report
            ' JMK 12/07/2001 new variable for ReportName with UserID appended
            'r_ExportFile$ = m_sReportOutputLocation & m_sReportName$ & sExt
            'This code add broker name as prefix to report file with time stamp if separated By option has some value
            If Not String.IsNullOrEmpty(v_sSeparatedByName) Then
                If InStrRev(m_sUserReportName, "\") > 0 Then
                    r_ExportFile = r_ExportFile & "\" & v_sSeparatedByName & "-" & Microsoft.VisualBasic.Mid(m_sUserReportName, InStrRev(m_sUserReportName, "\") + 1) & "_" & DateTime.Now.ToString("ddMMyyyyHHmmss") & sExt
                Else
                    r_ExportFile = r_ExportFile & "\" & v_sSeparatedByName & "-" & m_sUserReportName & "_" & DateTime.Now.ToString("ddMMyyyyHHmmss") & sExt
                End If
                'This code add report file with time stamp if separated By option has no value.
            ElseIf r_ExportFile = "" Then
                r_ExportFile = m_sReportOutputLocation & m_sUserReportName & sExt
            Else
                If r_ExportFile.Contains(".") Then
                    r_ExportFile = Path.GetDirectoryName(r_ExportFile)
                End If
                r_ExportFile = r_ExportFile & "\" & Microsoft.VisualBasic.Mid(m_sUserReportName, InStrRev(m_sUserReportName, "\") + 1) & "_" & DateTime.Now.ToString("ddMMyyyyHHmmss") & sExt
            End If

            ''If Not v_bIsAccessedFromSAM Then
            ''    If v_sExt.ToUpper() = ".XLS" Or v_sExt.ToUpper() = ".XLSX" Then
            ''        m_oReportDocument.ExportToDisk(ExportFormatType.Excel, r_ExportFile)
            ''    Else
            ''        m_oReportDocument.ExportToDisk(ExportFormatType.WordForWindows, r_ExportFile)
            ''    End If
            ''ElseIf v_sExt.ToUpper() = ".PDF" Then
            ''    m_oReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, r_ExportFile)
            ''Else
            ''    m_oReportDocument.ExportToDisk(ExportFormatType.RichText, r_ExportFile)
            ''End If

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(CloseReport(), gPMConstants.PMEReturnCode)
                Return result
            End If
            Dim sConnectionString As String = ""

            If v_sExt = ".CSV" Then
                GetConnectionString(sConnectionString)
                ConnectionString = sConnectionString

                ReportExport("csv", sConnectionString, r_ExportFile, m_sReportPath, reportName, v_vParameters)
                'm_oReportDocument.ExportToDisk(
                'm_lReturn = CType(PEStartPrintJob(CShort(m_iPrintJob), IIf(False, CShort(-1), CShort(0))), gPMConstants.PMEReturnCode)
            Else
                GetConnectionString(sConnectionString)
                ConnectionString = sConnectionString

                ReportExport("word", sConnectionString, r_ExportFile, m_sReportPath, reportName, v_vParameters)
                'm_lReturn = CType(PEStartPrintJob(CShort(m_iPrintJob), IIf(True, CShort(-1), CShort(0))), gPMConstants.PMEReturnCode)
                'm_oReportDocument.ExportToDisk(ExportFormatType.RichText, ScheduleReportPath)
            End If

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(CloseReport(), gPMConstants.PMEReturnCode)
                Return result
            End If

            m_lReturn = CType(CloseReport(), gPMConstants.PMEReturnCode)

            'If CalledFromCLI Then
            '    r_ExportFile = ScheduleReportPath
            'End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExportToDisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExportToDisk", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SendToPrint
    '
    ' Description:
    ' Changes:
    ' TB 12/07/2002:  Add optional parameter r_sCrystalErrorLine.
    ' Note: breaks compatibility as its a public interface.
    ' Returns Crystal Reports error information to the client
    ' ***************************************************************** '
    'developer guide no.101
    '    Public Function GetReportDetails(reportPath As String, reportName As String,
    '<[Optional]> isServerPath As Boolean) As Report
    '        If Not isServerPath Then
    '            Dim doc = XDocument.Load(reportPath & reportName & ".rdl")
    '            Dim sse = doc.XPathSelectElement("/*[local-name() = 'Report']")
    '            Dim serializer = New XmlSerializer(GetType(Report))
    '            Dim result = CType(serializer.Deserialize(sse.CreateReader()), Report)
    '            Return result
    '        End If
    '        Return Nothing
    '    End Function

    '    Public Function GetReportDataSet(reportPath As String, reportName As String, reportParameters As Object(,),
    '<[Optional]> isServerPath As Boolean) As ReportDataSets

    '        Dim report As Report = New Report()
    '        Dim reportDataSets As ReportDataSets = New ReportDataSets()
    '        Dim reportDataSet As List(Of ReportDataSet) = New List(Of ReportDataSet)()
    '        If reportPath.EndsWith("\") = False Then
    '            reportPath = reportPath & "\"

    '        End If

    '        report = GetReportDetails(reportPath, reportName)
    '        If report.DataSets IsNot Nothing AndAlso report.DataSets.DataSet.Count > 0 Then
    '            For Each ds As DataSet In report.DataSets.DataSet
    '                If ds IsNot Nothing AndAlso ds.Query IsNot Nothing Then
    '                    Dim reportDataSet1 As ReportDataSet = New ReportDataSet()
    '                    reportDataSet1.DataSetName = ds.Name
    '                    reportDataSet1.SqlCommandType = ds.Query.CommandType
    '                    reportDataSet1.SqlCommandText = ds.Query.CommandText
    '                    If ds.Query.QueryParameters IsNot Nothing AndAlso ds.Query.QueryParameters.QueryParameter.Count > 0 Then
    '                        reportDataSet1.ReportQueryParameters = New Dictionary(Of String, Object)()
    '                        'var queryParameters = new Dictionary<string, object>();
    '                        'for (int i = 0; i < reportParameters.GetLength(0); i++)
    '                        '{
    '                        '    queryParameters.Add(reportParameters[i, 0].ToString(), reportParameters[i, 1]);
    '                        '}



    '                        For Each queryParameter As QueryParameter In ds.Query.QueryParameters.QueryParameter
    '                            'var name = queryParameter.Name.Replace("@", "").Trim();
    '                            'object value ;
    '                            'if (queryParameters.TryGetValue(name,out value ))
    '                            '{
    '                            reportDataSet1.ReportQueryParameters.Add(queryParameter.Name, queryParameter.Value)

    '                            '}
    '                            'someVal = queryParameters[someKey];

    '                            'for (int i = 0; i < reportParameters.GetLength(0); i++)
    '                            '{
    '                            '    queryParameters.Add(reportParameters[i, 0].ToString(), reportParameters[i, 1]);
    '                            '}
    '                            ' for (int i = 0; i < reportParameters.GetLength(0); i++)
    '                            '{
    '                            '    queryParameters.Add(reportParameters[i, 0].ToString(), reportParameters[i, 1]);
    '                            '}

    '                        Next

    '                    End If
    '                    reportDataSet.Add(reportDataSet1)
    '                End If

    '            Next

    '        End If
    '        reportDataSets.ReportDataSet = New List(Of ReportDataSet)()
    '        reportDataSets.ReportDataSet = reportDataSet
    '        Return reportDataSets

    '    End Function

    '    Public Sub ReportExport(format As String, connectionString As String, compileReportPath As String, reportPath As String, reportName As String, reportParameters As Object(,))
    '        Dim viewer = New Microsoft.Reporting.WinForms.ReportViewer() With {
    ' .ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local
    '}
    '        Dim devInfo = "<DeviceInfo><Toolbar>False</Toolbar></DeviceInfo>"

    '        Dim localReport As Microsoft.Reporting.WinForms.LocalReport = viewer.LocalReport
    '        localReport.ReportPath = reportPath & reportName & ".rdl"
    '        ' localReport.LoadReportDefinition()(Stream, From _ In _ Select _)
    '        Dim reportDataSets As ReportDataSets = GetReportDataSet(reportPath, reportName, reportParameters)

    '        localReport.DataSources.Clear()
    '        Dim localReportParameter As List(Of Microsoft.Reporting.WinForms.ReportParameter) = New List(Of Microsoft.Reporting.WinForms.ReportParameter)()
    '        Dim dsReport As Data.DataSet = GetReportData(connectionString, reportDataSets.ReportDataSet(0), reportParameters, localReportParameter)

    '        'reportDS.Name = "DataSet1";
    '        'reportDS.Value = dsReport.Tables[0];

    '        localReport.DataSources.Add(New Microsoft.Reporting.WinForms.ReportDataSource() With {
    ' .Name = "DataSet1",
    ' .Value = dsReport.Tables(0)
    '})
    '        Dim warningslocal As Microsoft.Reporting.WinForms.Warning() = Nothing
    '        Dim encoding As String
    '        Dim streamIds As String()
    '        Dim mimeType As String
    '        Dim extension As String
    '        localReport.SetParameters(localReportParameter)
    '        Dim renderingExtension As Microsoft.Reporting.WinForms.RenderingExtension() = localReport.ListRenderingExtensions()
    '        localReport.Refresh()
    '        Dim bytes = localReport.Render(format, devInfo, mimeType, encoding, extension, streamIds, warningslocal)

    '        Dim strings = compileReportPath.Split(New Char() {"."c})
    '        Dim lenght = strings.Length
    '        Dim extensionToReplace = strings(lenght - 1)
    '        compileReportPath = compileReportPath.Replace("." & extensionToReplace, "." & format)
    '        File.WriteAllBytes(compileReportPath, bytes)

    '    End Sub

    Public Function SendToPrint(Optional ByVal v_sReportTitle As String = "", Optional ByRef r_sCompiledReportPath As String = "", Optional ByVal v_vParameters As Object = Nothing, Optional ByVal v_sReportOPSubPath As String = "", Optional ByRef r_sCrystalErrorLine As String = "",
                Optional ByVal v_bIsAccessedFromSAM As Boolean = False) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            If m_sReportName.Contains("\") Then
                'm_sReportName = m_sReportName.Replace("\", "").Trim()
                m_sReportName = m_sReportName.TrimEnd(Path.DirectorySeparatorChar)
            End If
            Dim oParameter As bSIRReportPrint.SIRReportParameter
            Dim lIDValue As Integer
            'Dim reportOptions As Module1.PEReportOptions = New Module1.PEReportOptions()
            Dim sPathComponents() As String
            Dim iPathLevel As Integer
            Dim vResultArray(,) As Object

            m_sUserReportName = m_sReportName & m_sReportUniqueKey
            ' Deal with specials
            For iParamCount As Integer = 0 To v_vParameters.GetUpperBound(0)

                Select Case v_vParameters(iParamCount, 0)
                    Case "branch_id"
                        For iCount As Integer = 0 To v_vParameters.GetUpperBound(0)

                            If CStr(v_vParameters(iCount, 0)) = "Branch" Then
                                'lIDValue = gPMFunctions.ToSafeLong(v_vParameters(iCount, 4))
                                oParameter = m_oSIRReportParameters(iCount + 1)

                                v_vParameters(iParamCount, 1) = oParameter.IDValues(v_vParameters(iCount, 4))
                                Exit For
                            End If
                        Next iCount

                    Case "period_id", "perioddate"
                        Select Case m_sReportName.Trim().ToUpper()
                            Case ACRptName_AccountsEarnedPremium, ACRptName_AccountsUnearnedPremium, ACRptName_ClaimsOSClaims, ACRptName_ClaimsOSClaimsGrossToNet

                                For iCount As Integer = 0 To v_vParameters.GetUpperBound(0)

                                    If CStr(v_vParameters(iCount, 0)) = "Period" Then
                                        'lIDValue = gPMFunctions.ToSafeLong(v_vParameters(iCount, 4))
                                        oParameter = m_oSIRReportParameters(iCount + 1)

                                        v_vParameters(iParamCount, 1) = oParameter.IDValues(v_vParameters(iCount, 4))
                                        Exit For
                                    End If
                                Next iCount
                        End Select
                        'MKR 25/10/2004 PN 15730 - added special case for Profit and Loss reports...
                        'Getting the cost centre code from the IDValues Array.
                    Case "cost_centre"
                        Select Case m_sReportName.Trim().ToUpper()
                            Case ACRptName_AccountsProfitAndLossAll, ACRptName_AccountsProfitAndLossYear, ACRptName_AccountsProfitAndLossBudget

                                For iCount As Integer = 0 To v_vParameters.GetUpperBound(0)

                                    If CStr(v_vParameters(iCount, 0)) = "cost_centre" Then

                                        lIDValue = CInt(v_vParameters(iCount, 4))
                                        oParameter = m_oSIRReportParameters(iCount + 1)
                                        'PN16216 Don't pass in 0 for all as Stored Proc expects text
                                        If lIDValue = 0 Then

                                            v_vParameters(iParamCount, 1) = "ALL"
                                        Else

                                            v_vParameters(iParamCount, 1) = oParameter.IDValues(v_vParameters(iCount, 4))
                                        End If
                                        Exit For
                                    End If
                                Next iCount
                        End Select
                End Select
            Next iParamCount

            ' Pass parameters required for report
            'If Informations.IsArray(v_vParameters) Then
            '    For iParamCount As Integer = 0 To v_vParameters.GetUpperBound(0)
            '        oParameter = m_oSIRReportParameters(iParamCount + 1)
            '        With oParameter
            '            .Report = m_oReportDocument

            '            'developer guide no.24
            '            .currentValue = v_vParameters(iParamCount, 1)
            '        End With
            '        m_lReturn = CType(oParameter.SendPropertiesToReport(), gPMConstants.PMEReturnCode)
            '        Ne        xt iParamCount
            'End If

            ' Print Report if required
            If (m_iPrintReport = AC_PRINT_ONLY) Or (m_iPrintReport = AC_PRINT_AND_VIEW) Then

                ' m_oReportDocument.PrintToPrinter(1, False, 0, 0)

            End If

            Dim lRecordsSelected As Integer = 0
            If (m_iPrintReport = AC_VIEW_ONLY) Or (m_iPrintReport = AC_PRINT_AND_VIEW) Then
                m_sReportPath = ReportTemplateLocation
                ' Set the path of the compiled report
                r_sCompiledReportPath = m_sReportOutputLocation & m_sUserReportName & ".rdl"
                ' Delete existing report file

                If Not Directory.Exists(Path.GetDirectoryName(r_sCompiledReportPath)) Then
                    Directory.CreateDirectory(Path.GetDirectoryName(r_sCompiledReportPath))
                End If
                Dim sConnectionString As String = ""
                GetConnectionString(sConnectionString)
                ConnectionString = sConnectionString
                'Try
                '    ReportExport("pdf", sConnectionString, r_sCompiledReportPath, m_sReportPath, m_sReportName, v_vParameters)
                'Catch ex As Exception
                '    Return gPMConstants.PMEReturnCode.PMError
                'End Try
                If reportName = "Claims\PLICO_Claim_Payments_Deductible" Then
                    m_lReturn = GetClaimData(v_vParameters(0, 1), v_vParameters(1, 1), r_vResultArray:=vResultArray)

                    If Not Informations.IsArray(vResultArray) Then
                        Return gPMConstants.PMEReturnCode.PMNotFound
                    End If
                End If

            End If

            'Export Report to HTML
            If m_iPrintReport = AC_EXPORT_TO_HTML Then
                If v_bIsAccessedFromSAM Then
                    'Exported HTML should be created in a unique folder. So modify the path string accordingly
                    sPathComponents = m_sUserReportName.Split("\")
                    iPathLevel = sPathComponents.GetUpperBound(0)
                    If iPathLevel >= 0 Then
                        m_sUserReportName = m_sUserReportName & "\" & sPathComponents(iPathLevel)
                    End If
                End If
                If v_sReportOPSubPath <> "" Then
                    ' Set the path of the compiled report
                    ' PW230903 - PS259 - use reportname with user id appended
                    r_sCompiledReportPath = m_sReportOutputLocation & v_sReportOPSubPath & m_sUserReportName & ".html"
                Else
                    ' Set the path of the compiled report
                    ' PW230903 - PS259 - use reportname with user id appended
                    If CalledFromCLI Then
                        If r_sCompiledReportPath = "" Then
                            r_sCompiledReportPath = m_sReportOutputLocation
                        End If
                        If Not r_sCompiledReportPath.EndsWith("\") Then
                            r_sCompiledReportPath = r_sCompiledReportPath & "\"
                        End If
                        r_sCompiledReportPath = r_sCompiledReportPath & v_sReportTitle & "_" & DateTime.Now.ToString("ddMMyyyyHHmmss") & ".html"
                    Else
                        r_sCompiledReportPath = m_sReportOutputLocation & m_sUserReportName & ".html"
                    End If

                End If
                Dim sConnectionString As String = ""
                GetConnectionString(sConnectionString)
                ConnectionString = sConnectionString

                ReportExport("html", sConnectionString, r_sCompiledReportPath, m_sReportPath, m_sReportName, v_vParameters)
                ' m_oReportDocument.ExportToDisk(ExportFormatType.HTML40, r_sCompiledReportPath)

            ElseIf m_iPrintReport = AC_EXPORT_TO_PDF Then
                If v_sReportOPSubPath <> "" Then
                    ' Set the path of the compiled report
                    ' PW230903 - PS259 - use reportname with user id appended
                    r_sCompiledReportPath = m_sReportOutputLocation & v_sReportOPSubPath & m_sUserReportName & ".pdf"
                Else
                    ' Set the path of the compiled report
                    ' PW230903 - PS259 - use reportname with user id appended
                    If CalledFromCLI Then
                        If r_sCompiledReportPath = "" Then
                            r_sCompiledReportPath = m_sReportOutputLocation
                        End If
                        If Not r_sCompiledReportPath.EndsWith("\") Then
                            r_sCompiledReportPath = r_sCompiledReportPath & "\"
                        End If
                        If Not Directory.Exists(Path.GetDirectoryName(r_sCompiledReportPath)) Then
                            Directory.CreateDirectory(Path.GetDirectoryName(r_sCompiledReportPath))
                        End If
                        r_sCompiledReportPath = r_sCompiledReportPath & v_sReportTitle & "_" & DateTime.Now.ToString("ddMMyyyyHHmmss") & ".pdf"
                    Else
                        r_sCompiledReportPath = m_sReportOutputLocation & m_sUserReportName & ".pdf"
                    End If

                End If
                Dim sConnectionString As String = ""
                GetConnectionString(sConnectionString)
                ConnectionString = sConnectionString

                ReportExport("pdf", sConnectionString, r_sCompiledReportPath, m_sReportPath, m_sReportName, v_vParameters)
                'm_oReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, r_sCompiledReportPath)

            End If

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError
            '        'JMK 20/02/2002 - change iType and sMsg
            '        gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Run-time Error", vApp:=ACApp, vClass:=ACClass, vMethod:="SendtoToPrint", excep:=excep)

            Return result
        End Try
        Return result
    End Function
    Public Sub ReportExport(format As String, connectionString As String, compileReportPath As String, reportPath As String, reportName As String, reportParameters As Object(,))
        Dim viewer = New Microsoft.Reporting.WinForms.ReportViewer() With {
  .ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local
}
        Dim devInfo = "<DeviceInfo><Toolbar>False</Toolbar></DeviceInfo>"

        Dim localReport As Microsoft.Reporting.WinForms.LocalReport = viewer.LocalReport
        localReport.ReportPath = reportPath & reportName & ".rdl"
        ReportFolderName = reportName.Split("\")(0)
        Dim addBranchInLocalParameter As Boolean = False

        For Each rp As ReportParameterInfo In localReport.GetParameters()
            If rp.Name.ToLower() = "branch" Then
                addBranchInLocalParameter = True
                Exit For
            End If
        Next

        Dim reportDataSets As ReportDataSets = GetReportDataSet(reportPath, reportName, reportParameters, False)

        localReport.DataSources.Clear()
        Dim localReportParameter As List(Of Microsoft.Reporting.WinForms.ReportParameter) = New List(Of Microsoft.Reporting.WinForms.ReportParameter)()
        Dim dsReport As Data.DataSet = GetReportData(connectionString, reportDataSets.ReportDataSet(0), reportParameters, localReportParameter, addBranchInLocalParameter)

        localReport.DataSources.Add(New Microsoft.Reporting.WinForms.ReportDataSource() With {
  .Name = "DataSet1",
  .Value = dsReport.Tables(0)
})
        ' Add a handler for SubreportProcessing.
        AddHandler localReport.SubreportProcessing, AddressOf Me.SubreportProcessingEventHandler

        Dim warningslocal As Microsoft.Reporting.WinForms.Warning() = Nothing
        Dim encoding As String = String.Empty
        Dim streamIds As String() = Nothing
        Dim mimeType As String = String.Empty
        Dim extension As String = String.Empty
        localReport.SetParameters(localReportParameter)
        Dim renderingExtension As Microsoft.Reporting.WinForms.RenderingExtension() = localReport.ListRenderingExtensions()
        localReport.Refresh()
        Dim bytes = localReport.Render(format, devInfo, mimeType, encoding, extension, streamIds, warningslocal)

        Dim strings = compileReportPath.Split(New Char() {"."c})
        Dim lenght = strings.Length
        Dim extensionToReplace = strings(lenght - 1)
        compileReportPath = compileReportPath.Replace("." & extensionToReplace, "." & format)
        File.WriteAllBytes(compileReportPath, bytes)

    End Sub
    Public Function GetReportDataSet(reportPath As String, reportName As String, reportParameters As Object(,),
<[Optional]> isServerPath As Boolean) As ReportDataSets

        Dim report As Report = New Report()
        Dim reportDataSets As ReportDataSets = New ReportDataSets()
        Dim reportDataSet As List(Of ReportDataSet) = New List(Of ReportDataSet)()
        If reportPath.EndsWith("\") = False Then
            reportPath = reportPath & "\"

        End If

        report = GetReportDetails(reportPath, reportName, False)
        If report.DataSets IsNot Nothing AndAlso report.DataSets.DataSet IsNot Nothing Then
            'foreach (DataSet ds in report.DataSets.DataSet)
            If True Then
                If report.DataSets.DataSet IsNot Nothing AndAlso report.DataSets.DataSet.Query IsNot Nothing Then
                    Dim reportDataSet1 As ReportDataSet = New ReportDataSet()
                    reportDataSet1.DataSetName = report.DataSets.DataSet.Name
                    reportDataSet1.SqlCommandType = report.DataSets.DataSet.Query.CommandType
                    reportDataSet1.SqlCommandText = report.DataSets.DataSet.Query.CommandText
                    If report.DataSets.DataSet.Query.QueryParameters IsNot Nothing AndAlso report.DataSets.DataSet.Query.QueryParameters.QueryParameter.Count > 0 Then
                        reportDataSet1.ReportQueryParameters = New Dictionary(Of String, Object)()
                        'var queryParameters = new Dictionary<string, object>();
                        'for (int i = 0; i < reportParameters.GetLength(0); i++)
                        '{
                        '    queryParameters.Add(reportParameters[i, 0].ToString(), reportParameters[i, 1]);
                        '}



                        For Each queryParameter As QueryParameter In report.DataSets.DataSet.Query.QueryParameters.QueryParameter
                            'var name = queryParameter.Name.Replace("@", "").Trim();
                            'object value ;
                            'if (queryParameters.TryGetValue(name,out value ))
                            '{
                            reportDataSet1.ReportQueryParameters.Add(queryParameter.Name, queryParameter.Value)

                            '}
                            'someVal = queryParameters[someKey];

                            'for (int i = 0; i < reportParameters.GetLength(0); i++)
                            '{
                            '    queryParameters.Add(reportParameters[i, 0].ToString(), reportParameters[i, 1]);
                            '}
                            ' for (int i = 0; i < reportParameters.GetLength(0); i++)
                            '{
                            '    queryParameters.Add(reportParameters[i, 0].ToString(), reportParameters[i, 1]);
                            '}

                        Next

                    End If
                    reportDataSet.Add(reportDataSet1)
                End If

            End If

        End If
        reportDataSets.ReportDataSet = New List(Of ReportDataSet)()
        reportDataSets.ReportDataSet = reportDataSet
        Return reportDataSets

    End Function
    Public Function GetSubReportDetails(reportPath As String, reportName As String) As SubReportQuery
        Dim reportWithPath As String = reportPath + reportName + ".rdlc"
        Dim doc As New XDocument
        doc = XDocument.Load(reportPath & "\" & reportName & ".rdlc")
        Dim query As New SubReportQuery
        query.DataSourceName = doc.XPathSelectElement("/*[local-name() = 'Report']/*[local-name()='DataSets']/*[local-name()='DataSet']/*[local-name()='Query']/*[local-name()='DataSourceName']")
        query.CommandText = doc.XPathSelectElement("/*[local-name() = 'Report']/*[local-name()='DataSets']/*[local-name()='DataSet']/*[local-name()='Query']/*[local-name()='CommandText']")
        query.CommandType = doc.XPathSelectElement("/*[local-name() = 'Report']/*[local-name()='DataSets']/*[local-name()='DataSet']/*[local-name()='Query']/*[local-name()='CommandType']")

        Return query
    End Function
    Private Sub RemoveNamespaces(ByRef element As XElement)
        If element.HasAttributes Then
            element.Attributes().Where(Function(a) a.IsNamespaceDeclaration).Remove()
        End If

        element.Name = element.Name.LocalName

        For Each child In element.Elements()
            RemoveNamespaces(child)
        Next
    End Sub

    Public Function GetReportDetails(reportPath As String, reportName As String,
<[Optional]> isServerPath As Boolean) As Report
        If Not isServerPath Then
            Dim doc = XDocument.Load(reportPath & reportName & ".rdl")

            Dim xElementDataSets = doc.XPathSelectElement("/*[local-name() = 'Report']/*[local-name()='DataSets']")
            RemoveNamespaces(xElementDataSets)
            Dim xElementParameters = doc.XPathSelectElement("/*[local-name() = 'Report']/*[local-name()='ReportParameters']")
            RemoveNamespaces(xElementParameters)

            Dim serializer = New XmlSerializer(GetType(DataSets))
            Dim report = New Report()
            report.DataSets = New DataSets()
            report.DataSets = CType(serializer.Deserialize(xElementDataSets.CreateReader()), DataSets)

            serializer = New XmlSerializer(GetType(ReportParameters))
            report.ReportParameters = New ReportParameters()
            report.ReportParameters = CType(serializer.Deserialize(xElementParameters.CreateReader()), ReportParameters)
            Return report
        End If
        Return Nothing
    End Function

    Public Function GetParameters(ByRef r_vParameters As Object, ByRef r_vDefaultValues As Object) As Integer

        Const kNumParameters As Integer = 11

        Dim result As Integer = 0
        Dim NumberOfParameters As Integer
        Dim oParameter As bSIRReportPrint.SIRReportParameter
        Dim paramcount As Integer = 0
        Dim vParametersIn(,) As Object ' Retain known parameters

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear existing collection
            'Set m_oSIRReportParameters = Nothing
            m_lReturn = CType(ClearParameters(), gPMConstants.PMEReturnCode)

            vParametersIn = r_vParameters

            r_vParameters = Nothing

            If m_sReportName.Contains("\") Then
                m_sReportName = m_sReportName.TrimEnd(Path.DirectorySeparatorChar)
            End If

            m_sUserReportName = m_sReportName & m_sReportUniqueKey
            If (String.IsNullOrEmpty(m_sReportPath)) Then
                m_sReportPath = ReportTemplateLocation
            End If
            m_oReportDocument = GetReportDetails(m_sReportPath, m_sReportName, False)
            ''sandeep
            ''m_lReturn = CType(OpenReport(), gPMConstants.PMEReturnCode)
            ''If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ''    Return gPMConstants.PMEReturnCode.PMFalse
            ''End If

            ' Get parameters from report
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '
            'AkashS     12-Oct-2010     Get Number of Report Parameters using ReportDocument object
            '
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim bBranchFound As Boolean = False
            Dim selectedValue = m_oReportDocument.ReportParameters.ReportParameter.Find(Function(p)
                                                                                            Dim value As Boolean = False
                                                                                            If p.Name = "Branch" Then
                                                                                                value = True
                                                                                            End If
                                                                                            Return value
                                                                                        End Function)
            NumberOfParameters = m_oReportDocument.ReportParameters.ReportParameter.Count '.ParameterFields.Count 'PEGetNParameterFields(printJob:=CShort(m_iPrintJob))

            If selectedValue Is Nothing Then
                For Each param As ReportParameter In m_oReportDocument.ReportParameters.ReportParameter
                    If (param.Name.ToLower() = "branch_id") Then
                        NumberOfParameters = NumberOfParameters + 1
                        bBranchFound = True
                        Exit For
                    End If
                Next
            End If
            If NumberOfParameters = -1 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'JK010499
            'Get Current Financial Year end date
            m_lReturn = CType(GetFinancialEndDate(dFinancialEndDate), gPMConstants.PMEReturnCode)

            'Set Financial Year dates
            dThisFinancialYearEnd = dFinancialEndDate
            dLastFinancialEndYear = dThisFinancialYearEnd
            dLastFinancialEndYear = dThisFinancialYearEnd.AddYears(-1)
            dLastFinancialStartYear = dThisFinancialYearEnd
            dLastFinancialStartYear = dLastFinancialEndYear.AddDays(1)
            dLastFinancialStartYear = dLastFinancialStartYear.AddYears(-1)
            dThisFinancialYearStart = dThisFinancialYearEnd
            dThisFinancialYearStart = dThisFinancialYearEnd.AddDays(1)
            dThisFinancialYearStart = dThisFinancialYearStart.AddYears(-1)
            dThisDateLastYear = DateTime.Today
            dThisDateLastYear = dThisDateLastYear.AddYears(-1)

            'Format dates
            sThisFinancialYearStart = dThisFinancialYearStart.ToString("dd/MM/yyyy")
            sLastFinancialEndYear = dLastFinancialEndYear.ToString("dd/MM/yyyy")
            sLastFinancialStartYear = dLastFinancialStartYear.ToString("dd/MM/yyyy")
            sThisFinancialYearEnd = dThisFinancialYearEnd.ToString("dd/MM/yyyy")
            sThisDateLastYear = dThisDateLastYear.ToString("dd/MM/yyyy")

            ReDim r_vParameters(NumberOfParameters - 1, kNumParameters)

            ReDim r_vDefaultValues(NumberOfParameters - 1, 0)
            Dim bIncreaseParamCount As Boolean
            Dim counter As Integer = NumberOfParameters

            If bBranchFound Then
                counter = counter - 1
            End If


            For iParamCount As Integer = 0 To counter - 1
                oParameter = New bSIRReportPrint.SIRReportParameter()

                With oParameter
                    .Business = Me
                    .printJob = m_iPrintJob
                    .ParamIndex = iParamCount
                    .reportName = m_sReportName
                    .Report = m_oReportDocument

                    If (m_oReportDocument.ReportParameters.ReportParameter(iParamCount).Name.ToLower() = "branch_id") AndAlso selectedValue Is Nothing Then
                        m_lReturn = CType(.ProcessBranch(), gPMConstants.PMEReturnCode)


                        m_lReturn = CType(ProcessParameters(iParamCount, oParameter, r_vDefaultValues, r_vParameters, NumberOfParameters, vParametersIn, iParamCount), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        r_vParameters(iParamCount, 0) = "Branch"
                        ' Check if value passed in
                        If Informations.IsArray(vParametersIn) Then

                            For iPassedParamCount As Integer = 0 To vParametersIn.GetUpperBound(1)

                                If CStr(vParametersIn(0, iPassedParamCount)).ToUpper() = .Name.ToUpper() Then ' PN18554

                                    'developer guide no.24
                                    .currentValue = vParametersIn(1, iPassedParamCount)
                                    Exit For
                                End If
                            Next iPassedParamCount
                        End If

                        If Not .currentValue Is Nothing Then
                            r_vParameters(iParamCount, 1) = .currentValue
                        End If

                        r_vParameters(iParamCount, 2) = 8

                        r_vParameters(iParamCount, 3) = "Branch:"

                        If Not .CurrentIDValue Is Nothing Then
                            r_vParameters(iParamCount, 4) = .CurrentIDValue.Trim()
                            'eck190602
                        End If

                        bIncreaseParamCount = True

                    End If
                    m_lReturn = CType(.SetPropertiesFromReport(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    Dim count As Integer = iParamCount
                    If (bIncreaseParamCount) Then
                        count = count + 1
                    End If
                    m_lReturn = CType(ProcessParameters(count, oParameter, r_vDefaultValues, r_vParameters, NumberOfParameters, vParametersIn, iParamCount), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End With

            Next iParamCount

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetParametersFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetParameters", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Private Function ProcessParameters(ByVal iParamCount As Integer, ByRef oParameter As bSIRReportPrint.SIRReportParameter, ByRef r_vDefaultValues As Object, ByRef r_vParameters As Object, ByVal NumberOfParameters As Integer, ByRef vParametersIn As Object(,), ByVal defaultValuesCount As Integer) As Integer

        Dim result As Integer = 0
        Dim paramcount As Integer = 0
        Dim lDDLimit As Integer
        Dim vDVal As Object
        Dim iLow As Integer

        m_oSIRReportParameters.Add(oParameter)
        With oParameter
            ' Set default username & dates if required
            Select Case .Name.ToLower()
                Case "operator"
                    If paramcount = 0 Then
                        'developer guide no.24
                        .currentValue = m_sUsername.Trim()
                        paramcount += 1
                    End If
                Case "financialyearend"

                    .currentValue = sThisFinancialYearEnd
                Case "financialyearstart"

                    .currentValue = sThisFinancialYearStart
                Case "lastfinancialyearstart"

                    .currentValue = sLastFinancialStartYear
                Case "lastfinancialyearend"

                    .currentValue = sLastFinancialEndYear
                Case "thisdatelastyear"

                    .currentValue = sThisDateLastYear

                Case "Else"
                    .currentValue = Nothing
            End Select
            ' Set defaults array
            If Informations.IsArray(.DefaultValues) Then

                If .DefaultValues.GetUpperBound(0) > r_vDefaultValues.GetUpperBound(1) Then
                    ReDim Preserve r_vDefaultValues(NumberOfParameters - 1, .DefaultValues.GetUpperBound(0))
                End If
                ' TB 10/07/2002
                ' previous For/Next code called the .DefaultValues 3 times per iteration
                ' Now it called 3 times in total .  RSAIB had 21000 items in the
                ' array, previous For/Next took 10 mins, now takes 1 second

                vDVal = .DefaultValues ' get whole array in one call to property get

                iLow = vDVal.GetLowerBound(0)

                If vDVal.GetUpperBound(0) > ACDropDownLimit Then
                    lDDLimit = ACDropDownLimit
                Else

                    lDDLimit = vDVal.GetUpperBound(0)
                End If
                For lDefaultCount As Integer = iLow To lDDLimit

                    r_vDefaultValues(iParamCount, lDefaultCount) = vDVal(lDefaultCount) '.DefaultValues(lDefaultCount)
                Next lDefaultCount
            End If

            ' Check if value passed in
            If Informations.IsArray(vParametersIn) Then

                For iPassedParamCount As Integer = 0 To vParametersIn.GetUpperBound(1)

                    If vParametersIn(0, iPassedParamCount) IsNot Nothing AndAlso CStr(vParametersIn(0, iPassedParamCount)).ToUpper() = .Name.ToUpper() Then ' PN18554

                        'developer guide no.24
                        .currentValue = vParametersIn(1, iPassedParamCount)
                        Exit For
                    End If
                Next iPassedParamCount
            End If

            r_vParameters(iParamCount, 0) = .Name.Trim()

            If Not .currentValue Is Nothing Then
                r_vParameters(iParamCount, 1) = .currentValue
            End If

            r_vParameters(iParamCount, 2) = CStr(.valueType).Trim()

            r_vParameters(iParamCount, 3) = .Prompt.Trim()

            If Not .CurrentIDValue Is Nothing Then
                r_vParameters(iParamCount, 4) = .CurrentIDValue.Trim()
                'eck190602
            End If

            If Not .PartySearch Is Nothing Then
                r_vParameters(iParamCount, 5) = .PartySearch.Trim()
            End If

            If Not .ParentParamName Is Nothing Then
                r_vParameters(iParamCount, 6) = .ParentParamName.Trim()
            End If

            If Not .TableName Is Nothing Then
                r_vParameters(iParamCount, 7) = .TableName.Trim()
            End If

            If Not .IDFieldName Is Nothing Then
                r_vParameters(iParamCount, 8) = .IDFieldName.Trim()
            End If

            If Not .DescriptionFieldName Is Nothing Then
                r_vParameters(iParamCount, 9) = .DescriptionFieldName.Trim()
            End If
            r_vParameters(iParamCount, 10) = .IsMultiSelect

            If Not .CustomStoredProcedure Is Nothing Then
                r_vParameters(iParamCount, 11) = .CustomStoredProcedure.Trim()
            End If
        End With
        Return 1
    End Function
    ' ***************************************************************** '
    ' Name: GetFinancialEndDate
    '
    ' Description: Gets the PeriodEndDate,Yearname for Period12(Last period
    '                                                           in year)
    ' JK010499
    '
    ' ***************************************************************** '
    Public Function GetFinancialEndDate(ByRef dFinancialEndDate As Date) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim r_vResultArray(,) As Object = Nothing
        Dim m_lError As Integer

        Dim dCurrentDate As Date
        Dim iThisYear As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            dCurrentDate = DateTime.Today
            'set This Year
            iThisYear = dCurrentDate.Year

            sSQL = ""
            sSQL = sSQL & "SELECT period_end_date, year_name" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " FROM Period" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " WHERE period_id = (Select Max(period_id) From Period Where DATEPART(yyyy,period_end_date) =" & iThisYear & ")"

            '' Execute SQL Statement - use array for speed
            'mkw220503 PN4333 - Changed to sirius database from orion database
            With m_oSiriusDatabase

                .Parameters.Clear()

                m_lError = .SQLSelect(sSQL:=sSQL, sSQLName:=ACFinancialYearEndQueryName, bStoredProcedure:=ACFinancialYearEndQueryStored, vResultArray:=r_vResultArray)

                ' If NO records were found return PMFalse
                If Not Informations.IsArray(r_vResultArray) Then
                    Return gPMConstants.PMEReturnCode.PMNotFound
                Else
                    m_dPeriodEndDate = CDate(r_vResultArray(0, 0))
                End If
            End With

            dFinancialEndDate = m_dPeriodEndDate

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFinancialEndDateFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFinancialEndDate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetInsurerID
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function GetInsurerID(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = ""
            sSQL = sSQL & "SELECT GIS_Insurer_ID" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " FROM GIS_Insurer" & Strings.ChrW(13) & Strings.ChrW(10)
            If Not m_bReportingOnDeletedData Then
                sSQL = sSQL & " WHERE is_deleted = " & "0" & ""
            End If

            ' Execute SQL Statement - use array for speed
            With m_oSiriusDatabase
                .Parameters.Clear()
                lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="ACGetInsurerID", bStoredProcedure:=False, vResultArray:=vResultArray)
                ' If NO records were found return PMFalse
                If Not Informations.IsArray(vResultArray) Then
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If
            End With

            'Return Array
            r_vResultArray = vResultArray

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInsurerIDFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsurerID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DC270303 -ISS1911
    ' ***************************************************************** '
    ' Name: GetNextSessionId
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function GetNextSessionId(ByRef r_lSessionId As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Execute SQL Statement - use array for speed
            With m_oSiriusDatabase

                .Parameters.Clear()

                lReturn = .Parameters.Add(sName:="r_lSessionId", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

                lReturn = .SQLSelect(sSQL:=ACGetNextReportSessionSQL, sSQLName:=ACGetNextReportSessionName, bStoredProcedure:=ACGetNextReportSessionStored)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                r_lSessionId = .Parameters.Item("r_lSessionId").Value

            End With

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNextSessionIDFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNextSessionID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DC270303 -ISS1911
    ' ***************************************************************** '
    ' Name: ClearSessionId
    '
    ' Description: To Clear Session Id That Was Being Used For This Run
    '
    ' ***************************************************************** '
    Public Function ClearSessionId(ByRef r_lSessionId As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Execute SQL Statement - use array for speed
            With m_oSiriusDatabase

                .Parameters.Clear()

                lReturn = .Parameters.Add(sName:="lSessionId", vValue:=CStr(r_lSessionId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                lReturn = .SQLSelect(sSQL:=ACClearReportSessionSQL, sSQLName:=ACClearReportSessionName, bStoredProcedure:=ACClearReportSessionStored)
            End With

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ClearSessionIDFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearSessionID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetParametersFromDB
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function GetParametersFromDB(ByVal v_sDatabaseName As String, ByVal v_sTableName As String, ByVal v_sDisplayFieldName As String, ByVal v_sIDFieldName As String, ByRef r_vDefaultValues As Object) As Integer

        Dim result As Integer = 0
        Dim oDatabase As dPMDAO.Database
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim lReturn As Integer
        Dim bIsFound As Boolean
        Dim sPartyType As String = ""
        Dim bIsGenMainFound As Boolean 'is_generic_maintenance field 
        Dim bIsUDLTable As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oDatabase = m_oSiriusDatabase

            ' Execute SQL Statement - use array for speed
            With oDatabase

                .Parameters.Clear()

                'Check special for Party
                If v_sTableName.StartsWith("party-") Then 'Developer Guide No
                    sPartyType = v_sTableName.Substring(7)
                    v_sTableName = "Party"
                End If

                If (v_sTableName$.Substring(0, 4).ToLower() = "udl_") Then
                    bIsUDLTable = True
                End If

                ' Check for is_delete field
                ' PW230502 - change back to sp_columns from spu_columns - this is a master database sp
                sSQL = "sp_columns @table_name = '"
                sSQL = sSQL & v_sTableName & "'"

                lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetColumns", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords)

                For iCount As Integer = 0 To .Records.Count() - 1
                    bIsFound = False
                    If .Records.Item(iCount).Fields()("column_name") = "is_deleted" Then

                        bIsFound = True
                        Exit For

                    End If
                Next iCount

                If v_sTableName.Trim().ToUpper() = "PMPRODUCT_LOOKUP" Then
                    For iCount As Integer = 0 To .Records.Count() - 1
                        bIsGenMainFound = False
                        If .Records.Item(iCount).Fields()("column_name") = "is_generic_maintenance" Then

                            bIsGenMainFound = True
                            Exit For

                        End If
                    Next iCount
                End If

                ' Get values from DB

                If v_sTableName.ToLower() = "period" Then

                    '12/03/2003 - PWC - Issue (ref:2896)
                    Select Case m_sReportName.Trim().ToUpper()
                        '21/05/2003 - PWC - No future periods allowed for these 2 reports
                        Case ACRptName_AccountsEarnedPremium, ACRptName_AccountsUnearnedPremium

                            sSQL = ""
                            sSQL = sSQL & "SELECT convert(char(11), period_end_date, 100)" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & ", period_id" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "FROM Period" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "WHERE ( period_end_complete = 1" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "OR year_name = (" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "select distinct year_name from period" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "WHERE period_end_date = ( SELECT min(period_end_Date)" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "FROM period " & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "WHERE period_end_date >= getdate() )" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & ") )" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "AND company_id = 1" & Strings.ChrW(13) & Strings.ChrW(10)
                            '21/05/2003 - PWC - No future periods allowed for these 2 reports
                            'so added this bit
                            sSQL = sSQL & "AND (" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "    period_end_date <= (" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "        SELECT" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "            Min (period_end_date)" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "        From" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "            period" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "        Where" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "            period_end_date >= getdate()" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "    )" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & ")" & Strings.ChrW(13) & Strings.ChrW(10)

                            sSQL = sSQL & "ORDER BY period_end_date DESC" & Strings.ChrW(13) & Strings.ChrW(10)

                        Case ACRptName_ClaimsOSClaims, ACRptName_ClaimsOSClaimsGrossToNet

                            sSQL = ""
                            sSQL = sSQL & "SELECT convert(char(11), period_end_date, 100)" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & ", period_id" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "FROM Period" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "WHERE ( period_end_complete = 1" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "OR year_name = (" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "select distinct year_name from period" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "WHERE period_end_date = ( SELECT min(period_end_Date)" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "FROM period " & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "WHERE period_end_date >= getdate() )" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & ") )" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "AND company_id = 1" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "ORDER BY period_end_date DESC" & Strings.ChrW(13) & Strings.ChrW(10)
                        Case ACRptName_TrialBalance, ACRptName_TrialBalanceSummary

                            sSQL = ""
                            sSQL = sSQL & "SELECT convert(char(11), period_end_date, 100)" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "FROM Period" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "Where period_end_Date  < =  " & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "(Select min(Period_end_date)  From Period " & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & " Where Period_end_Complete =0 And period_end_date> " & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "(Select max(period_end_date) From Period Where period_end_complete =1))" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "AND company_id = 1" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "ORDER BY period_end_date DESC" & Strings.ChrW(13) & Strings.ChrW(10)

                        Case Else 'As before

                            sSQL = ""
                            sSQL = sSQL & "SELECT convert(char(11), period_end_date, 100)" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "FROM Period" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "WHERE ( period_end_complete = 1" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "OR year_name <= (" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "select distinct year_name from period" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "WHERE period_end_date = ( SELECT min(period_end_Date)" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "FROM period " & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "WHERE period_end_date >= getdate() )" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & ") )" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "AND company_id = 1" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "ORDER BY period_end_date DESC" & Strings.ChrW(13) & Strings.ChrW(10)
                    End Select

                    'PN# 68632 START
                ElseIf (v_sTableName.ToLower() = "batch") Then
                    If (m_sReportName.ToUpper.Trim()) = ACRptName_AgentCommissionStatement Then

                        sSQL = "SELECT " & v_sDisplayFieldName
                        If v_sIDFieldName > "" Then
                            sSQL = sSQL & "," & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & If(bIsUDLTable, "0", v_sIDFieldName)
                        End If
                        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)

                        sSQL = sSQL & " FROM " & v_sTableName
                        sSQL = sSQL & " INNER JOIN BATCH_TYPE ON "
                        sSQL = sSQL & v_sTableName.Trim & ".batch_type_id = BATCH_TYPE.batch_type_id "
                        sSQL = sSQL & " AND BATCH_TYPE.code = 'CMS' "
                    End If
                    'PN# 68632 END


                ElseIf sPartyType = "" Then
                    If bIsUDLTable Then
                        sSQL$ = "SELECT DISTINCT (" & v_sDisplayFieldName$ & ")"
                    Else
                        sSQL = "SELECT " & v_sDisplayFieldName
                    End If

                    If v_sIDFieldName > "" Then
                        sSQL = sSQL & "," & Strings.ChrW(13) & Strings.ChrW(10)
                        sSQL = sSQL & If(bIsUDLTable, "0", v_sIDFieldName)
                    End If
                    sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & " FROM " & v_sTableName

                    If bIsFound Then
                        'Start (Girija chokkalingam) - (Tech Spec - LOA009 - Hide Inactive Users.doc) - (5.2.1.4)
                        If Not m_bReportingOnDeletedData Then
                            sSQL = sSQL & " WHERE (is_deleted IS NULL"
                            sSQL = sSQL & " OR is_deleted = 0"
                        Else
                            sSQL = sSQL & " WHERE (1=1"
                        End If
                        'End (Girija chokkalingam) - (Tech Spec - LOA009 - Hide Inactive Users.doc) - (5.2.1.4)

                        If v_sTableName.Trim().ToUpper() = "SOURCE" Then
                            sSQL = sSQL & " OR (is_deleted = 1 AND closed_allow_reports = 1)"
                        End If

                        sSQL = sSQL & ")"
                    End If

                    If v_sTableName.Trim().ToUpper() = "SOURCE" Then
                        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10) & "AND source_id NOT IN (SELECT source_id FROM "
                        sSQL = sSQL & "pmuser_source WHERE user_id = " & CStr(m_iUserID) & " )" & Strings.ChrW(13) & Strings.ChrW(10)
                    End If

                    If v_sTableName.Trim().ToUpper() = "SOURCE" And m_bMultiCompany Then
                        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10) & "AND source_id = " & CStr(m_iSourceID)
                    End If

                    If (v_sTableName.Trim().ToUpper() = "PMPRODUCT_LOOKUP") And bIsGenMainFound Then
                        sSQL = sSQL & " WHERE is_generic_maintenance = 1"
                    End If

                    sSQL = sSQL & " ORDER BY " & v_sDisplayFieldName

                Else

                    'RAG 2002-01-09 - Moved from below so that it only affects parties.
                    'MSB020102 - We don't want all the shortnames to be displayed in the combo box as it doesn't work

                    ' TB: Parallel maintenance.  Re-instated as it now handles combo limitations
                    sSQL = "SELECT "
                    sSQL = sSQL & v_sDisplayFieldName
                    If v_sIDFieldName > "" Then
                        sSQL = sSQL & "," & Strings.ChrW(13) & Strings.ChrW(10)
                        sSQL = sSQL & v_sIDFieldName
                    End If
                    sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & " FROM Party P," & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & " Party_Type PT" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & " WHERE P.party_type_id = PT.party_type_id " & Strings.ChrW(13) & Strings.ChrW(10)
                    ' 20/08/2002 TB : if party type contains %, do a like comparison
                    If sPartyType.IndexOf("%"c) >= 0 Then
                        sSQL = sSQL & " AND PT.code LIKE '" & sPartyType & "'"
                    Else
                        sSQL = sSQL & " AND PT.code = '" & sPartyType & "'"
                    End If
                    ' 20/08/2002 TB : END
                    'Start (Girija chokkalingam) - (Tech Spec - LOA009 - Hide Inactive Users.doc) - (5.2.1.4)
                    If Not m_bReportingOnDeletedData Then
                        sSQL = sSQL & " AND (P.is_deleted IS NULL"
                        sSQL = sSQL & " OR P.is_deleted = 0)"
                    End If
                    'End (Girija chokkalingam) - (Tech Spec - LOA009 - Hide Inactive Users.doc) - (5.2.1.4)
                    sSQL = sSQL & " ORDER BY " & v_sDisplayFieldName
                End If

                lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetParametersFromDB", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

                ' If NO records were found return PMFalse
                If Not Informations.IsArray(vResultArray) Then
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If
            End With

            'Return Array

            r_vDefaultValues = vResultArray

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetParametersFromDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetParametersFromDB", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: CheckAllowAllBranches
    '
    ' Description:
    '
    ' History: 08/10/2002 sj - Created.
    '
    ' ***************************************************************** '
    Public Function CheckAllowAllBranches(ByRef r_bAllowAll As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sSQL As String = ""
            Dim vResultArray(,) As Object = Nothing

            sSQL = "SELECT source_id FROM pmuser_source WHERE user_id = " & m_iUserID

            With m_oSiriusDatabase

                .Parameters.Clear()
                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="CheckAllowAllBranches", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.SQLSelectCheck Failed for " & sSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckAllowAllBranches")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            r_bAllowAll = Not Informations.IsArray(vResultArray)

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckAllowAllBranches Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckAllowAllBranches", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ClearParameters
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function ClearParameters() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iParamCount As Integer = m_oSIRReportParameters.Count To 1 Step -1
                m_oSIRReportParameters.Remove(iParamCount)
            Next iParamCount

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ClearParameters Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearParameters", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Sub New()
        MyBase.New()

        Try

            ' Class Initialise

            ' Added by Scalability Update Program - 30/07/2002
            m_oSIRReportParameters = New Collection()

        Catch excep As System.Exception

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

#Region "Public Methods"


    'DC270303 -ISS1911
    ' ***************************************************************** '
    ' Name: DeleteTempReportRecords
    '
    ' Description: Delete Any Temporary Records For Current Session
    '
    ' ***************************************************************** '
    Public Function DeleteTempReportRecords(ByVal lSessionID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="session_id", vValue:=CStr(lSessionID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACDeleteReportTempRecordsSQL, sSQLName:=ACDeleteReportTempRecordsName, bStoredProcedure:=ACDeleteReportTempRecordsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete Temp Report Records Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteTempReportRecords", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteTempReportExcludeRecords
    '
    ' Description: Delete Any Temporary Records For The Report
    '
    ' ***************************************************************** '
    Public Function DeleteTempReportExcludeRecords(ByVal sUniqueReportName As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="unique_report_name", vValue:=sUniqueReportName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACDeleteTempReportExcludeRecordsSQL, sSQLName:=ACDeleteTempReportExcludeRecordsName, bStoredProcedure:=ACDeleteTempReportExcludeRecordsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete Temp Report Exclude Records Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteTempReportExcludeRecords", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetTPVisibility
    '
    ' Description: Get if Third Party is visible.
    '
    ' ***************************************************************** '
    Public Function GetTPVisibility(ByVal sTPType As String, ByRef r_bTPVisible As Boolean) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Execute SQL Statement
            With m_oDatabase

                .Parameters.Clear()

                ' Add the parameter (INPUT)
                m_lReturn = .Parameters.Add(sName:="TP_type", vValue:=sTPType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTPVisibility")

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If

                m_lReturn = .SQLSelect(sSQL:=ACGetTPVisibleSQL, sSQLName:=ACGetTPVisibleName, bStoredProcedure:=ACGetTPVisibleStored, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If

                ' If NO records were found return PMNotFound
                If Not Informations.IsArray(vResultArray) Then
                    result = gPMConstants.PMEReturnCode.PMNotFound
                    Return result
                End If
            End With

            'Return value from array

            r_bTPVisible = CBool(vResultArray(0, 0))

            Return result
        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTPVisibility Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTPVisibility", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally
            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetAccountingPeriods
    '
    ' Description: Get a list of previous accounting periods.
    '
    ' ***************************************************************** '
    Public Function GetAccountingPeriods(ByVal v_iBranchID As Integer, ByVal v_iNumberOfPreviousPeriods As Integer, ByRef r_vAccPeriods(,) As Object) As Integer

        Dim result As Integer = 0

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sSQL As String = ""

            sSQL = "SELECT TOP " & v_iNumberOfPreviousPeriods & " period_end_date " &
                   "FROM period WHERE period_end_date < DATEADD(m,1,GETDATE()) AND company_id = " & CStr(v_iBranchID) &
                   " AND sub_branch_id = 1 ORDER BY period_end_date DESC"

            ' Execute SQL Statement
            With m_oSiriusDatabase

                .Parameters.Clear()
                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetAccountingPeriods", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vAccPeriods)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.SQLSelectCheck Failed for " & sSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountingPeriods")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountingPeriods Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountingPeriods", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            Return result
        End Try
    End Function

    '8.5 Copied function from ConverHTMLToPDF()
    Public Function ConvertHTMLToPDFForCIL(ByVal sInputFileName As String, ByRef r_sOutputFilename As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(gPMFunctions.ConvertHTMLToPDF(sInputFileName:=sInputFileName, r_sOutputFilename:=r_sOutputFilename), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            Return result
            Return result
        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ConvertHTMLToPDFForCIL Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertHTMLToPDFForCIL", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally
            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    Public Function FilterChildList(ByRef v_vDefaultValues As Object,
                                    ByVal v_sParentTableName As String,
                                    ByVal v_sChildTableName As String,
                                    ByVal v_sIDCol As String,
                                    ByVal v_sDescriptionCol As String,
                                    ByVal v_sFilterVal As String,
                                    Optional ByVal v_sCustomStoredProcedure As String = "") As Integer


        Const kMethodName As String = "FilterChildList"

        Dim lLoop As Long
        Dim ReportParameter As SIRReportParameter
        Dim iResult As Integer = gPMConstants.PMEReturnCode.PMTrue

        Try

            For lLoop = 1 To m_oSIRReportParameters.Count
                ReportParameter = m_oSIRReportParameters.Item(lLoop)

                If ReportParameter.TableName = v_sChildTableName Then

                    If ReportParameter.IsDBTable Or v_sParentTableName <> "" Then
                        m_lReturn = ReportParameter.FilterDefaultValues(m_oSiriusDatabase, v_sParentTableName, v_sIDCol, v_sDescriptionCol, v_sFilterVal, v_sCustomStoredProcedure)

                    ElseIf ReportParameter.SetReadOnly Then
                        m_lReturn = ReportParameter.HandleReadOnly(m_oSiriusDatabase, If(ReportParameter.ReadOnlyCriteria = v_sFilterVal, True, False), v_sIDCol, v_sDescriptionCol)
                    End If

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iResult = gPMConstants.PMEReturnCode.PMError
                        bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description)
                        Return iResult
                    End If

                    v_vDefaultValues = ReportParameter.DefaultValues
                    Exit For

                End If

            Next lLoop

        Catch excep As Exception
            iResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err.Number, vErrDesc:=excep.Message, excep:=excep)
        End Try

        Return iResult

    End Function
#End Region


#Region "Private Methods"
    ''' <summary>
    ''' Get the User Name and Password to connect to the DataBase
    ''' Password is Stored in Encrypted Form in the Registry.
    ''' </summary>
    ''' <param name="o_sUserName"></param>
    ''' <param name="o_sPassword"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetUserAndPassword(ByRef o_sUserName As String, ByRef o_sPassword As String) As Integer

        Dim nReturn As Integer = PMEReturnCode.PMFalse
        Try
            Dim sLoginId As String = ""
            nReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine,
                       v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture,
                       v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon,
                       v_sSettingName:=PMSQLLoginId,
                       r_sSettingValue:=sLoginId)

            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New ApplicationException("Failed to Retrive values from Regisrty")
            End If
            Dim sPasswordSecure As String = ""
            nReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine,
                       v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture,
                       v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon,
                       v_sSettingName:=PMSQLLoginPassword,
                       r_sSettingValue:=sPasswordSecure)

            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New ApplicationException("Failed to Retrive values from Regisrty")
            End If

            Dim aKeys As Byte()
            aKeys = Encoding.ASCII.GetBytes(PMEncryptionEntropy)

            o_sUserName = Decrypt(sLoginId, aKeys)
            o_sPassword = Decrypt(sPasswordSecure, aKeys)

            Return nReturn
        Catch ex As Exception
            nReturn = PMEReturnCode.PMFalse
            Throw New ApplicationException("GetUserAndPassword Failed", ex)
        End Try
        Return nReturn
    End Function

    ''' <summary>
    ''' Decrypt the Encrypted LoginIdPassword based on the Cipher
    ''' </summary>
    ''' <param name="sCipher"></param>
    ''' <param name="aKeys"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function Decrypt(sCipher As String, aKeys As Byte()) As String
        If sCipher = "" Then
            Return ""
        End If
        If sCipher Is Nothing Then
            Throw New ArgumentNullException("cipher")
        End If

        'parse base64 string
        Dim aData As Byte() = Convert.FromBase64String(sCipher)

        ' Const kScope As DataProtectionScope = DataProtectionScope.LocalMachine
        'decrypt data
        Dim aDecrypted As Byte() ' = New Byte() 'Security.Cryptography.ProtectedData.Unprotect(aData, aKeys, kScope)
        Return Encoding.Unicode.GetString(aDecrypted)
    End Function
#End Region
    ' ***************************************************************** '
    'PM029917
    Public Function GetClaimData(ByVal dStart_date As Date, ByVal dEnd_date As Date, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object
        Dim lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            With m_oDatabase

                .Parameters.Clear()

                ' Add the parameter (INPUT)
                m_lReturn = .Parameters.Add(sName:="start_date", vValue:=dStart_date, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = m_oDatabase.Parameters.Add(sName:="end_date", vValue:=dEnd_date, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                lReturn = .SQLSelect(sSQL:=ACGetClaimDataSQL, sSQLName:=ACGetClaimDataName, bStoredProcedure:=True, vResultArray:=vResultArray)
                ' If NO records were found return PMFalse
                If Not Informations.IsArray(vResultArray) Then
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If
            End With

            'Return Array
            r_vResultArray = vResultArray

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInsurerIDFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsurerID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Function GetReportData(connectionString As String, reportDataSet As bSIRReportPrint.ReportDataSet, reportParameters As Object(,), ByRef localReportParameters As List(Of Microsoft.Reporting.WinForms.ReportParameter), ByVal addBranchInLocalParameter As Boolean) As Data.DataSet
        Dim dsReport As Data.DataSet = New Data.DataSet()
        Dim storedProcedureName As String = reportDataSet.SqlCommandText
        Dim sqlCommandType As String = reportDataSet.SqlCommandType
        Dim queryParameters = New Dictionary(Of String, Object)()
        localReportParameters = New List(Of Microsoft.Reporting.WinForms.ReportParameter)()
        Dim i As Integer = 0
        For i = 0 To reportParameters.GetLength(0) - 1
            If (Not Equals(reportParameters(i, 0).ToString().ToLower(), "branch") Or addBranchInLocalParameter) Then
                Dim value As String = String.Empty
                If reportParameters(CInt(i), CInt(1)) Is Nothing Then
                    If (reportParameters(CInt(i), CInt(0)).ToString() = "TPACode") Then
                        value = "null"
                    Else
                        value = 0
                    End If
                Else
                    value = reportParameters(CInt(i), CInt(1)).ToString()
                End If
                localReportParameters.Add(New Microsoft.Reporting.WinForms.ReportParameter(reportParameters(CInt(i), CInt(0)).ToString(), value.ToString()))
            End If
                If (reportDataSet.ReportQueryParameters IsNot Nothing) Then

                For Each keyValue As KeyValuePair(Of String, Object) In reportDataSet.ReportQueryParameters
                    If keyValue.Key.ToString().StartsWith("@") Then
                        Dim key = keyValue.Key.Replace("@", "").Trim()
                        If Equals(key.ToLower(), reportParameters(i, 0).ToString().ToLower()) Then
                            queryParameters.Add(reportParameters(i, 0).ToString(), reportParameters(i, 1))
                            Exit For
                        End If
                    End If
                Next
            End If
        Next

        Using connection As SqlConnection = New SqlConnection(connectionString)
            Using command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand(storedProcedureName, connection)
                command.CommandType = If(Equals(sqlCommandType, "StoredProcedure"), CommandType.StoredProcedure, CommandType.Text)
                If (reportDataSet.ReportQueryParameters IsNot Nothing) Then

                    For Each param As KeyValuePair(Of String, Object) In reportDataSet.ReportQueryParameters
                        Dim name = param.Key.Replace("@", "").Trim()
                        Dim value As Object = Nothing

                        If queryParameters.TryGetValue(name, value) Then
                            value = CObj(Convert.ToString(value).Replace("<", "").Replace(">", ""))
                            'reportDataSet1.ReportQueryParameters.Add(name, value);
                            Dim cmdParam As New SqlParameter()
                            cmdParam.ParameterName = name
                            cmdParam.Value = value
                            command.Parameters.Add(cmdParam)
                        End If
                    Next
                End If
                Dim reportAdapter As Data.SqlClient.SqlDataAdapter = New Data.SqlClient.SqlDataAdapter(command)
                reportAdapter.Fill(dsReport, "ReportData")
            End Using
        End Using

        Return dsReport


    End Function

    'Public Function GetReportData(connectionString As String, reportDataSet As ReportDataSet, reportParameters As Object(,), ByRef localReportParameters As List(Of Microsoft.Reporting.WinForms.ReportParameter)) As Data.DataSet
    '    Dim dsReport As Data.DataSet = New Data.DataSet()
    '    Dim storedProcedureName As String = reportDataSet.SqlCommandText
    '    Dim sqlCommandType As String = reportDataSet.SqlCommandType
    '    Dim queryParameters = New Dictionary(Of String, Object)()
    '    localReportParameters = New List(Of Microsoft.Reporting.WinForms.ReportParameter)()

    '    For i = 0 To reportParameters.GetLength(0) - 1

    '        If Not Equals(reportParameters(i, 0).ToString().ToLower(), "branch") Then
    '            localReportParameters.Add(New Microsoft.Reporting.WinForms.ReportParameter(reportParameters(CInt(i), CInt(0)).ToString(), reportParameters(CInt(i), CInt(1)).ToString()))
    '        End If
    '        If (reportDataSet.ReportQueryParameters IsNot Nothing) Then
    '            For Each keyValue As KeyValuePair(Of String, Object) In reportDataSet.ReportQueryParameters
    '                If keyValue.Key.ToString().StartsWith("@") Then
    '                    Dim key = keyValue.Key.Replace("@", "").Trim()
    '                    If Equals(key.ToLower(), reportParameters(i, 0).ToString().ToLower()) Then
    '                        queryParameters.Add(reportParameters(i, 0).ToString(), reportParameters(i, 1))
    '                        Exit For
    '                    End If
    '                End If
    '            Next
    '        End If
    '    Next
    '    Using connection As SqlConnection = New SqlConnection(connectionString)
    '        Using command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand(storedProcedureName, connection)
    '            command.CommandType = If(Equals(sqlCommandType, "StoredProcedure"), CommandType.StoredProcedure, CommandType.Text)
    '            If (reportDataSet.ReportQueryParameters IsNot Nothing) Then
    '                For Each param As KeyValuePair(Of String, Object) In reportDataSet.ReportQueryParameters
    '                    Dim name = param.Key.Replace("@", "").Trim()
    '                    Dim value As Object
    '                    If queryParameters.TryGetValue(name, value) Then
    '                        'reportDataSet1.ReportQueryParameters.Add(name, value);
    '                        command.Parameters.Add(New Data.SqlClient.SqlParameter(name, value))
    '                    End If
    '                Next
    '            End If
    '            Dim reportAdapter As Data.SqlClient.SqlDataAdapter = New Data.SqlClient.SqlDataAdapter(command)
    '            reportAdapter.Fill(dsReport, "ReportData")
    '        End Using
    '    End Using

    '    Return dsReport

    'End Function

    Public Sub SubreportProcessingEventHandler(ByVal sender As Object,
                                               ByVal e As SubreportProcessingEventArgs)

        Dim query As New bSIRReportPrint.SubReportQuery
        query = GetSubReportDetails(m_sReportPath + ReportFolderName, e.ReportPath)

        Dim storedProcedureName As String = query.CommandText.ToString()
        Dim sqlCommandType As String = query.CommandType.ToString()
        Dim dataSourceName As String = query.DataSourceName.ToString()
        Dim dsSubReport As New System.Data.DataSet
        Using connection As SqlConnection = New SqlConnection(ConnectionString)
            Using command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand(storedProcedureName, connection)
                command.CommandType = If(Equals(sqlCommandType, "StoredProcedure"), CommandType.StoredProcedure, CommandType.Text)
                'connection.Close()
                If e.Parameters IsNot Nothing AndAlso e.Parameters.Count > 0 Then
                    For Each param As ReportParameterInfo In e.Parameters
                        If param.Name.ToLower().StartsWith("pm_sp") = False Then '  param.Name.ToLower().Contains(storedProcedureName.ToLower()) = False  Then
                            command.Parameters.Add(New SqlParameter(param.Name, param.Values(0)))
                        End If
                    Next
                End If
                Dim reportAdapter As Data.SqlClient.SqlDataAdapter = New Data.SqlClient.SqlDataAdapter(command)
                reportAdapter.Fill(dsSubReport, "ReportData")
            End Using
        End Using
        e.DataSources.Add(New ReportDataSource("DataSet1", dsSubReport.Tables(0)))

    End Sub

    ' ***************************************************************** '
    ' Name: GetPMUserSource
    '
    ' Description: Get information if user has some restricted branches
    '
    ' ***************************************************************** '
    Public Function GetPMUserSource(ByRef r_bIsAllBranches As Boolean, ByVal iUserId As Integer) As Long

        Dim nResult As Integer = 0

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the InsuranceFileID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="nUser_id", vValue:=iUserId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="bIsAllBranches", vValue:=r_bIsAllBranches, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPMUserSourceSQL, sSQLName:=ACGetPMUserSourceName, bStoredProcedure:=ACGetPMUserSourceStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_bIsAllBranches = m_oDatabase.Parameters.Item("bIsAllBranches").Value
            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPMUserSource Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPMUserSource", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult
        End Try

    End Function
End Class
