Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("bPMAddParameter_NET.bPMAddParameter")>
Public Module bPMAddParameter
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
    '   ByVal v_oDatabase As Object,       Pointer to open dPMDAO connection
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

    'Public Sub AddParameter(ByVal v_oDatabase As Object, ByRef r_sSQL As String, ByRef r_lResultCode As gPMConstants.PMEReturnCode, ByVal v_sName As String, ByVal v_vValue As String, ByVal v_iDirection As gPMConstants.PMEParameterDirection, ByVal v_iType As gPMConstants.PMEDataType, Optional ByVal v_iWhereMode As Integer = 0, Optional ByVal v_bIgnoreIfBlank As Boolean = False)
    Public Sub AddParameter(ByVal v_oDatabase As dPMDAO.Database, ByRef r_sSQL As String, ByRef r_lResultCode As PMEReturnCode, ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iDirection As gPMConstants.PMEParameterDirection, ByVal v_iType As gPMConstants.PMEDataType, Optional ByVal v_iWhereMode As Integer = 0, Optional ByVal v_bIgnoreIfBlank As Boolean = False)

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".AddParameter")

        Try
            Dim storedProcedure As Boolean : storedProcedure = True

            'check for first time in and clear parameters
            'Modified by Alkesh Kumar on 10/05/2010 19:07:13 refer developer guide no. For the stored procedure problem
            'If r_sSQL.IndexOf("call ") >= 0 Then
            'If r_sSQL.StartsWith("sp") Then
            If Not r_sSQL.Trim.Contains(" ") Then
                'If (r_sSQL.IndexOf("?"c) + 1) = 0 Then 'if first time in
                '    v_oDatabase.Parameters.Clear()
                '    r_lResultCode = gPMConstants.PMEReturnCode.PMTrue

            Else
                storedProcedure = False
                If (r_sSQL.IndexOf("{"c) + 1) = 0 Then 'if first time in
                    v_oDatabase.Parameters.Clear()
                    r_lResultCode = gPMConstants.PMEReturnCode.PMTrue
                End If
            End If

            'only continue if no error and we have a parameter to add
            Dim pos As Integer
            If (Not v_bIgnoreIfBlank Or (v_bIgnoreIfBlank And Convert.ToString(v_vValue) <> "")) Then

                'we are now adding a parameter
                If v_iWhereMode = 1 Then
                    v_vValue = v_vValue & "%" 'fixup 'like' parameter
                End If

                'add the parameter
                r_lResultCode = v_oDatabase.Parameters.Add(sName:=v_sName, vValue:=v_vValue, iDirection:=CType(v_iDirection, gPMConstants.PMEParameterDirection), iDataType:=CType(v_iType, gPMConstants.PMEDataType))

                If storedProcedure Then
                    'modify the stored procedure call
                    'value = (r_sSQL.IndexOf(")"c) + 1)
                    'r_sSQL = Mid(r_sSQL, 1, value - 1)
                    'If v_oDatabase.Parameters.Count() > 1 Then
                    '    r_sSQL = r_sSQL & ",?)}"
                    'Else
                    '    r_sSQL = r_sSQL & "?)}"


                    ' if an error then add description to SQL string
                    If r_lResultCode <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sSQL = r_sSQL & " parameter (" & v_sName & ") error " & CStr(r_lResultCode)
                    End If
                Else
                    'raw sql
                    Select Case v_iWhereMode
                        Case 2 'insert
                            pos = (r_sSQL.IndexOf(")"c) + 1)
                            'pos = Strings.InStr(pos + 1, r_sSQL, ")")
                            r_sSQL = r_sSQL.Substring(0, pos - 1)
                            If (r_sSQL.Substring(pos - 1, 1) <> "(") Then
                                r_sSQL = r_sSQL & ","
                            End If
                            r_sSQL = r_sSQL & "{" & v_sName & "})"

                        Case Else
                            If (r_sSQL.IndexOf("where") + 1) = 0 Then
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
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".AddParameter")

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".AddParameter")


            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddParameter Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddParameter", vErrNo:=Nothing, vErrDesc:=excep.Message, excep:=excep)

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
    '   ByVal v_oDatabase As Object,           Pointer to open dPMDAO connection
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


        ' Note: No error handling.
        '   Let serious errors bubble up to calling function.
        '   If we don't it will be very difficult to locate the cause.

        ' If this is the first parameter clear the current ones
        If v_bClearParameters Then
            v_oDatabase.Parameters.Clear()
        End If

        ' Add our new parameter

        'Modified by Deepak Sharma on 5/26/2010 4:37:31 PM refer developer guide no. 85
        'Dim lReturn As Integer = v_oDatabase.Parameters.Add(sName:=v_sName, vValue:=CStr(v_vValue), iDirection:=CType(v_iDirection, gPMConstants.PMEParameterDirection), iDataType:=CType(v_iDataType, gPMConstants.PMEDataType))
        Dim lReturn As Integer
        If v_vValue Is DBNull.Value Then
            lReturn = v_oDatabase.Parameters.Add(sName:=v_sName, vValue:=v_vValue, iDirection:=CType(v_iDirection, gPMConstants.PMEParameterDirection), iDataType:=CType(v_iDataType, gPMConstants.PMEDataType))
        Else

            lReturn = v_oDatabase.Parameters.Add(sName:=v_sName, vValue:=v_vValue, iDirection:=CType(v_iDirection, gPMConstants.PMEParameterDirection), iDataType:=CType(v_iDataType, gPMConstants.PMEDataType))
        End If

        ' Check for success
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Raise
            Throw New System.Exception(" AddDBParameter, " + "Error " & gPMFunctions.NullToString(lReturn) & " adding parameter '" & v_sName & "' with value '" & gPMFunctions.NullToString(v_vValue) & "'")
        End If

    End Sub
End Module