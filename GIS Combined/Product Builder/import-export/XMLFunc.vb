Option Strict Off
Option Explicit On

Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports SharedFiles

Module XMLFunc
	
	Private m_lReturn As gPMConstants.PMEReturnCode
	
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
		
		Dim result As Integer = 0
		Dim oGIS As iGIS.Application
		'Dim oGIS As iGIS.Application
		Dim lPolicyLinkID As Integer
		Dim sTopOIKey, sDataSetDefFile, sDataSetFile As String
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'First we remove what's already there...
			m_lReturn = CType(GISSharedConstants.GetDataSetFileNames(v_sDataModelCode:=v_sGISDataModel, r_sDataSetDefFile:=sDataSetDefFile, r_sDataSetFile:=sDataSetFile), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			If sDataSetDefFile <> "" Then
				If FileSystem.Dir(sDataSetDefFile, FileAttribute.Normal) <> "" Then
					File.Delete(sDataSetDefFile)
				End If
			End If
			
			If sDataSetFile <> "" Then
				If FileSystem.Dir(sDataSetFile, FileAttribute.Normal) <> "" Then
					File.Delete(sDataSetFile)
				End If
			End If
			
			'Then we call the GIS to recreate it...
			
			oGIS = New iGIS.Application()
			
			m_lReturn = CType(oGIS, SSP.S4I.Interfaces.ILocalInterface).Initialise()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				
				' Log Error Message
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the GIS interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDataSet")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oGIS.NewDataSet(v_sGisDataModelCode:=v_sGISDataModel, r_lPolicyLinkID:=lPolicyLinkID, r_sTopOIKey:=sTopOIKey)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create the new data set", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDataSet")

                result = gPMConstants.PMEReturnCode.PMFalse

                oGIS.Dispose()

                oGIS = Nothing

                Return result
            End If

            oGIS.Dispose()

            oGIS = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateDataSet Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDataSet", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
	End Function
End Module
