Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Text
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmCopy
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmCopy
	'
	' Date: {TodaysDate}
	'
	' Description: Copy interface.
	'
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
    'developer guide no. 50
    Dim objfrmProgress As New frmProgress
    'developer guide no. 50
    Dim objfrmLog As New frmLog
	Private Const ACClass As String = "frmCopy"
	
	'Return value
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	'Business object
	Private m_oBusiness As Object
	
	'Collection of DSNs
	Private m_cSystemDSN As Collection
	
	'Processes and maps
	Private m_vProcesses( ,  ) As Object
	Private m_vMaps( ,  ) As Object
	
	'Cancel flag
	Private m_bCancel As Boolean
	
	'Properties
	Private m_sDSN As String = ""
	Private m_sSourceType As String = ""
	Private m_lSourceID As Integer
	Private m_sDestination As String = ""
	Private m_sSourceName As String = ""
	
	Private Sub ShowOK()
		
		
		cmdOK.Enabled = ((cboProcess.SelectedIndex <> -1) Or (cboMap.SelectedIndex <> -1)) And txtDestination.Text <> ""
		
		
	End Sub
	
	' ***************************************************************** '
	' Name: Start ( Public )
	'
	' Description: Starts Copy form
	'
	' ***************************************************************** '
	Public Function Start(ByRef lNewID As Integer) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Show this form
			Me.ShowDialog()
			
			Me.Close()
			
			Return result
		
		Catch excep As System.Exception
			
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Copy Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Copy Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result

        End Try
    End Function

    Private Sub cboDSN_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboDSN.SelectedIndexChanged


        Try

            'Clear processes and maps
            cboProcess.Items.Clear()
            cboMap.Items.Clear()
            m_vProcesses = Nothing
            m_vMaps = Nothing

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'On change get the Processes and Maps for database

            m_lReturn = m_oBusiness.DSNProcessMaps(v_sDSN:=cboDSN.Text, r_vProcesses:=m_vProcesses, r_vMaps:=m_vMaps)

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Could not get details for this database.", "Get ProcessMaps", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            'Display Processes
            If Information.IsArray(m_vProcesses) Then

                For iPtr As Integer = m_vProcesses.GetLowerBound(1) To m_vProcesses.GetUpperBound(1)
                    cboProcess.Items.Add(CStr(m_vProcesses(1, iPtr)))
                Next iPtr

            End If

            'Display Maps
            If Information.IsArray(m_vMaps) Then

                For iPtr As Integer = m_vMaps.GetLowerBound(1) To m_vMaps.GetUpperBound(1)
                    cboMap.Items.Add(CStr(m_vMaps(1, iPtr)))
                Next iPtr

            End If

        Catch


            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        End Try


    End Sub

    Private Sub cboMap_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboMap.SelectedIndexChanged

        txtDestination.Focus()

    End Sub

    'PRIVATE CONSTANTS
    Private Sub cboMap_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboMap.Leave

        If (cboProcess.SelectedIndex <> -1) And (cboMap.SelectedIndex <> -1) Then
            cboProcess.SelectedIndex = -1
        End If

        ShowOK()

    End Sub


    Private Sub cboProcess_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboProcess.SelectedIndexChanged

        txtDestination.Text = cboProcess.Text
        txtDestination.Focus()

        ' CTAF 250100 - Updated the script file if enabled
        If txtFile.Enabled Then
            txtFile.Text = "C:\" & StringToFilename(cboProcess.Text) & ".sql"
        End If

    End Sub

    Private Sub cboProcess_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboProcess.Leave

        If (cboProcess.SelectedIndex <> -1) And (cboMap.SelectedIndex <> -1) Then
            cboMap.SelectedIndex = -1
        End If

        ShowOK()

    End Sub


    ' ***************************************************************** '
    ' Name: Initialise ( Public )
    '
    ' Description: Initialises Copy form
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef cSystemDSN As Collection, ByRef r_oBusiness As Object) As gPMConstants.PMEReturnCode




        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Make sure that we have DSNs
            If cSystemDSN.Count < 1 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the business object
            m_oBusiness = r_oBusiness

            'Get the List of System DSNs
            m_cSystemDSN = cSystemDSN

            'Get the Current DSN

            m_sDSN = m_oBusiness.DSN

            'Initialise properties
            m_sSourceType = ""
            m_lSourceID = 0
            m_sDestination = ""

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Copy Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Copy Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Sub chkComments_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkComments.CheckStateChanged

        cmdOK.Enabled = True
        ShowOK()

    End Sub

    ' CTAF 250100 - Function to convert a process name to a filename
    '               Note - strips spaces.
    Private Function StringToFilename(ByRef sString As String) As String

        Dim sByte_Renamed As New FixedLengthString(1)

        Dim sReturn As New StringBuilder

        Dim l As Integer
        For l = sByte_Renamed.Value To sString.Length
            Select Case sByte_Renamed.Value
                Case "*", "?", " ", "/", "\"
                Case Else
                    sReturn.Append(sByte_Renamed.Value)
            End Select
        Next

        Return sReturn.ToString()

    End Function


    Private Sub chkOutputScript_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkOutputScript.CheckStateChanged

        If chkOutputScript.CheckState = CheckState.Checked Then
            chkComments.CheckState = CheckState.Checked
            chkComments.Enabled = True
            txtFile.Enabled = True

            ' CTAF 250100 - Set the script name to the same as the process
            txtFile.Text = "C:\" & StringToFilename(cboProcess.Text) & ".sql"

        Else
            chkComments.CheckState = CheckState.Unchecked
            chkComments.Enabled = False
            txtFile.Enabled = False

        End If

        cmdOK.Enabled = True
        ShowOK()

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        m_bCancel = True
        Me.Hide()

    End Sub


    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim lNewID As Integer

        m_bCancel = False

        'Get the selected DSN
        m_sDSN = cboDSN.Text

        'Get the Selected Source type
        If cboProcess.SelectedIndex <> -1 Then
            m_sSourceType = NavProcConst.NavGrpProcess
            m_lSourceID = CInt(Conversion.Val(CStr(m_vProcesses(0, cboProcess.SelectedIndex))))
            m_sSourceName = cboProcess.Text

        ElseIf (cboMap.SelectedIndex <> -1) Then
            m_sSourceType = NavProcConst.NavGrpMap
            m_lSourceID = CInt(Conversion.Val(CStr(m_vMaps(0, cboMap.SelectedIndex))))
            m_sSourceName = cboMap.Text

        End If

        'Exit if Cancel
        If m_bCancel Then
            m_sDestination = ""
            Me.Hide()
            Exit Sub
        End If

        'Get the destination name
        m_sDestination = txtDestination.Text

        If m_sDestination = "" Then
            'No new name so exit
            Me.Hide()
            Exit Sub
        End If

        ' Set the mouse pointer to busy.
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        'Show Progress
        'developer guide no. 50
        m_lReturn = objfrmProgress.Initialise("Copying...Please Wait." & _
              cboProcess.Text, 0, , , False)

        'Check if build output script has been selected

        m_oBusiness.BuildCopyScript = (chkOutputScript.CheckState = CheckState.Checked)

        'Check if script comments has been selected

        m_oBusiness.CopyScriptComments = (chkComments.CheckState = CheckState.Checked)

        'Copy

        m_lReturn = m_oBusiness.Copy(v_sPMNavGroup:=m_sSourceType, v_sDSN:=m_sDSN, v_lID:=m_lSourceID, v_lNewID:=lNewID, v_sDescription:=m_sDestination, v_sSourceName:=m_sSourceName)

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        'Terminate Progress
        'developer guide no. 50
		objfrmProgress.Dispose()

        'Show copy log
        If chkOutputScript.CheckState = CheckState.Unchecked Then

            'developer guide no. 50
            objfrmLog.Initialise(m_oBusiness.CopyLog)
        Else


            'developer guide no. 50
            objfrmLog.Initialise(m_oBusiness.CopyScript, txtFile.Text)



            MessageBox.Show("Lookup Count = " & m_oBusiness.LookupCount & Strings.Chr(13) & _
                            "Insert Record Count = " & m_oBusiness.InsertRecordCount, Application.ProductName)

            'Clear the script files on business

            m_oBusiness.CopyLogSQLClear()

        End If

        'developer guide no. 50
        objfrmLog.Close()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        'Everthing went fine

        cmdOK.Enabled = False

    End Sub



    Private Sub frmCopy_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load


        'Default Copy Output Script
        chkOutputScript.CheckState = CheckState.Unchecked

        m_oBusiness.BuildCopyScript = False

        'Display System DSNs
        For iPtr As Integer = 1 To m_cSystemDSN.Count
            cboDSN.Items.Add(CStr(m_cSystemDSN.Item(iPtr)))
        Next iPtr

        'Show current DSN
        cboDSN.Text = m_sDSN

    End Sub
	
	Private Sub frmCopy_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		
		m_bCancel = True
		
		eventArgs.Cancel = Cancel <> 0
	End Sub
	
	
	Private isInitializingComponent As Boolean
	Private Sub txtDestination_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDestination.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		cmdOK.Enabled = True
		ShowOK()
		
	End Sub
	
	
	Private Sub txtFile_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFile.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		cmdOK.Enabled = True
		ShowOK()
		
	End Sub
End Class
