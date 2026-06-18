Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports SharedFiles

Module pbImportTables

    ' RAW 02/09/2003 : CQ2158 : added delete cascade where missing and drop and rebuild FK when importing tables that already exist


    Private m_lReturn As Integer

    Private Const ACClass As String = conEmptyString


    ' ***************************************************************** '
    '
    ' Name:        ImportFixedTables
    '
    ' Description: Processes the extraction of fixed tables from the
    '              binary file.  Receives the current control file
    '              array row and the file number of the file being
    '              processed
    '
    ' History:     30/08/2002 JB  - Created.
    '              24/09/2002 SJP - Changed to be a function, so can
    '                               pass back if function was successful.
    '              08/10/2002 SJP - Commented out call to CheckDatabaseVersion
    '                               as this is carried out at earlier stage.
    '
    ' ***************************************************************** '
    Public Function ImportFixedTables(ByRef r_oDatabase As dPMDAO.Database, ByVal v_iFileNumber As Short, ByVal v_iMode As Integer, ByVal v_lDataModelID As Integer, ByVal v_sDataModelCode As String, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByVal iTableIndex As Short) As Integer

        'Richard Clarke November 2008 - PIE enhancements
        'altered function definition to use the arrays
        'ByVal v_lDataModelID As Long,
        'ByVal v_sDataModelCode As String,
        'Richard Clarke November 2008 - PIE enhancements

        'Define array to hold the retrieved data
        Dim aRetrievedData As Object
        Dim sErrorMessage As String

        Try

            ' Debug message
            'Debug.Print Timer & ": Entering " & ACApp & conDot & ACClass & ".ImportFixedTables"

            ImportFixedTables = gPMConstants.PMEReturnCode.PMTrue

            'Set the panel to indicate the  type of import
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "Database table"
            objFrmMainForm.StatusBar_TextWrite("Database table", 1)
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(iTableIndex)(pbIeControl_objectName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = r_aIeControl(iTableIndex)(pbIeControl_objectName)
            objFrmMainForm.StatusBar_TextWrite(r_aIeControl(iTableIndex)(pbIeControl_objectName), 2)
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If Trim(r_aIeControl(iTableIndex)(pbIeControl_objectName)) = Trim("PMProduct_Lookup") Then
                Debug.Print("product lookup")
            End If

            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If Trim(r_aIeControl(iTableIndex)(pbIeControl_objectName)) = Trim("Product") Then
                Debug.Print("product")
            End If

            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(iTableIndex)(pbIeControl_objectName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If r_aIeControl(iTableIndex)(pbIeControl_objectName) = "business_type" Then
            End If

            'Read a row at a time passing in the definition for the row
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(iTableIndex)(0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            m_lReturn = GetBinaryFileRow(v_iFileNumber:=v_iFileNumber, v_vObjectType:=r_aIeControl(iTableIndex)(0), r_aDataDefinition:=r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_columnArray), aRetrievedData:=aRetrievedData, rowIndex:=iTableIndex)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ImportFixedTables = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            '*****************************************************************
            'Determine if the retrieved row is one of the rows from PMLogicalDatabase
            'If so, perform database check processing before throwing
            'the line away.

            ' If r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_objectName) = "pmlogicaldatabase" Then

            ' RAG 2003-10-09
            ' Do not import tables if they are marked as "DoNotExport". This may happen if an earlier version
            ' of the tool was used, and tables are included which shouldn't be.
            ' e.g. on Folgate the gis_insurer table was included in a v1.2 export
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(iTableIndex)(pbIeControl_operationMode). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_objectName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_objectName) = "pmlogicaldatabase" Or r_aIeControl(iTableIndex)(pbIeControl_operationMode) = pbIeMode_DoNotExport Then

            Else 'otherwise process table as normal

                r_oDatabase.SQLBeginTrans()
                If PrepareSQLStatements(r_oDatabase:=r_oDatabase, r_aIeControl:=r_aIeControl(iTableIndex), r_aIeTableDefinitions:=r_aIeTableDefinitions(iTableIndex), r_iTableIndex:=iTableIndex, r_aRetrievedData:=aRetrievedData, r_sErrorString:=sErrorMessage) = gPMConstants.PMEReturnCode.PMTrue Then

                    r_oDatabase.SQLCommitTrans()
                Else
                    writeToStatusBox("Error: " & sErrorMessage)
                    r_oDatabase.SQLRollbackTrans()
                    'log the error
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    LogPIEError("Error occurred in ImportFixedTables " & Err.Number & " " & Err.Description, True, False, "", False, v_sDataModelCode, r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_objectName))
                End If

                ' BSJ Sep 2009 - After inserting the new data model get the new id's and codes
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_objectName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_objectName) = "gis_data_model" Then

                    m_lReturn = RetrieveDataModelReplacementValues(r_oDatabase:=r_oDatabase, r_aRetrievedData:=aRetrievedData, r_iCounter:=0)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        ImportFixedTables = gPMConstants.PMEReturnCode.PMFalse
                        Exit Function
                    End If
                End If
            End If
            '*****************************************************************

            ' Debug message
            'Debug.Print Timer & ": Exiting " & ACApp & conDot & ACClass & ".ImportFixedTables"
            Exit Function

        Catch ex As Exception

            ImportFixedTables = gPMConstants.PMEReturnCode.PMError

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".ImportFixedTables")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ImportFixedTables Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportFixedTables", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function


        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name:        ImportUserTables
    '
    ' Description: Processes the extraction of user defined table
    '              details from the GIS Object and GIS Property details
    '              taken from the binary file to construct the required
    '              tables.
    '
    ' History:     10/09/2002 SJP - Created.
    '              25/05/2006 DD - rewritten to just call bGISMaintainDataDictionary
    '
    ' ***************************************************************** '
    Public Function ImportUserTables(ByRef r_oDatabase As dPMDAO.Database, ByVal v_sGISDMCode As String, ByVal v_lDataModelID As Integer) As Integer

        Dim oObjectManager As bObjectManager.ObjectManager
        Dim oGISMaintainDataDictionary As Object
        Dim vGISObject As Object
        Dim vGISProperty As Object

        ' Debug message
        Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".ImportUserTables")

        Try

            ImportUserTables = gPMConstants.PMEReturnCode.PMTrue

            'Set the panel to indicate the type of import
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(0).Items.Item(1).Text = "Import"
            objFrmMainForm.StatusBar_TextWrite("Import", 0)
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "User-defined Database tables"
            objFrmMainForm.StatusBar_TextWrite("User-defined Database tables", 1)
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = conEmptyString
            objFrmMainForm.StatusBar_TextWrite(conEmptyString, 2)

            oObjectManager = New bObjectManager.ObjectManager
            oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Get instance of GIS Data Dictionary object
            ' we will use this for generating the tables and fields
            ' it saves trying to duplicate the code here.
            ImportUserTables = oObjectManager.GetInstance(oObject:=oGISMaintainDataDictionary, sClassName:="bGISMaintainDataDictionary.Business", vInstanceManager:="ClientManager")
            If ImportUserTables <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Function
            End If

            'UPGRADE_WARNING: Couldn't resolve default property of object oGISMaintainDataDictionary.GISDataModelID. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            oGISMaintainDataDictionary.GISDataModelID = v_lDataModelID
            'UPGRADE_WARNING: Couldn't resolve default property of object oGISMaintainDataDictionary.GISDataModel. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            oGISMaintainDataDictionary.GISDataModel = v_sGISDMCode

            'Get the arrays for the data we've just imported into the GIS_Object and GIS_Property table
            'UPGRADE_WARNING: Couldn't resolve default property of object oGISMaintainDataDictionary.GetObjectAndPropertyDetails. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            ImportUserTables = oGISMaintainDataDictionary.GetObjectAndPropertyDetails(r_vGISObject:=vGISObject, r_vGISProperty:=vGISProperty)
            If ImportUserTables <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Function
            End If

            'Call the standard Update routine, letting it know that we are calling from PIE
            'this will generate/amend all the necessary tables
            'UPGRADE_WARNING: Couldn't resolve default property of object oGISMaintainDataDictionary.Update. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            ImportUserTables = oGISMaintainDataDictionary.Update(r_vGISObject:=vGISObject, r_vGISProperty:=vGISProperty, v_lSingleObjectId:=-1, bFromPIE:=True)
            If ImportUserTables <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Function
            End If

            'UPGRADE_WARNING: Couldn't resolve default property of object oGISMaintainDataDictionary.Terminate. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            oGISMaintainDataDictionary.Dispose()
            'UPGRADE_NOTE: Object oGISMaintainDataDictionary may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oGISMaintainDataDictionary = Nothing
            oObjectManager.Dispose()
            'UPGRADE_NOTE: Object oObjectManager may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oObjectManager = Nothing

            ' Debug message
            Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".ImportUserTables")
            Exit Function

        Catch ex As Exception

            ImportUserTables = gPMConstants.PMEReturnCode.PMError

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".ImportUserTables")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ImportUserTables Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportUserTables", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name:        CreateLookupFile
    '
    ' Description: Create flat file of the lookup data (path is defined
    '              in the registry)
    '
    ' History:     02/10/2002 SJP - Created.
    '
    ' ***************************************************************** '
    Public Function CreateLookupFile(ByRef r_oDatabase As dPMDAO.Database, ByVal v_sDataModelCode As String) As Integer

        Dim oMaint As bGISLookupManager.Maintain
        Dim sDataModelCode As String

        On Error GoTo Err_CreateLookupFile

        ' Debug message
        Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".CreateLookupFile")

        CreateLookupFile = gPMConstants.PMEReturnCode.PMTrue

        oMaint = New bGISLookupManager.Maintain

        If oMaint Is Nothing Then
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start LookupManager object", vApp:="TestData", vClass:="Interface", vMethod:="CreateLookupFile", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            CreateLookupFile = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'UPGRADE_WARNING: Couldn't resolve default property of object oMaint.Initialise. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        oMaint.Initialise(r_oDatabase)

        'Default
        sDataModelCode = RTrim(v_sDataModelCode)

        If sDataModelCode <> "" Then
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                'can error if no lookups, there might be no lookups!
                On Error Resume Next
                'UPGRADE_WARNING: Couldn't resolve default property of object oMaint.BuildLookupCacheFiles. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                m_lReturn = oMaint.BuildLookupCacheFiles(sDataModelCode, "1", True)
                On Error GoTo Err_CreateLookupFile
            End If
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CreateLookupFile = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'UPGRADE_NOTE: Object oMaint may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        oMaint = Nothing

        ' Debug message
        Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".CreateLookupFile")

        Exit Function

Err_CreateLookupFile:

        CreateLookupFile = gPMConstants.PMEReturnCode.PMError

        ' Debug message
        Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".CreateLookupFile")

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateLookupFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateLookupFile", vErrNo:=Err.Number, vErrDesc:=Err.Description)

        Exit Function

    End Function
    ' ***************************************************************** '
    '
    ' Name:        ImportFixedTableDefinition
    '
    ' Description: Processes the extraction of fixed tables definition from the
    '              binary file.
    '              Replaces the target table column names with only those exported
    '              from the source.
    '
    ' History:     18/10/2002 CLG  - Created.
    '
    ' ***************************************************************** '
    Public Function ImportFixedTableDefinition(ByRef r_oDatabase As dPMDAO.Database, ByVal v_iFileNumber As Short, ByVal v_iMode As Integer, ByVal v_lDataModelID As Short, ByVal v_sDataModelCode As String, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByVal iTableIndex As Short) As Integer


        'Define array to hold the retrieved data
        Dim aRetrievedData As Object

        Try

            ' Debug message
            '   Debug.Print Timer & ": Entering " & ACApp & conDot & ACClass & ".ImportFixedTableDefinition"

            ImportFixedTableDefinition = gPMConstants.PMEReturnCode.PMTrue

            'Set the panel to indicate the  type of import
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "Fixed Database definition"
            objFrmMainForm.StatusBar_TextWrite("Fixed Database definition", 1)

            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If LCase(r_aIeControl(iTableIndex)(pbIeControl_objectName)) = "claim_party_type" Then
                Debug.Print("foo")
            End If

            'Read a row at a time passing in the definition for the row
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(iTableIndex)(0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            m_lReturn = GetBinaryFileRow(v_iFileNumber:=v_iFileNumber, v_vObjectType:=r_aIeControl(iTableIndex)(0), r_aDataDefinition:=r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_columnArray), aRetrievedData:=aRetrievedData, rowIndex:=iTableIndex)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ImportFixedTableDefinition = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If



            'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(aRetrievedData(0))(pbIeControl_objectName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If r_aIeControl(aRetrievedData(0))(pbIeControl_objectName) = "progress_status" Then
                Debug.Print("test")
            End If

            'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(aRetrievedData(0))(pbIeControl_objectName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If r_aIeControl(aRetrievedData(0))(pbIeControl_objectName) = "gis_screen" Then
            End If

            'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(aRetrievedData(0))(pbIeControl_objectName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = r_aIeControl(aRetrievedData(0))(pbIeControl_objectName)
            objFrmMainForm.StatusBar_TextWrite(r_aIeControl(aRetrievedData(0))(pbIeControl_objectName), 2)
            '*****************************************************************
            'we now have the column names from the source database
            'these are the only one we can update in the target so re-build the target table definitions using
            'only the source columns and in the order they are defined here.

            'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl(aRetrievedData(0))(pbIeControl_objectName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            ImportFixedTableDefinition = GetColumnDetails(r_oDatabase:=r_oDatabase, sObjectName:=g_aIeControl(aRetrievedData(0))(pbIeControl_objectName), v_iTableIndex:=aRetrievedData(0), r_aIeControl:=g_aIeControl, r_aIeTableDefinitions:=g_aIeTableDefinitions, v_sColumnFilter:=aRetrievedData(1))


            ' Debug message
            '    Debug.Print Timer & ": Exiting " & ACApp & conDot & ACClass & ".ImportFixedTableDefinition"
            Exit Function

        Catch ex As Exception

            ImportFixedTableDefinition = gPMConstants.PMEReturnCode.PMError

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".ImportFixedTableDefinition")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ImportFixedTableDefinition Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportFixedTableDefinition", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function
        End Try


    End Function
End Module