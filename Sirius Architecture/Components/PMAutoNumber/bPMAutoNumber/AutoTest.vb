Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
Module Module1
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	
	Public Sub Main()
		
		test()
		
	End Sub
	
	Sub test()
		Dim bPMAutoNumber As Object
		Dim pmeRefTypeNBMakeLive, pmeRefTypeNBQuotation As String
		
		Dim NumRet As Integer
		Dim StrRet As String = ""
		
		'Set oGenerateNumber = CreateObject("bPMAutoNumber.Business")

		Dim oGenerateNumber As bPMAutoNumber.Business = New bPMAutoNumber.Business()
		

		Dim lError As Integer = CType(oGenerateNumber, SSP.S4I.Interfaces.IBusiness).Initialise("jimw", "jimw", 1, 1, 1, 1, 1, "Test")
		
		'    lError& = oGenerateNumber.GenerateInsFileReference(pmeRefTypeNBQuotation, # _
		''        1/10/97#, # _
		''        10/10/97#, _
		''        Date, _
		''        1, 1, 1, 1, StrRet$)
		
		Dim dt1 As Date = #1/1/97#
		Dim dt2 As Date = #10/10/97#
		
		MessageBox.Show("MakeLiveID: " & pmeRefTypeNBMakeLive, Application.ProductName)
		MessageBox.Show("QuotationID: " & pmeRefTypeNBQuotation, Application.ProductName)
		

		lError = oGenerateNumber.GenerateInsFileReference(pmeRefTypeNBMakeLive, dt1, dt2, DateTime.Now, 1, 1, 1, 1, StrRet)
		
		If lError = PMTrue Then
			MessageBox.Show("String returned:-     " & StrRet & "     -:Len: " & CStr(StrRet.Length), Application.ProductName)
		End If
		

		oGenerateNumber.Dispose()
		
		
	End Sub
	
	Sub test2()
		Dim iSIRClaimsServices As Object
		
		
		Dim proto As Object = New iSIRClaimsServices.interface()
		

		Dim ret As Integer = proto.initialise
		

		ret = proto.start
		
	End Sub
End Module