Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Module MainModule
	
	Public Const ACApp As String = "UpdateDevice"
	
	' number of arguments in command line
	Public Const COMMANDLINE_ARG_COUNT As Integer = 3
	
	Public Const COMMANDLINE_PRODUCT_CODE As Integer = 0
	Public Const COMMANDLINE_DOCUMENT_STORE_PATH As Integer = 1
	Public Const COMMANDLINE_DOCUMENT_SHARE As Integer = 2
	
	Public g_sUsername As String = ""
	Public g_sPassword As String = ""
	Public g_iSourceID As Integer
	Public g_iUserID As Integer
	Public g_iLanguageID As Integer
	Public g_iCurrencyID As Integer
	Public g_iLogLevel As Integer
	Public g_sCallingAppName As String = ""
	
	' program starts here - program designed for SERVER ONLY!!

	Public Sub Main()
		
		
		Dim oInterface As New frmInterface
		

        'Load(oInterface)
		
		oInterface.Start()
		
		oInterface.Close()
		
		oInterface = Nothing
		
	End Sub
	
	' split comma separated parms in commandline into array
	Public Function ParseCommandLine(ByVal sCmd As String, ByRef vCommandLine() As Object) As Integer
		
		Dim result As Integer = 0
        Dim iCount As Integer
        Dim sChar As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Parse command line
			vCommandLine = sCmd.Split(","c)
			
			iCount = vCommandLine.GetUpperBound(0) - vCommandLine.GetLowerBound(0) + 1
			
			' check number of arguments matches number expected
			If iCount <> COMMANDLINE_ARG_COUNT Then
				MessageBox.Show("Wrong number of arguments in command line." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) &  _
				                "Given = " & CStr(iCount) & ", expected = " & CStr(COMMANDLINE_ARG_COUNT), "PMProductUpdateHistory", MessageBoxButtons.OK, MessageBoxIcon.Error)
				
				Return result
			End If
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch 
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
End Module