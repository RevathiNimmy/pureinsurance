Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 12 July 2000
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	Public Const ACApp As String = "UsersLoggedOn"
	Private Const ACClass As String = "MainModule"
	
	Public g_sUsername As String = ""
	Public g_sPassword As String = ""
	
	Public g_iLanguageID As Integer
	Public g_iSourceID As Integer
	Public g_sCallingAppName As String = ""
	Public g_iCurrencyID As Integer
	Public g_iUserID As Integer
	
	' Declare an instance of the Business object.
	Public g_oBusiness As bPMLicenceAdmin.LicenceAdmin
	
	Public g_vUserData As Object
	

	Public Sub Main()
		Dim oForm As frmUsers
		Dim lReturn As gPMConstants.PMEReturnCode
		Dim iFilenum As Integer
		
		' RDC 15072002 delete file

		Try 
			
			File.Delete("c:\UsersLoggedOn.txt")
			
			Try 
				
				g_oBusiness = New bPMLicenceAdmin.LicenceAdmin()
				
				' Call the initialise method.
                lReturn = g_oBusiness.Initialise()

                'lReturn = g_oBusiness.Initialise()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    MessageBox.Show("Unable to Connect to Sirius Architecture Database" & Strings.Chr(13) & Strings.Chr(10) & "UsersLoggedOn will be shut down.", "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

                    iFilenum = FileSystem.FreeFile()
                    FileSystem.FileOpen(iFilenum, "C:\UsersLoggedOn.txt", OpenMode.Output)
                    FileSystem.PrintLine(iFilenum, "Error")
                    FileSystem.FileClose(iFilenum)

                    Exit Sub
                End If
				
				'call the function selectdata from the business
				lReturn = g_oBusiness.Selectdata(r_vUserDataArray:=g_vUserData)
				If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Log Error.
					MessageBox.Show("Unable to Read Data from Sirius Architecture Database" & Strings.Chr(13) & Strings.Chr(10) & "UsersLoggedOn will be shut down.", "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
					
					iFilenum = FileSystem.FreeFile()
					FileSystem.FileOpen(iFilenum, "C:\UsersLoggedOn.txt", OpenMode.Output)
					FileSystem.PrintLine(iFilenum, "Error")
					FileSystem.FileClose(iFilenum)
					
					Exit Sub
				End If
				
				If Not Information.IsArray(g_vUserData) Then
					
					iFilenum = FileSystem.FreeFile()
					FileSystem.FileOpen(iFilenum, "C:\UsersLoggedOn.txt", OpenMode.Output)
					FileSystem.PrintLine(iFilenum, "OK")
					FileSystem.FileClose(iFilenum)
					
					Exit Sub
				End If
				
				oForm = New frmUsers()
				
				oForm.ShowDialog()
				
				oForm = Nothing
                g_oBusiness.Dispose()
				g_oBusiness = Nothing
				
				Exit Sub
			
			Catch excep As System.Exception
				
				
				
				' Log Error Message
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Main Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Main", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
				
				Exit Sub
				
			End Try
		
		Catch exc As System.Exception
            'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
		End Try
	End Sub
End Module
