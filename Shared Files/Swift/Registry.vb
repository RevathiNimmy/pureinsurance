Option Strict Off
Option Explicit On
Imports System
Module MRegistry
	

	Public Const ksRegPathPlanner As String = CStr(ksRegPathApplications) & "\FactFind"
	
	Public Function RegReadMachineSetting(ByVal sFolder As String, ByVal sAttribute As String, ByVal vDefault As Byte) As Object
		Dim RegRead( ,  ,  ,  ) As Object
		
		If sFolder.Trim() <> "" Then
			Return RegRead(gPMConstants.HKEY_LOCAL_MACHINE, CInt(ksRegPathPlanner & "\" & sFolder), CInt(sAttribute), vDefault)
		Else
			Return RegRead(gPMConstants.HKEY_LOCAL_MACHINE, CInt(ksRegPathPlanner), CInt(sAttribute), vDefault)
		End If
		
	End Function
	
	Public Function RegReadUserSetting(ByVal sFolder As String, ByVal sAttribute As String, ByVal vDefault As Byte) As Object
		Dim RegRead( ,  ,  ,  ) As Object
		
		If sFolder.Trim() <> "" Then
			Return RegRead(gPMConstants.HKEY_CURRENT_USER, CInt(ksRegPathPlanner & "\" & sFolder), CInt(sAttribute), vDefault)
		Else
			Return RegRead(gPMConstants.HKEY_CURRENT_USER, CInt(ksRegPathPlanner), CInt(sAttribute), vDefault)
		End If
		
	End Function
	
	Public Sub RegWriteMachineSetting(ByVal sFolder As String, ByVal sAttribute As String, ByVal vValue As Byte)
		Dim RegWrite( ,  ,  ,  ) As Object
		
		If sFolder.Trim() <> "" Then
			Dim tempAuxVar As Object = RegWrite(gPMConstants.HKEY_LOCAL_MACHINE, CInt(ksRegPathPlanner & "\" & sFolder), CInt(sAttribute), vValue)
		Else
			Dim tempAuxVar2 As Object = RegWrite(gPMConstants.HKEY_LOCAL_MACHINE, CInt(ksRegPathPlanner), CInt(sAttribute), vValue)
		End If
		
	End Sub
	
	Public Sub RegWriteUserSetting(ByVal sFolder As String, ByVal sAttribute As String, ByVal vValue As Byte)
		Dim RegWrite( ,  ,  ,  ) As Object
		
		If sFolder.Trim() <> "" Then
			Dim tempAuxVar As Object = RegWrite(gPMConstants.HKEY_CURRENT_USER, CInt(ksRegPathPlanner & "\" & sFolder), CInt(sAttribute), vValue)
		Else
			Dim tempAuxVar2 As Object = RegWrite(gPMConstants.HKEY_CURRENT_USER, CInt(ksRegPathPlanner), CInt(sAttribute), vValue)
		End If
		
	End Sub
End Module