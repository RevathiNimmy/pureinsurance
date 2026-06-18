Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmExportCashBook
    Inherits System.Windows.Forms.Form
    Implements IDisposable
    Private m_bRet As Boolean
    Private m_lReturn As Integer

    Private m_lStatus As gPMConstants.PMEReturnCode

    Private m_sCallingAppName As String = ""

    Private m_iSourceID As Integer

    Private m_oBusiness As bACTExportCashListItems.Business

    Private m_sBranchID As String = ""
    Private m_sBranchCode As String = ""
    Private m_sBranchDesc As String = ""

    Private m_sFolder As String = ""
    Private m_sFilename As String = ""
    Private m_sMediaType As String = ""

    Private m_vMediaTypes(,) As Object

    Private Const MEDIA_CODE As Integer = 0
    Private Const MEDIA_DESC As Integer = 1

    Private Const EXPORT_FOLDER As String = "ExportFolder"
    Private Const EXPORT_FILENAME As String = "ExportFilename"
    Private Const EXPORT_MEDIATYPE As String = "ExportMediaType"
    Private Const EXPORT_BRANCHCODE As String = "ExportBranchCode"

    Private Const ACClass As String = "frmExportCashListItems"

    Public WriteOnly Property SourceId() As Integer
        Set(ByVal Value As Integer)
            m_iSourceID = Value
        End Set
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property

    Public WriteOnly Property Business() As bACTExportCashListItems.Business
        Set(ByVal Value As bACTExportCashListItems.Business)
            m_oBusiness = Value
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

    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
               
            End If
        End If
        Me.disposedValue = True
    End Sub


    Public Function Load_Renamed() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' get media types from db
            m_lReturn = GetMediaTypes()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' load up the drop-down list
            m_lReturn = PopulateMediaTypes()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_lReturn = LoadRegSettings()


            ' populate interface
            If Not m_sFolder.EndsWith("\") Then
                m_sFolder = m_sFolder & "\"
            End If

            If m_sFolder & m_sFilename <> "\" Then
                txtExportLocation.Text = m_sFolder & m_sFilename
            End If

            If m_sMediaType <> "" Then
                cboMediaType.Text = m_sMediaType
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    Private Sub cmdClear_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClear.Click

        txtExportLocation.Text = ""

    End Sub

    Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click

        If m_sCallingAppName = "SETUP" Then

            m_lStatus = gPMConstants.PMEReturnCode.PMSucceed

        Else

            m_lStatus = gPMConstants.PMEReturnCode.PMOK

        End If

        Me.Close()

    End Sub

    Private Sub cmdRunExport_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRunExport.Click

        If m_sCallingAppName = "SETUP" Then

            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            Me.Close()

        Else

            m_lReturn = RunExport()

            MessageBox.Show("Export completed", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)

        End If

    End Sub

    Private Sub cmdSaveSettings_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSaveSettings.Click

        m_lReturn = SaveRegSettings()

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        Me.Close()

    End Sub

    Private Sub cmdExportLocation_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdExportLocation.Click

        m_lReturn = ExportLocation()

    End Sub

    Private Sub frmExportCashBook_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            If m_sCallingAppName = CALLING_APP_WRAPPER Then
                ' via the wrapper, so auto-run the export
                Timer1.Enabled = True
            End If

        End If
    End Sub

    Private Sub Form_Initialize_Renamed()
        Try

            iPMFunc.ShowFormInTaskBar_Attach()

        Catch
        End Try



    End Sub


    Private Sub frmExportCashBook_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        Try

            iPMFunc.ShowFormInTaskBar_Detach()

        Catch
        End Try



    End Sub

    Private Sub frmExportCashBook_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        If m_lStatus = gPMConstants.PMEReturnCode.PMOK Or m_lStatus = gPMConstants.PMEReturnCode.PMSucceed Then
            Exit Sub
        End If

        If m_sCallingAppName <> CALLING_APP_WRAPPER Then
            ' via w/mgr, so pop-up confirm
            m_lReturn = MessageBox.Show("OK to exit?", "Cancel Export", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)

            If m_lReturn <> System.Windows.Forms.DialogResult.OK Then
                ' don't cancel
                Cancel = 1
            End If
        End If

        eventArgs.Cancel = Cancel <> 0
    End Sub

    Private Sub Timer1_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Timer1.Tick

        Timer1.Enabled = False

        m_lReturn = RunExport()

        Me.Close()

    End Sub

    Private Sub txtExportLocation_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtExportLocation.Enter

        m_lReturn = ExportLocation()

        cmdExportLocation.Focus()

    End Sub

    '################################################################################
    ' Method: ExportLocation
    ' Description: get the folder/file to export to
    '
    ' History: RDC 12112003 created
    '################################################################################
    Private Function ExportLocation() As Integer

        Dim result As Integer = 0
        Dim sFilename As String

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' default folder/file
            If txtExportLocation.Text = "" Then
                CommonDialog1Save.FileName = m_sBranchCode & "_" & StringsHelper.Format(DateTime.Now, "yymmdd_hhnnss") & ".csv"
            Else
                CommonDialog1Save.FileName = txtExportLocation.Text
            End If

            ' user can cancel and error is trapped


            'commented as CommonDialog1 is not declared
            'CommonDialog1.CancelError = True

            ' file type

            CommonDialog1Save.Filter = "Comma-separated values file (*.csv)|*.csv"

            m_bRet = True

            ' repeat until the user cancels, or a unique filename is selected
            Do While m_bRet

                CommonDialog1Save.ShowDialog()

                sFilename = CommonDialog1Save.FileName.Trim()

                m_lReturn = CheckFileExists(sFilename, m_bRet)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to check if file already exists", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                    Return result
                End If

                If m_bRet Then
                    MessageBox.Show("Not permitted: export file already exists", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

            Loop

            txtExportLocation.Text = sFilename.Trim()


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            If Information.Err().Number = ERROR_CANCEL_SELECTED Then
                ' user cancelled the export, so don't display error message

                Return gPMConstants.PMEReturnCode.PMError
            End If

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExportLocation failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExportLocation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '################################################################################
    ' Method: GetBranchName
    ' Description: get branch (source) code and description
    '
    ' History: RDC 12112003 created
    '################################################################################
    Private Function GetBranchName() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            m_lReturn = m_oBusiness.getbranchdetails(iSourceID:=m_iSourceID, sbranchcode:=m_sBranchCode, sBranchDesc:=m_sBranchDesc)


            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBranchName failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBranchName", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '################################################################################
    ' Method: GetMediaTypes
    ' Description: media types for drop-down list
    '
    ' History: RDC 12112003 created
    '################################################################################
    Private Function GetMediaTypes() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            m_lReturn = m_oBusiness.GetMediaTypes(vMediaTypes:=m_vMediaTypes)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMediaTypes failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMediaTypes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '################################################################################
    ' Method: PopulateMediaTypes
    ' Description: add media types to drop-down
    '
    ' History: RDC 12112003 created
    '################################################################################
    Private Function PopulateMediaTypes() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If Not Information.IsArray(m_vMediaTypes) Then
                Return result
            End If

            cboMediaType.Items.Clear()

            For iLoop As Integer = m_vMediaTypes.GetLowerBound(1) To m_vMediaTypes.GetUpperBound(1)
                cboMediaType.Items.Add(CStr(m_vMediaTypes(MEDIA_DESC, iLoop)))
            Next


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateMediaTypes failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateMediaTypes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '################################################################################
    ' Method: SaveRegSettings
    ' Description: write registry settings
    '
    ' History: RDC 12112003 created
    '################################################################################
    Private Function SaveRegSettings() As Integer

        Dim result As Integer = 0
        Dim iPos As Integer
        Dim sFullPath, sFolder, sFilename, sMediaType As String

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sFullPath = txtExportLocation.Text.Trim()
            sMediaType = cboMediaType.Text.Trim()

            m_lReturn = MessageBox.Show("Existing settings will be changed." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Do you want to continue?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If m_lReturn <> System.Windows.Forms.DialogResult.Yes Then
                Return result
            End If

            If sFullPath = "" Then
                sFolder = ""
                sFilename = ""
            Else
                iPos = IIf(sFullPath = "" And "\" = "", 0, (sFullPath.LastIndexOf("\") + 1))
                sFolder = sFullPath.Substring(0, iPos - 1)
                sFilename = Mid(sFullPath, iPos + 1)
            End If

            m_lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=EXPORT_FOLDER, v_sSettingValue:=sFolder)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to save setting 'ExportFolder'", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return result
            End If

            m_lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=EXPORT_FILENAME, v_sSettingValue:=sFilename)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to save setting 'ExportFilename'", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return result
            End If

            m_lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=EXPORT_MEDIATYPE, v_sSettingValue:=sMediaType)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to save setting 'ExportMediaType'", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return result
            End If

            m_lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=EXPORT_BRANCHCODE, v_sSettingValue:=m_sBranchCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to save setting 'ExportMediaType'", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return result
            End If



            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveRegSettings failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveRegSettings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '################################################################################
    ' Method: LoadRegSettings
    ' Description: get export registry settings
    '
    ' History: RDC 12112003 created
    '################################################################################
    Private Function LoadRegSettings() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' get existing settings from the server, if any
            ' export folder
            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=EXPORT_FOLDER, r_sSettingValue:=m_sFolder)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' filename
            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=EXPORT_FILENAME, r_sSettingValue:=m_sFilename)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' media type
            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=EXPORT_MEDIATYPE, r_sSettingValue:=m_sMediaType)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' export branch code
            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=EXPORT_BRANCHCODE, r_sSettingValue:=m_sBranchCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadRegSettings failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadRegSettings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '################################################################################
    ' Method: RunExport
    ' Description: get the export data and write the CSV
    '
    ' History: RDC 12112003 created
    '################################################################################
    Private Function RunExport() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RunExport"

        Dim iPos As Integer
        Dim sFullPath, sMediaType As String
        Dim vExportData(,) As Object
        Dim oFSO As Object
        Dim oFile As FileStream
        Dim sWriter As StreamWriter

        result = gPMConstants.PMEReturnCode.PMTrue

        sFullPath = txtExportLocation.Text.Trim()
        sMediaType = cboMediaType.Text.Trim()

        If sFullPath.Trim() = "" Then
            gPMFunctions.RaiseError("txtExportLocation.Text", "Export Location has not been specified", gPMConstants.PMELogLevel.PMLogError)
        End If

        If m_sCallingAppName = CALLING_APP_WRAPPER Then
            m_lReturn = GetBranchName()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetBranchName", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            iPos = IIf(sFullPath = "" And "\" = "", 0, (sFullPath.LastIndexOf("\") + 1))
            sFullPath = sFullPath.Substring(0, iPos) & m_sBranchCode & "_" & StringsHelper.Format(DateTime.Now, "yymmdd_hhnnss") & ".csv"
        End If


        m_lReturn = m_oBusiness.GetExportData(sMediaDesc:=sMediaType, iSourceID:=m_iSourceID, vExportData:=vExportData)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                GoTo Finally_Renamed

            Else
                gPMFunctions.RaiseError("m_oBusiness.GetExportData", "m_iSourceID = " & m_iSourceID, gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

        oFSO = New Object()

        oFile = New FileStream(sFullPath, FileMode.OpenOrCreate, FileAccess.Write)


        sWriter = New StreamWriter(oFile)

        For lRow As Integer = vExportData.GetLowerBound(1) To vExportData.GetUpperBound(1)

            For iCol As Integer = vExportData.GetLowerBound(0) To vExportData.GetUpperBound(0)
                If iCol = 0 Then
                    sWriter.Write(CStr(String.Format("{0: dd/MM/yyyy} ", CDate(vExportData(iCol, lRow)))))
                Else

                    sWriter.Write(CStr(vExportData(iCol, lRow)).Trim())
                End If

                If iCol < vExportData.GetUpperBound(0) Then
                    sWriter.Write(",")
                End If
            Next
            sWriter.Write(Strings.Chr(13) & Strings.Chr(10))
        Next

        sWriter.Close()

        ' export flag set for all records on cashlist, to ensure that they
        ' are not included in subsequent exports.
        'DC220404 PN11171 -process all branches if via scheduler or core

        m_lReturn = m_oBusiness.SetExportFlag(m_iSourceID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oBusiness.SetExportFlag", "m_iSourceID = " & m_iSourceID, gPMConstants.PMELogLevel.PMLogError)
        End If

        GoTo Finally_Renamed

Catch_Renamed:

        ' DO Not Call any functions before here or the error will be lost
        If m_sCallingAppName = CALLING_APP_WRAPPER Then
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RunExport Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RunExport", excep:=New Exception(Information.Err().Description))

            result = gPMConstants.PMEReturnCode.PMError
        Else
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
        End If

        ' If you want to rollback a transaction or something, do it here

        'causing error at compile time

        If Not (oFile Is Nothing) Then

            sWriter.Close()
        End If
        If Not (oFSO Is Nothing) Then
            File.Delete(sFullPath)
        End If

Finally_Renamed:

        ' Do any tidy up, e.g. Set x = Nothing here
        oFile = Nothing
        oFSO = Nothing

        Return result


    End Function
End Class
