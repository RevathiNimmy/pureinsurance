Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports SharedFiles

Module pbImportUserDefinedList

    'TO BE MOVED TO COMMON AREA
    Public g_sUDLTableName As String
    Public g_sUDLCompleteTableDefinition() As Object

    Private Const ACClass As String = conEmptyString

    Private m_lReturn As Integer

    ' ***************************************************************** '
    '
    ' Name:        ImportUserDefinedListFile
    '
    ' Description: Processes the import of Rule files from the binary
    '              file.  Recieves the current control file array row and
    '              the file number of the file being processed
    '
    ' History:     30/08/2002 JB  - Created.
    '              24/09/2002 SJP - Changed to be a function, so can
    '                               pass back if function was successful.
    '
    ' ***************************************************************** '
    Public Function ImportUserDefinedList(ByRef r_oDatabase As dPMDAO.Database, ByVal v_iFileNumber As Short, ByVal v_iMode As Integer, ByVal v_lDataModelID As Integer, ByVal v_sDataModelCode As String, ByVal r_aIeControl As Object, ByVal r_aIeTableDefinitions As Object, ByVal iTableIndex As Short) As Integer


        'Richard Clarke November 2008 - PIE enhancements
        'ByVal v_lDataModelID As Long, _
        ''ByVal v_sDataModelCode As String, _
        '
        'Define array to hold the retrieved data
        Dim aRetrievedData As Object
        Dim aUDLTableDefinition As Object
        Dim iUpper As Short

        On Error GoTo Err_ImportUserDefinedList

        ' Debug message
        'Debug.Print Timer & ": Entering " & ACApp & conDot & ACClass & ".ImportUserDefinedList"

        ImportUserDefinedList = gPMConstants.PMEReturnCode.PMTrue


        'Identify which table definition to use when extracting the UDL list data
        'from the binary file definition
        On Error Resume Next

        iUpper = UBound(g_sUDLCompleteTableDefinition)

        If Err.Number Then
            'Set the table definition to the 'standard' definition
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Couldn't resolve default property of object aUDLTableDefinition. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            aUDLTableDefinition = r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)
            Err.Clear()
        Else
            'Set the table  to the 'special' definition
            'UPGRADE_WARNING: Couldn't resolve default property of object g_sUDLCompleteTableDefinition(pbIeTableDefinitions_columnArray). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Couldn't resolve default property of object aUDLTableDefinition. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            aUDLTableDefinition = g_sUDLCompleteTableDefinition(pbIeTableDefinitions_columnArray)
        End If

        On Error GoTo 0

        'Read a row passing in the definition for the row
        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        m_lReturn = GetBinaryFileRow(v_iFileNumber:=v_iFileNumber, v_vObjectType:=r_aIeControl(pbIeControl_objectId), r_aDataDefinition:=aUDLTableDefinition, aRetrievedData:=aRetrievedData, rowIndex:=iTableIndex)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ImportUserDefinedList = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        '********************************************************
        'Process the headers, special headers and data rows differently
        'depending on ...
        Select Case iTableIndex

            Case pbIeDbt_UserDefinedListHeader
                'Call rountine to process a standard header
                m_lReturn = ProcessUDLHeader(r_oDatabase:=r_oDatabase, r_aIeControl:=r_aIeControl, v_aRetrievedData:=aRetrievedData)
            Case pbIeDbt_UserDefinedList
                'Call rountine to process data row using the identified
                'table definition.
                m_lReturn = ProcessUDLList(r_oDatabase:=r_oDatabase, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=aUDLTableDefinition, r_aCompleteTableDefinitions:=g_sUDLCompleteTableDefinition, r_iTableIndex:=iTableIndex, r_aRetrievedData:=aRetrievedData)
            Case Else
                'Unknown type has been received
                MsgBox("An unknown UDL type of" & iTableIndex & " has been supplied." & MsgBoxStyle.OkOnly)
                End
        End Select

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ImportUserDefinedList = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        ' Debug message
        'Debug.Print Timer & ": Entering " & ACApp & conDot & ACClass & ".ImportUserDefinedList"

        Exit Function

Err_ImportUserDefinedList:

        ' Debug message
        Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".ImportUserDefinedList")

        ImportUserDefinedList = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ImportUserDefinedList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportUserDefinedList", vErrNo:=Err.Number, vErrDesc:=Err.Description)

    End Function

    ' ***************************************************************** '
    '
    ' Name:        ProcessUDLHeaderStandard
    '
    ' Description: Processes the save the UDL table name for later use
    '              when processing the associated list data.
    '
    ' History:     30/08/2002 JB  - Created.
    '              24/09/2002 SJP - Changed to be a function, so can
    '                               pass back if function was successful.
    '
    ' ***************************************************************** '

    Private Function ProcessUDLHeader(ByRef r_oDatabase As dPMDAO.Database, ByRef r_aIeControl As Object, ByVal v_aRetrievedData As Object) As Integer


        ' Debug message
        'Debug.Print Timer & ": Entering " & ACApp & conDot & ACClass & ".ProcessUDLHeader"

        ProcessUDLHeader = gPMConstants.PMEReturnCode.PMTrue
        Dim sColumnNames As String

        'The header record only contains the name of the TABLE to
        'hold the User Defined List data which will be read in subsequent
        'data records.  This name needs to be substituted into those records
        'as they are read.  Therefore store the name in a global variable and
        'overwrite the data records as the arrive.  The global variable
        'will be overwritten everytime a new UDL header is received

        'Clear any existing values for safety!
        g_sUDLTableName = conEmptyString
        Erase g_sUDLCompleteTableDefinition

        'Add the new value (assume always held in position zero)
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        g_sUDLTableName = v_aRetrievedData(0)


        '**************************************************************************

        'Check for any special table definitions.  If one exists, a specific
        'table definition must be used to retrieve the subsequent data.  This
        'definition needs to be re-constructed from the data stored in the second
        'field in the header record.
        Dim aWorkingArray() As Object
        Dim aTemp2() As Object
        Dim iIndex As Short
        Dim bExistsOnTarget As Boolean
        If Len(v_aRetrievedData(1)) > 0 Then


            'Split the string into chunks/item/bits
            'UPGRADE_WARNING: Couldn't resolve default property of object v_aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Couldn't resolve default property of object aWorkingArray. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            aWorkingArray = Split(v_aRetrievedData(1), conComma)

            'Reconstruct a table definition array from the supplied string
            'add column name, type and size to array

            'Add the basic details to the new array. Assume that these will be
            'fixed (at least for now)
            Call addToArray(g_sUDLCompleteTableDefinition, g_sUDLTableName)

            'Set the panel to indicate the type of import
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "User Defined List"
            objFrmMainForm.StatusBar_TextWrite("User Defined List", 1)
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = g_sUDLTableName
            objFrmMainForm.StatusBar_TextWrite(g_sUDLTableName, 2)
            Call addToArray(g_sUDLCompleteTableDefinition, pbIeDbt_UserDefinedList)

            'Loop around the working array adding groups of three elements
            'as a sub array to the temporary array
            For iIndex = 0 To UBound(aWorkingArray) Step 8

                'store the column names
                'UPGRADE_WARNING: Couldn't resolve default property of object aWorkingArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                sColumnNames = sColumnNames & Trim(aWorkingArray(iIndex)) & ","

                'Add elements of the received data string to the table
                'definitions array
                'UPGRADE_WARNING: Couldn't resolve default property of object aWorkingArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                Call addToArray(aTemp2, New Object() {Trim(aWorkingArray(iIndex + udlTable_columnName)), Trim(aWorkingArray(iIndex + udlTable_columnDatatype)), Trim(aWorkingArray(iIndex + udlTable_columnSize)), Trim(aWorkingArray(iIndex + udlTable_columnPrecision)), Trim(aWorkingArray(iIndex + udlTable_columnScale)), Trim(aWorkingArray(iIndex + udlTable_columnNullability)), Trim(aWorkingArray(iIndex + udlTable_columnIsIdentity)), Trim(aWorkingArray(iIndex + udlTable_columnPKFlag))})


            Next iIndex

            'Need this information for the UDL list control array patch
            Call addToArray(g_sUDLCompleteTableDefinition, aTemp2)

            'add the column names onto the table definition
            Call addToArray(g_sUDLCompleteTableDefinition, Left(sColumnNames, Len(sColumnNames) - 1))

            'JB Jun 10 DROP the UDL and reinsert - Conceptually great even in case structure of the UDL gets changed at source level
            'Drop it if it's already there
            DropExistingTables(g_sUDLTableName, r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ProcessUDLHeader = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = CreateTable(r_oDatabase:=r_oDatabase, r_aTable:=g_sUDLCompleteTableDefinition)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ProcessUDLHeader = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        ' Debug message
        'Debug.Print Timer & ": Entering " & ACApp & conDot & ACClass & ".ProcessUDLHeader"

        Exit Function

    End Function

    ' ***************************************************************** '
    '
    ' Name:        ProcessUDLList
    '
    ' Description: Process to identify the correct table definition to
    '              use and to subsequently extract and save the UDL list
    '              data from the binary file
    '
    ' History:     30/08/2002 JB  - Created.
    '              24/09/2002 SJP - Changed to be a function, so can
    '                               pass back if function was successful.
    '
    ' ***************************************************************** '
    Private Function ProcessUDLList(ByRef r_oDatabase As dPMDAO.Database, ByRef r_aIeControl As Object, ByRef r_aIeTableDefinitions As Object, ByRef r_aCompleteTableDefinitions As Object, ByVal r_iTableIndex As Short, ByRef r_aRetrievedData As Object) As Integer

        Dim sErrorMessage As String

        ' Debug message
        'Debug.Print Timer & ": Entering " & ACApp & conDot & ACClass & ".ProcessUDLList"

        ProcessUDLList = gPMConstants.PMEReturnCode.PMTrue

        'Richard Clarke November 2008 - PIE enhancements
        'we need to add the guid columns to this table if they don't already exist
        'UPGRADE_WARNING: Couldn't resolve default property of object r_aCompleteTableDefinitions(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        m_lReturn = CheckSingleTableGuid(r_oDatabase, r_aCompleteTableDefinitions(0))

        'Patch the control array to make it look like the multi-purpose
        'UDL record looks like a specific record
        m_lReturn = PatchControlArray(r_oDatabase:=r_oDatabase, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, r_aCompleteTableDefinitions:=r_aCompleteTableDefinitions(pbIeTableDefinitions_columnArray))

        'Call a common routine to prepare and execute the SQL
        'needed to update or insert the data into the table
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = PrepareSQLStatements(r_oDatabase:=r_oDatabase, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aCompleteTableDefinitions, r_iTableIndex:=r_iTableIndex, r_aRetrievedData:=r_aRetrievedData, r_sErrorString:=sErrorMessage)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            writeToStatusBox("Error : " & sErrorMessage)
            ProcessUDLList = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        ' Debug message
        'Debug.Print Timer & ": Entering " & ACApp & conDot & ACClass & ".ProcessUDLList"

        Exit Function

    End Function

    ' ***************************************************************** '
    '
    ' Name:        CreateTable
    '
    ' Description: Process to build appropriate SQL statement to create
    '              a new table on the target database
    '
    ' History:     30/08/2002 JB  - Created.
    '              24/09/2002 SJP - Changed to be a function, so can
    '                               pass back if function was successful.
    '
    ' ***************************************************************** '
    Private Function CreateTable(ByRef r_oDatabase As dPMDAO.Database, ByRef r_aTable As Object) As Integer

        'Define local variables
        Dim sSQL As String
        Dim sPKConstraint As String
        Dim iCounter As Short
        Dim lRecordsAffected As Integer


        ' Debug message
        Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".CreateTable")

        CreateTable = gPMConstants.PMEReturnCode.PMTrue

        'Inform the user what's going on in ere
        'UPGRADE_WARNING: Couldn't resolve default property of object r_aTable(0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
        'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = "Creating " & r_aTable(0)
        objFrmMainForm.StatusBar_TextWrite("Creating " & r_aTable(0), 2)
        'Create table script by interogating the received data
        'UPGRADE_WARNING: Couldn't resolve default property of object r_aTable(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        sSQL = "CREATE TABLE " & r_aTable(0) & "(" & vbCrLf

        For iCounter = 0 To UBound(r_aTable(2))

            'Add the column name
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aTable()()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            sSQL = sSQL & "[" & CStr(r_aTable(2)(iCounter)(0)) & "] "

            'And the type, length, precision and scale (if appropriate)
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aTable()()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Select Case LCase(r_aTable(2)(iCounter)(1))
                ' Data type only cases
                Case LCase(gINTEGER), LCase(gDATETIME), LCase(gTINYINT), LCase(gSMALLINT), LCase(gMONEY), LCase(gTEXT), LCase(gBIT), LCase(gCAPTIONTEXT), LCase(gSTRING)
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aTable()()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sSQL = sSQL & CStr(LCase(r_aTable(2)(iCounter)(1))) & " "
                    ' Length cases
                Case LCase(gCHAR), LCase(gVARCHAR)
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aTable(2)(iCounter)(2). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aTable()()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sSQL = sSQL & CStr(LCase(r_aTable(2)(iCounter)(1))) & "(" & r_aTable(2)(iCounter)(2) & ") "
                    ' Full precision and scale cases
                Case LCase(gNUMERIC), LCase(gDECIMAL)
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aTable(2)(iCounter)(4). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aTable(2)(iCounter)(3). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aTable()()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sSQL = sSQL & CStr(LCase(r_aTable(2)(iCounter)(1))) & "(" & r_aTable(2)(iCounter)(3) & "," & r_aTable(2)(iCounter)(4) & ") "
            End Select

            'Add the 'nullability'
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aTable()()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If UCase(Trim(r_aTable(2)(iCounter)(5))) = "FALSE" Then
                sSQL = sSQL & "NOT NULL "
            Else
                sSQL = sSQL & "NULL "

            End If

            'UPGRADE_WARNING: Couldn't resolve default property of object r_aTable()()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If UCase(Trim(r_aTable(2)(iCounter)(6))) = "TRUE" Then
                sSQL = sSQL & "IDENTITY "
            End If

            'UPGRADE_WARNING: Couldn't resolve default property of object r_aTable()()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If CDbl(Trim(r_aTable(2)(iCounter)(7))) = 4 Then
                If sPKConstraint = conEmptyString Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aTable()()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aTable(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sPKConstraint = "CONSTRAINT PK__" & r_aTable(0) & " PRIMARY KEY CLUSTERED (" & vbCrLf & CStr(r_aTable(2)(iCounter)(0))
                Else
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aTable()()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sPKConstraint = sPKConstraint & conComma & vbCrLf & CStr(r_aTable(2)(iCounter)(0))
                End If
            End If

            'Add the final comma to the row if were not on the last
            'item to be added
            If iCounter <> UBound(r_aTable(2)) Then
                sSQL = sSQL & conComma & vbCrLf
            End If

        Next iCounter

        If sPKConstraint <> conEmptyString Then
            sPKConstraint = sPKConstraint & ")"
            sSQL = sSQL & conComma & vbCrLf & sPKConstraint
        End If

        'Add the final closing bracket to the end of the statement

        sSQL = sSQL & ")"

        'Execute the statement
        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="GenericInsert", bStoredProcedure:=False, lRecordsAffected:=lRecordsAffected)

        'Check for any errors
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aTable(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            MsgBox("Failed to build the UDL table " & r_aTable(0))
            CreateTable = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'Create Indexes
        For iCounter = 0 To UBound(r_aTable(2))
            sSQL = conEmptyString
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aTable()()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Select Case LCase(r_aTable(2)(iCounter)(0))
                Case "caption_id", "code"
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aTable()()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aTable(0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aTable(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sSQL = "CREATE INDEX I__" & r_aTable(0) & "__" & LCase(r_aTable(2)(iCounter)(0)) & " ON " & r_aTable(0) & "(" & LCase(r_aTable(2)(iCounter)(0)) & ")"
            End Select

            If sSQL <> conEmptyString Then
                m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="AddTableIndexes", bStoredProcedure:=False, lRecordsAffected:=lRecordsAffected)
            End If

            'Check for any errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aTable(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                MsgBox("Failed to add indexes to UDL table " & r_aTable(0))
                CreateTable = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
        Next iCounter

        ' Debug message
        Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".CreateTable")

        Exit Function

    End Function

    ' ***************************************************************** '
    '
    ' Name:        PatchControlArray
    '
    ' Description: Process to update the control array with amended table
    '              details.
    '
    ' History:     20/09/2002 JB -  Created.
    '              24/09/2002 SJP - Changed to be a function, so can
    '                               pass back if function was successful.
    '
    ' ***************************************************************** '
    Private Function PatchControlArray(ByRef r_oDatabase As dPMDAO.Database, ByRef r_aIeControl As Object, ByRef r_aIeTableDefinitions As Object, ByRef r_aCompleteTableDefinitions As Object) As Integer

        'Define the local variables
        Dim bListStarted As Boolean
        Dim iLoop As Short


        ' Debug message
        'Debug.Print Timer & ": Entering " & ACApp & conDot & ACClass & ".PatchControlArray"

        PatchControlArray = gPMConstants.PMEReturnCode.PMTrue

        '**************************************************************************

        'Patch the control array with the correct table name, previously
        'stored in a global variable
        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        r_aIeControl(pbIeControl_objectName) = g_sUDLTableName
        r_aIeControl(pbIeControl_IsIdentity) = "False"
        '**************************************************************************

        'clear any flags we might set here
        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        r_aIeControl(pbIeControl_Flags) = r_aIeControl(pbIeControl_Flags) And Not pbIeControl_Flags__IsCaption

        If IsArray(r_aCompleteTableDefinitions) Then
            For iLoop = 0 To UBound(r_aCompleteTableDefinitions)
                'Patch the control array with the correct Identity indicator relating
                'to the table name above.  This will enable the correct SQL statement
                'to be generated
                If Not CBool(r_aIeControl(pbIeControl_IsIdentity)) Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aCompleteTableDefinitions()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    r_aIeControl(pbIeControl_IsIdentity) = r_aCompleteTableDefinitions(iLoop)(udlTable_columnIsIdentity)
                End If

                '**************************************************************************
                'Patch the control array with the correct primary keys relating to
                'the table named above.  This will enable the correct SQL statements
                'to be generated
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aCompleteTableDefinitions(iLoop)(udlTable_columnPKFlag). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If r_aCompleteTableDefinitions(iLoop)(udlTable_columnPKFlag) = conPrimaryKey Then
                    If bListStarted Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        r_aIeControl(pbIeControl_PrimaryKeyColumns) = r_aIeControl(pbIeControl_PrimaryKeyColumns) & "," & iLoop + 1
                    Else
                        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        r_aIeControl(pbIeControl_PrimaryKeyColumns) = iLoop + 1
                        bListStarted = True
                    End If
                End If

                'UPGRADE_WARNING: Couldn't resolve default property of object r_aCompleteTableDefinitions()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If LCase(r_aCompleteTableDefinitions(iLoop)(udlTable_columnName)) = "caption_id" Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    r_aIeControl(pbIeControl_Flags) = r_aIeControl(pbIeControl_Flags) Or pbIeControl_Flags__IsCaption
                End If

            Next iLoop
            '**************************************************************************
            'Patch the id name in the table definition as the default is a general name
            'placeholder
            '**************************************************************************
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            r_aIeTableDefinitions(pbIeTableDefinitions_objectName)(0) = g_sUDLTableName & "_id"

        End If

        ' Debug message
        'Debug.Print Timer & ": Exiting " & ACApp & conDot & ACClass & ".PatchControlArray"

        Exit Function

    End Function


    ' ***************************************************************** '
    '
    ' Name: DropExistingTables
    '
    ' Description:
    '
    ' History: 29/06/2010 JB     - Created.

    '
    ' ***************************************************************** '
    Private Sub DropExistingTables(ByVal v_sTableName As String, ByRef r_lRetval As Object, ByRef r_oDatabase As dPMDAO.Database)

        ' Debug message
        Debug.Print(VB.Timer() & ": Entering " & ACApp & "." & ACClass & ".DropExistingTables")


        Dim sSQL As String

        If r_lRetval <> gPMConstants.PMEReturnCode.PMTrue Then Exit Sub

        r_oDatabase.Parameters.Clear()

        sSQL = ACDropTableName

        AddParameter(r_oDatabase, sSQL, m_lReturn, "sName", v_sTableName, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:=ACDropTableName, bStoredProcedure:=ACDropTableStored)
        End If

        ' Debug message
        Debug.Print(VB.Timer() & ": Exiting " & ACApp & "." & ACClass & ".DropExistingTables")

        Exit Sub

    End Sub
End Module