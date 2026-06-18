Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports VB6 = Microsoft.VisualBasic.Compatibility.VB6
<System.Runtime.InteropServices.ProgId("iGeneralFunc_NET.iGeneralFunc")> _
 Public Module iGeneralFunc
	' ***************************************************************** '
	'
	' Interface general functions module. Contains all of the global
	' functions that might be useful when writing the interface layer.
	'
	' ***************************************************************** '
	
	
	
	' Constant for the methods to identify which class this is.
	Private Const ACClass As String = "iGeneralFunc"
	
	
	' ***************************************************************** '
	' Name: LogMessage
	'
	' Description: Wrapper function to the LogMessage method call.
	'
	' ***************************************************************** '
	Public Sub LogMessage(ByRef iType As Integer, ByRef sMsg As String, Optional ByRef vApp As String = "", Optional ByRef vClass As String = "", Optional ByRef vMethod As String = "", Optional ByRef vErrNo As Byte = 0, Optional ByRef vErrDesc As String = "")
        Dim LogMessagePopup(,,,,,,) As Object = Nothing
		
		Try 
			
			' If Error Type is not supplied, set it to Fatal Error
			If iType = 0 Then
				iType = GeneralConst.PMLogFatal
			End If
			
			' Set the Optional Parameters to a default value if they are not supplied.

			If Information.IsNothing(vErrNo) Then
				vErrNo = 0
			End If
			

			If Information.IsNothing(vErrDesc) Then
				vErrDesc = ""
			End If
			

			If Information.IsNothing(vApp) Then
				vApp = ""
			End If
			

			If Information.IsNothing(vClass) Then
				vClass = ""
			End If
			

			If Information.IsNothing(vMethod) Then
				vMethod = ""
			End If
			
			Dim tempAuxVar As Object = LogMessagePopup(iType, CInt(sMsg), CInt(vApp), CInt(vClass), CInt(vMethod), vErrNo, CInt(vErrDesc))
		
		Catch 
			
			
			
			' Error Section.
			
			' Failed to log message, so we must call the
			' function to popup the message instead.
			Dim tempAuxVar2 As Object = LogMessagePopup(iType, CInt(sMsg), CInt(vApp), CInt(vClass), CInt(vMethod), vErrNo, CInt(vErrDesc))
			
			Exit Sub
		End Try
		
		
	End Sub
	
	' ***************************************************************** '
	' Name: SelectText
	'
	' Description: Hightlights any text within the control passed.
	'
	' ***************************************************************** '
	Public Sub SelectText(ByRef ctlControl As Control)
		
		Try 
			
			' Set the controls properties.
			With ctlControl

				ReflectionHelper.SetMember(ctlControl, "SelStart", 0)

				ReflectionHelper.SetMember(ctlControl, "SelLength", Strings.Len(ctlControl.ToString()))
			End With
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Log Error.
			LogMessage(iType:=GeneralConst.PMLogOnError, sMsg:="Failed to select the text", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectText", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: SetMousePointer
	'
	' Description: Sets the mouse pointer to the state passed.
	'
	' ***************************************************************** '
	Public Sub SetMousePointer(ByRef iMouseState As Integer)
		
		Static iMouseCounter As Integer
		
		Try 
			
			' Check the mouse state.
			Select Case (iMouseState)
				Case GeneralConst.PMMouseBusy
					' Set to busy mode.
					
					' Increament the mouse counter.
					iMouseCounter += 1
					
					' Set the mouse pointer to the busy state,
					' but only set it once.
					If iMouseCounter = 1 Then
						Cursor.Current = Cursors.WaitCursor
					End If
					
				Case GeneralConst.PMMouseNormal
					' Set to normal mode.
					
					If iMouseCounter > 0 Then
						' Decreament the mouse counter.
						iMouseCounter -= 1
					End If
					
					' Set the mouse pointer to the normal state.
					If iMouseCounter = 0 Then
						Cursor.Current = Cursors.Default
					End If
					
				Case GeneralConst.PMMouseReset
					' Reset to normal mode.
					
					' Reset the mouse counter.
					iMouseCounter = 0
					
					' Set the mouse pointer to the normal state.
					Cursor.Current = Cursors.Default
					
				Case Else
					' Invaild mouse state.
					
			End Select
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Reset the mouse counter.
			iMouseCounter = 0
			
			' Set the mouse pointer to the normal state.
			Cursor.Current = Cursors.Default
			
			' Log Error.
			LogMessage(iType:=GeneralConst.PMLogOnError, sMsg:="Failed to set the mouse pointer", vApp:=ACApp, vClass:=ACClass, vMethod:="SetMousePointer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: CenterForm
	'
	' Description: Center's the form to the screen.
	'
	' ***************************************************************** '
	Public Sub CenterForm(ByRef frmForm As Form)
		
		Try 
			
			' Center the form
			frmForm.SetBounds((Screen.PrimaryScreen.Bounds.Width / 2) - (frmForm.Width / 2), (Screen.PrimaryScreen.Bounds.Height / 2) - (frmForm.Height / 2), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			
			' Log Error.
			LogMessage(iType:=GeneralConst.PMLogOnError, sMsg:="Failed to center the form", vApp:=ACApp, vClass:=ACClass, vMethod:="CenterForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: PositionForm
	'
	' Description: Positions the form using the coordinates passed.
	'
	' ***************************************************************** '
	Public Sub PositionForm(ByRef frmForm As Form, ByRef lTop As Integer, ByRef lLeft As Integer)
		
		Try 
			
			' Center the form
			frmForm.SetBounds(VB6.TwipsToPixelsX(lLeft), VB6.TwipsToPixelsY(lTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			
			' Log Error.
			LogMessage(iType:=GeneralConst.PMLogOnError, sMsg:="Failed to position the form", vApp:=ACApp, vClass:=ACClass, vMethod:="PositionForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Exit Sub
			
		End Try
		
	End Sub
    'Modified by Deepak Sharma on 4/20/2010 2:06:44 PM refer developer guide no. 29(No Solution)
    'Shared Sub New()
    '    MainModule.JustForInvokeMain()

End Module