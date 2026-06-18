Option Strict Off
Option Explicit On
Module pbObjectAndPropertyConsts
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	Private Const ACClass As String = ""
	
	'Array constants for the GIS Object
	Public Const ACOGISObjectId As Short = 0
	Public Const ACOGISDataModelId As Short = 1
	Public Const ACOObjectName As Short = 2
	Public Const ACOTableName As Short = 3
	Public Const ACOMaxInstances As Short = 4
	Public Const ACOIsQuoteObject As Short = 5
	Public Const ACOParentObjectId As Short = 6
	Public Const ACOPolarisObjectId As Short = 7
	Public Const ACOIsSelectableForScreen As Short = 8
	Public Const ACOIsNonGIS As Short = 9
	Public Const ACOEditFlags As Short = 10
	Public Const ACOLastElement As Short = 10
	
	'Array constants for the GIS Property
	Public Const ACPGISPropertyId As Short = 0
	Public Const ACPGISObjectId As Short = 1
	Public Const ACPPropertyName As Short = 2
	Public Const ACPColumnName As Short = 3
	Public Const ACPDataType As Short = 4
	Public Const ACPIsInputProperty As Short = 5
	Public Const ACPIsIdentifyingProperty As Short = 6
	Public Const ACPIsPrimaryKey As Short = 7
	Public Const ACPPolarisPropertyId As Short = 8
	Public Const ACPIsDeleted As Short = 9
	Public Const ACPIsSearchProperty As Short = 10
	Public Const ACPIndexLinkingId As Short = 11
	Public Const ACPEditFlags As Short = 12
	Public Const ACPSpecialsType As Short = 13
	Public Const ACPSpecialsTypeReference As Short = 14
	Public Const ACPIsInMISExport As Short = 15
	Public Const ACPLastElement As Short = 15
End Module