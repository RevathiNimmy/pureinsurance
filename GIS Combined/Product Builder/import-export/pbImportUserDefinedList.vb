Option Strict Off
Option Explicit On

Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Text
Imports System.Windows.Forms
Imports SharedFiles

Module pbImportUserDefinedList

    Public g_sUDLTableName As String = ""
    Public g_sUDLCompleteTableDefinition() As Object

    Private Const ACClass As String = conEmptyString
    Private m_lReturn As gPMConstants.PMEReturnCode

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
    Public Function ImportUserDefinedList(ByRef r_oDatabase As dPMDAO.Database, ByVal v_iFileNumber As Integer, ByVal v_iMode As Integer, ByVal v_lDataModelId As Integer, ByVal v_sDataModelCode As String, ByVal r_aIeControl As Object, ByVal r_aIeTableDefinitions() As Object, ByVal iTableIndex As Integer) As Integer

        'Define array to hold the retrieved data
        Dim result As Integer = 0
        Dim aRetrievedData As Object

        'Dim aUDLTableDefinition As String = ""
        Dim aUDLTableDefinition As Object
        Dim iUpper As Integer

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ImportUserDefinedList")
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            'Set the panel to indicate the type of import
            With objFrmMainForm
                'TBD get rid of this
                '.StatusBar1(1).Panels(1).Text = "User Defined List"
                '.StatusBar1(2).Panels(1).Text = r_aIeControl(pbIeControl_objectName)
            End With

            'Identify which table definition to use when extracting the UDL list data
            'from the binary file definition

            Try

                iUpper = g_sUDLCompleteTableDefinition.GetUpperBound(0)

                If Information.Err().Number Then
                    'Set the table definition to the 'standard' definition

                    aUDLTableDefinition = r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)
                    Information.Err().Clear()
                Else
                    'Set the table  to the 'special' definition

                    aUDLTableDefinition = g_sUDLCompleteTableDefinition(pbIeTableDefinitions_columnArray)
                End If

            Catch exc As System.Exception
                If Information.Err().Number Then
                    'Set the table definition to the 'standard' definition

                    aUDLTableDefinition = r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)
                    Information.Err().Clear()
                Else
                    'Set the table  to the 'special' definition

                    aUDLTableDefinition = g_sUDLCompleteTableDefinition(pbIeTableDefinitions_columnArray)
                End If
            End Try

            'Read a row passing in the definition for the row

            m_lReturn = CType(GetBinaryFileRow(v_iFileNumber:=v_iFileNumber, v_vObjectType:=CInt(r_aIeControl(pbIeControl_objectId)), r_aDataDefinition:=aUDLTableDefinition, aRetrievedData:=aRetrievedData, rowIndex:=iTableIndex), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '********************************************************
            'Process the headers, special headers and data rows differently
            'depending on ...

            Select Case iTableIndex
                Case pbIeDbt_UserDefinedListHeader
                    'Call rountine to process a standard header

                    m_lReturn = CType(ProcessUDLHeader(r_oDatabase:=r_oDatabase, r_aIeControl:=r_aIeControl, v_aRetrievedData:=aRetrievedData), gPMConstants.PMEReturnCode)
                Case pbIeDbt_UserDefinedList
                    'Call rountine to process data row using the identified
                    'table definition.
                    m_lReturn = CType(ProcessUDLList(r_oDatabase:=r_oDatabase, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=aUDLTableDefinition, r_aCompleteTableDefinitions:=g_sUDLCompleteTableDefinition, r_iTableIndex:=iTableIndex, r_aRetrievedData:=aRetrievedData), gPMConstants.PMEReturnCode)
                Case Else
                    'Unknown type has been received
                    MessageBox.Show("An unknown UDL type of" & iTableIndex & " has been supplied." & CStr(MsgBoxStyle.OkOnly), Application.ProductName)
                    Environment.Exit(0)
            End Select

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ImportUserDefinedList")

            Return result

        Catch ex As Exception

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".ImportUserDefinedList")
            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ImportUserDefinedList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportUserDefinedList", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
            Return result

        End Try
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

    Private Function ProcessUDLHeader(ByRef r_oDatabase As dPMDAO.Database, ByRef r_aIeControl As Object, ByVal v_aRetrievedData() As Object) As Integer

        Dim result As Integer = 0
        

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ProcessUDLHeader")

            result = gPMConstants.PMEReturnCode.PMTrue
            Dim sColumnNames As New StringBuilder

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

            g_sUDLTableName = CStr(v_aRetrievedData(0))

            '    If Left(LCase(g_sUDLTableName), 7) = "udl_cor" Then
            ':
            '    End If
            '

            '**************************************************************************

            'Check for any special table definitions.  If one exists, a specific
            'table definition must be used to retrieve the subsequent data.  This
            'definition needs to be re-constructed from the data stored in the second
            'field in the header record.

        Dim aWorkingArray() As Object
            Dim aTemp2() As Object
            Dim bExistsOnTarget As Boolean
            If Strings.Len(CStr(v_aRetrievedData(1))) > 0 Then

                'Define local variables
                'Dim aTemp1() As Variant
                'Dim aTable() As Variant

                'Split the string into chunks/item/bits

                aWorkingArray = CStr(v_aRetrievedData(1)).Split(New String() {conComma}, StringSplitOptions.None)

                'Reconstruct a table definition array from the supplied string
                'add column name, type and size to array

                'Add the basic details to the new array. Assume that these will be
                'fixed (at least for now)
                addToArray(g_sUDLCompleteTableDefinition, g_sUDLTableName)

                'Set the panel to indicate the type of import
                objFrmMainForm.StatusBar1(1).Items.Item(0).Text = "User Defined List"
                objFrmMainForm.StatusBar1(2).Items.Item(0).Text = g_sUDLTableName

                addToArray(g_sUDLCompleteTableDefinition, pbIeDbt_UserDefinedList)

                'Loop around the working array adding groups of three elements
                'as a sub array to the temporary array

                ' For iIndex As Integer = 0 To aWorkingArray.GetUpperBound(0) Step 8
                For iIndex As Integer = 0 To UBound(aWorkingArray, 1) Step 8

                    'store the column names

                    'Modified by Vijay Pal on 5/27/2010 12:52:35 PM similar to Latest guide no. 136
                    'sColumnNames.Append(CStr(aWorkingArray(iIndex)).Trim() & ",")
                    sColumnNames.Append(aWorkingArray(iIndex).ToString.Trim() & ",")

                    'Add elements of the received data string to the special
                    'definitions to the temporary
                    '            Call addToArray(aTemp1, Array(Trim(aWorkingArray(iIndex)), _
                    ''                                          Trim(aWorkingArray(iIndex + 1)), _
                    ''                                          Trim(aWorkingArray(iIndex + 2))))

                    'Add elements of the received data string to the table
                    'definitions array

                    'Modified by Vijay Pal on 5/27/2010 12:50:16 PM similar to Latest guide no.136 
                    'addToArray(aTemp2, New Object(){CStr(aWorkingArray(iIndex + udlTable_columnName)).Trim(), CStr(aWorkingArray(iIndex + udlTable_columnDatatype)).Trim(), CStr(aWorkingArray(iIndex + udlTable_columnSize)).Trim(), CStr(aWorkingArray(iIndex + udlTable_columnPrecision)).Trim(), CStr(aWorkingArray(iIndex + udlTable_columnScale)).Trim(), CStr(aWorkingArray(iIndex + udlTable_columnNullability)).Trim(), CStr(aWorkingArray(iIndex + udlTable_columnIsIdentity)).Trim(), CStr(aWorkingArray(iIndex + udlTable_columnPKFlag)).Trim()})
                    addToArray(aTemp2, New Object() {aWorkingArray(iIndex + udlTable_columnName).ToString.Trim(), aWorkingArray(iIndex + udlTable_columnDatatype).ToString.Trim(), aWorkingArray(iIndex + udlTable_columnSize).ToString.Trim(), aWorkingArray(iIndex + udlTable_columnPrecision).ToString.Trim(), aWorkingArray(iIndex + udlTable_columnScale).ToString.Trim(), aWorkingArray(iIndex + udlTable_columnNullability).ToString.Trim(), aWorkingArray(iIndex + udlTable_columnIsIdentity).ToString.Trim(), aWorkingArray(iIndex + udlTable_columnPKFlag).ToString.Trim()})

                Next iIndex

                'Need this information for the UDL list control array patch
                addToArray(g_sUDLCompleteTableDefinition, aTemp2)

                'add the column names onto the table definition
                addToArray(g_sUDLCompleteTableDefinition, sColumnNames.ToString().Substring(0, sColumnNames.ToString().Length - 1))

                '**************************************************************************


                If g_sUDLTableName.ToUpper().StartsWith("UDL") Then
                    m_lReturn = CType(ExistsOnTarget(r_oDatabase:=r_oDatabase, v_sTableName:=g_sUDLTableName, r_bExistsOnTarget:=bExistsOnTarget), gPMConstants.PMEReturnCode)

                    If bExistsOnTarget Then
                        m_lReturn = CType(DropTable(r_oDatabase:=r_oDatabase, r_aTable:=g_sUDLCompleteTableDefinition), gPMConstants.PMEReturnCode)
                    End If

                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                        m_lReturn = CType(CreateTable(r_oDatabase:=r_oDatabase, r_aTable:=g_sUDLCompleteTableDefinition), gPMConstants.PMEReturnCode)

                    End If
                End If
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ProcessUDLHeader")

            Return result

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
    Private Function ProcessUDLList(ByRef r_oDatabase As dPMDAO.Database, ByRef r_aIeControl As Object, ByRef r_aIeTableDefinitions As Object, ByRef r_aCompleteTableDefinitions As Object, ByVal r_iTableIndex As Integer, ByRef r_aRetrievedData As Object) As Integer

        Dim result As Integer = 0
        
            Dim sErrorMessage As String = ""

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ProcessUDLList")

            result = gPMConstants.PMEReturnCode.PMTrue

            'Patch the control array to make it look like the multi-purpose
            'UDL record looks like a specific record

            m_lReturn = CType(PatchControlArray(r_oDatabase:=r_oDatabase, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, r_aCompleteTableDefinitions:=r_aCompleteTableDefinitions(pbIeTableDefinitions_columnArray)), gPMConstants.PMEReturnCode)

            'Call a common routine to prepare and execute the SQL
            'needed to update or insert the data into the table
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = CType(PrepareSQLStatements(r_oDatabase:=r_oDatabase, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aCompleteTableDefinitions, r_iTableIndex:=r_iTableIndex, r_aRetrievedData:=r_aRetrievedData, r_sErrorString:=sErrorMessage), gPMConstants.PMEReturnCode)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                writeToStatusBox("Error : " & sErrorMessage)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ProcessUDLList")

            Return result

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
    Private Function CreateTable(ByRef r_oDatabase As dPMDAO.Database, ByRef r_aTable() As Object) As Integer

        'Define local variables
        Dim result As Integer = 0
        Dim sPKConstraint As New StringBuilder
        Dim sSQL As New StringBuilder
        Dim lRecordsAffected As Integer

        

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".CreateTable")

            result = gPMConstants.PMEReturnCode.PMTrue

            'Inform the user what's going on in ere

            objFrmMainForm.StatusBar1(2).Items.Item(0).Text = "Creating " & CStr(r_aTable(0))

            'Create table script by interogating the received data

            sSQL = New StringBuilder("CREATE TABLE " & CStr(r_aTable(0)) & "(" & Strings.Chr(13) & Strings.Chr(10))

            For iCounter As Integer = 0 To r_aTable(2).GetUpperBound(0)

                'Add the column name

                sSQL.Append("[" & CStr(r_aTable(2)(iCounter)(0)) & "] ")

                'And the type, length, precision and scale (if appropriate)

                Select Case CStr(r_aTable(2)(iCounter)(1)).ToLower()
                    ' Data type only cases
                    Case gINTEGER.ToLower(), gDATETIME.ToLower(), gTINYINT.ToLower(), gSMALLINT.ToLower(), gMONEY.ToLower(), gTEXT.ToLower(), gBIT.ToLower(), gCAPTIONTEXT.ToLower(), gSTRING.ToLower()

                        sSQL.Append(CStr(r_aTable(2)(iCounter)(1)).ToLower() & " ")
                        ' Length cases
                    Case gCHAR.ToLower(), gVARCHAR.ToLower()

                        sSQL.Append(CStr(r_aTable(2)(iCounter)(1)).ToLower() & _
                                    "(" & CStr(r_aTable(2)(iCounter)(2)) & ") ")
                        ' Full precision and scale cases
                    Case gNUMERIC.ToLower(), gDECIMAL.ToLower()

                        sSQL.Append(CStr(r_aTable(2)(iCounter)(1)).ToLower() & _
                                    "(" & CStr(r_aTable(2)(iCounter)(3)) & "," & _
                                    CStr(r_aTable(2)(iCounter)(4)) & ") ")
                End Select

                'Add the 'nullability'

                If CStr(r_aTable(2)(iCounter)(5)).Trim().ToUpper() = "FALSE" Then
                    sSQL.Append("NOT NULL ")
                Else
                    sSQL.Append("NULL ")

                End If

                If CStr(r_aTable(2)(iCounter)(6)).Trim().ToUpper() = "TRUE" Then
                    sSQL.Append("IDENTITY ")
                End If

                If StringsHelper.ToDoubleSafe(CStr(r_aTable(2)(iCounter)(7)).Trim()) = 4 Then
                    If sPKConstraint.ToString() = conEmptyString Then

                        sPKConstraint = New StringBuilder("CONSTRAINT PK__" & CStr(r_aTable(0)) & _
                                        " PRIMARY KEY CLUSTERED (" & Strings.Chr(13) & Strings.Chr(10) & _
                                        CStr(r_aTable(2)(iCounter)(0)))
                    Else

                        sPKConstraint.Append(conComma & Strings.Chr(13) & Strings.Chr(10) & _
                                             CStr(r_aTable(2)(iCounter)(0)))
                    End If
                End If

                'Add the final comma to the row if were not on the last
                'item to be added

                If iCounter <> r_aTable(2).GetUpperBound(0) Then
                    sSQL.Append(conComma & Strings.Chr(13) & Strings.Chr(10))
                End If

            Next iCounter

            If sPKConstraint.ToString() <> conEmptyString Then
                sPKConstraint.Append(")")
                sSQL.Append(conComma & Strings.Chr(13) & Strings.Chr(10) & sPKConstraint.ToString())
            End If

            'Add the final closing bracket to the end of the statement

            sSQL.Append(")")

            'Execute the statement
            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="GenericInsert", bStoredProcedure:=False, lRecordsAffected:=lRecordsAffected)

            'Check for any errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                MessageBox.Show("Failed to build the UDL table " & CStr(r_aTable(0)), Application.ProductName)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Create Indexes

            For iCounter As Integer = 0 To r_aTable(2).GetUpperBound(0)
                sSQL = New StringBuilder(conEmptyString)

                Select Case CStr(r_aTable(2)(iCounter)(0)).ToLower()
                    Case "caption_id", "code"

                        sSQL = New StringBuilder("CREATE INDEX I__" & CStr(r_aTable(0)) & "__" & _
                               CStr(r_aTable(2)(iCounter)(0)).ToLower() & " ON " & CStr(r_aTable(0)) & _
                               "(" & CStr(r_aTable(2)(iCounter)(0)).ToLower() & ")")
                End Select

                If sSQL.ToString() <> conEmptyString Then
                    m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="AddTableIndexes", bStoredProcedure:=False, lRecordsAffected:=lRecordsAffected)
                End If

                'Check for any errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    MessageBox.Show("Failed to add indexes to UDL table " & CStr(r_aTable(0)), Application.ProductName)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Next iCounter

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".CreateTable")

            Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name:        DropTable
    '
    ' Description: Process to build appropriate SQL statement to drop
    '              a  table on the target database
    '
    '
    ' ***************************************************************** '
    Private Function DropTable(ByRef r_oDatabase As dPMDAO.Database, ByRef r_aTable() As Object) As Integer

        'Define local variables
        Dim result As Integer = 0
        Dim sSQL As New StringBuilder
        Dim lRecordsAffected As Integer

        

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".DropTable")

            result = gPMConstants.PMEReturnCode.PMTrue

            'Inform the user what's going on in ere

            objFrmMainForm.StatusBar1(2).Items.Item(0).Text = "Drop " & CStr(r_aTable(0))

            'Create table script by interogating the received data

            sSQL = New StringBuilder("DROP TABLE " & CStr(r_aTable(0)) & Strings.Chr(13) & Strings.Chr(10))


            'Execute the statement
            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="GenericInsert", bStoredProcedure:=False, lRecordsAffected:=lRecordsAffected)

            'Check for any errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                MessageBox.Show("Failed to build the UDL table " & CStr(r_aTable(0)), Application.ProductName)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".DropTable")

            Return result

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
    Private Function PatchControlArray(ByRef r_oDatabase As dPMDAO.Database, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByRef r_aCompleteTableDefinitions() As Object) As Integer

        'Define the local variables
        Dim result As Integer = 0
        Dim bListStarted As Boolean

        

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".PatchControlArray")

            result = gPMConstants.PMEReturnCode.pmtrue

            '**************************************************************************

            'Patch the control array with the correct table name, previously
            'stored in a global variable

            r_aIeControl(pbIeControl_objectName) = g_sUDLTableName

            '**************************************************************************

            'clear any flags we might set here

            r_aIeControl(pbIeControl_Flags) = CBool(r_aIeControl(pbIeControl_Flags)) And Not pbIeControl_Flags__IsCaption

            If Information.IsArray(r_aCompleteTableDefinitions) Then
                For iLoop As Integer = 0 To r_aCompleteTableDefinitions.GetUpperBound(0)
                    'Patch the control array with the correct Identity indicator relating
                    'to the table name above.  This will enable the correct SQL statement
                    'to be generated

                    If Not CBool(r_aIeControl(pbIeControl_IsIdentity)) Then

                        r_aIeControl(pbIeControl_IsIdentity) = r_aCompleteTableDefinitions(iLoop)(udlTable_columnIsIdentity)
                    End If

                    '**************************************************************************
                    'Patch the control array with the correct primary keys relating to
                    'the table named above.  This will enable the correct SQL statements
                    'to be generated

                    If (r_aCompleteTableDefinitions(iLoop)(udlTable_columnPKFlag)) = conPrimaryKey Then
                        If bListStarted Then

                            r_aIeControl(pbIeControl_PrimaryKeyColumns) = CStr(r_aIeControl(pbIeControl_PrimaryKeyColumns)) & "," & CStr(iLoop + 1)
                        Else

                            r_aIeControl(pbIeControl_PrimaryKeyColumns) = iLoop + 1
                            bListStarted = True
                        End If
                    End If

                    If CStr(r_aCompleteTableDefinitions(iLoop)(udlTable_columnName)).ToLower() = "caption_id" Then

                        r_aIeControl(pbIeControl_Flags) = CInt(r_aIeControl(pbIeControl_Flags)) Or pbIeControl_Flags__IsCaption
                    End If

                Next iLoop
                '**************************************************************************
                'Patch the id name in the table definition as the default is a general name
                'placeholder
                '**************************************************************************

                r_aIeTableDefinitions(pbIeTableDefinitions_objectName)(0) = g_sUDLTableName & "_id"

            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".PatchControlArray")

            Return result

    End Function
End Module
