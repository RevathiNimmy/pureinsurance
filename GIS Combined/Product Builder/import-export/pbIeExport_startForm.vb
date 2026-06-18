Option Strict Off
Option Explicit On

Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles

Partial Friend Class startForm
    Inherits System.Windows.Forms.Form

    Dim mainForm As New mainForm

    Dim warningForm As New warningForm
    ' RAW 02/09/2003 : CQ2158 : added code to get SQLServer version

    Private Sub startForm_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Dim bPMUser As bPMUser.Business        

        'force a log on before calling mainForm
        Dim g_oObjectManager As New bObjectManager.ObjectManager

        ' Call the initialise method.

        Dim lReturn As gPMConstants.PMEReturnCode = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
        If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            'try to access the database
            lReturn = gPMComponentServices.NewDatabase(g_sUsername, g_iSourceID, g_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_oDatabase:=g_oDatabase)
        End If

        If lReturn = gPMConstants.PMEReturnCode.PMTrue Then

            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID

                lReturn = g_oObjectManager.GetInstance(bPMUser, "bPMUser.Business", vInstanceManager:="ClientManager")

            End With
        End If

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            If lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                MessageBox.Show("User cancelled logon. Application will now terminate.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Could not log on to Sirius Solutions", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

            If Not (g_oObjectManager Is Nothing) Then
                g_oObjectManager.Dispose()
            End If
            g_oObjectManager = Nothing
            If Not (bPMUser Is Nothing) Then

                bPMUser.Dispose()
            End If
            bPMUser = Nothing
            Me.Close()
            Environment.Exit(0)
        End If

        'check if logged on with system access
        lReturn = GetSysAdminStatus(bPMUser)

        If Not (g_oObjectManager Is Nothing) Then
            g_oObjectManager.Dispose()
        End If
        g_oObjectManager = Nothing
        If Not (bPMUser Is Nothing) Then

            bPMUser.Dispose()
        End If
        bPMUser = Nothing

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Me.Close()
            Environment.Exit(0)
        End If

        ' RAW 02/09/2003 : CQ2158 : added
        If GetSQLServerVersion(r_oDatabase:=g_oDatabase) <= 0 Then
            MessageBox.Show("Failed to get SQLServer Version", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
            Environment.Exit(0)
        End If

        warningForm.ShowDialog()
        PbEdoEXport.objFrmMainForm = Me.mainForm
        mainForm.ShowDialog()

        mainForm.radioExportBasedOn(radioExportBasedOn_Migration).Visible = False
        mainForm.txtdatamodelId.Visible = False
        mainForm.Show()
        Me.Close()

    End Sub
    ' ***************************************************************** '
    ' Name: GetSysAdminStatus
    '
    ' Description: check if user is member of a SysAdmin user group.
    '
    ' History: RDC 16102002 created
    ' ***************************************************************** '
    Private Function GetSysAdminStatus(ByRef r_oBusiness As bPMUser.Business) As Integer

        Dim result As Integer = 0
        Dim lStatus As Integer
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            lReturn = r_oBusiness.GetSysAdminStatus(lStatus)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or lStatus = 0 Then

                MessageBox.Show("You do not have permission to access " & _
                                ACApp & "." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                                "Please contact your System Administrator.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return result
            End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSysAdminStatus Failed", vApp:=ACApp, vClass:="", vMethod:="GetSysAdminStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
End Class
