Option Strict Off
Option Explicit On
Imports System
'refer Developer Guide No. 129
Imports SSP.Shared
Module G2V2Func
	
	Private Const ACClass As String = "G2V2Func"
	
	Public Const ACDataSetSuffix As String = "DS"
	Public Const ACDataSetDefSuffix As String = "DSD"
	Public Const ACXMLFileExtension As String = ".XML"
	Public Const ACXSLFileExtension As String = ".XSL"
	Public Const GISRegDataSetPath As String = "DataSetsPath"
	Public Const GISRegSubKey As String = "GIS"
	Public Const ACOIMGISSubKey As String = "GIS"
	
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	' ***************************************************************** '
	' Name: GetTopLevelObjectName
	'
	' Description:
	'
	' History: 12/07/2004 SJ - Created.
	'
	' ***************************************************************** '
	Public Function GetTopLevelObjectName(ByRef r_sErrorMessage As String, Optional ByRef r_sTopLevelTableName As String = "", Optional ByRef r_sTopLevelObjectName As String = "", Optional ByVal v_sGisDataModelCode As String = "", Optional ByVal v_sGisBusinessTypeCode As String = "") As Integer
		
		Dim result As Integer = 0
		Try 
			
            Dim sDSDfilename As String = String.Empty
            Dim sDSfilename As String = String.Empty
			Dim oDataSet As cGISDataSetControl.Application
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			r_sErrorMessage = ""
			
			If v_sGisDataModelCode = "" And v_sGisBusinessTypeCode = "" Then
				r_sErrorMessage = "Either Gis Data Model Code or Gis Business Type must be supplied"
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			If v_sGisDataModelCode = "" Then
				'If no data model code supplied then work it out from the business
				Select Case v_sGisBusinessTypeCode.Trim().ToUpper()
					Case "GIIM"
						v_sGisDataModelCode = "GIIMotor"
					Case "GIIH"
						v_sGisDataModelCode = "GIIHouse"
					Case "GIIT"
						v_sGisDataModelCode = "GIITruck"
				End Select
			End If
			
			'Get the dataset and dataset definition file names from the registry
			m_lReturn = CType(GetDataSetFileNames(v_sDataModelCode:=v_sGisDataModelCode, r_sDataSetDefFile:=sDSDfilename, r_sDataSetFile:=sDSfilename), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				r_sErrorMessage = "GetDataSetFileNames Failed"
				Return gPMConstants.PMEReturnCode.PMFalse
			End If

			oDataSet = New cGISDataSetControl.Application()


			'Load up the dataset control from the files
			m_lReturn = oDataSet.LoadFromXMLFile(v_sDataSetDefFile:=sDSDfilename, v_sDataSetFile:=sDSfilename)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				r_sErrorMessage = "oDataSet.LoadFromXMLFile Failed"
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			'Get the top level object and table names
			m_lReturn = oDataSet.GetTopLevelRiskObject(r_sObjectName:=r_sTopLevelObjectName, r_sTableName:=r_sTopLevelTableName)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				r_sErrorMessage = "oDataSet.GetTopLevelRiskObject Failed"
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			r_sTopLevelObjectName = r_sTopLevelObjectName.Trim()
			r_sTopLevelTableName = r_sTopLevelTableName.Trim()
			
			oDataSet = Nothing
			
			Return result
		
		Catch 
			
			
			
			result = gPMConstants.PMEReturnCode.PMFalse
			r_sErrorMessage = "GetTopLevelObjectName Failed"
			
			Return result
		End Try
		
	End Function
	
	' ***************************************************************** '
	' Name: GetDataSetFileNames
	'
	' Description: Calculates the Location and File Name of the XML
	'              files for a given Data Model.
	'
	' ***************************************************************** '
	Public Function GetDataSetFileNames(ByVal v_sDataModelCode As String, ByRef r_sDataSetDefFile As String, ByRef r_sDataSetFile As String) As Integer
		
		Dim result As Integer = 0
		Dim sPath As String = ""
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get the Path where they live
			lReturn = CType(GetDataSetsPath(v_sDataModelCode:=v_sDataModelCode, r_sDataSetsPath:=sPath), gPMConstants.PMEReturnCode)
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			r_sDataSetDefFile = sPath & v_sDataModelCode.Trim().ToUpper() & ACDataSetDefSuffix & ACXMLFileExtension
			
			r_sDataSetFile = sPath & v_sDataModelCode.Trim().ToUpper() & ACDataSetSuffix & ACXMLFileExtension
			
			Return result
		
		Catch 
			
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	' ***************************************************************** '
	' Name: GetDataSetsPath
	'
	' Description: Return the Path for GIS Data Sets storage/retrieval.
	'
	' ***************************************************************** '
	Public Function GetDataSetsPath(ByVal v_sDataModelCode As String, ByRef r_sDataSetsPath As String) As Integer
		
		Dim result As Integer = 0
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' RFC110400 - Get the Data Model Specific Data Set Path
			lReturn = CType(GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sDataModelCode, v_sSettingName:=GISRegDataSetPath, r_sSettingValue:=r_sDataSetsPath, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon), gPMConstants.PMEReturnCode)
			
			' RFC110400 - If no Data Model Specific Data Set Path Found
			If r_sDataSetsPath.Trim() = "" Then
				
				' RFC110400 - Look for a setting in the Common\GIS
				' i.e. Not Data Model Specifc
				lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=GISRegDataSetPath, r_sSettingValue:=r_sDataSetsPath, v_sSubKey:=GISRegSubKey), gPMConstants.PMEReturnCode)
				
			End If
			
			If r_sDataSetsPath.Trim() = "" Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			If Not r_sDataSetsPath.EndsWith("\") Then
				r_sDataSetsPath = r_sDataSetsPath & "\"
			End If
			
			Return result
		
		Catch 
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	' ***************************************************************** '
	' Name: GetRegSettingFromDataBusModel
	'
	' Description:
	'
	' Date: CL220200
	' RFC310700 - Optionally specify a SubKey
	' RFC290300 - Make Business Type Code Optional
	' RFC110400 - Optionally specify the RegSetting Level i.e. Client/Common/Server(default)
	' ***************************************************************** '
	Public Function GetRegSettingFromDataBusModel(ByVal v_sDataModelCode As String, ByVal v_sSettingName As String, ByRef r_sSettingValue As String, Optional ByVal v_sBusinessTypeCode As String = "", Optional ByVal v_lPMERegSettingLevel As gPMConstants.PMERegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLServer, Optional ByVal v_sSubKey As String = "") As Integer
		
		Dim result As Integer = 0
		Dim lReturn As gPMConstants.PMEReturnCode
		Dim sValue, sSubKeyDataModel, sSubKeyDataModelBusModel As String
		Dim PMERegSettingLevel As gPMConstants.PMERegSettingLevel
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			sSubKeyDataModelBusModel = ACOIMGISSubKey & "\" & v_sDataModelCode & "\" & v_sBusinessTypeCode
			sSubKeyDataModel = ACOIMGISSubKey & "\" & v_sDataModelCode
			
			' If we have a SubKey
			v_sSubKey = v_sSubKey.Trim()
			If v_sSubKey <> "" Then
				' Append the sub key to both versions of the path
				sSubKeyDataModelBusModel = sSubKeyDataModelBusModel & "\" & v_sSubKey
				sSubKeyDataModel = sSubKeyDataModel & "\" & v_sSubKey
			End If
			
			sValue = ""
			
			' Use the supplied (or default reg setting level)
			PMERegSettingLevel = v_lPMERegSettingLevel
			
			' If we have a Business Type Code the Look for the setting there
			If v_sBusinessTypeCode <> "" Then
				
				' First look for value in data model/bus model key
				lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=PMERegSettingLevel, v_sSettingName:=v_sSettingName, r_sSettingValue:=sValue, v_sSubKey:=sSubKeyDataModelBusModel), gPMConstants.PMEReturnCode)
				
				If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
			End If
			
			' If we haven't got the value, look at Data Model level
			If sValue.Trim() = "" Then
				
				' Value not found in data model/bus model key
				' Therefore look in data model key
				lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=PMERegSettingLevel, v_sSettingName:=v_sSettingName, r_sSettingValue:=sValue, v_sSubKey:=sSubKeyDataModel), gPMConstants.PMEReturnCode)
				
				If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
			End If
			
			r_sSettingValue = sValue
			
			Return result
		
		Catch 
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
End Module