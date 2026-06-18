Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles
Module MainModule
	
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date:  20th February 2007
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bSIRFindRIParty"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Username.
	Public g_sUsername As New FixedLengthString(12)
	
	' Password.
	Public g_sPassword As New FixedLengthString(30)
	
    ' User ID
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iUserID As Integer
	
    ' Calling Application
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sCallingAppName As String = ""
	
    ' Source ID
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
	
    ' Language ID
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
	
    ' Log Level
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLogLevel As Integer
	
    ' Currency ID
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iCurrencyID As Integer
	
	'QBENZ005
	Public Const ACIRI2007ShortName As Integer = 0
	Public Const ACIRI2007LongName As Integer = 1
	Public Const ACIRI2007AccType As Integer = 2
	Public Const ACIRI2007Participation_percent As Integer = 3
	Public Const ACIRI2007SumInsured As Integer = 4
	Public Const ACIRI2007Premium As Integer = 5
	Public Const ACIRI2007Tax As Integer = 6
	Public Const ACIRI2007Comm_percent As Integer = 7
	Public Const ACIRI2007Commission As Integer = 8
	Public Const ACIRI2007CommTax As Integer = 9
	Public Const ACIRI2007AgreementCode As Integer = 10
	Public Const ACIRI2007PartyCnt As Integer = 11
	Public Const ACIRI2007RIArrangementLineId As Integer = 12
	
	Public Const ACIBrokerShortName As Integer = 0
	Public Const ACIBrokerLongName As Integer = 1
	Public Const ACIBrokerParticipant_percent As Integer = 2
	Public Const ACIBrokerAssociationPartyCnt As Integer = 3
	Public Const ACIBrokerPartyCnt As Integer = 4
	

	Public Sub Main()
		
		' Main entry point for the component
		
		'Call TEST
		
	End Sub
	
	Sub TEST()
		
		'SD 01/08/2002 Scalability changes
		Dim oFindParty As bSIRFindRIParty.Business
        Dim vResultArray(,) As Object

		Dim lerror As gPMConstants.PMEReturnCode = CType(CType(oFindParty, SSP.S4I.Interfaces.IBusiness).Initialise(sUsername:="philj", sPassword:="philj", iUserID:=1, iSourceID:=1, iLanguageID:=1, iCurrencyID:=1, iLogLevel:=6, sCallingAppName:="Test"), gPMConstants.PMEReturnCode)
		Dim sPartyName As String = "H"
		
		If lerror <> gPMConstants.PMEReturnCode.PMTrue Then
			MessageBox.Show("Error  in initialisation", Application.ProductName)
			Exit Sub
		End If
		
		Dim lNumberOfRecords As Integer = 999
		
		If Information.IsArray(vResultArray) Then

			For iCount As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

				For iCount1 As Integer = vResultArray.GetLowerBound(0) To vResultArray.GetUpperBound(0)

                    Debug.WriteLine(CStr(vResultArray(iCount1, iCount)) & " ")
				Next iCount1
			Next iCount
		End If
		
		
	End Sub
    Sub New()
        Main()
    End Sub
	Sub JustForInvokeMain()
	End Sub
End Module
