Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmStartInstall
	Inherits System.Windows.Forms.Form
	
	Private f_sPMProductCode As String = ""
	Private f_sClientInstallPath As String = ""
	Private f_sClientInstallProgram As String = ""
	Private f_iClientRebootLevel As Integer
	
	Private f_oParent As Object
	
	' ***************************************************************** '
	' Name: StartInstallTimer
	'
	' Description:
	'
	'
	' ***************************************************************** '
	Public Function StartInstallTimer(ByVal v_sPMProductCode As String, ByVal v_sClientInstallPath As String, ByVal v_sClientInstallProgram As String, ByVal v_iClientRebootLevel As Integer, ByVal v_oParent As Object) As Integer
		
		Dim result As Integer = 0

		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			f_sPMProductCode = v_sPMProductCode
			f_sClientInstallPath = v_sClientInstallPath
			f_sClientInstallProgram = v_sClientInstallProgram
			f_iClientRebootLevel = v_iClientRebootLevel
			
			' We need this the Component Doesn't Get Destroyed
			f_oParent = v_oParent
			
			tmrStartInstall.Interval = 1000
			tmrStartInstall.Enabled = True
			tmrStartInstall.Enabled = True
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartInstallTimerFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="StartInstallTimer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: StartInstall
	'
	' Description:
	'
	'
	' ***************************************************************** '
	Private Sub StartInstall()
		
		Dim sInstallProgram, sRunOnceName As String
		Dim dShellRet As Double
		
		Try 
			
			tmrStartInstall.Enabled = False
			
			sInstallProgram = f_sClientInstallPath.Trim() & f_sClientInstallProgram.Trim()
			
			If f_iClientRebootLevel = ACNoReboot Then
				

				Dim startInfo As ProcessStartInfo = New ProcessStartInfo(sInstallProgram)
				startInfo.WindowStyle = ProcessWindowStyle.Normal
				dShellRet = Process.Start(startInfo).Id
				
				If dShellRet = 0 Then
                    gPMFunctions.LogMessageToFile(susername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Start Install. Shell Failed : " & sInstallProgram, vApp:=ACApp)
                End If

            Else

                ' Create a Key Name
                sRunOnceName = f_sPMProductCode.Trim() & ACRunSettingName

                ' Add the Install Program Into the run once section of the Registry
                gPMFunctions.SetKeyValue(gPMConstants.HKEY_LOCAL_MACHINE, ACRunOnceRegKey, sRunOnceName, sInstallProgram, gPMConstants.REG_SZ)

                ' Either Logoff or Reboot the PC
                If f_iClientRebootLevel = ACLogoffOnly Then
                    ShutDownPC(True)
                Else
                    ShutDownPC(False)
                End If

            End If

            f_oParent = Nothing

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartInstallFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="StartInstall", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
		
	End Sub
	
	
	Private Sub tmrStartInstall_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tmrStartInstall.Tick
		
		StartInstall()
		
	End Sub
End Class
