Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("SiriusCoreFunc_NET.SiriusCoreFunc")>
Public Module SiriusCoreFunc
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '
    Public Const ACClass As String = "SiriusCoreFunc"

    'DN 06/12/02
    'Branches
    'Modified by Deepak Sharma on 5/4/2010 4:27:02 PM refer developer guide no. 39(Guide)
    'Private Const ACGetBranchSQL As String = "{call spu_PM_SelAll_Source}"
    Private Const ACGetBranchSQL As String = "spu_PM_SelAll_Source"
    Private Const ACGetBranchName As String = "SelectBranches"
    Private Const ACGetBranchStored As Boolean = True

    'Sub Branches
    'Modified by Deepak Sharma on 5/4/2010 4:27:02 PM refer developer guide no. 46(Guide)
    'Private Const ACGetSubBranchSQL As String = "{call spu_sub_branch_sel (?)}"
    Private Const ACGetSubBranchSQL As String = "spu_sub_branch_sel"
    Private Const ACGetSubBranchName As String = "SelectSubBranches"
    Private Const ACGetSubBranchStored As Boolean = True

    'Number validation
    Private Const g_kRegStringScripts As String = "Scripts"
    Private Const g_kLoyaltyNumberFileName As String = "Loyalty"
    Private Const g_kScriptFileExtension As String = "txt"
    Private Const g_kAlternativeIdentifierFileName As String = "AlternativeIdentifier"

    Private m_lReturn As gPMConstants.PMEReturnCode

    ' ***************************************************************** '
    '
    ' Name: GetBranches
    '
    ' Description:
    '
    ' History: 06/12/2002 DN - Created.
    '
    ' ***************************************************************** '
    Public Function GetBranches(ByVal v_oDatabase As dPMDAO.Database, ByRef r_vBranchArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lNumberOfRecords As Integer

            With v_oDatabase
                'TODO:MILAN::
                'Clearing any parameters that the database object may carry
                .Parameters.Clear()

                m_lReturn = .SQLSelect(sSQL:=ACGetBranchSQL, sSQLName:=ACGetBranchName, bStoredProcedure:=ACGetBranchStored, lNumberRecords:=lNumberOfRecords, vResultArray:=r_vBranchArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed to GetBranches", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBranches")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBranches Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBranches", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetSubBranches
    '
    ' Description:
    '
    ' History: 11/06/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function GetSubBranches(ByVal v_oDatabase As dPMDAO.Database, ByVal v_lSourceID As Integer, ByRef r_vSubBranchArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lNumberOfRecords As Integer

            With v_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="source_id", vValue:=CStr(v_lSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLSelect(sSQL:=ACGetSubBranchSQL, sSQLName:=ACGetSubBranchName, bStoredProcedure:=ACGetSubBranchStored, lNumberRecords:=lNumberOfRecords, vResultArray:=r_vSubBranchArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed to GetSubBranches", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSubBranches")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSubBranches Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSubBranches", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetNumberValidationScripts
    '
    ' Description:
    '
    ' History: 12/06/2002 SJ - Created.
    '
    ' ***************************************************************** '
      Public Function GetNumberValidationScripts(ByVal v_sBranchPrefix As String, ByRef r_sLoyaltyNumberScript As String, ByRef r_sAlternativeIdentifierScript As String) As Integer

      Dim result As Integer = 0
      Try

          result = gPMConstants.PMEReturnCode.PMTrue

          Dim sScriptsPath As String = String.Empty

          v_sBranchPrefix = v_sBranchPrefix.Trim()

          'Get the scripts file path from the registry
          m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=g_kRegStringScripts, r_sSettingValue:=sScriptsPath), gPMConstants.PMEReturnCode)

          If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
              bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get script file path from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNumberValidationScripts")
              Return gPMConstants.PMEReturnCode.PMFalse
          End If

          sScriptsPath = sScriptsPath.Trim()

          If sScriptsPath.Trim() = "" Then
              ' No path set up just exit
              Return result
          End If

          If Not sScriptsPath.EndsWith("\") Then
              sScriptsPath = sScriptsPath & "\"
          End If

          'Get the loyalty number validation script
          m_lReturn = CType(GetNumberValidationScript(v_sBranchPrefix:=v_sBranchPrefix, v_sScriptsPath:=sScriptsPath, v_sFileName:=g_kLoyaltyNumberFileName, v_sFileExt:=g_kScriptFileExtension, r_sNumberScript:=r_sLoyaltyNumberScript), gPMConstants.PMEReturnCode)

          If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
              bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNumberValidationScript Failed for " & g_kLoyaltyNumberFileName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetNumberValidationScripts")
              Return gPMConstants.PMEReturnCode.PMFalse
          End If

          'Get the alternative identifier validation script
          m_lReturn = CType(GetNumberValidationScript(v_sBranchPrefix:=v_sBranchPrefix, v_sScriptsPath:=sScriptsPath, v_sFileName:=g_kAlternativeIdentifierFileName, v_sFileExt:=g_kScriptFileExtension, r_sNumberScript:=r_sAlternativeIdentifierScript), gPMConstants.PMEReturnCode)

          If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
              bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNumberValidationScript Failed for " & g_kAlternativeIdentifierFileName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetNumberValidationScripts")
              Return gPMConstants.PMEReturnCode.PMFalse
          End If

          Return result

      Catch excep As System.Exception



          result = gPMConstants.PMEReturnCode.PMError

          ' Log Error Message
          bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNumberValidationScripts Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNumberValidationScripts", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

          Return result


          Return result
      End Try
  End Function

  ' ***************************************************************** '
  '
  ' Name: GetNumberValidationScript
  '
  ' Description:
  '
  ' History: 12/06/2002 SJ - Created.
  '
  ' ***************************************************************** '
  Private Function GetNumberValidationScript(ByVal v_sBranchPrefix As String, ByVal v_sScriptsPath As String, ByVal v_sFileName As String, ByVal v_sFileExt As String, ByRef r_sNumberScript As String) As Integer

      Dim result As Integer = 0


      result = gPMConstants.PMEReturnCode.PMTrue

      Dim sFullScriptsPath As String
      Dim iScriptFile As Integer

      If v_sBranchPrefix <> "" Then
          'Build Path for branch specific file
          sFullScriptsPath = v_sScriptsPath & v_sFileName & "_" & v_sBranchPrefix & "." & v_sFileExt
          If FileExists(sFullScriptsPath) = False Then
              'No branch specific file use default
              sFullScriptsPath = v_sScriptsPath & v_sFileName & "." & v_sFileExt
          End If
      Else
          'No branch specific file use default
          sFullScriptsPath = v_sScriptsPath & v_sFileName & "." & v_sFileExt
      End If

      If FileExists(sFullScriptsPath) = "" Then
          'file does not exist, issue warning and exit
          bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Warning: Failed to locate " & sFullScriptsPath, vApp:=ACApp, vClass:=ACClass, vMethod:="GetNumberValidationScript")
          Return result
      End If

      ' Open and read the file
      iScriptFile = FreeFile()

      FileSystem.FileOpen(iScriptFile, sFullScriptsPath, OpenMode.Input)
      r_sNumberScript = FileSystem.InputString(iScriptFile, FileSystem.LOF(iScriptFile))
      FileSystem.FileClose(iScriptFile)

      Return result

  End Function
End Module

