Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.IO
Module Test
	
	Public Sub Main()
		Dim bSIRPFInstalments As Object

		Dim oPFI As bSIRPFInstalments.Business
		Dim nBatch, nRecords As Integer
		Dim oOM As New bObjectManager.ObjectManager
		Dim vArray As Object
		
		oOM.Initialise("TestBed")

		

		oPFI.pfprem_finance_cnt = 1

		oPFI.pfprem_finance_version = 1

		oPFI.CreateInstalments(12, 40, 10, 50, 5, #11/1/2001#, "m", 1)
		
		'oPFI.pfprem_finance_cnt = 3
		'oPFI.pfprem_finance_version = 1
		'oPFI.CreateInstalments 12, 80, 10, 120, 15, #12/1/2001#, "m", 1
		
		'oPFI.pfprem_finance_cnt = 3
		'oPFI.pfprem_finance_version = 1
		'oPFI.TransferInstalments 4, 1, True
		

		oPFI.Dispose()
		oOM.Dispose()
	End Sub
End Module