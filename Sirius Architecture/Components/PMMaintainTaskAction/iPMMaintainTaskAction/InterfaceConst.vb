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
	Public Const ACResDataInterfaceButtonAdd As Integer = 105
	Public Const ACResDataInterfaceButtonEdit As Integer = 106
	Public Const ACResDataInterfaceButtonDelete As Integer = 107
	Public Const ACResDataInterfaceButtonView As Integer = 108
	Public Const ACResDataInterfaceButtonUndelete As Integer = 122
	
	' list view headers
	Public Const ACResDataInterfaceListViewHeaderCode As Integer = 109
	Public Const ACResDataInterfaceListViewHeaderDescription As Integer = 110
	Public Const ACResDataInterfaceListViewHeaderTemplate As Integer = 111
	Public Const ACResDataInterfaceListViewHeaderEffectiveDate As Integer = 112
	
	'*********************************
	' frmdetails constants
	'*********************************
	' form title
	Public Const ACResDataDetailsTitle As Integer = 120
	
	' tab title
	Public Const ACResDataDetailsTabActionType As Integer = 121
	
	' frmdetails labels
	Public Const ACResDataDetailsLabelDescription As Integer = 113
	Public Const ACResDataDetailsLabelEffectiveDate As Integer = 114
	Public Const ACResDataDetailsLabelDueDays As Integer = 115
	Public Const ACResDataDetailsLabelTemplate As Integer = 116
	Public Const ACResDataDetailsLabelOutcomeEditable As Integer = 117
	
	' frmdetails buttons captions
	Public Const ACResDataDetailsButtonOutcomes As Integer = 118
	Public Const ACResDataDetailsButtonOK As Integer = 119
	Public Const ACResDataDetailsButtonCancel As Integer = 123
	Public Const ACResDataDetailsButtonClear As Integer = 127
	
	Public Const ACResDataDetailsMessageInvalidDate As Integer = 124
	Public Const ACResDataDetailsMessageInvalidDueDays As Integer = 125
	Public Const ACResDataDetailsMessageInvalidCode As Integer = 126
	
	'*********************************
	' frmoutcomes constants
	'*********************************
	' form title
	Public Const ACResDataOutcomesTitle As Integer = 128
	
	' tab title
	Public Const ACResDataOutcomesTabOutcomes As Integer = 129
	
	' buttons
	Public Const ACResDataOutcomesButtonCancel As Integer = 130
	Public Const ACResDataOutcomesButtonOK As Integer = 131
	
	
	
	'********************************************************************
	'********************************************************************
	'********************************************************************
	
	Public Const ACListViewColIndexCode As Integer = 1
	Public Const ACListViewColIndexDescription As Integer = 2
	Public Const ACListViewColIndexTemplate As Integer = 3
	Public Const ACListViewColIndexEffectiveDate As Integer = 4
	
	Public Const ACListViewSubItemIndexDescription As Integer = 1
	Public Const ACListViewSubItemIndexTemplate As Integer = 2
	Public Const ACListViewSubItemIndexEffectiveDate As Integer = 3
	
	Public Const ACListViewColKeyCode As String = "Code"
	Public Const ACListViewColKeyDescription As String = "Description"
	Public Const ACListViewColKeyTemplate As String = "Template"
	Public Const ACListViewColKeyEffectiveDate As String = "EffectiveDate"
	
	
	' List View Column Tag Types
	Public Const ACListViewTagTypeString As String = "String"
	Public Const ACListViewTagTypeNumber As String = "Number"
	Public Const ACListViewTagTypeImage As String = "Image"
	Public Const ACListViewTagTypeDate As String = "Date"
	
	Public Const ACDataPosId As Integer = 0
	Public Const ACDataPosCode As Integer = 1
	Public Const ACDataPosDescription As Integer = 2
	Public Const ACDataPosIsDeleted As Integer = 3
	Public Const ACDataPosEffectiveDate As Integer = 4
	Public Const ACDataPosDueDays As Integer = 5
	Public Const ACDataPosDocumentTemplateCode As Integer = 6
	Public Const ACDataPosOutcomeNotEditable As Integer = 7
	
	' Actions
	Public Const ACActionView As Integer = 0
	Public Const ACActionEdit As Integer = 1
	Public Const ACActionAdd As Integer = 2
	
	
	' Document Template Array Fields
	Public Const ACDocTemplateId As Integer = 0
	Public Const ACDocTemplateDescription As Integer = 1
	Public Const ACDocTemplateCode As Integer = 2
	
	' array items
	Public Const ACDetailKey As Integer = 0
	Public Const ACDetailDesc As Integer = 1
	Public Const ACDetailCode As Integer = 2
	
	' Return Constants
	Public Const ACReturnOk As Integer = 1
	Public Const ACReturnCancel As Integer = 2
End Module