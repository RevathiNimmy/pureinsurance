Option Strict Off
Option Explicit On

Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Windows.Forms
Imports SharedFiles

Module pbImportHeader

    Private Const ACClass As String = conEmptyString

    Private m_lReturn As gPMConstants.PMEReturnCode

    ' ***************************************************************** '
    '
    ' Name:        ImportHeader
    '
    ' Description: Processes the import of Header information from the binary
    '              file.  Receives the current control file array row and
    '              the file number of the file being processed
    '
    ' History:     30/08/2002 JB  - Created.
    '              07/10/2002 SJP - Included Data Model Id in the export
    '                               file, in array element 1 (after import).
    '
    ' ***************************************************************** '
    Public Function ImportHeader(ByRef r_oDatabase As dPMDAO.Database, _
                                 ByVal v_iFileNumber As Integer, _
                                 ByVal v_aIeControl() As Object, _
                                 ByVal v_aIeTableDefinitions As Object, _
                                 ByVal v_lIntegerData As Integer, ByRef r_lDataModelId As Integer, _
                                 ByRef r_sDataModelCode As String, _
                                 ByRef r_iMode As Integer, _
                                 ByVal v_sVersionNumber As String, _
                                 ByRef r_lTotalLines As Integer) As Integer


        'Define array to hold the retrieved data
        Dim nResult As Integer = 0
        Dim oRetrievedData As Object

        'Check if the header information is correct

        Dim vDBSource(,) As Object
        Dim sDBSource, sTemp1, sTemp2 As String
        Dim vDBError As Object
        Dim nPosition, iLoop As Integer

        Try

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ImportHeader")

            nResult = gPMConstants.PMEReturnCode.PMTrue

            'Set the panel to indicate the  type of import
            With objFrmMainForm
                .StatusBar1(1).Items.Item(0).Text = "Header"

                .StatusBar1(2).Items.Item(0).Text = CStr(v_aIeControl(v_lIntegerData)(pbIeControl_objectName))
            End With

            'Read a row passing in the definition for the row

            m_lReturn = CType(GetBinaryFileRow(v_iFileNumber:=v_iFileNumber, v_vObjectType:=CInt(v_aIeControl(v_lIntegerData)(0)), r_aDataDefinition:=v_aIeTableDefinitions(v_lIntegerData)(pbIeTableDefinitions_columnArray), aRetrievedData:=oRetrievedData, rowIndex:=v_lIntegerData), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' ***************************************************************** '

            'Splat the header information to the import page

            'r_iMode = CInt(CStr(aRetrievedData(0)))
            r_iMode = buildExportMode()
            'set the GUI
            SetGUIOptionsBasedOnMode(r_iMode)

            'total lines in file

            r_lTotalLines = CInt(oRetrievedData(0))

            'Find out the data model id.

            If Not (CStr(oRetrievedData(2)) = conEmptyString) Then

                r_lDataModelId = CInt(oRetrievedData(2))
            Else
                writeToStatusBox("No data model id has been supplied in the export file")
            End If

            'Find out the data model code.

            If Not (CStr(oRetrievedData(3)) = conEmptyString) Then

                r_sDataModelCode = CStr(oRetrievedData(3))
                g_DataModelCode = r_sDataModelCode
            End If

            ' Find out which system is handled by export file

            g_bIsUnderwriting = CBool(oRetrievedData(4))

            ' Find out version number of export tool used to create export file

            v_sVersionNumber = CStr(oRetrievedData(5))

            sDBSource = CStr(oRetrievedData(7))
            iLoop = 0

            If sDBSource.Length > 0 Then
                Do
                    nPosition = (sDBSource.IndexOf(conComma) + 1)
                    vDBSource = ArraysHelper.RedimPreserve(Of Object(,))(vDBSource, New Integer() {conVersion - conName + 1, iLoop + 1}, New Integer() {conName, 0})

                    sTemp1 = sDBSource.Substring(0, nPosition - 1)
                    sDBSource = sDBSource.Substring(nPosition)
                    nPosition = (sDBSource.IndexOf(conComma) + 1)
                    If nPosition <> 0 Then
                        sTemp2 = sDBSource.Substring(0, nPosition - 1)
                        sDBSource = sDBSource.Substring(nPosition)
                    Else
                        sTemp2 = sDBSource
                    End If

                    ' System Name

                    vDBSource(conName, iLoop) = sTemp1
                    ' DB Version

                    vDBSource(conVersion, iLoop) = sTemp2

                    iLoop += 1
                Loop Until nPosition = 0

                m_lReturn = CType(CompareDBVersions(r_oDatabase:=r_oDatabase, v_vSource:=vDBSource, r_vErrorList:=vDBError), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If Information.IsArray(vDBError) Then

                m_lReturn = CType(BuildHeaderConfirmationText(txtConfirmationText:=objFrmMainForm.txtImportConfirmation, v_sVersionNumber:=v_sVersionNumber, v_sComments:=CStr(oRetrievedData(6)), v_vVersionError:=vDBError, v_lTotalLines:=r_lTotalLines, nExportedMode:=CInt(oRetrievedData(8))), gPMConstants.PMEReturnCode)
            Else

                m_lReturn = CType(BuildHeaderConfirmationText(txtConfirmationText:=objFrmMainForm.txtImportConfirmation, v_sVersionNumber:=v_sVersionNumber, v_sComments:=CStr(oRetrievedData(6)), v_lTotalLines:=r_lTotalLines, nExportedMode:=CInt(oRetrievedData(8))), gPMConstants.PMEReturnCode)
            End If



            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' ***************************************************************** '

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".ImportHeader")

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".ImportHeader")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ImportHeader Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportHeader", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name:        CompareDBVersions
    '
    ' Description: Checks if database version information received from
    '              source database is the same as that contained in the
    '              target database.  If not, then will throw up a warning
    '              before user decides if to proceed with import.
    '
    ' History:     01/10/2002 SJP - Created.
    '
    ' ***************************************************************** '
    Private Function CompareDBVersions(ByRef r_oDatabase As dPMDAO.Database, ByVal v_vSource(,) As Object, ByRef r_vErrorList() As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vTarget(,) As Object
        Dim iError As Integer
        Dim vErrorList() As Object



        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".CompareDBVersions")

        result = gPMConstants.PMEReturnCode.PMTrue

        If Information.IsArray(v_vSource) Then
            iError = 0
            For iLoopSource As Integer = 0 To v_vSource.GetUpperBound(1)

                sSQL = "SELECT version FROM PMLogicalDatabase" & Strings.Chr(13) & Strings.Chr(10) & _
                       "WHERE UPPER(name) = '" & CStr(v_vSource(conName, iLoopSource)).ToUpper() & "'"

                m_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetDBVersions", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vTarget)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Information.IsArray(vTarget) Then

                    If Not (CStr(v_vSource(conVersion, iLoopSource)) = CStr(vTarget(0, 0))) Then
                        ReDim Preserve vErrorList(iError)

                        vErrorList(iError) = v_vSource(conName, iLoopSource)
                        iError += 1
                    End If
                End If
            Next iLoopSource
        End If

        If iError > 0 Then
            r_vErrorList = vErrorList
        End If

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".CompareDBVersions")

        Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: SetGUIOptionsBasedOnMode
    '
    ' Description:
    '
    ' History: 02/04/2003 CLG - Created.
    '
    ' ***************************************************************** '
    Public Sub SetGUIOptionsBasedOnMode(ByVal r_iMode As Integer)

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".SetGUIOptionsBasedOnMode")

        Try

            objFrmMainForm.radioExportBasedOn(radioExportBasedOn_DataModel).Checked = r_iMode And pbIeMode_DataModel
            objFrmMainForm.radioExportBasedOn(radioExportBasedOn_Screen).Checked = r_iMode And pbIeMode_Screen
            objFrmMainForm.radioExportBasedOn(radioExportBasedOn_Scheme).Checked = r_iMode And pbIeMode_Scheme
            objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_Registry).CheckState = IIf(r_iMode And pbIeMode_Registry, 1, 0)
            objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_Documents).CheckState = IIf(r_iMode And pbIeMode_Documents, 1, 0)
            objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_Rulefiles).CheckState = IIf(r_iMode And pbIeMode_RuleFiles, 1, 0)
            objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_RiskGroupsCodes).CheckState = IIf(r_iMode And pbIeMode_RiskGroupsCodes, 1, 0)
            objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_3DRatings).CheckState = IIf(r_iMode And pbIeMode_3DLookups, 1, 0)
            objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_PBDocsOnly).CheckState = IIf(r_iMode And pbIeMode_PBDocsOnly, 1, 0)
            objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_UDLs).CheckState = IIf(r_iMode And pbIeMode_UDLs, 1, 0)
            If Not g_bIsUnderwriting Then objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_PBDocsOnly).Enabled = True

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".SetGUIOptionsBasedOnMode")

        Catch excep As System.Exception

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".SetGUIOptionsBasedOnMode")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetGUIOptionsBasedOnMode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetGUIOptionsBasedOnMode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
End Module
