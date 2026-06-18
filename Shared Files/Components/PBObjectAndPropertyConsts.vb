Option Strict Off
Option Explicit On
Imports System
<System.Runtime.InteropServices.ProgId("pbObjectAndPropertyConsts_NET.pbObjectAndPropertyConsts")> _
 Public Module pbObjectAndPropertyConsts
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	Private Const ACClass As String = ""
	
	'Array constants for the GIS Object
	Public Const ACOGISObjectId As Integer = 0
	Public Const ACOGISDataModelId As Integer = 1
	Public Const ACOObjectName As Integer = 2
	Public Const ACOTableName As Integer = 3
	Public Const ACOMaxInstances As Integer = 4
	Public Const ACOIsQuoteObject As Integer = 5
	Public Const ACOParentObjectId As Integer = 6
	Public Const ACOPolarisObjectId As Integer = 7
	Public Const ACOIsSelectableForScreen As Integer = 8
	Public Const ACOIsNonGIS As Integer = 9
	Public Const ACOEditFlags As Integer = 10
	Public Const ACOLastElement As Integer = 10
	
	'Array constants for the GIS Property
	Public Const ACPGISPropertyId As Integer = 0
	Public Const ACPGISObjectId As Integer = 1
	Public Const ACPPropertyName As Integer = 2
	Public Const ACPColumnName As Integer = 3
	Public Const ACPDataType As Integer = 4
	Public Const ACPIsInputProperty As Integer = 5
	Public Const ACPIsIdentifyingProperty As Integer = 6
	Public Const ACPIsPrimaryKey As Integer = 7
	Public Const ACPPolarisPropertyId As Integer = 8
	Public Const ACPIsDeleted As Integer = 9
	Public Const ACPIsSearchProperty As Integer = 10
	Public Const ACPIndexLinkingId As Integer = 11
	Public Const ACPEditFlags As Integer = 12
	Public Const ACPSpecialsType As Integer = 13
	Public Const ACPSpecialsTypeReference As Integer = 14
    Public Const ACPIsInMISExport As Integer = 15
    Public Const ACPIsFormattedText As Integer = 16
    Public Const ACPIsChaseCycleProperty As Integer = 17
    Public Const ACPISClaim360Display As Integer = 18
    Public Const ACPLastElement As Integer = 18

End Module