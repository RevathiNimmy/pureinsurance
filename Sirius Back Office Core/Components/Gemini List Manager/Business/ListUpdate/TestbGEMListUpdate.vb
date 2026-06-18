Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Module TestRun
	
	'RLDF Detail record
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Structure RLDFDetailRecord
		<VBFixedString(10),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=10)> _
		Public PropertyId As FixedLengthString
		<VBFixedString(70),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=70)> _
		Public Description As FixedLengthString
		<VBFixedString(10),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=10)> _
		Public ABICode As FixedLengthString
		Public Shared Function CreateInstance() As RLDFDetailRecord
			Dim result As New RLDFDetailRecord
			result.PropertyId = New FixedLengthString(10)
			result.Description = New FixedLengthString(70)
			result.ABICode = New FixedLengthString(10)
			Return result
		End Function
	End Structure
	
	'RLDF Index record
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Structure RLDFIndexRecord
		<VBFixedString(10),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=10)> _
		Public PropertyId As FixedLengthString
		Dim RecordNumber As Integer
		Public Shared Function CreateInstance() As RLDFIndexRecord
			Dim result As New RLDFIndexRecord
			result.PropertyId = New FixedLengthString(10)
			Return result
		End Function
	End Structure
	
	
	Public Sub Main()
		
		Test()
		
	End Sub
	Sub Test()
		Dim PMTrue As Object
		
		
		'frmTest.Show vbModal
		
		
		Dim oPolicy As New bGEMListUpdate.Form
		

		Dim lReturn As Integer = CInt(CType(oPolicy, SSP.S4I.Interfaces.IBusiness).Initialise(sUsername:="basilb", sPassword:="basilb", iUserID:=7, iSourceID:=1, iLanguageID:=1, iCurrencyID:=1, iLogLevel:=6, sCallingAppName:="Test"))
		

		If lReturn <> CDbl(PMTrue) Then
			MessageBox.Show(CStr(lReturn), Application.ProductName)
			Exit Sub
		End If
		
		

		lReturn = CInt(oPolicy.ListUpdateProcess())
		

		If lReturn <> CDbl(PMTrue) Then
			MessageBox.Show(CStr(lReturn), Application.ProductName)
			Exit Sub
		End If
		

        CInt(oPolicy.Dispose())
		
		
	End Sub
End Module