Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Module MainModule
	' #################################################################################
	' Program:     PMRegELSupport
	' Description: In order for components to write to the O/S Application event log
	'              two non-COMM DLLs must be registered on the system. These DLLS
	'              cannot be registered using normal methods. The DLLs are registered
	'              during the installation of Sirius Architecture, but to perform this
	'              task 'manually', run this program with a commandline set to the path
	'              that contains the DLLs cPMEventLogMsg.dll and cPMEventLogCat.dll
	'              eg. c:\program files\pm\sirius architecture\common\system
	' Created by:  RDC 25072002
	' #################################################################################
	
	Public Const ACApp As String = "PMRegELSupport"
	
	Public Sub Main()
		
		Dim bCatFound, bMsgFound As Boolean
		Dim lReturn As gPMConstants.PMEReturnCode
		Dim sCmd As String = ""
        Dim oInterface As Interface_Renamed
		
		Try 
			
			' get command line
			sCmd = Interaction.Command().Trim()
			
			' no trailing \ please
			If sCmd.EndsWith("\") Then
				sCmd = sCmd.Substring(0, sCmd.Length - 1)
			End If
			
			' start the form up
            oInterface = New Interface_Renamed()
			

            'Load(oInterface)
			
			oInterface.lblStatus.Text = ""
			oInterface.cmdOk.Visible = False
			
			If sCmd = "" Then
				' path not supplied
				oInterface.lblStatus.Text = "Command line parameter (path) not supplied"
				oInterface.cmdOk.Visible = True
				
				oInterface.ShowDialog()
				
				oInterface.Close()
				
				oInterface = Nothing
				
				Exit Sub
			End If
			
			' check DLLs exist in target path
			bCatFound = False
			bMsgFound = False
			
			If FileSystem.Dir(sCmd & "\" & CATEGORY_FILE, FileAttribute.Normal) <> "" Then
				bCatFound = True
			End If
			
			If FileSystem.Dir(sCmd & "\" & MESSAGE_FILE, FileAttribute.Normal) <> "" Then
				bMsgFound = True
			End If
			
			If Not bCatFound Or Not bMsgFound Then
				' DLLs don't exist
				oInterface.lblStatus.Text = "Message and/or Category file not found in supplied path"
				oInterface.cmdOk.Visible = True
				
				oInterface.ShowDialog()
				
				oInterface.Close()
				
				oInterface = Nothing
				
				Exit Sub
			End If
			
			oInterface.lblStatus.Text = "Registering event log support DLLs ..."
			
			Application.Run(oInterface)
			Application.DoEvents()
			
			' register DLLs
			lReturn = CType(RegisterEventLogDLLs(sCmd), gPMConstants.PMEReturnCode)
			
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' it failed
				oInterface.Hide()
				
				oInterface.lblStatus.Text = "Failed to register event log support DLLs"
				oInterface.cmdOk.Visible = True
				
				oInterface.ShowDialog()
			End If
			
			oInterface.Close()
			
			oInterface = Nothing
		
		Catch excep As System.Exception
			
			
			
			oInterface.lblStatus.Text = "Error " & Information.Err().Number & ": " & excep.Message
			oInterface.cmdOk.Visible = True
			
			oInterface.ShowDialog()
			
			oInterface.Close()
			
			oInterface = Nothing
			
		End Try
		
	End Sub
End Module