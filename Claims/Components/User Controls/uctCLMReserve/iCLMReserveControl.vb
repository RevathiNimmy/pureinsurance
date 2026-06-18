Option Strict Off
Option Explicit On
Imports System
Imports System.Globalization
'developer guide no 129. 
Imports SharedFiles

Module MainModule
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	Public Const ACReserveFrame As Integer = 125
	Public Const ACEditButton As Integer = 205
	
	'Constants used for resizing & positioning the user control
	Public Const ACListViewTop As Integer = 240
	Public Const ACCtrlVerticalSpacing As Integer = 120
	Public Const ACCommandButtonWidth As Integer = 1095
	Public Const ACCommandButtonHeight As Integer = 330
	
	Public Const ACApp As String = "uctCLMReserveControl"
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	Public g_iUserId As Integer
	
    'Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As Object
	
    ' Public array to keep track of the reserve types that are to be included in the total.
    'developer guide no. 107
    <ThreadStatic()> _
 Public v_vReserveTotalArray As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_vReserveDetails As Object 'to keep track of original values
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_vPaymentDetails As Object
	
	' Public constants declared for the fields in the ReserveType Array
	Public Const g_cIRTADescription As Integer = 0
	
	' Public constants declared for the Reserve Details Array
	Public Const g_cIRDAreserveid As Integer = 0
	Public Const g_cIRDAinitialreserve As Integer = 1
	Public Const g_cIRDApaidtodate As Integer = 2
	Public Const g_cIRDArevisedreserve As Integer = 3
	Public Const g_cIRDAsuminsured As Integer = 4
	Public Const g_cIRDAaverage As Integer = 5
	Public Const g_cIRDArevisioncount As Integer = 6
	Public Const g_cIRDAreservetype As Integer = 7
    Public Const g_cIRDArevisedentered As Integer = 8
    Public Const g_cIRDAlastversionrevisedreserve As Integer = 9

    Public Const g_cIRDAThisPayment As Integer = 9
	Public Const g_cIRDAThisPaymentTax As Integer = 10
	Public Const g_cIRDATaxPaidToDate As Integer = 11
	Public Const g_cIRDAInitialtax As Integer = 12
	Public Const g_cIRDATaxRevision As Integer = 13
	Public Const g_cIRDAThisReserveTax As Integer = 14
	
	' Public constants declared for the Recovery Details
	Public Const g_cIRecoveryDAinitialreserve As Integer = 0
	Public Const g_cIRecoveryDArevisedreserve As Integer = 1
	Public Const g_cIRecoveryDApaidtodate As Integer = 2
	
	' Public constants declared for the Payment Details Array
	Public Const g_cIPDApaymentid As Integer = 0
	Public Const g_cIPDAamount As Integer = 1
	
	' Invalid Data
	Public Const ACInvalidDataTitle As Integer = 310
	Public Const ACInvalidIntegerData As Integer = 311
	Public Const ACInvalidDateData As Integer = 314
	Public Const ACInvalidReserveDataTitle As Integer = 315
	Public Const ACInvalidReserveData As Integer = 316
	
	
	
	
	Public Function IsValidCurrency(ByRef cValue As String) As Integer
		' Function to test value supplied is a valid currency value
		Dim result As Integer = 0
		Try 
			Dim curTemp As Decimal
			
			' set the default return value
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' perform basic checks
			If cValue.Length < 1 Then Return result
			Dim dbNumericTemp As Double
			If Not Double.TryParse(cValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then Return result
			
			' test by converting value to a currency
			curTemp = CDec(cValue)
			
			' worked fine
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch 
			
			
			
			Return result
		End Try
	End Function
End Module