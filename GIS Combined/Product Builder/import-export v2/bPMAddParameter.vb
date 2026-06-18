Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports SharedFiles

Module bPMAddParameter
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	' Constant for the methods to identify which class this is.
	Private Const ACClass As String = "bPMAddParameter"
	
	
	' ***************************************************************** '
	'
	' Name: AddParameter
	'
	' Description: See below
	'
	' History: 29/11/2001 CLG - Created.
	'
	' ***************************************************************** '
	'
	' Builds a sql or sp definition line and adds the parameters
	'    sp format is "{call stored_procedure_name ()}"         "{call sp_add_gis_details ()}"
	'    sql format is "select columns from table"              "select name from gis_details"
	'
	' Public Sub AddParameter(
	'   ByVal v_oDatabase As dPMDAO.Database,       Pointer to open dPMDAO connection
	'   ByRef r_sSQL As String,                     Bare sp or sql definition
	'   ByRef r_lResultCode As PMEReturnCode,       PMEReturnCode
	'   ByVal v_sName As String,                    Parameter name
	'   ByVal v_vValue As Variant,                  Parameter value
	'   ByVal v_iDirection As PMEParamDirection,    PMEParamDirection of parameter
	'   ByVal v_iType As PMEDataType,               PMEDataType of parameter
	'   Optional ByVal v_iWhereMode As Integer,     See below
	'   Optional ByVal v_bIgnoreIfBlank As Boolean) See below
	'
	' v_iWhereMode
	'   0 creates where clause for sql in the form "where this_parameter ="
	'   1 creates where clause for sql in the form "where this_parameter like"
	'   2 creates sql for insert, no where clause!
	'   3 creates where clause for sql in the form "where this_parameter >="
	'
	' v_bIgnoreIfBlank
	'   used in where clauses. if true then this_parameter is not added if it is blank
	'
	' Examples
	'
	'    Add using sp
	'            sSQL = "{call spe_MQ_Provider_add ()}"
	'            sSQL2 = "Add Provider"
	'            AddParameter v_oDatabase, sSQL, iRetVal, "mq_provider_id", v_iProviderId, PMParamInput, PMLong
	'            AddParameter v_oDatabase, sSQL, iRetVal, "is_partner", v_isPartner, PMParamInput, PMInteger
	'            AddParameter v_oDatabase, sSQL, iRetVal, "provider_name", v_sName, PMParamInput, PMString
	'            If iRetVal = PMTrue Then
	'                iRetVal= v_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:=sSQL2, bStoredProcedure:=True)
	'            End If
	
	'    Add using SQL
	'            sSQL = "insert mq_security_statistics (mq_security_cnt) values ()"
	'            sSQL2 = "AddSecurity_Statistics"
	'            AddParameter v_oDatabase, sSQL, MQTXS_maintainSecurity_Statistics, "mq_security_cnt", v_iSecurityId, PMParamInput, PMInteger, 2
	'            If MQTXS_maintainSecurity_Statistics = PMTrue Then
	'                MQTXS_maintainSecurity_Statistics = v_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:=sSQL2, bStoredProcedure:=False)
	'            End If
	
	'   Get using SQL
	'            sSQL = "select * from mq_provider "
	'            sSQL2="Get Provider"
	'            AddParameter v_oDatabase, sSQL, iRetVal, "mq_provider_id", v_iProviderId, PMParamInput, PMInteger, v_bIgnoreIfBlank:=True
	'            AddParameter v_oDatabase, sSQL, iRetVal, "provider_name", v_sName, PMParamInput, PMString, v_bIgnoreIfBlank:=True
	'            If iRetVal = PMTrue Then
	'                iRetVal= v_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:=sSQL2, bStoredProcedure:=false)
	'            End If
	'
	' ***************************************************************** '
	Public Sub AddParameter(ByVal v_oDatabase As dPMDAO.Database, ByRef r_sSQL As String, ByRef r_lResultCode As gPMConstants.PMEReturnCode, ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iDirection As gPMConstants.PMEParameterDirection, ByVal v_iType As gPMConstants.PMEDataType, Optional ByVal v_iWhereMode As Short = 0, Optional ByVal v_bIgnoreIfBlank As Boolean = False)
		
		' Debug message
		Debug.Print(VB.Timer() & ": Entering " & ACApp & "." & ACClass & ".AddParameter")
		
		Try
		
		Dim value As Integer
		Dim storedProcedure As Boolean : storedProcedure = True
		
		'check for first time in and clear parameters
		If InStr(r_sSQL, "call ") <> 0 Then
			If InStr(r_sSQL, "?") = 0 Then 'if first time in
				v_oDatabase.Parameters.Clear()
				r_lResultCode = gPMConstants.PMEReturnCode.PMTrue
			End If
		Else
			storedProcedure = False
			If InStr(r_sSQL, "{") = 0 Then 'if first time in
				v_oDatabase.Parameters.Clear()
				r_lResultCode = gPMConstants.PMEReturnCode.PMTrue
			End If
		End If
		
		'only continue if no error and we have a parameter to add
		'UPGRADE_WARNING: Couldn't resolve default property of object v_vValue. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		Dim pos As Short
		If r_lResultCode = gPMConstants.PMEReturnCode.PMTrue And (v_bIgnoreIfBlank = False Or (v_bIgnoreIfBlank = True And v_vValue <> "")) Then
			
			'we are now adding a parameter
			If v_iWhereMode = 1 Then
				'UPGRADE_WARNING: Couldn't resolve default property of object v_vValue. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				v_vValue = v_vValue + "%" 'fixup 'like' parameter
			End If
			
			'add the parameter
			r_lResultCode = v_oDatabase.Parameters.Add(sName:=v_sName, vValue:=v_vValue, iDirection:=CShort(v_iDirection), iDataType:=CShort(v_iType))
			
			If storedProcedure = True Then
				'modify the stored procedure call
				value = InStr(r_sSQL, ")")
				r_sSQL = Mid(r_sSQL, 1, value - 1)
				If v_oDatabase.Parameters.Count > 1 Then
					r_sSQL = r_sSQL & ",?)}"
				Else
					r_sSQL = r_sSQL & "?)}"
				End If
				
				' if an error then add description to SQL string
				If r_lResultCode <> gPMConstants.PMEReturnCode.PMTrue Then
					r_sSQL = r_sSQL & " parameter (" & v_sName & ") error " & CStr(r_lResultCode)
				End If
			Else
				'raw sql
				Select Case v_iWhereMode
					Case 2 'insert
						pos = InStr(r_sSQL, ")")
						pos = InStr(pos + 1, r_sSQL, ")")
						r_sSQL = Left(r_sSQL, pos - 1)
						If Mid(r_sSQL, pos - 1, 1) <> "(" Then
							r_sSQL = r_sSQL & ","
						End If
						r_sSQL = r_sSQL & "{" & v_sName & "})"
						
					Case Else
						If InStr(r_sSQL, "where") = 0 Then
							r_sSQL = r_sSQL & " where"
						Else
							r_sSQL = r_sSQL & " and"
						End If
						Select Case v_iWhereMode
							Case 1
								r_sSQL = r_sSQL & " " & v_sName & " like {" & v_sName & "}"
							Case 3
								r_sSQL = r_sSQL & " " & v_sName & ">={" & v_sName & "}"
							Case Else
								'UPGRADE_WARNING: Couldn't resolve default property of object v_vValue. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
								If v_vValue <> "-1" Then
									r_sSQL = r_sSQL & " " & v_sName & "={" & v_sName & "}"
								Else
									r_sSQL = r_sSQL & " " & v_sName & " is Null"
								End If
						End Select
				End Select
			End If
		End If
		
		' Debug message
		Debug.Print(VB.Timer() & ": Exiting " & ACApp & "." & ACClass & ".AddParameter")
		
		Exit Sub
		
		Catch ex As Exception
		
		' Debug message
		Debug.Print(VB.Timer() & ": Errored in " & ACApp & "." & ACClass & ".AddParameter")
		
		
		' Log Error Message
        iPMFunc.LogMessage(susername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddParameter Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddParameter", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)
		
		Exit Sub
		
		End Try
	End Sub
	
	
	
	
	' ***************************************************************** '
	' Name: AddParameterLite
	'
	' Description: See below
	'
	' History: 20/06/2003 Peter Finney - Created.
	'
	' ***************************************************************** '
	'
	' Adds parameters for a stored procedure call
	'
	' Public Sub AddParameterLite(
	'   ByVal v_oDatabase As dPMDAO.Database,           Pointer to open dPMDAO connection
	'   ByVal v_sName As String,                        Parameter name
	'   ByVal v_vValue As Variant,                      Parameter value
	'   ByVal v_iDirection As PMEParamDirection,        PMEParamDirection of parameter
	'   ByVal v_iType As PMEDataType,                   PMEDataType of parameter
	'   Optional ByVal v_bClearParameters As Boolean)   Clear the parameter collection?
	'
	' v_bClearParameters
	'   used when adding the first parameter. if true then the database parameters collection if cleared
	'
	' Examples
	'
	'    Add using sp
	'       AddParameter v_oDatabase, "mq_provider_id", v_iProviderId, PMParamInput, PMLong, True
	'       AddParameter v_oDatabase, "is_partner", v_isPartner, PMParamInput, PMInteger
	'       AddParameter v_oDatabase, "provider_name", v_sName, PMParamInput, PMString
	'
	' ***************************************************************** '
	Public Sub AddParameterLite(ByVal v_oDatabase As dPMDAO.Database, ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iDirection As gPMConstants.PMEParameterDirection, ByVal v_iDataType As gPMConstants.PMEDataType, Optional ByVal v_bClearParameters As Boolean = False)
		
		Dim lReturn As Integer
		
		' Note: No error handling.
		'   Let serious errors bubble up to calling function.
		'   If we don't it will be very difficult to locate the cause.
		
		' If this is the first parameter clear the current ones
		If v_bClearParameters Then
			v_oDatabase.Parameters.Clear()
		End If
		
		' Add our new parameter
		lReturn = v_oDatabase.Parameters.Add(sName:=v_sName, vValue:=v_vValue, iDirection:=CShort(v_iDirection), iDataType:=CShort(v_iDataType))
		
		' Check for success
		If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
			' Raise
            RaiseError("AddDBParameter", "Error " & NullToString(lReturn) & " adding parameter '" & v_sName & "' with value '" & NullToString(v_vValue) & "'", vbObjectError)
		End If
		
	End Sub
End Module
