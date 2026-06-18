Option Strict Off
Option Explicit On

Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Windows.Forms
Imports SharedFiles

Module pbImportGisList
    
    Private Const ACClass As String = conEmptyString
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' ***************************************************************** '
    '
    ' Name:        ImportGisList
    '
    ' Description: Processes the import of Gis List information from the binary
    '              file.  Recieves the current control file array row and
    '              the file number of the file being processed
    '
    ' History:     30/08/2002 JB  - Created.
    '              24/09/2002 SJP - Changed to be a function, so can
    '                               pass back if function was successful.
    '
    ' ***************************************************************** '
    Public Function ImportGisList(ByVal v_iFileNumber As Integer, ByVal v_aIeControl As Object, ByVal v_aIeTableDefinitions As Object, ByVal v_lIntegerData As Integer, ByVal v_sDataModelCode As String) As Integer

        'Define array to hold the retrieved data
        Dim result As Integer = 0

        'Dim aRetrievedData() As String
        Dim aRetrievedData As Object
        Dim sPathName, sSubKey As String

        Try

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ImportGisList")

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set the panel to indicate the  type of import
            With objFrmMainForm
                .StatusBar1(1).Items.Item(0).Text = "ImportGisList"
                .StatusBar1(2).Items.Item(0).Text = conEmptyString

                'Read a row passing in the definition for the row

                m_lReturn = CType(GetBinaryFileRow(v_iFileNumber:=v_iFileNumber, v_vObjectType:=CInt(v_aIeControl(v_lIntegerData)(0)), r_aDataDefinition:=v_aIeTableDefinitions(v_lIntegerData)(pbIeTableDefinitions_columnArray), aRetrievedData:=aRetrievedData, rowIndex:=v_lIntegerData), gPMConstants.PMEReturnCode)

                .StatusBar1(2).Items.Item(0).Text = CStr(aRetrievedData(0))

            End With

            sSubKey = GISSharedConstants.ACOIMGISSubKey & "\" & v_sDataModelCode & "\" & "ListManagement"

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="ServerListFilePath", r_sSettingValue:=sPathName, v_sSubKey:=sSubKey)
            End If

            If Right(sPathName, 1) <> "\" Then
                sPathName += "\"
            End If

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = WriteCompressedFile(v_sPathName:=sPathName & aRetrievedData(0), r_vTheData:=CStr(aRetrievedData(2)), v_lTheOriginalSize:=CInt(aRetrievedData(1)))
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ImportGisList")

            Return result

        Catch excep As System.Exception

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".ImportGisList")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ImportGisList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportGisList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
End Module
