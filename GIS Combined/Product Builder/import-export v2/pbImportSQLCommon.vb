Option Strict Off
Option Explicit On
Imports SharedFiles

Module pbImportSQLCommon

    Private Const ACClass As String = conEmptyString

    Private m_lReturn As Integer

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
    Public Function PrepareSQLStatements(ByRef r_oDatabase As dPMDAO.Database, ByRef r_aIeControl As Object, ByRef r_aIeTableDefinitions As Object, ByVal r_iTableIndex As Short, ByRef r_aRetrievedData As Object, ByRef r_sErrorString As String) As Integer

        'Define local variables
        Dim sInsertSQL As String
        Dim sUpdateSQL As String
        Dim sSQLSearch As String
        Dim sSQLWhereClause As String
        Dim sErrorText As String

        Dim lRecordsAffected As Integer
        Dim aAdjustedData As Object
        Dim vExistingData(,) As Object
        Dim aMyArray() As String
        Dim iLoop As Short
        Dim bUpdateRequired As Boolean
        Dim aParentPK As Object
        Static sDeleteTable As String
        Static sDeleteSQL As String

        'Richard Clarke November 2008 - PIE enhancements
        Dim bGUIDAndPKMatch As Boolean
        Dim bPKOnlyMatch As Boolean
        Dim sSQLPreGUIDSearch As String
        Dim bNoGUID As Boolean
        Dim bNoGUIDOnTarget As Boolean
        Dim sSourceGUID As String
        Dim sTargetGUID As String
        Dim bNewIDSuccess As Boolean
        Dim vExistingGUIDData(,) As Object
        Dim bGUIDNotOnTarget As Boolean
        Dim vExistingScreenDetailData As Object
        Dim vExistingGISFindMappingData(,) As Object
        Dim iFindColumn As Integer
        Dim bSetNewId As Boolean
        Dim sSQLDelete As String
        'Richard Clarke November 2008 - PIE enhancements

        ' Debug message
        'Debug.Print Timer & ": Entering " & ACApp & conDot & ACClass & ".PrepareSQLStatements"
        On Error GoTo Err_PrepareSQLStatements
        PrepareSQLStatements = gPMConstants.PMEReturnCode.PMTrue

        'If r_aIeControl(pbIeControl_objectName) = "gis_list_type_usage" Then
        '   'Debug.Print "testing"
        'End If


        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(pbIeControl_objectName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If r_aIeControl(pbIeControl_objectName) = "gis_property" OrElse r_aIeControl(pbIeControl_objectName) = "gis_screen" Then
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If LCase(r_aRetrievedData(2)) = "prop_built" Then
            End If
        End If

        '    ' BSJ Oct 2009 ignore numbering scheme if multiple source
        '    If (g_bNukeDataModel = True And (LCase(r_aIeControl(pbIeControl_objectName)) = LCase("peril_type_usage"))) Then
        '        Exit Function
        '    End If
        ' JB Oct 2009 ignore numbering scheme
        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If (LCase(r_aIeControl(pbIeControl_objectName)) = LCase("numbering_scheme") OrElse LCase(r_aIeControl(pbIeControl_objectName)) = LCase("PMUser_Authority_Level")) Then
            Exit Function
        End If

        ''Debug.Print r_aIeControl(pbIeControl_objectName)
        'Need to adjust the retrieved row to cater for all the 'special' columns.  Need to do
        'it before the SQL statements are built, as the info is held on the control array
        m_lReturn = AdjustRetrievedData(r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, r_aRetrievedData:=r_aRetrievedData, r_aAdjustedData:=aAdjustedData, r_oDatabase:=r_oDatabase, r_sErrorText:=sErrorText)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            PrepareSQLStatements = gPMConstants.PMEReturnCode.PMFalse
            'UPGRADE_WARNING: Couldn't resolve default property of object aAdjustedData(0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            r_sErrorString = "Adjust Retrieved Data (" & sErrorText & ") failed for: " & r_aIeControl(pbIeControl_objectName) & "(" & aAdjustedData(0) & ")"
            Exit Function
        End If

        'see if we need to run a previously created delete command
        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(pbIeControl_objectName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If sDeleteSQL <> "" AndAlso (r_aIeControl(pbIeControl_Flags) <> pbIeControl_Flags__deleteBeforeAdd0 OrElse sDeleteTable <> r_aIeControl(pbIeControl_objectName)) Then
            sDeleteSQL = Left(sDeleteSQL, Len(sDeleteSQL) - 1) & ")"
            m_lReturn = r_oDatabase.SQLAction(sSQL:=sDeleteSQL, sSQLName:="delete extra entries", bStoredProcedure:=False, lRecordsAffected:=lRecordsAffected)
            sDeleteSQL = ""
            sDeleteTable = ""
        End If

        'see if we need to start or add to a delete command
        If r_aIeControl(pbIeControl_Flags) = pbIeControl_Flags__deleteBeforeAdd0 Then
            If sDeleteSQL = "" Then
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                sDeleteTable = r_aIeControl(pbIeControl_objectName)
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(1)(pbIeTableDefinitions_columnName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object aAdjustedData(0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(0)(pbIeTableDefinitions_columnName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                sDeleteSQL = "delete from " & r_aIeControl(pbIeControl_objectName) & " where " & r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(0)(pbIeTableDefinitions_columnName) & "=" & aAdjustedData(0) & " and " & r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(1)(pbIeTableDefinitions_columnName) & " not in ("
            End If
            'UPGRADE_WARNING: Couldn't resolve default property of object aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            sDeleteSQL = sDeleteSQL & aAdjustedData(1) & ","
        End If

        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        aMyArray = Split(r_aIeControl(pbIeControl_PrimaryKeyColumns), conComma)

        'Determine if the table consists of no keys whatsoever.  If so, abandon
        'the update attempt as it will not work without keys or values.
        'On Error Resume Next
        iLoop = (Len(aMyArray(0)) = 0)
        If Err.Number = 9 Then
            'Subscript out of range caused by aMyArray being an array with no elements'
            'This means that the split function returned no keys so set accordingly.
            For iLoop = 0 To UBound(r_aIeTableDefinitions)
                addToArray(aMyArray, iLoop + 1)
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                r_aIeControl(pbIeControl_PrimaryKeyColumns) = r_aIeControl(pbIeControl_PrimaryKeyColumns) & iLoop + 1 & ","
            Next
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            r_aIeControl(pbIeControl_PrimaryKeyColumns) = Left(r_aIeControl(pbIeControl_PrimaryKeyColumns), Len(r_aIeControl(pbIeControl_PrimaryKeyColumns)) - 1)
            Err.Clear()
        End If

        iFindColumn = findColumn(g_PIEGuidCol, r_aIeTableDefinitions)
        'On Error GoTo 0

        sSQLWhereClause = " where 1=1 " & vbCrLf
        'if table has an identity and is not a child
        'this if is never executed
        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(pbIeControl_IsIdentity). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        Dim aColumns As Object
        Dim iColumnFilterLoop As Short
        If 1 = 0 AndAlso r_aIeControl(pbIeControl_IsIdentity) = True Then
            'sSQLSearch = "SELECT " & r_aIeTableDefinitions(pbIeTableDefinitions_exportedColumns) & " FROM " & r_aIeControl(pbIeControl_objectName) & _
            ''   aColumns = Split(r_aIeTableDefinitions(pbIeTableDefinitions_exportedColumns), ",")
            ' BSJ Sep 2009 wrap each select column with [] because some have spaces in them (RGICL's UDL for example)
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(pbIeControl_objectName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            sSQLSearch = "SELECT " & "[" & Replace(r_aIeTableDefinitions(pbIeTableDefinitions_exportedColumns), ",", "],[") & "]" & " FROM " & r_aIeControl(pbIeControl_objectName)
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Couldn't resolve default property of object aColumns. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            aColumns = Split(r_aIeTableDefinitions(pbIeTableDefinitions_exportedColumns), ",")
            For iColumnFilterLoop = 0 To UBound(aColumns) - 1
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(iColumnFilterLoop + 1)(pbIeTableDefinitions_columnName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(iColumnFilterLoop + 1)(pbIeTableDefinitions_columnName) <> "caption_id" Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(iColumnFilterLoop + 1)(pbIeTableDefinitions_columnType). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions()()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sSQLWhereClause = sSQLWhereClause & AddWhereClauseItem(r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(iColumnFilterLoop + 1)(pbIeTableDefinitions_columnName), aAdjustedData(iColumnFilterLoop + 1), r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(iColumnFilterLoop + 1)(pbIeTableDefinitions_columnType))

                End If
            Next
        Else
            ' BSJ Sep 2009 wrap each select column with [] because some have spaces in them (RGICL's UDL for example)
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(pbIeControl_objectName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            sSQLSearch = "SELECT " & "[" & Replace(r_aIeTableDefinitions(pbIeTableDefinitions_exportedColumns), ",", "],[") & "]" & " FROM " & r_aIeControl(pbIeControl_objectName) & vbCrLf

            'JB Apr 10 - gis_property contains composite primary key as gis_property_id and gis_object_id. And this combinations is always
            'unique but while inserting the data primary key contraint has been disabled. So while inserting the data we used to search for the
            'existing property on the basis of gis_property_id and gis_object_id now i have changed the search only on the basis of candidate key
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If Trim(LCase(r_aIeControl(pbIeControl_objectName))) = "gis_property" Then
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(aMyArray(0) - 1)(pbIeTableDefinitions_columnName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(CDbl(aMyArray(0)) - 1)(pbIeTableDefinitions_columnName) = "gis_object_id" Then
                    sSQLWhereClause = sSQLWhereClause
                Else
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(aMyArray(0) - 1)(pbIeTableDefinitions_columnType). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions()()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sSQLWhereClause = sSQLWhereClause & AddWhereClauseItem(r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(CDbl(aMyArray(0)) - 1)(pbIeTableDefinitions_columnName), aAdjustedData(CDbl(aMyArray(0)) - 1), r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(CDbl(aMyArray(0)) - 1)(pbIeTableDefinitions_columnType))
                End If
            Else
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(aMyArray(0) - 1)(pbIeTableDefinitions_columnType). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions()()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If aMyArray(0) <> "" Then
                    sSQLWhereClause = sSQLWhereClause & AddWhereClauseItem(r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(CDbl(aMyArray(0)) - 1)(pbIeTableDefinitions_columnName), aAdjustedData(CDbl(aMyArray(0)) - 1), r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(CDbl(aMyArray(0)) - 1)(pbIeTableDefinitions_columnType))
                End If
            End If

            'If UBound(aMyArray) > 0 Then
            For iLoop = 1 To UBound(aMyArray)
                'JB Apr 10 - gis_property contains composite primary key as gis_property_id and gis_object_id. And this combinations is always
                'unique but while inserting the data primary key contraint has been disabled. So while inserting the data we used to search for the
                'existing property on the basis of gis_property_id and gis_object_id now i have changed the search only on the basis of candidate key
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(r_aIeControl(pbIeControl_objectName))) = "gis_property" Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(aMyArray(iLoop) - 1)(pbIeTableDefinitions_columnName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(CDbl(aMyArray(iLoop)) - 1)(pbIeTableDefinitions_columnName) = "gis_object_id" Then
                        sSQLWhereClause = sSQLWhereClause
                    Else
                        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(aMyArray(iLoop) - 1)(pbIeTableDefinitions_columnType). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions()()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        sSQLWhereClause = sSQLWhereClause & AddWhereClauseItem(r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(CDbl(aMyArray(iLoop)) - 1)(pbIeTableDefinitions_columnName), aAdjustedData(CDbl(aMyArray(iLoop)) - 1), r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(CDbl(aMyArray(iLoop)) - 1)(pbIeTableDefinitions_columnType))
                    End If
                Else
                    sSQLWhereClause = sSQLWhereClause & AddWhereClauseItem(r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(CDbl(aMyArray(iLoop)) - 1)(pbIeTableDefinitions_columnName), aAdjustedData(CDbl(aMyArray(iLoop)) - 1), r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(CDbl(aMyArray(iLoop)) - 1)(pbIeTableDefinitions_columnType))
                End If
            Next iLoop
            'End If

            '---------------------------------
            'Richard Clarke November 2008 - PIE Enhancements
            sSQLPreGUIDSearch = sSQLSearch & sSQLWhereClause
            'need to get the pie_guid column added to this check
            'add the PIE guid column to the check (note - it's not part of the PK so isn't added in above loop)
            If iFindColumn <> -1 Then
                'add the guid to the where clause
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(findColumn(g_PIEGuidCol, r_aIeTableDefinitions))(pbIeTableDefinitions_columnType). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                sSQLWhereClause = sSQLWhereClause & AddWhereClauseItem(g_PIEGuidCol, aAdjustedData(iFindColumn), r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(iFindColumn)(pbIeTableDefinitions_columnType))

            End If
            'Richard Clarke November 2008 - PIE Enhancements - END
            '---------------------------------
        End If
        If aMyArray(0) = "" Then
            sSQLPreGUIDSearch = sSQLSearch & sSQLWhereClause
        End If
        sSQLSearch = sSQLSearch & sSQLWhereClause

        ' BSJ Sep 2009 - This check is done to set bGUIDAndPKMatch
        ' However, bGUIDAndPKMatch will always be false as its never set to true earlier in procedure?
        ' Comment for now and ask RC why this check is here

        ''search for existing record
        'm_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQLSearch, _
        ''        sSQLName:="Existing Data Check", _
        ''        bStoredProcedure:=False, _
        ''        bKeepNulls:=True, _
        ''        lnumberrecords:=gPMConstants.PMAllRecords, _
        ''        vResultArray:=vExistingData)
        '
        'If m_lReturn <> PMtrue Then
        '    PrepareSQLStatements = PMFalse
        '    r_sErrorString = "SQL Search failed for: " & sSQLSearch
        '    Exit Function
        'End If
        ''------------------------------------
        ''Richard Clarke November 2008 - PIE enhancements - NEW
        ''are all these checks necessary but not if g_bFirstImport = True?
        ''so far there is no row that matches this PK check + guid
        'If Not IsArray(vExistingData) Then
        '    bGUIDAndPKMatch = False
        'End If

        If iFindColumn <> -1 Then
            ' BSJ Aug 09 - Check for NULL
            'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
            If IsDBNull(aAdjustedData(iFindColumn)) Then
                sSourceGUID = ""
            Else
                'UPGRADE_WARNING: Couldn't resolve default property of object aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                sSourceGUID = aAdjustedData(iFindColumn)
            End If
        End If

        'check if just the PKs match
        m_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQLPreGUIDSearch, sSQLName:="Existing Data Check", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vExistingData)
        If Not IsArray(vExistingData) Then
            bPKOnlyMatch = False
        Else
            bPKOnlyMatch = True
        End If

        'if the pks match does the target have a guid at all?
        'if not, then just copy them across from the export file later
        If bPKOnlyMatch Then
            'check to see if there is a guid on this row in the target
            If iFindColumn = -1 Then
                bNoGUID = True
                sTargetGUID = ""
                'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
            ElseIf IsDBNull(vExistingData(iFindColumn, 0)) Or IsNothing(vExistingData(iFindColumn, 0)) Then
                bNoGUID = True
                sTargetGUID = ""
            Else
                bNoGUID = False
                'UPGRADE_WARNING: Couldn't resolve default property of object vExistingData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                sTargetGUID = vExistingData(iFindColumn, 0)
            End If
        End If

        'do a guid match - if a row exists in the target with this guid then we can just update this row
        '(or alternatively, something went seriously wrong and we fluked the same guid
        'on two separate environments, it depends on unique GUIDs being generated ALWAYS)
        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        sSQLSearch = "SELECT * FROM " & r_aIeControl(pbIeControl_objectName) & " WHERE " & g_PIEGuidCol & " = '"
        sSQLSearch = sSQLSearch & sSourceGUID & "'"
        m_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQLSearch, sSQLName:="Existing GUID Check", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vExistingGUIDData)

        'JB Jun 10 Added one more condition that both source and target guid can not be blank else there would be wrong match
        If IsArray(vExistingGUIDData) AndAlso (sSourceGUID <> "" OrElse sTargetGUID <> "") Then
            bGUIDNotOnTarget = False
            'JB Nov 09
            'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
            sTargetGUID = IIf(IsDBNull(vExistingGUIDData(iFindColumn, 0)), "", vExistingGUIDData(iFindColumn, 0))
        Else
            bGUIDNotOnTarget = True
        End If
        'Richard Clarke November 2008 - PIE Enhancements - END BLOCK
        '-------------------------------------

        'special case query for gis_screen_details
        'gis_screen_details always has different guids when a screen is edited on source
        'so check to see if this one is already there and update the target row
        'with the guid from source.

        ' BSJ Sep 09 - you have to check the PK columns (gis_screen_id + screen_detail_cnt)
        ' not caption!!!
        'If LCase(r_aIeControl(pbIeControl_objectName)) = LCase("gis_screen_detail") And bGUIDNotOnTarget = True Then
        '    sSQLSearch = "SELECT * FROM gis_screen_detail WHERE gis_screen_id = "
        '    sSQLSearch = sSQLSearch & aAdjustedData(findColumn("gis_screen_id", r_aIeTableDefinitions))
        '    'escape any apostrophes in captions
        '    sSQLSearch = sSQLSearch & " AND screen_detail_cnt = " & aAdjustedData(findColumn("screen_detail_cnt", r_aIeTableDefinitions))
        '    If Not IsDBNull(aAdjustedData(findColumn("PMFormat", r_aIeTableDefinitions))) Then
        '        sSQLSearch = sSQLSearch & " AND PMFormat = " & aAdjustedData(findColumn("PMFormat", r_aIeTableDefinitions))
        '    End If

        '    m_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQLSearch, sSQLName:="gis screen details check", _
        '        bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, _
        '        vResultArray:=vExistingScreenDetailData)

        '    If IsArray(vExistingScreenDetailData) Then
        '        'this row already exists, we just need to update the guid and time updated from the source
        '        'into the target
        '        bGUIDNotOnTarget = False
        '        bUpdateRequired = False 'set this so we do an update for gis_screen_detail
        '        vExistingData = vExistingScreenDetailData 'Richard Clarke 12/05/2009
        '    End If



        'special case query for gis_find_mapping
        'we need to check the FindControl_id and the control_index and the viewfieldname
        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If LCase(r_aIeControl(pbIeControl_objectName)) = LCase("gis_find_mapping") And bGUIDNotOnTarget = True Then
            sSQLSearch = "SELECT * FROM gis_find_mapping WHERE FindControl_ID = "
            'UPGRADE_WARNING: Couldn't resolve default property of object aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            sSQLSearch = sSQLSearch & aAdjustedData(findColumn("FindControl_ID", r_aIeTableDefinitions))
            'UPGRADE_WARNING: Couldn't resolve default property of object aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            sSQLSearch = sSQLSearch & " AND ControlIndex = " & aAdjustedData(findColumn("ControlIndex", r_aIeTableDefinitions))
            'UPGRADE_WARNING: Couldn't resolve default property of object aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            sSQLSearch = sSQLSearch & " AND ViewFieldName = '" & aAdjustedData(findColumn("ViewFieldName", r_aIeTableDefinitions)) & "'"

            m_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQLSearch, sSQLName:="gis screen details check", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vExistingGISFindMappingData)

            If IsArray(vExistingGISFindMappingData) Then
                'this row already exists, we just need to update the guid and time updated from the source
                'into the target
                bGUIDNotOnTarget = False
                bUpdateRequired = True 'set this so we do an update for gis_find_mapping
            End If
        End If


        'check for existing record
        If Not g_bFirstImport Then 'Richard Clarke November 2008 - PIE enhancements

            'Richard Clarke November 2008 - PIE enhancements
            'not the first time the new PIE import has been run
            'need to check the IDs are not already in use with a different guid
            'if they are, this means the target was updated independently of the source
            'and we need to update the import row PK columns before doing the insert
            'BSJ Aug 09 - If PK's Matched and a target guid exists then sTargetGUID will not be blank
            'JB Jun 10 - If PK's Matched and sTargetGUID might be blank so removing the condition
            If sTargetGUID <> "" And bPKOnlyMatch Then
                bSetNewId = (sTargetGUID <> sSourceGUID) And bGUIDNotOnTarget
            Else
                bSetNewId = (Not bPKOnlyMatch) And bGUIDNotOnTarget
            End If

            'If ((sTargetGUID <> sSourceGUID And bGUIDNotOnTarget)) Then 'And (sTargetGUID <> "")) Then
            'If bSetNewId Then
            If bSetNewId And LCase(r_aIeControl(pbIeControl_objectName)) <> "pmproduct_lookup" Then

                'manual change on target - update source row with new ID values
                'and store this data for child record's parent ids

                m_lReturn = SetNewIDs(r_aIeControl, r_aIeTableDefinitions, r_aRetrievedData, aAdjustedData, r_oDatabase, aMyArray, bNewIDSuccess)
                'now the new IDs have been set, set the row to be inserteds guid to the same as the source
                '(cannot be an update as GUIDs didn't match)
                'UPGRADE_WARNING: Couldn't resolve default property of object aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                aAdjustedData(iFindColumn) = sSourceGUID
            End If
        End If

        'BSJ We don't want to insert if there was a pk only match and target guid is null
        If bSetNewId Then
            'INSERT *****************************************************************************
            'no record, build a SQL insert statement for the data passing the adjusted data array
            '        m_lReturn = BuildInsertStatement(v_aIeControl:=r_aIeControl, _
            ''                v_aTableDefinition:=r_aIeTableDefinitions(pbIeTableDefinitions_columnArray), _
            ''                v_aRetrievedData:=aAdjustedData, _
            ''                bGotNewID:=bNewIDSuccess, _
            ''                r_sSQL:=sInsertSQL, _
            ''                r_oDatabase:=r_oDatabase)

            m_lReturn = BuildInsertStatement(v_aIeControl:=r_aIeControl, v_aTableDefinition:=r_aIeTableDefinitions(pbIeTableDefinitions_columnArray), v_aRetrievedData:=aAdjustedData, v_aRetrievedOldData:=r_aRetrievedData, bGotNewID:=bNewIDSuccess, r_sSQL:=sInsertSQL, r_oDatabase:=r_oDatabase, r_aTableDefinition:=r_aIeTableDefinitions)


            'Execute the statement
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                'If r_aIeControl(pbIeControl_objectName) = "gis_list_type_usage" Then
                '    Debug.Print "testing"
                'End If

                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(pbIeControl_IsIdentity). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If r_aIeControl(pbIeControl_IsIdentity) = True Then
                    'SET IDENTITY_INSERT products ON
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'm_lReturn = r_oDatabase.SQLAction(sSQL:="SET IDENTITY_INSERT " & r_aIeControl(pbIeControl_objectName) & " ON", sSQLName:="SET IDENTITY_INSERT on", bStoredProcedure:=False, lRecordsAffected:=lRecordsAffected)
                    sInsertSQL = "SET IDENTITY_INSERT " & r_aIeControl(pbIeControl_objectName) & " ON " & sInsertSQL & " SET IDENTITY_INSERT " & r_aIeControl(pbIeControl_objectName) & " OFF "
                End If

                'Replace any apostrophes with '' to cater for vbscript in columns
                'sInsertSQL = Replace(sInsertSQL, "'", "''")

                If InStr(sInsertSQL, "TREATY") > 0 Then
                    Debug.Print("TREATY insert")
                End If


                g_sLastSQLCommandGenerated = sInsertSQL

                m_lReturn = r_oDatabase.SQLAction(sSQL:=sInsertSQL, sSQLName:="GenericInsert", bStoredProcedure:=False, lRecordsAffected:=lRecordsAffected)

                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(pbIeControl_IsIdentity). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If r_aIeControl(pbIeControl_IsIdentity) = True Then
                    'SET IDENTITY_INSERT products OFF
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'r_oDatabase.SQLAction(sSQL:="SET IDENTITY_INSERT " & r_aIeControl(pbIeControl_objectName) & " OFF", sSQLName:="SET IDENTITY_INSERT off", bStoredProcedure:=False, lRecordsAffected:=lRecordsAffected)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    PrepareSQLStatements = gPMConstants.PMEReturnCode.PMFalse
                    r_sErrorString = "SQL Insert failed for: " & sInsertSQL
                    Exit Function
                End If

                'UPGRADE_WARNING: Couldn't resolve default property of object aParentPK. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                '---------

                '---------
                If r_aIeControl(pbIeControl_objectId) IsNot Nothing AndAlso r_aIeControl(pbIeControl_objectId) = pbIeDbt_UserDefinedList Then
                Else
                    aParentPK = g_cParentPK.Item("PK_" & CStr(r_aIeControl(pbIeControl_objectId)))
                    'On Error GoTo Err_PrepareSQLStatements
                    'UPGRADE_WARNING: IsEmpty was upgraded to IsNothing and has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    If Not IsNothing(aParentPK) Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        g_cParentPK.Remove("PK_" & CStr(r_aIeControl(pbIeControl_objectId)))
                    End If
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    g_cParentPK.Add(New Object() {aAdjustedData(0), r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(conPKColumn)(pbIeTableDefinitions_columnName)}, "PK_" & CStr(r_aIeControl(pbIeControl_objectId)))




                    'JB Nov 09 Storage in an array to compare old and new value later while updating for tables which has got referential integrity
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If LCase(r_aIeControl(pbIeControl_objectName)) = LCase("class_of_business") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("commission_band") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("peril_group") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("peril_type") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("authority_level_type") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("product") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("rule_set") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("gis_object") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("gis_property") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("gis_screen") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("risk_type") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("risk_type_group") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("tax_group") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("tax_band") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("treaty") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("ri_model") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("ri_band") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("gis_data_model") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("GIS_User_Def_Header") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("gis_list_items") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("gis_list_type") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("tax_band_rate") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("tax_type") Then
                        Dim sDictionaryKey As String = ToSafeString(r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(conPKColumn)(pbIeTableDefinitions_columnName)).Trim() + "_" + ToSafeString(r_aRetrievedData(0)).Trim() + "_" + ToSafeString(aAdjustedData(0)).Trim()
                        'If IsValueInArray(r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(conPKColumn)(pbIeTableDefinitions_columnName), aAdjustedData(0), r_aRetrievedData(0), g_aParentForeignKeys) = -1 Then
                        If Not g_aParentForeignKeysDic.ContainsKey(sDictionaryKey) Then
                            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            'addToArray(g_aParentForeignKeys, New Object() {r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(conPKColumn)(pbIeTableDefinitions_columnName), aAdjustedData(0), r_aRetrievedData(0)})
                            g_aParentForeignKeysDic.Add(sDictionaryKey, New Object() {r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(conPKColumn)(pbIeTableDefinitions_columnName), aAdjustedData(0), r_aRetrievedData(0)})
                        End If
                    End If
                End If

            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                PrepareSQLStatements = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If g_CloneDbKeys <> True Then
                'Ensure that the update has worked correctly and
                'rollback if it hasn't
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    '**TODO - This needs to be replaced by correct error handler
                    MsgBox("Insert Failed - stopping processing")
                    PrepareSQLStatements = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                Else
                    'Even if it has worked, ensure that only one row has been updated.
                    'If more than one has been affected, rollback the changes
                    If (lRecordsAffected <> 1) Then
                        '**TODO - This needs to be replaced by correct error handler
                        MsgBox("Multiple rows inserted - stopping processing")
                        PrepareSQLStatements = gPMConstants.PMEReturnCode.PMFalse
                        Exit Function
                    End If
                End If
            Else
                'no error on insert in a cloned database
            End If
            'ElseIf bGUIDNotOnTarget = False And bUpdateRequired = False Then
            'exit the function as we're not going to update or insert
        Else
            'UPDATE *****************************************************************************
            'do we need to do an update? check each column for differences
            bUpdateRequired = False

            'JB July 10 Improve the performance for gis_list_items and gis_list_type_usage
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If Trim(LCase(r_aIeControl(pbIeControl_objectName))) = "gis_list_items" OrElse Trim(LCase(r_aIeControl(pbIeControl_objectName))) = "gis_list_type_usage" Then
                bUpdateRequired = True
                ' JB July 10 Update not required for product_claims_workflow
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            ElseIf Trim(LCase(r_aIeControl(pbIeControl_objectName))) = "product_claims_workflow" OrElse Trim(LCase(r_aIeControl(pbIeControl_objectName))) = "gis_screen_detail" Then
                bUpdateRequired = False
            Else
                For iLoop = 0 To UBound(r_aIeTableDefinitions(pbIeTableDefinitions_columnArray))
                    Try


                        If Not IsArray(vExistingData) Then
                            'Richard Clarke - check that existing data is an array and if not, set it to the existing guid array
                            vExistingData = vExistingGUIDData
                        End If
                        'sInsertSQL = TypeName(vExistingData(iLoop, 0))
                        ' BSJ Sep 2009 - Do the check if table name is numbering_scheme as its identity is further along!
                        If r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(iLoop)(pbIeTableDefinitions_columnIsIdentity) Then
                            'ignore identity columns
                        Else
                            'Select Case TypeName(vExistingData(iLoop, 0))
                            Select LCase(r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(iLoop)(pbIeTableDefinitions_columnType))
                                Case "varchar", "char"
                                    aAdjustedData(iLoop) = Trim(ToSafeString(aAdjustedData(iLoop)))
                                    vExistingData(iLoop, 0) = Trim(ToSafeString(vExistingData(iLoop, 0)))
                                    If aAdjustedData(iLoop) <> vExistingData(iLoop, 0) Then
                                        bUpdateRequired = True
                                        Exit For
                                    End If
                                Case "boolean", "tinyint", "bit"
                                    If Not (IsDBNull(aAdjustedData(iLoop)) Or IsDBNull(vExistingData(iLoop, 0))) Then
                                        If (aAdjustedData(iLoop) = 0 And ToSafeBoolean(vExistingData(iLoop, 0)) = True) Or (aAdjustedData(iLoop) = 1 And ToSafeBoolean(vExistingData(iLoop, 0)) = False) Then
                                            bUpdateRequired = True
                                            Exit For
                                        End If
                                    End If
                                Case "datetime"
                        'If CStr(CDate(IIf(IsDBNull(aAdjustedData(iLoop)), 0, aAdjustedData(iLoop)))) <> CStr(CDate(IIf(IsDBNull(vExistingData(iLoop, 0)), 0, vExistingData(iLoop, 0)))) Then
                        Dim dtCheck As DateTime = Convert.ToDateTime(VB6.Format(aAdjustedData(iLoop), "yyyy\-mm\-dd hh\:nn\:ss"))
                        If dtCheck <> CDate(IIf(IsDBNull(vExistingData(iLoop, 0)), 0, vExistingData(iLoop, 0))) Then
                            If r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(iLoop)(pbIeTableDefinitions_columnName) <> g_PIELastUpdatedCol Then
                                bUpdateRequired = True
                                Exit For
                            End If
                        End If
                                Case "money", "decimal", "int", "Long", "smallint", "float"
                        If r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(iLoop)(pbIeTableDefinitions_columnName) = "caption_id" Then
                        End If

                                    If (aAdjustedData(iLoop) & "" <> vExistingData(iLoop, 0) & "") And 1 = 1 Then 'r_aIeTab 'r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(iLoop)(pbIeTableDefinitions_columnName) <> "" Then
                                        bUpdateRequired = True
                                        Exit For
                                    End If
                            Case "Byte()"
                            Case Else
                        End Select
                    End If
                    Catch ex As Exception
                LogPIEError("Error in doImport", True, False, "", False, ex.Message, "")
            End Try
                Next
        End If

        'JB Nov 09 We need to make this flag to true manually updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If (LCase(r_aIeControl(pbIeControl_objectName)) = LCase("peril_type") OrElse LCase(r_aIeControl(pbIeControl_objectName)) = LCase("rating_section_type") OrElse LCase(r_aIeControl(pbIeControl_objectName)) = LCase("peril_type_usage") OrElse LCase(r_aIeControl(pbIeControl_objectName)) = LCase("PMUser_Authority_Rule_Set_Link") OrElse LCase(r_aIeControl(pbIeControl_objectName)) = LCase("risk_type_rule_set") OrElse LCase(r_aIeControl(pbIeControl_objectName)) = LCase("risk_type_usage")) OrElse LCase(r_aIeControl(pbIeControl_objectName)) = LCase("tax_group_tax_band") OrElse LCase(r_aIeControl(pbIeControl_objectName)) = LCase("product_risk_type_group") OrElse LCase(r_aIeControl(pbIeControl_objectName)) = LCase("tax_band_rate") OrElse LCase(r_aIeControl(pbIeControl_objectName)) = LCase("ri_model_line") OrElse LCase(r_aIeControl(pbIeControl_objectName)) = LCase("risk_type") OrElse LCase(r_aIeControl(pbIeControl_objectName)) = LCase("gis_user_def_detail") OrElse LCase(r_aIeControl(pbIeControl_objectName)) = LCase("gis_list_type_usage") OrElse LCase(r_aIeControl(pbIeControl_objectName)) = LCase("gis_find_mapping") OrElse LCase(r_aIeControl(pbIeControl_objectName)) = LCase("ri_model") OrElse LCase(r_aIeControl(pbIeControl_objectName)) = LCase("tax_band") OrElse LCase(r_aIeControl(pbIeControl_objectName)) = LCase("tax_band_rate") OrElse LCase(r_aIeControl(pbIeControl_objectName)) = LCase("gis_object") OrElse LCase(r_aIeControl(pbIeControl_objectName)) = LCase("gis_property") OrElse LCase(r_aIeControl(pbIeControl_objectName)) = LCase("gis_screen") AndAlso Not bGUIDNotOnTarget Then
            bUpdateRequired = True
        End If


        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If (bUpdateRequired = True And LCase(r_aIeControl(pbIeControl_objectName)) <> "gis_screen_detail") Then

                ' Build a SQL update statement based on the array
                m_lReturn = BuildUpdateStatement(v_aIeControl:=r_aIeControl, v_aTableDefinition:=r_aIeTableDefinitions(pbIeTableDefinitions_columnArray), v_aRetrievedData:=r_aRetrievedData, v_aAdjustedData:=aAdjustedData, v_sSQL:=sUpdateSQL, r_aTableDefinition:=r_aIeTableDefinitions)


                g_sLastSQLCommandGenerated = sUpdateSQL


                'Execute the statement
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                    g_sLastSQLCommandGenerated = sUpdateSQL

                    m_lReturn = r_oDatabase.SQLAction(sSQL:=sUpdateSQL, sSQLName:="GenericUpdate", bStoredProcedure:=False, lRecordsAffected:=lRecordsAffected)
                End If

                'Ensure that the update has worked correctly
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    PrepareSQLStatements = gPMConstants.PMEReturnCode.PMFalse
                    r_sErrorString = "SQL Update failed for: " & sUpdateSQL
                    Exit Function
                End If

                'JB Nov 09 Storage in an array to compare old and new value later while updating for tables which has got referential integrity
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If LCase(r_aIeControl(pbIeControl_objectName)) = LCase("class_of_business") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("commission_band") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("peril_group") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("peril_type") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("authority_level_type") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("product") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("rule_set") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("gis_object") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("gis_property") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("gis_screen") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("risk_type") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("risk_type_group") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("tax_group") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("tax_band") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("treaty") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("ri_model") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("ri_band") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("gis_data_model") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("GIS_User_Def_Header") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("gis_list_items") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("gis_list_type") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("tax_band_rate") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("tax_type") Then
                    Dim sDictionaryKey As String = ToSafeString(r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(conPKColumn)(pbIeTableDefinitions_columnName)).Trim() + "_" + ToSafeString(r_aRetrievedData(0)).Trim() + "_" + ToSafeString(aAdjustedData(0)).Trim()
                    'If IsValueInArray(r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(conPKColumn)(pbIeTableDefinitions_columnName), aAdjustedData(0), r_aRetrievedData(0), g_aParentForeignKeys) = -1 Then
                    If Not g_aParentForeignKeysDic.ContainsKey(sDictionaryKey) Then
                        'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                        'addToArray(g_aParentForeignKeys, New Object() {r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(conPKColumn)(pbIeTableDefinitions_columnName), aAdjustedData(0), r_aRetrievedData(0)})
                        g_aParentForeignKeysDic.Add(sDictionaryKey, New Object() {r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(conPKColumn)(pbIeTableDefinitions_columnName), aAdjustedData(0), r_aRetrievedData(0)})
                    End If
                End If



            Else

                'no update required as source and destination are an exact match
                'set the parent value to this record's key in case this record has children
                'On Error Resume Next
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object g_cParentPK.Item(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object aParentPK. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                aParentPK = g_cParentPK.Item("PK_" & CStr(r_aIeControl(pbIeControl_objectId)))
                'On Error GoTo Err_PrepareSQLStatements
                'UPGRADE_WARNING: IsEmpty was upgraded to IsNothing and has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                If Not IsNothing(aParentPK) Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    g_cParentPK.Remove("PK_" & CStr(r_aIeControl(pbIeControl_objectId)))
                End If
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                g_cParentPK.Add(New Object() {aAdjustedData(0), r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(conPKColumn)(pbIeTableDefinitions_columnName)}, "PK_" & CStr(r_aIeControl(pbIeControl_objectId)))

                'JB Nov 09 Storage in an array to compare old and new value later while updating for tables which has got referential integrity
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If LCase(r_aIeControl(pbIeControl_objectName)) = LCase("class_of_business") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("commission_band") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("peril_group") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("peril_type") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("authority_level_type") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("product") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("rule_set") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("gis_object") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("gis_property") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("gis_screen") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("risk_type") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("risk_type_group") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("tax_group") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("tax_band") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("treaty") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("ri_model") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("ri_band") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("gis_data_model") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("GIS_User_Def_Header") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("gis_list_items") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("gis_list_type") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("tax_band_rate") Or LCase(r_aIeControl(pbIeControl_objectName)) = LCase("tax_type") Then
                    Dim sDictionaryKey As String = ToSafeString(r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(conPKColumn)(pbIeTableDefinitions_columnName)).Trim() + "_" + ToSafeString(r_aRetrievedData(0)).Trim() + "_" + ToSafeString(aAdjustedData(0)).Trim()
                    'If IsValueInArray(r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(conPKColumn)(pbIeTableDefinitions_columnName), aAdjustedData(0), r_aRetrievedData(0), g_aParentForeignKeys) = -1 Then
                    If Not g_aParentForeignKeysDic.ContainsKey(sDictionaryKey) Then
                        'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                        'addToArray(g_aParentForeignKeys, New Object() {r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(conPKColumn)(pbIeTableDefinitions_columnName), aAdjustedData(0), r_aRetrievedData(0)})
                        g_aParentForeignKeysDic.Add(sDictionaryKey, New Object() {r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(conPKColumn)(pbIeTableDefinitions_columnName), aAdjustedData(0), r_aRetrievedData(0)})
                    End If
                End If


            End If

        End If 'isempty(varray)

        ' Debug message
        'Debug.Print Timer & ": Exiting " & ACApp & conDot & ACClass & ".PrepareSQLStatements"
        Exit Function

Err_PrepareSQLStatements:

        ' Debug message
        'Debug.Print Timer & ": Errored in " & ACApp & conDot & ACClass & ".PrepareSQLStatements"
        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        LogPIEError("Error in PrepareSQLStatements", True, False, "", False, "", r_aIeControl(pbIeControl_objectName), "", "", sSourceGUID)

        PrepareSQLStatements = gPMConstants.PMEReturnCode.PMError

        Exit Function

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
    Public Function DisableEnableConstraints(ByRef r_oDatabase As dPMDAO.Database, ByVal v_sTableName As String, ByVal v_ProcessType As Short) As Integer

        'Define the local variables
        Dim sSQL As String
        Dim lRecordsAffected As Integer

        ' Debug message
        'Debug.Print Timer & ": Entering " & ACApp & conDot & ACClass & ".DisableEnableConstraints"

        Try

            DisableEnableConstraints = gPMConstants.PMEReturnCode.PMTrue

            'Reset the local variables
            sSQL = "ALTER TABLE " & v_sTableName & vbCrLf

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
                DisableEnableConstraints = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' Debug message
            'Debug.Print Timer & ": Exiting " & ACApp & conDot & ACClass & ".DisableEnableConstraints"
            Exit Function

        Catch ex As Exception

            DisableEnableConstraints = gPMConstants.PMEReturnCode.PMError

            ' Debug message
            'Debug.Print Timer & ": Errored in " & ACApp & conDot & ACClass & ".DisableEnableConstraints"

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisableEnableConstraints Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableEnableConstraints", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)
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
    Private Function BuildInsertStatement(ByVal v_aIeControl As Object, ByVal v_aTableDefinition As Object, ByVal v_aRetrievedData As Object, ByVal v_aRetrievedOldData As Object, ByVal bGotNewID As Boolean, ByRef r_sSQL As String, ByRef r_oDatabase As dPMDAO.Database, ByRef r_aTableDefinition As Object) As Integer

        'Richard Clarke November 2008 - PIE enhancements
        'added bGotNewID to function declaration so we can pass in from preparesqlstatements
        'that we don't need a new id generated here as we've already got it from SetNewIDs

        'Local definitions
        Dim iCounter As Short
        Dim sSQLInsert As String
        Dim sSQLDelete As String
        Dim sSQLValues As String
        Dim sSQLColumns As String
        Dim bMoreThanOneValue As Boolean
        Dim vNewRowID As Object
        Dim vIndexforMatchedValue As Integer
        Dim vKeyforMatchedValue As String
        Dim lColumnIdFOrDelete As Integer
        Dim sSQLWhere As String
        Dim SColumnNameForWhere As String

        ' Debug message
        'Debug.Print Timer & ": Entering " & ACApp & conDot & ACClass & ".BuildInsertStatement"


        BuildInsertStatement = gPMConstants.PMEReturnCode.PMTrue

        'Build a SQL update statement based on the array
        sSQLInsert = conEmptyString
        sSQLValues = conEmptyString
        sSQLColumns = conEmptyString
        bMoreThanOneValue = False
        sSQLInsert = sSQLInsert & "INSERT INTO "
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        sSQLInsert = sSQLInsert & v_aIeControl(pbIeControl_objectName)

        sSQLColumns = sSQLColumns & "("

        'Build a values statement with the returned data.
        sSQLValues = sSQLValues & "VALUES ("

        'Set to indicate only one (or less) parameters
        bMoreThanOneValue = False
        lColumnIdFOrDelete = -1

        'JB Nov 09 This exercise could not be made generic due to time constraint and this has really made me cry SORRY!!
        'JB Nov 09 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("peril_type") Then
            For iCounter = 0 To UBound(v_aRetrievedOldData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("class_of_business_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("lead_commission_band") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("ri_band") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("gis_screen_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("tax_group") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedOldData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aRetrievedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Nov 09 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("rating_section_type") Then
            For iCounter = 0 To UBound(v_aRetrievedOldData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("peril_group_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedOldData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aRetrievedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Dec 09 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("peril_type_usage") Then
            For iCounter = 0 To UBound(v_aRetrievedOldData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("peril_group_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("peril_type_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedOldData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aRetrievedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Dec 09 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("PMUser_Authority_Rule_Set_Link") Then
            For iCounter = 0 To UBound(v_aRetrievedOldData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("product_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("authority_level_type_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("rule_set_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedOldData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aRetrievedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Dec 09 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("gis_screen_detail") Then
            For iCounter = 0 To UBound(v_aRetrievedOldData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("gis_screen_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("gis_object_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("gis_property_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("default_object_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("default_property_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("child_screen_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedOldData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aRetrievedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Feb 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("risk_type_rule_set") Then
            For iCounter = 0 To UBound(v_aRetrievedOldData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("risk_type_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedOldData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aRetrievedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If


        'JB Feb 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("risk_type_usage") Then
            For iCounter = 0 To UBound(v_aRetrievedOldData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("risk_type_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("risk_type_group_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedOldData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aRetrievedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Jun 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("gis_object") Then
            For iCounter = 0 To UBound(v_aRetrievedOldData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("parent_object_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("gis_data_model_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedOldData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aRetrievedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Jun 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("tax_group_tax_band") Then
            For iCounter = 0 To UBound(v_aRetrievedOldData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("tax_group_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("tax_band_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedOldData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aRetrievedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Jun 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("product_risk_type_group") Then
            For iCounter = 0 To UBound(v_aRetrievedOldData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("product_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("risk_type_group_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedOldData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aRetrievedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Jun 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("Risk_Type_Rating_Section_Type") Then
            For iCounter = 0 To UBound(v_aRetrievedData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("risk_type_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("rating_section_type_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aRetrievedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Jun 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("Earning_Pattern_Usage") Then
            For iCounter = 0 To UBound(v_aRetrievedData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("Rating_Section_type_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("Earning_Pattern_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aRetrievedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Jun 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("Earning_Pattern_Usage") Then
            For iCounter = 0 To UBound(v_aRetrievedData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("Rating_Section_type_id") OrElse Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("Earning_Pattern_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aRetrievedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Jun 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("tax_band_rate") Then
            For iCounter = 0 To UBound(v_aRetrievedOldData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("tax_band_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("class_of_business_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedOldData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aRetrievedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Jun 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("ri_model_line") Then
            For iCounter = 0 To UBound(v_aRetrievedOldData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("ri_model_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("treaty_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedOldData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aRetrievedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Jun 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("gis_screen") Then
            For iCounter = 0 To UBound(v_aRetrievedOldData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("parent_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("gis_data_model_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedOldData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aRetrievedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Jun 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("gis_property") Then
            For iCounter = 0 To UBound(v_aRetrievedOldData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aRetrievedOldData(findColumn(specials_type, r_aTableDefinition)). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("gis_object_id") Or (Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("specials_type_reference") And Not IsDbNull(v_aRetrievedOldData(findColumn("specials_type", r_aTableDefinition))) And v_aRetrievedOldData(findColumn("specials_type", r_aTableDefinition)) = 6) Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedOldData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aRetrievedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Jun 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("gis_find_mapping") Then
            For iCounter = 0 To UBound(v_aRetrievedOldData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("gis_object_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("gis_property_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedOldData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aRetrievedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Jun 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("gis_user_def_detail") Then
            For iCounter = 0 To UBound(v_aRetrievedOldData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("gis_user_def_header_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedOldData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aRetrievedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Jun 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("gis_list_type_usage") Then
            For iCounter = 0 To UBound(v_aRetrievedOldData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("gis_list_items_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("gis_list_type_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedOldData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aRetrievedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Aug 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("tax_band") Then
            For iCounter = 0 To UBound(v_aRetrievedOldData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("tax_type_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedOldData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aRetrievedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Aug 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("product_claims_workflow") Then
            For iCounter = 0 To UBound(v_aRetrievedOldData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("product_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedOldData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aRetrievedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If


        'JB Aug 10 To retain new entries as those should not be deleted again SPECIFICALLY for tax_band_rate
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If LCase(v_aIeControl(pbIeControl_objectName)) = "tax_band_rate" Then
            'UPGRADE_WARNING: Couldn't resolve default property of object v_aRetrievedData(findColumn(code, r_aTableDefinition)). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If g_sObjectValue <> v_aRetrievedData(findColumn("code", r_aTableDefinition)) Then
                g_bOldDataDeleted = False
            End If
        End If

        'JB Jun 10 pie_guid gets changed due to the manual intervention in sirius (source and target) as deletion-insertion process is the concept behind this
        'JB Aug 10 To retain new entries as those should not be deleted again
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If LCase(v_aIeControl(pbIeControl_objectName)) = "risk_type_usage" Or LCase(v_aIeControl(pbIeControl_objectName)) = "peril_type_usage" Or LCase(v_aIeControl(pbIeControl_objectName)) = "tax_band_rate" Or LCase(v_aIeControl(pbIeControl_objectName)) = "tax_group_tax_band" Or LCase(v_aIeControl(pbIeControl_objectName)) = "product_risk_type_group" Or LCase(v_aIeControl(pbIeControl_objectName)) = "pmuser_authority_rule_set_link" Or LCase(v_aIeControl(pbIeControl_objectName)) = LCase("Risk_Type_Rating_Section_Type") Or LCase(v_aIeControl(pbIeControl_objectName)) = LCase("Earning_Pattern_Usage") Or LCase(v_aIeControl(pbIeControl_objectName)) = LCase("product_claims_workflow") Or LCase(v_aIeControl(pbIeControl_objectName)) = LCase("gis_screen_detail") Or LCase(v_aIeControl(pbIeControl_objectName)) = LCase("report_group_user_groups") Then


            'Build a SQL update statement based on the array
            sSQLDelete = conEmptyString
            sSQLDelete = sSQLDelete & "DELETE FROM "
            'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            sSQLDelete = sSQLDelete & v_aIeControl(pbIeControl_objectName)

            'Initialise the WHERE clause
            sSQLWhere = " WHERE "

            'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If LCase(v_aIeControl(pbIeControl_objectName)) = "risk_type_usage" Then
                lColumnIdFOrDelete = findColumn("risk_type_id", r_aTableDefinition)
                SColumnNameForWhere = "risk_type_id"
                If lColumnIdFOrDelete > -1 Then
                    sSQLWhere = sSQLWhere & SColumnNameForWhere
                    sSQLWhere = sSQLWhere & " = "
                    'UPGRADE_WARNING: Couldn't resolve default property of object v_aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sSQLWhere = sSQLWhere & "'" & v_aRetrievedData(lColumnIdFOrDelete) & "'" & " And "
                End If

                lColumnIdFOrDelete = findColumn("risk_type_group_id", r_aTableDefinition)
                SColumnNameForWhere = "risk_type_group_id"

            ElseIf LCase(v_aIeControl(pbIeControl_objectName)) = "report_group_user_groups" Then
                lColumnIdFOrDelete = findColumn("report_group_id", r_aTableDefinition)
                SColumnNameForWhere = "report_group_id"
                If lColumnIdFOrDelete > -1 Then
                    sSQLWhere = sSQLWhere & SColumnNameForWhere
                    sSQLWhere = sSQLWhere & " = "
                    'UPGRADE_WARNING: Couldn't resolve default property of object v_aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sSQLWhere = sSQLWhere & "'" & v_aRetrievedData(lColumnIdFOrDelete) & "'" & " And "
                End If

                lColumnIdFOrDelete = findColumn("pmuser_group_id", r_aTableDefinition)
                SColumnNameForWhere = "pmuser_group_id"

                'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            ElseIf LCase(v_aIeControl(pbIeControl_objectName)) = "peril_type_usage" Then
                lColumnIdFOrDelete = findColumn("peril_group_id", r_aTableDefinition)
                SColumnNameForWhere = "peril_group_id"
                If lColumnIdFOrDelete > -1 Then
                    sSQLWhere = sSQLWhere & SColumnNameForWhere
                    sSQLWhere = sSQLWhere & " = "
                    'UPGRADE_WARNING: Couldn't resolve default property of object v_aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sSQLWhere = sSQLWhere & "'" & v_aRetrievedData(lColumnIdFOrDelete) & "'" & " And "
                End If

                lColumnIdFOrDelete = findColumn("peril_type_id", r_aTableDefinition)
                SColumnNameForWhere = "peril_type_id"

            ElseIf LCase(v_aIeControl(pbIeControl_objectName)) = "gis_screen_detail" Then
                lColumnIdFOrDelete = findColumn("gis_screen_id", r_aTableDefinition)
                SColumnNameForWhere = "gis_screen_id"
                If lColumnIdFOrDelete > -1 Then
                    sSQLWhere = sSQLWhere & SColumnNameForWhere
                    sSQLWhere = sSQLWhere & " = "
                    'UPGRADE_WARNING: Couldn't resolve default property of object v_aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sSQLWhere = sSQLWhere & "'" & v_aRetrievedData(lColumnIdFOrDelete) & "'" & " And "
                End If

                lColumnIdFOrDelete = findColumn("screen_detail_cnt", r_aTableDefinition)
                SColumnNameForWhere = "screen_detail_cnt"

            ElseIf LCase(v_aIeControl(pbIeControl_objectName)) = "product_claims_workflow" Then
                lColumnIdFOrDelete = findColumn("product_id", r_aTableDefinition)
                SColumnNameForWhere = "product_id"
                If lColumnIdFOrDelete > -1 Then
                    sSQLWhere = sSQLWhere & SColumnNameForWhere
                    sSQLWhere = sSQLWhere & " = "
                    'UPGRADE_WARNING: Couldn't resolve default property of object v_aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sSQLWhere = sSQLWhere & "'" & v_aRetrievedData(lColumnIdFOrDelete) & "'" & " And "
                End If

                lColumnIdFOrDelete = findColumn("claim_process_type_id", r_aTableDefinition)
                SColumnNameForWhere = "claim_process_type_id"

                'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            ElseIf LCase(v_aIeControl(pbIeControl_objectName)) = "tax_band_rate" And Not g_bOldDataDeleted Then
                lColumnIdFOrDelete = findColumn("tax_band_rate_id", r_aTableDefinition)
                SColumnNameForWhere = "tax_band_rate_id"
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                g_sObjectValue = v_aRetrievedData(lColumnIdFOrDelete)
                g_bOldDataDeleted = True

                'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            ElseIf LCase(v_aIeControl(pbIeControl_objectName)) = "tax_group_tax_band" Then
                lColumnIdFOrDelete = findColumn("tax_group_id", r_aTableDefinition)
                SColumnNameForWhere = "tax_group_id"
                If lColumnIdFOrDelete > -1 Then
                    sSQLWhere = sSQLWhere & SColumnNameForWhere
                    sSQLWhere = sSQLWhere & " = "
                    'UPGRADE_WARNING: Couldn't resolve default property of object v_aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sSQLWhere = sSQLWhere & "'" & v_aRetrievedData(lColumnIdFOrDelete) & "'" & " And "
                End If

                lColumnIdFOrDelete = findColumn("tax_band_id", r_aTableDefinition)
                SColumnNameForWhere = "tax_band_id"

                'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            ElseIf LCase(v_aIeControl(pbIeControl_objectName)) = "product_risk_type_group" Then
                lColumnIdFOrDelete = findColumn("product_id", r_aTableDefinition)
                SColumnNameForWhere = "product_id"
                If lColumnIdFOrDelete > -1 Then
                    sSQLWhere = sSQLWhere & SColumnNameForWhere
                    sSQLWhere = sSQLWhere & " = "
                    'UPGRADE_WARNING: Couldn't resolve default property of object v_aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sSQLWhere = sSQLWhere & "'" & v_aRetrievedData(lColumnIdFOrDelete) & "'" & " And "
                End If

                lColumnIdFOrDelete = findColumn("risk_type_group_id", r_aTableDefinition)
                SColumnNameForWhere = "risk_type_group_id"

            ElseIf LCase(v_aIeControl(pbIeControl_objectName)) = LCase("Risk_Type_Rating_Section_Type") Then
                lColumnIdFOrDelete = findColumn("risk_type_id", r_aTableDefinition)
                SColumnNameForWhere = "risk_type_id"
                If lColumnIdFOrDelete > -1 Then
                    sSQLWhere = sSQLWhere & SColumnNameForWhere
                    sSQLWhere = sSQLWhere & " = "
                    'UPGRADE_WARNING: Couldn't resolve default property of object v_aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sSQLWhere = sSQLWhere & "'" & v_aRetrievedData(lColumnIdFOrDelete) & "'" & " And "
                End If

                lColumnIdFOrDelete = findColumn("rating_section_type_id", r_aTableDefinition)
                SColumnNameForWhere = "rating_section_type_id"

            ElseIf LCase(v_aIeControl(pbIeControl_objectName)) = LCase("Earning_Pattern_Usage") Then
                lColumnIdFOrDelete = findColumn("rating_section_type_id", r_aTableDefinition)
                SColumnNameForWhere = "rating_section_type_id"
                If lColumnIdFOrDelete > -1 Then
                    sSQLWhere = sSQLWhere & SColumnNameForWhere
                    sSQLWhere = sSQLWhere & " = "
                    'UPGRADE_WARNING: Couldn't resolve default property of object v_aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sSQLWhere = sSQLWhere & "'" & v_aRetrievedData(lColumnIdFOrDelete) & "'" & " And "
                End If

                lColumnIdFOrDelete = findColumn("earning_pattern_id", r_aTableDefinition)
                SColumnNameForWhere = "earning_pattern_id"

                'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            ElseIf LCase(v_aIeControl(pbIeControl_objectName)) = "pmuser_authority_rule_set_link" Then
                lColumnIdFOrDelete = findColumn("product_id", r_aTableDefinition)
                SColumnNameForWhere = "product_id"
                If lColumnIdFOrDelete > -1 Then
                    sSQLWhere = sSQLWhere & SColumnNameForWhere
                    sSQLWhere = sSQLWhere & " = "
                    'UPGRADE_WARNING: Couldn't resolve default property of object v_aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sSQLWhere = sSQLWhere & "'" & v_aRetrievedData(lColumnIdFOrDelete) & "'" & " And "
                End If

                lColumnIdFOrDelete = findColumn("authority_level_type_id", r_aTableDefinition)
                SColumnNameForWhere = "authority_level_type_id"
                If lColumnIdFOrDelete > -1 Then
                    sSQLWhere = sSQLWhere & SColumnNameForWhere
                    sSQLWhere = sSQLWhere & " = "
                    'UPGRADE_WARNING: Couldn't resolve default property of object v_aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sSQLWhere = sSQLWhere & "'" & v_aRetrievedData(lColumnIdFOrDelete) & "'" & " And "
                End If

                lColumnIdFOrDelete = findColumn("is_underwriter", r_aTableDefinition)
                SColumnNameForWhere = "is_underwriter"
                If lColumnIdFOrDelete > -1 Then
                    sSQLWhere = sSQLWhere & SColumnNameForWhere
                    sSQLWhere = sSQLWhere & " = "
                    'UPGRADE_WARNING: Couldn't resolve default property of object v_aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sSQLWhere = sSQLWhere & "'" & v_aRetrievedData(lColumnIdFOrDelete) & "'" & " And "
                End If

                lColumnIdFOrDelete = findColumn("transaction_type_id", r_aTableDefinition)
                SColumnNameForWhere = "transaction_type_id"
                If lColumnIdFOrDelete > -1 Then
                    sSQLWhere = sSQLWhere & SColumnNameForWhere
                    sSQLWhere = sSQLWhere & " = "
                    'UPGRADE_WARNING: Couldn't resolve default property of object v_aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sSQLWhere = sSQLWhere & "'" & v_aRetrievedData(lColumnIdFOrDelete) & "'" & " And "
                End If
                lColumnIdFOrDelete = findColumn("rule_set_id", r_aTableDefinition)
                SColumnNameForWhere = "rule_set_id"
            End If

            If lColumnIdFOrDelete > -1 Then
                sSQLWhere = sSQLWhere & SColumnNameForWhere
                sSQLWhere = sSQLWhere & " = "
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                sSQLWhere = sSQLWhere & "'" & v_aRetrievedData(lColumnIdFOrDelete) & "'"
            Else
                sSQLDelete = ""
                sSQLWhere = ""
            End If

            'Add all the string segments together to form a complete SQL string
            r_sSQL = conEmptyString
            r_sSQL = r_sSQL & sSQLDelete & sSQLWhere

            'Build a SQL insert statement based on the array
            sSQLInsert = conEmptyString
            sSQLValues = conEmptyString
            sSQLColumns = conEmptyString
            sSQLInsert = sSQLInsert & "INSERT INTO "
            'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            sSQLInsert = sSQLInsert & v_aIeControl(pbIeControl_objectName)

            sSQLColumns = sSQLColumns & "("

            'Build a values statement with the returned data.
            sSQLValues = sSQLValues & "VALUES ("

            For iCounter = 0 To UBound(v_aRetrievedData)

                If v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnIsIdentity) Then

                    'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If bMoreThanOneValue Then
                        'Append the comma & space character
                        sSQLColumns = sSQLColumns & conComma & " "
                        sSQLValues = sSQLValues & conComma & " "
                    End If

                    sSQLColumns = sSQLColumns & "[" & v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName) & "]"

                    'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnType). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnType) = "varchar" OrElse v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnType) = "char" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        sSQLValues = sSQLValues & "'" & v_aRetrievedData(iCounter) & "'"
                    Else
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        sSQLValues = sSQLValues & CStr(v_aRetrievedData(iCounter))
                    End If

                    'Set the flag to indicate a comma will be required on the end of
                    'the current string if we continue looping
                    bMoreThanOneValue = True

                Else
                    'can't do anything with timestamps
                    'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnType). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnType) <> "timestamp" Then

                        'JB Jun 10 tax_band_rate_id is identity column so need to skip as it is not the first column to be set under pbIeControl_IsIdentity
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If LCase(Trim(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = "tax_band_rate_id" OrElse LCase(Trim(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = "prov_claim_auto_numbering_id" OrElse LCase(Trim(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = "full_claim_auto_numbering_id" Then
                            'Don't do anything here
                        Else

                            If bMoreThanOneValue = True Then
                                'Append the comma & space character
                                sSQLColumns = sSQLColumns & conComma & " "
                                sSQLValues = sSQLValues & conComma & " "
                            End If

                            'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            sSQLColumns = sSQLColumns & "[" & v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName) & "]"

                            'Add the value to the string
                            'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            sSQLValues = sSQLValues & PrepareValueForSQLString(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnType), v_aRetrievedData(iCounter))

                        End If

                        'Set the flag to indicate a comma will be required on the end of
                        'the current string if we continue looping
                        bMoreThanOneValue = True
                    End If
                End If
            Next iCounter

            'Add the final closing bracket
            sSQLValues = sSQLValues & ")"
            sSQLColumns = sSQLColumns & ")"

            'Add all the string segments together to form a complete SQL string
            r_sSQL = r_sSQL & vbNewLine
            r_sSQL = r_sSQL & sSQLInsert & sSQLColumns & sSQLValues

        Else
            'Need to add each element in the retrieved data array
            For iCounter = 0 To UBound(v_aRetrievedData)
                If Trim(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnIsIdentity)) = "" Or
                     Trim(UCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnIsIdentity))) = "NULL" Then
                    v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnIsIdentity) = "False"
                End If
                If v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnIsIdentity) Then
                    'Assume that any identity field will always be the first column, which
                    'may or may not be a 'real' identity

                    'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(pbIeControl_IsIdentity). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If 1 = 0 AndAlso v_aIeControl(pbIeControl_IsIdentity) = True Then

                        'No work required.  Column is a real identity and as such
                        'will automatically be populated during the insert processing

                    Else

                        If bMoreThanOneValue Then
                            'Append the comma & space character
                            sSQLColumns = sSQLColumns & conComma & " "
                            sSQLValues = sSQLValues & conComma & " "
                        End If

                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        sSQLColumns = sSQLColumns & "[" & v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName) & "]"

                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnType). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnType) = "varchar" OrElse v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnType) = "char" Then

                            'Richard Clarke - update gis_screen.script_dynamic_logic value with escape chars for vbscript
                            'UPGRADE_WARNING: Couldn't resolve default property of object v_aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            If InStr(v_aRetrievedData(iCounter), "'") > -1 Then
                                Debug.Print("Found a string with an apostrophe")
                                'UPGRADE_WARNING: Couldn't resolve default property of object v_aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                Call Replace(v_aRetrievedData(iCounter), "'", "''", , CompareMethod.Text)
                            End If

                            'UPGRADE_WARNING: Couldn't resolve default property of object v_aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            sSQLValues = sSQLValues & "'" & v_aRetrievedData(iCounter) & "'"
                        Else

                            'Richard Clarke November 2008 - PIE enhancements - Added if clause
                            If bGotNewID = gPMConstants.PMEReturnCode.PMFalse Then

                                'Add the next available ID to the SQL string
                                'UPGRADE_WARNING: Couldn't resolve default property of object lRetrieveNewRowID(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                'UPGRADE_WARNING: Couldn't resolve default property of object vNewRowID. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                vNewRowID = lRetrieveNewRowID(v_aIeControl(pbIeControl_objectName), v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), r_oDatabase, v_aRetrievedData(iCounter))

                            Else 'use the id from the a_retrieved data
                                'UPGRADE_WARNING: Couldn't resolve default property of object v_aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                'UPGRADE_WARNING: Couldn't resolve default property of object vNewRowID. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                vNewRowID = v_aRetrievedData(iCounter)
                            End If
                            'Richard Clarke November 2008 - PIE enhancements END

                            'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
                            If IsDBNull(vNewRowID) Then
                                sSQLValues = sSQLValues & "NULL"
                            Else
                                'UPGRADE_WARNING: Couldn't resolve default property of object vNewRowID. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                sSQLValues = sSQLValues & CStr(vNewRowID)
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
                    'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnType). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnType) <> "timestamp" Then

                        'JB Jun 10 tax_band_rate_id is identity column so need to skip as it is not the first column to be set under pbIeControl_IsIdentity
                        'JB Aug 10 1) We don't need to update claim numbering. 2) Update risk_type with proper gis_screen_id for risk/claim DM
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If LCase(Trim(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = "tax_band_rate_id" OrElse LCase(Trim(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = "prov_claim_auto_numbering_id" OrElse LCase(Trim(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = "full_claim_auto_numbering_id" Then
                            'Don't do anything here!!
                        Else
                            If bMoreThanOneValue = True Then
                                'Append the comma & space character
                                sSQLColumns = sSQLColumns & conComma & " "
                                sSQLValues = sSQLValues & conComma & " "
                            End If

                            'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            sSQLColumns = sSQLColumns & "[" & v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName) & "]"

                            'Add the value to the string
                            'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            sSQLValues = sSQLValues & PrepareValueForSQLString(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnType), v_aRetrievedData(iCounter))
                        End If

                        'Set the flag to indicate a comma will be required on the end of
                        'the current string if we continue looping
                        bMoreThanOneValue = True
                    End If
                End If

            Next iCounter

            'Add the final closing bracket
            sSQLValues = sSQLValues & ")"
            sSQLColumns = sSQLColumns & ")"

            'Add all the string segments together to form a complete SQL string
            r_sSQL = conEmptyString
            r_sSQL = r_sSQL & sSQLInsert & sSQLColumns & sSQLValues

        End If

        ' Debug message
        'Debug.Print Timer & ": Exiting " & ACApp & conDot & ACClass & ".BuildInsertStatement"
        Exit Function

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
    Private Function BuildUpdateStatement(ByRef v_aIeControl As Object, ByRef v_aTableDefinition As Object, ByRef v_aRetrievedData As Object, ByRef v_aAdjustedData As Object, ByRef v_sSQL As String, ByRef r_aTableDefinition As Object) As Integer

        'Local definitions
        Dim iCounter As Short
        Dim sSQLUpdate As String
        Dim sSQLWhere As String
        Dim sSQLValues As String
        Dim bMoreThanOneValue As Boolean
        Dim bMoreThanOneWhere As Boolean
        Dim lColumnId As Integer
        Dim bPIEColumnCheck As Boolean
        Dim vIndexforMatchedValue As Integer
        Dim vKeyforMatchedValue As String


        ' Debug message
        'Debug.Print Timer & ": Entering " & ACApp & conDot & ACClass & ".BuildUpdateStatement"

        BuildUpdateStatement = gPMConstants.PMEReturnCode.PMTrue

        'Build a SQL update statement based on the array
        sSQLUpdate = conEmptyString
        sSQLUpdate = sSQLUpdate & "UPDATE "
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        sSQLUpdate = sSQLUpdate & v_aIeControl(pbIeControl_objectName)
        sSQLUpdate = sSQLUpdate & " SET "

        'Build a values statement with the returned data.
        sSQLValues = " "

        'Initialise the WHERE clause
        sSQLWhere = " WHERE "

        'Set to indicate only one (or less) parameters & where clauses
        bMoreThanOneValue = False
        bMoreThanOneWhere = False

        'JB Nov 09 This exercise could not be made generic due to time constraint and this has really made me cry SORRY!!
        'JB Nov 09 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("peril_type") Then
            For iCounter = 0 To UBound(v_aRetrievedData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("class_of_business_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("lead_commission_band") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("ri_band") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("gis_screen_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("tax_group") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aAdjustedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Nov 09 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("rating_section_type") Then
            For iCounter = 0 To UBound(v_aRetrievedData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("peril_group_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aAdjustedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Dec 09 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("peril_type_usage") Then
            For iCounter = 0 To UBound(v_aRetrievedData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("peril_group_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("peril_type_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aAdjustedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Dec 09 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("PMUser_Authority_Rule_Set_Link") Then
            For iCounter = 0 To UBound(v_aRetrievedData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("product_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("authority_level_type_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("rule_set_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aAdjustedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Feb 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("risk_type_rule_set") Then
            For iCounter = 0 To UBound(v_aRetrievedData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("risk_type_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aAdjustedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Feb 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("risk_type_usage") Then
            For iCounter = 0 To UBound(v_aRetrievedData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("risk_type_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("risk_type_group_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aAdjustedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Jun 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("gis_object") Then
            For iCounter = 0 To UBound(v_aRetrievedData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("parent_object_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("gis_data_model_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aAdjustedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Jun 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("tax_group_tax_band") Then
            For iCounter = 0 To UBound(v_aRetrievedData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("tax_group_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("tax_band_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aAdjustedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Jun 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("product_risk_type_group") Then
            For iCounter = 0 To UBound(v_aRetrievedData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("product_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("risk_type_group_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aAdjustedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Jun 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("Risk_Type_Rating_Section_Type") Then
            For iCounter = 0 To UBound(v_aRetrievedData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("risk_type_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("rating_section_type_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aAdjustedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If


        'JB Jun 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("Earning_Pattern_Usage") Then
            For iCounter = 0 To UBound(v_aRetrievedData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("Rating_Section_type_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("Earning_Pattern_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aAdjustedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Jun 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("Risk_Type_Rating_Section_Type") Then
            For iCounter = 0 To UBound(v_aRetrievedData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("risk_type_id") OrElse Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("rating_section_type_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aAdjustedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If


        'JB Jun 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("Earning_Pattern_Usage") Then
            For iCounter = 0 To UBound(v_aRetrievedData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("Rating_Section_type_id") OrElse Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("Earning_Pattern_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aAdjustedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Jun 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("tax_band_rate") Then
            For iCounter = 0 To UBound(v_aRetrievedData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("tax_band_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("class_of_business_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aAdjustedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Jun 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("ri_model_line") Then
            For iCounter = 0 To UBound(v_aRetrievedData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("treaty_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("ri_model_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aAdjustedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Jun 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("gis_screen") Then
            For iCounter = 0 To UBound(v_aRetrievedData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("parent_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("gis_data_model_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aAdjustedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Jun 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("gis_property") Then
            For iCounter = 0 To UBound(v_aRetrievedData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aRetrievedData(findColumn(specials_type, r_aTableDefinition)). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("gis_object_id") Or (Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("specials_type_reference") And Not IsDbNull(v_aRetrievedData(findColumn("specials_type", r_aTableDefinition))) And v_aRetrievedData(findColumn("specials_type", r_aTableDefinition)) = 6) Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aAdjustedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Jun 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("gis_find_mapping") Then
            For iCounter = 0 To UBound(v_aRetrievedData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("gis_object_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("gis_property_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aAdjustedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Jun 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("gis_user_def_detail") Then
            For iCounter = 0 To UBound(v_aRetrievedData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("gis_user_def_header_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aAdjustedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Jun 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("gis_list_type_usage") Then
            For iCounter = 0 To UBound(v_aRetrievedData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("gis_list_items_id") Or Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("gis_list_type_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aAdjustedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'JB Aug 10 Swap foreign key value in an array with new value later while updating for tables which has got referential integrity
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("tax_band") Then
            For iCounter = 0 To UBound(v_aRetrievedData)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = LCase("tax_type_id") Then
                    vKeyforMatchedValue = GetValueFromArray(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName), v_aRetrievedData(iCounter), g_aParentForeignKeysDic)
                    If vKeyforMatchedValue <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        v_aAdjustedData(iCounter) = g_aParentForeignKeysDic.Item(vKeyforMatchedValue)(1)
                    End If
                End If
            Next iCounter
        End If

        'Need to add each element in the retrieved data array
        Dim iPos As Short
        Dim sTestString1 As String
        Dim sTestString2 As String
        For iCounter = 0 To UBound(v_aRetrievedData)

            'Determine if the value being examined is a key field.  If so, add it to the
            'WHERE clause, if not, add it to the VALUES clause

            'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnType). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnType) <> "timestamp" Then

                '    'Add an AND to the end of the current line.  Only needed if more than one parameter
                '    'is Being written to the string
                If bMoreThanOneWhere = True Then
                    'Append the AND
                    sSQLWhere = sSQLWhere & " AND "
                End If

                'Add the new WHERE clause
                'JB Nov 09 Making update statements on the basis of pie_guid, previously it was on the basis of ID column which screwed updating
                lColumnId = findColumn(g_PIEGuidCol, r_aTableDefinition)
                If lColumnId > 0 AndAlso Not bPIEColumnCheck Then
                    sSQLWhere = sSQLWhere & g_PIEGuidCol
                    sSQLWhere = sSQLWhere & " = "
                    'UPGRADE_WARNING: Couldn't resolve default property of object v_aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sSQLWhere = sSQLWhere & "'" & v_aRetrievedData(lColumnId) & "'"
                    bPIEColumnCheck = True
                End If

                '    'Set the flag to indicate a comma will be required on the end of
                '    'the current string if we continue looping
                '    '                bMoreThanOneWhere = true
                bMoreThanOneWhere = False
                'End If

                ' BSJ Sep 2009 - Do the check if table name is numbering_scheme as its identity is further along!
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(pbIeControl_IsIdentity). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'If v_aIeControl(pbIeControl_IsIdentity) = True And (iCounter = 0 Or LCase(v_aIeControl(pbIeControl_objectName)) = "numbering_scheme" Or LCase(v_aIeControl(pbIeControl_objectName)) = "accumulation") Then
                If v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnIsIdentity) Then
                Else

                    ' JB Nov 09 Update child tables with foreign key values not for PK values
                    'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If (iCounter > 0 Or (iCounter = 0 And (Trim(LCase(v_aIeControl(pbIeControl_objectName)))) = LCase("PMUser_Authority_Rule_Set_Link") Or Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("peril_type_usage")) Or Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("risk_type_usage") Or Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("tax_group_tax_band") Or Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("product_risk_type_group") Or Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("Risk_Type_Rating_Section_Type") Or Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("Earning_Pattern_Usage") Or Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("report_group_contents") Or Trim(LCase(v_aIeControl(pbIeControl_objectName))) = LCase("report_group_user_groups")) Then

                        'JB Aug 10 1) We don't need to update claim numbering. 2) Update risk_type with proper gis_screen_id for risk/claim DM
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If LCase(Trim(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = "tax_band_rate_id" OrElse LCase(Trim(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = "prov_claim_auto_numbering_id" OrElse LCase(Trim(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName))) = "full_claim_auto_numbering_id" Then
                            'Don't do anything here
                        Else

                            'Add a comma to the end of the current line.  Only needed if more than one parameter
                            'is Being written to the string
                            If bMoreThanOneValue = True Then
                                'Append the comma & space character
                                sSQLValues = sSQLValues & conComma & " "
                            End If

                            'Add the field name to the string
                            'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            sSQLValues = sSQLValues & "[" & v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnName) & "]"

                            'Add the equal sign
                            sSQLValues = sSQLValues & " = "

                            '                  'Add the value to the string
                            'UPGRADE_WARNING: Couldn't resolve default property of object v_aTableDefinition()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            sSQLValues = sSQLValues & PrepareValueForSQLString(v_aTableDefinition(iCounter)(pbIeTableDefinitions_columnType), v_aAdjustedData(iCounter))

                        End If
                        'Set the flag to indicate a comma will be required on the end of
                        'the current string if we continue looping
                        bMoreThanOneValue = True
                    End If
                End If

            End If
        Next iCounter

        'Add all the string segments together to form a complete SQL string
        v_sSQL = conEmptyString
        v_sSQL = v_sSQL & sSQLUpdate & sSQLValues & sSQLWhere


        ' Debug message
        'Debug.Print Timer & ": Exiting " & ACApp & conDot & ACClass & ".BuildUpdateStatement"
        Exit Function

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


        ' Debug message
        'Debug.Print Timer & ": Entering " & ACApp & conDot & ACClass & ".PrepareValueForSQLString"

        'Determine the type expected for the column
        Select Case sElementType

            Case gSTRING, gCHAR, gVARCHAR, gTEXT, gCAPTIONTEXT
                'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
                'If IsDBNull(sElement) Then
                If Convert.IsDBNull(sElement) Then
                    PrepareValueForSQLString = "NULL"
                Else
                    'UPGRADE_WARNING: Couldn't resolve default property of object sElement. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    PrepareValueForSQLString = "'" & ProcessCaption(sElement) & "'"
                End If

            Case gDATETIME
                'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
                'If IsDBNull(sElement) Then
                If Convert.IsDBNull(sElement) Then
                    PrepareValueForSQLString = "NULL"
                Else
                    'need to manipulate the date to the correct format

                    'UPGRADE_WARNING: Couldn't resolve default property of object sElement. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    PrepareValueForSQLString = "Convert(datetime, '" & VB6.Format(sElement, "yyyy\-mm\-dd hh\:nn\:ss") & "', 120)"
                End If

            Case gINTEGER, gTINYINT
                'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
                'If IsDBNull(sElement) Then
                If Convert.IsDBNull(sElement) Then
                    PrepareValueForSQLString = "NULL"
                Else
                    'need to manipulate the date to the correct format
                    'UPGRADE_WARNING: Couldn't resolve default property of object sElement. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    PrepareValueForSQLString = sElement
                End If

            Case gMONEY, gNUMERIC, gDECIMAL, gFLOAT
                'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
                'If IsDBNull(sElement) Then
                If Convert.IsDBNull(sElement) Then
                    PrepareValueForSQLString = "NULL"
                Else
                    'need to manipulate the date to the correct format
                    'UPGRADE_WARNING: Couldn't resolve default property of object sElement. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    PrepareValueForSQLString = sElement
                End If
            Case gTIMESTAMP
                PrepareValueForSQLString = CStr(0)

            Case Else
                'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
                'If IsDBNull(sElement) Then
                If Convert.IsDBNull(sElement) Then
                    PrepareValueForSQLString = "NULL"
                Else
                    'need to manipulate the date to the correct format
                    'UPGRADE_WARNING: Couldn't resolve default property of object sElement. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    PrepareValueForSQLString = sElement
                End If

        End Select

        ' Debug message
        'Debug.Print Timer & ": Exiting " & ACApp & conDot & ACClass & ".PrepareValueForSQLString"
        Exit Function

    End Function

    ' ***************************************************************** '
    '
    ' Name:        AdjustRetrievedData
    '
    ' Description: Process to adjust the retrieved file data prior
    '              to it being added to the target database via SQL
    '
    ' History:     30/08/2002 JB - Created.
    '
    ' ***************************************************************** '
    Private Function AdjustRetrievedData(ByRef r_aIeControl As Object, ByRef r_aIeTableDefinitions As Object, ByRef r_aRetrievedData() As Object, ByRef r_aAdjustedData As Object, ByRef r_oDatabase As dPMDAO.Database, ByRef r_sErrorText As String) As Integer

        'Define local variables
        Dim aParentPK As Object
        Static bFoundTopLevel As Boolean

        Static lDefaultInsurerId As Integer

        On Error GoTo Err_AdjustRetrievedData

        ' Debug message
        'Debug.Print Timer & ": Entering " & ACApp & conDot & ACClass & ".AdjustRetrievedData"

        AdjustRetrievedData = gPMConstants.PMEReturnCode.PMTrue

        'Reset the adjusted data array
        'UPGRADE_WARNING: Couldn't resolve default property of object r_aAdjustedData. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        r_aAdjustedData = conEmptyString
        'UPGRADE_WARNING: Couldn't resolve default property of object r_aRetrievedData. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object r_aAdjustedData. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        r_aAdjustedData = r_aRetrievedData.Clone


        'Have received a whole row, so need to loop through each column idenitifed in
        'the table definition looking for specific types

        '*************************************************************************************
        'Check and replace and Ids with the previous parent id,if the row we're
        'currently on is a child row.
        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(pbIeControl_objectType). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'If r_aIeControl(pbIeControl_objectType) = pbIeOt_dbTable_child Then
        '    m_lReturn = ReplaceIDs(r_aAdjustedData:=r_aAdjustedData, r_aIeTableDefinitions:=r_aIeTableDefinitions, r_aIeControl:=r_aIeControl)

        '    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        '        r_sErrorText = "ReplaceIDs"
        '        AdjustRetrievedData = gPMConstants.PMEReturnCode.PMFalse
        '        Exit Function
        '    End If
        'End If

        '*************************************************************************************


        'Check and replace the Data Model Id value
        'Must not do this if the object is GIS_DATA_MODEL as we will be looking
        'up and changing the SAME element on the SAME table.
        'only do it if the current value is > 0, gis_screens have 0 DMID on child screens
        '
        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(pbIeControl_DataModelIdColumn). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If r_aIeControl(pbIeControl_DataModelIdColumn) <> 0 Then
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aRetrievedData(r_aIeControl(pbIeControl_DataModelIdColumn) - 1). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(pbIeControl_objectName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If r_aIeControl(pbIeControl_objectName) <> "gis_data_model" AndAlso r_aRetrievedData(r_aIeControl(pbIeControl_DataModelIdColumn) - 1) > 0 Then

                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                m_lReturn = ReplaceDataModelId(r_aIeTableDefinitions:=r_aIeTableDefinitions(pbIeTableDefinitions_columnArray), r_aData:=r_aAdjustedData, r_oDatabase:=r_oDatabase, v_iCounter:=r_aIeControl(pbIeControl_DataModelIdColumn))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sErrorText = "ReplaceDataModelId"
                    AdjustRetrievedData = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
            End If
        End If


        '*************************************************************************************
        'Check and replace the Caption Ids for the caption supplied.  Assumed that
        'the caption is always.....
        If r_aIeControl(pbIeControl_Flags) = pbIeControl_Flags__IsCaption Then
            m_lReturn = ReplaceCaptionId(r_aIeTableDefinitions:=r_aIeTableDefinitions, r_aRetrievedData:=r_aRetrievedData, r_aAdjustedData:=r_aAdjustedData, r_oDatabase:=r_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sErrorText = "ReplaceCaptionId"
                AdjustRetrievedData = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
        End If

        '************* replace any created_by_id or modified_by_id values with the sirius user id ************
        Dim iColumnId As Short
        If r_aIeControl(pbIeControl_Flags) = pbIeControl_Flags__CreatedByOrModifiedBy Then
            iColumnId = findColumn("created_by_id", r_aIeTableDefinitions)
            If iColumnId <> -1 Then
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                r_aAdjustedData(iColumnId) = g_lSiriusUserId
            End If
            iColumnId = findColumn("modified_by_id", r_aIeTableDefinitions)
            If iColumnId <> -1 Then
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                r_aAdjustedData(iColumnId) = g_lSiriusUserId
            End If
        End If

        '************* RESET THE GLOBAL VARIABLES IF REQUIRED ************************
        'If we have imported the GIS_Data_Model table, set global variables for later
        'use by the import processing should only run the first time
        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(pbIeControl_objectName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If r_aIeControl(pbIeControl_objectName) = "gis_data_model" Then

            m_lReturn = RetrieveDataModelReplacementValues(r_oDatabase:=r_oDatabase, r_aRetrievedData:=r_aRetrievedData, r_iCounter:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sErrorText = "RetrieveDataModelReplacementValues"
                AdjustRetrievedData = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
        End If

        '************* RESET gis_insurer_id to 0 ************************
        'we cant import insurers so fix it up to the default one
        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(pbIeControl_GisInsurerIdColumn). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        Dim vResults(,) As Object
        Dim sSQL As String
        Dim lRetVal As Integer
        Dim lCaptionId As Integer
        If r_aIeControl(pbIeControl_GisInsurerIdColumn) <> 0 Then
            If lDefaultInsurerId = 0 Then
                r_oDatabase.SQLSelect(sSQL:="select gis_insurer_id from gis_insurer where code ='DEFAULT'", sSQLName:="AdjustRetrievedData find default insurer", bStoredProcedure:=False, vResultArray:=vResults)
                If IsArray(vResults) Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    lDefaultInsurerId = vResults(0, 0)
                Else
                    'create default insurer

                    With r_oDatabase
                        With .Parameters
                            .Clear() 'Clear any existing parameters and add the required parameters
                            .Add(sName:="language_id", vValue:=1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                            .Add(sName:="caption", vValue:="DEFAULT", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                            .Add(sName:="caption_id", vValue:=9, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        End With

                        'Retrieve the appropriate caption_id for the description
                        lRetVal = .SQLSelect(sSQL:=ACGetPMCaptionSQL, sSQLName:=ACGetPMCaptionName, bStoredProcedure:=ACGetPMCaptionStored, vResultArray:=vResults)
                        lCaptionId = CInt(r_oDatabase.Parameters.Item("caption_id").Value)

                        'get the next value for gis_insurer
                        .SQLSelect(sSQL:="select max(gis_insurer_id)+1 from gis_insurer", sSQLName:="gen next gis_insure_id", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)
                        'UPGRADE_WARNING: Couldn't resolve default property of object vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        lDefaultInsurerId = vResults(0, 0)

                    End With

                    sSQL = "insert into gis_insurer (gis_insurer_id,code,caption_id,description,is_deleted,effective_date) values ()"
                    AddParameter(r_oDatabase, sSQL, lRetVal, "gis_insurer_id", lDefaultInsurerId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, 2)
                    AddParameter(r_oDatabase, sSQL, lRetVal, "code", "DEFAULT", gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, 2)
                    AddParameter(r_oDatabase, sSQL, lRetVal, "caption_id", lCaptionId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, 2)
                    AddParameter(r_oDatabase, sSQL, lRetVal, "description", "DEFAULT", gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, 2)
                    AddParameter(r_oDatabase, sSQL, lRetVal, "is_deleted", 0, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, 2)
                    AddParameter(r_oDatabase, sSQL, lRetVal, "effective_date", Now, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate, 2)

                    lRetVal = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="CreateDefaultGISInsurer", bStoredProcedure:=False)

                End If

                'find or create default insurer
            End If

            'UPGRADE_WARNING: Couldn't resolve default property of object r_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            r_aAdjustedData(findColumn("gis_insurer_id", r_aIeTableDefinitions)) = lDefaultInsurerId
        End If

        '******************************************************************************
        'JB Nov 09 Adjusting the foreign keys for few tables
        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Trim(LCase(r_aIeControl(pbIeControl_objectName))) = "class_of_business" OrElse Trim(LCase(r_aIeControl(pbIeControl_objectName))) = "commission_band" OrElse Trim(LCase(r_aIeControl(pbIeControl_objectName))) = "peril_group" OrElse Trim(LCase(r_aIeControl(pbIeControl_objectName))) = "peril_type" OrElse Trim(LCase(r_aIeControl(pbIeControl_objectName))) = "authority_level_type" OrElse Trim(LCase(r_aIeControl(pbIeControl_objectName))) = "product" OrElse Trim(LCase(r_aIeControl(pbIeControl_objectName))) = "rule_set" OrElse Trim(LCase(r_aIeControl(pbIeControl_objectName))) = "gis_object" OrElse Trim(LCase(r_aIeControl(pbIeControl_objectName))) = "gis_property" OrElse Trim(LCase(r_aIeControl(pbIeControl_objectName))) = "gis_screen" OrElse Trim(LCase(r_aIeControl(pbIeControl_objectName))) = "risk_type" OrElse Trim(LCase(r_aIeControl(pbIeControl_objectName))) = "risk_type_group" OrElse Trim(LCase(r_aIeControl(pbIeControl_objectName))) = "tax_group" OrElse Trim(LCase(r_aIeControl(pbIeControl_objectName))) = "tax_band" OrElse Trim(LCase(r_aIeControl(pbIeControl_objectName))) = "treaty" OrElse Trim(LCase(r_aIeControl(pbIeControl_objectName))) = "ri_model" OrElse Trim(LCase(r_aIeControl(pbIeControl_objectName))) = "ri_band" OrElse Trim(LCase(r_aIeControl(pbIeControl_objectName))) = "gis_data_model" OrElse Trim(LCase(r_aIeControl(pbIeControl_objectName))) = "gis_user_def_header" OrElse Trim(LCase(r_aIeControl(pbIeControl_objectName))) = "gis_list_items" OrElse Trim(LCase(r_aIeControl(pbIeControl_objectName))) = "gis_list_type" OrElse Trim(LCase(r_aIeControl(pbIeControl_objectName))) = "tax_type" Then

            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            m_lReturn = RetrieveFKReplacementValues(r_oDatabase:=r_oDatabase, r_aRetrievedData:=r_aRetrievedData, r_aAdjustedData:=r_aAdjustedData, r_iCounter:=findColumn(g_PIEGuidCol, r_aIeTableDefinitions), sObjectName:=Trim(LCase(r_aIeControl(pbIeControl_objectName))))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sErrorText = "RetrieveFKReplacementValues"
                AdjustRetrievedData = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
        End If


        '******************************************************************************
        'Save the current PK value for later use with any child properties
        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(pbIeControl_objectType). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If r_aIeControl(pbIeControl_objectType) <> pbIeOt_UserDefinedList Then
            On Error Resume Next
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Couldn't resolve default property of object g_cParentPK.Item(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Couldn't resolve default property of object aParentPK. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            aParentPK = g_cParentPK.Item("PK_" & CStr(r_aIeControl(pbIeControl_objectId)))
            On Error GoTo Err_AdjustRetrievedData
            'UPGRADE_WARNING: IsEmpty was upgraded to IsNothing and has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            If Not IsNothing(aParentPK) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                g_cParentPK.Remove("PK_" & CStr(r_aIeControl(pbIeControl_objectId)))
            End If
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            g_cParentPK.Add(New Object() {r_aRetrievedData(conPKColumn), r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(conPKColumn)(pbIeTableDefinitions_columnName)}, "PK_" & CStr(r_aIeControl(pbIeControl_objectId)))
        End If

        Exit Function

Err_AdjustRetrievedData:

        AdjustRetrievedData = gPMConstants.PMEReturnCode.PMError

        ' Debug message
        'Debug.Print Timer & ": Errored in " & ACApp & conDot & ACClass & ".AdjustRetrievedData"
        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AdjustRetrievedData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AdjustRetrievedData", vErrNo:=Err.Number, vErrDesc:=Err.Description)

        Exit Function
        Resume Next

    End Function

    ' ***************************************************************** '
    '
    ' Name:        ReplaceDataModelCode
    '
    ' Description: Process to replace a data_model_id from a source box with
    '              its equivalent data_model_id from the target box
    '
    ' History:     30/08/2002 JB - Created.
    '
    ' ***************************************************************** '
    Private Function ReplaceDataModelCode(ByRef r_aIeTableDefinitions As Object, ByRef r_aData As Object, ByRef r_oDatabase As dPMDAO.Database, ByVal v_iCounter As Short) As Integer


        ' Debug message
        'Debug.Print Timer & ": Entering " & ACApp & conDot & ACClass & ".ReplaceDataModelCode"

        ReplaceDataModelCode = gPMConstants.PMEReturnCode.PMTrue

        'Just set the appropriate values to the global variablesdefined when the
        'GIS_Data_Model was loaded. The retrieved data is a zero based array, but the
        'control array is 1 based so need to adjust the column value accordingly by
        'subtracting 1
        'UPGRADE_WARNING: Couldn't resolve default property of object r_aData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        r_aData(v_iCounter - 1) = g_DataModelCode

        ' Debug message
        'Debug.Print Timer & ": Exiting " & ACApp & conDot & ACClass & ".ReplaceDataModelCode"
        Exit Function

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
    Private Function ReplaceDataModelId(ByRef r_aIeTableDefinitions As Object, ByRef r_aData As Object, ByRef r_oDatabase As dPMDAO.Database, ByVal v_iCounter As Short) As Integer


        ' Debug message
        'Debug.Print Timer & ": Entering " & ACApp & conDot & ACClass & ".ReplaceDataModelId"

        ReplaceDataModelId = gPMConstants.PMEReturnCode.PMTrue

        'Just set the appropriate values to the global variables defined when the
        'GIS_Data_Model was loaded.  The retrieved data is a zero based array, but the
        'control array is 1 based so need to adjust the column value accordingly by
        'subtracting 1
        'UPGRADE_WARNING: Couldn't resolve default property of object r_aData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        r_aData(v_iCounter - 1) = g_DataModelId

        ' Debug message
        'Debug.Print Timer & ": Exiting " & ACApp & conDot & ACClass & ".ReplaceDataModelId"
        Exit Function

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
    Private Function ReplaceCaptionId(ByRef r_aIeTableDefinitions As Object, ByRef r_aRetrievedData As Object, ByRef r_aAdjustedData As Object, ByRef r_oDatabase As dPMDAO.Database) As Integer

        'Define local variables
        Dim r_vResults(,) As Object


        ' Debug message
        'Debug.Print Timer & ": Entering " & ACApp & conDot & ACClass & ".ReplaceCaptionId"

        ReplaceCaptionId = gPMConstants.PMEReturnCode.PMTrue

        Dim iCaptionId As Short
        Dim iCaptionDescription As Short

        'Check if the current data definition is the one required
        'by the control. The retrieved data is a zero based array, but the
        'control array is 1 based so need to adjust the column value accordingly by
        'subtracting 1

        iCaptionId = findColumn("caption_id", r_aIeTableDefinitions)
        iCaptionDescription = findColumn("description", r_aIeTableDefinitions)
        If iCaptionId = -1 OrElse iCaptionDescription = -1 Then
            ReplaceCaptionId = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'UDL captions can be NULL! so fix up
        'UPGRADE_WARNING: IsEmpty was upgraded to IsNothing and has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If IsDBNull(r_aRetrievedData(iCaptionDescription)) OrElse IsNothing(r_aRetrievedData(iCaptionDescription)) Then
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            r_aRetrievedData(iCaptionDescription) = "No description"
        End If


        'Use the received database connection
        With r_oDatabase
            With .Parameters
                'Clear any existing parameters
                .Clear()

                'Add the required parameters
                m_lReturn = .Add(sName:="language_id", vValue:=1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                m_lReturn = .Add(sName:="caption", vValue:=r_aRetrievedData(iCaptionDescription), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                m_lReturn = .Add(sName:="caption_id", vValue:=9, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End With
            'Check that the parameters have ben successfully added
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Log Error Message
                ReplaceCaptionId = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Set Parameter failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AdjustRetrievedData")

                Exit Function
            End If

            'Retrieve the appropriate caption_id for the description
            m_lReturn = .SQLSelect(sSQL:=ACGetPMCaptionSQL, sSQLName:=ACGetPMCaptionName, bStoredProcedure:=ACGetPMCaptionStored, vResultArray:=r_vResults)

            'Ensure that the query ran successfully
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ReplaceCaptionId = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

        End With
        'Update the array with the correct value returned in the parameter
        'UPGRADE_WARNING: Couldn't resolve default property of object r_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        r_aAdjustedData(iCaptionId) = CInt(r_oDatabase.Parameters.Item("caption_id").Value)


        ' Debug message
        'Debug.Print Timer & ": Exiting " & ACApp & conDot & ACClass & ".ReplaceCaptionID"
        Exit Function

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
    Private Function ReplaceIDs(ByRef r_aAdjustedData As Object, ByRef r_aIeTableDefinitions As Object, ByRef r_aIeControl As Object) As Integer


        Dim aParentPK As Object
        Dim iParentColumn As Short

        ' Debug message
        'Debug.Print Timer & ": Entering " & ACApp & conDot & ACClass & ".ReplaceIDs"

        ReplaceIDs = gPMConstants.PMEReturnCode.PMTrue

        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object g_cParentPK.Item(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object aParentPK. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        aParentPK = g_cParentPK.Item("PK_" & CStr(r_aIeControl(pbIeControl_RelatedObjectId)))

        iParentColumn = findColumn(aParentPK(1), r_aIeTableDefinitions)
        If iParentColumn <> -1 Then
            'UPGRADE_WARNING: Couldn't resolve default property of object aParentPK(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            r_aAdjustedData(iParentColumn) = aParentPK(0)
        End If

        ' Debug message
        'Debug.Print Timer & ": Exiting " & ACApp & conDot & ACClass & ".ReplaceIDs"
        Exit Function

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
    'BSJ Sep 2009 Made public
    Public Function RetrieveDataModelReplacementValues(ByRef r_oDatabase As dPMDAO.Database, ByRef r_aRetrievedData As Object, ByRef r_iCounter As Short) As Integer

        'Define the local variables
        Dim sSQL As String
        Dim r_vResults(,) As Object

        Try

            ' Debug message
            'Debug.Print Timer & ": Entering " & ACApp & conDot & ACClass & ".RetrieveDataModelReplacementValues"

            RetrieveDataModelReplacementValues = gPMConstants.PMEReturnCode.PMTrue

            'Setup the SQL for retreival
            sSQL = conEmptyString
            sSQL = sSQL & "SELECT gis_data_model_id, code FROM gis_data_model "
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            sSQL = sSQL & "WHERE code = '" & r_aRetrievedData(r_iCounter + 1) & "'"

            'get the column information for this table
            m_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="genericEnquire", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResults)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RetrieveDataModelReplacementValues = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If IsArray(r_vResults) Then
                'Set the global vairables for use by the general replacement sub
                'UPGRADE_WARNING: Couldn't resolve default property of object r_vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                g_DataModelId = r_vResults(0, 0)
                'UPGRADE_WARNING: Couldn't resolve default property of object r_vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                g_DataModelCode = Trim(r_vResults(1, 0))
            Else
                'Set the global vairables to the retrieved values, as the
                'data model does not exist on the target machine
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                g_DataModelId = r_aRetrievedData(pbIeControl_objectId)
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                g_DataModelCode = r_aRetrievedData(pbIeControl_objectName)
            End If

            ' Debug message
            'Debug.Print Timer & ": Exiting " & ACApp & conDot & ACClass & ".RetrieveDataModelReplacementValues"
            Exit Function

        Catch ex As Exception

            ' Debug message
            'Debug.Print Timer & ": Errored in " & ACApp & conDot & ACClass & ".RetrieveDataModelReplacementValues"

            RetrieveDataModelReplacementValues = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RetrieveDataModelReplacementValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RetrieveDataModelReplacementValues", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function
        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name:        RetrieveFKReplacementValues
    '
    ' Description:
    '
    ' History:     24/11/2009 JB - Created.
    '
    ' ***************************************************************** '
    Public Function RetrieveFKReplacementValues(ByRef r_oDatabase As dPMDAO.Database, ByRef r_aRetrievedData As Object, ByRef r_aAdjustedData As Object, ByRef r_iCounter As Short, ByRef sObjectName As String) As Integer

        'Define the local variables
        Dim sSQL As String
        Dim r_vResults(,) As Object

        Try

            ' Debug message
            'Debug.Print Timer & ": Entering " & ACApp & conDot & ACClass & ".RetrieveFKReplacementValues"

            RetrieveFKReplacementValues = gPMConstants.PMEReturnCode.PMTrue

            'Setup the SQL for retreival
            sSQL = conEmptyString
            sSQL = sSQL & "SELECT " & sObjectName & "_id FROM " & sObjectName
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            sSQL = sSQL & " WHERE " & g_PIEGuidCol & "= '" & r_aRetrievedData(r_iCounter) & "'"

            'get the column information for this table
            m_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="genericEnquire", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResults)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RetrieveFKReplacementValues = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If IsArray(r_vResults) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object r_vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                r_aAdjustedData(pbIeControl_objectId) = r_vResults(0, 0)
            End If

            ' Debug message
            'Debug.Print Timer & ": Exiting " & ACApp & conDot & ACClass & ".RetrieveFKReplacementValues"
            Exit Function

        Catch ex As Exception

            ' Debug message
            'Debug.Print Timer & ": Errored in " & ACApp & conDot & ACClass & ".RetrieveFKReplacementValues"

            RetrieveFKReplacementValues = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RetrieveFKReplacementValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RetrieveFKReplacementValues", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function
        End Try
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

        Dim sSQL As String
        Dim r_vResults(,) As Object

        ' Debug message
        'Debug.Print Timer & ": Entering " & ACApp & conDot & ACClass & ".lRetrieveNewRowID"


        If g_CloneDbKeys = True Then
            'UPGRADE_WARNING: Couldn't resolve default property of object lOriginalId. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Couldn't resolve default property of object lRetrieveNewRowID. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            lRetrieveNewRowID = lOriginalId
        Else
            'Setup the SQL for retreival
            sSQL = "Select max(" & v_sColumnName & ") from " & v_sTableName

            'get the column information for this table
            m_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="genericEnquire", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResults)

            'Return the next available ID
            'UPGRADE_WARNING: Couldn't resolve default property of object r_vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Couldn't resolve default property of object lRetrieveNewRowID. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            lRetrieveNewRowID = r_vResults(0, 0) + 1
        End If

        ' Debug message
        'Debug.Print Timer & ": Exiting " & ACApp & conDot & ACClass & ".lRetrieveNewRowID"
        Exit Function

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
    Public Function ExistsOnTarget(ByRef r_oDatabase As dPMDAO.Database, ByVal v_sTableName As String, ByRef r_bExistsOnTarget As Object) As Integer

        'Define local variables
        Dim sSQL As String
        Dim vResults(,) As Object

        ' Debug message
        'Debug.Print Timer & ": Entering " & ACApp & conDot & ACClass & ".ExistsOnTarget"

        Try

            'Set the default value
            ExistsOnTarget = gPMConstants.PMEReturnCode.PMTrue

            'Build the SQL
            sSQL = conEmptyString
            sSQL = conEmptyString
            sSQL = "select name from sysobjects where name like upper('" & Trim(v_sTableName) & "')"

            'Execute the SQL
            m_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="genericEnquire", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ExistsOnTarget = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            'UPGRADE_WARNING: Couldn't resolve default property of object r_bExistsOnTarget. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            r_bExistsOnTarget = IsArray(vResults)

            ' Debug message
            'Debug.Print Timer & ": Exiting " & ACApp & conDot & ACClass & ".ExistsOnTarget"
            Exit Function

        Catch ex As Exception

            ExistsOnTarget = gPMConstants.PMEReturnCode.PMError

            ' Debug message
            'Debug.Print Timer & ": Errored in " & ACApp & conDot & ACClass & ".ExistsOnTarget"

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExistsOnTarget Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExistsOnTarget", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function
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

        ' Debug message
        'Debug.Print Timer & ": Entering " & ACApp & conDot & ACClass & ".ProcessCaption"


        ' BSJ Sep 2009 compare to 0 not -1 as first character is 0 otherwise this is always true!
        ' Also only do the replace if we have found one, we were always replacing wasting time
        If InStr(v_sCaption, "'") > 0 Then
            Debug.Print("Found a string with an apostrophe" & v_sCaption)

            'DC010705 PN22076 do not strip spaces from caption in case they were intended to be there
            ProcessCaption = Replace(v_sCaption, "'", "''")
        Else
            ProcessCaption = v_sCaption
        End If


        ' Debug message
        'Debug.Print Timer & ": Exiting " & ACApp & conDot & ACClass & ".ProcessCaption"
        Exit Function

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

        ' Debug message
        'Debug.Print Timer & ": Entering " & ACApp & "." & ACClass & ".AddWhereClauseItem"


        AddWhereClauseItem = AddWhereClauseItem & " and " & v_vColumnName

        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If IsDBNull(v_vDataItem) Then
            AddWhereClauseItem = AddWhereClauseItem & " is null"
        Else

            ' BSJ Aug 2009 - Moved the REPLACE to after the ISNULL check so it doesnt bomb out if column is null
            'Handle values with single quotes inside
            'UPGRADE_WARNING: Couldn't resolve default property of object v_vDataItem. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            v_vDataItem = Replace(v_vDataItem, "'", "''")

            If v_sColumnType = "datetime" Then
                'UPGRADE_WARNING: Couldn't resolve default property of object v_vDataItem. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                AddWhereClauseItem = AddWhereClauseItem & "=" & "Convert(datetime, '" & VB6.Format(v_vDataItem, "yyyy\-mm\-dd hh\:nn\:ss") & "', 120)"
            Else
                'UPGRADE_WARNING: Couldn't resolve default property of object v_vDataItem. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                AddWhereClauseItem = AddWhereClauseItem & "='" & v_vDataItem & "'"
            End If

            'tbd remove this
            'UPGRADE_WARNING: TypeName has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            'Select Case TypeName(v_vDataItem)
            '    Case "Date"
            '        'AddWhereClauseItem = AddWhereClauseItem & "=" & "Convert(datetime, '" & Format$(v_vDataItem, "yyyy\-mm\-dd hh\:nn\:ss") & "', 120)"

            '    Case Else
            '        'AddWhereClauseItem = AddWhereClauseItem & "='" & v_vDataItem & "'"
            'End Select
        End If
        AddWhereClauseItem = AddWhereClauseItem & vbCrLf

        ' Debug message
        'Debug.Print Timer & ": Exiting " & ACApp & "." & ACClass & ".AddWhereClauseItem"

        Exit Function

    End Function
    ' Converts a variant into the best form for an SQL statement.
    ' NOT TO BE USED FOR ANY OTHER PURPOSE
    Public Function ToSQL(ByVal vValue As Object) As String

        ' Valueless types
        Const VT_EMPTY As Short = 0 ' vbEmpty
        Const VT_NULL As Short = 1 ' vbNull
        ' Single values
        Const VT_BOOL As Short = 11 ' vbBoolean
        Const VT_I1 As Short = 16
        Const VT_UI1 As Short = 17 ' vbByte
        Const VT_I2 As Short = 2 ' vbInteger
        Const VT_UI2 As Short = 18
        Const VT_I4 As Short = 3 ' vbLong
        Const VT_UI4 As Short = 19
        Const VT_INT As Short = 22
        Const VT_UINT As Short = 23
        Const VT_I8 As Short = 20
        Const VT_UI8 As Short = 21
        Const VT_R4 As Short = 4 ' vbSingle
        Const VT_R8 As Short = 5 ' vbDouble
        Const VT_CY As Short = 6 ' vbCurrency
        Const VT_DECIMAL As Short = 14 ' vbDecimal
        Const VT_DATE As Short = 7 ' vbDate
        Const VT_BSTR As Short = 8 ' vbString
        ' Not interpretable
        Const VT_ERROR As Short = 10
        Const VT_VARIANT As Short = 12
        Const VT_FILETIME As Short = 64
        Const VT_LPSTR As Short = 30
        Const VT_LPWSTR As Short = 31
        Const VT_CLSID As Short = 72
        Const VT_CF As Short = 71
        Const VT_BLOB As Short = 65
        Const VT_BLOBOBJECT As Short = 70
        Const VT_STREAM As Short = 66
        Const VT_STREAMED_OBJECT As Short = 68
        Const VT_STORAGE As Short = 67
        Const VT_STORED_OBJECT As Short = 69
        ' Structured data flags
        Const VT_TYPEMASK As Integer = &HFFF
        Const VT_VECTOR As Integer = &H1000
        Const VT_ARRAY As Integer = &H2000 ' vbArray
        Const VT_BYREF As Integer = &H4000

        Dim sFormat As String
        Dim iByte As Integer
        Dim sValue As String
        Dim iByteFirst As Integer
        Dim iByteLast As Integer

        ' Copy input value (see module-level comment).
        Dim vCopy As Object
        'UPGRADE_WARNING: Couldn't resolve default property of object vValue. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object vCopy. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vCopy = vValue

        ' The vartype() function actually returns the value of the
        ' internal VARIANT structure type field. To accommodate
        ' code that passes in variants of a type unsupported by VB,
        ' we must interpret undocumented types as well.

        'UPGRADE_WARNING: VarType has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        Select Case VarType(vCopy)
            Case VT_NULL, VT_EMPTY
                ToSQL = "null"
            Case VT_BOOL
                ' this matches the BOOLEAN type definition in the database
                'UPGRADE_WARNING: Couldn't resolve default property of object vCopy. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                ToSQL = IIf(vCopy, "1", "0")
            Case VT_I1, VT_UI1, VT_I2, VT_UI2, VT_I4, VT_UI4, VT_INT, VT_UINT, VT_I8, VT_UI8
                ' use locate-independent string conversion
                'UPGRADE_WARNING: Couldn't resolve default property of object vCopy. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                ToSQL = LTrim(Str(vCopy))
            Case VT_R4, VT_R8, VT_CY, VT_DECIMAL
                ' use locate-independent string conversion
                'UPGRADE_WARNING: Couldn't resolve default property of object vCopy. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                ToSQL = LTrim(Str(vCopy))
            Case VT_DATE
                ' handle both dates and times, and use locale-independent format
                'UPGRADE_WARNING: Couldn't resolve default property of object vCopy. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: DateValue has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                If vCopy = DateValue(vCopy) Then
                    sFormat = "{\d'yyyy\-mm\-dd'}"
                    'UPGRADE_WARNING: Couldn't resolve default property of object vCopy. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                ElseIf vCopy = TimeValue(vCopy) Then
                    sFormat = "{\t'hh\:nn\:ss'}"
                Else
                    sFormat = "{\t\s'yyyy\-mm\-dd hh\:nn\:ss'}"
                End If
                'UPGRADE_WARNING: Couldn't resolve default property of object vCopy. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                ToSQL = VB6.Format(vCopy, sFormat)
            Case VT_BSTR
                ' must handle quotes properly
                'UPGRADE_WARNING: Couldn't resolve default property of object vCopy. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                vCopy = Replace(vCopy, "'", "''")
                'UPGRADE_WARNING: Couldn't resolve default property of object vCopy. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                ToSQL = "'" & vCopy & "'"
            Case VT_ARRAY + VT_I1, VT_ARRAY + VT_UI1
                ' translate a byte array into a binary literal
                On Error Resume Next
                iByteFirst = LBound(vCopy)
                iByteLast = UBound(vCopy)
                sValue = ""
                If Err.Number = 0 Then
                    sValue = "0x"
                    For iByte = iByteFirst To iByteLast
                        'UPGRADE_WARNING: Couldn't resolve default property of object vCopy(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        sValue = sValue & Right("00" & Hex(vCopy(iByte)), 2)
                    Next
                Else
                    Err.Clear()
                End If
                ToSQL = sValue
            Case Else
                RaiseError("ToSQL", "Type Mismatch (cannot convert to SQL).", 13)
        End Select

    End Function
    ' ***************************************************************** '
    '
    ' Name:        SetNewIDsForGisScreenDetail
    '
    ' Description: Gets new IDs where required for data being imported
    '
    ' History:     October 2009 BSJ - Created.
    '
    ' ***************************************************************** '
    Public Function SetNewIDsForGisScreenDetail(ByRef r_oDatabase As dPMDAO.Database, ByVal v_lDataModelID As Integer) As Integer

        Dim vResults(,) As Object
        Dim xmlNodes As MSXML2.IXMLDOMNodeList
        Dim xmlTableNode As MSXML2.IXMLDOMElement
        Dim xmlColNode As MSXML2.IXMLDOMElement
        Dim xmlOldValNode As MSXML2.IXMLDOMElement
        Dim xmlNewValNode As MSXML2.IXMLDOMElement
        Dim xmlColChildren As MSXML2.IXMLDOMNodeList
        Dim node As MSXML2.IXMLDOMNode
        Dim sNewValue As String
        Dim sColName As String
        Dim lCount As Integer
        Dim sSQL As String

        Try

            sSQL = "SELECT DISTINCT gis_screen_detail.gis_screen_id, gis_screen_detail.screen_detail_cnt, gis_screen_detail.child_screen_id FROM gis_screen_detail LEFT JOIN gis_screen ON gis_screen_detail.gis_screen_id = gis_screen.gis_screen_id WHERE child_screen_id IS NOT NULL AND gis_screen.gis_data_model_id = " & CStr(v_lDataModelID)

            m_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Get Screen Details", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults, bKeepNulls:=True)

            Dim lNewID As Integer

            If IsArray(vResults) Then

                For lCount = 0 To UBound(vResults, 2)

                    xmlNodes = g_xmlDocument.selectNodes("//gis_screen")

                    If (xmlNodes Is Nothing = False) Then
                        If xmlNodes.length > 0 Then

                            xmlTableNode = xmlNodes(0).firstChild

                            If xmlTableNode Is Nothing = False Then

                                xmlColNode = xmlTableNode.selectSingleNode("//" & "gis_screen_id")

                                If Not xmlColNode Is Nothing Then

                                    'we must have children if this exists
                                    xmlColChildren = xmlColNode.childNodes
                                    For Each node In xmlColChildren

                                        'only if this is the old value attribute check it
                                        If node.attributes(0).nodeName = "old_value" Then

                                            'UPGRADE_WARNING: Couldn't resolve default property of object vResults(2, lCount). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                            If CInt(node.attributes(0).text) = vResults(2, lCount) Then
                                                lNewID = CInt(node.nextSibling.attributes(0).text)

                                                'UPGRADE_WARNING: Couldn't resolve default property of object vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                                sSQL = "UPDATE gis_screen_detail SET child_screen_id = " & CStr(lNewID) & " WHERE gis_screen_id = " & CStr(vResults(0, lCount)) & " AND screen_detail_cnt = " & CStr(vResults(1, lCount)) & " AND child_screen_id = " & CStr(vResults(2, lCount))

                                                m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Update Gis Screen Detail", bStoredProcedure:=False)

                                                Exit For
                                            End If
                                        End If
                                    Next node
                                End If
                            End If
                        End If
                    End If
                Next
            End If

            SetNewIDsForGisScreenDetail = True

            Exit Function

        Catch ex As Exception
            SetNewIDsForGisScreenDetail = False
            'log the audit message
            LogPIEError("Error in SetNewIDsForGisScreenDetail", True, False, "", False, "", CStr(0), "")
            RaiseError("SetNewIDsForGisScreenDetail", "Error in SetNewIDsForGisScreenDetail")
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name:        SetNewIDs
    '
    ' Description: Gets new IDs where required for data being imported
    '
    ' History:     November 2008 Richard Clarke - Created.
    '
    ' ***************************************************************** '
    Public Function SetNewIDs(ByRef r_aIeControl As Object, ByRef r_aIeTableDefinitions As Object, ByRef r_aRetrievedData As Object, ByRef r_aAdjustedData As Object, ByRef r_oDatabase As dPMDAO.Database, ByRef r_aPKCol As Object, ByRef bNewIDSuccess As Boolean) As Integer

        Dim vResults(,) As Object
        Dim sPKColName As String
        Dim aParentPK As Object
        Dim sTableName As String
        Dim lNewID As Integer
        Dim iColIndex As Short

        Dim xmlNodes As MSXML2.IXMLDOMNodeList
        Dim xmlTableNode As MSXML2.IXMLDOMElement
        Dim xmlColNode As MSXML2.IXMLDOMElement
        Dim xmlOldValNode As MSXML2.IXMLDOMElement
        Dim xmlNewValNode As MSXML2.IXMLDOMElement
        Dim xmlColChildren As MSXML2.IXMLDOMNodeList
        Dim node As MSXML2.IXMLDOMNode

        Dim element As MSXML2.IXMLDOMElement


        Dim iFindColumn As Integer
        Dim sOldValue As String
        Dim sNewValue As String
        Dim bFoundNewValue As Boolean
        Dim sColName As String

        On Error GoTo Err_SetNewIDs

        If r_aPKCol(0) = "" Then
            SetNewIDs = True
            Exit Function
        End If

        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        sTableName = r_aIeControl(pbIeControl_objectName)

        'get the primary key column name
        'UPGRADE_WARNING: Couldn't resolve default property of object r_aPKCol(0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions()()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        sPKColName = r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(r_aPKCol(0) - 1)(pbIeTableDefinitions_columnName)


        'ToDo: Add loop here where we just loop over every column in this row to see if its
        'value has been updated?

        'for each column in the array, check to see if its got a new value in the xml memory dictionary
        For iColIndex = 0 To UBound(r_aIeTableDefinitions(pbIeTableDefinitions_columnArray))

            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions()()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            sColName = r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(iColIndex)(pbIeTableDefinitions_columnName)

            'first thing to do is see if we've already set the new value for this id
            xmlNodes = g_xmlDocument.selectNodes("//" & sTableName)

            ' BSJ March 2009 - Check for tablenode
            ' BSJ Sep 09
            ' Need to check the logic here with Richard Clarke
            ' Currently this gets messed up if new pk's on the second source data model
            ' Only check pk col's
            If (xmlNodes Is Nothing = False) AndAlso (sColName = sPKColName) Then

                If xmlNodes.length > 0 Then
                    xmlTableNode = xmlNodes(0).firstChild

                    ' BSJ March 2009 - Check for child
                    If xmlTableNode Is Nothing = False Then

                        xmlColNode = xmlTableNode.selectSingleNode("//" & sColName)

                        If Not xmlColNode Is Nothing Then

                            iFindColumn = findColumn(sColName, r_aIeTableDefinitions)
                            'we must have children if this exists
                            xmlColChildren = xmlColNode.childNodes
                            For Each node In xmlColChildren

                                ' BSJ Sep 2009 - Ignore special column parent_object_id in table gis_object
                                If xmlColNode.baseName = "gis_object_id" Then
                                    If node.nodeName = "parent_object_id" Then
                                        Exit For
                                    End If
                                End If
                                'Richard Clarke amended Aug 09 as there were too many loops should be two elements
                                'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
                                If Not IsDBNull(r_aAdjustedData(iFindColumn)) Then

                                    'only if this is the old value attribute check it
                                    If node.attributes(0).nodeName = "old_value" Then

                                        'UPGRADE_WARNING: Couldn't resolve default property of object r_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                        If CInt(node.attributes(0).text) = CInt(r_aAdjustedData(iFindColumn)) Then
                                            'get the new value from the xml memory dictionary
                                            'UPGRADE_WARNING: Couldn't resolve default property of object r_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                            r_aAdjustedData(iFindColumn) = node.nextSibling.attributes(0).text
                                            bFoundNewValue = True
                                            Exit For
                                        End If
                                    End If
                                End If
                            Next node
                        End If
                    End If
                Else
                    Exit For
                End If
            End If
        Next iColIndex

        If bFoundNewValue = True Then
            bNewIDSuccess = True
            Exit Function
        End If

        'no need to do anything here, just exit?
        If LCase(sTableName) = "gis_find_mapping" Then
            bNewIDSuccess = True
            Exit Function
        End If

        ''''    If LCase(sTableName) = "gis_screen_detail" Then
        ''''        'first check if there is already a screen_detail_cnt for this gis_screen_id
        ''''        GISScreenDetailCntExists r_aRetrievedData, r_aAdjustedData, r_aIeTableDefinitions

        If LCase(sTableName) = "gis_find_mapping" Then
            'need to do something different here for gis_find_mapping?
            'we have a compound key - so search the xml document for the new values of the following columns
            'Findcontrol_id?, gis_object_id, gis_property_id
            Debug.Print("gis_find_mapping")

            'JB Nov 09 exclude these tables as these are child tables and doesnot contain any primary keys
            'JB Feb risk_type_usage added in this segment
            'JB Jun 10 tax_group_tax_band, product_risk_type_group added too
        ElseIf LCase(sTableName) = "peril_type_usage" Or LCase(sTableName) = "pmuser_authority_rule_set_link" Or LCase(sTableName) = "risk_type_usage" Or LCase(sTableName) = "tax_group_tax_band" Or LCase(sTableName) = "product_risk_type_group" Or LCase(sTableName) = "report_group_contents" Or LCase(sTableName) = "report_group_user_groups" Then
            'don't update the IDs

        Else
            'get the next pk column value from the target database
            With r_oDatabase
                'get the next available value for this table's pk column
                .SQLSelect(sSQL:="select max(" & sPKColName & ") + 1 from " & sTableName, sSQLName:="generate next " & sPKColName & "_id", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)

                ' BSJ Chcek for empty table - if so set ID to 1
                If IsArray(vResults) Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    lNewID = Val(vResults(0, 0))
                    If lNewID = 0 Then
                        lNewID = 1
                    End If
                End If
            End With

            'now we need to update the PK column in the array from the file (which is now the data
            'we're going to import in memory).
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            r_aAdjustedData(findColumn(sPKColName, r_aIeTableDefinitions)) = lNewID

            'we need to put a logic here to update the array for foreign keys of few tables.
            '        If r_aIeControl(pbIeControl_objectName) = "class_of_business" Then
            '            r_aAdjustedData((findColumn(sPKColName, "peril_type"))) = lNewID
            '        End If
        End If

        'now get a new caption id if this table has a caption column
        If findColumn("caption_id", r_aIeTableDefinitions) > -1 Then

            r_oDatabase.Parameters.Clear()
            r_oDatabase.Parameters.Add(sName:="language_id", vValue:=1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            r_oDatabase.Parameters.Add(sName:="caption", vValue:=r_aAdjustedData(findColumn("description", r_aIeTableDefinitions)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            r_oDatabase.Parameters.Add(sName:="caption_id", vValue:=9, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'get the new caption id
            'no need to store this in xml though as it's not reused
            r_oDatabase.SQLSelect(sSQL:=ACGetPMCaptionSQL, sSQLName:=ACGetPMCaptionName, bStoredProcedure:=ACGetPMCaptionStored, vResultArray:=vResults)

            'UPGRADE_WARNING: Couldn't resolve default property of object r_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            r_aAdjustedData(findColumn("caption_id", r_aIeTableDefinitions)) = CInt(r_oDatabase.Parameters.Item("caption_id").Value)
            r_oDatabase.Parameters.Clear()
        End If

        'Now we 've got the new ID, we need to set the parent PK
        'copied this code from AdjustRetrievedData
        '******************************************************************************
        'Save the current PK value for later use with any child properties
        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(pbIeControl_objectType). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If r_aIeControl(pbIeControl_objectType) <> pbIeOt_UserDefinedList Then
            On Error Resume Next
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Couldn't resolve default property of object g_cParentPK.Item(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Couldn't resolve default property of object aParentPK. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            aParentPK = g_cParentPK.Item("PK_" & CStr(r_aIeControl(pbIeControl_objectId)))
            On Error GoTo Err_SetNewIDs
            'UPGRADE_WARNING: IsEmpty was upgraded to IsNothing and has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            If Not IsNothing(aParentPK) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                g_cParentPK.Remove("PK_" & CStr(r_aIeControl(pbIeControl_objectId)))
            End If
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            g_cParentPK.Add(New Object() {r_aAdjustedData(conPKColumn), r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(conPKColumn)(pbIeTableDefinitions_columnName)}, "PK_" & CStr(r_aIeControl(pbIeControl_objectId)))
        End If
        '******************************************************************************
        On Error Resume Next

        bFoundNewValue = False

        'Also need to store this update in the XML document we're using in memory
        'find this table's node (if it already exists)
        xmlNodes = g_xmlDocument.selectNodes("//" & sTableName)
        'Richard Clarke 12/05/2009 - added xmlNodes(0).firstChild is Nothing
        If xmlNodes.length = 0 OrElse xmlNodes(0).firstChild Is Nothing Then
            'we need to create this table's node
            xmlTableNode = g_xmlDocument.createElement(sTableName)
            bFoundNewValue = True
        Else

            'only ever one entry so get the first child
            xmlTableNode = xmlNodes(0).firstChild
        End If

        'now loop all the columns except pie_guid and pie_last_updated
        'if the col value has changed add the old and new values to the table node

        For iColIndex = 0 To UBound(r_aIeTableDefinitions(pbIeTableDefinitions_columnArray))

            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions()()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            sColName = r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(iColIndex)(pbIeTableDefinitions_columnName)

            If (sColName = sPKColName) Then

                iFindColumn = findColumn(sColName, r_aIeTableDefinitions)
                'check the values aren't the same or this is pointless
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aRetrievedData(findColumn(sColName, r_aIeTableDefinitions)). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aAdjustedData(findColumn(sColName, r_aIeTableDefinitions)). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If r_aAdjustedData(iFindColumn) <> r_aRetrievedData(iFindColumn) Then

                    'now check the column isn't already in the document
                    xmlColNode = xmlTableNode.selectSingleNode("//" & sColName)
                    If xmlColNode Is Nothing Then
                        xmlColNode = g_xmlDocument.createElement(sColName)
                        'UPGRADE_WARNING: Couldn't resolve default property of object xmlColNode. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        xmlTableNode.appendChild(xmlColNode)
                    End If

                    'now add the old value and new value to the col node
                    xmlOldValNode = g_xmlDocument.createElement("OldValue")
                    xmlNewValNode = g_xmlDocument.createElement("NewValue")

                    'set the old and new values
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sOldValue = r_aRetrievedData(iFindColumn)
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sNewValue = r_aAdjustedData(iFindColumn)

                    xmlOldValNode.setAttribute("old_value", sOldValue)
                    xmlNewValNode.setAttribute("new_value", sNewValue)
                    'UPGRADE_WARNING: Couldn't resolve default property of object xmlOldValNode. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    xmlColNode.appendChild(xmlOldValNode)
                    'UPGRADE_WARNING: Couldn't resolve default property of object xmlNewValNode. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    xmlColNode.appendChild(xmlNewValNode)
                End If
            End If
        Next iColIndex

        ' Sep 2009 only add the table node if new table required otherwise append the column info to the doc
        If bFoundNewValue = True Then
            'now append the table node to the document
            'UPGRADE_WARNING: Couldn't resolve default property of object xmlTableNode. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            g_xmlDocument.documentElement.appendChild(xmlTableNode)
        End If

        bFoundNewValue = False

        ' If tablename = gis_object and retrieved parent_object_id is not null
        ' Retrieve the new value from xml where old gis_object_id = retrieveddata parentobjectid and place it in adjusteddata(parent_object_id)
        If (sTableName = "gis_object") Then
            iFindColumn = findColumn("parent_object_id", r_aIeTableDefinitions)
            'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
            If (Not IsDBNull(r_aRetrievedData(iFindColumn))) Then
                xmlNodes = g_xmlDocument.selectNodes("//" & sTableName)

                If (xmlNodes Is Nothing = False) Then
                    If xmlNodes.length > 0 Then

                        For iColIndex = 0 To xmlNodes.length

                            xmlTableNode = xmlNodes(iColIndex).firstChild

                            If xmlTableNode Is Nothing = False Then

                                xmlColNode = xmlTableNode.selectSingleNode("//" & "gis_object_id")

                                If Not xmlColNode Is Nothing Then

                                    'we must have children if this exists
                                    xmlColChildren = xmlColNode.childNodes
                                    For Each node In xmlColChildren

                                        'only if this is the old value attribute check it
                                        If node.attributes(0).nodeName = "old_value" Then

                                            'UPGRADE_WARNING: Couldn't resolve default property of object r_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                            If CInt(node.attributes(0).text) = CInt(r_aAdjustedData(iFindColumn)) Then
                                                'get the new value from the xml memory dictionary
                                                'UPGRADE_WARNING: Couldn't resolve default property of object r_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                                r_aAdjustedData(iFindColumn) = node.nextSibling.attributes(0).text
                                                bFoundNewValue = True
                                                Exit For
                                            End If
                                        End If

                                        If bFoundNewValue = True Then
                                            Exit For
                                        End If
                                    Next node
                                End If
                            End If
                        Next
                    End If
                End If
            End If
        End If

        bFoundNewValue = False

        ' BSJ Oct 2009 - Do same for gis_screen as well
        ' If tablename = gis_screen and retrieved parent_id is not null
        ' Retrieve the new value from xml where old gis_screen_id = retrieveddata parent_id and place it in adjusteddata(parent_id)
        If (sTableName = "gis_screen") Then
            iFindColumn = findColumn("parent_id", r_aIeTableDefinitions)
            'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
            If (Not IsDBNull(r_aRetrievedData(iFindColumn))) Then
                xmlNodes = g_xmlDocument.selectNodes("//" & sTableName)

                If (xmlNodes Is Nothing = False) Then
                    If xmlNodes.length > 0 Then

                        For iColIndex = 0 To xmlNodes.length

                            xmlTableNode = xmlNodes(iColIndex).firstChild

                            If xmlTableNode Is Nothing = False Then

                                xmlColNode = xmlTableNode.selectSingleNode("//" & "gis_screen_id")

                                If Not xmlColNode Is Nothing Then

                                    'we must have children if this exists
                                    xmlColChildren = xmlColNode.childNodes
                                    For Each node In xmlColChildren

                                        'only if this is the old value attribute check it
                                        If node.attributes(0).nodeName = "old_value" Then

                                            'UPGRADE_WARNING: Couldn't resolve default property of object r_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                            If CInt(node.attributes(0).text) = CInt(r_aAdjustedData(iFindColumn)) Then
                                                'get the new value from the xml memory dictionary
                                                'UPGRADE_WARNING: Couldn't resolve default property of object r_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                                r_aAdjustedData(iFindColumn) = node.nextSibling.attributes(0).text
                                                bFoundNewValue = True
                                                Exit For
                                            End If
                                        End If

                                        If bFoundNewValue = True Then
                                            Exit For
                                        End If
                                    Next node
                                End If
                            End If
                        Next
                    End If
                End If
            End If
        End If


        SetNewIDs = True

        Exit Function

Err_SetNewIDs:
        SetNewIDs = False
        'log the audit message
        LogPIEError("Error in CreateNewIDs", True, False, "", False, "", CStr(0), sPKColName)
        RaiseError("SetNewIDs", "Error in SetNewIDs")

    End Function

    Private Function GISScreenDetailCntExists(ByRef r_aRetrievedData As Object, ByRef r_aAdjustedData As Object, ByRef r_aIeTableDefinitions As Object) As Integer

        Dim bFound As Boolean
        Dim sSQL As String
        Dim vResults(,) As Object
        Dim iCount As Short

        'first query the gis_screen_detail table to see if this gis_screen_cnt exists already
        'for this gis_screen_id
        'UPGRADE_WARNING: Couldn't resolve default property of object r_aAdjustedData(findColumn(screen_detail_cnt, r_aIeTableDefinitions)). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object r_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        sSQL = "SELECT screen_detail_cnt FROM gis_screen_detail WHERE gis_screen_id = " & r_aAdjustedData(findColumn("gis_screen_id", r_aIeTableDefinitions)) & " and screen_detail_cnt = " & r_aAdjustedData(findColumn("screen_detail_cnt", r_aIeTableDefinitions))

        g_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GISScreenDetailCntExists", bStoredProcedure:=False, vResultArray:=vResults)

        If IsArray(vResults) Then
            'loop over results and see if ours exists
            For iCount = 0 To UBound(vResults, 2)
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If CShort(vResults(0, iCount)) = CShort(r_aAdjustedData(findColumn("screen_detail_cnt", r_aIeTableDefinitions))) Then
                    bFound = True
                    Exit For
                End If
            Next iCount
        Else
            'no results for this gis_screen_id
            GISScreenDetailCntExists = gPMConstants.PMEReturnCode.PMFalse
        End If

        If bFound Then
            'get the new screen_detail_cnt from gis_screen_detail
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            sSQL = "SELECT MAX(screen_detail_cnt) + 1 FROM gis_screen_detail WHERE gis_screen_id = " & r_aAdjustedData(findColumn("gis_screen_id", r_aIeTableDefinitions))
            g_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GISScreenDetailCntExists", bStoredProcedure:=False, vResultArray:=vResults)

            'UPGRADE_WARNING: Couldn't resolve default property of object vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aAdjustedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            r_aAdjustedData(findColumn("screen_detail_cnt", r_aIeTableDefinitions)) = vResults(0, 0)
            GISScreenDetailCntExists = gPMConstants.PMEReturnCode.PMTrue
        Else
            GISScreenDetailCntExists = gPMConstants.PMEReturnCode.PMFalse
        End If

        Exit Function
    End Function
End Module