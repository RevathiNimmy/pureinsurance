Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Module MainModule
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iDOCLink"
	
	' Declare necessary API routines:
	Declare Function FindWindow Lib "user32"  Alias "FindWindowA"(ByVal lpClassName As String, ByVal lpWindowName As String) As Integer
	
	
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: {17/5/98}
	'
	' Description: Main module of start stub for DocuMaster.
	'
	' Edit History:
	'
	' SP040898 - Add 2 spaces to end of document manager window caption
	' to avoid confusion with explorer window of same name.
	'
	' MS 06/07/00 - Added code to ensure user is logged on to sirius server
	'               before any Briefcase download can commence. Added reference
	'               to ObjectManager in Project References
	' ***************************************************************** '

	Public Sub Main()
		
		Dim lReturn As Integer
		Dim iDocManager As iDOCManager.Interface_Renamed
		Dim lWinHand As Integer
		Dim oObjectManager As bObjectManager.ObjectManager
		Dim sTmpCommand, sSQLServerSystemName As String
		
		Try 
			
			' Create an instance of the object manager.
#If PD_EARLYBOUND = 1 Then

			Set oObjectManager = New bObjectManager.ObjectManager           ' MS 06/07/00  >
#Else
			oObjectManager = New bObjectManager.ObjectManager()
#End If
			
			' Call the initialise method.
			lReturn = oObjectManager.Initialise(sCallingAppName:="iDocLink")
			
			' Check for errors.
			
			Select Case lReturn
				Case gPMConstants.PMEReturnCode.PMTrue
					'Fine
					
					
				Case gPMConstants.PMEReturnCode.PMCancel
					'Fine, go
					oObjectManager = Nothing
					Exit Sub
					
				Case Else
					' Failed
					' Set the object manager to nothing.
					oObjectManager = Nothing
					
					' Log Error.
					LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:="iDocLink", vClass:="iDocLink", vMethod:="Initialise")
					
					Exit Sub
					
			End Select
			
			' in Briefcase download mode, if logged on locally then abort. Download is from Server only
			' Briefcase download command line format = "BC <filename>"
			sTmpCommand = Interaction.Command()
			sTmpCommand = sTmpCommand.Trim()
			sTmpCommand = sTmpCommand.ToUpper()
			
			If sTmpCommand.StartsWith("BC ") Then
				
				If oObjectManager.LoggedOnLocally Then
					MessageBox.Show("Need Server connection in order to download from DME to Briefcase" & Strings.Chr(13) & Strings.Chr(10) & "Please re-run, log off Sirius and log back with Server option enabled", "DocuMaster Enterprise download to Briefcase aborted", MessageBoxButtons.OK, MessageBoxIcon.Error)
					oObjectManager = Nothing
					Exit Sub
				End If
				
			End If
			
			oObjectManager = Nothing ' MS 06/07/00  <
			
			
			' See if already running
			'SP040898 - see above
			lWinHand = FindWindow(Nothing, "DocuMaster Enterprise  ")
			
			If lWinHand <> 0 Then
				
				'DocuMaster is already running
				'MsgBox DOCAppName & " is already running.", , DOCAppName
				iDocManager =  System.Runtime.InteropServices.Marshal.GetActiveObject("iDOCManager.Interface")
				
				'Show the interface
				lReturn = iDocManager.Activate(sTmpCommand)
				
				If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					'error
					MessageBox.Show("Failed to activate Document Manager.", DOCAppName)
                    iDocManager.Dispose()
                    iDocManager = Nothing
					Exit Sub
				End If
				
			Else
				
				'DocuMaster is not already running
				iDocManager = New iDOCManager.Interface_Renamed()
				
				'initialise the main interface
				lReturn = CType(iDocManager, SSP.S4I.Interfaces.ILocalInterface).Initialise()
				
				
				Select Case lReturn
					Case gPMConstants.PMEReturnCode.PMTrue
						
					Case gPMConstants.PMEReturnCode.PMCancel
                        iDocManager.Dispose()
                        iDocManager = Nothing
						Exit Sub
					Case Else
						'error
						MessageBox.Show("Failed to initialise Document Manager.", DOCAppName)
                        iDocManager.Dispose()
                        iDocManager = Nothing
						Exit Sub
				End Select
				
				'Start the interface
				lReturn = iDocManager.Start(sTmpCommand)
				
				If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					'error
					MessageBox.Show("Failed to start Document Manager.", DOCAppName)
                    iDocManager.Dispose()
                    iDocManager = Nothing
					Exit Sub
				End If
				
			End If
			
			'Finished now
			iDocManager = Nothing
		
		Catch excep As System.Exception
			
			
			
			MessageBox.Show("Failed to start DocuMaster" & Strings.Chr(10).ToString() &  _
			                "Error - " & CStr(Information.Err().Number) & ", " & excep.Message, DOCAppName)
			
			Exit Sub
			
		End Try
		
	End Sub
End Module