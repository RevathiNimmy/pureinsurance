Option Strict Off
Option Explicit On

Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports System.Windows.Forms
Imports SharedFiles

Module pbImportSQLCommon
	
	Private Const ACClass As String = conEmptyString
	
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	 ' ***************************************************************** '
	 '
	 ' Name:        PrepareSQLStatements
	 '
	 ' Description: Common processes to build insert or update SQL
	 '              statements
	 '
	 ' History:     30/08/2002 JB  - Created.
	 '              24/09/2002 SJP - Changed to be a function, so can
	 '                               pass back if function was successful.
	 '
	 ' ***************************************************************** '
	Public Function PrepareSQLStatements(ByRef r_oDatabase As dPMDAO.Database, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByVal r_iTableIndex As Integer, ByRef r_aRetrievedData() As Object, ByRef r_sErrorString As String) As Integer 
		
		 'Define local variables
		Dim result As Integer = 0 
		Dim sInsertSQL, sUpdateSQL, sSQLSearch, sErrorText As String 
        Dim sSQLWhereClause As New StringBuilder 
        Dim sSQLSelectClause As New StringBuilder 
		
		Dim lRecordsAffected As Integer 
		Dim aAdjustedData, vExistingData(,) As Object 
		Dim aMyArray() As String 
		Dim iLoop As Integer 
		Dim bUpdateRequired As Boolean 
		Dim aParentPK As Object 
		Static sDeleteTable, sDeleteSQL As String

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".PrepareSQLStatements")
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            If CStr(r_aIeControl(pbIeControl_objectName)) = "gis_property" Or CStr(r_aIeControl(pbIeControl_objectName)) = "gis_screen" Then

                If CStr(r_aRetrievedData(2)).ToLower() = "prop_built" Then
                End If
                 'Else
                 '    Exit Function
                 '
            End If

             'Need to adjust the retrieved row to cater for all the 'special' columns.  Need to do
             'it before the SQL statements are built, as the info is held on the control array

            m_lReturn = CType(AdjustRetrievedData(r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, r_aRetrievedData:=r_aRetrievedData, r_aAdjustedData:=aAdjustedData, r_oDatabase:=r_oDatabase, r_sErrorText:=sErrorText), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                r_sErrorString = "Adjust Retrieved Data (" & sErrorText & ") failed for: " & CStr(r_aIeControl(pbIeControl_objectName)) & "(" & CStr(aAdjustedData(0)) & ")"
                Return result
            End If

             'see if we need to run a previously created delete command

            If sDeleteSQL <> "" And ((r_aIeControl(pbIeControl_Flags) And pbIeControl_Flags__deleteBeforeAdd0 <> pbIeControl_Flags__deleteBeforeAdd0) Or sDeleteTable <> CStr(r_aIeControl(pbIeControl_objectName))) Then
                sDeleteSQL = sDeleteSQL.Substring(0, sDeleteSQL.Length - 1) & ")"
                m_lReturn = r_oDatabase.SQLAction(sSQL:=sDeleteSQL, sSQLName:="delete extra entries", bStoredProcedure:=False, lRecordsAffected:=lRecordsAffected)
                sDeleteSQL = ""
                sDeleteTable = ""
            End If

             'see if we need to start or add to a delete command

            If CBool(r_aIeControl(pbIeControl_Flags) And pbIeControl_Flags__deleteBeforeAdd0) Then
                If sDeleteSQL = "" Then

                    sDeleteTable = CStr(r_aIeControl(pbIeControl_objectName))

                    sDeleteSQL = "delete from " & CStr(r_aIeControl(pbIeControl_objectName)) & " where " & CStr(r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(0)(pbIeTableDefinitions_columnName)) & "=" & CStr(aAdjustedData(0)) & " and " & CStr(r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(1)(pbIeTableDefinitions_columnName)) & " not in ("
                End If

                sDeleteSQL = sDeleteSQL & "'" & CStr(aAdjustedData(1)) & "',"
            End If

            aMyArray = CStr(r_aIeControl(pbIeControl_PrimaryKeyColumns)).Split(New String() {conComma}, StringSplitOptions.None)

             'Determine if the table consists of no keys whatsoever.  If so, abandon
             'the update attempt as it will not work without keys or values.

            Try
                iLoop = (aMyArray(0).Length = 0)
                If Information.Err().Number = 9 Then
                     'Subscript out of range caused by aMyArray being an array with no elements
                     'This means that the split function returned no keys so set accordingly.
                    For iLoop = 0 To r_aIeTableDefinitions.GetUpperBound(0)
                        addToArray(aMyArray, iLoop + 1)

                        r_aIeControl(pbIeControl_PrimaryKeyColumns) = CStr(r_aIeControl(pbIeControl_PrimaryKeyColumns)) & CStr(iLoop + 1) & ","
                    Next

                    r_aIeControl(pbIeControl_PrimaryKeyColumns) = CStr(r_aIeControl(pbIeControl_PrimaryKeyColumns)).Substring(0, Strings.Len(CStr(r_aIeControl(pbIeControl_PrimaryKeyColumns))) - 1)
                    Information.Err().Clear()
                End If

            Catch exc As System.Exception

            End Try

            sSQLWhereClause = New StringBuilder(" where 1=1 " & Strings.Chr(13) & Strings.Chr(10))
            sSQLSelectClause = New StringBuilder()
             'if table has an identity and is not a child
             'this if is never executed

            Dim aColumns As Object 
            Dim oColumns As Object 
            If 1 = 0 And CBool(r_aIeControl(pbIeControl_IsIdentity)) Then
                 'sSQLSearch = "SELECT " & r_aIeTableDefinitions(pbIeTableDefinitions_exportedColumns) & " FROM " & r_aIeControl(pbIeControl_objectName) & _
                oColumns = Split(r_aIeTableDefinitions(pbIeTableDefinitions_exportedColumns), ",")

                For iColumnFilterLoop As Integer = 0 To oColumns.GetUpperBound(0)
                    If iColumnFilterLoop < oColumns.GetUpperBound(0) Then
                        If oColumns(iColumnFilterLoop).ToString.Contains(" ") Then
                            sSQLSelectClause.Append("[" & oColumns(iColumnFilterLoop).ToString & "],")
                        Else
                            sSQLSelectClause.Append(oColumns(iColumnFilterLoop).ToString & ",")
                        End If
                    Else
                        If oColumns(iColumnFilterLoop).ToString.Contains(" ") Then
                            sSQLSelectClause.Append("[" & oColumns(iColumnFilterLoop).ToString & "]")
                        Else
                            sSQLSelectClause.Append(oColumns(iColumnFilterLoop).ToString)
                        End If
                    End If
                Next

                sSQLSearch = "SELECT " & sSQLSelectClause.ToString() & " FROM " & CStr(r_aIeControl(pbIeControl_objectName))

                aColumns = CStr(r_aIeTableDefinitions(pbIeTableDefinitions_exportedColumns)).Split(","c)

                For iColumnFilterLoop As Integer = 0 To aColumns.GetUpperBound(0) - 1

                    If CStr(r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(iColumnFilterLoop + 1)(pbIeTableDefinitions_columnName)) <> "caption_id" Then

                        sSQLWhereClause.Append(AddWhereClauseItem(CStr(r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(iColumnFilterLoop + 1)(pbIeTableDefinitions_columnName)), aAdjustedData(iColumnFilterLoop + 1), CStr(r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(iColumnFilterLoop + 1)(pbIeTableDefinitions_columnType))))

                    End If
                Next
            Else
                oColumns = Split(r_aIeTableDefinitions(pbIeTableDefinitions_exportedColumns), ",")

                For iColumnFilterLoop As Integer = 0 To oColumns.GetUpperBound(0)
                    If iColumnFilterLoop < oColumns.GetUpperBound(0) Then
                         'If oColumns(iColumnFilterLoop).ToString.Contains(" ") Then
                        sSQLSelectClause.Append("[" & oColumns(iColumnFilterLoop).ToString & "],")
                         'Else
                         '    sSQLSelectClause.Append(oColumns(iColumnFilterLoop).ToString & ",")
                         'End If
                    Else
                         'If oColumns(iColumnFilterLoop).ToString.Contains(" ") Then
                        sSQLSelectClause.Append("[" & oColumns(iColumnFilterLoop).ToString & "]")
                         'Else
                         '    sSQLSelectClause.Append(oColumns(iColumnFilterLoop).ToString)
                         'End If
                    End If
                Next

                sSQLSearch = "SELECT " & sSQLSelectClause.ToString() & " FROM " & CStr(r_aIeControl(pbIeControl_objectName)) & Strings.Chr(13) & Strings.Chr(10)

                sSQLWhereClause.Append(AddWhereClauseItem(CStr(r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)((aMyArray(0)) - 1)(pbIeTableDefinitions_columnName)), aAdjustedData((aMyArray(0)) - 1), CStr(r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)((aMyArray(0)) - 1)(pbIeTableDefinitions_columnType))))
                If aMyArray.GetUpperBound(0) > 0 Then
                    For iLoop = 0 To aMyArray.GetUpperBound(0)

                        sSQLWhereClause.Append(AddWhereClauseItem(CStr(r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)((aMyArray(iLoop)) - 1)(pbIeTableDefinitions_columnName)), aAdjustedData((aMyArray(iLoop)) - 1), CStr(r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)((aMyArray(iLoop)) - 1)(pbIeTableDefinitions_columnType))))
                    Next iLoop
                End If
            End If
            sSQLSearch = sSQLSearch & sSQLWhereClause.ToString()

             'search for existing record
            m_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQLSearch, sSQLName:="Existing Data Check", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vExistingData)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                r_sErrorString = "SQL Search failed for: " & sSQLSearch
                Return result
            End If

             'check for existing record
            If Not Information.IsArray(vExistingData) Then
                 'INSERT *****************************************************************************
                 'no record, build a SQL insert statement for the data passing the adjusted data array

                m_lReturn = CType(BuildInsertStatement(v_aIeControl:=r_aIeControl, v_aTableDefinition:=r_aIeTableDefinitions(pbIeTableDefinitions_columnArray), v_aRetrievedData:=aAdjustedData, r_sSQL:=sInsertSQL, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)

                 'Execute the statement
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                    If CBool(r_aIeControl(pbIeControl_IsIdentity)) Then
                         'SET IDENTITY_INSERT products ON

                        m_lReturn = r_oDatabase.SQLAction(sSQL:="SET IDENTITY_INSERT " & CStr(r_aIeControl(pbIeControl_objectName)) & " ON", sSQLName:="SET IDENTITY_INSERT on", bStoredProcedure:=False, lRecordsAffected:=lRecordsAffected)
                    End If

                    m_lReturn = r_oDatabase.SQLAction(sSQL:=sInsertSQL, sSQLName:="GenericInsert", bStoredProcedure:=False, lRecordsAffected:=lRecordsAffected)

                    If CBool(r_aIeControl(pbIeControl_IsIdentity)) Then
                         'SET IDENTITY_INSERT products ON

                        r_oDatabase.SQLAction(sSQL:="SET IDENTITY_INSERT " & CStr(r_aIeControl(pbIeControl_objectName)) & " OFF", sSQLName:="SET IDENTITY_INSERT off", bStoredProcedure:=False, lRecordsAffected:=lRecordsAffected)
                    End If

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        r_sErrorString = "SQL Insert failed for: " & sInsertSQL
                        Return result
                    End If

                     'set the parent value to this records key in case this record has children
                    Try

                        aParentPK = g_cParentPK("PK_" & CStr(r_aIeControl(pbIeControl_objectId)))

                    Catch
                    End Try

                    If Not Object.Equals(aParentPK, Nothing) Then

                        g_cParentPK.Remove("PK_" & CStr(r_aIeControl(pbIeControl_objectId)))
                    End If

                    g_cParentPK.Add(New Object() {aAdjustedData(0), r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(conPKColumn)(pbIeTableDefinitions_columnName)}, "PK_" & CStr(r_aIeControl(pbIeControl_objectId)))

                     'End If
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not g_CloneDbKeys Then

                     'Ensure that the update has worked correctly and
                     'rollback if it hasn't
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                         '**TODO - This needs to be replaced by correct error handler
                        MessageBox.Show("Insert Failed - stopping processing", Application.ProductName)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    Else
                         'Even if it has worked, ensure that only one row has been updated.
                         'If more than one has been affected, rollback the changes
                        If lRecordsAffected <> 1 Then
                             '**TODO - This needs to be replaced by correct error handler
                            MessageBox.Show("Multiple rows inserted - stopping processing", Application.ProductName)
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                Else
                     'no error on insert in a cloned database
                End If

            Else
                 'Attempt an update
                 'UPDATE *****************************************************************************
                 'do we need to do an update? check each column for differences
                bUpdateRequired = False

                For iLoop = 0 To r_aIeTableDefinitions(pbIeTableDefinitions_columnArray).GetUpperBound(0)

                    If Not (vExistingData(iLoop, 0) Is Nothing) Then
                        sInsertSQL = vExistingData(iLoop, 0).GetType().Name

                        If iLoop = 0 And CBool(r_aIeControl(pbIeControl_IsIdentity)) Then
                             'ignore identity columns
                        Else

                            If Not (aAdjustedData(iLoop) Is Nothing) Then
                                Select Case vExistingData(iLoop, 0).GetType().Name
                                    Case "String"

                                        aAdjustedData(iLoop) = CStr(aAdjustedData(iLoop)).Trim()

                                        vExistingData(iLoop, 0) = CStr(vExistingData(iLoop, 0)).Trim()

                                        If Not aAdjustedData(iLoop).Equals(vExistingData(iLoop, 0)) Then
                                            bUpdateRequired = True
                                            Exit For
                                        End If
                                    Case "Boolean"

                                        If (CBool(aAdjustedData(iLoop) And (vExistingData(iLoop, 0)) = 0)) Or (Not CBool(aAdjustedData(iLoop)) And (vExistingData(iLoop, 0)) = 1) Then
                                            bUpdateRequired = True
                                            Exit For
                                        End If
                                    Case "Date"

                                        If DateTimeHelper.ToString(CDate(aAdjustedData(iLoop))) <> DateTimeHelper.ToString(CDate(vExistingData(iLoop, 0))) Then
                                            bUpdateRequired = True
                                            Exit For
                                        End If
                                    Case "Currency", "Decimal", "Integer", "Long", "Null", "Byte"

                                        If CStr(r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(iLoop)(pbIeTableDefinitions_columnName)) = "caption_id" Then
                                        End If

                                        If (CStr(aAdjustedData(iLoop)) & "" <> CStr(vExistingData(iLoop, 0)) & "") And 1 = 1 Then  'r_aIeTab 'r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(iLoop)(pbIeTableDefinitions_columnName) <> "" Then
                                            bUpdateRequired = True
                                            Exit For
                                        End If
                                    Case "Byte()"
                                    Case Else

                                End Select
                            End If
                        End If
                    End If
                Next

                If bUpdateRequired Then
                     'Build a SQL update statement based on the array

                    m_lReturn = CType(BuildUpdateStatement(v_aIeControl:=r_aIeControl, v_aTableDefinition:=r_aIeTableDefinitions(pbIeTableDefinitions_columnArray), v_aRetrievedData:=r_aRetrievedData, v_aAdjustedData:=aAdjustedData, v_sSQL:=sUpdateSQL), gPMConstants.PMEReturnCode)

                     'Execute the statement
                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = r_oDatabase.SQLAction(sSQL:=sUpdateSQL, sSQLName:="GenericUpdate", bStoredProcedure:=False, lRecordsAffected:=lRecordsAffected)
                    End If

                     'Ensure that the update has worked correctly
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        r_sErrorString = "SQL Update failed for: " & sUpdateSQL
                        Return result
                    End If
                Else
                     'no update required as source and destination are an exact match
                     'set the parent value to this records key in case this record has children
                    Try

                        aParentPK = g_cParentPK("PK_" & CStr(r_aIeControl(pbIeControl_objectId)))

                    Catch
                    End Try

                    If Not Object.Equals(aParentPK, Nothing) Then

                        g_cParentPK.Remove("PK_" & CStr(r_aIeControl(pbIeControl_objectId)))
                    End If

                    g_cParentPK.Add(New Object() {aAdjustedData(0), r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(conPKColumn)(pbIeTableDefinitions_columnName)}, "PK_" & CStr(r_aIeControl(pbIeControl_objectId)))
                End If
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".PrepareSQLStatements")
            Return result

        Catch ex As Exception

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".PrepareSQLStatements")
            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PrepareSQLStatements Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrepareSQLStatements", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
            Return result

        End Try
    End Function

     ' ***************************************************************** '
     '
     ' Name:        DisableEnableConstraints
     '
     ' Description: Processes to enable or diable the database constraints
     '              to allow the data to be unconditionally inserted to
     '              the database
     '
     ' History:     30/08/2002 JB  - Created.
     '              24/09/2002 SJP - Changed to be a function, so can
     '                               pass back if function was successful.
     '
     ' ***************************************************************** '
    Public Function DisableEnableConstraints(ByRef r_oDatabase As dPMDAO.Database, ByVal v_sTableName As String, ByVal v_ProcessType As Integer) As Integer

         'Define the local variables
        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim lRecordsAffected As Integer

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".DisableEnableConstraints")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

             'Reset the local variables
            sSQL = "ALTER TABLE " & v_sTableName & Strings.Chr(13) & Strings.Chr(10)

             'Determine the processing required
            If v_ProcessType = conDisable Then
                 'Must disable the constraints
                sSQL = sSQL & "NOCHECK CONSTRAINT ALL"
            Else
                 'Must enable the constraints
                sSQL = sSQL & "CHECK CONSTRAINT ALL"
            End If

             'Execute the derived SQL statement
            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Adjust Constraints", bStoredProcedure:=False, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".DisableEnableConstraints")
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".DisableEnableConstraints")

             ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisableEnableConstraints Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableEnableConstraints", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

     ' ***************************************************************** '
     '
     ' Name:        BuildInsertStatement
     '
     ' Description: Processes to build a SQL statement to insert a row
     '              based on the data held in the export file
     '
     ' History:     30/08/2002 JB  - Created.
     '              04/10/2002 SJP - Modified to become a function, so that
     '                               can pass back if successful or not.
     '
     ' ***************************************************************** '
    Private Function BuildInsertStatement(ByVal v_aIeControl() As Object, ByVal v_aTableDefinition() As Object, ByVal v_aRetrievedData() As Object, ByRef r_sSQL As String, ByRef r_oDatabase As dPMDAO.Database) As Integer

         'Local definitions
        Dim result As Integer = 0
        Dim sSQLInsert As String = ""
        Dim sSQLValues As New StringBuilder
        Dim sSQLColumns As New StringBuilder
        Dim bMoreThanOneValue As Boolean
        Dim vNewRowID As Object

        Debug.WriteLine(Convert.ToString(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".BuildInsertStatement")

        

            result = gPMConstants.PMEReturnCode.PMTrue

             'Build a SQL update statement based on the array
            sSQLInsert = conEmptyString
            sSQLValues = New StringBuilder(conEmptyString)
            sSQLColumns = New StringBuilder(conEmptyString)
            bMoreThanOneValue = False
            sSQLInsert = sSQLInsert & "INSERT INTO "

            sSQLInsert = sSQLInsert & v_aIeControl(pbIeControl_objectName)

            sSQLColumns.Append("(")

             'Build a values statement with the returned data.
            sSQLValues.Append("VALUES (")

             'Set to indicate only one (or less) parameters
            bMoreThanOneValue = False

             'Need to add each element in the retrieved data array
            For iCounter As Integer = 0 To v_aRetrievedData.GetUpperBound(0)

                If iCounter = 0 Then
                     'Assume that any identity field will always be the first column, which
                     'may or may not be a 'real' identity

                    If 1 = 0 And gPMFunctions.ToSafeBoolean(v_aIeControl(pbIeControl_IsIdentity)) Then

                         'No work required.  Column is a real identity and as such
                         'will automatically be populated during the insert processing

                    Else

                        sSQLColumns.Append("[" & v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName) & "]")

                        If Convert.ToString(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnType)) = "varchar" Then

                            sSQLValues.Append("'" & v_aRetrievedData(iCounter) & "'")
                        Else

                             'Add the next available ID to the SQL string

                            vNewRowID = lRetrieveNewRowID(Convert.ToString(v_aIeControl(pbIeControl_objectName)), Convert.ToString(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName)), r_oDatabase, v_aRetrievedData(iCounter))

                            If IsDBNull(vNewRowID) Or IsNothing(vNewRowID) Then
                                sSQLValues.Append("NULL")
                            Else
                                sSQLValues.Append(vNewRowID)
                            End If
                        End If
                         'Set the flag to indicate a comma will be required on the end of
                         'the current string if we continue looping
                        bMoreThanOneValue = True

                    End If

                Else
                     'Add a comma to the end of the current line.  Only needed if more than one parameter
                     'is being written to the string

                     'can't do anything with timestamps

                    If Convert.ToString(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnType)) <> "timestamp" Then

                        If bMoreThanOneValue Then
                             'Append the comma & space character
                            sSQLColumns.Append(conComma & " ")
                            sSQLValues.Append(conComma & " ")
                        End If

                        sSQLColumns.Append("[" & v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName) & "]")

                         'Add the value to the string

                        sSQLValues.Append( _
                                          PrepareValueForSQLString(Convert.ToString(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnType)), v_aRetrievedData(iCounter)))

                         'Set the flag to indicate a comma will be required on the end of
                         'the current string if we continue looping
                        bMoreThanOneValue = True
                    End If
                End If

            Next iCounter

             'Add the final closing bracket
            sSQLValues.Append(")")
            sSQLColumns.Append(")")

             'Add all the string segments together to form a complete SQL string
            r_sSQL = conEmptyString
            r_sSQL = r_sSQL & sSQLInsert & sSQLColumns.ToString() & sSQLValues.ToString()

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".BuildInsertStatement")
            Return result

    End Function

     ' ***************************************************************** '
     '
     ' Name:        BuildUpdateStatement
     '
     ' Description: Processes to build a SQL statement to update a row
     '              based on the data held in the export file
     '
     ' History:     30/08/2002 JB - Created.
     '
     ' ***************************************************************** '
    Private Function BuildUpdateStatement(ByRef v_aIeControl() As Object, ByRef v_aTableDefinition() As Object, ByRef v_aRetrievedData() As Object, ByRef v_aAdjustedData() As Object, ByRef v_sSQL As String) As Integer

         'Local definitions
        Dim result As Integer = 0
        Dim sSQLUpdate As String = ""
        Dim sSQLValues As New StringBuilder
        Dim sSQLWhere As New StringBuilder
        Dim bMoreThanOneValue, bMoreThanOneWhere As Boolean

        

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".BuildUpdateStatement")

            result = gPMConstants.PMEReturnCode.PMTrue

             'Build a SQL update statement based on the array
            sSQLUpdate = conEmptyString
            sSQLUpdate = sSQLUpdate & "UPDATE "

            sSQLUpdate = sSQLUpdate & CStr(v_aIeControl(pbIeControl_objectName))
            sSQLUpdate = sSQLUpdate & " SET "

             'Build a values statement with the returned data.
            sSQLValues = New StringBuilder(" ")

             'Initialise the WHERE clause
            sSQLWhere = New StringBuilder(" WHERE ")

             'Set to indicate only one (or less) parameters & where clauses
            bMoreThanOneValue = False
            bMoreThanOneWhere = False

             'Need to add each element in the retrieved data array
            Dim iPos As Integer
            Dim sTestString1, sTestString2 As String
            For iCounter As Integer = 0 To v_aRetrievedData.GetUpperBound(0)

                 'Determine if the value being examined is a key field.  If so, add it to the
                 'WHERE clause, if not, add it to the VALUES clause

                If gPMFunctions.ToSafeString(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnType)) <> "timestamp" Then

                     'Setup the string to be checked
                    sTestString1 = conComma & gPMFunctions.ToSafeString(iCounter + 1) & conComma

                    sTestString2 = conComma & gPMFunctions.ToSafeString(v_aIeControl(pbIeControl_PrimaryKeyColumns)) & conComma

                    iPos = (sTestString2.IndexOf(sTestString1) + 1)

                     'If the test string appears in the primary key list
                    If iPos <> 0 Then

                         'Add an AND to the end of the current line.  Only needed if more than one parameter
                         'is Being written to the string
                        If bMoreThanOneWhere Then
                             'Append the AND
                            sSQLWhere.Append(" AND ")
                        End If

                         'Add the new WHERE clause

                        sSQLWhere.Append(gPMFunctions.ToSafeString(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName)))
                        sSQLWhere.Append(" = ")

                        sSQLWhere.Append( _
                                         PrepareValueForSQLString(gPMFunctions.ToSafeString(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnType)), gPMFunctions.ToSafeString(v_aRetrievedData(iCounter))))

                         'Set the flag to indicate a comma will be required on the end of
                         'the current string if we continue looping
                        bMoreThanOneWhere = True
                    End If

                    If CBool(v_aIeControl(pbIeControl_IsIdentity) And iCounter = 0) Then
                    Else

                         'Add a comma to the end of the current line.  Only needed if more than one parameter
                         'is Being written to the string
                        If bMoreThanOneValue Then
                             'Append the comma & space character
                            sSQLValues.Append(conComma & " ")
                        End If

                         'Add the field name to the string

                        sSQLValues.Append("[" & gPMFunctions.ToSafeString(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName)) & "]")

                         'Add the equal sign
                        sSQLValues.Append(" = ")

                         'Add the value to the string

                        sSQLValues.Append( _
                                          PrepareValueForSQLString(gPMFunctions.ToSafeString(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnType)), v_aAdjustedData(iCounter)))

                         'Set the flag to indicate a comma will be required on the end of
                         'the current string if we continue looping
                        bMoreThanOneValue = True
                    End If

                End If
            Next iCounter

             'Add all the string segments together to form a complete SQL string
            v_sSQL = conEmptyString
            v_sSQL = v_sSQL & sSQLUpdate & sSQLValues.ToString() & sSQLWhere.ToString()

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".BuildUpdateStatement")
            Return result

    End Function

     ' ***************************************************************** '
     '
     ' Name:        PrepareValueForSQLString
     '
     ' Description: Processes to prepare a binary file element for inclusion
     '              in a SQL statement, based on the type of element being
     '              considered
     '
     ' History:     30/08/2002 JB - Created.
     '
     ' ***************************************************************** '
    Private Function PrepareValueForSQLString(ByVal sElementType As String, ByVal sElement As Object) As String

        Dim result As Object
        

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".PrepareValueForSQLString")

             'Determine the type expected for the column

            Select Case sElementType
                Case gSTRING, gCHAR, gVARCHAR, gTEXT, gCAPTIONTEXT

                    If IsDBNull(sElement) Or IsNothing(sElement) Then
                        result = "NULL"
                    Else
                        result = "'" & ProcessCaption(sElement) & "'"
                    End If

                Case gDATETIME

                    If IsDBNull(sElement) Or IsNothing(sElement) Then
                        result = "NULL"
                    Else
                         'need to manipulate the date to the correct format

                        result = "Convert(datetime, '" & StringsHelper.Format(sElement, "yyyy\-mm\-dd hh\:nn\:ss") & "', 120)"
                    End If

                Case gINTEGER, gTINYINT

                    If IsDBNull(sElement) Or IsNothing(sElement) Then
                        result = "NULL"
                    Else
                         'need to manipulate the date to the correct format
                        result = sElement
                    End If

                Case gMONEY, gNUMERIC

                    If IsDBNull(sElement) Or IsNothing(sElement) Then
                        result = "NULL"
                    Else
                         'need to manipulate the date to the correct format
                        result = sElement
                    End If
                Case gTIMESTAMP
                    result = 0

                Case Else

                    If IsDBNull(sElement) Or IsNothing(sElement) Then
                        result = "NULL"
                    Else
                         'need to manipulate the date to the correct format
                        result = sElement
                    End If

            End Select

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".PrepareValueForSQLString")
            Return result

    End Function

     ' ***************************************************************** '
     '
     ' Name:        AdjustRetrievedData
     '
     ' Description: Proces to adjust the retrieved file data prior
     '              to it being added to the target database via SQL
     '
     ' History:     30/08/2002 JB - Created.
     '
     ' ***************************************************************** '
    Private Function AdjustRetrievedData(ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByRef r_aRetrievedData As Object, ByRef r_aAdjustedData As Object, ByRef r_oDatabase As dPMDAO.Database, ByRef r_sErrorText As String) As Integer 
        Dim result As Integer = 0 
        Dim lRetVal As gPMConstants.PMEReturnCode 

         'Define local variables
        Dim aParentPK As Object 
        Static lDefaultInsurerId As Integer

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".AdjustRetrievedData")
        
            result = gPMConstants.PMEReturnCode.PMTrue

             'Reset the adjusted data array

            r_aAdjustedData = conEmptyString

            r_aAdjustedData = r_aRetrievedData

             'Have received a whole row, so need to loop through each column idenitifed in
             'the table definition looking for specific types

             '*************************************************************************************
             'Check and replace and Ids with the previous parent id,if the row we're
             'currently on is a child row.

            If (r_aIeControl(pbIeControl_objectType)) = pbIeOt_dbTable_child Then

                m_lReturn = CType(ReplaceIDs(r_aAdjustedData:=r_aAdjustedData, r_aIeTableDefinitions:=r_aIeTableDefinitions, r_aIeControl:=r_aIeControl), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sErrorText = "ReplaceIDs"
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

             '*************************************************************************************
             'Check and replace the Data Model Id value
             'Must not do this if the object is GIS_DATA_MODEL as we will be looking
             'up and changing the SAME element on the SAME table.
             'only do it if the current value is > 0, gis_screens have 0 DMID on child screens
             '

            If (r_aIeControl(pbIeControl_DataModelIdColumn)) <> 0 Then

                If CStr(r_aIeControl(pbIeControl_objectName)) <> "gis_data_model" And (r_aRetrievedData((r_aIeControl(pbIeControl_DataModelIdColumn)) - 1)) > 0 Then

                    m_lReturn = CType(ReplaceDataModelId(r_aIeTableDefinitions:=r_aIeTableDefinitions(pbIeTableDefinitions_columnArray), r_aData:=r_aAdjustedData, r_oDatabase:=r_oDatabase, v_iCounter:=CInt(r_aIeControl(pbIeControl_DataModelIdColumn))), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sErrorText = "ReplaceDataModelId"
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

             '*************************************************************************************
             'Check and replace the Caption Ids for the caption supplied.  Assumed that
             'the caption is always.....

            If CBool(r_aIeControl(pbIeControl_Flags) And pbIeControl_Flags__IsCaption) Then

                m_lReturn = CType(ReplaceCaptionId(r_aIeTableDefinitions:=r_aIeTableDefinitions, r_aRetrievedData:=r_aRetrievedData, r_aAdjustedData:=r_aAdjustedData, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sErrorText = "ReplaceCaptionId"
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

             '************* replace any created_by_id or modified_by_id values with the sirius user id ************

            Dim iColumnId As Integer 
            If CBool(r_aIeControl(pbIeControl_Flags) And pbIeControl_Flags__CreatedByOrModifiedBy) Then
                iColumnId = findColumn("created_by_id", r_aIeTableDefinitions)
                If iColumnId <> -1 Then

                    r_aAdjustedData(iColumnId) = g_lSiriusUserId
                End If
                iColumnId = findColumn("modified_by_id", r_aIeTableDefinitions)
                If iColumnId <> -1 Then

                    r_aAdjustedData(iColumnId) = g_lSiriusUserId
                End If
            End If

             '************* RESET THE GLOBAL VARIABLES IF REQUIRED ************************

             'If we have imported the GIS_Data_Model table, set global variables for later
             'use by the import processing should only run the first time

            If CStr(r_aIeControl(pbIeControl_objectName)) = "gis_data_model" Then

                m_lReturn = CType(RetrieveDataModelReplacementValues(r_oDatabase:=r_oDatabase, r_aRetrievedData:=r_aRetrievedData, r_iCounter:=0), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sErrorText = "RetrieveDataModelReplacementValues"
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

             '************* RESET gis_insurer_id to 0 ************************
             'we cant import insurers so fix it up to the default one

            Dim vResults(,) As Object 
            Dim sSQL As String = "" 
            Dim lCaptionId As Integer 
            If (r_aIeControl(pbIeControl_GisInsurerIdColumn)) <> 0 Then
                If lDefaultInsurerId = 0 Then
                    r_oDatabase.SQLSelect(sSQL:="select gis_insurer_id from gis_insurer where code ='DEFAULT'", sSQLName:="AdjustRetrievedData find default insurer", bStoredProcedure:=False, vResultArray:=vResults)
                    If Information.IsArray(vResults) Then

                        lDefaultInsurerId = CInt(vResults(0, 0))
                    Else
                         'create default insurer

                        With r_oDatabase
                            With .Parameters
                            .Clear() 'Clear any existing parameters and add the required parameters
                                .Add(sName:="language_id", vValue:=CStr(1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                                .Add(sName:="caption", vValue:="DEFAULT", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                                .Add(sName:="caption_id", vValue:=CStr(9), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
                            End With

                             'Retrieve the appropriate caption_id for the description
                            lRetVal = .SQLSelect(sSQL:=ACGetPMCaptionSQL, sSQLName:=ACGetPMCaptionName, bStoredProcedure:=ACGetPMCaptionStored, vResultArray:=vResults)
                            lCaptionId = r_oDatabase.Parameters.Item("caption_id").Value

                             'get the next value for gis_insurer
                            .SQLSelect(sSQL:="select max(gis_insurer_id)+1 from gis_insurer", sSQLName:="gen next gis_insure_id", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)

                            lDefaultInsurerId = CInt(vResults(0, 0))

                        End With

                        sSQL = "insert into gis_insurer (gis_insurer_id,code,caption_id,description,is_deleted,effective_date) values ()"
                        bPMAddParameter.AddParameter(r_oDatabase, sSQL, lRetVal, "gis_insurer_id", lDefaultInsurerId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, 2)
                        bPMAddParameter.AddParameter(r_oDatabase, sSQL, lRetVal, "code", "DEFAULT", gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, 2)
                        bPMAddParameter.AddParameter(r_oDatabase, sSQL, lRetVal, "caption_id", lCaptionId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, 2)
                        bPMAddParameter.AddParameter(r_oDatabase, sSQL, lRetVal, "description", "DEFAULT", gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, 2)
                        bPMAddParameter.AddParameter(r_oDatabase, sSQL, lRetVal, "is_deleted", 0, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, 2)
                        bPMAddParameter.AddParameter(r_oDatabase, sSQL, lRetVal, "effective_date", DateTime.Now, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate, 2)

                        lRetVal = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="CreateDefaultGISInsurer", bStoredProcedure:=False)

                    End If

                     'find or create default insurer
                End If

                r_aAdjustedData(findColumn("gis_insurer_id", r_aIeTableDefinitions)) = lDefaultInsurerId
            End If

             '******************************************************************************
             'Save the current PK value for later use with any child properties

            If (r_aIeControl(pbIeControl_objectType)) <> pbIeOt_UserDefinedList Then
                Try

                    aParentPK = g_cParentPK("PK_" & CStr(r_aIeControl(pbIeControl_objectId)))

                Catch
                End Try

                If Not Object.Equals(aParentPK, Nothing) Then

                    g_cParentPK.Remove("PK_" & CStr(r_aIeControl(pbIeControl_objectId)))
                End If

                g_cParentPK.Add(New Object() {r_aRetrievedData(conPKColumn), r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(conPKColumn)(pbIeTableDefinitions_columnName)}, "PK_" & CStr(r_aIeControl(pbIeControl_objectId)))
            End If
             '******************************************************************************

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".AdjustRetrievedData")
            Return result


    End Function

     ' ***************************************************************** '
     '
     ' Name:        ReplaceDataModelId
     '
     ' Description: Proces to replace a data_model_id from a source box with
     '              its equivalent data_model_id from the target box
     '
     ' History:     30/08/2002 JB - Created.
     '
     ' ***************************************************************** '
    Private Function ReplaceDataModelId(ByRef r_aIeTableDefinitions As Object, ByRef r_aData() As Object, ByRef r_oDatabase As dPMDAO.Database, ByVal v_iCounter As Integer) As Integer

        Dim result As Integer = 0
        

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ReplaceDataModelId")

            result = gPMConstants.PMEReturnCode.PMTrue

             'Just set the appropriate values to the global variables defined when the
             'GIS_Data_Model was loaded.  The retrieved data is a zero based array, but the
             'control array is 1 based so need to adjust the column value accordingly by
             'subtracting 1

            r_aData(v_iCounter - 1) = g_DataModelId

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".ReplaceDataModelId")
            Return result

    End Function

     ' ***************************************************************** '
     '
     ' Name:        ReplaceCaptionId
     '
     ' Description: Proces to replace a caption_id from a source box with
     '              its equivalent caption_id from the target box
     '
     ' History:     30/08/2002 JB - Created.
     '
     ' ***************************************************************** '
    Private Function ReplaceCaptionId(ByRef r_aIeTableDefinitions() As Object, ByRef r_aRetrievedData() As Object, ByRef r_aAdjustedData() As Object, ByRef r_oDatabase As dPMDAO.Database) As Integer 

         'Define local variables
        Dim result As Integer = 0 
        Dim r_vResults(,) As Object 

        

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ReplaceCaptionId")

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim iCaptionId, iCaptionDescription As Integer 

             'Check if the current data definition is the one required
             'by the control. The retrieved data is a zero based array, but the
             'control array is 1 based so need to adjust the column value accordingly by
             'subtracting 1

            iCaptionId = findColumn("caption_id", r_aIeTableDefinitions)
            iCaptionDescription = findColumn("description", r_aIeTableDefinitions)
            If iCaptionId = -1 Or iCaptionDescription = -1 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

             'UDL captions can be NULL! so fix up

            If IsDBNull(r_aRetrievedData(iCaptionDescription)) Or IsNothing(r_aRetrievedData(iCaptionDescription)) Or Object.Equals(r_aRetrievedData(iCaptionDescription), Nothing) Then

                r_aRetrievedData(iCaptionDescription) = "No description"
            End If

             'Use the received database connection
            With r_oDatabase
                With .Parameters
                     'Clear any existing parameters
                    .Clear()

                     'Add the required parameters
                    m_lReturn = .Add(sName:="language_id", vValue:=CStr(1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                    m_lReturn = .Add(sName:="caption", vValue:=CStr(r_aRetrievedData(iCaptionDescription)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    m_lReturn = .Add(sName:="caption_id", vValue:=CStr(9), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
                End With
                 'Check that the parameters have ben successfully added
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                     ' Log Error Message
                    result = gPMConstants.PMEReturnCode.PMFalse

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Set Parameter failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AdjustRetrievedData")

                    Return result
                End If

                 'Retrieve the appropriate caption_id for the description
                m_lReturn = .SQLSelect(sSQL:=ACGetPMCaptionSQL, sSQLName:=ACGetPMCaptionName, bStoredProcedure:=ACGetPMCaptionStored, vResultArray:=r_vResults)

                 'Ensure that the query ran successfully
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With
             'Update the array with the correct value returned in the parameter

            r_aAdjustedData(iCaptionId) = r_oDatabase.Parameters.Item("caption_id").Value

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".ReplaceCaptionID")
            Return result

    End Function

     ' ***************************************************************** '
     '
     ' Name:        ReplaceIDs
     '
     ' Description:
     '
     ' History:     ??/??/???? JB - Created.
     '
     ' ***************************************************************** '
    Private Function ReplaceIDs(ByRef r_aAdjustedData() As Object, ByRef r_aIeTableDefinitions() As Object, ByRef r_aIeControl() As Object) As Integer

        Dim result As Integer = 0
        

            Dim aParentPK As Object
            Dim iParentColumn As Integer

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ReplaceIDs")

            result = gPMConstants.PMEReturnCode.PMTrue

            aParentPK = g_cParentPK("PK_" & CStr(r_aIeControl(pbIeControl_RelatedObjectId)))

            iParentColumn = findColumn(aParentPK(1), r_aIeTableDefinitions)
            If iParentColumn <> -1 Then

                r_aAdjustedData(iParentColumn) = aParentPK(0)
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".ReplaceIDs")
            Return result

    End Function

     ' ***************************************************************** '
     '
     ' Name:        RetrieveDataModelReplacementValues
     '
     ' Description:
     '
     ' History:     ??/??/???? JB - Created.
     '
     ' ***************************************************************** '
    Private Function RetrieveDataModelReplacementValues(ByRef r_oDatabase As dPMDAO.Database, ByRef r_aRetrievedData() As Object, ByRef r_iCounter As Integer) As Integer 

         'Define the local variables
        Dim result As Integer = 0 
        Dim sSQL As String = "" 
        Dim r_vResults(,) As Object 

        

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".RetrieveDataModelReplacementValues")

            result = gPMConstants.PMEReturnCode.PMTrue

             'Setup the SQL for retreival
            sSQL = conEmptyString
            sSQL = sSQL & "SELECT gis_data_model_id, code FROM gis_data_model "

            sSQL = sSQL & "WHERE code = '" & r_aRetrievedData(r_iCounter + 1) & "'"

             'get the column information for this table
            m_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="genericEnquire", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResults)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(r_vResults) Then
                 'Set the global vairables for use by the general replacement sub

                g_DataModelId = CInt(r_vResults(0, 0))

                g_DataModelCode = CStr(r_vResults(1, 0))
            Else
                 'Set the global vairables to the retrieved values, as the
                 'data model does not exist on the target machine

                g_DataModelId = CInt(r_aRetrievedData(pbIeControl_objectId))

                g_DataModelCode = CStr(r_aRetrievedData(pbIeControl_objectName))
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".RetrieveDataModelReplacementValues")
            Return result

    End Function

     ' ***************************************************************** '
     '
     ' Name:        RetrieveNewRowID
     '
     ' Description: Function to retrieve the next maximum identity value
     '              for the supplied table
     '
     ' History:     30/08/2002 JB - Created.
     '
     ' ***************************************************************** '
    Private Function lRetrieveNewRowID(ByVal v_sTableName As String, ByVal v_sColumnName As String, ByRef r_oDatabase As dPMDAO.Database, ByVal lOriginalId As Object) As Object 

        Dim result As Object 
        Dim sSQL As String = "" 
        Dim r_vResults(,) As Object 

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".lRetrieveNewRowID")

        

            If g_CloneDbKeys Then
                result = lOriginalId
            Else
                 'Setup the SQL for retreival
                sSQL = "Select max(" & v_sColumnName & ") from " & v_sTableName

                 'get the column information for this table
                m_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="genericEnquire", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResults)

                 'Return the next available ID

                result = (r_vResults(0, 0)) + 1
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".lRetrieveNewRowID")
            Return result

    End Function

     ' ***************************************************************** '
     '
     ' Name:        ExistsOnTarget
     '
     ' Description: Function to determine if a table already exists in the
     '              target database
     '
     ' History:     30/08/2002 JB - Created.
     '
     ' ***************************************************************** '
    Public Function ExistsOnTarget(ByRef r_oDatabase As dPMDAO.Database, ByVal v_sTableName As String, ByRef r_bExistsOnTarget As Boolean) As Integer 

         'Define local variables
        Dim result As Integer = 0 
        Dim sSQL As String = "" 
        Dim vResults(,) As Object 

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ExistsOnTarget")

        Try

             'Set the default value
            result = gPMConstants.PMEReturnCode.PMTrue

             'Build the SQL
            sSQL = conEmptyString
            sSQL = conEmptyString
            sSQL = "select name from sysobjects where name like upper('" & v_sTableName.Trim() & "')"

             'Execute the SQL
            m_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="genericEnquire", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_bExistsOnTarget = Information.IsArray(vResults)

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".ExistsOnTarget")
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".ExistsOnTarget")

             ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExistsOnTarget Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExistsOnTarget", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

     ' ***************************************************************** '
     '
     ' Name:        ProcessCaption
     '
     ' Description:
     '
     ' History:     ??/??/???? ??? - Created.
     '
     ' ***************************************************************** '
    Private Function ProcessCaption(ByVal v_sCaption As String) As String

        Dim result As String = String.Empty
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ProcessCaption")

        

             'DC010705 PN22076 do not strip spaces from caption in case they were intended to be there
            result = v_sCaption.Replace("'", "''")

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".ProcessCaption")
            Return result

    End Function
     ' ***************************************************************** '
     '
     ' Name: AddWhereClauseItem
     '
     ' Description:
     '
     ' History: 11/10/2002 CLG - Created.
     '
     ' ***************************************************************** '
    Private Function AddWhereClauseItem(ByVal v_vColumnName As String, ByVal v_vDataItem As Object, ByVal v_sColumnType As String) As String

        Dim result As String = String.Empty
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".AddWhereClauseItem")

        

            result = result & " and " & v_vColumnName

             'Handle values with single quotes inside

            v_vDataItem = Convert.ToString(v_vDataItem).Replace("'", "''")

            If IsDBNull(v_vDataItem) Or IsNothing(v_vDataItem) Then
                result = result & " is null"
            Else
                If v_sColumnType = "datetime" Then

                    result = result & "=" & "Convert(datetime, '" & StringsHelper.Format(v_vDataItem, "yyyy\-mm\-dd hh\:nn\:ss") & "', 120)"
                Else

                    result = result & "='" & Convert.ToString(v_vDataItem) & "'"
                End If

                 'tbd remove this

                Select Case v_vDataItem.GetType().Name
                    Case "Date"
                         'AddWhereClauseItem = AddWhereClauseItem & "=" & "Convert(datetime, '" & Format$(v_vDataItem, "yyyy\-mm\-dd hh\:nn\:ss") & "', 120)"

                    Case Else
                         'AddWhereClauseItem = AddWhereClauseItem & "='" & v_vDataItem & "'"
                End Select
            End If
            result = result & Strings.Chr(13) & Strings.Chr(10)

            Debug.WriteLine(Convert.ToString(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".AddWhereClauseItem")

            Return result

    End Function
     ' Converts a variant into the best form for an SQL statement.
     ' NOT TO BE USED FOR ANY OTHER PURPOSE
    Public Function ToSQL(ByVal vValue As Object) As String

         ' Valueless types
        Dim result As String = String.Empty
        Const VT_EMPTY As VariantType = VariantType.Empty ' vbEmpty
        Const VT_NULL As VariantType = VariantType.Null ' vbNull
         ' Single values
        Const VT_BOOL As VariantType = VariantType.Boolean ' vbBoolean
        Const VT_I1 As VariantType = 16
        Const VT_UI1 As VariantType = VariantType.Byte ' vbByte
        Const VT_I2 As VariantType = VariantType.Short ' vbInteger
        Const VT_UI2 As VariantType = 18
        Const VT_I4 As VariantType = VariantType.Integer ' vbLong
        Const VT_UI4 As VariantType = 19
        Const VT_INT As VariantType = 22
        Const VT_UINT As VariantType = 23
        Const VT_I8 As VariantType = 20
        Const VT_UI8 As VariantType = 21
        Const VT_R4 As VariantType = VariantType.Single ' vbSingle
        Const VT_R8 As VariantType = VariantType.Double ' vbDouble
        Const VT_CY As VariantType = VariantType.Decimal ' vbCurrency
        Const VT_DECIMAL As VariantType = VariantType.Decimal ' vbDecimal
        Const VT_DATE As VariantType = VariantType.Date ' vbDate
        Const VT_BSTR As VariantType = VariantType.String ' vbString
         ' Not interpretable
        Const VT_ERROR As Integer = 10
        Const VT_VARIANT As Integer = 12
        Const VT_FILETIME As Integer = 64
        Const VT_LPSTR As Integer = 30
        Const VT_LPWSTR As Integer = 31
        Const VT_CLSID As Integer = 72
        Const VT_CF As Integer = 71
        Const VT_BLOB As Integer = 65
        Const VT_BLOBOBJECT As Integer = 70
        Const VT_STREAM As Integer = 66
        Const VT_STREAMED_OBJECT As Integer = 68
        Const VT_STORAGE As Integer = 67
        Const VT_STORED_OBJECT As Integer = 69
         ' Structured data flags
        Const VT_TYPEMASK As Integer = &HFFFS
        Const VT_VECTOR As Integer = &H1000S
        Const VT_ARRAY As Integer = &H2000S ' vbArray
        Const VT_BYREF As Integer = &H4000S

        Dim sFormat As String = ""
        Dim sValue As New StringBuilder
        Dim iByteFirst, iByteLast As Integer

         ' Copy input value (see module-level comment).

        Dim vCopy As Object = vValue

         ' The vartype() function actually returns the value of the
         ' internal VARIANT structure type field. To accommodate
         ' code that passes in variants of a type unsupported by VB,
         ' we must interpret undocumented types as well.

        Select Case Information.VarType(vCopy)
            Case VT_NULL, VT_EMPTY
                result = "null"
            Case VT_BOOL
                 ' this matches the BOOLEAN type definition in the database

                result = IIf(CBool(vCopy), "1", "0")
            Case VT_I1, VT_UI1, VT_I2, VT_UI2, VT_I4, VT_UI4, VT_INT, VT_UINT, VT_I8, VT_UI8, VT_R4, VT_R8, VT_CY, VT_DECIMAL
                 ' use locate-independent string conversion
                result = Microsoft.VisualBasic.Conversion.Str(vCopy).TrimStart()
            Case VT_DATE
                 ' handle both dates and times, and use locale-independent format

                If CDate(vCopy) = DateTime.Parse(CStr(vCopy)) Then
                    sFormat = "{\d'yyyy\-mm\-dd'}"
                ElseIf CDate(vCopy) = CDate(vCopy) Then
                    sFormat = "{\t'hh\:nn\:ss'}"
                Else
                    sFormat = "{\t\s'yyyy\-mm\-dd hh\:nn\:ss'}"
                End If

                result = StringsHelper.Format(vCopy, sFormat)
            Case VT_BSTR
                 ' must handle quotes properly

                vCopy = CStr(vCopy).Replace("'", "''")

                result = "'" & CStr(vCopy) & "'"
            Case VT_ARRAY + VT_I1, VT_ARRAY + VT_UI1
                 ' translate a byte array into a binary literal
                On Error Resume Next

                iByteFirst = vCopy.GetLowerBound(0)

                iByteLast = vCopy.GetUpperBound(0)
                sValue = New StringBuilder("")
                If Information.Err().Number = 0 Then
                    sValue = New StringBuilder("0x")
                    For iByte As Integer = iByteFirst To iByteLast

                        sValue.Append(("00" & CInt(vCopy(iByte)).ToString("X")).Substring(("00" & CInt(vCopy(iByte)).ToString("X")).Length - 2))
                    Next
                Else
                    Information.Err().Clear()
                End If
                result = sValue.ToString()
            Case Else
                Throw New System.Exception("13, ToSQL, Type Mismatch (cannot convert to SQL).")
        End Select

        Return result
    End Function
End Module
