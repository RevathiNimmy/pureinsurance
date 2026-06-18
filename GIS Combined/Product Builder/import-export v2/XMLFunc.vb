Option Strict Off
Option Explicit On
Imports SharedFiles
Module XMLFunc
	
	Private m_lReturn As Integer
	
	Private Const ACClass As String = ""
	
	' ***************************************************************** '
	'
	' Name: CreateDataSet
	'
	' Description:
	'
	' History: 17/09/2000 Tomo - Created.
	'
	' ***************************************************************** '
	
	Public Function CreateDataSet(ByRef v_sGISDataModel As String) As Integer
		
        Dim oGIS As iGIS.Application
		'Dim oGIS As iGIS.Application
		Dim lPolicyLinkID As Integer
		Dim sTopOIKey As String
		Dim sDataSetDefFile As String
		Dim sDataSetFile As String
		
		Try
		
		CreateDataSet = gPMConstants.PMEReturnCode.PMTrue
		
		'First we remove what's already there...
        m_lReturn = GISSharedConstants.GetDataSetFileNames(v_sDataModelCode:=v_sGISDataModel, r_sDataSetDefFile:=sDataSetDefFile, r_sDataSetFile:=sDataSetFile)
		
		If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            Exit Function
		End If
		
		If (sDataSetDefFile <> "") Then
			'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
			If (Dir(sDataSetDefFile) <> "") Then
				Kill(sDataSetDefFile)
			End If
		End If
		
		If (sDataSetFile <> "") Then
			'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
			If (Dir(sDataSetFile) <> "") Then
				Kill(sDataSetFile)
			End If
		End If
		
		'Then we call the GIS to recreate it...
        oGIS = New iGIS.Application
        'oGIS = CreateObject("iGIS.Application")
		
		'UPGRADE_WARNING: Couldn't resolve default property of object oGIS.Initialise. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		m_lReturn = oGIS.Initialise
		
		If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the GIS interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDataSet")

            CreateDataSet = gPMConstants.PMEReturnCode.PMFalse

            Exit Function
        End If

        'UPGRADE_WARNING: Couldn't resolve default property of object oGIS.NewDataSet. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        m_lReturn = oGIS.NewDataSet(v_sGisDataModelCode:=v_sGISDataModel, r_lPolicyLinkID:=lPolicyLinkID, r_sTopOIKey:=sTopOIKey)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create the new data set", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDataSet")

            CreateDataSet = gPMConstants.PMEReturnCode.PMFalse

            'UPGRADE_WARNING: Couldn't resolve default property of object oGIS.Terminate. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                oGIS.Dispose()

                'UPGRADE_NOTE: Object oGIS may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                oGIS = Nothing

            Exit Function
        End If

        'UPGRADE_WARNING: Couldn't resolve default property of object oGIS.Terminate. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            oGIS.Dispose()

            'UPGRADE_NOTE: Object oGIS may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oGIS = Nothing

        Exit Function

		Catch ex As Exception

        CreateDataSet = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateDataSet Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDataSet", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)
		
		Exit Function
		
		
		End Try
	End Function
End Module
