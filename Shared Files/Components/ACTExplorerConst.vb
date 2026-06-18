Option Strict Off
Option Explicit On
Imports System
<System.Runtime.InteropServices.ProgId("ACTExplorerConst_NET.ACTExplorerConst")> _
 Public Module ACTExplorerConst
	' ***************************************************************** '
	' Module Name: ACTExplorerConst
	'
	' Date: 09 August 1997
	'
	' Description: Constants used to communicate with ACTExplorer
	'               interface and business components
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Error numbers returned by business methods
	Public Const ACExpErrFirst As Integer = 0
	Public Const ACExpErrOK As Integer = 0
	Public Const ACExpErrGeneralFailure As Integer = 1
	Public Const ACExpErrDuplicateElement As Integer = 2 ' UpdateElement
	Public Const ACExpErrMultipleElementInPath As Integer = 3 ' InsertNode
	Public Const ACExpErrElementInStructure As Integer = 4 ' DeleteElement
	Public Const ACExpErrElementInElement As Integer = 5 ' DeleteElement
	Public Const ACExpErrNodeHasAccounts As Integer = 6
	Public Const ACExpErrLast As Integer = 6
	
	' Constants for accessing columns of result array
	' returned by GetNode, GetChildrenOfNode
	Public Const ACGetNodeNodeID As Integer = 0
	Public Const ACGetNodeElementID As Integer = 1
	Public Const ACGetNodeElementName As Integer = 2
	Public Const ACGetNodeAccountID As Integer = 3
	Public Const ACGetNodeAccountName As Integer = 4
	Public Const ACGetNodeAccountType As Integer = 5
	Public Const ACGetNodeShortCode As Integer = 6
	Public Const ACGetNodeMapID As Integer = 7
	Public Const ACGetNodeMapName As Integer = 8
	Public Const ACGetNodeCompanyID As Integer = 9
	Public Const ACGetNodeCompanyName As Integer = 10
	Public Const ACGetNodeSubBranchID As Integer = 11
	Public Const ACGetNodeSubBranchName As Integer = 12
	
	' Constants for accessing columns of result array
	' returned by GetElementRelationships
	Public Const ACGetRelParentID As Integer = 0
	Public Const ACGetRelParentName As Integer = 1
	Public Const ACGetRelElementID As Integer = 2
	Public Const ACGetRelElementName As Integer = 3
	
	' Constants for accessing columns of result array
	' returned by GetStructureTree
	Public Const ACGetStructureTreeNodeID As Integer = 0
	Public Const ACGetStructureTreeMappingID As Integer = 1
	Public Const ACGetStructureTreeAccountID As Integer = 2
	Public Const ACGetStructureTreeElementID As Integer = 3
	Public Const ACGetStructureTreeParentNodeID As Integer = 4
	
	Public Const ACDefaultMapType As Integer = 1
	Public Const ACDefaultCompanyId As Integer = 1
End Module