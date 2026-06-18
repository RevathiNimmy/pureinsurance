Option Strict Off
Option Explicit On
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.ServiceModel
Imports System.Text
Imports System.Xml
Imports SSP.Shared
Imports SSP.Shared.gPMConstants

Public NotInheritable Class Business
    Implements IDisposable

    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    Private m_lProcessMode As Integer
    Private m_dtEffectiveDate As Date
    Private Const ACClass As String = "Business"

    Private m_oDatabase As dPMDAO.Database

    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode
    Const kMaxReceivedMessageSize = 2147483647


    Public ACCCMBackboneSuffix As String = "PureBackbone_" + DateTime.Now.ToString("ddMMyyyy_HHmmss")
    Public Const ACCCMBackboneFileExtension As String = ".RTF"

    Public Const ACCCMBackboneXMLSuffix As String = "PureInputXMLAnalysisMode"
    Public Const ACCCMBackboneXMLFileExtension As String = ".XML"
    Const kxmlns As String = "urn:backbone:Data_Backbone"
    Public Const kCCMHeaderData As String = "<PureDataBackbone xmlns=""urn:backbone:Data_Backbone"" xmlns:p1=""urn:itp:backbone"" p1:databackbone-revision=""databackbone-revision1"" p1:version=""version1"" p1:format=""1.0""><ConfigFields><Mode>Analysis</Mode></ConfigFields><PolicyStandardWordings/></PureDataBackbone>"

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public Enum CCMStatusDesc
        Published = 1
        Current = 2
        Accepted = 3
    End Enum


#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

    ''' <summary>
    ''' Entry point for any initialisation code for this object
    ''' </summary>
    ''' <param name="sUsername"></param>
    ''' <param name="sPassword"></param>
    ''' <param name="iUserID"></param>
    ''' <param name="iSourceID"></param>
    ''' <param name="iLanguageID"></param>
    ''' <param name="iCurrencyID"></param>
    ''' <param name="iLogLevel"></param>
    ''' <param name="sCallingAppName"></param>
    ''' <param name="bStandAlone"></param>
    ''' <param name="vDatabase"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        Dim nResult As Integer = PMEReturnCode.PMTrue

        Try
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            ' Set Username and Password			
            m_lProcessMode = PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

        Catch excep As System.Exception

            nResult = PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try

        Return nResult
    End Function

    ''' <summary>
    ''' Get CCM lookup details for CCM ddl
    ''' </summary>
    ''' <param name="sCCMCustomer"></param>
    ''' <param name="sCCMPartner"></param>
    ''' <param name="sContractTypeName"></param>
    ''' <param name="sRepositoryProject"></param>
    ''' <param name="sContractTypeVersion"></param>
    ''' <param name="sRepStatus"></param>
    ''' <param name="asListofCCMTemplates"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCCMLookupDetails(ByVal sCCMCustomer As String, ByVal sCCMPartner As String, ByVal sContractTypeName As String,
                                        ByVal sRepositoryProject As String, ByVal sContractTypeVersion As String, ByVal sRepStatus As String,
                                        ByVal sCCMWebServiceURL As String, ByRef asListofCCMTemplates As String()) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim sJobid As String = Guid.NewGuid().ToString().Replace("-", "")
        Dim abDocTemplateList() As Byte = Nothing
        Dim sXMLDocTemplateList As String = ""
        Dim XMLDoc As XmlDocument = New XmlDocument
        Dim lXmlElementList As List(Of String) = New List(Of String)
        Dim azureWebService As CCM_Azure.TPInTheCloudClient

        Try
            azureWebService = New CCM_Azure.TPInTheCloudClient()
            SetServiceAddressAndBinding(sCCMWebServiceURL, azureWebService)

            Dim oGetDocTemplatesRequestInfoAzure As New CCM_Azure.requestinfo()

            Try
                oGetDocTemplatesRequestInfoAzure = azureWebService.DesignerListDocumentTemplatesV1(sCCMPartner, sCCMCustomer, sContractTypeName, sContractTypeVersion,
                                                                                                  sJobid, sRepositoryProject, sRepStatus, abDocTemplateList)
                sXMLDocTemplateList = System.Text.Encoding.UTF8.GetString(abDocTemplateList)

            Catch excep As Exception
                nResult = PMEReturnCode.PMError
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="CCM web service Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCCMLookupDetails", excep:=excep)

                Return nResult
            End Try

            If Not String.IsNullOrEmpty(sXMLDocTemplateList) Then
                XMLDoc.LoadXml(sXMLDocTemplateList)
                Dim elemList As XmlNodeList = XMLDoc.GetElementsByTagName("d1p1:documenttemplate")
                For i As Integer = 0 To elemList.Count - 1
                    For Each Attribute As XmlAttribute In elemList(i).Attributes
                        If Attribute.Name = "d1p1:name" Then
                            lXmlElementList.Add(Attribute.Value)
                        End If
                    Next
                Next
            End If

            If Not lXmlElementList Is Nothing Then
                asListofCCMTemplates = lXmlElementList.ToArray()
            End If
        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="GetCCMLookupDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCCMLookupDetails", excep:=excep)
        Finally
            azureWebService = Nothing
        End Try

        Return nResult
    End Function

    ''' <summary>
    '''  Build backbone file if CCM is enabled on data model rebuild
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CCMRecreateDataSets(ByVal sGISDataModelCode As String, ByRef bRecreateDataBackBone As Boolean,
                                        Optional ByVal bIncludeCoreFieldset As Boolean = True) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim sCCMBackboneFileName As String = String.Empty
        Dim sCCMBackboneXMLFileName As String = String.Empty
        Dim sCCMBackboneFilePath As String = String.Empty
        Dim oFieldArray As ArrayList = New ArrayList()
        Dim oCCMFieldSetArray As ArrayList = New ArrayList()

        Try
            Dim arrayOfDMs As String = sGISDataModelCode
            Dim arrayOfSelectedDataModels As String() = Nothing
            arrayOfSelectedDataModels = sGISDataModelCode.Split(New Char() {","c})

            For lTemp As Integer = arrayOfSelectedDataModels.GetLowerBound(0) To arrayOfSelectedDataModels.GetUpperBound(0)
                sGISDataModelCode = arrayOfSelectedDataModels(lTemp).ToString()
                If Not sGISDataModelCode.Contains("S4I") AndAlso Not sGISDataModelCode.Contains("GII") AndAlso
                    Not sGISDataModelCode.Contains("Test") Then ''DataModel like %S4I%, %GII%, Test not included for CCM

                    If bRecreateDataBackBone Then
                        nResult = CType(GISSharedConstants.GetCCMBackboneFileName(sCCMBackboneFilePath:=sCCMBackboneFilePath), PMEReturnCode)
                        If nResult <> PMEReturnCode.PMTrue Then
                            Return PMEReturnCode.PMFalse
                        End If

                        If (Not IO.Directory.Exists(sCCMBackboneFilePath)) Then
                            IO.Directory.CreateDirectory(sCCMBackboneFilePath)
                        End If

                        sCCMBackboneFileName = sCCMBackboneFilePath & ACCCMBackboneSuffix & ACCCMBackboneFileExtension
                        If File.Exists(sCCMBackboneFileName) Then
                            File.Delete(sCCMBackboneFileName)
                        End If

                        sCCMBackboneXMLFileName = sCCMBackboneFilePath & ACCCMBackboneXMLSuffix & ACCCMBackboneXMLFileExtension
                        If File.Exists(sCCMBackboneXMLFileName) Then
                            File.Delete(sCCMBackboneXMLFileName)
                        End If

                    End If

                    ''oFieldArray: content for databackbone
                    ''oCCMFieldSetArray: fieldsets to update in CCM
                    nResult = GetFieldSetandFieldArray(oFieldArray, oCCMFieldSetArray, arrayOfDMs, bIncludeCoreFieldset)
                    If nResult <> PMEReturnCode.PMTrue Then
                        Return PMEReturnCode.PMFalse
                    End If

                    If bRecreateDataBackBone Then
                        'create the backbone file once
                        BuildDatabackboneStructure(sCCMBackboneFileName, oFieldArray)
                        BuildInputXMLForAnalysisMode(sCCMBackboneXMLFileName)
                    End If

                    'update/populate the fieldsets for each DataModel
                    nResult = UpdateCCMFieldsets(sGISDataModelCode, oCCMFieldSetArray, bRecreateDataBackBone)
                    If nResult <> PMEReturnCode.PMTrue Then
                        Return PMEReturnCode.PMFalse
                    End If
                    bRecreateDataBackBone = False
                End If
            Next
        Catch excep As Exception
            nResult = PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="CCMRecreateDataSets Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CCMRecreateDataSets", excep:=excep)
        End Try
        Return nResult
    End Function

    ''' <summary>
    ''' Build CCM backbone file
    ''' </summary>
    ''' <param name="sCCMBackboneFilename"></param>
    ''' <param name="oFieldArray"></param>
    ''' <remarks></remarks>
    Private Sub BuildDatabackboneStructure(ByVal sCCMBackboneFilename As String, ByVal oFieldArray As ArrayList)
        Dim sw As StreamWriter = File.CreateText(sCCMBackboneFilename)
        For Each sField As String In oFieldArray
            sw.WriteLine(sField)
        Next
        sw.Close()
    End Sub

    ''' <summary>
    ''' Build input XML to get fieldsets for the Document Template from CCM 
    ''' </summary>
    ''' <param name="sCCMBackboneXMLFilename"></param>
    ''' <remarks></remarks>
    Private Sub BuildInputXMLForAnalysisMode(ByVal sCCMBackboneXMLFilename As String)
        Dim sw As StreamWriter = File.CreateText(sCCMBackboneXMLFilename)
        Dim sInputStr As String = String.Empty
        CreateInputXMLInAnalysisMode(sInputStr)
        sw.WriteLine(sInputStr)
        sw.Close()
    End Sub

    ''' <summary>
    ''' Write all datastructure and fieldsets to arraylist
    ''' </summary>
    ''' <param name="oFieldArray"></param>
    ''' <param name="oCCMFieldSets"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetFieldSetandFieldArray(ByRef oFieldArray As ArrayList, ByRef oCCMFieldSets As ArrayList,
                                              ByVal sDataModelCodes As String, ByVal bIncludeCoreFieldset As Boolean) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim oCoreArray(,) As Object = Nothing
        Dim oDataModelArray(,) As Object = Nothing
        Dim oSubGrpArray As Object(,) = Nothing
        Dim bDSDataModelsStartTagAdded As Boolean = False
        Dim bDSDataModelsEndTagAdded As Boolean = False
        Dim dtCoreWpFields As DataTable = Nothing

        Dim oLoopArray(,) As Object = Nothing
        Dim oLoop1Array(,) As Object = Nothing
        Dim oLoop2Array(,) As Object = Nothing
        Dim oLoop3Array(,) As Object = Nothing

        oFieldArray.Add("DATABACKBONE PureDataBackbone")
        oFieldArray.Add("BEGIN")
        oFieldArray.Add("FIELDSET ConfigFields")

        ''Add CORE FIELDS in databackbone
        ''Get core fields from DB
        ''loop1, loop2 ,loop3 is NULL
        If bIncludeCoreFieldset Then
            nResult = GetCoreFieldsForDataBackbone(oLoopArray)
            If nResult <> PMEReturnCode.PMTrue Then
                Throw New ApplicationException("GetCoreFieldsForDataBackbone Failed")
            End If
        End If

        If Not oLoopArray Is Nothing Then
            For nTemp As Integer = oLoopArray.GetLowerBound(1) To oLoopArray.GetUpperBound(1)
                If ToSafeString(oLoopArray(0, nTemp)).Replace("-", "").Replace(",", "").Replace("/", "").Replace(" ", "") = "CorePolicyEvent" Then
                    oFieldArray.Add("FIELDSET " & ToSafeString(oLoopArray(0, nTemp)).Replace("-", "").Replace(",", "").Replace("/", "").Replace(" ", "") & "Desc")
                    oCCMFieldSets.Add("CORE~" & ToSafeString(oLoopArray(0, nTemp)) & "Desc")

                    nResult = HandleSpecialCaseForCore(ToSafeString(oLoopArray(0, nTemp)).Replace("-", "").Replace(",", "").Replace("/", "").Replace(" ", ""))
                    If nResult <> PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "HandleSpecialCaseForCore Core fields Failed")
                        nResult = PMEReturnCode.PMTrue ''proceed further to update ccm fields
                    End If
                ElseIf ToSafeString(oLoopArray(0, nTemp)).Replace("-", "").Replace(",", "").Replace("/", "").Replace(" ", "") = "CorePolicySection" OrElse
                    ToSafeString(oLoopArray(0, nTemp)).Replace("-", "").Replace(",", "").Replace("/", "").Replace(" ", "") = "CorePolicyTax" OrElse
                    ToSafeString(oLoopArray(0, nTemp)).Replace("-", "").Replace(",", "").Replace("/", "").Replace(" ", "") = "CoreReinsuranceFac" Then
                    oFieldArray.Add("FIELDSET " & ToSafeString(oLoopArray(0, nTemp)).Replace("-", "").Replace(",", "").Replace("/", "").Replace(" ", "") & "Additional")
                    oCCMFieldSets.Add("CORE~" & ToSafeString(oLoopArray(0, nTemp)) & "Additional")

                    nResult = HandleSpecialCaseForCore(ToSafeString(oLoopArray(0, nTemp)).Replace("-", "").Replace(",", "").Replace("/", "").Replace(" ", ""))
                    If nResult <> PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "HandleSpecialCaseForCore Core fields Failed")
                        nResult = PMEReturnCode.PMTrue ''proceed further to update ccm fields
                    End If
                Else
                    oFieldArray.Add("FIELDSET " & ToSafeString(oLoopArray(0, nTemp)).Replace("-", "").Replace(",", "").Replace("/", "").Replace(" ", ""))
                    oCCMFieldSets.Add("CORE~" & ToSafeString(oLoopArray(0, nTemp)))
                End If
            Next
        End If

        ''Tag for policy level standard wordings
        oFieldArray.Add("ARRAY FIELDSET PolicyStandardWordings")
        oCCMFieldSets.Add("ENDORSEMENT~PolicyStandardWordings")

        ''Get distinct loop1 values from DB
        nResult = GetDistinctLoop1Values(oLoop1Array)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetDistinctLoop1Values Failed")
        End If

        If oLoop1Array IsNot Nothing Then
            For nTemp1 As Integer = oLoop1Array.GetLowerBound(1) To oLoop1Array.GetUpperBound(1)
                oFieldArray.Add("ARRAY DATASTRUCTURE DS_Core" & oLoop1Array(0, nTemp1).Replace("-", "").Replace(",", "").Replace("/", "").Replace(" ", ""))
                oFieldArray.Add("BEGIN")
                oFieldArray.Add("FIELDSET Core" & oLoop1Array(0, nTemp1).Replace("-", "").Replace(",", "").Replace("/", "").Replace(" ", ""))
                oCCMFieldSets.Add("CORE~Core" & oLoop1Array(0, nTemp1))

                ''Get distinct loop2 values from DB
                nResult = GetDistinctLoop2Values(oLoop1Array(0, nTemp1), oLoop2Array)
                If nResult <> PMEReturnCode.PMTrue Then
                    Throw New ApplicationException("GetDistinctLoop2Values Failed")
                End If

                If oLoop2Array IsNot Nothing Then
                    For nTemp2 As Integer = oLoop2Array.GetLowerBound(1) To oLoop2Array.GetUpperBound(1)
                        If Not String.IsNullOrEmpty(oLoop2Array(0, nTemp2)) Then ''loop1, loop2 is not NULL
                            oFieldArray.Add("ARRAY DATASTRUCTURE DS_" & oLoop2Array(1, nTemp2).Replace("-", "").Replace(",", "").Replace("/", "").Replace(" ", ""))
                            oFieldArray.Add("BEGIN")
                            oFieldArray.Add("FIELDSET " & oLoop2Array(1, nTemp2).Replace("-", "").Replace(",", "").Replace("/", "").Replace(" ", ""))
                            oCCMFieldSets.Add("CORE~" & oLoop2Array(1, nTemp2))

                            nResult = GetDistinctLoop3Values(oLoop1Array(0, nTemp1), oLoop2Array(0, nTemp2), oLoop3Array)
                            If nResult <> PMEReturnCode.PMTrue Then
                                Throw New ApplicationException("GetDistinctLoop3Values Failed")
                            End If

                            If oLoop3Array IsNot Nothing Then
                                For nTemp3 As Integer = oLoop3Array.GetLowerBound(1) To oLoop3Array.GetUpperBound(1)
                                    If Not String.IsNullOrEmpty(oLoop3Array(0, nTemp3)) Then ''loop1, loop2, loop3 is not NULL
                                        oFieldArray.Add("ARRAY DATASTRUCTURE DS_" & oLoop3Array(1, nTemp3).Replace("-", "").Replace(",", "").Replace("/", "").Replace(" ", ""))
                                        oFieldArray.Add("BEGIN")
                                        oFieldArray.Add("FIELDSET " & oLoop3Array(1, nTemp3).Replace("-", "").Replace(",", "").Replace("/", "").Replace(" ", ""))
                                        oCCMFieldSets.Add("CORE~" & oLoop3Array(1, nTemp3))

                                        oFieldArray.Add("END") ''end loop3
                                    End If
                                Next
                            End If
                            oFieldArray.Add("END") ''end loop2
                        End If
                    Next
                End If
                oFieldArray.Add("END") ''end loop1
                oFieldArray.Add("")
            Next
        End If

        ''Add NON-CORE FIELDS in databackbone
        ''Get all valid data models for CCM
        nResult = GetDataModelCode(oDataModelCodeArray:=oDataModelArray)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetDataModelCode Failed")
        End If

        Dim selectedDataModels As String() = Nothing
        selectedDataModels = sDataModelCodes.Split(New Char() {","c})

        For lTemp1 As Integer = oDataModelArray.GetLowerBound(1) To oDataModelArray.GetUpperBound(1)
            For lTempDM As Integer = selectedDataModels.GetLowerBound(0) To selectedDataModels.GetUpperBound(0)
                If oDataModelArray(0, lTemp1) = selectedDataModels(lTempDM) Then
                    Dim bDataModelAdded As Boolean = False

                    If oDataModelArray(2, lTemp1) <> 4 AndAlso Not bDSDataModelsStartTagAdded Then ''Add only once after Party Data Models
                        oFieldArray.Add("DATASTRUCTURE DS_DataModels")
                        oFieldArray.Add("BEGIN")
                        oFieldArray.Add("")
                        bDSDataModelsStartTagAdded = True
                    End If

                    ''Get sub_groups for particular datamodel
                    nResult = GetSubGroupDetails(oDataModelArray(0, lTemp1), aoResultArray:=oSubGrpArray)
                    If nResult <> PMEReturnCode.PMTrue Then
                        Throw New ApplicationException("GetSubGroupDetails Failed")
                    End If

                    If Informations.IsArray(oSubGrpArray) Then
                        ''Datastructure DataModel
                        oFieldArray.Add("ARRAY DATASTRUCTURE DS_" & oDataModelArray(0, lTemp1))
                        oFieldArray.Add("BEGIN")
                        bDataModelAdded = True ''to be added only once for each datamodel
                    End If

                    For lTemp2 As Integer = oSubGrpArray.GetLowerBound(1) To oSubGrpArray.GetUpperBound(1)
                        Dim dtWpFields As DataTable = Nothing

                        ''Update datastructure name like _Parent_ChildObj
                        nResult = UpdateDataStructureName(oDataModelArray(0, lTemp1), oSubGrpArray(0, lTemp2))
                        If nResult <> PMEReturnCode.PMTrue Then
                            Throw New ApplicationException("UpdateDataStructureName Failed")
                        End If

                        ''Get main_group, sub_group, loop1, loop2, loop3, table_name and datastructure_name
                        nResult = GetDataStructureDetails(oDataModelArray(0, lTemp1), oSubGrpArray(0, lTemp2), dtWpFields)
                        If nResult <> PMEReturnCode.PMTrue Then
                            Throw New ApplicationException("GetDataStructureDetails Failed")
                        End If

                        Dim nLevel As Integer = 0
                        Dim sLoop1 As String
                        Dim sLoop2 As String
                        Dim sLoop3 As String
                        Dim sNextRowLoop1 As String = String.Empty
                        Dim sNextRowLoop2 As String = String.Empty
                        Dim sNextRowLoop3 As String = String.Empty
                        Dim sTableName As String = String.Empty
                        Dim sDataStructureName As String = String.Empty
                        Dim bDataStructure As Boolean = False
                        Dim oSWColumnName(,) As Object = Nothing
                        Dim bHasEndorsement As Boolean = False

                        ''check if table contains standard wordings or not
                        If dtWpFields.Rows.Count > 0 Then
                            For i As Integer = 0 To dtWpFields.Rows.Count - 1
                                If ToSafeBoolean(dtWpFields.Rows(i)("HasEndorsement")) Then
                                    bHasEndorsement = True
                                Else
                                    bHasEndorsement = False
                                End If
                            Next
                        End If

                        If dtWpFields.Rows.Count > 1 Then
                            Dim nCount As Integer = 0
                            Dim odata As DataRow = Nothing
                            For Each oDataRow As DataRow In dtWpFields.Rows
                                sLoop1 = ToSafeString(oDataRow("loop1"))
                                sLoop2 = ToSafeString(oDataRow("loop2"))
                                sLoop3 = ToSafeString(oDataRow("loop3"))

                                sTableName = ToSafeString(oDataRow("Table_Name"))
                                sDataStructureName = ToSafeString(oDataRow("DataStructure_Name"))
                                If nCount <= dtWpFields.Rows.Count - 1 Then
                                    If nCount = dtWpFields.Rows.Count - 1 Then
                                        oDataRow("loop1") = DBNull.Value
                                        oDataRow("loop2") = DBNull.Value
                                    Else
                                        odata = dtWpFields.Rows(nCount + 1)
                                    End If
                                End If
                                sNextRowLoop1 = ToSafeString(odata("loop1"))
                                sNextRowLoop2 = ToSafeString(odata("loop2"))
                                sNextRowLoop3 = ToSafeString(odata("loop3"))

                                If String.IsNullOrEmpty(sLoop1) AndAlso String.IsNullOrEmpty(sLoop2) AndAlso String.IsNullOrEmpty(sLoop3) Then
                                    If Not String.IsNullOrEmpty(sTableName) Then
                                        oFieldArray.Add("DATASTRUCTURE DS_" & sDataStructureName)
                                        oFieldArray.Add("BEGIN")
                                        oFieldArray.Add("FIELDSET " & sDataStructureName)
                                        If bHasEndorsement Then
                                            ''check if endorsement for current table name
                                            ''add array fieldset for endoresement
                                            nResult = GetSWColumnName(sTableName, oDataModelArray(0, lTemp1), oSWColumnName)
                                            If nResult <> PMEReturnCode.PMTrue Then
                                                Throw New ApplicationException("GetSWColumnName Failed")
                                            End If

                                            If Not oSWColumnName Is Nothing Then
                                                For nCtr As Integer = oSWColumnName.GetLowerBound(1) To oSWColumnName.GetUpperBound(1)
                                                    oFieldArray.Add("ARRAY FIELDSET " & sDataStructureName & "_" & oSWColumnName(0, nCtr))
                                                    oCCMFieldSets.Add("ENDORSEMENT~" & sDataStructureName & "_" & oSWColumnName(0, nCtr))
                                                    nResult = UpdateEndorsementDataStructureName(oDataModelArray(0, lTemp1), sTableName, sDataStructureName, oSWColumnName(0, nCtr))
                                                    If nResult <> PMEReturnCode.PMTrue Then
                                                        Throw New ApplicationException("UpdateEndorsementDataStructureName Failed")
                                                    End If
                                                Next
                                            End If
                                        End If
                                        oCCMFieldSets.Add(oDataModelArray(0, lTemp1) & "~" & sTableName & "~" & sDataStructureName)
                                        nLevel = nLevel + 1
                                        bDataStructure = True
                                    End If
                                End If
                                If Not String.IsNullOrEmpty(sLoop1) AndAlso String.IsNullOrEmpty(sLoop2) AndAlso String.IsNullOrEmpty(sLoop3) Then
                                    If Not String.IsNullOrEmpty(sTableName) AndAlso Not String.IsNullOrEmpty(sDataStructureName) Then
                                        If Not bDataStructure Then
                                            oFieldArray.Add("DATASTRUCTURE DS_" & sDataStructureName)
                                            oFieldArray.Add("BEGIN")
                                            oFieldArray.Add("FIELDSET " & sDataStructureName)
                                            If bHasEndorsement Then
                                                ''check if endorsement for current table name
                                                ''add array fieldset for endoresement
                                                nResult = GetSWColumnName(sTableName, oDataModelArray(0, lTemp1), oSWColumnName)
                                                If nResult <> PMEReturnCode.PMTrue Then
                                                    Throw New ApplicationException("GetSWColumnName Failed")
                                                End If

                                                If Not oSWColumnName Is Nothing Then
                                                    For nCtr As Integer = oSWColumnName.GetLowerBound(1) To oSWColumnName.GetUpperBound(1)
                                                        oFieldArray.Add("ARRAY FIELDSET " & sDataStructureName & "_" & oSWColumnName(0, nCtr))
                                                        oCCMFieldSets.Add("ENDORSEMENT~" & sDataStructureName & "_" & oSWColumnName(0, nCtr))
                                                        nResult = UpdateEndorsementDataStructureName(oDataModelArray(0, lTemp1), sTableName, sDataStructureName, oSWColumnName(0, nCtr))
                                                        If nResult <> PMEReturnCode.PMTrue Then
                                                            Throw New ApplicationException("UpdateEndorsementDataStructureName Failed")
                                                        End If
                                                    Next
                                                End If
                                            End If
                                            oCCMFieldSets.Add(oDataModelArray(0, lTemp1) & "~" & sTableName & "~" & sDataStructureName)
                                            nLevel = nLevel + 1
                                        Else
                                            sNextRowLoop1 = ToSafeString(odata("loop1"))
                                            sNextRowLoop2 = ToSafeString(odata("loop2"))
                                            sNextRowLoop3 = ToSafeString(odata("loop3"))
                                            oFieldArray.Add("ARRAY DATASTRUCTURE DS_" & sDataStructureName)
                                            oFieldArray.Add("BEGIN")
                                            oFieldArray.Add("FIELDSET " & sDataStructureName)
                                            If bHasEndorsement Then
                                                ''check if endorsement for current table name
                                                ''add array fieldset for endoresement
                                                nResult = GetSWColumnName(sTableName, oDataModelArray(0, lTemp1), oSWColumnName)
                                                If nResult <> PMEReturnCode.PMTrue Then
                                                    Throw New ApplicationException("GetSWColumnName Failed")
                                                End If

                                                If Not oSWColumnName Is Nothing Then
                                                    For nCtr As Integer = oSWColumnName.GetLowerBound(1) To oSWColumnName.GetUpperBound(1)
                                                        oFieldArray.Add("ARRAY FIELDSET " & sDataStructureName & "_" & oSWColumnName(0, nCtr))
                                                        oCCMFieldSets.Add("ENDORSEMENT~" & sDataStructureName & "_" & oSWColumnName(0, nCtr))
                                                        nResult = UpdateEndorsementDataStructureName(oDataModelArray(0, lTemp1), sTableName, sDataStructureName, oSWColumnName(0, nCtr))
                                                        If nResult <> PMEReturnCode.PMTrue Then
                                                            Throw New ApplicationException("UpdateEndorsementDataStructureName Failed")
                                                        End If
                                                    Next
                                                End If
                                            End If
                                            oCCMFieldSets.Add(oDataModelArray(0, lTemp1) & "~" & sTableName & "~" & sDataStructureName)
                                            nLevel = nLevel + 1
                                            If sLoop1 = sNextRowLoop1 AndAlso sNextRowLoop2 <> "" Then
                                            Else
                                                oFieldArray.Add("END")
                                                nLevel = nLevel - 1
                                            End If
                                        End If

                                    End If
                                End If
                                If Not String.IsNullOrEmpty(sLoop1) AndAlso Not String.IsNullOrEmpty(sLoop2) AndAlso String.IsNullOrEmpty(sLoop3) Then
                                    If Not String.IsNullOrEmpty(sTableName) AndAlso Not String.IsNullOrEmpty(sDataStructureName) Then
                                        sNextRowLoop1 = ToSafeString(odata("loop1"))
                                        sNextRowLoop2 = ToSafeString(odata("loop2"))
                                        sNextRowLoop3 = ToSafeString(odata("loop3"))
                                        oFieldArray.Add("ARRAY DATASTRUCTURE DS_" & sDataStructureName)
                                        oFieldArray.Add("BEGIN")
                                        oFieldArray.Add("FIELDSET " & sDataStructureName)
                                        If bHasEndorsement Then
                                            ''check if endorsement for current table name
                                            ''add array fieldset for endoresement
                                            nResult = GetSWColumnName(sTableName, oDataModelArray(0, lTemp1), oSWColumnName)
                                            If nResult <> PMEReturnCode.PMTrue Then
                                                Throw New ApplicationException("GetSWColumnName Failed")
                                            End If

                                            If Not oSWColumnName Is Nothing Then
                                                For nCtr As Integer = oSWColumnName.GetLowerBound(1) To oSWColumnName.GetUpperBound(1)
                                                    oFieldArray.Add("ARRAY FIELDSET " & sDataStructureName & "_" & oSWColumnName(0, nCtr))
                                                    oCCMFieldSets.Add("ENDORSEMENT~" & sDataStructureName & "_" & oSWColumnName(0, nCtr))
                                                    nResult = UpdateEndorsementDataStructureName(oDataModelArray(0, lTemp1), sTableName, sDataStructureName, oSWColumnName(0, nCtr))
                                                    If nResult <> PMEReturnCode.PMTrue Then
                                                        Throw New ApplicationException("UpdateEndorsementDataStructureName Failed")
                                                    End If
                                                Next
                                            End If
                                        End If
                                        oCCMFieldSets.Add(oDataModelArray(0, lTemp1) & "~" & sTableName & "~" & sDataStructureName)
                                        nLevel = nLevel + 1
                                        If sLoop1 = sNextRowLoop1 AndAlso sLoop2 = sNextRowLoop2 AndAlso sNextRowLoop3 <> "" Then
                                        ElseIf sLoop1 <> sNextRowLoop1 Then
                                            oFieldArray.Add("END")
                                            oFieldArray.Add("END")
                                            nLevel = nLevel - 1
                                            nLevel = nLevel - 1
                                        Else
                                            oFieldArray.Add("END")
                                            nLevel = nLevel - 1
                                        End If
                                    End If
                                End If
                                If Not String.IsNullOrEmpty(sLoop1) AndAlso Not String.IsNullOrEmpty(sLoop2) AndAlso Not String.IsNullOrEmpty(sLoop3) Then
                                    If Not String.IsNullOrEmpty(sTableName) AndAlso Not String.IsNullOrEmpty(sDataStructureName) Then
                                        sNextRowLoop1 = ToSafeString(odata("loop1"))
                                        sNextRowLoop2 = ToSafeString(odata("loop2"))
                                        sNextRowLoop3 = ToSafeString(odata("loop3"))
                                        oFieldArray.Add("ARRAY DATASTRUCTURE DS_" & sDataStructureName)
                                        oFieldArray.Add("BEGIN")
                                        oFieldArray.Add("FIELDSET " & sDataStructureName)
                                        If bHasEndorsement Then
                                            ''check if endorsement for current table name
                                            ''add array fieldset for endoresement
                                            nResult = GetSWColumnName(sTableName, oDataModelArray(0, lTemp1), oSWColumnName)
                                            If nResult <> PMEReturnCode.PMTrue Then
                                                Throw New ApplicationException("GetSWColumnName Failed")
                                            End If

                                            If Not oSWColumnName Is Nothing Then
                                                For nCtr As Integer = oSWColumnName.GetLowerBound(1) To oSWColumnName.GetUpperBound(1)
                                                    oFieldArray.Add("ARRAY FIELDSET " & sDataStructureName & "_" & oSWColumnName(0, nCtr))
                                                    oCCMFieldSets.Add("ENDORSEMENT~" & sDataStructureName & "_" & oSWColumnName(0, nCtr))
                                                    nResult = UpdateEndorsementDataStructureName(oDataModelArray(0, lTemp1), sTableName, sDataStructureName, oSWColumnName(0, nCtr))
                                                    If nResult <> PMEReturnCode.PMTrue Then
                                                        Throw New ApplicationException("UpdateEndorsementDataStructureName Failed")
                                                    End If
                                                Next
                                            End If
                                        End If
                                        oCCMFieldSets.Add(oDataModelArray(0, lTemp1) & "~" & sTableName & "~" & sDataStructureName)
                                        nLevel = nLevel + 1
                                        If sLoop1 = sNextRowLoop1 AndAlso sLoop2 = sNextRowLoop2 AndAlso sNextRowLoop3 <> "" AndAlso sLoop3 = sNextRowLoop3 Then
                                        ElseIf sLoop1 <> sNextRowLoop1 Then
                                            oFieldArray.Add("END")
                                            oFieldArray.Add("END")
                                            oFieldArray.Add("END")
                                            nLevel = nLevel - 3
                                        ElseIf sLoop2 <> sNextRowLoop2 Then
                                            oFieldArray.Add("END")
                                            oFieldArray.Add("END")
                                            nLevel = nLevel - 2
                                        ElseIf sLoop1 = sNextRowLoop1 AndAlso sLoop2 = sNextRowLoop2 AndAlso sLoop3 <> sNextRowLoop3 Then
                                            oFieldArray.Add("END")
                                            nLevel = nLevel - 1
                                        Else
                                            oFieldArray.Add("END")
                                            nLevel = nLevel - 1
                                        End If
                                    End If
                                End If
                                nCount = nCount + 1
                            Next
                            For i As Integer = 1 To nLevel
                                oFieldArray.Add("END")
                            Next
                        ElseIf dtWpFields.Rows.Count = 1 Then
                            If Not String.IsNullOrEmpty(ToSafeString(dtWpFields.Rows(0)("loop1"))) AndAlso String.IsNullOrEmpty(ToSafeString(dtWpFields.Rows(0)("loop2"))) Then
                                If Not String.IsNullOrEmpty(ToSafeString(dtWpFields.Rows(0)("Table_Name"))) Then
                                    oFieldArray.Add("DATASTRUCTURE DS_" & ToSafeString(dtWpFields.Rows(0)("Table_Name")))
                                    oFieldArray.Add("BEGIN")
                                    oFieldArray.Add("FIELDSET " & ToSafeString(dtWpFields.Rows(0)("Table_Name")))
                                    If bHasEndorsement Then
                                        ''check if endorsement for current table name
                                        ''add array fieldset for endoresement
                                        nResult = GetSWColumnName(sTableName, oDataModelArray(0, lTemp1), oSWColumnName)
                                        If nResult <> PMEReturnCode.PMTrue Then
                                            Throw New ApplicationException("GetSWColumnName Failed")
                                        End If

                                        If Not oSWColumnName Is Nothing Then
                                            For nCtr As Integer = oSWColumnName.GetLowerBound(1) To oSWColumnName.GetUpperBound(1)
                                                oFieldArray.Add("ARRAY FIELDSET " & sDataStructureName & "_" & oSWColumnName(0, nCtr))
                                                oCCMFieldSets.Add("ENDORSEMENT~" & sDataStructureName & "_" & oSWColumnName(0, nCtr))
                                                nResult = UpdateEndorsementDataStructureName(oDataModelArray(0, lTemp1), sTableName, sDataStructureName, oSWColumnName(0, nCtr))
                                                If nResult <> PMEReturnCode.PMTrue Then
                                                    Throw New ApplicationException("UpdateEndorsementDataStructureName Failed")
                                                End If
                                            Next
                                        End If
                                    End If
                                    oCCMFieldSets.Add(oDataModelArray(0, lTemp1) & "~" & dtWpFields.Rows(0)("Table_Name") & "~" & dtWpFields.Rows(0)("DataStructure_Name"))
                                    oFieldArray.Add("END")
                                End If
                            ElseIf String.IsNullOrEmpty(ToSafeString(dtWpFields.Rows(0)("loop1"))) AndAlso String.IsNullOrEmpty(ToSafeString(dtWpFields.Rows(0)("loop2"))) Then
                                If Not String.IsNullOrEmpty(ToSafeString(dtWpFields.Rows(0)("Table_Name"))) Then
                                    oFieldArray.Add("DATASTRUCTURE DS_" & ToSafeString(dtWpFields.Rows(0)("Table_Name")))
                                    oFieldArray.Add("BEGIN")
                                    oFieldArray.Add("FIELDSET " & ToSafeString(dtWpFields.Rows(0)("Table_Name")))
                                    If bHasEndorsement Then
                                        ''check if endorsement for current table name
                                        ''add array fieldset for endoresement
                                        nResult = GetSWColumnName(sTableName, oDataModelArray(0, lTemp1), oSWColumnName)
                                        If nResult <> PMEReturnCode.PMTrue Then
                                            Throw New ApplicationException("GetSWColumnName Failed")
                                        End If

                                        If Not oSWColumnName Is Nothing Then
                                            For nCtr As Integer = oSWColumnName.GetLowerBound(1) To oSWColumnName.GetUpperBound(1)
                                                oFieldArray.Add("ARRAY FIELDSET " & sDataStructureName & "_" & oSWColumnName(0, nCtr))
                                                oCCMFieldSets.Add("ENDORSEMENT~" & sDataStructureName & "_" & oSWColumnName(0, nCtr))
                                                nResult = UpdateEndorsementDataStructureName(oDataModelArray(0, lTemp1), sTableName, sDataStructureName, oSWColumnName(0, nCtr))
                                                If nResult <> PMEReturnCode.PMTrue Then
                                                    Throw New ApplicationException("UpdateEndorsementDataStructureName Failed")
                                                End If
                                            Next
                                        End If
                                    End If
                                    oCCMFieldSets.Add(oDataModelArray(0, lTemp1) & "~" & dtWpFields.Rows(0)("Table_Name") & "~" & dtWpFields.Rows(0)("DataStructure_Name"))
                                    oFieldArray.Add("END")
                                End If
                            End If
                        End If
                    Next
                    If bDataModelAdded Then
                        oFieldArray.Add("END")
                        oFieldArray.Add("")
                    End If
                End If
            Next
        Next
        If Not bDSDataModelsEndTagAdded Then
            oFieldArray.Add("END")
            bDSDataModelsEndTagAdded = True ''end tag for ds_datamodelcode
        End If

        oFieldArray.Add("END")

        Return nResult
    End Function

    ''' <summary>
    ''' update/populate CCM fieldsets
    ''' </summary>
    ''' <param name="sGISDataModelCode"></param>
    ''' <param name="oCCMFieldSetArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdateCCMFieldsets(ByVal sGISDataModelCode As String, ByVal oCCMFieldSetArray As ArrayList, ByVal bRecreateDataBackBone As Boolean) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim sTableName As String
        Dim sDataStructureName As String
        Dim sCoreFieldSetName As String
        Dim sEndorsementFieldSet As String

        For Each sFieldSetVal As String In oCCMFieldSetArray
            If bRecreateDataBackBone Then
                If sFieldSetVal.Contains("CORE~") Then
                    sCoreFieldSetName = sFieldSetVal.Split("~")(1)
                    nResult = UpdateFieldSets(sCoreFieldSetName, String.Empty, True) ''update core fieldsets once
                    If nResult <> PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "UpdateCCMFieldsets Core fields Failed: " & sCoreFieldSetName)
                        nResult = PMEReturnCode.PMTrue ''proceed further to update ccm fields
                    End If
                End If
                If sFieldSetVal.Contains("ENDORSEMENT~") Then
                    sEndorsementFieldSet = sFieldSetVal.Split("~")(1) ''update standard wording fieldset in CCM once
                    nResult = UpdateEndorsementFieldSets(sEndorsementFieldSet)

                    If nResult <> PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "UpdateCCMFieldsets Endorsement fields Failed: " & sEndorsementFieldSet)
                        nResult = PMEReturnCode.PMTrue ''proceed further to update ccm fields
                    End If
                End If
            End If

            If sFieldSetVal.Contains(sGISDataModelCode & "~") Then
                sTableName = sFieldSetVal.Split("~")(1) ''update fieldset based on datamodel
                sDataStructureName = sFieldSetVal.Split("~")(2) ''DataStructure name
                nResult = UpdateFieldSets(sTableName, sDataStructureName, False)

                If nResult <> PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "UpdateCCMFieldsets Data Model fields Failed: " & sDataStructureName)
                    nResult = PMEReturnCode.PMTrue ''proceed further to update ccm fields
                End If
            End If
        Next

        Return nResult
    End Function

    ''' <summary>
    ''' Get core fields from DB
    ''' </summary>
    ''' <param name="r_oLoopArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetCoreFieldsForDataBackbone(ByRef r_oLoopArray(,) As Object) As Integer
        Dim nResult As Integer

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Execute selection Query
        nResult = m_oDatabase.SQLSelect(sSQL:=ACGetCoreFieldsSQL, sSQLName:=ACGetCoreFieldsName, bStoredProcedure:=ACGetCoreFieldsStored, vResultArray:=r_oLoopArray)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetCoreFieldsForDataBackbone SP Failed")
        End If

        Return nResult
    End Function

    ''' <summary>
    ''' Get all distinct datamodel codes
    ''' </summary>
    ''' <param name="oDataModelCodeArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetDataModelCode(ByRef oDataModelCodeArray(,) As Object) As Integer
        Dim nResult As Integer

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Execute selection Query
        nResult = m_oDatabase.SQLSelect(sSQL:=ACGetDataModelCodeSQL, sSQLName:=ACGetDataModelCodeName, bStoredProcedure:=ACGetDataModelCodeStored, vResultArray:=oDataModelCodeArray)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetDataModelCode SP Failed")
        End If

        Return nResult
    End Function

    ''' <summary>
    ''' Get datatable with sub_group, loop1, loop2 etc. for particular datamodel
    ''' </summary>
    ''' <param name="sDataModelCode"></param>
    ''' <param name="sSubGrp"></param>
    ''' <param name="r_dtWpFields"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetDataStructureDetails(ByVal sDataModelCode As String, ByVal sSubGrp As String, ByRef r_dtWpFields As DataTable) As Integer
        Dim nResult As Integer

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        m_oDatabase.Parameters.Add(sName:="DataModelCode", vValue:=sDataModelCode, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
        m_oDatabase.Parameters.Add(sName:="Sub_Group", vValue:=sSubGrp, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)

        ' Execute selection Query
        nResult = m_oDatabase.ExecuteDataTable(sSQL:=ACGetWPFieldsSQL, sSQLName:=ACGetWPFieldsName, bStoredProcedure:=ACGetWPFieldsStored, oRecordset:=r_dtWpFields)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetDataStructureDetails SP Failed")
        End If

        Return nResult
    End Function

    ''' <summary>
    ''' Get distinct sub_group for particular datamodel
    ''' </summary>
    ''' <param name="sDataModelCode"></param>
    ''' <param name="aoResultArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetSubGroupDetails(ByVal sDataModelCode As String, ByRef aoResultArray(,) As Object) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        m_oDatabase.Parameters.Add(sName:="DataModelCode", vValue:=sDataModelCode, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)

        ' Execute selection Query
        nResult = m_oDatabase.SQLSelect(sSQL:=ACGetSubGrpSQL, sSQLName:=ACGetSubGrpName, bStoredProcedure:=ACGetSubGrpStored, vResultArray:=aoResultArray)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetSubGroupDetails SP Failed")
        End If

        Return nResult
    End Function

    ''' <summary>
    ''' Get CCM doc template from DB
    ''' </summary>
    ''' <param name="aoResultArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetCCMDocumentTemplates(ByVal sTemplateDescription As String, ByRef aoResultArray(,) As Object) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue

        'Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        m_oDatabase.Parameters.Add(sName:="CCMDocumentTemplate", vValue:=sTemplateDescription, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)

        nResult = m_oDatabase.SQLSelect(sSQL:=ACGetCCMDocTemplateSQL, sSQLName:=ACGetCCMDocTemplateName, bStoredProcedure:=ACGetCCMDocTemplateStored, vResultArray:=aoResultArray)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetCCMDocumentTemplates SP Failed")
        End If

        Return nResult
    End Function

    ''' <summary>
    ''' Check if CCM Doc template is already mapped to CCM or not
    ''' </summary>
    ''' <param name="sTemplateDescription"></param>
    ''' <param name="bIsCCMTemplateMapped"></param>
    ''' <remarks></remarks>
    Public Sub IsCCMTemplateMapped(ByVal sTemplateDescription As String, ByVal sDocumentTemplateId As String, ByRef bIsCCMTemplateMapped As Boolean)
        'Use the description to determine if this template has already been mapped or not
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim oResultArray As Object = Nothing

        nResult = GetCCMDocumentTemplates(sTemplateDescription, oResultArray)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetCCMDocumentTemplates Failed")
        End If

        If oResultArray IsNot Nothing Then
            For iTemp As Integer = oResultArray.GetLowerBound(1) To oResultArray.GetUpperBound(1)
                If Not String.IsNullOrEmpty(sTemplateDescription) Then
                    If oResultArray(0, iTemp).Trim() = sDocumentTemplateId.Trim() Then
                        bIsCCMTemplateMapped = False
                    Else
                        bIsCCMTemplateMapped = True ''if trying to map CCM template to already mapped template
                    End If
                End If
            Next
        End If

    End Sub

    ''' <summary>
    ''' Update fields sets for particular datamodel
    ''' </summary>
    ''' <param name="sTableName">FieldSet Name</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdateFieldSets(ByVal sTableName As String, ByVal sDataStructureName As String, ByVal bCoreFieldSetOrNot As Boolean) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim oResultArray(,) As Object = Nothing

        Dim fieldsList As New StringBuilder
        Dim sCCMWebServiceURL As String = String.Empty
        Dim sCCMCustomer As String = String.Empty
        Dim sCCMPartner As String = String.Empty
        Dim sCCMContractTypeName As String = String.Empty
        Dim sCCMRepositoryProject As String = String.Empty
        Dim sCCMContractTypeVersion As String = String.Empty
        Dim sCCMStatus As String = String.Empty
        Dim sJobID As String = Guid.NewGuid().ToString().Replace("-", "")
        Dim sFieldSet As String = String.Empty
        Dim azureWebService As CCM_Azure.TPInTheCloudClient

        If Not String.IsNullOrEmpty(sTableName) Then
            nResult = GetCCMFieldsForFieldSet(sTableName, bCoreFieldSetOrNot, oResultArray)
            If nResult <> PMEReturnCode.PMTrue Then
                Throw New ApplicationException("GetCCMFieldsForFieldSet Failed")
            End If
        End If

        If bCoreFieldSetOrNot Then
            sTableName = sTableName.Replace("-", "").Replace(",", "").Replace("/", "").Replace(" ", "")
            sFieldSet = sTableName
        Else
            sFieldSet = sDataStructureName
        End If

        If Not oResultArray Is Nothing Then

            Dim lbnd, ubnd As Integer

            lbnd = oResultArray.GetLowerBound(1)
            ubnd = oResultArray.GetUpperBound(1)

            For i As Integer = lbnd To ubnd

                fieldsList.Append(oResultArray(0, i))
                If (i < ubnd) Then fieldsList.Append(",")

            Next

            GetCCMSystemOptions(sCCMWebServiceURL, sCCMCustomer, sCCMPartner, sCCMContractTypeName, sCCMRepositoryProject, sCCMContractTypeVersion, sCCMStatus)

            Try
                azureWebService = New CCM_Azure.TPInTheCloudClient()
                Dim getDocTemplatesRequestInfoAzure As New CCM_Azure.requestinfo()

                SetServiceAddressAndBinding(sCCMWebServiceURL, azureWebService)

                getDocTemplatesRequestInfoAzure = azureWebService.DesignerAddFieldsV1(sCCMPartner, sCCMCustomer, sCCMContractTypeName, sCCMContractTypeVersion, sJobID, sCCMRepositoryProject,
                                                                                      sFieldSet, fieldsList.ToString)
            Catch timeoutExcep As TimeoutException
                nResult = gPMConstants.PMEReturnCode.PMError
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Service Timeout calling CCM Server DesignerAddFieldsV1 method.", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFieldSets", excep:=timeoutExcep)

            Catch aiaFaultExcep As FaultException(Of CCM_Azure.AiaFaultV1)
                nResult = gPMConstants.PMEReturnCode.PMError

                Select Case aiaFaultExcep.Detail.errorcode
                    Case "302"
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                           sMsg:="AiaFaultV1 Fault Exception calling CCM Server DesignerAddFieldsV1 method with error code " & aiaFaultExcep.Detail.errorcode & ".",
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFieldSets", excep:=aiaFaultExcep)
                    Case Else
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                           sMsg:="AiaFaultV1 Fault Exception calling CCM Server DesignerAddFieldsV1 method with error code " & aiaFaultExcep.Detail.errorcode & ".",
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFieldSets", excep:=aiaFaultExcep)
                End Select

            Catch faultExcep As FaultException
                nResult = gPMConstants.PMEReturnCode.PMError
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Fault Exception calling CCM Server DesignerAddFieldsV1 method.",
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFieldSets", excep:=faultExcep)

            Catch excep As Exception
                nResult = gPMConstants.PMEReturnCode.PMError
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unknown Exception calling CCM Server DesignerAddFieldsV1 method.", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFieldSets", excep:=excep)
            Finally
                azureWebService = Nothing
            End Try

        End If

        Return nResult
    End Function

    ''' <summary>
    ''' Update fields for standard wording in CCM
    ''' </summary>
    ''' <param name="sEndorsementFieldSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdateEndorsementFieldSets(ByVal sEndorsementFieldSet As String) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const sFieldList As String = "SWCODE,SWDESC,FileName,FilePath"
        Dim sCCMWebServiceURL As String = String.Empty
        Dim sCCMCustomer As String = String.Empty
        Dim sCCMPartner As String = String.Empty
        Dim sCCMContractTypeName As String = String.Empty
        Dim sCCMRepositoryProject As String = String.Empty
        Dim sCCMContractTypeVersion As String = String.Empty
        Dim sCCMStatus As String = String.Empty
        Dim sJobID As String = Guid.NewGuid().ToString().Replace("-", "")
        Dim azureWebService As CCM_Azure.TPInTheCloudClient

        GetCCMSystemOptions(sCCMWebServiceURL, sCCMCustomer, sCCMPartner, sCCMContractTypeName, sCCMRepositoryProject, sCCMContractTypeVersion, sCCMStatus)

        Try
            azureWebService = New CCM_Azure.TPInTheCloudClient()
            Dim getDocTemplatesRequestInfoAzure As New CCM_Azure.requestinfo()

            SetServiceAddressAndBinding(sCCMWebServiceURL, azureWebService)

            getDocTemplatesRequestInfoAzure = azureWebService.DesignerAddFieldsV1(sCCMPartner, sCCMCustomer, sCCMContractTypeName, sCCMContractTypeVersion, sJobID, sCCMRepositoryProject,
                                                                                  sEndorsementFieldSet, sFieldList)
        Catch excep As Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateFieldSets in CCM Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFieldSets", excep:=excep)

        Finally
            azureWebService = Nothing
        End Try

        Return nResult
    End Function

    ''' <summary>
    ''' Get column names for particular fieldset
    ''' </summary>
    ''' <param name="sFieldSet"></param>
    ''' <param name="aoResultArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetCCMFieldsForFieldSet(ByVal sFieldSet As String, ByVal bCoreOrNot As Boolean, ByRef aoResultArray(,) As Object) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ''Get core fields
        If bCoreOrNot Then

            m_oDatabase.Parameters.Add(sName:="Table_Name", vValue:=sFieldSet, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)

            nResult = m_oDatabase.SQLSelect(sSQL:=ACGetCCMCoreFieldsForFieldSetSQL, sSQLName:=ACGetCCMCoreFieldsForFieldSetName, bStoredProcedure:=ACGetCCMCoreFieldsForFieldSetStored,
                                            vResultArray:=aoResultArray)
            If nResult <> PMEReturnCode.PMTrue Then
                Throw New ApplicationException("GetCCMFieldsForFieldSet Failed")
            End If
        Else
            ''Get Fields based on FieldSet excluding standard wordings
            m_oDatabase.Parameters.Add(sName:="FieldSet", vValue:=sFieldSet, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
            nResult = m_oDatabase.SQLSelect(sSQL:=ACGetCCMFieldsForFieldSetSQL, sSQLName:=ACGetCCMFieldsForFieldSetName, bStoredProcedure:=ACGetCCMFieldsForFieldSetStored,
                                            vResultArray:=aoResultArray)
            If nResult <> PMEReturnCode.PMTrue Then
                Throw New ApplicationException("GetCCMFieldsForFieldSet SP Failed")
            End If
        End If

        Return nResult
    End Function

    ''' <summary>
    ''' get fieldsets from CCM web service
    ''' </summary>
    ''' <param name="sCCMDocumentName"></param>
    ''' <param name="r_dtCCMFields"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCCMDocumentTemplateFields(ByVal sCCMDocumentName As String, ByRef r_dtCCMFields As DataTable) As Integer
        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim sCCMWebServiceURL As String = String.Empty
        Dim sCCMCustomer As String = String.Empty
        Dim sCCMPartner As String = String.Empty
        Dim sCCMContractTypeName As String = String.Empty
        Dim sCCMRepositoryProject As String = String.Empty
        Dim sCCMContractTypeVersion As String = String.Empty
        Dim sCCMStatus As String = String.Empty
        Dim sJobID As String = Guid.NewGuid().ToString().Replace("-", "")

        Dim xmlTemplateFieldList As String = String.Empty
        Dim XMLDoc As XmlDocument = New XmlDocument
        Dim oDocTemplatePackList() As Byte = Nothing
        Dim oDocDataBackboneXML() As Byte = Nothing
        Dim oArray As String() = Nothing
        Dim sCCMBackboneXMLFilePath As String = String.Empty
        Dim sCCMBackboneXMLFileName As String
        Dim dtCCMFields As New DataTable("DataCCMFields")
        dtCCMFields.Columns.Add("TableName", Type.GetType("System.String"))
        dtCCMFields.Columns.Add("ColumnName", Type.GetType("System.String"))
        Dim azureWebService As CCM_Azure.TPInTheCloudClient

        nResult = CType(GISSharedConstants.GetCCMBackboneFileName(sCCMBackboneFilePath:=sCCMBackboneXMLFilePath), PMEReturnCode)
        If nResult <> PMEReturnCode.PMTrue Then
            Return PMEReturnCode.PMFalse
        End If

        sCCMBackboneXMLFileName = sCCMBackboneXMLFilePath & ACCCMBackboneXMLSuffix & ACCCMBackboneXMLFileExtension
        Dim sInputDatabackBoneXML As String = File.ReadAllText(sCCMBackboneXMLFileName)

        GetCCMSystemOptions(sCCMWebServiceURL, sCCMCustomer, sCCMPartner, sCCMContractTypeName, sCCMRepositoryProject, sCCMContractTypeVersion, sCCMStatus)

        Try
            azureWebService = New CCM_Azure.TPInTheCloudClient
            Dim getDocTemplatesRequestInfoAzure As New CCM_Azure.requestinfo()

            SetServiceAddressAndBinding(sCCMWebServiceURL, azureWebService)

            DirectCast(azureWebService.Endpoint.Binding, ServiceModel.BasicHttpBinding).MaxReceivedMessageSize = kMaxReceivedMessageSize
            DirectCast(azureWebService.Endpoint.Binding, ServiceModel.BasicHttpBinding).ReaderQuotas.MaxBytesPerRead = kMaxReceivedMessageSize
            DirectCast(azureWebService.Endpoint.Binding, ServiceModel.BasicHttpBinding).ReaderQuotas.MaxArrayLength = kMaxReceivedMessageSize
            DirectCast(azureWebService.Endpoint.Binding, ServiceModel.BasicHttpBinding).ReaderQuotas.MaxDepth = kMaxReceivedMessageSize
            DirectCast(azureWebService.Endpoint.Binding, ServiceModel.BasicHttpBinding).ReaderQuotas.MaxNameTableCharCount = kMaxReceivedMessageSize
            DirectCast(azureWebService.Endpoint.Binding, ServiceModel.BasicHttpBinding).ReaderQuotas.MaxStringContentLength = kMaxReceivedMessageSize

            oDocDataBackboneXML = System.Text.Encoding.UTF8.GetBytes(sInputDatabackBoneXML)
            getDocTemplatesRequestInfoAzure = azureWebService.ComposeDocxV1(sCCMPartner, sCCMCustomer, sCCMContractTypeName, sCCMContractTypeVersion, sJobID, sCCMRepositoryProject,
                                                                            sCCMDocumentName, sCCMStatus, oDocDataBackboneXML, oDocTemplatePackList)
            xmlTemplateFieldList = System.Text.ASCIIEncoding.ASCII.GetString(oDocDataBackboneXML)

            If Not String.IsNullOrEmpty(xmlTemplateFieldList) Then
                XMLDoc.LoadXml(xmlTemplateFieldList)
                Dim elemList As XmlNodeList = XMLDoc.GetElementsByTagName("TemplateFields")
                For i As Integer = 0 To elemList.Count - 1
                    oArray = elemList(i).InnerText.Split(",")
                Next
            End If

            If Not oArray Is Nothing Then
                For Each sFieldVal As String In oArray
                    If sFieldVal <> "" Then
                        dtCCMFields.Rows.Add(sFieldVal.Split(".")(0), sFieldVal.Split(".")(1))
                        If sFieldVal.Split(".")(1) = "FilePath" OrElse sFieldVal.Split(".")(1) = "SWCODE" Then
                            dtCCMFields.Rows.Add(sFieldVal.Split(".")(0).Trim(), "UID")
                        End If
                    End If
                Next
            End If

            'Get the Table Name from the wp_fields DataStructure_Name 
            GetTableNameFromWPFields(dtCCMFields, r_dtCCMFields)

            If r_dtCCMFields IsNot Nothing AndAlso r_dtCCMFields.Rows.Count > 0 Then
                For Each row As DataRow In r_dtCCMFields.Rows
                    If ToSafeString(row("TableName")) = "" Then
                        bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="DataStructure Name is empty in wp fields. Please, Re-Build Datamodel after selecting CCM for document Generation.", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateCCMDataXML")
                    End If
                Next
            End If
        Catch excep As Exception
            nResult = PMEReturnCode.PMError
            ' Log Error Message
            Dim sErrorMessage As String = DirectCast(excep, System.ServiceModel.FaultException(Of bCCMDocumentProduction.CCM_Azure.AiaFaultV1)).Detail.message
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:=sErrorMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="GetCCMDocumentTemplateFields", excep:=excep)
        Finally
            azureWebService = Nothing
        End Try
        Return nResult
    End Function

    ''' <summary>
    ''' Compose the CCM Document
    ''' </summary>
    ''' <param name="sCCMDocumentName"></param>
    ''' <param name="sInputXMLString"></param>
    ''' <param name="r_oDocTemplatePackList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ComposeDocx(ByVal sCCMDocumentName As String, ByVal sInputXMLString As String, ByRef r_oDocTemplatePackList() As Byte) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim sCCMWebServiceURL As String = String.Empty
        Dim sCCMCustomer As String = String.Empty
        Dim sCCMPartner As String = String.Empty
        Dim sCCMContractTypeName As String = String.Empty
        Dim sCCMRepositoryProject As String = String.Empty
        Dim sCCMContractTypeVersion As String = String.Empty
        Dim sCCMStatus As String = String.Empty
        Dim sJobID As String = Guid.NewGuid().ToString().Replace("-", "")
        Dim oDocDataBackboneXML() As Byte = Nothing
        Dim azureWebService As CCM_Azure.TPInTheCloudClient
        Dim sCCMLoggingEnabled As String = String.Empty

        GetCCMSystemOptions(sCCMWebServiceURL, sCCMCustomer, sCCMPartner, sCCMContractTypeName, sCCMRepositoryProject, sCCMContractTypeVersion, sCCMStatus, sCCMLoggingEnabled)

        Try
            azureWebService = New CCM_Azure.TPInTheCloudClient
            Dim getDocTemplatesRequestInfoAzure As New CCM_Azure.requestinfo()

            SetServiceAddressAndBinding(sCCMWebServiceURL, azureWebService)

            DirectCast(azureWebService.Endpoint.Binding, ServiceModel.BasicHttpBinding).MaxReceivedMessageSize = kMaxReceivedMessageSize
            DirectCast(azureWebService.Endpoint.Binding, ServiceModel.BasicHttpBinding).ReaderQuotas.MaxBytesPerRead = kMaxReceivedMessageSize
            DirectCast(azureWebService.Endpoint.Binding, ServiceModel.BasicHttpBinding).ReaderQuotas.MaxArrayLength = kMaxReceivedMessageSize
            DirectCast(azureWebService.Endpoint.Binding, ServiceModel.BasicHttpBinding).ReaderQuotas.MaxDepth = kMaxReceivedMessageSize
            DirectCast(azureWebService.Endpoint.Binding, ServiceModel.BasicHttpBinding).ReaderQuotas.MaxNameTableCharCount = kMaxReceivedMessageSize
            DirectCast(azureWebService.Endpoint.Binding, ServiceModel.BasicHttpBinding).ReaderQuotas.MaxStringContentLength = kMaxReceivedMessageSize

            oDocDataBackboneXML = Text.Encoding.UTF8.GetBytes(sInputXMLString)

            If sCCMLoggingEnabled = "1" Then
                bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogInfo, sMsg:="CCM document generation dataset XML for " & sCCMDocumentName & " : " & vbCrLf & sInputXMLString, vApp:=ACApp,
                              vClass:=ACClass, vMethod:="ComposeDocx")
            End If

            getDocTemplatesRequestInfoAzure = azureWebService.ComposeDocxV1(sCCMPartner, sCCMCustomer, sCCMContractTypeName, sCCMContractTypeVersion, sJobID,
                                                                            sCCMRepositoryProject, sCCMDocumentName, sCCMStatus,
                                                                           oDocDataBackboneXML, r_oDocTemplatePackList)

        Catch excep As Exception
            nResult = PMEReturnCode.PMError
            Dim sErrorMessage As String = DirectCast(excep, System.ServiceModel.FaultException(Of bCCMDocumentProduction.CCM_Azure.AiaFaultV1)).Detail.message
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="Input XML : " & vbCrLf & sInputXMLString, vApp:=ACApp,
                               vClass:=ACClass, vMethod:="ComposeDocx")
        Finally
            azureWebService = Nothing
        End Try
        Return nResult
    End Function

    ''' <summary>
    ''' Get CCM system options
    ''' </summary>
    ''' <param name="r_sCCMWebServiceURL"></param>
    ''' <param name="r_sCCMCustomer"></param>
    ''' <param name="r_sCCMPartner"></param>
    ''' <param name="r_sCCMContractTypeName"></param>
    ''' <param name="r_sCCMRepositoryProject"></param>
    ''' <param name="r_sCCMContractTypeVersion"></param>
    ''' <param name="r_sCCMStatusDesc"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetCCMSystemOptions(ByRef r_sCCMWebServiceURL As String, ByRef r_sCCMCustomer As String, ByRef r_sCCMPartner As String,
                                          ByRef r_sCCMContractTypeName As String, ByRef r_sCCMRepositoryProject As String, ByRef r_sCCMContractTypeVersion As String,
                                          ByRef r_sCCMStatusDesc As String) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim sCCMStatusID As String = ""

        ' Get system option CCMWebServiceURL
        nResult = GetSystemOption(nOptionNumber:=kSystemOptionCCMWebServiceURL, r_sSystemOptionValue:=r_sCCMWebServiceURL)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetSystemOption Failed for SystemOptionCCMWebServiceURL")
        End If

        ' Get system option CCMCustomer
        nResult = GetSystemOption(nOptionNumber:=kSystemOptionCCMCustomer, r_sSystemOptionValue:=r_sCCMCustomer)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetSystemOption Failed for SystemOptionCCMCustomer")
        End If

        ' Get system option CCMPartner
        nResult = GetSystemOption(nOptionNumber:=kSystemOptionCCMPartner, r_sSystemOptionValue:=r_sCCMPartner)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetSystemOption Failed for SystemOptionCCMPartner")
        End If

        ' Get system option CCMContractTypeName
        nResult = GetSystemOption(nOptionNumber:=kSystemOptionCCMContractTypeName, r_sSystemOptionValue:=r_sCCMContractTypeName)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetSystemOption Failed for SystemOptionCCMContractTypeName")
        End If

        ' Get system option RepositoryProj
        nResult = GetSystemOption(nOptionNumber:=kSystemOptionCCMRepositoryProject, r_sSystemOptionValue:=r_sCCMRepositoryProject)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetSystemOption Failed for SystemOptionCCMRepositoryProject")
        End If

        ' Get system option CCMContractTypeVersion
        nResult = GetSystemOption(nOptionNumber:=kSystemOptionCCMContractTypeVersion, r_sSystemOptionValue:=r_sCCMContractTypeVersion)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetSystemOption Failed for SystemOptionCCMContractTypeVersion")
        End If

        ' Get system option CCMStatus
        nResult = GetSystemOption(nOptionNumber:=kSystemOptionCCMStatus, r_sSystemOptionValue:=sCCMStatusID)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetSystemOption Failed for SystemOptionCCMStatus")
        End If

        r_sCCMStatusDesc = CType(ToSafeInteger(sCCMStatusID), CCMStatusDesc).ToString()

        Return nResult
    End Function

    ''' <summary>
    ''' Get CCM system options overloaded to additionally retrieve ccmloggingenabled setting
    ''' </summary>
    ''' <param name="r_sCCMWebServiceURL"></param>
    ''' <param name="r_sCCMCustomer"></param>
    ''' <param name="r_sCCMPartner"></param>
    ''' <param name="r_sCCMContractTypeName"></param>
    ''' <param name="r_sCCMRepositoryProject"></param>
    ''' <param name="r_sCCMContractTypeVersion"></param>
    ''' <param name="r_sCCMStatusDesc"></param>
    ''' <param name="r_sCCMLoggingEnabled"></param>
    ''' <returns></returns>
    Private Function GetCCMSystemOptions(ByRef r_sCCMWebServiceURL As String, ByRef r_sCCMCustomer As String, ByRef r_sCCMPartner As String,
                                          ByRef r_sCCMContractTypeName As String, ByRef r_sCCMRepositoryProject As String, ByRef r_sCCMContractTypeVersion As String,
                                          ByRef r_sCCMStatusDesc As String, ByRef r_sCCMLoggingEnabled As String) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim sCCMStatusID As String = ""

        ' Get system option CCMWebServiceURL
        nResult = GetSystemOption(nOptionNumber:=kSystemOptionCCMWebServiceURL, r_sSystemOptionValue:=r_sCCMWebServiceURL)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetSystemOption Failed for SystemOptionCCMWebServiceURL")
        End If

        ' Get system option CCMCustomer
        nResult = GetSystemOption(nOptionNumber:=kSystemOptionCCMCustomer, r_sSystemOptionValue:=r_sCCMCustomer)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetSystemOption Failed for SystemOptionCCMCustomer")
        End If

        ' Get system option CCMPartner
        nResult = GetSystemOption(nOptionNumber:=kSystemOptionCCMPartner, r_sSystemOptionValue:=r_sCCMPartner)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetSystemOption Failed for SystemOptionCCMPartner")
        End If

        ' Get system option CCMContractTypeName
        nResult = GetSystemOption(nOptionNumber:=kSystemOptionCCMContractTypeName, r_sSystemOptionValue:=r_sCCMContractTypeName)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetSystemOption Failed for SystemOptionCCMContractTypeName")
        End If

        ' Get system option RepositoryProj
        nResult = GetSystemOption(nOptionNumber:=kSystemOptionCCMRepositoryProject, r_sSystemOptionValue:=r_sCCMRepositoryProject)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetSystemOption Failed for SystemOptionCCMRepositoryProject")
        End If

        ' Get system option CCMContractTypeVersion
        nResult = GetSystemOption(nOptionNumber:=kSystemOptionCCMContractTypeVersion, r_sSystemOptionValue:=r_sCCMContractTypeVersion)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetSystemOption Failed for SystemOptionCCMContractTypeVersion")
        End If

        ' Get system option CCMStatus
        nResult = GetSystemOption(nOptionNumber:=kSystemOptionCCMStatus, r_sSystemOptionValue:=sCCMStatusID)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetSystemOption Failed for SystemOptionCCMStatus")
        End If

        nResult = GetSystemOption(nOptionNumber:=kSystemOptionCCMEnableDatasetLogging, r_sSystemOptionValue:=r_sCCMLoggingEnabled)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetSystemOption Failed for SystemOptionCCMEnableDatasetLogging")
        End If

        r_sCCMStatusDesc = CType(ToSafeInteger(sCCMStatusID), CCMStatusDesc).ToString()

        Return nResult
    End Function

    ''' <summary>
    ''' Get system option values for CCM
    ''' </summary>
    ''' <param name="nOptionNumber"></param>
    ''' <param name="r_sSystemOptionValue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetSystemOption(ByVal nOptionNumber As Integer, ByRef r_sSystemOptionValue As String) As Integer
        Dim nReturn As Integer = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=nOptionNumber, r_sOptionValue:=r_sSystemOptionValue)
        Return nReturn
    End Function

    ''' <summary>
    ''' Update core fieldset name in DB
    ''' </summary>
    ''' <param name="oCCMFieldSetArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdateCoreFieldSets(ByVal oCCMFieldSetArray As ArrayList) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue

        For Each sFieldSetVal As String In oCCMFieldSetArray
            If sFieldSetVal.Contains("CORE~") Then
                nResult = UpdateCoreFieldSetsToDB(sFieldSetVal) ''update core fieldsets to table
                If nResult <> PMEReturnCode.PMTrue Then
                    Throw New ApplicationException("UpdateCoreFieldSets Core Failed")
                End If
            End If
        Next
        Return nResult
    End Function

    ''' <summary>
    ''' Update core fieldset name in DB stored Proc
    ''' </summary>
    ''' <param name="sFieldSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdateCoreFieldSetsToDB(ByVal sFieldSet As String) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()
        Dim sMainGrp As String = sFieldSet.Split("~")(1).Replace("Core", "")
        Dim sSubGrp As String = sFieldSet.Split("~")(2)

        Dim sCoreFieldSet As String = "Core" & sMainGrp & sSubGrp
        sCoreFieldSet = sCoreFieldSet.Replace("-", "").Replace(",", "").Replace("/", "").Replace(" ", "")

        m_oDatabase.Parameters.Add(sName:="main_group", vValue:=sMainGrp, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
        m_oDatabase.Parameters.Add(sName:="sub_group", vValue:=sSubGrp, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
        m_oDatabase.Parameters.Add(sName:="coreFieldset", vValue:=sCoreFieldSet, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)

        nResult = m_oDatabase.SQLAction(sSQL:=ACUpdateCoreFieldSetsSQL, sSQLName:=ACUpdateCoreFieldSetsName, bStoredProcedure:=ACUpdateCoreFieldSetsStored)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("UpdateCoreFieldSetsToDB SP Failed")
        End If

        Return nResult
    End Function

    ''' <summary>
    ''' Update datastructure name as _Parent_ChildObj in DB
    ''' </summary>
    ''' <param name="sDataModelCode"></param>
    ''' <param name="sSubGroup"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdateDataStructureName(ByVal sDataModelCode As String, ByVal sSubGroup As String) As Integer
        Dim nResult As Integer
        Dim sLoop1 As String
        Dim sLoop2 As String
        Dim sLoop3 As String
        Dim sTableName As String
        Dim bDataStructure As Boolean = False
        Dim sDataStructureName As String = String.Empty
        Dim dtWpFields As DataTable = New DataTable()
        Dim sParentDatastructureName As String
        nResult = GetDataStructureDetails(sDataModelCode, sSubGroup, dtWpFields) ''get values of main_group, sub_group, loop1, loop2, loop3, table_name
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetDataStructureDetails Failed")
        End If

        If dtWpFields.Rows.Count > 1 Then
            For i As Integer = 0 To dtWpFields.Rows.Count - 1
                sLoop1 = ToSafeString(dtWpFields.Rows(i)("loop1"))
                sLoop2 = ToSafeString(dtWpFields.Rows(i)("loop2"))
                sLoop3 = ToSafeString(dtWpFields.Rows(i)("loop3"))
                sTableName = ToSafeString(dtWpFields.Rows(i)("Table_Name"))
                sParentDatastructureName = ToSafeString(dtWpFields.Rows(i)("ParentDatastructureName"))
                If String.IsNullOrEmpty(sLoop1) AndAlso String.IsNullOrEmpty(sLoop2) AndAlso String.IsNullOrEmpty(sLoop3) Then ''get Parent object
                    If Not String.IsNullOrEmpty(sTableName) Then
                        sDataStructureName = sTableName
                        bDataStructure = True
                        UpdateDataStructureNameToDB(sDataModelCode, sSubGroup, sLoop1, sLoop2, sLoop3, sTableName, sDataStructureName)
                    End If
                End If
                If Not String.IsNullOrEmpty(sLoop1) AndAlso String.IsNullOrEmpty(sLoop2) AndAlso String.IsNullOrEmpty(sLoop3) Then
                    If Not String.IsNullOrEmpty(sTableName) Then
                        If Not bDataStructure Then
                            sDataStructureName = sTableName ''Parent object
                            UpdateDataStructureNameToDB(sDataModelCode, sSubGroup, sLoop1, sLoop2, sLoop3, sTableName, sDataStructureName)
                        Else
                            ''Get child obj in case of loop1
                            sDataStructureName = (sParentDatastructureName & sTableName.Remove(0, sDataModelCode.Length)).ToUpper()
                            UpdateDataStructureNameToDB(sDataModelCode, sSubGroup, sLoop1, sLoop2, sLoop3, sTableName, sDataStructureName)
                        End If

                    End If
                End If
                If Not String.IsNullOrEmpty(sLoop1) AndAlso Not String.IsNullOrEmpty(sLoop2) AndAlso String.IsNullOrEmpty(sLoop3) Then
                    If Not String.IsNullOrEmpty(sTableName) Then
                        If Not bDataStructure Then
                            sDataStructureName = ToSafeString(dtWpFields.Rows(i - 1)("Table_Name")).Remove(0, sDataModelCode.Length) & sTableName.Remove(0, sDataModelCode.Length)
                            UpdateDataStructureNameToDB(sDataModelCode, sSubGroup, sLoop1, sLoop2, sLoop3, sTableName, sDataModelCode & sDataStructureName)
                        Else
                            ''Get child obj in case of loop1 and loop2
                            sDataStructureName = (sParentDatastructureName & "_" & sLoop1.Remove(0, sDataModelCode.Length) & sTableName.Remove(0, sDataModelCode.Length)).ToUpper()
                            UpdateDataStructureNameToDB(sDataModelCode, sSubGroup, sLoop1, sLoop2, sLoop3, sTableName, sDataStructureName)
                        End If
                    End If
                End If

                If Not String.IsNullOrEmpty(sLoop1) AndAlso Not String.IsNullOrEmpty(sLoop2) AndAlso Not String.IsNullOrEmpty(sLoop3) Then
                    If Not String.IsNullOrEmpty(sTableName) AndAlso Not String.IsNullOrEmpty(sDataStructureName) Then
                        ''Get child obj in case of loop1, loop2 and loop3
                        sDataStructureName = (sParentDatastructureName & "_" & sLoop1.Remove(0, sDataModelCode.Length) & "_" & sLoop2.Remove(0, sDataModelCode.Length) & sTableName.Remove(0, sDataModelCode.Length)).ToUpper()
                        UpdateDataStructureNameToDB(sDataModelCode, sSubGroup, sLoop1, sLoop2, sLoop3, sTableName, sDataStructureName)
                    End If
                End If
            Next

        ElseIf dtWpFields.Rows.Count = 1 Then
            sLoop1 = ToSafeString(dtWpFields.Rows(0)("loop1"))
            sLoop2 = ToSafeString(dtWpFields.Rows(0)("loop2"))
            sLoop3 = ToSafeString(dtWpFields.Rows(0)("loop3"))
            sTableName = ToSafeString(dtWpFields.Rows(0)("Table_Name"))
            sDataStructureName = sTableName
            UpdateDataStructureNameToDB(sDataModelCode, sSubGroup, sLoop1, sLoop2, sLoop3, sTableName, sDataStructureName)
        End If

        Return nResult
    End Function

    ''' <summary>
    ''' Update datastructure name as _Parent_ChildObj in DB
    ''' </summary>
    ''' <param name="sDataModelCode"></param>
    ''' <param name="sSubGrp"></param>
    ''' <param name="sLoop1"></param>
    ''' <param name="sLoop2"></param>
    ''' <param name="sLoop3"></param>
    ''' <param name="sTableName"></param>
    ''' <param name="sDataStructureName"></param>
    ''' <remarks></remarks>
    Private Sub UpdateDataStructureNameToDB(ByVal sDataModelCode As String, ByVal sSubGrp As String, ByVal sLoop1 As String,
                                            ByVal sLoop2 As String, ByVal sLoop3 As String, ByVal sTableName As String,
                                            ByVal sDataStructureName As String)
        Dim nResult As Integer
        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        m_oDatabase.Parameters.Add(sName:="DataModelCode", vValue:=sDataModelCode, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
        m_oDatabase.Parameters.Add(sName:="Sub_Group", vValue:=sSubGrp, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
        If Not String.IsNullOrEmpty(sLoop1) Then
            m_oDatabase.Parameters.Add(sName:="Loop1", vValue:=sLoop1, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
        Else
            m_oDatabase.Parameters.Add(sName:="Loop1", vValue:=DBNull.Value, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
        End If

        If Not String.IsNullOrEmpty(sLoop2) Then
            m_oDatabase.Parameters.Add(sName:="Loop2", vValue:=sLoop2, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
        Else
            m_oDatabase.Parameters.Add(sName:="Loop2", vValue:=DBNull.Value, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
        End If

        If Not String.IsNullOrEmpty(sLoop3) Then
            m_oDatabase.Parameters.Add(sName:="Loop3", vValue:=sLoop3, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
        Else
            m_oDatabase.Parameters.Add(sName:="Loop3", vValue:=DBNull.Value, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
        End If
        m_oDatabase.Parameters.Add(sName:="Table_Name", vValue:=sTableName, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
        m_oDatabase.Parameters.Add(sName:="DataStructure_Name", vValue:=sDataStructureName, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)

        ' Execute selection Query
        nResult = m_oDatabase.SQLAction(sSQL:=ACUpdateDataStructureNameSQL, sSQLName:=ACUpdateDataStructureName, bStoredProcedure:=ACUpdateDataStructureNameStored)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("UpdateDataStructureNameToDB SP Failed")
        End If

    End Sub

    ''' <summary>
    ''' Get standard wordings for particular table
    ''' </summary>
    ''' <param name="sTableName"></param>
    ''' <param name="sDataModelCode"></param>
    ''' <param name="r_oSWColumnName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetSWColumnName(ByVal sTableName As String, ByVal sDataModelCode As String, ByRef r_oSWColumnName(,) As Object)
        Dim nResult As Integer = PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        m_oDatabase.Parameters.Add(sName:="DataModelCode", vValue:=sDataModelCode, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
        m_oDatabase.Parameters.Add(sName:="Table_Name", vValue:=sTableName, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)

        nResult = m_oDatabase.SQLSelect(sSQL:=ACGetSWColumnNameSQL, sSQLName:=ACGetSWColumnName, bStoredProcedure:=ACGetSWColumnNameStored, vResultArray:=r_oSWColumnName)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetSWColumnName SP Failed")
        End If

        Return nResult
    End Function

    ''' <summary>
    ''' Create empty input XML to get fieldsets from CCM
    ''' </summary>
    ''' <param name="sOutPut"></param>
    ''' <remarks></remarks>
    Private Sub CreateInputXMLInAnalysisMode(ByRef sOutPut As String)
        Dim sOldLoop1 As String = ""

        Dim sMainGroup As String
        Dim sSubGroup As String = ""
        Dim sOldSubGroup As String = ""
        Dim sTableName As String = ""
        Dim sLoop1 As String = ""
        Dim sLoop2 As String = ""
        Dim sOldLoop2 As String = ""
        Dim sOldLoop3 As String = ""
        Dim sLoop3 As String = ""
        Dim xmlDoc As New XmlDocument
        Dim oXMLElement As XmlElement
        Dim sDataModel As String = ""
        Dim sOldDataModel As String = ""
        Dim sOldTableName As String = ""

        xmlDoc.Load(New StringReader(kCCMHeaderData))

        Dim i As Integer

        Dim dtWPfields As New DataTable
        GetWPFieldsForDataModels(dtWPfields)

        Dim sPrefix As String = "DS_"
        oXMLElement = xmlDoc.CreateElement("", sPrefix & "DataModels", kxmlns)
        xmlDoc.DocumentElement.AppendChild(oXMLElement)

        Dim oRootXMLElement As XmlElement
        Dim oChildXMLElement As XmlElement = Nothing
        Dim oChildXMLElementDataModel As XmlElement = Nothing
        Dim oChildXMLElementSubGroup As XmlElement = Nothing
        Dim oChildXMLElementLoop1 As XmlElement = Nothing
        Dim oChildXMLElementLoop2 As XmlElement = Nothing
        oRootXMLElement = oXMLElement
        Dim bFirstElement As Boolean = True
        Dim sStandardWording As String = ""
        Dim sDatastructureName As String


        For Each row As DataRow In dtWPfields.Rows
            i = i + 1

            sMainGroup = ReplaceSpecialCharacter(row("MAINGROUP"))
            sTableName = ToSafeString(row("TableName"))
            sSubGroup = ReplaceSpecialCharacter(row("SubGroup"))
            sLoop1 = ToSafeString(ReplaceSpecialCharacter(ToSafeString(row("LOOP1"))))
            sLoop2 = ToSafeString(ReplaceSpecialCharacter(ToSafeString(row("LOOP2"))))
            sLoop3 = ToSafeString(ReplaceSpecialCharacter(ToSafeString(row("LOOP3"))))
            sDataModel = ToSafeString(row("datamodel"))
            sStandardWording = ToSafeString(row("SW"))
            sDatastructureName = ToSafeString(row("datastructure_name"))

            If sDataModel <> "" Then
                sSubGroup = sSubGroup.Replace(sDataModel, "").ToUpper()
                sLoop1 = sLoop1.Replace(sDataModel, "").ToUpper()
                sLoop2 = sLoop2.Replace(sDataModel, "").ToUpper()
                sLoop3 = sLoop3.Replace(sDataModel, "").ToUpper()
            End If

            If sDataModel = "" Then
                'Core Fields. handle where Loop1 <>"". where Loop1= "" then no need to have to prepare the XML in Analysis Mode
                If sLoop1 <> "" Then
                    If sTableName <> sOldTableName AndAlso sLoop1 <> "" And sLoop2 = "" Then   'Handle Loop1
                        sPrefix = "DS_"
                        oXMLElement = xmlDoc.CreateElement("", sPrefix & sTableName, kxmlns)
                        xmlDoc.DocumentElement.AppendChild(oXMLElement)
                        sOldTableName = sTableName

                    ElseIf sTableName <> sOldTableName AndAlso sLoop2 <> "" AndAlso sLoop2 <> sOldLoop2 Then      'Handle Loop2
                        sPrefix = "DS_" & sTableName
                        addChildElement(xmlDoc, oXMLElement, sPrefix, "")
                        sOldLoop2 = sLoop2
                    End If
                End If
            Else

                If sDataModel <> sOldDataModel Then
                    oXMLElement = oRootXMLElement
                    sPrefix = "DS_" & sDataModel
                    oChildXMLElement = addChildElement(xmlDoc, oXMLElement, sPrefix, "")
                    sOldDataModel = sDataModel
                    oChildXMLElementDataModel = oChildXMLElement
                    bFirstElement = True
                End If
                If bFirstElement Then
                    sPrefix = "DS_" & sDatastructureName
                    oChildXMLElement = addChildElement(xmlDoc, oChildXMLElement, sPrefix, "")
                    oChildXMLElementSubGroup = oChildXMLElement
                    bFirstElement = False
                    sOldSubGroup = sSubGroup
                End If
                If sSubGroup <> sOldSubGroup Then
                    oChildXMLElement = oChildXMLElementDataModel
                    sPrefix = "DS_" & sDatastructureName
                    oChildXMLElement = addChildElement(xmlDoc, oChildXMLElement, sPrefix, "")
                    oChildXMLElementSubGroup = oChildXMLElement
                    sOldSubGroup = sSubGroup
                ElseIf sLoop1 <> sOldLoop1 And sLoop1 <> "" And sLoop2 = "" Then
                    oChildXMLElement = oChildXMLElementSubGroup
                    sPrefix = "DS_" & sDatastructureName
                    oChildXMLElement = addChildElement(xmlDoc, oChildXMLElement, sPrefix, "")
                    oChildXMLElementLoop1 = oChildXMLElement
                    sOldLoop1 = sLoop1
                ElseIf sLoop2 <> sOldLoop2 And sLoop2 <> "" And sLoop3 = "" Then
                    oChildXMLElement = oChildXMLElementLoop1
                    sPrefix = "DS_" & sDatastructureName
                    oChildXMLElement = addChildElement(xmlDoc, oChildXMLElement, sPrefix, "")
                    oChildXMLElementLoop2 = oChildXMLElement
                    sOldLoop2 = sLoop2
                ElseIf sLoop3 <> sOldLoop3 And sLoop3 <> "" Then
                    oChildXMLElement = oChildXMLElementLoop2
                    sPrefix = "DS_" & sDatastructureName
                    oChildXMLElement = addChildElement(xmlDoc, oChildXMLElement, sPrefix, "")
                    sOldLoop3 = sLoop3
                End If
                If sStandardWording <> "" Then
                    sPrefix = sDatastructureName
                    If sPrefix <> "" Then
                        addChildElement(xmlDoc, oChildXMLElement, sPrefix, "")
                    End If
                End If
            End If
        Next
        sOutPut = xmlDoc.OuterXml
    End Sub

    ''' <summary>
    ''' Add child node in XML
    ''' </summary>
    ''' <param name="odoc"></param>
    ''' <param name="oElement"></param>
    ''' <param name="sFieldName"></param>
    ''' <param name="sValue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function addChildElement(ByVal odoc As XmlDocument, ByVal oElement As XmlElement, ByVal sFieldName As String, ByVal sValue As String) As XmlElement

        sValue = sValue.Trim()
        ' Create a new element and add it to the document.
        Dim elem As XmlElement = odoc.CreateElement("", sFieldName, "urn:backbone:Data_Backbone")
        elem.InnerText = sValue
        oElement.AppendChild(elem)
        Return elem
    End Function

    ''' <summary>
    ''' Remove special char from string
    ''' </summary>
    ''' <param name="sValue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ReplaceSpecialCharacter(ByVal sValue As String) As String
        Dim arrSpecialChar() As String = {".", ",", "<", ">", ":", "?", """", "/", "{", "[", "}", "]", "`", "~", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "_", "-", "+", "=", "|", " ", "\"}
        Dim i As Integer
        For i = 0 To arrSpecialChar.Length - 1
            sValue = sValue.Replace(arrSpecialChar(i), "")
        Next
        Return sValue
    End Function

    ''' <summary>
    ''' Get details from wp_Fields table
    ''' </summary>
    ''' <param name="oResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetWPFieldsForDataModels(ByRef oResultSet As DataTable) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue

        Dim sSQLQuery As String = ACGetWPFieldsForDatamodelsSQL
        oResultSet = New DataTable

        m_oDatabase.Parameters.Clear()

        m_oDatabase.ExecuteDataTable(sSQL:=sSQLQuery, sSQLName:=ACGetWPFieldsForDatamodelsName, bStoredProcedure:=ACGetWPFieldsForDatamodelsStored, oRecordset:=oResultSet)
        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetWPFieldsForDatamodels SP Failed")
        End If

        Return nResult
    End Function

    ''' <summary>
    ''' Get table name from datastructure_name in wp_fields
    ''' </summary>
    ''' <param name="dtDataDetails"></param>
    ''' <param name="r_dtDataDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetTableNameFromWPFields(ByVal dtDataDetails As DataTable, ByRef r_dtDataDetails As DataTable) As Integer
        Dim nResult As Integer
        m_oDatabase.Parameters.Clear()

        Using cmd As New SqlCommand(ACGetWPTableNameSQL)

            cmd.Parameters.AddWithValue("@CCMWPFields", dtDataDetails)
            cmd.CommandType = CommandType.StoredProcedure

            'Execute SQL Statement
            nResult = m_oDatabase.ExecuteDataTable(cmd, r_dtDataDetails)
            If nResult <> PMEReturnCode.PMTrue Then
                RaiseError(ACClass, "GetTableName Failed", PMELogLevel.PMLogError)
                Return nResult
            End If
        End Using

        Return nResult
    End Function

    ''' <summary>
    ''' Get comma separated DocumentFieldsetFieldList 
    ''' </summary>
    ''' <param name="sCCMDocumentName"></param>
    ''' <param name="r_sDocumentFieldsetFieldList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDocumentFieldsetFieldList(ByVal sCCMDocumentName As String, ByRef r_sDocumentFieldsetFieldList As String) As Integer
        Dim nResult As Integer
        Dim oResultArray(,) As Object = Nothing

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        m_oDatabase.Parameters.Add(sName:="CCMDocumentName", vValue:=sCCMDocumentName, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)

        nResult = m_oDatabase.SQLSelect(sSQL:=ACGetDocumentFieldsetFieldListSQL, sSQLName:=ACGetDocumentFieldsetFieldListName, bStoredProcedure:=ACGetDocumentFieldsetFieldListStored,
                                        vResultArray:=oResultArray)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetDocumentFieldsetFieldList SP Failed")
        End If

        If Informations.IsArray(oResultArray) Then
            r_sDocumentFieldsetFieldList = oResultArray(0, 0)
        End If

        Return nResult
    End Function

    ''' <summary>
    ''' Update DocumentFieldsetFieldList in document_template
    ''' </summary>
    ''' <param name="sCCMDocumentName"></param>
    ''' <param name="sDocumentFieldsetFieldList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateDocumentFieldsetFieldList(ByVal sCCMDocumentName As String, ByVal sDocumentFieldsetFieldList As String) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        m_oDatabase.Parameters.Add(sName:="CCMDocumentName", vValue:=sCCMDocumentName, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
        m_oDatabase.Parameters.Add(sName:="DocumentFieldsetFieldList", vValue:=sDocumentFieldsetFieldList, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)

        nResult = m_oDatabase.SQLAction(sSQL:=ACUpdateDocumentFieldsetFieldListSQL, sSQLName:=ACUpdateDocumentFieldsetFieldListName, bStoredProcedure:=ACUpdateDocumentFieldsetFieldListStored)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("sDocumentFieldsetFieldList SP Failed")
        End If

        Return nResult
    End Function

    ''' <summary>
    ''' Check if template is already mapped and should be added to ddl or not
    ''' </summary>
    ''' <param name="sDocumentTemplate"></param>
    ''' <param name="sDocumentTemplateID"></param>
    ''' <param name="oTemplateArray"></param>
    ''' <param name="r_bIsAlreadyMappedToCCM"></param>
    ''' <remarks></remarks>
    Public Sub CheckIfAlreadyMappedToCCM(ByVal sDocumentTemplate As String, ByVal sDocumentTemplateID As String, ByVal oTemplateArray(,) As Object,
                                              ByRef r_bIsAlreadyMappedToCCM As Boolean)

        If oTemplateArray IsNot Nothing Then
            For i As Integer = oTemplateArray.GetLowerBound(1) To oTemplateArray.GetUpperBound(1)
                If oTemplateArray(0, i) = sDocumentTemplate Then ''if document template is already mapped in DB
                    r_bIsAlreadyMappedToCCM = True
                End If

                If oTemplateArray(1, i) = sDocumentTemplateID AndAlso oTemplateArray(0, i) = sDocumentTemplate Then
                    r_bIsAlreadyMappedToCCM = False
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Get CCM Template and Document_template_id from DB
    ''' </summary>
    ''' <param name="r_oTemplateArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCCMTemplatesFromDB(ByRef r_oTemplateArray(,) As Object) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        nResult = m_oDatabase.SQLSelect(sSQL:=ACGetCCMTemplatesFromDBSQL, sSQLName:=ACGetCCMTemplatesFromDBName, bStoredProcedure:=ACGetCCMTemplatesFromDBStored,
                                        vResultArray:=r_oTemplateArray)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetCCMTemplatesFromDB SP Failed")
        End If

        Return nResult
    End Function

    ''' <summary>
    ''' Refresh CCM templates
    ''' </summary>
    ''' <param name="bRefreshAll"></param>
    ''' <param name="sDocumentTemplateID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RefreshCCMTemplates(ByVal bRefreshAll As Boolean, ByVal sDocumentTemplateID As String)
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim oDataModelArray(,) As Object = Nothing

        If bRefreshAll Then
            ''recreate input XML and databackbone 
            nResult = GetDataModelCode(oDataModelCodeArray:=oDataModelArray)
            If nResult <> PMEReturnCode.PMTrue Then
                Throw New ApplicationException("GetDataModelCode Failed")
            End If
            For lTemp1 As Integer = oDataModelArray.GetLowerBound(1) To oDataModelArray.GetUpperBound(1)
                If lTemp1 = 0 Then
                    CCMRecreateDataSets(oDataModelArray(0, lTemp1), True)
                Else
                    CCMRecreateDataSets(oDataModelArray(0, lTemp1), False)
                End If
            Next
        End If

        ''refresh document_template table
        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        m_oDatabase.Parameters.Add(sName:="RefreshAll", vValue:=bRefreshAll, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMBoolean)
        m_oDatabase.Parameters.Add(sName:="DocumentTemplateID", vValue:=ToSafeInteger(sDocumentTemplateID), iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)

        nResult = m_oDatabase.SQLAction(sSQL:=ACRefreshCCMTemplatesSQL, sSQLName:=ACRefreshCCMTemplatesName, bStoredProcedure:=ACRefreshCCMTemplatesStored)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("RefreshCCMTemplates SP Failed")
        End If

        Return nResult
    End Function

    ''' <summary>
    ''' Get distinct loop1 values from DB where loop1 is not null
    ''' </summary>
    ''' <param name="r_oLoop1Array"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetDistinctLoop1Values(ByRef r_oLoop1Array(,) As Object) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        nResult = m_oDatabase.SQLSelect(sSQL:=ACGetDistinctLoop1ValuesSQL, sSQLName:=ACGetDistinctLoop1ValuesName, bStoredProcedure:=ACGetDistinctLoop1ValuesStored,
                                        vResultArray:=r_oLoop1Array)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetDistinctLoop1Values SP Failed")
        End If

        Return nResult
    End Function

    ''' <summary>
    ''' Get distinct loop2 values for loop1 where loop1,loop2 are not null
    ''' </summary>
    ''' <param name="sLoop1"></param>
    ''' <param name="r_oLoop2Array"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetDistinctLoop2Values(ByVal sLoop1 As String, ByRef r_oLoop2Array(,) As Object) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        m_oDatabase.Parameters.Add(sName:="Loop1", vValue:=sLoop1, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)

        nResult = m_oDatabase.SQLSelect(sSQL:=ACGetDistinctLoop2ValuesSQL, sSQLName:=ACGetDistinctLoop2ValuesName, bStoredProcedure:=ACGetDistinctLoop2ValuesStored,
                                        vResultArray:=r_oLoop2Array)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetDistinctLoop2Values SP Failed")
        End If

        Return nResult
    End Function

    ''' <summary>
    ''' Get distinct loop3 values for loop1, loop2 where loop1,loop2,loop3 are not null
    ''' </summary>
    ''' <param name="sLoop1"></param>
    ''' <param name="sLoop2"></param>
    ''' <param name="r_oLoop3Array"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetDistinctLoop3Values(ByVal sLoop1 As String, ByVal sLoop2 As String, ByRef r_oLoop3Array(,) As Object) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        m_oDatabase.Parameters.Add(sName:="Loop1", vValue:=sLoop1, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
        m_oDatabase.Parameters.Add(sName:="Loop2", vValue:=sLoop2, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)

        nResult = m_oDatabase.SQLSelect(sSQL:=ACGetDistinctLoop3ValuesSQL, sSQLName:=ACGetDistinctLoop3ValuesName, bStoredProcedure:=ACGetDistinctLoop3ValuesStored,
                                        vResultArray:=r_oLoop3Array)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetDistinctLoop3Values SP Failed")
        End If

        Return nResult
    End Function

    ''' <summary>
    ''' Set address and binding for CCM web service
    ''' </summary>
    ''' <param name="sCCMWebServiceURL"></param>
    ''' <param name="r_AzureWebService"></param>
    ''' <remarks></remarks>
    Private Overloads Sub SetServiceAddressAndBinding(ByVal sCCMWebServiceURL As String, ByRef r_AzureWebService As CCM_Azure.TPInTheCloudClient)
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim sSSL As String = String.Empty

        'need to know from system options if using HTTPS or HTTP, then create the connection as appropriate
        ' Get system option SSL
        nResult = GetSystemOption(nOptionNumber:=kSystemOptionCCMSSL, r_sSystemOptionValue:=sSSL)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetSystemOption Failed for SystemOptionCCMSSL")
        End If

        Dim bSSLValue As Boolean = If(sSSL = "1", True, False)

        Dim binding As BasicHttpBinding = New BasicHttpBinding()
        binding.MaxBufferPoolSize = Int32.MaxValue
        binding.MaxBufferSize = Int32.MaxValue
        binding.MaxReceivedMessageSize = Int32.MaxValue
        binding.ReaderQuotas.MaxStringContentLength = Int32.MaxValue
        binding.ReaderQuotas.MaxArrayLength = Int32.MaxValue

        If bSSLValue Then
            'HTTPS only if SSL is checked 
            binding.Security.Mode = BasicHttpSecurityMode.Transport
            r_AzureWebService.Endpoint.Address = New EndpointAddress(sCCMWebServiceURL)
        Else
            'HTTP or HTTPS if SSL is unchecked
            If sCCMWebServiceURL.StartsWith("https") Then
                binding.Security.Mode = BasicHttpSecurityMode.Transport
                r_AzureWebService.Endpoint.Address = New EndpointAddress(sCCMWebServiceURL)
            Else
                binding.Security.Mode = BasicHttpSecurityMode.None
                r_AzureWebService.Endpoint.Address = New EndpointAddress(sCCMWebServiceURL)
            End If
        End If

        r_AzureWebService.Endpoint.Binding = binding

    End Sub

    ''' <summary>
    ''' Overloaded method to use convert to pdf method
    ''' </summary>
    ''' <param name="sCCMWebServiceURL"></param>
    ''' <param name="r_AzureWebServicePDF"></param>
    ''' <remarks></remarks>
    Private Overloads Sub SetServiceAddressAndBinding(ByVal sCCMWebServiceURL As String, ByRef r_AzureWebServicePDF As CCM_AzureForPDF.TPInTheCloudClient)
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim sSSL As String = String.Empty

        'need to know from system options if using HTTPS or HTTP, then create the connection as appropriate
        ' Get system option SSL
        nResult = GetSystemOption(nOptionNumber:=kSystemOptionCCMSSL, r_sSystemOptionValue:=sSSL)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetSystemOption Failed for SystemOptionCCMSSL")
        End If

        Dim bSSLValue As Boolean = If(sSSL = "1", True, False)

        If bSSLValue Then
            'HTTPS only if SSL is checked 
            r_AzureWebServicePDF.Endpoint.Binding = New BasicHttpBinding(BasicHttpSecurityMode.Transport)
            r_AzureWebServicePDF.Endpoint.Address = New EndpointAddress(sCCMWebServiceURL)
        Else
            'HTTP or HTTPS if SSL is unchecked
            If sCCMWebServiceURL.StartsWith("https") Then
                r_AzureWebServicePDF.Endpoint.Binding = New BasicHttpBinding(BasicHttpSecurityMode.Transport)
                r_AzureWebServicePDF.Endpoint.Address = New EndpointAddress(sCCMWebServiceURL)
            Else
                r_AzureWebServicePDF.Endpoint.Binding = New BasicHttpBinding(BasicHttpSecurityMode.None)
                r_AzureWebServicePDF.Endpoint.Address = New EndpointAddress(sCCMWebServiceURL)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Update table name and datastructure name for SW columns
    ''' </summary>
    ''' <param name="sDataModelCode"></param>
    ''' <param name="sTableName"></param>
    ''' <param name="sDataStructureName"></param>
    ''' <param name="sColumnName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdateEndorsementDataStructureName(ByVal sDataModelCode As String, ByVal sTableName As String, ByVal sDataStructureName As String, ByVal sColumnName As String)
        Dim nResult As Integer = PMEReturnCode.PMTrue
        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        m_oDatabase.Parameters.Add(sName:="DataModelCode", vValue:=sDataModelCode, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
        m_oDatabase.Parameters.Add(sName:="Table_Name", vValue:=sTableName, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
        m_oDatabase.Parameters.Add(sName:="DataStructure_Name", vValue:=sDataStructureName, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
        m_oDatabase.Parameters.Add(sName:="Column_Name", vValue:=sColumnName, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)

        nResult = m_oDatabase.SQLAction(sSQL:=ACUpdateEndorsementDSNameSQL, sSQLName:=ACUpdateEndorsementDSName, bStoredProcedure:=ACUpdateEndorsementDSNameStored)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("UpdateEndorsementDataStructureName SP Failed")
        End If
        Return nResult
    End Function

    ''' <summary>
    ''' Get sub grp, loop1, loop2, loop3 details for SW
    ''' </summary>
    ''' <param name="sTableName"></param>
    ''' <param name="sColumnName"></param>
    ''' <param name="r_dtSubGrpDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSubGroupDetailsForEndorsement(ByVal sTableName As String, ByVal sColumnName As String, ByRef r_dtSubGrpDetails As DataTable) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim oSubGrpArray(,) As Object = Nothing

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        m_oDatabase.Parameters.Add(sName:="Table_Name", vValue:=sTableName, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
        m_oDatabase.Parameters.Add(sName:="Column_Name", vValue:=sColumnName, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
        nResult = m_oDatabase.ExecuteDataTable(sSQL:=ACGetSubGroupForEndorsementSQL, sSQLName:=ACGetSubGroupForEndorsementName, bStoredProcedure:=ACGetSubGroupForEndorsementStored,
                                               oRecordset:=r_dtSubGrpDetails)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetSubGroupDetailsForEndorsement SP Failed")
        End If

        Return nResult
    End Function

    ''' <summary>
    ''' Handle special core fields
    ''' </summary>
    ''' <param name="sTableName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function HandleSpecialCaseForCore(ByVal sTableName As String) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        m_oDatabase.Parameters.Add(sName:="Table_Name", vValue:=sTableName, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
        nResult = m_oDatabase.SQLAction(sSQL:=ACHandleSpecialCaseForCoreSQL, sSQLName:=ACHandleSpecialCaseForCoreName, bStoredProcedure:=ACHandleSpecialCaseForCoreStored)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("HandleSpecialCaseForCore SP Failed")
        End If

        Return nResult
    End Function

    Public Function ConvertWordToPDFV1(ByVal sSourceFile As String, ByVal sDestinationFile As String)
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim sCCMWebServiceURL As String = String.Empty
        Dim sCCMCustomer As String = String.Empty
        Dim sCCMPartner As String = String.Empty
        Dim sCCMContractTypeName As String = String.Empty
        Dim sCCMRepositoryProject As String = String.Empty
        Dim sCCMContractTypeVersion As String = String.Empty
        Dim sCCMStatus As String = String.Empty
        Dim sJobID As String = Guid.NewGuid().ToString().Replace("-", "")
        Dim azureWebServicePDF As CCM_AzureForPDF.TPInTheCloudClient
        Dim oPDFTemplatePackList() As Byte = Nothing
        Dim oDocTemplatePackList() As Byte = Nothing

        GetCCMSystemOptions(sCCMWebServiceURL, sCCMCustomer, sCCMPartner, sCCMContractTypeName, sCCMRepositoryProject, sCCMContractTypeVersion, sCCMStatus)
        sCCMContractTypeName = ConfigurationManager.AppSettings("CCMContractTypeName")

        oDocTemplatePackList = File.ReadAllBytes(sSourceFile)

        Try
            azureWebServicePDF = New CCM_AzureForPDF.TPInTheCloudClient
            Dim getDocTemplatesRequestInfoAzurePDF As New CCM_AzureForPDF.requestinfo()

            SetServiceAddressAndBinding(sCCMWebServiceURL, azureWebServicePDF)

            DirectCast(azureWebServicePDF.Endpoint.Binding, ServiceModel.BasicHttpBinding).MaxReceivedMessageSize = kMaxReceivedMessageSize
            DirectCast(azureWebServicePDF.Endpoint.Binding, ServiceModel.BasicHttpBinding).ReaderQuotas.MaxBytesPerRead = kMaxReceivedMessageSize
            DirectCast(azureWebServicePDF.Endpoint.Binding, ServiceModel.BasicHttpBinding).ReaderQuotas.MaxArrayLength = kMaxReceivedMessageSize
            DirectCast(azureWebServicePDF.Endpoint.Binding, ServiceModel.BasicHttpBinding).ReaderQuotas.MaxDepth = kMaxReceivedMessageSize
            DirectCast(azureWebServicePDF.Endpoint.Binding, ServiceModel.BasicHttpBinding).ReaderQuotas.MaxNameTableCharCount = kMaxReceivedMessageSize
            DirectCast(azureWebServicePDF.Endpoint.Binding, ServiceModel.BasicHttpBinding).ReaderQuotas.MaxStringContentLength = kMaxReceivedMessageSize

            getDocTemplatesRequestInfoAzurePDF = azureWebServicePDF.ConvertWordToPDFV1(sCCMPartner, sCCMCustomer, sCCMContractTypeName,
                                                                                    sCCMContractTypeVersion, sJobID, oDocTemplatePackList, oPDFTemplatePackList)
            File.WriteAllBytes(sDestinationFile, oPDFTemplatePackList)

        Catch excep As Exception
            nResult = PMEReturnCode.PMError
            ' Log Error Message
            Dim sErrorMessage As String = DirectCast(excep, System.ServiceModel.FaultException(Of bCCMDocumentProduction.CCM_Azure.AiaFaultV1)).Detail.message
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="ConvertWordToPDFV1 Failed", vApp:=ACApp,
                               vClass:=ACClass, vMethod:="ConvertWordToPDFV1")
        Finally
            azureWebServicePDF = Nothing
        End Try
        Return nResult
    End Function

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
