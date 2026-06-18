Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Windows.Forms
Imports SharedFiles

Module MainModule
	
	Public Const ACApp As String = "iDOCScanStub"
	
	' **********************************************************************8* '
	' Module Name: iDOCScanStub
	'
	' Date:        13/07/98
	'
	' Description: Stub to start iDOCScan. Runs in Stand Alone mode
	'
	' Edit History:
	'               16/07/98    CF  Added check for PMCancel after logon
	'               06/08/98    CF  Changed so Splash isn't shown here. Form left in
	'                               for the icon though.
	'
	' ************************************************************************ '
	

	Public Sub Main()
	
		Dim vTemp As Object
		Dim lRet As Integer
		Dim oObj As Object
		
		Try 
			
			' We only want one copy running
			Dim Procesos() As Process = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName)
			If Procesos.Length > 1 And Process.GetCurrentProcess().StartTime <> Procesos(0).StartTime Then
				MessageBox.Show("ScanStation is already running. Please close down an instance." &  _
				                Environment.NewLine &  _
				                "You may need to use Task Manager if the application did not exit correctly last time.", "ScanStation", MessageBoxButtons.OK, MessageBoxIcon.Error)
				Environment.Exit(0)
			End If
			
			' Show a brief splash screen
			' The only reason this exists is because we needed a form for
			' an icon, so thought might as well use the form for something.
			'    frmInterface.Show
			'    frmInterface.Refresh
			
			' Create an instance of the scan interface
            oObj = New iDOCScan.interface_Renamed()
			
			' Intialise it

			lRet = oObj.Initialise(True)
			If lRet <> gPMConstants.PMEReturnCode.PMTrue Then
				frmInterface.Hide()
				If lRet <> gPMConstants.PMEReturnCode.PMCancel Then
					MessageBox.Show("Unable to create ScanStation interface.", Application.ProductName)
				End If
				
				frmInterface.Close()
				frmInterface = Nothing
				
				oObj = Nothing
				
				Environment.Exit(0)
			End If
			
			' Hide the splash screen
			'frmInterface.Hide
			

			lRet = oObj.Scan(vFolderTree:=vTemp)
			If lRet <> gPMConstants.PMEReturnCode.PMTrue Then
				MessageBox.Show("Unable to display scan dialog.", Application.ProductName)
			End If
			

            oObj.Dispose()

            oObj = Nothing
			
			Environment.Exit(0)
		
		Catch 
		End Try
		
		
		
		MessageBox.Show("Error : " & Information.Err().Description & Environment.NewLine & "Number : " & CStr(Information.Err().Number), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
		
		Environment.Exit(0)
		
	End Sub
End Module