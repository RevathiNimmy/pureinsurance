Imports System.Xml
Imports System.Text.RegularExpressions
Imports System.IO
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports Microsoft.Practices.EnterpriseLibrary.Common.Configuration
Imports Microsoft.Practices.EnterpriseLibrary.Caching.Expirations
Imports SharedFiles

<ComClass(DRE.ClassId, DRE.InterfaceId, DRE.EventsId)>
Public NotInheritable Class DRE
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness

#Region "Class Variables"

    Public Const ClassId As String = "14AAF6F5-6CF1-41B8-AAEF-61053A272CC5"
    Public Const InterfaceId As String = "5E37670A-9C29-41B1-8238-0294B5FE35E1"
    Public Const EventsId As String = "B8728DDA-C273-43AF-80D5-6EE2B4A53827"
    Protected Const DefaultDatasetKey As String = "DefaultDataset_"
    Public Shared iCache As ICacheManager

    'Standard class variables
    Private m_sUsername As String
    Private m_sPassword As String
    Private m_iUserID As Integer
    Private m_sCallingAppName As String
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_sGisDataModelCode As String
    Private m_sGisBusinessTypeCode As String
    Private m_iLogLevel As Integer
    Private m_lReturn As Integer

    ' Process Mode Properties
    Private m_iTask As Int16
    Private m_lNavigate As Long
    Private m_lProcessMode As Long
    Private m_sTransactionType As String
    Private m_dtEffectiveDate As Date

    Private dtCoverStartDate As Date
    Private dtCoverEndDate As Date
    Private DTPolicyCoverDate As DataTable

    Private m_oDREProxy As DREProxy.ExecutorService
    Private m_dSettings As DataTable

    Private m_oDatabase As dPMDAO.Database
    Private m_bCloseDatabase As Boolean
    Private m_sPREVersion As String
    Private m_bPREUseChildRulesetEffDate As Boolean

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

#End Region

#Region "Initialisation"
    Public Function Initialise(
                ByVal sUsername As String,
                ByVal sPassword As String,
                ByVal iUserID As Integer,
                ByVal iSourceID As Integer,
                ByVal iLanguageID As Integer,
                ByVal iCurrencyID As Integer,
                ByVal iLogLevel As Integer,
                ByVal sCallingAppName As String,
                Optional ByVal bStandalone As Boolean = False,
                Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise

        Try
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel
            m_sCallingAppName = sCallingAppName
            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch ex As Exception
            General.LogErrorMessage(m_sUsername, DREConstants.PMELogLevel.PMLogError, ex.Message, "bGISQEMDRE", "DRE", "Initialise", , ex.StackTrace)
            Return DREConstants.PMEReturnCode.PMFalse
        End Try

        Return DREConstants.PMEReturnCode.PMTrue

    End Function

    Public Function SetProcessModes(
                Optional ByVal vTask As Object = Nothing,
                Optional ByVal vNavigate As Object = Nothing,
                Optional ByVal vProcessMode As Object = Nothing,
                Optional ByVal vTransactionType As Object = Nothing,
                Optional ByVal vEffectiveDate As Object = Nothing) As Integer
        Try
            If Not vTask Is Nothing Then
                m_iTask = CType(vTask, Int16)
            End If
            If Not vNavigate Is Nothing Then
                m_lNavigate = CType(vNavigate, Integer)
            End If
            If Not vProcessMode Is Nothing Then
                m_lProcessMode = CType(vProcessMode, Integer)
            End If
            If Not vTransactionType Is Nothing Then
                m_sTransactionType = vTransactionType.ToString()
            End If
            If Not vEffectiveDate Is Nothing Then
                m_dtEffectiveDate = CType(vEffectiveDate, DateTime)
            End If
        Catch ex As Exception
            General.LogErrorMessage(m_sUsername, DREConstants.PMELogLevel.PMLogError, ex.Message, "bGISQEMDRE", "DRE", "SetProcessModes", , ex.StackTrace)
            Return DREConstants.PMEReturnCode.PMFalse
        End Try

        Return DREConstants.PMEReturnCode.PMTrue

    End Function

    Public Function InitialiseEngine(
                ByVal v_sGisDataModelCode As String,
                ByVal v_sGisBusinessTypeCode As String) As Long

        Dim result As Integer = 0

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            m_oDREProxy = New DREProxy.ExecutorService
            m_sGisDataModelCode = v_sGisDataModelCode
            m_sGisBusinessTypeCode = v_sGisBusinessTypeCode

            If m_oDatabase Is Nothing Then
                m_lReturn = CType(gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error Opening Database", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseEngine", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If
            End If

        Catch ex As Exception
            General.LogErrorMessage(m_sUsername, DREConstants.PMELogLevel.PMLogError, ex.Message, "bGISQEMDRE", "DRE", "InitialiseEngine", , ex.StackTrace)
            Return DREConstants.PMEReturnCode.PMFalse
        End Try

        Return DREConstants.PMEReturnCode.PMTrue

    End Function
#End Region

    Public Function NBQuote(
                ByVal v_vQEMDREAdditionalArray As Object,
                ByVal v_lQuoteType As Integer,
                ByRef r_oDataset As cGISDataSetControl.Application,
                ByVal v_dtEffectiveDate As Date,
                Optional ByRef r_vAdditionalDataArray As Object = Nothing,
                Optional ByVal v_bAfterPRETriggerRules As Boolean = False) As Integer

        Dim iReturn As Integer
        Dim iQuoteType As Integer
        Dim sProfileToken(0) As String
        Dim sDREAction As String = String.Empty
        Dim defXML As String = String.Empty
        Dim riskXML As String = String.Empty
        Dim sXMLDTD As String
        Dim responseXML As String
        Dim dXMLDocment As New XmlDocument
        Dim oAttribute As XmlAttribute

        Dim bRunPrePRERule As Boolean = False
        Dim bRunPostPRERule As Boolean = False
        Dim sPREAssemblyName As String = ""

        Const kMethodName As String = "NBQuote"

        Try
            iQuoteType = v_lQuoteType Mod 100
            Select Case iQuoteType
                Case DREConstants.PBCQemQuoteTypeQuote
                    sDREAction = DREConstants.PBCQemQuoteTypeQuoteActionName
                Case DREConstants.PBCQemQuoteTypeValidate
                    sDREAction = DREConstants.PBCQemQuoteTypeValidateActionName
                Case DREConstants.PBCQemQuoteTypeDefault
                    sDREAction = DREConstants.PBCQemQuoteTypeDefaultActionName
                Case DREConstants.PBCQemQuoteTypePreScreen
                    sDREAction = DREConstants.PBCQemQuoteTypePreScreenActionName
                    Return DREConstants.PMEReturnCode.PMTrue
                Case DREConstants.PBCQemQuoteTypeCopyRisk
                    sDREAction = DREConstants.PBCQemQuoteTypeCopyRiskActionName
                Case DREConstants.PBCQemQuoteTypeRenewal
                    sDREAction = DREConstants.PBCQemQuoteTypeRenewalActionName
            End Select

            iReturn = r_oDataset.ReturnAsXML(defXML, riskXML)
            If iReturn <> DREConstants.PMEReturnCode.PMTrue Then
                Throw New Exception("cGISDataSetControl.ReturnAsXML Failed")
            End If

            'New method of formatting the date properties' values in the dataset correctly so DRE will accept them
            Dim sKey As String = DefaultDatasetKey & m_sGisDataModelCode
            'first off check it's not in the cache as we can load once then cache for the rest "
            Try
                iCache = CacheFactory.GetCacheManager("PureCache")
            Catch ex As Exception

            End Try

            Dim sXMLDatasetDefinition As String = String.Empty

            If Not iCache Is Nothing AndAlso iCache.Contains(sKey) AndAlso Not String.IsNullOrEmpty(Convert.ToString(iCache.GetData(sKey))) Then
                sXMLDatasetDefinition = Convert.ToString(iCache.GetData(sKey))
            Else
                'it wasn't in the cache or there was some problem loading the cache so let's get the file and try to save to the cache
                Dim sFilePath As String
                Dim sDefaultDatasetsPath As String = String.Empty
                iReturn = GetPMRegSetting(v_lPMERegSettingRoot:=PMERegSettingRoot.pmeRSRLocalMachine,
                               v_lPMEProductFamily:=PMEProductFamily.pmePFSiriusSolutions,
                               v_lPMERegSettingLevel:=PMERegSettingLevel.pmeRSLServer,
                               v_sSettingName:="DataSetsPath",
                               r_sSettingValue:=sDefaultDatasetsPath,
                               v_sSubKey:="GIS")

                If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Right(sDefaultDatasetsPath, 1) <> "\" Then
                    sDefaultDatasetsPath += "\"
                End If

                'load the dsd for this datamodel
                Try
                    Dim sr As StreamReader = New StreamReader(path:=sDefaultDatasetsPath & m_sGisDataModelCode & "DSD.XML")
                    sXMLDatasetDefinition = sr.ReadToEnd
                    sr.Close()
                Catch
                    gPMFunctions.LogMessageToFile(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to read the DSD file for datamodel " & m_sGisDataModelCode,
                                                  vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName)
                End Try
                'get the cache path from the registry
                Dim m_sCachePath As String = String.Empty
                iReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine,
                                                       v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture,
                                                       v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer,
                                                       v_sSettingName:=gPMConstants.PMRegKeyCachePath, r_sSettingValue:=m_sCachePath)

                If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Right(m_sCachePath, 1) <> "\" Then
                    m_sCachePath += "\"
                End If

                Try
                    sFilePath = m_sCachePath + sKey + ".xml"

                    If Not FileExists(sFilePath) Then
                        Dim fileIO As FileStream
                        fileIO = File.Create(sFilePath)
                        fileIO.Close()
                        File.AppendAllText(sFilePath, sXMLDatasetDefinition)
                    End If

                    'we now need to add to the cache - a datamodel rebuild will need to clear the cache if it doesn't already
                    If Not iCache Is Nothing Then
                        iCache.Add(sKey, sXMLDatasetDefinition, CacheItemPriority.NotRemovable, Nothing, New FileDependency(sFilePath))
                    End If
                Catch ex As Exception
                    gPMFunctions.LogMessageToFile(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to cache the DSD file for datamodel " & m_sGisDataModelCode,
                                                  vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, excep:=ex)
                    iReturn = gPMConstants.PMEReturnCode.PMFalse
                    Return iReturn
                End Try
            End If

            'Richard Clarke May 2017 - now we need to know if we're running PREDre / PostDre rules.
            If Not Information.IsNothing(v_vQEMDREAdditionalArray) Then
                If Information.IsArray(v_vQEMDREAdditionalArray) Then
                    sPREAssemblyName = v_vQEMDREAdditionalArray(0, 0)
                    bRunPrePRERule = ToSafeBoolean(v_vQEMDREAdditionalArray(0, 1))
                    bRunPostPRERule = ToSafeBoolean(v_vQEMDREAdditionalArray(0, 2))
                End If
            End If

            If bRunPrePRERule Then
                Try
                    iReturn = RunPrePostPRERule(iQuoteType, r_oDataset, v_dtEffectiveDate, bRunPrePRERule, False, sPREAssemblyName, r_vAdditionalDataArray)
                Catch ex As Exception
                    Throw New Exception("Unable to run pre-PRE rules from bGISQEMDRE.")
                End Try
                If iReturn <> 1 Then
                    Throw New Exception("bGISQEMDRE RunPrePostPRERule Failed")
                End If
            End If

            iReturn = r_oDataset.ReturnAsXML(defXML, riskXML)
            If iReturn <> DREConstants.PMEReturnCode.PMTrue Then
                Throw New Exception("cGISDataSetControl.ReturnAsXML Failed")
            End If

            'now get the properties as per the xpath in Nexus.DRE
            Dim defXMLDoc As New XmlDocument
            Dim riskXMLDoc As New XmlDocument
            Dim nodeList As XmlNodeList
            Dim dateNodeList As XmlNodeList
            Dim propName As String
            Dim objName As String
            Dim sXPath As String
            defXMLDoc.LoadXml(sXMLDatasetDefinition)
            riskXMLDoc.LoadXml(riskXML) 'load the dataset into the temporary one we're going to update
            Dim sDateXPath As String
            sDateXPath = "//*[@DataType =" + "1" + "]"
            dateNodeList = defXMLDoc.SelectNodes(sDateXPath)
            Dim tempDate As DateTime
            If (Not dateNodeList Is Nothing) AndAlso dateNodeList.Count > 0 Then
                For Each node As XmlElement In dateNodeList
                    Dim objPropArray As String() = Split(node.Name, ".")
                    If objPropArray.Length > 0 Then
                        objName = objPropArray(0) 'Convert.ToString(dr("object_name")).ToUpper
                        propName = objPropArray(1) 'Convert.ToString(dr("property_name")).ToUpper
                        sXPath = "//" + objName + "/@" + propName

                        nodeList = riskXMLDoc.SelectNodes(sXPath)
                        If (Not nodeList Is Nothing) AndAlso nodeList.Count > 0 Then
                            'update the string to the correct date format
                            For Each att As XmlAttribute In nodeList
                                If DateTime.TryParse(att.Value, tempDate) Then
                                    If Not (att.Value.Contains("T") OrElse att.Value.Contains("Z")) Then
                                        'att.Value = tempDate.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fffffff'Z'")
                                        att.Value = CDate(tempDate).ToString(My.MySettings.Default.RiskDataDateformat)
                                    End If
                                Else
                                    gPMFunctions.LogMessageToFile(m_sUsername, DREConstants.PMELogLevel.PMLogError,
                                                                  "Error converting date in property " + propName + ", value is " + att.Value, ACApp, ACClass, kMethodName)
                                    Throw New Exception
                                End If
                            Next
                        End If
                    End If
                Next
            End If

            'Richard Clarke 27/10/2017 - we fixed the emtpy integer issue in PGR codeset but this never made it into Pure, it's reappeared so am fixing it once and for all
            '24, 23, 22, 21, 2 = integer or numbers based on looking at the WI5527 PGR code (not the same algorithm but data type consts should not change ever).
            Dim sIntXPath As String = String.Empty
            Dim numNodeList As XmlNodeList
            sIntXPath = "//*[@DataType=2 or @DataType=1 or @DataType=21 or @DataType=22 or @DataType=23 or @DataType=24 or @DataType=20" + "]"
            numNodeList = defXMLDoc.SelectNodes(sIntXPath)
            If (Not numNodeList Is Nothing) AndAlso (numNodeList.Count > 0) Then

                For Each node As XmlElement In numNodeList
                    Dim objPropArray As String() = Split(node.Name, ".")
                    If objPropArray.Length > 0 Then
                        objName = objPropArray(0) 'Convert.ToString(dr("object_name")).ToUpper
                        propName = objPropArray(1) 'Convert.ToString(dr("property_name")).ToUpper
                        sXPath = "//" + objName + "/@" + propName

                        nodeList = riskXMLDoc.SelectNodes(sXPath)
                        If (Not nodeList Is Nothing) AndAlso nodeList.Count > 0 Then
                            'update the string to the correct date format
                            For Each att As XmlAttribute In nodeList
                                If att.Value = "" AndAlso (Not Integer.TryParse(att.Value, 0)) Then
                                    att.Value = "0"
                                End If
                            Next
                        End If

                    End If
                Next

            End If

            riskXML = riskXMLDoc.InnerXml 'load the updated xml back into the original xml as this is the one we want to use
            riskXML = riskXML.Replace("<?xml version=""1.0"" encoding=""UTF-16"" standalone=""no""?>", "")
            dXMLDocment.LoadXml(riskXML)

            sXMLDTD = dXMLDocment.ChildNodes(0).OuterXml
            dXMLDocment.RemoveChild(dXMLDocment.ChildNodes(0))

            oAttribute = dXMLDocment.ChildNodes(0).Attributes("TransactionType")
            If IsNothing(oAttribute) Then
                oAttribute = dXMLDocment.CreateAttribute("TransactionType")
                dXMLDocment.ChildNodes(0).Attributes.SetNamedItem(oAttribute)
            End If

            dXMLDocment.ChildNodes(0).Attributes("TransactionType").Value = m_sTransactionType

            oAttribute = dXMLDocment.ChildNodes(0).ChildNodes(0).ChildNodes(0).Attributes("EffectiveDate")
            If IsNothing(oAttribute) Then
                oAttribute = dXMLDocment.CreateAttribute("EffectiveDate")
                dXMLDocment.ChildNodes(0).Attributes.SetNamedItem(oAttribute)
            End If
            dXMLDocment.ChildNodes(0).Attributes("EffectiveDate").Value = v_dtEffectiveDate.ToString(My.MySettings.Default.HeaderDateformat)

            ''addition of Cover Start Date and Cover end date which is expiry date  
            iReturn = GetPolicyCoverDetails(r_oDataset.PolicyLinkID)
            If iReturn = gPMConstants.PMEReturnCode.PMTrue Then
                If DTPolicyCoverDate.Rows.Count > 0 Then
                    dtCoverStartDate = CDate(DTPolicyCoverDate.Rows(0)("cover_start_date"))
                    dtCoverEndDate = CDate(DTPolicyCoverDate.Rows(0)("cover_end_date"))
                End If

                oAttribute = dXMLDocment.ChildNodes(0).ChildNodes(0).ChildNodes(0).Attributes("CoverStartDate")
                If IsNothing(oAttribute) Then
                    oAttribute = dXMLDocment.CreateAttribute("CoverStartDate")
                    dXMLDocment.ChildNodes(0).Attributes.SetNamedItem(oAttribute)
                End If
                dXMLDocment.ChildNodes(0).Attributes("CoverStartDate").Value = dtCoverStartDate.ToString(My.MySettings.Default.HeaderDateformat)

                oAttribute = dXMLDocment.ChildNodes(0).ChildNodes(0).ChildNodes(0).Attributes("CoverEndDate")
                If IsNothing(oAttribute) Then
                    oAttribute = dXMLDocment.CreateAttribute("CoverEndDate")
                    dXMLDocment.ChildNodes(0).Attributes.SetNamedItem(oAttribute)
                End If
                dXMLDocment.ChildNodes(0).Attributes("CoverEndDate").Value = dtCoverEndDate.ToString(My.MySettings.Default.HeaderDateformat)

            End If


            oAttribute = dXMLDocment.ChildNodes(0).Attributes("Action")
            If IsNothing(oAttribute) Then
                oAttribute = dXMLDocment.CreateAttribute("Action")
                dXMLDocment.ChildNodes(0).Attributes.SetNamedItem(oAttribute)
            End If
            dXMLDocment.ChildNodes(0).Attributes("Action").Value = sDREAction

            riskXML = dXMLDocment.InnerXml


            'Dim resolver = New XmlNamespaceManager(riskXMLDoc.NameTable)
            ''General.LogErrorMessage(m_sUsername, DREConstants.PMELogLevel.PMLogError, "risk xml: " + riskXML, "bGISQEMDRE", "DRE", "NBQuote")
            'resolver.AddNamespace("dre", "http://ssp-uk.com/DRE/")

            If ToSafeInteger(v_lQuoteType Mod 100) = DREConstants.PBCQemQuoteTypeRenewal Then
                iReturn = GetRatingInfo(r_oDataset.PolicyLinkID, v_dtEffectiveDate, iQuoteType, "RN")
            Else
                iReturn = GetRatingInfo(r_oDataset.PolicyLinkID, v_dtEffectiveDate, iQuoteType)
            End If
            If iReturn <> DREConstants.PMEReturnCode.PMTrue Then
                Throw New Exception("GetRatingInfo Failed")
            End If

            If m_dSettings.Rows.Count > 0 Then
                ReDim Preserve sProfileToken(0)
                sProfileToken(0) = m_dSettings.Rows(0)("dre_default_token").ToString
                m_oDREProxy.Url = m_dSettings.Rows(0)("dre_executor_url").ToString
                m_sPREVersion = m_dSettings.Rows(0)("pre_version").ToString()
                If String.IsNullOrEmpty(m_sPREVersion) Then
                    m_sPREVersion = "DREORPRE1"
                End If

                Try
                    Dim RulesetResp As New DREProxy.RuleSetReturn
                    If My.MySettings.Default.EnableTrace = True Then
                        General.LogErrorMessage(m_sUsername, DREConstants.PMELogLevel.PMLogError, riskXML, "bGISQEMDRE", "PRE", "NBQuote", "", "PRE Input XML")
                    End If

                    'We need to add the current time to the date to get PRE to use the most recently effective ruleset
                    v_dtEffectiveDate = New Date(v_dtEffectiveDate.Year, v_dtEffectiveDate.Month, v_dtEffectiveDate.Day, Now.Hour, Now.Minute, Now.Second, 0)
                    If m_sPREVersion = "DREORPRE1" Then
                        RulesetResp = m_oDREProxy.ExecuteRuleSet(riskXML, v_dtEffectiveDate, sProfileToken, "", "", False)
                    Else
                        m_bPREUseChildRulesetEffDate = IIf(m_dSettings.Rows(0)("pre_child_ruleset_effectivedate").ToString() = "1", True, False)
                        RulesetResp = m_oDREProxy.ExecuteRuleSet(riskXML, v_dtEffectiveDate, sProfileToken, "", "", m_bPREUseChildRulesetEffDate)
                    End If

                    If RulesetResp.Output.Length > 0 Then
                        responseXML = RulesetResp.Output(0).Output
                        If My.MySettings.Default.EnableTrace = True Then
                            General.LogErrorMessage(m_sUsername, DREConstants.PMELogLevel.PMLogError, responseXML, "bGISQEMDRE", "PRE", "NBQuote", "", "PRE Output XML")
                        End If

                        Dim NewdefXMLDoc As New XmlDocument
                        Dim resXMLDoc As New XmlDocument
                        NewdefXMLDoc.LoadXml(sXMLDatasetDefinition)
                        resXMLDoc.LoadXml(responseXML)

                        sDateXPath = "//*[@DataType =" + "1" + "]"
                        dateNodeList = defXMLDoc.SelectNodes(sDateXPath)
                        Dim newtempDate As DateTime
                        If (Not dateNodeList Is Nothing) AndAlso dateNodeList.Count > 0 Then
                            For Each node As XmlElement In dateNodeList
                                Dim objPropArray As String() = Split(node.Name, ".")
                                If objPropArray.Length > 0 Then
                                    objName = objPropArray(0)
                                    propName = objPropArray(1)
                                    sXPath = "//" + objName + "/@" + propName
                                    nodeList = resXMLDoc.SelectNodes(sXPath)
                                    If (Not nodeList Is Nothing) AndAlso nodeList.Count > 0 Then
                                        For Each att As XmlAttribute In nodeList
                                            If DateTime.TryParse(att.Value, newtempDate) Then
                                                If (att.Value.Contains("T") OrElse att.Value.Contains("Z")) Then
                                                    att.Value = att.Value.ToString().Replace("T", " ")
                                                    att.Value = att.Value.ToString().Replace("Z", "")
                                                End If
                                            Else
                                                gPMFunctions.LogMessageToFile(m_sUsername, DREConstants.PMELogLevel.PMLogError,
                                                                  "Error converting date in property " + propName + ", value is " + att.Value, ACApp, ACClass, kMethodName)
                                                Throw New Exception
                                            End If
                                        Next
                                    End If
                                End If
                            Next
                        End If
                        responseXML = resXMLDoc.InnerXml
                        responseXML = responseXML.Replace("<?xml version=""1.0"" encoding=""utf-8""?>", "")
                        Dim strTemp As String

                        strTemp = "<?xml version=""1.0"" encoding=""UTF-16"" standalone=""no""?>" & vbCrLf
                        strTemp = strTemp & sXMLDTD & vbCrLf
                        strTemp = strTemp & responseXML

                        responseXML = strTemp

                        iReturn = r_oDataset.LoadFromXML(defXML, responseXML)
                        If iReturn <> 1 Then
                            Throw New Exception("cGISDataSetControl.LoadFromXML Failed")
                        End If

                        If bRunPostPRERule Then
                            Try
                                iReturn = RunPrePostPRERule(iQuoteType, r_oDataset, v_dtEffectiveDate, False, bRunPostPRERule, sPREAssemblyName, r_vAdditionalDataArray)
                            Catch ex As Exception
                                Throw New Exception("Unable to run post-PRE rules from bGISQEMDRE.")
                            End Try
                            If iReturn <> 1 Then
                                Throw New Exception("bGISQEMDRE RunPrePostPRERule Failed")
                            End If
                            If iReturn = 1 Then
                                Return DREConstants.PMEReturnCode.PMTrue
                            End If

                        End If

                    Else
                        'No rule set configure so return true without any change 
                        Return DREConstants.PMEReturnCode.PMTrue
                    End If
                Catch ex As Exception
                    Return DREConstants.PMEReturnCode.PMPREFailed
                End Try
            Else
                Throw New Exception("Function GetRatingInfo returns no rows")
            End If

        Catch ex As Exception
            Return DREConstants.PMEReturnCode.PMError
        End Try

        Return DREConstants.PMEReturnCode.PMTrue

    End Function

    ''' <summary>
    ''' The GetPolicyCoverDetails function gets the cover start date and cover end date in DataTable
    ''' </summary>
    ''' <param name="nGISPolicyLinkId">Integer</param>
    ''' <returns>True if DataTable has records</returns>
    Private Function GetPolicyCoverDetails(ByVal nGISPolicyLinkId As Integer) As Integer
        Try
            Using con As Sirius.Architecture.Data.SiriusConnection = Sirius.Architecture.Data.SiriusConnection.FromSirius
                Using cmd As Sirius.Architecture.Data.SiriusCommand = Sirius.Architecture.Data.SiriusCommand.FromProcedure("spu_GIS_PolicyDetails_PRE")
                    cmd.AddInParameter("@nGis_policy_link_id", SqlDbType.Int).Value = nGISPolicyLinkId
                    DTPolicyCoverDate = con.ExecuteDataTable(cmd)
                End Using
            End Using
            If DTPolicyCoverDate Is Nothing Then
                Return DREConstants.PMEReturnCode.PMFalse
            End If
        Catch ex As Exception
            General.LogErrorMessage(m_sUsername, DREConstants.PMELogLevel.PMLogError, ex.Message, "bGISQEMDRE", "DRE", "GetPolicyCoverDetails", , ex.StackTrace)
            Return DREConstants.PMEReturnCode.PMFalse
        End Try

        Return DREConstants.PMEReturnCode.PMTrue

    End Function

    Private Function GetRatingInfo(
                ByVal v_lGISPolicyLinkId As Integer,
                ByVal v_dEffectiveDate As DateTime,
                ByVal v_lQuoteType As Integer,
                Optional ByVal v_sTransType As String = "RT") As Integer

        If m_dSettings Is Nothing Then
            m_dSettings = New DataTable()
        End If

        Using cmd As Sirius.Architecture.Data.SiriusCommand = Sirius.Architecture.Data.SiriusCommand.FromProcedure("spu_get_rule_file_name")
            cmd.AddInParameter("@gis_policy_link_id", SqlDbType.Int).Value = v_lGISPolicyLinkId
            cmd.AddInParameter("@effective_date", SqlDbType.DateTime).Value = v_dEffectiveDate
            cmd.AddInParameter("@quote_type", SqlDbType.Int).Value = v_lQuoteType
            cmd.AddInParameter("@type", SqlDbType.VarChar).Value = v_sTransType
            m_lReturn = m_oDatabase.ExecuteDataTable(cmd, m_dSettings)
        End Using

        Return m_lReturn

    End Function

    Private Function DREDateFormat(ByVal match As Match) As String

        Dim tempStr As String = match.Value

        Return CDate(tempStr).ToString(My.MySettings.Default.RiskDataDateformat)

    End Function


    ''' <summary>
    ''' New function to call bGISQEMCompiled method for new pre/post PRE compiled rules.
    ''' </summary>
    ''' <param name="nQuoteType"></param>
    ''' <param name="oDataset"></param>
    ''' <param name="dtEffectiveDate"></param>
    ''' <param name="bPrePRERule"></param>
    ''' <param name="bPostPRERule"></param>
    ''' <param name="sAssemblyName"></param>
    ''' <param name="oAdditionalDataArray"></param>
    ''' <returns></returns>
    Private Function RunPrePostPRERule(ByVal nQuoteType As Integer,
                ByRef oDataset As cGISDataSetControl.Application,
                ByVal dtEffectiveDate As Date,
                ByVal bPrePRERule As Boolean,
                ByVal bPostPRERule As Boolean,
                ByVal sAssemblyName As String,
                ByRef oAdditionalDataArray As Object)


        Dim bGISQEMCompiled As New bGISQEMCOMPILED.NET()
        Dim nReturn As Integer
        Dim sFIleExtension As String = ""

        'create an instance of bGISQEMCompiled and call its PREProcessRules method
        Try
            If Not String.IsNullOrEmpty(sAssemblyName) Then
                If IsArray(sAssemblyName.Split(".")) AndAlso sAssemblyName.Split(".").Count > 1 Then
                    sFIleExtension = sAssemblyName.Split(".")(1).Trim().ToUpper()
                End If

            End If


            Try
                If sFIleExtension <> "RUL" Then
                    nReturn = bGISQEMCompiled.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, ACApp, )

                    If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return nReturn
                    End If
                    nReturn = bGISQEMCompiled.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=dtEffectiveDate)

                    If Information.Err().Number = 438 Then
                        Information.Err().Clear()
                    End If

                    If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return nReturn
                    End If

                    nReturn = bGISQEMCompiled.InitialiseEngine(m_sGisDataModelCode, m_sGisBusinessTypeCode)

                    If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return nReturn
                    End If

                    nReturn = bGISQEMCompiled.PREProcessRules(sAssemblyClassName:=sAssemblyName, nQuoteType:=nQuoteType, oDataset:=oDataset, dtEffectiveDate:=dtEffectiveDate,
                                                          oAdditionalDataArray:=oAdditionalDataArray, bIsBackdatedMTA:=False, bPrePRE:=bPrePRERule, bPostPRE:=bPostPRERule)

                    If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return nReturn
                    End If
                Else
                    nReturn = gPMConstants.PMEReturnCode.PMTrue
                End If

            Catch
                Throw New Exception("Unable to run PRE compiled rules")
                bGISQEMCompiled.Dispose()
                bGISQEMCompiled = Nothing
            End Try
        Catch
            Throw New Exception("Unable to create PRE compiled rules assembly")
        Finally
            bGISQEMCompiled.Dispose()
            bGISQEMCompiled = Nothing
        End Try

        Return nReturn

    End Function

#Region "IDisposable Support"

    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

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
#End Region

End Class


