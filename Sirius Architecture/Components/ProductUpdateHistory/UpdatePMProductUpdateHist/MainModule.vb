Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Module MainModule
	
	Public Const ACApp As String = "PMProductUpdateHist"
	
	' number of arguments in command line
	Public Const COMMANDLINE_ARG_COUNT As Integer = 5
	
	Public Const COMMANDLINE_PRODUCT_CODE As Integer = 0
	Public Const COMMANDLINE_PRODUCT_VERSION As Integer = 1
	Public Const COMMANDLINE_RELEASE_NOTES As Integer = 2
	Public Const COMMANDLINE_UPDATE_DESCRIPTION As Integer = 3
	Public Const COMMANDLINE_TARGET_DIR As Integer = 4
	
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
		

        'Developer Guide No.68
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
			
			' full path to update history folder


			vCommandLine(COMMANDLINE_TARGET_DIR) = CStr(vCommandLine(COMMANDLINE_TARGET_DIR)) & "\PM\Product Update History"
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch 
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
End Module