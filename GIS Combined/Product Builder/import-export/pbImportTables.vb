Option Strict Off
Option Explicit On

Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Windows.Forms
Imports SharedFiles

Module pbImportTables

    ' RAW 02/09/2003 : CQ2158 : added delete cascade where missing and drop and rebuild FK when importing tables that already exist

    Private m_lReturn As gPMConstants.PMEReturnCode

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
    Public Function ImportFixedTables(ByRef r_oDatabase As dPMDAO.Database, ByVal v_iFileNumber As Integer, ByVal v_iMode As Integer, ByVal v_lDataModelId As Integer, ByVal v_sDataModelCode As String, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByVal iTableIndex As Integer) As Integer

        'Define array to hold the retrieved data
        Dim result As Integer = 0
        Dim aRetrievedData As Object
        Dim sErrorMessage As String = ""

        Try

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ImportFixedTables")

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set the panel to indicate the  type of import
            objFrmMainForm.StatusBar1(1).Items.Item(0).Text = "Database table"

            objFrmMainForm.StatusBar1(2).Items.Item(0).Text = CStr(r_aIeControl(iTableIndex)(pbIeControl_objectName))

            If CStr(r_aIeControl(iTableIndex)(pbIeControl_objectName)) = "business_type" Then
            End If

            'Read a row at a time passing in the definition for the row

            m_lReturn = CType(GetBinaryFileRow(v_iFileNumber:=v_iFileNumber, v_vObjectType:=CInt(r_aIeControl(iTableIndex)(0)), r_aDataDefinition:=r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_columnArray), aRetrievedData:=aRetrievedData, rowIndex:=iTableIndex), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
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

            If CStr(r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_objectName)) = "pmlogicaldatabase" Or (r_aIeControl(iTableIndex)(pbIeControl_operationMode)) = pbIeMode_DoNotExport Then

                'Check the database versions match
                'This checking is now done on the import confirmation tab.
                'Displays any database version discrepancies and leaves it up to the user
                'if they still wish to risk an import into a potentially incompatible database

                'm_lReturn = CheckDatabaseVersion(r_oDatabase:=r_oDatabase, _
                'r_aIeControl:=r_aIeControl(iTableIndex), _
                'r_aIeTableDefinitions:=r_aIeTableDefinitions(iTableIndex), _
                'r_iTableIndex:=iTableIndex, _
                'r_aRetrievedData:=aRetrievedData)

            Else
                'otherwise process table as normal

                'Call a common routine to prepare and execute the SQL
                'needed to update or insert the data into the table

                r_oDatabase.SQLBeginTrans()

                If PrepareSQLStatements(r_oDatabase:=r_oDatabase, r_aIeControl:=r_aIeControl(iTableIndex), r_aIeTableDefinitions:=r_aIeTableDefinitions(iTableIndex), r_iTableIndex:=iTableIndex, r_aRetrievedData:=aRetrievedData, r_sErrorString:=sErrorMessage) = gPMConstants.PMEReturnCode.PMTrue Then

                    r_oDatabase.SQLCommitTrans()
                Else
                    writeToStatusBox("Error: " & sErrorMessage)
                    r_oDatabase.SQLRollbackTrans()
                End If

            End If
            '*****************************************************************

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".ImportFixedTables")
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".ImportFixedTables")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ImportFixedTables Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportFixedTables", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

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
    Public Function ImportUserTables(ByRef r_oDatabase As dPMDAO.Database, ByVal v_sGISDMCode As String, ByVal v_lDataModelId As Integer) As Integer

        Dim result As Integer = 0
        Dim bGISMaintainDataDictionary As Object

        Dim oObjectManager As bObjectManager.ObjectManager
        Dim vGISObject, vGISProperty As Object

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ImportUserTables")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set the panel to indicate the type of import
            objFrmMainForm.StatusBar1(0).Items.Item(0).Text = "Import"
            objFrmMainForm.StatusBar1(1).Items.Item(0).Text = "User-defined Database tables"
            objFrmMainForm.StatusBar1(2).Items.Item(0).Text = conEmptyString

            oObjectManager = New bObjectManager.ObjectManager()
            oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Get instance of GIS Data Dictionary object
            ' we will use this for generating the tables and fields
            ' it saves trying to duplicate the code here.            
            result = oObjectManager.GetInstance(bGISMaintainDataDictionary, "bGISMaintainDataDictionary.Business", vInstanceManager:="ClientManager")

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            bGISMaintainDataDictionary.GISDataModelID = v_lDataModelId
            bGISMaintainDataDictionary.GISDataModel = v_sGISDMCode

            'Get the arrays for the data we've just imported into the GIS_Object and GIS_Property table

            result = bGISMaintainDataDictionary.GetObjectAndPropertyDetails(r_vGISObject:=vGISObject, r_vGISProperty:=vGISProperty)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            'Call the standard Update routine, letting it know that we are calling from PIE
            'this will generate/amend all the necessary tables

            result = bGISMaintainDataDictionary.Update(r_vGISObject:=vGISObject, r_vGISProperty:=vGISProperty, v_lSingleObjectId:=-1, bFromPIE:=True)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                'Richard Clarke
                objFrmMainForm.txtWarning_TextWrite("Unable to rebuild datamodel, please check and correct on source system, then export again.", 0)
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".ImportUserTables")
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ImportUserTables Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportUserTables", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        Finally
            bGISMaintainDataDictionary.Dispose()
            bGISMaintainDataDictionary = Nothing
            oObjectManager.Dispose()
            oObjectManager = Nothing
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

        Dim result As Integer = 0
        Dim oMaint As bGISLookupManager.Maintain
        Dim sDataModelCode As String = ""

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".CreateLookupFile")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oMaint = New bGISLookupManager.Maintain()

            If oMaint Is Nothing Then
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start LookupManager object", vApp:="TestData", vClass:="Interface", vMethod:="CreateLookupFile", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oMaint.initialise(r_oDatabase)

            'Default
            sDataModelCode = v_sDataModelCode.TrimEnd()

            If sDataModelCode <> "" Then
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    'can error if no lookups, there might be no lookups!
                    Try
                        m_lReturn = oMaint.BuildLookupCacheFiles(sDataModelCode, "1", CType(True, gPMConstants.PMEReturnCode))

                    Catch
                    End Try

                End If
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oMaint = Nothing

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".CreateLookupFile")

            Return result

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".CreateLookupFile")
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateLookupFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateLookupFile", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
            Return result

        End Try
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
    Public Function ImportFixedTableDefinition(ByRef r_oDatabase As dPMDAO.Database, ByVal v_iFileNumber As Integer, ByVal v_iMode As Integer, ByVal v_lDataModelId As Integer, ByVal v_sDataModelCode As String, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByVal iTableIndex As Integer) As Integer

        'Define array to hold the retrieved data
        Dim result As Integer = 0
        Dim aRetrievedData As Object

        Try

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ImportFixedTableDefinition")

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set the panel to indicate the  type of import
            objFrmMainForm.StatusBar1(1).Items.Item(0).Text = "Fixed Database definition"

            'Read a row at a time passing in the definition for the row

            m_lReturn = CType(GetBinaryFileRow(v_iFileNumber:=v_iFileNumber, v_vObjectType:=CInt(r_aIeControl(iTableIndex)(0)), r_aDataDefinition:=r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_columnArray), aRetrievedData:=aRetrievedData, rowIndex:=iTableIndex), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If CStr(r_aIeControl(CInt(aRetrievedData(0)))(pbIeControl_objectName)) = "gis_screen" Then
            End If

            objFrmMainForm.StatusBar1(2).Items.Item(0).Text = CStr(r_aIeControl(CInt(aRetrievedData(0)))(pbIeControl_objectName))

            '*****************************************************************
            'we now have the column names from the source database
            'these are the only one we can update in the target so re-build the target table definitions using
            'only the source columns and in the order they are defined here.

            result = GetColumnDetails(r_oDatabase:=r_oDatabase, sObjectName:=CStr(g_aIeControl(CInt(aRetrievedData(0)))(pbIeControl_objectName)), v_iTableIndex:=CInt(aRetrievedData(0)), r_aIeControl:=g_aIeControl, r_aIeTableDefinitions:=g_aIeTableDefinitions, v_sColumnFilter:=CStr(aRetrievedData(1)))

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".ImportFixedTableDefinition")
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".ImportFixedTableDefinition")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ImportFixedTableDefinition Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportFixedTableDefinition", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Module
