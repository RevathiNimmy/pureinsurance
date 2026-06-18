Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles
Friend Class frmMain
    Inherits System.Windows.Forms.Form

    Private g_oObjectManager As bObjectManager.ObjectManager

    Private m_lReturn As Integer
    Private g_iLanguageID As Short
    Private g_iSourceID As Short
    Private m_iTask As Short
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String
    Private m_dtEffectiveDate As Date
    Private m_lErrorNumber As Integer

    Private m_oBusiness As Object
    Private m_sDataModelCode As String

    Private Const m_iSourceid As Short = 1
    Private Const m_iLanguageID As Short = 1

    Public WriteOnly Property DataModel() As String
        Set(ByVal Value As String)
            m_sDataModelCode = Value
        End Set
    End Property

    Private Overloads Function RebuildDatamodel(ByVal v_lDataModelId As Integer, ByVal v_sDataModelCode As String) As Integer
        Return RebuildDatamodel(v_lDataModelId, False)
    End Function

    Private Overloads Function RebuildDatamodel(ByVal v_lDataModelId As Integer, ByVal v_sDataModelCode As String, ByRef bRecreateDataBackBone As Boolean) As Integer
        Try

            Dim sGISDataModelName As String
            Dim lGISDataModelType As Integer
            Dim vGISObject As Object = Nothing
            Dim vGISProperty As Object = Nothing
            Dim sPolicyBinderTable As String

            RebuildDatamodel = gPMConstants.PMEReturnCode.PMTrue

            m_oBusiness.GISDataModelId = v_lDataModelId
            m_oBusiness.GISDataModel = v_sDataModelCode

            sPolicyBinderTable = v_sDataModelCode + "_Policy_Binder"
            m_lReturn = m_oBusiness.RebuildDefaultObjects(sPolicyBinderTable)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RebuildDatamodel = gPMConstants.PMEReturnCode.PMFalse
                SharedFiles.bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to rebuild the datamodels - RebuildDefaultObjects", vApp:=ACApp, vClass:=ACClass, vMethod:="RebuildDatamodel")
                Exit Function
            End If

            m_lReturn = m_oBusiness.GetDataModelDetails()
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RebuildDatamodel = gPMConstants.PMEReturnCode.PMFalse
                SharedFiles.bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object - GetDataModelDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="RebuildDatamodel")
                Exit Function
            End If

            lGISDataModelType = m_oBusiness.GISDataModelType
            m_lReturn = m_oBusiness.GetObjectAndPropertyDetails(r_vGISObject:=vGISObject, r_vGISProperty:=vGISProperty)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                    RebuildDatamodel = gPMConstants.PMEReturnCode.PMFalse
                    SharedFiles.bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object - GetObjectAndPropertyDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="RebuildDatamodel")
                    Exit Function
                End If
            End If

            sGISDataModelName = m_oBusiness.GISDataModelName
            m_oBusiness.GISDataModel = v_sDataModelCode
            m_lReturn = m_oBusiness.Update(r_vGISObject:=vGISObject, r_vGISProperty:=vGISProperty, v_lSingleObjectId:=-1)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RebuildDatamodel = gPMConstants.PMEReturnCode.PMFalse
                SharedFiles.bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the business object - Update", vApp:=ACApp, vClass:=ACClass, vMethod:="RebuildDatamodel")
                Exit Function
            End If

            m_lReturn = CreateRegistrySettings(v_sDataModelCode)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RebuildDatamodel = gPMConstants.PMEReturnCode.PMFalse
                SharedFiles.bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create the registry settings", vApp:=ACApp, vClass:=ACClass, vMethod:="RebuildDatamodel")
                Exit Function
            End If

            m_lReturn = CreateDataSet(v_sDataModelCode, bRecreateDataBackBone)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RebuildDatamodel = gPMConstants.PMEReturnCode.PMFalse
                SharedFiles.bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create the dataset", vApp:=ACApp, vClass:=ACClass, vMethod:="RebuildDatamodel")
                Exit Function
            End If

        Catch ex As Exception
            RebuildDatamodel = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            SharedFiles.bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to rebuild the datamodel - '" & v_sDataModelCode & "'", vApp:=ACApp, vClass:=ACClass, vMethod:="RebuildDatamodel", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)
        End Try

    End Function

    Private Function GetBusiness() As Object

        GetBusiness = gPMConstants.PMEReturnCode.PMTrue

        m_oBusiness = New bGISMaintainDataDictionary.Business

        m_lReturn = m_oBusiness.SetProcessModes(vTask:=CObj(m_iTask), vNavigate:=CObj(m_lNavigate), vProcessMode:=CObj(m_lProcessMode), vTransactionType:=CObj(m_sTransactionType), vEffectiveDate:=CObj(m_dtEffectiveDate))

        ' Check for errors.
        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then

            GetBusiness = gPMConstants.PMEReturnCode.PMFalse

            SharedFiles.bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

            Exit Function
        End If

        m_lReturn = m_oBusiness.Initialise(sUserName:=m_sUserName, sPassword:=m_sPassword, iUserID:=0, iSourceID:=m_iSourceid, iLanguageID:=m_iLanguageID, iCurrencyID:=1, iLogLevel:=0, sCallingAppName:=ACApp)


        ' Check for errors.
        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            'UPGRADE_WARNING: Couldn't resolve default property of object GetBusiness. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            GetBusiness = gPMConstants.PMEReturnCode.PMFalse

            SharedFiles.bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

            Exit Function
        End If

    End Function

    Private Function CreateRegistrySettings(ByVal v_sDataModelCode As String) As Integer
        Try

            Dim sDefSubKey As String
            Dim sSubKey As String
            Dim sValue As String = String.Empty
            Dim sNBSubKey As String
            Dim sNBDefSubKey As String

            CreateRegistrySettings = gPMConstants.PMEReturnCode.PMTrue


            sDefSubKey = SharedFiles.GISSharedConstants.ACOIMGISSubKey & "\" & "DEF"
            sSubKey = SharedFiles.GISSharedConstants.ACOIMGISSubKey & "\" & v_sDataModelCode

            sNBDefSubKey = SharedFiles.GISSharedConstants.ACOIMGISSubKey & "\NB\" & "DEF"
            sNBSubKey = SharedFiles.GISSharedConstants.ACOIMGISSubKey & "\NB\" & v_sDataModelCode

            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="BOMRequired", r_sSettingValue:=sValue, v_sSubKey:=sSubKey)
            If sValue = "" Then
                m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="BOMRequired", r_sSettingValue:=sValue, v_sSubKey:=sDefSubKey)
                m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="BOMRequired", v_sSettingValue:=sValue, v_sSubKey:=sSubKey)
            End If

            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="DataSetsPath", r_sSettingValue:=sValue, v_sSubKey:=sSubKey)
            If sValue = "" Then
                m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="DataSetsPath", r_sSettingValue:=sValue, v_sSubKey:=sDefSubKey)
                m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="DataSetsPath", v_sSettingValue:=sValue, v_sSubKey:=sSubKey)
            End If

            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="Insurers", r_sSettingValue:=sValue, v_sSubKey:=sSubKey)
            If sValue = "" Then
                m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="Insurers", r_sSettingValue:=sValue, v_sSubKey:=sDefSubKey)
                m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="Insurers", v_sSettingValue:=sValue, v_sSubKey:=sSubKey)
            End If

            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="LookupsPath", r_sSettingValue:=sValue, v_sSubKey:=sSubKey)
            If sValue = "" Then
                m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="LookupsPath", r_sSettingValue:=sValue, v_sSubKey:=sDefSubKey)
                m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="LookupsPath", v_sSettingValue:=sValue, v_sSubKey:=sSubKey)
            End If

            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="QEMMethodsVersionNum", r_sSettingValue:=sValue, v_sSubKey:=sSubKey)
            If sValue = "" Then
                m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="QEMMethodsVersionNum", r_sSettingValue:=sValue, v_sSubKey:=sDefSubKey)
                m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="QEMMethodsVersionNum", v_sSettingValue:=sValue, v_sSubKey:=sSubKey)
            End If

            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="RulePath", r_sSettingValue:=sValue, v_sSubKey:=sSubKey)
            If sValue = "" Then
                m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="RulePath", r_sSettingValue:=sValue, v_sSubKey:=sDefSubKey)
                m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="RulePath", v_sSettingValue:=sValue, v_sSubKey:=sSubKey)
                m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="RulePath", v_sSettingValue:=sValue, v_sSubKey:=sNBSubKey)
            End If

            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="SaveOnQuote", r_sSettingValue:=sValue, v_sSubKey:=sSubKey & "\NB")
            If sValue = "" Then
                m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="SaveOnQuote", r_sSettingValue:=sValue, v_sSubKey:=sDefSubKey & "\NB")
                m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="SaveOnQuote", v_sSettingValue:=sValue, v_sSubKey:=sSubKey & "\NB")
            End If

            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="ServerListFileCompressed", r_sSettingValue:=sValue, v_sSubKey:=sSubKey & "\ListManagement")
            If sValue = "" Then
                m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="ServerListFileCompressed", r_sSettingValue:=sValue, v_sSubKey:=sDefSubKey & "\ListManagement")
                m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="ServerListFileCompressed", v_sSettingValue:=sValue, v_sSubKey:=sSubKey & "\ListManagement")
            End If

            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="ServerListFilePath", r_sSettingValue:=sValue, v_sSubKey:=sSubKey & "\ListManagement")
            If sValue = "" Then
                m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="ServerListFilePath", r_sSettingValue:=sValue, v_sSubKey:=sDefSubKey & "\ListManagement")
                m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="ServerListFilePath", v_sSettingValue:=sValue, v_sSubKey:=sSubKey & "\ListManagement")
            End If

            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="ServerListPrefVersion", r_sSettingValue:=sValue, v_sSubKey:=sSubKey & "\ListManagement")
            If sValue = "" Then
                m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="ServerListPrefVersion", r_sSettingValue:=sValue, v_sSubKey:=sDefSubKey & "\ListManagement")
                m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="ServerListPrefVersion", v_sSettingValue:=sValue, v_sSubKey:=sSubKey & "\ListManagement")
            End If

            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="ServerListVersion", r_sSettingValue:=sValue, v_sSubKey:=sSubKey & "\ListManagement")
            If sValue = "" Then
                m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="ServerListVersion", r_sSettingValue:=sValue, v_sSubKey:=sDefSubKey & "\ListManagement")
                m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="ServerListVersion", v_sSettingValue:=sValue, v_sSubKey:=sSubKey & "\ListManagement")
            End If

            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="DictPath", r_sSettingValue:=sValue, v_sSubKey:=sNBSubKey)
            If sValue = "" Then
                m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="DictPath", r_sSettingValue:=sValue, v_sSubKey:=sNBDefSubKey)
                m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="DictPath", v_sSettingValue:=sValue, v_sSubKey:=sNBDefSubKey)
            End If

            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="SchemePath", r_sSettingValue:=sValue, v_sSubKey:=sNBSubKey)
            If sValue = "" Then
                m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="SchemePath", r_sSettingValue:=sValue, v_sSubKey:=sNBDefSubKey)
                m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="SchemePath", v_sSettingValue:=sValue, v_sSubKey:=sNBDefSubKey)
            End If


        Catch ex As Exception
            CreateRegistrySettings = gPMConstants.PMEReturnCode.PMError
            SharedFiles.bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateRegistrySettings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateRegistrySettings", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)
        End Try
    End Function

    Public Overloads Function CreateDataSet(ByVal v_sGISDataModel As String) As Integer
        Return CreateDataSet(v_sGISDataModel, False)
    End Function

    Public Overloads Function CreateDataSet(ByVal v_sGISDataModel As String, ByRef bRecreateDataBackBone As Boolean) As Integer
        Dim oGIS As Object
        Try

            CreateDataSet = gPMConstants.PMEReturnCode.PMTrue
            'Then we call the GIS to recreate it...

            oGIS = New bGIS.Application

            m_lReturn = oGIS.Initialise(sUserName:=m_sUserName, sPassword:=m_sPassword, iUserID:=0, iSourceID:=1, iLanguageID:=1, iCurrencyID:=1, iLogLevel:=0, sCallingAppName:=ACApp)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                CreateDataSet = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                SharedFiles.bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the GIS interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDataSet")

                oGIS = Nothing
                Exit Function
            End If

            m_lReturn = oGIS.RecreateDatasets(v_sGISDataModel, bRecreateDataBackBone)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                CreateDataSet = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                SharedFiles.bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create the new data set", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDataSet")
                oGIS.Dispose()
                oGIS = Nothing

                Exit Function
            End If

            oGIS.Dispose()
            oGIS = Nothing

        Catch ex As Exception
            CreateDataSet = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            SharedFiles.bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateDataSet Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDataSet", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)
        End Try
    End Function

    Private Function DeleteBusiness() As Integer
        Try

            DeleteBusiness = gPMConstants.PMEReturnCode.PMTrue

            m_oBusiness.Dispose()
            m_oBusiness = Nothing


        Catch ex As Exception
            DeleteBusiness = gPMConstants.PMEReturnCode.PMError
            SharedFiles.bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteBusiness Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteBusiness", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)
            Exit Function
        End Try
    End Function

    Private Function GetDataModelsA(ByRef r_vDataModels As Object) As Integer

        GetDataModelsA = gPMConstants.PMEReturnCode.PMTrue

        Dim oBusiness As Object

        oBusiness = New bPMUList.bPMUListCreate


        m_lReturn = oBusiness.Initialise(sUserName:=m_sUserName, sPassword:=m_sPassword, iUserID:=0, iSourceID:=1, iLanguageID:=1, iCurrencyID:=1, iLogLevel:=0, sCallingAppName:=ACApp)
        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GetDataModelsA = gPMConstants.PMEReturnCode.PMFalse
            SharedFiles.bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the list object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDataModels")
            Exit Function
        End If

        m_lReturn = m_oBusiness.GetDataModels(r_vDataModels)
        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GetDataModelsA = gPMConstants.PMEReturnCode.PMFalse
            SharedFiles.bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to call GetDataModels method", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDataModels")
            Exit Function
        End If

        m_oBusiness.Dispose()

    End Function

    Private Function GetDataModels(ByRef r_vDataModels(,) As Object) As Integer

        Dim oDatabase As dPMDAO.Database
        Dim sSQL As String



        GetDataModels = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = gPMComponentServices.NewDatabase(v_sUsername:=m_sUserName, v_iSourceID:=m_iSourceid, v_iLanguageID:=m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_oDatabase:=oDatabase)

        sSQL = ""
        sSQL = sSQL & "SELECT" & vbCrLf
        sSQL = sSQL & "    gdm.gis_data_model_id," & vbCrLf
        sSQL = sSQL & "    gdm.description," & vbCrLf
        sSQL = sSQL & "    gdm.code" & vbCrLf
        sSQL = sSQL & "FROM GIS_data_model gdm" & vbCrLf
        sSQL = sSQL & "WHERE gdm.is_deleted = 0" & vbCrLf
        'sSQL = sSQL & "AND gdm.effective_date <= '" & VB6.Format(Now, "yyyy-mm-dd hh:nn:ss") & "'" & vbCrLf
        sSQL = sSQL & "AND gdm.effective_date <= '" & Format(DateTime.Today, "yyyy-MM-dd hh:mm:ss") & "'" & vbCrLf
        sSQL = sSQL & "AND EXISTS" & vbCrLf
        sSQL = sSQL & "    (" & vbCrLf
        sSQL = sSQL & "        SELECT" & vbCrLf
        sSQL = sSQL & "            NULL" & vbCrLf
        sSQL = sSQL & "        FROM sysobjects so" & vbCrLf
        sSQL = sSQL & "        WHERE so.name = RTRIM(gdm.code) + '_policy_binder'" & vbCrLf
        sSQL = sSQL & "        AND so.xtype = 'U'" & vbCrLf
        sSQL = sSQL & "        AND gdm.code NOT LIKE 'GII%'" & vbCrLf
        sSQL = sSQL & "    )"


        ' Execute SQL Statement
        m_lReturn = oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetDataModelsForRebuild", bStoredProcedure:=False, vResultArray:=r_vDataModels)

        oDatabase = Nothing

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GetDataModels = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

    End Function

    Public Sub RegenerateAllModels()
        Try

            Dim rc As Integer
            Dim vDataModels As Object = Nothing
            Dim lModels As Integer
            Dim lDMId As Integer
            Dim sDMCode As String
            Dim sDMName As String
            Dim recreateDataBackBone As Boolean = False

            g_sUserName = m_sUserName

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            m_lReturn = GetBusiness()

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = GetDataModels(vDataModels)

                ' if we got some datamodels to rebuild then loop through them...
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                    If IsArray(vDataModels) Then

                        lModels = UBound(vDataModels, 2)

                        pBar.Minimum = 0

                        If Trim(UCase(m_sDataModelCode)) = "ALL" Then
                            If lModels = 0 Then
                                pBar.Maximum = 1
                            Else
                                pBar.Maximum = lModels
                            End If
                        Else
                            pBar.Maximum = 1
                        End If

                        'Get system option KCMDocProduction
                        Dim ccmDocProductionOption As String = String.Empty

                        m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=g_sUserName, v_sPassword:=m_sPassword, v_iUserID:=0, v_iMainSourceID:=m_iSourceid, v_iLanguageID:=m_iLanguageID,
                                                        v_iCurrencyID:=1, v_iLogLevel:=0, v_sCallingAppName:=ACApp, v_iOptionNumber:=GeneralConst.kSystemOptionDocumentProductionSystem,
                                                        r_sOptionValue:=ccmDocProductionOption)
                        If ccmDocProductionOption = "1" OrElse ccmDocProductionOption = "2" Then
                            recreateDataBackBone = True
                        End If

                        For lLoop As Integer = 0 To lModels
                            pBar.Value = lLoop

                            lDMId = vDataModels(0, lLoop)
                            sDMName = Trim(vDataModels(1, lLoop))
                            sDMCode = Trim(vDataModels(2, lLoop))

                            ' check for specific dm code or ALL
                            If (Trim(UCase(m_sDataModelCode)) = Trim(UCase(sDMCode)) Or Trim(UCase(m_sDataModelCode)) = "ALL") Then
                                lblDataModelName.Text = "Rebuilding: " & sDMName & " (" & (lLoop + 1) & "/" & (lModels + 1) & ")"
                                System.Windows.Forms.Application.DoEvents()

                                ' rebuild datamodel
                                m_lReturn = RebuildDatamodel(lDMId, sDMCode, recreateDataBackBone)
                                'set the bRecreateDataBackbone =false after first iteration. But it could be possible that the Backbone is not created after First iteration.

                            End If
                        Next lLoop

                    End If

                End If
                m_lReturn = DeleteBusiness()
            End If
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default

        Catch ex As Exception

            ' Log Error.
            SharedFiles.bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed in RegenerateAllModels", vApp:=ACApp, vClass:=ACClass, vMethod:="RegenerateAllModels", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            m_lReturn = DeleteBusiness()

        End Try
    End Sub
    ''' <summary>
    ''' CreateAndSavePartyHistorySchema
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CreateAndSavePartyHistorySchema()
        Const kMethodName As String = "CreateAndSavePartyHistorySchema"
        Dim nReturn As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim oDatabase As dPMDAO.Database

        Try
            nReturn = gPMComponentServices.NewDatabase(v_sUsername:=m_sUserName, v_iSourceID:=m_iSourceid, v_iLanguageID:=m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_oDatabase:=oDatabase)
            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Create DataBase Object", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName)
                Exit Sub
            End If
            oDatabase.Parameters.Clear()
            nReturn = PartyFunc.CreateAndSavePartyHistorySchema(oDatabase)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Create PartyHistorySchema", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName)
            End If
        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Create PartyHistorySchema", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName)
        Finally
            oDatabase = Nothing
        End Try
    End Sub
End Class
