Option Strict Off
Option Explicit On
Imports System
Module MainModule
	
	Public Const ACApp As String = "iPMBPartyDuplicate"
	
	Public Const ACClientCode As Integer = 0
	Public Const ACClientName As Integer = 1
	Public Const ACClientAddress1 As Integer = 2
	Public Const ACClientAddress2 As Integer = 3
	Public Const ACClientPostcode As Integer = 4
	Public Const ACClientPartyType As Integer = 5
	Public Const ACClientBranch As Integer = 6
	Public Const ACClientPartyCnt As Integer = 7
	Public Const ACClientPartyTypeCode As Integer = 8
	
	
	Public Const kAbandonNewRecordandUseSelectedClient As Integer = 0
	Public Const kCreateUniqueCode As Integer = 1
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
End Module