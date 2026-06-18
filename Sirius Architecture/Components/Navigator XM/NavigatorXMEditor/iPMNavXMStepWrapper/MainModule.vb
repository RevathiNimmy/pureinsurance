Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports SharedFiles
Module MainModule
	Dim nShow As ProcessWindowStyle
	
	Private m_lReturn As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	Public Const ACApp As String = "iPMNavXMStepWrapper"
	Private Const ACClass As String = "MainModule"
	
	Private Const PROCESS_QUERY_INFORMATION As Integer = 1024
	Private Const PROCESS_VM_READ As Integer = 16
	Private Const MAX_PATH As Integer = 260
	Private Const STANDARD_RIGHTS_REQUIRED As Integer = &HF0000
	Private Const SYNCHRONIZE As Integer = &H100000
	Private Const PROCESS_ALL_ACCESS As Integer = &H1F0FFF
	Private Const TH32CS_SNAPPROCESS As Integer = &H2
	Private Const hNull As Integer = 0
	
	Public Const SEE_MASK_NOCLOSEPROCESS As Integer = &H40
	Public Const SEE_MASK_FLAG_DDEWAIT As Integer = &H100
	Public Const INFINITE As Integer = &HFFFFFFFF
	
	Private Structure SHELLEXECUTEINFO
		Dim cbSize As Integer
		Dim fMask As Integer
		Dim hWnd As Integer
		Dim lpVerb As String
		Dim lpFile As String
		Dim lpParameters As String
		Dim lpDirectory As String
		Dim nShow As Integer
		Dim hInstApp As Integer
		' Optional fields
		Dim lpIDList As Integer
		Dim lpClass As String
		Dim hkeyClass As Integer
		Dim dwHotKey As Integer
		Dim hIcon As Integer
		Dim hProcess As Integer
		Public Shared Function CreateInstance() As SHELLEXECUTEINFO
			Dim result As New SHELLEXECUTEINFO
			result.lpVerb = String.Empty
			result.lpFile = String.Empty
			result.lpParameters = String.Empty
			result.lpDirectory = String.Empty
			result.lpClass = String.Empty
			Return result
		End Function
	End Structure
	
	' runs executable, or runs associated executable for specified file

	Private Declare Function ShellExecuteEx Lib "shell32.dll" (ByRef lpExecInfo As SHELLEXECUTEINFO) As Integer
	
	Private Declare Function WaitForSingleObject Lib "kernel32" (ByVal hHandle As Integer, ByVal dwMilliseconds As Integer) As Integer
	
	Private Declare Function CloseHandle Lib "Kernel32.dll" (ByVal Handle As Integer) As Integer
	
	Public Function GetObjectManager() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Instance of object manager
			g_oObjectManager = New bObjectManager.ObjectManager()
			
			m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetObjectManager failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetObjectManager", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
	
	Public Function OpenDocument(ByVal sFileSpec As String, ByVal sParameters As String) As Integer
		
		Dim result As Integer = 0
		Dim bWaitUntilFinished As Boolean
		Dim lpExecInfo As SHELLEXECUTEINFO = SHELLEXECUTEINFO.CreateInstance()
		Dim bSuccess As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			bWaitUntilFinished = True
			
			' Initialize required variables.
			With lpExecInfo

				.cbSize = Marshal.SizeOf(lpExecInfo)
				.fMask = SEE_MASK_NOCLOSEPROCESS Or (IIf(bWaitUntilFinished, SEE_MASK_FLAG_DDEWAIT, 0))
				.lpVerb = Nothing
				.lpFile = sFileSpec
				.lpParameters = sParameters
				.lpDirectory = ""
				.nShow = ProcessWindowStyle.Normal
			End With
			
			' Open the document.
			bSuccess = ShellExecuteEx(lpExecInfo)
			
			If bSuccess = 0 Then
				Return result
			End If
			
			bSuccess = WaitForSingleObject(lpExecInfo.hProcess, INFINITE)
			
			' Close the process handle.
			bSuccess = CloseHandle(lpExecInfo.hProcess)
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OpenDocument failed", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
End Module
