Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.IO
'developer guide no. 129
Imports SharedFiles
Module MainModule
	
	Private Const ACClass As String = "MainModule"
	
	Public Const ACApp As String = "iACTExportCastListItems"
	
	Public Const ERROR_CANCEL_SELECTED As Integer = 32755
	
	Public Const CALLING_APP_WRAPPER As String = "ACTExportCashListItems"
	
	
	' ***************************************************************** '
	' Name: CheckFileExists
	'
	' Description: check if a file exists
	'
	' History:
	' ***************************************************************** '
	Public Function CheckFileExists(ByVal sFilename As String, ByRef bExists As Boolean) As Integer
		
		Dim result As Integer = 0
		Dim oFSO As Object
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			oFSO = New Object()
			
			bExists = File.Exists(sFilename)
			
			oFSO = Nothing
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckFileExists failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckFileExists", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
End Module
