Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms
<System.Runtime.InteropServices.ProgId("iPMValidate_NET.iPMValidate")> _
 Public Module iPMValidate
	' ***************************************************************** '
	'
	' Interface validation module. Contains all of the general
	' validation functions that might be needed for forms.
	'
	' ***************************************************************** '
	
	
	
	' Constant for the methods to identify which class this is.
	Private Const ACClass As String = "PMValidate"
	
	
	' ***************************************************************** '
	' Name: CheckDateGotFocus
	'
	' Description: Checks the format of the date etc for the got focus
	'              event.
	'
	' ***************************************************************** '
	Public Sub CheckDateGotFocus(ByRef ctlControl As Control)
		
		Try 
			

			If ctlControl.Text.Trim() <> "" Then

				ctlControl.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, ctlControl.Text)
			End If
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check the date on the got focus", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckDateGotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: CheckDateLostFocus
	'
	' Description: Checks the format of the date etc for the lost
	'              focus event.
	'
	' ***************************************************************** '
	Public Sub CheckDateLostFocus(ByRef ctlControl As Control)
		
		Dim sDate As String = ""
		
		Try 
			

			If ctlControl.Text.Trim() <> "" Then

				sDate = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, ctlControl.Text)
				
				If sDate = "" Then
					MessageBox.Show("Invalid date", Application.ProductName)
					ctlControl.Focus()
				Else
					ctlControl.Text = sDate
				End If
			End If
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check the date on the lost focus", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckDateLostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: CheckTimeGotFocus
	'
	' Description: Checks the format of the time etc for the got focus
	'              event.
	'
	' ***************************************************************** '
	Public Sub CheckTimeGotFocus(ByRef ctlControl As Control)
		
		Try 
			

			If ctlControl.Text.Trim() <> "" Then

				ctlControl.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatTimeShort, ctlControl.Text)
			End If
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check the time on the got focus", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckTimeGotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: CheckTimeLostFocus
	'
	' Description: Checks the format of the time etc for the lost
	'              focus event.
	'
	' ***************************************************************** '
	Public Sub CheckTimeLostFocus(ByRef ctlControl As Control)
		
		Dim sDate As String = ""
		
		Try 
			

			If ctlControl.Text.Trim() <> "" Then

				sDate = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatTimeLong, ctlControl.Text)
				
				If sDate = "" Then
					MessageBox.Show("Invalid date", Application.ProductName)
					ctlControl.Focus()
				Else
					ctlControl.Text = sDate
				End If
			End If
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check the time on the lost focus", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckTimeLostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: CheckIntegerLostFocus
	'
	' Description: Checks the format of the integer value for the lost
	'              focus event.
	'
	' ***************************************************************** '
	Public Sub CheckIntegerLostFocus(ByRef ctlControl As Control)
		
		Dim sInteger As String = ""
		
		Try 
			

			If ctlControl.Text.Trim() <> "" Then

				sInteger = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatInteger, ctlControl.Text)
				
				If sInteger = "" Then
					MessageBox.Show("Invalid number", Application.ProductName)
					ctlControl.Focus()
				Else
					ctlControl.Text = sInteger
				End If
			End If
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check the integer on the lost focus", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckIntegerLostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: IsChar
	'
	' Description: Checks if the character passed is outside of the
	'              base charater set.
	'
	' ***************************************************************** '
	Public Function IsChar(ByRef sCharacter As String) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			If (Strings.Asc(sCharacter(0))) >= 48 And (Strings.Asc(sCharacter(0))) <= 57 Then
				'char is a digit
				Return gPMConstants.PMEReturnCode.PMTrue
			End If
			
			If (Strings.Asc(sCharacter(0))) >= 65 And (Strings.Asc(sCharacter(0))) <= 90 Then
				'char is between A and Z
				Return gPMConstants.PMEReturnCode.PMTrue
			End If
			
			If Strings.Asc(sCharacter(0)) >= 97 And Strings.Asc(sCharacter(0)) <= 122 Then
				'char is between A and Z
				Return gPMConstants.PMEReturnCode.PMTrue
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check character", vApp:=ACApp, vClass:=ACClass, vMethod:="IsChar", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
End Module