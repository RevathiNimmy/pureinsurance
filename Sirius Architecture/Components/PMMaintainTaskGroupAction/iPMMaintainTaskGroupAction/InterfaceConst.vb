Option Strict Off
Option Explicit On
Imports System
Module InterfaceConst
	
	'********************************************************************
	'******** Resource Data *********************************************
	'********************************************************************
	
	'*********************************
	' frminterface constants
	'*********************************
	
	' form title
	Public Const ACResDataInterfaceTitle As Integer = 100
	
	' tab title
	Public Const ACResDataInterfaceTabActionTypes As Integer = 101
	
	' buttons captions
	Public Const ACResDataInterfaceButtonOK As Integer = 102
	Public Const ACResDataInterfaceButtonCancel As Integer = 103
	Public Const ACResDataInterfaceButtonApply As Integer = 104
	Public Const ACResDataInterfaceButtonActions As Integer = 105
	
	' list view headers
	Public Const ACResDataInterfaceListViewHeaderTaskGroup As Integer = 106
	Public Const ACResDataInterfaceListViewHeaderTask As Integer = 107
	Public Const ACResDataInterfaceListViewHeaderDescription As Integer = 108
	
	'*********************************
	' frmdetails constants
	'*********************************
	' form title
	Public Const ACResDataDetailsTitle As Integer = 109
	
	' tab title
	Public Const ACResDataDetailsTabActions As Integer = 110
	
	' frmdetails buttons captions
	Public Const ACResDataDetailsButtonOK As Integer = 111
	Public Const ACResDataDetailsButtonCancel As Integer = 112
	
	'********************************************************************
	'********************************************************************
	'********************************************************************
	
	Public Const ACListViewColIndexTaskGroupCode As Integer = 1
	Public Const ACListViewColIndexTaskCode As Integer = 2
	Public Const ACListViewColIndexDescription As Integer = 3
	
	Public Const ACListViewSubItemIndexTaskCode As Integer = 1
	Public Const ACListViewSubItemIndexTaskDescription As Integer = 2
	
	Public Const ACListViewColKeyTaskGroupCode As String = "TaskGroupCode"
	Public Const ACListViewColKeyTaskCode As String = "TaskCode"
	Public Const ACListViewColKeyTaskDescription As String = "Description"
	
	' List View Column Tag Types
	Public Const ACListViewTagTypeString As String = "String"
	Public Const ACListViewTagTypeNumber As String = "Number"
	Public Const ACListViewTagTypeImage As String = "Image"
	Public Const ACListViewTagTypeDate As String = "Date"
	
	' Maintain Data Array Positions
	Public Const ACMaintainDataTaskId As Integer = 0
	Public Const ACMaintainDataTaskGroupId As Integer = 1
	Public Const ACMaintainDataTaskGroupCode As Integer = 2
	Public Const ACMaintainDataTaskCode As Integer = 3
	Public Const ACMaintainDataTaskDescription As Integer = 4
	
	' Actions
	Public Const ACActionView As Integer = 0
	Public Const ACActionEdit As Integer = 1
	Public Const ACActionAdd As Integer = 2
	
	' array items
	Public Const ACDetailKey As Integer = 0
	Public Const ACDetailDesc As Integer = 1
	Public Const ACDetailCode As Integer = 2
	
	' Return Constants
	Public Const ACReturnOk As Integer = 1
	Public Const ACReturnCancel As Integer = 2
End Module