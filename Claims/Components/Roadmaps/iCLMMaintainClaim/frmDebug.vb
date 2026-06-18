Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Diagnostics
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmDebug
	Inherits System.Windows.Forms.Form
	Private Const ACClass As String = "frmDebug"
	
	' ***************************************************************** '
	'
	' Name: RefreshDebug
	'
	' Description: Refreshes the debug information
	'
	' History: 18/09/2001 CTAF - Created.
	'
	' ***************************************************************** '
	Public Function RefreshDebug() As Integer
		Dim result As Integer = 0
		Dim sText As String = ""
		
		Dim lstItem As ListViewItem
		Dim sKey As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Current step

            'panStep.Caption = CStr(m_lCurrentStep)
            panStep.Name = CStr(m_lCurrentStep)
			
			' Clear the keys list
			lvwKeys.Items.Clear()
			
			' Keys
			If Information.IsArray(m_vKeyArray) Then
				

				For iLoop1 As Integer = 0 To m_vKeyArray.GetUpperBound(1)
					
					sKey = "K" & iLoop1

                    sText = CStr(m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop1))

                    ' Add an entry
                    lstItem = lvwKeys.Items.Add(sKey, sText, "")

                    If Information.IsReference(m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)) Then
                        'sj 20/09/2002 - start
                        If m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1) Is Nothing Then
                            sText = "(Nothing)"
                        Else
                            sText = "(Object)"
                        End If
                        'sj 20/09/2002 - end
                    ElseIf Information.IsArray(m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)) Then
                        sText = "(Array)"
                        'sj 20/09/2002 - start
                        '            ElseIf m_vKeyArray(PMKeyValue, iLoop1) Is Nothing = True Then
                        '                sText = "(Nothing)"
                        'sj 20/09/2002 - end
                    Else

                        sText = CStr(m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                    End If
					
					' Set the value
					ListViewHelper.GetListViewSubItem(lstItem, 1).Text = sText
					
				Next iLoop1
				
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RefreshDebug Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshDebug", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			


			
			Return result
		End Try
	End Function
	
	Private Sub cmdClose_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClose.Click
		
		Me.Hide()
		
	End Sub
	
	' ***************************************************************** '
	'
	' Name: ShowLog
	'
	' Description: Shows the sirius log file
	'
	' History: 08/07/2002 CTAF - Created.
	'
	' ***************************************************************** '
	Private Function ShowLog() As Integer
		Dim result As Integer = 0
		Dim m_lReturn As Integer
		
		Dim sLogFile As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Read the location of the log file from the registry
			m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="LogFileName", r_sSettingValue:=sLogFile)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Return false
				result = gPMConstants.PMEReturnCode.PMFalse
				' Log Error Message
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowLog Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowLog", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				Return result
			End If
			
			' CTAF - Ideally this should use the default viewer...

			Process.Start("notepad " & sLogFile)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowLog Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowLog", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			


			
			Return result
		End Try
	End Function
	
	
	Private Sub cmdSiriusLog_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSiriusLog.Click
		Dim m_lReturn As Integer
		
		' Show the log file
		m_lReturn = ShowLog()
		
	End Sub
	

	Private Sub frmDebug_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		' Move to the top right
		Me.Top = 0
		Me.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Screen.PrimaryScreen.Bounds.Width) - VB6.PixelsToTwipsX(Me.Width) - 80)
		'Me.Height = Screen.Height - 200
		
	End Sub
	
	Private Sub frmDebug_Paint(ByVal eventSender As Object, ByVal eventArgs As PaintEventArgs) Handles MyBase.Paint
		
		'SetTopmost Me
		
	End Sub
	
	Private Sub frmDebug_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		
		'ClearTopmost Me
		
		eventArgs.Cancel = Cancel <> 0
	End Sub
	
	Private Sub lvwKeys_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwKeys.MouseUp
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		
		If Button = MouseButtonConstants.RightButton Then
			Ctx_mnuEntry.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
		End If
		
	End Sub
	
	Public Sub mnuEntryCopy_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuEntryCopy.Click
		Dim m_lReturn As Integer
		
		m_lReturn = CopyClipboard()
		
	End Sub
	
	' ***************************************************************** '
	'
	' Name: CopyClipboard
	'
	' Description:
	'
	' History: 18/09/2001 CTAF - Created.
	'
	' ***************************************************************** '
	Public Function CopyClipboard() As Integer
		
		Dim result As Integer = 0
		Dim sText As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If lvwKeys.FocusedItem Is Nothing Then
				Return result
			End If
			
            'sText = lvwKeys.listViewHelper1.GetListViewSubItem(lvwKeys.FocusedItem, 1).Text
            sText = lvwKeys.FocusedItem.SubItems(1).Text
			
			' Clear the clipboard
			My.Computer.Clipboard.Clear()
			' Add the new entry

			My.Computer.Clipboard.SetText(sText, TextDataFormat.Text)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyClipboard Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyClipboard", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			


			
			Return result
		End Try
	End Function
End Class
