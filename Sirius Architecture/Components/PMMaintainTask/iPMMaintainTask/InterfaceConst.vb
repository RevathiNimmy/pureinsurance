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
	Public Const ACResDataInterfaceButtonOK As Integer = 106
	Public Const ACResDataInterfaceButtonCancel As Integer = 107
	Public Const ACResDataInterfaceButtonEdit As Integer = 108
	Public Const ACResDataInterfaceButtonApply As Integer = 119
	
	' list view headers
	Public Const ACResDataInterfaceListViewHeaderCode As Integer = 102
	Public Const ACResDataInterfaceListViewHeaderDescription As Integer = 103
	Public Const ACResDataInterfaceListViewHeaderCompletionOutcome As Integer = 104
	Public Const ACResDataInterfaceListViewHeaderIncompleteOutcome As Integer = 105
	
	'*********************************
	' frmdetails constants
	'*********************************
	' form title
	Public Const ACResDataDetailsTitle As Integer = 109
	
	' tab title
	Public Const ACResDataDetailsTabActionType As Integer = 110
	
	' frmae caption
	Public Const ACResDataDetailsFrameDefaults As Integer = 113
	
	' frmdetails labels
	Public Const ACResDataDetailsLabelCode As Integer = 111
	Public Const ACResDataDetailsLabelDescription As Integer = 112
	Public Const ACResDataDetailsLabelCompletionTask As Integer = 114
	Public Const ACResDataDetailsLabelIncompleteTask As Integer = 115
	Public Const ACResDataDetailsLabelActionRequired As Integer = 116
	
	' frmdetails buttons captions
	Public Const ACResDataDetailsButtonOK As Integer = 117
	Public Const ACResDataDetailsButtonCancel As Integer = 118
	
	'********************************************************************
	'********************************************************************
	'********************************************************************
	
	Public Const ACListViewColIndexCode As Integer = 1
	Public Const ACListViewColIndexDescription As Integer = 2
	Public Const ACListViewColIndexCompletionTask As Integer = 3
	Public Const ACListViewColIndexIncompleteTask As Integer = 4
	
	Public Const ACListViewSubItemIndexDescription As Integer = 1
	Public Const ACListViewSubItemIndexCompletionTask As Integer = 2
	Public Const ACListViewSubItemIndexIncompleteTask As Integer = 3
	
	Public Const ACListViewColKeyCode As String = "Code"
	Public Const ACListViewColKeyDescription As String = "Description"
	Public Const ACListViewColKeyCompletionTask As String = "CompletionTask"
	Public Const ACListViewColKeyIncompleteTask As String = "IncompleteTask"
	
	
	' List View Column Tag Types
	Public Const ACListViewTagTypeString As String = "String"
	Public Const ACListViewTagTypeNumber As String = "Number"
	Public Const ACListViewTagTypeImage As String = "Image"
	Public Const ACListViewTagTypeDate As String = "Date"
	
	' Maintain Data Array Positions
	Public Const ACMaintainDataPosId As Integer = 0
	Public Const ACMaintainDataPosCode As Integer = 1
	Public Const ACMaintainDataPosDescription As Integer = 2
	Public Const ACMaintainDataPosCompletionTaskId As Integer = 3
	Public Const ACMaintainDataPosIncompleteTaskId As Integer = 4
	Public Const ACMaintainDataPosActionRequired As Integer = 5
	Public Const ACMaintainDataPosCompletionCode As Integer = 6
	Public Const ACMaintainDataPosCompletionDescription As Integer = 7
	Public Const ACMaintainDataPosIncompleteCode As Integer = 8
	Public Const ACMaintainDataPosIncompleteDescription As Integer = 9
	Public Const ACMaintainDataPosAutoUpdateBatch As Integer = 10
	
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
	
	' Task Outcome DAta Array Positions
	Public Const ACTaskOutcomePosId As Integer = 0
	Public Const ACTaskOutcomePosDescription As Integer = 1
	Public Const ACTaskOutcomePosCode As Integer = 2
End Module