Option Strict Off
Option Explicit On
Imports System
Module MainModule
	
	' Name of this application
	Public Const ACApp As String = "iACTCashReceipt"
	
	' Name of this class
	Private Const ACClass As String = "MainModule"
	
	Public Const ACCashList As String = "iACTCashList.Interface"
	Public Const ACCashListItem As String = "iACTCashListItem.Interface"
	Public Const ACCreateBatch As String = "bACTBatchWrapper.NavigatorV3"
	Public Const ACFindTransaction As String = "iACTFindTransaction.Interface"
	Public Const ACCreateAllocation As String = "bACTAllocationCreate.AutoForCL"
	Public Const ACAllocation As String = "iACTAllocation.Interface"
	Public Const ACPostAllocation As String = "bACTAllocationPost.NavigatorV3"
	Public Const ACPostCashList As String = "bACTCashListPost.Automated"
	
	Public g_iCurrencyID As Integer
	Public g_iSourceID As Integer
End Module