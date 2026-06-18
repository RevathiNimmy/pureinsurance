Option Strict Off
Option Explicit On
Imports System
Imports SharedFiles
Module MainModule
	
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	'constants
	Public Const ACAPP As String = "PBSearchFields"
	Public Const ACClass As String = "Product Builder"

	Public PMProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
	
	'control type
	Public Const ACText As Integer = 1
	Public Const ACCombo As Integer = 1
	
	' mapping data array positions
	Public Const kMappingFindControlId As Integer = 0
	Public Const kMappingControlIndex As Integer = 1
	Public Const kMappingViewFieldName As Integer = 2
	Public Const kMappingControlType As Integer = 3
	Public Const kMappingFuzzy As Integer = 4
	Public Const kMappingViewName As Integer = 5
	Public Const kMappingSearchValue As Integer = 6
	Public Const kMappingFoundValue As Integer = 7
	Public Const kMappingGisObjectId As Integer = 8
	Public Const kMappingGisPropertyId As Integer = 9
	Public Const kMappingObjectName As Integer = 10
	Public Const kMappingPropertyName As Integer = 11
	Public Const kMappingGridCaption As Integer = 12
	Public Const kMappingGridPosition As Integer = 13
	Public Const kMappingGridWidth As Integer = 14
	
	Public Const kTDBGridCol_LOWER As Integer = 0
	Public Const kTDBGridCol_GISPropertyId As Integer = 0
	Public Const kTDBGridCol_PropertName As Integer = 1
	Public Const kTDBGridCol_Value As Integer = 2
	Public Const kTDBGridCol_GISObjectId As Integer = 3
	Public Const kTDBGridCol_DataType As Integer = 4
	Public Const kTDBGridCol_SpecialType As Integer = 5
	Public Const kTDBGridCol_SpecialTypeRef As Integer = 6
	Public Const kTDBGridCol_UPPER As Integer = kTDBGridCol_SpecialTypeRef
	
	Public Const ACGISPropertyId As Integer = 0
	Public Const ACPropertName As Integer = 1
	'Public Const ACValue = 2
	Public Const ACGISObjectId As Integer = 2
	Public Const ACDataType As Integer = 3
	Public Const ACSpecialType As Integer = 4
	Public Const ACSpecialTypeRef As Integer = 5
	
	Public Const ACProperty As Integer = 1
	Public Const ACControl As Integer = 2
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sUsername As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
End Module