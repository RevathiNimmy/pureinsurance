Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Administration_NET.Administration")> _
Public NotInheritable Class Administration 
	
	' ************************************************
	' Added to replace global variables 19/09/2003
	' Username.
	Private m_sUsername As String = ""
	
	' Password.
	Private m_sPassword As String = ""
	
	' User ID
	Private m_iUserID As Integer
	
	' Calling Application
	Private m_sCallingAppName As String = ""
	' Source ID
	Private m_iSourceID As Integer
	' Language ID
	Private m_iLanguageID As Integer
	' Currency ID
	Private m_iCurrencyID As Integer
	' LogLevel
	Private m_iLogLevel As Integer
	' ************************************************
	
	
	Private Const ACClass As String = "Administration"
	
	Private Const ACAddAddOnSQL As String = "{call spu_gis_add_on_add(?,?,?,?,?,?,?)}"
	Private Const ACAddAddOnName As String = "AddAddOn"
	Private Const ACAddAddOnStored As Boolean = True
	
	Private Const ACUpdateAddOnSQL As String = "{call spu_gis_add_on_upd(?,?,?,?,?,?,?)}"
	Private Const ACUpdateAddOnName As String = "UpdateAddOn"
	Private Const ACUpdateAddOnStored As Boolean = True
	
	Private Const ACDeleteAddOnSQL As String = "{call spu_gis_add_on_del(?)}"
	Private Const ACDeleteAddOnName As String = "DeleteAddOn"
	Private Const ACDeleteAddOnStored As Boolean = True
	
	'Private Const ACSelectAddOnSQL = "{call spu_gis_add_on_sel(?)}"
	Private Const ACSelectAddOnSQL As String = "{call spu_gis_add_on_sel_adm(?)}"
	Private Const ACSelectAddOnName As String = "SelectAddOn"
	Private Const ACSelectAddOnStored As Boolean = True
	
	Private Const ACAddCoverLevelSQL As String = "{call spu_gis_add_on_cover_level_add(?,?,?,?,?)}"
	Private Const ACAddCoverLevelName As String = "AddCoverLevel"
	Private Const ACAddCoverLevelStored As Boolean = True
	
	Private Const ACUpdateCoverLevelSQL As String = "{call spu_gis_add_on_cover_level_upd(?,?,?,?,?)}"
	Private Const ACUpdateCoverLevelName As String = "UpdateCoverLevel"
	Private Const ACUpdateCoverLevelStored As Boolean = True
	
	Private Const ACDeleteCoverLevelSQL As String = "{call spu_gis_add_on_cover_level_del(?)}"
	Private Const ACDeleteCoverLevelName As String = "DeleteCoverLevel"
	Private Const ACDeleteCoverLevelStored As Boolean = True
	
	Private Const ACSelectCoverLevelSQL As String = "{call spu_gis_add_on_cover_level_sel(?,?)}"
	Private Const ACSelectCoverLevelName As String = "SelectCoverLevel"
	Private Const ACSelectCoverLevelStored As Boolean = True
	
	Private Const ACAddRateSQL As String = "{call spu_gis_add_on_rate_add(?,?,?,?,?,?,?,?,?,?,?,?,?)}"
	Private Const ACAddRateName As String = "AddRate"
	Private Const ACAddRateStored As Boolean = True
	
	Private Const ACUpdateRateSQL As String = "{call spu_gis_add_on_rate_upd(?,?,?,?,?,?,?,?,?,?,?,?,?)}"
	Private Const ACUpdateRateName As String = "UpdateRate"
	Private Const ACUpdateRateStored As Boolean = True
	
	Private Const ACDeleteRateSQL As String = "{call spu_gis_add_on_rate_del(?)}"
	Private Const ACDeleteRateName As String = "DeleteRate"
	Private Const ACDeleteRateStored As Boolean = True
	
	Private Const ACSelectRateSQL As String = "{call spu_gis_add_on_rate_sel(?,?,?,?,?)}"
	Private Const ACSelectRateName As String = "SelectRate"
	Private Const ACSelectRateStored As Boolean = True
	
	Private Const ACAddOnDataBusSQL As String = "{call spu_gis_add_on_databus}"
	Private Const ACAddOnDataBusName As String = "GetAddOnDataBus"
	Private Const ACAddOnDataBusStored As Boolean = True
	
	Private m_oDatabase As dPMDAO.Database
	
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	
	Public Function AddAddOn(ByVal v_sGisDataModelCode As String, ByRef r_lAddOnID As Integer, ByVal v_sCode As String, ByVal v_lCaptionID As Integer, ByVal v_sDescription As String, ByVal v_bIsDeleted As Boolean, ByVal v_dtEffectiveDate As Date, ByVal v_lPartyCnt As Object) As Integer
		
		' **************************************************
		' Name: AddAddOn
		'
		' Description: Creates a new add on record with the
		'   given values.
		' **************************************************
		
		Dim result As Integer = 0
		Try 
			
			Dim bNew As Boolean
			Dim vArray As Object
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Use the Data Model Specifc DSN
			m_lReturn = CType(GISSharedConstants.CheckGISDSN(v_sDataModelCode:=v_sGisDataModelCode, r_oDatabase:=m_oDatabase, r_bNew:=bNew), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Clear the Parameters collection
			m_oDatabase.Parameters.Clear()
			
			' ID (return parameter)
			m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_add_on_id", vValue:="", idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, idatatype:=gPMConstants.PMEDataType.PMLong)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Code
			m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=v_sCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMString)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Caption ID
			m_lReturn = m_oDatabase.Parameters.Add(sName:="caption_id", vValue:=CStr(v_lCaptionID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMLong)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Description
			m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=v_sDescription, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMString)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Is Deleted flag
			m_lReturn = m_oDatabase.Parameters.Add(sName:="is_deleted", vValue:=CStr(Math.Abs(CInt(v_bIsDeleted))), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMBoolean)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Effective date
			m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTimeHelper.ToString(v_dtEffectiveDate), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMDate)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Party cnt

			m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(v_lPartyCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMLong)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Call the SQL
			m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACAddAddOnSQL, ssqlname:=ACAddAddOnName, bstoredprocedure:=ACAddAddOnStored)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set the new id return value
			r_lAddOnID = m_oDatabase.Parameters.Item("gis_add_on_id").Value
			
			' If we made it this far we should have succeeded
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Log Error Message
            'developer guide no. 180(Latest Guide)
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAddOnFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddAddOn", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result
		End Try
	End Function
	
	
	Public Function AddAddOnCoverLevel(ByVal v_sGisDataModelCode As String, ByVal v_lAddOnID As Integer, ByRef r_lAddOnCoverLevelID As Integer, ByVal v_sCode As String, ByVal v_lCaptionID As Integer, ByVal v_sDescription As String) As Integer
		
		' **************************************************
		' Name: AddAddOnCoverLevel
		'
		' Description: Creates a new add on record with the
		'   given values.
		' **************************************************
		
		Dim result As Integer = 0
		Try 
			
			Dim bNew As Boolean
			Dim vArray As Object
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Use the Data Model Specifc DSN
			m_lReturn = CType(GISSharedConstants.CheckGISDSN(v_sDataModelCode:=v_sGisDataModelCode, r_oDatabase:=m_oDatabase, r_bNew:=bNew), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Clear the Parameters collection
			m_oDatabase.Parameters.Clear()
			
			' Add On ID
			m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_add_on_id", vValue:=CStr(v_lAddOnID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMLong)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Cover Level ID (return parameter)
			m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_add_on_cover_level_id", vValue:="", idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, idatatype:=gPMConstants.PMEDataType.PMLong)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Code
			m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=v_sCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMString)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Caption ID
			m_lReturn = m_oDatabase.Parameters.Add(sName:="caption_id", vValue:=CStr(v_lCaptionID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMLong)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Description
			m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=v_sDescription, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMString)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Call the SQL
			m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACAddCoverLevelSQL, ssqlname:=ACAddCoverLevelName, bstoredprocedure:=ACAddCoverLevelStored)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set the new id return value
			r_lAddOnCoverLevelID = m_oDatabase.Parameters.Item("gis_add_on_cover_level_id").Value
			
			' If we made it this far we should have succeeded
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Log Error Message
            'developer guide no. 180(Latest Guide)
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAddOnCoverLevelFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddAddOnCoverLevel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result
		End Try
	End Function
	
	
	Public Function AddAddOnRate(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_lAddOnRateID As Integer, ByVal v_lAddOnID As Integer, ByVal v_lAddOnCoverLevelID As Integer, ByVal v_cNewBusinessFee As Object, ByVal v_cNewBusinessRate As Object, ByVal v_cNewBusinessIPTRate As Object, ByVal v_dtEffectiveDate As Date, ByVal v_dtExpiryDate As Object, ByVal v_cCommissionAmt As Object, ByVal v_cCommissionPct As Object, ByVal v_cNewBusinessVATRate As Object) As Integer
		
		' **************************************************
		' Name: AddAddOnRate
		'
		' Description: Creates a new add on rate record with
		'   the given values.
		' **************************************************
		
		Dim result As Integer = 0
		Try 
			
			Dim bNew As Boolean
			Dim vArray As Object
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Use the Data Model Specifc DSN
			m_lReturn = CType(GISSharedConstants.CheckGISDSN(v_sDataModelCode:=v_sGisDataModelCode, r_oDatabase:=m_oDatabase, r_bNew:=bNew), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Clear the Parameters collection
			m_oDatabase.Parameters.Clear()
			
			' Rate ID (return parameter)
			m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_add_on_rate_id", vValue:="", idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, idatatype:=gPMConstants.PMEDataType.PMLong)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Add On ID
			m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_add_on_id", vValue:=CStr(v_lAddOnID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMLong)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Cover Level ID
			m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_add_on_cover_level_id", vValue:=CStr(v_lAddOnCoverLevelID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMLong)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Data model code
			m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_data_model_code", vValue:=v_sGisDataModelCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMString)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Business type code
			m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_business_type_code", vValue:=v_sGisBusinessTypeCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMString)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' New business fee

			m_lReturn = m_oDatabase.Parameters.Add(sName:="new_business_fee", vValue:=CStr(v_cNewBusinessFee), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMCurrency)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' New business rate

			m_lReturn = m_oDatabase.Parameters.Add(sName:="new_business_rate", vValue:=CStr(v_cNewBusinessRate), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMCurrency)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' New business ipt rate

			m_lReturn = m_oDatabase.Parameters.Add(sName:="new_business_ipt_rate", vValue:=CStr(v_cNewBusinessIPTRate), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMCurrency)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Effective date
			m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTimeHelper.ToString(v_dtEffectiveDate), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMDate)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Expiry date

			m_lReturn = m_oDatabase.Parameters.Add(sName:="expiry_date", vValue:=CStr(v_dtExpiryDate), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMDate)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Commission amount

			m_lReturn = m_oDatabase.Parameters.Add(sName:="commission_amt", vValue:=CStr(v_cCommissionAmt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMCurrency)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Commission percent

			m_lReturn = m_oDatabase.Parameters.Add(sName:="commission_pct", vValue:=CStr(v_cCommissionPct), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMCurrency)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' New business vat rate

			m_lReturn = m_oDatabase.Parameters.Add(sName:="new_business_vat_rate", vValue:=CStr(v_cNewBusinessVATRate), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMCurrency)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Call the SQL
			m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACAddRateSQL, ssqlname:=ACAddRateName, bstoredprocedure:=ACAddRateStored)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set the new id return value
			r_lAddOnRateID = m_oDatabase.Parameters.Item("gis_add_on_rate_id").Value
			
			' If we made it this far we should have succeeded
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Log Error Message
            'developer guide no. 180(Latest Guide)
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAddOnRateFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddAddOnRate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result
		End Try
	End Function
	
	
	Public Function DeleteAddOn(ByVal v_sGisDataModelCode As String, ByVal v_lAddOnID As Integer) As Integer
		
		' **************************************************
		' Name: DeleteAddOn
		'
		' Description: Delete the given add on
		' **************************************************
		
		Dim result As Integer = 0
		Try 
			
			Dim bNew As Boolean
			Dim vArray As Object
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Use the Data Model Specifc DSN
			m_lReturn = CType(GISSharedConstants.CheckGISDSN(v_sDataModelCode:=v_sGisDataModelCode, r_oDatabase:=m_oDatabase, r_bNew:=bNew), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Clear the Parameters collection
			m_oDatabase.Parameters.Clear()
			
			' ID (return parameter)
			m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_add_on_id", vValue:=CStr(v_lAddOnID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMLong)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Call the SQL
			m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACDeleteAddOnSQL, ssqlname:=ACDeleteAddOnName, bstoredprocedure:=ACDeleteAddOnStored)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' If we made it this far we should have succeeded
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Log Error Message
            'developer guide no. 180(Latest Guide)
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteAddOnFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAddOn", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result
		End Try
	End Function
	
	
	Public Function DeleteAddOnCoverLevel(ByVal v_sGisDataModelCode As String, ByVal v_lAddOnCoverLevelID As Integer) As Integer
		
		' **************************************************
		' Name: DeleteAddOnCoverLevel
		'
		' Description: Delete the given add on
		' **************************************************
		
		Dim result As Integer = 0
		Try 
			
			Dim bNew As Boolean
			Dim vArray As Object
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Use the Data Model Specifc DSN
			m_lReturn = CType(GISSharedConstants.CheckGISDSN(v_sDataModelCode:=v_sGisDataModelCode, r_oDatabase:=m_oDatabase, r_bNew:=bNew), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Clear the Parameters collection
			m_oDatabase.Parameters.Clear()
			
			' ID (return parameter)
			m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_add_on_cover_level_id", vValue:=CStr(v_lAddOnCoverLevelID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMLong)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Call the SQL
			m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACDeleteCoverLevelSQL, ssqlname:=ACDeleteCoverLevelName, bstoredprocedure:=ACDeleteCoverLevelStored)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' If we made it this far we should have succeeded
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Log Error Message
            'developer guide no. 180(Latest Guide)
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteAddOnCoverLevelFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAddOnCoverLevel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result
		End Try
	End Function
	
	
	Public Function DeleteAddOnRate(ByVal v_sGisDataModelCode As String, ByVal v_lAddOnRateID As Integer) As Integer
		
		' **************************************************
		' Name: DeleteAddOnRate
		'
		' Description: Delete the given add on
		' **************************************************
		
		Dim result As Integer = 0
		Try 
			
			Dim bNew As Boolean
			Dim vArray As Object
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Use the Data Model Specifc DSN
			m_lReturn = CType(GISSharedConstants.CheckGISDSN(v_sDataModelCode:=v_sGisDataModelCode, r_oDatabase:=m_oDatabase, r_bNew:=bNew), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Clear the Parameters collection
			m_oDatabase.Parameters.Clear()
			
			' ID (return parameter)
			m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_add_on_rate_id", vValue:=CStr(v_lAddOnRateID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMLong)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Call the SQL
			m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACDeleteCoverLevelSQL, ssqlname:=ACDeleteCoverLevelName, bstoredprocedure:=ACDeleteCoverLevelStored)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' If we made it this far we should have succeeded
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Log Error Message
            'developer guide no. 180(Latest Guide)
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteAddOnRateFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAddOnRate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result
		End Try
	End Function
	
	
    Public Function GetAddOn(ByVal v_sGisDataModelCode As String, ByVal v_lAddOnID As Byte, ByRef r_vAddOnArray(,) As Object) As Integer

        ' **************************************************
        ' Name: GetAddOn
        '
        ' Description: Retrieves an array of addons. If an
        '   add on id is specified returns restricted array.
        ' **************************************************

        Dim result As Integer = 0
        Try

            Dim bNew As Boolean
            Dim vArray(,) As Object

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Use the Data Model Specifc DSN
            m_lReturn = CType(GISSharedConstants.CheckGISDSN(v_sDataModelCode:=v_sGisDataModelCode, r_oDatabase:=m_oDatabase, r_bNew:=bNew), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Clear the Parameters collection
            m_oDatabase.Parameters.Clear()

            ' Add on id input param

            If False Or Convert.IsDBNull(v_lAddOnID) Or IsNothing(v_lAddOnID) Or (v_lAddOnID = 0) Then

                v_lAddOnID = Nothing
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_add_on_id", vValue:=CStr(v_lAddOnID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectAddOnSQL, ssqlname:=ACSelectAddOnName, bstoredprocedure:=ACSelectAddOnStored, vresultarray:=vArray, bkeepnulls:=True, lnumberrecords:=gPMConstants.PMAllRecords)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If we have a valid array return it else return failure
            If Information.IsArray(vArray) Then


                r_vAddOnArray = vArray
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            'developer guide no. 180(Latest Guide)
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAddOnFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAddOn", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function
	
	
    Public Function GetAddOnCoverLevel(ByVal v_sGisDataModelCode As String, ByVal v_lAddOnID As Byte, ByVal v_lAddOnCoverLevelID As Byte, ByRef r_vAddOnArray(,) As Object) As Integer

        ' **************************************************
        ' Name: GetAddOnCoverLevel
        '
        ' Description: Retrieves an array of addons. If an
        '   add on id is specified returns restricted array.
        ' **************************************************

        Dim result As Integer = 0
        Try

            Dim bNew As Boolean
            Dim vArray(,) As Object

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Use the Data Model Specifc DSN
            m_lReturn = CType(GISSharedConstants.CheckGISDSN(v_sDataModelCode:=v_sGisDataModelCode, r_oDatabase:=m_oDatabase, r_bNew:=bNew), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Clear the Parameters collection
            m_oDatabase.Parameters.Clear()

            ' Add on id input param

            If False Or Convert.IsDBNull(v_lAddOnID) Or IsNothing(v_lAddOnID) Or (v_lAddOnID = 0) Then

                v_lAddOnID = Nothing
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_add_on_id", vValue:=CStr(v_lAddOnID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add on cover level id input param

            If False Or Convert.IsDBNull(v_lAddOnCoverLevelID) Or IsNothing(v_lAddOnCoverLevelID) Or (v_lAddOnCoverLevelID = 0) Then

                v_lAddOnCoverLevelID = Nothing
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_add_on_cover_level_id", vValue:=CStr(v_lAddOnCoverLevelID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectCoverLevelSQL, ssqlname:=ACSelectCoverLevelName, bstoredprocedure:=ACSelectCoverLevelStored, vresultarray:=vArray, bkeepnulls:=True, lnumberrecords:=gPMConstants.PMAllRecords)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If we have a valid array return it else return failure
            If Information.IsArray(vArray) Then


                r_vAddOnArray = vArray
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            'developer guide no. 180(Latest Guide)
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAddOnCoverLevelFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAddOnCoverLevel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function
	
	
    Public Function GetAddOnDataBus(ByVal v_sGisDataModelCode As String, ByRef r_vAddOnArray(,) As Object) As Integer

        ' **************************************************
        ' Name: GetAddOnDataBus
        '
        ' Description: Retrieves a array of valid data model
        '   and business type combinations
        ' **************************************************

        Dim result As Integer = 0
        Try

            Dim bNew As Boolean
            Dim vArray(,) As Object

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Use the Data Model Specifc DSN
            m_lReturn = CType(GISSharedConstants.CheckGISDSN(v_sDataModelCode:=v_sGisDataModelCode, r_oDatabase:=m_oDatabase, r_bNew:=bNew), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Clear the Parameters collection
            m_oDatabase.Parameters.Clear()

            ' Call the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACAddOnDataBusSQL, ssqlname:=ACAddOnDataBusName, bstoredprocedure:=ACAddOnDataBusStored, vresultarray:=vArray, bkeepnulls:=True, lnumberrecords:=gPMConstants.PMAllRecords)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If we have a valid array return it else return failure
            If Information.IsArray(vArray) Then


                r_vAddOnArray = vArray
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            'developer guide no. 180(Latest guide)
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAddOnDataBusFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAddOnDataBus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function
	
	
    Public Function GetAddOnRate(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessType As String, ByVal v_lAddOnID As Byte, ByVal v_lAddOnCoverLevelID As Byte, ByVal v_lAddOnRateID As Byte, ByRef r_vAddOnArray(,) As Object) As Integer

        ' **************************************************
        ' Name: GetAddOnCoverLevel
        '
        ' Description: Retrieves an array of addons. If an
        '   add on id is specified returns restricted array.
        ' **************************************************

        Dim result As Integer = 0
        Try

            Dim bNew As Boolean
            Dim vArray(,) As Object

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Use the Data Model Specifc DSN
            m_lReturn = CType(GISSharedConstants.CheckGISDSN(v_sDataModelCode:=v_sGisDataModelCode, r_oDatabase:=m_oDatabase, r_bNew:=bNew), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Clear the Parameters collection
            m_oDatabase.Parameters.Clear()

            ' Data Model ID Input Param

            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_data_model_code", vValue:=IIf(v_sGisDataModelCode.Length, v_sGisDataModelCode, (DBNull.Value)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Business Type Input Param

            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_business_type_code", vValue:=IIf(v_sGisBusinessType.Length, v_sGisBusinessType, (DBNull.Value)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add on id input param

            If False Or Convert.IsDBNull(v_lAddOnID) Or IsNothing(v_lAddOnID) Or (v_lAddOnID = 0) Then

                v_lAddOnID = Nothing
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_add_on_id", vValue:=CStr(v_lAddOnID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add on cover level id input param

            If False Or Convert.IsDBNull(v_lAddOnCoverLevelID) Or IsNothing(v_lAddOnCoverLevelID) Or (v_lAddOnCoverLevelID = 0) Then

                v_lAddOnCoverLevelID = Nothing
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_add_on_cover_level_id", vValue:=CStr(v_lAddOnCoverLevelID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add on rate id input param

            If False Or Convert.IsDBNull(v_lAddOnRateID) Or IsNothing(v_lAddOnRateID) Or (v_lAddOnRateID = 0) Then

                v_lAddOnRateID = Nothing
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_add_on_rate_id", vValue:=CStr(v_lAddOnRateID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectRateSQL, ssqlname:=ACSelectRateName, bstoredprocedure:=ACSelectRateStored, vresultarray:=vArray, bkeepnulls:=True, lnumberrecords:=gPMConstants.PMAllRecords)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If we have a valid array return it else return failure
            If Information.IsArray(vArray) Then


                r_vAddOnArray = vArray
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            'developer guide no. 180(Latest guide)
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAddOnRateFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAddOnRate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function
	
	
	Public Function UpdateAddOn(ByVal v_sGisDataModelCode As String, ByVal v_lAddOnID As Integer, ByVal v_sCode As String, ByVal v_lCaptionID As Integer, ByVal v_sDescription As String, ByVal v_bIsDeleted As Boolean, ByVal v_dtEffectiveDate As Date, ByVal v_lPartyCnt As Object) As Integer
		
		' **************************************************
		' Name: UpdateAddOn
		'
		' Description: Creates a new add on record with the
		'   given values.
		' **************************************************
		
		Dim result As Integer = 0
		Try 
			
			Dim bNew As Boolean
			Dim vArray As Object
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Use the Data Model Specifc DSN
			m_lReturn = CType(GISSharedConstants.CheckGISDSN(v_sDataModelCode:=v_sGisDataModelCode, r_oDatabase:=m_oDatabase, r_bNew:=bNew), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Clear the Parameters collection
			m_oDatabase.Parameters.Clear()
			
			' ID (return parameter)
			m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_add_on_id", vValue:=CStr(v_lAddOnID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMLong)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Code
			m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=v_sCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMString)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Caption ID
			m_lReturn = m_oDatabase.Parameters.Add(sName:="caption_id", vValue:=CStr(v_lCaptionID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMLong)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Description
			m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=v_sDescription, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMString)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Is Deleted flag
			m_lReturn = m_oDatabase.Parameters.Add(sName:="is_deleted", vValue:=CStr(Math.Abs(CInt(v_bIsDeleted))), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMBoolean)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Effective date
			m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTimeHelper.ToString(v_dtEffectiveDate), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMDate)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Party cnt

			m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(v_lPartyCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMLong)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Call the SQL
			m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACUpdateAddOnSQL, ssqlname:=ACUpdateAddOnName, bstoredprocedure:=ACUpdateAddOnStored)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' If we made it this far we should have succeeded
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Log Error Message
            'developer guide no. 180(Latest guide)
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateAddOnFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAddOn", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result
		End Try
	End Function
	
	
	Public Function UpdateAddOnCoverLevel(ByVal v_sGisDataModelCode As String, ByVal v_lAddOnID As Integer, ByVal v_lAddOnCoverLevelID As Integer, ByVal v_sCode As String, ByVal v_lCaptionID As Integer, ByVal v_sDescription As String) As Integer
		
		' **************************************************
		' Name: UpdateAddOnCoverLevel
		'
		' Description: Creates a new add on record with the
		'   given values.
		' **************************************************
		
		Dim result As Integer = 0
		Try 
			
			Dim bNew As Boolean
			Dim vArray As Object
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Use the Data Model Specifc DSN
			m_lReturn = CType(GISSharedConstants.CheckGISDSN(v_sDataModelCode:=v_sGisDataModelCode, r_oDatabase:=m_oDatabase, r_bNew:=bNew), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Clear the Parameters collection
			m_oDatabase.Parameters.Clear()
			
			' Add On ID
			m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_add_on_id", vValue:=CStr(v_lAddOnID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMLong)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Cover Level ID (return parameter)
			m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_add_on_cover_level_id", vValue:=CStr(v_lAddOnCoverLevelID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMLong)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Code
			m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=v_sCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMString)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Caption ID
			m_lReturn = m_oDatabase.Parameters.Add(sName:="caption_id", vValue:=CStr(v_lCaptionID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMLong)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Description
			m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=v_sDescription, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMString)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Call the SQL
			m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACUpdateCoverLevelSQL, ssqlname:=ACUpdateCoverLevelName, bstoredprocedure:=ACUpdateCoverLevelStored)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' If we made it this far we should have succeeded
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Log Error Message
            'developer guide no. 180(Latest Guide)
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateAddOnCoverLevelFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAddOnCoverLevel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result
		End Try
	End Function
	
	
	Public Function UpdateAddOnRate(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lAddOnRateID As Integer, ByVal v_lAddOnID As Integer, ByVal v_lAddOnCoverLevelID As Integer, ByVal v_cNewBusinessFee As Object, ByVal v_cNewBusinessRate As Object, ByVal v_cNewBusinessIPTRate As Object, ByVal v_dtEffectiveDate As Date, ByVal v_dtExpiryDate As Object, ByVal v_cCommissionAmt As Object, ByVal v_cCommissionPct As Object, ByVal v_cNewBusinessVATRate As Object) As Integer
		
		' **************************************************
		' Name: UpdateAddOnRate
		'
		' Description: Creates a new add on rate record with
		'   the given values.
		' **************************************************
		
		Dim result As Integer = 0
		Try 
			
			Dim bNew As Boolean
			Dim vArray As Object
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Use the Data Model Specifc DSN
			m_lReturn = CType(GISSharedConstants.CheckGISDSN(v_sDataModelCode:=v_sGisDataModelCode, r_oDatabase:=m_oDatabase, r_bNew:=bNew), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Clear the Parameters collection
			m_oDatabase.Parameters.Clear()
			
			' Rate ID (return parameter)
			m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_add_on_rate_id", vValue:=CStr(v_lAddOnRateID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMLong)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Add On ID
			m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_add_on_id", vValue:=CStr(v_lAddOnID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMLong)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Cover Level ID
			m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_add_on_cover_level_id", vValue:=CStr(v_lAddOnCoverLevelID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMLong)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Data model code
			m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_data_model_code", vValue:=v_sGisDataModelCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMString)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Business type code
			m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_business_type_code", vValue:=v_sGisBusinessTypeCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMString)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' New business fee

			m_lReturn = m_oDatabase.Parameters.Add(sName:="new_business_fee", vValue:=CStr(v_cNewBusinessFee), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMCurrency)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' New business rate

			m_lReturn = m_oDatabase.Parameters.Add(sName:="new_business_rate", vValue:=CStr(v_cNewBusinessRate), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMCurrency)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' New business ipt rate

			m_lReturn = m_oDatabase.Parameters.Add(sName:="new_business_ipt_rate", vValue:=CStr(v_cNewBusinessIPTRate), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMCurrency)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Effective date
			m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTimeHelper.ToString(v_dtEffectiveDate), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMDate)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Expiry date

			m_lReturn = m_oDatabase.Parameters.Add(sName:="expiry_date", vValue:=CStr(v_dtExpiryDate), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMDate)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Commission amount

			m_lReturn = m_oDatabase.Parameters.Add(sName:="commission_amt", vValue:=CStr(v_cCommissionAmt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMCurrency)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Commission percent

			m_lReturn = m_oDatabase.Parameters.Add(sName:="commission_pct", vValue:=CStr(v_cCommissionPct), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMCurrency)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' New business vat rate

			m_lReturn = m_oDatabase.Parameters.Add(sName:="new_business_vat_rate", vValue:=CStr(v_cNewBusinessVATRate), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMCurrency)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Call the SQL
			m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACUpdateRateSQL, ssqlname:=ACUpdateRateName, bstoredprocedure:=ACUpdateRateStored)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' If we made it this far we should have succeeded
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Log Error Message
            'developer guide no. 180(Latest Guide)
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateAddOnRateFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAddOnRate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result
		End Try
	End Function
End Class
