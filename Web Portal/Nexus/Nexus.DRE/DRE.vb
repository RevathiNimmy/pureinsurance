Imports System.Xml
Imports System.Xml.XPath
Imports System.Web.HttpContext

Public Class DRE
#Region "Class Variables"

    Public Const ClassId As String = "14AAF6F5-6CF1-41B8-AAEF-61053A272CC5"
    Public Const InterfaceId As String = "5E37670A-9C29-41B1-8238-0294B5FE35E1"
    Public Const EventsId As String = "B8728DDA-C273-43AF-80D5-6EE2B4A53827"


    'Standard class variables
    Private m_sUsername As String
    Private m_sPassword As String
    Private m_iUserID As Integer
    Private m_sCallingAppName As String
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_sGisDataModelCode As String
    Private m_iLogLevel As Integer

    ' Process Mode Properties
    Private m_iTask As Int16
    Private m_lNavigate As Long
    Private m_lProcessMode As Long
    Private m_sTransactionType As String
    Private m_dtEffectiveDate As Date

    Private dtCoverStartDate As Date
    Private dtCoverEndDate As Date

    Private m_oDREProxy As DREProxy.ExecutorService
    Private m_dSettings As DataTable

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
                Optional ByVal vDatabase As Object = Nothing) As Integer

        Try
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel
            m_sCallingAppName = sCallingAppName

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

    'this function must be called before calling NBQuote
    Public Function InitialiseEngine(
                ByVal v_sGisDataModelCode As String,
                ByVal v_sGisBusinessTypeCode As String) As Long
        Try
            m_oDREProxy = New DREProxy.ExecutorService
            m_sGisDataModelCode = v_sGisDataModelCode
        Catch ex As Exception
            General.LogErrorMessage(m_sUsername, DREConstants.PMELogLevel.PMLogError, ex.Message, "bGISQEMDRE", "DRE", "InitialiseEngine", , ex.StackTrace)
            Return DREConstants.PMEReturnCode.PMFalse
        End Try

        Return DREConstants.PMEReturnCode.PMTrue

    End Function
#End Region

    Public Function NBQuote(
                ByVal v_vSchemeArray As Object,
                ByVal v_lQuoteType As Integer,
                ByRef r_oDataset As SiriusFS.SAM.Client.DataSetControl.Application,
                ByVal v_dtEffectiveDate As Date, ByVal sProfileToken As String, ByVal sDREProxy As String,
                Optional ByVal v_sPREVersion As String = "DREORPRE1", Optional ByVal v_bPREUseChildRulesetEffDate As Boolean = False) As Integer

        Dim iReturn As Integer
        Dim iQuoteType As Integer
        Dim localProfileToken(0) As String
        Dim sDREAction As String = String.Empty
        Dim defXML As String = String.Empty
        Dim riskXML As String = String.Empty
        Dim sXMLDTD As String
        Dim responseXML As String
        Dim dXMLDocment As New XmlDocument
        Dim oAttribute As XmlAttribute

        Try
            localProfileToken(0) = sProfileToken
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

            Dim sDataSetDefinition As String = GetDataSetDefinition(Current.Session("DATA_MODEL_CODE"))

            iReturn = GetInsuranceDetails()
            If iReturn <> DREConstants.PMEReturnCode.PMTrue Then
                Throw New Exception("DRE.GetInsuranceDetails Cover start date and cover end date not set.")
            End If

            Dim defXMLDoc As New XmlDocument
            Dim riskXMLDoc As New XmlDocument
            Dim nodeList As XmlNodeList
            Dim dateNodeList As XmlNodeList
            Dim propName As String
            Dim objName As String
            Dim sXPath As String
            defXMLDoc.LoadXml(sDataSetDefinition)
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
                                    att.Value = Format(tempDate, My.MySettings.Default.RiskDataDateformat)
                                Else
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

            riskXML = riskXMLDoc.InnerXml
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
            If My.Settings.EnableTrace = True Then
                'General.LogErrorMessage(m_sUsername, DREConstants.PMELogLevel.PMLogError, m_sTransactionType, "bGISQEMDRE", "DRE", "NBQuote", "")
            End If
            oAttribute = dXMLDocment.ChildNodes(0).ChildNodes(0).ChildNodes(0).Attributes("EffectiveDate")
            If IsNothing(oAttribute) Then
                oAttribute = dXMLDocment.CreateAttribute("EffectiveDate")
                dXMLDocment.ChildNodes(0).Attributes.SetNamedItem(oAttribute)
            End If
            dXMLDocment.ChildNodes(0).Attributes("EffectiveDate").Value = v_dtEffectiveDate.ToString(My.MySettings.Default.Headerdateformat)

            oAttribute = dXMLDocment.ChildNodes(0).ChildNodes(0).ChildNodes(0).Attributes("CoverStartDate")
            If IsNothing(oAttribute) Then
                oAttribute = dXMLDocment.CreateAttribute("CoverStartDate")
                dXMLDocment.ChildNodes(0).Attributes.SetNamedItem(oAttribute)
            End If
            dXMLDocment.ChildNodes(0).Attributes("CoverStartDate").Value = dtCoverStartDate.ToString(My.MySettings.Default.Headerdateformat)

            oAttribute = dXMLDocment.ChildNodes(0).ChildNodes(0).ChildNodes(0).Attributes("CoverEndDate")
            If IsNothing(oAttribute) Then
                oAttribute = dXMLDocment.CreateAttribute("CoverEndDate")
                dXMLDocment.ChildNodes(0).Attributes.SetNamedItem(oAttribute)
            End If
            dXMLDocment.ChildNodes(0).Attributes("CoverEndDate").Value = dtCoverEndDate.ToString(My.MySettings.Default.Headerdateformat)

            oAttribute = dXMLDocment.ChildNodes(0).Attributes("Action")
            If IsNothing(oAttribute) Then
                oAttribute = dXMLDocment.CreateAttribute("Action")
                dXMLDocment.ChildNodes(0).Attributes.SetNamedItem(oAttribute)
            End If
            dXMLDocment.ChildNodes(0).Attributes("Action").Value = sDREAction

            riskXML = dXMLDocment.InnerXml

            Try
                Dim RulesetResp As New DREProxy.RuleSetReturn
                If My.MySettings.Default.EnableTrace = True Then
                    'General.LogErrorMessage(m_sUsername, DREConstants.PMELogLevel.PMLogError, riskXML, "bGISQEMDRE", "DRE", "NBQuote", "")
                End If
                m_oDREProxy.Url = sDREProxy
                If v_sPREVersion = "DREORPRE1" Then
                    RulesetResp = m_oDREProxy.ExecuteRuleSet(riskXML, v_dtEffectiveDate, localProfileToken, "", "", False)
                Else
                    RulesetResp = m_oDREProxy.ExecuteRuleSet(riskXML, v_dtEffectiveDate, localProfileToken, "", "", v_bPREUseChildRulesetEffDate)
                End If

                If RulesetResp.Output.Length > 0 Then
                    responseXML = RulesetResp.Output(0).Output
                    If My.MySettings.Default.EnableTrace = True Then
                        'General.LogErrorMessage(m_sUsername, DREConstants.PMELogLevel.PMLogError, responseXML, "bGISQEMDRE", "DRE", "NBQuote", "")
                    End If
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
                Else
                    'No rule set configure so return true without any change 
                    Return DREConstants.PMEReturnCode.PMTrue
                End If
            Catch ex As Exception
                'General.LogErrorMessage(m_sUsername, DREConstants.PMELogLevel.PMLogError, ex.Message, "bGISQEMDRE", "DRE", "NBQuote", ex.StackTrace)
                Return DREConstants.PMEReturnCode.PMFalse
            End Try

        Catch ex As Exception
            'General.LogErrorMessage(m_sUsername, DREConstants.PMELogLevel.PMLogError, ex.Message, "bGISQEMDRE", "DRE", "NBQuote", , ex.StackTrace)
            'General.LogErrorMessage(m_sUsername, DREConstants.PMELogLevel.PMLogError, "Risk xml before date conversion " + preRegexRiskXML, "bGISQEMDRE", "DRE", "NBQuote")
            Return DREConstants.PMEReturnCode.PMError
        End Try

        Return DREConstants.PMEReturnCode.PMTrue

    End Function

    ''' <summary>
    ''' The GetInsuranceDetails function set the cover start date and cover end date 
    ''' </summary>
    ''' <returns>True if cover start date and cover end date is set</returns>
    Public Function GetInsuranceDetails() As Integer

        Try
            Dim oQuote As NexusProvider.Quote = Current.Session("QUOTE")
            If Current.Session("MTA_TYPE") Is Nothing Then
                dtCoverStartDate = CDate(oQuote.CoverStartDate)
            Else
                dtCoverStartDate = CDate(oQuote.InceptionTPI)
            End If
            dtCoverEndDate = CDate(oQuote.CoverEndDate)


        Catch ex As Exception
            Return DREConstants.PMEReturnCode.PMFalse
        End Try

        Return DREConstants.PMEReturnCode.PMTrue

    End Function

    ''' <summary>
    ''' Retrieve the dataset definition for the provided DataModelCode
    ''' </summary>
    ''' <param name="v_sDataModelCode">DataModelCode of the dataset definition to be retrieved,
    ''' if no DataModelCode is provided an attempt wil be made to retrieve the DataModelCode
    ''' from the current dataset</param>
    ''' <returns>xml string representation of the dataset definition</returns>
    ''' <remarks></remarks>
    Public Function GetDataSetDefinition(Optional ByVal v_sDataModelCode As String = Nothing) As String
        Dim oQuote As NexusProvider.Quote = Current.Session("QUOTE")
        If v_sDataModelCode Is Nothing Then

            'Read DataModelCode from DataSet if it's not been passed

            Dim Doc As XPathDocument = New XPathDocument(New IO.StringReader(oQuote.Risks(0).XMLDataset))
            Dim Navigator As XPathNavigator
            Navigator = Doc.CreateNavigator()

            Dim i As XPathNodeIterator = Navigator.Select("DATA_SET")

            While (i.MoveNext)
                v_sDataModelCode = i.Current.GetAttribute("DataModelCode", String.Empty)
            End While

            Current.Session.Item("DATA_MODEL_CODE") = v_sDataModelCode

        End If

        Dim sDataSetDefinition As String = String.Empty

        'Load from file while the SAM method is broken
        ' Dim oNexusConfig As NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), NexusFrameWork)
        ' Dim oProductConfig As Product = oNexusConfig.Portals.Portal(GetPortalID()).Products.Product(oQuote.ProductCode)
        ' Dim sFolder As String = AppSettings("WebRoot") & oNexusConfig.ProductsFolder & "/" & oProductConfig.Name & "/"
        '---------------------------------------------
        Dim sBranchCode As String = oQuote.BranchCode
        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

        Try
            sDataSetDefinition = oWebService.GetDatasetDefinition(v_sDataModelCode, sBranchCode)
        Finally
            oWebService = Nothing
        End Try

        Return sDataSetDefinition

    End Function

    Public Shared Function ConvertXmlNodeListToDataTable(ByVal xnl As XmlNodeList) As DataTable

        Dim dt As New DataTable()

        Dim TempColumn As Integer = 0



        For Each node As XmlNode In xnl.Item(0).ChildNodes

            TempColumn += 1

            Dim dc As New DataColumn(node.Name, System.Type.[GetType]("System.String"))

            If dt.Columns.Contains(node.Name) Then

                dt.Columns.Add(InlineAssignHelper(dc.ColumnName, dc.ColumnName + TempColumn.ToString()))
            Else



                dt.Columns.Add(dc)

            End If
        Next

        Dim ColumnsCount As Integer = dt.Columns.Count
        For i As Integer = 0 To xnl.Count - 1

            Dim dr As DataRow = dt.NewRow()

            For j As Integer = 0 To ColumnsCount - 1


                dr(j) = xnl.Item(i).ChildNodes(j).InnerText
            Next


            dt.Rows.Add(dr)
        Next

        Return dt

    End Function

    Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, ByVal value As T) As T
        target = value
        Return value
    End Function
End Class

