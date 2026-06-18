Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Windows.Forms
'Developer Guide No.129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"
    'Developer Guide No 7
    Private Const vbFormCode As Integer = 0
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sStepStatus As String = ""

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMURenInvitePrint.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    Private m_oFindDocTemplate As Object


    Private m_oDocTemplate As iPMBDocTemplate.Interface_Renamed


    Private m_oReport As bSIRReportPrint.Business

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the return value for the a function call.
    Private m_lReturn As Integer

    ' {* USER DEFINED CODE (Begin) *}
    'Developer Guide No 17
    Private m_vProductID As Object
    Private m_vAgentID As Object
    Private m_dtSelectionDate As Date
    Private m_lSortOrder As Integer

    'Report
    Private m_iPrintMode As Integer
    Private m_sCompiledReportPath As String = ""
    'JMK 10/08/2001
    Private m_sReportOutputLocation As String = ""
    Dim m_sPrintRenewalInvite As String = ""

    Private m_lSourceID As Integer

    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)

    Public Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
        Set(ByVal Value As Integer)

            ' Set the interface exit status.
            m_lStatus = Value

        End Set
    End Property

    Public Property Task() As Integer
        Get

            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            m_iTask = Value

        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            m_lNavigate = Value

        End Set
    End Property

    Public Property StepStatus() As String
        Get

            Return m_sStepStatus

        End Get
        Set(ByVal Value As String)

            m_sStepStatus = Value

        End Set
    End Property
    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            m_lProcessMode = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            m_sTransactionType = Value

        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            m_dtEffectiveDate = Value

        End Set
    End Property
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)
    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign all of the controls to
            ' PMFormControl
            '
            ' Example:-
            '
            '        ' Pass control and required settings to FormControl
            '        m_lReturn = m_oFormFields.AddNewFormField( _
            ''                       ctlControl:=<Control Name>, _
            ''                       lFieldType:=<PM field type>, _
            ''                       lFormat:=<PM format string>, _
            ''                       lMandatory:=<PMMandatory or PMNonMandatory)
            '
            '        'Error checking
            '        If m_lReturn <> PMTrue Then
            '          SetFieldValidation = PMFalse
            '          Exit Function
            '        End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oFormFields.AddNewFormField(ctlControl:=txtSelectionDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If m_oFormFields.AddNewFormField(ctlControl:=cboProductCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_oFormFields.AddNewFormField(ctlControl:=cboAgentCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_oFormFields.AddNewFormField(ctlControl:=cboSortOrder, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BusinessToInterface
    '
    ' Description: Updates all interface details from the business
    '              object.
    '
    ' ***************************************************************** '
    Public Function BusinessToInterface() As Integer


        Dim result As Integer = 0
        Return gPMConstants.PMEReturnCode.PMTrue



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToBusiness
    '
    ' Description: Updates all business members from the interface
    '              details.
    '
    ' ***************************************************************** '
    Public Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMFalse

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        ' Assign the details from the interface to the data storage.
        stbMain.Items.Item("MESSAGE").Text = "Processing Renewal Invite"

        If InterfaceToData() = gPMConstants.PMEReturnCode.PMTrue Then

            result = RenewalInvitePrint(v_dtSelectionDate:=m_dtSelectionDate, v_vProductID:=m_vProductID, v_vAgentID:=m_vAgentID, v_lSortOrder:=m_lSortOrder)
        End If

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Return result



        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' ***************************************************************** '
    ' Name: GetReportPath
    '
    ' Description: Gets the Report Templates location from the registry.
    '
    ' ***************************************************************** '
    Private Function GetReportPath() As Integer

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

            ' Location for Exported Reports
            sRegPath = ""
            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="PrntFileDir", r_sSettingValue:=sRegPath)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Report Destination directory from Registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReportPath", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                m_sReportOutputLocation = sRegPath
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetReportPathFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReportPath", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: PrintRenewalInviteReport
    '
    ' Description: Print Renewal Reports
    '
    ' ***************************************************************** '
    Private Function PrintRenewalInviteReport() As Integer

        Dim result As Integer = 0
        Dim sExportFile As String = ""
        Dim lDocTypeID As Integer
        Dim sReportOutput, sUserReportName As String
        Dim vParameters, vDefaultValues As Object

        Const ACReportAgentRenewalList As String = "AgentRenewalList"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            stbMain.Items.Item("MESSAGE").Text = "Preparing Renewal Agent List"

            If m_oReport Is Nothing Then
                Dim temp_m_oReport As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oReport, "bSIRReportPrint.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oReport = temp_m_oReport

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSIRReportPrint object", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintRenewalInvite", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'assign report name and get output path

            m_oReport.reportName = ACReportAgentRenewalList

            sReportOutput = m_oReport.ReportOutputLocation

            'get user report name - this is unique per user per session

            sUserReportName = m_oReport.UserReportName

            If sReportOutput.Length > 1 Then
                If Not sReportOutput.EndsWith("\") Then
                    sReportOutput = sReportOutput & "\"
                End If
            End If

            'delete old version of output file
            'If FileSystem.Dir(sReportOutput & sUserReportName & ".*", FileAttribute.Normal) <> "" Then
            '    File.Delete(sReportOutput & sUserReportName & ".*")
            'End If

            For Each fileName As String In Directory.GetFiles(sReportOutput, sUserReportName & ".*")
                File.Delete(fileName)
            Next
            stbMain.Items.Item("MESSAGE").Text = "Exporting Renewal Agent List"



            m_lReturn = m_oReport.GetParameters(r_vParameters:=vParameters, r_vDefaultValues:=vDefaultValues)

            'Only one parameter, so just add the user_id into it.

            vParameters(0, 1) = g_iUserID


            'export to word format

            m_lReturn = m_oReport.ExportToDisk(r_ExportFile:=sExportFile, v_iFormatType:=0, v_vParameters:=vParameters)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed To Export Agent List To Word Format", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintRenewalInviteReport", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            stbMain.Items.Item("MESSAGE").Text = "Get Document Type ID"


            m_lReturn = m_oBusiness.GetDocTypeID(v_sDocCode:="LETTER", r_lDocTypeID:=lDocTypeID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed To Get Document Type ID For Code (REPORT)", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintRenewalInviteReport", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'spool report

            Application.DoEvents()

            If m_oDocTemplate Is Nothing Then
                Dim temp_m_oDocTemplate As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oDocTemplate, sClassName:="iPMBDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oDocTemplate = temp_m_oDocTemplate

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create iPMBDocTemplate object", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintRenewalInvite", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            ' JMK 10/08/2001
            ' need to get Exported doc path from Client perspective
            ' (otherwise you get server drive letter path)
            ' ...and add userID
            GetReportPath()
            sExportFile = m_sReportOutputLocation & sUserReportName & ".doc"


            m_oDocTemplate.DocName = sExportFile

            m_oDocTemplate.SpoolDesc = "Renewal Invite Agent List"

            m_oDocTemplate.DocumentTypeId = lDocTypeID

            m_oDocTemplate.Mode = gSIRLibrary.ACSpoolReportMode

            stbMain.Items.Item("MESSAGE").Text = "Spooling Renewal Agent List - " & sExportFile


            m_lReturn = m_oDocTemplate.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed To Print Renewal Invite Agent List", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintRenewalInviteReport", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PreviewReport
    '
    ' Description: Preview printed report
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (PreviewReport) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function PreviewReport(ByVal v_sReportFileName As String, ByVal v_sWindowTitle As String, ByRef r_cryControl As AxCrystal.AxCrystalReport) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    'With r_cryControl
    '
    '.ReportFileName = v_sReportFileName
    'Set window to maximised
    '.WindowState = Crystal.WindowStateConstants.crptMaximized
    'Set Title for window if in preview mode
    '.WindowTitle = v_sWindowTitle
    'Tell the control where the report is going to
    '.Destination = Crystal.DestinationConstants.crptToWindow
    'result = .PrintReport()
    '
    '
    'End With
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PreviewReport Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PreviewReport", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    '********************************************************************
    ' ***************************************************************** '
    ' Name: RenewalInvitePrint
    '
    ' Desc: do renewal print for policies that need renewal
    '
    ' ***************************************************************** '
    Private Function RenewalInvitePrint(ByVal v_dtSelectionDate As Date, ByVal v_vProductID As Object, ByVal v_vAgentID As Object, ByVal v_lSortOrder As Integer) As Integer

        'Field position
        Dim result As Integer = 0
        Const ACFieldPosPartyCnt As Integer = 0
        Const ACFieldPosInsuranceFolderCnt As Integer = 1
        Const ACFieldPosInsuranceFileCnt As Integer = 2
        Const ACFieldPosRenewalStatusCnt As Integer = 3
        Const ACFieldPosProductIsTrueMonthlyPolicy As Integer = 5
        Const ACFieldPosInsuranceFileIsAnniversaryCopy As Integer = 6


        Dim vResultArray(,) As Object
        Dim lCount As Integer
        Dim sExceptionNote As String = ""

        Dim bProductIsTrueMonthlyPolicy, bIsAnniversaryCopy, bPrintInvite As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get all policies which need renewal
            stbMain.Items.Item("MESSAGE").Text = "Getting Renewal Invite List"


            If m_oBusiness.GetRenewalInviteList(v_dtSelectionDate:=v_dtSelectionDate, v_vProductID:=v_vProductID, v_vAgentID:=v_vAgentID, v_lSortOrder:=v_lSortOrder, r_vResultArray:=vResultArray, v_lSourceID:=m_lSourceID) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'do we have any data
            If Not Information.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            stbMain.Items.Item("MESSAGE").Text = "Preparing Print Table"

            'delete existing data on last_print_run table

            If m_oBusiness.DelLastPrintRun() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = g_oRenewal.GenerateAgentRenewalEmail(v_sType:="invitation")

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                result = gPMConstants.PMEReturnCode.PMError
                'Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenewalInvitePrint Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenewalInvitePrint", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            'loop thro and process each renewal invite

            For lCount = 0 To vResultArray.GetUpperBound(1)

                'PN16460 -- incase someone Cancels the process, stop further processing
                If m_lStatus = gPMConstants.PMEReturnCode.PMCancel Then
                    Return result
                End If

                ' get tmp related fields

                bProductIsTrueMonthlyPolicy = gPMFunctions.ToSafeBoolean(CDbl(vResultArray(ACFieldPosProductIsTrueMonthlyPolicy, lCount)) = 1)

                bIsAnniversaryCopy = gPMFunctions.ToSafeBoolean(CDbl(vResultArray(ACFieldPosInsuranceFileIsAnniversaryCopy, lCount)) = 1)
                bPrintInvite = True

                If bProductIsTrueMonthlyPolicy And Not bIsAnniversaryCopy Then
                    bPrintInvite = False
                    m_lReturn = gPMConstants.PMEReturnCode.PMTrue
                End If

                If bPrintInvite Then




                    m_lReturn = PrintRenewalInvite(v_lPartyCnt:=CInt(vResultArray(ACFieldPosPartyCnt, lCount)), v_lInsuranceFolderCnt:=CInt(vResultArray(ACFieldPosInsuranceFolderCnt, lCount)), v_lInsuranceFileCnt:=CInt(vResultArray(ACFieldPosInsuranceFileCnt, lCount)))

                End If

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                    stbMain.Items.Item("MESSAGE").Text = "Updating Renewal Status"
                    'update renewal status and is_invite_printed = PMTRUE

                    If m_oBusiness.UpdateRenewalStatus(v_lInsuranceFileCnt:=vResultArray(ACFieldPosInsuranceFileCnt, lCount)) = gPMConstants.PMEReturnCode.PMTrue Then

                        stbMain.Items.Item("MESSAGE").Text = "Adding To Print Table"
                        'add to last_print_run table

                        m_lReturn = m_oBusiness.AddLastPrintRun(v_lRenewalStatusCnt:=vResultArray(ACFieldPosRenewalStatusCnt, lCount))
                    End If
                    'RKS PN13490
                Else
                    sExceptionNote = ""
                    stbMain.Items.Item("MESSAGE").Text = "Adding Exceptions"

                    If m_oBusiness.UpdateRenewalStatus(v_lInsuranceFileCnt:=vResultArray(ACFieldPosInsuranceFileCnt, lCount), v_lRenewalExceptionReasonID:=4, v_sRenewalExceptionNote:=sExceptionNote) = gPMConstants.PMEReturnCode.PMTrue Then
                        stbMain.Items.Item("MESSAGE").Text = "Adding To Print Table"
                    End If
                End If

            Next


            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                result = gPMConstants.PMEReturnCode.PMError
                'Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenewalInvitePrint Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenewalInvitePrint", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If


            Return result

        Catch excep As System.Exception



            sExceptionNote = excep.Message

            stbMain.Items.Item("MESSAGE").Text = "Adding Exceptions"


            If m_oBusiness.UpdateRenewalStatus(v_lInsuranceFileCnt:=vResultArray(ACFieldPosInsuranceFileCnt, lCount), v_lRenewalExceptionReasonID:=5, v_sRenewalExceptionNote:=sExceptionNote) = gPMConstants.PMEReturnCode.PMTrue Then
                stbMain.Items.Item("MESSAGE").Text = "Adding To Print Table"
            End If

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RePrintRenewalInvite (Private)
    '
    ' Description: reprint renewal invite from last_print_run table
    '
    ' ***************************************************************** '
    Public Function RePrintRenewalInvite() As Integer

        Dim result As Integer = 0
        Try

            'Field position
            Const ACFieldPosPartyCnt As Integer = 0
            Const ACFieldPosInsuranceFolderCnt As Integer = 1
            Const ACFieldPosInsuranceFileCnt As Integer = 2
            ' Const ACFieldPosRenewalStatusCnt As Integer = 3

            Dim vResultArray(,) As Object

            result = gPMConstants.PMEReturnCode.PMTrue

            stbMain.Items.Item("MESSAGE").Text = "Getting Renewal Invite RePrint List"


            If m_oBusiness.GetRePrintList(r_vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'do we have any data
            If Not Information.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            'loop thro and reprint each policy

            For lCount As Integer = 0 To vResultArray.GetUpperBound(1)



                m_lReturn = PrintRenewalInvite(v_lPartyCnt:=CInt(vResultArray(ACFieldPosPartyCnt, lCount)), v_lInsuranceFolderCnt:=CInt(vResultArray(ACFieldPosInsuranceFolderCnt, lCount)), v_lInsuranceFileCnt:=CInt(vResultArray(ACFieldPosInsuranceFileCnt, lCount)))

            Next

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RePrintRenewalInvite Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RePrintRenewalInvite", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PrintRenewalInvite (Private)
    '
    ' Description: Print out invite letter
    '
    ' ***************************************************************** '
    Private Function PrintRenewalInvite(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0

        Dim oGetDocument As iPMUGetDocument.Interface_Renamed
        Dim vKeyArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = g_oRenewal.GenerateCustomerRenewalEmail(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_sType:="invitation")


            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then

                stbMain.Items.Item("MESSAGE").Text = "Getting Document ID"

                ReDim vKeyArray(1, 5)

                'Generate document.
                oGetDocument = New iPMUGetDocument.Interface_Renamed()

                If oGetDocument Is Nothing Then

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create iPMUGetDocument object", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Developer Guide No 9
                oGetDocument.Initialise()

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameInsFileCnt
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = v_lInsuranceFileCnt

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameDocumentID
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = 6 'RNC (Renewal Notice)

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameInsFolderCnt
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = v_lInsuranceFolderCnt

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNamePartyCnt
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = v_lPartyCnt

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeynameFormlessInterface
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = True

                m_lReturn = oGetDocument.SetKeys(vKeyArray:=vKeyArray)

                oGetDocument.FunctionalArea = 1
                oGetDocument.TransactionType = "RNI" ' Renewal Invite

                m_lReturn = oGetDocument.Start()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                oGetDocument.Dispose()
                oGetDocument = Nothing

            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PrintRenewalInvite Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintRenewalInvite", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDocID (Private)
    '
    ' Description: Get document template id and document type id
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetDocID) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetDocID(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lDocTemplateID As Integer, ByRef r_lDocTypeID As Integer) As Integer
    '
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '

    'm_lReturn = m_oBusiness.ValidateRenewalInvite(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_lDocumentTemplateId:=r_lDocTemplateID, r_lDocumentTypeId:=r_lDocTypeID)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch 
    '
    '
    '
    '
    'Return gPMConstants.PMEReturnCode.PMError
    'End Try
    '
    'End Function

    '********************************************************************

    ' ***************************************************************** '
    ' Name: InterfaceToData
    '
    ' Description: Updates the data storage from the interface details.
    '
    ' ***************************************************************** '
    Private Function InterfaceToData() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign all of the details from the
            ' interface to the data storage.
            '
            ' Example:-
            '
            '    m_DName$ = trim$(txtName.Text)
            '    m_DDate = CDate(txtDate.Text)
            '    m_iDCodeID% = cmbCode.ItemData(cmbCode.ListIndex)
            '    m_lReturn& = m_oFormFields.UnformatControl(txtName)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************


            m_dtSelectionDate = CDate(m_oFormFields.UnformatControl(ctlControl:=txtSelectionDate))

            'set to null if its all products
            If VB6.GetItemData(cboProductCode, cboProductCode.SelectedIndex) = 0 Then

                m_vProductID = Nothing
            Else
                m_vProductID = VB6.GetItemData(cboProductCode, cboProductCode.SelectedIndex)
            End If

            'set to null if its all agents
            If VB6.GetItemData(cboAgentCode, cboAgentCode.SelectedIndex) = 0 Then

                m_vAgentID = Nothing
            Else
                m_vAgentID = VB6.GetItemData(cboAgentCode, cboAgentCode.SelectedIndex)
            End If

            'sort order
            m_lSortOrder = VB6.GetItemData(cboSortOrder, cboSortOrder.SelectedIndex)

            'set to print mode
            m_iPrintMode = MainModule.AC_PRINT_ONLY

            ' Alix - 06/01/2003 - PN9261
            m_lSourceID = VB6.GetItemData(cboBranchCode, cboBranchCode.SelectedIndex)
            ' /Alix

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Dim vAgentArray(,) As Object

        Const ACAgentPosPartyCnt As Integer = 0
        Const ACAgentPosTradingName As Integer = 1

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            'default to first tab
            SSTabHelper.SetSelectedIndex(Me.tabMainTab, 0)

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            If m_oFormFields.FormatControl(ctlControl:=txtSelectionDate, vControlValue:=DateTime.Today) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Product Code
            If GetComboDetails(v_sTableName:="Product", v_sKeyIDFieldName:="product_id", v_sDescFieldName:="description", r_cboControl:=cboProductCode) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'Agent Code
            'RWH(10/05/2001) Get just Agents rather than all parties.

            m_lReturn = m_oBusiness.GetAgents(vAgentArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Populate combo.
            'add in non applicable value with ID of 0
            Dim cboAgentCode_NewIndex As Integer = -1
            'Developer Guide No 153
            cboAgentCode_NewIndex = cboAgentCode.Items.Add(New VB6.ListBoxItem("All", 0))

            If Information.IsArray(vAgentArray) Then

                For iAgentCount As Integer = 0 To vAgentArray.GetUpperBound(1)

                    'Developer Guide No 153
                    cboAgentCode_NewIndex = cboAgentCode.Items.Add(New VB6.ListBoxItem(CStr(vAgentArray(ACAgentPosTradingName, iAgentCount)), CInt(vAgentArray(ACAgentPosPartyCnt, iAgentCount))))
                Next iAgentCount
            End If

            cboAgentCode.SelectedIndex = 0


            'Sort Order
            '
            '********ITEMDATA DEFINE THE SORT ORDER AS DEFINED IN THE BUSINESS OBJECT**************
            '
            cboSortOrder.Items.Clear()

            Dim cboSortOrder_NewIndex As Integer = -1
            'Developer Guid eno 153
            cboSortOrder_NewIndex = cboSortOrder.Items.Add(New VB6.ListBoxItem("Product Code", 0))
            cboSortOrder_NewIndex = cboSortOrder.Items.Add(New VB6.ListBoxItem("Renewal Date", 1))
            cboSortOrder_NewIndex = cboSortOrder.Items.Add(New VB6.ListBoxItem("Policy Number", 2))
            cboSortOrder_NewIndex = cboSortOrder.Items.Add(New VB6.ListBoxItem("Client Code", 3))
            cboSortOrder_NewIndex = cboSortOrder.Items.Add(New VB6.ListBoxItem("Agent Code", 4))
            cboSortOrder.SelectedIndex = 0

            ' Alix - 06/01/2003 - PN9261
            ' /Alix

            ' {* USER DEFINED CODE (End) *}

            Return GetComboDetailsBranch(Me.cboBranchCode)

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetFirstLastControls
    '
    ' Description: Sets the first and last data entry controls for
    '              each tab to the control array, for use with the
    '              keyboard navigation.
    '
    ' ***************************************************************** '
    Private Function SetFirstLastControls() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).
            ReDim m_ctlTabFirstLast(1, 0)

            ' Set the first and last data entry controls for
            ' all of the tabs.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to set the first and last data entry
            ' controls for all of the tabs.
            '
            ' Example:-
            '
            '    Set m_ctlTabFirstLast(ACControlStart, 0) = txtName
            '    Set m_ctlTabFirstLast(ACControlEnd, 0) = txtAge
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            m_ctlTabFirstLast(ACControlStart, 0) = txtSelectionDate
            m_ctlTabFirstLast(ACControlEnd, 0) = cboSortOrder

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.


            'Developer Guide No 243
            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                    "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If

            'Developer Guide No 243

            'cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'cmdRePrint.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRePrintButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            '' {* USER DEFINED CODE (Begin) *}

            '' ************************************************************
            '' Enter your code here to display all language specific
            '' captions.
            '' The GetResData function will allow you to do this.
            ''
            '' Example:-
            ''
            ''    lblDesc.Caption = iPMFunc.GetResData( _
            ' ''        iLangID:=g_iLanguageID%, _
            ' ''        lID:=ACDesc, _
            ' ''        iDataType:=PMResString)
            ''
            '' NOTE: Replace this section with your new code.
            '' ************************************************************


            'lblSelectionDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSelectionDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'lblProductCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACProductCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'lblAgentCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAgentCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'lblSortOrder.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSortOrder, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdRePrint.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRePrintButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.
            ' The GetResData function will allow you to do this.
            '
            ' Example:-
            '
            '    lblDesc.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACDesc, _
            ''        iDataType:=PMResString)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************


            lblSelectionDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSelectionDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblProductCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACProductCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAgentCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAgentCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblSortOrder.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSortOrder, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'Ends
            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ValidateForm
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function ValidateForm() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateForm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetComboDetails
    '
    ' Description: get details from numbering scheme and add to combobox
    '
    '
    ' ***************************************************************** '
    Private Function GetComboDetails(ByVal v_sTableName As String, ByVal v_sKeyIDFieldName As String, ByVal v_sDescFieldName As String, ByRef r_cboControl As ComboBox, Optional ByVal v_sSecondaryDescFieldName As String = "") As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        result = gPMConstants.PMEReturnCode.PMFalse

        Try

            'make sure combobox is empty
            r_cboControl.Items.Clear()

            'add in non applicable value with ID of 0
            Dim r_cboControl_NewIndex As Integer = -1
            'Developer Guid No 153
            r_cboControl_NewIndex = r_cboControl.Items.Add(New VB6.ListBoxItem("All", 0))



            If m_oBusiness.GetLookUp(v_sTableName:=v_sTableName, v_sKeyIDFieldName:=v_sKeyIDFieldName, v_sDescFieldName:=v_sDescFieldName, r_vResultArray:=vResultArray, v_sSecondaryDescFieldName:=v_sSecondaryDescFieldName) = gPMConstants.PMEReturnCode.PMTrue Then

                If Information.IsArray(vResultArray) Then

                    For icount As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                        'Developer Guide No 153
                        r_cboControl_NewIndex = r_cboControl.Items.Add(New VB6.ListBoxItem(CStr(vResultArray(1, icount)), CInt(vResultArray(0, icount))))
                    Next
                End If

                result = gPMConstants.PMEReturnCode.PMTrue

            End If

            'default to all products
            r_cboControl.SelectedIndex = 0

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetComboDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetComboDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'Refresh Agents List according to Branch Selected
    Private Sub cboBranchCode_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboBranchCode.SelectedIndexChanged

        Dim vAgentArray(,) As Object



        m_lReturn = m_oBusiness.GetAgents(vAgentArray, VB6.GetItemData(cboBranchCode, cboBranchCode.SelectedIndex))

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Unable to retrieve the Branch Agents for " & VB6.GetItemString(cboBranchCode, cboBranchCode.SelectedIndex).Trim(), "Renewal Invite", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            cboAgentCode.SelectedIndex = 0
            Exit Sub
        End If

        'Populate combo.
        'add in non applicable value with ID of 0
        cboAgentCode.Items.Clear()
        Dim cboAgentCode_NewIndex As Integer = -1
        'Developer Guide No 153
        cboAgentCode_NewIndex = cboAgentCode.Items.Add(New VB6.ListBoxItem("All", 0))

        If Information.IsArray(vAgentArray) Then

            For iAgentCount As Integer = 0 To vAgentArray.GetUpperBound(1)

                'Developer Guide No 153
                cboAgentCode_NewIndex = cboAgentCode.Items.Add(New VB6.ListBoxItem(CStr(vAgentArray(1, iAgentCount)), CInt(vAgentArray(0, iAgentCount))))
            Next iAgentCount
        End If

        cboAgentCode.SelectedIndex = 0

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        ' Fire up the help screen
        'Developer Guide No 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, lContextID:=ScreenHelpID)

    End Sub

    Private Sub cmdRePrint_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRePrint.Click
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        stbMain.Items.Item("MESSAGE").Text = "Processing Renewal Invite"

        m_lReturn = RePrintRenewalInvite()

        Select Case m_lReturn
            Case gPMConstants.PMEReturnCode.PMTrue
                If m_sPrintRenewalInvite = "1" Then
                    If PrintRenewalInviteReport() = gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Renewal Invite Print Completed", "Renewal Invite", MessageBoxButtons.OK)
                    Else
                        MessageBox.Show("Renewal Invite Print Completed, But Failed To Print Agent List", "Renewal Invite", MessageBoxButtons.OK)
                    End If
                Else
                    MessageBox.Show("Renewal Invite Print Completed", "Renewal Invite", MessageBoxButtons.OK)
                End If
            Case gPMConstants.PMEReturnCode.PMNotFound
                MessageBox.Show("Nothing To Reprint", "Renewal Invite", MessageBoxButtons.OK)
            Case Else
                MessageBox.Show("Renewal Invite Print Failed", "Renewal Invite", MessageBoxButtons.OK)
        End Select

        stbMain.Items.Item("MESSAGE").Text = "Ready"

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

    End Sub

    ' PRIVATE Methods (End)

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.
        Try

            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRRenInvitePrint.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                'Developer Guide No 243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                'Developer Guide No 243
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If


            ' Create an instance of the general interface object.
            m_oGeneral = New iPMURenInvitePrint.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try

            iPMFunc.ShowFormInTaskBar_Detach()

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}

            ' {* USER DEFINED CODE (End) *}

            ' Validate fields using Forms Control
            m_lReturn = SetFieldValidation()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If
            If m_sPrintRenewalInvite = "" Then
                If iPMFunc.GetSystemOption(v_iOptionNumber:=1013, r_sOptionValue:=m_sPrintRenewalInvite) <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Fail to Get System Option", ACApp, MessageBoxButtons.OK)
                End If
            End If
            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If
            txtSelectionDate.Focus()
            txtSelectionDate.Select()
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    'Developer Guide No 184
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()
            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Terminate the business object


            ' Terminate the form control object.
            m_oFormFields.Dispose()
            ' Destroy the instance of the form control object
            ' from memory.
            m_oFormFields = Nothing

            If Not (m_oFindDocTemplate Is Nothing) Then

                m_oFindDocTemplate.Dispose()

                m_oFindDocTemplate = Nothing
            End If

            If Not (m_oDocTemplate Is Nothing) Then

                m_oDocTemplate.Dispose()

                m_oDocTemplate = Nothing
            End If

            If Not (m_oReport Is Nothing) Then

                m_oReport.Dispose()

                m_oReport = Nothing
            End If

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub


    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
            tabMainTab.Focus()
        End If
    End Sub


    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check mandatory controls have been entered into.
            If m_oFormFields.CheckMandatoryControls() = gPMConstants.PMEReturnCode.PMTrue Then
                If ValidateForm() = gPMConstants.PMEReturnCode.PMTrue Then
                    ' Process the next set of actions depending
                    ' upon the interface task etc.
                    m_lReturn = m_oGeneral.ProcessCommand()

                    'turn off reprint - data in the last_print_run table may be empty
                    Me.cmdRePrint.Enabled = False

                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        If Not (Status = gPMConstants.PMEReturnCode.PMCancel) Then
                            If m_sPrintRenewalInvite = "1" Then
                                '***************PRINT OUT REPORTS*****************
                                m_lReturn = PrintRenewalInviteReport()

                                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                    MessageBox.Show("Renewal Invite Completed Successfully", "Renewal Invite", MessageBoxButtons.OK)
                                Else
                                    MessageBox.Show("Renewal Invite Completed Successfully, But Failed To Produce Agent List", "Renewal Invite", MessageBoxButtons.OK)
                                End If
                            Else
                                MessageBox.Show("Renewal Invite Completed Successfully", "Renewal Invite", MessageBoxButtons.OK)
                            End If
                            'give user option to reprint
                            Me.cmdRePrint.Enabled = True
                        End If

                    ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                        Me.cmdRePrint.Enabled = True
                        MessageBox.Show("No data found for current criteria", "Renewal Invite", MessageBoxButtons.OK)
                    Else
                        MessageBox.Show("Renewal Invite Failed", "Renewal Invite", MessageBoxButtons.OK)
                    End If
                End If
            End If

            stbMain.Items.Item("MESSAGE").Text = "Ready"

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            stbMain.Items.Item("MESSAGE").Text = "Ready"

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            If m_oGeneral.ProcessCommand() = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub txtSelectionDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSelectionDate.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtSelectionDate)
    End Sub
    Private Sub txtSelectionDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSelectionDate.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtSelectionDate)
    End Sub

    ' ***************************************************************** '
    '
    ' Name: GetComboDetailsBranch
    '
    ' Description: get details from numbering scheme and add to combobox
    ' 190302 Add branch as a selection parameter
    '
    ' ***************************************************************** '
    Private Function GetComboDetailsBranch(ByRef r_cboControl As ComboBox) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        result = gPMConstants.PMEReturnCode.PMFalse

        Try

            'make sure combobox is empty
            r_cboControl.Items.Clear()

            'add in non applicable value with ID of 0
            Dim r_cboControl_NewIndex As Integer = -1
            'Developer Guie No 153
            r_cboControl_NewIndex = r_cboControl.Items.Add(New VB6.ListBoxItem("All", 0))


            If m_oBusiness.GetLookUp(v_sTableName:="Source", v_sKeyIDFieldName:="source_id", v_sDescFieldName:="description", r_vResultArray:=vResultArray) = gPMConstants.PMEReturnCode.PMTrue Then

                If Information.IsArray(vResultArray) Then

                    For icount As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                        'Developer Guie No 153
                        r_cboControl_NewIndex = r_cboControl.Items.Add(New VB6.ListBoxItem(CStr(vResultArray(1, icount)), CInt(vResultArray(0, icount))))
                    Next
                End If

                result = gPMConstants.PMEReturnCode.PMTrue

            End If

            'default to all branches
            r_cboControl.SelectedIndex = 0

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetComboDetailsBranch Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetComboDetailsBranch", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
        MemoryHelper.ReleaseMemory()
    End Sub

End Class
